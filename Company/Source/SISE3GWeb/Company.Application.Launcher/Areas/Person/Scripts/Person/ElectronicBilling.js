var rowElectronicBillingId = -1;
var ElectronicBillingId = 0;
var heightListViewElectronicBilling = 256;
var insuredId = 0;
var enableDeleteFiscalResponsibility = false;

class ElectronicBilling extends Uif2.Page {

    getInitialState() {

        if ($("#btnAcceptElectronicBilling").length > 0)
        {
            enableDeleteFiscalResponsibility = true;
        }
        else
        {
            enableDeleteFiscalResponsibility = false;
        }
        $("#listElectronicBilling").UifListView({
            //localMode: true,
            sourceData: null,//source: null,
            height: 430,
            displayTemplate: '#electronicBillingTemplate',
            add: false,
            edit: false,
            delete: enableDeleteFiscalResponsibility,
            customAdd: false,
            customEdit: false,
            customDelete: true
        });
        ElectronicBilling.loadFiscalResponsibility();                 
    }

    bindEvents() {

        $("#btnElectronicBillingAdd").click(this.startAddListElectronicBilling);
        $("#btnAcceptElectronicBilling").click(this.SaveElectronicBilling);
        $("#btnElectronicBilling").click(ElectronicBilling.loadElectronicBilling);
        $('#listElectronicBilling').on('rowEdit', this.ElectronicEdit);
        $('#listElectronicBilling').on('rowDelete', this.listElectronicBillingDelete);
        $("#btnElectronicBillingNew").click(ElectronicBilling.ClearControlElectronicBilling);

    }

    ElectronicEdit(event, data, index) {
        rowElectronicBillingId = index;
        ElectronicBillingId = data.Id;
        ElectronicBilling.EditElectronicBilling(data, index);
    }

    static EditElectronicBilling(data) {
        if (data.Id > 0) {
            ElectronicBillingRequest.GetFiscalResponsibilityId(data.FiscalResponsibilityId).done(function (dataResult) {
                if (dataResult.success) {
                    $("#selectFiscalResponsibility").UifSelect("setSelected", dataResult.result.Id);

                }
            });
        }

    }


    static loadFiscalResponsibility() {
        ElectronicBillingRequest.GetFiscalResponsibility().done(function (data) {
            if (data.success) {
                $("#selectFiscalResponsibility").UifSelect({ sourceData: data.result });
            }
        });        
    }


    startAddListElectronicBilling() {

        var listElectronicBilling = $("#listElectronicBilling").UifListView("getData");
      

        if (ElectronicBilling.ValidateElectronicBilling()) {

            var fiscalResponsibilityId = parseInt($("#selectFiscalResponsibility").UifSelect("getSelected"));

            var listElectronicBillingFilterRepeat = listElectronicBilling.filter(function (item) {
                return parseInt(item.FiscalResponsibilityId) == parseInt(fiscalResponsibilityId);
            });

            if (listElectronicBillingFilterRepeat.length > 0) {
                $.UifNotify('show', { 'type': 'info', 'message': 'La responsabilidad fiscal ya se encuentra asignada.', 'autoclose': true });
                return false;
            }
            if (fiscalResponsibilityId > 0) {
                ElectronicBilling.CreateElectronicBilling();
                ElectronicBilling.ClearControlElectronicBilling();
            }
        }        
    }

    static CreateElectronicBilling() {
        if (ElectronicBillingId == 0) {
            ElectronicBillingId = 0;
        }
        var ElectronicBillingTmp = this.CreateElectronicBillingModel();

        if (rowElectronicBillingId === -1) {
            $("#listElectronicBilling").UifListView("addItem", ElectronicBillingTmp);
        }
        else {
            $("#listElectronicBilling").UifListView("editItem", rowElectronicBillingId, ElectronicBillingTmp);

        }

    }


    static CreateElectronicBillingModel() {
        var ElectronicBillingModel = {};
        ElectronicBillingModel.Id = ElectronicBillingId !== 0 ? ElectronicBillingId : 0;
        ElectronicBillingModel.IndividualId = parseInt($('#lblPersonCode').val()) || parseInt($('#lblCompanyCode').val());
        ElectronicBillingModel.InsuredId = 0;
        ElectronicBillingModel.ResponsibleForVat = $("#ResponsibleForVat").is(':checked');
        ElectronicBillingModel.NotResponsibleForVat = $("#NotResponsibleForVat").is(':checked');
        ElectronicBillingModel.FiscalResponsibilityId = $("#selectFiscalResponsibility").UifSelect("getSelected");
        ElectronicBillingModel.FiscalResponsibilityDescription = $("#selectFiscalResponsibility").UifSelect("getSelectedText");

        return ElectronicBillingModel;
    }

    static ClearControlElectronicBilling() {
        rowElectronicBillingId = -1;
        ElectronicBillingId = 0;
        $("#selectFiscalResponsibility").UifSelect("setSelected", null);

    }

    static ClearListControlElectronicBilling() {
        rowElectronicBillingId = -1;
        ElectronicBillingId = 0;
        $("#selectFiscalResponsibility").UifSelect("setSelected", null);
        $("#listElectronicBilling").UifListView("refresh");
        $("#listElectronicBilling").UifListView("clear");



    }
    static ValidateElectronicBilling() {

        var msj = "";

        if ($("#selectFiscalResponsibility").UifSelect("getSelected") == null || $("#selectFiscalResponsibility").UifSelect("getSelected") == "")
        {
            msj = msj + AppResourcesPerson.MessageFiscalResponsibilityEmpty + " <br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true });
            return false;
        } else
        {
            return true;
        }

    }

