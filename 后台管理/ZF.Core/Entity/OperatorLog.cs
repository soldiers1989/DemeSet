using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：OperatorLog 
    /// </summary>
    public class OperatorLog:BaseEntity<Guid>
    {
       /// <summary>
       /// 模块Id
       /// </summary>     
       public int ModuleId{ get; set; }

       /// <summary>
       /// 标识Id
       /// </summary>     
       public string KeyId{ get; set; }

       /// <summary>
       /// 操作人
       /// </summary>     
       public string OperatorName{ get; set; }

       /// <summary>
       /// 操作时间
       /// </summary>     
       public DateTime OperatorDate{ get; set; }

       /// <summary>
       /// 操作类型
       /// </summary>     
       public int? OperatorType{ get; set; }

       /// <summary>
       /// 操作人Id
       /// </summary>     
       public string OperatorId{ get; set; }

       /// <summary>
       /// 操作内容
       /// </summary>     
       public string Remark{ get; set; }

       /// <summary>
       /// 操作人Ip
       /// </summary>     
       public string OperatorIp{ get; set; }

    }
}

