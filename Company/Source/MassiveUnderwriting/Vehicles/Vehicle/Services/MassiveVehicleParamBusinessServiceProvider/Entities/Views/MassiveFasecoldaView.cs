using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Entities.View
{
    [Serializable()]
    public class MassiveFasecoldaView : BusinessView
    {
        public BusinessCollection CiaAsynchronousProcessFasecoldaMassiveLoads
        {
            get
            {
                return this["CiaAsynchronousProcessFasecoldaMassiveLoad"];
            }
        }
        public BusinessCollection CiaAsynchronousProcessFasecoldaRows
        {
            get
            {
                return this["CiaAsynchronousProcessFasecoldaRow"];
            }
        }
    }
}
