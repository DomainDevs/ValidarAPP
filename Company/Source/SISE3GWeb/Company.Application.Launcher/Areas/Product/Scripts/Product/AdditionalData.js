$.ajaxSetup({ async: false });
var AddDataPrefixId;
var glbDataLimitRc = [];
var glbProductFormDeleted = [];
var ProductClone;
var moduleId = 0;
var limitRcLoaded = false;
var activitiesLoaded = false;
var deductiblesLoaded = false;
var productFormLoaded = false;

$(document).ready(function () {
    EventsAdditionalData();
    //new AdditionalDataRequest();
});
//Eventos
function EventsAdditionalData() {
    
    $("#selectModalFormAdditionalData").on('itemSelected', function (event, selectedItem) {
        moduleId = parseInt(selectedItem.Id);
        showModuleAdditionalData(moduleId);
    });

    $("#btnModalAdditionalDataSave").on("click", function () {
        SaveDataAdditional();
        ScrollTop();
    });

    $("#btnModalAdditionalDataClose").on("click", function () {
        clearFormForm();        
        glbProductFormDeleted = [];
        glbDataLimitRc = [];        
        ShowPanelsProduct(MenuProductType.Product);
    });
    $("#btnCancelForms").on("click", function () {
        clearFormForm();
    })
    $('#selectModalPolicyTypeRc').on('itemSelected', function (event, selectedItem) {

        var tmpLimitsRC = [];

        if (selectedItem.Id > 0) {            

            $.each(glbDataLimitRc, function (index, value) {
                if (value.PolicyTypeId == selectedItem.Id)
                {
                    tmpLimitsRC.push(value);
                }
            });

            $("#tableProductLimitRC").UifDataTable({ sourceData: tmpLimitsRC });
            tmpLimitsRC = [];                        
        }
        else
        {
            $("#tableProductLimitRC").UifDataTable({ sourceData: tmpLimitsRC });
        }
    });

}

function LoadTablesAdditional() {
    $("#selectModalFormAdditionalData").UifSelect({ source: rootPath + "Product/Product/GetProductModulesAdditionalDataSelect" });    
    lockScreen();
    setTimeout(function () {
        LoadProductLimitRc();
        LoadProductActivities();
        LoadProductDeductibles();
        LoadProductForm();
    }, 200);
}

function ValidateLockScreen() {
    if (limitRcLoaded && activitiesLoaded && deductiblesLoaded && productFormLoaded) {
        unlockScreen();
    }
}

function LoadProductDeductibles() {    
    AdditionalDataRequestT.LoadProductDeductibles(Product.Id, Product.PrefixId).done(function (data) {
        deductiblesLoaded = true;
        ValidateLockScreen();
        if (data.success) {
            $("#tableProductDeduct").UifDataTable({ sourceData: data.result });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        deductiblesLoaded = true;
        ValidateLockScreen();
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetDeductibles, 'autoclose': true });
    });
}


function LoadProductActivities() {
    AdditionalDataRequestT.LoadProductActivities(Product.Id).done(function (data) {
        activitiesLoaded = true;
        ValidateLockScreen();
        if (data.success) {
            $("#tableProductAtivities").UifDataTable({ sourceData: data.result });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        activitiesLoaded = true;
        ValidateLockScreen();
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetActivities, 'autoclose': true });
    });
}

function LoadProductForm() {

    if (Product.ProductCoveredRisks != null) {
        var groupCoverageSelect = [];
        var productForm = [];
        $.each(Product.ProductCoveredRisks, function (keyRisks, Risks) {
            if (Risks.GroupCoverages != null) {
                $.each(Risks.GroupCoverages, function (keyGroupCoverages, GroupCoverages) {
                    groupCoverageSelect.push({
                        Id: GroupCoverages.Id,
                        Description: GroupCoverages.Description
                    });
                });
            }
        });
        $("#selectFormsGroupCoverage").UifSelect({
            sourceData: groupCoverageSelect
        });

        AdditionalDataRequestT.LoadProductForm(Product.Id).done(function (data) {
            productFormLoaded = true;
            ValidateLockScreen();
            if (data.success) {

                if (data.result != null) {
                    $.each(data.result, function (key, value) {
                        value.Description = $("#selectFormsGroupCoverage option[value=" + value.CoverGroupId + "]").text();
                        productForm.push(value);
                    });
                }

            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            productFormLoaded = true;
            ValidateLockScreen();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetForms, 'autoclose': true });
        });

    }

    //llena las formas de impresion

    $("#listModalProductForms").UifListView({
        sourceData: productForm.length > 0 ? productForm : [],
        customEdit: true,
        localMode: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#display-template-ProductForms",
        height: 400,
        deleteCallback: deleteCallbackForm
    });
}

