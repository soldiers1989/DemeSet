using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.AppService
{
   public class ArticleAppService: BaseAppService<Article>
    {
        private readonly IArticleRepository _repository;

        public ArticleAppService( IArticleRepository repository ) : base( repository )
        {
            _repository = repository;
        }

        /// <summary>
        /// 编辑或新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit( ArticleInput input )
        {
            Article model;
            if ( !string.IsNullOrEmpty( input.Id ) )
            {
                model = _repository.Get( input.Id );
                if ( model != null )
                {
                    model.Id = input.Id;
                    model.ArticleTitle = input.ArticleTitle;
                    model.ArticleContent = input.ArticleContent;
                    model.ArticleImage = input.ArticleImage;
                    _repository.Update( model );
                    return new MessagesOutPut { Success = true, Message = "修改成功" };
                }
            }
            model = input.MapTo<Article>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.ArticleTitle = input.ArticleTitle;
            model.ArticleContent = input.ArticleContent;
            model.ArticleImage = input.ArticleImage;
            model.AddTime = DateTime.Now;            
            var keyId = _repository.InsertGetId( model );
          
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取数据字典集合
        /// </summary>
        public List<ArticleOutput> GetList( ArticleListInput input, out int count )
        {
            const string sql = " select a.* ";
            var strSql = new StringBuilder( " from t_article a  where 1=1 and a.IsDelete=0 " );
            var dynamicParameters = new DynamicParameters( );
            const string sqlCount = "select count(*) ";
            //if ( input != null )
            //{
            //    if ( !string.IsNullOrWhiteSpace( input.Name ) )
            //    {
            //        strSql.Append( " and a.Name like  @Name " );
            //        dynamicParameters.Add( ":Name", '%' + input.Name + '%', DbType.String );
            //    }
            //    if ( !string.IsNullOrWhiteSpace( input.DataTypeId ) )
            //    {
            //        strSql.Append( " and a.DataTypeId =  @DataTypeId " );
            //        dynamicParameters.Add( ":DataTypeId", input.DataTypeId, DbType.String );
            //    }
            //    if ( !string.IsNullOrEmpty( input.Code ) )
            //    {
            //        strSql.Append( " and a.Code =  @Code " );
            //        dynamicParameters.Add( ":Code", input.Code, DbType.String );
            //    }
            //}
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<ArticleOutput>( GetPageSql( sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            return list;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut LogicDelete( IdInputIds input )
        {
            var strIds = input.Ids.TrimEnd( ',' );
            var arr = strIds.Split( ',' );

            foreach ( var item in arr )
            {
                var model = _repository.Get( item );
                if ( model != null )
                {
                    _repository.LogicDelete( model );
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }

        public MessagesOutPut PhysicalDelete( IdInputIds input )
        {
            var strIds = input.Ids.TrimEnd( ',' );
            var arr = strIds.Split( ',' );

            foreach ( var item in arr )
            {
                var model = _repository.Get( item );
                if ( model != null )
                {
                    _repository.Delete( model );
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }
    }
}
