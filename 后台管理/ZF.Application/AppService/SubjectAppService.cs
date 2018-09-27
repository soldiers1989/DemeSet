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
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    public class SubjectAppService : BaseAppService<Subject>
    {
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly ISubjectRepository _repository;

        public SubjectAppService(ISubjectRepository repository, OperatorLogAppService operatorLogAppservice) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppservice;
        }

        /// <summary>
        /// 新增或编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit(SubjectInput input)
        {
            Subject model;
            RedisCacheHelper.Remove("GetProjectSubject");
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("SubjectList");
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                if (model != null)
                {
                    model.Id = input.Id;
                    model.SubjectName = input.SubjectName;
                    model.ProjectId = input.ProjectId;
                    model.OrderNo = input.OrderNo;
                    model.Remark = input.Remark;
                    model.UpdateUserId = UserObject.Id;
                    model.UpdateTime = DateTime.Now;
                    model.IsEconomicBase = input.IsEconomicBase;
                }
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.Subject,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改项目：" + model.SubjectName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功" };
            }
            model = input.MapTo<Subject>();
            model.Id = Guid.NewGuid().ToString();
            model.AddTime = DateTime.Now;
            model.AddUserId = UserObject.Id;
            _repository.Insert(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = model.Id,
                ModuleId = (int)Model.Subject,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增项目：" + model.SubjectName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功" };
        }

        /// <summary>
        /// 科目下是否存在未删除课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IfExistCourse( string id )
        {
            var strSql = "SELECT count(1)  FROM dbo.t_Course_Info a LEFT JOIN dbo.t_Base_Subject b ON a.SubjectId=b.Id WHERE a.SubjectId=@Id and a.IsDelete=0 ";
            var dynamicparameter = new DynamicParameters( );
            dynamicparameter.Add( ":Id",id,DbType.String);
            var count = Db.ExecuteScalar<int>( strSql, dynamicparameter );
            return count > 0; 
        }


        /// <summary>
        /// 科目下是否存在知识点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IfExistKnowledgePoint(string id)
        {
            var strSql = "SELECT count(1)  FROM dbo.t_Subject_KnowledgePoint a  WHERE a.SubjectId=@Id and a.IsDelete=0 ";
            var dynamicparameter = new DynamicParameters();
            dynamicparameter.Add(":Id", id, DbType.String);
            var count = Db.ExecuteScalar<int>(strSql, dynamicparameter);
            return count > 0;
        }

        

        /// <summary>
        /// 删除科目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            var idsStr = input.Ids.TrimEnd(',');
            var ids = idsStr.Split(',');
            foreach (var item in ids)
            {
                var model = _repository.Get(item);
                if (model != null)
                {
                    _repository.Delete(model);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = Guid.NewGuid().ToString(),
                        ModuleId = (int)Model.Subject,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除科目：" + model.SubjectName
                    });
                }
            }
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("SubjectList");
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }

        /// <summary>
        /// 获取科目列表
        /// </summary>
        /// <param name="input"></param>
        ///  <param name="count"></param>
        /// <returns></returns>
        public List<SubjectOutput> GetList(SubjectListInput input, out int count)
        {
            var sql = " select a.IsEconomicBase,a.Id,a.SubjectName,a.OrderNo,b.ProjectName,c.ProjectClassName";
            var strSql = new StringBuilder(@" from t_Base_Subject a
                                              left join t_Base_Project b ON a.ProjectId=b.Id
                                              left join t_Base_ProjectClass c ON b.ProjectClassId=c.Id
                                              where 1=1 and a.IsDelete=0 ");
            var strCount = " select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.SubjectName))
            {
                strSql.Append(" and a.SubjectName like  @SubjectName ");
                dynamicParameters.Add(":SubjectName", "%" + input.SubjectName + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectClassId))
            {
                strSql.Append(" and b.ProjectClassId like  @ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId", "%" + input.ProjectClassId + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectId))
            {
                strSql.Append(" and a.ProjectId=@ProjectId ");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }
            if ( !string.IsNullOrEmpty( input.ProjectClassId ) ) {
                strSql.Append( " and b.ProjectClassId=@ProjectClassId " );
                dynamicParameters.Add( ":ProjectClassId",input.ProjectClassId,DbType.String);
            }
            count = Db.ExecuteScalar<int>(strCount + strSql, dynamicParameters);
            var list = Db.QueryList<SubjectOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 获取科目列表  用于数据字典
        /// </summary>
        /// <returns></returns>
        public List<SubjectOutput> GetAllList(string projectId)
        {
            var list = new List<SubjectOutput>( );
            var dynamicParameter = new DynamicParameters( );
            if (!string.IsNullOrWhiteSpace( projectId ) ) {
                var strSql = "select a.Id,a.SubjectName  from t_Base_Subject a  where  a.IsDelete =0 and a.ProjectId=@ProjectId ";
                dynamicParameter.Add( ":ProjectId ",projectId,DbType.String);
                 list= Db.QueryList<SubjectOutput>( strSql ,dynamicParameter );
                return list;
            }
            list = Db.QueryList<SubjectOutput>(" select a.Id,a.SubjectName  from t_Base_Subject a  where  a.IsDelete =0 ", null);
            return list;
        }

        /// <summary>
        /// 获取下拉列表--根据条件作级联
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SubjectOutput> GetList( SubjectListInput input ) {
            var strSql = new StringBuilder( "SELECT a.id,a.SubjectName FROM dbo.t_Base_Subject a LEFT JOIN dbo.t_Course_Info b ON a.Id=b.SubjectId where 1=1 " );
            var dynamicParameters = new DynamicParameters( );
            if ( !string.IsNullOrWhiteSpace( input.CourseId) )
            {
                strSql.Append( " and b.CourseId =  @CourseId " );
                dynamicParameters.Add( ":CourseId",  input.CourseId , DbType.String );
            }
            var list = Db.QueryList<SubjectOutput>( strSql.ToString( ), dynamicParameters );
            return list;
        }
    }
}
