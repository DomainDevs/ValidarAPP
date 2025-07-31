namespace Sistran.Core.Application.ProductServices.EEProvider.Assemblers
{
    using COMMEN = Common.Entities;
    using CommonModels = CommonService.Models;
    using PRODEN = Product.Entities;
    public class EntityAssembler
    {
        #region Productos      

        #region ProductCurrency
        public static PRODEN.ProductCurrency CreateProductCurrency(CommonModels.Currency currency, int productId)
        {
            return new PRODEN.ProductCurrency(productId, currency.Id);
        }
        #endregion

        #region CoProductPolicyType
        public static PRODEN.CoProductPolicyType CreateCoProductPolicyType(CommonModels.PolicyType policyType, int productId)
        {
            return new PRODEN.CoProductPolicyType(productId, policyType.Prefix.Id, policyType.Id)
            {
                IsDefault = policyType.IsDefault
            };
        }
        #endregion

        #region ProductCoverRiskType
        public static PRODEN.ProductCoverRiskType CreateProductCoverRiskType(Models.CoveredRisk coveredRisk, int productId)
        {
            return new PRODEN.ProductCoverRiskType(productId, (int)coveredRisk.CoveredRiskType)
            {
                MaxRiskQuantity = coveredRisk.MaxRiskQuantity,
                RuleSetId = coveredRisk.RuleSetId,
                PreRuleSetId = coveredRisk.PreRuleSetId,
                ScriptId = coveredRisk.ScriptId
            };
        }
        #endregion



        #region ProductAgentCommis
        public static PRODEN.ProductAgencyCommiss CreateProductAgentCommission(Models.ProductAgencyCommiss productAgencyCommiss)
        {
            return new PRODEN.ProductAgencyCommiss(productAgencyCommiss.IndividualId, productAgencyCommiss.AgencyId, productAgencyCommiss.ProductId)
            {
                AdditCommissPercentage = productAgencyCommiss.AdditionalCommissionPercentage,
                StCommissPercentage = productAgencyCommiss.CommissPercentage
            };
        }
        #endregion
        #region Deducibles

        public static COMMEN.DeductibleProduct CreateDeductibleProduct(Models.DeductibleProduct modelo, int productId)
        {
            return new COMMEN.DeductibleProduct(modelo.DeductId, productId);
        }
        #endregion

        public static PRODEN.Product CreateProduct(Models.Product product)
        {
            return new PRODEN.Product
            {
                ProductId = product.Id,
                ScriptId = product.ScriptId,
                PreRuleSetId = product.PreRuleSetId,
                RuleSetId = product.RuleSetId,
                StandardCommissionPercentage = product.StandardCommissionPercentage,
                IsFlatRate = product.IsFlatRate,
                IsCollective = product.IsCollective,
                IsGreen = product.IsGreen
            };
        }
        #region ProductAgent
        public static PRODEN.ProductAgent CreateProductAgent(Models.ProductAgent productAgent)
        {
            return new PRODEN.ProductAgent(productAgent.ProductId, productAgent.IndividualId);
        }
        #endregion
        #endregion productos


    }
}
