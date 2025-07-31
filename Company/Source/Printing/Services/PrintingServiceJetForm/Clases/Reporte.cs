using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    class Reporte : Reportes
    {
        # region Propiedades

        // Delegado para el manejo del evento de finalizacion de printGeneralPolicies
        public delegate void ReportCompletedHandler(object sender, EventArgs e);

        //Evento declarado de tipo delegado ReportCompletedHandler
        public event ReportCompletedHandler GenerationCompleted;

        private IReporte _iReporte;
        private DataSet _dsOutput;
        private DataSet _dsPolicyRisks;
        private int _asyncProcessId;
        private int _printProcessId;
        private decimal _fileSize;
        private bool _status;
        private bool _hasError;
        private string _errorDescription;
        private int _version;
        private int _riskCount;
        private int _rangeMinValue;
        private int _rangeMaxValue;
        private string _docTypeDescription;

        public IReporte iReporte
        {
            get { return _iReporte; }
            set { _iReporte = value; }
        }

        public DataSet DsOutput
        {
            get { return _dsOutput; }
            set { _dsOutput = value; }
        }

        public DataSet DsPolicyRisks
        {
            get { return _dsPolicyRisks; }
            set { _dsPolicyRisks = value; }
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

        public string DocTypeDescription
        {
            get { return _docTypeDescription; }
            set { _docTypeDescription = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Contructor del proceso de impresión del reporte
        /// </summary>
        /// <param name="DsOutput">Datos para la generación del reporte</param>
        /// <param name="StrDataConn">Datos de la conexión a la base de datos</param>
        public Reporte(DataSet dsOutput)
        {
            this.iniReport(dsOutput);
        }

        # endregion

        #region Methods

        /// <summary>
        /// Imprime el reporte
        /// </summary>
        public void print(bool IsMassive, bool isCollective)
        {
            try
            {
                int intPolicyType = 0;
                int CoveredRiskType = 0;

                if (PolicyData.PolicyId != 0 || PolicyData.TempNum != 0)
                {
                    //Número de riesgos, cantidad de coberturas, tipo de poliza y estados de riesgo.
                    int intRiskCount = DsPolicyRisks.Tables[0].Rows.Count;
                    RiskCount = PolicyData.RangeMaxValue - PolicyData.RangeMinValue + 1;

                    if (RiskCount > 0)
                    {
                        if (DsPolicyRisks.Tables[0].Rows.Count > 0 && DsPolicyRisks.Tables[0].Rows[0] != null)
                        {
                            intPolicyType = Convert.ToInt32(DsPolicyRisks.Tables[0].Rows[0]["POLICY_TYPE"].ToString());
                        }

                        if (DsPolicyRisks.Tables[1].Rows.Count > 0 && DsPolicyRisks.Tables[1].Rows[0] !=null )
                        {

                            CoveredRiskType = Convert.ToInt32(DsPolicyRisks.Tables[1].Rows[0]["COVERED_RISK_TYPE_CD"].ToString());
                        }

                        //Si la póliza es de tipo colectiva con un solo riesgo, ésta se debe imprimir como tipo individual.
                        if ((intPolicyType == (int)(PolicyType.COLLECTIVE)) && (intRiskCount == 1))
                            intPolicyType = (int)(PolicyType.INDIVIDUAL);
                    }

                    NameValue[] paramsp = new NameValue[9];
                    paramsp[0] = new NameValue("PROCESS_ID", Convert.ToInt32(PolicyData.ProcessId));

                    int? polOrQuo = (PolicyData.QuotationId != 0) ? PolicyData.QuotationId : Convert.ToInt32(PolicyData.PolicyId);
                    paramsp[1] = new NameValue("POLICY_ID", polOrQuo);
                    
                    paramsp[2] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(PolicyData.EndorsementId));
                    paramsp[3] = new NameValue("TEMP_ID", Convert.ToInt32(PolicyData.TempNum));
                    paramsp[4] = new NameValue("RISK_NUM", Convert.ToInt32(PolicyData.RangeMaxValue));
                    paramsp[5] = new NameValue("FIRST_RISK", Convert.ToInt32(PolicyData.RangeMinValue));
                    paramsp[6] = new NameValue("LAST_RISK", Convert.ToInt32(PolicyData.RangeMaxValue));
                    paramsp[7] = new NameValue("POLICY_TYPE", intPolicyType);
                    paramsp[8] = new NameValue("RISK_NUM_ANT", 0);

                    iReporte.create(PolicyData, paramsp, CoveredRiskType, this.OperationId, isCollective);
                }

                if (iReporte != null)
                {
                    // TODO: Julio Guzmán, 13/02/2012, PV3G04-AE12: Se elimina primero el archivo antes de crearlo
                    if (iReporte.PdfFilePath != null)
                    {
                        System.IO.File.Delete(iReporte.PdfFilePath);
                    }
                    if (!IsMassive)
                        ReportServiceHelper.WriteFile(iReporte.FileName + ".dat", ConfigurationSettings.AppSettings["JetFormServer"], iReporte.File);
                    else
                        ReportServiceHelper.WriteFile(iReporte.FileName + ".dat", ConfigurationSettings.AppSettings["MassiveJetFormServer"], iReporte.File);

                    this.Status = true;
                }
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
                    GenerationCompleted(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Inicializa el reporte con los datos de la solicitud
        /// </summary>
        /// <param name="dsOutput">Datos de la solicitud</param>
        private void iniReport(DataSet dsOutput)
        {
            this.DsOutput = dsOutput;
            this.PolicyData = new Policy(DsOutput.Tables["PolicyPrinting"].Rows[0]);
            this.DsPolicyRisks = ReportServiceHelper.getRiskCount(PolicyData);
            this.AsyncProcessId = Convert.ToInt32(DsOutput.Tables["PendingPrintData"].Rows[0]["AsyncProcessId"].ToString());
            this.PrintProcessId = Convert.ToInt32(DsOutput.Tables["PendingPrintData"].Rows[0]["PrintProcessId"].ToString());
            this.DocTypeDescription = _dsOutput.Tables["PendingPrintData"].Rows[0]["DocTypeDescription"].ToString();
            this.RangeMinValue = Convert.ToInt32(DsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMinValue"].ToString());
            this.RangeMaxValue = Convert.ToInt32(DsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMaxValue"].ToString());
            int range = (RangeMaxValue - RangeMinValue) + 1;
            this.FileSize = ReportServiceHelper.calculateFileSize(range);
            this.Status = false;
            this.HasError = false;
            this.ErrorDescription = string.Empty;
            this.Version = Convert.ToInt32(ConfigurationSettings.AppSettings["RptUsedVersion"]);
            this.RiskCount = DsPolicyRisks.Tables[0].Rows.Count;
        }

        /// <summary>
        /// Imprime el texto correspondiente al riesgo
        /// </summary>
        protected void printRisk() { }

        /// <summary>
        /// Imprime la coberturas de la póliza
        /// </summary>
        protected void printCoverages() { }

        /// <summary>
        /// 
        /// </summary>
        //protected override void initializeCoveragesDataTable() { }

        #endregion
    }
}
