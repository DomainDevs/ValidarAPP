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
    /// Data access layer class for Tcpto_pago_egreso_suc
    /// </summary>
    class Tcpto_pago_egreso_suc_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tcpto_pago_egreso_suc_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tcpto_pago_egreso_suc_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tcpto_pago_egreso_suc_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tcpto_pago_egreso_suc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tcpto_pago_egreso_suc_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_cpto));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_suc));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_desc));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_cta_cble));
                sqlCommand.Parameters.Add(new AseParameter("@cod_clase_pago", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_clase_pago));
                sqlCommand.Parameters.Add(new AseParameter("@sn_prestador", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_prestador));
                sqlCommand.Parameters.Add(new AseParameter("@sn_agente", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_agente));
                sqlCommand.Parameters.Add(new AseParameter("@sn_fondofijo", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_fondofijo));
                sqlCommand.Parameters.Add(new AseParameter("@sn_reclamo", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_reclamo));
                sqlCommand.Parameters.Add(new AseParameter("@aplica_a", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.aplica_a));
                sqlCommand.Parameters.Add(new AseParameter("@sn_descuento", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_descuento));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto_retefuente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_cpto_retefuente));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_activo));
                sqlCommand.Parameters.Add(new AseParameter("@id_cta_puente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.id_cta_puente));
                sqlCommand.Parameters.Add(new AseParameter("@sn_proceso_jud", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_proceso_jud));
                sqlCommand.Parameters.Add(new AseParameter("@sn_afecta_impuestos", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_afecta_impuestos));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tcpto_pago_egreso_suc::Insert::Error occured.", ex);
            }     
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tcpto_pago_egreso_suc businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tcpto_pago_egreso_suc_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;                       

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_cpto));
                sqlCommand.Parameters.Add(new AseParameter("@cod_suc", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_suc));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_desc));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_cta_cble));
                sqlCommand.Parameters.Add(new AseParameter("@cod_clase_pago", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_clase_pago));
                sqlCommand.Parameters.Add(new AseParameter("@sn_prestador", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_prestador));
                sqlCommand.Parameters.Add(new AseParameter("@sn_agente", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_agente));
                sqlCommand.Parameters.Add(new AseParameter("@sn_fondofijo", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_fondofijo));
                sqlCommand.Parameters.Add(new AseParameter("@sn_reclamo", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_reclamo));
                sqlCommand.Parameters.Add(new AseParameter("@aplica_a", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.aplica_a));
                sqlCommand.Parameters.Add(new AseParameter("@sn_descuento", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_descuento));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cpto_retefuente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_cpto_retefuente));
                sqlCommand.Parameters.Add(new AseParameter("@sn_activo", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_activo));
                sqlCommand.Parameters.Add(new AseParameter("@id_cta_puente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.id_cta_puente));
                sqlCommand.Parameters.Add(new AseParameter("@sn_proceso_jud", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_proceso_jud));
                sqlCommand.Parameters.Add(new AseParameter("@sn_afecta_impuestos", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.sn_afecta_impuestos));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tcpto_pago_egreso_suc::Update::Error occured.", ex);
            }         
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tcpto_pago_egreso_suc businessObject, IDataReader dataReader)
        {


            businessObject.cod_cpto = dataReader.GetDecimal(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.cod_cpto.ToString()));

            businessObject.cod_suc = dataReader.GetDecimal(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.cod_suc.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.txt_desc.ToString()));

            businessObject.cod_cta_cble = dataReader.GetString(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.cod_cta_cble.ToString()));

            businessObject.cod_clase_pago = dataReader.GetDecimal(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.cod_clase_pago.ToString()));

            businessObject.sn_prestador = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_prestador.ToString()));

            businessObject.sn_agente = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_agente.ToString()));

            businessObject.sn_fondofijo = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_fondofijo.ToString()));

            businessObject.sn_reclamo = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_reclamo.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.aplica_a.ToString())))
            {
                businessObject.aplica_a = dataReader.GetString(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.aplica_a.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_descuento.ToString())))
            {
                businessObject.sn_descuento = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_descuento.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.cod_cpto_retefuente.ToString())))
            {
                businessObject.cod_cpto_retefuente = dataReader.GetInt32(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.cod_cpto_retefuente.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_activo.ToString())))
            {
                businessObject.sn_activo = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_activo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.id_cta_puente.ToString())))
            {
                businessObject.id_cta_puente = dataReader.GetInt32(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.id_cta_puente.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_proceso_jud.ToString())))
            {
                businessObject.sn_proceso_jud = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_proceso_jud.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_afecta_impuestos.ToString())))
            {
                businessObject.sn_afecta_impuestos = dataReader.GetDouble(dataReader.GetOrdinal(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields.sn_afecta_impuestos.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tcpto_pago_egreso_suc</returns>
        internal List<Tcpto_pago_egreso_suc> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tcpto_pago_egreso_suc> list = new List<Tcpto_pago_egreso_suc>();

            while (dataReader.Read())
            {
                Tcpto_pago_egreso_suc businessObject = new Tcpto_pago_egreso_suc();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
