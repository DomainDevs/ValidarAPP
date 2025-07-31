using Sistran.Company.Application.Location.CollectivePropertyRenewalService;
using Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using System;
using Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider
{
    public class CollectivePropertyRenewalServiceEEProvider : ICollectivePropertyRenewalService
    {
        public CollectivePropertyRenewalServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveEmission)
        {

            try
            {
                RenewalDAO dao = new RenewalDAO();
                return dao.CreateCollectiveRenewal(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex,Errors.ErrorInCreateColectiveRenewal));
            }
        }

        public void QuotateCollectiveRenewal(MassiveLoad massiveLoad)
        {
            try
            {
                RenewalQuotateDAO dao = new RenewalQuotateDAO();
                dao.QuotateCollectiveRenewal(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorInQuotateCollectiveRenewal));
            }
        }

        public MassiveLoad IssuanceCollectiveRenewal(MassiveLoad collectiveRenewal)
        {
            try
            {
                return new RenewalQuotateDAO().IssuanceCollectiveRenewal(collectiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
    }
}
