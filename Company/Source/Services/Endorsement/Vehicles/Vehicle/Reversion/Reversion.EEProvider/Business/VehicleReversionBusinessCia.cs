using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleReversionService.EEProvider.DAOs;
using Sistran.Company.Application.VehicleReversionService.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.VehicleReversionService.EEProvider.Business
{
    public class VehicleReversionBusinessCia
    {
        BaseBusinessCia provider;

        public VehicleReversionBusinessCia()
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
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);

                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
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

                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = Errors.ResourceManager.GetString(EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement));
                    companyPolicy.UserId = companyEndorsement.UserId;
                    companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
                    companyPolicy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                    companyPolicy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                    companyPolicy.IssueDate = companyEndorsement.IssueDate;
                    ReversionDAOCia reversionDAOCia = new ReversionDAOCia();
                    var policy = reversionDAOCia.CreateEndorsementReversion(companyPolicy, clearPolicies);
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
