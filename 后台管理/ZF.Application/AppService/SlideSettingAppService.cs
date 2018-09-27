
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：幻灯片设置表 
    /// </summary>
    public class SlideSettingAppService : BaseAppService<SlideSetting>
    {
        private readonly ISlideSettingRepository _iSlideSettingRepository;

        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly FileRelationshipAppService _fileRelationshipAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSlideSettingRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        public SlideSettingAppService(ISlideSettingRepository iSlideSettingRepository, OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService) : base(iSlideSettingRepository)
        {
            _iSlideSettingRepository = iSlideSettingRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        /// 查询列表实体：幻灯片设置表 
        /// </summary>
        public List<SlideSettingOutput> GetList(SlideSettingListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Base_SlideSetting  a  where 1=1  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.LinkAddress))
            {
                strSql.Append(" and a.LinkAddress like @LinkAddress ");
                dynamicParameters.Add(":LinkAddress", '%' + input.LinkAddress + '%', DbType.String);
            }
            if (input.State.HasValue)
            {
                strSql.Append(" and a.State = @State ");
                dynamicParameters.Add(":State", input.State, DbType.Int32);
            }
            if (input.Type.HasValue)
            {
                strSql.Append(" and a.Type = @Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.Int32);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SlideSettingOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  幻灯片设置表
        /// </summary>
        public MessagesOutPut AddOrEdit(SlideSettingInput input)
        {
            RedisCacheHelper.Remove("GetSlideSettingList");
            SlideSetting model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iSlideSettingRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.LinkAddress = input.LinkAddress;
                model.OrderNo = input.OrderNo;
                model.Remark = input.Remark;
                model.State = input.State;
                model.AppLinkAddress = input.AppLinkAddress;
                model.Url = DefuleDomain +"/"+ input.IdFilehiddenFile;
                model.AppUrl = DefuleDomain + "/" + input.IdFilehiddenFile1;
                #endregion
                _iSlideSettingRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SlideSetting,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改幻灯片设置表:" + input.Remark
                });
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                    Type = 0
                });
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile1,
                    Type = 1
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SlideSetting>();
            model.Url = DefuleDomain + "/" + input.IdFilehiddenFile;
            model.AppUrl = DefuleDomain + "/" + input.IdFilehiddenFile1;
            model.Id = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            var keyId = _iSlideSettingRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SlideSetting,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增幻灯片设置表:" + input.Remark
            });
            _fileRelationshipAppService.Add(new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = input.IdFilehiddenFile,
                Type = 0
            });
            _fileRelationshipAppService.Add(new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = input.IdFilehiddenFile1,
                Type = 1
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

