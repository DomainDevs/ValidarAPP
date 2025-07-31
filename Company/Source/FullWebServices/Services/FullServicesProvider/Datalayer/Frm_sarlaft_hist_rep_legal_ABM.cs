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
    /// Data access layer class for Frm_sarlaft_hist_rep_legal
    /// </summary>
    class Frm_sarlaft_hist_rep_legal_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_hist_rep_legal_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_hist_rep_legal_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_hist_rep_legal_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Frm_sarlaft_hist_rep_legal businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_hisrepleg_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_rep_legal", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_rep_legal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nacimiento", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nacimiento, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_nacimi", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_nacimi)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nacionalidad", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nacionalidad)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_expedicion_doc", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_expedicion_doc, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_expedicion", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_expedicion)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_ciudad", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_ciudad)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_celular", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_celular)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_email", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_facultades", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_facultades)));
                sqlCommand.Parameters.Add(new AseParameter("@vr_facultades", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.vr_facultades)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cargo", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cargo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_unidad", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_unidad)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modifica", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modifica, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_modifica", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_modifica)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_hist_rep_legal::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_hist_rep_legal businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_hisrepleg_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_rep_legal", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_rep_legal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nacimiento", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nacimiento, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_nacimi", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_nacimi)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nacionalidad", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nacionalidad)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_expedicion_doc", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_expedicion_doc, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_expedicion", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_expedicion)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_ciudad", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_ciudad)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_celular", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_celular)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_email", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_facultades", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_facultades)));
                sqlCommand.Parameters.Add(new AseParameter("@vr_facultades", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.vr_facultades)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cargo", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cargo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_unidad", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_unidad)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modifica", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modifica, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_modifica", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_modifica)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_hist_rep_legal::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_hist_rep_legal businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_hisrepleg_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_rep_legal", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_rep_legal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nacimiento", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nacimiento, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_nacimi", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_nacimi)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nacionalidad", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nacionalidad)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_expedicion_doc", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_expedicion_doc, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_expedicion", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_expedicion)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_ciudad", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_ciudad)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_celular", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_celular)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_email", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_facultades", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_facultades)));
                sqlCommand.Parameters.Add(new AseParameter("@vr_facultades", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.vr_facultades)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cargo", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cargo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_unidad", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_unidad)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modifica", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modifica, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_modifica", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_modifica)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_hist_rep_legal::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_hist_rep_legal businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.id_persona.ToString()));

            businessObject.nro_doc_rep_legal = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.nro_doc_rep_legal.ToString()));

            businessObject.cod_tipo_doc = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.cod_tipo_doc.ToString()));

            businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_nombre.ToString()));

            businessObject.fec_nacimiento = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.fec_nacimiento.ToString()));

            businessObject.txt_lugar_nacimi = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_lugar_nacimi.ToString()));

            businessObject.txt_nacionalidad = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_nacionalidad.ToString()));

            businessObject.fec_expedicion_doc = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.fec_expedicion_doc.ToString()));

            businessObject.txt_lugar_expedicion = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_lugar_expedicion.ToString()));

            businessObject.txt_ciudad = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_ciudad.ToString()));

            businessObject.txt_telefono = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_telefono.ToString()));

            businessObject.txt_celular = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_celular.ToString()));

            businessObject.txt_email = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_email.ToString()));

            businessObject.txt_facultades = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_facultades.ToString()));

            businessObject.vr_facultades = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.vr_facultades.ToString()));

            businessObject.txt_cargo = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.txt_cargo.ToString()));

            businessObject.cod_unidad = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.cod_unidad.ToString()));

            businessObject.fec_modifica = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.fec_modifica.ToString()));

            businessObject.usuario_modifica = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_hist_rep_legal.Frm_sarlaft_hist_rep_legalFields.usuario_modifica.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_hist_rep_legal</returns>
        internal List<Frm_sarlaft_hist_rep_legal> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_hist_rep_legal> list = new List<Frm_sarlaft_hist_rep_legal>();

            while (dataReader.Read())
            {
                Frm_sarlaft_hist_rep_legal businessObject = new Frm_sarlaft_hist_rep_legal();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
