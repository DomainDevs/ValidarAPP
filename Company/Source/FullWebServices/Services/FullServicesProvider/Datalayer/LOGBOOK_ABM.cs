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
    /// Data access layer class for LOGBOOK
    /// </summary>
    class LOGBOOK_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public LOGBOOK_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public LOGBOOK_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public LOGBOOK_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(LOGBOOK businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.LOGBOOK_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 2000;
            
            try
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.Parameters.Add(new AseParameter("@ID_LOGBOOK", AseDbType.Decimal, 17, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, businessObject.ID_LOGBOOK));
                sqlCommand.Parameters.Add(new AseParameter("@ID_PERSONA", AseDbType.Integer, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.ID_PERSONA));
                sqlCommand.Parameters.Add(new AseParameter("@ID_MECHANISM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.ID_MECHANISM));
                sqlCommand.Parameters.Add(new AseParameter("@REFERENCE", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.REFERENCE == null ? "" : businessObject.REFERENCE));
                sqlCommand.Parameters.Add(new AseParameter("@LIST_PURPOSES", AseDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.LIST_PURPOSES));
                sqlCommand.Parameters.Add(new AseParameter("@DATE_LOGBOOK", AseDbType.DateTime, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATE_LOGBOOK, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));


                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("LOGBOOK::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(LOGBOOK businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.LOGBOOK_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@ID_LOGBOOK", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.ID_LOGBOOK));
                sqlCommand.Parameters.Add(new AseParameter("@ID_PERSONA", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.ID_PERSONA));
                sqlCommand.Parameters.Add(new AseParameter("@ID_MECHANISM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.ID_MECHANISM));
                sqlCommand.Parameters.Add(new AseParameter("@REFERENCE", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.REFERENCE == null ? "" : businessObject.REFERENCE));
                sqlCommand.Parameters.Add(new AseParameter("@LIST_PURPOSES", AseDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.LIST_PURPOSES));
                sqlCommand.Parameters.Add(new AseParameter("@DATE_LOGBOOK", AseDbType.DateTime, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToDateTime(businessObject.DATE_LOGBOOK)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("LOGBOOK::Update::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(LOGBOOK businessObject, IDataReader dataReader)
        {


            businessObject.ID_LOGBOOK = dataReader.GetDecimal(dataReader.GetOrdinal(LOGBOOK.LOGBOOKFields.ID_LOGBOOK.ToString()));

            businessObject.ID_PERSONA = dataReader.GetInt32(dataReader.GetOrdinal(LOGBOOK.LOGBOOKFields.ID_PERSONA.ToString()));

            businessObject.ID_MECHANISM = dataReader.GetInt32(dataReader.GetOrdinal(LOGBOOK.LOGBOOKFields.ID_MECHANISM.ToString()));

            businessObject.REFERENCE = dataReader.GetString(dataReader.GetOrdinal(LOGBOOK.LOGBOOKFields.REFERENCE.ToString()));

            businessObject.LIST_PURPOSES = dataReader.GetString(dataReader.GetOrdinal(LOGBOOK.LOGBOOKFields.LIST_PURPOSES.ToString()));

            businessObject.DATE_LOGBOOK = dataReader.GetString(dataReader.GetOrdinal(LOGBOOK.LOGBOOKFields.DATE_LOGBOOK.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of LOGBOOK</returns>
        internal List<LOGBOOK> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<LOGBOOK> list = new List<LOGBOOK>();

            while (dataReader.Read())
            {
                LOGBOOK businessObject = new LOGBOOK();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
