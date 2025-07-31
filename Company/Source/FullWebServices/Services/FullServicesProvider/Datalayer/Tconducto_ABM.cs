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
    /// Data access layer class for Tconducto
    /// </summary>
    class Tconducto_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tconducto_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tconducto_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tconducto_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Tconducto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tconducto_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;


            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_conducto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_cond", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_cond)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_conducto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_digitos_tarj", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_digitos_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@dd_rend_tarj", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.dd_rend_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_limite", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite)));
                sqlCommand.Parameters.Add(new AseParameter("@id_banco", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_banco)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_pgma_asoc", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_pgma_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_valor_asociado", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_valor_asociado)));
                sqlCommand.Parameters.Add(new AseParameter("@id_bco_receptor_default", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_bco_receptor_default)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble_comision", AseDbType.Text, 13, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cta_cble_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble_cuenta_cobrar", AseDbType.Text, 13, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cta_cble_cuenta_cobrar)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_emision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_emision)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ingresos", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_datafono", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_datafono)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tconducto::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tconducto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tconducto_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_conducto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_cond", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_cond)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_conducto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_digitos_tarj", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_digitos_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@dd_rend_tarj", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.dd_rend_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_limite", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite)));
                sqlCommand.Parameters.Add(new AseParameter("@id_banco", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_banco)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_pgma_asoc", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_pgma_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_valor_asociado", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_valor_asociado)));
                sqlCommand.Parameters.Add(new AseParameter("@id_bco_receptor_default", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_bco_receptor_default)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble_comision", AseDbType.Text, 13, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cta_cble_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble_cuenta_cobrar", AseDbType.Text, 13, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cta_cble_cuenta_cobrar)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_emision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_emision)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ingresos", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_datafono", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_datafono)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));


                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tconducto::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tconducto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tconducto_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_conducto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_cond", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_cond)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_conducto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_conducto)));
                sqlCommand.Parameters.Add(new AseParameter("@cnt_digitos_tarj", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cnt_digitos_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@dd_rend_tarj", AseDbType.Integer, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.dd_rend_tarj)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_limite", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite)));
                sqlCommand.Parameters.Add(new AseParameter("@id_banco", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_banco)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_pgma_asoc", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_pgma_asoc)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_comision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_valor_asociado", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_valor_asociado)));
                sqlCommand.Parameters.Add(new AseParameter("@id_bco_receptor_default", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_bco_receptor_default)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble_comision", AseDbType.Text, 13, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cta_cble_comision)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_cta_cble_cuenta_cobrar", AseDbType.Text, 13, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_cta_cble_cuenta_cobrar)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_emision", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_emision)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_ingresos", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_datafono", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_datafono)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Tconducto::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tconducto businessObject, IDataReader dataReader)
        {


            businessObject.cod_conducto = dataReader.GetDouble(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_conducto.ToString()));

            businessObject.txt_desc_cond = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.txt_desc_cond.ToString()));

            businessObject.cod_tipo_conducto = dataReader.GetDouble(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_tipo_conducto.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.cnt_digitos_tarj.ToString())))
            {
                businessObject.cnt_digitos_tarj = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.cnt_digitos_tarj.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.dd_rend_tarj.ToString())))
            {
                businessObject.dd_rend_tarj = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.dd_rend_tarj.ToString()));
            }

            businessObject.imp_limite = dataReader.GetDouble(dataReader.GetOrdinal(Tconducto.TconductoFields.imp_limite.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.id_banco.ToString())))
            {
                businessObject.id_banco = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.id_banco.ToString()));
            }

            businessObject.txt_pgma_asoc = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.txt_pgma_asoc.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.pje_comision.ToString())))
            {
                businessObject.pje_comision = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.pje_comision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_valor_asociado.ToString())))
            {
                businessObject.cod_valor_asociado = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_valor_asociado.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.id_bco_receptor_default.ToString())))
            {
                businessObject.id_bco_receptor_default = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.id_bco_receptor_default.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_cta_cble_comision.ToString())))
            {
                businessObject.cod_cta_cble_comision = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_cta_cble_comision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_cta_cble_cuenta_cobrar.ToString())))
            {
                businessObject.cod_cta_cble_cuenta_cobrar = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.cod_cta_cble_cuenta_cobrar.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.sn_emision.ToString())))
            {
                businessObject.sn_emision = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.sn_emision.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.sn_ingresos.ToString())))
            {
                businessObject.sn_ingresos = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.sn_ingresos.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tconducto.TconductoFields.sn_datafono.ToString())))
            {
                businessObject.sn_datafono = dataReader.GetString(dataReader.GetOrdinal(Tconducto.TconductoFields.sn_datafono.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tconducto</returns>
        internal List<Tconducto> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tconducto> list = new List<Tconducto>();

            while (dataReader.Read())
            {
                Tconducto businessObject = new Tconducto();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
