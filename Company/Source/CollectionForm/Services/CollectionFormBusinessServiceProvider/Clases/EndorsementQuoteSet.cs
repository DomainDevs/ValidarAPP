using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessServiceProvider.Clases
{
    public class EndorsementQuoteSet
    {
        #region Properties
        private Dictionary<string, EndorsementQuote> endorsementQuoteDiccionary = null;
        #endregion

        public EndorsementQuoteSet()
        {
            endorsementQuoteDiccionary = new Dictionary<string, EndorsementQuote>();
        }
        public EndorsementQuoteSet(List<EndorsementQuote> endorsementQuoteList)
        {
            endorsementQuoteDiccionary = new Dictionary<string, EndorsementQuote>();

            foreach (EndorsementQuote oEndorsementQuote in endorsementQuoteList)
            {
                this.addOrReplace(oEndorsementQuote);
            }

        }
        public void addOrReplace(EndorsementQuote oEndorsementQuote)
        {
            string key = String.Concat(oEndorsementQuote.EndorsementNumber, "-", oEndorsementQuote.QuoteNum);

            if (String.IsNullOrEmpty(key))
            {
                throw new Exception("No se puede adicionar un endoso con clave vacía");
            }
            if (oEndorsementQuote == null)
            {
                throw new Exception("No se puede adicionar un endoso nulo");
            }
            endorsementQuoteDiccionary[key] = oEndorsementQuote;
        }

        public EndorsementQuote getEndorsementQuote(string EndorsementNumber, string QuoteNum)
        {
            EndorsementQuote ans = null;
            string key = string.Concat(EndorsementNumber, "-", QuoteNum);
            if (endorsementQuoteDiccionary == null)
            {
                throw new Exception("No se puede consultar un edoso en un diccionario nulo");
            }
            endorsementQuoteDiccionary.TryGetValue(key, out ans);

            return ans;
        }
        public EndorsementQuote getEndorsementQuote(string EndorsementNumber, string QuoteNum, int Status)
        {
            EndorsementQuote ans = null;
            ans = getEndorsementQuote(EndorsementNumber, QuoteNum);
            if (ans != null)
            {
                if (ans.Status != Status)
                {
                    ans = null;
                }
            }

            return ans;
        }

        public List<EndorsementQuote> getList()
        {
            List<EndorsementQuote> ans = new List<EndorsementQuote>(endorsementQuoteDiccionary.Values);
            return ans;
        }
    }
}
