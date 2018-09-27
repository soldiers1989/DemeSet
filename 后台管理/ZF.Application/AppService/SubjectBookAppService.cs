
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

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：科目关联书籍管理 
    /// </summary>
    public class SubjectBookAppService : BaseAppService<SubjectBook>
    {
        private readonly ISubjectBookRepository _iSubjectBookRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly FileRelationshipAppService _fileRelationshipAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSubjectBookRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        public SubjectBookAppService(ISubjectBookRepository iSubjectBookRepository, OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService) : base(iSubjectBookRepository)
        {
            _iSubjectBookRepository = iSubjectBookRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        /// 查询列表实体：科目关联书籍管理 
        /// </summary>
        public List<SubjectBookOutput> GetList(SubjectBookListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Base_SubjectBook  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.BookName))
            {
                strSql.Append(" and a.BookName like @BookName ");
                dynamicParameters.Add(":BookName", '%' + input.BookName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                strSql.Append(" and a.SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SubjectBookOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  科目关联书籍管理
        /// </summary>
        public MessagesOutPut AddOrEdit(SubjectBookInput input)
        {
            SubjectBook model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iSubjectBookRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.BookName = input.BookName;
                model.OrderNo = input.OrderNo;
                model.Url = input.Url;
                model.ImageUrl = input.IdFilehiddenFile;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iSubjectBookRepository.Update(model);
                if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
                {
                    _fileRelationshipAppService.Add(new FileRelationshipInput
                    {
                        ModuleId = input.Id,
                        IdFilehiddenFile = input.IdFilehiddenFile,
                        Type = 0//图片
                    });
                }
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectBook,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改科目关联书籍管理:" + model.BookName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SubjectBook>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.ImageUrl = input.IdFilehiddenFile;
            var keyId = _iSubjectBookRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SubjectBook,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增科目关联书籍管理:" + model.BookName
            });
            if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
            {
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                    Type = 0//图片
                });
            }
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

