using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:科目关联书籍管理 
    /// </summary>
    public class SubjectBookOutput
    {
       /// <summary>
       /// 科目编码
       /// </summary>     
      public string Id{ get; set; }
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

