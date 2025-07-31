using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServices.Clases;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.Application.PrintingServices.Resources;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    abstract class Reportes : IReporte
    {
        #region Atributes

        private ArrayList _file;
        private string _pdfFilePath;
        private string _fileName;
        private Policy _policy;
        //TODO: Julio Guzmán, 25/01/2011, Almacena tipo de riesgo.
        private int _coveredRiskType;
        private string _fileNameHour;

        private int _actRiskNum;
        private int _limitPageLines;
        private int _pageLinesCount;
        private int _appendixPageCount;
        private bool _iscopy;
        private bool _hasTitle;
        private bool _isCollective;
        //TODO: Julio Guzmán, 20/01/2011, Almacena tipo de coaseguro (Para impresion del encabezado)
        private string _coinsured;

        //campos PrintHeader
        private string _prefixCode;
        private string _prefixDescription;
        private string _policyTypeDescrition;
        private string _policyNumber;
        private string _endorsementDescription;
        private string _endorsementNumber;
        private string _issueDateDay;
        private string _issueDateMonth;
        private string _issueDateYear;
        private string _exchangeRate;
        //TODO: Julio Guzmán, 20/01/2011, Se agregan variables para almacenar informacion de Coaseguro
        private string _annexNum;
        private string _mainNum;

        //campos PrintPolicyHolderData
        private string _holderName;
        private string _holderAddress;
        private string _holderPhone;
        private string _holderDocumentType;
        private string _holderDocumentNumber;
        private string _loadToPHolder;
        private string _salePointCd;

        //campos PrintIssuanceData
        private string _branchDescription;
        private string _currencyDescription;
        private string _branchCode;
        //TODO: Autor: John Ruiz; Fecha: 27/09/2010; Asunto: Se agrega el nombre de la ciudad de expedicion para llenar el campo EMITIDO EN; compañia: 2
        private string _branchCity;
        private string _appropiationPres;
        private string _dayFrom;
        private string _monthFrom;
        private string _yearFrom;
        private string _hourFrom;
        private string _dayTo;
        private string _monthTo;
        private string _yearTo;
        private string _hourTo;
        private string _days;
        private string _currentDate;
        private string _currencySymbol;
        private string _totalInsuredValue;
        private string _digitalSignature;
        //TODO: Autor: John Ruiz; Fecha: 17/09/2010; Asunto: Se agrega campo para almacenar la descripcion de la forma; Compañia:2
        private string _formDescription;

        //PrintCommissionsAndCoinsuranceData   
        private DataTable _intermediaries;
        private string _premium;
        private string _tax;
        private string _expenses;
        private string _overall;
        private string _currencyAjustment;
        private DataTable _coinsurance;
        private string _agreedPaymentMethod;
        private string _insuredTotalValue;
        //TODO: Autor: John Ruiz; Fecha 29/09/2010; Asunto: Se agrega para almacenar el texto de la poliza; Compañia 2;
        //TODO: Autor: John Ruiz; Fecha 21/10/2010; Asunto: Se agrega para almacenar el texto general y a nivel de riesgo de la poliza; Compañia 2;
        private string _generalText;
        private string _riskLeveText;
        private string _policyPurpose;
        private string _promissoryNoteNum;

        //PrintInsuredData
        private string _insuredName;
        private string _insuredAddress;
        private string _insuredDocumentType;
        private string _insuredDocumentNumber;
        private string _insuredPhone;

        //PrintRiskVehicleData        
        private string _beneficiaryDocumentNumber;
        private string _insuredNamePayment;
        private string _beneficiaryDocumentType;
        private string _beneficiaryName;

        private DataTable _coverages;
        private DataTable _accessories;
        //private DataTable _clauses;
        private DataTable _payment;
        private DataSet _paymentCheckDataSet;
        //TODO: Julio Guzmán, tabla para impresión de beneficiarios
        //private DataTable _beneficiaries;

        //TODO: Julio Guzmán, 24/03/2011, Se agrega para almcaenar si es cotizacion
        private bool _isQuotation;

        private string _endorsementDate;

        private decimal _totalValue;
        private string _uniqueIdentifier = null;

        private int _operationId = 0;

        public string UniqueIdentifier
        {
            get {
                return _uniqueIdentifier == null?_uniqueIdentifier=Guid.NewGuid().ToString():_uniqueIdentifier;
            }
        }

        public int OperationId
        {
            get { return _operationId; }
            set { _operationId = value; }
        }

        /// <summary>
        /// Contiene las lineas del archivo de texto del reporte
        /// </summary>
        public ArrayList File
        {
            get { return _file; }
            set { _file = value; }
        }

        /// <summary>
        /// Contiene el nombre del archivo sin extensión
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Póliza a imprimir
        /// </summary>
        public Policy PolicyData
        {
            get { return _policy; }
            set { _policy = value; }
        }

        /// <summary>
        /// Contiene la ruta completa del archivo
        /// </summary>
        public string PdfFilePath
        {
            get { return _pdfFilePath; }
            set { _pdfFilePath = value; }
        }

        /// <summary>
        /// Almacena el tipo de riesgo cubierto
        /// </summary>
        public int CoveredRiskType
        {
            get { return _coveredRiskType; }
            set { _coveredRiskType = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        protected int ActRiskNum
        {
            get { return _actRiskNum; }
            set { _actRiskNum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected int LimitPageLines
        {
            //TODO: Autor: John Ruiz; Asunto: Se devuelve el valor que tenga _limitPageLines y no 25 ya que para los anexos esta logitud cambia
            //Compañia: 2
            get { return this._limitPageLines; }
            set { _limitPageLines = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageLinesCount
        {
            get { return _pageLinesCount; }
            set { _pageLinesCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected int AppendixPageCount
        {
            get { return _appendixPageCount; }
            set { _appendixPageCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool Iscopy
        {
            get { return _iscopy; }
            set { _iscopy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool HasTitle
        {
            get { return _hasTitle; }
            set { _hasTitle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool IsCollective
        {
            get { return _isCollective; }
            set { _isCollective = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string PrefixCode
        {
            get { return _prefixCode; }
            set { _prefixCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string PrefixDescription
        {
            get { return _prefixDescription; }
            set { _prefixDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string PolicyTypeDescrition
        {
            get { return _policyTypeDescrition; }
            set { _policyTypeDescrition = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string PolicyNumber
        {
            get { return _policyNumber; }
            set { _policyNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string EndorsementDescription
        {
            get { return _endorsementDescription; }
            set { _endorsementDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string EndorsementNumber
        {
            get { return _endorsementNumber; }
            set { _endorsementNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string IssueDateDay
        {
            get { return _issueDateDay; }
            set { _issueDateDay = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string IssueDateMonth
        {
            get { return _issueDateMonth; }
            set { _issueDateMonth = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string IssueDateYear
        {
            get { return _issueDateYear; }
            set { _issueDateYear = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string ExchangeRate
        {
            get { return _exchangeRate; }
            set { _exchangeRate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HolderName
        {
            get { return _holderName; }
            set { _holderName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HolderAddress
        {
            get { return _holderAddress; }
            set { _holderAddress = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HolderPhone
        {
            get { return _holderPhone; }
            set { _holderPhone = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HolderDocumentType
        {
            get { return _holderDocumentType; }
            set { _holderDocumentType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HolderDocumentNumber
        {
            get { return _holderDocumentNumber; }
            set { _holderDocumentNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string LoadToPHolder
        {
            get { return _loadToPHolder; }
            set { _loadToPHolder = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string SalePointCd
        {
            get { return _salePointCd; }
            set { _salePointCd = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string BranchDescription
        {
            get { return _branchDescription; }
            set { _branchDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string CurrencyDescription
        {
            get { return _currencyDescription; }
            set { _currencyDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string BranchCode
        {
            get { return _branchCode; }
            set { _branchCode = value; }
        }

        //TODO: Autor: John Ruiz; Fecha: 27/09/2010; Asunto: Se agrega el nombre de la ciudad de expedicion para llenar el campo EMITIDO EN; compañia: 2
        /// <summary>
        /// 
        /// </summary>
        protected string BranchCity
        {
            get { return _branchCity; }
            set { _branchCity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string AppropiationPres
        {
            get { return _appropiationPres; }
            set { _appropiationPres = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string DayFrom
        {
            get { return _dayFrom; }
            set { _dayFrom = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string MonthFrom
        {
            get { return _monthFrom; }
            set { _monthFrom = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string YearFrom
        {
            get { return _yearFrom; }
            set { _yearFrom = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HourFrom
        {
            get { return _hourFrom; }
            set { _hourFrom = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string DayTo
        {
            get { return _dayTo; }
            set { _dayTo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string MonthTo
        {
            get { return _monthTo; }
            set { _monthTo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string YearTo
        {
            get { return _yearTo; }
            set { _yearTo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string HourTo
        {
            get { return _hourTo; }
            set { _hourTo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Days
        {
            get { return _days; }
            set { _days = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string CurrentDate
        {
            get { return _currentDate; }
            set { _currentDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        /// <summary>
        /// Valor total asegurado de la póliza colectiva
        /// </summary>
        protected string TotalInsuredValue
        {
            get { return _totalInsuredValue; }
            set { _totalInsuredValue = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DigitalSignature
        {
            get { return _digitalSignature; }
            set { _digitalSignature = value; }
        }

        //TODO: Autor: John Ruiz; Fecha: 17/09/2010; Asunto: Se agrega campo para almacenar la descripcion de la forma; Compañia:2
        public string FormDescription
        {
            get { return _formDescription; }
            set { _formDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataTable Intermediaries
        {
            get { return _intermediaries; }
            set { _intermediaries = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Premium
        {
            get { return _premium; }
            set { _premium = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Tax
        {
            get { return _tax; }
            set { _tax = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Expenses
        {
            get { return _expenses; }
            set { _expenses = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Overall
        {
            get { return _overall; }
            set { _overall = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string CurrencyAjustment
        {
            get { return _currencyAjustment; }
            set { _currencyAjustment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataTable Coinsurance
        {
            get { return _coinsurance; }
            set { _coinsurance = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string AgreedPaymentMethod
        {
            get { return _agreedPaymentMethod; }
            set { _agreedPaymentMethod = value; }
        }

        /// <summary>
        /// Valor total asegurado de la póliza individual
        /// </summary>
        protected string InsuredTotalValue
        {
            get { return _insuredTotalValue; }
            set { _insuredTotalValue = value; }
        }

        //TODO: Autor: John Ruiz; Fecha 29/09/2010; Asunto: Se encapsula el campo; Compañia 2;
        /// <summary>
        /// Texto de la Póliza
        /// </summary>
        public string PolicyPurpose
        {
            get { return _policyPurpose; }
            set { _policyPurpose = value; }
        }

        //TODO: Autor: John Ruiz; Fecha 29/09/2010; Asunto: Se encapsula el campo; Compañia 2;
        /// <summary>
        /// Texto de la Póliza
        /// </summary>
        public string GeneralText
        {
            get { return _generalText; }
            set { _generalText = value; }
        }

        //TODO: Autor: John Ruiz; Fecha 13/10/2010; Asunto: Se encapsula el campo; Compañia 2;
        /// <summary>
        /// Texto a nivel de riesgo
        /// </summary>
        public string RiskLeveText
        {
            get { return _riskLeveText; }
            set { _riskLeveText = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string InsuredName
        {
            get { return _insuredName; }
            set { _insuredName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string InsuredAddress
        {
            get { return _insuredAddress; }
            set { _insuredAddress = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string InsuredDocumentType
        {
            get { return _insuredDocumentType; }
            set { _insuredDocumentType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string InsuredDocumentNumber
        {
            get { return _insuredDocumentNumber; }
            set { _insuredDocumentNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string InsuredPhone
        {
            get { return _insuredPhone; }
            set { _insuredPhone = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string BeneficiaryDocumentNumber
        {
            get { return _beneficiaryDocumentNumber; }
            set { _beneficiaryDocumentNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string BeneficiaryDocumentType
        {
            get { return _beneficiaryDocumentType; }
            set { _beneficiaryDocumentType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string BeneficiaryName
        {
            get { return _beneficiaryName; }
            set { _beneficiaryName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string InsuredNamePayment
        {
            get { return _insuredNamePayment; }
            set { _insuredNamePayment = value; }
        }


        //<< TODO: Julio Guzmán, 20/01/2011, Se agregan propiedades para acceder al Numero de certificado\
        //                                   y a la poliza lider
        /// <summary>
        /// 
        /// </summary>
        public string AnnexNum
        {
            get { return _annexNum; }
            set { _annexNum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MainNum
        {
            get { return _mainNum; }
            set { _mainNum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Coinsured
        {
            get { return _coinsured; }
            set { _coinsured = value; }
        }
        //  TODO: Julio Guzmán, 20/01/2011 >>

        /// <summary>
        /// 
        /// </summary>
        private DataTable Coverages
        {
            get { return _coverages; }
            set { _coverages = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataTable Accessories
        {
            get { return _accessories; }
            set { _accessories = value; }
        }

        /*protected DataTable Beneficiaries
        {
            get { return _beneficiaries; }
            set { _beneficiaries = value; }
        }*/


        /// <summary>
        /// 
        /// </summary>
        /*protected DataTable Clauses
        {
            get { return _clauses; }
            set { _clauses = value; }
        }*/

        /// <summary>
        /// 
        /// </summary>
        protected DataTable Payment
        {
            get { return _payment; }
            set { _payment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected DataSet PaymentCheckDataSet
        {
            get { return _paymentCheckDataSet; }
            set { _paymentCheckDataSet = value; }
        }

        protected string EndorsementDate
        {
            get { return _endorsementDate; }
            set { _endorsementDate = value; }
        }

        protected Decimal TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }

        protected DataSet dsPoliza;

        #endregion

        #region dat Labels

        /// <summary>
        /// Etiqueta '^Field' para construir archivo .dat
        /// </summary>
        protected string Field
        {
            get { return RptFields.LBL_FIELD_REPORT; }
            set { }
        }

        /// <summary>
        /// Etiqueta '^job' para construir archivo .dat
        /// </summary>
        protected string Job
        {
            get { return RptFields.LBL_JOB_REPORT; }
            set { }
        }

        /// <summary>
        /// Etiqueta '^reformat no' para construir archivo .dat
        /// </summary>
        protected string ReformatNo
        {
            get { return RptFields.LBL_REFORMAT_NO_REPORT; }
            set { }
        }

        /// <summary>
        /// Etiqueta '^form' para construir archivo .dat
        /// </summary>
        protected string Form
        {
            get { return RptFields.LBL_FORM_REPORT; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string PrefixName
        {
            get { return RptFields.LBL_PREFIX_NAME_REPORT; }
            set { }
        }

        /// <summary>
        /// Etiqueta ' -ass5 -aduon -ati1' para construir archivo .dat
        /// </summary>
        protected string Ass5AduonAti1
        {
            get { return RptFields.LBL_ASS5_ADUON_ATI1_REPORT; }
            set { }
        }

        /// <summary>
        /// Etiqueta ' -aip1 -aebon' para construir archivo .dat
        /// </summary>
        protected string A1p1Aebon
        {
            get { return RptFields.LBL_AIP1_AEBON_REPORT; }
            set { }
        }

        /// <summary>
        /// Nombre de la impresora que apartir del archivo .dat genera la impresión
        /// </summary>
        protected string PrinterName
        {
            get { return ConfigurationSettings.AppSettings["JetFormPrinterName"]; }
            set { }
        }

        /// <summary>
        /// Ruta de las firmas digitalizadas
        /// </summary>
        protected string DigitalizedSignature
        {
            get { return ConfigurationSettings.AppSettings["DigitalSignature"]; }
            set { }
        }

        /// <summary>
        /// Simbolo pesos
        /// </summary>
        private string Pesos
        {
            get { return RptFields.LBL_COP_SYMBOL; }
            set { }
        }

        /// <summary>
        /// Simbolo dolares
        /// </summary>
        private string Dolares
        {
            get { return RptFields.LBL_USD_SYMBOL; }
            set { }
        }

        /// <summary>
        /// TODO: Julio Guzmán, 24/03/2011, Propiedad para acceder a variable _isQuotation
        /// </summary>
        protected bool isQuotation
        {
            get { return _isQuotation; }
            set { _isQuotation = value; }
        }

        protected string PromissoryNoteNum
        {
            get { return _promissoryNoteNum; }
            set { _promissoryNoteNum = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Imprime el texto correspondiente al risk
        /// </summary>
        void printRisk() { throw new NotImplementedException(); }

        /// <summary>
        /// Imprime la coberturas de la póliza
        /// </summary>
        void printCoverages() { throw new NotImplementedException(); }

        /// <summary>
        /// Ejecuta los sp correspondientes al tipo de reporte para el llenado de tablas con los datos del reporte.
        /// Obtiene los datos del reporte (dataset) para ser colocados en el archivo '.dat'
        /// </summary>
        /// <param name="policy">Datos de la póliza a imprimir</param>
        /// <param name="paramSp">Parámetros del procedimiento almacenado correspondiente</param>
        /// <param name="coveredRiskType">Tipo de riesgos de la póliza que se va a imprimir</param>
        public void create(Policy policy, NameValue[] paramSetSp, int coveredRiskType, int operationId, bool isCollective)
        {
            _fileNameHour = String.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
            NameValue[] paramGetSp;
            this.PolicyData = policy;
            this.LimitPageLines = 29;
            this.CoveredRiskType = coveredRiskType;
            this.OperationId = operationId;
            // << TODO: Julio Guzmán,
            //          24/03/2011,
            //          Se valida si es cotizacion
            if (PolicyData.QuotationId != null && PolicyData.QuotationId > 0)
            {
                this.isQuotation = true;
            }
            //    TODO: Julio Guzmán,
            //          24/03/2011 >>

            switch (CoveredRiskType)
            {
                case (int)RiskType.AUTO:
                    paramGetSp = new NameValue[3];
                    paramGetSp[0] = paramSetSp[0];
                    paramGetSp[1] = paramSetSp[1];
                    paramGetSp[2] = paramSetSp[2];

                    if (!isCollective)
                        dsPoliza = this.getReportData(RptFields.SPR_SET_REPORT, paramSetSp, RptFields.SPR_GET_POLICY_REPORT, paramGetSp);
                    else
                        dsPoliza = this.getReportData("REPORT.MSV_CO_SINGLE_POLICY_REPORT", paramSetSp, RptFields.SPR_GET_POLICY_REPORT, paramGetSp);

                    break;
                case (int)RiskType.FIANZA:
                    paramGetSp = new NameValue[4];
                    paramGetSp[0] = paramSetSp[0];
                    paramGetSp[1] = paramSetSp[1];
                    paramGetSp[2] = paramSetSp[2];
                    paramGetSp[3] = paramSetSp[3];

                    dsPoliza = this.getReportData(RptFields.SPR_SET_REPORT, paramSetSp, RptFields.SPR_GET_POLICY_REPORT, paramGetSp);
                    break;
                case (int)RiskType.UBICACION:
                    paramGetSp = new NameValue[3];
                    paramGetSp[0] = paramSetSp[0];
                    paramGetSp[1] = paramSetSp[1];
                    paramGetSp[2] = paramSetSp[2];

                    dsPoliza = this.getReportData(RptFields.SPR_SET_REPORT, paramSetSp, RptFields.SPR_GET_POLICY_REPORT, paramGetSp);
                    break;
                case (int)RiskType.TRANSPORTE:
                    paramGetSp = new NameValue[3];
                    paramGetSp[0] = paramSetSp[0];
                    paramGetSp[1] = paramSetSp[1];
                    paramGetSp[2] = paramSetSp[2];

                    dsPoliza = this.getReportData(RptFields.SPR_SET_REPORT, paramSetSp, RptFields.SPR_GET_POLICY_REPORT, paramGetSp);
                    break;
                default:
                    break;
            }
            this.writeData();
        }

        /// <summary>
        /// Escribe las lineas de texto del archivo (dat) del reporte
        /// </summary>
        /// <param name="policy">Tablas con la información de la póliza</param>
        protected virtual void writeData()
        {
            DataSet dsRiesgos = this.getActualRisk(dsPoliza);

            if (dsPoliza.Tables["Table7"].Rows.Count > 1)
            {
                this.IsCollective = true;
            }

            this.iniRiskValues();
        }

        /// <summary>
        /// Inicializa los valores para imprimir el siguiente riesgo
        /// </summary>
        protected virtual void iniRiskValues()
        {
            switch (this.CoveredRiskType)
            {
                case (int)RiskType.AUTO:
                    this.LimitPageLines = 28;
                    break;
                case (int)RiskType.FIANZA:
                    this.LimitPageLines = 28;
                    break;
                case (int)RiskType.UBICACION:
                    this.LimitPageLines = 28;
                    break;
                case (int)RiskType.TRANSPORTE:
                    this.LimitPageLines = 28;
                    break;
            }

            //int
            this.PageLinesCount = 0;
            this.PageLinesCount = 0;
            this.AppendixPageCount = 0;

            //bool
            this.Iscopy = false;
        }

        /// <summary>
        /// Crea una instancia del archivo del reporte (dat)
        /// </summary>
        protected void createFile()
        {
            this.File = new ArrayList();
        }

        /// <summary>
        /// Obtiene los datos del risk que se va imprimir
        /// </summary>
        /// <param name="policy">Póliza a imprimir</param>
        /// <returns>Riesgo a imprimir</returns>
        protected DataSet getActualRisk(DataSet policy)
        {
            DataSet dataset = new DataSet();

            foreach (DataTable tbl in policy.Tables)
            {
                DataTable table = tbl.Clone();

                foreach (DataRow row in tbl.Rows)
                {
                    if (row["RISK_NUM"].ToString().Equals(this.ActRiskNum.ToString()))
                        table.ImportRow(row);
                }

                dataset.Tables.Add(table);
            }
            return dataset;
        }

        /// <summary>
        /// Adiciona a la lineas del archivo los datos del risk a imprimir.
        /// </summary>
        /// <param name="risk">Riesgo a imprimir</param>
        protected void setRiskData(DataSet risk)
        {
            this.BranchCode = risk.Tables["Table7"].Rows[0]["BRANCH_CD"].ToString();
            this.PrefixCode = risk.Tables["Table7"].Rows[0]["PREFIX_CD"].ToString();
            this.PolicyNumber = risk.Tables["Table7"].Rows[0]["POLICY_NUMBER"].ToString();
            this.EndorsementNumber = risk.Tables["Table7"].Rows[0]["DOCUMENT_NUM"].ToString();
            //TODO: Autor: John Ruiz; Asunto: Establezco el limete de lineas para la primera pagina de la impresion
            this.LimitPageLines = 29;

            switch (this.PolicyData.ReportType)
            {
                case (int)ReportType.COMPLETE_POLICY:
                    this.setPolicyData(risk);
                    this.setConventionData(risk);
                    break;
                case (int)ReportType.ONLY_POLICY:
                    this.setPolicyData(risk);
                    break;
                case (int)ReportType.PAYMENT_CONVENTION:
                    this.setConventionData(risk);
                    break;
                case (int)ReportType.TEMPORARY:
                    this.setPolicyData(risk);
                    this.setConventionData(risk);
                    break;
                case (int)ReportType.QUOTATION:
                    this.setPolicyData(risk);
                    this.setConventionData(risk);
                    break;
            }

            //Report file
            this.FileName = this.getFileName();
            this.PdfFilePath = this.setPdfFilePath();
        }

        /// <summary>
        /// Adiciona a las lineas del archivo los datos de la póliza a imprimir.
        /// </summary>
        /// <param name="risk">Riesgo a imprimir</param>
        protected virtual void setPolicyData(DataSet risk)
        {
            //Tablas
            this.initializeIntermediariesDataTable();
            this.initializeCoinsuranceDataTable();
            //this.initializeCoveragesDataTable();
            //this.initializeAccessoriesDataTable();
            //this.initializeClausesDataTable();
            //this.initializeBeneficiariesDataTable();

            this.BranchCity = risk.Tables["Table"].Rows[0]["BRANCH_CITY"].ToString();
            //Cabecera         
            this.PrefixCode = risk.Tables["Table7"].Rows[0]["PREFIX_CD"].ToString();
            this.PrefixDescription = risk.Tables["Table"].Rows[0]["PREFIX"].ToString();
            this.PolicyTypeDescrition = risk.Tables["Table"].Rows[0]["POLICY_TYPE"].ToString();
            this.PolicyNumber = risk.Tables["Table7"].Rows[0]["POLICY_NUMBER"].ToString();
            this.EndorsementDescription = risk.Tables["Table"].Rows[0]["ENDORSEMENT_TYPE_DESC"].ToString();
            this.EndorsementNumber = risk.Tables["Table7"].Rows[0]["DOCUMENT_NUM"].ToString();
            this.IssueDateDay = risk.Tables["Table"].Rows[0]["ISSUE_DATE_DAY"].ToString();
            this.IssueDateMonth = risk.Tables["Table"].Rows[0]["ISSUE_DATE_MONTH"].ToString();
            this.IssueDateYear = risk.Tables["Table"].Rows[0]["ISSUE_DATE_YEAR"].ToString();
            this.ExchangeRate = ReportServiceHelper.formatMoney(risk.Tables["Table"].Rows[0]["EXCHANGE_RATE"].ToString(), CultureInfo.CurrentCulture);

            //Beneficiary
            this.BeneficiaryName = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["BENEFICIARY_NAME"].ToString());
            this.BeneficiaryDocumentType = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["BENEFICIARY_DOC_TYPE"].ToString());

            if (BeneficiaryDocumentType.Equals(RptFields.LBL_NIT))
                this.BeneficiaryDocumentNumber = ReportServiceHelper.formatNIT(risk.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString());
            else
                this.BeneficiaryDocumentNumber = risk.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString();

            //Tomador            
            this.HolderName = String.Format(RptFields.FLD_NAME_FORMAT,
                                            risk.Tables["Table"].Rows[0]["POLICY_HOLDER_CD"].ToString(),
                                            risk.Tables["Table"].Rows[0]["POLICY_HOLDER_NAME"].ToString());
            //TODO: Autor: John Ruiz; Fecha: 21/10/2010; Asunto:Se corta la cadena para que no se sobreponga sobre el nro de doc; compañia: 2
            this.HolderName = this.HolderName.Length > 78 ? this.HolderName.Remove(77) : this.HolderName;
            this.HolderAddress = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["POLICY_HOLDER_ADD"].ToString().ToUpper());
            this.HolderPhone = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["POLICY_HOLDER_PHONE"].ToString());
            this.HolderDocumentType = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["POLICY_HOLDER_DOC_TYPE"].ToString());

            if (HolderDocumentType.Equals(RptFields.LBL_NIT))
            {
                //TODO: Autor: John Ruiz; Fecha 24/09/2010; Asunto: Se valida cuando viene " X" para prospecto sin nro de doc para que no lance excepcion; compañia: 2
                this.HolderDocumentNumber = risk.Tables["Table"].Rows[0]["POLICY_HOLDER_DOC"].ToString() == " X" ? "" : ReportServiceHelper.formatNIT(risk.Tables["Table"].Rows[0]["POLICY_HOLDER_DOC"].ToString());
            }
            else
                this.HolderDocumentNumber = risk.Tables["Table"].Rows[0]["POLICY_HOLDER_DOC"].ToString();

            this.LoadToPHolder = risk.Tables["Table"].Rows[0]["CHECK_PAYABLE_TO"].ToString();
            //TODO: Autor: John Ruiz; Fecha: 21/10/2010; Asunto:Se corta la cadena para que no se sobreponga sobre la forma de pago; compañia: 2
            this.LoadToPHolder = LoadToPHolder.Length > 57 ? LoadToPHolder.Remove(57) : LoadToPHolder;
            //TODO: Julio Guzmán, 04/03/2011, Se añade validacion para codigo de sucursal de temporales
            if (!risk.Tables["Table"].Rows[0]["SALE_POINT_CD"].ToString().Equals(string.Empty))
            {
                this.SalePointCd = risk.Tables["Table"].Rows[0]["SALE_POINT_CD"].ToString();
            }
            else
            {
                this.SalePointCd = risk.Tables["Table"].Rows[0]["SALE_POINT_CD_TMP"].ToString();
            }

            this.CurrencySymbol = risk.Tables["Table"].Rows[0]["CURRENCY_SYMBOL"].ToString();
            this.InsuredTotalValue = ReportServiceHelper.formatMoney(risk.Tables["Table"].Rows[0]["INSURED_TOTAL_VALUE"].ToString(),CultureInfo.CurrentCulture);

            //Issuance Data
            this.BranchDescription = risk.Tables["Table"].Rows[0]["BRANCH"].ToString();
            this.CurrencyDescription = risk.Tables["Table"].Rows[0]["CURRENCY"].ToString();
            this.BranchCode = risk.Tables["Table7"].Rows[0]["BRANCH_CD"].ToString();
            //TODO: Autor: John Ruiz; Fecha: 27/09/2010; Asunto: Se agrega el nombre de la ciudad de expedicion para llenar el campo EMITIDO EN; compañia: 2
            this.BranchCity = risk.Tables["Table"].Rows[0]["BRANCH_CITY"].ToString();
            this.AppropiationPres = "NO";//A.P.
            this.DayFrom = risk.Tables["Table"].Rows[0]["CURRENT_FROM_DAY"].ToString();
            this.MonthFrom = risk.Tables["Table"].Rows[0]["CURRENT_FROM_MONTH"].ToString();
            this.YearFrom = risk.Tables["Table"].Rows[0]["CURRENT_FROM_YEAR"].ToString();
            this.HourFrom = risk.Tables["Table"].Rows[0]["CURRENT_FROM_HOUR"].ToString();
            this.DayTo = risk.Tables["Table"].Rows[0]["CURRENT_TO_DAY"].ToString();
            this.MonthTo = risk.Tables["Table"].Rows[0]["CURRENT_TO_MONTH"].ToString();
            this.YearTo = risk.Tables["Table"].Rows[0]["CURRENT_TO_YEAR"].ToString();
            this.HourTo = risk.Tables["Table"].Rows[0]["CURRENT_TO_HOUR"].ToString();
            this.Days = risk.Tables["Table"].Rows[0]["DAY_COUNT"].ToString();
            this.CurrentDate = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);
            this.DigitalSignature = risk.Tables["Table"].Rows[0]["DIGITAL_SIGNATURE"].ToString();
            //TODO: Autor: John Ruiz; Fecha: 17/09/2010; Asunto: Se agrega campo para almacenar la descripcion de la forma; Compañia:2
            this.FormDescription = risk.Tables["Table"].Rows[0]["FORM_DESC"].ToString();

            this.setPremiumAmount(this.dsPoliza);

            //TODO: Autor: John Ruiz; Fecha: 21/10/2010; Asunto: Se multiplica por la taza de cambio para obtener el iva en pesos; Compañia:2
            decimal AuxTax = Convert.ToDecimal(risk.Tables["Table"].Rows[0]["EXCHANGE_RATE"].ToString()) * Convert.ToDecimal(risk.Tables["table"].Rows[0]["TAX"].ToString());
            this.Tax = ReportServiceHelper.formatMoney(AuxTax.ToString(), CultureInfo.CurrentCulture);

            this.Expenses = risk.Tables["table"].Rows[0]["EXPENSES"].ToString() == string.Empty ?
                            ReportServiceHelper.formatMoney("0", CultureInfo.CurrentCulture) :
                            ReportServiceHelper.formatMoney(risk.Tables["table"].Rows[0]["EXPENSES"].ToString(), CultureInfo.CurrentCulture);
            
            this.AgreedPaymentMethod = risk.Tables["table"].Rows[0]["AGREED_PAYMENT_METHOD"].ToString();

            this.setIntermediariesData(risk);

            //Coinsured
            foreach (DataRow row in risk.Tables["Table3"].Rows)
            {
                if (!row["PART_CIA_PCT"].ToString().Equals(string.Empty))
                {
                    DataRow newRow = Coinsurance.NewRow();
                    newRow["Code"] = row["INSURANCE_COMPANY_ID"];
                    newRow["Company"] = row["INSURANCE_COMPANY_DESC"];
                    //TODO: Julio Guzmán, 31/03/2011, Se cambia la conversion por la cultura actual CultureInfo.CurrentCulture
                    decimal commissionPercentage = Convert.ToDecimal(row["PART_CIA_PCT"].ToString(), CultureInfo.CurrentCulture);
                    newRow["Percentage"] = ReportServiceHelper.formatPercentage((commissionPercentage).ToString());
                    newRow["Premium"] = ReportServiceHelper.formatMoney(((commissionPercentage / 100) * Convert.ToDecimal(Premium)).ToString(), CultureInfo.CurrentCulture);//Prima
                    if (row.Table.Columns.Count > 6)
                    {
                        this.Coinsured = "(COASEGURO ACEPTADO)";
                        newRow["Annex"] = row["ANNEX_NUM_MAIN"];
                        this.AnnexNum = newRow["Annex"].ToString();
                        newRow["Main"] = row["POLICY_NUM_MAIN"];
                        this.MainNum = newRow["Main"].ToString();
                    }
                    this.Coinsurance.Rows.Add(newRow);
                }
            }

            this.setInsuredData(risk);

            //Asegurado      
            this.InsuredNamePayment = risk.Tables["Table"].Rows[0]["INSURED_NAME"].ToString();

            this.InsuredName = String.Format(RptFields.FLD_NAME_FORMAT,
                                             risk.Tables["Table"].Rows[0]["INSURED_CD"].ToString(),
                                             risk.Tables["Table"].Rows[0]["INSURED_NAME"].ToString());
            //TODO: Autor: John Ruiz; Fecha: 21/10/2010; Asunto:Se corta la cadena para que no se sobreponga sobre el nro de doc; compañia: 2
            this.InsuredName = this.InsuredName.Length > 78 ? this.InsuredName.Remove(78) : this.InsuredName;

            //TODO: Autor: John Ruiz; Fecha 24/09/2010; Asunto: Se valida cuando viene vacio para que no lance excepcion; compañia: 2
            this.InsuredAddress = risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString() == "" ? ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString()) : ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString().ToUpper());
            this.InsuredDocumentType = risk.Tables["Table"].Rows[0]["INSURED_DOC_TYPE"].ToString();

            if (this.InsuredDocumentType.Equals(RptFields.LBL_NIT))
                //TODO: Autor: John Ruiz; Fecha 24/09/2010; Asunto: Se valida cuando viene " X" para prospecto sin nro de doc para que no lance excepcion; compañia: 2
                this.InsuredDocumentNumber = risk.Tables["Table"].Rows[0]["INSURED_DOC"].ToString() == " X" ? "" : ReportServiceHelper.formatNIT(risk.Tables["Table"].Rows[0]["INSURED_DOC"].ToString());
            else
                this.InsuredDocumentNumber = risk.Tables["Table"].Rows[0]["INSURED_DOC"].ToString();

            this.InsuredPhone = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["INSURED_PHONE"].ToString());

            string text = string.Empty;
            string textRisk = string.Empty;
            if (Convert.ToInt32(PrefixCode) == (int)Sistran.Company.Application.PrintingServices.Enums.PrefixCode.SURETY)
            {
                text = ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["PURPOSE"].ToString()) + '\n';
            }
            text += ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["CONDITION_TEXT"].ToString());

            this.GeneralText = text.Replace((char)96, (char)32);
            this.RiskLeveText = (Convert.ToInt32(this.PrefixCode) != 2) ? risk.Tables["Table"].Rows[0]["RISK_CONDITION_TEXT"].ToString() : string.Empty;
            this.RiskLeveText = ReportServiceHelper.unicode_iso8859(RiskLeveText.Replace((char)96, (char)32));
            text = string.Empty;

            //Convenio de pago
            if (this.IsCollective)
            {
                if (this.ActRiskNum == 0)
                    this.setPaymentCheck(risk);
            }
            else
                this.setPaymentCheck(risk);


            this.EndorsementDate = ((this.DayFrom.Length == 1) ? "0" + this.DayFrom : this.DayFrom) + @"/" +
                         ((this.MonthFrom.Length == 1) ? "0" + this.MonthFrom : this.MonthFrom) + @"/" +
                           this.YearFrom;

            //Archivo de reporte
            this.PromissoryNoteNum = risk.Tables["Table"].Rows[0]["PROMISSORY_NOTE_NUM_CD"].ToString();

            this.FileName = this.getFileName();
            this.PdfFilePath = this.setPdfFilePath();
        }

        protected virtual void setIntermediariesData(DataSet risk)
        {
            //Intermediaries            
            foreach (DataRow row in risk.Tables["Table4"].Rows)
            {
                DataRow newRow = this.Intermediaries.NewRow();
                newRow["Id"] = row["AGENTE_CODIGO"];
                newRow["Type"] = row["AGENTE_CODIGO_TIPO"];
                newRow["Name"] = row["AGENTE_NOMBRE"];
                //TODO: Autor: John Ruiz; Fecha: 04/10/2010; Asunto: Se corrige la comision de los intermediarios; Compañia: 2
                newRow["Percentage"] = ReportServiceHelper.formatPercentage(row["AGENTE_COMISION"].ToString());
                decimal AgentParticipation = Convert.ToDecimal(row["AGENTE_PARTICIPACION"].ToString()) / 100;
                decimal commissionPercentage = Convert.ToDecimal(row["AGENTE_COMISION"].ToString()) / 100;
                newRow["Commission"] = ReportServiceHelper.formatMoney(row["PRIMA"].ToString(), CultureInfo.CurrentCulture);
                this.Intermediaries.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// sets the premium amount for collective policies from location line business
        /// </summary>
        /// <param name="policy">Policty to prints</param>
        protected virtual void setPremiumAmount(DataSet policy)
        {
            DataSet risk = this.getActualRisk(policy);
            this.Premium = ReportServiceHelper.formatMoney(risk.Tables["table"].Rows[0]["PREMIUM_AMT"].ToString(), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Set data to insured object
        /// </summary>
        /// <param name="risk">Risk to be printed</param>
        protected virtual void setInsuredData(DataSet risk)
        {
            //Asegurado      
            this.InsuredNamePayment = risk.Tables["Table"].Rows[0]["INSURED_NAME"].ToString();

            this.InsuredName = String.Format(RptFields.FLD_NAME_FORMAT,
                                             risk.Tables["Table"].Rows[0]["INSURED_CD"].ToString(),
                                             risk.Tables["Table"].Rows[0]["INSURED_NAME"].ToString());
            //TODO: Autor: John Ruiz; Fecha 24/09/2010; Asunto: Se valida cuando viene vacio para que no lance excepcion; compañia: 2
            this.InsuredAddress = risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString() == "" ? ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString()) : ReportServiceHelper.unicode_iso8859(risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString().ToUpper());
            //            this.InsuredAddress = risk.Tables["Table"].Rows[0]["INSURED_ADD"].ToString().ToUpper();
            this.InsuredDocumentType = risk.Tables["Table"].Rows[0]["INSURED_DOC_TYPE"].ToString();

            if (this.InsuredDocumentType.Equals(RptFields.LBL_NIT))
                //TODO: Autor: John Ruiz; Fecha 24/09/2010; Asunto: Se valida cuando viene " X" para prospecto sin nro de doc para que no lance excepcion; compañia: 2
                this.InsuredDocumentNumber = risk.Tables["Table"].Rows[0]["INSURED_DOC"].ToString() == " X" ? "" : ReportServiceHelper.formatNIT(risk.Tables["Table"].Rows[0]["INSURED_DOC"].ToString());
            else
                this.InsuredDocumentNumber = risk.Tables["Table"].Rows[0]["INSURED_DOC"].ToString();

            this.InsuredPhone = risk.Tables["Table"].Rows[0]["INSURED_PHONE"].ToString();
        }

        /// <summary>
        /// Adiciona a las lineas del archivo los datos del convenio de pago de la póliza a imprimir.
        /// </summary>
        /// <param name="risk">Riesgo a imprimir</param>
        protected void setConventionData(DataSet risk)
        {
            if (this.IsCollective)
            {
                if (this.ActRiskNum == 0)
                    this.setPaymentCheck(risk);
            }
            else
                this.setPaymentCheck(risk);
        }

        /// <summary>
        /// Forma la ruta del archivo pdf generado
        /// </summary>
        /// <returns>Ruta del archivo pdf generado</returns>
        protected virtual string setPdfFilePath()
        {
            string ruta = ConfigurationSettings.AppSettings["UserRemotePath"] +
                          this.BranchCode + @"\" +
                          this.PrefixCode + @"\" +
                          this.FileName +
                          ".pdf";

            ReportServiceHelper.verifyDirPath(Path.GetDirectoryName(ruta));

            return ruta;
        }

        /// <summary>
        /// Forma el nombre del archivo pdf generado
        /// </summary>
        /// <returns>Nombre del archivo pdf generado</returns>
        protected virtual string getFileName()
        {
            return this.BranchCode + this.PrefixCode + this.PolicyNumber + this.EndorsementNumber + _fileNameHour;
            
        }

        /// <summary>
        /// Imprime numero de pagare
        /// </summary>
        protected virtual void PrintPromissoryNoteNum()
        {
            if (!string.IsNullOrEmpty(this.PromissoryNoteNum))
            {
                this.validatePageLimit(2);
                this.File.Add(new DatRecord("\n" + string.Format(RptFields.LBL_PROMISSORY_NOTE, this.PromissoryNoteNum), null));
            }
        }

        /// <summary>
        /// Inicializa tabla de intermediarios de la póliza
        /// </summary>
        protected void initializeIntermediariesDataTable()
        {
            this.Intermediaries = new DataTable();
            this.Intermediaries.Columns.Add("Id", typeof(String));
            this.Intermediaries.Columns.Add("Type", typeof(String));
            this.Intermediaries.Columns.Add("Name", typeof(String));
            this.Intermediaries.Columns.Add("Percentage", typeof(String));
            this.Intermediaries.Columns.Add("Commission", typeof(String));
            this.Intermediaries.Columns.Add("Premium", typeof(String));
        }

        /// <summary>
        /// Inicializa tabla de coaseguro de la póliza
        /// </summary>
        protected void initializeCoinsuranceDataTable()
        {
            this.Coinsurance = new DataTable();
            this.Coinsurance.Columns.Add("Code", typeof(String));
            this.Coinsurance.Columns.Add("Company", typeof(String));
            this.Coinsurance.Columns.Add("Percentage", typeof(String));
            this.Coinsurance.Columns.Add("Premium", typeof(String));
            //TODO: Julio Guzmán, 20/01/2011, Se agregan columnas para N° Certificado y Poliza Lider
            this.Coinsurance.Columns.Add("Annex", typeof(string));
            this.Coinsurance.Columns.Add("Main", typeof(string));
        }

        /// <summary>
        /// Imprime el convenio de pago de la póliza
        /// </summary>
        protected void printReportPaymentCheck()
        {
            PaymentCheck payment = (this.IsCollective) ? new PaymentCheck(this.PaymentCheckDataSet, this.PolicyNumber, this.TotalInsuredValue) :
                                                        new PaymentCheck(this.PaymentCheckDataSet, this.PolicyNumber, this.InsuredTotalValue);

            foreach (DatRecord item in payment.FileLines)
                this.File.Add(item);
        }

        /// <summary>
        /// Imprime el anexo de la póliza
        /// </summary>
        protected void printAppendix()
        {
            //TODO: Autor: John Ruiz; Fecha 24/09/2010; Asunto; Se valida el tipo de poliza para imprimir correctamente el titulo de los anexos; Compañia: 2
            if (PolicyData.PrefixNum == (int)Sistran.Company.Application.PrintingServices.Enums.PrefixCode.SURETY)
                this.File.Add(new DatRecord(Field + RptFields.FLD_POLICY_DATA_APPENDIX, string.Format(RptFields.TTL_SURETY_APPENDIX_HEADER, AppendixPageCount, PolicyNumber)));
            else
                this.File.Add(new DatRecord(Field + RptFields.FLD_POLICY_DATA_APPENDIX, string.Format(RptFields.TTL_APPENDIX_HEADER, this.AppendixPageCount, this.PrefixDescription, this.PolicyNumber)));
            this.File.Add(new DatRecord(Field + RptFields.FLD_ENDORSEMENT_TYPE_APPENDIX, EndorsementDescription));
            this.File.Add(new DatRecord(Field + RptFields.FLD_CERTIFIED_NUMBER_APPENDIX, EndorsementNumber));
            this.File.Add(new DatRecord(Field + RptFields.TTL_CERTIFIED_TYPE_APPENDIX, RptFields.LBL_CERTIFIED_TYPE_APPENDIX));
            this.File.Add(new DatRecord(Field + RptFields.TTL_DESCRIPTION_APPENDIX, string.Empty));
        }

        /// <summary>
        /// Imprime el titulo del reporte
        /// </summary>
        /// <param name="page">Número de página a imprimir</param>
        /// <param name="templateName">Nombre de la plantilla</param>
        protected void printTitle(int page, string templateName)
        {
            if (this.IsCollective && !HasTitle)
            {   
                if (page == 0)
                {
                    this.File.Add(new DatRecord(this.Job + this.PrinterName + this.PrefixName + this.PdfFilePath + this.Ass5AduonAti1, null));
                    this.HasTitle = true;
                }
            }
            else
            {
                if (page == 1 && !HasTitle)
                {
                    this.File.Add(new DatRecord(this.Job + this.PrinterName + this.PrefixName + this.PdfFilePath + this.Ass5AduonAti1, null));
                    this.HasTitle = true;
                }
                /*TODO <<Autor: Luis E Moreno Fecha: 28/03/2011 Asunto: En caso de imprimir polizas individuales con sustitucion de riesgo */
                else
                    if (page == 0)
                {
                    this.File.Add(new DatRecord(this.Job + this.PrinterName + this.PrefixName + this.PdfFilePath + this.Ass5AduonAti1, null));
                }
                
            }

            if (this.PolicyData.ReportType == (int)ReportType.COMPLETE_POLICY || this.PolicyData.ReportType == (int)ReportType.ONLY_POLICY ||
               this.PolicyData.ReportType == (int)ReportType.TEMPORARY || this.PolicyData.ReportType == (int)ReportType.QUOTATION)
            {
                this.File.Add(new DatRecord(this.Form + templateName + this.A1p1Aebon, null));
            }

            this.File.Add(new DatRecord(this.ReformatNo, null));

            //TODO: Autor: John Ruiz; Fecha 16/09/2010; Asunto: Se valida AppendixPageCount para que no imprima el PrefixCode en las hojas anexo; Compañia: 2
            if (AppendixPageCount != 0)
                this.File.Add(new DatRecord(Field + RptFields.FLD_BRANCH_CD_TITLE, string.Empty));
            else
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_BRANCH_CD_TITLE, this.PrefixCode));
        }

        /// <summary>
        /// Imprime la cabecera del reporte
        /// </summary>
        protected virtual void printHeader()
        {
            string insurance = string.Empty;
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_BRANCH_CD_TITLE, this.PrefixCode));
            
            insurance = string.Format(RptFields.LBL_INSURANCE_POLICY_TYPE_HEADER,
                                             PrefixDescription,
                                             PolicyTypeDescrition,
                                         this.Coinsured);
            
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_INSURANSE_HEADER, insurance));
            this.File.Add(new DatRecord(this.Field + RptFields.LBL_TITLE_POLICY_NUMBER_HEADER, null));

            this.setReportTitle();

            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ENDORSEMENT_TYPE_HEADER, EndorsementDescription));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ENDORSEMENT_NUMBER_HEADER, EndorsementNumber));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_LEAD_POLICY_ID_HEADER, this.MainNum));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_LEAD_ENDORSEMENT_NUM_HEADER, this.AnnexNum));

            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ISSUE_DATE_DAY, IssueDateDay));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ISSUE_DATE_MONTH, IssueDateMonth));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ISSUE_DATE_YEAR, IssueDateYear));

            this.File.Add(new DatRecord(this.Field + RptFields.FLD_EXPEDITION_DATE_DAY, IssueDateDay));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_EXPEDITION_DATE_MONTH, IssueDateMonth));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_EXPEDITION_DATE_YEAR, IssueDateYear));

            this.File.Add(new DatRecord(this.Field + RptFields.FLD_EXPEDITION_DATE_YEAR, IssueDateYear));
        }

        /// <summary>
        /// Agrega el titu lo del reporte
        /// </summary>
        protected virtual void setReportTitle()
        {
            this.File.Add(new DatRecord(RptFields.LBL_POLICY_NUMBER_HEADER, null));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_POLICY_NUMBER_HEADER, PolicyNumber));
        }

        /// <summary>
        /// Adiciona los datos del tomador
        /// </summary>
        protected void printPolicyHolderData()
        {
            if (isQuotation == false)
            {
                this.File.Add(new DatRecord(this.Field + RptFields.TTL_POLICY_HOLDER_SIGN_PHOLDER, RptFields.LBL_POLICYHOLDER_SING));
            }
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_POLICY_HOLDER_PHOLDER, RptFields.LBL_POLICY_HOLDER_PHOLDER));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_POLICY_HOLDER_PHOLDER, this.HolderName));
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_ADDRESS_PHOLDER, RptFields.LBL_ADDRESS_PHOLDER));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ADDRESS_PHOLDER, this.HolderAddress));
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_PHONE_PHOLDER, RptFields.LBL_PHONE_PHOLDER));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_PHONE_NUMBER_PHOLDER, this.HolderPhone));
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_TRIBUTARY_ID_PHOLDER, this.HolderDocumentType));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_TRIBUTARY_ID_PHOLDER, this.HolderDocumentNumber));
        }

        /// <summary>
        /// Adiciona datos del tomador
        /// </summary>
        protected virtual void printIssuanceData()
        {
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_CURRENCY_PHOLDER, this.CurrencyDescription));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_EXCHANGE_CD_PHOLDER, this.ExchangeRate));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_FIELD_BRANCH_CD_PHOLDER, this.BranchCode));
            //TODO: Autor: John Ruiz; Fecha: 27/09/2010; Asunto: Se agrega el nombre de la ciudad de expedicion para llenar el campo EMITIDO EN; compañia: 2
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_SUSCRIPTION_CITY_PHOLDER, this.BranchCity));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_OPERATION_CENTER_PHOLDER, this.SalePointCd));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_APPROPIATION_PHOLDER, this.AppropiationPres));
            if (isQuotation == false)
            {
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_LOAD_TO_PHOLDER, this.LoadToPHolder));
            }
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ISSUANCE_DATE_PHOLDER, this.CurrentDate));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_DAY_FROM_PHOLDER, this.DayFrom));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_MONTH_FROM_PHOLDER, this.MonthFrom));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_YEAR_FROM_PHOLDER, this.YearFrom));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_HOUR_FROM_PHOLDER, this.HourFrom));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_DAY_TO_PHOLDER, this.DayTo));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_MONTH_TO_PHOLDER, this.MonthTo));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_YEAR_TO_PHOLDER, this.YearTo));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_HOUR_TO_PHOLDER, this.HourTo));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_DAYS_PHOLDER, this.Days));
            if (isQuotation == false)
            {
                this.File.Add(new DatRecord(this.Field + RptFields.IMG_DIGITAL_SIGNATURE, this.DigitalizedSignature + this.DigitalSignature));
            }
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_SUSCRIPTION_CITY_PHOLDER, this.BranchCity));
        }

        /// <summary>
        /// Adiciona datos de comisión y coasegurado
        /// </summary>
        protected void printCommissionsAndCoinsuranceData()
        {
            String currencySymbol = this.Pesos;

            //TODO: Autor: John Ruiz; Fecha: 16/09/2010; Asunto: Se valida si la longitud de AgreedPaymentMethod es > 19 para cortarla; Compañia: 2
            if (isQuotation == false)
            {
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_PAYMENT_METHOD_COMMISSION, this.AgreedPaymentMethod.Length > 19 ? this.AgreedPaymentMethod.Substring(0, 19) : this.AgreedPaymentMethod));
            }
            else
            {
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_PAYMENT_METHOD_COMMISSION, null));
            }
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_INSURED_AMT_COMMISSION, (ReportServiceHelper.completeCurrency(this.InsuredTotalValue, 10, this.CurrencySymbol, "*"))));//Overall
            this.File.Add(new DatRecord(this.Field + RptFields.SCT_INTERMEDIARIES_COMMISSION, null));

            //TODO: RECORRER LISTA DE INTERMEDIARIOS Y ESCRIBIR TODOS LOS DATOS
            int intermediariesCounter = 0;
            foreach (DataRow row in Intermediaries.Rows)
            {
                this.File.Add(new DatRecord(string.Empty, string.Format(RptFields.FLD_INTERMEDIARIES,
                                                                        ReportServiceHelper.completeColumnCoverages(row["Id"].ToString(), 6),
                                                                        ReportServiceHelper.completeColumnCoverages(row["Type"].ToString(), 1),
                                                                        ReportServiceHelper.completeColumnCoverages(row["Name"].ToString(), 20),
                                                                        //cesar giraldo 31/10/2013 asunto cambio de alineación de impresión 
                                                                        this.Iscopy ? ReportServiceHelper.rightAlign(row["Percentage"].ToString(), 7) : string.Empty,
                                                                        this.Iscopy ? ReportServiceHelper.rightAlign(row["Commission"].ToString(), 15) : string.Empty)));
                //cesar giraldo 31/10/2013 asunto cambio de alineación de impresión 
                intermediariesCounter++;
                if ((Intermediaries.Rows.Count > 7) && (intermediariesCounter == 6))
                {
                    this.File.Add(new DatRecord(string.Empty, string.Format(null, "Intermediarios continúa en Hojas de Anexos...")));
                    break;
                }
            }

            //TODO: Autor: John Ruiz; Fecha: 12/11/2010; Asunto: se agrega condicion para que imprima los valores de manera correcta cuando la poliza sea emitida en dolares; compañia: 2
            if (Convert.ToDecimal(ExchangeRate) != 1)
            {
                if (string.IsNullOrEmpty(this.Premium))
                    this.Premium = "0";

                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_PREMIUM_AMT_COMMISSION, ReportServiceHelper.completeCurrency(this.Premium, 16, this.CurrencySymbol, "*")));
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_EXPENSES_COMMISSION, ReportServiceHelper.completeCurrency(this.Expenses, 16, this.CurrencySymbol, "*")));
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_TAX_COMMISSION, ReportServiceHelper.completeCurrency(this.Tax, 16, "$", "*")));

                string USDOverall = (Convert.ToDecimal(Premium) + Convert.ToDecimal(Expenses)).ToString();

                this.File.Add(new DatRecord(this.Field + RptFields.FLD_USD_OVERALL_COMMISSION, ReportServiceHelper.completeCurrency(USDOverall.ToString(), 16, this.CurrencySymbol, "*")));

                this.Overall = Tax;
                this.CurrencyAjustment = ReportServiceHelper.formatMoney((Decimal.Round(Convert.ToDecimal(Overall)) - Convert.ToDecimal(Overall)).ToString(), CultureInfo.CurrentCulture);

                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_CURRENCY_ADJUSTMENT_COMMISSION, ReportServiceHelper.completeCurrency(this.CurrencyAjustment, 16, "$", "*")));
                this.Overall = ReportServiceHelper.formatMoney((Convert.ToDecimal(Overall) + Convert.ToDecimal(CurrencyAjustment)).ToString(), CultureInfo.CurrentCulture);
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_OVERALL_COMMISSION, ReportServiceHelper.completeCurrency(this.Overall, 16, "$", "*")));
                this.File.Add(new DatRecord(this.Field + RptFields.TTL_USD_OVERALL_COMMISSION, RptFields.LBL_USD_OVERALL_COMMISSION));
            }
            else
            {
                if (string.IsNullOrEmpty(this.Premium))
                    this.Premium = "0";

                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_PREMIUM_AMT_COMMISSION, ReportServiceHelper.completeCurrency(this.Premium, 16, this.CurrencySymbol, "*")));
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_TAX_COMMISSION, ReportServiceHelper.completeCurrency(this.Tax, 16, "$", "*")));
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_EXPENSES_COMMISSION, ReportServiceHelper.completeCurrency(this.Expenses, 16, "$", "*")));

                this.Overall = (Convert.ToDecimal(Premium) + Convert.ToDecimal(Tax) + Convert.ToDecimal(Expenses)).ToString();

                this.CurrencyAjustment = ReportServiceHelper.formatMoney((Decimal.Round(Convert.ToDecimal(Overall)) - Convert.ToDecimal(Overall)).ToString(), CultureInfo.CurrentCulture);

                this.Overall = ReportServiceHelper.formatMoney((Convert.ToDecimal(Overall) + Convert.ToDecimal(CurrencyAjustment)).ToString(), CultureInfo.CurrentCulture);

                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_OVERALL_COMMISSION, ReportServiceHelper.completeCurrency(this.Overall, 16, this.CurrencySymbol, "*")));

                this.File.Add(new DatRecord(this.Field + RptFields.FLD_COP_CURRENCY_ADJUSTMENT_COMMISSION, ReportServiceHelper.completeCurrency(this.CurrencyAjustment, 16, this.CurrencySymbol, "*")));
            }

            this.File.Add(new DatRecord(this.Field + RptFields.SCT_COINSURANCE_COMMISSION, null));

            //TODO: Julio Guzmán, 20/01/2011, Se valida que el tipo de coaseguro no sea Aceptado
            if (this.Coinsurance != null)
            {
                foreach (DataRow row in this.Coinsurance.Rows)
                    this.File.Add(new DatRecord(string.Empty, string.Format(RptFields.FLD_COINSURANCE_COMMISSION, ReportServiceHelper.completeColumnCoverages(row["Code"].ToString(), 5), ReportServiceHelper.completeColumnCoverages(row["Company"].ToString(), 23), ReportServiceHelper.completeColumnCoverages(row["Percentage"].ToString(), 7), row["Premium"])));
            }
            this.File.Add(new DatRecord(Field + RptFields.FLD_ISSUANCE_DATE_PHOLDER, string.Empty));
        }

        /// <summary>
        /// Adiciona datos del asegurado
        /// </summary>
        protected void printInsuredData()
        {
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_INSURED, RptFields.LBL_INSURED));

            if (this.ActRiskNum != 0)
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_HOLDER_NAME_INSURED, this.InsuredName));
            else
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_HOLDER_NAME_INSURED, RptFields.LBL_COLLECTIVE_ITEMS));

            this.File.Add(new DatRecord(this.Field + RptFields.TTL_ADDRESS_INSURED, RptFields.LBL_ADDRESS_INSURED));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_ADDRESS_INSURED, this.InsuredAddress));
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_TRIBUTARY_ID_INSURED, this.InsuredDocumentType));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_TRIBUTARY_ID_INSURED, this.InsuredDocumentNumber));
            this.File.Add(new DatRecord(this.Field + RptFields.TTL_PHONE_NUMBER_INSURED, RptFields.LBL_PHONE_NUMBER_INSURED));
            this.File.Add(new DatRecord(this.Field + RptFields.FLD_PHONE_NUMBER_INSURED, this.InsuredPhone));

        }

        //TODO: Autor: John Ruiz; Fecha 29/09/2010; Asunto: Adiciona al reporte el texto de la póliza; Compañia 2;
        /// <summary>
        /// Adiciona al reporte el texto de la póliza
        /// </summary>
        protected void PrintGeneralLevelText()
        {
            if (!GeneralText.Trim().Equals(string.Empty))
            {
                //TODO: Autor: John Ruiz; Fecha: 26/10/2010; Asunto: Se mueve dentro del if para que no agregue hoja en blanco; Compañia:2
                validatePageLimit(1);
                this.File.Add(new DatRecord("\n", null));
                string[] GeneralTextParagraphs = GeneralText.Split('\n');

                PrintParagraphArray(GeneralTextParagraphs);
            }
        }

        //TODO: Autor: John Ruiz; Fecha 29/09/2010; Asunto: Adiciona al reporte el texto a nivel de riesgo de la póliza; Compañia 2;
        /// <summary>
        /// Adiciona al reporte el texto a nivel de riesgo
        /// </summary>
        protected void PrintRiskLevelText()
        {
            if (!RiskLeveText.Trim().Equals(string.Empty))
            {
                //TODO: Autor: John Ruiz; Fecha: 26/10/2010; Asunto: Se mueve dentro del if para que no agregue hoja en blanco; Compañia:2
                validatePageLimit(1);
                this.File.Add(new DatRecord("\n", null));
                string[] riskLevelParagraphs = RiskLeveText.Split('\n');

                PrintParagraphArray(riskLevelParagraphs);
            }

        }

        //TODO: Autor: John Ruiz; Fecha 13/10/2010; Asunto: Imprime un arreglo de parrafos; Compañia 2;
        /// <summary>
        /// Adiciona al reporte los parrafos que contenga el array
        /// </summary>
        protected void PrintParagraphArray(string[] paragraphs)
        {
            foreach (string line in paragraphs)
            {
                //TODO: Autor: John Ruiz; Fecha: 23/09/2010; Asunto: Se reemplaza el retorno de linea por vacio para evitar error en el conteo del limite de las lineas; Compañia: 2
                string lineToAdd = line.Replace('\r', ' ');
                if (lineToAdd.Length > 0)
                {
                    //TODO: Autor: John Ruiz; Fecha: 23/09/2010; Asunto: Se valida por parrafos para evitar errores en los anexos de pagina; Compañia: 2
                    this.validatePageLimit(ReportServiceHelper.countLinesFor(lineToAdd));
                    this.File.Add(new DatRecord(lineToAdd, null));
                }
            }

        }


        /// <summary>
        /// Valida si se ha llegado al límite de la página en proceso de impresión.
        /// </summary>
        /// <param name="linesToAdd">Número de líneas a adicionar</param>
        protected void validatePageLimit(int linesToAdd)
        {
            if ((PageLinesCount + linesToAdd) >= LimitPageLines)
            {
                this.File.Add(new DatRecord(RptFields.LBL_NEXT_PAGE, null));
                this.AppendixPageCount += 1;
                this.printReportCoverAppendix();
                this.PageLinesCount = 0;
                this.PageLinesCount += linesToAdd;
                this.LimitPageLines = 65;
            }
            else
            {
                this.PageLinesCount += linesToAdd;
            }
        }

        /// <summary>
        /// Adiciona al reporte el anexo de la caratula de la póliza
        /// </summary>
        protected void printReportCoverAppendix()
        {
            this.printTitle(2, RptFields.LBL_VEHICLE_COVER_APPENDIX_NAME);
            this.printAppendix();
        }

        /// <summary>
        /// Inicializa la tabla de datos del convenio de pago de la póliza
        /// </summary>
        protected void initializePaymentCheckDataTable()
        {
            this.Payment = new DataTable();
            this.Payment.Columns.Add("DatePayment", typeof(String));
            this.Payment.Columns.Add("CurrencySymbol", typeof(String));
            this.Payment.Columns.Add("Expences", typeof(String));
            this.Payment.Columns.Add("PremiumAmt", typeof(String));
            this.Payment.Columns.Add("Tax", typeof(String));
        }

        /// <summary>
        /// Inicializa la tabla de datos del convenio de pago
        /// </summary>
        /// <param name="risk"></param>
        protected void setPaymentCheck(DataSet risk)
        {
            this.PaymentCheckDataSet = risk.Copy();
            this.PaymentCheckDataSet.Tables.Remove("Table1");
            this.PaymentCheckDataSet.Tables.Remove("Table2");
            this.PaymentCheckDataSet.Tables.Remove("Table3");
            this.PaymentCheckDataSet.Tables.Remove("Table4");
            this.PaymentCheckDataSet.Tables.Remove("Table5");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Obtiene los datos del reporte con la ejecución de los sp SET y GET correspondientes
        /// </summary>
        /// <param name="setSP">Procedimiento almacenado que carga en las tablas del esquema REPORT (BD), los datos del reporte</param>
        /// <param name="paramSetSp">Parámetros de setSP</param>
        /// <param name="getSP">Procedimiento almacenado que consulta las tablas del esquema REPORT (BD), los datos del reporte</param>
        /// <param name="paramGetSp">Parámetros de getSP</param>
        /// <returns>Set de datos del reporte</returns>
        private DataSet getReportData(string setSP, NameValue[] paramSetSp, string getSP, NameValue[] paramGetSp)
        {
            ReportServiceHelper.getData(setSP, paramSetSp);
            return ReportServiceHelper.getData(getSP, paramGetSp);
        }

        #endregion
    }
}
