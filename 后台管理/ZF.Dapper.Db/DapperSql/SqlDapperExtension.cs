﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace ZF.Dapper.Db.DapperSql
{
    public static class SqlDapperExtension
    {
        /// <summary>SqlDapperExtension
        /// 获取Datable（只限制Sql数据库用）
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="p">Oracle动态参数</param>
        /// <returns></returns>
        public static DataTable ExecuteTable(this DapperDb db, string sql, SqlDynamicParameters p)
        {
            DataTable dt = null;
            using (var conn = db.GetConnection())
            {
                dt = new DataTable();
                dt.Load(conn.ExecuteReader(sql, p));
            }
            return dt;
        }

        /// <summary>
        /// 获取Datable
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="p">Oracle动态参数</param>
        /// <param name="isCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable ExecuteTable(this DapperDb db, string sql, SqlDynamicParameters p, bool isCount, ref int recordCount)
        {
            DataTable dt = null;
            using (var conn = db.GetConnection())
            {
                dt = new DataTable();
                dt.Load(conn.ExecuteReader(sql, p));
                if (isCount)
                {
                    recordCount = p.GetRecordCount();
                }
            }
            return dt;
        }

        /// <summary>
        /// 查询List接口（只限制Oracle数据库用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="p">Oracle动态参数</param>
        /// <param name="isCount">是否返回总数</param>
        /// <param name="recordCount">总数</param>
        /// <returns></returns>
        public static List<T> QueryList<T>(this DapperDb db, string sql, SqlDynamicParameters p, bool isCount, ref int recordCount)
        {
            List<T> list = null;
            using (var conn = db.GetConnection())
            {
                list = conn.Query<T>(sql, p).ToList();
                if (isCount)
                {
                    recordCount = p.GetRecordCount();
                }
                return list;
            }
        }
    
    }
}
