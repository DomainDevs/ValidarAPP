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
    /// Data access layer class for Ttarifa
    /// </summary>
    class Ttarifa_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Ttarifa_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Ttarifa_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Ttarifa_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Ttarifa businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ttarifa_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_subramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_subramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tarifa", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tarifa)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_deduc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_deduc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_item_deduc_defa", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_item_deduc_defa)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_categ", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_categ)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_item_categ_defa", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_item_categ_defa)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_principal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ast", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ast)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_rc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_rc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lim_comun", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lim_comun)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lim_agreg", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lim_agreg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_riesgo", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_riesgo)));
                sqlCommand.Parameters.Add(new AseParameter("@col_cresta", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.col_cresta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_pje", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_pje)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_imprime", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_imprime)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_adicional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_adicional)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_acumsum_reas", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_acumsum_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilita", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilita)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_habilita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_habilita, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_deshabilita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_deshabilita, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Ttarifa::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Ttarifa businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ttarifa_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_subramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_subramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tarifa", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tarifa)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_deduc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_deduc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_item_deduc_defa", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_item_deduc_defa)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_categ", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_categ)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_item_categ_defa", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_item_categ_defa)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_principal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ast", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ast)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_rc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_rc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lim_comun", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lim_comun)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lim_agreg", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lim_agreg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_riesgo", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_riesgo)));
                sqlCommand.Parameters.Add(new AseParameter("@col_cresta", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.col_cresta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_pje", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_pje)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_imprime", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_imprime)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_adicional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_adicional)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_acumsum_reas", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_acumsum_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilita", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilita)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_habilita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_habilita, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_deshabilita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_deshabilita, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Ttarifa::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Ttarifa businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ttarifa_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_subramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_subramo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tarifa", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tarifa)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_deduc", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_deduc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_item_deduc_defa", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_item_deduc_defa)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_categ", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_categ)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_item_categ_defa", AseDbType.Double, 5, ParameterDirection.Input, false, 9, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_item_categ_defa)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_principal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ast", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ast)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_rc", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_rc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lim_comun", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lim_comun)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_lim_agreg", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_lim_agreg)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_riesgo", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_riesgo)));
                sqlCommand.Parameters.Add(new AseParameter("@col_cresta", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.col_cresta)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_pje", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_pje)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_imprime", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_imprime)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_adicional", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_adicional)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_acumsum_reas", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_acumsum_reas)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilita", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilita)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_habilita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_habilita, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_deshabilita", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_deshabilita, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Ttarifa::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Ttarifa businessObject, IDataReader dataReader)
        {


            businessObject.cod_ramo = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_ramo.ToString()));

            businessObject.cod_subramo = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_subramo.ToString()));

            businessObject.cod_tarifa = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_tarifa.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.txt_desc.ToString()));

            businessObject.cod_deduc = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_deduc.ToString()));

            businessObject.cod_item_deduc_defa = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_item_deduc_defa.ToString()));

            businessObject.cod_categ = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_categ.ToString()));

            businessObject.cod_item_categ_defa = dataReader.GetDouble(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_item_categ_defa.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_principal.ToString())))
            {
                businessObject.sn_principal = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_principal.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_ast.ToString())))
            {
                businessObject.sn_ast = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_ast.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_rc.ToString())))
            {
                businessObject.sn_rc = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_rc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.txt_lim_comun.ToString())))
            {
                businessObject.txt_lim_comun = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.txt_lim_comun.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.txt_lim_agreg.ToString())))
            {
                businessObject.txt_lim_agreg = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.txt_lim_agreg.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_riesgo.ToString())))
            {
                businessObject.cod_riesgo = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.cod_riesgo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.col_cresta.ToString())))
            {
                businessObject.col_cresta = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.col_cresta.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_pje.ToString())))
            {
                businessObject.sn_pje = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_pje.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_imprime.ToString())))
            {
                businessObject.sn_imprime = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_imprime.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_adicional.ToString())))
            {
                businessObject.sn_adicional = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_adicional.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_acumsum_reas.ToString())))
            {
                businessObject.sn_acumsum_reas = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_acumsum_reas.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_habilita.ToString())))
            {
                businessObject.sn_habilita = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.sn_habilita.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.fec_habilita.ToString())))
            {
                businessObject.fec_habilita = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.fec_habilita.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Ttarifa.TtarifaFields.fec_deshabilita.ToString())))
            {
                businessObject.fec_deshabilita = dataReader.GetString(dataReader.GetOrdinal(Ttarifa.TtarifaFields.fec_deshabilita.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Ttarifa</returns>
        internal List<Ttarifa> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Ttarifa> list = new List<Ttarifa>();

            while (dataReader.Read())
            {
                Ttarifa businessObject = new Ttarifa();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
