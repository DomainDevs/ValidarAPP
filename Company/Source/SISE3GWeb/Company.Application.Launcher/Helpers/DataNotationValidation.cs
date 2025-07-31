
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class DataNotationValidationAttribute : RegularExpressionAttribute
    {
        //
        // Resumen:
        //     Initializes a new instance of the System.ComponentModel.DataAnnotations.RegularExpressionAttribute
        //     class.
        //
        // Parámetros:
        //   pattern:
        //     The regular expression that is used to validate the data field value.
        //
        // Excepciones:
        //   T:System.ArgumentNullException:
        //     pattern is null.

        public DataNotationValidationAttribute()
            : base(GetRegex())
        { }

        

        private static string GetRegex()
        {
            List<ValidationRegularExpression> listRegular = DelegateService.utilitiesServiceCore.GetAllValidationRegularExpressions();
            if (listRegular.Count > 0 && listRegular.Where(x => x.FieldDescription.ToUpper().Trim() == "VALIDACION PLACA").Count() > 0)
            {
                
                return listRegular.Where(x => x.FieldDescription.ToUpper().Trim() == "VALIDACION PLACA").FirstOrDefault().ParameterValue;
            }
            else {
                return @"^[a-zA-Z]{3}[0-9]{3}|[a-zA-Z]{3}[0-9]{2}[a-zA-Z]{1}|[rRsS]{1}[0-9]{5}|[a-zA-z]{2}[0-9]{4}|[0-9]{3}[a-zA-z]{3}$";
            }            
        }
    }
}