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
    /// Data access layer class for Mpres
    /// </summary>
    class Mpres_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpres_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpres_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpres_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpres businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpres_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_pres", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pres)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_pres", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_pres)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_convenio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_convenio)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_especialidad", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_especialidad)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_porc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_porc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cheque_a_nom", AseDbType.VarChar, 65, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cheque_a_nom)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dependencia", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dependencia)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto_default", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cpto_default)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_habilitacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_habilitacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc_operacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc_operacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_intersuc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_intersuc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_afp", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_afp)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_eps", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_eps)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_afp", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_afp)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_eps", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_eps)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_nacional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_nacional)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ips", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ips)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_ipss", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_ipss)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_minsalud", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_minsalud)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpres::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpres businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpres_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_pres", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pres)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_pres", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_pres)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_convenio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_convenio)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_especialidad", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_especialidad)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_porc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_porc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cheque_a_nom", AseDbType.VarChar, 65, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cheque_a_nom)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dependencia", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dependencia)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto_default", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cpto_default)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_habilitacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_habilitacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc_operacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc_operacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_intersuc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_intersuc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_afp", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_afp)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_eps", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_eps)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_afp", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_afp)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_eps", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_eps)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_nacional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_nacional)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ips", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ips)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_ipss", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_ipss)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_minsalud", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_minsalud)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpres::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpres businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpres_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_pres", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pres)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_pres", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_pres)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_convenio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_convenio)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_especialidad", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_especialidad)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_porc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_porc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cheque_a_nom", AseDbType.VarChar, 65, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cheque_a_nom)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dependencia", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dependencia)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto_default", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cpto_default)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_habilitacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_habilitacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc_operacion", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc_operacion)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_intersuc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_intersuc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_afp", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_afp)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_eps", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_eps)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_afp", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_afp)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_eps", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_eps)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_nacional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_nacional)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ips", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ips)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_ipss", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_ipss)));
                sqlCommand.Parameters.Add(new AseParameter("@codigo_minsalud", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.codigo_minsalud)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpres::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpres businessObject, IDataReader dataReader)
        {


            businessObject.cod_pres = dataReader.GetDouble(dataReader.GetOrdinal(Mpres.MpresFields.cod_pres.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpres.MpresFields.id_persona.ToString()));

            businessObject.cod_ciiu = dataReader.GetDouble(dataReader.GetOrdinal(Mpres.MpresFields.cod_ciiu.ToString()));

            businessObject.cod_tipo_pres = dataReader.GetDouble(dataReader.GetOrdinal(Mpres.MpresFields.cod_tipo_pres.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.sn_convenio.ToString())))
            {
                businessObject.sn_convenio = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.sn_convenio.ToString()));
            }

            businessObject.cod_especialidad = dataReader.GetDouble(dataReader.GetOrdinal(Mpres.MpresFields.cod_especialidad.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.nro_porc.ToString())))
            {
                businessObject.nro_porc = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.nro_porc.ToString()));
            }

            businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.fec_alta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.fec_baja.ToString()));
            }

            businessObject.txt_cheque_a_nom = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.txt_cheque_a_nom.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.cod_dependencia.ToString())))
            {
                businessObject.cod_dependencia = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.cod_dependencia.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.cod_cpto_default.ToString())))
            {
                businessObject.cod_cpto_default = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.cod_cpto_default.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.fec_habilitacion.ToString())))
            {
                businessObject.fec_habilitacion = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.fec_habilitacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.cod_suc_operacion.ToString())))
            {
                businessObject.cod_suc_operacion = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.cod_suc_operacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.sn_intersuc.ToString())))
            {
                businessObject.sn_intersuc = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.sn_intersuc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.cod_baja.ToString())))
            {
                businessObject.cod_baja = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.cod_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.sn_afp.ToString())))
            {
                businessObject.sn_afp = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.sn_afp.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.sn_eps.ToString())))
            {
                businessObject.sn_eps = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.sn_eps.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.codigo_afp.ToString())))
            {
                businessObject.codigo_afp = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.codigo_afp.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.codigo_eps.ToString())))
            {
                businessObject.codigo_eps = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.codigo_eps.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.sn_nacional.ToString())))
            {
                businessObject.sn_nacional = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.sn_nacional.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.sn_ips.ToString())))
            {
                businessObject.sn_ips = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.sn_ips.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.codigo_ipss.ToString())))
            {
                businessObject.codigo_ipss = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.codigo_ipss.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpres.MpresFields.codigo_minsalud.ToString())))
            {
                businessObject.codigo_minsalud = dataReader.GetString(dataReader.GetOrdinal(Mpres.MpresFields.codigo_minsalud.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpres</returns>
        internal List<Mpres> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpres> list = new List<Mpres>();

            while (dataReader.Read())
            {
                Mpres businessObject = new Mpres();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
