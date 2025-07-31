using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

//Sistran Core
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Resources;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingCollectControlServiceEEProvider : IAccountingCollectControlService
    {
        #region Constants

        #endregion Constants

        #region Instance Viarables

        #region Interfaz

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        readonly CollectControlDAO _collectControlDAO = new CollectControlDAO();
        readonly CollectControlPaymentDAO _collectControlPaymentDAO = new CollectControlPaymentDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region CollectControl

        /// <summary>
        /// SaveCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public CollectControlDTO SaveCollectControl(CollectControlDTO collectControl)
        {
            try
            {

                if (collectControl.Branch.Id == 0 || collectControl.UserId == 0)
                {
                    throw new BusinessException(Resources.Resources.BRANCH_USER_VALIDATION_EXCEPTION);
                }

                return _collectControlDAO.SaveCollectControl(collectControl.ToModel()).ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// CloseBillControl
        /// Cambia el estado a cerrado de la Caja
        /// </summary>
        /// <param name="collectControl"></param>
        public void CloseCollectControl(CollectControlDTO collectControl)
        {
            try
            {
                _collectControlDAO.CloseCollectControl(collectControl.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// AllowOpenCollect
        /// Verifica y permite aperturar la Caja
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="accountingDate"></param>
        /// <param name="status"></param>
        /// <returns>bool</returns>
        public bool AllowOpenCollect(int userId, int branchId, DateTime accountingDate, int status)
        {
            try
            {
                ObjectCriteriaBuilder collectControlFilter = new ObjectCriteriaBuilder();
                collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.UserId);
                collectControlFilter.Equal();
                collectControlFilter.Constant(userId);
                collectControlFilter.And();
                collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode);
                collectControlFilter.Equal();
                collectControlFilter.Constant(branchId);
                collectControlFilter.And();
                if (accountingDate > DateTime.MinValue)
                {
                    accountingDate = new DateTime(accountingDate.Year, accountingDate.Month, accountingDate.Day, 23, 59, 59);

                    collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.OpenDate);
                    collectControlFilter.LessEqual();
                    collectControlFilter.Constant(accountingDate);
                    collectControlFilter.And();
                }
                collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.Status);
                collectControlFilter.Equal();
                collectControlFilter.Constant(status);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), collectControlFilter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// NeedCloseCollect
        /// Verifica si requiere cerrar la Caja
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="accountingDatePresent"></param>
        /// <param name="status"></param>
        /// <returns>CollectControl</returns>
        public CollectControlDTO NeedCloseCollect(int userId, int branchId, DateTime accountingDatePresent, int status)
        {
            CollectControl collectControl = new CollectControl();

            try
            {
                ObjectCriteriaBuilder collectControlFilter = new ObjectCriteriaBuilder();
                collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.UserId);
                collectControlFilter.Equal();
                collectControlFilter.Constant(userId);
                collectControlFilter.And();
                collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode);
                collectControlFilter.Equal();
                collectControlFilter.Constant(branchId);
                collectControlFilter.And();
                collectControlFilter.Property(ACCOUNTINGEN.CollectControl.Properties.Status);
                collectControlFilter.Equal();
                collectControlFilter.Constant(status);

                BusinessCollection collectControlCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), collectControlFilter.GetPredicate()));

                if (collectControlCollection.Count > 0)
                {
                    ACCOUNTINGEN.CollectControl collectControlEntity = collectControlCollection.OfType<ACCOUNTINGEN.CollectControl>().First();

                    collectControl.Status = collectControlEntity.AccountingDate < accountingDatePresent ? (int)CollectControlStatus.Open : (int)CollectControlStatus.Close;
                    collectControl.Id = collectControlEntity.CollectControlId;
                    collectControl.AccountingDate = Convert.ToDateTime(collectControlEntity.AccountingDate);
                    collectControl.OpenDate = Convert.ToDateTime(collectControlEntity.OpenDate);
                }

                return collectControl.ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(ConfigurationManager.AppSettings["UnhandledExceptionMsj"]);
            }
        }

        /// <summary>
        /// GetSumaryPayMethod
        /// Obtiene totales por tipo de pago para el cierre de caja
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>List<SumaryPayMethodDTO/></returns>
        public List<SEARCH.SumaryPayMethodDTO> GetSumaryPayMethod(int collectControlId)
        {
            try
            {
                ObjectCriteriaBuilder paymentMethodFilter = new ObjectCriteriaBuilder();

                //Por Id de Collect Control
                paymentMethodFilter.PropertyEquals(ACCOUNTINGEN.PaymentMethodTypeCollect.Properties.CollectControlId, collectControlId);

                UIView data = _dataFacadeManager.GetDataFacade().GetView("PaymentMethodTypeCollectView", paymentMethodFilter.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.SumaryPayMethodDTO> summaryPayMethodDTOs = new List<SEARCH.SumaryPayMethodDTO>();

                foreach (DataRow row in data.Rows)
                {
                    summaryPayMethodDTOs.Add(new SEARCH.SumaryPayMethodDTO()
                    {
                        CollectControlCode = Convert.ToInt32(row["CollectControlId"]),
                        Description = Convert.ToString(row["Description"]),
                        Total = Convert.ToDouble(row["Total"]),
                        PaymentMethodCode = Convert.ToInt32(row["PaymentMethodCode"]),
                    });
                }

                return summaryPayMethodDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Get las open date for an user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="branchId">Branch identifier</param>
        /// <returns>date</returns>
        public DateTime GetLastOpenDateByUserIdBranchId(int userId, int branchId)
        {
            try
            {
                Business.CollectControlBusiness collectControlBusiness = new Business.CollectControlBusiness();
                return collectControlBusiness.GetLastOpenDateByUserIdBranchId(userId, branchId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #region CollectControlPayment

        /// <summary>
        /// SaveCollectControlPayment
        /// </summary>
        /// <param name="collectControl"></param>
        /// <param name="registerNumber"></param>
        /// <returns>CollectControlPayment</returns>
        public CollectControlPaymentDTO SaveCollectControlPayment(CollectControlDTO collectControl, int registerNumber)
        {
            try
            {
                return _collectControlPaymentDAO.SaveCollectControlPayment(collectControl.ToModel(), registerNumber).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPercentageForPayQuota
        /// </summary>
        /// <param name="numberParameter"></param>
        /// <returns>List<PercentagePayQuotaDTO/></returns>
        public List<SEARCH.PercentagePayQuotaDTO> GetPercentageForPayQuota(int numberParameter)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Se busca por el número del parámetro
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PercentagePayQuota.Properties.ParameterId, numberParameter);

                UIView percentage = _dataFacadeManager.GetDataFacade().GetView("PercentagePayQuotaView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.PercentagePayQuotaDTO> percentagePayQuotaDTOs = new List<SEARCH.PercentagePayQuotaDTO>();

                foreach (DataRow row in percentage.Rows)
                {
                    percentagePayQuotaDTOs.Add(new SEARCH.PercentagePayQuotaDTO()
                    {
                        ParameterId = row[0] == DBNull.Value ? 0 : Convert.ToInt32(row[0]),
                        PercentageParameter = row[1] == DBNull.Value ? 0 : Convert.ToDouble(row[1])
                    });
                }
                return percentagePayQuotaDTOs;
            }

            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion CollectControlPayment

        #endregion CollectControl

        #endregion Public Methods

        public CollectControlDTO GetCollectControlByUserId(int UserId)
        {
            try
            {
                CollectControlDAO collectControlDAO = new CollectControlDAO();
                return DTOAssembler.CreateCollectControl(collectControlDAO.GetCollectControlByUserId(UserId));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
