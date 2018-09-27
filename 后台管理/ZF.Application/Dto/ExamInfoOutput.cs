using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.Dto
{
   public class ExamInfoOutput
    {
        public string Id { get; set; }
        /// <summary>
        /// 考试介绍
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 考试报名
        /// </summary>
        public string SignUp { get; set; }

        /// <summary>
        /// 考试内容
        /// </summary>
        public string Content { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 成绩管理
        /// </summary>
        public string ScoreManage { get; set; }
        /// <summary>
        /// 考试教材
        /// </summary>
        public string TextBox { get; set; }
        /// <summary>
        /// 是否启用 0:否  1：是
        /// </summary>
        public int IfUse { get; set; }
        /// <summary>
        /// 维护时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
