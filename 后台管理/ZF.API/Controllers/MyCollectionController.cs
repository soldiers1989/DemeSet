using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.MyCollectionModule;
using ZF.Application.WebApiAppService.MyCourseModule;
using ZF.Application.WebApiDto.MyCollectionModule;
using ZF.Application.WebApiDto.MyCourseModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 我的模块  服务
    /// </summary>
    public class MyCollectionController : BaseApiController
    {
        private readonly MyCollectionAppService _myCollectionAppService;

        private readonly MyFootprintAppService _footprintAppService;

        private readonly MyVideoWatchAppService _myVideoWatchAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myCollectionAppService"></param>
        /// <param name="footprintAppService"></param>
        /// <param name="myVideoWatchAppService"></param>
        public MyCollectionController(MyCollectionAppService myCollectionAppService, MyFootprintAppService footprintAppService, MyVideoWatchAppService myVideoWatchAppService)
        {
            _myCollectionAppService = myCollectionAppService;
            _footprintAppService = footprintAppService;
            _myVideoWatchAppService = myVideoWatchAppService;
        }


        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddCollection(MyCollectionModuleInput input)
        {
            input.UserId = UserObject.Id;
            return _myCollectionAppService.AddCollection(input);
        }


        /// <summary>
        /// 收藏课程视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut CollectCourseVideo( MyCollectionModuleInput input) {
            input.UserId = UserObject.Id;
            return _myCollectionAppService.CollectCourseVideo( input);
        }

        /// <summary>
        /// 取消收藏视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut CancelCollectVideo( MyCollectionModuleInput input ) {
            input.UserId = UserObject.Id;
            return _myCollectionAppService.CancelCollectVideo( input);
        }


        /// <summary>
        /// 课程视频是否收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut IsVideoCollected( MyCollectionModuleInput input) {
            input.UserId = UserObject.Id;
            return _myCollectionAppService.IsVideoCollected( input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<MyCollectionModuleOutput> GetList( MyCollectionModuleListInput input) {
            input.UserId = UserObject.Id;
            var count = 0;
            var list = _myCollectionAppService.GetList( input, out count );
            return new JqGridOutPut<MyCollectionModuleOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut CancelCollection(MyCollectionModuleInput input)
        {
            input.UserId = UserObject.Id;
            return _myCollectionAppService.CancelCollection(input);
        }


        /// <summary>
        /// 判断是否已经收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IsCollected(MyCollectionModuleInput input)
        {
            input.UserId = UserObject.Id;
            return _myCollectionAppService.IsCollected(input);
        }

        /// <summary>
        /// 添加我的足迹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddFootprint(MyFootprintInput input)
        {
            input.UserId = UserObject.Id;
            return _footprintAppService.AddOrEdit(input);
        }

        /// <summary>
        /// 获取我的足迹记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<MyFootprintOutput> GetFootprint(MyFootprintListInput input)
        {

            int count = 0;
            input.UserId = UserObject.Id;
            var list = _footprintAppService.GetList(input, out count);
            return new JqGridOutPut<MyFootprintOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        /// <summary>
        /// 添加我的视频观看记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddVideoWatch(MyVideoWatchInput input)
        {
            input.UserId = UserObject.Id;
            return _myVideoWatchAppService.AddOrEdit(input);
        }

        /// <summary>
        /// 获取我的视频观看记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<MyVideoWatchOutput> GetVideoWatch(MyVideoWatchListInput input)
        {

            int count = 0;
            input.UserId = UserObject.Id;
            var list = _myVideoWatchAppService.GetList(input, out count);
            return new JqGridOutPut<MyVideoWatchOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 更新我的科目
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateSubject(string subjectId)
        {
            return _myVideoWatchAppService.UpdateSubject(subjectId,UserObject.Id);
        }
    }
}
