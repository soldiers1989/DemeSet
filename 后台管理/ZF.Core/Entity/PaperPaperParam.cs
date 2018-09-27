using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperPaperParam 
    /// </summary>
    public class PaperPaperParam:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 试卷结构编码
       /// </summary>     
       public string StuctureId{ get; set; }

       /// <summary>
       /// 参数名称
       /// </summary>     
       public string ParamName{ get; set; }

        /// <summary>
        /// 参数发布状态
        /// </summary>
        public int State { get; set; }

    }
}

