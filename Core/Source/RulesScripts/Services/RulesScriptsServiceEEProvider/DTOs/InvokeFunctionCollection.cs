using System.Collections;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for Operators.
    /// </summary>
    public class InvokeFuntionCollection : IEnumerable
    {
        private static IDictionary _innerList;

        public static readonly DictionaryEntry InvokeMessage = new DictionaryEntry(0, "LBL_INVOKEMESSAGE");
        public static readonly DictionaryEntry InvokeRuleSet = new DictionaryEntry(1, "LBL_INVOKERULESET");
        public static readonly DictionaryEntry InvokeFunction = new DictionaryEntry(2, "LBL_INVOKEFUNCTION");
        // Se deja para V2
        //public static readonly DictionaryEntry InvokeFunction=new DictionaryEntry(1,"LBL_INVOKEFUNCTION");
        //public static readonly DictionaryEntry InvokeAction=new DictionaryEntry(2,"LBL_INVOKEACTION");

        public InvokeFuntionCollection()
        {
            if (_innerList == null)
            {
                _innerList = new ListDictionary();
                _innerList.Add(InvokeMessage.Key, InvokeMessage.Value);
                _innerList.Add(InvokeRuleSet.Key, InvokeRuleSet.Value);
                _innerList.Add(InvokeFunction.Key,InvokeFunction.Value);
                // Se deja para V2
                /*_innerList.Add(InvokeFunction.Key ,  InvokeFunction.Value );
                _innerList.Add(InvokeAction.Key ,  InvokeAction.Value );
                    */
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
