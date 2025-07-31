//Sistran Core
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.TempCommonServices.Provider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.TempCommonServices.Provider.DAOs
{
    public class ModuleDateDao
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns></returns>
        public ModuleDate SaveModuleDate(ModuleDate moduleDate)
        {
            try
            {
                // Convertir de model a entity
                Entities.ModuleDate moduleDateEntity = EntityAssembler.CreateModuleDate(moduleDate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(moduleDateEntity);

                // Return del model
                return ModelAssembler.CreateModuleDate(moduleDateEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message,ex);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns></returns>
        public ModuleDate UpdateModuleDate(ModuleDate moduleDate)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.ModuleDate.CreatePrimaryKey(moduleDate.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.ModuleDate moduleDateEntity = (Entities.ModuleDate)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                moduleDateEntity.CeilingMm = moduleDate.CeilingMm;
                moduleDateEntity.CeilingYyyy = moduleDate.CeilingYyyy;
                moduleDateEntity.Description = moduleDate.Description;
                moduleDateEntity.LastClosingMm = moduleDate.LastClosingMm;
                moduleDateEntity.LastClosingYyyy = moduleDate.LastClosingYyyy;
                moduleDateEntity.OfflineCeilingMm = moduleDate.OfflineCeilingMm;
                moduleDateEntity.OfflineCeilingYyyy = moduleDate.OfflineCeilingYyyy;
                //moduleDateEntity.TypifiedSeatLastNum = moduleDate.TypifiedSeatLastNum;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(moduleDateEntity);

                // Return del model
                return ModelAssembler.CreateModuleDate(moduleDateEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message,ex);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns></returns>
        public bool DeleteModuleDate(ModuleDate moduleDate)
        {
            try
            {
                PrimaryKey primaryKey = Entities.ModuleDate.CreatePrimaryKey(moduleDate.Id);
                Entities.ModuleDate moduleDateEntity = (Entities.ModuleDate)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(moduleDateEntity);

                return true;
            }
            catch (BusinessException exception)
            {
                if (exception.Message == "RELATED_OBJECT")
                {
                    return false;
                }
                throw new BusinessException(exception.Message);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message,ex);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetModuleDates
        /// </summary>
        /// <returns></returns>
        public List<ModuleDate> GetModuleDates()
        {
            try
            {
                //Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.ModuleDate)));

                //return del model
                return ModelAssembler.CreateModuleDates(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message,ex);
            }
        }

        /// <summary>
        /// GetModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns></returns>
        public ModuleDate GetModuleDate(ModuleDate moduleDate)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.ModuleDate.CreatePrimaryKey(moduleDate.Id);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.ModuleDate moduleDateEntity = (Entities.ModuleDate)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateModuleDate(moduleDateEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }


        #endregion

        #endregion

    }
}
