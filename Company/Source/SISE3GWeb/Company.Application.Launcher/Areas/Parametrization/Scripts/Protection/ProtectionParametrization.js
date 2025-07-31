var protectionIndex = null

class ProtectionParametrization extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });

        $("#listProtection").UifListView({
            displayTemplate: "#ProtectionTemplate",
            source: null,
            selectionType: 'single',
            height: 400
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        ProtectionParametrization.GetProtectionParametrizations();
        ProtectionParametrization.ClearForm()
    }

    bindEvents() {
        $("#inputSearchProtection").on('itemSelected', ProtectionParametrization.ShowAdvancedProtection);
        $("#inputSearchProtection").on("buttonClick", ProtectionParametrization.ShowAdvancedProtection);
        $("#btnNewProtection").on("click", ProtectionParametrization.ClearForm)
        $("#btnProtectionAccept").on("click", ProtectionParametrization.AddItemProtection);
        $("#listProtection").on('rowEdit', ProtectionParametrization.ShowData);
        $("#btnSaveProtection").on("click", ProtectionParametrization.SaveProtection);
        $("#btnExport").on("click", ProtectionParametrization.sendExcelProtection);
        $("#btnExit").on("click", ProtectionParametrization.Exit);
    }

    static GetProtectionParametrizations() {
        request('Parametrization/Protection/GetProtectionsAll', null, 'GET', AppResources.ErrorSearchProtection, ProtectionParametrization.LoadProtections);
    }

    static sendExcelProtection() {
        ProtectionParametrizationRequest.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }


    static ShowAdvancedProtection(event, selectedItem) {
        ProtectionParametrization.GetProtections(selectedItem);
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static GetProtections(description) {
        var find = false;
        var data = [];
        var search = $("#listProtection").UifListView('getData');
        if (description.length < 3) {
            $.UifNotify('show', {
                type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false
            });
        } else {
            $.each(search, function (key, value) {
                if ((value.DescriptionLong.toLowerCase().sistranReplaceAccentMark().
                    includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ||
                    (value.DescriptionShort.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))) {
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });
            ProtectionParametrization.ShowData(null, data, data.key);
            if (find == false) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ProtectionNotFound, 'autoclose': true })
            }
        }
    }

    static LoadProtections(protection) {
        if (Array.isArray(protection)) {
            $("#listProtection").UifListView({
                sourceData: protection,
                displayTemplate: "#ProtectionTemplate",
                selectionType: 'single',
                edit: true,
                delete: true,                
                customEdit: true,
                height: 400,
                deleteCallback: ProtectionParametrization.deleteCallbackList
            });          
        }
    }

	static deleteCallbackList(deferred, result) {
		deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined && result.Id !== 0) //Se elimina unicamente si existe en DB
		{
			result.Status = ParametrizationStatus.Delete;
			result.allowEdit = false;
			result.allowDelete = false;
			$("#listProtection").UifListView("addItem", result);
        }
    }

    static ResultSave(data) {
        if (Array.isArray(data.data)) {
            $.UifNotify('show', { 'type': 'info', 'message': data.message, 'autoclose': true });
            ProtectionParametrization.LoadProtections(data.data);
        }
    }
    static ShowData(event, result, index) {
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ProtectionParametrization.ShowSearchAdv(result);
        }
        if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            protectionIndex = index;
            $("#inputLongDescription").val(resultUnique.DescriptionLong);
            $("#inputShortDescription").val(resultUnique.DescriptionShort);
            $("#inputSearchProtection").val("");
            $("#inputId").val(resultUnique.Id);
            ClearValidation("#formProtection");
        }
    }

    static ClearForm() {
        protectionIndex = null;
        $("#inputLongDescription").val(null);
        $("#inputLongDescription").focus();
        $("#inputShortDescription").val(null);
        $("#inputSearchProtection").val(null);
        $("#inputId").val("0");
        ClearValidation("#formProtection");
    }

    static AddItemProtection() {
        $("#formProtection").validate();
        if ($("#formProtection").valid()) {
            let protectionParamerizationData = $("#formProtection").serializeObject();
            protectionParamerizationData.Id = parseInt($("#inputId").val());

            var list = $("#listProtection").UifListView('getData');
            var ifExist = list.filter(function (item) {
                return (item.DescriptionLong.toLowerCase().sistranReplaceAccentMark() == protectionParamerizationData.DescriptionLong.toLowerCase().sistranReplaceAccentMark()
                    || item.DescriptionShort.toLowerCase().sistranReplaceAccentMark() == protectionParamerizationData.DescriptionShort.toLowerCase().sistranReplaceAccentMark())
                    && item.Id != protectionParamerizationData.Id;
            });
            if (ifExist.length > 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistProtectionName, 'autoclose': true });
            }
            if (protectionIndex == null) {
                var ifExist = list.filter(function (item) {
                    return (item.DescriptionLong.toLowerCase().sistranReplaceAccentMark() == protectionParamerizationData.DescriptionLong.toLowerCase().sistranReplaceAccentMark()
                        || item.DescriptionShort.toLowerCase().sistranReplaceAccentMark() == protectionParamerizationData.DescriptionShort.toLowerCase().sistranReplaceAccentMark());
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistProtectionName, 'autoclose': true });
                }
                else {
                    protectionParamerizationData.StatusTypeService = ParametrizationStatus.Create;
                    protectionParamerizationData.Status = ParametrizationStatus.Create;
                    $("#listProtection").UifListView("addItem", protectionParamerizationData);
                    ProtectionParametrization.ClearForm();
                }               
            }
            else {
                var ifExist = list.filter(function (item) {
                    return (item.DescriptionLong.toLowerCase().sistranReplaceAccentMark() == protectionParamerizationData.DescriptionLong.toLowerCase().sistranReplaceAccentMark()
                        || item.DescriptionShort.toLowerCase().sistranReplaceAccentMark() == protectionParamerizationData.DescriptionShort.toLowerCase().sistranReplaceAccentMark())
                        && item.Id != protectionParamerizationData.Id;
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistProtectionName, 'autoclose': true });
                }
                else {
                    if (protectionParamerizationData.Id != 0) {
                        protectionParamerizationData.StatusTypeService = ParametrizationStatus.Update;
                        protectionParamerizationData.Status = ParametrizationStatus.Update;
                    }
                    else {
                        protectionParamerizationData.StatusTypeService = ParametrizationStatus.Create;
                        protectionParamerizationData.Status = ParametrizationStatus.Create;
                    }
                    $("#listProtection").UifListView('editItem', protectionIndex, protectionParamerizationData);
                    ProtectionParametrization.ClearForm();
                }                
            }
		}
     }

    static DeleteItemProtection(event, data) {
        var protections = $("#listProtection").UifListView('getData');
        $("#listProtection").UifListView({ source: null, displayTemplate: "#ProtectionTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 400 });
        $.each(protections, function (index, value) {
            if ((this.DescriptionLong == data.DescriptionLong && this.Id == data.Id)) {
                if (data.Id != 0) {
                    value.Status = ParametrizationStatus.Delete;
                    //glbProtectionDelete.push(value);
                }
            }
            else {
                $("#listProtection").UifListView("addItem", this);
            }
        });
        ProtectionParametrization.ClearForm();
    }

    static SaveProtection() {
        var itemModified = [];
        var protections = $("#listProtection").UifListView('getData');
        $.each(protections, function (index, value) {
            if (value.Status != undefined && value.Status != ParametrizationStatus.Original) {
                itemModified.push(value);
            }
        });
        if (itemModified.length > 0) {
            request('Parametrization/Protection/ExecuteOperation', JSON.stringify({ protectionParametrizations: itemModified }), 'POST', AppResources.ErrorSearchProtection, ProtectionParametrization.ResultSave);
        }
    }
    static ShowSearchAdv(data) {
        $("#lvSearchAdvProtection").UifListView({
            displayTemplate: "#ProtectionTemplate",
            selectionType: "single",
            height: 400
        });
        $("#lvSearchAdvProtection").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvProtection").UifListView("addItem", item);
            });
        }
        dropDownSearchProtection.show();
    }

    static HideSearchAdv() {
        dropDownSearchProtection.hide();
    }

}
