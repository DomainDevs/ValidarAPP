using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Company.Application.UniqueUserApplicationServices.DTO;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class ModelAssembler
    {
        #region User
        public static User CreateUser(UniqueUserModelsView userModel)
        {
            User user = new User();

            if (userModel.UserId == "0")
            {
                user.CreationDate = DateTime.Today;
                user.CreatedUserId = SessionHelper.GetUserId();
                user.AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard;
                user.LastModificationDate = null;
                user.LockDate = null;
            }
            else
            {
                user.ModifiedUserId = SessionHelper.GetUserId();
                user.LastModificationDate = DateTime.Today;
            }
            user.AccountName = userModel.AccountName;
            if (userModel.UniqueUsersLogin != null)
            {
                user.UniqueUsersLogin = new UniqueUserLogin
                {
                    ExpirationDate = userModel.UniqueUsersLogin.ExpirationDate,
                    ExpirationsDays = userModel.UniqueUsersLogin.ExpirationsDays,
                    MustChangePassword = userModel.UniqueUsersLogin.MustChangePassword,
                    Password = userModel.UniqueUsersLogin.Password,
                    UserId = userModel.UniqueUsersLogin.UserId
                };
            }

            user.PersonId = Convert.ToInt32(userModel.PersonId);
            user.DisableDate = userModel.DisableDate;
            user.ExpirationDate = userModel.DateExpirationUser;
            user.LockDate = userModel.LockDate;
            foreach (ProfileModelsView item in userModel.Profiles)
            {
                user.Profiles.Add(new Profile
                {
                    Description = item.Description,
                    EnabledDescription = item.EnabledDescription,
                    HasAccess = item.HasAccess,
                    Id = item.Id,
                    IsEnabled = item.Enabled,
                    Static = item.Static
                });
            }

            return user;
        }

        public List<CoHierarchyAssociation> CreateHierarchies(List<HierarchyModelsView> hierarchyModel)
        {

            foreach (HierarchyModelsView hierarchyModelsView in hierarchyModel)
            {
                CoHierarchyAssociation hierarchy = new CoHierarchyAssociation();
            }

            return null;
        }

        public static List<Application.UniquePersonService.V1.Models.IndividualRelationApp> CreateIndividualsRelation(List<AgentModelsView> AgentsModel)
        {
            List<Application.UniquePersonService.V1.Models.IndividualRelationApp> IndividualsRelation = new List<Application.UniquePersonService.V1.Models.IndividualRelationApp>();

            foreach (AgentModelsView agent in AgentsModel)
            {
                Application.UniquePersonService.V1.Models.IndividualRelationApp hierarchy = new Application.UniquePersonService.V1.Models.IndividualRelationApp();
                hierarchy.Agency.Id = agent.AgencyId;
                hierarchy.Agency.Code = Convert.ToInt32(agent.AgentCode);
                hierarchy.IndividualRelationAppId = agent.IndividualRelationAppId;
                hierarchy.ChildIndividual.IndividualId = agent.ChildIndividual;
                hierarchy.ParentIndividualId = agent.ParentIndividualId;

                IndividualsRelation.Add(hierarchy);
            }

            return IndividualsRelation;
        }

        public static List<UniqueUserModelsView> CreateUsersModelView(List<User> userModel)
        {
            List<UniqueUserModelsView> users = new List<UniqueUserModelsView>();
            if (userModel != null)
            {
                foreach (User item in userModel)
                {
                    users.Add(CreateUserModelView(item));
                }
            }
            return users;
        }
        public static UniqueUserModelsView CreateUserModelView(User userModel)
        {
            UniqueUserModelsView user = new UniqueUserModelsView();

            if (userModel.CreationDate != null && userModel.CreationDate != DateTime.MinValue)
            {
                user.CreationDate = (DateTime)userModel.CreationDate;
            }
            user.CreatedUserId = userModel.CreatedUserId;
            user.AuthenticationType = userModel.AuthenticationType;
            if (userModel.LastModificationDate != null && userModel.LastModificationDate != DateTime.MinValue)
            {
                user.LastModificationDate = (DateTime)userModel.LastModificationDate;
            }
            user.LockDate = userModel.LockDate;

            user.ModifiedUserId = userModel.ModifiedUserId;

            user.AccountName = userModel.AccountName;
            if (userModel.UniqueUsersLogin != null)
            {
                user.UniqueUsersLogin = new UniqueUserLoginModelsView();

                var imapper = CreateMapUserLogin();
                user.UniqueUsersLogin = imapper.Map<UniqueUserLogin, UniqueUserLoginModelsView>(userModel.UniqueUsersLogin);
            }

            user.PersonId = userModel.PersonId.ToString();
            user.DisableDate = userModel.DisableDate;
            if (userModel.ExpirationDate != null && userModel.ExpirationDate != DateTime.MinValue)
            {
                user.ExpirationDate = (DateTime)userModel.ExpirationDate;
            }
            if (user.UniqueUsersLogin != null)
            {
                if (user.UniqueUsersLogin.ExpirationDate == DateTime.MinValue)
                {
                    user.UniqueUsersLogin.ExpirationDate = null;
                }
            }
            if (userModel.Hierarchies != null)
            {
                user.Hierarchies = new List<CoHierarchyAssociationModelsView>();

                List<CoHierarchyAssociationModelsView> hierarchies = new List<CoHierarchyAssociationModelsView>();
                hierarchies = AutoMapper.Mapper.Map<List<CoHierarchyAssociation>, List<CoHierarchyAssociationModelsView>>(userModel.Hierarchies);
                user.Hierarchies.AddRange(hierarchies);
            }
            if (userModel.Branch != null)
            {
                user.Branch = userModel.Branch.OrderBy(x => x.Description).ToList();
            }
            if (userModel.IndividualsRelation != null)
            {
                user.IndividualsRelation = new List<IndividualRelationAppModelsView>();
                List<IndividualRelationAppModelsView> relations = new List<IndividualRelationAppModelsView>();

                relations = CreateIndividualRelationAppObject(userModel.IndividualsRelation, int.Parse(user.PersonId));

                user.IndividualsRelation.AddRange(relations);
            }
            if (userModel.Profiles != null)
            {
                user.Profiles = new List<ProfileModelsView>();
                List<ProfileModelsView> profiles = new List<ProfileModelsView>();

                profiles = AutoMapper.Mapper.Map<List<Profile>, List<ProfileModelsView>>(userModel.Profiles);
                user.Profiles.AddRange(profiles);
            }

            if (userModel.DisableDate != null)
            {
                user.Status = App_GlobalResources.Language.Disabled;
            }
            else
            {
                user.Status = App_GlobalResources.Language.LabelEnabled;
            }
            user.UserId = userModel.UserId.ToString();
            user.Name = userModel.Name;
            return user;
        }
        #endregion

        #region Module
        public static List<Module> CreateModules(List<ModuleModelsView> modulesModelView)
        {
            List<Module> modules = new List<Module>();
            foreach (ModuleModelsView model in modulesModelView)
            {
                Module module = new Module();
                module.Description = model.Description;
                module.IsEnabled = model.Enabled;
                module.ExpirationDate = model.ExpirationDate;
                module.Id = model.Id;
                module.VirtualFolder = model.VirtualFolder;
                module.Status = model.Status;
                modules.Add(module);
            }

            return modules;
        }

        public static List<ModuleModelsView> CreateModules(List<Module> modules)
        {
            List<ModuleModelsView> moduleModelsView = new List<ModuleModelsView>();
            foreach (Module model in modules)
            {
                ModuleModelsView module = new ModuleModelsView();
                module.Description = model.Description;
                module.Enabled = model.IsEnabled;
                module.ExpirationDate = model.ExpirationDate;
                module.Id = model.Id;
                module.VirtualFolder = model.VirtualFolder;
                module.EnabledDescription = model.EnabledDescription;
                moduleModelsView.Add(module);
            }

            return moduleModelsView;
        }
        #endregion

        #region SubModule
        public static List<SubModule> CreateSubModules(List<SubModuleModelsView> modulesModelView)
        {
            List<SubModule> modules = new List<SubModule>();
            foreach (SubModuleModelsView model in modulesModelView)
            {
                SubModule module = new SubModule();
                module.Description = model.Description;
                module.IsEnabled = model.Enabled;
                module.Id = model.Id;
                module.Module = new Module { Id = model.ModuleId, Description = model.ModuleDescription };
                module.Status = model.Status;
                modules.Add(module);
            }

            return modules;
        }

        public static List<SubModuleModelsView> CreateSubModules(List<SubModule> modules)
        {
            List<SubModuleModelsView> moduleModelsView = new List<SubModuleModelsView>();
            foreach (SubModule model in modules)
            {
                SubModuleModelsView module = new SubModuleModelsView();
                module.Description = model.Description;
                module.Enabled = model.IsEnabled;
                module.Id = model.Id;
                module.EnabledDescription = model.EnabledDescription;
                module.ModuleDescription = model.Module.Description;
                module.ModuleId = model.Module.Id;
                moduleModelsView.Add(module);
            }

            return moduleModelsView;
        }
        #endregion

        #region Profile
        public static Profile CreateProfile(ProfileModelsView profileModelView)
        {
            Profile profile = new Profile { Id = profileModelView.Id, Description = profileModelView.Description, IsEnabled = profileModelView.Enabled, Static = profileModelView.Static };
            return profile;
        }

        public static List<ProfileModelsView> CreateProfiles(List<Profile> profiles)
        {
            List<ProfileModelsView> profilesModel = new List<ProfileModelsView>();
            foreach (Profile item in profiles)
            {
                ProfileModelsView profileModelsView = new ProfileModelsView
                {
                    Id = item.Id,
                    Description =
                    item.Description,
                    Enabled = item.IsEnabled,
                    Static = item.Static,
                    HasAccess = item.HasAccess,
                    EnabledDescription = item.EnabledDescription
                };
                profilesModel.Add(profileModelsView);
            }
            return profilesModel;
        }

        public static List<AccessProfile> CreateProfiles(List<ProfileAccessView> profiles, int profileId)
        {
            List<AccessProfile> profilesModel = new List<AccessProfile>();
            if (profiles != null)
            {
                foreach (ProfileAccessView item in profiles)
                {
                    AccessProfile profileModelsView = new AccessProfile { AccessId = item.AccessId, Assigned = item.Assigned, ProfileId = profileId , AccessObjectId = item.AccessObjectId };
                    if (item.Type == (string)HttpContext.GetGlobalResourceObject("Language", "PERMISION"))//Albarracin
                    {
                        profileModelsView.AccessType = (int)AccessObjectType.PERMISION;
                    };
                    profilesModel.Add(profileModelsView);
                }
            }
            return profilesModel;
        }
        public static List<ProfileGuaranteeStatus> CreateGuaranteeProfiles(List<GuaranteeStatusModelsView> profiles, int profileId)
        {
            List<ProfileGuaranteeStatus> profilesModel = new List<ProfileGuaranteeStatus>();
            if (profiles != null)
            {
                foreach (GuaranteeStatusModelsView item in profiles)
                {
                    ProfileGuaranteeStatus profileModelsView = new ProfileGuaranteeStatus { Id = item.IdGuaranteeStatus, StatusId = item.Id, ProfileId = profileId, Enabled = item.Enabled };
                    profilesModel.Add(profileModelsView);
                }
            }
            return profilesModel;
        }
        public static List<Profile> CreateModelProfiles(List<ProfileModelsView> profiles)
        {
            List<Profile> profilesModel = new List<Profile>();
            if (profiles != null)
            {
                foreach (ProfileModelsView item in profiles)
                {
                    Profile profileModelsView = new Profile { Id = item.Id, Description = item.Description };
                    profilesModel.Add(profileModelsView);
                }
            }
            return profilesModel;
        }
        #endregion

        #region Access
        public static List<AccessModelsView> CreateAccessObject(List<AccessObject> access)
        {
            List<AccessModelsView> accessModel = new List<AccessModelsView>();
            foreach (AccessObject item in access)
            {
                AccessModelsView accessModelsView = new AccessModelsView
                {
                    Id = item.Id,
                    AccessObjectId = item.AccessObjectId,
                    Description = item.Description,
                    SubModuleId = item.SubModule.Id,
                    ModuleId = item.SubModule.Module.Id,
                    ModuleDescription = item.SubModule.Module.Description,
                    SubModuleDescription = item.SubModule.Description,
                    Path = item.Url,
                    Enabled = item.IsEnabled,
                    EnabledDescription = item.IsEnabled == true ? App_GlobalResources.Language.Enabled : App_GlobalResources.Language.Disabled,
                    AccessTypeId = item.ObjectTypeId,
                    AccessTypeDescription = (string)HttpContext.GetGlobalResourceObject("Language", Enum.GetName(typeof(AccessObjectType), item.ObjectTypeId)),
                    ParentAccessId = item.ParentAccessId
                };
                accessModel.Add(accessModelsView);
            }
            return accessModel;
        }

        public static List<IndividualRelationAppModelsView> CreateIndividualRelationAppObject(List<Sistran.Core.Application.UniqueUserServices.Models.IndividualRelationApp> individualRelationApps, int parentIndividualId)
        {
            List<IndividualRelationAppModelsView> individualRelationAppModel = new List<IndividualRelationAppModelsView>();
            foreach (Sistran.Core.Application.UniqueUserServices.Models.IndividualRelationApp item in individualRelationApps)
            {
                IndividualRelationAppModelsView individualRelationAppModelView = new IndividualRelationAppModelsView
                {
                    RelationTypeCd = item.RelationTypeId,
                    ChildIndividual = new AgentModelsView { IndividualId = item.Agency.Agent.IndividualId, IndividualRelationAppId = item.Id, FullName = item.Agency.Agent.FullName, ParentIndividualId = parentIndividualId },
                    IndividualRelationAppId = item.Id,
                    ParentIndividualId = parentIndividualId,
                    Agency = new AgencyModelsView { Code = item.Agency.Code, FullName = item.Agency.Agent.FullName, Id = item.Agency.Id }
                };

                individualRelationAppModel.Add(individualRelationAppModelView);
            }
            return individualRelationAppModel;
        }

        public static List<AccessObject> CreateAccessObject(List<AccessModelsView> access)
        {
            List<AccessObject> accessModel = new List<AccessObject>();
            foreach (AccessModelsView item in access)
            {
                AccessObject accessModelsView = new AccessObject
                {
                    Id = item.Id,
                    AccessObjectId = item.AccessObjectId,
                    Description = item.Description,
                    IsEnabled = item.Enabled,
                    ObjectTypeId = item.AccessTypeId,
                    SubModule = new SubModule { Id = item.SubModuleId, Description = item.SubModuleDescription, Module = new Module { Id = item.ModuleId, Description = item.ModuleDescription } },
                    Url = item.Path,
                    Status = item.Status,
                    ParentAccessId = item.ParentAccessId
                };
                accessModel.Add(accessModelsView);
            }
            return accessModel;
        }
        #endregion
        #region contextpermisions
        public static List<ContextProfileAccessPermissions> CreateContextPermissions(List<UserContextPermissionsModelsView> ContextPermissionsModelsView)
        {
            List<ContextProfileAccessPermissions> accessModel = new List<ContextProfileAccessPermissions>();
            foreach (UserContextPermissionsModelsView item in ContextPermissionsModelsView)
            {
                ContextProfileAccessPermissions accessModelsView = new ContextProfileAccessPermissions
                {
                    AccessPermission= new AccessPermissions { Id = item.PermissionsId },
                    SecurityContext = new SecurityContext { Id = item.Id },
                    Profile = new Profile { Id = item.ProfileId },
                    Assigned=item.Assigned
                  
                };
                accessModel.Add(accessModelsView);
            }
            return accessModel;
        }
        #endregion

        #region User Groups
        public static List<UserGroupModelsView> MappUserGroupModelView(GroupQueryDTO groups, UserGroupQueryDTO userGroups)
        {
            List<UserGroupModelsView> userGroupModel = new List<UserGroupModelsView>();

            if (groups.GroupDTO.Count != 0)
            {
                foreach (GroupDTO field in groups.GroupDTO)
                {
                    UserGroupModelsView userGroupModelsView = new UserGroupModelsView
                    {
                        IdGroup = field.Id,
                        Description = field.Description,
                        Check = userGroups.UserGroupDTO.Any(g => g.IdGroup == field.Id)
                    };

                    userGroupModel.Add(userGroupModelsView);
                }

                return userGroupModel;
            }

            return null;

        }
        #endregion

        #region automapper
        public static AutoMapper.IMapper CreateMapUserLogin()
        {
            var config = MapperCache.GetMapper<UniqueUserLogin, UniqueUserLoginModelsView>(cfg =>
            {
                cfg.CreateMap<UniqueUserLogin, UniqueUserLoginModelsView>();
            });

            return config;
        }


        #endregion automapper
    }
}