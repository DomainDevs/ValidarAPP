using System;
using System.Collections.Generic;
using Sistran.Company.ExternalPrinterServices.Models;

namespace Sistran.Company.ExternalPrinterServicesEEProvider.Assemblers
{
    public class ModelAssembler
    {      
        public static PrinterClass GenerateWSPolicyFormatCollect(WSPolicyPrinter.PrinterClass printer) => new PrinterClass
        {
            Message = printer.Message,
            PrinterBinary = printer.PrinterBinary,
            ProcessMessage = printer.ProcessMessage,
            XMLFTP = printer.XMLFTP
        };

        public static PrinterClass GenerateWSQuotePrinter(WSQuotePrinter.PrinterClass printer) => new PrinterClass
        {
            ProcessMessage = printer.ProcessMessage,
            Message = printer.Message,
            PrinterBinary = printer.PrinterBinary,
            XMLFTP = printer.XMLFTP
        };

       
    }
}
