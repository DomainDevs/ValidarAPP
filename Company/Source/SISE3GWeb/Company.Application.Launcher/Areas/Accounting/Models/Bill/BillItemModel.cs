using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("BillItemModel")]
    public class BillItemModel
    {
        public int BillItemId { get; set; }
        public int BillId { get; set; }
        public int ItemTypeId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
    }

    [KnownType("ItemsToPayGridModel")]
    public class ItemsToPayGridModel
    {
        public List<BillItemModel> BillItem { get; set; }
    }

}