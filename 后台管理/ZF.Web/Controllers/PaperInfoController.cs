using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.SubjectModule;
using ZF.Infrastructure.NPOI;

namespace ZF.Web.Controllers
{
    public class PaperInfoController : Controller
    {
        private readonly SubjectBigQuestionAppService _subjectBigQuestionAppService;

        public PaperInfoController(SubjectBigQuestionAppService subjectBigQuestionAppService)
        {
            _subjectBigQuestionAppService = subjectBigQuestionAppService;
        }

        // GET: PaperInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 自动组卷
        /// </summary>
        /// <returns></returns>
        public ActionResult AutomaticallyGroupVolume()
        {
            return PartialView();
        }

        /// <summary>
        /// 手工组卷
        /// </summary>
        /// <returns></returns>
        public ActionResult ManualGroupVolume(int type, string paperId, string paperState)
        {
            ViewBag.groupType = type;
            ViewBag.paperId = paperId;
            ViewBag.paperState = paperState;
            return PartialView();
        }

        public ActionResult EditInfo(string id)
        {
            ViewBag.Id = id ?? "";
            return PartialView();
        }

        /// <summary>
        /// 导出到xlsx
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public FileResult Export(string Id)
        {
            var list = _subjectBigQuestionAppService.GetPaperInfoQuestions(Id);
            foreach (var item in list)
            {
                item.QuestionTitle = ReplaceHtmlTag(item.QuestionTitle);
                item.QuestionContent = ReplaceHtmlTag(item.QuestionContent);
                item.Description = ReplaceHtmlTag(item.Description);
                item.Option1 = ReplaceHtmlTag(item.Option1);
                item.Option2 = ReplaceHtmlTag(item.Option2);
                item.Option3 = ReplaceHtmlTag(item.Option3);
                item.Option4 = ReplaceHtmlTag(item.Option4);
                item.Option5 = ReplaceHtmlTag(item.Option5);
                item.Option6 = ReplaceHtmlTag(item.Option6);
                item.Option7 = ReplaceHtmlTag(item.Option7);
                item.Option8 = ReplaceHtmlTag(item.Option8);
                item.StuAnswer = ReplaceHtmlTag(item.StuAnswer).Replace("1", "A").Replace("2", "B").Replace("3", "C").Replace("4", "D").Replace("5", "E").Replace("6", "F").Replace("7", "G").Replace("8", "H"); ;
            }
            var query = from q in list
                        select new
                        {
                            q.QuestionTitle,
                            q.Description,
                            q.QuestionContent,
                            q.Option1,
                            q.Option2,
                            q.Option3,
                            q.Option4,
                            q.Option5,
                            q.Option6,
                            q.Option7,
                            q.Option8,
                            q.StuAnswer,
                        };
            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "试题标题";
            dt.Columns[1].ColumnName = "大题描述";
            dt.Columns[2].ColumnName = "试题内容";
            dt.Columns[3].ColumnName = "A";
            dt.Columns[4].ColumnName = "B";
            dt.Columns[5].ColumnName = "C";
            dt.Columns[6].ColumnName = "D";
            dt.Columns[7].ColumnName = "E";
            dt.Columns[8].ColumnName = "F";
            dt.Columns[9].ColumnName = "G";
            dt.Columns[10].ColumnName = "H";
            dt.Columns[11].ColumnName = "正确答案";
            var ms = NPOIHelper.ExportDataTable(dt, "试卷试题明细导出");
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", "试卷试题明细导出.xls");
        }
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }


}