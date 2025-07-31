using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class SubModuleDAO
    {
        /// <summary>
        /// Obtener SubModule
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        public List<SubModule> GetSubModulesByModuleId(int moduleId)
        {
            List<SubModule> subModules = new List<SubModule>();
            SubModuleView subModuleView = new SubModuleView();
            ViewBuilder builder = new ViewBuilder("SubModuleView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (moduleId != 0)
            {
                filter.Property(UniqueUser.Entities.Submodules.Properties.ModuleCode, typeof(UniqueUser.Entities.Submodules).Name);
                filter.Equal();
                filter.Constant(moduleId);
                filter.And();
            }
            filter.Property(UniqueUser.Entities.Modules.Properties.Enabled, typeof(UniqueUser.Entities.Modules).Name);
            filter.Equal();
            filter.Constant(true);

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, subModuleView);

            if (subModuleView.Submodules.Count > 0)
            {
                List<Module> modules = Assemblers.ModelAssembler.CreateModules(subModuleView.Modules);
                subModules = Assemblers.ModelAssembler.CreateSubModules(subModuleView.Submodules, modules);
            }
            return subModules;

        }

        /// <summary>
        /// Obtener SubModule
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        public int GetCountSubModulesByModuleId(int moduleId, string description)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.Submodules.Properties.ModuleCode, typeof(UniqueUser.Entities.Submodules).Name);
            filter.Equal();
            filter.Constant(moduleId);

            if (description != "")
            {
                filter.And();
                filter.Property(UniqueUser.Entities.Submodules.Properties.Description, typeof(UniqueUser.Entities.Submodules).Name);
                filter.Equal();
                filter.Constant(description);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.Submodules), filter.GetPredicate()));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetCountSubModulesByModuleId");

            return businessCollection.Count;

        }
        /// <summary>
        /// Obtener Max SubModule By ModuleId
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        public int GetSubModuleIdByModuleId(int moduleId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int consecutive = 1;
            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Max);
            function.AddParameter(new Column(UniqueUser.Entities.Submodules.Properties.SubmoduleCode, typeof(UniqueUser.Entities.Submodules).Name));
            select.AddSelectValue(new SelectValue(function, "SubmoduleCode"));

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.Submodules.Properties.ModuleCode, typeof(UniqueUser.Entities.Submodules).Name);
            filter.Equal();
            filter.Constant(moduleId);
            select.Where = filter.GetPredicate();
            select.Table = new ClassNameTable(typeof(UniqueUser.Entities.Submodules), typeof(UniqueUser.Entities.Submodules).Name);

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        consecutive = Convert.ToInt32(reader[0].ToString()) + 1;
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetSubModulesByModuleId");

            return consecutive;
        }

        /// <summary>
        /// Guarda el objeto SubModule
        /// </summary>
        /// <param name="List<model.Module>">list Module</param>
        /// <returns>bool</returns>
        public List<SubModule> CreateSubModules(List<SubModule> subModules)
        {
            foreach (SubModule module in subModules)
            {
                PrimaryKey key = UniqueUser.Entities.Submodules.CreatePrimaryKey(module.Module.Id, module.Id);
                UniqueUser.Entities.Submodules moduleEntity = new UniqueUser.Entities.Submodules(module.Module.Id, module.Id);

                moduleEntity = (UniqueUser.Entities.Submodules)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (moduleEntity != null)
                {
                    if (module.Status == Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.StatusItem.Deleted.ToString())
                    {
                        AccessProfileDAO accessProfileDAO = new AccessProfileDAO();
                        List<AccessObject> access = accessProfileDAO.GetAccessObjectByModuleIdSubModuleId(moduleEntity.ModuleCode, moduleEntity.SubmoduleCode, 0, 0, 0);
                        if (access.Count == 0)
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
                        if (moduleEntity.Description != module.Description || moduleEntity.Enabled != module.IsEnabled)
                        {
                            if (GetCountSubModulesByModuleId(module.Module.Id, module.Description) == 0 || GetCountSubModulesByModuleId(module.Module.Id, module.Description) == 1)
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
                    if (GetCountSubModulesByModuleId(module.Module.Id, module.Description) == 0)
                    {
                        if (module.Status != Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.StatusItem.Deleted.ToString())
                        {
                            moduleEntity = Assemblers.EntityAssembler.CreateSubModule(module);
                            moduleEntity.SubmoduleCode = GetSubModuleIdByModuleId(moduleEntity.ModuleCode);
                            moduleEntity.VirtualFolder = "";
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(moduleEntity);
                        }
                    }

                }
            }
            return subModules;
        }
    }
}
