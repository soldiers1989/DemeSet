using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：意见反馈表 
    /// </summary>
    public interface IMyFeedbackRepository : IRepository<MyFeedback,Guid>
    {
	
    }
}

