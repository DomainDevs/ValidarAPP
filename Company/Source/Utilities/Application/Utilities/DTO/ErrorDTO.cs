using Sistran.Company.Application.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Company.Application.Utilities.DTO
{
    public class ErrorDTO
    {
         public List<string> ErrorDescription { get; set; }

        //se asocia el enum, hasta crear el ErrorTypeService 
         public ErrorType ErrorType { get; set; }
    }
}
