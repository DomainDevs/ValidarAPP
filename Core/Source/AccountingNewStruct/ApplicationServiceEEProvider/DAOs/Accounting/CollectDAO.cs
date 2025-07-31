//System
using System;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Enums;
using System.Data;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
  public class CollectDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Collect

        /// <summary>
        /// SaveCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <returns>Collect</returns>
        public Collect SaveCollect(Collect collect, int collectControlId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.Collect collectEntity = EntityAssembler.CreateCollect(collect, collectControlId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(collectEntity);

                // Return del model
                return ModelAssembler.CreateCollect(collectEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <returns>Collect</returns>
        public Collect UpdateCollect(Collect collect, int collectControlId)
        {
            try
            {
                if (collectControlId != -1)
                {
                    // Crea la Primary key con el código de la entidad
                    PrimaryKey primaryKey = ACCOUNTINGEN.Collect.CreatePrimaryKey(collect.Id);

                    // Encuentra el objeto en referencia a la llave primaria
                    ACCOUNTINGEN.Collect collectEntity = (ACCOUNTINGEN.Collect)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    collectEntity.CollectConceptCode = collect.Concept.Id;
                    collectEntity.CollectControlCode = collectControlId;
                    collectEntity.Description = collect.Description;
                    collectEntity.PaymentsTotal = collect.PaymentsTotal.Value;
                    collectEntity.RegisterDate = collect.Date;
                    collectEntity.Status = collect.Status;
                    collectEntity.UserId = collect.UserId;
                    collectEntity.IndividualId = collect.Payer.IndividualId;
                    collectEntity.Number = collect.Number;
                    collectEntity.IsTemporal = collect.IsTemporal;
                    collectEntity.Comments = collect.Comments;
                    collectEntity.AccountingCompanyCode = collect.AccountingCompany.IndividualId;
                    collectEntity.CollectType = Convert.ToInt32(collect.CollectType);

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().UpdateObject(collectEntity);

                    // Return del model
                    return ModelAssembler.CreateCollect(collectEntity);
                }
                else
                {
                    // Crea la Primary key con el código de la entidad
                    PrimaryKey primaryKey = ACCOUNTINGEN.Collect.CreatePrimaryKey(collect.Id);

                    // Encuentra el objeto en referencia a la llave primaria
                    ACCOUNTINGEN.Collect entityCollect = (ACCOUNTINGEN.Collect)
                        (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    if (entityCollect == null)
                    {
                        ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, collect.Id);

                        BusinessCollection collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuilder.GetPredicate()));

                        if (collectCollection != null  && collectCollection.Count > 0)
                        {
                            entityCollect = collectCollection.OfType<ACCOUNTINGEN.Collect>().First();
                        }
                    }

                    if (entityCollect != null)
                    {
                        entityCollect.Status = collect.Status;
                        entityCollect.UserId = collect.UserId;
                        if (collect.Transaction != null && collect.Transaction.TechnicalTransaction != 0)
                        {
                            entityCollect.TechnicalTransaction = collect.Transaction.TechnicalTransaction;
                        }

                        if (collect.Comments != null)
                        {
                            entityCollect.Comments = collect.Comments;
                        }

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(entityCollect);

                        // Return del model
                        return ModelAssembler.CreateCollect(entityCollect);
                    }
                    return new Collect();
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <returns>Collect</returns>
        public Collect GetCollect(Collect collect)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Collect.CreatePrimaryKey(collect.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Collect collectEntity = (ACCOUNTINGEN.Collect)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCollect(collectEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectByBookEntry
        /// <param name="bookEntry"></param>
        /// Obtiene el Id de imputaciòn dado el source y el tipo
        /// </summary>
        /// <returns>int</returns>
        public int GetCollectByBookEntry(int bookEntry)
        {
            try
            {
                int collectId = 0;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, bookEntry);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectType, CollectTypes.DailyAccount);

                BusinessCollection collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuilder.GetPredicate()));

                if (collectCollection.Count > 0)
                {
                    collectId = Convert.ToInt32(collectCollection.OfType<ACCOUNTINGEN.Collect>().First().CollectId);
                }                

                return collectId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        /// <summary>
        /// collect por technicaltransaction
        /// </summary>
        /// <param name="technicalTransactionId"></param>
        /// <returns></returns>
        public Collect GetCollectByTechnicalTransaction(int technicalTransactionId)
        {
            try
            {
                Collect collectModel = new Collect();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, technicalTransactionId);

                BusinessCollection collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuilder.GetPredicate()));

                if (collectCollection.Count > 0)
                {
                    collectModel = ModelAssembler.CreateCollect(collectCollection.OfType<ACCOUNTINGEN.Collect>().First());
                }

                return collectModel;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public Collect GetCollectByCollectId(int collectId)
        {
            try
            {
                Collect collectModel = new Collect();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collectId);

                BusinessCollection collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuilder.GetPredicate()));

                if (collectCollection.Count > 0)
                {
                    collectModel = ModelAssembler.CreateCollect(collectCollection.OfType<ACCOUNTINGEN.Collect>().First());
                }

                return collectModel;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public int GetTechnicalTransactionByPaymentId(int paymentId)
        {
            try
            {
                Collect collectModel = new Collect();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                ObjectCriteriaBuilder criteriaBuildercollect = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentId);
                int technicalTransaction = 0;
                BusinessCollection collectCollection = new BusinessCollection();
                BusinessCollection paymentCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));
                ACCOUNTINGEN.Payment entityPayment = new ACCOUNTINGEN.Payment(0);
                if (paymentCollection.Count > 0)
                {
                    entityPayment = paymentCollection.OfType<ACCOUNTINGEN.Payment>().First();


                    criteriaBuildercollect.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, entityPayment.CollectCode);

                    collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuildercollect.GetPredicate()));
                    if (collectCollection.Count > 0)
                    {
                        technicalTransaction = collectCollection.OfType<ACCOUNTINGEN.Collect>().First().TechnicalTransaction ?? 0;
                    }

                }

                return technicalTransaction;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        

        public bool UpdateCollectStatus(int collectId, int status)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Collect.CreatePrimaryKey(collectId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Collect entityCollect = (ACCOUNTINGEN.Collect)
                    (DataFacadeManager.GetObject(primaryKey));

                if (entityCollect != null)
                {
                    entityCollect.Status = status;
                    return DataFacadeManager.Update(entityCollect);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return false;
        }
        #endregion
    }
}
