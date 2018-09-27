using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    ///菜单查询输入Input
    /// </summary>
    public class ListMenuInput : BasePageInput
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
    }
}