var ConditionTextCode = null;
var CoverageConsec = 0;
$.ajaxSetup({ async: true });
class ConditionText extends Uif2.Page {
    getInitialState() {
        $("#lsvConditionText").UifListView({
            displayTemplate: "#template-cities",
            customAdd: true,
            height: 380,
            widht: 100,
            customEdit: true, edit: true,
            delete: true,
            selectionType: "single",
            title: "Texto precatalogado",
            deleteCallback: ConditionText.deleteRowConditiontext,
        });
        $('.Cobertura').hide();
        ConditionText.GetGetConditionLevelCombo().done(function (data) {
            if (data.success) {
                $('#ddlConditionLevel').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        ConditionText.GetConditionalText();
    }

    bindEvents() {
        $('#ddlConditionLevel').on('itemSelected', ConditionText.GetConditionLevelType);

        $("#btnNew").click(ConditionText.ClearControls);

        $('#tblResultListCoverage').on('rowSelected', ConditionText.SelectSearchCoverage);

        $('#btnSave').on('click', ConditionText.saveConditionText);

        $('#lsvConditionText').on('rowEdit', ConditionText.showData);

        $('#btnExportCity').click(ConditionText.ExportFile)

        $('#Coverage').on("search", ConditionText.SearchTextCoverage);

        $('#ObjectInsurance').on("search", ConditionText.SearchTextCoverage);

        $('#btnExit').click(this.Exit);

        $("#btnExport").click(ConditionText.ExportFile)

        $('#searchConditionText').on("search", ConditionText.SearchConditionText);
    }

    static SelectSearchCoverage(event, dataCoverage, position) {
        switch (CoverageConsec) {
            case 1:
                $("#Coverage").data("Object", dataCoverage);
                $("#Coverage").val(dataCoverage.Description + '(' + dataCoverage.Id + ')');
                $("#ObjectInsurance").val(dataCoverage.InsuredObjectServiceQueryModel.Description + '(' + dataCoverage.InsuredObjectServiceQueryModel.Id + ')');
                $("#Protection").val(dataCoverage.PerilServiceQueryModel.Description + '(' + dataCoverage.PerilServiceQueryModel.Id + ')');
                break;
            case 2:
                $("#ObjectInsurance").val(dataCoverage.Description + '(' + dataCoverage.Id + ')')
                break;
        }
        $('#modalListSearchCoverage').UifModal('hide');
    }

    static SearchConditionText(event, value) {
        if (value != null) {
            if (value.length >= 3 || value.length == 0) {
                ConditionText.GetConditionalText(String(value))
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
            }
        }
    }

    static SearchTextCoverage(event, value) {
        var Path = "";
        if (value.length >= 0) {
            switch (event.target.id) {
                case "Coverage":
                    Path = "Parametrization/Clauses/GetCoverage"
                    CoverageConsec = 1;
                    break;
                case "ObjectInsurance":
                    Path = "Parametrization/InsuredObject/GetInsuredObjectByDescription"
                    CoverageConsec = 2;
                    break;
                case "Protection":
                    Path = "Parametrization/InsuredObject/GetInsuredObjectByDescription"
                    CoverageConsec = 3;
                    break;

            }
            ConditionText.GetTextCoverage(value, Path);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': "", 'autoclose': true })
        }
    }

    static SearchTexObject(event, value) {
        if ($.trim($("#ObjectInsurance").val()) != "") {
            ConditionText.GetTextInsureObject($("#ObjectInsurance").val());
        }
    }

    static GetTextCoverage(description, Path) {
        if (description.length >= 0) {
            $.ajax({
                type: "POST",
                url: rootPath + Path,
                data: JSON.stringify({ description: description }),
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var DataSucces = false;
                    switch (CoverageConsec) {
                        case 1:
                            if (data.result.CoverageServiceModels !== null) {
                                DataSucces = true;

                                if (data.result.CoverageServiceModels.length === 1) {
                                    $("#Coverage").data("Object", data.result.CoverageServiceModels[0]);
                                    $("#Coverage").val(data.result.CoverageServiceModels[0].Description + '(' + data.result.CoverageServiceModels[0].Id + ')');
                                    $("#ObjectInsurance").val(data.result.CoverageServiceModels[0].InsuredObjectServiceQueryModel.Description);
                                    $("#Protection").val(data.result.CoverageServiceModels[0].PerilServiceQueryModel.Description);
                                }
                                else {
                                    ConditionText.ShowTextListCoverage(data.result.CoverageServiceModels);
                                }
                            }
                            break;
                        case 2:

                            if (data.result != null) {
                                DataSucces = true;
                                if (data.result.length === 1) {
                                    $("#ObjectInsurance").val(data.result[0].Description);
                                }
                                else {
                                    ConditionText.ShowTextListCoverage(data.result);
                                }
                            }
                            break;
                        case 3:
                            if (data.result != null) {
                                DataSucces = true;
                                if (data.result.length === 1) {
                                    $("#ObjectInsurance").val(data.result[0].Description);
                                }
                                else {
                                    ConditionText.ShowTextListCoverage(data.result);
                                }
                            }
                            break;
                            if (DataSucces == false) { $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true }); }
                    }
                }
            });
        }
    }

