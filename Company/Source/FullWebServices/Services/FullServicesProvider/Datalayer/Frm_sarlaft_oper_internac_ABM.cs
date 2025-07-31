using System;
using System.Data;
using System.Data.SqlTypes;
using Sybase.Data.AseClient;
using System.Collections.Generic;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;

namespace Sistran.Co.Previsora.Application.FullServices.Models.DataLayer
{
    /// <summary>
    /// Data access layer class for Frm_sarlaft_oper_internac
    /// </summary>
    class Frm_sarlaft_oper_internac_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_oper_internac_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_oper_internac_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_oper_internac_ABM(string Connection, string userId, int AppId, AseCommand Command)
            : base(Connection, userId, AppId, Command)
        {
            // Nothing for now.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// insert new row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true of successfully insert</returns>
        public bool Insert(Frm_sarlaft_oper_internac businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_oper_int_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@consecutivo_oper", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.consecutivo_oper)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_operacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_operacion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_producto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_producto", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais_origen", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais_origen)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_entidad", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_entidad)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_oper_internac::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_oper_internac businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_oper_int_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@consecutivo_oper", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.consecutivo_oper)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_operacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_operacion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_producto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_producto", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais_origen", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais_origen)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_entidad", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_entidad)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_oper_internac::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_oper_internac businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_oper_int_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@consecutivo_oper", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.consecutivo_oper)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_operacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_operacion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_producto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_producto", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais_origen", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais_origen)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_entidad", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_entidad)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_oper_internac::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_oper_internac businessObject, IDataReader dataReader)
        {


            businessObject.id_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.id_formulario.ToString()));

            businessObject.consecutivo_oper = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.consecutivo_oper.ToString()));

            businessObject.cod_tipo_operacion = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.cod_tipo_operacion.ToString()));

            businessObject.cod_tipo_producto = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.cod_tipo_producto.ToString()));

            businessObject.imp_producto = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.imp_producto.ToString()));

            businessObject.cod_moneda = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.cod_moneda.ToString()));

            businessObject.cod_pais_origen = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.cod_pais_origen.ToString()));

            businessObject.txt_entidad = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.txt_entidad.ToString()));

            businessObject.nro_producto = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.nro_producto.ToString()));

            businessObject.cod_dpto = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.cod_dpto.ToString()));

            businessObject.cod_municipio = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_oper_internac.Frm_sarlaft_oper_internacFields.cod_municipio.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_oper_internac</returns>
        internal List<Frm_sarlaft_oper_internac> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_oper_internac> list = new List<Frm_sarlaft_oper_internac>();

            while (dataReader.Read())
            {
                Frm_sarlaft_oper_internac businessObject = new Frm_sarlaft_oper_internac();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
