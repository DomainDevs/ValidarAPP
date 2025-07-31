using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Models;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Assemblers
{
    internal static class ModelAssembler
    {
        internal static IMapper CreateMapTechnicalTransactions()
        {
            var config = MapperCache.GetMapper<TechnicalTransactionDTO, TechnicalTransaction>(cfg =>
            {
                cfg.CreateMap<TechnicalTransactionDTO, TechnicalTransaction>();
            });
            return config;
        }

        internal static TechnicalTransaction ToModel(this TechnicalTransactionDTO technicalTransactionDTO)
        {
            var config = CreateMapTechnicalTransactions();
            return config.Map<TechnicalTransactionDTO, TechnicalTransaction>(technicalTransactionDTO);
        }

        internal static IMapper CreateMapTechnicalTransactionParameters()
        {
            var config = MapperCache.GetMapper<TechnicalTransactionParameterDTO, TechnicalTransactionParameter>(cfg =>
            {
                cfg.CreateMap<TechnicalTransactionParameterDTO, TechnicalTransactionParameter>();
            });
            return config;
        }

        internal static TechnicalTransactionParameter ToModel(this TechnicalTransactionParameterDTO technicalTransactionParameterDTO)
        {
            var config = CreateMapTechnicalTransactionParameters();
            return config.Map<TechnicalTransactionParameterDTO, TechnicalTransactionParameter>(technicalTransactionParameterDTO);
        }

        //internal static ConceptB CreateConceptB(ConceptBDTO conceptBDTO)
    }
}