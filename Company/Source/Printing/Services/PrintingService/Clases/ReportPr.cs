using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.PrintingServices.Enums;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    public class ReportPr
    {
        private int _asyncProcessId;
        private int _printProcessId;
        private int _requestId;
        private int _billingGroup;
        private int _policyId;
        private int _endorsementId;
        private int _quotationId;
        private int _reportType;
        private string _codeBar;
        private string _username;
        private int _tempId;
        private int _prefixId;
        private int _rangeMinValue;
        private int _rangeMaxValue;
        private string _processFromDate;
        private string _processToDate;
        private int _indivualId;
        private int _branchId;
        private bool _withFormatCollect;
        private bool _exportToExcel;
        private bool _printAsynchronously;
        private bool _currentFromFirst;
        private bool _endorsementText;
        private bool _tempAuthorization;

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

        public int RequestId
        {
            get { return _requestId; }
            set { _requestId = value; }
        }

        public int BillingGroup
        {
            get { return _billingGroup; }
            set { _billingGroup = value; }
        }

        public int PolicyId
        {
            get { return _policyId; }
            set { _policyId = value; }
        }

        public int EndorsementId
        {
            get { return _endorsementId; }
            set { _endorsementId = value; }
        }

        public int QuotationId
        {
            get { return _quotationId; }
            set { _quotationId = value; }
        }

        public int ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
        }

        public string CodeBar
        {
            get { return _codeBar; }
            set { _codeBar = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public int TempId
        {
            get { return _tempId; }
            set { _tempId = value; }
        }

        public int PrefixId
        {
            get { return _prefixId; }
            set { _prefixId = value; }
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

        public string ProcessFromDate
        {
            get { return _processFromDate; }
            set { _processFromDate = value; }
        }

        public string ProcessToDate
        {
            get { return _processToDate; }
            set { _processToDate = value; }
        }

        public int IndivualId
        {
            get { return _indivualId; }
            set { _indivualId = value; }
        }

        public int BranchId
        {
            get { return _branchId; }
            set { _branchId = value; }
        }

        public bool WithFormatCollect
        {
            get { return _withFormatCollect; }
            set { _withFormatCollect = value; }
        }

        public bool ExportToExcel
        {
            get { return _exportToExcel; }
            set { _exportToExcel = value; }
        }

        public bool PrintAsynchronously
        {
            get { return _printAsynchronously; }
            set { _printAsynchronously = value; }
        }

        public bool CurrentFromFirst
        {
            get { return _currentFromFirst; }
            set { _currentFromFirst = value; }
        }

        public bool EndorsementText
        {
            get { return _endorsementText; }
            set { _endorsementText = value; }
        }

        public bool TempAuthorization
        {
            get { return _tempAuthorization; }
            set { _tempAuthorization = value; }
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        public ReportPr()
        {
            AsyncProcessId = 0;
            PrintProcessId = 0;
            RequestId = 0;
            BillingGroup = 0;
            PolicyId = 0;
            EndorsementId = 0;
            QuotationId = 0;
            ReportType = (int)Sistran.Company.Application.PrintingServices.Enums.ReportType.COMPLETE_POLICY;
            CodeBar = string.Empty;
            Username = string.Empty;
            TempId = 0;
            PrefixId = 0;
            RangeMinValue = 0;
            RangeMaxValue = 0;
            ProcessFromDate = string.Empty;
            ProcessToDate = string.Empty;
            IndivualId = 0;
            BranchId = 0;
            WithFormatCollect = false;
            ExportToExcel = false;
            PrintAsynchronously = false;
            CurrentFromFirst = false;
            EndorsementText = false;
            TempAuthorization = false;
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="asyncProcessId">Id del proceso asincrono</param>
        /// <param name="printProcessId">Id del proceso de impresión</param>
        /// <param name="requestId">Id de la solicitud agrupadora</param>
        /// <param name="billingGroup">Agrupador</param>
        /// <param name="policyId">Id de la póliza que se imprime</param>
        /// <param name="endorsementId">Id del endoso que se imprime</param>
        /// <param name="quotationId">Id de la cotización que se imprime</param>
        /// <param name="reportType">Tipo de reporte a imprimir</param>
        /// <param name="codeBar">Cadena para el código de barras</param>
        /// <param name="username">Nombre del usuario que envia proceso</param>
        /// <param name="tempId">Id del temporario a imprimir</param>
        /// <param name="prefixId">Id del ramo</param>
        /// <param name="rangeMinValue">Valor mínimo del rango a imprimir</param>
        /// <param name="rangeMaxValue">Valor máximo del rango a imprimir</param>
        /// <param name="processFromDate">Fecha inicial del rango de pólizas migradas a imprimir</param>
        /// <param name="processToDate">Fecha final del rango de pólizas migradas a imprimir</param>
        /// <param name="indivualId">Id del intermediario que migra las pólizas</param>
        /// <param name="branchId">Id de la sucursal</param>
        /// <param name="withFormatCollect">Imprimir con formato de recaudo</param>
        /// <param name="exportToExcel">Exportar reporte a Excel</param>
        /// <param name="printAsynchronously">Impresión en modo asincrono</param>
        public ReportPr(int asyncProcessId, int printProcessId, int requestId, int billingGroup,
                      int policyId, int endorsementId, int quotationId, int reportType,
                      string codeBar, string username, int tempId, int prefixId,
                      int rangeMinValue, int rangeMaxValue, string processFromDate,
                      string processToDate, int indivualId, int branchId, bool withFormatCollect,
                      bool exportToExcel, bool printAsynchronously, bool currentFromFirst = false, bool endorsementText = false, bool tempAuthorization = false)
        {
            AsyncProcessId = asyncProcessId;
            PrintProcessId = printProcessId;
            RequestId = requestId;
            BillingGroup = billingGroup;
            PolicyId = policyId;
            EndorsementId = endorsementId;
            QuotationId = quotationId;
            ReportType = reportType;
            CodeBar = codeBar;
            Username = username;
            TempId = tempId;
            PrefixId = prefixId;
            RangeMinValue = rangeMinValue;
            RangeMaxValue = rangeMaxValue;
            ProcessFromDate = processFromDate;
            ProcessToDate = processToDate;
            IndivualId = indivualId;
            BranchId = branchId;
            WithFormatCollect = withFormatCollect;
            ExportToExcel = exportToExcel;
            PrintAsynchronously = printAsynchronously;
            CurrentFromFirst = currentFromFirst;
            EndorsementText = endorsementText;
            TempAuthorization = tempAuthorization;
        }
    }
}
