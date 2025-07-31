using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = Sistran.Core.Application.EventsServices.Models;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class ModuleDAO
    {
        /// <summary>
        ///  modulo por id del modulo  
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <returns></returns>
        public Model.Module GetModuleByIdModule(int IdModule)
        {
            try
            {
                PrimaryKey key = Modules.CreatePrimaryKey(IdModule);
                return ModelAssembler.CreateModule(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as Modules);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetModuleByIdModule", ex);
            }
        }

        /// <summary>
        /// Lista de modulos
        /// </summary>
        /// <returns></returns>
        public List<Model.Module> GetModules()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.Modules)));
                return ModelAssembler.CreateListModule(businessCollection).OrderBy(x => x.Description).ToList();

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetModules", ex);
            }
        }
    }
}
