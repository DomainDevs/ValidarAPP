using Sistran.Company.Application.PrintingServices.Models;
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
    internal class RiskAuto : RiskBase
    {
        #region Atributes

        /// <summary>
        /// Informacion Vehiculo - 
        /// Serial del chasis
        /// </summary>
        private string _chassisSerial;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Color del vehiculo
        /// </summary>
        private string _color;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Serial del motor
        /// </summary>
        private string _engineSerial;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Codigo Fasecolda
        /// </summary>
        private string _fasecoldaCode;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Placa
        /// </summary>
        private string _licensePlate;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Marca
        /// </summary>
        private string _make;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Modelo
        /// </summary>
        private string _model;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Tipo carroceria
        /// </summary>
        private string _reference;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Tipo de servicio
        /// </summary>
        private string _serviceType;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Tipo de vehiculo
        /// </summary>
        private string _vehicleType;
        /// <summary>
        /// Informacion Vehiculo - 
        /// Año
        /// </summary>
        private string _year;
        /// <summary>
        /// Informacion Riesgo -
        /// Cantidad de pasajeros
        /// </summary>
        private string _passengersQuantity;
        /// <summary>
        /// Informacion Riesgo - 
        /// Accesorios
        /// </summary>
        private DataTable _riskAccesories;

        #endregion

        #region Properties

        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _chassisSerial
        /// </summary>
        private string ChassisSerial
        {
            get { return _chassisSerial; }
            set { _chassisSerial = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _color
        /// </summary>
        private string Color
        {
            get { return _color; }
            set { _color = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _engineSerial
        /// </summary>
        private string EngineSerial
        {
            get { return _engineSerial; }
            set { _engineSerial = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _fasecoldaCode
        /// </summary>
        private string FasecoldaCode
        {
            get { return _fasecoldaCode; }
            set { _fasecoldaCode = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _licensePlate
        /// </summary>
        private string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _make
        /// </summary>
        private string Make
        {
            get { return _make; }
            set { _make = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _model
        /// </summary>
        private string Model
        {
            get { return _model; }
            set { _model = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _reference
        /// </summary>
        private string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _serviceType
        /// </summary>
        private string ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _vehicleType
        /// </summary>
        private string VehicleType
        {
            get { return _vehicleType; }
            set { _vehicleType = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _year
        /// </summary>
        private string Year
        {
            get { return _year; }
            set { _year = value; }
        }
        /// <summary>
        /// Informacion Vehiculo -
        /// Campo para acceder al campo _passengersQuantity
        /// </summary>
        private string PassengersQuantity
        {
            get { return _passengersQuantity; }
            set { _passengersQuantity = value; }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Campo para acceder al campo _riskAccesories
        /// </summary>
        private DataTable RiskAccesories
        {
            get { return _riskAccesories; }
            set { _riskAccesories = value; }
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
            this.getRiskAccesories();
            this.getRiskClauses();

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(this.Field + RptFields.SCC_COVERAGES_VDATA, null));

            this.printBeneficiariesSection();
            this.printRiskData();
            this.printRiskCoverages();
            this.printRiskFormDescription();
            this.printRiskAccesories();
            this.printRiskClauses();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Llena campos necesarios para el riesgo y crea DataTables.
        /// </summary>
        protected override void setRiskData()
        {
            base.setRiskData();

            this.BranchCity = this.RiskData.Tables["Table"].Rows[0]["BRANCH_CITY"].ToString();

            this.FasecoldaCode = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_FASECOLDA_CD"].ToString();
            this.Make = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_MAKE"].ToString();
            this.Year = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_YEAR"].ToString();
            this.Color = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_COLOR"].ToString();
            this.Model = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_MODEL"].ToString();
            this.Reference = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_BODY"].ToString();
            this.VehicleType = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_TYPE"].ToString();
            this.ServiceType = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_SERVICE_TYPE"].ToString();
            this.LicensePlate = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_LICENSE"].ToString();
            this.EngineSerial = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_ENGINE_SER"].ToString();
            this.ChassisSerial = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_CHASSIS_SER"].ToString();
            this.BeneficiaryName = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_NAME"].ToString());
            this.BeneficiaryDocumentType = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC_TYPE"].ToString());

            if (!string.IsNullOrEmpty(this.RiskData.Tables["Table"].Rows[0]["VEHICLE_PASSENGER_QTY"].ToString()))
                this.PassengersQuantity = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_PASSENGER_QTY"].ToString();

            if (BeneficiaryDocumentType.Equals(RptFields.LBL_NIT))
                this.BeneficiaryDocumentNumber = ReportServiceHelper.formatNIT(
                                                                    this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString());
            else
                this.BeneficiaryDocumentNumber = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString();

            this.initializeCoveragesDataTable();
            this.initializeBeneficiariesDataTable();
            this.initializeAccesoriesDataTable();
            this.initializeClausesDataTable();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar coberturas del riesgo.
        /// </summary>
        protected override void initializeCoveragesDataTable()
        {
            base.initializeCoveragesDataTable();
            this.RiskCoverages.Columns.Add("Deductible", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar accesorios del riesgo.
        /// </summary>
        private void initializeAccesoriesDataTable()
        {
            this.RiskAccesories = new DataTable();
            this.RiskAccesories.Columns.Add("RiskNum", typeof(int));
            this.RiskAccesories.Columns.Add("Accessory", typeof(String));
            this.RiskAccesories.Columns.Add("InsuredValue", typeof(String));
            this.RiskAccesories.Columns.Add("Deductible", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Llena tabla de RiskCoverages
        /// </summary>
        protected override void getRiskCoverages()
        {
            base.getRiskCoverages();

            int intCoverageNum = 1;
            foreach (DataRow row in this.RiskData.Tables["Table1"].Rows)
            {
                if ((int)row["COVERAGE_NUM"] != 0)
                {
                    DataRow newRow = RiskCoverages.NewRow();

                    if (Convert.ToBoolean(row["IS_CHILD"].ToString()))
                    {
                        newRow["CoverageNum"] = string.Empty;
                    }
                    else
                    {
                        newRow["CoverageNum"] = intCoverageNum.ToString();
                        intCoverageNum++;
                    }

                    newRow["PrintDescription"] = row["COVERAGE"];
                    newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["COVERAGE_PREMIUM"].ToString(), CultureInfo.CurrentCulture);

                    if (this.ActRiskNum != 0)
                    {
                        newRow["Deductible"] = row["COVERAGE_DEDUCT"];
                    }
                    else
                    {
                        if (newRow["CoverageNum"].ToString().Equals(string.Empty))
                        {
                            newRow["InsuredValue"] = RptFields.FLD_DIVERS;
                        }

                        newRow["Deductible"] = string.Empty;
                    }

                    this.RiskCoverages.Rows.Add(newRow);
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Llena tabla de RiskAccesories
        /// </summary>
        private void getRiskAccesories()
        {
            foreach (DataRow row in this.RiskData.Tables["Table2"].Rows)
            {
                DataRow newRow = this.RiskAccesories.NewRow();
                newRow["RiskNum"] = row["RISK_NUM"];
                newRow["Accessory"] = row["DESCRIPTION"];
                newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["PREMIUM_DETAIL"].ToString(), CultureInfo.CurrentCulture);
                newRow["Deductible"] = "0,00";
                this.RiskAccesories.Rows.Add(newRow);
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime Seccion de Beneficiarios.
        /// </summary>
        protected virtual void printBeneficiariesSection()
        {
            if (this.ActRiskNum != 0)
            {
                this.validatePageLimit(2);
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
                //Autor:	Jonnathan Garzon; Fecha: 19/11/2012;>>
                //this.File.Add(new DatRecord(string.Format(RptFields.SCT_BENEFICIARIES,
                //                                          this.BeneficiaryName,
                //                                          this.BeneficiaryDocumentType + @":",
                //                                          this.BeneficiaryDocumentNumber),
                //                            null));
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
        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime informacion del riesgo.
        /// </summary>
        protected override void printRiskData()
        {
            if (this.ActRiskNum != 0)
            {
                base.printRiskData();
                this.printVehicleData();
            }
        }
        /// <summary>
        /// Informacion Vehiculo - 
        /// Imprime los datos del vehiculo.
        /// </summary>
        private void printVehicleData()
        {
            this.validatePageLimit(1);
            this.File.Add(new DatRecord(string.Format(RptFields.LBL_VEHICLE_DESCRIPTION_VDATA, this.ActRiskNum), null));

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(string.Format(RptFields.LBL_FASECOLDA_CD, FasecoldaCode.Replace(@" ", string.Empty)), null));

            this.validatePageLimit(1);

           // if (string.IsNullOrEmpty(this.PassengersQuantity))
                this.File.Add(new DatRecord(string.Format(RptFields.LBL_MAKE_YEAR_COLOR_VDATA, this.Make, this.Year, this.Color), null));
          //  else
          //      this.File.Add(new DatRecord(string.Format(RptFields.LBL_MAKE_YEAR_COLOR_PASSENGERS_VDATA, this.Make, this.Year, this.Color, this.PassengersQuantity), null));

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(string.Format(RptFields.LBL_MODEL_REFERENCE_TYPE_SERVICE_VDATA,
                                                      this.Model, string.Empty, this.VehicleType, this.ServiceType), null));

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(string.Format(RptFields.LBL_PLATE_ENGINE_CHASIS_VDATA, this.LicensePlate, this.EngineSerial,
                                                     this.ChassisSerial), null));
        }
        /// <summary>
        /// Imformacion Riesgo -
        /// Imprime informacion de categoria del riesgo.
        /// </summary>
        protected override void printRiskCategory()
        {
            base.printRiskCategory();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime seccion de coberturas del riesgo.
        /// </summary>
        protected override void printRiskCoverages()
        {
            base.printRiskCoverages();

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(RptFields.SCT_CONTRACT_COVERAGES, null));

            string ttlDeductible = (this.ActRiskNum != 0) ? RptFields.LBL_CONTRACT_COVERAGES_DEDUCTIBLE : string.Empty;

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(RptFields.LBL_CONTRACT_COVERAGES_NUMBER + string.Empty.PadLeft(36) +
                                        RptFields.LBL_CONTRACT_COVERAGES_INSURED_VALUE + ttlDeductible, null));

            if (RiskCoverages.Rows.Count > 0)
            {
                foreach (DataRow row in this.RiskCoverages.Rows)
                {
                    string txtDeductible = (this.ActRiskNum != 0) ? row["Deductible"].ToString() : string.Empty;

                    this.validatePageLimit(1);
                    int coverageNumSpace = (row["CoverageNum"].ToString().Length == 0) ? 3 : 2;
                    int coverageSpace = (coverageNumSpace == 3) ? 13 : 14;
                    string line = string.Format(RptFields.FLD_CONTRACT_VEHICLE_COVERAGES,
                                                ReportServiceHelper.completeColumnCoverages(row["CoverageNum"].ToString(), coverageNumSpace),
                                                ReportServiceHelper.completeColumnCoverages(row["PrintDescription"].ToString(), 41),
                                                ReportServiceHelper.completeColumn(row["InsuredValue"].ToString(), coverageSpace),
                                                txtDeductible, string.Empty, string.Empty, string.Empty);
                    this.File.Add(new DatRecord(line, null));
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime los accesorios del riesgo.
        /// </summary>
        private void printRiskAccesories()
        {
            if (this.RiskAccesories.Rows.Count > 0)
            {
                this.validatePageLimit(1);
                this.File.Add(new DatRecord(RptFields.LBL_CONTRACT_ACCESSORIES, null));

                int count = 1;
                foreach (DataRow row in this.RiskAccesories.Rows)
                {
                    this.validatePageLimit(1);
                    this.File.Add(new DatRecord(string.Format(RptFields.FLD_CONTRACT_ACCESSORIES,
                                                              ReportServiceHelper.completeColumnCoverages(row["Accessory"].ToString(), 44),
                                                              ReportServiceHelper.completeColumn(row["InsuredValue"].ToString(), 14),
                                                              row["Deductible"]),
                                                     null)
                                       );
                    if (count < this.RiskAccesories.Rows.Count) this.validatePageLimit(1);
                    count++;
                }
            }
        }

        #endregion
    }
}
