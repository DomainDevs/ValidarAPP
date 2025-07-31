using Sistran.Co.Application.Data;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Quotation.Entities;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreAssemblers = Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using CoreEnum = Sistran.Core.Application.UnderwritingServices.Enums;
using ENTQUO = Sistran.Company.Application.Quotation.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAM = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoverageDAO
    {
        /// <summary>
        /// Obtener coberturas de un riesgo temporal
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="riskId">Id riesgo</param>
        /// <returns>Lista de coberturas</returns>
        public List<CompanyCoverage> GetCompanyCoveragesByTemporalIdRiskIdProductId(int temporalId, int riskId, int productId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(TMPEN.TempRiskCoverage.Properties.TempId, typeof(TMPEN.TempRiskCoverage).Name);
            filter.Equal();
            filter.Constant(temporalId);
            filter.And();
            filter.Property(TMPEN.TempRiskCoverage.Properties.RiskId, typeof(TMPEN.TempRiskCoverage).Name);
            filter.Equal();
            filter.Constant(riskId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(productId);

            TempRiskCoverageView view = new TempRiskCoverageView();
            ViewBuilder builder = new ViewBuilder("TempRiskCoverageView");
            builder.Filter = filter.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.TempRiskCoverages.Count > 0)
            {
                List<TMPEN.TempRiskCoverDeduct> tempRiskCoverDeducts = new List<TMPEN.TempRiskCoverDeduct>();
                List<QUOEN.Coverage> entityCoverages = new List<QUOEN.Coverage>();
                List<PRODEN.GroupCoverage> groupCoverages = new List<PRODEN.GroupCoverage>();

                if (view.TempRiskCoverDeducts.Count > 0)
                {
                    tempRiskCoverDeducts = view.TempRiskCoverDeducts.Cast<TMPEN.TempRiskCoverDeduct>().ToList();
                }
                if (view.Coverages.Count > 0)
                {
                    entityCoverages = view.Coverages.Cast<QUOEN.Coverage>().ToList();
                }
                if (view.GroupCoverages.Count > 0)
                {
                    groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
                }

                foreach (TMPEN.TempRiskCoverage item in view.TempRiskCoverages)
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                }

                List<CompanyCoverage> coverages = ModelAssembler.CreateTemporalCoverages(view.TempRiskCoverages);

                TP.Parallel.ForEach(coverages, item =>
                 {
                     item.Description = entityCoverages.First(x => x.CoverageId == item.Id).PrintDescription;
                     if (tempRiskCoverDeducts.Exists(x => x.CoverageId == item.Id))
                     {
                         item.Deductible = ModelAssembler.CreateTemporalDeductible(tempRiskCoverDeducts.Where(x => x.CoverageId == item.Id).First());
                     }
                     item.SubLineBusiness = ModelAssembler.CreateSubLineBusiness(entityCoverages.Where(x => x.CoverageId == item.Id).First());
                     item.RuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).RuleSetId;
                     item.PosRuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).PosRuleSetId;
                     item.CoverStatus = CoreEnum.CoverageStatusType.Original;
                     item.InsuredObject = new CompanyInsuredObject
                     {
                         Id = entityCoverages.First(x => x.CoverageId == item.Id).InsuredObjectId
                     };
                 });
                return coverages.OrderBy(x => x.Number).ToList();
            }
            else
            {
                return null;
            }
        }

        private bool CoverageExists(QUOEN.Coverage coverage)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(QUOEN.Coverage.Properties.LineBusinessCode, coverage.LineBusinessCode);
            filter.And().PropertyEquals(QUOEN.Coverage.Properties.SubLineBusinessCode, coverage.SubLineBusinessCode);
            filter.And().PropertyEquals(QUOEN.Coverage.Properties.PerilCode, coverage.PerilCode);
            filter.And().PropertyEquals(QUOEN.Coverage.Properties.InsuredObjectId, coverage.InsuredObjectId);
            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(QUOEN.Coverage), filter.GetPredicate()));
            }
            if (businessCollection != null & businessCollection.Any())
            {
                return businessCollection.Any();
            }
            else
            {
                return false;
            }
        }

        private QUOEN.Coverage GetCoverageById(int id)
        {
            PrimaryKey coveragePK = QUOEN.Coverage.CreatePrimaryKey(id);
            return (QUOEN.Coverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(coveragePK);
        }

        /// <summary>
        /// Obtener Coberturas por Riesgo
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <param name="endorsementId">Id Endoso</param>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Coberturas</returns>
        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            ObjectCriteriaBuilder filterCoverage = new ObjectCriteriaBuilder();
            filterCoverage.PropertyEquals(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name, endorsementId);
            filterCoverage.And();
            filterCoverage.PropertyEquals(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name, riskId);
            filterCoverage.And();
            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Distinct();
            filterCoverage.Constant(CoreEnum.CoverageStatusType.Excluded);

            CompanyPolicyCoverageView view = new CompanyPolicyCoverageView();
            ViewBuilder builder = new ViewBuilder("CompanyPolicyCoverageView");
            builder.Filter = filterCoverage.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
            List<QUOEN.Coverage> coverageEntities = view.Coverages.Cast<QUOEN.Coverage>().ToList();
            List<QUOEN.CoCoverage> coCoverageEntities = view.CoCoverage.Cast<QUOEN.CoCoverage>().ToList();
            List<ISSEN.EndorsementRiskCoverage> entityEndorsementRiskCoverage = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
            List<ISSEN.RiskCoverDeduct> riskCoverDeducts = view.RiskCoverDeducts.Cast<ISSEN.RiskCoverDeduct>().ToList();
            List<QUOEN.InsuredObject> insuredObjects = view.InsuredObject.Cast<QUOEN.InsuredObject>().ToList();
            List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
            List<QUOEN.AllyCoverage> allyCoverages = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
            List<ISSEN.RiskCoverClause> riskCoverClauses = view.RiskCoverClauses.Cast<ISSEN.RiskCoverClause>().ToList();

            if (entityEndorsementRiskCoverage.Count < 1)
            {
                throw new Exception(Errors.ErrorCoverages);
            }

            PrimaryKey primaryKey = ISSEN.Endorsement.CreatePrimaryKey(entityEndorsementRiskCoverage[0].EndorsementId, entityEndorsementRiskCoverage[0].PolicyId);
            ISSEN.Endorsement entityEndorsement = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                entityEndorsement = (ISSEN.Endorsement)daf.GetObjectByPrimaryKey(primaryKey);
            }

            foreach (ISSEN.RiskCoverage item in riskCoverages)
            {
                QUOEN.Coverage itemCoverage = coverageEntities.FirstOrDefault(p => p.CoverageId == item.CoverageId);
                QUOEN.CoCoverage coCoverage = coCoverageEntities.FirstOrDefault(p => p.CoverageId == item.CoverageId);
                CompanyCoverage companyCoverage = ModelAssembler.CreateCompanyCoverage(CoreAssemblers.ModelAssembler.CreatePolicyCoverage(item));
                if (itemCoverage != null)
                {
                    QUOEN.InsuredObject insuredObject = insuredObjects.FirstOrDefault(p => p.InsuredObjectId == itemCoverage.InsuredObjectId);

                    companyCoverage.Description = itemCoverage.PrintDescription;
                    companyCoverage.CoverStatus = (CoreEnum.CoverageStatusType)entityEndorsementRiskCoverage.First(x => x.RiskCoverId == item.RiskCoverId).CoverStatusCode;
                    companyCoverage.SubLineBusiness = new CompanySubLineBusiness()
                    {
                        Id = itemCoverage.SubLineBusinessCode,
                        LineBusiness = new CompanyLineBusiness()
                        {
                            Id = itemCoverage.LineBusinessCode
                        }
                    };
                    if (companyCoverage.InsuredObject == null)
                    {
                        companyCoverage.InsuredObject = new CompanyInsuredObject()
                        {
                            Id = itemCoverage.InsuredObjectId,
                            Description = insuredObject.Description,
                            IsDeclarative = insuredObject.IsDeclarative
                        };
                    }
                }
                if (coCoverage != null)
                {
                    companyCoverage.IsAssistance = coCoverage.IsAssistance;
                    companyCoverage.IsChild = coCoverage.IsChild;
                    companyCoverage.IsAccMinPremium = coCoverage.IsAccMinPremium;
                }
                companyCoverage.IsPrimary = coverageEntities.First(x => x.CoverageId == item.CoverageId).IsPrimary;
                companyCoverage.MainCoverageId = groupCoverages.First(x => x.CoverageId == item.CoverageId).MainCoverageId;
                companyCoverage.MainCoveragePercentage = groupCoverages.First(x => x.CoverageId == item.CoverageId).MainCoveragePercentage;
                companyCoverage.IsSelected = groupCoverages.First(x => x.CoverageId == item.CoverageId).IsSelected;
                companyCoverage.IsMandatory = groupCoverages.First(x => x.CoverageId == item.CoverageId).IsMandatory;
                int? AllyCoverage = null;
                var allyCoverage = allyCoverages.Where(x => x.AllyCoverageId == companyCoverage.Id);
                if (allyCoverage != null && allyCoverage.Count() > 0)
                {
                    AllyCoverage = allyCoverage.FirstOrDefault().AllyCoverageId;
                }
                if (AllyCoverage != null)
                {
                    companyCoverage.AllyCoverageId = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).AllyCoverageId;
                    companyCoverage.SublimitPercentage = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoveragePercentage;
                    companyCoverage.MainCoverageId = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoverageId;
                }
                companyCoverage.EndorsementType = (CoreEnum.EndorsementType)entityEndorsement.EndoTypeCode;
                coverages.Add(companyCoverage);
            }

            List<ISSEN.EndorsementRiskCoverage> endorsementCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
            List<QUOEN.Coverage> entityCoverages = view.Coverages.Cast<QUOEN.Coverage>().ToList();

            foreach (CompanyCoverage item in coverages)
            {
                item.CoverageOriginalStatus = (CoreEnum.CoverageStatusType)endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverStatusCode;
                item.Number = endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverNum;
                item.InsuredObject = new CompanyInsuredObject
                {
                    Id = entityCoverages.First(x => x.CoverageId == item.Id).InsuredObjectId,
                    Description = entityCoverages.First(x => x.CoverageId == item.Id).PrintDescription,
                };

                if (riskCoverDeducts.Exists(x => x.RiskCoverId == item.RiskCoverageId))
                {
                    AutoMapper.IMapper imapper = AutoMapperAssembler.CreateMapDeducible();
                    item.Deductible = imapper.Map<Deductible, CompanyDeductible>(CoreAssemblers.ModelAssembler.CreateCoverageDeductible(riskCoverDeducts.First(x => x.RiskCoverId == item.RiskCoverageId)));
                    PARAM.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAM.DeductibleUnit>().ToList().FirstOrDefault();
                    if (deductibleUnit != null)
                    {
                        item.Deductible.DeductibleUnit.Description = deductibleUnit.Description;
                    }
                    deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAM.DeductibleUnit>().ToList().FirstOrDefault();
                    if (deductibleUnit != null)
                    {
                        item.Deductible.MinDeductibleUnit.Description = deductibleUnit.Description;
                    }
                    item.Deductible.Description = item.Deductible.DeductValue + " " + item.Deductible.DeductibleUnit.Description;
                    Core.Application.Common.Entities.Currency currency = view.Currencies.Cast<Core.Application.Common.Entities.Currency>().ToList().FirstOrDefault();
                    if (currency != null)
                    {
                        item.Deductible.Currency.Description = currency.Description;
                        item.Deductible.Description += "(" + item.Deductible.Currency.Description + ")";
                    }
                    item.Deductible.Description += " - " + item.Deductible.MinDeductValue + " " + item.Deductible.MinDeductibleUnit.Description;
                }
                if (riskCoverClauses.Exists(x=> x.RiskCoverId == item.RiskCoverageId))
                {
                    item.Clauses = DelegateService.underwritingService.AddClauses(null, riskCoverClauses.Where(x => x.RiskCoverId == item.RiskCoverageId).Select(x => x.ClauseId).ToList());
                }
            }
            return coverages;
        }

        public List<CompanyCoverage> RemoveCoverages(List<CompanyCoverage> companyCoverages, List<int> coverageIds)
        {
            if (companyCoverages != null && coverageIds != null)
            {
                companyCoverages = companyCoverages.Where(x => !coverageIds.Any(y => y == x.Id)).ToList();
            }
            return companyCoverages;
        }

        /// <summary>
        /// Validar Coverturas Con Post Contractuales
        /// </summary>
        /// <param name="Policyid"></param>
        /// <returns></returns>
        public System.Collections.ArrayList ValidateCoveragePostContractual(int Policyid)
        {
            System.Collections.ArrayList coverPost;
            System.Data.DataTable result;
            Co.Application.Data.NameValue[] parameters = new Co.Application.Data.NameValue[1];
            try
            {
                parameters[0] = new Co.Application.Data.NameValue("@POLICY_ID", Policyid);
                using (Co.Application.Data.DynamicDataAccess pdb = new Co.Application.Data.DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("ISS.VALIDATE_CIA_COVERAGE_POSTCONTRACTUAL", parameters);
                }

                coverPost = new System.Collections.ArrayList(result.Rows.Count);

                if (result != null && result.Rows.Count > 0)
                {
                    for (int i = 0; i < result.Rows.Count; i++)
                        coverPost.Add(new object[] { (int)result.Rows[i][0], result.Rows[i][1] });

                }
                return coverPost;
            }
            catch (Exception ex)
            {
                throw new Core.Framework.BAF.BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// consulta si la poliza tiene coberturas postcontractuales 
        /// tabla: PRV_RISK_COVERAGE_POST para el ultimo endoso registrado
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <returns></returns>
        public bool ValidatePolicyWithCovPostcontractual(int PolicyId)
        {
            System.Collections.ArrayList coverPost;
            System.Data.DataTable result;
            Co.Application.Data.NameValue[] parameters = new Co.Application.Data.NameValue[1];
            try
            {
                parameters[0] = new Co.Application.Data.NameValue("@POLICY_ID", PolicyId);
                using (Co.Application.Data.DynamicDataAccess pdb = new Co.Application.Data.DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("ISS.VALIDATE_PRV_COVERAGE_POSTCONTRACTUAL", parameters);
                }
                coverPost = new System.Collections.ArrayList(result.Rows.Count);
                return true;
            }
            catch (Exception ex)
            {
                throw new Core.Framework.BAF.BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get list coverages postcontractual prefix surety
        /// </summary>
        /// <returns>List<CompanyCoverage></returns>
        public List<CompanyCoverage> getCoberturaPostContractualPrv()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENTQUO.CiaCoverage.Properties.IsPost, typeof(ENTQUO.CiaCoverage).Name);
            filter.Equal();
            filter.Constant(1);

            IList<CiaCoverage> result = DataFacadeManager.GetObjects(typeof(ENTQUO.CiaCoverage), filter.GetPredicate()).Select(m => (ENTQUO.CiaCoverage)m);
            DataFacadeManager.Dispose();
            List<CompanyCoverage> listCoverages = new List<CompanyCoverage>();
            foreach (ENTQUO.CiaCoverage item in result)
            {
                CompanyCoverage covPostcontractual = new CompanyCoverage();
                covPostcontractual.Id = item.CoverageId;
                covPostcontractual.IsPostcontractual = item.IsPost;
                listCoverages.Add(covPostcontractual);
            }
            return listCoverages;
        }

        public CiaCoverage GetPrvCoverageByIdAndNum(int coverageId, int coverageNum)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CiaCoverage.Properties.CoverageId, coverageId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(CiaCoverage), filter.GetPredicate());

            if (businessCollection.Count == 1)
            {
                return new CiaCoverage(coverageId, coverageNum)
                {
                    CoverageId = coverageId,
                    CoverageNum = coverageNum,
                    IsPost = ((CiaCoverage)businessCollection[0]).IsPost,
                    BeginDate = ((CiaCoverage)businessCollection[0]).BeginDate
                };
            }
            else
            {
                return null;
            }
        }

        public void UpdatePrvCoverage(CiaCoverage entityCiaCoverage)
        {
            PrimaryKey key = CiaCoverage.CreatePrimaryKey(entityCiaCoverage.CoverageId, entityCiaCoverage.CoverageNum);
            CiaCoverage entityCiaCoverag = (CiaCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entityCiaCoverag != null)
            {
                entityCiaCoverag.IsPost = entityCiaCoverage.IsPost;
                entityCiaCoverag.BeginDate = entityCiaCoverage.BeginDate;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCiaCoverag);
            }
        }

        public void CreatePrvCoverage(CiaCoverage entityCiaCoverage)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCiaCoverage);
        }

        #region coberturas
        /// <summary>
        /// Obtener Coberturas por el id del riesgo
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdByRiskId(int policyId, int riskId)
        {
            ObjectCriteriaBuilder filterCoverage = new ObjectCriteriaBuilder();

            filterCoverage.PropertyEquals(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name, policyId);
            filterCoverage.And();
            filterCoverage.PropertyEquals(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name, riskId);
            filterCoverage.And();
            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Distinct();
            filterCoverage.Constant(CoreEnum.CoverageStatusType.Excluded);

            CompanyPolicyCoverageView view = new CompanyPolicyCoverageView();
            ViewBuilder builder = new ViewBuilder("CompanyPolicyCoverageView");
            builder.Filter = filterCoverage.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            List<PRODEN.GroupCoverage> groupCoverages = new List<PRODEN.GroupCoverage>();
            if (view.GroupCoverages.Count > 0)
            {
                groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
            }

            List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
            List<QUOEN.Coverage> coverageEntities = view.Coverages.Cast<QUOEN.Coverage>().ToList();
            List<QUOEN.CoCoverage> coCoverageEntities = view.CoCoverage.Cast<QUOEN.CoCoverage>().ToList();
            List<ISSEN.EndorsementRiskCoverage> entityEndorsementRiskCoverage = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
            List<ISSEN.RiskCoverDeduct> riskCoverDeducts = view.RiskCoverDeducts.Cast<ISSEN.RiskCoverDeduct>().ToList();
            if (entityEndorsementRiskCoverage != null && entityEndorsementRiskCoverage.Any())
            {
                PrimaryKey primaryKey = ISSEN.Endorsement.CreatePrimaryKey(entityEndorsementRiskCoverage[0].EndorsementId, entityEndorsementRiskCoverage[0].PolicyId);
                ISSEN.Endorsement entityEndorsement = (ISSEN.Endorsement)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
                PRODEN.GroupCoverage itemgroupCoverage = null;
                foreach (ISSEN.RiskCoverage item in riskCoverages)
                {
                    QUOEN.Coverage itemCoverage = coverageEntities.FirstOrDefault(p => p.CoverageId == item.CoverageId);
                    QUOEN.CoCoverage coCoverage = coCoverageEntities.FirstOrDefault(p => p.CoverageId == item.CoverageId);

                    CompanyCoverage companyCoverage = ModelAssembler.CreateCompanyCoverage(CoreAssemblers.ModelAssembler.CreatePolicyCoverage(item));
                    if (itemCoverage != null)
                    {
                        if (groupCoverages.Count > 0)
                        {
                            itemgroupCoverage = groupCoverages.FirstOrDefault(g => g.CoverageId == item.CoverageId);
                            if (itemgroupCoverage != null)
                            {
                                companyCoverage.IsSelected = itemgroupCoverage.IsSelected;
                            }
                        }
                        companyCoverage.Description = itemCoverage.PrintDescription;
                        companyCoverage.CoverStatus = (CoreEnum.CoverageStatusType)entityEndorsementRiskCoverage.First(x => x.RiskCoverId == item.RiskCoverId).CoverStatusCode;
                        companyCoverage.SubLineBusiness = new CompanySubLineBusiness()
                        {
                            Id = itemCoverage.SubLineBusinessCode,
                            LineBusiness = new CompanyLineBusiness()
                            {
                                Id = itemCoverage.LineBusinessCode
                            }
                        };
                        if (companyCoverage.InsuredObject == null)
                        {
                            companyCoverage.InsuredObject = new CompanyInsuredObject()
                            {
                                Id = itemCoverage.InsuredObjectId
                            };
                        }
                    }
                    if (coCoverage != null)
                    {
                        companyCoverage.IsAssistance = coCoverage.IsAssistance;
                        companyCoverage.IsChild = coCoverage.IsChild;
                        companyCoverage.IsAccMinPremium = coCoverage.IsAccMinPremium;
                    }
                    companyCoverage.EndorsementType = (CoreEnum.EndorsementType)entityEndorsement.EndoTypeCode;
                    coverages.Add(companyCoverage);
                }

                List<ISSEN.EndorsementRiskCoverage> endorsementCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                List<QUOEN.Coverage> entityCoverages = view.Coverages.Cast<QUOEN.Coverage>().ToList();

                foreach (CompanyCoverage item in coverages)
                {
                    item.CoverageOriginalStatus = (CoreEnum.CoverageStatusType)endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverStatusCode;
                    item.Number = endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverNum;
                    item.InsuredObject = new CompanyInsuredObject
                    {
                        Id = entityCoverages.First(x => x.CoverageId == item.Id).InsuredObjectId
                    };

                    if (riskCoverDeducts.Exists(x => x.RiskCoverId == item.RiskCoverageId))
                    {
                        AutoMapper.IMapper imapper = AutoMapperAssembler.CreateMapDeducible();
                        item.Deductible = imapper.Map<Deductible, CompanyDeductible>(CoreAssemblers.ModelAssembler.CreateCoverageDeductible(riskCoverDeducts.First(x => x.RiskCoverId == item.RiskCoverageId)));
                        PARAM.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAM.DeductibleUnit>().ToList().FirstOrDefault();
                        if (deductibleUnit != null)
                        {
                            item.Deductible.DeductibleUnit.Description = deductibleUnit.Description;
                        }
                        deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAM.DeductibleUnit>().ToList().FirstOrDefault();
                        if (deductibleUnit != null)
                        {
                            item.Deductible.MinDeductibleUnit.Description = deductibleUnit.Description;
                        }
                        item.Deductible.Description = item.Deductible.DeductValue + " " + item.Deductible.DeductibleUnit.Description;
                        Core.Application.Common.Entities.Currency currency = view.Currencies.Cast<Core.Application.Common.Entities.Currency>().ToList().FirstOrDefault();
                        if (currency != null)
                        {
                            item.Deductible.Currency.Description = currency.Description;
                            item.Deductible.Description += "(" + item.Deductible.Currency.Description + ")";
                        }
                        item.Deductible.Description += " - " + item.Deductible.MinDeductValue + " " + item.Deductible.MinDeductibleUnit.Description;
                    }
                }
                return coverages;
            }
            else
            {
                throw new Exception("No existe el riesgo");
            }
        }
        #endregion
        public CompanySummary getTotalSumary(int endorsementId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            DataTable dt;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dt = dynamicDataAccess.ExecuteSPDataTable("QUO.GET_TOTAL_SUMMARY", parameters);
            }

            CompanySummary companySummary = new CompanySummary();
            if (dt != null && dt.Rows.Count > 0)
            {
                companySummary = new CompanySummary()
                {
                    RiskCount = Convert.ToInt16(dt.Rows[0][0].ToString()),
                    AmountInsured = Convert.ToDecimal(dt.Rows[0][1].ToString())
                };
            }
            return companySummary;
        }

        public long GetRateCoveragesByCoverageIdPolicyId(int policyId, int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("POLICY_ID", policyId);
            parameters[1] = new NameValue("COVERAGE_ID", coverageId);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ISS.GET_RATE_COVERAGES", parameters);
            }
            return (long)Convert.ToDecimal(result.Rows[0].ItemArray[0]);
        }
    }
}