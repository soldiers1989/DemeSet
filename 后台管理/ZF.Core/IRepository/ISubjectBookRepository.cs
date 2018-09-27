using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：科目关联书籍管理 
    /// </summary>
    public interface ISubjectBookRepository : IRepository<SubjectBook,Guid>
    {
	
    }
}

