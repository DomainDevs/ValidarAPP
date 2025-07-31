using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniqueUserServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PAYMENMOD = Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using UNPMOD = Sistran.Core.Application.UniquePersonService.V1.Models;
using UNDMOD = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using UTILTASK = Sistran.Core.Application.Utilities.Utility;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using Sistran.Core.Application.ClaimServices.EEProvider.Helpers;
using GLWKDTO = Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;

namespace Sistran.Core.Application.ClaimServices.EEProvider
{
    public class PaymentRequestApplicationServiceProvider : IPaymentRequestApplicationService
    {
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        public List<PrefixDTO> GetPrefixes()
        {
            return DTOAssembler.CreatePrefixes(DelegateService.commonServiceCore.GetPrefixes());
        }

        public List<BranchDTO> GetBranchesByUserId(int userId)
        {
            return DTOAssembler.CreateBranches(DelegateService.uniqueUserService.GetBranchesByUserId(userId));
        }

        public List<CurrencyDTO> GetCurrencies()
        {
            return DTOAssembler.CreateCurrencies(DelegateService.commonServiceCore.GetCurrencies());
        }

        public List<PaymentSourceDTO> GetPaymentSource()
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreatePaymentSources(DelegateService.claimsWorkerIntegrationService.GetConceptSources());
        }

        //public List<PaymentMovementTypeDTO> GetPaymentMovementType()
        //{
        //    PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
        //    return DTOAssembler.CreateMovementTypes(paymentRequestDAO.GetMovementTypes());
        //}

