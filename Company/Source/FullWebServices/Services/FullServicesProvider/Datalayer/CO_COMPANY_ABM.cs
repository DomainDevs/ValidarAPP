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
    /// Data access layer class for CO_COMPANY
    /// </summary>
    class CO_COMPANY_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_COMPANY_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_COMPANY_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_COMPANY_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(CO_COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_COMPANY_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@VERIFY_DIGIT", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VERIFY_DIGIT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSOCIATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSOCIATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZATION_AMOUNT", AseDbType.Decimal, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZATION_AMOUNT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CODE", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@CATEGORY_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CATEGORY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTITY_OFFICIAL_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTITY_OFFICIAL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_GROUP_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_GROUP_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_COMPANY::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(CO_COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_COMPANY_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@VERIFY_DIGIT", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VERIFY_DIGIT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSOCIATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSOCIATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZATION_AMOUNT", AseDbType.Decimal, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZATION_AMOUNT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CODE", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@CATEGORY_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CATEGORY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTITY_OFFICIAL_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTITY_OFFICIAL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_GROUP_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_GROUP_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_COMPANY::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(CO_COMPANY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_COMPANY_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@VERIFY_DIGIT", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VERIFY_DIGIT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSOCIATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSOCIATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZATION_AMOUNT", AseDbType.Decimal, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZATION_AMOUNT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CODE", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@CATEGORY_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CATEGORY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTITY_OFFICIAL_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTITY_OFFICIAL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_GROUP_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_GROUP_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_COMPANY::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(CO_COMPANY businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.INDIVIDUAL_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.VERIFY_DIGIT.ToString())))
            {
                businessObject.VERIFY_DIGIT = dataReader.GetString(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.VERIFY_DIGIT.ToString()));
            }

            businessObject.ASSOCIATION_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.ASSOCIATION_TYPE_CD.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.AUTHORIZATION_AMOUNT.ToString())))
            {
                businessObject.AUTHORIZATION_AMOUNT = dataReader.GetString(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.AUTHORIZATION_AMOUNT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.CURRENCY_CODE.ToString())))
            {
                businessObject.CURRENCY_CODE = dataReader.GetString(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.CURRENCY_CODE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.CATEGORY_CD.ToString())))
            {
                businessObject.CATEGORY_CD = dataReader.GetString(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.CATEGORY_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.ENTITY_OFFICIAL_CD.ToString())))
            {
                businessObject.ENTITY_OFFICIAL_CD = dataReader.GetString(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.ENTITY_OFFICIAL_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.INSURED_GROUP_ID.ToString())))
            {
                businessObject.INSURED_GROUP_ID = dataReader.GetString(dataReader.GetOrdinal(CO_COMPANY.CO_COMPANYFields.INSURED_GROUP_ID.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of CO_COMPANY</returns>
        internal List<CO_COMPANY> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<CO_COMPANY> list = new List<CO_COMPANY>();

            while (dataReader.Read())
            {
                CO_COMPANY businessObject = new CO_COMPANY();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
