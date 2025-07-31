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
    /// Data access layer class for Magente
    /// </summary>
    class Magente_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Magente businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cheque_a_nom", AseDbType.VarChar, 65, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cheque_a_nom)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_carnet", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_carnet)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_frentes", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_frentes)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_facturas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_facturas)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_casillero", AseDbType.VarChar, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_casillero)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_leyenda_fact", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_leyenda_fact)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_zona", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_zona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_estado", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_estado)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_gerente", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_gerente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dependencia", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dependencia)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_igss", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_igss)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_insc_agaps", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_insc_agaps)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_referencia", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_referencia)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_patrocinador", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_patrocinador)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_resolucion", AseDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_resolucion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_impresion", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impresion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comision_gm", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comision_gm)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_op_automatica", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_op_automatica)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cond_isr", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cond_isr)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_descuenta_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_descuenta_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_auxiliar", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_auxiliar)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calif_cart", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calif_cart)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_corte_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_corte_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_envio_aviso_cobro", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_envio_aviso_cobro)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inactivacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_anul", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_anul)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_timbre", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_timbre)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Magente::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Magente businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cheque_a_nom", AseDbType.VarChar, 65, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cheque_a_nom)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_carnet", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_carnet)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_frentes", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_frentes)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_facturas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_facturas)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_casillero", AseDbType.VarChar, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_casillero)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_leyenda_fact", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_leyenda_fact)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_zona", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_zona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_estado", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_estado)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_gerente", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_gerente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dependencia", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dependencia)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_igss", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_igss)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_insc_agaps", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_insc_agaps)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_referencia", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_referencia)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_patrocinador", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_patrocinador)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_resolucion", AseDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_resolucion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_impresion", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impresion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comision_gm", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comision_gm)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_op_automatica", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_op_automatica)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cond_isr", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cond_isr)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_descuenta_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_descuenta_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_auxiliar", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_auxiliar)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calif_cart", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calif_cart)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_corte_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_corte_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_envio_aviso_cobro", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_envio_aviso_cobro)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inactivacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_anul", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_anul)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_timbre", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_timbre)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Magente::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Magente businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cheque_a_nom", AseDbType.VarChar, 65, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cheque_a_nom)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_carnet", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_carnet)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_frentes", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_frentes)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_facturas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_facturas)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_casillero", AseDbType.VarChar, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_casillero)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_leyenda_fact", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_leyenda_fact)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_zona", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_zona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_estado", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_estado)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_gerente", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_gerente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dependencia", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dependencia)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_igss", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_igss)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_insc_agaps", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_insc_agaps)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_referencia", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_referencia)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_patrocinador", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_patrocinador)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_resolucion", AseDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_resolucion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_impresion", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impresion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comision_gm", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comision_gm)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_op_automatica", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_op_automatica)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cond_isr", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cond_isr)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_descuenta_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_descuenta_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_auxiliar", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_auxiliar)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calif_cart", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calif_cart)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_corte_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_corte_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_envio_aviso_cobro", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_envio_aviso_cobro)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inactivacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_anul", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_anul)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_timbre", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_timbre)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Magente::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Magente businessObject, IDataReader dataReader)
        {


            businessObject.cod_tipo_agente = dataReader.GetDouble(dataReader.GetOrdinal(Magente.MagenteFields.cod_tipo_agente.ToString()));

            businessObject.cod_agente = dataReader.GetInt32(dataReader.GetOrdinal(Magente.MagenteFields.cod_agente.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Magente.MagenteFields.id_persona.ToString()));

            businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.fec_alta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.fec_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_baja.ToString())))
            {
                businessObject.cod_baja = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_baja.ToString()));
            }

            businessObject.txt_cheque_a_nom = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_cheque_a_nom.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.txt_obs.ToString())))
            {
                businessObject.txt_obs = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_obs.ToString()));
            }

            businessObject.cod_grupo = dataReader.GetDouble(dataReader.GetOrdinal(Magente.MagenteFields.cod_grupo.ToString()));

            businessObject.nro_carnet = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.nro_carnet.ToString()));

            businessObject.cnt_frentes = dataReader.GetDouble(dataReader.GetOrdinal(Magente.MagenteFields.cnt_frentes.ToString()));

            businessObject.cnt_facturas = dataReader.GetDouble(dataReader.GetOrdinal(Magente.MagenteFields.cnt_facturas.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.txt_casillero.ToString())))
            {
                businessObject.txt_casillero = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_casillero.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.txt_leyenda_fact.ToString())))
            {
                businessObject.txt_leyenda_fact = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_leyenda_fact.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_zona.ToString())))
            {
                businessObject.cod_zona = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_zona.ToString()));
            }

            businessObject.cod_estado = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_estado.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_gerente.ToString())))
            {
                businessObject.cod_gerente = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_gerente.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_dependencia.ToString())))
            {
                businessObject.cod_dependencia = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_dependencia.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.nro_igss.ToString())))
            {
                businessObject.nro_igss = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.nro_igss.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_insc_agaps.ToString())))
            {
                businessObject.sn_insc_agaps = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_insc_agaps.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.txt_referencia.ToString())))
            {
                businessObject.txt_referencia = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_referencia.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.txt_patrocinador.ToString())))
            {
                businessObject.txt_patrocinador = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_patrocinador.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.nro_resolucion.ToString())))
            {
                businessObject.nro_resolucion = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.nro_resolucion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_impresion.ToString())))
            {
                businessObject.cod_impresion = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_impresion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_comision.ToString())))
            {
                businessObject.sn_comision = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_comision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.pje_comision_gm.ToString())))
            {
                businessObject.pje_comision_gm = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.pje_comision_gm.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_op_automatica.ToString())))
            {
                businessObject.sn_op_automatica = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_op_automatica.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_cond_isr.ToString())))
            {
                businessObject.cod_cond_isr = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_cond_isr.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.fec_ult_modif.ToString())))
            {
                businessObject.fec_ult_modif = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.fec_ult_modif.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_usuario.ToString())))
            {
                businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_usuario.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_descuenta_comision.ToString())))
            {
                businessObject.sn_descuenta_comision = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_descuenta_comision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.txt_auxiliar.ToString())))
            {
                businessObject.txt_auxiliar = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.txt_auxiliar.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_suc.ToString())))
            {
                businessObject.cod_suc = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_suc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cod_calif_cart.ToString())))
            {
                businessObject.cod_calif_cart = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cod_calif_cart.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_corte_cuenta.ToString())))
            {
                businessObject.sn_corte_cuenta = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_corte_cuenta.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_envio_aviso_cobro.ToString())))
            {
                businessObject.sn_envio_aviso_cobro = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_envio_aviso_cobro.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.fec_inactivacion.ToString())))
            {
                businessObject.fec_inactivacion = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.fec_inactivacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.cnt_dias_anul.ToString())))
            {
                businessObject.cnt_dias_anul = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.cnt_dias_anul.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Magente.MagenteFields.sn_timbre.ToString())))
            {
                businessObject.sn_timbre = dataReader.GetString(dataReader.GetOrdinal(Magente.MagenteFields.sn_timbre.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Magente</returns>
        internal List<Magente> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Magente> list = new List<Magente>();

            while (dataReader.Read())
            {
                Magente businessObject = new Magente();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
