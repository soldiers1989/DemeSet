using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：Project 
    /// </summary>
    public class Project:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 项目名称
       /// </summary>     
       public string ProjectName{ get; set; }

       /// <summary>
       /// 所属项目分类
       /// </summary>     
       public string ProjectClassId{ get; set; }

       /// <summary>
       /// 项目说明
       /// </summary>     
       public string Remark{ get; set; }

    }
}

