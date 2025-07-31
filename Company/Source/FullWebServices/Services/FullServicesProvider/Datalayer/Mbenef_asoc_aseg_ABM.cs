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
    /// Data access layer class for Mbenef_asoc_aseg
    /// </summary>
    class Mbenef_asoc_aseg_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mbenef_asoc_aseg_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mbenef_asoc_aseg_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mbenef_asoc_aseg_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mbenef_asoc_aseg businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mbenef_asoc_aseg_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_beneficiario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_parentesco", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_parentesco)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mbenef_asoc_aseg::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mbenef_asoc_aseg businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mbenef_asoc_aseg_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_beneficiario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_parentesco", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_parentesco)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mbenef_asoc_aseg::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mbenef_asoc_aseg businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mbenef_asoc_aseg_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_beneficiario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_parentesco", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_parentesco)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mbenef_asoc_aseg::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mbenef_asoc_aseg businessObject, IDataReader dataReader)
        {


            businessObject.cod_beneficiario = dataReader.GetInt32(dataReader.GetOrdinal(Mbenef_asoc_aseg.Mbenef_asoc_asegFields.cod_beneficiario.ToString()));

            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Mbenef_asoc_aseg.Mbenef_asoc_asegFields.cod_aseg.ToString()));

            businessObject.cod_parentesco = dataReader.GetDouble(dataReader.GetOrdinal(Mbenef_asoc_aseg.Mbenef_asoc_asegFields.cod_parentesco.ToString()));

            businessObject.sn_activo = dataReader.GetInt32(dataReader.GetOrdinal(Mbenef_asoc_aseg.Mbenef_asoc_asegFields.sn_activo.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mbenef_asoc_aseg</returns>
        internal List<Mbenef_asoc_aseg> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mbenef_asoc_aseg> list = new List<Mbenef_asoc_aseg>();

            while (dataReader.Read())
            {
                Mbenef_asoc_aseg businessObject = new Mbenef_asoc_aseg();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
