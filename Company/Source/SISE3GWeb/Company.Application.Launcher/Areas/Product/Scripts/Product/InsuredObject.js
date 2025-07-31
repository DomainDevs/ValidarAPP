$.ajaxSetup({ async: false });
var glbInsuredObjects = [];
var insuredObjectDelete = [];
//variables para mantener informacion
var modalCoverageRiskTypeList = [];
var modalCoverageGroupCoverageList = [];
var modalCoverageInsuredObjectsList = [];
var modalCoverageRiskTypeListTemp = [];
var ProductCoveredRisksTemp = [];
//Indice para listview
var insuredObjectIndex = null;
var modalCoverageInfoRiskTypes = [];

$(document).ready(function () {
    $("#listModalInsuredObjectData").UifListView({ source: null, displayTemplate: "#modalInsuredObjectTemplate", edit: true, delete: true, customEdit: true, deleteCallback: DeleteInsuredObject, height: 400, selectionType: 'single', drag: true });
    EventInsuredObject();
});

//EVENTOS DE LOS CONTROLES DEL FORMULARIO
function EventInsuredObject() {
    $("#selectModalInsuredObjectGroupCoverage").on("itemSelected", function (event, selectedItem) {
        insuredObjectIndex = null;
        DisabledControlInsured(true);

        $("#selectModalInsuredObject").removeData("Object");
        $("#listModalInsuredObjectData").UifListView('refresh');
        if (selectedItem.Id != "") {
            LoadCoverageInsuredObjectsList();
            GetCoverageInsuredObjectByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), 0);
        }
    });

    // Modulo Coberturas Objetos de Seguro
    $("#selectModalInsuredObjectCoverageRiskType").on("itemSelected", function (event, selectedItem) {
        if (selectedItem.Id > 0) {
            $("#selectModalInsuredObjectGroupCoverage").prop("disabled", false);
            GetCoverageGroupCoverageByRiskType(selectedItem.Id, 0);
            //modalCoverageCoveragesList = [];
            //modalCoverageInsuredObjectsList = [];
            //modalCoverageGroupCoverageList = [];
        }
        else {
            ClearInsuredObject();
            $("#selectModalInsuredObjectGroupCoverage").UifSelect();
            $("#selectModalInsuredObjectGroupCoverage").prop("disabled", true);
            $("#listModalInsuredObjectData").UifListView('refresh');
        }
    });

    $("#btnAssignModalInsuredObject").on('click', function () {
        if (ValidateDataInsuredObject()) {
            AddInsuredObject();
        }
    });

    $("#chkInitialIncludeInsuredObject, label[for='chkInitialIncludeInsuredObject']").click(function () {
        if ($("#chkInitialIncludeInsuredObject").is(':checked')) {
            $("#chkInitialIncludeInsuredObject").prop('checked', false);
        } else {
            $("#chkInitialIncludeInsuredObject").prop('checked', true);
        }
    });

    $("#chkMandatoryInsuredObject, label[for='chkMandatoryInsuredObject']").click(function () {
        if ($("#chkMandatoryInsuredObject").is(':checked')) {
            $("#chkMandatoryInsuredObject").prop('checked', false);
        } else {
            $("#chkMandatoryInsuredObject").prop('checked', true);
        }
    });

    $("#btnCancelAssignModalInsuredObject").on('click', function () {
        LoadInsuredObject();
        ClearInsuredObject();
        //insuredObject_getCoveredRisk();        
        $("#selectModalInsuredObjectGroupCoverage").UifSelect();
        $("#selectModalInsuredObjectGroupCoverage").prop("disabled", true);
        $("#listModalInsuredObjectData").UifListView('refresh');
        $("#selectModalInsuredObjectCoverageRiskType").UifSelect("setSelected", "");
        $("#selectModalInsuredObject").UifSelect("setSelected", "");
    });

    $('#listModalInsuredObjectData').on('rowAdd', function (event) {
        ClearInsuredObject();
    });

    $('#listModalInsuredObjectData').on('rowEdit', function (event, data, index) {
        insuredObjectIndex = index;
        DisabledControlInsured(false);
        SetInsuredObject(data);
    });

    //Coberturas
    $("#btnCoverage").on("click", function () {
        LoadInsuredObject();
    });

    $("#btnModalInsuredObjectSave").on("click", function () {
        SaveProductInsuredObject();
        Product.pendingChanges = true;
    });

    $("#btnModalInsuredObjectClose").on("click", function () {
        modalDeductibleByCoverageListTemp = [];
        modalCoverageRiskTypeListTemp = [];
        ShowPanelsProduct(MenuProductType.Product);
    });

}

