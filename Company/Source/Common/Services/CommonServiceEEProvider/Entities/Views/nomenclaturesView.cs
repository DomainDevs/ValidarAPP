using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class nomenclaturesView : BusinessView
    {
        public BusinessCollection Co_Nomenclature
        {
            get
            {
                return this["Co_Nomenclatures"];
            }
        }
    }
}
