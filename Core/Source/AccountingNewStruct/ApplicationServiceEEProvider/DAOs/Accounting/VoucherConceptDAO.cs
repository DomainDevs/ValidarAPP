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
    public class VoucherConceptDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveVoucherConcept
        /// </summary>
        /// <param name="voucherConcept"></param>
        /// <param name="voucherId"></param>
        /// <returns>VoucherConcept</returns>
        public VoucherConcept SaveVoucherConcept(VoucherConcept voucherConcept, int voucherId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.VoucherConcept voucherConceptEntity = EntityAssembler.CreateVoucherConcept(voucherConcept, voucherId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(voucherConceptEntity);

                // Return del model
                return ModelAssembler.CreateVoucherConcept(voucherConceptEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateVoucherConcept
        /// </summary>
        /// <param name="voucherConcept"></param>
        /// <param name="voucherId"></param>
        /// <returns>VoucherConcept</returns>
        public VoucherConcept UpdateVoucherConcept(VoucherConcept voucherConcept, int voucherId)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.VoucherConcept.CreatePrimaryKey(voucherConcept.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.VoucherConcept voucherConceptEntity = (ACCOUNTINGEN.VoucherConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                voucherConceptEntity.VoucherCode = voucherId;
                voucherConceptEntity.AccountingConceptCode = voucherConcept.AccountingConcept.Id;
                voucherConceptEntity.CostCenterCode = voucherConcept.CostCenter.CostCenterId;
                voucherConceptEntity.Amount = voucherConcept.Amount.Value;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(voucherConceptEntity);

                // Return del model
                return ModelAssembler.CreateVoucherConcept(voucherConceptEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteVoucherConcept
        /// </summary>
        /// <param name="voucherConcept"></param>
        public void DeleteVoucherConcept(VoucherConcept voucherConcept)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.VoucherConcept.CreatePrimaryKey(voucherConcept.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.VoucherConcept voucherConceptEntity = (ACCOUNTINGEN.VoucherConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(voucherConceptEntity);
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
        /// <param name="voucherConcept"></param>
        /// <returns>VoucherConcept</returns>
        public VoucherConcept GetVoucherConcept(VoucherConcept voucherConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.VoucherConcept.CreatePrimaryKey(voucherConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.VoucherConcept voucherConceptEntity = (ACCOUNTINGEN.VoucherConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateVoucherConcept(voucherConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetVoucherConcepts
        /// </summary>
        /// <returns>List<VoucherConcept></returns>
        public List<VoucherConcept> GetVoucherConcepts()
        {
            try
            {
                //Se asigna una BussinesCollection a una lista.
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.VoucherConcept)));

                //Se retorna como una lista.
                return ModelAssembler.CreateVoucherConcepts(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Get
    }
}
