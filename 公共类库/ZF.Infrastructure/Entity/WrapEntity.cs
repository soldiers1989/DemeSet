using System.Configuration;

namespace ZF.Infrastructure.Entity
{
    /// <summary>
    /// 包装对象
    /// </summary>
    public class WrapEntity
    {
        /// <summary>
        /// 创建禁止的wrap实体
        /// </summary>
        /// <returns></returns>
        public static WrapEntity CreateForbiddenEntity()
        {
            return new WrapEntity
            {
                TargetUrl = ConfigurationManager.AppSettings["loginUrl"],
                UnAuthorizedRequest = true,
                Error = "验证失败",
                Success = false
            };
        }

      

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WrapEntity()
        {
            
        }

        /// <summary>
        /// 创建WrapEntity
        /// </summary>
        /// <param name="obj"></param>
        public WrapEntity(object obj)
        {
            Result = obj;
            Success = true;
        }
        /// <summary>
        /// 返回结果集
        /// </summary>
        public object Result { set; get; }

        /// <summary>
        /// 目标Url
        /// </summary>
        public string TargetUrl { set; get; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { set; get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { set; get; }

        /// <summary>
        /// 是否是非法验证
        /// </summary>
        public bool UnAuthorizedRequest { set; get; }
    }
}
