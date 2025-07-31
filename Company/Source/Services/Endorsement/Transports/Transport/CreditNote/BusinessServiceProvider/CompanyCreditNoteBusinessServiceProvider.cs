using System.ServiceModel;
using System;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Resources;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices;
using System.Collections.Generic;
using Sistran.Core.Application.Transports.CreditNote.BusinessService.EEProvider;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyCreditNoteBusinessServiceProvider : CompanyBaseCreditNoteBusinessServiceProvider, ICompanyCreditNoteBusinessService 
    {
        public CompanyPolicy CreateEndorsementCreditNote(CompanyPolicy companyPolicy, CompanyRisk risk, CompanyCoverage coverage, decimal? premiumToReturn)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                return businessCreditNote.CreateEndorsementCreditNote(companyPolicy,risk,coverage,premiumToReturn);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCreditNoteEndorsement), ex);
            }
        }

        public CompanyPolicy CreateEndorsementCreditNote(CompanyPolicy companyPolicy, List<CompanyRisk> companyRisks)
        {
            try
            {
                BusinessCreditNote businessCreditNote = new BusinessCreditNote();
                //return businessCreditNote.CreateEndorsementCreditNote(companyPolicy, companyRisks);
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCreditNoteEndorsement), ex);
            }
        }
    }
}
