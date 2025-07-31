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
    /// Data access layer class for Maseg_header
    /// </summary>
    class Maseg_header_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_header_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_header_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_header_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Maseg_header businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_header_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_figura_aseg", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_figura_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_aseg", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_imp_aseg", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_imp_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_vincula", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_vincula)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ocupacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ocupacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_aviso_vto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_aviso_vto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_aseg_viejo", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_aseg_viejo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg_vinc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg_vinc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nom_factura", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nom_factura)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_seg_ocupacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_seg_ocupacion)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_sueldo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_sueldo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_niveloperativo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_niveloperativo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_garantia", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_garantia)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calif_cart", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calif_cart)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_empresa", AseDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_empresa)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tasociacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tasociacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_valida_cgarantia", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_valida_cgarantia)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_aseg_especial", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_aseg_especial)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_consorcio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_consorcio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_header::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Maseg_header businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_header_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_figura_aseg", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_figura_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_aseg", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_imp_aseg", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_imp_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_vincula", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_vincula)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ocupacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ocupacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_aviso_vto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_aviso_vto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_aseg_viejo", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_aseg_viejo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg_vinc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg_vinc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nom_factura", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nom_factura)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_seg_ocupacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_seg_ocupacion)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_sueldo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_sueldo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_niveloperativo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_niveloperativo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_garantia", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_garantia)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calif_cart", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calif_cart)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_empresa", AseDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_empresa)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tasociacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tasociacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_valida_cgarantia", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_valida_cgarantia)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_aseg_especial", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_aseg_especial)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_consorcio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_consorcio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_header::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Maseg_header businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_header_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_figura_aseg", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_figura_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_aseg", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_imp_aseg", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_imp_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_vincula", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_vincula)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ocupacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ocupacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_aviso_vto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_aviso_vto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_aseg_viejo", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_aseg_viejo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg_vinc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg_vinc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nom_factura", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nom_factura)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_seg_ocupacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_seg_ocupacion)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_sueldo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_sueldo)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_niveloperativo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_niveloperativo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_garantia", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_garantia)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calif_cart", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calif_cart)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_empresa", AseDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_empresa)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tasociacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tasociacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_valida_cgarantia", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_valida_cgarantia)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_aseg_especial", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_aseg_especial)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_consorcio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_consorcio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_header::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Maseg_header businessObject, IDataReader dataReader)
        {


            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_aseg.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.id_persona.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_figura_aseg.ToString())))
            {
                businessObject.cod_figura_aseg = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_figura_aseg.ToString()));
            }

            businessObject.cod_tipo_aseg = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_tipo_aseg.ToString()));

            businessObject.cod_imp_aseg = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_imp_aseg.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_vincula.ToString())))
            {
                businessObject.txt_vincula = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_vincula.ToString()));
            }

            businessObject.cod_tipo_agente = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_tipo_agente.ToString()));

            businessObject.cod_agente = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_agente.ToString()));

            businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.fec_alta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.fec_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_ocupacion.ToString())))
            {
                businessObject.cod_ocupacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_ocupacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.sn_aviso_vto.ToString())))
            {
                businessObject.sn_aviso_vto = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.sn_aviso_vto.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_aseg_viejo.ToString())))
            {
                businessObject.txt_aseg_viejo = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_aseg_viejo.ToString()));
            }

            businessObject.cod_aseg_vinc = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_aseg_vinc.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.fec_ult_modif.ToString())))
            {
                businessObject.fec_ult_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.fec_ult_modif.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_usuario.ToString())))
            {
                businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_usuario.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_nom_factura.ToString())))
            {
                businessObject.txt_nom_factura = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_nom_factura.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_seg_ocupacion.ToString())))
            {
                businessObject.cod_seg_ocupacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_seg_ocupacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.imp_sueldo.ToString())))
            {
                businessObject.imp_sueldo = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.imp_sueldo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.imp_otros_ingresos.ToString())))
            {
                businessObject.imp_otros_ingresos = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.imp_otros_ingresos.ToString()));
            }

            businessObject.cod_ciiu = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_ciiu.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.imp_niveloperativo.ToString())))
            {
                businessObject.imp_niveloperativo = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.imp_niveloperativo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_garantia.ToString())))
            {
                businessObject.txt_garantia = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.txt_garantia.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_baja.ToString())))
            {
                businessObject.cod_baja = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_calif_cart.ToString())))
            {
                businessObject.cod_calif_cart = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_calif_cart.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_ttipo_empresa.ToString())))
            {
                businessObject.cod_ttipo_empresa = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_ttipo_empresa.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_tasociacion.ToString())))
            {
                businessObject.cod_tasociacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.cod_tasociacion.ToString()));
            }

            businessObject.sn_valida_cgarantia = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.sn_valida_cgarantia.ToString()));

            businessObject.sn_aseg_especial = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.sn_aseg_especial.ToString()));

            businessObject.sn_consorcio = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_header.Maseg_headerFields.sn_consorcio.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_header</returns>
        internal List<Maseg_header> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Maseg_header> list = new List<Maseg_header>();

            while (dataReader.Read())
            {
                Maseg_header businessObject = new Maseg_header();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
