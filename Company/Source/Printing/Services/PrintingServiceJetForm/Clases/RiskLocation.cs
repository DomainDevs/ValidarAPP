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
    sealed class RiskLocation : RiskBase
    {
        #region Atributes

        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, calle.
        /// </summary>
        private string _additionalStreet;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, tipo.
        /// </summary>
        private string _addressType;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, apartamento.
        /// </summary>
        private string _apartment;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, bloque.
        /// </summary>
        private string _block;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, ciudad.
        /// </summary>
        private string _city;
        /// <summary>
        /// Informacion Riesgo -
        /// Almacena informacion de tipo de riesgo.
        /// </summary>
        private string _commRiskClass;
        /// <summary>
        /// Informacion Riesgo -
        /// Almacena informacion de clase de construccion.
        /// </summary>
        private string _constructionCategory;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, pais.
        /// </summary>
        private string _country;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, zona de cresta.
        /// </summary>
        private string _crestaZone;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, actividad economica.
        /// </summary>
        private string _economicActivity;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, PORCENTAJE EML Ó PML (PERDIDA MAXIMA ESTIMADA PARA REASEGUROS).
        /// </summary>
        private string _emlPct;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, piso.
        /// </summary>
        private string _floor;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, numero.
        /// </summary>
        private string _houseNumber;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, tipo de vivienda.
        /// </summary>
        private string _housingType;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, principal.
        /// </summary>
        private string _isMain;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, codigo de ubicacion.
        /// </summary>
        private string _locationCode;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, tipo de ocupacion.
        /// </summary>
        private string _occupationType;
        /// <summary>
        /// Informacion Riesgo -
        /// Almacena informacion de subtipo de riesgo.
        /// </summary>
        private string _riskCommSubtype;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, tipo de riesgo comercial.
        /// </summary>
        private string _riskCommercialType;
        /// <summary>
        /// Informacion Riesgo -
        /// Almacena informacion de tipo de peligrosidad.
        /// </summary>
        private string _riskDangerousness;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, estado.
        /// </summary>
        private string _state;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, calle.
        /// </summary>
        private string _street;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, tipo de calle.
        /// </summary>
        private string _streetType;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, urbanizacion.
        /// </summary>
        private string _urbanization;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, codigo ZIP.
        /// </summary>
        private string _zipCode;
        /// <summary>
        /// Informacion Direccion -
        /// Almacena informacion de direccion, texto del riesgo.
        /// </summary>
        private string _conditionText;
        /// <summary>
        /// Informacion Riesgo -
        /// Categorias
        /// </summary>
        private DataTable _riskCategories;
        /// <summary>
        /// Informacion Riesgo - 
        /// Garantias
        /// </summary>
        private DataTable _riskWarranties;

        #endregion

        #region Properties

        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _additionalStreet.
        /// </summary>
        public string AdditionalStreet
        {
            get { return _additionalStreet; }
            set { _additionalStreet = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _addressType.
        /// </summary>
        public string AddressType
        {
            get { return _addressType; }
            set { _addressType = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _apartment.
        /// </summary>
        public string Apartment
        {
            get { return _apartment; }
            set { _apartment = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _block.
        /// </summary>
        public string Block
        {
            get { return _block; }
            set { _block = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _city.
        /// </summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _commRiskClass.
        /// </summary>
        public string CommRiskClass
        {
            get { return _commRiskClass; }
            set { _commRiskClass = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _constructionCategory.
        /// </summary>
        public string ConstructionCategory
        {
            get { return _constructionCategory; }
            set { _constructionCategory = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _country.
        /// </summary>
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _crestaZone.
        /// </summary>
        public string CrestaZone
        {
            get { return _crestaZone; }
            set { _crestaZone = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _economicActivity.
        /// </summary>
        public string EconomicActivity
        {
            get { return _economicActivity; }
            set { _economicActivity = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _emlPct.
        /// </summary>
        public string EmlPct
        {
            get { return _emlPct; }
            set { _emlPct = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _floor.
        /// </summary>
        public string Floor
        {
            get { return _floor; }
            set { _floor = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _houseNumber.
        /// </summary>
        public string HouseNumber
        {
            get { return _houseNumber; }
            set { _houseNumber = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _housingType.
        /// </summary>
        public string HousingType
        {
            get { return _housingType; }
            set { _housingType = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _isMain.
        /// </summary>
        public string IsMain
        {
            get { return _isMain; }
            set { _isMain = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _locationCode.
        /// </summary>
        public string LocationCode
        {
            get { return _locationCode; }
            set { _locationCode = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _occupationType.
        /// </summary>
        public string OccupationType
        {
            get { return _occupationType; }
            set { _occupationType = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _riskCommSubtype.
        /// </summary>
        public string RiskCommSubtype
        {
            get { return _riskCommSubtype; }
            set { _riskCommSubtype = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _riskCommercialType.
        /// </summary>
        public string RiskCommercialType
        {
            get { return _riskCommercialType; }
            set { _riskCommercialType = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _riskDangerousness.
        /// </summary>
        public string RiskDangerousness
        {
            get { return _riskDangerousness; }
            set { _riskDangerousness = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _state.
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _street.
        /// </summary>
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _streetType.
        /// </summary>
        public string StreetType
        {
            get { return _streetType; }
            set { _streetType = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _urbanization.
        /// </summary>
        public string Urbanization
        {
            get { return _urbanization; }
            set { _urbanization = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _zipCode.
        /// </summary>
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }
        /// <summary>
        /// Informacion Direccion -
        /// Propiedad para acceder al campo _conditionText.
        /// </summary>
        public string ConditionText
        {
            get { return _conditionText; }
            set { _conditionText = value; }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Propiedad para acceder al campo _riskCategories.
        /// </summary>
        public DataTable RiskCategories
        {
            get { return _riskCategories; }
            set { _riskCategories = value; }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Propiedad para acceder al campo _riskWarranties.
        /// </summary>
        private DataTable RiskWarranties
        {
            get { return _riskWarranties; }
            set { _riskWarranties = value; }
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
            this.getRiskCategories();
            this.getRiskCoverages();
            this.getRiskBeneficiaries();
            this.getRiskWarranties();
            this.getRiskClauses();

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(this.Field + RptFields.SCC_COVERAGES_VDATA, null));

            this.printRiskData();
            this.printRiskCoverages();
            this.printRiskFormDescription();
            this.printBeneficiariesSection();
            this.printRiskWarranties();
            this.printRiskClauses();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Llena campos necesarios para el riesgo.
        /// </summary>
        protected override void setRiskData()
        {
            base.setRiskData();
            this.InsuredTotalValue = "0";

            this.PromissoryNoteNum = this.RiskData.Tables["Table"].Rows[0]["PROMISSORY_NOTE_NUM_CD"].ToString();
            this.BranchCity = this.RiskData.Tables["Table"].Rows[0]["BRANCH_CITY"].ToString();

            this.AdditionalStreet = this.RiskData.Tables["Table"].Rows[0]["ADDITIONAL_STREET"].ToString();
            this.AddressType = this.RiskData.Tables["Table"].Rows[0]["ADDRESS_TYPE"].ToString();
            this.Apartment = this.RiskData.Tables["Table"].Rows[0]["APARTMENT"].ToString();
            this.Block = this.RiskData.Tables["Table"].Rows[0]["BLOCK"].ToString();
            this.City = this.RiskData.Tables["Table"].Rows[0]["CITY"].ToString();
            this.State = this.RiskData.Tables["Table"].Rows[0]["STATE"].ToString();
            this.Street = this.RiskData.Tables["Table"].Rows[0]["STREET"].ToString();
            this.StreetType = this.RiskData.Tables["Table"].Rows[0]["STREET_TYPE"].ToString();
            this.Urbanization = this.RiskData.Tables["Table"].Rows[0]["URBANIZATION"].ToString();
            this.ZipCode = this.RiskData.Tables["Table"].Rows[0]["ZIP_CODE"].ToString();

            this.BeneficiaryName = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_NAME"].ToString());
            this.BeneficiaryDocumentType = ReportServiceHelper.unicode_iso8859(this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC_TYPE"].ToString());

            if (BeneficiaryDocumentType.Equals(RptFields.LBL_NIT))
                this.BeneficiaryDocumentNumber = ReportServiceHelper.formatNIT(
                                                                    this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString());
            else
                this.BeneficiaryDocumentNumber = this.RiskData.Tables["Table"].Rows[0]["BENEFICIARY_DOC"].ToString();

            this.initializeCoveragesDataTable();
            this.initializeCategoriesDataTable();
            this.initializeBeneficiariesDataTable();
            this.initializeWarrantiesDataTable();
            this.initializeClausesDataTable();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar coberturas del riesgo.
        /// </summary>
        protected override void initializeCoveragesDataTable()
        {
            base.initializeCoveragesDataTable();
            this.RiskCoverages.Columns.Add("AcumVA", typeof(String));
            this.RiskCoverages.Columns.Add("Premium", typeof(String));
            this.RiskCoverages.Columns.Add("Deductible", typeof(String));
            this.RiskCoverages.Columns.Add("InsuredObjectId", typeof(String));
            this.RiskCoverages.Columns.Add("InsuredObjectDesc", typeof(String));
            this.RiskCoverages.Columns.Add("LineBusinessId", typeof(String));
            this.RiskCoverages.Columns.Add("LineBusinessDesc", typeof(String));
            this.RiskCoverages.Columns.Add("ConditionText", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar categorias del riesgo.
        /// </summary>
        private void initializeCategoriesDataTable()
        {
            this.RiskCategories = this.RiskCoverages.Copy();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Crea el DataTable para almacenar garantias del riesgo.
        /// </summary>
        private void initializeWarrantiesDataTable()
        {
            this.RiskWarranties = new DataTable();
            this.RiskWarranties.Columns.Add("Accessory", typeof(String));
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Llena tabla de RiskCoverages
        /// </summary>
        protected override void getRiskCoverages()
        {
            //TODO:     Autor:Jonnathan Garzon; Fecha: 27/02/2013
            //Asunto:   Ajustes adicionales para acumulados de iva según coberturas
            bool withSublimitValue = false; // Variable que determina si la cobertura padre requiere que las coberturas hijas acumulen iva
            base.getRiskCoverages();

            int rowNum = 1;

            string[] sublimitCoverages = RptFields.TTL_LOCATION_SUBLIMIT_COVERAGES.Split(Convert.ToChar("|"));

            DataRow[] drSublimitList = this.RiskData.Tables["Table1"].Select(
                String.Format("COVERAGE_NUM <> 0 AND (COVERAGE = '{0}' OR COVERAGE = '{1}')", sublimitCoverages));
            //TODO:     <<<< Codigo:PV3G06-AE073;Autor:Jonnathan Garzon; Fecha: 22/10/2012
            //Asunto:   Adicion de parametros para organizacion de valores por riesgo, para impresion
            DataRow[] drList = this.RiskData.Tables["Table1"].Select(
                String.Format("COVERAGE_NUM <> 0 AND (COVERAGE <> '{0}' AND COVERAGE <> '{1}')", sublimitCoverages), "INO_DESC, NAME_ORDER, ORDER_COVERAGE");
            //Autor:	Jonnathan Garzon; Fecha: 22/10/2012;>>

            DataTable finalList = this.RiskData.Tables["Table1"].Clone();
            foreach (DataRow row in drList)
            {
                finalList.ImportRow(row);
                foreach (DataRow subRow in drSublimitList)
                {
                    if (row["COVERAGE_NUM"].ToString() == subRow["COVERAGE_NUM"].ToString() &&
                        row["ORDER_COVERAGE"].ToString() == subRow["ORDER_COVERAGE"].ToString())
                    {
                        finalList.ImportRow(subRow);
                    }
                }
            }

            foreach (DataRow row in finalList.Rows)
            {
                string premiumAmt = row["PREMIUM_AMT"].ToString().Replace(",", string.Empty).Split('.')[0];
                string limitAmt = row["LIMIT_OCCURRENCE_AMT"].ToString().Replace(",", string.Empty).Split('.')[0];

                DataRow newRow = this.RiskCoverages.NewRow();
                newRow["RiskNum"] = row["RISK_NUM"].ToString();
                //TODO:     Julio Guzmán 14/04/2011
                //ASUNTO:   Si es Child no se asigna coverageNum
                if (Convert.ToBoolean(row["IS_CHILD"].ToString()))
                    newRow["CoverageNum"] = string.Empty;
                else
                {
                    newRow["CoverageNum"] = rowNum.ToString();
                    rowNum++;
                    //TODO:     Autor:Jonnathan Garzon; Fecha: 01/03/2013
                    //Asunto:   Reestructuración para manejo de valores de coberturas y acumulados de iva

                    // Se evalúa si la cobertura padre es Suma asegurada y con ello marcar las coberturas hijas en SI
                    if (row["COVERAGE"].ToString().ToUpper().Trim().Equals("EXTRACONTRACTUAL-RESTABLECIMIENTO AUTOMATICO DE SUMA ASEGURADA"))
                        withSublimitValue = true;
                    else
                        withSublimitValue = false;
                }
                //TODO: Julio Guzmán Fecha: 14/04/2011

                newRow["PrintDescription"] = row["COVERAGE"];
                // << TODO: Julio Guzmán 14/04/2011
                //          Si es SUBLIMITE se asigna al InsuredValue el LIMIT_OCCURRENCE_AMT

                // ZONA PARA TRATAMIENTO DE VALORES
                if (!(row["COVERAGE"].ToString().ToUpper().Trim().Equals("SUBLIMITE POR EVENTO O PERSONA")) &&
                    !(row["COVERAGE"].ToString().ToUpper().Trim().Equals("LIMITE POR EVENTO O PERSONA")))
                {
                    if (row["COVERAGE"].ToString().ToUpper().Trim().Equals("SUBLIMITE AGREGADO ANUAL"))
                        newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["COVERAGE_PREMIUM"].ToString(), new CultureInfo("en-US"));
                    else
                        newRow["InsuredValue"] = ReportServiceHelper.formatMoney(row["COVERAGE_PREMIUM"].ToString(), new CultureInfo("en-US"));
                }
                else
                {
                    string limitOccurrenceAmt = row["LIMIT_OCCURRENCE_AMT"].ToString().Replace(",", ".");
                    newRow["InsuredValue"] = ReportServiceHelper.formatMoney(limitOccurrenceAmt, new CultureInfo("en-US"));
                }
                    
                // ZONA PARA TRATAMIENTO DE ACUMULADOS DE IVA
                if (!(row["COVERAGE"].ToString().ToUpper().Trim().Equals("SUBLIMITE POR EVENTO O PERSONA")) &&
                    !(row["COVERAGE"].ToString().ToUpper().Trim().Equals("LIMITE POR EVENTO O PERSONA")))
                {
                    if (row["COVERAGE"].ToString().ToUpper().Trim().Equals("SUBLIMITE AGREGADO ANUAL"))
                        if (withSublimitValue)
                            newRow["AcumVA"] = "SI";
                        else
                            newRow["AcumVA"] = "NO";
                    else
                    {
                        if (row["COVERAGE"].ToString().ToUpper().Trim().Equals("EXTRACONTRACTUAL-PREDIOS, LABORES Y OPERACIONES") ||
                            row["COVERAGE"].ToString().ToUpper().Trim().Equals("EXTRACONTRACTUAL-RESTABLECIMIENTO AUTOMATICO DE SUMA ASEGURADA"))
                            newRow["AcumVA"] = "SI";
                        else
                        {
                            if (!Convert.ToInt64(limitAmt).Equals(0))
                                newRow["AcumVA"] = "SI";
                            else
                                newRow["AcumVA"] = "NO";
                        }
                    }
                }
                else
                {
                    if (withSublimitValue)
                        newRow["AcumVA"] = "SI";
                    else
                        newRow["AcumVA"] = "NO";
                }
                //TODO: Autor:Jonnathan Garzon; Fecha: 01/03/2013
                //TODO: Julio Guzmán Fecha: 14/04/2011

                newRow["Premium"] = ReportServiceHelper.formatMoney(row["PREMIUM_AMT"].ToString().Replace(",", "."), new CultureInfo("en-US"));
                newRow["Deductible"] = row["COVERAGE_DEDUCT"];
                newRow["InsuredObjectId"] = row["INO_ID"];
                newRow["InsuredObjectDesc"] = row["INO_DESC"];
                newRow["LineBusinessId"] = row["LNB_CD"];
                newRow["LineBusinessDesc"] = row["LNB_DESC"];
                newRow["ConditionText"] = row["CONDITION_TEXT"];

                this.RiskCoverages.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// Informacion Riesgo - 
        /// Llena tabla de RiskCategories
        /// </summary>
        private void getRiskCategories()
        {
            DataRow[] drList = this.RiskData.Tables["Table1"].Select("COVERAGE_NUM = 0", "INO_DESC, LNB_DESC, NAME_ORDER, ORDER_COVERAGE");

            foreach (DataRow row in drList)
            {
                int cntInsutedObject = 0;
                if (this.RiskCategories.Rows.Count > 0)
                    cntInsutedObject = this.RiskCategories.Select(string.Format("InsuredObjectDesc = '{0}'", row["INO_DESC"].ToString())).Count();

                if (cntInsutedObject == 0)
                {
                    string premiumAmt = row["PREMIUM_AMT"].ToString().Replace(",", string.Empty).Split('.')[0];
                    string limitAmt = row["LIMIT_OCCURRENCE_AMT"].ToString().Replace(",", string.Empty).Split('.')[0];

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
                    newRow["ConditionText"] = row["CONDITION_TEXT"];

                    this.RiskCategories.Rows.Add(newRow);
                }
            }
        }

        // <summary>
        /// Informacion Riesgo - 
        /// Llena tabla de RiskWarranties
        /// </summary>
        private void getRiskWarranties()
        {
            foreach (DataRow row in this.RiskData.Tables["Table2"].Rows)
            {
                DataRow newRow = this.RiskWarranties.NewRow();
                newRow["Accessory"] = row["DESCRIPTION"];
                this.RiskWarranties.Rows.Add(newRow);
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
        /// Informacion Riesgo - 
        /// Imprime informacion del riesgo.
        /// </summary>
        protected override void printRiskData()
        {
            base.printRiskData();
        }
        /// <summary>
        /// Informacion Riesgo - 
        /// Imprime seccion de coberturas del riesgo.
        /// </summary>
        protected override void printRiskCoverages()
        {
            base.printRiskCoverages();

            int category = 0;
            int lineBusiness = 0;
            this.printRiskNumberInformation();

            if (this.RiskCategories.Rows.Count > 0)
            {
                foreach (DataRow categoriesRow in this.RiskCategories.Rows)
                {
                    category = Convert.ToInt32(categoriesRow["InsuredObjectId"].ToString());
                    //lineBusiness = Convert.ToInt32(categoriesRow["LineBusinessId"].ToString());

                    if (!category.Equals(0))
                    {
                        this.printLocation(category, this.ActRiskNum);
                    }

                    this.printCoverageHeader();

                    foreach (DataRow row in this.RiskCoverages.Rows)
                    {
                        int addLines = 0;

                        if ((row["InsuredObjectId"].ToString().Equals(category.ToString())) ||
                            category.Equals(0))
                        {
                            //TODO: Julio Guzmán, 14/04/2011, Se valida para que imprima las coberturas hijas indentadas
                            int coverageNumSpace = (row["CoverageNum"].ToString().Length == 0) ? 3 : 2;
                            int coverageSpace = (coverageNumSpace == 3) ? 18 : 19;

                            string deductibleText = (row["Deductible"].ToString().Trim().Length > 0) ? "\n" +
                                                    ReportServiceHelper.leftAlign(string.Empty, 4) + "Deducible: " +
                                                    ReportServiceHelper.leftAlign(row["Deductible"].ToString(), 90) :
                                                    string.Empty;

                            string line = string.Format(RptFields.FLD_CONTRACT_COVERAGES,
                                                        ReportServiceHelper.leftAlign(row["CoverageNum"].ToString(), coverageNumSpace),
                                                        ReportServiceHelper.leftAlign(row["PrintDescription"].ToString(), 49),
                                                        ReportServiceHelper.rightAlign(row["InsuredValue"].ToString(), coverageSpace) + " ",
                                                        ReportServiceHelper.centerAlign(row["AcumVA"].ToString(), 10),
                                                        ReportServiceHelper.rightAlign(row["Premium"].ToString(), 13),
                                                        deductibleText, string.Empty);

                            addLines = (deductibleText.Equals(string.Empty)) ? 1 : 2;
                            this.validatePageLimit(addLines);
                            this.File.Add(new DatRecord(line, null));
                        }
                    }
                }
            }
            else
            {
                this.printCoverageHeader();

                foreach (DataRow row in this.RiskCoverages.Rows)
                {
                    int addLines = 0;

                    if ((row["InsuredObjectId"].ToString().Equals(category.ToString()) &&
                        row["RiskNum"].ToString().Equals(this.ActRiskNum.ToString())) ||
                        category.Equals(0))
                    {
                        //TODO: Julio Guzmán, 14/04/2011, Se valida para que imprima las coberturas hijas indentadas
                        int coverageNumSpace = (row["CoverageNum"].ToString().Length == 0) ? 3 : 2;
                        int coverageSpace = (coverageNumSpace == 3) ? 18 : 19;

                        string deductibleText = (row["Deductible"].ToString().Trim().Length > 0) ? "\n" +
                                                ReportServiceHelper.leftAlign(string.Empty, 4) + "Deducible: " +
                                                ReportServiceHelper.leftAlign(row["Deductible"].ToString(), 90) :
                                                string.Empty;

                        string line = string.Format(RptFields.FLD_CONTRACT_COVERAGES,
                                                    ReportServiceHelper.leftAlign(row["CoverageNum"].ToString(), coverageNumSpace),
                                                    ReportServiceHelper.leftAlign(row["PrintDescription"].ToString(), 49),
                                                    ReportServiceHelper.rightAlign(row["InsuredValue"].ToString(), coverageSpace) + " ",
                                                    ReportServiceHelper.centerAlign(row["AcumVA"].ToString(), 10),
                                                    ReportServiceHelper.rightAlign(row["Premium"].ToString(), 13),
                                                    deductibleText, string.Empty);

                        addLines = (deductibleText.Equals(string.Empty)) ? 1 : 2;
                        this.validatePageLimit(addLines);
                        this.File.Add(new DatRecord(line, null));
                    }
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime informacion de Riesgo, numero y dirección.
        /// </summary>
        private void printRiskNumberInformation()
        {
            if (this.ActRiskNum > 0)
            {
                String riskLocation = string.Format(RptFields.LBL_RISK, this.ActRiskNum.ToString(), this.Street.ToString(), this.City.ToString(), this.State.ToString());
                this.validatePageLimit(1);
                this.File.Add(new DatRecord("\n", null));
                //TODO: Julio Guzmán, 24/01/2011, Se añade validacion para cuando no traiga informacion de ubicacion deje solo el numero del riesgo
                if ((this.Street == string.Empty) && (this.City == string.Empty) && (this.State == string.Empty))
                {
                    riskLocation = riskLocation.Replace(",", "");
                }
                this.validatePageLimit(1);
                this.File.Add(new DatRecord(riskLocation, null));
                //this.validatePageLimit(3);
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime informacion de categoria
        /// </summary>
        /// <param name="category">Categoria</param>
        /// <param name="riskNum">Numero de riesgo</param>
        private void printLocation(int category, int riskNum)
        {
            foreach (DataRow row in this.RiskCategories.Rows)
            {
                if (row["InsuredObjectId"].ToString().Equals(category.ToString()) &&
                    row["RiskNum"].ToString().Equals(riskNum.ToString()))
                {
                    // this.ActRiskNum = riskNum; 
                    // DataSet risk = this.getActualRisk(this.dsPoliza);
                    // setInsuredLocationData(risk);

                    //this.validatePageLimit(2);
                    String riskCategory = string.Format(RptFields.LBL_INSURED_OBJECT, row["InsuredObjectDesc"].ToString());
                    this.validatePageLimit(1);
                    this.File.Add(new DatRecord("\n", null));

                    this.validatePageLimit(1);
                    this.File.Add(new DatRecord(riskCategory, null));

                    break;
                }
            }
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime encabezado de amparos
        /// </summary>
        private void printCoverageHeader()
        {
            this.validatePageLimit(2);
            this.File.Add(new DatRecord(RptFields.SCT_CONTRACT_COVERAGES, null));

            this.validatePageLimit(1);
            this.File.Add(new DatRecord(ReportServiceHelper.leftAlign("No.", 4) +
                            ReportServiceHelper.leftAlign("Amparo", 50) +
                            ReportServiceHelper.rightAlign(" Valor Asegurado", 18) + " " +
                            ReportServiceHelper.centerAlign("AcumVA", 12) +
                            ReportServiceHelper.rightAlign("Prima", 13), null));
        }
        /// <summary>
        /// Informacion Riesgo -
        /// Imprime garantias del riesgo.
        /// </summary>
        private void printRiskWarranties()
        {
            if (this.RiskWarranties.Rows.Count > 0)
            {
                this.validatePageLimit(2);
                this.File.Add(new DatRecord(RptFields.LBL_CONTRACT_WARRANTIES, null));

                foreach (DataRow row in this.RiskWarranties.Rows)
                {
                    this.validatePageLimit(1);
                    this.File.Add(new DatRecord(row["Accessory"].ToString(), null));
                }
            }
        }

        #endregion
    }
}
