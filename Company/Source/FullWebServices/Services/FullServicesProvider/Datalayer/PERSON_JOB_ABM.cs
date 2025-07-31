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
    /// Data access layer class for PERSON_JOB
    /// </summary>
    class PERSON_JOB_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public PERSON_JOB_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PERSON_JOB_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PERSON_JOB_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(PERSON_JOB businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_JOB_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OCCUPATION_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OCCUPATION_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INCOME_LEVEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INCOME_LEVEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@JOB_SECTOR", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.JOB_SECTOR)));
                sqlCommand.Parameters.Add(new AseParameter("@POSITION", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.POSITION)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@SPECIALITY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SPECIALITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHER_OCCUPATION_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHER_OCCUPATION_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON_JOB::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(PERSON_JOB businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_JOB_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OCCUPATION_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OCCUPATION_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INCOME_LEVEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INCOME_LEVEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@JOB_SECTOR", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.JOB_SECTOR)));
                sqlCommand.Parameters.Add(new AseParameter("@POSITION", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.POSITION)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@SPECIALITY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SPECIALITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHER_OCCUPATION_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHER_OCCUPATION_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON_JOB::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(PERSON_JOB businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_JOB_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OCCUPATION_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OCCUPATION_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INCOME_LEVEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INCOME_LEVEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@JOB_SECTOR", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.JOB_SECTOR)));
                sqlCommand.Parameters.Add(new AseParameter("@POSITION", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.POSITION)));
                sqlCommand.Parameters.Add(new AseParameter("@CONTACT", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CONTACT)));
                sqlCommand.Parameters.Add(new AseParameter("@COMPANY_PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COMPANY_PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@SPECIALITY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SPECIALITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHER_OCCUPATION_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHER_OCCUPATION_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON_JOB::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(PERSON_JOB businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.INDIVIDUAL_ID.ToString()));

            businessObject.OCCUPATION_CD = dataReader.GetInt32(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.OCCUPATION_CD.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.INCOME_LEVEL_CD.ToString())))
            {
                businessObject.INCOME_LEVEL_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.INCOME_LEVEL_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.COMPANY_NAME.ToString())))
            {
                businessObject.COMPANY_NAME = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.COMPANY_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.JOB_SECTOR.ToString())))
            {
                businessObject.JOB_SECTOR = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.JOB_SECTOR.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.POSITION.ToString())))
            {
                businessObject.POSITION = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.POSITION.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.CONTACT.ToString())))
            {
                businessObject.CONTACT = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.CONTACT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.COMPANY_PHONE.ToString())))
            {
                businessObject.COMPANY_PHONE = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.COMPANY_PHONE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.SPECIALITY_CD.ToString())))
            {
                businessObject.SPECIALITY_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.SPECIALITY_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.OTHER_OCCUPATION_CD.ToString())))
            {
                businessObject.OTHER_OCCUPATION_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON_JOB.PERSON_JOBFields.OTHER_OCCUPATION_CD.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of PERSON_JOB</returns>
        internal List<PERSON_JOB> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<PERSON_JOB> list = new List<PERSON_JOB>();

            while (dataReader.Read())
            {
                PERSON_JOB businessObject = new PERSON_JOB();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
