$.ajaxSetup({ async: false });
// Tener informacion lista
var modalCoverageCoveragesList = []; // coverageId, coverageDescription, coverageLineBusiness, coverageSubLineBusiness, coverageInsuredObject
var modalCoverageCoveragesListTemp = [];

var modalDeductibleByCoverageList = [];
var modalDeductibleByCoverageListTemp = [];
var coverageDeleted = [];
//Indice para listview
var coverageIndex = null;
var groupCoverage;
var InsuredObject;
var Coverage;
var groupCoverageId;
var InsuredObjectId;
var CoverageId;
var glbPrefixId;

var glbDeductiblesByPrefix = [];
var tempModalDeductibles = [];

var glbBeneficiarytypes = [];
var validatePolicy;

$(document).ready(function () {
    $("#listModalAssignCoverage").UifListView({ source: null, displayTemplate: "#coverageTemplate", edit: true, delete: true, customEdit: true, deleteCallback: DeleteCoverage, height: 400, selectionType: 'single', drag: true });
    InitCoverage();
});

//EVENTOS DE LOS CONTROLES DEL FORMULARIO
function InitCoverage() {

    $("#btnModalTechnicalPlan").on("click", function () {
        GetTechnicalPlanByCoveredRiskTypeId($("#selectModalAssignCoverageRiskType").UifSelect("getSelected"));
        $("#modalTecnicalPlan").UifModal('showLocal', Resources.Language.LabelTechnicalPlan);
    });

    $("#acceptModalTecnicalPlan").on("click", function () {
        var data = $("#lsvTecnicalPlan").UifListView("getSelected");
        if (data.length > 0) {
            GetCoveragesByTechnicalPlanId(data[0].TechnicalPlanId, $("#selectModalAssignCoverageInsuredObject").UifSelect("getSelected"), 0);
        }
    });

    $("#chkModalAssignCoverageInitialIncludeCoverage,label[for=chkModalAssignCoverageInitialIncludeCoverage]").click(function () {
        if (!$("#chkModalAssignCoverageInitialIncludeCoverage").is(':checked')) {
            $("#chkModalAssignCoverageMandatoryCoverage").prop("checked", false);
        }
    });
    $("#chkModalAssignCoverageMandatoryCoverage,label[for=chkModalAssignCoverageMandatoryCoverage]").click(function () {
        if ($("#chkModalAssignCoverageMandatoryCoverage").is(':checked')) {
            $("#chkModalAssignCoverageInitialIncludeCoverage").prop("checked", true);
        }
    });

    $("#btnModalInsuredObjectAssignCoverage").on('click', function () {
        $('#btnModalAssignCoverageAllyCoverage').addClass("hidden");
        modalCoverageCoveragesListTemp = fnClonejQueryArray($("#selectModalInsuredObject").data("Object").Coverages);
        coverageDeleted = [];
        var insuredObject = $("#listModalInsuredObjectData").UifListView('getData')[insuredObjectIndex];
        if (insuredObject != null && insuredObjectIndex != null && insuredObjectIndex >= 0) {
            //Se asignan de esta forma mientras el framework libera funcionalidad del itemselected con todas las propiedades            
            var dataRiskTypeId = $('#selectModalInsuredObjectCoverageRiskType').UifSelect("getSelected");
            $("#selectModalAssignCoverageRiskType").UifSelect({ sourceData: [$("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelectedSource")], id: "Id", name: "Description", selectedId: dataRiskTypeId });
            var dataGroupCoverageId = $('#selectModalInsuredObjectGroupCoverage').UifSelect("getSelected");
            $("#selectModalAssignCoverageGroupCoverage").UifSelect({ sourceData: [$("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelectedSource")], id: "Id", name: "Description", selectedId: dataGroupCoverageId });
            $("#selectModalAssignCoverageInsuredObject").UifSelect({ sourceData: [insuredObject], id: "Id", name: "Description", selectedId: insuredObject.Id });
            $("#selectModalAssignCoverageGroupCoverage").prop("disabled", true);
            $("#selectModalAssignCoverageInsuredObject").prop("disabled", true);
            $("#selectModalAssignCoverageRiskType").prop("disabled", true);
            GetCoveragesByInsuredObjectId(insuredObject.Id, 0);
            GetCoveragesPrincipalByInsuredObjectId(insuredObject.Id, 0);
            LoadCoverageCoveragesList();
    
            ShowPanelsProduct(MenuProductType.Coverage);
            $("#btnModalDeductiblesByCoverage").prop("disabled", true);
            $("#listModalAssignCoverage .input-search.form-control.valid").val("");
            ClearValidation("#formCoverage");
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectInsuredObject, 'autoclose': true })
        }
    });

    $("#btnCancelAssignModalAssignCoverage").on('click', function () {
        $('#selectModalAssignCoverageCoverage').UifSelect("disabled", false);
        $('#btnModalAssignCoverageAllyCoverage').addClass("hidden");
        $("#selectModalAssignCoverageCoverage").UifSelect("setSelected", 0);
        $("#chkModalAssignCoverageInitialIncludeCoverage").prop('checked', false);
        $("#chkModalAssignCoverageMandatoryCoverage").prop('checked', false);
        $("#btnModalDeductiblesByCoverage").prop("disabled", true);
        $('#chkModalAssignCoverageIsSubLimit').prop('checked', false);
        $('#selectModalAssignCoverageCoverage').removeData("Object");
        $("#selectModalAssignCoveragePrincipalCoverage").UifSelect("setSelected", 0);
        $("#inputModalAssignCoverageSublimitCoverage").val("0");
        $("#listModalAssignCoverage .input-search.form-control.valid").val("");
        $("#listModalAssignCoverage .input-search.form-control.valid").keyup();
        ClearValidation("#formCoverage");
        coverageIndex = null;
    });

    //$("#btnModalDeductiblesByCoverage").on('click', function () {
    //    glbDeductiblesByCoverage = [];
    //    tempModalDeductibles = [];
    //    LoadDeductiblesControls();
    //    LoadDeductibleByControls();
    //    $("#modalDeductiblesByCoverage").UifModal("showLocal", Resources.Language.LabelDeductiblesByCoverage);
    //});

    $("#btnAssignModalAssignCoverage").on('click', function () {

        if (ValidateDataCoverage()) {
            AddCoverage();
        }
    });

    $('#listModalAssignCoverage').on('rowAdd', function (event) {
        ClearCoverage();
    });

    $('#listModalAssignCoverage').on('rowEdit', function (event, data, index) {

        if (data.CoverageAllied !== null && data.CoverageAllied.length != 0) {
            $('#btnModalAssignCoverageAllyCoverage').removeClass("hidden");
        }
        else {
            $('#btnModalAssignCoverageAllyCoverage').addClass("hidden");
        }

        coverageIndex = index;
        $('#selectModalAssignCoverageCoverage').data("Object", data);
        SetCoverage(data);
        $('#btnModalDeductiblesByCoverage').attr("disabled", false);

    });

    //grabar Coberturas
    $("#btnModalCoverageSave").on('click', function () {
        var listCoverages = $("#listModalAssignCoverage").UifListView('getData');
        if (listCoverages.length > 0) {
            $.each(listCoverages, function (index, item) {
                if (item.Coverage.Number != (index+1)) {
                    //item.Coverage.Number = index + 1;
                    if (item.Coverage.StatusTypeService === 1) {
                        item.Coverage.StatusTypeService = 3;
                    }                    
                }
            });
        }
        $("#selectModalInsuredObject").data("Object").Coverages = listCoverages;
        if (coverageDeleted.length > 0) {
            $.each(coverageDeleted, function (index, item) {
                $("#selectModalInsuredObject").data("Object").Coverages.push(item);
            });
        }
        var coverages = $("#listModalAssignCoverage").UifListView("getData");
        var InsuredObject = $("#listModalInsuredObjectData").UifListView("getData");
        $.each(InsuredObject, function (index, value) {
            var ObjectId = $("#selectModalAssignCoverageInsuredObject").val();
            if (InsuredObject[index].Id == ObjectId) {
                $.each(coverages, function (item, value) {
                    InsuredObject[index].Coverages.push(value);
                })
            }
        });
        modalCoverageCoveragesListTemp = [];
        coverageDeleted = [];
        Product.pendingChanges = true;
        ShowPanelsProduct(MenuProductType.InsuredObject);
    });

    //boton cerrar coberturas
    $("#btnModalCoverageClose").on('click', function () {
        //Se borra la variable global donde se encuentran los deducibles asignados "btnModalCoverageClose"
        //modalDeductibleByCoverageListTemp = [];
        ShowPanelsProduct(MenuProductType.InsuredObject);
    });

    $("#selectModalAssignCoverageCoverage").on('itemSelected', changeCoverage);
}

