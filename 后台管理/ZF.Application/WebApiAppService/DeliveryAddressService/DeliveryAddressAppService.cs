using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.DeliveryAddressModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.WebApiAppService.DeliveryAddressService
{
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
        public DeliveryAddressAppService(IDeliveryAddressRepository iDeliveryAddressRepository, OperatorLogAppService operatorLogAppService) : base(iDeliveryAddressRepository)
        {
            _iDeliveryAddressRepository = iDeliveryAddressRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 新增实体  编码
        /// </summary>
        public MessagesOutPut AddOrEdit(DeliveryAddressModelInput input)
        {
            DeliveryAddress model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iDeliveryAddressRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.City = input.City;
                model.Contact = input.Contact;
                model.ContactPhone = input.ContactPhone;
                model.DetailedAddress = input.DetailedAddress;
                model.Province = input.Province;
                model.Town = input.Town;
                model.UserId = input.UserId;
                model.Zip =string.IsNullOrEmpty(input.Zip)?"": input.Zip;
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

            const string sql = " select count(1) pathCount from DeliveryAddress where userid = @userid  ";
            var paraments = new DynamicParameters();
            paraments.Add(":userid", input.UserId, DbType.String);

            int count = Db.ExecuteScalar<int>(sql, paraments);
            if (count >= 20)
            {
                return new MessagesOutPut { Success = false, Message = "收件地址超过20个无法再添加!" };
            }
            else
            {
                model = input.MapTo<DeliveryAddress>();
                model.Id = Guid.NewGuid().ToString();
                model.AddTime = DateTime.Now;
                var keyId = _iDeliveryAddressRepository.InsertGetId(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = keyId,
                    ModuleId = (int)Model.DeliveryAddress,
                    OperatorType = (int)OperatorType.Add,
                    Remark = "新增编码:"
                });
                return new MessagesOutPut { Success = true, Message = "新增成功!" };
            }
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EnditiDefaultAddress(DeliveryAddressModelInput input)
        {
            DeliveryAddress model;
            //先将其他的设置为0
            const string sql = " update DeliveryAddress set DefaultAddress=0 where userid = @userid ";
            var parmanet = new DynamicParameters();
            parmanet.Add(":userid", input.UserId, DbType.String);
            var count = Db.ExecuteNonQuery(sql, parmanet);
            if (count > 0)
            {
                model = _iDeliveryAddressRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.DefaultAddress = 1;
                #endregion
                _iDeliveryAddressRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.DeliveryAddress,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改编码:"
                });
                return new MessagesOutPut { Success = true, Message = "设置成功!" };
            }
            else
            {
                return new MessagesOutPut { Success = false, Message = "设置失败!" };
            }
        }

        /// <summary>
        /// 查询用户所有收件地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<DeliveryAddressModuleOutPut> GetList(DeliveryAddressModelInput input)
        {
            const string sql = " select * from DeliveryAddress where userid=@userid order by AddTime desc ";
            var parameters = new DynamicParameters();
            parameters.Add(":userid", input.UserId, DbType.String);
            var data = Db.QueryList<DeliveryAddressModuleOutPut>(sql, parameters);
            return data;
        }

        /// <summary>
        /// 删除地址信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut DelDeliveryAddress(IdInput input)
        {
            try
            {
                var model = _iDeliveryAddressRepository.Get(input.Id);
                _iDeliveryAddressRepository.Delete(model);
                return new MessagesOutPut { Success = true, Message = "删除成功!" };
            }
            catch (Exception)
            {
                return new MessagesOutPut { Success = false, Message = "删除失败!" };
            }
        }

        /// <summary>
        /// 查询单个地址详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DeliveryAddress GetOne(IdInput input)
        {
            DeliveryAddress data = _iDeliveryAddressRepository.Get(input.Id);
            return data;
        }
    }
}
