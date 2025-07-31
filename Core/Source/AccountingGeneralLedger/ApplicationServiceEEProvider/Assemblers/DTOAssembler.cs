using Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        internal static List<List<ParameterDTO>> CreateRuleParameters(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            List<List<ParameterDTO>> ruleParameters = new List<List<ParameterDTO>>();

            List<ParameterDTO> paymentRequestParameters = new List<ParameterDTO> {
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.Amount, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PrefixId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.BranchId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.Id, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.IndividualId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.Number, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.AccountingDate, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.UserId, CultureInfo.InvariantCulture)
                }
            };

            ruleParameters.Add(paymentRequestParameters);

            foreach (VoucherDTO voucher in accountingPaymentRequest.Vocuher)
            {
                List<ParameterDTO> voucherParameters = new List<ParameterDTO>
                {
                    new ParameterDTO
                    {
                        Value = Convert.ToString(voucher.Id, CultureInfo.InvariantCulture)
                    },
                    new ParameterDTO
                    {
                        Value = Convert.ToString(voucher.CurrencyId, CultureInfo.InvariantCulture)
                    },
                    new ParameterDTO
                    {
                        Value = Convert.ToString(voucher.ExchangeRate, CultureInfo.InvariantCulture)
                    }
                };

                ruleParameters.Add(voucherParameters);

                foreach (ConceptDTO concept in voucher.Concepts)
                {
                    List<ParameterDTO> conceptParameters = new List<ParameterDTO>
                    {
                        new ParameterDTO
                        {
                            Value = Convert.ToString(concept.Id, CultureInfo.InvariantCulture)
                        }
                    };

                    ruleParameters.Add(conceptParameters);

                    foreach (TaxDTO tax in concept.Taxes)
                    {
                        List<ParameterDTO> conceptTaxParameters = new List<ParameterDTO>
                        {
                            new ParameterDTO
                            {
                                Value = Convert.ToString(tax.TaxId, CultureInfo.InvariantCulture)
                            },
                            new ParameterDTO
                            {
                                Value = Convert.ToString(tax.TaxValues, CultureInfo.InvariantCulture)
                            }
                        };

                        ruleParameters.Add(conceptTaxParameters);
                    }
                }
            }

            return ruleParameters;
        }
    }
}
