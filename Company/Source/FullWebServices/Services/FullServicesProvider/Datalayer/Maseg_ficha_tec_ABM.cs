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
    /// Data access layer class for Maseg_ficha_tec
    /// </summary>
    class Maseg_ficha_tec_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Maseg_ficha_tec businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_ficha_tec_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_matricula", AseDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_matricula)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_desde_matric", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_desde_matric, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_hasta_matric", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_hasta_matric, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_k_autorizado", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_k_autorizado)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_revisor_fiscal", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_revisor_fiscal)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_ubicacion_ficha", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_ubicacion_ficha)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_experiencia", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_experiencia)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cpto_financiero", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cpto_financiero)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs_cumulo", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs_cumulo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_otras_obs", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_otras_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_objeto_soc", AseDbType.VarChar, 240, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_objeto_soc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_experiencia_en", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_experiencia_en)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_referencias", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_referencias)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_creacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_creacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_crea", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_crea)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_modif", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_modif)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_contragarantias", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_contragarantias)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cumulo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cumulo)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_cifin", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_cifin, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_ficha_tec::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Maseg_ficha_tec businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_ficha_tec_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_matricula", AseDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_matricula)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_desde_matric", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_desde_matric, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_hasta_matric", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_hasta_matric, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_k_autorizado", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_k_autorizado)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_revisor_fiscal", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_revisor_fiscal)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_ubicacion_ficha", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_ubicacion_ficha)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_experiencia", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_experiencia)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cpto_financiero", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cpto_financiero)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs_cumulo", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs_cumulo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_otras_obs", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_otras_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_objeto_soc", AseDbType.VarChar, 240, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_objeto_soc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_experiencia_en", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_experiencia_en)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_referencias", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_referencias)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_creacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_creacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_crea", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_crea)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_modif", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_modif)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_contragarantias", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_contragarantias)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cumulo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cumulo)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_cifin", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_cifin, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_ficha_tec::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Maseg_ficha_tec businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_ficha_tec_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_matricula", AseDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_matricula)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_desde_matric", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_desde_matric, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_hasta_matric", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_hasta_matric, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_k_autorizado", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_k_autorizado)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_revisor_fiscal", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_revisor_fiscal)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_ubicacion_ficha", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_ubicacion_ficha)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_experiencia", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_experiencia)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cpto_financiero", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cpto_financiero)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs_cumulo", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs_cumulo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_otras_obs", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_otras_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_objeto_soc", AseDbType.VarChar, 240, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_objeto_soc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_experiencia_en", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_experiencia_en)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_referencias", AseDbType.VarChar, 144, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_referencias)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_creacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_creacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_crea", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_crea)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_modif", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_modif)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_contragarantias", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_contragarantias)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_cumulo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_cumulo)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_cifin", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_cifin, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_ficha_tec::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Maseg_ficha_tec businessObject, IDataReader dataReader)
        {


            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.cod_aseg.ToString()));

            businessObject.nro_matricula = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.nro_matricula.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_desde_matric.ToString())))
            {
                businessObject.fec_desde_matric = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_desde_matric.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_hasta_matric.ToString())))
            {
                businessObject.fec_hasta_matric = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_hasta_matric.ToString()));
            }

            businessObject.imp_k_autorizado = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.imp_k_autorizado.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_revisor_fiscal.ToString())))
            {
                businessObject.txt_revisor_fiscal = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_revisor_fiscal.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_ubicacion_ficha.ToString())))
            {
                businessObject.txt_ubicacion_ficha = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_ubicacion_ficha.ToString()));
            }

            businessObject.cod_experiencia = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.cod_experiencia.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_cpto_financiero.ToString())))
            {
                businessObject.txt_cpto_financiero = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_cpto_financiero.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_obs_cumulo.ToString())))
            {
                businessObject.txt_obs_cumulo = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_obs_cumulo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_otras_obs.ToString())))
            {
                businessObject.txt_otras_obs = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_otras_obs.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_objeto_soc.ToString())))
            {
                businessObject.txt_objeto_soc = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_objeto_soc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_experiencia_en.ToString())))
            {
                businessObject.txt_experiencia_en = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_experiencia_en.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_referencias.ToString())))
            {
                businessObject.txt_referencias = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_referencias.ToString()));
            }

            businessObject.fec_creacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_creacion.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_modif.ToString())))
            {
                businessObject.fec_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_modif.ToString()));
            }

            businessObject.cod_usuario_crea = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.cod_usuario_crea.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.cod_usuario_modif.ToString())))
            {
                businessObject.cod_usuario_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.cod_usuario_modif.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_contragarantias.ToString())))
            {
                businessObject.txt_contragarantias = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.txt_contragarantias.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.imp_cumulo.ToString())))
            {
                businessObject.imp_cumulo = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.imp_cumulo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_cifin.ToString())))
            {
                businessObject.fec_cifin = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec.Maseg_ficha_tecFields.fec_cifin.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_ficha_tec</returns>
        internal List<Maseg_ficha_tec> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Maseg_ficha_tec> list = new List<Maseg_ficha_tec>();

            while (dataReader.Read())
            {
                Maseg_ficha_tec businessObject = new Maseg_ficha_tec();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
