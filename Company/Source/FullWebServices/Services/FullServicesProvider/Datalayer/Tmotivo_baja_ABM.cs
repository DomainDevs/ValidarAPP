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
    /// Data access layer class for Tmotivo_baja
    /// </summary>
    class Tmotivo_baja_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tmotivo_baja_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tmotivo_baja_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tmotivo_baja_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tmotivo_baja businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tmotivo_baja_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_maseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_maseg)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_magente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_magente)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_mpres", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_mpres)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tercero", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tercero)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cesionario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cesionario)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_beneficiario)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tmotivo_baja::Insert::Error occured.", ex);
            }            
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tmotivo_baja businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tmotivo_baja_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_maseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_maseg)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_magente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_magente)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_mpres", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_mpres)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tercero", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tercero)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cesionario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cesionario)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_beneficiario)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tmotivo_baja::Update::Error occured.", ex);
            }     
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tmotivo_baja businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tmotivo_baja_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_baja)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_maseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_maseg)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_magente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_magente)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_mpres", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_mpres)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tercero", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tercero)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_cesionario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_cesionario)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_beneficiario)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tmotivo_baja::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tmotivo_baja businessObject, IDataReader dataReader)
        {


            businessObject.cod_baja = dataReader.GetDouble(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.cod_baja.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.txt_desc.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_maseg.ToString())))
            {
                businessObject.sn_maseg = dataReader.GetString(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_maseg.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_magente.ToString())))
            {
                businessObject.sn_magente = dataReader.GetString(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_magente.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_mpres.ToString())))
            {
                businessObject.sn_mpres = dataReader.GetString(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_mpres.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_tercero.ToString())))
            {
                businessObject.sn_tercero = dataReader.GetString(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_tercero.ToString()));
            }

            businessObject.sn_cesionario = dataReader.GetInt32(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_cesionario.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_beneficiario.ToString())))
            {
                businessObject.sn_beneficiario = dataReader.GetString(dataReader.GetOrdinal(Tmotivo_baja.Tmotivo_bajaFields.sn_beneficiario.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tmotivo_baja</returns>
        internal List<Tmotivo_baja> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tmotivo_baja> list = new List<Tmotivo_baja>();

            while (dataReader.Read())
            {
                Tmotivo_baja businessObject = new Tmotivo_baja();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
