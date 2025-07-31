using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Types
{
    public class Types
    {
        public enum EndorsementQuoteSearchResults
        {
            NotDefined = -1,
            RecordsFound = 1,
            PolicyNotExists = 2,
            PolicyNotEmitedInValidCurrency = 3,
            PolicyWithNotDebt = 4
        }

        public enum SystranSystem
        {
            NotDefined = -1,
            Sise2G = 1,
            Sise3G = 2
        }
    }
}
