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
    /// Data access layer class for Tplaza_trans_banc
    /// </summary>
    class Tplaza_trans_banc_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tplaza_trans_banc_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tplaza_trans_banc_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tplaza_trans_banc_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tplaza_trans_banc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tplaza_trans_banc_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_plaza", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_red", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_red)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_plaza", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tplaza_trans_banc::Insert::Error occured.", ex);
            }            
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tplaza_trans_banc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tplaza_trans_banc_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_plaza", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_red", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_red)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_plaza", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tplaza_trans_banc::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tplaza_trans_banc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tplaza_trans_banc_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_plaza", AseDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_red", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_red)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_plaza", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_plaza)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tplaza_trans_banc::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tplaza_trans_banc businessObject, IDataReader dataReader)
        {


            businessObject.cod_plaza = dataReader.GetString(dataReader.GetOrdinal(Tplaza_trans_banc.Tplaza_trans_bancFields.cod_plaza.ToString()));

            businessObject.cod_tipo_red = dataReader.GetInt32(dataReader.GetOrdinal(Tplaza_trans_banc.Tplaza_trans_bancFields.cod_tipo_red.ToString()));

            businessObject.cod_dpto = dataReader.GetDouble(dataReader.GetOrdinal(Tplaza_trans_banc.Tplaza_trans_bancFields.cod_dpto.ToString()));

            businessObject.cod_municipio = dataReader.GetDouble(dataReader.GetOrdinal(Tplaza_trans_banc.Tplaza_trans_bancFields.cod_municipio.ToString()));

            businessObject.txt_plaza = dataReader.GetString(dataReader.GetOrdinal(Tplaza_trans_banc.Tplaza_trans_bancFields.txt_plaza.ToString()));

            businessObject.sn_habilitado = dataReader.GetInt32(dataReader.GetOrdinal(Tplaza_trans_banc.Tplaza_trans_bancFields.sn_habilitado.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tplaza_trans_banc</returns>
        internal List<Tplaza_trans_banc> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tplaza_trans_banc> list = new List<Tplaza_trans_banc>();

            while (dataReader.Read())
            {
                Tplaza_trans_banc businessObject = new Tplaza_trans_banc();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
