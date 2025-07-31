using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Helpers
{
    public class FullServicesProviderHelper
    {
        public const int Insert = 1;
        public const int Update = 2;
        public const int GetByPrimaryKey = 3;
        public const int GetAll = 4;
        public const int GetAllBy = 5;
        public const int Delete = 6;
        public const int DeleteByField = 7;

        // METODO QUE ELIMINA CARACTERES ESPECIALES 
        public string ToSlug(string text)
        {
            StringBuilder sb = new StringBuilder();
            var lastWasInvalid = false;
            foreach (char c in text)
            {
                //PERMITE NUMEROS Y DIGITOS , SIGNOS DE PUNTUACION, ESPACIOS EN BLANCO Y SEPARADORES
               if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c) || char.IsSeparator(c))
                {
                    sb.Append(c);
                    lastWasInvalid = false;
                }
                else
                {
                    if (!lastWasInvalid)
                        sb.Append("");
                    lastWasInvalid = true;
                }
            }
            return sb.ToString().ToUpperInvariant().Trim();

        }

        public static object ToDBNull(object value)
        {
            if(null != value)
            {
                if (value.GetType() == Type.GetType("System.String"))
                    return (value.ToString().Equals("null")) ? DBNull.Value : (object)(value.ToString().ToUpper());
                else
                    return value;
            }
            else
                return DBNull.Value;
        }

        public static object ToNullInt(object value)
        {
            if (null != value)
            {
                if (value.GetType() == Type.GetType("System.String"))
                    return (value.ToString().Equals("null")) ? null : (object)(value.ToString().ToUpper());
                else
                    return value;
            }
            else
                return null;
        }

        public static object ToDBNull(object value,string format)
        {       
            if (value != null)
            {
                if ((value.Equals("null"))||(value.Equals(" ")))
                    return DBNull.Value;
                else
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    var returnValue = DateTime.ParseExact(value.ToString().Trim(), format, provider);
                    return returnValue;
                    //value = string.Format("{0:dd/MM/yyyy}",value.ToString());
                    //return value.ToString().Split(' ')[0];
                }                
            }
            return DBNull.Value;
        }
        
        public enum Dtos
        {
            DtoInsured = 1,
            DtoLawyer = 2,
            DtoBeneficiary = 3,
            DtoAssigneed = 4,
            DtoPrincipalNational = 5,
            DtoPrincipalComertial = 6,
            DtoTechnicalAssistant = 7,
            DtoEmployee = 8,
            DtoAgent = 9,
            DtoProvider = 10,
            DtoThird = 11,
            DtoUser = 12
        }

    }
}
