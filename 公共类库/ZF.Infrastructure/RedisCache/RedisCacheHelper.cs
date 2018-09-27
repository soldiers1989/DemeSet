using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using ServiceStack.Redis;
using StackExchange.Redis;
using System.Web.Script.Serialization;

namespace ZF.Infrastructure.RedisCache
{
    //public class RedisCacheHelper
    //{
    //    private static readonly PooledRedisClientManager pool = null;
    //    private static readonly string[] redisHosts = null;
    //    public static int RedisMaxReadPool = int.Parse(ConfigurationManager.AppSettings["redis_max_read_pool"]);
    //    public static int RedisMaxWritePool = int.Parse(ConfigurationManager.AppSettings["redis_max_write_pool"]);

    //    public static int DefultCaCheTime = int.Parse(ConfigurationManager.AppSettings["DefultCaCheTime"]);

    //    public static int IsEnable = int.Parse(ConfigurationManager.AppSettings["IsEnable"]);

    //    /// <summary>
    //    /// 实例化RedisCache
    //    /// </summary>
    //    static RedisCacheHelper()
    //    {
    //        var redisHostStr = ConfigurationManager.AppSettings["redis_server_session"];

    //        if (!string.IsNullOrEmpty(redisHostStr))
    //        {
    //            redisHosts = redisHostStr.Split(',');

    //            if (redisHosts.Length > 0)
    //            {
    //                if (IsEnable == 0)
    //                {
    //                    pool = null;
    //                }
    //                else
    //                {
    //                    pool = new PooledRedisClientManager(redisHosts, redisHosts,
    //                        new RedisClientManagerConfig()
    //                        {
    //                            MaxWritePoolSize = RedisMaxWritePool,
    //                            MaxReadPoolSize = RedisMaxReadPool,
    //                            AutoStart = true
    //                        });
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 添加缓存 指定日期 默认时间
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    public static void Add<T>(string key, T value)
    //    {
    //        if (value == null)
    //        {
    //            return;
    //        }
    //        try
    //        {
    //            if (pool != null)
    //            {
    //                using (var r = pool.GetClient())
    //                {
    //                    if (r != null)
    //                    {
    //                        r.SendTimeout = 1000;
    //                        r.Set(key, value, DateTime.Now.AddHours(DefultCaCheTime) - DateTime.Now);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
    //        }

    //    }

    //    /// <summary>
    //    /// 获取 指定缓存
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public static T Get<T>(string key)
    //    {
    //        if (string.IsNullOrEmpty(key))
    //        {
    //            return default(T);
    //        }
    //        T obj = default(T);
    //        try
    //        {
    //            if (pool != null)
    //            {
    //                using (var r = pool.GetClient())
    //                {
    //                    if (r != null)
    //                    {
    //                        r.SendTimeout = 1000;
    //                        obj = r.Get<T>(key);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
    //        }
    //        return obj;
    //    }

    //    /// <summary>
    //    /// 删除指定缓存
    //    /// </summary>
    //    /// <param name="key"></param>
    //    public static void Remove(string key)
    //    {
    //        try
    //        {
    //            if (pool != null)
    //            {
    //                using (var r = pool.GetClient())
    //                {
    //                    if (r != null)
    //                    {
    //                        r.SendTimeout = 1000;
    //                        r.Remove(key);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
    //        }
    //    }

    //    /// <summary>
    //    /// 检查缓存是否唯一
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public static bool Exists(string key)
    //    {
    //        try
    //        {
    //            if (pool != null)
    //            {
    //                using (var r = pool.GetClient())
    //                {
    //                    if (r != null)
    //                    {
    //                        r.SendTimeout = 1000;
    //                        return r.ContainsKey(key);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
    //        }
    //        return false;
    //    }
    //}

    /// <summary>
    /// Redis 助手
    /// </summary>
    public class RedisCacheHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string ConnectionString;

        /// <summary>
        /// redis 连接对象
        /// </summary>
        private static IConnectionMultiplexer _connMultiplexer;

        /// <summary>
        /// 默认的 Key 值（用来当作 RedisKey 的前缀）
        /// </summary>
        private static readonly string DefaultKey;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// 数据库
        /// </summary>
        private static IDatabase _db;

        /// <summary>
        /// 获取 Redis 连接对象
        /// </summary>
        /// <returns></returns>
        public IConnectionMultiplexer GetConnectionRedisMultiplexer()
        {
            if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
            {
                lock (Locker)
                {
                    if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
                        _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
                }
            }

            return _connMultiplexer;
        }

        #region 其它

        public ITransaction GetTransaction()
        {
            return _db.CreateTransaction();
        }

        #endregion 其它

        #region 构造函数

        static RedisCacheHelper()
        {
            try
            {
                ConnectionString = ConfigurationManager.AppSettings["redis_server_session"];
                _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
                DefaultKey = ConfigurationManager.AppSettings["Redis.DefaultKey"];
                _db = _connMultiplexer.GetDatabase(-1);
                AddRegisterEvent();

            }
            catch (Exception)
            {
            }
        }



        #endregion 构造函数

        #region String 操作


        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry">默认缓存时间为24个小时</param>
        /// <returns></returns>
        public static bool Add<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            try
            {
                if (expiry == null)
                {
                    expiry = new TimeSpan(24, 0, 0);
                }
                redisKey = AddKeyPrefix(redisKey);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var json = "";
                if (redisValue != null)
                {
                    json = jss.Serialize(redisValue);
                }
                return _db.StringSet(redisKey, json, expiry);

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T Get<T>(string redisKey)
        {
            try
            {
                redisKey = AddKeyPrefix(redisKey);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                if (_db.StringGet(redisKey) != "")
                {
                    return jss.Deserialize<T>(_db.StringGet(redisKey));
                }
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 判断值是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool Exists(string redisKey)
        {
            try
            {
                redisKey = AddKeyPrefix(redisKey);
                return _db.KeyExists(redisKey);
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// 删除redisKey
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool Remove(string redisKey)
        {
            try
            {
                redisKey = AddKeyPrefix(redisKey);
                return _db.KeyDelete(redisKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion String 操作



        #region private method

        /// <summary>
        /// 添加 Key 的前缀
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AddKeyPrefix(string key)
        {
            return $"{DefaultKey}:{key}";
        }

        #region 注册事件

        /// <summary>
        /// 添加注册事件
        /// </summary>
        private static void AddRegisterEvent()
        {
            _connMultiplexer.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            _connMultiplexer.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            _connMultiplexer.ErrorMessage += ConnMultiplexer_ErrorMessage;
            _connMultiplexer.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            _connMultiplexer.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            _connMultiplexer.InternalError += ConnMultiplexer_InternalError;
            _connMultiplexer.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }

        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChangedBroadcast)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_InternalError)}: {e.Exception}");
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine(
                $"{nameof(ConnMultiplexer_HashSlotMoved)}: {nameof(e.OldEndPoint)}-{e.OldEndPoint} To {nameof(e.NewEndPoint)}-{e.NewEndPoint}, ");
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChanged)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ErrorMessage)}: {e.Message}");
        }

        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionFailed)}: {e.Exception}");
        }

        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionRestored)}: {e.Exception}");
        }

        #endregion 注册事件

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var data = memoryStream.ToArray();
                return data;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        #endregion private method
    }

}