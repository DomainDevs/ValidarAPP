using Sistran.Company.Application.PrintingServices.Clases;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Resources;
using Sistran.Company.PrintingService.JetForm.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    class Auto : Reportes
    {
        #region Atributes

        /// <summary>
        /// Dataset para almacenar los datos del riesgo
        /// </summary>
        private DataSet _risk;
        /// <summary>
        /// Tipo de riesgo
        /// </summary>
        private IRisk _coveredRisk;

        #endregion

        #region Properties

        /// <summary>
        /// Propiedad para acceder al campo _risk
        /// </summary>
        public DataSet Risk
        {
            get { return _risk; }
            set { _risk = value; }
        }
        /// <summary>
        /// Propiedad para acceder al campo _coveredRisk
        /// </summary>
        public IRisk CoveredRisk
        {
            get { return _coveredRisk; }
            set { _coveredRisk = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metodo principal: Imprime poliza
        /// </summary>
        protected override void writeData()
        {
            DataTable dsRiesgos = this.dsPoliza.Tables["Table7"];
            this.createFile();

            foreach (DataRow risk in dsRiesgos.Rows)
            {
                this.ActRiskNum = Convert.ToInt32(risk["RISK_NUM"]);
                Risk = this.getActualRisk(this.dsPoliza);
                if (this.ActRiskNum != 0)
                {
                    this.CoveredRiskType = Convert.ToInt32(Risk.Tables["Table"].Rows[0]["COVERED_RISK_TYPE_CD"].ToString());
                    this.PrefixCode = Risk.Tables["Table7"].Rows[0]["PREFIX_CD"].ToString();
                }
                CoveredRisk = ReportServiceHelper.selectCoveredRiskTypeToPrint(this.CoveredRiskType, Convert.ToInt32(this.PrefixCode));
                base.writeData();
                this.setPolicyData(Risk);

                int page = 0;

                if (this.ActRiskNum > 0)
                {
                    //page = 1;
                    this.printCover(page);
                    this.printCoverCopy(page);
                }
                else
                {
                    this.printCover(page);
                    this.printCoverCopy(page);
                }
            }

            if (!this.PolicyData.PolicyId.Equals(0))
            {
                if (this.PaymentCheckDataSet != null)
                {
                    this.printReportPaymentCheck();
                }
            }
        }

        /// <summary>
        /// Inicializa informacion de la poliza
        /// </summary>
        /// <param name="risk"></param>
        protected override void setPolicyData(DataSet risk)
        {
            base.setPolicyData(risk);

            DataRow[] sumRows = risk.Tables[1].Select("COVERAGE_NUM IN (2,4,10)");

            decimal coveragePremiumValue = 0;
            foreach (DataRow row in sumRows)
            {
                coveragePremiumValue += ReportServiceHelper.validateValue(row["COVERAGE_PREMIUM"].ToString(), CultureInfo.CurrentCulture);
            }
            foreach (DataRow row in risk.Tables["Table2"].Rows)
            {
                decimal valor = ReportServiceHelper.validateValue(row["PREMIUM_DETAIL"].ToString(), CultureInfo.CurrentCulture);
                coveragePremiumValue += valor;
            }

            //this.InsuredTotalValue = ReportServiceHelper.formatMoney(coveragePremiumValue.ToString(), CultureInfo.CurrentCulture);

            if (this.ActRiskNum != 0)
            {
                this.TotalValue += coveragePremiumValue;
                this.TotalInsuredValue = ReportServiceHelper.formatMoney(TotalValue.ToString(), CultureInfo.CurrentCulture);
            }

            this.PromissoryNoteNum = risk.Tables["Table"].Rows[0]["PROMISSORY_NOTE_NUM_CD"].ToString();
        }
        /// <summary>
        /// Imprime la pagina original del reporte
        /// </summary>
        /// <param name="page"></param>
        private void printCover(int page)
        {
            if (this.isQuotation == false)
            {
                this.printTitle(page, RptFields.LBL_VEHICLE_COVER_NAME);
            }
            else
            {
                this.printTitle(page, RptFields.LBL_QUOTATION_COVER_NAME);
            }

            switch (this.PolicyData.ReportType)
            {
                case (int)ReportType.COMPLETE_POLICY:
                    this.printPolicyBody(page);
                    break;
                case (int)ReportType.ONLY_POLICY:
                    this.printPolicyBody(page);
                    break;
                case (int)ReportType.TEMPORARY:
                    this.printPolicyBody(page);
                    break;
                case (int)ReportType.QUOTATION:
                    this.printPolicyBody(page);
                    break;
            }
        }
        /// <summary>
        /// Imprime la pagina de copia del reporte
        /// </summary>
        /// <param name="page"></param>
        private void printCoverCopy(int page)
        {
            this.LimitPageLines = 28;
            this.PageLinesCount = 0;
            this.AppendixPageCount = 0;
            this.Iscopy = true;

            switch (this.PolicyData.ReportType)
            {
                case (int)ReportType.COMPLETE_POLICY:
                    if (isQuotation == false)
                    {
                        this.printTitle(2, RptFields.LBL_COPY_VEHICLE_COVER_NAME);
                    }
                    else
                    {
                        this.printTitle(2, RptFields.LBL_COPY_QUOTATION_COVER_NAME);
                    }
                    this.printPolicyBody(page);
                    break;
                case (int)ReportType.ONLY_POLICY:
                    if (isQuotation == false)
                    {
                        this.printTitle(2, RptFields.LBL_COPY_VEHICLE_COVER_NAME);
                    }
                    else
                    {
                        this.printTitle(2, RptFields.LBL_COPY_QUOTATION_COVER_NAME);
                    }
                    this.printPolicyBody(page);
                    break;
                case (int)ReportType.TEMPORARY:
                    if (isQuotation == false)
                    {
                        this.printTitle(2, RptFields.LBL_COPY_VEHICLE_COVER_NAME);
                    }
                    else
                    {
                        this.printTitle(2, RptFields.LBL_COPY_QUOTATION_COVER_NAME);
                    }
                    this.printPolicyBody(page);
                    break;
            }
        }
        /// <summary>
        /// Imprime la poliza
        /// </summary>
        /// <param name="page"></param>
        private void printPolicyBody(int page)
        {
            this.printHeader();
            this.printPolicyHolderData();
            this.printIssuanceData();
            this.printCommissionsAndCoinsuranceData();
            this.printInsuredData();

            int _pagesLines;
            int _limitPages;
            int _anexNum;
            CoveredRisk.printRisk(this.Risk, this.File, this.PolicyData, this.ActRiskNum, this.CoveredRiskType, out _pagesLines, out _limitPages, out _anexNum);
            this.PageLinesCount = _pagesLines;
            this.LimitPageLines = _limitPages;
            this.AppendixPageCount = _anexNum;

            if (ActRiskNum == 0 || !IsCollective)
                this.PrintGeneralLevelText();

            this.PrintRiskLevelText();

            if (ActRiskNum == 0 || !IsCollective)
                this.PrintPromissoryNoteNum();
        }
        /// <summary>
        /// Impresion de encabezado de la pagina
        /// </summary>
        protected override void printHeader()
        {
            if (this.ActRiskNum != 0 && isQuotation == false)
                this.File.Add(new DatRecord(this.Field + RptFields.FLD_INDIVIDUAL_CERTIFICATE, RptFields.TTL_INDIVIDUAL_CERTIFICATE));

            base.printHeader();
        }


        #endregion
    }
}
