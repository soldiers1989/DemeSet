using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.MyCollectionModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.MyCollectionModule
{
    public class MyCollectionAppService : BaseAppService<MyCollection>
    {
        private readonly IMyCollectionRepository _repository;

        public MyCollectionAppService( IMyCollectionRepository repository ) : base( repository )
        {
            _repository = repository;
        }

        /// <summary>
        /// 添加我的课程收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddCollection( MyCollectionModuleInput input )
        {
            MyCollection model;
            if ( !string.IsNullOrEmpty( input.Id ) )
            {
                model = _repository.Get( input.Id );
                if ( model != null )
                {
                    model.CourseId = input.CourseId;
                    model.UserId = input.UserId;
                    _repository.Update( model );
                    return new MessagesOutPut { Success = true, Message = "修改成功" };
                }
            }
            model = input.MapTo<MyCollection>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.UserId = input.UserId;
            model.AddTime = DateTime.Now;
            model.CourseId = input.CourseId;
            _repository.InsertGetId( model );
            return new MessagesOutPut { Success = true, Message = "新增成功" };
        }

        /// <summary>
        /// 收藏课程视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut CollectCourseVideo( MyCollectionModuleInput input )
        {
            MyCollection model;
            //if ( !string.IsNullOrEmpty( input.Id ) )
            //{
            //    model = _repository.Get( input.Id );
            //    if ( model != null )
            //    {
            //        model.CourseId = input.CourseId;
            //        model.UserId = input.UserId;
            //        _repository.Update( model );
            //        return new MessagesOutPut { Success = true, Message = "修改成功" };
            //    }
            //}
            model = input.MapTo<MyCollection>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.UserId = input.UserId;
            model.AddTime = DateTime.Now;
            model.CourseId = input.CourseId;
            model.VideoId = input.VideoId;
            _repository.InsertGetId( model );
            return new MessagesOutPut { Success = true, Message = "新增成功" };
        }

        /// <summary>
        /// 取消收藏视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut CancelCollectVideo( MyCollectionModuleInput input ) {
            var sql = string.Format(" select * from t_My_Collection where UserId='{0}' and CourseId='{1}' and VideoId='{2}'",input.UserId,input.CourseId,input.VideoId);
            var model = Db.QueryFirstOrDefault<MyCollection>( sql,null);
            if ( model != null ) {
                var strSql =string.Format( " delete from t_My_Collection where UserId='{0}' and CourseId='{1}' and VideoId='{2}'", input.UserId, input.CourseId, input.VideoId );
                Db.ExecuteScalar<int>( strSql, null );
            }
            return new MessagesOutPut { Success=true,Message="操作成功"};
        }


        public MessagesOutPut IsVideoCollected( MyCollectionModuleInput input )
        {
            var sql = string.Format( " select count(*) from t_My_Collection where UserId='{0}' and CourseId='{1}' and VideoId='{2}'", input.UserId, input.CourseId, input.VideoId );
            var result =Db.ExecuteScalar<int>( sql, null );
            if ( result > 0 ) {
                return new MessagesOutPut { Success=true};
            }
             return new MessagesOutPut { Success = false };
        }

        /// <summary>
        /// 获取收藏课程视频列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<MyCollectionModuleOutput> GetList( MyCollectionModuleListInput input, out int count )
        {
            const string sql = "SELECT b.CourseName,a.AddTime,c.VideoName ,c.Id as VideoId ,a.CourseId,b.CourseIamge";
            const string sqlCount = "select count(*) ";
            var strSql = new StringBuilder( @" FROM dbo.t_My_Collection a LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id LEFT JOIN dbo.t_Course_Video c ON a.videoId=c.Id WHERE  a.UserId=@UserId " );
            var dynamicParameters = new DynamicParameters( );
            dynamicParameters.Add( ":UserId",input.UserId,DbType.String);
            if ( !string.IsNullOrWhiteSpace( input.CourseName ) )
            {
                strSql.Append( " and b.CourseName like @CourseName " );
                dynamicParameters.Add( ":CourseName", '%' + input.CourseName + '%', DbType.String );
            }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<MyCollectionModuleOutput>( GetPageSql( sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, "AddTime", "desc" ), dynamicParameters );
            return list;

        }
        /// <summary>
        /// 判断是否已收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsCollected( MyCollectionModuleInput input )
        {
            var sql = " select * from t_My_Collection where UserId=@UserId and CourseId=@CourseId ";
            var parameters = new DynamicParameters( );
            parameters.Add( ":UserId", input.UserId, DbType.String );
            parameters.Add( ":CourseId", input.CourseId, DbType.String );
            var obj = Db.QueryFirstOrDefault<MyCollection>( sql, parameters );
            return obj != null;
        }


        /// <summary>
        /// 取消课程收藏
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut CancelCollection( MyCollectionModuleInput input )
        {
            var sql = "select * from t_My_Collection where UserId=@UserId AND CourseId=@CourseId";
            var parameters = new DynamicParameters( );
            parameters.Add( ":UserId", input.UserId, DbType.String );
            parameters.Add( ":CourseId", input.CourseId, DbType.String );
            var model = Db.QueryFirstOrDefault<MyCollection>( sql, parameters );
            if ( model != null )
            {
                _repository.Delete( model );
            }
            return new MessagesOutPut { Success = true, Message = "操作成功" };
        }


    }
}
