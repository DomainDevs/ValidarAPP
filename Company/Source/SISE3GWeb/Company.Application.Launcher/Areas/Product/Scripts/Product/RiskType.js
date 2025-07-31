$.ajaxSetup({ async: false });
var currentRiskTypeIndex = -1;
var heightListView = 256;
var glbRiskTypeDeleted = [];
$(document).ready(function () {
    EventRiskType();
});
function EventRiskType() {
    //Tipo de riesgo
    $("#btnRiskType").on("click", function () {
        if (ValidateProductRiskType()) {
            LoadRiskType();
            getRiskTypes();
            ClearRiskType();
            $("#modalRiskType").UifModal('showLocal', Resources.Language.MessageRiskType);
        }
    });

    $("#btnModalAddRiskType").on("click", function () {
        if (ValidateRiskType()) {
            AddRiskType();
            ClearRiskType();
        }
    });


    $("#btnModalRiskTypeSave").on("click", function () {
        SaveRiskType();
        setRiskType();
        HidePanelsProduct(MenuProductType.RiskType);
    });

    $("#btnModalPolicyTypeClose").on("click", function () {
        HidePanelsProduct(MenuProductType.RiskType);
    });
    $('#listModalRiskType').on('rowEdit', function (event, data, position) {
        currentRiskTypeIndex = position;
        bindRiskType(data);
    });
}

function LoadRiskType() {
    var controller = rootPath + 'Product/Product/GetRiskTypeByPrefixId?prefixId=' + $("#selectPrefixCommercial").UifSelect("getSelected");
    $("#selectModalRiskType").UifSelect({ source: controller });
}
function AddRiskType() {
    var maxRiskCuantity = $('#inputModalMaximumNumberRisk').val().trim() == "" ? 0 : $('#inputModalMaximumNumberRisk').val().trim();
    if ($("#inputModalMaximumNumberRisk").data("source") !== null) {
        var rikTypeAdd = { Id: $('#selectModalRiskType').UifSelect("getSelected"), Description: $("#selectModalRiskType").UifSelect("getSelectedText"), MaxRiskQuantity: maxRiskCuantity };
        rikTypeAdd = jQuery.extend(true, $("#inputModalMaximumNumberRisk").data("source"), rikTypeAdd);
    }
    else {
        var rikTypeAdd = {
            Id: parseInt($('#selectModalRiskType').UifSelect("getSelected")),
            Description: $("#selectModalRiskType").UifSelect("getSelectedText"),
            MaxRiskQuantity: parseInt(maxRiskCuantity),
            CoveredRiskType: null,
            GroupCoverages: null,
            PreRuleSetId: null,
            RuleSetId: null,
            ScriptId: null,
            StatusTypeService: 2,
            SubCoveredRiskType: null
        };
    }
    CreateRiskType(rikTypeAdd);
}
function CreateRiskType(rikTypeAdd) {
    if (currentRiskTypeIndex == -1) {
        rikTypeAdd.StatusTypeService = 2;
        if (rikTypeAdd.Id) {
            $('#listModalRiskType').UifListView('addItem', rikTypeAdd);
        }       
    }
    else {
        if (rikTypeAdd.StatusTypeService === 1 || rikTypeAdd.StatusTypeService === 3) {
            rikTypeAdd.StatusTypeService = 3;
        }
        else {
            rikTypeAdd.StatusTypeService = 2;
        }
        $('#listModalRiskType').UifListView('editItem', currentRiskTypeIndex, rikTypeAdd);
    }
}
function SaveRiskType() {
    SendRiskType();
}
function SendRiskType() {
    if ($('#listModalRiskType').UifListView("getData").length > 0) {
        Product.RiskTypes = $('#listModalRiskType').UifListView("getData");
        if (glbRiskTypeDeleted.length > 0) {
            $.each(glbRiskTypeDeleted, function (i, item) {
                Product.RiskTypes.push(item);
            });
            glbRiskTypeDeleted = [];
        }
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorRiskTypeEmpty, 'autoclose': true })
    }
}
function bindRiskType(vm) {
    $("#selectModalRiskType").UifSelect("setSelected", vm.Id);
    $("#formRiskType").find("#inputModalMaximumNumberRisk").val(vm.MaxRiskQuantity);
    $("#inputModalMaximumNumberRisk").data("source", vm);
    $("#selectModalRiskType").prop("disabled", true);
}
function ValidateRiskType() {
    var msj = "";
    if ($("#selectModalRiskType").UifSelect("getSelected") == null || $("#selectModalRiskType").UifSelect("getSelected") == "") {
        msj = Resources.Language.ErrorSelectTypeRisk + "<br>"
    }
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    else if (DuplicateRiskType()) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorRiskTypeDuplicate, 'autoclose': true })
        return false;
    }
    return true;
}
function ClearRiskType() {
    currentRiskTypeIndex = -1;
    $("#selectModalRiskType").prop("disabled", false);
    $("#selectModalRiskType").UifSelect("setSelected", "");
    $("#inputModalMaximumNumberRisk").val('');
    $("#inputModalMaximumNumberRisk").val('');
    $("#inputModalMaximumNumberRisk").data("source", null);
}

