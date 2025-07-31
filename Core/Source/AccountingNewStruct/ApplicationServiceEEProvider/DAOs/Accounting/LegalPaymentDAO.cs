//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class LegalPaymentDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveLegalPayment
        /// </summary>
        /// <param name="legalPaymentId"></param>
        /// <param name="rejectedPaymentId"></param>
        /// <param name="legalDate"></param>
        /// <returns>int</returns>
        /// 
        public int SaveLegalPayment(int legalPaymentId, int rejectedPaymentId, DateTime legalDate)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.LegalPayment legalPaymentEntity = EntityAssembler.CreateLegalPayment(legalPaymentId, rejectedPaymentId, legalDate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(legalPaymentEntity);

                // Return del model
                return legalPaymentEntity.LegalPaymentCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


    }
}
