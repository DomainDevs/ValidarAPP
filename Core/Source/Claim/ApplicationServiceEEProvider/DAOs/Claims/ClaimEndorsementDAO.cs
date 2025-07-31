using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Framework.DAF;
using System.Data;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimEndorsementDAO
    {
        public List<ClaimEndorsement> GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(int? prefixId, int? branchId, CoveredRiskType coveredRiskTypeId, decimal documentNumber, DateTime claimDate)
        {
            int policyId = ValidatePolicyCanceledByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(prefixId, branchId, coveredRiskTypeId, documentNumber, claimDate);
            List<int> voidEndorsements = new List<int>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name, policyId);
            filter.And();
            filter.OpenParenthesis();
            filter.Property(ISSEN.RiskCoverage.Properties.EndorsementSublimitAmount);
            filter.Distinct();
            filter.Constant(0);
            filter.Or();
            filter.Property(ISSEN.RiskCoverage.Properties.PremiumAmount);
            filter.Distinct();
            filter.Constant(0);
            filter.CloseParenthesis();

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.GroupEndorsement.Properties.EndorsementId, typeof(ISSEN.GroupEndorsement).Name), "EndorsementId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.GroupEndorsement.Properties.RefEndorsementId, typeof(ISSEN.GroupEndorsement).Name), "RefEndorsementId"));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.GroupEndorsement), typeof(ISSEN.GroupEndorsement).Name);
            selectQuery.Where = new ObjectCriteriaBuilder()
                .PropertyEquals(ISSEN.GroupEndorsement.Properties.PolicyId, typeof(ISSEN.GroupEndorsement).Name, policyId)
                .And()
                .Property(ISSEN.GroupEndorsement.Properties.RefEndorsementId, typeof(ISSEN.GroupEndorsement).Name)
                .IsNotNull()
                .GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    voidEndorsements.Add(Convert.ToInt32(reader["EndorsementId"]));
                    voidEndorsements.Add(Convert.ToInt32(reader["RefEndorsementId"]));
                }
            }

            if (voidEndorsements.Any())
            {
                filter.And();
                filter.Not();
                filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
                filter.In();
                filter.ListValue();

                voidEndorsements.ForEach(x =>
                {
                    filter.Constant(x);
                });

                filter.EndList();
            }


            PolicyEndorsementView policyEndorsementView = new PolicyEndorsementView();
            ViewBuilder viewBuilder = new ViewBuilder("PolicyEndorsementView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, policyEndorsementView);

            if (policyEndorsementView.Policies.Count > 0 && policyEndorsementView.Endorsements.Count > 0)
            {
                List<ISSEN.Endorsement> entityEndorsements = policyEndorsementView.Endorsements.Cast<ISSEN.Endorsement>().Where(x => { return x.CurrentFrom <= claimDate && x.CurrentTo >= claimDate; }).ToList();


                if (!entityEndorsements.Any())
                    throw new BusinessException(Resources.Resources.ErrorPolicyNotValidToOccurrenceDate);

                List<int> endorsementTypesClaimEnabled = GetEndorsementTypesClaimEnabled();
                ISSEN.Endorsement entityEndorsement = entityEndorsements.OrderBy(x => x.DocumentNum).LastOrDefault();
                ISSEN.EndorsementRiskCoverage entityEndorsementRiskCoverage = policyEndorsementView.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().Where(z => z.EndorsementId == entityEndorsement.EndorsementId).FirstOrDefault();

                if (policyEndorsementView.RiskCoverages.Cast<ISSEN.RiskCoverage>().Where(z => z.RiskCoverId == entityEndorsementRiskCoverage.RiskCoverId).FirstOrDefault().SublimitAmount == 0)
                {
                    throw new BusinessException(string.Format(Resources.Resources.EndorsementNotClaims, entityEndorsement.DocumentNum, documentNumber));
                }

                if (!endorsementTypesClaimEnabled.Exists(x => x == entityEndorsement.EndoTypeCode))
                    throw new BusinessException(string.Format(Resources.Resources.ErrorEndorsementNotEnabledToClaim, policyEndorsementView.EndorsementTypes.Cast<PARAMEN.EndorsementType>().FirstOrDefault(x => x.EndoTypeCode == entityEndorsement.EndoTypeCode).Description));

                return ModelAssembler.CreateClaimEndorsements(entityEndorsements.OrderBy(x => x.DocumentNum).ToList());
            }

            throw new BusinessException(Resources.Resources.ErrorPolicyDontExist);
        }

        public int ValidatePolicyCanceledByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(int? prefixId, int? branchId, CoveredRiskType coveredRiskTypeId, decimal documentNumber, DateTime claimDate)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (prefixId != null)
            {
                filter.PropertyEquals(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name, prefixId);
                filter.And();
            }

            if (branchId != null)
            {
                filter.PropertyEquals(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name, branchId);
                filter.And();
            }            
            
            filter.PropertyEquals(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name, documentNumber);

            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.CurrentFrom, typeof(ISSEN.Endorsement).Name);
            filter.LessEqual();
            filter.Constant(claimDate);
            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.CurrentTo, typeof(ISSEN.Endorsement).Name);
            filter.GreaterEqual();
            filter.Constant(claimDate);

            PolicyEndorsementView policyEndorsementView = new PolicyEndorsementView();
            ViewBuilder viewBuilder = new ViewBuilder("PolicyEndorsementView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, policyEndorsementView);

            if (policyEndorsementView.Policies.Any() && policyEndorsementView.Endorsements.Any())
            {
                if (prefixId == null && !DelegateService.commonServiceCore.GetPrefixesByCoveredRiskType(coveredRiskTypeId).Any(x => { return policyEndorsementView.Policies.Cast<ISSEN.Policy>().Any(y => y.PrefixCode == x.Id); }))
                    throw new BusinessException(Resources.Resources.ErrorPolicyDontExist);

                List<ISSEN.Endorsement> entityEndorsements = policyEndorsementView.Endorsements.Cast<ISSEN.Endorsement>().ToList();

                if (entityEndorsements.OrderBy(x => x.DocumentNum).Last().EndoTypeCode == 3)
                    throw new BusinessException(Resources.Resources.ErrorCanceledPolicy);

                return entityEndorsements.OrderBy(x => x.DocumentNum).Last().PolicyId;
            }
            else
            {
                throw new BusinessException(Resources.Resources.ErrorPolicyDontExistOrNotValidToOccurrenceDate);
            }
        }

        public List<int> GetEndorsementTypesClaimEnabled()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.ClaimEndorsementType.Properties.IsClaimEnabled, typeof(CLMEN.ClaimEndorsementType).Name);
            filter.Equal();
            filter.Constant(true);

            return DataFacadeManager.GetObjects(typeof(CLMEN.ClaimEndorsementType), filter.GetPredicate()).Cast<CLMEN.ClaimEndorsementType>().Select(x => x.EndoTypeCode).ToList();
        }

        public int GetInsuredIdByRiskId(int riskId)
        {
            PrimaryKey primaryKey = ISSEN.Risk.CreatePrimaryKey(riskId);

            return ((ISSEN.Risk)DataFacadeManager.GetObject(primaryKey)).InsuredId;
        }
    }
}
