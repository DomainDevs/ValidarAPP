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
    /// Data access layer class for Maseg_pmin_gastos_emi
    /// </summary>
    class Maseg_pmin_gastos_emi_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_pmin_gastos_emi_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_pmin_gastos_emi_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_pmin_gastos_emi_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Maseg_pmin_gastos_emi businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_pmin_gast_emi_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_prima_min", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_prima_min)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_gastos_emi", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_gastos_emi)));
                sqlCommand.Parameters.Add(new AseParameter("@suma_aseg_max", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.suma_aseg_max)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_pmin_gastos_emi::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Maseg_pmin_gastos_emi businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_pmin_gast_emi_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_prima_min", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_prima_min)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_gastos_emi", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_gastos_emi)));
                sqlCommand.Parameters.Add(new AseParameter("@suma_aseg_max", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.suma_aseg_max)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_pmin_gastos_emi::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Maseg_pmin_gastos_emi businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_pmin_gastos_emi_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_prima_min", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_prima_min)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_gastos_emi", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_gastos_emi)));
                sqlCommand.Parameters.Add(new AseParameter("@suma_aseg_max", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.suma_aseg_max)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_pmin_gastos_emi::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Maseg_pmin_gastos_emi businessObject, IDataReader dataReader)
        {


            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields.cod_aseg.ToString()));

            businessObject.cod_ramo = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields.cod_ramo.ToString()));

            businessObject.cod_moneda = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields.cod_moneda.ToString()));

            businessObject.imp_prima_min = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields.imp_prima_min.ToString()));

            businessObject.imp_gastos_emi = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields.imp_gastos_emi.ToString()));

            businessObject.suma_aseg_max = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields.suma_aseg_max.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_pmin_gastos_emi</returns>
        internal List<Maseg_pmin_gastos_emi> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Maseg_pmin_gastos_emi> list = new List<Maseg_pmin_gastos_emi>();

            while (dataReader.Read())
            {
                Maseg_pmin_gastos_emi businessObject = new Maseg_pmin_gastos_emi();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
