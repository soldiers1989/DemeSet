using System.Data;
using System.Text;
using Dapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Dapper.Db.Repository
{
    /// <summary>
    /// 数据表实体仓储实现：RegisterUser 
    /// </summary>
    public class RegisterUserRepository : BaseRepositoryEntity<RegisterUser>, IRegisterUserRepository
    {
        /// <summary>
        /// 通过手机号码获取一条用户信息
        /// </summary>
        /// <param name="telphoneNum"></param>
        /// <returns></returns>
        public RegisterUserOutputDto GetLogin(string telphoneNum)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from V_UserTicket  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(telphoneNum))
            {
                strSql.Append("and (TelphoneNum = @TelphoneNum or Id = @TelphoneNum or WechatId = @TelphoneNum)");
                dynamicParameters.Add(":TelphoneNum", telphoneNum, DbType.String);
            }
            return Db.QueryFirstOrDefault<RegisterUserOutputDto>(sql + strSql, dynamicParameters);
        }
    }

    /// <summary>
    /// 用户信息查询输出Dto
    /// </summary>
    public class RegisterUserOutputDto
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string LoginPwd { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 购物车记录数
        /// </summary>
        public int CartCount { get; set; }

        /// <summary>
        /// 消息记录数
        /// </summary>
        public int MessageCount { get; set; }

        /// <summary>
        /// 学习记录数
        /// </summary>
        public int LearnCount { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickNamw { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string TelphoneNum { get; set; }

        /// <summary>
        /// 是否绑定微信号
        /// </summary>
        public int IsBindWiki { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string InstitutionsId { get; set; }
    }
}