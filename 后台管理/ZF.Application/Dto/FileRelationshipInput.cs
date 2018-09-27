using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：FileRelationship 
    /// </summary>
   [AutoMap(typeof(FileRelationship ))]
    public class FileRelationshipInput
    {
       /// <summary>
       /// 编号
       /// </summary>     
       public string Id{ get; set; }

       /// <summary>
       /// 模块编号
       /// </summary>     
       public string ModuleId{ get; set; }

        /// <summary>
        /// 类型
        /// </summary>     
        public int? Type { get; set; } = 0; ///默认为0   如果一条数据需要关联多个文件属性  则可以扩展其他数值
       /// <summary>
       /// 文件集合
       /// </summary>     
       public string IdFilehiddenFile { get; set; }
      
    }
}

