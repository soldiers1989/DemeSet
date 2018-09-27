using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;
using ZF.Infrastructure.zTree;
using Topevery.Application.Dto;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：PaperStructureDetail 
    /// </summary>
    public class PaperStructureDetailAppService : BaseAppService<PaperStructureDetail>
    {
        private readonly IPaperStructureDetailRepository _iPaperStructureDetailRepository;
        private readonly ISubjectClassRepository _iSubjectClassRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPaperStructureDetailRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PaperStructureDetailAppService(IPaperStructureDetailRepository iPaperStructureDetailRepository, OperatorLogAppService operatorLogAppService, ISubjectClassRepository iSubjectClassRepository) : base(iPaperStructureDetailRepository)
        {
            _iPaperStructureDetailRepository = iPaperStructureDetailRepository;
            _operatorLogAppService = operatorLogAppService;
            _iSubjectClassRepository = iSubjectClassRepository;
        }

        /// <summary>
        /// 查询列表实体：PaperStructureDetail 
        /// </summary>
        public List<PaperStructureDetailOutput> GetList(PaperStructureDetailListInput input, out int count)
        {
            const string sql = @"select  structureDetail.Id,
                                         structureDetail.StuctureId,
                                         structureDetail.QuesitonTypeName,
                                         structureDetail.QuestionType,
                                         dbo.F_Enum(1, structureDetail.QuestionType)QuestionTypeVal,
                                         structureDetail.QuestionClass,
                                         subjectClass.ClassName QuestionClassName,
                                         structureDetail.DifficultLevel,
                                         dbo.F_Enum(2, structureDetail.DifficultLevel)DifficultLevelName,
                                         structureDetail.QuestionCount,
                                         structureDetail.QuestionTypeScoreSum ,structureDetail.OrderNo,subjectClass.OrderNo OrderNo1  ";
            var strSql = new StringBuilder(@"  from t_Paper_StructureDetail structureDetail
                                               inner join t_Subject_Class subjectClass
                                               on structureDetail.QuestionClass = subjectClass.Id  where structureDetail.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(input.StuctureId))
            {
                strSql.Append(" and StuctureId = @StuctureId ");
                dynamicParameters.Add(":StuctureId", input.StuctureId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.QuesitonTypeName))
            {
                strSql.Append(" and structureDetail.QuesitonTypeName =like @QuesitonTypeName ");
                dynamicParameters.Add(":QuesitonTypeName", '%' + input.QuesitonTypeName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperStructureDetailOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, "OrderNo1,DifficultLevel", ""), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  PaperStructureDetail
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperStructureDetailInput input)
        {

            //判断题型类型，题型分类，难度等级是否是唯一记录
            const string sql = " select StuctureId from t_Paper_StructureDetail where StuctureId=@StuctureId and IsDelete=0  and QuestionClass =@QuestionClass and DifficultLevel =@DifficultLevel AND Id<>@Id ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":StuctureId", input.StuctureId, DbType.String);
            dynamicParameters.Add(":QuestionClass", input.QuestionClass, DbType.String);
            dynamicParameters.Add(":DifficultLevel", input.DifficultLevel, DbType.String);
            dynamicParameters.Add(":Id", input.Id, DbType.String);
            var list = Db.QueryList<PaperStructureDetailOutput>(sql, dynamicParameters);
            if (list.Count > 0)
            {
                return new MessagesOutPut { Success = false, Message = "已存在相同分类、难度等级的参数明细,保存失败!" };
            }

            //获得题型类型
            SubjectClass subjectInfo = _iSubjectClassRepository.Get(input.QuestionClass);

            PaperStructureDetail model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iPaperStructureDetailRepository.Get(input.Id);
                #region 修改逻辑

                model.Id = input.Id;
                model.QuesitonTypeName = input.QuesitonTypeName;
                model.QuestionType = subjectInfo.BigType.ToString();
                model.QuestionCount = input.QuestionCount;
                model.QuestionClass = input.QuestionClass;
                model.QuestionTypeScoreSum = input.QuestionTypeScoreSum;
                model.DifficultLevel = input.DifficultLevel;
                model.UpdateUserId = UserObject.Id;
                model.OrderNo = input.OrderNo;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iPaperStructureDetailRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PaperStructureDetail,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改PaperStructureDetail:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<PaperStructureDetail>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.QuestionType = subjectInfo.BigType.ToString();
            var keyId = _iPaperStructureDetailRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PaperStructureDetail,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增PaperStructureDetail:"
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');

            const string sql = " select PaperStuctureDetailId from t_Paper_ParamDetail where isdelete=0 ";
            var detailInfo = Db.QueryList<PaperParamDetail>(sql, null);
            string outMessage = string.Empty;

            foreach (var item in array)
            {
                var info = (from data in detailInfo where data.PaperStuctureDetailId == item select data).LastOrDefault();
                var model = _iPaperStructureDetailRepository.Get(item);
                if (info == null)
                {
                    if (model != null)
                    {
                        _operatorLogAppService.Add(new OperatorLogInput
                        {
                            KeyId = model.Id,
                            ModuleId = (int)Model.PaperStructureDetail,
                            OperatorType = (int)OperatorType.Delete,
                            Remark = "删除PaperStructureDetail:"
                        });
                    }
                    _iPaperStructureDetailRepository.LogicDelete(model);
                }
                else
                {
                    outMessage += model.QuesitonTypeName;
                }
            }
            if (string.IsNullOrEmpty(outMessage))
            {
                return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
            }
            else
            {
                return new MessagesOutPut { Id = 1, Message = "删除失败,该明细已被引用!", Success = false };
            }

        }

    }
}

