using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = Sistran.Core.Application.EventsServices.Models;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class SubModuleDAO
    {
        /// <summary>
        /// obtiene Model.SubModule por IdModule y IdSubModule
        /// </summary>
        /// <param name="IdModule">id de modulo</param>
        /// <param name="IdSubModule">id de submodulo</param>
        /// <returns></returns>
        public Model.SubModule GetSubModuleByIdModuleIdSubModule(int IdModule, int IdSubModule)
        {
            try
            {
                PrimaryKey key = Submodules.CreatePrimaryKey(IdModule, IdSubModule);
                return ModelAssembler.CreateSubModule(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as Submodules);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetSubModuleByIdModuleIdSubModule", ex);
            }
        }

        /// <summary>
        /// obtiene la lista de Model.SubModule que pertenecen al IdModule
        /// </summary>
        /// <param name="IdModule">id de modulo</param>
        /// <returns></returns>
        public List<Model.SubModule> GetSubModulesByIdModule(int IdModule)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(EVENTEN.Submodules.Properties.ModuleCode, IdModule);

                BusinessCollection businessCollection = (BusinessCollection)DataFacadeManager.Instance.GetDataFacade().List(typeof(EVENTEN.Submodules), filter.GetPredicate());

                return ModelAssembler.CreateListSubModule(businessCollection).OrderBy(x => x.Description).ToList();

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetSubModulesByIdModule", ex);
            }
        }
    }
}
