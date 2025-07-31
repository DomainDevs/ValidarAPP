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
    /// Data access layer class for Referencias_agente
    /// </summary>
    class Referencias_agente_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Referencias_agente_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Referencias_agente_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Referencias_agente_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Referencias businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.referencias_agente_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_rol)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@tipo_ref_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.tipo_ref)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref1", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ref1)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref2", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ref2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_producto", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@num_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.num_producto)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Referencias_agente::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Referencias businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.referencias_agente_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_rol)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@tipo_ref_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.tipo_ref)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref1", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ref1)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref2", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ref2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_producto", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@num_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.num_producto)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Referencias_agente::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Referencias businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.referencias_agente_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_rol)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@tipo_ref_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.tipo_ref)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref1", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ref1)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref2", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ref2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_telefono)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_producto", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_producto)));
                sqlCommand.Parameters.Add(new AseParameter("@num_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.num_producto)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Referencias_agente::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Referencias_agente businessObject, IDataReader dataReader)
        {


            businessObject.cod_agente = dataReader.GetInt32(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.cod_agente.ToString()));

            businessObject.cod_tipo_agente = dataReader.GetDouble(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.cod_tipo_agente.ToString()));

            businessObject.tipo_ref_agente = dataReader.GetDouble(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.tipo_ref_agente.ToString()));

            businessObject.sn_ref1 = dataReader.GetInt32(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.sn_ref1.ToString()));

            businessObject.sn_ref2 = dataReader.GetInt32(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.sn_ref2.ToString()));

            businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.txt_nombre.ToString()));

            businessObject.txt_direccion = dataReader.GetString(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.txt_direccion.ToString()));

            businessObject.txt_telefono = dataReader.GetString(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.txt_telefono.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.txt_producto.ToString())))
            {
                businessObject.txt_producto = dataReader.GetString(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.txt_producto.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.num_producto.ToString())))
            {
                businessObject.num_producto = dataReader.GetString(dataReader.GetOrdinal(Referencias_agente.Referencias_agenteFields.num_producto.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Referencias_agente</returns>
        internal List<Referencias_agente> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Referencias_agente> list = new List<Referencias_agente>();

            while (dataReader.Read())
            {
                Referencias_agente businessObject = new Referencias_agente();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