function LoadProductLimitRc() {
    AdditionalDataRequestT.LoadProductLimitRc(Product.PolicyTypes, Product.Id, Product.PrefixId).done(function (data) {
        limitRcLoaded = true;
        ValidateLockScreen();
        if (data.success) {
            glbDataLimitRc = data.result;
            //$("#tableProductLimitRC").UifDataTable({ sourceData: data.result });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        limitRcLoaded = true;
        ValidateLockScreen();
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemporary, 'autoclose': true });
    });  
}

function LoadAdditionalData() {
    ProductClone = fnClonejQuery(Product);
    //marcar Actividades
    if (Product.ProductCommercialClass != null) {
        var data = $("#tableProductAtivities").UifDataTable("getData");
        $.each(data, function (keyActivityTable, ActivityTable) {
            data[keyActivityTable].AssignedInd = false;
            data[keyActivityTable].DefaultInd = false;
            $.each(Product.ProductCommercialClass, function (keyActivity, Activity) {
                if (ActivityTable.RiskCommercialClassCode == Activity.RiskCommercialClass.RiskCommercialClassCode) {
                    data[keyActivityTable].AssignedInd = true;
                    data[keyActivityTable].DefaultInd = Activity.DefaultRiskCommercial;
                    return;
                }
            });
        });
        $("#tableProductAtivities").UifDataTable({ sourceData: data })
    }

    
    //llena las formas de impresion

    $("#listModalProductForms").UifListView({
        sourceData: ProductClone.ProductForm != null ? ProductClone.ProductForm : [],
        customEdit: true,
        localMode: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#display-template-ProductForms",
        height: 400,
        deleteCallback: deleteCallbackForm
    });
}

function SaveDataAdditional() {

    var listActivities = $("#tableProductAtivities").UifDataTable("getData");
    var listLimitsRC = glbDataLimitRc;
    var listDeducts = $("#tableProductDeduct").UifDataTable("getData");
    var listProductForm = $("#listModalProductForms").UifListView("getData").concat(glbProductFormDeleted);    

    if (listActivities.length > 0) {
        var ifExist = listActivities.filter(function (item) {
            return item.IsSelected == true && item.StatusTypeService != 4;
        });
        if (ifExist.length > 0) {
            var ifExistCheckDefault = ifExist.filter(function (itemCheckDefault) {
                return itemCheckDefault.IsDefault == true;
            });
            if (ifExistCheckDefault.length != 1) {
                $.UifNotify('show', { 'type': 'info', 'message': 'Debe existir una actividad predeterminada', 'autoclose': true });
                return false;
            }
        }
    }


    if (listLimitsRC.length > 0) {
        var lstMsj = [];
        var respuestaLimiteRc = false;
        $.each(Product.PolicyTypes, function (keyActivityTable, ActivityTable) {
            var ifExistPolicyType = listLimitsRC.filter(function (itemPolicyType) {
                return itemPolicyType.PolicyTypeId == ActivityTable.Id;
            });

            if (ifExistPolicyType.length > 0) {
                var ifExist = ifExistPolicyType.filter(function (item) {
                    return item.IsSelected == true && item.StatusTypeService != 4;
                });
                if (ifExist.length > 0) {
                    var ifExistCheckDefault = ifExist.filter(function (itemCheckDefault) {
                        return itemCheckDefault.IsDefault == true;
                    });
                    if (ifExistCheckDefault.length != 1) {
                        lstMsj.push('Debe existir un límite RC predeterminado para ' + ActivityTable.Description);
                        respuestaLimiteRc = true;
                        //$.UifNotify('show', { 'type': 'info', 'message': 'Debe existir un límite RC predeterminado para ' + ActivityTable.Description, 'autoclose': true });
                        //return true;
                    }
                }
            }
        });

        if (respuestaLimiteRc == true) {
            $.each(lstMsj, function (keyActivityTable, itemMsj) {
                $.UifNotify('show', { 'type': 'info', 'message': itemMsj, 'autoclose': true });
            });
            return false;
        }
    }
    
        AdditionalDataRequestT.SaveDataAdditional(listActivities, listLimitsRC, listDeducts, listProductForm, Product.Id).done(function (data) {
        if (data.success) {
            clearFormForm();
            glbProductFormDeleted = [];
            glbDataLimitRc = [];
            $("#tableProductDeduct").UifDataTable("clear")
            $("#tableProductAtivities").UifDataTable("clear")
            $("#tableProductLimitRC").UifDataTable("clear");
            ShowPanelsProduct(MenuProductType.Product);
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AdditionalDataSaved, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemporary, 'autoclose': true });
    });  
	
}

