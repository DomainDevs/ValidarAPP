using AutoMapper;
using Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Business;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs.AccountingConcept;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GLINTDTO = Sistran.Core.Integration.GeneralLedgerServices.DTOs;
using GLMO = Sistran.Core.Application.GeneralLedgerServices.DTOs;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        internal static List<ParameterDTO> CreateCoinsuranceRuleParameters(AccountingPaymentRequestDTO accountingPaymentRequest, CoInsuranceAssignedDTO coInsuranceAssigned)
        {
            List<ParameterDTO> paymentRequestParameters = new List<ParameterDTO> {
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.BranchId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.SalePointId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PrefixId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.CurrencyId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.Amount * (coInsuranceAssigned.PartCiaPercentage / 100), CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PaymentSourceId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(3, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(coInsuranceAssigned.InsuranceCompanyId, CultureInfo.InvariantCulture)
                }
            };

            return paymentRequestParameters;
        }

        internal static List<ParameterDTO> CreatePaymentRequestRuleParameters(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            List<ParameterDTO> paymentRequestParameters = new List<ParameterDTO> {
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.BranchId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.SalePointId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PrefixId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.CurrencyId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.Amount, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PaymentSourceId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(1, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(-1, CultureInfo.InvariantCulture)
                }
            };

            return paymentRequestParameters;
        }

        internal static List<ParameterDTO> CreateChargeRequestRuleParameters(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            List<ParameterDTO> paymentRequestParameters = new List<ParameterDTO> {
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.BranchId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.SalePointId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PrefixId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.CurrencyId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.Amount, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.PaymentSourceId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(1, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(-1, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.SalvageId, CultureInfo.InvariantCulture)
                },
                new ParameterDTO
                {
                    Value = Convert.ToString(accountingPaymentRequest.RecoveryId, CultureInfo.InvariantCulture)
                }
            };

            return paymentRequestParameters;
        }

        internal static GLMO.JournalEntryDTO CreateJournalEntry(AccountingPaymentRequestDTO accountingPaymentRequest, int moduleId)
        {
            return new GLMO.JournalEntryDTO
            {
                Id = 0,
                AccountingCompany = new GLMO.AccountingCompanyDTO()
                {
                    AccountingCompanyId = -1
                },
                AccountingMovementType = new GLMO.AccountingMovementTypeDTO(),
                ModuleDateId = moduleId,
                Branch = new GLMO.BranchDTO()
                {
                    Id = accountingPaymentRequest.BranchId
                },
                SalePoint = new GLMO.SalePointDTO()
                {
                    Id = accountingPaymentRequest.SalePointId
                },
                EntryNumber = 0,
                TechnicalTransaction = accountingPaymentRequest.TechnicalTransaction,
                Description = accountingPaymentRequest.Description,
                AccountingDate = accountingPaymentRequest.AccountingDate,
                RegisterDate = DateTime.Now,
                Status = 1,
                UserId = accountingPaymentRequest.UserId,
                JournalEntryItems = CreateJournarEntryItems(accountingPaymentRequest)
            };
        }

        internal static List<GLMO.JournalEntryItemDTO> CreateJournarEntryItems(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            ClaimsGeneralLedgerWorkerBusiness claimsGeneralLedgerWorkerBusiness = new ClaimsGeneralLedgerWorkerBusiness();
            List<GLMO.JournalEntryItemDTO> journalEntryItemsDTO = new List<GLMO.JournalEntryItemDTO>();

            foreach (CoInsuranceAssignedDTO coInsuranceAssigned in accountingPaymentRequest.CoInsuranceAssigneds)
            {
                journalEntryItemsDTO.Add(CreateCoInsuranceJournalEntryItem(accountingPaymentRequest, coInsuranceAssigned));
            }

            foreach (VoucherDTO voucher in accountingPaymentRequest.Vouchers)
            {
                foreach (ConceptDTO concept in voucher.Concepts)
                {
                    journalEntryItemsDTO.Add(CreateConceptJournalEntryItem(accountingPaymentRequest, voucher, concept));

                    foreach (TaxDTO tax in concept.Taxes)
                    {
                        journalEntryItemsDTO.Add(CreateTaxJournalEntryItem(accountingPaymentRequest, voucher, concept, tax));
                    }
                }
            }

            //Cuenta puente
            journalEntryItemsDTO.Add(CreateJournalEntryItem(accountingPaymentRequest));
            
            return claimsGeneralLedgerWorkerBusiness.CalculateDifferenceIntoDebitsAndCredits(journalEntryItemsDTO);
        }

        internal static GLMO.JournalEntryItemDTO CreateJournalEntryItem(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            return new GLMO.JournalEntryItemDTO
            {
                AccountingAccount = new GLMO.AccountingAccountDTO
                {
                    AccountingAccountId = accountingPaymentRequest.AccountingAccountId,
                    Number = accountingPaymentRequest.AccountingAccountNumber
                },
                Amount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = accountingPaymentRequest.CurrencyId
                    },
                    Value = Math.Abs(Math.Round(accountingPaymentRequest.Amount + accountingPaymentRequest.Vouchers.Sum(x => x.Concepts.Sum(y => y.Taxes.Sum(z => z.Amount))), 2, MidpointRounding.ToEven))
                },
                ExchangeRate = new GLMO.ExchangeRateDTO
                {
                    SellAmount = accountingPaymentRequest.Vouchers.FirstOrDefault().ExchangeRate
                },
                Analysis = new List<GLMO.AnalysisDTO>(),
                CostCenters = new List<GLMO.CostCenterDTO>(),
                Currency = new GLMO.CurrencyDTO
                {
                    Id = accountingPaymentRequest.CurrencyId
                },
                Description = accountingPaymentRequest.Description,
                EntryType = new GLMO.EntryTypeDTO(),
                Id = 0,
                Individual = new GLMO.IndividualDTO
                {
                    IndividualId = accountingPaymentRequest.IndividualId
                },
                PostDated = new List<GLMO.PostDatedDTO>(),
                Receipt = new GLMO.ReceiptDTO(),
                ReconciliationMovementType = new GLMO.ReconciliationMovementTypeDTO(),
                SourceCode = 0,
                LocalAmount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = accountingPaymentRequest.CurrencyId
                    },
                    Value = Math.Abs(Math.Round(accountingPaymentRequest.Amount + accountingPaymentRequest.Vouchers.Sum(x => x.Concepts.Sum(y => y.Taxes.Sum(z => z.Amount))), 2, MidpointRounding.ToEven) * accountingPaymentRequest.Vouchers.FirstOrDefault().ExchangeRate)
                },
                Branch = new GLMO.BranchDTO
                {
                    Id = accountingPaymentRequest.BranchId
                },
                SalePoint = new GLMO.SalePointDTO
                {
                    Id = accountingPaymentRequest.SalePointId
                },
                AccountingNature = accountingPaymentRequest.AccountingNatureId
            };
        }

        internal static GLMO.JournalEntryItemDTO CreateCoInsuranceJournalEntryItem(AccountingPaymentRequestDTO accountingPaymentRequest, CoInsuranceAssignedDTO coInsuranceAssigned)
        {
            return new GLMO.JournalEntryItemDTO
            {
                AccountingAccount = new GLMO.AccountingAccountDTO
                {
                    AccountingAccountId = coInsuranceAssigned.AccountingAccountId,
                    Number = coInsuranceAssigned.AccountingAccountNumber,
                    AccountingNature = coInsuranceAssigned.AccountingNatureId
                },
                Amount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = accountingPaymentRequest.CurrencyId
                    },
                    Value = Math.Abs(accountingPaymentRequest.Amount * (coInsuranceAssigned.PartCiaPercentage / 100))
                },
                ExchangeRate = new GLMO.ExchangeRateDTO
                {
                    SellAmount = accountingPaymentRequest.Vouchers.FirstOrDefault().ExchangeRate
                },
                Analysis = new List<GLMO.AnalysisDTO>(),
                CostCenters = new List<GLMO.CostCenterDTO>(),
                Currency = new GLMO.CurrencyDTO
                {
                    Id = accountingPaymentRequest.CurrencyId
                },
                Description = accountingPaymentRequest.Description,
                EntryType = new GLMO.EntryTypeDTO(),
                Id = 0,
                Individual = new GLMO.IndividualDTO
                {
                    //To Remember: No cambiar porque la integración a 2G arma la cuenta corriente de coaseguro con este dato
                    IndividualId = coInsuranceAssigned.InsuranceCompanyId
                },
                PostDated = new List<GLMO.PostDatedDTO>(),
                Receipt = new GLMO.ReceiptDTO(),
                ReconciliationMovementType = new GLMO.ReconciliationMovementTypeDTO(),
                SourceCode = 0,
                LocalAmount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = accountingPaymentRequest.CurrencyId
                    },
                    Value = Math.Abs(accountingPaymentRequest.Amount * (coInsuranceAssigned.PartCiaPercentage / 100) * accountingPaymentRequest.Vouchers.FirstOrDefault().ExchangeRate)
                },
                Branch = new GLMO.BranchDTO
                {
                    Id = accountingPaymentRequest.BranchId
                },
                SalePoint = new GLMO.SalePointDTO
                {
                    Id = accountingPaymentRequest.SalePointId
                },
                AccountingNature = (int)AccountingNatures.Debit,
            };
        }

        internal static GLMO.JournalEntryItemDTO CreateConceptJournalEntryItem(AccountingPaymentRequestDTO accountingPaymentRequest, VoucherDTO voucher, ConceptDTO concept)
        {
            var coinsuranceAmout = accountingPaymentRequest.CoInsuranceAssigneds.Sum(x => (x.PartCiaPercentage / 100 * concept.Amount));
            return new GLMO.JournalEntryItemDTO
            {
                AccountingAccount = new GLMO.AccountingAccountDTO
                {
                    AccountingAccountId = concept.AccountingAccountId,
                    Number = concept.AccountingAccountNumber,
                    AccountingNature = concept.AccountingNatureId
                },
                Amount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = voucher.CurrencyId
                    },
                    Value = Math.Abs(concept.Amount - coinsuranceAmout)
                },
                ExchangeRate = new GLMO.ExchangeRateDTO
                {
                    SellAmount = voucher.ExchangeRate
                },
                Analysis = new List<GLMO.AnalysisDTO>(),
                CostCenters = new List<GLMO.CostCenterDTO>(),
                Currency = new GLMO.CurrencyDTO
                {
                    Id = voucher.CurrencyId
                },
                Description = concept.Description,
                EntryType = new GLMO.EntryTypeDTO(),
                Id = 0,
                Individual = new GLMO.IndividualDTO
                {
                    IndividualId = accountingPaymentRequest.IndividualId
                },
                PostDated = new List<GLMO.PostDatedDTO>(),
                Receipt = new GLMO.ReceiptDTO(),
                ReconciliationMovementType = new GLMO.ReconciliationMovementTypeDTO(),
                SourceCode = 0,
                LocalAmount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = voucher.CurrencyId
                    },
                    Value = Math.Abs((concept.Amount - coinsuranceAmout)) * voucher.ExchangeRate
                },
                AccountingConcept = concept.Id,
                Branch = new GLMO.BranchDTO
                {
                    Id = accountingPaymentRequest.BranchId
                },
                SalePoint = new GLMO.SalePointDTO
                {
                    Id = accountingPaymentRequest.SalePointId
                },
                AccountingNature = accountingPaymentRequest.SalvageId > 0 || accountingPaymentRequest.RecoveryId > 0 ? (int)AccountingNatures.Credit : (int)AccountingNatures.Debit,

            };
        }

        internal static GLMO.JournalEntryItemDTO CreateTaxJournalEntryItem(AccountingPaymentRequestDTO accountingPaymentRequest, VoucherDTO voucher, ConceptDTO concept, TaxDTO tax)
        {
            return new GLMO.JournalEntryItemDTO
            {
                AccountingAccount = new GLMO.AccountingAccountDTO
                {
                    AccountingAccountId = tax.AccountingAccountId,
                    Number = tax.AccountingAccountNumber,
                    AccountingNature = tax.AccountingNatureId
                },
                Amount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = voucher.CurrencyId
                    },
                    Value = Math.Abs(Math.Round(tax.Amount, 2, MidpointRounding.ToEven))
                },
                ExchangeRate = new GLMO.ExchangeRateDTO
                {
                    SellAmount = voucher.ExchangeRate
                },
                Analysis = new List<GLMO.AnalysisDTO>(),
                CostCenters = new List<GLMO.CostCenterDTO>(),
                Currency = new GLMO.CurrencyDTO
                {
                    Id = voucher.CurrencyId
                },
                Description = tax.Description,
                EntryType = new GLMO.EntryTypeDTO(),
                Id = 0,
                Individual = new GLMO.IndividualDTO
                {
                    IndividualId = accountingPaymentRequest.IndividualId
                },
                PostDated = new List<GLMO.PostDatedDTO>(),
                Receipt = new GLMO.ReceiptDTO(),
                ReconciliationMovementType = new GLMO.ReconciliationMovementTypeDTO(),
                SourceCode = 0,
                LocalAmount = new GLMO.AmountDTO
                {
                    Currency = new GLMO.CurrencyDTO
                    {
                        Id = voucher.CurrencyId
                    },
                    Value = Math.Abs(Math.Round(tax.Amount, 2, MidpointRounding.ToEven)) * voucher.ExchangeRate
                },
                AccountingConcept = concept.Id,
                Branch = new GLMO.BranchDTO
                {
                    Id = accountingPaymentRequest.BranchId
                },
                SalePoint = new GLMO.SalePointDTO
                {
                    Id = accountingPaymentRequest.SalePointId
                },
                AccountingNature = tax.Amount > 0 ? (int)AccountingNatures.Debit : (int)AccountingNatures.Credit,
            };
        }

        public static IMapper CreateMapIntAccountingConcepts()
        {
            var config = MapperCache.GetMapper<GLINTDTO.AccountingConcepts.AccountingConceptDTO, AccountingConceptDTO>(cfg =>
            {
                cfg.CreateMap<GLINTDTO.AccountingConcepts.AccountingConceptDTO, AccountingConceptDTO>();
            });
            return config;
        }

        public static AccountingConceptDTO ToDTOInt(this GLINTDTO.AccountingConcepts.AccountingConceptDTO accountingConcept)
        {
            var config = CreateMapIntAccountingConcepts();
            return config.Map<GLINTDTO.AccountingConcepts.AccountingConceptDTO, AccountingConceptDTO>(accountingConcept);
        }

        public static IEnumerable<AccountingConceptDTO> ToDTOsInt(this IEnumerable<GLINTDTO.AccountingConcepts.AccountingConceptDTO> accountingConcept)
        {
            return accountingConcept.Select(ToDTOInt);
        }

        public static IMapper CreateMapIntJournalEntry()
        {
            var config = MapperCache.GetMapper<GLINTDTO.JournalEntryDTO, JournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<GLINTDTO.JournalEntryDTO, JournalEntryDTO>();
                cfg.CreateMap<GLINTDTO.AccountingCompanyDTO, AccountingCompanyDTO>();
                cfg.CreateMap<GLINTDTO.AccountingMovementTypeDTO, AccountingMovementTypeDTO>();
                cfg.CreateMap<GLINTDTO.BranchDTO, BranchDTO>();
                cfg.CreateMap<GLINTDTO.SalePointDTO, SalePointDTO>();
                cfg.CreateMap<GLINTDTO.JournalEntryItemDTO, JournalEntryItemDTO>();
                cfg.CreateMap<GLINTDTO.IndividualDTO, IndividualDTO>();
                cfg.CreateMap<GLINTDTO.EntryTypeDTO, EntryTypeDTO>();
                cfg.CreateMap<GLINTDTO.EntryTypeItemDTO, EntryTypeItemDTO>();
                cfg.CreateMap<GLINTDTO.AccountingAccountDTO, AccountingAccountDTO>();
                cfg.CreateMap<GLINTDTO.CostCenterDTO, CostCenterDTO>();
                cfg.CreateMap<GLINTDTO.AnalysisDTO, AnalysisDTO>();
                cfg.CreateMap<GLINTDTO.AnalysisConceptDTO, AnalysisConceptDTO>();
                cfg.CreateMap<GLINTDTO.PostDatedDTO, PostDatedDTO>();
                cfg.CreateMap<GLINTDTO.AmountDTO, AmountDTO>();
            });
            return config;
        }

        public static JournalEntryDTO ToDTOInt(this GLINTDTO.JournalEntryDTO journalEntry)
        {
            var config = CreateMapIntJournalEntry();
            return config.Map<GLINTDTO.JournalEntryDTO, JournalEntryDTO>(journalEntry);
        }

        public static IMapper CreateMapIntMovementTypes()
        {
            var config = MapperCache.GetMapper<GLINTDTO.AccountingConcepts.MovementTypeDTO, MovementTypeDTO>(cfg =>
            {
                cfg.CreateMap<GLINTDTO.AccountingConcepts.MovementTypeDTO, MovementTypeDTO>();
                cfg.CreateMap<GLINTDTO.AccountingConcepts.ConceptSourceDTO, ConceptSourceDTO>();
            });
            return config;
        }

        public static MovementTypeDTO ToDTOInt(this GLINTDTO.AccountingConcepts.MovementTypeDTO movementType)
        {
            var config = CreateMapIntMovementTypes();
            return config.Map<GLINTDTO.AccountingConcepts.MovementTypeDTO, MovementTypeDTO>(movementType);
        }

        public static IEnumerable<MovementTypeDTO> ToDTOsInt(this IEnumerable<GLINTDTO.AccountingConcepts.MovementTypeDTO> movementType)
        {
            return movementType.Select(ToDTOInt);
        }

        public static IMapper CreateMapIntConceptSources()
        {
            var config = MapperCache.GetMapper<GLINTDTO.AccountingConcepts.ConceptSourceDTO, ConceptSourceDTO>(cfg =>
            {
                cfg.CreateMap<GLINTDTO.AccountingConcepts.ConceptSourceDTO, ConceptSourceDTO>();
            });
            return config;
        }

        public static ConceptSourceDTO ToDTOInt(this GLINTDTO.AccountingConcepts.ConceptSourceDTO conceptSource)
        {
            var config = CreateMapIntConceptSources();
            return config.Map<GLINTDTO.AccountingConcepts.ConceptSourceDTO, ConceptSourceDTO>(conceptSource);
        }

        public static IEnumerable<ConceptSourceDTO> ToDTOsInt(this IEnumerable<GLINTDTO.AccountingConcepts.ConceptSourceDTO> conceptSource)
        {
            return conceptSource.Select(ToDTOInt);
        }
    }
}
