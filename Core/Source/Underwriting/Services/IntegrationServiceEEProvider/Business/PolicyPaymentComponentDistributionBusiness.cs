using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.Business
{
    public class PolicyPaymentComponentDistributionBusiness
    {
        public PayerPaymentDTO GetPayment(int EndorsementId, int Quota)
        {
            try
            {
                PolicyPaymentComponentDistributionDAO payerPaymentDAO = new PolicyPaymentComponentDistributionDAO();
                return DTOAssembler.CreatePayerPaymentDTO(payerPaymentDAO.GetPayerPayment(EndorsementId, Quota));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        public  void UpdateStatusPayerPayment(PayerPaymentDTO payerPayment)
        {
            try
            {
                PolicyPaymentComponentDistributionDAO payerPaymentDAO = new PolicyPaymentComponentDistributionDAO();
                payerPaymentDAO.UpdateStatusPayerPayment(ModelAssembler.CreatePayerPaymentModel(payerPayment));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<PayerPaymentComponentDTO> GetPaymentComp(int payerPaymentId)
        {
            try
            {
                PolicyPaymentComponentDistributionDAO payerPaymentDAO = new PolicyPaymentComponentDistributionDAO();
                return DTOAssembler.CreatePayerPaymentCompsDTO(payerPaymentDAO.GetPayerPaymentComp(payerPaymentId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<PayerPaymentComponentLBSBDTO> GetPaymentCompLbsb(int payerPaymentId)
        {
            try
            {
                PolicyPaymentComponentDistributionDAO payerPaymentDAO = new PolicyPaymentComponentDistributionDAO();
                return DTOAssembler.CreatePayerPaymentCompsDTO(payerPaymentDAO.GetPayerPaymentCompLbsb(payerPaymentId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<PremiumSearchPolicyDTO> GetPremiumSearchPolicies(SearchPolicyPaymentDTO searchPolicyPaymentDTO) {
            PolicyPremiumDAO policyPremiumDAO = new PolicyPremiumDAO();
            return DTOAssembler.CreateSearchPremiumPolicies(policyPremiumDAO.GetPremiumSearchPolicies(ModelAssembler.CreateSearchPolicyPayment(searchPolicyPaymentDTO)));
            }
    }
}
