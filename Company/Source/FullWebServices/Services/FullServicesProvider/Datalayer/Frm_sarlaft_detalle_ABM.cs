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
    /// Data access layer class for Frm_sarlaft_detalle
    /// </summary>
    class Frm_sarlaft_detalle_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_detalle_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_detalle_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_detalle_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Frm_sarlaft_detalle businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_detalle_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaa_formulario", AseDbType.Integer, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaa_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_formulario", AseDbType.Double, 6, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@origen", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.origen)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_diligenciamiento", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_diligenciamiento, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_verifica", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_verifica, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usuario_veri", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usuario_veri)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_sise", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_sise)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usuario_auto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usuario_auto)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_aprobacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_aprobacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_detalle::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_detalle businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_detalle_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaa_formulario", AseDbType.Integer, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaa_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_formulario", AseDbType.Double, 6, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@origen", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.origen)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_diligenciamiento", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_diligenciamiento, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_verifica", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_verifica, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usuario_veri", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usuario_veri)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_sise", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_sise)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usuario_auto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usuario_auto)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_aprobacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_aprobacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_detalle::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_detalle businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_detalle_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaa_formulario", AseDbType.Integer, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaa_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_formulario", AseDbType.Double, 6, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@origen", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.origen)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_diligenciamiento", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_diligenciamiento, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_verifica", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_verifica, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usuario_veri", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usuario_veri)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_sise", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_sise)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usuario_auto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usuario_auto)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_aprobacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_aprobacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_detalle::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_detalle businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.id_persona.ToString()));

            businessObject.id_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.id_formulario.ToString()));

            businessObject.aaaa_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.aaaa_formulario.ToString()));

            businessObject.nro_formulario = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.nro_formulario.ToString()));

            businessObject.origen = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.origen.ToString()));

            businessObject.cod_suc = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.cod_suc.ToString()));

            businessObject.fec_diligenciamiento = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.fec_diligenciamiento.ToString()));

            businessObject.fec_verifica = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.fec_verifica.ToString()));

            businessObject.txt_usuario_veri = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.txt_usuario_veri.ToString()));

            businessObject.cod_usuario_sise = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.cod_usuario_sise.ToString()));

            businessObject.txt_usuario_auto = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.txt_usuario_auto.ToString()));

            businessObject.fec_registro = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.fec_registro.ToString()));

            businessObject.fec_aprobacion = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle.Frm_sarlaft_detalleFields.fec_aprobacion.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_detalle</returns>
        internal List<Frm_sarlaft_detalle> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_detalle> list = new List<Frm_sarlaft_detalle>();

            while (dataReader.Read())
            {
                Frm_sarlaft_detalle businessObject = new Frm_sarlaft_detalle();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
