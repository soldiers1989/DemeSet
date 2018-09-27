using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.MyCollectionItemModule
{
    /// <summary>
    /// 练习试题主表
    /// </summary>
   public class SubjectPracticeModelInput
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime? AddTime { get; set; }

        public int Type { get; set; }

        public List<SubjectPracticeQuestion> SubjectPracticeList { get; set; }
    }
}
/// <summary>
/// 练习试题明细表
/// </summary>
public class SubjectPracticeQuestion {
    public string Id { get; set; }

    public string PracticeNo { get; set; }

    public string BigQuestionId { get; set; }

    public string SmallQuestionId { get; set; }

    public string StuAnswer { get; set; }

    public int IsCorrect { get; set; }
}
