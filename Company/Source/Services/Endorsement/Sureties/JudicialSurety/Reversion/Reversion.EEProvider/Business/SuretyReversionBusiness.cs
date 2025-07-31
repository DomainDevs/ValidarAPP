using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyReversionService.EEProvider.DAOs;
using Sistran.Company.Application.JudicialSuretyReversionService.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVSE = Sistran.Company.Application.Sureties.SuretyServices.EEProvider;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.JudicialSuretyReversionService.EEProvider.Business
{
    public class SuretyReversionBusinessCompany
    {
        BaseBusinessCia provider;

        public SuretyReversionBusinessCompany()
        {
            provider = new BaseBusinessCia();
        }
        /// <summary>
        /// Crea un nuevo temporal
        /// </summary>
        /// <param name="reversionViewModel"></param>
        /// <returns> Identificador del temporal creado </returns>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool clearPolicies)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErroEndorsementNotFound);
            }
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                }

                if (companyPolicy != null)
                {
                    companyPolicy.Endorsement = new CompanyEndorsement
                    {
                        Id = companyEndorsement.Id,
                        PolicyId = companyEndorsement.PolicyId,
                        EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                        EndorsementType = EndorsementType.LastEndorsementCancellation,
                        Text = new CompanyText
                        {
                            TextBody = companyEndorsement.Text.TextBody,
                            Observations = companyEndorsement.Text.Observations
                        }
                    };

                    companyPolicy.Id = companyEndorsement.TemporalId;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = Errors.ResourceManager.GetString(EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement));
                    companyPolicy.UserId = companyEndorsement.UserId;
                    companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
                    companyPolicy.TicketNumber = companyEndorsement.TicketNumber;
                    companyPolicy.TicketDate = companyEndorsement.TicketDate;
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3,DateTime.Now);
                    companyPolicy.IssueDate = Convert.ToDateTime(companyPolicy.IssueDate.ToString("dd-MM-yyyy"));
                    companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                    ReversionDAOCompany reversionDAOCompany = new ReversionDAOCompany();
                    CompanyPolicy policy = reversionDAOCompany.CreateEndorsementReversion(companyPolicy, clearPolicies);
                    return policy;
                }
                else
                {
                    throw new Exception(Errors.ErroEndorsementNotFound);
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
