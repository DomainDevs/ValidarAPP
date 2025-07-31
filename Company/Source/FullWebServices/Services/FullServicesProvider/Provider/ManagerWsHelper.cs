using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary;
using System.Data.Odbc;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Sistran.Co.Application.Data;
using System.Data.SqlClient;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using System.Configuration;
/// <summary>
/// Summary description for DataHelper
/// </summary>

public class ManagerWsHelper
{
        
    public ManagerWsHelper()
    {
        
    }

    public int UpdateProcessMethod(ProcessMethodRequest ProcessMethodRequest, DynamicDataAccess pdbMWS)
    {
        try
        {            
            NameValue[] paramsp = new NameValue[5];
            paramsp[0] = new NameValue("IdWs", ProcessMethodRequest.IdWs);
            paramsp[1] = new NameValue("IdMethod", ProcessMethodRequest.IdMethod);
            paramsp[2] = new NameValue("BeginDate", ProcessMethodRequest.BeginDate);
            paramsp[3] = new NameValue("EndDate", ProcessMethodRequest.EndDate);
            paramsp[4] = new NameValue("State", ProcessMethodRequest.State);
            return pdbMWS.ExecuteSPNonQuery("UPDATE_PROCESS_METHOD", paramsp);           
        }
        catch
        {            
            throw;
        }
    }


    public DataSet ValidateAccessUser(EntityUserAcessRequest EntityUserAcessRequest, DynamicDataAccess pdbMWS)
    {
        try
        {            
            NameValue[] paramsp = new NameValue[4];
            paramsp[0] = new NameValue("UserName", EntityUserAcessRequest.UserName);
            paramsp[1] = new NameValue("Passwoord", EntityUserAcessRequest.Passwoord);
            paramsp[2] = new NameValue("IdWs", EntityUserAcessRequest.IdWs);
            paramsp[3] = new NameValue("IdMethod", EntityUserAcessRequest.IdMethod);
            return pdbMWS.ExecuteSPSerDataSetNoTransaction("VALIDATE_ACCESS_USER", paramsp);           
        }
        catch
        {            
            throw;
        }
    }


    public DataSet GetProcessMethod(ProcessMethodRequest ProcessMethodRequest, DynamicDataAccess pdbMWS)
    {
        try
        {
            NameValue[] paramsp = new NameValue[2];
            paramsp[0] = new NameValue("IdWs", ProcessMethodRequest.IdWs);
            paramsp[1] = new NameValue("IdMethod", ProcessMethodRequest.IdMethod);
            return pdbMWS.ExecuteSPSerDataSetNoTransaction("GET_PROCESS_METHOD", paramsp);
        }
        catch
        {
            throw;
        }
    }   
}

