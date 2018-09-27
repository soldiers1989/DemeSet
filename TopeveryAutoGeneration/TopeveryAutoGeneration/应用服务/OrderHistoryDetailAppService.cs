
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
    /// 数据表实体应用服务现实：订单明细表(历史表) 
    /// </summary>
    public class OrderHistoryDetailAppService : BaseAppService<OrderHistoryDetail>
    {
	   private readonly IOrderHistoryDetailRepository _iOrderHistoryDetailRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iOrderHistoryDetailRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public OrderHistoryDetailAppService(IOrderHistoryDetailRepository iOrderHistoryDetailRepository,OperatorLogAppService operatorLogAppService): base(iOrderHistoryDetailRepository)
	   {
			_iOrderHistoryDetailRepository = iOrderHistoryDetailRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：订单明细表(历史表) 
       /// </summary>
	   public  List<OrderHistoryDetailOutput> GetList(OrderHistoryDetailListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Order_HistoryDetail  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<OrderHistoryDetailOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  订单明细表(历史表)
        /// </summary>
        public MessagesOutPut AddOrEdit(OrderHistoryDetailInput input)
        {
            OrderHistoryDetail model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iOrderHistoryDetailRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iOrderHistoryDetailRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.OrderHistoryDetail,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改订单明细表(历史表):"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<OrderHistoryDetail>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iOrderHistoryDetailRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OrderHistoryDetail,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增订单明细表(历史表):" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

