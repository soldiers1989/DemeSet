
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
    /// 数据表实体应用服务现实：文本配置表(联系我们,注册协议) 
    /// </summary>
    public class DanyeAppService : BaseAppService<Danye>
    {
        private readonly IDanyeRepository _iDanyeRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iDanyeRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public DanyeAppService(IDanyeRepository iDanyeRepository, OperatorLogAppService operatorLogAppService) : base(iDanyeRepository)
        {
            _iDanyeRepository = iDanyeRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：文本配置表(联系我们,注册协议) 
        /// </summary>
        public List<DanyeOutput> GetList(DanyeListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from T_Base_Danye  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                strSql.Append(" and a.Name = @Name ");
                dynamicParameters.Add(":Name", input.Name, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<DanyeOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Danye GetOne(DanyeInput input)
        {
            var strsql = " select * from T_Base_Danye where Code= @Code";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Code", input.Code, DbType.String);
            Danye model = Db.QueryFirstOrDefault<Danye>(strsql, dynamicParameters);
            return model;
        }

        /// <summary>
        /// 新增实体  文本配置表(联系我们,注册协议)
        /// </summary>
        public MessagesOutPut AddOrEdit(DanyeInput input)
        {
            var strsql = " select * from T_Base_Danye where Code= @Code";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Code", input.Code, DbType.String);
            Danye model = Db.QueryFirstOrDefault<Danye>(strsql, dynamicParameters);
            if (model != null)
            {
                model = _iDanyeRepository.Get(model.Id);
                #region 修改逻辑
                model.Content = input.Content;

                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iDanyeRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.Danye,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "更新成功" + input.Code + input.Name
                });
                return new MessagesOutPut { Success = true, Message = "更新成功!" };
            }
            model = input.MapTo<Danye>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iDanyeRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.Danye,
                OperatorType = (int)OperatorType.Add,
                Remark = "更新成功" + input.Code + input.Name
            });
            return new MessagesOutPut { Success = true, Message = "更新成功!" };
        }

    }
}

