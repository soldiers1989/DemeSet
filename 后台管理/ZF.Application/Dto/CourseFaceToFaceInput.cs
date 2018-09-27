using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：面试班 
    /// </summary>
   [AutoMap(typeof(CourseFaceToFace ))]
    public class CourseFaceToFaceInput : BaseEntity<Guid>
    {
        /// <summary>
        /// 班级名称
        /// </summary>     
        public string ClassName { get; set; }

        /// <summary>
        /// 邮件讲义
        /// </summary>     
        public int EmailNotes { get; set; }

        /// <summary>
        /// 所属科目
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 封面
        /// </summary>     
        public string CourseIamge { get; set; }

        /// <summary>
        /// 详细描述
        /// </summary>     
        public string CourseContent { get; set; }

        /// <summary>
        /// 课程介绍
        /// </summary>     
        public string CourseIntroduce { get; set; }

        /// <summary>
        /// 备注
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 原价
        /// </summary>     
        public decimal Price { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>     
        public decimal? Discount { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>     
        public decimal FavourablePrice { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>     
        public string Address { get; set; }

        /// <summary>
        /// 上课人数
        /// </summary>     
        public int? Number { get; set; }

        /// <summary>
        /// 上课时间起
        /// </summary>     
        public DateTime? ClassTimeStart { get; set; }

        /// <summary>
        /// 上课时间止
        /// </summary>     
        public DateTime? ClassTimeEnd { get; set; }

        /// <summary>
        /// 上下架状态
        /// </summary>     
        public int? State { get; set; }

        /// <summary>
        /// 主讲教师
        /// </summary>     
        public string TeacherId { get; set; }

        /// <summary>
        /// 教学对象
        /// </summary>     
        public string TeachingObject { get; set; }

        /// <summary>
        /// 教学目标
        /// </summary>     
        public string TeachingGoal { get; set; }

        /// <summary>
        /// 授课内容
        /// </summary>     
        public string WhatTeach { get; set; }

        /// <summary>
        /// 课程特色
        /// </summary>     
        public string Characteristic { get; set; }

        /// <summary>
        /// 课程安排
        /// </summary>     
        public string Curriculum { get; set; }

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
        /// 课程属性
        /// </summary>     
        public string CourseAttribute { get; set; }

        /// <summary>
        /// 所属类目
        /// </summary>     
        public string ClassId { get; set; }

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
        /// 文件集合
        /// </summary>
        public string IdFilehiddenFile { get; set; }
    }
}