//funcion para el evento onchange de cobertura
function changeCoverage() {
    if ($("#selectModalAssignCoverageCoverage").UifSelect("getSelected") != "") {
        $('#btnModalDeductiblesByCoverage').attr("disabled", false);
    }
    else {
        $('#btnModalDeductiblesByCoverage').attr("disabled", true);
    }
}

//funcion para cargar los controles
function LoadDeductiblesControls() {
    groupCoverageId = $('#selectModalAssignCoverageGroupCoverage').UifSelect("getSelected");
    InsuredObjectId = $('#selectModalAssignCoverageInsuredObject').UifSelect("getSelected");
    CoverageId = $('#selectModalAssignCoverageCoverage').UifSelect("getSelected");
    groupCoverage = $('#selectModalAssignCoverageGroupCoverage').UifSelect("getSelectedText") + ' (' + groupCoverageId + ')';
    InsuredObject = $('#selectModalAssignCoverageInsuredObject').UifSelect("getSelectedText") + ' (' + InsuredObjectId + ')';
    Coverage = $('#selectModalAssignCoverageCoverage').UifSelect("getSelectedText") + ' (' + CoverageId + ')';
    $("#inputGroupCoverageDC").val(groupCoverage);
    $("#inputObjectInsuranceDC").val(InsuredObject);
    $("#inputCoverageDC").val(Coverage);
    band = true;
}

