
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：面试班 
    /// </summary>
    public class CourseFaceToFaceAppService : BaseAppService<CourseFaceToFace>
    {
        private readonly ICourseFaceToFaceRepository _iCourseFaceToFaceRepository;

        private readonly CourseOnTeachersAppService _courseOnTeachersAppService;

        private readonly ProjectAppService _projectAppService;

        private readonly SubjectAppService _subjectAppService;

        private readonly FileRelationshipAppService _fileRelationshipAppService;

        private readonly IBaseTagRepository _baseTagRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseFaceToFaceRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="courseOnTeachersAppService"></param>
        /// <param name="projectAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        /// <param name="baseTagRepository"></param>
        /// <param name="subjectAppService"></param>
        public CourseFaceToFaceAppService(ICourseFaceToFaceRepository iCourseFaceToFaceRepository, OperatorLogAppService operatorLogAppService, CourseOnTeachersAppService courseOnTeachersAppService, ProjectAppService projectAppService, FileRelationshipAppService fileRelationshipAppService, IBaseTagRepository baseTagRepository, SubjectAppService subjectAppService) : base(iCourseFaceToFaceRepository)
        {
            _iCourseFaceToFaceRepository = iCourseFaceToFaceRepository;
            _operatorLogAppService = operatorLogAppService;
            _courseOnTeachersAppService = courseOnTeachersAppService;
            _projectAppService = projectAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
            _baseTagRepository = baseTagRepository;
            _subjectAppService = subjectAppService;
        }

        /// <summary>
        /// 查询列表实体：面试班 
        /// </summary>
        public List<CourseFaceToFaceOutput> GetList(CourseFaceToFaceListInput input, out int count)
        {
            const string sql = "select  a.*,Isnull(b.COUNT,0) COUNT ";
            var strSql = new StringBuilder(@" from t_Course_FaceToFace  a  left join (SELECT CourseId,COUNT(1) COUNT,CourseType FROM dbo.t_Order_Detail GROUP BY CourseId,CourseType) b on a.Id=b.CourseId
                                              where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.ClassName))
            {
                strSql.Append(" and a.ClassName like @ClassName ");
                dynamicParameters.Add(":ClassName", '%' + input.ClassName + '%', DbType.String);
            }
            if (!string.IsNullOrEmpty(input.TeacherId))
            {
                strSql.Append(" and a.TeacherId like @TeacherId ");
                dynamicParameters.Add(":TeacherId", "%" + input.TeacherId + "%", DbType.String);
            }
            if (input.State.HasValue)
            {
                strSql.Append(" and isnull(a.State,0)=@State ");
                dynamicParameters.Add(":State", input.State, DbType.Int32);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseFaceToFaceOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  面试班
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseFaceToFaceInput input)
        {
            CourseFaceToFace model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseFaceToFaceRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.ClassName = input.ClassName;
                model.Address = input.Address;
                model.Characteristic = input.Characteristic;
                model.ClassTimeEnd = input.ClassTimeEnd;
                model.ClassTimeStart = input.ClassTimeStart;
                model.CourseAttribute = input.CourseAttribute;
                model.CourseContent = input.CourseContent;
                model.CourseIamge = input.CourseIamge;
                model.CourseIntroduce = input.CourseIntroduce;
                model.CourseTag = input.CourseTag;
                model.Curriculum = input.Curriculum;
                model.Discount = input.Discount;
                model.Characteristic = input.Characteristic;
                model.EmailNotes = model.EmailNotes;
                model.FavourablePrice = input.FavourablePrice;
                model.Price = input.Price;
                model.Remark = input.Remark;
                model.WhatTeach = input.WhatTeach;
                model.TeachingGoal = input.TeachingGoal;
                model.TeacherId = input.TeacherId;
                model.Number = input.Number;
                model.IsTop = input.IsTop;
                model.IsRecommend = input.IsRecommend;
                model.ClassId = input.ClassId;
                model.CourseIamge = input.IdFilehiddenFile;

                model.Title = input.Title;
                model.KeyWord = input.KeyWord;
                model.Description = input.Description;

                if (!string.IsNullOrEmpty(input.ClassId))
                {
                    var classId = input.ClassId.Split(',');
                    var projectClassId = classId.Select(item => _projectAppService.Get(item).ProjectClassId)
                        .Aggregate("", (current, id) => current + (id + ","));
                    model.ProjectClassId = projectClassId;
                }
                if (!string.IsNullOrEmpty(input.CourseTag))
                {
                    var tag = input.CourseTag.Split(',');
                    var tags = tag.Select(item => TagInsert(new Base_Tag
                    {
                        Id = Guid.NewGuid().ToString(),
                        ModelCode = "kcbq",
                        TagName = item
                    })).Aggregate("", (current, id) => current + (id + ","));
                    model.CourseTag = tags;
                }

                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iCourseFaceToFaceRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseFaceToFace,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改面试班:" + input.ClassName
                });

                if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
                {
                    _fileRelationshipAppService.Add(new FileRelationshipInput
                    {
                        ModuleId = model.Id,
                        IdFilehiddenFile = input.IdFilehiddenFile,
                        Type = 0
                    });
                }

                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseFaceToFace>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.CourseIamge = input.IdFilehiddenFile;
            model.AddTime = DateTime.Now;
            model.State = 1;
            if (!string.IsNullOrEmpty(input.CourseTag))
            {
                var tag = input.CourseTag.Split(',');
                var tags = tag.Select(item => TagInsert(new Base_Tag
                {
                    Id = Guid.NewGuid().ToString(),
                    ModelCode = "kcbq",
                    TagName = item
                })).Aggregate("", (current, id) => current + (id + ","));
                model.CourseTag = tags;
            }
            if (!string.IsNullOrEmpty(input.ClassId))
            {
                var classId = input.ClassId.Split(',');
                var projectClassId = classId.Select(item => _projectAppService.Get(item).ProjectClassId)
                    .Aggregate("", (current, id) => current + (id + ","));
                model.ProjectClassId = projectClassId;
            }
            var keyId = _iCourseFaceToFaceRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseFaceToFace,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增面试班:" + input.ClassName
            });
            if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
            {
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                    Type = 0
                });
            }
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string TagInsert(Base_Tag input)
        {
            if (!string.IsNullOrEmpty(input.TagName) || !string.IsNullOrEmpty(input.ModelCode))
            {
                var str = "select TagName from t_base_tag where 1=1";
                var dy = new DynamicParameters();
                str += " and ModelCode=@ModelCode ";
                str += " and TagName=@TagName ";
                dy.Add(":ModelCode", input.ModelCode, DbType.String);
                dy.Add(":TagName", input.TagName, DbType.String);
                var tagName = Db.QueryFirstOrDefault<string>(str, dy);
                if (!string.IsNullOrEmpty(tagName))
                {
                    return tagName;
                }
                input.Id = Guid.NewGuid().ToString();
                _baseTagRepository.InsertGetId(input);
                return input.TagName;
            }
            return "";
        }



        /// <summary>
        /// 更新上下架状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateState(CourseFaceToFaceInput input)
        {
            CourseFaceToFace model;
            model = _iCourseFaceToFaceRepository.Get(input.Id);
            #region 修改逻辑
            model.Id = input.Id;
            model.State = input.State;
            model.UpdateUserId = UserObject.Id;
            model.UpdateTime = DateTime.Now;
            #endregion
            _iCourseFaceToFaceRepository.Update(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = model.Id,
                ModuleId = (int)Model.CourseInfo,
                OperatorType = (int)OperatorType.Edit,
                Remark = string.Format("修改面授班上下架状态:{0}-{1}", model.Id, model.ClassName)
            });
            return new MessagesOutPut { Success = true, Message = "修改成功!" };
        }

    }
}

