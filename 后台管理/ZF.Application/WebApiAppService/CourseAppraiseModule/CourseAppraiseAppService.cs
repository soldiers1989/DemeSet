using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.CourseAppraiseModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseAppraiseModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseAppraiseAppService : BaseAppService<CourseAppraise>
    {
        private readonly ICourseAppraiseRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CourseAppraiseAppService(ICourseAppraiseRepository repository) : base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 是否开启评价功能 0表示课程不能评论 1表示开放课程评论
        /// </summary>
        /// <returns></returns>
        public bool IsPj()
        {
            var str = " SELECT ArguValue FROM dbo.T_Base_SysSet where 1=1 and Isdelete=0 and ArguName='kcpl' ";
            var dynamicparameter = new DynamicParameters();
            var arguValue = Db.QueryFirstOrDefault<string>(str, dynamicparameter);
            if (arguValue == "0")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 添加课程评价
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Add(CourseAppraiseModelInput input)
        {
            var str = " SELECT ArguValue FROM dbo.T_Base_SysSet where 1=1 and Isdelete=0 and ArguName='kcpl' ";
            var dynamicparameter = new DynamicParameters();
            var arguValue = Db.QueryFirstOrDefault<string>(str, dynamicparameter);
            if (arguValue == "0")
            {
                return new MessagesOutPut { Success = false, Message = "系统已关闭评论功能!" };
            }
            CourseAppraise model;
            model = input.MapTo<CourseAppraise>();
            model.Id = Guid.NewGuid().ToString();
            model.CourseId = input.CourseId;
            model.UserId = input.UserId;
            model.AppraiseCotent = input.AppraiseCotent;
            model.AppraiseLevel = input.AppraiseLevel;
            model.AppraiseIp = input.AppraiseIp;
            model.AppraiseTime = DateTime.Now;
            model.CourseType = input.CourseType;
            _repository.InsertGetId(model);
            return new MessagesOutPut { Success = true, Message = "评价成功,审核通过后会显示在评论列表!" };
        }

        /// <summary>
        /// 是否已评价
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IfAppraise(CourseAppraiseModelInput input)
        {
            var sql = string.Format("SELECT COUNT(*) FROM dbo.t_Course_Appraise WHERE CourseId='{0}' AND UserId='{1}' ", input.CourseId, input.UserId);
            return Db.ExecuteScalar<int>(sql, null) > 0;
        }



        /// <summary>
        /// 查询评价列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CourseAppraiseModelOutput> GetList(CourseAppraiseModelInput input, out int count)
        {
            const string sql = "select a.*,b.NickNamw UserName,b.HeadImage  ";
            var strSql = " from t_Course_Appraise a left join t_Base_RegisterUser b on a.UserId=b.Id where 1=1 and a.AuditStatus=1 ";
            const string strCount = "select count(1) ";
            var dynamicparameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.CourseId))
            {
                strSql += " and CourseId=@CourseId ";
                dynamicparameter.Add(":CourseId", input.CourseId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(strCount + strSql, dynamicparameter);
            var list = Db.QueryList<CourseAppraiseModelOutput>(GetPageSql(sql + strSql,
                  dynamicparameter,
                  input.Page,
                  input.Rows, "AppraiseTime", "desc"), dynamicparameter);
            return list;
        }
    }
}
