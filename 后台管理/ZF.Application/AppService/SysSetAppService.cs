
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
    /// 数据表实体应用服务现实：网站相关配置表 
    /// </summary>
    public class SysSetAppService : BaseAppService<SysSet>
    {
        private readonly ISysSetRepository _iSysSetRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSysSetRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public SysSetAppService(ISysSetRepository iSysSetRepository, OperatorLogAppService operatorLogAppService) : base(iSysSetRepository)
        {
            _iSysSetRepository = iSysSetRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：网站相关配置表 
        /// </summary>
        public List<SysSetOutput> GetList(SysSetListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from T_Base_SysSet  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                strSql.Append(" and a.Name = @Name ");
                dynamicParameters.Add(":Name", input.Name, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SysSetOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  网站相关配置表
        /// </summary>
        public MessagesOutPut AddOrEdit(SysSetInput input)
        {
            RedisCacheHelper.Remove("GetArguValue" + input.ArguName);
            RedisCacheHelper.Remove(input.ArguName);
            SysSet model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                RedisCacheHelper.Add(input.ArguName, input.ArguValue);
                model = _iSysSetRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;

                model.ArguValue = input.ArguValue;
                model.Remark = input.Remark;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iSysSetRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SysSet,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改网站相关配置表:" + input.ArguValue
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SysSet>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            RedisCacheHelper.Add(input.ArguName, input.ArguValue);
            var keyId = _iSysSetRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SysSet,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增网站相关配置表:" + input.ArguValue
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取ArguValue
        /// </summary>
        /// <param name="arguName"></param>
        /// <returns></returns>
        public string GetArguValue(string arguName)
        {
            string strSql = " SELECT ArguValue FROM  T_Base_SysSet where 1=1 ";
            var dy = new DynamicParameters();
            strSql += " and ArguName =@ArguName";
            dy.Add(":ArguName", arguName, DbType.String);
            return Db.QueryFirstOrDefault<string>(strSql, dy);
        }
    }
}

