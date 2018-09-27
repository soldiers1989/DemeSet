﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using Dapper;

namespace ZF.Dapper.Db.DapperSql
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlDynamicParameters : SqlMapper.IDynamicParameters
    {
        private static Dictionary<SqlMapper.Identity, Action<IDbCommand, object>> paramReaderCache = new Dictionary<SqlMapper.Identity, Action<IDbCommand, object>>();

        private Dictionary<string, ParamInfo> parameters = new Dictionary<string, ParamInfo>();
        private List<object> templates;

        private class ParamInfo
        {

            public string Name { get; set; }

            public object Value { get; set; }

            public ParameterDirection ParameterDirection { get; set; }

            public SqlDbType? DbType { get; set; }

            public int? Size { get; set; }

            public IDbDataParameter AttachedParam { get; set; }
        }

        /// <summary>
        /// construct a dynamic parameter bag
        /// </summary>
        public SqlDynamicParameters()
        {
        }

        /// <summary>
        /// construct a dynamic parameter bag
        /// </summary>
        /// <param name="template">can be an anonymous type or a DynamicParameters bag</param>
        public SqlDynamicParameters(object template)
        {
            AddDynamicParams(template);
        }

        /// <summary>
        /// Append a whole object full of params to the dynamic
        /// EG: AddDynamicParams(new {A = 1, B = 2}) // will add property A and B to the dynamic
        /// </summary>
        /// <param name="param"></param>
        public void AddDynamicParams(
#if CSHARP30
            object param
#else
dynamic param
#endif
)
        {
            var obj = param as object;
            if (obj != null)
            {
                var subDynamic = obj as SqlDynamicParameters;
                if (subDynamic == null)
                {
                    var dictionary = obj as IEnumerable<KeyValuePair<string, object>>;
                    if (dictionary == null)
                    {
                        templates = templates ?? new List<object>();
                        templates.Add(obj);
                    }
                    else
                    {
                        foreach (var kvp in dictionary)
                        {
#if CSHARP30
                            Add(kvp.Key, kvp.Value, null, null, null);
#else
                            Add(kvp.Key, kvp.Value);
#endif
                        }
                    }
                }
                else
                {
                    if (subDynamic.parameters != null)
                    {
                        foreach (var kvp in subDynamic.parameters)
                        {
                            parameters.Add(kvp.Key, kvp.Value);
                        }
                    }

                    if (subDynamic.templates != null)
                    {
                        templates = templates ?? new List<object>();
                        foreach (var t in subDynamic.templates)
                        {
                            templates.Add(t);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add a parameter to this dynamic parameter list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        public void Add(
#if CSHARP30
            string name, object value, DbType? dbType, ParameterDirection? direction, int? size
#else
string name, object value = null, SqlDbType? dbType = null, ParameterDirection? direction = null, int? size = null
#endif
)
        {
            parameters[Clean(name)] = new ParamInfo() { Name = name, Value = value, ParameterDirection = direction ?? ParameterDirection.Input, DbType = dbType, Size = size };
        }

        private static string Clean(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                switch (name[0])
                {
                    case '@':
                    case ':':
                    case '?':
                        return name.Substring(1);
                }
            }
            return name;
        }

        void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddParameters(command, identity);
        }

        /// <summary>
        /// Add all the parameters needed to the command just before it executes
        /// </summary>
        /// <param name="command">The raw command prior to execution</param>
        /// <param name="identity">Information about the query</param>
        protected void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    var newIdent = identity.ForDynamicParameters(template.GetType());
                    Action<IDbCommand, object> appender;

                    lock (paramReaderCache)
                    {
                        if (!paramReaderCache.TryGetValue(newIdent, out appender))
                        {
                            appender = SqlMapper.CreateParamInfoGenerator(newIdent, false, false);
                            paramReaderCache[newIdent] = appender;
                        }
                    }

                    appender(command, template);
                }
            }

            foreach (var param in parameters.Values)
            {
                string name = Clean(param.Name);
                bool add = !((SqlCommand)command).Parameters.Contains(name);
                SqlParameter p;
                if (add)
                {
                    p = ((SqlCommand)command).CreateParameter();
                    p.ParameterName = name;
                }
                else
                {
                    p = ((SqlCommand)command).Parameters[name];
                }
                var val = param.Value;
                p.Value = val ?? DBNull.Value;
                p.Direction = param.ParameterDirection;
                var s = val as string;
                if (s != null)
                {
                    if (s.Length <= 4000)
                    {
                        p.Size = 4000;
                    }
                }
                if (param.Size != null)
                {
                    p.Size = param.Size.Value;
                }
                if (param.DbType != null)
                {
                    p.SqlDbType = param.DbType.Value;
                }
                if (add)
                {
                    command.Parameters.Add(p);
                }
                param.AttachedParam = p;
            }
        }

        /// <summary>
        /// All the names of the param in the bag, use Get to yank them out
        /// </summary>
        public IEnumerable<string> ParameterNames
        {
            get
            {
                return parameters.Select(p => p.Key);
            }
        }

        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>The value, note DBNull.Value is not returned, instead the value is returned as null</returns>
        public T Get<T>(string name)
        {
            var val = parameters[Clean(name)].AttachedParam.Value;
            if (val == DBNull.Value)
            {
                if (default(T) != null)
                {
                    throw new ApplicationException("Attempting to cast a DBNull to a non nullable type!");
                }
                return default(T);
            }
            return (T)val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Sql数据类型</typeparam>
        /// <typeparam name="Y">Net数据类型</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public Y Get<T, Y>(string name) 
        {
            var val = parameters[Clean(name)].AttachedParam.Value;
            if (val == DBNull.Value)
            {
                if (default(Y) != null)
                {
                    throw new ApplicationException("Attempting to cast a DBNull to a non nullable type!");
                }
                return default(Y);
            }
            dynamic dayVal = (T)val;
            return (Y)dayVal;
        }


        /// <summary>
        /// 从Sql数据类型转成 T 类型（T ，int decimal ....)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public  T ChangeByDecimal<T>(string name) where T : struct
        {
            return Get<SqlDecimal, T>(name);
        }


        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public  int GetRecordCount()
        {
            return ChangeByDecimal<int>(DapperConstant.RecordCount);
        }
    }
}
