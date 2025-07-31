using Sistran.Core.Integration.CommonServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.CommonServices
{
    [ServiceContract]
    public interface ICommonIntegrationServiceCore
    {
        [OperationContract]
        List<LineBusinessDTO> GetLinesBusiness();

        [OperationContract]
        List<SubLineBusinessDTO> GetSubLineBusinessByLineBusinessId();

        [OperationContract]
        ParameterDTO GetParameterByParameterId(int parameterId);

        [OperationContract]
        DateTime GetModuleDateIssue(int moduleCode, DateTime issueDate);

        [OperationContract]
        ParameterDTO UpdateParameter(ParameterDTO parameter);

        [OperationContract]
        List<CurrencyDTO> GetCurrencies();


        /// <summary>
        /// Obtener Sucursales
        /// </summary>
        /// <returns>Obtener Lista de Branchs</returns>
        [OperationContract]
        List<BranchDTO> GetBranches();

        /// <summary>
        /// Obtener Ramos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PrefixDTO> GetPrefixes();

        /// <summary>
        /// Obtener sucursal por id
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        BranchDTO GetBranchById(int id);

        /// <summary>
        /// Obtener un ramo por id
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        PrefixDTO GetPrefixById(int id);

        [OperationContract]
        ExchangeRateDTO GetExchangeRateByCurrencyId(int currencyId);

        [OperationContract]
        List<ExchangeRateDTO> GetExchangeRates(DateTime? dateCumulus = null, int? CurrecyCode = null);
        [OperationContract]
        List<LineBusinessDTO> GetLinesBusinessByPrefixId(int prefixId);
    }
}
