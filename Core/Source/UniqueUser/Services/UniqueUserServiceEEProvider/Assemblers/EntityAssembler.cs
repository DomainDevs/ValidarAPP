
using Sistran.Core.Application.UniqueUserServices.EEProvider.Helper;
using System.Collections.Generic;
using MdCommon = Sistran.Core.Application.CommonService.Models;
using Model = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using ENUP = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.DAF;
using System;
using COMMEN = Sistran.Core.Application.Common.Entities;
using System.Linq;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {

        /// <summary>
        /// Converts to entities UniqueUser.
        /// </summary>
        /// <param name="uniqueUser">The uniqueUser.</param>
        /// <returns>UniqueUser</returns>
        public static UniqueUser.Entities.UniqueUsers CreateUser(Model.User uniqueUser)
        {
            UniqueUser.Entities.UniqueUsers userEntities = new UniqueUser.Entities.UniqueUsers()
            {
                AccountName = uniqueUser.AccountName,
                ActivationDate = false,
                AuthenticationTypeCode = (int)uniqueUser.AuthenticationType,
                CreatedDate = uniqueUser.CreationDate,
                CreatedUserId = uniqueUser.CreatedUserId,
                DisabledDate = uniqueUser.DisableDate,
                ExpirationDate = uniqueUser.ExpirationDate,
                PersonId = uniqueUser.PersonId,
                UserDomain = uniqueUser.UserDomain,
                LockDate = uniqueUser.LockDate,
                LockPassword = false,
                ModifiedDate = uniqueUser.LastModificationDate,
                ModifiedUserId = uniqueUser.ModifiedUserId
            };
            return userEntities;
        }

        internal static BusinessObject CreatePrefixUser(MdCommon.Prefix prefix, int userId)
        {
            return new COMMEN.PrefixUser(userId, prefix.Id);
        }

        #region CoHierarchyAccess
        /// <summary>
        /// Converts to entities CoHierarchyAccess.
        /// </summary>
        /// <param name="hierarchy">The CoHierarchyAssociation.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>CoHierarchyAccess</returns>
        public static UniqueUser.Entities.CoHierarchyAccesses CreateCoHierarchyAccess(Model.CoHierarchyAssociation hierarchy, int userid)
        {
            UniqueUser.Entities.CoHierarchyAccesses coHierarchyAccess = new UniqueUser.Entities.CoHierarchyAccesses()
            {
                UserId = userid,
                ModuleCode = hierarchy.Module.Id,
                SubmoduleCode = hierarchy.SubModule.Id,
                HierarchyCode = hierarchy.Id
            };
            return coHierarchyAccess;
        }

        #endregion

        #region ProfileUniqueUser

        /// <summary>
        /// Converts to entities ProfileUniqueUser.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>ProfileUniqueUser</returns>
        public static UniqueUser.Entities.ProfileUniqueUser CreateProfileUniqueUser(Model.Profile profile, int userid)
        {
            UniqueUser.Entities.ProfileUniqueUser profileUniqueUser = new UniqueUser.Entities.ProfileUniqueUser(userid, profile.Id)
            {
                ExpirationDate = null
            };
            return profileUniqueUser;
        }


        #endregion

        #region SalePoint

        /// <summary>
        /// Converts to entities UserSalePoint.
        /// </summary>
        /// <param name="salePoint">The salePoint.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>UserSalePoint</returns>
        public static UUEN.UserSalePoint CreateUserSalePoint(MdCommon.SalePoint salePoint, int branchId, int userId)
        {
            UUEN.UserSalePoint userSalePoint = new UUEN.UserSalePoint(userId, branchId, salePoint.Id)
            {
                BranchCode = branchId,
                DefaultSalePoint = salePoint.IsDefault == true ? 1 : 0,
                SalePointCode = salePoint.Id,
                UserId = userId
            };
            return userSalePoint;
        }

        #endregion

        #region UserBranch

        /// <summary>
        /// Converts to entities UserBranch.
        /// </summary>
        /// <param name="branch">The branch.</param>
        /// <param name="userid">The userid.</param>
        /// <returns>UserBranch</returns>
        public static UUEN.UserBranch CreateUserBranch(MdCommon.Branch branch, int userId)
        {
            return new UUEN.UserBranch(userId, branch.Id)
            {
                DefaultBranch = branch.IsDefault == true ? 1 : 0,
            };
        }

        #endregion

        #region UniqueUserLogin

        /// <summary>
        /// Converts to entities UniqueUserLogin.
        /// </summary>
        /// <param name="uniqueUserLogin">The uniqueUserLogin.</param>
        /// <returns>UniqueUserLogin</returns>
        public static UniqueUser.Entities.UniqueUserLogin CreateUniqueUserLogin(Models.UniqueUserLogin uniqueUserLogin)
        {
            //string salt = SecurityHelper.CreateSalt();
            bool boolChangePassword = false;

            if (uniqueUserLogin.MustChangePassword != null)
            {
                boolChangePassword = Convert.ToBoolean(uniqueUserLogin.MustChangePassword);
            }

            CryptoEngine ce = new CryptoEngine();
            ce.EncryptionKey = uniqueUserLogin.Password;
            ce.CryptedText = uniqueUserLogin.Password;
            ce.Decrypt();

            UniqueUser.Entities.UniqueUserLogin userBranch = new UniqueUser.Entities.UniqueUserLogin(uniqueUserLogin.UserId)
            {        
                //Salt = salt,
                //Password = SecurityHelper.GetHashSha256(uniqueUserLogin.Password, salt),
                Salt = null,
                Password = ce.CryptedText,
                MustChangePassword = boolChangePassword,
                PasswordExpirationDate = uniqueUserLogin.ExpirationDate,
                PasswordExpirationDays = uniqueUserLogin.ExpirationsDays,
                PasswordNeverExpire = false,
                CanChangePassword = false,
                LockPassword = false,
            };

            return userBranch;
        }

        #endregion


        #region Module
        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.Modules CreateModule(Models.Module module)
        {
            return new UniqueUser.Entities.Modules()
            {
                Description = module.Description,
                Enabled = module.IsEnabled
            };
        }
        #endregion

        #region SubModule
        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.Submodules CreateSubModule(Models.SubModule module)
        {
            return new UniqueUser.Entities.Submodules()
            {
                ModuleCode = module.Module.Id,
                Description = module.Description,
                Enabled = module.IsEnabled
            };
        }
        #endregion

        #region Profile
        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.Profiles CreateProfile(Models.Profile profile)
        {
            return new UniqueUser.Entities.Profiles()
            {
                Description = profile.Description,
                Enabled = profile.IsEnabled
            };
        }
        #endregion

        #region Access
        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.Accesses CreateAccess(Models.AccessObject access)
        {
            return new UniqueUser.Entities.Accesses()
            {
                Description = access.Description,
                AccessObjectId = access.AccessObjectId,
                ModuleCode = access.SubModule.Module.Id,
                SubmoduleCode = access.SubModule.Id,
                Enabled = access.IsEnabled,
                Url = access.Url,
                ParentAccessId = access.ParentAccessId
            };
        }
        #endregion

        #region AccessObject
        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.AccessObjects CreateAccessObject(Models.AccessObject access)
        {
            return new UniqueUser.Entities.AccessObjects()
            {
                Description = access.Description,
                ObjectName = access.Description,
                AccessObjTypeCode = access.ObjectTypeId
            };
        }
        #endregion

        #region AccessObject
        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.AccessProfiles CreateAccessProfile(UniqueUser.Entities.AccessProfiles access, int newProfileId)
        {
            return new UniqueUser.Entities.AccessProfiles(newProfileId, access.AccessId, access.DatabaseId)
            {
                ExpirationDate = access.ExpirationDate,
                Enabled = access.Enabled
            };
        }
        /// <summary>
        /// Convierte un objeto de tipo Models.AccessProfile a Entities.AccessProfile
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static UniqueUser.Entities.AccessProfiles CreateAccessProfile(Model.AccessProfile access)
        {
            return new UniqueUser.Entities.AccessProfiles(access.ProfileId, access.AccessId, access.DatabaseId)
            {
                ExpirationDate = access.ExpirationDate,
                Enabled = access.Enabled
            };
        }
        #endregion

        /// <summary>
        /// Mapea una lista de modelos de negocio UniqueUsersProduct a una lista de entidades UniqueUsersProduct.
        /// </summary>
        /// <param name="uniqueUsersProductModelList">Listado de modelos de negocio de tipo UniqueUsersProduct.</param>
        /// <returns>Listado de entidades de tipo UniqueUsersProduct.</returns>
        public static List<UUEN.UniqueUsersProduct> MappUniqueUsersProductList(List<Model.UniqueUsersProduct> uniqueUsersProductModelList)
        {
            List<UUEN.UniqueUsersProduct> uniqueUsersProductEntityList = new List<UUEN.UniqueUsersProduct>();

            foreach (Model.UniqueUsersProduct uniqueUsersProductModel in uniqueUsersProductModelList)
            {
                uniqueUsersProductEntityList.Add(EntityAssembler.MappUniqueUsersProduct(uniqueUsersProductModel));
            }

            return uniqueUsersProductEntityList;
        }

        /// <summary>
        /// Mapea un modelo de negocio UniqueUsersProduct a una entidad de tipo UniqueUsersProduct.
        /// </summary>
        /// <param name="uniqueUsersProductModel"></param>
        /// <returns>Entidad de tipo UniqueUsersProduct.</returns>
        public static UUEN.UniqueUsersProduct MappUniqueUsersProduct(Model.UniqueUsersProduct uniqueUsersProductModel)
        {
            return new UUEN.UniqueUsersProduct(uniqueUsersProductModel.UserId, uniqueUsersProductModel.ProductId, uniqueUsersProductModel.PrefixCode)
            {
                UserId = uniqueUsersProductModel.UserId,
                ProductId = uniqueUsersProductModel.ProductId,
                PrefixCode = uniqueUsersProductModel.PrefixCode,
                Enabled = (bool)uniqueUsersProductModel.Enabled,
                Assign = (bool)uniqueUsersProductModel.Assign
            };
        }

        public static ENUP.IndividualRelationApp CreateIndividualRelationApp(Models.IndividualRelationApp individualRelationApp)
        {
            return new ENUP.IndividualRelationApp()
            {
                AgentAgencyId = individualRelationApp.Agency.Id,
                ChildIndividualId = individualRelationApp.IndividualId,
                RelationTypeCode = individualRelationApp.RelationTypeId,
                ParentIndividualId = individualRelationApp.Id
            };
        }
        
        public static COMMEN.PrefixUser CreatePrefixUser(PrefixUser prefixUser)
        {
            return new COMMEN.PrefixUser(prefixUser.UserId, prefixUser.PrefixCode)
            {
            };
        }

        #region ACCESS_PERMISSIONS
        /// <summary>
        /// Convierte un objeto de tipo Models.AccessPermissions a UUEN.Entities.AccessPermissions
        /// </summary>
        /// <param name="accessPermissions">Objeto del tipo Models.AccessPermissions</param>
        /// <returns>Retorna un objeto del tipo UUEN.Entities.AccessPermissions</returns>
        public static UUEN.AccessPermissions CreateAccessPermission(Models.AccessPermissions accessPermissions)
        {
            return new UUEN.AccessPermissions(accessPermissions.Id)
            {
                Description = accessPermissions.Description,
                Code = accessPermissions.Code
            };
        }

        /// <summary>
        /// Convierte un objeto de tipo Models.AccessPermissions a UUEN.Entities.AccessPermissions
        /// </summary>
        /// <param name="accessPermissions">Objeto del tipo Models.AccessPermissions</param>
        /// <returns>Retorna un objeto del tipo UUEN.Entities.AccessPermissions</returns>
        public static List<UUEN.AccessPermissions> CreateAccessPermissions(List<Models.AccessPermissions> accessPermissions)
        {
            return accessPermissions.Select(CreateAccessPermission).ToList();
        }


        public static UUEN.ProfileAccessPermissions CreateProfileAccessPermissions(Models.AccessProfile accesprofile)
        {
            return new UUEN.ProfileAccessPermissions()
            {
                 AccessPermissionsCode = accesprofile.AccessId,
                 ProfileCode = accesprofile.ProfileId
            };
        }

        public static UUEN.UserContextPermission CreateProfileAccessPermissions(Models.ContextProfileAccessPermissions ContextPermissions)
        {
            return new UUEN.UserContextPermission()
            {
                ContextPermissionsCode = ContextPermissions.SecurityContext.Id,
                PermissionCode =ContextPermissions.AccessPermission.Id,
                ProfileCode=ContextPermissions.Profile.Id
            };
        }
        #endregion

        #region Guarantee

        public static List<UUEN.ProfileGuaranteeStatus> CreateProfilesGuaranteesStatus(List<Models.ProfileGuaranteeStatus> ListCollection)
        {
            List<UUEN.ProfileGuaranteeStatus> entityGuaranteeStatus = new List<UUEN.ProfileGuaranteeStatus>();
            foreach (Models.ProfileGuaranteeStatus item in ListCollection)
            {
                entityGuaranteeStatus.Add(CreateProfileGuaranteeStatus(item));
            }
            return entityGuaranteeStatus;
        }


        public static UUEN.ProfileGuaranteeStatus CreateProfileGuaranteeStatus(Models.ProfileGuaranteeStatus guaranteProfStatus)
        {
            return new UUEN.ProfileGuaranteeStatus()
            {
               // Id = guaranteProfStatus.Id,
                ProfileId = guaranteProfStatus.ProfileId,
                Enabled = guaranteProfStatus.Enabled,
                GuaranteeStatusCode = guaranteProfStatus.StatusId
            };
        }

        #endregion

    }
}