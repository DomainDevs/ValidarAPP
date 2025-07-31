
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingCollectControlService
    {

        #region CollectControlDAO

        /// <summary>
        /// SaveCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        [OperationContract]
        CollectControlDTO SaveCollectControl(CollectControlDTO collectControl);

        /// <summary>
        /// CloseCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns></returns>
        [OperationContract]
        void CloseCollectControl(CollectControlDTO collectControl);

        #endregion

        #region CollectControl

        /// <summary>
        /// AllowOpenCollect
        /// </summary>
        /// <param name="user"></param>
        /// <param name="branchId"></param>
        /// <param name="accountingDate"></param>
        /// <param name="status"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool AllowOpenCollect(int userId, int branchId, DateTime accountingDate, int status);

        /// <summary>
        /// NeedCloseCollect
        /// </summary>
        /// <param name="user"></param>
        /// <param name="branchId"></param>
        /// <param name="accountingDatePresent"></param>
        /// <param name="status"></param>
        /// <returns>CollectControl</returns>
        [OperationContract]
        CollectControlDTO NeedCloseCollect(int userId, int branchId, DateTime accountingDatePresent, int status);

        /// <summary>
        /// GetSumaryPayMethod
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>List<SumaryPayMethodDTO/></returns>
        [OperationContract]
        List<SearchDTO.SumaryPayMethodDTO> GetSumaryPayMethod(int collectControlId);

        /// <summary>
        /// Get las open date for an user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="branchId">Branch identifier</param>
        /// <returns>date</returns>
        [OperationContract]
        DateTime GetLastOpenDateByUserIdBranchId(int userId, int branchId);
        #endregion

        #region CollectControlPayment

        /// <summary>
        /// SaveCollectControlPayment
        /// </summary>
        /// <param name="collectControl"></param>
        /// <param name="registerNumber"></param>
        /// <returns>CollectControlPayment</returns>
        [OperationContract]
        CollectControlPaymentDTO SaveCollectControlPayment(CollectControlDTO collectControl, int registerNumber);





        /// <summary>
        /// GetPercentageForPayQuota
        /// </summary>
        /// <param name="numberParameter"></param>
        /// <returns>List<PercentagePayQuotaDTO/></returns>
        [OperationContract]
        List<SearchDTO.PercentagePayQuotaDTO> GetPercentageForPayQuota(int numberParameter);

        #endregion CollectControlPayment

        [OperationContract]
        CollectControlDTO GetCollectControlByUserId(int UserId);
    }
}
