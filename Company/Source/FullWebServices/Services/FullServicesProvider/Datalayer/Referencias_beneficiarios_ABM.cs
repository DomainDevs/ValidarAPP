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
    /// Data access layer class for Referencias_beneficiarios
    /// </summary>
    class Referencias_beneficiarios_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Referencias_beneficiarios_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Referencias_beneficiarios_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Referencias_beneficiarios_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
            sqlCommand.CommandText = "SUP.referen_beneficiari_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_rol));
                sqlCommand.Parameters.Add(new AseParameter("@tipo_ref_benef", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.tipo_ref));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref1", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_ref1));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref2", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_ref2));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_nombre));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_direccion));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_telefono));
                sqlCommand.Parameters.Add(new AseParameter("@txt_producto", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_producto));
                sqlCommand.Parameters.Add(new AseParameter("@num_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.num_producto));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Referencias_beneficiarios::Insert::Error occured.", ex);
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
            sqlCommand.CommandText = "SUP.referen_beneficiari_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;            

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_beneficiario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_rol));
                sqlCommand.Parameters.Add(new AseParameter("@tipo_ref_benef", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.tipo_ref));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref1", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_ref1));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ref2", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_ref2));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_nombre));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_direccion));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_telefono));
                sqlCommand.Parameters.Add(new AseParameter("@txt_producto", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_producto));
                sqlCommand.Parameters.Add(new AseParameter("@num_producto", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.num_producto));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Referencias_beneficiarios::Update::Error occured.", ex);
            }        
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Referencias_beneficiarios businessObject, IDataReader dataReader)
        {


            businessObject.cod_beneficiario = dataReader.GetInt32(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.cod_beneficiario.ToString()));

            businessObject.tipo_ref_benef = dataReader.GetDouble(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.tipo_ref_benef.ToString()));

            businessObject.sn_ref1 = dataReader.GetDouble(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.sn_ref1.ToString()));

            businessObject.sn_ref2 = dataReader.GetDouble(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.sn_ref2.ToString()));

            businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.txt_nombre.ToString()));

            businessObject.txt_direccion = dataReader.GetString(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.txt_direccion.ToString()));

            businessObject.txt_telefono = dataReader.GetString(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.txt_telefono.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.txt_producto.ToString())))
            {
                businessObject.txt_producto = dataReader.GetString(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.txt_producto.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.num_producto.ToString())))
            {
                businessObject.num_producto = dataReader.GetString(dataReader.GetOrdinal(Referencias_beneficiarios.Referencias_beneficiariosFields.num_producto.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Referencias_beneficiarios</returns>
        internal List<Referencias_beneficiarios> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Referencias_beneficiarios> list = new List<Referencias_beneficiarios>();

            while (dataReader.Read())
            {
                Referencias_beneficiarios businessObject = new Referencias_beneficiarios();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
