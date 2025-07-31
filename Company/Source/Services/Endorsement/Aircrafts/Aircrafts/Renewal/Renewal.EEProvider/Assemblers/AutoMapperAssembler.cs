using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config.CreateMapper();
        }
        #endregion Clausula
    }
}
