//Sistran Core
//using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempUsedDepositPremiumDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods
        
        /// <summary>
        /// SaveTempUsedDepositPremium
        /// </summary>
        /// <param name="tempUsedDepositPremiumDto"></param>
        /// <returns>int</returns>
        public int SaveTempUsedDepositPremium(TempUsedDepositPremiumDTO tempUsedDepositPremiumDto)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempUsedDepositPremium tempUsedDepositPremiumEntity = EntityAssembler.CreateTempUsedDepositPremium(tempUsedDepositPremiumDto);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempUsedDepositPremiumEntity);

                return tempUsedDepositPremiumEntity.TempUsedDepositPremiumCode;
            }
            catch (ArgumentException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempUsedDepositPremium
        /// </summary>
        /// <param name="tempUsedDepositPremiumId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempUsedDepositPremium(int tempUsedDepositPremiumId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempUsedDepositPremium.CreatePrimaryKey(tempUsedDepositPremiumId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempUsedDepositPremium tempUsedDepositPremiumEntity = (ACCOUNTINGEN.TempUsedDepositPremium)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempUsedDepositPremiumEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempUsedDepositPremiumByTempPremiumReceivableId
        /// </summary>
        /// <param name="tempPremiumReceivableId"></param>
        /// <returns>List<TempUsedDepositPremiumDTO></returns>
        public List<TempUsedDepositPremiumDTO> GetTempUsedDepositPremiumByTempPremiumReceivableId(int tempPremiumReceivableId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempUsedDepositPremium.Properties.TempPremiumReceivableTransCode, tempPremiumReceivableId);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempUsedDepositPremium), criteriaBuilder.GetPredicate()));

                List<TempUsedDepositPremiumDTO> tempUsedDepositPremiumDTOs = new List<TempUsedDepositPremiumDTO>();
                foreach (ACCOUNTINGEN.TempUsedDepositPremium tempUsedDepositPremiumEntity in businessCollection.OfType<ACCOUNTINGEN.TempUsedDepositPremium>())
                {
                    tempUsedDepositPremiumDTOs.Add(new TempUsedDepositPremiumDTO
                    {
                        Id = tempUsedDepositPremiumEntity.TempUsedDepositPremiumCode,
                        DepositPremiumTransactionId = Convert.ToInt32(tempUsedDepositPremiumEntity.DepositPremiumTransactionCode),
                        TempPremiumReceivableItemId = Convert.ToInt32(tempUsedDepositPremiumEntity.TempPremiumReceivableTransCode),
                        Amount = Convert.ToDecimal(tempUsedDepositPremiumEntity.Amount)
                    });
                }

                return tempUsedDepositPremiumDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        ///// <summary>
        ///// GetTempUsedDepositPremiumByTempPremiumReceivableId
        ///// </summary>
        ///// <param name="tempPremiumReceivableId"></param>
        ///// <returns>List<TempUsedDepositPremiumDTO></returns>
        //public TempUsedDepositPremiumDTO GetUPDByTempPremiumReceivableId(int tempPremiumReceivableId)
        //{
        //    try
        //    {
        //        ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
        //        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempUsedDepositPremium.Properties.TempPremiumReceivableTransCode, tempPremiumReceivableId);

        //        BusinessCollection businessCollection = new BusinessCollection(
        //            _dataFacadeManager.GetDataFacade().SelectObjects(
        //                typeof(ACCOUNTINGEN.TempUsedDepositPremium), criteriaBuilder.GetPredicate()));

        //        List<TempUsedDepositPremiumDTO> tempUsedDepositPremiumDTOs = new List<TempUsedDepositPremiumDTO>();
        //        foreach (ACCOUNTINGEN.TempUsedDepositPremium tempUsedDepositPremiumEntity in businessCollection.OfType<ACCOUNTINGEN.TempUsedDepositPremium>())
        //        {
        //            tempUsedDepositPremiumDTOs.Add(new TempUsedDepositPremiumDTO
        //            {
        //                Id = tempUsedDepositPremiumEntity.TempUsedDepositPremiumCode,
        //                DepositPremiumTransactionId = Convert.ToInt32(tempUsedDepositPremiumEntity.DepositPremiumTransactionCode),
        //                TempPremiumReceivableItemId = Convert.ToInt32(tempUsedDepositPremiumEntity.TempPremiumReceivableTransCode),
        //                Amount = Convert.ToDecimal(tempUsedDepositPremiumEntity.Amount)
        //            });
        //        }

        //        return tempUsedDepositPremiumDTOs;
        //    }
        //    catch (BusinessException ex)
        //    {
        //        throw new BusinessException(ex.Message);
        //    }
        //}

        #endregion
    }
}
