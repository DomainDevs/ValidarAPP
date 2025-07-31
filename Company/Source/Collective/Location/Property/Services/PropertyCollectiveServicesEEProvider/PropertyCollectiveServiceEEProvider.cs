using Sistran.Company.Application.Location.PropertyCollectiveService.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.Location.PropertyCollectiveService.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Company.Application.Location.PropertyCollectiveService.EEProvider
{
    public class PropertyCollectiveServiceEEProvider : IPropertyCollectiveService
    {
        public PropertyCollectiveServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            try
            {
                CollectiveEmissionPropertyDAO CollectiveLoadPropertyDAO = new CollectiveEmissionPropertyDAO();
                return CollectiveLoadPropertyDAO.CreateCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = ex.Message;
                return collectiveEmission;
            }
        }

        public void QuotateCollectiveEmission(List<int> collectiveEmissionIds)
        {
            try
            {
                CollectiveEmissionRowPropertyDAO dao = new CollectiveEmissionRowPropertyDAO();
                dao.QuotateCollectiveEmission(collectiveEmissionIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateMassiveLoad), ex);
            }
        }

        public void QuotateMassiveCollectiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                CollectiveEmissionRowPropertyDAO dao = new CollectiveEmissionRowPropertyDAO();
                dao.QuotateMassiveCollectiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        public string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType)
        {
            try
            {
                CollectiveEmissionPropertyDAO CollectiveLoadPropertyDAO = new CollectiveEmissionPropertyDAO();
                return CollectiveLoadPropertyDAO.GenerateReportToCollectiveLoad(collectiveEmission, endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveEmission)
        {
            try
            {
                return new CollectiveEmissionRowPropertyDAO().IssuanceCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingPropertyPolicies), ex);
            }
        }
    }
}