    static GetTextInsureObject(description) {
        if (description.length >= 0) {
            $.ajax({
                type: "POST",
                url: rootPath + "Parametrization/InsuredObject/GetInsuredObjectByDescription",
                data: JSON.stringify({ description: description }),
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0) {
                        if (data.result.length === 1) {
                            $("#ObjectInsurance").val(data.result[0].Description);
                        }
                        else {
                            ConditionText.ShowTextListCoverage(data.result);
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }
    static ShowTextListCoverage(dataTable) {
        $('#tblResultListCoverage').UifDataTable('clear');
        $('#tblResultListCoverage').UifDataTable('addRow', dataTable);
        $('#modalListSearchCoverage').UifModal('showLocal', AppResources.LabelSelectText);
    }

    static GetConditionalText(Description = "") {
        var Controller = "";
        if (Description != "") {
            Controller = "Parametrization/ConditionText/GetConditiontextByDescription?description=" + Description;
        }
        else {
            Controller = "Parametrization/ConditionText/GetConditiontext";
        }
        $.ajax({
            type: "POST",
            async: false,
            url: rootPath + Controller
        }).done(function (n) {
            if (n.success) {
                ConditionText.SetListConditionText(n.result);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': n.result, 'autoclose': true });
            }
        });
    }
    static SearchCoverage(event, data, index) {
        var inputObject = $('#searchCobertura').val();

        request('Parametrization/ConditionText/GetCoverageByDescription', JSON.stringify({ description: inputObject }), 'POST', Resources.Language.ErrorExistSalePoint, this.ShowSearchAdv)
        $('#inputSalePoint').val("");
        SalePointParametrization.ClearListView();
    }

    static ShowSearchAdv(data) {
        if (data.length != 0) {
            $("#lvSearchAdvSalePoint").UifListView("clear");
            if (data.length === 1) {
                var list = $("#listViewSalePoint").UifListView('getData');
                $.each(list, function (key, value) {
                    if (value.Id == data[0].Id) {
                        index = key;
                    }
                });
                SalePointParametrization.SalePointSearch(data[0]);
                SalePointParametrization.ShowData(null, data[0], index);

            } else {
                $.each(data, function (key, value) {
                    var object =
                        {
                            Id: this.Id,
                            Description: this.Description,
                            SmallDescription: this.SmallDescription,
                            Branch: this.Branch,
                            BranchId: this.BranchId
                        };
                    $("#lvSearchAdvSalePoint").UifListView("addItem", object);
                });
                SalePointAdvancedSearch.SearchAdvSalePoint();
            }
        } else {
            $.UifNotify('show', { type: 'info', message: Resources.Language.TechnicalPlanNotFound, autoclose: true });
        }
    }
    static SetListConditionText(conditionText) {
        $("#lsvConditionText").UifListView("refresh");
        conditionText.forEach(item => {
            $("#lsvConditionText").UifListView("addItem", item);
        });
    }

    static GetListConditionLevel(Path) {
        return $.ajax({
            type: 'POST',
            url: rootPath + Path,
            async: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetGetConditionLevelCombo() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/ConditionText/GetConditionLevel',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetListStates(countryId) {

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetStatesByCountryId?countryId=' + countryId,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });

    }

    static GetListCityByDescription(cityDescription) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetCitiesByDescription?cityDescription=' + cityDescription,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ClearControls() {
        ConditionTextCode = null;
        $("#Title").val("");
        $("#ddlConditionLevelType").UifSelect("setSelected", null);
        $("#ddlConditionLevel").UifSelect("setSelected", null);
        $("#Body").val("");
        $("#Coverage").data("");
        $("#Coverage").val("");
        $("#ObjectInsurance").val("");
        $("#Protection").val("")

        // ClearValidation("#formcities");
    }
    static GetConditionLevelType() {
        if ($("#ddlConditionLevel").val() != null && $("#ddlConditionLevel").val() != "") {
            var PathController = "", Level = parseInt($("#ddlConditionLevel").val());
            PathController = "Parametrization/ConditionText/GetGenericConditionLevel?iType=" + Level;
            $("#divConditionLevelType").show();
            $(".Cobertura").hide();
            ConditionText.GetListConditionLevel("Parametrization/ConditionText/GetConditiontext").done(function (data) {
                if (data.success) {
                    $("#ddlConditionLevelType").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
            });
            if (Level != 1) {
                ConditionText.GetListConditionLevel(PathController).done(function (data) {
                    if (data.success) {
                        $("#ddlConditionLevelType").UifSelect({ sourceData: data.result, native: false, filter: true });
                    }
                });
            }
            switch (Level) {
                case 1:
                    $("#divConditionLevelType").fadeOut("slow");
                    break;
                case 2:
                    $("#TxtConditionLevelType").text("Ramo Comercial")
                    //$("#ddlConditionLevelType").UifSelect("disabled", true);
                    break;
                case 3:
                    $("#TxtConditionLevelType").text("Tipo de riesgo cubierto")
                    break;
                case 4:
                    $("#TxtConditionLevelType").text("Ramo Técnico")
                    break;
                case 5:
                    $(".Cobertura").fadeIn("slow");
                    $("#divConditionLevelType").fadeOut();
                    break;
            }
        }
        else
            $('#ddlConditionLevelType').UifSelect({ source: null });
    }
    static saveConditionText() {

        if ($("#formConditionText").valid()) {

            if (ConditionTextCode != null) {
                var ConditionTextEdit = {
                    Id: ConditionTextCode,
                    Title: $("#Title").val(),
                    Body: $("#Body").val(),
                    ConditionTextLevel: { Id: $("#ddlConditionLevel").UifSelect("getSelected") },
                    ConditionTextLevelType: { Id: $("#ddlConditionLevelType").UifSelect("getSelected") },
                };
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Parametrization/ConditionText/UpdateConditionText',
                    data: JSON.stringify({ conditionTextViewModel: ConditionTextEdit }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    var dataNum = data;
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'info', 'message': "Elemento modificado", 'autoclose': true });

                        ConditionText.GetConditionalText();
                        ConditionText.ClearControls()
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Error", 'autoclose': true })
                });
            }
            else {
                var idType, idTypeDescription;
                if ($("#ddlConditionLevel").UifSelect("getSelected") == 5) {
                    idType = $("#Coverage").data("Object").Id;
                    idTypeDescription = $("#Coverage").data("Object").Description;
                }
                else {
                    idType = $("#ddlConditionLevelType").UifSelect("getSelected");
                    idTypeDescription = "";
                }
                var ConditionTextNew = {
                    Id: 0,
                    Title: $("#Title").val(),
                    Body: $("#Body").val(),
                    ConditionTextLevel: { Id: $("#ddlConditionLevel").UifSelect("getSelected") },
                    ConditionTextLevelType: { Id: idType }
                };
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Parametrization/ConditionText/CreateConditionText',
                    data: JSON.stringify({ conditionTextViewModel: ConditionTextNew }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    var dataNum = data;
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'info', 'message': "Elemento Agregado", 'autoclose': true });

                        ConditionText.GetConditionalText();
                        ConditionText.ClearControls()
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Error", 'autoclose': true })
                });
            }

        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': "FALTAN CAMPOS", 'autoclose': true });
        }
    }

