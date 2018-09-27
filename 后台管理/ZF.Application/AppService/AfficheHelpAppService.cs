
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// 数据表实体应用服务现实：资讯,帮助管理表 
    /// </summary>
    public class AfficheHelpAppService : BaseAppService<AfficheHelp>
    {

        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        private readonly IAfficheHelpRepository _iAfficheHelpRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly FileRelationshipAppService _fileRelationshipAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iAfficheHelpRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        public AfficheHelpAppService(IAfficheHelpRepository iAfficheHelpRepository, OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService) : base(iAfficheHelpRepository)
        {
            _iAfficheHelpRepository = iAfficheHelpRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>
        public List<AfficheHelpOutput> GetList(AfficheHelpListInput input, out int count)
        {
            const string sql = "select  a.*,b.Name,c.DataTypeName ";
            var strSql = new StringBuilder(" from T_Base_AfficheHelp  a left join t_Base_Basedata  b on a.ClassId=b.Id left join t_Base_DataType c on a.BigClassId=c.Id   where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.BigClassId))
            {
                strSql.Append(" and a.BigClassId = @BigClassId ");
                dynamicParameters.Add(":BigClassId", input.BigClassId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ClassId))
            {
                strSql.Append(" and a.ClassId = @ClassId ");
                dynamicParameters.Add(":ClassId", input.ClassId, DbType.String);
            }
            if (input.Type.HasValue)
            {
                strSql.Append(" and a.Type = @Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.String);
            }
            if (input.IsIndex.HasValue)
            {
                strSql.Append(" and a.IsIndex = @IsIndex ");
                dynamicParameters.Add(":IsIndex", input.IsIndex, DbType.String);
            }
            if (input.IsTop.HasValue)
            {
                strSql.Append(" and a.IsTop = @IsTop ");
                dynamicParameters.Add(":IsTop", input.IsTop, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.Title))
            {
                strSql.Append(" and a.Title like @Title ");
                dynamicParameters.Add(":Title", '%' + input.Title + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<AfficheHelpOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  资讯,帮助管理表
        /// </summary>
        public MessagesOutPut AddOrEdit(AfficheHelpInput input)
        {
            RedisCacheHelper.Remove("GetAfficheHelp");
            AfficheHelp model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iAfficheHelpRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;

                model.BigClassId = input.BigClassId;
                model.ClassId = input.ClassId;
                model.Content = input.Content;
                model.IsIndex = input.IsIndex;
                model.IsTop = input.IsTop;
                model.Title = input.Title;
                model.Type = input.Type;
                model.AfficheIamge = DefuleDomain + "/" + input.IdFilehiddenFile;

                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iAfficheHelpRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.AfficheHelp,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改资讯,帮助管理表:" + model.Title
                });
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<AfficheHelp>();
            model.Id = Guid.NewGuid().ToString();
            model.AfficheIamge = DefuleDomain + "/" + input.IdFilehiddenFile;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iAfficheHelpRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.AfficheHelp,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增资讯,帮助管理表:" + model.Title
            });
            _fileRelationshipAppService.Add(new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = input.IdFilehiddenFile
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

