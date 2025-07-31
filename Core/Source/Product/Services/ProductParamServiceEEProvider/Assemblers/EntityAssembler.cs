namespace Sistran.Core.Application.ProductParamService.EEProvider.Assemblers
{
   
    /// <summary>
    /// Convierte el modelo del servicio al  modelo de la entidad 
    /// </summary>
    public static class EntityAssembler
    {
        #region Technical Plan
        ///// <summary>
        ///// Construye entidad de Plan Técnico
        ///// </summary>
        ///// <param name="paramTechnicalPlan">Plan Técnico</param>
        ///// <returns>Plan Técnico - ENTIDAD</returns>
        //public static virtual PRODEN.TechnicalPlan CreateTechnicalPlan(ParamTechnicalPlan paramTechnicalPlan)
        //{
        //    return new PRODEN.TechnicalPlan()
        //    {
        //        Description = paramTechnicalPlan.Description,
        //        SmallDescription = paramTechnicalPlan.SmallDescription,
        //        CoveredRiskTypeCode = paramTechnicalPlan.CoveredRiskType.Id,
        //        CurrentFrom = paramTechnicalPlan.CurrentFrom,
        //        CurrentTo = paramTechnicalPlan.CurrentTo
        //    };
        //}

        ///// <summary>
        ///// Construye entidad Cobertura des Plan Técnico
        ///// </summary>
        ///// <param name="paramTechnicalPlan">Cobertura de Plan Técnico</param>
        ///// <returns>Cobertura de Plan Técnico - ENTIDAD</returns>
        //public static PRODEN.TechnicalPlanCoverage CreateTechnicalPlanCoverage(int technicalPlanId, ParamTechnicalPlanCoverage paramTechnicalPlanCoverage)
        //{
        //    PRODEN.TechnicalPlanCoverage dataEntity = new PRODEN.TechnicalPlanCoverage(technicalPlanId, paramTechnicalPlanCoverage.Coverage.Id);
        //    dataEntity.IsSublimit = false;
        //    dataEntity.MainCoveragePercentage = null;
        //    dataEntity.MainCoverageId = null;
        //    if (paramTechnicalPlanCoverage.PrincipalCoverage != null)
        //    {
        //        dataEntity.MainCoveragePercentage = paramTechnicalPlanCoverage.CoveragePercentage < 0 ? 0 : paramTechnicalPlanCoverage.CoveragePercentage;
        //        if (paramTechnicalPlanCoverage.PrincipalCoverage.Id > 0)
        //        {
        //            dataEntity.MainCoverageId = paramTechnicalPlanCoverage.PrincipalCoverage.Id;
        //        }
        //    }
        //    return dataEntity;
        //}
        #endregion

        #region ProductFinancialPlan
        //public static PRODEN.ProductFinancialPlan CreateProductFinancialPlan(Models.ProductFinancialPlan productFinancialPlan)
        //{
        //    return new PRODEN.ProductFinancialPlan(productFinancialPlan.ProductId, productFinancialPlan.Id)
        //    {
        //        IsDefault = productFinancialPlan.IsDefault
        //    };
        //}
        //public static PRODEN.ProductFinancialPlan CreateProductFinancialPlan(Models.FinancialPlan productFinancialPlan, int productId)
        //{
        //    return new PRODEN.ProductFinancialPlan(productId, productFinancialPlan.Id)
        //    {
        //        IsDefault = productFinancialPlan.IsDefault
        //    };
        //}

        #endregion
        #region ProductForm
        //public static PRODEN.ProductForm CreateProductForm(Models.ProductForm productForm)
        //{
        //    return new PRODEN.ProductForm
        //    {
        //        CurrentFrom = productForm.CurrentFrom,
        //        FormNumber = productForm.FormNumber,
        //        ProductId = productForm.Product.Id,
        //        CoverGroupId = productForm.GroupCoverage.Id
        //    };
        //}
        #endregion

        #region Activities
        //public static COMMEN.ProductRiskCommercialClass CreateProductCommercialClass(Models.ProductRiskCommercialClass productRiskCommercialClass, int ProductId)
        //{
        //    return new COMMEN.ProductRiskCommercialClass(ProductId, productRiskCommercialClass.RiskCommercialClass.RiskCommercialClassCode)
        //    {
        //        DefaultRiskCommercial = productRiskCommercialClass.DefaultRiskCommercial
        //    };
        //}
        #endregion
    }
}
