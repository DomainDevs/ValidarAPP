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
    /// Data access layer class for Maseg_ficha_tec_financ
    /// </summary>
    class Maseg_ficha_tec_financ_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_financ_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_financ_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_financ_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Maseg_ficha_tec_financ businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.masegFichTecFina_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_informacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_informacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_inventarios", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_inventarios)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cuentas_cobrar", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cuentas_cobrar)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_cte", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_equipos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_equipos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_fijo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_fijo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_total", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_total)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_cte", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_lplazo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_lplazo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_total", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_total)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_patrimonio", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_patrimonio)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_costo_vtas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_costo_vtas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ventas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ventas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_bruta", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_bruta)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_neta", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_neta)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_oper", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_oper)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_terrenos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_terrenos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_edificios", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_edificios)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ctas_pagar", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ctas_pagar)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_obl_bancos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_obl_bancos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_obl_lplazo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_obl_lplazo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cap_social", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cap_social)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_acum", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_acum)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_reval_patrim", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_reval_patrim)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_invers_temp", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_invers_temp)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activ_fijos_br", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activ_fijos_br)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_superavit_valoriz", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_superavit_valoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_superavit", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_superavit)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_reservas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_reservas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_primas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_primas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingr_nopera", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingr_nopera)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ajustes_infl", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ajustes_infl)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_intereses", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_intereses)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_gastos_nopera", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_gastos_nopera)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_valorizaciones", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_valorizaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_creacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_creacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_crea", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_crea)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_modif", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_modif)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_ficha_tec_financ::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Maseg_ficha_tec_financ businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.masegFichTecFina_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_informacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_informacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_inventarios", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_inventarios)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cuentas_cobrar", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cuentas_cobrar)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_cte", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_equipos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_equipos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_fijo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_fijo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_total", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_total)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_cte", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_lplazo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_lplazo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_total", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_total)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_patrimonio", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_patrimonio)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_costo_vtas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_costo_vtas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ventas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ventas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_bruta", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_bruta)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_neta", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_neta)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_oper", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_oper)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_terrenos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_terrenos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_edificios", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_edificios)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ctas_pagar", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ctas_pagar)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_obl_bancos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_obl_bancos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_obl_lplazo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_obl_lplazo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cap_social", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cap_social)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_acum", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_acum)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_reval_patrim", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_reval_patrim)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_invers_temp", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_invers_temp)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activ_fijos_br", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activ_fijos_br)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_superavit_valoriz", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_superavit_valoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_superavit", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_superavit)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_reservas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_reservas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_primas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_primas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingr_nopera", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingr_nopera)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ajustes_infl", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ajustes_infl)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_intereses", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_intereses)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_gastos_nopera", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_gastos_nopera)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_valorizaciones", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_valorizaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_creacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_creacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_crea", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_crea)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_modif", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_modif)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_ficha_tec_financ::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Maseg_ficha_tec_financ businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.masegFichTecFina_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_informacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_informacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_inventarios", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_inventarios)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cuentas_cobrar", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cuentas_cobrar)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_cte", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_equipos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_equipos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_fijo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_fijo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activo_total", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activo_total)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_cte", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_lplazo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_lplazo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivo_total", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivo_total)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_patrimonio", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_patrimonio)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_costo_vtas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_costo_vtas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ventas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ventas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_bruta", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_bruta)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_neta", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_neta)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_oper", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_oper)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_terrenos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_terrenos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_edificios", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_edificios)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ctas_pagar", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ctas_pagar)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_obl_bancos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_obl_bancos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_obl_lplazo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_obl_lplazo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cap_social", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cap_social)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_util_acum", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_util_acum)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_reval_patrim", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_reval_patrim)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_invers_temp", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_invers_temp)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activ_fijos_br", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activ_fijos_br)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_superavit_valoriz", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_superavit_valoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_superavit", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_superavit)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_reservas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_reservas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_primas", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_primas)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingr_nopera", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingr_nopera)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ajustes_infl", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ajustes_infl)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_intereses", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_intereses)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_gastos_nopera", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_gastos_nopera)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_valorizaciones", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_valorizaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_creacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_creacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_crea", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_crea)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_modif", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_modif)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_ficha_tec_financ::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Maseg_ficha_tec_financ businessObject, IDataReader dataReader)
        {


            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.cod_aseg.ToString()));

            businessObject.fec_informacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.fec_informacion.ToString()));

            businessObject.imp_inventarios = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_inventarios.ToString()));

            businessObject.imp_cuentas_cobrar = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_cuentas_cobrar.ToString()));

            businessObject.imp_activo_cte = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_activo_cte.ToString()));

            businessObject.imp_equipos = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_equipos.ToString()));

            businessObject.imp_activo_fijo = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_activo_fijo.ToString()));

            businessObject.imp_activo_total = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_activo_total.ToString()));

            businessObject.imp_pasivo_cte = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_pasivo_cte.ToString()));

            businessObject.imp_pasivo_lplazo = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_pasivo_lplazo.ToString()));

            businessObject.imp_pasivo_total = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_pasivo_total.ToString()));

            businessObject.imp_patrimonio = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_patrimonio.ToString()));

            businessObject.imp_costo_vtas = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_costo_vtas.ToString()));

            businessObject.imp_ventas = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_ventas.ToString()));

            businessObject.imp_util_bruta = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_util_bruta.ToString()));

            businessObject.imp_util_neta = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_util_neta.ToString()));

            businessObject.imp_util_oper = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_util_oper.ToString()));

            businessObject.imp_terrenos = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_terrenos.ToString()));

            businessObject.imp_edificios = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_edificios.ToString()));

            businessObject.imp_ctas_pagar = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_ctas_pagar.ToString()));

            businessObject.imp_obl_bancos = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_obl_bancos.ToString()));

            businessObject.imp_obl_lplazo = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_obl_lplazo.ToString()));

            businessObject.imp_cap_social = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_cap_social.ToString()));

            businessObject.imp_util_acum = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_util_acum.ToString()));

            businessObject.imp_reval_patrim = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_reval_patrim.ToString()));

            businessObject.imp_invers_temp = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_invers_temp.ToString()));

            businessObject.imp_activ_fijos_br = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_activ_fijos_br.ToString()));

            businessObject.imp_superavit_valoriz = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_superavit_valoriz.ToString()));

            businessObject.imp_otros_superavit = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_otros_superavit.ToString()));

            businessObject.imp_reservas = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_reservas.ToString()));

            businessObject.imp_primas = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_primas.ToString()));

            businessObject.imp_otros_ingr_nopera = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_otros_ingr_nopera.ToString()));

            businessObject.imp_ajustes_infl = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_ajustes_infl.ToString()));

            businessObject.imp_intereses = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_intereses.ToString()));

            businessObject.imp_otros_gastos_nopera = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_otros_gastos_nopera.ToString()));

            businessObject.imp_valorizaciones = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.imp_valorizaciones.ToString()));

            businessObject.fec_creacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.fec_creacion.ToString()));

            businessObject.fec_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.fec_modif.ToString()));

            businessObject.cod_usuario_crea = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.cod_usuario_crea.ToString()));

            businessObject.cod_usuario_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields.cod_usuario_modif.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_ficha_tec_financ</returns>
        internal List<Maseg_ficha_tec_financ> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Maseg_ficha_tec_financ> list = new List<Maseg_ficha_tec_financ>();

            while (dataReader.Read())
            {
                Maseg_ficha_tec_financ businessObject = new Maseg_ficha_tec_financ();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
