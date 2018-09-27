
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
using ZF.Infrastructure.zTree;
using System.Linq;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：PaperParamDetail 
    /// </summary>
    public class PaperParamDetailAppService : BaseAppService<PaperParamDetail>
    {
        private readonly IPaperParamDetailRepository _iPaperParamDetailRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 知识点
        /// </summary>
        private readonly SubjectKnowledgePointAppService _subjectKnowledgePointAppService;
        /// <summary>
        /// 试题
        /// </summary>
        private readonly SubjectBigQuestionAppService _subjectBigQuestionAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPaperParamDetailRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PaperParamDetailAppService(IPaperParamDetailRepository iPaperParamDetailRepository, OperatorLogAppService operatorLogAppService, SubjectKnowledgePointAppService subjectKnowledgePointAppService, SubjectBigQuestionAppService subjectBigQuestionAppService) : base(iPaperParamDetailRepository)
        {
            _iPaperParamDetailRepository = iPaperParamDetailRepository;
            _operatorLogAppService = operatorLogAppService;
            _subjectKnowledgePointAppService = subjectKnowledgePointAppService;
            _subjectBigQuestionAppService = subjectBigQuestionAppService;


        }

        /// <summary>
        /// 查询列表实体：PaperParamDetail 
        /// </summary>
        public List<PaperParamDetailOutput> GetList(PaperParamDetailListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Paper_ParamDetail  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.PaperParamId))
            {
                strSql.Append(" and a.PaperParamId = @PaperParamId ");
                dynamicParameters.Add(":PaperParamId", input.PaperParamId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperParamDetailOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 查询列表实体：PaperParamDetail 
        /// </summary>
        public List<PaperParamDetailOutput> GetList(string paperParamId)
        {
            const string sql = " select a.QuestionCount,a.DifficultLevel,a.QuestionScoreSum,a.Id,a.PaperParamId,a.KnowledgePointId,b.SubjectId,c.QuestionType,c.QuestionClass ";
            var strSql = new StringBuilder(@" from t_Paper_ParamDetail a
                                              inner join(select SubjectId, b.Id from t_Paper_Structure a 
                                              inner join t_Paper_PaperParam b on a.Id = b.StuctureId
                                             where b.Id = @PaperParamId)b
                                              on a.PaperParamId = b.Id
                                              inner join t_Paper_StructureDetail c on a.PaperStuctureDetailId = c.Id
                                             where a.IsDelete = 0  and PaperParamId = @PaperParamId ");
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":PaperParamId", paperParamId, DbType.String);
            var list = Db.QueryList<PaperParamDetailOutput>(sql + strSql, dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  PaperParamDetail
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperParamDetailInput input)
        {
            PaperParamDetail model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iPaperParamDetailRepository.Get(input.Id);

                const string sql = " select PaperParamId from t_Paper_Info where  PaperParamId in (select PaperParamId from t_Paper_ParamDetail where IsDelete= 0 and Id=@Id) ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":Id", input.Id.TrimEnd(','), DbType.String);
                List<PaperInfo> list = Db.QueryList<PaperInfo>(sql, dynamicParameters);

                if (list.Count > 0)
                {
                    return new MessagesOutPut { Id = 1, Message = "该参数明细已被引用,修改失败!", Success = false };
                }
                else
                {
                    #region 修改逻辑
                    model.Id = input.Id;
                    model.UpdateUserId = UserObject.Id;
                    model.UpdateTime = DateTime.Now;
                    model.PaperParamId = input.PaperParamId;
                    model.PaperStuctureDetailId = input.PaperStuctureDetailId;
                    model.KnowledgePointId = input.KnowledgePointId;
                    model.QuestionCount = input.QuestionCount;
                    model.DifficultLevel = input.DifficultLevel;
                    model.QuestionScoreSum = input.QuestionScoreSum;
                    #endregion
                    _iPaperParamDetailRepository.Update(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.PaperParamDetail,
                        OperatorType = (int)OperatorType.Edit,
                        Remark = "修改PaperParamDetail:"
                    });
                    return new MessagesOutPut { Success = true, Message = "修改成功!" };
                }
               
            }
            model = input.MapTo<PaperParamDetail>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iPaperParamDetailRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PaperParamDetail,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增PaperParamDetail:"
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }



        /// <summary>
        /// 获取已选择的知识点树
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> ParamDetailTreeList(PaperParamDetailListInput input)
        {
            const string sql = @"select id,name,pId,PDetailId,DLevel,QScoreSum,type,QuestionCount,DetailId,KIdList,KnowledgePointPid from ( select a.Id id,b.ClassName+'-'+[dbo].[F_Enum](2,DifficultLevel)+' (共 '+CAST(QuestionCount as nvarchar(20))+'道,'+CAST(QuestionTypeScoreSum as nvarchar(20))+'分 )' name,'' pId ,
                                a.Id PDetailId,DifficultLevel DLevel,QuestionTypeScoreSum QScoreSum,1 [type],QuestionCount,
                                 '' DetailId,
                                 '' KIdList,
                                 a.QuestionClass,
                                 a.DifficultLevel,
                                 b.OrderNo,'' KnowledgePointPid
                                 from t_Paper_StructureDetail a inner join t_Subject_Class b on a.QuestionClass = b.Id where StuctureId = @StuctureId and a.isdelete=0
                                 union
                                 select
                                 knowledgePoint.Id id,
                                 knowledgePoint.KnowledgePointName name,
                                 paramDetail.PaperStuctureDetailId pId,
                                 paramDetail.PaperStuctureDetailId PDetailId,
                                 paramDetail.DifficultLevel DLevel,
                                 paramDetail.QuestionScoreSum QScoreSum,2 [type],paramDetail.QuestionCount,
                                 paramDetail.Id DetailId,
                                 paramDetail.KnowledgePointId KIdList,
                                '123' QuestionClass,
                                 999 DifficultLevel,0 OrderNo,knowledgePoint.ParentId KnowledgePointPid
                                 from t_Paper_ParamDetail paramDetail
                                 inner
                                 join t_Subject_KnowledgePoint knowledgePoint
                                 on CHARINDEX(knowledgePoint.Id,paramDetail.KnowledgePointId) > 0
                                 where paramDetail.PaperParamId = @PaperParamId and paramDetail.isdelete=0 ) a order by OrderNo,DifficultLevel ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":StuctureId", input.StuctureId, DbType.String);
            dynamicParameters.Add(":PaperParamId", input.PaperParamId, DbType.String);
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql, dynamicParameters);
            List<Tree.zTree> query = (from item in dt where item.pId == "" select item).ToList<Tree.zTree>();
            var row = new List<Tree.zTree>();
            List<string> paramentList = new List<string>();

            foreach (Tree.zTree item in query)
            {
                Tree.zTree zt = new Tree.zTree();
                zt.id = item.id;
                zt.pId = item.pId;
                zt.name = item.name;
                zt.PDetailId = item.PDetailId;
                zt.type = item.type;
                zt.children = CreateChildTreeTwo(dt, zt.id, out paramentList);
                zt.QuestionCount = item.QuestionCount;
                zt.DLevel = item.DLevel;
                zt.QScoreSum = item.QScoreSum;
                zt.DetailId = item.DetailId;
                zt.KIdList = item.KIdList;
                zt.QuestionCount = item.QuestionCount;
                zt.DLevel = item.DLevel;
                zt.QScoreSum = item.QScoreSum;
                row.Add(zt);
            }
            return row;

        }

        private List<Tree.zTree> CreateChildTreeTwo(List<Tree.zTree> dt, string pid, out List<string> paramentList)
        {
            paramentList = new List<string>();
            string keyid = pid;                                        //根节点ID
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            var children = (from item in dt where item.pId == pid select item).ToList<Tree.zTree>();


            //参数明细ID集合
            List<string> detailIdList = new List<string>();

            try
            {
                foreach (var dr in children)
                {
                    Tree.zTree node = new Tree.zTree();
                    if (dr.KIdList.Contains(","))
                    {

                        if ((from data in dt where data.KnowledgePointPid == dr.id select data).ToList<Tree.zTree>().Count > 0 && !paramentList.Contains(dr.id))
                        {
                            paramentList.Add(dr.id);
                            paramentList.Add(dr.DetailId);
                            node.id = dr.id;
                            node.pId = keyid;
                            node.PDetailId = dr.PDetailId;
                            node.DetailId = dr.DetailId;
                            node.KIdList = dr.KIdList;
                            node.type = dr.type;
                            node.name = string.Format("{0}-组合知识点", dr.name);
                            node.children = BindDetailId(dr, null, dt, 1);
                            node.QuestionCount = dr.QuestionCount;
                            node.DLevel = dr.DLevel;
                            node.QScoreSum = dr.QScoreSum;
                            nodeList.Add(node);
                        }
                        else if ((from data in dt where data.id == dr.KnowledgePointPid select data).ToList<Tree.zTree>().Count <= 0 && !paramentList.Contains(dr.DetailId))
                        {
                            paramentList.Add(dr.DetailId);
                            paramentList.Add(dr.id);
                            node.id = dr.id;
                            node.pId = keyid;
                            node.PDetailId = dr.PDetailId;
                            node.DetailId = dr.DetailId;
                            node.KIdList = dr.KIdList;
                            node.type = dr.type;
                            var klistChildren = (from item in children where item.DetailId == dr.DetailId select item).ToList<Tree.zTree>();
                            //获取知识点id与name保存到泛型
                            Dictionary<string, string> kList = new Dictionary<string, string>();
                            if (klistChildren.Count > 1)
                            {
                                foreach (var item in klistChildren)
                                {
                                    kList.Add(item.id, item.name);
                                }
                            }
                            node.name = string.Format("组合知识点", dr.name);
                            node.children = BindDetailId(dr, kList, dt, 3);
                            node.QuestionCount = dr.QuestionCount;
                            node.DLevel = dr.DLevel;
                            node.QScoreSum = dr.QScoreSum;
                            nodeList.Add(node);
                        }

                    }
                    else
                    {
                        node.id = dr.id;
                        node.name = string.Format("{0}", dr.name);
                        node.pId = keyid;
                        node.PDetailId = dr.PDetailId;
                        node.DetailId = dr.DetailId;
                        node.KIdList = dr.KIdList;
                        node.type = dr.type;
                        node.children = null;
                        node.QuestionCount = dr.QuestionCount;
                        node.DLevel = dr.DLevel;
                        node.QScoreSum = dr.QScoreSum;
                        nodeList.Add(node);
                    }


                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return nodeList.Count == 0 ? null : nodeList;
        }

        private List<Tree.zTree> BindDetailId(Tree.zTree node, Dictionary<string, string> klist, List<Tree.zTree> dt,int type)
        {
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            try
            {
                if (type == 3)
                {
                    foreach (var item in klist)
                    {
                        Tree.zTree nodes = new Tree.zTree();
                        nodes.id = node.id;
                        nodes.name = item.Value;
                        nodes.pId = item.Key;
                        nodes.PDetailId = node.PDetailId;
                        nodes.DetailId = node.DetailId;
                        nodes.KIdList = node.KIdList;
                        nodes.type = "3";
                        nodes.children = null;
                        nodes.QuestionCount = node.QuestionCount;
                        nodes.DLevel = node.DLevel;
                        nodes.QScoreSum = node.QScoreSum;
                        nodeList.Add(nodes);
                    }
                }
                else
                {
                    var knowledgePoint = (from data in dt where data.KnowledgePointPid == node.id && data.DetailId == node.DetailId select data).ToList<Tree.zTree>();
                    if (knowledgePoint.Count <= 0)
                    {
                        knowledgePoint = (from data in dt where data.KnowledgePointPid == node.KnowledgePointPid && data.DetailId == node.DetailId select data).ToList<Tree.zTree>();
                    }
                    foreach (var item in knowledgePoint)
                    {
                        Tree.zTree nodes = new Tree.zTree();
                        nodes.id = node.id;
                        nodes.name = item.name;
                        nodes.pId = item.id;
                        nodes.PDetailId = node.PDetailId;
                        nodes.DetailId = node.DetailId;
                        nodes.KIdList = node.KIdList;
                        nodes.type = "3";
                        nodes.children = null;
                        nodes.QuestionCount = node.QuestionCount;
                        nodes.DLevel = node.DLevel;
                        nodes.QScoreSum = node.QScoreSum;
                        nodes.children = null;
                        nodeList.Add(nodes);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        
          
            return nodeList;
        }


        #region 手动组卷树加载
        /// <summary>
        /// 手工组卷
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Tree.zTree> PaperTreeList(PaperParamDetailListInput input)
        {
            const string sql = @"select id,name,pId,PDetailId,DLevel,QScoreSum,type,QuestionCount,DetailId,KIdList,OneCode,KnowledgePointPid from ( select Id id,ParamName name,'' pId ,
                                 Id PDetailId,
                                 0 DLevel,
                                 0 QScoreSum,
                                 1 [type],
                                 0 QuestionCount,
                                 '' DetailId,
                                 '' KIdList
                                ,'' OneCode,
                                '' QuestionClass,0 OrderNo,'' KnowledgePointPid
                                 from t_Paper_PaperParam
                                 where Id=@PaperParamId and isdelete=0
                                 union
                                 select
                                 knowledgePoint.Id id,
                                 knowledgePoint.KnowledgePointName+'-'+b.ClassName+'('+[dbo].[F_Enum](2,paramDetail.DifficultLevel)+')' name,
                                 paramDetail.PaperParamId pId,
                                 knowledgePoint.KnowledgePointCode PDetailId,
                                 paramDetail.DifficultLevel DLevel,
                                 paramDetail.QuestionScoreSum QScoreSum,2 [type],paramDetail.QuestionCount,
                                 paramDetail.Id DetailId,
                                 paramDetail.KnowledgePointId KIdList
                                ,knowledgePoint.Id +CAST(paramDetail.DifficultLevel as nvarchar(10))+SubjectId+QuestionType OneCode,StructureDetail.QuestionClass,b.OrderNo,knowledgePoint.ParentId KnowledgePointPid
                                 from t_Paper_ParamDetail paramDetail 
                                 inner join t_Paper_StructureDetail StructureDetail on paramDetail.PaperStuctureDetailId = StructureDetail.Id
                                 inner
                                 join t_Subject_KnowledgePoint knowledgePoint
                                 on CHARINDEX(knowledgePoint.Id,paramDetail.KnowledgePointId) > 0
                                 inner join t_Subject_Class b on StructureDetail.QuestionClass = b.Id
                                 where paramDetail.PaperParamId = @PaperParamId and paramDetail.isdelete=0 ) a order by OrderNo,DLevel ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":PaperParamId", input.PaperParamId, DbType.String);
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql, dynamicParameters);
            List<Tree.zTree> query = (from item in dt where item.pId == "" select item).ToList<Tree.zTree>();
            var row = new List<Tree.zTree>();
            List<string> paramentList = new List<string>();
            List<Tree.zTree> paperDetatail = new List<Tree.zTree>();
            //查询出已选择的试题
            if (input.GroupType == 1)
            {
                const string sqlinfo = @"select a.Id id,c.PaperName+'--'+ b.QuestionTitle name,b.KnowledgePointId pId,b.Id BigQuestionId ,b.KnowledgePointId+CAST(DifficultLevel as nvarchar(50))+b.SubjectId+CAST(QuestionTypeId as nvarchar(50)) OneCode
                                        from t_Paper_Detatail a inner join t_Subject_BigQuestion b on a.QuestionId = b.Id  
                                        inner join t_Paper_Info c on a.PaperId=c.Id where a.IsDelete = 0 and c.PaperParamId=@PaperParamId and c.Id=@Id ";
                dynamicParameters.Add(":Id", input.PaperId, DbType.String);
                paperDetatail = Db.QueryList<Tree.zTree>(sqlinfo, dynamicParameters);
            }
            foreach (Tree.zTree item in query)
            {
                Tree.zTree zt = new Tree.zTree();
                zt.id = item.id;
                zt.pId = item.pId;
                zt.name = item.name;
                zt.PDetailId = item.PDetailId;
                zt.type = item.type;
                zt.children = CreatePaperChildTreeTwo(dt, zt.id, out paramentList, paperDetatail, input);
                zt.QuestionCount = item.QuestionCount;
                zt.DLevel = item.DLevel;
                zt.QScoreSum = item.QScoreSum;
                zt.DetailId = item.DetailId;
                zt.KIdList = item.KIdList;
                zt.QuestionCount = item.QuestionCount;
                zt.DLevel = item.DLevel;
                zt.QScoreSum = item.QScoreSum;
                row.Add(zt);
            }
            return row;

        }



        private List<Tree.zTree> CreatePaperChildTreeTwo(List<Tree.zTree> dt, string pid, out List<string> paramentList, List<Tree.zTree> paperDetatail, PaperParamDetailListInput input)
        {
            paramentList = new List<string>();
            string keyid = pid;                                        //根节点ID
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            var children = (from item in dt where item.pId == pid select item).ToList<Tree.zTree>();


            //参数明细ID集合
            List<string> detailIdList = new List<string>();

            foreach (var dr in children)
            {

                Tree.zTree node = new Tree.zTree();
                if (dr.KIdList.Contains(","))
                {
                    if ((from data in dt where data.KnowledgePointPid == dr.id select data).ToList<Tree.zTree>().Count > 0 )
                    {
                        detailIdList.Add(dr.id);
                        node.id = dr.id;
                        node.name = dr.name;
                        node.pId = dr.id;
                        node.PDetailId = dr.PDetailId;
                        node.DetailId = dr.DetailId;
                        node.KIdList = dr.KIdList;
                        node.OneCode = dr.OneCode;
                        node.name = string.Format("{3}-组合知识点-{2}(共{0}道,{1}分)", dr.QuestionCount, dr.QScoreSum, dr.name.Split('-')[1], dr.name);
                        node.children = BindPaperDetailId(dr, null, paperDetatail, input, 0, dt);
                        node.QuestionCount = dr.QuestionCount;
                        node.DLevel = dr.DLevel;
                        node.QScoreSum = dr.QScoreSum;
                        node.type = "3";
                        nodeList.Add(node);

                    }
                    else if ((from data in dt where data.id == dr.KnowledgePointPid select data).ToList<Tree.zTree>().Count <= 0 && !detailIdList.Contains(dr.DetailId))
                    {
                        node.id = dr.id;
                        node.pId = keyid;
                        node.PDetailId = dr.PDetailId;
                        node.DetailId = dr.DetailId;
                        node.KIdList = dr.KIdList;
                        detailIdList.Add(dr.DetailId);
                        var klistChildren = (from item in children where item.DetailId == dr.DetailId select item).ToList<Tree.zTree>();
                        //获取知识点id与name保存到泛型
                        Dictionary<string, string[]> kList = new Dictionary<string, string[]>();
                        if (klistChildren.Count > 1)
                        {
                            foreach (var item in klistChildren)
                            {
                                string[] info = { item.name, item.OneCode };
                                kList.Add(item.id, info);
                            }
                        }
                        node.name = string.Format("组合知识点-{2}(共{0}道,{1}分)", dr.QuestionCount, dr.QScoreSum, dr.name.Split('-')[1]);
                        node.children = BindPaperDetailId(dr, kList, paperDetatail, input, 1, dt);
                        node.OneCode = null;
                        node.type = dr.type;
                        node.QuestionCount = dr.QuestionCount;
                        node.DLevel = dr.DLevel;
                        node.QScoreSum = dr.QScoreSum;
                        nodeList.Add(node);
                    }

                }
                else if (!string.IsNullOrEmpty(dr.KIdList))
                {
                    node.id = dr.id;
                    node.name = dr.name;
                    node.pId = keyid;
                    node.PDetailId = dr.PDetailId;
                    node.DetailId = dr.DetailId;
                    node.KIdList = dr.KIdList;
                    node.OneCode = dr.OneCode;
                    node.name = string.Format("{2}(共{0}道,{1}分)", dr.QuestionCount, dr.QScoreSum, dr.name);
                    node.children = BindPaperInfo(paperDetatail, dr.OneCode, input);
                    node.type = "3";
                    node.QuestionCount = dr.QuestionCount;
                    node.DLevel = dr.DLevel;
                    node.QScoreSum = dr.QScoreSum;
                    nodeList.Add(node);
                }
                else
                {
                    node.id = dr.id;
                    node.name = dr.name;
                    node.pId = keyid;
                    node.PDetailId = dr.PDetailId;
                    node.DetailId = dr.DetailId;
                    node.KIdList = dr.KIdList;
                    node.OneCode = dr.OneCode;
                    node.type = "3";
                    node.children = null;
                    node.QuestionCount = dr.QuestionCount;
                    node.DLevel = dr.DLevel;
                    node.QScoreSum = dr.QScoreSum;
                    nodeList.Add(node);
                }



            }
            return nodeList.Count == 0 ? null : nodeList;
        }

        private List<Tree.zTree> BindPaperDetailId(Tree.zTree node, Dictionary<string, string[]> klist, List<Tree.zTree> paperDetatail, PaperParamDetailListInput input,int type, List<Tree.zTree> dt)
        {
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            if (type == 1)
            {
                foreach (var item in klist)
                {
                    Tree.zTree nodes = new Tree.zTree();
                    nodes.id = node.id;
                    nodes.name = item.Value[0].Split('-')[0];
                    nodes.pId = item.Key;
                    nodes.PDetailId = node.PDetailId;
                    nodes.DetailId = node.DetailId;
                    nodes.KIdList = node.KIdList;
                    nodes.type = "3";
                    nodes.children = BindPaperInfo(paperDetatail, item.Value[1], input);
                    nodes.QuestionCount = node.QuestionCount;
                    nodes.DLevel = node.DLevel;
                    nodes.QScoreSum = node.QScoreSum;
                    nodes.Code = "NO";
                    nodes.OneCode = item.Value[1];
                    nodeList.Add(nodes);
                }
            }
            else
            {
                var knowledgePoint = (from data in dt where data.KnowledgePointPid == node.id && data.DetailId == node.DetailId select data).ToList<Tree.zTree>();
                if (knowledgePoint.Count <= 0)
                {
                    knowledgePoint = (from data in dt where data.KnowledgePointPid == node.KnowledgePointPid && data.DetailId == node.DetailId select data).ToList<Tree.zTree>();
                }

                var listSelect = (from data in paperDetatail where data.OneCode == node.OneCode select data).ToList<Tree.zTree>();
                if (listSelect.Count > 0 && input.GroupType == 1)
                {
                    foreach (var info in listSelect)
                    {
                        Tree.zTree nodeinfo = new Tree.zTree();
                        nodeinfo.id = info.id;
                        nodeinfo.name = info.name;
                        nodeinfo.pId = info.pId;
                        nodeinfo.type = "4";
                        nodeinfo.BigQuestionId = info.BigQuestionId;
                        nodeList.Add(nodeinfo);
                    }
                }
                foreach (var item in knowledgePoint)
                {
                   
                    Tree.zTree nodes = new Tree.zTree();
                    nodes.id = item.id;
                    nodes.name = item.name;
                    nodes.pId = item.id;
                    nodes.PDetailId = item.PDetailId;
                    nodes.DetailId = item.DetailId;
                    nodes.KIdList = item.KIdList;
                    nodes.type = "3";
                    nodes.children = BindPaperInfo(paperDetatail, item.OneCode, input);
                    nodes.QuestionCount = item.QuestionCount;
                    nodes.DLevel = item.DLevel;
                    nodes.QScoreSum = item.QScoreSum;
                    nodes.Code = "NO";
                    nodes.OneCode = item.OneCode;
                    nodeList.Add(nodes);
                }
            }
            return nodeList;
        }

        private List<Tree.zTree> BindPaperInfo(List<Tree.zTree> list, string kid, PaperParamDetailListInput input)
        {
            var listSelect = (from item in list where item.OneCode == kid select item).ToList<Tree.zTree>();
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            if (listSelect.Count > 0 && input.GroupType == 1)
            {
                foreach (var item in listSelect)
                {
                    Tree.zTree nodes = new Tree.zTree();
                    nodes.id = item.id;
                    nodes.name = item.name;
                    nodes.pId = item.pId;
                    nodes.type = "4";
                    nodes.BigQuestionId = item.BigQuestionId;
                    nodeList.Add(nodes);
                }
                return nodeList;
            }
            else
            {
                return null;
            }

        }
        #endregion


        /// <summary>
        /// 删除试卷参数明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            const string sql = " select PaperParamId from t_Paper_Info where  PaperParamId in (select PaperParamId from t_Paper_ParamDetail where IsDelete= 0 and Id=@Id) ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Id", input.Ids.TrimEnd(','), DbType.String);
            List<PaperInfo> list = Db.QueryList<PaperInfo>(sql, dynamicParameters);
            if (list.Count > 0)
            {
                return new MessagesOutPut { Id = 1, Message = "该参数明细已被引用,删除失败!", Success = false };
            }
            else
            {
                foreach (var item in array)
                {
                    var model = _iPaperParamDetailRepository.Get(item);
                    if (model != null)
                    {
                        _operatorLogAppService.Add(new OperatorLogInput
                        {
                            KeyId = model.Id,
                            ModuleId = (int)Model.PaperParamDetail,
                            OperatorType = (int)OperatorType.Delete,
                            Remark = "删除PaperParamDetail:"
                        });
                    }
                    _iPaperParamDetailRepository.LogicDelete(model);
                }
                return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
            }
           
        }
    }
}

