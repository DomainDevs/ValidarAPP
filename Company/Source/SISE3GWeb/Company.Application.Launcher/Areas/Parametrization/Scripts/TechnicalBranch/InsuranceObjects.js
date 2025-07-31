$(() => {
    new ParametrizationInsuranceObjects();
});

class ParametrizationInsuranceObjects extends Uif2.Page {
    getInitialState() {

    }

    bindEvents() {
        $("#btnModalInsuranceobjectsTechnicalBranchAssignAll").on("click", this.CopyAllInsurenceObjects);
        $("#btnModalInsuranceobjectsTechnicalBranchDeallocateAll").on("click", this.DeallocateAllInsurenceObjects);
        $("#btnModalInsuranceobjectsTechnicalBranchAssign").on("click", this.CopyInsuranceObject);
        $("#btnModalInsuranceobjectsTechnicalBranchDeallocate").on("click", this.DeallocateInsuranceObject);
        $('#btnInsuranceObjects').on('click', ParametrizationInsuranceObjects.InsuranceObjects);
        $("#btnSaveObjects").on("click", this.SaveInsuranceObject);
    }

    static ShowModal() {

    }
    static InsuranceObjects() {
        var indexObject = $('#inputTechnicalBranchCode').val();
        if (indexObject != "") {
            ParametrizationInsuranceObjects.LoadInsuranceObjectsByIndex(indexObject);
        }
       
    }

    static LoadInsuranceObjects(url) {
        return $.ajax({
            type: 'POST',
            url: url,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static LoadInsuranceObjectsByIndex(index) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/TechnicalBranch/GetObjectByLineBusinessId?idLineBusiness=' + index,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
            if (data.success) {
                if (listAsigned.length == undefined || $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData').length == 0
                    || $("#listviewInsuranceobjectsTechnicalBranch").UifListView('getData').length == 0)
                    listAsigned = data.result;
                else listAsigned = $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData');

                ParametrizationInsuranceObjects.LoadInsuranceObjects('GetAllInsuranceObjects').done(function (dataInsured) {
                    if (dataInsured.success) {

                        $("#listviewInsuranceobjectsTechnicalBranch").UifListView({ displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
                        $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView({ source: null, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
                        $.each(dataInsured.result, function (key, value) {
                            var insuranceObject =
                                {
                                    Description: this.Description,
                                    Id: this.Id
                                }
                            $("#listviewInsuranceobjectsTechnicalBranch").UifListView("addItem", insuranceObject);
                        });
                        if (listAsigned != null && listAsigned != Resources.Language.ErrMsgRangeEntityNotFound) {
                            $.each(listAsigned, function (key, value) {
                                var insuranceObjectAsg =
                                    {
                                        Description: this.Description,
                                        Id: this.Id,
                                    };
                                var findObject = function (element, index, array) {
                                    return element.Id === insuranceObjectAsg.Id
                                }
                                var index = $("#listviewInsuranceobjectsTechnicalBranch").UifListView("findIndex", findObject);
                                $("#listviewInsuranceobjectsTechnicalBranch").UifListView("deleteItem", index);
                                $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView("addItem", insuranceObjectAsg);
                            });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                    $('#ModalInsuranceObjects').UifModal('showLocal', Resources.Language.LabelInsuredObjects);
                    ParametrizationTechnicalBranch.countModal();
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    CopyAllInsurenceObjects() {
        var lstObjects = $("#listviewInsuranceobjectsTechnicalBranch").UifListView('getData');
       
        if (lstObjects.length > 0) {
            $.each($("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData'), function (key, value) {
                lstObjects.push(value)
            });
            lstObjects = $("#listviewPaymentPlanAssing").UifListView('getData').concat(lstObjects);
            $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView({ sourceData: lstObjects, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
            $("#listviewInsuranceobjectsTechnicalBranch").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        }
    }

    DeallocateAllInsurenceObjects() {
        var lstObjects = $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData');

        if (lstObjects.length > 0) {
            lstObjects = $("#listviewInsuranceobjectsTechnicalBranch").UifListView('getData').concat(lstObjects);
            $("#listviewInsuranceobjectsTechnicalBranch").UifListView({ sourceData: lstObjects, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
            $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView({ displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
        }
    }

    CopyInsuranceObject() {
        if ($("#listviewInsuranceobjectsTechnicalBranch").UifListView('getData').length > 0) {
            $("#listviewInsuranceobjectsTechnicalBranchAssing").prop("disabled", true);
            try {
                ParametrizationInsuranceObjects.CopyPaymentPlanSelected($("#listviewInsuranceobjectsTechnicalBranch").UifListView('getSelected'))
            } catch (e) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMovingPaymentPlan, 'autoclose': true });
            } finally {
                $("#listviewInsuranceobjectsTechnicalBranchAssing").prop("disabled", false);
            }
        }
    }

    DeallocateInsuranceObject() {
        if ($("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData').length > 0) {
            ParametrizationInsuranceObjects.DeallocatePaymentPlanSelect($("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getSelected'))
        }
    }

    SaveInsuranceObject() {
        var lstInsuranceObjects = $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData');

        linebusiness.ListIsnsuranceObjects = null;
        if (lstInsuranceObjects != null && lstInsuranceObjects.length > 0) {
            var data = [];
            $.each(lstInsuranceObjects, function (key, value) {
                data.push(value.Id);
            });
            linebusiness.ListIsnsuranceObjects = data;
            $("#ModalInsuranceObjects").UifModal('hide');
            $("#selectedCoInsurance").text(lstInsuranceObjects.length);
        }
    }

    static CopyPaymentPlanSelected(data) {
        var objectsAsign = $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView('getData');
        $.each(data, function (index, data) {
            var findObject = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listviewInsuranceobjectsTechnicalBranch").UifListView("findIndex", findObject);
            $("#listviewInsuranceobjectsTechnicalBranch").UifListView("deleteItem", index);
        });
        objectsAsign = objectsAsign.concat(data);
        $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView({ sourceData: objectsAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
    }

    static DeallocatePaymentPlanSelect(data) {
        var objectsNoAsign = $("#listviewInsuranceobjectsTechnicalBranch").UifListView('getData');
        $.each(data, function (index, data) {
            var findObject = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView("findIndex", findObject);
            $("#listviewInsuranceobjectsTechnicalBranchAssing").UifListView("deleteItem", index);
        });
        objectsNoAsign = objectsNoAsign.concat(data);
        $("#listviewInsuranceobjectsTechnicalBranch").UifListView({ sourceData: objectsNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
    }
}