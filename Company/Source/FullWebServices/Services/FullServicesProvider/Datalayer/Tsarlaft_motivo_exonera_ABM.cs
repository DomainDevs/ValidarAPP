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
    /// Data access layer class for Tsarlaft_motivo_exonera
    /// </summary>
    class Tsarlaft_motivo_exonera_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tsarlaft_motivo_exonera_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tsarlaft_motivo_exonera_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tsarlaft_motivo_exonera_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tsarlaft_motivo_exonera businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tsarl_motiv_exonera_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_motivo_exonera", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_motivo_exonera)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modificacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tsarlaft_motivo_exonera::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tsarlaft_motivo_exonera businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tsarl_motiv_exonera_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_motivo_exonera", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_motivo_exonera)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modificacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tsarlaft_motivo_exonera::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tsarlaft_motivo_exonera businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tsarl_motiv_exonera_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_motivo_exonera", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_motivo_exonera)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_modificacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tsarlaft_motivo_exonera::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tsarlaft_motivo_exonera businessObject, IDataReader dataReader)
        {


            businessObject.cod_motivo_exonera = dataReader.GetDouble(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.cod_motivo_exonera.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.txt_desc.ToString()));

            businessObject.sn_habilitado = dataReader.GetInt32(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.sn_habilitado.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.fec_modificacion.ToString())))
            {
                businessObject.fec_modificacion = dataReader.GetString(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.fec_modificacion.ToString()));
            }

            businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.cod_usuario.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.cod_tipo_persona.ToString())))
            {
                businessObject.cod_tipo_persona = dataReader.GetString(dataReader.GetOrdinal(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields.cod_tipo_persona.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tsarlaft_motivo_exonera</returns>
        internal List<Tsarlaft_motivo_exonera> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tsarlaft_motivo_exonera> list = new List<Tsarlaft_motivo_exonera>();

            while (dataReader.Read())
            {
                Tsarlaft_motivo_exonera businessObject = new Tsarlaft_motivo_exonera();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
