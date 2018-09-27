
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.TwoDimensionalCode;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：下单机构表 
    /// </summary>
    public class OrderInstitutionsAppService : BaseAppService<OrderInstitutions>
    {

        private static readonly string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private static readonly string InstitutionsUrl = ConfigurationManager.AppSettings["InstitutionsUrl"];



        private readonly IOrderInstitutionsRepository _iOrderInstitutionsRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iOrderInstitutionsRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public OrderInstitutionsAppService(IOrderInstitutionsRepository iOrderInstitutionsRepository, OperatorLogAppService operatorLogAppService) : base(iOrderInstitutionsRepository)
        {
            _iOrderInstitutionsRepository = iOrderInstitutionsRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：下单机构表 
        /// </summary>
        public List<OrderInstitutionsOutput> GetList(OrderInstitutionsListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Order_Institutions  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                strSql.Append(" and a.Name = @Name ");
                dynamicParameters.Add(":Name", input.Name, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<OrderInstitutionsOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  下单机构表
        /// </summary>
        public MessagesOutPut AddOrEdit(OrderInstitutionsInput input)
        {
            OrderInstitutions model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iOrderInstitutionsRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;

                model.Name = input.Name;
                model.Contact = input.Contact;
                model.ContactAddress = input.ContactAddress;
                model.ContactPhone = input.ContactPhone;
                model.Remark = input.Remark;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;

                Bitmap bt1;
                bt1 = QRCodeHelper.Create(InstitutionsUrl + model.Id, 6);
                Stream stream1 = new MemoryStream(CreateTwoDimensionalCode.Bitmap2Byte(bt1));
                var isok1 = AliyunFileUpdata.ResumeUploader(stream1, model.Id + ".jpg");
                if (isok1)
                {
                    model.Url = DefuleDomain + "/" + model.Id + ".jpg";
                }
                #endregion
                _iOrderInstitutionsRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.OrderInstitutions,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改下单机构表:" + input.Name
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<OrderInstitutions>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            Bitmap bt;
            bt = QRCodeHelper.Create(InstitutionsUrl + model.Id, 6);
            Stream stream = new MemoryStream(CreateTwoDimensionalCode.Bitmap2Byte(bt));
            var isok = AliyunFileUpdata.ResumeUploader(stream, model.Id + ".jpg");
            if (isok)
            {
                model.Url = DefuleDomain + "/" + model.Id + ".jpg";
            }
            var keyId = _iOrderInstitutionsRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OrderInstitutions,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增下单机构表:" + input.Name
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

