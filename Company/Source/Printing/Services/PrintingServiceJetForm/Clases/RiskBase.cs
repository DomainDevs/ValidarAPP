using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.PrintingService.JetForm.Clases;
using Sistran.Company.Application.PrintingServices.Enums;
using System.Globalization;
using Sistran.Company.Application.PrintingServices.Resources;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    internal class RiskBase : Reportes, IRisk
    {
        #region Atributes

        /// <summary>
        /// Informacion Riesgo - 
        /// Campo para guardar las coberturas.
        /// </summary>
        private DataTable _riskCoverages;
        /// <summary>
        /// Informacion Riesgo - 
        /// Campo para guardar las beneficiarios.
        /// </summary>
        private DataTable _riskBeneficiaries;
        /// <summary>
        /// Informacion Riesgo - 
        /// Campo para guardar las clausulas.
        /// </summary>
        private DataTable _riskClauses;
        /// <summary>
        /// Informacion Riesgo - 
        /// Campo para guardar los datos especificos del riesgo.
        /// </summary>
        private DataSet _riskData;

        #endregion

        #region Properties

        /// <summary>
        /// Informacion Riesgo - 
        /// Propiedad para acceder al campo _riskCoverages.
        /// </summary>
        /// <value>DataTable de coberturas del riesgo.</value>
        public DataTable RiskCoverages
        {
            get { return _riskCoverages; }
            set { _riskCoverages = value; }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Propiedad para acceder al campo _riskBeneficiaries.
        /// </summary>
        /// <value>Datatable de Beneficiarios.</value>
        public DataTable RiskBeneficiaries
        {
            get { return _riskBeneficiaries; }
            set { _riskBeneficiaries = value; }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Propiedad para acceder al campo _riskClauses.
        /// </summary>
        /// <value>DataTable de Clausulas</value>
        public DataTable RiskClauses
        {
            get { return _riskClauses; }
            set { _riskClauses = value; }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Propiedad para acceder al campo _riskData.
        /// </summary>
        /// <value>DataTable con informacion del riesgo.</value>
        public DataSet RiskData
        {
            get { return _riskData; }
            set { _riskData = value; }
        }

        #endregion

        #region Miembros de IRisk

        /// <summary>
        /// Informacion Riesgo -
        /// Impresion de Riesgo.
        /// </summary>
        /// <param name="_risk">DataSet del riesgo</param>
        public void printRisk(DataSet _risk)
        {
            this.RiskData = _risk;
            printRiskBody(_risk);
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Impresion de Riesgo.
        /// </summary>
        /// <param name="_risk">DataSet del riesgo</param>
        /// <param name="_file">Representa ArrayList que se generara como .dat</param>
        /// <param name="_policyData">Informacion de la poliza</param>
        /// <param name="_actRiskNum">Numero de riesgo Activo</param>
        public void printRisk(DataSet _risk, ArrayList _file, Policy _policyData, int _actRiskNum, int _coveredRiskType, out int _pageLinesCount, out int _limitPageLines, out int _pageAnexNum)
        {
            this.RiskData = _risk;
            this.File = _file;
            this.PolicyData = _policyData;
            this.ActRiskNum = _actRiskNum;
            this.CoveredRiskType = _coveredRiskType;
            this.FormDescription = this.RiskData.Tables["Table"].Rows[0]["FORM_DESC"].ToString();
            this.PrefixDescription = this.RiskData.Tables["Table"].Rows[0]["PREFIX"].ToString();
            this.PolicyNumber = this.RiskData.Tables["Table7"].Rows[0]["POLICY_NUMBER"].ToString();
            this.EndorsementDescription = this.RiskData.Tables["Table"].Rows[0]["ENDORSEMENT_TYPE_DESC"].ToString();
            this.EndorsementNumber = this.RiskData.Tables["Table7"].Rows[0]["DOCUMENT_NUM"].ToString();
            printRiskBody(_risk);
            _pageLinesCount = this.PageLinesCount;
            _limitPageLines = this.LimitPageLines;
            _pageAnexNum = this.AppendixPageCount;
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Constructor
        /// </summary>
        public RiskBase() { }

        #endregion

        #region Methods

        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar coberturas.
        /// </summary>
        protected virtual void initializeCoveragesDataTable()
        {
            this.RiskCoverages = new DataTable();
            this.RiskCoverages.Columns.Add("RiskNum", typeof(String));
            this.RiskCoverages.Columns.Add("CoverageNum", typeof(String));
            this.RiskCoverages.Columns.Add("PrintDescription", typeof(String));
            this.RiskCoverages.Columns.Add("InsuredValue", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar beneficiarios.
        /// </summary>
        protected virtual void initializeBeneficiariesDataTable()
        {
            this.RiskBeneficiaries = new DataTable();
            this.RiskBeneficiaries.Columns.Add("RiskNum", typeof(int));
            this.RiskBeneficiaries.Columns.Add("Name", typeof(String));
            //TODO:     <<<< Codigo:PV3G06-AE075;Autor:Jonnathan Garzon; Fecha: 19/11/2012
            //Asunto:   Adicion de columna para tipo de documento del beneficiario
            this.RiskBeneficiaries.Columns.Add("DocType", typeof(String));
            //Autor:	Jonnathan Garzon; Fecha: 02/10/2012;>>
            this.RiskBeneficiaries.Columns.Add("Document", typeof(String));
            this.RiskBeneficiaries.Columns.Add("Percentage", typeof(String));
            this.RiskBeneficiaries.Columns.Add("Type", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar clausulas.
        /// </summary>
        protected virtual void initializeClausesDataTable()
        {
            this.RiskClauses = new DataTable();
            this.RiskClauses.Columns.Add("RiskNum", typeof(int));
            this.RiskClauses.Columns.Add("ClauseTitle", typeof(String));
            this.RiskClauses.Columns.Add("ClauseText", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Obtiene el tipo de riesgo cubierto.
        /// </summary>
        private IRisk getCoveredRiskType(int coverRiskType)
        {
            try
            {
                switch (coverRiskType)
                {
                    case ((int)RiskType.AUTO):
                        return null;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Obtiene las coberturas.
        /// </summary>
        protected virtual void getRiskCoverages() { }
        /// <summary>
        /// Informacion Riesgo - 
        /// Obtiene los beneficiarios.
        /// </summary>
        protected virtual void getRiskBeneficiaries()
        {
            foreach (DataRow row in this.RiskData.Tables["Table8"].Rows)
            {
                if (row["RISK_NUM"].ToString() == this.ActRiskNum.ToString())
                {
                    DataRow newRow = this.RiskBeneficiaries.NewRow();
                    newRow["RiskNum"] = row["RISK_NUM"];
                    //TODO:     <<<< Codigo:PV3G06-AE075;Autor:Jonnathan Garzon; Fecha: 25/10/2012
                    //Asunto:   Asignación de valores de nombre y documento de Beneficiarios de la tabla retornada en la consulta en lugar de los
                    //          valores del this, ya que por cada beneficiario retornado en la tabla se repetia el mismo beneficiario del this.
                    newRow["Name"] = row["NAME"];
                    newRow["DocType"] = row["DOC_TYPE"];

                    //TODO:     <<<< Codigo:PV3G06-AE058;Autor:César Giraldo; Fecha: 05/08/2013
                    //Asunto:   Se ajusta la funcionalidad de impresion de beneficiarios para el ramo de automoviles, para formato de impresión
                    //          del nit o la cédula.

                    if (newRow["DocType"].ToString() != "NIT")
                    {
                        newRow["Document"] = ReportServiceHelper.formatCedula(row["DOC_ID"].ToString(), CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        newRow["Document"] = ReportServiceHelper.formatNIT(row["DOC_ID"].ToString());
                    }
                    //Autor:	César Giraldo; Fecha: 05/08/2013;>>

                    //Autor:	Jonnathan Garzon; Fecha: 25/10/2012;>>
                    //newRow["Name"] = this.BeneficiaryName;
                    //newRow["Document"] = this.BeneficiaryDocumentNumber;
                    newRow["Percentage"] = row["BENEFIT_PCT"];
                    newRow["Type"] = row["SMALL_DESCRIPTION"];
                    this.RiskBeneficiaries.Rows.Add(newRow);
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Obtiene las clausulas.
        /// </summary>
        protected virtual void getRiskClauses()
        {
            foreach (DataRow row in this.RiskData.Tables["Table5"].Rows)
            {
                if (row["RISK_NUM"].ToString() == this.ActRiskNum.ToString())
                {
                    DataRow newRow = this.RiskClauses.NewRow();
                    newRow["RiskNum"] = row["RISK_NUM"];
                    newRow["ClauseTitle"] = row["CLAUSE_NAME"];
                    newRow["ClauseText"] = row["CLAUSE_TEXT"];
                    this.RiskClauses.Rows.Add(newRow);
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Metodo base para obtener informacion general del riesgo.
        /// </summary>
        protected virtual void setRiskData()
        {

        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Metodo base para imprimir datos del riesgo.
        /// </summary>
        protected virtual void printRiskData() { }
        /// <summary>
        /// Informacion Riesgo - 
        /// Metodo base para imprimir categoria del riesgo.
        /// </summary>
        protected virtual void printRiskCategory() { }
        /// <summary>
        /// Informacion Riesgo - 
        /// Metodo base para imprimir coberturas.
        /// </summary>
        protected virtual void printRiskCoverages() { }
        /// <summary>
        /// Informacion Riesgo -
        /// Impresion informacion de la seccion del riesgo.
        /// </summary>
        protected virtual void printRiskBody(DataSet _risk) { }
        /// <summary>
        /// Imprime Forma (PROD.PRODUCT_FORM)
        /// </summary>
        protected virtual void printRiskFormDescription()
        {
            if (base.FormDescription.Length > 0)
            {
                this.validatePageLimit(2);
                this.File.Add(new DatRecord("\n" + base.FormDescription, null));
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Metodo base para imprimir clausulas.
        /// </summary>
        public virtual void printRiskClauses()
        {
            if (this.RiskClauses.Rows.Count > 0)
            {
                foreach (DataRow row in this.RiskClauses.Rows)
                {
                    this.validatePageLimit(2);
                    File.Add(new DatRecord(string.Format(RptFields.LBL_CLAUSES_TITLE, row["ClauseTitle"]), null));
                    //this.validatePageLimit(1);

                    string[] ClauseParagraphs = row["ClauseText"].ToString().Split('\n');

                    PrintParagraphArray(ClauseParagraphs);
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime encabezado de seccion de beneficiarios.
        /// </summary>
        protected virtual void printBeneficiariesHeader()
        {
            this.validatePageLimit(2);
            this.File.Add(new DatRecord("\nBeneficiarios", null));
            this.validatePageLimit(1);
            this.File.Add(new DatRecord(ReportServiceHelper.leftAlign("Nombre/Razon Social", 45) +
                            ReportServiceHelper.rightAlign("Documento", 20) +
                            ReportServiceHelper.rightAlign("Porcentaje", 15) + " " +
                            ReportServiceHelper.rightAlign("Tipo Beneficiario", 18), null));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Metodo base para imprimir beneficiarios.
        /// </summary>
        protected virtual void printRiskBeneficiaries()
        {
            if (this.RiskBeneficiaries.Rows.Count > 0)
            {
                foreach (DataRow row in this.RiskBeneficiaries.Rows)
                {
                    if (this.ActRiskNum.ToString().Equals(row["RiskNum"].ToString()))
                    {
                        this.validatePageLimit(2);
                        //TODO:     <<<< Codigo:PV3G06-AE075;Autor:Jonnathan Garzon; Fecha: 25/10/2012
                        //Asunto:   Cambios en valores para impresion. Ahora se toman el nombre y documento del DataSet de Beneficiarios
                        //          en lugar de los valores del this, ya que se repetia en mismo valor por cada beneficiario.
                        this.File.Add(new DatRecord(ReportServiceHelper.leftAlign(row["Name"].ToString() == null ? "" : row["Name"].ToString(), 45) +
                        //Cesar giraldo asunto: cambio alineación a derecha 31/10/2013
                        ReportServiceHelper.rightAlign(row["Document"].ToString() == null ? "" : row["Document"].ToString(), 20) +
                        ReportServiceHelper.rightAlign(row["Percentage"].ToString(), 15) + " " +
                        ReportServiceHelper.rightAlign(row["Type"].ToString(), 18), null));
                        //Cesar giraldo asunto: cambio alineación 31/10/2013
                        //Autor:	Jonnathan Garzon; Fecha: 25/10/2012;>>

                        //this.File.Add(new DatRecord(ReportServiceHelper.leftAlign(this.BeneficiaryName == null ? "" : this.BeneficiaryName, 45) +
                        //    ReportServiceHelper.centerAlign(this.BeneficiaryDocumentNumber == null ? "" : this.BeneficiaryDocumentNumber, 20) +
                        //    ReportServiceHelper.centerAlign(row["Percentage"].ToString(), 15) + " " +
                        //    ReportServiceHelper.centerAlign(row["Type"].ToString(), 18), null));
                    }
                }
            }
        }

        #endregion
    }
}
