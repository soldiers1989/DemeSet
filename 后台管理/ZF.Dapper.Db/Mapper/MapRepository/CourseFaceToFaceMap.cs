using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：面试班 
    /// </summary>
    public sealed class CourseFaceToFaceMap : BaseClassMapper<CourseFaceToFace, Guid>
    {
        public CourseFaceToFaceMap()
        {
            Table("t_Course_FaceToFace");

            Map(x => x.ClassName).Column("ClassName");
            Map(x => x.EmailNotes).Column("EmailNotes");
            Map(x => x.SubjectId).Column("SubjectId");
            Map(x => x.CourseIamge).Column("CourseIamge");
            Map(x => x.CourseContent).Column("CourseContent");
            Map(x => x.CourseIntroduce).Column("CourseIntroduce");
            Map(x => x.Remark).Column("Remark");
            Map(x => x.Price).Column("Price");
            Map(x => x.Discount).Column("Discount");
            Map(x => x.FavourablePrice).Column("FavourablePrice");
            Map(x => x.Address).Column("Address");
            Map(x => x.Number).Column("Number");
            Map(x => x.ClassTimeStart).Column("ClassTimeStart");
            Map(x => x.ClassTimeEnd).Column("ClassTimeEnd");
            Map(x => x.State).Column("State");
            Map(x => x.TeacherId).Column("TeacherId");
            Map(x => x.TeachingObject).Column("TeachingObject");
            Map(x => x.TeachingGoal).Column("TeachingGoal");
            Map(x => x.WhatTeach).Column("WhatTeach");
            Map(x => x.Characteristic).Column("Characteristic");
            Map(x => x.Curriculum).Column("Curriculum");
            Map(x => x.IsTop).Column("IsTop");
            Map(x => x.IsRecommend).Column("IsRecommend");
            Map(x => x.CourseTag).Column("CourseTag");
            Map(x => x.AddTime).Column("AddTime");
            Map(x => x.AddUserId).Column("AddUserId");
            Map(x => x.UpdateTime).Column("UpdateTime");
            Map(x => x.UpdateUserId).Column("UpdateUserId");
            Map(x => x.IsDelete).Column("IsDelete");
            Map(x => x.CourseAttribute).Column("CourseAttribute");
            Map(x => x.ClassId).Column("ClassId");
            Map(x => x.ProjectClassId).Column("ProjectClassId");
            Map(x => x.Title).Column("Title");
            Map(x => x.KeyWord).Column("KeyWord");
            Map(x => x.Description).Column("Description");

            this.AutoMap();
        }
    }
}

