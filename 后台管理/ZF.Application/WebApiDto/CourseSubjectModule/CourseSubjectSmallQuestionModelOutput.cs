using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CourseSubjectModule
{
    /// <summary>
    /// 
    /// </summary>
   public class CourseSubjectSmallQuestionModelOutput
    {
        /// <summary>
        /// 试题编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 试题标题
        /// </summary>     
        public string QuestionTitle { get; set; }
        /// <summary>
        /// 试题内容
        /// </summary>     
        public string QuestionContent { get; set; }
        /// <summary>
        /// 大题编码
        /// </summary>     
        public string BigQuestionId { get; set; }
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
        /// 试题类型
        /// </summary>     
        public int SubjectType { get; set; }
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
        /// 电子书页码
        /// </summary>     
        public string DigitalBookPage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddTime { set; get; }

        /// <summary>
        /// 试题关联视频
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }
    }
}
