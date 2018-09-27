using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：FileRelationship 
    /// </summary>
    public class FileRelationship:BaseEntity<Guid>
    {
       /// <summary>
       /// 模块编号
       /// </summary>     
       public string ModuleId{ get; set; }

       /// <summary>
       /// 类型
       /// </summary>     
       public int? Type{ get; set; }

       /// <summary>
       /// 七牛云存储文件名
       /// </summary>     
       public string QlyName{ get; set; }

       /// <summary>
       /// 上传时间
       /// </summary>     
       public DateTime? CreateTime{ get; set; }

    }
}

