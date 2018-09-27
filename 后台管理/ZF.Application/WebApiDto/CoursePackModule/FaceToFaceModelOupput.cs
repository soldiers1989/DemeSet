using System;
using ZF.Core;

namespace ZF.Application.WebApiDto.CoursePackModule
{
    /// <summary>
    /// 
    /// </summary>
    public class FaceToFaceModelOupput : BaseEntity<Guid>
    {

        /// <summary>
        /// 班级名称
        /// </summary>     
        public string ClassName { get; set; }


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
        /// 原价
        /// </summary>     
        public decimal Price { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>     
        public decimal FavourablePrice { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>     
        public string Address { get; set; }

        /// <summary>
        /// 上课时间起
        /// </summary>     
        public DateTime? ClassTimeStart { get; set; }

        /// <summary>
        /// 上课时间止
        /// </summary>     
        public DateTime? ClassTimeEnd { get; set; }

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
        /// 课程属性
        /// </summary>     
        public string CourseAttribute { get; set; }


        /// <summary>
        /// 评价分数
        /// </summary>
        public int EvaluationScore { get; set; }

        /// <summary>
        /// 收藏总数
        /// </summary>
        public int CollectionNum { get; set; }
        /// <summary>
        /// 购买总数
        /// </summary>
        public int PurchaseNum { get; set; }

        /// <summary>
        /// 评价次数  wyf 2018-05-16 add
        /// </summary> 
        public int? AppraiseNum { get; set; }

        /// <summary>
        /// 上课人数
        /// </summary>
        public int? Number { get; set; }

    }
}