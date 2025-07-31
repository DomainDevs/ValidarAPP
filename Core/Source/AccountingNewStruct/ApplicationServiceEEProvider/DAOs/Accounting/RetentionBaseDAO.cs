using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class RetentionBaseDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>CollectConcept</returns>
        public bool SaveRetentionBase(Models.Retentions.RetentionBase retentionBase)
        {
            bool isSaved = false;
            try
            {
                if (retentionBase.Id == 0)
                {
                    List<Models.Retentions.RetentionBase> rejections = GetRetentionBases();

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
                                retentionBase.Id = i;
                            }
                        }
                        if (retentionBase.Id == 0)
                        {
                            retentionBase.Id = rejections[rejections.Count - 1].Id + 1;
                        }
                    }
                    else //Ingresa nuevo registro en tabla vacía
                    {
                        retentionBase.Id = 1;
                    }
                }

                // Convertir de model a entity
                ACCOUNTINGEN.RetentionBase retentionBaseEntity = EntityAssembler.CreateRetentionBase(retentionBase);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(retentionBaseEntity);

                isSaved = true;
            }
            catch (BusinessException)
            {
                isSaved = false;
            }

            return isSaved;
        }

        /// <summary>
        /// UpdateRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>CollectConcept</returns>
        public bool UpdateRetentionBase(Models.Retentions.RetentionBase retentionBase)
        {
            bool isUpdated = false;
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.RetentionBase.CreatePrimaryKey(retentionBase.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.RetentionBase retentionBaseEntity = (ACCOUNTINGEN.RetentionBase)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                retentionBaseEntity.Description = retentionBase.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(retentionBaseEntity);

                isUpdated = true;
            }
            catch (BusinessException)
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// DeleteRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        public bool DeleteRetentionBase(Models.Retentions.RetentionBase retentionBase)
        {
            bool isDeleted = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.RetentionBase.Properties.RetentionBaseCode, retentionBase.Id);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.RetentionBase), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.RetentionBase retentionBaseEntity in businessCollection.OfType<ACCOUNTINGEN.RetentionBase>())
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(retentionBaseEntity);
                }

                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// GetRetentionBases
        /// </summary>
        /// <returns>List<RetentionBase></returns>
        public List<Models.Retentions.RetentionBase> GetRetentionBases()
        {
            try
            {
                // Asignamos BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.RetentionBase)));

                // Return como lista
                return ModelAssembler.CreateRetentionBases(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
