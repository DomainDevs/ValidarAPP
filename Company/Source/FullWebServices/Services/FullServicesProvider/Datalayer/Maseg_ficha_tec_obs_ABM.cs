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
    /// Data access layer class for Maseg_ficha_tec_obs
    /// </summary>
    class Maseg_ficha_tec_obs_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_obs_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_obs_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_ficha_tec_obs_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Maseg_ficha_tec_obs businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.masegFichaTecObs_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@consec_obs", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.consec_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs", AseDbType.VarChar, 360, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs)));
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
                throw new SupException("Maseg_ficha_tec_obs::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Maseg_ficha_tec_obs businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.masegFichaTecObs_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@consec_obs", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.consec_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs", AseDbType.VarChar, 360, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs)));
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
                throw new SupException("Maseg_ficha_tec_obs::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Maseg_ficha_tec_obs businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.masegFichaTecObs_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@consec_obs", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.consec_obs)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obs", AseDbType.VarChar, 360, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obs)));
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
                throw new SupException("Maseg_ficha_tec_obs::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Maseg_ficha_tec_obs businessObject, IDataReader dataReader)
        {


            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.cod_aseg.ToString()));

            businessObject.consec_obs = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.consec_obs.ToString()));

            businessObject.txt_obs = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.txt_obs.ToString()));

            businessObject.fec_creacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.fec_creacion.ToString()));

            businessObject.fec_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.fec_modif.ToString()));

            businessObject.cod_usuario_crea = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.cod_usuario_crea.ToString()));

            businessObject.cod_usuario_modif = dataReader.GetString(dataReader.GetOrdinal(Maseg_ficha_tec_obs.Maseg_ficha_tec_obsFields.cod_usuario_modif.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_ficha_tec_obs</returns>
        internal List<Maseg_ficha_tec_obs> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Maseg_ficha_tec_obs> list = new List<Maseg_ficha_tec_obs>();

            while (dataReader.Read())
            {
                Maseg_ficha_tec_obs businessObject = new Maseg_ficha_tec_obs();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
