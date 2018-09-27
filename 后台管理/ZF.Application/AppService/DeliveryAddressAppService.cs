
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
using System.Linq;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：编码 
    /// </summary>
    public class DeliveryAddressAppService : BaseAppService<DeliveryAddress>
    {
	   private readonly IDeliveryAddressRepository _iDeliveryAddressRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iDeliveryAddressRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public DeliveryAddressAppService(IDeliveryAddressRepository iDeliveryAddressRepository,OperatorLogAppService operatorLogAppService): base(iDeliveryAddressRepository)
	   {
			_iDeliveryAddressRepository = iDeliveryAddressRepository;
			_operatorLogAppService = operatorLogAppService;
	   }

        /// <summary>
        /// 查询列表实体：编码 
        /// </summary>
        public List<DeliveryAddressOutput> GetList(DeliveryAddressListInput input)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from v_DeliveryAddressList a ");
            //const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (input.SelectType == 0)
            {
                strSql.Append(" where ExpressCompanyId is null ");
            }
            else if (input.SelectType == 1)
            {
                strSql.Append(" where ExpressCompanyId is not null ");
            }

            //count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<DeliveryAddressOutput>(sql + strSql, dynamicParameters);
            var exList = Db.QueryList<Express>(" select Id,Name from ExpressCompany where IsDelete = 0 order by isnull(isDefault,0) desc ");
            if (input.SelectType == 0)
            {
                foreach (var item in list)
                {
                    item.ExpressList = exList;
                    item.SelectType = input.SelectType;
                }
            }
            else
            {
                foreach (var item in list)
                {
                    item.SelectType = input.SelectType;
                }

            }
           
            return list;
        }

	   /// <summary>
        /// 新增实体  编码
        /// </summary>
        public MessagesOutPut AddOrEdit(DeliveryAddressInput input)
        {
            DeliveryAddress model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iDeliveryAddressRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                #endregion
                _iDeliveryAddressRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.DeliveryAddress,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改编码:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<DeliveryAddress>();
			model.Id = Guid.NewGuid().ToString();
            model.AddTime = DateTime.Now;
            var keyId = _iDeliveryAddressRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.DeliveryAddress,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增编码:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