function LoadInsuredObject() {
    //if (Product.Id === 0) {
    //    var maxRiskCuantity = $('#inputModalMaximumNumberRisk').val().trim() == "" ? 0 : $('#inputModalMaximumNumberRisk').val().trim();
    //    Product.ProductCoveredRisks = { Id: $('#selectRiskType').UifSelect("getSelected"), Description: $("#selectRiskType").UifSelect("getSelectedText"), MaxRiskQuantity: maxRiskCuantity };
    //}
    if (typeof Product.ProductCoveredRisks !== 'undefined' || Product.ProductCoveredRisks != null || Product.Id === 0) {
        glbInsuredObjects = fnClonejQueryArray(Product.ProductCoveredRisks);
        var riskTypeId = $("#selectRiskType").UifSelect("getSelected");
        if (riskTypeId != null && riskTypeId != "" && riskTypeId != 0) {
            ClearForm();
            GetRiskTypeSelected(riskTypeId);
            ShowPanelsProduct(MenuProductType.InsuredObject);
            $("#selectModalInsuredObjectGroupCoverage").prop("disabled", false);
            GetCoverageGroupCoverageByRiskType(riskTypeId, 0);
            if (glbInsuredObjects != null && glbInsuredObjects[0].GroupCoverages != null) {
                if (glbInsuredObjects[0].GroupCoverages.length < 0) {
                    $("#selectModalInsuredObjectGroupCoverage").UifSelect('setSelected', glbInsuredObjects[0].GroupCoverages[0].Id);
                }                
            }
            LoadCoverageInsuredObjectsList();
            GetCoverageInsuredObjectByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), 0);
            if ($("#selectModalInsuredObjectGroupCoverage option[value!='']").length === 1) {
                $('#selectModalInsuredObjectGroupCoverage').UifSelect("setSelected", $("#selectModalInsuredObjectGroupCoverage option:eq(1)").val());
            }
            if ($("#selectModalInsuredObject option[value!='']").length === 1) {
                $('#selectModalInsuredObject').UifSelect("setSelected", $("#selectModalInsuredObject option:eq(1)").val());
                if (!$("#chkInitialIncludeInsuredObject").is(':checked')) {
                    $("#chkInitialIncludeInsuredObject").prop('checked', true);
                }

                if (!$("#chkMandatoryInsuredObject").is(':checked')) {
                    $("#chkMandatoryInsuredObject").prop('checked', true);
                }
            }
        }
    }
    else {
        ClearInsuredObject();
        ClearForm();
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectTypeRisk, 'autoclose': true });
    }
}

