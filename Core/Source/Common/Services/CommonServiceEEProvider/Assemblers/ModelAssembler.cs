using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {

        #region Country

        public static Country CreateCountry(COMMEN.Country entityCountry)
        {
            return new Country
            {
                Id = entityCountry.CountryCode,
                Description = entityCountry.Description,
                SmallDescription = entityCountry.SmallDescription
            };
        }

        public static List<Country> CreateCountries(BusinessCollection businessCollection)
        {
            var sync = new object();
            List<Country> countries = new List<Country>();
            TP.Parallel.ForEach(businessCollection, (entity) =>
            {
                lock (sync)
                {
                    countries.Add(ModelAssembler.CreateCountry((COMMEN.Country)entity));
                }
            }
            );

            return countries;
        }

        #endregion

        #region Currency

        private static Currency CreateCurrency(COMMEN.Currency entityCurrency)
        {
            return new Currency
            {
                Id = entityCurrency.CurrencyCode,
                Description = entityCurrency.Description,
                SmallDescription = entityCurrency.SmallDescription,
                TinyDescription = entityCurrency.TinyDescription
            };
        }

        public static List<Currency> CreateCurrencies(BusinessCollection businessCollection)
        {
            List<Currency> currencies = new List<Currency>();

            foreach (COMMEN.Currency entity in businessCollection)
            {
                currencies.Add(ModelAssembler.CreateCurrency(entity));
            }

            return currencies.OrderBy(x => x.Id).ToList();
        }

        #endregion

        #region State

        public static State CreateState(COMMEN.State entityState)
        {
            return new State
            {
                Id = entityState.StateCode,
                Description = entityState.Description,
                SmallDescription = entityState.SmallDescription,
                Country = new Country
                {
                    Id = entityState.CountryCode
                }
            };
        }

        public static List<State> CreateStates(BusinessCollection businessCollection)
        {
            var sync = new object();
            List<State> states = new List<State>();

            TP.Parallel.ForEach(businessCollection, (entity) =>
            {
                lock (sync)
                {
                    states.Add(ModelAssembler.CreateState((COMMEN.State)entity));
                }
            }
            );
            return states;
        }

        #endregion

        #region City

        public static City CreateCity(COMMEN.City entityCity)
        {
            return new City
            {
                Id = entityCity.CityCode,
                Description = entityCity.Description,
                SmallDescription = entityCity.SmallDescription,
                DANECode = entityCity.DivipolaId,
                //ExtendedProperties = CreateExtendedProperties(entityCity.ExtendedProperties),
                State = new State
                {
                    Id = entityCity.StateCode,
                    Country = new Country
                    {
                        Id = entityCity.CountryCode
                    }
                }
            };
        }

        public static List<City> CreateCities(BusinessCollection businessCollection)
        {
            var sync = new object();
            List<City> cities = new List<City>();
            TP.Parallel.ForEach(businessCollection, (entity) =>
            {
                lock (sync)
                {
                    cities.Add(ModelAssembler.CreateCity((COMMEN.City)entity));
                }
            }
            );

            return cities;
        }

        #endregion

        #region Bank

        public static Bank CreateBank(COMMEN.Bank entityBank)
        {
            return new Bank
            {
                Id = entityBank.BankCode,
                Description = entityBank.Description,
                Aba = entityBank.Aba,
                Address = entityBank.Address,
                BankCode = entityBank.BankCode,
                BankTypeCode = entityBank.BankTypeCode,
                ChargeAch = entityBank.ChargeAch,
                ChargeAchId = entityBank.ChargeAchId,
                ChisId = entityBank.ChipsId,
                CityDescription = entityBank.CityDescription,
                CountryCode = entityBank.CountryCode,
                Enable = entityBank.Enable,
                PaymentAch = entityBank.PaymentAch,
                PaymentAchId = entityBank.PaymentAchId,
                SuperCode = entityBank.SuperCode,
                Swift = entityBank.Swift,
                TributaryIdNo = entityBank.TributaryIdNo,
                SquareCode = entityBank.SquareCode
            };
        }

        public static List<Bank> CreateBanks(BusinessCollection businessCollection)
        {
            List<Bank> banks = new List<Bank>();

            foreach (COMMEN.Bank entity in businessCollection)
            {
                banks.Add(ModelAssembler.CreateBank(entity));
            }

            return banks;
        }

        #endregion

        #region BankBranch

        private static BankBranch CreateBanckBranch(COMMEN.BankBranch entityBankBranch)
        {
            return new BankBranch
            {
                Id = entityBankBranch.BankBranchCode,
                Description = entityBankBranch.Description
            };
        }

        public static List<BankBranch> CreateBankBranches(BusinessCollection businessCollection)
        {
            List<BankBranch> bankBranches = new List<BankBranch>();

            foreach (COMMEN.BankBranch entity in businessCollection)
            {
                bankBranches.Add(ModelAssembler.CreateBanckBranch(entity));
            }

            return bankBranches;
        }

        #endregion

        #region PaymentMethod

        private static PaymentMethod CreatePaymentMethod(COMMEN.PaymentMethod entityPaymentMethod)
        {
            return new PaymentMethod
            {
                Id = entityPaymentMethod.PaymentMethodCode,
                Description = entityPaymentMethod.Description,
            };
        }

        public static List<PaymentMethod> CreatePaymentMethods(BusinessCollection businessCollection)
        {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

            foreach (COMMEN.PaymentMethod entity in businessCollection)
            {
                paymentMethods.Add(ModelAssembler.CreatePaymentMethod(entity));
            }

            return paymentMethods;
        }

        #endregion

        #region Parameter

        public static Parameter CreateParameter(COMMEN.Parameter entityParameter)
        {
            return new Parameter
            {
                Id = entityParameter.ParameterId,
                Description = entityParameter.Description,
                BoolParameter = entityParameter.BoolParameter,
                NumberParameter = entityParameter.NumberParameter,
                DateParameter = entityParameter.DateParameter,
                TextParameter = entityParameter.TextParameter,
                PercentageParameter = entityParameter.PercentageParameter,
                AmountParameter = entityParameter.AmountParameter
            };
        }

        public static List<Parameter> CreateParameters(BusinessCollection businessCollection)
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (COMMEN.Parameter entity in businessCollection)
            {
                parameters.Add(ModelAssembler.CreateParameter(entity));
            }

            return parameters;
        }

        public static Parameter CreateCoParameter(COMMEN.CoParameter entityCoParameter)
        {
            return new Parameter
            {
                Id = entityCoParameter.ParameterId,
                Description = entityCoParameter.Description,
                BoolParameter = entityCoParameter.BoolParameter,
                NumberParameter = entityCoParameter.NumberParameter,
                DateParameter = entityCoParameter.DateParameter,
                TextParameter = entityCoParameter.TextParameter,
                PercentageParameter = entityCoParameter.PercentageParameter,
                AmountParameter = entityCoParameter.AmountParameter
            };
        }

        public static List<Parameter> CreateCoParameters(BusinessCollection businessCollection)
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (COMMEN.CoParameter entity in businessCollection)
            {
                parameters.Add(ModelAssembler.CreateCoParameter(entity));
            }

            return parameters;
        }

        #endregion

        #region LineBusiness

        public static LineBusiness CreateLineBusiness(COMMEN.LineBusiness lineBusiness)
        {
            return new LineBusiness
            {
                Id = lineBusiness.LineBusinessCode,
                Description = lineBusiness.Description,
                ShortDescription = lineBusiness.SmallDescription,
                TyniDescription = lineBusiness.TinyDescription,
                ReportLineBusiness = int.Parse(lineBusiness.ReportLineBusinessCode)
            };
        }

        public static List<LineBusiness> CreateLinesBusiness(BusinessCollection businessCollection)
        {
            List<LineBusiness> linesBusiness = new List<LineBusiness>();

            foreach (COMMEN.LineBusiness entity in businessCollection)
            {
                linesBusiness.Add(ModelAssembler.CreateLineBusiness(entity));
            }

            return linesBusiness;
        }
        public static List<LineBusiness> CreateCoveredRiskType(BusinessCollection businessCollection, BusinessCollection hardRiskCollection)
        {
            List<LineBusiness> linesBusiness = new List<LineBusiness>();
            List<PARAMEN.HardRiskType> hardRisks = hardRiskCollection.Cast<PARAMEN.HardRiskType>().ToList();
            foreach (COMMEN.LineBusiness entity in businessCollection)
            {
                PARAMEN.HardRiskType hardRisk = hardRisks.Where(x => x.LineBusinessCode == entity.LineBusinessCode).FirstOrDefault();
                LineBusiness line = ModelAssembler.CreateHardRiskType(entity, hardRisk);
                linesBusiness.Add(line);
            }

            return linesBusiness;
        }
        #endregion

        public static List<COMMML.PrefixType> CreatePrefixType(BusinessCollection businessCollection)
        {
            List<COMMML.PrefixType> PrefixType = new List<COMMML.PrefixType>();

            foreach (PARAMEN.PrefixType entity in businessCollection)
            {
                PrefixType.Add(ModelAssembler.CreatePrefixTypeType(entity));
            }

            return PrefixType;
        }

        private static COMMML.PrefixType CreatePrefixTypeType(PARAMEN.PrefixType entity)
        {
            return new COMMML.PrefixType()
            {
                Id = entity.PrefixTypeCode,
                Description = entity.Description,
                SmallDescription = entity.SmallDescription
            };
        }

        #region SubLineBusiness

        public static SubLineBusiness CreateSubLineBusiness(COMMEN.SubLineBusiness subLineBusiness)
        {
            return new SubLineBusiness()
            {
                Id = subLineBusiness.SubLineBusinessCode,
                Description = subLineBusiness.Description,
                SmallDescription = subLineBusiness.SmallDescription,
                LineBusinessId = subLineBusiness.LineBusinessCode
            };
        }

        public static List<SubLineBusiness> CreateSubLinesBusiness(BusinessCollection businessCollection)
        {
            List<SubLineBusiness> subLinesBusiness = new List<SubLineBusiness>();

            foreach (COMMEN.SubLineBusiness entity in businessCollection)
            {
                subLinesBusiness.Add(ModelAssembler.CreateSubLineBusiness(entity));
            }

            return subLinesBusiness;
        }

        #endregion

        #region Branch

        public static Branch CreateBranch(COMMEN.Branch entityBranch)
        {
            return new Branch
            {
                Id = entityBranch.BranchCode,
                Description = entityBranch.Description,
                SmallDescription = entityBranch.SmallDescription
            };
        }

        public static List<Branch> CreateBranches(BusinessCollection businessCollection)
        {
            List<Branch> branches = new List<Branch>();

            foreach (COMMEN.Branch entity in businessCollection)
            {
                branches.Add(ModelAssembler.CreateBranch(entity));
            }

            return branches;
        }

        #endregion


        #region Prefix

        public static Prefix CreatePrefix(COMMEN.Prefix entityPrefix)
        {
            return new Prefix
            {
                Id = entityPrefix.PrefixCode,
                Description = entityPrefix.Description,
                SmallDescription = entityPrefix.SmallDescription,
                TinyDescription = entityPrefix.TinyDescription,
                PrefixType = new COMMML.PrefixType { Id = entityPrefix.PrefixTypeCode },
                PrefixTypeCode = entityPrefix.PrefixTypeCode
            };
        }

        public static List<Prefix> CreatePrefixes(BusinessCollection businessCollection)
        {
            List<Prefix> prefixes = new List<Prefix>();

            foreach (COMMEN.Prefix entity in businessCollection)
            {
                prefixes.Add(ModelAssembler.CreatePrefix(entity));
            }

            return prefixes;
        }

        public static Prefix CreatePrefix(COMMEN.PrefixUser entityPrefixUser)
        {
            return new Prefix
            {
                Id = entityPrefixUser.PrefixCode
            };
        }

        public static List<Prefix> CreateUserPrefixes(BusinessCollection businessCollection)
        {
            List<Prefix> prefixes = new List<Prefix>();

            foreach (COMMEN.PrefixUser entityPrefixUser in businessCollection)
            {
                prefixes.Add(CreatePrefix(entityPrefixUser));
            }

            return prefixes;
        }

        #endregion

        #region PrefixType

        public static COMMML.PrefixType CreatePrefixType(PARAMEN.PrefixType entityPrefixType)
        {
            return new COMMML.PrefixType
            {
                Id = entityPrefixType.PrefixTypeCode,
                Description = entityPrefixType.Description,
                SmallDescription = entityPrefixType.SmallDescription
            };
        }

        #endregion

        #region PrefixLineBusiness

        //public static PrefixLineBusiness CreatePrefixLineBusiness(COMMEN.PrefixLineBusiness entityPrefixLineBusiness)
        //{
        //    return new PrefixLineBusiness
        //    {
        //        PrefixCode = entityPrefixLineBusiness.PrefixCode,
        //        LineBusinessCode = entityPrefixLineBusiness.LineBusinessCode
        //    };
        //}

        //public static List<PrefixLineBusiness> CreatePrefixLinesBusiness(BusinessCollection businessCollection)
        //{
        //    List<PrefixLineBusiness> prefixesLineBusiness = new List<PrefixLineBusiness>();

        //    foreach (COMMEN.PrefixLineBusiness entity in businessCollection)
        //    {
        //        prefixesLineBusiness.Add(ModelAssembler.CreatePrefixLineBusiness(entity));
        //    }

        //    return prefixesLineBusiness;
        //}

        #endregion

        #region SalePoint

        public static SalePoint CreateSalePoint(COMMEN.SalePoint entitySalePoint, COMMEN.Branch entityBranch)
        {
            return new SalePoint
            {
                Id = (int)entitySalePoint.SalePointCode,
                Description = entitySalePoint.Description,
                SmallDescription = entitySalePoint.SmallDescription
            };
        }

        public static List<SalePoint> CreateSalePoints(BusinessCollection businessCollection)
        {
            List<SalePoint> salePoints = new List<SalePoint>();

            foreach (COMMEN.SalePoint entity in businessCollection)
            {
                salePoints.Add(ModelAssembler.CreateSalePoint(entity, null));
            }

            return salePoints;
        }

        public static List<SalePoint> CreateSalePoints(BusinessCollection businessCollection, BusinessCollection branchCollection)
        {
            List<SalePoint> salePoints = new List<SalePoint>();
            List<COMMEN.Branch> entityBranches = branchCollection.Cast<COMMEN.Branch>().ToList();

            foreach (COMMEN.SalePoint entity in businessCollection)
            {
                COMMEN.Branch entityBranch = entityBranches.Where(x => x.BranchCode == entity.BranchCode).FirstOrDefault();
                salePoints.Add(ModelAssembler.CreateSalePoint(entity, entityBranch));
            }

            return salePoints;
        }

        #endregion

        #region PolicyType

        public static PolicyType CreatePolicyType(COMMEN.CoPolicyType entityPolicyType)
        {
            return new PolicyType
            {
                Id = entityPolicyType.PolicyTypeCode,
                Description = entityPolicyType.Description,
                IsFloating = entityPolicyType.Floating,
                Prefix = new Prefix
                {
                    Id = entityPolicyType.PrefixCode
                }
            };
        }

        /// <summary>
        /// Creates the policy types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<PolicyType> CreatePolicyTypes(BusinessCollection businessCollection)
        {
            ConcurrentBag<PolicyType> policyTypes = new ConcurrentBag<PolicyType>();
            TP.Parallel.ForEach(businessCollection.Cast<COMMEN.CoPolicyType>().ToList(), entity =>
            {
                policyTypes.Add(ModelAssembler.CreatePolicyType(entity));
            });
            return policyTypes.ToList();
        }

        public static ProductPolicyType CreateProductPolicyType(PRODEN.CoProductPolicyType productpolicyType)
        {
            return new ProductPolicyType
            {
                Id = productpolicyType.PolicyTypeCode,
                ProductId = productpolicyType.ProductId,
                IsDefault = productpolicyType.IsDefault,
                Prefix = new Prefix { Id = productpolicyType.PrefixCode }
            };
        }

        /// <summary>
        /// Creates the product policy types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<ProductPolicyType> CreateProductPolicyTypes(BusinessCollection businessCollection)
        {
            ConcurrentBag<ProductPolicyType> productPolicyTypes = new ConcurrentBag<ProductPolicyType>();
            TP.Parallel.ForEach(businessCollection.Cast<PRODEN.CoProductPolicyType>().ToList(), entity =>
            {
                productPolicyTypes.Add(ModelAssembler.CreateProductPolicyType(entity));
            });
            return productPolicyTypes.ToList();
        }

        #endregion

        #region ExchangeRate

        public static ExchangeRate CreateExchangeRate(COMMEN.ExchangeRate entityExchangeRate)
        {
            return new ExchangeRate
            {
                RateDate = entityExchangeRate.RateDate,
                Currency = new Currency
                {
                    Id = entityExchangeRate.CurrencyCode
                },
                SellAmount = entityExchangeRate.SellAmount,
                BuyAmount = entityExchangeRate.BuyAmount.GetValueOrDefault()
            };
        }

        public static List<ExchangeRate> CreateExchangeRates(BusinessCollection businessCollection)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (COMMEN.ExchangeRate entity in businessCollection)
            {
                exchangeRates.Add(ModelAssembler.CreateExchangeRate(entity));
            }

            return exchangeRates;
        }

        #endregion
        
        #region DefaultValue

        private static DefaultValue CreateDefaultValue(PARAMEN.DefaultValue entityDefaultValue)
        {
            return new DefaultValue
            {
                ProfileId = entityDefaultValue.ProfileId,
                UserId = entityDefaultValue.UserId,
                ModuleId = entityDefaultValue.ModuleId,
                SubModuleId = entityDefaultValue.SubmoduleId,
                ViewName = entityDefaultValue.ViewName,
                ControlName = entityDefaultValue.ControlName,
                ControlValue = entityDefaultValue.ControlValue,
                ControlType = entityDefaultValue.ControlType
            };
        }

        public static List<DefaultValue> CreateDefaultValues(BusinessCollection businessCollection)
        {
            List<DefaultValue> defaultValues = new List<DefaultValue>();

            foreach (PARAMEN.DefaultValue entity in businessCollection)
            {
                defaultValues.Add(ModelAssembler.CreateDefaultValue(entity));
            }

            return defaultValues;
        }

        #endregion




        public static File CreateFile(COMMEN.File entityFile)
        {
            return new File()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                SmallDescription = entityFile.SmallDescription,
                Observations = entityFile.Observations,
                IsEnabled = entityFile.IsEnabled,
                FileType = (FileType)entityFile.FileTypeId,
                Templates = new List<Template>()

            };
        }

        public static FileProcessValue CreateFileProcessValue(BusinessCollection businessCollection)
        {
            FileProcessValue fileProcess = null;

            if (businessCollection.Count > 0)
            {
                foreach (COMMEN.FileProcessValue item in businessCollection)
                {
                    fileProcess = new FileProcessValue()
                    {
                        Id = item.Id,
                        FileId = item.FileId,
                        Key1 = item.Key1,
                        Key2 = item.Key2.HasValue ? item.Key2.Value : 0,
                        Key3 = item.Key3.HasValue ? item.Key3.Value : 0,
                        Key4 = item.Key4.HasValue ? item.Key4.Value : 0,
                        Key5 = item.Key5.HasValue ? item.Key5.Value : 0
                    };
                }
            }

            return fileProcess;
        }


        public static Field CreateField(COMMEN.FileTemplateField entityFileTemplateField, COMMEN.Field entityField)
        {
            return new Field
            {
                Id = entityField.Id,
                TemplateFieldId = entityFileTemplateField.Id,
                Description = entityFileTemplateField.Description,
                SmallDescription = entityField.SmallDescription,
                FieldType = (FieldType)entityField.FieldTypeId,
                IsEnabled = entityFileTemplateField.IsEnabled,
                IsMandatory = entityFileTemplateField.IsMandatory,
                Order = entityFileTemplateField.Order,
                ColumnSpan = entityFileTemplateField.ColumnSpan,
                RowPosition = entityFileTemplateField.RowPosition,
                PropertyName = entityField.PropertyName,
                PropertyLength = entityField.PropertyLength
            };
        }

        #region HardRiskType

        public static LineBusiness CreateHardRiskType(COMMEN.LineBusiness lineBusiness, PARAMEN.HardRiskType hardRiskType)
        {
            return new LineBusiness
            {
                Id = lineBusiness.LineBusinessCode,
                Description = lineBusiness.Description,
                ShortDescription = lineBusiness.SmallDescription,
                TyniDescription = lineBusiness.TinyDescription,
                ReportLineBusiness = int.Parse(lineBusiness.ReportLineBusinessCode),
                IdLineBusinessbyRiskType = (int)hardRiskType.SubCoveredRiskTypeCode
            };
        }

        public static List<LineBusiness> CreateLineBusinessByHardRiskType(BusinessCollection businessCollection)
        {
            var linebusiness = new List<LineBusiness>();
            foreach (PARAMEN.HardRiskType h in businessCollection)
            {
                linebusiness.Add(new LineBusiness
                {
                    Id = h.LineBusinessCode,
                    IdLineBusinessbyRiskType = (int)h.SubCoveredRiskTypeCode
                });
            }
            return linebusiness;
        }


        #endregion

        #region LineBusinessCoveredRiskType

        //public static List<LineBusinessCoveredRiskType> CreateLineBusinessCoveredRiskTypes(BusinessCollection businessCollection)
        //{
        //    var lineBusinessCoveredRiskTypes = new List<LineBusinessCoveredRiskType>();
        //    foreach (EtLineBusiness.LineBusinessCoveredRiskType lb in businessCollection)
        //    {
        //        lineBusinessCoveredRiskTypes.Add(CreateLineBusinessCoveredRiskType(lb));
        //    }
        //    return lineBusinessCoveredRiskTypes;
        //}

        //public static LineBusinessCoveredRiskType CreateLineBusinessCoveredRiskType(EtLineBusiness.LineBusinessCoveredRiskType lb)
        //{
        //    return new LineBusinessCoveredRiskType
        //    {
        //        IdLineBusiness = lb.LineBusinessCode,
        //        IdRiskType = lb.CoveredRiskTypeCode
        //    };
        //}
        #endregion


        #region ClaimNoticeType
        public static ClaimNoticeType CreateClaimNoticeType(PARAMEN.ClaimNoticeType claimNoticeTypeEntity)
        {
            return new ClaimNoticeType()
            {
                Id = claimNoticeTypeEntity.ClaimNoticeTypeId,
                Description = claimNoticeTypeEntity.Description
            };
        }

        public static List<ClaimNoticeType> CreateClaimNoticeTypes(BusinessCollection businessCollection)
        {
            List<ClaimNoticeType> claimNoticeType = new List<ClaimNoticeType>();
            foreach (PARAMEN.ClaimNoticeType claimNoticeTypeEntity in businessCollection)
            {
                claimNoticeType.Add(ModelAssembler.CreateClaimNoticeType(claimNoticeTypeEntity));
            }

            return claimNoticeType;
        }
        #endregion

        #region AccountType
        /// <summary>
        /// CreateAccountType
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns>BankAccountType</returns>
        public static AccountType CreateAccountType(COMMEN.AccountType accountType)
        {
            return new AccountType()
            {
                Id = accountType.AccountTypeCode,
                Description = accountType.Description,
                IsEnabled = Convert.ToBoolean(accountType.Enabled)
            };
        }

        /// <summary>
        /// CreateAccountTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BankAccountType/></returns>
        public static List<AccountType> CreateAccountTypes(BusinessCollection businessCollection)
        {
            List<AccountType> bankAccountTypes = new List<AccountType>();

            foreach (COMMEN.AccountType accountTypeEntity in businessCollection.OfType<COMMEN.AccountType>())
            {
                bankAccountTypes.Add(CreateAccountType(accountTypeEntity));
            }

            return bankAccountTypes;
        }
        #endregion AccountType
        #region ComponentType

        public static List<ComponentType> CreateComponentTypes(BusinessCollection businessCollection)
        {
            List<ComponentType> ComponentType = new List<ComponentType>();

            foreach (PARAMEN.ComponentType entity in businessCollection)
            {
                ComponentType.Add(ModelAssembler.CreateComponentType(entity));
            }

            return ComponentType;
        }

        public static ComponentType CreateComponentType(PARAMEN.ComponentType entityComponentType)
        {
            return new ComponentType
            {
                ComponentTypeId = entityComponentType.ComponentTypeCode,
                SmallDescription = entityComponentType.SmallDescription,
                TinyDescription = entityComponentType.TinyDescription,
            };
        }
        #endregion
    }
}