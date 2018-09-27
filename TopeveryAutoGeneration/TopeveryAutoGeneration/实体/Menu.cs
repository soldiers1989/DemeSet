using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：Menu 
    /// </summary>
    public class Menu:BaseEntity<Guid>
    {
       /// <summary>
       /// 模块编号
       /// </summary>     
       public string ModuleId{ get; set; }

       /// <summary>
       /// 菜单名称
       /// </summary>     
       public string MenuName{ get; set; }

       /// <summary>
       /// 路径
       /// </summary>     
       public string Url{ get; set; }

       /// <summary>
       /// 排序号
       /// </summary>     
       public int? Sort{ get; set; }

       /// <summary>
       /// 样式
       /// </summary>     
       public string Class{ get; set; }

       /// <summary>
       /// 描述
       /// </summary>     
       public string Description{ get; set; }

    }
}

