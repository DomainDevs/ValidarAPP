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
    class Ubicacion : Reportes
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
                    page = 1;
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
                this.printReportPaymentCheck();
            }
        }
        /// <summary>
        /// Inicializa informacion de la poliza
        /// </summary>
        /// <param name="risk"></param>
        protected override void setPolicyData(DataSet risk)
        {
            base.setPolicyData(risk);

            this.InsuredTotalValue = ReportServiceHelper.formatMoney(risk.Tables["Table"].Rows[0]["LIMIT_AMT"].ToString(), CultureInfo.CurrentCulture);

            if (this.IsCollective && this.ActRiskNum.Equals(0))
            {
                this.TotalInsuredValue = ReportServiceHelper.formatMoney(risk.Tables["Table"].Rows[0]["LIMIT_AMT"].ToString(), CultureInfo.CurrentCulture);
            }
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
            this.LimitPageLines = 29;
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
            //TODO:     <<<< Codigo:PV3G06-AE091;Autor:Jonnathan Garzon; Fecha: 21/12/2012
            //Asunto:   Llamado a metodo de impresion de intermediarios faltantes
            this.printAdditionalIntermediaries();
            this.PageLinesCount = _pagesLines;
            this.LimitPageLines = _limitPages;
            this.AppendixPageCount = _anexNum;
            //Autor:	Jonnathan Garzon; Fecha: 21/12/2012;>>
            if (ActRiskNum == 0 || !IsCollective)
            {
                this.PrintGeneralLevelText();
            }
            this.PrintRiskLevelText();
            if (ActRiskNum == 0 || !IsCollective)
                this.PrintPromissoryNoteNum();
        }

        /// <summary>
        /// Impresion de encabezado de la pagina
        /// </summary>
        protected override void printHeader()
        {
            base.printHeader();
        }

        //TODO:     <<<< Codigo:PV3G06-AE091;Autor:Jonnathan Garzon; Fecha: 21/12/2012
        //Asunto:   Elaboración metodos de impresion de encabezado y registros de intermediarios faltantes, cuando la cantidad de
        //          registros de intermediarios registrados supera el espacio de la plantilla (7 renglones)
        /// <summary>
        /// Metodo que imprime el encabezado y los registros de los intermediarios faltantes
        /// </summary>
        private void printAdditionalIntermediaries()
        {
            if (Intermediaries.Rows.Count > 6)
            {
                printHeaderIntermediaries();
                printDataIntermediaries();
            }
        }

        /// <summary>
        /// Metodo que imprime el encabezado para los intermediarios faltantes
        /// </summary>
        private void printHeaderIntermediaries()
        {
            this.validatePageLimit(2);
            this.File.Add(new DatRecord("\nContinuación Intermediarios", null));
            this.validatePageLimit(1);
            this.File.Add(new DatRecord(ReportServiceHelper.leftAlign("CLAVE", 8) +
                            ReportServiceHelper.centerAlign("CLASE", 5) +
                            ReportServiceHelper.centerAlign("NOMBRE", 23) + " " +
                            ReportServiceHelper.centerAlign("%", 7) + " " +
                            //cesar giraldo 31/10/2013 asunto cambio de alineación de impresión 
                            ReportServiceHelper.rightAlign("COMISIÓN", 14), null));
            //cesar giraldo 31/10/2013 asunto cambio de alineación de impresión 
        }

        /// <summary>
        /// Metodo que imprime los registros de los intermediarios faltantes
        /// </summary>
        private void printDataIntermediaries()
        {
            for (int i = 6; i < Intermediaries.Rows.Count; i++)
            {
                this.File.Add(new DatRecord(string.Empty, string.Format(RptFields.FLD_INTERMEDIARIES,
                                                        ReportServiceHelper.completeColumnCoverages(Intermediaries.Rows[i]["Id"].ToString(), 6),
                                                        ReportServiceHelper.completeColumnCoverages(Intermediaries.Rows[i]["Type"].ToString(), 5),
                                                        ReportServiceHelper.completeColumnCoverages(Intermediaries.Rows[i]["Name"].ToString(), 20),
                                                        //cesar giraldo 31/10/2013 asunto cambio de alineación de impresión 
                                                        this.Iscopy ? ReportServiceHelper.rightAlign(Intermediaries.Rows[i]["Percentage"].ToString(), 7) : string.Empty,
                                                        this.Iscopy ? ReportServiceHelper.rightAlign(Intermediaries.Rows[i]["Commission"].ToString(), 15) : string.Empty)));
                //cesar giraldo 31/10/2013 asunto cambio de alineación de impresión 
            }
        }
        //Autor:	Jonnathan Garzon; Fecha: 21/12/2012;>>

        #endregion
    }
}
