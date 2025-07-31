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
    /// Data access layer class for Frm_sarlaft_aut_incrementos
    /// </summary>
    class Frm_sarlaft_aut_incrementos_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_aut_incrementos_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_aut_incrementos_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_aut_incrementos_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Frm_sarlaft_aut_incrementos businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_aut_incre_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaa_formulario", AseDbType.Integer, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaa_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_formulario", AseDbType.Double, 6, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_solicita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_solicita, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vig_desde", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vig_desde, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vig_hasta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vig_hasta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_autoriza", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_autoriza, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_anterior", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_anterior)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_actual", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_actual)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_autoriza", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_solic", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_solic)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_autoriza", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_motivo_autoriza", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_motivo_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_procesado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_procesado)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_aut_incrementos::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_aut_incrementos businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_aut_incre_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaa_formulario", AseDbType.Integer, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaa_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_formulario", AseDbType.Double, 6, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_solicita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_solicita, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vig_desde", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vig_desde, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vig_hasta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vig_hasta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_autoriza", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_autoriza, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_anterior", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_anterior)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_actual", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_actual)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_autoriza", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_solic", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_solic)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_autoriza", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_motivo_autoriza", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_motivo_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_procesado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_procesado)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_aut_incrementos::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_aut_incrementos businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_aut_incre_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaa_formulario", AseDbType.Integer, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaa_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_formulario", AseDbType.Double, 6, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_suc)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_solicita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_solicita, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.VarChar, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vig_desde", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vig_desde, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_vig_hasta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_vig_hasta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_autoriza", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_autoriza, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@imp_anterior", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_anterior)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_actual", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_actual)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_autoriza", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_solic", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_solic)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario_autoriza", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_motivo_autoriza", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_motivo_autoriza)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_procesado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_procesado)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_aut_incrementos::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_aut_incrementos businessObject, IDataReader dataReader)
        {


            businessObject.id_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.id_formulario.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.id_persona.ToString()));

            businessObject.aaaa_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.aaaa_formulario.ToString()));

            businessObject.nro_formulario = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.nro_formulario.ToString()));

            businessObject.cod_suc = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_suc.ToString()));

            businessObject.fec_solicita = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.fec_solicita.ToString()));

            businessObject.cod_tipo_persona = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_tipo_persona.ToString()));

            businessObject.fec_vig_desde = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.fec_vig_desde.ToString()));

            businessObject.fec_vig_hasta = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.fec_vig_hasta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.fec_autoriza.ToString())))
            {
                businessObject.fec_autoriza = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.fec_autoriza.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.imp_anterior.ToString())))
            {
                businessObject.imp_anterior = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.imp_anterior.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.imp_actual.ToString())))
            {
                businessObject.imp_actual = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.imp_actual.ToString()));
            }

            businessObject.sn_autoriza = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.sn_autoriza.ToString()));

            businessObject.cod_usuario_solic = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_usuario_solic.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_usuario_autoriza.ToString())))
            {
                businessObject.cod_usuario_autoriza = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_usuario_autoriza.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_motivo_autoriza.ToString())))
            {
                businessObject.cod_motivo_autoriza = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.cod_motivo_autoriza.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.sn_procesado.ToString())))
            {
                businessObject.sn_procesado = dataReader.GetString(dataReader.GetOrdinal(Frm_sarlaft_aut_incrementos.Frm_sarlaft_aut_incrementosFields.sn_procesado.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_aut_incrementos</returns>
        internal List<Frm_sarlaft_aut_incrementos> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_aut_incrementos> list = new List<Frm_sarlaft_aut_incrementos>();

            while (dataReader.Read())
            {
                Frm_sarlaft_aut_incrementos businessObject = new Frm_sarlaft_aut_incrementos();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
