using System.CodeDom;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Colección de operadores de comparación.
    /// </summary>
    public sealed class ArithmeticOperatorCollection : OperatorCollection
    {
        #region Constants
        public static readonly Operator Add = new
            Operator(CodeBinaryOperatorType.Add,
            "LBL_ADD", "LBL_ADD_SMALLDESC");

        public static readonly Operator Subtract = new
            Operator(CodeBinaryOperatorType.Subtract,
            "LBL_SUBSTRACT", "LBL_SUBSTRACT_SMALLDESC");

        public static readonly Operator Multiply = new
            Operator(CodeBinaryOperatorType.Multiply,
            "LBL_MULTIPLY", "LBL_MULTIPLY_SMALLDESC");

        public static readonly Operator Divide = new
            Operator(CodeBinaryOperatorType.Divide,
            "LBL_DIVIDE", "LBL_DIVIDE_SMALLDESC");

        public static readonly Operator Assign = new
            Operator(CodeBinaryOperatorType.Assign,
            "LBL_ASSIGN", "LBL_ASSIGN_SMALLDESC");

        public static readonly Operator Round = new
            Operator(CodeBinaryOperatorType.BitwiseAnd,
            "LBL_ROUND", "LBL_ROUND_SMALLDESC");
        #endregion Constants

        public ArithmeticOperatorCollection()
        {
            _innerList = new ListDictionary();
            _innerList.Add(ArithmeticOperatorCollection.Add.Code, ArithmeticOperatorCollection.Add);
            _innerList.Add(ArithmeticOperatorCollection.Divide.Code, ArithmeticOperatorCollection.Divide);
            _innerList.Add(ArithmeticOperatorCollection.Multiply.Code, ArithmeticOperatorCollection.Multiply);
            _innerList.Add(ArithmeticOperatorCollection.Subtract.Code, ArithmeticOperatorCollection.Subtract);
            _innerList.Add(ArithmeticOperatorCollection.Assign.Code, ArithmeticOperatorCollection.Assign);
            _innerList.Add(ArithmeticOperatorCollection.Round.Code, ArithmeticOperatorCollection.Round);
        }
    }
}
