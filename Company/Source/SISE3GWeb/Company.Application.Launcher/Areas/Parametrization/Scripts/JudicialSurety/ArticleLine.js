var objectToAdd = [];
var objectToDelete = [];
var objectToUpdate = [];
var glbElementEdit = null;

class ArticleLine extends Uif2.Page {
    getInitialState() {
        ArticleLine.LoadArticlesLine();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }
    bindEvents() {
        $('#inputArticleLine').on('buttonClick', ArticleLine.SearchArticuleLine);
        $('#listViewArticleLine').on('rowEdit', ArticleLine.ShowData);
        $("#inputLongDescription").TextTransform(ValidatorType.UpperCase);
        $("#inputSmallDescription").TextTransform(ValidatorType.UpperCase);
        $('#btnAcceptObject').click(ArticleLine.AddObject);
        $('#btnNewObject').click(ArticleLine.clearControls);
        $("#btnSave").click(ArticleLine.SaveObjects);
    }

    static getObjectModel(objectModel) {
        var returnObject = [];
        $.each(objectModel, function (key, value) {
            var object = new Object();
            object.ArticleLineCd = parseInt(value.ArticleLineCd);
            object.SmallDescription = value.SmallDescription;
            object.Description = value.Description;
            object.Enabled = value.Enabled;
            returnObject.push(object);
        });
        return returnObject;
    }

