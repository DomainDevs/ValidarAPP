using Sistran.Core.Application.UniqueUser.Entities;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao User
    /// </summary>
    public class AccessPermissionsDAO
    {

        /// <summary>
        /// Get User By AccountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>User</returns>
        public List<Models.AccessPermissions> GetAccessPermissionsByUserId(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId, typeof(UniqueUser.Entities.ProfileUniqueUser).Name);
            filter.Equal();
            filter.Constant(userId);

            UserProfilePermissionsView accessObjectView = new UserProfilePermissionsView();
            ViewBuilder builder = new ViewBuilder("UserProfilePermissionsView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);
            var permissions = ModelAssembler.CreateAccessPermissions(accessObjectView.AccessPermissions);

            //ForEach(accessObjectView.ProfileAccessPermissions, ParallelHelper.DebugParallelFor(), (profileAccessPermission) =>
            foreach (var profileAccessPermission in accessObjectView.ProfileAccessPermissions)
            {
                var profileAccessPermissionEntity = (UUEN.ProfileAccessPermissions)profileAccessPermission;
                ObjectCriteriaBuilder filterContext = new ObjectCriteriaBuilder();

                filterContext.Property(UniqueUser.Entities.UserContextPermission.Properties.PermissionCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
                filterContext.Equal();
                filterContext.Constant(profileAccessPermissionEntity.AccessPermissionsCode);

                filterContext.And();
                filterContext.Property(UniqueUser.Entities.UserContextPermission.Properties.ProfileCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
                filterContext.Equal();
                filterContext.Constant(profileAccessPermissionEntity.ProfileCode);

                UserContextPermissionView userContextPermissionView = new UserContextPermissionView();
                ViewBuilder builderUserContextPermissionView = new ViewBuilder("UserContextPermissionView");

                builderUserContextPermissionView.Filter = filterContext.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderUserContextPermissionView, userContextPermissionView);
                foreach (var permission in permissions)
                {
                    if (permission.Id == profileAccessPermissionEntity.AccessPermissionsCode)
                        permission.ContextPermissions = ModelAssembler.CreateContexts(userContextPermissionView.SecurityContext);
                }
            }

            return permissions;
        }

        /// <summary>
        /// Get User By AccountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>User</returns>
        public List<Models.AccessPermissions> GetAccessPermissionsByProfileId(int profileId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.ProfileAccessPermissions.Properties.ProfileCode, typeof(UniqueUser.Entities.ProfileAccessPermissions).Name);
            filter.Equal();
            filter.Constant(profileId);

            ProfilePermissionsView accessObjectView = new ProfilePermissionsView();
            ViewBuilder builder = new ViewBuilder("ProfilePermissionsView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);
            var permissions = ModelAssembler.CreateAccessPermissions(accessObjectView.AccessPermissions);
            //Parallel.ForEach(accessObjectView.ProfileAccessPermissions, ParallelHelper.DebugParallelFor(), (profileAccessPermission) =>
            foreach (var profileAccessPermission in accessObjectView.ProfileAccessPermissions)
            {
                var profileAccessPermissionEntity = (UUEN.ProfileAccessPermissions)profileAccessPermission;
                ObjectCriteriaBuilder filterContext = new ObjectCriteriaBuilder();

                filterContext.Property(UniqueUser.Entities.UserContextPermission.Properties.PermissionCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
                filterContext.Equal();
                filterContext.Constant(profileAccessPermissionEntity.AccessPermissionsCode);

                filterContext.And();
                filterContext.Property(UniqueUser.Entities.UserContextPermission.Properties.ProfileCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
                filterContext.Equal();
                filterContext.Constant(profileId);

                UserContextPermissionView userContextPermissionView = new UserContextPermissionView();
                ViewBuilder builderUserContextPermissionView = new ViewBuilder("UserContextPermissionView");

                builderUserContextPermissionView.Filter = filterContext.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderUserContextPermissionView, userContextPermissionView);
                foreach (var permission in permissions)
                {
                    if (permission.Id == profileAccessPermissionEntity.AccessPermissionsCode)
                        permission.ContextPermissions = ModelAssembler.CreateContexts(userContextPermissionView.SecurityContext);
                }


            }
            return permissions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public List<Models.ProfileAccesPermissions> GetAccessPermissionsByModuleIdSubModuleIdProfileId(int moduleId, int subModuleId, int ProfileId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.ProfileAccessPermissions.Properties.ProfileCode, typeof(UniqueUser.Entities.ProfileAccessPermissions).Name);
            filter.Equal();
            filter.Constant(ProfileId);
            if (moduleId != 0)
            {
                filter.And();
                filter.Property(UUEN.AccessPermissions.Properties.ModuleCode, typeof(UniqueUser.Entities.AccessPermissions).Name);
                filter.Equal();
                filter.Constant(moduleId);
            }
            if (subModuleId != 0)
            {
                filter.And();
                filter.Property(UUEN.AccessPermissions.Properties.SubmoduleCode, typeof(UniqueUser.Entities.AccessPermissions).Name);
                filter.Equal();
                filter.Constant(subModuleId);
            }
            ProfilePermissionsView accessObjectView = new ProfilePermissionsView();
            ViewBuilder builder = new ViewBuilder("ProfilePermissionsView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessObjectView);
           
            List<ProfileAccesPermissions> profileAccessPermissions = ModelAssembler.CreateProfileAccessPermissions(accessObjectView.ProfileAccessPermissions);
            return profileAccessPermissions;

        }
        
        /// <summary>
        /// Get User By AccountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>User</returns>
        public List<Models.AccessPermissions> GetAccessPermissionsBysubmoduleCode(int moduleCode, int submoduleCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.AccessPermissions.Properties.ModuleCode, typeof(UniqueUser.Entities.AccessPermissions).Name);
            filter.Equal();
            filter.Constant(moduleCode);
            filter.And();
            filter.Property(UniqueUser.Entities.AccessPermissions.Properties.SubmoduleCode, typeof(UniqueUser.Entities.AccessPermissions).Name);
            filter.Equal();
            filter.Constant(submoduleCode);

            var permissions = DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.AccessPermissions), filter.GetPredicate());
            return ModelAssembler.CreateAccessPermissions(permissions);
        }
        
        public int SaveAccesPermission(AccessProfile profile)
        {
            int idProfileAccesPermissions = 0;
            PrimaryKey key = UniqueUser.Entities.ProfileAccessPermissions.CreatePrimaryKey(profile.AccessObjectId);
            UniqueUser.Entities.ProfileAccessPermissions entity = (UniqueUser.Entities.ProfileAccessPermissions)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (entity != null && !profile.Assigned)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
                idProfileAccesPermissions = entity.Id;
            }
            if (entity == null && profile.Assigned)
            {
                    entity = Assemblers.EntityAssembler.CreateProfileAccessPermissions(profile);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                    idProfileAccesPermissions = entity.Id;
            }
            return idProfileAccesPermissions;
        }

        /// <summary>
        /// lista los contextos assignados por usuario  y permiso
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="PermissionsId"></param>
        /// <returns></returns>
        public List<Models.ContextProfileAccessPermissions> GetcontextPermissionsByProfileIdPermissionsId(int profileId,int PermissionsId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UserContextPermission.Properties.PermissionCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
            filter.Equal();
            filter.Constant(PermissionsId);
            filter.And();
            filter.Property(UniqueUser.Entities.UserContextPermission.Properties.ProfileCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
            filter.Equal();
            filter.Constant(profileId);
            var contextPermissions = DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.UserContextPermission), filter.GetPredicate());
            return ModelAssembler.CreateContextPermissions(contextPermissions);
        }


        /// <summary>
        /// lista los contextos registrados 
        /// </summary>
        /// <returns></returns>
        public List<Models.SecurityContext> GetSecurityContexts()
        {
            var SecurityContext = DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.SecurityContext));
            return ModelAssembler.CreateSecurityContexts(SecurityContext);
        }

        public int SaveContextPermissions(ContextProfileAccessPermissions contextPermissions)
        {
            int contextPermissionsid = 0;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UserContextPermission.Properties.PermissionCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
            filter.Equal();
            filter.Constant(contextPermissions.AccessPermission.Id);
            filter.And();
            filter.Property(UniqueUser.Entities.UserContextPermission.Properties.ProfileCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
            filter.Equal();
            filter.Constant(contextPermissions.Profile.Id);
            filter.And();
            filter.Property(UniqueUser.Entities.UserContextPermission.Properties.ContextPermissionsCode, typeof(UniqueUser.Entities.UserContextPermission).Name);
            filter.Equal();
            filter.Constant(contextPermissions.SecurityContext.Id);


            var entity = (UniqueUser.Entities.UserContextPermission)DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.UserContextPermission), filter.GetPredicate()).FirstOrDefault();

            if (entity != null && !contextPermissions.Assigned )
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
                contextPermissionsid = entity.Id;
            }
            if (entity == null && contextPermissions.Assigned)
            {
                entity = Assemblers.EntityAssembler.CreateProfileAccessPermissions(contextPermissions);
                entity.PermissionCode = contextPermissions.AccessPermission.Id;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                contextPermissionsid = entity.Id;
            };
            return contextPermissionsid;
        }
    }
}