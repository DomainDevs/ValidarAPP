using Sistran.Core.Application.Utilities.Models;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

namespace Sistran.Core.Application.Utilities.Helper
{
    public class EnumHelper
    {

        public static ConcurrentDictionary<string, EnumParameter> enumParameterCache = new ConcurrentDictionary<string, EnumParameter>();

        /// <summary>
        /// Retorna la descripcion de un Enum
        /// [Description("Coaseguro")]
        /// Coinsurance = 2,
        /// </summary>
        /// <param name="en">Enum</param>
        /// <returns>retorna la descripcion del Enum</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
        public static string GetItemName<T>(object value)
        {
            string ItemName = Enum.GetName(typeof(T), value);
            return ItemName;
        }

        /// <summary>
        /// Metodo encargado de devolver el enum segun el id solicitado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetItemNameParameterValue<T>(object value) where T : struct
        {
            var unionEnumCache = enumParameterCache.ToArray().Where(x => typeof(T).GetFields().Select(z => z.Name).ToArray().Contains(x.Key));
            string descriptionEnum = unionEnumCache.Where(z => z.Value.Value.ToString() == value.ToString()).FirstOrDefault().Key;

            if (descriptionEnum == null)
            {
                throw new ArgumentException("No existe información para el valor: "+ value.ToString());
            }

            var enumerator = typeof(T).GetEnumValues().GetEnumerator();
            while (enumerator.MoveNext())
            {
                T valor = (T)enumerator.Current;
                if (valor.ToString().Equals(descriptionEnum))
                {
                    return valor;
                }
            }

            throw new ArgumentException("No se encontro "+ descriptionEnum + " en los parametros de cache");
        }

        public static object GetEnumParameterValue<T>(T keys) where T : struct
        {
            EnumParameter enumparameter = new EnumParameter();
            var key = keys.ToString();
            if (enumParameterCache.ContainsKey(key))
            {
                EnumParameter enumparameterOut;
                enumParameterCache.TryGetValue(key, out enumparameterOut);
                enumparameter = enumparameterOut;
            }
            else
            {
                throw new ArgumentException("No se encontro el parametro.");
            }
            return enumparameter.Value;
        }
    }
}
