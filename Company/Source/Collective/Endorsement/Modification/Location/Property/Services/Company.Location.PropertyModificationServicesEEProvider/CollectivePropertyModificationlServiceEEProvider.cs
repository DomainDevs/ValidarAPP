using Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using System;
using Sistran.Company.Application.Location.PropertyModificationService.EEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider
{
    public class CollectivePropertyModificationlServiceEEProvider : ICollectivePropertyModificationlService
    {
        public CollectivePropertyModificationlServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveModification(CollectiveEmission collectiveEmission)
        {
            try
            {
                if ((SubMassiveProcessType)collectiveEmission.LoadType.Id == SubMassiveProcessType.Inclusion)
                {
                    InclusionDAO dao = new InclusionDAO();
                    return dao.CreateCollectiveInclution(collectiveEmission);
                }
                else if ((SubMassiveProcessType)collectiveEmission.LoadType.Id == SubMassiveProcessType.Exclusion)
                {
                    ExclusionDAO dao = new ExclusionDAO();
                    return dao.CreateCollectiveExclusion(collectiveEmission);
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

        public void QuotateCollectiveInclusion(MassiveLoad massiveLoad)
        {
            try
            {
                InclusionQuotateDAO dao = new InclusionQuotateDAO();
                dao.QuotateCollectiveInclusion(massiveLoad);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }

        public void QuotateCollectiveExclusion(MassiveLoad massiveLoad)
        {
            try
            {
                ExclusionQuotateDAO dao = new ExclusionQuotateDAO();
                dao.QuotateCollectiveExclusion(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }

        }

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad)
        {
            try
            {
                PropertyModificationIssueDAO propertyDAO = new PropertyModificationIssueDAO();
                return propertyDAO.IssuanceCollectiveEmission(collectiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
    }
}
