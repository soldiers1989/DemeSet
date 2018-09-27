using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.MyCollectionItemModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.MyCollectionItemModule
{
    /// <summary>
    /// 
    /// </summary>
    public class MyCollectionItemAppService : BaseAppService<MyCollectionItem>
    {
        private readonly IMyCollectionItemRepository _repository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public MyCollectionItemAppService(IMyCollectionItemRepository repository) : base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public MessagesOutPut CancelCollectSubject(IdInput input,string userid)
        {
            //var model = _repository.Get(input.Id);
            //if (model != null)
            //{
            //    _repository.Delete(model);
            //}
            var sql = string.Format( " select * from t_My_CollectionItem where UserId='{0}' and QuestionId='{1}'",userid,input.Id);
            var model = Db.QueryFirstOrDefault<MyCollectionItem>( sql, null );
            if ( model != null ) {
                var delSql = string.Format( " delete  from t_My_CollectionItem where UserId='{0}' and QuestionId='{1}'", userid, input.Id );
                Db.ExecuteScalar<int>( delSql, null );
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }

        
        /// <summary>
        /// 收藏试题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddCollectSubject(MyCollectionItemModelInput input, string userID)
        {
            var model = input.MapTo<MyCollectionItem>();
            model.Id = Guid.NewGuid().ToString();
            model.QuestionId = input.QuestionId;
            model.UserId = userID;
            //model.UserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _repository.InsertGetId(model);
            return new MessagesOutPut { ModelId = keyId,Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 通过视频获取课程,课程章节等信息
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public MySubjectVideoInfoOutput GetVideoInfo( string videoId ) {
            var sql = string.Format( @" SELECT b.CourseId,a.ChapterId,a.Id AS VideoId FROM dbo.t_Course_Video a LEFT JOIN dbo.t_Course_Chapter b ON a.ChapterId = b.Id where a.Id='{0}'",videoId );
            return Db.QueryFirstOrDefault<MySubjectVideoInfoOutput>( sql, null );
        }
    }
}
