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
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models.DataLayer
{
    /// <summary>
    /// Data access layer class for Tusuario
    /// </summary>
    class Tusuario_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tusuario_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tusuario_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tusuario_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tusuario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tusuario_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo_usuario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_validez_pwd", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_validez_pwd)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_password", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_password));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@circuito_op", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.circuito_op)));
                sqlCommand.Parameters.Add(new AseParameter("@iniciales", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.iniciales)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cpto_cble", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cpto_cble)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_todos_cpto_cb", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_todos_cpto_cb)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cta_cble", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cta_cble)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_todos_cta_cb", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_todos_cta_cb)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_caja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_caja)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pto_vta", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pto_vta)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_perfil", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_perfil)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil)));
                sqlCommand.Parameters.Add(new AseParameter("@limite_op", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.limite_op)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_pedir_password", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_pedir_password)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@perfil_director", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.perfil_director)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_web", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_web)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_usuario_externo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_usuario_externo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_bloqueo_x_clave", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_bloqueo_x_clave)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_hora_activacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_hora_activacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_email", AseDbType.VarChar, 62, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_email)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_3g", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_3g)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_perfil_delegado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_perfil_delegado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_activacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_activacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_desactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_desactivacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tusuario::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tusuario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tusuario_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo_usuario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_validez_pwd", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_validez_pwd)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_password", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_password)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@circuito_op", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.circuito_op)));
                sqlCommand.Parameters.Add(new AseParameter("@iniciales", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.iniciales)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cpto_cble", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cpto_cble)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_todos_cpto_cb", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_todos_cpto_cb)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cta_cble", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cta_cble)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_todos_cta_cb", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_todos_cta_cb)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_caja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_caja)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pto_vta", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pto_vta)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_perfil", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_perfil)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil)));
                sqlCommand.Parameters.Add(new AseParameter("@limite_op", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.limite_op)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_pedir_password", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_pedir_password)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@perfil_director", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.perfil_director)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_web", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_web)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_usuario_externo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_usuario_externo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_bloqueo_x_clave", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_bloqueo_x_clave)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_hora_activacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_hora_activacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_email", AseDbType.VarChar, 62, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_email)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_3g", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_3g)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_perfil_delegado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_perfil_delegado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_activacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_activacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_desactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_desactivacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tusuario::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tusuario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tusuario_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo_usuario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_sector", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_sector)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vto_password", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vto_password, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_validez_pwd", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_validez_pwd)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_password", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_password)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@circuito_op", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.circuito_op)));
                sqlCommand.Parameters.Add(new AseParameter("@iniciales", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.iniciales)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cpto_cble", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cpto_cble)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_todos_cpto_cb", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_todos_cpto_cb)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cta_cble", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cta_cble)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_todos_cta_cb", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_todos_cta_cb)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_caja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_caja)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pto_vta", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pto_vta)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_perfil", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_perfil)));
                sqlCommand.Parameters.Add(new AseParameter("@usuario_perfil", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.usuario_perfil)));
                sqlCommand.Parameters.Add(new AseParameter("@limite_op", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.limite_op)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_pedir_password", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_pedir_password)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@perfil_director", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.perfil_director)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_web", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_web)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_usuario_externo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_usuario_externo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_bloqueo_x_clave", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_bloqueo_x_clave)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_hora_activacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_hora_activacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_email", AseDbType.VarChar, 62, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_email)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo_3g", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo_3g)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_perfil_delegado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_perfil_delegado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_activacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_activacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_desactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_desactivacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tusuario::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tusuario businessObject, IDataReader dataReader)
        {


            businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_usuario.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_grupo_usuario.ToString())))
            {
                businessObject.cod_grupo_usuario = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_grupo_usuario.ToString()));
            }

            businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.txt_nombre.ToString()));

            businessObject.cod_suc = dataReader.GetDouble(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_suc.ToString()));

            businessObject.cod_sector = dataReader.GetDouble(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_sector.ToString()));

            businessObject.sn_activo = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_activo.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_vto_password.ToString())))
            {
                businessObject.fec_vto_password = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_vto_password.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.cnt_dias_validez_pwd.ToString())))
            {
                businessObject.cnt_dias_validez_pwd = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.cnt_dias_validez_pwd.ToString()));
            }

            businessObject.txt_password = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.txt_password.ToString()));

            businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_alta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.circuito_op.ToString())))
            {
                businessObject.circuito_op = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.circuito_op.ToString()));
            }

            businessObject.iniciales = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.iniciales.ToString()));

            businessObject.sn_cpto_cble = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_cpto_cble.ToString()));

            businessObject.sn_todos_cpto_cb = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_todos_cpto_cb.ToString()));

            businessObject.sn_cta_cble = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_cta_cble.ToString()));

            businessObject.sn_todos_cta_cb = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_todos_cta_cb.ToString()));

            businessObject.nro_caja = dataReader.GetDouble(dataReader.GetOrdinal(Tusuario.TusuarioFields.nro_caja.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_pto_vta.ToString())))
            {
                businessObject.cod_pto_vta = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_pto_vta.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_ramo.ToString())))
            {
                businessObject.cod_ramo = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.cod_ramo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_perfil.ToString())))
            {
                businessObject.sn_perfil = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_perfil.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.usuario_perfil.ToString())))
            {
                businessObject.usuario_perfil = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.usuario_perfil.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.limite_op.ToString())))
            {
                businessObject.limite_op = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.limite_op.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_pedir_password.ToString())))
            {
                businessObject.sn_pedir_password = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_pedir_password.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_habilitado.ToString())))
            {
                businessObject.sn_habilitado = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_habilitado.ToString()));
            }

            businessObject.perfil_director = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.perfil_director.ToString()));

            businessObject.sn_activo_web = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_activo_web.ToString()));

            businessObject.sn_usuario_externo = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_usuario_externo.ToString()));

            businessObject.sn_bloqueo_x_clave = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_bloqueo_x_clave.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_hora_activacion.ToString())))
            {
                businessObject.fec_hora_activacion = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_hora_activacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.txt_email.ToString())))
            {
                businessObject.txt_email = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.txt_email.ToString()));
            }

            businessObject.sn_activo_3g = dataReader.GetInt32(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_activo_3g.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_perfil_delegado.ToString())))
            {
                businessObject.sn_perfil_delegado = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.sn_perfil_delegado.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_activacion.ToString())))
            {
                businessObject.fec_activacion = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_activacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_desactivacion.ToString())))
            {
                businessObject.fec_desactivacion = dataReader.GetString(dataReader.GetOrdinal(Tusuario.TusuarioFields.fec_desactivacion.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tusuario</returns>
        internal List<Tusuario> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tusuario> list = new List<Tusuario>();

            while (dataReader.Read())
            {
                Tusuario businessObject = new Tusuario();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        /*public string md5(string password)
        {
            //Declaraciones
            System.Security.Cryptography.MD5 md5;
            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //Conversion
            Byte[] encodedBytes = md5.ComputeHash(ASCIIEncoding.Default.GetBytes(password));  //genero el hash a partir de la password original

            //Resultado

            //return BitConverter.ToString(encodedBytes);      //esto, devuelve el hash con "-" cada 2 char
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(encodedBytes).ToLower(), @"-", "");     //devuelve el hash continuo y en minuscula. (igual que en php)
        }*/

        private static string Encrypt(string txtpassw)
        {
            string Password = "Ña25%fy&";
            string p = "", b = "", S = "";
            int J = 0;
            int A1 = 0, A2 = 0, A3 = 0;

            for (int i = 0; i < Password.Length; i++)
            {
                p = p + System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(Password.Substring(i, 1))[0];
            }

            for (int i = 0; i < txtpassw.Length; i++)
            {
                A1 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(p.Substring(J, 1))[0];
                J = (J > p.Length ? 1 : J + 1);
                A2 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(txtpassw.Substring(i, 1))[0];
                A3 = A1 ^ A2;
                b = String.Format("{0:X}", A3);
                if (b.Length < 2) { b = "0" + b; }
                S = S + b;
            }
            return S;
        }

        #endregion

    }
}
