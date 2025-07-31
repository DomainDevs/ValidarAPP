using Sistran.Co.Application.Data;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.UtilitiesServicesEEProvider.Assemblers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs
{
    public class PendingOperationsDAO
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
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                COMMEN.PendingOperations pendingOperationEntity = EntityAssembler.CreatePendingOperation(pendingOperation);


                DataFacadeManager.Instance.GetDataFacade().InsertObject(pendingOperationEntity);
                DataFacadeManager.Dispose();


                pendingOperation.Id = pendingOperationEntity.Id;
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.CreatePendingOperation");
                return pendingOperation;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Actualizar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation UpdatePendingOperation(PendingOperation pendingOperation)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.PendingOperations.CreatePrimaryKey(pendingOperation.Id);
            COMMEN.PendingOperations pendingOperationsEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                pendingOperationsEntity = (COMMEN.PendingOperations)daf.GetObjectByPrimaryKey(key);

            }

            if (pendingOperationsEntity != null)
            {
                pendingOperationsEntity.Operation = pendingOperation.Operation;
                pendingOperationsEntity.ModificationDate = DateTime.Now;

                if (pendingOperation.ParentId > 0)
                {
                    pendingOperationsEntity.ParentId = pendingOperation.ParentId;
                }
                if (pendingOperation.AdditionalInformation != null)
                {
                    pendingOperationsEntity.AdditionalInformation = pendingOperation.AdditionalInformation;
                }

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(pendingOperationsEntity);
                DataFacadeManager.Dispose();


                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.UpdatePendingOperation");
                return pendingOperation;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        public bool DeletePendingOperation(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.PendingOperations.CreatePrimaryKey(id);
            COMMEN.PendingOperations pendingOperationsEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                pendingOperationsEntity = (COMMEN.PendingOperations)daf.GetObjectByPrimaryKey(key);
            }


            if (pendingOperationsEntity != null)
            {

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(pendingOperationsEntity);
                DataFacadeManager.Dispose();

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.DeletePendingOperation");
                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.DeletePendingOperation");
                return false;
            }
        }

        /// <summary>
        /// Obtener JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation GetPendingOperationById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.PendingOperations.CreatePrimaryKey(id);
            COMMEN.PendingOperations pendingOperations = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {

                pendingOperations = (COMMEN.PendingOperations)daf.GetObjectByPrimaryKey(key);

            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetPendingOperationById");
            if (pendingOperations != null)
            {
                return ModelAssembler.CreatePendingOperation(pendingOperations);
            }
            else
            {
                return null;
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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.PendingOperations.Properties.Id, typeof(COMMEN.PendingOperations).Name);
            filter.Equal();
            filter.Constant(id);
            filter.And();
            if (parentId == 0)
            {
                filter.Property(COMMEN.PendingOperations.Properties.ParentId, typeof(COMMEN.PendingOperations).Name);
                filter.IsNull();
            }
            else
            {
                filter.Property(COMMEN.PendingOperations.Properties.ParentId, typeof(COMMEN.PendingOperations).Name);
                filter.Equal();
                filter.Constant(parentId);
            }

            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {

                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.PendingOperations), filter.GetPredicate()));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetPendingOperationByIdParentId");
            return ModelAssembler.CreatePendingOperations(businessCollection).FirstOrDefault();
        }

        /// <summary>
        /// Obtener lista de JSON
        /// </summary>
        /// <param name="parentId">Id padre</param>
        /// <returns>Lista de JSONs</returns>
        public List<PendingOperation> GetPendingOperationsByParentId(int parentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.PendingOperations.Properties.ParentId, typeof(COMMEN.PendingOperations).Name);
            filter.Equal();
            filter.Constant(parentId);


            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {

                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.PendingOperations), filter.GetPredicate()));
            }
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetPendingOperationsByParentId");
            return ModelAssembler.CreatePendingOperations(businessCollection);
        }

        /// <summary>
        /// Eliminar Hijos de un JSON
        /// </summary>
        /// <param name="parentId">Id Padre</param>
        /// <returns>Eliminados Si/No</returns>
        public bool DeletePendingOperationsByParentId(int parentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.PendingOperations.Properties.ParentId, typeof(COMMEN.PendingOperations).Name);
            filter.Equal();
            filter.Constant(parentId);
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.PendingOperations), filter.GetPredicate()));
            }
            
            foreach (COMMEN.PendingOperations item in businessCollection)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
            }
            DataFacadeManager.Dispose();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.DeletePendingOperationsByParentId");
            return true;
        }
    }
}
