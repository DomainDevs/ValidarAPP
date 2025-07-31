using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Salvage;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.Business.Salvage;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.ClaimServices.EEProvider.Enums;
using UNDMO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;

namespace Sistran.Core.Application.ClaimServices.EEProvider
{
    public class SalvageApplicationServiceProvider : ISalvageApplicationService
    {
        public SalvageDTO CreateSalvage(SalvageDTO salvage)
        {
            try
            {
                SalvageDAO salvageDAO = new SalvageDAO();
                return DTOAssembler.CreateSalvage(salvageDAO.CreateSalvage(ModelAssembler.CreateSalvage(salvage)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorCreateRecovery), ex);
            }
        }

        public SalvageDTO UpdateSalvage(SalvageDTO salvage)
        {
            try
            {
                SalvageDAO salvageDAO = new SalvageDAO();
                return DTOAssembler.CreateSalvage(salvageDAO.UpdateSalvage(ModelAssembler.CreateSalvage(salvage)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteSalvage(int salvageId)
        {
            try
            {
                SalvageDAO salvageDAO = new SalvageDAO();
                salvageDAO.DeleteSalvage(salvageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<SalvageDTO> GetSalvagesByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            try
            {
                SalvageDAO salvageDAO = new SalvageDAO();
                return DTOAssembler.CreateSalvages(salvageDAO.GetSalvagesByClaimIdSubClaimId(claimId, subClaimId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<SalvageDTO> GetSalvagesByClaim(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                SalvageDAO salvageDAO = new SalvageDAO();
                Claim claim = claimDAO.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);

                if (claim != null)
                {
                    UNDMO.PolicyDTO policyDTO = DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(claim.Endorsement.Id);

                    List<SalvageDTO> salvages = DTOAssembler.CreateSalvages(salvageDAO.GetSalvagesByClaimId(claim.Id));

                    salvages.ForEach(x => { x.PolicyHolderId = policyDTO.HolderId; });

                    return salvages;
                }
                else
                {
                    throw new BusinessException(Resources.Resources.ClaimNotFound);
                }               

            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int GetSalvageNumberByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            SalvageDAO salvageDAO = new SalvageDAO();
            return salvageDAO.GetSalvageNumberByClaimIdSubClaimId(claimId, subClaimId);
        }

        public List<SalvageDTO> GetSalvagesByClaimId(int claimId)
        {
            try
            {
                SalvageDAO salvageDAO = new SalvageDAO();
                return DTOAssembler.CreateSalvages(salvageDAO.GetSalvagesByClaimId(claimId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public SalvageDTO GetSalvageBySalvageId(int salvageId)
        {
            try
            {
                SalvageDAO salvageDAO = new SalvageDAO();
                return DTOAssembler.CreateSalvage(salvageDAO.GetSalvageBySalvageId(salvageId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }

        public SaleDTO CreateSale(SaleDTO saleDTO, int salvageId)
        {
            try
            {                
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                Sale sale = ModelAssembler.CreateSale(saleDTO);
                sale.PaymentPlan = paymentPlanDAO.CreatePaymentPlan(sale.PaymentPlan);
                                
                SaleDAO saleDAO = new SaleDAO();
                return DTOAssembler.CreateSale(saleDAO.CreateSale(sale, salvageId));

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        public SaleDTO UpdateSale(SaleDTO saleDTO, int salvageId)
        {
            try
            {
                SaleDAO saleDAO = new SaleDAO();
                
                Sale sale = new Sale();
                sale = ModelAssembler.CreateSale(saleDTO);                

                return DTOAssembler.CreateSale(saleDAO.UpdateSale(sale, salvageId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public SaleDTO GetSaleBySaleId(int saleId)
        {
            SaleDAO saleDAO = new SaleDAO();
            BuyerDAO buyerDAO = new BuyerDAO();
            Sale sale = saleDAO.GetSaleBySaleId(saleId);

            if (sale != null)
            {                
                sale.Buyer = buyerDAO.GetBuyersByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(sale.Buyer.Id), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
            }

            return DTOAssembler.CreateSale(sale);
        }

        public List<SaleDTO> GetSalesBySalvageId(int salvageId)
        {
            SaleBusiness saleBusiness = new SaleBusiness();

            SaleDAO saleDAO = new SaleDAO();
            List<Sale> sales = saleDAO.GetSalesBySalvageId(salvageId);
            foreach (Sale sale in sales)
            {
                BuyerDAO buyerDAO = new BuyerDAO();
                if (sale.Buyer.Id != 0)
                {
                    sale.Buyer = buyerDAO.GetBuyersByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(sale.Buyer.Id), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();                                        
                }
            }

            return DTOAssembler.CreateSales(sales);
        }

        public List<SelectDTO> GetPaymentClasses()
        {
            try
            {
                SalvageDAO salvageBusiness = new SalvageDAO();
                return DTOAssembler.CreateGetPaymentClasses(salvageBusiness.GetPaymentClasses());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public List<PaymentQuotaDTO> CalculateSaleAgreedPlan(DateTime dateStart, decimal totalSale, int payments, string currencyDescription)
        {
            SaleBusiness saleBusiness = new SaleBusiness();
            return saleBusiness.CalculateSaleAgreedPlan(dateStart, totalSale, payments, currencyDescription);
        }


        public List<BuyerDTO> GetBuyersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                BuyerDAO buyerDAO = new BuyerDAO();
                return DTOAssembler.CreateBuyers(buyerDAO.GetBuyersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorGetBuyers), ex);
            }
        }

        public List<PrefixDTO> GetPrefixesSalvage()
        {
            List<PrefixDTO> prefixes = DTOAssembler.CreatePrefixes(DelegateService.commonServiceCore.GetPrefixes());

            try
            {
                List<string> recoveryPrefixes = Convert.ToString(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_SALVAGE_PREFIX)).Split(',').ToList();

                if (recoveryPrefixes != null)
                {
                    return prefixes.Where(x => recoveryPrefixes.Any(y => Convert.ToInt32(y) == x.Id)).ToList();
                }

                return prefixes;
            }
            catch (Exception)
            {
                return prefixes;
            }
        }
    }
}
