using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.TranportReversionService.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.AircraftReversionService.EEProvider.Resources;

namespace Sistran.Company.Application.TranportReversionService.EEProvider.Business
{
    public class TranportReversionBusinessCia
    {
        BaseBusinessCia provider;

        public TranportReversionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }
        /// <summary>
        /// Crea un nuevo temporal
        /// </summary>
        /// <param name="reversionViewModel"></param>
        /// <returns> Identificador del temporal creado </returns>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErroEndorsementNotFound);
            }
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                if (companyPolicy != null)
                {
                    companyPolicy.Endorsement = new CompanyEndorsement
                    {
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
                    companyPolicy.UserId = BusinessContext.Current.UserId;
                    companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);



                    ReversionDAOCia reversionDAOCia = new ReversionDAOCia();
                    var policy = reversionDAOCia.CreateEndorsementReversion(companyPolicy);
                    return policy;
                }
                else
                {
                    throw new Exception(Errors.ErroEndorsementNotFound);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
