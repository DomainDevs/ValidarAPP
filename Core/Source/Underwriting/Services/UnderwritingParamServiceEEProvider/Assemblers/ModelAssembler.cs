// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers
{
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Quotation.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using COMMENUM = Sistran.Core.Application.CommonService.Enums;
    using COMMO = Sistran.Core.Application.CommonParamService.Models;
    using EntRules = Sistran.Core.Application.Script.Entities;
    using ENUM = Sistran.Core.Application.UnderwritingParamService.Enums;
    using INTEN = Sistran.Core.Application.Integration.Entities;
    using MODEN = ModelServices.Enums;
    using PAREN = Sistran.Core.Application.Parameters.Entities;
    using PAYM = Sistran.Core.Application.Parameters.Entities;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using REQEN = Sistran.Core.Application.Request.Entities;
    using PAYMM = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using TAXEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UNDEN = Sistran.Core.Application.UnderwritingServices.Enums;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using MOLUNPARAM = Sistran.Core.Application.UnderwritingParamService.Models;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsCommon = Sistran.Core.Application.CommonService.Models;

    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public static class ModelAssembler
    {

        /// <summary>
        /// Mapeo de la entidad CoveredRiskType al modelo de negocio ParamCoveredRiskType.
        /// </summary>
        /// <param name="coveredRiskType">Entifdad de tipo CoveredRiskType.</param>
        /// <returns>Modelo de negocio ParamCoveredRiskType.</returns>
        public static Result<ParamCoveredRiskType, ErrorModel> CreateCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            Result<ParamCoveredRiskType, ErrorModel> result = ParamCoveredRiskType.GetParamCoveredRiskType(coveredRiskType.CoveredRiskTypeCode, coveredRiskType.SmallDescription);
            return result;
        }
        
        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo InsuredProfile
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos InsuredProfile</returns>
        public static Result<List<ParamCoveredRiskType>, ErrorModel> CreateCoveredRiskTypes(BusinessCollection businessCollection)
        {
            List<ParamCoveredRiskType> coveredRiskTypes = new List<ParamCoveredRiskType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCoveredRiskType, ErrorModel> result;
            foreach (CoveredRiskType entityCoveredRiskType in businessCollection)
            {
                result = CreateCoveredRiskType(entityCoveredRiskType);
                if (result is ResultError<ParamCoveredRiskType, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeMappingEntityError);
                    return new ResultError<List<ParamCoveredRiskType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamCoveredRiskType resultValue = (result as ResultValue<ParamCoveredRiskType, ErrorModel>).Value;
                    coveredRiskTypes.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamCoveredRiskType>, ErrorModel>(coveredRiskTypes);
        }

        #region ParametrizationPaymentPlan
        /// <summary>
        /// Construye plan de pago (Entidad=>Modelo negocio)
        /// </summary>
        /// <param name="paymentShedule">Plan de pago-Entidad</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static ParametrizationPaymentPlan CreateParametrizationPaymentPlan(PRODEN.PaymentSchedule paymentShedule)
        {
            ParametrizationPaymentPlan paymentPlan = new ParametrizationPaymentPlan
            {
                Id = paymentShedule.PaymentScheduleId,
                Description = paymentShedule.Description,
                SmallDescription = paymentShedule.SmallDescription,
                Quantity = Convert.ToInt32(paymentShedule.PaymentQuantity),
                FirstPayQuantity = Convert.ToInt32(paymentShedule.FirstPayQuantity),
                LastPayQuantity = paymentShedule.LastPayQuantity,
                IsIssueDate = paymentShedule.IsIssueDate,
                IsGreaterDate = paymentShedule.IsGreaterDate,
                GapUnit = paymentShedule.GapUnitCode,
                GapQuantity = paymentShedule.GapQuantity,
                ParametrizationQuotas = new List<ParametrizationQuota>(),
                Financing = paymentShedule.Financing

            };
            return paymentPlan;
        }

        /// <summary>
        /// Retorna listado de plan de pago(negocio) a partir de BusinessCollection
        /// </summary>
        /// <param name="collection">Collecion de entidad PRODEN.PaymentSchedule</param>
        /// <returns>Listado de plan de pago MOD-B</returns>
        public static List<ParametrizationPaymentPlan> CreateParametrizationPaymentPlans(BusinessCollection collection)
        {
            List<ParametrizationPaymentPlan> paymentPlans = new List<ParametrizationPaymentPlan>();
            foreach (PRODEN.PaymentSchedule item in collection)
            {
                paymentPlans.Add(CreateParametrizationPaymentPlan(item));
            }

            return paymentPlans;
        }
        #endregion

        #region Quota

        /// <summary>
        /// Retorna MOD-BUSS del listado de cuotas a partir del BusinessCollection
        /// </summary>
        /// <param name="businessCollection">Coleccion de Cuotas(PaymentDistribution)</param>
        /// <returns>Listado de cuotas MOD-B</returns>
        public static List<ParametrizationQuota> CreateQuotas(BusinessCollection businessCollection)
        {
            List<ParametrizationQuota> quotas = new List<ParametrizationQuota>();

            foreach (PRODEN.PaymentDistribution item in businessCollection)
            {
                quotas.Add(ModelAssembler.CreateParametrizationQuota(item));
            }

            return quotas;
        }

        /// <summary>
        /// Convierte ENTIDAD de distribuccion de cuota a => MOD-B 
        /// </summary>
        /// <param name="paymentDistribution">Entidad Cuota-PaymentDistribution </param>
        /// <returns>item Cuota MOD-B</returns>
        public static ParametrizationQuota CreateParametrizationQuota(PRODEN.PaymentDistribution paymentDistribution)
        {
            
            return new ParametrizationQuota
            {
                Id = paymentDistribution.PaymentScheduleId,
                Number = paymentDistribution.PaymentNumber,
                Percentage = Convert.ToDecimal(paymentDistribution.PaymentPercentage),
                GapQuantity = paymentDistribution.GapQuantity
            };
        }
        /// <summary>
        /// Convierte ENTIDAD de distribuccion de cuota a => MOD-B 
        /// </summary>
        /// <param name="paymentDistribution">Entidad Cuota-PaymentDistribution </param>
        /// <returns>item Cuota MOD-B</returns>
        public static ParametrizationQuota CreateParametrizationQuotaAndDistributionComponent(PRODEN.PaymentDistribution paymentDistribution, List<ParametrizacionQuotaTypeComponent> parametrizacionQuotaTypeComponents)
        {
            ParametrizationQuota parametrizationQuota = new ParametrizationQuota();
            parametrizationQuota.Id = paymentDistribution.PaymentScheduleId;
            parametrizationQuota.Number = paymentDistribution.PaymentNumber;
            parametrizationQuota.Percentage = Convert.ToDecimal(paymentDistribution.PaymentPercentage);
            parametrizationQuota.GapQuantity = paymentDistribution.GapQuantity;
            parametrizationQuota.ListQuotaComponent = new List<ParametrizacionQuotaTypeComponent>();
            if (parametrizacionQuotaTypeComponents != null)
            {
                foreach (ParametrizacionQuotaTypeComponent item in parametrizacionQuotaTypeComponents)
                {
                    ParametrizacionQuotaTypeComponent objectParametrizationQuotaComponent = new ParametrizacionQuotaTypeComponent();
                    objectParametrizationQuotaComponent.Id = item.Id;
                    objectParametrizationQuotaComponent.PaymentNumber = item.PaymentNumber;
                    objectParametrizationQuotaComponent.PaymentScheduleId = item.PaymentScheduleId;
                    objectParametrizationQuotaComponent.Value = item.Value;
                    parametrizationQuota.ListQuotaComponent.Add(objectParametrizationQuotaComponent);
                }
            }

            return parametrizationQuota;
        }


        /// <summary>
        /// Retorna MOD-BUSS del listado de cuotas a partir del BusinessCollection
        /// </summary>
        /// <param name="businessCollection">Coleccion de Cuotas(PaymentDistribution)</param>
        /// <returns>Listado de cuotas MOD-B</returns>
        public static List<ParametrizacionQuotaTypeComponent> CreateQuotasComponent(BusinessCollection businessCollection)
        {
            List<ParametrizacionQuotaTypeComponent> quotas = new List<ParametrizacionQuotaTypeComponent>();

            foreach (PRODEN.CoPaymentDistributionComponent item in businessCollection)
            {
                quotas.Add(ModelAssembler.CreateParametrizationQuotaComponent(item));
            }

            return quotas;
        }

        public static ParametrizacionQuotaTypeComponent CreateParametrizationQuotaComponent(PRODEN.CoPaymentDistributionComponent paymentDistributionComponent)
        {
            return new ParametrizacionQuotaTypeComponent
            {
                PaymentScheduleId = paymentDistributionComponent.PaymentScheduleId,
                PaymentNumber = paymentDistributionComponent.PaymentNumber,
                Id = paymentDistributionComponent.ComponentCode,
                Value = Convert.ToInt32(paymentDistributionComponent.Value),
               
            };
        }


        #endregion Quota

        /// <summary>
        /// Convierte entidad a modelo
        /// </summary>
        /// <param name="entityFile">Entidad File</param>
        /// <returns>Modelo File</returns>
        public static File CreateFile(COMMEN.File entityFile)
        {
            return new File()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                SmallDescription = entityFile.SmallDescription,
                Observations = entityFile.Observations,
                IsEnabled = entityFile.IsEnabled,
                FileType = (UTILEN.FileType)entityFile.FileTypeId,
                Templates = new List<Template>()
            };
        }

        /// <summary>
        /// Convierte a Modelo File
        /// </summary>
        /// <param name="businessCollection">Colección File</param>
        /// <returns>Modelo File</returns>
        public static List<File> CreateFiles(BusinessCollection businessCollection)
        {
            List<File> files = new List<File>();

            foreach (COMMEN.File entity in businessCollection)
            {
                files.Add(ModelAssembler.CreateFile(entity));
            }

            return files;
        }

        /// <summary>
        /// Convierte a Modelo FileProcessValue
        /// </summary>
        /// <param name="businessCollection">Coleccion FileProcess</param>
        /// <returns>Modelo FileProcessValue</returns>
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

        /// <summary>
        /// Llena los campos
        /// </summary>
        /// <param name="entityFile">Modelo Field</param>
        /// <returns>Propiedades de los campos</returns>
        public static Field CreateField(COMMEN.Field entityFile)
        {
            return new Field()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                SmallDescription = entityFile.SmallDescription,
                FieldType = (UTILEN.FieldType)entityFile.FieldTypeId,
                PropertyName = entityFile.PropertyName,
                PropertyLength = entityFile.PropertyLength
            };
        }

        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityFileTemplateField">Modelo FileTemplateField</param>
        /// <param name="entityField">Modelo Field</param>
        /// <returns>Datos de los campos</returns>
        public static Field CreateField(COMMEN.FileTemplateField entityFileTemplateField, COMMEN.Field entityField)
        {
            return new Field
            {
                Id = entityField.Id,
                TemplateFieldId = entityFileTemplateField.Id,
                Description = entityFileTemplateField.Description,
                SmallDescription = entityField.SmallDescription,
                FieldType = (UTILEN.FieldType)entityField.FieldTypeId,
                IsEnabled = entityFileTemplateField.IsEnabled,
                IsMandatory = entityFileTemplateField.IsMandatory,
                Order = entityFileTemplateField.Order,
                ColumnSpan = entityFileTemplateField.ColumnSpan,
                RowPosition = entityFileTemplateField.RowPosition,
                PropertyName = entityField.PropertyName,
                PropertyLength = entityField.PropertyLength
            };
        }

        /// <summary>
        /// Convierte de entidad a ParamLineBusinessModel
        /// </summary>
        /// <param name="lineBusiness">Entidad linebusiness</param>
        /// <returns>Modelo ParamLineBusinessModel</returns>
        public static Result<ParamLineBusinessModel, ErrorModel> CreateLineBusiness(COMMEN.LineBusiness lineBusiness)
        {
            return ParamLineBusinessModel.GetParamLineBusinessModel(lineBusiness.LineBusinessCode, lineBusiness.Description, lineBusiness.SmallDescription, lineBusiness.TinyDescription);
        }

        /// <summary>
        /// Convierte de businesscollection a ParamLineBusinessModel
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo ParamLineBusinessModel</returns>
        public static Result<List<ParamLineBusinessModel>, ErrorModel> CreateLinesBusiness(BusinessCollection businessCollection)
        {
            List<ParamLineBusinessModel> lstLineBusiness = new List<ParamLineBusinessModel>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamLineBusinessModel, ErrorModel> result;
            foreach (COMMEN.LineBusiness lineBusiness in businessCollection)
            {
                result = CreateLineBusiness(lineBusiness);
                if (result is ResultError<ParamLineBusinessModel, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                    return new ResultError<List<ParamLineBusinessModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamLineBusinessModel resultValue = (result as ResultValue<ParamLineBusinessModel, ErrorModel>).Value;
                    lstLineBusiness.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamLineBusinessModel>, ErrorModel>(lstLineBusiness);
        }

        /// <summary>
        /// Convierte de businesscollection a Currency
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo Currency</returns>
        public static List<Currency> CreateCurrencies(BusinessCollection businessCollection)
        {
            List<Currency> currencies = new List<Currency>();

            foreach (COMMEN.Currency entity in businessCollection)
            {
                currencies.Add(ModelAssembler.CreateCurrency(entity));
            }

            return currencies;
        }

        /// <summary>
        /// Convierte de entidad a Currency
        /// </summary>
        /// <param name="entityCurrency">Entidad currency</param>
        /// <returns>Modelo Currency</returns>
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

        /// <summary>
        /// Convierte de entidad a Deductible
        /// </summary>
        /// <param name="deductible">Entidad Deductible</param>
        /// <returns>Modelo Deductible</returns>
        public static UNDMO.Deductible CreateDeductible(QUOEN.Deductible deductible)
        {
            return new UNDMO.Deductible
            {
                Id = deductible.DeductId,
                Rate = deductible.Rate,
                RateType = (TAXEN.RateType)deductible.RateTypeCode,
                DeductValue = deductible.DeductValue,
                DeductibleUnit = new UNDMO.DeductibleUnit
                {
                    Id = deductible.DeductUnitCode
                },
                MinDeductValue = deductible.MinDeductValue,
                MinDeductibleUnit = new UNDMO.DeductibleUnit
                {
                    Id = deductible.MinDeductUnitCode.GetValueOrDefault()
                },
                MinDeductibleSubject = new UNDMO.DeductibleSubject
                {
                    Id = deductible.MinDeductSubjectCode.GetValueOrDefault()
                },
                MaxDeductValue = deductible.MaxDeductValue,
                MaxDeductibleUnit = new UNDMO.DeductibleUnit
                {
                    Id = deductible.MaxDeductUnitCode.GetValueOrDefault()
                },
                MaxDeductibleSubject = new UNDMO.DeductibleSubject
                {
                    Id = deductible.MaxDeductSubjectCode.GetValueOrDefault()
                },
                DeductibleSubject = new UNDMO.DeductibleSubject
                {
                    Id = deductible.DeductSubjectCode.GetValueOrDefault()
                },
                Currency = new Currency
                {
                    Id = deductible.CurrencyCode.GetValueOrDefault()
                },
                LineBusiness = new LineBusiness
                {
                    Id = deductible.LineBusinessCode
                },
                Description = deductible.Description
            };
        }

        /// <summary>
        /// Convierte de businesscollection a Deductible
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo Deductible</returns>
        public static List<UNDMO.Deductible> CreateDeductibles(BusinessCollection businessCollection)
        {
            List<UNDMO.Deductible> deductibles = new List<UNDMO.Deductible>();

            foreach (QUOEN.Deductible field in businessCollection)
            {
                deductibles.Add(ModelAssembler.CreateDeductible(field));
            }

            return deductibles;
        }


        /// <summary>
        /// Convierte de businesscollection a DeductibleSubject
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo DeductibleSubject</returns>
        public static List<UNDMO.DeductibleSubject> CreateDeductibleSubjects(BusinessCollection businessCollection)
        {
            List<UNDMO.DeductibleSubject> deductibles = new List<UNDMO.DeductibleSubject>();

            foreach (PAREN.DeductibleSubject entity in businessCollection)
            {
                deductibles.Add(ModelAssembler.CreateDeductibleSubject(entity));
            }

            return deductibles;
        }

        /// <summary>
        /// Convierte de entidad a DeductibleSubject
        /// </summary>
        /// <param name="entitydeduct">Entidad deductiblesubject</param>
        /// <returns>Modelo DeductibleSubject</returns>    
        private static UNDMO.DeductibleSubject CreateDeductibleSubject(PAREN.DeductibleSubject entitydeduct)
        {
            return new UNDMO.DeductibleSubject()
            {
                Id = entitydeduct.DeductSubjectCode,
                Description = entitydeduct.Description
            };
        }

        /// <summary>
        /// Convierte de businesscollection a DeductibleUnit
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo DeductibleUnit</returns>
        public static List<UNDMO.DeductibleUnit> CreateDeductibleUnits(BusinessCollection businessCollection)
        {
            List<UNDMO.DeductibleUnit> deductibles = new List<UNDMO.DeductibleUnit>();

            foreach (PAREN.DeductibleUnit entity in businessCollection)
            {
                deductibles.Add(ModelAssembler.CreateDeductibleUnit(entity));
            }

            return deductibles;
        }

        /// <summary>
        /// Convierte de entidad a DeductibleUnit
        /// </summary>
        /// <param name="entitydeduct">Entidad deductibleunit</param>
        /// <returns>Modelo DeductibleUnit</returns>
        private static UNDMO.DeductibleUnit CreateDeductibleUnit(PAREN.DeductibleUnit entitydeduct)
        {
            return new UNDMO.DeductibleUnit()
            {
                Id = entitydeduct.DeductUnitCode,
                Description = entitydeduct.Description,
                HasSubject = entitydeduct.HasSubject
            };

        }

        #region surcharge
        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="component">componetes entidad</param>
        /// <param name="surchargeComponent">recargo entidad</param>
        /// <param name="rateType">tipo de tasa</param>
        /// <returns> modelo de negocio </returns>
        public static ParamSurcharge CreateSurcharge(QUOEN.Component component, QUOEN.SurchargeComponent surchargeComponent, PAREN.RateType rateType)
        {
            return new ParamSurcharge
            {
                Rate = surchargeComponent.Rate,
                Description = component.SmallDescription,
                Type = (MODEN.RateType)surchargeComponent.RateTypeCode,
                Id = component.ComponentCode,
                TinyDescription = component.TinyDescription
            };
        }

        /// <summary>
        /// Lista los componente
        /// </summary>
        /// <param name="component">tipo de tasa</param>
        /// <returns> retorna el modelo de componentes </returns>
        public static UNDMO.Component CreateComponent(QUOEN.Component component)
        {
            return new UNDMO.Component
            {
                Description = component.SmallDescription,
                Id = component.ComponentCode,

            };
        }

        public static ParamSurcharge CreateComponentSurcharge(QUOEN.Component component)
        {
            return new ParamSurcharge
            {
                Description = component.SmallDescription,
                Id = component.ComponentCode,
                TinyDescription = component.TinyDescription
            };
        }
        #endregion

        #region PlanFinanciero
        /// <summary>
        /// Convierte de entidad a ParamPaymentMethod
        /// </summary>
        /// <param name="paymentMethod">Entidad paymentMethod</param>
        /// <returns>Modelo LineBusiness</returns>
        public static ParamPaymentMethod CreatePaymentMethod(COMMEN.PaymentMethod paymentMethod)
        {
            return new ParamPaymentMethod
            {
                Id = paymentMethod.PaymentMethodCode,
                Description = paymentMethod.Description,
                IsInCome = paymentMethod.IsIncome
            };
        }

        /// <summary>
        /// Convierte de businesscollection a LineBusiness
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo LineBusiness</returns>
        public static List<ParamPaymentMethod> CreatePaymentsMethod(BusinessCollection businessCollection)
        {
            List<ParamPaymentMethod> paymentMethod = new List<ParamPaymentMethod>();

            foreach (COMMEN.PaymentMethod entity in businessCollection)
            {
                paymentMethod.Add(ModelAssembler.CreatePaymentMethod(entity));
            }

            return paymentMethod;
        }

        /// <summary>
        /// Convierte de entidad a ParamPaymentMethod
        /// </summary>
        /// <param name="paymentMethod">Entidad paymentMethod</param>
        /// <returns>Modelo LineBusiness</returns>
        public static UNDMO.Component CreateComponentRelation(QUOEN.Component component)
        {
            return new UNDMO.Component
            {
                Id = component.ComponentCode,
                Description = component.SmallDescription,
                ComponentType = UnderwritingServices.Enums.ComponentType.Premium,
                ComponentClass = UNDEN.ComponentClassType.HardComponent
            };
        }

        /// <summary>
        /// Convierte de businesscollection a LineBusiness
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo LineBusiness</returns>
        public static List<UNDMO.Component> CreateComponentRelations(BusinessCollection businessCollection)
        {
            List<UNDMO.Component> componentRelation = new List<UNDMO.Component>();

            foreach (QUOEN.Component entity in businessCollection)
            {
                componentRelation.Add(ModelAssembler.CreateComponentRelation(entity));
            }

            return componentRelation;
        }

        public static ParamFinancialPlanComponent CreateParamFinancialPlan(PRODEN.FinancialPlan financialPlan)
        {
            return new ParamFinancialPlanComponent()
            {
                ParamFinancialPlan = new ParamFinancialPlan()
                {
                    Id = financialPlan.FinancialPlanId,
                    MinQuota = 0,
                    ParametrizationPaymentPlan = new ParametrizationPaymentPlan()
                    {
                        Id = financialPlan.PaymentScheduleId
                    },
                    ParamPaymentMethod = new ParamPaymentMethod()
                    {
                        Id = financialPlan.PaymentMethodCode
                    },
                    ParamCurrency = new ParamCurrency()
                    {
                        Id = financialPlan.CurrencyCode
                    },

                }

            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio
        /// </summary>
        /// <param name="businessCollection">collecion Coverage</param>
        /// <returns>modelo de negocio Coverage</returns>
        public static List<ParamFinancialPlanComponent> CreateParamFinancialPlans(BusinessCollection businessCollection)
        {
            List<ParamFinancialPlanComponent> paramFinancial = new List<ParamFinancialPlanComponent>();
            foreach (PRODEN.FinancialPlan item in businessCollection)
            {
                paramFinancial.Add(CreateParamFinancialPlan(item));
            }

            return paramFinancial;
        }

        /// <summary>
        /// Convierte de entidad a ParamPaymentMethod
        /// </summary>
        /// <param name="paymentMethod">Entidad paymentMethod</param>
        /// <returns>Modelo LineBusiness</returns>
        public static ParamFirstPayComponent CreateFirstPayComponent(PRODEN.FirstPayComponent component)
        {
            return new ParamFirstPayComponent
            {
                IdComponent = component.ComponentCode,
                IdFinancialPlan = component.FinancialPlanId
            };
        }

        /// <summary>
        /// Convierte de businesscollection a LineBusiness
        /// </summary>
        /// <param name="businessCollection">Coleccion de negocio</param>
        /// <returns>Modelo LineBusiness</returns>
        public static List<ParamFirstPayComponent> CreateFirstPayComponents(BusinessCollection businessCollection)
        {
            List<ParamFirstPayComponent> componentRelation = new List<ParamFirstPayComponent>();

            foreach (PRODEN.FirstPayComponent entity in businessCollection)
            {
                componentRelation.Add(ModelAssembler.CreateFirstPayComponent(entity));
            }

            return componentRelation;
        }
        #endregion

        #region Descuentos 
        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="component">parametro de componetes</param>
        /// <param name="discountComponent">recargo componente</param>
        /// <param name="rateType">tipo de tasa</param>
        /// <returns> modelo de negocio </returns>
        public static ParamDiscount CreateDiscount(QUOEN.Component component, QUOEN.DiscountComponent discountComponent, PAREN.RateType rateType)
        {
            return new ParamDiscount
            {
                Rate = discountComponent.Rate,
                Description = component.SmallDescription,
                Type = (MODEN.RateType)discountComponent.RateTypeCode,
                Id = component.ComponentCode,
                TinyDescription = component.TinyDescription
            };
        }

        #endregion
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
        #region DetailType

        /// <summary>
        /// Retorna MOD-BUSS del listado de cuotas a partir del BusinessCollection
        /// </summary>
        /// <param name="businessCollection">Coleccion de Cuotas(PaymentDistribution)</param>
        /// <returns>Listado de cuotas MOD-B</returns>
        public static List<MOLUNPARAM.DetailType> CreateDetailTypes(BusinessCollection businessCollection)
        {
            List<MOLUNPARAM.DetailType> detailTypes = new List<MOLUNPARAM.DetailType>();

            foreach (QUOEN.DetailType item in businessCollection)
            {
                detailTypes.Add(ModelAssembler.CreateDetailType(item));
            }

            return detailTypes;
        }

        /// <summary>
        /// Convierte entidad a modelo
        /// </summary>
        /// <param name="detailType">Entidad DetailType</param>
        /// <returns>Modelo DetailType</returns>
        public static MOLUNPARAM.DetailType CreateDetailType(QUOEN.DetailType detailType)
        {
            return new MOLUNPARAM.DetailType
            {
                Description = detailType.Description,
                DetailClassCode = detailType.DetailClassCode,
                DetailTypeCode = detailType.DetailTypeCode,
                SmallDescription = detailType.SmallDescription
            };
        }
        #endregion DetailType

        #region ParametrizationDetail
        /// <summary>
        /// Construye plan de pago (Entidad=>Modelo negocio)
        /// </summary>
        /// <param name="paymentShedule">Plan de pago-Entidad</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static ParametrizationDetail CreateParametrizationDetail(QUOEN.Detail detail)
        {
            ParametrizationDetail parametrizationDetail = new ParametrizationDetail
            {
                Id = detail.DetailId,
                Description = detail.Description,
                DetailType = new ParametrizationDetailType { Id = detail.DetailTypeCode },
                Enabled = detail.Enabled,
                Rate = detail.Rate,
                RateType = detail.RateTypeCode == null ? 0 : (MODEN.RateType)detail.RateTypeCode,
                SublimitAmt = detail.SublimitAmount == null ? default(decimal?) : detail.SublimitAmount
            };
            return parametrizationDetail;
        }

        /// <summary>
        /// Retorna listado de plan de pago(negocio) a partir de BusinessCollection
        /// </summary>
        /// <param name="collection">Collecion de entidad PRODEN.PaymentSchedule</param>
        /// <returns>Listado de plan de pago MOD-B</returns>
        public static List<ParametrizationDetail> CreateParametrizationDetails(BusinessCollection collection)
        {
            List<ParametrizationDetail> parametrizationDetail = new List<ParametrizationDetail>();
            foreach (QUOEN.Detail item in collection)
            {
                parametrizationDetail.Add(CreateParametrizationDetail(item));
            }

            return parametrizationDetail;
        }
        #endregion

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y carrocerias
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculos</param>
        /// <param name="vehicleBodies">Tipo de carrocerias</param>
        /// <returns>Modelo de tipo de vehiculos y carrocerias</returns>
        public static Result<ParamVehicleTypeBody, ErrorModel> CreateVehicleTypeBody(COMMEN.VehicleType vehicleType, BusinessCollection vehicleBodies)
        {
            List<string> errorModelListDescription = new List<string>();

            Result<ParamVehicleType, ErrorModel> resultVehicleType = CreateVehicleType(vehicleType);
            if (resultVehicleType is ResultError<ParamVehicleType, ErrorModel>)
            {
                return new ResultError<ParamVehicleTypeBody, ErrorModel>(((ResultError<ParamVehicleType, ErrorModel>)resultVehicleType).Message);
            }

            Result<List<ParamVehicleBody>, ErrorModel> resultVehicleBodies = CreateVehicleBody(vehicleBodies);
            if (resultVehicleBodies is ResultError<List<ParamVehicleBody>, ErrorModel>)
            {
                return new ResultError<ParamVehicleTypeBody, ErrorModel>(((ResultError<List<ParamVehicleBody>, ErrorModel>)resultVehicleBodies).Message);
            }

            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)resultVehicleType).Value;
            List<ParamVehicleBody> paramVehicleBodies = ((ResultValue<List<ParamVehicleBody>, ErrorModel>)resultVehicleBodies).Value;

            return ParamVehicleTypeBody.CreateParamVehicleBody(paramVehicleType, paramVehicleBodies);
        }

        /// <summary>
        /// Mapeo de la entidad VehicleBody al modelo ParamVehicleBody
        /// </summary>
        /// <param name="vehicleBody">Entidad VehicleBody</param>
        /// <returns>Modelo ParamVehicleBody</returns>
        public static Result<ParamVehicleBody, ErrorModel> CreateVehicleBody(COMMEN.VehicleBody vehicleBody)
        {
            Result<ParamVehicleBody, ErrorModel> result = ParamVehicleBody.CreateParamVehicleBody(vehicleBody.VehicleBodyCode, vehicleBody.SmallDescription);
            return result;
        }

        public static Result<List<ParamVehicleBody>, ErrorModel> CreateVehicleBody(BusinessCollection businessCollection)
        {
            List<ParamVehicleBody> vehicleTypes = new List<ParamVehicleBody>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleBody, ErrorModel> result;
            foreach (COMMEN.VehicleBody entityVehicleBody in businessCollection)
            {
                result = CreateVehicleBody(entityVehicleBody);
                if (result is ResultError<ParamVehicleBody, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad carroceria cubierto a modelo de negocio.");
                    return new ResultError<List<ParamVehicleBody>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleBody resultValue = (result as ResultValue<ParamVehicleBody, ErrorModel>).Value;
                    vehicleTypes.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamVehicleBody>, ErrorModel>(vehicleTypes);
        }

        /// <summary>
        /// Mapeo de la entidad VehicleType al modelo ParamVehicleType
        /// </summary>
        /// <param name="vehicleType">Entidad VehicleType</param>
        /// <returns>Modelo ParamVehicleType</returns>
        public static Result<ParamVehicleType, ErrorModel> CreateVehicleType(COMMEN.VehicleType vehicleType)
        {
            Result<ParamVehicleType, ErrorModel> result = ParamVehicleType.CreateParamVehicleType(vehicleType.VehicleTypeCode, vehicleType.Description, vehicleType.SmallDescription, vehicleType.Enabled, vehicleType.IsTruck);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a la lista de modelo ParamVehicleType
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection con la entidad VehicleType</param>
        /// <returns>Lista de modelos VehicleType</returns>
        public static Result<List<ParamVehicleType>, ErrorModel> CreateVehicleTypes(BusinessCollection businessCollection)
        {
            List<ParamVehicleType> vehicleTypes = new List<ParamVehicleType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleType, ErrorModel> result;
            foreach (COMMEN.VehicleType entityVehicleType in businessCollection)
            {
                result = CreateVehicleType(entityVehicleType);
                if (result is ResultError<ParamVehicleType, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad tipo de vehiculo cubierto a modelo de negocio.");
                    return new ResultError<List<ParamVehicleType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleType resultValue = (result as ResultValue<ParamVehicleType, ErrorModel>).Value;
                    vehicleTypes.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamVehicleType>, ErrorModel>(vehicleTypes);
        }

        #region ParamPeril
        /// <summary>
        /// Convierte entidad a modelo de negocio amparo 
        /// </summary>
        /// <param name="peril">Amparo ENT</param>
        /// <returns>Amparo MOD-B</returns>
        public static ParamPeril CreateParamPeril(QUOEN.Peril peril)
        {
            return new ParamPeril()
            {
                Id = peril.PerilCode,
                Description = peril.Description,
                SmallDescription = peril.SmallDescription
            };
        }
        #endregion

        #region ParamInsuredObjectDesc
        /// <summary>
        /// Convierte entidad a modelo de negocio objeto del seguro
        /// </summary>
        /// <param name="insuredObject">Objeto del seguro => ENT</param>
        /// <returns>objeto del seguro => MOD-B</returns>
        public static ParamInsuredObjectDesc CreateParamInsuredObjectDesc(QUOEN.InsuredObject insuredObject)
        {
            return new ParamInsuredObjectDesc
            {
                Id = insuredObject.InsuredObjectId,
                Description = insuredObject.Description
            };
        }

        /// <summary>
        /// Convierte listado entidad a modelo de negocio objetos del seguro
        /// </summary>
        /// <param name="businessCollection">coleccion objetos del seguro => ENT</param>
        /// <returns>Listado objetos del seguro=> MOD-B</returns>
        public static List<ParamInsuredObjectDesc> CreateParamInsuredObjectDescs(BusinessCollection businessCollection)
        {
            List<ParamInsuredObjectDesc> paramInsuredObjectDescs = new List<ParamInsuredObjectDesc>();
            foreach (QUOEN.InsuredObject item in businessCollection)
            {
                paramInsuredObjectDescs.Add(CreateParamInsuredObjectDesc(item));
            }

            return paramInsuredObjectDescs;
        }

        #endregion



        #region ParamClauseDesc
        /// <summary>
        /// Convierte entidad a modelo ClauseLevel
        /// </summary>
        /// <param name="clauseLevel">entidad ClauseLevel</param>
        /// <returns>modelo ParamClauseDesc</returns>
        public static ParamClauseDesc CreateParamClauseDesc(QUOEN.ClauseLevel clauseLevel)
        {
            return new ParamClauseDesc()
            {
                Id = clauseLevel.ClauseId,
                IsMandatory = clauseLevel.IsMandatory
            };
        }

        /// <summary>
        /// Convierte listado entidad a modelo ParamClauseDesc
        /// </summary>
        /// <param name="businessCollection">coleccion ClauseLevel</param>
        /// <returns>modelo ParamClauseDesc</returns>
        public static List<ParamClauseDesc> CreateParamClauseItems(BusinessCollection businessCollection)
        {
            List<ParamClauseDesc> paramClauseDescs = new List<ParamClauseDesc>();
            foreach (QUOEN.ClauseLevel item in businessCollection)
            {
                paramClauseDescs.Add(CreateParamClauseDesc(item));
            }

            return paramClauseDescs;
        }

        /// <summary>
        /// Convierte entidad a modelo negocio Clause
        /// </summary>
        /// <param name="clause">entidad Clause</param>
        /// <returns>modelo Clause</returns>
        public static ParamClauseDesc CreateParamClauseDesc(QUOEN.Clause clause)
        {
            return new ParamClauseDesc()
            {
                Id = clause.ClauseId,
                Description = clause.ClauseName
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio Clause
        /// </summary>
        /// <param name="businessCollection">collecion Clause</param>
        /// <returns>modelo de negocio Clause</returns>
        public static List<ParamClauseDesc> CreateParamClauseDescs(BusinessCollection businessCollection)
        {
            List<ParamClauseDesc> paramClauseDescs = new List<ParamClauseDesc>();
            foreach (QUOEN.Clause item in businessCollection)
            {
                paramClauseDescs.Add(CreateParamClauseDesc(item));
            }

            return paramClauseDescs;
        }
        #endregion

        #region ParamDeductibleDesc
        /// <summary>
        /// Convierte entidad a modelo negocio CoverageDeductible
        /// </summary>
        /// <param name="coverageDeductible">entidad CoverageDeductible</param>
        /// <returns>modelo de negocio CoverageDeductible</returns>
        public static ParamDeductibleDesc CreateParamDeductibleDesc(QUOEN.CoverageDeductible coverageDeductible)
        {
            return new ParamDeductibleDesc()
            {
                Id = coverageDeductible.DeductId,
                IsMandatory = coverageDeductible.IsDefault
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio Deductible
        /// </summary>
        /// <param name="deductible">entidad Deductible</param>
        /// <returns>modelo de negocio Deductible</returns>
        public static ParamDeductibleDesc CreateParamDeductibleDesc(QUOEN.Deductible deductible)
        {
            return new ParamDeductibleDesc()
            {
                Id = deductible.DeductId,
                Description = deductible.Description
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio Deductible
        /// </summary>
        /// <param name="businessCollection">coleccion Deductible</param>
        /// <returns>modelo de negocio Deductible</returns>
        public static List<ParamDeductibleDesc> CreateParamDeductibleDescs(BusinessCollection businessCollection)
        {
            List<ParamDeductibleDesc> paramDeductibleDescs = new List<ParamDeductibleDesc>();
            foreach (QUOEN.Deductible item in businessCollection)
            {
                paramDeductibleDescs.Add(CreateParamDeductibleDesc(item));
            }

            return paramDeductibleDescs;
        }

        /// <summary>
        /// Convierte entidad a modelo negocio CoverageDeductible
        /// </summary>
        /// <param name="coverageDeductible">entidad CoverageDeductible</param>
        /// <returns>modelo de negocio CoverageDeductible</returns>
        public static ParamDeductibleDesc CreateParamDeductibleDescRelation(QUOEN.CoverageDeductible coverageDeductible)
        {
            return new ParamDeductibleDesc()
            {
                Id = coverageDeductible.DeductId,
                IsMandatory = coverageDeductible.IsDefault
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio CoverageDeductible
        /// </summary>
        /// <param name="businessCollection">coleccion CoverageDeductible</param>
        /// <returns>modelo de negocio CoverageDeductible</returns>
        public static List<ParamDeductibleDesc> CreateParamDeductibleDescsRelation(BusinessCollection businessCollection)
        {
            List<ParamDeductibleDesc> paramDeductibleDescs = new List<ParamDeductibleDesc>();
            foreach (QUOEN.CoverageDeductible item in businessCollection)
            {
                paramDeductibleDescs.Add(CreateParamDeductibleDescRelation(item));
            }

            return paramDeductibleDescs;
        }
        #endregion

        #region DetailType
        /// <summary>
        /// Convierte entidad a modelo negocio CoverDetailType
        /// </summary>
        /// <param name="coverDetailType">entidad CoverDetailType</param>
        /// <returns>modelo de negocio CoverDetailType</returns>
        public static ParamDetailTypeDesc CreateParamDetailTypeDescByCoverDetailType(QUOEN.CoverDetailType coverDetailType)
        {
            return new ParamDetailTypeDesc()
            {
                Id = coverDetailType.DetailTypeCode,
                IsMandatory = coverDetailType.IsMandatory
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio DetailType
        /// </summary>
        /// <param name="detailType">entidad DetailType</param>
        /// <returns>modelo de negocio DetailType</returns>
        public static ParamDetailTypeDesc CreateParamDetailTypeDesc(QUOEN.DetailType detailType)
        {
            return new ParamDetailTypeDesc()
            {
                Id = detailType.DetailTypeCode,
                Description = detailType.Description
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio DetailType
        /// </summary>
        /// <param name="businessCollection">coleccion DetailType</param>
        /// <returns>modelo de negocio DetailType</returns>
        public static List<ParamDetailTypeDesc> CreateParamDetailTypeDescs(BusinessCollection businessCollection)
        {
            List<ParamDetailTypeDesc> detailTypes = new List<ParamDetailTypeDesc>();
            foreach (QUOEN.DetailType item in businessCollection)
            {
                detailTypes.Add(CreateParamDetailTypeDesc(item));
            }

            return detailTypes;
        }

        /// <summary>
        /// Convierte entidad a modelo negocio CoverDetailType
        /// </summary>
        /// <param name="detailType">entidad CoverDetailType</param>
        /// <returns>modelo de negocio CoverDetailType</returns>
        public static ParamDetailTypeDesc CreateDetailTypeRelation(QUOEN.CoverDetailType detailType)
        {
            return new ParamDetailTypeDesc()
            {
                Id = detailType.DetailTypeCode,
                IsMandatory = detailType.IsMandatory
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio CoverDetailType
        /// </summary>
        /// <param name="businessCollection">coleccion CoverDetailType</param>
        /// <returns>modelo de negocio CoverDetailType</returns>
        public static List<ParamDetailTypeDesc> CreateDetailTypesRelation(BusinessCollection businessCollection)
        {
            List<ParamDetailTypeDesc> detailTypes = new List<ParamDetailTypeDesc>();
            foreach (QUOEN.CoverDetailType item in businessCollection)
            {
                detailTypes.Add(CreateDetailTypeRelation(item));
            }

            return detailTypes;
        }
        #endregion

        #region CreateParamCoverage
        /// <summary>
        /// Convierte entidad a modelo negocio Coverage
        /// </summary>
        /// <param name="coverage">entidad Coverage</param>
        /// <returns>modelo de negocio Coverage</returns>
        public static ParamCoverage CreateParamCoverage(QUOEN.Coverage coverage)
        {
            return new ParamCoverage()
            {
                Id = coverage.CoverageId,
                Description = coverage.PrintDescription,
                CompositionTypeId = coverage.CompositionTypeCode,
                IsPrincipal = coverage.IsPrimary,
                InsuredObjectDesc = new ParamInsuredObjectDesc()
                {
                    Id = coverage.InsuredObjectId,
                },
                Peril = new ParamPeril()
                {
                    Id = coverage.PerilCode,
                },
                LineBusiness = new ParamLineBusinessDesc()
                {
                    Id = coverage.LineBusinessCode,
                },
                SubLineBusiness = new ParamSubLineBusinessDesc()
                {
                    Id = coverage.SubLineBusinessCode,
                }
            };
        }

        /// <summary>
        /// Convierte entidad a modelo negocio
        /// </summary>
        /// <param name="businessCollection">collecion Coverage</param>
        /// <returns>modelo de negocio Coverage</returns>
        public static List<ParamCoverage> CreateParamCoverages(BusinessCollection businessCollection)
        {
            List<ParamCoverage> paramCoverages = new List<ParamCoverage>();
            foreach (QUOEN.Coverage item in businessCollection)
            {
                paramCoverages.Add(CreateParamCoverage(item));
            }

            return paramCoverages;
        }
        #endregion

        #region CreateParamCoverage
        /// <summary>
        /// Convierte entidad a modelo negocio CoCoverage
        /// </summary>
        /// <param name="cocoverage">Entidad CoCoverage</param>
        /// <returns>modelo de negocio CoCoverage</returns>
        public static ParamCoCoverage CreateParamCoCoverage(QUOEN.CoCoverage cocoverage)
        {
            return new ParamCoCoverage()
            {
                Description = cocoverage?.PrintDescription,
                ImpressionValue = cocoverage?.PrintDescriptionLimit,
                IsAccMinPremium = Convert.ToBoolean(cocoverage?.IsAccMinPremium),
                IsAssistance = Convert.ToBoolean(cocoverage?.IsAssistance),
                IsImpression = Convert.ToBoolean(cocoverage?.IsImpression),
                Id = Convert.ToInt32(cocoverage?.CoverageNum),
                IsChild = Convert.ToBoolean(cocoverage?.IsChild),
                IsSeriousOffer = Convert.ToBoolean(cocoverage?.IsSeriousOffer),
            };
        }
        /// <summary>
        /// Convierte entidad a modelo negocio CoCoverages
        /// </summary>
        /// <param name="cocoverages">Entidad CoCoverage</param>
        /// <returns>modelo de negocio CoCoverage</returns>
        public static List<ParamCoCoverage> CreateParamCoCoverages(List<QUOEN.CoCoverage> cocoverages)
        {
            List<ParamCoCoverage> cocCoverages = new List<ParamCoCoverage>();
            foreach (var cocoverage in cocoverages)
            {
                cocCoverages.Add(new ParamCoCoverage()
                {
                    Description = cocoverage?.PrintDescription,
                    ImpressionValue = cocoverage?.PrintDescriptionLimit,
                    IsAccMinPremium = Convert.ToBoolean(cocoverage?.IsAccMinPremium),
                    IsAssistance = Convert.ToBoolean(cocoverage?.IsAssistance),
                    IsImpression = Convert.ToBoolean(cocoverage?.IsImpression),
                    Id = cocoverage.CoverageNum
                });
            }
            return cocCoverages;
        }
        /// <summary>
        /// Convierte entidad a modelo negocio CoCoverage
        /// </summary>
        /// <param name="businessCollection">coleccion CoCoverage</param>
        /// <returns>modelo de negocio CoCoverage</returns>
        public static List<ParamCoCoverage> CreateParamCoCoverages(BusinessCollection businessCollection)
        {
            List<ParamCoCoverage> paramCoCoverages = new List<ParamCoCoverage>();
            foreach (QUOEN.CoCoverage item in businessCollection)
            {
                paramCoCoverages.Add(CreateParamCoCoverage(item));
            }

            return paramCoCoverages;
        }
        #endregion

        #region InsuredObject
        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="businessCollection">business Collection</param>       
        /// <returns>Modelo de objeto de seguro</returns>
        public static List<ParamInsuredObject> CreateCompanyInsuredObjects(BusinessCollection businessCollection)
        {
            List<ParamInsuredObject> companyInsuredObjects = new List<ParamInsuredObject>();

            foreach (QUOEN.InsuredObject entityInsuredObject in businessCollection)
            {
                companyInsuredObjects.Add(CreateCompanyInsuredObject(entityInsuredObject));
            }

            return companyInsuredObjects;
        }

        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityInsuredObject">entidad de objetos de seguro</param>       
        /// <returns>Modelo de objeto de seguro</returns>
        private static ParamInsuredObject CreateCompanyInsuredObject(QUOEN.InsuredObject entityInsuredObject)
        {
            return new ParamInsuredObject
            {
                Id = entityInsuredObject.InsuredObjectId,
                Description = entityInsuredObject.Description,
                SmallDescription = entityInsuredObject.SmallDescription,
                IsDeclarative = entityInsuredObject.IsDeclarative
            };
        }
        #endregion

        #region Component Expense
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="paramExponse"></param>
        /////// <returns></returns>
        public static Result<ParamExpense, ErrorModel> CreateParamExpense(QUOEN.Component component, QUOEN.ExpenseComponent expense)
        {
            int Rate = (int)expense.Rate;
            int? RuleSetId = expense.RuleSetId;
            Result<ParamExpense, ErrorModel> result =
             ParamExpense.GetParamComponent
             (
                 component.ComponentCode,
                 component.SmallDescription,
                 component.TinyDescription,
                 ENUM.ComponentClass.EXPENSES,
                 ENUM.ComponnetType.EXPENSES,
                 Rate,
                 expense.IsMandatory,
                 expense.IsInitially,
                 ((ResultValue<ParamRuleSet, ErrorModel>)ParamRuleSet.GetParamRuleSet(RuleSetId, null)).Value,
                 ((ResultValue<ParamRateType, ErrorModel>)ParamRateType.GetParamRuleSet(expense.RateTypeCode, null)).Value
                 );
            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="paramExponse"></param>
        /////// <returns></returns>
        public static Result<ParamExpense, ErrorModel> CreateParamExpenses(QUOEN.Component component, QUOEN.ExpenseComponent expense, EntRules.RuleSet ruleSet, RateType rateType)
        {
            int Rate = (int)expense.Rate;
            int? RuleSetId = expense.RuleSetId;
            Result<ParamExpense, ErrorModel> result =
             ParamExpense.GetParamComponent
             (
                 component.ComponentCode,
                 component.SmallDescription,
                 component.TinyDescription,
                 ENUM.ComponentClass.EXPENSES,
                 ENUM.ComponnetType.EXPENSES,
                 Rate,
                 expense.IsMandatory,
                 expense.IsInitially,
                 ((ResultValue<ParamRuleSet, ErrorModel>)ParamRuleSet.GetParamRuleSet(RuleSetId, ruleSet.Description)).Value,
                 ((ResultValue<ParamRateType, ErrorModel>)ParamRateType.GetParamRuleSet(expense.RateTypeCode, rateType.Description)).Value
                 );
            return result;
        }

        #endregion

        #region RateType
        /// <summary>
        /// Crea los campos  tipo de tasa
        /// </summary>
        /// <param name="businessCollection">business Collection</param>       
        /// <returns>Modelo de objeto de seguro</returns>
        public static Result<List<ParamRateType>, ErrorModel> CreateRateTypes(BusinessCollection businessCollection)
        {
            List<ParamRateType> paramRateType = new List<ParamRateType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamRateType, ErrorModel> result;
            foreach (RateType entityRateType in businessCollection)
            {
                result = CreateRateType(entityRateType);
                if (result is ResultError<ParamRateType, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleVersion a modelo de negocio.");
                    return new ResultError<List<ParamRateType>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamRateType resultValue = (result as ResultValue<ParamRateType, ErrorModel>).Value;
                    paramRateType.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamRateType>, ErrorModel>(paramRateType);
        }


        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityInsuredObject">entidad de objetos de seguro</param>       
        /// <returns>Modelo de objeto de seguro</returns>
        public static Result<ParamRateType, ErrorModel> CreateRateType(RateType rateType)
        {
            Result<ParamRateType, ErrorModel> result =
             ParamRateType.CreateParamRateType
             (
                 rateType.RateTypeCode,
                 rateType.Description
                 );
            return result;
        }
        #endregion

        #region RuleSet

        /// <summary>
        /// Crea los campos  tipo de tasa
        /// </summary>
        /// <param name="businessCollection">business Collection</param>       
        /// <returns>Modelo de objeto de seguro</returns>
        public static Result<List<ParamRuleSet>, ErrorModel> CreateRuleSet(BusinessCollection businessCollection)
        {
            List<ParamRuleSet> paramRuleSet = new List<ParamRuleSet>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamRuleSet, ErrorModel> result;
            foreach (EntRules.RuleSet entityRuleSet in businessCollection)
            {
                result = CreateRuleSet(entityRuleSet);
                if (result is ResultError<ParamRuleSet, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleVersion a modelo de negocio.");
                    return new ResultError<List<ParamRuleSet>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamRuleSet resultValue = (result as ResultValue<ParamRuleSet, ErrorModel>).Value;
                    paramRuleSet.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamRuleSet>, ErrorModel>(paramRuleSet);
        }


        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityInsuredObject">entidad de objetos de seguro</param>       
        /// <returns>Modelo de objeto de seguro</returns>
        public static Result<ParamRuleSet, ErrorModel> CreateRuleSet(EntRules.RuleSet ruleSet)
        {
            Result<ParamRuleSet, ErrorModel> result =
             ParamRuleSet.CreateParamRuleSet
             (
                 ruleSet.RuleSetId,
                 ruleSet.Description
                 );
            return result;
        }
        #endregion

        #region Clause
        /// <summary>
        /// Metodo para mapear la entidad al modelo de servicio
        /// </summary>
        /// <param name="clause">recibe clausulas</param>
        /// <returns>retorna clausulas</returns>
        public static ParamClause CreateParametrizationClause(QUOEN.Clause clause)
        {
            ParamClause parametrizationClause = new ParamClause
            {
                Clause = new UNDMO.Clause
                {
                    Id = clause.ClauseId,
                    Name = clause.ClauseName,
                    Title = clause.ClauseTitle,
                    Text = clause.ClauseText,
                    ConditionLevel = new UNDMO.ConditionLevel
                    {
                        Id = clause.ConditionLevelCode,
                        EmissionLevel = UNDEN.EmissionLevel.General
                    }
                },
                InputStartDate = clause.CurrentFrom,
                DueDate = clause.CurrentTo
            };
            return parametrizationClause;
        }

        /// <summary>
        /// Metodo lista de clausulas
        /// </summary>
        /// <param name="collection">Lista de clausulas</param>
        /// <returns>Retorna coleccion clasusulas</returns>
        public static List<ParamClause> CreateParametrizationClauses(BusinessCollection collection)
        {
            List<ParamClause> clauses = new List<ParamClause>();
            foreach (QUOEN.Clause item in collection)
            {
                clauses.Add(CreateParametrizationClause(item));
            }

            return clauses;
        }
        #endregion Clause

        #region ClauseLevel
        /// <summary>
        /// Metodo para crear clausula por nivel
        /// </summary>
        /// <param name="clauseLevelEntity">Recibe clausulas por nivel</param>
        /// <returns>Retorna clausulas por nivel</returns>
        public static ParamClauseLevel CreateClauseLevel(QUOEN.ClauseLevel clauseLevelEntity)
        {
            ParamClauseLevel clauseLevel = new ParamClauseLevel
            {
                ClauseLevelId = clauseLevelEntity.ClauseLevelId,
                ClauseId = clauseLevelEntity.ClauseId,
                ConditionLevelId = clauseLevelEntity.ConditionLevelId,
                IsMandatory = clauseLevelEntity.IsMandatory
            };
            return clauseLevel;
        }

        /// <summary>
        /// Metodo contiene lista de clausulas por nivel
        /// </summary>
        /// <param name="collection">Recibe coleccion de clausulas por nivel</param>
        /// <returns>Retorna coleccion de clausulas por nivel</returns>
        public static List<ParamClauseLevel> CreateClauseLevels(BusinessCollection collection)
        {
            List<ParamClauseLevel> clauses = new List<ParamClauseLevel>();
            foreach (QUOEN.ClauseLevel item in collection)
            {
                clauses.Add(CreateClauseLevel(item));
            }

            return clauses;
        }
        #endregion ClauseLevel

        #region ConditionLevel
        /// <summary>
        /// Metodo para crear condicion por nivel
        /// </summary>
        /// <param name="conditionLevel">Recibe condicion por nivel</param>
        /// <returns>Retorna condicion por nivel</returns>
        public static UNDMO.ConditionLevel CreateConditionLevel(PAREN.ConditionLevel conditionLevel)
        {
            UNDMO.ConditionLevel parametrizationClause = new UNDMO.ConditionLevel
            {
                Id = conditionLevel.ConditionLevelCode,
                EmissionLevel = UNDEN.EmissionLevel.General,
                Description = conditionLevel.SmallDescription
            };
            return parametrizationClause;
        }

        /// <summary>
        /// Metodo contiene lista condicion por niveles
        /// </summary>
        /// <param name="collection">Recobibe coleccion por niveles</param>
        /// <returns>Retorna lista de condicion por nivel</returns>
        public static List<UNDMO.ConditionLevel> CreateConditionLevels(BusinessCollection collection)
        {
            List<UNDMO.ConditionLevel> clauses = new List<UNDMO.ConditionLevel>();
            foreach (PAREN.ConditionLevel item in collection)
            {
                clauses.Add(CreateConditionLevel(item));
            }

            return clauses;
        }
        #endregion ConditionLevel
        #region Prefix
        /// <summary>
        /// Metodo modelo ramo comercial
        /// </summary>
        /// <param name="prefix">Ramo comercial</param>
        /// <returns>Retorna ramo comercial</returns>
        public static ParamClausePrefix CreatePrefix(COMMEN.Prefix prefix)
        {
            ParamClausePrefix paramClausePrefix = new ParamClausePrefix
            {
                Id = prefix.PrefixCode,
                Description = prefix.Description,
                SmallDescription = prefix.SmallDescription
            };
            return paramClausePrefix;
        }

        /// <summary>
        /// Metodo lista de ramo comercial
        /// </summary>
        /// <param name="collection">coleccion ramo comercial</param>
        /// <returns>Lista ramo comercial</returns>
        public static List<ParamClausePrefix> CreatePrefixs(BusinessCollection collection)
        {
            List<ParamClausePrefix> paramClausesPrefix = new List<ParamClausePrefix>();
            foreach (COMMEN.Prefix item in collection)
            {
                paramClausesPrefix.Add(CreatePrefix(item));
            }

            return paramClausesPrefix;
        }
        #endregion Prefix

        #region RiskType
        /// <summary>
        /// Metodo contiene tipo de riesgo
        /// </summary>
        /// <param name="riskType">Recibe tipo de riesgo</param>
        /// <returns>Retorna tipo de riesgo</returns>
        public static UNDMO.RiskType CreateCoveredRiskTypeConsult(PAREN.CoveredRiskType riskType)
        {
            UNDMO.RiskType parametrizationClause = new UNDMO.RiskType
            {
                Id = riskType.CoveredRiskTypeCode,
                Description = riskType.SmallDescription
            };
            return parametrizationClause;
        }

        /// <summary>
        /// Metodo contiene coleccion de tipo de riesgo
        /// </summary>
        /// <param name="collection">Recibe coleccion tipo de riesgo</param>
        /// <returns>Retorna lista tipo de riesgo</returns>
        public static List<UNDMO.RiskType> CreateCoveredRiskTypesConsult(BusinessCollection collection)
        {
            List<UNDMO.RiskType> clauses = new List<UNDMO.RiskType>();
            foreach (PAREN.CoveredRiskType item in collection)
            {
                clauses.Add(CreateCoveredRiskTypeConsult(item));
            }

            return clauses;
        }
        #endregion RiskType

        #region LineBusinessForClause
        /// <summary>
        /// Metodo contiene ramo tecnico
        /// </summary>
        /// <param name="lineBusiness">Recibe ramo tecnico</param>
        /// <returns>Retorna ramo tecnico</returns>
        public static ParamClauseLineBusiness CreateLienBusinessConsult(COMMEN.LineBusiness lineBusiness)
        {
            ParamClauseLineBusiness parametrizationClause = new ParamClauseLineBusiness
            {
                Id = lineBusiness.LineBusinessCode,
                Description = lineBusiness.SmallDescription
            };
            return parametrizationClause;
        }

        /// <summary>
        /// Metodo contiene coleccion de tipo de riesgo
        /// </summary>
        /// <param name="collection">Recibe coleccion tipo de riesgo</param>
        /// <returns>Retorna lista tipo de riesgo</returns>
        public static List<ParamClauseLineBusiness> CreateLinesBusinessConsult(BusinessCollection collection)
        {
            List<ParamClauseLineBusiness> clauses = new List<ParamClauseLineBusiness>();
            foreach (COMMEN.LineBusiness item in collection)
            {
                clauses.Add(CreateLienBusinessConsult(item));
            }

            return clauses;
        }
        #endregion

        #region Text
        /// <summary>
        /// Metodo Textos
        /// </summary>
        /// <param name="conditionText">Recibe Textos</param>
        /// <returns>Retorna Modelo Textos</returns>
        public static ParamClause CreateText(QUOEN.ConditionText conditionText)
        {
            ParamClause parametrizationClause = new ParamClause
            {
                Text = new UNDMO.Text
                {
                    Id = conditionText.ConditionTextId,
                    TextTitle = conditionText.TextTitle,
                    TextBody = conditionText.TextBody
                }
            };
            return parametrizationClause;
        }

        /// <summary>
        /// Metodo lista de textos
        /// </summary>
        /// <param name="collection">Coleccion de textos</param>
        /// <returns>Retorna listado de textos</returns>
        public static List<ParamClause> CreateTexts(BusinessCollection collection)
        {
            List<ParamClause> clauses = new List<ParamClause>();
            foreach (QUOEN.ConditionText item in collection)
            {
                clauses.Add(CreateText(item));
            }

            return clauses;
        }

        #endregion

        #region Coverage
        /// <summary>
        /// cobertura objeto del seguro y amparo
        /// </summary>
        /// <param name="coverage">Recibe Coberturas</param>
        /// <returns>Retorna coberturas</returns>
        public static ParamClauseCoverage CreateParametrizationCoverage(QUOEN.Coverage coverage)
        {
            ParamClauseCoverage clauseParametrization = new ParamClauseCoverage
            {
                Id = coverage.CoverageId,
                Description = coverage.PrintDescription,
                ParamClauseInsuredObject = new ParamClauseInsuredObject
                {
                    Id = coverage.InsuredObjectId
                },
                Peril = new UNDMO.Peril
                {
                    Id = coverage.PerilCode
                },
            };
            return clauseParametrization;
        }

        /// <summary>
        /// Metodo coleccion de coberturas
        /// </summary>
        /// <param name="collection">Lista Coberturas</param>
        /// <returns>Retorna listado coberturas</returns>
        public static List<ParamClauseCoverage> CreateParametrizationCoverages(BusinessCollection collection)
        {
            List<ParamClauseCoverage> clausesCoverage = new List<ParamClauseCoverage>();
            foreach (QUOEN.Coverage item in collection)
            {
                clausesCoverage.Add(ModelAssembler.CreateParametrizationCoverage(item));
            }

            return clausesCoverage;
        }
        #endregion Coverage
        #region ClauseInsuredObject
        /// <summary>
        /// Metodo objeto del seguro
        /// </summary>
        /// <param name="clauseInsuredObjectEntity">Recibe objeto del seguro</param>
        /// <returns>Retorna objeto del seguro</returns>
        public static ParamClauseInsuredObject CreateClauseInsuredObject(QUOEN.InsuredObject clauseInsuredObjectEntity)
        {
            ParamClauseInsuredObject insuredObject = new ParamClauseInsuredObject
            {
                Id = clauseInsuredObjectEntity.InsuredObjectId,
                Description = clauseInsuredObjectEntity.Description,
                SmallDescription = clauseInsuredObjectEntity.SmallDescription
            };
            return insuredObject;
        }

        /// <summary>
        /// Metodo contiene lista objeto del seguro
        /// </summary>
        /// <param name="collection">Recibe coleccion objeto del seguro</param>
        /// <returns>Retorna coleccion Objecto del seguro</returns>
        public static List<ParamClauseInsuredObject> CreateClauseInsuredObjects(BusinessCollection collection)
        {
            List<ParamClauseInsuredObject> paramClausesInsuredObject = new List<ParamClauseInsuredObject>();
            foreach (QUOEN.InsuredObject item in collection)
            {
                paramClausesInsuredObject.Add(CreateClauseInsuredObject(item));
            }

            return paramClausesInsuredObject;
        }
        #endregion ClauseInsuredObject

        #region ClausePeril
        /// <summary>
        /// Metodo para crear peril
        /// </summary>
        /// <param name="clausePerilEntity">Recibe amparo</param>
        /// <returns>Retorna amparo</returns>
        public static UNDMO.Peril CreateClausePeril(QUOEN.Peril clausePerilEntity)
        {
            UNDMO.Peril clauseLevel = new UNDMO.Peril
            {
                Id = clausePerilEntity.PerilCode,
                Description = clausePerilEntity.Description,
                SmallDescription = clausePerilEntity.SmallDescription
            };
            return clauseLevel;
        }

        /// <summary>
        /// Metodo coleccion amparo
        /// </summary>
        /// <param name="collection">coleccion amparo</param>
        /// <returns>retorna coleccion del amparo</returns>
        public static List<UNDMO.Peril> CreateClausePerils(BusinessCollection collection)
        {
            List<UNDMO.Peril> paramClausesPeril = new List<UNDMO.Peril>();
            foreach (QUOEN.Peril item in collection)
            {
                paramClausesPeril.Add(CreateClausePeril(item));
            }

            return paramClausesPeril;
        }
        #endregion ClausePeril

        #region Prefix
        /// <summary>
        /// Mapeo de la entidad Prefix al modelo Prefix
        /// </summary>
        /// <param name="prefix">Entidad Prefix</param>
        /// <returns>Modelo Prefix</returns>
        public static Result<ParamPrefix, ErrorModel> CreatePrefixBusiness(COMMEN.Prefix prefix)
        {
            Result<ParamPrefix, ErrorModel> result = ParamPrefix.GetParamPrefix(prefix.PrefixCode, prefix.Description, prefix.SmallDescription);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo Prefix
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos Prefix</returns>
        public static Result<List<ParamPrefix>, ErrorModel> CreatePrefixBusiness(BusinessCollection businessCollection)
        {
            List<ParamPrefix> prefix = new List<ParamPrefix>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamPrefix, ErrorModel> result;
            foreach (COMMEN.Prefix entityPrefix in businessCollection)
            {
                result = CreatePrefixBusiness(entityPrefix);
                if (result is ResultError<ParamPrefix, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingPrefix);
                    return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamPrefix resultValue = (result as ResultValue<ParamPrefix, ErrorModel>).Value;
                    prefix.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamPrefix>, ErrorModel>(prefix);
        }
        #endregion

        #region AssistanceType
        /// <summary>
        /// Mapeo de la entidad AssistanceType al modelo AssistanceType
        /// </summary>
        /// <param name="assistanceType">Entidad AssistanceType</param>
        /// <returns>Modelo AssistanceType</returns>
        public static Result<ParamAssistanceType, ErrorModel> CreateAssistanceType(COMMEN.CptAssistanceType assistanceType)
        {
            Result<ParamAssistanceType, ErrorModel> result = ParamAssistanceType.GetParamAssistanceType(assistanceType.AssistanceCode, assistanceType.Description);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo AssistanceType
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos AssistanceType</returns>
        public static Result<List<ParamAssistanceType>, ErrorModel> CreateAssistanceType(BusinessCollection businessCollection)
        {
            List<ParamAssistanceType> assistanceType = new List<ParamAssistanceType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamAssistanceType, ErrorModel> result;
            foreach (COMMEN.CptAssistanceType entityAssistanceType in businessCollection)
            {
                result = CreateAssistanceType(entityAssistanceType);
                if (result is ResultError<ParamAssistanceType, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingAssistanceType);
                    return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamAssistanceType resultValue = (result as ResultValue<ParamAssistanceType, ErrorModel>).Value;
                    assistanceType.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamAssistanceType>, ErrorModel>(assistanceType);
        }
        #endregion

        #region GroupCoverage
        /// <summary>
        /// Mapeo de la entidad GroupCoverage al modelo GroupCoverage
        /// </summary>
        /// <param name="groupCover">Entidad ProductGroupCover</param>
        /// <returns>Modelo GroupCoverage</returns>
        public static Result<ParamGroupCoverage, ErrorModel> CreateGroupCoverage(PRODEN.ProductGroupCover groupCover)
        {
            Result<ParamGroupCoverage, ErrorModel> result = ParamGroupCoverage.GetParamGroupCoverage(groupCover.CoverGroupId, groupCover.SmallDescription);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo GroupCoverage
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos GroupCoverage</returns>
        public static Result<List<ParamGroupCoverage>, ErrorModel> CreateGroupCoverage(BusinessCollection businessCollection)
        {
            List<ParamGroupCoverage> groupCover = new List<ParamGroupCoverage>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamGroupCoverage, ErrorModel> result;
            foreach (PRODEN.ProductGroupCover entityGroupCoverage in businessCollection)
            {
                result = CreateGroupCoverage(entityGroupCoverage);
                if (result is ResultError<ParamGroupCoverage, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingGroupCoverage);
                    return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamGroupCoverage resultValue = (result as ResultValue<ParamGroupCoverage, ErrorModel>).Value;
                    groupCover.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamGroupCoverage>, ErrorModel>(groupCover);
        }
        #endregion

        #region Product
        /// <summary>
        /// Mapeo de la entidad Product al modelo Product
        /// </summary>
        /// <param name="product">Entidad Product</param>
        /// <returns>Modelo Product</returns>
        public static Result<ParamProduct, ErrorModel> CreateProduct(PRODEN.Product product)
        {
            bool activeProduct = false;
            if (product.CurrentTo == null || product.CurrentTo >= DateTime.Now)
            {
                activeProduct = true;
            }
            Result<ParamProduct, ErrorModel> result = ParamProduct.GetParamProduct(product.ProductId, product.Description, product.SmallDescription, activeProduct);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo Product
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos Product</returns>
        public static Result<List<ParamProduct>, ErrorModel> CreateProduct(BusinessCollection businessCollection)
        {
            List<ParamProduct> product = new List<ParamProduct>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamProduct, ErrorModel> result;
            foreach (PRODEN.Product entityProduct in businessCollection)
            {
                result = CreateProduct(entityProduct);
                if (result is ResultError<ParamProduct, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingProduct);
                    return new ResultError<List<ParamProduct>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamProduct resultValue = (result as ResultValue<ParamProduct, ErrorModel>).Value;
                    product.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamProduct>, ErrorModel>(product);
        }
        #endregion

        #region RequestEndorsement
        /// <summary>
        /// Mapeo de la entidad RequestEndorsement al modelo RequestEndorsement
        /// </summary>
        /// <param name="requestEndorsement">Entidad RequestEndorsement</param>
        /// <returns>Modelo RequestEndorsement</returns>
        public static Result<ParamRequestEndorsement, ErrorModel> CreateRequestEndorsement(REQEN.CoRequestEndorsement requestEndorsement)
        {
            Result<ParamRequestEndorsement, ErrorModel> result = ParamRequestEndorsement.GetParamRequestEndorsement(requestEndorsement.RequestEndorsementId, requestEndorsement.RequestId, (int)requestEndorsement.ProductId, (int)requestEndorsement.PrefixCode);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo RequestEndorsement
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos RequestEndorsement</returns>
        public static Result<List<ParamRequestEndorsement>, ErrorModel> CreateRequestEndorsement(BusinessCollection businessCollection)
        {
            List<ParamRequestEndorsement> requestEndorsement = new List<ParamRequestEndorsement>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamRequestEndorsement, ErrorModel> result;
            foreach (REQEN.CoRequestEndorsement entityRequestEndorsement in businessCollection)
            {
                result = CreateRequestEndorsement(entityRequestEndorsement);
                if (result is ResultError<ParamRequestEndorsement, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingRequestEndorsement);
                    return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamRequestEndorsement resultValue = (result as ResultValue<ParamRequestEndorsement, ErrorModel>).Value;
                    requestEndorsement.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamRequestEndorsement>, ErrorModel>(requestEndorsement);
        }
        #endregion

        #region Business
        /// <summary>
        /// Mapeo de la entidad Business al modelo Business
        /// </summary>
        /// <param name="business">Entidad Business</param>
        /// <returns>Modelo Business</returns>
        public static Result<ParamBusiness, ErrorModel> CreateBusiness(COMMEN.Business business, string prefixDescription, string prefixSmallDescription)
        {
            Result<ParamPrefix, ErrorModel> paramPrefix = ParamPrefix.GetParamPrefix((int)business.PrefixCode, prefixDescription, prefixSmallDescription);
            if (paramPrefix is ResultValue<ParamPrefix, ErrorModel>)
            {
                Result<ParamBusiness, ErrorModel> result = ParamBusiness.GetParamBusiness(business.BusinessId, business.Description, business.IsEnabled, (paramPrefix as ResultValue<ParamPrefix, ErrorModel>).Value);
                return result;
            }
            else
            {
                List<string> errorModelListDescription = new List<string>();
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingBusiness);
                return new ResultError<ParamBusiness, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
        }
        #endregion

        #region BusinessConfiguration
        /// <summary>
        /// Mapeo de la entidad BusinessConfiguration al modelo BusinessConfiguration
        /// </summary>
        /// <param name="businessConfiguration">Entidad BusinessConfiguration</param>
        /// <returns>Modelo BusinessConfiguration</returns>
        public static Result<ParamBusinessConfiguration, ErrorModel> CreateBusinessConfiguration(QUOEN.BusinessConfiguration businessConfiguration, REQEN.CoRequestEndorsement requestEndorsement, PRODEN.Product product, PRODEN.ProductGroupCover groupCover, COMMEN.CptAssistanceType assistanceType)
        {
            Result<ParamRequestEndorsement, ErrorModel> paramRequestEndorsement;
            List<string> errorModelListDescription = new List<string>();
            if (requestEndorsement != null)
            {
                paramRequestEndorsement = ParamRequestEndorsement.GetParamRequestEndorsement(requestEndorsement.RequestEndorsementId, requestEndorsement.RequestId, (int)requestEndorsement.ProductId, (int)requestEndorsement.PrefixCode);
                if (paramRequestEndorsement is ResultError<ParamRequestEndorsement, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingRequestEndorsement);
                    return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
            }
            else
            {
                paramRequestEndorsement = null;
            }

            bool activeProduct = false;
            if (product.CurrentTo == null || product.CurrentTo >= DateTime.Now)
            {
                activeProduct = true;
            }
            Result<ParamProduct, ErrorModel> paramProduct = ParamProduct.GetParamProduct(product.ProductId, product.Description, product.SmallDescription, activeProduct);
            if (paramProduct is ResultError<ParamProduct, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingProduct);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            Result<ParamGroupCoverage, ErrorModel> paramGroupCoverage = ParamGroupCoverage.GetParamGroupCoverage(groupCover.CoverGroupId, groupCover.SmallDescription);
            if (paramGroupCoverage is ResultError<ParamGroupCoverage, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingGroupCoverage);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }

            Result<ParamAssistanceType, ErrorModel> paramAssistanceType = ParamAssistanceType.GetParamAssistanceType(assistanceType.AssistanceCode, assistanceType.Description);
            if (paramAssistanceType is ResultError<ParamAssistanceType, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingAssistanceType);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            Result<ParamBusinessConfiguration, ErrorModel> result;
            if (requestEndorsement != null)
            {
                result = ParamBusinessConfiguration.GetParamBusinessConfiguration(businessConfiguration.BusinessConfigurationId, businessConfiguration.BusinessId, (paramRequestEndorsement as ResultValue<ParamRequestEndorsement, ErrorModel>).Value, (paramProduct as ResultValue<ParamProduct, ErrorModel>).Value, (paramGroupCoverage as ResultValue<ParamGroupCoverage, ErrorModel>).Value, (paramAssistanceType as ResultValue<ParamAssistanceType, ErrorModel>).Value, businessConfiguration.ProductIdResponse);
            }
            else
            {
                result = ParamBusinessConfiguration.GetParamBusinessConfiguration(businessConfiguration.BusinessConfigurationId, businessConfiguration.BusinessId, null, (paramProduct as ResultValue<ParamProduct, ErrorModel>).Value, (paramGroupCoverage as ResultValue<ParamGroupCoverage, ErrorModel>).Value, (paramAssistanceType as ResultValue<ParamAssistanceType, ErrorModel>).Value, businessConfiguration.ProductIdResponse);
            }

            return result;
        }
        #endregion

        #region Branch
        /// <summary>
        /// Convierte entidad a modelo de sucursal
        /// </summary>
        /// <param name="entityBranch">entidad de sucursal</param>
        /// <returns>Branch para parametrización</returns>
        public static COMMO.ParamBranch CreateParamBranch(COMMEN.Branch entityBranch)
        {
            return new COMMO.ParamBranch
            {
                Id = entityBranch.BranchCode,
                Description = entityBranch.Description,
                SmallDescription = entityBranch.SmallDescription
            };
        }

        /// <summary>
        /// Convierte una colección en lista de sucursales
        /// </summary>
        /// <param name="businessCollection">Colección de sucursales</param>
        /// <returns>Lista de sucursales</returns>
        public static List<COMMO.ParamBranch> CreateParamBranchs(BusinessCollection businessCollection)
        {
            List<COMMO.ParamBranch> branches = new List<COMMO.ParamBranch>();

            foreach (COMMEN.Branch entity in businessCollection)
            {
                branches.Add(ModelAssembler.CreateParamBranch(entity));
            }

            return branches;
        }

        #endregion

        #region Prefix
        /// <summary>
        /// Convierte entidad en modelo de ramo
        /// </summary>
        /// <param name="entityPrefix">entidad de ramo</param>
        /// <returns>Modelo de ramo</returns>
        public static ParamPrefix CreateParamPrefix(COMMEN.Prefix entityPrefix)
        {
            return new ParamPrefix(entityPrefix.PrefixCode, entityPrefix.Description, entityPrefix.SmallDescription);
        }

        /// <summary>
        /// Convierte colección en lista de ramos
        /// </summary>
        /// <param name="businessCollection">Colección de ramos</param>
        /// <returns>Lista de ramos</returns>
        public static List<ParamPrefix> CreateParamPrefixs(BusinessCollection businessCollection)
        {
            List<ParamPrefix> prefixes = new List<ParamPrefix>();

            foreach (COMMEN.Prefix entity in businessCollection)
            {
                prefixes.Add(ModelAssembler.CreateParamPrefix(entity));
            }

            return prefixes;
        }

        #endregion

        #region business Line Parametrization
        /// <summary>
        /// Convierte entidad de ramo técnoco a modelo de parametrozación de ramo técnico
        /// </summary>
        /// <param name="lineBusinessEntity">Entidad de ramo técnico</param>
        /// <returns>Modelo de ramo técnico</returns>
        public static ParamLineBusiness CreateParamLineBusiness(COMMEN.LineBusiness lineBusinessEntity)
        {
            return new ParamLineBusiness
            {
                Id = lineBusinessEntity.LineBusinessCode,
                Description = lineBusinessEntity.Description,
                SmallDescription = lineBusinessEntity.SmallDescription,
                TinyDescription = lineBusinessEntity.TinyDescription
            };
        }

        /// <summary>
        /// Convierte businnessColection a Modelo de parametrización de ramo técnico
        /// </summary>
        /// <param name="businessCollection">colección de ramo técnico</param>
        /// <returns>Lista de modelos de ramo técnico</returns>
        public static List<ParamLineBusiness> CreateParamLineBusinesss(BusinessCollection businessCollection)
        {
            List<ParamLineBusiness> lineBusinesses = new List<ParamLineBusiness>();

            foreach (COMMEN.LineBusiness entityLineBusiness in businessCollection)
            {
                lineBusinesses.Add(ModelAssembler.CreateParamLineBusiness(entityLineBusiness));
            }

            return lineBusinesses;
        }
        #endregion

        #region Perils
        /// <summary>
        /// Convierte Colección en modelo de amparo
        /// </summary>
        /// <param name="businessCollection">Colección de amparos</param>
        /// <returns>Lista de modelos de amparo</returns>
        public static List<UNDMO.Peril> CreatePerils(BusinessCollection businessCollection)
        {
            List<UNDMO.Peril> perils = new List<UNDMO.Peril>();

            foreach (QUOEN.Peril entityPeril in businessCollection)
            {
                perils.Add(ModelAssembler.CreatePeril(entityPeril));
            }

            return perils;
        }

        /// <summary>
        /// Convierte entidad en modelo de amparo
        /// </summary>
        /// <param name="entityPeril">Entidad de amparo</param>
        /// <returns>Modelo de amparo</returns>
        public static UNDMO.Peril CreatePeril(QUOEN.Peril entityPeril)
        {
            return new UNDMO.Peril
            {
                Id = entityPeril.PerilCode,
                Description = entityPeril.Description,
                SmallDescription = entityPeril.SmallDescription
            };
        }
        #endregion

        #region Clauses
        /// <summary>
        /// Convierte colección en modelos de cláusulas
        /// </summary>
        /// <param name="businessCollection">colección de cláusulas</param>
        /// <returns>Lista de modelo de cláusulas</returns>
        public static List<UNDMO.Clause> CreateClauses(BusinessCollection businessCollection)
        {
            List<UNDMO.Clause> clauses = new List<UNDMO.Clause>();

            foreach (QUOEN.Clause entityClause in businessCollection)
            {
                clauses.Add(ModelAssembler.CreateClause(entityClause));
            }

            return clauses;
        }

        /// <summary>
        /// Convierte entidad en modelo de cláusula
        /// </summary>
        /// <param name="entityClause">entidad de cláusula</param>
        /// <returns>Modelo de cláusula</returns>
        public static UNDMO.Clause CreateClause(QUOEN.Clause entityClause)
        {
            return new UNDMO.Clause
            {
                Id = entityClause.ClauseId,
                Name = entityClause.ClauseName
            };
        }
        #endregion

        #region Tipo de riesgo cubierto
        /// <summary>
        /// Convierte colección en modelo de tipo de riesgo cubierto
        /// </summary>
        /// <param name="businessCollection">Colección de tipos de riesgo cubiertos</param>
        /// <returns>Lista de modelos de tipo de riesgo cubierto</returns>
        public static List<ParamCoveredRiskTypeDesc> CreateParamCoveredRiskTypeDescs(BusinessCollection businessCollection)
        {
            List<ParamCoveredRiskTypeDesc> paramParamCoveredRiskTypeDescs = new List<ParamCoveredRiskTypeDesc>();

            foreach (CoveredRiskType entityParamCoveredRiskTypeDesc in businessCollection)
            {
                paramParamCoveredRiskTypeDescs.Add(ModelAssembler.CreateParamGroupCoverageDesc(entityParamCoveredRiskTypeDesc));
            }

            return paramParamCoveredRiskTypeDescs;
        }

        /// <summary>
        /// Convierte entidad en modelo de tipo de riesgo cubierto
        /// </summary>
        /// <param name="entityParamCoveredRiskTypeDesc">Entidad de tipos de riesgo cubiertos</param>
        /// <returns>Modelo de tipo de riesgo cubierto</returns>
        public static ParamCoveredRiskTypeDesc CreateParamGroupCoverageDesc(CoveredRiskType entityParamCoveredRiskTypeDesc)
        {
            return new ParamCoveredRiskTypeDesc
            {
                Id = entityParamCoveredRiskTypeDesc.CoveredRiskTypeCode,
                Description = entityParamCoveredRiskTypeDesc.SmallDescription
            };
        }
        #endregion

        #region Carrocería de vehículo

        /// <summary>
        /// Crea el modelo de tipo de carroceria y usos
        /// </summary>
        /// <param name="vehicleBody">Tipo de carrocerias</param>
        /// <param name="vehicleUses">Tipo de usos</param>
        /// <returns>Modelo de carroceria y usos</returns>
        public static Result<ParamVehicleBodyUse, ErrorModel> CreateVehicleBodyUse(COMMEN.VehicleBody vehicleBody, BusinessCollection vehicleUses)
        {
            List<string> errorModelListDescription = new List<string>();

            Result<ParamVehicleBody, ErrorModel> resultVehicleBody = CreateVehicleBody(vehicleBody);
            if (resultVehicleBody is ResultError<ParamVehicleBody, ErrorModel>)
            {
                return new ResultError<ParamVehicleBodyUse, ErrorModel>(((ResultError<ParamVehicleBody, ErrorModel>)resultVehicleBody).Message);
            }

            Result<List<ParamVehicleUse>, ErrorModel> resultVehicleBodies = CreateVehicleUse(vehicleUses);
            if (resultVehicleBodies is ResultError<List<ParamVehicleUse>, ErrorModel>)
            {
                return new ResultError<ParamVehicleBodyUse, ErrorModel>(((ResultError<List<ParamVehicleUse>, ErrorModel>)resultVehicleBodies).Message);
            }

            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)resultVehicleBody).Value;
            List<ParamVehicleUse> paramVehicleBodies = ((ResultValue<List<ParamVehicleUse>, ErrorModel>)resultVehicleBodies).Value;

            return ParamVehicleBodyUse.CreateParamVehicleBodyUse(paramVehicleBody, paramVehicleBodies);
        }

        /// <summary>
        /// Mapeo de la entidad VehicleUse al modelo ParamVehicleUse
        /// </summary>
        /// <param name="vehicleUse">Entidad VehicleUse</param>
        /// <returns>Modelo ParamVehicleUse</returns>
        public static Result<ParamVehicleUse, ErrorModel> CreateVehicleUse(COMMEN.VehicleUse vehicleUse)
        {
            Result<ParamVehicleUse, ErrorModel> result = ParamVehicleUse.CreateParamVehicleUse(vehicleUse.VehicleUseCode, vehicleUse.SmallDescription);
            return result;
        }

        /// <summary>
        /// Método para mapear de businessCollection a Result
        /// </summary>
        /// <param name="businessCollection">Lista Usos</param>
        /// <returns>Lista de usos</returns>
        public static Result<List<ParamVehicleUse>, ErrorModel> CreateVehicleUse(BusinessCollection businessCollection)
        {
            List<ParamVehicleUse> vehicleUses = new List<ParamVehicleUse>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleUse, ErrorModel> result;
            foreach (COMMEN.VehicleUse entityVehicleUse in businessCollection)
            {
                result = CreateVehicleUse(entityVehicleUse);
                if (result is ResultError<ParamVehicleUse, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingEntitiesToBusinessModelsVehicleBody);
                    return new ResultError<List<ParamVehicleUse>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleUse resultValue = (result as ResultValue<ParamVehicleUse, ErrorModel>).Value;
                    vehicleUses.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamVehicleUse>, ErrorModel>(vehicleUses);
        }
        #endregion Carrocería de vehículo

        #region CoCoverage2G
        /// <summary>
        /// Crea el modelo de las coverturas de 2G
        /// </summary>
        /// <param name="businessCollection">Coleeccion de entidades</param>
        /// <returns>Modelo de servicio</returns>
        public static List<ParamCoCoverage2G> CreateCoCoverage2G(BusinessCollection businessCollection)
        {
            List<ParamCoCoverage2G> coverages2G = new List<ParamCoCoverage2G>();
            foreach (INTEN.CoCoverage2g coverage2G in businessCollection)
            {
                coverages2G.Add(CreateCoCoverage2G(coverage2G));
            }
            return coverages2G;
        }

        /// <summary>
        /// Crea el modelo de las coverturas de 2G
        /// </summary>
        /// <param name="coverage2G">Entidad de la covertura 2G</param>
        /// <returns>Modelo de servicio</returns>
        public static ParamCoCoverage2G CreateCoCoverage2G(INTEN.CoCoverage2g coverage2G)
        {
            return new ParamCoCoverage2G()
            {
                Id = Convert.ToInt32(coverage2G.CoverageCode),
                Description = coverage2G.Description,
                InsuredObjectId = Convert.ToInt32(coverage2G.InsuredObject),
                LineBusinessId = Convert.ToInt32(coverage2G.LineBusinessCode),
                SubLineBusinessId = Convert.ToInt32(coverage2G.SubLineBusinessCode)
            };
        }

        /// <summary>
        /// Crea el modelo de las coverturas de 2G
        /// </summary>
        /// <param name="coverage2G">Entidad de la covertura 2G</param>
        /// <returns>Modelo de servicio</returns>
        public static ParamCoCoverage2G CreateCoCoverage2G(INTEN.CoEquivalenceCoverage coverage2G, int subLineBusinessCd)
        {
            if (coverage2G == null)
                return null;
            return new ParamCoCoverage2G()
            {
                Id = Convert.ToInt32(coverage2G.Coverage2gId),
                InsuredObjectId = Convert.ToInt32(coverage2G.InsuredObject2gId),
                LineBusinessId = Convert.ToInt32(subLineBusinessCd),
                SubLineBusinessId = Convert.ToInt32(coverage2G.SubLineBusinessCode)
            };
        }
        #endregion

        #region Limit Rc

        public static List<ParamLimitRc> CreateLimitsRc(BusinessCollection businessCollection)
        {
            List<ParamLimitRc> paramLimitRc = new List<ParamLimitRc>();

            foreach (COMMEN.CoLimitsRc entity in businessCollection)
            {
                paramLimitRc.Add(ModelAssembler.CreateLimitRc(entity));
            }

            return paramLimitRc;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="coLimitsRc">Entidad CoLimitsRc</param>
        /// <returns>Retorna modelo ParamLimitRc</returns>
        public static ParamLimitRc CreateLimitRc(COMMEN.CoLimitsRc coLimitsRc)
        {
            return new ParamLimitRc
            {
                Id = coLimitsRc.LimitRcCode,
                Limit1 = coLimitsRc.Limit1.Value,
                Limit2 = coLimitsRc.Limit2.Value,
                Limit3 = coLimitsRc.Limit3.Value,
                LimitUnique = coLimitsRc.LimitUnique.Value.ToString(),
                Description = coLimitsRc.Description
            };
        }

        #endregion

        #region Metodos_Pago

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <returns></returns>
        public static Result<BmParamPaymentMethodType, ErrorModel> CreatePaymentMethodType(PAYM.PaymentMethodType paymentMethodType)
        {
            Result<BmParamPaymentMethodType, ErrorModel> result = BmParamPaymentMethodType.CreateParamPayMethodType(paymentMethodType.PaymentMethodTypeCode, paymentMethodType.Description);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        public static Result<BmParamPaymentMethod, ErrorModel> CreateParamPaymentMethod(PAYM.PaymentMethod paymentMethod)
        {
            PaymentMethodType paymentMethodType = new PaymentMethodType(paymentMethod.PaymentMethodTypeCode);
            Result<BmParamPaymentMethodType, ErrorModel> Type = CreatePaymentMethodType(paymentMethodType);
            BmParamPaymentMethodType paramPaymentMethodType = ((ResultValue<BmParamPaymentMethodType, ErrorModel>)Type).Value;
            Result<BmParamPaymentMethod, ErrorModel> result = BmParamPaymentMethod.CreateParamPaymentMethod(paymentMethod.PaymentMethodCode, paymentMethod.Description, paymentMethod.TinyDescription, paymentMethod.SmallDescription, paramPaymentMethodType);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static Result<List<BmParamPaymentMethod>, ErrorModel> CreateParamPaymentMethods(BusinessCollection businessCollection)
        {
            List<BmParamPaymentMethod> paramPaymentMethods = new List<BmParamPaymentMethod>();
            List<string> errorModelListDescription = new List<string>();
            Result<BmParamPaymentMethod, ErrorModel> result;
            foreach (PAYM.PaymentMethod entityParamPaymentMethod in businessCollection)
            {
                result = CreateParamPaymentMethod(entityParamPaymentMethod);
                if (result is ResultError<BmParamPaymentMethod, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad metodo de pago a modelo de negocio.");
                    return new ResultError<List<BmParamPaymentMethod>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    BmParamPaymentMethod resultValue = (result as ResultValue<BmParamPaymentMethod, ErrorModel>).Value;
                    paramPaymentMethods.Add(resultValue);
                }
            }

            return new ResultValue<List<BmParamPaymentMethod>, ErrorModel>(paramPaymentMethods);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static Result<List<BmParamPaymentMethodType>, ErrorModel> CreatePaymentMethodTypes(BusinessCollection businessCollection)
        {
            List<BmParamPaymentMethodType> paramPaymentMethodTypess = new List<BmParamPaymentMethodType>();
            List<string> errorModelListDescription = new List<string>();
            Result<BmParamPaymentMethodType, ErrorModel> result;
            foreach (PAYM.PaymentMethodType entityParamPaymentMethodType in businessCollection)
            {
                result = CreatePaymentMethodType(entityParamPaymentMethodType);
                if (result is ResultError<BmParamPaymentMethodType, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad metodo de pago a modelo de negocio.");
                    return new ResultError<List<BmParamPaymentMethodType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    BmParamPaymentMethodType resultValue = (result as ResultValue<BmParamPaymentMethodType, ErrorModel>).Value;
                    paramPaymentMethodTypess.Add(resultValue);
                }
            }

            return new ResultValue<List<BmParamPaymentMethodType>, ErrorModel>(paramPaymentMethodTypess);
        }
        #endregion Metodos_Pago
        #region Technical Plan


        public static Result<List<ParamTechnicalPlan>, ErrorModel> CreateTechnicalPlans(BusinessCollection technicalPlanCollection, BusinessCollection coveredRiskTypeCollection)
        {
            List<string> errorModelListDescription = new List<string>();
            List<ParamTechnicalPlan> technicalPlans = new List<ParamTechnicalPlan>();
            Result<ParamTechnicalPlan, ErrorModel> resultTechnicalPlan;
            Result<ParamCoveredRiskType, ErrorModel> resultCoveredRiskType;
            List<PAREN.CoveredRiskType> coveredRiskTypes = coveredRiskTypeCollection.Cast<PAREN.CoveredRiskType>().ToList();
            ParamCoveredRiskType coveredRiskType;
            foreach (PRODEN.TechnicalPlan entityTechnicalPlan in technicalPlanCollection)
            {
                PAREN.CoveredRiskType item = coveredRiskTypes.First(x => x.CoveredRiskTypeCode == entityTechnicalPlan.CoveredRiskTypeCode);
                if (item == null)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeMappingEntityError); //pendiente error
                    return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                resultCoveredRiskType = ParamCoveredRiskType.CreateParamCoveredRiskType(item.CoveredRiskTypeCode, item.SmallDescription);
                if (resultCoveredRiskType is ResultError<ParamCoveredRiskType, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeMappingEntityError); //pendiente error
                    return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                coveredRiskType = (resultCoveredRiskType as ResultValue<ParamCoveredRiskType, ErrorModel>).Value;
                resultTechnicalPlan = ParamTechnicalPlan.CreateParamTechnicalPlan(entityTechnicalPlan.TechnicalPlanId, entityTechnicalPlan.Description, entityTechnicalPlan.SmallDescription, coveredRiskType, entityTechnicalPlan.CurrentFrom, entityTechnicalPlan.CurrentTo);
                if (resultTechnicalPlan is ResultError<ParamTechnicalPlan, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeMappingEntityError); //pendiente error
                    return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                ParamTechnicalPlan resultValue = (resultTechnicalPlan as ResultValue<ParamTechnicalPlan, ErrorModel>).Value;
                technicalPlans.Add(resultValue);
            }
            return new ResultValue<List<ParamTechnicalPlan>, ErrorModel>(technicalPlans);
        }



        /// <summary>
        /// Convierte entidad a modelo negocio
        /// </summary>
        /// <param name="businessCollection">collecion Coverage</param>
        /// <returns>modelo de negocio Coverage</returns>
        public static Result<List<ParamAllyCoverage>, ErrorModel> CreateAllyParamCoverages(BusinessCollection coverages, BusinessCollection allyCoverages)
        {
            List<ParamAllyCoverage> alliedCoverages = new List<ParamAllyCoverage>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamAllyCoverage, ErrorModel> result;
            List<QUOEN.AllyCoverage> alliedCoveragesPercentage = allyCoverages.Cast<QUOEN.AllyCoverage>().ToList();

            foreach (QUOEN.Coverage entityAllyCovered in coverages)
            {
                decimal coveragePercentage = alliedCoveragesPercentage.First(x => x.AllyCoverageId == entityAllyCovered.CoverageId).CoveragePercentage;
                result = ParamAllyCoverage.CreateParamAllyCoverage(entityAllyCovered.CoverageId, entityAllyCovered.PrintDescription, coveragePercentage);
                if (result is ResultError<ParamAllyCoverage, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeMappingEntityError); //pendiente error
                    return new ResultError<List<ParamAllyCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamAllyCoverage resultValue = (result as ResultValue<ParamAllyCoverage, ErrorModel>).Value;
                    alliedCoverages.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamAllyCoverage>, ErrorModel>(alliedCoverages);
        }


        /// <summary>
        /// Convierte entidad a modelo de negocio objeto del seguro
        /// </summary>
        /// <param name="insuredObject">Objeto del seguro => ENT</param>
        /// <returns>objeto del seguro => MOD-B</returns>
        public static ParamInsuredObject CreateParamInsuredObject(QUOEN.InsuredObject insuredObject)
        {
            return new ParamInsuredObject
            {
                Id = insuredObject.InsuredObjectId,
                Description = insuredObject.Description,
                IsDeclarative = insuredObject.IsDeclarative,
                SmallDescription = insuredObject.SmallDescription
            };
        }

        public static Result<ParamTechnicalPlan, ErrorModel> CreateParamTechnicalPlan(PRODEN.TechnicalPlan technicalPlan)
        {
            DAOs.CoveredRiskTypeDAO coveredRiskTypeDAO = new DAOs.CoveredRiskTypeDAO();
            Result<List<ParamCoveredRiskType>, ErrorModel> resultCoveredRiskTypes;
            Result<ParamTechnicalPlan, ErrorModel> resultTechnicalPlan;
            ResultError<List<ParamCoveredRiskType>, ErrorModel> errorCoveredRiskTypes;
            ResultError<ParamTechnicalPlan, ErrorModel> errorTechnicalPlan;
            ResultValue<List<ParamCoveredRiskType>, ErrorModel> valueCoveredRiskTypes;

            resultCoveredRiskTypes = coveredRiskTypeDAO.GetCoveredRiskTypes();

            if (resultCoveredRiskTypes is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
            {
                errorCoveredRiskTypes = (resultCoveredRiskTypes as ResultError<List<ParamCoveredRiskType>, ErrorModel>);
                return new ResultError<ParamTechnicalPlan, ErrorModel>(ErrorModel.CreateErrorModel(errorCoveredRiskTypes.Message.ErrorDescription, errorCoveredRiskTypes.Message.ErrorType, errorCoveredRiskTypes.Message.Exception));
            }
            valueCoveredRiskTypes = (resultCoveredRiskTypes as ResultValue<List<ParamCoveredRiskType>, ErrorModel>);
            ParamCoveredRiskType coveredRiskType = valueCoveredRiskTypes.Value.Where(x => x.Id == technicalPlan.CoveredRiskTypeCode).FirstOrDefault();

            if (coveredRiskType == null)
            {
                return new ResultError<ParamTechnicalPlan, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "" }, ErrorType.BusinessFault, null)); //pendiente
            }

            resultTechnicalPlan = ParamTechnicalPlan.CreateParamTechnicalPlan(technicalPlan.TechnicalPlanId, technicalPlan.Description, technicalPlan.SmallDescription, coveredRiskType, technicalPlan.CurrentFrom, technicalPlan.CurrentTo);
            if (resultTechnicalPlan is ResultError<ParamTechnicalPlan, ErrorModel>)
            {
                errorTechnicalPlan = (resultTechnicalPlan as ResultError<ParamTechnicalPlan, ErrorModel>);
                return new ResultError<ParamTechnicalPlan, ErrorModel>(ErrorModel.CreateErrorModel(errorTechnicalPlan.Message.ErrorDescription, errorTechnicalPlan.Message.ErrorType, errorTechnicalPlan.Message.Exception));
            }
            return (resultTechnicalPlan as ResultValue<ParamTechnicalPlan, ErrorModel>);
        }

        public static Result<ParamTechnicalPlanCoverage, ErrorModel> CreateParamTechnicalPlanCoverage(PRODEN.TechnicalPlanCoverage technicalPlanCoverage)
        {
            Result<ParamCoverage, ErrorModel> resultCoverage;
            ResultError<ParamCoverage, ErrorModel> errorCoverage;

            Result<ParamCoverage, ErrorModel> resultPrincipalCoverage;
            ResultError<ParamCoverage, ErrorModel> errorPrincipalCoverage;

            ParamCoverage paramCoverage;
            ParamCoverage paramPrincipalCoverage = null;

            CoverageDAO coverageDAO = new CoverageDAO();
            resultCoverage = coverageDAO.GetParamCoverageById(technicalPlanCoverage.CoverageId);
            if (resultCoverage is ResultError<ParamCoverage, ErrorModel>)
            {
                errorCoverage = (resultCoverage as ResultError<ParamCoverage, ErrorModel>);
                return new ResultError<ParamTechnicalPlanCoverage, ErrorModel>(ErrorModel.CreateErrorModel(errorCoverage.Message.ErrorDescription, errorCoverage.Message.ErrorType, errorCoverage.Message.Exception));
            }
            paramCoverage = (resultCoverage as ResultValue<ParamCoverage, ErrorModel>).Value;

            if (paramCoverage == null)
            {
                return new ResultError<ParamTechnicalPlanCoverage, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "" }, ErrorType.TechnicalFault, null));
            }

            ParamInsuredObject paramInsuredObject = new ParamInsuredObject()
            {
                Id = paramCoverage.InsuredObjectDesc.Id,
                Description = paramCoverage.InsuredObjectDesc.Description,
            };

            if (technicalPlanCoverage.MainCoverageId > 0)
            {
                resultPrincipalCoverage = coverageDAO.GetParamCoverageById((int)technicalPlanCoverage.MainCoverageId);
                if (resultPrincipalCoverage is ResultError<ParamCoverage, ErrorModel>)
                {
                    errorPrincipalCoverage = (resultPrincipalCoverage as ResultError<ParamCoverage, ErrorModel>);
                    return new ResultError<ParamTechnicalPlanCoverage, ErrorModel>(ErrorModel.CreateErrorModel(errorPrincipalCoverage.Message.ErrorDescription, errorPrincipalCoverage.Message.ErrorType, errorPrincipalCoverage.Message.Exception));
                }
                paramPrincipalCoverage = (resultPrincipalCoverage as ResultValue<ParamCoverage, ErrorModel>).Value;
            }
            return ParamTechnicalPlanCoverage.CreateParamTechnicalPlanCoverage(paramInsuredObject, paramCoverage, paramPrincipalCoverage, technicalPlanCoverage.MainCoveragePercentage);
        }
        #endregion
        #region Niveles de influencia
        /// <summary>
        /// Convierte entidad a modelo negocio CompositionType
        /// </summary>
        /// <param name="businessCollection">collecion CompositionType</param>
        /// <returns>modelo de negocio CompositionType</returns>
        public static List<ParamComposition> CreateParametrizationComposition(BusinessCollection businessCollection)
        {
            List<ParamComposition> paramCompositions = new List<ParamComposition>();
            foreach (PAREN.CompositionType item in businessCollection)
            {
                paramCompositions.Add(CreateParamCompositionType(item));

            }

            return paramCompositions;
        }
        /// <summary>
        /// Convierte entidad a modelo CompositionType
        /// </summary>
        /// <param name="compositionType">entidad CompositionType</param>
        /// <returns>modelo ParamComposition</returns>
        private static ParamComposition CreateParamCompositionType(PAREN.CompositionType compositionType)
        {
            return new ParamComposition()
            {
                Id = compositionType.CompositionTypeCode,
                Description = compositionType.Description,
                SmallDescription = compositionType.SmallDescription
            };
        }



        #endregion

        #region AllyCoverage
        public static ParamQueryCoverage CreateAllyCoverage(AllyCoverage entity)
        {
            //ParamQueryCoverage result = new ParamQueryCoverage(entity.AllyCoverageId, entity.CoveragePercentage, null);
            ParamQueryCoverage result = new ParamQueryCoverage();
            return result;
        }

        public static List<ParamQueryCoverage> ConvertToModelAllyCoverage(System.Collections.IList allyList)
        {
            List<ParamQueryCoverage> allies = new List<ParamQueryCoverage>();
            foreach (AllyCoverage ally in allyList)
            {
                allies.Add(MappParamAllyCoverage(ally));
            }
            return allies;
        }

        /// <summary>
        /// MappParamAllyCoverage mapea de entity AllyCoverage a modelo de negocio company
        /// </summary>
        /// <param name="ally"></param>
        /// <returns></returns>
        public static ParamQueryCoverage MappParamAllyCoverage(AllyCoverage ally)
        {
            //ParamQueryCoverage paramAlly = new ParamQueryCoverage(ally.AllyCoverageId, ally.CoveragePercentage,null);
            ParamQueryCoverage paramAlly = new ParamQueryCoverage();
            

            return paramAlly;
        }
        #endregion

    }
}
