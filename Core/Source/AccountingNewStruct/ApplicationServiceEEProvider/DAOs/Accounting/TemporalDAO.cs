namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    /// <summary>
    /// Temporales 
    /// </summary>
    public class TemporalDAO
    {

        /// <summary>
        /// Deletes the temporals.
        /// </summary>
        /// <param name="tempApplicationId">The temporary application identifier.</param>
        public static void DeleteTemporals(int tempApplicationId)
        {
            TempBrokerCheckingAccountTransactionItemDAO _tempBrokerCheckingAccountTransactionItemDAO = new TempBrokerCheckingAccountTransactionItemDAO();
            TempCoinsuranceCheckingAccountTransactionItemDAO _tempCoinsuranceCheckingAccountTransactionItemDAO = new TempCoinsuranceCheckingAccountTransactionItemDAO();
            TempReinsuranceCheckingAccountTransactionItemDAO _tempReinsuranceCheckingAccountTransactionItemDAO = new TempReinsuranceCheckingAccountTransactionItemDAO();
            TempApplicationAccountingItemDAO tempApplicationAccountingItemDAO = new TempApplicationAccountingItemDAO();
            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
            TempClaimPaymentRequestDAO _tempClaimPaymentRequestDAO = new TempClaimPaymentRequestDAO();
            TempApplicationDAO tempApplicationDAO = new TempApplicationDAO();
            // Cta. Cte. Agentes
            _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempApplicationId);
            _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempApplicationId);

            // Cta. Cte. Coaseguros
            _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempApplicationId);
            _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempApplicationId);

            // Cta. Cte. Reaseguros
            _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempApplicationId);
            _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempApplicationId);

            // Contabilidad
            tempApplicationAccountingItemDAO.DeleteTempApplicationAccountingsByTempApplicationId(tempApplicationId);

            // Borra temporales solicitudes de pagos varios
            tempApplicationPremiumDAO.DeleteTempApplicationPremiumByTempApplication(tempApplicationId);

            // Borra temporales solicitudes de pago de siniestros
            _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempApplicationId);

            // Borra temporales Reversion de Primas
            TempApplicationPremiumItemDAO.DeleteTempPremiumReversionTransactionItem(tempApplicationId);
            tempApplicationDAO.DeleteTempApplication(tempApplicationId);
        }
    }
}
