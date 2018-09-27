
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
    /// 数据表实体应用服务现实：CoursePackcourseInfo 
    /// </summary>
    public class CoursePackcourseInfoAppService : BaseAppService<CoursePackcourseInfo>
    {
        private readonly ICoursePackcourseInfoRepository _iCoursePackcourseInfoRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly FileRelationshipAppService _fileRelationshipAppService;

        private readonly ProjectAppService _projectAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCoursePackcourseInfoRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        /// <param name="projectAppService"></param>
        public CoursePackcourseInfoAppService(ICoursePackcourseInfoRepository iCoursePackcourseInfoRepository, FileRelationshipAppService fileRelationshipAppService, OperatorLogAppService operatorLogAppService, ProjectAppService projectAppService) : base(iCoursePackcourseInfoRepository)
        {
            _iCoursePackcourseInfoRepository = iCoursePackcourseInfoRepository;
            _operatorLogAppService = operatorLogAppService;
            _projectAppService = projectAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        /// 查询列表实体：CoursePackcourseInfo 
        /// </summary>
        public List<CoursePackcourseInfoOutput> GetList(CoursePackcourseInfoListInput input, out int count)
        {
            const string sql = @"SELECT DISTINCT
        a.Id ,
        a.CourseName ,
        a.SubjectIds ,
        a.Price ,
        a.FavourablePrice ,
        a.ValidityPeriod ,
        a.State ,
        d.CourseLongTime ,
        d.CourseWareCount ,
        a.AddTime ,
        a.OrderNo,
        ISNULL(f.LearnCount, 0) AS LearnCount ,
        ISNULL(e.CourseCount, 0) AS CourseCount ";
            var strSql = new StringBuilder(@" FROM    t_Course_PackcourseInfo a
        LEFT JOIN dbo.t_Course_SuitDetail b ON a.Id = b.PackCourseId
        LEFT JOIN dbo.t_Course_Info c ON b.CourseId = c.Id
        LEFT JOIN(SELECT  a.Id,
                            SUM(ISNULL(c.CourseWareCount, 0)) AS CourseWareCount,
                            SUM(ISNULL(c.CourseLongTime, 0)) AS CourseLongTime
                    FROM    dbo.t_Course_PackcourseInfo a
                            LEFT JOIN dbo.t_Course_SuitDetail b ON a.Id = b.PackCourseId
                            LEFT JOIN dbo.t_Course_Info c ON b.CourseId = c.Id
                    GROUP BY a.Id
                  ) d ON a.Id = d.Id
        LEFT JOIN(SELECT  COUNT(1) AS LearnCount,
                            CourseId
                    FROM    dbo.t_My_Course
                    GROUP BY CourseId
                  ) f ON a.Id = f.CourseId
        LEFT JOIN(SELECT  PackCourseId,
                            COUNT(*) AS CourseCount
                    FROM    dbo.t_Course_SuitDetail
                    GROUP BY PackCourseId
                  ) e ON a.Id = e.PackCourseId
WHERE   a.IsDelete = 0 " );
            const string sqlCount = "select  count(DISTINCT a.Id) count ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and a.CourseName like @CourseName ");
                dynamicParameters.Add(":CourseName", "%"+input.CourseName+"%", DbType.String);
            }
            if ( !string.IsNullOrWhiteSpace( input.TeacherId ) ) {
                strSql.Append(" and c.TeacherId=@TeacherId " );
                dynamicParameters.Add(":TeacherId",input.TeacherId,DbType.String);
            }
            if ( !string.IsNullOrWhiteSpace( input.State.ToString( ) ) ) {
                    strSql.Append( " and isnull(a.State,0)=@State " );
                dynamicParameters.Add( ":State",input.State,DbType.Int16);
            }

            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CoursePackcourseInfoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  CoursePackcourseInfo
        /// </summary>
        public MessagesOutPut AddOrEdit(CoursePackcourseInfoInput input)
        {
            CoursePackcourseInfo model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCoursePackcourseInfoRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.CourseName = input.CourseName;
                model.CourseIamge = input.IdFilehiddenFile;
                model.CourseVedio = input.CourseVediohiddenFile;
                model.CourseContent = input.CourseContent;
                model.FavourablePrice = input.FavourablePrice;
                model.ValidityPeriod = input.ValidityPeriod;
                model.IsTop = input.IsTop;
                model.IsRecommend = input.IsRecommend;
                model.Title = input.Title;
                model.KeyWord = input.KeyWord;
                model.Description = input.Description;
                model.ClassId = input.ClassId;
                model.CourseTag = input.CourseTag;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                model.EmailNotes = input.EmailNotes;
                model.OrderNo = input.OrderNo;
                model.ValidityEndDate = input.ValidityEndDate;
                #endregion
                _iCoursePackcourseInfoRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CoursePackcourseInfo,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改套餐课程:" + model.CourseName
                });
                if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
                {
                    _fileRelationshipAppService.Add(new FileRelationshipInput
                    {
                        ModuleId = input.Id,
                        IdFilehiddenFile = input.IdFilehiddenFile,
                        Type = 0
                    });
                }
                //2018-05-18 wyf  add   新增项目分类属性
                if (!string.IsNullOrEmpty(input.ClassId))
                {
                    var classId = input.ClassId.Split(',');
                    var projectClassId = classId.Select(item => _projectAppService.Get(item).ProjectClassId)
                        .Aggregate("", (current, id) => current + (id + ","));
                    model.ProjectClassId = projectClassId;
                }
                if ( !string.IsNullOrWhiteSpace( input.CourseVediohiddenFile ) )
                {
                    _fileRelationshipAppService.Add( new FileRelationshipInput
                    {
                        ModuleId = model.Id,
                        IdFilehiddenFile = input.CourseVediohiddenFile,
                        Type = 1//视频
                    } );
                }
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CoursePackcourseInfo>();
            model.Id = Guid.NewGuid().ToString();
            model.CourseName = input.CourseName;
            model.CourseIamge = input.IdFilehiddenFile;
            model.CourseContent = input.CourseContent;
            model.Price = input.Price;
            model.FavourablePrice = input.FavourablePrice;
            model.ValidityPeriod = input.ValidityPeriod;
            model.State = input.State;
            model.IsTop = model.IsTop;
            model.IsRecommend = input.IsRecommend;
            model.CourseTag = input.CourseTag;
            model.ClassId = input.ClassId;
            model.Title = input.Title;
            model.KeyWord = input.KeyWord;
            model.Description = input.Description;
            model.CourseLongTime = input.CourseLongTime;
            model.CourseWareCount = input.CourseWareCount;
            model.CourseVedio = input.CourseVediohiddenFile;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.OrderNo = input.OrderNo;
            model.ValidityEndDate = input.ValidityEndDate;
            //2018-05-18 wyf  add   新增项目分类属性
            if (!string.IsNullOrEmpty(input.ClassId))
            {
                var classId = input.ClassId.Split(',');
                var projectClassId = classId.Select(item => _projectAppService.Get(item).ProjectClassId)
                    .Aggregate("", (current, id) => current + (id + ","));
                model.ProjectClassId = projectClassId;
            }
            var keyId = _iCoursePackcourseInfoRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CoursePackcourseInfo,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增套餐课程:" + model.CourseName
            });
            if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
            {
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                    Type = 0
                });
            }
            if ( !string.IsNullOrWhiteSpace( input.CourseVediohiddenFile ) ) {
                _fileRelationshipAppService.Add( new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.CourseVediohiddenFile,
                    Type = 1//视频
                } );
            }
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        

        /// <summary>
        /// 判断是否在购物车或订单中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ExistInOrderOrCart( CoursePackcourseInfoInput input )
        {
            var sql = " SELECT  COUNT(*) FROM (SELECT CourseId FROM t_Order_Detail UNION SELECT CourseId FROM dbo.t_Order_CartDetail)a  WHERE a.CourseId='" + input.Id + "'";
            return Db.ExecuteScalar<int>( sql, null ) > 0;
        }


        /// <summary>
        /// 更新上下架状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateState( CoursePackcourseInfoInput input )
        {
            CoursePackcourseInfo model;
            model = _iCoursePackcourseInfoRepository.Get( input.Id );
            #region 修改逻辑
            model.Id = input.Id;
            model.State = input.State;
            model.UpdateUserId = UserObject.Id;
            model.UpdateTime = DateTime.Now;
            #endregion
            _iCoursePackcourseInfoRepository.Update( model );
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = model.Id,
                ModuleId = ( int )Model.CourseInfo,
                OperatorType = ( int )OperatorType.Edit,
                Remark = string.Format( "修改套餐课程上下架状态:{0}-{1}", model.Id, model.CourseName )
            } );
            return new MessagesOutPut { Success = true, Message = "修改成功!" };
        }
    }
}

