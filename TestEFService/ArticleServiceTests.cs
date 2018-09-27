using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZF.EF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.Dto;

namespace EF.Tests
{
    [TestClass( )]
    public class ArticleServiceTests
    {
        public readonly ArticleService service = new ArticleService( );

        [TestMethod( )]
        public void AddOrEditTest( )
        {
            service.AddOrEdit( new ArticleInput
            {
                ArticleTitle = "今日头条",
                ArticleImage = "",
                ArticleContent = "杀妻冰柜藏尸",
                Type = 1,
                AddTime = DateTime.Now,
                IsDelete = 0
            } );
        }
        [TestMethod( )]
        public void DeleteTest( )
        {
            service.Delete( new ZF.Application.BaseDto.IdInputIds {
                Ids=""
            } );
        }

    }
}