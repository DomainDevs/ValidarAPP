using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("InvoiceModel")]
    public class InvoiceModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceClientName { get; set; }
        public string InvoiceClientRuc { get; set; }
        public string InvoiceClientAddress { get; set; }
        public string InvoiceClientPhone { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceIva { get; set; }
        public decimal InvoiceSubTotal { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string InvoiceBankName { get; set; }
        public string InvoiceBarcode { get; set; }
        public Byte[] InvoiceBarcodeImage { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
    }

    public class InvoiceDetail
    {
        public int DetailQuantity { get; set; }
        public string DetailDescription { get; set; }
        public decimal DetailUnitPrice { get; set; }
        public decimal DetailTotalPrice { get; set; }
    }
}