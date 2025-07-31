//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class BankNetworkStatusDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns></returns>
        public BankNetworkStatus SaveBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                // Convertir de model a entity
                Entities.BankNetworkStatus bankNetworkStatusEntity = EntityAssembler.CreateBankNetworkStatus(bankNetworkStatus);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankNetworkStatusEntity);

                return ModelAssembler.CreateBankNetworkStatus(bankNetworkStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns></returns>
        public BankNetworkStatus UpdateBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                PrimaryKey primaryKey = Entities.BankNetworkStatus.CreatePrimaryKey(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.BankNetworkStatus bankNetworkStatusEntity = (Entities.BankNetworkStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankNetworkStatusEntity.AppliedDefault = bankNetworkStatus.AcceptedCouponStatus[0].SmallDescription;
                bankNetworkStatusEntity.RejectionDefault = bankNetworkStatus.RejectedCouponStatus[0].SmallDescription;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankNetworkStatusEntity);

                return ModelAssembler.CreateBankNetworkStatus(bankNetworkStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns></returns>
        public bool DeleteBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankNetworkStatus.CreatePrimaryKey(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.BankNetworkStatus rejectionBankEntity = (Entities.BankNetworkStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(rejectionBankEntity);

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
        /// GetBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns></returns>
        public BankNetworkStatus GetBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankNetworkStatus.CreatePrimaryKey(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.BankNetworkStatus bankNetworkStatusEntity = (Entities.BankNetworkStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateBankNetworkStatus(bankNetworkStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankNetworksStatus
        /// </summary>
        /// <returns></returns>
        public List<BankNetworkStatus> GetBankNetworksStatus()
        {
            int rows;
            List<BankNetworkStatus> bankNetworksStatus = new List<BankNetworkStatus>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(Entities.BankNetworkStatus.Properties.CouponStatusCode);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(0);

            UIView bankNetworkStatusView = _dataFacadeManager.GetDataFacade().GetView("BankResponseCodesView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

            //Add New row for return total records
            if (bankNetworkStatusView.Rows.Count > 0)
            {
                bankNetworkStatusView.Columns.Add("rows", typeof(int));
                bankNetworkStatusView.Rows[0]["rows"] = bankNetworkStatusView.Rows.Count;
            }

            foreach (DataRow dataRow in bankNetworkStatusView)
            {
                List<CouponStatus> acceptedCuponStatus = new List<CouponStatus>();
                List<CouponStatus> rejectedCouponStatus = new List<CouponStatus>();

                acceptedCuponStatus.Add(new CouponStatus()
                {
                    SmallDescription = dataRow["AppliedDefault"].ToString(),
                    Description = dataRow["AppliedDefaultDescription"].ToString(),
                    CouponStatusType = CouponStatusTypes.Applied
                });
                rejectedCouponStatus.Add(new CouponStatus()
                {
                    SmallDescription = dataRow["RejectionDefault"].ToString(),
                    Description = dataRow["RejectionDefaultDescription"].ToString(),
                    CouponStatusType = CouponStatusTypes.Rejected
                });

                bankNetworksStatus.Add(new BankNetworkStatus()
                {
                    AcceptedCouponStatus = acceptedCuponStatus,
                    BankNetwork = new BankNetwork() { Id = Convert.ToInt32(dataRow["BankNetworkCode"]), Description = dataRow["BankNetworkDescription"].ToString() },
                    Id = Convert.ToInt32(dataRow["CouponStatusCode"]), // cod_tabla_rechazo
                    RejectedCouponStatus = rejectedCouponStatus
                });

            }
            return bankNetworksStatus;
        }

        #endregion

        #endregion
    }
}
