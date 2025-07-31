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
	/// Data access layer class for Tredes_secuencia
	/// </summary>
	class Tredes_secuencia_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public Tredes_secuencia_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public Tredes_secuencia_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public Tredes_secuencia_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
		public bool Insert(Tredes_secuencia businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
			sqlCommand.CommandText = "SUP.tredes_secuencia_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;
			
			try
			{
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
				sqlCommand.Parameters.Add(new AseParameter("@nro_secuencia", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_secuencia)));
				sqlCommand.Parameters.Add(new AseParameter("@txt_descripcion", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_descripcion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId)); 								
								
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch (Exception ex)             
            {
                throw new SupException("Tredes_secuencia::Insert::Error occured.", ex);
			}			
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(Tredes_secuencia businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tredes_secuencia_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;            

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
				sqlCommand.Parameters.Add(new AseParameter("@nro_secuencia", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_secuencia)));
				sqlCommand.Parameters.Add(new AseParameter("@txt_descripcion", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_descripcion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)           
            {
                throw new SupException("Tredes_secuencia::Update::Error occured.", ex);
            }            
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(Tredes_secuencia businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.tredes_secuencia_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@cod_red", AseDbType.Double, 2, ParameterDirection.Input, false, 2, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.cod_red)));
				sqlCommand.Parameters.Add(new AseParameter("@nro_secuencia", AseDbType.Double, 2, ParameterDirection.Input, false, 1, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.nro_secuencia)));
				sqlCommand.Parameters.Add(new AseParameter("@txt_descripcion", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.txt_descripcion)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)             {                   throw new SupException("Tredes_secuencia::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(Tredes_secuencia businessObject, IDataReader dataReader)
        {


				businessObject.cod_red = dataReader.GetDouble(dataReader.GetOrdinal(Tredes_secuencia.Tredes_secuenciaFields.cod_red.ToString()));

				businessObject.nro_secuencia = dataReader.GetDouble(dataReader.GetOrdinal(Tredes_secuencia.Tredes_secuenciaFields.nro_secuencia.ToString()));

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(Tredes_secuencia.Tredes_secuenciaFields.txt_descripcion.ToString())))
				{
					businessObject.txt_descripcion = dataReader.GetString(dataReader.GetOrdinal(Tredes_secuencia.Tredes_secuenciaFields.txt_descripcion.ToString()));
				}


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of Tredes_secuencia</returns>
        internal List<Tredes_secuencia> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<Tredes_secuencia> list = new List<Tredes_secuencia>();

            while (dataReader.Read())
            {
                Tredes_secuencia businessObject = new Tredes_secuencia();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
