// -----------------------------------------------------------------------
// <copyright file="ModelsServicesAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers
{
    using AutoMapper;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.Error;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using COMMO = Sistran.Core.Application.CommonService.Models;
    using COMPAMO = Sistran.Core.Application.CommonParamService.Models;
    using ENUM = Sistran.Core.Application.UnderwritingParamService.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using MODCO = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using MODCOPA = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
    using MODUN = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using UTIERR = Sistran.Core.Application.Utilities.Error;
    using MOLUNPARAM = Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Application.Common.Entities;

    /// <summary>
    /// Convierte el modelo del negocio al modelo del servicio
    /// </summary>
    public static class ModelsServicesAssembler
    {
        #region ParamPaymentPlanServicesModel
        /// <summary>
        /// Convierte MOD-B de Plan de pago a MOD-S   
        /// </summary>
        /// <param name="parametrizationPaymentPlan">Plan de pago MOD-B</param>
        /// <returns>Plan de pago MOD-S</returns>
        public static PARUPSM.PaymentPlanServiceModel CreatePaymentPlanServiceModel(ParametrizationPaymentPlan parametrizationPaymentPlan)
        {
            var config = MapperCache.GetMapper<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceModel>(cfg =>
            {
                cfg.CreateMap<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceModel>();
            });
            PARUPSM.PaymentPlanServiceModel paramPaymentPlanServicesModel = config.Map<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceModel>(parametrizationPaymentPlan);
            paramPaymentPlanServicesModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            paramPaymentPlanServicesModel.QuotasServiceModel = CreateQuotaServiceModels(parametrizationPaymentPlan.ParametrizationQuotas);
            paramPaymentPlanServicesModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };
            return paramPaymentPlanServicesModel;
        }

        /// <summary>
        /// Convierte listado del MOD-B de Plan de pago a MOD-S
        /// </summary>
        /// <param name="parametrizationPaymentPlans">Plan de pago MOD-B</param>
        /// <returns>Plan de pago MOD-S</returns>
        public static List<PARUPSM.PaymentPlanServiceModel> CreatePaymentPlanServiceModels(List<ParametrizationPaymentPlan> parametrizationPaymentPlans)
        {
            List<PARUPSM.PaymentPlanServiceModel> paymentPlanServiceModels = new List<PARUPSM.PaymentPlanServiceModel>();
            foreach (var item in parametrizationPaymentPlans)
            {
                paymentPlanServiceModels.Add(CreatePaymentPlanServiceModel(item));
            }

            return paymentPlanServiceModels;
        }
        #endregion

        #region QuotaServiceModel
        /// <summary>
        /// Convierte MOD-B de las cuotas al MOD-S
        /// </summary>
        /// <param name="quota">Cuota MOD-B</param>
        /// <returns>Cuota MOD-S</returns>
        public static PARUPSM.QuotaServiceModel CreateQuotaServiceModel(ParametrizationQuota quota)
        {
            PARUPSM.QuotaServiceModel quotaServiceModel = new PARUPSM.QuotaServiceModel();
            quotaServiceModel.Id = quota.Id;
            quotaServiceModel.Number = quota.Number;
            quotaServiceModel.Percentage = quota.Percentage;
            quotaServiceModel.GapQuantity = Convert.ToInt32(quota.GapQuantity);
            quotaServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            quotaServiceModel.QuotaComponentTypeServiceModel = new List<QuotaComponentTypeServiceModel>();
            if (quota.ListQuotaComponent != null)
            {
                foreach (ParametrizacionQuotaTypeComponent item in quota.ListQuotaComponent)
                {
                    quotaServiceModel.QuotaComponentTypeServiceModel.Add(CreatequotaComponentTypeServiceModel(item));
                }
            }

            return quotaServiceModel;
        }


        public static QuotaComponentTypeServiceModel CreatequotaComponentTypeServiceModel(ParametrizacionQuotaTypeComponent quotaComponentTypeServiceModel)
        {
            QuotaComponentTypeServiceModel quotaTypeComponent = new QuotaComponentTypeServiceModel()
            {
                Id = quotaComponentTypeServiceModel.Id,
                Value = quotaComponentTypeServiceModel.Value
            };

            return quotaTypeComponent;
        }
        /// <summary>
        /// Convierte el MOD-B de las cuotas al MOD-S
        /// </summary>
        /// <param name="quotas">Listado cuotas MOD-B</param>
        /// <returns>Listado cuotas MOD-S</returns>
        public static List<PARUPSM.QuotaServiceModel> CreateQuotaServiceModels(List<ParametrizationQuota> quotas)
        {
            List<PARUPSM.QuotaServiceModel> quotaServiceModel = new List<PARUPSM.QuotaServiceModel>();
            foreach (var item in quotas)
            {
                quotaServiceModel.Add(CreateQuotaServiceModel(item));
            }

            return quotaServiceModel;
        }

        /// <summary>
        /// Crea el modelo
        /// </summary>
        /// <param name="serviceModel">Modelo de servicio</param>
        /// <returns>Modelo lectura</returns>
        public static UTIERR.Result<ParamVehicleTypeBody, UTIERR.ErrorModel> CreateVehicleTypeBody(PARUPSM.VehicleTypeServiceModel serviceModel)
        {
            List<string> errorCreateModel = new List<string>();
            UTIERR.Result<ParamVehicleType, UTIERR.ErrorModel> vehicleType = ParamVehicleType.CreateParamVehicleType(serviceModel.Id, serviceModel.Description, serviceModel.SmallDescription, serviceModel.IsEnable, serviceModel.IsTruck);
            if (vehicleType is UTIERR.ResultError<ParamVehicleType, UTIERR.ErrorModel>)
            {
                errorCreateModel.AddRange(((UTIERR.ResultError<ParamVehicleType, UTIERR.ErrorModel>)vehicleType).Message.ErrorDescription);
            }
            List<ParamVehicleBody> paramVehicleBodies = new List<ParamVehicleBody>();
            if (serviceModel.VehicleBodyServiceQueryModel != null)
            {
                foreach (PARUPSM.VehicleBodyServiceQueryModel item in serviceModel.VehicleBodyServiceQueryModel)
                {
                    UTIERR.ResultValue<ParamVehicleBody, UTIERR.ErrorModel> vehicleBody = (UTIERR.ResultValue<ParamVehicleBody, UTIERR.ErrorModel>)ParamVehicleBody.GetParamVehicleBody(item.Id, item.Description);
                    paramVehicleBodies.Add(vehicleBody.Value);
                }
            }
            if (errorCreateModel.Count > 0)
            {
                return new UTIERR.ResultError<ParamVehicleTypeBody, UTIERR.ErrorModel>(UTIERR.ErrorModel.CreateErrorModel(errorCreateModel, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            //((UTIERR.ResultValue<ParamVehicleType, UTIERR.ErrorModel>)vehicleType).Value.ExtendedProperties.Add(new Extensions.ExtendedProperty
            //{
            //    Name = "IsElectronicPolicy",
            //    Value = serviceModel.IsElectronicPolicy
            //});

            return ParamVehicleTypeBody.CreateParamVehicleBody(((UTIERR.ResultValue<ParamVehicleType, UTIERR.ErrorModel>)vehicleType).Value, paramVehicleBodies);
        }
        #endregion

        #region DetailType
        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="detailTypes">Modelo DetailType</param>
        /// <returns>Modelo DetailTypeServiceQueryModel</returns>
        public static PARUPSM.DetailTypeServiceQueryModel CreateDetailTypeServiceModel(MOLUNPARAM.DetailType detailTypes)
        {
            var config = MapperCache.GetMapper<MOLUNPARAM.DetailType, PARUPSM.DetailTypeServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<MOLUNPARAM.DetailType, PARUPSM.DetailTypeServiceQueryModel>();
            });
            PARUPSM.DetailTypeServiceQueryModel detailTypeServiceQueryModel = config.Map<MOLUNPARAM.DetailType, PARUPSM.DetailTypeServiceQueryModel>(detailTypes);
            detailTypeServiceQueryModel.Id = detailTypes.DetailTypeCode;
            detailTypeServiceQueryModel.Description = detailTypes.Description;
            return detailTypeServiceQueryModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="detailTypes">Modelo DetailType</param>
        /// <returns>Modelo DetailTypeServiceQueryModel</returns>
        public static List<PARUPSM.DetailTypeServiceQueryModel> CreateDetailTypesServiceModels(List<MOLUNPARAM.DetailType> detailTypes)
        {
            List<PARUPSM.DetailTypeServiceQueryModel> detailTypeServiceQueryModel = new List<PARUPSM.DetailTypeServiceQueryModel>();
            foreach (var item in detailTypes)
            {
                detailTypeServiceQueryModel.Add(CreateDetailTypeServiceModel(item));
            }

            return detailTypeServiceQueryModel;
        }
        #endregion DetailType
        #region Detail

        public static List<PARUPSM.DetailServiceModel> CreateDetailsServiceModel(List<ParametrizationDetail> parametrizationDetail)
        {
            List<PARUPSM.DetailServiceModel> detailServiceModel = new List<PARUPSM.DetailServiceModel>();
            foreach (var item in parametrizationDetail)
            {
                detailServiceModel.Add(CreateDetailServiceModel(item));
            }

            return detailServiceModel;
        }


        public static PARUPSM.DetailServiceModel CreateDetailServiceModel(ParametrizationDetail detail)
        {
            return new PARUPSM.DetailServiceModel
            {
                Id = detail.Id,
                Description = detail.Description,
                DetailType = new PARUPSM.DetailTypeServiceQueryModel { Id = detail.DetailType.Id, Description = detail.DetailType.Description },
                Enabled = detail.Enabled,
                Rate = detail.Rate,
                RateType = detail.RateType,
                SublimitAmt = detail.SublimitAmt
            };
        }
        #endregion

        #region Deductible
        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="deductibleUnit">Modelo DeductibleUnit</param>
        /// <returns>Modelo DeductibleUnitServiceQueryModel</returns>
        public static PARUPSM.DeductibleUnitServiceQueryModel CreateDeductibleUnitServiceModel(UNDMO.DeductibleUnit deductibleUnit)
        {
            var config = MapperCache.GetMapper<UNDMO.DeductibleUnit, PARUPSM.DeductibleUnitServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<UNDMO.DeductibleUnit, PARUPSM.DeductibleUnitServiceQueryModel>();
            });
            PARUPSM.DeductibleUnitServiceQueryModel paramPaymentPlanServicesModel = config.Map<UNDMO.DeductibleUnit, PARUPSM.DeductibleUnitServiceQueryModel>(deductibleUnit);
            paramPaymentPlanServicesModel.Id = deductibleUnit.Id;
            paramPaymentPlanServicesModel.Description = deductibleUnit.Description;
            return paramPaymentPlanServicesModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="deductibles">Modelo DeductibleUnit</param>
        /// <returns>Modelo DeductibleUnitServiceQueryModel</returns>
        public static List<PARUPSM.DeductibleUnitServiceQueryModel> CreateDeductibleUnitServiceModels(List<UNDMO.DeductibleUnit> deductibles)
        {
            List<PARUPSM.DeductibleUnitServiceQueryModel> deductibleUnitServiceModel = new List<PARUPSM.DeductibleUnitServiceQueryModel>();
            foreach (var item in deductibles)
            {
                deductibleUnitServiceModel.Add(CreateDeductibleUnitServiceModel(item));
            }

            return deductibleUnitServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="deductibleSubject">Modelo deductibleSubject</param>
        /// <returns>Modelo DeductibleSubjectServiceQueryModel</returns>
        public static PARUPSM.DeductibleSubjectServiceQueryModel CreateDeductibleSubjectServiceModel(UNDMO.DeductibleSubject deductibleSubject)
        {
            var config = MapperCache.GetMapper<UNDMO.DeductibleSubject, PARUPSM.DeductibleSubjectServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<UNDMO.DeductibleSubject, PARUPSM.DeductibleSubjectServiceQueryModel>();
            });
            PARUPSM.DeductibleSubjectServiceQueryModel deductibleSubjectServiceModel = config.Map<UNDMO.DeductibleSubject, PARUPSM.DeductibleSubjectServiceQueryModel>(deductibleSubject);
            deductibleSubjectServiceModel.Id = deductibleSubject.Id;
            deductibleSubjectServiceModel.Description = deductibleSubject.Description;
            return deductibleSubjectServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="deductibles">Modelo DeductibleSubject</param>
        /// <returns>Modelo DeductibleSubjectServiceQueryModel</returns>
        public static List<PARUPSM.DeductibleSubjectServiceQueryModel> CreateDeductibleSubjectServiceModels(List<UNDMO.DeductibleSubject> deductibles)
        {
            List<PARUPSM.DeductibleSubjectServiceQueryModel> deductibleUnitServiceModel = new List<PARUPSM.DeductibleSubjectServiceQueryModel>();
            foreach (var item in deductibles)
            {
                deductibleUnitServiceModel.Add(CreateDeductibleSubjectServiceModel(item));
            }

            return deductibleUnitServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="currency">Modelo Currency</param>
        /// <returns>Modelo CurrencyServiceQueryModel</returns>
        public static PARUPSM.CurrencyServiceQueryModel CreateCurrencyServiceModel(COMMO.Currency currency)
        {
            var config = MapperCache.GetMapper<COMMO.Currency, PARUPSM.CurrencyServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<COMMO.Currency, PARUPSM.CurrencyServiceQueryModel>();
            });
            PARUPSM.CurrencyServiceQueryModel currencyServiceModel = config.Map<COMMO.Currency, PARUPSM.CurrencyServiceQueryModel>(currency);
            currencyServiceModel.Id = currency.Id;
            currencyServiceModel.Description = currency.Description;
            return currencyServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="currencies">Modelo Currency</param>
        /// <returns>Modelo CurrencyServiceQueryModel</returns>
        public static List<PARUPSM.CurrencyServiceQueryModel> CreateCurrencyServiceModels(List<COMMO.Currency> currencies)
        {
            List<PARUPSM.CurrencyServiceQueryModel> currencyServiceModel = new List<PARUPSM.CurrencyServiceQueryModel>();
            foreach (var item in currencies)
            {
                currencyServiceModel.Add(CreateCurrencyServiceModel(item));
            }

            return currencyServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="deductible">Modelo Deductible</param>
        /// <returns>Modelo DeductibleServiceModel</returns>
        public static PARUPSM.DeductibleServiceModel CreateDeductibleServiceModel(UNDMO.Deductible deductible)
        {
            PARUPSM.DeductibleServiceModel deductibleServiceModel = new PARUPSM.DeductibleServiceModel();
            deductibleServiceModel.Id = deductible.Id;
            deductibleServiceModel.Description = deductible.Description;
            deductibleServiceModel.AccDeductAmt = deductible.AccDeductAmt;
            deductibleServiceModel.Currency = new PARUPSM.CurrencyServiceQueryModel { Id = deductible.Currency.Id, Description = deductible.Currency.Description };
            deductibleServiceModel.MinDeductValue = deductible.MinDeductValue;
            deductibleServiceModel.MaxDeductValue = deductible.MaxDeductValue;
            deductibleServiceModel.DeductibleSubject = new PARUPSM.DeductibleSubjectServiceQueryModel
            {
                Id = deductible.DeductibleSubject.Id,
                Description = deductible.DeductibleSubject.Description
            };
            deductibleServiceModel.DeductibleUnit = new PARUPSM.DeductibleUnitServiceQueryModel
            {
                Id = deductible.DeductibleUnit.Id,
                Description = deductible.DeductibleUnit.Description
            };
            deductibleServiceModel.DeductPremiumAmount = deductible.DeductPremiumAmount;
            deductibleServiceModel.DeductValue = deductible.DeductValue;
            deductibleServiceModel.IsDefault = deductible.IsDefault;
            deductibleServiceModel.LineBusiness = new PARUPSM.LineBusinessServiceQueryModel
            {
                Id = deductible.LineBusiness.Id,
                Description = deductible.LineBusiness.Description
            };
            deductibleServiceModel.MaxDeductibleSubject = new PARUPSM.DeductibleSubjectServiceQueryModel
            {
                Id = deductible.MaxDeductibleSubject.Id,
                Description = deductible.MaxDeductibleSubject.Description
            };
            deductibleServiceModel.MinDeductibleSubject = new PARUPSM.DeductibleSubjectServiceQueryModel
            {
                Id = deductible.MinDeductibleSubject.Id,
                Description = deductible.MinDeductibleSubject.Description
            };
            deductibleServiceModel.MaxDeductibleUnit = new PARUPSM.DeductibleUnitServiceQueryModel
            {
                Id = deductible.MaxDeductibleUnit.Id,
                Description = deductible.MaxDeductibleUnit.Description
            };
            deductibleServiceModel.MinDeductibleUnit = new PARUPSM.DeductibleUnitServiceQueryModel
            {
                Id = deductible.MinDeductibleUnit.Id,
                Description = deductible.MinDeductibleUnit.Description
            };
            deductibleServiceModel.Rate = deductible.Rate;
            deductibleServiceModel.RateType = (ENUMSM.RateType)deductible.RateType;
            deductibleServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            deductibleServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };
            return deductibleServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="deductible">Modelo Deductible</param>
        /// <returns>Modelo DeductibleServiceModel</returns>
        public static List<PARUPSM.DeductibleServiceModel> CreateDeductibleServiceModels(List<UNDMO.Deductible> deductible)
        {
            List<PARUPSM.DeductibleServiceModel> deductibleServiceModel = new List<PARUPSM.DeductibleServiceModel>();
            foreach (var item in deductible)
            {
                deductibleServiceModel.Add(CreateDeductibleServiceModel(item));
            }

            return deductibleServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="lineBusiness">Modelo LineBusiness</param>
        /// <returns>Modelo LineBusinessServiceQueryModel</returns>
        public static PARUPSM.LineBusinessServiceQueryModel CreateLineBusinessServiceModel(ParamLineBusinessModel lineBusiness)
        {
            PARUPSM.LineBusinessServiceQueryModel lineBusinessServiceModel = new PARUPSM.LineBusinessServiceQueryModel();
            lineBusinessServiceModel.Id = lineBusiness.Id;
            lineBusinessServiceModel.Description = lineBusiness.Description;

            return lineBusinessServiceModel;
        }

        /// <summary>
        /// Convierte a servicequerymodel
        /// </summary>
        /// <param name="lineBusiness">Modelo LineBusiness</param>
        /// <returns>Modelo LineBusinessServiceQueryModel</returns>
        public static List<PARUPSM.LineBusinessServiceQueryModel> CreateLineBusinessServiceModels(List<ParamLineBusinessModel> lineBusiness)
        {
            List<PARUPSM.LineBusinessServiceQueryModel> lineBusinessServiceModel = new List<PARUPSM.LineBusinessServiceQueryModel>();
            foreach (var item in lineBusiness)
            {
                lineBusinessServiceModel.Add(CreateLineBusinessServiceModel(item));
            }

            return lineBusinessServiceModel;
        }
        #endregion

        /// <summary>
        /// Mapear modelos de negocio ParamCoveredRiskType a modelos de Servicio CoveredRiskTypesServiceModel.
        /// </summary>
        /// <param name="paramCoveredRiskTypes">Modelos de negocio ParamCoveredRiskType.</param>
        /// <returns>Modelos de Servicio CoveredRiskTypesServiceModel</returns>
        public static MODUN.CoveredRiskTypesServiceModel MappCoveredRiskTypes(List<ParamCoveredRiskType> paramCoveredRiskTypes)
        {
            MODUN.CoveredRiskTypesServiceModel coveredRiskTypesServiceModel = new MODUN.CoveredRiskTypesServiceModel();
            List<MODUN.CoveredRiskTypeServiceModel> listCoveredRiskTypeServiceModel = new List<MODUN.CoveredRiskTypeServiceModel>();
            foreach (ParamCoveredRiskType coveredRiskTypeBusinessModel in paramCoveredRiskTypes)
            {
                MODUN.CoveredRiskTypeServiceModel itemCoveredRiskTypeServiceModel = new MODUN.CoveredRiskTypeServiceModel();
                itemCoveredRiskTypeServiceModel.Id = coveredRiskTypeBusinessModel.Id;
                itemCoveredRiskTypeServiceModel.SmallDescription = coveredRiskTypeBusinessModel.SmallDescription;
                listCoveredRiskTypeServiceModel.Add(itemCoveredRiskTypeServiceModel);
            }

            coveredRiskTypesServiceModel.ErrorDescription = new List<string>();
            coveredRiskTypesServiceModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;

            coveredRiskTypesServiceModel.CoveredRiskTypeServiceModel = listCoveredRiskTypeServiceModel;

            return coveredRiskTypesServiceModel;
        }

        #region Surcharge
        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="surcharge">parametro de recargos</param>       
        /// <returns> modelo de negocio </returns>
        public static List<PARUPSM.SurchargeServiceModel> CreateSurchargesServiceModels(List<ParamSurcharge> surcharge)
        {
            List<PARUPSM.SurchargeServiceModel> surchargeServiceModel = new List<PARUPSM.SurchargeServiceModel>();
            foreach (var item in surcharge)
            {
                surchargeServiceModel.Add(CreateSurchargeServiceModel(item));
            }

            return surchargeServiceModel;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="surcharge">parametro de recargo</param>       
        /// <returns> modelo de negocio </returns>
        public static PARUPSM.SurchargeServiceModel CreateSurchargeServiceModel(ParamSurcharge surcharge)
        {

            var config = MapperCache.GetMapper<ParamSurcharge, PARUPSM.SurchargeServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamSurcharge, PARUPSM.SurchargeServiceModel>();
            });
            PARUPSM.SurchargeServiceModel surchargeServiceModel = config.Map<ParamSurcharge, PARUPSM.SurchargeServiceModel>(surcharge);
            surchargeServiceModel.Id = surcharge.Id;
            surchargeServiceModel.Description = surcharge.Description;
            surchargeServiceModel.TinyDescription = surcharge.TinyDescription;
            surchargeServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            surchargeServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };

            return surchargeServiceModel;
        }
        #endregion

        #region Discount
        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="discount">parametro de componetes</param>       
        /// <returns> modelo de negocio </returns>
        public static List<PARUPSM.DiscountServiceModel> CreateDiscountsServiceModels(List<ParamDiscount> discount)
        {
            List<PARUPSM.DiscountServiceModel> discountServiceModel = new List<PARUPSM.DiscountServiceModel>();
            foreach (var item in discount)
            {
                discountServiceModel.Add(CreateDiscountServiceModel(item));
            }

            return discountServiceModel;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="discount">parametro de descuento</param>       
        /// <returns> modelo de negocio </returns>
        public static PARUPSM.DiscountServiceModel CreateDiscountServiceModel(ParamDiscount discount)
        {
            var config = MapperCache.GetMapper<ParamDiscount, PARUPSM.DiscountServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamDiscount, PARUPSM.DiscountServiceModel>();
            });

            PARUPSM.DiscountServiceModel discountServiceModel = config.Map<ParamDiscount, PARUPSM.DiscountServiceModel>(discount);

            discountServiceModel.Id = discount.Id;
            discountServiceModel.Description = discount.Description;
            discountServiceModel.TinyDescription = discount.TinyDescription;
            discountServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            discountServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };

            return discountServiceModel;
        }
        #endregion

        #region SubLineBusiness
        public static PARUPSM.SubLineBranchServiceModel CreateSubLineBusinessServiceModel(COMMO.SubLineBusiness subLine)
        {

            var config = MapperCache.GetMapper<COMMO.SubLineBusiness, PARUPSM.SubLineBranchServiceModel>(cfg =>
            {
                cfg.CreateMap<COMMO.SubLineBusiness, PARUPSM.SubLineBranchServiceModel>();
            });

            PARUPSM.SubLineBranchServiceModel SublineBusinessServiceModel = config.Map<COMMO.SubLineBusiness, PARUPSM.SubLineBranchServiceModel>(subLine);
            SublineBusinessServiceModel.Id = subLine.Id;
            SublineBusinessServiceModel.Description = subLine.Description;
            SublineBusinessServiceModel.SmallDescription = subLine.SmallDescription;
            SublineBusinessServiceModel.LineBusinessQuery = new PARUPSM.LineBusinessServiceQueryModel()
            {
                Id = subLine.LineBusinessId,
                Description = subLine.LineBusinessDescription
            };
            SublineBusinessServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            SublineBusinessServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };
            return SublineBusinessServiceModel;
        }


        /// <summary>
        /// Metodo lista de textos 
        /// </summary>
        /// <param name="text">Recibe text</param>
        /// <returns>retorna clauseTextServiceModel</returns>
        public static List<PARUPSM.SubLineBranchServiceModel> CreateSubLinesBusinessServiceModel(List<COMMO.SubLineBusiness> subLines)
        {
            List<PARUPSM.SubLineBranchServiceModel> subLinesServiceModel = new List<PARUPSM.SubLineBranchServiceModel>();
            foreach (var item in subLines)
            {
                subLinesServiceModel.Add(CreateSubLineBusinessServiceModel(item));
            }

            return subLinesServiceModel;
        }
        #endregion SubLineBusiness

        #region LineBusinessServiceQueryModel
        /// <summary>
        /// Convierte el MOD-B del subramo tecnico al MOD-S
        /// </summary>
        /// <param name="subLineBusiness">subramo tecnico MOD-S</param>
        /// <returns>SubLineBusinessServiceQueryModel MOD-S</returns>
        public static MODUN.SubLineBusinessServiceQueryModel CreateSubLineBusinessServiceQueryModel(COMMO.SubLineBusiness subLineBusiness)
        {
            var config = MapperCache.GetMapper<COMMO.SubLineBusiness, MODUN.SubLineBusinessServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<COMMO.SubLineBusiness, MODUN.SubLineBusinessServiceQueryModel>();
            });

            MODUN.SubLineBusinessServiceQueryModel subLineBusinessServiceModel = config.Map<COMMO.SubLineBusiness, MODUN.SubLineBusinessServiceQueryModel>(subLineBusiness);

            return subLineBusinessServiceModel;
        }

        /// <summary>
        /// Convierte la lista MOD-B de los subramos tecnicos al MOD-S
        /// </summary>
        /// <param name="subLineBusiness">listado de subramos tecnicos MOD-S</param>
        /// <returns>listado SubLineBusinessServiceQueryModel MOD-S</returns>
        public static List<MODUN.SubLineBusinessServiceQueryModel> CreateSubLineBusinessServiceQueryModels(List<COMMO.SubLineBusiness> subLineBusiness)
        {
            List<MODUN.SubLineBusinessServiceQueryModel> sublineBusinessServiceModel = new List<MODUN.SubLineBusinessServiceQueryModel>();
            foreach (var item in subLineBusiness)
            {
                sublineBusinessServiceModel.Add(CreateSubLineBusinessServiceQueryModel(item));
            }

            return sublineBusinessServiceModel;
        }
        #endregion

        #region PerilsServiceQueryModel
        /// <summary>
        /// Modelo de negocio a modelo de servicio peril
        /// </summary>
        /// <param name="paramPeril">M. negocio peril</param>
        /// <returns>M. servicio peril</returns>
        public static MODUN.PerilServiceQueryModel CreatePerilServiceQueryModel(ParamPeril paramPeril)
        {

            var config = MapperCache.GetMapper<ParamPeril, MODUN.PerilServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamPeril, MODUN.PerilServiceQueryModel>();
            });

            MODUN.PerilServiceQueryModel perilServiceQueryModel = config.Map<ParamPeril, MODUN.PerilServiceQueryModel>(paramPeril);
            return perilServiceQueryModel;
        }

        /// <summary>
        /// Convierte la lista MOD-B de los amparos al MOD-S
        /// </summary>
        /// <param name="paramPeril">amparos MOD-B</param>
        /// <returns>amparos MOD-S</returns>
        public static List<MODUN.PerilServiceQueryModel> CreatePerilsServiceQueryModels(List<ParamPeril> paramPeril)
        {
            List<MODUN.PerilServiceQueryModel> perilServiceQueryModels = new List<MODUN.PerilServiceQueryModel>();
            foreach (var item in paramPeril)
            {
                perilServiceQueryModels.Add(CreatePerilServiceQueryModel(item));
            }

            return perilServiceQueryModels;
        }
        #endregion

        #region PerilsServiceModel
        /// <summary>
        /// Convierte MOD-P de Amparos a MOD-S   
        /// </summary>
        /// <param name="peril">Amparos MOD-P</param>
        /// <returns>Amparos MOD-S</returns>
        public static PARUPSM.PerilServiceModel CreatePerilServiceModel(ParamPerilModel peril)
        {
            return new PARUPSM.PerilServiceModel()
            {
                Description = peril.Description,
                Id = peril.Id,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                }
            };
        }

        /// <summary>
        /// Convierte listado del MOD-P de Amparos a MOD-S
        /// </summary>
        /// <param name="perils">Amparos MOD-B</param>
        /// <returns>Amparos MOD-S</returns>
        public static List<PARUPSM.PerilServiceModel> CreatePerilsServiceModel(List<ParamPerilModel> perils)
        {
            List<PARUPSM.PerilServiceModel> perilsServiceModel = new List<PARUPSM.PerilServiceModel>();
            foreach (var item in perils)
            {
                perilsServiceModel.Add(CreatePerilServiceModel(item));
            }

            return perilsServiceModel;
        }
        #endregion

        #region PerilsServiceQueryModel
        /// <summary>
        /// Convierte MOD-B de objetos del seguro al MOD-S
        /// </summary>
        /// <param name="paramInsuredObjectDesc">Objeto del seguro-MOD-B</param>
        /// <returns>Objeto del seguro MOD-S</returns>
        public static MODUN.InsuredObjectServiceQueryModel CreateInsuredObjectServiceQueryModel(ParamInsuredObjectDesc paramInsuredObjectDesc)
        {
            var config = MapperCache.GetMapper<ParamInsuredObjectDesc, MODUN.InsuredObjectServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamInsuredObjectDesc, MODUN.InsuredObjectServiceQueryModel>();
            });

            MODUN.InsuredObjectServiceQueryModel insuredObjectServiceQueryModels = config.Map<ParamInsuredObjectDesc, MODUN.InsuredObjectServiceQueryModel>(paramInsuredObjectDesc);
            return insuredObjectServiceQueryModels;
        }

        /// <summary>
        /// Convierte el listado MOD-B de objetos del seguro al MOD-S
        /// </summary>
        /// <param name="paramInsuredObjectDesc">Objetos del seguro MOD-B</param>
        /// <returns>Objetos del seguro MOD-S</returns>
        public static List<MODUN.InsuredObjectServiceQueryModel> CreateInsuredObjectServiceQueryModels(List<ParamInsuredObjectDesc> paramInsuredObjectDesc)
        {
            List<MODUN.InsuredObjectServiceQueryModel> insuredObjectServiceQueryModel = new List<MODUN.InsuredObjectServiceQueryModel>();
            foreach (var item in paramInsuredObjectDesc)
            {
                insuredObjectServiceQueryModel.Add(CreateInsuredObjectServiceQueryModel(item));
            }

            return insuredObjectServiceQueryModel;
        }
        #endregion


        #region ClausesServiceQueryModel
        /// <summary>
        /// Modelo de negocio a modelo de servicio Clause
        /// </summary>
        /// <param name="paramClauseDesc">M. negocio Clause</param>
        /// <returns>M. servicio Clause</returns>
        public static PARUPSM.ClauseServiceQueryModel CreateClauseServiceQueryModel(ParamClauseDesc paramClauseDesc)
        {
            var config = MapperCache.GetMapper<ParamClauseDesc, PARUPSM.ClauseServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamClauseDesc, PARUPSM.ClauseServiceQueryModel>();
            });
            PARUPSM.ClauseServiceQueryModel clausesServiceQueryModel = config.Map<ParamClauseDesc, PARUPSM.ClauseServiceQueryModel>(paramClauseDesc);

            return clausesServiceQueryModel;
        }

        /// <summary>
        /// Modelo de negocio a modelo de servicio Clauses
        /// </summary>
        /// <param name="paramClauseDesc">M. negocio Clauses</param>
        /// <returns>M. servicio Clauses</returns>
        public static List<PARUPSM.ClauseServiceQueryModel> CreateClausesServiceQueryModels(List<ParamClauseDesc> paramClauseDesc)
        {
            List<PARUPSM.ClauseServiceQueryModel> clauseServiceQueryModels = new List<PARUPSM.ClauseServiceQueryModel>();
            foreach (ParamClauseDesc item in paramClauseDesc)
            {
                clauseServiceQueryModels.Add(CreateClauseServiceQueryModel(item));
            }

            return clauseServiceQueryModels;
        }
        #endregion

        #region DeductiblesServiceQueryModel
        /// <summary>
        /// Modelo de negocio a modelo de servicio Deductible
        /// </summary>
        /// <param name="paramDeductibleDesc">MOD negocio Deductible</param>
        /// <returns>MOD servicio Deductible</returns>
        public static PARUPSM.DeductibleServiceQueryModel CreateDeductibleServiceQueryModel(ParamDeductibleDesc paramDeductibleDesc)
        {
            var config = MapperCache.GetMapper<ParamDeductibleDesc, PARUPSM.DeductibleServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamDeductibleDesc, PARUPSM.DeductibleServiceQueryModel>();
            });
            PARUPSM.DeductibleServiceQueryModel deductibleServiceQueryModel = config.Map<ParamDeductibleDesc, PARUPSM.DeductibleServiceQueryModel>(paramDeductibleDesc);

            return deductibleServiceQueryModel;
        }

        /// <summary>
        /// Modelo de negocio a modelo de servicio Deductibles
        /// </summary>
        /// <param name="paramDeductibleDescs">MOD negocio Deductibles</param>
        /// <returns>MOD servicio Deductibles</returns>
        public static List<PARUPSM.DeductibleServiceQueryModel> CreateDeductibleServiceQueryModels(List<ParamDeductibleDesc> paramDeductibleDescs)
        {
            List<PARUPSM.DeductibleServiceQueryModel> deductibleServiceQueryModels = new List<PARUPSM.DeductibleServiceQueryModel>();
            foreach (ParamDeductibleDesc item in paramDeductibleDescs)
            {
                deductibleServiceQueryModels.Add(CreateDeductibleServiceQueryModel(item));
            }

            return deductibleServiceQueryModels;
        }

        #endregion

        #region DetailTypeServiceQueryModel
        /// <summary>
        /// Modelo de negocio a modelo de servicio DetailType
        /// </summary>
        /// <param name="detailType">MOD negocio DetailType</param>
        /// <returns>MOD de servicio DetailType</returns>
        public static PARUPSM.DetailTypeServiceQueryModel CreateDetailTypesServiceQueryModel(ParamDetailTypeDesc detailType)
        {
            var config = MapperCache.GetMapper<ParamDetailTypeDesc, PARUPSM.DetailTypeServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamDetailTypeDesc, PARUPSM.DetailTypeServiceQueryModel>();
            });
            PARUPSM.DetailTypeServiceQueryModel detailTypeServiceQueryModel = config.Map<ParamDetailTypeDesc, PARUPSM.DetailTypeServiceQueryModel>(detailType);

            return detailTypeServiceQueryModel;
        }

        /// <summary>
        /// Modelo de negocio a modelo de servicio DetailTypes
        /// </summary>
        /// <param name="detailTypes">MOD negocio DetailTypes</param>
        /// <returns>MOD servicio DetailTypes</returns>
        public static List<PARUPSM.DetailTypeServiceQueryModel> CreateDetailTypesServiceQueryModels(List<ParamDetailTypeDesc> detailTypes)
        {
            List<PARUPSM.DetailTypeServiceQueryModel> detailTypeServiceQueryModels = new List<PARUPSM.DetailTypeServiceQueryModel>();
            foreach (ParamDetailTypeDesc item in detailTypes)
            {
                detailTypeServiceQueryModels.Add(CreateDetailTypesServiceQueryModel(item));
            }

            return detailTypeServiceQueryModels;
        }
        #endregion

        #region CoverageServiceQueryModel
        /// <summary>
        /// Modelo de negocio a modelo de servicio Coverage
        /// </summary>
        /// <param name="paramCoverage">MOD. negocio Coverage</param>
        /// <returns>MOD. servicio Coverage</returns>
        public static PARUPSM.CoverageServiceModel CreateCoverageServiceModel(ParamCoverage paramCoverage)
        {
            var config = MapperCache.GetMapper<ParamCoverage, PARUPSM.CoverageServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamCoverage, PARUPSM.CoverageServiceModel>();
                cfg.CreateMap<ParamCoCoverage, PARUPSM.CoCoverageServiceModel>();
                cfg.CreateMap<ParamPeril, MODUN.PerilServiceQueryModel>();
                cfg.CreateMap<ParamInsuredObjectDesc, PARUPSM.InsuredObjectServiceQueryModel>();
                cfg.CreateMap<ParamLineBusinessDesc, PARUPSM.LineBusinessServiceQueryModel>();
                cfg.CreateMap<ParamSubLineBusinessDesc, MODUN.SubLineBusinessServiceQueryModel>();
                cfg.CreateMap<ParamCoCoverage2G, PARUPSM.Coverage2GServiceModel>();
            });
            PARUPSM.CoverageServiceModel coverageServiceQueryModel = config.Map<ParamCoverage, PARUPSM.CoverageServiceModel>(paramCoverage);
            coverageServiceQueryModel.StatusTypeService = ENUMSM.StatusTypeService.Original;
            coverageServiceQueryModel.CoCoverageServiceModel = config.Map<ParamCoCoverage, PARUPSM.CoCoverageServiceModel>(paramCoverage.CoCoverage);
            coverageServiceQueryModel.CoCoverageServiceModels = config.Map<List<ParamCoCoverage>, List<PARUPSM.CoCoverageServiceModel>>(paramCoverage.CoCoverages);
            if (coverageServiceQueryModel.CoCoverageServiceModel != null)
            {
                coverageServiceQueryModel.CoCoverageServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Original;
            }
            if (coverageServiceQueryModel.CoCoverageServiceModels != null)
            {
                foreach (var coCoverageService in coverageServiceQueryModel.CoCoverageServiceModels)
                {
                    coCoverageService.StatusTypeService = ENUMSM.StatusTypeService.Original;
                }
            }
            coverageServiceQueryModel.Peril = config.Map<ParamPeril, MODUN.PerilServiceQueryModel>(paramCoverage.Peril);
            coverageServiceQueryModel.InsuredObject = config.Map<ParamInsuredObjectDesc, PARUPSM.InsuredObjectServiceQueryModel>(paramCoverage.InsuredObjectDesc);
            coverageServiceQueryModel.LineBusiness = config.Map<ParamLineBusinessDesc, PARUPSM.LineBusinessServiceQueryModel>(paramCoverage.LineBusiness);
            coverageServiceQueryModel.SubLineBusiness = config.Map<ParamSubLineBusinessDesc, MODUN.SubLineBusinessServiceQueryModel>(paramCoverage.SubLineBusiness);
            if (paramCoverage.Homologation2G != null)
            {
                coverageServiceQueryModel.Homologation2G = CreateCoverage2GServiceModels(paramCoverage.Homologation2G);

            }
            return coverageServiceQueryModel;
        }

        /// <summary>
        /// Modelo de negocio a modelo de servicio Coverages
        /// </summary>
        /// <param name="paramCoverages">MOD negocio Coverages</param>
        /// <returns>MOD servicio Coverages</returns>
        public static List<PARUPSM.CoverageServiceModel> CreateCoverageServiceModels(List<ParamCoverage> paramCoverages)
        {
            List<PARUPSM.CoverageServiceModel> coverageServiceModels = new List<PARUPSM.CoverageServiceModel>();
            foreach (ParamCoverage item in paramCoverages)
            {
                coverageServiceModels.Add(CreateCoverageServiceModel(item));
            }

            return coverageServiceModels;
        }
        #endregion

        #region CoCoverageServiceModel
        /// <summary>
        /// Modelo de negocio a modelo de servicio CoCoverage
        /// </summary>
        /// <param name="paramCocoverage">MOD negocio CoCoverage</param>
        /// <returns>MOD servicio CoCoverage</returns>
        public static PARUPSM.CoCoverageServiceModel CreateCoCoverageServiceModel(ParamCoCoverage paramCocoverage)
        {
            var config = MapperCache.GetMapper<ParamCoCoverage, PARUPSM.CoCoverageServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamCoCoverage, PARUPSM.CoCoverageServiceModel>();
            });
            var cocoverageServiceQueryModel = config.Map<ParamCoCoverage, PARUPSM.CoCoverageServiceModel>(paramCocoverage);

            return cocoverageServiceQueryModel;
        }
        #endregion

        #region InsuredObjectServicesModel
        /// <summary>
        /// Convierte MOD-B de objetos del seguro a MOD-S   
        /// </summary>
        /// <param name="insuredObject">Objeto del seguro MOD-B</param>
        /// <returns>Objeto del seguro MOD-S</returns>
        public static PARUPSM.InsuredObjectServiceModel CreateInsuredObjectServiceModel(ParamInsuredObject insuredObject)
        {
            return new PARUPSM.InsuredObjectServiceModel()
            {
                Description = insuredObject.Description,
                Id = insuredObject.Id,
                SmallDescription = insuredObject.SmallDescription,
                IsDeclarative = insuredObject.IsDeclarative,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                }
            };
        }

        /// <summary>
        /// Convierte listado del MOD-B de Objeto del seguro a MOD-S
        /// </summary>
        /// <param name="insuredObject">Objeto del seguro MOD-B</param>
        /// <returns>Plan de pago MOD-S</returns>
        public static List<PARUPSM.InsuredObjectServiceModel> CreateInsurancesObjectServiceModel(List<ParamInsuredObject> insuredObject)
        {
            List<PARUPSM.InsuredObjectServiceModel> insuredObjectServiceModel = new List<PARUPSM.InsuredObjectServiceModel>();
            foreach (var item in insuredObject)
            {
                insuredObjectServiceModel.Add(CreateInsuredObjectServiceModel(item));
            }

            return insuredObjectServiceModel;
        }

        /// <summary>
        /// Convierte MOD-P de objetos del seguro a MOD-S   
        /// </summary>
        /// <param name="insuredObject">Objeto del seguro MOD-P</param>
        /// <returns>Objeto del seguro MOD-S</returns>
        public static PARUPSM.InsuredObjectServiceModel CreateInsuredObjectServiceModel(ParamInsuredObjectModel insuredObject)
        {
            return new PARUPSM.InsuredObjectServiceModel()
            {
                Description = insuredObject.Description,
                Id = insuredObject.Id,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                }
            };
        }

        /// <summary>
        /// Convierte listado del MOD-P de Objeto del seguro a MOD-S
        /// </summary>
        /// <param name="insuredObject">Objeto del seguro MOD-B</param>
        /// <returns>Objeto del seguro MOD-S</returns>
        public static List<PARUPSM.InsuredObjectServiceModel> CreateInsurancesObjectServiceModel(List<ParamInsuredObjectModel> insuredObject)
        {
            List<PARUPSM.InsuredObjectServiceModel> insuredObjectServiceModel = new List<PARUPSM.InsuredObjectServiceModel>();
            foreach (var item in insuredObject)
            {
                insuredObjectServiceModel.Add(CreateInsuredObjectServiceModel(item));
            }

            return insuredObjectServiceModel;
        }
        #endregion

        #region Expense Component

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="expense">parametro de componetes</param>       
        /// <returns> modelo de negocio </returns>     
        /// 
        public static List<ExpenseServiceModel> CreateExpenseServiceModels(List<ParamExpense> expense)
        {
            List<ExpenseServiceModel> expenseServiceModel = new List<ExpenseServiceModel>();
            foreach (var item in expense)
            {
                expenseServiceModel.Add(CreateExpenseServiceModel(item));
            }

            return expenseServiceModel;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="expense">parametro de descuento</param>       
        /// <returns> modelo de negocio </returns>
        public static ExpenseServiceModel CreateExpenseServiceModel(ParamExpense expense)
        {
            var config = MapperCache.GetMapper<ParamExpense, ExpenseServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamExpense, ExpenseServiceModel>();
            });
            var expenseServiceModel = config.Map<ParamExpense, ExpenseServiceModel>(expense);

            expenseServiceModel.Id = expense.Id;
            expenseServiceModel.SmallDescription = expense.SmallDescription;
            expenseServiceModel.TinyDescripcion = expense.TinyDescripcion;
            expenseServiceModel.ComponentClass = ModelServices.Enums.ComponentClass.EXPENSES;
            expenseServiceModel.ComponentType = ModelServices.Enums.ComponnetType.EXPENSES;
            expenseServiceModel.IsMandatory = expense.IsMandatory;
            expenseServiceModel.IsInitially = expense.IsInitially;
            expenseServiceModel.RateTypeServiceQueryModel = new RateTypeServiceQueryModel();
            expenseServiceModel.RateTypeServiceQueryModel.Id = expense.ParamRateType.Id;
            expenseServiceModel.RateTypeServiceQueryModel.description = expense.ParamRateType.Description;
            if (expense.ParamRuleSet != null)
            {
                expenseServiceModel.RuleSetServiceQueryModel = new RuleSetServiceQueryModel();
                expenseServiceModel.RuleSetServiceQueryModel.Id = expense.ParamRuleSet.Id;
                expenseServiceModel.RuleSetServiceQueryModel.description = expense.ParamRuleSet.Description;
            }
            expenseServiceModel.Rate = expense.Rate;
            expenseServiceModel.StatusTypeService = StatusTypeService.Original;
            expenseServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };

            return expenseServiceModel;
        }

        public static UTIERR.Result<ParamExpense, UTIERR.ErrorModel> CreateExpenseServiceModel(ExpenseServiceModel serviceModel)
        {
            List<string> errorCreateModel = new List<string>();

            UTIERR.Result<ParamExpense, UTIERR.ErrorModel> ExpenceCom = ParamExpense.CreateParamExpense(
                serviceModel.Id,
                serviceModel.SmallDescription,
                serviceModel.TinyDescripcion,
                ENUM.ComponentClass.EXPENSES,
                ENUM.ComponnetType.EXPENSES,
                serviceModel.Rate,
                serviceModel.IsMandatory,
                serviceModel.IsInitially,
                ((ResultValue<ParamRuleSet, ErrorModel>)ParamRuleSet.GetParamRuleSet(serviceModel.RuleSetServiceQueryModel.Id, serviceModel.RuleSetServiceQueryModel.description)).Value,
                ((ResultValue<ParamRateType, ErrorModel>)ParamRateType.GetParamRuleSet(serviceModel.RateTypeServiceQueryModel.Id, serviceModel.RateTypeServiceQueryModel.description)).Value);
            if (ExpenceCom is UTIERR.ResultError<ParamExpense, UTIERR.ErrorModel>)
            {
                errorCreateModel.AddRange(((UTIERR.ResultError<ParamExpense, UTIERR.ErrorModel>)ExpenceCom).Message.ErrorDescription);
            }
            if (errorCreateModel.Count > 0)
            {
                return new UTIERR.ResultError<ParamExpense, UTIERR.ErrorModel>(UTIERR.ErrorModel.CreateErrorModel(errorCreateModel, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return ExpenceCom;
        }
        #endregion

        #region RateType    

        /// <summary>
        /// Convierte el listado de rata type
        /// </summary>
        /// <param name="paramRateType">Objetos del seguro MOD-B</param>
        /// <returns>Objetos del seguro MOD-S</returns>
        public static List<RateTypeServiceQueryModel> CreateRateTypeQueryModels(List<ParamRateType> paramRateType)
        {
            List<RateTypeServiceQueryModel> rateTypeServiceQueryModel = new List<RateTypeServiceQueryModel>();
            foreach (var item in paramRateType)
            {
                rateTypeServiceQueryModel.Add(CreateRateTypeServiceModel(item));
            }

            return rateTypeServiceQueryModel;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="rateType">parametro de descuento</param>       
        /// <returns> modelo de negocio </returns>
        public static RateTypeServiceQueryModel CreateRateTypeServiceModel(ParamRateType rateType)
        {
            var config = MapperCache.GetMapper<ParamRateType, RateTypeServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamRateType, RateTypeServiceQueryModel>();
            });
            var rateTypeServiceQueryModel = config.Map<ParamRateType, RateTypeServiceQueryModel>(rateType);
            rateTypeServiceQueryModel.Id = (int)rateType.Id;
            rateTypeServiceQueryModel.description = rateType.Description;
            rateTypeServiceQueryModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;

            return rateTypeServiceQueryModel;
        }
        #endregion

        #region RuleSet

        /// <summary>
        /// Convierte el listado de rata type
        /// </summary>
        /// <param name="paramRateType">Objetos del seguro MOD-B</param>
        /// <returns>Objetos del seguro MOD-S</returns>
        public static List<RuleSetServiceQueryModel> CreateRuleSetQueryModels(List<ParamRuleSet> paramRuleSet)
        {
            List<RuleSetServiceQueryModel> ruleSetServiceQueryModel = new List<RuleSetServiceQueryModel>();
            foreach (var item in paramRuleSet)
            {
                ruleSetServiceQueryModel.Add(CreateRuleSetServiceModel(item));
            }

            return ruleSetServiceQueryModel;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="rateType">parametro de descuento</param>       
        /// <returns> modelo de negocio </returns>
        public static RuleSetServiceQueryModel CreateRuleSetServiceModel(ParamRuleSet ruleSet)
        {

            var config = MapperCache.GetMapper<ParamRuleSet, RuleSetServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamRuleSet, RuleSetServiceQueryModel>();
            });
            var ruleSetServiceQueryModel = config.Map<ParamRuleSet, RuleSetServiceQueryModel>(ruleSet);

            ruleSetServiceQueryModel.Id = ruleSet.Id;
            ruleSetServiceQueryModel.description = ruleSet.Description;
            ruleSetServiceQueryModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;

            return ruleSetServiceQueryModel;
        }

        #endregion

        #region ClauseServicesModelQuery
        /// <summary>
        /// Metodo modelo clausulas
        /// </summary>
        /// <param name="parametrizationClause">Recibe parametrizationClause </param>
        /// <returns>Retorna Modelo ClauseServiceModel</returns>
        public static MODUN.ClauseServiceModel ClauseServicesModelConsult(ParamClause parametrizationClause)
        {
            var config = MapperCache.GetMapper<ParamClause, MODUN.ClauseServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamClause, MODUN.ClauseServiceModel>();
            });
            var paramClauseServiceModel = config.Map<ParamClause, MODUN.ClauseServiceModel>(parametrizationClause);

            paramClauseServiceModel.Id = parametrizationClause.Clause.Id;
            paramClauseServiceModel.Name = parametrizationClause.Clause.Name;
            paramClauseServiceModel.Title = parametrizationClause.Clause.Title;
            paramClauseServiceModel.ClauseText = parametrizationClause.Clause.Text;
            paramClauseServiceModel.ConditionLevelServiceQueryModel = new MODUN.ConditionLevelServiceModel()
            {
                Id = parametrizationClause.Clause.ConditionLevel.Id,
                Description = parametrizationClause.Clause.ConditionLevel.Description
            };
            paramClauseServiceModel.InputStartDate = parametrizationClause.InputStartDate;
            paramClauseServiceModel.DueDate = parametrizationClause.DueDate;
            paramClauseServiceModel.ClauseLevelServiceModel = new MODUN.ClauseLevelServiceModel()
            {
                ClauseId = parametrizationClause.ClauseLevel.ClauseId,
                ConditionLevelId = parametrizationClause.ClauseLevel.ConditionLevelId,
                IsMandatory = parametrizationClause.ClauseLevel.IsMandatory,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                }
            };
            paramClauseServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            paramClauseServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };

            if (paramClauseServiceModel.ConditionLevelServiceQueryModel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "Coverage") && parametrizationClause.ParamClauseCoverage != null)
            {
                if (parametrizationClause.ParamClauseCoverage != null)
                {
                    paramClauseServiceModel.CoverageServiceQueryModel = new MODUN.CoverageClauseServiceModel()
                    {
                        Id = parametrizationClause.ParamClauseCoverage.Id,
                        Description = parametrizationClause.ParamClauseCoverage.Description,
                        InsuredObjectServiceQueryModel = new MODUN.InsuredObjectServiceQueryModel()
                        {
                            Id = parametrizationClause.ParamClauseCoverage.ParamClauseInsuredObject.Id,
                            Description = parametrizationClause.ParamClauseCoverage.ParamClauseInsuredObject.Description
                        },
                        PerilServiceQueryModel = new MODUN.PerilServiceQueryModel()
                        {
                            Id = parametrizationClause.ParamClauseCoverage.Peril.Id,
                            Description = parametrizationClause.ParamClauseCoverage.Peril.Description
                        }
                    };

                }

            }

            if (paramClauseServiceModel.ConditionLevelServiceQueryModel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "Prefix"))
            {
                if (parametrizationClause.ParamClausePrefix != null)
                {
                    paramClauseServiceModel.PrefixServiceQueryModel = new MODUN.PrefixServiceQueryModel()
                    {
                        PrefixCode = parametrizationClause.ParamClausePrefix.Id,
                        PrefixDescription = parametrizationClause.ParamClausePrefix.Description
                    };
                }

            }

            if (paramClauseServiceModel.ConditionLevelServiceQueryModel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "TypeRisk"))
            {
                if (parametrizationClause.RiskType != null)
                {
                    paramClauseServiceModel.RiskTypeServiceQueryModel = new MODUN.RiskTypeServiceModel()
                    {
                        Id = parametrizationClause.RiskType.Id,
                        Description = parametrizationClause.RiskType.Description
                    };
                }

            }
            if (paramClauseServiceModel.ConditionLevelServiceQueryModel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "TechnicalBranch"))
            {
                if (parametrizationClause.ParamClauseLineBusiness != null)
                {
                    paramClauseServiceModel.LineBusinessServiceQueryModel = new LineBusinessServiceQueryModel()
                    {
                        Id = parametrizationClause.ParamClauseLineBusiness.Id,
                        Description = parametrizationClause.ParamClauseLineBusiness.Description
                    };
                }

            }


            return paramClauseServiceModel;
        }

        /// <summary>
        /// Metodo lista de clausulas
        /// </summary>
        /// <param name="parametrizationClause">Recibe parametrizationClause</param>
        /// <returns>Retorna parametrizationClause</returns>
        public static List<MODUN.ClauseServiceModel> CreateClauseServiceModelsConsults(List<ParamClause> parametrizationClause)
        {
            List<MODUN.ClauseServiceModel> clauseServiceModel = new List<MODUN.ClauseServiceModel>();
            foreach (var item in parametrizationClause)
            {
                clauseServiceModel.Add(ClauseServicesModelConsult(item));
            }

            return clauseServiceModel;
        }
        #endregion ClauseServicesModelQuery

        #region ClauseServicesModel
        /// <summary>
        /// Metodo crear clausulas
        /// </summary>
        /// <param name="parametrizationClause">Recibe parametrizationClause</param>
        /// <returns>Retorna parametrizationClause</returns>
        public static MODUN.ClauseServiceModel CreateClauseServiceModel(ParamClause parametrizationClause)
        {
            var config = MapperCache.GetMapper<ParamClause, MODUN.ClauseServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamClause, MODUN.ClauseServiceModel>();
            });
            var paramClauseServiceModel = config.Map<ParamClause, MODUN.ClauseServiceModel>(parametrizationClause);

            paramClauseServiceModel.Id = parametrizationClause.Clause.Id;
            paramClauseServiceModel.Name = parametrizationClause.Clause.Name;
            paramClauseServiceModel.Title = parametrizationClause.Clause.Title;
            paramClauseServiceModel.ClauseText = parametrizationClause.Clause.Text;
            paramClauseServiceModel.ConditionLevelServiceQueryModel = new MODUN.ConditionLevelServiceModel();
            paramClauseServiceModel.InputStartDate = parametrizationClause.InputStartDate;
            paramClauseServiceModel.DueDate = parametrizationClause.DueDate;
            paramClauseServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            paramClauseServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };
            return paramClauseServiceModel;
        }

        /// <summary>
        /// Metodo retorna lista
        /// </summary>
        /// <param name="parametrizationClause">Recibe parametrizationClause</param>
        /// <returns>Retorna parametrizationClause</returns>
        public static List<MODUN.ClauseServiceModel> CreateClauseServiceModels(List<ParamClause> parametrizationClause)
        {
            List<MODUN.ClauseServiceModel> clauseServiceModel = new List<MODUN.ClauseServiceModel>();
            foreach (var item in parametrizationClause)
            {
                clauseServiceModel.Add(CreateClauseServiceModel(item));
            }

            return clauseServiceModel;
        }
        #endregion ClauseServicesModel

        #region ClauseLevel
        /// <summary>
        /// Metodo clausula por nivel
        /// </summary>
        /// <param name="clauseLevel">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevel</returns>
        public static MODUN.ClauseLevelServiceModel CreateClauseLevelServiceModel(ParamClauseLevel clauseLevel)
        {
            return new MODUN.ClauseLevelServiceModel()
            {
                ClauseId = clauseLevel.ClauseId,
                ConditionLevelId = clauseLevel.ConditionLevelId,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Metodo lista de clausulas por nivel
        /// </summary>
        /// <param name="clauseLevel">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevelServiceModel </returns>
        public static List<MODUN.ClauseLevelServiceModel> CreateClauseLevelServiceModels(List<ParamClauseLevel> clauseLevel)
        {
            List<MODUN.ClauseLevelServiceModel> clauseLevelServiceModel = new List<MODUN.ClauseLevelServiceModel>();
            foreach (var item in clauseLevel)
            {
                clauseLevelServiceModel.Add(CreateClauseLevelServiceModel(item));
            }

            return clauseLevelServiceModel;
        }

        public static MODUN.ClauseLevelServiceModel CreateClauseLevelServiceModel(ParamClauseModel clauseLevel)
        {
            return new MODUN.ClauseLevelServiceModel()
            {
                ClauseId = clauseLevel.Id,
                IsMandatory = clauseLevel.IsMandatory,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                }
            };
        }

        /// <summary>
        /// Metodo lista de clausulas por nivel
        /// </summary>
        /// <param name="clauseLevel">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevelServiceModel </returns>
        public static List<MODUN.ClauseLevelServiceModel> CreateClauseLevelServiceModels(List<ParamClauseModel> clauseLevel)
        {
            List<MODUN.ClauseLevelServiceModel> clauseLevelServiceModel = new List<MODUN.ClauseLevelServiceModel>();
            foreach (var item in clauseLevel)
            {
                clauseLevelServiceModel.Add(CreateClauseLevelServiceModel(item));
            }

            return clauseLevelServiceModel;
        }
        #endregion ClauseLevel

        #region ConditionLevel
        /// <summary>
        /// Metodo condicion nivel
        /// </summary>
        /// <param name="conditionLevel">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevel</returns>
        public static MODUN.ConditionLevelServiceModel CreateConditionLevelServiceModel(UNDMO.ConditionLevel conditionLevel)
        {
            return new MODUN.ConditionLevelServiceModel()
            {
                Id = conditionLevel.Id,
                Description = conditionLevel.Description,
            };
        }

        /// <summary>
        /// Metodo lista de condicion por nivel
        /// </summary>
        /// <param name="conditionLevel">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevelServiceModel </returns>
        public static List<MODUN.ConditionLevelServiceModel> CreateConditionLevelServiceModels(List<UNDMO.ConditionLevel> conditionLevel)
        {
            List<MODUN.ConditionLevelServiceModel> clauseConditionLevelServiceModel = new List<MODUN.ConditionLevelServiceModel>();
            foreach (var item in conditionLevel)
            {
                clauseConditionLevelServiceModel.Add(CreateConditionLevelServiceModel(item));
            }

            return clauseConditionLevelServiceModel;
        }
        #endregion ConditionLevel

        #region Prefix
        /// <summary>
        /// Metodo ramo comercial
        /// </summary>
        /// <param name="clausePrefix">Recibe clausePrefix</param>
        /// <returns>Retorna clausePrefix</returns>
        public static MODUN.PrefixServiceQueryModel CreateCommercialBranchServiceModel(ParamClausePrefix clausePrefix)
        {
            return new MODUN.PrefixServiceQueryModel()
            {
                PrefixCode = clausePrefix.Id,
                PrefixDescription = clausePrefix.Description,
                PrefixSmallDescription = clausePrefix.SmallDescription
            };
        }

        /// <summary>
        /// Metodo lista de ramo comercial
        /// </summary>
        /// <param name="clausePrefix">Recibe clausePrefix</param>
        /// <returns>Retorna clausePrefixServiceModel</returns>
        public static List<MODUN.PrefixServiceQueryModel> CreateCommercialBranchServiceModels(List<ParamClausePrefix> clausePrefix)
        {
            List<MODUN.PrefixServiceQueryModel> clausePrefixServiceModel = new List<MODUN.PrefixServiceQueryModel>();
            foreach (var item in clausePrefix)
            {
                clausePrefixServiceModel.Add(CreateCommercialBranchServiceModel(item));
            }

            return clausePrefixServiceModel;
        }
        #endregion Prefix

        #region RiskType
        /// <summary>
        /// Metodo tipo de riesgo
        /// </summary>
        /// <param name="clauseRiskType">Recibe clauseRiskType</param>
        /// <returns>Retorna clauseRiskType</returns>
        public static MODUN.RiskTypeServiceModel CreateCoveredRiskTypeServiceModel(UNDMO.RiskType clauseRiskType)
        {
            return new MODUN.RiskTypeServiceModel()
            {
                Id = clauseRiskType.Id,
                Description = clauseRiskType.Description
            };
        }

        /// <summary>
        /// Metodo lista de tipo de riesgo
        /// </summary>
        /// <param name="clauseRiskType">Recibe clauseRiskType</param>
        /// <returns>Retorna clauseRiskTypeServiceModel</returns>
        public static List<MODUN.RiskTypeServiceModel> CreateCoveredRiskTypeServiceModels(List<UNDMO.RiskType> clauseRiskType)
        {
            List<MODUN.RiskTypeServiceModel> clauseRiskTypeServiceModel = new List<MODUN.RiskTypeServiceModel>();
            foreach (var item in clauseRiskType)
            {
                clauseRiskTypeServiceModel.Add(CreateCoveredRiskTypeServiceModel(item));
            }

            return clauseRiskTypeServiceModel;
        }
        #endregion RiskType

        #region Text
        /// <summary>
        /// Metodo textos 
        /// </summary>
        /// <param name="text">Recibe text</param>
        /// <returns>Retorna text</returns>
        public static MODUN.TextServiceModel CreateTextServiceModel(ParamClause text)
        {
            return new MODUN.TextServiceModel()
            {
                Id = text.Text.Id,
                Description = text.Text.TextTitle,
                TextBody = text.Text.TextBody
            };
        }

        /// <summary>
        /// Metodo lista de textos 
        /// </summary>
        /// <param name="text">Recibe text</param>
        /// <returns>retorna clauseTextServiceModel</returns>
        public static List<MODUN.TextServiceModel> CreateTextServiceModels(List<ParamClause> text)
        {
            List<MODUN.TextServiceModel> clauseTextServiceModel = new List<MODUN.TextServiceModel>();
            foreach (var item in text)
            {
                clauseTextServiceModel.Add(CreateTextServiceModel(item));
            }

            return clauseTextServiceModel;
        }
        #endregion Text

        #region Coverage
        /// <summary>
        /// Metodo coberturas
        /// </summary>
        /// <param name="clauseCoverage">Recibe clauseCoverage</param>
        /// <returns>Retorna clauseCoverage</returns>
        public static MODUN.CoverageClauseServiceModel CreateCoverageServiceModel(ParamClauseCoverage clauseCoverage)
        {
            return new MODUN.CoverageClauseServiceModel()
            {
                Id = clauseCoverage.Id,
                Description = clauseCoverage.Description,
                PerilServiceQueryModel = new MODUN.PerilServiceQueryModel
                {
                    Id = clauseCoverage.Peril.Id,
                    Description = clauseCoverage.Peril.Description
                },

                InsuredObjectServiceQueryModel = new MODUN.InsuredObjectServiceQueryModel
                {
                    Id = clauseCoverage.ParamClauseInsuredObject.Id,
                    Description = clauseCoverage.ParamClauseInsuredObject.Description
                },

            };
        }

        /// <summary>
        /// Metodo listas de coberturas
        /// </summary>
        /// <param name="clauseCoverage">Recibe clauseCoverage</param>
        /// <returns>Retorna clauseCoverageServiceModel</returns>
        public static List<MODUN.CoverageClauseServiceModel> CreateCoverageServiceModels(List<ParamClauseCoverage> clauseCoverage)
        {
            List<MODUN.CoverageClauseServiceModel> clauseCoverageServiceModel = new List<MODUN.CoverageClauseServiceModel>();
            foreach (var item in clauseCoverage)
            {
                clauseCoverageServiceModel.Add(CreateCoverageServiceModel(item));
            }

            return clauseCoverageServiceModel;
        }
        #endregion Coverage

        /// <summary>
        /// Mapeo lista modelo de negocio AssistanceType a modelo de servicio.
        /// </summary>
        /// <param name="paramAssistanceType">>modelo de negocio de ParamAssistanceType</param>
        /// <returns></returns>
        public static MODUN.AssistanceTypeServiceQueryModel MappAssistanceType(ParamAssistanceType paramAssistanceType)
        {
            MODUN.AssistanceTypeServiceQueryModel itemAssistanceTypeServiceModel = new MODUN.AssistanceTypeServiceQueryModel();
            itemAssistanceTypeServiceModel.AssistanceCode = paramAssistanceType.AssistanceCode;
            itemAssistanceTypeServiceModel.AssistanceDescription = paramAssistanceType.AssistanceDescription;

            return itemAssistanceTypeServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio Prefix a modelo de servicio.
        /// </summary>
        /// <param name="paramPrefix">>modelo de negocio de ParamPrefix</param>
        /// <returns></returns>
        public static MODUN.PrefixServiceQueryModel MappPrefix(ParamPrefix paramPrefix)
        {
            MODUN.PrefixServiceQueryModel itemPrefixServiceModel = new MODUN.PrefixServiceQueryModel();
            itemPrefixServiceModel.PrefixCode = paramPrefix.PrefixCode;
            itemPrefixServiceModel.PrefixDescription = paramPrefix.Description;
            itemPrefixServiceModel.PrefixSmallDescription = paramPrefix.SmallDescription;

            return itemPrefixServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio Product a modelo de servicio.
        /// </summary>
        /// <param name="paramProduct">>modelo de negocio de ParamProduct</param>
        /// <returns></returns>
        public static MODUN.ProductServiceQueryModel MappProduct(ParamProduct paramProduct)
        {
            MODUN.ProductServiceQueryModel itemProductServiceModel = new MODUN.ProductServiceQueryModel();
            itemProductServiceModel.ProductId = paramProduct.ProductId;
            itemProductServiceModel.ProductDescription = paramProduct.ProductDescription;
            itemProductServiceModel.ProductSmallDescription = paramProduct.ProductSmallDescription;
            itemProductServiceModel.ActiveProduct = paramProduct.ActiveProduct;

            return itemProductServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio RequestEndorsement a modelo de servicio.
        /// </summary>
        /// <param name="paramRequestEndorsement">modelo de negocio de ParamRequestEndorsement</param>
        /// <returns></returns>
        public static MODUN.RequestEndorsementServiceQueryModel MappRequestEndorsement(ParamRequestEndorsement paramRequestEndorsement)
        {
            if (paramRequestEndorsement != null)
            {
                MODUN.RequestEndorsementServiceQueryModel itemRequestEndorsementServiceModel = new MODUN.RequestEndorsementServiceQueryModel();
                itemRequestEndorsementServiceModel.RequestEndorsementId = paramRequestEndorsement.RequestEndorsementId;
                itemRequestEndorsementServiceModel.RequestId = paramRequestEndorsement.RequestId;
                itemRequestEndorsementServiceModel.ProductId = paramRequestEndorsement.ProductId;
                itemRequestEndorsementServiceModel.PrefixCode = paramRequestEndorsement.PrefixCode;

                return itemRequestEndorsementServiceModel;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Mapeo lista modelo de negocio GroupCoverage a modelo de servicio.
        /// </summary>
        /// <param name="paramGroupCoverage">>modelo de negocio de ParamGroupCoverage</param>
        /// <returns></returns>
        public static MODUN.GroupCoverageServiceQueryModel MappGroupCoverage(ParamGroupCoverage paramGroupCoverage)
        {
            MODUN.GroupCoverageServiceQueryModel itemGroupCoverageServiceModel = new MODUN.GroupCoverageServiceQueryModel();
            itemGroupCoverageServiceModel.GroupCoverageId = paramGroupCoverage.GroupCoverageId;
            itemGroupCoverageServiceModel.GroupCoverageSmallDescription = paramGroupCoverage.GroupCoverageSmallDescription;

            return itemGroupCoverageServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio AssistanceType a modelo de servicio.
        /// </summary>
        /// <param name="paramAssistanceType">>modelo de negocio de ParamAssistanceType</param>
        /// <returns></returns>
        public static MODUN.AssistanceTypeServiceQueryModel MappAssistenceType(ParamAssistanceType paramAssistanceType)
        {
            MODUN.AssistanceTypeServiceQueryModel itemAssistanceTypeServiceModel = new MODUN.AssistanceTypeServiceQueryModel();
            itemAssistanceTypeServiceModel.AssistanceCode = paramAssistanceType.AssistanceCode;
            itemAssistanceTypeServiceModel.AssistanceDescription = paramAssistanceType.AssistanceDescription;

            return itemAssistanceTypeServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio Business a modelo de servicio.
        /// </summary>
        /// <param name="paramBusiness">modelo de negocio ParamBusiness</param>
        /// <returns></returns>
        public static MODUN.BusinessServiceModel MappBusiness(ParamBusiness paramBusiness)
        {
            MODPA.ErrorServiceModel errorServiceModel = new MODPA.ErrorServiceModel();
            errorServiceModel.ErrorDescription = new List<string>();
            errorServiceModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;
            MODUN.BusinessServiceModel itemBusinessServiceModel = new MODUN.BusinessServiceModel();
            itemBusinessServiceModel.BusinessId = paramBusiness.BusinessId;
            itemBusinessServiceModel.Description = paramBusiness.Description;
            itemBusinessServiceModel.IsEnabled = paramBusiness.IsEnabled;
            itemBusinessServiceModel.PrefixCode = MappPrefix(paramBusiness.Prefix);
            itemBusinessServiceModel.ErrorServiceModel = errorServiceModel;
            itemBusinessServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Original;

            return itemBusinessServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio BusinessConfiguration a modelo de servicio.
        /// </summary>
        /// <param name="paramBusinessConfiguration">modelo de negocio ParamBusinessConfiguration</param>
        /// <returns></returns>
        public static MODUN.BusinessConfigurationServiceModel MappBusinessConfiguration(ParamBusinessConfiguration paramBusinessConfiguration)
        {
            MODPA.ErrorServiceModel errorServiceModel = new MODPA.ErrorServiceModel();
            errorServiceModel.ErrorDescription = new List<string>();
            errorServiceModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;
            MODUN.BusinessConfigurationServiceModel itemBusinessConfigurationServiceModel = new MODUN.BusinessConfigurationServiceModel();
            itemBusinessConfigurationServiceModel.BusinessConfigurationId = paramBusinessConfiguration.BusinessConfigurationId;
            itemBusinessConfigurationServiceModel.BusinessId = paramBusinessConfiguration.BusinessId;
            itemBusinessConfigurationServiceModel.Request = MappRequestEndorsement(paramBusinessConfiguration.Request);
            itemBusinessConfigurationServiceModel.Product = MappProduct(paramBusinessConfiguration.Product);
            itemBusinessConfigurationServiceModel.GroupCoverage = MappGroupCoverage(paramBusinessConfiguration.GroupCoverage);
            itemBusinessConfigurationServiceModel.Assistance = MappAssistanceType(paramBusinessConfiguration.Assistance);
            itemBusinessConfigurationServiceModel.ProductIdResponse = paramBusinessConfiguration.ProductIdResponse;
            itemBusinessConfigurationServiceModel.ErrorServiceModel = errorServiceModel;
            itemBusinessConfigurationServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Original;

            return itemBusinessConfigurationServiceModel;
        }

        /// <summary>
        /// Mapeo lista modelo de negocio Business a modelo de servicio.
        /// </summary>
        /// <param name="paramBusinessParamBusinessConfiguration">modelo de negocio ParamBusiness</param>
        /// <returns></returns>
        public static MODUN.BusinessServiceModel MappBusinessBusinessConfiguration(ParamBusinessParamBusinessConfiguration paramBusinessParamBusinessConfiguration)
        {
            MODPA.ErrorServiceModel errorServiceModel = new MODPA.ErrorServiceModel();
            errorServiceModel.ErrorDescription = new List<string>();
            errorServiceModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;
            MODUN.BusinessServiceModel itemBusinessServiceModel = new MODUN.BusinessServiceModel();
            itemBusinessServiceModel.BusinessId = paramBusinessParamBusinessConfiguration.ParamBusiness.BusinessId;
            itemBusinessServiceModel.Description = paramBusinessParamBusinessConfiguration.ParamBusiness.Description;
            itemBusinessServiceModel.IsEnabled = paramBusinessParamBusinessConfiguration.ParamBusiness.IsEnabled;
            itemBusinessServiceModel.PrefixCode = MappPrefix(paramBusinessParamBusinessConfiguration.ParamBusiness.Prefix);
            itemBusinessServiceModel.BusinessConfiguration = new List<MODUN.BusinessConfigurationServiceModel>();
            itemBusinessServiceModel.ErrorServiceModel = errorServiceModel;
            itemBusinessServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Original;
            foreach (ParamBusinessConfiguration businessConfigurationBusiness in paramBusinessParamBusinessConfiguration.ParamBusinessConfiguration)
            {
                MODUN.BusinessConfigurationServiceModel businessConfigurationServiceModel = MappBusinessConfiguration(businessConfigurationBusiness);
                itemBusinessServiceModel.BusinessConfiguration.Add(businessConfigurationServiceModel);
            }

            return itemBusinessServiceModel;
        }

        #region QuotationNumber
        /// <summary>
        /// Convierte modelo de cuotas en modelo de servicio
        /// </summary>
        /// <param name="parametrizationQuotationNumber">modelo de cuotas</param>
        /// <returns>Modelo de servicio</returns>
        public static PARUPSM.QuotationNumberServiceModel CreateQuotationNumberServiceModel(ParametrizationQuotationNumber parametrizationQuotationNumber)
        {
            return new PARUPSM.QuotationNumberServiceModel
            {
                Branch = ModelsServicesAssembler.CreateBranchServiceQueryModel(parametrizationQuotationNumber.Branch),
                HasQuotation = parametrizationQuotationNumber.HasQuotation,
                Prefix = ModelsServicesAssembler.CreatePrefixServiceQueryModel(parametrizationQuotationNumber.Prefix),
                QuotNumber = parametrizationQuotationNumber.QuotNumber
            };
        }

        /// <summary>
        /// Convierte lista de modelos de cuotas en modelos de servicio
        /// </summary>
        /// <param name="parametrizationQuotationNumbers">Modelos de cuotas</param>
        /// <returns>Modelos de servicio</returns>
        public static List<PARUPSM.QuotationNumberServiceModel> CreateQuotationNumberServiceModel(List<ParametrizationQuotationNumber> parametrizationQuotationNumbers)
        {
            List<PARUPSM.QuotationNumberServiceModel> quotationNumberServiceModels = new List<PARUPSM.QuotationNumberServiceModel>();

            foreach (ParametrizationQuotationNumber parametrizationQuotationNumber in parametrizationQuotationNumbers)
            {
                quotationNumberServiceModels.Add(ModelsServicesAssembler.CreateQuotationNumberServiceModel(parametrizationQuotationNumber));
            }

            return quotationNumberServiceModels;
        }
        #endregion

        #region Branch
        /// <summary>
        /// Convierte modelos de sucursal en modelo de servicios
        /// </summary>
        /// <param name="paramBranch">Modelo de sucursal</param>
        /// <returns>Modelo de servicio</returns>
        public static MODCOPA.BranchServiceQueryModel CreateBranchServiceQueryModel(COMPAMO.ParamBranch paramBranch)
        {
            return new MODCOPA.BranchServiceQueryModel
            {
                Id = paramBranch.Id,
                Description = paramBranch.Description,
                SmallDescription = paramBranch.SmallDescription
            };
        }

        /// <summary>
        /// Convierte mLista de modelos de sucursal en modelos de servicio
        /// </summary>
        /// <param name="paramBranchs">Modelos de sucursal</param>
        /// <returns>Modelos de servicio</returns>
        public static List<MODCOPA.BranchServiceQueryModel> CreateBranchServiceQueryModels(List<COMPAMO.ParamBranch> paramBranchs)
        {
            List<MODCOPA.BranchServiceQueryModel> branches = new List<MODCOPA.BranchServiceQueryModel>();

            foreach (COMPAMO.ParamBranch paramBranch in paramBranchs)
            {
                branches.Add(ModelsServicesAssembler.CreateBranchServiceQueryModel(paramBranch));
            }

            return branches;
        }
        #endregion

        #region Prefix
        /// <summary>
        /// Convierte modelo de ramo en modelo de servicio
        /// </summary>
        /// <param name="paramPrefix">Modelo de ramo</param>
        /// <returns>Modelo de servicio</returns>
        public static MODUN.PrefixServiceQueryModel CreatePrefixServiceQueryModel(ParamPrefix paramPrefix)
        {
            return new MODUN.PrefixServiceQueryModel
            {
                PrefixCode = paramPrefix.PrefixCode,
                PrefixDescription = paramPrefix.Description,
                PrefixSmallDescription = paramPrefix.SmallDescription
            };
        }

        /// <summary>
        /// Convierte lista de modelos de ramo en modelos de servicio
        /// </summary>
        /// <param name="paramPrefixs">Modelos de ramo</param>
        /// <returns>Modelos de servicio</returns>
        public static List<MODUN.PrefixServiceQueryModel> CreatePrefixServiceQueryModels(List<ParamPrefix> paramPrefixs)
        {
            List<MODUN.PrefixServiceQueryModel> prefixes = new List<MODUN.PrefixServiceQueryModel>();

            foreach (ParamPrefix paramPrefix in paramPrefixs)
            {
                prefixes.Add(ModelsServicesAssembler.CreatePrefixServiceQueryModel(paramPrefix));
            }

            return prefixes;
        }
        #endregion

        #region Policy Number
        /// <summary>
        /// convierte de una lista de service model a una lista de modelo de negocio 
        /// </summary>
        /// <param name="parametrizationPolicyNumber">lista de modelo de negocio </param>
        /// <returns>lista de service model</returns>
        public static List<PARUPSM.PolicyNumberServiceModel> CreatePolicyNumbersServiceModel(List<ParamPolicyNumber> parametrizationPolicyNumber)
        {
            List<PARUPSM.PolicyNumberServiceModel> policyNumberServiceModel = new List<PARUPSM.PolicyNumberServiceModel>();
            foreach (var item in parametrizationPolicyNumber)
            {
                policyNumberServiceModel.Add(CreatePolicyNumberServiceModel(item));
            }

            return policyNumberServiceModel;
        }

        /// <summary>
        /// convierte de un service model a un modelo de negocio 
        /// </summary>
        /// <param name="item">modelo de negocio </param>
        /// <returns>service model</returns>
        private static PARUPSM.PolicyNumberServiceModel CreatePolicyNumberServiceModel(ParamPolicyNumber item) => new PARUPSM.PolicyNumberServiceModel
        {
            Branch = ModelsServicesAssembler.CreateBranchServiceQueryModel(item.Branch),
            LastPolicyDate = item.LastPolicyDate,
            HasPolicy = item.HasPolicy,
            Prefix = ModelsServicesAssembler.CreatePrefixServiceQueryModel(item.Prefix),
            PolicyLastNumber = item.PolicyNumber
        };
        #endregion

        #region business Line Parametrization
        /// <summary>
        /// Convierte Modelo de servicio a modelo de ramo técnico
        /// </summary>
        /// <param name="businessLineServiceModel">Modelo de servicio</param>
        /// <returns>Modelo de ramo técnico</returns>
        public static Result<ParamLineBusinessModel, ErrorModel> CreateParamLineBusiness(PARUPSM.LineBusinessServiceModel businessLineServiceModel)
        {
            List<string> errorModelListDescription = new List<string>();
            Result<ParamLineBusinessModel, ErrorModel> lineBusinessItem = ParamLineBusinessModel.CreateParamLineBusinessModel(businessLineServiceModel.Id, businessLineServiceModel.Description, businessLineServiceModel.SmallDescription, businessLineServiceModel.TinyDescription);
            if (lineBusinessItem is ResultError<ParamLineBusinessModel, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                return new ResultError<ParamLineBusinessModel, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
            }
            return lineBusinessItem;
        }

        /// <summary>
        /// Convierte Modelo de servicio a modelo de ramo técnico
        /// </summary>
        /// <param name="businessLineServiceModel">Modelo de servicio</param>
        /// <returns>Modelo de ramo técnico</returns>
        public static Result<ParamCoveredRiskType, ErrorModel> CreateCoveredRiskType(PARUPSM.CoveredRiskTypeServiceRelationModel coveredRiskTypeServiceModel)
        {
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCoveredRiskType, ErrorModel> coveredRiskTypeItem = ParamCoveredRiskType.CreateParamCoveredRiskType(coveredRiskTypeServiceModel.Id, coveredRiskTypeServiceModel.SmallDescription);
            if (coveredRiskTypeItem is ResultError<ParamCoveredRiskType, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                return new ResultError<ParamCoveredRiskType, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
            }
            return coveredRiskTypeItem;
        }

        public static Result<List<ParamCoveredRiskType>, ErrorModel> CreateCoveredRiskTypes(List<PARUPSM.CoveredRiskTypeServiceRelationModel> coveredRiskTypesServiceModel)
        {
            List<ParamCoveredRiskType> lstLineBusiness = new List<ParamCoveredRiskType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCoveredRiskType, ErrorModel> result;
            foreach (PARUPSM.CoveredRiskTypeServiceRelationModel coveredRiskType in coveredRiskTypesServiceModel)
            {
                result = CreateCoveredRiskType(coveredRiskType);
                if (result is ResultError<ParamCoveredRiskType, ErrorModel>)
                {
                    errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                    return new ResultError<List<ParamCoveredRiskType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamCoveredRiskType resultValue = (result as ResultValue<ParamCoveredRiskType, ErrorModel>).Value;
                    lstLineBusiness.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamCoveredRiskType>, ErrorModel>(lstLineBusiness);
        }

        /// <summary>
        /// Convierte modelo de ramo técnico a modelo de servicio
        /// </summary>
        /// <param name="paramLineBusiness">Ramo técnico</param>
        /// <returns>Modelo de servicio</returns>
        public static PARUPSM.LineBusinessServiceModel CreateLineBusinessServiceModel(ParamLineBusinessModel paramLineBusiness, List<ParamCoveredRiskType> coveredRiskTypesLst)
        {
            return new PARUPSM.LineBusinessServiceModel
            {
                Id = paramLineBusiness.Id,
                Description = paramLineBusiness.Description,
                SmallDescription = paramLineBusiness.SmallDescription,
                TinyDescription = paramLineBusiness.TinyDescription,
                CoveredRiskTypeServiceModel = CreateCoveredRiskTypes(coveredRiskTypesLst),
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Mapear modelos de negocio ParamCoveredRiskType a modelos de Servicio CoveredRiskTypesServiceModel.
        /// </summary>
        /// <param name="paramCoveredRiskTypes">Modelos de negocio ParamCoveredRiskType.</param>
        /// <returns>Modelos de Servicio CoveredRiskTypesServiceModel</returns>
        public static List<PARUPSM.CoveredRiskTypeServiceRelationModel> CreateCoveredRiskTypes(List<ParamCoveredRiskType> paramCoveredRiskTypes)
        {
            List<PARUPSM.CoveredRiskTypeServiceRelationModel> listCoveredRiskTypeServiceModel = new List<PARUPSM.CoveredRiskTypeServiceRelationModel>();
            foreach (ParamCoveredRiskType coveredRiskTypeBusinessModel in paramCoveredRiskTypes)
            {
                PARUPSM.CoveredRiskTypeServiceRelationModel itemCoveredRiskTypeServiceModel = new PARUPSM.CoveredRiskTypeServiceRelationModel();
                itemCoveredRiskTypeServiceModel.Id = coveredRiskTypeBusinessModel.Id;
                itemCoveredRiskTypeServiceModel.SmallDescription = coveredRiskTypeBusinessModel.SmallDescription;
                itemCoveredRiskTypeServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
                listCoveredRiskTypeServiceModel.Add(itemCoveredRiskTypeServiceModel);
            }

            return listCoveredRiskTypeServiceModel;
        }

        /// <summary>
        /// Convierte modelo de ramo técnico a modelo de servicio
        /// </summary>
        /// <param name="paramLineBusiness">Ramo técnico</param>
        /// <returns>Modelo de servicio</returns>
        public static PARUPSM.LineBusinessServiceModel CreateLineBusinessServiceModel(ParamLineBusiness paramLineBusiness)
        {
            return new PARUPSM.LineBusinessServiceModel
            {
                Id = paramLineBusiness.Id,
                Description = paramLineBusiness.Description,
                SmallDescription = paramLineBusiness.SmallDescription,
                TinyDescription = paramLineBusiness.TinyDescription,
                Clauses = CreateClauseServiceModels(paramLineBusiness.Clauses ?? new List<UNDMO.Clause>()),
                CoveredRiskTypes = paramLineBusiness.CoveredRiskTypes,
                InsuredObjects = CreateInsuredObjectServiceModels(paramLineBusiness.InsuredObjects ?? new List<ParamInsuredObjectDesc>()),
                Perils = CreatePerilServiceModels(paramLineBusiness.Perils ?? new List<UNDMO.Peril>()),
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Convierte modelo de ramo técnico a modelo de servicio
        /// </summary>
        /// <param name="paramLineBusinesss">Lista de modelo de ramo técnico</param>
        /// <returns>Lista de modelo de servicio</returns>
        public static List<PARUPSM.LineBusinessServiceModel> CreateLineBusinessServiceModels(List<ParamLineBusiness> paramLineBusinesss)
        {
            List<PARUPSM.LineBusinessServiceModel> lineBusinesses = new List<PARUPSM.LineBusinessServiceModel>();

            foreach (ParamLineBusiness paramLineBusiness in paramLineBusinesss)
            {
                lineBusinesses.Add(CreateLineBusinessServiceModel(paramLineBusiness));
            }

            return lineBusinesses;
        }

        /// <summary>
        /// Convierte modelo de servicio en modelo de cláusula
        /// </summary>
        /// <param name="clauseServiceModels">Modelos de servicio</param>
        /// <returns>Lista de modelos de cláusulas</returns>
        public static List<UNDMO.Clause> CreateClauses(List<ClauseServiceModel> clauseServiceModels)
        {
            List<UNDMO.Clause> clauses = new List<UNDMO.Clause>();

            foreach (ClauseServiceModel clauseServiceModel in clauseServiceModels)
            {
                clauses.Add(CreateClause(clauseServiceModel));
            }

            return clauses;
        }

        /// <summary>
        /// Convierte modelo de servicio en modelo de cláusula
        /// </summary>
        /// <param name="clauseServiceModel">Modelo de servicio</param>
        /// <returns>Modelo de cláusula</returns>
        public static UNDMO.Clause CreateClause(ClauseServiceModel clauseServiceModel)
        {
            return new UNDMO.Clause
            {
                Id = clauseServiceModel.Id,
                Name = clauseServiceModel.Name
            };
        }

        /// <summary>
        /// Convierte modelo de cláusula en modelo de servicio
        /// </summary>
        /// <param name="clauses">Modelos de cláusulas</param>
        /// <returns>Lista de modelos de servicio</returns>
        public static List<ClauseServiceModel> CreateClauseServiceModels(List<UNDMO.Clause> clauses)
        {
            List<ClauseServiceModel> clauseServiceModels = new List<ClauseServiceModel>();

            foreach (UNDMO.Clause clause in clauses)
            {
                clauseServiceModels.Add(CreateClauseServiceModel(clause));
            }

            return clauseServiceModels;
        }

        /// <summary>
        /// convierteModelo de cláusula en modelo de servicio
        /// </summary>
        /// <param name="clause">Modelo de cláusula</param>
        /// <returns>Modelo de servicio</returns>
        public static ClauseServiceModel CreateClauseServiceModel(UNDMO.Clause clause)
        {
            return new ClauseServiceModel
            {
                Id = clause.Id,
                Name = clause.Name,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        #region ClausesServiceQueryModel
        /// <summary>
        /// Convierte modelo de cláusula en modelo se servicio
        /// </summary>
        /// <param name="paramClauseDesc">Modelo de cláusula</param>
        /// <returns>Modelo de servico</returns>
        public static PARUPSM.ClauseServiceQueryModel CreateClausesServiceQueryModel(ParamClauseDesc paramClauseDesc)
        {
            var config = MapperCache.GetMapper<ParamClauseDesc, PARUPSM.ClauseServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamClauseDesc, PARUPSM.ClauseServiceQueryModel>();
            });
            var clausesServiceQueryModel = config.Map<ParamClauseDesc, PARUPSM.ClauseServiceQueryModel>(paramClauseDesc);

            return clausesServiceQueryModel;
        }

        /// <summary>
        /// Convierte modelo de cláusula en modelo se servicio
        /// </summary>
        /// <param name="paramClauseDesc">Modelos de cláusula</param>
        /// <returns>Lista de modelos de servico</returns>
        public static List<PARUPSM.ClauseServiceQueryModel> CreateClauseServiceQueryModels(List<ParamClauseDesc> paramClauseDesc)
        {
            List<PARUPSM.ClauseServiceQueryModel> clauseServiceQueryModels = new List<PARUPSM.ClauseServiceQueryModel>();
            foreach (ParamClauseDesc item in paramClauseDesc)
            {
                clauseServiceQueryModels.Add(CreateClausesServiceQueryModel(item));
            }

            return clauseServiceQueryModels;
        }
        #endregion

        /// <summary>
        /// Convierte Modelo de objeto del seguro a modelo de servicio
        /// </summary>
        /// <param name="paramInsuredObjectDescs">Lista de modelos de objeto del seguro</param>
        /// <returns>Lista de modelos de servicio</returns>
        public static List<PARUPSM.InsuredObjectServiceModel> CreateInsuredObjectServiceModels(List<ParamInsuredObjectDesc> paramInsuredObjectDescs)
        {
            List<PARUPSM.InsuredObjectServiceModel> insuredObjects = new List<PARUPSM.InsuredObjectServiceModel>();

            foreach (ParamInsuredObjectDesc paramInsuredObjectDesc in paramInsuredObjectDescs)
            {
                insuredObjects.Add(CreateInsuredObjectServiceModel(paramInsuredObjectDesc));
            }

            return insuredObjects;
        }

        /// <summary>
        /// Convierte modelo de objeto del seguro a modelo de servicio
        /// </summary>
        /// <param name="paramInsuredObjectDesc">Modelo de objeto del seguro</param>
        /// <returns>Modelo de servicio</returns>
        public static PARUPSM.InsuredObjectServiceModel CreateInsuredObjectServiceModel(ParamInsuredObjectDesc paramInsuredObjectDesc)
        {
            return new PARUPSM.InsuredObjectServiceModel
            {
                Id = paramInsuredObjectDesc.Id,
                Description = paramInsuredObjectDesc.Description,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Convierte modelo de servicio en modelo de objetos del seguro
        /// </summary>
        /// <param name="insuredObjectServiceModels">Modelo de servicio</param>
        /// <returns>Lista de modelo de objeto de seguro</returns>
        public static List<ParamInsuredObjectDesc> CreateParamInsuredObjectDescs(List<PARUPSM.InsuredObjectServiceModel> insuredObjectServiceModels)
        {
            List<ParamInsuredObjectDesc> insuredObjects = new List<ParamInsuredObjectDesc>();

            foreach (PARUPSM.InsuredObjectServiceModel insuredObjectServiceModel in insuredObjectServiceModels)
            {
                insuredObjects.Add(CreateParamInsuredObjectDesc(insuredObjectServiceModel));
            }

            return insuredObjects;
        }

        /// <summary>
        /// Convierte Modelo de servicio a Modelo de objeto del seguro
        /// </summary>
        /// <param name="insuredObjectServiceModel">Modelo de servicio</param>
        /// <returns>Modelo de objeto del seguro</returns>
        public static ParamInsuredObjectDesc CreateParamInsuredObjectDesc(PARUPSM.InsuredObjectServiceModel insuredObjectServiceModel)
        {
            return new ParamInsuredObjectDesc
            {
                Id = insuredObjectServiceModel.Id,
                Description = insuredObjectServiceModel.Description
            };
        }

        /// <summary>
        /// Convierte entidad en modelo de servicio
        /// </summary>
        /// <param name="perils">Modelos de amparo</param>
        /// <returns>Lista de modelos de servicio</returns>
        public static List<PARUPSM.PerilServiceModel> CreatePerilServiceModels(List<UNDMO.Peril> perils)
        {
            List<PARUPSM.PerilServiceModel> perilServiceModels = new List<PARUPSM.PerilServiceModel>();

            foreach (UNDMO.Peril peril in perils)
            {
                perilServiceModels.Add(CreatePerilServiceModel(peril));
            }

            return perilServiceModels;
        }

        /// <summary>
        /// Convierte entidad en modelo de servicio
        /// </summary>
        /// <param name="peril">Modelo de amparo</param>
        /// <returns>Modelo de servicio</returns>
        public static PARUPSM.PerilServiceModel CreatePerilServiceModel(UNDMO.Peril peril)
        {
            return new PARUPSM.PerilServiceModel
            {
                Id = peril.Id,
                Description = peril.Description,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Convierte modelo de servicio en modelo de amparo
        /// </summary>
        /// <param name="perilServiceModels">Modelos de servicio</param>
        /// <returns>Lista de modelos de amparo</returns>
        public static List<UNDMO.Peril> CreatePerils(List<PARUPSM.PerilServiceModel> perilServiceModels)
        {
            List<UNDMO.Peril> perils = new List<UNDMO.Peril>();

            foreach (PARUPSM.PerilServiceModel perilServiceModel in perilServiceModels)
            {
                perils.Add(CreatePeril(perilServiceModel));
            }

            return perils;
        }

        /// <summary>
        /// Convierte modelo de servicio en modelo de amparo
        /// </summary>
        /// <param name="perilServiceModel">Modelo de servicio</param>
        /// <returns>Modelo de amparo</returns>
        public static UNDMO.Peril CreatePeril(PARUPSM.PerilServiceModel perilServiceModel)
        {
            return new UNDMO.Peril
            {
                Id = perilServiceModel.Id,
                Description = perilServiceModel.Description
            };
        }
        #endregion

        #region CoveredRiskTypeQueryServiceModel
        /// <summary>
        /// Convierte modelo de tipo de riesgo cubierto en modelo se servicio
        /// </summary>
        /// <param name="paramCoveredRiskTypeDesc">Modelo de tipo de riesgo cubierto</param>
        /// <returns>Modelo de servico</returns>
        public static MODUN.CoveredRiskTypeQueryServiceModel CreateCoveredRiskTypeQueryServiceModel(ParamCoveredRiskTypeDesc paramCoveredRiskTypeDesc)
        {
            var config = MapperCache.GetMapper<ParamCoveredRiskTypeDesc, MODUN.CoveredRiskTypeQueryServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamCoveredRiskTypeDesc, MODUN.CoveredRiskTypeQueryServiceModel>();
            });
            var coveredRiskTypeQueryServiceModel = config.Map<ParamCoveredRiskTypeDesc, MODUN.CoveredRiskTypeQueryServiceModel>(paramCoveredRiskTypeDesc);

            return coveredRiskTypeQueryServiceModel;
        }

        /// <summary>
        /// Convierte modelo de tipo de riesgo cubierto en modelo se servicio
        /// </summary>
        /// <param name="paramGroupCoverageDescs">Modelos de tipo de riesgo cubierto</param>
        /// <returns>Lista de modelos de servico</returns>
        public static List<MODUN.CoveredRiskTypeQueryServiceModel> CreateCoveredRiskTypeQueryServiceModels(List<ParamCoveredRiskTypeDesc> paramGroupCoverageDescs)
        {
            List<MODUN.CoveredRiskTypeQueryServiceModel> coveredRiskTypeQueryServiceModel = new List<MODUN.CoveredRiskTypeQueryServiceModel>();
            foreach (ParamCoveredRiskTypeDesc item in paramGroupCoverageDescs)
            {
                coveredRiskTypeQueryServiceModel.Add(CreateCoveredRiskTypeQueryServiceModel(item));
            }

            return coveredRiskTypeQueryServiceModel;
        }
        #endregion

        #region RatingZone
        /// <summary>
        /// Convierte el MOD-B de las zonas de tarifacion al MOD-S
        /// </summary>
        /// <param name="paramRatingZoneCities">listado de zonas de tarifacion MOD-B</param>
        /// <returns>listado de zonas de tarifacion MOD-S</returns>
        public static List<PARUPSM.RatingZoneServiceModel> CreateRatingZoneServiceModels(List<ParamRatingZoneCity> paramRatingZoneCities)
        {
            return paramRatingZoneCities.Select(CreateRatingZoneServiceModel).ToList();
        }

        /// <summary>
        /// Convierte el MOD-B de las zonas de tarifacion al MOD-S
        /// </summary>
        /// <param name="paramRatingZoneCity">zonas de tarifacio MOD-B</param>
        /// <returns>Zona de tarifacion MOD-S</returns>
        public static PARUPSM.RatingZoneServiceModel CreateRatingZoneServiceModel(ParamRatingZoneCity paramRatingZoneCity)
        {
            return new PARUPSM.RatingZoneServiceModel
            {
                RatingZoneCode = paramRatingZoneCity.RatingZone.Id,
                Description = paramRatingZoneCity.RatingZone.Description,
                SmallDescription = paramRatingZoneCity.RatingZone.SmallDescription,
                IsDefault = paramRatingZoneCity.RatingZone.IsDefault,
                Prefix = new MODUN.PrefixServiceQueryModel
                {
                    PrefixCode = paramRatingZoneCity.RatingZone.Prefix.Id,
                    PrefixDescription = paramRatingZoneCity.RatingZone.Prefix.Description
                },
                Cities = CreateCityServiceRelationModels(paramRatingZoneCity.Cities)
            };
        }

        /// <summary>
        /// Convierte el MOD-B de las paises al MOD-S
        /// </summary>
        /// <param name="countries">MOD-B de las paises</param>
        /// <returns>MOD-S de las paises</returns>
        public static List<MODCO.CountryServiceQueryModel> CreateCountryServiceQueryModel(List<COMMO.Country> countries)
        {
            List<MODCO.CountryServiceQueryModel> countryServiceQueryModels = new List<MODCO.CountryServiceQueryModel>();
            foreach (COMMO.Country country in countries)
            {
                countryServiceQueryModels.Add(new MODCO.CountryServiceQueryModel
                {
                    Id = country.Id,
                    Description = country.Description,
                });
            }
            return countryServiceQueryModels;
        }

        /// <summary>
        /// Convierte el MOD-B de las estados al MOD-S
        /// </summary>
        /// <param name="states">MOD-B de las estados</param>
        /// <returns>MOD-S de las estados</returns>
        public static List<MODCO.StateServiceQueryModel> CreateStatesServiceQueryModel(List<COMMO.State> states)
        {
            List<MODCO.StateServiceQueryModel> stateServiceQueryModels = new List<MODCO.StateServiceQueryModel>();
            foreach (COMMO.State state in states)
            {
                stateServiceQueryModels.Add(new MODCO.StateServiceQueryModel
                {
                    Id = state.Id,
                    Description = state.Description,
                    Country = new MODCO.CountryServiceQueryModel
                    {
                        Id = state.Country.Id
                    }
                });
            }
            return stateServiceQueryModels;
        }


        /// <summary>
        /// Convierte el MOD-B de las ciudades al MOD-S
        /// </summary>
        /// <param name="cities">MOD-B de las ciudades</param>
        /// <returns>MOD-S de las ciudades</returns>
        public static List<MODCO.CityServiceRelationModel> CreateCityServiceRelationModels(List<COMMO.City> cities)
        {
            List<MODCO.CityServiceRelationModel> cityServiceRelationModels = new List<MODCO.CityServiceRelationModel>();
            foreach (COMMO.City city in cities)
            {
                cityServiceRelationModels.Add(new MODCO.CityServiceRelationModel
                {
                    Id = city.Id,
                    Description = city.Description,
                    State = new MODCO.StateServiceQueryModel
                    {
                        Id = city.State.Id,
                        Description = city.State.Description,
                        Country = new MODCO.CountryServiceQueryModel
                        {
                            Id = city.State.Country.Id,
                            Description = city.State.Country.Description
                        }
                    }
                });
            }
            return cityServiceRelationModels;
        }
        #endregion

        #region Carrocería de vehículo
        /// <summary>
        /// Crea el modelo
        /// </summary>
        /// <param name="serviceModel">Modelo de servicio</param>
        /// <returns>Modelo lectura</returns>
        public static UTIERR.Result<ParamVehicleBodyUse, UTIERR.ErrorModel> CreateVehicleBodyUse(PARUPSM.VehicleBodyServiceModel serviceModel)
        {
            List<string> errorCreateModel = new List<string>();
            UTIERR.Result<ParamVehicleBody, UTIERR.ErrorModel> vehicleBody = ParamVehicleBody.CreateParamVehicleBody(serviceModel.Id, serviceModel.SmallDescription);
            if (vehicleBody is UTIERR.ResultError<ParamVehicleBody, UTIERR.ErrorModel>)
            {
                errorCreateModel.AddRange(((UTIERR.ResultError<ParamVehicleBody, UTIERR.ErrorModel>)vehicleBody).Message.ErrorDescription);
            }
            List<ParamVehicleUse> paramVehicleBodies = new List<ParamVehicleUse>();
            if (serviceModel.VehicleUseServiceQueryModel != null)
            {
                foreach (PARUPSM.VehicleUseServiceQueryModel item in serviceModel.VehicleUseServiceQueryModel)
                {
                    UTIERR.ResultValue<ParamVehicleUse, UTIERR.ErrorModel> vehicleUse = (UTIERR.ResultValue<ParamVehicleUse, UTIERR.ErrorModel>)ParamVehicleUse.GetParamVehicleUse(item.Id, item.Description);
                    paramVehicleBodies.Add(vehicleUse.Value);
                }
            }
            if (errorCreateModel.Count > 0)
            {
                return new UTIERR.ResultError<ParamVehicleBodyUse, UTIERR.ErrorModel>(UTIERR.ErrorModel.CreateErrorModel(errorCreateModel, Utilities.Enums.ErrorType.BusinessFault, null));
            }
            return ParamVehicleBodyUse.CreateParamVehicleBodyUse(((UTIERR.ResultValue<ParamVehicleBody, UTIERR.ErrorModel>)vehicleBody).Value, paramVehicleBodies);
        }
        #endregion Carrocería de vehículo
        #region CoCoverage2G
        /// <summary>
        /// Crea el modelo de servicio
        /// </summary>
        /// <param name="paramCoverages">Modelo de parametrizacion</param>
        /// <returns>Listado de modelo de servicio</returns>
        public static List<PARUPSM.Coverage2GServiceModel> CreateCoverage2GServiceModels(List<ParamCoCoverage2G> paramCoverages)
        {
            List<PARUPSM.Coverage2GServiceModel> coverages2G = new List<PARUPSM.Coverage2GServiceModel>();
            foreach (ParamCoCoverage2G item in paramCoverages)
            {
                coverages2G.Add(CreateCoverage2GServiceModels(item));
            }
            return coverages2G;
        }

        /// <summary>
        /// Crea el modelo de servicio
        /// </summary>
        /// <param name="paramCoverage">Modelo de parametrizacion</param>
        /// <returnsModelo de servicio></returns>
        public static PARUPSM.Coverage2GServiceModel CreateCoverage2GServiceModels(ParamCoCoverage2G paramCoverage)
        {
            return new PARUPSM.Coverage2GServiceModel()
            {
                Description = paramCoverage.Description,
                Id = paramCoverage.Id,
                InsuredObjectId = paramCoverage.InsuredObjectId,
                LineBusinessId = paramCoverage.LineBusinessId,
                SubLineBusinessId = paramCoverage.SubLineBusinessId,
                StatusTypeService = ENUMSM.StatusTypeService.Original
            };
        }
        #endregion CoCoverage2G

        #region FinancialPlan
        /// <summary>
        /// Convierte MOD-B de Metodo de pago a MOD-S   
        /// </summary>
        /// <param name="parametrizationPaymentMethod">Metodo de pago MOD-B</param>
        /// <returns>Plan de pago MOD-S</returns>
        public static PARUPSM.PaymentMethodServiceQueryModel CreatePaymentMethodServiceModel(ParamPaymentMethod parametrizationPaymentMethod)
        {
            var config = MapperCache.GetMapper<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>();
            });
            var paramPaymentPlanServicesModel = config.Map<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>(parametrizationPaymentMethod);

            paramPaymentPlanServicesModel.Id = parametrizationPaymentMethod.Id;
            paramPaymentPlanServicesModel.Description = parametrizationPaymentMethod.Description;


            return paramPaymentPlanServicesModel;
        }

        /// <summary>
        /// Convierte listado del MOD-B de Metodo de pago a MOD-S
        /// </summary>
        /// <param name="parametrizationPaymentMethod">Metodo de pago MOD-B</param>
        /// <returns>Metodo de pago MOD-S</returns>
        public static List<PARUPSM.PaymentMethodServiceQueryModel> CreatePaymentMethodsServiceModel(List<ParamPaymentMethod> listParamPaymentMethod)
        {
            List<PARUPSM.PaymentMethodServiceQueryModel> paymentMethodServiceModels = new List<PARUPSM.PaymentMethodServiceQueryModel>();
            foreach (var item in listParamPaymentMethod)
            {
                paymentMethodServiceModels.Add(CreatePaymentMethodServiceModel(item));
            }

            return paymentMethodServiceModels;
        }


        /// <summary>
        /// Convierte MOD-B de componente a MOD-S   
        /// </summary>
        /// <param name="parametrizationComponent">Componente MOD-B</param>
        /// <returns>Plan de pago MOD-S</returns>
        public static PARUPSM.ComponentRelationServiceModel CreateComponentRelationServiceModel(UNDMO.Component parametrizationComponent)
        {
            var config = MapperCache.GetMapper<UNDMO.Component, PARUPSM.ComponentRelationServiceModel>(cfg =>
            {
                cfg.CreateMap<UNDMO.Component, PARUPSM.ComponentRelationServiceModel>();
            });
            var paramPaymentPlanServicesModel = config.Map<UNDMO.Component, PARUPSM.ComponentRelationServiceModel>(parametrizationComponent);

            return paramPaymentPlanServicesModel;
        }

        /// <summary>
        /// Convierte listado del MOD-B de Componente a MOD-S
        /// </summary>
        /// <param name="component">Componente MOD-B</param>
        /// <returns>Componente MOD-S</returns>
        public static List<PARUPSM.ComponentRelationServiceModel> CreateComponentRelationsServiceModel(List<UNDMO.Component> component)
        {
            List<PARUPSM.ComponentRelationServiceModel> paymentMethodServiceModels = new List<PARUPSM.ComponentRelationServiceModel>();
            foreach (var item in component)
            {
                paymentMethodServiceModels.Add(CreateComponentRelationServiceModel(item));
            }

            return paymentMethodServiceModels;
        }

        /// <summary>
        /// Convierte modelo de negocio a modelo de servicio
        /// </summary>
        /// <param name="paramFinancialPlan">Recibe paramFinancialPlan</param>
        /// <returns>Devuelve modelo de servicio</returns>
        public static PARUPSM.FinancialPlanServiceModel CreateFinancialPlanServiceModel(ParamFinancialPlanComponent paramFinancialPlan)
        {
            var config = MapperCache.GetMapper<ParamFinancialPlanComponent, PARUPSM.FinancialPlanServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamFinancialPlanComponent, PARUPSM.FinancialPlanServiceModel>();
                cfg.CreateMap<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceQueryModel>();
                cfg.CreateMap<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>();
                cfg.CreateMap<ParamCurrency, PARUPSM.CurrencyServiceQueryModel>();
            });
            PARUPSM.FinancialPlanServiceModel financialPlanServiceQueryModel = config.Map<ParamFinancialPlanComponent, PARUPSM.FinancialPlanServiceModel>(paramFinancialPlan);
            financialPlanServiceQueryModel.StatusTypeService = ENUMSM.StatusTypeService.Original;
            financialPlanServiceQueryModel.PaymentPlanServiceQueryModel = config.Map<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceQueryModel>(paramFinancialPlan.ParamFinancialPlan.ParametrizationPaymentPlan);
            financialPlanServiceQueryModel.PaymentMethodServiceQueryModel = config.Map<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>(paramFinancialPlan.ParamFinancialPlan.ParamPaymentMethod);
            financialPlanServiceQueryModel.CurrencyServiceQueryModel = config.Map<ParamCurrency, PARUPSM.CurrencyServiceQueryModel>(paramFinancialPlan.ParamFinancialPlan.ParamCurrency);
            financialPlanServiceQueryModel.FirstPayComponentsServiceModel = CreateFirtsServiceModels(paramFinancialPlan.ParamFirstPayComponent);

            return financialPlanServiceQueryModel;
        }

        /// <summary>
        /// Modelo de negocio a modelo de servicio FinancialPlanServiceModel
        /// </summary>
        /// <param name="paramFinancialPlas">MOD negocio ParamFinancialPlanComponent</param>
        /// <returns>MOD servicio FinancialPlanServiceModel</returns>
        public static List<PARUPSM.FinancialPlanServiceModel> CreateFinancialPlanServiceModels(List<ParamFinancialPlanComponent> paramFinancialPlas)
        {
            List<PARUPSM.FinancialPlanServiceModel> coverageServiceModels = new List<PARUPSM.FinancialPlanServiceModel>();
            foreach (ParamFinancialPlanComponent item in paramFinancialPlas)
            {
                coverageServiceModels.Add(CreateFinancialPlanServiceModel(item));
            }

            return coverageServiceModels;
        }


        /// <summary>
        /// Metodo plan financiero por componente
        /// </summary>
        /// <param name="firtsComponent">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevel</returns>
        public static PARUPSM.FirstPayComponentServiceModel CreateFirtsComponenetServiceModel(ParamFirstPayComponent firtsComponent)
        {
            return new PARUPSM.FirstPayComponentServiceModel()
            {
                FinancialPlanId = firtsComponent.IdFinancialPlan,
                ComponentId = firtsComponent.IdComponent
            };
        }

        /// <summary>
        /// Metodo lista de clausulas por nivel
        /// </summary>
        /// <param name="FirtsComponent">Recibe clauseLevel</param>
        /// <returns>Retorna clauseLevelServiceModel </returns>
        public static List<PARUPSM.FirstPayComponentServiceModel> CreateFirtsComponenetServiceModels(List<ParamFirstPayComponent> FirtsComponent)
        {
            List<PARUPSM.FirstPayComponentServiceModel> clauseLevelServiceModel = new List<PARUPSM.FirstPayComponentServiceModel>();
            foreach (var item in FirtsComponent)
            {
                clauseLevelServiceModel.Add(CreateFirtsComponenetServiceModel(item));
            }

            return clauseLevelServiceModel;
        }

        /// <summary>
        /// Convierte MOD-B de las cuotas al MOD-S
        /// </summary>
        /// <param name="firts">Cuota MOD-B</param>
        /// <returns>Cuota MOD-S</returns>
        public static PARUPSM.FirstPayComponentServiceModel CreateFirtsServiceModel(ParamFirstPayComponent firts)
        {
            return new PARUPSM.FirstPayComponentServiceModel()
            {
                Description = firts.Description,
                ComponentId = firts.IdComponent,
                FinancialPlanId = firts.IdFinancialPlan
            };
        }

        /// <summary>
        /// Convierte el MOD-B de las cuotas al MOD-S
        /// </summary>
        /// <param name="firts">Listado cuotas MOD-B</param>
        /// <returns>Listado cuotas MOD-S</returns>
        public static List<PARUPSM.FirstPayComponentServiceModel> CreateFirtsServiceModels(List<ParamFirstPayComponent> firts)
        {
            List<PARUPSM.FirstPayComponentServiceModel> firtsServiceModel = new List<PARUPSM.FirstPayComponentServiceModel>();
            foreach (var item in firts)
            {
                firtsServiceModel.Add(CreateFirtsServiceModel(item));
            }

            return firtsServiceModel;
        }

        public static PARUPSM.FinancialPlanServiceModel UpdateFinancialPlanServiceModel(ParamFinancialPlanComponent paramFinancialPlan)
        {
            var config = MapperCache.GetMapper<ParamFinancialPlanComponent, PARUPSM.FinancialPlanServiceModel>(cfg =>
            {
                cfg.CreateMap<ParamFinancialPlanComponent, PARUPSM.FinancialPlanServiceModel>();
                cfg.CreateMap<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceQueryModel>();
                cfg.CreateMap<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>();
                cfg.CreateMap<ParamCurrency, PARUPSM.CurrencyServiceQueryModel>();
            });
            PARUPSM.FinancialPlanServiceModel financialPlanServiceQueryModel = config.Map<ParamFinancialPlanComponent, PARUPSM.FinancialPlanServiceModel>(paramFinancialPlan);
            financialPlanServiceQueryModel.StatusTypeService = ENUMSM.StatusTypeService.Update;
            financialPlanServiceQueryModel.PaymentPlanServiceQueryModel = config.Map<ParametrizationPaymentPlan, PARUPSM.PaymentPlanServiceQueryModel>(paramFinancialPlan.ParamFinancialPlan.ParametrizationPaymentPlan);
            financialPlanServiceQueryModel.PaymentMethodServiceQueryModel = config.Map<ParamPaymentMethod, PARUPSM.PaymentMethodServiceQueryModel>(paramFinancialPlan.ParamFinancialPlan.ParamPaymentMethod);
            financialPlanServiceQueryModel.CurrencyServiceQueryModel = config.Map<ParamCurrency, PARUPSM.CurrencyServiceQueryModel>(paramFinancialPlan.ParamFinancialPlan.ParamCurrency);
            financialPlanServiceQueryModel.FirstPayComponentsServiceModel = CreateFirtsServiceModels(paramFinancialPlan.ParamFirstPayComponent);
            financialPlanServiceQueryModel.Id = paramFinancialPlan.ParamFinancialPlan.Id;

            return financialPlanServiceQueryModel;
        }

        /// <summary>
        /// Modelo de negocio a modelo de servicio Coverages
        /// </summary>
        /// <param name="paramCoverages">MOD negocio Coverages</param>
        /// <returns>MOD servicio Coverages</returns>
        public static List<PARUPSM.FinancialPlanServiceModel> UpdateFinancialPlanServiceModels(List<ParamFinancialPlanComponent> paramFinancialPlas)
        {
            List<PARUPSM.FinancialPlanServiceModel> coverageServiceModels = new List<PARUPSM.FinancialPlanServiceModel>();
            foreach (ParamFinancialPlanComponent item in paramFinancialPlas)
            {
                coverageServiceModels.Add(UpdateFinancialPlanServiceModel(item));
            }

            return coverageServiceModels;
        }
        #endregion

        #region Limit Rc

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="paramLimitRc">Lista de LimitRc</param>
        /// <returns>Retorna lista de LimitRcServiceModel</returns>
        public static List<PARUPSM.LimitRcServiceModel> CreateLimitRcServiceModel(List<ParamLimitRc> paramLimitRc)
        {
            List<PARUPSM.LimitRcServiceModel> limitRcServiceModel = new List<PARUPSM.LimitRcServiceModel>();
            foreach (var item in paramLimitRc)
            {
                limitRcServiceModel.Add(CreateLimitRcServiceModel(item));
            }

            return limitRcServiceModel;
        }

        /// <summary>
        /// convierte a modelo de negocio
        /// </summary>
        /// <param name="paramLimitRc">Objeto de LimitRc</param>
        /// <returns>Retorna objeto LimitRcServiceModel</returns>
        public static PARUPSM.LimitRcServiceModel CreateLimitRcServiceModel(ParamLimitRc paramLimitRc)
        {
            PARUPSM.LimitRcServiceModel limitRcServiceModel = new PARUPSM.LimitRcServiceModel();
            limitRcServiceModel.LimitRcCd = paramLimitRc.Id;
            limitRcServiceModel.Limit1 = paramLimitRc.Limit1;
            limitRcServiceModel.Limit2 = paramLimitRc.Limit2;
            limitRcServiceModel.Limit3 = paramLimitRc.Limit3;
            limitRcServiceModel.LimitUnique = Convert.ToDecimal(paramLimitRc.LimitUnique);
            limitRcServiceModel.Description = paramLimitRc.Description;
            limitRcServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            limitRcServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ErrorTypeService.Ok
            };

            return limitRcServiceModel;
        }
        #endregion

        #region Metodos_Pago

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MODUN.PaymentMethodServiceModel CreatePaymentMethodServiceModel(BmParamPaymentMethod paramPaymentMethod)
        {
            var config = MapperCache.GetMapper<BmParamPaymentMethod, MODUN.PaymentMethodServiceModel>(cfg =>
            {
                cfg.CreateMap<BmParamPaymentMethod, MODUN.PaymentMethodServiceModel>();
            });
            MODUN.PaymentMethodServiceModel paymentMethodServiceModel = config.Map<BmParamPaymentMethod, MODUN.PaymentMethodServiceModel>(paramPaymentMethod);
            paymentMethodServiceModel.Id = paramPaymentMethod.Id;
            paymentMethodServiceModel.Description = paramPaymentMethod.Description;
            paymentMethodServiceModel.TinyDescription = paramPaymentMethod.TinyDescription;
            paymentMethodServiceModel.SmallDescription = paramPaymentMethod.SmallDescription;
            paymentMethodServiceModel.PaymentMethodTypeServiceQueryModel = new MODUN.PaymentMethodTypeServiceQueryModel()
            {
                Id = paramPaymentMethod.PaymentMethod.Id,
                Description = paramPaymentMethod.PaymentMethod.Description
            };
            paymentMethodServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            paymentMethodServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };

            return paymentMethodServiceModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethodType"></param>
        /// <returns></returns>
        public static MODUN.PaymentMethodTypeServiceQueryModel CreatePaymentMethodTypeServiceQueryModel(BmParamPaymentMethodType paramPaymentMethodType)
        {
            var config = MapperCache.GetMapper<BmParamPaymentMethodType, MODUN.PaymentMethodTypeServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<BmParamPaymentMethodType, MODUN.PaymentMethodTypeServiceQueryModel>();
            });
            var paymentMethodTypeServiceQueryModel = config.Map<BmParamPaymentMethodType, MODUN.PaymentMethodTypeServiceQueryModel>(paramPaymentMethodType);

            return paymentMethodTypeServiceQueryModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethodTypes"></param>
        /// <returns></returns>
        public static List<MODUN.PaymentMethodTypeServiceQueryModel> CreatePaymentMethodTypeServiceQueryModels(List<BmParamPaymentMethodType> paramPaymentMethodTypes)
        {
            List<MODUN.PaymentMethodTypeServiceQueryModel> listResult = new List<MODUN.PaymentMethodTypeServiceQueryModel>();

            var config = MapperCache.GetMapper<BmParamPaymentMethodType, MODUN.PaymentMethodTypeServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<BmParamPaymentMethodType, MODUN.PaymentMethodTypeServiceQueryModel>();
            });

            foreach (BmParamPaymentMethodType paramPaymentMethodType in paramPaymentMethodTypes)
            {
                var paymentMethodTypeServiceQueryModel = config.Map<BmParamPaymentMethodType, MODUN.PaymentMethodTypeServiceQueryModel>(paramPaymentMethodType);
                listResult.Add(paymentMethodTypeServiceQueryModel);
            }


            return listResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethods"></param>
        /// <returns></returns>
        public static MODUN.PaymentMethodsServiceModel CreatePaymentMethodsServiceModel(List<BmParamPaymentMethod> paramPaymentMethods)
        {

            MODUN.PaymentMethodsServiceModel paymentMethodsServiceModel = new MODUN.PaymentMethodsServiceModel();
            paymentMethodsServiceModel.PaymentMethodServiceModel = new List<MODUN.PaymentMethodServiceModel>();

            foreach (var item in paramPaymentMethods)
            {
                paymentMethodsServiceModel.PaymentMethodServiceModel.Add(CreatePaymentMethodServiceModel(item));
            }

            return paymentMethodsServiceModel;
        }

        #endregion Metodos_Pago
        #region Technical Plan
        public static List<PARUPSM.AllyCoverageServiceModel> CreateAllyCoveragesObjectServiceModel(List<ParamAllyCoverage> insuredObject)
        {
            List<PARUPSM.AllyCoverageServiceModel> allyObjectServiceModel = new List<PARUPSM.AllyCoverageServiceModel>();
            foreach (var item in insuredObject)
            {
                allyObjectServiceModel.Add(CreateAllyCoverageObjectServiceModel(item));
            }
            return allyObjectServiceModel;
        }

        public static PARUPSM.AllyCoverageServiceModel CreateAllyCoverageObjectServiceModel(ParamAllyCoverage coverageObject)
        {
            return new PARUPSM.AllyCoverageServiceModel()
            {
                Id = coverageObject.Id,
                Description = coverageObject.Description,
                AlliedCoveragePercentage = coverageObject.CoveragePercentage,
                StatusTypeService = ENUMSM.StatusTypeService.Original
            };
        }

        public static List<PARUPSM.TechnicalPlanServiceModel> CreateTechnicalPlansServiceModel(List<ParamTechnicalPlanDTO> technicalPlansObject)
        {
            List<PARUPSM.TechnicalPlanServiceModel> technicalPlanServiceModel = new List<PARUPSM.TechnicalPlanServiceModel>();
            foreach (var item in technicalPlansObject)
            {
                technicalPlanServiceModel.Add(CreateTechnicalPlanServiceModel(item));
            }
            return technicalPlanServiceModel;
        }

        public static PARUPSM.TechnicalPlanServiceModel CreateTechnicalPlanServiceModel(ParamTechnicalPlanDTO technicalPlanObject)
        {
            PARUPSM.TechnicalPlanServiceModel modelReturn = new PARUPSM.TechnicalPlanServiceModel();

            modelReturn.Id = technicalPlanObject.TechnicalPlan.Id;
            modelReturn.Description = technicalPlanObject.TechnicalPlan.Description;
            modelReturn.SmallDescription = technicalPlanObject.TechnicalPlan.SmallDescription;
            modelReturn.CoveredRiskType = new PARUPSM.CoveredRiskTypeServiceQueryModel();
            modelReturn.StatusTypeService = ENUMSM.StatusTypeService.Original;
            modelReturn.ErrorServiceModel = new MODPA.ErrorServiceModel()
            {
                ErrorDescription = new List<string>() { "" },
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok
            };
            if (technicalPlanObject.TechnicalPlan.CoveredRiskType != null)
            {
                modelReturn.CoveredRiskType.Id = technicalPlanObject.TechnicalPlan.CoveredRiskType.Id;
                modelReturn.CoveredRiskType.SmallDescription = technicalPlanObject.TechnicalPlan.CoveredRiskType.SmallDescription;
            };
            modelReturn.TechnicalPlanCoverages = new List<PARUPSM.TechnicalPlanCoverageServiceRelationModel>();
            foreach (ParamTechnicalPlansCoverage coverage in technicalPlanObject.TechnicalPlanCoverages)
            {
                PARUPSM.TechnicalPlanCoverageServiceRelationModel smCoverage = new PARUPSM.TechnicalPlanCoverageServiceRelationModel();
                smCoverage.InsuredObject = new PARUPSM.InsuredObjectServiceQueryModel();
                if (coverage.TechnicalPlanCoverage.InsuredObject != null)
                {
                    smCoverage.InsuredObject.Id = coverage.TechnicalPlanCoverage.InsuredObject.Id;
                    smCoverage.InsuredObject.Description = coverage.TechnicalPlanCoverage.InsuredObject.Description;
                }
                smCoverage.Coverage = new PARUPSM.CoverageServiceQueryModel();
                if (coverage.TechnicalPlanCoverage.Coverage != null)
                {
                    smCoverage.Coverage.Id = coverage.TechnicalPlanCoverage.Coverage.Id;
                    smCoverage.Coverage.Description = coverage.TechnicalPlanCoverage.Coverage.Description;
                }
                smCoverage.PrincipalCoverage = new PARUPSM.CoverageServiceQueryModel();
                if (coverage.TechnicalPlanCoverage.PrincipalCoverage != null)
                {
                    smCoverage.PrincipalCoverage.Id = coverage.TechnicalPlanCoverage.PrincipalCoverage.Id;
                    smCoverage.PrincipalCoverage.Description = coverage.TechnicalPlanCoverage.PrincipalCoverage.Description;
                }
                smCoverage.CoveragePercentage = coverage.TechnicalPlanCoverage.CoveragePercentage;
                smCoverage.AlliedCoverages = new List<PARUPSM.AllyCoverageServiceModel>();
                if (coverage.TechnicalPlanAllyCoverages != null)
                {
                    List<ParamAllyCoverage> allyCoverages = coverage.TechnicalPlanAllyCoverages.ToList().Where(x => x.Id == smCoverage.Coverage.Id).ToList();

                    foreach (ParamAllyCoverage ally in allyCoverages)
                    {
                        PARUPSM.AllyCoverageServiceModel allyModel = new PARUPSM.AllyCoverageServiceModel();
                        allyModel.Id = ally.Id;
                        allyModel.Description = ally.Description;
                        allyModel.AlliedCoveragePercentage = ally.CoveragePercentage;
                        smCoverage.AlliedCoverages.Add(allyModel);
                    }
                }
                smCoverage.StatusTypeService = ENUMSM.StatusTypeService.Original;
                modelReturn.TechnicalPlanCoverages.Add(smCoverage);
            }
            return modelReturn;
        }

        public static List<PARUPSM.TechnicalPlanServiceQueryModel> CreateTechnicalPlansServiceModelWithoutCoverages(List<ParamTechnicalPlan> technicalPlansObject)
        {
            List<PARUPSM.TechnicalPlanServiceQueryModel> technicalPlanServiceModel = new List<PARUPSM.TechnicalPlanServiceQueryModel>();
            foreach (var item in technicalPlansObject)
            {
                technicalPlanServiceModel.Add(CreateTechnicalPlanServiceModelWithoutCoverages(item));
            }
            return technicalPlanServiceModel;
        }

        public static PARUPSM.TechnicalPlanServiceQueryModel CreateTechnicalPlanServiceModelWithoutCoverages(ParamTechnicalPlan technicalPlanObject)
        {
            PARUPSM.TechnicalPlanServiceQueryModel modelReturn = new PARUPSM.TechnicalPlanServiceQueryModel();
            modelReturn.Id = technicalPlanObject.Id;
            modelReturn.Description = technicalPlanObject.Description;
            modelReturn.SmallDescription = technicalPlanObject.SmallDescription;
            modelReturn.CoveredRiskType = new PARUPSM.CoveredRiskTypeServiceQueryModel();
            if (technicalPlanObject.CoveredRiskType != null)
            {
                modelReturn.CoveredRiskType.Id = technicalPlanObject.CoveredRiskType.Id;
                modelReturn.CoveredRiskType.SmallDescription = technicalPlanObject.CoveredRiskType.SmallDescription;
            };
            return modelReturn;
        }

        public static List<PARUPSM.TechnicalPlanCoverageServiceRelationModel> CreateTechnicalPlanCoverages(List<ParamTechnicalPlansCoverage> paramCoverages)
        {
            List<PARUPSM.TechnicalPlanCoverageServiceRelationModel> technicalPlanCoverages = new List<PARUPSM.TechnicalPlanCoverageServiceRelationModel>();
            foreach (var item in paramCoverages)
            {
                technicalPlanCoverages.Add(CreateTechnicalPlanCoverage(item));
            }
            return technicalPlanCoverages;
        }

        public static PARUPSM.TechnicalPlanCoverageServiceRelationModel CreateTechnicalPlanCoverage(ParamTechnicalPlansCoverage paramCoverage)
        {
            PARUPSM.TechnicalPlanCoverageServiceRelationModel modelReturn = new PARUPSM.TechnicalPlanCoverageServiceRelationModel();
            modelReturn.Coverage = new PARUPSM.CoverageServiceQueryModel()
            {
                Id = paramCoverage.TechnicalPlanCoverage.Coverage.Id,
                Description = paramCoverage.TechnicalPlanCoverage.Coverage.Description
            };

            if (paramCoverage.TechnicalPlanCoverage.PrincipalCoverage != null)
            {
                modelReturn.PrincipalCoverage = new PARUPSM.CoverageServiceQueryModel()
                {
                    Id = paramCoverage.TechnicalPlanCoverage.PrincipalCoverage.Id,
                    Description = paramCoverage.TechnicalPlanCoverage.PrincipalCoverage.Description
                };
            }
            modelReturn.CoveragePercentage = paramCoverage.TechnicalPlanCoverage.CoveragePercentage;
            modelReturn.InsuredObject = new PARUPSM.InsuredObjectServiceQueryModel()
            {
                Id = paramCoverage.TechnicalPlanCoverage.InsuredObject.Id,
                Description = paramCoverage.TechnicalPlanCoverage.InsuredObject.Description
            };
            modelReturn.AlliedCoverages = new List<PARUPSM.AllyCoverageServiceModel>();
            foreach (var item in paramCoverage.TechnicalPlanAllyCoverages)
            {
                PARUPSM.AllyCoverageServiceModel modelAllyReturn = new PARUPSM.AllyCoverageServiceModel()
                {
                    Id = item.Id,
                    Description = item.Description,
                    AlliedCoveragePercentage = item.CoveragePercentage,
                    StatusTypeService = ENUMSM.StatusTypeService.Original
                };
                modelReturn.AlliedCoverages.Add(modelAllyReturn);
            }
            modelReturn.StatusTypeService = ENUMSM.StatusTypeService.Original;
            return modelReturn;
        }
        #endregion
        #region CompositionTypes
        /// <summary>
        /// Modelo de negocio a modelo de servicio CompositionType
        /// </summary>
        /// <param name="paramCompositionTypes">M. negocio CompositionType</param>
        /// <returns>M. servicio CompositionType</returns>
        public static List<MODUN.CompositionTypeServiceQueryModel> CreateClausesServiceQueryModels(List<ParamComposition> paramCompositionTypes)
        {
            List<MODUN.CompositionTypeServiceQueryModel> compositionTypeServiceQueryModels = new List<MODUN.CompositionTypeServiceQueryModel>();
            foreach (ParamComposition item in paramCompositionTypes)
            {
                compositionTypeServiceQueryModels.Add(CreateCompositionTypeServiceQueryModel(item));
            }

            return compositionTypeServiceQueryModels;
        }
        /// <summary>
        /// Modelo de negocio a modelo de servicio CompositionType
        /// </summary>
        /// <param name="paramComposition">M. negocio CompositionType</param>
        /// <returns>M. servicio CompositionType</returns>
        public static MODUN.CompositionTypeServiceQueryModel CreateCompositionTypeServiceQueryModel(ParamComposition paramCompositionType)
        {
            var config = MapperCache.GetMapper<ParamComposition, MODUN.CompositionTypeServiceQueryModel>(cfg =>
            {
                cfg.CreateMap<ParamComposition, MODUN.CompositionTypeServiceQueryModel>();
            });
            var compositionTypeServiceQueryModel = config.Map<ParamComposition, MODUN.CompositionTypeServiceQueryModel>(paramCompositionType);

            return compositionTypeServiceQueryModel;
        }
        #endregion


        #region Rating Zone

        public static ParamRatingZone CreateRatingZoneEntity(RatingZone entityRatingZone)
        {
            return new ParamRatingZone
            {
                Id = entityRatingZone.RatingZoneCode,
                Description = entityRatingZone.Description,
                SmallDescription = entityRatingZone.SmallDescription,
                IsDefault = entityRatingZone.IsDefault,
                Prefix = new COMMO.Prefix
                {
                    Id = entityRatingZone.PrefixCode,
                }

            };
        }

        public static List<ParamRatingZone> CreateRatingZoneEntitys(List<RatingZone> businessCollection)
        {
            List<ParamRatingZone> companyRatingZones = new List<ParamRatingZone>();

            foreach (RatingZone entityRatingZone in businessCollection)
            {
                companyRatingZones.Add(CreateRatingZoneEntity(entityRatingZone));
            }

            return companyRatingZones;
        }
        #endregion

    }


}
