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
	/// Data access layer class for Tusuario_limites
	/// </summary>
	class Tusuario_limites_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public Tusuario_limites_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public Tusuario_limites_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tusuario_limites_ABM(string Connection, string userId, int AppId, AseCommand Command)
            : base(Connection, userId, AppId,Command)
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
		public bool Insert(Tusuario_limites businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
			sqlCommand.CommandText = "SUP.tusuario_limites_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			
			try
			{
                sqlCommand.Parameters.Clear();

                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
				sqlCommand.Parameters.Add(new AseParameter("@imp_limite_me", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite_me)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId)); 						
							
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch (Exception ex)             
            {  
                throw new SupException("Tusuario_limites::Insert::Error occured.", ex);
			}
			
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tusuario_limites businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tusuario_limites_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;
                        
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
				sqlCommand.Parameters.Add(new AseParameter("@imp_limite_me", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite_me)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)  
            {
                throw new SupException("Tusuario_limites::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tusuario_limites businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tusuario_limites_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_usuario", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_usuario)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_ramo", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ramo)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_moneda", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_moneda)));
				sqlCommand.Parameters.Add(new AseParameter("@imp_limite_me", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.imp_limite_me)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)             {                   throw new SupException("Tusuario_limites::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tusuario_limites businessObject, IDataReader dataReader)
        {


				businessObject.cod_usuario = dataReader.GetString(dataReader.GetOrdinal(Tusuario_limites.Tusuario_limitesFields.cod_usuario.ToString()));

				businessObject.cod_ramo = dataReader.GetDouble(dataReader.GetOrdinal(Tusuario_limites.Tusuario_limitesFields.cod_ramo.ToString()));

				businessObject.cod_moneda = dataReader.GetDouble(dataReader.GetOrdinal(Tusuario_limites.Tusuario_limitesFields.cod_moneda.ToString()));

				businessObject.imp_limite_me = dataReader.GetDouble(dataReader.GetOrdinal(Tusuario_limites.Tusuario_limitesFields.imp_limite_me.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tusuario_limites</returns>
        internal List<Tusuario_limites> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tusuario_limites> list = new List<Tusuario_limites>();

            while (dataReader.Read())
            {
                Tusuario_limites businessObject = new Tusuario_limites();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
