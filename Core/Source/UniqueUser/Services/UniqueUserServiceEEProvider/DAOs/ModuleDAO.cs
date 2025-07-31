using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class ModuleDAO
    {
        /// <summary>
        /// Obtener Modulos
        /// </summary>
        /// <returns>Lista de Module</returns>  
        public List<Module> GetModulesByDescription(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (description != "")
            {
                filter.Property(UniqueUser.Entities.Modules.Properties.Description);
                filter.Equal();
                filter.Constant(description);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.Modules), filter.GetPredicate()));
            List<Module> modules = Assemblers.ModelAssembler.CreateModules(businessCollection);
            return modules;
        }

        /// <summary>
        /// Guarda el objeto Module
        /// </summary>
        /// <param name="List<model.Module>">list Module</param>
        /// <returns>bool</returns>
        public List<Module> CreateModules(List<Module> modules)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (Module module in modules)
            {
                PrimaryKey key = UniqueUser.Entities.Modules.CreatePrimaryKey(module.Id);
                UniqueUser.Entities.Modules moduleEntity = new UniqueUser.Entities.Modules(module.Id);
                moduleEntity = (UniqueUser.Entities.Modules)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (moduleEntity != null)
                {                   
                    if (module.Status == Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.StatusItem.Deleted.ToString())
                    {
                        SubModuleDAO submoduleDAO = new SubModuleDAO();
                        int countSubModule = submoduleDAO.GetCountSubModulesByModuleId(moduleEntity.ModuleCode, "");
                        if (countSubModule == 0)
                        {
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(moduleEntity);
                        }
                        else
                        {
                            module.Status = "NotDelete";
                        }
                    }
                    else
                    {
                        if (moduleEntity.Description == module.Description || moduleEntity.Description != module.Description)
                        {
                            if (GetModulesByDescription(module.Description).Count == 0 || GetModulesByDescription(module.Description).Count == 1)
                            {
                                moduleEntity.Description = module.Description;
                                moduleEntity.Enabled = module.IsEnabled;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(moduleEntity);
                                module.Status = "Update";
                            }                            
                        }
                        else
                        {
                            moduleEntity.Description = module.Description;
                            moduleEntity.Enabled = module.IsEnabled;
                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(moduleEntity);
                        }
                    }
                }
                else
                {
                    if (GetModulesByDescription(module.Description).Count == 0)
                    {
                        if (module.Status != Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.StatusItem.Deleted.ToString())
                        {
                            moduleEntity = Assemblers.EntityAssembler.CreateModule(module);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(moduleEntity);
                        }
                    }
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs.CreateModules");
            return modules;
        }

    }
}