//funcion para cargar los deducibles por producto cobertura y grupo de cobertura
function LoadDeductibleByControls() {

    var filter = [];

    if (glbBeneficiarytypes.length == 0) {
        GetBeneficiaryTypes();
    }

    if ($('#selectModalAssignCoverageCoverage').data("Object") !== undefined) {
        if ($('#selectModalAssignCoverageCoverage').data("Object").Coverage.DeductiblesCoverage !== null) {
            glbDeductiblesByCoverage = fnClonejQueryArray($('#selectModalAssignCoverageCoverage').data("Object").Coverage.DeductiblesCoverage);
        }
    }
    else {
        $('#selectModalAssignCoverageCoverage').data("Object", GetCoverage());
    }

    if (glbDeductiblesByCoverage.length == 0) {
        glbBeneficiarytypes.forEach((itemBeneficiarytypes, index) => {
            GetDeductiblesByProductIdByGroupCoverageBycoverageId(Product.Id, groupCoverageId, CoverageId, itemBeneficiarytypes.Id).done(function (dataDeductible) {
                if (dataDeductible.success) {
                    if (dataDeductible.result != null) {

                        dataDeductible.result.forEach((itemResult) => {
                            glbDeductiblesByCoverage.push(itemResult);
                        });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        });
    }



    tempModalDeductibles = jQuery.extend(true, {}, glbDeductiblesByCoverage);

    $("#selectModalBeneficiaryTypeDC").UifSelect("setSelected", null);

    LoadDeductyiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId();
}

//Crea una copia por valor de un array
function fnClonejQueryArray(arr) {
    return jQuery.extend(true, [], arr);
}

//OBTENER COBERTURAS POR OBJETO DEL SEGURO
function GetCoveragesByInsuredObjectId(insuredObjectId, selectedId) {
    var controller = rootPath + 'Product/Product/GetCoveragesByInsuredObjectId?insuredObjectId=' + insuredObjectId;
    $("#selectModalAssignCoverageCoverage").prop("disabled", false);
    if (selectedId == 0) {
        $("#selectModalAssignCoverageCoverage").UifSelect({ source: controller });
    }
    else {
        $("#selectModalAssignCoverageCoverage").UifSelect({ source: controller, selectedId: selectedId });
    }
}

//AGREGAR O EDITAR COBERTURAS
function AddCoverage() {
    $("#formCoverage").validate();


    if ($("#formCoverage").valid()) {
        if ($('#selectModalAssignCoveragePrincipalCoverage').UifSelect('getSelected') != null && $('#selectModalAssignCoveragePrincipalCoverage').UifSelect('getSelected') != "" && $("#inputModalAssignCoverageSublimitCoverage").val() == "0") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMainCoverageSubLimit, 'autoclose': true });
        }
        else {
            if (coverageIndex == null) {
                if (!ValidateCoverage($("#selectModalAssignCoverageCoverage").UifSelect("getSelected"))) {
                    if ($('#selectModalAssignCoverageCoverage').data("Object") !== undefined) {
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsMandatory = $("#chkModalAssignCoverageMandatoryCoverage").prop('checked');
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsSelected = $("#chkModalAssignCoverageInitialIncludeCoverage").prop('checked');
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsSublimit = $("#chkModalAssignCoverageIsSubLimit").prop('checked');
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.MainCoverageId = ($("#selectModalAssignCoveragePrincipalCoverage").UifSelect('getSelected') != "") ? parseInt($("#selectModalAssignCoveragePrincipalCoverage").UifSelect('getSelected')) : null;
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.MainCoveragePercentage = $("#inputModalAssignCoverageSublimitCoverage").val();
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsPremiumMin = $("#chkModalAssignIsPremiumMin").prop('checked');
                        $('#selectModalAssignCoverageCoverage').data("Object").Coverage.NoCalculate = $("#chkModalAssignNoCalculate").prop('checked');
                        $("#listModalAssignCoverage").UifListView("addItem", $('#selectModalAssignCoverageCoverage').data("Object"));
                    }
                    else {
                        $("#listModalAssignCoverage").UifListView("addItem", GetCoverage());
                    }
                    ClearCoverage();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorCoverageAssigned, 'autoclose': true });
                }
            }
            else {
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsMandatory = $("#chkModalAssignCoverageMandatoryCoverage").prop('checked');
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsSelected = $("#chkModalAssignCoverageInitialIncludeCoverage").prop('checked');
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsSublimit = $("#chkModalAssignCoverageIsSubLimit").prop('checked');
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.IsPremiumMin = $("#chkModalAssignIsPremiumMin").prop('checked');
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.NoCalculate = $("#chkModalAssignNoCalculate").prop('checked');
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.MainCoverageId = ($("#selectModalAssignCoveragePrincipalCoverage").UifSelect('getSelected') != "") ? parseInt($("#selectModalAssignCoveragePrincipalCoverage").UifSelect('getSelected')) : null;
                $('#selectModalAssignCoverageCoverage').data("Object").Coverage.MainCoveragePercentage = $("#inputModalAssignCoverageSublimitCoverage").val();
                if ($('#selectModalAssignCoverageCoverage').data("Object").Coverage.StatusTypeService === 1) {
                    $('#selectModalAssignCoverageCoverage').data("Object").Coverage.StatusTypeService = 3;
                }

                $("#listModalAssignCoverage").UifListView("editItem", coverageIndex, $('#selectModalAssignCoverageCoverage').data("Object"));
 
                ClearCoverage();
            }
            $('#listModalAssignCoverage').find('.content').scrollTop(99999);
        }
    }
}

