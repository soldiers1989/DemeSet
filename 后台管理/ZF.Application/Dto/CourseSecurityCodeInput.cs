using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：课程防伪码管理 
    /// </summary>
   [AutoMap(typeof(CourseSecurityCode ))]
    public class CourseSecurityCodeInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }
       /// <summary>
       /// 防伪码
       /// </summary>     
       public string Code{ get; set; }
       /// <summary>
       /// 是否使用
       /// </summary>     
       public int? IsUse{ get; set; }
       /// <summary>
       /// 使用用户编号
       /// </summary>     
       public string UserId{ get; set; }

        /// <summary>
        /// 是否增值服务  1是 0否
        /// </summary>
        public int IsValueAdded { get; set; }
    }
}

