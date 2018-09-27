
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using Topevery.Application.Dto;
using ZF.Application.AppService;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：CourseOnTeachers 
    /// </summary>
    public class CourseOnTeachersAppService : BaseAppService<CourseOnTeachers>
    {
	   private readonly ICourseOnTeachersRepository _iCourseOnTeachersRepository;
        private readonly FileRelationshipAppService _fileRelationshipAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly ProjectAppService _projectAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iCourseOnTeachersRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public CourseOnTeachersAppService(ICourseOnTeachersRepository iCourseOnTeachersRepository,OperatorLogAppService operatorLogAppService,FileRelationshipAppService fileRelationshipAppService ,ProjectAppService projectAppService) : base(iCourseOnTeachersRepository)
	   {
			_iCourseOnTeachersRepository = iCourseOnTeachersRepository;
			_operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
            _projectAppService = projectAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：CourseOnTeachers 
       /// </summary>
	   public  List<CourseOnTeachersOutput> GetList(CourseOnTeachersListInput input, out int count)
	   {
		  const string sql = "select  a.*,b.ProjectName ";
          var strSql = new StringBuilder( " from t_Course_OnTeachers  a  left join dbo.t_Base_Project b ON a.ProjectId  =b.Id  where a.IsDelete=0  " );
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.TeachersName))
          {
              strSql.Append( " and a.TeachersName like @TeachersName " );
              dynamicParameters.Add( ":TeachersName", "%"+input.TeachersName+"%", DbType.String);
          }
            if ( !string.IsNullOrEmpty( input.ProjectId ) ) {
                strSql.Append( "and a.ProjectId like @ProjectId ");
                dynamicParameters.Add(":ProjectId" ,"%"+input.ProjectId+"%",DbType.String);
            }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<CourseOnTeachersOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            foreach ( var item in list ) {
                var projectName = "";
                if ( !string.IsNullOrEmpty( item.ProjectId ) ) {
                    var arrId = item.ProjectId.Split( ',');
                    for ( var i = 0; i < arrId.Length; i++ ) {
                        projectName += _projectAppService.Get( arrId[i] ).ProjectName + ",";
                    }
                }
                item.ProjectName = projectName.TrimEnd( ',');
            }
          return list;
	   }

	   /// <summary>
        /// 新增实体  CourseOnTeachers
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseOnTeachersInput input)
        {
            CourseOnTeachers model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseOnTeachersRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                model.TeachersName = input.TeachersName;
                model.TheLabel = input.TheLabel;
                model.Synopsis = input.Synopsis;
                model.UpdateUserId = UserObject.Id;
                model.IsFamous = input.IsFamous;
                model.TeachStyle = input.TeachStyle;
                model.ProjectId = input.ProjectId;
                model.TeacherPhoto = input.IdFilehiddenFile;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iCourseOnTeachersRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseOnTeachers,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = string.Format("修改主讲教师:(0):(1)",model.TeachersName,model.Id)
                });
                _fileRelationshipAppService.Add( new FileRelationshipInput
                {
                    ModuleId = input.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                    Type = 0//图片
                } );
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseOnTeachers>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.TeachersName = input.TeachersName;
            model.TeacherPhoto = input.IdFilehiddenFile;
            model.TheLabel = input.TheLabel;
            model.Synopsis = input.Synopsis;
            model.IsFamous = input.IsFamous;
            model.TeachStyle = input.TeachStyle;
            model.ProjectId = input.ProjectId;
            var keyId = _iCourseOnTeachersRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseOnTeachers,
                OperatorType =(int) OperatorType.Add,
                Remark = string.Format( "新增主讲教师:(0):(1)", model.TeachersName, model.Id )
            } );
            _fileRelationshipAppService.Add( new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = input.IdFilehiddenFile,
                Type = 0//图片
            } );
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