//VALIDAR SI LA COBERTURA YA EXISTE EN LA LISYVIEW
function ValidateCoverage(id) {
    var exists = false;

    $.each($("#listModalAssignCoverage").UifListView('getData'), function (index, value) {
        if (this.Id == id) {
            exists = true;
        }
    });
    return exists;
}

//OBJETER LA INFORMACION DEL OBJETO PARA AGREGAR A LA LISTVIEW
function GetCoverage() {
    var coverageData = GetCoverageProductByCoverageId($("#selectModalAssignCoverageCoverage").UifSelect("getSelected"));
    var lineDescription;
    var sublineDescription;

    if (coverageData.SubLineBusiness != null) {
        lineDescription = coverageData.SubLineBusiness.LineBusiness.Description;
        sublineDescription = coverageData.SubLineBusiness.Description;
    }
    var coverageAlliedData = null;
    if (coverageData.CoverageAllied.length > 0) {
        coverageAlliedData = [];
        $.each(coverageData.CoverageAllied, function (index, value) {
            var CoverageAllied = {
                DeductiblesCoverage: null,
                Description: value.Description,
                Id: value.Id,
                InsuredObjectId: null,
                InsuredObjectIdDescription: null,
                IsMandatory: null,
                IsSelected: null,
                IsSublimit: null,
                LineBusinessDescription: null,
                LineBusinessId: null,
                MainCoverageId: null,
                MainCoveragePercentage: null,
                Number: null,
                PosRuleSetId: null,
                RuleSetId: null,
                ScriptId: null,
                StatusTypeService: 1,
                SubLineBusinessDescription: null,
                SubLineBusinessId: null,
                SublimitPercentage: value.SublimitPercentage
            }
        });
    }
    var InsuredObject = { Id: $("#selectModalAssignCoverageInsuredObject").UifSelect("getSelected"), Description: $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText') };
    var Coverage = {
        Coverage: {
            DeductiblesCoverage: null,
            Description: $("#selectModalAssignCoverageCoverage").UifSelect('getSelectedText'),
            Id: parseInt($("#selectModalAssignCoverageCoverage").UifSelect("getSelected")),
            InitialInclude: null,
            InsuredObjectId: parseInt(InsuredObject.Id),
            InsuredObjectIdDescription: $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText'),
            IsMandatory: $("#chkModalAssignCoverageMandatoryCoverage").prop('checked'),
            IsSelected: $("#chkModalAssignCoverageInitialIncludeCoverage").prop('checked'),
            IsSublimit: $("#chkModalAssignCoverageIsSubLimit").prop('checked'),
            IsPremiumMin: $("#chkModalAssignIsPremiumMin").prop('checked'),
            NoCalculate: $("#chkModalAssignNoCalculate").prop('checked'),
            LineBusinessDescription: lineDescription,
            LineBusinessId: (coverageData.SubLineBusiness != null) ? coverageData.SubLineBusiness.LineBusiness.Id : null,
            MainCoverageId: ($('#selectModalAssignCoveragePrincipalCoverage').UifSelect('getSelected') != "") ? parseInt($('#selectModalAssignCoveragePrincipalCoverage').UifSelect('getSelected')) : null,
            MainCoveragePercentage: $('inputModalAssignCoverageSublimitCoverage').val(),
            Mandatory: null,
            Number: 0,
            PosRuleSetId: null,
            RuleSetId: null,
            ScriptId: null,
            StatusTypeService: 2,
            SubLineBusinessDescription: sublineDescription,
            SubLineBusinessId: (coverageData.SubLineBusiness != null) ? coverageData.SubLineBusiness.Id : null,
            SublimitPercentage: 0
        },
        CoverageAllied: coverageAlliedData,
        Description: $("#selectModalAssignCoverageCoverage").UifSelect('getSelectedText'),
        HaveAllyCoverage: coverageData.CoverageAllied.length > 0 ? Resources.Language.CoverageHaveAllyCoverage : Resources.Language.CoverageNotHaveAllyCoverage,
        Id: parseInt($("#selectModalAssignCoverageCoverage").UifSelect("getSelected")),
        InsuredObjectDescription: $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText'),
        LineBusinessDescription: lineDescription,
        SubLineBusinessDescription: sublineDescription
    };
    Coverage.Coverage.Number = $('#listModalAssignCoverage').UifListView('getData').length === 0 ? 1 : Math.max.apply(Math, $('#listModalAssignCoverage').UifListView('getData').map(function (o) { return o.Coverage.Number; })) + 1;
    if (Coverage.Coverage.IsSelected) {
        Coverage.InitialInclude = "SI";
    }
    else {
        Coverage.InitialInclude = "NO";
    }

    if (Coverage.Coverage.IsMandatory) {
        Coverage.Mandatory = "SI";
    }
    else {
        Coverage.Mandatory = "NO";
    }
    AssignCoverageCoverages(Coverage);
    return Coverage;
}

//OBTENER LA INFORMACION DE RAMO Y SUBRAMO DE LA COBERTURA Y COBERTURAS ALIADAS
function GetCoverageProductByCoverageId(coverageId) {
    var coverageResult;
    if (coverageId != "") {

        CoverageRequestT.GetCoverageProductByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                coverageResult = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchRisk, 'autoclose': true });
        });      
    }
    return coverageResult;
}
// ASIGNAR COBERTURAS
function AssignCoverageCoverages(coverage, id) {
    var coverageExistList = false;
    $.each(modalCoverageCoveragesListTemp, function (index, item) {
        if (item.Id == coverage.Id) {
            coverageExistList = true;
        }
    });
    if (!coverageExistList) {
        modalCoverageCoveragesListTemp.push(coverage);
    }

}

