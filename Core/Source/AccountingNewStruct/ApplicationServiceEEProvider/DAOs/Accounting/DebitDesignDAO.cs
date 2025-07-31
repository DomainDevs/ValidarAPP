//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Debit = Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class DebitDesignDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveDebitDesign
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Debit.Format SaveDebitDesign(Debit.Format format)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.DebitDesign debitDesign = EntityAssembler.CreateDebitDesign(format);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(debitDesign);

                // Return del model
                return ModelAssembler.CreateDebitDesign(debitDesign, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateDebitDesign
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Debit.Format UpdateDebitDesign(Debit.Format format)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.DebitDesign.CreatePrimaryKey(format.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.DebitDesign actionDebitDesign = (ACCOUNTINGEN.DebitDesign)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionDebitDesign.Description = format.Description;
                actionDebitDesign.BankNetworkCode = format.BankNetwork.Id;
                actionDebitDesign.StartDate = format.Date;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionDebitDesign);

                // Return del model
                return ModelAssembler.CreateDebitDesign(actionDebitDesign, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteDebitDesign
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Debit.Format DeleteDebitDesign(Debit.Format format)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.DebitDesign.CreatePrimaryKey(format.Id);
                ACCOUNTINGEN.DebitDesign actionDebitDesign = (ACCOUNTINGEN.DebitDesign)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(actionDebitDesign);

                // Return del model
                return ModelAssembler.CreateDebitDesign(actionDebitDesign, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
