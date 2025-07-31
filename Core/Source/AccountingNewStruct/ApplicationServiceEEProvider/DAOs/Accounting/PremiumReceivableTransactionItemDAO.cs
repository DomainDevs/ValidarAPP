using System;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class PremiumReceivableTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SavePremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        /// 
        public PremiumReceivableTransactionItem SavePremiumRecievableTransactionItem(PremiumReceivableTransactionItem premiumRecievableTransactionItem, int imputationId, decimal exchangeRate, DateTime registerDate)
        {
            try
            {
                //Afectacion 1: El parametro premiumRecievableEntity recibe el resultado del metodo del ASSEMBLY CreatePremiumReceivable y al eliminar
                //este , el metodo CreatePremiumReceivableTransactionItem del Model assembler no retorna nada
                //la funcion principal es crear una prima por item 

                // Convertir de model a entity

                /* ACCOUNTINGEN.PremiumReceivableTrans premiumRecievableEntity = EntityAssembler.CreatePremiumReceivable(premiumRecievableTransactionItem, imputationId, exchangeRate, registerDate);

                 // Realizar las operaciones con los entities utilizando DAF

                 _dataFacadeManager.GetDataFacade().InsertObject(premiumRecievableEntity);

                 return ModelAssembler.CreatePremiumReceivableTransactionItem(premiumRecievableEntity);*/

                return default(PremiumReceivableTransactionItem);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItemId"></param>
        public void DeletePremiumRecievableTransactionItem(int premiumRecievableTransactionItemId)
        {
            try
            {
                //Afectacion 2: No se crear la llave primaria por falta del metodo , igualmente GetObjectByprimary requier el objeto PremiumReceivableTrans
                // Crea la Primary key con el código de la entidad
                //la funcion principal es eliminar un objeto de entidad de prima


                /*PrimaryKey primaryKey = ACCOUNTINGEN.PremiumReceivableTrans.CreatePrimaryKey(premiumRecievableTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableEntity = (ACCOUNTINGEN.PremiumReceivableTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(premiumReceivableEntity);
                */
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"> </param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public PremiumReceivableTransactionItem GetPremiumRecievableTransactionItem(PremiumReceivableTransactionItem premiumRecievableTransactionItem)
        {
            try
            {
                //Afectacion 3: El objetivo principal es crear prima por transaccion item

                // Crea la Primary key con el código de la entidad
                /*   PrimaryKey primaryKey = ACCOUNTINGEN.PremiumReceivableTrans.CreatePrimaryKey(premiumRecievableTransactionItem.Id);

                   // Realizar las operaciones con los entities utilizando DAF
                   ACCOUNTINGEN.PremiumReceivableTrans premiumRecievableEntity = (ACCOUNTINGEN.PremiumReceivableTrans)
                       (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                   // Return del model
                   return ModelAssembler.CreatePremiumReceivableTransactionItem(premiumRecievableEntity);*/
                return default(PremiumReceivableTransactionItem);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
