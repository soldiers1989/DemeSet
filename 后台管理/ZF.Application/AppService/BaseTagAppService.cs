using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    public class BaseTagAppService : BaseAppService<Base_Tag>
    {
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly IBaseTagRepository _repository;

        public BaseTagAppService(IBaseTagRepository repository, OperatorLogAppService logService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = logService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit(BaseTagInput input)
        {
            Base_Tag model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                if (model != null)
                {
                    model.Id = input.Id;
                    model.ModelCode = input.ModelCode;
                    model.TagName = input.TagName;
                    model.Remark = input.Remark;
                    _repository.Update(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.BaseData,
                        OperatorType = (int)OperatorType.Edit,
                        Remark = "修改标签：" + model.TagName
                    });
                    return new MessagesOutPut { Success = true, Message = "修改成功" };
                }
            }
            if(string.IsNullOrEmpty(input.TagName))
                return new MessagesOutPut { ModelId = input.Id, Success = false, Message = "添加失败,标签名称不能为空!" };
            model = input.MapTo<Base_Tag>();
            model.Id = Guid.NewGuid().ToString();
            var str = " select  count(1)  from t_Base_Tag where 1=1  and ModelCode=@ModelCode  and TagName=@TagName ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":ModelCode", input.ModelCode, DbType.String);
            dynamicParameters.Add(":TagName", input.TagName, DbType.String);
            if (Db.QueryFirstOrDefault<int>(str, dynamicParameters) > 0)
                return new MessagesOutPut { ModelId = model.Id, Success = false, Message = "添加失败,该标签名称已经存在!" };

            model.ModelCode = input.ModelCode;
            model.TagName = input.TagName;
            model.Remark = input.Remark;
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.BaseTag,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增标签:" + model.TagName
            });
            return new MessagesOutPut { ModelId = model.Id, Success = true, Message = "添加标签成功!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<BaseTagOutput> GetList(BaseTagListInput input, out int count)
        {
            const string sql = " select  *  ";
            var strSql = new StringBuilder(" from t_Base_Tag where 1=1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (input != null)
            {
                if (!string.IsNullOrWhiteSpace(input.ModelCode))
                {
                    strSql.Append(" and ModelCode=@ModelCode ");
                    dynamicParameters.Add(":ModelCode", input.ModelCode, DbType.String);
                }
                if (!string.IsNullOrEmpty(input.TagName))
                {
                    strSql.Append(" and TagName like @TagName ");
                    dynamicParameters.Add(":TagName", "%" + input.TagName + "%", DbType.String);
                }
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<BaseTagOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, "ModelCode", "desc"), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var strIds = input.Ids.TrimEnd(',');
            var arr = strIds.Split(',');

            foreach (var item in arr)
            {
                var model = _repository.Get(item);
                if (model != null)
                {
                    _repository.Delete(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.BaseTag,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除标签:" + model.TagName
                    });
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<BaseTagOutput> GetList(BaseTagInput input)
        {
            var sql = new StringBuilder(" select  * from t_Base_Tag where 1=1  ");
            var dynamicParameters = new DynamicParameters();
            if (input != null)
            {
                if (!string.IsNullOrWhiteSpace(input.ModelCode))
                {
                    sql.Append(" and ModelCode=@ModelCode ");
                    dynamicParameters.Add(":ModelCode", input.ModelCode, DbType.String);
                }
            }
            var list = Db.QueryList<BaseTagOutput>(sql.ToString(), dynamicParameters);
            return list;
        }
    }
}
