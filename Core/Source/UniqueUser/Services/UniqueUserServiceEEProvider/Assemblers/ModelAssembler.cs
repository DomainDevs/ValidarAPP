using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using AGE= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniqueUser.Entities;
using Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Resources;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using UUML = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {

        #region Usuario
        /// <summary>
        /// Get list of Models.UniqueUsers
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.User</returns>
        public static List<Models.User> CreateUniqueUsers(Sistran.Core.Framework.DAF.BusinessCollection businessCollection, Sistran.Core.Framework.DAF.BusinessCollection person)
        {
            if (businessCollection != null && businessCollection.Count > 0)
            {
                List<Models.User> uniqueUsers = new List<Models.User>();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<UniqueUser.Entities.UniqueUsers, Models.User>()
                    .ForMember(x => x.CreationDate, opts => opts.MapFrom(y => y.CreatedDate))
                    .ForMember(x => x.LastModificationDate, opts => opts.MapFrom(y => y.ModifiedDate))
                    .ForMember(x => x.LastModificationDate, opts => opts.MapFrom(y => y.ModifiedDate));
                    cfg.CreateMap<List<UniqueUser.Entities.UniqueUsers>, List<Models.User>>()
                   .ConvertUsing(ss => ss.Select(bs => Mapper.Map<UniqueUsers, Models.User>(bs)).ToList());
                });
                uniqueUsers = Mapper.Map<List<UniqueUser.Entities.UniqueUsers>, List<Models.User>>(businessCollection.Cast<UniqueUser.Entities.UniqueUsers>().ToList());
                var persons = person.Cast<Sistran.Core.Application.UniquePerson.Entities.Person>().ToList();
                uniqueUsers.AsParallel().ForAll(x => x.Name = persons?.FirstOrDefault(y => y.IndividualId == x.PersonId)?.Name + " " + persons?.FirstOrDefault(y => y.IndividualId == x.PersonId)?.Surname);
                return uniqueUsers;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get list of Models.UniqueUsers
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.User</returns>
        public static List<Models.User> CreateUniqueUsers(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<Models.User> uniqueUsers = new List<Models.User>();
            if (businessCollection != null)
            {
                foreach (UniqueUser.Entities.UniqueUsers fields in businessCollection)
                {
                    uniqueUsers.Add(CreateUniqueUser(fields));
                }
            }
            return uniqueUsers;
        }
        /// <summary>
        /// Creates the unique user.
        /// </summary>
        /// <param name="uniqueUsers">The unique users.</param>
        /// <returns>Models.User</returns>
        public static Models.User CreateUniqueUser(UniqueUser.Entities.UniqueUsers uniqueUsers)
        {
            Models.User user = new Models.User()
            {
                UserId = uniqueUsers.UserId,
                AccountName = uniqueUsers.AccountName,
                PersonId = uniqueUsers.PersonId,
                CreationDate = uniqueUsers.CreatedDate == null ? DateTime.MinValue : (DateTime)uniqueUsers.CreatedDate,
                LastModificationDate = uniqueUsers.ModifiedDate == null ? DateTime.MinValue : (DateTime)uniqueUsers.ModifiedDate,
                ExpirationDate = uniqueUsers.ExpirationDate == null ? DateTime.MinValue : (DateTime)uniqueUsers.ExpirationDate,
                LockDate = uniqueUsers.LockDate,
                DisableDate = uniqueUsers.DisabledDate,
                AuthenticationTypeCode = uniqueUsers.AuthenticationTypeCode,
                IsDisabledDateNull = uniqueUsers.DisabledDate == null,
                IsExpirationDateNull = uniqueUsers.ExpirationDate == null,
                IsLockDateNull = uniqueUsers.LockDate == null,
                LockPassword = uniqueUsers.LockPassword
            };
            if (uniqueUsers.DisabledDate < DateTime.Now.AddDays(1))
            {
                user.StatusDescription = Errors.StatusDisabled;
            }
            else
            {
                user.StatusDescription = Errors.StatusEnabled;
            }
            return user;
        }


        public static List<Models.User> CreateUniqueeUsers(List<UUEN.UniqueUsers> uniqueUsers)
        {
            var users = new List<Models.User>();
            foreach (UniqueUsers user in uniqueUsers)
            {
                users.Add(CreateUniqueUser(user));
            }
            return users;
        }

        #endregion

        #region UsuarioLogin
        /// <summary>
        ///  Get model of UniqueUserLogin
        /// </summary>
        /// <param name="businessCollection">La colección que contiene la lista de objetos.</param>
        /// <returns>Models.UniqueUserLogin</returns>
        public static Models.UniqueUserLogin CreateUniqueUserLogin(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            Models.UniqueUserLogin uniqueUserLogin = new Models.UniqueUserLogin();
            foreach (UniqueUser.Entities.UniqueUserLogin fields in businessCollection)
            {
                uniqueUserLogin = ModelAssembler.CreateUniqueUserLogin(fields);
            }
            return uniqueUserLogin;
        }
        /// <summary>
        /// Creates uniqueUserLogin.
        /// </summary>
        /// <param name="uniqueUserLogin">The uniqueUserLogin.</param>
        /// <returns>Models.UniqueUserLogin</returns>
        public static Models.UniqueUserLogin CreateUniqueUserLogin(UniqueUser.Entities.UniqueUserLogin uniqueUserLogin)
        {
            return new Models.UniqueUserLogin()
            {
                UserId = uniqueUserLogin.UserId,
                ExpirationDate = uniqueUserLogin.PasswordExpirationDate == null ? DateTime.MinValue : (DateTime)uniqueUserLogin.PasswordExpirationDate,
                MustChangePassword = uniqueUserLogin.MustChangePassword
            };
        }
        #endregion        

        #region HierarchyAssociationDAO

        public static Models.CoHierarchyAssociation CreateHierarchyAssociation(UniqueUser.Entities.CoHierarchyAssociation entityCoHierarchyAssociation)
        {
            return new Models.CoHierarchyAssociation
            {
                Id = entityCoHierarchyAssociation.HierarchyCode,
                Description = entityCoHierarchyAssociation.Description,
                EnabledInd = entityCoHierarchyAssociation.EnabledInd,
                ExclusionayInd = entityCoHierarchyAssociation.ExclusionaryInd,
                LimitInsuredAmt = entityCoHierarchyAssociation.LimitInsuredAmount,
                Module = new Models.Module
                {
                    Id = entityCoHierarchyAssociation.ModuleCode
                },
                SubModule = new Models.SubModule
                {
                    Id = entityCoHierarchyAssociation.SubmoduleCode
                }
            };
        }

        public static List<Models.CoHierarchyAssociation> CreateHierarchiesAssociation(BusinessCollection businessCollection)
        {
            List<Models.CoHierarchyAssociation> coHierarchiesAssociation = new List<Models.CoHierarchyAssociation>();

            foreach (UniqueUser.Entities.CoHierarchyAssociation entity in businessCollection)
            {
                coHierarchiesAssociation.Add(ModelAssembler.CreateHierarchyAssociation(entity));
            }

            return coHierarchiesAssociation;
        }

        public static Models.CoHierarchyAssociation CreateModelHierarchyAssociation(UniqueUser.Entities.CoHierarchyAssociation entityCoHierarchyAssociation, UniqueUser.Entities.Modules module, UniqueUser.Entities.Submodules subModule)
        {
            return new Models.CoHierarchyAssociation
            {
                Id = entityCoHierarchyAssociation.HierarchyCode,
                Description = entityCoHierarchyAssociation.Description,
                EnabledInd = entityCoHierarchyAssociation.EnabledInd,
                Module = new Models.Module
                {
                    Id = entityCoHierarchyAssociation.ModuleCode,
                    Description = module.Description
                },
                SubModule = new Models.SubModule
                {
                    Id = entityCoHierarchyAssociation.SubmoduleCode,
                    Description = subModule.Description
                }
            };
        }

        public static List<Models.CoHierarchyAssociation> CreateModelHierarchiesAssociation(BusinessCollection entityCoHierarchyAssociation, BusinessCollection modulesCollection, BusinessCollection subModuleCollection)
        {
            List<Models.CoHierarchyAssociation> associations = new List<UUML.CoHierarchyAssociation>();
            List<UniqueUser.Entities.CoHierarchyAssociation> hierarchies = entityCoHierarchyAssociation.Cast<UniqueUser.Entities.CoHierarchyAssociation>().ToList();
            List<UniqueUser.Entities.Modules> modules = modulesCollection.Cast<UniqueUser.Entities.Modules>().ToList();
            List<UniqueUser.Entities.Submodules> submodules = subModuleCollection.Cast<UniqueUser.Entities.Submodules>().ToList();

            foreach (UniqueUser.Entities.CoHierarchyAssociation item in hierarchies)
            {
                associations.Add(CreateModelHierarchyAssociation(item,
                    modules.Where(x => x.ModuleCode == item.ModuleCode).FirstOrDefault(),
                    submodules.Where(x => x.ModuleCode == item.ModuleCode && x.SubmoduleCode == item.SubmoduleCode).FirstOrDefault()));
            }

            return associations;
        }
        #endregion

        #region Profile

        public static Models.Profile CreateProfile(UniqueUser.Entities.Profiles entityProfile)
        {
            bool hasAccess = false;
            hasAccess = AccessProfileDAO.HasAccessByProfileId(entityProfile.ProfileId);
            return new Models.Profile()
            {
                Id = entityProfile.ProfileId,
                Description = entityProfile.Description,
                IsEnabled = entityProfile.Enabled,
                HasAccess = hasAccess
            };
        }

        public static List<Models.Profile> CreateProfiles(BusinessCollection businessCollection)
        {
            List<Models.Profile> profilesUser = new List<Models.Profile>();

            foreach (UniqueUser.Entities.Profiles entity in businessCollection)
            {
                profilesUser.Add(ModelAssembler.CreateProfile(entity));
            }

            return profilesUser;
        }

        #endregion

        #region SalePoint


        public static SalePoint CreateSalePoint(COMMEN.SalePoint entitySalePoint, UserSalePoint usersalePoint)
        {
            return new SalePoint()
            {
                Id = (int)entitySalePoint.SalePointCode,
                Description = entitySalePoint.Description,
                IsDefault = usersalePoint != null ? (usersalePoint.DefaultSalePoint == 0 ? false : true) : false
            };
        }
        public static List<SalePoint> CreateSalePoints(BusinessCollection salesPointCollection, BusinessCollection userSalesCollection)
        {
            var salePoints = salesPointCollection.Cast<COMMEN.SalePoint>().ToList();
            if (salePoints?.Count() > 0)
            {
                var immaper = AutommaperAssembler.CreateMapSalePoints();
                var salePointsOutput = immaper.Map<List<COMMEN.SalePoint>, List<SalePoint>>(salePoints);
                var usersalepoints = userSalesCollection.Cast<UserSalePoint>().ToList();
                salePointsOutput.AsParallel().ForAll(x =>
                {
                    var defaultsale = usersalepoints.FirstOrDefault(z => z.SalePointCode == x.Id).DefaultSalePoint;
                    x.IsDefault = defaultsale == 0 ? false : true;
                });
                return salePointsOutput;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region IndividualRelationApp
        public static IndividualRelationApp CreateIndividualRelationApp(UPEN.IndividualRelationApp individualRelationApp)
        {
            return new IndividualRelationApp()
            {
                Id = individualRelationApp.IndividualRelationAppId,
                RelationTypeId = individualRelationApp.RelationTypeCode,
                Agency = new UserAgency
                {
                    Id = individualRelationApp.AgentAgencyId,
                    Agent = new UserAgent
                    {
                        IndividualId = individualRelationApp.ChildIndividualId
                    }
                }
            };
        }


        /// <summary>
        /// Creates the user person.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<IndividualRelationApp> CreateIndividualsRelationApp(BusinessCollection businessCollection)
        {
            List<IndividualRelationApp> individualsRelationApp = new List<IndividualRelationApp>();

            foreach (UPEN.IndividualRelationApp field in businessCollection)
            {
                individualsRelationApp.Add(ModelAssembler.CreateIndividualRelationApp(field));
            }

            return individualsRelationApp;
        }
        #endregion

        #region Agency

        public static UserAgency CreateAgency(UPEN.AgentAgency agency)
        {
            return new UserAgency
            {
                Id = agency.AgentAgencyId,
                Code = agency.AgentCode,
                FullName = agency.Description,
                DateDeclined = agency.DeclinedDate,
                Agent = new UserAgent
                {
                    IndividualId = agency.IndividualId, 
                    AgentType = new AGE.AgentType
                    { Id= agency.AgentTypeCode}

                }
            };
        }

       


        public static List<UserAgency> CreateAgencies(BusinessCollection businessCollection)
        {
            List<UserAgency> agencies = new List<UserAgency>();
            foreach (UPEN.AgentAgency field in businessCollection)
            {
                agencies.Add(ModelAssembler.CreateAgency(field));

            }
            return agencies;
        }

       

        #endregion


        #region Agent

        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns></returns>
        public static UserAgent CreateAgent(UPEN.Agent agent)
        {
            return new UserAgent
            {
                IndividualId = agent.IndividualId,
                FullName = agent.CheckPayableTo,
                DateDeclined = agent.DeclinedDate,
                AgentType = new AGE.AgentType
                {
                    Id = agent.AgentTypeCode                    
                }
            };
        }

        /// <summary>
        /// Creates the agents.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<UserAgent> CreateAgents(BusinessCollection businessCollection)
        {
            List<UserAgent> agents = new List<UserAgent>();

            foreach (UPEN.Agent field in businessCollection)
            {
                agents.Add(ModelAssembler.CreateAgent(field));
            }

            return agents;
        }

        #endregion

        #region Email

        /// <summary>
        /// Creates the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static UserEmail CreateEmail(UPEN.Email email)
        {
            return new UserEmail
            {
                Id = email.DataId,
                Description = email.Address
            };
        }

        /// <summary>
        /// Creates the emails.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<UserEmail> CreateEmails(BusinessCollection businessCollection)
        {
            List<UserEmail> emails = new List<UserEmail>();

            foreach (UPEN.Email field in businessCollection)
            {
                emails.Add(ModelAssembler.CreateEmail(field));
            }

            return emails;
        }

        #endregion

        #region Person
        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static UserPerson CreatePerson(UPEN.Person person)
        {

            return new UserPerson
            {
                Id = person.IndividualId,
                FullName = person.Surname + " " + person.MotherLastName + " " + person.Name
            };
        }

        /// <summary>
        /// Creates the persons.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<UserPerson> CreatePersons(BusinessCollection businessCollection)
        {
            List<UserPerson> persons = new List<UserPerson>();

            foreach (UPEN.Person field in businessCollection)
            {
                persons.Add(ModelAssembler.CreatePerson(field));
            }

            return persons;
        }

        #endregion

        public static Models.Module CreateModule(UniqueUser.Entities.Modules entityModule)
        {
            return new Models.Module
            {
                Id = entityModule.ModuleCode,
                Description = entityModule.Description,
                IsEnabled = entityModule.Enabled,
                ExpirationDate = entityModule.ExpirationDate,
                VirtualFolder = entityModule.VirtualFolder
            };
        }

        public static List<Models.Module> CreateModules(BusinessCollection businessCollection)
        {
            List<Models.Module> modules = new List<Models.Module>();

            foreach (UniqueUser.Entities.Modules entity in businessCollection)
            {
                modules.Add(ModelAssembler.CreateModule(entity));
            }

            return modules;
        }

        public static Models.SubModule CreateSubModule(UniqueUser.Entities.Submodules subModule, Models.Module module)
        {
            return new Models.SubModule
            {
                Id = subModule.SubmoduleCode,
                Description = subModule.Description,
                IsEnabled = subModule.Enabled,
                ExpirationDate = subModule.ExpirationDate,
                VirtualFolder = subModule.VirtualFolder,
                Module = new Models.Module { Id = subModule.ModuleCode, Description = module == null ? "" : module.Description },
                ParentModuleId = subModule.ParentModuleCode == null ? 0 : (int)subModule.ParentModuleCode,
                ParentSubModuleId = subModule.ParentSubmoduleCode == null ? 0 : (int)subModule.ParentSubmoduleCode
            };
        }

        public static List<Models.SubModule> CreateSubModules(BusinessCollection businessCollection, List<Models.Module> modules)
        {
            List<Models.SubModule> subModules = new List<Models.SubModule>();

            foreach (UniqueUser.Entities.Submodules entity in businessCollection)
            {
                Models.Module module = new Models.Module();
                if (modules.Count > 0)
                {
                    module = modules.Where(x => x.Id == entity.ModuleCode).FirstOrDefault();
                }
                subModules.Add(ModelAssembler.CreateSubModule(entity, module));
            }

            return subModules;
        }


        /// <summary>
        /// Creates the module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="submodule">The submodule.</param>
        /// <returns></returns>
        public static Models.Module CreateModule(UniqueUser.Entities.Modules module, List<Models.Module> submodule)
        {
            Models.Module modelModule = new Models.Module();
            modelModule.Description = module.Description;
            modelModule.IsEnabled = module.Enabled;
            modelModule.Id = module.ModuleCode;
            modelModule.VirtualFolder = "";
            modelModule.SubModules = submodule;
            modelModule.Description = module.Description;

            return modelModule;
        }


        #region InsuredGuaranteeLog


        /// <summary>
        /// Accounts the name by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        private static string AccountNameById(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
            filter.Equal();
            filter.Constant(userId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));
            return ((UniqueUser.Entities.UniqueUsers)businessCollection[0]).AccountName;
        }

        public static List<Models.ProfileGuaranteeStatus> CreateProfilesGuaranteesStatus(BusinessCollection businessCollection)
        {
            List<Models.ProfileGuaranteeStatus> ModelguaranteeStatus = new List<Models.ProfileGuaranteeStatus>();
            foreach (UUEN.ProfileGuaranteeStatus item in businessCollection)
            {
                ModelguaranteeStatus.Add(CreateProfileGuaranteeStatus(item));
            }
            return ModelguaranteeStatus;
        }


        public static Models.ProfileGuaranteeStatus CreateProfileGuaranteeStatus(UUEN.ProfileGuaranteeStatus guaranteProfStatus)
        {
            return new Models.ProfileGuaranteeStatus()
            {
                Id = guaranteProfStatus.Id,
                ProfileId = guaranteProfStatus.ProfileId,
                Enabled = guaranteProfStatus.Enabled,
                StatusId = guaranteProfStatus.GuaranteeStatusCode 
            };
        }

        

        #endregion

        #region AccessObject
        /// <summary>
        /// Get list of Models.AccessObject
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.AccessObject</returns>
        public static List<Models.AccessObject> CreateAccessObjects(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<Models.AccessObject> accessObjects = new List<Models.AccessObject>();

            foreach (UniqueUser.Entities.AccessObjects fields in businessCollection)
            {
                Models.AccessObject accessObject = new Models.AccessObject();
                accessObject = ModelAssembler.CreateAccessObject(fields);
                accessObjects.Add(accessObject);
            }
            return accessObjects;
        }
        /// <summary>
        /// Get list of Models.AccessObject
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.AccessObject</returns>
        public static List<Models.AccessObject> CreateAccessObjects(Sistran.Core.Framework.DAF.BusinessCollection businessCollection,
            Sistran.Core.Framework.DAF.BusinessCollection businessCollectionAccess,
            Sistran.Core.Framework.DAF.BusinessCollection collectionAccessParent)
        {
            List<Models.AccessObject> accessObjects = new List<Models.AccessObject>();
            List<UniqueUser.Entities.Accesses> accessParent = new List<UniqueUser.Entities.Accesses>();

            if (collectionAccessParent != null)
            {
                accessParent = collectionAccessParent.Cast<UniqueUser.Entities.Accesses>().ToList();
            }

            foreach (UniqueUser.Entities.AccessObjects fields in businessCollection)
            {
                Models.AccessObject accessObject = new Models.AccessObject();
                accessObject = ModelAssembler.CreateAccessObject(fields);
                accessObject.AccessId = (int)businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("AccessId");

                var objectAccess = businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("ParentAccessId");
                if (objectAccess != null)
                {
                    accessObject.ParentAccessId = (int)businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("ParentAccessId");
                }
                if (accessParent.Count > 0)
                {
                    accessObject.Url = accessParent.Where(x => x.AccessId == accessObject.ParentAccessId).FirstOrDefault().Url;
                    char character = '#';
                    accessObject.Url = accessObject.Url.Split(character)[0];
                    accessObject.Description = businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("Url").ToString();
                }

                accessObjects.Add(accessObject);
            }
            return accessObjects;
        }

        /// <summary>
        /// Creates the unique AccessObject.
        /// </summary>
        /// <param name="accessObject">The access Object.</param>
        /// <returns>Models.AccessObject</returns>
        public static Models.AccessObject CreateAccessObject(UniqueUser.Entities.AccessObjects accessObject)
        {
            return new Models.AccessObject()
            {
                Description = accessObject.ObjectName,
                Id = accessObject.AccessObjectId,
                //Assigned = true
            };
        }

        /// <summary>
        /// Get list of Models.AccessObject
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.AccessObject</returns>
        public static List<Models.AccessObject> CreateAccessProfileObjects(Sistran.Core.Framework.DAF.BusinessCollection accessCollection,
            Sistran.Core.Framework.DAF.BusinessCollection accessObjectCollection,
            Sistran.Core.Framework.DAF.BusinessCollection moduleCollection,
            Sistran.Core.Framework.DAF.BusinessCollection submoduleCollection
            )
        {
            List<Models.AccessObject> accessObjects = new List<Models.AccessObject>();
            List<UniqueUser.Entities.Modules> moduleEntity = new List<UniqueUser.Entities.Modules>();
            List<UniqueUser.Entities.Submodules> submoduleEntity = new List<UniqueUser.Entities.Submodules>();

            List<UniqueUser.Entities.AccessObjects> accessObjectEntity = accessObjectCollection.Cast<UniqueUser.Entities.AccessObjects>().ToList();
            if (moduleCollection != null)
            {
                moduleEntity = moduleCollection.Cast<UniqueUser.Entities.Modules>().ToList();
            }
            if (submoduleCollection != null)
            {
                submoduleEntity = submoduleCollection.Cast<UniqueUser.Entities.Submodules>().ToList();
            }

            foreach (UniqueUser.Entities.Accesses fields in accessCollection)
            {
                Models.AccessObject model = new Models.AccessObject();
                UniqueUser.Entities.AccessObjects accessObject = accessObjectEntity.Where(x => x.AccessObjectId == fields.AccessObjectId).FirstOrDefault();
                UniqueUser.Entities.Modules module = moduleEntity.Where(x => x.ModuleCode == fields.ModuleCode).FirstOrDefault();
                UniqueUser.Entities.Submodules Submodule = submoduleEntity.Where(x => x.SubmoduleCode == fields.SubmoduleCode && x.ModuleCode == fields.ModuleCode).FirstOrDefault();
                model = ModelAssembler.CreateAccessObjectProfile(fields, accessObject, module, Submodule);
                accessObjects.Add(model);
            }
            return accessObjects;
        }
        /// <summary>
        /// Creates the unique AccessObject.
        /// </summary>
        /// <param name="accessObject">The access Object.</param>
        /// <returns>Models.AccessObject</returns>
        public static Models.AccessObject CreateAccessObjectProfile(UniqueUser.Entities.Accesses access, UniqueUser.Entities.AccessObjects accessObject, UniqueUser.Entities.Modules module, UniqueUser.Entities.Submodules submodule)
        {
            int parentId = 0;
            if (!access.IsParentAccessIdNull)
            {
                parentId = access.ParentAccessId.GetValueOrDefault();
            }
            return new Models.AccessObject()
            {
                Description = accessObject.ObjectName,
                Id = access.AccessId,
                AccessObjectId = accessObject.AccessObjectId,
                ObjectTypeId = accessObject.AccessObjTypeCode,
                SubModule = new Models.SubModule { Id = submodule.SubmoduleCode, Description = submodule.Description, Module = new Models.Module { Id = module.ModuleCode, Description = module.Description } },
                IsEnabled = access.Enabled,
                Url = access.Url,
                ParentAccessId = parentId
            };
        }
        #endregion

        #region AccessProfile
        /// <summary>
        /// Get list of Models.AccessObject
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.AccessObject</returns>
        public static List<Models.AccessProfile> CreateAccessProfiles(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<Models.AccessProfile> accessProfiles = new List<Models.AccessProfile>();

            foreach (UniqueUser.Entities.AccessProfiles fields in businessCollection)
            {
                Models.AccessProfile accessProfile = new Models.AccessProfile();
                accessProfile = ModelAssembler.CreateAccessProfile(fields);
                accessProfiles.Add(accessProfile);
            }
            return accessProfiles;
        }
        /// <summary>
        /// Creates the unique AccessProfile.
        /// </summary>
        /// <param name="accessObject">The access Profile.</param>
        /// <returns>Models.AccessProfile</returns>
        public static Models.AccessProfile CreateAccessProfile(UniqueUser.Entities.AccessProfiles accessProfile)
        {
            return new Models.AccessProfile()
            {
                AccessId = accessProfile.AccessId,
                DatabaseId = accessProfile.DatabaseId,
                Enabled = accessProfile.Enabled,
                ExpirationDate = accessProfile.ExpirationDate,
                IsExpirationDateNull = accessProfile.IsExpirationDateNull,
                ProfileId = accessProfile.ProfileId
            };
        }

        #endregion


        /// <summary>
        /// Get list of Models.AccessObject
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.AccessObject</returns>
        public static List<Models.AccessObject> CreateAccessObjectsButtons(Sistran.Core.Framework.DAF.BusinessCollection businessCollection,
            Sistran.Core.Framework.DAF.BusinessCollection businessCollectionAccess,
            Sistran.Core.Framework.DAF.BusinessCollection collectionAccessParent)
        {
            List<Models.AccessObject> accessObjects = new List<Models.AccessObject>();
            List<UniqueUser.Entities.Accesses> accessParent = new List<UniqueUser.Entities.Accesses>();

            if (collectionAccessParent != null)
            {
                accessParent = collectionAccessParent.Cast<UniqueUser.Entities.Accesses>().ToList();
            }
            foreach (UniqueUser.Entities.AccessObjects fields in businessCollection)
            {
                Models.AccessObject accessObject = new Models.AccessObject();
                accessObject = ModelAssembler.CreateAccessObject(fields);
                accessObject.AccessId = (int)businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("AccessId");

                var objectAccess = businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("ParentAccessId");
                if (objectAccess != null)
                {
                    accessObject.ParentAccessId = (int)businessCollectionAccess.Where(x => (int)x.GetProperty("AccessObjectId") == fields.AccessObjectId).First().GetProperty("ParentAccessId");
                }
                if (accessParent.Count > 0)
                {
                    accessObject.Url = accessParent.Where(x => x.AccessId == accessObject.ParentAccessId).FirstOrDefault().Url;
                }

                accessObjects.Add(accessObject);
            }
            return accessObjects;
        }

        public static AccessObject CreateAccessObjects(UUML.AccessPermissions accessPermission)
        {
            AccessObject result = new AccessObject
            {
                AccessId = accessPermission.Id,
                Description = accessPermission.Description,
                Assigned = false
            };
            return result;
        }

        public static List<Models.AccessObject> CreateAccessObjects(List<UUML.AccessPermissions> accessPermissionsDAOs )
        {
            return accessPermissionsDAOs.Select(CreateAccessObjects).ToList();
        }

        #region Branch
        public static Branch CreateBranch(UserBranch entityUserBranch, COMMEN.Branch entityBranch)
        {
            return new Branch()
            {
                Id = entityBranch.BranchCode,
                Description = entityBranch.Description,
                SmallDescription = entityBranch.SmallDescription
            };
        }

        #endregion

        #region Branch

        public static Branch CreateBranch(COMMEN.Branch entityBranch, UserBranch entityUser)
        {
            return new Branch
            {
                Id = entityBranch.BranchCode,
                Description = entityBranch.Description,
                SalePoints = new List<SalePoint>(),
                IsDefault = entityUser != null ? (entityUser.DefaultBranch == 0 ? false : true) : false
            };
        }

        public static List<Branch> CreateBranches(BusinessCollection businessCollection, BusinessCollection userBranchCollection)
        {
            List<Branch> branches = new List<Branch>();
            List<UserBranch> userbranches = new List<UserBranch>();
            if (userBranchCollection != null)
            {
                userbranches = userBranchCollection.Cast<UserBranch>().ToList();
            }
            foreach (COMMEN.Branch entity in businessCollection)
            {
                UserBranch userBranch = userbranches.Where(x => x.BranchCode == entity.BranchCode).FirstOrDefault();
                branches.Add(ModelAssembler.CreateBranch(entity, userBranch));
            }

            return branches;
        }

        #endregion



        #region SalePoint        

        public static List<Branch> CreateBranchWithSalePoint(List<Branch> branchs, BusinessCollection userSalesPointCollection, BusinessCollection salesPointCollection)
        {
            List<COMMEN.SalePoint> sales = salesPointCollection.Cast<COMMEN.SalePoint>().ToList();
            foreach (UserSalePoint entity in userSalesPointCollection)
            {
                COMMEN.SalePoint entitySalePoint = sales.Where(x => x.BranchCode == entity.BranchCode && x.SalePointCode == entity.SalePointCode).FirstOrDefault();
                Branch branch = branchs.Where(x => x.Id == entity.BranchCode).FirstOrDefault();
                branchs.Remove(branch);
                if (branch != null && entitySalePoint != null)
                {
                    branch.SalePoints.Add(ModelAssembler.CreateSalePoint(entitySalePoint, entity));
                    branchs.Add(branch);
                }
            }

            return branchs;
        }

        #endregion

        /// <summary>
        /// Mapea una lista de entidades UniqueUsersProduct a una lista de modelos de negocio UniqueUsersProduct.
        /// </summary>
        /// <param name="businessCollection">Lista de entidades UniqueUsersProduct.</param>
        /// <returns>Lista de modelos de negocio UniqueUsersProduct.</returns>
        public static List<UUML.UniqueUsersProduct> MappUniqueUsersProductList(BusinessCollection businessCollection)
        {
            List<UUML.UniqueUsersProduct> uniqueUsersProductList = new List<UUML.UniqueUsersProduct>();

            foreach (UniqueUser.Entities.UniqueUsersProduct uniqueUsersProductEntity in businessCollection)
            {
                uniqueUsersProductList.Add(ModelAssembler.MappUniqueUsersProduct(uniqueUsersProductEntity));
            }

            return uniqueUsersProductList;
        }

        /// <summary>
        /// Mapea una entidad de tipo UniqueUsersProduct a modelo de negocio UniqueUsersProduct.
        /// </summary>
        /// <param name="uniqueUsersProductEntity">Entidad de tipo UniqueUsersProduct.</param>
        /// <returns>Modelo de negocio UniqueUsersProduct.</returns>
        public static UUML.UniqueUsersProduct MappUniqueUsersProduct(UniqueUser.Entities.UniqueUsersProduct uniqueUsersProductEntity)
        {

            return new Models.UniqueUsersProduct
            {
                UserId = uniqueUsersProductEntity.UserId,
                ProductId = uniqueUsersProductEntity.ProductId,
                PrefixCode = uniqueUsersProductEntity.PrefixCode,
                Enabled = (bool)uniqueUsersProductEntity.Enabled,
                Assign = (bool)uniqueUsersProductEntity.Assign
            };
        }

        public static UUML.PrefixUser CreatePrefixUser(COMMEN.PrefixUser entityPrefixUser)
        {
            return new UUML.PrefixUser
            {
                UserId = entityPrefixUser.PrefixCode
            };
        }

        #region ACCESS_PERMISSIONS
        /// <summary>
        /// Convierte un objeto de tipo Models.AccessPermissions a UUEN.Entities.AccessPermissions
        /// </summary>
        /// <param name="accessPermissions">Objeto del tipo Models.AccessPermissions</param>
        /// <returns>Retorna un objeto del tipo UUEN.Entities.AccessPermissions</returns>
        public static Models.AccessPermissions CreateAccessPermission(BusinessObject businessObject)
        {
            UUEN.AccessPermissions accessPermissions = (UUEN.AccessPermissions)businessObject;
            return new Models.AccessPermissions()
            {
                Id = accessPermissions.Id,
                Description = accessPermissions.Description,
                Code = accessPermissions.Code
            };
        }

        /// <summary>
        /// Convierte un objeto de tipo Models.AccessPermissions a UUEN.Entities.AccessPermissions
        /// </summary>
        /// <param name="accessPermissions">Objeto del tipo Models.AccessPermissions</param>
        /// <returns>Retorna un objeto del tipo UUEN.Entities.AccessPermissions</returns>
        public static List<Models.AccessPermissions> CreateAccessPermissions(BusinessCollection accessPermissions)
        {
            return accessPermissions.Select(CreateAccessPermission).ToList();
        }

        public static Models.ProfileAccesPermissions CreateProfileAccessPermission(BusinessObject businessObject)
        {
            UUEN.ProfileAccessPermissions profileaccessPermissions = (UUEN.ProfileAccessPermissions)businessObject;
            return new Models.ProfileAccesPermissions()
            {
                Id = profileaccessPermissions.Id,
                AccessPermissionsId = profileaccessPermissions.AccessPermissionsCode,
                ProfileId = profileaccessPermissions.ProfileCode
            };
        }
        
        public static List<Models.ProfileAccesPermissions> CreateProfileAccessPermissions(BusinessCollection profileaccessPermissions)
        {
            return profileaccessPermissions.Select(CreateProfileAccessPermission).ToList();
        }

        

        public static List<Models.SecurityContext> CreateContexts(BusinessCollection contextPermissions)
        {
            return contextPermissions.Select(CreateContext).ToList();
        }

        public static Models.SecurityContext CreateContext(BusinessObject businessObject)
        {
            UUEN.SecurityContext profileaccessPermissions = (UUEN.SecurityContext)businessObject;
            return new Models.SecurityContext()
            {
                Id = profileaccessPermissions.Id,
                Code = profileaccessPermissions.Code,
                Description = profileaccessPermissions.Description
            };
        }

        public static List<Models.ContextProfileAccessPermissions> CreateContextPermissions(BusinessCollection contextPermissions)
        {
            return contextPermissions.Select(CreateContextPermission).ToList();
        }

        public static Models.ContextProfileAccessPermissions CreateContextPermission(BusinessObject businessObject)
        {
            UUEN.UserContextPermission contextPermissions = (UUEN.UserContextPermission)businessObject;
            return new Models.ContextProfileAccessPermissions()
            {
                Id = contextPermissions.Id,
                Profile = new UUML.Profile { Id = contextPermissions.ProfileCode },
                AccessPermission = new UUML.AccessPermissions { Id = contextPermissions.PermissionCode },
                SecurityContext = new UUML.SecurityContext { Id = contextPermissions.ContextPermissionsCode }
            };
        }

        

        public static List<Models.SecurityContext> CreateSecurityContexts(BusinessCollection SecurityContextS)
        {
            return SecurityContextS.Select(CreateSecurityContext).ToList();
        }

        public static Models.SecurityContext CreateSecurityContext(BusinessObject businessObject)
        {
            UUEN.SecurityContext SecurityContext = (UUEN.SecurityContext)businessObject;
            return new Models.SecurityContext()
            {
                Id = SecurityContext.Id,
                Code = SecurityContext.Code,
                Description = SecurityContext.Description

            };
        }

        public static List<Models.ContextProfileAccessPermissions> CreateContextsPermisions(List<Models.SecurityContext> SecurityContexts)
        {
            return SecurityContexts.Select(CreateContextPermisions).ToList();
        }


        public static Models.ContextProfileAccessPermissions CreateContextPermisions(Models.SecurityContext securityContext)
        {
            
            return new Models.ContextProfileAccessPermissions()
            {
                SecurityContext = securityContext
            };
        }

        public static Models.UniqueUserSession CreateUniqueUserSession(BusinessObject businessObject)
        {
            UUEN.UniqueUserSession uniqueUserSession = (UUEN.UniqueUserSession)businessObject;
            return new Models.UniqueUserSession()
            {
                UserId = uniqueUserSession.UserId,
                ExpirationDate = uniqueUserSession.ExpirationDate,
                LastUpdateDate = uniqueUserSession.LastUpdateDate,
                RegistrationDate = uniqueUserSession.RegistrationDate
            };
        }


        #endregion

        internal static List<UPEN.Company> CreateCompanies(BusinessCollection businessObjects)
        {
            List<UPEN.Company> companies = new List<UPEN.Company>();

            foreach (UPEN.Company company in businessObjects)
            {
                companies.Add(ModelAssembler.CreateCompany(company));
            }

            return companies;
        }

        private static UPEN.Company CreateCompany(UPEN.Company company)
        {
            return new UPEN.Company
            {
                IndividualId = company.IndividualId,
            };
        }

        internal static List<UPEN.Person> CreatePeoples(BusinessCollection businessObjectsPerson)
        {
            List<UPEN.Person> persons = new List<UPEN.Person>();

            foreach (UPEN.Person person in businessObjectsPerson)
            {
                persons.Add(ModelAssembler.CreatePeople(person));
            }

            return persons;
        }

        private static UPEN.Person CreatePeople(UPEN.Person person)
        {
            return new UPEN.Person
            {
                IndividualId = person.IndividualId,
            };
        }
    }
}
