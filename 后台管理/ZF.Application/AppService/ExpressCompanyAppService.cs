
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
    /// 数据表实体应用服务现实：快递公司 
    /// </summary>
    public class ExpressCompanyAppService : BaseAppService<ExpressCompany>
    {
	   private readonly IExpressCompanyRepository _iExpressCompanyRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iExpressCompanyRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public ExpressCompanyAppService(IExpressCompanyRepository iExpressCompanyRepository,OperatorLogAppService operatorLogAppService): base(iExpressCompanyRepository)
	   {
			_iExpressCompanyRepository = iExpressCompanyRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：快递公司 
       /// </summary>
	   public  List<ExpressCompanyOutput> GetList(ExpressCompanyListInput input, out int count)
	   {
		  const string sql = "select  a.Id,a.Name,a.Companyurl,a.AddTime,a.AddUser,a.UpdateTime,a.UpdateUser,a.IsDelete,isnull(a.IsDefault,0)IsDefault ";
          var strSql = new StringBuilder(" from ExpressCompany  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.AddUser, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<ExpressCompanyOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

        /// <summary>
        /// 修改快递公司是否是默认
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateDefault(IdInput input)
        {
            string sql = string.Empty;
            try
            {
                sql = "  update ExpressCompany set IsDefault = 0 ";
                var count = Db.ExecuteNonQuery(sql, null);
                if (count > 0)
                {
                    sql = " update ExpressCompany set IsDefault = 1 where Id = @Id; ";
                    var parament = new DynamicParameters();
                    parament.Add(":Id", input.Id, DbType.String);
                    Db.ExecuteNonQuery(sql, parament);
                }
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = input.Id,
                    ModuleId = (int)Model.ExpressCompany,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改默认快递公司:"
                });
                return new MessagesOutPut { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = input.Id,
                    ModuleId = (int)Model.ExpressCompany,
                    OperatorType = (int)OperatorType.Add,
                    Remark = "修改默认快递公司失败，异常信息是:" + ex.Message
                });
                return new MessagesOutPut { Success = true, Message = "设置失败" };
            }
        }


	   /// <summary>
        /// 新增实体  快递公司
        /// </summary>
        public MessagesOutPut AddOrEdit(ExpressCompanyInput input)
        {
            ExpressCompany model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iExpressCompanyRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;
                model.UpdateUser = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                model.Name = input.Name;
                model.Companyurl = input.Companyurl;
                #endregion
                _iExpressCompanyRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.ExpressCompany,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改快递公司:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<ExpressCompany>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUser = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iExpressCompanyRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.ExpressCompany,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增快递公司:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(ExpressCompanyInput input)
        {
            try
            {
                ExpressCompany model = _iExpressCompanyRepository.Get(input.Id);
                _iExpressCompanyRepository.LogicDelete(model);
                return new MessagesOutPut { Success = true, Message = "删除成功" };
            }
            catch (Exception)
            {
                return new MessagesOutPut { Success = false, Message = "删除失败" };
            }
        }

        /// <summary>
        /// 查询单个信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ExpressCompany GetOne(IdInput input)
        {
            var model = _iExpressCompanyRepository.Get(input.Id);
            return model;
        }
    }
}

