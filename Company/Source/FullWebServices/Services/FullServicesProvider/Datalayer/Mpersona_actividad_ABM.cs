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
    /// Data access layer class for Mpersona_actividad
    /// </summary>
    class Mpersona_actividad_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_actividad_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_actividad_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_actividad_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona_actividad businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mperson_actividad_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_abona", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_abona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_munic", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_munic)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_actividad", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_actividad)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_actividad::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona_actividad businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mperson_actividad_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_abona", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_abona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_munic", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_munic)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_actividad", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_actividad)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_actividad::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona_actividad businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mperson_actividad_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_abona", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_abona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_munic", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_munic)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_actividad", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_actividad)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_registro", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_registro, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_actividad::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona_actividad businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.id_persona.ToString()));

            businessObject.cod_abona = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.cod_abona.ToString()));

            businessObject.cod_dpto = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.cod_dpto.ToString()));

            businessObject.cod_munic = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.cod_munic.ToString()));

            businessObject.cod_actividad = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.cod_actividad.ToString()));

            businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.cod_usuario.ToString()));

            businessObject.fec_registro = dataReader.GetString(dataReader.GetOrdinal(Mpersona_actividad.Mpersona_actividadFields.fec_registro.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona_actividad</returns>
        internal List<Mpersona_actividad> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona_actividad> list = new List<Mpersona_actividad>();

            while (dataReader.Read())
            {
                Mpersona_actividad businessObject = new Mpersona_actividad();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
