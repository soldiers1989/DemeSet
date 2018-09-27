using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：科目关联书籍管理 
    /// </summary>
    [AutoMap(typeof(SubjectBook))]
    public class SubjectBookInput
    {
        /// <summary>
        /// 科目编码
        /// </summary>     
        public string Id { get; set; }

        /// <summary>
        /// 所属科目
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 书籍名称
        /// </summary>     
        public string BookName { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>     
        public int? OrderNo { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>     
        public string ImageUrl { get; set; }
        /// <summary>
        /// 链接Url
        /// </summary>     
        public string Url { get; set; }
        /// <summary>
        /// 文件集合
        /// </summary>
        public string IdFilehiddenFile { get; set; }
    }
}

