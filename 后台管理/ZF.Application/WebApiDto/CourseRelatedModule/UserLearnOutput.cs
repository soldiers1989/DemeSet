using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CourseRelatedModule
{
    /// <summary>
    /// 
    /// </summary>
   public class UserLearnOutput
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickNamw { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImage { get; set; }
    }
}
