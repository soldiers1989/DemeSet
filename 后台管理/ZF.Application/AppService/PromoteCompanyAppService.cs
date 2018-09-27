
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
    /// 数据表实体应用服务现实：推广公司表 
    /// </summary>
    public class PromoteCompanyAppService : BaseAppService<PromoteCompany>
    {
        private readonly IPromoteCompanyRepository _iPromoteCompanyRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPromoteCompanyRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PromoteCompanyAppService(IPromoteCompanyRepository iPromoteCompanyRepository, OperatorLogAppService operatorLogAppService) : base(iPromoteCompanyRepository)
        {
            _iPromoteCompanyRepository = iPromoteCompanyRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：推广公司表 
        /// </summary>
        public List<PromoteCompanyOutput> GetList(PromoteCompanyListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Promote_Company  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                strSql.Append(" and a.Name like @Name ");
                dynamicParameters.Add(":Name", '%' + input.Name + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PromoteCompanyOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  推广公司表
        /// </summary>
        public MessagesOutPut AddOrEdit(PromoteCompanyInput input)
        {
            PromoteCompany model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iPromoteCompanyRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.Name = input.Name;
                model.Address = input.Address;
                model.BankCard = input.BankCard;
                model.CommissionRatio = input.CommissionRatio;
                model.Contact = input.Contact;
                model.TheContact = input.TheContact;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iPromoteCompanyRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PromoteCompany,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改推广公司表:" + model.Name
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<PromoteCompany>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iPromoteCompanyRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PromoteCompany,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增推广公司表:" + model.Name
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

