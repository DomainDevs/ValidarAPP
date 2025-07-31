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
    /// Data access layer class for Log_usuario
    /// </summary>
    class Log_usuario_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Log_usuario_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Log_usuario_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Log_usuario_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Log_usuario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.log_usuario_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1_ant", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1_nue", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2_ant", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2_nue", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_ant", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_nue", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_ant", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_nue", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_ant", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_nue", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac_ant", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac_ant, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac_nue", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac_nue, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ant", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_nue", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_mod", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_mod)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modificacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password_ant", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password_ant, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password_act", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password_act, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector_ant", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector_act", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector_act)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_ant", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_act", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_act)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil_ant", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil_act", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil_act)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_tiquete", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_tiquete)));
                sqlCommand.Parameters.Add(new AseParameter("@menu", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.menu)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Log_usuario::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Log_usuario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.log_usuario_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1_ant", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1_nue", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2_ant", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2_nue", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_ant", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_nue", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_ant", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_nue", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_ant", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_nue", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac_ant", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac_ant, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac_nue", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac_nue, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ant", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_nue", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_mod", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_mod)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modificacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password_ant", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password_ant, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password_act", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password_act, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector_ant", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector_act", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector_act)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_ant", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_act", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_act)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil_ant", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil_act", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil_act)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_tiquete", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_tiquete)));
                sqlCommand.Parameters.Add(new AseParameter("@menu", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.menu)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Log_usuario::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Log_usuario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.log_usuario_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1_ant", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1_nue", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2_ant", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2_nue", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_ant", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_nue", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_ant", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_nue", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_ant", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_nue", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac_ant", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac_ant, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac_nue", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac_nue, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ant", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_nue", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_nue)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_mod", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_mod)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modificacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password_ant", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password_ant, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password_act", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password_act, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector_ant", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector_act", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector_act)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_ant", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_act", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_act)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil_ant", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil_ant)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil_act", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil_act)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_tiquete", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_tiquete)));
                sqlCommand.Parameters.Add(new AseParameter("@menu", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.menu)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Log_usuario::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Log_usuario businessObject, IDataReader dataReader)
        {


            businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_usuario.ToString()));

            businessObject.txt_apellido1_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.txt_apellido1_ant.ToString()));

            businessObject.txt_apellido1_nue = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.txt_apellido1_nue.ToString()));

            businessObject.txt_apellido2_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.txt_apellido2_ant.ToString()));

            businessObject.txt_apellido2_nue = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.txt_apellido2_nue.ToString()));

            businessObject.txt_nombre_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.txt_nombre_ant.ToString()));

            businessObject.txt_nombre_nue = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.txt_nombre_nue.ToString()));

            businessObject.cod_tipo_doc_ant = dataReader.GetDouble(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_tipo_doc_ant.ToString()));

            businessObject.cod_tipo_doc_nue = dataReader.GetDouble(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_tipo_doc_nue.ToString()));

            businessObject.nro_doc_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.nro_doc_ant.ToString()));

            businessObject.nro_doc_nue = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.nro_doc_nue.ToString()));

            businessObject.fec_nac_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.fec_nac_ant.ToString()));

            businessObject.fec_nac_nue = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.fec_nac_nue.ToString()));

            businessObject.cod_ciiu_ant = dataReader.GetDouble(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_ciiu_ant.ToString()));

            businessObject.cod_ciiu_nue = dataReader.GetDouble(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_ciiu_nue.ToString()));

            businessObject.cod_usuario_mod = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_usuario_mod.ToString()));

            businessObject.fec_modificacion = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.fec_modificacion.ToString()));

            businessObject.fec_vto_password_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.fec_vto_password_ant.ToString()));

            businessObject.fec_vto_password_act = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.fec_vto_password_act.ToString()));

            businessObject.cod_sector_ant = dataReader.GetDouble(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_sector_ant.ToString()));

            businessObject.cod_sector_act = dataReader.GetDouble(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.cod_sector_act.ToString()));

            businessObject.sn_activo_ant = dataReader.GetInt32(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.sn_activo_ant.ToString()));

            businessObject.sn_activo_act = dataReader.GetInt32(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.sn_activo_act.ToString()));

            businessObject.usuario_perfil_ant = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.usuario_perfil_ant.ToString()));

            businessObject.usuario_perfil_act = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.usuario_perfil_act.ToString()));

            businessObject.nro_tiquete = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.nro_tiquete.ToString()));

            businessObject.menu = dataReader.GetString(dataReader.GetOrdinal(Log_usuario.Log_usuarioFields.menu.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Log_usuario</returns>
        internal List<Log_usuario> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Log_usuario> list = new List<Log_usuario>();

            while (dataReader.Read())
            {
                Log_usuario businessObject = new Log_usuario();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
