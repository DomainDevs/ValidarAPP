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
    /// Data access layer class for Mpersona
    /// </summary>
    class Mpersona_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_iva", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_iva)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nit", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nit)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cia_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cia_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_dpto_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_dpto_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_puesto_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_puesto_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_asistente_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_asistente_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_nac", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_nac)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_sexo", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_sexo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_est_civil", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_est_civil)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_notas", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_notas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_origen", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_origen)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ent_oficial", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ent_oficial)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona::Insert::Error occured.", ex);
            }
       
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_iva", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_iva)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nit", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nit)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cia_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cia_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_dpto_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_dpto_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_puesto_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_puesto_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_asistente_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_asistente_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_nac", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_nac)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_sexo", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_sexo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_est_civil", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_est_civil)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_notas", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_notas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_origen", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_origen)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ent_oficial", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ent_oficial)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona::Update::Error occured.", ex);
            }
        }

        /// <summary>
        ////Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool UpdateU(Mpersona businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_UpdateU";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nit", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nit)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona::UpdateU::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido1", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apellido2", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apellido2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_iva", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_iva)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nit", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nit)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_cia_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_cia_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_dpto_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_dpto_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_puesto_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_puesto_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_asistente_tra", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_asistente_tra)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_nac", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_nac, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_nac", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_nac)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_sexo", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_sexo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_est_civil", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_est_civil)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_notas", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_notas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_origen", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_origen)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ent_oficial", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ent_oficial)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona.MpersonaFields.id_persona.ToString()));

            businessObject.txt_apellido1 = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_apellido1.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_apellido2.ToString())))
            {
                businessObject.txt_apellido2 = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_apellido2.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_nombre.ToString())))
            {
                businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_nombre.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_tipo_doc.ToString())))
            {
                businessObject.cod_tipo_doc = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_tipo_doc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.nro_doc.ToString())))
            {
                businessObject.nro_doc = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.nro_doc.ToString()));
            }

            businessObject.cod_tipo_iva = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_tipo_iva.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.nro_nit.ToString())))
            {
                businessObject.nro_nit = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.nro_nit.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_cia_tra.ToString())))
            {
                businessObject.txt_cia_tra = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_cia_tra.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_dpto_tra.ToString())))
            {
                businessObject.txt_dpto_tra = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_dpto_tra.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_puesto_tra.ToString())))
            {
                businessObject.txt_puesto_tra = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_puesto_tra.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_asistente_tra.ToString())))
            {
                businessObject.txt_asistente_tra = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_asistente_tra.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.fec_nac.ToString())))
            {
                businessObject.fec_nac = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.fec_nac.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_lugar_nac.ToString())))
            {
                businessObject.txt_lugar_nac = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_lugar_nac.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_sexo.ToString())))
            {
                businessObject.txt_sexo = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_sexo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_est_civil.ToString())))
            {
                businessObject.cod_est_civil = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_est_civil.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_notas.ToString())))
            {
                businessObject.txt_notas = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_notas.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_tipo_persona.ToString())))
            {
                businessObject.cod_tipo_persona = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_tipo_persona.ToString()));
            }

            businessObject.txt_origen = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.txt_origen.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_ent_oficial.ToString())))
            {
                businessObject.cod_ent_oficial = dataReader.GetString(dataReader.GetOrdinal(Mpersona.MpersonaFields.cod_ent_oficial.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona</returns>
        internal List<Mpersona> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona> list = new List<Mpersona>();

            while (dataReader.Read())
            {
                Mpersona businessObject = new Mpersona();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
