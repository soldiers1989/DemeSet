//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZF.EF.Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_Base_User
    {
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public Nullable<int> IsAdmin { get; set; }
        public string Phone { get; set; }
        public System.DateTime AddTime { get; set; }
        public string AddUserId { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string UpdateUserId { get; set; }
        public bool IsDelete { get; set; }
    }
}