using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Company.Application.Vehicle.ModificationService.EEProvider
{
    public class VehicleModificationServiceEEProvider : IVehicleModificationService
    {
        public VehicleModificationServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveModification(CollectiveEmission collectiveEmission)
        {
            try
            {
                if ((SubMassiveProcessType)collectiveEmission.LoadType.Id == SubMassiveProcessType.Inclusion)
                {
                    VehicleInclutionDAO modificationInclutionVehicleDAO = new VehicleInclutionDAO();
                    return modificationInclutionVehicleDAO.CreateVehicleInclution(collectiveEmission);
                }
                else if ((SubMassiveProcessType)collectiveEmission.LoadType.Id == SubMassiveProcessType.Exclusion)
                {
                    VehicleExclutionDAO modificationExclutionVehicleDAO = new VehicleExclutionDAO();
                    return modificationExclutionVehicleDAO.CreateVehicleExclution(collectiveEmission);
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
                VehicleInclutionQuotateDAO collectiveProcessDAO = new VehicleInclutionQuotateDAO();
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
                VehicleExclutionQuotateDAO dao = new VehicleExclutionQuotateDAO();
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
                return new VehicleModificationIssueDAO().IssuanceCollectiveEmission(collectiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
    }
}