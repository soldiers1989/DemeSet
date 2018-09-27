using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Model;
using Oracle.ManagedDataAccess.Client;

namespace Model
{
    public abstract class TopeveryRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<TopeveryDbContext, TEntity, TPrimaryKey>, ITopeveryRepositoryBase
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly string _connection = ConfigurationManager.ConnectionStrings["Default"].ToString();

        protected TopeveryRepositoryBase(IDbContextProvider<TopeveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //在下面添加存储层公共方法

        /// <summary>
        /// 获取总数的SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public string GetCountSql(string tableName,string whereSql)
        {
            return $@"SELECT count(1) FROM {tableName} {whereSql}";
        }

        /// <summary>
        /// 获取查询的SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereSql"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetPageSql(string tableName, string whereSql, string sort)
        {
            return string.Format(@"SELECT tmp.* FROM (SELECT row_number() over(order by {2}) AS rw,t.* FROM {0} t {1}) tmp
            WHERE rw between :p_begin_index AND :p_end_index", tableName, whereSql, sort);
        }

        public string GetPageSql(string sql)
        {
            return string.Format(@"select * from ( select rownum rn,temp.* from ({0}) temp) where rn between :p_begin_index AND :p_end_index", sql);
        }

        /// <summary>
        /// 逻辑删除表记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">主键Id</param>
        public async Task DelTableInfoAsync(string tableName, int id)
        {
           
        }


        /// <summary>
        /// 逻辑删除表记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">主键Id</param>
        public void DelTableInfo(string tableName, int id)
        {
            StringBuilder sbBuilder = new StringBuilder();
            sbBuilder.AppendFormat(@"delete from {0} where id=:p_id", tableName);
             Context.Database.ExecuteSqlCommand(sbBuilder.ToString(),
                    new OracleParameter(":p_id", OracleDbType.Int32, id, ParameterDirection.Input));
        }

     
        /// <summary>
        /// 执行Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task ExecutionSqlAsync(string sql)
        {
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public void AddRange<T>(IList<T> list) 
        {
            var entities = list as IList<TEntity>;
            if (entities != null)
            {
              Context.Set<TEntity>().AddRange(entities);
            }
        }
    }

    public class TopeveryDbContext:AbpDbContext
    {

    }

    public abstract class TopeveryRepositoryBase<TEntity> : TopeveryRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected TopeveryRepositoryBase(IDbContextProvider<TopeveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //这里不能添加任何方法, 在上一个类里面添加 (这个类继承了上一个类)

       
    }
}