function showModuleAdditionalData(moduleId) {
    hideModuleAdditionalData();
    switch (moduleId) {
        case 1:
            $("#productActivities").show();
            break;
        case 2:
            $("#formProductForms").resetear();
            $("#productForms").show();
            $("#productForms").css("display", "block");
            break;
        case 3:
            $("#selectModalPolicyTypeRc").UifSelect({ sourceData: Product.PolicyTypes, id: "RiskTypeId", name: "Description" });

            $("#productLimitRc").show();
            break;
        case 4:

            $("#productDeduct").show();
            break;
        default:
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectModule, 'autoclose': true });
            break;
    }
}
function hideModuleAdditionalData() {
    $("#productForms, #productActivities, #productLimitRc, #productDeduct").hide();
}

//function GetDeductiblesByPrefixIdProductId(prefixId, idProduct) {
//	$.ajax({
//		type: "POST",
//		url: rootPath + 'Product/Product/GetDeductiblesByPrefixIdProductId',
//		data: JSON.stringify({ prefixId: prefixId, idProduct: idProduct }),
//		dataType: "json",
//		contentType: "application/json; charset=utf-8"
//	}).done(function (data) {
//		if (data.success) {
//			$("#tableProductDeduct").UifDataTable({ sourceData: data.result });
//		}
//		else {
//			$.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorConsultingDeductibles, 'autoclose': true })
//		}
//	}).fail(function (jqXHR, textStatus, errorThrown) {
//		$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorConsultingDeductibles, 'autoclose': true })
//	});
//}


$.fn.resetear = function () {
    $(this).each(function () { this.reset(); });
}

//funcinalidad item Actividades
$("#tableProductAtivities").on("rowEdit", function (event, item, index) {
    $("#editTableProductAtivities").find("#nameProductAtivities").text(item.Description);
    $("#editTableProductAtivities").find("#IdProductAtivities").val(item.Id);
    if (item.IsSelected != null && item.IsSelected) {
        $("#editTableProductAtivities").find("#chkDefaultProductAtivities").prop('checked', item.IsDefault);
    } else {
        $("#editTableProductAtivities").find("#chkDefaultProductAtivities").removeAttr('checked');
    }
    if (item.IsSelected != null && item.IsSelected) {
        $("#editTableProductAtivities").find("#chkAssignedProductAtivities").prop('checked', item.IsSelected);
    }
    else {
        $("#editTableProductAtivities").find("#chkAssignedProductAtivities").removeAttr('checked');
    }

    $('#editTableProductAtivities').UifInline('show');
    ScrollTop();
});
$('#editTableProductAtivities').on('Save', function () {
    var data = $("#tableProductAtivities").UifDataTable("getData");
    $.each(data, function (key, item) {
        if (item.Id == $("#editTableProductAtivities").find("#IdProductAtivities").val()) {
            var tmpIsSelected = $("#editTableProductAtivities").find("#chkAssignedProductAtivities").is(":checked");
            var tmpIsDefault = $("#editTableProductAtivities").find("#chkDefaultProductAtivities").is(":checked");
            if (tmpIsSelected != this.IsSelected || tmpIsDefault != this.IsDefault)
            {
                var IsSelectedBefore = this.IsSelected;
                this.IsDefault = tmpIsDefault;
                if (this.IsDefault) {
                    this.IsSelected = tmpIsDefault;
                }
                else {
                    this.IsSelected = tmpIsSelected;
                }
                this.StatusTypeService = validateStatusTypeService(this.StatusTypeService, IsSelectedBefore, this.IsSelected);
            }
        } else {
            if ($("#editTableProductAtivities").find("#chkDefaultProductAtivities").is(":checked")) {
                this.IsDefault = false;
                if (this.IsSelected && this.StatusTypeService != 2) {
                    this.StatusTypeService = 3;
                }
            }
        }
    });

    $("#tableProductAtivities").UifDataTable({ sourceData: data });
    $('#editTableProductAtivities').UifInline('hide');
});
//FIN - funcinalidad item Actividades

