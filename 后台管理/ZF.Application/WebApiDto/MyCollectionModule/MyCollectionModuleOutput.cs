using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.MyCollectionModule
{
    /// <summary>
    /// 
    /// </summary>
    public class MyCollectionModuleOutput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId { get; set; }

        public string CourseName { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string CourseIamge { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }


        public string VideoId { get; set; }
        /// <summary>
        ///视频名称
        /// </summary>
        public string VideoName { get; set; }


    }
}