function DuplicateRiskType() {
    var duplicate = false;
    $.each($('#listModalRiskType').UifListView('getData'), function (i, item) {
        if (item.Id == $("#selectModalRiskType").UifSelect("getSelected") && i != currentRiskTypeIndex) {
            duplicate = true;
            return false;
        }
    });
    return duplicate;
}
function ValidateProductRiskType() {
    var msj = "";
    if ($("#selectPrefixCommercial").UifSelect("getSelected") == null || $("#selectPrefixCommercial").UifSelect("getSelected") == "") {
        msj = Resources.Language.ErrorPrefix + "<br>"
    }
    //if ($("#selectRiskType").UifSelect("getSelected") == null || $("#selectRiskType").UifSelect("getSelected") == "") {
    //    msj = msj + Resources.Language.ErrorSelectTypeRisk + "<br>"
    //}
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    return true;
}
function getRiskTypes() {
    CleanObjectRiskType();
    var MaxRisk = $("#inputMaximumNumberRisk").val().trim() == "" ? 1 : $("#inputMaximumNumberRisk").val().trim()
    var rikTypeAdd = {
        Id: parseInt($('#selectRiskType').UifSelect("getSelected")),
        Description: $("#selectRiskType").UifSelect("getSelectedText"),
        MaxRiskQuantity: parseInt(MaxRisk),
        CoveredRiskType: null,
        GroupCoverages: null,
        PreRuleSetId: null,
        RuleSetId: null,
        ScriptId: null,
        StatusTypeService: 2,
        SubCoveredRiskType: null
    };
    var exists = false;
    if (Product.RiskTypes == null) {
        Product.RiskTypes = [];
    }
    if (Product.RiskTypes != null && Product.RiskTypes.length > 0) {
        var deletedRiskTypes = Product.RiskTypes.filter(function (item) {
            return item.StatusTypeService === 4;
        });
        if (deletedRiskTypes != null && deletedRiskTypes.length > 0) {
            $.each(deletedRiskTypes, function (i, item) {
                glbRiskTypeDeleted.push(item);
            });
        }
        var ifExist = Product.RiskTypes.filter(function (item) {
            return item.StatusTypeService !== 4;
        });
        if (ifExist != null && ifExist.length > 0) {
            RiskType(ifExist);
            $.each($("#listModalRiskType").UifListView("getData"), function (key, value) {
                if (value.Id == $("#selectRiskType").UifSelect("getSelected")) {
                    rikTypeAdd = jQuery.extend({}, value, rikTypeAdd);
                    currentRiskTypeIndex = key;
                    exists = true;
                    return false;
                }
            });
        }
        if (!exists) {
            currentRiskTypeIndex = -1;
            //CreateRiskType(rikTypeAdd)
        }
        //else {
        //    currentRiskTypeIndex = -1;
        //    CreateRiskType(rikTypeAdd)
        //}

    }
    else {
        currentRiskTypeIndex = -1;
        CreateRiskType(rikTypeAdd);
    }
}
//seccion set
function setRiskType() {
    $.each($("#listModalRiskType").UifListView('getData'), function (i, item) {
        if (item.Id == $("#selectRiskType").UifSelect("getSelected")) {
            $('#inputMaximumNumberRisk').val(item.MaxRiskQuantity);
        }
    });
    if ($("#listModalRiskType").UifListView('getData').length > 0) {
        $.each($("#listModalRiskType").UifListView('getData'), function (i, item) {
            var validate = true;
            $.each(Product.ProductCoveredRisks, function (x, itemX) {
                if (item.Id == itemX.Id) {
                    validate = false;
                }
            });
            if (validate) {
                var rikTypeAdd = {
                    Id: item.Id,
                    Description: item.Description,
                    MaxRiskQuantity: item.MaxRiskQuantity,
                    CoveredRiskType: null,
                    GroupCoverages: null,
                    PreRuleSetId: null,
                    RuleSetId: null,
                    ScriptId: null,
                    StatusTypeService: 2,
                    SubCoveredRiskType: null
                };
                Product.ProductCoveredRisks.push(rikTypeAdd);
            }
        });
        //Agregar riesgos al select
        if (Product.RiskTypes.length > 0) {
            if (Product.RiskTypes.length == 1) {
                $("#selectRiskType").UifSelect({ 'sourceData': Product.RiskTypes, 'selectedId': Product.RiskTypes[0].Id });
            }
            else {
                $("#selectRiskType").UifSelect({ 'sourceData': Product.RiskTypes});
            }
            $("#selectRiskType").prop("disabled", false);
        }
        else {
            $("#selectRiskType").prop("disabled", true);
        }
    } else {
        $("#selectRiskType").prop("disabled", false);
    }
}
function setRiskTypeMain() {
    if (Product.ProductCoveredRisks == null || Product.ProductCoveredRisks.length <= 1) {
        Product.ProductCoveredRisks = [];
        var MaxRisk = $("#inputMaximumNumberRisk").val().trim() == "" ? 1 : $("#inputMaximumNumberRisk").val().trim();
        var rikTypeAdd = {
            Id: $('#selectRiskType').UifSelect("getSelected"),
            Description: $("#selectRiskType").UifSelect("getSelectedText"),
            MaxRiskQuantity: MaxRisk,
            CoveredRiskType: null,
            GroupCoverages: null,
            PreRuleSetId: null,
            RuleSetId: null,
            ScriptId: null,
            StatusTypeService: 2,
            SubCoveredRiskType: null
        };
        Product.ProductCoveredRisks.push(rikTypeAdd);
    }
}
function CleanObjectRiskType() {
    $("#listModalRiskType").UifListView({
        source: null,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: Product.IsUse? !Product.IsUse: true,
        displayTemplate: "#RiskTypeTemplate",
        height: heightListView,
        deleteCallback: deleteCallbackRiskType
    });
}
function RiskType(riskType) {
    $("#listModalRiskType").UifListView({
        sourceData: riskType,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: Product.IsUse ? !Product.IsUse : true,
        displayTemplate: "#RiskTypeTemplate",
        height: heightListView,
        deleteCallback: deleteCallbackRiskType
    });
}

var deleteCallbackRiskType = function (deferred, data) {

    var dataList = $("#listModalRiskType").UifListView("getData");

    if (dataList.length == 1) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorTypeOfRisk, 'autoclose': true })
    }
    else {
        if (data.StatusTypeService === 1 || data.StatusTypeService === 3) {
            data.StatusTypeService = 4;
            glbRiskTypeDeleted.push(data);
        }
        deferred.resolve();
    }
}