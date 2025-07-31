using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.VehicleEndorsementCancellationService3GProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;

namespace Sistran.Core.Application.VehicleEndorsementCancellationService.EEProvider
{
    public class VehicleCancellationServiceEEProvider : 
        Sistran.Core.Application.CancellationEndorsement.EEProvider.CancellationEndorsementEEProvider, IVehicleCancellationService
    {
        internal static IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #region Miembros de IVehicleEndorsementCancellation

        public virtual object GetVehicleEndorsementCancellation(int idVehicleEndorsementCancellation)
        {
            throw new NotImplementedException();            
        }

        /// <summary>
        /// Cancela una póliza a partir del inicio de vigencia
        /// </summary>
        /// <param name="documentNumber"> Número de documento </param>
        /// <param name="branchCode"> Sucursal </param>
        /// <param name="prefixCode"> Ramo comercial </param>
        /// <param name="conditionText"> Texto </param>
        /// <param name="endorsementReason">  </param>
        /// <param name="userId"></param>
        /// <param name="annotations"></param>
        /// <param name="isNominative"></param>
        public void CancellationPolicy(int documentNumber, int branchCode, int prefixCode, string conditionText, int endorsementReason, int userId, string annotations, bool isNominative)
        {
            try
            {
                CancellationDAO cancellationDAO = new CancellationDAO();
                cancellationDAO.CancellationPolicy(documentNumber, branchCode, prefixCode, conditionText, endorsementReason, userId, annotations, isNominative);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, VehicleEndorsementCancellationService3GProvider.Resources.Errors.ErrorCancelPolicyTheBeginningOfValidity), ex);
            } 
        }

        /// <summary>
        /// Crea el temporal de cnacelación de una póliza
        /// </summary>
        /// <param name="policyId"> Identificador de la póliza </param>
        /// <param name="userId"> Identificador del usuario que está realizando la operación </param>
        /// <param name="conditions">Texto de las condiciones </param>
        /// <param name="endorsementReason">Razón del endoso</param>
        /// <param name="annotations">  </param>
        public void CreateTemporalCancelEndorsement(int policyId, int userId, string conditions, int endorsementReason, string annotations)
        {
            try
            {
                CancellationDAO cancellationDAO = new CancellationDAO();
                cancellationDAO.CreateTemporalCancelEndorsement(policyId, userId, conditions, endorsementReason, annotations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, VehicleEndorsementCancellationService3GProvider.Resources.Errors.ErrorCreateTemporaryCancellationOfPolicy), ex);
            }
        }

        /// <summary>
        /// Guarda el endoso de cancelación de un temporal
        /// </summary>
        /// <param name="tempId"> Identificador del temporal </param>
        public void SaveEndorsementCancel(int tempId)
        {
            try
            {
                CancellationDAO cancellationDAO = new CancellationDAO();
                cancellationDAO.SaveEndorsementCancel(tempId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, VehicleEndorsementCancellationService3GProvider.Resources.Errors.ErrorSaveEndorsementTemporaryCancellation), ex);
            }
        }

        #endregion
    }
}
