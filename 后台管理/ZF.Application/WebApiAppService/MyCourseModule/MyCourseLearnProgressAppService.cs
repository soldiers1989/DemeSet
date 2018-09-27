using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.MyCourseModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.MyCourseModule
{
    /// <summary>
    /// 我的课程学习进度
    /// </summary>
   public class MyCourseLearnProgressAppService:BaseAppService<CourseLearnProgress>
    {
        private  readonly ICourseLearnProgressRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public MyCourseLearnProgressAppService(ICourseLearnProgressRepository repository):base(repository){
            _repository = repository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseLearnProgress GetOne( MyCourseLearnProgressInput input ) {
            return _repository.Get( input.Id);
        }

        /// <summary>
        /// 是否已开始学习
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IfStartLearn(MyCourseLearnProgressInput input ) {
            var sql = string.Format("select count(*) from T_Course_LearnProgress where UserId='{0}' and CourseId='{1}'",input.UserId,input.CourseId);
            return Db.ExecuteScalar<int>( sql, null ) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit( MyCourseLearnProgressInput input ) {
            if ( !string.IsNullOrEmpty( input.Id ) ) {
                var sql = string.Format( "update T_Course_LearnProgress set state={0} where UserId='{1}' and CourseId='{2}' and ChapterId='{3}'" ,input.State,input.UserId,input.CourseId,input.ChapterId);
                Db.ExecuteScalar<int>( sql, null );
                return new MessagesOutPut { Success=true,Message="操作成功"};
            }
            _repository.Insert( new CourseLearnProgress {
                Id = Guid.NewGuid( ).ToString( ),
                UserId = input.UserId,
                CourseId = input.CourseId,
                ChapterId = input.ChapterId,
                State = input.State
            } );
            return new MessagesOutPut { Success = true, Message = "操作成功" };
        }
    }
}
