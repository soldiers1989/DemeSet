
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
    /// 数据表实体应用服务现实：FileRelationship 
    /// </summary>
    public class FileRelationshipAppService : BaseAppService<FileRelationship>
    {
        private readonly IFileRelationshipRepository _iFileRelationshipRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iFileRelationshipRepository"></param>
        public FileRelationshipAppService(IFileRelationshipRepository iFileRelationshipRepository) : base(iFileRelationshipRepository)
        {
            _iFileRelationshipRepository = iFileRelationshipRepository;
        }

        /// <summary>
        /// 查询列表实体：FileRelationship 
        /// </summary>
        public List<FileRelationshipOutput> GetList(FileRelationshipListInput input)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Base_FileRelationship  a  where 1=1  ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.ModuleId))
            {
                strSql.Append(" and a.ModuleId = @ModuleId ");
                dynamicParameters.Add(":ModuleId", input.ModuleId, DbType.String);
            }
            if (input.Type.HasValue)
            {
                strSql.Append(" and a.Type = @Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.String);
            }
            var list = Db.QueryList<FileRelationshipOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  FileRelationship
        /// </summary>
        public void Add(FileRelationshipInput input)
        {
            if (!string.IsNullOrEmpty(input.ModuleId))
            {
                var dynamicParameters = new DynamicParameters();
                if (input.Type.HasValue && input.Type > 0)
                {
                    dynamicParameters.Add(":Type", input.Type, DbType.Int32);
                    dynamicParameters.Add(":ModuleId", input.ModuleId, DbType.String);
                    Db.Execute(" delete t_Base_FileRelationship where ModuleId=@ModuleId and  Type=@Type ", dynamicParameters);
                }
                else
                {
                    dynamicParameters.Add(":ModuleId", input.ModuleId, DbType.String);
                    Db.Execute(" delete t_Base_FileRelationship where ModuleId=@ModuleId  ", dynamicParameters);
                }
            }
            var model = input.MapTo<FileRelationship>();
            model.CreateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(input.IdFilehiddenFile))
            {
                var fileList = input.IdFilehiddenFile.Split(',');
                foreach (var item in fileList)
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.QlyName = item;
                    _iFileRelationshipRepository.Insert(model);
                }
            }
        }
    }
}

