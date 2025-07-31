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
    /// Data access layer class for Mpersona_dir
    /// </summary>
    class Mpersona_dir_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_dir_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_dir_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_dir_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona_dir businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_dir_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calle1", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calle1)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro1", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc1", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc1)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calle2", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calle2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro2", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc2", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro3", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro3)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_apto", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_apto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cod_postal", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cod_postal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_zona_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_zona_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_domicilio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_domicilio))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@sn_principal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_principal))); //SUPDB
                
                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_dir::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona_dir businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_dir_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir)));
                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir_old", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir_old)));
                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                sqlCommand.Parameters.Add(new AseParameter("@cod_calle1", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calle1)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro1", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc1", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc1)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calle2", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calle2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro2", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc2", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro3", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro3)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_apto", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_apto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cod_postal", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cod_postal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_zona_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_zona_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_domicilio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_domicilio))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@sn_principal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_principal))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_dir::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona_dir businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_dir_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_tipo_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_tipo_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calle1", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calle1)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro1", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro1)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc1", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc1)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_calle2", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_calle2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro2", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro2)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_desc2", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc2)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_nro3", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_nro3)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_apto", AseDbType.Double, 4, ParameterDirection.Input, false, 5, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_apto)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_observaciones", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_observaciones)));
                sqlCommand.Parameters.Add(new AseParameter("@txt_direccion", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_direccion)));
                sqlCommand.Parameters.Add(new AseParameter("@nro_cod_postal", AseDbType.Double, 5, ParameterDirection.Input, false, 8, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_cod_postal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_pais", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_pais)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_dpto", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_dpto)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_municipio", AseDbType.Double, 3, ParameterDirection.Input, false, 3, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_municipio)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_zona_dir", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_zona_dir)));
                sqlCommand.Parameters.Add(new AseParameter("@sn_domicilio", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_domicilio))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@sn_principal", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.sn_principal))); //SUPDB
                
                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_dir::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona_dir businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.id_persona.ToString()));

            businessObject.cod_tipo_dir = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_tipo_dir.ToString()));

            businessObject.cod_calle1 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_calle1.ToString()));

            businessObject.nro_nro1 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.nro_nro1.ToString()));

            businessObject.txt_desc1 = dataReader.GetString(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.txt_desc1.ToString()));

            businessObject.cod_calle2 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_calle2.ToString()));

            businessObject.nro_nro2 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.nro_nro2.ToString()));

            businessObject.txt_desc2 = dataReader.GetString(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.txt_desc2.ToString()));

            businessObject.nro_nro3 = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.nro_nro3.ToString()));

            businessObject.nro_apto = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.nro_apto.ToString()));

            businessObject.txt_observaciones = dataReader.GetString(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.txt_observaciones.ToString()));

            businessObject.txt_direccion = dataReader.GetString(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.txt_direccion.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.nro_cod_postal.ToString())))
            {
                businessObject.nro_cod_postal = dataReader.GetString(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.nro_cod_postal.ToString()));
            }

            businessObject.cod_pais = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_pais.ToString()));

            businessObject.cod_dpto = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_dpto.ToString()));

            businessObject.cod_municipio = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_municipio.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_zona_dir.ToString())))
            {
                businessObject.cod_zona_dir = dataReader.GetString(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.cod_zona_dir.ToString()));
            }

            businessObject.sn_domicilio = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.sn_domicilio.ToString())); //SUPDB

            businessObject.sn_principal = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_dir.Mpersona_dirFields.sn_principal.ToString())); //SUPDB

        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona_dir</returns>
        internal List<Mpersona_dir> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona_dir> list = new List<Mpersona_dir>();

            while (dataReader.Read())
            {
                Mpersona_dir businessObject = new Mpersona_dir();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
