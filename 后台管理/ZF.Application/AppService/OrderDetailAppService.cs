
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
    /// 数据表实体应用服务现实：订单明细表 
    /// </summary>
    public class OrderDetailAppService : BaseAppService<OrderDetail>
    {
        private readonly IOrderDetailRepository _iOrderDetailRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iOrderDetailRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public OrderDetailAppService(IOrderDetailRepository iOrderDetailRepository, OperatorLogAppService operatorLogAppService) : base(iOrderDetailRepository)
        {
            _iOrderDetailRepository = iOrderDetailRepository;
            _operatorLogAppService = operatorLogAppService;
        }


        /// <summary>
        /// 通过订单编号获取订单课程明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<OrderDetailOutput> GetOrderCourseList(IdInput input)
        {
            var strsql = @" SELECT  CASE a.CourseType 
          WHEN 1 THEN '套餐'
          WHEN 0 THEN '课程'
          ELSE ''
        END AS CourseTypeName,
        a.*,
        b.CourseName
FROM    dbo.t_Order_Detail a
        LEFT JOIN dbo.V_Course_Packcourse_Info b ON a.CourseId = b.Id where 1=1  ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                strsql += " and a.OrderNo =@Id ";
                dynamicParameters.Add(":Id", input.Id, DbType.String);
                return Db.QueryList<OrderDetailOutput>(strsql, dynamicParameters);
            }
            return new List<OrderDetailOutput>();
        }


        /// <summary>
        /// 新增实体  订单明细表
        /// </summary>
        public MessagesOutPut AddOrEdit(OrderDetailInput input)
        {
            OrderDetail model;
            model = input.MapTo<OrderDetail>();
            model.Id = Guid.NewGuid().ToString();
            var keyId = _iOrderDetailRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OrderDetail,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增订单明细表:订单编号" + model.OrderNo
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

