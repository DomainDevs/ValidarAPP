var inputSearch = "";
var glbProtection = {};
var glbProtectionDelete = [];
var glbProtectionModify = [];
var glbProtectionInserted = [];
var protection = {};
var protectionIndex = null;
var dropDownSearchProtection = null;


$(() => {
    new ProtectionParametrization();
})

class Protections {
    static SaveProtection(protectionsInserted, protectionsModify, protectionsDeleted) {
        return $.ajax({
            type: 'POST',
            url: 'SaveProtection',
            data: JSON.stringify({
                protectionsInserted: protectionsInserted,
                protectionsModify: protectionsModify,
                protectionsDeleted: protectionsDeleted
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}


class ProtectionControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get SelectorTableResults_tbody() { return $("#tableResults tbody"); }
    static get ProtectionTemplate() { return "#ProtectionTemplate"; }
    static get listProtection() { return $("#listProtection"); }
    static get inputSearchProtection() { return $("#inputSearchProtection"); }
    static get btnNewProtection() { return $("#btnNewProtection"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnProtectionAccept() { return $("#btnProtectionAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputLongDescription() { return $("#inputLongDescription"); }
    static get inputShortDescription() { return $("#inputShortDescription"); }
    static get inputSearchProtection() { return $("#inputSearchProtection"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formProtection() { return $("#formProtection"); }
    static get btnSaveProtection() { return $("#btnSaveProtection"); }
    static get sendExcelProtection() { return $("#btnExport"); }
}


class ProtectionParametrization extends Uif2.Page {
    getInitialState() {
        ProtectionControls.SelectorTypeText.TextTransform(ValidatorType.UpperCase);
        ProtectionControls.listProtection.UifListView({ displayTemplate: ProtectionControls.ProtectionTemplate, edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        ProtectionParametrization.LoadFirstTimeProtection();
      
    }

    bindEvents() {
        ProtectionControls.inputSearchProtection.on('itemSelected', ProtectionParametrization.ShowAdvancedProtection);
        ProtectionControls.inputSearchProtection.on("buttonClick", ProtectionParametrization.ShowAdvancedProtection);
        ProtectionControls.btnNewProtection.on("click", ProtectionParametrization.CleanForm)
        ProtectionControls.btnExit.on("click", ProtectionParametrization.Exit);
        ProtectionControls.btnProtectionAccept.on("click", ProtectionParametrization.AddItemProtection);
        ProtectionControls.listProtection.on('rowDelete', ProtectionParametrization.DeleteItemProtection);
        ProtectionControls.listProtection.on('rowEdit', ProtectionParametrization.ShowData);
        ProtectionControls.btnSaveProtection.on("click", ProtectionParametrization.SaveProtection);
        ProtectionControls.sendExcelProtection.on("click", ProtectionParametrization.sendExcelProtection);
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

    static LoadFirstTimeProtection() {
        Protection.GetProtectionsAll().done(function (data) {
            if (data.success) {
                glbProtection = data.result;
                ProtectionParametrization.LoadProtections();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static sendExcelProtection() {
        Protections.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }


    static ShowAdvancedProtection(event, selectedItem) {
        inputSearch = selectedItem;
        ProtectionParametrization.GetProtections(inputSearch);
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static GetProtection(id) {
        var find = false;
        var data = [];
        var search = glbProtection;
        $.each(search, function (key, value) {
            if (value.Id == id) {
                protectionIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ProtectionParametrization.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ProtectionNotFound, 'autoclose': true })
        }
    }

    static GetProtections(description) {
        var find = false;
        var data = [];
        var search = ProtectionControls.listProtection.UifListView('getData');
        if (description.length < 3)
            return;
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
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ProtectionNotFound, 'autoclose': true })
        }
    }

    static LoadProtections() {
        ProtectionControls.listProtection.UifListView({ source: null, displayTemplate: ProtectionControls.ProtectionTemplate, edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(glbProtection, function (key, value) {
            protection = {
                Id: this.Id,
                DescriptionLong: this.DescriptionLong,
                DescriptionShort: this.DescriptionShort
            }
            ProtectionControls.listProtection.UifListView("addItem", protection);
        });
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
            ProtectionControls.inputLongDescription.val(resultUnique.DescriptionLong);
            ProtectionControls.inputShortDescription.val(resultUnique.DescriptionShort);
            ProtectionControls.inputSearchProtection.val(resultUnique.DescriptionLong);
            ProtectionControls.inputId.val(resultUnique.Id);
        }
    }

    static CleanForm() {
        protectionIndex = null;
        ProtectionControls.inputLongDescription.val(null);
        ProtectionControls.inputShortDescription.val(null);
        ProtectionControls.inputSearchProtection.val(null);
        ProtectionControls.inputId.val(null);
    }

    static RefreshList() {
        glbProtectionDelete = [];
        glbProtectionInserted = [];
        glbProtectionModify = [];
    }
    static AddItemProtection() {
        ProtectionControls.formProtection.validate();
        if (ProtectionControls.formProtection.valid()) {
            protection = {
                Id: ProtectionControls.inputId.val(),
                DescriptionLong: ProtectionControls.inputLongDescription.val(),
                DescriptionShort: ProtectionControls.inputShortDescription.val()
            }
            if (protectionIndex == null) {
                var list = ProtectionControls.listProtection.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.DescriptionLong.toLowerCase().sistranReplaceAccentMark()
                        == protection.DescriptionLong.toLowerCase().sistranReplaceAccentMark();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistProtectionName, 'autoclose': true });
                }
                else {
                    protection.Status = 'Added';
                    ProtectionControls.listProtection.UifListView("addItem", protection);
                }
            }
            else {
                if (protection.Id != 0) {
                    protection.Status = 'Modified';
                }
                else {
                    protection.Status = 'Added';
                }
                ProtectionControls.listProtection.UifListView('editItem', protectionIndex, protection);
            }
            ProtectionParametrization.CleanForm();
        }
    }

    static DeleteItemProtection(event, data) {
        var protections = ProtectionControls.listProtection.UifListView('getData');
        ProtectionControls.listProtection.UifListView({ source: null, displayTemplate: ProtectionControls.ProtectionTemplate, edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(protections, function (index, value) {
            if ((this.DescriptionLong == data.DescriptionLong && this.Id == data.Id)) {
                if (data.Id != 0) {
                    glbProtectionDelete.push(value);
                }
            }
            else {
                ProtectionControls.listProtection.UifListView("addItem", this);
            }
        });
        ProtectionParametrization.CleanForm();
    }

    static SaveProtection() {
        var protections = ProtectionControls.listProtection.UifListView('getData');
        $.each(protections, function (key, value) {
            if (this.Status == "Added") {
                glbProtectionInserted.push(this);
            }
            else if (this.Status == "Modified") {
                glbProtectionModify.push(this);
            }
        });
        if ((glbProtectionDelete.length + glbProtectionInserted.length + glbProtectionModify.length) > 0) {
            Protections.SaveProtection(glbProtectionInserted, glbProtectionModify, glbProtectionDelete)
                .done(function (data) {
                    if (data.success) {
                        ProtectionParametrization.RefreshList();
                        glbProtection = data.result.data;
                        ProtectionParametrization.LoadProtections();
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                })
                .fail(function () {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveProtection, 'autoclose': true })

                });
        }

    }
}
