var itemIndex = null
var glbGroupDelete = [];
$.ajaxSetup({ async: true });

class CoverageGroupParametrization extends Uif2.Page {
    getInitialState() {
        $("#listCoverGroup").UifListView({
            displayTemplate: "#CoverGroupTemplate",
            source: null,
            selectionType: 'single',
            height: 400
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //Se cargan los datos en los campos iniciales
        request('Parametrization/CoverageGroup/GetCoveredRiskType', null, 'GET', Resources.Language.ErrorSearch, CoverageGroupParametrization.GetCoveredRiskType);
        request('Parametrization/CoverageGroup/GetCoverageGroupAll', null, 'GET', AppResources.ErrorSearch, CoverageGroupParametrization.LoadGroups);
        CoverageGroupParametrization.ClearForm();
    }

    bindEvents() {
        $("#btnNew").on("click", CoverageGroupParametrization.ClearForm);
        $("#btnAccept").on("click", CoverageGroupParametrization.AddItem);
        $("#listCoverGroup").on('rowEdit', CoverageGroupParametrization.ShowData);
        $("#btnExit").on("click", CoverageGroupParametrization.Exit);
        $("#btnExport").click(this.exportExcel);
        $("#btnSave").click(CoverageGroupParametrization.Save);
    }

    exportExcel() {
        request('Parametrization/CoverageGroup/GenerateFileToExport', null, 'GET', AppResources.ErrorQueryCoverageGroups, CoverageGroupParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    //EVENTOS CONTROLES
    static GetCoveredRiskType(data) {
        $("#selectRiskType").UifSelect({ sourceData: data });
    }

    static LoadGroups(details) {
        $("#listCoverGroup").UifListView({
            displayTemplate: "#CoverGroupTemplate",
            sourceData: details,
            selectionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: CoverageGroupParametrization.deleteCallbackList
        });
    }

    static ResultSave(data) {
        if (Array.isArray(data.data)) {
            $.UifNotify('show', { 'type': 'info', 'message': data.message, 'autoclose': true });
            glbGroupDelete = [];
            CoverageGroupParametrization.LoadGroups(data.data);
        }
    }

    static deleteCallbackList(deferred, result) {
        deferred.resolve();
        if (result.IdCoverGroupRisk !== undefined && result.IdCoverGroupRisk !== "" && result.IdCoverGroupRisk !== "0" && result.Status == 1) //Se elimina unicamente si existe en DB
        {
            result.Status = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listCoverGroup").UifListView("addItem", result);
        }
        
        CoverageGroupParametrization.ClearForm();
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
            $("#IdCoverGroupRisk").val(resultUnique.IdCoverGroupRisk);
            $("#CoverageGroupCode").val(resultUnique.CoverageGroupCode);
            if (resultUnique.Enabled == true) {
                $('#chkEnabled').prop('checked', true);
            }
            else {
                $('#chkEnabled').prop('checked', false);
            }
            $("#selectRiskType").UifSelect("setSelected", resultUnique.CoveredRiskTypeCode);
            $("#inputDescription").val(resultUnique.Description);
            $("#inputShortDescription").val(resultUnique.SmallDescription);
            ClearValidation("#formGroup");
        }
    }

    static ClearForm() {
        itemIndex = null;
        $("#selectRiskType").UifSelect('setSelected', null);
        $("#inputShortDescription").val("");
        $("#inputDescription").val("");
        $("#IdCoverGroupRisk").val("0");
        $("#CoverageGroupCode").val("0");
        $('#chkEnabled').prop('checked', false);
        ClearValidation("#formGroup");
    }

    static AddItem() {
        $("#formGroup").validate();
        if ($("#formGroup").valid()) {
            let data = $("#formGroup").serializeObject();

            data.IdCoverGroupRisk = parseInt($("#IdCoverGroupRisk").val());
            data.CoverageGroupCode = parseInt($("#CoverageGroupCode").val());
            data.RiskTypeCoverageDescription = $("#selectRiskType").UifSelect('getSelectedText');
            if ($('#chkEnabled').prop("checked")) {
                data.Enabled = true;
            }
            else {
                data.Enabled = false;
                data.EnabledDescription = AppResources.LabelIf;
            }
            var list = $("#listCoverGroup").UifListView('getData');

            if (itemIndex == null) {
                var ifExist = list.filter(function (item) {
                    return (item.Description.toLowerCase().sistranReplaceAccentMark() == data.Description.toLowerCase().sistranReplaceAccentMark()
                        && item.DetailTypeId == data.DetailTypeId);
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistCoverageGroup, 'autoclose': true });
                }
                else {
                    data.Status = ParametrizationStatus.Create;
                    $("#listCoverGroup").UifListView("addItem", data);
                    CoverageGroupParametrization.ClearForm();
                }
            }
            else {
                var ifExist = list.filter(function (item) {
                    return (item.Description.toLowerCase().sistranReplaceAccentMark() == data.Description.toLowerCase().sistranReplaceAccentMark()
                        && item.DetailTypeId == data.DetailTypeId
                        && item.IdCoverGroupRisk != data.IdCoverGroupRisk);
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistCoverageGroup, 'autoclose': true });
                }
                else {
                    if (data.IdCoverGroupRisk != 0) {
                        data.Status = ParametrizationStatus.Update;
                    }
                    else {
                        data.Status = ParametrizationStatus.Create;
                    }
                    $("#listCoverGroup").UifListView('editItem', itemIndex, data);
                    CoverageGroupParametrization.ClearForm();
                }
            }
        }
    }

    static Save() {
        var itemModified = [];
        var details = $("#listCoverGroup").UifListView('getData');
        $.each(details, function (index, value) {
            if (value.Status != undefined && value.Status != ParametrizationStatus.Original) {
                itemModified.push(value);
            }
        });
        if (glbGroupDelete.length > 0) {
            $.each(glbGroupDelete, function (index, value) {
                itemModified.push(value);
            });
        }
        if (itemModified.length > 0) {
            request('Parametrization/CoverageGroup/ExecuteOperation', JSON.stringify({ coverageGroups: itemModified }), 'POST', AppResources.ErrorSearchDetails, CoverageGroupParametrization.ResultSave);
        }
    }
}
