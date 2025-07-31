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
    /// Data access layer class for TECHNICAL_CARD
    /// </summary>
    class TECHNICAL_CARD_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public TECHNICAL_CARD_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public TECHNICAL_CARD_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public TECHNICAL_CARD_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(TECHNICAL_CARD businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TECHNICAL_CARD_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPERIENCE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPERIENCE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_CAPITAL_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_CAPITAL_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_PILE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_PILE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_NUM", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_FROM", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_FROM, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_TO", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_TO, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_LOCATION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_LOCATION)));
                sqlCommand.Parameters.Add(new AseParameter("@TAX_INSPECTOR", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_INSPECTOR)));
                sqlCommand.Parameters.Add(new AseParameter("@CORPORATE_PURPOSE", AseDbType.VarChar, 1000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CORPORATE_PURPOSE)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERENCES", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERENCES)));
                sqlCommand.Parameters.Add(new AseParameter("@FINANCIAL_CONCEPT", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FINANCIAL_CONCEPT)));
                sqlCommand.Parameters.Add(new AseParameter("@PILE_DESCRIPTION", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PILE_DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPERIENCE", AseDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPERIENCE)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTERED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTERED_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("TECHNICAL_CARD::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(TECHNICAL_CARD businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TECHNICAL_CARD_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPERIENCE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPERIENCE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_CAPITAL_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_CAPITAL_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_PILE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_PILE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_NUM", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_FROM", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_FROM, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_TO", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_TO, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_LOCATION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_LOCATION)));
                sqlCommand.Parameters.Add(new AseParameter("@TAX_INSPECTOR", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_INSPECTOR)));
                sqlCommand.Parameters.Add(new AseParameter("@CORPORATE_PURPOSE", AseDbType.VarChar, 1000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CORPORATE_PURPOSE)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERENCES", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERENCES)));
                sqlCommand.Parameters.Add(new AseParameter("@FINANCIAL_CONCEPT", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FINANCIAL_CONCEPT)));
                sqlCommand.Parameters.Add(new AseParameter("@PILE_DESCRIPTION", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PILE_DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPERIENCE", AseDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPERIENCE)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTERED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTERED_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("TECHNICAL_CARD::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(TECHNICAL_CARD businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TECHNICAL_CARD_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

           
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPERIENCE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPERIENCE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_CAPITAL_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_CAPITAL_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_PILE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_PILE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_NUM", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_FROM", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_FROM, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ENROLLMENT_TO", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENROLLMENT_TO, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_LOCATION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_LOCATION)));
                sqlCommand.Parameters.Add(new AseParameter("@TAX_INSPECTOR", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_INSPECTOR)));
                sqlCommand.Parameters.Add(new AseParameter("@CORPORATE_PURPOSE", AseDbType.VarChar, 1000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CORPORATE_PURPOSE)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERENCES", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERENCES)));
                sqlCommand.Parameters.Add(new AseParameter("@FINANCIAL_CONCEPT", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FINANCIAL_CONCEPT)));
                sqlCommand.Parameters.Add(new AseParameter("@PILE_DESCRIPTION", AseDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PILE_DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPERIENCE", AseDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPERIENCE)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTERED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTERED_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("TECHNICAL_CARD::Delete::Error occured.", ex);
            }        
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(TECHNICAL_CARD businessObject, IDataReader dataReader)
        {


            businessObject.TECHNICAL_CARD_ID = dataReader.GetInt32(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.TECHNICAL_CARD_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.INDIVIDUAL_ID.ToString())))
            {
                businessObject.INDIVIDUAL_ID = dataReader.GetInt16(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.INDIVIDUAL_ID.ToString()));
            }

            businessObject.EXPERIENCE_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.EXPERIENCE_TYPE_CD.ToString()));

            businessObject.AUTHORIZED_CAPITAL_AMT = dataReader.GetDouble(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.AUTHORIZED_CAPITAL_AMT.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.CURRENT_PILE_AMT.ToString())))
            {
                businessObject.CURRENT_PILE_AMT = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.CURRENT_PILE_AMT.ToString()));
            }

            businessObject.ENROLLMENT_NUM = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.ENROLLMENT_NUM.ToString()));

            businessObject.ENROLLMENT_FROM = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.ENROLLMENT_FROM.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.ENROLLMENT_TO.ToString())))
            {
                businessObject.ENROLLMENT_TO = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.ENROLLMENT_TO.ToString()));
            }

            businessObject.TECHNICAL_CARD_LOCATION = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.TECHNICAL_CARD_LOCATION.ToString()));

            businessObject.TAX_INSPECTOR = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.TAX_INSPECTOR.ToString()));

            businessObject.CORPORATE_PURPOSE = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.CORPORATE_PURPOSE.ToString()));

            businessObject.REFERENCES = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.REFERENCES.ToString()));

            businessObject.FINANCIAL_CONCEPT = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.FINANCIAL_CONCEPT.ToString()));

            businessObject.PILE_DESCRIPTION = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.PILE_DESCRIPTION.ToString()));

            businessObject.EXPERIENCE = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.EXPERIENCE.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.REGISTRATION_DATE.ToString())))
            {
                businessObject.REGISTRATION_DATE = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.REGISTRATION_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.REGISTERED_USER_ID.ToString())))
            {
                businessObject.REGISTERED_USER_ID = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD.TECHNICAL_CARDFields.REGISTERED_USER_ID.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of TECHNICAL_CARD</returns>
        internal List<TECHNICAL_CARD> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<TECHNICAL_CARD> list = new List<TECHNICAL_CARD>();

            while (dataReader.Read())
            {
                TECHNICAL_CARD businessObject = new TECHNICAL_CARD();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
