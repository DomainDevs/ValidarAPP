using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ClaimServices
{
    [ServiceContract]
    public interface IPaymentRequestApplicationService
    {

        /// <summary>
        /// Retorna las Sucursales
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PrefixDTO> GetPrefixes();

        /// <summary>
        /// Retorna las Sucursales
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<BranchDTO> GetBranchesByUserId(int userId);

        /// <summary>
        /// Retorna las Monedas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CurrencyDTO> GetCurrencies();

        /// <summary>
        /// Retorna el Origen de Pago
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PaymentSourceDTO> GetPaymentSource();

        /// <summary>
        /// Retorna el tipo de Movimiento
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        //List<PaymentMovementTypeDTO> GetPaymentMovementType();

        /// <summary>
        /// Retorna a quien desea pagar
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RoleDTO> GetRoles();

        /// <summary>
        /// Obtiene los Tipos de Comprobantes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<VoucherTypeDTO> GetVoucherTypes();

        /// <summary>
        /// Obtiene los Tipos de Comprobantes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PaymentMethodDTO> GetPaymentMethods();

        /// Obtener un listado de tasas de cambio a partir de la moneda
        /// </summary>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <returns>Listado de tasas de cambio</returns>
        [OperationContract]
        decimal GetExchangeRateByCurrencyId(int currencyId);

        /// <summary>
        /// Crea la Solicitud de pago
        /// </summary>
        /// <returns>Solicitud de pago</returns>
        [OperationContract]
        PaymentRequestDTO CreatePaymentRequest(PaymentRequestDTO paymentRequestDTO);

        /// <summary>
        /// cancela la Solicitud de pago o de cobro
        /// </summary>
        /// <returns>Solicitud de pago</returns>
        [OperationContract]
        PaymentRequestDTO SaveRequestCancellation(int paymentRequestId, bool IsChargeRequest);


        /// <summary>
        /// Retorna el tipo de Movimiento por fuente de pago
        /// </summary>
        /// <param name="paymentSourceId"></param>
        /// <returns>Lista de Tipos de Movimiento</returns>
        [OperationContract]
        List<SelectDTO> GetMovementTypesByConceptSourceId(int conceptSourceId);

        /// <summary>
        /// Retorna los ids de los tipos de estimación para un tipo de movimiento
        /// </summary>
        /// <param name="movementTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<int> GetEstimationsTypesIdByMovementTypeId(int movementTypeId);

        /// <summary>
        ///  Consultar asegurados por numero de documento o apellidos y nombres.
        /// </summary>
        /// <param name="">Numero de documento o apellidos y nombres o razon social</param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualDTO> GetInsuredsByDescriptionIndividualSearchType(string description, InsuredSearchType insuredSearchType, CustomerType customerType);

        /// <summary>
        /// Obtiene los Tipos de persona
        /// </summary>
        /// <returns>Lista de Tipos de persona</returns>
        [OperationContract]
        List<PersonTypeDTO> GetPersonTypes(bool isPaymentRequest);

        /// <summary>
        /// Obtiene conceptos de pago por sucursal y tipo de movimiento
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="movementTypeId"></param>
        /// <returns>Lista de Tipos de compronbantes</returns>
        [OperationContract]
        List<SelectDTO> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId);

        /// <summary>
        ///  Consultar cuentas de banco por individuo.
        /// </summary>
        /// <param name="individualId">Identificador individuo</param>
        /// <returns></returns>
        [OperationContract]
        List<AccountBankDTO> GetAccountBanksByIndividualId(int individualId);

        /// <summary>
        ///  Obtiene la tasa de cambio por fecha y tipo de moneda.
        /// </summary>
        /// <param name="rateDate">Fecha</param>
        /// <param name="currencyId">Tipo de moneda</param>
        /// <returns></returns>
        [OperationContract]
        ExchangeRateDTO GetExchangeRateByRateDateCurrencyId(DateTime rateDate, int currencyId);


        /// <summary>
        /// Consulta fecha contable
        /// </summary>
        /// <param name="moduleType">tipos de modulo</param>
        /// <param name="date">fecha</param>
        /// <returns>Fecha contable</returns>
        [OperationContract]
        DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime date);

        /// <summary>
        /// Listado de concetos de pago
        /// </summary>
        //[OperationContract]
        //List<SelectDTO> GetPaymentConcepts();

        /// <summary>
        /// Crea la Solicitud de cobro
        /// </summary>
        /// <returns>Solicitud de cobro</returns>
        [OperationContract]
        List<ChargeRequestDTO> CreateChargeRequests(List<ChargeRequestDTO> chargeRequestsDTO);

        /// <summary>
        /// Obtiene la solicitud de pago por sucursal, ramo y número
        /// </summary>
        /// <returns>Solicitud de pago</returns>
        [OperationContract]
        PaymentRequestDTO GetPaymentRequestByPrefixIdBranchIdNumber(int prefixId, int branchId, int number);

        /// <summary>
        /// Obtiene la solicitud de cobro por sucursal, ramo y número
        /// </summary>
        /// <returns>Solicitud de cobro</returns>
        [OperationContract]
        ChargeRequestDTO GetChargeRequestByPrefixIdBranchIdNumber(int prefixId, int branchId, int number);
        
        /// <summary>
        /// Buscar solicitudes de pago
        /// </summary>
        /// <param name="paymentRequestDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentRequestDTO> SearchPaymentRequests(PaymentRequestDTO paymentRequestDTO);

        /// <summary>
        /// Buscar solicitudes de cobro
        /// </summary>
        /// <param name="paymentRequestDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<ChargeRequestDTO> SearchChargeRequests(ChargeRequestDTO chargeRequestDTO);

        /// <summary>
        /// Obtener la solicitudes de pago de una denuncia por el identificador
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentRequestDTO> GetPaymentRequestClaimsByPaymentRequestId(int paymentRequestId);

        /// <summary>
        /// Obtener la solicitudes de pago de una denuncia por el identificador
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        [OperationContract]
        PaymentRequestDTO GetPaymentRequestByPaymentRequestId(int paymentRequestId);

        /// <summary>
        /// Obtener la solicitudes de cobro por el identificador
        /// </summary>
        /// <param name="chargeRequestId"></param>
        /// <returns></returns>
        [OperationContract]
        ChargeRequestDTO GetChargeRequestByChargeRequestId(int chargeRequestId);

        /// <summary>
        ///  Obtiene los origenes de pago por isChargerequest
        /// </summary>
        /// <param name="isChargeRequest"></param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentSourceDTO> GetPaymentSourcesByChargeRequest(bool isChargeRequest);

        /// <summary>
        /// Crea la solicitud de pago a partir de un temporal
        /// </summary>
        /// <param name="temporalId"></param>
        [OperationContract]
        void CreatePaymentRequestByTemporalId(int temporalId);

        /// <summary>
        /// Crea la solicitud de cobro a partir de un temporal
        /// </summary>
        /// <param name="temporalId"></param>
        [OperationContract]
        void CreateChargeRequestByTemporalId(int temporalId);

        /// <summary>
        /// Obtiene la moneda por defecto parametrizada para el módulo de solicitudes de pago
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetDefaultPaymentCurrency();

        [OperationContract]
        BranchDTO GetBranchById(int branchId);

        [OperationContract]
        PrefixDTO GetPrefixByPrefixId(int prefixId);

        /// <summary>
        /// Retorna los tipos de persona por el filtro de la tabla ClaimSearchPersonType
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PersonTypeDTO> GetClaimSearchPersonType(int prefixId, int searchType);

        /// <summary>
        /// Obtener la información de solicitudes de pago de una denuncia para generar reporte por el identificador 
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        [OperationContract]
        PaymentRequestReportDTO GetReportPaymentRequestByPaymentRequestId(int paymentRequestId);

        /// <summary>
        /// Obtiene el identificador del impuesto de Retención a la industria y comercio (RTEICA)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetTaxCodeOfRetetionToIndustryAndCommerce();
    }
}