    static SaveObjects() {
        if (objectToAdd.length > 0 || objectToDelete.length > 0 || objectToUpdate.length > 0) {
            if (objectToAdd.length > 0) {
                objectToAdd = ArticleLine.getObjectModel(objectToAdd)
            }
            if (objectToDelete.length > 0) {
                objectToDelete = ArticleLine.getObjectModel(objectToDelete)
            }
            if (objectToUpdate.length > 0) {
                objectToUpdate = ArticleLine.getObjectModel(objectToUpdate)
            }
            ArticleLineRequest.UpdateArticleLine(objectToDelete, objectToUpdate, objectToAdd).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { type: 'success', message: data.result, autoclose: false });
                }
                else {
                    $.UifNotify('show', { type: 'danger', message: data.result, autoclose: false });
                }
                objectToAdd = [];
                objectToDelete = [];
                objectToUpdate = [];
                $("#listViewArticleLine").UifListView('clear');
                ArticleLine.LoadArticlesLine();
            });
        }
    }

    static ShowData(event, result, index) {
        if (result != undefined && result != null) {
            result.index = index;
            ArticleLine.setData(result);
        }

    }

    static SearchArticuleLine(event, data, index) {
        var inputObject = $('#inputArticleLine').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $("#lvSearchAdvArticleLine").UifListView("clear");
            ArticleLineRequest.LoadSearchArticleLine(inputObject).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        $("#lvSearchAdvArticleLine").UifListView("clear");
                        $.each(data.result, function (key, value) {
                            $("#lvSearchAdvArticleLine").UifListView("addItem", value);
                        });
                        ArticleLineSearch.SearchAdvAdvArticleLine();
                    }
                }
            });
        }
    }

    static clearControls() {
        $("#inputLongDescription").attr("disabled", false);
        $("#inputSmallDescription").attr("disabled", false);
        $('#chkEnabledArticleLine').attr("disabled", false);
        $('#btnAcceptObject').attr("disabled", false);

        $("#inputLongDescription").val("");
        $("#inputSmallDescription").val("");
        $('#chkEnabledArticleLine').prop('checked', false);
        glbElementEdit = null;
    }


    static AddObject() {

        var lista = $("#listViewArticleLine").UifListView('getData');
        var existObjectinList = [];
        if (glbElementEdit == null) {
            existObjectinList = lista.filter(function (item) {
                return item.Description == $("#inputLongDescription").val() && item.SmallDescription == $("#inputSmallDescription").val()
            });
        }
       
        if (existObjectinList.length > 0) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.ErrorExpansesTaxes, autoclose: true });
        }
        else {
            if (glbElementEdit != null) {
                glbElementEdit.Description = $("#inputLongDescription").val();
                glbElementEdit.SmallDescription = $("#inputSmallDescription").val();
                glbElementEdit.Enabled = $('#chkEnabledArticleLine').is(':checked');
                

                if (glbElementEdit.ArticleLineCd > 0) {
                    var existObject = objectToUpdate.filter(function (item) {
                        return item.ArticleLineCd == glbElementEdit.ArticleLineCd
                    });
                    if (existObject.length > 0) {
                        objectToUpdate = objectToUpdate.filter(function (item) {
                            return item.ArticleLineCd != glbElementEdit.ArticleLineCd
                        });
                    }
                    objectToUpdate.push(glbElementEdit);
                    $("#listViewArticleLine").UifListView("editItem", glbElementEdit.index, glbElementEdit);
                }
               
                glbElementEdit = null;
            } else {
                var newObject = new Object();
                newObject.allowEdit = false;
                newObject.Description = $("#inputLongDescription").val();
                newObject.SmallDescription = $("#inputSmallDescription").val();
                newObject.Enabled = $('#chkEnabledArticleLine').is(':checked');
                newObject.ArticleLineCd = 0;
                objectToAdd.push(newObject);
                $("#listViewArticleLine").UifListView("addItem", newObject);
            }
        }
        ArticleLine.clearControls();
        
    }

    static LoadArticlesLine() {

        ArticleLineRequest.LoadArticlesLine().done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#listViewArticleLine").UifListView({
                        displayTemplate: "#ArticleLineTemplate",
                        sourceData: data.result,
                        edit: true,
                        delete: true,
                        customAdd: true,
                        customEdit: true,
                        height: 300,
                        deleteCallback: ArticleLine.DeleteItemArticleLine
                    });
                }
                else {
                    $.UifNotify('show', { type: 'info', message: Resources.Language.getDataError, autoclose: true });
                }
            }

        });
    }

    static setData(data) {
        if (data.index == undefined || data.index == null) {
            var lista = $("#listViewArticleLine").UifListView('getData');
            $.each(lista, function (key, value) {
                if (value.ArticleLineCd == data.ArticleLineCd) {
                    data.index = key;
                    data.SmallDescription = value.SmallDescription;
                    data.Description = value.Description;
                    data.Enabled = value.Enabled;
                }
            
            });

            var existDelete = objectToDelete.filter(function (item) {
                return item.ArticleLineCd == data.ArticleLineCd
            });
            if (existDelete.length > 0) {
                $("#inputLongDescription").attr("disabled", true);
                $("#inputSmallDescription").attr("disabled", true);
                $('#chkEnabledArticleLine').attr("disabled", true);
                $('#btnAcceptObject').attr("disabled", true);
            }
               
        }
        $("#inputLongDescription").val(data.Description);
        $("#inputSmallDescription").val(data.SmallDescription);
        $('#chkEnabledArticleLine').prop('checked', data.Enabled);
        glbElementEdit = data;
    }

    static DeleteItemArticleLine(event, data) {
        event.resolve();
        data.allowEdit = false;
        data.allowDelete = false;
        data.Status = ParametrizationStatus.Delete;
        if (data.ArticleLineCd > 0) {
            var existObject = objectToUpdate.filter(function (item) {
                return item.ArticleLineCd == data.ArticleLineCd
            });
            if (existObject.length > 0) {
                objectToUpdate = objectToUpdate.filter(function (item) {
                    return item.ArticleLineCd != data.ArticleLineCd
                });
            }
            $("#listViewArticleLine").UifListView("addItem", data);
            objectToDelete.push(data);
        }
        else {
            var existObject = objectToAdd.filter(function (item) {
                return item.Description == $("#inputLongDescription").val() && item.SmallDescription == $("#inputSmallDescription").val()
            });
            if (existObject.length > 0) {
                objectToAdd = objectToAdd.filter(function (item) {
                    return item.Description != $("#inputLongDescription").val() && item.SmallDescription != $("#inputSmallDescription").val()
                });
            }
        }
        ArticleLine.clearControls();
       
    }
}