//funcinalidad item Deducibles
$("#tableProductDeduct").on("rowEdit", function (event, item, index) {
    $("#editTableProductDeduct").find("#nameProductDeduct").text(item.Description);
    $("#editTableProductDeduct").find("#IdProductDeduct").val(item.DeductId);
    if (item.IsSelected != null && item.IsSelected) {
        $("#editTableProductDeduct").find("#chkAssignedProductDeduct").prop('checked', item.IsSelected);
    }
    else {
        $("#editTableProductDeduct").find("#chkAssignedProductDeduct").removeAttr('checked');
    }
    $('#editTableProductDeduct').UifInline('show');
    ScrollTop();
});
$('#editTableProductDeduct').on('Save', function () {
    var data = $("#tableProductDeduct").UifDataTable("getData");
    $.each(data, function (key, item) {
        if (item.DeductId == $("#editTableProductDeduct").find("#IdProductDeduct").val()) {
            var isSelectedBefore = this.IsSelected;
            var isSelectedAfter = $("#editTableProductDeduct").find("#chkAssignedProductDeduct").is(":checked");
            this.IsSelected = isSelectedAfter;
            this.StatusTypeService = validateStatusTypeServiceOnlyAssigned(this.StatusTypeService, isSelectedBefore, isSelectedAfter);
        }
    });
    $("#tableProductDeduct").UifDataTable({ sourceData: data });
    $('#editTableProductDeduct').UifInline('hide');
});
//FIN - funcinalidad item Deducibles

//funcinalidad item LimitesRC
$("#tableProductLimitRC").on("rowEdit", function (event, item, index) {
    $("#editTableProductLimitRC").find("#nameProductLimitRC").text(item.Description);
    $("#editTableProductLimitRC").find("#IdProductLimitRC").val(item.Id);
    if (item.IsDefault != null && item.IsDefault) {
        $("#editTableProductLimitRC").find("#chkDefaultProductLimitRC").prop('checked', item.IsDefault);
    } else {
        $("#editTableProductLimitRC").find("#chkDefaultProductLimitRC").removeAttr('checked');
    }
    if (item.IsSelected != null && item.IsSelected) {
        $("#editTableProductLimitRC").find("#chkAssignedProductLimitRC").prop('checked', item.IsSelected);
    }
    else {
        $("#editTableProductLimitRC").find("#chkAssignedProductLimitRC").removeAttr('checked');
    }

    $('#editTableProductLimitRC').UifInline('show');
    ScrollTop();
});

$('#editTableProductLimitRC').on('Save', function () {
    if ($('#selectModalPolicyTypeRc').UifSelect("getSelected") == "" || $('#selectModalPolicyTypeRc').UifSelect("getSelected") == null) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + Resources.Language.ErrorSelectPoliciType, 'autoclose': true });
        return false;
    }
    var data = $("#tableProductLimitRC").UifDataTable("getData");
    $.each(data, function (key, item) {
        if (item.Id == $("#editTableProductLimitRC").find("#IdProductLimitRC").val()) {
            var tmpIsSelected = $("#editTableProductLimitRC").find("#chkAssignedProductLimitRC").is(":checked");
            var tmpIsDefault = $("#editTableProductLimitRC").find("#chkDefaultProductLimitRC").is(":checked");
            if (tmpIsSelected != this.IsSelected || tmpIsDefault != this.IsDefault)
            {
                var IsSelectedBefore = this.IsSelected;
                this.IsDefault = tmpIsDefault;
                if (this.IsDefault) {
                    this.IsSelected = tmpIsDefault;
                } else {
                    this.IsSelected = tmpIsSelected;
                }
                if (this.IsSelected) {
                    this.PolicyTypeId = $('#selectModalPolicyTypeRc').UifSelect("getSelected");
                }
                this.StatusTypeService = validateStatusTypeService(this.StatusTypeService, IsSelectedBefore, this.IsSelected);
            }
        } else {
            if ($("#editTableProductLimitRC").find("#chkDefaultProductLimitRC").is(":checked")) {
                this.IsDefault = false;
                if (this.IsSelected && this.StatusTypeService != 2) {
                    this.StatusTypeService = 3;
                }
            }
        }
    });

    $("#tableProductLimitRC").UifDataTable({ sourceData: data });
    $('#editTableProductLimitRC').UifInline('hide');
});
//FIN - funcinalidad item LimitesRC

