
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class FormPrintBillReportModel
    {
        public int PolicyNumber { get; set; } //nro_pol
        public int InsuredId { get; set; } //cod_aseg
        public string ShortDescription { get; set; } //txt_desc
        public string Description { get; set; } //txt_desc
        public int AgentId { get; set; } //cod_agente
        public string Collector { get; set; } //Cobrador        
        public string Name { get; set; } //NOMBRE
        public string Address { get; set; } //Direccion
        public string Nit { get; set; } //nro_nit
        public string CollectionDate { get; set; } //fecha_cobro
        public string Cuote { get; set; } //CUOTA
        public string IssuanceDate { get; set; } //fec_emi_comprobante //BROSADO_CORRECCIONES
        public string FromDate { get; set; } //Del
        public string ToDate { get; set; } //Al
        public int EndorsementNumber { get; set; } //nro_endoso
        public string Premium { get; set; } //Prima       
        public string OutGo { get; set; } //Gastos       
        public string IVA { get; set; } //IVA       
        //3790
        public decimal Decree { get; set; } //Decreto  //REQ - 16443 / formato impresion_factura     
        public decimal EquivalentPremium { get; set; } //imp_prima_eq       
        public string Text { get; set; } //TEXTO      
        public string AmountInWords { get; set; } //Cantidad_en_Letras      
        public string Collector_Receiver { get; set; } //receptor_cobrador      
        public string MadeBy { get; set; } //Elaborado_por      
        public string CollectionDate2 { get; set; } //fecha_cobro  
        public string VoucherNumber { get; set; } //nro_comprobante  
        public string SeveralConcepts { get; set; }//conceptos_varios
        //REQ - 16443 / formato impresion_factura
        public decimal PremiumValue { get; set; } //Prima           
        public decimal OutGoValue { get; set; } //Gastos            
        public decimal IVAValue { get; set; } //IVA 
        //FIN 3790
        public string SerialNumber { get; set; } //IVA 
        //FIN REQ - 16443 / formato impresion_factura
        public string Beneficiary { get; set; }//REQ - 16586 / Modificación para adaptar reporte de seguros y fianzas
        //BROSADO_CORRECCIONES        
        public string PaymentType { get; set; }
        public string ShortCurrencyName { get; set; } 
        //FIN BROSADO_CORRECCIONES

        //3852 - Correccion
        /// <summary>
        /// Suma Asegurada
        /// </summary>        
        public decimal SumInsured { get; set; }

        //3872 - Impresion de Notas de Credito, por medio de orden de pago
        /// <summary>
        /// Indice para el numero de copias para la impresion de notas de credito
        /// </summary>
        public int Indice { get; set; }
        //FIN 3872
    }
}