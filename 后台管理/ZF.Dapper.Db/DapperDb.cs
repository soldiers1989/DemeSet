using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace ZF.Dapper.Db
{
    /// <summary>
    /// Dapper类
    /// 可以用sql的形式访问数据库
    /// </summary>
    public class DapperDb
    {
        private IDatabase _extDb;
        /// <summary>
        /// DapperExtension数据库访问类
        /// </summary>
        public IDatabase ExtDb
        {
            get
            {
                if (_extDb == null)
                {
                    var assembly = new List<Assembly>();
                    assembly.Add(GetType().Assembly);
                    var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), assembly, _sqlDialectBase);
                    var sqlGenerator = new SqlGeneratorImpl(config);
                    var connection = _dbConnectionFunc();
                    connection.ConnectionString = _connectionString;
                    _extDb = new Database(connection, sqlGenerator);
                }
                return _extDb;
            }
        }


        /// <summary>
        /// 连接字符串
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// DapperExtension数据库类型
        /// </summary>
        private readonly SqlDialectBase _sqlDialectBase;

        /// <summary>
        /// 创建连接Func
        /// </summary>
        private readonly Func<IDbConnection> _dbConnectionFunc;

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnectionFunc"></param>
        /// <param name="connectionString"></param>
        /// <param name="sqlDialectBase">DapperExtension数据库类型</param>
        protected DapperDb(Func<IDbConnection> dbConnectionFunc,
            string connectionString, SqlDialectBase sqlDialectBase)
        {
            _connectionString = connectionString;
            _sqlDialectBase = sqlDialectBase;
            _dbConnectionFunc = dbConnectionFunc;

        }
        #endregion

        /// <summary>
        /// 创建具体数据库访问类
        /// </summary>
        /// <typeparam name="TProvider"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sqlDialectBase"></param>
        /// <returns></returns>

        #region Methods
        public static DapperDb CreateDatabase<TProvider>(string connectionString, SqlDialectBase sqlDialectBase)
            where TProvider : IDbConnection
        {
            var db = new DapperDb(() => Activator.CreateInstance<TProvider>(),
                connectionString, sqlDialectBase);
            return db;
        }
        #endregion


        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection(DbTransaction transaction = null)
        {
            if (transaction != null)
                return transaction.Connection;
            var connection = _dbConnectionFunc();
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }



        /// <summary>
        /// 获取Datable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p">Oracle动态参数</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sql)
        {
            DataTable dt = null;
            using (var conn = GetConnection())
            {
                dt = new DataTable();
                dt.Load(conn.ExecuteReader(sql));

            }
            return dt;
        }



        /// <summary>
        /// 查询List接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="p">object动态参数</param>
        /// <returns></returns>
        public List<T> QueryList<T>(string sql, object p = null)
        {
            List<T> list = null;
            using (var conn = GetConnection())
            {
                list = conn.Query<T>(sql, p).ToList();
                return list;
            }
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="p">object动态参数</param>
        /// <returns></returns>
        public  List<T> ExecStoredProcedure<T>(string sql, object p)
        {
            List<T> list = null;
            using (var conn = GetConnection())
            {
                list = conn.Query<T>(sql, p, commandType: CommandType.StoredProcedure).ToList();
                return list;
            }
        }

        /// <summary>
        /// 查询实体接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="p">Oracle动态参数</param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, DynamicParameters p)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, p).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询实体接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="p">Oracle动态参数</param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object p)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, p).FirstOrDefault();
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <param name="id">自增长Id</param>
        /// <returns>是否插入成功</returns>
        public bool Insert(string sql, DynamicParameters p, ref int id)
        {
            using (var conn = GetConnection())
            {
                int i = conn.Execute(sql, p);
                id = p.Get<int>(DapperConstant.PId);
                if (i > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Execute(string sql, DynamicParameters p)
        {
            using (var conn = GetConnection())
            {
                int i = conn.Execute(sql, p);
                if (i > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Execute(string sql, object p)
        {
            using (var conn = GetConnection())
            {
                int i = conn.Execute(sql, p);
                if (i > 0)
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, DynamicParameters p)
        {
            using (var conn = GetConnection())
            {
                return conn.Execute(sql, p);
            }
        }


        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, DynamicParameters p, DbTransaction t)
        {
            var conn = t.Connection;
            return conn.Execute(sql, p);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object p)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(sql, p);
            }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, DynamicParameters p)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(sql, p);
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int InsertMultiple<T>(string sql, IEnumerable<T> entities) where T : class, new()
        {
            using (var conn = GetConnection())
            {
                int records = 0;
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(sql, entities, trans, 30, CommandType.Text);
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    trans.Commit();
                }
                return records;
            }
        }

    }
}
