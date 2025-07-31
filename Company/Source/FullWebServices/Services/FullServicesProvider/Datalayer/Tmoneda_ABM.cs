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
    /// Data access layer class for Tmoneda
    /// </summary>
    class Tmoneda_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tmoneda_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tmoneda_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tmoneda_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tmoneda businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tmoneda_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_redu", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_redu)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_dif_max", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_dif_max)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_decimales", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_decimales)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_decimales_cambio", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_decimales_cambio)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_desvio_cambio_ingreso", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_desvio_cambio_ingreso)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_desvio_cambio_aplicacion", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_desvio_cambio_aplicacion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tmoneda::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tmoneda businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tmoneda_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_redu", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_redu)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_dif_max", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_dif_max)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_decimales", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_decimales)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_decimales_cambio", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_decimales_cambio)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_desvio_cambio_ingreso", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_desvio_cambio_ingreso)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_desvio_cambio_aplicacion", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_desvio_cambio_aplicacion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tmoneda::Update::Error occured.", ex);
            }
            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tmoneda businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tmoneda_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_redu", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_redu)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_dif_max", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_dif_max)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_decimales", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_decimales)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_decimales_cambio", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_decimales_cambio)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_desvio_cambio_ingreso", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_desvio_cambio_ingreso)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_desvio_cambio_aplicacion", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_desvio_cambio_aplicacion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tmoneda::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tmoneda businessObject, IDataReader dataReader)
        {


            businessObject.cod_moneda = dataReader.GetDouble(dataReader.GetOrdinal(Tmoneda.TmonedaFields.cod_moneda.ToString()));

            businessObject.txt_desc_redu = dataReader.GetString(dataReader.GetOrdinal(Tmoneda.TmonedaFields.txt_desc_redu.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Tmoneda.TmonedaFields.txt_desc.ToString()));

            businessObject.imp_dif_max = dataReader.GetDouble(dataReader.GetOrdinal(Tmoneda.TmonedaFields.imp_dif_max.ToString()));

            businessObject.cnt_decimales = dataReader.GetDouble(dataReader.GetOrdinal(Tmoneda.TmonedaFields.cnt_decimales.ToString()));

            businessObject.cnt_decimales_cambio = dataReader.GetDouble(dataReader.GetOrdinal(Tmoneda.TmonedaFields.cnt_decimales_cambio.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmoneda.TmonedaFields.pje_desvio_cambio_ingreso.ToString())))
            {
                businessObject.pje_desvio_cambio_ingreso = dataReader.GetString(dataReader.GetOrdinal(Tmoneda.TmonedaFields.pje_desvio_cambio_ingreso.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tmoneda.TmonedaFields.pje_desvio_cambio_aplicacion.ToString())))
            {
                businessObject.pje_desvio_cambio_aplicacion = dataReader.GetString(dataReader.GetOrdinal(Tmoneda.TmonedaFields.pje_desvio_cambio_aplicacion.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tmoneda</returns>
        internal List<Tmoneda> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tmoneda> list = new List<Tmoneda>();

            while (dataReader.Read())
            {
                Tmoneda businessObject = new Tmoneda();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
