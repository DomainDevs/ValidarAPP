using System;
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;


namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class RangeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        /// <summary>
        /// SaveRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public Range SaveRange(Range range)
        {
            try
            {
                ACCOUNTINGEN.Range rangeEntity = EntityAssembler.CreateRange(range);
                if (range.Id == 0)
                {
                    _dataFacadeManager.GetDataFacade().InsertObject(rangeEntity);
                }

                // Detalle de rangos
                foreach (RangeItem rangeItem in range.RangeItems)
                {
                    ACCOUNTINGEN.RangeItem rangeValueEntity = EntityAssembler.CreateRanges(rangeItem, rangeEntity.RangeCode);
                    _dataFacadeManager.GetDataFacade().InsertObject(rangeValueEntity);
                }

                range.Id = rangeEntity.RangeCode;
                return range;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRanges
        /// </summary>
        /// <returns>List<Range/></returns>
        public List<Range> GetRanges()
        {
            List<Range> ranges = new List<Range>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Range.Properties.RangeCode);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection rangesCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.Range), criteriaBuilder.GetPredicate()));

                if (rangesCollection.Count > 0)
                {
                    // Return del model
                    ranges = ModelAssembler.CreateRanges(rangesCollection);
                }                
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return ranges;
        }

        /// <summary>
        /// GetRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public Range GetRange(Range range)
        {
            Range newRange = new Range();
            List<RangeItem> rangeItems = new List<RangeItem>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.RangeItem.Properties.RangeItemCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(range.Id);

                newRange.Id = range.Id;

                BusinessCollection collections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.RangeItem), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.RangeItem rangeEntity in collections.OfType<ACCOUNTINGEN.RangeItem>())
                {
                    rangeItems.Add(new RangeItem
                    {
                        Id = rangeEntity.RangeItemCode,
                        Order = rangeEntity.RangeOrder,
                        RangeFrom = Convert.ToInt32(rangeEntity.FromValue),
                        RangeTo = Convert.ToInt32(rangeEntity.ToValue)
                    });
                }

                newRange.RangeItems = rangeItems;
                return newRange;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public Range UpdateRange(Range range)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Range.CreatePrimaryKey(range.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Range rangeEntity = (ACCOUNTINGEN.Range)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                rangeEntity.Description = range.Description;
                rangeEntity.RangeDefault = range.IsDefault;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(rangeEntity);

                return range;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteRange
        /// </summary>
        /// <param name="rangeId"></param>
        /// <returns>bool</returns>
        public bool DeleteRange(int rangeId)
        {
            bool isDeleted = false;

            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.Range.CreatePrimaryKey(rangeId);
                ACCOUNTINGEN.Range rangeDeleteEntity = (ACCOUNTINGEN.Range)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (rangeDeleteEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(rangeDeleteEntity);
                }
                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
			
            return isDeleted;
        }

    }
}
