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
    /// Data access layer class for BOARD_DIRECTORS
    /// </summary>
    class BOARD_DIRECTORS_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public BOARD_DIRECTORS_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public BOARD_DIRECTORS_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public BOARD_DIRECTORS_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(BOARD_DIRECTORS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.BOARD_DIRECTORS_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_DIRECTORS_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_DIRECTORS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_MEMBER_NAME", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_MEMBER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_MEMBER_JOB_TITLE", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_MEMBER_JOB_TITLE)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("BOARD_DIRECTORS::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(BOARD_DIRECTORS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.BOARD_DIRECTORS_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_DIRECTORS_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_DIRECTORS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_MEMBER_NAME", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_MEMBER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_MEMBER_JOB_TITLE", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_MEMBER_JOB_TITLE)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("BOARD_DIRECTORS::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(BOARD_DIRECTORS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.BOARD_DIRECTORS_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_DIRECTORS_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_DIRECTORS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_MEMBER_NAME", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_MEMBER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@BOARD_MEMBER_JOB_TITLE", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BOARD_MEMBER_JOB_TITLE)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("BOARD_DIRECTORS::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(BOARD_DIRECTORS businessObject, IDataReader dataReader)
        {


            businessObject.BOARD_DIRECTORS_CD = dataReader.GetInt32(dataReader.GetOrdinal(BOARD_DIRECTORS.BOARD_DIRECTORSFields.BOARD_DIRECTORS_CD.ToString()));

            businessObject.TECHNICAL_CARD_ID = dataReader.GetInt32(dataReader.GetOrdinal(BOARD_DIRECTORS.BOARD_DIRECTORSFields.TECHNICAL_CARD_ID.ToString()));

            businessObject.BOARD_MEMBER_NAME = dataReader.GetString(dataReader.GetOrdinal(BOARD_DIRECTORS.BOARD_DIRECTORSFields.BOARD_MEMBER_NAME.ToString()));

            businessObject.BOARD_MEMBER_JOB_TITLE = dataReader.GetString(dataReader.GetOrdinal(BOARD_DIRECTORS.BOARD_DIRECTORSFields.BOARD_MEMBER_JOB_TITLE.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of BOARD_DIRECTORS</returns>
        internal List<BOARD_DIRECTORS> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<BOARD_DIRECTORS> list = new List<BOARD_DIRECTORS>();

            while (dataReader.Read())
            {
                BOARD_DIRECTORS businessObject = new BOARD_DIRECTORS();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
