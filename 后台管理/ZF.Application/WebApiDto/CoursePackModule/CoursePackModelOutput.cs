using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiDto.CourseModule;

namespace ZF.Application.WebApiDto.CoursePackModule
{
    /// <summary>
    /// 
    /// </summary>
   public class CoursePackModelOutput
    {
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }
        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }
        /// <summary>
        /// 课程简介
        /// </summary>     
        public string CourseContent { get; set; }
        /// <summary>
        /// 原价
        /// </summary>     
        public decimal Price { get; set; }
        /// <summary>
        /// 优惠价
        /// </summary>     
        public decimal FavourablePrice { get; set; }
        /// <summary>
        /// 有效期天数
        /// </summary>     
        public int? ValidityPeriod { get; set; }
        /// <summary>
        /// 上下架状态
        /// </summary>     
        public int? State { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>     
        public int? IsTop { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>     
        public int? IsRecommend { get; set; }
        /// <summary>
        /// 课程标签
        /// </summary>     
        public string CourseTag { get; set; }
        /// <summary>
        /// 课程时长
        /// </summary>     
        public string CourseLongTime { get; set; }
        /// <summary>
        /// 课程数
        /// </summary>     
        public int? CourseWareCount { get; set; }

        /// <summary>
        /// 已学习课时数
        /// </summary>
        public int HasLearnCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 套餐课程视频介绍
        /// </summary>
        public string CourseVedio { get; set; }

        /// <summary>
        /// 评价分数
        /// </summary>
        public int EvaluationScore { get; set; }

        /// <summary>
        /// 套餐课程子课程
        /// </summary>
        public List<CourseInfoModelOutput> SubCourseList { get; set; }

        /// <summary>
        /// 收藏总数
        /// </summary>
        public int CollectionNum { get; set;}
        /// <summary>
        /// 购买总数
        /// </summary>
        public int PurchaseNum { get; set;}

        /// <summary>
        /// 评价次数  wyf 2018-05-16 add
        /// </summary> 
        public int? AppraiseNum { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public  DateTime? ValidityEndDate { get; set; }
    }
}
