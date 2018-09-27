using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.ModuleSiteConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(MyFeedback))]
    public class FeedbackMolde
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Advice { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Relation { get; set; }
    }
}