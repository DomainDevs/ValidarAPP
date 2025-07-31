using System;

namespace Sistran.Core.Application.WrapperAccountingServiceEEProvider.Models
{
    public class CollectControl
    {
        internal int UserId { get; set; }
        internal DateTime accountingDate { get; set; }
        internal short Branch { get; set; }        
    }
}
