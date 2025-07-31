using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.MassiveRenewalServices.EEProvider.DAOs;
using Sistran.Company.Application.MassiveRenewalServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveRenewalServiceEEProvider : MassiveRenewalServiceEEProviderCore, IMassiveRenewalService
    {
        public MassiveRenewalServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Generar Proceso De Renovación Masiva
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="description">Descripción Proceso</param>
        /// <param name="policies">Pólizas a Renovar</param>
        /// <returns>Id Proceso</returns>
        public int GenerateProcessRenewal(int userId, string description, List<CompanyPolicy> policies)
        {
            try
            {
                RenewalDAO renewalDAO = new RenewalDAO();
                return renewalDAO.GenerateProcessRenewal(userId, description, policies);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error in GenerateProcessRenewal", ex);
            }
        }

        /// <summary>
        /// Generar Archivo Planilla Intermediario
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToPayrollByAgent(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToPayrollByAgent(massiveRenewalRows, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy CreateCompanyPolicy(File file, string templatePropertyName, int userId, int selectedPrefixId)
        {
            try
            {
                MassiveLoadRenewalPolicyDAO massiveLoadPolicyDAO = new MassiveLoadRenewalPolicyDAO();
                return massiveLoadPolicyDAO.CreateCompanyPolicy(file, templatePropertyName, userId, selectedPrefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex,Errors.ErrorCreatingCompanyPolicy));
            }
        }

        public MassiveLoad IssuanceMassiveEmission(int massiveLoadId)
        {
            try
            {
                MassiveLoadRenewalPolicyDAO massiveLoadPolicyDAO = new MassiveLoadRenewalPolicyDAO();
                return massiveLoadPolicyDAO.IssuanceMassiveEmission(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingMassiveLoadPolicy));
            }
        }

        public string PrintRenewalLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user,bool checkIssuedDetail)
        {
            try
            {
                RenewalPrintDAO renewalPrintDAO = new RenewalPrintDAO();
                return renewalPrintDAO.PrintRenewalLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingMassiveLoadPolicy));
            }
        }

        public string GenerateFileErrorsMassiveRenewals(int massiveLoadId)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileErrorsMassiveRenewals(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }
    }
}