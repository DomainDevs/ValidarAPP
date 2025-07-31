using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renewal.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Clausulas
        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config;
        }
        #endregion Clausula
    }
}
