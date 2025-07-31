using Sistran.Core.Application.MassiveRenewalServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveRenewalServiceEEProviderCore : IMassiveRenewalServiceCore
    {
        public MassiveRenewalServiceEEProviderCore()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Obtener Pólizas Por Fecha De Vencimiento
        /// </summary>
        /// <param name="policy">Parametros De Busqueda</param>
        /// <returns>Lista De Pólizas</returns>
        public List<UnderwritingServices.Models.Policy> GetPoliciesByDueDate(UnderwritingServices.Models.Policy policy)
        {
            try
            {
                PolicyRenewalDAO policyRenewalDAO = new PolicyRenewalDAO();
                return policyRenewalDAO.GetPoliciesByDueDate(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();
                return massiveRenewalDAO.CreateMassiveRenewal(massiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MassiveRenewal UpdateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();
                return massiveRenewalDAO.UpdateMassiveRenewal(massiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MassiveRenewalRow CreateMassiveRenewalRow(MassiveRenewalRow massiveRenewalRow)
        {
            try
            {
                MassiveRenewalRowDAO massiveRenewalDAO = new MassiveRenewalRowDAO();
                return massiveRenewalDAO.CreateMassiveRenewalRow(massiveRenewalRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public MassiveRenewalRow UpdateMassiveRenewalRow(MassiveRenewalRow massiveRenewalRow)
        {
            try
            {
                MassiveRenewalRowDAO massiveRenewalDAO = new MassiveRenewalRowDAO();
                return massiveRenewalDAO.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MassiveRenewal GetMassiveRenewalByMassiveRenewalId(int massiveLoadId, bool withRows, bool? withError, bool? withEvent)
        {
            try
            {
                MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();
                return massiveRenewalDAO.GetMassiveRenewalByMassiveLoadIdWithRowsErrors(massiveLoadId,withRows, withError, withEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<MassiveRenewalRow> GetMassiveLoadProcessByMassiveRenewalProcessId(int massiveRenewalId, MassiveLoadProcessStatus massiveLoadProcessStatus)
        {
            try
            {
                MassiveRenewalRowDAO massiveRenewalDAO = new MassiveRenewalRowDAO();
                return massiveRenewalDAO.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveRenewalId, massiveLoadProcessStatus);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Excluye y elimina los temporales del cargue
        /// </summary>
        /// <param name="massiveRenewalId">Id del Cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Nombre del Usuario</param>
        public bool ExcludeMassiveRenewalRowsTemporals(int massiveRenewalId, List<int> temps, string userName)
        {
            try
            {
                MassiveRenewalRowDAO massiveRenewalDAO = new MassiveRenewalRowDAO();
                return massiveRenewalDAO.ExcludeMassiveRenewalRowsTemporals(massiveRenewalId, temps, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        

        // <summary>
        // Genera el archivo de error del proceso de emisión masiva
        // </summary>
        // <param name = "massiveLoadProccessId" ></ param >
        // < returns ></ returns >
        public string GenerateFileErrorsMassiveRenewal(int massiveLoadId)
        {
            try
            {
                MassiveRenewalDAO massiveRenewalDAO = new MassiveRenewalDAO();
                return massiveRenewalDAO.GenerateFileErrorsMassiveRenewal(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<MassiveRenewalRow> GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadProcessId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            try
            {
                MassiveRenewalRowDAO massiveRenewalRowDAO = new MassiveRenewalRowDAO();
                return massiveRenewalRowDAO.GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadProcessId, massiveLoadProcessStatus, withError, withEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}