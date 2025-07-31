var banEditar = 0;
$.ajaxSetup({ async: true });
class ParametrizationCoverageValue extends Uif2.Page {

    getInitialState() {
        $("#lsvCoverageValue").UifListView({
            displayTemplate: "#template-coverageValue",
            customAdd: true,
            height: 380,
            widht: 100,
            customEdit: true, edit: true,
            delete: true,
            selectionType: "single",
            title: Resources.Language.titleCoverageValue,
            deleteCallback: ParametrizationCoverageValue.deleteRow,
        });

        ParametrizationCoverageValue.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlPrefix').UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        ParametrizationCoverageValue.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlPrefixSimpleSearch').UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        ParametrizationCoverageValue.GetCoverageValues().done(function (n) {
            if (n.success) {
                ParametrizationCoverageValue.SetListCoverageValue(n.result);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': n.result, 'autoclose': true });
            }
        });

    }

    bindEvents() {
        $('#ddlPrefix').on('itemSelected', ParametrizationCoverageValue.GetCoverages);

        $("#btnNew").click(ParametrizationCoverageValue.clearForm);

        $('#btnSave').on('click', ParametrizationCoverageValue.saveCoverages);

        $('#lsvCoverageValue').on('rowEdit', ParametrizationCoverageValue.showData);

        $('#ddlPrefixSimpleSearch').on('itemSelected', ParametrizationCoverageValue.AcceptSearch);

        $('#btnExportCoverageValue').click(ParametrizationCoverageValue.ExportFile)

        $('#btnSalir').click(CityParametrization.Exit);
    }

    static GetCoverageValues() {

        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/CoverageValue/GetCoverageValue"
        });
    }

    static SetListCoverageValue(data) {
        $("#lsvCoverageValue").UifListView("refresh");
        $.each(data, function (index, val) {
            $("#lsvCoverageValue").UifListView("addItem", val);
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoverageValue/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
    static GetCoverages() {   
        var prefixId = 0;
        if ($("#ddlPrefix").val() != null && $("#ddlPrefix").val() != "") {
            prefixId = $("#ddlPrefix").val();
            ParametrizationCoverageValue.GetListCoverages(prefixId).done(function (data) {
                if (data.success) {
                    $("#ddlCoverage").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
            });
        }
        else
            $('#ddlCoverage').UifSelect({ source: null });
    }

    static GetListCoverages(prefixId) {

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoverageValue/GetCoverages?idPrefix=' + prefixId,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });

    }
    
    static clearForm() {
        $("#Porcentage").val("");      
        $("#ddlPrefix").UifSelect("setSelected", null);
        ParametrizationCoverageValue.GetCoverages();
        $("#ddlCoverage").UifSelect("setSelected", null);
        ClearValidation("#formCoverageValue");
    }

    static saveCoverages() {
        var porcentajeDecimal = $("#Porcentage").val();
        porcentajeDecimal = parseFloat(porcentajeDecimal.replace(",", "."));
        var CocoverageInsert = {
            Prefix: { Id: $("#ddlPrefix").UifSelect("getSelected") },
            Coverage: { Id: $("#ddlCoverage").UifSelect("getSelected") },
            Porcentage: porcentajeDecimal
        };
        if (porcentajeDecimal == 0 || porcentajeDecimal > 100 ) {
            $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorPercentageHigher, 'autoclose': true });
        }
        else {
            if ($('#formCoverageValue').valid()) {
                if (banEditar == 0) {
                    $.ajax({
                        type: "POST",
                        url: rootPath + 'Parametrization/CoverageValue/CreateCoCoverage',
                        data: JSON.stringify({ coverageValueViewModel: CocoverageInsert }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    }).done(function (data) {
                        if (data.success) {

                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                            ParametrizationCoverageValue.GetCoverageValues().done(function (n) {
                                if (n.success) {
                                    ParametrizationCoverageValue.SetListCoverageValue(n.result);
                                } else {
                                    $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCoCoverageValue, 'autoclose': true });
                                }
                            });
                            ParametrizationCoverageValue.clearForm();
                        }
                        else
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoverageValueSaveError, 'autoclose': true })
                    });
                }
                else {

                    $.ajax({
                        type: "POST",
                        url: rootPath + 'Parametrization/CoverageValue/UpdateCoCoverageValue',
                        data: JSON.stringify({ coverageValueViewModel: CocoverageInsert }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    }).done(function (data) {

                        if (data.success) {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                            ParametrizationCoverageValue.GetCoverageValues().done(function (n) {
                                if (n.success) {
                                    ParametrizationCoverageValue.SetListCoverageValue(n.result);

                                } else {
                                    $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCoCoverageValue, 'autoclose': true });
                                }
                            });
                            ParametrizationCoverageValue.clearForm();
                            banEditar = 0;
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoverageValueSaveError, 'autoclose': true })
                    });
                }
            }
    }
    }

    static showData(event, result, index) {
        ParametrizationCoverageValue.clearForm();  
        banEditar = 1;
            $('#Porcentage').val(result.Porcentage);
        $("#ddlPrefix").UifSelect("setSelected", result.Prefix.Id);
        ParametrizationCoverageValue.GetCoverages();
            $("#ddlCoverage").UifSelect("setSelected", result.Coverage.Id);
    }

    static showDataSearch(result) {
        ParametrizationCoverageValue.clearForm();
        banEditar = 1;
        $('#Porcentage').val(result.Porcentage);
        $("#ddlPrefix").UifSelect("setSelected", result.Prefix.Id);
        ParametrizationCoverageValue.GetCoverages();
        $("#ddlCoverage").UifSelect("setSelected", result.Coverage.Id);

        }    

    static deleteRow(deferred, result) {

        ParametrizationCoverageValue.clearForm()

        var CocoverageInsert = {          
            Prefix: result.Prefix,
            Coverage: result.Coverage,
            Porcentage: result.Porcentage
        };

        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CoverageValue/DeleteCoCoverageValue',
            data: JSON.stringify({ coverageValueViewModel: CocoverageInsert }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            var dataNum = data;
            if (data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                ParametrizationCoverageValue.GetCoverageValues().done(function (n) {
                    if (n.success) {
                        ParametrizationCoverageValue.SetListCoverageValue(n.result);
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCoCoverageValue, 'autoclose': true });
                    }
                });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoverageValueDeleteError, 'autoclose': true })
        });
    }

    static AcceptSearch() {
        var form = $('#frmAdvCoCoverageValue').serializeObject();       
        ParametrizationCoverageValue.GetSearch().done(function (data) {
                if (data.success) {
                    if (data.result.length > 0)
                        ParametrizationCoverageValue.successSearch(data.result);
                    else {
                        $("#lsvCoverageValue").UifListView("refresh");
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true });
                    }
                }
                else
                    $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCoCoverageValue, 'autoclose': true });
            });
       
    }

    static GetSearch() {
        var OpcionesSearch = {
            Coverage: {Id:0},
            Prefix: { Id: $("#ddlPrefixSimpleSearch").UifSelect("getSelected") }
        };

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoverageValue/GetAdvancedSearch',
            data: JSON.stringify({ coverageValueViewModel: OpcionesSearch }),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static successSearch(data) {
        $("#lsvCoverageValue").UifListView("refresh");
        $.each(data, function (index, val) {
            $("#lsvCoverageValue").UifListView("addItem", val);
        });
    }

    static ExportFile() {

        request('Parametrization/CoverageValue/ExportFile', JSON.stringify({}), 'POST', AppResources.ErrorExportExcel, ParametrizationCoverageValue.ExportFileSuccess);
    }

    static ExportFileSuccess(data) {
        DownloadFile(data);
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }
}