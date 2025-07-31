using Sistran.Core.Application.Utilities.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.Utilities.Helper
{
    /// <summary>
    /// Extension Propiedades
    /// </summary>
    public static class ExtensionHelper
    {
        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        #region  Convertir  Consulta Store Por alias
        /// <summary>
        /// Retorna Una Lista T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <returns>List<T></returns>
        public static List<T> ToListDynamic<T>(this DataTable dataTable)
        {
            object obj = new object();

            var columnNames = dataTable.Columns.Cast<DataColumn>()
                .Where(c => !c.ColumnName.Contains("."))
                .Select(c => c.ColumnName)
                .ToList();
            var subColumnNames = dataTable.Columns.Cast<DataColumn>()
               .Where(c => c.ColumnName.Contains("."))
               .Select(c => c.ColumnName)
               .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var SubObjectProperties = SubProperties<T>(objectProperties, new ConcurrentDictionary<PropertyInfo, Object>(), 0);
            List<T> target = new List<T>();
            //SubPropiedes
            Parallel.ForEach(dataTable.AsEnumerable(), DebugParalel(), (dataRow) =>
             {
                 var item = Activator.CreateInstance<T>();
                 Parallel.ForEach(SubObjectProperties, DebugParalel(), (subObjectPropertie) =>
                   {
                       var subProperty = subObjectPropertie.Value.GetType().GetProperties();
                       if (subObjectPropertie.Key.PropertyType.Name != "String" && subObjectPropertie.Key.PropertyType.IsClass && !subObjectPropertie.Key.PropertyType.FullName.Contains("System") && !subObjectPropertie.Key.PropertyType.IsPrimitive)
                       {
                           subObjectPropertie.Key.SetValue(item, LoadProperties(subObjectPropertie.Key.PropertyType, dataRow), null);
                       }
                       else
                       {
                           Parallel.ForEach(subProperty.Where(properti => subColumnNames.Contains(subObjectPropertie.Key.Name + "." + properti.Name) && dataRow[subObjectPropertie.Key.Name + "." + properti.Name] != DBNull.Value), DebugParalel(), (property) =>
                            {
                                try
                                {
                                    subObjectPropertie.Value.SetPropertyValue(property.Name, dataRow[subObjectPropertie.Key.Name + "." + property.Name]);
                                }
                                catch (Exception)
                                {

                                    property.SetValue(item, default(T), null);
                                }


                            });
                       }

                   });

                 Parallel.ForEach(objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value), DebugParalel(), (propertie) =>
                 {
                     propertie.SetValue(item, dataRow[propertie.Name], null);
                 });
                 lock (obj)
                 {
                     target.Add(item);
                 }
             });

            return target;
        }
        /// <summary>
        /// Obtener Subpropiedades Recursivament hasta n nivel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfos">Propiedades Lista</param>
        /// <param name="properties">Propiedades</param>
        /// <param name="level">Nivel Inicia 0</param>
        /// <returns></returns>
        public static ConcurrentDictionary<PropertyInfo, Object> SubProperties<T>(PropertyInfo[] propertyInfos, ConcurrentDictionary<PropertyInfo, Object> properties, int level)
        {
            if (level > PropertyType.MaxLevel)
            {
                return properties;
            }
            TP.Parallel.ForEach(propertyInfos, x =>
            {
                if (x.PropertyType.Name != "String" && x.PropertyType.IsClass && !x.PropertyType.FullName.Contains("System"))
                {
                    Type tipo = Type.GetType(x.PropertyType.AssemblyQualifiedName);
                    if (tipo != null)
                    {
                        var obj = Activator.CreateInstance(tipo);
                        properties[x] = obj;
                        level = level + 1;
                        //SubProperties<T>(obj.GetType().GetProperties(flags), properties, level);
                    }

                }
            });
            return properties;
        }


        /// <summary>
        /// Setear el valor a una Propiedad
        /// </summary>
        /// <param name="pObject">The p object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">Propiedad Inexistente</exception>
        public static void SetPropertyValue(this object pObject, string propertyName, object value)
        {
            PropertyInfo property = pObject.GetType().GetProperty(propertyName);
            if (property != null)
            {
                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                property.SetValue(pObject, safeValue, null);
            }
            else
            {
                throw new ArgumentException("Propiedad Inexistente");
            }
        }
        /// <summary>
        /// Cargar las propeidades de una clase recursivamente
        /// </summary>
        /// <param name="valType">Tipo de Objecto.</param>
        /// <param name="dataRow">Filas o registros con los datos</param>
        /// <returns></returns>
        private static object LoadProperties(Type valType, DataRow dataRow)
        {
            var Instance = Activator.CreateInstance(valType);
            var properties = Instance.GetType().GetProperties();
            Parallel.ForEach(properties, DebugParalel(), x =>
            {
                if (x.PropertyType.Name != "String" && x.PropertyType.IsClass && !x.PropertyType.IsPrimitive && !x.PropertyType.FullName.Contains("System"))
                {
                    x.SetValue(Instance, LoadProperties(x.PropertyType, dataRow), null);
                }
                else
                {
                    if (dataRow.Table.Columns.Contains(valType.Name + "." + x.Name))
                    {
                        x.SetValue(Instance, dataRow[valType.Name + "." + x.Name], null);

                    }
                }

            });

            return Instance;
        }
        #endregion
        #region debug      
        public static ParallelQuery<TSource> AsDebugFriendlyParallel<TSource>(this IEnumerable<TSource> source)
        {
            var pQuery = source.AsParallel();
#if DEBUG
            pQuery = pQuery.WithDegreeOfParallelism(1);
#endif

            return pQuery;
        }

        public static ParallelOptions DebugParalel()
        {
            var paralelOptions = new ParallelOptions();
#if DEBUG
            paralelOptions.MaxDegreeOfParallelism = 1;
#endif
            return paralelOptions;
        }
        #endregion

    }
}
