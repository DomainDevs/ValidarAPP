
/**
 * Variables Locales y Globales
 */

var glbDeductiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId = [];
var glbDeductiblesByCoverage = [];
var band = false;
var glbBeneficiaryTypeId;
var glbProductId = 0;

var auxModalDeductibleByCoverageListTemp = [];
$(document).ready(function () {
    bindEvents();
});

/**
 * Funcion para inicilizar la vista
 */
function getInitialState() {
    this.GetBeneficiaryTypes();
}


/**
 * Eventos de los controles de la clase
 */
function bindEvents() {
    $("#btnAcceptDeductiblesByCoverage").on("click", SaveDeductiblesByCoverage);
    $("#selectModalBeneficiaryTypeDC").on('itemSelected', LoadDeductyiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId);
    $("#btnCancelDeductiblesByCoverage").on("click", CloseDeductibles);
    $("#tableDeductibleBycoverage tbody").on("click", "tr td:first-child", Assign);
    $("#tableDeductibleBycoverage tbody").on("click", "tr td:nth-child(4)", AssignDefault);
}

function Assign() {
    var deductibleEdited = $('#tableDeductibleBycoverage').DataTable().row(this).data();
    if (deductibleEdited.IsSelected == true) {
        switch (deductibleEdited.StatusTypeService) {

            case 1:
                deductibleEdited.StatusTypeService = 4;
                break
            case 2:
                deductibleEdited.StatusTypeService = 1;
                break
            case 3:
                deductibleEdited.StatusTypeService = 4;
                break
            default:
                deductibleEdited.StatusTypeService = 1;
        }
        deductibleEdited.IsSelected = false;
        deductibleEdited.IsDefault = false;
    } else {
        switch (deductibleEdited.StatusTypeService) {

            case 1:
                deductibleEdited.StatusTypeService = 2;
                break
            case 4:
                deductibleEdited.StatusTypeService = 3;
                break
            default:
                deductibleEdited.StatusTypeService = 2;
        }
        deductibleEdited.IsSelected = true;
    }
    $('#tableDeductibleBycoverage').DataTable().row(this).data(deductibleEdited);
}
function AssignDefault() {
    var deductibleEdited = $('#tableDeductibleBycoverage').DataTable().row(this).data();
    var check = deductibleEdited.IsDefault;
    if (deductibleEdited.IsSelected == true) {
        var deductibleEditedTable = $('#tableDeductibleBycoverage').DataTable().rows().data();
        $.each(deductibleEditedTable, function (index, value) {
            if (value.IsDefault !== false && value.Id !== deductibleEdited.Id && value.IsSelected == true) {
                value.IsDefault = false;
                value.StatusTypeService === 1 ? value.StatusTypeService = 3 : null;
                $('#tableDeductibleBycoverage').DataTable().row(index).data(value);
            }      
        });
        if (check == true) {
            deductibleEdited.IsDefault = false;
        } else {
            deductibleEdited.IsDefault = true;
        }
        deductibleEdited.StatusTypeService === 1 ? deductibleEdited.StatusTypeService = 3 : null;
        $('#tableDeductibleBycoverage').DataTable().row(this).data(deductibleEdited);

    }
}

function loadControls() {
    if (band == true) {
        GetDeductiblesByPrefixId(glbPrefixId);
        GetBeneficiaryTypes();
        band = true;
    }
}

function CleanControls() {
    $("#listviewDeductiblesByCoverage").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#DeductiblesByPrefixTemplate", selectionType: 'multiple', height: 310 });
    $("#listviewDeductiblesByCoverageAssing").UifListView({ displayTemplate: "#DeductiblesByPrefixAsignedTemplate", selectionType: 'multiple', height: 310 });
}

