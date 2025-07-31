
using Sistran.Core.Application.AutomaticDebitServices.DTOs;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.AutomaticDebitServices
{
    [ServiceContract]
    public interface IAutomaticDebitService
    {


        #region BankNetwork

        /// <summary>
        /// GetBankNetworks : Obtiene Redes
        /// </summary>
        /// <returns>List<BankNetwork></returns>
        [OperationContract]
        List<BankNetwork> GetBankNetworks();


        /// <summary>
        /// SaveBankNetwork: Graba Red
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        [OperationContract]
        BankNetwork SaveBankNetwork(BankNetwork bankNetwork);

        /// <summary>
        /// UpdateBankNetwork: Actualiza Red
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        [OperationContract]
        BankNetwork UpdateBankNetwork(BankNetwork bankNetwork);

        /// <summary>
        /// DeleteBankNetwork: Elimina una Red
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteBankNetwork(BankNetwork bankNetwork);



        #endregion

        #region CouponStatus

        /// <summary>
        /// GetCouponStatusByGroup: Obtiene todos los estado de cupones por Grupo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CouponStatus> GetCouponStatusByGroup(string GroupDescription);

        /// <summary>
        /// SaveCouponStatus: Graba Estado de Cupones
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        [OperationContract]
        CouponStatus SaveCouponStatus(CouponStatus couponStatus);

        /// <summary>
        /// UpdateCouponStatus: Actualiza el Estado del Cupon
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        [OperationContract]
        CouponStatus UpdateCouponStatus(CouponStatus couponStatus);

        /// <summary>
        /// DeleteCouponStatus: Elimina un Estado del Cupon
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteCouponStatus(CouponStatus couponStatus);

        #endregion

        #region PaymentMethodBankNetwork

        /// <summary>
        /// GetPaymentMethodBankNetwork: Obtiene todos los Metodos de Pago - Red 
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>List<PaymentMethodBankNetwork></returns>
        [OperationContract]
        List<PaymentMethodBankNetwork> GetPaymentMethodBankNetworks(PaymentMethodBankNetwork paymentMethodBankNetwork);

        /// <summary>
        /// SavePaymentMethodBankNetwork: Graba Metodo de Pago - Red
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        [OperationContract]
        PaymentMethodBankNetwork SavePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork);

        /// <summary>
        /// UpdatePaymentMethodBankNetwork: Actualiza Metodo de Pago - Red
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        [OperationContract]
        PaymentMethodBankNetwork UpdatePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork);

        /// <summary>
        /// DeletePaymentMethodBankNetwork: Borra Metod de Pago - red
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeletePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork);

        #endregion

        #region PaymentMethodAccountType

        /// <summary>
        /// GetPaymentMethodAccountTypes: Obtiene todos los Metodos de Pago-Tipo de Cuenta
        /// </summary>
        /// <returns>List<PaymentMethodAccountType></returns>
        [OperationContract]
        List<PaymentMethodAccountType> GetPaymentMethodAccountTypes();

        /// <summary>
        /// SavePaymentMethodAccountType: Graba un Metodo de Pago-Tipo de Cuenta
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        [OperationContract]
        PaymentMethodAccountType SavePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType);

        /// <summary>
        /// UpdatePaymentMethodAccountType: Actualiza un Metodo de Pago-Tipo de Cuenta
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        [OperationContract]
        PaymentMethodAccountType UpdatePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType);

        /// <summary>
        /// DeletePaymentMethodAccountType: Elimina un Metodo de Pago-Tipo de Cuenta
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeletePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType);

        #endregion 

        #region GeneratedAutomaticDebit

        /// <summary>
        /// SaveAutomaticDebit : Grabar un Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        [OperationContract]
        AutomaticDebit SaveAutomaticDebit(AutomaticDebit automaticDebit);

        /// <summary>
        /// UpdateAutomaticDebit : Actualiza un Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        [OperationContract]
        AutomaticDebit UpdateAutomaticDebit(AutomaticDebit automaticDebit);

        /// <summary>
        /// GetAutomaticDebit : Obtiene el Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        [OperationContract]
        AutomaticDebit GetAutomaticDebit(AutomaticDebit automaticDebit);

        /// <summary>
        /// GetAutomaticDebits : Obtiene todos los Debito Automaticos
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        [OperationContract]
        List<AutomaticDebit> GetAutomaticDebits(AutomaticDebit automaticDebit);


        /// <summary>
        /// GetAutomaticDebitSummary : Obtiene Resumen del Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns> List<SummaryAutomaticDebitDTO></SummaryAutomaticDebitDTO></returns>
        [OperationContract]
        List<AutomaticDebitSummaryDTO> GetAutomaticDebitSummary(AutomaticDebit automaticDebit);

        /// <summary>
        /// GetCouponStatusDetail: Obtiene Reorte de Estado de Cupones
        /// </summary>
        /// <param name="DateTo"></param>
        /// <param name="DateFrom"></param>
        /// <param name="prefix"></param>
        /// <param name="agent"></param>
        /// <param name="couponStatus"></param>
        /// <returns> List<CouponStatusDTO></CouponStatusDTO></returns>
        [OperationContract]
        List<CouponStatusDTO> GetCouponStatusDetail(DateTime DateTo, DateTime DateFrom, Prefix prefix, Agent agent, CouponStatus couponStatus);


        /// <summary>
        /// GetAutomaticDebitsDetail: Obtiene Reporte de Estado de Debitos Automaticos
        /// </summary>
        /// <param name="DateTo"></param>
        /// <param name="DateFrom"></param>
        /// <returns> List<AutomaticDebitsDTO></AutomaticDebitsDTO></returns>
        [OperationContract]
        List<AutomaticDebitsDTO> GetAutomaticDebitsDetail(DateTime DateTo, DateTime DateFrom);


        /// <summary>
        /// GetAutomaticDebitStatus: Obtiene los estados del Debito Automatico
        /// </summary>
        /// <returns>List<Status></returns>
        [OperationContract]
        List<AutomaticDebitStatus> GetAutomaticDebitStatus();

        #endregion

        #region BankNetworkStatus

        /// <summary>
        /// BankNetworkStatus : Obtiene los estados defecto de las  Redes
        /// </summary>
        /// <returns>List<BankNetwork></returns>
        [OperationContract]
        List<BankNetworkStatus> BankNetworkStatus();


        /// <summary>
        /// GetBankNetworkStatus: Obtiene una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        [OperationContract]
        BankNetworkStatus GetBankNetworkStatus(BankNetworkStatus bankNetworkStatus);

        /// <summary>
        /// SaveBankNetworkStatus: Graba una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        [OperationContract]
        BankNetworkStatus SaveBankNetworkStatus(BankNetworkStatus bankNetworkStatus);

        /// <summary>
        /// UpdateBankNetworkStatus: Actualiza una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        [OperationContract]
        BankNetworkStatus UpdateBankNetworkStatus(BankNetworkStatus bankNetworkStatus);


        /// <summary>
        /// DeleteBankNetworkStatus: Elimina una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        [OperationContract]
        bool DeleteBankNetworkStatus(BankNetworkStatus bankNetworkStatus);

        #endregion

        #region AutomaticDebitFormat

        /// <summary>
        /// SaveAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>AutomaticDebitFormat</returns>
        [OperationContract]
        AutomaticDebitFormat SaveAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat);

        /// <summary>
        /// UpdateAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>AutomaticDebitFormat</returns>
        [OperationContract]
        AutomaticDebitFormat UpdateAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat);

        /// <summary>
        /// DeleteAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat);


        /// <summary>
        /// GetFormatsbyBankNetworkId
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AutomaticDebitFormat> GetFormatsbyBankNetworkId(int bankNetworkId);
        #endregion
    }
}
