var objectToAdd = [];
var objectToDelete = [];
var objectToUpdate = [];
var glbElementEdit = null;

class CourtType extends Uif2.Page {
    getInitialState() {
        CourtType.LoadCourtsType();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }
    bindEvents() {
        $('#inputCourtType').on('buttonClick', CourtType.SearchArticuleLine);
        $('#listViewCourtType').on('rowEdit', CourtType.ShowData);
        $("#DescriptionCd").TextTransform(ValidatorType.UpperCase);
        $("#inputSmallDescription").TextTransform(ValidatorType.UpperCase);
        $('#btnAcceptObject').click(CourtType.AddObject);
        $('#btnNewObject').click(CourtType.clearControls);
        $("#btnSave").click(CourtType.SaveObjects);
    }

    static getObjectModel(objectModel) {
        var returnObject = [];
        $.each(objectModel, function (key, value) {
            var object = new Object();
            object.Id = parseInt(value.Id);
            object.SmallDescription = value.SmallDescription;
            object.Description = value.Description;
            object.Enabled = value.Enabled;
            returnObject.push(object);
        });
        return returnObject;
    }

    static SearchArticuleLine(event, data, index) {
        var inputObject = $('#inputCourtType').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $("#lvSearchAdvCourtType").UifListView("clear");
            CourtTypeRequest.LoadSearchCourtType(inputObject).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        $("#lvSearchAdvCourtType").UifListView("clear");
                        $.each(data.result, function (key, value) {
                            $("#lvSearchAdvCourtType").UifListView("addItem", value);
                        });
                        CourtTypeSearch.SearchAdvAdvCourtType();
                    }
                }
            });
        }
    }

    static SaveObjects() {
        if (objectToAdd.length > 0 || objectToDelete.length > 0 || objectToUpdate.length > 0) {
            if (objectToAdd.length > 0) {
                objectToAdd = CourtType.getObjectModel(objectToAdd)
            }
            if (objectToDelete.length > 0) {
                objectToDelete = CourtType.getObjectModel(objectToDelete)
            }
            if (objectToUpdate.length > 0) {
                objectToUpdate = CourtType.getObjectModel(objectToUpdate)
            }
            CourtTypeRequest.UpdateCourtType(objectToDelete, objectToUpdate, objectToAdd).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { type: 'success', message: data.result, autoclose: false });
                }
                else {
                    $.UifNotify('show', { type: 'danger', message: data.result, autoclose: false });
                }
                objectToAdd = [];
                objectToDelete = [];
                objectToUpdate = [];
                $("#listViewCourtType").UifListView('clear');
                CourtType.LoadCourtsType();
            });
        }
    }

    static AddObject() {
        var lista = $("#listViewCourtType").UifListView('getData');
        var existObjectinList = [];
        if (glbElementEdit == null) {
            existObjectinList = lista.filter(function (item) {
                return item.Description == $("#DescriptionCd").val() && item.SmallDescription == $("#inputSmallDescription").val()
            });
        }
        if (existObjectinList.length > 0) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.ErrorExpansesTaxes, autoclose: true });
        }
        else {
            if (glbElementEdit != null) {
                glbElementEdit.Description = $("#DescriptionCd").val();
                glbElementEdit.SmallDescription = $("#inputSmallDescription").val();
                glbElementEdit.Enabled = $('#chkEnabledCourtType').is(':checked');


                if (glbElementEdit.Id > 0) {
                    var existObject = objectToUpdate.filter(function (item) {
                        return item.Id == glbElementEdit.Id
                    });
                    if (existObject.length > 0) {
                        objectToUpdate = objectToUpdate.filter(function (item) {
                            return item.Id != glbElementEdit.Id
                        });
                    }
                    objectToUpdate.push(glbElementEdit);
                    $("#listViewCourtType").UifListView("editItem", glbElementEdit.index, glbElementEdit);
                }

                glbElementEdit = null;
            } else {
                var newObject = new Object();
                newObject.allowEdit = false;
                newObject.Description = $("#DescriptionCd").val();
                newObject.SmallDescription = $("#inputSmallDescription").val();
                newObject.Enabled = $('#chkEnabledCourtType').is(':checked');
                newObject.Id = 0;
                objectToAdd.push(newObject);
                $("#listViewCourtType").UifListView("addItem", newObject);
            }
        }
        CourtType.clearControls();
    }

    static clearControls() {
        $("#DescriptionCd").attr("disabled", false);
        $("#inputSmallDescription").attr("disabled", false);
        $('#chkEnabledCourtType').attr("disabled", false);
        $('#btnAcceptObject').attr("disabled", false);

        $("#DescriptionCd").val("");
        $("#inputSmallDescription").val("");
        $('#chkEnabledCourtType').prop('checked', false);
        glbElementEdit = null;
    }

    static LoadCourtsType() {

        CourtTypeRequest.LoadCourtsType().done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#listViewCourtType").UifListView({
                        displayTemplate: "#CourtTypeTemplate",
                        sourceData: data.result,
                        edit: true,
                        delete: true,
                        customAdd: true,
                        customEdit: true,
                        height: 300,
                        deleteCallback: CourtType.DeleteItemCourtType
                    });
                }
                else {
                    $.UifNotify('show', { type: 'info', message: Resources.Language.getDataError, autoclose: true });
                }
            }

        });
    }

    static ShowData(event, result, index) {
        if (result != undefined && result != null) {
            result.index = index;
            CourtType.setData(result);
        }

    }
    static setData(data) {
        if (data.index == undefined || data.index == null) {
            var lista = $("#listViewCourtType").UifListView('getData');
            $.each(lista, function (key, value) {
                if (value.Id == data.Id) {
                    data.index = key;
                    data.SmallDescription = value.SmallDescription;
                    data.Description = value.Description;
                    data.Enabled = value.Enabled;
                }

            });

            var existDelete = objectToDelete.filter(function (item) {
                return item.Id == data.Id
            });
            if (existDelete.length > 0) {
                $("#DescriptionCd").attr("disabled", true);
                $("#inputSmallDescription").attr("disabled", true);
                $('#chkEnabledCourtType').attr("disabled", true);
                $('#btnAcceptObject').attr("disabled", true);
            }

        }
        $("#DescriptionCd").val(data.Description);
        $("#inputSmallDescription").val(data.SmallDescription);
        $('#chkEnabledCourtType').prop('checked', data.Enabled);
        glbElementEdit = data;
    }

    static DeleteItemCourtType(event, data) {
        event.resolve();
        data.allowEdit = false;
        data.allowDelete = false;
        data.Status = ParametrizationStatus.Delete;
        if (data.Id > 0) {
            var existObject = objectToUpdate.filter(function (item) {
                return item.Id == data.Id
            });
            if (existObject.length > 0) {
                objectToUpdate = objectToUpdate.filter(function (item) {
                    return item.Id != data.Id
                });
            }
            $("#listViewCourtType").UifListView("addItem", data);
            objectToDelete.push(data);
        }
        else {
            var existObject = objectToAdd.filter(function (item) {
                return item.Description == $("#DescriptionCd").val() && item.SmallDescription == $("#inputSmallDescription").val()
            });
            if (existObject.length > 0) {
                objectToAdd = objectToAdd.filter(function (item) {
                    return item.Description != $("#DescriptionCd").val() && item.SmallDescription != $("#inputSmallDescription").val()
                });
            }
        }
        CourtType.clearControls();
    }
   
}