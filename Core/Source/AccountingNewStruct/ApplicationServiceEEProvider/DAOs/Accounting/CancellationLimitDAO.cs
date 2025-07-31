using System;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class CancellationLimitDAO
    {
        #region Constants

        #endregion

        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimit</returns>
        public CancellationLimit SaveCancellationLimit(CancellationLimit cancellationLimit)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CancellationLimit cancellationLimitEntities = EntityAssembler.CreateCancelationLimit(cancellationLimit);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(cancellationLimitEntities);

                // Return del model
                return ModelAssembler.GetCancelationLimit(cancellationLimitEntities);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimit</returns>
        public CancellationLimit UpdateCancellationLimit(CancellationLimit cancellationLimit)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CancellationLimit.CreatePrimaryKey(cancellationLimit.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CancellationLimit actionCancellationLimit = (ACCOUNTINGEN.CancellationLimit)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionCancellationLimit.CancellationLimitDays = cancellationLimit.CancellationLimitDays;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionCancellationLimit);

                // Return del model
                return ModelAssembler.GetCancelationLimit(actionCancellationLimit);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        public void DeleteCancellationLimit(CancellationLimit cancellationLimit)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.CancellationLimit.CreatePrimaryKey(cancellationLimit.Id);

                ACCOUNTINGEN.CancellationLimit actioncancellationLimit = (ACCOUNTINGEN.CancellationLimit)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(actioncancellationLimit);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCancellationLimits
        /// </summary>
        /// <returns>List<CancellationLimit/></returns>
        public List<CancellationLimit> GetCancellationLimits()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.CancellationLimit.Properties.CancellationLimitId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                UIView cancellationDayPrefixes = _dataFacadeManager.GetDataFacade().
                    GetView("CancellationLimitPrefixView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (cancellationDayPrefixes.Rows.Count > 0)
                {
                    // Return del model
                    return ModelAssembler.CreateCancellationLimits(cancellationDayPrefixes);
                }

                return new List<CancellationLimit>();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
