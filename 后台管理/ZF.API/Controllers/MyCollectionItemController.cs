using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.MyCollectionItemModule;
using ZF.Application.WebApiDto.MyCollectionItemModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MyCollectionItemController : BaseApiController
    {
        private readonly MyCollectionItemAppService _myCollectionItemAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myCollectionItemAppService"></param>
        public MyCollectionItemController( MyCollectionItemAppService myCollectionItemAppService ) {
            _myCollectionItemAppService = myCollectionItemAppService;
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut CancelCollectSubject( IdInput input ) {
            return _myCollectionItemAppService.CancelCollectSubject( input,UserObject.Id);
        }

        /// <summary>
        /// 添加试题收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddCollectSubject( MyCollectionItemModelInput input ) {
            return _myCollectionItemAppService.AddCollectSubject( input,UserObject.Id);
        }

        /// <summary>
        /// 通过videoId获取课程等信息
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpPost]
        public MySubjectVideoInfoOutput GetVideoInfo( string videoId ) {
            return _myCollectionItemAppService.GetVideoInfo( videoId);
        }
    }
}
