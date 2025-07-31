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
    /// Data access layer class for Magente_producto
    /// </summary>
    class Magente_producto_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_producto_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_producto_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_producto_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Magente_producto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_producto_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_producto", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_producto)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Magente_producto::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Magente_producto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_producto_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_producto", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_producto)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Magente_producto::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Magente_producto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_producto_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_producto", AseDbType.Double, 3, ParameterDirection.Input, false, 4, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_producto)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Magente_producto::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Magente_producto businessObject, IDataReader dataReader)
        {


            businessObject.cod_tipo_agente = dataReader.GetDouble(dataReader.GetOrdinal(Magente_producto.Magente_productoFields.cod_tipo_agente.ToString()));

            businessObject.cod_agente = dataReader.GetInt32(dataReader.GetOrdinal(Magente_producto.Magente_productoFields.cod_agente.ToString()));

            businessObject.cod_producto = dataReader.GetDouble(dataReader.GetOrdinal(Magente_producto.Magente_productoFields.cod_producto.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Magente_producto</returns>
        internal List<Magente_producto> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Magente_producto> list = new List<Magente_producto>();

            while (dataReader.Read())
            {
                Magente_producto businessObject = new Magente_producto();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
