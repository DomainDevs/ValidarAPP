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
    /// Data access layer class for Mpersona_telef
    /// </summary>
    class Mpersona_telef_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_telef_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_telef_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_telef_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona_telef businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_telef_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_telef", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_telef)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_pais == "0") ? null : businessObject.cod_pais))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_dpto == "0") ? null : businessObject.cod_dpto))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_municipio == "0") ? null : businessObject.cod_municipio))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@sn_domicilio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_domicilio))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_telef::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona_telef businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_telef_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_telef", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_telef)));
                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_telef_old", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_telef_old)));
                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_pais == "0") ? null : businessObject.cod_pais))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_dpto == "0") ? null : businessObject.cod_dpto))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_municipio == "0") ? null : businessObject.cod_municipio))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@sn_domicilio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_domicilio))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_telef::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona_telef businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_telef_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_telef", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_telef)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_pais == "0") ? null : businessObject.cod_pais))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_dpto == "0") ? null : businessObject.cod_dpto))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull((businessObject.cod_municipio == "0") ? null : businessObject.cod_municipio))); //SUPDB //Edward Rubiano -- 18/07/2016 -- C11226 -- Se realiza nulo para no hacer fallas por foraneos
                sqlCommand.Parameters.Add(new AseParameter("@sn_domicilio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_domicilio))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_telef::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona_telef businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.id_persona.ToString()));

            businessObject.cod_tipo_telef = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_tipo_telef.ToString()));

            businessObject.txt_telefono = dataReader.GetString(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.txt_telefono.ToString()));

			//SUPDB - INICIO
            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_pais.ToString())))
            {
                businessObject.cod_pais = dataReader.GetString(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_pais.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_dpto.ToString())))
            {
                businessObject.cod_dpto = dataReader.GetString(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_dpto.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_municipio.ToString())))
            {
                businessObject.cod_municipio = dataReader.GetString(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.cod_municipio.ToString()));
            }

            businessObject.sn_domicilio = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_telef.Mpersona_telefFields.sn_domicilio.ToString()));
			//SUPDB - FIN

        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona_telef</returns>
        internal List<Mpersona_telef> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona_telef> list = new List<Mpersona_telef>();

            while (dataReader.Read())
            {
                Mpersona_telef businessObject = new Mpersona_telef();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
