using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.ModuleSiteConfiguration;
using ZF.Application.WebApiDto.ModuleSiteConfiguration;
using ZF.Core.Entity;
using ZF.Infrastructure.RedisCache;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 文本配置Api
    /// </summary>
    public class BaseDanyeController : ApiController
    {
        private readonly BaseDanyeAppService _repository;

        private readonly AfficheHelpAppService _afficheHelpAppService;

        private readonly SlideSettingAppService _slideSettingAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public BaseDanyeController(BaseDanyeAppService repository, SlideSettingAppService slideSettingAppService, AfficheHelpAppService afficheHelpAppService)
        {
            _repository = repository;
            _slideSettingAppService = slideSettingAppService;
            _afficheHelpAppService = afficheHelpAppService;
        }

        /// <summary>
        /// 通过编码获取相关配置文本
        /// </summary>
        /// <param name="arguName"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut GetArguValue(string arguName)
        {
            if (RedisCacheHelper.Exists("GetArguValue" + arguName))
            {
                return RedisCacheHelper.Get<MessagesOutPut>("GetArguValue" + arguName);
            }
            var model = _repository.GetArguValue(arguName);
            RedisCacheHelper.Add("GetArguValue" + arguName, model);
            return model;
        }

        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>

        [HttpPost]
        public JqGridOutPut<AfficheHelpOutput> GetAfficheHelp(AfficheHelpListInput input)
        {
            int count = 0;
            input.Sidx = "AddTime";
            var list = _repository.GetList(input, out count);
            return new JqGridOutPut<AfficheHelpOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        ///  获取资讯详情
        /// </summary>

        [HttpPost]
        public AfficheHelp GetAfficheHelpView(IdInput input)
        {
            var model = _afficheHelpAppService.Get(input.Id);
            return model;
        }


        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool InsertFeedback(FeedbackMolde input)
        {
            return _repository.InsertFeedback(input);
        }


        /// <summary>
        /// 获取字段分类
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DataTypeOutput> GetDataType(string code)
        {
            return _repository.GetDataType(code);
        }

        /// <summary>
        /// 获取首页幻灯片
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public List<SlideSettingOutput> GetSlideSettingList()
        {
            if (RedisCacheHelper.Exists("GetSlideSettingList"))
            {
                return RedisCacheHelper.Get<List<SlideSettingOutput>>("GetSlideSettingList");
            }
            var count = 0;
            var model = _slideSettingAppService.GetList(new SlideSettingListInput { State = 0, Type = 0, Sidx = "OrderNo" }, out count);
            RedisCacheHelper.Add("GetSlideSettingList", model);
            return model;
        }

        /// <summary>
        /// 获取课程幻灯片
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public List<SlideSettingOutput> GetSlideSettingCourseList()
        {
            var count = 0;
            return _slideSettingAppService.GetList(new SlideSettingListInput { State = 0, Type = 1 }, out count);
        }
    }
}