        public List<RoleDTO> GetRoles()
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreateRole(paymentRequestDAO.GetRoles());
        }

        public List<VoucherTypeDTO> GetVoucherTypes()
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreateVoucherTypes(paymentRequestDAO.GetVoucherTypes());
        }

        public List<PaymentMethodDTO> GetPaymentMethods()
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreatePaymentMethods(paymentRequestDAO.GetPaymentMethods());
        }

        public decimal GetExchangeRateByCurrencyId(int currencyId)
        {
            return DTOAssembler.CreateExchangeRate(DelegateService.commonServiceCore.GetExchangeRateByCurrencyId(currencyId)).BuyAmount;
        }

        public List<SelectDTO> GetDocumentTypeById(int documentTypeId)
        {
            return DTOAssembler.CreateDocumentTypes(DelegateService.uniquePersonServiceCore.GetDocumentTypes(documentTypeId));
        }

        public PaymentRequestDTO CreatePaymentRequest(PaymentRequestDTO paymentRequestDTO)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            VoucherDAO voucherDAO = new VoucherDAO();
            List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = new List<CoInsuranceAssignedDTO>();
            GLWKDTO.AccountingPaymentRequestDTO accountingPaymentRequestDTO = new GLWKDTO.AccountingPaymentRequestDTO();

            paymentRequestDTO.Number = paymentRequestDAO.GetPaymentRequestNumerByBranchId(paymentRequestDTO.BranchId);

            if (Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_ENABLE_GENERAL_LEDGER))))
            {
                paymentRequestDTO.TechnicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(new TechnicalTransactionParameterDTO { BranchId = paymentRequestDTO.BranchId }).Id;
            }
                        
            PaymentRequest paymentRequest = ModelAssembler.CreatePaymentRequest(paymentRequestDTO);

            // Carga información de Coaseguro
            foreach (Claim claim in paymentRequest.Claims)
            {
                if (claim.BusinessTypeId == 3 && claim.IsTotalParticipation)
                {
                    coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claim.Endorsement.PolicyId, claim.Endorsement.Id));
                }

                claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);                
            }

            // Validación y cargue de la contabilidad
            accountingPaymentRequestDTO = DelegateService.claimsWorkerIntegrationService.LoadAccountingPaymentRequest(DTOAssembler.CreateAccountingPaymentRequest(paymentRequestDTO, coInsuranceAssignedDTOs, paymentRequestDTO.TechnicalTransaction));

            if (paymentRequestDTO.TemporalId == 0)
            {
                paymentRequestDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(paymentRequest);                

                if (paymentRequestDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!paymentRequestDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        paymentRequest.AccountingPayment = ModelAssembler.CreateAccountingPaymentRequest(accountingPaymentRequestDTO);

                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        paymentRequestDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByPaymentRequest(paymentRequest)).Id;
                    }

                    return paymentRequestDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(paymentRequestDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.PaymentAuthorizationRequestPending);
                }

                if (paymentRequest.AuthorizationPolicies.Any())
                {
                    paymentRequest.AuthorizationPolicies.Clear();
                }
            }

            PaymentRequest paymentRequestModel = paymentRequestDAO.CreatePaymentRequest(paymentRequest);
            paymentRequestDTO.Id = paymentRequestModel.Id;              

            foreach (Claim claim in paymentRequest.Claims)
            {                
                foreach (Models.Claim.CoInsuranceAssigned coInsuranceAssigned in claim.CoInsuranceAssigned)
                {
                    PaymentRequestCoInsurance paymentRequestCoInsurance = ModelAssembler.CreatePaymentRequestCoInsurance(paymentRequest, coInsuranceAssigned, Convert.ToInt32(claim.Modifications.Last().Coverages?.First().CoverageId));

                    if (paymentRequest.Claims.IndexOf(claim) == 0 || paymentRequestDAO.GetPaymentRequestCoInsuranceByPaymentRequestIdCompanyId(paymentRequest.Id, paymentRequestCoInsurance.CompanyId) == null)
                    {
                        paymentRequestDAO.SavePaymentRequestCoInsurance(paymentRequestCoInsurance, paymentRequest.Id, paymentRequestCoInsurance.CompanyId);
                    }
                }

                voucherDAO.CreateVouchers(claim, paymentRequestModel);

                if (paymentRequestDTO.IsTotal)
                {
                    DelegateService.claimApplicationService.SetClaimReserveByClaimIdSubClaimEstimationTypeIdPaymentUserId(claim.Id, Convert.ToInt32(claim.Modifications.Last().Coverages?.First().SubClaim), claim.Modifications.Last().Coverages.First().Estimations.First().Type.Id, paymentRequest.UserId);
                }
            }

            #region PaymentRequestControl

            paymentRequestDAO.CreatePaymentRequestControl(paymentRequestModel);

            #endregion

            if (Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_ENABLE_GENERAL_LEDGER))))
            {                
                SaveAccountingPaymentRequest(accountingPaymentRequestDTO);
            }
            else
            {
                paymentRequestDTO.SaveDailyEntryMessage = Resources.Resources.IntegrationServiceDisabledLabel;
            }

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsurancePaymentRequest(paymentRequestDTO.Id, paymentRequestDTO.UserId);
            }
            #endregion

            return paymentRequestDTO;
        }

        public PaymentRequestDTO SaveRequestCancellation(int paymentRequestId, bool IsChargeRequest)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            PaymentRequest paymentRequest = paymentRequestDAO.SaveRequestCancellation(paymentRequestId, IsChargeRequest);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceCancellationPayment(paymentRequest.Id, paymentRequest.UserId);
            }
            #endregion

            return DTOAssembler.CreatePaymentRequest(paymentRequest);
        }

        public List<PersonTypeDTO> GetPersonTypes(bool isPaymentRequest)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreatePersonTypes(paymentRequestDAO.GetPersonTypes(isPaymentRequest));
        }

        public List<SelectDTO> GetMovementTypesByConceptSourceId(int conceptSourceId)
        {
            return DTOAssembler.CreateMovementTypesSelect(DelegateService.claimsWorkerIntegrationService.GetMovementTypesByConceptSourceId(conceptSourceId));
        }

        public List<int> GetEstimationsTypesIdByMovementTypeId(int movementTypeId)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return paymentRequestDAO.GetEstimationsTypesIdByMovementTypeId(movementTypeId);
        }

        public List<IndividualDTO> GetInsuredsByDescriptionIndividualSearchType(string description, InsuredSearchType insuredSearchType, CustomerType customerType)
        {
            return DTOAssembler.CreateIndividuals(DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public List<SelectDTO> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId)
        {
            return DTOAssembler.CreateAccountingConceptsSelect(DelegateService.claimsWorkerIntegrationService.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(branchId, movementTypeId, personTypeId, individualId));
        }

        public List<AccountBankDTO> GetAccountBanksByIndividualId(int individualId)
        {
            AccountBankDAO accountBankDAO = new AccountBankDAO();
            return DTOAssembler.CreateAccounBankDTOs(accountBankDAO.GetAccountBanksByIndividualId(individualId));
        }

        public ExchangeRateDTO GetExchangeRateByRateDateCurrencyId(DateTime rateDate, int currencyId)
        {
            return DTOAssembler.CreateExchangeRate(DelegateService.commonServiceCore.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId));
        }

        public DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime date)
        {
            return DelegateService.commonServiceCore.GetModuleDateIssue((int)moduleType, date);
        }

        //public List<SelectDTO> GetPaymentConcepts()
        //{
        //    return DTOAssembler.CreateSelectPaymentConcepts(DelegateService.commonServiceCore.GetPaymentConcept());
        //}

        public List<ChargeRequestDTO> CreateChargeRequests(List<ChargeRequestDTO> chargeRequestsDTO)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            GLWKDTO.AccountingPaymentRequestDTO accountingPaymentRequestDTO = new GLWKDTO.AccountingPaymentRequestDTO();

            foreach (ChargeRequestDTO chargeRequestDTO in chargeRequestsDTO)
            {
                chargeRequestDTO.Number = paymentRequestDAO.GetPaymentRequestNumerByBranchId(chargeRequestDTO.BranchId);
                chargeRequestDTO.TotalAmount = chargeRequestDTO.Voucher.Concepts.Sum(x => x.Value);

                if (Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_ENABLE_GENERAL_LEDGER))))
                {
                    chargeRequestDTO.TechnicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(new TechnicalTransactionParameterDTO { BranchId = chargeRequestDTO.BranchId }).Id;
                }

                // Validación y cargue de la contabilidad
                accountingPaymentRequestDTO = DelegateService.claimsWorkerIntegrationService.LoadAccountingPaymentRequest(DTOAssembler.CreateAccountingChargeRequest(chargeRequestDTO, chargeRequestDTO.TechnicalTransaction));

                ChargeRequest chargeRequest = ModelAssembler.CreateChargeRequest(chargeRequestDTO);
                
                if (chargeRequestDTO.TemporalId == 0)
                {
                    chargeRequestDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(chargeRequest);

                    if (chargeRequestDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        if (!chargeRequestDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                        {
                            chargeRequest.AccountingCharge = ModelAssembler.CreateAccountingPaymentRequest(accountingPaymentRequestDTO);

                            PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                            chargeRequestDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByChargeRequest(chargeRequest)).Id;
                        }

                        continue;
                    }
                }
                else
                {
                    List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(chargeRequestDTO.TemporalId.ToString());

                    if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                    {
                        throw new BusinessException(Resources.Resources.PaymentAuthorizationRequestPending);
                    }

                    if (chargeRequestDTO.AuthorizationPolicies.Any())
                    {
                        chargeRequestDTO.AuthorizationPolicies.Clear();
                    }
                }

                chargeRequestDTO.Id = paymentRequestDAO.CreateChargeRequest(chargeRequest).Id;

                if (Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_ENABLE_GENERAL_LEDGER))))
                {                    
                    SaveAccountingChargeRequest(accountingPaymentRequestDTO);
                }

                #region Payment Request Claim Control
                PaymentRequestControl paymentRequestControl = new PaymentRequestControl();
                paymentRequestControl.PaymentRequestId = chargeRequestDTO.Id;
                paymentRequestControl.Action = "I";
                paymentRequestDAO.CreatePaymentRequestControl(paymentRequestControl);
                #endregion

                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsurancePaymentRequest(chargeRequestDTO.Id, chargeRequestDTO.UserId);
                }
                #endregion

            }

            return chargeRequestsDTO;
        }

        public PaymentRequestDTO GetPaymentRequestByPrefixIdBranchIdNumber(int prefixId, int branchId, int number)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            PaymentRequestDTO paymentRequestDTO = DTOAssembler.CreatePaymentRequest(paymentRequestDAO.GetPaymentRequestByPrefixIdBranchIdNumber(prefixId, branchId, number));

            if (Convert.ToInt32(paymentRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_PROVIDER)))
            {
                ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
                ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestDTO.BeneficiaryDocumentNumber = claimSupplier.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = claimSupplier.FullName;
            }
            else if (Convert.ToInt32(paymentRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_INSURED)))
            {
                Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.InsuredDTO insuredDTO = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestDTO.BeneficiaryDocumentNumber = insuredDTO.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = insuredDTO.FullName;
            }
            else if (Convert.ToInt32(paymentRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_THIRD)))
            {
                UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();

                paymentRequestDTO.BeneficiaryDocumentNumber = thirdPerson.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = thirdPerson.Fullname;
            }
            else if (Convert.ToInt32(paymentRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_HOLDER)))
            {
                UNDMOD.HolderDTO holderDTO = DelegateService.underwritingIntegrationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestDTO.BeneficiaryDocumentNumber = holderDTO.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = holderDTO.FullName;
            }

            return paymentRequestDTO;
        }

        public ChargeRequestDTO GetChargeRequestByPrefixIdBranchIdNumber(int prefixId, int branchId, int number)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            ChargeRequestDTO chargeRequestDTO = DTOAssembler.CreateChargeRequest(paymentRequestDAO.GetChargeRequestByPrefixIdBranchIdNumber(prefixId, branchId, number));

            if (Convert.ToInt32(chargeRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_PROVIDER)))
            {
                ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
                ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                chargeRequestDTO.BeneficiaryDocumentNumber = claimSupplier.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = claimSupplier.FullName;
            }
            else if (Convert.ToInt32(chargeRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_INSURED)))
            {
                UNDMOD.InsuredDTO insuredDTO = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                chargeRequestDTO.BeneficiaryDocumentNumber = insuredDTO.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = insuredDTO.FullName;
            }
            else if (Convert.ToInt32(chargeRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_THIRD)))
            {
                UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();

                chargeRequestDTO.BeneficiaryDocumentNumber = thirdPerson.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = thirdPerson.Fullname;
            }
            else if (Convert.ToInt32(chargeRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_HOLDER)))
            {
                UNDMOD.HolderDTO holderDTO = DelegateService.underwritingIntegrationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                chargeRequestDTO.BeneficiaryDocumentNumber = holderDTO.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = holderDTO.FullName;
            }

            return chargeRequestDTO;
        }

        public List<PaymentRequestDTO> SearchPaymentRequests(PaymentRequestDTO paymentRequestDTO)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreatePaymentRequests(paymentRequestDAO.SearchPaymentRequests(ModelAssembler.CreatePaymentRequest(paymentRequestDTO)));
        }

        public List<ChargeRequestDTO> SearchChargeRequests(ChargeRequestDTO chargeRequestDTO)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreateChargeRequests(paymentRequestDAO.SearchChargeRequests(ModelAssembler.CreateChargeRequest(chargeRequestDTO)));
        }

        public List<PaymentRequestDTO> GetPaymentRequestClaimsByPaymentRequestId(int paymentRequestId)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreatePaymentRequests(paymentRequestDAO.GetPaymentRequestClaimsByPaymentRequestId(paymentRequestId));
        }

        public PaymentRequestDTO GetPaymentRequestByPaymentRequestId(int paymentRequestId)
        {
            List<CurrencyDTO> currencies = GetCurrencies();
            List<VoucherTypeDTO> voucherTypes = GetVoucherTypes();

            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            PaymentRequestDTO paymentRequestDTO = DTOAssembler.CreatePaymentRequest(paymentRequestDAO.GetPaymentRequestByPaymentRequestId(paymentRequestId));

            if (paymentRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_PROVIDER)))
            {
                ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
                ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestDTO.BeneficiaryDocumentNumber = claimSupplier.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = claimSupplier.FullName;
            }
            else if (paymentRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_INSURED)))
            {
                UNDMOD.InsuredDTO insuredDTO = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestDTO.BeneficiaryDocumentNumber = insuredDTO.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = insuredDTO.FullName;
            }
            else if (paymentRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_THIRD)))
            {                
                UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();

                paymentRequestDTO.BeneficiaryDocumentNumber = thirdPerson.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = thirdPerson.Fullname;
            }
            else if (Convert.ToInt32(paymentRequestDTO?.PersonTypeId) == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_HOLDER)))
            {
                UNDMOD.HolderDTO holderDTO = DelegateService.underwritingIntegrationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(paymentRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestDTO.BeneficiaryDocumentNumber = holderDTO.DocumentNumber;
                paymentRequestDTO.BeneficiaryFullName = holderDTO.FullName;
            }

            return paymentRequestDTO;
        }

        public ChargeRequestDTO GetChargeRequestByChargeRequestId(int chargeRequestId)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            ChargeRequestDTO chargeRequestDTO = DTOAssembler.CreateChargeRequest(paymentRequestDAO.GetChargeRequestByChargeRequestId(chargeRequestId));

            if (chargeRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_PROVIDER)))
            {
                ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
                ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                chargeRequestDTO.BeneficiaryDocumentNumber = claimSupplier.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = claimSupplier.FullName;
            }
            else if (chargeRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_INSURED)))
            {
                UNDMOD.InsuredDTO insuredDTO = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                chargeRequestDTO.BeneficiaryDocumentNumber = insuredDTO.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = insuredDTO.FullName;
            }
            else if (chargeRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_THIRD)))
            {
                UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();

                chargeRequestDTO.BeneficiaryDocumentNumber = thirdPerson.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = thirdPerson.Fullname;
            }
            else if (chargeRequestDTO.PersonTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_HOLDER)))
            {
                UNDMOD.HolderDTO holderDTO = DelegateService.underwritingIntegrationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(chargeRequestDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                chargeRequestDTO.BeneficiaryDocumentNumber = holderDTO.DocumentNumber;
                chargeRequestDTO.BeneficiaryFullName = holderDTO.FullName;
            }

            List<SelectDTO> paymentConcepts = GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(chargeRequestDTO.BranchId, chargeRequestDTO.MovementTypeId, chargeRequestDTO.PersonTypeId, chargeRequestDTO.IndividualId);

            foreach (VoucherConceptDTO voucherConcept in chargeRequestDTO.Voucher.Concepts)
            {
                voucherConcept.PaymentConcept = paymentConcepts.FirstOrDefault(x => x.Id == voucherConcept.PaymentConceptId)?.Description;
            }

            return chargeRequestDTO;
        }

        public PaymentRequestReportDTO GetReportPaymentRequestByPaymentRequestId(int paymentRequestId)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            ClaimEndorsementDAO claimEndorsementDAO = new ClaimEndorsementDAO();

            PaymentRequest paymentRequest = paymentRequestDAO.GetReportPaymentRequestByPaymentRequestId(paymentRequestId);

            PaymentRequestReportDTO paymentRequestReportDTO = DTOAssembler.CreatePaymentRequestReport(paymentRequest);

            if (paymentRequest.PersonType.Id == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_PROVIDER)))
            {
                ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
                ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(paymentRequest.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestReportDTO.PaymentBeneficiaryName = claimSupplier.FullName;
                paymentRequestReportDTO.PaymentBeneficiaryDocumentNumber = DelegateService.uniquePersonServiceCore.GetDocumentTypes(claimSupplier.DocumentTypeId).FirstOrDefault(x => x.Id == claimSupplier.DocumentTypeId)?.SmallDescription + " - " + claimSupplier.DocumentNumber;
            }
            else if (paymentRequest.PersonType.Id == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_INSURED)))
            {
                UNDMOD.InsuredDTO insuredDTO = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(paymentRequest.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                paymentRequestReportDTO.PaymentBeneficiaryName = insuredDTO.FullName;
                paymentRequestReportDTO.PaymentBeneficiaryDocumentNumber = DelegateService.uniquePersonServiceCore.GetDocumentTypes(insuredDTO.DocumentTypeId).FirstOrDefault(x => x.Id == insuredDTO.DocumentTypeId)?.SmallDescription + " - " + insuredDTO.DocumentNumber;
            }
            else if (paymentRequest.PersonType.Id == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PERSON_TYPE_THIRD)))
            {
                UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(paymentRequest.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();

                paymentRequestReportDTO.PaymentBeneficiaryName = thirdPerson.Fullname;
                paymentRequestReportDTO.PaymentBeneficiaryDocumentNumber = DelegateService.uniquePersonServiceCore.GetDocumentTypes(Convert.ToInt32(thirdPerson.DocumentTypeId)).FirstOrDefault(x => x.Id == Convert.ToInt32(thirdPerson.DocumentTypeId))?.SmallDescription + " - " + thirdPerson.DocumentNumber;
            }


            PolicyDTO policyDTO = DTOAssembler.CreatePolicy(DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(paymentRequest.Claims.FirstOrDefault().Endorsement.Id));

            paymentRequestReportDTO.PolicyAgent = policyDTO.Agent;
            paymentRequestReportDTO.PolicyHolder = policyDTO.HolderName;

            if (paymentRequest.Claims.FirstOrDefault().Modifications.First().Coverages.First().IsInsured)
            {
                paymentRequestReportDTO.PolicyInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(paymentRequest.Claims.FirstOrDefault().Modifications.First().Coverages.First().IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault().FullName;
            }
            else
            {
                paymentRequestReportDTO.PolicyInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(claimEndorsementDAO.GetInsuredIdByRiskId(paymentRequest.Claims.FirstOrDefault().Modifications.First().Coverages.First().RiskId).ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault().FullName;
            }
            
            foreach (ClaimReportDTO claim in paymentRequestReportDTO.Claims)
            {
                claim.Reinsurance = DelegateService.claimsReinsuranceWorkerIntegrationServices.ValidateClaimPaymentRequestReinsurance(paymentRequest.Id) ? Resources.Resources.PaymentRequestReinsurance : Resources.Resources.PaymentRequestReinsurance; //TODO: Cambiar cuando la consulta del reaseguro esté en 3G
            }

            paymentRequestReportDTO.TRM = GetTRMExchangeRateByRateDateCurrencyId(paymentRequest.RegistrationDate, paymentRequest.Currency.Id).SellAmount.ToExchangeRate();                        

            GLWKDTO.JournalEntryDTO journalEntryDTO = DelegateService.claimsWorkerIntegrationService.GetJournalEntryDetailByTechnicalTransaction(paymentRequest.TechnicalTransaction);
            paymentRequestReportDTO.Accountings = DTOAssembler.CreateAccountingsReport(journalEntryDTO.JournalEntryItems);

            return paymentRequestReportDTO;
        }

        public List<PaymentSourceDTO> GetPaymentSourcesByChargeRequest(bool isChargeRequest)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            List<ConceptSource> conceptSources = paymentRequestDAO.GetPaymentSourcesByChargeRequest(isChargeRequest);
            List<PaymentSourceDTO> paymentSources = DTOAssembler.CreatePaymentSources(DelegateService.claimsWorkerIntegrationService.GetConceptSources());

            paymentSources.RemoveAll(x => !conceptSources.Exists(y => x.Id == y.Id));

            return paymentSources;
        }

        public void CreatePaymentRequestByTemporalId(int temporalId)
        {
            try
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PaymentRequest, temporalId.ToString(), null, "Procesando");

                PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
                VoucherDAO voucherDAO = new VoucherDAO();
                PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                PendingOperationDTO pendingOperation = DTOAssembler.CreatePendingOperation(pendingOperationsDAO.GetPendingOperationByPendingOperationId(temporalId));

                if (pendingOperation != null)
                {
                    PaymentRequest paymentRequest = JsonConvert.DeserializeObject<PaymentRequest>(pendingOperation.Operation);
                    paymentRequest.TemporalId = temporalId;
                    List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = new List<CoInsuranceAssignedDTO>();

                    paymentRequest.Id = paymentRequestDAO.CreatePaymentRequest(paymentRequest).Id;

                    foreach (Claim claim in paymentRequest.Claims)
                    {
                        claim.CoveredRiskType = CoveredRiskType.Vehicle;

                        foreach (Models.Claim.CoInsuranceAssigned coInsuranceAssigned in claim.CoInsuranceAssigned)
                        {
                            PaymentRequestCoInsurance paymentRequestCoInsurance = ModelAssembler.CreatePaymentRequestCoInsurance(paymentRequest, coInsuranceAssigned, Convert.ToInt32(claim.Modifications.First().Coverages.First().CoverageId));

                            if (paymentRequest.Claims.IndexOf(claim) == 0 || paymentRequestDAO.GetPaymentRequestCoInsuranceByPaymentRequestIdCompanyId(paymentRequest.Id, paymentRequestCoInsurance.CompanyId) == null)
                            {
                                paymentRequestDAO.SavePaymentRequestCoInsurance(paymentRequestCoInsurance, paymentRequest.Id, paymentRequestCoInsurance.CompanyId);
                            }
                        }

                        voucherDAO.CreateVouchers(claim, paymentRequest);

                        if (paymentRequest.IsTotal)
                        {
                            DelegateService.claimApplicationService.SetClaimReserveByClaimIdSubClaimEstimationTypeIdPaymentUserId(claim.Id, Convert.ToInt32(claim.Modifications.First().Coverages.First().SubClaim), claim.Modifications.Last().Coverages.First().Estimations.First().Type.Id, paymentRequest.UserId);
                        }
                    }

                    if (Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_ENABLE_GENERAL_LEDGER))))
                    {
                        SaveAccountingPaymentRequest(DTOAssembler.CreateAccountingPaymentRequest(paymentRequest.AccountingPayment));
                    }
                    else
                    {
                        paymentRequest.SaveDailyEntryMessage = Resources.Resources.IntegrationServiceDisabledLabel;
                    }

                    DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PaymentRequest, temporalId.ToString(), null, paymentRequest.Id.ToString());

                    CreatePaymentRequestNotificationByAuthorizationPolicies(paymentRequest);
                    pendingOperationsDAO.DeletePendingOperationByPendingOperationId(temporalId);

                    #region PaymentRequestControl
                    paymentRequestDAO.CreatePaymentRequestControl(paymentRequest);
                    #endregion

                    #region Reinsurance
                    if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                    {
                        ReinsurancePaymentRequest(paymentRequest.Id, paymentRequest.UserId);
                    }
                    #endregion

                }
                else
                {
                    throw new Exception(Resources.Resources.TemporalNotFound);
                }
            }
            catch (Exception ex)
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PaymentRequest, temporalId.ToString(), null, "Error al Procesar");
                throw ex;
            }
        }
        
        public void CreateChargeRequestByTemporalId(int temporalId)
        {
            try
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.ChargeRequest, temporalId.ToString(), null, "Procesando");


                PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
                VoucherDAO voucherDAO = new VoucherDAO();
                PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                PendingOperationDTO pendingOperation = DTOAssembler.CreatePendingOperation(pendingOperationsDAO.GetPendingOperationByPendingOperationId(temporalId));

                if (pendingOperation != null)
                {
                    ChargeRequest chargeRequest = JsonConvert.DeserializeObject<ChargeRequest>(pendingOperation.Operation);

                    chargeRequest.TemporalId = temporalId;

                    chargeRequest.Id = paymentRequestDAO.CreateChargeRequest(chargeRequest).Id;

                    if (Convert.ToBoolean(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_IS_ENABLE_GENERAL_LEDGER))))
                    {
                        SaveAccountingChargeRequest(DTOAssembler.CreateAccountingPaymentRequest(chargeRequest.AccountingCharge));
                    }

                    DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.ChargeRequest, temporalId.ToString(), null, chargeRequest.Id.ToString());

                    CreateChargeRequestNotificationByAuthorizationPolicies(chargeRequest);
                    pendingOperationsDAO.DeletePendingOperationByPendingOperationId(temporalId);

                    #region Payment Request Claim Control
                    PaymentRequestControl paymentRequestControl = new PaymentRequestControl();
                    paymentRequestControl.PaymentRequestId = chargeRequest.Id;
                    paymentRequestControl.Action = "I";
                    paymentRequestDAO.CreatePaymentRequestControl(paymentRequestControl);
                    #endregion

                    #region Reinsurance
                    if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                    {
                        ReinsurancePaymentRequest(chargeRequest.Id, chargeRequest.UserId);
                    }
                    #endregion

                }
                else
                {
                    throw new Exception(Resources.Resources.TemporalNotFound);
                }
            }
            catch (Exception ex)
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.ChargeRequest, temporalId.ToString(), null, "Error al Procesar");
                throw ex;
            }
        }

        public int GetDefaultPaymentCurrency()
        {
            return Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.PAYM_PAYMENT_CURRENCY));
        }

        #region AccountingPaymentRequest

        private async void SaveAccountingPaymentRequest(GLWKDTO.AccountingPaymentRequestDTO accountingPaymentRequestDTO)
        {
            try
            {
                await UTILTASK.Task.Run(() =>
                {
                    DelegateService.claimsWorkerIntegrationService.SaveAccountingPaymentRequest(accountingPaymentRequestDTO);
                });
            }
            catch (Exception)
            {
                
            }
        }

        private async void SaveAccountingChargeRequest(GLWKDTO.AccountingPaymentRequestDTO accountingPaymentRequestDTO)
        {
            try
            {
                await UTILTASK.Task.Run(() =>
                {
                    DelegateService.claimsWorkerIntegrationService.SaveAccountingPaymentRequest(accountingPaymentRequestDTO);
                });
            }
            catch (Exception)
            {

            }
        }

        private async void AccountingPaymentRequest(PaymentRequestDTO paymentRequestDTO, List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs)
        {
            try
            {
                await UTILTASK.Task.Run(() =>
                {
                    DelegateService.claimsWorkerIntegrationService.AccountingPaymentRequest(DTOAssembler.CreateAccountingPaymentRequest(paymentRequestDTO, coInsuranceAssignedDTOs, paymentRequestDTO.TechnicalTransaction));
                });
            }
            catch (Exception)
            {
                
            }            
        }

        private async void AccountingChargeRequest(ChargeRequestDTO chargeRequestDTO)
        {
            try
            {
                await UTILTASK.Task.Run(() =>
                {
                    DelegateService.claimsWorkerIntegrationService.AccountingPaymentRequest(DTOAssembler.CreateAccountingChargeRequest(chargeRequestDTO, chargeRequestDTO.TechnicalTransaction));
                });
            }
            catch (Exception)
            {

            }            
        }

        #endregion

        #region ValidateAuthorizationPolicies
        private List<PoliciesAut> ValidateAuthorizationPolicies(PAYMENMOD.PaymentRequest paymentRequest)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, paymentRequest.UserId);

            foreach (Claim claim in paymentRequest.Claims)
            {
                EntityAssembler.CreateFacadePaymentRequest(facade, paymentRequest, claim);

                policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_PAYMENT_REQUEST)), 1, facade, FacadeType.RULE_FACADE_PAYMENT_REQUEST));

                foreach (Voucher voucher in claim.Vouchers)
                {
                    EntityAssembler.CreateFacadeVoucher(facade, voucher, voucher.Concepts);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_PAYMENT_REQUEST)), 1, facade, FacadeType.RULE_FACADE_VOUCHER));
                    foreach (VoucherConcept voucherConcept in voucher.Concepts)
                    {
                        EntityAssembler.CreateFacadeVoucherConcept(facade, voucherConcept);
                        policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_PAYMENT_REQUEST)), 1, facade, FacadeType.RULE_FACADE_VOUCHER_CONCEPT));
                    }
                }
            }

            return policiesAuts;
        }
        
        private void CreatePaymentRequestNotificationByAuthorizationPolicies(PAYMENMOD.PaymentRequest paymentRequest)
        {
            var result = string.Empty;
            var notification = string.Empty;

            result += "La solicitud de pago se ha generado con exito." +
                      "</br>Número de solicitud: " + paymentRequest.Number + ".</br>" +
                      (!string.IsNullOrEmpty(paymentRequest.SaveDailyEntryMessage) ? "</br>" + paymentRequest.SaveDailyEntryMessage + ".</br>" : "");

            notification = "Se genero la solicitud de pago: " + paymentRequest.Number + ".";

            NotificationUser notificationUser = new NotificationUser
            {
                UserId = paymentRequest.UserId,
                CreateDate = DateTime.Now,
                NotificationType = new NotificationType { Type = NotificationTypes.PaymentRequest },
                Message = notification,
                Parameters = new Dictionary<string, object>
                    {
                        { "SearchType", (int)NotificationTypes.PaymentRequest },
                        { "BranchId", paymentRequest.Branch.Id },
                        { "Number", paymentRequest.Number },
                        { "ClaimNumber", paymentRequest.Claims.FirstOrDefault().Number }
                    }
            };

            UserPerson person = DelegateService.uniqueUserService.GetPersonByUserId(paymentRequest.UserId);
            if (person != null && person.Emails.Any())
            {
                string strAddress = person.Emails[0].Description;

                EmailCriteria email = new EmailCriteria
                {
                    Addressed = new List<string> { strAddress },
                    Message = "<h3>Todas las politicas fueron autorizadas</h3>" + paymentRequest.Number,
                    Subject = "Politicas autorizadas - " + paymentRequest.Id
                };

                DelegateService.authorizationPoliciesService.SendEmail(email);
            }

            DelegateService.uniqueUserService.CreateNotification(notificationUser);
        }

        private void CreateChargeRequestNotificationByAuthorizationPolicies(PAYMENMOD.ChargeRequest chargeRequest)
        {
            var result = string.Empty;
            var notification = string.Empty;

            result += "La solicitud de cobro se ha generado con exito." + "</br>Número de solicitud: " + chargeRequest.Number + ".";

            notification = "Se genero la solicitud de cobro: " + chargeRequest.Number + ".";

            NotificationUser notificationUser = new NotificationUser
            {
                UserId = chargeRequest.UserId,
                CreateDate = DateTime.Now,
                NotificationType = new NotificationType { Type = NotificationTypes.ChargeRequest },
                Message = notification,
                Parameters = new Dictionary<string, object>
                    {
                        { "SearchType", (int)NotificationTypes.ChargeRequest },
                        { "BranchId", chargeRequest.Branch.Id },
                        { "Number", chargeRequest.Number },
                        { "PaymentSourceId", chargeRequest.MovementType.Source.Id }
                    }
            };

            UserPerson person = DelegateService.uniqueUserService.GetPersonByUserId(chargeRequest.UserId);
            if (person != null && person.Emails.Any())
            {
                string strAddress = person.Emails[0].Description;

                EmailCriteria email = new EmailCriteria
                {
                    Addressed = new List<string> { strAddress },
                    Message = "<h3>Todas las politicas fueron autorizadas</h3>" + chargeRequest.Number,
                    Subject = "Politicas autorizadas - " + chargeRequest.Id
                };

                DelegateService.authorizationPoliciesService.SendEmail(email);
            }

            DelegateService.uniqueUserService.CreateNotification(notificationUser);

        }

        private List<PoliciesAut> ValidateAuthorizationPolicies(PAYMENMOD.ChargeRequest chargeRequest)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, chargeRequest.UserId);

            EntityAssembler.CreateFacadeChargeRequest(facade, chargeRequest, chargeRequest.Claim);
            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CHARGE_REQUEST)), 1, facade, FacadeType.RULE_FACADE_CHARGE_REQUEST));

            EntityAssembler.CreateFacadeVoucher(facade, chargeRequest.Voucher, chargeRequest.Voucher.Concepts);
            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_PAYMENT_REQUEST)), 1, facade, FacadeType.RULE_FACADE_VOUCHER));

            foreach (VoucherConcept voucherConcept in chargeRequest.Voucher.Concepts)
            {
                EntityAssembler.CreateFacadeVoucherConcept(facade, voucherConcept);
                policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_PAYMENT_REQUEST)), 1, facade, FacadeType.RULE_FACADE_VOUCHER_CONCEPT));
            }

            return policiesAuts;
        }

        #endregion

        public BranchDTO GetBranchById(int branchId)
        {
            return DTOAssembler.CreateBranch(DelegateService.commonServiceCore.GetBranchById(branchId));
        }

        public PrefixDTO GetPrefixByPrefixId(int prefixId)
        {
            return DTOAssembler.CreatePrefix(DelegateService.commonServiceCore.GetPrefixById(prefixId));
        }

        public List<CoInsuranceAssignedDTO> GetCoInsuranceByPolicyIdByEndorsementId(int endorsementId, int policyId)
        {
            return DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(endorsementId, policyId));
        }

        public List<PersonTypeDTO> GetClaimSearchPersonType(int prefixId, int searchType)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            return DTOAssembler.CreatePersonTypes(paymentRequestDAO.GetClaimSearchPersonType(prefixId, searchType));
        }

        public int GetTaxCodeOfRetetionToIndustryAndCommerce()
        {
            try
            {
                return Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_RETENTION_ICA));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private async void ReinsuranceCancellationPayment(int paymentRequestId, int userId)
        {                        
            await UTILTASK.Task.Run(() =>
            {
                DelegateService.claimsReinsuranceWorkerIntegrationServices.ReinsuranceCancellationPayment(paymentRequestId, userId);
            });
        }

        private async void ReinsurancePaymentRequest(int paymentRequestId, int userId)
        {
            await UTILTASK.Task.Run(() =>
            {
                try
                {
                    DelegateService.claimsReinsuranceWorkerIntegrationServices.ReinsurancePayment(paymentRequestId, userId);
                }
                catch (Exception)
                {
                    
                }                
            });
        }

        private async void ReinsuranceClaim(int claimId, int claimModifyId, int userId)
        {
            await UTILTASK.Task.Run(() =>
            {
                DelegateService.claimsReinsuranceWorkerIntegrationServices.ReinsuranceClaim(claimId, claimModifyId, userId);
            });
        }

        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonServiceCore.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            return parameters;
        }

        private ExchangeRate GetTRMExchangeRateByRateDateCurrencyId(DateTime RateDate, int currencyId)
        {
            if (!parameters.Any())
            {
                parameters = this.GetParameters();
            }

            if (currencyId != (int)parameters.First(x => x.Description == "Currency").Value)
            {
                return DelegateService.commonServiceCore.GetExchangeRateByRateDateCurrencyId(DateTime.Now, currencyId);
            }
            else
            {
                return new ExchangeRate
                {
                    Currency = new Currency
                    {
                        Id = currencyId
                    },
                    RateDate = DateTime.Now,
                    SellAmount = 1
                };
            }
        }
    }
}