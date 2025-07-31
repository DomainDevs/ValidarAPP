using Domain.Extensions;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Domain.DataAccess
{
    public class Sybase : IDataAccess
    {
        private string _connectionString;
        public Sybase(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataSet consultarSP(string storeProcedure, IDictionary<string, object> param)
        {
            var ds = new DataSet();
            using (var db = new AseConnection(_connectionString))
            //using (var db = new AseConnection().ConnectionStringSet(Connection))
            {
                using (var comm = new AseCommand(storeProcedure, db))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(comm.CreateParameter("@DOCUMENT_NUM", AseDbType.Numeric, ParameterDirection.Input, param["DOCUMENT_NUM"]));                    
                    comm.Parameters.Add(comm.CreateParameter("@BRANCH_CD", AseDbType.Integer, ParameterDirection.Input, param["BRANCH_CD"]));
                    comm.Parameters.Add(comm.CreateParameter("@PREFIX_CD", AseDbType.Integer, ParameterDirection.Input, param["PREFIX_CD"]));
                    comm.Parameters.Add(comm.CreateParameter("@END_DOCUMENT_NUM", AseDbType.Integer, ParameterDirection.Input, null));


                    try
                    {
                        db.Open();

                        var da = new AseDataAdapter(comm);
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex);
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }

            return ds;
        }

        public DataSet consultarSP(string storeProcedure, int id)
        {
            var ds = new DataSet();
            using (var db = new AseConnection(_connectionString))
            //using (var db = new AseConnection().ConnectionStringSet(Connection))
            {
                using (var comm = new AseCommand(storeProcedure, db))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(comm.CreateParameter("@ID", AseDbType.Integer, ParameterDirection.Input, id));

                    try
                    {
                        db.Open();

                        var da = new AseDataAdapter(comm);
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex);
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }

            return ds;
        }
    }
}