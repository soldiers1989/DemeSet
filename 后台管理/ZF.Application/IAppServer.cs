using ZF.Application.BaseDto;
using ZF.Core;

namespace ZF.Application
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface IAppServer<T,TV>  where T: BaseEntity<TV> where TV : struct
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        void Insert(T t);

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
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        MessagesOutPut LogicDelete(string id);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="t"></param>
        void LogicDelete(T t);

       //IEnumerable<T> GetList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppServer<T> :IAppServer<T,int> where T : BaseEntity<int> 
    {
        
    }
}