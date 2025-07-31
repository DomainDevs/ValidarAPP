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
    /// Data access layer class for Mpersona_cuentas_bancarias
    /// </summary>
    class Mpersona_cuentas_bancarias_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_cuentas_bancarias_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_cuentas_bancarias_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_cuentas_bancarias_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona_cuentas_bancarias businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mper_cuent_bancaria_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_red", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_red)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_plaza", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_banco", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_banco)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_cta_bco", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_cta_bco)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nro_cta", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nro_cta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_titular", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_titular)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activa_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activa_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_default_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_default_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_email", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_dominio_email", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_dominio_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion_email", AseDbType.VarChar, 62, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion_email)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_cuentas_bancarias::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona_cuentas_bancarias businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mper_cuent_bancaria_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_red", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_red)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_plaza", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_banco", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_banco)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_cta_bco", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_cta_bco)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nro_cta", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nro_cta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_titular", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_titular)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activa_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activa_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_default_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_default_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_email", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_dominio_email", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_dominio_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion_email", AseDbType.VarChar, 62, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion_email)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_cuentas_bancarias::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona_cuentas_bancarias businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mper_cuent_bancaria_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_red", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_red)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_plaza", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_banco", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_banco)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_cta_bco", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_cta_bco)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nro_cta", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nro_cta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_titular", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_titular)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activa_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activa_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_default_cuenta", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_default_cuenta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_email", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_dominio_email", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_dominio_email)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion_email", AseDbType.VarChar, 62, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion_email)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_cuentas_bancarias::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona_cuentas_bancarias businessObject, IDataReader dataReader)
        {


            businessObject.cod_tipo_red = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_tipo_red.ToString()));

            businessObject.cod_plaza = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_plaza.ToString()));

            businessObject.cod_banco = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_banco.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.id_persona.ToString()));

            businessObject.cod_tipo_cta_bco = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_tipo_cta_bco.ToString()));

            businessObject.cod_moneda = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_moneda.ToString()));

            businessObject.txt_nro_cta = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.txt_nro_cta.ToString()));

            businessObject.txt_nombre_titular = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.txt_nombre_titular.ToString()));

            businessObject.sn_activa_cuenta = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.sn_activa_cuenta.ToString()));

            businessObject.sn_default_cuenta = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.sn_default_cuenta.ToString()));

            businessObject.txt_nombre_email = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.txt_nombre_email.ToString()));

            businessObject.txt_dominio_email = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.txt_dominio_email.ToString()));

            businessObject.txt_direccion_email = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.txt_direccion_email.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_usuario.ToString())))
            {
                businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.cod_usuario.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.fec_registro.ToString())))
            {
                businessObject.fec_registro = dataReader.GetString(dataReader.GetOrdinal(Mpersona_cuentas_bancarias.Mpersona_cuentas_bancariasFields.fec_registro.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona_cuentas_bancarias</returns>
        internal List<Mpersona_cuentas_bancarias> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona_cuentas_bancarias> list = new List<Mpersona_cuentas_bancarias>();

            while (dataReader.Read())
            {
                Mpersona_cuentas_bancarias businessObject = new Mpersona_cuentas_bancarias();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
