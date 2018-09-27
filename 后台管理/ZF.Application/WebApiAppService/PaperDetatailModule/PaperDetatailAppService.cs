using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.PaperDetatailModule
{
    /// <summary>
    /// 试卷明细表  服务
    /// </summary>
    public class PaperDetatailAppService : BaseAppService<PaperDetatail>
    {

        private readonly IPaperDetatailRepository _repository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public PaperDetatailAppService(IPaperDetatailRepository repository):base(repository) {
            _repository = repository;
        }



    }
}