//GUARDAR OBJETOS DEL SEGURO 
function SaveProductInsuredObject() {
    if ($("#selectModalInsuredObject").data("Object") !== null || $("#selectModalInsuredObject").data("Object") !== undefined) {
        var validCoverage = true;
        var idInsured = [];
        //validar si todos los objetos del seguro tienen cobertura
        $.each(glbInsuredObjects, function (index, item) {
            var groupCoverages = this.GroupCoverages;
            if (groupCoverages !== null) {
                groupCoverages = this.GroupCoverages.filter(function (item) {
                    return item.StatusTypeService !== 4;
                });
                if (groupCoverages.length > 0) {
                    $.each(groupCoverages, function (index, item) {
                        var insuredObjects = this.InsuredObjects;
                        if (insuredObjects !== null) {
                            insuredObjects = this.InsuredObjects.filter(function (item) {
                                return item.StatusTypeService !== 4;
                            });
                            if (insuredObjects.length > 0) {
                                $.each(insuredObjects, function (index, item) {
                                    var coverages = this.Coverages.filter(function (item) {
                                        validCoverage = true;
                                        return item.Coverage.StatusTypeService !== 4;
                                    });
                                    if (coverages.length === 0) {
                                        validCoverage = false;
                                        return;
                                    }
                                });
                            }
                            else {
                                validCoverage = false;
                                return;
                            }
                        }
                        else {
                            validCoverage = false;
                            return;
                        }
                    });
                }
                else {
                    validCoverage = false;
                    return;
                }
            }
            else {
                validCoverage = false;
                return;
            }
        });


        //$.each(modalCoverageRiskTypeListTemp, function (index, item) {
        //    var groupCoverages = this.GroupCoverages
        //    if (groupCoverages != undefined) {
        //        $.each(groupCoverages, function (index, item) {
        //            var insuredObjects = this.InsuredObjects;
        //            $.each(insuredObjects, function (index, item) {
        //                if ($.inArray(parseInt(item.Id), idInsured) == -1) {
        //                    validCoverage = false;
        //                    return;
        //                }
        //            });
        //        });
        //    }
        //});


        if (validCoverage) {
            //modalCoverageRiskTypeList = fnClonejQueryArray(modalCoverageRiskTypeListTemp);
            //Product.ProductCoveredRisks = fnClonejQueryArray(modalCoverageRiskTypeListTemp);
            //modalCoverageRiskTypeListTemp = [];
            //if (Product.ProductForm != undefined) {
            //    var productForm = Product.ProductForm;
            //    var productFormTemp = [];
            //    $.each(productForm, function (index, item) {
            //        var groupFrom = this.GroupCoverage;
            //        $.each(modalCoverageRiskTypeList, function (index, item) {
            //            var groupCoverages = this.GroupCoverages;
            //            $.each(groupCoverages, function (index, item) {
            //                if (this.Id == groupFrom.Id) {
            //                    productFormTemp.push(productForm[index]);
            //                }
            //            });
            //        });
            //    });

            //    Product.ProductForm = fnClonejQueryArray(productFormTemp);
            //}
            Product.ProductCoveredRisks = fnClonejQueryArray(glbInsuredObjects);
            glbInsuredObjects = [];
            ShowPanelsProduct(MenuProductType.Product);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidCoverage, 'autoclose': true });
        }
    } else {
        $.UifDialog(
            'confirm',
            { 'message': 'Los cambios no confirmados serán descartados ', 'title': '¿Existen cambios sin confirmar, desea continuar?' },
            function (result) {
                if (result) {
                    $("#btnCancelAssignModalInsuredObject").click();
                    $("#btnModalInsuredObjectSave").click();
                }
            });
    }
}
//ELIMINAR EL OBJETO DEL SEGURO
var DeleteInsuredObject = function (deferred, data) {
    var validInsuredObject = true;
    if (Product.Id > 0) {
        ValidatePolicyByProductId();
        if (validatePolicy != null && validatePolicy == true & ($("#chkInitialIncludeInsuredObject").is(':checked') || $("#chkMandatoryInsuredObject").is(':checked'))) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageInsuredObjectInUse, 'autoclose': true });
            return false;
        }
        validInsuredObject = ExistInsuredObjectProduct(Product.Id, data.CoverageGroupId, data.Id);
    }
    else {
        validInsuredObject = false;
    }
    if (!validInsuredObject) {
        $.each(glbInsuredObjects, function (indexRisk, itemRisk) {
            if (itemRisk.Id == data.RiskTypeId) {
                $.each(itemRisk.GroupCoverages, function (indexGroupCoverages, itemGroupCoverages) {
                    if (itemGroupCoverages.Id == data.CoverageGroupId) {
                        $.each(itemGroupCoverages.InsuredObjects, function (indexInsuredObjects, itemInsuredObjects) {
                            if (itemInsuredObjects.Id == data.Id) {
                                if (itemInsuredObjects.StatusTypeService === 1 || itemInsuredObjects.StatusTypeService === 3) {
                                    glbInsuredObjects[indexRisk].GroupCoverages[indexGroupCoverages].InsuredObjects[indexInsuredObjects].StatusTypeService = 4;
                                    data.StatusTypeService = 4;
                                    insuredObjectDelete.push(data);
                                }
                                else {
                                    glbInsuredObjects[indexRisk].GroupCoverages[indexGroupCoverages].InsuredObjects.splice(indexInsuredObjects, 1);
                                }
                                return false;
                            }
                        });
                        if (itemGroupCoverages.InsuredObjects === []) {
                            if (itemGroupCoverages.StatusTypeService !== 2) {
                                itemGroupCoverages.StatusTypeService = 4;
                            }
                            else {
                                glbInsuredObjects[indexRisk].GroupCoverages.splice(indexGroupCoverages, 1);
                            }
                        } else {
                            var validateGroupCoverage = itemGroupCoverages.InsuredObjects.filter(function (item) {
                                return item.StatusTypeService === 4;
                            });
                            if (itemGroupCoverages.InsuredObjects.length === validateGroupCoverage.length) {
                                itemGroupCoverages.StatusTypeService = 4;
                            }
                        }
                    }
                });
            }
        });
        deferred.resolve();
        var riskTypeId = $("#selectRiskType").UifSelect("getSelected");
        var groupCoverage = $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected");
        GetCoverageGroupCoverageByRiskType(riskTypeId, groupCoverage);
        GetCoverageInsuredObjectByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), 0);
        $("#chkInitialIncludeInsuredObject").prop('checked', false);
        $("#chkMandatoryInsuredObject").prop('checked', false);
        insuredObjectIndex = null;
        DisabledControlInsured(true);
    }
};

