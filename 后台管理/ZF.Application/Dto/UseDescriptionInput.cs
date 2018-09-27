using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：使用指南 
    /// </summary>
   [AutoMap(typeof(UseDescription ))]
    public class UseDescriptionInput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 大类编号
       /// </summary>     
       public string BigClassId { get; set; }
       /// <summary>
       /// 小类编号
       /// </summary>     
       public string ClassId{ get; set; }
       /// <summary>
       /// 内容
       /// </summary>     
       public string Content{ get; set; }
      
    }
}

