var itemIndex = null
var glbDetailDelete = [];
class DetailParametrization extends Uif2.Page {
    getInitialState() {
        $("#listDetail").UifListView({
            displayTemplate: "#DetailTemplate",
            source: null,
            selectionType: 'single',
            height: 400
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //Se cargan los datos en los campos iniciales
        request('Parametrization/Detail/GetDetailTypes', null, 'GET', Resources.Language.ErrorSearch, DetailParametrization.GetDetailType);
        request('Parametrization/Detail/GetRateTypes', null, 'GET', Resources.Language.ErrorSearch, DetailParametrization.GetRateTypes);
        request('Parametrization/Detail/GetDetails', null, 'GET', AppResources.ErrorSearchDetails, DetailParametrization.LoadDetails);
        DetailParametrization.ClearForm()
        $("#inputSubLimitAmount").OnlyDecimals(2);
        $("#inputRate").OnlyDecimals(4);
    }

    bindEvents() {
        $("#btnNew").on("click", DetailParametrization.ClearForm)
        $("#btnAccept").on("click", DetailParametrization.AddItem);
        $("#listDetail").on('rowEdit', DetailParametrization.ShowData);
        $("#btnExit").on("click", DetailParametrization.Exit);
        $("#btnExport").click(this.exportExcel);
        $("#btnSave").on("click", DetailParametrization.Save);

        $('#inputSubLimitAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputSubLimitAmount').focusout(function (event) {
            event.stopPropagation();
            if (parseFloat(NotFormatMoney($(this).val())) > 100) {
                $(this).val("100,00");
            }
            $(this).val(FormatMoney($(this).val()));
        });

    }

    exportExcel() {
        request('Parametrization/Detail/GenerateFileToExport', null, 'GET', AppResources.ErrorSearchDetails, DetailParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    //EVENTOS CONTROLES
    static GetDetailType(data) {
        $("#selectDetailType").UifSelect({ sourceData: data });
    }
    static GetRateTypes(data) {
        $("#selectRateType").UifSelect({ sourceData: data });
    }

    static LoadDetails(details) {
        if (details.ErrorDescription != undefined) {
            $.UifNotify('show', { 'type': 'danger', 'message': details.ErrorDescription[0], 'autoclose': true });
        }
        else {
            $("#listDetail").UifListView({
                displayTemplate: "#DetailTemplate",
                sourceData: details,
                selectionType: 'single',
                edit: true,
                delete: true,
                customEdit: true,
                height: 400,
                deleteCallback: DetailParametrization.deleteCallbackList
            });
        }
    }

    static deleteCallbackList(deferred, result) {
        deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined && result.Id !== "0" && result.Status == 1) //Se elimina unicamente si existe en DB
        {
            result.Status = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listDetail").UifListView("addItem", result);
        }
        
    }

    static ResultSave(data) {
        if (Array.isArray(data.data)) {
            $.UifNotify('show', { 'type': 'info', 'message': data.message, 'autoclose': true });
            glbDetailDelete = [];
            DetailParametrization.LoadDetails(data.data);
        }
    }

    static ShowData(event, result, index) {
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }

        if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            itemIndex = index;
            $("#inputId").val(resultUnique.Id);
            $("#inputIdByType").val(resultUnique.IdByType);
            if (resultUnique.Enabled == true) {
                $('#chkEnabled').prop('checked', true);
            }
            else {
                $('#chkEnabled').prop('checked', false);
            }
            $("#selectDetailType").UifSelect("setSelected", result.DetailTypeId);
            $("#inputRate").val(resultUnique.Rate);
            $("#inputDescription").val(resultUnique.Description);
            if (parseFloat(NotFormatMoney(resultUnique.SublimitAmt)) > 100) {
                resultUnique.SublimitAmt = "100";
            }
            $("#inputSubLimitAmount").val(FormatMoney(resultUnique.SublimitAmt));
            $("#selectRateType").UifSelect("setSelected", result.RateTypeId);
            ClearValidation("#formDetail");
        }
    }

    static ClearForm() {
        itemIndex = null;
        $("#selectDetailType").UifSelect('setSelected', null);
        $("#inputDescription").val(null);
        $("#inputSubLimitAmount").val("0");
        $("#chkEnabled").val(null);
        $("#selectRateType").UifSelect('setSelected', null);
        $("#inputRate").val("0");
        $("#inputId").val("0");
        $("#inputIdByType").val("0");
        $('#chkEnabled').prop('checked', false);
        ClearValidation("#formDetail");
    }

    static AddItem() {
        $("#formDetail").validate();
        if ($("#formDetail").valid()) {
            let data = $("#formDetail").serializeObject();
            data.Id = parseInt($("#inputId").val());
            data.IdByType = parseInt($("#inputIdByType").val());
            data.Description = $("#inputDescription").val();
            data.DetailTypeId = parseInt($("#selectDetailType").UifSelect("getSelected"));
            if ($('#chkEnabled').prop("checked")) {
                data.Enabled = true;
                data.EnabledDescription = AppResources.LabelIf;
            }
            else {
                data.Enabled = false;
                data.EnabledDescription = AppResources.LabelNot;
            }

            data.Rate = $("#inputRate").val();
            if ($("#selectRateType").UifSelect("getSelected") != "") {
                data.RateTypeDescription = $("#selectRateType").UifSelect("getSelectedText");
                data.RateTypeId = parseInt($("#selectRateType").UifSelect("getSelected"));
            }
            if ($("#inputSubLimitAmount").val() != "") {
                data.SublimitAmt = NotFormatMoney($("#inputSubLimitAmount").val());
            }           
            data.TypeDescription = $("#selectDetailType").UifSelect("getSelectedText");

            var list = $("#listDetail").UifListView('getData');

            if (itemIndex == null) {
                var ifExist = list.filter(function (item) {
                    return (item.Description.toLowerCase().sistranReplaceAccentMark() == data.Description.toLowerCase().sistranReplaceAccentMark()
                        && item.DetailTypeId == data.DetailTypeId);
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistDetailPreCharge, 'autoclose': true });
                }
                else {
                    data.Status = ParametrizationStatus.Create;
                    $("#listDetail").UifListView("addItem", data);
                    DetailParametrization.ClearForm();
                }
            }
            else {
                var ifExist = list.filter(function (item) {
                    return (item.Description.toLowerCase().sistranReplaceAccentMark() == data.Description.toLowerCase().sistranReplaceAccentMark()
                        && item.DetailTypeId == data.DetailTypeId
                        && item.Id != data.Id);
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistDetailPreCharge, 'autoclose': true });
                }
                else {
                    if (data.Id != 0) {
                        data.Status = ParametrizationStatus.Update;
                    }
                    else {
                        data.Status = ParametrizationStatus.Create;
                    }
                    $("#listDetail").UifListView('editItem', itemIndex, data);
                    DetailParametrization.ClearForm();
                }
            }
        }
    }

    static Save() {
        var itemModified = [];
        var details = $("#listDetail").UifListView('getData');
        $.each(details, function (index, value) {
            if (value.Status != undefined && value.Status != ParametrizationStatus.Original) {
                itemModified.push(value);
            }
        });
        if (glbDetailDelete.length > 0) {
            $.each(glbDetailDelete, function (index, value) {
                itemModified.push(value);
            });
        }
        if (itemModified.length > 0) {
            request('Parametrization/Detail/ExecuteOperation', JSON.stringify({ details: itemModified }), 'POST', AppResources.ErrorSearchDetails, DetailParametrization.ResultSave);
        }
    }
}
