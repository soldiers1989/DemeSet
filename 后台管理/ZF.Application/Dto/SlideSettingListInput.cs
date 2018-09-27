using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：幻灯片设置表 
    /// </summary>
    public class SlideSettingListInput: BasePageInput
    {
       /// <summary>
       /// 状态
       /// </summary>     
       public int? State{ get; set; }
       /// <summary>
       /// 链接地址
       /// </summary>     
       public string LinkAddress{ get; set; }

        /// <summary>
        /// 类型  0首页  1课程
        /// </summary>
        public int? Type { get; set; }
    }
}
