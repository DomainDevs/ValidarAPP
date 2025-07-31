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
    /// Data access layer class for Tramo
    /// </summary>
    class Tramo_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tramo_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tramo_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tramo_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tramo businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tramo_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_abrev", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_abrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_redu", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_redu)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_iva", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_iva)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_bomberos", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_bomberos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_rrc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_rrc)));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro_poliza", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ult_nro_poliza)));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro_stro", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ult_nro_stro)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_det_cta_cte", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_det_cta_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_super", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_super)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_fasecolda", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_fasecolda)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_emision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_emision)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_ptos_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_ptos_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_rolfil", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_rolfil)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_cumulo_reas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_cumulo_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_ejer_reas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_ejer_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_estado", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_estado)));
                sqlCommand.Parameters.Add(new AseParameter("@fecha_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fecha_modificacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tramo::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tramo businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tramo_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_abrev", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_abrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_redu", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_redu)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_iva", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_iva)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_bomberos", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_bomberos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_rrc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_rrc)));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro_poliza", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ult_nro_poliza)));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro_stro", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ult_nro_stro)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_det_cta_cte", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_det_cta_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_super", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_super)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_fasecolda", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_fasecolda)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_emision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_emision)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_ptos_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_ptos_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_rolfil", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_rolfil)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_cumulo_reas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_cumulo_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_ejer_reas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_ejer_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_estado", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_estado)));
                sqlCommand.Parameters.Add(new AseParameter("@fecha_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fecha_modificacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tramo::Update::Error occured.", ex);
            }        
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tramo businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tramo_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_abrev", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_abrev)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_redu", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_redu)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_iva", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_iva)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_bomberos", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_bomberos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_rrc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_rrc)));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro_poliza", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ult_nro_poliza)));
                sqlCommand.Parameters.Add(new AseParameter("@ult_nro_stro", AseDbType.Double, 4, ParameterDirection.Input, false, 7, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ult_nro_stro)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_det_cta_cte", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_det_cta_cte)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_grupo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_grupo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_super", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_super)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_fasecolda", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_fasecolda)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_emision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_emision)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_ptos_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_ptos_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_rolfil", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_rolfil)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo_cumulo_reas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo_cumulo_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_ejer_reas", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_ejer_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_estado", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_estado)));
                sqlCommand.Parameters.Add(new AseParameter("@fecha_modificacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fecha_modificacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tramo::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tramo businessObject, IDataReader dataReader)
        {


            businessObject.cod_ramo = dataReader.GetDouble(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo.ToString()));

            businessObject.txt_desc_abrev = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.txt_desc_abrev.ToString()));

            businessObject.txt_desc_redu = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.txt_desc_redu.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.txt_desc.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.sn_iva.ToString())))
            {
                businessObject.sn_iva = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.sn_iva.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.pje_bomberos.ToString())))
            {
                businessObject.pje_bomberos = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.pje_bomberos.ToString()));
            }

            businessObject.cod_ttipo_ramo = dataReader.GetDouble(dataReader.GetOrdinal(Tramo.TramoFields.cod_ttipo_ramo.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.pje_rrc.ToString())))
            {
                businessObject.pje_rrc = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.pje_rrc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.ult_nro_poliza.ToString())))
            {
                businessObject.ult_nro_poliza = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.ult_nro_poliza.ToString()));
            }

            businessObject.ult_nro_stro = dataReader.GetDouble(dataReader.GetOrdinal(Tramo.TramoFields.ult_nro_stro.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.sn_det_cta_cte.ToString())))
            {
                businessObject.sn_det_cta_cte = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.sn_det_cta_cte.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cod_grupo.ToString())))
            {
                businessObject.cod_grupo = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cod_grupo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo_super.ToString())))
            {
                businessObject.cod_ramo_super = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo_super.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo_fasecolda.ToString())))
            {
                businessObject.cod_ramo_fasecolda = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo_fasecolda.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.sn_emision.ToString())))
            {
                businessObject.sn_emision = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.sn_emision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cnt_ptos_comision.ToString())))
            {
                businessObject.cnt_ptos_comision = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cnt_ptos_comision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cod_rolfil.ToString())))
            {
                businessObject.cod_rolfil = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cod_rolfil.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo_cumulo_reas.ToString())))
            {
                businessObject.cod_ramo_cumulo_reas = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cod_ramo_cumulo_reas.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tramo.TramoFields.cod_tipo_ejer_reas.ToString())))
            {
                businessObject.cod_tipo_ejer_reas = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.cod_tipo_ejer_reas.ToString()));
            }

            businessObject.cod_estado = dataReader.GetDouble(dataReader.GetOrdinal(Tramo.TramoFields.cod_estado.ToString()));

            businessObject.fecha_modificacion = dataReader.GetString(dataReader.GetOrdinal(Tramo.TramoFields.fecha_modificacion.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tramo</returns>
        internal List<Tramo> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tramo> list = new List<Tramo>();

            while (dataReader.Read())
            {
                Tramo businessObject = new Tramo();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
