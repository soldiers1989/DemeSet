using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：文本配置表(联系我们,注册协议) 
    /// </summary>
   [AutoMap(typeof(Danye ))]
    public class DanyeInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Code{ get; set; }
       /// <summary>
       /// 名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 内容
       /// </summary>     
       public string Content{ get; set; }
    }
}

