using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.Location.PropertyServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        public static PropertyRisk CreateRiskLocation(ISSEN.RiskLocation entityRiskLocation)
        {
            var mapper = AutoMapperAssembler.CreateMapRisksubActivities();
            return mapper.Map<ISSEN.RiskLocation, PropertyRisk>(entityRiskLocation);

            //return new PropertyRisk
            //{
            //    Risk = new Risk
            //    {
            //        RiskId = entityRiskLocation.RiskId
            //    },
            //    City = new City
            //    {
            //        Id = Convert.ToInt32(entityRiskLocation.CityCode),
            //        State = new State
            //        {
            //            Id = Convert.ToInt32(entityRiskLocation.StateCode),
            //            Country = new Country
            //            {
            //                Id = Convert.ToInt32(entityRiskLocation.CountryCode)
            //            }
            //        }
            //    },
            //    Street = entityRiskLocation.Street
            //};
        }

        public static List<PropertyRisk> CreateRiskLocations(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRisksubActivities();
            return mapper.Map<List<ISSEN.RiskLocation>, List<PropertyRisk>>(businessCollection.Cast<ISSEN.RiskLocation>().ToList());

            //List<PropertyRisk> propertyRisks = new List<PropertyRisk>();

            //foreach (ISSEN.RiskLocation entityRiskLocation in businessCollection)
            //{
            //    propertyRisks.Add(CreateRiskLocation(entityRiskLocation));
            //}

            //return propertyRisks;
        }
    }
}
