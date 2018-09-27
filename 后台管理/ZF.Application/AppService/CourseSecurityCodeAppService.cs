
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Dynamic;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.CourseSecurityCodeModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：课程防伪码管理 
    /// </summary>
    public class CourseSecurityCodeAppService : BaseAppService<CourseSecurityCode>
    {
        private readonly ICourseSecurityCodeRepository _iCourseSecurityCodeRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly CourseInfoAppService _courseInfoAppService;

        private readonly MyCourseApiService _courseApiService;

        //我的课程
        private readonly IMyCourseRepository _iMyCourseRepository;

        private readonly IDiscount_CardRepository _discountCardRepository;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseSecurityCodeRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="courseInfoAppService"></param>
        public CourseSecurityCodeAppService(ICourseSecurityCodeRepository iCourseSecurityCodeRepository,
            OperatorLogAppService operatorLogAppService, CourseInfoAppService courseInfoAppService, IMyCourseRepository iMyCourseRepository, IDiscount_CardRepository discountCardRepository) : base(iCourseSecurityCodeRepository)
        {
            _iCourseSecurityCodeRepository = iCourseSecurityCodeRepository;
            _operatorLogAppService = operatorLogAppService;
            _courseInfoAppService = courseInfoAppService;
            _iMyCourseRepository = iMyCourseRepository;
            _discountCardRepository = discountCardRepository;
        }

        /// <summary>
        /// 查询列表实体：课程防伪码管理 
        /// </summary>
        public List<CourseSecurityCodeOutput> GetList(CourseSecurityCodeListInput input, out int count)
        {
            const string sql = "select  a.*,b.NickNamw NickNamw ";
            var strSql = new StringBuilder(" from t_Course_SecurityCode  a left join t_Base_RegisterUser b on a.UserId=b.Id  where 1=1 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(input.CourseId))
            {
                strSql.Append(" and a.CourseId =@CourseId ");
                dynamicParameters.Add(":CourseId", input.CourseId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.Code))
            {
                strSql.Append(" and a.Code like @Code ");
                dynamicParameters.Add(":Code", '%' + input.Code + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseSecurityCodeOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 导入防伪码信息
        /// </summary>
        /// <param name="dvHouseBan"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public MessagesOutPut ReptInsert(DataView dvHouseBan, string courseId)
        {
            var list = new List<CourseSecurityCode>();
            for (int i = 0; i < dvHouseBan.Count; i++)
            {
                try
                {
                    var model = new CourseSecurityCode
                    {
                        CourseId = courseId,
                        Code = dvHouseBan[i]["防伪码"].ToString().Trim(),
                        IsValueAdded = int.Parse(dvHouseBan[i]["是否增值"].ToString().Trim()),
                        Id = Guid.NewGuid().ToString(),
                        IsUse = 0,
                    };
                    if (Db.QueryFirstOrDefault<int>($"select count(1) from t_Course_SecurityCode where Code='{model.Code}'  ", null) > 0)
                    {
                        return new MessagesOutPut { Success = false, Message = "导入失败!失败数据：第" + (i + 1) + "条,失败原因，该防伪码已经在数据库中存在!" };
                    }
                    list.Add(model);
                }
                catch (Exception)
                {
                    return new MessagesOutPut { Success = false, Message = "导入失败!失败数据：第" + (i + 1) + "条,失败原因，数据格式不正确!" };
                }
            }
            foreach (var item in list)
            {
                _iCourseSecurityCodeRepository.Insert(item);
            }
            return new MessagesOutPut { Success = true, Message = "导入成功!" };
        }

        /// <summary>
        /// 扫描防伪码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userId"></param>
        /// <param name="isValueAdded">是否增值服务</param>
        /// <returns></returns>
        public MessagesOutPut CheckCourseCode(string code, string userId, bool? isValueAdded)
        {
            var courseCount = 0;
            var tikuCount = 0;
            var cardCount = 0;
            var pic = 0.0;
            var message = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return new MessagesOutPut { Id = -2, Message = "扫码失败,系统不存在该防伪码!", Success = false };
                }
                var strSql = " select * from t_Course_SecurityCode where Code=@Code ";
                var dy = new DynamicParameters();
                dy.Add(":Code", code, DbType.String);
                var model = Db.QueryFirstOrDefault<CourseSecurityCode>(strSql, dy);
                if (model == null)
                {
                    return new MessagesOutPut { Id = -2, Message = "扫码失败,系统不存在该防伪码!", Success = false };
                }
                if (model.IsUse == 1)
                {
                    return new MessagesOutPut { Id = -1, Message = "扫码失败,该防伪码已经被使用!", Success = false };
                }
                if (isValueAdded.HasValue)
                {
                    if (isValueAdded.Value)
                    {
                        if (model.IsValueAdded == 0)
                        {
                            return new MessagesOutPut { Id = -1, Message = "扫码失败,该防伪码请在'人事社经济师课堂'公众号中领取!", Success = false };
                        }
                    }
                    else
                    {
                        if (model.IsValueAdded == 1)
                        {
                            return new MessagesOutPut { Id = -1, Message = "扫码失败,该防伪码请在'人事社经济师增值服务'公众号中领取!", Success = false };
                        }
                    }
                }
                if (model.IsUse != 0) return new MessagesOutPut { Id = -1, Message = "扫码失败!", Success = false };
                var courseId = model.CourseId;
                var courseModel = _courseInfoAppService.Get(courseId);
                if (courseModel == null)
                    return new MessagesOutPut { Id = -2, Message = "扫码失败!", Success = false };
                if (courseModel.ValidityEndDate != null)
                {
                    var endTime = courseModel.ValidityEndDate;


                    //判断是否购买过该课程
                    var strSql1 = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                    var dy1 = new DynamicParameters();
                    dy1.Add(":CourseId", courseModel.Id, DbType.String);
                    dy1.Add(":UserId", userId, DbType.String);
                    var model1 = Db.QueryFirstOrDefault<MyCourse>(strSql1, dy1);
                    if (model1 == null)
                    {
                        var sqlList = $" insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{Guid.NewGuid()}','{userId}','{courseModel.Id}','{DateTime.Now}','{DateTime.Now}','{endTime}','{model.Id}',0)";
                        Db.ExecuteNonQuery(sqlList, null);
                    }
                    else
                    {
                        if (model1.EndTime != null)
                            model1.EndTime = endTime;
                        _iMyCourseRepository.Update(model1);
                    }
                    courseCount = 1;
                }
                var courseSecurityCode = _discountCardRepository.Get(courseModel.DiscountCardId);

                //写入课程关联学习卡到我的学习卡中
                if (courseSecurityCode != null)
                {
                    cardCount = 1;
                    pic = courseSecurityCode.Amount;
                    //if (Db.QueryFirstOrDefault<int>($"select count(1) from t_User_Discount_Card where CardId='{courseSecurityCode.CardCode}' and UserId='{userId}' ", null) == 0)
                    //{
                    var sqldis =
                        $@"INSERT INTO dbo.t_User_Discount_Card
        ( Id ,
          UserId ,
          CardId ,
          AddTime ,
          IfUse ,
          Type
        )
VALUES  ( N'{Guid.NewGuid()}' , -- Id - nvarchar(36)
          N'{userId}' , -- UserId - nvarchar(36)
          N'{courseSecurityCode.CardCode}' , -- CardId - nvarchar(36)
          GETDATE() , -- AddTime - datetime
          0 , -- IfUse - int
          0  -- Type - int
        )";
                    Db.ExecuteNonQuery(sqldis, null);
                    //}
                }
                model.IsUse = 1;
                model.UserId = userId;
                model.GetDateTime = DateTime.Now;
                _iCourseSecurityCodeRepository.Update(model);

                #region  插入课程关联的题库到我的课程
                StringBuilder sqlList1 = new StringBuilder();
                var linkCourseSql = $"select * from t_Course_Info where 1=1 and IsDelete=0  and Type=1 and LinkCourse like'%{courseId}%'";
                var linkCourseList = Db.QueryList<CourseInfo>(linkCourseSql);
                tikuCount = linkCourseList.Count;
                foreach (var itemLinkCourse in linkCourseList)
                {
                    if (itemLinkCourse.ValidityEndDate != null)
                    {
                        var endTime1 = itemLinkCourse.ValidityEndDate;
                        //判断是否购买过该课程
                        var strSq2 = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                        var dy2 = new DynamicParameters();
                        dy2.Add(":CourseId", itemLinkCourse.Id, DbType.String);
                        dy2.Add(":UserId", userId, DbType.String);
                        var model2 = Db.QueryFirstOrDefault<MyCourse>(strSq2, dy2);
                        if (model2 == null)
                        {
                            sqlList1.AppendFormat(" insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                Guid.NewGuid(), userId, itemLinkCourse.Id, DateTime.Now, DateTime.Now, endTime1, model.Id, 0);
                        }
                        else
                        {
                            if (model2.EndTime != null)
                                model2.EndTime = itemLinkCourse.ValidityEndDate;
                            _iMyCourseRepository.Update(model2);
                        }
                    }
                }
                if (sqlList1.Length > 0)
                {
                    Db.ExecuteNonQuery(sqlList1.ToString(), null);
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new MessagesOutPut { Id = -1, Message = message, Success = false };
            }
            return new MessagesOutPut { Id = -1, Message = "扫码成功!", Success = true, ModelId = courseCount + "," + tikuCount + "," + cardCount + "," + pic };
        }

        /// <summary>
        /// 更加课程编号清空防伪码
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(string courseId)
        {
            var str = "select count(1) from t_Course_SecurityCode where CourseId=@CourseId and  IsUse=1 ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            if (Db.QueryFirstOrDefault<int>(str, dy) > 0)
            {
                return new MessagesOutPut { Id = -1, Message = "清空失败,存在使用过的防伪码!", Success = false };
            }
            Db.ExecuteNonQuery("delete t_Course_SecurityCode where CourseId=@CourseId", dy);
            return new MessagesOutPut { Id = -1, Message = "清空成功!", Success = true };
        }

        /// <summary>
        /// 新增实体  CourseSuitDetail
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseSecurityCodeInput input)
        {
            CourseSecurityCode model;
            model = input.MapTo<CourseSecurityCode>();
            if (Db.QueryFirstOrDefault<int>($"select count(1) from t_Course_SecurityCode where Code='{model.Code}'  ", null) > 0)
            {
                return new MessagesOutPut { Success = false, Message = "新增失败，该防伪码已经在数据库中存在!" };
            }
            model.CourseId = input.CourseId;
            model.Id = Guid.NewGuid().ToString();
            model.IsUse = 0;
            _iCourseSecurityCodeRepository.Insert(model);
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 新增实体  CourseSuitDetail
        /// </summary>
        public MessagesOutPut Complaints(ComplaintsInput input)
        {
            var insertSql =
                $@"INSERT INTO dbo.t_Base_Complaints
        ( Id ,
          BranchesName ,
          PhysicalStoreName ,
          ImageUrl
        )
VALUES  ( N'{Guid.NewGuid()}' , -- Id - nvarchar(36)
          N'{input.BranchesName}' , -- BranchesName - nvarchar(200)
          N'{input.PhysicalStoreName}' , -- PhysicalStoreName - nvarchar(200)
          N'{input.ImageUrl}'  -- ImageUrl - nvarchar(200)
        )";
            Db.ExecuteNonQuery(insertSql, null);
            return new MessagesOutPut { Success = true, Message = "提交成功!" };
        }
    }
}

