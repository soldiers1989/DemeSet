﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
   public interface IArticleRepository:IRepository<Article,Guid>
    {

    }
}
