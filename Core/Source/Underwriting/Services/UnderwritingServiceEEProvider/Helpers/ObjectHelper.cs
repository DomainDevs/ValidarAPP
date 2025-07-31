using System;
using System.Collections;
using System.Reflection;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Helpers
{
    /// <summary>
    /// Comparar dos Objetos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ObjectHelper<T>
    {
        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="ignoreList">The ignore list.</param>
        /// <returns></returns>
        public static IList Compare(T x, T y, params string[] ignoreList)
        {
            if (x != null && y != null)
            {
                IList diffProperties = new ArrayList();
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public);
                FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public);
                int compareValue = 0;
                foreach (PropertyInfo property in properties)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        Console.Write("a");
                    }
                    IComparable valx = property.GetValue(x, null) as IComparable;
                    if (valx == null)
                        continue;
                    object valy = property.GetValue(y, null);
                    compareValue = valx.CompareTo(valy);
                    if (compareValue != 0)
                        diffProperties.Add(property.Name);
                }
                foreach (FieldInfo field in fields)
                {
                    IComparable valx = field.GetValue(x) as IComparable;
                    if (valx == null)
                        continue;
                    object valy = field.GetValue(y);
                    compareValue = valx.CompareTo(valy);
                    if (compareValue != 0)
                        diffProperties.Add(field.Name);
                }

                return diffProperties;
            }
            else
            {
                return null;
            }
        }
    }
}
