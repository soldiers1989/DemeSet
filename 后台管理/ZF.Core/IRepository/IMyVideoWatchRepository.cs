using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：视频观看历史记录 
    /// </summary>
    public interface IMyVideoWatchRepository : IRepository<MyVideoWatch,Guid>
    {
	
    }
}