    static showData(event, result, index) {
        //ConditionText.ClearControls();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }

        if (result.Id != undefined) {
            ConditionTextCode = result.Id;
            $("#ddlConditionLevel").UifSelect("setSelected", result.ConditionTextLevel.Id);
            ConditionText.GetConditionLevelType();
            $("#Title").val(result.Title);
            if (result.ConditionTextLevel.Id == 5) {
                //ConditionText.GetTextCoverage(result.ConditionTextLevelType.Id, "Parametrization/Clauses/GetCoverage")
                var Path = "Parametrization/Clauses/GetCoverage"
                CoverageConsec = 1;
                ConditionText.GetTextCoverage(String(result.ConditionTextLevelType.Id), Path);

            }

            else {
                $("#ddlConditionLevelType").UifSelect("setSelected", result.ConditionTextLevelType.Id);
            }
        }

        $("#Body").val(result.Body);

    }


    // carga los datos de la vista de busqueda avanzada
    static showDataSearch(result) {
        CityParametrization.clearFormCities();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }

        if (result.Id != undefined) {
            var CityId = index;
            $('#Description').val(result.Description);
            $('#SmallDescription').val(result.SmallDescription);
            $("#ddlCountries").UifSelect("setSelected", result.Country.Id);
            CityParametrization.getStates();
            $("#ddlStates").UifSelect("setSelected", result.State.Id);
        }

    }

    static searchRangeCity(event, value) {

        if (value != null && value.length > 2) {

            $("#lsvCities").UifListView("clear");
            $("#searchCity").val("");
            CityParametrization.GetListCityByDescription(value).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0)
                        CityParametrization.SetListCities(data.result);
                    else
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorSearch, 'autoclose': true });
                }
                else {
                    $.UifNotify("show", { 'type': "danger", 'message': "Error obteniendo textos", 'autoclose': true });
                }

            });
        }
        else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true });
        }
    }

    static ExportFile() {

        request('Parametrization/Conditiontext/GenerateFileConditionText', JSON.stringify({}), 'POST', AppResources.ErrorExportExcel, ConditionText.ExportFileSuccess);
    }

    static ExportFileSuccess(data) {
        DownloadFile(data);
    }

    static deleteRowConditiontext(deferred, result) {
        var ConditionTextDelete = {
            Id: result.Id,
            Title: $("#Title").val(),
            Body: $("#Body").val(),
            ConditionTextLevel: { Id: $("#ddlConditionLevel").UifSelect("getSelected") },
            ConditionTextLevelType: { Id: $("#ddlConditionLevelType").UifSelect("getSelected") },
        };
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ConditionText/DeleteConditionText',
            data: JSON.stringify({ conditionTextViewModel: ConditionTextDelete }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            var dataNum = data;
            if (data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                ConditionText.GetConditionalText();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.Error, 'autoclose': true })
        });
    }
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}