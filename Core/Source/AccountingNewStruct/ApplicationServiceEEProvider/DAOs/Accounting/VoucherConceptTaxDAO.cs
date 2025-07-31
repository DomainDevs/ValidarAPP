using System;
using System.Collections.Generic;

//Sitran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class VoucherConceptTaxDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveVoucherConceptTax
        /// </summary>
        /// <param name="voucherConceptTax"></param>
        /// <param name="voucherConceptId"></param>
        /// <returns></returns>
        public VoucherConceptTax SaveVoucherConceptTax(VoucherConceptTax voucherConceptTax, int voucherConceptId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.VoucherConceptTax voucherConceptTaxEntity = EntityAssembler.CreateVoucherConceptTax(voucherConceptTax, voucherConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(voucherConceptTaxEntity);

                // Return del model
                return ModelAssembler.CreateVoucherConceptTax(voucherConceptTaxEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateVoucherConceptTax
        /// </summary>
        /// <param name="voucherConceptTax"></param>
        /// <param name="voucherConceptId"></param>
        /// <returns></returns>
        public VoucherConceptTax UpdateVoucherConceptTax(VoucherConceptTax voucherConceptTax, int voucherConceptId)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.VoucherConceptTax.CreatePrimaryKey(voucherConceptTax.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.VoucherConceptTax voucherConceptTaxEntity = (ACCOUNTINGEN.VoucherConceptTax)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                voucherConceptTaxEntity.VoucherConceptCode = voucherConceptId;
                voucherConceptTaxEntity.TaxCode = voucherConceptTax.Tax.Id;
                voucherConceptTaxEntity.TaxCategoryCode = voucherConceptTax.TaxCategory.Id;
                voucherConceptTaxEntity.TaxConditionCode = voucherConceptTax.TaxCondition.Id;
                voucherConceptTaxEntity.TaxRate = voucherConceptTax.TaxeRate;
                voucherConceptTaxEntity.TaxBaseAmount = voucherConceptTax.TaxeBaseAmount;
                voucherConceptTaxEntity.TaxValue = voucherConceptTax.TaxValue;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(voucherConceptTaxEntity);

                // Return del model
                return ModelAssembler.CreateVoucherConceptTax(voucherConceptTaxEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteVoucherConceptTax
        /// </summary>
        /// <param name="voucherConceptTax"></param>
        public void DeleteVoucherConceptTax(VoucherConceptTax voucherConceptTax)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.VoucherConceptTax.CreatePrimaryKey(voucherConceptTax.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.VoucherConceptTax voucherConceptTaxEntity = (ACCOUNTINGEN.VoucherConceptTax)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(voucherConceptTaxEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <param name="voucherConceptTax"></param>
        /// <returns></returns>
        public VoucherConceptTax GetVoucherConceptTax(VoucherConceptTax voucherConceptTax)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.VoucherConceptTax.CreatePrimaryKey(voucherConceptTax.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.VoucherConceptTax voucherConceptTaxEntity = (ACCOUNTINGEN.VoucherConceptTax)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateVoucherConceptTax(voucherConceptTaxEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetVoucherConceptTaxes
        /// </summary>
        /// <returns></returns>
        public List<VoucherConceptTax> GetVoucherConceptTaxes()
        {
            try
            {
                //Se asigna una BussinesCollection a una lista.
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.VoucherConceptTax)));

                //Se retorna como una lista.
                return ModelAssembler.CreateVoucherConceptTaxes(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Get
    }
}
