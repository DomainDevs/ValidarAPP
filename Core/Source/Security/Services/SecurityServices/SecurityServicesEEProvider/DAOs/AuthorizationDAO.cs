using Sistran.Core.Application.EEProvider.Constant;
using Sistran.Core.Application.SecurityServices.EEProvider.Assemblers;
using Sistran.Core.Application.SecurityServices.EEProvider.Entities.Views;
using Sistran.Core.Application.SecurityServices.EEProvider.Enums;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.SecurityServices.EEProvider.DAOs
{

    /// <summary>
    /// Dao de Autorizacion Controles
    /// </summary>
    public class AuthorizationDAO
    {
        /// <summary>
        /// Buscar usuario por id
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public UniqueUser.Entities.UniqueUsers FindUserByUserId(int userId)
        {
            // Busco la entidad
            PrimaryKey primaryKey = UniqueUser.Entities.UniqueUsers.CreatePrimaryKey(userId);
            UniqueUser.Entities.UniqueUsers user = (UniqueUser.Entities.UniqueUsers)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            return user;
        }

        /// <summary>
        /// Obtener Listado de Perfiles
        /// </summary>
        /// <param name="Filter">Filtro Predicate</param>
        /// <returns></returns>
        public List<Profile> ListProfile(Predicate Filter)
        {
            BusinessCollection collection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.ProfileUniqueUser), Filter));
            return ModelAssembler.CreateProfileUsers(collection);
        }

        /// <summary>
        /// Lists the operation profile.
        /// </summary>
        /// <param name="Filter">The filter.</param>
        /// <returns></returns>
        public List<Models.OperationProfile> ListOperationProfile(Predicate Filter)
        {
            // Busco objetos
            BusinessCollection collection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.AccessProfiles), Filter));
            return ModelAssembler.CreateOperationProfileUsers(collection);
        }

        /// <summary>
        /// Lista de Accesos
        /// </summary>
        /// <param name="Filter">Filtro</param>
        /// <returns></returns>
        public List<Models.Operation> ListOperations(Predicate Filter)
        {
            BusinessCollection collection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.Accesses), Filter));
            return ModelAssembler.CreateOperationsBussiness(collection);
        }

        /// <summary>
        /// Obbtenr Modulos del Usuario
        /// </summary>
        /// <param name="userName">Nombre Usuario</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">ENTERR_USER_DOESNT_EXIST</exception>
        public IList<Module> GetModulesByUserName(string userName)
        {
            if (userName.Trim() == "")
            {
                throw new BusinessException("ENTERR_USER_DOESNT_EXIST");
            }
            AuthenticationDAO authenticationProvider = new AuthenticationDAO();
            int userId = authenticationProvider.GetUserId(userName);

            UserOperationByProfileSecurityView view = new UserOperationByProfileSecurityView();
            ViewBuilder builder = new ViewBuilder("UserOperationByProfileSecurityView");
            builder.Filter = SetAccessByUserNameByUserId(userName, userId);
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AccessProfile.Count > 0)
            {
                List<Module> modules = new List<Module>();

                foreach (UniqueUser.Entities.Modules entityModule in view.Module)
                {
                    List<Module> submodules = new List<Module>();
                    Module module = new Module();
                    module = ModelAssembler.CreateModuleByModule(entityModule);

                    foreach (UniqueUser.Entities.Submodules entitySubmodule in view.GetSubModulesByModule(entityModule))
                    {
                        Module submodule = new Module();
                        submodule = ModelAssembler.CreateModuleBySubmodule(entitySubmodule);
                        submodule.SubModules = ModelAssembler.CreateModules(view.GetOperationBySubmodule(entitySubmodule), view.AccessObject.Cast<UniqueUser.Entities.AccessObjects>().ToList());

                        submodules.Add(submodule);
                    }

                    module.SubModules = submodules;
                    modules.Add(module);
                }

                return modules;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Aplicar Filtro por perfil
        /// </summary>
        /// <param name="UserId">Id Usuario</param>
        /// <param name="OperationByProfileFilter">The operation by profile filter.</param>
        /// <returns></returns>
        private ObjectCriteriaBuilder SetOperationByProfileCriteria(int UserId, Predicate OperationByProfileFilter)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property("UserId");
            filter.Equal();
            filter.Constant(UserId);

            if (OperationByProfileFilter != null)
            {
                filter.And();
                filter.Predicate(OperationByProfileFilter);
            }

            return filter;
        }

        /// <summary>
        /// Validar si el Modulo es Valido
        /// </summary>
        /// <param name="userAccesses">Vista del usaurio accesos</param>
        /// <exception cref="BusinessException"></exception>
        private void ValidateValidModule(UserOperationByProfileSecurityView userAccesses)
        {
            //if (userAccesses.Module.Count < 1)
            //{
            //    throw new BusinessException(Security.ErrorModuleEmpty);
            //}
        }
        /// <summary>
        /// Para evitar que en la consulta de los accesos asociados a un perfil
        /// se recupere objetos que no cumplan con las reglas de negocio, se 
        /// establece un primer "filtro", para eliminar relaciones innecesarias.      
        /// </summary>        
        /// <returns>
        /// Criterio de selección que permite vincular :
        ///  - AccessUser no expirados.
        ///  - AccessUser asociados al User dado.
        ///  - Access no deshabilitados.
        /// </returns>
        private Predicate SetAccessByUserNameByUserId(string userName, int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (userName != PublicConstants.ADMIN_UU_NICK)
            {
                filter.Property("UserId", "UniqueUser").Equal().Constant(userId);
                filter.And();
                filter.OpenParenthesis();
                filter.Property("ExpirationDate", "AccessProfile");
                filter.IsNull();
                filter.Or();
                filter.Property("ExpirationDate", "AccessProfile");
                filter.Greater();
                filter.Constant(BusinessServices.GetDate());
                filter.CloseParenthesis();
                filter.And().Property("Enabled", "Module").Equal().Constant(1);
                filter.And().Property("Enabled", "Access").Equal().Constant(1);
                filter.And().Property("AccessObjTypeCode", "AccessObject").Equal().Constant(AccessObjectsType.MenuR2);
                return filter.GetPredicate();
            }
            else
            {
                return null;
            }
        }

        #region accessType
        public IList<Module> GetModulesByUserNameAccesType(string userName, int accessObjectsType)
        {
            if (userName.Trim() == "")
            {
                throw new BusinessException("ENTERR_USER_DOESNT_EXIST");
            }
            AuthenticationDAO authenticationProvider = new AuthenticationDAO();
            int userId = authenticationProvider.GetUserId(userName);

            UserOperationByProfileSecurityView view = new UserOperationByProfileSecurityView();
            ViewBuilder builder = new ViewBuilder("UserOperationByProfileSecurityView");
            builder.Filter = SetAccessByUserNameByUserIdAccesType(userName, userId, accessObjectsType);
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AccessProfile.Count > 0)
            {
                List<Module> modules = new List<Module>();

                foreach (UniqueUser.Entities.Modules entityModule in view.Module)
                {
                    List<Module> submodules = new List<Module>();
                    Module module = new Module();
                    module = ModelAssembler.CreateModuleByModule(entityModule);

                    foreach (UniqueUser.Entities.Submodules entitySubmodule in view.GetSubModulesByModule(entityModule))
                    {
                        Module submodule = new Module();
                        submodule = ModelAssembler.CreateModuleBySubmodule(entitySubmodule);
                        submodule.SubModules = ModelAssembler.CreateModules(view.GetOperationBySubmodule(entitySubmodule), view.AccessObject.Cast<UniqueUser.Entities.AccessObjects>().ToList());

                        submodules.Add(submodule);
                    }

                    module.SubModules = submodules;
                    modules.Add(module);
                }

                return modules;
            }
            else
            {
                return null;
            }

        }

        public IList<Module> GetModulesByUserNameSql(string userName)
        {
            if (userName.Trim() == "")
            {
                throw new BusinessException("ENTERR_USER_DOESNT_EXIST");
            }
            AuthenticationDAO authenticationProvider = new AuthenticationDAO();
            int userId = authenticationProvider.GetUserId(userName);

            SelectQuery selectQuery = new SelectQuery();

            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.Modules.Properties.Description, "MO"), "MODULE_DES"));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.Submodules.Properties.Description, "SMO"), "SUB_MODULE"));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.Accesses.Properties.AccessId, "AC"), "ACCESS_ID"));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.Accesses.Properties.ModuleCode, "AC"), "MODULE_CODE"));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.Accesses.Properties.SubmoduleCode, "AC"), "SUB_MODULE_CODE"));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.Accesses.Properties.Url, "AC"), "PATH"));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniqueUser.Entities.AccessObjects.Properties.ObjectName, "ACO"), "ACCESS"));

            Join join = new Join(new ClassNameTable(typeof(UniqueUser.Entities.ProfileUniqueUser), "PUU"),
                new ClassNameTable(typeof(UniqueUser.Entities.AccessProfiles), "ACP"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniqueUser.Entities.ProfileUniqueUser.Properties.ProfileId, "PUU")
                .Equal()
                .Property(UniqueUser.Entities.AccessProfiles.Properties.ProfileId, "ACP")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniqueUser.Entities.Accesses), "AC"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniqueUser.Entities.AccessProfiles.Properties.AccessId, "ACP")
                .Equal().Property(UniqueUser.Entities.Accesses.Properties.AccessId, "AC")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniqueUser.Entities.AccessObjects), "ACO"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniqueUser.Entities.Accesses.Properties.AccessObjectId, "AC")
                .Equal().Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjectId, "ACO")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniqueUser.Entities.Modules), "MO"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniqueUser.Entities.Accesses.Properties.ModuleCode, "AC")
                .Equal().Property(UniqueUser.Entities.Modules.Properties.ModuleCode, "MO")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniqueUser.Entities.Submodules), "SMO"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniqueUser.Entities.Accesses.Properties.SubmoduleCode, "AC")
                .Equal().Property(UniqueUser.Entities.Submodules.Properties.SubmoduleCode, "SMO")
                .And()
                .Property(UniqueUser.Entities.Accesses.Properties.ModuleCode, "AC")
                .Equal().Property(UniqueUser.Entities.Submodules.Properties.ModuleCode, "SMO")
                
                .GetPredicate());

           

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode);
            filter.Equal();
            filter.Constant(AccessObjectsType.MenuR2);
            filter.And();
            filter.Property(UniqueUser.Entities.Accesses.Properties.Enabled,"AC");
            filter.Equal();
            filter.Constant(1);

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (System.Data.IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                List<ModuleAcces> modulesAccess = new List<ModuleAcces>();
                while (reader.Read())
                {
                    modulesAccess.Add(new ModuleAcces
                    {
                        Module = reader["MODULE_DES"].ToString(),
                        SubModule = reader["SUB_MODULE"].ToString(),
                        Id = Convert.ToInt32(reader["ACCESS_ID"]),
                        ModuleId = Convert.ToInt32(reader["MODULE_CODE"]),
                        SubModuleId = Convert.ToInt32(reader["SUB_MODULE_CODE"]),
                        Description = reader["ACCESS"].ToString(),
                        Path = reader["PATH"].ToString(),
                    });
                }
                var grouped = modulesAccess.GroupBy(l => new { l.ModuleId, l.Module })
                        .Select
                        (
                            g =>
                            new Module
                            {
                                Id = g.Key.ModuleId,
                                Description = g.Key.Module,
                                Title = g.Key.Module,
                                SubModules = g.GroupBy(l => new { l.SubModuleId, l.SubModule }).
                                Select
                                (
                                    gsm =>
                                    new Module
                                    {
                                        Id = gsm.Key.SubModuleId,
                                        Description = gsm.Key.SubModule,
                                        Title = gsm.Key.SubModule,
                                        SubModules = gsm.GroupBy(l => new { l.Id, l.Description,l.Path }).
                                        Select
                                        (
                                            ga =>
                                            new Module
                                            {
                                                Id = ga.Key.Id,
                                                Description = ga.Key.Description,
                                                Title = ga.Key.Description,
                                                Path = ga.Key.Path,
                                            }
                                        ).ToList()
                                    }
                                ).ToList()
                            }
                        ).ToList();

                return grouped;


            }

        }

        private Predicate SetAccessByUserNameByUserIdAccesType(string userName, int userId, int accessObjectsType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (userName != PublicConstants.ADMIN_UU_NICK)
            {
                filter.Property("UserId", "UniqueUser").Equal().Constant(userId);
                filter.And();
                filter.OpenParenthesis();
                filter.Property("ExpirationDate", "AccessProfile");
                filter.IsNull();
                filter.Or();
                filter.Property("ExpirationDate", "AccessProfile");
                filter.Greater();
                filter.Constant(BusinessServices.GetDate());
                filter.CloseParenthesis();
                filter.And().Property("Enabled", "Module").Equal().Constant(1);
                filter.And().Property("Enabled", "Access").Equal().Constant(1);
                filter.And().Property("AccessObjTypeCode", "AccessObject").Equal().Constant(accessObjectsType);
                return filter.GetPredicate();
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
