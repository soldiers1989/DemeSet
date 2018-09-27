
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;
using ZF.Infrastructure.zTree;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：SubjectKnowledgePoint 
    /// </summary>
    public class SubjectKnowledgePointAppService : BaseAppService<SubjectKnowledgePoint>
    {
        private readonly ISubjectKnowledgePointRepository _iSubjectKnowledgePointRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSubjectKnowledgePointRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public SubjectKnowledgePointAppService(ISubjectKnowledgePointRepository iSubjectKnowledgePointRepository, OperatorLogAppService operatorLogAppService) : base(iSubjectKnowledgePointRepository)
        {
            _iSubjectKnowledgePointRepository = iSubjectKnowledgePointRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectKnowledgePoint 
        /// </summary>
        public List<SubjectKnowledgePointOutput> GetList(SubjectKnowledgePointListInput input, out int count)
        {
            const string sql = " select  a.*,ISNULL(b.KnowledgePointName,' ') ParentName,c.SubjectName ";
            var strSql = new StringBuilder(@" from t_Subject_KnowledgePoint a 
                                           left join t_Subject_KnowledgePoint b on a.ParentId =b.Id 
                                           left join t_Base_Subject c on a.SubjectId=c.Id
                                           left join t_Base_Project d on c.ProjectId=d.Id
                                            where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.ParentId))
            {
                strSql.Append(" and a.ParentId = @ParentId ");
                dynamicParameters.Add(":ParentId", input.ParentId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectId))
            {
                strSql.Append(" and d.Id = @ProjectId ");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectClassId))
            {
                strSql.Append(" and d.ProjectClassId = @ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId", input.ProjectClassId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.KnowledgePointName))
            {
                strSql.Append(" and a.KnowledgePointName like @KnowledgePointName ");
                dynamicParameters.Add(":KnowledgePointName", '%' + input.KnowledgePointName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.KnowledgePointCode))
            {
                strSql.Append(" and a.KnowledgePointCode like @KnowledgePointCode ");
                dynamicParameters.Add(":KnowledgePointCode", '%' + input.KnowledgePointCode + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                strSql.Append(" and  a.SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SubjectKnowledgePointOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 根据知识点ID查询包含自己的所有知识点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SubjectKnowledgePointOutput> GetList(string id)
        {
            const string sql = "select Id from t_Subject_KnowledgePoint where KnowledgePointCode like ''+(select KnowledgePointCode from t_Subject_KnowledgePoint where Id=@Id)+'%' and IsDelete=0 ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Id", id, DbType.String);
            var list = Db.QueryList<SubjectKnowledgePointOutput>(sql, dynamicParameters);
            return list;
        }


        /// <summary>
        /// 查询列表实体：SubjectKnowledgePoint 
        /// </summary>
        public SubjectKnowledgePointOutput GetOnes(string id)
        {
            const string sql = "select  a.*,b.KnowledgePointName ParentName,c.SubjectName ";
            var strSql = new StringBuilder(" from t_Subject_KnowledgePoint a  " +
                                           " left join t_Subject_KnowledgePoint b on a.ParentId =b.Id  left join t_Base_Subject c on a.ParentId=c.Id" +
                                           "  where a.IsDelete=0  ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(id))
            {
                strSql.Append(" and a.Id = @Id ");
                dynamicParameters.Add(":Id", id, DbType.String);
            }
            var list = Db.QueryFirstOrDefault<SubjectKnowledgePointOutput>(sql + strSql, dynamicParameters);
            return list;
        }

        /// <summary>
        /// 删除知识点
        /// </summary>
        public MessagesOutPut SubjectKnowledgePointDelete(SubjectKnowledgePoint input)
        {

            string strBigQuestion = @" select count(1) from t_Subject_BigQuestion where KnowledgePointId = '" + input.Id + "'  and IsDelete=0 ";
            var bigQuestionCount = Db.ExecuteScalar<int>(strBigQuestion, null);
            if (bigQuestionCount>0)
            {
                return  new MessagesOutPut {Id = -1,Message = "删除失败,该知识点下存在试题!",Success = false};
            }
            _iSubjectKnowledgePointRepository.LogicDelete(input);
            string strSqlModule = @" SELECT  Id ,
        KnowledgePointName Name ,
        ParentId pId
FROM    t_Subject_KnowledgePoint
WHERE   1 = 1  AND IsDelete=0 and ParentId='" + input.Id + "'  order BY AddTime ASC ";
            var children = Db.QueryList<Tree.zTree>(strSqlModule);
            var list = CreateChildTree1(children, new Tree.zTree { id = input.Id, pId = input.ParentId }, new List<Tree.zTree>());
            var idList = list.Aggregate("", (current, item) => current + ("'" + item.id + "'" + ",")).TrimEnd(',');
            if (!string.IsNullOrEmpty(idList))
            {
                if (Db.ExecuteScalar<int>(" select count(1) from t_Subject_BigQuestion where KnowledgePointId in(" + idList + ")  and IsDelete=0 ",null)>0)
                {
                    return new MessagesOutPut { Id = -1, Message = "删除失败,该知识点子知识节点下存在试题!", Success = false };
                }
                Db.ExecuteNonQuery(" update t_Subject_KnowledgePoint set isdelete =1  where Id in(" + idList + ")", null);
            }
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            return new MessagesOutPut { Id = -1, Message = "删除成功!" ,Success = true};
        }

        private List<Tree.zTree> CreateChildTree1(List<Tree.zTree> dt, Tree.zTree jt, List<Tree.zTree> nodeList)
        {
            string keyid = jt.id;                                        //根节点ID
            var children = dt.Where(x => x.pId == keyid).ToList();
            foreach (var dr in children)
            {
                Tree.zTree node = new Tree.zTree();
                node.id = dr.id;
                node.name = dr.name;
                node.pId = keyid;
                node.children = CreateChildTree1(dt, node, nodeList);
                nodeList.Add(node);
            }
            return nodeList;
        }


        /// <summary>
        /// 新增实体  SubjectKnowledgePoint
        /// </summary>
        public MessagesOutPut AddOrEdit(SubjectKnowledgePointInput input)
        {
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            SubjectKnowledgePoint model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iSubjectKnowledgePointRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.KnowledgePointName = input.KnowledgePointName;
                model.ParentId = input.ParentId;
                model.SubjectId = input.SubjectId;
                model.DigitalBookPage = input.DigitalBookPage;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iSubjectKnowledgePointRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectKnowledgePoint,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改科目知识点:" + model.KnowledgePointName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SubjectKnowledgePoint>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var sql = " SELECT MAX(KnowledgePointCode)  from t_Subject_KnowledgePoint where 1=1 ";

            var parentModel = new SubjectKnowledgePoint { KnowledgePointCode = "" };
            if (!string.IsNullOrEmpty(model.ParentId))
            {
                sql += " and ParentId ='" + model.ParentId + "'";
                if (_iSubjectKnowledgePointRepository.Get(model.ParentId) != null)
                {
                    parentModel = _iSubjectKnowledgePointRepository.Get(model.ParentId);
                }
            }
            var row = Db.QueryFirstOrDefault<string>(sql, null);
            if (row == null)
            {
                model.KnowledgePointCode = parentModel.KnowledgePointCode + "001";
            } else
            {
                var code = (int.Parse(row.Substring(row.Length - 3, 3)) + 1).ToString();
                switch (code.Length)
                {
                    case 1:
                        model.KnowledgePointCode = row.Substring(0, row.Length - 3) + "00" + code;
                        break;
                    case 2:
                        model.KnowledgePointCode = row.Substring(0, row.Length - 3) + "0" + code;
                        break;
                    case 3:
                        model.KnowledgePointCode = row.Substring(0, 3) + code;
                        break;
                }
            }
            var keyId = _iSubjectKnowledgePointRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SubjectKnowledgePoint,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增科目知识点:" + model.KnowledgePointName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 获取知识点树  保护项目分类  项目 科目
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> SubjectKnowledgePointTree()
        {
            const string sql2 = @" SELECT  * FROM   V_Subject_KnowledgePoint  ORDER BY Code ASC;  ";
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql2);
            List<Tree.zTree> drList = dt.Where(x => x.pId == "").ToList();
            var row = new List<Tree.zTree>();
            foreach (var dr in drList)
            {
                Tree.zTree jt = new Tree.zTree();
                jt.id = dr.id;
                jt.name = dr.name;
                jt.type = dr.type;
                jt.subjectId = dr.subjectId;
                jt.children = CreatePointTree(dt, jt);
                row.Add(jt);
            }
            return row;
        }

        private List<Tree.zTree> CreatePointTree(List<Tree.zTree> dt, Tree.zTree jt)
        {
            string keyid = jt.id;                                        //根节点ID
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            var children = dt.Where(x => x.pId == keyid).ToList();
            foreach (var dr in children)
            {
                Tree.zTree node = new Tree.zTree();
                node.id = dr.id;
                node.name = dr.name;
                node.pId = keyid;
                node.type = dr.type;
                node.subjectId = dr.subjectId;
                node.children = CreatePointTree(dt, node);
                node.Code = dr.Code;
                nodeList.Add(node);
            }
            return nodeList.Count == 0 ? null : nodeList;
        }

        /// <summary>
        /// 获取知识点树
        /// </summary>
        public List<Tree.zTree> SubjectKnowledgePointLists(string subjectId)
        {
            string sql2 = @" SELECT* FROM   V_Subject_KnowledgePoint where type NOT IN('1', '2')  ";

                sql2 += " and subjectId= '" + subjectId + "'";
            sql2 += " ORDER BY Code ASC ";
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql2);
            List<Tree.zTree> drList = dt.Where(x => x.type == "3").ToList();
            var row = new List<Tree.zTree>();
            foreach (var dr in drList)
            {
                Tree.zTree jt = new Tree.zTree();
                jt.id = dr.id;
                jt.name = dr.name;
                jt.type = dr.type;
                jt.subjectId = dr.subjectId;
                jt.children = CreatePointTree(dt, jt);
                row.Add(jt);
            }
            return row;
        }

        /// <summary>
        /// 根据试卷参数id
        /// 获取知识点树
        /// </summary>
        public List<Tree.zTree> SubjectKnowledgePointLists(string subjectId, string paperParamId,string structureDetailId, string detailId)
        {
            string sql2 = @" SELECT* FROM   V_Subject_KnowledgePoint where type NOT IN('1', '2')  ";
            string strStuffSql = string.Empty;
            if (!string.IsNullOrEmpty(subjectId))
            {
                sql2 += " and subjectId= '" + subjectId + "'";
            }
            sql2 += " ORDER BY Code ASC ";

            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql2);
            //得到已添加的知识点
            sql2 = @" with info as(   select KnowledgePointId
                        from t_Paper_ParamDetail a where PaperParamId=@paperParamId   and isdelete=0 ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":paperParamId", paperParamId, DbType.String);

            if (!string.IsNullOrEmpty(structureDetailId))
            {
                sql2 += " and PaperStuctureDetailId=@PaperStuctureDetailId ";
                dynamicParameters.Add(":PaperStuctureDetailId", structureDetailId, DbType.String);
            }

            if (!string.IsNullOrEmpty(detailId))
            {
                sql2 += " and Id<>@Id ";
                dynamicParameters.Add(":Id", detailId, DbType.String);
            }

            dynamicParameters.Add(":stuffwhere", strStuffSql, DbType.String);

            sql2 += " )  SELECT distinct* FROM DBO.fc_findKnowledgePointId((select distinct STUFF((select ','+ KnowledgePointId from info for  xml path('')),1,1,'')KnowledgePointId  from info a group by KnowledgePointId  )) ";


            var paramDetailList = Db.QueryList<PaperParamDetail>(sql2, dynamicParameters);
            //从树中删除已添加过的知识点
            foreach (var item in paramDetailList)
            {
                try
                {
                    dt.Remove(dt.Where(info => info.id == item.KnowledgePointId).Single());
                } catch
                {
                    continue;
                }
            }

            List<Tree.zTree> drList = dt.Where(x => x.type == "3").ToList();
            var row = new List<Tree.zTree>();
            foreach (var dr in drList)
            {
                Tree.zTree jt = new Tree.zTree();
                jt.id = dr.id;
                jt.name = dr.name;
                jt.type = dr.type;
                jt.subjectId = dr.subjectId;
                jt.children = CreatePointTree(dt, jt);
                row.Add(jt);
            }
            return row;
        }

        /// <summary>
        /// 获取知识点，包含试题
        /// </summary>
        /// <param KnowledgePointId="KnowledgePointId"></param>
        /// <returns></returns>
        public List<Tree.zTree> SubjectKnowledgePointAndpaperInfoLists(string KnowledgePointId, string DifficultLevel,string PaperId, string DetailId)
        {

            var dynamicParameters = new DynamicParameters();
            string sql = string.Empty;
            //得到试卷参数明细
            sql = @" select a.DifficultLevel,c.SubjectId,b.QuestionType,b.QuestionClass 
                     from t_Paper_ParamDetail a 
                     inner join t_Paper_StructureDetail b on a.PaperStuctureDetailId = b.Id
                     inner join t_Paper_Structure c on b.StuctureId = c.Id
                     where a.Id=@Id ";
            dynamicParameters.Add(":Id", DetailId, DbType.String);
            PaperParamDetailOutput outInfo = Db.QueryFirstOrDefault<PaperParamDetailOutput>(sql, dynamicParameters);
            dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":KnowledgePointIdList", KnowledgePointId, DbType.String);
            dynamicParameters.Add(":DifficultLevel", DifficultLevel, DbType.String);
            dynamicParameters.Add(":SubjectId", outInfo.SubjectId, DbType.String);
            dynamicParameters.Add(":QuestionType", outInfo.QuestionType, DbType.String);
            dynamicParameters.Add(":QuestionClass", outInfo.QuestionClass, DbType.String);
            List<Tree.zTree> dt = Db.ExecStoredProcedure<Tree.zTree>("sp_ManualGroupVolume", dynamicParameters);
            if (!string.IsNullOrEmpty(PaperId))
            {
                //获得试卷已选试题
                sql = " select QuestionId from t_Paper_Detatail where PaperId=@PaperId and isDelete=0 ";
                dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":PaperId", PaperId, DbType.String);
                var list = Db.QueryList<PaperDetatail>(sql, dynamicParameters);
                foreach (var item in list)
                {
                    try
                    {
                        dt.Remove(dt.Where(info => info.id == item.QuestionId).Single());
                    } catch
                    {
                        continue;
                    }
                }
            }
            List<Tree.zTree> drList = dt.Where(x => x.type == "4").ToList();
            if (drList.Count <= 0)
            {
                drList = dt.Where(x => x.type == "5").ToList();
            }
            var row = new List<Tree.zTree>();
            List<string> listStr = new List<string>();
            foreach (var dr in drList)
            {
                //加载章
                var info = (from data in dt where data.pId == dr.id && data.type == "6" select data).ToList<Tree.zTree>();
                Tree.zTree jt = new Tree.zTree();
                listStr.Add(dr.pId);
                jt.id = dr.id;
                jt.name = dr.name;
                jt.type = dr.type;
                jt.subjectId = dr.subjectId;
                jt.children = CreatePointTreeNew(info, jt);
                jt.Code = dr.Code;
                row.Add(jt);
            }
            return row;
        }

        private List<Tree.zTree> CreatePointTreeNew(List<Tree.zTree> dt, Tree.zTree jt)
        {
            string keyid = jt.id;                                        //根节点ID
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            foreach (var dr in dt)
            {
                Tree.zTree node = new Tree.zTree();
                node.id = dr.id;
                node.name = dr.name;
                node.pId = keyid;
                node.type = dr.type;
                node.subjectId = dr.subjectId;
                node.children = null;
                node.Code = dr.Code;
                nodeList.Add(node);
            }
            return nodeList.Count == 0 ? null : nodeList;
        }
    }
}

