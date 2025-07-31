using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.EEProvider.Constant;
using Sistran.Core.Application.EEProvider.Helper;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Resources;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using EtPerson = Sistran.Core.Application.UniquePerson.Entities;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao User
    /// </summary>
    public class UserDAO
    {

        UserBranchDAO userbranchDao = new UserBranchDAO();
        /// <summary>
        /// Get User By AccountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>User</returns>
        public User GetUserByAccountNameUserId(string accountName, int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (userId != 0)
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId);
                filter.Equal();
                filter.Constant(userId);
            }
            else
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
                filter.Equal();
                filter.Constant(accountName);
            }

            //Crear una vista con la tabla de personas para obtener el nombre en la misma consulta de los usuarios

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));

            List<Models.User> users = ModelAssembler.CreateUniqueUsers(businessCollection);

            //foreach (Models.User user in users)
            //{
            //    UniquePersonDAO personDao = new UniquePersonDAO();
            //    Person personModel = personDao.GetPersonByUserIdOrPersonId(0, user.PersonId);
            //    user.Name = personModel.Name.ToString() + " " + personModel.Surname.ToString();
            //}

            return users.FirstOrDefault();

        }

        /// <summary>
        /// Get User By AccountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>User</returns>
        public List<User> GetUserByTextPersonId(string text, int personId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (personId != 0)
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId);
                filter.Equal();
                filter.Constant(personId);
            }
            else
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
                filter.Like();
                filter.Constant("%" + text + "%");
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));
            return ModelAssembler.CreateUniqueUsers(businessCollection);
        }

        /// <summary>
        /// Get User By AccountName or personId
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <param name="personId">personId</param>
        /// <returns>Models User</returns>
        public List<User> GetUserByAccountName(string accountName, int personId, int userId, bool getAllData)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (userId > 0)
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(userId);
            }
            if (accountName != "")
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Like();
                filter.Constant("%" + accountName + "%");
            }
            if (personId != 0)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(personId);
            }

            UniqueUserView view = new UniqueUserView();
            ViewBuilder builder = new ViewBuilder("UniqueUserView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<User> users = new List<User>();
            if (view.UniqueUsers.Count == 1)
            {
                User user = new User();
                if (view.Persons.Count > 0)
                {
                    user = ModelAssembler.CreateUniqueUsers(view.UniqueUsers, view.Persons).FirstOrDefault();
                }
                else
                {
                    user = ModelAssembler.CreateUniqueUsers(view.UniqueUsers).FirstOrDefault();
                }
                if (getAllData)
                {
                    user.Profiles = ModelAssembler.CreateProfiles(view.Profiles);
                    user.Hierarchies = ModelAssembler.CreateHierarchiesAssociation(view.CoHierarchiesAssociation);
                    user.Branch = userbranchDao.GetBranchesByUserId(user.UserId);

                    user.IndividualsRelation = ModelAssembler.CreateIndividualsRelationApp(view.IndividualsRelationApp);

                    List<UniqueUser.Entities.Modules> modules = new List<UniqueUser.Entities.Modules>();
                    List<UniqueUser.Entities.Submodules> subModules = new List<UniqueUser.Entities.Submodules>();
                    List<EtPerson.AgentAgency> agencies = new List<EtPerson.AgentAgency>();

                    modules = view.Modules.Cast<UniqueUser.Entities.Modules>().ToList();
                    subModules = view.SubModules.Cast<UniqueUser.Entities.Submodules>().ToList();
                    agencies = view.AgentAgencies.Cast<EtPerson.AgentAgency>().ToList();

                    foreach (Models.CoHierarchyAssociation item in user.Hierarchies)
                    {
                        item.Module.Description = modules.Where(x => x.ModuleCode == item.Module.Id).FirstOrDefault().Description;
                        item.SubModule.Description = subModules.Where(x => x.ModuleCode == item.Module.Id).FirstOrDefault().Description;
                    }

                    foreach (IndividualRelationApp item in user.IndividualsRelation)
                    {
                        item.Agency.Agent.FullName = agencies.Where(x => x.IndividualId == item.Agency.Agent.IndividualId
                            && x.AgentAgencyId == item.Agency.Id).FirstOrDefault().Description;
                        item.Agency.Code = agencies.Where(x => x.AgentAgencyId == item.Agency.Id).FirstOrDefault().AgentCode;
                    }

                    user.Branch = ModelAssembler.CreateBranchWithSalePoint(user.Branch, view.UserSalesPoint, view.SalesPoint);
                    user.UniqueUsersLogin = GetUniqueUserLogin(user.UserId);
                }
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetUserByAccountName");
                users.Add(user);
                return users;

            }
            if (view.UniqueUsers.Count > 0)
            {
                users = ModelAssembler.CreateUniqueUsers(view.UniqueUsers, view.Persons);
                return users;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Save User
        /// </summary>
        /// <param name="User">Model User</param>
        /// <returns>User</returns>
        public int CreateUniqueUser(User user)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            User findUser = GetUserByAccountNameUserId(user.AccountName, user.UserId);
            UniqueUser.Entities.UniqueUsers userEntity = new UniqueUser.Entities.UniqueUsers();

            if (findUser != null)
            {
                PrimaryKey key = UniqueUser.Entities.UniqueUsers.CreatePrimaryKey(findUser.UserId);
                userEntity = (UniqueUser.Entities.UniqueUsers)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                findUser = null;
                if (userEntity.AccountName != user.AccountName)
                {
                    //valida usuario existente
                    findUser = GetUserByAccountNameUserId(user.AccountName, 0);
                }
                if (findUser == null)
                {
                    userEntity.AccountName = user.AccountName;
                    userEntity.ActivationDate = false;
                    userEntity.DisabledDate = user.DisableDate;
                    userEntity.ExpirationDate = user.ExpirationDate;
                    userEntity.PersonId = user.PersonId;
                    userEntity.UserDomain = user.UserDomain;
                    userEntity.LockDate = user.LockDate;
                    userEntity.ModifiedDate = user.LastModificationDate;
                    userEntity.ModifiedUserId = user.ModifiedUserId;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(userEntity);
                }
            }
            else
            {
                if (user.AccountName != "" && user.PersonId != 0)
                {
                    userEntity = EntityAssembler.CreateUser(user);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(userEntity);
                }
            }

            //update hierarchy
            CoHierarchyAccessDAO coHierarchyAccessDAO = new CoHierarchyAccessDAO();
            coHierarchyAccessDAO.DeleteCoHierarchyAccessByUserId(userEntity.UserId);
            if (user.Hierarchies != null)
            {
                coHierarchyAccessDAO.CreateCoHierarchyAccessByUserId(user.Hierarchies, userEntity.UserId);
            }
            else
            {
                coHierarchyAccessDAO.DeleteCoHierarchyAccessByUserId(userEntity.UserId);
            }

            //update profiles
            if (user.Profiles != null && user.Profiles.Any())
            {
                ProfileUserDAO profileDAO = new ProfileUserDAO();
                profileDAO.CreateProfiles(user.Profiles, userEntity.UserId);
            }
            UserSalePointDAO salePointDAO = new UserSalePointDAO();
            salePointDAO.DeleteUserSalesPointByUserId(userEntity.UserId, 0);
            UserBranchDAO userBranchDAO = new UserBranchDAO();
            userBranchDAO.DeleteUserBranchByUserId(userEntity.UserId, 0);
            //update SalesPoint and branch
            IndividualRelationAppDAO individualRelationAppDAO = new IndividualRelationAppDAO();
            if (user.Branch != null && user.Branch.Any())
            {
                salePointDAO.CreateBranchs(user.Branch, userEntity.UserId);
            }
            //update IndividualsRelation
            if (user.IndividualsRelation != null && user.IndividualsRelation.Any())
            {
                individualRelationAppDAO.SaveIndividualRelationApp(user.IndividualsRelation);
            }
            else
            {
                individualRelationAppDAO.DeleteIndividualRelationAppByUserId(user.PersonId);
            }
            if (user.UniqueUsersLogin != null)
            {
                UniqueUsersLoginDAO uniqueUsersLoginDAO = new UniqueUsersLoginDAO();
                user.UniqueUsersLogin.UserId = userEntity.UserId;
                Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("DAYS_EXPIRATION_PASSWORD");
                if (parameter.NumberParameter >= 0 && user.UniqueUsersLogin.ExpirationDate == null)
                {
                    user.UniqueUsersLogin.ExpirationDate = DateTime.Now.AddDays(parameter.NumberParameter.GetValueOrDefault());
                }
                uniqueUsersLoginDAO.CreateUniqueUsersLogin(user.UniqueUsersLogin);
            }
            ////User products
            //if (user.UniqueUsersProduct != null)
            //{
            //    foreach (UniqueUsersProduct uniqueUsersProduct in user.UniqueUsersProduct)
            //    {
            //        uniqueUsersProduct.UserId = userEntity.UserId;
            //    }
            //    UniqueUsersProductDAO uniqueUsersProductDAO = new UniqueUsersProductDAO();
            //    uniqueUsersProductDAO.SaveUniqueUsersProduct(user.UniqueUsersProduct, userEntity.UserId);
            //}

            if (user.Prefixes != null && user.Prefixes.Count() > 0)
            {
                PrefixUserDAO prefixUserDAO = new PrefixUserDAO();
                prefixUserDAO.SavePrefixes(user.Prefixes, userEntity.UserId);
            }

            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.SaveUniqueUser");
            return userEntity.UserId;
        }

        public Models.UniqueUserLogin GetUniqueUserLogin(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUserLogin.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUserLogin), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreateUniqueUserLogin(businessCollection);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Bloqueo de usuario
        /// </summary>
        /// <param name="userName">userName</param>
        public static void UpdateLockPasswordByUserName(string userName)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
            filter.Equal();
            filter.Constant(userName);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));

            UniqueUser.Entities.UniqueUsers entity = businessCollection.Cast<UniqueUser.Entities.UniqueUsers>().FirstOrDefault();
            //Se bloquea el usuario para todos los intentos de sesión a partir de hoy 00:00
            entity.LockDate = DateTime.Now.AddDays(-1);
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
        }

        /// <summary>
        /// Listado de Usuario Por Filtro
        /// </summary>
        /// <param name="user">Filtro</param>
        /// <returns>Users</returns>
        public List<User> GetUsersByUser(User user)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (user.AccountName != null)
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
                filter.Equal();
                filter.Constant(user.AccountName);
            }
            if (user.CreationDate != null)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.CreatedDate);
                filter.Equal();
                filter.Constant(user.CreationDate);
            }
            if (user.CreatedUserId == (int)EnabledStatus.Disabled)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.DisabledDate);
                filter.Less();
                filter.Constant(DateTime.Now.AddDays(1));
            }
            if (user.CreatedUserId == (int)EnabledStatus.Enabled)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.DisabledDate);
                filter.IsNull();
            }
            if (user.DisableDate != null)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.DisabledDate);
                filter.IsNotNull();
            }
            if (user.LastModificationDate != null)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.ModifiedDate);
                filter.Equal();
                filter.Constant(user.LastModificationDate);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));

            return ModelAssembler.CreateUniqueUsers(businessCollection);
        }


        /// <summary>
        /// Obtiene el id del usuario  a travez del nombre
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetUserId(string name)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
            filter.Equal();
            filter.Constant(name);
            BusinessCollection users = new
                            BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));
            if (users != null)
            {
                foreach (UniqueUser.Entities.UniqueUsers uniqueUser in users)
                {
                    return uniqueUser.UserId;
                }
            }
            return 0;
        }

        public List<User> GetUserByName(string name)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
            filter.Like();
            filter.Constant(name + "%");
            BusinessCollection users = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));
            if (users != null)
            {
                return ModelAssembler.CreateUniqueUsers(users);
            }
            return new List<User>();
        }

        public List<User> GetCountUsersById(int userId)
        {
            Models.User user = new Models.User();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.DisabledDate);
            filter.IsNull();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));

            List<Models.User> users = ModelAssembler.CreateUniqueUsers(businessCollection);
            return users;
        }

        /// <summary>
        /// Obtiene el usuario por el Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            PrimaryKey key = UniqueUser.Entities.UniqueUsers.CreatePrimaryKey(userId);
            UniqueUser.Entities.UniqueUsers userEntity = (UniqueUser.Entities.UniqueUsers)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return ModelAssembler.CreateUniqueUser(userEntity);
        }

        /// <summary>
        /// Asigna todos los branch por usuario
        /// </summary>   
        public void AssingAllBranchByUserId(int userId, bool active)
        {
            UserSalePointDAO salePointDAO = new UserSalePointDAO();
            if (active)
            {
                salePointDAO.CreateBranchs(DelegateService.commonServiceCore.GetBranches(), userId);
                this.AssingDefaultBranchByUserId(userId);
            }
            else
            {
                salePointDAO.DeleteUserSalesPointByUserId(userId, 0);
                salePointDAO.DeleteUserBranchByUserId(userId);
            }
        }
        /// <summary>
        /// Asigna un branch por defecto al usuario
        /// </summary> 
        public void AssingDefaultBranchByUserId(int userId)
        {
            UpdateQuery update = new UpdateQuery();
            update.Table = new ClassNameTable(typeof(UUEN.UserBranch));
            update.ColumnValues.Add(new Column(UUEN.UserBranch.Properties.DefaultBranch), new Constant(0, System.Data.DbType.String));

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserSalePoint.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);
            update.Where = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(update);

            if (userbranchDao.GetBranchesByUserId(userId).FirstOrDefault() != null)
            {
                int branchId = userbranchDao.GetBranchesByUserId(userId).FirstOrDefault().Id;

                update = new UpdateQuery();
                update.Table = new ClassNameTable(typeof(UUEN.UserBranch));
                update.ColumnValues.Add(new Column(UUEN.UserBranch.Properties.DefaultBranch), new Constant(1, System.Data.DbType.String));

                filter = new ObjectCriteriaBuilder();
                filter.Property(UUEN.UserSalePoint.Properties.UserId);
                filter.Equal();
                filter.Constant(userId);
                filter.And();
                filter.Property(UUEN.UserSalePoint.Properties.BranchCode);
                filter.Equal();
                filter.Constant(branchId);
                update.Where = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().Execute(update);
            }

        }

        /// <summary>
        /// Consulta los usuarios en la tabla CoHierarchyAccesses segun los parametros
        /// </summary>
        /// <param name="idHierarchy">id de jerarquia</param>
        /// <param name="idModule">id modulo</param>
        /// <param name="idSubmodule">id del submodulo</param>
        /// <returns></returns>
        public List<Models.User> GetUsersByHierarchyModuleSubmodule(int idHierarchy, int idModule, int idSubmodule)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UUEN.UniqueUsers.Properties.UserId, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEN.UniqueUsers.Properties.AccountName, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEN.UniqueUsers.Properties.DisabledDate, "uu")));

            Join join = new Join(new ClassNameTable(typeof(UUEN.UniqueUsers), "uu"), new ClassNameTable(typeof(UUEN.CoHierarchyAccesses), "cha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEN.UniqueUsers.Properties.UserId, "uu")
                    .Equal().Property(UUEN.CoHierarchyAccesses.Properties.UserId, "cha").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(UUEN.CoHierarchyAccesses.Properties.HierarchyCode, "cha").Equal().Constant(idHierarchy);
            where.And().Property(UUEN.CoHierarchyAccesses.Properties.ModuleCode, "cha").Equal().Constant(idModule);
            where.And().Property(UUEN.CoHierarchyAccesses.Properties.SubmoduleCode, "cha").Equal().Constant(idSubmodule);

            select.Table = join;
            select.Where = where.GetPredicate();
            select.AddSortValue(new SortValue(new Column(UUEN.UniqueUsers.Properties.AccountName), SortOrderType.Ascending));

            var users = new List<Models.User>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = (int)reader["UserId"],
                        AccountName = (string)reader["AccountName"],
                        DisableDate = reader["DisabledDate"] != null ? DateTime.Parse(reader["DisabledDate"].ToString()) : (DateTime?)null
                    });
                }
            }
            return users;
        }/// <summary>
         /// Realiza las validaciones paramétricas para el cambio de contraseña 
         /// </summary>
         /// <param name="login"></param>
         /// <param name="oldPassword"></param>
         /// <param name="newPassword"></param>
         /// <param name="errors"></param>
         /// <returns>Verdadero sis cambió la contraseña</returns>
        public Models.User GetUserByLogin(string login)
        {
            int integrated =
                (int)PublicConstants.AUTHENTICATION_CODE.Integrated;
            int standard =
                (int)PublicConstants.AUTHENTICATION_CODE.Standard;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.OpenParenthesis();
            filter.Property("AuthenticationTypeCode");
            filter.Equal();
            filter.Constant(integrated);
            filter.And();
            filter.Property("UserDomain");
            filter.Equal();
            filter.Constant(CommonUtilitiesUser.GetDomainFromLoginName(login));
            filter.And();
            filter.Property("AccountName");
            filter.Equal();
            filter.Constant(CommonUtilitiesUser.GetDomainFromLoginName(login) + "\\" + CommonUtilitiesUser.GetNickFromLoginName(login));
            filter.CloseParenthesis();
            filter.Or();
            filter.OpenParenthesis();
            filter.Property("AuthenticationTypeCode");
            filter.Equal();
            filter.Constant(standard);
            filter.And();
            filter.Property("AccountName");
            filter.Equal();
            filter.Constant(CommonUtilitiesUser.GetNickFromLoginName(login));
            filter.CloseParenthesis();
            UUEN.UniqueUsers user;
            user = (UUEN.UniqueUsers)DataFacadeManager.Instance.GetDataFacade().List(typeof(UUEN.UniqueUsers), filter.GetPredicate()).FirstOrDefault();

            if (user == null)
            {
                throw new BusinessException(Errors.UserNotFound);
            }

            return ModelAssembler.CreateUniqueUser(user);
        }

        /// <summary>
        /// Get User By individualId
        /// </summary>
        /// <param name="individualId">individual Id</param>
        /// <returns>retorna el usuario</returns>
        public User GetUserByIndividualId(int individualId)
        {
            Models.User user = new Models.User();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));

            List<Models.User> users = ModelAssembler.CreateUniqueUsers(businessCollection);
            user = users.First();

            UniquePersonDAO personDao = new UniquePersonDAO();
            UserPerson personModel = personDao.GetPersonByUserIdOrPersonId(0, user.PersonId);
            user.Name = personModel.FullName;

            return users.FirstOrDefault();
        }

        /// <summary>
        /// Obtiene los usuario de acuerdo a los filtros
        /// </summary>
        /// <param name="accountName">Nombre de usuario</param>
        /// <param name="userId">Id de usuario</param>
        /// <param name="identificationNumber">Nro de identificacion de la persona</param>
        /// <param name="creationDate">Fecha de creacion</param>
        /// <param name="status">Estado del usuario (Activado, Desactivado o Todos)</param>
        /// <param name="lastModificationDate">Ultima Fecha de modificacion</param>
        /// <returns>Listado de usuarios</returns>
        public List<User> GetUserByAdvancedSearch(string accountName, int? userId, string identificationNumber, DateTime? creationDate, int? status, DateTime? lastModificationDate)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(accountName))
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
                filter.Equal();
                filter.Constant(accountName);
            }
            if (creationDate != null)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.CreatedDate);
                filter.Equal();
                filter.Constant(creationDate);
            }
            if (status == (int)EnabledStatus.Disabled)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.DisabledDate);
                filter.Less();
                filter.Constant(DateTime.Now.AddDays(1));
            }
            if (status == (int)EnabledStatus.Enabled)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.DisabledDate);
                filter.IsNull();
            }
            if (lastModificationDate != null)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.ModifiedDate);
                filter.Equal();
                filter.Constant(lastModificationDate);
            }
            if (!string.IsNullOrEmpty(identificationNumber))
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, typeof(EtPerson.Person).Name);
                filter.Equal();
                filter.Constant(identificationNumber);
            }
            if (userId != null)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId);
                filter.Equal();
                filter.Constant(userId);
            }

            UniqueUserPersonView view = new UniqueUserPersonView();
            ViewBuilder builder = new ViewBuilder("UniqueUserPersonView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<User> users = ModelAssembler.CreateUniqueUsers(view.UniqueUser);
            List<UserPerson> persons = ModelAssembler.CreatePersons(view.People);
            foreach (User user in users)
            {
                UserPerson person = persons.Find(p => p.Id == user.PersonId);
                if (person != null)
                {
                    user.Name = person.FullName;
                }
            }
            return users;
        }

        /// <summary>
        /// Get User By AccountName 
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>Models User</returns>
        public List<User> GetUsersByAccountName(string accountName, int userId, int personId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Models.User> users = new List<User>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (userId != 0)
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId);
                filter.Equal();
                filter.Constant(userId);
            }
            else
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
                filter.Like();
                filter.Constant("%" + accountName + "%");
            }
            if (personId != 0)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(personId);
            }
            BusinessCollection businessCollection = new
                            BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));
            if (businessCollection != null)
            {
                users = ModelAssembler.CreateUniqueUsers(businessCollection);
                if (users.Count == 1)
                {
                    if (users[0].PersonId != 0)
                    {
                        UniquePersonDAO uniquePersonDAO = new UniquePersonDAO();
                        UserPerson person = uniquePersonDAO.GetPersonByUserIdOrPersonId(users[0].UserId, users[0].PersonId);
                        if (person != null)
                        {
                            users[0].Name = person.FullName;
                        }

                    }
                    //Perfiles
                    ProfileUserDAO profileUserDAO = new ProfileUserDAO();
                    List<UniqueUser.Entities.ProfileUniqueUser> entities = profileUserDAO.GetProfilesByUserId(users[0].UserId);
                    users[0].Profiles = new List<Profile>();
                    foreach (UniqueUser.Entities.ProfileUniqueUser item in entities)
                    {
                        users[0].Profiles.Add(new Profile { Id = item.ProfileId });
                    }

                    //Jerarquias
                    CoHierarchyAccessDAO coHierarchyAccessDAO = new CoHierarchyAccessDAO();
                    List<CoHierarchyAssociation> access = coHierarchyAccessDAO.GetHierarchiesAccessByUserId(users[0].UserId);
                    users[0].Hierarchies = access;

                    //contraseña
                    users[0].UniqueUsersLogin = GetUniqueUserLogin(users[0].UserId);

                    //Sucursales
                    UserBranchDAO userBranchDAO = new UserBranchDAO();

                    filter = new ObjectCriteriaBuilder();
                    filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UserSalePoint).Name);
                    filter.Equal();
                    filter.Constant(users[0].UserId);

                    users[0].Branch = userbranchDao.GetBranchesByUserId(users[0].UserId);
                    UserSalePointView view = new UserSalePointView();
                    ViewBuilder builder = new ViewBuilder("UserSalePointView");
                    builder.Filter = filter.GetPredicate();
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.FillView(builder, view);
                    }
                    if (view.UserSalesPoint != null && view.SalesPoint != null)
                    {
                        users[0].Branch = ModelAssembler.CreateBranchWithSalePoint(users[0].Branch, view.UserSalesPoint, view.SalesPoint);
                    }

                    //Intermediarios
                    if (users[0].PersonId != 0)
                    {
                        filter = new ObjectCriteriaBuilder();
                        filter.Property(EtPerson.IndividualRelationApp.Properties.ParentIndividualId, typeof(EtPerson.IndividualRelationApp).Name);
                        filter.Equal();
                        filter.Constant(users[0].PersonId);

                        AgencyIndividualRelationUUView viewRelation = new AgencyIndividualRelationUUView();
                        ViewBuilder builderRelation = new ViewBuilder("AgencyIndividualRelationUUView");
                        builderRelation.Filter = filter.GetPredicate();
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            daf.FillView(builderRelation, viewRelation);
                        }
                        users[0].IndividualsRelation = ModelAssembler.CreateIndividualsRelationApp(viewRelation.IndividualRelationsApp);

                        List<EtPerson.AgentAgency> agencies = new List<EtPerson.AgentAgency>();
                        agencies = viewRelation.AgentAgencies.Cast<EtPerson.AgentAgency>().ToList();
                        foreach (IndividualRelationApp item in users[0].IndividualsRelation)
                        {
                            item.Agency.Agent.FullName = agencies.Where(x => x.IndividualId == item.Agency.Agent.IndividualId
                                && x.AgentAgencyId == item.Agency.Id).FirstOrDefault().Description;
                            item.Agency.Code = agencies.Where(x => x.AgentAgencyId == item.Agency.Id).FirstOrDefault().AgentCode;
                        }

                    }
                }
                else
                {
                    foreach (Models.User user in users)
                    {
                        UniquePersonDAO personDao = new UniquePersonDAO();
                        if (user.PersonId != 0)
                        {
                            UniquePersonDAO uniquePersonDAO = new UniquePersonDAO();
                            UserPerson person = uniquePersonDAO.GetPersonByUserIdOrPersonId(user.UserId, user.PersonId);
                            user.Name = person?.FullName ?? "";
                        }
                    }
                }

            }
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "-------GetUserByAccountName--------");
            return users;
        }
        public List<User> GetUsers()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(UUEN.UniqueUsers), new[] { UUEN.UniqueUsers.Properties.AccountName }));

            return ModelAssembler.CreateUniqueeUsers(businessCollection.Cast<UUEN.UniqueUsers>().ToList());

        }

    }
}