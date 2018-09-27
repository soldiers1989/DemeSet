using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseInfoModelOutput
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
        /// 课程所属科目
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 课程所属科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }

        /// <summary>
        /// 课程简介视频
        /// </summary>
        public string CourseVedio { get; set; }

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
        /// 主讲教师
        /// </summary>     
        public string TeacherId { get; set; }

        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeachersName { get; set; }

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
        public int? CourseLongTime { get; set; }

        /// <summary>
        /// 课件数
        /// </summary>     
        public int CourseWareCount { get; set; }

        /// <summary>
        /// 课程时长  wyf 2018-05-16 add
        /// </summary>     
        public string CourseLongTimes { get; set; }

        /// <summary>
        /// 课件数  wyf 2018-05-16 add
        /// </summary>     
        public int CourseWareCounts { get; set; }

        /// <summary>
        /// 评价次数  wyf 2018-05-16 add
        /// </summary> 
        public int AppraiseNum { get; set; }

        /// <summary>
        /// 已学习课时数
        /// </summary>
        public int HasLearnCount { get; set; }

        /// <summary>
        /// 类型  套餐 和课程
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 类型  套餐 和课程
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 讲师标签
        /// </summary>
        public string TheLabel { get; set; }

        /// <summary>
        /// 讲师简介
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// 讲师照片
        /// </summary>
        public string TeacherPhoto { get; set; }

        /// <summary>
        /// 评价分数
        /// </summary>
        public int EvaluationScore { get; set; }

        /// <summary>
        /// 学习人数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        public int CollectionNum { get; set; }

        /// <summary>
        /// 购买数
        /// </summary>
        public int PurchaseNum { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }

        /// <summary>
        /// SEO 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// SEO 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// SEO 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CourseOnTeachers> TeacherList { get; set; }

        /// <summary>
        /// 购买次数
        /// </summary>
        public int PurchaseCount { get; set; }

        public string VideoId { get; set; }

        /// <summary>
        /// 最近观看视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 类型：0-课程 1：题库
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 关联课程名称
        /// </summary>
        public string LinkCourseName { get; set; }

        /// <summary>
        /// 关联课程编号
        /// </summary>
        public string LinkCourse { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }

        /// <summary>
        /// 是否增值服务
        /// </summary>
        public int IsValueAdded { get; set; }
    }
}
