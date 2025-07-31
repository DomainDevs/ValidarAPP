using Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.Location.LiabilityModificationService.EEProvider
{
    public class CollectiveLiabilityModificationlServiceEEProvider : ICollectiveLiabilityModificationlService
    {
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

        public void QuotateCollectiveInclusion(int collectiveEmissionId)
        {
            try
            {
                InclusionQuotateDAO dao = new InclusionQuotateDAO();
                dao.QuotateCollectiveInclusion(collectiveEmissionId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }

        public void QuotateCollectiveExclusion(int collectiveEmissionId)
        {
            try
            {
                ExclusionQuotateDAO dao = new ExclusionQuotateDAO();
                dao.QuotateCollectiveExclusion(collectiveEmissionId);
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
                LiabilityModificationIssueDAO propertyDAO = new LiabilityModificationIssueDAO();
                return propertyDAO.IssuanceCollectiveEmission(collectiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
    }
}