//AGREGAR O EDITAR OBJECTOS DEL SEGURO
function AddInsuredObject() {
    $("#formInsuredObjects").validate();
    if ($("#formInsuredObjects").valid()) {
        if (insuredObjectIndex == null) {
            if (!ValidateInsuredObject($("#selectModalInsuredObject").UifSelect("getSelected"))) {
                $("#listModalInsuredObjectData").UifListView("addItem", GetInsuredObject());
                $.each(glbInsuredObjects, function (index, value) {
                    if (this.Id == $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")) {
                        if (value.GroupCoverages != null) {
                            var validateGroupCoverage = value.GroupCoverages.filter(function (item) {
                                return item.Id == $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected");
                            });
                            if (validateGroupCoverage.length > 0) {
                                $.each(value.GroupCoverages, function (key, valueGroupCoverage) {
                                    if (this.Id == $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")) {
                                        var insuredDelete = glbInsuredObjects[index].GroupCoverages[key].InsuredObjects.filter(function (item) {
                                            return item.StatusTypeService === 4;
                                        });
                                        glbInsuredObjects[index].GroupCoverages[key].InsuredObjects = [];
                                        $.each($("#listModalInsuredObjectData").UifListView("getData"), function (index2, itemInsuredObject) {
                                            glbInsuredObjects[index].GroupCoverages[key].InsuredObjects.push(itemInsuredObject);
                                        });
                                        if (insuredDelete.length > 0) {
                                            $.each(insuredDelete, function (index, item) {
                                                glbInsuredObjects[index].GroupCoverages[key].InsuredObjects.push(item);
                                            });
                                        }
                                        if (glbInsuredObjects[index].GroupCoverages[key].StatusTypeService !== 2) {
                                            glbInsuredObjects[index].GroupCoverages[key].StatusTypeService = 3;
                                        }
                                    }
                                });
                            }
                            else {
                                glbInsuredObjects[index].GroupCoverages.push(
                                    {
                                        Id: parseInt($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")),
                                        Description: $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelectedText"),
                                        RiskTypeId: parseInt($("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")),
                                        InsuredObjects: $("#listModalInsuredObjectData").UifListView("getData"),
                                        Form: null,
                                        StatusTypeService: 2
                                    }
                                );
                            }
                        }
                        else {
                            glbInsuredObjects[index].GroupCoverages = [];
                            glbInsuredObjects[index].GroupCoverages.push(
                                {
                                    Id: parseInt($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")),
                                    Description: $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelectedText"),
                                    RiskTypeId: parseInt($("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")),
                                    InsuredObjects: $("#listModalInsuredObjectData").UifListView("getData"),
                                    Form: null,
                                    StatusTypeService: 2
                                }
                            );
                        }
                    }
                });
                var riskTypeId = $("#selectRiskType").UifSelect("getSelected");
                var groupCoverage = $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected");
                GetCoverageGroupCoverageByRiskType(riskTypeId, groupCoverage);
                ClearInsuredObject();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInsuredObjectAssigned, 'autoclose': true });
            }
        }
        else {
            var insuredObjectAdd = $("#selectModalInsuredObject").data("Object");
            insuredObjectAdd.IsSelected = $("#chkInitialIncludeInsuredObject").prop('checked');
            insuredObjectAdd.IsMandatory = $("#chkMandatoryInsuredObject").prop('checked');
            if (insuredObjectAdd.StatusTypeService !== 2) {
                insuredObjectAdd.StatusTypeService = 3;
            }
            $("#listModalInsuredObjectData").UifListView("editItem", insuredObjectIndex, insuredObjectAdd);
            $.each(glbInsuredObjects, function (index, value) {
                if (this.Id == $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")) {
                    if (value.GroupCoverages != null) {
                        var validateGroupCoverage = value.GroupCoverages.filter(function (item) {
                            return item.Id == $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected");
                        });
                        if (validateGroupCoverage.length > 0) {
                            $.each(value.GroupCoverages, function (key, valueGroupCoverage) {
                                var insuredDelete = glbInsuredObjects[index].GroupCoverages[key].InsuredObjects.filter(function (item) {
                                    return item.StatusTypeService === 4;
                                });
                                if (this.Id == $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")) {
                                    glbInsuredObjects[index].GroupCoverages[key].InsuredObjects = $("#listModalInsuredObjectData").UifListView("getData");
                                    if (glbInsuredObjects[index].GroupCoverages[key].StatusTypeService !== 2) {
                                        glbInsuredObjects[index].GroupCoverages[key].StatusTypeService = 3;
                                    }
                                }
                                if (insuredDelete.length > 0) {
                                    $.each(insuredDelete, function (index, item) {
                                        let existDelete = false;
                                        $.each(glbInsuredObjects[index].GroupCoverages[key].InsuredObjects, function (indexEdit, itemEdit) {
                                            if (itemEdit == item) {
                                                existDelete = true;
                                            }
                                        });
                                        if (!existDelete) {
                                            glbInsuredObjects[index].GroupCoverages[key].InsuredObjects.push(item);
                                        }
                                    });
                                }
                            });

                        }
                        else {
                            glbInsuredObjects[index].GroupCoverages.push(
                                {
                                    Id: parseInt($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")),
                                    Description: $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelectedText"),
                                    RiskTypeId: parseInt($("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")),
                                    InsuredObjects: $("#listModalInsuredObjectData").UifListView("getData"),
                                    Form: null,
                                    StatusTypeService: 2
                                }
                            );
                        }
                    }
                    else {
                        glbInsuredObjects[index].GroupCoverages = [];
                        glbInsuredObjects[index].GroupCoverages.push(
                            {
                                Id: parseInt($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")),
                                Description: $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelectedText"),
                                RiskTypeId: parseInt($("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")),
                                InsuredObjects: $("#listModalInsuredObjectData").UifListView("getData"),
                                Form: null,
                                StatusTypeService: 2
                            }
                        );
                    }
                }
            });
            ClearInsuredObject();
        }
        DisabledControlInsured(true);
    }
}

//VALIDAR SI EL OBJETO YA EXISTE EN LA LISYVIEW
function ValidateInsuredObject(id) {
    var exists = false;
    $.each($("#listModalInsuredObjectData").UifListView('getData'), function (index, value) {
        if (this.Id == id) {
            exists = true;
            return false;
        }
    });
    return exists;
}

//OBJETER LA INFORMACION DEL OBJETO PARA AGREGAR A LA LISTVIEW
function GetInsuredObject() {
    var InsuredObject = {
        Id: parseInt($("#selectModalInsuredObject").UifSelect("getSelected")),
        Description: $("#selectModalInsuredObject").UifSelect('getSelectedText'),
        CoverageGroupId: parseInt($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")),
        RiskTypeId: parseInt($("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")),
        IsSelected: $("#chkInitialIncludeInsuredObject").prop('checked'),
        IsMandatory: $("#chkMandatoryInsuredObject").prop('checked'),
        Coverages: [],
        StatusTypeService: 2
    };
    if (InsuredObject.IsSelected) {
        InsuredObject.InitialInclude = "SI";
    }
    else {
        InsuredObject.InitialInclude = "NO";
    }

    if (InsuredObject.IsMandatory) {
        InsuredObject.Mandatory = "SI";
    }
    else {
        InsuredObject.Mandatory = "NO";
    }

    var insuredObjectSelected = $("#selectModalInsuredObject").UifSelect("getSelectedSource");
    insuredObjectSelected.Description = insuredObjectSelected.Description + " (asignado)";

    //AssignCoverageInsuredObjectList(InsuredObject);
    //AssignInsuredObjectGroupCoverage($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected"), $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelectedText"), $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected"));
    //var riskType = { RiskTypeId: $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected"), Description: $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelectedText"), GroupCoverages: modalCoverageGroupCoverageList };
    //AssignCoverageRiskType(riskType);
    return InsuredObject;
}

//LIMPIAR EL FORMULARIO DE OBJETO DEL SEGURO
function ClearInsuredObject() {
    insuredObjectIndex = null;
    DisabledControlInsured(true);
    $("#chkInitialIncludeInsuredObject").prop('checked', false);
    $("#chkMandatoryInsuredObject").prop('checked', false);
    GetCoverageInsuredObjectByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), 0);
    $("#selectModalInsuredObject").removeData("Object");
}

//SETEAR LA INFORMACION PARA EDITAR OBJETO DEL SEGURO
function SetInsuredObject(insuredObject) {
    $("#selectModalInsuredObjectCoverageRiskType").UifSelect("setSelected", insuredObject.RiskTypeId);
    GetCoverageGroupCoverageByRiskType($("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected"), insuredObject.CoverageGroupId);
    GetCoverageInsuredObjectByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), insuredObject.Id);
    $('#chkInitialIncludeInsuredObject').prop('checked', insuredObject.IsSelected);
    $('#chkMandatoryInsuredObject').prop('checked', insuredObject.IsMandatory);
    $("#selectModalInsuredObject").data("Object", jQuery.extend(true, {}, insuredObject));
}

