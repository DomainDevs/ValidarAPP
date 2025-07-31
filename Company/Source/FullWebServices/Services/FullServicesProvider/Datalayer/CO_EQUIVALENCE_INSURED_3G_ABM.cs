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
    /// Data access layer class for CO_EQUIVALENCE_INSURED_3G
    /// </summary>
    class CO_EQUIVALENCE_INSURED_3G_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_EQUIVALENCE_INSURED_3G_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_EQUIVALENCE_INSURED_3G_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_EQUIVALENCE_INSURED_3G_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(CO_EQUIVALENCE_INSURED_3G businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_EQUIV_INSUR_3G_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_2G_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_2G_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_3G_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_3G_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_2G_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_2G_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_3G_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_3G_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME_NUM", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME_NUM)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_EQUIVALENCE_INSURED_3G::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(CO_EQUIVALENCE_INSURED_3G businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_EQUIV_INSUR_3G_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_2G_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_2G_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_3G_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_3G_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_2G_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_2G_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_3G_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_3G_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME_NUM", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME_NUM)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_EQUIVALENCE_INSURED_3G::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(CO_EQUIVALENCE_INSURED_3G businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_EQUIV_INSUR_3G_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_2G_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_2G_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_3G_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_3G_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_2G_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_2G_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_3G_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_3G_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME_NUM", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME_NUM)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_EQUIVALENCE_INSURED_3G::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(CO_EQUIVALENCE_INSURED_3G businessObject, IDataReader dataReader)
        {
            businessObject.INDIVIDUAL_2G_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INDIVIDUAL_2G_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INDIVIDUAL_3G_ID.ToString())))
            {
                businessObject.INDIVIDUAL_3G_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INDIVIDUAL_3G_ID.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INSURED_2G_CD.ToString())))
            {
                businessObject.INSURED_2G_CD = dataReader.GetInt32(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INSURED_2G_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INSURED_3G_CD.ToString())))
            {
                businessObject.INSURED_3G_CD = dataReader.GetInt32(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.INSURED_3G_CD.ToString()));
            }

            businessObject.NAME_NUM = dataReader.GetInt32(dataReader.GetOrdinal(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields.NAME_NUM.ToString()));
        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of CO_EQUIVALENCE_INSURED_3G</returns>
        internal List<CO_EQUIVALENCE_INSURED_3G> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<CO_EQUIVALENCE_INSURED_3G> list = new List<CO_EQUIVALENCE_INSURED_3G>();

            while (dataReader.Read())
            {
                CO_EQUIVALENCE_INSURED_3G businessObject = new CO_EQUIVALENCE_INSURED_3G();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
