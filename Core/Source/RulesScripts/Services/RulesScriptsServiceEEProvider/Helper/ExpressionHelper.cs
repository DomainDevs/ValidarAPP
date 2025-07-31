using Sistran.Co.Application.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// Summary description for ExpressionHelper.
    /// </summary>
    public sealed class ExpressionHelper
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="fp"></param>
        /// <param name="exp"></param>
		public static void GetAdvancedValueString(StringBuilder sb, IFormatProvider fp, CodeExpression exp)
		{
			if(exp is CodeBinaryOperatorExpression)
			{
				GetAdvancedValueString(sb, fp, (CodeBinaryOperatorExpression)exp);
			}
			else if(exp is CodePrimitiveExpression)
			{
				GetAdvancedValueString(sb, fp, (CodePrimitiveExpression)exp);
			}
			else if(exp is CodeConceptExpression)
			{
				GetAdvancedValueString(sb, fp, (CodeConceptExpression)exp);
			}
			else if(exp is CodeSnippetExpression)
			{
				GetAdvancedValueString(sb, fp, (CodeSnippetExpression)exp);
			}
			else
			{
				throw new ApplicationException("Type not supported: " + exp.GetType().FullName);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="fp"></param>
        /// <param name="exp"></param>
		public static void GetAdvancedValueString(StringBuilder sb, IFormatProvider fp, CodeSnippetExpression exp)
		{
			sb.Append(exp.Value.ToString(fp));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="fp"></param>
        /// <param name="exp"></param>
		public static void GetAdvancedValueString(StringBuilder sb, IFormatProvider fp, CodePrimitiveExpression exp)
		{
			sb.Append(((decimal)exp.Value).ToString(fp));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="fp"></param>
        /// <param name="exp"></param>
		public static void GetAdvancedValueString(StringBuilder sb, IFormatProvider fp, CodeConceptExpression exp)
		{
			sb.Append("[");
			PrimaryKey pk = exp.ConceptKey;

			string desc = GetConceptDescriptionByKey((int)pk["EntityId"], (int)pk["ConceptId"]);
			if(desc == null)
			{
				throw new BusinessException("MSGERR_CONCEPT_NOT_FOUND");
			}

			sb.Append(desc);
			sb.Append("]");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="fp"></param>
        /// <param name="exp"></param>
		public static void GetAdvancedValueString(StringBuilder sb, IFormatProvider fp, CodeBinaryOperatorExpression exp)
		{
			sb.Append("(");
			GetAdvancedValueString(sb, fp, exp.Left);
			sb.Append(" ");

			switch(exp.Operator)
			{
				case CodeBinaryOperatorType.Add:
					sb.Append("+");
					break;
				case CodeBinaryOperatorType.Divide:
					sb.Append("/");
					break;
				case CodeBinaryOperatorType.Multiply:
					sb.Append("*");
					break;
				case CodeBinaryOperatorType.Subtract:
					sb.Append("-");
					break;

				default:
					throw new ApplicationException("Operator type not supported: " + exp.Operator.ToString());
			}

			sb.Append(" ");
			GetAdvancedValueString(sb, fp, exp.Right);
			sb.Append(")");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
		public static CodeExpression CreateAdvancedExpression(ExpressionParser.Expression exp)
		{
			if(exp is ExpressionParser.Number)
			{
				return CreateAdvancedExpression((ExpressionParser.Number)exp);
			}

			if(exp is ExpressionParser.Variable)
			{
				return CreateAdvancedExpression((ExpressionParser.Variable)exp);
			}

			if(exp is ExpressionParser.Operator)
			{
				return CreateAdvancedExpression((ExpressionParser.Operator)exp);
			}

			throw new ApplicationException("Expression type not supported: " + exp.GetType().ToString());
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
		public static CodeExpression CreateAdvancedExpression(ExpressionParser.Operator exp)
		{
			CodeBinaryOperatorType op;
			switch(exp.Op)
			{
				case "+":
					op = CodeBinaryOperatorType.Add;
					break;
				case "-":
					op = CodeBinaryOperatorType.Subtract;
					break;
				case "*":
					op = CodeBinaryOperatorType.Multiply;
					break;
				case "/":
					op = CodeBinaryOperatorType.Divide;
					break;
				default:
					throw new ApplicationException("Operator type not supported: " + exp.Op);
			}

			return new CodeBinaryOperatorExpression(CreateAdvancedExpression(exp.LeftExpression), op, CreateAdvancedExpression(exp.RightExpression));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
		public static CodeExpression CreateAdvancedExpression(ExpressionParser.Number exp)
		{
			return new CodePrimitiveExpression(decimal.Parse(exp.Value, System.Globalization.CultureInfo.InvariantCulture));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
		public static CodeExpression CreateAdvancedExpression(ExpressionParser.Variable exp)
		{
			PrimaryKey pk = ExpressionHelper.GetConceptKeyByDescription(exp.Name);
			if(pk == null)
			{
				throw new BusinessException("MSGERR_CONCEPT_NOT_FOUND");
			}

			return new CodeConceptExpression(pk);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
		public static PrimaryKey GetConceptKeyByDescription(string description)
		{
			ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(SCREN.Concept.Properties.Description, description );

            IList conceptList = ConceptDAO.ListConcepts(filter.GetPredicate(), null);

            if (conceptList.Count == 0)
			{
				return null;
			}


            return ((SCREN.Concept)conceptList[0]).PrimaryKey;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="conceptId"></param>
        /// <returns></returns>
		public static string GetConceptDescriptionByKey(int entityId, int conceptId)
		{
            SCREN.Concept concept = ConceptDAO.GetConceptByConceptIdEntityId(conceptId, entityId);

            if (concept == null)
            {
                return null;
            }

            return concept.Description;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public static int GetRuleSetIdByDescription(string name)
		{
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(RuleSet.Properties.Description, name);

            IList ruleSetList = RuleSetDAO.ListRuleSet(filter.GetPredicate(), null);

            if (ruleSetList.Count == 0)
            {
                return -1;
            }

            return ((RuleSet)ruleSetList[0]).RuleSetId;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public static string GetRuleSetDescriptionById(int id)
		{
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(RuleSet.Properties.RuleSetId, id);

            IList ruleSetList = RuleSetDAO.ListRuleSet(filter.GetPredicate(), null);

            if (ruleSetList.Count == 0)
            {
                return null;
            }

            return ((RuleSet)ruleSetList[0]).Description;

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="conceptId"></param>
        /// <returns></returns>
		public static string GetSearchComboConfigFile(int entityId, int conceptId)
		{

            ReferenceConcept referenceConcept = ReferenceConceptDAO.FindReferenceConcept(conceptId, entityId);


            if (referenceConcept == null)
            {
                throw new BusinessException("No se encontro el ReferenceConcept o entidad relacionada para el concepto (" + entityId.ToString() + "-" + conceptId.ToString() + ").");
            }

            PARAMEN.Entity entity = EntityDAO.FindEntity(referenceConcept.FentityId);

            if(entity == null)
			{
                throw new BusinessException("La entidad relacionada no tiene archivo de SearchCombo asociado para el concepto (" + entityId.ToString() + "-" + conceptId.ToString() + ").");
			}

			return entity.ConfigFile;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="conceptId"></param>
        /// <returns></returns>
		public static IList<Pair> GetListEntityValuesFor(int entityId, int conceptId)
		{
            SCREN.ListConcept listConcept = ListConceptDAO.FindListConcept(conceptId, entityId);

            if (listConcept == null)
            {
                return null;
            }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(SCREN.ListEntityValue.Properties.ListEntityCode, listConcept.ListEntityCode);

            IList listEntityValueList = ListEntityValueDAO.ListListEntityValue(filter.GetPredicate(), null);			

			if(listEntityValueList.Count == 0)
			{
				return null;
			}

            List<Pair> list = new List<Pair>();
			foreach(SCREN.ListEntityValue listEntityValue in listEntityValueList)
			{
				list.Add(new Pair(listEntityValue.ListValueCode, listEntityValue.ListValue));
			}

			return list;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="conceptId"></param>
        /// <returns></returns>
        public static IList<Pair> GetRangeEntityValuesFor(int entityId, int conceptId)
        {
            SCREN.RangeConcept rangeConcept = RangeConceptDAO.FindRangeConcept(conceptId, entityId);

            if (rangeConcept == null)
            {
                return null;
            }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(SCREN.RangeConcept.Properties.RangeEntityCode, rangeConcept.RangeEntityCode);

            IList rangeEntityValueList = RangeConceptDAO.ListRangeConcept(filter.GetPredicate(), null);

            if (rangeEntityValueList.Count == 0)
            {
                return null;
            }

            List<Pair> list = new List<Pair>();
            foreach (RangeEntityValue rangeEntityValue in rangeEntityValueList)
            {
                list.Add(new Pair(rangeEntityValue.RangeValueCode, string.Format(CultureInfo.InvariantCulture, "{0}-{1}", rangeEntityValue.FromValue, rangeEntityValue.ToValue)));
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="conceptId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSearchComboDescription(int entityId, int conceptId, string key)
        {
            PARAMEN.Entity entity = EntityDAO.FindEntity(entityId);
            
            string[] fields = entity.PropertySearch.Split(';');

            string propertySearch = "";

            for (int i = 0; i < fields.Length; i++)
            {
                propertySearch += fields[i].Split(',')[0];
                if (i < fields.Length-1)
                {
                    propertySearch += ",";    
                }                
            }
            
            string filter = entity.PropertySearch.Split(';')[0].Split(',')[0].Split(' ')[0] + " = " + key;

            NameValue[] pars = new NameValue[3];
            pars[0] = new NameValue("TABLES", entity.BusinessView);
            pars[1] = new NameValue("FIELDS", propertySearch);
            pars[2] = new NameValue("FILTER", filter);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", pars);
            }

            if (result != null && result.Rows.Count > 0)
            {
                return result.Rows[0][1].ToString() + " (" + key + ")";
            }

            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="conceptId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSearchComboDescription2(int entityId, int conceptId, string key)
		{            
            PARAMEN.Entity entity = EntityDAO.FindEntity(entityId);

            string[] fields = entity.PropertySearch.Split(',');

            string[] keyField = fields[0].Split(' ');

            string filter = keyField[0] + " = " + key;

            NameValue[] pars = new NameValue[3];
            pars[0] = new NameValue("TABLES", entity.BusinessView);
            pars[1] = new NameValue("FIELDS", entity.PropertySearch);
            pars[2] = new NameValue("FILTER", filter);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", pars);
            }

            if (result != null && result.Rows.Count > 0)
            {
                return result.Rows[0][1].ToString() + " (" + key + ")" ;
            }   

            return key;
		}

		private ExpressionHelper()
		{
		}
	}
}
