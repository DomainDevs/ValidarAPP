using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.Application.PrintingServices.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases.Temp
{
    class TmpResponsabilidadCivil : ResponsabilidadCivil
    {
        public TmpResponsabilidadCivil() : base() { }

        /// <summary>
        /// Forma el nombre del archivo pdf generado
        /// </summary>
        /// <returns>Nombre del archivo pdf generado</returns>
        //protected override string getFileName()
        //{
        //    return this.BranchCode +
        //           this.PrefixCode +
        //           this.PolicyData.TempNum +
        //           this.EndorsementNumber;
        //}

        /// <summary>
        /// Agrega el titu lo del reporte
        /// </summary>
        protected override void setReportTitle()
        {
            this.File.Add(new DatRecord(RptFields.LBL_TEMPORAL_NUMBER_HEADER, null));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_POLICY_NUMBER_HEADER, this.OperationId.ToString()));
        }
    }
}
