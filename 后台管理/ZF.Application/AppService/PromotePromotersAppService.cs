
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

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：推广员表 
    /// </summary>
    public class PromotePromotersAppService : BaseAppService<PromotePromoters>
    {
        public string SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];

        private readonly IPromotePromotersRepository _iPromotePromotersRepository;

        private readonly IPromoteCompanyRepository _promoteCompanyRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPromotePromotersRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="promoteCompanyRepository"></param>
        public PromotePromotersAppService(IPromotePromotersRepository iPromotePromotersRepository, OperatorLogAppService operatorLogAppService, IPromoteCompanyRepository promoteCompanyRepository) : base(iPromotePromotersRepository)
        {
            _iPromotePromotersRepository = iPromotePromotersRepository;
            _operatorLogAppService = operatorLogAppService;
            _promoteCompanyRepository = promoteCompanyRepository;
        }

        /// <summary>
        /// 查询列表实体：推广员表 
        /// </summary>
        public List<PromotePromotersOutput> GetList(PromotePromotersListInput input, out int count)
        {
            string sql = "select  a.*,b.Name as CompanyName ,'" + SiteDomain + "?PromotionCode='+a.PromotionCode as PromotionCodeUrl ";
            var strSql = new StringBuilder(" from t_Promote_Promoters  a left join t_Promote_Company b  on a.CompanyId=b.Id  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                strSql.Append(" and a.Name like  @Name ");
                dynamicParameters.Add(":Name", '%' + input.Name + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.CompanyId))
            {
                strSql.Append(" and a.CompanyId = @CompanyId ");
                dynamicParameters.Add(":CompanyId", input.CompanyId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PromotePromotersOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  推广员表
        /// </summary>
        public MessagesOutPut AddOrEdit(PromotePromotersInput input)
        {
            PromotePromoters model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iPromotePromotersRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.CompanyId = input.CompanyId;
                model.Contact = input.Contact;
                model.Name = input.Name;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iPromotePromotersRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PromotePromoters,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改推广员表:" + model.Name
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<PromotePromoters>();
            var promoteCompany = _promoteCompanyRepository.Get(model.CompanyId);
            model.PromotionCode = GetRandomString(6);
            model.CommissionRatio = promoteCompany.CommissionRatio;
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iPromotePromotersRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PromotePromoters,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增推广员表:" + model.Name
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取随机码
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="availableChars">指定随机字符，为空，默认系统指定</param>
        /// <returns></returns>
        private string GetRandomString(int length, string availableChars = null)
        {
            if (availableChars == null) availableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var id = new char[length];
            Random random = new Random();
            for (var i = 0; i < length; i++)
            {
                id[i] = availableChars[random.Next(0, availableChars.Length)];
            }

            return new string(id);
        }

    }
}



