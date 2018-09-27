using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.MyCollectionModule
{
   public class MyCollectionModuleListInput:BasePageInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set;}

        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 视频Id
        /// </summary>
        public string VideoId { get; set; }
    }
}
