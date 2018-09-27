using System;
using ZF.Application.BaseDto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 用户信息查询输入InPut
    /// </summary>
    public class UserInput : BasePageInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}

