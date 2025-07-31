using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Enums
{
    [DataContract]
    public enum OperatorConditionType
    {
        [EnumMember(Value = "=")]
        Equals = 1,

        [EnumMember(Value = "<")]
        LessThan = 2,

        [EnumMember(Value = "<=")]
        LessThanOrEquals = 3,

        [EnumMember(Value = ">")]
        GreaterThan = 4,

        [EnumMember(Value = ">=")]
        GreaterThanOrEquals = 5,

        [EnumMember(Value = "<>")]
        Distinct = 6,

        [EnumMember(Value = "Null")]
        Null = 7,

        [EnumMember(Value = "Not null")]
        NotNull = 8

    }

    [DataContract]
    public enum ComparatorType
    {
        [EnumMember(Value = "Constant Value")]
        ConstantValue = 1,

        [EnumMember(Value = "Concept Value")]
        ConceptValue = 2,

        [EnumMember(Value = "Expression Value")]
        ExpressionValue = 3,

        [EnumMember(Value = "Temporaly Value")]
        TemporalyValue = 4
    }

    [DataContract]
    public enum AssignType
    {
        [EnumMember(Value = "Concept Assign")]
        ConceptAssign = 1,

        [EnumMember(Value = "Invoke Assign")]
        InvokeAssign = 2,

        [EnumMember(Value = "Temporal Assign")]
        TemporalAssign = 3
    }


    [DataContract]
    public enum ArithmeticOperatorType
    {
        [EnumMember(Value = "+")]
        Add = 0,

        [EnumMember(Value = "-")]
        Subtract = 1,

        [EnumMember(Value = "*")]
        Multiply = 2,

        [EnumMember(Value = "/")]
        Divide = 3,

        [EnumMember(Value = "=")]
        Assign = 5,

        [EnumMember(Value = "Round ")] //CodeBinaryOperatorType.BitwiseAnd
        Round = 10
    }


    [DataContract]
    public enum InvokeType
    {
        [EnumMember(Value = "MessageInvoke")]
        MessageInvoke = 1,

        [EnumMember(Value = "RuleSetInvoke")]
        RuleSetInvoke = 2,

        [EnumMember(Value = "FunctionInvoke")]
        FunctionInvoke = 3
    }

    [DataContract]
    public enum ConceptType
    {
        [EnumMember(Value = "Basic")]
        Basic = 1,

        [EnumMember(Value = "Range")]
        Range = 2,

        [EnumMember(Value = "List")]
        List = 3,

        [EnumMember(Value = "Reference")]
        Reference = 4
    }

    [DataContract]
    public enum RuleBaseType
    {
        [EnumMember(Value = "Rule")]
        Sequence = 1,

        [EnumMember(Value = "Decision table")]
        Option = 2

        //[EnumMember(Value = "OptionLoop")]
        //OptionLoop = 3
    }



    [DataContract]
    public enum ConceptControlType
    {
        [EnumMember(Value = "TextBox")]
        TextBox = 1,

        [EnumMember(Value = "NumberEditor")]
        NumberEditor = 2,

        [EnumMember(Value = "DateEditor")]
        DateEditor = 3,

        [EnumMember(Value = "ListBox")]
        ListBox = 4,

        [EnumMember(Value = "SearchCombo")]
        SearchCombo = 5,

        [EnumMember(Value = "RichTextEditor")]
        RichTextEditor = 6
    }

    [DataContract]
    public enum BasicType
    {
        [EnumMember(Value = "Null")]
        Null = 0,

        [EnumMember(Value = "Numeric")]
        Numeric = 1,

        [EnumMember(Value = "Text")]
        Text = 2,

        [EnumMember(Value = "Decimal")]
        Decimal = 3,

        [EnumMember(Value = "Date")]
        Date = 4
    }




    /***************************
     * Antiguo
     ********************************/





    [DataContract]
    public enum Level
    {
        [EnumMember(Value = "General")]
        General = 1,
        [EnumMember(Value = "Risk")]
        Risk = 2,
        [EnumMember(Value = "Coverage")]
        Coverage = 3,
        [EnumMember(Value = "Component")]
        Component = 4,
        [EnumMember(Value = "Commission")]
        Commission = 5,
    }

    public enum LevelAutomaticQuota
    {
        [EnumMember(Value = "General")]
        General = 1,
        [EnumMember(Value = "Tercero")]
        Risk = 2,
        [EnumMember(Value = "Utilidad")]
        Coverage = 3,
        [EnumMember(Value = "Indicadores")]
        Component = 4,
    }
    [DataContract]
    public enum ValueType
    {
        [EnumMember(Value = "Value")]
        Value,
        [EnumMember(Value = "Concept")]
        Concept,
        [EnumMember(Value = "ExpressionResult")]
        ExpressionResult,
        [EnumMember(Value = "TemporalValue")]
        TemporalValue
    }


}
