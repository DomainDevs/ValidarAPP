using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ReversionController : EndorsementBaseController
    {
        public ActionResult Reversion()
        {
            return PartialView();
        }
        /// <summary>
        /// Obtiene un endoso por ramo, sucursal y número de póliza
        /// </summary>
        /// <param name="prefixId"> Id del ramo </param>
        /// <param name="branchId"> Id de la sucursal </param>
        /// <param name="policyNumber"> Número de póliza </param>
        /// <returns> Endoso </returns>
        public ActionResult GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                var imapp = ModelAssembler.CreateMapCompanyEndorsement();
                List<CompanyEndorsement> endorsements = imapp.Map<List<MOS.Endorsement>, List<CompanyEndorsement>>(DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber));
                endorsements = endorsements.Where(x => x.IsCurrent == true).ToList();
                return new UifSelectResult(endorsements);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        /// <summary>
        /// Obtiene la suma de montos de un endoso
        /// </summary>
        /// <param name="endorsementId"> Identificador del endoso </param>
        /// <returns> Endoso  </returns>
        public ActionResult GetSummaryByEndorsementId(int endorsementId)
        {
            try
            {
                Policy policy = DelegateService.underwritingService.GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, false);

                if (policy != null)
                {
                    policy.BusinessTypeDescription = EnumsHelper.GetItemName<BusinessType>(policy.BusinessType);
                    policy.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoInformationEndorsement);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInformationEndorsement);
            }
        }

        /// <summary>
        /// Obtiene los motivos de anluación
        /// </summary>
        /// <returns> Listado con motivos de anulación </returns>
        public ActionResult GetReversionReasons()
        {
            try
            {
                List<EndorsementReason> endorsementReasons = DelegateService.endorsementBaseService.GetEndorsementReasonsByEndorsementType(EndorsementType.LastEndorsementCancellation);
                return new UifJsonResult(true,endorsementReasons.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsementReasons);
            }
        }

        //busqueda para nro de radicado en endoso reversion
        // 
        public ActionResult GetEndorsementWorkFlow(int PolyciId)
        {
            try
            {
                List<string> FilingNumber = new List<string>();
                FilingNumber = DelegateService.ReversionEndorsementService.GetEndorsementWorkFlow(PolyciId);
                return new UifJsonResult(true, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
                //return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPerson);
            }
        }


    }
}