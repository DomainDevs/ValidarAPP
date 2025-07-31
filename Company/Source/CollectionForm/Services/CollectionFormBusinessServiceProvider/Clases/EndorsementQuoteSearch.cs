using Sistran.Company.Application.CollectionFormBusinessService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessServiceProvider.Clases
{
    public class EndorsementQuoteSearch
    {
        #region " Properties"
        public Types.EndorsementQuoteSearchResults searchResult = Types.EndorsementQuoteSearchResults.NotDefined;
        public List<EndorsementQuote> list = null;
        #endregion

        public EndorsementQuoteSearch(Types.EndorsementQuoteSearchResults searchResult, List<EndorsementQuote> list)
        {
            this.searchResult = searchResult;
            this.list = list;
        }
        public EndorsementQuoteSearch()
        {
            this.searchResult = Types.EndorsementQuoteSearchResults.NotDefined;
        }
    }
}
