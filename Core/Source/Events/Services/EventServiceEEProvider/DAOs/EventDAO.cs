using Sistran.Co.Application.Data;
using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model = Sistran.Core.Application.EventsServices.Models;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventDAO
    {
        /// <summary>
        /// Obtiene el nombre de las tablas que cumplan con los parametros
        /// </summary>
        /// <param name="schema">nombre del schema</param>
        /// <param name="table">nombre de la tabla</param>
        /// <returns></returns>
        public List<Model.Objects> GetTablesNames(string schema, string table)
        {
            try
            {
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("schema", schema);
                parameters[1] = new NameValue("table", table);

                DataTable result = null;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("EVE.SP_EVENT_GET_TABLES", parameters);
                }
                ArrayList list = new ArrayList();

                foreach (DataRow item in result.Rows)
                {
                    list.Add((Object[])item.ItemArray);
                }

                return ModelAssembler.CreateListObjects(list);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en:GetTablesNames ", ex);
            }
        }

        /// <summary>
        /// Obtiene el nombre columnas de una tabla que cumplan con los parametros
        /// </summary>
        /// <param name="idTable">id de la tabla a consultar</param>
        /// <param name="column">nombre de la columna</param>
        /// <returns></returns>
        public List<Model.Objects> GetColumnsTableByIdTableColumn(long IdTable, string TableName, string Column)
        {
            try
            {
                NameValue[] parameters = new NameValue[3];
                parameters[0] = new NameValue("idTable", IdTable);
                parameters[1] = new NameValue("TableName", TableName);
                parameters[2] = new NameValue("column", Column);

                DataTable result = null;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("EVE.SP_EVENT_GET_COLUMNS_TABLE", parameters);
                }

                ArrayList list = new ArrayList();

                foreach (DataRow item in result.Rows)
                {
                    list.Add((Object[])item.ItemArray);
                }

                return ModelAssembler.CreateListObjects(list);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en:GetColumnsTableByIdTableColumn ", ex);
            }
        }

        /// <summary>
        /// Obtiene el nombre de los procedimientos almacenados en la BD que cumpla con el parametro
        /// </summary>
        /// <param name="SPName">Nombre del sp a consultar</param>
        /// <returns></returns>
        public List<Model.Objects> GetStoreProceduresNamesBySPName(string schema, string SPName)
        {
            try
            {
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("schema", schema);
                parameters[1] = new NameValue("SPName", SPName);

                DataTable result = null;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("EVE.SP_EVENT_GET_STORE_PROCEDURES", parameters);
                }

                ArrayList list = new ArrayList();

                foreach (DataRow item in result.Rows)
                {
                    list.Add((Object[])item.ItemArray);
                }

                return ModelAssembler.CreateListObjects(list);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetStoreProceduresNamesBySPName ", ex);
            }
        }

        /// <summary>
        /// obtiene la lista de accesos TEMP
        /// </summary>
        /// <returns></returns>
        public List<Model.Objects> GetAccesses()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.Accesses)));

                var lista = new List<Model.Objects>();

                foreach (EVENTEN.Accesses item in businessCollection)
                {
                    if (item.Description != "0" && item.Description != "\0" && item.Description != " ")
                    {
                        lista.Add(new Model.Objects
                        {
                            Description = item.Description,
                            Id = item.AccessId
                        });
                    }
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetAccesses", ex);
            }
        }

        /// <summary>
        /// obtiene los eventos que fueron ejecutados en la pantalla
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdSubmodule">id del submodulo</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="ObjectName">nombre de la pantalla</param>
        /// <param name="IdTemp">id del temporal</param>
        /// <param name="key_1"></param>
        /// <param name="key_2"></param>
        /// <param name="key_3"></param>
        /// <param name="key_4"></param>
        /// <returns></returns>
        public List<Model.EventNotification> GetEventsNotificationByEventsCriteria(Model.EventsCriteria eventsCriteria)
        {
            try
            {
                NameValue[] parameters = new NameValue[9];
                parameters[0] = new NameValue("@MODULE_CD", eventsCriteria.IdModule);
                parameters[1] = new NameValue("@SUBMODULE_CD", eventsCriteria.IdSubmodule);
                parameters[2] = new NameValue("@USER_ID", eventsCriteria.IdUser);
                parameters[3] = new NameValue("@OBJECT_NAME", eventsCriteria.ObjectName);
                parameters[4] = new NameValue("@OPERATION1_ID", eventsCriteria.IdTemp.ToString());
                parameters[5] = new NameValue("@KEY1_FIELD_IN", eventsCriteria.key1, DbType.String);
                parameters[6] = new NameValue("@KEY2_FIELD_IN", eventsCriteria.key2, DbType.String);
                parameters[7] = new NameValue("@KEY3_FIELD_IN", eventsCriteria.key3, DbType.String);
                parameters[8] = new NameValue("@KEY4_FIELD_IN", eventsCriteria.key4, DbType.String);
                DataTable dataReturn;
                List<Model.EventNotification> list = new List<Model.EventNotification>();
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    DataSet Ds = pdb.ExecuteSPDataSet("EVE.CO_SP_EVENT_VALIDATE", parameters);                 

                    if (Ds.Tables.Count == 3)
                    {
                        dataReturn = Ds.Tables[2];
                    }
                    else
                    {
                        dataReturn = Ds.Tables[1];
                    }
                }
                foreach (DataRow item in dataReturn.Rows)
                {
                    var existGroup = list.Where(x =>
                        x.EventId == Convert.ToInt32(item["EVENT_ID"].ToString()) &&
                        x.DescriptionError == item["DESCRIPTION_ERROR_MESSAGE"].ToString() &&
                        x.EnabledStop == Convert.ToBoolean(item["ENABLED_STOP"].ToString())
                    ).FirstOrDefault();

                    if (existGroup != null)
                    {
                        list.Remove(existGroup);
                        existGroup.Count++;
                        list.Add(existGroup);
                    }
                    else
                    {
                        list.Add(new Models.EventNotification
                        {
                            Count = 1,
                            EventId = Convert.ToInt32(item["EVENT_ID"].ToString()),
                            RecordId = Convert.ToInt32(item["RECORD_ID"].ToString()),
                            ResultId = Convert.ToInt32(item["RESULT_ID"].ToString()),
                            DescriptionError = item["DESCRIPTION_ERROR_MESSAGE"].ToString(),
                            EnabledStop = Convert.ToBoolean(item["ENABLED_STOP"].ToString())
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventsNotificationByEventsCriteria", ex);
            }
        }
    }
}
