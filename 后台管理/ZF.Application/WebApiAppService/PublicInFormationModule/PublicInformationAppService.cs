using System.Data;
using Dapper;
using ZF.Application.WebApiDto.PublicInformationModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.PublicInFormationModule
{
    /// <summary>
    /// 
    /// </summary>
   public class PublicInformationAppService:BaseAppService<Danye>
    {
        private readonly IDanyeRepository _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public PublicInformationAppService( IDanyeRepository repository ) : base( repository ) {
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PublicInformationModelOutput GetContentInfo( PublicInformationModelInput input) {
            var sql = "SELECT TOP 1 * FROM T_Base_Danye  WHERE Code=@Code order by AddTime desc ";
            var dynamicparameter = new DynamicParameters( );
            dynamicparameter.Add(":Code",input.Code,DbType.String );
            return Db.QueryFirstOrDefault<PublicInformationModelOutput>(sql,dynamicparameter);
        }

    }
}
