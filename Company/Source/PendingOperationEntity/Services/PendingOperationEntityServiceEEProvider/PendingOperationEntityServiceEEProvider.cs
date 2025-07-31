using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.PendingOperationEntityServiceEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PendingOperationEntityServiceEEProvider : IPendingOperationEntityService
    {
        /// <summary>
        /// Guardar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation CreatePendingOperation(PendingOperation pendingOperation)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        public bool DeletePendingOperation(int id)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.DeletePendingOperation(id);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar Hijos de un JSON
        /// </summary>
        /// <param name="parentId">Id Padre</param>
        /// <returns>Eliminados Si/No</returns>
        public bool DeletePendingOperationsByParentId(int parentId)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.DeletePendingOperationsByParentId(parentId);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation GetPendingOperationById(int id)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.GetPendingOperationById(id);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener JSON hijo
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="parentId">Id padre</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation GetPendingOperationByIdParentId(int id, int parentId)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.GetPendingOperationByIdParentId(id, parentId);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de JSON
        /// </summary>
        /// <param name="parentId">Id padre</param>
        /// <returns>Lista de JSONs</returns>
        public List<PendingOperation> GetPendingOperationsByParentId(int parentId)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(parentId);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation UpdatePendingOperation(PendingOperation pendingOperation)
        {
            try
            {
                return DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para devolver poliza del esquema report
        /// </summary>
        /// <param name="prefixId">ramo </param>
        /// <param name="branchId">sucursal</param>
        /// <param name="documentNumber">numero de poliza</param>
        /// <param name="endorsementType"> tipo de endos</param>
        /// <returns>modelo company policy</returns>
        public String GetPolicyByEndorsementDocumentNumber(int endorsementId, decimal documentNumber)
        {
            try
            {
                return DelegateService.underwritingService.GetPolicyByEndorsementDocumentNumber(endorsementId, documentNumber);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void RecordEndorsementOperation(int endorsementId, int pendingOperationId)
        {
            try
            {
                DelegateService.underwritingService.RecordEndorsementOperation(endorsementId, pendingOperationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<String> GetRiskByEndorsementDocumentNumber(int endorsementId)
        {
            try
            {
                return DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public string GetCompanyPolicyJsonByEndorsementId(int endorsementId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtener Póliza Por Identificador
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Póliza</returns>
        /*   public string GetCompanyPolicyJsonByEndorsementId(int endorsementId)
           {
               try
               {
                   return DelegateService.underwritingService.GetCompanyPolicyJsonByEndorsementId(endorsementId);
               }
               catch (Exception ex)
               {
                   throw new BusinessException(ex.Message, ex);
               }
           }*/
    }
}