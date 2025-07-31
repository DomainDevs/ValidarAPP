using System;
using System.Data;
using System.Data.SqlTypes;
using Sybase.Data.AseClient;
using System.Collections.Generic;
using Sistran.Co.Application.Data;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sistran.Co.Previsora.Application.FullServices;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServicesProvider.com.asobancaria.cifinpruebas;
using System.Net;
using System.Configuration;
using Microsoft.Web.Services3.Security.Tokens;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Design;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Net.Mail;
using System.Collections;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer
{

    class DataGenericExecute : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public DataGenericExecute()
        {
            // Nothing for now.
        }

        public DataGenericExecute(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        public DataGenericExecute(string Connection, string userId, int AppId)
            : base(Connection, userId, AppId)
        {
            // Nothing for now.
        }

        #endregion

        public bool ExecuteStoreProcedureScalar(string generic, List<Parameters> listParameter, string Conex)
        {
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            int Return = 0;
            AseCommand sqlCommand = new AseCommand();
            AseConnection conn = new AseConnection();
            conn.ConnectionString = Conex;
            bool _return = false;

            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = conn;
                conn.Open();
                sqlCommand.Parameters.Clear();
                foreach (Parameters item in listParameter)
                {
                    switch (item.ParameterType)
                    {

                        case "AseDbType.Integer":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Integer, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.SmallInt":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallInt, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.Decimal":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Decimal, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.Double":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Double, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.SmallDateTime":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallDateTime, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDateTime(item.Value)));
                            break;
                        case "AseDbType.VarChar":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.VarChar, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToString(item.Value)));
                            break;
                        case "AseDbType.Char":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Char, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToChar(item.Value)));
                            break;
                        case "AseDbType.Bit":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Bit, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBoolean(item.Value)));
                            break;
                    }
                }  
              
                Return = Convert.ToInt32(sqlCommand.ExecuteScalar());
                if (Return == 0)
                    _return = true;
            }
            catch (Exception ex)
            {
                conn.Close();
                sqlCommand.Dispose();
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {             
                conn.Close();
                sqlCommand.Dispose();
            }

            return _return;
        }


        /// <summary>
        /// insert new row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true of successfully insert</returns>
        public DataSet ExecuteStoreProcedure(string generic, List<Parameters> listParameter)
        {
            DataSet response = new DataSet();
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                foreach (Parameters item in listParameter)
                {
                    switch (item.ParameterType)
                    {

                        case "AseDbType.Integer":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Integer, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.SmallInt":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallInt, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.Decimal":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Decimal, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.Double":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Double, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.SmallDateTime":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallDateTime, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDateTime(item.Value)));
                            break;
                        case "AseDbType.VarChar":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.VarChar, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToString(item.Value)));
                            break;
                        case "AseDbType.Char":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Char, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToChar(item.Value)));
                            break;
                        case "AseDbType.Bit":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Bit, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBoolean(item.Value)));
                            break;
                    }
                }

                MainConnection.Open();
                if (generic != "spiu_tvarias_ult_nro")
                {
                    AseDataAdapter sqlAdapter = new AseDataAdapter(sqlCommand);
                    sqlAdapter.Fill(response);
                }
                else
                {
                    int Consecutive = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("Consecutive");
                    DataRow _row = dt.NewRow();
                    _row["Consecutive"] = Consecutive;
                    dt.Rows.Add(_row);
                    response.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                MainConnection.Dispose();
                sqlCommand.Dispose();
            }

            return response;
        }

        /// <summary>
        /// insert new row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true of successfully insert</returns>
        public DataSet ExecuteStoreProcedure(string generic, List<Parameters> listParameter,AseCommand Command)
        {
            DataSet response = new DataSet();
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = Command;
            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                foreach (Parameters item in listParameter)
                {
                    switch (item.ParameterType)
                    {

                        case "AseDbType.Integer":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Integer, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.SmallInt":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallInt, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.Decimal":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Decimal, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.Double":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Double, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.SmallDateTime":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallDateTime, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDateTime(item.Value)));
                            break;
                        case "AseDbType.VarChar":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.VarChar, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToString(item.Value)));
                            break;
                        case "AseDbType.Char":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Char, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToChar(item.Value)));
                            break;
                        case "AseDbType.Bit":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Bit, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBoolean(item.Value)));
                            break;
                    }
                }
                
                if (generic != "spiu_tvarias_ult_nro")
                {
                    AseDataAdapter sqlAdapter = new AseDataAdapter(sqlCommand);
                    sqlAdapter.Fill(response);
                }
                else
                {
                    int Consecutive = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("Consecutive");
                    DataRow _row = dt.NewRow();
                    _row["Consecutive"] = Consecutive;
                    dt.Rows.Add(_row);
                    response.Tables.Add(dt);
                }


            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }         

            return response;
        }

        public int ExecuteStoreProcedureScalar(string generic, List<Parameters> listParameter)
        {
            int Return = 0;
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                foreach (Parameters item in listParameter)
                {
                    switch (item.ParameterType)
                    {

                        case "AseDbType.Integer":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Integer, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.SmallInt":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallInt, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.Decimal":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Decimal, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.Double":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Double, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.SmallDateTime":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallDateTime, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDateTime(item.Value)));
                            break;
                        case "AseDbType.VarChar":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.VarChar, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToString(item.Value)));
                            break;
                        case "AseDbType.Char":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Char, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToChar(item.Value)));
                            break;
                        case "AseDbType.Bit":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Bit, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBoolean(item.Value)));
                            break;
                    }
                }

                MainConnection.Open();
                Return = Convert.ToInt32(sqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }

            return Return;
        }

        public int ExecuteStoreProcedureScalar(string generic, List<Parameters> listParameter, AseCommand Command)
        {
            int Return = 0;
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = Command;
            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                foreach (Parameters item in listParameter)
                {
                    switch (item.ParameterType)
                    {
                        case "AseDbType.Integer":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Integer, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.SmallInt":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallInt, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToInt32(item.Value)));
                            break;
                        case "AseDbType.Decimal":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Decimal, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.Double":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Double, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(item.Value)));
                            break;
                        case "AseDbType.SmallDateTime":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.SmallDateTime, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDateTime(item.Value)));
                            break;
                        case "AseDbType.VarChar":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.VarChar, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToString(item.Value)));
                            break;
                        case "AseDbType.Char":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Char, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToChar(item.Value)));
                            break;
                        case "AseDbType.Bit":
                            sqlCommand.Parameters.Add(new AseParameter("@" + item.Parameter, AseDbType.Bit, item.ParameterSize, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBoolean(item.Value)));
                            break;
                    }
                }
                Return = Convert.ToInt32(sqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            return Return;
        }
        public int GetConsecutive(string nom_tabla)
        {
            int Return = 0;
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = "spiu_tvarias_ult_nro";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@sn_muestra", AseDbType.Double, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDecimal("-1")));
                sqlCommand.Parameters.Add(new AseParameter("@nom_tabla", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, nom_tabla));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro", AseDbType.Integer, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 0));
                MainConnection.Open();
                Return = Convert.ToInt32(sqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }

            return Return;
        }

        public DataTable GetListRolApp(int id_rol)
        {
            DataSet response = new DataSet();
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = "SUP.GET_ROLES_SYSTEM";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@ID_ROLE", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, id_rol));
                AseDataAdapter sqlAdapter = new AseDataAdapter(sqlCommand);
                sqlAdapter.Fill(response);
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }

            return response.Tables[0];
        }


        public DataSet ExecuteStoreProcedure(string generic, int codigo, string parametro)
        {
            DataSet response = new DataSet();
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@" + parametro, AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, codigo));

                AseDataAdapter sqlAdapter = new AseDataAdapter(sqlCommand);
                sqlAdapter.Fill(response);
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }

            return response;
        }
        //SE INSTANCIA LA CLASE PARA INVOCAR EL METODO ToSlug
        FullServicesProviderHelper FullServicesProviderHelper = new FullServicesProviderHelper();


        //SobreCarga del Método ExecuteStoreProcedure
        public DataSet ExecuteStoreProcedure(string generic, string codigo, string parametro)
        {
            DataSet response = new DataSet();
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = generic;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@" + parametro, AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, codigo));

                AseDataAdapter sqlAdapter = new AseDataAdapter(sqlCommand);
                sqlAdapter.Fill(response);
            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }

            return response;
        }




        /// <summary>
        /// insert new row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true of successfully insert</returns>
        public List<ResponseSearch> SearchPerson(ResquestSearch resquestSearch)
        {
            AseCommand sqlCommand = new AseCommand();
            try
            {
                sqlCommand.CommandText = "SUP.GET_PERSON";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = MainConnection;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@ID_ROLE", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, resquestSearch.idRol));
                sqlCommand.Parameters.Add(new AseParameter("@COD_ROLE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, resquestSearch.cod_Rol));
                sqlCommand.Parameters.Add(new AseParameter("@TIPO_DOC", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, resquestSearch.tipo_doc));
                sqlCommand.Parameters.Add(new AseParameter("@NRO_DOC", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, resquestSearch.nro_documento));
                sqlCommand.Parameters.Add(new AseParameter("@NAME", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, resquestSearch.name));
                sqlCommand.Parameters.Add(new AseParameter("@CODUSER", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, resquestSearch.ID_user));
                MainConnection.Open();
                IDataReader dataReader = sqlCommand.ExecuteReader();


                List<ResponseSearch> list = new List<ResponseSearch>();
                int cont = 1;
                while (dataReader.Read())
                {
                    ResponseSearch businessObject = new ResponseSearch();
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("IdRol")))
                        businessObject.IdRol = dataReader.GetInt32(dataReader.GetOrdinal("IdRol"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Rol")))
                        businessObject.Rol = dataReader.GetString(dataReader.GetOrdinal("Rol"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Codigo")))
                        businessObject.Codigo = dataReader.GetString(dataReader.GetOrdinal("Codigo"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("IdPersona")))
                        businessObject.IdPersona = dataReader.GetInt32(dataReader.GetOrdinal("IdPersona"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("TipoDoc")))
                        businessObject.TipoDoc = dataReader.GetString(dataReader.GetOrdinal("TipoDoc"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Documento")))
                        businessObject.Documento = dataReader.GetString(dataReader.GetOrdinal("Documento"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("NombreRS")))
                        businessObject.NombreRS = FullServicesProviderHelper.ToSlug(dataReader.GetString(dataReader.GetOrdinal("NombreRS")));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("TipoPersona")))
                        businessObject.TipoPersona = dataReader.GetString(dataReader.GetOrdinal("TipoPersona"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("CodAbona")))
                        businessObject.CodAbona = dataReader.GetInt32(dataReader.GetOrdinal("CodAbona"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("CodTipoDoc")))
                        businessObject.CodTipoDoc = dataReader.GetInt32(dataReader.GetOrdinal("CodTipoDoc"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Active")))
                        businessObject.Activo = dataReader.GetString(dataReader.GetOrdinal("Active"));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Suc")))
                        businessObject.CodSucursal = dataReader.GetInt32(dataReader.GetOrdinal("Suc"));
                    /*JABM PRE-1204 -- Inconsistencia en la respuesta del Web Service SUP*/
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Apellido1")))
                        businessObject.Apellido1 = FullServicesProviderHelper.ToSlug(dataReader.GetString(dataReader.GetOrdinal("Apellido1")));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Apellido2")))
                        businessObject.Apellido2 = FullServicesProviderHelper.ToSlug(dataReader.GetString(dataReader.GetOrdinal("Apellido2")));
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Nombre")))
                        businessObject.Nombre = FullServicesProviderHelper.ToSlug(dataReader.GetString(dataReader.GetOrdinal("Nombre")));
                    /*FIN JABM PRE-1204 -- Inconsistencia en la respuesta del Web Service SUP*/
                    if (resquestSearch.tipo_doc == 1)
                    {
                        businessObject.NombreRS = null;
                    }
                    else
                    {
                        businessObject.Apellido1 = null;
                        businessObject.Apellido2 = null;
                        businessObject.Nombre = null;
                    }
                    businessObject.Identity = cont;
                    list.Add(businessObject);
                    cont++;
                }

                return list;

            }
            catch (Exception ex)
            {
                throw new SupException("ExecuteStoreProcedure::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        /*********************************************************************************************************
         *                                  conectarHost2Host
        * *******************************************************************************************************/
        public static bool validate()
        {
            //Acá tenemos que poner el certificado
            X509Certificate2 objCert = null;
            X509Chain objChain = new X509Chain();

            string Thumbprintcifin = ConfigurationManager.AppSettings["Thumbprintcifin"];

            X509Store objStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            objStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection objCol = objStore.Certificates.Find(X509FindType.FindByThumbprint, Thumbprintcifin, false);
            if (objCol.Count > 0)
                objCert = objCol[0];

            //Verifico toda la cadena de revocación
            objChain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            objChain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

            //Timeout para las listas de revocación
            objChain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 30);

            //Verificar todo
            objChain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

            //Se puede cambiar la fecha de verificación
            objChain.ChainPolicy.VerificationTime = new DateTime(2010, 10, 1);

            objChain.Build(objCert);

            if (objChain.ChainStatus.Length == 0)
                return false;
            else
            {
                //tabla de fechas maxima cifin
                DataSet ds = new DataSet();
                DatatableToList Dtl = new DatatableToList();
                FullServicesProviderSUP da = new FullServicesProviderSUP();

                ds = da.dateMax();
                List<CIFIN_DATEMAX> listdateMax = new List<CIFIN_DATEMAX>();
                listdateMax = Dtl.ConvertTo<CIFIN_DATEMAX>(ds.Tables[0]);

                int day = listdateMax.Select(x => x.CertDate).First();
                if ((DateTime.Now.AddDays(day) >= objCert.NotAfter))
                {
                    List<Parameters> listParameter = new List<Parameters>();

                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.VarChar";
                    parameter.Parameter = "NAME_PARAMETER";
                    parameter.Value = Convert.ToString("EMAILS_VALID_CER");
                    listParameter.Add(parameter);

                    DataGenericExecute DGE = new DataGenericExecute("SISE2G");
                    ds = DGE.ExecuteStoreProcedure("SUP.GET_SUP_PARAMETRICS", listParameter);
                    List<Object> listMailsDomains = new List<Object>();
                    String[] ListEmail = ds.Tables[0].Rows[0][1].ToString().Split('|');

                    List<MailAddress> lstEmails = new List<MailAddress>();
                    foreach (string em in ListEmail)
                        lstEmails.Add(new MailAddress(em, em));

                    Send_Email(lstEmails);
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public static bool Send_Email(List<MailAddress> lstEmails)
        {
            bool isSend = true;
            string SubjectMessage = "Certificado SUP-CIFIN por Expirar";
            if (lstEmails.Count > 0)
            {
                if (!sendMail(lstEmails, SubjectMessage, "El Certificado del producto SUP-CIFIN esta a punto vencerse valide por favor", null))
                {
                    isSend = false;
                }
            }
            else
            {
                isSend = false;
            }
            return isSend;
        }


        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"80%\">";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td width=\"35%\" style=\"font-family: Courier New, Courier, monospace; font-weight: bold; font-size: small;\">" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td width=\"35%\" style=\"font-family: Courier New, Courier, monospace; font-weight: bold; font-size: small;\">" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        public static bool sendMail(List<MailAddress> To, string SubjectMessage, string BodyMessage, ArrayList FilePathList)
        {
            try
            {
                //System.IO.File.WriteAllText(@"C:\Users\guzmanju\Music\mail.txt", BodyMessage);
                //Objetos SMTP para envio de mensaje
                string SmtpServer = ConfigurationManager.AppSettings.Get("SMTPServer"); //Servidor SMTP.
                int PortNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPServerPort"));
                //Puerto Servidor SMTP.
                string strUserName = ConfigurationManager.AppSettings.Get("SenderEMail"); //UserName
                string strSenderName = ConfigurationManager.AppSettings.Get("SenderName"); //UserName
                string strPassword = ConfigurationManager.AppSettings.Get("SenderEMailPass"); //Password
                string strEmailFrom = ConfigurationManager.AppSettings.Get("EmailFrom");
                bool bolUseDefaultCredential = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("UseDefaultCredential"));
                NetworkCredential Auth = new NetworkCredential(strUserName, strPassword);
                MailAddress From = new MailAddress(strEmailFrom, strSenderName);

                // Se Configura el servidor SMTP.
                SmtpClient SC = new SmtpClient(SmtpServer, PortNum);

                // Se establece en False la Conexión segura ya que el servidor no la soporta.
                SC.EnableSsl = false;
                SC.UseDefaultCredentials = bolUseDefaultCredential;
                // Se Crea el mensaje de email.
                using (MailMessage message = new MailMessage())
                {
                    // Se añade el remitente.
                    message.From = From;

                    // Se añaden los destinatarios.
                    foreach (MailAddress ma in To)
                        message.To.Add(ma);

                    // Se establece el Asunto del email.
                    message.Subject = SubjectMessage;
                    // Se establece el cuerpo del correo.
                    System.Net.Mail.AlternateView htmlView = AlternateView.CreateAlternateViewFromString(BodyMessage, null, "text/html");
                    message.AlternateViews.Add(htmlView);

                    if (FilePathList != null)
                        foreach (Object ObjFilePath in FilePathList)
                            message.Attachments.Add(new Attachment(ObjFilePath.ToString())); // Se agrega el archivo como archivo adjunto.

                    if (SC.UseDefaultCredentials)
                        SC.Credentials = Auth; // Se indican las credenciales para autenticarse.

                    // Se indica el modo de envío.
                    SC.DeliveryMethod = SmtpDeliveryMethod.Network;
                    // Se envia el email.
                    SC.Send(message);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static InformacionComercialWSServiceWse conectarHost2Host()
        {
            InformacionComercialWSServiceWse infComercialWse = new InformacionComercialWSServiceWse();
            try
            {
                /**********************************************************************
                 * ESTABLECER CONEXION CON WS
                 **********************************************************************/

                /*Creación de objeto de conexión con el WS*/

                bool isvalid = validate();


                /*Cargar usuario y clave con la cual se establecerá la conexión*/
                string Usercifin = ConfigurationManager.AppSettings["Usercifin"];
                string Passcifin = ConfigurationManager.AppSettings["Passcifin"];
                string Thumbprintcifin = ConfigurationManager.AppSettings["Thumbprintcifin"];
                string TypeLocationCifin = ConfigurationManager.AppSettings["TypeLocationCifin"];
                infComercialWse.Credentials = new NetworkCredential(Usercifin, Passcifin);

                /**********************************************************************
                 * CARGAR POLITICAS DE SEGURIDAD PARA CONEXIÓN
                 **********************************************************************/

                /*Cargar las politicas de seguridad, el nombre que se ingresa es el que se
                 le coloco al momento de crear el policy por el wizard de WSE 3.0*/
                SecurityToken clientToken = null;
                if (TypeLocationCifin == "LocalMachine")
                    clientToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, Thumbprintcifin, X509FindType.FindByThumbprint);
                else if (TypeLocationCifin == "CurrentUser")
                    clientToken = X509TokenProvider.CreateToken(StoreLocation.CurrentUser, StoreName.My, Thumbprintcifin, X509FindType.FindByThumbprint);

                //SecurityToken clientToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, "CN=cifinpruebas.asobancaria.com, OU=Soporte Tecnologico, O=Previsora Seguros, L=Bogota, S=Cundinamarca, C=CO");

                MessageSignature sig = new MessageSignature(clientToken);

                infComercialWse.RequestSoapContext.Security.Tokens.Add(clientToken);
                infComercialWse.RequestSoapContext.Security.Elements.Add(sig);
                //infComercialWse.SetPolicy("HOST2HOST");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return infComercialWse;
        }


        /*********************************************************************************************************
       *                                    consultaXMLHost2Host
       * *******************************************************************************************************/

        public string consultarInformacionComercial(string tipoIdentificacion, string numeroIdentificacion)
        {
            string resultado = null;

            ParametrosConsultaDTO parametrosDto = new ParametrosConsultaDTO();
            parametrosDto.tipoIdentificacion = tipoIdentificacion;
            parametrosDto.numeroIdentificacion = numeroIdentificacion;
            parametrosDto.motivoConsulta = "22";
            parametrosDto.codigoInformacion = "153";
            return resultado = conectarHost2Host().consultaXml(parametrosDto);
        }



        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(ResponseSearch businessObject, IDataReader dataReader)
        {

            businessObject.IdRol = dataReader.GetInt32(dataReader.GetOrdinal("IdRol"));

            businessObject.Rol = dataReader.GetString(dataReader.GetOrdinal("Rol"));

            businessObject.Codigo = dataReader.GetString(dataReader.GetOrdinal("Codigo"));

            businessObject.IdPersona = dataReader.GetInt32(dataReader.GetOrdinal("IdPersona"));

            businessObject.TipoDoc = dataReader.GetString(dataReader.GetOrdinal("TipoDoc"));

            businessObject.Documento = dataReader.GetString(dataReader.GetOrdinal("Documento"));

            businessObject.NombreRS = dataReader.GetString(dataReader.GetOrdinal("NombreRS"));

            businessObject.Activo = dataReader.GetString(dataReader.GetOrdinal("Active"));
        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_asoc</returns>
        internal List<ResponseSearch> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<ResponseSearch> list = new List<ResponseSearch>();
            int cont = 1;
            while (dataReader.Read())
            {
                ResponseSearch businessObject = new ResponseSearch();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                businessObject.Identity = cont;
                list.Add(businessObject);
                cont++;
            }
            return list;

        }


        public TDB GetTDB(string name_table)
        {

            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SUP.GET_DATABASE_TABLE";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {

                sqlCommand.Parameters.Add(new AseParameter("@NAME_TABLE", name_table));
                MainConnection.Open();

                IDataReader dataReader = sqlCommand.ExecuteReader();

                TDB tdb = new TDB();

                while (dataReader.Read())
                {
                    tdb.NameTable = dataReader.GetString(dataReader.GetOrdinal("NAME_TABLE"));
                    tdb.Prefix = dataReader.GetString(dataReader.GetOrdinal("PREFIX"));
                }

                return tdb;
            }
            catch (Exception ex)
            {
                throw new SupException("SUP.GET_DATABASE_TABLE::Error occured.", ex);
            }
            finally
            {

                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        public bool GetStatusAplication(StatusAplication statusaplication)
        {

            bool bstatus = false;
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SUP.GET_STATUS_APLICATION";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {

                sqlCommand.Parameters.Add(new AseParameter("@ID_APLICATION", statusaplication.IdAplication));
                sqlCommand.Parameters.Add(new AseParameter("@KEY_APLICATION", statusaplication.KeyAplication));
                MainConnection.Open();

                bstatus = Convert.ToBoolean(sqlCommand.ExecuteScalar());
                return bstatus;
            }
            catch (Exception ex)
            {
                throw new SupException("SUP.GET_STATUS_APLICATION::Error occured.", ex);
            }
            finally
            {

                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Metodo que valida permisos para acceder al servicio web SASS
        /// 
        /// ** Sergio Diaz - (24/Dic/2013)
        /// </summary>
        /// <param name="statusaplication">Informacion de logeo: ID APlicacion y LLave de Aplicacion</param>
        /// <returns>Verdadero si tiene permisos, falso en caso contrario</returns>
        public bool GetStatusSASSAplication(StatusAplication statusaplication)
        {
            bool bstatus = false;
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SASS.GET_STATUS_APLICATION";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {
                sqlCommand.Parameters.Add(new AseParameter("@ID_APLICATION", statusaplication.IdAplication));
                sqlCommand.Parameters.Add(new AseParameter("@KEY_APLICATION", statusaplication.KeyAplication));
                MainConnection.Open();

                bstatus = Convert.ToBoolean(sqlCommand.ExecuteScalar());
                return bstatus;
            }

            catch (Exception ex)
            {
                throw new Exception("SASS.GET_STATUS_APLICATION::Error occured.", ex);
            }

            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        public int GetId(string tableName)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "spiu_tvarias_ult_nro";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {
                sqlCommand.Parameters.Add(new AseParameter("@sn_muestra", -1));
                sqlCommand.Parameters.Add(new AseParameter("@nom_tabla", tableName));

                AseParameter parametroSalida = new AseParameter();
                parametroSalida.ParameterName = "@ult_nro";
                parametroSalida.AseDbType = AseDbType.Integer;
                parametroSalida.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parametroSalida);

                MainConnection.Open();

                sqlCommand.ExecuteNonQuery();

                return Convert.ToInt32(sqlCommand.Parameters["@ult_nro"].Value.ToString());
            }

            catch (Exception ex)
            {
                throw new Exception("dts_spiu_tvarias_ult_nro::Error occured.", ex);
            }

            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        public List<RoleView> GetlistViews(int idRole)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SUP.GET_ROLES_VIEWS";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {

                sqlCommand.Parameters.Add(new AseParameter("@ID_ROLE", idRole));
                MainConnection.Open();

                IDataReader dataReader = sqlCommand.ExecuteReader();

                List<RoleView> ListRoleViews = new List<RoleView>();
                while (dataReader.Read())
                {
                    RoleView roleview = new RoleView();
                    roleview.IdRole = dataReader.GetInt32(dataReader.GetOrdinal("ID_ROLE"));
                    roleview.NameView = dataReader.GetString(dataReader.GetOrdinal("NAME_VIEW"));
                    roleview.Main = dataReader.GetBoolean(dataReader.GetOrdinal("MAIN"));
                    ListRoleViews.Add(roleview);
                }
                return ListRoleViews;
            }
            catch (Exception ex)
            {
                throw new SupException("SUP.GET_ROLES_VIEWS::Error occured.", ex);
            }
            finally
            {

                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        public bool InsertLOG(CIFIN_LOG businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SUP.CIFINLOG_INSERT";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // Use connection object of base class
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.VarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@number_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.number_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@request_date", AseDbType.DateTime, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.request_date)));
                sqlCommand.Parameters.Add(new AseParameter("@user_log", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.user_log)));
                sqlCommand.Parameters.Add(new AseParameter("@IdentificadorLinea", AseDbType.VarChar, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IdentificadorLinea)));
                sqlCommand.Parameters.Add(new AseParameter("@CodigoTipoIndentificacion", AseDbType.VarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CodigoTipoIndentificacion)));
                sqlCommand.Parameters.Add(new AseParameter("@NumeroIdentificacion", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NumeroIdentificacion)));
                sqlCommand.Parameters.Add(new AseParameter("@NombreTitular", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NombreTitular)));
                sqlCommand.Parameters.Add(new AseParameter("@CodigoCiiu", AseDbType.VarChar, 6, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CodigoCiiu)));
                sqlCommand.Parameters.Add(new AseParameter("@NombreCiiu", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NombreCiiu)));
                sqlCommand.Parameters.Add(new AseParameter("@Estado", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Estado)));
                sqlCommand.Parameters.Add(new AseParameter("@Fecha", AseDbType.DateTime, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Fecha)));
                sqlCommand.Parameters.Add(new AseParameter("@Hora", AseDbType.DateTime, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Hora)));
                sqlCommand.Parameters.Add(new AseParameter("@Entidad", AseDbType.VarChar, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Entidad)));
                sqlCommand.Parameters.Add(new AseParameter("@RespuestaConsulta", AseDbType.VarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RespuestaConsulta)));
                sqlCommand.Parameters.Add(new AseParameter("@Nombre", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@Apellido1", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Apellido1)));
                sqlCommand.Parameters.Add(new AseParameter("@Apellido2", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Apellido2)));
                sqlCommand.Parameters.Add(new AseParameter("@CodigoEstado", AseDbType.VarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CodigoEstado)));

                MainConnection.Open();
                sqlCommand.ExecuteReader();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("SUP.CIFINLOG_INSERT::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        public bool InsertError(ERROR_CIFIN businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SUP.CIFINERROR_INSERT";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // Use connection object of base class
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {
                sqlCommand.Parameters.Add(new AseParameter("@CodigoError", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CodigoError)));
                sqlCommand.Parameters.Add(new AseParameter("@MensajeError", AseDbType.VarChar, 250, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MensajeError)));
                sqlCommand.Parameters.Add(new AseParameter("@CodigoInformacion", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CodigoInformacion)));
                sqlCommand.Parameters.Add(new AseParameter("@MotivoConsulta", AseDbType.VarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MotivoConsulta)));
                sqlCommand.Parameters.Add(new AseParameter("@NumeroIdentificacion", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NumeroIdentificacion)));
                sqlCommand.Parameters.Add(new AseParameter("@TipoIdentificacion", AseDbType.VarChar, 6, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TipoIdentificacion)));
                sqlCommand.Parameters.Add(new AseParameter("@Operacion", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Operacion)));

                MainConnection.Open();
                sqlCommand.ExecuteReader();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("SUP.CIFINERROR_INSERT::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }

        public bool UpdateCont(CIFIN_DATEMAX businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand.CommandText = "SUP.CIFIN_DATEMAX_UPDATE";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // Use connection object of base class
            sqlCommand.Connection = MainConnection;
            sqlCommand.Parameters.Clear();
            try
            {
                sqlCommand.Parameters.Add(new AseParameter("@Id", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.Id)));
                sqlCommand.Parameters.Add(new AseParameter("@DateMonthMax", AseDbType.DateTime, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DateMonthMax)));
                sqlCommand.Parameters.Add(new AseParameter("@LeftOver", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LeftOver)));
                sqlCommand.Parameters.Add(new AseParameter("@ContMax", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ContMax)));
                sqlCommand.Parameters.Add(new AseParameter("@SubCon", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SubCon)));

                MainConnection.Open();
                sqlCommand.ExecuteReader();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("SUP.CIFIN_DATEMAX_UPDATE::Update::Error occured.", ex);
            }
            finally
            {
                MainConnection.Close();
                sqlCommand.Dispose();
            }
        }
    }
}
