using Sistran.Company.Application.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Utilities.DTO
{
    public class ExcelFileDTO: ErrorDTO
    {
        public string File { get; set; }

       // public ErrorType ErrorType { get; set; }
    }
}
