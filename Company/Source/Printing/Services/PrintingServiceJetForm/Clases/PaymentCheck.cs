using Sistran.Company.Application.PrintingServices.Resources;
using Sistran.Company.PrintingService.JetForm.Clases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    class PaymentCheck
    {
        #region Properties

        private String field
        {
            get { return RptFields.LBL_FIELD_REPORT; }
            set { }
        }

        private DataTable Payment;

        private ArrayList fileLines;

        public ArrayList FileLines
        {
            get { return fileLines; }
            set { fileLines = value; }
        }

        private String ReformatNo
        {
            get { return RptFields.LBL_REFORMAT_NO_REPORT; }
            set { }
        }

        private String form
        {
            get { return RptFields.LBL_FORM_REPORT; }
            set { }
        }

        private String a1p1Aebon
        {
            get { return RptFields.LBL_AIP1_AEBON_REPORT; }
            set { }
        }

        private String PolicyNumber;
        private String EndorsementNumber;
        private String PrefixCode;
        private String PrefixDescription;
        private String BranchDescription;
        private String CurrencySymbol;
        private String Premium;
        private String Tax;
        private String Expenses;
        private String ExchangeRate;
        private String Overall;
        private String CurrencyAjustment;
        private String PaymentMethod;
        private String HolderName;
        private String HolderNamePayment;
        private String InsuredNamePayment;
        private String BranchCity;
        private String TotalInsuredValue;

        #endregion

        #region Constructors

        public PaymentCheck(DataSet ds, String policyNumber, String totalInsuredValue)
        {
            this.initializeList();

            this.TotalInsuredValue = totalInsuredValue;
            this.PolicyNumber = policyNumber;
            this.EndorsementNumber = ds.Tables["Table"].Rows[0]["DOCUMENT_NUM"].ToString();
            this.PrefixCode = ds.Tables["Table"].Rows[0]["PREFIX_CD"].ToString();
            this.PrefixDescription = ds.Tables["Table"].Rows[0]["PREFIX"].ToString();
            this.BranchDescription = ds.Tables["Table"].Rows[0]["BRANCH"].ToString();
            this.CurrencySymbol = ds.Tables["Table"].Rows[0]["CURRENCY_SYMBOL"].ToString();
            this.Premium = ReportServiceHelper.formatMoney(ds.Tables["table"].Rows[0]["PREMIUM_AMT"].ToString(), CultureInfo.CurrentCulture);
            this.Premium = ds.Tables["table"].Rows[0]["PREMIUM_AMT"].ToString() == string.Empty ? "0" : ReportServiceHelper.formatMoney(ds.Tables["table"].Rows[0]["PREMIUM_AMT"].ToString(), CultureInfo.CurrentCulture);
            this.Tax = ds.Tables["table"].Rows[0]["TAX"].ToString() == string.Empty ? "0" : ReportServiceHelper.formatMoney(ds.Tables["table"].Rows[0]["TAX"].ToString(), CultureInfo.CurrentCulture);
            this.Expenses = ds.Tables["table"].Rows[0]["EXPENSES"].ToString() == string.Empty ? "0" : ReportServiceHelper.formatMoney(ds.Tables["table"].Rows[0]["EXPENSES"].ToString(), CultureInfo.CurrentCulture);
            this.ExchangeRate = ReportServiceHelper.formatMoney(ds.Tables["Table"].Rows[0]["EXCHANGE_RATE"].ToString(), CultureInfo.CurrentCulture);

            if (this.CurrencySymbol.Equals("$"))
            {
                this.Overall = (Convert.ToDecimal(Premium) + Convert.ToDecimal(Tax) + Convert.ToDecimal(Expenses)).ToString();
                this.CurrencyAjustment = ReportServiceHelper.formatMoney((Decimal.Round(Convert.ToDecimal(Overall)) - Convert.ToDecimal(Overall)).ToString(), CultureInfo.CurrentCulture);
                this.Overall = ReportServiceHelper.formatMoney((Convert.ToDecimal(Overall) + Convert.ToDecimal(CurrencyAjustment)).ToString(), CultureInfo.CurrentCulture);
            }
            else
            {
                this.Overall = ReportServiceHelper.formatMoney((Convert.ToDecimal(Premium) + Convert.ToDecimal(Tax) + Convert.ToDecimal(Expenses)).ToString(), CultureInfo.CurrentCulture);
                this.Tax = (Convert.ToDecimal(this.Tax) * Convert.ToDecimal(this.ExchangeRate)).ToString();
                this.Expenses = (Convert.ToDecimal(this.Expenses) * Convert.ToDecimal(this.ExchangeRate)).ToString();
                this.CurrencyAjustment = string.Empty;
            }

            this.PaymentMethod = ds.Tables["table"].Rows[0]["AGREED_PAYMENT_METHOD"].ToString();

            this.HolderName = String.Format(RptFields.FLD_NAME_FORMAT,
                             ds.Tables["Table"].Rows[0]["POLICY_HOLDER_ID"].ToString(),
                             ds.Tables["Table"].Rows[0]["POLICY_HOLDER_NAME"].ToString());
            //TODO: Autor: John Ruiz; Fecha 22/10/2010; Asunto: Se agrega para asignar el nombre del policyholder al convenio de pago y no el del asegurado; Compañia: 2
            this.HolderNamePayment = ds.Tables["Table"].Rows[0]["POLICY_HOLDER_NAME"].ToString();
            this.InsuredNamePayment = ds.Tables["Table"].Rows[0]["INSURED_NAME"].ToString();
            this.BranchCity = ds.Tables["Table"].Rows[0]["BRANCH_CITY"].ToString();

            this.initializePaymentCheckDataTable();

            foreach (DataRow row in ds.Tables["Table6"].Rows)
            {
                DataRow newRow = Payment.NewRow();
                newRow["DatePayment"] = row["DATE_PAYMENT"];
                newRow["CurrencySymbol"] = CurrencySymbol;
                newRow["Expenses"] = ReportServiceHelper.formatMoney(Convert.ToDecimal(row["EXPENCES"]).ToString(), CultureInfo.CurrentCulture);
                newRow["PremiumAmt"] = ReportServiceHelper.formatMoney(Convert.ToDecimal(row["PREMIUM_AMT"]).ToString(), CultureInfo.CurrentCulture);
                //TODO: Autor: Julio Guzmán; Fecha: 30/12/2010; Asunto: se corrige error con la impresion en las polizas de IVA diferente en la parte de convenio de pago y en las paginas anteriores; Compañia: 2
                newRow["Tax"] = ReportServiceHelper.formatMoney(Convert.ToDecimal(row["Tax"]).ToString(), CultureInfo.CurrentCulture);
                Payment.Rows.Add(newRow);
            }

            printPaymentCheck();
        }

        #endregion

        #region Methods

        private void initializeList()
        {
            fileLines = new ArrayList();
        }

        private void initializePaymentCheckDataTable()
        {
            Payment = new DataTable();
            Payment.Columns.Add("DatePayment", typeof(String));
            Payment.Columns.Add("CurrencySymbol", typeof(String));
            Payment.Columns.Add("Expenses", typeof(String));
            Payment.Columns.Add("PremiumAmt", typeof(String));
            Payment.Columns.Add("Tax", typeof(String));
        }

        private void printTitle(int page, string templateName)
        {

            fileLines.Add(new DatRecord(null, null));
            fileLines.Add(new DatRecord(form + templateName + a1p1Aebon, null));
            fileLines.Add(new DatRecord(ReformatNo, null));
            fileLines.Add(new DatRecord(field + RptFields.FLD_BRANCH_CD_TITLE, string.Empty));
        }

        private void printPaymentCheck()
        {
            printTitle(3, RptFields.LBL_VEHICLE_PAYMENT_CHECK_NAME);
            fileLines.Add(new DatRecord(field + RptFields.FLD_POLICY_NUMBER_PAYMENT, this.PolicyNumber));
            fileLines.Add(new DatRecord(field + RptFields.FLD_ENDORSEMENT_NUMBER_PAYMENT, this.EndorsementNumber));
            fileLines.Add(new DatRecord(field + RptFields.FLD_PREFIX_CD_PAYMENT, this.PrefixDescription));
            fileLines.Add(new DatRecord(field + RptFields.FLD_BRANCH_CD_PAYMENT, this.BranchDescription));
            fileLines.Add(new DatRecord(field + RptFields.FLD_PREMIUM_AMT_PAYMENT, this.CurrencySymbol + this.Premium));
            fileLines.Add(new DatRecord(field + RptFields.FLD_TAX_VALUE_PAYMENT, "$" + ReportServiceHelper.formatMoney(this.Tax, CultureInfo.CurrentCulture)));
            fileLines.Add(new DatRecord(field + RptFields.FLD_HOLDER_NAME_PAYMENT, this.HolderName));

            fileLines.Add(new DatRecord(field + RptFields.FLD_FIRST_PAYMENT_CONDITION, getPaymentConditions()));
            fileLines.Add(new DatRecord(field + RptFields.FLD_SCDN_PAYMENT_CONDITION, string.Empty));

            fileLines.Add(new DatRecord(field + RptFields.FLD_COMPLIANCE_NUMBER, string.Format(RptFields.LBL_PAYMENT_AGREEMENT_FORMAT,
                RptFields.LBL_PAYMENT_AGREEMENT, PaymentMethod)));
            //TODO: Autor: John Ruiz; Fecha 22/10/2010; Asunto: Se asignar el nombre del policyholder al convenio de pago y no el del asegurado; Compañia: 2
            //fileLines.Add(new DATRecord(field + RptFields.FLD_PAYMENT_RESPONSIBLE, this.InsuredNamePayment));
            fileLines.Add(new DatRecord(field + RptFields.FLD_PAYMENT_RESPONSIBLE, this.HolderNamePayment));
            fileLines.Add(new DatRecord(field + RptFields.FLD_PAYMENT_TOTAL_VALUE, ReportServiceHelper.formatMoney(this.CurrencySymbol + Convert.ToDecimal(Overall).ToString(), CultureInfo.CurrentCulture)));

            string[] paymentTerms = getPaymentTerms().Split('\n');
            //TODO: Autor: John Ruiz; Fecha: 04/10/2010; Asunto: se corrige error con la impresion de la cuotas de plan de pagos segunda columna; Compañia: 2
            string payment1 = string.Empty;
            string payment2 = string.Empty;


            for (int i = 0; i < paymentTerms.Length; i++)
            {
                if (i <= 5)
                {
                    payment1 += paymentTerms[i] + "\n";
                }
                else
                {
                    payment2 += paymentTerms[i] + "\n";
                }
            }

            fileLines.Add(new DatRecord(field + RptFields.FLD_PAYMENT_FIRST_TERMS, payment1));
            fileLines.Add(new DatRecord(field + RptFields.FLD_PAYMENT_SCDN_TERMS, payment2));

            fileLines.Add(new DatRecord(field + RptFields.FLD_PAYMENT_POLICIES, getPolicyData()));

            fileLines.Add(new DatRecord(field + RptFields.FLD_CONSTANCY_PAYMENT_SIGNATURE,
                string.Format(RptFields.FLD_CONSTANCY_PAYMENT_SIGNATURE_TEXT,
                BranchCity, string.Format(RptFields.LBL_DATE_FORMAT, DateTime.Now))));
        }

        private string getPaymentConditions()
        {
            String paymentConditions = string.Empty;

            foreach (DataRow row in Payment.Rows)
            {
                string paymentTax = row["Tax"].ToString();
                paymentTax = (Convert.ToDecimal(paymentTax) * Convert.ToDecimal(this.ExchangeRate)).ToString();

                string paymentExpenses = row["Expenses"].ToString();
                paymentExpenses = (Convert.ToDecimal(paymentExpenses) * Convert.ToDecimal(this.ExchangeRate)).ToString();

                paymentConditions += row["DatePayment"] + string.Empty.PadLeft(10) +
                                     ReportServiceHelper.completeCurrency(paymentExpenses, 16, "$", "*") +
                                     string.Empty.PadLeft(10) +
                                     ReportServiceHelper.completeCurrency(row["PremiumAmt"].ToString(), 16, row["CurrencySymbol"].ToString().Trim(), "*") +
                                     string.Empty.PadLeft(10) +
                                     ReportServiceHelper.completeCurrency(paymentTax, 16, "$", "*") + "\n";
            }
            return paymentConditions;
        }

        private string getPaymentTerms()
        {
            String paymentTerms = string.Empty;
            int i = 1;

            foreach (DataRow row in Payment.Rows)
            {
                string paymentTax = row["Tax"].ToString();
                paymentTax = (Convert.ToDecimal(paymentTax) * Convert.ToDecimal(this.ExchangeRate)).ToString();

                string paymentExpenses = row["Expenses"].ToString();
                paymentExpenses = (Convert.ToDecimal(paymentExpenses) * Convert.ToDecimal(this.ExchangeRate)).ToString();
                //TODO: Autor: John Ruiz; Fecha: 04/10/2010; Asunto: Se corrige tabulado de la cuotas; Compañia:2
                paymentTerms += string.Empty.PadLeft(3) + (i < 10 ? i.ToString() + "  " : i.ToString()) +
                                row["DatePayment"].ToString().PadLeft(22) +
                                string.Empty.PadLeft(7) +
                                ReportServiceHelper.completeCurrency(paymentExpenses, 16, "$", "*") +
                                string.Empty.PadLeft(12) +
                                ReportServiceHelper.completeCurrency(row["PremiumAmt"].ToString(), 16, row["CurrencySymbol"].ToString().Trim(), "*") +
                                string.Empty.PadLeft(9) +
                                ReportServiceHelper.completeCurrency(paymentTax, 16, "$", "*") + "\n";
                i++;
            }
            return paymentTerms;
        }


        private string getPolicyData()
        {
            String PolicyData = string.Empty;
            //int i = 1;
            //TODO: Autor: John Ruiz; Fecha 16/09/2010; Asunto: Se comenta el foreach para que no duplique registros; Compañia: 2
            //foreach (DataRow row in Payment.Rows)
            //{
            PolicyData += string.Empty.PadLeft(5) +
                PolicyNumber.PadRight(23) +
                string.Empty.PadRight(1) +
                PrefixDescription.PadRight(26) +
                string.Empty.PadLeft(1) +
                EndorsementNumber.PadLeft(19) +
                string.Empty.PadLeft(13) +
                (CurrencySymbol + this.TotalInsuredValue).PadLeft(19) + "\n";
            //i++;
            //}
            return PolicyData;
        }

        #endregion
    }
}
