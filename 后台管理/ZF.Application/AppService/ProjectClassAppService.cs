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
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 吴永福
    /// 2018-03-09
    /// 项目分类
    /// </summary>
    public class ProjectClassAppService : BaseAppService<ProjectClass>
    {
        private readonly IProjectClassRepository _repository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        public ProjectClassAppService(IProjectClassRepository repository, OperatorLogAppService operatorLogAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 新增项目分类  t_Base_ProjectClass
        /// </summary>
        public MessagesOutPut AddOrEdit(ProjectClassInput input)
        {
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            ProjectClass model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.OrderNo = input.OrderNo;
                model.ProjectClassName = input.ProjectClassName;
                model.Remark = input.Remark;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.ProjectClass,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改项目分类:" + model.ProjectClassName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<ProjectClass>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.ProjectClass,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增项目分类:" + model.ProjectClassName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取项目分类集合
        /// </summary>
        public List<ProjectClassOutput> GetList(ProjectClassListInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_ProjectClass  where IsDelete=0 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrWhiteSpace(input.ProjectClassName))
            {
                strSql.Append(" and ProjectClassName like  @ProjectClassName ");
                dynamicParameters.Add(":ProjectClassName", '%' + input.ProjectClassName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<ProjectClassOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }
    }
}