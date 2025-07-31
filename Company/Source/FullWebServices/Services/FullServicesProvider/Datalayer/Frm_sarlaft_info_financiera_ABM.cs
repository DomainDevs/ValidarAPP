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
    /// Data access layer class for Frm_sarlaft_info_financiera
    /// </summary>
    class Frm_sarlaft_info_financiera_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_info_financiera_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_info_financiera_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Frm_sarlaft_info_financiera_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Frm_sarlaft_info_financiera businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_info_fin_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ingresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_egresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_egresos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingr", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingr)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_act_secundaria", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_act_secundaria)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_act_principal", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_act_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ppal_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ppal_nuevo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_scndria_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_scndria_nuevo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_info_financiera::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Frm_sarlaft_info_financiera businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_info_fin_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ingresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_egresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_egresos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingr", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingr)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_act_secundaria", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_act_secundaria)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_act_principal", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_act_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ppal_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ppal_nuevo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_scndria_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_scndria_nuevo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_info_financiera::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Frm_sarlaft_info_financiera businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.sarlaft_info_fin_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_formulario", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_formulario)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_ingresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_ingresos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_egresos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_egresos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_otros_ingr", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_otros_ingr)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_act_secundaria", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_act_secundaria)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_activos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_activos)));
                sqlCommand.Parameters.Add(new AseParameter("@imp_pasivos", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_pasivos)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_act_principal", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_act_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ppal_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ppal_nuevo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_scndria_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_scndria_nuevo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Frm_sarlaft_info_financiera::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Frm_sarlaft_info_financiera businessObject, IDataReader dataReader)
        {


            businessObject.id_formulario = dataReader.GetInt32(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.id_formulario.ToString()));

            businessObject.imp_ingresos = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.imp_ingresos.ToString()));

            businessObject.imp_egresos = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.imp_egresos.ToString()));

            businessObject.imp_otros_ingr = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.imp_otros_ingr.ToString()));

            businessObject.cod_act_secundaria = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.cod_act_secundaria.ToString()));

            businessObject.imp_activos = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.imp_activos.ToString()));

            businessObject.imp_pasivos = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.imp_pasivos.ToString()));

            businessObject.cod_act_principal = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.cod_act_principal.ToString()));

            businessObject.cod_ciiu_ppal_nuevo = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.cod_ciiu_ppal_nuevo.ToString()));

            businessObject.cod_ciiu_scndria_nuevo = dataReader.GetDouble(dataReader.GetOrdinal(Frm_sarlaft_info_financiera.Frm_sarlaft_info_financieraFields.cod_ciiu_scndria_nuevo.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Frm_sarlaft_info_financiera</returns>
        internal List<Frm_sarlaft_info_financiera> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Frm_sarlaft_info_financiera> list = new List<Frm_sarlaft_info_financiera>();

            while (dataReader.Read())
            {
                Frm_sarlaft_info_financiera businessObject = new Frm_sarlaft_info_financiera();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
