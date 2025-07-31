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
	/// Data access layer class for Ttipo_empresa
	/// </summary>
	class Ttipo_empresa_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public Ttipo_empresa_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public Ttipo_empresa_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public Ttipo_empresa_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
		public bool Insert(Ttipo_empresa businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
			sqlCommand.CommandText = "SUP.ttipo_empresa_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;
			
			try
			{
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_empresa", AseDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_empresa)));
				sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_super", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_super)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId)); 							
								
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch (Exception ex)            
            {
                throw new SupException("Ttipo_empresa::Insert::Error occured.", ex);
			}			
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Ttipo_empresa businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ttipo_empresa_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;
                        

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_empresa", AseDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_empresa)));
				sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_super", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_super)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));                                

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)          
            {
                throw new SupException("Ttipo_empresa::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Ttipo_empresa businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ttipo_empresa_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_ttipo_empresa", AseDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_ttipo_empresa)));
				sqlCommand.Parameters.Add(new AseParameter("@txt_desc", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_desc)));
				sqlCommand.Parameters.Add(new AseParameter("@cod_super", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_super)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)             {                   throw new SupException("Ttipo_empresa::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Ttipo_empresa businessObject, IDataReader dataReader)
        {


				businessObject.cod_ttipo_empresa = dataReader.GetString(dataReader.GetOrdinal(Ttipo_empresa.Ttipo_empresaFields.cod_ttipo_empresa.ToString()));

				businessObject.txt_desc = dataReader.GetString(dataReader.GetOrdinal(Ttipo_empresa.Ttipo_empresaFields.txt_desc.ToString()));

				businessObject.cod_super = dataReader.GetInt32(dataReader.GetOrdinal(Ttipo_empresa.Ttipo_empresaFields.cod_super.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Ttipo_empresa</returns>
        internal List<Ttipo_empresa> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Ttipo_empresa> list = new List<Ttipo_empresa>();

            while (dataReader.Read())
            {
                Ttipo_empresa businessObject = new Ttipo_empresa();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
