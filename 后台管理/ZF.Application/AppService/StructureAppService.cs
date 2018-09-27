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
using ZF.Infrastructure.zTree;


namespace ZF.Application.AppService
{
    /// <summary>
    /// 试卷结构
    /// </summary>
    public class StructureAppService : BaseAppService<PaperStructure>
    {
        private readonly IPaperStructureRepository _repository;
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly IPaperPaperParamRepository _iPaperPaperParamRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        public StructureAppService(IPaperStructureRepository repository, OperatorLogAppService operatorLogAppService, IPaperPaperParamRepository iPaperPaperParamRepository) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
            _iPaperPaperParamRepository = iPaperPaperParamRepository;
        }

        /// <summary>
        /// 获取树
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> SubjectTreeList()
        {
            const string sql = " select * from V_SubjectList ";
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql);
            List<Tree.zTree> query = (from item in dt where item.pId == "" select item).ToList<Tree.zTree>();
            var row = new List<Tree.zTree>();
            foreach (Tree.zTree item in query)
            {
                Tree.zTree zt = new Tree.zTree();
                zt.id = item.id;
                zt.pId = item.pId;
                zt.name = item.name;
                zt.type = item.type;
                zt.DLevel = item.DLevel;
                zt.children = CreateChildTreeTwo(dt, zt.id);
                row.Add(zt);
            }
            return row;

        }

        private List<Tree.zTree> CreateChildTreeTwo(List<Tree.zTree> dt, string pid)
        {
            string keyid = pid;                                        //根节点ID
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            var children = (from item in dt where item.pId == pid select item).ToList<Tree.zTree>();
            foreach (var dr in children)
            {
                Tree.zTree node = new Tree.zTree();
                node.id = dr.id;
                node.name = dr.name;
                node.pId = keyid;
                node.type = dr.type;
                node.DLevel = dr.DLevel;
                node.children = CreateChildTreeTwo(dt, node.id);
                nodeList.Add(node);
            }
            return nodeList.Count == 0 ? null : nodeList;
        }


        /// <summary>
        /// 查询试卷结构信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PaperStructureOutput> GetList(PaperStructureListInput input, out int count)
        {
            const string sql = " select  * ";
            var strSql = new StringBuilder(" from V_StructureList where 1=1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrWhiteSpace(input.StuctureName))
            {
                strSql.Append(" and StuctureName like  @StuctureName ");
                dynamicParameters.Add(":StuctureName", '%' + input.StuctureName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                strSql.Append(" and (bsid =  @SubjectId or pid =  @SubjectId or pcid =  @SubjectId) ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperStructureOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 根据参数ID查询结构所属科目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSubjectId(string id)
        {
            const string sql = "select SubjectId from t_Paper_Structure a where exists(select StuctureId from t_Paper_PaperParam where Id=@Id and a.Id = StuctureId) and IsDelete = 0 ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Id", id, DbType.String);
            return Db.ExecuteScalar<string>(sql, dynamicParameters);
        }

        /// <summary>
        /// 新增试卷结构
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperStructureInput input)
        {
            PaperStructure model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.StuctureName = input.StuctureName;
                model.SubjectId = input.SubjectId;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.Structure,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改项目:" + model.StuctureName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<PaperStructure>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            //model.StuctureName = input.StuctureName;
            //model.SubjectId = input.SubjectId;
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.Structure,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增项目:" + model.StuctureName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 删除试卷结构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            var errorMessage = string.Empty;
            const string sql = " select StuctureId from t_Paper_PaperParam where isdelete=0 ";
            var paperList = Db.QueryList<PaperPaperParam>(sql, null);

            foreach (var item in array)
            {
                var model = _repository.Get(item);
                var paper = (from info in paperList where info.StuctureId == item select info).LastOrDefault<PaperPaperParam>();
                if (paper == null)
                {
                    if (model != null)
                    {
                        _operatorLogAppService.Add(new OperatorLogInput
                        {
                            KeyId = model.Id,
                            ModuleId = (int)Model.CourseChapter,
                            OperatorType = (int)OperatorType.Delete,
                            Remark = string.Format("删除试卷结构{0}", model.StuctureName)
                        });
                    }
                    _repository.LogicDelete(model);
                }
                else
                {
                    errorMessage += model.StuctureName + ",";
                }
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = errorMessage.TrimEnd(',') + "已被试卷参数引用，删除失败 ";
                return new MessagesOutPut { Id = 1, Message = errorMessage, Success = true };
            }
            else
            {
                return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
            }
            
        }
    }

}
