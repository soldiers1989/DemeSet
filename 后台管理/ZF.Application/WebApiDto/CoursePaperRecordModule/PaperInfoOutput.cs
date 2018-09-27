using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CoursePaperRecordModule
{
    /// <summary>
    /// 
    /// </summary>
   public class PaperInfoOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string PaperId { get; set; }
        /// <summary>
        /// 试卷名称
        /// </summary>     
        public string PaperName { get; set; }
        /// <summary>
        /// 考试时长
        /// </summary>     
        public int? TestTime { get; set; }
        /// <summary>
        /// 试卷使用次数
        /// </summary>
        public int UseTime { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public int ScoreSum { get; set; }

        public List<PaperInfoStructure> Structures { get; set; }

    }

    public class PaperInfoStructure {
        /// <summary>
        /// 试题类型
        /// </summary>
        public string QuestionTypeId { get; set; }
        /// <summary>
        /// 单个类型试题数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 单个试题类型总分
        /// </summary>
        public float Score { get; set; }
    }
}
