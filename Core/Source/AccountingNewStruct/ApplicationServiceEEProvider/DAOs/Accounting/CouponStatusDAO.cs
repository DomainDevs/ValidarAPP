//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class CouponStatusDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #region CouponStatusDAO

        /// <summary>
        /// SaveCouponStatus
        /// Guarda un nuevo registro en la tabla.
        /// </summary>
        /// <param name="couponSatatus"></param>
        /// <returns>CouponStatus</returns>
        public CouponStatus SaveCouponStatus(CouponStatus couponSatatus)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CouponStatus couponStatusEntity = EntityAssembler.CreateCouponStatus(couponSatatus);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(couponStatusEntity);

                return ModelAssembler.CreateCouponStatus(couponStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCouponStatus
        /// Actualiza un registro de la tabla.
        /// </summary>
        /// <param name="couponSatatus"></param>
        /// <returns>CouponStatus</returns>
        public CouponStatus UpdateCouponStatus(CouponStatus couponSatatus)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.CouponStatus.CreatePrimaryKey(couponSatatus.Id, couponSatatus.SmallDescription);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CouponStatus couponSatatusEntity = (ACCOUNTINGEN.CouponStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                couponSatatusEntity.Rejection = (couponSatatus.CouponStatusType == CouponStatusTypes.Rejected);
                couponSatatusEntity.NumberOfRetries = couponSatatus.RetriesNumber;
                couponSatatusEntity.Applied = (couponSatatus.CouponStatusType == CouponStatusTypes.Applied);
                couponSatatusEntity.Enabled = couponSatatus.IsEnabled;
                couponSatatusEntity.Description = couponSatatus.Description;
                couponSatatusEntity.Retry = Convert.ToBoolean(couponSatatus.RetriesNumber == 0 ? 0 : 1);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(couponSatatusEntity);

                return ModelAssembler.CreateCouponStatus(couponSatatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCouponStatusById
        /// Obtiene registro de un mismo  CouponStatusId
        /// </summary>
        /// <param name="couponStatusId"></param>
        /// <returns>List<CouponStatus/></returns>
        public List<CouponStatus> GetCouponStatusById(int couponStatusId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CouponStatus.Properties.CouponStatusId, couponStatusId);
                BusinessCollection couponStatusCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.CouponStatus), criteriaBuilder.GetPredicate()));

                if (couponStatusCollection.Count > 0)
                {
                    return ModelAssembler.CreateCouponStatus(couponStatusCollection);
                }

                return new List<CouponStatus>();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCouponStatus
        /// Obtiene todos registros de la tabla
        /// </summary>
        /// <returns>List<CouponStatus/></returns>
        public List<CouponStatus> GetCouponStatus()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.CouponStatus.Properties.CouponStatusId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection couponStatusCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.CouponStatus), criteriaBuilder.GetPredicate()));

                if (couponStatusCollection.Count > 0)
                {
                    // Return del model
                    return ModelAssembler.CreateCouponStatus(couponStatusCollection);
                }

                return new List<CouponStatus>();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCouponStatus
        /// </summary>
        /// <param name="couponStatusId"></param>
        /// <param name="couponStatusBankCode"></param>
        public bool DeleteCouponStatus(int couponStatusId, string couponStatusBankCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CouponStatus.CreatePrimaryKey(couponStatusId, couponStatusBankCode);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CouponStatus couponStatusEntity = (ACCOUNTINGEN.CouponStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(couponStatusEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion

    }
}
