using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ListModuleInput : BasePageInput
    {
        /// <summary>
       /// 模块名称
       /// </summary>
        public string ModuleName { get; set; }

    }
}