using Company.Location.LiabilityCollectiveService.EEProvider.DAOs;
using Company.Location.LiabilityCollectiveService.EEProvider.Resources;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.Location.LiabilityCollectiveService.EEProvider
{
    public class LiabilityCollectiveServiceEEProvider : ILiabilityCollectiveService
    {
        public CollectiveEmission CreateCollectiveLoad(CollectiveEmission collectiveLoad)
        {
            try
            {
                CollectiveLoadLiabilityDAO liabilityCollectiveDAO = new CollectiveLoadLiabilityDAO();
                return liabilityCollectiveDAO.CreateCollectiveLoad(collectiveLoad);
            }
            catch (Exception ex)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = ex.Message;
                return collectiveLoad;
            }
        }

        public void QuotateCollectiveLoad(List<int> collectiveEmissionIds)
        {
            try
            {
                CollectiveLoadProcessLiabilityDAO dao = new CollectiveLoadProcessLiabilityDAO();
                dao.QuotateCollectiveEmission(collectiveEmissionIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString(), ex);
            }
        }

        public void QuotateMassiveCollectiveEmission(int collectiveEmissionId)
        {
            try
            {
                CollectiveLoadProcessLiabilityDAO dao = new CollectiveLoadProcessLiabilityDAO();
                dao.QuotateMassiveCollectiveEmission(collectiveEmissionId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveEmission)
        {
            try
            {
                return new CollectiveLoadProcessLiabilityDAO().IssuanceCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingVehiclePolicies), ex);
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
                CollectiveLoadLiabilityDAO collectiveLoadLiabilityDAO = new CollectiveLoadLiabilityDAO();
                return collectiveLoadLiabilityDAO.GenerateReportToCollectiveLoad(collectiveEmission, endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString(), ex);
            }

        }
    }
}