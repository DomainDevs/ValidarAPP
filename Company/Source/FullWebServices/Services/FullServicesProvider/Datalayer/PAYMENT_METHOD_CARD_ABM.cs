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
    /// Data access layer class for PAYMENT_METHOD_CARD
    /// </summary>
    class PAYMENT_METHOD_CARD_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public PAYMENT_METHOD_CARD_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PAYMENT_METHOD_CARD_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PAYMENT_METHOD_CARD_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(PAYMENT_METHOD_CARD businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PAYMENT_METHOD_CARD_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PAYMENT_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PAYMENT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CARD_NUMBER", AseDbType.Decimal, 8, ParameterDirection.Input, false, 16, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CARD_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@BANK_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BANK_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SECURITY_NUMBER", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECURITY_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@SINCE", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SINCE)));
                sqlCommand.Parameters.Add(new AseParameter("@THRU", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.THRU)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PAYMENT_METHOD_CARD::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(PAYMENT_METHOD_CARD businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PAYMENT_METHOD_CARD_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PAYMENT_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PAYMENT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CARD_NUMBER", AseDbType.Decimal, 8, ParameterDirection.Input, false, 16, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CARD_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@BANK_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BANK_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SECURITY_NUMBER", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECURITY_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@SINCE", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SINCE)));
                sqlCommand.Parameters.Add(new AseParameter("@THRU", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.THRU)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PAYMENT_METHOD_CARD::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(PAYMENT_METHOD_CARD businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PAYMENT_METHOD_CARD_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PAYMENT_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PAYMENT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CARD_NUMBER", AseDbType.Decimal, 8, ParameterDirection.Input, false, 16, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CARD_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@BANK_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BANK_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SECURITY_NUMBER", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECURITY_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@SINCE", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SINCE)));
                sqlCommand.Parameters.Add(new AseParameter("@THRU", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.THRU)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PAYMENT_METHOD_CARD::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(PAYMENT_METHOD_CARD businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.INDIVIDUAL_ID.ToString()));

            businessObject.PAYMENT_ID = dataReader.GetInt32(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.PAYMENT_ID.ToString()));

            businessObject.CARD_NUMBER = dataReader.GetDouble(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.CARD_NUMBER.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.BANK_CD.ToString())))
            {
                businessObject.BANK_CD = dataReader.GetString(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.BANK_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.SECURITY_NUMBER.ToString())))
            {
                businessObject.SECURITY_NUMBER = dataReader.GetString(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.SECURITY_NUMBER.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.SINCE.ToString())))
            {
                businessObject.SINCE = dataReader.GetString(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.SINCE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.THRU.ToString())))
            {
                businessObject.THRU = dataReader.GetString(dataReader.GetOrdinal(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields.THRU.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of PAYMENT_METHOD_CARD</returns>
        internal List<PAYMENT_METHOD_CARD> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<PAYMENT_METHOD_CARD> list = new List<PAYMENT_METHOD_CARD>();

            while (dataReader.Read())
            {
                PAYMENT_METHOD_CARD businessObject = new PAYMENT_METHOD_CARD();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
