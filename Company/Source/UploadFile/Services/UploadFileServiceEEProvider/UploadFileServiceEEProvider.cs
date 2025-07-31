
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Company.Application.UploadFileServices.Models;
using Sistran.Company.Services.UploadFileServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.BAF.Application;
using Sistran.Core.Framework.SAF.Integration.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.UploadFileServices.EEProvider
{
    [ActionTransaction(ActionTransactionOption.Required)]
    public class UploadFileServiceEEProvider : ServiceBase, IUploadFileService
    {
        /// <summary>
        /// Inicia el proceso de Excel
        /// Obtiene los valores y Guarda en la Base de Datos.
        /// </summary>
        /// <param name="massiveLoadProcess">Información del proceso a iniciar.</param>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="fieldSet">Id del ramo.</param>
        /// <returns>Modelo MassiveLoadError si se presenta un error.</returns>
        public List<MassiveLoadVehicleReception> InitExcelProcess(MassiveLoadProcess massiveLoadProcess, string userName, int fieldSetId, int tempId)
        {
            try
            {
                MassiveLoadThreadDAO massiveLoadThread = new MassiveLoadThreadDAO(this.RequestProcessor);
                massiveLoadThread.setMassiveLoadProcess(massiveLoadProcess);
                return massiveLoadThread.InitExcelProcess(userName, fieldSetId, tempId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
