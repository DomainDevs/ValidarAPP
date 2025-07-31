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
    /// Data access layer class for Mtercero
    /// </summary>
    class Mtercero_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mtercero_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mtercero_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mtercero_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mtercero businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mtercero_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tercero", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_tercero));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.id_persona));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_nombre));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_direccion));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_telefono));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_tipo_persona));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_tipo_doc));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.nro_doc));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nit", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.nro_nit));
                sqlCommand.Parameters.Add(new AseParameter("@txt_sexo", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_sexo));
                sqlCommand.Parameters.Add(new AseParameter("@edad", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.edad));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_baja));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Mtercero::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mtercero businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mtercero_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_tercero", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_tercero));
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.id_persona));
                sqlCommand.Parameters.Add(new AseParameter("@txt_nombre", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_nombre));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_direccion));
                sqlCommand.Parameters.Add(new AseParameter("@txt_telefono", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_telefono));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_persona", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_tipo_persona));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_tipo_doc));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.nro_doc));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nit", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.nro_nit));
                sqlCommand.Parameters.Add(new AseParameter("@txt_sexo", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.txt_sexo));
                sqlCommand.Parameters.Add(new AseParameter("@edad", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.edad));
                sqlCommand.Parameters.Add(new AseParameter("@fec_alta", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_alta, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@fec_baja", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.fec_baja, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@cod_baja", AseDbType.Decimal, 17, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, businessObject.cod_baja));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Mtercero::Update::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mtercero businessObject, IDataReader dataReader)
        {


            businessObject.cod_tercero = dataReader.GetInt32(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_tercero.ToString()));

            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mtercero.MterceroFields.id_persona.ToString()));

            businessObject.txt_nombre = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_nombre.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_direccion.ToString())))
            {
                businessObject.txt_direccion = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_direccion.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_telefono.ToString())))
            {
                businessObject.txt_telefono = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_telefono.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_tipo_persona.ToString())))
            {
                businessObject.cod_tipo_persona = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_tipo_persona.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_tipo_doc.ToString())))
            {
                businessObject.cod_tipo_doc = dataReader.GetDecimal(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_tipo_doc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.nro_doc.ToString())))
            {
                businessObject.nro_doc = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.nro_doc.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.nro_nit.ToString())))
            {
                businessObject.nro_nit = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.nro_nit.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_sexo.ToString())))
            {
                businessObject.txt_sexo = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.txt_sexo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.edad.ToString())))
            {
                businessObject.edad = dataReader.GetDecimal(dataReader.GetOrdinal(Mtercero.MterceroFields.edad.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.fec_alta.ToString())))
            {
                businessObject.fec_alta = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.fec_alta.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.fec_baja.ToString())))
            {
                businessObject.fec_baja = dataReader.GetString(dataReader.GetOrdinal(Mtercero.MterceroFields.fec_baja.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_baja.ToString())))
            {
                businessObject.cod_baja = dataReader.GetDecimal(dataReader.GetOrdinal(Mtercero.MterceroFields.cod_baja.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mtercero</returns>
        internal List<Mtercero> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mtercero> list = new List<Mtercero>();

            while (dataReader.Read())
            {
                Mtercero businessObject = new Mtercero();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
