using Sistran.Company.Application.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Company.Application.ModelServices.Models.Reports;
using Sistran.Company.Application.UniqueUserServices.Models;
using USEN = Sistran.Core.Application.UniqueUser.Entities;
using EVEN = Sistran.Core.Application.Events.Entities;
using AUEN = Sistran.Core.Application.AuthorizationPolicies.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Data;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using static Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
using Company.AuthorizationPoliciesServices.Models;


namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {

        #region ReportAuthorizationPolicies
        /// <summary>
        /// Obtiene listado de status
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public static List<Models.Status> CreateListStatusParam(BusinessCollection Status)
        {
            List<Models.Status> result = new List<Models.Status>();

            foreach (APEntity.Status item in Status)
            {
                result.Add(CreateStatusParam(item));
            }
            return result;
        }

        /// <summary>
        /// Obtiene status individual
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public static Models.Status CreateStatusParam(APEntity.Status status)
        {
            Models.Status statusmodel = new Models.Status
            {
                Id = status.StatusId,
                Description = status.Description
            };
            return statusmodel;
        }


        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="StatusServicemodel">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>
        public static List<StatusServicemodel> CreateStatusServiceModels(List<Status> status)
        {
            List<StatusServicemodel> statusServicemodel = new List<StatusServicemodel>();
            foreach (var item in status)
            {
                statusServicemodel.Add(CreateStatusServiceModel(item));
            }

            return statusServicemodel;
        }

        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="StatusServicemodel">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>

        public static StatusServicemodel CreateStatusServiceModel(Status status)
        {
            StatusServicemodel result = new StatusServicemodel
            {
                Id = status.Id,
                Description = status.Description
            };
            return result;
        }

        #endregion

        #region Report Policies

        public static CompanyReportPolicies CreateCompanyReportPolicies(DataSet ds)
        {
            CompanyReportPolicies companyPolicies = new CompanyReportPolicies();
            List<CompanyReportPolicy> LscompanyPolicies = new List<CompanyReportPolicy>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LscompanyPolicies.Add(CreateCompanyPolicy(dr));
            }
            companyPolicies.companyPolicies = LscompanyPolicies;

            return companyPolicies;
        }

        private static CompanyReportPolicy CreateCompanyPolicy(DataRow dr) => new CompanyReportPolicy
        {
            idPolicies = Convert.ToInt32(dr["POLICIES_ID"]),
            descriptionPolicy = Convert.ToString(dr["DESCRIPTION_POLICIES"]),
            userPolicy = Convert.ToString(dr["ACCOUNT_NAME"]),
            datePolicy = Convert.ToDateTime(dr["DATE_POLICIES"]),
            branch = Convert.ToString(dr["BRANCH"]),
            numberPolicy = Convert.ToInt64(dr["POLICY_NUM"]),
            typeEndosment = Convert.ToString(dr["ENDORSEMENT_TYPE"]),
            user = Convert.ToString(dr["USER_ANSWER"]),
            //Se rompe el programa teniendo en cuenta que para aprobaciones pendientes no shay fecha de respuesta.
            dateAuthorization = (!string.IsNullOrEmpty(dr["DATE_ANSWER"].ToString())) ? Convert.ToDateTime(dr["DATE_ANSWER"]) : (DateTime?)null,//Convert.ToDateTime(dr["DATE_ANSWER"]),
            prefix = Convert.ToString(dr["PREFIX"]),
            groupPolicies = Convert.ToString(dr["GROUP_POLICIES"]),
            statusPolicy = Convert.ToString(dr["DESCRIPTION_STATUS"]),
            waitingTime = Convert.ToInt32(dr["TIME"])
        };


        public static PoliciesServicesModel GetCompanyReportPolicies(CompanyReportPolicies resultValue)
        {
            PoliciesServicesModel policiesServicesModel = new PoliciesServicesModel();
            List<PoliciesServiceModel> policiesServiceModel = new List<PoliciesServiceModel>();
            foreach (var item in resultValue.companyPolicies)
            {
                policiesServiceModel.Add(GetCompanyPolicy(item));
            }
            policiesServicesModel.policiesServiceModel = policiesServiceModel;

            return policiesServicesModel;
        }

        private static PoliciesServiceModel GetCompanyPolicy(CompanyReportPolicy item) => new PoliciesServiceModel
        {
            idPolicies = item.idPolicies,
            descriptionPolicy = item.descriptionPolicy,
            userPolicy = item.userPolicy,
            datePolicy = item.datePolicy,
            prefix = item.prefix,
            numberPolicy = item.numberPolicy,
            typeEndosment = item.typeEndosment,
            user = item.user,
            dateAuthorization = item.dateAuthorization,
            branch = item.branch,
            groupPolicies = item.groupPolicies,
            statusPolicy = item.statusPolicy,
            waitingTime = item.waitingTime
        };
        #endregion

        #region WorkFlow Policies

        public static CompanyUser CreateCompanyUser(USEN.UniqueUsers entityUniqueUser)
        {
            return new CompanyUser
            {
                UserId = entityUniqueUser.UserId,
                AccountName = entityUniqueUser.AccountName,
                PersonId = entityUniqueUser.PersonId,
                AuthenticationType = (AuthenticationType)entityUniqueUser.AuthenticationTypeCode,
                UserDomain = entityUniqueUser.UserDomain,
                DisableDate = entityUniqueUser.DisabledDate,
                LockDate = entityUniqueUser.LockDate,
                ExpirationDate = entityUniqueUser.ExpirationDate,
                LockPassword = entityUniqueUser.LockPassword,
                CreationDate = entityUniqueUser.CreatedDate,
                CreatedUserId = Convert.ToInt32(entityUniqueUser.CreatedUserId),
                LastModificationDate = entityUniqueUser.ModifiedDate,
                ModifiedUserId = Convert.ToInt32(entityUniqueUser.ModifiedUserId)

            };
        }

        public static List<CompanyUser> CreateCompanyUsers(BusinessCollection businessCollection)
        {
            List<CompanyUser> companyUsers = new List<CompanyUser>();
            foreach (USEN.UniqueUsers entityUniqueUser in businessCollection)
            {
                companyUsers.Add(CreateCompanyUser(entityUniqueUser));
            }

            return companyUsers;
        }

        public static CompanyEventGroup CreateCompanyEventGroup(EVEN.CoEventGroup entityCoEventGroup)
        {
            return new CompanyEventGroup
            {
                Id = entityCoEventGroup.GroupEventId,
                Description = entityCoEventGroup.Description
            };

        }

        public static List<CompanyEventGroup> CreateCompanyEventGroups(BusinessCollection businessCollection)
        {
            List<CompanyEventGroup> companyEventGroups = new List<CompanyEventGroup>();
            foreach (EVEN.CoEventGroup entityCoEventGroup in businessCollection)
            {
                companyEventGroups.Add(CreateCompanyEventGroup(entityCoEventGroup));
            }

            return companyEventGroups;
        }

        public static List<CompanySarlaftAuthorizationReason> CreateCompanyAuthorizationReasons(BusinessCollection businessCollection)
        {
            List<CompanySarlaftAuthorizationReason> companySarlaftAuthorizationReasons = new List<CompanySarlaftAuthorizationReason>();
            foreach (COMMEN.AuthorizationReason entityAuthorizationReason in businessCollection)
            {
                companySarlaftAuthorizationReasons.Add(CreateCompanyAuthorizationReason(entityAuthorizationReason));
            }

            return companySarlaftAuthorizationReasons;
        }

        public static CompanySarlaftEventAuthorization CreateSarlaftEventAuthorization(EVEN.CoEventAuthorization entityEventAuthorization)
        {
            return new CompanySarlaftEventAuthorization
            {
            };
        }

        public static CompanySarlaftAuthorizationReason CreateCompanyAuthorizationReason(COMMEN.AuthorizationReason entityAuthorizationReason)
        {
            return new CompanySarlaftAuthorizationReason
            {
                Id = entityAuthorizationReason.AuthorizationReasonCode,
                Description = entityAuthorizationReason.Description
            };
        }

        public static CompanyWorkFlowUser CreateCompanyWorkFlowUser(USEN.UniqueUsers entityUniqueUsers)
        {
            if (entityUniqueUsers == null)
            {
                return null;
            }

            return new CompanyWorkFlowUser
            {
                Id = entityUniqueUsers.UserId,
                AccountName = entityUniqueUsers.AccountName
            };
        }

        public static CompanyWorkFlowBranch CreateCompanyWorkFlowBranch(COMMEN.Branch entityBranch)
        {
            if (entityBranch == null)
            {
                return null;
            }

            return new CompanyWorkFlowBranch
            {
                Id = entityBranch.BranchCode,
                Description = entityBranch.Description
            };
        }

        public static CompanyWorkFlowPrefix CreateCompanyWorkFlowPrefix(COMMEN.Prefix entityPrefix)
        {
            if (entityPrefix == null)
            {
                return null;
            }

            return new CompanyWorkFlowPrefix
            {
                Id = entityPrefix.PrefixCode,
                Description = entityPrefix.Description
            };
        }

        #endregion

        /// <summary>
        /// Creates the EventAuthorization by CompanyPolicy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static CompanyEventAuthorization CreateCompanyEventAuthorizationRelease(CompanyEventAuthorization Event)
        {
            try
            {
                Event.EventId = (int)EventTypes.Release;
                Event.EventWorkFlowModule = (int)EventWorkFlowAssociation.Module;
                Event.EventWorkFlowSubModule = (int)EventWorkFlowAssociation.Submodule;
            }
            catch (Exception ex)
            {
                throw new Core.Framework.BAF.BusinessException(ex.Message);
            }
            return Event;
        }

        /// <summary>
        /// Creates the EventAuthorization by CompanyPolicy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static List<CompanyEventAuthorization> CreateCompanyEventAuthorization(BusinessCollection Event)
        {
            try
            {
                List<CompanyEventAuthorization> lstEvent = new List<CompanyEventAuthorization>();
                Event.ForEach(x =>
                {
                    lstEvent.Add(ConvertEventAuthorization((EVEN.CoEventAuthorization)x));
                });
                return lstEvent;
            }
            catch (Exception ex)
            {
                throw new Core.Framework.BAF.BusinessException(ex.Message);
            }
           
        }

        public static CompanyEventAuthorization ConvertEventAuthorization(EVEN.CoEventAuthorization eventAuthorization) {

             CompanyEventAuthorization objLocal = new CompanyEventAuthorization()
            {
                AccessId = eventAuthorization.AccessId,
                AuthorizationDate = eventAuthorization.AuthorizationDate,
                AuthorizationDescription = eventAuthorization.AuthorizationDescription,
                AuthorizationId = eventAuthorization.AuthorizationId,
                AuthorizationReasonCode = eventAuthorization.AuthorizationReasonCode.ToString(),
                AuthorizedInd = eventAuthorization.AuthorizedInd,
                AuthoUserId = eventAuthorization.AuthoUserId,
                Description = eventAuthorization.Description,
                DescriptionError = eventAuthorization.DescriptionErrorMessage,

            };
            return objLocal;
        }


    }
}
