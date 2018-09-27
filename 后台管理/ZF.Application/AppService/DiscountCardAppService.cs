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
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    public class DiscountCardAppService : BaseAppService<Discount_Card>
    {
        private readonly IDiscount_CardRepository _discountCardAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly CourseInfoAppService _courseInfoAppService;

        public DiscountCardAppService(IDiscount_CardRepository repository, OperatorLogAppService operatorLogAppService, CourseInfoAppService courseInfoAppService) : base(repository)
        {
            _discountCardAppService = repository;
            _operatorLogAppService = operatorLogAppService;
            _courseInfoAppService = courseInfoAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit(DiscountCardInput input)
        {
            Discount_Card model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _discountCardAppService.Get(input.Id);
                if (model != null)
                {
                    model.Id = input.Id;
                    model.CardCode = input.CardCode;
                    model.CardName = input.CardName;
                    model.Amount = input.Amount;
                    model.BeginDate = input.BeginDate;
                    model.EndDate = input.EndDate;
                    model.TargetCourse = input.TargetCourse;

                    _discountCardAppService.Update(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.Discount_Card,
                        OperatorType = (int)OperatorType.Edit,
                        Remark = "修改学习卡：" + model.CardName
                    });
                    return new MessagesOutPut { Success = true, Message = "修改成功" };
                }
            }
            model = input.MapTo<Discount_Card>();
            model.Id = Guid.NewGuid().ToString();
            model.CardCode = input.CardCode;
            model.CardName = input.CardName;
            model.Amount = input.Amount;
            model.TargetCourse = input.TargetCourse;
            model.BeginDate = input.BeginDate;
            model.EndDate = input.EndDate;
            var keyId = _discountCardAppService.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.Project,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增学习卡:" + model.CardName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 判断卡号是否唯一
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        public bool IfUnique(string cardCode)
        {
            var sql = " select count(1) from t_Discount_Card where CardCode=@CardCode ";
            var dy = new DynamicParameters();
            dy.Add(":CardCode", cardCode, DbType.String);
            return Db.ExecuteScalar<int>(sql, dy) > 0 ? false : true;
        }


        /// <summary>
        /// 获取有效的学习卡集合
        /// </summary>
        /// <returns></returns>
        public List<DiscountCardOutput> GetList()
        {
            var sql = @"SELECT  Id ,
        CardName ,
        Amount
FROM    dbo.t_Discount_Card
WHERE   BeginDate < GETDATE()
        AND EndDate > GETDATE() AND TargetCourse IS NULL";
            return Db.QueryList<DiscountCardOutput>(sql);
        }


        /// <summary>
        /// 获取数据字典集合
        /// </summary>
        public List<DiscountCardOutput> GetList(DiscountCardListInput input, out int count)
        {
            const string sql = " select a.*,CONVERT(NVARCHAR,ISNULL(c.UseRecord,0))+'/'+CONVERT(NVARCHAR,ISNULL(b.GetRecord,0)) Record  ";
            var strSql = new StringBuilder(" from t_discount_card a LEFT JOIN (SELECT CardId, COUNT( 1 ) AS GetRecord FROM dbo.t_User_Discount_Card GROUP BY CardId )b ON a.CardCode = b.CardId LEFT JOIN (SELECT CardId, COUNT( 1 ) AS UseRecord FROM dbo.t_User_Discount_Card WHERE IfUse = 1 GROUP BY CardId)c ON a.CardCode = c.CardId  where 1=1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (input != null)
            {
                if (!string.IsNullOrWhiteSpace(input.CardName))
                {
                    strSql.Append(" and a.CardName like  @CardName ");
                    dynamicParameters.Add(":CardName", "%" + input.CardName + "%", DbType.String);
                }
                if (!string.IsNullOrWhiteSpace(input.TargetCourse))
                {
                    strSql.Append(" and a.TargetCourse like  @TargetCourse or isnull(a.TargetCourse,'')=''");
                    dynamicParameters.Add(":TargetCourse", "%" + input.TargetCourse + "%", DbType.String);
                }
                if (input.BeginDate.HasValue)
                {
                    strSql.Append(" and isnull(a.EndDate,'9999-12-31') >=  @BeginDate ");
                    dynamicParameters.Add(":BeginDate", input.BeginDate, DbType.String);
                }
                if (input.EndDate.HasValue)
                {
                    strSql.Append(" and isnull(a.BeginDate,'0001-01-01') <=  @EndDate ");
                    dynamicParameters.Add(":EndDate", input.EndDate, DbType.String);
                }
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<DiscountCardOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);

            foreach (var item in list)
            {
                var targetCourseName = "";
                var target = item.TargetCourse;
                if (!string.IsNullOrEmpty(target))
                {
                    var arr = target.Split(',');
                    for (var i = 0; i < arr.Length; i++)
                    {
                        var model = _courseInfoAppService.Get(arr[i]);
                        if (model != null)
                        {
                            targetCourseName += model.CourseName + "，";
                        }
                    }
                    item.TargetCourse = targetCourseName.TrimEnd('，');
                }
            }

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
                var model = _discountCardAppService.Get(item);
                if (model != null)
                {
                    _discountCardAppService.Delete(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.BaseData,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除学习卡:" + model.CardName
                    });
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }

    }
}
