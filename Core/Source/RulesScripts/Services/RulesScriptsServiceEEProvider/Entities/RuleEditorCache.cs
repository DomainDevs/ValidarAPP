using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using Sistran.Core.Framework.Data;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Globalization;
using Sistran.Core.Application.CommonService.Entities;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Framework.BAF.Application;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities
{
    /// <summary>
    /// Summary description for RuleEditorCache
    /// </summary>
    public static class RuleEditorCache
    {
        public const int BOOLEAN_LIST_CODE = 2;

        private static IDictionary _ruleFunctionList;

        public static Concept GetConcept(int entityId, int conceptId)
        {
            ConceptCache cache = (ConceptCache)HttpContext.Current.Cache.Get("RE_ConceptCache");
            if (cache == null)
            {
                cache = new ConceptCache(ApplicationSettings.RequestProcessor);
                HttpContext.Current.Cache.Add("RE_ConceptCache", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return cache[entityId, conceptId];
        }

        public static ListConcept GetListConcept(int entityId, int conceptId)
        {
            DictionaryCache cache = (DictionaryCache)HttpContext.Current.Cache.Get("RE_ListConcept");
            if (cache == null)
            {
                cache = new DictionaryCache(new KeyGenerator(RuleEditorCache.CreateConceptKey), new ValueRetriever(RuleEditorCache.FindListConcept));
                HttpContext.Current.Cache.Add("RE_ListConcept", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return (ListConcept)cache[new int[] { entityId, conceptId }];
        }

        public static Entity GetEntity(int entityId)
        {
            EntityCache cache = (EntityCache)HttpContext.Current.Cache.Get("RE_EntityCache");
            if (cache == null)
            {
                cache = new EntityCache(ApplicationSettings.RequestProcessor);
                HttpContext.Current.Cache.Add("RE_EntityCache", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return cache[entityId];
        }

        public static Package GetPackage(int packageId)
        {
            DictionaryCache cache = (DictionaryCache)HttpContext.Current.Cache.Get("RE_Package");
            if (cache == null)
            {
                cache = new DictionaryCache(new ValueRetriever(RuleEditorCache.FindPackage));
                HttpContext.Current.Cache.Add("RE_Package", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return (Package)cache[packageId];
        }

        private static object FindPackage(object key)
        {
            return Sistran.Core.Framework.UIF.BusinessObjectHelper.Find<Package>(Package.CreatePrimaryKey((int)key));
        }

        public static IList<Pair> GetConceptListValue(int entityId, int conceptId)
        {
            DictionaryCache cache = (DictionaryCache)HttpContext.Current.Cache["RE_ConceptListValue"];
            if (cache == null)
            {
                cache = new DictionaryCache(new KeyGenerator(RuleEditorCache.CreateConceptKey), new ValueRetriever(RuleEditorCache.ReadConceptList));
                HttpContext.Current.Cache.Add("RE_ConceptListValue", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(20), CacheItemPriority.Normal, null);
            }

            return (IList<Pair>)cache[new int[] { entityId, conceptId }];
        }

        public static IList<Pair> GetConceptRangeValue(int entityId, int conceptId)
        {
            DictionaryCache cache = (DictionaryCache)HttpContext.Current.Cache["RE_ConceptRangeValue"];
            if (cache == null)
            {
                cache = new DictionaryCache(new KeyGenerator(RuleEditorCache.CreateConceptKey), new ValueRetriever(RuleEditorCache.ReadConceptRange));
                HttpContext.Current.Cache.Add("RE_ConceptRangeValue", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(20), CacheItemPriority.Normal, null);
            }

            return (IList<Pair>)cache[new int[] { entityId, conceptId }];
        }

        private static object CreateConceptValueKey(object keys)
        {
            int[] values = (int[])keys;
            return string.Format("{0}|{1}|{2}", values[0], values[1], values[2]);
        }

        private static object ReadConceptList(object keys)
        {
            return ExpressionHelper.GetListEntityValuesFor(((int[])keys)[0], ((int[])keys)[1]);
        }

        private static object ReadConceptRange(object keys)
        {
            return ExpressionHelper.GetRangeEntityValuesFor(((int[])keys)[0], ((int[])keys)[1]);
        }

        private static object CreateConceptKey(object keys)
        {
            int[] values = (int[])keys;
            return string.Format("{0}|{1}", values[0], values[1]);
        }

        public static string GetSearchComboDescription(int entityId, int conceptId, int itemId)
        {
            return RuleEditorCache.GetSearchComboDescription(entityId, conceptId, itemId);
        }

        private static object ReadSearchComboDescription(object keys)
        {
            int[] values = (int[])keys;
            return ExpressionHelper.GetSearchComboDescription(values[0], values[1], values[2].ToString());
        }

        public static string GetConceptValueDescription(Concept c, string value)
        {
            DictionaryCache cache = (DictionaryCache)HttpContext.Current.Cache["RE_ConceptValueDesc"];
            if (cache == null)
            {
                cache = new DictionaryCache(
                    new KeyGenerator(RuleEditorCache.ConceptValueKeyGenerator),
                    new ValueRetriever(RuleEditorCache.ReadConceptValue));
                HttpContext.Current.Cache.Add("RE_ConceptValueDesc", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(20), CacheItemPriority.Normal, null);
            }

            return (string)cache[new object[] { c, value }];
        }

        private static object ConceptValueKeyGenerator(object key)
        {
            object[] objs = (object[])key;
            Concept c = (Concept)objs[0];
            string value = (string)objs[1];

            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}|{1}|{2}", c.EntityId, c.ConceptId, value);
        }

        private static object ReadConceptValue(object key)
        {
            object[] objs = (object[])key;
            Concept c = (Concept)objs[0];
            string value = (string)objs[1];

            ConceptControl.Types controlType = (ConceptControl.Types)c.ConceptControlCode;
            switch (controlType)
            {
                case ConceptControl.Types.NumberEditor:
                    return string.Format("{0}", decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
                case ConceptControl.Types.DateEditor:
                    return string.Format("{0}", DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture));

                case ConceptControl.Types.TextBox:
                    return "\"" + value + "\"";

                case ConceptControl.Types.ListBox:
                    int listItemId;

                    if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out listItemId))
                    {
                        bool bval;
                        if (!bool.TryParse(value, out bval))
                        {
                            throw new Exception("Valor de lista no reconocido: " + value);
                        }

                        listItemId = (bval ? 1 : 0);
                    }

                    ConceptType.Types ctype = (ConceptType.Types)c.ConceptTypeCode;
                    if (ctype == ConceptType.Types.List)
                    {
                        return RuleEditorCache.GetListItemDescription(c.EntityId, c.ConceptId, listItemId);
                    }

                    if (ctype == ConceptType.Types.Range)
                    {
                        return RuleEditorCache.GetRangeItemDescription(c.EntityId, c.ConceptId, listItemId);
                    }

                    throw new Exception("Tipo de concepto no soportado para lista.");

                case ConceptControl.Types.SearchCombo:
                    return ExpressionHelper.GetSearchComboDescription(c.EntityId, c.ConceptId, value);
            }

            return string.Empty;
        }

        public static string GetListItemDescription(int entityId, int conceptId, int itemId)
        {
            IList<Pair> list = RuleEditorCache.GetConceptListValue(entityId, conceptId);
            foreach (Pair p in list)
            {
                if (itemId == (int)p.First)
                {
                    return (string)p.Second;
                }
            }

            return string.Empty;
        }

        public static string GetRangeItemDescription(int entityId, int conceptId, int itemId)
        {
            IList<Pair> list = RuleEditorCache.GetConceptRangeValue(entityId, conceptId);
            foreach (Pair p in list)
            {
                if (itemId == (int)p.First)
                {
                    return (string)p.Second;
                }
            }

            return string.Empty;
        }

        private static object FindListConcept(object key)
        { 
            int[] keys = (int[])key;

            return BusinessObjectHelper.Find<ListConcept>(ListConcept.CreatePrimaryKey(keys[0], keys[1]));
        }

        public static System.CodeDom.CodeTypeReference GetConceptTypeReference(int entityId, int conceptId)
        {
            Concept c = GetConcept(entityId, conceptId);
            bool isNullable = (c.IsNullable || !c.IsStatic);

            switch ((ConceptType.Types)c.ConceptTypeCode)
            {
                case ConceptType.Types.List:
                    ListConcept lc = GetListConcept(entityId, conceptId);
                    if (lc == null)
                    {
                        throw new Exception(string.Format("No se encontro la lista para el concepto: {0}|{1}.", entityId, conceptId));
                    }

                    if (lc.ListEntityCode == BOOLEAN_LIST_CODE)
                    {
                        if (isNullable)
                        {
                            return new System.CodeDom.CodeTypeReference("bool?");
                        }
                        return new System.CodeDom.CodeTypeReference(typeof(bool));
                    }

                    goto case ConceptType.Types.Range;

                case ConceptType.Types.Range:
                case ConceptType.Types.Reference:
                    if (isNullable)
                    {
                        return new System.CodeDom.CodeTypeReference("int?");
                    }

                    return new System.CodeDom.CodeTypeReference(typeof(int));

                case ConceptType.Types.Basic:
                    BasicConcept bc = GetBasicConcept(entityId, conceptId);
                    switch((BasicType.Types)bc.BasicTypeCode)
                    {
                        case BasicType.Types.Date:
                            if (isNullable)
                            {
                                return new System.CodeDom.CodeTypeReference("DateTime?");
                            }

                            return new System.CodeDom.CodeTypeReference(typeof(DateTime));

                        case BasicType.Types.Decimal:
                            if (isNullable)
                            {
                                return new System.CodeDom.CodeTypeReference("decimal?");
                            }

                            return new System.CodeDom.CodeTypeReference(typeof(decimal));

                        case BasicType.Types.Number:
                            if (isNullable)
                            {
                                return new System.CodeDom.CodeTypeReference("int?");
                            }

                            return new System.CodeDom.CodeTypeReference(typeof(int));

                        case BasicType.Types.Text:
                            return new System.CodeDom.CodeTypeReference(typeof(string));

                     
                        default:
                            throw new Exception("Basic type not supported: " + bc.BasicTypeCode.ToString());
                    }

                default:
                    throw new Exception("Concept type not supported: " + c.ConceptTypeCode.ToString());
            }
        }

        public static BasicConcept GetBasicConcept(int entityId, int conceptId)
        {
            DictionaryCache cache = (DictionaryCache)HttpContext.Current.Cache.Get("RE_BasicConcept");
            if (cache == null)
            {
                cache = new DictionaryCache(new KeyGenerator(RuleEditorCache.CreateConceptKey), new ValueRetriever(RuleEditorCache.FindBasicConcept));
                HttpContext.Current.Cache.Add("RE_BasicConcept", cache, null, DateTime.MaxValue, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return (BasicConcept)cache[new int[] { entityId, conceptId }];
        }

        private static object FindBasicConcept(object key)
        {
            int[] keys = (int[])key;
            return BusinessObjectHelper.Find<BasicConcept>(BasicConcept.CreatePrimaryKey(keys[0], keys[1]));
        }

        public  static IDictionary ListRuleFunction()
        {
            if (_ruleFunctionList == null)
            {
                _ruleFunctionList = new SortedList();

                ListRuleFunctionActionRequest req = new ListRuleFunctionActionRequest();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.PropertyEquals(RuleFunction.Properties.PackageId, 1);
                    // packageid 1 = sistran.core.application.quotations

                req.Filter = filter.GetPredicate();
                req.Sort = null;

                ListRuleFunctionActionResponse resp =
                    (ListRuleFunctionActionResponse)
                    ApplicationSettings.RequestProcessor.Process(req);
                
                foreach (RuleFunction ruleFunction in resp.RuleFunctionList)
                {
                    _ruleFunctionList.Add(ruleFunction.FunctionName, ruleFunction.Description);
                }
            }


            return _ruleFunctionList;
        }


    }
}