//Obtener Grupos de cobertura
function GetCoverageGroupCoverageByRiskType(riskTypeId, selectedId) {
    $("#selectModalInsuredObjectGroupCoverage").prop("disabled", false);

    var dataCoverageGroupCoverage = [];

    lockScreen();
    setTimeout(function () {
        InsuredObjectRequestT.GetCoverageGroupCoverageByRiskType(riskTypeId).done(function (data) {
            unlockScreen();
            $.each(data.data, function (index, item) {
                if (glbInsuredObjects.length > 0) {
                    $.each(glbInsuredObjects[0].GroupCoverages, function (indexA, itemA) {
                        if (item.Id == itemA.Id && itemA.StatusTypeService !== 4) {
                            data.data[index].Description = item.Description + " (asignado)";
                        }
                    });
                }
            });
            dataCoverageGroupCoverage = data.data;
        }).fail(function (jqXHR, textStatus, errorThrown) {
            unlockScreen();
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorImportingTemporary, 'autoclose': true })
        });

        $("#selectModalInsuredObjectGroupCoverage").UifSelect({
            sourceData: dataCoverageGroupCoverage,
            id: "Id",
            name: "Description",
            native: false,
            filter: true,
            enable: true
        });

        if (selectedId != 0) {
            $("#selectModalInsuredObjectGroupCoverage").UifSelect("setSelected", selectedId);
        }
        //if (selectedId == 0) {
        //    $("#selectModalInsuredObjectGroupCoverage").UifSelect({ source: controller });
        //}
        //else {
        //    $("#selectModalInsuredObjectGroupCoverage").UifSelect({ source: controller, selectedId: selectedId });
        //}
    }, 200);
}

//Obtener Objetos del seguro
function GetCoverageInsuredObjectByPrefixId(prefixId, selectedId) {
    var insuredObjectsAdd = '';
    var dataInsuredObjects = [];

    InsuredObjectRequestT.GetInsuredObjectsByPrefixId(prefixId, insuredObjectsAdd).done(function (data) {
        $.each(data.data, function (index, item) {
            if (glbInsuredObjects.length > 0) {
                $.each(glbInsuredObjects[0].GroupCoverages, function (indexA, itemA) {
                    var groupCoverageSelected = $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected");
                    if (groupCoverageSelected) {
                        var groupCoverage = glbInsuredObjects[0].GroupCoverages.filter(function (groupCoverage) {
                            return groupCoverage.Id == parseInt($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected"));
                        });
                        if (groupCoverage.length > 0) {
                            $.each(groupCoverage[0].InsuredObjects, function (indexInsuredObj, insuredObject) {
                                if (data.data[index].Id == insuredObject.Id && insuredObject.StatusTypeService !== 4) {
                                    data.data[index].Description = item.Description + " (asignado)";
                                }
                            });
                        }
                    }
                });
            }
        });
        dataInsuredObjects = data.data;
    }).fail(function (jqXHR, textStatus, errorThrown) {
        unlockScreen();
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorImportingTemporary, 'autoclose': true })
    });

    $("#selectModalInsuredObject").UifSelect({
        sourceData: dataInsuredObjects,
        id: "Id",
        name: "Description",
        native: false,
        filter: true,
        enable: true
    });

    if (selectedId != 0) {
        $("#selectModalInsuredObject").UifSelect("setSelected", selectedId);
    }
}

