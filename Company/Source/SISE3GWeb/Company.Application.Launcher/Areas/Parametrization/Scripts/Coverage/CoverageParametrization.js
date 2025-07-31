//Clase formulario principal
var coverageParametrizationData = {};
var glbBandNameCoveragePrint = true;
var prvCoverage = {};
var originalCoverage2G = null;
class CoverageParametrization extends Uif2.Page {

    /**
    * @summary 
       *  Metodo que se ejecuta al instanciar la clase     
    */
    getInitialState() {
      
        request('Parametrization/Coverage/GetLineBusiness', null, 'GET', AppResources.ErrorSearchLineBusinessCONNEX, CoverageParametrization.getLineBusiness)
        request('Parametrization/Coverage/GetCompositionTypes', null, 'GET', AppResources.ErrorSearchCompositionTypesCONNEX, CoverageParametrization.getCompositionTypes)
        $("#Status").val(ParametrizationStatus.Create);
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#selectLineBusiness').on('itemSelected', this.ChangeLineBusiness);
        $("#inputCoverageSearch").on("search", this.coverageSearch);
        $("#btnNewCoverage").click(CoverageParametrization.NewCoverage);
        $("#btnExit").click(this.redirectIndex);
        $("#selectInsuranceObject").on('itemSelected', this.assignNameCoverage);
        $("#selectPeril").on('itemSelected', this.assignNameCoverage);
        $("#inputCoverageName").focusout(CoverageParametrization.focusoutInputCoverageName);
        $("#ImpressionName").focusout(this.focusoutImpressionName);
        $("#btnExport").click(this.sendExcelCoverage);
        $("#btnDeleteCoverage").click(this.DeleteCoverage);
        $("#btnSaveCoverage").click(this.SaveCoverage);
        $("#IsSeriousOffer").prop("disabled", true);
    }

