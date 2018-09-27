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
    /// 数据表实体应用服务现实：PaperPaperParam 
    /// </summary>
    public class PaperPaperParamAppService : BaseAppService<PaperPaperParam>
    {
        private readonly IPaperPaperParamRepository _iPaperPaperParamRepository;
        private readonly IPaperInfoRepository _iPaperInfoRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPaperPaperParamRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PaperPaperParamAppService(IPaperPaperParamRepository iPaperPaperParamRepository, OperatorLogAppService operatorLogAppService, IPaperInfoRepository iPaperInfoRepository) : base(iPaperPaperParamRepository)
        {
            _iPaperPaperParamRepository = iPaperPaperParamRepository;
            _operatorLogAppService = operatorLogAppService;
            _iPaperInfoRepository = iPaperInfoRepository;
        }

        /// <summary>
        /// 查询列表实体：PaperPaperParam 
        /// </summary>
        public List<PaperPaperParamOutput> GetList(PaperPaperParamListInput input, out int count)
        {
            const string sql = "select paperparam.Id,paperparam.ParamName,paperparam.AddTime,paperparam.AddUserId,paperparam.UpdateTime,paperparam.UpdateUserId,structure.StuctureName StuctureId,paperparam.State ";
            var strSql = new StringBuilder(" from t_Paper_PaperParam paperparam inner join t_Paper_Structure structure on paperparam.StuctureId = structure.Id where paperparam.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(input.StuctureId))
            {
                strSql.Append(" and paperparam.StuctureId = @StuctureId ");
                dynamicParameters.Add(":StuctureId", input.StuctureId, DbType.String);
            }

            if (!string.IsNullOrWhiteSpace(input.ParamName))
            {
                strSql.Append(" and paperparam.ParamName  like @ParamName ");
                dynamicParameters.Add(":ParamName", "%" + input.ParamName + "%", DbType.String);
            }

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                strSql.Append(" and paperparam.StuctureId in( select id from V_StructureList where (bsid =  @SubjectId or pid =  @SubjectId or pcid =  @SubjectId) ) ");
                dynamicParameters.Add(":SubjectId", input.Id, DbType.String);
            }

            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperPaperParamOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 根据参数ID查询结构ID与科目ID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PaperPaperParamOutput GetListById(PaperPaperParamListInput input)
        {
            const string sql = "select b.Id,b.SubjectId StuctureId from t_Paper_PaperParam  a inner join t_Paper_Structure b on a.StuctureId = b.Id where a.Id = @Id and a.IsDelete = 0";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Id", input.Id, DbType.String);
            var info = Db.QueryFirstOrDefault<PaperPaperParamOutput>(sql, dynamicParameters);
            return info;
        }

        /// <summary>
        /// 试卷参数下拉列表
        /// </summary>
        /// <returns></returns>
        public List<PaperPaperParamOutput> GetListToSelect()
        {
            const string sql = "select a.Id,a.ParamName from t_Paper_PaperParam  a inner join t_Paper_Structure structure on a.StuctureId = structure.Id where a.IsDelete = 0 and State=0 and structure.IsDelete=0";
            var info = Db.QueryList<PaperPaperParamOutput>(sql);
            return info;
        }

        /// <summary>
        /// 新增实体  PaperPaperParam
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperPaperParamInput input)
        {
            PaperPaperParam model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                const string sql = " select COUNT(Id)Id from t_Paper_Info where IsDelete = 0 and PaperParamId=@PaperParamId ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":PaperParamId", input.Id, DbType.String);
                var rowcount = Db.ExecuteScalar<int>(sql, dynamicParameters);
                if (rowcount > 0)
                {
                    return new MessagesOutPut { Success = false, Message = "该参数已被组卷引用，不允许修改!" };
                }
                else
                {
                    model = _iPaperPaperParamRepository.Get(input.Id);
                    #region 修改逻辑
                    model.Id = input.Id;
                    model.ParamName = input.ParamName;
                    model.UpdateUserId = UserObject.Id;
                    model.UpdateTime = DateTime.Now;
                    #endregion
                    _iPaperPaperParamRepository.Update(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.PaperPaperParam,
                        OperatorType = (int)OperatorType.Edit,
                        Remark = "修改PaperPaperParam:"
                    });
                    return new MessagesOutPut { Success = true, Message = "修改成功!" };
                }
            }
            model = input.MapTo<PaperPaperParam>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.State = 1;
            var keyId = _iPaperPaperParamRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PaperPaperParam,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增PaperPaperParam:"
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');

            const string sql = " select PaperParamId from t_Paper_Info where isdelete=0 ";

            List<PaperInfo> paperInfo = Db.QueryList<PaperInfo>(sql, null);
            string outMsg = string.Empty;
            foreach (var item in array)
            {
                var model = _iPaperPaperParamRepository.Get(item);
                var info = (from data in paperInfo where data.PaperParamId == item select data).LastOrDefault();
                if (info == null)
                {
                    if (model != null)
                    {
                        _operatorLogAppService.Add(new OperatorLogInput
                        {
                            KeyId = model.Id,
                            ModuleId = (int)Model.PaperPaperParam,
                            OperatorType = (int)OperatorType.Delete,
                            Remark = "删除PaperPaperParam:"
                        });
                    }
                    _iPaperPaperParamRepository.LogicDelete(model);
                }
                else
                {
                    outMsg += model.ParamName + ",";
                }
            }
            if (string.IsNullOrEmpty(outMsg))
            {
                return new MessagesOutPut { Message = "删除成功", Success = true };
            }
            else
            {
                return new MessagesOutPut { Message = outMsg.TrimEnd(',') + "已组试卷,删除失败", Success = false };
            }
        }



        /// <summary>
        /// 修改参数发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateState(PaperPaperParamInput input)
        {
            PaperPaperParam model = _iPaperPaperParamRepository.Get(input.Id);

            MessagesOutPut msg = null;
            //发布
            if (input.State == 0)
            {
                //判断是否满足发布条件
                //获得结构所需题目数量以及分数
                const string sql = @"select 
                                     a.Id,
                                     SUM(ISNULL(b.QuestionTypeScoreSum, 0))QuestionTypeScoreSum,
                                     SUM(ISNULL(b.QuestionCount, 0)) QuestionCount
                                      from t_Paper_Structure a inner join t_Paper_StructureDetail b
                                     on a.Id = b.StuctureId
                                     where a.Id = @id
                                     and a.IsDelete = 0 and b.IsDelete = 0
                                     group by a.Id ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":Id", model.StuctureId, DbType.String);
                PaperStructureDetail info = Db.QueryFirstOrDefault<PaperStructureDetail>(sql, dynamicParameters);

                const string sqlOne = @"select 
                                        a.Id,
                                        SUM(isnull(b.QuestionCount,0))QuestionCount,
                                        SUM(isnull(b.QuestionScoreSum,0))QuestionScoreSum
                                        from t_Paper_PaperParam a 
                                        left join t_Paper_ParamDetail b
                                        on a.Id = b.PaperParamId
                                        where a.IsDelete=0 and b.IsDelete = 0 and a.Id = @id
                                        group by a.Id";
                dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":Id", input.Id, DbType.String);
                PaperParamDetail paperInfo = Db.QueryFirstOrDefault<PaperParamDetail>(sqlOne, dynamicParameters);



                if (paperInfo != null)
                {
                    if ((info.QuestionTypeScoreSum != paperInfo.QuestionScoreSum || info.QuestionCount != paperInfo.QuestionCount))
                    {
                        msg = new MessagesOutPut { Message = "参数所需分数或试题数量不满足发布条件,发布失败!", Success = false };
                    }
                    else
                    {
                        model.State = input.State;
                        try
                        {
                            _iPaperPaperParamRepository.Update(model);
                            msg = new MessagesOutPut { Message = "发布成功", Success = true };
                        }
                        catch
                        {
                            msg = new MessagesOutPut { Message = "发布失败", Success = false };
                        }
                    }
                }
                else
                {
                    msg = new MessagesOutPut { Message = "参数所需分数或试题数量不满足发布条件,发布失败!", Success = false };
                }
            }
            else if (input.State == 1) //取消发布
            {
                //判断是否被引用
                const string sql = " select COUNT(Id)Id from t_Paper_Info where IsDelete = 0 and PaperParamId=@PaperParamId ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":PaperParamId", input.Id, DbType.String);
                int rowCount = Db.ExecuteScalar<int>(sql, dynamicParameters);
                if (rowCount > 0)
                {
                    msg = new MessagesOutPut { Message = "该参数已被组卷引用,取消发布失败", Success = false };
                }
                else
                {
                    model.State = input.State;
                    try
                    {
                        _iPaperPaperParamRepository.Update(model);
                        msg = new MessagesOutPut { Message = "取消发布成功", Success = true };
                    }
                    catch
                    {
                        msg = new MessagesOutPut { Message = "取消发布失败", Success = false };
                    }
                }
            }
            return msg;
        }
    }
}

