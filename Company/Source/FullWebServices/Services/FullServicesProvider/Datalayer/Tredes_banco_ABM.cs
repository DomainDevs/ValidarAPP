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
    /// Data access layer class for Tredes_banco
    /// </summary>
    class Tredes_banco_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tredes_banco_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tredes_banco_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tredes_banco_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tredes_banco businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tredes_banco_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_requiere_autoriz", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_requiere_autoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_correlativo_negocio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_correlativo_negocio)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_lote", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_lote)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_red_asociado", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red_asociado)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_archivo_autoriz", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_archivo_autoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_archivo_recaudo", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_archivo_recaudo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_control", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_control)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_cabecera", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_cabecera)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tredes_banco::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tredes_banco businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tredes_banco_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_requiere_autoriz", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_requiere_autoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_correlativo_negocio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_correlativo_negocio)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_lote", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_lote)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_red_asociado", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red_asociado)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_archivo_autoriz", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_archivo_autoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_archivo_recaudo", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_archivo_recaudo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_control", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_control)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_cabecera", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_cabecera)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tredes_banco::Update::Error occured.", ex);
            }          
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tredes_banco businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tredes_banco_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_requiere_autoriz", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_requiere_autoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_correlativo_negocio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_correlativo_negocio)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_lote", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_lote)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_red_asociado", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red_asociado)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_archivo_autoriz", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_archivo_autoriz)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre_archivo_recaudo", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_nombre_archivo_recaudo)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_control", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_control)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_tiene_cabecera", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_tiene_cabecera)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tredes_banco::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tredes_banco businessObject, IDataReader dataReader)
        {


            businessObject.cod_red = dataReader.GetDouble(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.cod_red.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.txt_nombre.ToString())))
            {
                businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.txt_nombre.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_requiere_autoriz.ToString())))
            {
                businessObject.sn_requiere_autoriz = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_requiere_autoriz.ToString()));
            }

            businessObject.nro_correlativo_negocio = dataReader.GetInt32(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.nro_correlativo_negocio.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_tiene_lote.ToString())))
            {
                businessObject.sn_tiene_lote = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_tiene_lote.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.cod_red_asociado.ToString())))
            {
                businessObject.cod_red_asociado = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.cod_red_asociado.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.txt_nombre_archivo_autoriz.ToString())))
            {
                businessObject.txt_nombre_archivo_autoriz = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.txt_nombre_archivo_autoriz.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.txt_nombre_archivo_recaudo.ToString())))
            {
                businessObject.txt_nombre_archivo_recaudo = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.txt_nombre_archivo_recaudo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_tiene_control.ToString())))
            {
                businessObject.sn_tiene_control = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_tiene_control.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_tiene_cabecera.ToString())))
            {
                businessObject.sn_tiene_cabecera = dataReader.GetString(dataReader.GetOrdinal(Tredes_banco.Tredes_bancoFields.sn_tiene_cabecera.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tredes_banco</returns>
        internal List<Tredes_banco> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tredes_banco> list = new List<Tredes_banco>();

            while (dataReader.Read())
            {
                Tredes_banco businessObject = new Tredes_banco();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
