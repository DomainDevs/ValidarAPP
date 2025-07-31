using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Viewss;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using EntitiesCommon = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class DeductibleDAO
    {
        /// <summary>
        /// Finds the specified deduct identifier.
        /// </summary>
        /// <param name="deductId">The deduct identifier.</param>
        /// <returns></returns>
        public static QUOEN.Deductible GetDeductibleByDeductId(int deductId)
        {
            PrimaryKey key = QUOEN.Deductible.CreatePrimaryKey(deductId);
            return (QUOEN.Deductible)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList GetDeductiblesByFilterSort(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Deductible), filter, sort);
        }

        /// <summary>
        /// Obtene el Deducible especifico relacionado con la covertura
        /// </summary>
        /// <param name="coverageId">Idenbtificador de covertura</param>
        /// <param name="deductibleId">Identificador de Deductible</param>
        /// <returns>
        /// Deducible asignado
        /// </returns>
        public static Deductible GetDeductibleDefaultByCoverageId(int coverageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.CoverageDeductible.Properties.CoverageId, typeof(QUOEN.CoverageDeductible).Name);
            filter.Equal();
            filter.Constant(coverageId);
            filter.And();
            filter.Property(QUOEN.CoverageDeductible.Properties.IsDefault, typeof(QUOEN.CoverageDeductible).Name);
            filter.Equal();
            filter.Constant(1);

            CoverageDeductibleView view = new CoverageDeductibleView();
            ViewBuilder builder = new ViewBuilder("CoverageDeductibleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Deductibles.Count > 0)
            {
                Deductible deductible = ModelAssembler.CreateDeductible(view.Deductibles.Cast<QUOEN.Deductible>().FirstOrDefault());

                PARAMEN.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                deductible.DeductibleUnit.Description = deductibleUnit.Description;

                deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                if (deductibleUnit != null && deductible.MinDeductibleUnit != null)
                {
                    deductible.MinDeductibleUnit.Description = deductibleUnit.Description;
                }
                PARAMEN.DeductibleSubject deductibleSubject = view.DeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList().FirstOrDefault();
                if (deductibleSubject != null && deductible.DeductibleSubject != null)
                {
                    deductible.DeductibleSubject.Description = deductibleSubject.Description;
                }
                else
                {
                    deductible.DeductibleSubject = null;
                }
                deductibleSubject = view.MinimumDeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList().FirstOrDefault();
                if (deductibleSubject != null && deductible.MinDeductibleSubject != null)
                {
                    deductible.MinDeductibleSubject.Description = deductibleSubject.Description;
                }
                deductible.Description = deductible.DeductValue.ToString() + " " + deductible.DeductibleUnit.Description;
                EntitiesCommon.Currency currency = view.Currencies.Cast<EntitiesCommon.Currency>().ToList().FirstOrDefault();
                if (currency != null && deductible.Currency != null)
                {
                    deductible.Currency.Description = currency.Description;
                    deductible.Description += "(" + deductible.Currency.Description + ")";
                }
                deductible.Description += " - " + deductible.MinDeductValue == null ? "" : deductible.MinDeductValue.ToString() + " " + deductible.MinDeductibleUnit == null ? "" : deductible.MinDeductibleUnit.Description;

                return deductible;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtene los deducibles de una cobertura si lo ubiese
        /// </summary>
        /// <param name="coverageId">Idenbtificador de cobertura</param>
        /// <returns>Deducible por defecto para covertura</returns>
        public List<Deductible> GetDeductiblesByCoverageId(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.CoverageDeductible.Properties.CoverageId, typeof(QUOEN.CoverageDeductible).Name);
            filter.Equal();
            filter.Constant(coverageId);

            CoverageDeductibleView view = new CoverageDeductibleView();
            ViewBuilder builder = new ViewBuilder("CoverageDeductibleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Deductibles != null)
            {
                List<Deductible> deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

                foreach (Deductible item in deductibles)
                {
                    item.DeductibleUnit.Description = (view.DeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == item.DeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;

                    if (item.MinDeductibleUnit?.Id > 0)
                    {
                        item.MinDeductibleUnit.Description = (view.MinimumDeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == item.MinDeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;
                    }
                    if (item.DeductibleSubject?.Id > 0)
                    {

                        item.DeductibleSubject.Description = (view.DeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == item.DeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                    }
                    else
                    {
                        item.DeductibleSubject = null;
                    }
                    if (item.MinDeductibleSubject?.Id > 0)
                    {
                        item.MinDeductibleSubject.Description = (view.MinimumDeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == item.MinDeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                    }
                    item.IsDefault = (view.CoverageDeductibles.FirstOrDefault(x => ((QUOEN.CoverageDeductible)x).DeductId == item.Id) as QUOEN.CoverageDeductible)?.IsDefault ?? false;
                    if (item.Currency != null && view.Currencies.Count > 0)
                    {
                        EntitiesCommon.Currency currency = (EntitiesCommon.Currency)view.Currencies.First(x => ((EntitiesCommon.Currency)x).CurrencyCode == item.Currency.Id);
                        item.Description = item.DeductValue.ToString() + " " + item.DeductibleUnit.Description;
                        if (currency != null)
                        {
                            item.Currency.Description = currency.Description;
                            item.Description += "(" + item.Currency.Description + ")";
                        }
                    }
                    item.Description += " - " + item.MinDeductValue.ToString() + " " + item.MinDeductibleUnit?.Description;

                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetDeductiblesByCoverageId");
                return deductibles;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetDeductiblesByCoverageId");
                return null;
            }
        }

        /// <summary>
        /// Obtiene el deducible por Id
        /// </summary>
        /// <param name="deductibleId">Identificador del deducible</param>
        /// <returns>Model de deducible</returns>
        public static Deductible GetDeductibleByCoverageIdDeductibleId(int coverageId, int deductibleId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.CoverageDeductible.Properties.CoverageId, typeof(QUOEN.CoverageDeductible).Name);
            filter.Equal();
            filter.Constant(coverageId);
            filter.And();
            filter.Property(QUOEN.CoverageDeductible.Properties.DeductId, typeof(QUOEN.CoverageDeductible).Name);
            filter.Equal();
            filter.Constant(deductibleId);

            CoverageDeductibleView view = new CoverageDeductibleView();
            ViewBuilder builder = new ViewBuilder("CoverageDeductibleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Deductibles != null && view.Deductibles.Capacity > 0)
            {
                Deductible deductible = ModelAssembler.CreateDeductible(view.Deductibles.Cast<QUOEN.Deductible>().FirstOrDefault());

                PARAMEN.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                if (deductibleUnit != null)
                {
                    deductible.DeductibleUnit.Description = deductibleUnit.Description;
                }

                deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                if (deductibleUnit != null)
                {
                    deductible.MinDeductibleUnit.Description = deductibleUnit.Description;
                }
                PARAMEN.DeductibleSubject deductibleSubject = view.DeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList().FirstOrDefault();
                if (deductibleSubject != null)
                {
                    deductible.DeductibleSubject.Description = deductibleSubject.Description;
                }
                else
                {
                    deductible.DeductibleSubject = null;
                }
                deductibleSubject = view.MinimumDeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList().FirstOrDefault();
                if (deductibleSubject != null)
                {
                    deductible.MinDeductibleSubject.Description = deductibleSubject.Description;
                }
                EntitiesCommon.Currency currency = view.Currencies.Cast<EntitiesCommon.Currency>().ToList().FirstOrDefault();
                deductible.Description = deductible.DeductValue.ToString() + " " + deductible.DeductibleUnit.Description;
                if (currency != null && deductible.Currency != null)
                {
                    deductible.Currency.Description = currency.Description;
                    deductible.Description += "(" + deductible.Currency.Description + ")";
                }
                deductible.Description += " - " + deductible.MinDeductValue == null ? "" : deductible.MinDeductValue.ToString() + " " + deductible.MinDeductibleUnit == null ? "" : deductible.MinDeductibleUnit.Description;

                return deductible;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el deducible por Ramo tecnico
        /// </summary>
        /// <param name="prefixCd">Codigo Ramo Comercial</param>
        /// <returns>Model de deducible</returns>
        public List<Deductible> GetDeductiblesByPrefixId(int prefixCd)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EntitiesCommon.PrefixLineBusiness.Properties.PrefixCode, typeof(EntitiesCommon.PrefixLineBusiness).Name);
            filter.Equal();
            filter.Constant(prefixCd);

            PrefixLineBusinessDeductiblesView view = new PrefixLineBusinessDeductiblesView();
            ViewBuilder builder = new ViewBuilder("PrefixLineBusinessDeductiblesView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Deductibles != null)
            {
                List<Deductible> deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

                foreach (Deductible item in deductibles)
                {
                    PARAMEN.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                    item.DeductibleUnit.Description = deductibleUnit.Description;

                    deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                    item.MinDeductibleUnit.Description = deductibleUnit.Description;

                    PARAMEN.DeductibleSubject deductibleSubject = view.DeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList().FirstOrDefault();
                    item.DeductibleSubject.Description = deductibleSubject.Description;

                    deductibleSubject = view.MinimumDeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList().FirstOrDefault();
                    item.MinDeductibleSubject.Description = deductibleSubject.Description;

                    EntitiesCommon.Currency currency = view.Currencies.Cast<EntitiesCommon.Currency>().ToList().FirstOrDefault();
                    item.Description = item.DeductValue.ToString() + " " + item.DeductibleUnit.Description;
                    if (currency != null && item.Currency != null)
                    {
                        item.Currency.Description = currency.Description;
                        item.Description += "(" + item.Currency.Description + ")";
                    }
                    item.Description += " - " + item.MinDeductValue.ToString() + " " + item.MinDeductibleUnit.Description;
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetDeductiblesByPrefixId");

                return deductibles;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetDeductiblesByPrefixId");

                return null;
            }
        }

        /// <summary>
        /// Obtiene el deducible por Ramo tecnico
        /// </summary>
        /// <param name="lineBusinessCd">Codigo Ramo tecnico</param>
        /// <returns>Model de deducible</returns>
        public List<Deductible> GetDeductiblesByLineBusinessId(int lineBusinessCd)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Deductible.Properties.LineBusinessCode, typeof(QUOEN.Deductible).Name);
            filter.Equal();
            filter.Constant(lineBusinessCd);

            DeductibleView view = new DeductibleView();
            ViewBuilder builder = new ViewBuilder("DeductibleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            var deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

            foreach (var deductible in deductibles)
            {
                deductible.DeductibleUnit.Description = (view.DeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.DeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;
                deductible.MinDeductibleUnit.Description = (view.MinimumDeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.MinDeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;
                deductible.DeductibleSubject.Description = (view.DeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.DeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                deductible.MinDeductibleSubject.Description = (view.MinimumDeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.MinDeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;

                EntitiesCommon.Currency currency = (EntitiesCommon.Currency)view.Currencies.First(x => ((EntitiesCommon.Currency)x).CurrencyCode == deductible.Currency.Id);
                deductible.Description = deductible.DeductValue.ToString() + " " + deductible.DeductibleUnit.Description;
                if (currency != null && deductible.Currency != null)
                {
                    deductible.Currency.Description = currency.Description;
                    deductible.Description += "(" + deductible.Currency.Description + ")";
                }
                deductible.Description += " - " + deductible.MinDeductValue.ToString() + " " + deductible.MinDeductibleUnit.Description;
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), GetType().FullName);
            return deductibles;
        }

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Model de deducible</returns>
        public List<Deductible> GetDeductiblesAll()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            DeductibleView view = new DeductibleView();
            ViewBuilder builder = new ViewBuilder("DeductibleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            var deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

            foreach (Deductible deductible in deductibles)
            {
                if (deductible.DeductibleUnit != null && view.DeductibleUnits.Exists(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.DeductibleUnit.Id))
                {
                    deductible.DeductibleUnit.Description = (view.DeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.DeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;
                }
                if (deductible.MinDeductibleUnit != null && view.MinimumDeductibleUnits.Exists(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.MinDeductibleUnit.Id))
                {
                    deductible.MinDeductibleUnit.Description = (view.MinimumDeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.MinDeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;
                }
                if (deductible.DeductibleSubject != null && view.DeductibleSubjects.Exists(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.DeductibleSubject.Id))
                {
                    deductible.DeductibleSubject.Description = (view.DeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.DeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                }
                else
                {
                    deductible.DeductibleSubject = null;
                }
                if (deductible.MinDeductibleSubject != null && view.MinimumDeductibleSubjects.Exists(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.MinDeductibleSubject.Id))
                {
                    deductible.MinDeductibleSubject.Description = (view.MinimumDeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.MinDeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                }

                deductible.Description = deductible.DeductValue.ToString() + " " + deductible.DeductibleUnit.Description;

                if (deductible.Currency != null)
                {
                    EntitiesCommon.Currency currency = (EntitiesCommon.Currency)view.Currencies.First(x => ((EntitiesCommon.Currency)x).CurrencyCode == deductible.Currency.Id);

                    if (currency != null && deductible.Currency != null)
                    {
                        deductible.Currency.Description = currency.Description;
                        deductible.Description += "(" + deductible.Currency.Description + ")";
                    }
                }

                deductible.Description += " - " + deductible.MinDeductValue.ToString() + (deductible.MinDeductibleUnit != null ? (" " + deductible.MinDeductibleUnit.Description) : "");
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), GetType().FullName);
            return deductibles;
        }

        /// <summary>
        /// Obtener deducibles de las coberturas
        /// </summary>
        /// <param name="coverages">Lista de Coberturas</param>
        /// <returns>Coberturas</returns>
        public static List<Coverage> GetDeductiblesByCoverages(List<Coverage> coverages)
        {
            if (coverages == null || !coverages.Any() || coverages.Where(x => x.Deductible != null && x.Deductible.Id != -1)?.Count() < 1)
            {
                return coverages;
            }
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.CoverageDeductible.Properties.CoverageId, typeof(QUOEN.CoverageDeductible).Name);
            filter.In();
            filter.ListValue();
            foreach (Coverage item in coverages.Distinct().ToList())
            {
                filter.Constant(item.Id);
            }
            filter.EndList();
            filter.And();
            filter.Property(QUOEN.CoverageDeductible.Properties.DeductId, typeof(QUOEN.CoverageDeductible).Name);
            filter.In();
            filter.ListValue();
            List<int> deductiblesFilter = coverages.Where(z => z != null && z.Deductible != null && z.Deductible.Id != -1).Select(x => x.Deductible.Id).Distinct().ToList();
            foreach (int item in deductiblesFilter)
            {
                filter.Constant(item);
            }
            filter.EndList();


            CoverageDeductibleView view = new CoverageDeductibleView();
            ViewBuilder builder = new ViewBuilder("CoverageDeductibleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
            filter2.OpenParenthesis();
            filter2.Property(QUOEN.CoverageDeductible.Properties.CoverageId, typeof(QUOEN.CoverageDeductible).Name);
            filter2.In();
            filter2.ListValue();
            filter2.Constant(0);
            foreach (var item in coverages.Distinct().Select(x => new { x.Id, x.Deductible }).ToList())
            {
                if (!(item.Deductible != null && item.Deductible.Id != -1))
                    filter2.Constant(item.Id);
            }

            filter2.EndList();
            filter2.And();
            filter2.Property(QUOEN.CoverageDeductible.Properties.IsDefault, typeof(QUOEN.CoverageDeductible).Name);
            filter2.Equal();
            filter2.Constant(true);
            filter2.CloseParenthesis();

            CoverageDeductibleView view2 = new CoverageDeductibleView();
            ViewBuilder builder2 = new ViewBuilder("CoverageDeductibleView");
            builder2.Filter = filter2.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder2, view2);
            }

            if ((view.Deductibles != null && view.Deductibles.Capacity > 0) || (view2.Deductibles != null && view2.Deductibles.Capacity > 0))
            {
                List<QUOEN.Deductible> deductibles = view.Deductibles.Cast<QUOEN.Deductible>().ToList();
                List<QUOEN.CoverageDeductible> coverageDeductibles = view.CoverageDeductibles.Cast<QUOEN.CoverageDeductible>().ToList();
                List<PARAMEN.DeductibleUnit> deductibleUnits = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList();
                List<PARAMEN.DeductibleUnit> minimumDeductibleUnits = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList();
                List<PARAMEN.DeductibleSubject> deductibleSubjects = view.DeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList();
                List<PARAMEN.DeductibleSubject> minimumDeductibleSubjects = view.MinimumDeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList();
                List<EntitiesCommon.Currency> currencies = view.Currencies.Cast<EntitiesCommon.Currency>().ToList();

                deductibles.AddRange(view2.Deductibles.Cast<QUOEN.Deductible>().ToList());
                coverageDeductibles.AddRange(view2.CoverageDeductibles.Cast<QUOEN.CoverageDeductible>().ToList());
                deductibleUnits.AddRange(view2.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList());
                minimumDeductibleUnits.AddRange(view2.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList());
                deductibleSubjects.AddRange(view2.DeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList());
                minimumDeductibleSubjects.AddRange(view2.MinimumDeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().ToList());
                currencies.AddRange(view2.Currencies.Cast<EntitiesCommon.Currency>().ToList());


                foreach (Coverage item in coverages.Where(z => z != null).ToList())
                {
                    if (item.Deductible == null && coverageDeductibles.Exists(x => x.CoverageId == item.Id && x.IsDefault))
                    {
                        item.Deductible = new Deductible
                        {
                            Id = coverageDeductibles.First(y => y.CoverageId == item.Id && y.IsDefault).DeductId
                        };
                    }

                    if (item.Deductible != null && item.Deductible.Id != -1 && deductibles.Exists(x => x.DeductId == item.Deductible.Id))
                    {
                        item.Deductible = ModelAssembler.CreateDeductible(deductibles.First(x => x.DeductId == item.Deductible.Id));
                        item.Deductible.Description = item.Deductible.DeductValue.ToString();

                        if (deductibleUnits != null && item.Deductible.DeductibleUnit.Id > -1)
                        {
                            item.Deductible.DeductibleUnit.Description = deductibleUnits.First(x => x.DeductUnitCode == item.Deductible.DeductibleUnit.Id).Description;
                            item.Deductible.Description += " " + item.Deductible.DeductibleUnit.Description;
                        }

                        if (currencies != null && currencies.Count != 0 && item.Deductible.Currency != null)
                        {
                            item.Deductible.Currency.Description = currencies.First(x => x.CurrencyCode == item.Deductible.Currency.Id).Description;
                            item.Deductible.Description += " (" + item.Deductible.Currency.Description + ")";
                        }

                        if (minimumDeductibleUnits != null && minimumDeductibleUnits.Count != 0 && item.Deductible.MinDeductibleUnit?.Id > -1)
                        {
                            string description = minimumDeductibleUnits.FirstOrDefault(x => x.DeductUnitCode == item.Deductible.MinDeductibleUnit.Id)?.Description;
                            if (description != null && description != string.Empty)
                            {
                                item.Deductible.MinDeductibleUnit.Description = description;
                                item.Deductible.Description += " - " + item.Deductible.MinDeductValue.ToString() + " " + item.Deductible.MinDeductibleUnit.Description;
                            }
                        }
                        if (deductibleSubjects != null && deductibleSubjects.Any() && item.Deductible.DeductibleSubject.Id > -1)
                        {
                            string description = deductibleSubjects.FirstOrDefault(x => x.DeductSubjectCode == item.Deductible.DeductibleSubject.Id)?.Description;
                            if (description != null && description != string.Empty)
                            {
                                item.Deductible.DeductibleSubject.Description = description;
                            }
                            else
                            {
                                item.Deductible.DeductibleSubject = null;
                            }
                            if (minimumDeductibleSubjects != null && minimumDeductibleSubjects.Count != 0 && item.Deductible.MinDeductibleSubject.Id > -1)
                            {
                                string minDescription = minimumDeductibleSubjects.FirstOrDefault(x => x.DeductSubjectCode == item.Deductible.MinDeductibleSubject.Id)?.Description;
                                if (minDescription != null && minDescription != string.Empty)
                                {
                                    item.Deductible.MinDeductibleSubject.Description = minDescription;
                                }
                            }
                        }
                        else
                        {
                            item.Deductible.DeductibleSubject = null;
                        }
                    }
                    else if (item.Deductible != null && item.Deductible.Id == -1)
                    {
                        //No haga nada, deje el deducible tal cual lo dejo el usuario
                    }
                    else
                    {
                        item.Deductible = null;
                    }
                }
            }

            return coverages;
        }

        #region Claims

        public List<Deductible> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum)
        {
            List<Deductible> deductibles = new List<Deductible>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name, policyId);
            filter.And();
            filter.PropertyEquals(ISSEN.EndorsementRiskCoverage.Properties.RiskNum, typeof(ISSEN.EndorsementRiskCoverage).Name, riskNum);
            filter.And();
            filter.PropertyEquals(ISSEN.RiskCoverage.Properties.CoverageId, typeof(ISSEN.RiskCoverage).Name, coverageId);
            filter.And();
            filter.PropertyEquals(ISSEN.EndorsementRiskCoverage.Properties.CoverNum, typeof(ISSEN.EndorsementRiskCoverage).Name, coverNum);

            ClaimDeductibleView view = new ClaimDeductibleView();
            ViewBuilder builder = new ViewBuilder("ClaimDeductibleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.RiskCoverDeducts != null)
            {
                deductibles = ModelAssembler.CreateDeductiblesByRiskCoverage(view.RiskCoverDeducts);

                foreach (Deductible deductible in deductibles)
                {
                    deductible.DeductibleUnit.Description = (view.DeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.DeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;

                    if (Convert.ToInt32(deductible.MinDeductibleUnit?.Id) > 0)
                    {
                        deductible.MinDeductibleUnit.Description = (view.MinimumDeductibleUnits.First(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.MinDeductibleUnit.Id) as PARAMEN.DeductibleUnit).Description;
                    }
                    if (Convert.ToInt32(deductible.DeductibleSubject?.Id) > 0)
                    {
                        deductible.DeductibleSubject.Description = (view.DeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.DeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                    }
                    if (Convert.ToInt32(deductible.MinDeductibleSubject?.Id) > 0)
                    {
                        deductible.MinDeductibleSubject.Description = (view.MinimumDeductibleSubjects.First(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.MinDeductibleSubject.Id) as PARAMEN.DeductibleSubject).Description;
                    }
                    else
                    {
                        deductible.MinDeductibleSubject = null;
                    }
                    
                    if (view.Currencies.Count > 0)
                    {
                        if (deductible.Currency != null)
                        {
                            COMMEN.Currency entityCurrency = (COMMEN.Currency)view.Currencies.First(x => ((COMMEN.Currency)x).CurrencyCode == deductible.Currency.Id);

                            deductible.Description = deductible.DeductValue.ToString() + " " + deductible.DeductibleUnit?.Description;
                            if (entityCurrency != null)
                            {
                                deductible.Currency.Description = entityCurrency.Description;
                                deductible.Description += " (" + deductible.Currency.Description + ")";
                            }
                           
                            deductible.Description += " " + deductible.DeductibleSubject?.Description +", " + Resources.Errors.Minimum;                                                                                                                                       

                        }
                    }

                    deductible.Description += " " + deductible.MinDeductValue.ToString() + " " + deductible.MinDeductibleUnit?.Description;
                    deductible.Description += " " + deductible.MinDeductibleSubject?.Description; 
                }                                
            }

            return deductibles;
        }

        #endregion
    }
}
