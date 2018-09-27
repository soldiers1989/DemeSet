using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.MyCollectionModule
{
    /// <summary>
    /// 课程收藏
    /// </summary>
    [AutoMap( typeof( MyCollection ) )]
    public class MyCollectionModuleInput
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
