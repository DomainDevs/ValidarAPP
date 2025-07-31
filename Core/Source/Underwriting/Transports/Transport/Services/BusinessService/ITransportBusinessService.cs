using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Transports.TransportBusinessService
{
    [ServiceContract]
    public interface ITransportBusinessService
    {
        #region Transport
        /// <summary>
        /// Obtiener la lista de tipos de mercancias
        /// </summary>
        /// <returns>Lista de Mercancias</returns>
        [OperationContract]
        List<CargoType> GetCargoTypes();

        /// <summary>
        /// Obtiener la lista de medios de transporte
        /// </summary>
        /// <returns>Lista de Medios de Transporte</returns>
        [OperationContract]
        List<TransportType> GetTransportTypes();

        /// <summary>
        /// Obtiener la lista de tipos de empaques
        /// </summary>
        /// <returns>Lista de Empaques</returns>
        [OperationContract]
        List<PackagingType> GetPackagingTypes();

        /// <summary>
        /// Obtiener la lista de Periodos de Declaración
        /// </summary>
        /// <returns>Lista de periodos de declaracion </returns>
        [OperationContract]
        List<DeclarationPeriod> GetDeclarationPeriods();

        /// <summary>
        /// Obtiener la lista de tipos de vías
        /// </summary>
        /// <returns>Lista de tipos de vías</returns>
        [OperationContract]
        List<ViaType> GetViaTypes();

        /// <summary>
        /// Obtiener la lista de periodos de ajuste
        /// </summary>
        /// <returns>Lista de tipos de periodos de ajuste</returns>
        [OperationContract]
        List<AdjustPeriod> GetAdjustPeriods();

        #endregion
        #region Integration

        [OperationContract]
        List<Transport> GetTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        List<Transport> GetTransportsByInsuredId(int insuredId);

        [OperationContract]
        Transport GetTransportByRiskId(int riskId);
        #endregion
    }
}