using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.SystemModule;
using ZF.Application.WebApiDto.CourseChapterModule;
using ZF.Application.WebApiDto.CourseModule;
using ZF.Application.WebApiDto.CoursePackModule;
using ZF.Application.WebApiDto.CourseRelatedModule;
using ZF.Application.WebApiDto.MyCollectionModule;
using ZF.Application.WebApiDto.SystemModule;
using ZF.Core.Entity;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.SmsService;
using CourseInfoAppService = ZF.Application.WebApiAppService.CourseModule.CourseInfoAppService;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseInfoController : ApiController
    {
        private readonly CourseInfoAppService _courseInfoAppService;

        private readonly CoursePaperAppService _coursePaperAppService;

        private readonly CourseAppraiseAppService _courseAppraiseAppService;

        private readonly ProjectApiService _projectApiService;

        private readonly SubjectAppService _subjectAppService;

        private readonly CourseSecurityCodeAppService _courseSecurityCodeAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="courseInfoAppService"></param>
        /// <param name="coursePaperAppService"></param>
        /// <param name="courseAppraiseAppService"></param>
        /// <param name="projectApiService"></param>
        /// <param name="subjectAppService"></param>
        /// <param name="courseSecurityCode"></param>
        public CourseInfoController(CourseInfoAppService courseInfoAppService,
            CoursePaperAppService coursePaperAppService, CourseAppraiseAppService courseAppraiseAppService, ProjectApiService projectApiService, SubjectAppService subjectAppService, CourseSecurityCodeAppService courseSecurityCode)
        {
            _courseInfoAppService = courseInfoAppService;
            _coursePaperAppService = coursePaperAppService;
            _courseAppraiseAppService = courseAppraiseAppService;
            _projectApiService = projectApiService;
            _subjectAppService = subjectAppService;
            _courseSecurityCodeAppService = courseSecurityCode;
        }


        /// <summary>
        ///  获取课程带分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseInfoModel> GetCourseInfoAll(CourseInfoModelInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetCourseInfoAll(input, out count);
            return new JqGridOutPut<CourseInfoModel>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 根据标签获取推荐课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseInfoModel> GetRecommendCourseByTagExceptSelf(CourseInfoModelInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetRecommendCourseByTagExceptSelf(input);
            return new JqGridOutPut<CourseInfoModel>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        ///  获取项目分类下面的项目和科目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ProjectClassModel1> GetProjectClass()
        {
            var list = _courseInfoAppService.GetProjectClass();
            return list;
        }


        /// <summary>
        ///  获取项目下面的科目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ProjectModel> GetProject()
        {
            var list = _courseInfoAppService.GetProject();
            return list;
        }

        /// <summary>
        ///  获取所有项目分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ProjectClassModel> GetProjectClassAll()
        {
            var list = _projectApiService.GetProjectClassAll();
            return list;
        }

        /// <summary>
        /// 获取项目分类 详细信息
        /// </summary>
        /// <param name="projectClassId"></param>
        /// <returns></returns>
        [HttpPost]
        public ProjectClass GetProjectClassId(string projectClassId)
        {
            var list = _projectApiService.GetOne(projectClassId);
            return list;
        }

        /// <summary>
        ///  获取项目下面的科目和课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ProjectCourseModel> GetProjectCourseInfo()
        {
            var list = _courseInfoAppService.GetProjectCourseInfo();
            return list;
        }

        /// <summary>
        /// 获取指定课程信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseInfoModelOutput GetOne(IdInput para)
        {
            return _courseInfoAppService.GetOne(para.Id);
        }

        /// <summary>
        /// 获取视频页面相关的课程信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseInfoVideoOutput GetCourseVideo(IdInput para)
        {
            return _courseInfoAppService.GetCourseVideo(para.Id);
        }

        /// <summary>
        ///  获取课程排名 前十条
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<PaidListingsModel> GetPaidListings(PaidListingsInput input)
        {

            var list = _courseInfoAppService.GetPaidListings(input);
            return list;
        }

        /// <summary>
        /// 根据课程讲师获取课程列表 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModelOutput> GetList(CourseInfoModelInput input)
        {
            return _courseInfoAppService.GetList(input);
        }




        /// <summary>
        /// 获取与指定课程所属相同的项目下的课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModelOutput> GetRandomListExceptCurrent(string courseId)
        {
            return _courseInfoAppService.GetRandomListExceptCurrent(courseId);
        }



        /// <summary>
        /// 获取和面授课程匹配的打包课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModelOutput> GetRandomListFaceToFace(string courseId)
        {
            return _courseInfoAppService.GetRandomListFaceToFace(courseId);
        }

        /// <summary>
        /// 获取面授课程的主讲教师
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CourseOnTeachers> GetOnTeachers(string courseId)
        {
            return _courseInfoAppService.GetOnTeachers(courseId);
        }


        /// <summary>
        /// 获取指定课程所属打包课程集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CoursePackcourseInfoModelOutput> GetPackcourseInfoList(string courseId)
        {
            return _courseInfoAppService.GetPackcourseInfoList(courseId);
        }


        /// <summary>
        /// 查询课程
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseInfoModelOutput> SeacrchCoruse(CourseInfoModelInput input, out int count)
        {
            var list = _courseInfoAppService.GetSearchList(input, out count);
            return new JqGridOutPut<CourseInfoModelOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 通过课程编号获取科目，项目编号信息
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <returns></returns>
        [HttpPost]
        public ProjectSubjectOutput GetProjectSubject(string courseId)
        {
            return _courseInfoAppService.GetProjectSubject(courseId);
        }



        /// <summary>
        ///
        /// </summary>
        /// <param name="input">课程编号</param>
        /// <returns></returns>
        [HttpPost]
        public List<AdvertisingOutput> Advertising(IdInput input)
        {
            return _courseInfoAppService.Advertising(input.Id, UserObject.Id, UserObject.SubjectId);
        }


        /// <summary>
        /// 用户类
        /// </summary>
        protected RegisterUserOutputDto UserObject
        {
            get
            {
                if (Request.Headers.Authorization?.Parameter != null)
                {
                    var authorization = Request.Headers.Authorization.Parameter;
                    //解密Ticket
                    var formsAuthenticationTicket = Des3Cryption.Decrypt3DES(authorization);
                    if (!string.IsNullOrEmpty(formsAuthenticationTicket))
                    {
                        var strTicket = formsAuthenticationTicket;
                        //从Ticket里面获取用户名和密码
                        string telphoneNum = "";
                        if (!string.IsNullOrEmpty(strTicket))
                        {
                            var index = strTicket.Split('&');
                            telphoneNum = index[0];
                        }
                        var model = new RegisterUserRepository().GetLogin(telphoneNum);
                        return model;
                    }
                    return null;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取我的专业   相关科目  课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public MyCourseSubjectOutput GetCourseSubject(CourseInfoModelInput input)
        {
            var model = new MyCourseSubjectOutput();
            var subject = _subjectAppService.Get(input.SubjectId);
            if (subject != null)
            {
                model.SubjectId = subject.Id;
                model.SubjectName = subject.SubjectName;
                model.SubjectModel = _projectApiService.GetSubjectAll(new SubjectModelInput { ProjectId = subject.ProjectId });
            }
            return model;
        }



        /// <summary>
        /// 获取课程试题列表
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public List<CoursePaperOutput> GetPaperList(string courseId)
        {
            if (UserObject != null)
            {
                return _coursePaperAppService.GetList(courseId, UserObject.Id);
            }
            return _coursePaperAppService.GetList(courseId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CoursePaperOutput> GetPaperListWithType(string courseId, int type)
        {
            if (UserObject != null)
            {
                return _coursePaperAppService.GetListByType(courseId, UserObject.Id, type);
            }
            return _coursePaperAppService.GetListByType(courseId, type);
        }

        /// <summary>
        /// 获取推荐书籍
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public List<SubjectBookOutput> GetSubjectBook(string courseId)
        {
            return _coursePaperAppService.GetSubjectBook(courseId);
        }


        /// <summary>
        /// 推荐教材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<SubjectBookOutput> GetBooks(CourseInfoListInput input)
        {
            var count = 0;
            var list = _coursePaperAppService.GetBooks(input, out count);
            return new JqGridOutPut<SubjectBookOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 获取学习指定课程用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<UserLearnOutput> GetLearnList(CourseLearnUserInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetLearnPerson(input, out count);
            return new JqGridOutPut<UserLearnOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateViewCount(CourseInfoInput input)
        {
            return _courseInfoAppService.UpdateViewCount(input);
        }
        /// <summary>
        /// 获取标题 描述 关键字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public ModelJob GetTitle(TitleInput input)
        {
            return _courseInfoAppService.GetTitle(input);
        }


        /// <summary>
        /// 根据视频code  获取课程编号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCourseId(IdInput input)
        {
            return _courseInfoAppService.GetCourseId(input.Id);
        }

        /// <summary>
        /// 根据课程编号获取各子菜单的显示状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseStatus GetCourseStatus(IdInput input)
        {
            return _courseInfoAppService.GetCourseStatus(input.Id);
        }

        /// <summary>
        /// 获取题库关联课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModelOutput> GetLinkCourse(IdInput input)
        {
            return _courseInfoAppService.GetLinkCourse(input);
        }
    }
}