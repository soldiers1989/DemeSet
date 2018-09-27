using System;
using System.Collections.Generic;
using ZF.Core;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表CourseInfo 输出Dto
    /// </summary>
    public class CourseInfoOutput
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
        /// 所属项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 课程所属类目
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 课程所属类目名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 课程所属科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }

        /// <summary>
        /// 课程视频简介
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
        public int? CourseWareCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

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
        public int? CourseType { get; set; }
        /// <summary>
        ///浏览量
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }

        /// <summary>
        ///讲师列表
        /// </summary>
        public List<CourseOnTeachers> TeacherList { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 截止有效日期
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }

        public string Number { get; set; }

    }
}

