using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：Basedata 
    /// </summary>
   [AutoMap(typeof(Basedata ))]
    public class BasedataInput
    {
       /// <summary>
       /// 数据编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 命名空间编号
       /// </summary>     
       public string DataTypeId{ get; set; }
       /// <summary>
       /// 数据名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 数据类型代码
       /// </summary>     
       public string Code{ get; set; }
       /// <summary>
       /// 说明
       /// </summary>     
       public string Desc{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
       public int? Sort{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime? UpdateTime{ get; set; }
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

