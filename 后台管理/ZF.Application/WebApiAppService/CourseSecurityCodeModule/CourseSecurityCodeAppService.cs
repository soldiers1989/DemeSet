using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseSecurityCodeModule
{
    /// <summary>
    /// 课程防伪码前台服务类
    /// </summary>
    public class CourseSecurityCodeAppService : BaseAppService<CourseSecurityCode>
    {
        private readonly ICourseSecurityCodeRepository _iCourseSecurityCodeRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseSecurityCodeRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public CourseSecurityCodeAppService(ICourseSecurityCodeRepository iCourseSecurityCodeRepository,
            OperatorLogAppService operatorLogAppService) : base(iCourseSecurityCodeRepository)
        {
            _iCourseSecurityCodeRepository = iCourseSecurityCodeRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 验证防伪码是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut VerifySecurityCode(CourseSecurityCodeListInput input)
        {
            const string sql = " SELECT * FROM t_Course_SecurityCode WHERE Code=@Code  ";
            var parament = new DynamicParameters();
            parament.Add(":Code", input.Code, System.Data.DbType.String);
            var model = Db.QueryFirstOrDefault<CourseSecurityCode>(sql, parament);
            if (model != null)
            {
                return new MessagesOutPut { Success = true, Message = input.Code, Id = model.IsValueAdded };
            }
            return new MessagesOutPut { Success = false, Message = input.Code, Id = 0 };
        }
    }
}
