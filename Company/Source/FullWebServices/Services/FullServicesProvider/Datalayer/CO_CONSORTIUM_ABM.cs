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
    /// Data access layer class for CO_CONSORTIUM
    /// </summary>
    class CO_CONSORTIUM_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_CONSORTIUM_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_CONSORTIUM_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_CONSORTIUM_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(CO_CONSORTIUM businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_CONSORTIUM_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CONSORTIUM_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONSORTIUM_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_MAIN", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_MAIN)));
                sqlCommand.Parameters.Add(new AseParameter("@PARTICIPATION_RATE", AseDbType.Decimal, 4, ParameterDirection.Input, false, 5, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PARTICIPATION_RATE)));
                sqlCommand.Parameters.Add(new AseParameter("@START_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ENABLED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENABLED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_CONSORTIUM::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(CO_CONSORTIUM businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_CONSORTIUM_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CONSORTIUM_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONSORTIUM_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_MAIN", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_MAIN)));
                sqlCommand.Parameters.Add(new AseParameter("@PARTICIPATION_RATE", AseDbType.Decimal, 4, ParameterDirection.Input, false, 5, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PARTICIPATION_RATE)));
                sqlCommand.Parameters.Add(new AseParameter("@START_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.START_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ENABLED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENABLED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_CONSORTIUM::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(CO_CONSORTIUM businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_CONSORTIUM_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CONSORTIUM_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONSORTIUM_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_MAIN", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_MAIN)));
                sqlCommand.Parameters.Add(new AseParameter("@PARTICIPATION_RATE", AseDbType.Decimal, 4, ParameterDirection.Input, false, 5, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PARTICIPATION_RATE)));
                sqlCommand.Parameters.Add(new AseParameter("@START_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.START_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ENABLED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENABLED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_CONSORTIUM::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(CO_CONSORTIUM businessObject, IDataReader dataReader)
        {


            businessObject.INSURED_CD = dataReader.GetInt32(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.INSURED_CD.ToString()));

            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.INDIVIDUAL_ID.ToString()));

            businessObject.CONSORTIUM_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.CONSORTIUM_ID.ToString()));

            businessObject.IS_MAIN = dataReader.GetBoolean(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.IS_MAIN.ToString()));

            businessObject.PARTICIPATION_RATE = dataReader.GetDouble(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.PARTICIPATION_RATE.ToString()));

            businessObject.START_DATE = dataReader.GetString(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.START_DATE.ToString()));

            businessObject.ENABLED = dataReader.GetBoolean(dataReader.GetOrdinal(CO_CONSORTIUM.CO_CONSORTIUMFields.ENABLED.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of CO_CONSORTIUM</returns>
        internal List<CO_CONSORTIUM> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<CO_CONSORTIUM> list = new List<CO_CONSORTIUM>();

            while (dataReader.Read())
            {
                CO_CONSORTIUM businessObject = new CO_CONSORTIUM();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
