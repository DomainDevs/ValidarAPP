using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskComboDTO
    {
        public List<CompanyRisk> CompanyRisks { get; set; }

        public List<GroupCoverage> GroupCoverages { get; set; }

        public List<Make> Makes { get; set; }

        public List<Model> Models { get; set; }

        public List<Application.Vehicles.Models.Version> Versions { get; set; }

        public List<Application.Vehicles.Models.Type> Types { get; set; }

        public List<Year> Years { get; set; }

        public List<Use> Uses { get; set; }

        public List<Color> Colors { get; set; }

        public List<CompanyServiceType> CompanyServiceTypes { get; set; }

        public List<RatingZone> RatingZones { get; set; }

        public List<LimitRc> LimitRcs { get; set; }

        public List<Fuel> Fuels { get; set; }
        

    }
}