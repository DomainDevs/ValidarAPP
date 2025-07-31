using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices
{
   [ServiceContract]
    public interface ICompanyCreditNoteBusinessService 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="risk"></param>
        /// <param name="coverage"></param>
        /// <param name="premiumToReturn"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateEndorsementCreditNote(CompanyPolicy companyPolicy,CompanyRisk risk,CompanyCoverage coverage,Decimal? premiumToReturn);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="companyRisks"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateEndorsementCreditNote(CompanyPolicy companyPolicy, List<CompanyRisk> companyRisks);


    }
}