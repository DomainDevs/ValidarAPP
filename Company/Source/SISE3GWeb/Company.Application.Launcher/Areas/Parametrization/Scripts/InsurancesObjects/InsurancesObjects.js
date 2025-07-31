var glbObjectsCreate = [];
var glbObjectsUpdate = [];
var glbObjectsDelete = [];
var objectIndex = null;
var dropDownSearchAdvInsurancesObjects = null;

$(() => {
    new ParametrizationInsurancesObjects();
});

class ParametrizationInsurancesObjects extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        ParametrizationInsurancesObjects.GetAllInsurancesObjects().done(function (data) {
            if (data.success) {
                ParametrizationInsurancesObjects.LoadListViewInsurancesObjects(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        //dropDownSearchAdvInsurancesObjects = uif2.dropDown({
        //    source: rootPath + 'Parametrization/InsurancesObjects/InsurancesObjectsAdvancedSearch',
        //    element: '#btnSearchAdvInsurancesObject',
        //    align: 'right',
        //    width: 600,
        //    height: 500,
        //    loadedCallback: function () { }
        //});
       
    }

    bindEvents() {
        $('#btnNewObject').click(ParametrizationInsurancesObjects.Clear);
        $('#inputInsurancesObject').on('buttonClick', this.SearchInsurancesObject);
        $('#btnExit').click(this.Exit);
        $('#btnAcceptObject').click(this.AddObject);
        $('#btnSave').click(this.SaveInsurencesObjects);
        $('#listViewInsurancesObjects').on('rowEdit', ParametrizationInsurancesObjects.ShowData);
        $('#listViewInsurancesObjects').on('rowDelete', this.DeleteItemInsurenceObject);
        $('#btnExport').click(this.sendExcelInsurancesObjects);

        //$("#btnSearchAdvInsurancesObject").on("click", this.SearchAdvInsurancesObject);
        //$("#btnCancelSearchAdv").on("click", this.CancelSearchAdv);
        //$("#btnOkSearchAdv").on("click", this.OkSearchAdv);
    }

       

    //CancelSearchAdv() {
    //    ParametrizationInsurancesObjects.HideSearchAdv();
    //}

    //OkSearchAdv() {
    //    let data = $("#lvSearchAdvInsurancesObjects").UifListView("getSelected");
    //    if (data.length === 1) {
    //        let object =
    //        {
    //            Id: data[0].Id,
    //            Description: data[0].Description,
    //            SmallDescription: data[0].SmallDescription,
    //            IsDeraclarative: data[0].IsDeraclarative,
    //            DeclarativeDescription: data[0].DeclarativeDescription
    //        };
    //        if (object.IsDeraclarative == true) {
    //            object.DeclarativeDescription = Resources.Language.IsDeraclarative;
    //        }
    //        var lista = $("#listViewInsurancesObjects").UifListView('getData');
    //        var index = lista.findIndex(function (item) {
    //            return item.Id = object.Id;
    //        });
    //        ParametrizationInsurancesObjects.ShowData(null, object, index);
    //    }
    //    ParametrizationInsurancesObjects.HideSearchAdv();
    //}

    static ShowSearchAdv(data) {
        $("#lvSearchAdvInsurancesObjects").UifListView({
            displayTemplate: "#InsurancesObjectsTemplate",
            selectionType: "single",
            height: 400
        });
        $("#lvSearchAdvInsurancesObjects").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvInsurancesObjects").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvInsurancesObjects.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvInsurancesObjects.hide();
    }

    AddObject() {
        $("#formInsurancesObject").validate();
        if ($("#formInsurancesObject").valid()) {
            var object = ParametrizationInsurancesObjects.GetForm();
            if (object.IsDeraclarative == true) {
                object.DeclarativeDescription = Resources.Language.IsDeraclarative;
            }
            if (objectIndex == null) {
                var lista = $("#listViewInsurancesObjects").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == object.Description.toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistInsurancesObject, 'autoclose': true });
                }
                else {
                    ParametrizationInsurancesObjects.SetStatus(object);
                    $("#listViewInsurancesObjects").UifListView("addItem", object);
                }
            }
            else {
                ParametrizationInsurancesObjects.SetStatus(object);
                $('#listViewInsurancesObjects').UifListView('editItem', objectIndex, object);
            }
            ParametrizationInsurancesObjects.Clear();
        }
    }

    SaveInsurencesObjects() {
        ParametrizationInsurancesObjects.SetListToSend();
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/InsurancesObjects/SaveInsurencesObjects',
            data: JSON.stringify({
                createInsurencesObjectsModel: glbObjectsCreate,
                updateInsurencesObjectsModel: glbObjectsUpdate,
                deleteInsurencesObjectsModel: glbObjectsDelete
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                ParametrizationInsurancesObjects.RefreshList();
                ParametrizationInsurancesObjects.LoadListViewInsurancesObjects(data.result.data);
                $.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true })
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
            }
        })
    }

    DeleteItemInsurenceObject(event, data, index) {
        var objects = $("#listViewInsurancesObjects").UifListView('getData');
        $("#listViewInsurancesObjects").UifListView({ source: null, displayTemplate: "#InsurancesObjectsTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });

        $.each(objects, function (index, value) {
            if (this.Id == data.Id && this.Description == data.Description) {
                value.Status = 'Deleted';
                glbObjectsDelete.push(value);
            }
            else {
                $("#listViewInsurancesObjects").UifListView("addItem", this);
            }
        });
        ParametrizationInsurancesObjects.Clear();
    }

    SearchInsurancesObject() {
        var inputObject = $('#inputInsurancesObject').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/InsurancesObjects/GetInsurancesObjectByDescription',
                data: JSON.stringify({ description: inputObject }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var insurancesObjects = data.result;
                    if (data.result.length > 1) {
                        ParametrizationInsurancesObjects.ShowSearchAdv(insurancesObjects);
                    }
                    else {

                        var object = {};
                        $.each(insurancesObjects, function (key, value) {
                            object =
                                {
                                    Id: this.Id,
                                    Description: this.Description,
                                    SmallDescription: this.SmallDescription,
                                    IsDeraclarative: this.IsDeraclarative,
                                    DeclarativeDescription: this.DeclarativeDescription
                                };
                            if (object.IsDeraclarative == true) {
                                object.DeclarativeDescription = Resources.Language.IsDeraclarative;
                            }
                        })
                        var lista = $("#listViewInsurancesObjects").UifListView('getData')
                        var index = lista.findIndex(function (item) {
                            return item.Id = object.Id;
                        });
                        ParametrizationInsurancesObjects.ShowData(null, object, index);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    sendExcelInsurancesObjects() {
        ParametrizationInsurancesObjects.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    SelectSearch() {
        var list = $("#listViewInsurancesObjects").UifListView('getData')
        var index = $(this).children()[0].innerHTML;
        var object = [];
        $.each(list, function (key, value) {
            if (value.Id == index) {
                object.push(value);
            }
        });
        ParametrizationInsurancesObjects.ShowData(null, object[0], index);
        $('#inputInsurancesObject').val("");
        $('#modalDefaultSearch').UifModal("hide");
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    static LoadListViewInsurancesObjects(data) {
        $("#listViewInsurancesObjects").UifListView({ displayTemplate: "#InsurancesObjectsTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(data, function (key, value) {
            var object =
                {
                    Id: this.Id,
                    Description: this.Description,
                    SmallDescription: this.SmallDescription,
                    IsDeraclarative: this.IsDeraclarative,
                    DeclarativeDescription: this.DeclarativeDescription
                };
            if (object.IsDeraclarative == true) {
                object.DeclarativeDescription = Resources.Language.IsDeraclarative;
            }
            $("#listViewInsurancesObjects").UifListView("addItem", object);
        })
    }

    static GetForm() {
        var data = {
        };
        $("#formInsurancesObject").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Description = $("#inputLongDescription").val();
        data.SmallDescription = $("#inputShortDescription").val();
        data.IsDeraclarative = $("#chkIsDeclarative").is(":checked");
        data.Id = $("#hiddenId").val()
        return data;
    }

    static ShowData(event, result, index) {
        ParametrizationInsurancesObjects.Clear();
        if (result.Id != undefined) {
            objectIndex = index;
            $("#inputLongDescription").val(result.Description);
            $("#inputShortDescription").val(result.SmallDescription);
            $("#hiddenId").val(result.Id);
            if (result.IsDeraclarative) {
                $('#chkIsDeclarative').prop("checked", true);
            }
        }
    }

    static GetAllInsurancesObjects() {
        return $.ajax({
            type: 'POST',
            url: 'GetInsurancesObjectByDescription',
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

    static SetStatus(object) {
        if (object.Id != "") {
            object.Status = "update";
        }
        else {
            object.Status = "create";
        }
    }

    static SetListToSend() {
        var objectsData = $('#listViewInsurancesObjects').UifListView('getData');
        $.each(objectsData, function (key, value) {
            if (value.Status == "create") {
                glbObjectsCreate.push(value);
            }
            else if (value.Status == "update") {
                glbObjectsUpdate.push(value);
            }
        })
    }

    static Clear() {
        $("#hiddenId").val("");
        $("#Id").val(0);
        $("#chkIsDeclarative").prop("checked", "");
        $("#inputLongDescription").val("");
        $("#inputShortDescription").val("");
        objectIndex = null;
    }

    static RefreshList() {
        glbObjectsCreate = [];
        glbObjectsUpdate = [];
        glbObjectsDelete = [];
    }
}