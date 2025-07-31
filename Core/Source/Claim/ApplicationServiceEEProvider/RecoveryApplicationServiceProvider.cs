using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.ClaimServices.DTOs.Recovery;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.Business.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Recovery;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.Helper;
using UNDMO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.ClaimServices.EEProvider
{
    public class RecoveryApplicationServiceProvider : IRecoveryApplicationService
    {

        public RecoveryDTO CreateRecovery(RecoveryDTO recoveryDTO)
        {
            try
            {
                Recovery recovery = ModelAssembler.CreateRecovery(recoveryDTO);
                               
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                paymentPlanDAO.CreatePaymentPlan(recovery.PaymentPlan);

                RecoveryDAO recoveryDAO = new RecoveryDAO();
                return DTOAssembler.CreateRecovery(recoveryDAO.CreateRecovery(recovery));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public RecoveryDTO UpdateRecovery(RecoveryDTO recoveryDTO)
        {
            try
            {
                RecoveryDAO recoveryDAO = new RecoveryDAO();                
                Recovery recovery = ModelAssembler.CreateRecovery(recoveryDTO);                                          

                return DTOAssembler.CreateRecovery(recoveryDAO.UpdateRecovery(recovery));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<RecoveryTypeDTO> GetRecoveryTypesById(int recoveryTypeId)
        {
            try
            {
                RecoveryDAO recoveryDAO = new RecoveryDAO();
                return DTOAssembler.CreateRecoveryTypes(recoveryDAO.GetRecoveryTypesById(recoveryTypeId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<RecoveryDTO> GetRecoveriesByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            try
            {                
                RecoveryDAO recoveryDAO = new RecoveryDAO();
                List<Recovery> recoveries = recoveryDAO.GetRecoveriesByClaimIdSubClaimId(claimId, subClaimId);

                foreach (Recovery recovery in recoveries)
                {
                    if (recovery.Debtor != null)
                    {
                        DebtorDAO debtorDAO = new DebtorDAO();
                        recovery.Debtor = debtorDAO.GetDebtorByDescriptionInsuredSearchTypeCustomerType(recovery.Debtor.Id.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                    }

                    RecuperatorDAO recuperatorDAO = new RecuperatorDAO();
                    recovery.Recuperator = recuperatorDAO.GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(recovery.Recuperator.Id.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                    recovery.RecoveryType = recoveryDAO.GetRecoveryTypesById(recovery.RecoveryType.Id).First();
                }

                List<RecoveryDTO> recoveriesDTO = DTOAssembler.CreateRecoveries(recoveries);

                foreach (RecoveryDTO recoveryDTO in recoveriesDTO)
                {
                    if (recoveryDTO.CurrencyId != null)
                    {
                        recoveryDTO.CurrencyDescription = GetCurrencies().First(x => x.Id == recoveryDTO.CurrencyId).Description;
                    }
                }

                return recoveriesDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<RecoveryTypeDTO> GetRecoveryTypes()
        {
            try
            {
                RecoveryDAO recoveryDAO = new RecoveryDAO();
                return DTOAssembler.CreateRecoveryTypes(recoveryDAO.GetRecoveryTypes());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<RecoveryDTO> GetRecoveriesByClaimId(int claimId)
        {
            try
            {
                RecoveryDAO recoveryDAO = new RecoveryDAO();
                return DTOAssembler.CreateRecoveries(recoveryDAO.GetRecoveriesByClaimId(claimId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<RecoveryDTO> GetRecoveriesByClaim(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                RecoveryDAO recoveryDAO = new RecoveryDAO();
                Claim claim = claimDAO.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);
                if (claim != null)
                {
                    UNDMO.PolicyDTO policyDTO = DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(claim.Endorsement.Id);

                    List<RecoveryDTO> recoveries = DTOAssembler.CreateRecoveries(recoveryDAO.GetRecoveriesByClaimId(claim.Id));

                    recoveries.ForEach(x => { x.PolicyHolderId = policyDTO.HolderId; });

                    return recoveries;
                }
                else
                {
                    throw new BusinessException(Resources.Resources.ClaimNotFound);
                }

            }   
            catch(BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);                
            }
        }

        public RecoveryDTO GetRecoveryByRecoveryId(int recoveryId)
        {
            RecoveryDAO recoveryDAO = new RecoveryDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            RecuperatorDAO recuperatorDAO = new RecuperatorDAO();
            Recovery recovery = recoveryDAO.GetRecoveryByRecoveryId(recoveryId);
            if (recovery != null)
            {
                if (recovery.Recuperator.Id > 0)
                {
                    recovery.Recuperator = recuperatorDAO.GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(recovery.Recuperator.Id.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                }

                if (recovery.Debtor != null)
                {
                    DebtorDAO debtorDAO = new DebtorDAO();
                    recovery.Debtor = debtorDAO.GetDebtorByDescriptionInsuredSearchTypeCustomerType(recovery.Debtor.Id.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();

                }                
                recovery.RecoveryAmount = paymentRequestDAO.GetRecoveryAmountByRecoveryIdOrSalvageId(recovery.Id, true);
            }
            return DTOAssembler.CreateRecovery(recovery);
        }

        public int GetRecoveryNumberByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            RecoveryDAO recoveryDAO = new RecoveryDAO();
            return recoveryDAO.GetRecoveryNumberByClaimIdSubClaimId(claimId, subClaimId);
        }

        public List<CurrencyDTO> GetCurrencies()
        {
            return DTOAssembler.CreateCurrencies(DelegateService.commonServiceCore.GetCurrencies());
        }
    }
}