using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Models
{
    static internal class ModelAssembler
    {
        internal static PaymentRequestReportModel CreatePaymentRequestReportModel(PaymentRequestReportDTO paymentRequestReport)
        {
            return new PaymentRequestReportModel
            {
                Number = paymentRequestReport.Number,
                ReportDate = paymentRequestReport.ReportDate,
                Branch = paymentRequestReport.Branch,
                ContractCity = paymentRequestReport.ContractCity,
                PolicyAgent = paymentRequestReport.PolicyAgent,
                Prefix = paymentRequestReport.Prefix,
                PolicyNumber = paymentRequestReport.PolicyNumber,
                ClaimNumber = paymentRequestReport.ClaimNumber,
                ClaimRegistrationDate = paymentRequestReport.ClaimRegistrationDate,
                PolicyHolder = paymentRequestReport.PolicyHolder,
                PolicyInsured = paymentRequestReport.PolicyInsured,
                PaymentBeneficiaryPersonType = paymentRequestReport.PaymentBeneficiaryPersonType,
                PaymentBeneficiaryName = paymentRequestReport.PaymentBeneficiaryName,
                PaymentBeneficiaryDocumentNumber = paymentRequestReport.PaymentBeneficiaryDocumentNumber,
                PaymentTechnicalTransaction = paymentRequestReport.PaymentTechnicalTransaction,
                VoucherType = paymentRequestReport.VoucherType,
                PaymentMethod = paymentRequestReport.PaymentMethod,
                PaymentCurrency = paymentRequestReport.PaymentCurrency,
                VoucherCurrency = paymentRequestReport.VoucherCurrency,
                CostCenter = paymentRequestReport.CostCenter,
                PaymentTotalAmount = paymentRequestReport.PaymentTotalAmount,
                TRM = paymentRequestReport.TRM,
                TotalAmountConcepts = paymentRequestReport.TotalAmountConcepts,
                PaymentDescription = paymentRequestReport.PaymentDescription
            };
        }

        internal static List<PaymentRequestDetailReportModel> CreatePaymentRequestDetailsReportModel(List<ClaimReportDTO> claimsReportDTO)
        {
            List<PaymentRequestDetailReportModel> paymentRequestDetailsReportModel = new List<PaymentRequestDetailReportModel>();

            foreach (ClaimReportDTO claim in claimsReportDTO)
            {
                paymentRequestDetailsReportModel.Add(CreatePaymentRequestDetailReportModel(claim));
            }

            return paymentRequestDetailsReportModel;
        }

        internal static PaymentRequestDetailReportModel CreatePaymentRequestDetailReportModel(ClaimReportDTO claimReportDTO)
        {
            return new PaymentRequestDetailReportModel
            {
                ClaimNumber = claimReportDTO.ClaimNumber,
                SubClaim = claimReportDTO.SubClaim,
                BusinessTurn = claimReportDTO.BusinessTurn,
                Coverage = claimReportDTO.Coverage,
                Deducible = claimReportDTO.Deducible,
                Compensation = claimReportDTO.Compensation,
                Expenses = claimReportDTO.Expenses,
                Reinsurance = claimReportDTO.Reinsurance
            };
        }        

        internal static List<PaymentRequestTaxReportModel> CreatePaymentRequestTaxesReportModel(List<TaxReportDTO> taxes)
        {
            List<PaymentRequestTaxReportModel> paymentRequestTaxesReportModel = new List<PaymentRequestTaxReportModel>();
            
            foreach (TaxReportDTO tax in taxes)
            {
                paymentRequestTaxesReportModel.Add(CreatePaymentRequestTaxReportModel(tax));
            }

            return paymentRequestTaxesReportModel;
        }

        internal static PaymentRequestTaxReportModel CreatePaymentRequestTaxReportModel(TaxReportDTO tax)
        {
            return new PaymentRequestTaxReportModel
            {
                TaxCode = tax.TaxCode,
                TaxCategory = tax.TaxCategory,
                TaxDescription = tax.TaxDescription,
                TaxBaseAmount = tax.TaxBaseAmount,
                TaxValue = tax.TaxValue
            };
        }

        internal static List<PaymentRequestCoInsuranceReportModel> CreatePaymentRequestCoInsurancesReportModel(List<CoInsuranceReportDTO> coInsurances)
        {
            List<PaymentRequestCoInsuranceReportModel> coinsurances = new List<PaymentRequestCoInsuranceReportModel>();

            foreach (CoInsuranceReportDTO coInsurance in coInsurances)
            {
                coinsurances.Add(CreatePaymentRequestCoInsuranceReportModel(coInsurance));
            }

            return coinsurances;
        }

        internal static PaymentRequestCoInsuranceReportModel CreatePaymentRequestCoInsuranceReportModel(CoInsuranceReportDTO coInsurance)
        {
            return new PaymentRequestCoInsuranceReportModel
            {
                Amount = coInsurance.Amount,
                Participation = coInsurance.Participation,
                Company = coInsurance.Company,
            };
        }

        internal static List<PaymentRequestAccountingReportModel> CreatePaymentRequestAccountingReportModels(List<AccountingReportDTO> accountings)
        {
            List<PaymentRequestAccountingReportModel> paymentRequestAccountingReportModels = new List<PaymentRequestAccountingReportModel>();

            foreach (AccountingReportDTO accounting in accountings)
            {
                paymentRequestAccountingReportModels.Add(CreatePaymentRequestAccountingReportModel(accounting));
            }

            return paymentRequestAccountingReportModels;
        }

        internal static PaymentRequestAccountingReportModel CreatePaymentRequestAccountingReportModel(AccountingReportDTO accounting)
        {
            return new PaymentRequestAccountingReportModel
            {
               Account = accounting.Account,
               CreditAmount = accounting.CreditAmount,
               DebitAmount = accounting.DebitAmount,
               Description = accounting.Description
            };
        }
    }
}