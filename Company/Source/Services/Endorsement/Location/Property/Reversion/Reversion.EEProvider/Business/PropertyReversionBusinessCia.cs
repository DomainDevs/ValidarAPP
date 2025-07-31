using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.PropertyReversionService.EEProvider.DAOs;
using Sistran.Company.Application.PropertyReversionService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPSE = Sistran.Company.Application.ReversionEndorsement.EEProvider;
using CPS = Sistran.Company.Application.Location.PropertyServices;

namespace Sistran.Company.Application.PropertyReversionService.EEProvider.Business
{
    public class PropertyReversionBusinessCia
    {
        BaseBusinessCia provider;

        public PropertyReversionBusinessCia()
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
                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
                }

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
                companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                companyPolicy.IssueDate = Convert.ToDateTime(companyPolicy.IssueDate.ToString("dd-MM-yyyy"));

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
