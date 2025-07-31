using System;
using System.Collections.Generic;
using System.Data;

namespace Domain.DataAccess
{
    public class DataAccessClass : IDataAccess
    {
        public BaseDeDatos origenDeDatos { get; }
        private string connectionString;

        public DataAccessClass(string dataSource, string baseDeDatos, string usuario, string password, BaseDeDatos origen = BaseDeDatos.Sybase)
        {
            origenDeDatos = origen;

            if (origen.Equals(BaseDeDatos.SqlServer)){
                connectionString = string.Format("data source={0};initial catalog={1};user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework", dataSource, baseDeDatos, usuario, password);
            }
            else
            {
                connectionString = string.Format("database={1}; User ID={2}; Password={3}; Data Source={0}; Port=5000; Connection Timeout=10000;Language=us_english;APP=DESARROLLOEE;Named Parameters=false;TextSize=128000;", dataSource, baseDeDatos, usuario, password);
            }

        }
        
        public IDataAccess MethodFactory()
        {
            IDataAccess resp = new Sybase(connectionString);

            if (this.origenDeDatos.Equals(BaseDeDatos.SqlServer))
                resp = new SQLServer(connectionString);                


            return resp;
        }

        public DataSet consultarSP(string storeProcedure, IDictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public DataSet consultarSP(string storeProcedure, int id)
        {
            throw new NotImplementedException();
        }
    }

    public enum BaseDeDatos
    {
        SqlServer, Sybase
    }
}