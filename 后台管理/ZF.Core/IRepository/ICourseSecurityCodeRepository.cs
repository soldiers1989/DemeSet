using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：课程防伪码管理 
    /// </summary>
    public interface ICourseSecurityCodeRepository : IRepository<CourseSecurityCode,Guid>
    {
	
    }
}

