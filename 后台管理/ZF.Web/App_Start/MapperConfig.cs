using System;
using System.Linq;
using System.Reflection;
using log4net;
using ZF.AutoMapper.AutoMapper;

namespace ZF.Web
{
    public class CreateMapper
    {
        /// <summary>
        /// 日志帮助类
        /// </summary>
        private static ILog _logger;
        private static readonly object LockObj = new object();
        protected static ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    lock (LockObj)
                    {
                        if (_logger == null)
                        {
                            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); ;
                        }
                    }
                }
                return _logger;
            }
        }


        public static void  Init()
        {
            Assembly assembly = Assembly.Load("ZF.Application");
            var types = assembly.GetTypes().Where(type =>
             type.IsDefined(typeof(AutoMapAttribute)));

            var enumerable = types as Type[] ?? types.ToArray();
            Logger.DebugFormat("Found {0} classes defines auto mapping attributes", enumerable.Length);
            foreach (var type in enumerable)
            {
                Logger.Debug(type.FullName);
                AutoMapperHelper.CreateMap(type);
            }
        }

        
    }
}