// ASIGNAR ORDEN A COBERTURAS
function AssignOrderCoverageCoverages(coverage, id) {

    modalCoverageCoveragesListTemp.push({
        Id: coverage.Id,
        Description: coverage.Description,
        IsSelected: coverage.IsSelected,
        IsMandatory: coverage.IsMandatory,
        InsuredObject: coverage.InsuredObject,
        LineBusinessDescription: coverage.LineBusinessDescription,
        SubLineBusinessDescription: coverage.SubLineBusinessDescription,
        SubLineBusiness: coverage.SubLineBusiness,
        CoverageAllied: coverage.CoverageAllied,
        Number: id
    });
}

//CARGAR COBERTURAS
function LoadCoverageCoveragesList() {
    $("#listModalAssignCoverage").UifListView('refresh');
    var riskTypeId = $('#selectModalAssignCoverageRiskType').UifSelect("getSelected");
    var groupCoverageId = $('#selectModalAssignCoverageGroupCoverage').UifSelect("getSelected");
    var InsuredObjectId = $('#selectModalAssignCoverageInsuredObject').UifSelect("getSelected");
    if (modalCoverageCoveragesListTemp.length > 0) {
        $.each(modalCoverageCoveragesListTemp, function (index, item) {
            if (item.Coverage.IsSelected) {
                item.Coverage.InitialInclude = "SI"
            }
            else {
                item.Coverage.InitialInclude = "NO"
            }
            if (item.Coverage.IsMandatory) {
                item.Coverage.Mandatory = "SI"
            }
            else {
                item.Coverage.Mandatory = "NO"
            }
            item.Id = item.Coverage.Id;
            item.Description = item.Coverage.Description;
            item.SubLineBusinessDescription = item.Coverage.SubLineBusinessDescription;
            item.LineBusinessDescription = item.Coverage.LineBusinessDescription;
            item.InsuredObjectDescription = $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText');
            if (item.CoverageAllied == null || item.CoverageAllied.length > 0) {
                item.HaveAllyCoverage = Resources.Language.CoverageHaveAllyCoverage;
            }
            else {
                item.HaveAllyCoverage = Resources.Language.CoverageNotHaveAllyCoverage; 
            }
            $("#listModalAssignCoverage").UifListView("addItem", item);
        });
    }
    
}

