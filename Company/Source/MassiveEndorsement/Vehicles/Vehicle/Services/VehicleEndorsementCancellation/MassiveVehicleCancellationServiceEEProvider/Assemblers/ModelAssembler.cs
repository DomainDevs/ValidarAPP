using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        internal static List<CompanyCoverage> CreateCompanyCoverages(List<Coverage> coverages)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Coverage, CompanyCoverage>();
                
            });
            return config.CreateMapper().Map<List<Coverage>, List<CompanyCoverage>>(coverages);
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
    }
}
