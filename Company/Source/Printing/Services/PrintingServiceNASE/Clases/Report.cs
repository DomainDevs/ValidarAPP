using CrystalDecisions.Shared;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServicesNase.Clases;
using Sistran.Company.Application.PrintingServicesNase.Enum;
using Sistran.Company.Application.PrintingServicesNase.Resources;
using Sistran.Company.PrintingService.NASE.spResponse;
using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class Report
    {
        #region Properties
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
        private ArrayList blankPaths;
        private bool _hasClauses;
        // << TODO: Autor: Miguel López; Fecha: 04/10/2010; Asunto: Esta propiedad guarda el código del tipo de póliza,
        //                                                          para realizar la discriminación en la impresión de Caución Judicial
        private int _policyType;

        public int PolicyType
        {
            get { return _policyType; }
            set { _policyType = value; }
        }
        /* Autor: Miguel López, Fecha: 04/10/2010 >>*/

        // << TODO: Autor: Miguel López; Fecha: 21/10/2010; Asunto: Esta propiedad discrimina si se imprime un carnet


        private bool _isLicense;
        // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
        private int _balancePremium;
        // Edgar O. Piraneque E.; 05/11/2010;>>

        // << TODO: Edgar O. Piraneque E.; 20/12/2010; Se incluye propiedad para manejar máximas filas a imprimir
        private int _maxRow;
        // Edgar O. Piraneque E.; 20/12/2010; Se incluye propiedad para manejar máximas filas a imprimir>>

        //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 23/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
        private int _renewalProcessId;
        /* Autor: Luisa Fernanda Ramírez, Fecha: 23/12/2010 >>*/

        // << TODO: Autor: Miguel López; Fecha: 25/03/2011; Asunto: Propiedades para el código del tomador
        //															en impresión de pagaré
        private int _insuredIndividualId;

        public int InsuredIndividualId
        {
            get { return _insuredIndividualId; }
            set { _insuredIndividualId = value; }
        }

        private int _insuredId;

        public int InsuredId
        {
            get { return _insuredId; }
            set { _insuredId = value; }
        }
        /*Autor: MIguel López; Fecha: 25/03/2011 >>*/

        // << TODO: Autor: Miguel López; Fecha: 27/05/2011; Asunto: Propiedad bandera de impresión de formato de pagaré
        private int _printPromissoryNote;

        public int PrintPromissoryNote
        {
            get { return _printPromissoryNote; }
            set { _printPromissoryNote = value; }
        }

        /*Autor: MIguel López; Fecha: 27/05/2011 >>*/

        public bool IsLicense
        {
            get { return _isLicense; }
            set { _isLicense = value; }
        }

        /* Autor: Miguel López, Fecha: 21/10/2010 >>*/


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

        public bool HasClauses
        {
            get { return _hasClauses; }
            set { _hasClauses = value; }
        }
        // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g
        public int BalancePremium
        {
            get { return _balancePremium; }
            set { _balancePremium = value; }
        }
        // Edgar O. Piraneque E.; 05/11/2010;>>
        // << TODO: Edgar O. Piraneque E.; 20/12/2010; Se incluye propiedad para manejar máximas filas a imprimir
        public int MaxRow
        {
            get { return _maxRow; }
            set { _maxRow = value; }
        }
        // Edgar O. Piraneque E.; 20/12/2010;>>

        //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 23/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
        public int RenewalProcessId
        {
            get { return _renewalProcessId; }
            set { _renewalProcessId = value; }
        }
        /* Autor: Luisa Fernanda Ramírez, Fecha: 23/12/2010 >>*/

        #endregion

        #region Constructor
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
            // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g
            BalancePremium = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["BalancePremium"].ToString());
            // Edgar O. Piraneque E.; 05/11/2010;>>
            IsLicense = false;
        }

        public Report()
        {

        }
        #endregion

        #region Additional Methods

        //<<TODO: Autor: Miguel López; Fecha: 08/06/2011; Asunto: Validamos si la póliza tiene coaseguro cedido. Si es así
        //                                                        se imprimirá el formato de cláusula de coaseguro.
        private bool HasCoinsuranceAssigned(Policy policyData)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = new DataTable();
            dt = dda.ExecuteDataTable("SELECT POLICY_ID FROM ISS.POLICY WHERE POLICY_ID = " + policyData.PolicyId + " AND BUSINESS_TYPE_CD = 3");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                dda = new DynamicDataAccess();
                dt = new DataTable();
                dt = dda.ExecuteDataTable("SELECT TEMP_ID FROM TMP.TEMP_SUBSCRIPTION WHERE TEMP_ID = " + policyData.TempNum + " AND BUSINESS_TYPE_CD = 3");
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
        /*Autor: Miguel López; Fecha: 08/06/2011 >>*/

        // <<TODO: Autor: Miguel López; Fecha: 02/06/2011; Asunto: Validamos si la póliza para imprimir tiene cláusulas.
        private bool ClausesExist(Policy policyData)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = new DataTable();
            dt = dda.ExecuteDataTable("SELECT PROCESS_ID FROM REPORT.CO_TMP_POLICY_CLAUSES WHERE POLICY_ID = " + policyData.PolicyId + " AND PROCESS_ID = " + policyData.ProcessId + " AND ENDORSEMENT_ID = " + policyData.EndorsementId);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*Autor: Miguel López; Fecha: 02/06/2011 >>*/

        // <<TODO: Autor: Miguel López; Fecha: 25/03/2011; Asunto: Métodos para obtener los códigos del tomador, determinar
        //														   si el tomador es consorcio y discriminar si el tomador es persona
        //														   o compañía.
        private void GetInsuredData(Policy policyData)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = new DataTable();

            dt = dda.ExecuteDataTable("SELECT POLICYHOLDER_ID FROM ISS.POLICY WHERE POLICY_ID = " + policyData.PolicyId);
            if (dt.Rows[0]["POLICYHOLDER_ID"].ToString() == string.Empty)
            {
                _insuredIndividualId = 0;
            }
            else
            {
                _insuredIndividualId = Convert.ToInt32(dt.Rows[0]["POLICYHOLDER_ID"].ToString());
            }

            dt = new DataTable();

            dt = dda.ExecuteDataTable("SELECT INSURED_CD FROM UP.INSURED WHERE INDIVIDUAL_ID = " + _insuredIndividualId);

            if (dt.Rows[0]["INSURED_CD"].ToString() == string.Empty)
            {
                _insuredId = 0;
            }
            else
            {
                _insuredId = Convert.ToInt32(dt.Rows[0]["INSURED_CD"].ToString());
            }
        }
        //TODO: Fecha:04/10/2011 ;Autor: John Ruiz; Asunto: Metodo que recupera el insuredCd para impresion de pagare en polizas de cumplimiento ; Compañia: 3
        private string GetInsuredByIndividualId(string individualId)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = new DataTable();

            dt = dda.ExecuteDataTable("SELECT INSURED_CD FROM UP.INSURED WHERE INDIVIDUAL_ID = " + individualId);

            if (dt.Rows[0]["INSURED_CD"].ToString() == string.Empty)
            {
                return "0";
            }
            else
            {
                return dt.Rows[0]["INSURED_CD"].ToString();
            }
        }
        //TODO: Fecha:04/10/2011 ;Autor: John Ruiz; Asunto:Metodo que recupera el numero de pagare para impresion de pagare en polizas de cumplimiento ; Compañia: 3
        private string GetPromisoryNoteNum(Policy policyData)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = new DataTable();

            dt = dda.ExecuteDataTable("SELECT PROMISSORY_NOTE_NUMBER FROM REPORT.CO_TMP_PROMISSORY_NOTE_INDIVIDUAL WHERE PROCESS_ID = " + policyData.ProcessId);

            if (dt.Rows[0]["PROMISSORY_NOTE_NUMBER"].ToString().Equals(string.Empty))
            {
                return "0";
            }
            else
            {
                return dt.Rows[0]["PROMISSORY_NOTE_NUMBER"].ToString();
            }
        }
        //TODO: Fecha:04/10/2011 ;Autor: John Ruiz; Asunto:Metodo que recupera los consorciados para impresion de pagare en polizas de cumplimiento ; Compañia: 3
        private DataTable GetConsortiumIndividualsIds(Policy policyData)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            return dda.ExecuteDataTable("SELECT INDIVIDUAL_ID FROM REPORT.CO_TMP_PROMISSORY_NOTE_CONSORTIUM WHERE PROCESS_ID = " + policyData.ProcessId);
        }

        private bool IsConsortium()
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = dda.ExecuteDataTable("SELECT  COUNT (*) FROM UP.CO_CONSORTIUM WHERE INSURED_CD = " + _insuredId);
            if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsPerson()
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = dda.ExecuteDataTable("SELECT INDIVIDUAL_ID FROM UP.PERSON WHERE INDIVIDUAL_ID = " + _insuredIndividualId);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsCompany()
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable dt = dda.ExecuteDataTable("SELECT INDIVIDUAL_ID FROM UP.COMPANY WHERE INDIVIDUAL_ID = " + _insuredIndividualId);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*Autor: Miguel López; Fecha: 25/03/2011 >>*/

        private bool IsArticle(int processId, int articleNumber)
        {
            string articleName = string.Empty;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable table = dda.ExecuteDataTable("SELECT * FROM REPORT.CO_TMP_POLICY_CAUTION WHERE PROCESS_ID = " + processId +
                                                   " AND ARTICLE_DESCRIPTION LIKE '%" + articleNumber + "%'");
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private object GetParameterValue(int parameterId, System.Type parameterType)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable table = dda.ExecuteDataTable("SELECT * FROM COMM.PARAMETER WHERE PARAMETER_ID = " + parameterId);
            if (table.Rows.Count > 0)
            {

                if (parameterType == typeof(System.Int32))
                {
                    return table.Rows[0]["NUMBER_PARAMETER"];
                }
                else if (parameterType == typeof(System.String))
                {
                    return table.Rows[0]["TEXT_PARAMETER"];
                }
                else if (parameterType == typeof(System.Boolean))
                {
                    return table.Rows[0]["BOOL_PARAMETER"];
                }
                else if (parameterType == typeof(System.DateTime))
                {
                    return table.Rows[0]["DATE_PARAMETER"];
                }
                else if (parameterType == typeof(System.Double))
                {
                    return table.Rows[0]["AMOUNT_PARAMETER"];
                }
                else
                {
                    return table.Rows[0]["TEXT_PARAMETER"];
                }
            }
            else
            {
                return null;
            }
        }

        // << TODO: Autor: Miguel López; Fecha: 05/11/2010; Asunto: Método para validar si la impresión
        //                                                          de una póliza RC corresponde a RC Pasajeros
        private bool IsRcPassenger(Policy policyData, int passengerCode)
        {
            // << TODO: Autor: Edgar Piraneque
            DynamicDataAccess dda = new DynamicDataAccess();
            NameValue[] parametros = new NameValue[4];
            parametros[0] = new NameValue("POLICY_ID", Convert.ToInt32(policyData.PolicyId));
            parametros[1] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(policyData.EndorsementId));
            parametros[2] = new NameValue("TEMP_ID", Convert.ToInt32(policyData.TempNum));
            parametros[3] = new NameValue("COVERED_RISK_TYPE_CD", Convert.ToInt32(passengerCode));
            DataTable result = dda.ExecuteSPDataTable("REPORT.VALIDATE_CROP_COVER", parametros);
            if (result.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Autor: Miguel López, Fecha: 05/11/2010 >>*/

        #endregion

        #region Get
        /// <summary>
        /// Genera el reporte de poliza individual.
        /// </summary>
        /// <param name="policyData">Datos de la póliza.</param>
        /// <param name="paramsp">Parámetro del SP que genera el conjunto de datos del reporte.</param>
        /// <returns></returns>
        private string getIndividualPolicyReport(Policy policyData, NameValue[] paramsp)
        {
            DataSet dsInfo;
            DataTable dt1, dt2;
            string strFinalReportPath = string.Empty;

            //Se determina que procedimiento almacenado se invocará dependendiendo del ramo.
            switch (policyData.PrefixNum)
            {
                case ((int)ReportEnum.PrefixCode.AUTOS):
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Vehicle vc = new Vehicle(dt1, dt2);

                    //Se valida que haya datos para mostrar.
                    if (vc.RegisterCount != 0)
                    {
                        //Crea los reportes.
                        showVehicleReport(policyData, vc, 0, 0);
                        ///HD-2317 04/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Se incluye validación cuando se imprime solo formato de recaudo y por
                        ///realizar devolución de prima no se genera reporte

                        if (rutas.Count > 1)
                        {
                            ReportServiceHelper.joinPdfFiles(rutas);
                            //Se retorna la ruta del reporte final.
                            strFinalReportPath = ((Paths)rutas[0]).FilePath;
                        }
                        else
                        {
                            if (vc.PrintFormat == 0)
                            {
                                strFinalReportPath = "PRINTFORMAT";
                            }
                            else
                            {
                                strFinalReportPath = "";
                            }
                        }
                        /// FIN HD-2317 *********************************
                    }
                    break;

                case ((int)ReportEnum.PrefixCode.CROP):
                    NameValue[] cropSpParams = new NameValue[4];
                    cropSpParams[0] = paramsp[0];
                    cropSpParams[1] = paramsp[1];
                    cropSpParams[2] = paramsp[2];
                    cropSpParams[3] = paramsp[3];

                    // << TODO: Autor: Miguel López; Fecha: 05/11/2010; Asunto: Llamamos al método de validación para
                    //                                                          discriminar si se hace la impresión RC pasajeros
                    //                                                          o una impresión RC normal.
                    if (!IsRcPassenger(policyData, (int)ReportEnum.PrefixPolicyType.RC_PASSENGER))
                    {
                        //SP que llenará las tablas para los reportes.
                        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_CROP_POLICY_COVER", cropSpParams);
                        dt1 = dsInfo.Tables[0];
                        dt2 = dsInfo.Tables[1];

                        Crop rc = new Crop(dt1, dt2);


                        /*<< TODO: 
                         * Autor: Manuel E Gomez.
                         * Fecha: 16/04/2014.
                         * Asunto: CAR-43 se elimina el convenio de primas en la impresión de polizas.
                         * Compañia: Cardinal.
                         */
                        //rc.PrintConvection = 0;
                        /*<< Autor: Manuel E Gomez. Fecha: 16/04/2014.*/

                        //Se valida que haya datos para mostrar.
                        if (rc.RegisterCount == 0)
                        {
                            //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                            strFinalReportPath = "";
                        }
                        else
                        {
                            //Este metodo crea los reportes y los concatena.
                            showCropReport(rc, policyData);
                            //HD-2181
                            if (rutas.Count > 1)
                            {
                                ReportServiceHelper.joinPdfFiles(rutas);

                                //Se retorna la ruta del reporte final.
                                strFinalReportPath = ((Paths)rutas[0]).FilePath;
                            }
                            else
                            {
                                if (rc.PrintFormat == 0)
                                {
                                    strFinalReportPath = "PRINTFORMAT";
                                }
                                else
                                {
                                    strFinalReportPath = "";
                                }
                            }
                            /// FIN HD-2181 
                        }
                    }
                    else
                    {
                        //SP que llenará las tablas para los reportes.
                        dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_PASSENGER_POLICY_COVER", paramsp);
                        dt1 = dsInfo.Tables[0];
                        dt2 = dsInfo.Tables[1];

                        PassengerCrop pcp = new PassengerCrop(dt1, dt2);

                        /*<< TODO: 
                         * Autor: Manuel E Gomez.
                         * Fecha: 16/04/2014.
                         * Asunto: CAR-43 se elimina el convenio de primas en la impresión de polizas.
                         * Compañia: Cardinal.
                         */
                        //pcp.PrintConvection = 0;
                        /*<< Autor: Manuel E Gomez. Fecha: 16/04/2014.*/

                        //Se valida que haya datos para mostrar.
                        if (pcp.RegisterCount == 0)
                        {
                            //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                            strFinalReportPath = "";
                        }
                        else
                        {
                            //Este metodo crea los reportes y los concatena.
                            showPassengerReport(pcp, policyData, 0, 0);
                            //HD-2181
                            if (rutas.Count > 1)
                            {
                                //<<TODO: Autor: Miguel López; Fecha: 21/10/2010; Asunto: Evaluamos si el reporte de RC Pasajeros
                                //                                                corresponde a un reporte de póliza o a un carnet.

                                if (!this.IsLicense)
                                {
                                    ReportServiceHelper.joinPdfFiles(rutas);
                                }
                                else
                                {
                                    ReportServiceHelper.joinPdfFiles(rutas, 0, 0, 0, 0);
                                }

                                /* Autor: Miguel López; Fecha: 21/10/2010 >>*/

                                //Se retorna la ruta del reporte final.
                                strFinalReportPath = ((Paths)rutas[0]).FilePath;

                            }
                            else
                            {
                                if (pcp.PrintFormat == 0)
                                {
                                    strFinalReportPath = "PRINTFORMAT";
                                }
                                else
                                {
                                    strFinalReportPath = "";
                                }
                            }
                            /// FIN HD-2181 
                        }
                    }
                    /* Autor: Miguel López, Fecha: 05/11/2010 >>*/

                    break;

                case ((int)ReportEnum.PrefixCode.COMPLIANCE): /* Cumplimiento. Variable que almacena el valor que indica si se debe o no mostrar 
                                                                   * el certificado de pago del reporte de cumplimiento*/
                                                              //Reasigna parametros del sp
                                                              //TODO:  <<Autor: Edgar Cervantes De Los Rios; Fecha: 02/08/2010; Asunto: HD 2508 - Se agrega checkBox para validar si en la impresión, la fecha desde debe ser la misma fecha de inicio de vigencia de la póliza. Esto solo aplica para cumplimiento. Compañía: 1 - CPT.
                    NameValue[] suretySpParams = new NameValue[5];
                    /* Autor: Edgar Cervantes De Los Rios, Fecha: 02/08/2010 >>*/
                    suretySpParams[0] = paramsp[0];
                    suretySpParams[1] = paramsp[1];
                    suretySpParams[2] = paramsp[2];
                    suretySpParams[3] = paramsp[3];

                    //TODO:  <<Autor: Edgar Cervantes De Los Rios; Fecha: 02/08/2010; Asunto: HD 2508 - Se agrega checkBox para validar si en la impresión, la fecha desde debe ser la misma fecha de inicio de vigencia de la póliza. Esto solo aplica para cumplimiento. Compañía: 1 - CPT.
                    suretySpParams[4] = new NameValue("PRINT_FROM_CURRENT_FROM_DATE", dsOutput.Tables["PolicyPrinting"].Rows[0]["PrintFromCurrentFromDate"]);
                    /* Autor: Edgar Cervantes De Los Rios, Fecha: 02/08/2010 >>*/

                    //Se invoca el procedimiento almacenado que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_SURETY_POLICY_COVER", suretySpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Surety sr = new Surety(dt1, dt2);

                    /*<< TODO: 
                         * Autor: Manuel E Gomez.
                         * Fecha: 16/04/2014.
                         * Asunto: CAR-43 se elimina el convenio de primas en la impresión de polizas.
                         * Compañia: Cardinal.
                         */
                    //sr.PrintConvection = 0;
                    /*<< Autor: Manuel E Gomez. Fecha: 16/04/2014.*/

                    //Se valida que haya datos para mostrar.
                    if (sr.RegisterCount == 0)
                    {
                        strFinalReportPath = "";//Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                    }
                    else
                    {
                        //Crea los reportes y los concatena.
                        showSuretyReport(sr, policyData);
                        ///HD-2317 04/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Se incluye validación cuando se imprime solo formato de recaudo y por
                        ///realizar devolución de prima no se genera reporte

                        if (rutas.Count > 1)
                        {
                            ReportServiceHelper.joinPdfFiles(rutas);
                            //Se retorna la ruta del reporte final.
                            strFinalReportPath = ((Paths)rutas[0]).FilePath;
                        }
                        else
                        {
                            if (sr.PrintFormat == 0)
                            {
                                strFinalReportPath = "PRINTFORMAT";
                            }
                            else
                            {
                                strFinalReportPath = "";
                            }
                        }
                        ///FIN HD-2317 ***************************************
                    }
                    break;

                case ((int)ReportEnum.PrefixCode.LOCATION):
                    NameValue[] LocationSpParams = new NameValue[6];
                    LocationSpParams[0] = paramsp[0];
                    LocationSpParams[1] = paramsp[1];
                    LocationSpParams[2] = paramsp[2];
                    LocationSpParams[3] = paramsp[3];
                    LocationSpParams[4] = paramsp[5];
                    LocationSpParams[5] = paramsp[6];

                    //SP que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_LOCATION_POLICY_COVER", LocationSpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Location lc = new Location(dt1, dt2);

                    /*<< TODO: 
                         * Autor: Manuel E Gomez.
                         * Fecha: 16/04/2014.
                         * Asunto: CAR-43 se elimina el convenio de primas en la impresión de polizas.
                         * Compañia: Cardinal.
                         */
                    //lc.PrintConvection = 0;
                    /*<< Autor: Manuel E Gomez. Fecha: 16/04/2014.*/

                    //Se valida que haya datos para mostrar.
                    if (lc.RegisterCount == 0)
                    {
                        //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                        strFinalReportPath = "";
                    }
                    else
                    {
                        //Este metodo crea los reportes y los concatena.
                        showLocationReport(lc, policyData);
                        //HD-2181
                        if (rutas.Count > 1)
                        {
                            ReportServiceHelper.joinPdfFiles(rutas);

                            //Se retorna la ruta del reporte final.
                            strFinalReportPath = ((Paths)rutas[0]).FilePath;
                        }
                        else
                        {
                            if (lc.PrintFormat == 0)
                            {
                                strFinalReportPath = "PRINTFORMAT";
                            }
                            else
                            {
                                strFinalReportPath = "";
                            }
                        }
                        /// FIN HD-2181 
                    }
                    break;

                //<<TODO: Autor: Miguel López; Fecha: 20/08/2010; Asunto: Agregamos los casos cuando el ramo
                //                                                es RC Pasajeros o Caución. Generamos los objetos
                //                                                con los datos y llamamos al método que genera
                //                                                los reportes para los ramos en cuestión
                case ((int)ReportEnum.PrefixCode.PASSENGER_CROP):
                    NameValue[] PassengerSpParams = new NameValue[4];
                    PassengerSpParams[0] = paramsp[0];
                    PassengerSpParams[1] = paramsp[1];
                    PassengerSpParams[2] = paramsp[2];
                    PassengerSpParams[3] = paramsp[3];

                    //SP que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_PASSENGER_POLICY_COVER", paramsp);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    PassengerCrop pc = new PassengerCrop(dt1, dt2);

                    //Se valida que haya datos para mostrar.
                    if (pc.RegisterCount == 0)
                    {
                        //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                        strFinalReportPath = "";
                    }
                    else
                    {
                        //Este metodo crea los reportes y los concatena.
                        showPassengerReport(pc, policyData, 0, 0);
                        //HD-2181
                        if (rutas.Count > 1)
                        {
                            //<<TODO: Autor: Miguel López; Fecha: 21/10/2010; Asunto: Evaluamos si el reporte de RC Pasajeros
                            //                                                corresponde a un reporte de póliza o a un carnet.

                            if (!this.IsLicense)
                            {
                                ReportServiceHelper.joinPdfFiles(rutas);
                            }
                            else
                            {
                                ReportServiceHelper.joinPdfFiles(rutas, 0, 0, 0, 0);
                            }

                            /* Autor: Miguel López; Fecha: 21/10/2010 >>*/

                            //Se retorna la ruta del reporte final.
                            strFinalReportPath = ((Paths)rutas[0]).FilePath;

                        }
                        else
                        {
                            if (pc.PrintFormat == 0)
                            {
                                strFinalReportPath = "PRINTFORMAT";
                            }
                            else
                            {
                                strFinalReportPath = "";
                            }
                        }
                        /// FIN HD-2181 
                    }
                    break;


                case ((int)ReportEnum.PrefixCode.CAUTION):
                    NameValue[] CautionSpParams = new NameValue[4];
                    CautionSpParams[0] = paramsp[0];
                    CautionSpParams[1] = paramsp[1];
                    CautionSpParams[2] = paramsp[2];
                    CautionSpParams[3] = paramsp[3];

                    //SP que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_CAUTION_POLICY_COVER", CautionSpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Caution bd = new Caution(dt1, dt2);

                    /*<< TODO: 
                         * Autor: Manuel E Gomez.
                         * Fecha: 16/04/2014.
                         * Asunto: CAR-43 se elimina el convenio de primas en la impresión de polizas.
                         * Compañia: Cardinal.
                         */
                    //bd.PrintConvection = 0;
                    /*<< Autor: Manuel E Gomez. Fecha: 16/04/2014.*/

                    //Se valida que haya datos para mostrar.
                    if (bd.RegisterCount == 0)
                    {
                        //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                        strFinalReportPath = "";
                    }
                    else
                    {
                        //Este metodo crea los reportes y los concatena.
                        showCautionReport(bd, policyData);
                        //HD-2181
                        if (rutas.Count > 1)
                        {
                            ReportServiceHelper.joinPdfFiles(rutas);

                            //Se retorna la ruta del reporte final.
                            strFinalReportPath = ((Paths)rutas[0]).FilePath;
                        }
                        else
                        {
                            if (bd.PrintFormat == 0)
                            {
                                strFinalReportPath = "PRINTFORMAT";
                            }
                            else
                            {
                                strFinalReportPath = "";
                            }
                        }
                        /// FIN HD-2181 
                    }
                    break;
                /* Autor: Miguel López; Fecha: 20/08/2010 >>*/

                //TODO:  <<Autor: Nicolas Gonzalez R. - NGR ; Fecha: 12/11/2015; 
                //Asunto: Se agrega funcionalidad para el ramo de arrendamiento
                case ((int)ReportEnum.PrefixCode.LEASE): /* Arrendamiento. Variable que almacena el valor que indica si se debe o no mostrar 
                                                                   * el certificado de pago del reporte de arrendamiento */
                                                         //Reasigna parametros del sp
                    NameValue[] leaseSpParams = new NameValue[5];
                    leaseSpParams[0] = paramsp[0];
                    leaseSpParams[1] = paramsp[1];
                    leaseSpParams[2] = paramsp[2];
                    leaseSpParams[3] = paramsp[3];
                    leaseSpParams[4] = new NameValue("PRINT_FROM_CURRENT_FROM_DATE", dsOutput.Tables["PolicyPrinting"].Rows[0]["PrintFromCurrentFromDate"]);

                    //Se invoca el procedimiento almacenado que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_SURETY_POLICY_COVER", leaseSpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Surety srLease = new Surety(dt1, dt2);

                    //Se valida que haya datos para mostrar.
                    if (srLease.RegisterCount == 0)
                    {
                        strFinalReportPath = "";//Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                    }
                    else
                    {
                        //Crea los reportes y los concatena.
                        showSuretyReport(srLease, policyData);
                        if (rutas.Count > 1)
                        {
                            ReportServiceHelper.joinPdfFiles(rutas);
                            //Se retorna la ruta del reporte final.
                            strFinalReportPath = ((Paths)rutas[0]).FilePath;
                        }
                        else
                        {
                            if (srLease.PrintFormat == 0)
                            {
                                strFinalReportPath = "PRINTFORMAT";
                            }
                            else
                            {
                                strFinalReportPath = "";
                            }
                        }
                    }
                    break;
                    // Autor: Nicolas Gonzalez R. - NGR, Fecha: 12/11/2015 >>
            }

            //Ruta reporte final.
            return strFinalReportPath;
        }

        /// <summary>
        /// Genera Reporte Formato de recaudo cuando se requiere individual
        /// </summary>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <returns></returns>
        private string getFormatCollectReport(Policy policyData, NameValue[] paramsp)
        {
            DataSet dsInfo;
            DataTable dt1, dt2;
            string strFinalReportPath = string.Empty;

            //Se determina que procedimiento almacenado se invocará dependendiendo del ramo.
            switch (policyData.PrefixNum)
            {
                case ((int)ReportEnum.PrefixCode.AUTOS):
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Vehicle vc = new Vehicle(dt1, dt2);

                    /*<< TODO: 
                         * Autor: Manuel E Gomez.
                         * Fecha: 16/04/2014.
                         * Asunto: CAR-43 se elimina el convenio de primas en la impresión de polizas.
                         * Compañia: Cardinal.
                         */
                    //vc.PrintConvection = 0;
                    /*<< Autor: Manuel E Gomez. Fecha: 16/04/2014.*/

                    //Se valida que haya datos para mostrar.
                    if (vc.RegisterCount != 0)
                    {
                        //Crea los reportes.
                        if ((policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION) || (policyData.ReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT))
                        {
                            policyData.RangeMinValue = 0;
                            policyData.RangeMaxValue = 0;
                        }
                        showVehicleReport(policyData, vc, 0, 0);

                        ReportServiceHelper.joinPdfFiles(rutas);

                        //Se retorna la ruta del reporte final.
                        strFinalReportPath = ((Paths)rutas[0]).FilePath;
                    }
                    break;

                case ((int)ReportEnum.PrefixCode.CROP):
                    NameValue[] cropSpParams = new NameValue[4];
                    cropSpParams[0] = paramsp[0];
                    cropSpParams[1] = paramsp[1];
                    cropSpParams[2] = paramsp[2];
                    cropSpParams[3] = paramsp[3];

                    //SP que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_CROP_POLICY_COVER", cropSpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Crop rc = new Crop(dt1, dt2);

                    //Se valida que haya datos para mostrar.
                    if (rc.RegisterCount == 0)
                    {
                        //Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                        strFinalReportPath = "";
                    }
                    else
                    {
                        //Este metodo crea los reportes y los concatena.
                        showCropReport(rc, policyData);

                        ReportServiceHelper.joinPdfFiles(rutas);

                        //Se retorna la ruta del reporte final.
                        strFinalReportPath = ((Paths)rutas[0]).FilePath;
                    }
                    break;

                case ((int)ReportEnum.PrefixCode.COMPLIANCE): /* Cumplimiento. Variable que almacena el valor que indica si se debe o no mostrar 
                                                                   * el certificado de pago del reporte de cumplimiento*/
                                                              //Reasigna parametros del sp
                    NameValue[] suretySpParams = new NameValue[4];
                    suretySpParams[0] = paramsp[0];
                    suretySpParams[1] = paramsp[1];
                    suretySpParams[2] = paramsp[2];
                    suretySpParams[3] = paramsp[3];

                    //Se invoca el procedimiento almacenado que llenará las tablas para los reportes.
                    dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_SURETY_POLICY_COVER", suretySpParams);
                    dt1 = dsInfo.Tables[0];
                    dt2 = dsInfo.Tables[1];

                    Surety sr = new Surety(dt1, dt2);

                    //Se valida que haya datos para mostrar.
                    if (sr.RegisterCount == 0)
                    {
                        strFinalReportPath = "";//Se retorna la ruta en blanco y se valida en donde se invoca a este metodo.
                    }
                    else
                    {
                        //Crea los reportes y los concatena.
                        showSuretyReport(sr, policyData);

                        ReportServiceHelper.joinPdfFiles(rutas);

                        //Se retorna la ruta del reporte final.
                        strFinalReportPath = ((Paths)rutas[0]).FilePath;
                    }
                    break;
            }
            //Ruta reporte final.
            return strFinalReportPath;
        }

        /// <summary>
        /// Genera el reporte de cada riesgo de la poliza colectiva
        /// </summary>
        /// <param name="policyData">Datos de la poliza</param>
        /// <param name="paramsp">Parametros del reporte</param>
        /// <param name="policyCoverData">Datos de la caratula generada</param>
        /// <param name="dsPolicyRisks">DataSet de los riesgos de la poliza</param>
        public void showCollectivePolicyRisks(Policy policyData)
        {
            policyData.CodeBar = policyData.PrefixNum.ToString() + policyData.PolicyId.ToString() + policyData.EndorsementId.ToString();
            int currentRisk = 1;
            //Rutas de generación de reporte
            string[] strPaths = new string[2];
            //Parámetros a enviar al reporte por cada riesgo
            string[] strReportParameters = new string[5];
            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = MaxRow.ToString();
            strReportParameters[2] = policyData.CodeBar;
            strReportParameters[3] = policyData.TempNum.ToString();
            strReportParameters[4] = "0";


            PolicyData.ReportType = (int)ReportEnum.ReportType.ONLY_POLICY;

            while (currentRisk <= PolicyData.RangeMaxValue)
            {
                strPaths[0] = "";
                strPaths[1] = "";
                switch (currentRisk)
                {
                    case 1:

                        Paths hj1Vehicle = new Paths(rutas.Count);
                        hj1Vehicle.setFileName(policyData, ".pdf");
                        rutas.Add(hj1Vehicle);

                        strPaths[0] = hj1Vehicle.getPath("VehicleCover", true);
                        strPaths[1] = hj1Vehicle.FilePath;
                        break;
                    case 2:
                        Paths hj2Vehicle = new Paths(rutas.Count);
                        hj2Vehicle.setFileName(policyData, ".pdf");
                        rutas.Add(hj2Vehicle);

                        strPaths[0] = hj2Vehicle.getPath("VehicleCover", true);
                        strPaths[1] = hj2Vehicle.FilePath;
                        break;
                    default:
                        break;
                }

                strReportParameters[4] = currentRisk.ToString();
                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                currentRisk++;
            }


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
                    int intRiskCount = 0;

                    if (PolicyData.PrefixNum != Convert.ToInt32(ReportEnum.PrefixCode.LOCATION))
                    {
                        //Número de riesgos, cantidad de coverturas, tipo de poliza y estados de riesgo.
                        intRiskCount = DsPolicyRisks.Tables[0].Rows.Count;
                        //eopiraneque 
                        //int intRiskCount = 0;
                        RiskCount = PolicyData.RangeMaxValue - PolicyData.RangeMinValue + 1;
                    }
                    else
                    {
                        intRiskCount = RiskCount = 1;
                    }

                    if (RiskCount > 0)
                    {
                        intPolicyType = Convert.ToInt32(DsPolicyRisks.Tables[0].Rows[0]["POLICY_TYPE"].ToString());

                        //Si la póliza es de tipo colectiva con un solo riesgo, ésta
                        //se debe imprimir como tipo individual.
                        if ((intPolicyType == (int)(ReportEnum.PolicyType.COLLECTIVE)) && (RiskCount == 1))
                        {
                            intPolicyType = (int)(ReportEnum.PolicyType.INDIVIDUAL);
                        }

                    }

                    if (RiskCount > 1)
                    {
                        foreach (DataRow row in DsPolicyRisks.Tables[0].Rows)
                        {
                            int intRiskStatus = 0;
                            intRiskStatus = Convert.ToInt32(row["RISK_STATUS_CD"].ToString());

                            if (intRiskStatus == (int)ReportEnum.RiskStatus.EXCLUDED) //Riesgo Excluido
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
                        case ((int)ReportEnum.PrefixCode.AUTOS):
                            FilePath = printAutos(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                        case ((int)ReportEnum.PrefixCode.CROP):
                            FilePath = printRC(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                        case ((int)ReportEnum.PrefixCode.COMPLIANCE):
                            FilePath = printCompliance(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                        case ((int)ReportEnum.PrefixCode.LOCATION):
                            ///paramsp[4] = new NameValue("RISK_NUM", Convert.ToInt32(1));
                            FilePath = printLocation(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;

                        // <<TODO: Autor: Miguel López; Fecha: 20/08/2010; Asunto: Incluimos los ramos RC Pasajeros
                        //                                                         y Caución en la selección.
                        case ((int)ReportEnum.PrefixCode.PASSENGER_CROP):
                            FilePath = printPassenger(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                        case ((int)ReportEnum.PrefixCode.CAUTION):
                            FilePath = printCaution(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;

                        /* Autor: Miguel López; Fecha: 20/08/2010 >>*/

                        //TODO:  <<Autor: Nicolas Gonzalez R. - NGR ; Fecha: 12/11/2015; 
                        //Asunto: Se agrega funcionalidad para el ramo de arrendamiento
                        case ((int)ReportEnum.PrefixCode.LEASE):
                            FilePath = printCompliance(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                            break;
                            // Autor: Nicolas Gonzalez R. - NGR, Fecha: 12/11/2015 >>
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
            intPolicyType = (int)ReportEnum.PolicyType.INDIVIDUAL;
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
                case ((int)ReportEnum.PrefixCode.AUTOS):
                    FilePath = printAutos(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                case ((int)ReportEnum.PrefixCode.CROP):
                    FilePath = printRC(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                case ((int)ReportEnum.PrefixCode.COMPLIANCE):
                    FilePath = printCompliance(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
            }

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

                if ((Convert.ToInt32(policyData.ReportType) == (int)ReportEnum.ReportType.COMPLETE_REQUEST) ||
                    (Convert.ToInt32(policyData.ReportType) == (int)ReportEnum.ReportType.ONLY_REQUEST))
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

                    if (Convert.ToInt32(policyData.ReportType) == (int)ReportEnum.ReportType.ONLY_REQUEST)
                    {
                        ReportServiceHelper.joinPdfFiles(rutas);

                        //Si la opción seleccionada es "Solo la Solicitud Agrupadora" se envía la ruta sin pólizas
                        FilePath = ((Paths)rutas[0]).FilePath;
                    }
                }

                //Si la opción elegida es Solicitud Agrupadora y las pólizas relacionadas.            
                if ((Convert.ToInt32(policyData.ReportType) == (int)ReportEnum.ReportType.COMPLETE_REQUEST) ||
                    (Convert.ToInt32(policyData.ReportType) == (int)ReportEnum.ReportType.ONLY_POLICIES_REQUEST))
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
        /// Generación de ruta de archivo de impresión de cada una de las pólizas 
        /// incluídas en cargue masivo
        /// </summary>
        public void printMassiveLoadPoliciesInd()
        {
            int intPolicyType = 0;
            intPolicyType = (int)ReportEnum.PolicyType.INDIVIDUAL;
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
            // <<TODO: Edgar Piraneque; Compañía: 1; Asunto: Ajuste impresión masivas RC
            ///Int32 printRCPassenger = 0;
            if (Convert.ToInt32(PolicyData.PrefixNum) == (int)ReportEnum.PrefixCode.CROP)
            {
                if (IsRcPassenger(PolicyData, (int)ReportEnum.PrefixPolicyType.RC_PASSENGER))
                {
                    PolicyData.PrefixNum = ((int)ReportEnum.PrefixCode.PASSENGER_CROP);
                }
            }
            // Edgar Piraneque; Compañía: 1; Asunto: Ajuste impresión masivas RC >>
            switch (Convert.ToInt32(PolicyData.PrefixNum)) //PrefixNum
            {
                case ((int)ReportEnum.PrefixCode.AUTOS):
                    FilePath = printAutos(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                case ((int)ReportEnum.PrefixCode.CROP):
                    FilePath = printRC(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                case ((int)ReportEnum.PrefixCode.COMPLIANCE):
                    FilePath = printCompliance(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;

                // << TODO: Autor: Miguel López; Fecha: 20/08/2010; Asunto: Tomamos en cuenta los ramos
                //                                                          Caución y RC Pasajeros
                case ((int)ReportEnum.PrefixCode.PASSENGER_CROP):
                    FilePath = printPassenger(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;

                case ((int)ReportEnum.PrefixCode.CAUTION):
                    FilePath = printCaution(intPolicyType, PolicyData, paramsp, DsPolicyRisks);
                    break;
                    /* Autor: Miguel López; Fecha: 20/08/2010*/
            }

            this.Status = true;
            //return FilePath;
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
            string ReportPath = String.Empty;
            ReportPath = getIndividualPolicyReport(PolicyData, paramsp);
            return ReportPath;
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
        /// Realiza impresión del ramo Multiesgo hogar
        /// </summary>
        /// <param name="intPolicyType"></param>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <param name="dsPolicyRisks"></param>
        /// <returns></returns>
        public string printLocation(int intPolicyType, Policy policyData, NameValue[] paramsp, DataSet dsPolicyRisks)
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
            string ReportPath = getIndividualPolicyReport(policyData, paramsp);
            return ReportPath;
        }


        // << TODO: Autor: Miguel López; Fecha: 20/08/2010; Asunto: Métodos generales para impresión de ramos
        //                                                          RC Pasajeros y Caución
        /// <summary>
        /// Realiza impresión del ramo RC Pasajeros
        /// </summary>
        /// <param name="intPolicyType"></param>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <param name="dsPolicyRisks"></param>
        /// <returns></returns>
        public string printPassenger(int intPolicyType, Policy policyData, NameValue[] paramsp, DataSet dsPolicyRisks)
        {
            string ReportPath = getIndividualPolicyReport(policyData, paramsp);
            return ReportPath;
        }

        /// <summary>
        /// Realiza impresión del ramo Caución
        /// </summary>
        /// <param name="intPolicyType"></param>
        /// <param name="policyData"></param>
        /// <param name="paramsp"></param>
        /// <param name="dsPolicyRisks"></param>
        /// <returns></returns>
        public string printCaution(int intPolicyType, Policy policyData, NameValue[] paramsp, DataSet dsPolicyRisks)
        {
            this.PolicyType = intPolicyType;
            string ReportPath = getIndividualPolicyReport(policyData, paramsp);
            return ReportPath;
        }
        /* Autor: Miguel López; Fecha: 20/08/2010*/

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

        /// <summary>
        /// Impresión de pólizas incluídas en cargue masivo
        /// </summary>
        /// <returns>Ruta del reporte generado</returns>
        public void printMassiveLoadPolicies()
        {
            try
            {
                //Almacena los parametros del SP que llena tabla de reportes.
                NameValue[] requestParam = new NameValue[1];

                //Almacena todas las pólizas de la solicitud agrupadora.
                DataSet dsMassLoadPolicies;

                Policy policyData = new Policy(dsOutput.Tables["PolicyPrinting"].Rows[0]);

                //Se asignan los valores de los parametros para el reporte de Solicitud Agrupadora.
                requestParam[0] = new NameValue("MASSLOAD_ID", policyData.PolicyId);

                policyData.CodeBar = policyData.PrefixNum.ToString() + policyData.PolicyId.ToString() + policyData.EndorsementId.ToString();

                //SP que llena las tablas para Solicitud Agrupadora.
                dsMassLoadPolicies = ReportServiceHelper.getData("REPORT.CO_MASSIVE_LOAD_POLICIES", requestParam);

                ArrayList rutasdef = (ArrayList)rutas.Clone();
                Paths final = new Paths(rutas.Count);
                final.setFileName(policyData, ".pdf");
                rutasdef.Add(final);
                int filesPrint = dsMassLoadPolicies.Tables[0].Rows.Count + 1;
                string[] rutasPrint = new string[filesPrint];
                rutasPrint[0] = final.FilePath;

                if (dsMassLoadPolicies.Tables[0].Rows.Count > 0)
                {
                    //estructura que debe ser cargada para cada póliza
                    DataSet dsPolicies = dsOutput.Clone();
                    int riskAnt = 0;
                    //ArrayList rutasdef = (ArrayList)rutas.Clone(); 
                    int i = 0;
                    foreach (DataRow dr in dsMassLoadPolicies.Tables[0].Rows)
                    {
                        i++;
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
                        Paths procPolicy = new Paths();
                        procPolicy.setFileName(PolicyData, ".pdf");
                        printMassiveLoadPoliciesInd();
                        rutasdef.Add(procPolicy);
                        rutas.Clear();
                    }
                    ReportServiceHelper.joinPdfFiles(rutasdef);
                    FilePath = final.FilePath;
                    FileName = final.getFileName(final.FilePath);
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Ocurrio un error al generar el reporte de CARGUE MASIVO: " + Ex.Message);
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



        public void printCollectivePolicies()
        {
            // <<TODO: Autor: Edgar Piraneque; Compañía: 1; 21/12/2010; Genera reporte de pólizas de vehículos con sustitución   
            NameValue[] paramsp = new NameValue[9];
            paramsp[0] = new NameValue("PROCESS_ID", Convert.ToInt32(PolicyData.ProcessId));
            paramsp[1] = new NameValue("POLICY_ID", Convert.ToInt32(PolicyData.PolicyId));
            paramsp[2] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(PolicyData.EndorsementId));
            paramsp[3] = new NameValue("TEMP_ID", Convert.ToInt32(PolicyData.TempNum));
            paramsp[4] = new NameValue("RISK_NUM", Convert.ToInt32(PolicyData.RangeMaxValue));
            paramsp[5] = new NameValue("FIRST_RISK", Convert.ToInt32(PolicyData.RangeMinValue));
            paramsp[6] = new NameValue("LAST_RISK", Convert.ToInt32(PolicyData.RangeMaxValue));
            paramsp[7] = new NameValue("POLICY_TYPE", (int)ReportEnum.PolicyType.COLLECTIVE);
            paramsp[8] = new NameValue("RISK_NUM_ANT", null);

            showCollectivePolicyCoverReport(PolicyData, paramsp);

            ReportServiceHelper.joinPdfFiles(rutas);
            FilePath = ((Paths)rutas[0]).FilePath;
            // Autor: Edgar Piraneque; Compañía: 1; 21/12/2010;>>    
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

            policyData.CodeBar = policyData.PrefixNum.ToString() + policyData.PolicyId.ToString() + policyData.EndorsementId.ToString();

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
            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);

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
                ReportServiceHelper.loadReportFile(strReportParametersCnv, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);
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
            int intReportType = Convert.ToInt32(policyData.ReportType);
            if (intReportType == (int)ReportEnum.ReportType.MASS_LOAD)
            {
                intReportType = (int)ReportEnum.ReportType.ONLY_POLICY;
            }

            if (RiskCount == 1)
            {
                Paths final = new Paths(rutas.Count);
                final.setFileName(policyData, ".pdf");
                rutas.Add(final);
            }

            int intLines = vc.CoveragesCount + vc.AccesoriesCount;
            int intTotalLines = Convert.ToInt32(ConfigurationSettings.AppSettings["CountCoverAccesories"]);

            if (policyData.TempNum == 0)//Marca de agua: Temporarios o Cotizaciones.
            {
                waterMark = string.Empty;
            }
            else
            {
                if (policyData.QuotationId != 0)
                {
                    waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
                }
                else
                {
                    waterMark = ConfigurationSettings.AppSettings["WaterMark"];
                }
            }

            policyData.CodeBar = policyData.PrefixNum.ToString() + policyData.PolicyId.ToString() + policyData.EndorsementId.ToString();

            //Parametros reportes Automoviles
            string[] strReportParameters = new string[10];
            strReportParameters[0] = PolicyData.ProcessId.ToString();
            strReportParameters[1] = vc.MinRow.ToString();
            strReportParameters[2] = PolicyData.CodeBar;
            strReportParameters[3] = PolicyData.TempNum.ToString();
            strReportParameters[4] = (intRiskNum != 0) ? intRiskNum.ToString() : policyData.EndorsementId.ToString();
            strReportParameters[5] = policyData.EndorsementId.ToString();

            if ((intRiskNum == 0) && (intReportType != (int)ReportEnum.ReportType.COMPLETE_REQUEST))
            {
                intRiskNum = 1;
            }


            //Para Impresión de riesgo de exclusión.
            //int intReportType = (intRiskType != 3) ? Convert.ToInt32(policyData.ReportType) : 2;

            //Rutas de generación de reporte
            string[] strPaths = new string[2];

            try
            {
                //Evalua tipo de reporte
                if (intReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT)
                {
                    ///HD-2317 04/05/2010
                    ///Autor: Edgar O. Piraneque E.
                    ///Descripción: Se ajusta validación cuando se imprime un endoso cuyo número de riesgo es diferente a 1
                    ///para que se imprima correctamente la póliza y determinar si el valor de la prima es positivo

                    if (vc.PrintFormat == 1)
                    {
                        if (policyData.RangeMaxValue > 1)
                        {
                            if ((RiskCount == 1) && (policyData.RangeMinValue == policyData.RangeMaxValue))
                            {
                                strReportParameters[4] = policyData.RangeMaxValue.ToString();
                            }
                            else if (policyData.RangeMaxValue == 1)
                            {
                                strReportParameters[4] = "1";
                            }
                            else
                            {
                                strReportParameters[4] = "0";
                            }
                        }
                        else
                        {
                            strReportParameters[4] = "1";
                        }
                        //Autor: German Silva; Jira: 156: Fecha: 24/07/2015
                        addFormatCollect(strReportParameters, policyData);
                    }
                    ///FIN HD-2317 *************************************************
                }
                else if ((intReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                    (intReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                    (PolicyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_REQUEST))
                {
                    if (vc.RowCount > 5)
                    {
                        //Valida si debe mostrar el convenio o no.
                        if ((intReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                            (intReportType == (int)ReportEnum.ReportType.COMPLETE_REQUEST))
                        {
                            Paths hj1Vehicle = new Paths(rutas.Count);
                            hj1Vehicle.setFileName(policyData, ".pdf");
                            rutas.Add(hj1Vehicle);

                            strPaths[0] = hj1Vehicle.getPath("VehicleCover", (RiskCount > 1));
                            strPaths[1] = hj1Vehicle.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            if (vc.printSecondPage(Version))
                            {
                                Paths hj2Vehicle = new Paths(rutas.Count);
                                hj2Vehicle.setFileName(policyData, ".pdf");
                                rutas.Add(hj2Vehicle);

                                strPaths[0] = hj2Vehicle.getPath("VehicleCoverAppendix", (RiskCount > 1));
                                strPaths[1] = hj2Vehicle.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                //<< TODO: Edgar O. Piraneque E.; Compañía: 1; 25/10/2010; HD-2613
                                printConditionTextAuto(policyData, vc, strReportParameters, strPaths);
                                //FIN: Edgar O. Piraneque E.; Compañía: 1; 25/10/2010 >>
                                if (HasClauses == false)
                                {
                                    HasClauses = true;
                                }
                            }
                            ///eopiraneque filtro para imprimir solo un plan de pagos cuando es una póliza colectiva
                            if (intRiskNum == RiskCount)
                            {
                                ///HD-2181
                                if (vc.PrintConvection == 1)
                                {
                                    Paths hj3Vehicle = new Paths(rutas.Count);
                                    hj3Vehicle.setFileName(policyData, ".pdf");
                                    rutas.Add(hj3Vehicle);

                                    strPaths[0] = hj3Vehicle.getPath("VehicleConvection", (RiskCount > 1));
                                    strPaths[1] = hj3Vehicle.FilePath;

                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                }
                            }
                            ///eopiraneque

                            if ((policyData.WithFormatCollect) && (intRiskNum == RiskCount) && (policyData.IdPv2g > 0))
                            {
                                ///HD-2317 04/05/2010
                                ///Autor: Edgar O. Piraneque E.
                                ///Descripción: Se ajusta validación cuando se imprime un endoso cuyo número de riesgo es diferente a 1
                                ///para que se imprima correctamente la póliza y determinar si el valor de la prima es positivo
                                if (vc.PrintFormat == 1)
                                {
                                    if (policyData.RangeMaxValue > 1)
                                    {
                                        if ((RiskCount == 1) && (policyData.RangeMinValue == policyData.RangeMaxValue))
                                        {
                                            strReportParameters[4] = policyData.RangeMaxValue.ToString();
                                        }
                                        else if (policyData.RangeMaxValue == 1)
                                        {
                                            strReportParameters[4] = "1";
                                        }
                                        else
                                        {
                                            strReportParameters[4] = "0";
                                        }
                                    }
                                    else
                                    {
                                        strReportParameters[4] = "1";
                                    }
                                    addFormatCollect(strReportParameters, policyData);
                                }
                                ///FIN HD-2317 ************************************************
                            }
                        }
                        else if ((intReportType == (int)ReportEnum.ReportType.ONLY_POLICY))
                        {
                            Paths hj1Vehicle = new Paths(rutas.Count);
                            hj1Vehicle.setFileName(policyData, ".pdf");
                            rutas.Add(hj1Vehicle);

                            strPaths[0] = hj1Vehicle.getPath("VehicleCover", (RiskCount > 1));
                            strPaths[1] = hj1Vehicle.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            if (vc.printSecondPage(Version))
                            {
                                Paths hj5Vehicle = new Paths(rutas.Count);
                                hj5Vehicle.setFileName(policyData, ".pdf");
                                rutas.Add(hj5Vehicle);

                                strPaths[0] = hj5Vehicle.getPath("VehicleCoverAppendix", (RiskCount > 1));
                                strPaths[1] = hj5Vehicle.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                //<< TODO: Edgar O. Piraneque E.; Compañía: 1; 25/10/2010; HD-2613
                                printConditionTextAuto(policyData, vc, strReportParameters, strPaths);
                                //FIN: Edgar O. Piraneque E.; Compañía: 1; 25/10/2010 >>
                                if (policyData.WithFormatCollect)
                                {
                                    ///HD-2317 04/05/2010
                                    ///Autor: Edgar O. Piraneque E.
                                    ///Descripción: Se ajusta validación cuando se imprime un endoso cuyo número de riesgo es diferente a 1
                                    ///para que se imprima correctamente la póliza y determinar si el valor de la prima es positivo

                                    if ((vc.PrintFormat == 1) && (policyData.IdPv2g > 0))
                                    {
                                        if (policyData.RangeMaxValue > 1)
                                        {
                                            if ((RiskCount == 1) && (policyData.RangeMinValue == policyData.RangeMaxValue))
                                            {
                                                strReportParameters[4] = policyData.RangeMaxValue.ToString();
                                            }
                                            else if (policyData.RangeMaxValue == 1)
                                            {
                                                strReportParameters[4] = "1";
                                            }
                                            else
                                            {
                                                strReportParameters[4] = "0";
                                            }
                                        }
                                        else
                                        {
                                            strReportParameters[4] = "1";
                                        }
                                        addFormatCollect(strReportParameters, policyData);
                                    }
                                    ///FIN HD-2317 ************************************************
                                }
                            }
                            /// eopiraneque Se incluye modificación para que imprima el formato de recaudo

                            else if ((policyData.WithFormatCollect) && (intRiskNum == RiskCount))
                            {
                                ///HD-2317 04/05/2010
                                ///Autor: Edgar O. Piraneque E.
                                ///Descripción: Se ajusta validación cuando se imprime un endoso cuyo número de riesgo es diferente a 1
                                ///para que se imprima correctamente la póliza y determinar si el valor de la prima es positivo
                                if ((vc.PrintFormat == 1) && (policyData.IdPv2g > 0))
                                {
                                    if (policyData.RangeMaxValue > 1)
                                    {
                                        if ((RiskCount == 1) && (policyData.RangeMinValue == policyData.RangeMaxValue))
                                        {
                                            strReportParameters[4] = policyData.RangeMaxValue.ToString();
                                        }
                                        else if (policyData.RangeMaxValue == 1)
                                        {
                                            strReportParameters[4] = "1";
                                        }
                                        else
                                        {
                                            strReportParameters[4] = "0";
                                        }
                                    }
                                    else
                                    {
                                        strReportParameters[4] = "1";
                                    }
                                    addFormatCollect(strReportParameters, policyData);
                                }
                                ///FIN HD-2317 ************************************************
                            }
                            /// eopiraneque
                        }

                    }
                }
                else if ((intReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION) && (intRiskNum == RiskCount))
                {
                    ///HD-2181
                    if (vc.PrintConvection == 1)
                    {
                        Paths hj7Vehicle = new Paths(rutas.Count);
                        hj7Vehicle.setFileName(policyData, ".pdf");
                        rutas.Add(hj7Vehicle);

                        strPaths[0] = hj7Vehicle.getPath("VehicleConvection", (RiskCount > 1));
                        strPaths[1] = hj7Vehicle.FilePath;

                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    }
                    if (intRiskNum == RiskCount)
                    {
                        if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                        {
                            ///HD-2317 04/05/2010
                            ///Autor: Edgar O. Piraneque E.
                            ///Descripción: Se ajusta validación cuando se imprime un endoso cuyo número de riesgo es diferente a 1
                            ///para que se imprima correctamente la póliza y determinar si el valor de la prima es positivo
                            if (vc.PrintFormat == 1)
                            {
                                if (policyData.RangeMaxValue > 1)
                                {
                                    if ((RiskCount == 1) && (policyData.RangeMinValue == policyData.RangeMaxValue))
                                    {
                                        strReportParameters[4] = policyData.RangeMaxValue.ToString();
                                    }
                                    else if (policyData.RangeMaxValue == 1)
                                    {
                                        strReportParameters[4] = "1";
                                    }
                                    else
                                    {
                                        strReportParameters[4] = "0";
                                    }
                                }
                                else
                                {
                                    strReportParameters[4] = "1";
                                }
                                addFormatCollect(strReportParameters, policyData);
                            }
                            ///FIN HD-2317 ************************************************
                        }
                    }
                }
                else if ((intReportType == (int)ReportEnum.ReportType.QUOTATION))
                {
                    //**********************************************
                    //Edgar Piraneque: 08/03/2010
                    //Se suprime la impresión del plan de pagos para cotización según HD-2139
                    //**********************************************
                    //Paths hj1Vehicle = new Paths(rutas.Count);
                    //hj1Vehicle.setFileName(policyData, ".pdf");
                    //rutas.Add(hj1Vehicle);

                    //strPaths[0] = hj1Vehicle.getPath("VehicleQuotationCover", (RiskCount > 1));
                    //strPaths[1] = hj1Vehicle.FilePath;

                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    Paths hj1Vehicle = new Paths(rutas.Count);
                    hj1Vehicle.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Vehicle);

                    strPaths[0] = hj1Vehicle.getPath("VehicleQuotationCover", (RiskCount > 1));
                    strPaths[1] = hj1Vehicle.FilePath;

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    if (vc.printSecondPage(Version))
                    {
                        Paths hj2Vehicle = new Paths(rutas.Count);
                        hj2Vehicle.setFileName(policyData, ".pdf");
                        rutas.Add(hj2Vehicle);

                        strPaths[0] = hj2Vehicle.getPath("VehicleQuotationAppendix", (RiskCount > 1));
                        strPaths[1] = hj2Vehicle.FilePath;

                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    }
                    ///HD-2139 Autor: Edgar O. Piraneque
                    ///Descripción: No se debe imprimir el plan de pagos para cotizaciones

                }
                else if (intReportType == (int)ReportEnum.ReportType.TEMPORARY)
                {
                    //eopiraneque 12-03-2010: Se ajusta generación de reporte de temporarios
                    //de sustitución para que imprima solo las cláusulas del riesgo incluído
                    Paths hj1Vehicle = new Paths(rutas.Count);
                    hj1Vehicle.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Vehicle);

                    strPaths[0] = hj1Vehicle.getPath("VehicleCover", (RiskCount > 1));
                    strPaths[1] = hj1Vehicle.FilePath;

                    // <<TODO: Autor: JDavila; Fecha: 10/12/2010; Asunto: OT0051 Renovación Masiva; Compañía: 1 
                    waterMark = (vc.IsPrintTmpRenewal) ? "" : waterMark;
                    // TODO: Autor: JDavila; Fecha: 10/12/2010; >>

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    if (vc.printSecondPage(Version))
                    {
                        Paths hj2Vehicle = new Paths(rutas.Count);
                        hj2Vehicle.setFileName(policyData, ".pdf");
                        rutas.Add(hj2Vehicle);

                        strPaths[0] = hj2Vehicle.getPath("VehicleCoverAppendix", (RiskCount > 1));
                        strPaths[1] = hj2Vehicle.FilePath;

                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        //<< TODO: Edgar O. Piraneque E.; Compañía: 1; 25/10/2010; HD-2613
                        printConditionTextAuto(policyData, vc, strReportParameters, strPaths);
                        //FIN: Edgar O. Piraneque E.; Compañía: 1; 25/10/2010 >>
                        if (HasClauses == false)
                        {
                            HasClauses = true;
                        }
                    }

                    //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 25/03/2011; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1
                    //Se agrega inpresion de Formato de Recaudo y COnvenio de pagos a la impresion masiva de temporarios.
                    if (vc.IsPrintTmpRenewal)
                    {
                        if (intRiskNum == RiskCount)
                        {
                            if (vc.PrintConvection == 1)
                            {
                                Paths hj3Vehicle = new Paths(rutas.Count);
                                hj3Vehicle.setFileName(policyData, ".pdf");
                                rutas.Add(hj3Vehicle);

                                strPaths[0] = hj3Vehicle.getPath("VehicleConvection", (RiskCount > 1));
                                strPaths[1] = hj3Vehicle.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            }
                        }
                        if (vc.PrintFormat == 1)
                        {
                            if (policyData.RangeMaxValue > 1)
                            {
                                if ((RiskCount == 1) && (policyData.RangeMinValue == policyData.RangeMaxValue))
                                {
                                    strReportParameters[4] = policyData.RangeMaxValue.ToString();
                                }
                                else if (policyData.RangeMaxValue == 1)
                                {
                                    strReportParameters[4] = "1";
                                }
                                else
                                {
                                    strReportParameters[4] = "0";
                                }
                            }
                            else
                            {
                                strReportParameters[4] = "1";
                            }
                            addFormatCollect(strReportParameters, policyData);

                        }

                    }
                    /* Autor: Luisa Fernanda Ramírez, Fecha: 25/03/2011>>*/

                    ///eopiraneque: 
                    ///HD-2109 se retira filtro para no imprimir plan de pagos
                    //if (intRiskNum == RiskCount)
                    //{
                    //    ///HD-2181
                    //    if (vc.PrintConvection == 1)
                    //    {
                    //        Paths hj3Vehicle = new Paths(rutas.Count);
                    //        hj3Vehicle.setFileName(policyData, ".pdf");
                    //        rutas.Add(hj3Vehicle);

                    //        strPaths[0] = hj3Vehicle.getPath("VehicleConvection", (RiskCount > 1));
                    //        strPaths[1] = hj3Vehicle.FilePath;

                    //        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    //    }
                    //}
                }
                if ((RiskCount == 2) && (HasClauses) && (intRiskNum == RiskCount))
                {
                    int i = 0;
                    string[] finalPdfPaths = new string[rutas.Count];

                    foreach (Paths ruta in rutas)
                    {
                        finalPdfPaths[i] = ruta.FilePath;
                        i++;
                    }
                    int rutasTemp = rutas.Count;

                    for (i = 0; i < rutasTemp; i++)
                    {
                        ((Paths)rutas[i]).FilePath = " ";
                        if (i <= 1)
                        {
                            FilePath = finalPdfPaths[i].ToString();
                        }
                        else if (i < (rutasTemp - 2))
                        {
                            FilePath = finalPdfPaths[i + 1].ToString();
                        }
                        else if (i == (rutasTemp - 2))
                        {
                            FilePath = finalPdfPaths[i - 2].ToString();
                        }
                        else
                        {
                            FilePath = finalPdfPaths[rutasTemp - 1];

                        }
                        ((Paths)rutas[i]).FilePath = FilePath;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Genera los reportes para el ramo de reponsabilidad civil y los exporta a .pdf.
        /// </summary>
        /// <param name="rc">Datos del reporte de RC.</param>
        /// <param name="policyData">Datos de la póliza</param>
        private void showCropReport(Crop rc, Policy policyData)
        {
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);
            //Para saber si la marca de agua debe ser de Temporarios o de Cotizaciones
            if (policyData.TempNum == 0)
            {
                waterMark = "";
            }
            else
            {
                if (policyData.QuotationId > 0)
                {
                    waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
                }
                else
                {
                    waterMark = (policyData.TempNum == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];
                }
            }

            //policyData.CodeBar = policyData.PrefixNum.ToString() + policyData.PolicyId.ToString() + policyData.EndorsementId.ToString();

            //Parametros reportes Responsabilidad civil

            //desarrollo de generacion de codigo de barras
            BarcodeGenerator Barcode = new BarcodeGenerator();

            String pathBarCodeImage = String.Empty;
            String dataBarCode = String.Empty;
            String fileBarCode = String.Empty;
            String pathBarCode = ConfigurationSettings.AppSettings["pathBarCode"].ToString();

            fileBarCode = policyData.PrefixNum.ToString() +
                        policyData.LicensePlate.ToString() +
                        policyData.EndorsementId.ToString() + ".png";
            pathBarCodeImage = pathBarCode + fileBarCode;

            dataBarCode = policyData.PrefixNum.ToString().PadLeft(4, '0') + ";" + policyData.LicensePlate.ToString().PadLeft(6, '0') + ";" + policyData.EndorsementId.ToString().PadLeft(10, '0');

            Barcode.createBarCodeGS1Image(dataBarCode, pathBarCodeImage);
            //hasta aqui el desarrollo de codigo de barras

            policyData.CodeBar = pathBarCodeImage;

            string[] strReportParameters = new string[10];

            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = rc.SecondPage.ToString();
            strReportParameters[2] = policyData.CodeBar;
            strReportParameters[3] = policyData.TempNum.ToString();
            strReportParameters[4] = rc.RegisterCount.ToString();
            strReportParameters[5] = rc.BeneficiariesCount.ToString();
            strReportParameters[8] = ((int)GetPolicyCoInsurance(policyData.TempNum, policyData.PolicyId, policyData.EndorsementId)).ToString() ?? "0";

            string[] strPaths = new string[3];//Rutas de generación de reporte

            try
            {
                if (policyData.ReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT)
                {
                    // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g
                    if ((rc.PrintFormat == 1) && (policyData.BalancePremium > 0))
                    {
                        // Edgar O. Piraneque E.; 05/11/2010;>>
                        strReportParameters[4] = RiskCount.ToString();
                        addFormatCollect(strReportParameters, policyData);
                    }
                    // Edgar O. Piraneque E.; 05/11/2010;>>
                }
                //Se valida que tipo de reporte se seleccionó.
                else if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                {
                    /* Se valida si existe texto a mostrar en la segunda hoja o si la cantidad
                     * es amparos es mayor o igual a 6. Cuando la cantidad de amparos es mayor o 
                     * igual a 6, en la primera hoja se alcanza a mostrar toda la información.*/

                    //TODO: Fecha:26/10/2011 ;Autor: John Ruiz; Asunto:Se recuperan textos para imprimir objeto de la poliza y anexos si es nesesario ; Compañia: 3
                    List<string> policyObject = GetTextPages(policyData, 3, 2, 97, true);

                    if (policyData.PolicyId > 0 && policyData.EndorsementId > 0 && policyData.CurrentFromFirst)
                    {
                        CurrentoFromFirstEndorsement(policyData);
                    }

                    Paths hj1Crop = new Paths(rutas.Count);
                    hj1Crop.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Crop);

                    if (policyData.TempAuthorization)
                    {
                        strPaths[0] = hj1Crop.getPath("CropCoverAutho", (RiskCount > 1));
                    }
                    else
                    {
                        strPaths[0] = hj1Crop.getPath("CropCover", (RiskCount > 1));
                    }
                    strPaths[1] = hj1Crop.FilePath;
                    strReportParameters[6] = policyObject[0].Trim();
                    strReportParameters[7] = policyObject[1];


                    if (policyData.TempAuthorization)
                    {
                        strReportParameters[9] = string.Empty;
                    }

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    //if (GetNumberDeductibles(policyData.TempNum, policyData.PolicyId, policyData.EndorsementId) > 2)
                    //{
                    //    Paths deductibleCrop = new Paths(rutas.Count);
                    //    deductibleCrop.setFileName(policyData, ".pdf");
                    //    rutas.Add(deductibleCrop);

                    //    strPaths[0] = deductibleCrop.getPath("CropCoverAppendixDeductible", (RiskCount > 1));
                    //    strPaths[1] = deductibleCrop.FilePath;


                    //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    //}

                    if ((policyObject.Count > 0) || (rc.CoveragesCount >= 6))
                    {

                        /* En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
                         * Se valida si muestra debe mostrar el convenio o no.*/
                        if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                            (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                            (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                        {
                            int policyObjectCount = 0;
                            if (policyData.TempAuthorization)
                            {
                                policyObjectCount = 2;
                            }
                            else
                            {
                                policyObjectCount = 1;
                            }
                            if (policyObject.Count > policyObjectCount && policyObject.Where(x => !x.Equals(string.Empty)).Count() > 0)
                            {
                                List<string> listText = new List<string>();
                                if (policyObject[policyObjectCount].Length > 62650)
                                {
                                    listText = GetTextMaximumCharacter(policyObject[policyObjectCount]);
                                }
                                if (listText.Count > 0)
                                {
                                    foreach (var text in listText)
                                    {
                                        Paths hj3Crop = new Paths(rutas.Count);
                                        hj3Crop.setFileName(policyData, ".pdf");
                                        rutas.Add(hj3Crop);

                                        strPaths[0] = hj3Crop.getPath("CropCoverAppendix", (RiskCount > 1));
                                        strPaths[1] = hj3Crop.FilePath;

                                        strReportParameters[6] = text;

                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                    }
                                }
                                else
                                {
                                    Paths hj3Crop = new Paths(rutas.Count);
                                    hj3Crop.setFileName(policyData, ".pdf");
                                    rutas.Add(hj3Crop);

                                    strPaths[0] = hj3Crop.getPath("CropCoverAppendix", (RiskCount > 1));
                                    strPaths[1] = hj3Crop.FilePath;

                                    strReportParameters[6] = policyObject.Count > 0 ? policyObject[policyObjectCount] : string.Empty;

                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                }
                            }

                            ///* Reporte Convenio de Pago. Se carga la ruta del reporte a cargar.*/
                            /////HD-2181
                            if (rc.PrintConvection == 1 && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                            {
                                // << TODO: Autor: Miguel López; Fecha: 07/03/2011; Asunto: HD-2872

                                Paths hj2Crop = new Paths(rutas.Count);
                                hj2Crop.setFileName(policyData, ".pdf");

                                // << TODO: Autor: Manuel E Gomez; Fecha: 26/12/2013; Asunto: HD-1113 se elimina el convenio de primas
                                //en la impresión de polizas RC
                                // << TODO: Autor: German Silva; Fecha: 22/07/2015; Asunto: JIRA-156 se Habilita el convenio de primas para RC

                                //if ((policyData.PrefixNum != 15) && (HasCoinsuranceAssigned(policyData) == false))
                                //{
                                rutas.Add(hj2Crop);
                                strPaths[0] = hj2Crop.getPath("CropConvection", (RiskCount > 1));
                                strPaths[1] = hj2Crop.FilePath;
                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                //}

                                /* Autor: German Silva; Fecha: 22/07/2015*/
                                /* Autor: Manuel E Gómez; Fecha: 26/12/2013*/
                                /* Autor: Miguel López; Fecha: 07/03/2011*/
                            }
                            ////intPdfQuantity++;

                            if (policyData.TempAuthorization && ValidateCountTempAuthorization(policyData.TempNum))
                            {
                                Paths hj3Crop = new Paths(rutas.Count);
                                hj3Crop.setFileName(policyData, ".pdf");
                                rutas.Add(hj3Crop);

                                strPaths[0] = hj3Crop.getPath("AuthoAppendix", (RiskCount > 1));
                                strPaths[1] = hj3Crop.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            }
                        }
                        else if (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY)
                        {
                            if (policyObject.Count > 2)
                            {
                                Paths hj3Crop = new Paths(rutas.Count);
                                hj3Crop.setFileName(policyData, ".pdf");
                                rutas.Add(hj3Crop);

                                strPaths[0] = hj3Crop.getPath("CropCoverAppendix", (RiskCount > 1));
                                strPaths[1] = hj3Crop.FilePath;

                                strReportParameters[6] = policyObject.Count > 0 ? policyObject[2] : string.Empty;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            }
                        }
                    }

                    //TODO: Fecha:02/11/2011 ;Autor: John Ruiz; Asunto: Se imprime forma de producto ; Compañia: 3
                    if (isExpedition(policyData.EndorsementId))
                    {
                        string FormName = GetFormName(policyData.PolicyId);

                        if (!FormName.Equals(string.Empty))
                        {
                            string FormPdfFile = ConfigurationSettings.AppSettings["FormsPath"].ToString() + FormName + ".pdf";
                            if (System.IO.File.Exists(FormPdfFile))
                            {
                                Paths form = new Paths(rutas.Count);
                                form.setFileName(policyData, ".pdf");
                                rutas.Add(form);

                                System.IO.File.Copy(FormPdfFile, form.FilePath);
                            }
                        }
                    }

                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
                        ///HD-2333 07/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if ((rc.PrintFormat == 1) && (policyData.BalancePremium > 0))
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2333 ************************************************
                        // Edgar O. Piraneque E.; 05/11/2010;>>
                    }
                }
                else if (policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION)
                {
                    //Reporte Convenio de Pago.
                    ///HD-2181
                    if (rc.PrintConvection == 1 && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                    {
                        // << TODO: Autor: Miguel López; Fecha: 07/03/2011; Asunto: HD-2872

                        Paths hj6Crop = new Paths(rutas.Count);
                        hj6Crop.setFileName(policyData, ".pdf");
                        rutas.Add(hj6Crop);

                        strPaths[0] = hj6Crop.getPath("CropConvection", (RiskCount > 1));//strArrPaths[0] + ConfigurationSettings.AppSettings["VehicleConvectionFile"];
                        strPaths[1] = hj6Crop.FilePath;//strArrPaths[intPathIndex];


                        //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        /* Autor: Miguel López; Fecha: 07/03/2011 */
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2333 07/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if ((rc.PrintFormat == 1) && (policyData.BalancePremium > 0))
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2333 ************************************************
                    }

                }
                //else if ((policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                //{
                //    Paths hj1Crop = new Paths(rutas.Count);
                //    hj1Crop.setFileName(policyData, ".pdf");
                //    rutas.Add(hj1Crop);

                //    strPaths[0] = hj1Crop.getPath("CropQuotationCover", (RiskCount > 1));
                //    strPaths[1] = hj1Crop.FilePath;

                //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Muestra el reporte de cumplimiento.
        /// </summary>
        /// <param name="sr">Datos del reporte de Surety</param>
        /// <param name="policyData">Datos de la Póliza</param>
        private void showSuretyReport(Surety sr, Policy policyData)
        {
            string reportOriginal = ConfigurationSettings.AppSettings["ReportExportPath"];
            string UserServiceConsumer = (ConfigurationSettings.AppSettings["UserServiceConsumer"]);
            string PasswordServiceConsumer = (ConfigurationSettings.AppSettings["PasswordServiceConsumer"]);
            string DomainOfUser = (ConfigurationSettings.AppSettings["DomainOfUser"]);

            using (NetworkConnection networkCon = new NetworkConnection(reportOriginal, new NetworkCredential(UserServiceConsumer, PasswordServiceConsumer, DomainOfUser)))
            {

                int intReportType = Convert.ToInt32(policyData.ReportType);
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);

            /* Marca de agua para Temporario o Cotización*/
            if (policyData.TempNum == 0)
            {
                waterMark = "";
            }
            else
            {
                if (policyData.QuotationId > 0)
                {
                    waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
                }
                else
                {
                    waterMark = (policyData.TempNum == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];
                }
            }

            //Parametros reportes Cumplimiento

            string[] strReportParameters = null;
            if (policyData.TempAuthorization)
            {
                strReportParameters = new string[12];
            }
            else
            {
                strReportParameters = new string[11];
            }

            //desarrollo de generacion de codigo de barras
            BarcodeGenerator Barcode = new BarcodeGenerator();

            String pathBarCodeImage = String.Empty;
            String dataBarCode = String.Empty;
            String fileBarCode = String.Empty;
            String pathBarCode = ConfigurationSettings.AppSettings["pathBarCode"].ToString();

            fileBarCode = policyData.PrefixNum.ToString() +
                        policyData.LicensePlate.ToString() +
                        policyData.EndorsementId.ToString() + ".png";
            pathBarCodeImage = pathBarCode + fileBarCode;

            dataBarCode = policyData.PrefixNum.ToString().PadLeft(4, '0') + ";" + policyData.LicensePlate.ToString().PadLeft(6, '0') + ";" + policyData.EndorsementId.ToString().PadLeft(10, '0');

            Barcode.createBarCodeGS1Image(dataBarCode, pathBarCodeImage);
            //hasta aqui el desarrollo de codigo de barras

            policyData.CodeBar = pathBarCodeImage;

            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = sr.SecondPage.ToString();
            strReportParameters[2] = policyData.CodeBar;
            strReportParameters[3] = policyData.TempNum.ToString();
            strReportParameters[4] = sr.ShowPaymentCert.ToString();
            strReportParameters[5] = sr.TextLinesCount.ToString();
            strReportParameters[6] = sr.BeneficiariesCount.ToString();
            strReportParameters[10] = ((int)GetPolicyCoInsurance(policyData.TempNum, policyData.PolicyId, policyData.EndorsementId)).ToString();

            string[] strPaths = new string[3];//Rutas de generación de reporte

            try
            {
                //Se valida que tipo de reporte se seleccionó.
                //Evalua tipo de reporte
                if (intReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT)
                {
                    ///HD-2317 04/05/2010
                    ///Autor: Edgar O. Piraneque E.
                    ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                    ///esto implica si se imprime o no el formato de recaudo
                    if (sr.PrintFormat == 1)
                    {
                        strReportParameters[4] = RiskCount.ToString();
                        addFormatCollect(strReportParameters, policyData);
                    }
                    ///FIN HD-2317 ************************************************
                }
                else if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                {
                    //TODO: Fecha:26/10/2011 ;Autor: John Ruiz; Asunto:Se recuperan textos para imprimir objeto de la poliza y anexos si es nesesario ; Compañia: 3
                    List<string> policyObject = GetTextPages(policyData, 13, 2, 100);
                    if (policyData.PolicyId > 0 && policyData.EndorsementId > 0 && policyData.CurrentFromFirst)
                    {
                        CurrentoFromFirstEndorsement(policyData);
                    }

                    Paths hj1Surety = new Paths(rutas.Count);
                    hj1Surety.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Surety);

                    if (policyData.TempAuthorization)
                    {
                        strPaths[0] = hj1Surety.getPath("SuretyCoverAutho", (RiskCount > 1));
                    }
                    else
                    {
                        strPaths[0] = hj1Surety.getPath("SuretyCover", (RiskCount > 1));
                    }

                    strPaths[1] = hj1Surety.FilePath;

                    strReportParameters[7] = policyObject[0].Trim();
                    strReportParameters[8] = policyObject[1];
                    strReportParameters[9] = string.Empty;
                    if (policyData.TempAuthorization)
                    {
                        strReportParameters[11] = string.Empty;
                    }

                    if (validateSuretyLatePaymentText())
                    {
                        strReportParameters[9] = Resources.SuretyLatePaymentText;
                    }

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    //Valida si la cantidad de líneas a mostrar de texto es diferente a cero.

                    //En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
                    //Se valida si muestra debe mostrar el convenio o no.
                    if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                        (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                        (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                    {
                        //TODO: Fecha:26/10/2011 ;Autor: John Ruiz; Asunto:Se recuperan textos para imprimir objeto de la poliza y anexos si es nesesario ; Compañia: 3
                        int policyObjectCount = 0;
                        if (policyData.TempAuthorization)
                        {
                            policyObjectCount = 2;
                        }
                        else
                        {
                            policyObjectCount = 1;
                        }
                        if (policyObject.Count > policyObjectCount && !policyObject[policyObjectCount].Equals(string.Empty))
                        {
                            List<string> listText = new List<string>();
                            if (policyObject[policyObjectCount].Length > 62650)
                            {
                                listText = GetTextMaximumCharacter(policyObject[policyObjectCount]);
                            }
                            if (listText.Count > 0)
                            {
                                foreach (var text in listText)
                                {
                                    Paths hj2Surety = new Paths(rutas.Count);
                                    hj2Surety.setFileName(policyData, ".pdf");
                                    rutas.Add(hj2Surety);

                                    strPaths[0] = hj2Surety.getPath("SuretyCoverAppendix", (RiskCount > 1));
                                    strPaths[1] = hj2Surety.FilePath;

                                    strReportParameters[7] = text;

                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                }
                            }
                            else
                            {
                                Paths hj2Surety = new Paths(rutas.Count);
                                hj2Surety.setFileName(policyData, ".pdf");
                                rutas.Add(hj2Surety);

                                strPaths[0] = hj2Surety.getPath("SuretyCoverAppendix", (RiskCount > 1));
                                strPaths[1] = hj2Surety.FilePath;

                                strReportParameters[7] = policyObject.Count > 0 ? policyObject[policyObjectCount] : string.Empty;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            }
                        }
                        if (policyData.TempAuthorization && ValidateCountTempAuthorization(policyData.TempNum))
                        {
                            Paths hj3Crop = new Paths(rutas.Count);
                            hj3Crop.setFileName(policyData, ".pdf");
                            rutas.Add(hj3Crop);

                            strPaths[0] = hj3Crop.getPath("AuthoAppendix", (RiskCount > 1));
                            strPaths[1] = hj3Crop.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        }
                        //TODO: <<Autor: Edgar Piraneque; Compañía: 1; 01/12/2010; Asunto: HD-2690 no se genera el reporte 
                        // de Convenio de pago si la propiedad ShowPaymentCert es cero
                        //Reporte Convenio de Pago.
                        //HD-2181
                        if ((sr.PrintConvection == 1) && (sr.ShowPaymentCert == 1) && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                        {
                            //FIN: Edgar Piraneque; 01/12/2010>>
                            Paths hj3Surety = new Paths(rutas.Count);
                            hj3Surety.setFileName(policyData, ".pdf");
                            rutas.Add(hj3Surety);

                            strPaths[0] = hj3Surety.getPath("SuretyConvection", (RiskCount > 1));
                            strPaths[1] = hj3Surety.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        }

                        //TODO: Fecha:02/11/2011 ;Autor: John Ruiz; Asunto: Se imprime forma de producto ; Compañia: 3
                        if (isExpedition(policyData.EndorsementId))
                        {
                            string FormName = GetFormName(policyData.PolicyId);

                            if (!FormName.Equals(string.Empty))
                            {
                                string FormPdfFile = ConfigurationSettings.AppSettings["FormsPath"].ToString() + FormName + ".pdf";
                                if (System.IO.File.Exists(FormPdfFile))
                                {
                                    Paths form = new Paths(rutas.Count);
                                    form.setFileName(policyData, ".pdf");
                                    rutas.Add(form);

                                    System.IO.File.Copy(FormPdfFile, form.FilePath);
                                }
                            }
                        }
                    }
                    else if ((policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY))
                    {
                        //Reporte de Anexo de la Poliza.
                        //TODO: Fecha:26/10/2011 ;Autor: John Ruiz; Asunto:Se recuperan textos para imprimir objeto de la poliza y anexos si es nesesario ; Compañia: 3
                        if (policyObject.Count > 2)
                        {

                            Paths hj4Surety = new Paths(rutas.Count);
                            hj4Surety.setFileName(policyData, ".pdf");
                            rutas.Add(hj4Surety);

                            strReportParameters[7] = policyObject.Count > 0 ? policyObject[2] : string.Empty;

                            strPaths[0] = hj4Surety.getPath("SuretyCoverAppendix", (RiskCount > 1));
                            strPaths[1] = hj4Surety.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        }

                    }

                    else if ((policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION) ||
                             (policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                             (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                             (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                    {
                        //Reporte Convenio de Pago.
                        //HD-2181
                        if (sr.PrintConvection == 1 && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                        {
                            Paths hj5Surety = new Paths(rutas.Count);
                            hj5Surety.setFileName(policyData, ".pdf");
                            rutas.Add(hj5Surety);

                            strPaths[0] = hj5Surety.getPath("SuretyConvection", (RiskCount > 1));
                            strPaths[1] = hj5Surety.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        }
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2317 04/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (sr.PrintFormat == 1 && 1 != 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2317 ************************************************
                    }
                    //TODO: Fecha:04/10/2011 ;Autor: John Ruiz; Asunto:Se agrega impresion de pagare para polizas de cumplimiento ; Compañia: 3
                    if (policyData.ReportType != (int)ReportEnum.ReportType.TEMPORARY ||
                        (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                    {
                        if (policyData.PrintPromissoryNote == 1)
                        {
                            #region Impresión de Pagaré y Carta de Instrucciones
                            //CARGAMOS LOS DATOS DEL ASEGURADO
                            GetInsuredData(policyData);
                            bool IsClosedFlag = GetGuaranteeClosedInd(policyData.ProcessId);
                            //EVALUAMOS SI EL ASEGURADO ES UN CONSORCIO
                            Paths pathPagare;
                            if (IsConsortium())
                            {
                                List<string[]> consortiumMembersAndSigns = GetConsortiumMembers();
                                if (IsClosedFlag)
                                {
                                    //PAGARÉ CERRADO CONSORCIO
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteClosedConsortium", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;

                                    string[] ConsortiumReportsParameters = new string[3];
                                    ConsortiumReportsParameters[0] = policyData.ProcessId.ToString();
                                    ConsortiumReportsParameters[1] = consortiumMembersAndSigns[1][0];
                                    ConsortiumReportsParameters[2] = consortiumMembersAndSigns[1][1];
                                    ReportServiceHelper.loadReportFile(ConsortiumReportsParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    //CARTA DE INSTRUCCIONES CERRADA CONSORCIO
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteClosedLetterConsortium", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;

                                    ConsortiumReportsParameters = new string[3];
                                    ConsortiumReportsParameters[0] = policyData.ProcessId.ToString();
                                    ConsortiumReportsParameters[1] = consortiumMembersAndSigns[0][0];
                                    ConsortiumReportsParameters[2] = consortiumMembersAndSigns[0][1];

                                    ReportServiceHelper.loadReportFile(ConsortiumReportsParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    //string promisoryNoteNum = GetPromisoryNoteNum(policyData);
                                }
                                else
                                {
                                    //PAGARÉ CERRADO CONSORCIO
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteOpenConsortium", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;

                                    string[] ConsortiumReportsParameters = new string[3];
                                    ConsortiumReportsParameters[0] = policyData.ProcessId.ToString();
                                    ConsortiumReportsParameters[1] = consortiumMembersAndSigns[1][0];
                                    ConsortiumReportsParameters[2] = consortiumMembersAndSigns[1][1];
                                    ReportServiceHelper.loadReportFile(ConsortiumReportsParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    //CARTA DE INSTRUCCIONES CERRADA CONSORCIO
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteOpenLetterConsortium", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;

                                    ConsortiumReportsParameters = new string[3];
                                    ConsortiumReportsParameters[0] = policyData.ProcessId.ToString();
                                    ConsortiumReportsParameters[1] = consortiumMembersAndSigns[0][0];
                                    ConsortiumReportsParameters[2] = consortiumMembersAndSigns[0][1];

                                    ReportServiceHelper.loadReportFile(ConsortiumReportsParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                }
                                //PAGARÉCO
                                //Se comenta por que la carta de instrucciones y pagare salen en un solo Rpt
                                //pathPagare = new Paths(rutas.Count);
                                //pathPagare.setFileName(policyData, ".pdf");
                                //rutas.Add(pathPagare);
                                //strPaths[0] = pathPagare.getPath("PromissoryNoteCoverConsortium", (RiskCount > 1));
                                //strPaths[1] = pathPagare.FilePath;
                                //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);



                                //DataTable dtindividuals = GetConsortiumIndividualsIds(policyData);

                                //for (int i = 1; i <= dtindividuals.Rows.Count; i++)
                                //{
                                //    string individualId = dtindividuals.Rows[i - 1]["INDIVIDUAL_ID"].ToString();
                                //    string insuredCd = GetInsuredByIndividualId(individualId);

                                //    string[] strConsortiumReportParameters = new string[4];
                                //    strConsortiumReportParameters[0] = policyData.ProcessId.ToString();
                                //    strConsortiumReportParameters[1] = promisoryNoteNum + "-" + i.ToString();
                                //    strConsortiumReportParameters[2] = individualId;
                                //    strConsortiumReportParameters[3] = insuredCd;                                            


                                //    //CARTA DE INSTRUCCIONES
                                //    pathPagare = new Paths(rutas.Count);
                                //    pathPagare.setFileName(policyData, ".pdf");
                                //    rutas.Add(pathPagare);
                                //    strPaths[0] = pathPagare.getPath("PromissoryNoteCoverConsortiumIndividual", (RiskCount > 1));
                                //    strPaths[1] = pathPagare.FilePath;
                                //    ReportServiceHelper.loadReportFile(strConsortiumReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                //    //FORMATO PAGARE
                                //    pathPagare = new Paths(rutas.Count);
                                //    pathPagare.setFileName(policyData, ".pdf");
                                //    rutas.Add(pathPagare);
                                //    strPaths[0] = pathPagare.getPath("PromissoryNoteConsortiumIndividual", (RiskCount > 1));
                                //    strPaths[1] = pathPagare.FilePath;
                                //    ReportServiceHelper.loadReportFile(strConsortiumReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                //}
                            }
                            else
                            {
                                //SI NO ES CONSORCIO VERIFICAMOS SI EL ASEGURADO ES PERSONA O COMPAÑIA
                                if (IsPerson())
                                {
                                    if (IsClosedFlag)
                                    {
                                        //PAGARÉ CERRADO PERSONA
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteClosedPerson", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    }
                                    else
                                    {
                                        //PAGARÉ ABIERTO PERSONA
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteOpenPerson", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                    }



                                    //FORMATO PAGARE
                                    //pathPagare = new Paths(rutas.Count);
                                    //pathPagare.setFileName(policyData, ".pdf");
                                    //rutas.Add(pathPagare);
                                    //strPaths[0] = pathPagare.getPath("PromissoryNotePerson", (RiskCount > 1));
                                    //strPaths[1] = pathPagare.FilePath;
                                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                }
                                else if (IsCompany())
                                {
                                    if (IsClosedFlag)
                                    {
                                        //PAGARÉ CERRADO COMPAÑIA
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteClosedCompany", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                    }
                                    else
                                    {
                                        //PAGARÉ ABIERTO COMPAÑIA
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteOpenCompany", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                    }

                                    ////FORMATO PAGARE
                                    //pathPagare = new Paths(rutas.Count);
                                    //pathPagare.setFileName(policyData, ".pdf");
                                    //rutas.Add(pathPagare);
                                    //strPaths[0] = pathPagare.getPath("PromissoryNoteCompany", (RiskCount > 1));
                                    //strPaths[1] = pathPagare.FilePath;
                                    //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                }
                            }
                            #endregion
                        }
                    }


                }
                else if (policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION)//Opción 3
                {
                    //Reporte Convenio de Pago.
                    //HD-2181
                    if (sr.PrintConvection == 1 && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                    {
                        Paths hj6Surety = new Paths(rutas.Count);
                        hj6Surety.setFileName(policyData, ".pdf");
                        rutas.Add(hj6Surety);

                        strPaths[0] = hj6Surety.getPath("SuretyConvection", (RiskCount > 1));
                        strPaths[1] = hj6Surety.FilePath;

                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2317 04/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (sr.PrintFormat == 1 && 1 != 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2317 ************************************************
                    }

                }
                //else if ((policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                //{
                //    Paths hj1Surety = new Paths(rutas.Count);
                //    hj1Surety.setFileName(policyData, ".pdf");
                //    rutas.Add(hj1Surety);

                //    strPaths[0] = hj1Surety.getPath("SuretyQuotationCover", (RiskCount > 1));
                //    strPaths[1] = hj1Surety.FilePath;

                //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
          }
        }

        private bool validateSuretyLatePaymentText()
        {
            bool res = false;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetPrefixAndPolicyTypeByProcessID, PolicyData.ProcessId));

            if (result.Rows.Count > 0)
            {
                int PrefixCd = Convert.ToInt32(result.Rows[0]["PREFIX_CD"].ToString());
                int PolicyTypeCd = Convert.ToInt32(result.Rows[0]["POLICY_TYPE_CD"].ToString());

                if (PrefixCd == 30 && PolicyTypeCd == 2)//Si es cumplimiento Particulaares retorno true 
                {
                    res = true;
                }
            }

            return res;
        }

        private List<string[]> GetConsortiumMembers()
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetConsortiumMembersInfoCompanyByInsuredCd, _insuredId));

            string[] members = new string[2];
            string[] signs = new string[2];

            bool flagMember = false;

            foreach (DataRow row in result.Rows)
            {
                if (flagMember)
                {
                    members[1] += GetCompanyMember(row);
                    signs[1] += Resources.ConsortiumSignFormat;
                    flagMember = false;
                }
                else
                {
                    members[0] += GetCompanyMember(row);
                    signs[0] += Resources.ConsortiumSignFormat;
                    flagMember = true;
                }
            }

            result = dda.ExecuteDataTable(string.Format(Querys.GetConsortiumMembersInfoPersonByInsuredCd, _insuredId));

            foreach (DataRow row in result.Rows)
            {
                if (flagMember)
                {
                    members[1] += GetPersonMember(row);
                    signs[1] += Resources.ConsortiumSignFormat;
                    flagMember = false;
                }
                else
                {
                    members[0] += GetPersonMember(row);
                    signs[0] += Resources.ConsortiumSignFormat;
                    flagMember = true;
                }
            }

            List<string[]> ret = new List<string[]>();
            ret.Add(members);
            ret.Add(signs);
            return ret;
        }

        private string GetCompanyMember(DataRow row)
        {
            return string.Format(Resources.ConsortiumCompanyMemberFormat
                        , row["LR_NAME"]
                        , row["LR_ID_CARD_TYPE"].ToString() == string.Empty ? "CC" : row["LR_ID_CARD_TYPE"].ToString()
                        , row["LR_ID_CARD_NUM"].ToString() == string.Empty ? string.Empty : string.Format("{0:#,##}", Convert.ToInt32(row["LR_ID_CARD_NUM"].ToString()))
                        , row["COMP_NAME"]
                        , string.Format("{0:#,##}", Convert.ToInt32(row["COMP_ID_NUM"].ToString())) + "-" + row["COM_VERIFY_DIGIT"].ToString()
                        , row["COMP_ADDRESS"]
                        , row["PHONE_NUMBER"]
                        , row["CITY"]);

        }

        private string GetPersonMember(DataRow row)
        {
            return string.Format(Resources.ConsortiumPersonMemberFormat
                        , row["FULL_NAME"]
                        , row["ID_CARD_TYPE"] == null ? "CC" : row["ID_CARD_TYPE"]
                        , string.Format("{0:#,##}", Convert.ToInt32(row["ID_CARD_NO"].ToString()))
                        , row["ADDRESS"]
                        , row["PHONE_NUMBER"]
                        , row["CITY"]);

        }

        private bool GetGuaranteeClosedInd(int processId)
        {
            bool ClosedInd = false;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetGuaranteeClosedIndBYProcessId, processId));

            ClosedInd = Convert.ToBoolean(result.Rows[0]["CLOSED_IND"].ToString());

            return ClosedInd;
        }

        private string GetFormName(int policyId)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format("SELECT PF.FORM_NUMBER FROM ISS.POLICY P INNER JOIN PROD.PRODUCT_FORM PF ON P.PRODUCT_ID = PF.PRODUCT_ID WHERE P.POLICY_ID = {0}", policyId));
            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["FORM_NUMBER"].ToString().Replace("/", "");
            }
            return string.Empty;
        }

        /// <summary>
        /// Valida si el endoso es de Expedición
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        private bool isExpedition(int endorsementId)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.EndorsementTypeByEndorsementId, endorsementId));

            int endorsementType = 0;
            if (result.Rows.Count > 0)
            {
                endorsementType = Convert.ToInt32(result.Rows[0]["ENDO_TYPE_CD"].ToString());
            }

            if (endorsementType == 1)
            {
                return true;
            }
            return false;
        }

        //TODO: Fecha:26/10/2011 ;Autor: John Ruiz; Asunto:Se recuperan textos para imprimir objeto de la poliza y anexos si es nesesario ; Compañia: 3
        /// <summary>
        /// Procesa el texto de la poliza y devuelve la cantidad de hojas con sus textos
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        //TODO: Fecha:26/10/2011 ;Autor: John Ruiz; Asunto:Se recuperan textos para imprimir objeto de la poliza y anexos si es nesesario ; Compañia: 3
        /// <summary>
        /// Procesa el texto de la poliza y devuelve la cantidad de hojas con sus textos
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<string> GetTextPages(Policy policyData, int CoverTextLines, int CoverAnnotationLines, int pageLineCharacters, bool textCaution = false)
        {
            List<string> result = new List<string>();
            string ApendixText = string.Empty;
            int textLength = 0;
            string[] PolicyText = new string[1];

            List<string> PolicyTexts = GetPolicyText(policyData.ProcessId, policyData.EndorsementText);

            if (policyData.TempAuthorization)
            {
                textLength = 600; //CountLinesText(PolicyTexts, 700, 4);
            }
            else
            {
                textLength = 1200; //CountLinesText(PolicyTexts, 1100, 8);
            }

            List<string> CoverArr = GetTextGeneralPolicy(PolicyTexts, textLength, textCaution);
            result.Add(CoverArr[0]);

            if (CoverArr.Count > 1)
            {
                if (!CoverArr[1].Equals(string.Empty) && CoverArr[1].Trim().Length > 0)
                {
                    ApendixText += CoverArr[1];
                }
            }

            if (policyData.TempAuthorization)
            {
                List<string> AnnotationsText = GetAnnotaions(policyData.ProcessId, policyData.EndorsementText);
                //textLength = CountLinesText(textSplit, 700, 4);

                List<string> AnnotationsArr = GetTextGeneralPolicy(AnnotationsText, 650, false);
                //string[] AnnotationsArr = GetFirstAndSecondPageText(CoverAnnotationLines, pageLineCharacters, AnnotationsText);
                result.Add(AnnotationsArr[0]);

                if (AnnotationsArr != null && AnnotationsArr.Count > 1)
                {
                    if (!AnnotationsArr[1].Equals(string.Empty) && AnnotationsArr[1].Trim().Length > 0)
                    {
                        ApendixText += "\r\rOBSERVACIONES:\r\r" + AnnotationsArr[1];
                    }
                }
            }

            //ApendixText += PolicyText[1];

            string clauses = GetCluases(policyData.ProcessId);
            if (!string.IsNullOrEmpty(clauses))
            {
                ApendixText += clauses;
            }

            result.Add(ApendixText);

            //TODO: Fecha: 03/11/2011; Autor: John Ruiz; Asunto: Valido que la ultima pagina no tenga solamente retornos de carro, en caso de ser asi elimino el elemento; Compañia:2

            if (result.Count > 2)
            {

                string lastPage = result.ElementAt(result.Count - 1);

                bool deleteLastPage = true;

                foreach (string item in lastPage.Split('\r'))
                {
                    if (!item.Trim().Equals(string.Empty))
                    {
                        deleteLastPage = false;
                        break;
                    }
                }

                if (deleteLastPage)
                {
                    result.RemoveAt(result.Count - 1);
                }
            }

            return result;
        }

        public int CountLinesText(List<string> textSplit, int count, int lines)
        {
            int lineText = 0;
            int countResult = count;
            int countLines = 0;
            if (textSplit != null && textSplit.Where(x => string.IsNullOrEmpty(x)).Count() > 0)
            {
                for (int i = 0; i < lines; i++)
                {
                    count -= textSplit[i].Length;
                    if (count <= 0 || countLines > countResult)
                    {
                        countLines = countResult;
                        break;
                    }
                    else
                    {
                        if (textSplit[i].Length > 140)
                        {
                            lineText = textSplit[i].Length;
                            while (lineText > 0)
                            {
                                if (lineText > 140)
                                {
                                    lines -= 1;
                                    lineText -= 140;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        countLines += textSplit[i].Length;
                    }
                }
            }
            else
            {
                countLines = count;
            }
            return countLines;
        }

        private string[] GetTextPagesEndorsement(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string tempChar = string.Empty;
                List<string> tempCharList = new List<string>();
                string[] textLine = text.Split(new char[] { '\r', '\n' });
                for (int i = 0; i < text.Length; i++)
                {
                    if (text.Substring(i, 1) == "\n" || text.Substring(i, 1) == "\r")
                    {
                        tempChar += text.Substring(i, 1);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tempChar))
                        {
                            tempCharList.Add(tempChar);
                            tempChar = string.Empty;
                        }
                    }
                }
                for (int i = 0; i < textLine.Length; i++)
                {
                    if (textLine[i].Length > 0)
                    {
                        if (tempCharList.Count == 0)
                        {
                            textLine[i] += "\r\r";
                        }
                        else
                        {
                            if (tempCharList.FirstOrDefault().Length > 1)
                            {
                                if (tempCharList.FirstOrDefault().Substring(0, 2) == "\r\n" && string.IsNullOrEmpty(textLine[i + 1]))
                                {
                                    textLine = textLine.Where((val, idx) => idx != (i + 1)).ToArray();
                                }
                            }
                            textLine[i] += tempCharList.FirstOrDefault();
                            tempCharList = tempCharList.Skip(1).ToList();
                        }
                    }
                }
                return textLine;
            }
            return null;
        }

        public List<string> GetTextMaximumCharacter(string text)
        {
            List<string> listText = new List<string>();
            bool process = true;
            while (process)
            {
                string temporalText = string.Empty;
                int position = 0;
                temporalText = text.Remove(text.Length > 62650 ? 62650 : text.Length);
                position = temporalText.LastIndexOf(" ");
                temporalText = temporalText.Remove(position);
                listText.Add(temporalText);
                text = text.Substring(temporalText.Length + 1);
                if (text.Length > 62650)
                {
                    process = true;
                }
                else
                {
                    process = false;
                    listText.Add(text);
                }
            }
            return listText;
        }

        public List<string> GetTextGeneralPolicy(List<string> text, int length, bool textCaution = false)
        {
            List<string> listText = new List<string>();
            string temporalText = string.Empty;
            string textMain = string.Empty;
            string textSecond = string.Empty;

            if (text != null && text.Count > 0)
            {
                if (!textCaution)
                {
                    for (int i = 0; i < text.Count(); i++)
                    {
                        if (text[i].Length > length && length > 0)
                        {
                            int position = 0;
                            temporalText = text[i].Remove(text[i].Length > length ? length : text[i].Length);
                            length -= text[i].Length;
                            position = temporalText.LastIndexOf(" ");
                            if (position > 0)
                            {
                                temporalText = temporalText.Remove(position);
                            }
                            textMain += temporalText;
                            textSecond += text[i].Substring(temporalText.Length + 1);
                        }
                        else if (length < 0)
                        {
                            if (text[i].Length > 0)
                            {
                                textSecond += text[i];
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(text[i]))
                            {
                                if (text[i].Length < 150)
                                {
                                    length -= (150 - text[i].Length);
                                    length -= text[i].Length;
                                }
                                else if ((150 - text[i].Length) == 0)
                                {
                                    length = -150;
                                }
                                else
                                {
                                    if (text[i].Length > 150)
                                    {
                                        int lengthTemp = text[i].Length;
                                        while (lengthTemp > 0)
                                        {
                                            if (lengthTemp < 150)
                                            {
                                                length -= (150 - lengthTemp);
                                                length -= lengthTemp;
                                                lengthTemp -= (150 - lengthTemp);
                                                lengthTemp -= lengthTemp;
                                            }
                                            else
                                            {
                                                length -= 150;
                                                lengthTemp -= 150;
                                            }
                                        }
                                    }
                                }
                                if (length < 0)
                                {
                                    textSecond += text[i];
                                }
                                else
                                {
                                    textMain += text[i];
                                }
                            }
                            else
                            {
                                length -= 150;
                            }
                        }
                    }
                    listText.Add(textMain);
                    listText.Add(textSecond);

                    return listText.ToList();
                }
                else
                {
                    listText.Add(string.Empty);
                    for (int i = 0; i < text.Count(); i++)
                    {
                        if (text[i].Length > 0)
                        {
                            textSecond += text[i];
                        }
                    }
                    listText.Add(textSecond);
                }
            }
            else
            {
                listText.Add(string.Empty);
                listText.Add(string.Empty);
            }
            return listText;
        }

        //private IEnumerable<string> getApendixPageList(int ApendixLines, int pageLineCharacters, string ApendixText)
        //{
        //    List<string> result = new List<string>();

        //    string[] Lines = ApendixText.Split('\r');
        //    string PageText = string.Empty;
        //    int LinesCount = 0;

        //    for (int i = 0; i < Lines.Length; i++)
        //    {
        //        string item = Lines[i];

        //        if (LinesCount >= ApendixLines)
        //        {
        //            result.Add(PageText);
        //            PageText = string.Empty;
        //            LinesCount = 0;
        //        }


        //        PageText += item.Replace("\n", string.Empty) + "\r";

        //        LinesCount += item.Length % pageLineCharacters > 0 ? 1 : 0;

        //        LinesCount += item.Length / pageLineCharacters;

        //        if (i == Lines.Length - 1)
        //        {
        //            result.Add(PageText);
        //        }

        //    }

        //    return validateReturnsOnEnd(result);
        //}

        private string[] GetFirstAndSecondPageText(int TextLines, int pageLineCharacters, string Text)
        {
            List<string> Textarr = new List<string>();

            string PageText = string.Empty;
            bool FirstPageComplete = false;
            int LinesCount = 0;

            string[] Lines = Text.Split('\r');
            for (int i = 0; i < Lines.Length; i++)
            {
                string item = Lines[i];

                if (LinesCount >= TextLines && !FirstPageComplete)
                {
                    Textarr.Add(PageText);
                    PageText = string.Empty;
                    LinesCount = 0;
                    FirstPageComplete = true;
                }


                PageText += item.Replace("\n", string.Empty) + "\r";

                LinesCount += item.Length % pageLineCharacters > 0 ? 1 : 0;

                LinesCount += item.Length / pageLineCharacters;

                if (i == Lines.Length - 1)
                {
                    Textarr.Add(PageText);
                }

            }

            return validateReturnsOnEnd(Textarr).ToArray();
        }

        private List<string> validateReturnsOnEnd(List<string> Textarr)
        {
            for (int i = 0; i < Textarr.Count; i++)
            {
                if (Textarr[i].Trim().Equals("\r") || Textarr[i].Trim().Equals(string.Empty))
                {
                    Textarr[i] = string.Empty;
                    continue;
                }
                else
                {
                    List<string> lines = Textarr[i].Split('\r').ToList();

                    bool flag = true;

                    while (flag)
                    {
                        if (lines[lines.Count - 1].Trim().Equals(string.Empty))
                        {
                            lines.RemoveAt(lines.Count - 1);
                        }

                        if (!lines[lines.Count - 1].Trim().Equals(string.Empty))
                        {
                            flag = false;
                        }
                    }

                    Textarr[i] = string.Empty;

                    foreach (var item in lines)
                    {
                        Textarr[i] += item + "\r";
                    }
                }


            }
            return Textarr;
        }

        private List<string> GetAnnotaions(int processId, bool endorsementText)
        {
            string result = string.Empty;
            DynamicDataAccess dda = new DynamicDataAccess();

            if (endorsementText)
            {
                DataTable dtResult = dda.ExecuteDataTable(string.Format(Querys.GetAnnotationEndorsementByPolicyId, processId));

                if (dtResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        result += dtResult.Rows[i]["ANNOTATIONS"].ToString().Trim() + "\r\r";
                    }
                }
            }
            else
            {
                DataTable dtResult = dda.ExecuteDataTable(string.Format(Querys.AnnotationsByProcessId, processId));

                if (dtResult.Rows.Count > 0)
                {
                    result += dtResult.Rows[0]["ANNOTATIONS"].ToString().Trim();
                }
            }

            List<string> resultAnnotations = new List<string>();
            string[] splitAnnotations = GetTextPagesEndorsement(result);
            if (splitAnnotations != null && splitAnnotations.Count() > 0)
            {
                foreach (string item in splitAnnotations)
                {
                    resultAnnotations.Add(item);
                }
            }

            return resultAnnotations;
        }

        private string GetCluases(int processId)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            string strClausesText = string.Empty;

            string ClausesSql = string.Format(Querys.GeneralClausesByProcessId, processId);
            DataTable ClausesDt = dda.ExecuteDataTable(ClausesSql);

            if (ClausesDt != null)
            {
                if (ClausesDt.Rows.Count > 0 &&
                    (ClausesDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["CLAUSE_TITLE"].ToString())).Count() > 0 ||
                    ClausesDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["CLAUSE_TEXT"].ToString())).Count() > 0))
                {
                    strClausesText += "\r\rCLAUSULAS GENERALES:\r\r";
                    foreach (DataRow row in ClausesDt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["CLAUSE_TITLE"].ToString()))
                        {
                            strClausesText += row["CLAUSE_TITLE"].ToString() + "\r\r";
                        }
                        if (!string.IsNullOrEmpty(row["CLAUSE_TEXT"].ToString()))
                        {
                            strClausesText += row["CLAUSE_TEXT"].ToString() + "\r\r";
                        }
                    }
                }
            }

            ClausesSql = string.Format(Querys.RiskClausesByProcessId, processId);
            dda = new DynamicDataAccess();
            ClausesDt = dda.ExecuteDataTable(ClausesSql);

            if (ClausesDt != null)
            {
                if (ClausesDt.Rows.Count > 0 &&
                    (ClausesDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["CLAUSE_TITLE"].ToString())).Count() > 0 ||
                    ClausesDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["CLAUSE_TEXT"].ToString())).Count() > 0))
                {
                    strClausesText += "\r\rCLAUSULAS RIESGO:\r\r";
                    foreach (DataRow row in ClausesDt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["CLAUSE_TITLE"].ToString()))
                        {
                            strClausesText += row["CLAUSE_TITLE"].ToString() + "\r\r";
                        }
                        if (!string.IsNullOrEmpty(row["CLAUSE_TEXT"].ToString()))
                        {
                            strClausesText += row["CLAUSE_TEXT"].ToString() + "\r\r";
                        }
                    }
                }
            }

            ClausesSql = string.Format(Querys.CoverageClausesByProcessId, processId);
            dda = new DynamicDataAccess();
            ClausesDt = dda.ExecuteDataTable(ClausesSql);

            if (ClausesDt != null)
            {
                if (ClausesDt.Rows.Count > 0 &&
                    (ClausesDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["CLAUSE_TITLE"].ToString())).Count() > 0 ||
                    ClausesDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["CLAUSE_TEXT"].ToString())).Count() > 0))
                {
                    strClausesText += "\r\rCLAUSULAS COBERTURAS:\r\r";
                    foreach (DataRow row in ClausesDt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["CLAUSE_TITLE"].ToString()))
                        {
                            strClausesText += row["CLAUSE_TITLE"].ToString() + "\r\r";
                        }
                        if (!string.IsNullOrEmpty(row["CLAUSE_TEXT"].ToString()))
                        {
                            strClausesText += row["CLAUSE_TEXT"].ToString() + "\r\r";
                        }
                    }
                }
            }
            return strClausesText;
        }

        private EnumCoInsurance GetPolicyCoInsurance(int tempId, int policyId, int endorsementId)
        {
            if (tempId == 0)
            {
                DynamicDataAccess dda = new DynamicDataAccess();
                string QuerySql = string.Format(Querys.GetPolicyCoInsuranceAccepted, policyId, endorsementId);
                DataTable CoInsuranceDt = dda.ExecuteDataTable(QuerySql);

                if (CoInsuranceDt != null && CoInsuranceDt.Rows.Count > 0)
                {
                    return EnumCoInsurance.CoInsuranceAccepted;
                }

                dda = new DynamicDataAccess();
                QuerySql = string.Format(Querys.GetPolicyCoInsuranceAssigned, policyId, endorsementId);
                CoInsuranceDt = dda.ExecuteDataTable(QuerySql);

                if (CoInsuranceDt != null && CoInsuranceDt.Rows.Count > 0)
                {
                    return EnumCoInsurance.CoInsuranceAssigned;
                }
            }
            else
            {
                DynamicDataAccess dda = new DynamicDataAccess();
                string QuerySql = string.Format(Querys.GetTempCoInsuranceAccepted, tempId);
                DataTable CoInsuranceDt = dda.ExecuteDataTable(QuerySql);

                if (CoInsuranceDt != null && CoInsuranceDt.Rows.Count > 0)
                {
                    return EnumCoInsurance.CoInsuranceAccepted;
                }

                dda = new DynamicDataAccess();
                QuerySql = string.Format(Querys.GetTempCoInsuranceAssigned, tempId);
                CoInsuranceDt = dda.ExecuteDataTable(QuerySql);

                if (CoInsuranceDt != null && CoInsuranceDt.Rows.Count > 0)
                {
                    return EnumCoInsurance.CoInsuranceAssigned;
                }
            }
            return EnumCoInsurance.CoInsuranceNull;
        }

        private bool ValidateTempAuthorization(int tempId)
        {
            if (tempId > 0)
            {
                DynamicDataAccess dda = new DynamicDataAccess();
                string QuerySql = string.Format(Querys.GetTempAuthorization, tempId);
                DataTable AuthoDt = dda.ExecuteDataTable(QuerySql);

                if (AuthoDt != null && AuthoDt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private bool ValidateCountTempAuthorization(int tempId)
        {
            if (tempId > 0)
            {
                DynamicDataAccess dda = new DynamicDataAccess();
                string QuerySql = string.Format(Querys.GetCountTempAuthorzation, tempId);
                DataTable AuthoDt = dda.ExecuteDataTable(QuerySql);

                if (AuthoDt != null && AuthoDt.Rows.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private string GetAnnotationsEndorsementAutho(int processId, int policyId)
        {
            string result = string.Empty;
            DynamicDataAccess dda = new DynamicDataAccess();

            if (policyId > 0)
            {
                DataTable dtResult = dda.ExecuteDataTable(string.Format(Querys.GetAnnotationEndorsementByPolicyId, policyId));

                if (dtResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        result += dtResult.Rows[i]["ANNOTATIONS"].ToString().Trim();
                    }
                }
            }
            else
            {
                DataTable dtResult = dda.ExecuteDataTable(string.Format(Querys.AnnotationsByProcessId, processId));

                if (dtResult.Rows.Count > 0)
                {
                    result += dtResult.Rows[0]["ANNOTATIONS"].ToString().Trim();
                }
            }

            return result;
        }

        private string[] GetPolicyTextEndorsementAutho(int processId, int policyId)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable GeneralTextDt = null;
            string strGeneralText = string.Empty;

            if (policyId > 0)
            {
                string GeneralLevelSql = string.Format(Querys.GetTextEndorsementByPolicyId, policyId);
                GeneralTextDt = dda.ExecuteDataTable(GeneralLevelSql);
                if (GeneralTextDt != null && GeneralTextDt.Rows.Count > 0)
                {
                    for (int i = 0; i < GeneralTextDt.Rows.Count; i++)
                    {
                        strGeneralText += GeneralTextDt.Rows[i]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
                    }
                }
            }
            else
            {
                string GeneralLevelSql = string.Format(Querys.GeneralTextByProcessId, processId);
                GeneralTextDt = dda.ExecuteDataTable(GeneralLevelSql);
                if (GeneralTextDt != null && GeneralTextDt.Rows.Count > 0)
                {
                    strGeneralText = GeneralTextDt.Rows[0]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
                }
            }
            string[] result = { strGeneralText };

            return result;
        }


        public int GetNumberDeductibles(int tempId, int policyId, int endorsementId)
        {
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable GeneralTextDt = null;
            string strGeneralText = string.Empty;
            string GeneralLevelSql = string.Empty;

            if (tempId > 0)
            {
                GeneralLevelSql = string.Format(Querys.GetDeductiblesByTempId, tempId);
            }
            else
            {
                GeneralLevelSql = string.Format(Querys.GetDeductibleByPolicyId, policyId, endorsementId);
            }
            GeneralTextDt = dda.ExecuteDataTable(GeneralLevelSql);
            if (GeneralTextDt != null && GeneralTextDt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(GeneralTextDt.Rows[0]["DEDUCTIBLE"].ToString()))
                {
                    return Convert.ToInt16(GeneralTextDt.Rows[0]["DEDUCTIBLE"].ToString());
                }
            }
            return 0;
        }

        public void CurrentoFromFirstEndorsement(Policy policy)
        {
            NameValue[] paramsp = new NameValue[2];
            paramsp[0] = new NameValue("PROCESS_ID", Convert.ToInt32(policy.ProcessId));
            paramsp[1] = new NameValue("POLICY_ID", Convert.ToInt32(policy.PolicyId));

            ReportServiceHelper.getData("REPORT.CO_UPDATE_CURRENT_FROM_ENDORSEMENT", paramsp);
        }

        private List<string> GetPolicyText(int processId, bool endorsementText)
        {
            //Textos a nivel general
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable GeneralTextDt = null;
            string strGeneralText = string.Empty;

            if (endorsementText)
            {
                string GeneralLevelSql = string.Format(Querys.GetTextEndorsementByPolicyId, processId);
                GeneralTextDt = dda.ExecuteDataTable(GeneralLevelSql);
                if (GeneralTextDt != null && GeneralTextDt.Rows.Count > 0)
                {
                    for (int i = 0; i < GeneralTextDt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(GeneralTextDt.Rows[i]["CONDITION_TEXT"].ToString()))
                        {
                            strGeneralText += "ANEXO " + ((int)GeneralTextDt.Rows[i]["DOCUMENT_NUM"]) + ":\r\r";
                            strGeneralText += GeneralTextDt.Rows[i]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
                        }
                    }
                }
            }
            else
            {
                string GeneralLevelSql = string.Format(Querys.GeneralTextByProcessId, processId);
                GeneralTextDt = dda.ExecuteDataTable(GeneralLevelSql);
                if (GeneralTextDt != null && GeneralTextDt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(GeneralTextDt.Rows[0]["CONDITION_TEXT"].ToString()))
                    {
                        strGeneralText = GeneralTextDt.Rows[0]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
                    }
                }
            }
            string[] splitGeneralText = GetTextPagesEndorsement(strGeneralText);
            //if (GeneralTextDt.Rows.Count > 0 && GeneralTextDt.Rows[0]["CONDITION_TEXT"].ToString() != string.Empty)
            //    strGeneralText = GeneralTextDt.Rows[0]["CONDITION_TEXT"].ToString() + "\r\r";



            //textos a nivel de riesgo
            dda = new DynamicDataAccess();
            string RiskLevelSql = string.Format(Querys.RiskTextByProcessId, processId);
            DataTable riskLevelDt = dda.ExecuteDataTable(RiskLevelSql);
            string strRiskLevelText = string.Empty;

            //if (riskLevelDt.Rows.Count > 0 && riskLevelDt.Rows[0]["CONDITION_TEXT"].ToString() != string.Empty)
            //    strRiskLevelText = riskLevelDt.Rows[0]["CONDITION_TEXT"].ToString() + "\r\r";

            if (riskLevelDt != null && riskLevelDt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(riskLevelDt.Rows[0]["CONDITION_TEXT"].ToString()))
                {
                    strRiskLevelText += "OBJETO DE LA POLIZA:\r\r";
                    strRiskLevelText += riskLevelDt.Rows[0]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
                }
            }
            else
            {
                dda = new DynamicDataAccess();
                RiskLevelSql = string.Format(Querys.RiskTempTextByProcessId, processId);
                riskLevelDt = dda.ExecuteDataTable(RiskLevelSql);
                strRiskLevelText = string.Empty;
                if (riskLevelDt != null && riskLevelDt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(riskLevelDt.Rows[0]["CONDITION_TEXT"].ToString()))
                    {
                        strRiskLevelText += "OBJETO DE LA POLIZA:\r\r";
                        strRiskLevelText += riskLevelDt.Rows[0]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
                    }
                }
            }
            string[] splitRiskLevelText = GetTextPagesEndorsement(strRiskLevelText);

            //Texto a nivel de covertura
            dda = new DynamicDataAccess();
            string CoverageLevelSql = string.Format(Querys.CoverageTextByProcessId, processId);
            DataTable coverageLevelDt = dda.ExecuteDataTable(CoverageLevelSql);
            string strCoverageLevelText = string.Empty;

            //if (coverageLevelDt.Rows.Count > 0 && coverageLevelDt.Rows[0]["CONDITION_TEXT"].ToString() != string.Empty)
            //    strCoverageLevelText = coverageLevelDt.Rows[0]["CONDITION_TEXT"].ToString() + "\r\r";

            if (coverageLevelDt != null && coverageLevelDt.Rows.Count > 0)
            {
                strCoverageLevelText = coverageLevelDt.Rows[0]["CONDITION_TEXT"].ToString().Trim() + "\r\r";
            }

            //Clusulas se lee desde pdf con nombre de forma parametrizado
            //string ClausesSql = string.Format(Querys.GeneralClausesByProcessId, processId);
            //DataTable ClausesDt = dda.ExecuteDataTable(ClausesSql);
            //string strClausesText = string.Empty;
            //if (ClausesDt.Rows.Count > 0)
            //{
            //    foreach (DataRow row in ClausesDt.Rows)
            //    {
            //        strClausesText += ClausesDt.Rows[0]["CLAUSE_TITLE"].ToString() + "\r\r";
            //        strClausesText += ClausesDt.Rows[0]["CLAUSE_TEXT"].ToString() + "\r\r";
            //    }
            //}
            //kevin comentaarea codigo a continuacion
            //Beneficiarios en caso que exista mas de uno
            //DataTable dtBeneficiaries = dda.ExecuteSPDataTable("REPORT.CO_GET_BENEFICIARY_LIST", new NameValue("PROCESS_ID", processId));

            string beneficiaries = string.Empty;
            //if (dtBeneficiaries.Rows.Count > 1)
            //{
            //    beneficiaries += "\rBeneficiarios\rNombre\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tDocumento\r";
            //    foreach (DataRow item in dtBeneficiaries.Rows)
            //    {
            //        string name = item["NAME"].ToString().Length > 60 ? item["NAME"].ToString().Substring(0, 60) : item["NAME"].ToString();
            //        string doc = item["DOC_NUMBER"].ToString();

            //        int spaces = 8 - name.ToString().Length % 8;

            //        for (int i = 0; i < spaces; i++)
            //        {
            //            name += " ";
            //        }

            //        int tabs = 16 - (name.Length / 8);
            //        //tabs -= name.ToString().Length % 8 > 0 ? 1 : 0;

            //        beneficiaries += name;

            //        for (int i = 0; i < tabs; i++)
            //        {
            //            beneficiaries += "\t";
            //        }

            //        beneficiaries += doc + "\r";
            //    }

            //}

            List<string> result = new List<string>();
            if (splitGeneralText != null && splitGeneralText.Count() > 0)
            {
                foreach (string item in splitGeneralText)
                {
                    result.Add(item);
                }
            }
            if (splitRiskLevelText != null && splitRiskLevelText.Count() > 0)
            {
                foreach (string item in splitRiskLevelText)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Genera impresión de texto general para automóviles
        /// </summary>
        /// <param name="policyData"></param>
        /// <param name="vc"></param>
        /// <param name="strReportParameters"></param>
        /// <param name="strPaths"></param>
        private void printConditionTextAuto(Policy policyData, Vehicle vc, string[] strReportParameters, string[] strPaths)
        {
            double lenCondition = vc.ConditionText.ToString().Length / (float)(ReportEnum.TextLines.MAX_CHAR_PER_PRINT);
            if (lenCondition > 1)
            {
                if ((vc.ConditionText.ToString().Length % (int)ReportEnum.TextLines.MAX_CHAR_PER_PRINT) > 0)
                {
                    lenCondition = Math.Truncate(lenCondition);
                    lenCondition += 1;
                }
                int i = 1;
                Int64 pointInitial;
                NameValue[] paramsp = new NameValue[2];
                DataSet dsText;
                Paths hjTextVehicle;

                while (i < lenCondition)
                {
                    //Ejecutar procedimiento en el cual se retiran los primeros 8000 caracteres en el
                    //campo CONDITION_TEXT y se vuelve a generar el reporte
                    if (i == 1)
                    {
                        pointInitial = (int)(ReportEnum.TextLines.MAX_CHAR_PER_LINE) + 1;
                    }
                    else
                    {
                        pointInitial = (int)(ReportEnum.TextLines.MAX_CHAR_PER_LINE) + (i * (int)(ReportEnum.TextLines.MAX_CHAR_PER_PRINT)) + 1;
                    }
                    paramsp[0] = new NameValue("PROCESS_ID", policyData.ProcessId);
                    paramsp[1] = new NameValue("POINT_INI", pointInitial);
                    dsText = ReportServiceHelper.getData("REPORT.WRITE_SECOND_PAGE_TEXT", paramsp);
                    dsText.Clear();
                    hjTextVehicle = new Paths(rutas.Count);
                    hjTextVehicle.setFileName(policyData, ".pdf");
                    rutas.Add(hjTextVehicle);

                    strPaths[0] = hjTextVehicle.getPath("VehicleCoverAppendix", (RiskCount > 1));
                    strPaths[1] = hjTextVehicle.FilePath;

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    i += 1;
                }
            }
        }


        /// <summary>
        /// Genera los reportes que conforman la caratula de la póliza colectiva o Individual con exclusión.
        /// </summary>
        /// <param name="intProcessId">Id. del Proceso.</param>
        /// <param name="policyData">Datos de la poliza</param>
        /// <returns>Ruta del archivo de la caratula del reporte generado</returns>
        private string showVehicleCoverReport(int intProcessId, Vehicle vc, Policy policyData)
        {
            try
            {
                Paths caratula = new Paths();
                caratula.setFileName(policyData, ".pdf");
                rutas.Add(caratula);
                Paths hj1Caratula = new Paths(rutas.Count);
                hj1Caratula.setFileName(policyData, ".pdf");
                rutas.Add(hj1Caratula);

                waterMark = (Convert.ToInt32(policyData.TempNum) == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];

                //Parametros reportes Automoviles
                string[] strReportParameters = new string[10];
                strReportParameters[0] = intProcessId.ToString();
                strReportParameters[1] = (vc.RowCount + 5).ToString();
                strReportParameters[2] = policyData.CodeBar;
                strReportParameters[3] = policyData.TempNum.ToString();
                strReportParameters[4] = policyData.RangeMinValue.ToString();
                strReportParameters[5] = policyData.RangeMaxValue.ToString();

                //Rutas de generación de reporte
                string[] strPaths = new string[3];

                strPaths[0] = hj1Caratula.getPath("CoverVehicleCover", (RiskCount > 1));
                strPaths[1] = hj1Caratula.FilePath;

                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                return caratula.FilePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.Excel);
            }
            else
            {
                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);
            }
        }

        /// <summary>
        /// Genera los reportes para el ramo de Multiriesgos Localizacion y los exporta a .pdf.
        /// </summary>
        /// <param name="rc">Datos del reporte de LC.</param>
        /// <param name="policyData">Datos de la póliza</param>

        private void showLocationReport(Location lc, Policy policyData)
        {
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);

            //Para saber si la marca de agua debe ser de Temporarios o de Cotizaciones
            if ((policyData.TempNum > 0) && (policyData.QuotationId == 0))
            {
                waterMark = ConfigurationSettings.AppSettings["WaterMark"];
            }

            NameValue[] LocationSpParams = new NameValue[1];

            //Parametros reportes Responsabilidad civil
            string[] strReportParameters = new string[10];
            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = lc.SecondPage.ToString();
            strReportParameters[2] = policyData.CodeBar;
            strReportParameters[3] = policyData.TempNum.ToString();
            strReportParameters[4] = lc.RegisterCount.ToString();
            strReportParameters[5] = lc.BeneficiariesCount.ToString();

            string[] strPaths = new string[3];//Rutas de generación de reporte

            try
            {
                if (policyData.ReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT)
                {
                    if (lc.PrintFormat == 1)
                    {
                        strReportParameters[4] = RiskCount.ToString();
                        addFormatCollect(strReportParameters, policyData);
                    }

                }
                //Se valida que tipo de reporte se seleccionó.
                else if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY))
                {
                    /* Se valida si existe texto a mostrar en la segunda hoja o si la cantidad
                     * es amparos es mayor o igual a 6. Cuando la cantidad de amparos es mayor o 
                     * igual a 6, en la primera hoja se alcanza a mostrar toda la información.*/
                    Paths hj1Crop = new Paths(rutas.Count);
                    hj1Crop.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Crop);
                    strPaths[0] = hj1Crop.getPath("LocationCover", (RiskCount > 1));
                    strPaths[1] = hj1Crop.FilePath;

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    if ((lc.SecondPage > 4) || (lc.CoveragesCount > 5))
                    {

                        /* En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
                         * Se valida si muestra debe mostrar el convenio o no.*/
                        if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                            (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY))
                        {
                            Paths hj2Crop = new Paths(rutas.Count);
                            hj2Crop.setFileName(policyData, ".pdf");
                            rutas.Add(hj2Crop);
                            //TODO: <<Edgar O. Piraneque E.; 17/02/2011; Asunto: Manejo de multiriesgos
                            //NameValue[] LocationSpParams = new NameValue[1];
                            LocationSpParams[0] = new NameValue("PROCESS_ID", policyData.ProcessId);
                            DataSet dsInfo;
                            //SP que llenará las tablas para los reportes.
                            dsInfo = ReportServiceHelper.getData("REPORT.CO_DELETE_MULTIRISK", LocationSpParams);
                            //RiskCount = int.Parse(dsInfo.Tables[0].Rows[0].ToString());
                            //Edgar O. Piraneque E.; 17/02/2011;>>
                            //TODO: <<Autor: Edgar Cervantes De Los Rios; Fecha: 29/11/2010; Asunto: Al imprimir un temporario del ramo multiriesgo, se genera desbordamiento por la cantidad coberturas.; Compañía: 1.
                            strPaths[0] = hj2Crop.getPath("LocationCoverAppendix", (RiskCount > 1));
                            /* Autor: Edgar Cervantes De Los Rios, Fecha: 29/11/2010 >>*/
                            strPaths[1] = hj2Crop.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            /* Reporte Convenio de Pago. Se carga la ruta del reporte a cargar.*/
                            ///HD-2181
                            if (lc.PrintConvection == 1)
                            {
                                //NameValue[] LocationSpParams = new NameValue[1];
                                LocationSpParams[0] = new NameValue("PROCESS_ID", policyData.ProcessId);
                                //DataSet dsInfo;
                                //SP que llenará las tablas para los reportes.
                                dsInfo = ReportServiceHelper.getData("REPORT.CO_DELETE_MULTIRISK", LocationSpParams);

                                Paths hj3Crop = new Paths(rutas.Count);
                                hj3Crop.setFileName(policyData, ".pdf");
                                rutas.Add(hj3Crop);
                                //<<TODO: Autor: Edgar O. Piraneque E.; Compañía: 1; Fecha: 12/01/2011; Asunto: Se crea reporte para imprimir Plan de pagos
                                strPaths[0] = hj3Crop.getPath("LocationConvection", (RiskCount > 1));
                                //Autor: Edgar O. Piraneque E.; Compañía: 1; Fecha: 12/01/2011;>>
                                strPaths[1] = hj3Crop.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            }
                            //intPdfQuantity++;
                        }
                        else if (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY)
                        {
                            Paths hj4Crop = new Paths(rutas.Count);
                            hj4Crop.setFileName(policyData, ".pdf");
                            rutas.Add(hj4Crop);

                            //TODO: <<Autor: Edgar Cervantes De Los Rios; Fecha: 29/11/2010; Asunto: Al imprimir un temporario del ramo multiriesgo, se genera desbordamiento por la cantidad coberturas.; Compañía: 1.
                            strPaths[0] = hj4Crop.getPath("LocationCoverAppendix", (RiskCount > 1));
                            /* Autor: Edgar Cervantes De Los Rios, Fecha: 29/11/2010 >>*/
                            strPaths[1] = hj4Crop.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        }
                    }

                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2333 07/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (lc.PrintFormat == 1 && 1 != 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2333 ************************************************
                    }
                }
                else if (policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION)
                {
                    //Reporte Convenio de Pago.
                    ///HD-2181
                    if (lc.PrintConvection == 1)
                    {
                        // << TODO: Autor: Miguel López; Fecha: 07/03/2011; Asunto: HD-2872
                        /*
                        Paths hj6Crop = new Paths(rutas.Count);
                        hj6Crop.setFileName(policyData, ".pdf");
                        rutas.Add(hj6Crop);

                        strPaths[0] = hj6Crop.getPath("CropConvection", (RiskCount > 1));//strArrPaths[0] + ConfigurationSettings.AppSettings["VehicleConvectionFile"];
                        strPaths[1] = hj6Crop.FilePath;//strArrPaths[intPathIndex];
                         * */

                        //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                        /* Autor: Miguel López; Fecha: 07/03/2011*/
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2333 07/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (lc.PrintFormat == 1 && 1 != 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2333 ************************************************
                    }

                }
                else if ((policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                {
                    //TODO: <<Edgar O. Piraneque E.; 17/02/2011; Asunto: Manejo de multiriesgos

                    waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
                    Paths hj1Crop = new Paths(rutas.Count);
                    hj1Crop.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Crop);

                    strPaths[0] = hj1Crop.getPath("LocationQuotationCover", (RiskCount > 1));
                    strPaths[1] = hj1Crop.FilePath;

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    if (lc.PrintConvection == 1) /* || (lc.CoveragesCount > 5))*/
                    {
                        //
                        //NameValue[] LocationSpParams = new NameValue[1];
                        LocationSpParams[0] = new NameValue("PROCESS_ID", policyData.ProcessId);
                        DataSet dsInfo;
                        //SP que llenará las tablas para los reportes.
                        dsInfo = ReportServiceHelper.getData("REPORT.CO_DELETE_MULTIRISK", LocationSpParams);
                        //
                        Paths hj2Vehicle = new Paths(rutas.Count);
                        hj2Vehicle.setFileName(policyData, ".pdf");
                        rutas.Add(hj2Vehicle);

                        strPaths[0] = hj2Vehicle.getPath("LocationQuotationAppendix", (RiskCount > 1));
                        strPaths[1] = hj2Vehicle.FilePath;

                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // << TODO: Autor: Miguel López; Fecha: 20/08/2010; Asunto: Incluimos los métodos que generan los reportes
        //                                                          para los ramos RC Pasajeros y Caución y los exportamos
        //                                                          a PDF.
        /// <summary>
        /// Genera los reportes para el ramo de RC Pasajeros y los exporta a .pdf.
        /// </summary>
        /// <param name="rc">Datos del reporte de RCP.</param>
        /// <param name="policyData">Datos de la póliza</param>
        private void showPassengerReport(PassengerCrop pc, Policy policyData, int intRiskNum, int intRiskType)
        {
            int intReportType = Convert.ToInt32(policyData.ReportType);
            if (intReportType == (int)ReportEnum.ReportType.MASS_LOAD)
            {
                intReportType = (int)ReportEnum.ReportType.ONLY_POLICY;
            }
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);

            //Para saber si la marca de agua debe ser de Temporarios o de Cotizaciones
            if (policyData.TempNum == 0)
            {
                waterMark = "";
            }
            else
            {
                if (policyData.QuotationId != 0)
                {
                    waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
                }
                else
                {
                    waterMark = (policyData.TempNum == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];
                }
            }

            //Parametros reportes RC Pasajeros
            string[] strReportParameters = new string[10];
            strReportParameters[0] = PolicyData.ProcessId.ToString();
            strReportParameters[1] = pc.MinRow.ToString();
            strReportParameters[2] = PolicyData.CodeBar;
            strReportParameters[3] = PolicyData.TempNum.ToString();
            strReportParameters[4] = (intRiskNum != 0) ? intRiskNum.ToString() : policyData.EndorsementId.ToString();
            strReportParameters[5] = policyData.EndorsementId.ToString();

            string[] strPaths = new string[3];//Rutas de generación de reporte

            try
            {
                if (intReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT)
                {
                    if (pc.PrintFormat == 1)
                    {
                        strReportParameters[4] = RiskCount.ToString();
                        addFormatCollect(strReportParameters, policyData);
                    }

                }
                // << TODO: Autor: Miguel López; Fecha: 04/10/2010; Asunto: Este código se ejecuta cuando se hace la impresión de carnet para
                //                                                          el ramo RC Pasajeros
                //else if (policyData.ReportType == (int)ReportEnum.ReportType.LICENSE || policyData.ReportType == (int)ReportEnum.ReportType.LICENSE_BLANK)
                //{
                //    IsLicense = true;
                //    string[] strLicenseParameters = new string[10];
                //    strLicenseParameters[0] = PolicyData.ProcessId.ToString();
                //    strLicenseParameters[1] = PolicyData.LicensePlate;
                //    strLicenseParameters[2] = PolicyData.ShowPremium.ToString();
                //    strLicenseParameters[3] = PolicyData.CopiesQuantity.ToString();
                //    strLicenseParameters[4] = PolicyData.PolicyId.ToString();
                //    strLicenseParameters[5] = PolicyData.EndorsementId.ToString();
                //    strLicenseParameters[6] = PolicyData.TempNum.ToString();

                //    Paths hjCCrop = new Paths(rutas.Count);

                //    if (policyData.ReportType == (int)ReportEnum.ReportType.LICENSE)
                //    {
                //        hjCCrop.setFileName(policyData, ".pdf");
                //        rutas.Add(hjCCrop);
                //        strPaths[0] = hjCCrop.getPath("PassengerLicense", (RiskCount > 1));
                //        strPaths[1] = hjCCrop.FilePath;
                //    }
                //    else
                //    {
                //        hjCCrop.PathId -= 1;

                //        hjCCrop.setBlankFileName(policyData, ".pdf",policyData.ProcessId,"B");
                //        rutas.Add(hjCCrop);
                //        strPaths[0] = hjCCrop.getPath("PassengerLicenseBlank", (RiskCount > 1));
                //        strPaths[1] = hjCCrop.FilePath;
                //    }

                //    ReportServiceHelper.loadReportFile(strLicenseParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);


                //}
                /* Autor: Miguel López; Fecha: 04/10/2010 >>*/

                //Se valida que tipo de reporte se seleccionó.
                else
                if ((intReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                    (intReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                    (intReportType == (int)ReportEnum.ReportType.TEMPORARY))
                {
                    /* Se valida si existe texto a mostrar en la segunda hoja o si la cantidad
                     * es amparos es mayor o igual a 6. Cuando la cantidad de amparos es mayor o 
                     * igual a 6, en la primera hoja se alcanza a mostrar toda la información.*/
                    Paths hj1Crop = new Paths(rutas.Count);
                    hj1Crop.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Crop);

                    strPaths[0] = hj1Crop.getPath("PassengerCover", (RiskCount > 1));
                    strPaths[1] = hj1Crop.FilePath;

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    if (pc.SecondPage != 0)
                    {

                        /* En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
                         * Se valida si muestra debe mostrar el convenio o no.*/
                        if ((intReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                            (intReportType == (int)ReportEnum.ReportType.TEMPORARY))
                        {
                            Paths hj2Crop = new Paths(rutas.Count);
                            hj2Crop.setFileName(policyData, ".pdf");
                            rutas.Add(hj2Crop);

                            strPaths[0] = hj2Crop.getPath("PassengerCoverAppendix", (RiskCount > 1));
                            strPaths[1] = hj2Crop.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            /* <<TODO: Autor: Edgar Piraneque; Compañía: 1; 07/04/2011;
                             * AsuntoPor solicitud de Arelis, se retira la hoja de convenio de pago en RC pasajeros
                             * Reporte Convenio de Pago. Se carga la ruta del reporte a cargar.*/
                            ///HD-2181
                            //if (pc.PrintConvection == 1)
                            //{
                            //    Paths hj3Crop = new Paths(rutas.Count);
                            //    hj3Crop.setFileName(policyData, ".pdf");
                            //    rutas.Add(hj3Crop);

                            //    strPaths[0] = hj3Crop.getPath("PassengerConvection", (RiskCount > 1));
                            //    strPaths[1] = hj3Crop.FilePath;

                            //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            //}
                            //intPdfQuantity++;
                        }
                        //else if (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY)
                        //{
                        //    Paths hj2Crop = new Paths(rutas.Count);
                        //    hj1Crop.setFileName(policyData, ".pdf");
                        //    rutas.Add(hj2Crop);

                        //    strPaths[0] = hj2Crop.getPath("PassengerCover", (RiskCount > 1));
                        //    strPaths[1] = hj2Crop.FilePath;

                        //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        //}
                        // Autor: Edgar Piraneque; Compañía: 1; 07/04/2011; >>
                    }

                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2333 07/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (pc.PrintFormat == 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2333 ************************************************
                    }
                }
                else if (intReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION)
                {
                    //Reporte Convenio de Pago.
                    ///HD-2181
                    if (pc.PrintConvection == 1)
                    {
                        Paths hj6Crop = new Paths(rutas.Count);
                        hj6Crop.setFileName(policyData, ".pdf");
                        rutas.Add(hj6Crop);

                        strPaths[0] = hj6Crop.getPath("PassengerConvection", (RiskCount > 1));//strArrPaths[0] + ConfigurationSettings.AppSettings["VehicleConvectionFile"];
                        strPaths[1] = hj6Crop.FilePath;//strArrPaths[intPathIndex];

                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2333 07/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (pc.PrintFormat == 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2333 ************************************************
                    }

                }
                //else if ((policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                //{
                //    Paths hj1Crop = new Paths(rutas.Count);
                //    hj1Crop.setFileName(policyData, ".pdf");
                //    rutas.Add(hj1Crop);

                //    strPaths[0] = hj1Crop.getPath("CropQuotationCover", (RiskCount > 1));
                //    strPaths[1] = hj1Crop.FilePath;

                //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Genera los reportes para el ramo de Caución y los exporta a .pdf.
        /// </summary>
        /// <param name="rc">Datos del reporte de CN.</param>
        /// <param name="policyData">Datos de la póliza</param>
        private void showCautionReport(Caution bd, Policy policyData)
        {
            int intReportType = Convert.ToInt32(policyData.ReportType);
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);

            //Para saber si la marca de agua debe ser de Temporarios o de Cotizaciones
            if (policyData.TempNum == 0)
            {
                waterMark = "";
            }
            else
            {
                if (policyData.QuotationId > 0)
                {
                    waterMark = ConfigurationSettings.AppSettings["QuoWaterMark"];
                }
                else
                {
                    waterMark = (policyData.TempNum == 0) ? "" : ConfigurationSettings.AppSettings["WaterMark"];
                }
            }

            //Parametros reportes Caución

            //desarrollo de generacion de codigo de barras
            BarcodeGenerator Barcode = new BarcodeGenerator();

            String pathBarCodeImage = String.Empty;
            String dataBarCode = String.Empty;
            String fileBarCode = String.Empty;
            String pathBarCode = ConfigurationSettings.AppSettings["pathBarCode"].ToString();

            fileBarCode = policyData.PrefixNum.ToString() +
                        policyData.LicensePlate.ToString() +
                        policyData.EndorsementId.ToString() + ".png";
            pathBarCodeImage = pathBarCode + fileBarCode;

            dataBarCode = policyData.PrefixNum.ToString().PadLeft(4, '0') + ";" + policyData.LicensePlate.ToString().PadLeft(6, '0') + ";" + policyData.EndorsementId.ToString().PadLeft(10, '0');

            Barcode.createBarCodeGS1Image(dataBarCode, pathBarCodeImage);
            //hasta aqui el desarrollo de codigo de barras

            policyData.CodeBar = pathBarCodeImage;

            string[] strReportParameters = new string[10];
            strReportParameters[0] = policyData.ProcessId.ToString();
            strReportParameters[1] = bd.SecondPage.ToString();
            strReportParameters[2] = policyData.CodeBar;
            strReportParameters[3] = policyData.TempNum.ToString();
            strReportParameters[4] = bd.ShowPaymentCert.ToString();
            strReportParameters[5] = bd.TextLinesCount.ToString();
            strReportParameters[6] = bd.BeneficiariesCount.ToString();
            strReportParameters[7] = ((int)GetPolicyCoInsurance(policyData.TempNum, policyData.PolicyId, policyData.EndorsementId)).ToString();
            strReportParameters[8] = string.Empty;

            string[] strPaths = new string[3];//Rutas de generación de reporte

            try
            {
                //Se valida que tipo de reporte se seleccionó.
                //Evalua tipo de reporte
                if (intReportType == (int)ReportEnum.ReportType.FORMAT_COLLECT)
                {
                    ///HD-2317 04/05/2010
                    ///Autor: Edgar O. Piraneque E.
                    ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                    ///esto implica si se imprime o no el formato de recaudo
                    if (bd.PrintFormat == 1)
                    {
                        strReportParameters[4] = RiskCount.ToString();
                        addFormatCollect(strReportParameters, policyData);
                    }
                    ///FIN HD-2317 ************************************************
                }
                else if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                    (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                {
                    List<string> policyObject = GetTextPages(policyData, 13, 2, 100, true);

                    if (policyData.PolicyId > 0 && policyData.EndorsementId > 0 && policyData.CurrentFromFirst)
                    {
                        CurrentoFromFirstEndorsement(policyData);
                    }

                    if (policyData.TempAuthorization)
                    {
                        //policyObjectEndorsement = GetTextPagesEndorsementAutho(policyData, 13, 2, 100);
                        strReportParameters[8] = policyObject[1];
                    }

                    //bool tempAutho = ValidateTempAuthorization(policyData.TempNum);
                    Paths hj1Caution = new Paths(rutas.Count);
                    hj1Caution.setFileName(policyData, ".pdf");
                    rutas.Add(hj1Caution);

                    if (policyData.TempAuthorization)
                    {
                        strPaths[0] = hj1Caution.getPath("CautionCoverAutho", (RiskCount > 1));
                    }
                    else
                    {
                        strPaths[0] = hj1Caution.getPath("CautionArticleCover", (RiskCount > 1));
                    }

                    // << TODO: Autor: Miguel López; Fecha: 04/10/2010; Asunto: Discriminamos el tipo de póliza para el ramo Caución.
                    //                                                          Si el tipo corresponde al artículo 513, se usa el formato
                    //                                                          establecido para dicho artículo. En caso contrario
                    //                                                          se usa el formato estándar de Caución.

                    //TODO: Autor: John Ruiz; Fecha: 22/05/2012; Asunto: Se comenta ya que la caratula es la misma para todos los casos
                    //if (IsArticle(policyData.ProcessId,Convert.ToInt32(GetParameterValue(6009,typeof(System.Int32)))))
                    //{
                    //strPaths[0] = hj1Caution.getPath("CautionArticleCover", (RiskCount > 1));
                    //}
                    //else 
                    //{
                    //    strPaths[0] = hj1Caution.getPath("CautionCover", (RiskCount > 1));
                    //}
                    /* Autor: Miguel López; Fecha: 04/10/2010 >>*/

                    strPaths[1] = hj1Caution.FilePath;

                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                    //Valida si la cantidad de líneas a mostrar de texto es diferente a cero.

                    //En caso de ser mayor de 5, los beneficiarios se muestran en el reporte de anexos.
                    //Se valida si muestra debe mostrar el convenio o no.
                    if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                        (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                        (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                    {
                        int policyObjectCount = 0;
                        if (policyData.TempAuthorization)
                        {
                            policyObjectCount = 2;
                        }
                        else
                        {
                            policyObjectCount = 1;
                        }
                        if (policyObject.Count > policyObjectCount && policyObject.Where(x => !x.Equals(string.Empty)).Count() > 0)
                        {
                            List<string> listText = new List<string>();
                            if (policyObject[policyObjectCount].Length > 62650)
                            {
                                listText = GetTextMaximumCharacter(policyObject[policyObjectCount]);
                            }
                            if (listText.Count > 0)
                            {
                                foreach (var text in listText)
                                {
                                    Paths hj2Surety = new Paths(rutas.Count);
                                    hj2Surety.setFileName(policyData, ".pdf");
                                    rutas.Add(hj2Surety);

                                    strPaths[0] = hj2Surety.getPath("CautionCoverObservationAppendix", (RiskCount > 1));
                                    strPaths[1] = hj2Surety.FilePath;

                                    strReportParameters[7] = policyObject.Count > 0 ? policyObject[policyObjectCount] : string.Empty;

                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                                }
                            }
                            else
                            {
                                Paths hj2Surety = new Paths(rutas.Count);
                                hj2Surety.setFileName(policyData, ".pdf");
                                rutas.Add(hj2Surety);

                                strPaths[0] = hj2Surety.getPath("CautionCoverObservationAppendix", (RiskCount > 1));
                                strPaths[1] = hj2Surety.FilePath;

                                strReportParameters[7] = policyObject.Count > 0 ? policyObject[policyObjectCount] : string.Empty;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                            }
                        }
                        if (policyData.TempAuthorization && ValidateCountTempAuthorization(policyData.TempNum))
                        {
                            Paths hj3Crop = new Paths(rutas.Count);
                            hj3Crop.setFileName(policyData, ".pdf");
                            rutas.Add(hj3Crop);

                            strPaths[0] = hj3Crop.getPath("AuthoAppendix", (RiskCount > 1));
                            strPaths[1] = hj3Crop.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                        }
                        if (bd.SecondPage != 0)
                        {
                            //Desactivamos temporalmente la impresión de hoja anexa con el texto extendido
                            //Paths hjTextoCaution = new Paths(rutas.Count);
                            //hjTextoCaution.setFileName(policyData, ".pdf");
                            //rutas.Add(hjTextoCaution);

                            //strPaths[0] = hjTextoCaution.getPath("CautionCoverTextAppendix", (RiskCount > 1));
                            //strPaths[1] = hjTextoCaution.FilePath;

                            //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            //Si la póliza tiene cláusulas, imprimimos la Hoja de cláusulas
                            if (ClausesExist(policyData))
                            {
                                //Paths hj2Caution = new Paths(rutas.Count);
                                //hj2Caution.setFileName(policyData, ".pdf");
                                //rutas.Add(hj2Caution);

                                //strPaths[0] = hj2Caution.getPath("CautionCoverAppendix", (RiskCount > 1));
                                //strPaths[1] = hj2Caution.FilePath;

                                //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            }
                        }
                        //Reporte Convenio de Pago.
                        //HD-2181
                        if (bd.PrintConvection == 1)
                        {
                            if (HasCoinsuranceAssigned(policyData) && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                            {
                                Paths hj3Caution = new Paths(rutas.Count);
                                hj3Caution.setFileName(policyData, ".pdf");
                                rutas.Add(hj3Caution);

                                strPaths[0] = hj3Caution.getPath("CautionConvection", (RiskCount > 1));
                                strPaths[1] = hj3Caution.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            }

                            if (policyData.ReportType != (int)ReportEnum.ReportType.TEMPORARY ||
                                (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                            {
                                if (policyData.PrintPromissoryNote == 1)
                                {
                                    #region Impresión de Pagaré y Carta de Instrucciones
                                    //CARGAMOS LOS DATOS DEL ASEGURADO
                                    GetInsuredData(policyData);
                                    //EVALUAMOS SI EL ASEGURADO ES UN CONSORCIO
                                    Paths pathPagare;
                                    if (IsConsortium())
                                    {
                                        //CARTA DE INSTRUCCIONES
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteCoverConsortium", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                        //FORMATO PAGARE
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteConsortium", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    }
                                    else
                                    {
                                        //SI NO ES CONSORCIO VERIFICAMOS SI EL ASEGURADO ES PERSONA O COMPAÑIA
                                        if (IsPerson())
                                        {
                                            //CARTA DE INSTRUCCIONES
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNoteCoverPerson", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                            //FORMATO PAGARE
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNotePerson", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                        }
                                        else if (IsCompany())
                                        {
                                            //CARTA DE INSTRUCCIONES
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNoteCoverCompany", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                            //FORMATO PAGARE
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNoteCompany", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                        }
                                    }
                                    #endregion
                                }
                            }
                        }

                        //TODO: Fecha:02/11/2011 ;Autor: John Ruiz; Asunto: Se imprime forma de producto ; Compañia: 3
                        if (isExpedition(policyData.EndorsementId))
                        {
                            string FormName = GetFormName(policyData.PolicyId);

                            if (!FormName.Equals(string.Empty))
                            {
                                string FormPdfFile = ConfigurationSettings.AppSettings["FormsPath"].ToString() + FormName + ".pdf";
                                if (System.IO.File.Exists(FormPdfFile))
                                {
                                    Paths form = new Paths(rutas.Count);
                                    form.setFileName(policyData, ".pdf");
                                    rutas.Add(form);

                                    System.IO.File.Copy(FormPdfFile, form.FilePath);
                                }
                            }
                        }
                    }
                    else if ((policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY))
                    {
                        //Reporte de Anexo de la Poliza.
                        if (bd.SecondPage != 0)
                        {
                            //Desactivamos temporalmente la impresión de hoja anexa con el texto extendido
                            //Paths hjTextoCaution = new Paths(rutas.Count);
                            //hjTextoCaution.setFileName(policyData, ".pdf");
                            //rutas.Add(hjTextoCaution);

                            //strPaths[0] = hjTextoCaution.getPath("CautionCoverTextAppendix", (RiskCount > 1));
                            //strPaths[1] = hjTextoCaution.FilePath;

                            //ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            //Impresión de la hoja de cláusulas
                            if (ClausesExist(policyData))
                            {
                                Paths hj4Caution = new Paths(rutas.Count);
                                hj4Caution.setFileName(policyData, ".pdf");
                                rutas.Add(hj4Caution);

                                strPaths[0] = hj4Caution.getPath("CautionCoverAppendix", (RiskCount > 1));
                                strPaths[1] = hj4Caution.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            }
                        }

                    }

                    else if ((policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION) ||
                             (policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                             (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY) ||
                             (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                    {
                        //Reporte Convenio de Pago.
                        //HD-2181
                        if (bd.PrintConvection == 1)
                        {
                            if (HasCoinsuranceAssigned(policyData) && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                            {
                                Paths hj5Caution = new Paths(rutas.Count);
                                hj5Caution.setFileName(policyData, ".pdf");
                                rutas.Add(hj5Caution);

                                strPaths[0] = hj5Caution.getPath("CautionConvection", (RiskCount > 1));
                                strPaths[1] = hj5Caution.FilePath;

                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            }

                            if (policyData.ReportType != (int)ReportEnum.ReportType.TEMPORARY ||
                                (policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                            {
                                if (policyData.PrintPromissoryNote == 1)
                                {
                                    #region Impresión de Pagaré y Carta de Instrucciones
                                    //CARGAMOS LOS DATOS DEL ASEGURADO
                                    GetInsuredData(policyData);
                                    //EVALUAMOS SI EL ASEGURADO ES UN CONSORCIO
                                    Paths pathPagare;
                                    if (IsConsortium())
                                    {
                                        //CARTA DE INSTRUCCIONES
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteCoverConsortium", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                        //FORMATO PAGARE
                                        pathPagare = new Paths(rutas.Count);
                                        pathPagare.setFileName(policyData, ".pdf");
                                        rutas.Add(pathPagare);
                                        strPaths[0] = pathPagare.getPath("PromissoryNoteConsortium", (RiskCount > 1));
                                        strPaths[1] = pathPagare.FilePath;
                                        ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    }
                                    else
                                    {
                                        //SI NO ES CONSORCIO VERIFICAMOS SI EL ASEGURADO ES PERSONA O COMPAÑIA
                                        if (IsPerson())
                                        {
                                            //CARTA DE INSTRUCCIONES
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNoteCoverPerson", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                            //FORMATO PAGARE
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNotePerson", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                        }
                                        else if (IsCompany())
                                        {
                                            //CARTA DE INSTRUCCIONES
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNoteCoverCompany", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                            //FORMATO PAGARE
                                            pathPagare = new Paths(rutas.Count);
                                            pathPagare.setFileName(policyData, ".pdf");
                                            rutas.Add(pathPagare);
                                            strPaths[0] = pathPagare.getPath("PromissoryNoteCompany", (RiskCount > 1));
                                            strPaths[1] = pathPagare.FilePath;
                                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2317 04/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (bd.PrintFormat == 1 && 1 != 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2317 ************************************************
                    }

                }
                else if (policyData.ReportType == (int)ReportEnum.ReportType.PAYMENT_CONVENTION)//Opción 3
                {
                    //Reporte Convenio de Pago.
                    //HD-2181
                    if (bd.PrintConvection == 1)
                    {
                        if (HasCoinsuranceAssigned(policyData) && (policyData.ReportType != (int)ReportEnum.ReportType.QUOTATION))
                        {
                            Paths hj6Caution = new Paths(rutas.Count);
                            hj6Caution.setFileName(policyData, ".pdf");
                            rutas.Add(hj6Caution);

                            strPaths[0] = hj6Caution.getPath("CautionConvection", (RiskCount > 1));
                            strPaths[1] = hj6Caution.FilePath;

                            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                        }

                        //HD-3356: Suprimimos la hoja de convenio de pago en Caución y la sustituimos por el formato de pagaré
                        if (policyData.PrintPromissoryNote == 1)
                        {
                            #region Impresión de Pagaré y Carta de Instrucciones
                            //CARGAMOS LOS DATOS DEL ASEGURADO
                            GetInsuredData(policyData);
                            //EVALUAMOS SI EL ASEGURADO ES UN CONSORCIO
                            Paths pathPagare;
                            if (IsConsortium())
                            {
                                //CARTA DE INSTRUCCIONES
                                pathPagare = new Paths(rutas.Count);
                                pathPagare.setFileName(policyData, ".pdf");
                                rutas.Add(pathPagare);
                                strPaths[0] = pathPagare.getPath("PromissoryNoteCoverConsortium", (RiskCount > 1));
                                strPaths[1] = pathPagare.FilePath;
                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                //FORMATO PAGARE
                                pathPagare = new Paths(rutas.Count);
                                pathPagare.setFileName(policyData, ".pdf");
                                rutas.Add(pathPagare);
                                strPaths[0] = pathPagare.getPath("PromissoryNoteConsortium", (RiskCount > 1));
                                strPaths[1] = pathPagare.FilePath;
                                ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                            }
                            else
                            {
                                //SI NO ES CONSORCIO VERIFICAMOS SI EL ASEGURADO ES PERSONA O COMPAÑIA
                                if (IsPerson())
                                {
                                    //CARTA DE INSTRUCCIONES
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteCoverPerson", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;
                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    //FORMATO PAGARE
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNotePerson", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;
                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                }
                                else if (IsCompany())
                                {
                                    //CARTA DE INSTRUCCIONES
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteCoverCompany", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;
                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                    //FORMATO PAGARE
                                    pathPagare = new Paths(rutas.Count);
                                    pathPagare.setFileName(policyData, ".pdf");
                                    rutas.Add(pathPagare);
                                    strPaths[0] = pathPagare.getPath("PromissoryNoteCompany", (RiskCount > 1));
                                    strPaths[1] = pathPagare.FilePath;
                                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);

                                }
                            }
                            #endregion
                        }
                    }
                    if ((policyData.WithFormatCollect) && (policyData.IdPv2g > 0))
                    {
                        ///HD-2317 04/05/2010
                        ///Autor: Edgar O. Piraneque E.
                        ///Descripción: Validación del estado que indica si el valor de la prima es positivo
                        ///esto implica si se imprime o no el formato de recaudo
                        if (bd.PrintFormat == 1)
                        {
                            strReportParameters[4] = "1";
                            addFormatCollect(strReportParameters, policyData);
                        }
                        ///FIN HD-2317 ************************************************
                    }

                }
                //else if ((policyData.ReportType == (int)ReportEnum.ReportType.QUOTATION))
                //{
                //    Paths hj1Surety = new Paths(rutas.Count);
                //    hj1Surety.setFileName(policyData, ".pdf");
                //    rutas.Add(hj1Surety);

                //    strPaths[0] = hj1Surety.getPath("SuretyQuotationCover", (RiskCount > 1));
                //    strPaths[1] = hj1Surety.FilePath;

                //    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* Autor: Miguel López; Fecha: 20/08/2010 >>*/

        /// <summary>
        /// Genera caratula del reporte de poliza colectiva
        /// </summary>
        /// <param name="policyData">Datos de la poliza</param>
        /// <param name="paramsp">Parametros del SP</param>
        /// <returns>Ruta de la caratula</returns>

        public void showCollectivePolicyCoverReport(Policy policyData, NameValue[] paramsp)
        {
            // <<TODO: Autor: Edgar Piraneque; Compañía: 1; 21/12/2010; Genera reporte de pólizas de vehículos con sustitución    
            string[] strDataPolicyCover = new string[3];
            DataSet dsInfo;
            DataTable dt1, dt2;
            Paths final = new Paths(rutas.Count);
            final.setFileName(policyData, ".pdf");
            rutas.Add(final);
            //Inicializa variables locales
            string[] strPaths = new string[2];
            string[] strReportParameters = new string[5];
            dsInfo = ReportServiceHelper.getData("REPORT.CO_SINGLE_VEHICLE_POLICY_COVER", paramsp);
            dt1 = dsInfo.Tables[0];
            dt2 = dsInfo.Tables[1];
            DataRow dtRow = dsInfo.Tables[0].Rows[0];
            Vehicle vc = new Vehicle(dt1, dt2);
            strDataPolicyCover[0] = vc.MinRow.ToString();
            strDataPolicyCover[1] = vc.RowCount.ToString();
            MaxRow = (int)dtRow["MAX_ROW"];
            if (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY)
            {
                waterMark = ConfigurationSettings.AppSettings["WaterMark"];
            }
            else
            {
                waterMark = string.Empty;
            }
            if ((policyData.ReportType != (int)ReportEnum.ReportType.FORMAT_COLLECT))
            {
                if ((policyData.ReportType == (int)ReportEnum.ReportType.COMPLETE_POLICY) ||
                (policyData.ReportType == (int)ReportEnum.ReportType.ONLY_POLICY) ||
                (policyData.ReportType == (int)ReportEnum.ReportType.TEMPORARY))
                {
                    Paths caratula = new Paths(rutas.Count);
                    caratula.setFileName(policyData, ".pdf");
                    rutas.Add(caratula);

                    strPaths[0] = caratula.getPath("CoverVehicleCover", true);

                    strPaths[1] = caratula.FilePath;

                    strReportParameters[0] = policyData.ProcessId.ToString();
                    strReportParameters[1] = MaxRow.ToString();
                    strReportParameters[2] = policyData.CodeBar;
                    strReportParameters[3] = policyData.TempNum.ToString();
                    strReportParameters[4] = "0";

                    //Se carga la ruta del reporte a cargar.
                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    // <<TODO: Edgar Piraneque; 27/12/2010; Asunto: Inclusión de página con clausulas de sustitución
                    Paths clausula = new Paths(rutas.Count);
                    clausula.setFileName(policyData, ".pdf");
                    rutas.Add(clausula);

                    strPaths[0] = clausula.getPath("CoverVehicleCoverAppendix", true);

                    strPaths[1] = clausula.FilePath;

                    strReportParameters[0] = policyData.ProcessId.ToString();
                    strReportParameters[1] = vc.MinRow.ToString();
                    strReportParameters[2] = policyData.CodeBar;
                    strReportParameters[3] = policyData.TempNum.ToString();
                    strReportParameters[4] = policyData.EndorsementId.ToString();

                    //Se carga la ruta del reporte a imprimir.
                    ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
                    //TODO: Edgar Piraneque; 27/12/2010>>
                }

                if ((vc.RegisterCount != 0))
                {
                    if (policyData.ReportType != (int)ReportEnum.ReportType.PAYMENT_CONVENTION)
                    {
                        showCollectivePolicyRisks(PolicyData);
                    }
                    else
                    {
                        if (vc.PrintConvection == 1)
                        {
                            Paths caratula = new Paths();
                            caratula.setFileName(policyData, ".pdf");
                            rutas.Add(caratula);
                            strDataPolicyCover[2] = " ";
                        }
                    }
                }
                if (vc.PrintFormat.Equals(1) && PolicyData.WithFormatCollect == true)
                {
                    string[] strReportParametersFr = new string[10];
                    strReportParametersFr[0] = policyData.ProcessId.ToString();
                    strReportParametersFr[1] = vc.MinRow.ToString();
                    strReportParametersFr[2] = policyData.CodeBar;
                    strReportParametersFr[3] = policyData.TempNum.ToString();
                    strReportParametersFr[4] = "0";
                    strReportParametersFr[5] = policyData.EndorsementId.ToString();
                    strReportParametersFr[6] = ReportServiceHelper.reference2.ToString();
                    addFormatCollect(policyData, strReportParametersFr);
                }
            }
            else
            {
                string[] strReportParametersFr = new string[10];
                strReportParametersFr[0] = policyData.ProcessId.ToString();
                strReportParametersFr[1] = vc.MinRow.ToString();
                strReportParametersFr[2] = policyData.CodeBar;
                strReportParametersFr[3] = policyData.TempNum.ToString();
                strReportParametersFr[4] = "0";
                strReportParametersFr[5] = policyData.EndorsementId.ToString();
                strReportParametersFr[6] = ReportServiceHelper.reference2.ToString();
                if (vc.PrintFormat.Equals(1))
                {
                    addFormatCollect(policyData, strReportParametersFr);
                }
            }
            // Autor: Edgar Piraneque; Compañía: 1; 21/12/2010;>>
        }


        //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 23/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
        public void printGenerateRenewalTemplateAgent()
        {
            try
            {

                Policy policyRenewal = new Policy();
                policyRenewal.ProcessId = Convert.ToInt32(dsOutput.Tables["TemplateAgent"].Rows[0]["PrintProcessId"].ToString());
                policyRenewal.RenewalProcessId = Convert.ToInt32(dsOutput.Tables["TemplateAgent"].Rows[0]["RenewalProcessId"].ToString());
                policyRenewal.User = dsOutput.Tables["PolicyPrinting"].Rows[0]["User"].ToString();

                NameValue[] paramSp = new NameValue[1];
                paramSp[0] = new NameValue("RENEWALPROCESS_ID", Convert.ToInt32(policyRenewal.RenewalProcessId));

                FilePath = printTemplateAgent(policyRenewal, paramSp);
            }
            catch (Exception ex)
            {
                this.HasError = true;
                this.ErrorDescription = ex.ToString();
                throw ex;
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

        public string printTemplateAgent(Policy renewalPolicy, NameValue[] paramSp)
        {
            string reportPath = string.Empty;

            string[] strPaths = new string[2];

            Paths inicial = new Paths(rutas.Count);
            inicial.setFileName(renewalPolicy, ".pdf");
            rutas.Add(inicial);

            strPaths[0] = inicial.getPath("TemplateAgent", false);
            strPaths[1] = inicial.FilePath;

            // ReportServiceHelper.joinPdfFiles(rutas);

            string[] strReportParameters = new string[1];

            strReportParameters[0] = Convert.ToString(paramSp[0].Value);

            ReportServiceHelper.loadReportFile(strReportParameters, strDataConn, strPaths, string.Empty, ExportFormatType.PortableDocFormat);

            reportPath = ((Paths)rutas[0]).FilePath;
            return reportPath;
        }

        /* Autor: Luisa Fernanda Ramírez, Fecha: 23/12/2010 >>*/


        #endregion

        #region FormatCollect
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
            NameValue[] parametersp = new NameValue[1];
            Paths formatCollect = new Paths(rutas.Count);
            DataSet ds = new DataSet();
            formatCollect.setFileName(policyData, ".pdf");
            rutas.Add(formatCollect);

            strPaths[0] = formatCollect.getPath("FormatCollect", (RiskCount > 1));
            strPaths[1] = formatCollect.FilePath;
            paramBarCode = ReportServiceHelper.getBarCode128(rptParams, policyData.IdPv2g);
            rptParams[2] = paramBarCode[1];
            rptParams[5] = paramBarCode[0];
            // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
            rptParams[6] = policyData.BalancePremium.ToString();
            // Edgar O. Piraneque E.; 05/11/2010;>>
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
            NameValue[] parametersp = new NameValue[1];
            Paths formatCollect = new Paths(rutas.Count);
            DataSet ds = new DataSet();
            formatCollect.setFileName(policyData, ".pdf");
            rutas.Add(formatCollect);

            strPaths[0] = formatCollect.getPath("FormatCollect", (RiskCount > 1));
            strPaths[1] = formatCollect.FilePath;
            paramBarCode = ReportServiceHelper.getBarCode128(rptParams, policyData.IdPv2g);
            rptParams[2] = paramBarCode[1];
            rptParams[5] = paramBarCode[0];
            // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
            rptParams[6] = policyData.BalancePremium.ToString();
            // Edgar O. Piraneque E.; 05/11/2010;>>
            //ReportServiceHelper.loadReportFile(rptParams, strDataConn, strPaths, waterMark, ExportFormatType.PortableDocFormat);
            return strPaths[1];


        }

        #endregion

        public string PrintCounterGuarantees(int processID, int guaranteeID, int individualID)
        {
            DataSet dsInfo;
            DataTable dt1, dt2;
            string reportPath = string.Empty;

            string[] strPaths = new string[2];

            Paths inicial = new Paths(1);

            NameValue[] paramsp2 = new NameValue[3];
            paramsp2[0] = new NameValue("PROCESS_ID", processID);
            paramsp2[1] = new NameValue("GUARANTEE_ID", guaranteeID);
            paramsp2[2] = new NameValue("INDIVIDUAL_ID", individualID);

            dsInfo = ReportServiceHelper.getData("REPORT.CO_PROMISSORY_NOTE", paramsp2);

            //CARTA DE COMPROMISO
            if (GetGuaranteeTypeByIndividualId(guaranteeID, individualID) == (int)EnumGuaranteeCode.CARTA_DE_COMPROMISO)
            {
                strPaths[0] = inicial.getPath("PromissoryNoteLetterOfEngagement", false);
            }
            //COASEGURO
            else if (GetGuaranteeCoInsurance(guaranteeID, individualID))
            {
                strPaths[0] = inicial.getPath("PromissoryNoteCoinsurance", false);
            }
            //PAGARE ABIERTO
            else if (!GetGuaranteeClosedIndByRiskId(guaranteeID, individualID))
            {
                if (GetGuaranteeArrendamiento(guaranteeID, individualID) == (int)EnumPrefix.ARRENDAMIENTOS)
                {
                    strPaths[0] = inicial.getPath("PromissoryNoteArrendamiento", false);
                }
                else
                {
                    strPaths[0] = inicial.getPath("PromissoryNoteOpenedNew", false);
                }
            }
            //PAGARE CERRADO
            else
            {
                if (GetGuaranteeArrendamiento(guaranteeID, individualID) == (int)EnumPrefix.ARRENDAMIENTOS)
                {
                    strPaths[0] = inicial.getPath("PromissoryNoteArrendamiento", false);
                }
                else
                {
                    strPaths[0] = inicial.getPath("PromissoryNoteClosedNew", false);
                }
            }
           

            strPaths[1] = ConfigurationSettings.AppSettings["UserReportPath"] + DateTime.Now.ToString("ddMMyyyyHHmm") + ".pdf";

            string[] strReportParameters = new string[4];

            strReportParameters[0] = processID.ToString();
            strReportParameters[1] = individualID.ToString();
            strReportParameters[2] = processID.ToString();
            strReportParameters[3] = processID.ToString();

            DynamicDataAccess dda = new DynamicDataAccess();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(dda.DataBaseName);

            string[] strDataConnWa = new string[4];

            strDataConnWa[0] = builder.UserID;
            strDataConnWa[1] = builder.Password;
            strDataConnWa[2] = builder.DataSource;
            strDataConnWa[3] = builder.InitialCatalog;

            ReportServiceHelper.loadReportFile(strReportParameters, strDataConnWa, strPaths, string.Empty, ExportFormatType.PortableDocFormat);

            return strPaths[1];
        }

        private bool GetGuaranteeClosedIndByRiskId(int guaranteeID, int individualId)
        {
            bool ClosedInd = false;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetGuaranteeClosedIndBYRiskID, guaranteeID, individualId));

            if (result.Rows.Count > 0)
            {
                ClosedInd = Convert.ToBoolean(result.Rows[0]["CLOSED_IND"].ToString());
            }
            else
            {
                throw new Exception("No se encontro la contragarantia");
            }

            return ClosedInd;
        }

        private int GetGuaranteeTypeByIndividualId(int guaranteeID, int individualId)
        {
            int guaranteeCd = 0;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetGuaranteeTypeByIndividualId, guaranteeID, individualId));

            if (result.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(result.Rows[0]["GUARANTEE_CD"].ToString()))
                {
                    guaranteeCd = Convert.ToInt16(result.Rows[0]["GUARANTEE_CD"].ToString());
                }
            }

            return guaranteeCd;
        }

        private int GetGuaranteeArrendamiento(int guaranteeID, int individualId)
        {
            int prefix = 0;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetGuaranteeArrendamiento, guaranteeID, individualId));

            if (result.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(result.Rows[0]["PREFIX_CD"].ToString()))
                {
                    prefix = Convert.ToInt16(result.Rows[0]["PREFIX_CD"].ToString());
                }
            }

            return prefix;
        }

        private bool GetGuaranteeCoInsurance(int guaranteeID, int individualId)
        {
            bool coinsurance = false;
            DynamicDataAccess dda = new DynamicDataAccess();
            DataTable result = dda.ExecuteDataTable(string.Format(Querys.GetGuaranteeCoInsurance, guaranteeID, individualId));

            if (result.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(result.Rows[0]["ACCEPTED"].ToString())
                    || !string.IsNullOrEmpty(result.Rows[0]["ASSIGNED"].ToString()))
                {
                    coinsurance = true;
                }

            }

            return coinsurance;
        }
    }
}
