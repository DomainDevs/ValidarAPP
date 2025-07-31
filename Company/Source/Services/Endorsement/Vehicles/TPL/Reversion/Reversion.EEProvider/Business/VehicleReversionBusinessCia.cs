using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.DAOs;
using Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.Business
{
    public class ThirdPartyLiabilityReversionBusinessCia
    {
        BaseBusinessCia provider;

        public ThirdPartyLiabilityReversionBusinessCia()
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

            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);

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
                companyPolicy.UserId = BusinessContext.Current.UserId;
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
                companyPolicy.TicketNumber = companyEndorsement.TicketNumber;
                companyPolicy.TicketDate = companyEndorsement.TicketDate;
                companyPolicy.IssueDate = DelegateService.commonService.GetDate();

                ReversionDAOCia reversionDAOCia = new ReversionDAOCia();
                return reversionDAOCia.CreateEndorsementReversion(companyPolicy, clearPolicies);
            }
            else
            {
                throw new Exception(Errors.ErroEndorsementNotFound);
            }
        }
    }
}
