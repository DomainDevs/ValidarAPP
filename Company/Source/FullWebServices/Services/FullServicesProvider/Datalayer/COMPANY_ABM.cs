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
    /// Data access layer class for COMPANY
    /// </summary>
    class COMPANY_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public COMPANY_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public COMPANY_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public COMPANY_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.COMPANY_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@TRADE_NAME", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRADE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BUSINESS_LEGAL_STATUS_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BUSINESS_LEGAL_STATUS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MARKET_SECTOR_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MARKET_SECTOR_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MANAGER_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MANAGER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@GENERAL_MANAGER_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GENERAL_MANAGER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@LEGAL_REPRESENTATIVE_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LEGAL_REPRESENTATIVE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@LR_ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LR_ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LR_ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LR_ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT_ADDITIONAL_INFO", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT_ADDITIONAL_INFO)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("COMPANY::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.COMPANY_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@TRADE_NAME", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRADE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BUSINESS_LEGAL_STATUS_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BUSINESS_LEGAL_STATUS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MARKET_SECTOR_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MARKET_SECTOR_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MANAGER_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MANAGER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@GENERAL_MANAGER_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GENERAL_MANAGER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@LEGAL_REPRESENTATIVE_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LEGAL_REPRESENTATIVE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@LR_ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LR_ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LR_ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LR_ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT_ADDITIONAL_INFO", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT_ADDITIONAL_INFO)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("COMPANY::Update::Error occured.", ex);
            }
        }

        /// <summary>
        ////Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool UpdateU(COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.COMPANY_UpdateU";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@TRADE_NAME", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRADE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("COMPANY::UpdateU::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.COMPANY_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@TRADE_NAME", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRADE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BUSINESS_LEGAL_STATUS_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BUSINESS_LEGAL_STATUS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MARKET_SECTOR_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MARKET_SECTOR_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MANAGER_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MANAGER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@GENERAL_MANAGER_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GENERAL_MANAGER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@LEGAL_REPRESENTATIVE_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LEGAL_REPRESENTATIVE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@LR_ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LR_ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LR_ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LR_ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT_ADDITIONAL_INFO", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT_ADDITIONAL_INFO)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("COMPANY::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(COMPANY businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(COMPANY.COMPANYFields.INDIVIDUAL_ID.ToString()));

            businessObject.TRADE_NAME = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.TRADE_NAME.ToString()));

            businessObject.TRIBUTARY_ID_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(COMPANY.COMPANYFields.TRIBUTARY_ID_TYPE_CD.ToString()));

            businessObject.TRIBUTARY_ID_NO = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.TRIBUTARY_ID_NO.ToString()));

            businessObject.COUNTRY_CD = dataReader.GetInt32(dataReader.GetOrdinal(COMPANY.COMPANYFields.COUNTRY_CD.ToString()));

            businessObject.COMPANY_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(COMPANY.COMPANYFields.COMPANY_TYPE_CD.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.BUSINESS_LEGAL_STATUS_CD.ToString())))
            {
                businessObject.BUSINESS_LEGAL_STATUS_CD = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.BUSINESS_LEGAL_STATUS_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.MARKET_SECTOR_CD.ToString())))
            {
                businessObject.MARKET_SECTOR_CD = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.MARKET_SECTOR_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.MANAGER_NAME.ToString())))
            {
                businessObject.MANAGER_NAME = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.MANAGER_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.GENERAL_MANAGER_NAME.ToString())))
            {
                businessObject.GENERAL_MANAGER_NAME = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.GENERAL_MANAGER_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.LEGAL_REPRESENTATIVE_NAME.ToString())))
            {
                businessObject.LEGAL_REPRESENTATIVE_NAME = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.LEGAL_REPRESENTATIVE_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.LR_ID_CARD_TYPE_CD.ToString())))
            {
                businessObject.LR_ID_CARD_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.LR_ID_CARD_TYPE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.LR_ID_CARD_NO.ToString())))
            {
                businessObject.LR_ID_CARD_NO = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.LR_ID_CARD_NO.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.CONTACT_NAME.ToString())))
            {
                businessObject.CONTACT_NAME = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.CONTACT_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(COMPANY.COMPANYFields.CONTACT_ADDITIONAL_INFO.ToString())))
            {
                businessObject.CONTACT_ADDITIONAL_INFO = dataReader.GetString(dataReader.GetOrdinal(COMPANY.COMPANYFields.CONTACT_ADDITIONAL_INFO.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of COMPANY</returns>
        internal List<COMPANY> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<COMPANY> list = new List<COMPANY>();

            while (dataReader.Read())
            {
                COMPANY businessObject = new COMPANY();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
