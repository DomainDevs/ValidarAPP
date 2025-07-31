using System;
using System.Configuration;
using System.Data;
using Sistran.Co.Application.Data;
using Sistran.Co.Previsora.Application.FullServices;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;

namespace Sistran.Co.Previsora.Application.FullServicesProvider
{
    public class FullServicesProvider : IFullServices
    {
        private const String PIVOTE = "PIVOTE";
        private const String SOAT = "SOAT";
        private const String SISE = "SISE";
        private const String MANAGER = "MWS";              


        public String ExecuteETLSistran(EntityExampleRequest entityExampleRequest)
        {
            //se deben instanciar los DynamicDataAccess por cada base de datos a conectar            
            DynamicDataAccess pdbPivot;
            pdbPivot = new DynamicDataAccess(ConfigurationManager.AppSettings[PIVOTE].ToString());
            DynamicDataAccess pdbPivot2 = new DynamicDataAccess(ConfigurationManager.AppSettings[PIVOTE].ToString());
            String Message = string.Empty;
            int Step = 0;
            try
            {
                pdbPivot.BeginTran();
                pdbPivot2.BeginTran();
                FullServicesHelper dataHelper = new FullServicesHelper();
                //Llamado al Procedimiento Paso 1
                Step++;
                DataSet Sd1 = dataHelper.ExecuteETLSistranFunction1(entityExampleRequest, pdbPivot);
                //Llamado al Procedimiento Paso 1
                Step++;
                DataSet Sd2 = dataHelper.ExecuteETLSistranFunction2(entityExampleRequest, pdbPivot2);                                
                //dtReturn = Sd2.Tables[0];
            }
            catch
            {
                //si hubo errores de se debe devolver todo
                Message = "Error Ejecutando ETL ExecuteETLSistran en el paso " + Step.ToString();
                pdbPivot.Rollback();
                pdbPivot2.Rollback();                
            }
            finally
            {
                //si todo estubo bien ejecutar.
                pdbPivot.Commit();
                pdbPivot2.Commit();
                Message = "Se Ejecuto ETL ExecuteETLSistran completo ";
            }
            return Message;              
        }

        #region "Procesos Helper"
        public String ValidateAccessUser(EntityUserAcessRequest EntityUserAcessRequest)
        {
            String MessageValidate = string.Empty;
            DynamicDataAccess pdbMWS = new DynamicDataAccess(ConfigurationManager.AppSettings[MANAGER].ToString());
            DataSet DtReturn;
            int iReturn = 0;
            try
            {
                pdbMWS.BeginTran();
                ManagerWsHelper managerWsHelper = new ManagerWsHelper();
                DtReturn = managerWsHelper.ValidateAccessUser(EntityUserAcessRequest, pdbMWS);
                if (DtReturn.Tables[0] != null)
                    iReturn = Convert.ToInt32(DtReturn.Tables[0].Rows[0][0].ToString());

                switch (iReturn)
                {
                    case 0:
                        MessageValidate = "Error Usuario o Password Incorrecto Verifique";
                        break;
                    case 1:
                        MessageValidate = "Ok";
                        break;
                    case 2:
                        MessageValidate = "Error No tiene privilegios para ejecutar este metodo";
                        break;
                    default:
                        MessageValidate = "Error Validando Acceso";
                        break;
                }
            }
            catch
            {
                pdbMWS.Rollback();
            }
            finally
            {
                pdbMWS.Commit();
            }
            return MessageValidate;
        }

        public bool IsInProcess(ProcessMethodRequest processMethodRequest)
        {
            String Message = string.Empty;
            DynamicDataAccess pdbMWS = new DynamicDataAccess(ConfigurationManager.AppSettings[MANAGER].ToString());
            bool bReturn = false;
            DataSet DtReturn;
            try
            {
                pdbMWS.BeginTran();
                ManagerWsHelper managerWsHelper = new ManagerWsHelper();
                DtReturn = managerWsHelper.GetProcessMethod(processMethodRequest, pdbMWS);
                if (DtReturn.Tables[0] != null)
                    if (DtReturn.Tables[0].Rows[0]["BeginDate"].ToString().Length > 0)
                        bReturn = true;
            }
            catch
            {
                pdbMWS.Rollback();
            }
            finally
            {
                pdbMWS.Commit();
            }
            return bReturn;
        }

        public int UpdateProcessMethod(ProcessMethodRequest processMethodRequest)
        {
            int ireturn = 0;
            DynamicDataAccess pdbMWS = new DynamicDataAccess(ConfigurationManager.AppSettings[MANAGER].ToString());
            try
            {
                pdbMWS.BeginTran();
                ManagerWsHelper managerWsHelper = new ManagerWsHelper();
                ireturn = managerWsHelper.UpdateProcessMethod(processMethodRequest, pdbMWS);
            }
            catch
            {
                pdbMWS.Rollback();
            }
            finally
            {
                pdbMWS.Commit();
            }
            return ireturn;
        }

        private DateTime BeginProcess()
        {
            String Message = string.Empty;
            DynamicDataAccess pdbMWS = new DynamicDataAccess(ConfigurationManager.AppSettings[MANAGER].ToString());
            DateTime BeginDate = DateTime.Now;
            try
            {
                pdbMWS.BeginTran();
                ManagerWsHelper managerWsHelper = new ManagerWsHelper();
                ProcessMethodRequest processMethodRequest = new ProcessMethodRequest();
                processMethodRequest.IdWs = 1;
                processMethodRequest.IdMethod = 1;
                processMethodRequest.BeginDate = BeginDate;
                processMethodRequest.State = 1;
                int esp = managerWsHelper.UpdateProcessMethod(processMethodRequest, pdbMWS);
            }
            catch
            {
                pdbMWS.Rollback();
            }
            finally
            {
                pdbMWS.Commit();
            }
            return BeginDate;
        }

        private DateTime EndProcess()
        {
            String Message = string.Empty;
            DynamicDataAccess pdbMWS = new DynamicDataAccess(ConfigurationManager.AppSettings[MANAGER].ToString());
            DateTime EndDate = DateTime.Now;
            try
            {
                pdbMWS.BeginTran();
                ManagerWsHelper managerWsHelper = new ManagerWsHelper();
                ProcessMethodRequest processMethodRequest = new ProcessMethodRequest();
                processMethodRequest.IdWs = 1;
                processMethodRequest.IdMethod = 1;
                processMethodRequest.State = 0;
                int esp = managerWsHelper.UpdateProcessMethod(processMethodRequest, pdbMWS);

            }
            catch
            {
                pdbMWS.Rollback();
            }
            finally
            {
                pdbMWS.Commit();
            }
            return EndDate;
        }
        #endregion
        
    }

}
