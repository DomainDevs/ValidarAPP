//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FKW
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class CreditCardTypeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// SaveCreditCardType
        /// </summary>
        /// <param name="creditCardType"></param>
        /// <returns></returns>
        public CreditCardType SaveCreditCardType(CreditCardType creditCardType)
        {
            try
            {
                // Convertir de model a entity
                COMMEN.CreditCardType creditCardTypeEntity = EntityAssembler.CreateCreditCardType(creditCardType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(creditCardTypeEntity);

                // Return del model
                return ModelAssembler.CreateCreditCardType(creditCardTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCreditCardType
        /// </summary>
        /// <param name="creditCardType"></param>
        /// <returns>BankNetwork</returns>
        public CreditCardType UpdateCreditCardType(CreditCardType creditCardType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetwork.CreatePrimaryKey(creditCardType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                COMMEN.CreditCardType creditCardTypeEntity = (COMMEN.CreditCardType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                creditCardTypeEntity.Commission = creditCardType.Commission;
                creditCardTypeEntity.Description = creditCardType.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(creditCardTypeEntity);

                // Return del model
                return ModelAssembler.CreateCreditCardType(creditCardTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCreditCardType
        /// </summary>
        /// <param name="creditCardType"></param>
        /// <returns></returns>
        public bool DeleteCreditCardType(CreditCardType creditCardType)
        {
            try
            {
                PrimaryKey primaryKey = COMMEN.CreditCardType.CreatePrimaryKey(creditCardType.Id);
                COMMEN.CreditCardType creditCardTypeEntity = (COMMEN.CreditCardType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(creditCardTypeEntity);

                return true;
            }
            catch (BusinessException exception)
            {
                if (exception.Message == "RELATED_OBJECT")
                {
                    return false;
                }
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetCreditCardType
        /// Obtiene un rejistro de la tabla usando su primary Key.
        /// </summary>
        /// <param name="creditCardType"></param>
        /// <returns>CreditCardType</returns>
        public CreditCardType GetCreditCardType(CreditCardType creditCardType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = COMMEN.CreditCardType.CreatePrimaryKey(creditCardType.Id);

                // Realizar las operaciones con los entities utilizando DAF
                COMMEN.CreditCardType creditCardTypeEntity = (COMMEN.CreditCardType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateCreditCardType(creditCardTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
