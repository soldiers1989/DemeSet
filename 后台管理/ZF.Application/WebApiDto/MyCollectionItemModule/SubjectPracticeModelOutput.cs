using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.Dto;

namespace ZF.Application.WebApiDto.MyCollectionItemModule
{
    /// <summary>
    /// 
    /// </summary>
   public class SubjectPracticeModelOutput
    {
        /// <summary>
        /// 试题Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddTime { get; set; }

        /// <summary>
        /// 试题标题
        /// </summary>     
        public string QuestionTitle { get; set; }
        /// <summary>
        /// 试题内容
        /// </summary>     
        public string QuestionContent { get; set; }
        /// <summary>
        /// 选项1
        /// </summary>     
        public string Option1 { get; set; }
        /// <summary>
        /// 选项2
        /// </summary>     
        public string Option2 { get; set; }
        /// <summary>
        /// 选项3
        /// </summary>     
        public string Option3 { get; set; }
        /// <summary>
        /// 选项4
        /// </summary>     
        public string Option4 { get; set; }
        /// <summary>
        /// 选项5
        /// </summary>     
        public string Option5 { get; set; }
        /// <summary>
        /// 选项6
        /// </summary>     
        public string Option6 { get; set; }
        /// <summary>
        /// 选项7
        /// </summary>     
        public string Option7 { get; set; }
        /// <summary>
        /// 选项8
        /// </summary>     
        public string Option8 { get; set; }
        /// <summary>
        /// 选项数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 所属科目
        /// </summary>     
        public string SubjectId { get; set; }
        /// <summary>
        /// 试题所属知识点编码
        /// </summary>     
        public string KnowledgePointId { get; set; }
        /// <summary>
        /// 试题类型
        /// </summary>     
        public int SubjectType { get; set; }
        /// <summary>
        /// 试题分类
        /// </summary>     
        public string SubjectClassId { get; set; }
        /// <summary>
        /// 正确答案
        /// </summary>     
        public string RightAnswer { get; set; }
        /// <summary>
        /// 参考答案
        /// </summary>     
        public string ConsultAnswer { get; set; }
        /// <summary>
        /// 试题状态
        /// </summary>     
        public int State { get; set; }
        /// <summary>
        /// 试题文字解析
        /// </summary>     
        public string QuestionTextAnalysis { get; set; }
        /// <summary>
        /// 试题音频解析
        /// </summary>     
        public string QuestionAudioAnalysis { get; set; }
        /// <summary>
        /// 试题视频解析
        /// </summary>     
        public string QuestionVedioAnalysis { get; set; }
        /// <summary>
        /// 试题所属知识点名称
        /// </summary>
        public string KnowledgePointName { get; set; }

        /// <summary>
        /// 试题分类名称
        /// </summary>
        public string ClassName { get; set; }



        /// <summary>
        /// 我的答案
        /// </summary>
        public string StuAnswer { get; set; }

        /// <summary>
        /// 小题列表
        /// </summary>
        public List<SubjectSmallquestionOutput> SubjectSmallquestions { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// 视频编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }
        /// <summary>
        /// 1:正确 0 错误
        /// </summary>
        public int IsCorrect { get; set; }
    }
}
