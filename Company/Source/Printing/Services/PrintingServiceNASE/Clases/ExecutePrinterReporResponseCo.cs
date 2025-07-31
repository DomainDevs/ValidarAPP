using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class ExecutePrinterReporResponseCo
    {
        public ExecutePrinterReporResponseCo() { }

        public string PathReportConvention { get; set; }
        public string PathReportPolicy { get; set; }
        public SerialDataSet ReportPrinter { get; set; }
    }
}
