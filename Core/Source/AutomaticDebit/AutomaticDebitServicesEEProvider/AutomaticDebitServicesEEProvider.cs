//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.DTOs;
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider
{
    public class AutomaticDebitServicesEEProvider : IAutomaticDebitService
    {
        #region Instance Variables

        #region Interfaz

        #endregion

        #region DAOs

        private readonly AutomaticDebitDAO _automaticDebitDAO = new AutomaticDebitDAO();
        private readonly AutomaticDebitFormatDAO _automaticDebitFormatDAO = new AutomaticDebitFormatDAO();
        private readonly BankNetworkDAO _bankNetworkDAO = new BankNetworkDAO();
        private readonly BankNetworkStatusDAO _bankNetworkStatusDAO = new BankNetworkStatusDAO();
        private readonly CouponStatusDAO _couponStatusDAO = new CouponStatusDAO();
        private readonly PaymentMethodBankNetworkDAO _paymentMethodBankNetworkDAO = new PaymentMethodBankNetworkDAO();
        private readonly PaymentMethodAccountTypeDAO _paymentMethodAccountTypeDAO = new PaymentMethodAccountTypeDAO();

        #endregion

        #endregion

        #region AutomaticDebitFormat

        /// <summary>
        /// SaveAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>AutomaticDebitFormat</returns>
        public AutomaticDebitFormat SaveAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            return _automaticDebitFormatDAO.SaveAutomaticDebitFormat(automaticDebitFormat);
        }

        /// <summary>
        /// UpdateAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>AutomaticDebitFormat</returns>
        public AutomaticDebitFormat UpdateAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            return _automaticDebitFormatDAO.UpdateAutomaticDebitFormat(automaticDebitFormat);
        }

        /// <summary>
        /// DeleteAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>bool</returns>
        public bool DeleteAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            return _automaticDebitFormatDAO.DeleteAutomaticDebitFormat(automaticDebitFormat);
        }

        /// <summary>
        /// GetFormatsbyBankNetworkId
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns></returns>
        public List<AutomaticDebitFormat> GetFormatsbyBankNetworkId(int bankNetworkId)
        {
            return _automaticDebitFormatDAO.GetFormatsbyBankNetworkId(bankNetworkId);
        }

        #endregion

        #region BankNetwork

        /// <summary>
        /// GetBankNetworks : Obtiene Redes
        /// </summary>
        /// <returns>List<BankNetwork></returns>
        public List<BankNetwork> GetBankNetworks()
        {
            return _bankNetworkDAO.GetBankNetworks();
        }


        /// <summary>
        /// SaveBankNetwork: Graba Red
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public BankNetwork SaveBankNetwork(BankNetwork bankNetwork)
        {
            return _bankNetworkDAO.SaveBankNetwork(bankNetwork);
        }

        /// <summary>
        /// UpdateBankNetwork: Actualiza Red
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public BankNetwork UpdateBankNetwork(BankNetwork bankNetwork)
        {
            return _bankNetworkDAO.UpdateBankNetwork(bankNetwork);
        }

        /// <summary>
        /// DeleteBankNetwork: Elimina una Red
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>bool</returns>
        public bool DeleteBankNetwork(BankNetwork bankNetwork)
        {
            return _bankNetworkDAO.DeleteBankNetwork(bankNetwork);
        }

        #endregion

        #region BankNetworkStatus

        /// <summary>
        /// BankNetworkStatus : Obtiene los estados defecto de las  Redes
        /// </summary>
        /// <returns>List<BankNetwork></returns>
        public List<BankNetworkStatus> BankNetworkStatus()
        {
            return _bankNetworkStatusDAO.GetBankNetworksStatus();
        }


        /// <summary>
        /// GetBankNetworkStatus: Obtiene una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public BankNetworkStatus GetBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            return _bankNetworkStatusDAO.GetBankNetworkStatus(bankNetworkStatus);
        }

        /// <summary>
        /// SaveBankNetworkStatus: Graba una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public BankNetworkStatus SaveBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            return _bankNetworkStatusDAO.SaveBankNetworkStatus(bankNetworkStatus);
        }

        /// <summary>
        /// UpdateBankNetworkStatus: Actualiza una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public BankNetworkStatus UpdateBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            return _bankNetworkStatusDAO.UpdateBankNetworkStatus(bankNetworkStatus);
        }

        /// <summary>
        /// DeleteBankNetworkStatus: Elimina una red con sus estados por defecto
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public bool DeleteBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            return _bankNetworkStatusDAO.DeleteBankNetworkStatus(bankNetworkStatus);
        }

        #endregion

        #region CouponStatus

        /// <summary>
        /// GetCouponStatusByGroup: Obtiene todos los estado de cupones por Grupo
        /// </summary>
        /// <returns></returns>
        public List<CouponStatus> GetCouponStatusByGroup(string GroupDescription)
        {
            return _couponStatusDAO.GetCouponStatusByGroup(GroupDescription);
        }

        /// <summary>
        /// SaveCouponStatus: Graba Estado de Cupones
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        public CouponStatus SaveCouponStatus(CouponStatus couponStatus)
        {
            return _couponStatusDAO.SaveCouponStatus(couponStatus);
        }

        /// <summary>
        /// UpdateCouponStatus: Actualiza el Estado del Cupon
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        public CouponStatus UpdateCouponStatus(CouponStatus couponStatus)
        {
            return _couponStatusDAO.UpdateCouponStatus(couponStatus);
        }

        /// <summary>
        /// DeleteCouponStatus: Elimina un Estado del Cupon
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>bool</returns>
        public bool DeleteCouponStatus(CouponStatus couponStatus)
        {
            return _couponStatusDAO.DeleteCouponStatus(couponStatus);
        }

        #endregion

        #region GeneratedAutomaticDebit

        /// <summary>
        /// SaveAutomaticDebit : Grabar un Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        public AutomaticDebit SaveAutomaticDebit(AutomaticDebit automaticDebit)
        {
            return _automaticDebitDAO.SaveAutomaticDebit(automaticDebit);
        }

        /// <summary>
        /// UpdateAutomaticDebit : Actualiza un Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        public AutomaticDebit UpdateAutomaticDebit(AutomaticDebit automaticDebit)
        {
            return _automaticDebitDAO.UpdateAutomaticDebit(automaticDebit);
        }

        /// <summary>
        /// GetAutomaticDebit : Obtiene el Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        public AutomaticDebit GetAutomaticDebit(AutomaticDebit automaticDebit)
        {
            return _automaticDebitDAO.GetAutomaticDebit(automaticDebit);
        }

        /// <summary>
        /// GetAutomaticDebits : Obtiene todos los Debito Automaticos
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>AutomaticDebit</returns>
        public List<AutomaticDebit> GetAutomaticDebits(AutomaticDebit automaticDebit)
        {
            return _automaticDebitDAO.GetAutomaticDebits(automaticDebit);
        }

        /// <summary>
        /// GetAutomaticDebitSummary : Obtiene Resumen del Debito Automatico
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns> List<SummaryAutomaticDebitDTO></SummaryAutomaticDebitDTO></returns>
        public List<AutomaticDebitSummaryDTO> GetAutomaticDebitSummary(AutomaticDebit automaticDebit)
        {
            return _automaticDebitDAO.GetAutomaticDebitSummary(automaticDebit);
        }

        /// <summary>
        /// GetCouponStatusDetail: Obtiene Reorte de Estado de Cupones
        /// </summary>
        /// <param name="DateTo"></param>
        /// <param name="DateFrom"></param>
        /// <param name="prefix"></param>
        /// <param name="agent"></param>
        /// <param name="couponStatus"></param>
        /// <returns> List<CouponStatusDTO></CouponStatusDTO></returns>
        public List<CouponStatusDTO> GetCouponStatusDetail(DateTime DateTo, DateTime DateFrom, Prefix prefix, Agent agent, CouponStatus couponStatus)
        {
            return _automaticDebitDAO.GetCouponStatusDetail(DateTo, DateFrom, prefix, agent, couponStatus);
        }

        /// <summary>
        /// GetAutomaticDebitsDetail: Obtiene Reporte de Estado de Debitos Automaticos
        /// </summary>
        /// <param name="DateTo"></param>
        /// <param name="DateFrom"></param>
        /// <returns> List<AutomaticDebitsDTO></AutomaticDebitsDTO></returns>
        public List<AutomaticDebitsDTO> GetAutomaticDebitsDetail(DateTime DateTo, DateTime DateFrom)
        {
            return _automaticDebitDAO.GetAutomaticDebitsDetail(DateTo, DateFrom);
        }

        /// <summary>
        /// GetAutomaticDebitStatus: Obtiene los estados del Debito Automatico
        /// </summary>
        /// <returns>List<Status></returns>
        public List<AutomaticDebitStatus> GetAutomaticDebitStatus()
        {
            return _automaticDebitDAO.GetAutomaticDebitStatus();
        }

        #endregion

        #region PaymentMethodAccountType

        /// <summary>
        /// GetPaymentMethodAccountTypes: Obtiene todos los Metodos de Pago-Tipo de Cuenta
        /// </summary>
        /// <returns>List<PaymentMethodAccountType></returns>
        public List<PaymentMethodAccountType> GetPaymentMethodAccountTypes()
        {
            return _paymentMethodAccountTypeDAO.GetPaymentMethodAccountTypes();
        }

        /// <summary>
        /// SavePaymentMethodAccountType: Graba un Metodo de Pago-Tipo de Cuenta
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public PaymentMethodAccountType SavePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            return _paymentMethodAccountTypeDAO.SavePaymentMethodAccountType(paymentMethodAccountType);
        }

        /// <summary>
        /// UpdatePaymentMethodAccountType: Actualiza un Metodo de Pago-Tipo de Cuenta
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public PaymentMethodAccountType UpdatePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            return _paymentMethodAccountTypeDAO.UpdatePaymentMethodAccountType(paymentMethodAccountType);
        }

        /// <summary>
        /// DeletePaymentMethodAccountType: Elimina un Metodo de Pago-Tipo de Cuenta
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>bool</returns>
        public bool DeletePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            return _paymentMethodAccountTypeDAO.DeletePaymentMethodAccountType(paymentMethodAccountType);
        }

        #endregion
        
        #region PaymentMethodBankNetwork

        /// <summary>
        /// GetPaymentMethodBankNetwork: Obtiene todos los Metodos de Pago - Red 
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>List<PaymentMethodBankNetwork></returns>
        public List<PaymentMethodBankNetwork> GetPaymentMethodBankNetworks(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return _paymentMethodBankNetworkDAO.GetPaymentMethodBankNetworks(paymentMethodBankNetwork);
        }

        /// <summary>
        /// SavePaymentMethodBankNetwork: Graba Metodo de Pago - Red
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public PaymentMethodBankNetwork SavePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return _paymentMethodBankNetworkDAO.SavePaymentMethodBankNetwork(paymentMethodBankNetwork);
        }

        /// <summary>
        /// UpdatePaymentMethodBankNetwork: Actualiza Metodo de Pago - Red
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public PaymentMethodBankNetwork UpdatePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return _paymentMethodBankNetworkDAO.UpdatePaymentMethodBankNetwork(paymentMethodBankNetwork);
        }

        /// <summary>
        /// DeletePaymentMethodBankNetwork: Borra Metod de Pago - red
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns></returns>
        public bool DeletePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return _paymentMethodBankNetworkDAO.DeletePaymentMethodBankNetwork(paymentMethodBankNetwork);
        }

        #endregion

    }
}
