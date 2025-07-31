using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao de AcceessProfile
    /// </summary>
    public class AccessProfileDAO
    {
        /// <summary>
        /// Obtener lista de GetAccessObjectByModuleIdSubModuleId
        /// </summary>
        /// <param name="moduleId">Id modulo/param>
        /// <param name="subModuleId">Id subModule/param>
        /// <param name="typeId">Id type/param>
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetAccessObjectByModuleIdSubModuleId(int moduleId, int subModuleId, int typeId, int profileId, int parentId)
        {
            List<AccessObject> accessObjects = new List<AccessObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.Accesses.Properties.ModuleCode, typeof(UniqueUser.Entities.Accesses).Name);
            filter.Equal();
            filter.Constant(moduleId);
            if (subModuleId != 0)
            {
                filter.And();
                filter.Property(UniqueUser.Entities.Accesses.Properties.SubmoduleCode, typeof(UniqueUser.Entities.Accesses).Name);
                filter.Equal();
                filter.Constant(subModuleId);
            }
            if (typeId != 0)
            {
                filter.And();
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant(typeId);
            }
            if (parentId != 0)
            {
                filter.And();
                filter.Property(UniqueUser.Entities.Accesses.Properties.ParentAccessId, typeof(UniqueUser.Entities.Accesses).Name);
                filter.Equal();
                filter.Constant(parentId);
            }

            AccessObjectProfileView accessObjectView = new AccessObjectProfileView();
            ViewBuilder builder = new ViewBuilder("AccessObjectProfileView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);

            if (accessObjectView.Accesses.Count > 0)
            {
                accessObjects = Assemblers.ModelAssembler.CreateAccessObjects(accessObjectView.AccessObjects, accessObjectView.Accesses, null);
            }
            if (profileId != 0)
            {
                List<UniqueUser.Entities.AccessProfiles> AccessProfiles = GetAccessProfileByProfileIdAccessId(profileId, 0);
                foreach (AccessObject obj in accessObjects)
                {
                    if (AccessProfiles.Count() > 0)
                    {
                        if (AccessProfiles.Where(x => x.AccessId == obj.AccessId).Count() > 0)
                        {
                            obj.Assigned = true;//habilitado
                        }
                        else
                        {
                            obj.Assigned = false;
                        }
                    }
                }
            }
            return accessObjects;
        }

        /// <summary>
        /// Obtener lista de GetButtonsByUserId
        /// </summary>
        /// <param name="userId">Id user/param>
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetButtonsByUserName(string userName)
        {

            int userId = UserDAO.GetUserId(userName);
            List<AccessObject> accessObjects = new List<AccessObject>();

            if (userId != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant((int)AccessObjectType.BUTTON);
                filter.And();
                filter.Property(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId, typeof(UniqueUser.Entities.ProfileUniqueUser).Name);
                filter.Equal();
                filter.Constant(userId);
                filter.And();
                filter.Property(UniqueUser.Entities.Accesses.Properties.Enabled, typeof(UniqueUser.Entities.Accesses).Name);
                filter.Equal();
                filter.Constant(true);

                AccessObjectProfileView accessObjectView = new AccessObjectProfileView();
                ViewBuilder builder = new ViewBuilder("AccessObjectProfileView");

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);

                if (accessObjectView.Accesses.Count > 0)
                {
                    accessObjects = Assemblers.ModelAssembler.CreateAccessObjects(accessObjectView.AccessObjects, accessObjectView.Accesses, accessObjectView.AccessParents);
                }
            }

            return accessObjects;
        }
        /// <summary>
        /// Obtener lista de AccessProfile
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetAccessObject(bool onlyButtons)
        {
            List<AccessObject> accessObjects = new List<AccessObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!onlyButtons)
            {
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant((int)AccessObjectType.MENUR2);
                filter.Or();
            }

            filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
            filter.Equal();
            filter.Constant((int)AccessObjectType.BUTTON);
            AccessObjectView accessObjectView = new AccessObjectView();
            ViewBuilder builder = new ViewBuilder("AccessObjectView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);

            if (accessObjectView.Access.Count > 0)
            {
                accessObjects = Assemblers.ModelAssembler.CreateAccessProfileObjects(accessObjectView.Access,
                    accessObjectView.AccessObjects,
                    accessObjectView.Modules,
                    accessObjectView.SubModules
                    );
            }
            return accessObjects;
        }

        /// <summary>
        /// Guarda los accesos
        /// </summary>
        /// <param name="accesses">list AccessObject</param>
        /// <returns>bool</returns>
        public List<AccessObject> CreateAccessObject(List<AccessObject> accesses)
        {
            foreach (AccessObject item in accesses)
            {
                PrimaryKey key = UniqueUser.Entities.Accesses.CreatePrimaryKey(item.Id);
                UniqueUser.Entities.Accesses entity = new UniqueUser.Entities.Accesses(item.Id);

                entity = (UniqueUser.Entities.Accesses)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (entity != null && item.Id != 0)
                {
                    if (item.Status == Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.StatusItem.Deleted.ToString())
                    {
                        AccessProfileDAO accessProfileDAO = new AccessProfileDAO();
                        List<UniqueUser.Entities.AccessProfiles> profiles = GetAccessProfileByProfileIdAccessId(0, entity.AccessId);
                        if (profiles.Count == 0)
                        {
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
                        }
                        else
                        {
                            item.Status = "NotDelete";
                        }
                    }
                    else
                    {
                        entity.Url = item.Url;
                        entity.SubmoduleCode = item.SubModule.Id;
                        entity.ModuleCode = item.SubModule.Module.Id;
                        entity.ParentAccessId = (int)item.ParentAccessId;
                        entity.Enabled = item.IsEnabled;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
                        item.Status = "Update";

                        key = UniqueUser.Entities.AccessObjects.CreatePrimaryKey(item.AccessObjectId);
                        UniqueUser.Entities.AccessObjects entityAccessObject = new UniqueUser.Entities.AccessObjects(item.AccessObjectId);
                        entityAccessObject = (UniqueUser.Entities.AccessObjects)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        if (entityAccessObject != null)
                        {
                            entityAccessObject.ObjectName = item.Description;
                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityAccessObject);
                            item.Status = "Update";
                        }
                    }
                }
                else
                {
                    if (item.Status != Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.StatusItem.Deleted.ToString())
                    {
                        key = UniqueUser.Entities.AccessObjects.CreatePrimaryKey(item.AccessObjectId);
                        UniqueUser.Entities.AccessObjects entityAccessObject = Assemblers.EntityAssembler.CreateAccessObject(item);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entityAccessObject);
                        item.AccessObjectId = entityAccessObject.AccessObjectId;
                        entity = Assemblers.EntityAssembler.CreateAccess(item);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);

                    }
                }
            }
            return accesses;
        }
        /// <summary>
        /// Obtener Entities de AccessProfile por Id perfil
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public static List<UniqueUser.Entities.AccessProfiles> GetAccessProfileByProfileIdAccessId(int profileId, int accessId)
        {
            List<UniqueUser.Entities.AccessProfiles> entities = new List<UniqueUser.Entities.AccessProfiles>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (profileId != 0)
            {
                filter.Property(UniqueUser.Entities.AccessProfiles.Properties.ProfileId, typeof(UniqueUser.Entities.AccessProfiles).Name);
                filter.Equal();
                filter.Constant(profileId);
            }
            if (profileId != 0 && accessId != 0)
            {
                filter.And();
            }
            if (accessId != 0)
            {
                filter.Property(UniqueUser.Entities.Accesses.Properties.AccessId, typeof(UniqueUser.Entities.Accesses).Name);
                filter.Equal();
                filter.Constant(accessId);
            }

            AccessObjectProfileView accessObjectView = new AccessObjectProfileView();
            ViewBuilder builder = new ViewBuilder("AccessObjectProfileView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);

            if (accessObjectView.AccessProfiles.Count > 0)
            {
                entities = accessObjectView.AccessProfiles.Cast<UniqueUser.Entities.AccessProfiles>().ToList();
            }
            return entities;
        }
        /// <summary>
        /// Guarda los accesos en AccessProfile
        /// </summary>
        /// <param name="accesses">list AccessObject</param>
        /// <returns>bool</returns>
        public static void CreateAccessProfile(List<UniqueUser.Entities.AccessProfiles> accesses, int newProfileId)
        {
            foreach (UniqueUser.Entities.AccessProfiles item in accesses)
            {
                PrimaryKey key = UniqueUser.Entities.AccessProfiles.CreatePrimaryKey(newProfileId, item.AccessId, item.DatabaseId);
                UniqueUser.Entities.AccessProfiles entity = (UniqueUser.Entities.AccessProfiles)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (entity == null)
                {
                    entity = Assemblers.EntityAssembler.CreateAccessProfile(item, newProfileId);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                }
            }
        }
        /// <summary>
        /// Guarda un acceso en AccessProfile
        /// </summary>
        /// <param name="accesses">list AccessObject</param>
        /// <returns>bool</returns>
        public static void CreateAccessProfile(UniqueUser.Entities.AccessProfiles access)
        {
            PrimaryKey key = UniqueUser.Entities.AccessProfiles.CreatePrimaryKey(access.ProfileId, access.AccessId, access.DatabaseId);
            UniqueUser.Entities.AccessProfiles entity = (UniqueUser.Entities.AccessProfiles)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (entity == null)
            {
                entity = Assemblers.EntityAssembler.CreateAccessProfile(access, access.ProfileId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            }
        }
        /// <summary>
        /// borra los accesos de AccessProfile
        /// </summary>
        /// <param name="accesses">list AccessObject</param>
        /// <returns>bool</returns>
        public static int DeleteAccessProfile(int accessId, int newProfileId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.AccessProfiles.Properties.ProfileId, typeof(UniqueUser.Entities.AccessProfiles).Name);
            filter.Equal();
            filter.Constant(newProfileId);
            filter.And();
            filter.Property(UniqueUser.Entities.AccessProfiles.Properties.AccessId, typeof(UniqueUser.Entities.AccessProfiles).Name);
            filter.Equal();
            filter.Constant(accessId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.AccessProfiles), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                UniqueUser.Entities.AccessProfiles entity = (UniqueUser.Entities.AccessProfiles)businessCollection[0];
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
            }
            return 0;
        }

        /// <summary>
        /// Obtener lista de AccessProfile
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public List<UniqueUser.Entities.Accesses> GetEntitiesAccessObject(bool onlyButtons)
        {
            List<UniqueUser.Entities.Accesses> accessObjects = new List<UniqueUser.Entities.Accesses>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!onlyButtons)
            {
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant((int)AccessObjectType.MENUR2);
                filter.Or();
            }

            filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
            filter.Equal();
            filter.Constant((int)AccessObjectType.BUTTON);
            AccessObjectView accessObjectView = new AccessObjectView();
            ViewBuilder builder = new ViewBuilder("AccessObjectView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);

            if (accessObjectView.Access.Count > 0)
            {
                accessObjects = accessObjectView.Access.Cast<UniqueUser.Entities.Accesses>().ToList();
            }
            return accessObjects;
        }

        /// <summary>
        /// Obtener lista de AccessObject por filtro
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetAccessesByAccess(AccessObject accessObject, bool validateEnabled)
        {
            List<AccessObject> accessObjects = new List<AccessObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (accessObject.SubModule.Module.Id != 0)
            {
                filter.Property(UniqueUser.Entities.Modules.Properties.ModuleCode, typeof(UniqueUser.Entities.Modules).Name);
                filter.Equal();
                filter.Constant(accessObject.SubModule.Module.Id);
            }

            if (accessObject.SubModule.Id != 0)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.Submodules.Properties.SubmoduleCode, typeof(UniqueUser.Entities.Submodules).Name);
                filter.Equal();
                filter.Constant(accessObject.SubModule.Id);
            }
            if (validateEnabled)
            {
                if (accessObject.IsEnabled == true)
                {
                    if (filter.GetPredicate() != null)
                    {
                        filter.And();
                    }
                    filter.Property(UniqueUser.Entities.Accesses.Properties.Enabled, typeof(UniqueUser.Entities.Accesses).Name);
                    filter.Equal();
                    filter.Constant(true);
                }
                else
                {
                    if (filter.GetPredicate() != null)
                    {
                        filter.And();
                    }
                    filter.Property(UniqueUser.Entities.Accesses.Properties.Enabled, typeof(UniqueUser.Entities.Accesses).Name);
                    filter.Equal();
                    filter.Constant(false);
                }
            }           
            if (accessObject.ObjectTypeId != 0)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant((int)accessObject.ObjectTypeId);
            }
            else
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.OpenParenthesis();
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant((int)AccessObjectType.MENUR2);
                filter.Or();
                filter.Property(UniqueUser.Entities.AccessObjects.Properties.AccessObjTypeCode, typeof(UniqueUser.Entities.AccessObjects).Name);
                filter.Equal();
                filter.Constant((int)AccessObjectType.BUTTON);
                filter.CloseParenthesis();
            }


            AccessObjectView accessObjectView = new AccessObjectView();
            ViewBuilder builder = new ViewBuilder("AccessObjectView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);

            if (accessObjectView.Access.Count > 0)
            {
                accessObjects = Assemblers.ModelAssembler.CreateAccessProfileObjects(accessObjectView.Access, accessObjectView.AccessObjects,
                    accessObjectView.Modules, accessObjectView.SubModules);
            }
            return accessObjects;
        }

        /// <summary>
        /// Consulta si el perfil tiene accesos asignados
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public static bool HasAccessByProfileId(int profileId)
        {
            bool hasAccess = false;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.AccessProfiles.Properties.ProfileId, typeof(UniqueUser.Entities.AccessProfiles).Name);
            filter.Equal();
            filter.Constant(profileId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.AccessProfiles), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                hasAccess = true;
            }
            return hasAccess;
        }

        /// <summary>
        /// Asigna o elimina los accesos al perfil
        /// </summary>       
        public void AssingAllAccess(int moduleId, int subModuleId, int profileId, bool active)
        {

            AccessObject accessObject = new AccessObject();
            accessObject.SubModule = new SubModule { Id = subModuleId, Module = new Module { Id = moduleId } };
            List<AccessObject> activeAccess = this.GetAccessesByAccess(accessObject, false);

            if (active)
            {
                foreach (AccessObject item in activeAccess)
                {
                    //Se asigna el acceso al perfil
                    ProfileDAO profileDao = new ProfileDAO();
                    Profile profile = profileDao.GetProfilesByDescription("", profileId).FirstOrDefault();

                    List<UniqueUser.Entities.AccessProfiles> accessProfile = new List<UniqueUser.Entities.AccessProfiles>();
                    UniqueUser.Entities.AccessProfiles newAccess = new UniqueUser.Entities.AccessProfiles(profile.Id, item.Id, (int)UniqueUserTypes.DataBasesEnum.DataBase1);
                    newAccess.ExpirationDate = null;
                    newAccess.Enabled = true;
                    accessProfile.Add(newAccess);
                    AccessProfileDAO.CreateAccessProfile(accessProfile, profile.Id);
                }
            }
            else
            {
                foreach (AccessObject item in activeAccess)
                {
                    AccessProfileDAO.DeleteAccessProfile(item.Id, profileId);
                }
            }
            
        }
     

    }

}