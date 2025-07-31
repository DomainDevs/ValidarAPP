using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Models;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        internal static IMapper CreateMapTechnicalTransactions()
        {
            var config = MapperCache.GetMapper<TechnicalTransaction, TechnicalTransactionDTO>(cfg =>
            {
                cfg.CreateMap<TechnicalTransaction, TechnicalTransactionDTO>();
            });
            return config;
        }

        internal static TechnicalTransactionDTO ToDTO(this TechnicalTransaction technicalTransaction)
        {
            var config = CreateMapTechnicalTransactions();
            return config.Map<TechnicalTransaction, TechnicalTransactionDTO>(technicalTransaction);
        }

        internal static IMapper CreateMapTechnicalTransactionParameters()
        {
            var config = MapperCache.GetMapper<TechnicalTransactionParameter, TechnicalTransactionParameterDTO>(cfg =>
            {
                cfg.CreateMap<TechnicalTransactionParameter, TechnicalTransactionParameterDTO>();
            });
            return config;
        }

        internal static TechnicalTransactionParameterDTO ToDTO(this TechnicalTransactionParameter technicalTransactionParameter)
        {
            var config = CreateMapTechnicalTransactionParameters();
            return config.Map<TechnicalTransactionParameter, TechnicalTransactionParameterDTO>(technicalTransactionParameter);
        }
    }
}