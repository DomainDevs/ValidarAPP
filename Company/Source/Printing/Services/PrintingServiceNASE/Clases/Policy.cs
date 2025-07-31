using System;
using System.Data;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class Policy
    {
        #region Propierties
        private int _processId;
        private int _requestId;
        private int _reportType;
        private int _policyId;
        private int _endorsementId;
        private int _quotationId;
        private string _user;
        private string _codeBar;
        private int _tempNum;
        private int _prefixNum;
        private int _rangeMinValue;
        private int _rangeMaxValue;
        private bool _withFormatCollect;
        private int _riskAnt;
        private int _idPv2g;
        private string _licensePlate;
        private bool _showPremium;
        private int _copiesQuantity;
        private bool _currentFromFirst;
        private bool _endorsementText;
        private bool _tempAuthorization;
        // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
        private int _balancePremium;
        // Edgar O. Piraneque E.; 05/11/2010;>>

        //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 27/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
        private int _renewalProcessId;
        /* Autor: Luisa Fernanda Ramírez, Fecha: 21/12/2010 >>*/

        //TODO:  <<Autor: Miguel López; Fecha: 27/12/2010; Asunto: Agregamos bandera de impresión de pagaré
        private int _printPromissoryNote;
        /* Autor: Miguel López, Fecha: 21/12/2010 >>*/


        public int ProcessId
        {
            get { return _processId; }
            set { _processId = value; }
        }

        public int RequestId
        {
            get { return _requestId; }
            set { _requestId = value; }
        }

        public int ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
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

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public string CodeBar
        {
            get { return _codeBar; }
            set { _codeBar = value; }
        }

        public int TempNum
        {
            get { return _tempNum; }
            set { _tempNum = value; }
        }

        public int PrefixNum
        {
            get { return _prefixNum; }
            set { _prefixNum = value; }
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

        public bool WithFormatCollect
        {
            get { return _withFormatCollect; }
            set { _withFormatCollect = value; }
        }

        public int RiskAnt
        {
            get { return _riskAnt; }
            set { _riskAnt = value; }
        }

        public int IdPv2g
        {
            get { return _idPv2g; }
            set { _idPv2g = value; }
        }

        public string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }

        public bool ShowPremium
        {
            get { return _showPremium; }
            set { _showPremium = value; }
        }
        // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
        public int BalancePremium
        {
            get { return _balancePremium; }
            set { _balancePremium = value; }
        }
        // Edgar O. Piraneque E.; 05/11/2010;>>

        public int CopiesQuantity
        {
            get { return _copiesQuantity; }
            set { _copiesQuantity = value; }
        }
        //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 27/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
        public int RenewalProcessId
        {
            get { return _renewalProcessId; }
            set { _renewalProcessId = value; }
        }
        /* Autor: Luisa Fernanda Ramírez, Fecha: 21/12/2010 >>*/


        public int PrintPromissoryNote
        {
            get { return _printPromissoryNote; }
            set { _printPromissoryNote = value; }
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

        #endregion

        public Policy()
        {
            ProcessId = 0;
            RequestId = 0;
            ReportType = 0;
            PolicyId = 0;
            EndorsementId = 0;
            QuotationId = 0;
            User = string.Empty;
            CodeBar = string.Empty;
            TempNum = 0;
            PrefixNum = 0;
            RangeMinValue = 0;
            RangeMaxValue = 0;
            WithFormatCollect = false;
            RiskAnt = 0;
            IdPv2g = 0;
            LicensePlate = string.Empty;
            ShowPremium = true;
            BalancePremium = 0;
            CopiesQuantity = 1;
            PrintPromissoryNote = 0;
            CurrentFromFirst = false;
            EndorsementText = false;
            TempAuthorization = false;
        }

        public Policy(int processId, int requestId, int reportType, int policyId, int endorsementId, int quotationId,
                      string user, string codeBar, int tempNum, int prefixNum, int rangeMinValue,
                      int rangeMaxValue, bool withFormatCollect, int riskAnt, int idPv2g, int balancePremium)
        {
            ProcessId = processId;
            RequestId = requestId;
            ReportType = reportType;
            PolicyId = policyId;
            EndorsementId = endorsementId;
            QuotationId = quotationId;
            User = user;
            CodeBar = codeBar;
            TempNum = tempNum;
            PrefixNum = prefixNum;
            RangeMinValue = rangeMinValue;
            RangeMaxValue = rangeMaxValue;
            WithFormatCollect = withFormatCollect;
            RiskAnt = riskAnt;
            IdPv2g = idPv2g;
            LicensePlate = string.Empty;
        }

        public Policy(int processId, int requestId, int reportType, int policyId, int endorsementId, int quotationId,
              string user, string codeBar, int tempNum, int prefixNum, int rangeMinValue,
              int rangeMaxValue, bool withFormatCollect, int riskAnt, int idPv2g, string licensePlate, bool showPremium, int copiesQuantity, int balancePremium, int printPromissoryNote)
        {
            ProcessId = processId;
            RequestId = requestId;
            ReportType = reportType;
            PolicyId = policyId;
            EndorsementId = endorsementId;
            QuotationId = quotationId;
            User = user;
            CodeBar = codeBar;
            TempNum = tempNum;
            PrefixNum = prefixNum;
            RangeMinValue = rangeMinValue;
            RangeMaxValue = rangeMaxValue;
            WithFormatCollect = withFormatCollect;
            RiskAnt = riskAnt;
            IdPv2g = idPv2g;
            LicensePlate = licensePlate;
            ShowPremium = showPremium;
            BalancePremium = balancePremium;
            CopiesQuantity = copiesQuantity;
            PrintPromissoryNote = printPromissoryNote;
        }

        public Policy(DataRow dr)
        {
            ProcessId = Convert.ToInt32(dr["PrintProccesId"].ToString());
            ReportType = Convert.ToInt32(dr["TypeReport"].ToString());
            PolicyId = Convert.ToInt32(dr["PolicyId"].ToString());
            EndorsementId = Convert.ToInt32(dr["EndorsementId"].ToString());
            QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
            User = dr["User"].ToString();
            CodeBar = dr["CodeBar"].ToString();
            TempNum = Convert.ToInt32(dr["TempNum"].ToString());
            PrefixNum = Convert.ToInt32(dr["PrefixNum"].ToString());
            RangeMinValue = Convert.ToInt32(dr["RangeMinValue"].ToString());
            RangeMaxValue = Convert.ToInt32(dr["RangeMaxValue"].ToString());
            WithFormatCollect = Convert.ToBoolean(dr["WithFormatCollect"].ToString());
            IdPv2g = Convert.ToInt32(dr["IdPv"].ToString());
            LicensePlate = dr["LicensePlate"].ToString();
            ShowPremium = Convert.ToBoolean(dr["ShowPremium"].ToString());
            // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
            BalancePremium = Convert.ToInt32(dr["BalancePremium"].ToString());
            // Edgar O. Piraneque E.; 05/11/2010;>>
            CopiesQuantity = Convert.ToInt32(dr["CopiesQuantity"].ToString());
            PrintPromissoryNote = 0;//Convert.ToInt32(dr["PrintPromissoryNote"].ToString());
            CurrentFromFirst = Convert.ToBoolean(dr["CurrentFromFirst"].ToString());
            EndorsementText = Convert.ToBoolean(dr["EndorsementText"].ToString());
            TempAuthorization = Convert.ToBoolean(dr["TempAuthorization"].ToString());
        }

        //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 27/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
        public Policy(int processId, int reportType, int renewalProcessId)
        {
            ProcessId = processId;
            ReportType = reportType;
            RenewalProcessId = renewalProcessId;
        }
    }
}
