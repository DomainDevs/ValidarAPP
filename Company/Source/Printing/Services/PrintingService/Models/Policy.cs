using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
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
        }

        public Policy(int processId, int requestId, int reportType, int policyId, int endorsementId, int quotationId,
                      string user, string codeBar, int tempNum, int prefixNum, int rangeMinValue,
                      int rangeMaxValue, bool withFormatCollect, int riskAnt)
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
        }
    }
}
