
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
using ZF.Infrastructure.RandomHelper;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：购物车表 
    /// </summary>
    public class OrderCartAppService : BaseAppService<OrderCart>
    {
	   private readonly IOrderCartRepository _iOrderCartRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iOrderCartRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public OrderCartAppService(IOrderCartRepository iOrderCartRepository,OperatorLogAppService operatorLogAppService): base(iOrderCartRepository)
	   {
			_iOrderCartRepository = iOrderCartRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：购物车表 
       /// </summary>
	   public  List<OrderCartOutput> GetList(OrderCartListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Order_Cart  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.RegisterUserId))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.RegisterUserId, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<OrderCartOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  购物车表
        /// </summary>
        public MessagesOutPut AddOrEdit(OrderCartInput input)
        {
            OrderCart model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iOrderCartRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                #endregion
                _iOrderCartRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.OrderCart,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改购物车表:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<OrderCart>();
			model.Id = Guid.NewGuid().ToString();
            var keyId = _iOrderCartRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OrderCart,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增购物车表:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 购物车写入方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddCartInfo(OrderCartInput input)
        {
            StringBuilder sqlList = new StringBuilder();
            sqlList.Append(" insert into t_Order_Cart(Id,OrderNo,RegisterUserId) values(@id,@orderNo,@RegisterUserId); ");
            sqlList.Append(" insert into t_Order_CartDetail(Id,OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType)values(@CartDetailId,@DetailOrderNo,@CourseId,@Price,@FavourablePrice,@Num,@Amount,@CourseType) ");
            var dynamicParameters = new DynamicParameters();
            string cartId = Guid.NewGuid().ToString();
            //订单主表
            dynamicParameters.Add(":id", cartId, DbType.String);
            dynamicParameters.Add(":orderNo", DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString(), DbType.String);
            dynamicParameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
            //订单明细表
            dynamicParameters.Add(":CartDetailId", Guid.NewGuid().ToString(), DbType.String);
            dynamicParameters.Add(":DetailOrderNo", cartId, DbType.String);
            dynamicParameters.Add("CourseId", input.DetailOrderNo, DbType.String);
            dynamicParameters.Add(":Price", input.Price, DbType.Decimal);
            dynamicParameters.Add(":FavourablePrice", input.FavourablePrice, DbType.Decimal);
            dynamicParameters.Add(":Num", input.Num, DbType.Int32);
            dynamicParameters.Add(":Amount", input.Amount, DbType.Decimal);
            dynamicParameters.Add(":CourseType", input.CourseType, DbType.Int32);

            int count = Db.ExecuteNonQuery(sqlList.ToString(), dynamicParameters);

            if (count > 0)
            {
                return new MessagesOutPut { Success = true, Message = "新增成功!" };
            }
            else
            {
                return new MessagesOutPut { Success = true, Message = "新增成功!" };
            }
        }
	   
    }
}

