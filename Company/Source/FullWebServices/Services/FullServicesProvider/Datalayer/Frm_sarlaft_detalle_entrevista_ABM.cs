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
    /// Data access layer class for Frm_sarlaft_detalle_entrevista
    /// </summary>
    class Frm_sarlaft_detalle_entrevista_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_detalle_entrevista_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_detalle_entrevista_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_detalle_entrevista_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Frm_sarlaft_detalle_entrevista businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_det_entrv_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_entrev", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obser_entrev", AseDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obser_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_resul_entrev", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_resul_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_entrevista", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_entrevista, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usua_entrev", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usua_entrev)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_detalle_entrevista::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_detalle_entrevista businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_det_entrv_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_entrev", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obser_entrev", AseDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obser_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_resul_entrev", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_resul_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_entrevista", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_entrevista, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usua_entrev", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usua_entrev)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_detalle_entrevista::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_detalle_entrevista businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_det_entrv_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lugar_entrev", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lugar_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_obser_entrev", AseDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_obser_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_resul_entrev", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_resul_entrev)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_entrevista", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_entrevista, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_usua_entrev", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_usua_entrev)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_detalle_entrevista::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_detalle_entrevista businessObject, IDataReader dataReader)
        {


            businessObject.id_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_detalle_entrevista.Frm_sarlaft_detalle_entrevistaFields.id_formulario.ToString()));

            businessObject.txt_lugar_entrev = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle_entrevista.Frm_sarlaft_detalle_entrevistaFields.txt_lugar_entrev.ToString()));

            businessObject.txt_obser_entrev = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle_entrevista.Frm_sarlaft_detalle_entrevistaFields.txt_obser_entrev.ToString()));

            businessObject.txt_resul_entrev = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle_entrevista.Frm_sarlaft_detalle_entrevistaFields.txt_resul_entrev.ToString()));

            businessObject.fec_entrevista = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle_entrevista.Frm_sarlaft_detalle_entrevistaFields.fec_entrevista.ToString()));

            businessObject.txt_usua_entrev = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_detalle_entrevista.Frm_sarlaft_detalle_entrevistaFields.txt_usua_entrev.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_detalle_entrevista</returns>
        internal List<Frm_sarlaft_detalle_entrevista> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_detalle_entrevista> list = new List<Frm_sarlaft_detalle_entrevista>();

            while (dataReader.Read())
            {
                Frm_sarlaft_detalle_entrevista businessObject = new Frm_sarlaft_detalle_entrevista();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
