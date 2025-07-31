using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Enums
{
    public class ParametrizationTypes
    {
        public enum QuotaType
        {
            IssueDate=1, //fecha de emision 
            CurrentDate=2, //fecha de inicio de vigencia 
            GreaterDate=3, //la mayor de ambas
        }

        public enum PaymentCalculationType
        {            
            Day = 1, //Dia           
            Fortnight = 2, // Quincena           
            Month = 3 //Mes
        }

        public enum StatusTypeService
        {
            Original = 1,
            Create = 2,
            Update = 3,
            Delete = 4,
            Error = 5
        }
    }
}