using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：ProjectClass 
    /// </summary>
   [AutoMap(typeof(ProjectClass ))]
    public class ProjectClassInput
    {
       /// <summary>
       /// 项目分类编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 项目分类名称
       /// </summary>     
       public string ProjectClassName{ get; set; }
       /// <summary>
       /// 分类说明
       /// </summary>     
       public string Remark{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
       public int? OrderNo{ get; set; }
    }
}

