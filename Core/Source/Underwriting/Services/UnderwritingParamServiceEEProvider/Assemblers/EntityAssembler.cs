// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved
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
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Integration.Entities;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using ENUMUN = UnderwritingServices.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using PAYM = Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Utilities.Error;

    using ENUM = Sistran.Core.Application.UnderwritingParamService.Enums;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Extensions;

    /// <summary>
    /// Convierte el modelo del servicio al  modelo de la entidad 
    /// </summary>
    public static class EntityAssembler
    {
        #region PaymentSchedule

        /// <summary>
        /// Construye la entidad de plan de pago (PaymentSchedule)
        /// </summary>
        /// <param name="parametrizationPaymentPlan">Plan de pago MOD-B</param>
        /// <returns>Plan de pago - ENTIDAD</returns>
        public static PRODEN.PaymentSchedule CreatePaymentSchedule(ParametrizationPaymentPlan parametrizationPaymentPlan)
        {
            return new PRODEN.PaymentSchedule()
            {
                Description = parametrizationPaymentPlan.Description,
                SmallDescription = parametrizationPaymentPlan.SmallDescription,
                IsGreaterDate = parametrizationPaymentPlan.IsGreaterDate,
                IsIssueDate = parametrizationPaymentPlan.IsIssueDate,
                FirstPayQuantity = parametrizationPaymentPlan.FirstPayQuantity,
                PaymentQuantity = parametrizationPaymentPlan.Quantity,
                GapUnitCode = parametrizationPaymentPlan.GapUnit,
                GapQuantity = parametrizationPaymentPlan.GapQuantity,
                LastPayQuantity = parametrizationPaymentPlan.LastPayQuantity,
                Financing = parametrizationPaymentPlan.Financing
            };
        }

        #endregion PaymentSchedule

        #region PaymentDistribution
        /// <summary>
        /// Construye la entidad de la distribuccion de la cuotas (PaymentDistribution)
        /// </summary>
        /// <param name="quota">Cuota MOD-B</param>
        /// <returns>Entidad Cuota</returns>
        public static PRODEN.PaymentDistribution CreatePaymentDistribution(ParametrizationQuota quota)
        {
            return new PRODEN.PaymentDistribution(quota.Id, quota.Number)
            {
                PaymentPercentage = quota.Percentage,
                GapQuantity = quota.GapQuantity
            };
        }

        public static PRODEN.CoPaymentDistributionComponent CreatePaymentDistributionComponent(ParametrizacionQuotaTypeComponent quota)
        {
            return new PRODEN.CoPaymentDistributionComponent(quota.PaymentScheduleId, quota.PaymentNumber, quota.Id)
            {
                Value = quota.Value
            };
        }


        #endregion PaymentDistribution

        #region CoPaymentDistributionComponent
        /// <summary>
        /// Construye la entidad de la distribuccion de la cuotas (PaymentDistribution)
        /// </summary>
        /// <param name="quota">Cuota MOD-B</param>
        /// <returns>Entidad Cuota</returns>
        public static PRODEN.CoPaymentDistributionComponent CreateCoPaymentDistributionComponent(ParametrizacionQuotaTypeComponent quota)
        {
            return new PRODEN.CoPaymentDistributionComponent(quota.PaymentScheduleId, quota.PaymentNumber, quota.Id)
            {
                Value = quota.Value
            };
        }
        #endregion



        #region Recargos
        /// <summary>
        /// Construye la entidad de recargos
        /// </summary>
        /// <param name="item">modelo racargos MOD-B</param>
        /// <param name="surchargeType">tipo de componente para recargos</param>
        /// <param name="surchargeClass">tipo de clase para recargos</param>
        /// <returns>surcharge - ENTIDAD</returns>
        public static QUOEN.Component CreateSurcharge(ParamSurcharge item, ENUMUN.ComponentType surchargeType, ENUMUN.ComponentClassType surchargeClass)
        {
            return new QUOEN.Component(item.Id)
            {
                ComponentCode = item.Id,
                SmallDescription = item.Description,
                TinyDescription = item.TinyDescription,
                ComponentTypeCode = (int)surchargeType,
                ComponentClassCode = (int)surchargeClass
            };
        }

        #endregion

        /// <summary>
        /// Construye la entidad de recargos
        /// </summary>
        /// <param name="item">recargos MOD-B</param>
        /// <returns>surcharge - ENTIDAD</returns>
        public static QUOEN.SurchargeComponent CreateSurchargeComponent(ParamSurcharge item)
        {
            return new QUOEN.SurchargeComponent(item.Id)
            {
                ComponentCode = item.Id,
                RateTypeCode = (int)item.Type,
                Rate = item.Rate
            };
        }

        #region Descuentos
        /// <summary>
        /// Construye la entidad de descuentos
        /// </summary>
        /// <param name="item">Descuento MOD-B</param>
        /// <returns>descuentos - ENTIDAD</returns>
        public static QUOEN.DiscountComponent CreateDiscountComponent(ParamDiscount item)
        {
            return new QUOEN.DiscountComponent(item.Id)
            {
                ComponentCode = item.Id,
                RateTypeCode = (int)item.Type,
                Rate = item.Rate
            };
        }
        #endregion

        #region Component

        /// <summary>
        /// Construye la entidad de descuentos
        /// </summary>
        /// <param name="item" >descuentos MOD-B</param>
        /// <param name="discountType">tipo de componente</param>
        /// <param name="discountClass">tipo de clase de componnete</param> 
        /// <returns>ENTIDAD de componente</returns>
        public static QUOEN.Component CreateComponent(ParamDiscount item, ENUMUN.ComponentType discountType, ENUMUN.ComponentClassType discountClass)
        {
            return new QUOEN.Component(item.Id)
            {
                ComponentCode = item.Id,
                SmallDescription = item.Description,
                TinyDescription = item.TinyDescription,
                ComponentTypeCode = (int)discountType,
                ComponentClassCode = (int)discountClass
            };
        }
        #endregion

        #region Carrocería de vehículo
        /// <summary>
        /// Crea la entidad a partir del modelo 
        /// </summary>
        /// <param name="vehicleBody">Carrocería de vehículo</param>
        /// <returns>Entidad de Carrocería de vehículo</returns>
        public static COMMEN.VehicleBody CreateVehicleBody(ParamVehicleBody vehicleBody)
        {
            return new COMMEN.VehicleBody(vehicleBody.Id)
            {
                SmallDescription = vehicleBody.Description,
            };
        }

        /// <summary>
        /// Crea la entidad a partir del modelo
        /// </summary>
        /// <param name="vehicleTypeCode">Id de Carrocería de vehículo</param>
        /// <param name="vehicleUses">Listado de Usos/param>
        /// <returns>Listado de Usos/returns>
        public static List<COMMEN.VehicleBodyUse> CreateVehicleBodyUse(int vehicleTypeCode, List<ParamVehicleUse> vehicleUses)
        {
            List<COMMEN.VehicleBodyUse> vehicleBodyUses = new List<COMMEN.VehicleBodyUse>();
            if (vehicleUses != null)
            {
                foreach (ParamVehicleUse item in vehicleUses)
                {
                    vehicleBodyUses.Add(new COMMEN.VehicleBodyUse(vehicleTypeCode, item.Id));
                }
            }
            return vehicleBodyUses;
        }
        #endregion Carrocería de vehículo

        #region Vehicle Type
        /// <summary>
        /// Crea la entidad a partir del modelo 
        /// </summary>
        /// <param name="vehicleType">tipo de vehiculo</param>
        /// <returns>Entidad de tipo de vehiculo</returns>
        public static COMMEN.VehicleType CreateVehicleType(ParamVehicleType vehicleType)
        {
            return new COMMEN.VehicleType(vehicleType.Id)
            {
                Description = vehicleType.Description,
                SmallDescription = vehicleType.SmallDescription,
                Enabled = vehicleType.IsEnable,
                IsTruck = vehicleType.IsTruck,
                ExtendedProperties = CreateExtendedProperties(vehicleType.ExtendedProperties)
            };
        }

        public static List<Framework.DAF.ExtendedProperty> CreateExtendedProperties(List<ExtendedProperty> extendedProperties)
        {
            List<Framework.DAF.ExtendedProperty> entityExtendedProperties = new List<Framework.DAF.ExtendedProperty>();

            if (extendedProperties != null)
            {
                foreach (ExtendedProperty extendedProperty in extendedProperties)
                {
                    entityExtendedProperties.Add(new Framework.DAF.ExtendedProperty
                    {
                        Name = extendedProperty.Name,
                        Value = extendedProperty.Value
                    });
                }
            }

            return entityExtendedProperties;
        }

        /// <summary>
        /// Crea la entidad a partir del modelo
        /// </summary>
        /// <param name="vehicleTypeCode">Id del tipo de vehiculo</param>
        /// <param name="vehicleBodies">Listado de tipo de carrocerias/param>
        /// <returns>Listado de entidad de carrocerias/returns>
        public static List<COMMEN.VehicleTypeBody> CreateVehicleTypeBody(int vehicleTypeCode, List<ParamVehicleBody> vehicleBodies)
        {
            List<COMMEN.VehicleTypeBody> vehicleTypeBodies = new List<COMMEN.VehicleTypeBody>();
            if (vehicleBodies != null)
            {
                foreach (ParamVehicleBody item in vehicleBodies)
                {
                    vehicleTypeBodies.Add(new COMMEN.VehicleTypeBody(vehicleTypeCode, item.Id));
                }
            }
            return vehicleTypeBodies;
        }

        #endregion Vehicle Type


        #region Coverage
        /// <summary>
        /// Construye entidad de cobertura
        /// </summary>
        /// <param name="paramCoverage">Cobertura MOD-B</param>
        /// <returns>Cobertura - ENTIDAD</returns>
        public static QUOEN.Coverage CreateCoverage(ParamCoverage paramCoverage)
        {
            return new QUOEN.Coverage()
            {
                PrintDescription = paramCoverage.Description,
                LineBusinessCode = paramCoverage.LineBusiness.Id,
                SubLineBusinessCode = paramCoverage.SubLineBusiness.Id,
                PerilCode = paramCoverage.Peril.Id,
                InsuredObjectId = paramCoverage.InsuredObjectDesc.Id,
                IsPrimary = paramCoverage.IsPrincipal,
                CompositionTypeCode = paramCoverage.CompositionTypeId
            };
        }
        #endregion

        #region CoCoverage
        /// <summary>
        /// Construye la entidad CoCoverage 
        /// </summary>
        /// <param name="paramCoCoverage">CoCoverage MOD-B</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>CoCoverage - ENTIDAD</returns>
        public static QUOEN.CoCoverage CreateCoCoverage(ParamCoCoverage paramCoCoverage, int coverageId)
        {
            return new QUOEN.CoCoverage(coverageId, paramCoCoverage.Id)
            {
                IsImpression = paramCoCoverage.IsImpression,
                PrintDescription = paramCoCoverage.Description,
                PrintDescriptionLimit = paramCoCoverage.ImpressionValue,
                IsAccMinPremium = paramCoCoverage.IsAccMinPremium,
                IsAssistance = paramCoCoverage.IsAssistance,
                IsChild = paramCoCoverage.IsChild,
                IsSeriousOffer = paramCoCoverage.IsSeriousOffer
            };
        }
        #endregion

        #region Clause
        /// <summary>
        /// Metodo para crear clausulas
        /// </summary>
        /// <param name="parametrizationClause">Dastos clausula</param>
        /// <returns>Retorna clausulas</returns>
        public static QUOEN.Clause CreateClause(ParamClause parametrizationClause)
        {
            QUOEN.Clause clause = new QUOEN.Clause()
            {

                ClauseName = parametrizationClause.Clause.Name,
                ClauseTitle = parametrizationClause.Clause.Title,
                ClauseText = parametrizationClause.Clause.Text,
                CurrentFrom = parametrizationClause.InputStartDate,
                CurrentTo = parametrizationClause.DueDate,
                ConditionLevelCode = parametrizationClause.Clause.ConditionLevel.Id
            };
            if (parametrizationClause.Clause.Id > 0)
            {
                clause.ClauseId = parametrizationClause.Clause.Id;
            }
            return clause;
        }
        #endregion Clause

        #region ClauseLevel
        /// <summary>
        /// Construye la en entidad ClauseLevel (relacion de la cobertura con la clausula)
        /// </summary>
        /// <param name="paramClauseDesc">Clausula MOD-B</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>Clausulas relacion con Level - ENTIDAD</returns>
        public static QUOEN.ClauseLevel CreateClauseLevel(ParamClauseDesc paramClauseDesc, int coverageId)
        {
            return new QUOEN.ClauseLevel()
            {
                ConditionLevelId = coverageId,
                ClauseId = paramClauseDesc.Id,
                IsMandatory = paramClauseDesc.IsMandatory
            };
        }
        #endregion 

        #region CoverageDeductible
        /// <summary>
        /// Construye la entidad CoverageDeductible (relacion de cobertura con los deducibles)
        /// </summary>
        /// <param name="paramDeductibleDesc">Deducibles MOD.B</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>Cobertura-Deducible relacion ENTIDAD</returns>
        public static QUOEN.CoverageDeductible CreateCoverageDeductible(ParamDeductibleDesc paramDeductibleDesc, int coverageId)
        {
            return new QUOEN.CoverageDeductible(paramDeductibleDesc.Id, coverageId)
            {
                IsDefault = paramDeductibleDesc.IsMandatory
            };
        }
        #endregion 

        #region CoverDetailType
        /// <summary>
        /// Construye la entidad CoverDetailType(relacion de cobertura con los tipos de detalle)
        /// </summary>
        /// <param name="paramDetailTypeDesc">Tipos de detalle MOD.B</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>Cobertura-Tipo de detalle ENTIDAD</returns>
        public static QUOEN.CoverDetailType CreateCoverDetailType(ParamDetailTypeDesc paramDetailTypeDesc, int coverageId)
        {
            return new QUOEN.CoverDetailType(coverageId, paramDetailTypeDesc.Id)
            {
                IsMandatory = paramDetailTypeDesc.IsMandatory
            };
        }
        #endregion

        #region ClauseLevelParam
        /// <summary>
        /// Metodo para crear clausula por nivel
        /// </summary>
        /// <param name="clauseLevel">clausulas por nivel</param>
        /// <returns>retorna clausulas por nivel</returns>
        public static QUOEN.ClauseLevel CreateClauseLevelParam(ParamClauseLevel clauseLevel)
        {
            return new QUOEN.ClauseLevel()
            {
                ClauseId = clauseLevel.ClauseId,
                ConditionLevelId = clauseLevel.ConditionLevelId,
                IsMandatory = clauseLevel.IsMandatory
            };
        }
        #endregion ClauseLevelParam

        #region Business Configuration
        /// <summary>
        /// Mapeo de la modelo Business a la entidad Business.
        /// </summary>
        /// <param name="business">Modelo Business</param>
        /// <returns>Entidad Business</returns>
        public static COMMEN.Business CreateBusiness(ParamBusiness business)
        {
            COMMEN.Business result = new COMMEN.Business(business.BusinessId) { BusinessId = business.BusinessId, Description = business.Description, IsEnabled = business.IsEnabled, PrefixCode = business.Prefix.PrefixCode };
            return result;
        }

        /// <summary>
        /// Mapeo de la modelo BusinessConfiguration a la entidad BusinessConfiguration.
        /// </summary>
        /// <param name="business">Modelo BusinessConfiguration</param>
        /// <returns>Entidad BusinessConfiguration</returns>
        public static QUOEN.BusinessConfiguration CreateBusinessConfiguration(ParamBusinessConfiguration businessConfiguration)
        {
            QUOEN.BusinessConfiguration result = new QUOEN.BusinessConfiguration();
            if (businessConfiguration.Request != null)
            {
                result = new QUOEN.BusinessConfiguration(businessConfiguration.BusinessConfigurationId) { BusinessConfigurationId = businessConfiguration.BusinessConfigurationId, BusinessId = businessConfiguration.BusinessId, RequestId = businessConfiguration.Request.RequestId, ProductId = businessConfiguration.Product.ProductId, GroupCoverageId = businessConfiguration.GroupCoverage.GroupCoverageId, AssistanceCode = businessConfiguration.Assistance.AssistanceCode, ProductIdResponse = businessConfiguration.ProductIdResponse };
            }
            else
            {
                result = new QUOEN.BusinessConfiguration(businessConfiguration.BusinessConfigurationId) { BusinessConfigurationId = businessConfiguration.BusinessConfigurationId, BusinessId = businessConfiguration.BusinessId, RequestId = null, ProductId = businessConfiguration.Product.ProductId, GroupCoverageId = businessConfiguration.GroupCoverage.GroupCoverageId, AssistanceCode = businessConfiguration.Assistance.AssistanceCode, ProductIdResponse = businessConfiguration.ProductIdResponse };
            }
            return result;
        }
        #endregion

        #region Metodos_Pago

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static PAYM.PaymentMethodType CreatePaymentMethodType(BmParamPaymentMethodType item)
        {
            return new PAYM.PaymentMethodType(item.Id)
            {
                PaymentMethodTypeCode = item.Id,
                Description = item.Description
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static PAYM.PaymentMethod CreatePaymentMethod(BmParamPaymentMethod item)
        {
            return new PAYM.PaymentMethod(item.Id)
            {
                PaymentMethodCode = item.Id,
                Description = item.Description,
                TinyDescription = item.TinyDescription,
                SmallDescription = item.SmallDescription,
                PaymentMethodTypeCode = item.PaymentMethod.Id
            };
        }


        #endregion Metodos_Pago


        #region LineBusiness
        /// <summary>
        /// Convierte del modelo de negocio a la entidad (LineBusiness)
        /// </summary>
        /// <param name="lineBusiness">modelo de negocio LineBusiness</param>
        /// <returns>Entidad LineBusiness</returns>
        public static COMMEN.LineBusiness CreateLineBusiness(ParamLineBusinessModel lineBusiness)
        {
            return new COMMEN.LineBusiness(lineBusiness.Id)
            {
                Description = lineBusiness.Description,
                SmallDescription = lineBusiness.SmallDescription,
                TinyDescription = lineBusiness.TinyDescription,
                ReportLineBusinessCode = lineBusiness.Id.ToString()
            };
        }
        #endregion

        #region RatingZone
        /// <summary>
        /// Construye la entidad de zona de tarifacion(RatingZone)
        /// </summary>
        /// <param name="paramRatingZone">zona de tarifacion MOD-B</param>
        /// <returns>zona de tarifacion - ENTIDAD</returns>
        public static COMMEN.RatingZone CreateRatingZone(ParamRatingZone paramRatingZone)
        {
            return new COMMEN.RatingZone(paramRatingZone.Id)
            {
                Description = paramRatingZone.Description,
                SmallDescription = paramRatingZone.SmallDescription,
                PrefixCode = paramRatingZone.Prefix.Id,
                IsDefault = paramRatingZone.IsDefault
            };
        }

        /// <summary>
        /// Construye la entidad de ciudades
        /// </summary>
        /// <param name="cities">lista de ciudades MOD-B</param>
        /// <returns>lista de ciudades - ENTIDAD</returns>
        public static List<COMMEN.City> CreateCities(List<City> cities)
        {
            return cities.Select(x => new COMMEN.City(x.State.Country.Id, x.Id, x.State.Id)).ToList();
        }
        #endregion

        #region Homologacion 2G

        public static CiaEquivalenceCoverage CreateCiaEquivalenceCoverage(ParamCoverage paramCoverage, int coverageId3g)
        {
            return new CiaEquivalenceCoverage()
            {
                CoverageId2g = paramCoverage.Homologation2G.Id,
                CoverageId3g = coverageId3g,
                InsuredObject2g = paramCoverage.Homologation2G.InsuredObjectId,
                LineBusinessCode = paramCoverage.Homologation2G.LineBusinessId,
                SubLineBusinessCode = paramCoverage.Homologation2G.SubLineBusinessId
            };
        }


        #endregion

        #region FinancialPlan
        /// <summary>
        /// Metodo para crear clausulas
        /// </summary>
        /// <param name="parametrizationFinancialPlan">Dastos clausula</param>
        /// <returns>Retorna clausulas</returns>
        public static PRODEN.FinancialPlan CreateFinancialPlan(ParamFinancialPlanComponent parametrizationFinancialPlan)
        {
            return new PRODEN.FinancialPlan()
            {
                PaymentMethodCode = parametrizationFinancialPlan.ParamFinancialPlan.ParamPaymentMethod.Id,
                CurrencyCode = parametrizationFinancialPlan.ParamFinancialPlan.ParamCurrency.Id,
                PaymentScheduleId = parametrizationFinancialPlan.ParamFinancialPlan.ParametrizationPaymentPlan.Id,
                MinPayAmount = parametrizationFinancialPlan.ParamFinancialPlan.MinQuota
            };
        }

        /// <summary>
        /// Metodo para crear clausula por nivel
        /// </summary>
        /// <param name="firtsPayComponent">clausulas por nivel</param>
        /// <returns>retorna clausulas por nivel</returns>
        public static PRODEN.FirstPayComponent CreateFirtsPayComponent(ParamFirstPayComponent firtsPayComponent, int financialPlanId)
        {
            return new PRODEN.FirstPayComponent(firtsPayComponent.IdComponent, financialPlanId)
            {
                FinancialPlanId = financialPlanId,
                ComponentCode = firtsPayComponent.IdComponent
            };
        }
        #endregion

        #region Limit Rc

        /// <summary>
        /// Construye la entidad de limite Rc
        /// </summary>
        /// <param name="paramLimitRc">Objeto ParamLimitRc</param>
        /// <returns>Retorna entidad CoLimitsRc</returns>
        public static COMMEN.CoLimitsRc CreateLimitRc(ParamLimitRc paramLimitRc)
        {
            return new COMMEN.CoLimitsRc(paramLimitRc.Id)
            {
                LimitRcCode = paramLimitRc.Id,
                Limit1 = paramLimitRc.Limit1,
                Limit2 = paramLimitRc.Limit2,
                Limit3 = paramLimitRc.Limit3,
                LimitUnique = Convert.ToDecimal(paramLimitRc.LimitUnique),
                Description = paramLimitRc.Description
            };
        }

        #endregion

        #region Component expense        

        /// <summary>
        /// Crea la entidad a partir del modelo 
        /// </summary>
        /// <param name="componentExpense">tipo de vehiculo</param>
        /// <returns>Entidad de tipo de vehiculo</returns>
        public static QUOEN.Component CreateComponentExpense(ParamExpense componentExpense)
        {
            return new QUOEN.Component(componentExpense.Id)
            {

                SmallDescription = componentExpense.SmallDescription,
                TinyDescription = componentExpense.TinyDescripcion,
                ComponentClassCode = (int)ENUM.ComponentClass.EXPENSES,
                ComponentTypeCode = (int)ENUMUN.ComponentClassType.Expenses
            };
        }
        /// <summary>
        /// Crea la entidad a partir del modelo 
        /// </summary>
        /// <param name="componentExpense">tipo de vehiculo</param>
        /// <returns>Entidad de tipo de vehiculo</returns>
        public static QUOEN.ExpenseComponent CreateExpense(ParamExpense componentExpense)
        {
            var result = new QUOEN.ExpenseComponent(componentExpense.Id);

            result.Rate = componentExpense.Rate;
            result.IsMandatory = componentExpense.IsMandatory;
            result.IsInitially = componentExpense.IsInitially;
            result.RuleSetId = componentExpense.ParamRuleSet.Id;
            result.RateTypeCode = componentExpense.ParamRateType.Id;
            return result;

        }
        #endregion


        #region Technical Plan
        /// <summary>
        /// Construye entidad de Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlan">Plan Técnico</param>
        /// <returns>Plan Técnico - ENTIDAD</returns>
        public static PRODEN.TechnicalPlan CreateTechnicalPlan(ParamTechnicalPlan paramTechnicalPlan)
        {
            return new PRODEN.TechnicalPlan()
            {
                Description = paramTechnicalPlan.Description,
                SmallDescription = paramTechnicalPlan.SmallDescription,
                CoveredRiskTypeCode = paramTechnicalPlan.CoveredRiskType.Id,
                CurrentFrom = paramTechnicalPlan.CurrentFrom,
                CurrentTo = paramTechnicalPlan.CurrentTo
            };
        }

        /// <summary>
        /// Construye entidad Cobertura des Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlan">Cobertura de Plan Técnico</param>
        /// <returns>Cobertura de Plan Técnico - ENTIDAD</returns>
        public static PRODEN.TechnicalPlanCoverage CreateTechnicalPlanCoverage(int technicalPlanId, ParamTechnicalPlanCoverage paramTechnicalPlanCoverage)
        {
            PRODEN.TechnicalPlanCoverage dataEntity = new PRODEN.TechnicalPlanCoverage(technicalPlanId, paramTechnicalPlanCoverage.Coverage.Id);
            dataEntity.IsSublimit = false;
            dataEntity.MainCoveragePercentage = null;
            dataEntity.MainCoverageId = null;
            if (paramTechnicalPlanCoverage.PrincipalCoverage != null)
            {
                dataEntity.MainCoveragePercentage = paramTechnicalPlanCoverage.CoveragePercentage < 0 ? 0 : paramTechnicalPlanCoverage.CoveragePercentage;
                if (paramTechnicalPlanCoverage.PrincipalCoverage.Id > 0)
                {
                    dataEntity.MainCoverageId = paramTechnicalPlanCoverage.PrincipalCoverage.Id;
                }
            }
            return dataEntity;
        }
        #endregion

        #region AllyCoverage
        public static QUOEN.AllyCoverage CreateParamAllyCoverage(ParamQueryCoverage paramQueryAllyCoverage)
        {
            return new QUOEN.AllyCoverage(paramQueryAllyCoverage.AllyCoverage.Id, paramQueryAllyCoverage.CoveragePrincipal.Id)
            {
                AllyCoverageId = paramQueryAllyCoverage.AllyCoverage.Id,
                CoverageId = paramQueryAllyCoverage.CoveragePrincipal.Id,
                CoveragePercentage = paramQueryAllyCoverage.CoveragePercentage
            };
        }
        #endregion
    }
}
