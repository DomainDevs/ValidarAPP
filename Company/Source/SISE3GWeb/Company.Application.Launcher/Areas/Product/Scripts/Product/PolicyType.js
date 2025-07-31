$.ajaxSetup({ async: false });
var currentPolicyTypeIndex = -1;
var glbPolicyTypeDeleted = [];
$(document).ready(function () {
    EventsPolicyType();
});
function EventsPolicyType() {
    $("#selectPolicyType").prop("disabled", true);
    //Tipo de póliza
    $("#btnPolicyType").on("click", function () {
        if (validatePolicyType()) {
            LoadPolicyType();
            $('#listModalPolicyType').UifListView('clear');
            getPolicyTypes();
            ClearPolicyType();
            $("#modalPolicyType").UifModal('showLocal', Resources.Language.LabelPolicyType);
        }
    });
    $("#btnModalPolicyTypeSave").click(function () {
        if (ValidatePolicyTypes()) {
            SavePolicyTypes();
            setMainPolicyType();
            Product.pendingChanges = true;
            HidePanelsProduct(MenuProductType.PolicyType);
        }
    });
    $("#btnModalPolicyTypeClose").click(function () {
        HidePanelsProduct(MenuProductType.PolicyType);
    });

    $("#selectModalPolicyType").on("itemSelected", function (event, selectedItem) {
        $("#chkModalDefaultPolicyType").prop('checked', false);
    });
    $("#btnModalAddPolicyType").click(function () {
        if (ValidatePolicyType()) {
            AddPolicyType();
            ClearPolicyType();
        }
    });
    $("#btnCancelPolicyType").click(function () {
            ClearPolicyType();
    });
    $('#listModalPolicyType').on('rowEdit', function (event, data, position) {
        currentPolicyTypeIndex = position;
        bindPolicyType(data);
    });
}
//seccion funciones
function bindPolicyType(vm) {
    $("#formPolicyType").find("#selectModalPolicyType").UifSelect("setSelected", vm.Id);
    if (vm.IsDefault) {
        $('#chkModalDefaultPolicyType').prop('checked', true);
    }
    else {
        $('#chkModalDefaultPolicyType').prop('checked', false);
    }
    $("#chkModalDefaultPolicyType").data("source", vm);
    $("#selectModalPolicyType").prop("disabled", true);
}
function LoadPolicyType() {
    var productId = Product.Id;
    var controller = rootPath + 'Product/Product/GetPolicyTypesByPrefixId?prefixId=' + $("#selectPrefixCommercial").UifSelect('getSelected');
    $("#selectModalPolicyType").UifSelect({ source: controller });
}

function AddPolicyType() {

    var policyTypeAdd = {
        Id: parseInt($('#selectModalPolicyType').UifSelect("getSelected")),
        Description: $("#selectModalPolicyType").UifSelect("getSelectedText"),
        IsDefault: $('#chkModalDefaultPolicyType').is(':checked')
    };
    CreatePolicyType(policyTypeAdd);
}

var SavePolicyTypes = function () {
    //Product.PolicyTypes = [];
    SendPolicyTypes();
}

function getPolicyTypes() {
    var PoliciTypeAdd = { Id: parseInt($('#selectPolicyType').UifSelect("getSelected")), Description: $("#selectPolicyType").UifSelect("getSelectedText"), IsDefault: true, PrefixId: $("#selectPrefixCommercial").UifSelect('getSelected')};
    var exists = false;
    glbPolicyTypeDeleted = [];
    if (Product.PolicyTypes == null) {
        Product.PolicyTypes = [];
    }
    if (Product.PolicyTypes != null && Product.PolicyTypes.length > 0) {
        var deletedPolicyType  = Product.PolicyTypes.filter(function (item) {
            return item.StatusTypeService === 4;
        });
        if (deletedPolicyType != null && deletedPolicyType.length > 0) {
            $.each(deletedPolicyType, function (i, item) {
                glbPolicyTypeDeleted.push(item);
            });
        }
        var ifExist = Product.PolicyTypes.filter(function (item) {
            return item.StatusTypeService !== 4;
        });
        if (ifExist != null && ifExist.length > 0) {
            PolicyType(ifExist);
            $.each($("#listModalPolicyType").UifListView("getData"), function (key, value) {
                if (value.Id == $("#selectPolicyType").UifSelect("getSelected")) {
                    PoliciTypeAdd = jQuery.extend({}, value, PoliciTypeAdd);
                    currentPolicyTypeIndex = key;
                    exists = true;
                    return false;
                }
            });
        }
        if (!exists) {
            currentPolicyTypeIndex = -1;
        }
    }
    //else {
    //    currentPolicyTypeIndex = -1;
    //    CreatePolicyType(PoliciTypeAdd);
    //}

}

function ClearPolicyType() {
    currentPolicyTypeIndex = -1;
    $("#selectModalPolicyType").prop("disabled", false);
    $("#selectModalPolicyType").UifSelect("setSelected", "");
    $("#chkModalDefaultPolicyType").prop("checked", false);
}

