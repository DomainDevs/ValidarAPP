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
    /// Data access layer class for Frm_sarlaft_accionistas_asoc
    /// </summary>
    class Frm_sarlaft_accionistas_asoc_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_accionistas_asoc_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_accionistas_asoc_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_accionistas_asoc_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Frm_sarlaft_accionistas_asoc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_accio_aso_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_asociacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_asociacion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_asoc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_asoc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@txtnombre", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txtnombre)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_estado_Asoc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_estado_Asoc)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_accionistas_asoc::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_accionistas_asoc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_accio_aso_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_asociacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_asociacion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_asoc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_asoc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@txtnombre", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txtnombre)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_estado_Asoc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_estado_Asoc)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_accionistas_asoc::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_accionistas_asoc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_accio_aso_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_asociacion", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_asociacion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc_asoc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_asoc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@txtnombre", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txtnombre)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_estado_Asoc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_estado_Asoc)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_accionistas_asoc::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_accionistas_asoc businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_accionistas_asoc.Frm_sarlaft_accionistas_asocFields.id_persona.ToString()));

            businessObject.nro_asociacion = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_accionistas_asoc.Frm_sarlaft_accionistas_asocFields.nro_asociacion.ToString()));

            businessObject.cod_tipo_doc_asoc = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_accionistas_asoc.Frm_sarlaft_accionistas_asocFields.cod_tipo_doc_asoc.ToString()));

            businessObject.nro_doc_asoc = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_accionistas_asoc.Frm_sarlaft_accionistas_asocFields.nro_doc_asoc.ToString()));

            businessObject.txtnombre = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_accionistas_asoc.Frm_sarlaft_accionistas_asocFields.txtnombre.ToString()));

            businessObject.sn_estado_Asoc = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_accionistas_asoc.Frm_sarlaft_accionistas_asocFields.sn_estado_Asoc.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_accionistas_asoc</returns>
        internal List<Frm_sarlaft_accionistas_asoc> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_accionistas_asoc> list = new List<Frm_sarlaft_accionistas_asoc>();

            while (dataReader.Read())
            {
                Frm_sarlaft_accionistas_asoc businessObject = new Frm_sarlaft_accionistas_asoc();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
