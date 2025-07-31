#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class PostDatedDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SavePostDated
        /// </summary>
        /// <param name="postDated"></param>
        /// <param name="entryId"></param>
        /// <param name="isDailyEntry"></param>
        /// <returns></returns>
        public PostDated SavePostDated(PostDated postDated, int entryId, bool isDailyEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity = EntityAssembler.CreatePostdatedEntryItem(postDated, entryId, isDailyEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(postdatedEntryItemEntity);

                // Return del model
                return ModelAssembler.CreatePostDated(postdatedEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdatePostDated
        /// </summary>
        /// <param name="postDated"></param>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        public void UpdatePostDated(PostDated postDated, int entryItemId, bool isJournalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.PostdatedEntryItem.CreatePrimaryKey(postDated.PostDatedId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity = (GENERALLEDGEREN.PostdatedEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                postdatedEntryItemEntity.EntryItemId = entryItemId;
                postdatedEntryItemEntity.PostdatedType = Convert.ToInt32(postDated.PostDateType);
                postdatedEntryItemEntity.CurrencyCode = postDated.Amount.Currency.Id;
                postdatedEntryItemEntity.ExchangeRate = postDated.ExchangeRate.SellAmount;
                postdatedEntryItemEntity.DocumentNumber = postDated.DocumentNumber;
                postdatedEntryItemEntity.AmountValue = postDated.Amount.Value;
                postdatedEntryItemEntity.AmountLocalValue = postDated.LocalAmount.Value;
                postdatedEntryItemEntity.IsJournalEntry = isJournalEntry;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(postdatedEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeletePostDated
        /// </summary>
        /// <param name="postDatedId"></param>
        public void DeletePostDated(int postDatedId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.PostdatedEntryItem.CreatePrimaryKey(postDatedId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity = (GENERALLEDGEREN.PostdatedEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(postdatedEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetPostDated
        /// </summary>
        /// <param name="postDatedId"></param>
        /// <returns></returns>
        public PostDated GetPostDated(int postDatedId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.PostdatedEntryItem.CreatePrimaryKey(postDatedId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity = (GENERALLEDGEREN.PostdatedEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreatePostDated(postdatedEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPostDates
        /// </summary>
        /// <returns></returns>
        public List<PostDated> GetPostDates()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.PostdatedEntryItem)));

                // Return como Lista
                return ModelAssembler.CreatePostDates(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
