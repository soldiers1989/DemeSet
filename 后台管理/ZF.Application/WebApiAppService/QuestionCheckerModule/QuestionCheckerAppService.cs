using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.CourseSubjectModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.QuestionCheckerModule
{
   public class QuestionCheckerAppService:BaseAppService<QuestionChecker>
    {
        private readonly IQuestionCheckerRepository _repository;
        public QuestionCheckerAppService( IQuestionCheckerRepository repository):base(repository) {
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Add( QuestionCheckerInput input) {
            var model = input.MapTo<QuestionChecker>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.Content = input.Content;
            model.UserId = input.UserId;
            model.AddTime = DateTime.Now;
            _repository.Insert( model );
            return new MessagesOutPut { Success = true, Message = "新增成功" };
        }
    }
}
