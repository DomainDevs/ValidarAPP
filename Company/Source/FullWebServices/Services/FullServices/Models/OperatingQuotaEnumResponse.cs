using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class OperatingQuotaEnumResponse
    {
        public static string update = "UPDATE";
        public static string insert = "INSERT";
        public static string Sucessfull = "Registro Exitoso.";
        public static string PersonNotExists = "No existen personas registradas para la identificación dada";
        public static string AutenticationError = "Error de autenticación";
        public static string DatabaseError = "Error en Base de Datos";
        public static string ExchangeTaskError = "El valor de la tasa de cambio es 0";
        public static string coma = ",";
    }
}
