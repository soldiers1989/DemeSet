
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
    /// 数据表实体应用服务现实：试卷组表 
    /// </summary>
    public class PaperGroupAppService : BaseAppService<PaperGroup>
    {
        private readonly IPaperGroupRepository _iPaperGroupRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPaperGroupRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public PaperGroupAppService(IPaperGroupRepository iPaperGroupRepository, OperatorLogAppService operatorLogAppService) : base(iPaperGroupRepository)
        {
            _iPaperGroupRepository = iPaperGroupRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：试卷组表 
        /// </summary>
        public List<PaperGroupOutput> GetList(PaperGroupListInput input, out int count)
        {
            const string sql = @"SELECT  a.* ,
        b.SubjectName ,
        CASE a.Type
          WHEN 0 THEN '历年真题'
          WHEN 1 THEN '模拟试卷'
        END AS TypeName ,
        CASE a.State
          WHEN 1 THEN '已发布'
          WHEN 0 THEN '未发布'
        END AS StateName ";
            var strSql = new StringBuilder(@" FROM    t_Paper_Group a
        LEFT JOIN dbo.t_Base_Subject b ON a.SubjectId = b.Id
WHERE   a.IsDelete = 0 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.PaperGroupName))
            {
                strSql.Append(" and a.PaperGroupName like @PaperGroupName ");
                dynamicParameters.Add(":PaperGroupName", '%' + input.PaperGroupName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                strSql.Append(" and a.SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<PaperGroupOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  试卷组表
        /// </summary>
        public MessagesOutPut AddOrEdit(PaperGroupInput input)
        {
            PaperGroup model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iPaperGroupRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.PaperGroupName = input.PaperGroupName;
                model.State = input.State;
                model.SubjectId = input.SubjectId;
                model.Type = input.Type;

                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iPaperGroupRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.PaperGroup,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改试卷组表:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<PaperGroup>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iPaperGroupRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.PaperGroup,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增试卷组表:"
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 修改试卷发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EditInfoState(PaperGroupInput input)
        {
            MessagesOutPut message = null;
            var model = _iPaperGroupRepository.Get(input.Id);
            //取消发布
            if (input.State == 0)
            {
                model.State = input.State;
                _iPaperGroupRepository.Update(model);
                message = new MessagesOutPut
                {
                    Success = true,
                    Message = "取消发布成功!"
                };
            }
            else if (input.State == 1) //发布试卷
            {
                model.State = input.State;
                _iPaperGroupRepository.Update(model);
                message = new MessagesOutPut
                {
                    Success = true,
                    Message = "发布成功!"
                };
            }
            return message;
        }

    }
}

