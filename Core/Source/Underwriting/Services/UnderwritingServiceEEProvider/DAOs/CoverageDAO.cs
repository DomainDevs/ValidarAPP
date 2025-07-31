using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ENCO = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using TASKU = Sistran.Core.Application.Utilities.Utility;
using TMPEN = Sistran.Core.Application.Temporary.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoverageDAO
    {
        /// <summary>
        /// Finds the specified QUOEN.Coverage identifier.
        /// </summary>
        /// <param name="coverageId">The QUOEN.Coverage identifier.</param>
        /// <returns></returns>
        public static QUOEN.Coverage GetCoverageEntityByCoverageId(int coverageId)
        {
            PrimaryKey key = QUOEN.Coverage.CreatePrimaryKey(coverageId);
            return (QUOEN.Coverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Finds the specified QUOEN.Coverage identifier.
        /// </summary>
        /// <param name="coverageId">The QUOEN.Coverage identifier.</param>
        /// <returns>Datos de la cobertura</returns>
        public Model.Coverage GetCoverageByCoverageIdProductIdGroupCoverageId(int coverageId, int productId,
            int groupCoverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(coverageId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.CoverGroupId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);

            CoverageView view = new CoverageView();
            ViewBuilder builder = new ViewBuilder("CoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Coverages.Count > 0)
            {
                List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
                if (coverages != null && coverages.Any())
                {
                    List<QUOEN.CoCoverage> coCoverages =
                 view.CoCoverages.Cast<QUOEN.CoCoverage>().ToList();
                    List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
                    List<QUOEN.CoCoverageValue> coCoverageValues =
                        view.CoCoverageValues.Cast<QUOEN.CoCoverageValue>().ToList();
                    List<QUOEN.CoverDetailType> coverDetailTypes =
                        view.CoverDetailTypes.Cast<QUOEN.CoverDetailType>().ToList();
                    List<ENCO.SubLineBusiness> subLinesBusiness =
                        view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();
                    object obj = new object();
                    TASKU.Parallel.For(0, coverages.Count, itemRow =>
                    {
                        Model.Coverage item = null;
                        lock (obj)
                        {
                            item = coverages[itemRow];
                        }
                        item.IsMandatory = groupCoverages.First(x => x.CoverageId == item.Id).IsMandatory;
                        item.IsSelected = groupCoverages.First(x => x.CoverageId == item.Id).IsSelected;
                        item.Number = groupCoverages.FirstOrDefault(x => x.CoverageId == item.Id)?.CoverNum ?? 0;
                        item.MainCoverageId = groupCoverages.First(x => x.CoverageId == item.Id).MainCoverageId
                        .GetValueOrDefault();
                        item.SublimitPercentage = groupCoverages.First(x => x.CoverageId == item.Id).MainCoveragePercentage;
                        item.FlatRatePorcentage = coCoverageValues.Exists(x => x.CoverageId == item.Id)
                        ? coCoverageValues.First(x => x.CoverageId == item.Id).ValuePje.GetValueOrDefault()
                        : 0;
                        item.RuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).RuleSetId;
                        item.PosRuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).PosRuleSetId;
                        item.IsVisible = coverDetailTypes.Exists(x => x.CoverageId == item.Id) ? false : true;
                        item.SubLineBusiness.Description = subLinesBusiness
                        .First(x => x.SubLineBusinessCode == item.SubLineBusiness.Id).Description;
                        item.SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == item.SubLineBusiness.LineBusiness.Id).Description;
                        item.Deductible = DeductibleDAO.GetDeductibleDefaultByCoverageId(item.Id);

                        if (coCoverages != null && coCoverages.Count > 0)
                        {
                            item.CoverNum = coCoverages.First(x => x.CoverageId == item.Id).CoverageNum;
                            item.IsImpression = coCoverages.First(x => x.CoverageId == item.Id).IsImpression;
                            item.PrintDescription = coCoverages.First(x => x.CoverageId == item.Id).PrintDescription;
                            item.PrintDescriptionLimit = coCoverages.First(x => x.CoverageId == item.Id).PrintDescriptionLimit;
                            item.IsChild = coCoverages.First(x => x.CoverageId == item.Id).IsChild;
                            item.IsAccMinPremium = coCoverages.First(x => x.CoverageId == item.Id).IsAccMinPremium;
                            item.IsAssistance = coCoverages.First(x => x.CoverageId == item.Id).IsAssistance;
                            item.IsSeriousOffer = coCoverages.First(x => x.CoverageId == item.Id).IsSeriousOffer;
                        }
                        lock (obj)
                        {
                            coverages[itemRow] = item;
                        }

                    });

                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(),
                        "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoverageByCoverageIdProductIdGroupCoverageId");
                    return coverages.FirstOrDefault();
                }
                else
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(),
                        "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoverageByCoverageIdProductIdGroupCoverageId");
                    return null;
                }
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(),
                    "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoverageByCoverageIdProductIdGroupCoverageId");
                return null;
            }
        }

        /// <summary>
        /// Finds the specified QUOEN.Coverage identifier.
        /// </summary>
        /// <param name="coverageId">The QUOEN.Coverage identifier.</param>
        /// <returns>Datos de la cobertura</returns>
        public List<Model.Coverage> GetCoverageByCoverageIdProductIdGroupCoverageId(List<int> coverageIds,
            int productId, int groupCoverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
            filter.In();
            filter.ListValue();
            foreach (int coverageId in coverageIds)
            {
                filter.Constant(coverageId);
            }
            filter.EndList();
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.CoverGroupId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);

            CoverageView view = new CoverageView();
            ViewBuilder builder = new ViewBuilder("CoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }


            if (view.Coverages.Count > 0)
            {
                List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
                if (coverages != null && coverages.Any())
                {
                    List<QUOEN.CoCoverage> coCoverages = view.CoCoverages.Cast<QUOEN.CoCoverage>().ToList();
                    List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
                    List<QUOEN.CoCoverageValue> coCoverageValues =
                        view.CoCoverageValues.Cast<QUOEN.CoCoverageValue>().ToList();
                    List<QUOEN.CoverDetailType> coverDetailTypes =
                        view.CoverDetailTypes.Cast<QUOEN.CoverDetailType>().ToList();
                    List<ENCO.SubLineBusiness> subLinesBusiness =
                        view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();
                    List<QUOEN.AllyCoverage> allyCoverages =
                        view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
                    object obj = new object();
                    TASKU.Parallel.For(0, coverages.Count, itemRow =>
                    {
                        Model.Coverage item = null;
                        lock (obj)
                        {
                            item = coverages[itemRow];
                        }
                        item.IsMandatory = groupCoverages.First(x => x.CoverageId == item.Id).IsMandatory;
                        item.IsSelected = groupCoverages.First(x => x.CoverageId == item.Id).IsSelected;
                        item.Number = groupCoverages.FirstOrDefault(x => x.CoverageId == item.Id)?.CoverNum ?? 0;
                        item.MainCoverageId = groupCoverages.First(x => x.CoverageId == item.Id).MainCoverageId
                        .GetValueOrDefault();
                        item.SublimitPercentage = groupCoverages.First(x => x.CoverageId == item.Id).MainCoveragePercentage;
                        item.FlatRatePorcentage = coCoverageValues.Exists(x => x.CoverageId == item.Id)
                        ? coCoverageValues.First(x => x.CoverageId == item.Id).ValuePje.GetValueOrDefault()
                        : 0;
                        item.RuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).RuleSetId;
                        item.PosRuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).PosRuleSetId;
                        item.IsVisible = coverDetailTypes.Exists(x => x.CoverageId == item.Id) ? false : true;
                        item.SubLineBusiness.Description = subLinesBusiness
                        .First(x => x.SubLineBusinessCode == item.SubLineBusiness.Id).Description;
                        item.SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == item.SubLineBusiness.LineBusiness.Id).Description;
                        item.Deductible = DeductibleDAO.GetDeductibleDefaultByCoverageId(item.Id);


                        int? AllyCoverage = null;
                        var allyCoverage = allyCoverages.Where(x => x.AllyCoverageId == item.Id);
                        if (allyCoverage != null && allyCoverage.Count() > 0)
                        {
                            AllyCoverage = allyCoverage.FirstOrDefault().AllyCoverageId;
                        }
                        if (AllyCoverage != null)
                        {
                            item.AllyCoverageId = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoverageId;
                            item.SublimitPercentage = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoveragePercentage;
                        }
                        lock (obj)
                        {
                            coverages[itemRow] = item;
                        }

                    });
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoverageByCoverageIdProductIdGroupCoverageId");
                    return coverages;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(),
                    "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoverageByCoverageIdProductIdGroupCoverageId");
                return null;
            }
        }

        /// <summary>
        /// Obtener Coberturas por Producto, Grupo de Coberturas y Ramo
        /// </summary>
        /// <param name="productId">Id Producto</param>
        /// <param name="groupCoverageId">Id Grupo Cobertura</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId,
            int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.CoverGroupId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);

            CoverageView view = new CoverageView();
            ViewBuilder builder = new ViewBuilder("CoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
            if (coverages.Any())
            {
                List<QUOEN.CoCoverage> coCoverages = view.CoCoverages.Cast<QUOEN.CoCoverage>().ToList();
                List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
                List<PRODEN.GroupInsuredObject> groupInsuredObject = view.GroupInsuredObject.Cast<PRODEN.GroupInsuredObject>().ToList();
                List<QUOEN.CoCoverageValue> coCoverageValues = view.CoCoverageValues.Cast<QUOEN.CoCoverageValue>().ToList();
                List<QUOEN.CoverDetailType> coverDetailTypes = view.CoverDetailTypes.Cast<QUOEN.CoverDetailType>().ToList();
                List<ENCO.SubLineBusiness> subLinesBusiness = view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();
                List<QUOEN.InsuredObject> insuredObjects = view.InsuredObjects.Cast<QUOEN.InsuredObject>().ToList();
                List<QUOEN.AllyCoverage> allyCoverages = null;
                if (view.CoverageAllied != null)
                {
                    allyCoverages = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
                }
                object obj = new object();
                ConcurrentBag<string> concurrentBag = new ConcurrentBag<string>();
                TASKU.Parallel.For(0, coverages.Count, itemRow =>
                  {
                      try
                      {
                          Model.Coverage item = null;
                          lock (obj)
                          {
                              item = coverages[itemRow];
                          }
                          item.IsMandatory = groupCoverages.First(x => x.CoverageId == item.Id).IsMandatory;
                          item.IsSelected = groupCoverages.First(x => x.CoverageId == item.Id).IsSelected;
                          item.Number = groupCoverages.FirstOrDefault(x => x.CoverageId == item.Id)?.CoverNum ?? 0;
                          item.MainCoverageId = groupCoverages.First(x => x.CoverageId == item.Id).MainCoverageId
                          .GetValueOrDefault();
                          item.SublimitPercentage = groupCoverages.First(x => x.CoverageId == item.Id).MainCoveragePercentage;
                          item.FlatRatePorcentage = coCoverageValues.Exists(x => x.CoverageId == item.Id && x.PrefixCode == prefixId)
                          ? coCoverageValues.First(x => x.CoverageId == item.Id && x.PrefixCode == prefixId).ValuePje.GetValueOrDefault()
                          : 0;
                          item.RuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).RuleSetId;
                          item.PosRuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).PosRuleSetId;
                          item.IsVisible = coverDetailTypes.Exists(x => x.CoverageId == item.Id) ? false : true;
                          item.SubLineBusiness.Description = subLinesBusiness
                          .First(x => x.SubLineBusinessCode == item.SubLineBusiness.Id).Description;
                          item.SubLineBusiness.LineBusiness.Description = subLinesBusiness
                          .First(x => x.LineBusinessCode == item.SubLineBusiness.LineBusiness.Id).Description;
                          item.InsuredObject.Id = insuredObjects.First(x => x.InsuredObjectId == item.InsuredObject.Id).InsuredObjectId;
                          item.InsuredObject.Description = insuredObjects.First(x => x.InsuredObjectId == item.InsuredObject.Id).Description.ToString();
                          item.IsSeriousOffer = coCoverages.Exists(x => x.CoverageId == item.Id)
                          ? coCoverages.First(x => x.CoverageId == item.Id).IsSeriousOffer
                          : false;
                          int? AllyCoverage = null;
                          var allyCoverage = allyCoverages.Where(x => x.AllyCoverageId == item.Id);
                          if (allyCoverage != null && allyCoverage.Count() > 0)
                          {
                              AllyCoverage = allyCoverage.FirstOrDefault().AllyCoverageId;
                          }
                          if (AllyCoverage != null)
                          {
                              item.AllyCoverageId = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).AllyCoverageId;
                              item.SublimitPercentage = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoveragePercentage;
                          }
                          lock (obj)
                          {
                              coverages[itemRow] = item;
                          }
                      }
                      catch (Exception ex)
                      {

                          concurrentBag.Add(ex.Message);
                      }

                  });
                if (concurrentBag != null && concurrentBag.Count > 0)
                {
                    throw new Exception(string.Join(";", concurrentBag.ToList()));
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(),
                    "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByProductIdGroupCoverageIdPrefixId");
                return coverages.OrderBy(x => x.Number).ToList();
            }
            else
            {
                return coverages;
            }
        }

        /// <summary>
        /// Obtener lista de coberturas por objeto del seguro
        /// </summary>
        /// <param name="insuredObjectId">Id objeto del seguro</param>        
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.InsuredObjectId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(insuredObjectId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.CoverGroupId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();

            filter.Constant(groupCoverageId);

            if (productId != 0)
            {
                filter.And();
                filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
                filter.Equal();
                filter.Constant(productId);
            }

            List<Model.Coverage> coverages = GetCoveragesByFilter(filter);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(),
                "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId");

            return coverages.OrderBy(x => x.Number).ToList();
        }

        public List<Model.Coverage> GetCoveragesByInsuredObjectIdsGroupCoverageIdProductId(List<int> insuredObjectsIds, int groupCoverageId, int productId, bool filterSelected)
        {
            if (insuredObjectsIds == null || !insuredObjectsIds.Any())
            {
                throw new BusinessException("InsuredObjectsIsNullOrEmpty");
            }
            var filter = new ObjectCriteriaBuilder();
            var groupCoverageTableAlias = typeof(PRODEN.GroupCoverage).Name;
            filter.Property(QUOEN.Coverage.Properties.InsuredObjectId, typeof(QUOEN.Coverage).Name);
            filter.In();
            filter.ListValue();
            foreach (int insuredObjectId in insuredObjectsIds)
            {
                filter.Constant(insuredObjectId);
            }
            filter.EndList();
            filter.And();
            filter.PropertyEquals(PRODEN.GroupCoverage.Properties.CoverGroupId, groupCoverageTableAlias, groupCoverageId);
            filter.And();
            filter.PropertyEquals(PRODEN.GroupCoverage.Properties.ProductId, groupCoverageTableAlias, productId);
            if (filterSelected)
            {
                filter.And();
                filter.PropertyEquals(PRODEN.GroupCoverage.Properties.IsSelected, groupCoverageTableAlias, 1);
            }
            List<Model.Coverage> coverages = GetCoveragesByFilter(filter);
            return coverages.OrderBy(x => x.Number).ToList();
        }

        private List<Model.Coverage> GetCoveragesByFilter(ObjectCriteriaBuilder filter)
        {
            CoverageView view = new CoverageView();
            ViewBuilder builder = new ViewBuilder("CoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }


            List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
            List<QUOEN.CoCoverage> coCoverages = view.CoCoverages.Cast<QUOEN.CoCoverage>().ToList();
            List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
            List<QUOEN.CoCoverageValue> coCoverageValues = view.CoCoverageValues.Cast<QUOEN.CoCoverageValue>().ToList();
            List<QUOEN.CoverDetailType> coverDetailTypes = view.CoverDetailTypes.Cast<QUOEN.CoverDetailType>().ToList();
            List<ENCO.SubLineBusiness> subLinesBusiness = view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();
            List<QUOEN.AllyCoverage> allyCoverages = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
            foreach (Model.Coverage item in coverages)
            {
                PRODEN.GroupCoverage groupCoverage = groupCoverages.First(x => x.CoverageId == item.Id);
                item.IsMandatory = groupCoverage.IsMandatory;
                item.IsSelected = groupCoverage.IsSelected;
                item.CoverNum = groupCoverage.CoverNum;
                item.Number = groupCoverage.CoverNum;
                //item.Number = coCoverages.FirstOrDefault(x => x.CoverageId == item.Id)?.CoverageNum ?? 0;
                item.MainCoverageId = groupCoverage.MainCoverageId.GetValueOrDefault();
                item.SublimitPercentage = groupCoverage.MainCoveragePercentage;
                QUOEN.CoCoverageValue coCoverageValue = coCoverageValues.Find(x => x.CoverageId == item.Id);
                item.FlatRatePorcentage = coCoverageValue != null ? coCoverageValue.ValuePje.GetValueOrDefault() : 0;
                item.RuleSetId = groupCoverage.RuleSetId;
                item.PosRuleSetId = groupCoverage.PosRuleSetId;
                item.IsVisible = !coverDetailTypes.Exists(x => x.CoverageId == item.Id);
                item.SubLineBusiness.Description = subLinesBusiness
                    .First(x => x.SubLineBusinessCode == item.SubLineBusiness.Id).Description;
                item.SubLineBusiness.LineBusiness.Description = subLinesBusiness
                    .First(x => x.LineBusinessCode == item.SubLineBusiness.LineBusiness.Id).Description;
                item.Deductible = DeductibleDAO.GetDeductibleDefaultByCoverageId(item.Id);
                int? AllyCoverage = null;
                var allyCoverage = allyCoverages.Where(x => x.AllyCoverageId == item.Id);
                if (allyCoverage != null && allyCoverage.Count() > 0)
                {
                    AllyCoverage = allyCoverage.FirstOrDefault().AllyCoverageId;
                }
                if (AllyCoverage != null)
                {
                    item.AllyCoverageId = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).AllyCoverageId;
                    item.SublimitPercentage = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoveragePercentage;
                    item.MainCoverageId = allyCoverages.First(x => x.AllyCoverageId == AllyCoverage).CoverageId;
                }
                if (item.MainCoverageId == 0)
                {
                    continue;
                }
            }
            return coverages.OrderBy(x => x.Number).ToList();
        }


        /// <summary>
        /// Obtener las coberturas de accesorios originales y no originales
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id grupo de coberturas</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter { Id = Convert.ToInt32(ExtendedParametersTypes.OriginalAccessories) });
            parameters.Add(new Parameter { Id = Convert.ToInt32(ExtendedParametersTypes.NonOriginalAccessories) });

            parameters = DelegateService.commonServiceCore.GetExtendedParameters(parameters);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.CoverGroupId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);
            filter.And();
            filter.OpenParenthesis();
            filter.Property(QUOEN.CoCoverageValue.Properties.PrefixCode, typeof(QUOEN.CoCoverageValue).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.Or();
            filter.Property(QUOEN.CoCoverageValue.Properties.PrefixCode, typeof(QUOEN.CoCoverageValue).Name);
            filter.IsNull();
            filter.CloseParenthesis();
            filter.And();
            filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
            filter.In();
            filter.ListValue();
            foreach (Parameter item in parameters)
            {
                filter.Constant(item.NumberParameter.Value);
            }
            filter.EndList();

            CoverageView view = new CoverageView();
            ViewBuilder builder = new ViewBuilder("CoverageView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Coverages.Count > 0)
            {
                List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
                List<QUOEN.CoCoverage> coCoverages = view.CoCoverages.Cast<QUOEN.CoCoverage>().ToList();
                List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
                List<QUOEN.CoCoverageValue> coCoverageValues = view.CoCoverageValues.Cast<QUOEN.CoCoverageValue>().ToList();
                List<QUOEN.CoverDetailType> coverDetailTypes = view.CoverDetailTypes.Cast<QUOEN.CoverDetailType>().ToList();
                List<ENCO.SubLineBusiness> subLinesBusiness = view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();

                foreach (Model.Coverage item in coverages)
                {
                    item.IsMandatory = groupCoverages.First(x => x.CoverageId == item.Id).IsMandatory;
                    item.IsSelected = groupCoverages.First(x => x.CoverageId == item.Id).IsSelected;
                    item.Number = groupCoverages.FirstOrDefault(x => x.CoverageId == item.Id)?.CoverNum ?? 0;
                    item.MainCoverageId = groupCoverages.First(x => x.CoverageId == item.Id).MainCoverageId.GetValueOrDefault();
                    item.SublimitPercentage = groupCoverages.First(x => x.CoverageId == item.Id).MainCoveragePercentage;
                    item.FlatRatePorcentage = coCoverageValues.Exists(x => x.CoverageId == item.Id) ? coCoverageValues.First(x => x.CoverageId == item.Id).ValuePje.GetValueOrDefault() : 0;
                    item.RuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).RuleSetId;
                    item.PosRuleSetId = groupCoverages.First(x => x.CoverageId == item.Id).PosRuleSetId;
                    item.IsVisible = coverDetailTypes.Exists(x => x.CoverageId == item.Id) ? false : true;
                    item.SubLineBusiness.Description = subLinesBusiness.First(x => x.SubLineBusinessCode == item.SubLineBusiness.Id).Description;
                    item.SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == item.SubLineBusiness.LineBusiness.Id).Description;
                    item.Deductible = DeductibleDAO.GetDeductibleDefaultByCoverageId(item.Id);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId");

                return coverages.OrderBy(x => x.Number).ToList();
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId");

                return null;
            }
        }

        /// <summary>
        /// Obtener las coberturas aliadas
        /// </summary>
        /// <param name="productId">Id QUOEN.Coverage</param>
        /// <returns>Lista de coberturas</returns>
        public List<Models.Coverage> GetAllyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.Coverage> coverages = new List<Models.Coverage>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.AllyCoverage.Properties.CoverageId, typeof(QUOEN.AllyCoverage).Name);
            filter.Equal();
            filter.Constant(coverageId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.AllyCoverage), filter.GetPredicate()));

            foreach (QUOEN.AllyCoverage allyCoverage in businessCollection)
            {
                Models.Coverage coverage = GetCoverageByCoverageIdProductIdGroupCoverageId(allyCoverage.AllyCoverageId, productId, groupCoverageId);
                if (coverage != null)
                {
                    coverage.AllyCoverageId = allyCoverage.AllyCoverageId;
                    coverage.MainCoverageId = coverageId;
                    coverage.SublimitPercentage = allyCoverage.CoveragePercentage;
                    coverages.Add(coverage);
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetAllyCoveragesByCoverageIdProductIdGroupCoverageId");

            return coverages;
        }

        /// <summary>
        /// Obtener las coberturas adicionales 
        /// </summary>
        /// <param name="coverageId">Id QUOEN.Coverage</param>
        /// <param name="productId">Id QUOEN.Group</param>
        /// <param name="groupCoverageId">Id QUOEN.Coverage</param>
        /// <returns>Lista de coberturas</returns>
        public List<Models.Coverage> GetAddCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.Coverage> coverages = new List<Models.Coverage>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.GroupCoverage.Properties.MainCoverageId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(coverageId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.ProductId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(PRODEN.GroupCoverage.Properties.CoverGroupId, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);

            CoverageView view = new CoverageView();
            ViewBuilder builder = new ViewBuilder("CoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Coverages.Count > 0)
            {
                coverages = ModelAssembler.CreateCoverages(view.Coverages);

                if (coverages != null && coverages.Any())
                {
                    List<QUOEN.CoCoverage> coCoverages =
                 view.CoCoverages.Cast<QUOEN.CoCoverage>().ToList();
                    List<PRODEN.GroupCoverage> groupCoverages = view.GroupCoverages.Cast<PRODEN.GroupCoverage>().ToList();
                    List<QUOEN.CoCoverageValue> coCoverageValues =
                        view.CoCoverageValues.Cast<QUOEN.CoCoverageValue>().ToList();
                    List<QUOEN.CoverDetailType> coverDetailTypes =
                        view.CoverDetailTypes.Cast<QUOEN.CoverDetailType>().ToList();
                    List<ENCO.SubLineBusiness> subLinesBusiness =
                        view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();
                    List<QUOEN.AllyCoverage> allyCoverages = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();

                    TASKU.Parallel.ForEach(coverages, coverage =>
                    {
                        coverage.IsMandatory = groupCoverages.First(x => x.CoverageId == coverage.Id).IsMandatory;
                        coverage.IsSelected = groupCoverages.First(x => x.CoverageId == coverage.Id).IsSelected;
                        coverage.Number = groupCoverages.FirstOrDefault(x => x.CoverageId == coverage.Id)?.CoverNum ?? 0;
                        coverage.MainCoverageId = groupCoverages.First(x => x.CoverageId == coverage.Id).MainCoverageId
                        .GetValueOrDefault();
                        coverage.SublimitPercentage = groupCoverages.First(x => x.CoverageId == coverage.Id).MainCoveragePercentage;
                        coverage.FlatRatePorcentage = coCoverageValues.Exists(x => x.CoverageId == coverage.Id)
                        ? coCoverageValues.First(x => x.CoverageId == coverage.Id).ValuePje.GetValueOrDefault()
                        : 0;
                        coverage.RuleSetId = groupCoverages.First(x => x.CoverageId == coverage.Id).RuleSetId;
                        coverage.PosRuleSetId = groupCoverages.First(x => x.CoverageId == coverage.Id).PosRuleSetId;
                        coverage.IsVisible = coverDetailTypes.Exists(x => x.CoverageId == coverage.Id) ? false : true;
                        coverage.SubLineBusiness.Description = subLinesBusiness
                        .First(x => x.SubLineBusinessCode == coverage.SubLineBusiness.Id).Description;
                        coverage.SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == coverage.SubLineBusiness.LineBusiness.Id).Description;
                        coverage.Deductible = DeductibleDAO.GetDeductibleDefaultByCoverageId(coverage.Id);
                    });
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetAllyCoveragesByCoverageIdProductIdGroupCoverageId");

            return coverages;
        }

        /// <summary>
        /// Obtener las coberturas por endosos de una poliza
        /// </summary>
        /// <param name="policyId">Id póliza</param>
        /// <param name="riskId">Id riesgo</param>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetCoveragesByPolicyIdRiskIdCoverageId(int policyId, int riskId, int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            NameValue[] parameters = new NameValue[3];

            parameters[0] = new NameValue("POLICY_ID", policyId);
            parameters[1] = new NameValue("RISK_ID", riskId);
            parameters[2] = new NameValue("COVERAGE_ID", coverageId);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ISS.READ_COVERAGES_ENDORSEMENT", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                ConcurrentBag<Model.Coverage> coverages = new ConcurrentBag<Model.Coverage>();

                TASKU.Parallel.For(0, result.Rows.Count, arrayItem =>
                {
                    Model.Coverage coverage = new Model.Coverage();
                    coverage.RiskCoverageId = Convert.ToInt32(result.Rows[arrayItem][0]);
                    coverage.PremiumAmount = Convert.ToDecimal(result.Rows[arrayItem][1]);
                    if (result.Rows[arrayItem][2] != DBNull.Value)
                        coverage.Rate = Convert.ToDecimal(result.Rows[arrayItem][2]);
                    var rateTypeCode = Convert.ToInt32(result.Rows[arrayItem][3]);
                    coverage.RateType = (RateType)rateTypeCode;
                    coverage.CurrentFrom = Convert.ToDateTime(result.Rows[arrayItem][4]);
                    coverage.CurrentTo = Convert.ToDateTime(result.Rows[arrayItem][5]);
                    coverage.CoverStatus = (Enums.CoverageStatusType?)Convert.ToInt32(result.Rows[arrayItem][6]);
                    coverage.CalculationType = (CalculationType?)Convert.ToInt32(result.Rows[arrayItem][7]);
                    coverages.Add(coverage);
                });
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByPolicyIdRiskIdCoverageId");

                return coverages.ToList();
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByPolicyIdRiskIdCoverageId");

                return null;
            }
        }

        /// <summary>
        /// Obtener Coberturas por Objecto del Seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByInsuredObjectId(int insuredObjectId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.InsuredObjectId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(insuredObjectId);

            CoverageInsuredObjectView view = new CoverageInsuredObjectView();
            ViewBuilder builder = new ViewBuilder("CoverageInsuredObjectView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
            List<QUOEN.AllyCoverage> coveragesAllied = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByInsuredObjectId");

            return coverages.Where(x => !coveragesAllied.Any(y => y.AllyCoverageId == x.Id)).ToList();
        }

        /// <summary>
        /// Obtener Toda la información de las diferentes tablas relacionadas de coberturas
        /// </summary>
        /// <param name="coverageId">Id Cobertura</param>
        /// <returns>Coberturas</returns>
        public Model.Coverage GetTechnicalPlanRelatedEntitiesByCoverageId(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(coverageId);

            TechnicalPlanRelatedEntitiesView view = new TechnicalPlanRelatedEntitiesView();
            ViewBuilder builder = new ViewBuilder("TechnicalPlanRelatedEntitiesView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            Model.Coverage coverage = new Model.Coverage();
            if (view.CoverageList.Count > 0)
            {
                coverage = ModelAssembler.CreateCoverage((QUOEN.Coverage)view.CoverageList[0]);
                List<ENCO.SubLineBusiness> subLinesBusiness = view.SubLineBusinessList.Cast<ENCO.SubLineBusiness>().ToList();
                coverage.SubLineBusiness.Description = subLinesBusiness.First(x => x.SubLineBusinessCode == coverage.SubLineBusiness.Id).Description;
                coverage.SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == coverage.SubLineBusiness.LineBusiness.Id).Description;
                coverage.CoverageAllied = GetCoverageAlliedByCoverageId(coverage.Id);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetTechnicalPlanRelatedEntitiesByCoverageId");

            return coverage;
        }

        /// <summary>
        /// Busca la informacion de la cobertura asociada al producto y asigna coberturas aliadas
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Datos de la cobertura</returns>
        public Model.Coverage GetCoverageProductByCoverageId(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(coverageId);

            CoverageProductView view = new CoverageProductView();
            ViewBuilder builder = new ViewBuilder("CoverageProductView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Coverages.Count > 0)
            {
                List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
                List<ENCO.SubLineBusiness> subLinesBusiness = view.SubLinesBusiness.Cast<ENCO.SubLineBusiness>().ToList();

                foreach (Model.Coverage item in coverages)
                {
                    item.SubLineBusiness.Description = subLinesBusiness.First(x => x.SubLineBusinessCode == item.SubLineBusiness.Id).Description;
                    item.SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == item.SubLineBusiness.LineBusiness.Id).Description;
                    item.Deductible = DeductibleDAO.GetDeductibleDefaultByCoverageId(item.Id);
                    item.CoverageAllied = GetCoverageAlliedByCoverageId(item.Id);
                }

                return coverages.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Toda la información de las coberturas aliadas
        /// </summary>
        /// <param name="coverageId">Id Cobertura</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoverageAlliedByCoverageId(int coverageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.AllyCoverage.Properties.CoverageId, typeof(QUOEN.AllyCoverage).Name);
            filter.Equal();
            filter.Constant(coverageId);

            CoverageAlliedView view = new CoverageAlliedView();
            ViewBuilder builder = new ViewBuilder("CoverageAlliedView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
            List<QUOEN.AllyCoverage> coverageAllied = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
            foreach (Model.Coverage item in coverages)
            {
                item.SublimitPercentage = coverageAllied.First(x => x.AllyCoverageId == item.Id).CoveragePercentage;
            }
            return coverages;
        }

        /// <summary>
        /// Obtener Cobertura Por Id
        /// </summary>
        /// <param name="riskCoverageId">Id Cobertura</param>
        /// <returns>Cobertura</returns>
        public Model.Coverage GetCoverageByRiskCoverageId(int riskCoverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filterCoverage = new ObjectCriteriaBuilder();

            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskCoverId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(riskCoverageId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.EndorsementRiskCoverage), filterCoverage.GetPredicate()));

            ISSEN.EndorsementRiskCoverage endorsementRiskCoverage = businessCollection.Cast<ISSEN.EndorsementRiskCoverage>().FirstOrDefault();

            filterCoverage = new ObjectCriteriaBuilder();
            filterCoverage.Property(ISSEN.RiskCoverage.Properties.RiskCoverId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(riskCoverageId);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.RiskCoverage), filterCoverage.GetPredicate()));

            ISSEN.RiskCoverage riskCoverage = businessCollection.Cast<ISSEN.RiskCoverage>().FirstOrDefault();

            EndorsementDAO endorsementDAO = new EndorsementDAO();
            List<Model.Endorsement> endorsements = endorsementDAO.GetEffectiveEndorsementsByPolicyId(endorsementRiskCoverage.PolicyId);

            if (endorsements != null)
            {
                if (endorsements.Exists(x => x.EndorsementType == Enums.EndorsementType.EffectiveExtension))
                {
                    endorsements = endorsements.Where(x => x.Id >= endorsements.First(y => y.EndorsementType == Enums.EndorsementType.EffectiveExtension).Id).ToList();
                }
            }
            filterCoverage = new ObjectCriteriaBuilder();
            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.In();
            filterCoverage.ListValue();
            foreach (Model.Endorsement endorsement in endorsements)
            {
                filterCoverage.Constant(endorsement.Id);
            }
            filterCoverage.EndList();
            filterCoverage.And();
            filterCoverage.Property(ISSEN.RiskCoverage.Properties.CoverageId, typeof(ISSEN.RiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(riskCoverage.CoverageId);
            filterCoverage.And();
            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskNum, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(endorsementRiskCoverage.RiskNum);


            PolicyCoverageView view = new PolicyCoverageView();
            ViewBuilder builder = new ViewBuilder("PolicyCoverageView");
            builder.Filter = filterCoverage.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            List<Model.Coverage> coverages = new List<Model.Coverage>();
            List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();

            foreach (ISSEN.RiskCoverage item in riskCoverages)
            {
                dataFacade.LoadDynamicProperties(item);
                coverages.Add(ModelAssembler.CreatePolicyCoverage(item));
            }

            List<ISSEN.EndorsementRiskCoverage> endorsementCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
            List<QUOEN.Coverage> entityCoverages = view.Coverages.Cast<QUOEN.Coverage>().ToList();

            foreach (Model.Coverage item in coverages)
            {
                item.CoverageOriginalStatus = (Enums.CoverageStatusType)endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverStatusCode;
                item.Number = endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverNum;
                item.InsuredObject = new Model.InsuredObject
                {
                    Id = entityCoverages.First(x => x.CoverageId == item.Id).InsuredObjectId
                };
            }
            Model.Coverage coverage = coverages.FirstOrDefault();
            coverage.PremiumAmount = coverages.Sum(u => u.PremiumAmount);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoverageByRiskCoverageId");

            return coverage;
        }

        /// <summary>
        /// Obtener Coberturas por Riesgo
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <param name="endorsementId">Id Endoso</param>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filterCoverage = new ObjectCriteriaBuilder();

            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(policyId);

            if (endorsementId != 0)
            {
                filterCoverage.And();
                filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filterCoverage.Equal();
                filterCoverage.Constant(endorsementId);
            }

            filterCoverage.And();
            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(riskId);
            filterCoverage.And();
            filterCoverage.Not();
            filterCoverage.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filterCoverage.Equal();
            filterCoverage.Constant(Enums.CoverageStatusType.Excluded);


            PolicyCoverageView view = new PolicyCoverageView();
            ViewBuilder builder = new ViewBuilder("PolicyCoverageView");
            builder.Filter = filterCoverage.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            List<Model.Coverage> coverages = new List<Model.Coverage>();
            List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
            List<ISSEN.RiskCoverDeduct> riskCoverDeducts = view.RiskCoverDeducts.Cast<ISSEN.RiskCoverDeduct>().ToList();

            foreach (ISSEN.RiskCoverage item in riskCoverages)
            {
                dataFacade.LoadDynamicProperties(item);
                coverages.Add(ModelAssembler.CreatePolicyCoverage(item));
            }

            List<ISSEN.EndorsementRiskCoverage> endorsementCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
            List<QUOEN.Coverage> entityCoverages = view.Coverages.Cast<QUOEN.Coverage>().ToList();

            foreach (Model.Coverage item in coverages)
            {
                item.PrintDescription = view.Coverages.Cast<QUOEN.Coverage>().First(x => x.CoverageId == item.Id).PrintDescription;
                item.CoverageOriginalStatus = (Enums.CoverageStatusType)endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverStatusCode;
                item.Number = endorsementCoverages.First(x => x.RiskCoverId == item.RiskCoverageId).CoverNum;
                item.SubLineBusiness = new SubLineBusiness
                {
                    Id = entityCoverages.First(x => x.CoverageId == item.Id).SubLineBusinessCode,
                    LineBusinessId = entityCoverages.First(x => x.CoverageId == item.Id).LineBusinessCode,
                    LineBusiness = new LineBusiness
                    {
                        Id = entityCoverages.First(x => x.CoverageId == item.Id).LineBusinessCode
                    }
                };
                item.InsuredObject = new Model.InsuredObject
                {
                    Id = entityCoverages.First(x => x.CoverageId == item.Id).InsuredObjectId
                };

                if (riskCoverDeducts.Exists(x => x.RiskCoverId == item.RiskCoverageId))
                {
                    item.Deductible = ModelAssembler.CreateCoverageDeductible(riskCoverDeducts.First(x => x.RiskCoverId == item.RiskCoverageId));

                    PARAMEN.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                    item.Deductible.DeductibleUnit.Description = deductibleUnit.Description;

                    deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                    item.Deductible.MinDeductibleUnit.Description = deductibleUnit.Description;

                    ENCO.Currency currency = view.Currencies.Cast<ENCO.Currency>().ToList().FirstOrDefault();
                    item.Deductible.Description = item.Deductible.DeductValue.ToString() + " " + item.Deductible.DeductibleUnit.Description;

                    if (currency != null)
                    {
                        item.Deductible.Currency.Description = currency.Description;
                        item.Deductible.Description += "(" + item.Deductible.Currency.Description + ")";
                    }

                    item.Deductible.Description += " - " + item.Deductible.MinDeductValue.ToString() + " " + item.Deductible.MinDeductibleUnit.Description;
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByPolicyIdEndorsementIdRiskId");

            return coverages;
        }

        /// <summary>
        /// Obtener coberturas principales por objeto de seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas Principales
        /// </returns>
        public List<Model.Coverage> GetCoveragesPrincipalByInsuredObjectId(int insuredObjectId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.InsuredObjectId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(insuredObjectId);

            CoverageInsuredObjectView view = new CoverageInsuredObjectView();
            ViewBuilder builder = new ViewBuilder("CoverageInsuredObjectView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesPrincipalByInsuredObjectId");

            return coverages.Where(x => x.IsPrimary == true).ToList();
        }

        /// <summary>
        /// Obtener coberturas por plan tecnico
        /// </summary>
        /// <param name="insuredObjectId">Id Plan Tecnico</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByTechnicalPlanId(int technicalPlanId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.TechnicalPlanCoverage.Properties.TechnicalPlanId, typeof(PRODEN.TechnicalPlanCoverage).Name);
            filter.Equal();
            filter.Constant(technicalPlanId);

            CoverageTechnicalPlanView view = new CoverageTechnicalPlanView();
            ViewBuilder builder = new ViewBuilder("CoverageTechnicalPlanView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<Model.Coverage> coverages = ModelAssembler.CreateCoverages(view.Coverages);
            List<ENCO.LineBusiness> linesBusiness = view.LineBusiness.Cast<ENCO.LineBusiness>().ToList();
            List<ENCO.SubLineBusiness> subLinesBusiness = view.SubLineBusiness.Cast<ENCO.SubLineBusiness>().ToList();
            List<QUOEN.AllyCoverage> coverageAllied = view.CoverageAllied.Cast<QUOEN.AllyCoverage>().ToList();
            for (int i = 0; i < coverages.Count; i++)
            {
                coverages[i].SubLineBusiness.Description = subLinesBusiness.First(x => x.SubLineBusinessCode == coverages[i].SubLineBusiness.Id).Description;
                coverages[i].SubLineBusiness.LineBusiness.Description = subLinesBusiness.First(x => x.LineBusinessCode == coverages[i].SubLineBusiness.LineBusiness.Id).Description;
                if (coverageAllied.Count > 0)
                {
                    var itemcoverageAllied = coverageAllied.FirstOrDefault(x => x.AllyCoverageId == coverages[i].Id);

                    if (itemcoverageAllied != null)
                    {
                        coverages[i].SublimitPercentage = itemcoverageAllied.CoveragePercentage;
                    }

                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByTechnicalPlanId");

            return coverages.OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        ///Obtiene  LineBusiness
        /// </summary>
        /// <param name="coverageId">Identificador de la cobertura</param>       
        /// <returns></returns>
        public int GetLineBusinessByCoverageId(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(coverageId);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetLineBusinessByCoverageId");
            return ((QUOEN.Coverage)DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Coverage), filter.GetPredicate()).FirstOrDefault()).LineBusinessCode;

        }

        /// <summary>
        /// Obtener lista de Coberturas
        /// </summary>
        /// <param name="lineBusinessId">Identificador ramo</param>
        /// <param name="subLineBusinessId">Identificador Subramo</param>
        /// <param name="description">Descripcion cobertura.</param>
        /// <returns>List<Model.Coverage></returns>
        public List<Model.Coverage> GetCoveragesByLineBusinessIdBySubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            filter.And();
            filter.Property(QUOEN.Coverage.Properties.SubLineBusinessCode, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(subLineBusinessId);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCoveragesByLineBusinessIdBySubLineBusinessId");
            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Coverage), filter.GetPredicate());
            return ModelAssembler.CreateCoverages(businessCollection);
        }


        /// <summary>
        /// Obtiene las coberturas del endoso para calcular suma asegurada y el numero de riesgos
        /// </summary>
        /// <param filter="filter">filtro pa la consulta</param>
        /// <param summary="summary">Summary inicial</param>
        /// <param name="endorsementType"> Tipo de endoso</param>
        /// <returns>Summary con suma asegurada y numero de riesgos</returns>        
        public Models.Summary GetCoveragesEndorsementSummary(ObjectCriteriaBuilder filter, Models.Summary summary, Enums.EndorsementType? endorsementType)
        {
            EndorsementRiskCoverageView view = new EndorsementRiskCoverageView();
            ViewBuilder builder = new ViewBuilder("EndorsementRiskCoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            List<ISSEN.EndorsementRisk> endorRisk = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

            if (view.EndorsementRisks != null)
            {
                summary.RiskCount = endorRisk.Where(x => x.RiskStatusCode != Convert.ToInt16(Enums.RiskStatusType.Excluded)).Count();
                //summary.RiskCount = view.EndorsementRisks.Count;

                if (summary.RiskCount > 0)
                {
                    summary.PolicyId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault().PolicyId;

                    //var risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                    //summary.Risks = ModelAssembler.ConvertIssRiskToUnderwritingRisk(risks);
                    //foreach (var risk in summary.Risks)
                    //{
                    //    var insuredUniquePersonServicesModel = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(risk.MainInsured.IndividualId);
                    //    risk.MainInsured = ModelAssembler.CreateUnderitingInsuredObject(insuredUniquePersonServicesModel);
                    //}
                    List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                    if (endorsementType.HasValue)
                    {
                        if (endorsementType == Enums.EndorsementType.Modification)
                        {
                            summary.AmountInsured += (decimal)riskCoverages.Sum(x => x.EndorsementLimitAmount);
                        }
                        else
                        {
                            summary.AmountInsured += riskCoverages.Sum(x => x.LimitAmount);
                        }
                    }
                    else
                    {
                        summary.AmountInsured += riskCoverages.Sum(x => x.LimitAmount);
                    }
                }
                return summary;
            }
            else
            {
                throw new Exception("Error Obteniendo Resumen");
            }
        }

        /// <summary>
        /// Obtiene las coberturas del temporal para calcular suma asegurada y el numero de riesgos
        /// </summary>
        /// <param summary="summary">Summary inicial</param>  
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Summary con suma asegurada y numero de riesgos</returns>        
        public Models.Summary GetCoveragesTemporalSummary(Models.Summary summary, int temporalId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempRisk.Properties.TempId, typeof(TMPEN.TempRisk).Name);
            filter.Equal();
            filter.Constant(temporalId);
            TemporalRiskCoverageView view = new TemporalRiskCoverageView();
            ViewBuilder builder = new ViewBuilder("TemporalRiskCoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            summary.RiskCount = view.TempRisks.Count;
            if (summary.RiskCount > 0)
            {
                List<TMPEN.TempRiskCoverage> tempRiskCoverages = view.TempRiskCoverages.Cast<TMPEN.TempRiskCoverage>().ToList();
                summary.AmountInsured += tempRiskCoverages.Sum(x => x.LimitAmount);
            }
            return summary;
        }

        internal List<Model.Coverage> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            filter.And();
            filter.Property(QUOEN.Coverage.Properties.SubLineBusinessCode, typeof(PRODEN.GroupCoverage).Name);
            filter.Equal();
            filter.Constant(subLineBusinessId);

            return ModelAssembler.CreateCoverages(DataFacadeManager.GetObjects(typeof(QUOEN.Coverage), filter.GetPredicate()));
        }

        internal List<Model.Coverage> GetCoveragesByRiskId(int riskId)
        {
            List<Model.Coverage> coverages = new List<Model.Coverage>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(riskId);
            filter.And();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Distinct();
            filter.Constant(Enums.CoverageStatusType.Excluded);

            EndorsementRiskCoverageDeductView claimCoverageView = new EndorsementRiskCoverageDeductView();
            ViewBuilder viewBuilder = new ViewBuilder("EndorsementRiskCoverageDeductView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageView);

            if (claimCoverageView.EndorsementRiskCoverages.Count > 0)
            {
                List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = claimCoverageView.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                List<ISSEN.RiskCoverDeduct> entityRiskCoverDeducts = claimCoverageView.RiskCoverDeducts.Cast<ISSEN.RiskCoverDeduct>().ToList();
                List<ENCO.SubLineBusiness> entitySublineBusiness = claimCoverageView.SubLineBusiness.Cast<ENCO.SubLineBusiness>().ToList();
                List<QUOEN.InsuredObject> entityInsuredObjects = claimCoverageView.InsuredObjects.Cast<QUOEN.InsuredObject>().ToList();
                List<QUOEN.Coverage> entityCoverages = claimCoverageView.Coverages.Cast<QUOEN.Coverage>().ToList();

                coverages = ModelAssembler.CreatePolicyCoverages(claimCoverageView.RiskCoverages);

                foreach (Model.Coverage coverage in coverages)
                {
                    coverage.Number = endorsementRiskCoverages.First(x => x.RiskCoverId == coverage.RiskCoverageId).CoverNum;
                    coverage.CoverStatus = (Enums.CoverageStatusType)endorsementRiskCoverages.First(x => x.RiskCoverId == coverage.RiskCoverageId).CoverStatusCode;
                    coverage.Description = entityCoverages.First(x => x.CoverageId == coverage.Id).PrintDescription;

                    coverage.SubLineBusiness = ModelAssembler.CreateSubLineBusiness(entitySublineBusiness.First());
                    coverage.InsuredObject = ModelAssembler.CreateInsuredObject(entityInsuredObjects.First());

                    if (entityRiskCoverDeducts.Exists(x => x.RiskCoverId == coverage.RiskCoverageId))
                    {
                        coverage.Deductible = ModelAssembler.CreateCoverageDeductible(entityRiskCoverDeducts.First(x => x.RiskCoverId == coverage.RiskCoverageId));
                    }
                }
            }

            return coverages;
        }

        internal List<Model.Coverage> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime? occurrenceDate, decimal companyParticipationPercentage)
        {
            List<Model.Coverage> coverages = new List<Model.Coverage>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(riskId);
            filter.And();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.CoverStatusCode, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Distinct();
            filter.Constant(Enums.CoverageStatusType.Excluded);

            if (occurrenceDate != null)
            {
                //To Remember: Se suprime la hora de la fecha de ocurrencia para que pueda ser cubierta el casos donde la fecha de ocurrencia es igual al último día de vigencia de la cobertura
                occurrenceDate = Convert.ToDateTime(occurrenceDate).Date;

                filter.And();
                filter.OpenParenthesis();
                filter.Property(ISSEN.RiskCoverage.Properties.CurrentFrom, typeof(ISSEN.RiskCoverage).Name);
                filter.LessEqual();
                filter.Constant(occurrenceDate);
                filter.And();
                filter.Property(ISSEN.RiskCoverage.Properties.CurrentTo, typeof(ISSEN.RiskCoverage).Name);
                filter.GreaterEqual();
                filter.Constant(occurrenceDate);
                filter.CloseParenthesis();
            }

            EndorsementRiskCoverageDeductView claimCoverageView = new EndorsementRiskCoverageDeductView();
            ViewBuilder viewBuilder = new ViewBuilder("EndorsementRiskCoverageDeductView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageView);

            if (claimCoverageView.EndorsementRiskCoverages.Count > 0)
            {
                List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = claimCoverageView.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                List<ISSEN.RiskCoverDeduct> entityRiskCoverDeducts = claimCoverageView.RiskCoverDeducts.Cast<ISSEN.RiskCoverDeduct>().ToList();
                List<ENCO.SubLineBusiness> entitySublineBusiness = claimCoverageView.SubLineBusiness.Cast<ENCO.SubLineBusiness>().ToList();
                List<QUOEN.InsuredObject> entityInsuredObjects = claimCoverageView.InsuredObjects.Cast<QUOEN.InsuredObject>().ToList();
                List<QUOEN.Coverage> entityCoverages = claimCoverageView.Coverages.Cast<QUOEN.Coverage>().ToList();

                coverages = ModelAssembler.CreatePolicyCoverages(claimCoverageView.RiskCoverages);

                foreach (Model.Coverage coverage in coverages)
                {
                    coverage.Number = endorsementRiskCoverages.First(x => x.RiskCoverId == coverage.RiskCoverageId).CoverNum;
                    coverage.CoverStatus = (Enums.CoverageStatusType)endorsementRiskCoverages.First(x => x.RiskCoverId == coverage.RiskCoverageId).CoverStatusCode;
                    coverage.Description = entityCoverages.First(x => x.CoverageId == coverage.Id).PrintDescription;

                    #region CompanyParticipationPercentage
                    coverage.AccumulatedDeductAmount = coverage.AccumulatedDeductAmount * (companyParticipationPercentage / 100);
                    coverage.AccumulatedLimitAmount = coverage.AccumulatedLimitAmount * (companyParticipationPercentage / 100);
                    coverage.AccumulatedPremiumAmount = coverage.AccumulatedPremiumAmount * (companyParticipationPercentage / 100);
                    coverage.AccumulatedSubLimitAmount = coverage.AccumulatedSubLimitAmount * (companyParticipationPercentage / 100);
                    coverage.ContractAmountPercentage = coverage.ContractAmountPercentage * (companyParticipationPercentage / 100);
                    coverage.DeclaredAmount = coverage.DeclaredAmount * (companyParticipationPercentage / 100);
                    coverage.DiffMinPremiumAmount = coverage.DiffMinPremiumAmount * (companyParticipationPercentage / 100);
                    coverage.EndorsementLimitAmount = coverage.EndorsementLimitAmount * (companyParticipationPercentage / 100);
                    coverage.EndorsementSublimitAmount = coverage.EndorsementSublimitAmount * (companyParticipationPercentage / 100);
                    coverage.LimitAmount = coverage.LimitAmount * (companyParticipationPercentage / 100);
                    coverage.LimitClaimantAmount = coverage.LimitClaimantAmount * (companyParticipationPercentage / 100);
                    coverage.LimitOccurrenceAmount = coverage.LimitOccurrenceAmount * (companyParticipationPercentage / 100);
                    coverage.MaxLiabilityAmount = coverage.MaxLiabilityAmount * (companyParticipationPercentage / 100);
                    coverage.OriginalLimitAmount = coverage.OriginalLimitAmount * (companyParticipationPercentage / 100);
                    coverage.OriginalSubLimitAmount = coverage.OriginalSubLimitAmount * (companyParticipationPercentage / 100);
                    coverage.PremiumAmount = coverage.PremiumAmount * (companyParticipationPercentage / 100);
                    coverage.SubLimitAmount = coverage.SubLimitAmount * (companyParticipationPercentage / 100);
                    #endregion

                    coverage.SubLineBusiness = ModelAssembler.CreateSubLineBusiness(entitySublineBusiness.First());
                    coverage.InsuredObject = ModelAssembler.CreateInsuredObject(entityInsuredObjects.First());

                    if (entityRiskCoverDeducts.Exists(x => x.RiskCoverId == coverage.RiskCoverageId))
                    {
                        coverage.Deductible = ModelAssembler.CreateCoverageDeductible(entityRiskCoverDeducts.First(x => x.RiskCoverId == coverage.RiskCoverageId));
                    }
                }
            }

            return coverages;
        }

        internal List<Model.Coverage> GetCoveragesByLineBusinessId(int lineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            return ModelAssembler.CreateCoverages(DataFacadeManager.GetObjects(typeof(QUOEN.Coverage), filter.GetPredicate()));
        }
    }
}