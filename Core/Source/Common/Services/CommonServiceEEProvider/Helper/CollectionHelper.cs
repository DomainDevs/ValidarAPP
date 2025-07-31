using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Sistran.Core.Application.CommonServices.EEProvider.Helper
{
    public class CollectionHelper
    {
        /// <summary>
        /// Constructor de CollectionHelper
        /// </summary>
        private CollectionHelper()
        {
        }

        /// <summary>
        /// Convierte una lista a datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">La lista</param>
        /// <returns></returns>
        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            if (list != null)
            {
                foreach (T item in list)
                {
                    DataRow row = table.NewRow();

                    foreach (PropertyDescriptor prop in properties)
                    {
                        if ((prop.PropertyType.Name == "Nullable`1"))
                        {
                            row[prop.Name] = DBNull.Value;
                        }
                        else
                        {
                            row[prop.Name] = prop.GetValue(item);
                        }
                    }

                    table.Rows.Add(row);
                }

                return table;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// convierte DataRows en IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows">Las Filas</param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Convierte un DataTable a Ilist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">Objeto DataTable</param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(DataTable table)
        {

            if (table == null)
            {
                return null;
            }
            List<T> rows = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                rows.Add(CreateItem<T>(row));
            }
            return rows;
        }

        /// <summary>
        /// Crea un Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);

                    if (prop == null)
                    {
                        continue;
                    }
                    try
                    {
                        object value = CastPropertyValue(prop, row[column.ColumnName].ToString());
                        prop.SetValue(obj, value, null);
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException("error en CreateItem",ex);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Crea un Objeto DataTable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {

                if (!(prop.PropertyType.Name == "Nullable`1"))
                {
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                else
                {
                    table.Columns.Add(prop.Name, typeof(System.Nullable));
                }


            }

            return table;
        }

        /// <summary>
        /// Convierte el valor de la propiedad.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object CastPropertyValue(PropertyInfo property, string value)
        {
            if (property == null || String.IsNullOrEmpty(value))
            {
                return null;
            }
            if (property.PropertyType == typeof(string))
            {
                return Convert.ToString(value);
            }
            if (property.PropertyType.IsEnum)
            {
                Type enumType = property.PropertyType;
                if (Enum.GetUnderlyingType(enumType).Name == "Int32")
                {
                    if (Enum.IsDefined(enumType, Int32.Parse(value)))
                    {
                        return Enum.Parse(enumType, value);
                    }
                }
                else if (Enum.IsDefined(enumType, value))
                {
                    return Enum.Parse(enumType, value);
                }
            }
            if (property.PropertyType == typeof(bool))
            {
                return value == "1" || value == "true" || value == "True" || value == "on" || value == "checked";
            }
            else if (property.PropertyType == typeof(Uri))
            {
                return new Uri(Convert.ToString(value));
            }
            else if (property.PropertyType == typeof(Int32))
            {
                return Convert.ToInt32(value);
            }
            else if (property.PropertyType == typeof(double))
            {
                return Convert.ToDouble(value);
            }
            else if (property.PropertyType == typeof(int?))
            {
                return  Convert.ToInt32(value);
            }
            else if (property.PropertyType == typeof(Int32?))
            {
                return  Convert.ToInt32(value);
            }
            else if (property.PropertyType == typeof(double?))
            {
                return Convert.ToDouble(value);
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                try
                {
                    return Convert.ToDateTime(value);
                }
                catch
                {
                    return  null;
                }
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return Convert.ChangeType(value, property.PropertyType);
            }
        }

        /// <summary>
        /// Convierte el BusinessCollection a un Ilist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static IList<T> ConvertDAFTo<T>(BusinessCollection businessCollection)
        {
            IList<T> list = new List<T>();
            foreach (BusinessObject item in businessCollection)
            {
                T newItem = CreateItemDAF<T>(item);
                list.Add(newItem);
            }
            return list;
        }

        /// <summary>
        /// Convierte el IDataReader a un IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static IList<T> ConvertDAFTo<T>(IDataReader reader)
        {
            IList<T> list = new List<T>();
            while (reader.Read())
            {
                T newItem = CollectionHelper.CreateItemDAF<T>(reader);
                list.Add(newItem);
            }
            return list;
        }

        /// <summary>
        /// Crea un ItemDAF de un BusinessObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static T CreateItemDAF<T>(BusinessObject item)
        {
            T obj = default(T);

            if (item != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo propModel in obj.GetType().GetProperties())
                {
                    object val = item.GetOriginalPropertyValues()[propModel.Name];
                    if (val == null)
                    {
                        val = item.GetProperties()[propModel.Name];
                    }
                    propModel.SetValue(obj, val, null);
                }
            }
            return obj;
        }

        /// <summary>
        /// create un ItemDAF de un IDataReader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static T CreateItemDAF<T>(IDataReader reader)
        {
            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            int index = -1;
            foreach (PropertyInfo propModel in obj.GetType().GetProperties())
            {
                index = reader.GetOrdinal(propModel.Name);
                if (index >= 0)
                {
                    propModel.SetValue(obj, reader.GetValue(index), null);
                }
            }
            return obj;
        }
    }
}