//Obtener los tipos de riesgos
function GetRiskTypeSelected(selectedId) {
    if (Product.ProductCoveredRisks != undefined) {
        var data = Product.ProductCoveredRisks;
        var riskTypeText = $("#selectRiskType").UifSelect("getSelectedText");
        $.each(data, function (index, item) {
            item.Description = riskTypeText
        });
        if (selectedId == 0) {
            $("#selectModalInsuredObjectCoverageRiskType").UifSelect({ sourceData: data, id: "Id", name: "Description" });
        }
        else {
            $("#selectModalInsuredObjectCoverageRiskType").UifSelect({ sourceData: data, id: "Id", name: "Description", selectedId: selectedId });
        }
    }
    //ProductCoveredRisksTemp = [];
    //ProductCoveredRisksTemp = fnClonejQueryArray(Product.ProductCoveredRisks);
    //$.each(ProductCoveredRisksTemp, function (index, item) {
    //    var groupCoverages = this.GroupCoverages;
    //    if (groupCoverages != undefined) {
    //        $.each(groupCoverages, function (index, item) {
    //            var insuredObjects = this.InsuredObjects;
    //            if (insuredObjects != undefined) {
    //                $.each(insuredObjects, function (index, item) {
    //                    AssignCoverageInsuredObjectList(this);
    //                });
    //            }
    //            var coverages = this.Coverages;
    //            if (coverages != undefined) {
    //                $.each(coverages, function (index, item) {
    //                    AssignCoverageCoveragesInsured(this);
    //                });
    //            }
    //            AssignInsuredObjectGroupCoverage(this.Id, this.Description, selectedId);
    //            modalCoverageInsuredObjectsList = [];
    //            modalCoverageCoveragesList = [];
    //        });
    //    }
    //    AssignCoverageRiskType(item);
    //    modalCoverageGroupCoverageList = [];
    //});
}

// ASIGNAR COBERTURAS
function AssignCoverageCoveragesInsured(coverage) {
    var coverageExistList = false;
    $.each(modalCoverageCoveragesList, function (index, item) {
        if (item.Id == coverage.Id) {
            coverageExistList = true;
        }
    });
    if (!coverageExistList) {
        modalCoverageCoveragesList.push(coverage);
    } else {
        $.each(modalCoverageCoveragesList, function (index, item) {
            if (item.Id == coverage.Id) {
                modalCoverageCoveragesList[index].IsSelected = coverage.IsSelected;
                modalCoverageCoveragesList[index].IsMandatory = coverage.IsMandatory;
            }
        });
    }
}

// ASIGNAR OBJETOS DEL SEGURO
function AssignCoverageInsuredObjectList(insuredObject) {
    var insuredObjectExistList = false;
    $.each(modalCoverageInsuredObjectsList, function (index, item) {
        if (item.Id == insuredObject.Id) {
            insuredObjectExistList = true;
            return false;
        }
    });
    if (!insuredObjectExistList) {
        modalCoverageInsuredObjectsList.push({
            Id: insuredObject.Id,
            Description: insuredObject.Description,
            IsSelected: insuredObject.IsSelected,
            IsMandatory: insuredObject.IsMandatory,
            RiskTypeId: insuredObject.RiskTypeId,
            CoverageGroupId: insuredObject.CoverageGroupId
        });
    }
    else {
        $.each(modalCoverageInsuredObjectsList, function (index, item) {
            if (item.Id == insuredObject.Id) {
                modalCoverageInsuredObjectsList[index].IsSelected = insuredObject.IsSelected;
                modalCoverageInsuredObjectsList[index].IsMandatory = insuredObject.IsMandatory;
                return false;
            }
        });
    }
}

// ASIGNAR GRUPOS DE COBERTURAS
function AssignInsuredObjectGroupCoverage(groupCoverageIdSelected, groupCoverageDescriptionSelected, riskTypeIdSelected) {
    if (groupCoverageIdSelected > 0) {
        var groupCoverageExistList = false;
        $.each(modalCoverageGroupCoverageList, function (index, item) {
            if (item.Id == groupCoverageIdSelected) {
                groupCoverageExistList = true;
                return false;
            }
        });
        if (!groupCoverageExistList) {
            modalCoverageGroupCoverageList.push({
                Id: groupCoverageIdSelected,
                Description: groupCoverageDescriptionSelected,
                InsuredObjects: modalCoverageInsuredObjectsList,
                CoveredRiskType: riskTypeIdSelected,
                Coverages: modalCoverageCoveragesList
            });
        } else {
            $.each(modalCoverageGroupCoverageList, function (index, item) {
                if (item.Id == groupCoverageIdSelected) {
                    modalCoverageGroupCoverageList[index].InsuredObjects = modalCoverageInsuredObjectsList;
                    modalCoverageGroupCoverageList[index].Coverages = modalCoverageCoveragesList;
                    return false;
                }
            });
        }
    }
}

