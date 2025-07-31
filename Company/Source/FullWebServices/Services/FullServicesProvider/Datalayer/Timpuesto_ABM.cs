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
    /// Data access layer class for Timpuesto
    /// </summary>
    class Timpuesto_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Timpuesto_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Timpuesto_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Timpuesto_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Timpuesto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.timpuesto_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_impuesto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_reducida", AseDbType.VarChar, 6, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_reducida)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_impuesto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@base_minima", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.base_minima)));
                sqlCommand.Parameters.Add(new AseParameter("@impuesto_minimo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.impuesto_minimo)));
                sqlCommand.Parameters.Add(new AseParameter("@retencion_impuesto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.retencion_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_vigente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_vigente)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_devengado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_devengado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_retencion", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_retencion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_impuesto_base_ret", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impuesto_base_ret)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Timpuesto::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Timpuesto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.timpuesto_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;


            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_impuesto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_reducida", AseDbType.VarChar, 6, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_reducida)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_impuesto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@base_minima", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.base_minima)));
                sqlCommand.Parameters.Add(new AseParameter("@impuesto_minimo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.impuesto_minimo)));
                sqlCommand.Parameters.Add(new AseParameter("@retencion_impuesto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.retencion_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_vigente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_vigente)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_devengado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_devengado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_retencion", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_retencion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_impuesto_base_ret", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impuesto_base_ret)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Timpuesto::Update::Error occured.", ex);
            }         
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Timpuesto businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.timpuesto_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_impuesto", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_reducida", AseDbType.VarChar, 6, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_reducida)));
                sqlCommand.Parameters.Add(new AseParameter("@pje_impuesto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.pje_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@base_minima", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.base_minima)));
                sqlCommand.Parameters.Add(new AseParameter("@impuesto_minimo", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.impuesto_minimo)));
                sqlCommand.Parameters.Add(new AseParameter("@retencion_impuesto", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.retencion_impuesto)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_vigente", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_vigente)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_devengado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_devengado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_retencion", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_retencion)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_impuesto_base_ret", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_impuesto_base_ret)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Timpuesto::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Timpuesto businessObject, IDataReader dataReader)
        {


            businessObject.cod_impuesto = dataReader.GetDouble(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.cod_impuesto.ToString()));

            businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.txt_desc.ToString()));

            businessObject.txt_desc_reducida = dataReader.GetString(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.txt_desc_reducida.ToString()));

            businessObject.pje_impuesto = dataReader.GetInt32(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.pje_impuesto.ToString()));

            businessObject.base_minima = dataReader.GetDouble(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.base_minima.ToString()));

            businessObject.impuesto_minimo = dataReader.GetDouble(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.impuesto_minimo.ToString()));

            businessObject.retencion_impuesto = dataReader.GetInt32(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.retencion_impuesto.ToString()));

            businessObject.sn_vigente = dataReader.GetInt32(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.sn_vigente.ToString()));

            businessObject.sn_devengado = dataReader.GetInt32(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.sn_devengado.ToString()));

            businessObject.sn_retencion = dataReader.GetInt32(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.sn_retencion.ToString()));

            businessObject.cod_impuesto_base_ret = dataReader.GetDouble(dataReader.GetOrdinal(Timpuesto.TimpuestoFields.cod_impuesto_base_ret.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Timpuesto</returns>
        internal List<Timpuesto> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Timpuesto> list = new List<Timpuesto>();

            while (dataReader.Read())
            {
                Timpuesto businessObject = new Timpuesto();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
