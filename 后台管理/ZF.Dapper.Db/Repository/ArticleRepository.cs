using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Dapper.Db;

namespace ZF.Dapper.Db.Repository
{
   public class ArticleRepository:BaseRepositoryEntity<Article>,IArticleRepository
    {

    }
}