//LIMPIAR Datos cobertura
function ClearCoverage() {
    coverageIndex = null;
    $('#btnModalAssignCoverageAllyCoverage').addClass("hidden");
    $("#selectModalAssignCoverageCoverage").UifSelect("setSelected", null);
    $("#chkModalAssignCoverageInitialIncludeCoverage").prop('checked', false);
    $("#chkModalAssignCoverageMandatoryCoverage").prop('checked', false);
    $('#chkModalAssignCoverageIsSubLimit').prop('checked', false);
    $("#chkModalAssignIsPremiumMin").prop('checked', false);
    $("#chkModalAssignNoCalculate").prop('checked', false);
    $("#selectModalAssignCoverageCoverage").prop("disabled", false);
    $("#selectModalAssignCoveragePrincipalCoverage").UifSelect("setSelected", 0);
    $("#inputModalAssignCoverageSublimitCoverage").val("0");
    $('#selectModalAssignCoverageCoverage').removeData("Object");

}

//SETEAR LA INFORMACION PARA EDITAR la cobertura
function SetCoverage(coverage) {
    $("#selectModalAssignCoverageCoverage").prop("disabled", true);
    $("#selectModalAssignCoverageCoverage").UifSelect("setSelected", coverage.Id);
    $('#chkModalAssignCoverageInitialIncludeCoverage').prop('checked', coverage.Coverage.IsSelected);
    $('#chkModalAssignCoverageMandatoryCoverage').prop('checked', coverage.Coverage.IsMandatory);
    $('#chkModalAssignCoverageIsSubLimit').prop('checked', coverage.Coverage.IsSublimit);
    $("#chkModalAssignIsPremiumMin").prop('checked', coverage.Coverage.IsPremiumMin);
    $("#chkModalAssignNoCalculate").prop('checked', coverage.Coverage.NoCalculate);
    if (coverage.Coverage.MainCoverageId !== 0 && coverage.Coverage.MainCoverageId !== null) {
        $('#selectModalAssignCoveragePrincipalCoverage').UifSelect("setSelected", coverage.Coverage.MainCoverageId);
        $('#inputModalAssignCoverageSublimitCoverage').val((coverage.Coverage.MainCoveragePercentage !== null) ? coverage.Coverage.MainCoveragePercentage : 0);
    }
}

//ELIMINAR LA COBERTURA
var DeleteCoverage = function (deferred, data) {
    var validCoverage = true;
    if (Product.Id > 0) {
        validCoverage = ExistCoverageProduct(Product.Id, $("#selectModalAssignCoverageGroupCoverage").UifSelect("getSelected"), $("#selectModalAssignCoverageInsuredObject").UifSelect("getSelected"), data.Id);
    }
    else {
        validCoverage = false;
    }
    if (!validCoverage) {
        var ifExist = $('#listModalAssignCoverage').UifListView('getData').filter(function (itemCoverage) {
            return itemCoverage.Coverage.MainCoverageId == data.Id;
        });
        if (ifExist.length > 0) {
            $.UifNotify('show', { 'type': 'info', 'message': 'No se puede eliminar la cobertura por que ya está asignada a otra cobertura ', 'autoclose': true });
        } else {
            if (data.Coverage.StatusTypeService !== 2)
            {
                data.Coverage.StatusTypeService = 4;
                data.StatusTypeService = 4;
                coverageDeleted.push(data);
                $("#listModalAssignCoverage").UifListView("addItem", data);
               
            };
            deferred.resolve();
        }

    }
};

//Consulta si la cobertura existe en el producto y este ya esta en uso
function ExistCoverageProduct(productId, groupCoverageId, insuredObjectId, coverageId) {
    var result = true;

    CoverageRequestT.ExistCoverageProduct(productId, groupCoverageId, insuredObjectId, coverageId).done(function (data) {
        if (data.success) {
            if (data.result) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDeleteCoverage, 'autoclose': true });
            }
            else {
                result = data.result;
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetCoverage, 'autoclose': true });
    });

    
    return result;
}

