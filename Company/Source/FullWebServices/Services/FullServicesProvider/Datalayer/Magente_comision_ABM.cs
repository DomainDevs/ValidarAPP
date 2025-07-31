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
    /// Data access layer class for Magente_comision
    /// </summary>
    class Magente_comision_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_comision_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_comision_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Magente_comision_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Magente_comision businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_comision_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_subramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_subramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_cob", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_cob)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_cob", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_cob)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_normal", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_normal)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_normal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_normal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_extra", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_extra)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_extra", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_extra)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_recargo_adm", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_recargo_adm)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Magente_comision::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Magente_comision businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_comision_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_subramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_subramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_cob", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_cob)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_cob", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_cob)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_normal", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_normal)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_normal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_normal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_extra", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_extra)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_extra", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_extra)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_recargo_adm", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_recargo_adm)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Magente_comision::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Magente_comision businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.magente_comision_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_agente", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_agente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_agente)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_subramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_subramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_cob", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_cob)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_cob", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_cob)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_normal", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_normal)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_normal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_normal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calc_comis_extra", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calc_comis_extra)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comis_extra", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comis_extra)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_recargo_adm", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_recargo_adm)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Magente_comision::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Magente_comision businessObject, IDataReader dataReader)
        {


            businessObject.cod_tipo_agente = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_tipo_agente.ToString()));

            businessObject.cod_agente = dataReader.GetInt32(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_agente.ToString()));

            businessObject.cod_ramo = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_ramo.ToString()));

            businessObject.cod_subramo = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_subramo.ToString()));

            businessObject.cod_moneda = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_moneda.ToString()));

            businessObject.cod_calc_comis_cob = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_calc_comis_cob.ToString()));

            businessObject.pje_comis_cob = dataReader.GetInt32(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.pje_comis_cob.ToString()));

            businessObject.cod_calc_comis_normal = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_calc_comis_normal.ToString()));

            businessObject.pje_comis_normal = dataReader.GetInt32(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.pje_comis_normal.ToString()));

            businessObject.cod_calc_comis_extra = dataReader.GetDouble(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.cod_calc_comis_extra.ToString()));

            businessObject.pje_comis_extra = dataReader.GetInt32(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.pje_comis_extra.ToString()));

            businessObject.pje_recargo_adm = dataReader.GetInt32(dataReader.GetOrdinal(Magente_comision.Magente_comisionFields.pje_recargo_adm.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Magente_comision</returns>
        internal List<Magente_comision> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Magente_comision> list = new List<Magente_comision>();

            while (dataReader.Read())
            {
                Magente_comision businessObject = new Magente_comision();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
