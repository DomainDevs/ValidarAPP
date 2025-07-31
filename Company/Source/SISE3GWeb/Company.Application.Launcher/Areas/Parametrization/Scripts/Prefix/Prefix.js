var linesBusiness = {};
var PrefixeNew = {};
var inputSearch = "";
var prefixIndex = null;
var prefixIdSelected = null;
var glbBusinessBranchDelete = [];
var glbTechnicalBranchDelete = [];
var auxLineBusiness = [];

class PrefixQueries {
    static GetPrefixTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Prefix/GetPrefixType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Prefix/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Prefix/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class LinesBussiness {
    static GetLinesBusiness() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Prefix/GetLinesBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class ParametrizationPrefix extends Uif2.Page {

    static deleteCallbackBussinesBranch(deferred, result) {
        deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined && result.Status == undefined) {
            result.Status = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listViewBusinessBranch").UifListView("addItem", result);
        }
    }
    static deleteCallbackTechnicalBranch(deferred, result) {
        if ($("#listViewTechnicalBranch").UifListView('getData').length == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageThereMustBeAtLeastOneAssociatedBranch, 'autoclose': true });
        }
        else {
            deferred.resolve();
            if (prefixIdSelected != null) {
                if (result.Id !== "" && result.Id !== undefined && result.Status == undefined) {
                    result.Status = ParametrizationStatus.Delete;
                    glbTechnicalBranchDelete.push(result);
                }
            }
        }
    }

    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listViewBusinessBranch").UifListView({
            displayTemplate: "#BusinessTemplate",
            height: 300
        });
        $("#listViewTechnicalBranch").UifListView({
            displayTemplate: "#TechnicalBranchTemplate",
            edit: false,
            delete: true,
            customEdit: true,
            deleteCallback: ParametrizationPrefix.deleteCallbackTechnicalBranch,
            height: 300
        });
        PrefixQueries.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlPrixeTechnicalBranch').UifSelect({ sourceData: data.result });
                ParametrizationPrefix.loadPrefix(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        LinesBussiness.GetLinesBusiness().done(function (data) {
            if (data.success) {
                linesBusiness = data.result;
                $('#ddlBusinessBranch').UifSelect({ sourceData: linesBusiness });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        PrefixQueries.GetPrefixTypes().done(function (data) {
            if (data.success) {
                $('#ddlBusinessType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    bindEvents() {
        $('#btnTechnicalBranch').on('click', this.openTechnicalBranch);
        $('#btnCancelTechnicalBranch').on('click', this.closeTechnicalBranch);
        $('#btnExit').click(this.exit);
        $('#inputProcess').on('buttonClick', this.searchProc);
        $('#listViewBusinessBranch').on('rowEdit', ParametrizationPrefix.showData);

        //$('#btnNewBusinessBrach').on('click', ParametrizationPrefix.clearPanel);
        $('#btnExport').on('click', this.sendExcelBusinessBranch);
        //$('#btnAcceptBusinessBrach').on('click', this.addItemBusinessBrach);
        //$('#btnSave').on('click', this.saveBusinessBranch);
        $('#btnCancelTechnicalBranchBusiness').on('click', ParametrizationPrefix.clearPanelTechnicalBranch);
        $('#btnSaveTechnicalBranchBusiness').on('click', this.saveToListViewTechnicalBranch);
        $('#btnSaveTechnicalBranch').on('click', this.saveTechnicalBranch);
        $("#inputCommercialBranchCode").OnlyDecimals(2);
        $("#btnAditionalInformation").click(this.BtnAditionalInformation);
        $('.OnlyNumber').ValidatorKey(ValidatorType.Number);

    }

    static clearPanel() {
        prefixIndex = null;
        prefixIdSelected = null;
        $("#inputCommercialBranchCode").val('');
        $("#inputCommercialBranchCode").focus();
        $('#inputCommercialBranchCode').attr("disabled", false);
        $("#inputDescription").val('');
        $("#inputDescriptionShortBusiness").val('');
        $("#inputTiny").val('');
        $("#ddlBusinessType").UifSelect("setSelected", null);
        $("#ddlBusinessType").UifSelect("disabled", false);
        $("#ddlBusinessBranch").UifSelect("setSelected", null);
        $("#ddlBusinessBranch").UifSelect("disabled", false);
        $("#inputProcess").val('');
        $("#inputDetailCommission").attr("checked", false);
        if ($("#listViewTechnicalBranch").UifListView('getData').length > 0) {
            //$("#listViewTechnicalBranch").UifListView("clear");
            $("#listViewTechnicalBranch").UifListView({
                displayTemplate: "#TechnicalBranchTemplate",
                edit: false,
                delete: true,
                customEdit: true,
                deleteCallback: ParametrizationPrefix.deleteCallbackTechnicalBranch,
                height: 300
            });
        }
        ParametrizationPrefix.clearValidation();
        AditionalInformation.SetDefaultInformation();
    }

    static loadPrefix(prefixes) {
        glbTechnicalBranchDelete = [];
        $("#listViewBusinessBranch").UifListView({ source: null, displayTemplate: "#BusinessTemplate", edit: true, delete: false, customEdit: true, deleteCallback: this.deleteCallbackBussinesBranch, height: 300 });
        prefixes.forEach(item => {
            //item.Status = ParametrizationStatus.Create;
            $("#listViewBusinessBranch").UifListView("addItem", item);
        });
    }

    static showData(event, result, index) {
        ParametrizationPrefix.clearPanel();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        var TechBranch = $("#listViewTechnicalBranch").UifListView('getData');
        if (TechBranch != undefined) {
            if (TechBranch.length > 0) {
                $("#listViewTechnicalBranch").UifListView("clear");
            }
        }
        if (result.Id != undefined) {
            prefixIndex = index;
            prefixIdSelected = result.Id;
            $("#inputCommercialBranchCode").val(result.Id);
            $('#inputCommercialBranchCode').attr("disabled", true);
            $("#inputDescription").val(result.Description);
            $("#inputDescriptionShortBusiness").val(result.SmallDescription);
            $("#inputTiny").val(result.TinyDescription);
            if (result.HasDetailCommiss) {
                $("#inputDetailCommission").attr("checked", true);
                $("#inputDetailCommission").prop("checked", true);
            }
            else {
                $("#inputDetailCommission").attr("checked", false);
                $("#inputDetailCommission").prop("checked", false);
            }
            
            $("#ddlBusinessType").UifSelect("setSelected", result.PrefixType.Id);
            if (auxLineBusiness.length > 0) {
                PrefixeNew.LineBusiness = auxLineBusiness;
                auxLineBusiness = [];
            }
            if (result.LineBusiness != undefined) {
                if (result.LineBusiness.length > 0) {
                    if (result.LineBusiness.length > 1) {
                        $('#ddlBusinessBranch').attr("disabled", true);
                    }
                    $("#ddlBusinessBranch").UifSelect("setSelected", result.LineBusiness[0].Id);
                    $("#ddlTechnicalBranchBusiness").UifSelect("setSelected", result.LineBusiness[0].Id);
                    ParametrizationPrefix.loadLinesBusiness(result.LineBusiness);
                }
            }
            AditionalInformation.SetAditionalInformation(result.AditionalInformation);
        }
        $('#TitleTechnical').val();
        ParametrizationPrefix.updateLineBusinesss();
    }

    static SearchProcess(id, inputSearch) {
        var data = [];
        var search = $("#listViewBusinessBranch").UifListView('getData');
        var find = false;
        if (id == 0) {
            $.each(search, function (key, value) {
                if (value.Description.toLowerCase().sistranReplaceAccentMark().includes(inputSearch.toLowerCase().sistranReplaceAccentMark())) {
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });
        }
        else {
            $.each(search, function (key, value) {
                if (value.Id == id) {
                    prefixIndex = key
                    value.key = key;
                    data.push(value);
                    find = true;
                    return false;
                }
            });
        }

        if (find === false) {
            $.UifNotify('show',
                { 'type': 'danger', 'message': Resources.Language.MessageNotFoundPrefixes, 'autoclose': true })
        } else {
            if (data.length === 1) {
                ParametrizationPrefix.showData(null, data, data.key);
            } else {
                AdvancedSearchBusiness.ShowSearchAdv(data);
            }
        }
    }

    static updateLineBusinesss() {
        var enabledLines = [];
        enabledLines = jQuery.extend(true, [], linesBusiness);

        if ($("#listViewTechnicalBranch").UifListView("getData").length == 0) {
            if ($("#ddlBusinessBranch").UifSelect("getSelected") != "") {
                var lineBusiness = [];
                var item = {};
                item.Id = parseInt($("#ddlBusinessBranch").UifSelect("getSelected"));
                item.Description = $("#ddlBusinessBranch").UifSelect("getSelectedText");
                lineBusiness.push(item);
                ParametrizationPrefix.loadLinesBusiness(lineBusiness);
                glbTechnicalBranchDelete = [];
                auxLineBusiness = [];
            }
        }

        var lista = $("#listViewTechnicalBranch").UifListView('getData');
        $.each(lista, function (index, value) {
            var item = enabledLines.filter(x => x.Id === value.Id);
            if (item.length > 0) {
                if (prefixIdSelected != null) {
                    var lineBussinessPrefix = ($("#listViewBusinessBranch").UifListView('getData').filter(x => x.Id == prefixIdSelected))[0].LineBusiness;
                    if (lineBussinessPrefix.filter(x => x.Id == item[0].Id).length == 0) {
                        $("#listViewBusinessBranch").UifListView('getData').filter(x => x.Id == prefixIdSelected)[0].LineBusiness.push(item[0]);
                    }
                }
                var ind = enabledLines.indexOf(item[0]);
                enabledLines.splice(ind, 1);
            }
        })
        $('#ddlTechnicalBranchBusiness').UifSelect({ sourceData: enabledLines });
    }

    static clearPanelTechnicalBranch() {
        $("#ddlTechnicalBranchBusiness").UifSelect("setSelected", null);
        $("#ddlTechnicalBranchBusiness").UifSelect("disabled", false);

    }

    static loadLinesBusiness(assignedLines) {
        //$("#listViewTechnicalBranch").UifListView({ source: null, displayTemplate: "#TechnicalBranchTemplate", delete: true, deleteCallback: this.deleteCallback, height: 300 });
        if (auxLineBusiness.length > 0) {
            assignedLines = btnTechnicalBranch;
        }
        if (assignedLines) {
            assignedLines.forEach(item => {
                $("#listViewTechnicalBranch").UifListView("addItem", item);
            });
        }
    }

    exit() {
        window.location = rootPath + "Home/Index";
    }

    searchProc(event, selectedItem) {
        if (!selectedItem || !selectedItem.trim() || selectedItem.length < 3) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInputSearchCoverage, 'autoclose': true })
            return;
        }
        inputSearch = selectedItem;
        ParametrizationPrefix.SearchProcess(0, inputSearch);
    }

    SelectSearch() {
        ParametrizationPrefix.SearchProcess($(this).children()[0].innerHTML, "");
        $('#modalDefaultSearch').UifModal("hide");
    }

    sendExcelBusinessBranch() {
        PrefixQueries.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    addItemBusinessBrach() {
        $("#formBusinessBranch").validate();
        if ($("#formBusinessBranch").valid()) {
            PrefixeNew = {};
            PrefixeNew.Id = parseInt($("#inputCommercialBranchCode").val());
            PrefixeNew.Description = $("#inputDescription").val();
            PrefixeNew.SmallDescription = $("#inputDescriptionShortBusiness").val();
            PrefixeNew.TinyDescription = $("#inputTiny").val();
            PrefixeNew.PrefixType = { Id: $("#ddlBusinessType").UifSelect("getSelected") };
            PrefixeNew.PrefixTypeCode = $("#ddlBusinessType").UifSelect("getSelected");
            PrefixeNew.AditionalInformation = AditionalInformation.GetAditionalInformation();
            PrefixeNew.HasDetailCommiss = $("#inputDetailCommission").is(':checked');
            if ($("#listViewTechnicalBranch").UifListView('getData').length == 0) {
                var lineBusiness = {};
                PrefixeNew.LineBusiness = [];
                lineBusiness.Id = $("#ddlBusinessBranch").UifSelect("getSelected");
                lineBusiness.Description = $("#ddlBusinessBranch").UifSelect("getSelectedText");
                lineBusiness.Status = ParametrizationStatus.Create;
                PrefixeNew.LineBusiness.push(lineBusiness)
            }
            else {
                PrefixeNew.LineBusiness = $("#listViewTechnicalBranch").UifListView('getData');
                auxLineBusiness = PrefixeNew.LineBusiness.slice();
                $.each(glbTechnicalBranchDelete, function (index, value) {
                    PrefixeNew.LineBusiness.push(value);
                });
            }
            //PrefixeNew.Status = 'Modified';
            if (prefixIndex == null) {
                var ifExist = $("#listViewBusinessBranch").UifListView('getData').filter(function (item) {
                    return item.Id == $("#inputCommercialBranchCode").val() || item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase()
                        || item.TinyDescription == $("#inputTiny").val()
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistPrefix, 'autoclose': true });
                }
                else {
                    PrefixeNew.Status = ParametrizationStatus.Create;
                    $("#listViewBusinessBranch").UifListView("addItem", PrefixeNew);
                }
            }
            else {
                var ifExist = $("#listViewBusinessBranch").UifListView('getData').filter(function (item) {
                    return
                    ((item.Id != $("#inputCommercialBranchCode").val()) &&
                        (item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase() || item.TinyDescription == $("#inputTiny").val()))

                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistPrefix, 'autoclose': true });
                }
                PrefixeNew.Status = ParametrizationStatus.Update;
                var data = $("#listViewBusinessBranch").UifListView('getData');
                $.each(data, function (index, value) {
                    if (value.Id == prefixIdSelected) {
                        if (PrefixeNew.LineBusiness == null) {
                            PrefixeNew.LineBusiness = [];
                        }
                        $.each(value.LineBusiness.filter(x => x.Status == ParametrizationStatus.Delete), function (indexLine, valueLine) {

                            var ifExist = PrefixeNew.LineBusiness.filter(function (item) {
                                return item.Id == valueLine.Id 
                            });
                            if (ifExist.length > 0) {
                                //$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistPrefix, 'autoclose': true });
                            }
                            else {
                                //PrefixeNew.Status = ParametrizationStatus.Create;
                                //$("#listViewBusinessBranch").UifListView("addItem", PrefixeNew);
                                PrefixeNew.LineBusiness.push(valueLine);
                            }

                            //PrefixeNew.LineBusiness.push(valueLine);
                        });                       
                        data[index] = PrefixeNew;
                    }
                });
                //$('#listViewBusinessBranch').UifListView('editItem', prefixIndex, PrefixeNew);
                ParametrizationPrefix.loadPrefix(data);
            }
            ParametrizationPrefix.clearPanel();

        }
    }

    saveBusinessBranch() {
        var data = $("#listViewBusinessBranch").UifListView('getData');
        var prefixes = $("#listViewBusinessBranch").UifListView('getData').filter(x => x.Status == ParametrizationStatus.Create || x.Status == ParametrizationStatus.Update || x.Status == ParametrizationStatus.Delete);
        $.each(glbBusinessBranchDelete, function (index, item) {
            prefixes.push(item);
        });
        if (prefixes.length > 0) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/Prefix/SavePrefix',
                data: JSON.stringify({ prefixUpdate: prefixes }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                var dataNum = data;
                if (data.success) {
                    PrefixQueries.GetPrefixes().done(function (data) {
                        if (data.success) {
                            $('#ddlPrixeTechnicalBranch').UifSelect({ sourceData: data.result });
                            ParametrizationPrefix.loadPrefix(data.result);
                            $.UifNotify('show', {
                                'type': 'info', 'message': dataNum.result.Message ,
                               // 'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                               // "Agregados" + ':' + dataNum.result.TotalAdded + '<br> ' +
                               // "Modificados" + ':' + dataNum.result.TotalModified + '<br> ' +
                               // "Eliminados" + ':' + dataNum.result.TotalDeleted + '<br> ' +
                               // + dataNum.result.Message,
                                'autoclose': true
                            });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });

                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveModule, 'autoclose': true })
            });
        }

    }

    deleteItemBusinessBranch(event, data) {
        var BusinessBranch = $("#listViewBusinessBranch").UifListView('getData');
        $("#listViewBusinessBranch").UifListView({ source: null, displayTemplate: "#BusinessTemplate", edit: true, delete: false, customAdd: true, customEdit: true, deleteCallback: this.deleteCallback, height: 300 });
        $.each(BusinessBranch, function (index, value) {
            if (this.Id == data.Id) {
                glbBusinessBranchDelete.push(value);
            }
            else {
                $("#listViewBusinessBranch").UifListView("addItem", this);
            }
        });
        ParametrizationPrefix.clearPanel();
    }



    openTechnicalBranch() {
        $('#ModalTechnicalBranch').UifModal('showLocal', Resources.Language.TitleTechnicalBranches + ' ' + $('#inputDescription').val());
        ParametrizationPrefix.updateLineBusinesss();
    }

    closeTechnicalBranch() {
        var technicalBranch = [];
        var BusinessBranch = $("#listViewTechnicalBranch").UifListView('getData');
        //$("#listViewTechnicalBranch").UifListView({ source: null, displayTemplate: "#TechnicalBranchTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: this.deleteCallback, height: 300 });

        $.each(BusinessBranch, function (index, value) {
            if (value.Status != undefined) {
                if (value.Status != ParametrizationStatus.Create) {
                    technicalBranch.push(this);
                }
            }
            //else {
            //    technicalBranch.push(this);
            //}
        });

        technicalBranch.forEach(item => {
            $("#listViewTechnicalBranch").UifListView("addItem", item);
        }
    );
        ParametrizationPrefix.updateLineBusinesss();


        $('#ModalTechnicalBranch').UifModal('hide');
    }

    addItemTechnicalBranch() {
        var TechnicalBranch = {};
        $("#formTechnicalBranch").validate();
        if ($("#formTechnicalBranch").valid()) {
            var enabled = true;
            var enabledDescription = Resources.Language.LabelEnabled;
            TechnicalBranch =
                {
                    Enabled: enabled,
                    Id: $("#inputCommercialBranchCode").val(),
                    Description: $("#ddlTechnicalBranchBusiness").UifSelect("getSelectedText"),
                    SmallDescription: $("inputDescriptionShortBusiness").val(),
                    PrefixCode: $("#ddlBusinessType").UifSelect("getSelected"),
                    LineBusinessCode: $("#ddlTechnicalBranchBusiness").UifSelect("getSelected")
                }
            if (BusinessBranchIndex != null) {
                var lista = $("#listViewTechnicalBranch").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == TechnicalBranch.Description.toUpperCase() && item.Id == TechnicalBranch.Id;
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistBranchName, 'autoclose': true });
                }
                else {
                    $("#listViewTechnicalBranch").UifListView("addItem", TechnicalBranch);
                }
            }
            else {
                $('#listViewTechnicalBranch').UifListView('editItem', TechnicalBranch);
            }
        }
    }

    saveToListViewTechnicalBranch() {
        var TechnicalBranchView = {};
        if (parseInt($("#ddlTechnicalBranchBusiness").UifSelect("getSelected")) > 0) {
            TechnicalBranchView =
                {
                    Id: parseInt($("#ddlTechnicalBranchBusiness").UifSelect("getSelected")),
                    Description: $("#ddlTechnicalBranchBusiness").UifSelect("getSelectedText"),
                    Status: ParametrizationStatus.Create
                };
            $("#listViewTechnicalBranch").UifListView("addItem", TechnicalBranchView);
            ParametrizationPrefix.updateLineBusinesss();
        }
    }
    saveTechnicalBranch() {
        if (prefixIdSelected != null) {
            var data = $("#listViewBusinessBranch").UifListView('getData').filter(x => x.Id == prefixIdSelected);
            data.Status = ParametrizationStatus.Update;

            $("#listViewBusinessBranch").UifListView('clear');
            ParametrizationPrefix.loadPrefix(data);


        }
        var item = $("#listViewTechnicalBranch").UifListView("getData");
        if (item.length > 0) {
            $.each(item, function (index, value) {
                if (value.Status == ParametrizationStatus.Create) {
                    value.PrefixCode = $("#inputCommercialBranchCode").val();
                }
            });
            $("#ddlBusinessBranch").UifSelect("setSelected", item[0].Id)
        } else {
            $('#ddlBusinessBranch').UifSelect({ sourceData: linesBusiness });
            $("#formBusinessBranch").validate();
        }
        $("#ModalTechnicalBranch").UifModal('hide');
    }

    static clearValidation() {
        var validator = $("#formBusinessBranch").validate();
        $('[name]', "#formBusinessBranch").each(function () {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present
        });
        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }

    BtnAditionalInformation() {
        AditionalInformation.OpenAditionalInformation();
    }


}