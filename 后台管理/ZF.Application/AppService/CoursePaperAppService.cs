using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// 
    /// </summary>
    public class CoursePaperAppService : BaseAppService<CoursePaper>
    {
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly ICoursePaperRepository _repository;

        private readonly IPaperGroupRepository _paperGroupRepository;

        private readonly ICourseInfoRepository _courseInfoRepository;


        private readonly PaperInfoAppService _paperInfoRepository;

        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="paperInfoRepository"></param>
        public CoursePaperAppService(ICoursePaperRepository repository, OperatorLogAppService operatorLogAppService, PaperInfoAppService paperInfoRepository, IPaperGroupRepository paperGroupRepository, ICourseInfoRepository courseInfoRepository) : base(repository)
        {
            _operatorLogAppService = operatorLogAppService;
            _paperInfoRepository = paperInfoRepository;
            _paperGroupRepository = paperGroupRepository;
            _courseInfoRepository = courseInfoRepository;
            _repository = repository;
        }

        /// <summary>
        /// 编辑或新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit(CoursePaperInput input)
        {
            var ids = input.PaperInfoId.Split(',');
            var index =
                Db.QueryFirstOrDefault<int>(
                    $@"select Isnull(OrderNo,0) from t_Course_Paper where CourseId='{input.CourseId}'", null);
            foreach (var item in ids)
            {
                index++;
                var paperInfo = _paperGroupRepository.Get(item);
                var model = input.MapTo<CoursePaper>();
                model.Id = Guid.NewGuid().ToString();
                model.CourseId = input.CourseId;
                model.PaperInfoId = paperInfo.Id;
                model.PaperInfoName = paperInfo.PaperGroupName;
                model.OrderNo = index;
                _repository.Insert(model);
            }
            return new MessagesOutPut { Success = true, Message = "保存成功!" };
        }


        /// <summary>
        /// 获取课程试题列表
        /// </summary>
        /// <returns></returns>
        public List<CoursePaperOutput> GetList(CoursePaperListInput input, out int count)
        {

            var strSql = "SELECT  a.*,b.CourseName,ISNULL(c.type,0) Type FROM dbo.t_Course_Paper a LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id LEFT JOIN dbo.t_Paper_Group c ON a.PaperInfoId=c.Id where 1=1   and a.CourseId=@CourseId ";
            var strcount = "select count(*) from t_Course_Paper  where 1=1 and CourseId=@CourseId ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", input.CourseId, DbType.String);

            var list = Db.QueryList<CoursePaperOutput>(GetPageSql(strSql, dy, input.Page, input.Rows, input.Sidx, input.Sord), dy);
            count = Db.ExecuteScalar<int>(strcount, dy);
            return list;
        }


        /// <summary>
        /// 获取推荐书籍
        /// </summary>
        /// <returns></returns>
        public List<SubjectBookOutput> GetSubjectBook(string courseId)
        {
            var strSql = @"SELECT  a.*,'" + DefuleDomain + "/'" + @"+ a.ImageUrl as ImageUrl
FROM t_base_SubjectBook a
WHERE   a.SubjectId = (SELECT  SubjectId
                    FROM    dbo.t_Course_Info
                    WHERE   Id = @CourseId
                      )
        AND a.IsDelete = 0
ORDER BY a.OrderNo DESC ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            var list = Db.QueryList<SubjectBookOutput>(strSql, dy);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SubjectBookOutput> GetBooks(CourseInfoListInput input, out int count)
        {
            input.Sidx = "OrderNo";
            input.Sord = "Asc";
            var strSql = @"SELECT   a.BookName,a.OrderNo,a.Url,a.Id,'" + DefuleDomain + "/'" + @"+ a.ImageUrl as ImageUrl ";
            var sqlfrom = @" FROM t_base_SubjectBook a where a.IsDelete = 0 ";
            var sqlCount = " select count(*) ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                sqlfrom += " and a.SubjectId=@SubjectId ";
                dy.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + sqlfrom, dy);
            var list = Db.QueryList<SubjectBookOutput>(GetPageSql(strSql + sqlfrom, dy, input.Page, input.Rows, input.Sidx, input.Sord), dy);
            return list;
        }

        /// <summary>
        /// 获取课程试题列表
        /// </summary>
        /// <returns></returns>
        public List<CoursePaperOutput> GetList(string courseId)
        {
            var strSql = "SELECT  a.*,b.CourseName FROM dbo.t_Course_Paper a LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id where 1=1   and a.CourseId=@CourseId ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            var list = Db.QueryList<CoursePaperOutput>(strSql, dy);
            return list;
        }

        /// <summary>
        /// 获取课程试题列表
        /// </summary>
        /// <returns></returns>
        public List<CoursePaperOutput> GetList(string courseId, string userId)
        {
            var strSql = @"SELECT  MAX(a.Id) Id ,
        MAX(a.CourseId) CourseId ,
        MAX(a.PaperInfoId) PaperInfoId ,
        MAX(a.PaperInfoName) PaperInfoName ,
        MAX(b.CourseName) CourseName ,
        COUNT(c.Id) Count
FROM dbo.t_Course_Paper a
        LEFT JOIN dbo.t_Course_Info b ON a.CourseId = b.Id
        LEFT JOIN t_My_PaperRecords c ON a.PaperInfoId = c.PaperId  and c.UserId=@UserId  and c.Status=1
WHERE   1 = 1  and a.CourseId=@CourseId  
GROUP BY a.Id,a.OrderNo order by a.OrderNo ASC ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            dy.Add(":UserId", userId, DbType.String);
            var list = Db.QueryList<CoursePaperOutput>(strSql, dy);
            return list;
        }

        /// <summary>
        /// 根据不同类别获取试卷-已登录
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CoursePaperOutput> GetListByType(string courseId, string userId, int type)
        {
            var strSql = @"SELECT  MAX(a.Id) Id ,
        MAX(a.CourseId) CourseId ,
        MAX(a.PaperInfoId) PaperInfoId ,
        MAX(a.PaperInfoName) PaperInfoName ,
        MAX(b.CourseName) CourseName ,
        COUNT(c.Id) Count
FROM dbo.t_Course_Paper a
        LEFT JOIN dbo.t_Course_Info b ON a.CourseId = b.Id
        LEFT JOIN t_My_PaperRecords c ON a.PaperInfoId = c.PaperId  and c.UserId=@UserId  and c.Status=1
        LEFT JOIN dbo.t_Paper_Group d ON a.PaperInfoId=d.Id
WHERE   1 = 1  and a.CourseId=@CourseId  and ISNULL(d.type,0)=@Type
GROUP BY a.Id,a.OrderNo order by a.OrderNo ASC ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            dy.Add(":UserId", userId, DbType.String);
            dy.Add(":Type", type, DbType.Int16);
            var list = Db.QueryList<CoursePaperOutput>(strSql, dy);
            return list;
        }

        public List<CoursePaperOutput> GetListByType(string courseId, int type)
        {
            var strSql = "SELECT  a.*,b.CourseName FROM dbo.t_Course_Paper a LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id LEFT JOIN dbo.t_Paper_Group c ON a.PaperInfoId=c.Id where 1=1   and a.CourseId=@CourseId  and ISNULL(c.type,0)=@Type order by a.OrderNo ASC ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            dy.Add(":Type", type, DbType.Int16);
            var list = Db.QueryList<CoursePaperOutput>(strSql, dy);
            return list;
        }

        /// <summary>
        /// 获取课程试题 可添加试卷列表
        /// </summary>
        /// <returns></returns>
        public List<CoursePaperOutput> GetListAdd(CoursePaperListInput input, out int count)
        {
            var courseModel = _courseInfoRepository.Get(input.CourseId);

            var strSql = "SELECT  a.PaperInfoId FROM dbo.t_Course_Paper a  where 1=1  and a.CourseId=@CourseId ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", input.CourseId, DbType.String);
            var list = Db.QueryList<CoursePaperOutput>(strSql, dy);
            var paperInfoIdList = list.Aggregate(string.Empty, (current, item) => current + ("'" + item.PaperInfoId + "'" + ","));
            if (paperInfoIdList.Length > 0)
            {
                paperInfoIdList = paperInfoIdList.TrimEnd(',');
            }
            else
            {
                paperInfoIdList = "''";
            }
            var sql = $@" SELECT a.PaperGroupName  PaperInfoName,a.Id,a.Type FROM t_Paper_Group a  where 1=1 and State=1  and IsDelete=0  and Id not in ({
            paperInfoIdList}) and SubjectId='{courseModel.SubjectId}'  ";
            var list1 = Db.QueryList<CoursePaperOutput>(GetPageSql(sql, null, input.Page, input.Rows, "Type", input.Sord), null);
            var strcount = $@"select count(*) from t_Paper_Group  where 1=1  and State=1  and IsDelete=0 and Id not in ({paperInfoIdList}) and SubjectId='{courseModel.SubjectId}' ";
            count = Db.ExecuteScalar<int>(strcount, null);
            return list1;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var strIds = input.Ids.TrimEnd(',');
            var arr = strIds.Split(',');

            foreach (var item in arr)
            {
                var model = _repository.Get(item);
                if (model != null)
                {
                    _repository.Delete(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.BaseData,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除课程试题:" + model.PaperInfoName
                    });
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }
    }
}
