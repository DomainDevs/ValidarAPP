using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Location.PropertyServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region RisksubActivities
        public static IMapper CreateMapRisksubActivities()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.RiskLocation, PropertyRisk>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskLocation, PropertyRisk>()
                .ForMember(d => d.Risk, o => o.MapFrom(c => new Risk
                {
                    RiskId = c.RiskId
                }))
                .ForMember(d => d.City, o => o.MapFrom(c => new City
                {
                    Id = Convert.ToInt32(c.CityCode),
                    State = new State
                    {
                        Id = Convert.ToInt32(c.StateCode),
                        Country = new Country
                        {
                            Id = Convert.ToInt32(c.CountryCode)
                        }
                    }
                }));
            });
            return config;
        }
        #endregion RisksubActivities
    }
}
