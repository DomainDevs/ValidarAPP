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
    /// Data access layer class for Maseg_conducto
    /// </summary>
    class Maseg_conducto_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_conducto_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_conducto_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Maseg_conducto_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Maseg_conducto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_conducto_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@ind_conducto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ind_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_conducto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cta_tarj", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cta_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaamm_vto_tarj", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaamm_vto_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_limite_tarj", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_banco_emisor_tarj", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_banco_emisor_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_autorizada", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_autorizada)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_rechazos", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_rechazos)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_autorizacion", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_autorizacion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_secuencia", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_secuencia)));
                sqlCommand.Parameters.Add(new AseParameter("@id_negocio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_negocio)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_respeta_secuencia", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_respeta_secuencia)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_autorizacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_autorizacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_conducto::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Maseg_conducto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_conducto_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@ind_conducto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ind_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_conducto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cta_tarj", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cta_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaamm_vto_tarj", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaamm_vto_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_limite_tarj", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_banco_emisor_tarj", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_banco_emisor_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_autorizada", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_autorizada)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_rechazos", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_rechazos)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_autorizacion", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_autorizacion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_secuencia", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_secuencia)));
                sqlCommand.Parameters.Add(new AseParameter("@id_negocio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_negocio)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_respeta_secuencia", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_respeta_secuencia)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_autorizacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_autorizacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_conducto::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Maseg_conducto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.maseg_conducto_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_aseg", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_aseg)));
                sqlCommand.Parameters.Add(new AseParameter("@ind_conducto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ind_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_conducto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cta_tarj", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cta_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@aaaamm_vto_tarj", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.aaaamm_vto_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_limite_tarj", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_banco_emisor_tarj", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_banco_emisor_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_autorizada", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_autorizada)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_dias_rechazos", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_dias_rechazos)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_autorizacion", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_autorizacion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_secuencia", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_secuencia)));
                sqlCommand.Parameters.Add(new AseParameter("@id_negocio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_negocio)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_respeta_secuencia", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_respeta_secuencia)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_autorizacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_autorizacion, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Maseg_conducto::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Maseg_conducto businessObject, IDataReader dataReader)
        {


            businessObject.cod_aseg = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cod_aseg.ToString()));

            businessObject.ind_conducto = dataReader.GetInt32(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.ind_conducto.ToString()));

            businessObject.cod_conducto = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cod_conducto.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cod_red.ToString())))
            {
                businessObject.cod_red = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cod_red.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.nro_cta_tarj.ToString())))
            {
                businessObject.nro_cta_tarj = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.nro_cta_tarj.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.aaaamm_vto_tarj.ToString())))
            {
                businessObject.aaaamm_vto_tarj = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.aaaamm_vto_tarj.ToString()));
            }

            businessObject.imp_limite_tarj = dataReader.GetDouble(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.imp_limite_tarj.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cod_banco_emisor_tarj.ToString())))
            {
                businessObject.cod_banco_emisor_tarj = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cod_banco_emisor_tarj.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.sn_habilitado.ToString())))
            {
                businessObject.sn_habilitado = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.sn_habilitado.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.sn_autorizada.ToString())))
            {
                businessObject.sn_autorizada = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.sn_autorizada.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cnt_dias_rechazos.ToString())))
            {
                businessObject.cnt_dias_rechazos = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.cnt_dias_rechazos.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.nro_autorizacion.ToString())))
            {
                businessObject.nro_autorizacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.nro_autorizacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.nro_secuencia.ToString())))
            {
                businessObject.nro_secuencia = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.nro_secuencia.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.id_negocio.ToString())))
            {
                businessObject.id_negocio = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.id_negocio.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.sn_respeta_secuencia.ToString())))
            {
                businessObject.sn_respeta_secuencia = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.sn_respeta_secuencia.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.fec_autorizacion.ToString())))
            {
                businessObject.fec_autorizacion = dataReader.GetString(dataReader.GetOrdinal(Maseg_conducto.Maseg_conductoFields.fec_autorizacion.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Maseg_conducto</returns>
        internal List<Maseg_conducto> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Maseg_conducto> list = new List<Maseg_conducto>();

            while (dataReader.Read())
            {
                Maseg_conducto businessObject = new Maseg_conducto();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
