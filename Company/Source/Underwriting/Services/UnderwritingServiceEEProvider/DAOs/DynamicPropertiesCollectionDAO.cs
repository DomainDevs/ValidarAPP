using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class DynamicPropertiesCollectionDAO
    {
        /// <summary>
        /// LoadDynamicPropertiesEndorsementConcepts
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <returns>DynamicPropertiesCollection</returns>
        //public List<DynamicConcept> LoadDynamicPropertiesEndorsementConcepts(int policyId, int endorsementId)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(ISSEN.EndorsementConcepts.Properties.PolicyId, typeof(ISSEN.EndorsementConcepts).Name).Equal().Constant(policyId);
        //    filter.And();
        //    filter.Property(ISSEN.EndorsementConcepts.Properties.EndorsementId, typeof(ISSEN.EndorsementConcepts).Name).Equal().Constant(endorsementId);

        //    BusinessCollection businessCollection = null;
        //    using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //    {
        //        businessCollection = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.EndorsementConcepts), filter.GetPredicate()));
        //    }
        //    List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

        //    if (businessCollection != null && businessCollection.Count > 0)
        //    {
        //        List<ISSEN.EndorsementConcepts> endorsementConcepts = businessCollection.Cast<ISSEN.EndorsementConcepts>().ToList();
        //        foreach (ISSEN.EndorsementConcepts endorsementConcept in endorsementConcepts)
        //        {
        //            DynamicConcept dynamicConcept = new DynamicConcept
        //            {
        //                Id = endorsementConcept.ConceptId,
        //                Value = endorsementConcept.ConceptValue,
        //                EntityId = endorsementConcept.EntityId,
        //                QuestionId = endorsementConcept.QuestionId
        //            };

        //            if (dynamicConcept.Value != null &&
        //                DateTime.TryParseExact(dynamicConcept.Value.ToString(),
        //                StringHelper.DateFormatCompatibleRulesR1(),
        //                System.Globalization.CultureInfo.InvariantCulture,
        //                System.Globalization.DateTimeStyles.None, out DateTime dateTime))
        //            {
        //                dynamicConcept.Value = dateTime;
        //            }
        //            else if (dynamicConcept.Value != null && dynamicConcept.ValueType == typeof(DateTime).ToString())
        //            {
        //                dynamicConcept.Value = DateTime.Parse(dynamicConcept.Value.ToString());
        //            }
        //            dynamicConcept.ValueType = dynamicConcept.Value != null ? dynamicConcept.Value.GetType().FullName : "Null";
        //            dynamicConcepts.Add(dynamicConcept);
        //        }

        //        return dynamicConcepts;
        //    }
        //    else
        //    {
        //        return dynamicConcepts;
        //    }
        //}


        /// <summary>
        /// LoadDynamicPropertiesRiskConcepts
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <param name="riskId">Id risk</param>
        /// <returns>DynamicPropertiesCollection</returns>
        public List<DynamicConcept> LoadDynamicPropertiesRiskConcepts(int policyId, int endorsementId, int riskId, int riskNum)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskConcepts.Properties.PolicyId, typeof(ISSEN.RiskConcepts).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.RiskConcepts.Properties.EndorsementId, typeof(ISSEN.RiskConcepts).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.RiskConcepts.Properties.RiskId, typeof(ISSEN.RiskConcepts).Name);
            filter.Equal();
            filter.Constant(riskId);

            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.RiskConcepts), filter.GetPredicate()));
            }
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            if (businessCollection != null && businessCollection.Count > 0)
            {
                IList<ISSEN.RiskConcepts> riskConcepts = businessCollection.Cast<ISSEN.RiskConcepts>().ToList();
                foreach (ISSEN.RiskConcepts riskConcept in riskConcepts)
                {
                    DynamicConcept dynamicConcept = new DynamicConcept
                    {
                        Id = riskConcept.ConceptId,
                        Value = riskConcept.ConceptValue,
                        EntityId = riskConcept.EntityId,
                        QuestionId = riskConcept.QuestionId
                    };
                    if (dynamicConcept.Value != null &&
                        DateTime.TryParseExact(dynamicConcept.Value.ToString(),
                        StringHelper.DateFormatCompatibleRulesR1(),
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                    {
                        dynamicConcept.Value = dateTime;
                    }
                    else if (dynamicConcept.Value != null && dynamicConcept.ValueType == typeof(DateTime).ToString())
                    {
                        dynamicConcept.Value = DateTime.Parse(dynamicConcept.Value.ToString());
                    }
                    dynamicConcept.ValueType = dynamicConcept.Value != null ? dynamicConcept.Value.GetType().FullName : "Null";
                    dynamicConcepts.Add(dynamicConcept);
                }

                return dynamicConcepts;
            }
            else
            {
                dynamicConcepts = DelegateService.underwritingService.GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(endorsementId, riskNum, policyId, riskId);
                return dynamicConcepts;
            }
        }


        /// <summary>
        /// LoadDynamicPropertiesRiskConcepts
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <param name="riskId">Id risk</param>
        /// <returns>DynamicPropertiesCollection</returns>
        //public List<DynamicConcept> LoadDynamicPropertiesRiskCoverConcepts(int policyId, int endorsementId, int riskId, int coverageId)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(ISSEN.RiskCoverConcepts.Properties.PolicyId, typeof(ISSEN.RiskCoverConcepts).Name);
        //    filter.Equal();
        //    filter.Constant(policyId);
        //    filter.And();
        //    filter.Property(ISSEN.RiskCoverConcepts.Properties.EndorsementId, typeof(ISSEN.RiskCoverConcepts).Name);
        //    filter.Equal();
        //    filter.Constant(endorsementId);
        //    filter.And();
        //    filter.Property(ISSEN.RiskCoverConcepts.Properties.RiskId, typeof(ISSEN.RiskCoverConcepts).Name);
        //    filter.Equal();
        //    filter.Constant(riskId);
        //    filter.And();
        //    filter.Property(ISSEN.RiskCoverConcepts.Properties.CoverageId, typeof(ISSEN.RiskCoverConcepts).Name);
        //    filter.Equal();
        //    filter.Constant(coverageId);

        //    BusinessCollection businessCollection = null;
        //    using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //    {
        //        businessCollection = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.RiskCoverConcepts), filter.GetPredicate()));
        //    }
        //    List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

        //    if (businessCollection != null && businessCollection.Count > 0)
        //    {
        //        IList<ISSEN.RiskCoverConcepts> RiskCoverConcepts = businessCollection.Cast<ISSEN.RiskCoverConcepts>().ToList();
        //        foreach (ISSEN.RiskCoverConcepts riskConcept in RiskCoverConcepts)
        //        {
        //            DynamicConcept dynamicConcept = new DynamicConcept
        //            {
        //                Id = riskConcept.ConceptId,
        //                Value = riskConcept.ConceptValue,
        //                EntityId = riskConcept.EntityId,
        //                QuestionId = riskConcept.QuestionId
        //            };
        //            if (dynamicConcept.Value != null &&
        //                DateTime.TryParseExact(dynamicConcept.Value.ToString(),
        //                StringHelper.DateFormatCompatibleRulesR1(),
        //                System.Globalization.CultureInfo.InvariantCulture,
        //                System.Globalization.DateTimeStyles.None, out DateTime dateTime))
        //            {
        //                dynamicConcept.Value = dateTime;
        //            }
        //            else if (dynamicConcept.Value != null && dynamicConcept.ValueType == typeof(DateTime).ToString())
        //            {
        //                dynamicConcept.Value = DateTime.Parse(dynamicConcept.Value.ToString());
        //            }
        //            dynamicConcept.ValueType = dynamicConcept.Value != null ? dynamicConcept.Value.GetType().FullName : "Null";
        //            dynamicConcepts.Add(dynamicConcept);
        //        }

        //        return dynamicConcepts;
        //    }
        //    else
        //    {
        //        return dynamicConcepts;
        //    }
        //}
    }
}
