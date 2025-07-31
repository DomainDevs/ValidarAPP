using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.Utilities.DataFacade;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class CollectControlDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public CollectControl SaveCollectControl(CollectControl collectControl)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CollectControl collectControlEntity = EntityAssembler.CreateBillControl(collectControl);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(collectControlEntity);

                // Return del model
                return ModelAssembler.CreateCollectControl(collectControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public CollectControl GetCollectControl(CollectControl collectControl)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CollectControl.CreatePrimaryKey(collectControl.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CollectControl collectControlEntity = (ACCOUNTINGEN.CollectControl)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCollectControl(collectControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectControlByCollectControlId
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public CollectControl GetCollectControlByCollectControlId(int collectControlId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CollectControl.CreatePrimaryKey(collectControlId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CollectControl collectControlEntity = (ACCOUNTINGEN.CollectControl)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCollectControl(collectControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public DateTime GetLastOpenDateByUserIdBranchId(int userId, int branchId)
        {
            DateTime openDate = new DateTime();
            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, userId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.Status, 1);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.CollectControl entityCollectControl in businessCollection.OfType<ACCOUNTINGEN.CollectControl>())
                {
                    openDate = Convert.ToDateTime(entityCollectControl.AccountingDate);
                }
                // Return del model
                return openDate;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// CloseCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        public void CloseCollectControl(CollectControl collectControl)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CollectControl.CreatePrimaryKey(collectControl.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CollectControl collectControlEntity = (ACCOUNTINGEN.CollectControl)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                collectControlEntity.Status = collectControl.Status;
                collectControlEntity.CloseDate = collectControl.CloseDate;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(collectControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectControlByCollectControlId
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public DateTime GetAccountingDateForCollectControlByCollectId(int collectId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collectId);

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectControl.Properties.AccountingDate, typeof(ACCOUNTINGEN.CollectControl).Name), ACCOUNTINGEN.CollectControl.Properties.AccountingDate));

                Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.Collect), typeof(ACCOUNTINGEN.Collect).Name), new ClassNameTable(typeof(ACCOUNTINGEN.CollectControl), typeof(ACCOUNTINGEN.CollectControl).Name), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCOUNTINGEN.Collect.Properties.CollectControlCode, typeof(ACCOUNTINGEN.Collect).Name)
                    .Equal()
                    .Property(ACCOUNTINGEN.CollectControl.Properties.CollectControlId, typeof(ACCOUNTINGEN.CollectControl).Name)
                    .GetPredicate());

                selectQuery.Table = join;
                selectQuery.Where = criteriaBuilder.GetPredicate();

                DateTime dateTime = DateTime.MinValue;

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        if (reader[ACCOUNTINGEN.CollectControl.Properties.AccountingDate] != null)
                            dateTime = Convert.ToDateTime(reader[ACCOUNTINGEN.CollectControl.Properties.AccountingDate]);
                    }
                }
                // Return del model
                return dateTime;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectControlByCollectControlId
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public CollectControl GetCollectControlByUserId(int UserId)
        {
            try
            {
                List<CollectControl> collectControls = new List<CollectControl>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, typeof(ACCOUNTINGEN.CollectControl).Name, UserId);
                BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.CollectControl), filter.GetPredicate());
                EntityAssembler.CreateCollectControls(businessObjects).OrderByDescending(x => x.Id);
                return EntityAssembler.CreateCollectControls(businessObjects).Last();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