    coverageSearch(event, value) {
        var technicalBranchId = parseInt($("#selectSearchTechnicalBranch").UifSelect('getSelected'));
        if (value.length < 3) {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.MustEnterThreeCharactersSearch, 'autoclose': true });
        }
        else if (!(technicalBranchId > 0)) {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.MustSelectBranchSearch, 'autoclose': true });
        }
        else {
            request('Parametrization/Coverage/GetCoverageSQMByDescriptionTechnicalBranchId', JSON.stringify({ description: value, technicalBranchId: technicalBranchId }), 'POST', AppResources.ErrorGetCoverageByRelationCONNEX, CoverageParametrization.setCoverageSearch);
        }
    }
    static setCoverageSearch(data) {
        if (data.length === 0) {
            CoverageParametrization.ClearForm();
            $.UifNotify("show", { 'type': "info", 'message': AppResources.CoverageNotExist, 'autoclose': true });
        }
        else if (data.length === 1) {
            CoverageParametrization.ClearForm();
            CoverageParametrization.setSelectedCoverage(data[0]);
            DeductiblesParametrization.LoadDeductiblesByCoverageId(data[0].DeductiblesServiceQueryModel.DeductibleServiceQueryModels);
            ClausesParametrization.LoadClausesByCoverageId(data[0].ClausesServiceQueryModel.ClauseServiceModels);
            DetailTypesParametrization.LoadDetailTypesByCoverageId(data[0].DetailTypesServiceQueryModel.DetailTypeServiceQueryModel);
            //PrintCoveragesParametrization.LoadPrintsByCoverageId(data[0].CoCoverageServiceModels);
        }
        else if (data.length > 1) {
            CoverageParametrizationAdvanced.ShowSearchAdv(data);
        }
        Coverage2GParametrization.LoadSubTitles();
    }
    static getLineBusiness(data) {
        $("#selectSearchTechnicalBranch").UifSelect({ sourceData: data, emptyText: AppResources.SelectBranch });
        $("#selectLineBusiness").UifSelect({ sourceData: data });
        unlockScreen();
    }
    static NewCoverage() {
        lockScreen();
        CoverageParametrization.ClearForm();
        CoverageParametrization.disableFields(true);
        request('Parametrization/Coverage/GetLineBusiness', null, 'GET', AppResources.ErrorSearchLineBusinessCONNEX, CoverageParametrization.getLineBusiness);

    }
    static ClearForm() {
        glbBandNameCoveragePrint = true;
        $("#Id").val("");
        $("#Coverage2GId").val(null);
        originalCoverage2G = null;
        prvCoverage = null;
        $("#InsuredObject2GId").val(null);
        $("#LineBusiness2GId").val(null);
        $("#SubLineBusiness2GId").val(null);
        $("#selectLineBusiness").UifSelect('setSelected', null);
        $("#selectSearchTechnicalBranch").UifSelect('setSelected', null);
        $("#inputCoverageSearch").val("");
        $("#selectSubLineBusiness").UifSelect();
        $("#selectPeril").UifSelect();
        $("#selectInsuranceObject").UifSelect();
        $("#inputCoverageName").val("");
        $("#selectCompositionType").UifSelect('setSelected', null);
        $("#principalIndicator").prop("checked", false);
        $("#IsImpression").prop("checked", false);
        $("#IsAccMinPremium").prop("checked", false);
        $("#IsAssistance").prop("checked", false);
        $("#IsPost").prop("checked", false);
        $("#IsSeriousOffer").prop("checked", false);
        $("#IsSeriousOffer").prop("disabled", true);
        $("#ImpressionName").val("");
        $("#ImpressionValue").val("");
        ClausesParametrization.LoadClausesInitial(glbClausesParam);
        DeductiblesParametrization.LoadDeductiblesInitial(glbDeductiblesParam);
        DetailTypesParametrization.LoadDetailTypesInitial(glbDetailTypes);
        CoverageParametrization.disableFields(false);
        $("#Status").val(ParametrizationStatus.Create);
        //CoverageParametrizationAdvanced.ClearAdvanced();
        ClearValidation("#formCoverage");
        //PrintCoveragesParametrization.ClearFormNewPrint();
        //PrintCoveragesParametrization.ClearListPrints();
        //PrintCoveragesParametrization.LoadListPrints();
        $("#selectCoverage2G").text('');
    }

    ChangeLineBusiness(event, selectedItem) {
        $("#inputCoverageName").val('');
        if (selectedItem.Id > 0) {
            request('Parametrization/Coverage/GetSubLineBusinessByLineBusinessId', JSON.stringify({ lineBusinessId: selectedItem.Id }), 'POST', AppResources.ErrorSearchSubLineBusinessCONNEX, CoverageParametrization.getSubLineBusinessByLineBusinessId)
            request('Parametrization/Coverage/GetPerilsByLineBusinessId', JSON.stringify({ lineBusinessId: selectedItem.Id }), 'POST', AppResources.ErrorSearchPerilCONNEX, CoverageParametrization.getPerilsByLineBusinessId)
            request('Parametrization/Coverage/GetInsuredObjectsByLineBusinessId', JSON.stringify({ lineBusinessId: selectedItem.Id }), 'POST', AppResources.ErrorSearchInsuredObjectCONNEX, CoverageParametrization.getInsuredObjectsByLineBusinessId)
            $("#inputCoverageName").val("");
            $("#ImpressionName").val("");
            if (PrefixType.CUMPLIMIENTO == selectedItem.Id || PrefixType.ARRENDAMIENTO == selectedItem.Id) {
                $("#IsSeriousOffer").prop("disabled", false);
            }
            else {
                $("#IsSeriousOffer").prop("disabled", true);
                $("#IsSeriousOffer").prop("checked", false);
            }
        }
        else {
            $("#selectSubLineBusiness").UifSelect();
            $("#selectPeril").UifSelect();
            $("#selectInsuranceObject").UifSelect();
        }

    }

    static getSubLineBusinessByLineBusinessId(data) {
        $("#selectSubLineBusiness").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
        if (data.selectedId) {
            $("#selectSubLineBusiness").UifSelect("disabled", true); //X funcionalidad de framework
        }
    }

    static getPerilsByLineBusinessId(data) {
        $("#selectPeril").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
        if (data.selectedId) {
            $("#selectPeril").UifSelect("disabled", true); //X funcionalidad de framework
        }
    }

    static getInsuredObjectsByLineBusinessId(data) {
        $("#selectInsuranceObject").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
        if (data.selectedId) {
            $("#selectInsuranceObject").UifSelect("disabled", true); //X funcionalidad de framework

        }
    }

    static getCompositionTypes(data) {
        $("#selectCompositionType").UifSelect({ sourceData: data });
    }

    /**
   * @summary 
    * Metodo para cargar la informacion de la cobertura en la pantalla principal
   */
    static setSelectedCoverage(coverage) {
        CoverageParametrization.GetPrvCoverage(coverage.Id, coverage.Peril.Id).done(function (data) {
            if (data.success) {
                $("#Id").val(coverage.Id);
                if (data.result !== null) {
                    prvCoverage = data.result;
                    prvCoverage.BeginDate = moment(prvCoverage.BeginDate).toDate();
                    $("#IsPost").prop("checked", data.result.IsPost);//Postcontractual
                }
            }
        });
        if (coverage.Homologation2G !== null && coverage.Homologation2G !== undefined) {
            $("#Coverage2GId").val(coverage.Homologation2G.Id);
            $("#InsuredObject2GId").val(coverage.Homologation2G.InsuredObjectId);
            $("#LineBusiness2GId").val(parseInt(coverage.Homologation2G.LineBusinessId));
            $("#SubLineBusiness2GId").val(parseInt(coverage.Homologation2G.SubLineBusinessId));
            originalCoverage2G = {
                CoverageId2G: parseInt($("#Coverage2GId").val()),
                InsuredObject2G: parseInt($("#InsuredObject2GId").val()),
                LineBusiness2G: parseInt($("#LineBusiness2GId").val()),
                SubLineBusiness2G: parseInt($("#SubLineBusiness2GId").val())
            };
        }
        else {
            $("#Coverage2GId").val(null);
            $("#InsuredObject2GId").val(null);
            $("#LineBusiness2GId").val(null);
            $("#SubLineBusiness2GId").val(null);
            originalCoverage2G = null;
        }
        if (coverage != null) $("#Id").val(coverage.Id);
        $("#selectLineBusiness").UifSelect("setSelected", coverage.LineBusiness.Id);
        $("#selectSearchTechnicalBranch").UifSelect("setSelected", coverage.LineBusiness.Id);
        $("#inputCoverageName").val(coverage.Description);
        $("#inputCoverageSearch").val(coverage.Description);
        $("#selectCompositionType").UifSelect("setSelected", coverage.CompositionTypeId);
        request('Parametrization/Coverage/GetSubLineBusinessByLineBusinessId', JSON.stringify({ lineBusinessId: coverage.LineBusiness.Id, selectedId: coverage.SubLineBusiness.Id }), 'POST', AppResources.ErrorSearchSubLineBusinessCONNEX, CoverageParametrization.getSubLineBusinessByLineBusinessId)
        request('Parametrization/Coverage/GetPerilsByLineBusinessId', JSON.stringify({ lineBusinessId: coverage.LineBusiness.Id, selectedId: coverage.Peril.Id }), 'POST', AppResources.ErrorSearchPerilCONNEX, CoverageParametrization.getPerilsByLineBusinessId)
        request('Parametrization/Coverage/GetInsuredObjectsByLineBusinessId', JSON.stringify({ lineBusinessId: coverage.LineBusiness.Id, selectedId: coverage.InsuredObject.Id }), 'POST', AppResources.ErrorSearchInsuredObjectCONNEX, CoverageParametrization.getInsuredObjectsByLineBusinessId)
        $("#principalIndicator").prop("checked", coverage.IsPrincipal);
        $("#IsImpression").prop("checked", coverage.CoCoverageServiceModel.IsImpression);
        $("#IsAccMinPremium").prop("checked", coverage.CoCoverageServiceModel.IsAccMinPremium);
        $("#IsSeriousOffer").prop("checked", coverage.CoCoverageServiceModel.IsSeriousOffer);
        //$("#IsAssistance").prop("checked", coverage.CoCoverageServiceModel.IsAssistance);
        $("#IsAssistance").prop("checked", false);
        $("#ImpressionName").val(coverage.CoCoverageServiceModel.Description);
        $("#ImpressionValue").val(coverage.CoCoverageServiceModel.ImpressionValue);
        CoverageParametrization.disableFields(true);
        if (coverage.LineBusiness.Id == PrefixType.CUMPLIMIENTO || coverage.LineBusiness.Id == PrefixType.ARRENDAMIENTO) {
            $("#IsSeriousOffer").prop("disabled", false);
        }
        else {
            $("#IsSeriousOffer").prop("disabled", true);
        }
        $("#Status").val(ParametrizationStatus.Update);
    }
    redirectIndex() {
        lockScreen();
        window.location = rootPath + "Home/Index";
        unlockScreen();
    }
    assignNameCoverage() {
        var selectedPeril = parseInt($("#selectPeril").UifSelect("getSelected"));
        var selectedInsuranceObject = parseInt($("#selectInsuranceObject").UifSelect("getSelected"));
        if (selectedPeril > 0 && selectedInsuranceObject > 0) {
            $("#inputCoverageName").val($("#selectInsuranceObject").UifSelect("getSelectedText") + " - " + $("#selectPeril").UifSelect("getSelectedText"));
            CoverageParametrization.focusoutInputCoverageName();
        }
    }
    static focusoutInputCoverageName() {
        if (glbBandNameCoveragePrint) {
            $("#ImpressionName").val($("#inputCoverageName").val());
        }
    }
    focusoutImpressionName() {
        if ($("#ImpressionName").val() !== $("#inputCoverageName").val()) {
            glbBandNameCoveragePrint = false;
        }
    }

    static disableFields(disabled) {
        $("#selectLineBusiness").UifSelect("disabled", disabled);
        $("#selectSubLineBusiness").UifSelect("disabled", disabled);
        $("#selectPeril").UifSelect("disabled", disabled);
        $("#selectInsuranceObject").UifSelect("disabled", disabled);

    }


    static GenerateFileToExportRequest() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Coverage/GenerateFileToExport',          
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    sendExcelCoverage(urlFile) {
        lockScreen();
        CoverageParametrization.GenerateFileToExportRequest().done(function (data) {
            if (data.success) {
                CoverageParametrization.downloadFile(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': 'Archivo descargado correctamente', 'autoclose': true });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                unlockScreen();

            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Se generó un error importando el excel', 'autoclose': true });
            unlockScreen();
        });

    }

    static downloadFile(urlFile) {
        window.open(urlFile);
        unlockScreen();
    }

    DeleteCoverage() {
        lockScreen();
        var coverage = $("#formCoverage").serializeObject();
        if (coverage.Id > 0) {
            $.UifDialog('confirm', { 'message': 'Desea Confirmar?' }, function (result) {
                if (result) {
                    coverage.Status = ParametrizationStatus.Delete;
                    request('Parametrization/Coverage/ExecuteOperations', JSON.stringify({ coverage: coverage }), 'POST', AppResources.ErrorCRUDCoverageCONNEX, CoverageParametrization.resultExecuteOpereations);
                }
            });
            unlockScreen();
        }
        else {
            //$.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDeletingRegister, 'autoclose': true });
            $.UifNotify('show', { 'type': 'info', 'message': 'Seleccione una cobertura para eliminar', 'autoclose': true });
            unlockScreen();
        }
    }
    static resultExecuteOpereations(data) {
        if (data === ParametrizationStatus.Delete) {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.CoverageEliminated, 'autoclose': true });
            unlockScreen();
        }
        else {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.CoverageSaved, 'autoclose': true });
            unlockScreen();
        }
        CoverageParametrization.ClearForm();
        unlockScreen();
    }
    SaveCoverage() {
        lockScreen();
        if ($("#formCoverage").valid()) {
            var coverage = $("#formCoverage").serializeObject();
            var prvCoverage = {};
            coverage.CompositionTypeId = $("#selectCompositionType").UifSelect("getSelected");
            if (coverage.IsAccMinPremium === "on") {
                coverage.IsAccMinPremium = true;
            }
            if (coverage.IsAssistance === "on" && coverage.IsAssistance === "off") {
                coverage.IsAssistance = false;
            }
            if (coverage.IsImpression === "on") {
                coverage.IsImpression = true;
            }
            if (coverage.IsPrimary === "on") {
                coverage.IsPrimary = true;
            }
            if (coverage.IsSeriousOffer === "on") {
                coverage.IsSeriousOffer = true;
            }
            coverage.Clauses = $("#listviewClausesAssing").UifListView('getData');
            coverage.Deductibles = $("#listviewDeductiblesAssing").UifListView('getData');
            coverage.DetailTypes = $("#listviewDetailTypesAssing").UifListView('getData');
            coverage.Homologation2G = null;

            //Assembly prvcoverage
            if (coverage.Status == ParametrizationStatus.Create) {
                prvCoverage.IsPost = $("#IsPost").is(":checked");
                prvCoverage.BeginDate = new Date();
            }
            else if (coverage.Status == ParametrizationStatus.Update && prvCoverage !== null) {
                if (prvCoverage.IsPost === false && $("#IsPost").is(":checked") === true) { //Update date only if activate Postcontractual on save
                    prvCoverage.IsPost = $("#IsPost").is(":checked");
                    prvCoverage.BeginDate = new Date();
                }
                else {
                    prvCoverage.IsPost = $("#IsPost").is(":checked");
                }
            }

            coverage.CoCoverageServiceModels = null;

            CoverageParametrization.executeOperations(coverage, prvCoverage).done(function (data) {
                if (data.success) {
                    CoverageParametrization.resultExecuteOpereations();
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    unlockScreen();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCRUDCoverageCONNEX, 'autoclose': true });
                unlockScreen();
            });



            //   request('Parametrization/Coverage/ExecuteOperations', JSON.stringify({ coverage: coverage, prvCoverageData: prvCoverage }), 'POST', AppResources.ErrorCRUDCoverageCONNEX, CoverageParametrization.resultExecuteOpereations);
            //    request('Parametrization/Coverage/GetLineBusiness', null, 'GET', AppResources.ErrorSearchLineBusinessCONNEX, CoverageParametrization.getLineBusiness);

        }
        else {
            unlockScreen();
        }
    }

    static executeOperations(coverage, prvCoverage) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Coverage/ExecuteOperations',
            data: JSON.stringify({ coverage: coverage, prvCoverageData: prvCoverage  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetPrvCoverage(coverageId, perilId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Coverage/GetPrvCoverage',
            data: JSON.stringify({ coverageId: coverageId, coverageNum: perilId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
