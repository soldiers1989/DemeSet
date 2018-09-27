namespace ZF.WebSite.Models
{
    public class ModelJob<T>
    {

        public T Result { get; set; }

        public bool Success { get; set; }
    }

    public class ModelClass
    {
        /// <summary>
        /// 标题
        /// </summary>     
        public string Title { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>     
        public string KeyWord { get; set; }

        /// <summary>
        /// 描述
        /// </summary>     
        public string Description { get; set; }
    }

}