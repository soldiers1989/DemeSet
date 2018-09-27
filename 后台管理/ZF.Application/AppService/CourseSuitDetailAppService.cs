
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：CourseSuitDetail 
    /// </summary>
    public class CourseSuitDetailAppService : BaseAppService<CourseSuitDetail>
    {
        private readonly ICourseSuitDetailRepository _iCourseSuitDetailRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseSuitDetailRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public CourseSuitDetailAppService(ICourseSuitDetailRepository iCourseSuitDetailRepository, OperatorLogAppService operatorLogAppService) : base(iCourseSuitDetailRepository)
        {
            _iCourseSuitDetailRepository = iCourseSuitDetailRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseSuitDetail 
        /// </summary>
        public List<CourseInfoOutput> GetList(CourseSuitDetailListInput input, out int count)
        {
            const string sql = "select  isnull(a.OrderNo,0) OrderNo,a.Id,b.CourseName,b.SubjectId,b.CourseIamge,b.CourseContent,b.Price,b.FavourablePrice,b.ValidityPeriod,b.State,b.TeacherId,b.IsTop,b.IsRecommend,b.CourseTag,b.CourseLongTime,b.CourseWareCount,b.AddTime,c.SubjectName ";
            var strSql = new StringBuilder(" from t_Course_SuitDetail  a left join dbo.t_Course_Info b on a.CourseId=b.Id  LEFT JOIN dbo.t_Base_Subject c ON b.SubjectId=c.Id where 1 = 1 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.PackCourseId))
            {
                strSql.Append(" and a.PackCourseId = @PackCourseId ");
                dynamicParameters.Add(":PackCourseId", input.PackCourseId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseInfoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 获取制定套餐对应科目
        /// </summary>
        /// <param name="packCourseId"></param>
        /// <returns></returns>
        public string GetList(string packCourseId)
        {
            if (!string.IsNullOrEmpty(packCourseId))
            {
                var subjectIds = string.Empty;
                var strSql = "select MAX(c.Id)  from  t_Course_SuitDetail   a " +
                             " left JOIN  dbo.t_Course_Info  b ON a.CourseId=b.Id " +
                             " LEFT JOIN dbo.t_Base_Subject c ON b.SubjectId = c.Id where packCourseId='" + packCourseId + "' GROUP BY c.Id  ";
                var list = Db.QueryList<string>(strSql, null);
                return list.Aggregate(subjectIds, (current, item) => current + (item + ","));
            }
            return "";
        }


        /// <summary>
        /// 新增实体  CourseSuitDetail
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseSuitDetailInput input)
        {
            CourseSuitDetail model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseSuitDetailRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.PackCourseId = input.PackCourseId;
                model.CourseId = input.CourseId;
                model.OrderNo = input.OrderNo;
                #endregion
                _iCourseSuitDetailRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseSuitDetail,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改套餐明细:" + model.PackCourseId
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseSuitDetail>();
            model.PackCourseId = input.PackCourseId;
            model.CourseId = input.CourseId;
            model.Id = Guid.NewGuid().ToString();
            var sql = string.Format( "select count(*) from t_Course_SuitDetail where PackCourseId='{0}'", input.CourseId );
            model.OrderNo = Db.ExecuteScalar<int>( sql, null );
            var keyId = _iCourseSuitDetailRepository.InsertGetId(model);


            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseSuitDetail,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增套餐子课程:" + model.CourseId
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 套餐课程是否含有子课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsContainsSubCourse( CourseSuitDetailInput input )
        {
            var sql = " SELECT * FROM dbo.t_Course_SuitDetail WHERE PackCourseId=@PackCourseId ";
            var parameters = new DynamicParameters( );
            parameters.Add( ":PackCourseId",input.PackCourseId,DbType.String);
            var list = Db.QueryList<CourseSuitDetailOutput>( sql, parameters );
            return list.Count > 0;
        }

        /// <summary>
        /// 基础课程是否已维护到套餐课程中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ExistInCoursepack( CourseSuitDetailInput input )
        {
            const string sql = "SELECT * FROM dbo.t_Course_SuitDetail where CourseId=@CourseId";
            var paramters = new DynamicParameters( );
            paramters.Add( ":CourseId",input.CourseId,DbType.String);
            var list = Db.QueryList<CourseSuitDetailOutput>( sql, paramters );
            return list.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseSuitDetailOutput GetDetail( IdInput input )
        {
            string sql =string.Format( @"SELECT isnull(a.OrderNo,0) OrderNo,a.CourseId,a.Id,a.PackCourseId,b.CourseName AS PackCourseName,c.CourseName FROM dbo.t_Course_SuitDetail a
LEFT JOIN dbo.t_Course_PackcourseInfo b ON a.PackCourseId = b.Id
LEFT JOIN dbo.t_Course_Info c ON a.CourseId = c.Id where a.Id='{0}'", input.Id);
            return Db.QueryFirstOrDefault<CourseSuitDetailOutput>( sql, null );
        }
    }
}

