using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Application.Dto;
using Dapper;
using System.Data;
using ZF.AutoMapper.AutoMapper;
using ZF.Application.BaseDto;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    public class DataTypeAppService : BaseAppService<DataType>
    {

        private readonly IDataTypeRepository _repository;
        private readonly OperatorLogAppService _operatorLogAppService;


        public DataTypeAppService(IDataTypeRepository repository, OperatorLogAppService operatorLogAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
        }



        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DataTypeOutput> GetList(DataTypeListInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_DataType  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";

            strSql.Append(" and IsDelete = 0 ");
            if (!string.IsNullOrEmpty(input.Id))
            {
                strSql.Append(" and Id = @Id ");
                dynamicParameters.Add(":Id", input.Id, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.DataTypeName))
            {
                strSql.Append(" and DataTypeName like  @DataTypeName ");
                dynamicParameters.Add(":DataTypeName", '%' + input.DataTypeName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.DataTypeCode))
            {
                strSql.Append(" and DataTypeCode =  @DataTypeCode ");
                dynamicParameters.Add(":DataTypeCode",  input.DataTypeCode, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<DataTypeOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);

            return list;
        }

        public MessagesOutPut AddOrEdit(DataTypeInput input)
        {
            DataType dataType;

            if (string.IsNullOrEmpty(input.Id))//新增
            {
                dataType = input.MapTo<DataType>();
                dataType.Id = Guid.NewGuid().ToString();
                dataType.AddUserId = UserObject.Id;
                dataType.AddTime = DateTime.Now;
                var keyId = _repository.InsertGetId(dataType);

                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = keyId,
                    ModuleId = (int)Model.DataType,
                    OperatorType = (int)OperatorType.Add,
                    Remark = "新增字典分类:" + dataType.DataTypeName
                });

                return new MessagesOutPut { Success = true, Message = "新增成功" };    
            }

            dataType = _repository.Get(input.Id);
            dataType.DataTypeCode = input.DataTypeCode;
            dataType.DataTypeName = input.DataTypeName;
            dataType.Desc = input.Desc;
            dataType.Sort = input.Sort;
            _repository.Update(dataType);

            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = dataType.Id,
                ModuleId = (int)Model.DataType,
                OperatorType = (int)OperatorType.Edit,
                Remark = "修改字典分类:" + dataType.DataTypeName
            });
            return new MessagesOutPut { Success = true, Message = "修改成功" };
        }
    }



}
