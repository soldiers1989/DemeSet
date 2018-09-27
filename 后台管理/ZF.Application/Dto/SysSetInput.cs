using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：网站相关配置表 
    /// </summary>
   [AutoMap(typeof(SysSet ))]
    public class SysSetInput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 参数名称
       /// </summary>     
       public string ArguName{ get; set; }
       /// <summary>
       /// 参数值
       /// </summary>     
       public string ArguValue{ get; set; }
       /// <summary>
       /// 备注
       /// </summary>     
       public string Remark{ get; set; }
    }
}

