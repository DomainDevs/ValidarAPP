
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using EmRules = Sistran.Core.Application.RulesScriptsServices.Enums;
using EmUtil= Sistran.Core.Application.Utilities.Enums;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{

    #region Reglas
    [DataContract]
    public class _RuleSet
    {
        [DataMember]
        public int RuleSetId { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public _Level Level { set; get; }

        [DataMember]
        public _Package Package { set; get; }

        [DataMember]
        public DateTime CurrentFrom { set; get; }

        [DataMember]
        public DateTime? CurrentTo { set; get; }

        [DataMember]
        public int RuleSetVer { set; get; }

        [DataMember]
        public bool? IsEvent { set; get; }

        [DataMember]
        public List<_Rule> Rules { set; get; }

        [DataMember]
        public Enums.RuleBaseType Type { set; get; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public EmUtil.ActiveRuleSetType? ActiveType { get; set; }

        [DataMember]
        public bool Active{ get; set; }

        [DataMember]
        public string FileExceptions { get; set; }
    }

    [DataContract]
    public class _Rule
    {
        [DataMember]
        public int RuleId { get; set; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public List<_Parameter> Parameters { set; get; }

        [DataMember]
        public List<_Condition> Conditions { set; get; }

        [DataMember]
        public List<_Action> Actions { set; get; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }
    }

    [DataContract]
    public class _Parameter //: CodeParameterDeclarationExpression
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class _Condition
    {
        [DataMember]
        public dynamic Concept { set; get; }

        [DataMember]
        public _Comparator Comparator { get; set; }

        [DataMember]
        public Enums.ComparatorType ComparatorType { get; set; }

        [DataMember]
        public dynamic Expression { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }
    }


    [DataContract]
    public class _Comparator
    {
        [DataMember]
        public Enums.OperatorConditionType Operator { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Symbol { get; set; }
    }

    [DataContract]
    public class _ArithmeticOperator
    {
        [DataMember]
        public Enums.ArithmeticOperatorType ArithmeticOperatorType { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Symbol { get; set; }
    }

    [DataContract]
    public class _Action
    {
        [DataMember]
        public Enums.AssignType AssignType { get; set; }

        [DataMember]
        public dynamic Expression { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }
    }

    [DataContract]
    public class _ActionConcept : _Action
    {
        [DataMember]
        public dynamic Concept { set; get; }

        [DataMember]
        public Enums.ComparatorType ComparatorType { get; set; }

        [DataMember]
        public _ArithmeticOperator ArithmeticOperator { get; set; }
    }

    [DataContract]
    public class _ActionValueTemp : _Action
    {
        [DataMember]
        public string ValueTemp { set; get; }

        [DataMember]
        public Enums.ComparatorType ComparatorType { get; set; }

        [DataMember]
        public _ArithmeticOperator ArithmeticOperator { get; set; }
    }

    [DataContract]
    public class _ActionInvoke : _Action
    {
        [DataMember]
        public Enums.InvokeType InvokeType { set; get; }
    }


    [DataContract]
    public class _Level
    {
        [DataMember]
        public int LevelId { set; get; }

        [DataMember]
        public string Description { set; get; }
    }

    [DataContract]
    public class _Package
    {
        [DataMember]
        public int PackageId { set; get; }

        [DataMember]
        public string Description { set; get; }
    }

    [DataContract]
    public class _Concept
    {
        [DataMember]
        public int ConceptId { get; set; }

        [DataMember]
        public Entity Entity { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ConceptName { get; set; }

        [DataMember]
        public Enums.ConceptType ConceptType { get; set; }

        [DataMember]
        public int KeyOrder { get; set; }

        [DataMember]
        public bool IsStatic { get; set; }

        [DataMember]
        public Enums.ConceptControlType ConceptControlType { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public bool IsVisible { get; set; }

        [DataMember]
        public bool IsNulleable { get; set; }

        [DataMember]
        public bool IsPersistible { get; set; }

        [DataMember]
        public List<_ConceptDependence> ConceptDependences { set; get; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }
    }

    [DataContract]
    public class _ConceptDependence
    {
        [DataMember]
        public _Concept DependsConcept { set; get; }

        [DataMember]
        public int Order { set; get; }

        [DataMember]
        public string ColumnName { set; get; }
    }

    [DataContract]
    public class _BasicConcept : _Concept
    {
        [DataMember]
        public Enums.BasicType BasicType { set; get; }

        [DataMember]
        public decimal? MinValue { get; set; }


        [DataMember]
        public decimal? MaxValue { get; set; }

        [DataMember]
        public DateTime? MinDate { get; set; }

        [DataMember]
        public DateTime? MaxDate { get; set; }

        [DataMember]
        public decimal? Length { get; set; }
    }


    [DataContract]
    public class _RangeConcept : _Concept
    {
        [DataMember]
        public _RangeEntity RangeEntity { get; set; }
    }

    [DataContract]
    public class _RangeEntity
    {
        [DataMember]
        public int RangeEntityCode { get; set; }

        [DataMember]
        public string DescriptionRange { get; set; }

        [DataMember]
        public int RangeValueAt { get; set; }

        [DataMember]
        public List<_RangeEntityValue> RangeEntityValues { get; set; }
    }

    [DataContract]
    public class _RangeEntityValue
    {
        [DataMember]
        public int RangeValueCode { get; set; }

        [DataMember]
        public int FromValue { get; set; }

        [DataMember]
        public int ToValue { get; set; }
    }


    [DataContract]
    public class _ListConcept : _Concept
    {
        [DataMember]
        public _ListEntity ListEntity { get; set; }
    }

    [DataContract]
    public class _ListEntity
    {
        [DataMember]
        public int ListEntityCode { get; set; }

        [DataMember]
        public string DescriptionList { get; set; }

        [DataMember]
        public int ListValueAt { get; set; }

        [DataMember]
        public List<_ListEntityValue> ListEntityValues { get; set; }
    }

    [DataContract]
    public class _ListEntityValue
    {
        [DataMember]
        public int ListValueCode { get; set; }

        [DataMember]
        public string ListValue { get; set; }
    }

    [DataContract]
    public class _ReferenceConcept : _Concept
    {
        [DataMember]
        public Entity FEntity { get; set; }

        [DataMember]
        public List<_ReferenceValue> ReferenceValues { get; set; }
    }

    [DataContract]
    public class _ReferenceValue
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class _RuleFunction
    {
        [DataMember]
        public int RuleFunctionId { set; get; }

        [DataMember]
        public _Package Package { set; get; }

        [DataMember]
        public _Level Level { set; get; }

        [DataMember]
        public string FunctionName { set; get; }

        [DataMember]
        public string Description { set; get; }
    }

    [DataContract]
    public class _ComparatorType
    {
        [DataMember]
        public int Id { set; get; }

        [DataMember]
        public string Description { set; get; }
    }

    [DataContract]
    public class _ActionType
    {
        [DataMember]
        public int Id { set; get; }

        [DataMember]
        public string Description { set; get; }
    }

    [DataContract]
    public class _InvokeType
    {
        [DataMember]
        public int Id { set; get; }

        [DataMember]
        public string Description { set; get; }
    }

    [DataContract]
    public class _ArithmeticOperatorType
    {
        [DataMember]
        public int Id { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public string Symbol { set; get; }
    }

    #endregion

    #region DecisionTable
    [DataContract]
    public class _RuleBase
    {
        [DataMember]
        public int RuleBaseId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public EmRules.RuleBaseType RuleBaseType { get; set; }

        [DataMember]
        public _Package Package { set; get; }

        [DataMember]
        public _Level Level { set; get; }

        [DataMember]
        public string CurrentFrom { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public bool IsPublished { get; set; }

        [DataMember]
        public int RuleEnumerator { get; set; }

        [DataMember]
        public bool IsEvent { get; set; }
    }

    [DataContract]
    public class _DecisionTableMappingResult
    {
        /// <summary>
        /// propiedad RuleBase
        /// </summary>
        [DataMember]
        public _RuleBase RuleBase { get; set; }

        /// <summary>
        /// propiedad CountActions
        /// </summary>
        [DataMember]
        public int CountActions { get; set; }

        /// <summary>
        /// propiedad CountCondition
        /// </summary>
        [DataMember]
        public int CountCondition { get; set; }

        /// <summary>
        /// propiedad CountRules
        /// </summary>
        [DataMember]
        public int CountRules { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// propiedad DataSet
        /// </summary>
        [DataMember]
        public DataTable DataSet { get; set; }

        /// <summary>
        /// url de archivo con exceptions
        /// </summary>
        [DataMember]
        public string FileExceptions { get; set; }
    }
    #endregion
}