/**
* @sumary Funcion para Obtener los deducibles por ramo
* @param {any} prefixid
*/
function GetDeductiblesByPrefixId(prefixid) {
    return DeductiblesByCoverageRequest.GetDeductiblesByPrefixId(prefixid,1).done(function (data) {

        if (data.success) {
            glbDeductiblesByPrefix = data.result;

            $.each(glbDeductiblesByPrefix, function (key, value) {
                $("#listviewDeductiblesByCoverage").UifListView("addItem", this);
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    });
}

function GetDeductiblesByProductIdByGroupCoverageBycoverageId(productId, coverGroupId, CoverageID, BeneficiaryTypeCd) {
    return DeductiblesByCoverageRequest.GetDeductiblesByProductIdByGroupCoverageBycoverageId(productId, coverGroupId, CoverageID, BeneficiaryTypeCd, $("#selectPrefixCommercial").UifSelect("getSelected"));
}

function GetDeductiblesByProductByGroupCoverageByCoverageByBeneficiaryType(productId, groupCoverageId, coverageId, beneficiaryTypeId) {

    return DeductiblesByCoverageRequest.GetDeductiblesByProductByGroupCoverageByCoverageByBeneficiaryType(productId, groupCoverageId, coverageId, beneficiaryTypeId).done(function (data) {

        if (data.success) {
            glbDeductiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId = data.result;
            if (glbDeductiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId.length > 0) {
                $.each(glbDeductiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId, function (key, value) {
                    $("#listviewDeductiblesByCoverageAssing").UifListView("addItem", this);
                });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    });  
}

function CopyAllDeductiblesByCoverage() {
    if ($('#selectModalBeneficiaryTypeDC').UifSelect("getSelected") != '') {
        var lstObjects = $("#listviewDeductiblesByCoverage").UifListView('getData');

        if (lstObjects.length > 0) {
            $.each($("#listviewDeductiblesByCoverageAssing").UifListView('getData'), function (key, value) {
                lstObjects.push(value)
            });
            lstObjects = $("#listviewPaymentPlanAssing").UifListView('getData').concat(lstObjects);
            $("#listviewDeductiblesByCoverageAssing").UifListView({ sourceData: lstObjects, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#DeductiblesByPrefixAsignedTemplate", selectionType: 'multiple', height: 310 });
            $("#listviewDeductiblesByCoverage").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#DeductiblesByPrefixTemplate", selectionType: 'multiple', height: 310 });
        }
    }
    else {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoSelectedBeneficiaryType, 'autoclose': true });
    }
}

function DeallocateAllDeductiblesByCoverage() {
    var lstObjects = $("#listviewDeductiblesByCoverageAssing").UifListView('getData');

    if (lstObjects.length > 0) {
        lstObjects = $("#listviewDeductiblesByCoverage").UifListView('getData').concat(lstObjects);
        $("#listviewDeductiblesByCoverage").UifListView({ sourceData: lstObjects, displayTemplate: "#DeductiblesByPrefixTemplate", selectionType: 'multiple', height: 310 });
        $("#listviewDeductiblesByCoverageAssing").UifListView({ displayTemplate: "#DeductiblesByPrefixAsignedTemplate", selectionType: 'multiple', height: 310 });
    }
}

function CopyDeductiblesByCoverage() {

    if ($('#selectModalBeneficiaryTypeDC').UifSelect("getSelected") != '') {
        if ($("#listviewDeductiblesByCoverage").UifListView('getData').length > 0) {
            $("#btnModalDeductiblesByCoverageAssign").prop("disabled", true);
            try {
                CopyDeductiblesByCoverageSelected($("#listviewDeductiblesByCoverage").UifListView('getSelected'))
            } catch (e) {
                alert(e);
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMovingPaymentPlan, 'autoclose': true });
            } finally {
                $("#btnModalDeductiblesByCoverageAssign").prop("disabled", false);
            }
        }
    }
    else {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NoSelectedBeneficiaryType, 'autoclose': true });
    }
}

function DeallocateDeductiblesByCoverage() {
    if ($("#listviewDeductiblesByCoverageAssing").UifListView('getData').length > 0) {
        DeallocateDeductiblesByCoverageSelect($("#listviewDeductiblesByCoverageAssing").UifListView('getSelected'))
    }
}

function CopyDeductiblesByCoverageSelected(data) {
    var ifExist = null;
    if (data != undefined && data.length > 0) {
        //var objectsNoAsign = $("#listviewDeductiblesByCoverage").UifListView('getData');
        var objectsAsign = $("#listviewDeductiblesByCoverageAssing").UifListView('getData');
        if ($("#listviewDeductiblesByCoverageAssing").UifListView('getData'))
            ifExist = $("#listviewDeductiblesByCoverageAssing").UifListView('getData').filter(function (item) {
                return item.Id == data[0].Id;
            });
        if (ifExist.length > 0) {
            //var objectsNoAsign = $("#listviewDeductiblesByCoverage").UifListView('getData');
            //var objectsAsign = $("#listviewDeductiblesByCoverageAssing").UifListView('getData');
            //$.each(data, function (index, data) {
            //    var findPayment = function (element, index, array) {
            //        return element.Id === data.Id
            //    }
            //    var index = $("#listviewDeductiblesByCoverage").UifListView("findIndex", findPayment);
            //    $("#listviewDeductiblesByCoverage").UifListView("deleteItem", index);
            //});
            //objectsAsign = objectsAsign.concat(data);
            //$("#listviewDeductiblesByCoverageAssing").UifListView({ sourceData: objectsAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#DeductiblesByPrefixAsignedTemplate", selectionType: 'multiple', height: 310 });
        }
        else {
            objectsAsign = objectsAsign.concat(data);
            $.each(data, function (index, data) {



                var findObject = function (element, index, array) {
                    return element.Id === data.Id
                }
                var index = $("#listviewDeductiblesByCoverage").UifListView("findIndex", findObject);
                $("#listviewDeductiblesByCoverage").UifListView("deleteItem", index);
            });

            $("#listviewDeductiblesByCoverageAssing").UifListView({ sourceData: objectsAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#DeductiblesByPrefixAsignedTemplate", selectionType: 'multiple', height: 310 });
        }
    }
}

function DeallocateDeductiblesByCoverageSelect(data) {
    var objectsNoAsign = $("#listviewDeductiblesByCoverage").UifListView('getData');
    $.each(data, function (index, data) {
        var findObject = function (element, index, array) {
            return element.Id === data.Id
        }
        var index = $("#listviewDeductiblesByCoverageAssing").UifListView("findIndex", findObject);
        $("#listviewDeductiblesByCoverageAssing").UifListView("deleteItem", index);
    });
    objectsNoAsign = objectsNoAsign.concat(data);
    $("#listviewDeductiblesByCoverage").UifListView({ sourceData: objectsNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#DeductiblesByPrefixTemplate", selectionType: 'multiple', height: 310 });
}

function ClearDeductiblesByCoverage() {

}

/**
 //* Asigna el Decucible por cobertura predeterminado
 */
function AssignDeductiblesByCoverageDefault() {
    var DeductibleSelected = $("#listviewDeductiblesByCoverageAssing").UifListView('getSelected');
    if (DeductibleSelected.length == 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectDeductibleByCoverage, 'autoclose': true });
    }
    else if (DeductibleSelected.length > 1) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectMaxiumDeductibles, 'autoclose': true });
    }
    else {
        DeductibleSelected = DeductibleSelected[0];
        var planData = $("#listviewDeductiblesByCoverageAssing").UifListView('getData');
        $.each(planData, function (key, value) {
            if (value.Id == DeductibleSelected.Id) {
                DeductibleSelected.IsDefaultDescription = "Predeterminado"
                DeductibleSelected.IsDefault = true;
                $('#listviewDeductiblesByCoverageAssing').UifListView("editItem", key, DeductibleSelected);
            } else {
                if (value.Currency.Id == DeductibleSelected.Currency.Id) {
                    delete value.IsDefaultDescription;
                    value.IsDefault = false;
                    $('#listviewDeductiblesByCoverageAssing').UifListView("editItem", key, value);
                }
            }
        });
    }

    $("#listviewDeductiblesByCoverageAssing .item").removeClass("selected");
}

