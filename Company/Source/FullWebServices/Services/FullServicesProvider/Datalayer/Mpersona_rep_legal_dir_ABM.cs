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
    /// Data access layer class for Mpersona_rep_legal_dir
    /// </summary>
    class Mpersona_rep_legal_dir_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_rep_legal_dir_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_rep_legal_dir_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_rep_legal_dir_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona_rep_legal_dir businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpers_rep_legal_dir_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_rep_legal", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_rep_legal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_calle_campo1", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_calle_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo1", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_campo1", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_calle_campo2", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_calle_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo2", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_campo2", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo3", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo3)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apto_ofic", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apto_ofic)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cod_postal", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cod_postal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_rep_legal_dir::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona_rep_legal_dir businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpers_rep_legal_dir_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_rep_legal", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_rep_legal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_calle_campo1", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_calle_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo1", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_campo1", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_calle_campo2", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_calle_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo2", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_campo2", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo3", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo3)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apto_ofic", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apto_ofic)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cod_postal", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cod_postal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_rep_legal_dir::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona_rep_legal_dir businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_rep_legal_dir_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_doc_rep_legal", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_doc_rep_legal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_doc", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_doc)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_calle_campo1", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_calle_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo1", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_campo1", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_campo1)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_calle_campo2", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_calle_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo2", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc_campo2", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc_campo2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_campo3", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_campo3)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_apto_ofic", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_apto_ofic)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cod_postal", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cod_postal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_rep_legal_dir::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona_rep_legal_dir businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.id_persona.ToString()));

            businessObject.nro_doc_rep_legal = dataReader.GetString(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.nro_doc_rep_legal.ToString()));

            businessObject.cod_tipo_doc = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_tipo_doc.ToString()));

            businessObject.cod_tipo_dir = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_tipo_dir.ToString()));

            businessObject.cod_tipo_calle_campo1 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_tipo_calle_campo1.ToString()));

            businessObject.nro_campo1 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.nro_campo1.ToString()));

            businessObject.txt_desc_campo1 = dataReader.GetString(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.txt_desc_campo1.ToString()));

            businessObject.cod_tipo_calle_campo2 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_tipo_calle_campo2.ToString()));

            businessObject.nro_campo2 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.nro_campo2.ToString()));

            businessObject.txt_desc_campo2 = dataReader.GetString(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.txt_desc_campo2.ToString()));

            businessObject.nro_campo3 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.nro_campo3.ToString()));

            businessObject.txt_apto_ofic = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.txt_apto_ofic.ToString()));

            businessObject.txt_observaciones = dataReader.GetString(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.txt_observaciones.ToString()));

            businessObject.txt_direccion = dataReader.GetString(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.txt_direccion.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.nro_cod_postal.ToString())))
            {
                businessObject.nro_cod_postal = dataReader.GetString(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.nro_cod_postal.ToString()));
            }

            businessObject.cod_pais = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_pais.ToString()));

            businessObject.cod_dpto = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_dpto.ToString()));

            businessObject.cod_municipio = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields.cod_municipio.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona_rep_legal_dir</returns>
        internal List<Mpersona_rep_legal_dir> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona_rep_legal_dir> list = new List<Mpersona_rep_legal_dir>();

            while (dataReader.Read())
            {
                Mpersona_rep_legal_dir businessObject = new Mpersona_rep_legal_dir();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