    SaveElectronicBilling() {

        var data = $("#listElectronicBilling").UifListView("getData");


        if (data.length > 0) {
            var responsibleForVat;
            var electronicBiller;
            if ($('#ResponsibleForVat').is(':checked') == true) {
                responsibleForVat = true;
            } else {
                responsibleForVat = false;
            }

            boolRegimeType = responsibleForVat;

            if ($('#rdElectronicBiller').is(':checked') == true) {
                electronicBiller = true;
            } else {
                electronicBiller = false;
            }

            boolElectronicBiller = electronicBiller;
            $("#btnAcceptElectronicBilling").prop('disabled', true);
            ElectronicBillingRequest.SaveElectronicBilling(data, responsibleForVat, electronicBiller).done(function (result) {
                if (result.success && result.result != 'Error to create Electronic Billing') {
                    $("#modalElectronicBilling").UifModal("hide");
                    $.UifNotify('show', { 'type': 'info', 'message': 'Guardado correctamente', 'autoclose': true });
                } else {
                    $("#modalElectronicBilling").UifModal("hide");
                    $.UifNotify('show', { 'type': 'info', 'message': result.result, 'autoclose': true });
                }
                ElectronicBilling.ClearListControlElectronicBilling();
                unlockScreen()

            }).always(function () {
                $("#btnAcceptElectronicBilling").prop('disabled', false);
            });
        }

        else {
            $("#modalElectronicBilling").UifModal("hide");
            $.UifNotify('show', { 'type': 'info', 'message': 'Lista vacía', 'autoclose': true });
        }
    }

    static LoadListView() {        
        var insured = $('#lblCompanyCode').val() == undefined ? $('#lblPersonCode').val() : $('#lblCompanyCode').val();
        if ($("#btnAcceptElectronicBilling").length > 0) {
            enableDeleteFiscalResponsibility = true;
        }
        else {
            enableDeleteFiscalResponsibility = false;
        }
        if (insured != "") {
            ElectronicBillingRequest.GetCompanyElectronicBillingByIndividualId(parseInt(insured)).done(function (data) {
                $("#listElectronicBilling").UifListView({
                    sourceData: data.result,
                    height: 430,
                    displayTemplate: '#electronicBillingTemplate',
                    add: false,
                    edit: false,
                    delete: enableDeleteFiscalResponsibility,
                    customAdd: false,
                    customEdit: false,
                    customDelete: true
                });
            });
        }

    }

    static loadElectronicBilling() {
        $('#rdElectronicBiller').prop("checked", true)
        if (TypePerson.PersonNatural === parseInt($('#selectSearchPersonType').UifSelect("getSelected"))) {

            ElectronicBillingRequest.GetRegimeTypeId(parseInt($('#lblPersonCode').val())).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.RegimeType == true) {
                            $('#ResponsibleForVat').prop("checked", true);
                            $('#NotResponsibleForVat').prop("checked", false);
                        } else {
                            $('#ResponsibleForVat').prop("checked", false);
                            $('#NotResponsibleForVat').prop("checked", true);
                        }

                        if (data.result.ElectronicBiller == true) {
                            $('#rdElectronicBiller').prop("checked", true)
                        } else {
                            $('#rdElectronicBiller').prop("checked", false)
                        }
                        ElectronicBilling.LoadListView();
                        //llamado

                    }
                }

            });

        }
        else if (TypePerson.PersonLegal === parseInt($('#selectSearchPersonType').UifSelect("getSelected"))) {

            ElectronicBillingRequest.GetRegimeTypeId(parseInt($('#lblCompanyCode').val())).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.RegimeType == true) {
                            $('#ResponsibleForVat').prop("checked", true);
                            $('#NotResponsibleForVat').prop("checked", false);
                        } else {
                            $('#ResponsibleForVat').prop("checked", false);
                            $('#NotResponsibleForVat').prop("checked", true);
                        }

                        if (data.result.ElectronicBiller == true) {
                            $('#rdElectronicBiller').prop("checked", true)
                        } else {
                            $('#rdElectronicBiller').prop("checked", false)
                        }
                        ElectronicBilling.LoadListView();
                    }

                }

            });


        }

    }

    listElectronicBillingDelete(event, data) {
        if ($('#listElectronicBilling').UifListView("getData") != null && $('#listElectronicBilling').UifListView("getData").length > 1) {
            $.UifDialog('confirm', { 'message': Resources.ConfirmDelete, 'title': Resources.DeleteRecord }, function (result) {
                if (result) {
                    ElectronicBilling.DeleteInsuredFiscalResponsibility(data);
                }
            });            
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteIdFiscalResponsibility, 'autoclose': true });
        }
    }

    static DeleteInsuredFiscalResponsibility(data) {
        const index = $("#listElectronicBilling").UifListView("findIndex", (x) => { return x.Id === data.Id });
        if (data.IndividualId > 0) {
            ElectronicBillingRequest.DeleteElectronicBilling(data).done(function (dataresult) {
                if (dataresult.success) {
                    $("#listElectronicBilling").UifListView("deleteItem", index);

                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.DeleteInsuredFiscalResponsibility, 'autoclose': true });
                }
                else {
                    $("#listElectronicBilling").UifListView("deleteItem", index);
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.DeleteInsuredFiscalResponsibility, 'autoclose': true });
                }
            });

        }

    }

}