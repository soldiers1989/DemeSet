using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：SubjectKnowledgePoint 
    /// </summary>
   [AutoMap(typeof(SubjectKnowledgePoint ))]
    public class SubjectKnowledgePointInput
    {
       /// <summary>
       /// 知识点编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 知识点名称
       /// </summary>     
       public string KnowledgePointName{ get; set; }
       /// <summary>
       /// 所属科目
       /// </summary>     
       public string SubjectId{ get; set; }
       /// <summary>
       /// 上级知识点编码
       /// </summary>     
       public string ParentId{ get; set; }

        /// <summary>
        /// 电子书页面
        /// </summary>
        public string DigitalBookPage { get; set; }

    }
}

