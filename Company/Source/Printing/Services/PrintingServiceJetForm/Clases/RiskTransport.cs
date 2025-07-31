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
    sealed class RiskTransport : RiskBase
    {
        #region Atributes

        /// <summary>
        /// Informacion Trayecto -
        /// Almacena informacion de Origen
        /// </summary>
        private string _origin;
        /// <summary>
        /// Informacion Trayecto -
        /// Almacena informacion de destino
        /// </summary>
        private string _destination;
        /// <summary>
        /// Informacion Riesgo -
        /// Categorias
        /// </summary>
        private DataTable _riskCategories;
        /// <summary>
        /// Informacion Riesgo - 
        /// Sistemas de seguridad
        /// </summary>
        private DataTable _riskSecuritySystems;

        #endregion

        #region Properties

        /// <summary>
        /// Propiedad para acceder al campo _origin
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        /// <summary>
        /// Propiedad para acceder al campo _destination
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }
        /// <summary>
        /// Propiedad para acceder al campo _riskCategories
        /// </summary>
        private DataTable RiskCategories
        {
            get { return _riskCategories; }
            set { _riskCategories = value; }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Propiedad para acceder al campo _riskSecuritySystems.
        /// </summary>
        private DataTable RiskSecuritySystems
        {
            get { return _riskSecuritySystems; }
            set { _riskSecuritySystems = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Informacion Riesgo -
        /// Metodo Principal: Imprime todo lo referente al riesgo.
        /// </summary>
        protected override void printRiskBody(DataSet _risk)
        {
            this.iniRiskValues();
            this.setRiskData();
            this.getRiskCoverages();
            this.getRiskBeneficiaries();
            this.getRiskSecuritySystems();
            this.getRiskClauses();

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(this.Field + RptFields.SCC_COVERAGES_VDATA, null));

            this.printRiskNum();

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(RptFields.SCT_CONTRACT_COVERAGES, null));

            this.printRiskData();
            this.printRiskCoverages();
            this.printBeneficiariesSection();
            this.printRiskFormDescription();
            this.printRiskSecuritySystems();
            this.printRiskClauses();
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Obtiene informacion basica del trayecto.
        /// </summary>
        protected override void setRiskData()
        {
            base.setRiskData();

            this.Origin = this.RiskData.Tables["Table"].Rows[0]["SOURCE"].ToString();
            this.Destination = this.RiskData.Tables["Table"].Rows[0]["DESTINY"].ToString();

            string DayFrom = this.RiskData.Tables["Table"].Rows[0]["CURRENT_FROM_DAY"].ToString();
            string MonthFrom = this.RiskData.Tables["Table"].Rows[0]["CURRENT_FROM_MONTH"].ToString();
            string YearFrom = this.RiskData.Tables["Table"].Rows[0]["CURRENT_FROM_YEAR"].ToString();

            this.BeneficiaryName = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_NAME"].ToString());
            this.BeneficiaryDocumentType = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC_TYPE"].ToString());

            if (BeneficiaryDocumentType.Equals(RptFields.LBL_NIT))
                this.BeneficiaryDocumentNumber = ReportServiceHelper.formatNIT(
                                                                    this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString());
            else
                this.BeneficiaryDocumentNumber = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString();

            this.EndorsementDate = ((DayFrom.Length == 1) ? "0" + DayFrom : DayFrom) + @"/" +
                         ((MonthFrom.Length == 1) ? "0" + MonthFrom : MonthFrom) + @"/" +
                           YearFrom;

            this.initializeCoveragesDataTable();
            this.initializeCategoriesDataTable();
            this.initializeBeneficiariesDataTable();
            this.initializeSecuritySystemsDataTable();
            this.initializeClausesDataTable();
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Inicializa tabla de coberturas de la póliza.
        /// </summary>
        protected override void initializeCoveragesDataTable()
        {
            base.initializeCoveragesDataTable();
            this.RiskCoverages.Columns.Add("AcumVA", typeof(String));
            this.RiskCoverages.Columns.Add("CurrentFrom", typeof(String));
            this.RiskCoverages.Columns.Add("CurrentTo", typeof(String));
            this.RiskCoverages.Columns.Add("Premium", typeof(String));
            this.RiskCoverages.Columns.Add("Deductible", typeof(String));
            this.RiskCoverages.Columns.Add("InsuredObjectId", typeof(String));
            this.RiskCoverages.Columns.Add("InsuredObjectDesc", typeof(String));
            this.RiskCoverages.Columns.Add("LineBusinessId", typeof(String));
            this.RiskCoverages.Columns.Add("LineBusinessDesc", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Inicializa tabla de categorias.
        /// </summary>
        private void initializeCategoriesDataTable()
        {
            this.RiskCategories = this.RiskCoverages.Copy();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar sistemas de seguridad del riesgo.
        /// </summary>
        private void initializeSecuritySystemsDataTable()
        {
            this.RiskSecuritySystems = new DataTable();
            this.RiskSecuritySystems.Columns.Add("Accessory", typeof(String));
        }

        /// <summary>
        /// Informacion Riesgo -
        /// Obtiene informacion de categorias y coberturas.
        /// </summary>
        protected override void getRiskCoverages()
        {
            base.getRiskCoverages();

            int rowNum = 0;
            //Transport coverages
            foreach (DataRow row in this.RiskData.Tables["Table1"].Rows)
            {
                rowNum++;
                string limitAmt = row["LIMIT_OCCURRENCE_AMT"].ToString().Replace(",", string.Empty).Split('.')[0];

                if (row["COVERAGE_NUM"].ToString().Equals("0"))
                {
                    string[] coverageCategory = row["COVERAGE"].ToString().Split('|');
                    DataRow newRow = this.RiskCategories.NewRow();
                    newRow["RiskNum"] = row["RISK_NUM"].ToString();
                    newRow["CoverageNum"] = coverageCategory[0];
                    newRow["PrintDescription"] = coverageCategory[1];
                    newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["COVERAGE_PREMIUM"].ToString(), new CultureInfo("en-US"));
                    newRow["Premium"] = ReportServiceHelper.formatMoney(row["PREMIUM_AMT"].ToString().Replace(",", "."), new CultureInfo("en-US"));
                    if (!Convert.ToInt64(limitAmt).Equals(0))
                        newRow["AcumVA"] = "SI";
                    else
                        newRow["AcumVA"] = "NO";
                    newRow["Deductible"] = row["COVERAGE_DEDUCT"];
                    newRow["InsuredObjectId"] = row["INO_ID"];
                    newRow["InsuredObjectDesc"] = row["INO_DESC"];
                    newRow["LineBusinessId"] = row["LNB_CD"];
                    newRow["LineBusinessDesc"] = row["LNB_DESC"];
                    //   newRow["ConditionText"] = row["CONDITION_TEXT"];

                    this.RiskCategories.Rows.Add(newRow);
                }
                else
                {
                    DataRow newRow = this.RiskCoverages.NewRow();
                    newRow["CoverageNum"] = rowNum;
                    newRow["PrintDescription"] = row["COVERAGE"];
                    newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["COVERAGE_PREMIUM"].ToString(), new CultureInfo("en-US"));

                    if (!Convert.ToInt64(limitAmt).Equals(0))
                        newRow["AcumVA"] = "SI";
                    else
                        newRow["AcumVA"] = "NO";

                    newRow["CurrentFrom"] = this.EndorsementDate;
                    newRow["CurrentTo"] = row["CURRENT_TO"];
                    newRow["Premium"] = ReportServiceHelper.formatMoney(row["PREMIUM_AMT"].ToString().Replace(",", "."), new CultureInfo("en-US"));
                    newRow["Deductible"] = row["COVERAGE_DEDUCT"];
                    this.RiskCoverages.Rows.Add(newRow);
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime informacion especifica del riesgo.
        /// </summary>
        protected override void printRiskData()
        {
            if (this.ActRiskNum != 0)
            {
                base.printRiskData();
                this.printOriginAndDestination();
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime informacion del numero de riesgo.
        /// </summary>
        private void printRiskNum()
        {
            if (this.ActRiskNum != 0)
            {
                this.validatePageLimit(2);
                this.File.Add(new DatRecord("\n", string.Format(RptFields.LBL_RISK_NUM, this.ActRiskNum.ToString())));
            }
        }

        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime el origen y el destino del trayecto del riesgo.
        /// </summary>
        private void printOriginAndDestination()
        {
            this.validatePageLimit(1);
            this.File.Add(new DatRecord(string.Format(RptFields.LBL_ORIGIN_DESTINATION,
                                        this.Origin.ToUpper(),
                                        ReportServiceHelper.centerAlign(" ", ((this.Origin.Length + this.Destination.Length) < 70) ? (70 - this.Origin.Length - this.Destination.Length) : 3),
                                        this.Destination.ToUpper()), null));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime las coberturas del riesgo.
        /// </summary>
        protected override void printRiskCoverages()
        {
            base.printRiskCoverages();

            this.printCoveragesHeader();
            int addLines = 1;

            foreach (DataRow row in this.RiskCoverages.Rows)
            {
                string deductibleText = (row["Deductible"].ToString().Trim().Length > 0) ? "\n" +
                                        ReportServiceHelper.leftAlign(string.Empty, 4) + "Deducible: " +
                                        ReportServiceHelper.leftAlign(row["Deductible"].ToString(), 90) :
                                        string.Empty;

                string line = string.Format(RptFields.FLD_CONTRACT_COVERAGES,
                                            ReportServiceHelper.leftAlign(row["CoverageNum"].ToString(), 3),
                                            ReportServiceHelper.leftAlign(row["PrintDescription"].ToString(), 49),
                                            ReportServiceHelper.rightAlign(row["InsuredValue"].ToString(), 18) + " ",
                                            ReportServiceHelper.centerAlign(row["AcumVA"].ToString(), 10),
                                            ReportServiceHelper.rightAlign(row["Premium"].ToString(), 13),
                                            deductibleText, string.Empty);

                addLines = (deductibleText.Equals(string.Empty)) ? 1 : 2;
                this.validatePageLimit(addLines);
                this.File.Add(new DatRecord(line, null));
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime Seccion de Beneficiarios.
        /// </summary>
        private void printBeneficiariesSection()
        {
            if (this.ActRiskNum != 0)
            {
                this.printBeneficiariesHeader();
                this.printRiskBeneficiaries();
            }
        }
        /// <summary>
        /// Informacion Coberturas -
        /// Imprime titulos de area de coberturas.
        /// </summary>
        private void printCoveragesHeader()
        {
            this.validatePageLimit(1);
            this.File.Add(new DatRecord(ReportServiceHelper.leftAlign("No.", 4) +
                     ReportServiceHelper.leftAlign("Amparo", 30) +
                     ReportServiceHelper.rightAlign("Valor Asegurado", 27) + " " +
                     ReportServiceHelper.centerAlign("AcumVA", 22) +
                     ReportServiceHelper.rightAlign("Prima", 15), null));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Llena tabla de RiskSecuritySystem
        /// </summary>
        private void getRiskSecuritySystems()
        {
            foreach (DataRow row in this.RiskData.Tables["Table2"].Rows)
            {
                DataRow newRow = this.RiskSecuritySystems.NewRow();
                newRow["Accessory"] = row["DESCRIPTION"];
                this.RiskSecuritySystems.Rows.Add(newRow);
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime sistemas de seguridad del riesgo.
        /// </summary>
        private void printRiskSecuritySystems()
        {
            if (this.RiskSecuritySystems.Rows.Count > 0)
            {
                this.validatePageLimit(2);
                this.File.Add(new DatRecord(RptFields.LBL_CONTRACT_SECURITY_SYSTEMS, null));

                foreach (DataRow row in this.RiskSecuritySystems.Rows)
                {
                    this.validatePageLimit(1);
                    this.File.Add(new DatRecord(row["Accessory"].ToString(), null));
                }
            }
        }


        #endregion
    }
}
