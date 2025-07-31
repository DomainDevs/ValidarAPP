// Sistran Core
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.ReportingServices.Provider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.ReportingServices.Provider.DAOs
{
    public class FormatModuleDAO
    {
        #region Instance Variables

        /// <summary>
        /// Declaración del contexto y del DataFacadeManager
        /// </summary>
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance Variables

        #region Public Methods
        
        #region FormatModule

        /// <summary>
        /// SaveFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public FormatModule SaveFormatModule(FormatModule formatModule)
        {
            try
            {
                Entities.FormatModule formatModuleEntity = EntityAssembler.CreateFormatModule(formatModule);
                _dataFacadeManager.GetDataFacade().InsertObject(formatModuleEntity);

                formatModule.Id = formatModuleEntity.FormatModuleCode;
                return formatModule;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public FormatModule UpdateFormatModule(FormatModule formatModule)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.FormatModule.CreatePrimaryKey(formatModule.Id);

                Entities.FormatModule formatModuleEntity = (Entities.FormatModule)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                formatModuleEntity.Description = formatModule.Description;

                _dataFacadeManager.GetDataFacade().UpdateObject(formatModuleEntity);

                return formatModule;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>FormatModule</returns>
        public FormatModule DeleteFormatModule(FormatModule formatModule)
        {
            try
            {
                PrimaryKey primaryKey = Entities.FormatModule.CreatePrimaryKey(formatModule.Id);
                Entities.FormatModule formatModuleEntity = (Entities.FormatModule)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (formatModuleEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(formatModuleEntity);
                }
                return formatModule;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetFormatModules
        /// </summary>
        /// <returns>List<FormatModule></returns>
        public List<FormatModule> GetFormatModules()
        {
            try
            {
                ObjectCriteriaBuilder formatModuleCriteriaBuilder = new ObjectCriteriaBuilder();
                formatModuleCriteriaBuilder.Property(Entities.FormatModule.Properties.FormatModuleCode);
                formatModuleCriteriaBuilder.GreaterEqual();
                formatModuleCriteriaBuilder.Constant(0);

                BusinessCollection formatModuleCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.FormatModule),
                    formatModuleCriteriaBuilder.GetPredicate()));

                if (formatModuleCollection.Count > 0)
                {
                    return ModelAssembler.CreateFormatModules(formatModuleCollection);
                }
                else
                {
                    return new List<FormatModule>();
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion Public Methods


    }
}
