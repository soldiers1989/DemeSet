
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
using Topevery.Application.Dto;
using System.Linq;
using ZF.Infrastructure.PublicPaper;
using Newtonsoft.Json;
using ZF.Application.WebApiDto.SubjectModule;
using ZF.Infrastructure.RedisCache;
using PaperInfoOutput = ZF.Application.Dto.PaperInfoOutput;
using SubjectBigQuestionOutput = ZF.Application.Dto.SubjectBigQuestionOutput;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：PaperInfo 
    /// </summary>
    public class PaperInfoAppService : BaseAppService<PaperInfo>
    {
        private readonly IPaperInfoRepository _iPaperInfoRepository;

        private readonly ISubjectKnowledgePointRepository _iSubjectKnowledgePointRepository;

        private readonly ISubjectBigQuestionRepository _iSubjectBigQuestionRepository;
        /// <summary>
        /// 试卷参数明细
        /// </summary>
        private readonly PaperParamDetailAppService _paperParamDetailAppService;
        /// <summary>
        /// 知识点
        /// </summary>
        private readonly SubjectKnowledgePointAppService _subjectKnowledgePointAppService;
        /// <summary>
        /// 试题
        /// </summary>
        private readonly SubjectBigQuestionAppService _subjectBigQuestionAppService;
        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;
        /// <summary>
        /// 试卷结构服务类
        /// </summary>
        private readonly StructureAppService _structureAppService;

        /// <summary>
        /// 试卷集合
        /// </summary>
        private List<SubjectBigQuestionOutput> paperDetatailLists = new List<SubjectBigQuestionOutput>();
        private Dictionary<string, string> paperDetatailList = new Dictionary<string, string>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPaperInfoRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PaperInfoAppService(IPaperInfoRepository iPaperInfoRepository, OperatorLogAppService operatorLogAppService,
            PaperParamDetailAppService paperParamDetailAppService,
            SubjectKnowledgePointAppService subjectKnowledgePointAppService,
            SubjectBigQuestionAppService subjectBigQuestionAppService,
            StructureAppService structureAppService,
            ISubjectKnowledgePointRepository iSubjectKnowledgePointRepository,
            ISubjectBigQuestionRepository iSubjectBigQuestionRepository
            ) : base(iPaperInfoRepository)
        {
            _iPaperInfoRepository = iPaperInfoRepository;
            _operatorLogAppService = operatorLogAppService;
            _paperParamDetailAppService = paperParamDetailAppService;
            _subjectKnowledgePointAppService = subjectKnowledgePointAppService;
            _subjectBigQuestionAppService = subjectBigQuestionAppService;
            _structureAppService = structureAppService;
            _iSubjectKnowledgePointRepository = iSubjectKnowledgePointRepository;
            _iSubjectBigQuestionRepository = iSubjectBigQuestionRepository;
        }

        /// <summary>
        /// 查询列表实体：PaperInfo 
        /// </summary>
        public List<PaperInfoOutput> GetList(PaperInfoListInput input, out int count)
        {
            const string sql = "select Id,SubjectName,Subjectid,TestTime,PaperName,SUM(ISNULL(QuestionScore,0))QuestionScore,State,Type ";
            var strSql = new StringBuilder(" from V_Paper_Info_List  where 1=1 ");
            string sqlCount = "select count(*) from t_Paper_Info where IsDelete=0  ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.PaperName))
            {
                strSql.Append(" and PaperName like @PaperName ");
                sqlCount += " and PaperName like @PaperName ";
                dynamicParameters.Add(":PaperName", "%" + input.PaperName + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                strSql.Append(" and SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            if (input.Type.HasValue && input.Type > -1)
            {
                strSql.Append("and Type=@Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.Int16);
            }
            strSql.Append("group by Id,SubjectId,SubjectName,TestTime,PaperName,State,Type");
            count = Db.ExecuteScalar<int>(sqlCount, dynamicParameters);
            var list = Db.QueryList<PaperInfoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 获取试卷列表
        /// </summary>
        /// <returns></returns>
        public List<PaperInfoOutput> GetList()
        {
            var strSql = "select * from t_Paper_Info where IsDelete=0 ";
            return Db.QueryList<PaperInfoOutput>(strSql);
        }

        /// <summary>
        /// 查询试卷信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PaperInfoOutput GetInfo(string id)
        {
            const string sql = " select *  from t_Paper_Info where IsDelete = 0 and Id=@id ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":id", id, DbType.String);
            var info = Db.QueryFirstOrDefault<PaperInfoOutput>(sql, dynamicParameters);
            return info;
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EditInfo(PaperInfoOutput input)
        {
            PaperInfo model;

            if (!string.IsNullOrEmpty(input.Id))
            {
                RedisCacheHelper.Remove("ExaminationPaper_" + input.Id);
                const string sql = " update t_Paper_Info set PaperName=@PaperName,TestTime=@TestTime,UpdateTime=@UpdateTime,UpdateUserId=@UpdateUserId ,Type=@Type where Id=@id ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":PaperName", input.PaperName, DbType.String);
                dynamicParameters.Add(":TestTime", input.TestTime, DbType.String);
                dynamicParameters.Add(":UpdateTime", DateTime.Now, DbType.String);
                dynamicParameters.Add(":UpdateUserId", UserObject.Id, DbType.String);
                dynamicParameters.Add(":id", input.Id, DbType.String);
                dynamicParameters.Add(":Type", input.Type, DbType.Int16);
                Db.ExecuteNonQuery(sql, dynamicParameters);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = input.Id,
                    ModuleId = (int)Model.PaperInfo,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改PaperInfo:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            else
            {
                return new MessagesOutPut { Success = false, Message = "修改失败!" };
            }
        }




        /// <summary>
        /// 新增实体  PaperInfo
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperInfoInput input)
        {
            PaperInfo model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iPaperInfoRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iPaperInfoRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PaperInfo,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改PaperInfo:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<PaperInfo>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iPaperInfoRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PaperInfo,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增PaperInfo:"
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 修改试卷发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EditInfoState(PaperInfo input)
        {
            string sql = string.Empty;
            var dynamicParameters = new DynamicParameters();
            MessagesOutPut message = null;
            //取消发布
            if (input.State == 0)
            {
                //判断该试题是否被引用
                sql = "select COUNT(1) from t_My_PaperRecords where PaperId=@PaperId ";
                dynamicParameters.Add(":PaperId", input.Id);
                var count = Db.ExecuteScalar<int>(sql, dynamicParameters);
                if (count > 0)
                {
                    message = new MessagesOutPut { Success = false, Message = "试卷已被学生引用,取消失败" };
                }
                else
                {
                    sql = " update t_Paper_Info set State=@State where id=@PaperId ";
                    dynamicParameters.Add(":State", input.State);
                    try
                    {
                        Db.ExecuteNonQuery(sql, dynamicParameters);
                        message = new MessagesOutPut { Success = true, Message = "取消发布成功" };
                    }
                    catch
                    {
                        message = new MessagesOutPut { Success = false, Message = "取消发布失败" };
                    }

                }
            }
            else if (input.State == 1) //发布试卷
            {
                //判断在同一结构参数明细下是否存在多知识点不同分数与题目数的清空
                sql = @" select PaperStuctureDetailId from (
                       select PaperStuctureDetailId,QuestionScoreSum/QuestionCount QuestionScoreSum from t_Paper_ParamDetail  a
                       where exists(select 1 from  t_Paper_Info  where Id=@Id and a.PaperParamId = PaperParamId) and a.isdelete=0 
                       group by PaperStuctureDetailId,QuestionScoreSum/QuestionCount
                       ) a group by PaperStuctureDetailId having(COUNT(1)>1)";

                dynamicParameters.Add(":Id", input.Id);

                var stuctureDetail = Db.QueryFirstOrDefault<PaperParamDetail>(sql, dynamicParameters);
                if (stuctureDetail != null)
                {
                    message = new MessagesOutPut { Success = false, Message = "同一结构参数明细下存在多知识点不同分数的情况,发布失败" };
                }
                else
                {
                    //判断试题是否满足参数设置试题数
                    sql = @"select case when b.DetailCount = a.QuestionCount then 1 else 0 end istrue  from (
                                 select SUM(QuestionCount)QuestionCount ,PaperParamId
                                 from t_Paper_ParamDetail a
                                 where exists(select 1 from  t_Paper_Info
                                 where Id=@Id and a.PaperParamId = PaperParamId)
                                 and isdelete=0 
                                 group by a.PaperParamId
                                 ) a left join (select a.PaperParamId,COUNT(b.Id)DetailCount from t_Paper_Info a left join t_Paper_Detatail b
                                 on a.Id=b.PaperId
                                 where a.Id=@Id and a.isdelete=0
                                 group by a.PaperParamId)b on a.PaperParamId = b.PaperParamId";

                    var isTrue = Db.ExecuteScalar<bool>(sql, dynamicParameters);
                    if (isTrue)
                    {
                        sql = " update t_Paper_Info set State=@State where id=@Id ";
                        dynamicParameters.Add(":State", input.State);
                        try
                        {
                            Db.ExecuteNonQuery(sql, dynamicParameters);
                            message = new MessagesOutPut { Success = true, Message = "发布成功" };
                        }
                        catch
                        {
                            message = new MessagesOutPut { Success = false, Message = "发布失败" };
                        }
                    }
                    else
                    {
                        message = new MessagesOutPut { Success = false, Message = "试卷不满足参数设置条件,发布失败" };
                    }
                }
            }
            return message;
        }


        #region 自动组卷
        /// <summary>
        /// 自动组卷
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public MessagesOutPut PaperInfoAddList(PaperInfoInput input)
        {
            #region 通过试卷参数ID去查询参数明细表数据
            List<PaperParamDetailOutput> parameDetailList = _paperParamDetailAppService.GetList(input.PaperParamId);
            if (parameDetailList.Count <= 0)
            {
                return new MessagesOutPut { Id = -1, Message = "请先设置试卷参数之后再进行组卷!", Success = false };
            }

            input.SubjectId = _structureAppService.GetSubjectId(input.PaperParamId);

            if (string.IsNullOrEmpty(input.SubjectId))
            {
                return new MessagesOutPut { Id = -1, Message = "组卷失败，未能找到参数对应的科目!", Success = false };
            }
            #endregion

            #region 得到参数明细之后，传入知识点ID以及难度等级得到试题
            //sql集合
            StringBuilder sqlList;
            StringBuilder insertList = new StringBuilder();
            //知识点id
            string strKnowId = string.Empty;
            //知识点集合
            List<SubjectKnowledgePointOutput> konwList = new List<SubjectKnowledgePointOutput>();
            //试题集合
            List<SubjectBigQuestionOutput> bigQuestionList = new List<SubjectBigQuestionOutput>();
            PaperInfo paperInfo = new PaperInfo();
            //根据试卷份数创建GID
            List<string> GidList = PaperInfoGidList(input);
            var rowcount = 0;
            foreach (var gid in GidList)
            {
                foreach (var item in parameDetailList)
                {
                    sqlList = new StringBuilder();
                    foreach (string know in item.KnowledgePointId.Split(','))
                    {
                        konwList = _subjectKnowledgePointAppService.GetList(know);

                        foreach (var knowInfo in konwList)
                        {
                            if (sqlList.Length <= 0)
                            {
                                sqlList.AppendFormat(" where 1=1 and KnowledgePointId in( '{0}'", knowInfo.Id);
                            }
                            else
                            {
                                sqlList.AppendFormat(",'{0}'", knowInfo.Id);
                            }
                        }
                    }
                    //得到参数明细下所有知识点后去查询试题
                    sqlList.AppendFormat(") and DifficultLevel={0} and SubjectId='{1}' and SubjectType='{2}' and  SubjectClassId='{3}' and IsDelete=0  AND  State=0 ", item.DifficultLevel, item.SubjectId, item.QuestionType, item.QuestionClass);
                    bigQuestionList = _subjectBigQuestionAppService.GetList(sqlList.ToString());

                    #region 得到试题数据后，针对所需题目数量对试题进行随机筛选

                    InsertPaperInfoAndPaperDetatail(item, bigQuestionList, input);

                    #endregion
                }
                string jsonData = JsonConvert.SerializeObject(paperDetatailLists);
                paperDetatailList.Add(gid, jsonData);

                #region 将筛选过后的试题写入试卷表

                paperInfo = SetPaperInfoDetail(input, gid, rowcount == 0 ? input.PaperName : input.PaperName + "-" + rowcount.ToString());

                insertList.AppendFormat(@"  insert into t_Paper_Info(Id,PaperName,PaperParamId,SubjectId,TestTime,State,AddTime,AddUserId,IsDelete,[Type])
                                            values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}',{9}) ",
                                            paperInfo.Id, paperInfo.PaperName, paperInfo.PaperParamId, paperInfo.SubjectId, paperInfo.TestTime, paperInfo.State, paperInfo.AddTime, paperInfo.AddUserId, paperInfo.IsDelete, paperInfo.Type);

                var val = paperDetatailList.Where(q => q.Key == paperInfo.Id).Select(q => q.Value);

                List<SubjectBigQuestionOutput> outList = JsonConvert.DeserializeObject<List<SubjectBigQuestionOutput>>(val.ElementAt(0)); ;
                foreach (var paperDetatail in outList)
                {
                    insertList.AppendFormat(@" insert into t_Paper_Detatail(Id,PaperId,QuestionId,QuestionTypeId,QuestionScore,AddTime,AddUserId,IsDelete)
                                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", Guid.NewGuid().ToString(), gid, paperDetatail.Id,
                                                paperDetatail.SubjectType,
                                                paperDetatail.QuestionScoreSum,
                                                DateTime.Now,
                                                UserObject.Id,
                                                0);
                }
                #endregion

                rowcount++;
                paperDetatailLists = new List<SubjectBigQuestionOutput>();
            }
            #endregion

            if (insertList.Length <= 0)
            {
                return new MessagesOutPut { Id = -1, Message = "自动组卷失败，未能在试卷参数设置的知识点下找到对应的试题!", Success = false };
            }
            else
            {
                int count = Db.ExecuteNonQuery(insertList.ToString(), null);
                if (count <= 0)
                {
                    return new MessagesOutPut { Id = -1, Message = "自动组卷失败!", Success = false };
                }
                else
                {
                    return new MessagesOutPut { Id = -1, Message = "自动组卷成功!", Success = false };
                }
            }
        }




        /// <summary>
        /// 根据试卷份数创建Gid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<string> PaperInfoGidList(PaperInfoInput input)
        {
            List<string> GidList = new List<string>();

            for (int j = 0; j < input.PaperInfoCount; j++)
            {
                GidList.Add(Guid.NewGuid().ToString());
            }
            return GidList;
        }

        private void InsertPaperInfoAndPaperDetatail(PaperParamDetailOutput parameDetail, List<SubjectBigQuestionOutput> subjectBigQuestionList, PaperInfoInput input)
        {
            List<SubjectBigQuestionOutput> questionList = new List<SubjectBigQuestionOutput>();
            List<string> GidList = new List<string>();
            //根据试卷份数创建Gid
            for (int j = 0; j < input.PaperInfoCount; j++)
            {
                GidList.Add(Guid.NewGuid().ToString());
            }

            //算出试题平均分数
            string QuestionScore = Convert.ToDouble(Convert.ToDouble(parameDetail.QuestionScoreSum) / Convert.ToDouble(parameDetail.QuestionCount)).ToString("F2");
            //随机抽取试题
            Random random = new Random();
            int rannext = subjectBigQuestionList.Count;
            List<int> rannextList = new List<int>();

            //当所需的试题数目大于等于题库查出的试题数时取所有
            if (parameDetail.QuestionCount >= subjectBigQuestionList.Count)
            {
                foreach (var item in subjectBigQuestionList)
                {
                    SubjectBigQuestionOutput subjectBigQuestionOutput = new SubjectBigQuestionOutput();
                    subjectBigQuestionOutput.Id = item.Id;
                    subjectBigQuestionOutput.SubjectType = item.SubjectType;
                    subjectBigQuestionOutput.QuestionScoreSum = QuestionScore;
                    paperDetatailLists.Add(subjectBigQuestionOutput);
                }
            }
            else //否则随机抽取
            {
                //写入随机数
                while (1 == 1)
                {
                    if (rannextList.Count == parameDetail.QuestionCount)
                    {
                        break;
                    }
                    int next = random.Next(rannext);
                    if (!rannextList.Contains(next) && next != 0)
                    {
                        rannextList.Add(next);
                    }
                }

                foreach (var item in rannextList)
                {
                    var info = (from question in subjectBigQuestionList where question.rowid == item select question).ToList<SubjectBigQuestionOutput>();
                    SubjectBigQuestionOutput subjectBigQuestionOutput = new SubjectBigQuestionOutput()
                    {
                        Id = info[0].Id,
                        SubjectType = info[0].SubjectType,
                        QuestionScoreSum = QuestionScore
                    };
                    paperDetatailLists.Add(subjectBigQuestionOutput);
                }

            }

        }

        /// <summary>
        /// 试卷
        /// </summary>
        /// <param name="input"></param>
        /// <param name="GidList"></param>
        /// <param name="QuestionScore"></param>
        /// <returns></returns>
        private PaperInfo SetPaperInfoDetail(PaperInfoInput input, string GidList, string paperName)
        {
            PaperInfo model = new PaperInfo();
            model.Id = GidList;
            model.PaperParamId = input.PaperParamId;
            model.PaperName = paperName;//多份试卷生成不同的试卷名称
            model.TestTime = input.TestTime;
            model.State = 0;
            model.IsDelete = 0;
            model.PaperDetatailList = null; //SetPaperInfo(paperDetatailList, item, QuestionScore);
            model.SubjectId = input.SubjectId;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.Type = input.Type;
            //试卷明细表写入
            return model;
        }


        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="paperDetatailList"></param>
        /// <param name="gid"></param>
        /// <param name="questionScore"></param>
        /// <returns></returns>
        private List<PaperDetatail> SetPaperInfo(List<SubjectBigQuestionOutput> paperDetatailList, string gid, decimal questionScore)
        {
            List<PaperDetatail> paperInfoList = new List<PaperDetatail>();
            foreach (var item in paperDetatailList)
            {
                PaperDetatail model = new PaperDetatail();
                model.Id = Guid.NewGuid().ToString();
                model.PaperId = gid;
                model.QuestionId = item.Id;
                model.QuestionTypeId = item.SubjectType;
                model.QuestionScore = questionScore;
                model.AddUserId = UserObject.Id;
                model.AddTime = DateTime.Now;
                paperInfoList.Add(model);
            }
            return paperInfoList;
        }

        #endregion


        #region 手工组卷
        /// <summary>
        /// 手工组卷
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public MessagesOutPut ManualGroupVolumeAddInfo(PublicPaperInfo inputList)
        {
            var subjectId = string.Empty;
            // 获取所属科目

            subjectId = _structureAppService.GetSubjectId(inputList.PaperParamId);

            if (string.IsNullOrEmpty(subjectId))
            {
                return new MessagesOutPut { Id = -1, Message = "组卷失败，未能找到参数对应的科目!", Success = false };
            }

            StringBuilder sqlList = new StringBuilder();
            //写入试卷主表
            PaperInfo model = new PaperInfo();
            model.Id = Guid.NewGuid().ToString();
            model.PaperName = inputList.PaperName;
            model.PaperParamId = inputList.PaperParamId;
            model.SubjectId = subjectId;
            model.TestTime = inputList.TestTime;
            model.State = 0;
            model.AddTime = DateTime.Now;
            model.AddUserId = UserObject.Id;
            model.Type = inputList.Type;
            //算出
            var rowcount = 0;
            var dynamicParameters = new DynamicParameters();

            //查询试题分数
            string sql = @"select QuestionScoreSum/a.QuestionCount QuestionScoreSum,b.Id
                                     from t_Paper_ParamDetail a inner JOIN t_Subject_BigQuestion b
                                          on CHARINDEX(b.KnowledgePointId, a.KnowledgePointId) > 0
                                          inner JOIN  t_Paper_StructureDetail c 
                                          ON a.PaperStuctureDetailId = c.Id AND b.DifficultLevel = c.DifficultLevel AND b.SubjectType = c.QuestionType AND b.SubjectClassId = c.QuestionClass
                                    where a.PaperParamId = @PaperParamId";
            dynamicParameters.Add(":PaperParamId", inputList.PaperParamId, DbType.String);

            var bigQuestionList = Db.QueryList<PaperParamDetail>(sql, dynamicParameters);
            //新增
            if (string.IsNullOrEmpty(inputList.PaperId))
            {
                sqlList.AppendFormat(@" insert into t_Paper_Info(Id,PaperName,PaperParamId,SubjectId,TestTime,State,AddTime,AddUserId,IsDelete,[Type])
                                    values('{0}','{1}','{2}','{3}','{4}',0,'{5}','{6}',0,{7})",
                                       model.Id, model.PaperName, model.PaperParamId, model.SubjectId, model.TestTime, model.AddTime, model.AddUserId, model.Type);
                if (!string.IsNullOrEmpty(inputList.QuestionId))
                {
                    foreach (var item in inputList.QuestionId.Split(','))
                    {
                        var info = (from data in bigQuestionList where data.Id == item select data).LastOrDefault<PaperParamDetail>();

                        sqlList.AppendFormat(@" insert into t_Paper_Detatail(Id,PaperId,QuestionId,
                                        QuestionTypeId, QuestionScore, AddTime, AddUserId, IsDelete) select '{0}' Id,'{1}','{2}',SubjectType,{3},'{4}','{5}',0 from t_Subject_BigQuestion where IsDelete = 0 and id='{2}'  ",
                                            Guid.NewGuid().ToString(), model.Id, item, info.QuestionScoreSum, model.AddTime, model.AddUserId);
                        rowcount++;
                    }
                }
            }
            else //修改
            {
                model.Id = inputList.PaperId;

                sqlList.AppendFormat(@" update t_Paper_Info set PaperName='{1}',PaperParamId='{2}',SubjectId='{3}',TestTime='{4}',UpdateTime='{5}',UpdateUserId='{6}' where Id='{0}'",
                                       model.Id, model.PaperName, model.PaperParamId, model.SubjectId, model.TestTime, DateTime.Now, UserObject.Id);

                if (!string.IsNullOrEmpty(inputList.QuestionId))
                {
                    sqlList.AppendFormat(@" delete from t_Paper_Detatail where PaperId='{0}';  ", model.Id);
                    foreach (var item in inputList.QuestionId.Split(','))
                    {
                        var info = (from data in bigQuestionList where data.Id == item select data).LastOrDefault<PaperParamDetail>();

                        sqlList.AppendFormat(@" insert into t_Paper_Detatail(Id,PaperId,QuestionId,
                                        QuestionTypeId, QuestionScore, AddTime, AddUserId, IsDelete) select '{0}' Id,'{1}','{2}',SubjectType,{3},'{4}','{5}',0 from t_Subject_BigQuestion where IsDelete = 0 and id='{2}'  ",
                                            Guid.NewGuid().ToString(), model.Id, item, info.QuestionScoreSum, model.AddTime, model.AddUserId);
                        rowcount++;
                    }
                }

            }
            int count = Db.ExecuteNonQuery(sqlList.ToString(), null);
            if (count > 0)
            {
                //写入日志
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PaperInfo,
                    OperatorType = (int)OperatorType.Add,
                    Remark = string.IsNullOrEmpty(inputList.PaperId) ? "新增PaperInfo:" : "修改PaperInfo:"
                });
                //写入日志
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PaperDetatail,
                    OperatorType = (int)OperatorType.Add,
                    Remark = string.IsNullOrEmpty(inputList.PaperId) ? "新增Paper_Detatail:" : "修改Paper_Detatail:"
                });
                return new MessagesOutPut { Id = -1, Message = "组卷成功!", Success = true };
            }
            else
            {
                return new MessagesOutPut { Id = -1, Message = "组卷失败!", Success = false };
            }

        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IfUse(string id)
        {
            var strSql = string.Format("SELECT COUNT(*) FROM dbo.t_Course_Paper WHERE PaperInfoId='{0}'", id);
            return Db.ExecuteScalar<int>(strSql, null) > 0;
        }
    }
}

