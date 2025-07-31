/***********************************************************************************************
COPYRIGHTNOTICE:
©2003-2013 SISTRAN 
soluciones de Software para Compañías de Seguros
Colombia
www.sistran.com.co
Allrightsreserved.
***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;

namespace ExtensionMethods
{
    public static class ListConvert<T> where T : new()
    {

        public static List<T> DataTableToList(DataTable dt, ArrayList ListExcepctionFields)
        {
            List<T> lista = new List<T>();

            T elemento;

            if (ListExcepctionFields == null) 
            {
                ListExcepctionFields = new ArrayList();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Type itemsType = typeof(T);
                elemento = new T();

                foreach (PropertyInfo prop in itemsType.GetProperties())
                {
                    string FieldName = prop.Name;
                    if ( !ListExcepctionFields.Contains(FieldName.ToLower()))
                    {
                        if (dt.Rows[i][prop.Name] == DBNull.Value)
                        {
                            switch (dt.Rows[i][prop.Name].GetType().FullName)
                            {
                                case "System.String":
                                    prop.SetValue(elemento, "", null);
                                    break;
                                case "System.Decimal":
                                    prop.SetValue(elemento, Decimal.MinValue, null);
                                    break;
                                case "System.Int32":
                                    prop.SetValue(elemento, Int32.MinValue, null);
                                    break;
                                case "System.Boolean":
                                    prop.SetValue(elemento, false, null);
                                    break;

                            }
                        }
                        else
                        {
                            switch (prop.PropertyType.Name) 
                            {
                                case "String":
                                    prop.SetValue(elemento, dt.Rows[i][prop.Name].ToString(), null);
                                    break;
                                case "Decimal":
                                    prop.SetValue(elemento, dt.Rows[i][prop.Name].ToString().ToDecimal(), null);
                                    break;
                                case "Int32":
                                    prop.SetValue(elemento, dt.Rows[i][prop.Name].ToString().ToInt(), null);
                                    break;
                                case "Boolean":
                                    prop.SetValue(elemento, dt.Rows[i][prop.Name].ToString().ToBoolean(), null);
                                    break;
                            }
                            //prop.SetValue(elemento, dt.Rows[i][prop.Name], null);
                        }
                    }
                }
                lista.Add(elemento);
            }
            return lista;
        }


        /// <summary>
        /// Convierto una entidad a un List<entidadesz></entidadesz>
        /// </summary>
        /// <param name="dt">DataTable a convertir</param>
        /// <returns>List<entidades></entidades></returns>
        public static List<T> DataTableToList(DataTable dt)
        {
            string propiedad = null;
            try
            {
                List<T> lista = new List<T>();
                T elemento;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Type itemsType = typeof(T);
                    elemento = new T();

                    foreach (PropertyInfo prop in itemsType.GetProperties())
                    {
                        propiedad = prop.Name;
                        if (dt.Rows[i][propiedad] == DBNull.Value)
                        {
                            switch (dt.Rows[i][prop.Name].GetType().FullName)
                            {
                                case "System.String":
                                    prop.SetValue(elemento, "", null);
                                    break;
                                case "System.Decimal":
                                    prop.SetValue(elemento, Decimal.MinValue, null);
                                    break;
                                case "System.Int32":
                                    prop.SetValue(elemento, Int32.MinValue, null);
                                    break;
                                case "System.Boolean":
                                    prop.SetValue(elemento, false, null);
                                    break;
                            }
                        }
                        else
                        {
                            prop.SetValue(elemento, dt.Rows[i][propiedad], null);
                        }
                    }
                    lista.Add(elemento);
                }
                return lista;
            }
            catch (Exception ex)
            {
                string msg = "ConvertidorListas: " + propiedad + " " + ex.Message;
                throw new SupException(msg);
            }
        }

    }
}
