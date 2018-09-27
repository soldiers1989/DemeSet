using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using Dapper;
using DapperExtensions.Sql;
using ZF.Application.BaseDto;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Dapper.Db;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;

namespace ZF.Application
{
    /// <summary>
    /// 领域基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class BaseAppServer<T, V> : IAppServer<T, V> where T : BaseEntity<V> where V : struct
    {
        private readonly string _connection = ConfigurationManager.ConnectionStrings["Default"].ToString();

        private readonly string _connectionWrite = ConfigurationManager.ConnectionStrings["Default"].ToString();

        protected readonly IRepository<T, V> Repository;

        /// <summary>
        /// 数据库Db
        /// </summary>
        protected DapperDb Db;

        protected DapperDb DbWrite;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public BaseAppServer(IRepository<T, V> repository)
        {
            Db = DapperDb.CreateDatabase<SqlConnection>(_connection, new SqlServerDialect());
            DbWrite = DapperDb.CreateDatabase<SqlConnection>(_connectionWrite, new SqlServerDialect());
            Repository = repository;
        }

        public BaseAppServer()
        {
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="t"></param>
        public void Delete(T t)
        {
            Repository.Delete(t);
        }

        /// <summary>
        /// 根据主键Id获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(string id)
        {
            return Repository.Get(id);
        }

        public T GetWrite(string id)
        {
            return Repository.GetWrite(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        public void Insert(T t)
        {
            Repository.Insert(t);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="t"></param>
        public void Update(T t)
        {
            Repository.Update(t);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        public MessagesOutPut LogicDelete(string id)
        {
            var obj = Get(id);
            if (obj == null)
            {
                return new MessagesOutPut() { Success = false, Message = "删除失败,没有找到对应的数据!" };
            }
            Repository.LogicDelete(obj);
            return new MessagesOutPut() { Success = true, Message = "删除成功！" };
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="t"></param>
        public void LogicDelete(T t)
        {
            Repository.LogicDelete(t);
        }

        //public IEnumerable<T> GetList()
        //{
        //    return Repository.GetList();
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseAppService<T> : BaseAppServer<T, Guid> where T : BaseEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public BaseAppService(IRepository<T, Guid> repository) : base(repository)
        {

        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="index">第几页</param>
        /// <param name="rowNumber">查询数量</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public string GetPageSql(string sql, int? index, int? rowNumber, string order)
        {
            var sqlString = @"SELECT TOP  {1}
        *
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY {3} ) AS RowNumber ,
                    *
          FROM      ({0}) AS b
        ) AS A
WHERE   RowNumber > {2}    AND  RowNumber<={4}          ";
            return string.Format(sqlString, sql, rowNumber, (index - 1) * rowNumber, order, index * rowNumber);
        }

        /// <summary>
        /// 获取分页基方法
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="dynamicParameters">参数</param>
        /// <param name="page">当前页</param>
        /// <param name="rows">查询出的记录数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序方式</param>
        /// <returns></returns>
        public string GetPageSql(string sql, DynamicParameters dynamicParameters, int? page, int? rows, string sidx, string sord)
        {
            if (page.HasValue && rows.HasValue)
            {
                return GetPageSql(sql, page, rows, sidx + ' ' + sord);
            }
            return sql;
        }

        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public string GetAddressIp()
        {
            string ip;
            try
            {
                if (HttpContext.Current == null)
                    ip = "";
                string customerIp = "";
                customerIp = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(customerIp))
                {
                    ip = customerIp;
                }
                customerIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(customerIp))
                    ip = customerIp;
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    customerIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (customerIp == null)
                        customerIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    customerIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.Compare(customerIp, "unknown", StringComparison.OrdinalIgnoreCase) == 0)
                    ip = HttpContext.Current.Request.UserHostAddress;
                ip = customerIp;
            }
            catch
            {
                ip = "";
            }
            return ip.Contains(",") ? ip.Split(',')[0] : ip;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIp(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 用户类
        /// </summary>
        protected User UserObject
        {
            get
            {
                //读取cookies
                string cook = "";
                if (HttpContext.Current.Request.Cookies["UserInfo"] != null)
                {
                    var httpCookie = HttpContext.Current.Request.Cookies["UserInfo"]["User"];
                    if (httpCookie != null)
                        cook = httpCookie;
                }
                return (User)JsonHelper.jsonDes<User>(Des3Cryption.Decrypt3DES(cook));
            }
        }
    }
}
