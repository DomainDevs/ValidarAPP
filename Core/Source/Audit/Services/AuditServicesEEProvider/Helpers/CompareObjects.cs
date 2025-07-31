namespace Sistran.Core.Application.AuditServices.EEProvider.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ObjectHelper<T> where T : class
    {
        static readonly EventLog ev = new EventLog();
        /// <summary>
        /// Comparar Dos Objetos
        /// </summary>
        /// <param name="x">Clase Inicial</param>
        /// <param name="y">Clase Final</param>
        /// <param name="ignoreList">Propiedades de la Clase a ignorar</param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, T> Compare(T x, T y, params string[] ignoreList)
        {
            if (x != null && y != null && x.GetType().Name == y.GetType().Name)
            {
                ConcurrentDictionary<string, T> diffProperties = new ConcurrentDictionary<string, T>();
                Type type = typeof(T);
                if (type != null)
                {
                    PropertyInfo<T>(x, y, type, ref diffProperties, ignoreList);
                }
                return diffProperties;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Leer Propiedadaes
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="x">Valor Inicial</param>
        /// <param name="y">Valor Final</param>
        /// <param name="type">Tipo</param>
        /// <param name="diffProperties">Variable guardar Diferencias</param>
        /// <param name="ProductDic">Diccionario Propiedades</param>
        /// <param name="ignoreList">Lista Con Propiedades a ignorar</param>
        private static void PropertyInfo<S>(S x, S y, Type type, ref ConcurrentDictionary<string, T> diffProperties, params string[] ignoreList)
        {
            try
            {

                if (x.GetType().Name == "List`1")
                {
                    IList item = (IList)x;
                    IList ItemNew = (IList)y;
                    if (item != null && ItemNew != null)
                    {
                        if (!item.Equals(ItemNew))
                        {
                            for (int i = 0; i < ItemNew.Count; i++)
                            {

                                PropertyInfo(item[i], ItemNew[i], item[i].GetType(), ref diffProperties, ignoreList);
                            }
                        }

                    }
                }
                else if (x != null & y == null)
                {
                    if (!x.Equals(y))
                    {
                        diffProperties.TryAdd(x.GetType().Name, default(T));
                    }
                }
                else
                {

                    foreach (PropertyInfo property in type.GetProperties().Where(p => p.MemberType == MemberTypes.Property && p.CanRead && !ignoreList.Contains(p.Name) && p.Name != "MemberType"))
                    {
                        int compareValue = -1;
                        if (property.PropertyType.Name == "List`1")
                        {
                            if (x != null && y != null)
                            {
                                IList item = (IList)property.GetValue(x, null);
                                IList ItemNew = (IList)property.GetValue(y, null);
                                if (item != null && ItemNew != null)
                                {
                                    if (!item.Equals(ItemNew))
                                    {
                                        if (ItemNew != null && ItemNew.Count > 0)
                                        {
                                            for (int i = 0; i < ItemNew.Count; i++)
                                            {
                                                if (!(i < item.Count) && ItemNew[i] != null)
                                                {
                                                    try
                                                    {
                                                        StringBuilder sb = new StringBuilder();
                                                        if (ItemNew[i].GetType().IsClass && ItemNew[i].GetType().Name != typeof(string).Name && ItemNew[i].GetType().Name != "System")
                                                        {
                                                            foreach (PropertyInfo p in ItemNew[i].GetType().GetProperties().Select(p => p).ToList())
                                                            {
                                                                sb.Append(String.Format("Id: {0}, value: {1}", p.Name, p.GetValue(ItemNew[i], null)).ToString());
                                                            }
                                                        }
                                                        diffProperties.TryAdd(property.Name, default(T));

                                                    }
                                                    catch (Exception)
                                                    {
                                                        ev.WriteEntry("Validar");
                                                    }
                                                }
                                                else
                                                {
                                                    PropertyInfo(item[i], ItemNew[i], item[i].GetType(), ref diffProperties, ignoreList);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (property.PropertyType.IsClass && System.Type.GetTypeCode(property.PropertyType) == TypeCode.Object && property.Name != "MemberType" && !property.PropertyType.FullName.Contains("System.String") && !ignoreList.Contains(property.PropertyType.Name) && property.Name != "List`1")
                        {

                            if (property.GetValue(x, null) != null)
                            {
                                if (!property.GetValue(x, null).Equals(property.GetValue(y, null)))
                                {
                                    PropertyInfo(property.GetValue(x, null), property.GetValue(y, null), property.GetValue(x, null).GetType(), ref diffProperties, ignoreList);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            IComparable valx = null;
                            object valy = null;
                            if (x != null)
                            {
                                valx = property.GetValue(x, null) as IComparable;
                                if (valx == null)
                                {
                                    bool a = property.GetValue(x, null) is IComparable;
                                    if (!a)
                                    {
                                        if (property.GetValue(x, null) != null && property.GetValue(y, null) != null && !(property.GetValue(x, null).Equals(property.GetValue(y, null))))
                                        {

                                            diffProperties.TryAdd(x.GetType().Name, default(T));
                                            continue;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            if (y != null)
                            {
                                valy = property.GetValue(y, null);
                            }
                            if (valx != null && valy != null)
                            {
                                compareValue = valx.CompareTo(valy);
                            }
                            else if ((valx == null && valy != null))
                            {
                                compareValue = 1;
                            }
                            else if (valx == null && valy == null)
                            {
                                compareValue = 0;
                            }
                            if (compareValue != 0)
                            {

                                diffProperties.TryAdd(x.GetType().Name, default(T));
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Clonar propiedades por Valor
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static U CloneAndUpcast<U>(U b) where U : new()
        {
            U clone = new U();
            if (b.GetType().Name != "List`1")
            {
                clone = (U)Activator.CreateInstance(b.GetType());
            }
            try
            {
                var underlyingType = Nullable.GetUnderlyingType(b.GetType()) ?? b.GetType();
                Convert.ChangeType(clone, underlyingType);

            }
            catch (InvalidCastException)
            {

                clone = default(U);
            }
            if (clone != null)
            {

                for (int i = 0; i < b.GetType().GetProperties().Where(p => p.MemberType == MemberTypes.Property && p.CanRead && p.Name != "MemberType").Count(); i++)
                {

                    if (b.GetType().GetProperties()[i].GetValue(b, null) != null && b.GetType().GetProperties()[i].GetValue(b, null).GetType().Name == "List`1")
                    {
                        clone
                        .GetType()
                        .GetProperty(b.GetType().GetProperties()[i].Name)
                        .SetValue(clone, b.GetType().GetProperties()[i].GetValue(b, null), null);
                    }
                    else
                    {
                        clone
                        .GetType()
                        .GetProperty(b.GetType().GetProperties()[i].Name)
                        .SetValue(clone, b.GetType().GetProperty(b.GetType().GetProperties()[i].Name).GetValue(b, null), null);

                    }
                }
            }
            return clone;
        }

        /// <summary>
        /// Clonar Objetos
        /// </summary>
        /// <typeparam name="T">tipo Objeto a Clonar</typeparam>
        /// <param name="obj">Objeto a Clonar</param>
        /// <returns>Nueva referencia del Objeto</returns>
        public static T CloneObject(T obj)
        {
            if (obj == null) return null;
            System.Reflection.MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                return (T)inst.Invoke(obj, null);
            else
                return null;
        }
    }
}
