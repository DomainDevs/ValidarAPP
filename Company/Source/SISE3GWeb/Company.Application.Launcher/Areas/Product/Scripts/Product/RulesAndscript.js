$.ajaxSetup({ async: false });
var ProductRules;
var prefixId;
var listRulesGeneral = [];
var listRulesRisk = [];
var listRulesCoverage = [];
var CurrentSearch;
var CurrentIdRules = 0;

$(document).ready(function () {
    EventRulesAndScripts();
});

function EventRulesAndScripts() {
    //Cargar Reglas y Guiones
    $("#btnRulesAndscripts").on("click", function () {
        lockScreen();
        setTimeout(function () {
            CurrentIdScripts = 0;
            CurrentIdRules = 0;
            ProductRules = [];
            listRulesGeneral = [];
            listRulesRisk = [];
            listRulesCoverage = [];
            //insuredObject_getCoveredRisk();
            //ObjectProduct.getCoveredRiskByProductId();
            getRiskTypes();
            prefixId = $("#selectPrefixCommercial").val();
            if (prefixId == null || prefixId == '') {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectPrefixTypeRisk, 'autoclose': true })
            } else {
                ProductRules = fnClonejQuery(Product);
                GetAllRuleSetsByLevelId();
                GetAllScriptByLevelId();
                GetRiskTypeRulesSelected("#listModalRiskType", "#selectModalRiskTypeRules");
                GetRiskTypeRulesSelected("#listModalRiskType", "#selectModalRiskTypeScript");
                GetPackageEnabled();
                disabledselectRules();
                ShowPanelsProduct(MenuProductType.Rule);
                SetProductCoveredRisksScripts();
            }
        }, 200);
    });

    //Acción validar y deshabilitar acciones en tabla coberturas (reglas)
    $('#tableNone').on('click', 'tr', function () {
        setTimeout(DisableSelectRulesCoverage(), 300);
    });



    //Accion al seleccionar el tipo de riesgo (reglas)
    $("#selectModalRiskTypeRules").on("itemSelected", function (event, selectedItem) {
        if (CurrentIdRules != 0 && ($("#inputModalRiskInitialRulesPackage").val() == "" || $("#inputModalRiskEndRulesPackage").val() == "") && ProductRules.ProductCoveredRisks != null) {
            $.each(ProductRules.ProductCoveredRisks, function (key, item) {
                if (item.Id == CurrentIdRules) {
                    if (item.PreRuleSetId != null && $("#inputModalRiskInitialRulesPackage").val() == "") {
                        item.PreRuleSetId = null;
                    }
                    if (item.RuleSetId != null && $("#inputModalRiskEndRulesPackage").val() == "") {
                        item.RuleSetId = null;
                    }
                    return;
                }
            });
        }
        ClearControlsRulesRisk();
        if (selectedItem.Id > 0) {
            if (listRulesRisk.length > 0) {
                $('#inputModalRiskInitialRulesPackage').UifInputSearch('disabled', false);
                $('#inputModalRiskEndRulesPackage').UifInputSearch('disabled', false);
            }
            SetProductCoveredRisks();
            DisableSelectRulesCoverage();

        }
        else {
            CurrentIdRules = 0;
        }
    });

    $('#formRulesAndScripts .dropdown-toggle').on('click', function () {
        var data = $('#tableNone').UifDataTable('getSelected');
        if (data == null) {
            $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', true);
            $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', true);
            $('#inputModalCoverageInitialRulesPackage').val('');
            $('#inputModalCoverageEndRulesPackage').val('');
            $("#btnModalAssignRules").attr("disabled", "disabled");
        }
    });

    //Accion al seleccionar todas las coberturas (reglas)
    $('#tableNone').on('selectAll', function (event) {
        $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', false);
        $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', false);
        $("#btnModalAssignRules").removeAttr("disabled");
        DisableSelectRulesCoverage();
    });

    //Accion al seleccionar un item de coberturas (reglas)
    $('#tableNone').on('rowSelected', function (event, data) {
        DisableSelectRulesCoverage();
        if (listRulesCoverage.length > 0) {
            var result = [];
            var resultEnd = [];
            $.each(listRulesCoverage, function (key, value) {
                if (value.RuleSetId === data.InitialBusinessRulesId) {
                    result.push(value);
                }
                if (value.RuleSetId === data.EndBusinessRulesId) {
                    resultEnd.push(value);
                }
            });
            if (result.length > 0) {
                $("#inputModalCoverageInitialRulesPackage").data("Object", result[0]);
                $("#inputModalCoverageInitialRulesPackage").val(result[0].Description);
            }
            if (resultEnd.length > 0) {
                $("#inputModalCoverageEndRulesPackage").data("Object", resultEnd[0]);
                $("#inputModalCoverageEndRulesPackage").val(resultEnd[0].Description);
            }
        }
    });

    //Accion del boton asignar reglas
    $('#btnModalAssignRules').on('click', function () {
        var data = $('#tableNone').UifDataTable('getSelected');
        if (data != null) {
            AssignRulesCoverages();
        }
        //else {
        $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', true);
        $('#inputModalCoverageInitialRulesPackage').val('');
        $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', true);
        $('#inputModalCoverageEndRulesPackage').val('');
        $("#btnModalAssignRules").attr("disabled", "disabled");
        //}
    });

    //Accion al guardar la informacion de la modal
    $("#btnModalRulesAndscriptsSave").on("click", function () {

        if ($('#inputModalGeneralInitialRulesPackage').val() == "") {
            ProductRules.PreRuleSetId = null;
        }
        if ($('#inputModalGeneralEndRulesPackage').val() == "") {
            ProductRules.RuleSetId = null;
        }
        if ($('#inputModalGeneralInitialScriptPackage').val() == "") {
            ProductRules.ScriptId = null;
        }

        if (ProductRules.ProductCoveredRisks != null && $("#selectModalRiskTypeScript").val() != "" && $("#inputModalRiskInitialScriptPackage").val() == "") {
            $.each(ProductRules.ProductCoveredRisks, function (key, item) {
                if (item.Id == $("#selectModalRiskTypeScript").val()) {
                    if (item.ScriptId != null) {
                        item.ScriptId = null;
                    }
                    return;
                }
            });
        }

        if ($("#selectModalRiskTypeRules").val() != "" && ($("#inputModalRiskInitialRulesPackage").val() == "" || $("#inputModalRiskEndRulesPackage").val() == "") && ProductRules.ProductCoveredRisks != null) {
            $.each(ProductRules.ProductCoveredRisks, function (key, item) {
                if (item.Id == $("#selectModalRiskTypeRules").val()) {
                    if (item.PreRuleSetId != null && $("#inputModalRiskInitialRulesPackage").val() == "") {
                        item.PreRuleSetId = null;
                    }
                    if (item.RuleSetId != null && $("#inputModalRiskEndRulesPackage").val() == "") {
                        item.RuleSetId = null;
                    }
                    if (item.StatusTypeService === 1) {
                        item.StatusTypeService = 3;
                    }
                    return;
                }
            });
        }

        Product = fnClonejQuery(ProductRules);
        ProductRules = [];
        Product.pendingChanges = true;
        ShowPanelsProduct(MenuProductType.Product);
        listRulesGeneral = [];
        listRulesRisk = [];
        listRulesCoverage = [];
        listScriptsGeneral = [];
        listScriptRisk = [];
        listScriptCoverage = [];
        CurrentIdRules = 0;
        CurrentIdScripts = 0;
    });

    //Accion al cerrar el modal
    $("#btnModalRulesAndscriptsClose").on("click", function () {
        ProductRules = [];
        ShowPanelsProduct(MenuProductType.Product);
        listRulesGeneral = [];
        listRulesRisk = [];
        listRulesCoverage = [];
        listScriptsGeneral = [];
        listScriptRisk = [];
        listScriptCoverage = [];
        CurrentIdRules = 0;
        CurrentIdScripts = 0;
    });

    //Selección de busqueda
    $('#tableResultsRulesScripts tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        $("#" + CurrentSearch).data("Object", $('#tableResultsRulesScripts').DataTable().row($(this)).data());
        $("#" + CurrentSearch).val($('#tableResultsRulesScripts').DataTable().row($(this)).data().Description);
        switch (CurrentSearch) {
            case 'inputModalGeneralInitialRulesPackage':
                ProductRules.PreRuleSetId = $('#tableResultsRulesScripts').DataTable().row($(this)).data().RuleSetId;
                if (ProductRules.StatusTypeService === 1) {
                    ProductRules.StatusTypeService = 3;
                }
                break;
            case 'inputModalGeneralEndRulesPackage':
                ProductRules.RuleSetId = $('#tableResultsRulesScripts').DataTable().row($(this)).data().RuleSetId;
                if (ProductRules.StatusTypeService === 1) {
                    ProductRules.StatusTypeService = 3;
                }
                break;
            case 'inputModalRiskInitialRulesPackage':
                if (ProductRules.ProductCoveredRisks != null) {
                    for (var i = 0; i < ProductRules.ProductCoveredRisks.length; i++) {
                        if (ProductRules.ProductCoveredRisks[i].Id == $("#selectModalRiskTypeRules").UifSelect("getSelected") && ProductRules.ProductCoveredRisks[i].StatusTypeService !== 4) {
                            ProductRules.ProductCoveredRisks[i].PreRuleSetId = $('#tableResultsRulesScripts').DataTable().row($(this)).data().RuleSetId;
                            if (ProductRules.ProductCoveredRisks[i].StatusTypeService === 1) {
                                ProductRules.ProductCoveredRisks[i].StatusTypeService = 3;
                            }
                            break;
                        }
                    }
                }
                break;
            case 'inputModalRiskEndRulesPackage':
                if (ProductRules.ProductCoveredRisks != null) {
                    for (var i = 0; i < ProductRules.ProductCoveredRisks.length; i++) {
                        if (ProductRules.ProductCoveredRisks[i].Id == $("#selectModalRiskTypeRules").val() && ProductRules.ProductCoveredRisks[i].StatusTypeService !== 4) {
                            ProductRules.ProductCoveredRisks[i].RuleSetId = $('#tableResultsRulesScripts').DataTable().row($(this)).data().RuleSetId;
                            if (ProductRules.ProductCoveredRisks[i].StatusTypeService === 1) {
                                ProductRules.ProductCoveredRisks[i].StatusTypeService = 3;
                            }
                            break;
                        }
                    }
                }
                break;
        }
        $('#modalRulesAndScriptsResults').UifModal("hide");
    });

    //Selección de busqueda
    $('#tableResultsScripts tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        $("#" + CurrentSearch).data("Object", $('#tableResultsScripts').DataTable().row($(this)).data());
        $("#" + CurrentSearch).val($('#tableResultsScripts').DataTable().row($(this)).data().Description);
        switch (CurrentSearch) {
            case 'inputModalGeneralInitialScriptPackage':
                ProductRules.ScriptId = $('#tableResultsScripts').DataTable().row($(this)).data().ScriptId;
                if (ProductRules.StatusTypeService === 1) {
                    ProductRules.StatusTypeService = 3;
                }
                break;
            case 'inputModalRiskInitialScriptPackage':
                if (ProductRules.ProductCoveredRisks != null) {
                    for (var i = 0; i < ProductRules.ProductCoveredRisks.length; i++) {
                        if (ProductRules.ProductCoveredRisks[i].Id == $("#selectModalRiskTypeRules").val() && ProductRules.ProductCoveredRisks[i].StatusTypeService !== 4) {
                            ProductRules.ProductCoveredRisks[i].ScriptId = $('#tableResultsScripts').DataTable().row($(this)).data().ScriptId;
                            if (ProductRules.ProductCoveredRisks[i].StatusTypeService === 1) {
                                ProductRules.ProductCoveredRisks[i].StatusTypeService = 3;
                            }
                            break;
                        }
                    }
                }
                break;
        }
        $('#modalScriptsResults').UifModal("hide");
    });

    //Filtro reglas general inicial
    $("#inputModalGeneralInitialRulesPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalGeneralInitialRulesPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro reglas general final
    $("#inputModalGeneralEndRulesPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalGeneralEndRulesPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro reglas riesgo inicial
    $("#inputModalRiskInitialRulesPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalRiskInitialRulesPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro reglas riesgo final
    $("#inputModalRiskEndRulesPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalRiskEndRulesPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro reglas cobertura inicial
    $("#inputModalCoverageInitialRulesPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalCoverageInitialRulesPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro reglas cobertura final
    $("#inputModalCoverageEndRulesPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalCoverageEndRulesPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro guiones general
    $("#inputModalGeneralInitialScriptPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalGeneralInitialScriptPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro guiones riesgo
    $("#inputModalRiskInitialScriptPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalRiskInitialScriptPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });

    //Filtro guiones cobertura
    $("#inputModalCoverageInitialScriptPackage").on('buttonClick', function () {
        CurrentSearch = 'inputModalCoverageInitialScriptPackage';
        GetGeneralInitialFilterByDescription(CurrentSearch, $("#" + CurrentSearch).val().trim());
    });
}

function ClearControlsRulesRisk() {
    $('#inputModalRiskInitialRulesPackage').UifInputSearch('disabled', true);
    $('#inputModalRiskEndRulesPackage').UifInputSearch('disabled', true);
    $('#inputModalRiskInitialRulesPackage').val('');
    $('#inputModalRiskEndRulesPackage').val('');
    $("#tableNone").UifDataTable("clear");
}

//Obtiene todas las reglas por nivel 
var GetAllRuleSetsByLevelId = function () {

    RulesAndscriptRequestT.GetAllRuleSetsByLevelId().done(function (data) {
        unlockScreen();
        if (data.success) {
            $.each(data.result, function (index, value) {
                if (!value.IsEvent) {
                    if (value.LevelId == Level.General) {
                        listRulesGeneral.push(this);
                    } else if (value.LevelId == Level.Risk) {
                        listRulesRisk.push(this);
                    } else if (value.LevelId == Level.Coverage) {
                        listRulesCoverage.push(this);
                    }
                }
            });
            if (listRulesGeneral.length > 0) {
                $('#inputModalGeneralInitialRulesPackage').UifInputSearch('disabled', false);
                $('#inputModalGeneralEndRulesPackage').UifInputSearch('disabled', false);
            }
            else {
                $('#inputModalGeneralInitialRulesPackage').UifInputSearch('disabled', true);
                $('#inputModalGeneralInitialRulesPackage').val('');
                $('#inputModalGeneralEndRulesPackage').UifInputSearch('disabled', true);
                $('#inputModalGeneralEndRulesPackage').val('');
            }
            if (listRulesRisk.length > 0) {
                $('#inputModalRiskInitialRulesPackage').UifInputSearch('disabled', false);
                $('#inputModalRiskEndRulesPackage').UifInputSearch('disabled', false);
            }
            else {
                $('#inputModalRiskInitialRulesPackage').UifInputSearch('disabled', true);
                $('#inputModalRiskInitialRulesPackage').val('');
                $('#inputModalRiskEndRulesPackage').UifInputSearch('disabled', true);
                $('#inputModalRiskEndRulesPackage').val('');
            }
            if (listRulesCoverage.length > 0) {
                $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', false);
                $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', false);
            }
            else {
                $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', true);
                $('#inputModalCoverageInitialRulesPackage').val('');
                $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', true);
                $('#inputModalCoverageEndRulesPackage').val('');
            }
            if (ProductRules.PreRuleSetId != null) {
                var filter = [];
                $.each(listRulesGeneral, function (key, value) {
                    if (value.RuleSetId == ProductRules.PreRuleSetId) {
                        filter.push(value);
                    }
                });
                if (filter.length > 0) {
                    $("#inputModalGeneralInitialRulesPackage").data("Object", filter[0]);
                    $("#inputModalGeneralInitialRulesPackage").val(filter[0].Description);
                }
            }
            if (ProductRules.RuleSetId != null) {
                var filter = [];
                $.each(listRulesGeneral, function (key, value) {
                    if (value.RuleSetId == ProductRules.RuleSetId) {
                        filter.push(value);
                    }
                });
                if (filter.length > 0) {
                    $("#inputModalGeneralEndRulesPackage").data("Object", filter[0]);
                    $("#inputModalGeneralEndRulesPackage").val(filter[0].Description);
                }
            }
        }
    });
}

var GetPackageEnabled = function () {

    RulesAndscriptRequestT.GetPackageEnabled().done(function (data) {
        if (data.success) {
            if (data.result != 0) {
                $("#inputModalModuleDescriptionRules, #inputModalModuleDescriptionScripts").val(data.result[0].Description);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoModulesEnabled, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'error', 'message': Resources.Language.ErrorConsultingModule, 'autoclose': true });
        }
    });
}

//Setea las reglas a nivel de riesgo (Script)
function SetProductCoveredRisks() {
    var IdProductCoveredRisks = $("#selectModalRiskTypeRules").val();
    if (ProductRules.ProductCoveredRisks != null) {
        $.each(ProductRules.ProductCoveredRisks, function (key, item) {
            if (item.Id == IdProductCoveredRisks) {
                if (item.PreRuleSetId != null) {
                    var filter = [];
                    $.each(listRulesRisk, function (key, value) {
                        if (value.RuleSetId == item.PreRuleSetId) {
                            filter.push(value);
                        }
                    });
                    if (filter.length > 0) {
                        $("#inputModalRiskInitialRulesPackage").data("Object", filter[0]);
                        $("#inputModalRiskInitialRulesPackage").val(filter[0].Description);
                    }
                }
                if (item.RuleSetId != null) {
                    var filter = [];
                    $.each(listRulesRisk, function (key, value) {
                        if (value.RuleSetId == item.RuleSetId) {
                            filter.push(value);
                        }
                    });
                    if (filter.length > 0) {
                        $("#inputModalRiskEndRulesPackage").data("Object", filter[0]);
                        $("#inputModalRiskEndRulesPackage").val(filter[0].Description);
                    }
                }
                return;
            }
        });
        CurrentIdRules = IdProductCoveredRisks;
        LoadTableCoverages();
    }
}

//Setea los guiones a nivel de riesgo
function SetProductCoveredRisksScripts() {
    var IdProductCoveredRisks = $("#selectModalRiskTypeScript").val();
    if (ProductRules.ProductCoveredRisks != null) {
        $.each(ProductRules.ProductCoveredRisks, function (key, item) {
            if (item.Id == IdProductCoveredRisks) {
                if (item.ScriptId != null) {
                    var filter = [];
                    $.each(listScriptRisk, function (key, value) {
                        if (value.ScriptId == item.ScriptId) {
                            filter.push(value);
                        }
                    });
                    if (filter.length > 0) {
                        $("#inputModalRiskInitialScriptPackage").data("Object", filter[0]);
                        $("#inputModalRiskInitialScriptPackage").val(filter[0].Description);
                    }
                }
                return;
            }
        });
        CurrentIdScripts = IdProductCoveredRisks;
        LoadTableCoveragesScripts();
    }
}

//Llena la tabla de coberturas (reglas)
function LoadTableCoverages() {
    var listCoverages = [];
    $("#tableNone").UifDataTable('clear');

    if (ProductRules.ProductCoveredRisks != null) {
        $.each(ProductRules.ProductCoveredRisks, function (keyCoveredRisks, CoveredRisks) {
            if (CoveredRisks.GroupCoverages != null) {
                $.each(CoveredRisks.GroupCoverages, function (keyGroupCoverages, GroupCoverage) {
                    if (GroupCoverage.RiskTypeId == $("#selectModalRiskTypeRules").val() && GroupCoverage.StatusTypeService !== 4) {
                        if (GroupCoverage.InsuredObjects != null) {
                            $.each(GroupCoverage.InsuredObjects, function (keyInsuredObjects, InsuredObject) {
                                if (InsuredObject.StatusTypeService !== 4) {
                                    if (InsuredObject.Coverages != null) {
                                        $.each(InsuredObject.Coverages, function (keyCoverages, Coverage) {
                                            if (Coverage.Coverage.StatusTypeService !== 4) {
                                                var RuleSetName = "";
                                                var PosRuleSetName = "";
                                                if (Coverage.Coverage.RuleSetId != null) {
                                                    if (listRulesCoverage.length > 0) {
                                                        var filter = [];
                                                        $.each(listRulesCoverage, function (key, value) {
                                                            if (value.RuleSetId == Coverage.Coverage.RuleSetId) {
                                                                filter.push(value);
                                                            }
                                                        });
                                                        if (filter.length > 0) {
                                                            RuleSetName = filter[0].Description;
                                                        }
                                                    }
                                                }
                                                if (Coverage.Coverage.PosRuleSetId != null) {
                                                    if (listRulesCoverage.length > 0) {
                                                        var filter = [];
                                                        $.each(listRulesCoverage, function (key, value) {
                                                            if (value.RuleSetId == Coverage.Coverage.PosRuleSetId) {
                                                                filter.push(value);
                                                            }
                                                        });
                                                        if (filter.length > 0) {
                                                            PosRuleSetName = filter[0].Description;
                                                        }
                                                    }
                                                }

                                                listCoverages.push({
                                                    GroupCoverageId: GroupCoverage.Id,
                                                    CoverageId: Coverage.Coverage.Id,
                                                    GroupCoverage: GroupCoverage.Description,
                                                    LineBusiness: Coverage.Coverage.LineBusinessDescription == null ? '' : Coverage.Coverage.LineBusinessDescription,
                                                    SubLineBusiness: Coverage.Coverage.SubLineBusinessDescription == null ? '' : Coverage.Coverage.SubLineBusinessDescription,
                                                    Amparo: Coverage.Coverage.Description,
                                                    ObjetoSeguro: InsuredObject.Description,
                                                    InitialBusinessRulesId: Coverage.Coverage.RuleSetId == null ? '' : Coverage.Coverage.RuleSetId,
                                                    InitialBusinessRules: RuleSetName,
                                                    EndBusinessRulesId: Coverage.Coverage.PosRuleSetId == null ? '' : Coverage.Coverage.PosRuleSetId,
                                                    EndBusinessRules: PosRuleSetName
                                                });
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                });
            }
        });
    }

    $("#tableNone").UifDataTable({ sourceData: listCoverages });
}

//Desabilita los combos de reglas de las coberturas
function DisableSelectRulesCoverage() {
    var data = $('#tableNone').UifDataTable('getSelected');
    if (data != null) {
        $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', false);
        $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', false);
        $("#btnModalAssignRules").removeAttr("disabled");
    } else {
        $('#inputModalCoverageInitialRulesPackage').UifInputSearch('disabled', true);
        $('#inputModalCoverageEndRulesPackage').UifInputSearch('disabled', true);

        $('#inputModalCoverageInitialRulesPackage').val('');
        $('#inputModalCoverageEndRulesPackage').val('');
        $("#btnModalAssignRules").attr("disabled", "disabled");
    }
}

//Asigna las reglas a las coberturas
function AssignRulesCoverages() {
    var dataSelected = $('#tableNone').UifDataTable('getSelected');

    if (dataSelected != null) {
        if (ProductRules.ProductCoveredRisks != null) {
            $.each(ProductRules.ProductCoveredRisks, function (keyCoveredRisk, CoveredRisk) {
                if (CoveredRisk.Id == $("#selectModalRiskTypeRules").val()) {
                    if (CoveredRisk.GroupCoverages != null) {
                        $.each(CoveredRisk.GroupCoverages, function (keyGroupCoverages, GroupCoverage) {
                            if (GroupCoverage.RiskTypeId == $("#selectModalRiskTypeRules").val() && GroupCoverage.StatusTypeService !== 4) {

                                if (GroupCoverage.InsuredObjects != null) {
                                    $.each(GroupCoverage.InsuredObjects, function (keyInsuredObjects, InsuredObject) {
                                        if (InsuredObject.Coverages != null && InsuredObject.StatusTypeService !== 4) {
                                            $.each(InsuredObject.Coverages, function (keyCoverages, Coverage) {
                                                $.each(dataSelected, function (keyCoverageSelected, CoverageSelected) {
                                                    if (CoverageSelected.GroupCoverageId == GroupCoverage.Id) {
                                                        if (CoverageSelected.CoverageId == Coverage.Coverage.Id) {
                                                            var RuleSetId = null;
                                                            var PosRuleSetId = null;

                                                            if ($("#inputModalCoverageInitialRulesPackage").val() != '' && $("#inputModalCoverageInitialRulesPackage").val() == $("#inputModalCoverageInitialRulesPackage").data("Object").Description) {

                                                                RuleSetId = $("#inputModalCoverageInitialRulesPackage").data("Object").RuleSetId;
                                                            }
                                                            if ($("#inputModalCoverageEndRulesPackage").val() != '' && $("#inputModalCoverageEndRulesPackage").val() == $("#inputModalCoverageEndRulesPackage").data("Object").Description) {
                                                                PosRuleSetId = $("#inputModalCoverageEndRulesPackage").data("Object").RuleSetId;
                                                            }
                                                            ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.RuleSetId = RuleSetId;
                                                            ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.PosRuleSetId = PosRuleSetId;
                                                            if (ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.StatusTypeService === 1) {
                                                                ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.StatusTypeService = 3;
                                                            }
                                                        }
                                                    }
                                                });
                                            })
                                        }
                                    });
                                }
                            }
                        });
                    }
                    return;
                }
            });
        }
        LoadTableCoverages();
    }

}

//Crea una copia por valor de un objeto
function fnClonejQuery(arr) {
    return jQuery.extend(true, {}, arr);
}

//Cargar tipos de Riesgo
function GetRiskTypeRulesSelected(selectTable, selectName) {
    var items = $(selectTable).UifListView("getData");
    if (items != null && items.length > 0) {
        var data = items;
        $(selectName).prop("disabled", false);
        $(selectName).UifSelect({ sourceData: data, id: "RiskTypeId", name: "Description" });
    }
    else {
        $(selectName).prop("disabled", true);
    }
}

function disabledselectRules() {

    //desabilita el combo si  no tiene datos (reglas)
    if ($("#selectModalRiskTypeRules option[value!='']").length == 0) {
        $("#selectModalRiskTypeRules").attr("disabled", "disabled");
    }
    //selecciona el primer item (si solo hay uno) (reglas)
    else if ($("#selectModalRiskTypeRules option[value!='']").length == 1) {
        var firtItem = $($("#selectModalRiskTypeRules option[value!='']")).val();
        $("#selectModalRiskTypeRules").UifSelect("setSelected", firtItem);
        $("#selectModalRiskTypeRules").change();
    }
    else {
        ClearControlsRulesRisk();
    }

    //desabilita el combo si  no tiene datos (guiones)
    if ($("#selectModalRiskTypeScript option[value!='']").length == 0) {
        $("#selectModalRiskTypeScript").attr("disabled", "disabled");
    }
    //selecciona el primer item (si solo hay uno) (guiones)
    else if ($("#selectModalRiskTypeScript option[value!='']").length == 1) {
        var firtItem = $($("#selectModalRiskTypeScript option[value!='']")).val();
        $("#selectModalRiskTypeScript").UifSelect("setSelected", firtItem);
        $("#selectModalRiskTypeScript").change();
    }
}

//Obtiene los resultados de las busquedas en el formulario de reglas y guiones
function GetGeneralInitialFilterByDescription(input, description) {
    if (description.length < 3 && !/^([0-9])*$/.test(description)) {
        $.UifNotify('show', {
            'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
        })
    }
    else {
        var result = [];
        switch (input) {
            case 'inputModalGeneralInitialRulesPackage':
                $.each(listRulesGeneral, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalGeneralEndRulesPackage':
                $.each(listRulesGeneral, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalRiskInitialRulesPackage':
                $.each(listRulesRisk, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalRiskEndRulesPackage':
                $.each(listRulesRisk, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalCoverageInitialRulesPackage':
                $.each(listRulesCoverage, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalCoverageEndRulesPackage':
                $.each(listRulesCoverage, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalGeneralInitialScriptPackage':
                $.each(listScriptsGeneral, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalRiskInitialScriptPackage':
                $.each(listScriptRisk, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
            case 'inputModalCoverageInitialScriptPackage':
                $.each(listScriptCoverage, function (key, value) {
                    if (/^([0-9])*$/.test(description)) {
                        if (value.RuleSetId == description) {
                            result.push(value);
                        }
                    }
                    else if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        result.push(value);
                    }
                });
                break;
        }

        if (result.length == 0) {
            $.UifNotify('show',
                {
                    'type': 'danger', 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true
                })
        }
        else {
            if (result.length == 1) {
                $("#" + input).data("Object", result[0]);
                $("#" + input).val(result[0].Description);
                switch (input) {
                    case 'inputModalGeneralInitialRulesPackage':
                        ProductRules.PreRuleSetId = result[0].RuleSetId;
                        if (ProductRules.StatusTypeService === 1) {
                            ProductRules.StatusTypeService = 3;
                        }
                        break;
                    case 'inputModalGeneralEndRulesPackage':
                        ProductRules.RuleSetId = result[0].RuleSetId;
                        if (ProductRules.StatusTypeService === 1) {
                            ProductRules.StatusTypeService = 3;
                        }
                        break;
                    case 'inputModalRiskInitialRulesPackage':
                        if (ProductRules.ProductCoveredRisks != null) {
                            for (var i = 0; i < ProductRules.ProductCoveredRisks.length; i++) {
                                if (ProductRules.ProductCoveredRisks[i].Id == $("#selectModalRiskTypeRules").UifSelect("getSelected") && ProductRules.ProductCoveredRisks[i].StatusTypeService !== 4) {
                                    ProductRules.ProductCoveredRisks[i].PreRuleSetId = result[0].RuleSetId;
                                    if (ProductRules.ProductCoveredRisks[i].StatusTypeService === 1) {
                                        ProductRules.ProductCoveredRisks[i].StatusTypeService = 3;
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case 'inputModalRiskEndRulesPackage':
                        if (ProductRules.ProductCoveredRisks != null) {
                            for (var i = 0; i < ProductRules.ProductCoveredRisks.length; i++) {
                                if (ProductRules.ProductCoveredRisks[i].Id == $("#selectModalRiskTypeRules").val()) {
                                    ProductRules.ProductCoveredRisks[i].RuleSetId = result[0].RuleSetId;
                                    if (ProductRules.ProductCoveredRisks[i].StatusTypeService === 1) {
                                        ProductRules.ProductCoveredRisks[i].StatusTypeService = 3;
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case 'inputModalGeneralInitialScriptPackage':
                        ProductRules.ScriptId = result[0].ScriptId;
                        if (ProductRules.StatusTypeService === 1) {
                            ProductRules.StatusTypeService = 3;
                        }
                        break;
                    case 'inputModalRiskInitialScriptPackage':
                        if (ProductRules.ProductCoveredRisks != null) {
                            for (var i = 0; i < ProductRules.ProductCoveredRisks.length; i++) {
                                if (ProductRules.ProductCoveredRisks[i].Id == $("#selectModalRiskTypeRules").val() && ProductRules.ProductCoveredRisks[i].StatusTypeService !== 4) {
                                    ProductRules.ProductCoveredRisks[i].ScriptId = result[0].ScriptId;
                                    if (ProductRules.ProductCoveredRisks[i].StatusTypeService === 1) {
                                        ProductRules.ProductCoveredRisks[i].StatusTypeService = 3;
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                }

            }
            else {
                if (input == 'inputModalGeneralInitialScriptPackage' || input == 'inputModalRiskInitialScriptPackage' || input == 'inputModalCoverageInitialScriptPackage') {
                    $('#tableResultsScripts').UifDataTable({ stateSave: false });
                    $('#tableResultsScripts').UifDataTable('clear');
                    $('#tableResultsScripts').UifDataTable('addRow', result);
                } else {
                    $('#tableResultsRulesScripts').UifDataTable({ stateSave: false });
                    $('#tableResultsRulesScripts').UifDataTable('clear');
                    $('#tableResultsRulesScripts').UifDataTable('addRow', result);
                }

                switch (input) {
                    case 'inputModalGeneralInitialRulesPackage':
                        $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelInitialRulesPackage);
                        break;
                    case 'inputModalGeneralEndRulesPackage':
                        $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelEndRulesPackage);
                        break;
                    case 'inputModalRiskInitialRulesPackage':
                        $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelInitialRulesPackage);
                        break;
                    case 'inputModalRiskEndRulesPackage':
                        $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelEndRulesPackage);
                        break;
                    case 'inputModalCoverageInitialRulesPackage':
                        $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelEndRulesPackage);
                        break;
                    case 'inputModalCoverageEndRulesPackage':
                        $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelEndRulesPackage);
                        break;
                    case 'inputModalGeneralInitialScriptPackage':
                        $('#modalScriptsResults').UifModal('showLocal', Resources.Language.LabelScript + ' ' + Resources.Language.LabelGeneralLevel);
                        break;
                    case 'inputModalRiskInitialScriptPackage':
                        $('#modalScriptsResults').UifModal('showLocal', Resources.Language.LabelScript + ' ' + Resources.Language.LabelRiskLevel);
                        break;
                    case 'inputModalCoverageInitialScriptPackage':
                        $('#modalScriptsResults').UifModal('showLocal', Resources.Language.LabelScript + ' ' + Resources.Language.LabelCoverageLevel);
                        break;
                }
            }
        }
    }
}