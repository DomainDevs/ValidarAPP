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
    /// Data access layer class for Mbeneficiario
    /// </summary>
    class Mbeneficiario_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mbeneficiario_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mbeneficiario_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mbeneficiario_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mbeneficiario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mbeneficiario_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_beneficiario)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inactivacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@edad", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.edad)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_justificacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_justificacion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mbeneficiario::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mbeneficiario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mbeneficiario_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_beneficiario)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inactivacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@edad", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.edad)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_justificacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_justificacion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mbeneficiario::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mbeneficiario businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mbeneficiario_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_beneficiario)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inactivacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inactivacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@edad", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.edad)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_justificacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_justificacion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mbeneficiario::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mbeneficiario businessObject, IDataReader dataReader)
        {


            businessObject.cod_beneficiario = dataReader.GetInt32(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.cod_beneficiario.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.id_persona.ToString()));

            businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.fec_alta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.fec_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.cod_baja.ToString())))
            {
                businessObject.cod_baja = dataReader.GetString(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.cod_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.fec_inactivacion.ToString())))
            {
                businessObject.fec_inactivacion = dataReader.GetString(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.fec_inactivacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.edad.ToString())))
            {
                businessObject.edad = dataReader.GetString(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.edad.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.cod_justificacion.ToString())))
            {
                businessObject.cod_justificacion = dataReader.GetString(dataReader.GetOrdinal(Mbeneficiario.MbeneficiarioFields.cod_justificacion.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mbeneficiario</returns>
        internal List<Mbeneficiario> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mbeneficiario> list = new List<Mbeneficiario>();

            while (dataReader.Read())
            {
                Mbeneficiario businessObject = new Mbeneficiario();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
