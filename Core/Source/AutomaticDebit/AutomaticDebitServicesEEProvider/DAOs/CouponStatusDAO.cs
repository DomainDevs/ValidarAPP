//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class CouponStatusDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns></returns>
        public CouponStatus SaveCouponStatus(CouponStatus couponStatus)
        {
            try
            {
                if (couponStatus.Id == 0)
                {
                    List<CouponStatus> rejections = GetCouponStatus();

                    if (rejections.Count > 0)
                    {
                        rejections = rejections
                                  .GroupBy(p => p.Id)
                                  .Select(g => g.Last())
                                  .ToList();

                        //Rellena espacios vacios
                        for (int i = 1; i <= rejections.Count; i++)
                        {
                            if (rejections[i - 1].Id != i)
                            {
                                couponStatus.Id = i;
                            }
                        }
                        if (couponStatus.Id == 0)
                        {
                            couponStatus.Id = rejections[rejections.Count - 1].Id + 1;
                        }
                    }
                    else //Ingresa nuevo registro en tabla vacía
                    {
                        couponStatus.Id = 1;
                    }
                }

                // Convertir de model a entity
                Entities.CouponStatus couponStatusEntity = EntityAssembler.CreateCouponStatus(couponStatus);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(couponStatusEntity);

                return ModelAssembler.CreateCouponStatus(couponStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns></returns>
        public CouponStatus UpdateCouponStatus(CouponStatus couponStatus)
        {
            try
            {
                PrimaryKey primaryKey = Entities.CouponStatus.CreatePrimaryKey(couponStatus.Id, couponStatus.SmallDescription);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.CouponStatus couponSatatusEntity = (Entities.CouponStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                couponSatatusEntity.Rejection = couponStatus.CouponStatusType == CouponStatusTypes.Rejected;
                couponSatatusEntity.NumberOfRetries = couponStatus.RetriesNumber;
                couponSatatusEntity.Applied = couponStatus.CouponStatusType == CouponStatusTypes.Applied;
                couponSatatusEntity.Enabled = couponStatus.IsEnabled;
                couponSatatusEntity.Description = couponStatus.Description;
                couponSatatusEntity.Retry = Convert.ToBoolean(couponStatus.RetriesNumber == 0 ? 0 : 1);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(couponSatatusEntity);

                return ModelAssembler.CreateCouponStatus(couponSatatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns></returns>
        public bool DeleteCouponStatus(CouponStatus couponStatus)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.CouponStatus.CreatePrimaryKey(couponStatus.Id, couponStatus.SmallDescription);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.CouponStatus couponStatusEntity = (Entities.CouponStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(couponStatusEntity);

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

        #endregion

        #region Get

        /// <summary>
        /// GetCouponStatusByGroup
        /// </summary>
        /// <param name="groupDescription"></param>
        /// <returns></returns>
        public List<CouponStatus> GetCouponStatusByGroup(string groupDescription)
        {
            List<CouponStatus> couponsStatus = new List<CouponStatus>();

            string operationType = (groupDescription == "T") ? "T" : "A";
            int tableId = (groupDescription == "T") ? 0 : Convert.ToInt32(groupDescription);

            if (operationType.Equals("A"))
            {
                List<CouponStatus> rejectionsForGrid = GetCouponStatusById(tableId);
                couponsStatus = (from CouponStatus element in rejectionsForGrid
                                 select new CouponStatus()
                                 {
                                     CouponStatusType = element.CouponStatusType,
                                     Description = element.Description,
                                     ExternalDescription = "",
                                     GroupDescription = element.GroupDescription,
                                     Id = Convert.ToInt32(element.Id),
                                     IsEnabled = Convert.ToBoolean(element.IsEnabled),
                                     RetriesNumber = Convert.ToInt32(element.RetriesNumber),
                                     SmallDescription = element.SmallDescription
                                 }).ToList();

            }
            else
            {
                List<CouponStatus> rejectionsForSelect = GetCouponStatus();
                // filtro
                couponsStatus = rejectionsForSelect
                   .GroupBy(p => p.Id)
                   .Select(g => g.First())
                   .ToList();

                couponsStatus = (from CouponStatus element in couponsStatus
                                 select new CouponStatus()
                                 {
                                     CouponStatusType = CouponStatusTypes.Applied,
                                     ExternalDescription = "Código - " + element.Id,
                                     Id = Convert.ToInt32(element.Id),
                                 }).ToList();
            }

            return couponsStatus;
        }

        #endregion

        #endregion

        #region Private Methods 

        /// <summary>
        /// GetCouponStatus
        /// Obtiene todos registros de la tabla
        /// </summary>
        /// <returns>List<CouponStatus></returns>
        private List<CouponStatus> GetCouponStatus()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(Entities.CouponStatus.Properties.CouponStatusId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection couponStatusCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(Entities.CouponStatus), criteriaBuilder.GetPredicate()));

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
        /// GetCouponStatusById
        /// Obtiene registro de un mismo CouponStatusId
        /// </summary>
        /// <param name="couponStatusId"></param>
        /// <returns>List<CouponStatus></returns>
        private List<CouponStatus> GetCouponStatusById(int couponStatusId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(Entities.CouponStatus.Properties.CouponStatusId, couponStatusId);
                BusinessCollection couponStatusCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(Entities.CouponStatus), criteriaBuilder.GetPredicate()));

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

        #endregion
    }
}
