using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Automapper

        #region Branch
        public static IMapper CreateMapBranch()
        {
            var config = MapperCache.GetMapper<CompanyBranch, Branch>(cfg =>
            {
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanySalesPoint, SalePoint>();
            });
            return config;
        }
        #endregion
        #endregion
    }
}
