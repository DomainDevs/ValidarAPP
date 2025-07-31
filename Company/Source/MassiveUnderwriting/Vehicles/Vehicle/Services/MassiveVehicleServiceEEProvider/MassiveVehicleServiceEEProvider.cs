using System;
using System.ServiceModel;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using System.Collections.Generic;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Helpers;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveVehicleServiceEEProvider : Sistran.Core.Application.Vehicles.MassiveVehicleServices.EEProvider.MassiveVehicleServiceEEProvider, IMassiveVehicleService
    {
        public MassiveVehicleServiceEEProvider()
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
                MassiveLoadVehicleDAO massiveLoadVehicleDAO = new MassiveLoadVehicleDAO();
                return massiveLoadVehicleDAO.CreateMassiveEmission(massiveEmission);
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
                MassiveLoadRowVehicleDAO massiveLoadProcessVehicleDAO = new MassiveLoadRowVehicleDAO();
                return massiveLoadProcessVehicleDAO.QuotateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorQuotateMassiveLoad), ex);
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
                MassiveLoadVehicleDAO massiveLoadVehicleDAO = new MassiveLoadVehicleDAO();
                return massiveLoadVehicleDAO.GenerateReportToMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateReportToMassiveLoad), ex);
            }
        }

        public MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                MassiveLoadRenewalVehicleDAO massiveLoadRenewalDAO = new MassiveLoadRenewalVehicleDAO();
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
                MassiveLoadRenewalRowVehicleDAO massiveRowRenewalDAO = new MassiveLoadRenewalRowVehicleDAO();
                return massiveRowRenewalDAO.QuotateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorCreateMassiveRenewal), ex);
            }
        }

        //public List<VehiclePolicyExtend> GetExtendListByVehiclePlateCollection(List<string> licensePlateVehicleCollection)
        //{
        //    try
        //    {
        //        return DelegateService.vehicleService.GetExtendListByVehiclePlateCollection(licensePlateVehicleCollection);
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
                MassiveLoadVehicleDAO massiveLoadProcessDao = new MassiveLoadVehicleDAO();
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
                MassiveLoadRenewalVehicleDAO massiveRenewalPropertyDao = new DAOs.MassiveLoadRenewalVehicleDAO();
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
                MassiveLoadRowVehicleDAO massiveVehicleRowDao = new DAOs.MassiveLoadRowVehicleDAO();
                return massiveVehicleRowDao.IssuanceMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuing), ex);
            }
        }

        //public int FindExternalServicesLoad(int massiveLoadId)
        //{
        //    try
        //    {
        //        MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
        //        massiveLoad.Status = Core.Application.MassiveServices.Enums.MassiveLoadStatus.Querying;
        //        DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
        //        return DelegateService.externalProxyService.FindExternalServicesLoad(massiveLoadId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExternalServices), ex);
        //    }
        //}

        public void ProcessResponseFromScoreService(string response)
        {
            try
            {
                MassiveLoadVehicleDAO massiveLoadVehicleDAO = new MassiveLoadVehicleDAO();
                massiveLoadVehicleDAO.ProcessResponseFromScoreService(response);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorResponseScoreService), ex);
            }
        }

        public void ProcessResponseFromSimitService(string response)
        {
            try
            {
                MassiveLoadVehicleDAO massiveLoadVehicleDAO = new MassiveLoadVehicleDAO();
                massiveLoadVehicleDAO.ProcessResponseFromSimitService(response);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorResponseSimitService), ex);
            }
        }

        public void ProcessResponseFromExperienceServiceHistoricPolicies(string response)
        {
            try
            {
                MassiveLoadVehicleDAO massiveLoadVehicleDAO = new MassiveLoadVehicleDAO();
                massiveLoadVehicleDAO.ProcessResponseFromExperienceServiceHistoricPolicies(response);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorResponseHistoricPoliciesService), ex);
            }
        }

        public void ProcessResponseFromExperienceServiceHistoricSinister(string response)
        {
            try
            {
                MassiveLoadVehicleDAO massiveLoadVehicleDAO = new MassiveLoadVehicleDAO();
                massiveLoadVehicleDAO.ProcessResponseFromExperienceServiceHistoricSinister(response);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorResponseHistoricSinestersService), ex);
            }
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadRenewalRowVehicleDAO massiveLoadRenewalRowVehicleDAO = new DAOs.MassiveLoadRenewalRowVehicleDAO();
                return massiveLoadRenewalRowVehicleDAO.IssuanceRenewalMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuing), ex);
            }
        }
    }
}