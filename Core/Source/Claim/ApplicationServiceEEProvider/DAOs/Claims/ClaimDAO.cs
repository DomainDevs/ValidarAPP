using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;
using System.Data;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.ClaimServices.EEProvider.Enums;
using CLMMOD = Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest;
using System.Data.SqlClient;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimDAO
    {
        public Claim GetClaimByClaimId(int claimId)
        {
            PrimaryKey primaryKey = CLMEN.Claim.CreatePrimaryKey(claimId);
            Claim claim = ModelAssembler.CreateClaim((CLMEN.Claim)DataFacadeManager.GetObject(primaryKey));

            if (claim != null)
            {
                //Se arma el flitro para obtener la última modificación de la denuncia
                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, claim.Id);

                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                claim.TextOperation = GetTextOperationByTextOperationId(claim.TextOperation.Id);
            }
            return claim;
        }

        public Claim CreateClaim(Claim claim)
        {            
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            bool isUniqueClaimNumber = Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_UNIQUE_CLAIM_NUMBER)));
            int claimNumberCode = 0;
            BusinessCollection businessCollection = new BusinessCollection();

            if (isUniqueClaimNumber)
            {
                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.Claim.Properties.Number;
                filter.PropertyEquals(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name, claim.Endorsement.PolicyNumber);
                businessCollection = DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate(), sortColumn);

                if (businessCollection.Count > 0)
                {
                    claimNumberCode = Convert.ToInt32(businessCollection.Cast<CLMEN.Claim>().FirstOrDefault()?.Number);
                    claimNumberCode++;
                }
                else
                {
                    claimNumberCode++;
                }
            }
            else
            {
                filter.PropertyEquals(CLMEN.ClaimNumber.Properties.BranchCode, typeof(CLMEN.ClaimNumber).Name, claim.Branch.Id);
                filter.And();
                filter.PropertyEquals(CLMEN.ClaimNumber.Properties.PrefixCode, typeof(CLMEN.ClaimNumber).Name, claim.Prefix.Id);
                businessCollection = DataFacadeManager.GetObjects(typeof(CLMEN.ClaimNumber), filter.GetPredicate());

                if (businessCollection.Count > 0)
                {
                    CLMEN.ClaimNumber entityClaimNumber = (CLMEN.ClaimNumber)businessCollection.First();
                    claimNumberCode = entityClaimNumber.ClaimNumberCode;
                    claimNumberCode++;
                    CLMEN.ClaimNumber claimNumber = new CLMEN.ClaimNumber()
                    {
                        PrefixCode = entityClaimNumber.PrefixCode,
                        BranchCode = entityClaimNumber.BranchCode,
                        ClaimNumberCode = claimNumberCode
                    };
                    PrimaryKey primaryKey = CLMEN.ClaimNumber.CreatePrimaryKey(entityClaimNumber.PrefixCode, entityClaimNumber.BranchCode, entityClaimNumber.ClaimNumberCode);
                    DataFacadeManager.Delete(primaryKey);
                    DataFacadeManager.Insert(claimNumber);
                }
                else
                {
                    throw new Exception(Resources.Resources.BranchOrPrefixDoNotParametrized);
                }
            }

            if (claim.TextOperation.Operation != null)
            {
                claim.TextOperation = CreateTextOperation(claim.TextOperation);
            }
            else
            {
                claim.TextOperation = null;
            }

            claim.Number = claimNumberCode;

            CLMEN.Claim entityClaim = EntityAssembler.CreateClaim(claim);
            
            if (entityClaim.CityCode == 0)
            {
                entityClaim.CityCode = null;
                entityClaim.StateCode = null;
                entityClaim.CountryCode = null;
            }

            if (entityClaim.CauseId == 0)
            {
                entityClaim.CauseId = null;
            }

            DataFacadeManager.Insert(entityClaim);

            claim.Id = entityClaim.ClaimCode;

            //Al crear la denuncia solo hay una modificación
            CLMEN.ClaimModify entityClaimModify = EntityAssembler.CreateClaimModify(claim.Modifications.First(), claim.Id);
            DataFacadeManager.Insert(entityClaimModify);
            claim.Modifications.First().Id = entityClaimModify.ClaimModifyCode;

            int subClaim = 1;

            foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
            {
                
                if (claimCoverage.TextOperation.Operation != null)
                {
                    claimCoverage.TextOperation = CreateTextOperation(claimCoverage.TextOperation);
                }
                else
                {
                    claimCoverage.TextOperation = null;
                }

                CLMEN.ClaimCoverage entityClaimCoverage = EntityAssembler.CreateClaimCoverage(claimCoverage, claim.Modifications.First().Id);
                entityClaimCoverage.SubClaim = subClaim;

                DataFacadeManager.Insert(entityClaimCoverage);

                claimCoverage.Id = entityClaimCoverage.ClaimCoverageCode;

                foreach (Estimation estimation in claimCoverage.Estimations)
                {
                    estimation.Version = 1;
                    estimation.Amount = estimation.Amount - estimation.AmountAccumulate;
                    estimation.AmountAccumulate += estimation.Amount;

                    CLMEN.ClaimCoverageAmount entityClaimCoverageAmount = EntityAssembler.CreateClaimCoverageAmount(estimation, claimCoverage.Id);

                    if (claim.BusinessTypeId == 3 && claim.IsTotalParticipation)
                    {
                        foreach (CoInsuranceAssigned coInsuranceAssigned in claim.CoInsuranceAssigned)
                        {
                            ClaimCoverageCoInsurance claimCoverageCoInsurance = ModelAssembler.CreateClaimCoverageCoInsurance(estimation, claimCoverage, coInsuranceAssigned);
                            SaveClaimCoverageCoInsurance(claimCoverageCoInsurance, claimCoverage.Id, coInsuranceAssigned.CompanyNum, estimation.Type.Id);
                        }
                    }

                    DataFacadeManager.Insert(entityClaimCoverageAmount);
                }

                if (claimCoverage.Driver?.DocumentNumber != null)
                {
                    CreateDriver(claimCoverage.Driver, claimCoverage.Id);
                }

                if (claimCoverage.ThirdAffected?.DocumentNumber != null)
                {
                    CreateThirdAffected(claimCoverage.ThirdAffected, claimCoverage.Id);
                }

                if (claimCoverage.ThirdPartyVehicle?.Plate != null)
                {
                    CreateThirdPartyVehicle(claimCoverage.ThirdPartyVehicle, claimCoverage.Id);
                }

                subClaim++;
            }

            if (claim.CatastrophicEvent != null)
            {
                if (claim.CatastrophicEvent.Catastrophe.Id == 0)
                {
                    PARAMEN.Catastrophe catastrophe = EntityAssembler.CreateCatastrophe(claim.CatastrophicEvent.Catastrophe);
                    DataFacadeManager.Insert(catastrophe);

                    claim.CatastrophicEvent.Catastrophe.Id = catastrophe.CatastropheCode;
                }

                CLMEN.ClaimCatastrophicInformation claimCatastrophicInformation = EntityAssembler.CreateCatastrophicInformation(claim.CatastrophicEvent, claim.Id);

                DataFacadeManager.Insert(claimCatastrophicInformation);
            }

            if (claim.Inspection != null)
            {
                CLMEN.ClaimSupplier entityClaimSupplier = EntityAssembler.CreateClaimSupplier(claim.Inspection, claim.Id);
                entityClaimSupplier.ClaimCode = entityClaim.ClaimCode;

                DataFacadeManager.Insert(entityClaimSupplier);
            }

            #region ClaimControl

            ClaimControl claimControl = ModelAssembler.CreateClaimControl(claim);
            claimControl.Action = "I";

            CreateClaimControl(claimControl);

            #endregion

            return claim;
        }

        public Claim UpdateClaim(Claim claim)
        {
            ParticipantDAO participantDAO = new ParticipantDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();

            if (claim.TextOperation?.Operation != null)
            {
                claim.TextOperation = CreateTextOperation(claim.TextOperation);
            }
            else
            {
                claim.TextOperation = null;
            }

            CLMEN.Claim entityClaim = EntityAssembler.CreateClaim(claim);

            if (entityClaim.CauseId == 0)
                entityClaim.CauseId = null;

            if (entityClaim.CityCode == 0)
            {
                entityClaim.CityCode = null;
                entityClaim.StateCode = null;
                entityClaim.CountryCode = null;
            }

            DataFacadeManager.Update(entityClaim);

            claim.Id = entityClaim.ClaimCode;

            int subClaim = 1;

            // Se captura la ultima modificación
            ClaimModify claimModify = claim.Modifications.Last();

            // Se guarda la modificación
            CLMEN.ClaimModify entityClaimModify = EntityAssembler.CreateClaimModify(claimModify, claim.Id);
            DataFacadeManager.Insert(entityClaimModify);

            claimModify.Id = entityClaimModify.ClaimModifyCode;

            // Se guardan las coberturas de la ultima modificación
            foreach (ClaimCoverage claimCoverage in claimModify.Coverages)
            {

                if (claimCoverage.TextOperation?.Operation != null)
                {
                    claimCoverage.TextOperation = CreateTextOperation(claimCoverage.TextOperation);
                }
                else
                {
                    claimCoverage.TextOperation = null;
                }

                CLMEN.ClaimCoverage entityClaimCoverage = EntityAssembler.CreateClaimCoverage(claimCoverage, claimModify.Id);
                entityClaimCoverage.SubClaim = subClaim;

                //Siempre se inserta un nuevo registro por modificación de la denuncia
                DataFacadeManager.Insert(entityClaimCoverage);

                claimCoverage.Id = entityClaimCoverage.ClaimCoverageCode;

                foreach (Estimation estimation in claimCoverage.Estimations)
                {
                    if (estimation.Reason.Status.InternalStatus.Id == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_ESTIMATION_INTERNAL_STATUS_CLOSED)))
                    {
                        estimation.Amount = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(claim.Id, claimCoverage.SubClaim, estimation.Type.Id, estimation.Currency.Id);
                    }

                    estimation.Version = 1;
                    estimation.Amount = estimation.Amount - estimation.AmountAccumulate;
                    estimation.AmountAccumulate += estimation.Amount;

                    CLMEN.ClaimCoverageAmount entityClaimCoverageAmount = EntityAssembler.CreateClaimCoverageAmount(estimation, claimCoverage.Id);
                    
                    if (claim.BusinessTypeId == 3 && claim.IsTotalParticipation && claim.CoInsuranceAssigned != null)
                    {
                        foreach (CoInsuranceAssigned coInsuranceAssigned in claim.CoInsuranceAssigned)
                        {
                            ClaimCoverageCoInsurance claimCoverageCoInsurance = ModelAssembler.CreateClaimCoverageCoInsurance(estimation, claimCoverage, coInsuranceAssigned);
                            SaveClaimCoverageCoInsurance(claimCoverageCoInsurance, claimCoverage.Id, coInsuranceAssigned.CompanyNum, estimation.Type.Id);
                        }
                    }

                    //Siempre se inserta un nuevo registro por modificación de la denuncia
                    DataFacadeManager.Insert(entityClaimCoverageAmount);

                }

                if (claimCoverage.Driver?.DocumentNumber != null)
                {
                    CreateDriver(claimCoverage.Driver, claimCoverage.Id);
                }
                else
                {
                    claimCoverage.Driver = null;
                }

                if (claimCoverage.ThirdAffected?.DocumentNumber != null)
                {
                    CreateThirdAffected(claimCoverage.ThirdAffected, claimCoverage.Id);
                }
                else
                {
                    claimCoverage.ThirdAffected = null;
                }

                if (claimCoverage.ThirdPartyVehicle?.Plate != null)
                {
                    CreateThirdPartyVehicle(claimCoverage.ThirdPartyVehicle, claimCoverage.Id);
                }
                else
                {
                    claimCoverage.ThirdPartyVehicle = null;
                }
                subClaim++;
            }

            if (claim.CatastrophicEvent != null)
            {
                if (claim.CatastrophicEvent.Catastrophe.Id == 0)
                {
                    PARAMEN.Catastrophe catastrophe = EntityAssembler.CreateCatastrophe(claim.CatastrophicEvent.Catastrophe);
                    DataFacadeManager.Insert(catastrophe);

                    claim.CatastrophicEvent.Catastrophe.Id = catastrophe.CatastropheCode;
                }

                CLMEN.ClaimCatastrophicInformation claimCatastrophicInformation = EntityAssembler.CreateCatastrophicInformation(claim.CatastrophicEvent, claim.Id);

                PrimaryKey key = CLMEN.ClaimCatastrophicInformation.CreatePrimaryKey(claim.Id);
                CLMEN.ClaimCatastrophicInformation catastrophicInformation = (CLMEN.ClaimCatastrophicInformation)DataFacadeManager.GetObject(key);

                if (catastrophicInformation == null)
                {
                    DataFacadeManager.Insert(claimCatastrophicInformation);
                }
                else
                {
                    DataFacadeManager.Update(claimCatastrophicInformation);
                }
            }

            if (claim.Inspection != null)
            {
                CLMEN.ClaimSupplier entityClaimSupplier = EntityAssembler.CreateClaimSupplier(claim.Inspection, claim.Id);
                entityClaimSupplier.ClaimCode = entityClaim.ClaimCode;

                PrimaryKey key = CLMEN.ClaimSupplier.CreatePrimaryKey(claim.Id);
                CLMEN.ClaimSupplier supplier = (CLMEN.ClaimSupplier)DataFacadeManager.GetObject(key);

                if (supplier == null)
                {
                    DataFacadeManager.Insert(entityClaimSupplier);
                }
                else
                {
                    DataFacadeManager.Update(entityClaimSupplier);
                }
            }

            #region ClaimControl

            ClaimControl claimControl = ModelAssembler.CreateClaimControl(claim);
            claimControl.Action = "U";

            CreateClaimControl(claimControl);

            #endregion

            return claim;
        }
        
        #region Vehicle
        public Driver GetDriverByDocumentNumberFullName(string description, InsuredSearchType insuredSearchType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Driver driver = new Driver();

            switch (insuredSearchType)
            {
                case InsuredSearchType.DocumentNumber:
                    filter.Property(CLMEN.ClaimCoverageDriverInformation.Properties.DocumentNumber, typeof(CLMEN.ClaimCoverageDriverInformation).Name);
                    filter.Equal();
                    filter.Constant(description);
                    CLMEN.ClaimCoverageDriverInformation entityDriverInformationDocument = DataFacadeManager.GetObjects(typeof(CLMEN.ClaimCoverageDriverInformation), filter.GetPredicate()).Cast<CLMEN.ClaimCoverageDriverInformation>().FirstOrDefault();

                    if (entityDriverInformationDocument != null)
                    {
                        driver = ModelAssembler.CreateDriver(entityDriverInformationDocument);
                    }
                    else
                    {
                        return null;
                    }

                    break;
                case InsuredSearchType.IndividualId:
                    filter.Property(CLMEN.ClaimCoverageDriverInformation.Properties.Name, typeof(CLMEN.ClaimCoverageDriverInformation).Name);
                    filter.Equal();
                    filter.Constant(description);
                    CLMEN.ClaimCoverageDriverInformation entityDriverInformation = DataFacadeManager.GetObjects(typeof(CLMEN.ClaimCoverageDriverInformation), filter.GetPredicate()).Cast<CLMEN.ClaimCoverageDriverInformation>().FirstOrDefault();

                    if (entityDriverInformation != null)
                    {
                        driver = ModelAssembler.CreateDriver(entityDriverInformation);
                    }
                    else
                    {
                        return null;
                    }

                    break;
            }

            return driver;
        }

        #endregion

        public Claim GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (branchId != null)
            {
                filter.PropertyEquals(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name, branchId);
                filter.And();
            }
            if (prefixId != null)
            {
                filter.PropertyEquals(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name, prefixId);
                filter.And();
            }
            filter.PropertyEquals(CLMEN.Claim.Properties.Number, typeof(CLMEN.Claim).Name, claimNumber);
            filter.And();
            filter.PropertyEquals(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name, policyDocumentNumber);

            //Se consulta la cabecera de la denuncia
            CLMEN.Claim entityClaim = (CLMEN.Claim)DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate()).FirstOrDefault();

            if (entityClaim != null)
            {
                //Se arma el filtro para las modificaciones de la denuncia
                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, entityClaim.ClaimCode);

                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                //Se obtiene la última modificación de la denuncia
                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);

                ClaimCoverageView claimCoverageView = new ClaimCoverageView();
                ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageView);

                Claim claim = ModelAssembler.CreateClaim(entityClaim);

                List<QUOEN.Coverage> entityCoverages = claimCoverageView.Coverages.Cast<QUOEN.Coverage>().ToList();
                List<COMMEN.Currency> entityCurrencies = claimCoverageView.Currencies.Cast<COMMEN.Currency>().ToList();
                List<PARAMEN.EstimationType> entityEstimationTypes = claimCoverageView.EstimationTypes.Cast<PARAMEN.EstimationType>().ToList();
                List<PARAMEN.EstimationTypeStatus> entityEstimationTypeStatuses = claimCoverageView.EstimationTypesStatuses.Cast<PARAMEN.EstimationTypeStatus>().ToList();
                List<PARAMEN.EstimationTypeInternalStatus> entityEstimationTypeInternalStatuses = claimCoverageView.EstimationTypeInternalStatuses.Cast<PARAMEN.EstimationTypeInternalStatus>().ToList();

                //Se guarda la ultima modificación
                claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                claim.Modifications.First().Coverages = ModelAssembler.CreateClaimCoverages(claimCoverageView.ClaimCoverages);

                foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                {
                    claimCoverage.Description = entityCoverages.First(x => x.CoverageId == claimCoverage.CoverageId).PrintDescription;
                    claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimCoverageView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());

                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = entityEstimationTypeStatuses.First(x => x.EstimationTypeStatusCode == estimation.Reason.Status.Id);
                        estimation.Currency.Description = entityCurrencies.First(x => x.CurrencyCode == estimation.Currency.Id).Description;
                        estimation.Type.Description = entityEstimationTypes.First(x => x.EstimateTypeCode == estimation.Type.Id).Description;
                        estimation.Reason.Status.Description = entityEstimationTypeStatus.Description;
                        estimation.Reason.Status.InternalStatus = new InternalStatus
                        {
                            Id = entityEstimationTypeStatus.InternalStatusCode,
                            Description = entityEstimationTypeInternalStatuses.First(x => x.InternalStatusCode == entityEstimationTypeStatus.InternalStatusCode).Description
                        };
                    }
                }

                return claim;
            }
            else
            {
                return null;
            }
        }

        public Claim GetClaimByClaimIdSubClaimEstimationTypeId(int claimId, int subClaim, int estimationTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Claim.Properties.ClaimCode, typeof(CLMEN.Claim).Name, claimId);

            //Se consulta la cabecera de la denuncia
            CLMEN.Claim entityClaim = (CLMEN.Claim)DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate()).FirstOrDefault();

            if (entityClaim != null)
            {
                //Se arma el filtro para las modificaciones de la denuncia
                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, entityClaim.ClaimCode);

                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                //Se obtiene la última modificación de la denuncia
                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);
                filter.And();
                filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.SubClaim, typeof(CLMEN.ClaimCoverage).Name, subClaim);
                filter.And();
                filter.PropertyEquals(CLMEN.ClaimCoverageAmount.Properties.EstimationTypeCode, typeof(CLMEN.ClaimCoverageAmount).Name, estimationTypeId);

                ClaimCoverageView claimCoverageView = new ClaimCoverageView();
                ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageView);

                Claim claim = ModelAssembler.CreateClaim(entityClaim);

                List<QUOEN.Coverage> entityCoverages = claimCoverageView.Coverages.Cast<QUOEN.Coverage>().ToList();
                List<COMMEN.Currency> entityCurrencies = claimCoverageView.Currencies.Cast<COMMEN.Currency>().ToList();
                List<PARAMEN.EstimationType> entityEstimationTypes = claimCoverageView.EstimationTypes.Cast<PARAMEN.EstimationType>().ToList();
                List<PARAMEN.EstimationTypeStatus> entityEstimationTypeStatuses = claimCoverageView.EstimationTypesStatuses.Cast<PARAMEN.EstimationTypeStatus>().ToList();
                List<PARAMEN.EstimationTypeInternalStatus> entityEstimationTypeInternalStatuses = claimCoverageView.EstimationTypeInternalStatuses.Cast<PARAMEN.EstimationTypeInternalStatus>().ToList();

                //Se guarda la ultima modificación
                claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                claim.Modifications.First().Coverages = ModelAssembler.CreateClaimCoverages(claimCoverageView.ClaimCoverages);

                foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                {
                    claimCoverage.Description = entityCoverages.First(x => x.CoverageId == claimCoverage.CoverageId).PrintDescription;
                    claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimCoverageView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());

                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = entityEstimationTypeStatuses.First(x => x.EstimationTypeStatusCode == estimation.Reason.Status.Id);
                        estimation.Currency.Description = entityCurrencies.First(x => x.CurrencyCode == estimation.Currency.Id).Description;
                        estimation.Type.Description = entityEstimationTypes.First(x => x.EstimateTypeCode == estimation.Type.Id).Description;
                        estimation.Reason.Status.Description = entityEstimationTypeStatus.Description;
                        estimation.Reason.Status.InternalStatus = new InternalStatus
                        {
                            Id = entityEstimationTypeStatus.InternalStatusCode,
                            Description = entityEstimationTypeInternalStatuses.First(x => x.InternalStatusCode == entityEstimationTypeStatus.InternalStatusCode).Description
                        };
                    }
                }

                return claim;
            }
            else
            {
                return null;
            }
        }

        public List<Claim> SearchClaims(SearchClaim searchClaim)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<Claim> claims = new List<Claim>();
            List<Dictionary<string, dynamic>> dictionaryClaims = new List<Dictionary<string, dynamic>>();
            bool emptyFilter = true;

            #region Filters

            if (searchClaim.ClaimNumber != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.Number, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.ClaimNumber);
                emptyFilter = false;
            }

            if (searchClaim.BranchId != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.BranchId);
                emptyFilter = false;
            }

            if (searchClaim.HolderId != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.IndividualId, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.HolderId);
                emptyFilter = false;
            }

            if (searchClaim.ClaimDateFrom != null && searchClaim.ClaimDateTo != null)
            {
                if (searchClaim.ClaimDateFrom > DateTime.MinValue)
                {

                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name);
                    filter.GreaterEqual();
                    filter.Constant(searchClaim.ClaimDateFrom);
                    emptyFilter = false;
                }

                if (searchClaim.ClaimDateTo > DateTime.MinValue)
                {
                    searchClaim.ClaimDateTo = Convert.ToDateTime(searchClaim.ClaimDateTo).Add(new TimeSpan(23, 59, 59));

                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name);
                    filter.LessEqual();
                    filter.Constant(searchClaim.ClaimDateTo);
                    emptyFilter = false;
                }

            }

            if (searchClaim.NoticeDateFrom != null && searchClaim.NoticeDateTo != null)
            {
                if (searchClaim.NoticeDateFrom > DateTime.MinValue)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.NoticeDate, typeof(CLMEN.Claim).Name);
                    filter.GreaterEqual();
                    filter.Constant(searchClaim.NoticeDateFrom);
                    emptyFilter = false;
                }

                if (searchClaim.NoticeDateTo > DateTime.MinValue)
                {
                    searchClaim.NoticeDateTo = Convert.ToDateTime(searchClaim.NoticeDateTo).Add(new TimeSpan(23, 59, 59));

                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.NoticeDate, typeof(CLMEN.Claim).Name);
                    filter.LessEqual();
                    filter.Constant(searchClaim.NoticeDateTo);
                    emptyFilter = false;
                }
            }

            if (searchClaim.DocumentNumber != null && searchClaim.DocumentNumber != "")
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.DocumentNumber);
                emptyFilter = false;
            }

            if (searchClaim.PrefixId != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.PrefixId);
                emptyFilter = true;
            }



            #endregion

            #region Selects

            SelectQuery selectQuery = new SelectQuery();

            //Claims
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimCode, typeof(CLMEN.Claim).Name), "ClaimCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name), "OccurrenceDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name), "ClaimBranchCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.EndorsementId, typeof(CLMEN.Claim).Name), "EndorsementId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.IndividualId, typeof(CLMEN.Claim).Name), "IndividualId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.PolicyId, typeof(CLMEN.Claim).Name), "PolicyId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name), "DocumentNumber"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.BusinessTypeCode, typeof(CLMEN.Claim).Name), "BusinessTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.Number, typeof(CLMEN.Claim).Name), "Number"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimNoticeCode, typeof(CLMEN.Claim).Name), "ClaimNoticeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.NoticeDate, typeof(CLMEN.Claim).Name), "NoticeDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name), "PrefixCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.CityCode, typeof(CLMEN.Claim).Name), "CityCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.StateCode, typeof(CLMEN.Claim).Name), "StateCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.CountryCode, typeof(CLMEN.Claim).Name), "CountryCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.Location, typeof(CLMEN.Claim).Name), "Location"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimDamageResponsibilityCode, typeof(CLMEN.Claim).Name), "ClaimDamageResponsibilityCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimDamageTypeCode, typeof(CLMEN.Claim).Name), "ClaimDamageTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.CauseId, typeof(CLMEN.Claim).Name), "CauseId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.TextOperationCode, typeof(CLMEN.Claim).Name), "Id"));

            //Endorsement Risk
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name), "RiskId"));

            //Branch
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, typeof(COMMEN.Branch).Name), "BranchDescription"));

            //Prefix
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.Description, typeof(COMMEN.Prefix).Name), "PrefixDescription"));

            //Hard Risk Type
            selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.HardRiskType.Properties.SubCoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "2"), "SubCoveredRiskTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "2"), "CoveredRiskTypeCode"));

            #endregion

            #region Joins

            Join join = new Join(new ClassNameTable(typeof(CLMEN.Claim), typeof(CLMEN.Claim).Name), new ClassNameTable(typeof(COMMEN.Prefix), typeof(COMMEN.Prefix).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(COMMEN.Prefix.Properties.PrefixCode, typeof(COMMEN.Prefix).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), typeof(COMMEN.Branch).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.HardRiskType), typeof(PARAMEN.HardRiskType).Name + "1"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name + "1"))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.HardRiskType), typeof(PARAMEN.HardRiskType).Name + "2"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                 .Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name + "2")
                .And()
                .Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "1")
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "2"))
                .GetPredicate();


            join = new Join(join, new ClassNameTable(typeof(ISSEN.EndorsementRisk), typeof(ISSEN.EndorsementRisk).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.EndorsementId, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name)
                .And()
                .Property(CLMEN.Claim.Properties.PolicyId, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Risk), typeof(ISSEN.Risk).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                .Equal()
                .Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name))
                .GetPredicate();


            if (searchClaim.PrefixId != null)
            {
                switch ((SubCoveredRiskType)Convert.ToInt32(GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(Convert.ToInt32(searchClaim.PrefixId), GetClaimPrefixCoveredRiskTypeByPrefixCode(Convert.ToInt32(searchClaim.PrefixId))).SubCoveredRiskTypeCode))
                {
                    case SubCoveredRiskType.Vehicle:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                            .GetPredicate();
                        break;
                    //case SubCoveredRiskType.ThirdPartyLiability:
                    //selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                    //join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                    //join.Criteria = (new ObjectCriteriaBuilder()
                    //    .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                    //    .Equal()
                    //    .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                    //    .GetPredicate();
                    //break;
                    case SubCoveredRiskType.Property:
                    case SubCoveredRiskType.Liability:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name), "Street"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskLocation), typeof(ISSEN.RiskLocation).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Surety:
                    case SubCoveredRiskType.Lease:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskSurety), typeof(ISSEN.RiskSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskSurety.Properties.RiskId, typeof(ISSEN.RiskSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskJudicialSurety.Properties.InsuredId, typeof(ISSEN.RiskJudicialSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskJudicialSurety), typeof(ISSEN.RiskJudicialSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskJudicialSurety.Properties.RiskId, typeof(ISSEN.RiskJudicialSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Transport:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskTransport.Properties.TransportCargoTypeCode, typeof(ISSEN.RiskTransport).Name), "TransportCargoTypeCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskTransport), typeof(ISSEN.RiskTransport).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.RiskTransport).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Aircraft:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.RegisterNo, typeof(ISSEN.RiskAircraft).Name), "RegisterNo"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Marine:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftDescription, typeof(ISSEN.RiskAircraft).Name), "AircraftDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));


                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Fidelity:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.Description, typeof(ISSEN.RiskFidelity).Name), "FidelityDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.RiskCommercialClassCode, typeof(ISSEN.RiskFidelity).Name), "RiskCommercialClassCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskFidelity), typeof(ISSEN.RiskFidelity).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskFidelity.Properties.RiskId, typeof(ISSEN.RiskFidelity).Name))
                            .GetPredicate();
                        break;
                }
            }

            #endregion

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    Dictionary<string, dynamic> dictionaryClaim = new Dictionary<string, dynamic>();

                    dictionaryClaim.Add("ClaimCode", Convert.ToInt32(reader["ClaimCode"]));
                    dictionaryClaim.Add("OccurrenceDate", Convert.ToDateTime(reader["OccurrenceDate"]));
                    dictionaryClaim.Add("ClaimBranchCode", Convert.ToInt32(reader["ClaimBranchCode"]));
                    dictionaryClaim.Add("BranchDescription", Convert.ToString(reader["BranchDescription"]));
                    dictionaryClaim.Add("PolicyId", Convert.ToInt32(reader["PolicyId"]));
                    dictionaryClaim.Add("DocumentNumber", Convert.ToString(reader["DocumentNumber"]));
                    dictionaryClaim.Add("EndorsementId", Convert.ToInt32(reader["EndorsementId"]));
                    dictionaryClaim.Add("IndividualId", Convert.ToInt32(reader["IndividualId"]));
                    dictionaryClaim.Add("RiskId", Convert.ToInt32(reader["RiskId"]));
                    dictionaryClaim.Add("BusinessTypeCode", Convert.ToInt32(reader["BusinessTypeCode"]));
                    dictionaryClaim.Add("Number", Convert.ToInt32(reader["Number"]));
                    dictionaryClaim.Add("ClaimNoticeCode", Convert.ToInt32(reader["ClaimNoticeCode"]));
                    dictionaryClaim.Add("NoticeDate", Convert.ToDateTime(reader["NoticeDate"]));
                    dictionaryClaim.Add("PrefixCode", Convert.ToInt32(reader["PrefixCode"]));
                    dictionaryClaim.Add("PrefixDescription", Convert.ToString(reader["PrefixDescription"]));
                    dictionaryClaim.Add("CityCode", Convert.ToInt32(reader["CityCode"]));
                    dictionaryClaim.Add("StateCode", Convert.ToInt32(reader["StateCode"]));
                    dictionaryClaim.Add("CountryCode", Convert.ToInt32(reader["CountryCode"]));
                    dictionaryClaim.Add("Location", Convert.ToString(reader["Location"]));
                    dictionaryClaim.Add("ClaimDamageResponsibilityCode", Convert.ToInt32(reader["ClaimDamageResponsibilityCode"]));
                    dictionaryClaim.Add("ClaimDamageTypeCode", Convert.ToInt32(reader["ClaimDamageTypeCode"]));
                    dictionaryClaim.Add("CauseId", Convert.ToInt32(reader["CauseId"]));
                    dictionaryClaim.Add("Id", Convert.ToInt32(reader["Id"]));
                    dictionaryClaim.Add("SubCoveredRiskTypeCode", Convert.ToInt32(reader["SubCoveredRiskTypeCode"]));
                    dictionaryClaim.Add("CoveredRiskTypeCode", Convert.ToInt32(reader["CoveredRiskTypeCode"]));

                    switch ((SubCoveredRiskType)Convert.ToInt32(reader["SubCoveredRiskTypeCode"]))
                    {
                        case SubCoveredRiskType.Vehicle:
                        case SubCoveredRiskType.ThirdPartyLiability:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("LicensePlate", Convert.ToString(reader["LicensePlate"]));
                            }
                            break;
                        case SubCoveredRiskType.Property:
                        case SubCoveredRiskType.Liability:
                            //TODO: Validación del ramo 7 (Automoviles) por que el ramos tiene más de un tipo de riesgo cubierto
                            if (searchClaim.PrefixId != null && searchClaim.PrefixId != 7)
                            {
                                dictionaryClaim.Add("Street", Convert.ToString(reader["Street"]));
                            }
                            break;
                        case SubCoveredRiskType.Surety:
                        case SubCoveredRiskType.Lease:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.Transport:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("TransportCargoTypeCode", Convert.ToInt32(reader["TransportCargoTypeCode"]));
                            }
                            break;
                        case SubCoveredRiskType.Aircraft:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                        case SubCoveredRiskType.Marine:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                        case SubCoveredRiskType.Fidelity:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("RiskCommercialClassCode", Convert.ToInt32(reader["RiskCommercialClassCode"]));
                            }
                            break;
                    }

                    dictionaryClaims.Add(dictionaryClaim);
                }
            }

            foreach (var dictionaryClaim in dictionaryClaims)
            {
                Claim claim = new Claim
                {
                    Id = dictionaryClaim["ClaimCode"],
                    OccurrenceDate = dictionaryClaim["OccurrenceDate"],
                    Branch = new Branch
                    {
                        Id = dictionaryClaim["ClaimBranchCode"],
                        Description = dictionaryClaim["BranchDescription"]
                    },
                    Endorsement = new ClaimEndorsement
                    {
                        Id = dictionaryClaim["EndorsementId"],
                        PolicyId = dictionaryClaim["PolicyId"],
                        PolicyNumber = dictionaryClaim["DocumentNumber"],
                        RiskId = dictionaryClaim["RiskId"]
                    },
                    BusinessTypeId = dictionaryClaim["BusinessTypeCode"],
                    IndividualId = dictionaryClaim["IndividualId"],
                    Number = dictionaryClaim["Number"],
                    NoticeId = dictionaryClaim["ClaimNoticeCode"],
                    NoticeDate = dictionaryClaim["NoticeDate"],
                    Prefix = new Prefix
                    {
                        Id = dictionaryClaim["PrefixCode"],
                        Description = dictionaryClaim["PrefixDescription"]
                    },
                    City = new City
                    {
                        Id = dictionaryClaim["CityCode"],
                        State = new State
                        {
                            Id = dictionaryClaim["StateCode"],
                            Country = new Country
                            {
                                Id = dictionaryClaim["CountryCode"]
                            }
                        }
                    },
                    Location = dictionaryClaim["Location"],
                    DamageResponsability = new DamageResponsibility
                    {
                        Id = dictionaryClaim["ClaimDamageResponsibilityCode"]
                    },
                    DamageType = new DamageType
                    {
                        Id = dictionaryClaim["ClaimDamageTypeCode"]
                    },
                    Cause = new Cause
                    {
                        Id = dictionaryClaim["CauseId"]
                    },
                    TextOperation = new TextOperation
                    {
                        Id = dictionaryClaim["Id"]
                    },
                    CoveredRiskType = (CoveredRiskType)dictionaryClaim["CoveredRiskTypeCode"]
                };

                if (claim.NoticeId > 0)
                {
                    PrimaryKey key = CLMEN.ClaimNotice.CreatePrimaryKey(Convert.ToInt32(claim.NoticeId));
                    CLMEN.ClaimNotice entityClaimNotice = (CLMEN.ClaimNotice)DataFacadeManager.GetObject(key);

                    if (entityClaimNotice != null)
                    {
                        claim.NoticeInternalConsecutive = entityClaimNotice.InternalConsecutive;
                    }
                    else
                    {
                        claim.NoticeInternalConsecutive = String.Empty;
                    }
                }

                if (claim.TextOperation.Id > 0)
                {
                    claim.TextOperation = GetTextOperationByTextOperationId(claim.TextOperation.Id);
                }

                switch ((SubCoveredRiskType)dictionaryClaim["SubCoveredRiskTypeCode"])
                {
                    case SubCoveredRiskType.Vehicle:
                    case SubCoveredRiskType.ThirdPartyLiability:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["LicensePlate"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskVehicle.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(key);

                            if (entityRiskVehicle != null)
                            {
                                claim.RiskDescription = entityRiskVehicle.LicensePlate;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Property:
                    case SubCoveredRiskType.Liability:
                        //TODO: Validación del ramo 7 (Automoviles) por que el ramos tiene más de un tipo de riesgo cubierto
                        if (searchClaim.PrefixId != null && searchClaim.PrefixId != 7)
                        {
                            claim.RiskDescription = dictionaryClaim["Street"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskLocation.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(key);

                            if (entityRiskLocation != null)
                            {
                                claim.RiskDescription = entityRiskLocation.Street;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Surety:
                    case SubCoveredRiskType.Lease:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["InsuranceId"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskSurety.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskSurety != null)
                            {
                                claim.RiskDescription = Convert.ToString(entityRiskSurety.IndividualId);
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["InsuranceId"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskJudicialSurety.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskJudicialSurety entityRiskJudicialSurety = (ISSEN.RiskJudicialSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskJudicialSurety != null)
                            {
                                claim.RiskDescription = Convert.ToString(entityRiskJudicialSurety.InsuredId);
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Transport:
                        if (searchClaim.PrefixId != null)
                        {
                            PrimaryKey primaryKey = COMMEN.TransportCargoType.CreatePrimaryKey(dictionaryClaim["TransportCargoTypeCode"]);
                            COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(primaryKey);

                            claim.RiskDescription = entityTransportCargoType.Description;
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskTransport.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskTransport entityRiskTransport = (ISSEN.RiskTransport)DataFacadeManager.GetObject(key);
                            if (entityRiskTransport != null)
                            {

                                key = COMMEN.TransportCargoType.CreatePrimaryKey(entityRiskTransport.TransportCargoTypeCode);
                                COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(key);

                                claim.RiskDescription = entityTransportCargoType.Description;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Marine:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["AircraftDescription"] + " - " + dictionaryClaim["AircraftYear"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);
                            if (entityRiskAircraft != null)
                            {
                                claim.RiskDescription = entityRiskAircraft.AircraftDescription + " - " + entityRiskAircraft.AircraftYear;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Fidelity:
                        if (searchClaim.PrefixId != null)
                        {
                            PrimaryKey key = PARAMEN.RiskCommercialClass.CreatePrimaryKey(dictionaryClaim["RiskCommercialClassCode"]);
                            PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(key);

                            if (entityRiskCommercialClass != null)
                            {
                                claim.RiskDescription = (!string.IsNullOrEmpty(dictionaryClaim["FidelityDescription"]) ? dictionaryClaim["FidelityDescription"] + " - " : "") + entityRiskCommercialClass.Description;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskFidelity.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskFidelity entityRiskFidelity = (ISSEN.RiskFidelity)DataFacadeManager.GetObject(key);

                            if (entityRiskFidelity != null)
                            {
                                key = PARAMEN.RiskCommercialClass.CreatePrimaryKey(entityRiskFidelity.RiskCommercialClassCode);
                                PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(key);

                                claim.RiskDescription = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass.Description;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Aircraft:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["RegisterNo"] + " - " + dictionaryClaim["AircraftYear"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);
                            if (entityRiskAircraft != null)
                            {
                                claim.RiskDescription = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                }

                // Obtener la última modificación de la denuncia y si se busca por usuario la última modificación que realizó el usuario
                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, claim.Id);

                if (searchClaim.UserId != null)
                {
                    filter.And();
                    filter.Property(CLMEN.ClaimModify.Properties.UserId, typeof(CLMEN.ClaimModify).Name);
                    filter.Equal();
                    filter.Constant(searchClaim.UserId);
                }

                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                if (searchClaim.IndividualId != null && entityClaimModify != null)
                {
                    filter.Clear();
                    filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.IndividualId, typeof(CLMEN.ClaimCoverage).Name, searchClaim.IndividualId);
                    filter.And();
                    filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);

                    ClaimCoverageModifiesView claimCoverageModifiesView = new ClaimCoverageModifiesView();
                    ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageModifiesView");
                    viewBuilder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageModifiesView);

                    if (claimCoverageModifiesView.ClaimCoverages.Count > 0)
                    {
                        claim.Modifications = ModelAssembler.CreateClaimModifies(entityClaimModify);

                        claims.Add(claim);
                    }
                }
                else if (entityClaimModify != null)
                {
                    claim.Modifications = ModelAssembler.CreateClaimModifies(entityClaimModify);

                    claims.Add(claim);
                }
            }

            return claims;
        }


        public List<Claim> SearchClaimsBySalaryEstimation(SearchClaim searchClaim)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<Claim> claims = new List<Claim>();
            List<Dictionary<string, dynamic>> dictionaryClaims = new List<Dictionary<string, dynamic>>();
            bool emptyFilter = true;

            #region Filters

            if (searchClaim.ClaimNumber != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.Number, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.ClaimNumber);
                emptyFilter = false;
            }

            if (searchClaim.BranchId != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.BranchId);
                emptyFilter = false;
            }


            if (searchClaim.ClaimDateFrom != null && searchClaim.ClaimDateTo != null)
            {
                if (searchClaim.ClaimDateFrom > DateTime.MinValue)
                {

                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name);
                    filter.GreaterEqual();
                    filter.Constant(searchClaim.ClaimDateFrom);
                    emptyFilter = false;
                }

                if (searchClaim.ClaimDateTo > DateTime.MinValue)
                {
                    searchClaim.ClaimDateTo = Convert.ToDateTime(searchClaim.ClaimDateTo).Add(new TimeSpan(23, 59, 59));

                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name);
                    filter.LessEqual();
                    filter.Constant(searchClaim.ClaimDateTo);
                    emptyFilter = false;
                }

            }

            if (searchClaim.NoticeDateFrom != null && searchClaim.NoticeDateTo != null)
            {
                if (searchClaim.NoticeDateFrom > DateTime.MinValue)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.NoticeDate, typeof(CLMEN.Claim).Name);
                    filter.GreaterEqual();
                    filter.Constant(searchClaim.NoticeDateFrom);
                    emptyFilter = false;
                }

                if (searchClaim.NoticeDateTo > DateTime.MinValue)
                {
                    searchClaim.NoticeDateTo = Convert.ToDateTime(searchClaim.NoticeDateTo).Add(new TimeSpan(23, 59, 59));

                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.Claim.Properties.NoticeDate, typeof(CLMEN.Claim).Name);
                    filter.LessEqual();
                    filter.Constant(searchClaim.NoticeDateTo);
                    emptyFilter = false;
                }
            }

            if (searchClaim.DocumentNumber != null && searchClaim.DocumentNumber != "")
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.DocumentNumber);
                emptyFilter = false;
            }

            if (searchClaim.PrefixId != null)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name);
                filter.Equal();
                filter.Constant(searchClaim.PrefixId);
                emptyFilter = true;
            }



            #endregion

            #region Selects

            SelectQuery selectQuery = new SelectQuery();

            //Claims
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimCode, typeof(CLMEN.Claim).Name), "ClaimCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name), "OccurrenceDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name), "ClaimBranchCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.EndorsementId, typeof(CLMEN.Claim).Name), "EndorsementId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.IndividualId, typeof(CLMEN.Claim).Name), "IndividualId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.PolicyId, typeof(CLMEN.Claim).Name), "PolicyId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name), "DocumentNumber"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.BusinessTypeCode, typeof(CLMEN.Claim).Name), "BusinessTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.Number, typeof(CLMEN.Claim).Name), "Number"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimNoticeCode, typeof(CLMEN.Claim).Name), "ClaimNoticeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.NoticeDate, typeof(CLMEN.Claim).Name), "NoticeDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name), "PrefixCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.CityCode, typeof(CLMEN.Claim).Name), "CityCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.StateCode, typeof(CLMEN.Claim).Name), "StateCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.CountryCode, typeof(CLMEN.Claim).Name), "CountryCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.Location, typeof(CLMEN.Claim).Name), "Location"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimDamageResponsibilityCode, typeof(CLMEN.Claim).Name), "ClaimDamageResponsibilityCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.ClaimDamageTypeCode, typeof(CLMEN.Claim).Name), "ClaimDamageTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.CauseId, typeof(CLMEN.Claim).Name), "CauseId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.Claim.Properties.TextOperationCode, typeof(CLMEN.Claim).Name), "Id"));

            //Endorsement Risk
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name), "RiskId"));

            //Branch
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, typeof(COMMEN.Branch).Name), "BranchDescription"));

            //Prefix
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.Description, typeof(COMMEN.Prefix).Name), "PrefixDescription"));

            //Hard Risk Type
            selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.HardRiskType.Properties.SubCoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "2"), "SubCoveredRiskTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "2"), "CoveredRiskTypeCode"));

            #endregion

            #region Joins

            Join join = new Join(new ClassNameTable(typeof(CLMEN.Claim), typeof(CLMEN.Claim).Name), new ClassNameTable(typeof(COMMEN.Prefix), typeof(COMMEN.Prefix).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(COMMEN.Prefix.Properties.PrefixCode, typeof(COMMEN.Prefix).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), typeof(COMMEN.Branch).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.HardRiskType), typeof(PARAMEN.HardRiskType).Name + "1"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name + "1"))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.HardRiskType), typeof(PARAMEN.HardRiskType).Name + "2"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                 .Property(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name + "2")
                .And()
                .Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "1")
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name + "2"))
                .GetPredicate();


            join = new Join(join, new ClassNameTable(typeof(ISSEN.EndorsementRisk), typeof(ISSEN.EndorsementRisk).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.Claim.Properties.EndorsementId, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name)
                .And()
                .Property(CLMEN.Claim.Properties.PolicyId, typeof(CLMEN.Claim).Name)
                .Equal()
                .Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Risk), typeof(ISSEN.Risk).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                .Equal()
                .Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name))
                .GetPredicate();


            if (searchClaim.PrefixId != null)
            {
                switch ((SubCoveredRiskType)Convert.ToInt32(GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(Convert.ToInt32(searchClaim.PrefixId), GetClaimPrefixCoveredRiskTypeByPrefixCode(Convert.ToInt32(searchClaim.PrefixId))).SubCoveredRiskTypeCode))
                {
                    case SubCoveredRiskType.Vehicle:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                            .GetPredicate();
                        break;
                    //case SubCoveredRiskType.ThirdPartyLiability:
                    //selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                    //join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                    //join.Criteria = (new ObjectCriteriaBuilder()
                    //    .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                    //    .Equal()
                    //    .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                    //    .GetPredicate();
                    //break;
                    case SubCoveredRiskType.Property:
                    case SubCoveredRiskType.Liability:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name), "Street"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskLocation), typeof(ISSEN.RiskLocation).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Surety:
                    case SubCoveredRiskType.Lease:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskSurety), typeof(ISSEN.RiskSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskSurety.Properties.RiskId, typeof(ISSEN.RiskSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskJudicialSurety.Properties.InsuredId, typeof(ISSEN.RiskJudicialSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskJudicialSurety), typeof(ISSEN.RiskJudicialSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskJudicialSurety.Properties.RiskId, typeof(ISSEN.RiskJudicialSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Transport:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskTransport.Properties.TransportCargoTypeCode, typeof(ISSEN.RiskTransport).Name), "TransportCargoTypeCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskTransport), typeof(ISSEN.RiskTransport).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.RiskTransport).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Aircraft:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.RegisterNo, typeof(ISSEN.RiskAircraft).Name), "RegisterNo"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Marine:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftDescription, typeof(ISSEN.RiskAircraft).Name), "AircraftDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));


                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Fidelity:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.Description, typeof(ISSEN.RiskFidelity).Name), "FidelityDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.RiskCommercialClassCode, typeof(ISSEN.RiskFidelity).Name), "RiskCommercialClassCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskFidelity), typeof(ISSEN.RiskFidelity).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskFidelity.Properties.RiskId, typeof(ISSEN.RiskFidelity).Name))
                            .GetPredicate();
                        break;
                }
            }

            #endregion

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    Dictionary<string, dynamic> dictionaryClaim = new Dictionary<string, dynamic>();

                    dictionaryClaim.Add("ClaimCode", Convert.ToInt32(reader["ClaimCode"]));
                    dictionaryClaim.Add("OccurrenceDate", Convert.ToDateTime(reader["OccurrenceDate"]));
                    dictionaryClaim.Add("ClaimBranchCode", Convert.ToInt32(reader["ClaimBranchCode"]));
                    dictionaryClaim.Add("BranchDescription", Convert.ToString(reader["BranchDescription"]));
                    dictionaryClaim.Add("PolicyId", Convert.ToInt32(reader["PolicyId"]));
                    dictionaryClaim.Add("DocumentNumber", Convert.ToString(reader["DocumentNumber"]));
                    dictionaryClaim.Add("EndorsementId", Convert.ToInt32(reader["EndorsementId"]));
                    dictionaryClaim.Add("IndividualId", Convert.ToInt32(reader["IndividualId"]));
                    dictionaryClaim.Add("RiskId", Convert.ToInt32(reader["RiskId"]));
                    dictionaryClaim.Add("BusinessTypeCode", Convert.ToInt32(reader["BusinessTypeCode"]));
                    dictionaryClaim.Add("Number", Convert.ToInt32(reader["Number"]));
                    dictionaryClaim.Add("ClaimNoticeCode", Convert.ToInt32(reader["ClaimNoticeCode"]));
                    dictionaryClaim.Add("NoticeDate", Convert.ToDateTime(reader["NoticeDate"]));
                    dictionaryClaim.Add("PrefixCode", Convert.ToInt32(reader["PrefixCode"]));
                    dictionaryClaim.Add("PrefixDescription", Convert.ToString(reader["PrefixDescription"]));
                    dictionaryClaim.Add("CityCode", Convert.ToInt32(reader["CityCode"]));
                    dictionaryClaim.Add("StateCode", Convert.ToInt32(reader["StateCode"]));
                    dictionaryClaim.Add("CountryCode", Convert.ToInt32(reader["CountryCode"]));
                    dictionaryClaim.Add("Location", Convert.ToString(reader["Location"]));
                    dictionaryClaim.Add("ClaimDamageResponsibilityCode", Convert.ToInt32(reader["ClaimDamageResponsibilityCode"]));
                    dictionaryClaim.Add("ClaimDamageTypeCode", Convert.ToInt32(reader["ClaimDamageTypeCode"]));
                    dictionaryClaim.Add("CauseId", Convert.ToInt32(reader["CauseId"]));
                    dictionaryClaim.Add("Id", Convert.ToInt32(reader["Id"]));
                    dictionaryClaim.Add("SubCoveredRiskTypeCode", Convert.ToInt32(reader["SubCoveredRiskTypeCode"]));
                    dictionaryClaim.Add("CoveredRiskTypeCode", Convert.ToInt32(reader["CoveredRiskTypeCode"]));

                    switch ((SubCoveredRiskType)Convert.ToInt32(reader["SubCoveredRiskTypeCode"]))
                    {
                        case SubCoveredRiskType.Vehicle:
                        case SubCoveredRiskType.ThirdPartyLiability:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("LicensePlate", Convert.ToString(reader["LicensePlate"]));
                            }
                            break;
                        case SubCoveredRiskType.Property:
                        case SubCoveredRiskType.Liability:
                            //TODO: Validación del ramo 7 (Automoviles) por que el ramos tiene más de un tipo de riesgo cubierto
                            if (searchClaim.PrefixId != null && searchClaim.PrefixId != 7)
                            {
                                dictionaryClaim.Add("Street", Convert.ToString(reader["Street"]));
                            }
                            break;
                        case SubCoveredRiskType.Surety:
                        case SubCoveredRiskType.Lease:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.Transport:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("TransportCargoTypeCode", Convert.ToInt32(reader["TransportCargoTypeCode"]));
                            }
                            break;
                        case SubCoveredRiskType.Aircraft:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                        case SubCoveredRiskType.Marine:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                        case SubCoveredRiskType.Fidelity:
                            if (searchClaim.PrefixId != null)
                            {
                                dictionaryClaim.Add("RiskCommercialClassCode", Convert.ToInt32(reader["RiskCommercialClassCode"]));
                            }
                            break;
                    }

                    dictionaryClaims.Add(dictionaryClaim);
                }
            }

            foreach (var dictionaryClaim in dictionaryClaims)
            {
                Claim claim = new Claim
                {
                    Id = dictionaryClaim["ClaimCode"],
                    OccurrenceDate = dictionaryClaim["OccurrenceDate"],
                    Branch = new Branch
                    {
                        Id = dictionaryClaim["ClaimBranchCode"],
                        Description = dictionaryClaim["BranchDescription"]
                    },
                    Endorsement = new ClaimEndorsement
                    {
                        Id = dictionaryClaim["EndorsementId"],
                        PolicyId = dictionaryClaim["PolicyId"],
                        PolicyNumber = dictionaryClaim["DocumentNumber"],
                        RiskId = dictionaryClaim["RiskId"]
                    },
                    BusinessTypeId = dictionaryClaim["BusinessTypeCode"],
                    IndividualId = dictionaryClaim["IndividualId"],
                    Number = dictionaryClaim["Number"],
                    NoticeId = dictionaryClaim["ClaimNoticeCode"],
                    NoticeDate = dictionaryClaim["NoticeDate"],
                    Prefix = new Prefix
                    {
                        Id = dictionaryClaim["PrefixCode"],
                        Description = dictionaryClaim["PrefixDescription"]
                    },
                    City = new City
                    {
                        Id = dictionaryClaim["CityCode"],
                        State = new State
                        {
                            Id = dictionaryClaim["StateCode"],
                            Country = new Country
                            {
                                Id = dictionaryClaim["CountryCode"]
                            }
                        }
                    },
                    Location = dictionaryClaim["Location"],
                    DamageResponsability = new DamageResponsibility
                    {
                        Id = dictionaryClaim["ClaimDamageResponsibilityCode"]
                    },
                    DamageType = new DamageType
                    {
                        Id = dictionaryClaim["ClaimDamageTypeCode"]
                    },
                    Cause = new Cause
                    {
                        Id = dictionaryClaim["CauseId"]
                    },
                    TextOperation = new TextOperation
                    {
                        Id = dictionaryClaim["Id"]
                    },
                    CoveredRiskType = (CoveredRiskType)dictionaryClaim["CoveredRiskTypeCode"]
                };

                if (claim.NoticeId > 0)
                {
                    PrimaryKey key = CLMEN.ClaimNotice.CreatePrimaryKey(Convert.ToInt32(claim.NoticeId));
                    CLMEN.ClaimNotice entityClaimNotice = (CLMEN.ClaimNotice)DataFacadeManager.GetObject(key);
                    
                    if (entityClaimNotice != null)
                    {
                        claim.NoticeInternalConsecutive = entityClaimNotice.InternalConsecutive;
                    }
                    else
                    {
                        claim.NoticeInternalConsecutive = String.Empty;
                    }
                }

                if (claim.TextOperation.Id > 0)
                {
                    claim.TextOperation = GetTextOperationByTextOperationId(claim.TextOperation.Id);
                }

                switch ((SubCoveredRiskType)dictionaryClaim["SubCoveredRiskTypeCode"])
                {
                    case SubCoveredRiskType.Vehicle:
                    case SubCoveredRiskType.ThirdPartyLiability:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["LicensePlate"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskVehicle.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(key);

                            if (entityRiskVehicle != null)
                            {
                                claim.RiskDescription = entityRiskVehicle.LicensePlate;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Property:
                    case SubCoveredRiskType.Liability:
                        //TODO: Validación del ramo 7 (Automoviles) por que el ramos tiene más de un tipo de riesgo cubierto
                        if (searchClaim.PrefixId != null && searchClaim.PrefixId != 7)
                        {
                            claim.RiskDescription = dictionaryClaim["Street"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskLocation.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(key);

                            if (entityRiskLocation != null)
                            {
                                claim.RiskDescription = entityRiskLocation.Street;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Surety:
                    case SubCoveredRiskType.Lease:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["InsuranceId"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskSurety.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskSurety != null)
                            {
                                claim.RiskDescription = Convert.ToString(entityRiskSurety.IndividualId);
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["InsuranceId"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskJudicialSurety.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskJudicialSurety entityRiskJudicialSurety = (ISSEN.RiskJudicialSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskJudicialSurety != null)
                            {
                                claim.RiskDescription = Convert.ToString(entityRiskJudicialSurety.InsuredId);
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Transport:
                        if (searchClaim.PrefixId != null)
                        {
                            PrimaryKey primaryKey = COMMEN.TransportCargoType.CreatePrimaryKey(dictionaryClaim["TransportCargoTypeCode"]);
                            COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(primaryKey);

                            claim.RiskDescription = entityTransportCargoType.Description;
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskTransport.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskTransport entityRiskTransport = (ISSEN.RiskTransport)DataFacadeManager.GetObject(key);
                            if (entityRiskTransport != null)
                            {

                                key = COMMEN.TransportCargoType.CreatePrimaryKey(entityRiskTransport.TransportCargoTypeCode);
                                COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(key);

                                claim.RiskDescription = entityTransportCargoType.Description;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Marine:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["AircraftDescription"] + " - " + dictionaryClaim["AircraftYear"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);
                            if (entityRiskAircraft != null)
                            {
                                claim.RiskDescription = entityRiskAircraft.AircraftDescription + " - " + entityRiskAircraft.AircraftYear;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Fidelity:
                        if (searchClaim.PrefixId != null)
                        {
                            PrimaryKey key = PARAMEN.RiskCommercialClass.CreatePrimaryKey(dictionaryClaim["RiskCommercialClassCode"]);
                            PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(key);

                            if (entityRiskCommercialClass != null)
                            {
                                claim.RiskDescription = (!string.IsNullOrEmpty(dictionaryClaim["FidelityDescription"]) ? dictionaryClaim["FidelityDescription"] + " - " : "") + entityRiskCommercialClass.Description;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskFidelity.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskFidelity entityRiskFidelity = (ISSEN.RiskFidelity)DataFacadeManager.GetObject(key);

                            if (entityRiskFidelity != null)
                            {
                                key = PARAMEN.RiskCommercialClass.CreatePrimaryKey(entityRiskFidelity.RiskCommercialClassCode);
                                PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(key);

                                claim.RiskDescription = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass.Description;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                    case SubCoveredRiskType.Aircraft:
                        if (searchClaim.PrefixId != null)
                        {
                            claim.RiskDescription = dictionaryClaim["RegisterNo"] + " - " + dictionaryClaim["AircraftYear"];
                        }
                        else
                        {
                            PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(claim.Endorsement.RiskId);
                            ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);
                            if (entityRiskAircraft != null)
                            {
                                claim.RiskDescription = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;
                            }
                            else
                            {
                                claim.RiskDescription = Resources.Resources.RiskNotFound;
                            }
                        }
                        break;
                }

                // Obtener la última modificación de la denuncia y si se busca por usuario la última modificación que realizó el usuario
                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, claim.Id);
                
                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                if (entityClaimModify != null)
                {
                    filter.Clear();                    
                    filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);
                    filter.And();
                    filter.PropertyEquals(CLMEN.ClaimCoverageAmount.Properties.IsMinimumSalary, typeof(CLMEN.ClaimCoverageAmount).Name, searchClaim.IsMinimumSalary);
                    filter.And();
                    filter.Not();
                    filter.Property(CLMEN.ClaimCoverageAmount.Properties.MinimumSalaryValue, typeof(CLMEN.ClaimCoverageAmount).Name);
                    filter.Equal();
                    filter.Constant(searchClaim.CurrentMinimumSalaryValue);

                    ClaimCoverageModifiesView claimCoverageModifiesView = new ClaimCoverageModifiesView();
                    ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageModifiesView");
                    viewBuilder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageModifiesView);

                    List<PARAMEN.EstimationType> entityEstimationTypes = claimCoverageModifiesView.EstimationTypes.Cast<PARAMEN.EstimationType>().ToList();

                    if (claimCoverageModifiesView.ClaimCoverages.Count > 0)
                    {
                        claim.Modifications = ModelAssembler.CreateClaimModifies(entityClaimModify);

                        // Cargo las coberturas
                        claim.Modifications.First().Coverages = ModelAssembler.CreateClaimCoverages(claimCoverageModifiesView.ClaimCoverages);

                        foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                        {
                            // Cargo las estimaciones
                            claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimCoverageModifiesView.ClaimCoveragesAmount.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());

                            PrimaryKey key = ISSEN.Risk.CreatePrimaryKey(claimCoverage.RiskId);
                            ISSEN.Risk risk = (ISSEN.Risk)DataFacadeManager.GetObject(key);

                            claim.CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode;

                            switch (claim.CoveredRiskType)
                            {
                                case CoveredRiskType.Vehicle:
                                    PrimaryKey keyVehicle = ISSEN.RiskVehicle.CreatePrimaryKey(claimCoverage.RiskId);
                                    ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(keyVehicle);
                                    claimCoverage.RiskDescription = entityRiskVehicle.LicensePlate;
                                    break;
                                case CoveredRiskType.Location:
                                    PrimaryKey keyProperty = ISSEN.RiskLocation.CreatePrimaryKey(claimCoverage.RiskId);
                                    ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(keyProperty);
                                    claimCoverage.RiskDescription = entityRiskLocation.Street;
                                    break;
                                case CoveredRiskType.Surety:
                                    claimCoverage.RiskDescription = Convert.ToString(risk.InsuredId);
                                    break;
                                case CoveredRiskType.Transport:
                                    ClaimTransportDAO claimTransportDAO = new ClaimTransportDAO();
                                    claimCoverage.RiskDescription = claimTransportDAO.GetDescriptionRiskTransportByRiskId(claimCoverage.RiskId);
                                    break;
                                default:
                                    claimCoverage.RiskDescription = "";
                                    break;
                            }

                            foreach (Estimation estimation in claimCoverage.Estimations)
                            {                                
                                estimation.Type.Description = entityEstimationTypes.First(x => x.EstimateTypeCode == estimation.Type.Id).Description;                                
                            }

                        }

                        claims.Add(claim);
                    }
                }

            }

            return claims;
        }


        public int GetClaimPrefixCoveredRiskTypeByPrefixCode(int prefixCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name, prefixCode);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.HardRiskType), filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                return Convert.ToInt32(businessCollection.Cast<PARAMEN.HardRiskType>().ToList().First().CoveredRiskTypeCode);
            }
            else
            {
                return 0;
            }
        }

        public Driver GetClaimDriverInformationByClaimCoverageId(int claimCoverageId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimCoverageDriverInformation.CreatePrimaryKey(claimCoverageId);
            CLMEN.ClaimCoverageDriverInformation entityClaimCoverageDriverInformation = (CLMEN.ClaimCoverageDriverInformation)DataFacadeManager.GetObject(primaryKey);

            if (entityClaimCoverageDriverInformation != null)
            {
                return ModelAssembler.CreateDriver(entityClaimCoverageDriverInformation);
            }
            else
            {
                return null;
            }
        }

        public ThirdPartyVehicle GetClaimThirdPartyVehicleByClaimCoverageId(int claimCoverageId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimCoverageThirdPartyVehicle.CreatePrimaryKey(claimCoverageId);
            CLMEN.ClaimCoverageThirdPartyVehicle entityClaimCoverageThirdPartyVehicle = (CLMEN.ClaimCoverageThirdPartyVehicle)DataFacadeManager.GetObject(primaryKey);

            if (entityClaimCoverageThirdPartyVehicle != null)
            {
                return ModelAssembler.CreateThirdPartyVehicle(entityClaimCoverageThirdPartyVehicle);
            }
            else
            {
                return null;
            }
        }

        public Inspection GetInspectionByClaimId(int claimId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimSupplier.CreatePrimaryKey(claimId);
            CLMEN.ClaimSupplier entityClaimSupplier = (CLMEN.ClaimSupplier)DataFacadeManager.GetObject(primaryKey);

            if (entityClaimSupplier != null)
            {
                return ModelAssembler.CreateInspection(entityClaimSupplier);
            }
            else
            {
                return null;
            }
        }

        public CatastrophicEvent GetCatastrophicEventByClaimId(int claimId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimCatastrophicInformation.CreatePrimaryKey(claimId);
            CLMEN.ClaimCatastrophicInformation entityClaimCatastrophicInformation = (CLMEN.ClaimCatastrophicInformation)DataFacadeManager.GetObject(primaryKey);

            if (entityClaimCatastrophicInformation != null)
            {
                CatastrophicEvent catastrophicEvent = ModelAssembler.CreateClaimCatastrophicInformation(entityClaimCatastrophicInformation);

                PrimaryKey key = PARAMEN.Catastrophe.CreatePrimaryKey(catastrophicEvent.Catastrophe.Id);

                PARAMEN.Catastrophe catastrophe = (PARAMEN.Catastrophe)DataFacadeManager.GetObject(key);
                catastrophicEvent.Catastrophe.Description = catastrophe.Description;

                return catastrophicEvent;
            }
            else
            {
                return null;
            }
        }

        public Claim GetEstimationByClaimId(int claimId)
        {
            PrimaryKey primaryKey = CLMEN.Claim.CreatePrimaryKey(claimId);
            CLMEN.Claim entityClaim = (CLMEN.Claim)DataFacadeManager.GetObject(primaryKey);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (entityClaim != null)
            {
                Claim claim = ModelAssembler.CreateClaim(entityClaim);

                //Se arma el filtro para obtener la última modificación de la denuncia
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, entityClaim.ClaimCode);
                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                //Se obtiene la última modificación de la denuncia
                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                //Se agrega la última modificación de la denuncia
                claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, claim.Modifications.First().Id);

                ClaimCoverageView claimCoverageView = new ClaimCoverageView();
                ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageView);

                List<QUOEN.Coverage> entityCoverages = claimCoverageView.Coverages.Cast<QUOEN.Coverage>().ToList();
                List<COMMEN.Currency> entityCurrencies = claimCoverageView.Currencies.Cast<COMMEN.Currency>().ToList();
                List<PARAMEN.EstimationType> entityEstimationTypes = claimCoverageView.EstimationTypes.Cast<PARAMEN.EstimationType>().ToList();
                List<PARAMEN.EstimationTypeStatus> entityEstimationTypeStatuses = claimCoverageView.EstimationTypesStatuses.Cast<PARAMEN.EstimationTypeStatus>().ToList();
                List<PARAMEN.EstimationTypeInternalStatus> entityEstimationTypeInternalStatuses = claimCoverageView.EstimationTypeInternalStatuses.Cast<PARAMEN.EstimationTypeInternalStatus>().ToList();
                claim.Modifications.First().Coverages = ModelAssembler.CreateClaimCoverages(claimCoverageView.ClaimCoverages);

                foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                {
                    claimCoverage.Description = entityCoverages.First(x => x.CoverageId == claimCoverage.CoverageId).PrintDescription;
                    claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimCoverageView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());

                    PrimaryKey key = ISSEN.Risk.CreatePrimaryKey(claimCoverage.RiskId);
                    ISSEN.Risk risk = (ISSEN.Risk)DataFacadeManager.GetObject(key);

                    claim.CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode;

                    switch (claim.CoveredRiskType)
                    {
                        case CoveredRiskType.Vehicle:
                            PrimaryKey keyVehicle = ISSEN.RiskVehicle.CreatePrimaryKey(claimCoverage.RiskId);
                            ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(keyVehicle);
                            claimCoverage.RiskDescription = entityRiskVehicle.LicensePlate;
                            break;
                        case CoveredRiskType.Location:
                            PrimaryKey keyProperty = ISSEN.RiskLocation.CreatePrimaryKey(claimCoverage.RiskId);
                            ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(keyProperty);
                            claimCoverage.RiskDescription = entityRiskLocation.Street;
                            break;
                        case CoveredRiskType.Surety:
                            claimCoverage.RiskDescription = Convert.ToString(risk.InsuredId);
                            break;
                        case CoveredRiskType.Transport:
                            ClaimTransportDAO claimTransportDAO = new ClaimTransportDAO();
                            claimCoverage.RiskDescription = claimTransportDAO.GetDescriptionRiskTransportByRiskId(claimCoverage.RiskId);
                            break;
                        default:
                            claimCoverage.RiskDescription = "";
                            break;
                    }

                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        estimation.Currency.Description = entityCurrencies.First(x => x.CurrencyCode == estimation.Currency.Id).Description;
                        estimation.Type.Description = entityEstimationTypes.First(x => x.EstimateTypeCode == estimation.Type.Id).Description;
                        PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = entityEstimationTypeStatuses.First(x => x.EstimationTypeStatusCode == estimation.Reason.Status.Id);
                        PARAMEN.EstimationTypeInternalStatus entityEstimationTypeInternalStatus = entityEstimationTypeInternalStatuses.First(x => x.InternalStatusCode == entityEstimationTypeStatus.InternalStatusCode);
                        estimation.Reason.Status.InternalStatus = ModelAssembler.CreateInternalStatus(entityEstimationTypeInternalStatus);
                    }
                }

                return claim;
            }
            else
            {
                return null;
            }
        }

        private PARAMEN.HardRiskType GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(int prefixId, int riskTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(riskTypeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.HardRiskType), filter.GetPredicate());
            PARAMEN.HardRiskType entityHardRiskType = businessCollection.Cast<PARAMEN.HardRiskType>().FirstOrDefault();

            return entityHardRiskType;
        }

        #region Reserve
        public ClaimReserve SetClaimReserve(ClaimReserve claimReserve)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();

            // Se captura la ultima modificación
            ClaimModify claimModify = claimReserve.Claim.Modifications.Last();

            // Validación de Fechas
           
            object date = GetDatebyModuleCDbyModuledate(4,claimModify.RegistrationDate);

            claimModify.RegistrationDate = Convert.ToDateTime(date);
            claimModify.AccountingDate = Convert.ToDateTime(date);


            // Se guarda la modificación
            CLMEN.ClaimModify entityClaimModify = EntityAssembler.CreateClaimModify(claimModify, claimReserve.Claim.Id);
            DataFacadeManager.Insert(entityClaimModify);

            claimModify.Id = entityClaimModify.ClaimModifyCode;

            // Se guardan las coberturas de la ultima modificación
            foreach (ClaimCoverage claimCoverage in claimModify.Coverages)
            {
                if (claimCoverage.TextOperation.Operation != null)
                {
                    claimCoverage.TextOperation = CreateTextOperation(claimCoverage.TextOperation);
                }
                else
                {
                    claimCoverage.TextOperation = null;
                }
                CLMEN.ClaimCoverage entityClaimCoverage = EntityAssembler.CreateClaimCoverage(claimCoverage, claimModify.Id);

                //Siempre se inserta un nuevo registro por modificación de la denuncia
                DataFacadeManager.Insert(entityClaimCoverage);

                claimCoverage.Id = entityClaimCoverage.ClaimCoverageCode;

                foreach (Estimation estimation in claimCoverage.Estimations)
                {
                    if (estimation.Reason.Status.InternalStatus.Id == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_ESTIMATION_INTERNAL_STATUS_CLOSED)))
                    {
                        estimation.Amount = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(claimReserve.Claim.Id, claimCoverage.SubClaim, estimation.Type.Id, estimation.Currency.Id) + estimation.DeductibleAmount;
                    }


                    estimation.CreationDate = Convert.ToDateTime(date);
                    estimation.Version = 1;
                    estimation.Amount = estimation.Amount - estimation.AmountAccumulate;
                    estimation.AmountAccumulate += estimation.Amount;



                    CLMEN.ClaimCoverageAmount entityClaimCoverageAmount = EntityAssembler.CreateClaimCoverageAmount(estimation, claimCoverage.Id);

                    PrimaryKey key = CLMEN.ClaimCoverageAmount.CreatePrimaryKey(entityClaimCoverageAmount.ClaimCoverageCode, entityClaimCoverageAmount.EstimationTypeCode);
                    CLMEN.ClaimCoverageAmount coverageAmount = (CLMEN.ClaimCoverageAmount)DataFacadeManager.GetObject(key);

                    if (claimReserve.Claim.BusinessTypeId == 3 && claimReserve.Claim.IsTotalParticipation && claimReserve.Claim.CoInsuranceAssigned != null)
                    {
                        foreach (CoInsuranceAssigned coInsuranceAssigned in claimReserve.Claim.CoInsuranceAssigned)
                        {
                            ClaimCoverageCoInsurance claimCoverageCoInsurance = ModelAssembler.CreateClaimCoverageCoInsurance(estimation, claimCoverage, coInsuranceAssigned);
                            SaveClaimCoverageCoInsurance(claimCoverageCoInsurance, claimCoverage.Id, coInsuranceAssigned.CompanyNum, estimation.Type.Id);
                        }
                    }

                    //Siempre se inserta un nuevo registro por modificación de la denuncia
                    DataFacadeManager.Insert(entityClaimCoverageAmount);

                }
            }

            #region ClaimControl

            ClaimControl claimControl = ModelAssembler.CreateClaimControl(claimReserve);
            claimControl.Action = "U";

            CreateClaimControl(claimControl);

            #endregion

            return claimReserve;
        }

        public object GetDatebyModuleCDbyModuledate(int modulecd, DateTime moduledate)
        {

          //_- CompanyPolicy companyPolicy = new CompanyPolicy();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@MODULE_CD", modulecd);
        //    parameters[1] = new NameValue("@CLAIM_MODULE_ISSUE_DATE", moduledate);

            object result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("CLM.GET_CLAIM_MODULE_DATE", parameters);
            }

            if (result != null)
            {
                return result.ToString();
            }
            return null;

        }

        public Claim GetClaimReserveByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();

            if (branchId != null)
            {
                filter.PropertyEquals(CLMEN.Claim.Properties.ClaimBranchCode, typeof(CLMEN.Claim).Name, branchId);
                filter.And();
            }
            if (prefixId != null)
            {
                filter.PropertyEquals(CLMEN.Claim.Properties.PrefixCode, typeof(CLMEN.Claim).Name, prefixId);
                filter.And();
            }

            filter.PropertyEquals(CLMEN.Claim.Properties.DocumentNumber, typeof(CLMEN.Claim).Name, policyDocumentNumber);
            filter.And();

            filter.PropertyEquals(CLMEN.Claim.Properties.Number, typeof(CLMEN.Claim).Name, claimNumber);

            //Se consulta la cabecera de la denuncia
            CLMEN.Claim entityClaim = (CLMEN.Claim)DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate()).FirstOrDefault();

            if (entityClaim != null)
            {

                //Se arma el filtro para las modificaciones de la denuncia
                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, entityClaim.ClaimCode);

                string[] sortColumn = new string[1];
                sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                //Se obtiene la última modificación de la denuncia
                CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                filter.Clear();
                filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);

                ClaimCoverageView claimCoverageView = new ClaimCoverageView();
                ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageView);

                Claim claim = ModelAssembler.CreateClaim(entityClaim);

                if (claim.TextOperation?.Id > 0)
                {
                    claim.TextOperation = GetTextOperationByTextOperationId(claim.TextOperation.Id);
                }

                List<EstimationType> estimations = estimationTypeDAO.GetEstimationTypesByPrefixId(claim.Prefix.Id);
                List<QUOEN.Coverage> entityCoverages = claimCoverageView.Coverages.Cast<QUOEN.Coverage>().ToList();
                List<COMMEN.Currency> entityCurrencies = claimCoverageView.Currencies.Cast<COMMEN.Currency>().ToList();
                List<PARAMEN.EstimationType> entityEstimationTypes = claimCoverageView.EstimationTypes.Cast<PARAMEN.EstimationType>().ToList();
                List<PARAMEN.EstimationTypeStatus> entityEstimationTypeStatuses = claimCoverageView.EstimationTypesStatuses.Cast<PARAMEN.EstimationTypeStatus>().ToList();
                List<PARAMEN.EstimationTypeInternalStatus> entityEstimationTypeInternalStatuses = claimCoverageView.EstimationTypeInternalStatuses.Cast<PARAMEN.EstimationTypeInternalStatus>().ToList();

                //Se guarda la ultima modificación
                claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                claim.Modifications.First().Coverages = ModelAssembler.CreateClaimCoverages(claimCoverageView.ClaimCoverages);

                List<ClaimCoverage> claimCoverages = new List<ClaimCoverage>();

                foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                {
                    claimCoverage.Description = entityCoverages.First(x => x.CoverageId == claimCoverage.CoverageId).PrintDescription;
                    claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimCoverageView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());

                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = entityEstimationTypeStatuses.First(x => x.EstimationTypeStatusCode == estimation.Reason.Status.Id);
                        PARAMEN.EstimationTypeInternalStatus entityEstimationTypeInternalStatus = entityEstimationTypeInternalStatuses.First(x => x.InternalStatusCode == entityEstimationTypeStatus.InternalStatusCode);
                        estimation.Currency.Description = entityCurrencies.First(x => x.CurrencyCode == estimation.Currency.Id).Description;
                        estimation.Type.Description = entityEstimationTypes.First(x => x.EstimateTypeCode == estimation.Type.Id).Description;
                        estimation.Reason.Status.InternalStatus = ModelAssembler.CreateInternalStatus(entityEstimationTypeInternalStatus);
                    }

                    foreach (EstimationType estimationType in estimations)
                    {
                        Estimation modelEstimation = claimCoverage.Estimations.Find(X => X.Type.Id == estimationType.Id);

                        if (modelEstimation == null)
                        {
                            claimCoverages.Add(ModelAssembler.CreateClaimCoveraByModel(claimCoverage, estimationType, entityCurrencies));
                        }
                    }
                }

                foreach (ClaimCoverage claimCoverage in claimCoverages)
                {
                    claim.Modifications.First().Coverages.Add(claimCoverage);
                }

                return claim;
            }

            return null;
        }
        #endregion

        public List<Claim> GetClaimsByPolicyId(int policyId)
        {
            List<Claim> claims = new List<Claim>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Claim.Properties.PolicyId, typeof(CLMEN.Claim).Name, policyId);

            BusinessCollection entitiesClaim = DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate());

            if (entitiesClaim.Count > 0)
            {
                foreach (CLMEN.Claim entityClaim in entitiesClaim)
                {
                    Claim claim = ModelAssembler.CreateClaim(entityClaim);

                    //Se arma el filtro para las modificaciones de la denuncia
                    filter.Clear();
                    filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, entityClaim.ClaimCode);

                    string[] sortColumn = new string[1];
                    sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                    //Se obtiene la última modificación de la denuncia
                    CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                    //Se agrega la última modificación de la denuncia
                    claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                    filter.Clear();
                    filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);

                    ClaimsByPolicyView claimsByPolicyView = new ClaimsByPolicyView();
                    ViewBuilder viewBuilder = new ViewBuilder("claimsByPolicyView");
                    viewBuilder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimsByPolicyView);

                    List<CLMEN.ClaimCoverage> entityClaimCoverages = claimsByPolicyView.ClaimCoverages.Cast<CLMEN.ClaimCoverage>().ToList();

                    foreach (CLMEN.ClaimCoverage entityClaimCoverage in entityClaimCoverages)
                    {
                        claim.Modifications.First().Coverages.Add(ModelAssembler.CreateClaimCoverage(entityClaimCoverage));
                    }

                    foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                    {
                        claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimsByPolicyView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());
                    }

                    claims.Add(claim);
                }
            }

            return claims;
        }
        
        public List<Claim> GetClaimsByPolicyIdOccurrenceDate(int policyId, DateTime occurrenceDate)
        {
            // OccurrenceDate
            List<Claim> claims = new List<Claim>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Claim.Properties.PolicyId, typeof(CLMEN.Claim).Name, policyId);
            filter.And();
            filter.Property(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name);
            filter.GreaterEqual();
            filter.Constant(occurrenceDate);
            filter.And();
            filter.Property(CLMEN.Claim.Properties.OccurrenceDate, typeof(CLMEN.Claim).Name);
            filter.LessEqual();
            filter.Constant(occurrenceDate.Add(new TimeSpan(23,59,59)));

            BusinessCollection entitiesClaim = DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate());

            if (entitiesClaim.Count > 0)
            {
                foreach (CLMEN.Claim entityClaim in entitiesClaim)
                {
                    Claim claim = ModelAssembler.CreateClaim(entityClaim);

                    //Se arma el filtro para las modificaciones de la denuncia
                    filter.Clear();
                    filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, entityClaim.ClaimCode);

                    string[] sortColumn = new string[1];
                    sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                    //Se obtiene la última modificación de la denuncia
                    CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                    //Se agrega la última modificación de la denuncia
                    claim.Modifications.Add(ModelAssembler.CreateClaimModify(entityClaimModify));

                    filter.Clear();
                    filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);

                    ClaimsByPolicyView claimsByPolicyView = new ClaimsByPolicyView();
                    ViewBuilder viewBuilder = new ViewBuilder("claimsByPolicyView");
                    viewBuilder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimsByPolicyView);

                    List<CLMEN.ClaimCoverage> entityClaimCoverages = claimsByPolicyView.ClaimCoverages.Cast<CLMEN.ClaimCoverage>().ToList();

                    foreach (CLMEN.ClaimCoverage entityClaimCoverage in entityClaimCoverages)
                    {
                        claim.Modifications.First().Coverages.Add(ModelAssembler.CreateClaimCoverage(entityClaimCoverage));
                    }

                    foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                    {
                        claimCoverage.Estimations = ModelAssembler.CreateEstimations(claimsByPolicyView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList());
                    }

                    claims.Add(claim);
                }
            }

            return claims;

        }


        public Limit GetInsuredAmount(int policyId, int riskNum, int coverageId, int coverNum, int claimId, int subClaimId)
        {
            Limit limit = new Limit();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            //Se arma el filtro para las modificaciones de la denuncia
            filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimModify).Name, claimId);

            string[] sortColumn = new string[1];
            sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

            //Se obtiene la última modificación de la denuncia
            CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

            filter.Clear();
            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, entityClaimModify.ClaimModifyCode);
            filter.And();
            filter.PropertyEquals(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name, policyId);
            filter.And();
            filter.PropertyEquals(ISSEN.EndorsementRiskCoverage.Properties.RiskNum, typeof(ISSEN.EndorsementRiskCoverage).Name, riskNum);
            filter.And();
            filter.PropertyEquals(ISSEN.RiskCoverage.Properties.CoverageId, typeof(ISSEN.RiskCoverage).Name, coverageId);
            filter.And();
            filter.PropertyEquals(ISSEN.EndorsementRiskCoverage.Properties.CoverNum, typeof(ISSEN.EndorsementRiskCoverage).Name, coverNum);
            filter.And();
            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.CoverageId, typeof(CLMEN.ClaimCoverage).Name, coverageId);
            filter.And();
            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.CoverageNum, typeof(CLMEN.ClaimCoverage).Name, coverNum);

            ClaimInsuredAmountView claimInsuredAmountView = new ClaimInsuredAmountView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimInsuredAmountView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimInsuredAmountView);

            if (claimInsuredAmountView.RiskCoverages.Count > 0)
            {
                limit.InsuredAmount = Convert.ToDecimal(claimInsuredAmountView.RiskCoverages.Cast<ISSEN.RiskCoverage>().DistinctBy(x => x.CoverageId).Sum(x => x.SublimitAmount));
            }

            limit.Payment = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(claimId, subClaimId);

            if (limit.InsuredAmount > 0)
            {
                limit.Consumption = Math.Round((limit.Payment * 100) / limit.InsuredAmount, 2);
                limit.Reserve = (claimInsuredAmountView.ClaimCoverageAmounts?.Count > 0) ? claimInsuredAmountView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Sum(x => x.EstimateAmountAccumulate) - limit.Payment : 0;
                limit.PaymentConsumption = 0;
            }
            else
            {
                limit.InsuredAmount = 0;
            }
            return limit;
        }

        public CoveragePaymentConcept CreatePaymentConcept(CoveragePaymentConcept coveragePaymentConcepts)
        {
            CLMEN.CoveragePaymentConcept entityPaymentConcept = EntityAssembler.CreatePaymentConcept(coveragePaymentConcepts);
            DataFacadeManager.Insert(entityPaymentConcept);

            return ModelAssembler.CreatePaymentConcept(entityPaymentConcept);
        }

        public void DeletePaymentConcept(int conceptId, int coverageId, int estimationTypeId)
        {
            PrimaryKey primaryKey = CLMEN.CoveragePaymentConcept.CreatePrimaryKey(conceptId, coverageId, estimationTypeId);
            DataFacadeManager.Delete(primaryKey);
        }

        public string GetAffectedPropertyByClaimCoverageId(int claimCoverageId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimCoverage.CreatePrimaryKey(claimCoverageId);
            ClaimCoverage claimCoverage = ModelAssembler.CreateClaimCoverage((CLMEN.ClaimCoverage)DataFacadeManager.GetObject(primaryKey));

            if (claimCoverage != null)
            {
                return GetTextOperationByTextOperationId(claimCoverage.TextOperation.Id)?.Operation;
            }

            return null;
        }

        public ClaimCoverage GetClaimedAmountByClaimCoverageId(int claimCoverageId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimCoverage.CreatePrimaryKey(claimCoverageId);
            CLMEN.ClaimCoverage entityClaimCoverage = (CLMEN.ClaimCoverage)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateClaimCoverage(entityClaimCoverage);
        }

        public Driver CreateDriver(Driver driver, int claimCoverageId)
        {
            CLMEN.ClaimCoverageDriverInformation entityDriverInformation = EntityAssembler.CreateDriver(driver);
            entityDriverInformation.ClaimCoverageCode = claimCoverageId;

            return ModelAssembler.CreateDriver((CLMEN.ClaimCoverageDriverInformation)DataFacadeManager.Insert(entityDriverInformation));
        }

        public ThirdPartyVehicle CreateThirdPartyVehicle(ThirdPartyVehicle thirdPartyVehicle, int claimCoverageId)
        {
            CLMEN.ClaimCoverageThirdPartyVehicle entityThirdPartyVehicle = EntityAssembler.CreateThirdPartyVehicle(thirdPartyVehicle);
            entityThirdPartyVehicle.ClaimCoverageCode = claimCoverageId;

            return ModelAssembler.CreateThirdPartyVehicle((CLMEN.ClaimCoverageThirdPartyVehicle)DataFacadeManager.Insert(entityThirdPartyVehicle));
        }

        public ThirdAffected CreateThirdAffected(ThirdAffected thirdAffected, int claimCoverageId)
        {
            CLMEN.ClaimCoverageThirdAffected entityclaimCoverageThirdAffected = EntityAssembler.CreateThirdAffected(thirdAffected);
            entityclaimCoverageThirdAffected.ClaimCoverageCode = claimCoverageId;

            return ModelAssembler.CreateThirdAffected((CLMEN.ClaimCoverageThirdAffected)DataFacadeManager.Insert(entityclaimCoverageThirdAffected));
        }

        public List<ThirdAffected> GetThirdAffectedByClaimCoverageId(int claimCoverageId)
        {
            ViewBuilder builder = new ViewBuilder("ClaimCoverageThirdAffectedView");
            List<ThirdAffected> claimCoverageThirdAffecteds = new List<ThirdAffected>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            ClaimCoverageThirdAffectedView view = new ClaimCoverageThirdAffectedView();

            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimCoverageCode, typeof(CLMEN.ClaimCoverage).Name, claimCoverageId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.ClaimCoverageThirdAffecteds.Count > 0)
            {
                claimCoverageThirdAffecteds = ModelAssembler.CreateThirdAffecteds(view.ClaimCoverageThirdAffecteds);
                return claimCoverageThirdAffecteds;
            }
            else
            {
                return null;
            }
        }
        public TextOperation CreateTextOperation(TextOperation textOperation)
        {
            return ModelAssembler.CreateTextOperation((CLMEN.TextOperation)DataFacadeManager.Insert(EntityAssembler.CreateTextOperation(textOperation)));
        }

        public TextOperation GetTextOperationByTextOperationId(int TextOperationId)
        {
            PrimaryKey primaryKey = CLMEN.TextOperation.CreatePrimaryKey(TextOperationId);
            CLMEN.TextOperation entityTextOperation = (CLMEN.TextOperation)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateTextOperation(entityTextOperation);
        }

        public List<Estimation> GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(int claimModifyId, int prefixId, int coverageId, int individualId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, claimModifyId);
            filter.And();
            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.CoverageId, typeof(CLMEN.ClaimCoverage).Name, coverageId);
            filter.And();
            filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.IndividualId, typeof(CLMEN.ClaimCoverage).Name, individualId);

            EstimationTypesClaimView estimationTypesClaimView = new EstimationTypesClaimView();
            ViewBuilder viewBuilder = new ViewBuilder("EstimationTypesClaimView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, estimationTypesClaimView);
            List<EstimationType> estimationTypes = estimationTypeDAO.GetEstimationTypesByPrefixId(prefixId);
            List<Estimation> estimationsReturn = new List<Estimation>();

            if (estimationTypesClaimView.ClaimCoverages.Count > 0)
            {
                List<CLMEN.ClaimCoverageAmount> entityClaimCoverageAmounts = estimationTypesClaimView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().ToList();
                List<PARAMEN.EstimationType> entityEstimationTypes = estimationTypesClaimView.EstimationTypes.Cast<PARAMEN.EstimationType>().ToList();
                List<PARAMEN.EstimationTypeStatus> entityEstimationTypeStatuses = estimationTypesClaimView.EstimationTypeStatuses.Cast<PARAMEN.EstimationTypeStatus>().ToList();
                List<PARAMEN.EstimationTypeStatusReason> entityEstimationTypeStatusReasons = estimationTypesClaimView.EstimationTypeStatusReasons.Cast<PARAMEN.EstimationTypeStatusReason>().ToList();
                List<COMMEN.Currency> entityCurrencies = estimationTypesClaimView.Currencies.Cast<COMMEN.Currency>().ToList();
                List<PAYMEN.PaymentRequest> entityPaymentRequests = estimationTypesClaimView.PaymentRequests.Cast<PAYMEN.PaymentRequest>().ToList();
                List<PAYMEN.PaymentRequestClaim> entityPaymentRequestClaims = estimationTypesClaimView.PaymentRequestClaims.Cast<PAYMEN.PaymentRequestClaim>().ToList();
                List<PARAMEN.EstimationTypeInternalStatus> entityEstimationTypeInternalStatuses = estimationTypesClaimView.EstimationTypeInternalStatuses.Cast<PARAMEN.EstimationTypeInternalStatus>().ToList();

                List<Estimation> estimations = ModelAssembler.CreateEstimations(entityClaimCoverageAmounts);

                foreach (EstimationType estimationType in estimationTypes)
                {
                    Estimation payEstimation = estimationsReturn.FirstOrDefault(x => x.Type.Id == estimationType.Id);

                    if (payEstimation == null)
                    {
                        Estimation estimation = estimations.FirstOrDefault(x => x.Type.Id == estimationType.Id);
                        if (estimation != null)
                        {
                            PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = entityEstimationTypeStatuses.FirstOrDefault(X => X.EstimationTypeStatusCode == estimation.Reason.Status.Id);
                            estimation.Type.Description = entityEstimationTypes.FirstOrDefault(X => X.EstimateTypeCode == estimation.Type.Id).Description;
                            estimation.Reason.Description = entityEstimationTypeStatusReasons.FirstOrDefault(X => X.EstimationTypeStatusReasonCode == estimation.Reason.Id).Description;
                            estimation.Reason.Status.Description = entityEstimationTypeStatus.Description;
                            estimation.Reason.Status.InternalStatus = new InternalStatus
                            {
                                Id = entityEstimationTypeStatus.InternalStatusCode,
                                Description = entityEstimationTypeInternalStatuses.First(x => x.InternalStatusCode == entityEstimationTypeStatus.InternalStatusCode).Description
                            };
                            estimation.Currency.Description = entityCurrencies.FirstOrDefault(X => X.CurrencyCode == estimation.Currency.Id).Description;

                            int paymentRequestCode = Convert.ToInt32(entityPaymentRequestClaims.FirstOrDefault(x => x.EstimationTypeCode == estimationType.Id)?.PaymentRequestCode);

                            if (paymentRequestCode != 0)
                            {
                                estimation.PaymentAmount = Convert.ToDecimal(entityPaymentRequests.First(X => X.PaymentRequestCode == paymentRequestCode).TotalAmount);
                            }

                            estimationsReturn.Add(estimation);
                        }
                        else
                        {
                            estimationsReturn.Add(new Estimation()
                            {
                                Type = new EstimationType
                                {
                                    Id = estimationType.Id,
                                    Description = estimationType.Description
                                }
                            });
                        }
                    }
                }

                return estimationsReturn;
            }
            else
            {
                foreach (EstimationType estimationType in estimationTypes)
                {
                    estimationsReturn.Add(new Estimation()
                    {
                        Type = new EstimationType
                        {
                            Id = estimationType.Id,
                            Description = estimationType.Description
                        }
                    });
                }

                return estimationsReturn;
            }
        }

        public Claim GetClaimModifiesByClaimId(int claimId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Claim.Properties.ClaimCode, typeof(CLMEN.Claim).Name, claimId);

            ClaimsModifiesView claimsModifiesByClaimView = new ClaimsModifiesView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimsModifiesView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimsModifiesByClaimView);

            if (claimsModifiesByClaimView.Claims.Count > 0)
            {
                CLMEN.Claim entityClaim = claimsModifiesByClaimView.Claims.Cast<CLMEN.Claim>().FirstOrDefault();
                Claim claim = ModelAssembler.CreateClaim(entityClaim);

                List<CLMEN.ClaimModify> entityClaimModifies = claimsModifiesByClaimView.ClaimModifies.Cast<CLMEN.ClaimModify>().ToList();
                claim.Modifications = ModelAssembler.CreateClaimModifies(entityClaimModifies);

                foreach (ClaimModify claimModify in claim.Modifications)
                {
                    List<CLMEN.ClaimCoverage> entityClaimCoverages = claimsModifiesByClaimView.ClaimCoverages.Cast<CLMEN.ClaimCoverage>().Where(x => x.ClaimModifyCode == claimModify.Id).ToList();
                    claimModify.Coverages = ModelAssembler.CreateClaimCoverages(entityClaimCoverages);

                    UniqueUser.Entities.UniqueUsers entityUniqueUsers = claimsModifiesByClaimView.UniqueUsers.Cast<UniqueUser.Entities.UniqueUsers>().First(x => x.UserId == claimModify.UserId);
                    claimModify.UserName = entityUniqueUsers.AccountName;
                    foreach (ClaimCoverage claimCoverage in claimModify.Coverages)
                    {
                        QUOEN.Coverage entityCoverages = claimsModifiesByClaimView.Coverages.Cast<QUOEN.Coverage>().First(x => x.CoverageId == claimCoverage.CoverageId);
                        claimCoverage.Description = entityCoverages.PrintDescription;
                        List<CLMEN.ClaimCoverageAmount> entityclaimCoverageAmounts = claimsModifiesByClaimView.ClaimCoverageAmounts.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList();
                        claimCoverage.Estimations = ModelAssembler.CreateClaimCoverageAmounts(entityclaimCoverageAmounts);
                        claimCoverage.RiskDescription = GetRiskDescriptionByRiskIdPrefixId(claimCoverage.RiskId, claim.Prefix.Id);
                        foreach (Estimation estimation in claimCoverage.Estimations)
                        {
                            COMMEN.Currency entityCurrency = claimsModifiesByClaimView.Currencies.Cast<COMMEN.Currency>().First(x => x.CurrencyCode == estimation.Currency.Id);
                            estimation.Currency.Description = entityCurrency.Description;
                            PARAMEN.EstimationType entityEstimationTypes = claimsModifiesByClaimView.EstimationTypes.Cast<PARAMEN.EstimationType>().First(x => x.EstimateTypeCode == estimation.Type.Id);
                            PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = claimsModifiesByClaimView.EstimationTypesStatuses.Cast<PARAMEN.EstimationTypeStatus>().FirstOrDefault(x => x.EstimationTypeStatusCode == estimation.EstimationTypeStatus.Id);
                            estimation.Type.Description = entityEstimationTypes.Description;
                            estimation.EstimationTypeStatus.Description = entityEstimationTypeStatus.Description;
                            estimation.Reason.Status = new Status();
                        }
                    }
                }
                return claim;
            }

            return null;

        }

        public ClaimCoverageCoInsurance SaveClaimCoverageCoInsurance(ClaimCoverageCoInsurance claimCoverageCoInsurance, int claimcoverage, int companyId, int estimationTypeId)
        {
            return ModelAssembler.CreateClaimCoverageCoInsurance((CLMEN.ClaimCoverageCoinsurance)DataFacadeManager.Insert(EntityAssembler.CreateClaimCoverageCoInsurance(claimCoverageCoInsurance, claimcoverage, companyId, estimationTypeId)));
        }

        public string GetRiskDescriptionByRiskIdPrefixId(int riskId, int prefixId)
        {
            string riskDescription = string.Empty;
            PrimaryKey primaryKey = ISSEN.Risk.CreatePrimaryKey(riskId);
            ISSEN.Risk entityRisk = (ISSEN.Risk)DataFacadeManager.GetObject(primaryKey);

            if (entityRisk != null)
            {
                switch ((SubCoveredRiskType)GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(prefixId, entityRisk.CoveredRiskTypeCode).SubCoveredRiskTypeCode)
                {
                    case SubCoveredRiskType.Vehicle:
                    case SubCoveredRiskType.ThirdPartyLiability:
                        primaryKey = ISSEN.RiskVehicle.CreatePrimaryKey(riskId);
                        ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(primaryKey);

                        if (entityRiskVehicle != null)
                        {
                            riskDescription = entityRiskVehicle.LicensePlate;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.Property:
                    case SubCoveredRiskType.Liability:
                        primaryKey = ISSEN.RiskLocation.CreatePrimaryKey(riskId);
                        ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(primaryKey);

                        if (entityRiskLocation != null)
                        {
                            riskDescription = entityRiskLocation.Street;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.Surety:
                    case SubCoveredRiskType.Lease:
                        primaryKey = ISSEN.RiskSurety.CreatePrimaryKey(riskId);
                        ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.GetObject(primaryKey);

                        if (entityRiskSurety != null)
                        {
                            InsuredDTO suretyIssuanceInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskSurety.IndividualId), InsuredSearchType.IndividualId, CustomerType.Individual).First();

                            riskDescription = suretyIssuanceInsured.FullName;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        primaryKey = ISSEN.RiskJudicialSurety.CreatePrimaryKey(riskId);
                        ISSEN.RiskJudicialSurety entityRiskJudicialSurety = (ISSEN.RiskJudicialSurety)DataFacadeManager.GetObject(primaryKey);

                        if (entityRiskJudicialSurety != null)
                        {
                            InsuredDTO judicialIssuanceInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskJudicialSurety.InsuredId), InsuredSearchType.IndividualId, CustomerType.Individual).First();

                            riskDescription = judicialIssuanceInsured.FullName;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.Transport:
                        primaryKey = ISSEN.RiskTransport.CreatePrimaryKey(riskId);
                        ISSEN.RiskTransport entityRiskTransport = (ISSEN.RiskTransport)DataFacadeManager.GetObject(primaryKey);
                        if (entityRiskTransport != null)
                        {
                            primaryKey = COMMEN.TransportCargoType.CreatePrimaryKey(entityRiskTransport.TransportCargoTypeCode);
                            COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(primaryKey);

                            riskDescription = entityTransportCargoType.Description;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.Aircraft:
                        primaryKey = ISSEN.RiskAircraft.CreatePrimaryKey(riskId);
                        ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(primaryKey);
                        if (entityRiskAircraft != null)
                        {
                            riskDescription = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.Marine:
                        primaryKey = ISSEN.RiskAircraft.CreatePrimaryKey(riskId);
                        ISSEN.RiskAircraft entityRiskMarine = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(primaryKey);
                        if (entityRiskMarine != null)
                        {
                            riskDescription = entityRiskMarine.AircraftDescription + " - " + entityRiskMarine.AircraftYear;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                    case SubCoveredRiskType.Fidelity:
                        primaryKey = ISSEN.RiskFidelity.CreatePrimaryKey(riskId);
                        ISSEN.RiskFidelity entityRiskFidelity = (ISSEN.RiskFidelity)DataFacadeManager.GetObject(primaryKey);

                        if (entityRiskFidelity != null)
                        {
                            primaryKey = PARAMEN.RiskCommercialClass.CreatePrimaryKey(entityRiskFidelity.RiskCommercialClassCode);
                            PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(primaryKey);

                            riskDescription = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass.Description;
                        }
                        else
                        {
                            riskDescription = Resources.Resources.RiskNotFound;
                        }
                        break;
                }
            }

            return riskDescription;
        }

        public List<Claim> GetClaimsByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Claim.Properties.IndividualId, typeof(CLMEN.Claim).Name, individualId);

            return ModelAssembler.CreateClaims(DataFacadeManager.GetObjects(typeof(CLMEN.Claim), filter.GetPredicate()));
        }

        public int GetInsuredIdByRiskId(int riskId)
        {
            PrimaryKey key = ISSEN.Risk.CreatePrimaryKey(riskId);
            return ((ISSEN.Risk)DataFacadeManager.GetObject(key)).InsuredId;
        }

        private ClaimControl CreateClaimControl(ClaimControl claimControl)
        {
            return ModelAssembler.CreateClaimControl((INTEN.ClmClaimControl)DataFacadeManager.Insert(EntityAssembler.CreateClaimControl(claimControl)));
        }
    }
}

