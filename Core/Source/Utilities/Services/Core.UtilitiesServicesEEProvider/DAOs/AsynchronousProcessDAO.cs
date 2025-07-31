using Sistran.Core.UtilitiesServicesEEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs
{
    public class AsynchronousProcessDAO
    {
        private static readonly Object thisLock = new Object();
        /// <summary>
        /// Generar Id Proceso Asíncrono
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="description">Descripción Proceso</param>
        /// <returns>Id Proceso</returns>
        public int GenerateAsynchronousProcessId(int userId, string description, int statusId)
        {
            COMMEN.AsynchronousProcess entityAsynchronousProcess = new COMMEN.AsynchronousProcess();
            entityAsynchronousProcess.UserId = userId;
            entityAsynchronousProcess.Description = description;
            entityAsynchronousProcess.BeginDate = DateTime.Now;
            //entityAsynchronousProcess.Active = true;
            //entityAsynchronousProcess.StatusId = statusId;

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityAsynchronousProcess);

            return entityAsynchronousProcess.ProcessId;
        }

        /// <summary>
        /// Actualizar Proceso Asíncrono
        /// </summary>
        /// <param name="asynchronousProcess">Proceso Asíncrono</param>
        /// <returns>Proceso Asíncrono</returns>
        public AsynchronousProcess UpdateAsynchronousProcess(AsynchronousProcess asynchronousProcess)
        {
            PrimaryKey primaryKey = COMMEN.AsynchronousProcess.CreatePrimaryKey(asynchronousProcess.Id);

            COMMEN.AsynchronousProcess entityAsynchronousProcess = (COMMEN.AsynchronousProcess)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityAsynchronousProcess != null)
            {
                entityAsynchronousProcess.EndDate = asynchronousProcess.EndDate;
                entityAsynchronousProcess.HasError = asynchronousProcess.HasError;
                entityAsynchronousProcess.ErrorDescription = asynchronousProcess.ErrorDescription;
                //entityAsynchronousProcess.Active = asynchronousProcess.Active;
                //entityAsynchronousProcess.StatusId = asynchronousProcess.StatusId;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityAsynchronousProcess);

                if (entityAsynchronousProcess != null)
                {
                    return asynchronousProcess;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Obtener Proceso Asíncrono Por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Proceso Asíncrono</returns>
        public AsynchronousProcess GetAsynchronousProcessById(int id)
        {
            PrimaryKey primaryKey = COMMEN.AsynchronousProcess.CreatePrimaryKey(id);
            COMMEN.AsynchronousProcess entityAsynchronousProcess = (COMMEN.AsynchronousProcess)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityAsynchronousProcess != null)
            {
                return ModelAssembler.CreateAsynchronousProcess(entityAsynchronousProcess);
            }
            else
            {
                return null;
            }
        }
    }
}