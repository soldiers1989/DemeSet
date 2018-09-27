using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：Basedata 
    /// </summary>
    public class BasedataListInput: BasePageInput
    {


        /// <summary>
        /// 命名空间编号
        /// </summary>     
        public string DataTypeId { get; set; }
        /// <summary>
        /// 数据编码
        /// </summary>     
        public string Id{ get; set; }
       /// <summary>
       /// 数据名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 数据类型代码
       /// </summary>     
       public string Code{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
       public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
       public int IsDelete{ get; set; }
    }
}
