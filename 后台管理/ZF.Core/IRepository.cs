using System;
using System.Collections.Generic;

namespace ZF.Core
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface IRepository<T, V> where T : BaseEntity<V> where V : struct
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        void Insert(T t);


        /// <summary>
        /// 新增返回Id
        /// </summary>
        /// <param name="t"></param>
        dynamic InsertGetId(T t);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t"></param>
        void Update(T t);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t"></param>
        void Delete(T t);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(string id);


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetWrite(string id);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="t"></param>
        void LogicDelete(T t);

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<List<T>> GetAll(string id);
    }

  
}
