
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.Components.DictionaryAdapter;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.Enum;
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：SubjectClass 
    /// </summary>
    public class SubjectClassAppService : BaseAppService<SubjectClass>
    {
        private readonly ISubjectClassRepository _iSubjectClassRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSubjectClassRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public SubjectClassAppService(ISubjectClassRepository iSubjectClassRepository, OperatorLogAppService operatorLogAppService) : base(iSubjectClassRepository)
        {
            _iSubjectClassRepository = iSubjectClassRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 根据科目编号获取试题分类数据
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public List<SubjectClassOutput> GetList(string subjectId)
        {
            var strSql = new StringBuilder(@"SELECT  MAX(a.Id) Id,MAX(a.ClassName) ClassName
FROM    dbo.t_Subject_Class a
        INNER JOIN dbo.t_Base_Project b ON a.ProjectId = b.Id
        INNER JOIN dbo.t_Base_Subject c ON b.Id = c.ProjectId
        WHERE a.IsDelete = 0  ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(subjectId))
            {
                strSql.Append(" and c.Id = @Id ");
                dynamicParameters.Add(":Id", subjectId, DbType.String);
                return Db.QueryList<SubjectClassOutput>(strSql + "GROUP BY a.OrderNo ", dynamicParameters);
            }
            return new List<SubjectClassOutput>();

        }

        /// <summary>
        /// 查询列表实体：SubjectClass 
        /// </summary>
        public List<SubjectClassOutput> GetList(SubjectClassListInput input, out int count)
        {
            const string sql = "select  a.*,b.ProjectName ";
            var strSql = new StringBuilder(" from t_Subject_Class  a left join t_Base_Project b on a.ProjectId=b.Id  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.ClassName))
            {
                strSql.Append(" and a.ClassName like @ClassName ");
                dynamicParameters.Add(":ClassName", '%' + input.ClassName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectId))
            {
                strSql.Append(" and a.ProjectId = @ProjectId ");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SubjectClassOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);

            foreach (var item in list)
            {
                if (item.BigType.HasValue)
                    item.BigTypeName = EnumHelper.GetEnumName<QuestionType>(item.BigType.Value);
            }
            return list;
        }

        /// <summary>
        /// 根据科目试卷结构id查询试题分类信息
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<SubjectClassOutput> GetListByStructureId(string ProjectId)
        {

            if (!string.IsNullOrWhiteSpace(ProjectId))
            {
                const string sql = @"select Id,ClassName ";
                var dynamicParameters = new DynamicParameters();
                var strSql = new StringBuilder(@" from t_Subject_Class c where exists(
                          select b.Id from t_Base_Subject a
                          inner
                                      join t_Base_Project b
                                on a.ProjectId = b.Id
                          where c.ProjectId = b.Id
                          and a.Id in(select subjectid from t_Paper_Structure where id = @ProjectId)
                        )  
                        and IsDelete=0  ");
                dynamicParameters.Add(":ProjectId", ProjectId, DbType.String);
                var list = Db.QueryList<SubjectClassOutput>(sql + strSql, dynamicParameters);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断制定题型集合中是否存在试题
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int? GetCount(string ids)
        {
            var sql = @"SELECT COUNT(1) FROM dbo.t_Subject_BigQuestion WHERE 1=1 AND IsDelete=0 AND SubjectClassId IN(" + ids + ")";
            return Db.QueryFirstOrDefault<int>(sql, null);
        }

        /// <summary>
        /// 新增实体  SubjectClass
        /// </summary>
        public MessagesOutPut AddOrEdit(SubjectClassInput input)
        {

            SubjectClass model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iSubjectClassRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;

                model.ClassName = input.ClassName;
                model.Column_6 = input.Column_6;
                model.OrderNo = input.OrderNo;
                model.ProjectId = input.ProjectId;
                model.Remark = input.Remark;

                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iSubjectClassRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectClass,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改试题分类:" + model.ClassName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SubjectClass>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iSubjectClassRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SubjectClass,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增试题分类:" + model.ClassName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

