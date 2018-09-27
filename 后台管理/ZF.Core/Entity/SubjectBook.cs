using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：科目关联书籍管理 
    /// </summary>
    public class SubjectBook:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 书籍名称
       /// </summary>     
       public string BookName{ get; set; }

       /// <summary>
       /// 所属科目
       /// </summary>     
       public string SubjectId{ get; set; }

       /// <summary>
       /// 排序号
       /// </summary>     
       public int? OrderNo{ get; set; }

       /// <summary>
       /// 封面图片
       /// </summary>     
       public string ImageUrl{ get; set; }

       /// <summary>
       /// 链接Url
       /// </summary>     
       public string Url{ get; set; }

    }
}

