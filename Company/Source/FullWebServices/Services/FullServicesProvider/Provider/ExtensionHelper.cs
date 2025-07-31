/***********************************************************************************************
COPYRIGHTNOTICE:
©2003-2013 SISTRAN 
soluciones de Software para Compañías de Seguros
Colombia
www.sistran.com.co
Allrightsreserved.
***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Configuration;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
//using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Helpers
{
    public static class ExtensionHelper
    {

        public static void SetParam(this SqlParameter sqlParam, System.Data.ParameterDirection dir, string Nombre, object Valor)
        {
            sqlParam.Direction = dir;
            sqlParam.ParameterName = Nombre;
            sqlParam.Value = Valor;
        }

        public static Int32 ToInt(this String intval)
        {
            int number;
            bool result = Int32.TryParse(intval, out number);
            if (result)
            {
                return Convert.ToInt32(intval);
            }

            return Int32.MinValue;
        }

        public static Int32 ToInt(this Boolean intval)
        {
            if (intval)
            {
                return 1;
            }

            return 0;

        }

        public static Decimal ToDecimal(this String str)
        {
            decimal de;
            if (Decimal.TryParse(str, out de))
            {
                return Convert.ToDecimal(str);
            }

            return Decimal.MinValue;
        }

        public static DateTime ToDateTime(this String str)
        {
            DateTimeStyles styles = DateTimeStyles.None;
            DateTime dateResult;

            CultureInfo cultura = CultureInfo.CreateSpecificCulture("es-MX");

            if (DateTime.TryParse(str, cultura, styles, out dateResult))
            {
                return Convert.ToDateTime(str, cultura);
            }

            if (str.Equals(string.Empty))
            {
                return new DateTime();
            }
            return DateTime.MinValue;

        }

        public static Boolean ToBoolean(this String str)
        {
            try
            {
                Boolean bresult;

                if (!Int32.MinValue.Equals(str.ToInt()))
                {
                    return str.ToInt().ToBoolean();
                }

                if (Boolean.TryParse(str, out bresult))
                {
                    return Convert.ToBoolean(str);
                }
                else
                {
                    throw new SupException("La cadena no se puede convertir a boleano");
                }
            }
            catch (Exception ex)
            {
                throw new SupException(ex.Message);
            }


        }

        public static Boolean ToBoolean(this Int32 valor)
        {
            try
            {
                if (valor.Equals(1) || valor.Equals(0))
                {
                    return Convert.ToBoolean(valor);
                }
                else
                {
                    throw new SupException("El número no se puede convertir a boleano");
                }

            }
            catch (Exception ex)
            {
                throw new SupException(ex.Message);
            }


        }

        public static Boolean ValidateExistItem<T>(this IList<T> list)
        {
            return (list.Count > 0);
        }

        public static void ValidateErrorSoat(this string Mensaje)
        {
            if (!Mensaje.Equals(string.Empty)) throw new SupException();
        }

        public static void ValidateErrorInsertSoat(this bool value)
        {
            if (!value) throw new SupException();
        }

        public static Boolean IsObjectDefaultDecimal(this object obj)
        {
            return obj.ToString().ToDecimal().Equals(Decimal.MinValue);
        }

        public static Boolean IsObjectDefaultInt(this object obj)
        {
            return obj.ToString().ToInt().Equals(Int32.MinValue);
        }

        public static Boolean IsObjectDefaultDateTime(this object obj)
        {
            return obj.ToString().ToDateTime().Equals(DateTime.MinValue);
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (T element in source) act(element);
            return source;
        }

        public static void Iterate<T>(this IEnumerable<T> enumerable, Action<T> callback)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            IterateHelper(enumerable, (x, i) => callback(x));
        }

        public static void Iterate<T>(this IEnumerable<T> enumerable, Action<T, int> callback)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            IterateHelper(enumerable, callback);
        }

        private static void IterateHelper<T>(this IEnumerable<T> enumerable, Action<T, int> callback)
        {
            int count = 0;
            foreach (var cur in enumerable)
            {
                callback(cur, count);
                count++;
            }
        }


        public static void Update<T>(this DtoMaster dtoMaster,int id_persona,int cod_rol)
        {
            Type type = typeof(T);
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                string FieldName = property.Name;
                //property.SetValue(id_persona, "", null);
            }
        }
    }
}