using Sistran.Company.Application.PrintingServices.Resources;
using Sistran.Company.PrintingService.JetForm.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    sealed class RiskRCPassengers : RiskAuto
    {
        #region Atributes

        /// <summary>
        /// Campo para almacenar descripcion del producto
        /// </summary>
        private string _productDescription;

        /// <summary>
        /// Campo para almacenar si el vehiculo fue repotenciado
        /// </summary>
        private string _isTransformVehicle;

        /// <summary>
        /// Campo para almacenar el año en el que el vehiculo fue repotenciado
        /// </summary>
        private string _yearTransformVehicle;

        /// <summary>
        /// Campo para almacenar la capacidad del vehiculo en galones
        /// </summary>
        private string _gallonQty;

        /// <summary>
        /// Campo para almacenar el tipo de carga del vehiculo
        /// </summary>
        private string _transportCargoType;

        /// <summary>
        /// Campo para almacenar el año del vehiculo
        /// </summary>
        private string _yearVehicle;

        /// <summary>
        /// Campo para almacenar la placa del vehiculo
        /// </summary>
        private string _licensePlate;

        /// <summary>
        /// Campo para almacenar el motor del vehiculo
        /// </summary>
        private string _engineSerial;

        /// <summary>
        /// Campo para almacenar el chasis del vehiculo
        /// </summary>
        private string _chassisSerial;

        /// <summary>
        /// Informacion Riesgo - 
        /// Accesorios
        /// </summary>
        private DataTable _riskAccesories;

        #endregion

        #region Properties

        /// <summary>
        /// Propiedad para acceder al campo _productDescription
        /// </summary>
        private string ProductDescription
        {
            get { return _productDescription; }
            set { _productDescription = value; }
        }

        private string IsTransformVehicle
        {
            get { return _isTransformVehicle; }
            set { _isTransformVehicle = value; }
        }

        private string YearTransformVehicle
        {
            get { return _yearTransformVehicle; }
            set { _yearTransformVehicle = value; }
        }

        private string GallonQty
        {
            get { return _gallonQty; }
            set { _gallonQty = value; }
        }

        private string TransportCargoType
        {
            get { return _transportCargoType; }
            set { _transportCargoType = value; }
        }

        private string YearVehicle
        {
            get { return _yearVehicle; }
            set { _yearVehicle = value; }
        }

        private string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }

        private string EngineSerial
        {
            get { return _engineSerial; }
            set { _engineSerial = value; }
        }

        private string ChassisSerial
        {
            get { return _chassisSerial; }
            set { _chassisSerial = value; }
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
        /// Agrego la propiedad ProductDescription
        /// </summary>
        protected override void setRiskData()
        {
            base.setRiskData();

            this.BranchCity = this.RiskData.Tables["Table"].Rows[0]["BRANCH_CITY"].ToString();

            this.YearVehicle = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_YEAR"].ToString();//Anio de vehiculo desde tabla CIA_RISK_VEHICLE
            this.LicensePlate = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_LICENSE"].ToString();
            this.EngineSerial = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_ENGINE_SER"].ToString();
            this.ChassisSerial = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_CHASSIS_SER"].ToString();
            this.BeneficiaryName = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_NAME"].ToString();
            this.BeneficiaryDocumentType = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC_TYPE"].ToString();
            this.IsTransformVehicle = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_USE"].ToString();//Vehiculo repotenciado
            this.TransportCargoType = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_ADDRESS"].ToString();//Tipo de sustancia de carga
            this.YearTransformVehicle = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_SERVICE_TYPE"].ToString();//Año repotenciado del vehiculo
            this.GallonQty = this.RiskData.Tables["Table"].Rows[0]["VEHICLE_PASSENGER_QTY"].ToString();//Cantidad de galones del vehiculo

            if (BeneficiaryDocumentType.Equals(RptFields.LBL_NIT))
                this.BeneficiaryDocumentNumber = ReportServiceHelper.formatNIT(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString());
            else
                this.BeneficiaryDocumentNumber = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString();

            this.initializeCoveragesDataTable();
            this.initializeBeneficiariesDataTable();
            //this.initializeAccesoriesDataTable();
            this.initializeClausesDataTable();

            this.ProductDescription = this.RiskData.Tables["Table"].Rows[0]["PRODUCT_DESCRIPTION"].ToString();
        }

        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime informacion del riesgo.
        /// </summary>
        protected override void printRiskData()
        {
            if (this.ActRiskNum != 0)
            {
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
            this.File.Add(new DatRecord(string.Format(RptFields.TTL_MODEL, this.YearVehicle), null));
            this.File.Add(new DatRecord(string.Format(RptFields.LBL_PLATE_ENGINE_CHASIS_VDATA, this.LicensePlate, this.EngineSerial,
                                                     this.ChassisSerial), null));

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(string.Format(RptFields.TTL_CARGO_TYPE, this.GallonQty, this.TransportCargoType), null));

            this.validatePageLimit(1);
            if (this.IsTransformVehicle == "SI")
                this.File.Add(new DatRecord(string.Format(RptFields.TTL_TRANSFORM_VEHICLE_COMP, this.IsTransformVehicle, this.YearTransformVehicle), null));
            else
                this.File.Add(new DatRecord(string.Format(RptFields.TTL_TRANSFORM_VEHICLE, this.IsTransformVehicle), null));

            this.validatePageLimit(1);
        }

        /// <summary>
        /// Imprime primero la descripcion del producto y despues las coberturas
        /// </summary>
        protected override void printRiskCoverages()
        {
            this.printRiskCategory();
            base.printRiskCoverages();
        }

        /// <summary>
        /// Imprime descripcion del producto
        /// </summary>
        protected override void printRiskCategory()
        {
            this.validatePageLimit(2);
            this.File.Add(new DatRecord("\n", string.Format(RptFields.LBL_PRODUCT_DESCRIPTION, this.ProductDescription)));
        }

        protected override void printBeneficiariesSection()
        {
            if (this.ActRiskNum != 0)
            {
                this.printBeneficiariesHeader();
                this.printRiskBeneficiaries();
            }
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

        #endregion
    }
}
