using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：Project 
    /// </summary>
    public class ProjectListInput: BasePageInput
    {
       /// <summary>
       /// 项目编码
       /// </summary>     
       public string Id{ get; set; }
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
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
       public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
       public int? IsDelete{ get; set; }
    }
}
