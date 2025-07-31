// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using UNDERMO = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using PARUSM = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using TAXEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using COMMMO = Sistran.Core.Application.CommonService.Models;
    using MODCO = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Cache;

    /// <summary>
    /// Convierte el Modelo del servicio al modelo del negocio
    /// </summary>
    public static class ServicesModelsAssembler
    {
        #region PaymentPlanServiceModel
        /// <summary>
        /// Convierte el MOD-S del plan de pago al MOD-B
        /// </summary>
        /// <param name="paymentPlanServiceModel">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static ParametrizationPaymentPlan CreateParametrizationPaymentPlan(PARUPSM.PaymentPlanServiceModel paymentPlanServiceModel)
        {
            ParametrizationPaymentPlan parametrizationPaymentPlan = new ParametrizationPaymentPlan();
            //Mapper.CreateMap<PARUPSM.PaymentPlanServiceModel, ParametrizationPaymentPlan>();
            //parametrizationPaymentPlan = Mapper.Map<PARUPSM.PaymentPlanServiceModel, ParametrizationPaymentPlan>(paymentPlanServiceModel);         
            parametrizationPaymentPlan.Id = paymentPlanServiceModel.Id;
            parametrizationPaymentPlan.Description = paymentPlanServiceModel.Description;
            parametrizationPaymentPlan.FirstPayQuantity = paymentPlanServiceModel.FirstPayQuantity;
            parametrizationPaymentPlan.GapQuantity = paymentPlanServiceModel.GapQuantity;
            parametrizationPaymentPlan.GapUnit = paymentPlanServiceModel.GapUnit;
            parametrizationPaymentPlan.IsGreaterDate = paymentPlanServiceModel.IsGreaterDate;
            parametrizationPaymentPlan.IsIssueDate = paymentPlanServiceModel.IsIssueDate;
            parametrizationPaymentPlan.LastPayQuantity = paymentPlanServiceModel.LastPayQuantity;
            parametrizationPaymentPlan.Financing = paymentPlanServiceModel.Financing;
            List<ParametrizationQuota> parametrizacionQuotas = new List<ParametrizationQuota>();
            if (paymentPlanServiceModel.QuotasServiceModel != null)
            {
                foreach (var quotaServiceModel in paymentPlanServiceModel.QuotasServiceModel)
                {
                    List<ParametrizacionQuotaTypeComponent> parametrizacionQuotaTypeComponent = new List<ParametrizacionQuotaTypeComponent>();
                    if (quotaServiceModel.QuotaComponentTypeServiceModel != null)
                    {
                        foreach (var item in quotaServiceModel.QuotaComponentTypeServiceModel)
                        {
                            parametrizacionQuotaTypeComponent.Add(new ParametrizacionQuotaTypeComponent()
                            {
                                Id = item.Id,
                                Value = item.Value,
                                PaymentNumber = quotaServiceModel.Number,
                            });
                        }
                    }

                    parametrizacionQuotas.Add(new ParametrizationQuota()
                    {
                        GapQuantity = quotaServiceModel.GapQuantity,
                        Number = quotaServiceModel.Number,
                        Id = quotaServiceModel.Id,
                        Percentage = quotaServiceModel.Percentage,
                        ListQuotaComponent = parametrizacionQuotaTypeComponent


                    });
                }
            }
            if (parametrizacionQuotas.Count > 0)
            {
                parametrizationPaymentPlan.ParametrizationQuotas = parametrizacionQuotas;
            }

            parametrizationPaymentPlan.Quantity = paymentPlanServiceModel.Quantity;
            parametrizationPaymentPlan.SmallDescription = paymentPlanServiceModel.SmallDescription;
            return parametrizationPaymentPlan;
        }

        /// <summary>
        /// Convierte el listado del MOD-S del plan de pago al MOD-B
        /// </summary>
        /// <param name="paymentPlanServiceModels">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static List<ParametrizationPaymentPlan> CreateParametrizationPaymentPlans(List<PARUPSM.PaymentPlanServiceModel> paymentPlanServiceModels)
        {
            List<ParametrizationPaymentPlan> parametrizationPaymentPlan = new List<ParametrizationPaymentPlan>();
            foreach (var item in paymentPlanServiceModels)
            {
                parametrizationPaymentPlan.Add(CreateParametrizationPaymentPlan(item));
            }

            return parametrizationPaymentPlan;
        }
        #endregion

        #region Quotas
        /// <summary>
        /// Convierte el MOD-S de la cuota al MOD-B
        /// </summary>
        /// <param name="quotaSM">Cuota MOD-S</param>
        /// <returns>Cuota MOD-B</returns>
        public static ParametrizationQuota CreateParametrizationQuota(QuotaServiceModel quotaSM)
        {
            ParametrizationQuota parametrizationQuota = new ParametrizationQuota();

            parametrizationQuota.Percentage = quotaSM.Percentage;
            parametrizationQuota.GapQuantity = quotaSM.GapQuantity;
            parametrizationQuota.ListQuotaComponent = new List<ParametrizacionQuotaTypeComponent>();

            if (quotaSM.QuotaComponentTypeServiceModel != null)
            {
                foreach (QuotaComponentTypeServiceModel item in quotaSM.QuotaComponentTypeServiceModel)
                {
                    parametrizationQuota.ListQuotaComponent.Add(CreatequotaComponentTypeServiceModel(item));
                }
            }

            return parametrizationQuota;
        }

        public static ParametrizacionQuotaTypeComponent CreatequotaComponentTypeServiceModel(QuotaComponentTypeServiceModel quotaComponentTypeServiceModel)
        {
            ParametrizacionQuotaTypeComponent quotaTypeComponent = new ParametrizacionQuotaTypeComponent()
            {
                Id = quotaComponentTypeServiceModel.Id,
                Value = quotaComponentTypeServiceModel.Value
            };

            return quotaTypeComponent;
        }

        /// <summary>
        /// Convierte el listado MOD-S de las cuotas al MOD-B
        /// </summary>
        /// <param name="quotasSM">Cuotas MOD-S</param>
        /// <returns>Cuotas MOD-B</returns>
        public static List<ParametrizationQuota> CreateParametrizationQuotas(List<QuotaServiceModel> quotasSM)
        {
            List<ParametrizationQuota> quotas = new List<ParametrizationQuota>();
            foreach (var item in quotasSM)
            {
                quotas.Add(CreateParametrizationQuota(item));
            }

            return quotas;
        }
        #endregion

        #region DeductibleServiceModel
        /// <summary>
        /// Convierte de servicemodel a modelo negocio
        /// </summary>
        /// <param name="deductibleServiceModel">Modelo DeductibleServiceModel</param>
        /// <returns>Modelo Deductible</returns>
        public static UNDMO.Deductible CreateDeductible(PARUPSM.DeductibleServiceModel deductibleServiceModel)
        {
            UNDMO.Deductible deductible = new UNDMO.Deductible();
            deductible.Id = deductibleServiceModel.Id;
            deductible.Description = deductibleServiceModel.DeductValue.ToString() + " " + deductibleServiceModel.DeductibleUnit.Description + " " + deductibleServiceModel.DeductibleSubject.Description;
            deductible.AccDeductAmt = deductibleServiceModel.AccDeductAmt;
            deductible.Currency = new CommonService.Models.Currency { Id = deductibleServiceModel.Currency.Id, Description = deductibleServiceModel.Currency.Description };
            deductible.DeductibleSubject = new UNDMO.DeductibleSubject { Id = deductibleServiceModel.DeductibleSubject.Id, Description = deductibleServiceModel.DeductibleSubject.Description };
            deductible.DeductibleUnit = new UNDMO.DeductibleUnit { Id = deductibleServiceModel.DeductibleUnit.Id, Description = deductibleServiceModel.DeductibleUnit.Description };
            deductible.DeductPremiumAmount = deductibleServiceModel.DeductPremiumAmount;
            deductible.DeductValue = deductibleServiceModel.DeductValue;
            deductible.IsDefault = deductibleServiceModel.IsDefault;
            deductible.LineBusiness = new CommonService.Models.LineBusiness { Id = deductibleServiceModel.LineBusiness.Id, Description = deductibleServiceModel.LineBusiness.Description };
            deductible.MaxDeductibleSubject = new UNDMO.DeductibleSubject { Id = deductibleServiceModel.MaxDeductibleSubject.Id, Description = deductibleServiceModel.MaxDeductibleSubject.Description };
            deductible.MinDeductibleSubject = new UNDMO.DeductibleSubject { Id = deductibleServiceModel.MinDeductibleSubject.Id, Description = deductibleServiceModel.MinDeductibleSubject.Description };
            deductible.MaxDeductibleUnit = new UNDMO.DeductibleUnit { Id = deductibleServiceModel.MaxDeductibleUnit.Id, Description = deductibleServiceModel.MaxDeductibleUnit.Description };
            deductible.MinDeductibleUnit = new UNDMO.DeductibleUnit { Id = deductibleServiceModel.MinDeductibleUnit.Id, Description = deductibleServiceModel.MinDeductibleUnit.Description };
            deductible.Rate = deductibleServiceModel.Rate;
            deductible.RateType = (TAXEN.RateType)deductibleServiceModel.RateType;
            deductible.MaxDeductValue = deductibleServiceModel.MaxDeductValue;
            deductible.MinDeductValue = deductibleServiceModel.MinDeductValue;
            return deductible;
        }

        /// <summary>
        /// Convierte de servicemodel a modelo negocio
        /// </summary>
        /// <param name="deductibleServiceModel">Modelo DeductibleServiceModel</param>
        /// <returns>Modelo Deductible</returns>
        public static List<UNDMO.Deductible> CreateDeductibles(List<PARUPSM.DeductibleServiceModel> deductibleServiceModel)
        {
            List<UNDMO.Deductible> deductibles = new List<UNDMO.Deductible>();
            foreach (var item in deductibleServiceModel)
            {
                deductibles.Add(CreateDeductible(item));
            }

            return deductibles;
        }
        #endregion

        #region Amparos
        /// <summary>
        /// Convierte de servicemodel a modelo negocio
        /// </summary>
        /// <param name="perilServiceModel">Modelo PerilServiceModel</param>
        /// <returns>Modelo Peril</returns>
        public static UNDMO.Peril CreatePeril(PARUPSM.PerilServiceModel perilServiceModel)
        {
            UNDMO.Peril peril = new UNDMO.Peril();

            peril.Id = perilServiceModel.Id;
            peril.Description = perilServiceModel.Description;
            peril.SmallDescription = perilServiceModel.SmallDescription;

            return peril;
        }

        /// <summary>
        /// Convierte de servicemodel a modelo negocio
        /// </summary>
        /// <param name="deductibleServiceModel">Modelo PerilServiceModel</param>
        /// <returns>Modelo Peril</returns>
        public static List<UNDMO.Peril> CreatePerils(List<PARUPSM.PerilServiceModel> deductibleServiceModel)
        {
            List<UNDMO.Peril> perils = new List<UNDMO.Peril>();
            foreach (var item in deductibleServiceModel)
            {
                perils.Add(CreatePeril(item));
            }

            return perils;
        }
        #endregion

        #region surcharge
        /// <summary>
        /// creacion de componente
        /// </summary>
        /// <param name="surchargeServiceModel">modelo de servicio de recargos</param>
        /// <returns>retorna lista de componentes </returns>
        public static List<ParamSurcharge> CreateComponent(List<SurchargeServiceModel> surchargeServiceModel)
        {
            List<ParamSurcharge> component = new List<ParamSurcharge>();
            foreach (var item in surchargeServiceModel)
            {
                component.Add(CreateSurcharge(item));
            }

            return component;
        }

        /// <summary>
        /// convierte modelo de servicio a modelo de negocio 
        /// </summary>
        /// <param name="surchargeServiceModel">modelo de servicio de recargos</param>
        /// <returns>retorna modelo de negocio </returns>
        public static ParamSurcharge CreateSurcharge(SurchargeServiceModel surchargeServiceModel)
        {
            ParamSurcharge surcharge = new ParamSurcharge();

            surcharge.Id = surchargeServiceModel.Id;
            surcharge.Description = surchargeServiceModel.Description;
            surcharge.Rate = surchargeServiceModel.Rate;
            surcharge.TinyDescription = surchargeServiceModel.TinyDescription;
            surcharge.Type = surchargeServiceModel.Type;

            return surcharge;
        }
        #endregion

        #region Discount
        /// <summary>
        /// creacion de componente
        /// </summary>
        /// <param name="discountServiceModel">modelo de servicio de descuentos</param>
        /// <returns>retorna lista de componentes </returns>
        public static List<ParamDiscount> CreateComponent(List<DiscountServiceModel> discountServiceModel)
        {
            List<ParamDiscount> component = new List<ParamDiscount>();
            foreach (var item in discountServiceModel)
            {
                component.Add(CreateDiscount(item));
            }

            return component;
        }

        /// <summary>
        /// convierte modelo de servicio a modelo de negocio 
        /// </summary>
        /// <param name="discountServiceModel">modelo de servicio de descuentos</param>
        /// <returns>retorna modelo de negocio </returns>
        public static ParamDiscount CreateDiscount(DiscountServiceModel discountServiceModel)
        {
            ParamDiscount discount = new ParamDiscount();

            discount.Id = discountServiceModel.Id;
            discount.Description = discountServiceModel.Description;
            discount.Rate = discountServiceModel.Rate;
            discount.TinyDescription = discountServiceModel.TinyDescription;
            discount.Type = discountServiceModel.Type;

            return discount;
        }
        #endregion

        #region SubLineBusiness
        public static PARUPSM.SubLineBranchServiceModel CreateSubLineBusinessServiceModel(COMMMO.SubLineBusiness subLine)
        {
            var config = MapperCache.GetMapper<COMMMO.SubLineBusiness, PARUPSM.SubLineBranchServiceModel>(cfg =>
            {
                cfg.CreateMap<COMMMO.SubLineBusiness, PARUPSM.SubLineBranchServiceModel>();
            });
            var SublineBusinessServiceModel = config.Map<COMMMO.SubLineBusiness, PARUPSM.SubLineBranchServiceModel>(subLine);

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
        public static List<PARUPSM.SubLineBranchServiceModel> CreateSubLinesBusinessServiceModel(List<COMMMO.SubLineBusiness> subLines)
        {
            List<PARUPSM.SubLineBranchServiceModel> subLinesServiceModel = new List<PARUPSM.SubLineBranchServiceModel>();
            foreach (var item in subLines)
            {
                subLinesServiceModel.Add(CreateSubLineBusinessServiceModel(item));
            }

            return subLinesServiceModel;
        }
        #endregion SubLineBusiness


        #region SubLineBusinessServiceModel
        /// <summary>
        /// Convierte el MOD-S del plan de pago al MOD-B
        /// </summary>
        /// <param name="paymentPlanServiceModel">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static COMMMO.SubLineBusiness CreateParametrizationSubLineBusiness(PARUPSM.SubLineBranchServiceModel subLineBusinessServiceModel)
        {
            COMMMO.SubLineBusiness parametrizationSubLineBusiness = new COMMMO.SubLineBusiness();
            var config = MapperCache.GetMapper<PARUPSM.SubLineBranchServiceModel, COMMMO.SubLineBusiness>(cfg =>
            {
                cfg.CreateMap<PARUPSM.SubLineBranchServiceModel, COMMMO.SubLineBusiness>();
            });

            parametrizationSubLineBusiness = config.Map<PARUPSM.SubLineBranchServiceModel, COMMMO.SubLineBusiness>(subLineBusinessServiceModel);
            parametrizationSubLineBusiness.LineBusinessDescription = subLineBusinessServiceModel.LineBusinessQuery.Description;
            parametrizationSubLineBusiness.LineBusinessId = subLineBusinessServiceModel.LineBusinessQuery.Id;
            return parametrizationSubLineBusiness;

        }

        /// <summary>
        /// Convierte el listado del MOD-S del plan de pago al MOD-B
        /// </summary>
        /// <param name="paymentPlanServiceModels">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static List<COMMMO.SubLineBusiness> CreateParametrizationSubLinesBusiness(List<PARUPSM.SubLineBranchServiceModel> subLineBusinessServiceModels)
        {
            List<COMMMO.SubLineBusiness> parametrizationSubLineBusiness = new List<COMMMO.SubLineBusiness>();
            foreach (var item in subLineBusinessServiceModels)
            {
                parametrizationSubLineBusiness.Add(CreateParametrizationSubLineBusiness(item));
            }

            return parametrizationSubLineBusiness;
        }
        #endregion SubLineBusinessServiceModel

        #region Details
        /// <summary>
        /// Convierte el MOD-S del Detalle al MOD-B
        /// </summary>
        /// <param name="detailServiceModel">Detalle MOD-S</param>
        /// <returns>Detalle MOD-B</returns>
        public static ParametrizationDetail CreateParametrizationDetail(PARUPSM.DetailServiceModel detailServiceModel)
        {
            ParametrizationDetail parametrizationDetail = new ParametrizationDetail();

            parametrizationDetail.Description = detailServiceModel.Description;
            parametrizationDetail.DetailType = new ParametrizationDetailType
            {
                Id = detailServiceModel.DetailType.Id,
                Description = detailServiceModel.DetailType.Description
            };
            parametrizationDetail.Id = detailServiceModel.Id;
            parametrizationDetail.Enabled = detailServiceModel.Enabled;
            if (detailServiceModel.Rate != null)
            {
                parametrizationDetail.Rate = detailServiceModel.Rate.Value;
            }
            if (detailServiceModel.RateType != null)
            {
                parametrizationDetail.RateType = detailServiceModel.RateType.Value;
            }
            if (detailServiceModel.SublimitAmt != null)
            {
                parametrizationDetail.SublimitAmt = detailServiceModel.SublimitAmt.Value;
            }
            return parametrizationDetail;
        }

        /// <summary>
        /// Convierte el listado del MOD-S del detalle al MOD-B
        /// </summary>
        /// <param name="detailServiceModel">Detalle MOD-S</param>
        /// <returns>Detalle MOD-B</returns>
        public static List<ParametrizationDetail> CreateParametrizationDetails(List<PARUPSM.DetailServiceModel> detailServiceModel)
        {
            List<ParametrizationDetail> parametrizationDetail = new List<ParametrizationDetail>();
            foreach (var item in detailServiceModel)
            {
                parametrizationDetail.Add(CreateParametrizationDetail(item));
            }

            return parametrizationDetail;
        }
        #endregion

        public static VehicleTypesServiceModel CreateVehicleTypesServiceModel(Result<List<ParamVehicleTypeBody>, ErrorModel> resultParamVehicleTypeBody)
        {
            if (resultParamVehicleTypeBody is ResultError<List<ParamVehicleTypeBody>, ErrorModel>)
            {
                ResultError<List<ParamVehicleTypeBody>, ErrorModel> resultError = ((ResultError<List<ParamVehicleTypeBody>, ErrorModel>)resultParamVehicleTypeBody);

                return new VehicleTypesServiceModel()
                {
                    ErrorDescription = resultError.Message.ErrorDescription,
                    ErrorTypeService = (ModelServices.Enums.ErrorTypeService)resultError.Message.ErrorType
                };
            }
            else
            {
                ResultValue<List<ParamVehicleTypeBody>, ErrorModel> resultValue = ((ResultValue<List<ParamVehicleTypeBody>, ErrorModel>)resultParamVehicleTypeBody);

                return CreateVehicleTypeServiceModel(resultValue.Value);
            }
        }

        /// <summary>
        /// Crea el modelo de servicio a a partir del parametetro
        /// </summary>
        /// <param name="paramVehicleTypesBodies">Parametro de tipo de vehiculo y carrocerias</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleTypesServiceModel CreateVehicleTypeServiceModel(List<ParamVehicleTypeBody> paramVehicleTypesBodies)
        {
            VehicleTypesServiceModel vehicleTypes = new VehicleTypesServiceModel();
            vehicleTypes.VehicleTypeServiceModel = new List<VehicleTypeServiceModel>();
            foreach (ParamVehicleTypeBody item in paramVehicleTypesBodies)
            {
                vehicleTypes.VehicleTypeServiceModel.Add(CreateVehicleTypeServiceModel(item));
            }
            return vehicleTypes;
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametro
        /// </summary>
        /// <param name="paramVehicleTypeBody">Parametro de tipo de vehiculo y carrocerias</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleTypeServiceModel CreateVehicleTypeServiceModel(ParamVehicleTypeBody paramVehicleTypeBody)
        {
            VehicleTypeServiceModel vehicleTypeModel = new VehicleTypeServiceModel()
            {
                Id = paramVehicleTypeBody.VehicleType.Id,
                Description = paramVehicleTypeBody.VehicleType.Description,
                IsEnable = paramVehicleTypeBody.VehicleType.IsEnable,
                IsTruck = paramVehicleTypeBody.VehicleType.IsTruck,
                SmallDescription = paramVehicleTypeBody.VehicleType.SmallDescription,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
            };
            vehicleTypeModel.VehicleBodyServiceQueryModel = CreateVehicleBodyServiceModel(paramVehicleTypeBody.VehicleBodies);
            return vehicleTypeModel;
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametro
        /// </summary>
        /// <param name="paramVehicleBody">Parametro de tipo de vehiculo y carrocerias</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleBodyServiceQueryModel CreateVehicleBodyServiceModel(ParamVehicleBody paramVehicleBody)
        {
            return new VehicleBodyServiceQueryModel()
            {
                Description = paramVehicleBody.Description,
                Id = paramVehicleBody.Id,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametro
        /// </summary>
        /// <param name="paramsVehicleBodies">Listado de parametros</param>
        /// <returns>Modelo de servicio</returns>
        public static List<VehicleBodyServiceQueryModel> CreateVehicleBodyServiceModel(List<ParamVehicleBody> paramsVehicleBodies)
        {
            List<VehicleBodyServiceQueryModel> vehicleBodies = new List<VehicleBodyServiceQueryModel>();
            foreach (ParamVehicleBody vehicleBody in paramsVehicleBodies)
            {
                vehicleBodies.Add(CreateVehicleBodyServiceModel(vehicleBody));
            }
            return vehicleBodies;
        }

        #region ParamCoverage      
        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio Coverage
        /// </summary>
        /// <param name="coverageServiceModel">MOD servicio Coverage</param>
        /// <returns>MOD negocio Coverage</returns>
        public static ParamCoverage CreateParamCoverage(CoverageServiceModel coverageServiceModel)
        {
            ParamCoverage paramCoverage = new ParamCoverage();
            var config = MapperCache.GetMapper<CoverageServiceModel, ParamCoverage>(cfg =>
            {
                cfg.CreateMap<CoCoverageServiceModel, ParamCoCoverage>();
                cfg.CreateMap<CoverageServiceModel, ParamCoverage>();
                cfg.CreateMap<UNDERMO.PerilServiceQueryModel, ParamPeril>();
                cfg.CreateMap<InsuredObjectServiceQueryModel, ParamInsuredObjectDesc>();
                cfg.CreateMap<LineBusinessServiceQueryModel, ParamLineBusinessDesc>();
                cfg.CreateMap<Coverage2GServiceModel, ParamCoCoverage2G>();
                cfg.CreateMap<UNDERMO.SubLineBusinessServiceQueryModel, ParamSubLineBusinessDesc>();
            });

            paramCoverage = config.Map<CoverageServiceModel, ParamCoverage>(coverageServiceModel);
            paramCoverage.CoCoverage = config.Map<CoCoverageServiceModel, ParamCoCoverage>(coverageServiceModel.CoCoverageServiceModel);
            paramCoverage.Peril = config.Map<UNDERMO.PerilServiceQueryModel, ParamPeril>(coverageServiceModel.Peril);
            paramCoverage.InsuredObjectDesc = config.Map<InsuredObjectServiceQueryModel, ParamInsuredObjectDesc>(coverageServiceModel.InsuredObject);
            paramCoverage.LineBusiness = config.Map<LineBusinessServiceQueryModel, ParamLineBusinessDesc>(coverageServiceModel.LineBusiness);
            paramCoverage.SubLineBusiness = config.Map<UNDERMO.SubLineBusinessServiceQueryModel, ParamSubLineBusinessDesc>(coverageServiceModel.SubLineBusiness);
            return paramCoverage;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio Coverage
        /// </summary>
        /// <param name="coverageServiceModel">MOD servicio Coverage</param>
        /// <returns>MOD negocio Coverage</returns>
        public static List<ParamCoverage> CreateParamCoverages(List<CoverageServiceModel> coverageServiceModel)
        {
            List<ParamCoverage> paramCoverages = new List<ParamCoverage>();
            foreach (CoverageServiceModel item in coverageServiceModel)
            {
                paramCoverages.Add(CreateParamCoverage(item));
            }

            return paramCoverages;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio CoCoverage
        /// </summary>
        /// <param name="cocoverageServiceModel">MOD servicio CoCoverage</param>
        /// <returns>MOD negocio CoCoverage</returns>
        public static ParamCoCoverage CreateParamCoCoverage(CoCoverageServiceModel cocoverageServiceModel)
        {
            var config = MapperCache.GetMapper<CoCoverageServiceModel, ParamCoCoverage>(cfg =>
            {
                cfg.CreateMap<CoCoverageServiceModel, ParamCoCoverage>();
            });

            ParamCoCoverage paramCoCoverage = config.Map<CoCoverageServiceModel, ParamCoCoverage>(cocoverageServiceModel);
            return paramCoCoverage;
        }
        #endregion

        #region ParamClauseDesc
        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio Clause
        /// </summary>
        /// <param name="clauseServiceQueryModel">MOD servicio Clause</param>
        /// <returns>MOD negocio Clause</returns>
        public static ParamClauseDesc CreateParamClauseDesc(ClauseServiceQueryModel clauseServiceQueryModel)
        {
            var config = MapperCache.GetMapper<ClauseServiceQueryModel, ParamClauseDesc>(cfg =>
            {
                cfg.CreateMap<ClauseServiceQueryModel, ParamClauseDesc>();
            });

            ParamClauseDesc paramClauseDesc = config.Map<ClauseServiceQueryModel, ParamClauseDesc>(clauseServiceQueryModel);
            return paramClauseDesc;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio Clauses
        /// </summary>
        /// <param name="clauseServiceQueryModels">MOD servicio Clauses</param>
        /// <returns>MOD negocio Clauses</returns>
        public static List<ParamClauseDesc> CreateParamClauseDescs(List<ClauseServiceQueryModel> clauseServiceQueryModels)
        {
            List<ParamClauseDesc> paramClauseDescs = new List<ParamClauseDesc>();
            foreach (ClauseServiceQueryModel item in clauseServiceQueryModels)
            {
                paramClauseDescs.Add(CreateParamClauseDesc(item));
            }

            return paramClauseDescs;
        }
        #endregion

        #region ParamDeductibleDesc
        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio Deductible
        /// </summary>
        /// <param name="deductibleServiceQueryModel">MOD servicio Deductible</param>
        /// <returns>MOD negocio Deductible</returns>
        public static ParamDeductibleDesc CreateParamDeductibleDesc(DeductibleServiceQueryModel deductibleServiceQueryModel)
        {
            var config = MapperCache.GetMapper<DeductibleServiceQueryModel, ParamDeductibleDesc>(cfg =>
            {
                cfg.CreateMap<DeductibleServiceQueryModel, ParamDeductibleDesc>();
            });

            ParamDeductibleDesc paramDeductibleDesc = config.Map<DeductibleServiceQueryModel, ParamDeductibleDesc>(deductibleServiceQueryModel);
            return paramDeductibleDesc;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio Deductibles
        /// </summary>
        /// <param name="deductibleServiceQueryModels">MOD servicio Deductibles</param>
        /// <returns>MOD negocio Deductibles</returns>
        public static List<ParamDeductibleDesc> CreateParamDeductibleDescs(List<DeductibleServiceQueryModel> deductibleServiceQueryModels)
        {
            List<ParamDeductibleDesc> paramDeductibleDescs = new List<ParamDeductibleDesc>();
            foreach (DeductibleServiceQueryModel item in deductibleServiceQueryModels)
            {
                paramDeductibleDescs.Add(CreateParamDeductibleDesc(item));
            }

            return paramDeductibleDescs;
        }
        #endregion

        #region ParamDetailTypeDesc
        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio DetailType
        /// </summary>
        /// <param name="detailTypeServiceQueryModel">MOD servicio DetailType</param>
        /// <returns>MOD negocio DetailTypes</returns>
        public static ParamDetailTypeDesc CreateParamDetailTypeDesc(DetailTypeServiceQueryModel detailTypeServiceQueryModel)
        {
            var config = MapperCache.GetMapper<DetailTypeServiceQueryModel, ParamDetailTypeDesc>(cfg =>
            {
                cfg.CreateMap<DetailTypeServiceQueryModel, ParamDetailTypeDesc>();
            });

            ParamDetailTypeDesc paramDetailTypeDesc = config.Map<DetailTypeServiceQueryModel, ParamDetailTypeDesc>(detailTypeServiceQueryModel);
            return paramDetailTypeDesc;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio DetailTypes
        /// </summary>
        /// <param name="deductibleServiceQueryModels">MOD servicio DetailTypes</param>
        /// <returns>MOD negocio DetailTypes</returns>
        public static List<ParamDetailTypeDesc> CreateParamDetailTypeDescs(List<DetailTypeServiceQueryModel> deductibleServiceQueryModels)
        {
            List<ParamDetailTypeDesc> paramDetailTypeDescs = new List<ParamDetailTypeDesc>();
            foreach (DetailTypeServiceQueryModel item in deductibleServiceQueryModels)
            {
                paramDetailTypeDescs.Add(CreateParamDetailTypeDesc(item));
            }

            return paramDetailTypeDescs;
        }
        #endregion

        #region InsuredObject
        /// <summary>
        /// Convierte el MOD-S del plan de pago al MOD-B
        /// </summary>
        /// <param name="insuredObjectServiceModel">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static Models.ParamInsuredObject CreateParametrizationInsuredObject(PARUPSM.InsuredObjectServiceModel insuredObjectServiceModel)
        {
            var config = MapperCache.GetMapper<PARUPSM.InsuredObjectServiceModel, Models.ParamInsuredObject>(cfg =>
            {
                cfg.CreateMap<PARUPSM.InsuredObjectServiceModel, Models.ParamInsuredObject>();
            });

            var insuredObject = config.Map<PARUPSM.InsuredObjectServiceModel, Models.ParamInsuredObject>(insuredObjectServiceModel);
            return insuredObject;
        }

        /// <summary>
        /// Convierte el listado del MOD-S del objeto de seguro al MOD-B
        /// </summary>
        /// <param name="insuredObjectServiceModel">objeto del seguro MOD-S</param>
        /// <returns>Objeto del seguro MOD-B</returns>
        public static List<Models.ParamInsuredObject> CreateParametrizationInsurancesObjects(List<PARUPSM.InsuredObjectServiceModel> insuredObjectServiceModel)
        {
            List<Models.ParamInsuredObject> parametrizationPaymentPlan = new List<Models.ParamInsuredObject>();
            foreach (var item in insuredObjectServiceModel)
            {
                parametrizationPaymentPlan.Add(CreateParametrizationInsuredObject(item));
            }

            return parametrizationPaymentPlan;
        }
        #endregion

        #region GrupoCobertura
        /// <summary>
        /// Convierte de servicemodel a modelo negocio
        /// </summary>
        /// <param name="deductibleServiceModel">Modelo DeductibleServiceModel</param>
        /// <returns>Modelo Deductible</returns>
        public static ParametrizationCoverageGroupRiskType CreateCoverageGroup(PARUPSM.CoverageGroupRiskTypeServiceModel coverageGroupRiskTypeServiceModel)
        {
            ParametrizationCoverageGroupRiskType parametrizationCoverageGroup = new ParametrizationCoverageGroupRiskType();

            parametrizationCoverageGroup.CoverageRiskType = new ParamCoveredRiskType(coverageGroupRiskTypeServiceModel.CoverageRiskType.Id, coverageGroupRiskTypeServiceModel.CoverageRiskType.SmallDescription);
            parametrizationCoverageGroup.Description = coverageGroupRiskTypeServiceModel.Description;
            parametrizationCoverageGroup.SmallDescription = coverageGroupRiskTypeServiceModel.SmallDescription;
            parametrizationCoverageGroup.IdCoverGroupRisk = coverageGroupRiskTypeServiceModel.IdCoverGroupRisk;
            parametrizationCoverageGroup.IdCoverageGroup = coverageGroupRiskTypeServiceModel.IdCoverageGroup;
            parametrizationCoverageGroup.Enabled = coverageGroupRiskTypeServiceModel.Enabled;

            return parametrizationCoverageGroup;
        }

        /// <summary>
        /// Convierte de servicemodel a modelo negocio
        /// </summary>
        /// <param name="deductibleServiceModel">Modelo DeductibleServiceModel</param>
        /// <returns>Modelo Deductible</returns>
        public static List<ParametrizationCoverageGroupRiskType> CreateCoverageGroups(List<PARUPSM.CoverageGroupRiskTypeServiceModel> coverageGroupRiskTypeServiceModels)
        {
            List<ParametrizationCoverageGroupRiskType> parametrizationCoverageGroupRiskType = new List<ParametrizationCoverageGroupRiskType>();
            foreach (var item in coverageGroupRiskTypeServiceModels)
            {
                parametrizationCoverageGroupRiskType.Add(CreateCoverageGroup(item));
            }

            return parametrizationCoverageGroupRiskType;
        }
        #endregion

        #region Component

        #endregion

        #region ClauseServiceModel
        /// <summary>
        /// Metodo clausula
        /// </summary>
        /// <param name="clauseServiceModel">Recibe clauseServiceModel</param>
        /// <returns>Retorna ParamClause</returns>
        public static ParamClause CreateParametrizationClause(UNDERMO.ClauseServiceModel clauseServiceModel)
        {
            return new ParamClause()
            {
                Clause = new UNDMO.Clause
                {
                    Id = clauseServiceModel.Id,
                    Name = clauseServiceModel.Name,
                    Title = clauseServiceModel.Title,
                    ConditionLevel = new UNDMO.ConditionLevel
                    {
                        Id = clauseServiceModel.ConditionLevelServiceQueryModel.Id,
                        Description = clauseServiceModel.ConditionLevelServiceQueryModel.Description
                    },
                    Text = clauseServiceModel.ClauseText,
                    IsMandatory = clauseServiceModel.ClauseLevelServiceModel.IsMandatory
                },
                ParamClausePrefix = (clauseServiceModel.PrefixServiceQueryModel == null) ? new ParamClausePrefix() : new ParamClausePrefix
                {
                    Id = clauseServiceModel.PrefixServiceQueryModel.PrefixCode,
                    Description = clauseServiceModel.PrefixServiceQueryModel.PrefixDescription,
                    SmallDescription = clauseServiceModel.PrefixServiceQueryModel.PrefixSmallDescription
                },
                InputStartDate = clauseServiceModel.InputStartDate,
                DueDate = clauseServiceModel.DueDate,

            };
        }

        /// <summary>
        /// Metodo lista de clausulas 
        /// </summary>
        /// <param name="clauseServiceModels">Recibe clauseServiceModels</param>
        /// <returns>Retorna parametrizationClause</returns>
        public static List<ParamClause> CreateParametrizationClauses(List<UNDERMO.ClauseServiceModel> clauseServiceModels)
        {
            List<ParamClause> parametrizationClause = new List<ParamClause>();
            foreach (var item in clauseServiceModels)
            {
                parametrizationClause.Add(CreateParametrizationClause(item));
            }

            return parametrizationClause;
        }
        #endregion ClauseServiceModel

        #region ClauseLevel
        /// <summary>
        /// Metodo nivel de clausula
        /// </summary>
        /// <param name="clauseLevelSM">Recibe clauseLevelSM</param>
        /// <returns>Retorna ParamClauseLevel</returns>
        public static ParamClauseLevel CreateClauseLevel(UNDERMO.ClauseLevelServiceModel clauseLevelSM)
        {
            return new ParamClauseLevel()
            {
                ClauseId = clauseLevelSM.ClauseId,
                ConditionLevelId = clauseLevelSM.ConditionLevelId,
                IsMandatory = clauseLevelSM.IsMandatory
            };
        }

        /// <summary>
        /// Metodo lista nivel por clausula 
        /// </summary>
        /// <param name="clauseLevelSM">Recibe clauseLevelSM</param>
        /// <returns>Retorna lista ParamClauseLevel </returns>
        public static List<ParamClauseLevel> CreateClauseLevels(List<UNDERMO.ClauseLevelServiceModel> clauseLevelSM)
        {
            List<ParamClauseLevel> levels = new List<ParamClauseLevel>();
            foreach (var item in clauseLevelSM)
            {
                levels.Add(CreateClauseLevel(item));
            }

            return levels;
        }
        #endregion ClauseLevel

        /// <summary>
        /// Mapeo lista modelo de servicio businessServiceModel a modelo de negocio.
        /// </summary>
        /// <param name="businessServiceModel">>modelo de servicio de BusinessServiceModel</param>
        /// <returns>Modelo de negocio o error</returns>
        public static Result<ParamBusiness, ErrorModel> MappBusiness(UNDERMO.BusinessServiceModel businessServiceModel)
        {
            List<string> errorModelListDescription = new List<string>();
            Result<ParamPrefix, ErrorModel> paramPrefix = ParamPrefix.GetParamPrefix(businessServiceModel.PrefixCode.PrefixCode, businessServiceModel.PrefixCode.PrefixDescription, businessServiceModel.PrefixCode.PrefixSmallDescription);
            if (paramPrefix is ResultError<ParamPrefix, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamPrefix);
                return new ResultError<ParamBusiness, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            Result<ParamBusiness, ErrorModel> paramBusiness = ParamBusiness.CreateParamBusiness(businessServiceModel.BusinessId, businessServiceModel.Description, businessServiceModel.IsEnabled, (paramPrefix as ResultValue<ParamPrefix, ErrorModel>).Value);
            if (paramBusiness is ResultError<ParamBusiness, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamBusiness);
                return new ResultError<ParamBusiness, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }

            return paramBusiness;
        }

        /// <summary>
        /// Mapeo lista modelo de servicio businessServiceModel a modelo de negocio.
        /// </summary>
        /// <param name="businessServiceModel">>modelo de servicio de BusinessServiceModel</param>
        /// <returns>Modelo de negocio o error</returns>
        public static Result<ParamBusinessConfiguration, ErrorModel> MappBusinessConfiguration(UNDERMO.BusinessConfigurationServiceModel businessConfigurationServiceModel)
        {
            List<string> errorModelListDescription = new List<string>();
            Result<ParamRequestEndorsement, ErrorModel> paramRequestEndorsement;
            if (businessConfigurationServiceModel.Request != null)
            {
                paramRequestEndorsement = ParamRequestEndorsement.GetParamRequestEndorsement(businessConfigurationServiceModel.Request.RequestEndorsementId, businessConfigurationServiceModel.Request.RequestId, businessConfigurationServiceModel.Request.ProductId, businessConfigurationServiceModel.Request.PrefixCode);
            }
            else
            {
                paramRequestEndorsement = null;
            }
            Result<ParamProduct, ErrorModel> paramProduct = ParamProduct.GetParamProduct(businessConfigurationServiceModel.Product.ProductId, businessConfigurationServiceModel.Product.ProductDescription, businessConfigurationServiceModel.Product.ProductSmallDescription, businessConfigurationServiceModel.Product.ActiveProduct);
            if (paramProduct is ResultError<ParamProduct, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamProduct);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            Result<ParamGroupCoverage, ErrorModel> paramGroupCoverage = ParamGroupCoverage.GetParamGroupCoverage(businessConfigurationServiceModel.GroupCoverage.GroupCoverageId, businessConfigurationServiceModel.GroupCoverage.GroupCoverageSmallDescription);
            if (paramGroupCoverage is ResultError<ParamGroupCoverage, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamGroupCoverage);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            Result<ParamAssistanceType, ErrorModel> paramAssistanceType = ParamAssistanceType.GetParamAssistanceType(businessConfigurationServiceModel.Assistance.AssistanceCode, businessConfigurationServiceModel.Assistance.AssistanceDescription);
            if (paramAssistanceType is ResultError<ParamAssistanceType, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamAssistanceType);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            Result<ParamBusinessConfiguration, ErrorModel> paramBusinessConfiguration;
            if (businessConfigurationServiceModel.Request != null)
            {
                paramBusinessConfiguration = ParamBusinessConfiguration.CreateParamBusinessConfiguration(businessConfigurationServiceModel.BusinessConfigurationId, businessConfigurationServiceModel.BusinessId, (paramRequestEndorsement as ResultValue<ParamRequestEndorsement, ErrorModel>).Value, (paramProduct as ResultValue<ParamProduct, ErrorModel>).Value, (paramGroupCoverage as ResultValue<ParamGroupCoverage, ErrorModel>).Value, (paramAssistanceType as ResultValue<ParamAssistanceType, ErrorModel>).Value, businessConfigurationServiceModel.ProductIdResponse);
            }
            else
            {
                paramBusinessConfiguration = ParamBusinessConfiguration.CreateParamBusinessConfiguration(businessConfigurationServiceModel.BusinessConfigurationId, businessConfigurationServiceModel.BusinessId, null, (paramProduct as ResultValue<ParamProduct, ErrorModel>).Value, (paramGroupCoverage as ResultValue<ParamGroupCoverage, ErrorModel>).Value, (paramAssistanceType as ResultValue<ParamAssistanceType, ErrorModel>).Value, businessConfigurationServiceModel.ProductIdResponse);
            }
            if (paramBusinessConfiguration is ResultError<ParamBusinessConfiguration, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamBusinessConfiguration);
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            else
            {
                return paramBusinessConfiguration;
            }
        }

        /// <summary>
        /// Mapeo lista modelo de servicio businessServiceModel a modelo de negocio.
        /// </summary>
        /// <param name="businessServiceModel">>modelo de servicio de BusinessServiceModel</param>
        /// <returns>Modelo de negocio o error</returns>
        public static Result<ParamBusinessParamBusinessConfiguration, ErrorModel> MappBusinessBusinessConfiguration(UNDERMO.BusinessServiceModel businessServiceModel)
        {
            List<string> errorModelListDescription = new List<string>();
            List<ParamBusinessConfiguration> listParamBusinessConfiguration = new List<ParamBusinessConfiguration>();
            Result<ParamPrefix, ErrorModel> paramPrefix = ParamPrefix.GetParamPrefix(businessServiceModel.PrefixCode.PrefixCode, businessServiceModel.PrefixCode.PrefixDescription, businessServiceModel.PrefixCode.PrefixSmallDescription);

            Result<ParamBusiness, ErrorModel> paramBusiness = MappBusiness(businessServiceModel);
            if (paramBusiness is ResultError<ParamBusiness, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamBusiness);
                return new ResultError<ParamBusinessParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }
            if (businessServiceModel.BusinessConfiguration != null)
            {
                foreach (UNDERMO.BusinessConfigurationServiceModel businessConfigurationServiceModel in businessServiceModel.BusinessConfiguration)
                {
                    Result<ParamBusinessConfiguration, ErrorModel> paramBusinessConfiguration = MappBusinessConfiguration(businessConfigurationServiceModel);
                    if (paramBusinessConfiguration is ResultError<ParamBusinessConfiguration, ErrorModel>)
                    {
                        errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamBusinessConfiguration);
                        return new ResultError<ParamBusinessParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        listParamBusinessConfiguration.Add((paramBusinessConfiguration as ResultValue<ParamBusinessConfiguration, ErrorModel>).Value);
                    }
                }
            }
            Result<ParamBusinessParamBusinessConfiguration, ErrorModel> paramBusinessResult = ParamBusinessParamBusinessConfiguration.CreateParamBusinessParamBusinessConfiguration((paramBusiness as ResultValue<ParamBusiness, ErrorModel>).Value, listParamBusinessConfiguration);
            if (paramBusinessResult is ResultError<ParamBusinessParamBusinessConfiguration, ErrorModel>)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.ErrorMappingParamBusiness);
                return new ResultError<ParamBusinessParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
            }

            return paramBusinessResult;
        }

        #region RatingZone
        /// <summary>
        /// Convierte el MOD-S de la zona de tarifacion al MOD-B
        /// </summary>
        /// <param name="ratingZoneServiceModel">zona de tarifacion MOD-S</param>
        /// <returns>zona de tarifacion MOD-B</returns>
        public static ParamRatingZoneCity CreateParamRatingZoneCity(RatingZoneServiceModel ratingZoneServiceModel)
        {
            return new ParamRatingZoneCity
            {
                RatingZone = new ParamRatingZone
                {
                    Id = ratingZoneServiceModel.RatingZoneCode,
                    Description = ratingZoneServiceModel.Description,
                    SmallDescription = ratingZoneServiceModel.SmallDescription,
                    IsDefault = ratingZoneServiceModel.IsDefault,
                    Prefix = new COMMMO.Prefix
                    {
                        Id = ratingZoneServiceModel.Prefix.PrefixCode
                    }
                },
                Cities = CreateCities(ratingZoneServiceModel.Cities)
            };
        }

        /// <summary>
        /// Convierte el MOD-S de Ciudades al MOD-B
        /// </summary>
        /// <param name="cityServiceRelationModels">Ciudades MOD-S</param>
        /// <returns>Ciudades MOD-B</returns>
        public static List<COMMMO.City> CreateCities(List<MODCO.CityServiceRelationModel> cityServiceRelationModels)
        {
            List<COMMMO.City> cities = new List<COMMMO.City>();
            foreach (MODCO.CityServiceRelationModel city in cityServiceRelationModels)
            {
                cities.Add(new COMMMO.City
                {
                    Id = city.Id,
                    Description = city.Description,
                    State = new COMMMO.State
                    {
                        Id = city.State.Id,
                        Description = city.State.Description,
                        Country = new COMMMO.Country
                        {
                            Id = city.State.Country.Id,
                            Description = city.State.Country.Description
                        }
                    }
                });
            }

            return cities;
        }
        #endregion

        #region Carrocería de vehículo
        /// <summary>
        /// Método para mapear de resultParamVehicleBodyUse a VehicleBodiesServiceModel
        /// </summary>
        /// <param name="resultParamVehicleBodyUse">Listado de ParamVehicleBodyUse</param>
        /// <returns>Lista de VehicleBodiesServiceModel</returns>
        public static VehicleBodiesServiceModel CreateVehicleBodiesServiceModel(Result<List<ParamVehicleBodyUse>, ErrorModel> resultParamVehicleBodyUse)
        {
            if (resultParamVehicleBodyUse is ResultError<List<ParamVehicleBodyUse>, ErrorModel>)
            {
                ResultError<List<ParamVehicleBodyUse>, ErrorModel> resultError = ((ResultError<List<ParamVehicleBodyUse>, ErrorModel>)resultParamVehicleBodyUse);

                return new VehicleBodiesServiceModel()
                {
                    ErrorDescription = resultError.Message.ErrorDescription,
                    ErrorTypeService = (ModelServices.Enums.ErrorTypeService)resultError.Message.ErrorType
                };
            }
            else
            {
                ResultValue<List<ParamVehicleBodyUse>, ErrorModel> resultValue = ((ResultValue<List<ParamVehicleBodyUse>, ErrorModel>)resultParamVehicleBodyUse);

                return CreateVehicleBodyServiceModel(resultValue.Value);
            }
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametetro
        /// </summary>
        /// <param name="paramVehicleBodyUses">Parametro de tipo de vehiculo y carrocerias</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleBodiesServiceModel CreateVehicleBodyServiceModel(List<ParamVehicleBodyUse> paramVehicleBodyUses)
        {
            VehicleBodiesServiceModel vehicleBodies = new VehicleBodiesServiceModel();
            vehicleBodies.VehicleBodyServiceModel = new List<VehicleBodyServiceModel>();
            foreach (ParamVehicleBodyUse item in paramVehicleBodyUses)
            {
                vehicleBodies.VehicleBodyServiceModel.Add(CreateVehicleBodyServiceModel(item));
            }
            return vehicleBodies;
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametro
        /// </summary>
        /// <param name="paramVehicleBodyUse">Parametro de carroceria y usos</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleBodyServiceModel CreateVehicleBodyServiceModel(ParamVehicleBodyUse paramVehicleBodyUse)
        {
            VehicleBodyServiceModel vehicleBodyModel = new VehicleBodyServiceModel()
            {
                Id = paramVehicleBodyUse.VehicleBody.Id,
                SmallDescription = paramVehicleBodyUse.VehicleBody.Description,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original,
            };
            vehicleBodyModel.VehicleUseServiceQueryModel = CreateVehicleUseServiceModel(paramVehicleBodyUse.VehicleUses);
            return vehicleBodyModel;
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametro
        /// </summary>
        /// <param name="paramVehicleUse">Parametro de carroceria y usos</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleUseServiceQueryModel CreateVehicleUseServiceModel(ParamVehicleUse paramVehicleUse)
        {
            return new VehicleUseServiceQueryModel()
            {
                Description = paramVehicleUse.Description,
                Id = paramVehicleUse.Id,
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Crea el modelo de servicio a partir del parametro
        /// </summary>
        /// <param name="paramsVehicleUses">Listado de parametros</param>
        /// <returns>Modelo de servicio</returns>
        public static List<VehicleUseServiceQueryModel> CreateVehicleUseServiceModel(List<ParamVehicleUse> paramsVehicleUses)
        {
            List<VehicleUseServiceQueryModel> vehicleUses = new List<VehicleUseServiceQueryModel>();
            foreach (ParamVehicleUse vehicleUse in paramsVehicleUses)
            {
                vehicleUses.Add(CreateVehicleUseServiceModel(vehicleUse));
            }
            return vehicleUses;
        }
        #endregion Carrocería de vehículo

        #region FinancialPlan
        /// <summary>
        /// Convierte el MOD-S del plan financiero al MOD-B
        /// </summary>
        /// <param name="paymentPlanServiceModel">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static ParamFinancialPlanComponent CreateParametrizationFinancialPlan(PARUPSM.FinancialPlanServiceModel financialPlanServiceModel)
        {
            ParamFinancialPlanComponent parametrizationFinancialPlan = new ParamFinancialPlanComponent();
            var config = MapperCache.GetMapper<PARUPSM.FinancialPlanServiceModel, ParamFinancialPlanComponent>(cfg =>
            {
                cfg.CreateMap<PARUPSM.FinancialPlanServiceModel, ParamFinancialPlanComponent>();
            });

            parametrizationFinancialPlan = config.Map<PARUPSM.FinancialPlanServiceModel, ParamFinancialPlanComponent>(financialPlanServiceModel);
            //parametrizationFinancialPlan.ParamFirstPayComponent = new List<ParamFirstPayComponent>();

            //parametrizationFinancialPlan.ParamFinancialPlan.ParametrizationPaymentPlan.Description = financialPlanServiceModel.PaymentPlanServiceQueryModel.Description;
            parametrizationFinancialPlan.ParamFinancialPlan = new ParamFinancialPlan()
            {
                Id = financialPlanServiceModel.Id,
                ParamPaymentMethod = new ParamPaymentMethod()
                {
                    Id = financialPlanServiceModel.PaymentMethodServiceQueryModel.Id,
                    Description = financialPlanServiceModel.PaymentMethodServiceQueryModel.Description

                },
                ParamCurrency = new ParamCurrency()
                {
                    Id = financialPlanServiceModel.CurrencyServiceQueryModel.Id,
                    Description = financialPlanServiceModel.CurrencyServiceQueryModel.Description
                },
                ParametrizationPaymentPlan = new ParametrizationPaymentPlan()
                {
                    Id = financialPlanServiceModel.PaymentPlanServiceQueryModel.Id,
                    Description = financialPlanServiceModel.PaymentPlanServiceQueryModel.Description
                }
            };
            parametrizationFinancialPlan.ParamFirstPayComponent = CreateParamFirtsComponents(financialPlanServiceModel.FirstPayComponentsServiceModel);
            //Component = financialPlanServiceModel.FirstPayComponentsServiceModel;
            //parametrizationFinancialPlan.ParamFirstPayComponent = CreateParamFirtsComponents(financialPlanServiceModel.FirstPayComponentsServiceModel);


            //parametrizationFinancialPlan.ParamFinancialPlan.ParamCurrency.Description = financialPlanServiceModel.CurrencyServiceQueryModel.Description;
            return parametrizationFinancialPlan;
        }

        /// <summary>
        /// Convierte el listado del MOD-S del plan de pago al MOD-B
        /// </summary>
        /// <param name="paymentPlanServiceModels">Plan de pago MOD-S</param>
        /// <returns>Plan de pago MOD-B</returns>
        public static List<ParamFinancialPlanComponent> CreateParametrizationFinancialPlans(List<PARUPSM.FinancialPlanServiceModel> paymentPlanServiceModels)
        {
            List<ParamFinancialPlanComponent> parametrizationPaymentPlan = new List<ParamFinancialPlanComponent>();
            foreach (var item in paymentPlanServiceModels)
            {
                parametrizationPaymentPlan.Add(CreateParametrizationFinancialPlan(item));
            }

            return parametrizationPaymentPlan;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio FirtsComponent
        /// </summary>
        /// <param name="firstComponentServiceQueryModel">MOD servicio FirtsComponent</param>
        /// <returns>MOD negocio FirtsComponent</returns>
        public static ParamFirstPayComponent CreateParamFirtsComponent(FirstPayComponentServiceModel firstComponentServiceQueryModel)
        {
            var config = MapperCache.GetMapper<FirstPayComponentServiceModel, ParamFirstPayComponent>(cfg =>
            {
                cfg.CreateMap<FirstPayComponentServiceModel, ParamFirstPayComponent>();
            });

            ParamFirstPayComponent paramFirtsComponent = config.Map<FirstPayComponentServiceModel, ParamFirstPayComponent>(firstComponentServiceQueryModel);
            paramFirtsComponent.IdComponent = firstComponentServiceQueryModel.ComponentId;
            paramFirtsComponent.IdFinancialPlan = firstComponentServiceQueryModel.FinancialPlanId;
            return paramFirtsComponent;
        }

        /// <summary>
        /// Convierte modelo de servicio a modelo de negocio FirtsComponent
        /// </summary>
        /// <param name="firstComponentServiceQueryModel">MOD servicio FirtsComponent</param>
        /// <returns>MOD negocio FirtsComponent</returns>
        public static List<ParamFirstPayComponent> CreateParamFirtsComponents(List<FirstPayComponentServiceModel> firstComponentServiceQueryModel)
        {
            List<ParamFirstPayComponent> paramFirtsComponent = new List<ParamFirstPayComponent>();
            foreach (FirstPayComponentServiceModel item in firstComponentServiceQueryModel)
            {
                paramFirtsComponent.Add(CreateParamFirtsComponent(item));
            }

            return paramFirtsComponent;
        }
        #endregion

        #region Limit Rc

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitRcServiceModel"></param>
        /// <returns></returns>
        public static List<ParamLimitRc> CreateLimitsRc(List<LimitRcServiceModel> limitRcServiceModel)
        {
            List<ParamLimitRc> component = new List<ParamLimitRc>();
            foreach (var item in limitRcServiceModel)
            {
                component.Add(CreateLimitRc(item));
            }

            return component;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitRcServiceModel"></param>
        /// <returns></returns>
        public static ParamLimitRc CreateLimitRc(LimitRcServiceModel limitRcServiceModel)
        {
            ParamLimitRc limitRc = new ParamLimitRc();

            limitRc.Id = limitRcServiceModel.LimitRcCd;
            limitRc.Limit1 = limitRcServiceModel.Limit1;
            limitRc.Limit2 = limitRcServiceModel.Limit2;
            limitRc.Limit3 = limitRcServiceModel.Limit3;
            limitRc.LimitUnique = limitRcServiceModel.LimitUnique.ToString();
            limitRc.Description = limitRcServiceModel.Description;

            return limitRc;
        }

        #endregion

        #region Metodos_Pago

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethodType"></param>
        /// <returns></returns>
        public static BmParamPaymentMethodType CreateParamPaymentMethodType(UNDERMO.PaymentMethodTypeServiceQueryModel paramPaymentMethodType)
        {
            Result<BmParamPaymentMethodType, ErrorModel> paramPaymentMethodTypeResult = BmParamPaymentMethodType.GetParamPayMethodType(paramPaymentMethodType.Id, paramPaymentMethodType.Description);
            BmParamPaymentMethodType result = ((ResultValue<BmParamPaymentMethodType, ErrorModel>)paramPaymentMethodTypeResult).Value;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethod"></param>
        /// <returns></returns>
        public static BmParamPaymentMethod CreateParamPaymentMethod(UNDERMO.PaymentMethodServiceModel paramPaymentMethod)
        {
            BmParamPaymentMethodType type = CreateParamPaymentMethodType(paramPaymentMethod.PaymentMethodTypeServiceQueryModel);

            Result<BmParamPaymentMethod, ErrorModel> paramPaymentMethodResult = BmParamPaymentMethod.GetParamPaymentMethod(paramPaymentMethod.Id, paramPaymentMethod.Description, paramPaymentMethod.TinyDescription, paramPaymentMethod.SmallDescription, type);
            BmParamPaymentMethod result = ((ResultValue<BmParamPaymentMethod, ErrorModel>)paramPaymentMethodResult).Value;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethod"></param>
        /// <returns></returns>
        public static List<BmParamPaymentMethod> CreateParamPaymentMethods(List<UNDERMO.PaymentMethodServiceModel> listPaymentMethodServiceModel)
        {
            List<BmParamPaymentMethod> listResult = new List<BmParamPaymentMethod>();
            foreach (UNDERMO.PaymentMethodServiceModel item in listPaymentMethodServiceModel)
            {
                BmParamPaymentMethodType type = CreateParamPaymentMethodType(item.PaymentMethodTypeServiceQueryModel);
                Result<BmParamPaymentMethod, ErrorModel> paramPaymentMethodResult = BmParamPaymentMethod.GetParamPaymentMethod(item.Id, item.Description, item.TinyDescription, item.SmallDescription, type);
                BmParamPaymentMethod result = ((ResultValue<BmParamPaymentMethod, ErrorModel>)paramPaymentMethodResult).Value;
                listResult.Add(result);
            }

            return listResult;
        }
        #endregion Metodos_Pago
        #region Technical Plan
        public static Result<ParamTechnicalPlan, ErrorModel> CreateParamTechnicalPlan(TechnicalPlanServiceModel technicalPlan)
        {
            ParamCoveredRiskType paramCoveredRiskType;
            List<ParamTechnicalPlansCoverage> paramTechnicalPlanCoverages = new List<ParamTechnicalPlansCoverage>();
            Result<ParamCoveredRiskType, ErrorModel> resultCoveredRiskType;

            resultCoveredRiskType = ParamCoveredRiskType.CreateParamCoveredRiskType(technicalPlan.CoveredRiskType.Id, technicalPlan.CoveredRiskType.SmallDescription);

            if (resultCoveredRiskType is ResultError<ParamCoveredRiskType, ErrorModel>)
            {
                ResultError<ParamCoveredRiskType, ErrorModel> errorCoveredRiskType = (resultCoveredRiskType as ResultError<ParamCoveredRiskType, ErrorModel>);
                return new ResultError<ParamTechnicalPlan, ErrorModel>(ErrorModel.CreateErrorModel(errorCoveredRiskType.Message.ErrorDescription, errorCoveredRiskType.Message.ErrorType, errorCoveredRiskType.Message.Exception));
            }
            paramCoveredRiskType = (resultCoveredRiskType as ResultValue<ParamCoveredRiskType, ErrorModel>).Value;
            return ParamTechnicalPlan.CreateParamTechnicalPlan(technicalPlan.Id, technicalPlan.Description, technicalPlan.SmallDescription, paramCoveredRiskType, technicalPlan.CurrentFrom, technicalPlan.CurrentTo);
        }

        public static Result<ParamTechnicalPlanCoverage, ErrorModel> CreateParamTechnicalPlanCoverage(TechnicalPlanCoverageServiceRelationModel technicalPlanCoverage)
        {
            ParamCoverage paramCoverage = new ParamCoverage()
            {
                Id = technicalPlanCoverage.Coverage.Id,
                Description = technicalPlanCoverage.Coverage.Description
            };
            ParamInsuredObject paramInsuredObject = new ParamInsuredObject()
            {
                Id = technicalPlanCoverage.InsuredObject.Id,
                Description = technicalPlanCoverage.InsuredObject.Description
            };
            ParamCoverage paramPrincipalCoverage = null;
            decimal? paramCoveragePercentage = null;
            if (technicalPlanCoverage.PrincipalCoverage != null)
            {
                paramPrincipalCoverage = new ParamCoverage()
                {
                    Id = technicalPlanCoverage.PrincipalCoverage.Id,
                    Description = technicalPlanCoverage.PrincipalCoverage.Description
                };
                paramCoveragePercentage = technicalPlanCoverage.CoveragePercentage;
            }
            Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverages = ParamTechnicalPlanCoverage.CreateParamTechnicalPlanCoverage(paramInsuredObject, paramCoverage, paramPrincipalCoverage, paramCoveragePercentage);
            return resultCoverages;
        }

        public static Result<List<ParamTechnicalPlanCoverage>, ErrorModel> CreateParamTechnicalPlanCoverages(List<TechnicalPlanCoverageServiceRelationModel> TechnicalPlanCoverages)
        {
            List<ParamTechnicalPlanCoverage> returnList = new List<ParamTechnicalPlanCoverage>();
            Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverage;
            ResultError<ParamTechnicalPlanCoverage, ErrorModel> errorCoverage;
            foreach (TechnicalPlanCoverageServiceRelationModel coverage in TechnicalPlanCoverages)
            {
                resultCoverage = CreateParamTechnicalPlanCoverage(coverage);
                if (resultCoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                {
                    errorCoverage = (resultCoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                    return new ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorCoverage.Message.ErrorDescription, errorCoverage.Message.ErrorType, errorCoverage.Message.Exception));
                }
                returnList.Add((resultCoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value);
            }
            return new ResultValue<List<ParamTechnicalPlanCoverage>, ErrorModel>(returnList);
        }

        public static Result<ParamTechnicalPlanCoverage, ErrorModel> CreateParamTechnicalPlanCoverage(TechnicalPlanCoverageServiceRelationModel technicalPlanCoverage, AllyCoverageServiceModel TechnicalPlanAllyCoverage)
        {
            TechnicalPlanCoverageServiceRelationModel tmpAlly = new TechnicalPlanCoverageServiceRelationModel()
            {
                InsuredObject = technicalPlanCoverage.InsuredObject,
                Coverage = new CoverageServiceQueryModel()
                {
                    Id = TechnicalPlanAllyCoverage.Id,
                    Description = TechnicalPlanAllyCoverage.Description
                },
                PrincipalCoverage = technicalPlanCoverage.Coverage,
                CoveragePercentage = TechnicalPlanAllyCoverage.AlliedCoveragePercentage,
                AlliedCoverages = new List<AllyCoverageServiceModel>(),
                StatusTypeService = TechnicalPlanAllyCoverage.StatusTypeService
            };
            return CreateParamTechnicalPlanCoverage(tmpAlly);
        }

        public static Result<List<ParamTechnicalPlanCoverage>, ErrorModel> CreateParamTechnicalPlanCoverages(TechnicalPlanCoverageServiceRelationModel technicalPlanCoverage, List<AllyCoverageServiceModel> TechnicalPlanAllyCoverages)
        {
            List<ParamTechnicalPlanCoverage> returnList = new List<ParamTechnicalPlanCoverage>();
            Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverage;
            ResultError<ParamTechnicalPlanCoverage, ErrorModel> errorCoverage;
            foreach (AllyCoverageServiceModel ally in TechnicalPlanAllyCoverages)
            {
                resultCoverage = CreateParamTechnicalPlanCoverage(technicalPlanCoverage, ally);
                if (resultCoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                {
                    errorCoverage = (resultCoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                    return new ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorCoverage.Message.ErrorDescription, errorCoverage.Message.ErrorType, errorCoverage.Message.Exception));
                }
                returnList.Add((resultCoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value);
            }
            return new ResultValue<List<ParamTechnicalPlanCoverage>, ErrorModel>(returnList);
        }

        /// <summary>
        /// Convierte el listado del MOD-S del plan técnico al MOD-B
        /// </summary>
        /// <param name="technicalPlanServiceModels">Plan técnico MOD-S</param>
        /// <returns>Plan técnico MOD-B</returns>
        public static Result<List<ParamTechnicalPlanDTO>, ErrorModel> CreateParamTechnicalPlans(List<PARUPSM.TechnicalPlanServiceModel> technicalPlanServiceModels)
        {
            List<ParamTechnicalPlanDTO> paramTechnicalPlan = new List<ParamTechnicalPlanDTO>();
            Result<ParamTechnicalPlanDTO, ErrorModel> resultData;
            foreach (TechnicalPlanServiceModel item in technicalPlanServiceModels)
            {
                resultData = CreateParamTechnicalPlanDTO(item);
                if (resultData is ResultError<ParamTechnicalPlanDTO, ErrorModel>)
                {
                    ResultError<ParamTechnicalPlanDTO, ErrorModel> errorItem = (resultData as ResultError<ParamTechnicalPlanDTO, ErrorModel>);
                    return new ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>(ErrorModel.CreateErrorModel(errorItem.Message.ErrorDescription, errorItem.Message.ErrorType, errorItem.Message.Exception));
                }
                paramTechnicalPlan.Add((resultData as ResultValue<ParamTechnicalPlanDTO, ErrorModel>).Value);
            }
            return new ResultValue<List<ParamTechnicalPlanDTO>, ErrorModel>(paramTechnicalPlan);
        }

        /// <summary>
        /// Convierte el MOD-S del plan técnico al MOD-B
        /// </summary>
        /// <param name="technicalPlanServiceModel">Plan técnico MOD-S</param>
        /// <returns>Plan técnico MOD-B</returns>
        public static Result<ParamTechnicalPlanDTO, ErrorModel> CreateParamTechnicalPlanDTO(PARUPSM.TechnicalPlanServiceModel technicalPlanServiceModel)
        {
            List<ParamTechnicalPlansCoverage> TechnicalPlanCoverages = new List<ParamTechnicalPlansCoverage>();
            Result<ParamTechnicalPlan, ErrorModel> resultTechnicalPlan = CreateParamTechnicalPlan(technicalPlanServiceModel);
            if (resultTechnicalPlan is ResultError<ParamTechnicalPlan, ErrorModel>)
            {
                ResultError<ParamTechnicalPlan, ErrorModel> errorItem = (resultTechnicalPlan as ResultError<ParamTechnicalPlan, ErrorModel>);
                return new ResultError<ParamTechnicalPlanDTO, ErrorModel>(ErrorModel.CreateErrorModel(errorItem.Message.ErrorDescription, errorItem.Message.ErrorType, errorItem.Message.Exception));
            }
            ParamTechnicalPlan TechnicalPlan = (resultTechnicalPlan as ResultValue<ParamTechnicalPlan, ErrorModel>).Value;
            foreach (TechnicalPlanCoverageServiceRelationModel coverage in technicalPlanServiceModel.TechnicalPlanCoverages)
            {
                Result<ParamTechnicalPlanCoverage, ErrorModel> resultcoverage = CreateParamTechnicalPlanCoverage(coverage);
                if (resultcoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                {
                    ResultError<ParamTechnicalPlanCoverage, ErrorModel> errorcoverage = (resultcoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                    return new ResultError<ParamTechnicalPlanDTO, ErrorModel>(ErrorModel.CreateErrorModel(errorcoverage.Message.ErrorDescription, errorcoverage.Message.ErrorType, errorcoverage.Message.Exception));
                }
                ParamTechnicalPlanCoverage paramTechnicalPlanCoverage = (resultcoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value;

                List<ParamAllyCoverage> TechnicalPlanAllyCoverages = new List<ParamAllyCoverage>();
                foreach (AllyCoverageServiceModel ally in coverage.AlliedCoverages)
                {
                    Result<ParamAllyCoverage, ErrorModel> resultAlly = ParamAllyCoverage.CreateParamAllyCoverage(ally.Id, ally.Description, ally.AlliedCoveragePercentage);
                    if (resultAlly is ResultError<ParamAllyCoverage, ErrorModel>)
                    {
                        ResultError<ParamAllyCoverage, ErrorModel> errorAlly = (resultAlly as ResultError<ParamAllyCoverage, ErrorModel>);
                        return new ResultError<ParamTechnicalPlanDTO, ErrorModel>(ErrorModel.CreateErrorModel(errorAlly.Message.ErrorDescription, errorAlly.Message.ErrorType, errorAlly.Message.Exception));
                    }
                    ParamAllyCoverage paramAllyCoverage = (resultAlly as ResultValue<ParamAllyCoverage, ErrorModel>).Value;
                    //Result<ParamTechnicalPlanAllyCoverage, ErrorModel> resultParamAlly = ParamTechnicalPlanAllyCoverage.CreateParamTechnicalPlanAllyCoverage(paramAllyCoverage, ally.AlliedCoveragePercentage);
                    //if (resultParamAlly is ResultError<ParamTechnicalPlanAllyCoverage, ErrorModel>)
                    //{
                    //    ResultError<ParamTechnicalPlanAllyCoverage, ErrorModel> errorParamAllyCoverage = (resultParamAlly as ResultError<ParamTechnicalPlanAllyCoverage, ErrorModel>);
                    //    return new ResultError<ParamTechnicalPlanDTO, ErrorModel>(ErrorModel.CreateErrorModel(errorParamAllyCoverage.Message.ErrorDescription, errorParamAllyCoverage.Message.ErrorType, errorParamAllyCoverage.Message.Exception));
                    //}
                    TechnicalPlanAllyCoverages.Add(paramAllyCoverage);
                }
                Result<ParamTechnicalPlansCoverage, ErrorModel> resultParamCoverage = ParamTechnicalPlansCoverage.CreateParamTechnicalPlansCoverage(paramTechnicalPlanCoverage, TechnicalPlanAllyCoverages);
                if (resultParamCoverage is ResultError<ParamTechnicalPlansCoverage, ErrorModel>)
                {
                    ResultError<ParamTechnicalPlansCoverage, ErrorModel> errorParamCoverage = (resultParamCoverage as ResultError<ParamTechnicalPlansCoverage, ErrorModel>);
                    return new ResultError<ParamTechnicalPlanDTO, ErrorModel>(ErrorModel.CreateErrorModel(errorParamCoverage.Message.ErrorDescription, errorParamCoverage.Message.ErrorType, errorParamCoverage.Message.Exception));
                }
                TechnicalPlanCoverages.Add((resultParamCoverage as ResultValue<ParamTechnicalPlansCoverage, ErrorModel>).Value);
            }
            return ParamTechnicalPlanDTO.CreateParamTechnicalPlanDTO(TechnicalPlan, TechnicalPlanCoverages);
        }
        #endregion

    }
}