function CreatePolicyType(policiTypeAdd) {
    if (policiTypeAdd.IsDefault) {
        $.each($('#listModalPolicyType').UifListView("getData"), function (index, item) {
            if (item.IsDefault == true) {
                if (item.StatusTypeService == 1) {
                    item.StatusTypeService = 3;
                }
                item.IsDefault = false;
                $('#listModalPolicyType').UifListView('editItem', index, item);
            }
        });
    }

    if (currentPolicyTypeIndex == -1) {
        policiTypeAdd.StatusTypeService = 2;
        $('#listModalPolicyType').UifListView('addItem', policiTypeAdd);
    }
    else {
        var currentPolicy = $('#listModalPolicyType').UifListView("getData")[currentPolicyTypeIndex];
        if (currentPolicy.StatusTypeService === 1 || currentPolicy.StatusTypeService === 3) {
            policiTypeAdd.StatusTypeService = 3;
        }
        else {
            policiTypeAdd.StatusTypeService = currentPolicy.StatusTypeService;
        }
        $('#listModalPolicyType').UifListView('editItem', currentPolicyTypeIndex, policiTypeAdd);
    }
}
function ValidatePolicyType() {
    var msj = "";
    if ($("#selectModalPolicyType").UifSelect("getSelected") == null || $("#selectModalPolicyType").UifSelect("getSelected") == "") {
        msj = Resources.Language.ErrorSelectPoliciType + "<br>"
    }
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    else if (DuplicatePolicyType()) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPolicyTypeDuplicate, 'autoclose': true })
        return false;
    }
    else if (DuplicateMainPolicyType()) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPolicyTypeDefaultDuplicate, 'autoclose': true })
        return false;
    }
    return true;
}
function DuplicatePolicyType() {
    var duplicate = false;
    $.each($("#listModalPolicyType").UifListView('getData'), function (i, item) {
        if (item.Id == $("#selectModalPolicyType").UifSelect("getSelected") && i != currentPolicyTypeIndex) {
            duplicate = true;
            return false;
        }
    });
    return duplicate;
}
//setMainPolicyType()
function setPolicyTypeMain() {
    if (Product.PolicyTypes == null || Product.PolicyTypes.length <= 1) {
        Product.PolicyTypes = [];
        var policyTypeAdd = {
            Id: parseInt($('#selectPolicyType').UifSelect("getSelected")),
            Description: $("#selectPolicyType").UifSelect("getSelectedText"),
            IsDefault: true,
            PrefixId: parseInt($("#selectPrefixCommercial").UifSelect('getSelected'))
        };
        Product.PolicyTypes.push(policyTypeAdd);
    }
}
function DuplicateMainPolicyType() {
    var duplicate = false;
    var isPolicyType = $('#chkModalDefaultPolicyType').is(':checked');
    if (isPolicyType) {
        $.each($("#listModalPolicyType").UifListView('getData'), function (i, item) {
            if (item.IsDefault == isPolicyType && i != currentPolicyTypeIndex) {
                item.IsDefault = false;
                $("#listModalPolicyType").UifListView("editItem", i, item);
            }
        });
    }
    return duplicate;
}
function ValidatePolicyTypes() {
    var validate = false;
    var countPolicyType = 0;
    $.each($("#listModalPolicyType").UifListView('getData'), function (i, item) {
        if (item.IsDefault == true) {
            countPolicyType++;
        };
    });
    if (countPolicyType != 1) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPolicyTypeMain, 'autoclose': true })
    }
    else {
        validate = true;
    }
    return validate;
}

function setMainPolicyType() {
    $.each($("#listModalPolicyType").UifListView('getData'), function (i, item) {
        if (item.IsDefault == true) {
            GetPolicyTypesByPrefixId($("#selectPrefixCommercial").UifSelect('getSelected'));
            $("#selectPolicyType").UifSelect("setSelected", item.Id);
        }
    });

    if ($("#listModalPolicyType").UifListView('getData').length > 1) {
        $("#selectPolicyType").prop("disabled", true);
    } else {
        $("#selectPolicyType").prop("disabled", false);
    }
}
function SendPolicyTypes() {
    if ($('#listModalPolicyType').UifListView("getData").length > 0) {
        Product.PolicyTypes = $('#listModalPolicyType').UifListView("getData");
        if (glbPolicyTypeDeleted.length > 0) {
            $.each(glbPolicyTypeDeleted, function (i, item) {
                Product.PolicyTypes.push(item);
            });
            glbPolicyTypeDeleted = [];
        }
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorRiskTypeEmpty, 'autoclose': true })
    }
}
function PolicyType(policyType) {
    $("#listModalPolicyType").UifListView({
        sourceData: policyType,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: Product.IsUse ? !Product.IsUse : true,
        displayTemplate: "#PolicyTypeTemplate",
        height: heightListView,
        deleteCallback: deleteCallbackPolicyType,
    });
}
function validatePolicyType() {
    var msj = "";
    if ($("#selectPrefixCommercial").UifSelect('getSelected') == null || $("#selectPrefixCommercial").UifSelect('getSelected') == '') {
        msj = Resources.Language.MessageBranch + "<br>";
    }
   
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    return true;
}
function CleanObjectPolicyType() {
    $("#listModalPolicyType").UifListView({
        source: null,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: Product.IsUse ? !Product.IsUse : true,
        displayTemplate: "#PolicyTypeTemplate",
        height: heightListView,
        deleteCallback: deleteCallbackPolicyType,
    });
}

var deleteCallbackPolicyType = function (deferred, data) {
    if (data.IsDefault) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDeleteDefault, 'autoclose': true });
    } else {
        var dataList = $("#listModalPolicyType").UifListView("getData");

        if (dataList.length == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorTypePolicy, 'autoclose': true })
        }
        else {
            if (data.StatusTypeService === 1 || data.StatusTypeService === 3) {
                data.StatusTypeService = 4;
                glbPolicyTypeDeleted.push(data);
            }
            deferred.resolve();
        }
    }
}