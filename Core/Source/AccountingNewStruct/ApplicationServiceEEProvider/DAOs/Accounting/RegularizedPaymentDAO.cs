//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class RegularizedPaymentDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="billIdFinal"></param>
        /// <returns>int</returns>
        public int SaveRegularizedPayment(int regularizedPaymentId, int paymentIdSource, int billIdFinal)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.RegularizePayment regularizePaymentEntity = EntityAssembler.CreateRegularizedPayment(regularizedPaymentId, paymentIdSource, billIdFinal);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(regularizePaymentEntity);

                // Return del model
                return regularizePaymentEntity.RegularizePaymentCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="billIdFinal"></param>
        /// <returns>int</returns>
        public int UpdateRegularizedPayment(int regularizedPaymentId, int paymentIdSource, int billIdFinal)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.RegularizePayment.CreatePrimaryKey(regularizedPaymentId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.RegularizePayment regularizePaymentEntity = (ACCOUNTINGEN.RegularizePayment)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                regularizePaymentEntity.PaymentCdSource = paymentIdSource;
                regularizePaymentEntity.BillCdFinal = billIdFinal;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(regularizePaymentEntity);

                // Return del model
                return regularizePaymentEntity.RegularizePaymentCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        public void DeleteRegularizedPayment(int regularizedPaymentId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.RegularizePayment.CreatePrimaryKey(regularizedPaymentId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.RegularizePayment regularizePaymentEntity = (ACCOUNTINGEN.RegularizePayment)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(regularizePaymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