// ASIGNAR GRUPOS DE COBERTURAS
function AssignCoverageGroupCoverage(groupCoverageIdSelected, groupCoverageDescriptionSelected, riskTypeIdSelected) {
    if (groupCoverageIdSelected > 0) {
        var groupCoverageExistList = false;

        $.each(modalCoverageRiskTypeListTemp, function (index, item) {
            if (this.RiskTypeId == riskTypeIdSelected) {
                var groupCoverages = this.GroupCoverages
                $.each(groupCoverages, function (index, item) {
                    if (this.Id == groupCoverageIdSelected) {
                        groupCoverageExistList = true;
                    }
                });
            }
        });

        if (!groupCoverageExistList) {
            $.each(modalCoverageRiskTypeListTemp, function (index, item) {
                if (this.RiskTypeId == riskTypeIdSelected) {
                    var groupCoverages = this.GroupCoverages
                    groupCoverages.push({
                        Id: groupCoverageIdSelected,
                        Description: groupCoverageDescriptionSelected,
                        CoveredRiskType: riskTypeIdSelected,
                        Coverages: modalCoverageCoveragesList
                    });
                }
            });
        } else {
            $.each(modalCoverageRiskTypeListTemp, function (index, item) {
                if (this.RiskTypeId == riskTypeIdSelected) {
                    var groupCoverages = this.GroupCoverages
                    $.each(groupCoverages, function (index, item) {
                        if (this.Id == groupCoverageIdSelected) {
                            groupCoverages[index].Coverages = modalCoverageCoveragesList
                        }
                    });
                }
            });
        }
    }
}

// VALIDAR QUE NO SE REPITA LA COBERTURA EN LA LISTA DE AGREGADAS
function validateCoverageAssigned(item) {
    if (item.Description === $("#selectModalAssignCoverageCoverage").UifSelect("getSelectedText"))
        return true;
    return false;
}

// OBTENER COBERTURAS PRINCIPALES POR OBJETO DEL SEGURO
function GetCoveragesPrincipalByInsuredObjectId(insuredObjectId, selectedId) {

    CoverageRequestT.GetCoveragesPrincipalByInsuredObjectId(insuredObjectId).success(function (response) {
        if (response.success) {
            $("#selectModalAssignCoveragePrincipalCoverage").prop("disabled", false);
            var assignCoverage = [];
            $.each(response.result, function (index, item) {
                var ifExist = $('#listModalAssignCoverage').UifListView('getData').filter(function (itemCoverage) {
                    return itemCoverage.Id == item.Id;
                });
                if (ifExist.length > 0) {
                    $.each(ifExist, function (index, itemIfExist) {
                        assignCoverage.push(itemIfExist);
                    });
                }
            });

            if (assignCoverage.length > 0) {
                $("#inputModalAssignCoverageSublimitCoverage").prop("disabled", false);
            }
            else {
                $("#inputModalAssignCoverageSublimitCoverage").prop("disabled", true);
            }

            if (selectedId == 0) {
                $("#selectModalAssignCoveragePrincipalCoverage").UifSelect({ sourceData: assignCoverage });
            }
            else {
                $("#selectModalAssignCoveragePrincipalCoverage").UifSelect({ sourceData: assignCoverage, selectedId: selectedId });
            }
        }

    }).error(function (data) {
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al ejecutar transacción' });
    });


  
   
}

// OBTENER PLANES TECNICOS
function GetTechnicalPlanByCoveredRiskTypeId(coveredRiskTypeId) {
    var insuredObjectId = $("#selectModalAssignCoverageInsuredObject").UifSelect("getSelected");

    $("#lsvTecnicalPlan").UifListView({
        source: rootPath + 'Product/Product/GetTechnicalPlanByCoveredRiskTypeId?coveredRiskTypeId=' + coveredRiskTypeId + "&insuredObjectId=" + insuredObjectId,
        displayTemplate: "#display-template-TecnicalPlan",
        height: 410,
        selectionType: 'single'
    });
}

