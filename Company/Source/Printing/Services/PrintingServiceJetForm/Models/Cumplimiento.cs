using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    class Cumplimiento : Fianza
    {
        public Cumplimiento() : base() { }

        /// <summary>
        /// Adiciona las coberturas de la póliza
        /// </summary>
        /*protected override void printRiskCoverageData()
        {
            base.printRiskCoverageData();
            this.File.Add(new DatRecord(RptFields.TTL_SCOPE_OF_WARRANTY, null));
            this.validatePageLimit(2);
            this.PrintGeneralLevelText();
            this.File.Add(new DatRecord(String.Empty, null));
            this.printAccesories();
            this.printClauses();
        }*/
    }
}
