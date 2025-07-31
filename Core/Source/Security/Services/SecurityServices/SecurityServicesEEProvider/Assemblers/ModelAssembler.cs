using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Sistran.Core.Application.SecurityServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region ProfileUser

        /// <summary>
        /// Creates the profile user.
        /// </summary>
        /// <param name="profileUser">The profile user.</param>
        /// <returns></returns>
        public static Profile CreateProfileUser(UniqueUser.Entities.ProfileUniqueUser profileUser)
        {
            Profile modelProfileUser = new Profile();
            modelProfileUser.ProfileId = profileUser.ProfileId;
            modelProfileUser.ExpirationDate = profileUser.ExpirationDate;
            modelProfileUser.IsExpirationDateNull = profileUser.IsExpirationDateNull;
            return modelProfileUser;
        }

        /// <summary>
        /// Creates the profile users.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Profile> CreateProfileUsers(BusinessCollection businessCollection)
        {
            List<Profile> profileUser = new List<Profile>();

            foreach (UniqueUser.Entities.ProfileUniqueUser field in businessCollection)
            {
                profileUser.Add(ModelAssembler.CreateProfileUser(field));
            }

            return profileUser;
        }

        #endregion
        #region OperationProfileUser

        /// <summary>
        /// Creates the operation profile user.
        /// </summary>
        /// <param name="operationProfileUser">The operation profile user.</param>
        /// <returns></returns>
        public static Models.OperationProfile CreateOperationProfileUser(UniqueUser.Entities.AccessProfiles operationProfileUser)
        {
            Models.OperationProfile modelOperationProfileUser = new Models.OperationProfile();
            modelOperationProfileUser.DatabaseId = operationProfileUser.DatabaseId;
            modelOperationProfileUser.Enabled = operationProfileUser.Enabled;
            modelOperationProfileUser.ExpirationDate = operationProfileUser.ExpirationDate;
            modelOperationProfileUser.IsExpirationDateNull = operationProfileUser.IsExpirationDateNull;
            modelOperationProfileUser.ProfileId = operationProfileUser.ProfileId;
            return modelOperationProfileUser;
        }

        /// <summary>
        /// Creates the operation profile users.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.OperationProfile> CreateOperationProfileUsers(BusinessCollection businessCollection)
        {
            List<Models.OperationProfile> operationProfile = new List<Models.OperationProfile>();

            foreach (UniqueUser.Entities.AccessProfiles field in businessCollection)
            {
                operationProfile.Add(ModelAssembler.CreateOperationProfileUser(field));
            }

            return operationProfile;
        }

        #endregion
        #region Access

        /// <summary>
        /// Creates the operation field.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        public static Models.Operation CreateOperationField(UniqueUser.Entities.Accesses operation)
        {
            Models.Operation modelOperation = new Models.Operation();
            modelOperation.OperationId = operation.AccessId;
            modelOperation.OperationObjectId = (int)operation.AccessObjectId;
            modelOperation.Description = operation.Description;
            modelOperation.Enabled = operation.Enabled;
            modelOperation.IsOperationObjectIdNull = operation.IsAccessObjectIdNull;
            modelOperation.IsParentOperationIdNull = operation.IsParentAccessIdNull;
            modelOperation.ModuleId = operation.ModuleCode;
            modelOperation.ParentOperationId = (int)operation.ParentAccessId;
            modelOperation.SubmoduleId = operation.SubmoduleCode;
            modelOperation.Route = operation.Url;

            return modelOperation;
        }

        /// <summary>
        /// Creates the accesses.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Operation> CreateOperationsBussiness(BusinessCollection businessCollection)
        {
            List<Models.Operation> operation = new List<Models.Operation>();

            foreach (UniqueUser.Entities.Accesses field in businessCollection)
            {
                operation.Add(ModelAssembler.CreateOperationField(field));
            }

            return operation;
        }

        #endregion
        //#region Modulos

        ///// <summary>
        ///// Creates the module.
        ///// </summary>
        ///// <param name="module">The module.</param>
        ///// <param name="submodule">The submodule.</param>
        ///// <returns></returns>
        //public static Sistran.Core.Application.SecurityServices.Models.Module CreateModule(Module module, List<Sistran.Core.Application.SecurityServices.Models.Module> submodule)
        //{
        //    Sistran.Core.Application.SecurityServices.Models.Module modelModule = new Sistran.Core.Application.SecurityServices.Models.Module();
        //    modelModule.Description = module.Description;
        //    modelModule.Disabled = module.Enabled;
        //    modelModule.Id = module.ModuleId;
        //    modelModule.Path = "";
        //    modelModule.SubModules = submodule;
        //    modelModule.Title = module.Description;

        //    return modelModule;
        //}

        /// <summary>
        /// Creates the sub module.
        /// </summary>
        /// <param name="submodule">The submodule.</param>
        /// <param name="operations">The operations.</param>
        /// <returns></returns>
        public static Sistran.Core.Application.SecurityServices.Models.Module CreateModuleByModule(UniqueUser.Entities.Modules module)
        {
            return new Sistran.Core.Application.SecurityServices.Models.Module
            {
                Description = module.Description,
                Disabled = module.Enabled,
                Id = module.ModuleCode,
                Title = module.Description
            };
        }

        /// <summary>
        /// Creates the sub module.
        /// </summary>
        /// <param name="submodule">The submodule.</param>
        /// <param name="operations">The operations.</param>
        /// <returns></returns>
        public static Sistran.Core.Application.SecurityServices.Models.Module CreateModuleBySubmodule(UniqueUser.Entities.Submodules submodule)
        {
            return new Sistran.Core.Application.SecurityServices.Models.Module
            {
                Description = submodule.Description,
                Disabled = submodule.Enabled,
                Id = submodule.ModuleCode,
                Title = submodule.Description
            };
        }

        /// <summary>
        /// Creates the operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private static Sistran.Core.Application.SecurityServices.Models.Module CreateModule(UniqueUser.Entities.Accesses operation)
        {
            string value = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LevelOperation"]) ? "" : ConfigurationManager.AppSettings["LevelOperation"];
            Sistran.Core.Application.SecurityServices.Models.Module modelModule = new Sistran.Core.Application.SecurityServices.Models.Module();

            modelModule.Description = operation.Description;
            modelModule.Disabled = operation.Enabled;
            modelModule.Id = operation.AccessId;
            modelModule.Path = value + operation.Url;
            modelModule.SubModules = new List<Sistran.Core.Application.SecurityServices.Models.Module>();
            modelModule.Title = operation.Description;

            return modelModule;
        }

        /// <summary>
        /// Creates the operations.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Sistran.Core.Application.SecurityServices.Models.Module> CreateModules(BusinessCollection operations, List<UniqueUser.Entities.AccessObjects> operationObjects)
        {
            List<Sistran.Core.Application.SecurityServices.Models.Module> modules = new List<Sistran.Core.Application.SecurityServices.Models.Module>();

            foreach (UniqueUser.Entities.Accesses field in operations)
            {
                field.Description = operationObjects.First(x => x.AccessObjectId == field.AccessObjectId).ObjectName;
                modules.Add(CreateModule(field));
            }

            return modules;
        }

        //#endregion
        //#region AccessObject
        //public static List<Models.OperationObject> CreateOperationObjects(BusinessCollection businessCollection)
        //{
        //    List<Models.OperationObject> operationObject = new List<Models.OperationObject>();

        //    foreach (AccessObject item in businessCollection)
        //    {
        //        operationObject.Add(ModelAssembler.CreateOperationObject(item));
        //    }

        //    return operationObject;
        //}

        //public static Models.OperationObject CreateOperationObject(AccessObject OperationObjects)
        //{
        //    return new Models.OperationObject
        //    {
        //        Id = OperationObjects.AccessObjectId,
        //        Description = OperationObjects.Description
        //    };
        //}
        //#endregion
        //#region Usuario
        ///// <summary>
        ///// De acuerdo al BusinessCollection obtiene una lista de objetos del tipo Models.UniqueUsers
        ///// </summary>
        ///// <param name="businessCollection">La colección que contiene la lista de objetos.</param>
        ///// <returns>Retorna una lista de objetos del tipo Models.UniqueUsers</returns>
        //public static List<Models.User> CreateUniqueUsers(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        //{
        //    List<Models.User> uniqueUsers = new List<Models.User>();

        //    foreach (Entities.UniqueUsers fields in businessCollection)
        //    {
        //        uniqueUsers.Add(ModelAssembler.CreateUniqueUser(fields));
        //    }

        //    return uniqueUsers;
        //}
        ///// <summary>
        ///// Creates the unique user.
        ///// </summary>
        ///// <param name="uniqueUsers">The unique users.</param>
        ///// <returns></returns>
        //public static Models.User CreateUniqueUser(EtUser.UniqueUser uniqueUsers)
        //{
        //    return new Models.User()
        //    {
        //        Id = uniqueUsers.UserId,
        //        Nick = uniqueUsers.AccountName,
        //        PersonId = uniqueUsers.PersonId
        //    };
        //}
        //#endregion

        #region Usuario
        /// <summary>
        /// De acuerdo al BusinessCollection obtiene una lista de objetos del tipo Models.UniqueUsers
        /// </summary>
        /// <param name="businessCollection">La colección que contiene la lista de objetos.</param>
        /// <returns>Retorna una lista de objetos del tipo Models.UniqueUsers</returns>
        public static List<User> CreateUniqueUsers(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<User> uniqueUsers = new List<User>();

            foreach (UniqueUser.Entities.UniqueUsers fields in businessCollection)
            {
                uniqueUsers.Add(ModelAssembler.CreateUniqueUser(fields));
            }

            return uniqueUsers;
        }
        /// <summary>
        /// Creates the unique user.
        /// </summary>
        /// <param name="uniqueUsers">The unique users.</param>
        /// <returns></returns>
        public static User CreateUniqueUser(UniqueUser.Entities.UniqueUsers uniqueUsers)
        {
            return new User()
            {
                Id = uniqueUsers.UserId,
                Nick = uniqueUsers.AccountName,
                PersonId = uniqueUsers.PersonId
            };
        }
        #endregion
    }
}