// OBTENER COBERTURAS POR PLAN TECNICO
function GetCoveragesByTechnicalPlanId(technicalPlanId, insuredObjectId) {

    CoverageRequestT.GetCoveragesByTechnicalPlanId(technicalPlanId, insuredObjectId).done(function (data) {
        if (data.success) {

            var coverages = data.result;
            if (coverages != undefined) {
                $.each(coverages, function (index, item) {

                    var coverageData = coverages[index];
                    var lineDescription;
                    var sublineDescription;

                    if (coverageData.SubLineBusiness != null) {
                        lineDescription = coverageData.SubLineBusiness.LineBusiness.Description;
                        sublineDescription = coverageData.SubLineBusiness.Description;
                    }
                    var coverageAlliedData = null;
                    if (coverageData.CoverageAllied.length > 0) {
                        coverageAlliedData = [];
                        $.each(coverageData.CoverageAllied, function (index, value) {
                            var CoverageAllied = {
                                DeductiblesCoverage: null,
                                Description: value.Description,
                                Id: value.Id,
                                InsuredObjectId: null,
                                InsuredObjectIdDescription: null,
                                IsMandatory: null,
                                IsSelected: null,
                                IsSublimit: null,
                                LineBusinessDescription: null,
                                LineBusinessId: null,
                                MainCoverageId: null,
                                MainCoveragePercentage: null,
                                Number: null,
                                PosRuleSetId: null,
                                RuleSetId: null,
                                ScriptId: null,
                                StatusTypeService: 1,
                                SubLineBusinessDescription: null,
                                SubLineBusinessId: null,
                                SublimitPercentage: value.SublimitPercentage
                            }
                        });
                    }
                    var InsuredObject = { Id: $("#selectModalAssignCoverageInsuredObject").UifSelect("getSelected"), Description: $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText') };
                    var Coverage = {
                        Coverage: {
                            DeductiblesCoverage: null,
                            Description: coverageData.Description,
                            Id: coverageData.Id,
                            InitialInclude: null,
                            InsuredObjectId: parseInt(InsuredObject.Id),
                            InsuredObjectIdDescription: $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText'),
                            IsMandatory: true,
                            IsSelected: true,
                            IsSublimit: true,
                            LineBusinessDescription: lineDescription,
                            LineBusinessId: (coverageData.SubLineBusiness != null) ? coverageData.SubLineBusiness.LineBusiness.Id : null,
                            MainCoverageId: null,
                            MainCoveragePercentage: 0,
                            Mandatory: null,
                            Number: 0,
                            PosRuleSetId: null,
                            RuleSetId: null,
                            ScriptId: null,
                            StatusTypeService: 2,
                            SubLineBusinessDescription: sublineDescription,
                            SubLineBusinessId: (coverageData.SubLineBusiness != null) ? coverageData.SubLineBusiness.Id : null,
                            SublimitPercentage: 0
                        },
                        CoverageAllied: coverageAlliedData,
                        Description: coverageData.Description,
                        HaveAllyCoverage: coverageData.CoverageAllied.length > 0 ? Resources.Language.CoverageHaveAllyCoverage : Resources.Language.CoverageNotHaveAllyCoverage,
                        Id: coverageData.Id,
                        InsuredObjectDescription: $("#selectModalAssignCoverageInsuredObject").UifSelect('getSelectedText'),
                        LineBusinessDescription: lineDescription,
                        SubLineBusinessDescription: sublineDescription
                    };

                    if (Coverage.Coverage.IsSelected) {
                        Coverage.InitialInclude = "SI";
                    }
                    else {
                        Coverage.InitialInclude = "NO";
                    }

                    if (Coverage.Coverage.IsMandatory) {
                        Coverage.Mandatory = "SI";
                    }
                    else {
                        Coverage.Mandatory = "NO";
                    }
                    Coverage.Coverage.Number = $('#listModalAssignCoverage').UifListView('getData').length === 0 ? 1 : Math.max.apply(Math, $('#listModalAssignCoverage').UifListView('getData').map(function (o) { return o.Coverage.Number; }));
                    ValidatePolicyByProductId()
                    if (validatePolicy != null && validatePolicy == true) {
                        Coverage.Coverage.IsMandatory = false;
                        Coverage.Coverage.IsSelected = false;
                    }
                    var ifExist = $('#listModalAssignCoverage').UifListView('getData').filter(function (item) {
                        return item.Description.toUpperCase() == Coverage.Coverage.Description;
                    });
                    if (ifExist.length === 0) {
                        AssignCoverageCoverages(Coverage);
                        $("#listModalAssignCoverage").UifListView("addItem", Coverage);
                    }

                });
          
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetCoveragesByTechnicalPlan, 'autoclose': true });
    });

  
}

function ValidateDataCoverage() {
    var msj = "";

    if (coverageIndex == null) {
        var index = $("#listModalAssignCoverage").UifListView("findIndex", validateCoverageAssigned);
        if (index !== -1) {
            $("#selectModalAssignCoverageCoverage").UifSelect("setSelected", -1);
            msj = msj + Resources.Language.CoverageAlreadyExists;
        }
    }
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    return true;
}

function ValidatePolicyByProductId() {

    CoverageRequestT.ValidatePolicyByProductId(Product.Id, $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected"), $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")).success(function (response) {
        if (response.success) {
            validatePolicy = response.result;
        }
    }).error( function (data) {
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al ejecutar transacción' });
    });   
   
}



