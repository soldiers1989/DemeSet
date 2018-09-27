using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DapperExtensions.Sql;
using ZF.Core;

namespace ZF.Dapper.Db
{
    /// <summary>
    /// 数据库仓储基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TV">int的值类型</typeparam>
    public abstract class BaseRepository<T, TV> : IRepository<T, TV> where T : BaseEntity<TV> where TV : struct
    {
        private readonly string _connection = ConfigurationManager.ConnectionStrings["Default"].ToString();

        private readonly string _connectionWrite = ConfigurationManager.ConnectionStrings["Default"].ToString();

        protected DapperDb Db;

        protected DapperDb DbWrite;


        protected BaseRepository()
        {
            Db = DapperDb.CreateDatabase<SqlConnection>(_connection, new SqlServerDialect());
            DbWrite = DapperDb.CreateDatabase<SqlConnection>(_connectionWrite, new SqlServerDialect());
        }

        public void Insert(T t)
        {
            Db.ExtDb.Insert(t);
        }


        public dynamic InsertGetId(T t)
        {
            return Db.ExtDb.Insert(t);
        }

        public void Update(T t)
        {
            Db.ExtDb.Update(t);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="t"></param>
        public void Delete(T t)
        {
            Db.ExtDb.Delete(t);
        }

        public T Get(string id)
        {
            return DbWrite.ExtDb.Get<T>(id);
        }


        public T GetWrite(string id)
        {
            return DbWrite.ExtDb.Get<T>(id);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="t"></param>
        public void LogicDelete(T t)
        {
            dynamic model = t;
            model.IsDelete = 1;
            Update(model);
        }

        public IEnumerable<List<T>> GetAll(string id)
        {
            return DbWrite.ExtDb.GetList<List<T>>(id);
        }
    }

    /// <summary>
    /// 默认的int类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : BaseRepository<T, Guid> where T : DeleteEntity<Guid>
    {

    }

    /// <summary>
    /// 默认的int类型 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepositoryEntity<T> : BaseRepository<T, Guid> where T : BaseEntity<Guid>
    {

    }
}