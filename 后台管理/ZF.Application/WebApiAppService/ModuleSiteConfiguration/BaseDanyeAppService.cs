using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.ModuleSiteConfiguration;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.WebApiAppService.ModuleSiteConfiguration
{
    /// <summary>
    /// 文本配置Api服务
    /// </summary>
    public class BaseDanyeAppService : BaseAppService<Danye>
    {
        private readonly IDanyeRepository _repository;

        private readonly IMyFeedbackRepository _myFeedbackRepository;

        private readonly IAfficheHelpRepository _afficheHelpRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="myFeedbackRepository"></param>
        public BaseDanyeAppService(IDanyeRepository repository, IMyFeedbackRepository myFeedbackRepository, IAfficheHelpRepository afficheHelpRepository) : base(repository)
        {
            _repository = repository;
            _myFeedbackRepository = myFeedbackRepository;
            _afficheHelpRepository = afficheHelpRepository;
        }

        /// <summary>
        /// 通过编码获取相关配置文本
        /// </summary>
        /// <param name="arguName"></param>
        /// <returns></returns>
        public MessagesOutPut GetArguValue(string arguName)
        {
            var strSql = " select ArguValue from T_Base_SysSet where 1=1 and IsDelete=0 ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(arguName))
            {
                if (RedisCacheHelper.Exists(arguName))
                {
                    return new MessagesOutPut { Id = -1, Message = RedisCacheHelper.Get<string>(arguName), Success = true };
                }
                strSql += " and ArguName=@ArguName ";
                dy.Add(":ArguName", arguName, DbType.String);
                var model = Db.QueryFirstOrDefault<string>(strSql, dy);
                RedisCacheHelper.Add(arguName, model);
                return new MessagesOutPut { Id = -1, Message = model, Success = true };
            }
            return new MessagesOutPut { Id = -1, Message = "查询失败", Success = false };
        }

        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>
        public List<AfficheHelpOutput> GetList(AfficheHelpListInput input, out int count)
        {
            string sql = $"select  a.*,b.DataTypeName,c.Name ";
            var strSql = new StringBuilder(" from T_Base_AfficheHelp  a LEFT JOIN dbo.t_Base_DataType b ON a.BigClassId=b.Id LEFT JOIN dbo.t_Base_Basedata c ON a.ClassId=c.Id   where a.IsDelete=0  ");
            var dynamicParameters = new DynamicParameters();
            if (input.Type.HasValue)
            {
                strSql.Append(" and a.Type = @Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.String);
            }
            if (input.IsIndex.HasValue)
            {
                strSql.Append(" and a.IsIndex = @IsIndex ");
                dynamicParameters.Add(":IsIndex", input.IsIndex, DbType.Int32);
            }
            if (!string.IsNullOrEmpty(input.BigClassId))
            {
                strSql.Append(" and a.BigClassId = @BigClassId ");
                dynamicParameters.Add(":BigClassId", input.BigClassId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.Id))
            {
                strSql.Append(" and a.Id != @Id ");
                dynamicParameters.Add(":Id", input.Id, DbType.String);

                var model = _afficheHelpRepository.Get(input.Id);
                strSql.Append(" and a.BigClassId = @BigClassId1 ");
                dynamicParameters.Add(":BigClassId1", model.BigClassId, DbType.String);
            }
            var list = Db.QueryList<AfficheHelpOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            count = Db.QueryFirstOrDefault<int>(" select count(1) from (" + sql + strSql + ")  b", dynamicParameters);
            return list;
        }

        /// <summary>
        /// 提交意见反馈
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool InsertFeedback(FeedbackMolde input)
        {
            var model = input.MapTo<MyFeedback>();
            model.AddTime = DateTime.Now;
            model.Id = Guid.NewGuid().ToString();
            _myFeedbackRepository.Insert(model);
            return true;
        }

        /// <summary>
        /// 获取字段分类
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<DataTypeOutput> GetDataType(string code)
        {
            string sql = $"select  a.* ";
            var strSql = new StringBuilder(" from t_Base_DataType  a   where a.IsDelete=0  ");
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(code))
            {
                strSql.Append(" and a.DataTypeCode = @DataTypeCode ");
                dy.Add(":DataTypeCode", code, DbType.String);
            }
            var list = Db.QueryList<DataTypeOutput>(sql + strSql + " Order by a.Sort Asc", dy);
            return list;
        }
    }
}