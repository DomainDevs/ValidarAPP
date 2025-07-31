using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.Application.PrintingServices.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases.Quotation
{
    class QuoAutos : Autos
    {
        public QuoAutos() : base() { }

        /// <summary>
        /// Forma el nombre del archivo pdf generado
        /// </summary>
        /// <returns>Nombre del archivo pdf generado</returns>
        //protected override string getFileName()
        //{
        //    return this.BranchCode +
        //           this.PrefixCode +
        //           this.PolicyData.QuotationId +
        //           this.EndorsementNumber;
        //}

        /// <summary>
        /// Agrega el titu lo del reporte
        /// </summary>
        protected override void setReportTitle()
        {
            this.File.Add(new DatRecord(RptFields.LBL_QUOTATION_NUMBER_HEADER, null));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_POLICY_NUMBER_HEADER, PolicyData.QuotationId.ToString()));
        }
    }
}
