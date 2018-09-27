using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZF.EF.Service
{
   public  class DbHelper 
    {
        //#region 公开方法

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        /// <param name="entity">带参实体</param>
        /// <returns></returns>
        public static int Add<T>(  DbContext dbContext, T entity ) where T : class, new()
        {
            dbContext.Set<T>( ).Add( entity );
            return dbContext.SaveChanges( );
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        /// <param name="dataArr">带参实体集合</param>
        /// <returns></returns>
        public static int Add<T>( DbContext dbContext, IEnumerable<T> dataArr ) where T : class, new()
        {
            try
            {
                dbContext.Set<T>( ).Concat<T>( dataArr );
                return dataArr.Count( );
            } catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        /// <param name="entity">带参实体</param>
        /// <returns></returns>
        public static int Update<T>( DbContext dbContext, T entity ) where T : class, new()
        {
            dbContext.Set<T>( ).Attach( entity );
            dbContext.Entry( entity ).State = EntityState.Modified;
            return dbContext.SaveChanges( );
        }

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        /// <param name="limit">限制条件</param>
        /// <param name="entity">需要修改的属性</param>
        /// <returns></returns>
        public static int Update<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit, T entity ) where T : class, new()
        {
            return dbContext.Set<T>( ).Where( limit ).Update( data => entity );
        }

        ///// <summary>
        ///// 通过SQL修改实体
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static int Update( this PressDataEntities dbContext, string sql, params object[] sqlParams )
        //{
        //    return dbContext.RunSql( sql, sqlParams );
        //}

        ///// <summary>
        ///// 移除实体并提交到数据服务器
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="entity">带参实体</param>
        ///// <returns></returns>
        //public static int Delete<T>( this PressDataEntities dbContext, T entity ) where T : class, new()
        //{
        //    dbContext.Set<T>( ).Remove( entity );
        //    return dbContext.SaveChange( );
        //}

        ///// <summary>
        ///// 移除实体并提交到数据服务器
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="limit">限制条件</param>
        ///// <returns></returns>
        //public static int Delete<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit ) where T : class, new()
        //{
        //    return dbContext.Set<T>( ).Where( limit ).Delete( );
        //}

        ///// <summary>
        ///// 通过SQL删除实体
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static int Delete( this PressDataEntities dbContext, string sql, params object[] sqlParams )
        //{
        //    return dbContext.RunSql( sql, sqlParams );
        //}

        ///// <summary>
        ///// 计算当前数据量
        ///// </summary>
        ///// <returns></returns>
        //public static int Count<T>( this PressDataEntities dbContext ) where T : class, new()
        //{
        //    return dbContext.Set<T>( ).Count( );
        //}

        ///// <summary>
        ///// 计算当前数据量
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="limit">限制条件</param>
        ///// <returns></returns>
        //public static int Count<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit ) where T : class, new()
        //{
        //    return dbContext.Set<T>( ).Where( limit ).Count( );
        //}

        ///// <summary>
        ///// 通过SQL计算当前数据量
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static int Count( this PressDataEntities dbContext, string sql, params object[] sqlParams )
        //{
        //    return dbContext.Database.SqlQuery<int>( sql, sqlParams ).SingleOrDefault( );
        //}

        ///// <summary>
        ///// 通过 主键id 获取带参实体
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="id">主键值</param>
        ///// <returns></returns>
        //public static T Model<T>( this PressDataEntities dbContext, params object[] id ) where T : class, new()
        //{
        //    return dbContext.Set<T>( ).Find( id );
        //}

        ///// <summary>
        ///// 获取带参实体
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="limit">限制条件</param>
        ///// <returns></returns>
        //public static T Model<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit ) where T : class, new()
        //{
        //    return dbContext.Set<T>( ).FirstOrDefault( limit );
        //}

        ///// <summary>
        ///// 通过SQL获取带参实体
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static T Model<T>( this PressDataEntities dbContext, string sql, params object[] sqlParams ) where T : class, new()
        //{
        //    return dbContext.QuerySql<T>( sql, sqlParams );
        //}

        ///// <summary>
        ///// 获取泛型列表
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <returns></returns>
        //public static IEnumerable<T> Enumerable<T>( this PressDataEntities dbContext ) where T : class, new()
        //{
        //    return dbContext.Set<T>( );
        //}

        ///// <summary>
        ///// 获取泛型列表
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="limit">限制条件</param>
        ///// <returns></returns>
        //public static IEnumerable<T> Enumerable<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit ) where T : class, new()
        //{
        //    return dbContext.Set<T>( ).Where( limit );
        //}

        ///// <summary>
        ///// 通过SQL获取泛型列表
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static IEnumerable<T> Enumerable<T>( this PressDataEntities dbContext, string sql, params object[] sqlParams ) where T : class, new()
        //{
        //    return dbContext.QuerySql<IEnumerable<T>>( sql, sqlParams );
        //}

        ///// <summary>
        ///// 获取泛型列表
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <returns></returns>
        //public static List<T> List<T>( this PressDataEntities dbContext ) where T : class, new()
        //{
        //    return dbContext.Enumerable<T>( ).ToList( );
        //}

        ///// <summary>
        ///// 获取泛型列表
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="limit">限制条件</param>
        ///// <returns></returns>
        //public static List<T> List<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit ) where T : class, new()
        //{
        //    return dbContext.Enumerable( limit ).ToList( );
        //}

        ///// <summary>
        ///// 通过SQL获取泛型列表
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static List<T> List<T>( this PressDataEntities dbContext, string sql, params object[] sqlParams ) where T : class, new()
        //{
        //    return dbContext.Enumerable<T>( sql, sqlParams ).ToList( );
        //}

        ///// <summary>
        ///// 获取 DataTable
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <returns></returns>
        //public static DataTable DataTable<T>( this PressDataEntities dbContext ) where T : class, new()
        //{
        //    return dbContext.List<T>( ).List2Dt( );
        //}

        ///// <summary>
        ///// 获取 DataTable
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="limit">限制条件</param>
        ///// <returns></returns>
        //public static DataTable DataTable<T>( this PressDataEntities dbContext, Expression<Func<T, bool>> limit ) where T : class, new()
        //{
        //    return dbContext.List( limit ).List2Dt( );
        //}

        ///// <summary>
        ///// 通过SQL获取DataTable
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static DataTable DataTable<T>( this PressDataEntities dbContext, string sql, params object[] sqlParams ) where T : class, new()
        //{
        //    return dbContext.List<T>( sql, sqlParams ).List2Dt( );
        //}

        ///// <summary>
        ///// 执行SQL语句，并返回执行结果
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static int RunSql( this PressDataEntities dbContext, string sql, params object[] sqlParams )
        //{
        //    return dbContext.ExecuteSql( sql, sqlParams );
        //}

        ///// <summary>
        ///// 执行SQL语句，并返回查询结果
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //public static T QuerySql<T>( this PressDataEntities dbContext, string sql, params object[] sqlParams ) where T : class
        //{
        //    return dbContext.SqlQuery<T>( sql, sqlParams );
        //}

        //#endregion

        //#region 私有方法

        ///// <summary>
        ///// 执行SQL，返回执行结果
        ///// </summary>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //private static int ExecuteSql( this PressDataEntities dbContext, string sql, params object[] sqlParams )
        //{
        //    var result = dbContext.Database.ExecuteSqlCommand( sql, sqlParams );
        //    return result;
        //}

        ///// <summary>
        ///// 执行SQL，返回查询结果集
        ///// </summary>
        ///// <typeparam name="T">返回类型</typeparam>
        ///// <param name="dbContext">数据上下文</param>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="sqlParams">SQL参数</param>
        ///// <returns></returns>
        //private static T SqlQuery<T>( this PressDataEntities dbContext, string sql, params object[] sqlParams ) where T : class
        //{
        //    return dbContext.Database.SqlQuery<T>( sql, sqlParams ) as T;
        //}

        ///// <summary>
        ///// 保存数据更改
        ///// </summary>
        //private static int SaveChange( this PressDataEntities dbContext )
        //{
        //    try
        //    {
        //        return dbContext.SaveChanges( );
        //    } catch ( Exception ex )
        //    {
        //        string errorMsg = "错误：";
        //        if ( ex.InnerException == null )
        //            errorMsg += ex.Message + "，";
        //        else if ( ex.InnerException.InnerException == null )
        //            errorMsg += ex.InnerException.Message + "，";
        //        else if ( ex.InnerException.InnerException.InnerException == null )
        //            errorMsg += ex.InnerException.InnerException.Message;
        //        throw new Exception( errorMsg );
        //    }
        //}

        //#endregion


        ///// <summary>
        ///// 分页查询 + 条件查询 + 排序
        ///// </summary>
        ///// <typeparam name="Tkey">泛型</typeparam>
        ///// <param name="pageSize">每页大小</param>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="total">总数量</param>
        ///// <param name="whereLambda">查询条件</param>
        ///// <param name="orderbyLambda">排序条件</param>
        ///// <param name="isAsc">是否升序</param>
        ///// <returns>IQueryable 泛型集合</returns>
        //public static IQueryable<T> LoadPageItems<Tkey>(this PressDataEntities dbContext, int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc )
        //{
        //    total = dbContext.Set<T>( ).Where( whereLambda ).Count( );
        //    if ( isAsc )
        //    {
        //        var temp = dbContext.Set<T>( ).Where( whereLambda )
        //                     .OrderBy<T, Tkey>( orderbyLambda )
        //                     .Skip( pageSize * ( pageIndex - 1 ) )
        //                     .Take( pageSize );
        //        return temp.AsQueryable( );
        //    } else
        //    {
        //        var temp = dbContext.Set<T>( ).Where( whereLambda )
        //                   .OrderByDescending<T, Tkey>( orderbyLambda )
        //                   .Skip( pageSize * ( pageIndex - 1 ) )
        //                   .Take( pageSize );
        //        return temp.AsQueryable( );
        //    }
        //}

    }
}
