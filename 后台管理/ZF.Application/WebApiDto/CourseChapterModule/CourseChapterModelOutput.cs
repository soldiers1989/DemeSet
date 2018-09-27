using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CourseChapterModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseChapterModelOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 章节名称
        /// </summary>     
        public string CapterName { get; set; }
        /// <summary>
        /// 父节点编码
        /// </summary>     
        public string ParentId { get; set; }
        /// <summary>
        /// 章节代码
        /// </summary>     
        public string CapterCode { get; set; }
        /// <summary>
        /// 所属课程
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string VideoUrl { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public string VideoLongTime { get; set; }

        /// <summary>
        /// 练习试题数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 视频是否可试听
        /// </summary>
        public int IsTaste { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// 视频学习状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 试听时长
        /// </summary>
        public int TasteLongTime { get; set; }
        /// <summary>
        /// 未登录可视频时长
        /// </summary>
        public int TasteLongTime2 { get; set; }
        /// <summary>
        /// 是否为练习
        /// </summary>
        public int IsExercise { get; set; }


        /// <summary>
        /// 显示序号
        /// </summary>
        public int? RowsIndex { get; set; }

        /// <summary>
        /// 子章节列表
        /// </summary>
        public List<CourseChapterModelOutput> SubChapterList { get; set; }
    }
}
