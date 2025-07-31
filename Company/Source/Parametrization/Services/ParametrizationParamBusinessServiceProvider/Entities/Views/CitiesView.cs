using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class CitiesView : BusinessView
    {

        public BusinessCollection Country
        {
            get
            {
                return this["Country"];
            }
        }

        public BusinessCollection State
        {
            get
            {
                return this["State"];
            }
        }


        public BusinessCollection City
        {
            get
            {
                return this["City"];
            }
        }

    }
}
