using System;
using System.Collections.Generic;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Enums;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class RenewalDAO
    {
        /// <summary>
        /// Generar Proceso De Renovación Masiva
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="description">Descripción Proceso</param>
        /// <param name="policies">Pólizas a Renovar</param>
        /// <returns>Id Proceso</returns>
        public int GenerateProcessRenewal(int userId, string description, List<CompanyPolicy> policies)
        {
            int processId = DelegateService.utilitiesService.GenerateAsynchronousProcessId(userId, description, (int)ProcessRenewalStatus.Inicialized);

            if (processId > 0)
            {
                ThreadRenewalDAO threadRenewalDAO = new ThreadRenewalDAO();
                TP.Task.Run(() => threadRenewalDAO.InicializeRenewal(userId, processId, policies));
            }            

            return processId;
        }

        /// <summary>
        /// Obtener Temporales Por Id De Proceso
        /// </summary>
        /// <param name="processId">Id De Proceso</param>
        /// <returns>Lista De Temporales</returns>
        public List<Policy> GetTemporalPoliciesByProcessId(int processId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtener Pólizas Por Id De Proceso
        /// </summary>
        /// <param name="processId">Id De Proceso</param>
        /// <returns>Lista De Pólizas</returns>
        public List<Policy> GetPoliciesByProcessId(int processId)
        {
            throw new NotImplementedException();
        }
    }
}