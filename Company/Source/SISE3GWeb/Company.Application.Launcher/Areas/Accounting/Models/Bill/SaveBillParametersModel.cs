using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("BillModel")]
    public class SaveBillParametersModel
    {
        public int BillId { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
    }
}