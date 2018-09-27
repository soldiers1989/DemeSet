using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Dapper.Db.Repository;

namespace ZF.Dapper.Db.Repository
{
   public class My_StudyCardRepository: BaseRepositoryEntity<My_StudyCard>,IMy_StudyCardRepository
    {
    }
}