//ASIGNAR TIPOS DE RIESGO
function AssignCoverageRiskType(riskType) {
    if (riskType.RiskTypeId > 0) {
        var riskTypeExistList = false;
        $.each(modalCoverageRiskTypeListTemp, function (index, item) {
            if (item.RiskTypeId == riskType.RiskTypeId) {
                riskTypeExistList = true;
                return false;
            }
        });
        if (!riskTypeExistList) {
            modalCoverageRiskTypeListTemp.push(riskType);
        }
        else {
            $.each(modalCoverageRiskTypeListTemp, function (index, item) {
                var indexRisk = index;
                if (item.RiskTypeId == riskType.RiskTypeId) {
                    $.each(modalCoverageGroupCoverageList, function (index, item) {
                        if (modalCoverageRiskTypeListTemp[indexRisk].GroupCoverages != undefined) {
                            if (!validGroup(modalCoverageGroupCoverageList[index].Id)) {
                                modalCoverageRiskTypeListTemp[indexRisk].GroupCoverages.push({
                                    Coverages: modalCoverageGroupCoverageList[index].Coverages,
                                    CoveredRiskType: modalCoverageGroupCoverageList[index].CoveredRiskType,
                                    Description: modalCoverageGroupCoverageList[index].Description,
                                    Id: modalCoverageGroupCoverageList[index].Id,
                                    InsuredObjects: modalCoverageGroupCoverageList[index].InsuredObjects
                                });
                            }
                            else {
                                $.each(modalCoverageRiskTypeListTemp[indexRisk].GroupCoverages, function (indexG, item) {
                                    if (item.Id == modalCoverageGroupCoverageList[index].Id) {
                                        modalCoverageRiskTypeListTemp[indexRisk].GroupCoverages[indexG].InsuredObjects = modalCoverageInsuredObjectsList;
                                    }

                                });
                            }
                        }
                        else {
                            modalCoverageRiskTypeListTemp[indexRisk].GroupCoverages = modalCoverageGroupCoverageList;
                        }
                    });
                    return false;
                }
            });

        }
    }
}

function validGroup(idGroup) {
    var GroupExistList = false;
    $.each(modalCoverageRiskTypeListTemp, function (index, item) {
        var groupCoverage = this.GroupCoverages;
        $.each(groupCoverage, function (index, item) {
            if (item.Id == idGroup) {
                GroupExistList = true;
                return false;
            }
        });
    });
    return GroupExistList;
}

//CARGAR OBJETOS DEL SEGURO
function LoadCoverageInsuredObjectsList() {
    $("#listModalInsuredObjectData").UifListView('refresh');
    var riskTypeId = $('#selectModalInsuredObjectCoverageRiskType').UifSelect("getSelected");
    var groupCoverageId = $('#selectModalInsuredObjectGroupCoverage').UifSelect("getSelected");
    $.each(glbInsuredObjects, function (index, item) {
        if (this.Id == riskTypeId) {
            var groupCoverages = this.GroupCoverages
            if (groupCoverages != null) {
                $.each(groupCoverages, function (index, item) {
                    if (this.Id == groupCoverageId) {
                        var insuredObjects = this.InsuredObjects;

                        $.each(insuredObjects, function (index, item) {
                            if (insuredObjects[index].StatusTypeService !== 4) {
                                if (insuredObjects[index].IsSelected) {
                                    insuredObjects[index].InitialInclude = "SI"
                                }
                                else {
                                    insuredObjects[index].InitialInclude = "NO"
                                }
                                if (insuredObjects[index].IsMandatory) {
                                    insuredObjects[index].Mandatory = "SI"
                                }
                                else {
                                    insuredObjects[index].Mandatory = "NO"
                                }
                                if (insuredObjects[index].CoverageGroupId === 0) {
                                    insuredObjects[index].CoverageGroupId = parseInt(groupCoverageId);
                                }

                                if (insuredObjects[index].RiskTypeId === 0) {
                                    insuredObjects[index].RiskTypeId = parseInt(riskTypeId);
                                }

                                $("#listModalInsuredObjectData").UifListView("addItem", insuredObjects[index]);
                            }

                        });
                    }
                });
            }
        }
    });
}

//LIMPIAR FORMULARIO
function ClearForm() {
    insuredObjectIndex = null;
    $("#selectModalInsuredObjectGroupCoverage").UifSelect();
    $("#selectModalInsuredObjectCoverageRiskType").UifSelect();
    $("#selectModalInsuredObject").UifSelect();
    EnabledDisabledChecksInsuredObject(false);
    DisabledControlInsured(true);
    $("#listModalInsuredObjectData").UifListView('refresh');
}
function EnabledDisabledControlsInsuredObject(control) {
    $("#selectModalInsuredObjectCoverageRiskType").prop("disabled", control);
    $("#selectModalInsuredObjectGroupCoverage").prop("disabled", control);
}
function EnabledDisabledChecksInsuredObject(control) {
    $("#chkInitialIncludeInsuredObject").prop('checked', control);
    $("#chkMandatoryInsuredObject").prop('checked', control);
}

