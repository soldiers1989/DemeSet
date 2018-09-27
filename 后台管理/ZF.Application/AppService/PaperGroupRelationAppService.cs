
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
    /// 数据表实体应用服务现实：试卷组试卷关系表 
    /// </summary>
    public class PaperGroupRelationAppService : BaseAppService<PaperGroupRelation>
    {
        private readonly IPaperGroupRelationRepository _iPaperGroupRelationRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPaperGroupRelationRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PaperGroupRelationAppService(IPaperGroupRelationRepository iPaperGroupRelationRepository, OperatorLogAppService operatorLogAppService) : base(iPaperGroupRelationRepository)
        {
            _iPaperGroupRelationRepository = iPaperGroupRelationRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：角色人员关系表 
        /// </summary>
        public List<PaperGroupRelationOutput> GetList(PaperGroupRelationListInput input, out int count)
        {
            const string sql = @"select  a.*,  b.PaperName ,b.TestTime         ";
           
            var strSql = new StringBuilder(@"FROM    t_Paper_GroupRelation a
        LEFT JOIN t_Paper_Info b ON a.PaperId = b.Id
WHERE   b.IsDelete = 0 and a.PaperGroupId=@PaperGroupId ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":PaperGroupId", input.PaperGroupId, DbType.String);
            if (!string.IsNullOrEmpty(input.PaperName))
            {
                strSql.Append(" and b.PaperName like @PaperName");
                dynamicParameters.Add(":PaperName", '%' + input.PaperName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperGroupRelationOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }


        /// <summary>
        /// 查询列表实体：角色人员关系表 
        /// </summary>
        public List<PaperGroupRelationOutput> GetList1(PaperGroupRelationListInput input, out int count)
        {
            const string sql = @"SELECT b.Id, b.Id AS PaperId ,
        b.PaperName ,
        b.TestTime 
        ";
            var strSql = new StringBuilder(@" FROM    t_Paper_Info b
WHERE   1 = 1
         AND b.State = 1
        AND b.IsDelete = 0
        AND b.Id NOT IN (
        SELECT  a.PaperId
        FROM    t_Paper_GroupRelation a
        WHERE   a.PaperGroupId = @PaperGroupId )
        AND b.SubjectId IN (
        SELECT  c.SubjectId
        FROM    t_Paper_Group c
        WHERE   c.Id = @PaperGroupId )
        AND b.Type IN (
        SELECT  c.Type
        FROM    t_Paper_Group c
        WHERE   c.Id = @PaperGroupId )");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":PaperGroupId", input.PaperGroupId, DbType.String);
            if (!string.IsNullOrEmpty(input.PaperName))
            {
                strSql.Append(" and b.PaperName like @PaperName");
                dynamicParameters.Add(":PaperName", '%' + input.PaperName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperGroupRelationOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  试卷组试卷关系表
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperGroupRelationInput input)
        {
            if (!string.IsNullOrEmpty(input.PaperIds))
            {
                var ids = input.PaperIds;
                var list = ids.Split(',');
                foreach (var item in list.Where(item => !string.IsNullOrEmpty(item)))
                {
                    _iPaperGroupRelationRepository.Insert(new PaperGroupRelation
                    {
                        Id = Guid.NewGuid().ToString(),
                        PaperId = item,
                        PaperGroupId = input.PaperGroupId,
                    });
                }
            }
            return new MessagesOutPut
            {
                Id = -1,
                Message = "保存成功!",
                Success = true
            };
        }

    }
}

