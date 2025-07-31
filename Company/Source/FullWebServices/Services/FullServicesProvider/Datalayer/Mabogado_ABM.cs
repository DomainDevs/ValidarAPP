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
    /// Data access layer class for Mabogado
    /// </summary>
    class Mabogado_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mabogado_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mabogado_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mabogado_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mabogado businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mabogado_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_abogado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_abogado)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_vinc_abogado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_vinc_abogado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inhabilitacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inhabilitacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_motivo_inhab", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_motivo_inhab)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@nro_tarj_profesional", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_tarj_profesional)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pto_vta", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pto_vta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mabogado::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mabogado businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mabogado_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_abogado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_abogado)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_vinc_abogado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_vinc_abogado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inhabilitacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inhabilitacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_motivo_inhab", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_motivo_inhab)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@nro_tarj_profesional", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_tarj_profesional)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pto_vta", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pto_vta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mabogado::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mabogado businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mabogado_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_abogado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_abogado)));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_vinc_abogado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_vinc_abogado)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_habilitado", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_habilitado)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_inhabilitacion", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_inhabilitacion, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@txt_motivo_inhab", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_motivo_inhab)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@nro_tarj_profesional", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_tarj_profesional)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pto_vta", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pto_vta)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@fec_ult_modif", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_ult_modif, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mabogado::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mabogado businessObject, IDataReader dataReader)
        {


            businessObject.cod_abogado = dataReader.GetInt32(dataReader.GetOrdinal(Mabogado.MabogadoFields.cod_abogado.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mabogado.MabogadoFields.id_persona.ToString()));

            businessObject.cod_vinc_abogado = dataReader.GetInt32(dataReader.GetOrdinal(Mabogado.MabogadoFields.cod_vinc_abogado.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.sn_habilitado.ToString())))
            {
                businessObject.sn_habilitado = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.sn_habilitado.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_inhabilitacion.ToString())))
            {
                businessObject.fec_inhabilitacion = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_inhabilitacion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.txt_motivo_inhab.ToString())))
            {
                businessObject.txt_motivo_inhab = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.txt_motivo_inhab.ToString()));
            }

            businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_alta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.nro_tarj_profesional.ToString())))
            {
                businessObject.nro_tarj_profesional = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.nro_tarj_profesional.ToString()));
            }

            businessObject.cod_pto_vta = dataReader.GetDouble(dataReader.GetOrdinal(Mabogado.MabogadoFields.cod_pto_vta.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.txt_observaciones.ToString())))
            {
                businessObject.txt_observaciones = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.txt_observaciones.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_ult_modif.ToString())))
            {
                businessObject.fec_ult_modif = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.fec_ult_modif.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mabogado.MabogadoFields.cod_usuario.ToString())))
            {
                businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Mabogado.MabogadoFields.cod_usuario.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mabogado</returns>
        internal List<Mabogado> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mabogado> list = new List<Mabogado>();

            while (dataReader.Read())
            {
                Mabogado businessObject = new Mabogado();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
