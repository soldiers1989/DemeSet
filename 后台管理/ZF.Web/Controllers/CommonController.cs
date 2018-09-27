using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure;
using ZF.Infrastructure.Enum;
using ZF.Infrastructure.RedisCache;
using ZF.Infrastructure.zTree;

namespace ZF.Web.Controllers
{
    public class CommonController : BaseController
    {

        private readonly SubjectClassAppService _subjectClassAppService;
        private readonly SubjectAppService _subjectAppService;
        private readonly ProjectAppService _projectAppService;
        private readonly ModuleAppService _moduleAppService;
        private readonly MenuAppService _menuAppService;

        private readonly BaseDataAppService _baseDataAppService;
        private readonly ProjectClassAppService _projectClassAppService;
        private readonly DataTypeAppService _dataTypeAppService;
        private readonly UserLoginLogAppService _userLoginLogAppService;
        private readonly SubjectKnowledgePointAppService _subjectKnowledgePointAppService;
        private readonly StructureAppService _structureAppService;
        private readonly CourseOnTeachersAppService _courseOnTeachersAppService;
        private readonly CourseInfoAppService _courseInfoAppService;
        private readonly PaperParamDetailAppService _paperParamDetailAppService;

        private readonly CourseChapterAppService _courseChapterAppService;

        private readonly PaperPaperParamAppService _paperPaperParamAppService;

        private readonly RegisterUserAppService _registerUserAppService;

        private readonly PaperInfoAppService _paperInfoAppService;


        private readonly CourseVideoFileAppService _courseVideoFileAppService;

        private readonly PromoteCompanyAppService _promoteCompanyAppService;

        private readonly CourseVideoAppService _courseVideoAppService;

        private readonly DiscountCardAppService _discountCardAppService;

        private readonly OrderInstitutionsAppService _orderInstitutions;


        public CommonController(ModuleAppService moduleAppService, MenuAppService menuAppService, ProjectClassAppService projectClassAppService, DataTypeAppService dataTypeAppService, SubjectKnowledgePointAppService subjectKnowledgePointAppService, ProjectAppService projectAppService, SubjectAppService subjectAppService, SubjectClassAppService subjectClassAppService, UserLoginLogAppService userLoginLogAppService, StructureAppService structureAppService, CourseOnTeachersAppService courseOnTeachersAppService, CourseInfoAppService courseInfoAppService, PaperParamDetailAppService paperParamDetailAppService, PaperPaperParamAppService paperPaperParamAppService, CourseChapterAppService courseChapterAppService, BaseDataAppService baseDataAppService, RegisterUserAppService registerUserAppService, PaperInfoAppService paperInfoAppService, PromoteCompanyAppService promoteCompanyAppService, CourseVideoFileAppService courseVideoFileAppService, CourseVideoAppService courseVideoAppService, DiscountCardAppService discountCardAppService, OrderInstitutionsAppService orderInstitutions)

        {
            _moduleAppService = moduleAppService;
            _menuAppService = menuAppService;
            _projectClassAppService = projectClassAppService;
            _dataTypeAppService = dataTypeAppService;
            _subjectKnowledgePointAppService = subjectKnowledgePointAppService;
            _projectAppService = projectAppService;
            _userLoginLogAppService = userLoginLogAppService;
            _subjectAppService = subjectAppService;
            _subjectClassAppService = subjectClassAppService;
            _structureAppService = structureAppService;
            _courseOnTeachersAppService = courseOnTeachersAppService;
            _courseInfoAppService = courseInfoAppService;
            _paperParamDetailAppService = paperParamDetailAppService;

            _courseChapterAppService = courseChapterAppService;
            _baseDataAppService = baseDataAppService;
            _registerUserAppService = registerUserAppService;

            _paperPaperParamAppService = paperPaperParamAppService;
            _paperInfoAppService = paperInfoAppService;
            _promoteCompanyAppService = promoteCompanyAppService;
            _courseVideoFileAppService = courseVideoFileAppService;
            _courseVideoAppService = courseVideoAppService;
            _discountCardAppService = discountCardAppService;
            _orderInstitutions = orderInstitutions;
        }

