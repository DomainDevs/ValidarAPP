using AutoMapper;
using Sistran.Company.Application.MassiveTPLCancellationServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.MassiveTPLCancellationService.EEProvider.Assemblers
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
