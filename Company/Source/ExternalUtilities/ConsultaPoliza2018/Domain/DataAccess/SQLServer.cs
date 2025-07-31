using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
//using System.Web.Mvc;
namespace Domain.DataAccess
{
    public class SQLServer : IDataAccess
    {
        private string _connectionString;
        public SQLServer (string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataSet consultarSP(string storeProcedure, IDictionary<string, object> param)
        {
            var ds = new DataSet();
            using (var db = new DbContext(_connectionString))
            {
                using (var comm = db.Database.Connection.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = storeProcedure.Substring(storeProcedure.IndexOf('.') + 1);
                    comm.Parameters.Add(comm.CreateParameter("@DOCUMENT_NUM", DbType.Decimal, ParameterDirection.Input, param["DOCUMENT_NUM"]));
                    comm.Parameters.Add(comm.CreateParameter("@BRANCH_CD", DbType.Int32, ParameterDirection.Input, param["BRANCH_CD"]));
                    comm.Parameters.Add(comm.CreateParameter("@PREFIX_CD", DbType.Int32, ParameterDirection.Input, param["PREFIX_CD"]));
                    comm.Parameters.Add(comm.CreateParameter("@END_DOCUMENT_NUM", DbType.Int32, ParameterDirection.Input, null));

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = comm.ExecuteReader();
                        ds.Load(reader, LoadOption.OverwriteChanges, new string[] { "COMM.BRANCH", "COMM.PREFIX", "ISS.POLICY", "ISS.CO_POLICY", "ISS.ENDORSEMENT", "ISS.CO_ENDORSEMENT", /*"ISS.CPT_ENDORSEMENT",*/ "ISS.GROUP_ENDORSEMENT", "ISS.COINSURANCE_ACCEPTED", "ISS.COINSURANCE_ASSIGNED", "ISS.POLICY_AGENT", "ISS.COMMISS_AGENT", "ISS.POLICY_CLAUSE", "ISS.ENDORSEMENT_PAYER", "ISS.FIRST_PAY_COMP", "ISS.PAYER_PAYMENT", "ISS.CO_PAYER_PAYMENT_COMP", "", "ISS.RISK", "ISS.CO_RISK", /*"ISS.CPT_RISK",*/ "ISS.ENDORSEMENT_RISK", "ISS.RISK_VEHICLE", "ISS.CO_RISK_VEHICLE", "ISS.RISK_VEHICLE_DRIVER", "ISS.RISK_BENEFICIARY", "ISS.RISK_CLAUSE", "ISS.RISK_SURETY", "ISS.CO_RISK_SURETY", "ISS.RISK_LOCATION", "ISS.RISK_SURETY_GUARANTEE", "ISS.ENDO_RISK_COVERAGE", "ISS.RISK_COVERAGE", "ISS.RISK_COVERAGE_2", "ISS.RISK_COVER_DEDUCT", "ISS.RISK_COVER_CLAUSE", "ISS.PAYER_COMP", "ISS.PAYER_COMP_2", "ISS.RISK_COVER_DETAIL", "ISS.RISK_DETAIL_ACCESSORY", "ISS.RISK_DETAIL", "ISS.RISK_COVER_DETAIL_DEDUCT", "ISS.RISK_DETAIL_DESCRIPTION" });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex);
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                }
            }

            return ds;
        }

        public DataSet consultarSP(string storeProcedure, int id)
        {
            var ds = new DataSet();
            using (var db = new DbContext(_connectionString))
            {
                using (var comm = db.Database.Connection.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(comm.CreateParameter("@ID", DbType.Int32, ParameterDirection.Input, id));

                    try
                    {
                        db.Database.Connection.Open();

                        var reader = comm.ExecuteReader();
                        ds.Load(reader, LoadOption.OverwriteChanges, new string[] { "tabla"});
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex);
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                }
            }

            return ds;
        }
    }
}