
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
    /// 数据表实体应用服务现实：购物车明细表 
    /// </summary>
    public class OrderCartDetailAppService : BaseAppService<OrderCartDetail>
    {
	   private readonly IOrderCartDetailRepository _iOrderCartDetailRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iOrderCartDetailRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public OrderCartDetailAppService(IOrderCartDetailRepository iOrderCartDetailRepository,OperatorLogAppService operatorLogAppService): base(iOrderCartDetailRepository)
	   {
			_iOrderCartDetailRepository = iOrderCartDetailRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：购物车明细表 
       /// </summary>
	   public  List<OrderCartDetailOutput> GetList(OrderCartDetailListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Order_CartDetail  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.OrderNo))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.OrderNo, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<OrderCartDetailOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  购物车明细表
        /// </summary>
        public MessagesOutPut AddOrEdit(OrderCartDetailInput input)
        {
            OrderCartDetail model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iOrderCartDetailRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                #endregion
                _iOrderCartDetailRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.OrderCartDetail,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改购物车明细表:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<OrderCartDetail>();
			model.Id = Guid.NewGuid().ToString();
            var keyId = _iOrderCartDetailRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OrderCartDetail,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增购物车明细表:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