function SaveDeductiblesByCoverage() {
    if ($('#selectModalBeneficiaryTypeDC').UifSelect("getSelected") != "" && $('#inputCoverageDC').val() != "") {

        if (modalDeductibleByCoverageListTemp.length > 0) {
            auxModalDeductibleByCoverageListTemp = jQuery.extend(true, {}, modalDeductibleByCoverageListTemp);
            modalDeductibleByCoverageListTemp = [0];
            modalDeductibleByCoverageListTemp = RemovItem(auxModalDeductibleByCoverageListTemp, CoverageId)
        }

        $.each(tempModalDeductibles, function (key, value) {            
            modalDeductibleByCoverageListTemp.push(this);            
        });
    }
    $('#selectModalAssignCoverageCoverage').data("Object").Coverage.DeductiblesCoverage = jQuery.extend(true, {}, tempModalDeductibles);
}

function RemovItem(modalDeductibleByCoverageListTemp, CoverageIdItem) {

    var DeductibleByCoverageListTemp = [];

    $.each(modalDeductibleByCoverageListTemp, function (key, value) {
        if (this.CoverageId != parseInt(CoverageIdItem)) {
            DeductibleByCoverageListTemp.push(this);
        }
    });
    return DeductibleByCoverageListTemp;
}

function GetValuesOfControls() {
    glbDeductiblesByCoverage = $("#listviewDeductiblesByCoverageAssing").UifListView('getData');
    DeductByCoverProductModel.GroupCoverageId = groupCoverageId;
    DeductByCoverProductModel.InsuredObjectId = InsuredObjectId;
    DeductByCoverProductModel.CoverageId = CoverageId;
    glbBeneficiaryTypeId = $('#selectModalBeneficiaryTypeDC').UifSelect("getSelected");
    DeductByCoverProductModel.BeneficiaryTypeId = glbBeneficiaryTypeId;
    DeductByCoverProductModel.ListDeductiblesByCoverage = glbDeductiblesByCoverage;
}

function GetBeneficiaryTypes() {

    return DeductiblesByCoverageRequest.GetBeneficiaryTypes().done(function (data) {

        if (data.success) {
            glbBeneficiarytypes = data.result;
            $("#selectModalBeneficiaryTypeDC").UifSelect({ sourceData: glbBeneficiarytypes });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }); 
}

function LoadDeductyiblesByProductIdByGroupCoverageIdByCoverageIdByBeneficiaryTypeId() {
    var filter = [];

    if ($("#selectModalBeneficiaryTypeDC").UifSelect("getSelected") != "") {

        $.each(tempModalDeductibles, function (key, value) {
            if (this.BeneficiaryTypeId == parseInt($("#selectModalBeneficiaryTypeDC").UifSelect("getSelected"))) {
                filter.push(this);
            }
        });
    }
    $("#tableDeductibleBycoverage").UifDataTable({ sourceData: filter });
}

function CloseDeductibles() {
    glbDeductiblesByCoverage = [];
    tempModalDeductibles = [];
    $("modalDeductiblesByCoverage").hide();
}

