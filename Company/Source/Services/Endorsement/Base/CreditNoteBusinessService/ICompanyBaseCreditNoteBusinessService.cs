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
        /// Retorna la lista de endosos que tienen cobro de dep�sito
        /// </summary>
        /// <param name="policyId">Identificador de la p�liza</param>
        /// <returns>Listado de endosos</returns>
        List<CompanyEndorsement> GetEndorsementsWithPremiumAmount(int policyId);

        /// <summary>
        /// Retorna un listado de riesgos asociados a un endoso
        /// </summary>
        /// <param name="policyId">Identificador de la p�liza</param>
        /// <param name="endorsementId">Identificador del endoso</param>
        /// <returns>Listado de riesgos</returns>
        [OperationContract]
        List<CompanyRisk> GetRisksByPolicyIdEndorsementId(int policyId, int endorsementId);

        /// <summary>
        /// C�lculo del endoso
        /// </summary>
        /// <param name="creditNote">Endoso de nota cr�dito</param>
        /// <returns>Res�men</returns>
        [OperationContract]
        CreditNote Calculate(CreditNote creditNote);

        /// <summary>
        /// Retorna el porcentaje m�ximo que se va a devolver para una p�liza
        /// </summary>
        /// <param name="policyId">Identificador de la p�liza</param>
        /// <returns>Porcentaje m�ximo a devolver</returns>
        [OperationContract]
        decimal GetMaximumPremiumPercetToReturn(int policyId);
    }
}