function GetCoverageRiskTypeByPrefixId(prefixId) {
    modalCoverageInfoRiskTypes = [];

    InsuredObjectRequestT.GetCoverageRiskTypeByPrefixId(prefixId).done(function (data) {
        if (data.success) {
            $.each(data.result, function (index, item) {
                modalCoverageInfoRiskTypes.push({
                    riskTypeId: item.Id,
                    riskTypeDescription: item.Description,
                    maxRiskQuantity: item.MaxRiskQuantity
                });
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    });
}

//Consulta si el objeto del seguro existe en el producto y este ya esta en uso
function ExistInsuredObjectProduct(productId, groupCoverageId, insuredObjectId) {
    var result = true;

    InsuredObjectRequestT.ExistInsuredObjectProduct(productId, groupCoverageId, insuredObjectId, $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected")).done(function (data) {
        if (data.success) {
            if (data.result) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDeleteInsuredObject, 'autoclose': true });
            }
            else {
                result = data.result;
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetInsuredObject, 'autoclose': true });
    });


    return result;
}
function DisabledControlInsured(control) {
    $("#btnModalInsuredObjectAssignCoverage").prop("disabled", control);
}
function ValidateDataInsuredObject() {
    var msj = "";
    //ValidatePolicyByProductId();
    //if (validatePolicy != null && validatePolicy == true & ($("#chkInitialIncludeInsuredObject").is(':checked') || $("#chkInitialIncludeInsuredObject").is(':checked'))) {
    //    msj = Resources.Language.MessageInsuredObjectInUse + "<br>"
    //}
    if ($("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected") == null || $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected") == "") {

        msj = msj + Resources.Language.LabelGroupCoverage + "<br>"
    }
    if ($("#selectModalInsuredObject").UifSelect("getSelected") == null || $("#selectModalInsuredObject").UifSelect("getSelected") == "") {

        msj = msj + Resources.Language.LabelInsuredObject + "<br>"
    }
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    return true;
}

function ajaxGetCoveredRiskByProductId(productId) {
    return InsuredObjectRequestT.ajaxGetCoveredRiskByProductId(productId);
}

function ValidatePolicyByProductId() {
    InsuredObjectRequestT.ValidatePolicyByProductId(Product.Id, $("#selectModalInsuredObjectCoverageRiskType").UifSelect("getSelected"), $("#selectModalInsuredObjectGroupCoverage").UifSelect("getSelected")).done(function (response) {
        if (response.success) {
            validatePolicy = response.result;
        }
    }).error(function (data) {
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al ejecutar transacción' });
    });
}

//function insuredObject_getCoveredRisk() {
//    if (Product.Id > 0 && Product.ProductCoveredRisks[0].GroupCoverages.length == 0) {
//        glbProductId = Product.Id;
//        var getCoveredRiskByProductId = ajaxGetCoveredRiskByProductId(Product.Id);
//        getCoveredRiskByProductId.done(function (data) {
//            if (ObjectProduct.successAjax(data)) {
//                if (typeof Product.ProductCoveredRisks != 'undefined' && Product.ProductCoveredRisks.length > 0) {
//                    $.each(Product.ProductCoveredRisks, function (key, value) {
//                        this.GroupCoverages = data.result.GroupCoverages;
//                        this.RiskTypeId = value.CoveredRiskType;
//                        this.Description = data.result.ProductCoveredRisks[key].Description;
//                    });
//                    Product.ProductOld.GroupCoverages = data.result.GroupCoverages;
//                    if (Product.ProductOld.GroupCoverages != null) {
//                        $.each(Product.ProductOld.GroupCoverages, function (key, item) {
//                            item.Product = null;
//                            if (item.Coverages != null) {
//                                $.each(item.Coverages, function (keyCoverage, itemCoverage) {
//                                    itemCoverage.CurrentFrom = FormatDate(itemCoverage.CurrentFrom);
//                                    itemCoverage.CurrentTo = FormatDate(itemCoverage.CurrentTo);

//                                    Product.ProductOld.GroupCoverages[key].Coverages[keyCoverage].CurrentFrom = FormatDate(itemCoverage.CurrentFrom);
//                                    Product.ProductOld.GroupCoverages[key].Coverages[keyCoverage].CurrentTo = FormatDate(itemCoverage.CurrentTo);

//                                    if (itemCoverage.CoverageAllied != null) {
//                                        $.each(itemCoverage.CoverageAllied, function (indexAllied, Allied) {
//                                            Allied.CurrentFrom = FormatDate(Allied.CurrentFrom);
//                                            Allied.CurrentTo = FormatDate(Allied.CurrentTo);

//                                            Product.ProductOld.GroupCoverages[key].Coverages[keyCoverage].CoverageAllied[indexAllied].CurrentTo = FormatDate(Allied.CurrentTo);
//                                            Product.ProductOld.GroupCoverages[key].Coverages[keyCoverage].CoverageAllied[indexAllied].CurrentFrom = FormatDate(Allied.CurrentFrom);
//                                        });
//                                    }
//                                });
//                            }
//                        });
//                    }
//                }
//            }
//        }).fail(function (jqXHR, textStatus, errorThrown) {
//            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryCoveredRiskByProductId, 'autoclose': true })
//        });
//    }
//}
