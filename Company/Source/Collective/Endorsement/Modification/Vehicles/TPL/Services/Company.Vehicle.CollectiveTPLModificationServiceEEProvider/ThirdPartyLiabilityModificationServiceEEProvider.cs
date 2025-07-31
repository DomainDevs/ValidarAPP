using Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationService;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider
{
    public class ThirdPartyLiabilityModificationServiceEEProvider : IThirdPartyLiabilityModificationService
    {
        public ThirdPartyLiabilityModificationServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveModification(CollectiveEmission collectiveEmission)
        {
            try
            {
                if ((SubMassiveProcessType)collectiveEmission.LoadType.Id == SubMassiveProcessType.Inclusion)
                {
                    ThirdPartyLiabilityInclutionDAO modificationInclutionTplDAO = new ThirdPartyLiabilityInclutionDAO();
                    return modificationInclutionTplDAO.CreateVehicleInclution(collectiveEmission);
                }
                else if ((SubMassiveProcessType)collectiveEmission.LoadType.Id == SubMassiveProcessType.Exclusion)
                {
                    ThirdPartyLiabilityExclutionDAO modificationExclutionTplDAO = new ThirdPartyLiabilityExclutionDAO();
                    return modificationExclutionTplDAO.CreateVehicleExclution(collectiveEmission);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = ex.Message;
                return collectiveEmission;
            }
        }

        public void QuotateCollectiveIncluition(MassiveLoad massiveLoad)
        {
            try
            {
                ThirdPartyLiabilityInclutionQuotateDAO collectiveProcessDAO = new ThirdPartyLiabilityInclutionQuotateDAO();
                collectiveProcessDAO.QuotateCollectiveIncluition(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString(), ex);
            }
        }

        public void QuotateCollectiveExclusion(MassiveLoad massiveLoad)
        {
            try
            {
                ThirdPartyLiabilityExclutionQuotateDAO dao = new ThirdPartyLiabilityExclutionQuotateDAO();
                dao.QuotateCollectiveExclusion(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString(), ex);
            }
        }

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveRenewal)
        {
            try
            {
                return new ThirdPartyLiabilityModificationIssueDAO().IssuanceCollectiveEmission(collectiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
    }
}