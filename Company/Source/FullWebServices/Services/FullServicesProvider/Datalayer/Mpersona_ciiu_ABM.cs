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
    /// Data access layer class for Mpersona_ciiu
    /// </summary>
    class Mpersona_ciiu_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_ciiu_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_ciiu_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Mpersona_ciiu_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(Mpersona_ciiu businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_ciiu_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // Use connection object of base class
            //sqlCommand.Connection = MainConnection;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_principal", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_secundaria", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_secundaria)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ppal_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ppal_nuevo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_scndria_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_scndria_nuevo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_ciiu::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Mpersona_ciiu businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_ciiu_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_principal", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_secundaria", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_secundaria)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ppal_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ppal_nuevo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_scndria_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_scndria_nuevo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_ciiu::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Mpersona_ciiu businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.mpersona_ciiu_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@id_persona", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.id_persona)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_principal", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_principal)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_secundaria", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_secundaria)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_ppal_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_ppal_nuevo)));
                sqlCommand.Parameters.Add(new AseParameter("@cod_ciiu_scndria_nuevo", AseDbType.Double, 4, ParameterDirection.Input, false, 6, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ciiu_scndria_nuevo)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("Mpersona_ciiu::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Mpersona_ciiu businessObject, IDataReader dataReader)
        {


            businessObject.id_persona = dataReader.GetInt32(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.id_persona.ToString()));

            businessObject.cod_ciiu_principal = dataReader.GetDouble(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_principal.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_secundaria.ToString())))
            {
                businessObject.cod_ciiu_secundaria = dataReader.GetString(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_secundaria.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_ppal_nuevo.ToString())))
            {
                businessObject.cod_ciiu_ppal_nuevo = dataReader.GetString(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_ppal_nuevo.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_scndria_nuevo.ToString())))
            {
                businessObject.cod_ciiu_scndria_nuevo = dataReader.GetString(dataReader.GetOrdinal(Mpersona_ciiu.Mpersona_ciiuFields.cod_ciiu_scndria_nuevo.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Mpersona_ciiu</returns>
        internal List<Mpersona_ciiu> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Mpersona_ciiu> list = new List<Mpersona_ciiu>();

            while (dataReader.Read())
            {
                Mpersona_ciiu businessObject = new Mpersona_ciiu();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
