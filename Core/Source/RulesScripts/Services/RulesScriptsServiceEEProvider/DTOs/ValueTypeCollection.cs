using System.Collections;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for Operators.
    /// </summary>
    public class ValueTypeCollection : IEnumerable
    {
        private static IDictionary _innerList;

        public static readonly DictionaryEntry ConstantValue = new DictionaryEntry(0, "LBL_CONSTANTVALUE");
        public static readonly DictionaryEntry ConceptValue = new DictionaryEntry(1, "LBL_COCNEPTVALUE");
        public static readonly DictionaryEntry AdvancedValue = new DictionaryEntry(2, "LBL_ADVANCEDVALUE");
        public static readonly DictionaryEntry TemporaryValue = new DictionaryEntry(3, "LBL_TEMPORARYVALUE");

        public ValueTypeCollection()
        {
            if (_innerList == null)
            {
                _innerList = new ListDictionary();
                _innerList.Add(ConstantValue.Key, ConstantValue.Value);
                _innerList.Add(ConceptValue.Key, ConceptValue.Value);
                _innerList.Add(AdvancedValue.Key, AdvancedValue.Value);
                _innerList.Add(TemporaryValue.Key, TemporaryValue.Value);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        public object this[int key]
        {
            get { return _innerList[key]; }
        }
    }
}
