using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Amortizations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingAmortizationService
    {

        /// <summary>
        /// SaveAmortization: Grabar Amortizacion
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branch"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="amount"></param>
        /// <returns>AmortizationDTO</returns>
        [OperationContract]
        AmortizationDTO GenerateAmortization(int operationType, BranchDTO branch, PrefixDTO prefix, PolicyDTO policy, IndividualDTO insured, decimal amount);

        /// <summary>
        /// ApplyAmortization
        /// </summary>
        /// <param name="amortization"></param>
        /// <returns>AmortizationDTO</returns>
        [OperationContract]
        AmortizationDTO ApplyAmortization(AmortizationDTO amortization);

        /// <summary>
        /// GetAmortization: Obtener Amortizacion
        /// </summary>
        /// <param name="amortizationId"></param>
        /// <returns>AmortizationDTO</returns>
        [OperationContract]
        AmortizationDTO GetAmortizationById(int amortizationId);


        /// <summary>
        /// UpdateAmortization: Actualiza Amortizacion: elimina o aplica items
        /// </summary>
        /// <param name="amortization"></param>
        /// <returns>AmortizationDTO</returns>
        [OperationContract]
        AmortizationDTO UpdateAmortization(AmortizationDTO amortization);

        /// <summary>
        /// GetAmortizations: Obtener todas las Amortizaciones
        /// </summary>        
        /// <returns>List<AmortizationDTO></returns>
        [OperationContract]
        List<AmortizationDTO> GetAmortizations();

    }
}
