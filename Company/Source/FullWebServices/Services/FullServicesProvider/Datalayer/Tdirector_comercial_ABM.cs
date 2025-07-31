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
    /// Data access layer class for Tdirector_comercial
    /// </summary>
    class Tdirector_comercial_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tdirector_comercial_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tdirector_comercial_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tdirector_comercial_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tdirector_comercial businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tdirector_comercial_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_director_comercial", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_director_comercial)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_director_nacional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_director_nacional)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc_asoc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_mov", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_mov, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));
                sqlCommand.Parameters.Add(new AseParameter("@user_sise", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.user_sise)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tdirector_comercial::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tdirector_comercial businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tdirector_comercial_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_director_comercial", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_director_comercial)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_director_nacional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_director_nacional)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc_asoc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_mov", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_mov, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));
                sqlCommand.Parameters.Add(new AseParameter("@user_sise", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.user_sise)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tdirector_comercial::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tdirector_comercial businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tdirector_comercial_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_director_comercial", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_director_comercial)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_director_nacional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_director_nacional)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc_asoc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_mov", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_mov, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_activo)));
                sqlCommand.Parameters.Add(new AseParameter("@user_sise", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.user_sise)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tdirector_comercial::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tdirector_comercial businessObject, IDataReader dataReader)
        {


            businessObject.cod_director_comercial = dataReader.GetInt32(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.cod_director_comercial.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.id_persona.ToString()));

            businessObject.cod_director_nacional = dataReader.GetInt32(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.cod_director_nacional.ToString()));

            businessObject.cod_suc_asoc = dataReader.GetDouble(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.cod_suc_asoc.ToString()));

            businessObject.fec_ult_mov = dataReader.GetString(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.fec_ult_mov.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.sn_activo.ToString())))
            {
                businessObject.sn_activo = dataReader.GetString(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.sn_activo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.user_sise.ToString())))
            {
                businessObject.user_sise = dataReader.GetString(dataReader.GetOrdinal(Tdirector_comercial.Tdirector_comercialFields.user_sise.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tdirector_comercial</returns>
        internal List<Tdirector_comercial> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tdirector_comercial> list = new List<Tdirector_comercial>();

            while (dataReader.Read())
            {
                Tdirector_comercial businessObject = new Tdirector_comercial();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
