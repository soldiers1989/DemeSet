
using System;
using System.Collections.Generic;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：CourseSubject 
    /// </summary>
    public class CourseSubjectController : BaseController
    {
	   private readonly CourseSubjectAppService _courseSubjectAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public CourseSubjectController(CourseSubjectAppService  courseSubjectAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_courseSubjectAppService =courseSubjectAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：CourseSubject 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<CourseSubjectOutput> GetList(CourseSubjectListInput input)
        {
            var count = 0;
            var list = _courseSubjectAppService.GetList(input, out count);
            return new JqGridOutPut<CourseSubjectOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

	
	   /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _courseSubjectAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int) Model.CourseSubject,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = string.Format("删除课程试题:{0}-{1}", model.CourseId, model.SubjectId)
                    });
                    _courseSubjectAppService.Delete(model);
                }
                else
                {
                    return new MessagesOutPut { Id = 1, Message = "删除失败,未找到对应的记录!", Success = false };
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：CourseSubject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseSubjectInput input)
        {
            var data = _courseSubjectAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }


        /// <summary>
        /// 批量提添加课程章节试题
        /// </summary>
        /// <param name="courseId">课程编码</param>
        /// <param name="chapterId">章节编码</param>
        /// <param name="subjectIdList">试题编码集合</param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddCourseSubjectList( string courseId,string chapterId,string strQuestionIds) {
            if ( !string.IsNullOrEmpty( courseId ) && !string.IsNullOrWhiteSpace( chapterId ) &&strQuestionIds!="" ) {
                var data = new MessagesOutPut( ) ;
                var flag = true;
                var subjectIdList = strQuestionIds.TrimEnd( ',' ).Split( ',' );
                foreach ( var subjectId in subjectIdList ) {

                    if ( _courseSubjectAppService.Exist( new CourseSubjectInput
                    {
                        CourseId = courseId,
                        ChapterId = chapterId,
                        SubjectId = subjectId
                    } ) )
                    {
                        flag = false;
                        continue;
                    }
                    data= _courseSubjectAppService.AddOrEdit( new CourseSubjectInput {
                        CourseId=courseId,
                        ChapterId=chapterId,
                        SubjectId= subjectId
                    } );
                }
                if ( !flag ) {
                    return new MessagesOutPut { Message = "存在试题已在章节列表中，已自动过滤掉重复数据！", Success = true };
                }
                return new MessagesOutPut {  Message = "添加成功", Success = true };
            }
            return new MessagesOutPut { Success = true, Message = "添加失败!" };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public CourseSubject GetOne(IdInput input)
        {
            var model = _courseSubjectAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 根据条件获取章节已有的试题
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<SubjectBigQuestionOutput> GetExistBigQuestionWithSmallQuestion( SubjectBigQuestionInputForCourseChapter input ) {
            var count = 0;
            var list = _courseSubjectAppService.GetExistBigQuestionWithSmallQuestion( input, out count );
            return new JqGridOutPut<SubjectBigQuestionOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
    }
}
