using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.ChapterExerciseModule
{
    public class CoursePaperRecordInput : BasePageInput
    {
        public string query { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public  string UserId { get; set; }
    }
}