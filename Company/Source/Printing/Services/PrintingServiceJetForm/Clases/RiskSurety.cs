using Sistran.Company.Application.PrintingServices.Resources;
using Sistran.Company.PrintingService.JetForm.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    sealed class RiskSurety : RiskBase
    {
        #region Atributes

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar coberturas del riesgo.
        /// </summary>
        protected override void initializeCoveragesDataTable()
        {
            base.initializeCoveragesDataTable();
            this.RiskCoverages.Columns.Add("AcumVA", typeof(String));
            this.RiskCoverages.Columns.Add("CurrentFrom", typeof(String));
            this.RiskCoverages.Columns.Add("CurrentTo", typeof(String));
            this.RiskCoverages.Columns.Add("Premium", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Inicializo tabla de coberturas y beneficiarios para el riesgo.
        /// </summary>
        protected override void setRiskData()
        {
            base.setRiskData();

            string DayFrom = this.RiskData.Tables["Table"].Rows[0]["CURRENT_FROM_DAY"].ToString();
            string MonthFrom = this.RiskData.Tables["Table"].Rows[0]["CURRENT_FROM_MONTH"].ToString();
            string YearFrom = this.RiskData.Tables["Table"].Rows[0]["CURRENT_FROM_YEAR"].ToString();

            this.EndorsementDate = ((DayFrom.Length == 1) ? "0" + DayFrom : DayFrom) + @"/" +
                         ((MonthFrom.Length == 1) ? "0" + MonthFrom : MonthFrom) + @"/" +
                           YearFrom;

            this.BeneficiaryName = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_NAME"].ToString());
            this.BeneficiaryDocumentType = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC_TYPE"].ToString());

            if (BeneficiaryDocumentType.Equals(RptFields.LBL_NIT))
                this.BeneficiaryDocumentNumber = ReportServiceHelper.formatNIT(
                                                                    this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString());
            else
                this.BeneficiaryDocumentNumber = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString();

            this.initializeCoveragesDataTable();
            this.initializeBeneficiariesDataTable();
            this.initializeClausesDataTable();
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Obtengo y guardo las coberturas del riesgo en la tabla RiskData
        /// </summary>
        protected override void getRiskCoverages()
        {
            base.getRiskCoverages();

            int rowNum = 0;

            foreach (DataRow row in this.RiskData.Tables["Table1"].Rows)
            {
                rowNum++;
                DataRow newRow = this.RiskCoverages.NewRow();
                newRow["CoverageNum"] = rowNum;
                newRow["PrintDescription"] = row["COVERAGE"];
                newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["COVERAGE_PREMIUM"].ToString(), new CultureInfo("en-US"));

                string limitAmt = row["LIMIT_OCCURRENCE_AMT"].ToString().Replace(",", string.Empty).Split('.')[0];

                if (!Convert.ToInt64(limitAmt).Equals(0))
                    newRow["AcumVA"] = "SI";
                else
                    newRow["AcumVA"] = "NO";

                newRow["CurrentFrom"] = row["CURRENT_FROM"];
                newRow["CurrentTo"] = row["CURRENT_TO"];
                newRow["Premium"] = ReportServiceHelper.formatMoney(row["PREMIUM_AMT"].ToString().Replace(",", "."), new CultureInfo("en-US"));

                this.RiskCoverages.Rows.Add(newRow);
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Guarda en el .dat la informacion de las coberturas del riesgo.
        /// </summary>
        protected override void printRiskCoverages()
        {
            base.printRiskCoverages();

            this.printCoverageHeader();

            if (this.RiskCoverages.Rows.Count > 0)
            {
                foreach (DataRow row in this.RiskCoverages.Rows)
                {
                    string line = string.Format(RptFields.FLD_CONTRACT_COVERAGES,
                                                ReportServiceHelper.leftAlign(row["CoverageNum"].ToString(), 3),
                                                ReportServiceHelper.leftAlign(row["PrintDescription"].ToString(), 25),
                                                ReportServiceHelper.rightAlign(row["InsuredValue"].ToString(), 18) + " ",
                                                ReportServiceHelper.centerAlign(row["AcumVA"].ToString(), 10),
                                                ReportServiceHelper.rightAlign(row["CurrentFrom"].ToString(), 10),
                                                ReportServiceHelper.rightAlign(row["CurrentTo"].ToString(), 10) + " ",
                                                ReportServiceHelper.rightAlign(row["Premium"].ToString(), 13));
                    this.validatePageLimit(1);
                    this.File.Add(new DatRecord(line, null));
                }
            }
        }
        /// <summary>
        /// Informacion General - 
        /// Imprime titulo de las columnas de coberturas.
        /// </summary>
        private void printCoverageHeader()
        {
            this.File.Add(new DatRecord(ReportServiceHelper.leftAlign("No.", 4) +
                     ReportServiceHelper.leftAlign("Amparo", 26) +
                     ReportServiceHelper.rightAlign(" Valor Asegurado", 18) + " " +
                     ReportServiceHelper.centerAlign("AcumVA", 12) +
                     ReportServiceHelper.rightAlign("Vig. Desde", 10) + " " +
                     ReportServiceHelper.rightAlign("Vig. Hasta", 10) + " " +
                     ReportServiceHelper.rightAlign("Prima", 13), null));
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Obtiene y genera informacion del riesgo, metodo principal.
        /// </summary>
        /// <param name="_risk">DataSet con informacion del riesgo</param>
        protected override void printRiskBody(DataSet _risk)
        {
            this.iniRiskValues();
            this.setRiskData();
            this.getRiskCoverages();
            this.getRiskBeneficiaries();
            this.getRiskClauses();

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(this.Field + RptFields.SCC_COVERAGES_VDATA, null));

            this.printBeneficiariesSection();
            this.printRiskData();
            this.printRiskCoverages();
            this.printRiskFormDescription();
            this.printRiskClauses();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Genera informacion de beneficiarios en el archivo .dat
        /// </summary>
        private void printBeneficiariesSection()
        {
            if (this.ActRiskNum != 0)
            {
                this.validatePageLimit(2);
                // this.File.Add(new DatRecord(string.Format(RptFields.SCT_BENEFICIARIES, this.BeneficiaryName, this.BeneficiaryDocumentType + @":", this.BeneficiaryDocumentNumber), null));

                //TODO:     <<<< Codigo:PV3G06-AE075;Autor:Jonnathan Garzon; Fecha: 19/11/2012
                //Asunto:   Se ajusta la funcionalidad de impresion de beneficiarios para el ramo de automoviles, ya que en la forma anterior
                //          solo se imprimia un beneficiario (el primero por orden alfabético).
                this.File.Add(new DatRecord(string.Format(RptFields.SCT_BENEFICIARIES, null, null, null), null));
                foreach (DataRow row in this.RiskBeneficiaries.Rows)
                {
                    this.File.Add(new DatRecord(ReportServiceHelper.leftAlign(row["Name"].ToString() == null ? "" : row["Name"].ToString(), 45) +
                        ReportServiceHelper.centerAlign(row["DocType"].ToString() == null ? "" : row["DocType"].ToString() + @":", 20) +
                        ReportServiceHelper.centerAlign(row["Document"].ToString() == null ? "" :
                        (row["DocType"].ToString() == "CC"
                        ? (ReportServiceHelper.formatCedula(row["Document"].ToString(), CultureInfo.CurrentCulture)) :
                        (ReportServiceHelper.formatNIT(row["Document"].ToString()))), 20), null));

                }

            }
            else
            {
                this.validatePageLimit(2);
                this.File.Add(new DatRecord(string.Format(RptFields.SCT_BENEFICIARIES,
                                                          string.Empty,
                                                          string.Empty,
                                                          RptFields.LBL_COLLECTIVE_ITEMS),
                                            null));
            }
        }

        #endregion
    }
}
