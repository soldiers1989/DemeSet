using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 用户信息编辑输入InPut
    /// </summary>
    [AutoMap(typeof(User))]
    public class UserEditInPut : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>     
        public string Phone { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public int IsAdmin { get; set; }

        /// <summary>
        /// 文件集合
        /// </summary>
        public string IdFilehiddenFile { get; set; }

    }
}