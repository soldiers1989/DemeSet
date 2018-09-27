
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

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：CourseResource 
    /// </summary>
    public class CourseResourceAppService : BaseAppService<CourseResource>
    {
	   private readonly ICourseResourceRepository _iCourseResourceRepository;
        private readonly FileRelationshipAppService _fileRelationshipAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseResourceRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// /// <param name="fileRelationshipAppService"></param>
        public CourseResourceAppService(ICourseResourceRepository iCourseResourceRepository,OperatorLogAppService operatorLogAppService,FileRelationshipAppService fileRelationshipAppService ) : base(iCourseResourceRepository)
	   {
			_iCourseResourceRepository = iCourseResourceRepository;
			_operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：CourseResource 
       /// </summary>
	   public  List<CourseResourceOutput> GetList(CourseResourceListInput input,out int count )
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder( " from t_Course_Resource  a   where a.IsDelete=0  " );
            var sqlCount = " select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.CourseId))
          {
              strSql.Append( " and a.CourseId = @CourseId " );
              dynamicParameters.Add( ":CourseId", input.CourseId, DbType.String);
          }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<CourseResourceOutput>( GetPageSql( sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            return list;
	   }

	   /// <summary>
        /// 新增实体  CourseResource
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseResourceInput input)
        {
            CourseResource model;
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                model = _iCourseResourceRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                model.ResourceName = input.ResourceName;
                //配合上传组件
                model.ResourceUrl = input.IdFilehiddenFile;
                model.ResourceSize = input.ResourceSize;
                model.CourseId = input.CourseId;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iCourseResourceRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseResource,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = string.Format( "修改课程资源:{0}-{1}", model.CourseId, model.ResourceName )
                } );

                if ( !string.IsNullOrWhiteSpace( input.IdFilehiddenFile ) )
                {
                    _fileRelationshipAppService.Add( new FileRelationshipInput
                    {
                        ModuleId = model.Id,
                        IdFilehiddenFile = input.IdFilehiddenFile,
                    } );
                }
            return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseResource>();
			model.Id = Guid.NewGuid().ToString();
            model.ResourceName = input.ResourceName;
            model.ResourceUrl = input.IdFilehiddenFile;
            model.ResourceSize = input.ResourceSize;
            model.CourseId = input.CourseId;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iCourseResourceRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseResource,
                OperatorType =(int) OperatorType.Add,
                Remark = string.Format( "添加课程资源:{0}-{1}", model.CourseId, model.ResourceName )
            } );

            if ( !string.IsNullOrWhiteSpace( input.IdFilehiddenFile ) )
            {
                _fileRelationshipAppService.Add( new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                } );
            }
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