//funcionalidad para Forma de impresion
$("#listModalProductForms").on('rowEdit', function (event, data, index) {
    var index = $("#listModalProductForms").UifListView("findIndex",
        function (item) {
            if (item.FormNumber == data.FormNumber &&
                item.StrCurrentFrom == data.StrCurrentFrom &&
                item.CoverGroupId == data.CoverGroupId) {
                return true;
            } else {
                return false;
            }
        });
    $("#formProductForms").find("#hdnFormId").val(data.FormId);
    $("#formProductForms").find("#inputModalFormNumber").val(data.FormNumber);
    $("#formProductForms").find("#inputCurrentFrom").val(data.StrCurrentFrom);
    $("#selectFormsGroupCoverage").UifSelect('setSelected', data.CoverGroupId);
    $("#formProductForms").find("#hdnStatusTypeService").val(data.StatusTypeService);
    $("#formProductForms").find("#hdnIndexForm").val(index);
    ScrollTop();
});
var deleteCallbackForm = function (deferred, data) {
    if (data.StatusTypeService == 1 || data.StatusTypeService == 3)
    {
        data.StatusTypeService = 4;
        glbProductFormDeleted.push(data);
    }    
    deferred.resolve();
    clearFormForm();
}
//function filterPolicyType(obj) {
//    if ('PolicyTypeId' in obj && !isNaN(obj.PolicyTypeId) && (obj.PolicyTypeId == $('#selectModalPolicyTypeRc').UifSelect("getSelected") || obj.PolicyTypeId == 0)) {
//        return true;
//    } else {
//        return false;
//    }
//}

$("#btnAssignForms").on("click", function () {
    $("#formProductForms").validate();
    if ($("#formProductForms").valid()) {
        var object = { GroupCoverage: {} };
        object.FormId = $("#formProductForms").find("#hdnFormId").val();
        object.FormNumber = $("#formProductForms").find("#inputModalFormNumber").val().toUpperCase();
        object.StrCurrentFrom = $("#formProductForms").find("#inputCurrentFrom").val();
        object.CoverGroupId = $("#formProductForms").find("#selectFormsGroupCoverage").val();
        object.Description = $("#formProductForms").find("#selectFormsGroupCoverage option[value=" + object.CoverGroupId + "]").text();
        var statusTypeService = $("#formProductForms").find("#hdnStatusTypeService").val();        
        
        var index = $("#formProductForms").find("#hdnIndexForm").val();
        if (index == "" || index == null) {            
            object.StatusTypeService = 2;
            $('#listModalProductForms').UifListView("addItem", object);            
        } else {            
            if (statusTypeService == 1) {
                object.StatusTypeService = 3;
            }
            $('#listModalProductForms').UifListView("editItem", index, object);
        }
        clearFormForm();
        ScrollTop();
    }
})

function validateStatusTypeServiceOnlyAssigned(statusTypeService, isSelectedBefore, isSelectedAfter)
{
    if (statusTypeService == 1)
    {
        if (!isSelectedBefore && isSelectedAfter) {
            return 2;
        }
        else if (isSelectedBefore && !isSelectedAfter)
        {
            return 4;
        }
    }
    if (statusTypeService == 2) {
        if (isSelectedBefore && !isSelectedAfter) {
            return 1;
        }        
    }

    if (statusTypeService == 4) {
        if (!isSelectedBefore && isSelectedAfter) {
            return 1;
        }
    }

    return statusTypeService;
}

function validateStatusTypeService(statusTypeService, isSelectedBefore, isSelectedAfter)
{
    if (statusTypeService == 1)
    {
        if (!isSelectedBefore && isSelectedAfter)
        {
            return 2;
        }
        else if (isSelectedBefore && !isSelectedAfter)
        {
            return 4;
        }
        else if (isSelectedBefore && isSelectedAfter) {
            return 3;
        }
        else if (!isSelectedBefore && !isSelectedAfter) {
            return statusTypeService;
        }
    }
    if (statusTypeService == 2)
    {
        if (isSelectedBefore && !isSelectedAfter) {
            return 1;
        }
        else if (isSelectedBefore && isSelectedAfter) {
            return statusTypeService;
        }
    }
    if (statusTypeService == 3) {
        if (isSelectedBefore && isSelectedAfter) {
            return statusTypeService;
        }
        else if (isSelectedBefore && !isSelectedAfter) {
            return 4;
        }
    }
    if (statusTypeService == 4) {
        if (!isSelectedBefore && isSelectedAfter) {
            return 3;
        }
    }
    return statusTypeService;
}
function clearFormForm() {
    $("#formProductForms").find("#inputModalFormNumber").val("");
    $("#formProductForms").find("#inputCurrentFrom").val("");
    $("#formProductForms").find("#selectFormsGroupCoverage").val("");
    $("#formProductForms").find("#hdnIndexForm").val("");
    $("#formProductForms").find("#hdnFormId").val("");
    $("#formProductForms").find("#hdnStatusTypeService").val("");        
}
