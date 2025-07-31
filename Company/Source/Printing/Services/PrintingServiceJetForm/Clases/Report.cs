using CrystalDecisions.Shared;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServices.Clases;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    public class Report
    {
        # region Properties
        public delegate void ReportCompletedHandler(object sender, EventArgs e);// delegado para el manejo del evento de finalizacion de printGeneralPolicies
        public event ReportCompletedHandler GenerationCompleted;// evento declarado de tipo delegado ReportCompletedHandler

        private string _filePath;
        private string _waterMark;
        private bool _bolOneFile;
        private DataSet _dsOutput;
        private Policy _policyData;
        private DataSet _dsPolicyRisks;
        private string[] _strDataConn;
        private int _asyncProcessId;
        private int _printProcessId;
        private string _docTypeDescription;
        private int _rangeMinValue;
        private int _rangeMaxValue;
        private decimal _fileSize;
        private string _fileName;
        private bool _status;
        private bool _hasError;
        private string _errorDescription;
        private int _version;
        private int _riskCount;
        private ArrayList rutas;


        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private string waterMark
        {
            get { return _waterMark; }
            set { _waterMark = value; }
        }

        private bool bolOneFile
        {
            get { return _bolOneFile; }
            set { _bolOneFile = value; }
        }

        private DataSet dsOutput
        {
            get { return _dsOutput; }
            set { _dsOutput = value; }
        }

        private Policy PolicyData
        {
            get { return _policyData; }
            set { _policyData = value; }
        }

        private DataSet DsPolicyRisks
        {
            get { return _dsPolicyRisks; }
            set { _dsPolicyRisks = value; }
        }

        private string[] strDataConn
        {
            get { return _strDataConn; }
            set { _strDataConn = value; }
        }

        public int AsyncProcessId
        {
            get { return _asyncProcessId; }
            set { _asyncProcessId = value; }
        }

        public int PrintProcessId
        {
            get { return _printProcessId; }
            set { _printProcessId = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string DocTypeDescription
        {
            get { return _docTypeDescription; }
            set { _docTypeDescription = value; }
        }

        public int RangeMinValue
        {
            get { return _rangeMinValue; }
            set { _rangeMinValue = value; }
        }

        public int RangeMaxValue
        {
            get { return _rangeMaxValue; }
            set { _rangeMaxValue = value; }
        }

        public decimal FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; }
        }

        public string ErrorDescription
        {
            get { return _errorDescription; }
            set { _errorDescription = value; }
        }

        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public int RiskCount
        {
            get { return _riskCount; }
            set { _riskCount = value; }
        }



        # endregion

        # region Constructor
        /// <summary>
        /// Contructor del proceso de impresión del reporte
        /// </summary>
        /// <param name="DsOutput">Datos para la generación del reporte</param>
        /// <param name="StrDataConn">Datos de la conexión a la base de datos</param>
        public Report(DataSet DsOutput, string[] StrDataConn)
        {
            
            dsOutput = DsOutput;
            strDataConn = StrDataConn;
            PolicyData = new Policy(dsOutput.Tables["PolicyPrinting"].Rows[0]);
            DsPolicyRisks = ReportServiceHelper.getRiskCount(PolicyData);
            AsyncProcessId = Convert.ToInt32(dsOutput.Tables["PendingPrintData"].Rows[0]["AsyncProcessId"].ToString());
            PrintProcessId = Convert.ToInt32(dsOutput.Tables["PendingPrintData"].Rows[0]["PrintProcessId"].ToString());
            FilePath = string.Empty;
            FileName = string.Empty;
            DocTypeDescription = dsOutput.Tables["PendingPrintData"].Rows[0]["DocTypeDescription"].ToString();
            RangeMinValue = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMinValue"].ToString());
            RangeMaxValue = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMaxValue"].ToString());
            int range = (RangeMaxValue - RangeMinValue) + 1;
            FileSize = ReportServiceHelper.calculateFileSize(range);
            Status = false;
            HasError = false;
            ErrorDescription = string.Empty;
            Version = Convert.ToInt32(ConfigurationSettings.AppSettings["RptUsedVersion"]);
            RiskCount = DsPolicyRisks.Tables[0].Rows.Count;
            rutas = new ArrayList();
        }
        # endregion

        #region Get
        /// <summary>
        /// Genera el reporte de poliza individual.
        /// </summary>
        /// <param name="policyData">Datos de la póliza.</param>
        /// <param name="paramsp">Parámetro del SP que genera el conjunto de datos del reporte.</param>
        /// <returns></returns>
        private string getIndividualPolicyReport(Policy policyData, NameValue[] paramsp)
        {
            //DataSet dsInfo;
            //DataTable dt1, dt2;
            //string strFinalReportPath = string.Empty;

            ////Se determina que procedimiento almacenado se invocará dependendiendo del ramo.
            //switch (policyData.PrefixNum)
            //{
            //    case ((int)PrefixCode.AUTOS):
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];


            //        Vehicle vc = new Vehicle(dt1, dt2);

            //        //Se valida que haya datos para mostrar.
            //        if (vc.RegisterCount != 0)
            //        {
            //            //Crea los reportes.
            //            showVehicleReport(policyData, vc, 0, 0);

            //            ReportServiceHelper.joinPdfFiles(rutas);

            //            //Se retorna la ruta del reporte final.
            //            strFinalReportPath = ((Paths)rutas[0]).FilePath;
            //        }
            //        break;

            //    case ((int)PrefixCode.CROP):
            //        NameValue[] cropSpParams = new NameValue[4];
            //        cropSpParams[0] = paramsp[0];
            //        cropSpParams[1] = paramsp[1];
            //        cropSpParams[2] = paramsp[2];
            //        cropSpParams[3] = paramsp[3];

            //        //SP que llenará las tablas para los reportes.
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_CROP_POLICY_COVER", cropSpParams);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];

            //        Crop rc = new Crop(dt1, dt2);

            //        //Se valida que haya datos para mostrar.
            //        if (rc.RegisterCount == 0)
            //        {
            //            //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
            //            strFinalReportPath = "";
            //        }
            //        else
            //        {
            //            //Este metodo crea los reportes y los concatena.
            //            showCropReport(rc, policyData);

            //            ReportServiceHelper.joinPdfFiles(rutas);

            //            //Se retorna la ruta del reporte final.
            //            strFinalReportPath = ((Paths)rutas[0]).FilePath;
            //        }
            //        break;

            //    case ((int)PrefixCode.COMPLIANCE): /* Cumplimiento. Variable que almacena el valor que indica si se debe o no mostrar 
            //                                                       * el certificado de pago del reporte de cumplimiento*/
            //        //Reasigna parametros del sp
            //        NameValue[] suretySpParams = new NameValue[4];
            //        suretySpParams[0] = paramsp[0];
            //        suretySpParams[1] = paramsp[1];
            //        suretySpParams[2] = paramsp[2];
            //        suretySpParams[3] = paramsp[3];

            //        //Se invoca el procedimiento almacenado que llenará las tablas para los reportes.
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_SURETY_POLICY_COVER", suretySpParams);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];

            //        Surety sr = new Surety(dt1, dt2);

            //        //Se valida que haya datos para mostrar.
            //        if (sr.RegisterCount == 0)
            //        {
            //            strFinalReportPath = "";//Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
            //        }
            //        else
            //        {
            //            //Crea los reportes y los concatena.
            //            showSuretyReport(sr, policyData);

            //            ReportServiceHelper.joinPdfFiles(rutas);

            //            //Se retorna la ruta del reporte final.
            //            strFinalReportPath = ((Paths)rutas[0]).FilePath;
            //        }
            //        break;
            //}
            ////Ruta reporte final.
            //return strFinalReportPath;
            return string.Empty;
        }

        private string getIndividualPolicyReport1(Policy policyData, NameValue[] paramsp)
        {
            DataSet dsInfo;
            DataSet dsReportData;
            DataTable dt1, dt2;
            string strReportPath = string.Empty;

            //Se determina que procedimiento almacenado se invocará dependendiendo del ramo.
            switch (policyData.PrefixNum)
            {
                case ((int)PrefixCode.AUTOS):
                    break;
                case ((int)PrefixCode.COMPLIANCE):
                    break;
                    /////////////////////////////////////////////////////OJO: SE COMENTA ESE BLOQUE DE CODiGO////////////////////////////////////////
                    /*NameValue[] suretySpParams = new NameValue[4];
                    suretySpParams[0] = paramsp[0];
                    suretySpParams[1] = paramsp[1];
                    suretySpParams[2] = paramsp[2];
                    suretySpParams[3] = paramsp[3];

                    //Se invoca el procedimiento almacenado que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_SURETY_POLICY_COVER", suretySpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    NameValue[] complianceSpParams = new NameValue[1];
                    complianceSpParams[0] = paramsp[0];

                    dsReportData = ReportServiceHelper.getData("REPORT.PRV_GET_SURETY_POLICY", complianceSpParams);
                    if ((int)paramsp[3].Value == 0)
                    {
                        Surety surety = new Surety(dsReportData);
                        strReportPath = ReportServiceHelper.WriteFile(surety.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], surety.FileLines);
                    }
                    else if (policyData.QuotationId != 0)
                    {
                        QuotationSurety surety = new QuotationSurety(dsReportData);
                        strReportPath = ReportServiceHelper.WriteFile(surety.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], surety.FileLines);
                    }
                    else
                    {
                        TempSurety surety = new TempSurety(dsReportData);
                        strReportPath = ReportServiceHelper.WriteFile(surety.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], surety.FileLines);
                    }
                    break;*/
            }
            return strReportPath;
        }

        /// <summary>
        /// Genera Reporte Formato de recaudo cuando se requiere individual
        /// </summary>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <returns></returns>
        private string getFormatCollectReport(Policy policyData, NameValue[] paramsp)
        {
            //DataSet dsInfo;
            //DataTable dt1, dt2;
            //string strFinalReportPath = string.Empty;

            ////Se determina que procedimiento almacenado se invocará dependendiendo del ramo.
            //switch (policyData.PrefixNum)
            //{
            //    case ((int)PrefixCode.AUTOS):
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];

            //        Vehicle vc = new Vehicle(dt1, dt2);

            //        //Se valida que haya datos para mostrar.
            //        if (vc.RegisterCount != 0)
            //        {
            //            //Crea los reportes.
            //            if ((policyData.ReportType == (int)ReportType.PAYMENT_CONVENTION) || (policyData.ReportType == (int)ReportType.FORMAT_COLLECT))
            //            {
            //                policyData.RangeMinValue = 0;
            //                policyData.RangeMaxValue = 0;
            //            }
            //            showVehicleReport(policyData, vc, 0, 0);

            //            ReportServiceHelper.joinPdfFiles(rutas);

            //            //Se retorna la ruta del reporte final.
            //            strFinalReportPath = ((Paths)rutas[0]).FilePath;
            //        }
            //        break;

            //    case ((int)PrefixCode.CROP):
            //        NameValue[] cropSpParams = new NameValue[4];
            //        cropSpParams[0] = paramsp[0];
            //        cropSpParams[1] = paramsp[1];
            //        cropSpParams[2] = paramsp[2];
            //        cropSpParams[3] = paramsp[3];

            //        //SP que llenará las tablas para los reportes.
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_CROP_POLICY_COVER", cropSpParams);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];

            //        Crop rc = new Crop(dt1, dt2);

            //        //Se valida que haya datos para mostrar.
            //        if (rc.RegisterCount == 0)
            //        {
            //            //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
            //            strFinalReportPath = "";
            //        }
            //        else
            //        {
            //            //Este metodo crea los reportes y los concatena.
            //            showCropReport(rc, policyData);

            //            ReportServiceHelper.joinPdfFiles(rutas);

            //            //Se retorna la ruta del reporte final.
            //            strFinalReportPath = ((Paths)rutas[0]).FilePath;
            //        }
            //        break;

            //    case ((int)PrefixCode.COMPLIANCE): /* Cumplimiento. Variable que almacena el valor que indica si se debe o no mostrar 
            //                                                       * el certificado de pago del reporte de cumplimiento*/
            //        //Reasigna parametros del sp
            //        NameValue[] suretySpParams = new NameValue[4];
            //        suretySpParams[0] = paramsp[0];
            //        suretySpParams[1] = paramsp[1];
            //        suretySpParams[2] = paramsp[2];
            //        suretySpParams[3] = paramsp[3];

            //        //Se invoca el procedimiento almacenado que llenará las tablas para los reportes.
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_SURETY_POLICY_COVER", suretySpParams);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];

            //        Surety sr = new Surety(dt1, dt2);

            //        //Se valida que haya datos para mostrar.
            //        if (sr.RegisterCount == 0)
            //        {
            //            strFinalReportPath = "";//Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
            //        }
            //        else
            //        {
            //            //Crea los reportes y los concatena.
            //            showSuretyReport(sr, policyData);

            //            ReportServiceHelper.joinPdfFiles(rutas);

            //            //Se retorna la ruta del reporte final.
            //            strFinalReportPath = ((Paths)rutas[0]).FilePath;
            //        }
            //        break;
            //}
            ////Ruta reporte final.
            //return strFinalReportPath;
            return string.Empty;
        }

        /// <summary>
        /// Genera caratula del reporte de poliza colectiva
        /// </summary>
        /// <param name="policyData">Datos de la poliza</param>
        /// <param name="paramsp">Parametros del SP</param>
        /// <returns>Ruta de la caratula</returns>
        public string[] getPolicyCoverReport(Policy policyData, NameValue[] paramsp)
        {
            //string[] strDataPolicyCover = new string[3];
            //DataSet dsInfo;
            //DataTable dt1, dt2;

            //switch (Convert.ToInt32(policyData.PrefixNum))//PrefixNum
            //{
            //    case ((int)PrefixCode.AUTOS): //Automoviles.
            //        ///eopiraneque 14/01/2010
            //        ///Se incluye filtro cuando se ha seleccionado Sólo Formato de recaudo
            //        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
            //        dt1 = dsInfo.Tables[0];
            //        dt2 = dsInfo.Tables[1];

            //        Vehicle vc = new Vehicle(dt1, dt2);
            //        strDataPolicyCover[0] = vc.MinRow.ToString();
            //        strDataPolicyCover[1] = vc.RowCount.ToString();

            //        if ((policyData.ReportType != (int)ReportType.FORMAT_COLLECT))
            //        {
            //            if ((vc.RegisterCount != 0))
            //            {
            //                if (policyData.ReportType != (int)ReportType.PAYMENT_CONVENTION)
            //                {
            //                    strDataPolicyCover[2] = showVehicleCoverReport(Convert.ToInt32(paramsp[0].Value), vc, policyData);
            //                }
            //                else
            //                {
            //                    Paths caratula = new Paths();
            //                    caratula.setFileName(policyData, ".pdf");
            //                    rutas.Add(caratula);
            //                    strDataPolicyCover[2] = " ";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            string[] strReportParameters = new string[10];
            //            strReportParameters[0] = policyData.ProcessId.ToString();
            //            strReportParameters[1] = vc.MinRow.ToString();
            //            strReportParameters[2] = policyData.CodeBar;
            //            strReportParameters[3] = policyData.TempNum.ToString();
            //            strReportParameters[4] = "0";
            //            strReportParameters[5] = policyData.EndorsementId.ToString();

            //            strDataPolicyCover[2] = addFormatCollect(policyData, strReportParameters);

            //        }

            //        break;
            //    case ((int)PrefixCode.CROP):
            //        break;
            //    case ((int)PrefixCode.COMPLIANCE):
            //        break;
            //}
            //return strDataPolicyCover;
            return "".Split('|');
        }

        /// <summary>
        /// Genera el reporte de cada riesgo de la poliza colectiva
        /// </summary>
        /// <param name="policyData">Datos de la poliza</param>
        /// <param name="paramsp">Parametros del reporte</param>
        /// <param name="policyCoverData">Datos de la caratula generada</param>
        /// <param name="dsPolicyRisks">DataSet de los riesgos de la poliza</param>
        public void getPolicyRisksReport(Policy policyData, NameValue[] paramsp, string[] policyCoverData, DataSet dsPolicyRisks)
        {
            //int risk = 0;
            //int intRiskNum = 0;
            //int intRiskType = 0;
            //int firstRisk = Convert.ToInt32(policyData.RangeMinValue);
            //int lastRisk = Convert.ToInt32(policyData.RangeMaxValue);
            //int[] printableRisks = ReportServiceHelper.getCountPrintableRisks(dsPolicyRisks, firstRisk, lastRisk);

            //switch (Convert.ToInt32(policyData.PrefixNum))//PrefixNum
            //{
            //    case ((int)PrefixCode.AUTOS):
            //        if (Convert.IsDBNull(policyCoverData[0])) policyCoverData[0] = "0";

            //        Vehicle vc = new Vehicle();
            //        vc.MinRow = Convert.ToInt32(policyCoverData[0]);
            //        vc.RowCount = Convert.ToInt32(policyCoverData[1]);

            //        if (policyCoverData[2].Length >= 1)
            //        {
            //            for (int riskIndex = 1; riskIndex <= RiskCount; riskIndex++)
            //            {
            //                intRiskNum = printableRisks[risk];

            //                if (dsPolicyRisks.Tables[0].Rows[risk]["RISK_STATUS_CD"].ToString() != string.Empty &&
            //                    ReportServiceHelper.isNumeric(dsPolicyRisks.Tables[0].Rows[risk]["RISK_STATUS_CD"].ToString()))
            //                {
            //                    intRiskType = Convert.ToInt32(dsPolicyRisks.Tables[0].Rows[risk]["RISK_STATUS_CD"].ToString());
            //                }

            //                showVehicleReport(policyData, vc, intRiskNum, intRiskType);
            //                risk++;
            //            }
            //        }
            //        break;
            //    case ((int)PrefixCode.CROP):
            //        break;
            //    case ((int)PrefixCode.COMPLIANCE):
            //        break;
            //}
        }

        /// <summary>
        /// Genera reporte de pólizas migradas.
        /// </summary>
        /// <param name="policyData">Datos de la póliza</param>
        /// <param name="MigratedPolicies">Datos necesarios para generar la lista</param>
        /// <returns>Ruta del archivo generado</returns>
        public string getListMigratedPolicies(Policy policyData, string[] MigratedPolicies)
        {
            string fileExtension;

            if (MigratedPolicies[4].Equals(true.ToString()))
            {
                fileExtension = ".xls";
            }
            else
            {
                fileExtension = ".pdf";
            }

            Paths migrated = new Paths(rutas.Count);
            migrated.setFileName(policyData, fileExtension);
            rutas.Add(migrated);

            string[] strPaths = new string[2];
            strPaths[0] = migrated.getPath("MigratedPoliciesList", false);
            strPaths[1] = migrated.FilePath;

            //Parametros del SP.
            NameValue[] paramsp = new NameValue[5];
            paramsp[0] = new NameValue("PROCESS_ID", Convert.ToInt32(policyData.ProcessId));
            paramsp[1] = new NameValue("PROCESS_FROM_DT", MigratedPolicies[2]);
            paramsp[2] = new NameValue("PROCESS_TO_DT", MigratedPolicies[3]);
            paramsp[3] = new NameValue("MAIN_AGENT", MigratedPolicies[0]);
            paramsp[4] = new NameValue("BRANCH_ID", MigratedPolicies[1]);

            //Parametros del reporte
            string[] strReportParameters = new string[4];
            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = MigratedPolicies[2];
            strReportParameters[2] = MigratedPolicies[3];
            strReportParameters[3] = MigratedPolicies[4];

            //Genera el reporte
            ReportServiceHelper.getData("REPORT.CO_MIGRATED_POLICIES", paramsp);//Inserta polizas migradas
            showMigratedPoliciesList(strReportParameters, strPaths);
            ReportServiceHelper.getData("REPORT.CO_MIGRATED_POLICIES", paramsp);//Borra polizas migradas
            return migrated.FilePath;
        }
        #endregion

        #region Print
        /// <summary>
        /// Impresión general. Pólizas, Temporarios, cotizaciones y listado de pólizas migradas.
        /// </summary>
        public void printGeneralPolicies()
        {
            try
            {
                int intPolicyType = 0;
                int intRiskType = 0;

                //Datos de Pólizas Migradas
                string[] MigratedPolicies = new string[5];
                MigratedPolicies[0] = dsOutput.Tables["MigratedPolicies"].Rows[0]["IntermediaryId"].ToString();//Id del Intermediario
                MigratedPolicies[1] = dsOutput.Tables["MigratedPolicies"].Rows[0]["BranchId"].ToString();//Id de la sucursal
                MigratedPolicies[2] = dsOutput.Tables["MigratedPolicies"].Rows[0]["ProcessFromDate"].ToString();//fecha inicial
                MigratedPolicies[3] = dsOutput.Tables["MigratedPolicies"].Rows[0]["ProcessToDate"].ToString();//fecha final
                MigratedPolicies[4] = dsOutput.Tables["MigratedPolicies"].Rows[0]["ExportToExcel"].ToString();//Establece si el reporte se debe exportar a Excel.

                if (PolicyData.PolicyId != 0 || PolicyData.TempNum != 0) //Evalua PolicyId ó TempNum
                {
                    //Número de riesgos, cantidad de coverturas, tipo de poliza y estados de riesgo.
                    int intRiskCount = DsPolicyRisks.Tables[0].Rows.Count;
                    //eopiraneque 
                    //int intRiskCount = 0;
                    RiskCount = PolicyData.RangeMaxValue - PolicyData.RangeMinValue + 1;

                    if (RiskCount > 0)
                    {
                        intPolicyType = Convert.ToInt32(DsPolicyRisks.Tables[0].Rows[0]["POLICY_TYPE"].ToString());

                        //Si la póliza es de tipo colectiva con un solo riesgo, ésta
                        //se debe imprimir como tipo individual.
                        if ((intPolicyType == (int)(PolicyType.COLLECTIVE)) && (intRiskCount == 1))
                        {
                            intPolicyType = (int)(PolicyType.INDIVIDUAL);
                        }
                        //else
                        //{
                        //    intPolicyType = (intRiskType != 0) ? 2 : intPolicyType;
                        //    //intPolicyType = (intRiskCount != 0) ? 2 : intPolicyType;
                        //}
                    }

                    if (RiskCount > 1)
                    {
                        foreach (DataRow row in DsPolicyRisks.Tables[0].Rows)
                        {
                            int intRiskStatus = 0;
                            string strRiskStatus = row["RISK_STATUS_CD"].ToString();
                            if (strRiskStatus.Length > 0) intRiskStatus = Convert.ToInt32(strRiskStatus);

                            if (intRiskStatus == (int)RiskStatus.EXCLUDED) //Riesgo Excluido
                            {
                                intRiskType =
                                    Convert.ToInt32(DsPolicyRisks.Tables[0].Rows[0]["RISK_STATUS_CD"].ToString());
                                PolicyData.RangeMaxValue = (PolicyData.TempNum != 0) ? 2 : PolicyData.RangeMaxValue;
                            }
                        }
                    }

                    intPolicyType = (intRiskType != 0) ? 2 : intPolicyType; //Imprimir la poliza como colectiva.

                    //Asigna parametros del SP.
                    NameValue[] paramsp = new NameValue[9];
                    paramsp[0] = new NameValue("PROCESS_ID", Convert.ToInt32(PolicyData.ProcessId));
                    paramsp[1] = new NameValue("POLICY_ID", Convert.ToInt32(PolicyData.PolicyId));
                    paramsp[2] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(PolicyData.EndorsementId));
                    paramsp[3] = new NameValue("TEMP_ID", Convert.ToInt32(PolicyData.TempNum));
                    paramsp[4] = new NameValue("RISK_NUM", Convert.ToInt32(PolicyData.RangeMaxValue));
                    paramsp[5] = new NameValue("FIRST_RISK", Convert.ToInt32(PolicyData.RangeMinValue));
                    paramsp[6] = new NameValue("LAST_RISK", Convert.ToInt32(PolicyData.RangeMaxValue));
                    paramsp[7] = new NameValue("POLICY_TYPE", intPolicyType);
                    paramsp[8] = new NameValue("RISK_NUM_ANT", null);

                    switch (Convert.ToInt32(PolicyData.PrefixNum)) //PrefixNum
                    {
                        case ((int)PrefixCode.AUTOS):
                            FilePath = printAutos(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                        case ((int)PrefixCode.CROP):
                            FilePath = printRC(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                        case ((int)PrefixCode.COMPLIANCE):
                            FilePath = printCompliance(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                    }
                }

                if (Convert.ToInt32(MigratedPolicies[0]) > 0) //Evalua Id del Intermediario.
                {
                    FilePath = printMigratedPolicies(PolicyData, MigratedPolicies);
                }


                this.Status = true;
            }
            catch (Exception Ex)
            {
                this.HasError = true;
                this.ErrorDescription = Ex.ToString();
                throw Ex;
            }
            finally
            {
                if (GenerationCompleted != null)
                {
                    ReportServiceHelper.deleteFiles(rutas);
                    GenerationCompleted(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Impresión general. Pólizas mensualizadas.
        /// </summary>
        public void printGeneralPoliciesRq()
        {
            int intPolicyType = 0;
            //int intRiskType = 0;

            //Datos de Pólizas Migradas
            //string[] MigratedPolicies = new string[5];
            //MigratedPolicies[0] = dsOutput.Tables["MigratedPolicies"].Rows[0]["IntermediaryId"].ToString();//Id del Intermediario
            //MigratedPolicies[1] = dsOutput.Tables["MigratedPolicies"].Rows[0]["BranchId"].ToString();//Id de la sucursal
            //MigratedPolicies[2] = dsOutput.Tables["MigratedPolicies"].Rows[0]["ProcessFromDate"].ToString();//fecha inicial
            //MigratedPolicies[3] = dsOutput.Tables["MigratedPolicies"].Rows[0]["ProcessToDate"].ToString();//fecha final
            //MigratedPolicies[4] = dsOutput.Tables["MigratedPolicies"].Rows[0]["ExportToExcel"].ToString();//Establece si el reporte se debe exportar a Excel.

            //if ((PolicyData.PolicyId != 0) || (PolicyData.TempNum != 0) || (PolicyData.RequestId != 0)) //Evalua PolicyId ó TempNum ó RequestId
            //if (PolicyData.RequestId != 0)
            //{
            //    //Número de riesgos, cantidad de coverturas, tipo de poliza y estados de riesgo.
            //    int intRiskCount = DsPolicyRisks.Tables[0].Rows.Count;

            //    if (intRiskCount > 0)
            //    {
            //        intPolicyType = Convert.ToInt32(DsPolicyRisks.Tables[0].Rows[0]["POLICY_TYPE"].ToString());
            //    }

            //    if (intRiskCount > 1)
            //    {
            //        foreach (DataRow row in DsPolicyRisks.Tables[0].Rows)
            //        {
            //            int intRiskStatus = 0;
            //            string strRiskStatus = row["RISK_STATUS_CD"].ToString();
            //            if (strRiskStatus.Length > 0) intRiskStatus = Convert.ToInt32(strRiskStatus);

            //            if (intRiskStatus == (int)RiskStatus.EXCLUDED) //Riesgo Excluido
            //            {
            //                intRiskType =
            //                    Convert.ToInt32(DsPolicyRisks.Tables[0].Rows[0]["RISK_STATUS_CD"].ToString());
            //                PolicyData.RangeMaxValue = (PolicyData.TempNum != 0) ? 2 : PolicyData.RangeMaxValue;
            //            }
            //        }
            //    }

            //    intPolicyType = (intRiskType != 0) ? 2 : intPolicyType; //Imprimir la poliza como colectiva.

            intPolicyType = (int)PolicyType.INDIVIDUAL;
            //Asigna parametros del SP.
            NameValue[] paramsp = new NameValue[9];
            paramsp[0] = new NameValue("PROCESS_ID", Convert.ToInt32(PolicyData.ProcessId));
            paramsp[1] = new NameValue("POLICY_ID", Convert.ToInt32(PolicyData.PolicyId));
            paramsp[2] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(PolicyData.EndorsementId));
            paramsp[3] = new NameValue("TEMP_ID", Convert.ToInt32(PolicyData.TempNum));
            paramsp[4] = new NameValue("RISK_NUM", Convert.ToInt32(PolicyData.RangeMaxValue));
            paramsp[5] = new NameValue("FIRST_RISK", Convert.ToInt32(PolicyData.RangeMinValue));
            paramsp[6] = new NameValue("LAST_RISK", Convert.ToInt32(PolicyData.RangeMaxValue));
            paramsp[7] = new NameValue("POLICY_TYPE", intPolicyType);
            paramsp[8] = new NameValue("RISK_NUM_ANT", Convert.ToInt32(PolicyData.RiskAnt));

            switch (Convert.ToInt32(PolicyData.PrefixNum)) //PrefixNum
            {
                case ((int)PrefixCode.AUTOS):
                    FilePath = printAutos(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                case ((int)PrefixCode.CROP):
                    FilePath = printRC(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                case ((int)PrefixCode.COMPLIANCE):
                    FilePath = printCompliance(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
            }
            //}

            //if (Convert.ToInt32(MigratedPolicies[0]) > 0) //Evalua Id del Intermediario.
            //{
            //    FilePath = printMigratedPolicies(PolicyData, MigratedPolicies);
            //}

            this.Status = true;
        }

        /// <summary>
        /// Impresión de Solicitud Agrupadora
        /// </summary>
        /// <returns>Ruta del reporte generado</returns>
        public void printGroupRequestPolicies()
        {
            try
            {
                //Almacena los parametros del SP que llena tabla de reportes.
                NameValue[] requestParam = new NameValue[4];

                //Almacena todas las pólizas de la solicitud agrupadora.
                DataSet dsRequestPolicies;

                Policy policyData = new Policy(dsOutput.Tables["PolicyPrinting"].Rows[0]);
                policyData.RequestId = Convert.ToInt32(dsOutput.Tables["GroupRequest"].Rows[0]["RequestId"].ToString());

                //Se asignan los valores de los parametros para el reporte de Solicitud Agrupadora.
                requestParam[0] = new NameValue("PROCESS_ID", policyData.ProcessId);
                requestParam[1] = new NameValue("REQUEST_ID", policyData.RequestId);
                requestParam[2] = new NameValue("RANGEMIN", policyData.RangeMinValue);
                requestParam[3] = new NameValue("RANGEMAX", policyData.RangeMaxValue);


                //SP que llena las tablas para Solicitud Agrupadora.
                dsRequestPolicies = ReportServiceHelper.getData("REPORT.CO_SINGLE_REQUEST_POLICY_COVER", requestParam);

                //epiraneque 14.12.2009 variable que determina si la solicitud agrupadora es con vigenvia abierta o cerrada
                bool isOpenRequest = Convert.ToBoolean(dsRequestPolicies.Tables[0].Rows[0]["ISOPEN"].ToString());

                if ((Convert.ToInt32(policyData.ReportType) == (int)ReportType.COMPLETE_REQUEST) ||
                    (Convert.ToInt32(policyData.ReportType) == (int)ReportType.ONLY_REQUEST))
                {
                    //Parametros de la carátula de la Solicitud Agrupadora.
                    Policy polCaratula = new Policy();
                    polCaratula.ReportType = policyData.ReportType;
                    polCaratula.PolicyId = 0;
                    polCaratula.RequestId = policyData.RequestId;
                    polCaratula.ProcessId = policyData.ProcessId;
                    polCaratula.User = policyData.User;
                    polCaratula.CodeBar = policyData.CodeBar;
                    polCaratula.TempNum = 0;
                    polCaratula.PrefixNum = 0;
                    polCaratula.RangeMinValue = 0;
                    polCaratula.RangeMaxValue = 0;
                    polCaratula.WithFormatCollect = false;

                    showCoverRequestReport(polCaratula, isOpenRequest);

                    if (Convert.ToInt32(policyData.ReportType) == (int)ReportType.ONLY_REQUEST)
                    {
                        ReportServiceHelper.joinPdfFiles(rutas);

                        //Si la opción seleccionada es "Solo la Solicitud Agrupadora" se envía la ruta sin pólizas
                        FilePath = ((Paths)rutas[0]).FilePath;
                    }
                }

                //Si la opción elegida es Solicitud Agrupadora y las pólizas relacionadas.            
                if ((Convert.ToInt32(policyData.ReportType) == (int)ReportType.COMPLETE_REQUEST) ||
                    (Convert.ToInt32(policyData.ReportType) == (int)ReportType.ONLY_POLICIES_REQUEST))
                {

                    //Se valida si existen polizas asociadas a la solicitud agrupadora.
                    if (dsRequestPolicies.Tables[0].Rows.Count == 0)
                    {
                        //Si no hay pólizas, se retorna vacío y en
                        //la presentación se muestra un mensaje de alerta.
                        FilePath = String.Empty;
                    }
                    else
                    {
                        //estructura que debe ser cargada para cada póliza
                        DataSet dsPolicies = dsOutput.Clone();
                        int riskAnt = 0;
                        foreach (DataRow dr in dsRequestPolicies.Tables[0].Rows)
                        {
                            //PolicyData.ReportType = 1;
                            PolicyData.PolicyId = Convert.ToInt32(dr["PolicyId"].ToString());
                            PolicyData.EndorsementId = Convert.ToInt32(dr["PolicyEndorsId"].ToString());
                            PolicyData.ProcessId = Convert.ToInt32(policyData.ProcessId.ToString());
                            PolicyData.User = policyData.User;
                            PolicyData.CodeBar = policyData.CodeBar;
                            PolicyData.TempNum = policyData.TempNum;
                            PolicyData.PrefixNum = Convert.ToInt32(dr["PrefixCd"].ToString());
                            PolicyData.RangeMinValue = Convert.ToInt32(dr["FirstRisk"].ToString());
                            PolicyData.RangeMaxValue = Convert.ToInt32(dr["LastRisk"].ToString());
                            PolicyData.WithFormatCollect = false;
                            PolicyData.RiskAnt = riskAnt;
                            riskAnt = Convert.ToInt32(dr["LastRisk"].ToString());
                            printGeneralPoliciesRq();
                            //individualPaths.Add(this.FilePath);
                            dsPolicies.Clear();
                        }
                        ReportServiceHelper.joinPdfFiles(rutas);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Ocurrio un error al generar el reporte de solicitud agrupadora: " + Ex.Message);
            }
            finally
            {
                if (GenerationCompleted != null)
                {
                    ReportServiceHelper.deleteFiles(rutas);
                    GenerationCompleted(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Realiza impresión del ramo autos
        /// </summary>
        /// <param name="intPolicyType"></param>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <param name="dsPolicyRisks"></param>
        /// <returns></returns>
        public string printAutos(int intPolicyType, Policy policyData, NameValue[] paramsp, DataSet dsPolicyRisks)
        {
            DataSet dsInfo;
            DataSet dsReportData;
            DataTable dt1, dt2;
            string strReportPath = string.Empty;

            dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
            dt1 = dsInfo.Tables[0];//
            dt2 = dsInfo.Tables[1];//

            NameValue[] vehicleSpParams = new NameValue[3];
            vehicleSpParams[0] = paramsp[0];//
            vehicleSpParams[1] = paramsp[1];//
            vehicleSpParams[2] = paramsp[2];//

            dsReportData = ReportServiceHelper.getData("REPORT.PRV_GET_VEHICLE_POLICY", vehicleSpParams);

            ///////////////////////////////////////////////OJO: SE COMENTA ESTE BLOQUE DE CODIGO//////////////////////////////////////////////////////////////
            /*
            if ((int)paramsp[3].Value == 0)
            {
                Vehicle vehicle = new Vehicle(dsReportData);
                strReportPath = ReportServiceHelper.WriteFile(vehicle.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], vehicle.FileLines);
            }
            else if (policyData.QuotationId != 0)
            {
                QuotationVehicle vehicle = new QuotationVehicle(dsReportData);
                strReportPath = ReportServiceHelper.WriteFile(vehicle.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], vehicle.FileLines);
            }
            else
            {
                TempVehicle vehicle = new TempVehicle(dsReportData);
                strReportPath = ReportServiceHelper.WriteFile(vehicle.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], vehicle.FileLines);
            }*/

            return strReportPath;
        }

        /// <summary>
        /// Realiza impresión del ramo responsabilidad civil
        /// </summary>
        /// <param name="intPolicyType"></param>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <param name="dsPolicyRisks"></param>
        /// <returns></returns>
        public string printRC(int intPolicyType, Policy policyData, NameValue[] paramsp, DataSet dsPolicyRisks)
        {
            string ReportPath = getIndividualPolicyReport(policyData, paramsp);
            return ReportPath;
        }

        /// <summary>
        /// Realiza impresión del ramo Cumplimiento
        /// </summary>
        /// <param name="intPolicyType"></param>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <param name="dsPolicyRisks"></param>
        /// <returns></returns>
        public string printCompliance(int intPolicyType, Policy policyData, NameValue[] paramsp, DataSet dsPolicyRisks)
        {
            string ReportPath = getIndividualPolicyReport1(policyData, paramsp);
            return ReportPath;
        }

        /// <summary>
        /// Imprime listado de pólizas migradas.
        /// </summary>
        /// <param name="policyData">        [0]:intReportType: Tipo de reporte.
        ///                                  [1]:intPolicyId: Póliza a imprimir.
        ///                                  [2]:intEndorsId: Endoso a imprimir.
        ///                                  [3]:intProcessId: Proceso de impresión.
        ///                                  [4]:strUser: Usuario.
        ///                                  [5]:strCodeBarNum: Código de barras.
        ///                                  [6]:intTempNum: Temporario.
        ///                                  [7]:intPrefixNum: Sucursal.
        ///                                  [8]:FirstRisk: Primer riesgo del conjunto.
        ///                                  [9]:LastRisk: Último riesgo del conjunto.</param>
        /// <param name="MigratedPolicies">  [0]:ProcessFromDate: Fecha Desde.
        ///                                  [1]:ProcessToDate: Fecha Hasta.
        ///                                  [2]:intMainAgent: Intermediario.</param>
        /// <returns>Ruta del archivo pdf creado a partir del reporte de pólizas migradas</returns>
        public string printMigratedPolicies(Policy policyData, string[] MigratedPolicies)
        {
            string ReportPath = getListMigratedPolicies(policyData, MigratedPolicies);
            return ReportPath;
        }
        #endregion

        #region Show
        /// <summary>
        /// Genera reporte Solicitud Agrupadora
        /// </summary>
        /// <param name="policyData"></param>
        /// <param name="isOpenRequest"></param>
        private void showCoverRequestReport(Policy policyData, bool isOpenRequest)
        {
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);

            Paths caratula = new Paths(rutas.Count);
            caratula.setFileName(policyData, ".pdf");
            rutas.Add(caratula);

            //Inicializa variables locales
            string[] strPaths = new string[2];
            string[] strReportParameters = new string[4];

            strPaths[0] = caratula.getPath("RequestCover", false);
            strPaths[1] = caratula.FilePath;

            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = policyData.CodeBar;
            strReportParameters[2] = policyData.TempNum.ToString();
            strReportParameters[3] = policyData.RequestId.ToString();

            //Se carga la ruta del reporte a cargar.
            //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);

            if (!isOpenRequest)
            {
                Paths convenio = new Paths(rutas.Count);
                convenio.setFileName(policyData, ".pdf");
                rutas.Add(convenio);

                strPaths[0] = convenio.getPath("RequestConvection", false);
                strPaths[1] = convenio.FilePath;

                string[] strReportParametersCnv = new string[4];
                strReportParametersCnv[0] = policyData.ProcessId.ToString();
                strReportParametersCnv[1] = "0";
                strReportParametersCnv[2] = policyData.CodeBar;
                strReportParametersCnv[3] = policyData.TempNum.ToString();

                //Se carga la ruta del reporte a cargar.
                //ReportServiceHelper.loadReportFile(strReportParametersCnv, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);
            }
        }

        /// <summary>
        /// Genera los reportes para el ramo de automoviles y los exporta a .pdf.
        /// </summary>
        /// <param name="policyData">Arreglo con datos de la poliza</param>
        /// <param name="vc">Valores de parámetros del reporte de Vehicle Cover</param>
        /// <param name="intRiskNum">número del riesgo</param>
        /// <param name="intRiskType">Tipo de riesgo</param>
        private void showVehicleReport(Policy policyData, Vehicle vc, int intRiskNum, int intRiskType)
        {
            //int intReportType = (int)(Convert.ToInt32(policyData.ReportType));

            //if (RiskCount == 1)
            //{
            //    Paths final = new Paths(rutas.Count);
            //    final.setFileName(policyData, ".pdf");
            //    rutas.Add(final);
            //}

            //int intLines = vc.CoveragesCount + vc.AccesoriesCount;
            //int intTotalLines = Convert.ToInt32(ConfigurationSettings.AppSettings["CountCoverAccesories"]);

            //if (policyData.TempNum == 0)//Marca de agua: Temporarios o Cotizaciones.
            //{
            //    waterMark = string.Empty;
            //}
            //else
            //{
            //    if (policyData.QuotationId != 0)
            //    {
            //        waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
            //    }
            //    else
            //    {
            //        waterMark = ConfigurationSettings.AppSettings["WaterMark"];
            //    }
            //}

            ////Parametros reportes Automoviles
            //string[] strReportParameters = new string[10];
            //strReportParameters[0] = PolicyData.ProcessId.ToString();
            //strReportParameters[1] = vc.MinRow.ToString();
            //strReportParameters[2] = PolicyData.CodeBar;
            //strReportParameters[3] = PolicyData.TempNum.ToString();
            //strReportParameters[4] = (intRiskNum != 0) ? intRiskNum.ToString() : policyData.EndorsementId.ToString();
            //strReportParameters[5] = policyData.EndorsementId.ToString();

            //if ((intRiskNum == 0) && (intReportType != (int)ReportType.COMPLETE_REQUEST) && (intReportType != (int)ReportType.ONLY_REQUEST))
            //{
            //    intRiskNum = 1;
            //}


            ////Para Impresión de riesgo de exclusión.
            ////int intReportType = (intRiskType != 3) ? Convert.ToInt32(policyData.ReportType) : 2;

            ////Rutas de generación de reporte
            //string[] strPaths = new string[2];

            //try
            //{
            //    //Evalua tipo de reporte
            //    if (intReportType == (int)ReportType.FORMAT_COLLECT)
            //    {
            //        strReportParameters[4] = RiskCount.ToString();
            //        addFormatCollect(strReportParameters, policyData);
            //    }
            //    else if ((intReportType == (int)ReportType.COMPLETE_POLICY) ||
            //        (intReportType == (int)ReportType.ONLY_POLICY) ||
            //        (intReportType == (int)ReportType.TEMPORARY) ||
            //        (PolicyData.ReportType == (int)ReportType.COMPLETE_REQUEST))
            //    {
            //        Paths hj1Vehicle = new Paths(rutas.Count);
            //        hj1Vehicle.setFileName(policyData, ".pdf");
            //        rutas.Add(hj1Vehicle);

            //        strPaths[0] = hj1Vehicle.getPath("VehicleCover", (RiskCount > 1));
            //        strPaths[1] = hj1Vehicle.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //        if ((vc.RowCount > 5) || (intReportType == (int)ReportType.QUOTATION))
            //        {
            //            //Valida si debe mostrar el convenio o no.
            //            if ((intReportType == (int)ReportType.COMPLETE_POLICY) ||
            //                (intReportType == (int)ReportType.TEMPORARY) ||
            //                (intReportType == (int)ReportType.COMPLETE_REQUEST))
            //            {
            //                if (vc.printSecondPage(Version))
            //                {
            //                    Paths hj2Vehicle = new Paths(rutas.Count);
            //                    hj2Vehicle.setFileName(policyData, ".pdf");
            //                    rutas.Add(hj2Vehicle);

            //                    strPaths[0] = hj2Vehicle.getPath("VehicleCoverAppendix", (RiskCount > 1));
            //                    strPaths[1] = hj2Vehicle.FilePath;

            //                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //                }

            //                ///eopiraneque filtro para imprimir solo un plan de pagos cuando es una póliza colectiva
            //                if (intRiskNum == RiskCount)
            //                {
            //                    Paths hj3Vehicle = new Paths(rutas.Count);
            //                    hj3Vehicle.setFileName(policyData, ".pdf");
            //                    rutas.Add(hj3Vehicle);

            //                    strPaths[0] = hj3Vehicle.getPath("VehicleConvection", (RiskCount > 1));
            //                    strPaths[1] = hj3Vehicle.FilePath;

            //                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //                }
            //                ///eopiraneque

            //                if ((policyData.WithFormatCollect) && (intRiskNum == RiskCount))
            //                {
            //                    //strReportParameters[2] = ReportServiceHelper.getBarCode128(PolicyData.PolicyId, PolicyData.EndorsementId);
            //                    if (RiskCount > 1)
            //                    {
            //                        strReportParameters[4] = "0";
            //                    }
            //                    else
            //                    {
            //                        strReportParameters[4] = intRiskNum.ToString();
            //                    }
            //                    addFormatCollect(strReportParameters, policyData);
            //                }
            //            }
            //            else if ((intReportType == (int)ReportType.ONLY_POLICY))
            //            {
            //                if (vc.printSecondPage(Version))
            //                {
            //                    Paths hj5Vehicle = new Paths(rutas.Count);
            //                    hj5Vehicle.setFileName(policyData, ".pdf");
            //                    rutas.Add(hj5Vehicle);

            //                    strPaths[0] = hj5Vehicle.getPath("VehicleCoverAppendix", (RiskCount > 1));
            //                    strPaths[1] = hj5Vehicle.FilePath;

            //                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

            //                    if (policyData.WithFormatCollect)
            //                    {
            //                        addFormatCollect(strReportParameters, policyData);
            //                    }
            //                }
            //                /// eopiraneque Se incluye modificación para que imprima el formato de recaudo

            //                else if ((policyData.WithFormatCollect) && (intRiskNum == RiskCount))
            //                {
            //                    if (RiskCount > 1)
            //                    {
            //                        strReportParameters[4] = "0";
            //                    }
            //                    else
            //                    {
            //                        strReportParameters[4] = intRiskNum.ToString();
            //                    }
            //                    addFormatCollect(strReportParameters, policyData);
            //                }
            //                /// eopiraneque
            //            }

            //        }
            //        else if (intReportType == (int)ReportType.PAYMENT_CONVENTION)
            //        {
            //            Paths hj6Vehicle = new Paths(rutas.Count);
            //            hj6Vehicle.setFileName(policyData, ".pdf");
            //            rutas.Add(hj6Vehicle);

            //            strPaths[0] = hj6Vehicle.getPath("VehicleConvection", (RiskCount > 1));
            //            strPaths[1] = hj6Vehicle.FilePath;
            //            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

            //            if (policyData.WithFormatCollect)
            //            {
            //                addFormatCollect(strReportParameters, policyData);
            //            }
            //        }
            //    }
            //    else if ((intReportType == (int)ReportType.PAYMENT_CONVENTION) && (intRiskNum == RiskCount))
            //    {
            //        Paths hj7Vehicle = new Paths(rutas.Count);
            //        hj7Vehicle.setFileName(policyData, ".pdf");
            //        rutas.Add(hj7Vehicle);

            //        strPaths[0] = hj7Vehicle.getPath("VehicleConvection", (RiskCount > 1));
            //        strPaths[1] = hj7Vehicle.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //        if (intRiskNum == RiskCount)
            //        {
            //            if (policyData.WithFormatCollect)
            //            {
            //                if (RiskCount > 1)
            //                {
            //                    strReportParameters[4] = "0";
            //                }
            //                else
            //                {
            //                    strReportParameters[4] = intRiskNum.ToString();
            //                }
            //                addFormatCollect(strReportParameters, policyData);
            //            }
            //        }
            //    }
            //    else if ((intReportType == (int)ReportType.QUOTATION))
            //    {
            //        Paths hj1Vehicle = new Paths(rutas.Count);
            //        hj1Vehicle.setFileName(policyData, ".pdf");
            //        rutas.Add(hj1Vehicle);

            //        strPaths[0] = hj1Vehicle.getPath("VehicleQuotationCover", (RiskCount > 1));
            //        strPaths[1] = hj1Vehicle.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

            //        if (vc.printSecondPage(Version))
            //        {
            //            Paths hj2Vehicle = new Paths(rutas.Count);
            //            hj2Vehicle.setFileName(policyData, ".pdf");
            //            rutas.Add(hj2Vehicle);

            //            strPaths[0] = hj2Vehicle.getPath("VehicleQuotationAppendix", (RiskCount > 1));
            //            strPaths[1] = hj2Vehicle.FilePath;

            //            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        private void showVehicleReport1(Policy policyData, int intRiskNum, int intRiskType)
        {

        }

        ///////////////////////////////////////////////OJO: SE COMENTA ESTE METODO DE CODIGO//////////////////////////////////////////////////////////////
        /// <summary>
        /// Genera los reportes para el ramo de reponsabilidad civil y los exporta a .pdf.
        /// </summary>
        /// <param name="rc">Datos del reporte de RC.</param>
        /// <param name="policyData">Datos de la póliza</param>
        //private void showCropReport(Crop rc, Policy policyData)
        //{
        //    Paths final = new Paths(rutas.Count);
        //    final.setFileName(policyData, ".pdf");
        //    rutas.Add(final);

        //    //Para saber si la marca de agua debe ser de Temporarios o de Cotizaciones
        //    if (policyData.TempNum == 0)
        //    {
        //        waterMark = "";
        //    }
        //    else
        //    {
        //        if (policyData.PolicyId < 0)
        //        {
        //            waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
        //        }
        //        else
        //        {
        //            waterMark = (policyData.TempNum == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];
        //        }
        //    }

        //    //Parametros reportes Responsabilidad civil
        //    string[] strReportParameters = new string[10];
        //    strReportParameters[0] = policyData.ProcessId.ToString();
        //    strReportParameters[1] = rc.SecondPage.ToString();
        //    strReportParameters[2] = policyData.CodeBar;
        //    strReportParameters[3] = policyData.TempNum.ToString();
        //    strReportParameters[4] = rc.RegisterCount.ToString();
        //    strReportParameters[5] = rc.BeneficiariesCount.ToString();

        //    string[] strPaths = new string[3];//Rutas de generación de reporte


        //    try
        //    {
        //        if (policyData.ReportType != (int)ReportType.QUOTATION)
        //        {
        //            Paths hj1Crop = new Paths(rutas.Count);
        //            hj1Crop.setFileName(policyData, ".pdf");
        //            rutas.Add(hj1Crop);

        //            strPaths[0] = hj1Crop.getPath("CropCover", (RiskCount > 1));
        //            strPaths[1] = hj1Crop.FilePath;

        //            //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
        //        }
        //        //Se valida que tipo de reporte se seleccionó.
        //        if ((policyData.ReportType == (int)ReportType.COMPLETE_POLICY) ||
        //            (policyData.ReportType == (int)ReportType.ONLY_POLICY) ||
        //            (policyData.ReportType == (int)ReportType.TEMPORARY))
        //        {
        //            /* Se valida si existe texto a mostrar en la segunda hoja o si la cantidad
        //             * es amparos es mayor o igual a 6. Cuando la cantidad de amparos es mayor o 
        //             * igual a 6, en la primera hoja se alcanza a mostrar toda la información.*/
        //            if ((rc.SecondPage != 0) || (rc.CoveragesCount >= 6))
        //            {
        //                /* En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
        //                 * Se valida si muestra debe mostrar el convenio o no.*/
        //                if ((policyData.ReportType == (int)ReportType.COMPLETE_POLICY) ||
        //                    (policyData.ReportType == (int)ReportType.TEMPORARY))
        //                {
        //                    Paths hj2Crop = new Paths(rutas.Count);
        //                    hj2Crop.setFileName(policyData, ".pdf");
        //                    rutas.Add(hj2Crop);

        //                    strPaths[0] = hj2Crop.getPath("CropCoverAppendix", (RiskCount > 1));
        //                    strPaths[1] = hj2Crop.FilePath;

        //                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

        //                    /* Reporte Convenio de Pago. Se carga la ruta del reporte a cargar.*/
        //                    Paths hj3Crop = new Paths(rutas.Count);
        //                    hj3Crop.setFileName(policyData, ".pdf");
        //                    rutas.Add(hj3Crop);

        //                    strPaths[0] = hj3Crop.getPath("CropConvection", (RiskCount > 1));
        //                    strPaths[1] = hj3Crop.FilePath;

        //                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

        //                    //intPdfQuantity++;
        //                }
        //                else if (policyData.ReportType == (int)ReportType.ONLY_POLICY)
        //                {
        //                    Paths hj4Crop = new Paths(rutas.Count);
        //                    hj4Crop.setFileName(policyData, ".pdf");
        //                    rutas.Add(hj4Crop);

        //                    strPaths[0] = hj4Crop.getPath("CropCoverAppendix", (RiskCount > 1));
        //                    strPaths[1] = hj4Crop.FilePath;

        //                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
        //                }
        //            }
        //            else
        //            {
        //                // Se omite impresión de Convenio de pago y clausula de coaseguro cedido en caso de ser una cotización.
        //                if (policyData.ReportType != (int)ReportType.QUOTATION)
        //                {
        //                    Paths hj5Crop = new Paths(rutas.Count);
        //                    hj5Crop.setFileName(policyData, ".pdf");
        //                    rutas.Add(hj5Crop);

        //                    strPaths[0] = hj5Crop.getPath("CropConvection", (RiskCount > 1));
        //                    strPaths[1] = hj5Crop.FilePath;

        //                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
        //                }
        //            }
        //        }
        //        else if (policyData.ReportType == (int)ReportType.PAYMENT_CONVENTION)
        //        {
        //            //Reporte Convenio de Pago.
        //            Paths hj6Crop = new Paths(rutas.Count);
        //            hj6Crop.setFileName(policyData, ".pdf");
        //            rutas.Add(hj6Crop);

        //            strPaths[0] = hj6Crop.getPath("CropConvection", (RiskCount > 1));//strArrPaths[0] + ConfigurationSettings.AppSettings["VehicleConvectionFile"];
        //            strPaths[1] = hj6Crop.FilePath;//strArrPaths[intPathIndex];

        //            //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
        //        }
        //        else if ((policyData.ReportType == (int)ReportType.QUOTATION))
        //        {
        //            Paths hj1Crop = new Paths(rutas.Count);
        //            hj1Crop.setFileName(policyData, ".pdf");
        //            rutas.Add(hj1Crop);

        //            strPaths[0] = hj1Crop.getPath("CropQuotationCover", (RiskCount > 1));
        //            strPaths[1] = hj1Crop.FilePath;

        //            //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Muestra el reporte de cumplimiento.
        /// </summary>
        /// <param name="sr">Datos del reporte de Surety</param>
        /// <param name="policyData">Datos de la Póliza</param>
        private void showSuretyReport(Surety sr, Policy policyData)
        {
            //Paths final = new Paths(rutas.Count);
            //final.setFileName(policyData, ".pdf");
            //rutas.Add(final);

            ///* Marca de agua para Temporario o Cotización*/
            //if (policyData.TempNum == 0)
            //{
            //    waterMark = "";
            //}
            //else
            //{
            //    if (policyData.QuotationId == 1)
            //    {
            //        waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
            //    }
            //    else
            //    {
            //        waterMark = (policyData.TempNum == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];
            //    }
            //}

            ////Parametros reportes Cumplimiento
            //string[] strReportParameters = new string[10];
            //strReportParameters[0] = policyData.ProcessId.ToString();
            //strReportParameters[1] = sr.SecondPage.ToString();
            //strReportParameters[2] = policyData.CodeBar;
            //strReportParameters[3] = policyData.TempNum.ToString();
            //strReportParameters[4] = sr.ShowPaymentCert.ToString();
            //strReportParameters[5] = sr.TextLinesCount.ToString();
            //strReportParameters[6] = sr.BeneficiariesCount.ToString();


            //string[] strPaths = new string[3];//Rutas de generación de reporte

            //try
            //{
            //    //Se valida que tipo de reporte se seleccionó.
            //    if ((policyData.ReportType == (int)ReportType.COMPLETE_POLICY) ||
            //        (policyData.ReportType == (int)ReportType.ONLY_POLICY) ||
            //        (policyData.ReportType == (int)ReportType.TEMPORARY))
            //    {
            //        Paths hj1Surety = new Paths(rutas.Count);
            //        hj1Surety.setFileName(policyData, ".pdf");
            //        rutas.Add(hj1Surety);

            //        strPaths[0] = hj1Surety.getPath("SuretyCover", (RiskCount > 1));
            //        strPaths[1] = hj1Surety.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

            //        //Valida si la cantidad de líneas a mostrar de texto es diferente a cero.
            //        if (sr.SecondPage != 0)
            //        {
            //            //En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
            //            //Se valida si muestra debe mostrar el convenio o no.
            //            if ((policyData.ReportType == (int)ReportType.COMPLETE_POLICY) ||
            //                (policyData.ReportType == (int)ReportType.TEMPORARY))
            //            {
            //                Paths hj2Surety = new Paths(rutas.Count);
            //                hj2Surety.setFileName(policyData, ".pdf");
            //                rutas.Add(hj2Surety);

            //                strPaths[0] = hj2Surety.getPath("SuretyCoverAppendix", (RiskCount > 1));
            //                strPaths[1] = hj2Surety.FilePath;

            //                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

            //                //Reporte Convenio de Pago.
            //                Paths hj3Surety = new Paths(rutas.Count);
            //                hj3Surety.setFileName(policyData, ".pdf");
            //                rutas.Add(hj3Surety);

            //                strPaths[0] = hj3Surety.getPath("SuretyConvection", (RiskCount > 1));
            //                strPaths[1] = hj3Surety.FilePath;

            //                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //            }
            //            else if ((policyData.ReportType == (int)ReportType.ONLY_POLICY))
            //            {
            //                //Reporte de Anexo de la Poliza.
            //                Paths hj4Surety = new Paths(rutas.Count);
            //                hj4Surety.setFileName(policyData, ".pdf");
            //                rutas.Add(hj4Surety);

            //                strPaths[0] = hj4Surety.getPath("SuretyCoverAppendix", (RiskCount > 1));
            //                strPaths[1] = hj4Surety.FilePath;

            //                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //            }
            //        }
            //        else if ((policyData.ReportType == (int)ReportType.PAYMENT_CONVENTION) ||
            //                 (policyData.ReportType == (int)ReportType.COMPLETE_POLICY) ||
            //                 (policyData.ReportType == (int)ReportType.TEMPORARY))
            //        {
            //            //Reporte Convenio de Pago.
            //            Paths hj5Surety = new Paths(rutas.Count);
            //            hj5Surety.setFileName(policyData, ".pdf");
            //            rutas.Add(hj5Surety);

            //            strPaths[0] = hj5Surety.getPath("SuretyConvection", (RiskCount > 1));
            //            strPaths[1] = hj5Surety.FilePath;

            //            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //        }
            //    }
            //    else if (policyData.ReportType == (int)ReportType.PAYMENT_CONVENTION)//Opción 3
            //    {
            //        //Reporte Convenio de Pago.
            //        Paths hj6Surety = new Paths(rutas.Count);
            //        hj6Surety.setFileName(policyData, ".pdf");
            //        rutas.Add(hj6Surety);

            //        strPaths[0] = hj6Surety.getPath("SuretyConvection", (RiskCount > 1));
            //        strPaths[1] = hj6Surety.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //    }
            //    else if ((policyData.ReportType == (int)ReportType.QUOTATION))
            //    {
            //        Paths hj1Surety = new Paths(rutas.Count);
            //        hj1Surety.setFileName(policyData, ".pdf");
            //        rutas.Add(hj1Surety);

            //        strPaths[0] = hj1Surety.getPath("SuretyQuotationCover", (RiskCount > 1));
            //        strPaths[1] = hj1Surety.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// Genera los reportes que conforman la caratula de la póliza colectiva o Individual con exclusión.
        /// </summary>
        /// <param name="intProcessId">Id. del Proceso.</param>
        /// <param name="policyData">Datos de la poliza</param>
        /// <returns>Ruta del archivo de la caratula del reporte generado</returns>
        private string showVehicleCoverReport(int intProcessId, Vehicle vc, Policy policyData)
        {
            //Paths caratula = new Paths();
            //caratula.setFileName(policyData, ".pdf");
            //rutas.Add(caratula);

            //Paths hj1Caratula = new Paths(rutas.Count);
            //hj1Caratula.setFileName(policyData, ".pdf");
            //rutas.Add(hj1Caratula);

            //waterMark = (Convert.ToInt32(policyData.TempNum) == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];

            ////Parametros reportes Automoviles
            //string[] strReportParameters = new string[10];
            //strReportParameters[0] = intProcessId.ToString();
            //strReportParameters[1] = (vc.RowCount + 5).ToString();
            //strReportParameters[2] = policyData.CodeBar;
            //strReportParameters[3] = policyData.TempNum.ToString();
            //strReportParameters[4] = policyData.RangeMinValue.ToString();
            //strReportParameters[5] = policyData.RangeMaxValue.ToString();

            ////Rutas de generación de reporte
            //string[] strPaths = new string[3];

            //try
            //{
            //    strPaths[0] = hj1Caratula.getPath("CoverVehicleCover", (RiskCount > 1));
            //    strPaths[1] = hj1Caratula.FilePath;

            //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

            //    if (vc.printSecondPage(Version))
            //    {
            //        Paths hj2Caratula = new Paths(rutas.Count);
            //        hj2Caratula.setFileName(policyData, ".pdf");
            //        rutas.Add(hj2Caratula);

            //        //Reporte de Anexo de la Poliza.
            //        strPaths[0] = hj2Caratula.getPath("CoverVehicleCoverAppendix", (RiskCount > 1));
            //        strPaths[1] = hj2Caratula.FilePath;

            //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            //    }

            //    return caratula.FilePath;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return string.Empty;
        }

        /// <summary>
        /// Genera los reportes que conforman la caratula de la póliza colectiva o Individual con exclusión.
        /// </summary>
        /// <param name="intProcessId">Id. del Proceso.</param>
        /// <param name="policyData">Datos de la poliza</param>
        /// <returns>Ruta del archivo de la caratula del reporte generado</returns>
        private string showVehicleCoverReport1(int intProcessId, Policy policyData)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }

        /// <summary>
        /// Muestra reporte de pólizas migradas
        /// </summary>
        /// <param name="strReportParameters">Parámetros del reporte</param>
        /// <param name="strPaths">Rutas para el reporte</param>
        private void showMigratedPoliciesList(string[] strReportParameters, string[] strPaths)
        {
            if (strReportParameters[3].Equals(true.ToString()))
            {
                //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.Excel);
            }
            else
            {
                //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);
            }
        }
        #endregion

        /// <summary>
        /// Adiciona formato de recaudo a la impresión actual
        /// </summary>
        /// <param name="rptParams">Parametros del reporte</param>
        /// <param name="policyData">Datos de la póliza</param>
        public void addFormatCollect(string[] rptParams, Policy policyData)
        {
            //Rutas de generación de reporte
            string[] strPaths = new string[2];
            string waterMark = String.Empty;
            string[] paramBarCode = new string[2];
            Paths formatCollect = new Paths(rutas.Count);
            formatCollect.setFileName(policyData, ".pdf");
            rutas.Add(formatCollect);

            strPaths[0] = formatCollect.getPath("FormatCollect", (RiskCount > 1));
            strPaths[1] = formatCollect.FilePath;
            paramBarCode = ReportServiceHelper.getBarCode128(rptParams, policyData.EndorsementId);
            rptParams[2] = paramBarCode[1];
            rptParams[5] = paramBarCode[0];

            //ReportServiceHelper.loadReportFile(rptParams, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
        }

        /// <summary>
        /// Sobrecarga que devuelve un string
        /// </summary>
        /// <param name="policyData">Datos de la póliza</param>
        /// <param name="rptParams">Parametros del reporte</param>
        /// <returns>Ruta de archivo utilizada cuando se imprime solo el formato de recaudo</returns>
        public string addFormatCollect(Policy policyData, string[] rptParams)
        {
            //Rutas de generación de reporte
            string[] strPaths = new string[2];
            string waterMark = String.Empty;
            string[] paramBarCode = new string[2];
            Paths formatCollect = new Paths(rutas.Count);
            formatCollect.setFileName(policyData, ".pdf");
            rutas.Add(formatCollect);

            strPaths[0] = formatCollect.getPath("FormatCollect", (RiskCount > 1));
            strPaths[1] = formatCollect.FilePath;
            paramBarCode = ReportServiceHelper.getBarCode128(rptParams, policyData.EndorsementId);
            rptParams[2] = paramBarCode[1];
            rptParams[5] = paramBarCode[0];
            //ReportServiceHelper.loadReportFile(rptParams, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            return strPaths[1];
        }
    }
}
