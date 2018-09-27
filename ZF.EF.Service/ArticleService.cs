using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.EF.Service
{
   public class ArticleService
    {
        private readonly PressDataEntities dbContext=new PressDataEntities();

        public MessagesOutPut AddOrEdit( ArticleInput input) {
            t_Article model;
            if ( !string.IsNullOrEmpty( input.Id ) )
            {
                 model = ( from t in dbContext.t_Article where t.Id == input.Id select t ).First( );
                model.ArticleTitle = input.ArticleTitle;
                model.ArticleImage = input.ArticleImage;
                model.ArticleContent = input.ArticleContent;
                model.Type = input.Type;
                dbContext.SaveChanges( );
                return new MessagesOutPut { Success=true,Message="修改成功"};
            }
            model = new t_Article( )
            {
                Id = Guid.NewGuid( ).ToString( ),
                ArticleTitle = input.ArticleTitle,
                ArticleImage = input.ArticleImage,
                ArticleContent = input.ArticleContent,
                Type = input.Type,
                AddTime = DateTime.Now,
                IsDelete=0
            };
            if ( dbContext.SaveChanges( ) > 0 ) {
                return new MessagesOutPut { Success = true, Message = "添加成功" };
            }
            return new MessagesOutPut { Success = false, Message = "添加失败" };
        }


        //public List<ArticleOutput> GetList( ArticleListInput input, out int count )
        //{

        //    Expression<Func<t_Article, bool>> func = u => true;
        //    func = u => u.Type==1;
        //    //if ( !string.IsNullOrEmpty(input.Type.ToString() ) )
        //    //{
        //    //    func&&func=u=>u.
        //    //}

        //    //var list=(from article in dbContext.t_Article )
        //    if ( !string.IsNullOrEmpty( input.Type.ToString( ) ) ) {
        //        dbContext.t_Article.Where( s => s.Type == input.Type );
        //    }

        //}


        public MessagesOutPut Delete( IdInputIds input ) {
            if ( !string.IsNullOrEmpty( input.Ids ) ) {
                var ids = input.Ids.Split( ',' );
                t_Article model;
                foreach ( var id in ids ) {
                    model = ( from obj in dbContext.t_Article where obj.Id == id select obj ).First( );
                    dbContext.t_Article.Remove( model);
                }
                return new MessagesOutPut { Success=true,Message="删除成功"};
            }
            return new MessagesOutPut { Success = false, Message = "删除失败" };
        }
    }
}