        /// <summary>
        /// 获取修改类型对象
        /// </summary>
        /// <returns></returns>
        public ActionResult OperatorType()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(OperatorType); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(OperatorType)Enum.Parse(typeof(OperatorType), name)).ToString();
                data.Value = EnumHelper.GetEnumName<OperatorType>((int)(OperatorType)Enum.Parse(typeof(OperatorType), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取模块类型
        /// </summary>
        /// <returns></returns>
        public ActionResult ModuleList()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(Model); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(Model)Enum.Parse(typeof(Model), name)).ToString();
                data.Value = EnumHelper.GetEnumName<Model>((int)(Model)Enum.Parse(typeof(Model), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取模块-菜单集合
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuModuleList()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            var count = 0;
            var row = _moduleAppService.GetList(new ListModuleInput(), out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.ModuleName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取项目分类集合
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectClassificationList()
        {

            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            var count = 0;
            var row = _projectClassAppService.GetList(new ProjectClassListInput { Sidx = "Id" }, out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.ProjectClassName;
                list.Add(data);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取试卷结构集合
        /// </summary>
        /// <returns></returns>
        public ActionResult StructureList()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            var count = 0;
            var row = _structureAppService.GetList(new PaperStructureListInput(), out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.StuctureName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取模块-菜单集合
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuModuleTree()
        {
            List<Tree.zTree> row;
            if (RedisCacheHelper.Exists("MenuModuleTree"))
            {
                row = RedisCacheHelper.Get<List<Tree.zTree>>("MenuModuleTree");
            }
            else
            {
                row = _menuAppService.GetMenuModuleLists();
                RedisCacheHelper.Add("MenuModuleTree", row);
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectIdSelect(string ProjectClassId)
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            var count = 0;
            var row = _projectAppService.GetList(new ProjectListInput { ProjectClassId = ProjectClassId }, out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.ProjectName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectIdSelect2(string ProjectClassId)
        {

            List<Select2> list = new List<Select2>();
            var count = 0;
            var row = _projectAppService.GetList(new ProjectListInput(), out count);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.ProjectName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取知识点树
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectKnowledgePointLists(string subjectId)
        {
            var row = _subjectKnowledgePointAppService.SubjectKnowledgePointLists(subjectId);
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据试卷参数获取知识点树
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="paperParamId"></param>
        /// <returns></returns>

        public ActionResult SubjectKnowledgePointListsByPaperParamId(string subjectId, string PaperParamId, string structureDetailId, string detailId)
        {
            var row = _subjectKnowledgePointAppService.SubjectKnowledgePointLists(subjectId, PaperParamId, structureDetailId, detailId);
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取知识点，包含试题
        /// </summary>
        /// <param name="knowledgeCode">input.KnowledgePointId, input.DifficultLevel</param>
        /// <returns></returns>

        public ActionResult SubjectKnowledgePointAndpaperInfoLists(string KnowledgePointId, string DifficultLevel, string PaperId, string DetailId)
        {
            var row = _subjectKnowledgePointAppService.SubjectKnowledgePointAndpaperInfoLists(KnowledgePointId, DifficultLevel, PaperId, DetailId);
            return Json(row, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取知识点树  包含项目分类  项目 科目 
        /// 吴永福
        /// 20180312
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectKnowledgePointTree()
        {
            List<Tree.zTree> row;
            if (RedisCacheHelper.Exists("SubjectKnowledgePointTree"))
            {
                row = RedisCacheHelper.Get<List<Tree.zTree>>("SubjectKnowledgePointTree");
            }
            else
            {
                row = _subjectKnowledgePointAppService.SubjectKnowledgePointTree();
                RedisCacheHelper.Add("SubjectKnowledgePointTree", row);
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取课程节点树,包括项目分类->项目->科目
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseInfoPointTree()
        {
            List<Tree.zTree> row;
            if (RedisCacheHelper.Exists("SubjectTree"))
            {
                row = RedisCacheHelper.Get<List<Tree.zTree>>("SubjectTree");
            }
            else
            {
                row = _courseInfoAppService.CourseInfoPointTree();

                RedisCacheHelper.Add("SubjectTree", row);
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 项目分类-项目树
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectTree()
        {
            List<Tree.zTree> row;
            row = _courseInfoAppService.CourseProjectClassTree();
            return Json(row, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取课程下拉列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult CourseList(CourseInfoListInput input)
        {

            List<Dictionarys> list = new List<Dictionarys>();
            var row = _courseInfoAppService.GetList(input);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.CourseName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 课程select2列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Select2CourseList()
        {
            List<Select2> list = new List<Select2>();
            var row = _courseInfoAppService.GetList(new CourseInfoListInput { });
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.CourseName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询所有课程，包括套餐课程，单个课程
        /// </summary>
        /// <returns></returns>

        public ActionResult AllCourseList(int courseType)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var rows = _courseInfoAppService.GetAllCourseList(courseType);
            foreach (var item in rows)
            {

                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.CourseName;

                list.Add(data);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取科目下来--与课程级联
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectLevelList(SubjectListInput input)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var row = _subjectAppService.GetList(input);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.SubjectName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取章节下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseChapterTree(CourseChapterListInput input)
        {
            var row = _courseChapterAppService.GetChpaterTree(input);
            return Json(row, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取章节-视频节点树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult GetChpaterVideoTree(CourseChapterListInput input)
        {
            var row = _courseChapterAppService.GetChpaterVideoTree(input);
            return Json(row, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取科目树  包含项目分类  项目 科目 
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectTreeList()
        {
            List<Tree.zTree> row;
            if (RedisCacheHelper.Exists("SubjectTreeList"))
            {
                row = RedisCacheHelper.Get<List<Tree.zTree>>("SubjectTreeList");
            }
            else
            {
                row = _structureAppService.SubjectTreeList();
                RedisCacheHelper.Add("SubjectTreeList", row);
            }
            return Json(row, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取已选择的知识点树
        /// </summary>
        /// <returns></returns>
        public ActionResult ParamDetailTreeList(PaperParamDetailListInput input)
        {
            List<Tree.zTree> row;
            //if (RedisCacheHelper.Exists("ParamDetailTreeList"))
            //{
            //    row = RedisCacheHelper.Get<List<Tree.zTree>>("ParamDetailTreeList");
            //}
            //else
            //{
            //    row = _paperParamDetailAppService.ParamDetailTreeList( input);
            //    RedisCacheHelper.Add("ParamDetailTreeList", row, DateTime.Now.AddDays(1));
            //}
            row = _paperParamDetailAppService.ParamDetailTreeList(input);

            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(row),
                ContentType = "application/json"
            };

            //return Json(row, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取数据类型下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DataTypeList()
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var count = 0;
            var row = _dataTypeAppService.GetList(new DataTypeListInput(), out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.DataTypeName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取教师下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherList()
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var count = 0;
            var row = _courseOnTeachersAppService.GetList(new CourseOnTeachersListInput(), out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.TeachersName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 带模糊查询下拉
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectTeacherList()
        {
            List<Select2> list = new List<Select2>();
            var count = 0;
            var row = _courseOnTeachersAppService.GetList(new CourseOnTeachersListInput(), out count);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.TeachersName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 带模糊查询下拉
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectLinkCourseList(string id)
        {
            List<Select2> list = new List<Select2>();
            var row = _courseInfoAppService.SelectLinkCourseList(id);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.CourseName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 带模糊查询下拉  查询视频
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectVideoList()
        {
            List<Select2> list = new List<Select2>();
            var row = _courseVideoAppService.GetList(new CourseVideoListInput());
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.VideoName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectProjectList()
        {
            List<Select2> list = new List<Select2>();
            var count = 0;
            var row = _projectAppService.GetList(new ProjectListInput { }, out count);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.ProjectName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectList(string ProjectClassId)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var count = 0;
            var row = _projectAppService.GetList(new ProjectListInput { ProjectClassId = ProjectClassId }, out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.ProjectName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyList()
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var count = 0;
            var row = _promoteCompanyAppService.GetList(new PromoteCompanyListInput { }, out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.Name;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取用户登陆统计数据
        /// </summary>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public ActionResult GetStatics(int yearPart)
        {
            var o = _userLoginLogAppService.GetStatics(yearPart);
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据登陆方式获取用户统计数据
        /// </summary>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public ActionResult GetClassStatics(int yearPart)
        {
            var list = _userLoginLogAppService.GetClassStatics(yearPart);
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取试题分类下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectClassList(string subjectId)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var row = _subjectClassAppService.GetList(subjectId);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.ClassName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取机构下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult InstitutionsList()
        {
            List<Select2> list = new List<Select2>();
            var count = 0;
            var row = _orderInstitutions.GetList(new OrderInstitutionsListInput(), out count);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.Name;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取 科目下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectSubjectList(string projectId)
        {
            List<Select2> list = new List<Select2>();
            var row = _subjectAppService.GetAllList(projectId);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.SubjectName;
                list.Add(data);
            }
            RedisCacheHelper.Add("SubjectList", list);

            return Json(list, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 获取 科目下拉列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectList(string projectId)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var row = _subjectAppService.GetAllList(projectId);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.SubjectName;
                list.Add(data);
            }
            RedisCacheHelper.Add("SubjectList", list);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取模块类型
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionTypeList()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(QuestionType); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(QuestionType)Enum.Parse(typeof(QuestionType), name)).ToString();
                data.Value = EnumHelper.GetEnumName<QuestionType>((int)(QuestionType)Enum.Parse(typeof(QuestionType), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 难度等级
        /// </summary>
        /// <returns></returns>
        public ActionResult DifficultLevelList()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(DifficultLevel); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(DifficultLevel)Enum.Parse(typeof(DifficultLevel), name)).ToString();
                data.Value = EnumHelper.GetEnumName<DifficultLevel>((int)(DifficultLevel)Enum.Parse(typeof(DifficultLevel), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 试题状态
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionState()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(QuestionState); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(QuestionState)Enum.Parse(typeof(QuestionState), name)).ToString();
                data.Value = EnumHelper.GetEnumName<QuestionState>((int)(QuestionState)Enum.Parse(typeof(QuestionState), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 试题类型
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionType()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(QuestionType); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(QuestionType)Enum.Parse(typeof(QuestionType), name)).ToString();
                data.Value = EnumHelper.GetEnumName<QuestionType>((int)(QuestionType)Enum.Parse(typeof(QuestionType), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderState()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(OrderState); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(OrderState)Enum.Parse(typeof(OrderState), name)).ToString();
                data.Value = EnumHelper.GetEnumName<OrderState>((int)(OrderState)Enum.Parse(typeof(OrderState), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下单终端
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderType()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(OrderType); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                Dictionarys data = new Dictionarys();
                data.Key = ((int)(OrderType)Enum.Parse(typeof(OrderType), name)).ToString();
                data.Value = EnumHelper.GetEnumName<OrderType>((int)(OrderType)Enum.Parse(typeof(OrderType), name));
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 试题类型  排除案例分析
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionTypeRemoveSeven()
        {
            List<Dictionarys> list = new List<Dictionarys>(); //定义一个集合存储枚举内容
            Type t = typeof(QuestionType); //viewType为需要获取内容的枚举类型
            foreach (var name in Enum.GetNames(t))
            {
                if (((int)(QuestionType)Enum.Parse(typeof(QuestionType), name)).ToString() != "7")
                {
                    Dictionarys data = new Dictionarys();
                    data.Key = ((int)(QuestionType)Enum.Parse(typeof(QuestionType), name)).ToString();
                    data.Value = EnumHelper.GetEnumName<QuestionType>((int)(QuestionType)Enum.Parse(typeof(QuestionType), name));
                    list.Add(data);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通用方法  通过编码编号 获取数据字典内容
        /// </summary>
        /// <returns></returns>
        public ActionResult DataDictionary(string dataTypeId)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            if (string.IsNullOrWhiteSpace(dataTypeId))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            var count = 0;
            var row = _baseDataAppService.GetList(new BasedataListInput { DataTypeId = dataTypeId }, out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.Name;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public class Select2
        {
            public string id { get; set; }
            public string text { get; set; }
        }

        #region 试卷结构
        // <summary>
        // 查询试卷结构参数到下拉列表
        // </summary>
        // <returns></returns>
        public ActionResult GetListToSelect()
        {
            List<Dictionarys> list = new List<Dictionarys>();
            List<PaperPaperParamOutput> info = _paperPaperParamAppService.GetListToSelect();
            foreach (var item in info)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.ParamName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 获取终端用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectRegisterUser()
        {
            List<Select2> list = new List<Select2>();
            var count = 0;
            var row = _registerUserAppService.GetList(new RegisterUserListInput(), out count);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.NickNamw + "(" + item.TelphoneNum + ")";
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            List<Select2> list = new List<Select2>();
            var row = _registerUserAppService.GetList();
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.NickNamw + "(" + item.TelphoneNum + ")";
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取视频文件列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoList()
        {
            List<Select2> list = new List<Select2>();
            var count = 0;
            var row = _courseVideoFileAppService.GetList(new CourseVideoFileListInput(), out count);
            foreach (var item in row)
            {
                Select2 data = new Select2();
                data.id = item.Id;
                data.text = item.Name;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过DataTypeCode  获取数据类型数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBigClassIdList(string code)
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var count = 0;
            var row = _dataTypeAppService.GetList(new DataTypeListInput { DataTypeCode = code }, out count);
            foreach (var item in row)
            {
                Dictionarys data = new Dictionarys();
                data.Key = item.Id;
                data.Value = item.DataTypeName;
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取试卷列表
        /// </summary>
        /// <returns></returns>

        public ActionResult GetPaperList()
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var rows = _paperInfoAppService.GetList();
            foreach (var item in rows)
            {
                var dic = new Dictionarys();
                dic.Key = item.Id;
                dic.Value = item.PaperName;
                list.Add(dic);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取学习卡列表
        /// </summary>
        /// <returns></returns>

        public ActionResult GetDiscountCardList()
        {
            List<Dictionarys> list = new List<Dictionarys>();
            var rows = _discountCardAppService.GetList();
            foreach (var item in rows)
            {
                var dic = new Dictionarys();
                dic.Key = item.Id;
                dic.Value = item.CardName + "(" + item.Amount + ")";
                list.Add(dic);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public class Dictionarys
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }


    }
}