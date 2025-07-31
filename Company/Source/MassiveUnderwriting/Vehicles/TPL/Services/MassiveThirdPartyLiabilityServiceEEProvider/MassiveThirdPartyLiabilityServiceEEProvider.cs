using System;
using System.ServiceModel;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using System.Collections.Generic;
using Sistran.Company.Application.Vehicles.MassiveTPLServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Helpers;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveThirdPartyLiabilityServiceEEProvider : Sistran.Core.Application.Vehicles.MassiveTPLServices.EEProvider.MassiveTPLServiceEEProvider, IMassiveThirdPartyLiabilityService
    {
        public MassiveThirdPartyLiabilityServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear Cargue
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            try
            {
                MassiveLoadTplDAO MassiveLoadTplDAO = new MassiveLoadTplDAO();
                return MassiveLoadTplDAO.CreateMassiveEmission(massiveEmission);
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogError(massiveEmission.ErrorDescription);
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = ex.Message;
                return massiveEmission;
            }
        }

        /// <summary>
        /// Tarifar Cargue
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad QuotateMassiveLoad(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadRowTplDAO massiveLoadProcessVehicleDAO = new MassiveLoadRowTplDAO();
                return massiveLoadProcessVehicleDAO.QuotateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadProccessId"></param>
        /// <returns></returns>
        public string GenerateReportToMassiveLoad(MassiveEmission massiveLoad)
        {
            try
            {
                MassiveLoadTplDAO MassiveLoadTplDAO = new MassiveLoadTplDAO();
                return MassiveLoadTplDAO.GenerateReportToMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateReportToMassiveLoad), ex);
            }
        }

        public MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                MassiveLoadRenewalTplDAO massiveLoadRenewalDAO = new MassiveLoadRenewalTplDAO();
                return massiveLoadRenewalDAO.CreateMassiveLoad(massiveRenewal);
            }
            catch (Exception ex)
            {
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = ex.Message;
                return massiveRenewal;
            }
        }

        public MassiveLoad QuotateMassiveRenewal(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadRenewalRowTplDAO massiveRowRenewalDAO = new MassiveLoadRenewalRowTplDAO();
                return massiveRowRenewalDAO.QuotateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateMassiveRenewal), ex);
            }
        }

        //public List<VehiclePolicyExtend> GetExtendListByVehiclePlateCollection(List<string> licensePlateVehicleCollection)
        //{
        //    try
        //    {
        //        return DelegateService.tplService.GetExtendListByVehiclePlateCollection(licensePlateVehicleCollection);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetExtendListByVehiclePlateCollection), ex);
        //    }
        //}

        public string GenerateFileErrorsMassiveEmission(int massiveLoadId)
        {
            try
            {
                MassiveLoadTplDAO massiveLoadProcessDao = new MassiveLoadTplDAO();
                return massiveLoadProcessDao.GenerateFileErrorsMassiveEmission(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }

        public string GenerateReportToMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                MassiveLoadRenewalTplDAO massiveRenewalPropertyDao = new MassiveLoadRenewalTplDAO();
                return massiveRenewalPropertyDao.GenerateReportToMassiveRenewal(massiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }

        public MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadRowTplDAO massiveVehicleRowDao = new MassiveLoadRowTplDAO();
                return massiveVehicleRowDao.IssuanceMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuing), ex);
            }
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadRenewalRowTplDAO massiveLoadRenewalRowVehicleDAO = new MassiveLoadRenewalRowTplDAO();
                return massiveLoadRenewalRowVehicleDAO.IssuanceRenewalMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuing), ex);
            }
        }
    }
}