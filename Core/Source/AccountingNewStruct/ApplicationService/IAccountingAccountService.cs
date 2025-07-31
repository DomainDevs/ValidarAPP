using System;
using System.Collections.Generic;
using System.ServiceModel;
using SearchDTO=Sistran.Core.Application.AccountingServices.DTOs.Search;

//Sistran
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingAccountService
    {
        #region Parameters

        /// <summary>
        /// Metodo para obtener los parámetros para armar la contabilización de un ingreso de caja
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<AccountingParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.AccountingParameterDTO> GetAccountingParameters(int collectId);

        /// <summary>
        /// Metodo para obtener los parámetros para armar la contabilización de un ingreso de caja
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ApplicationParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.ApplicationParameterDTO> GetApplicationParameters(int collectId, int imputationTypeId);

        /// <summary>
        /// Método para cargar los ramos y subramos de cada componente.
        /// </summary>
        /// <param name="applicationParameters"></param>
        /// <returns>List<ApplicationParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.ApplicationParameterDTO> GetPrefixComponentCollectionsByComponent(List<SearchDTO.ApplicationParameterDTO> applicationParameters);

        /// <summary>
        /// Metodo para obtener los parámetros para armar la contabilización de boleta de depósito de cheques
        /// </summary>
        /// <param name="paymentBallotId"></param>
        /// <returns>List<CheckBallotAccountingParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckBallotAccountingParameterDTO> GetCheckBallotAccountingParameters(int paymentBallotId);

        /// <summary>
        /// Obtiene la imputación de tablas reales, junto con todos los movimientos involucrados
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="moduleDateId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.ImputationParameterDTO> GetImputationParameters(int sourceId, int imputationTypeId, int userId, int moduleId, int subModuleId, int moduleDateId);


        /// <summary>
        /// Método para obtener los parámetros para contabilización de solicitud de pagos/ solicitud de pagos varios
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="paymentSourceId"></param>
        /// <param name="moduleId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="userId"></param>
        /// <returns>List<PaymentRequestAccountingParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.PaymentRequestAccountingParameterDTO> GetPaymentRequestAccountingParameters(int paymentRequestId, int paymentSourceId, int moduleId, int subModuleId, int moduleDateId, int userId);

        /// <summary>
        /// Obtiene parametros para pagos contabilidad
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="billId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        int AccountingParametersRequest(SaveBillParametersDTO saveBillParametersDTO);


        /// <summary>
        /// contabiilidad rechazo de cheques
        /// </summary>
        /// <param name="saveBillParametersDTO"></param>
        /// <returns></returns>
        [OperationContract]
        int AccountingChecks(SaveBillParametersDTO saveBillParametersDTO);

        /// <summary>
        /// Realiza grabación (ingreso y contabilidad) incluido boleta interna y deposito (aplica solo cuando el metodo de pago es consignación de cheque)
        /// </summary>
        [OperationContract]
        MessageSuccessDTO SaveCollectGeneralLedgerPayment(CollectGeneralLedgerDTO collectGeneralLedgerDTO);

        #endregion Parameters

        #region calculate

        /// <summary>
        /// Metodo para realizar los calculos cuando es imputacion de primas con coaseguro aceptado o cedido
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List<ApplicationParameterDTO/></returns>
        [OperationContract]
        List<SearchDTO.ApplicationParameterDTO> Calculate(List<SearchDTO.ApplicationParameterDTO> list);

        #endregion calculate

    }
}
