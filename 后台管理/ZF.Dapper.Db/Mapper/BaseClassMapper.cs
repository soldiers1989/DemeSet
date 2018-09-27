using System;
using DapperExtensions.Mapper;
using ZF.Core;

namespace ZF.Dapper.Db.Mapper
{
    /// <summary>
    /// 映射基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT">主键类型</typeparam>
    public class BaseClassMapper<T,KT>:ClassMapper<T> where T :BaseEntity<KT> where KT : struct
    {
        public BaseClassMapper()
        {
            Map(x => x.Id).Key(KeyType.Assigned).Column("Id");
        }
    }

    /// <summary>
    /// 映射基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT">主键类型</typeparam>
    public class DeleteEntityClassMapper<T, KT> : BaseClassMapper<T, KT> where T : DeleteEntity<KT> where KT : struct
    {
        public DeleteEntityClassMapper()
        {
            Map(x => x.IsDelete).Column("IsDelete");
        }
    }

    /// <summary>
    /// 映射基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT">主键类型</typeparam>
    public class AuditEntityClassMapper<T, KT> : BaseClassMapper<T,KT> where T : AuditEntity<KT> where KT : struct
    {
        public AuditEntityClassMapper()
        {
            Map(x => x.AddTime).Column("AddTime");
            Map(x => x.AddUserId).Column("AddUserId");
            Map(x => x.UpdateTime).Column("UpdateTime");
            Map(x => x.UpdateUserId).Column("UpdateUserId");
        }
    }


    /// <summary>
    /// 映射基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT">主键类型</typeparam>
    public class FullAuditEntityClassMapper<T, KT> : AuditEntityClassMapper<T, KT> where T : FullAuditEntity<KT> where KT : struct
    {
        public FullAuditEntityClassMapper()
        {
            Map(x => x.IsDelete).Column("IsDelete");
        }
    }


    /// <summary>
    /// 默认int类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseClassMapper<T> : BaseClassMapper<T,Guid> where T: BaseEntity<Guid>
    {
        
    }
    
}
