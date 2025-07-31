using Sistran.Company.Application.Endorsement.CreditNoteBusinessService.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Base.Endorsement.CreditNoteBusinessService;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService
{
    [ServiceContract]
    public interface ICompanyBaseCreditNoteBusinessService : IBaseCreditNoteBusinessService
    {
        [OperationContract]
        /// <summary>
        /// Retorna la lista de endosos que tienen cobro de depósito
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Listado de endosos</returns>
        List<CompanyEndorsement> GetEndorsementsWithPremiumAmount(int policyId);

        /// <summary>
        /// Retorna un listado de riesgos asociados a un endoso
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Listado de riesgos</returns>
        [OperationContract]
        List<CompanyRisk> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId);

        /// <summary>
        /// Cálculo del endoso
        /// </summary>
        /// <param name="creditNote">Endoso de nota crédito</param>
        /// <returns>Resúmen</returns>
        [OperationContract]
        CreditNote Calculate(CreditNote creditNote);

        /// <summary>
        /// Retorna el porcentaje máximo que se va a devolver para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Porcentaje máximo a devolver</returns>
        [OperationContract]
        decimal GetMaximumPremiumPercetToReturn(int policyId);
    }
}