var glbObjectsCreate = [];
var glbObjectsUpdate = [];
var objectIndex = null;
var dropDownSearch = null;
var formInsuredObject = {};
var index;

class InsuredObject extends Uif2.Page {
    /**
  * @summary 
     *  Metodo que se ejecuta al instanciar la clase     
  */
    getInitialState() {
        //Se cargan los datos en los campos iniciales
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        request('Parametrization/InsuredObject/GetInsuredObjectByDescription', null, 'POST', Resources.Language.ErrorExistInsuredObject, InsuredObject.LoadListViewInsuredObject)
    }

    /**
   * @summary 
       *  Metodo con los eventos de todos los controles 
   */
    bindEvents() {
        $('#btnNewObject').click(InsuredObject.Clear);
        $('#inputInsuredObject').on('buttonClick', this.SearchInsuredObject);
        $('#btnExit').click(this.Exit);
        $('#btnAcceptObject').click(this.AddObject);
        $('#btnSave').click(this.SaveInsurencesObjects);
        $('#listViewInsuredObject').on('rowEdit', InsuredObject.ShowData);      
        $('#btnExport').click(this.sendExcelInsuredObject);

    }      

    AddObject() {
        $("#formInsuredObject").validate();
        if ($("#formInsuredObject").valid()) {
            var object = InsuredObject.GetForm();
            if ($("#Id").val() == "") {
                object.Id = 0;
            }
            else {
                object.Id = parseInt($("#Id").val());
            }
            if (object.IsDeraclarative == true) {
                object.DeclarativeDescription = Resources.Language.IsDeraclarative;
            }
            if (objectIndex == null) {
                var lista = $("#listViewInsuredObject").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == object.Description.toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExistNameInsuredObject, 'autoclose': true });
                }
                else {
                    InsuredObject.SetStatus(object);
                    object.Status = ParametrizationStatus.Create;
                    $("#listViewInsuredObject").UifListView("addItem", object);
                }
            }
            else {
                InsuredObject.SetStatus(object);
				if (object.Id=="0") {
                    object.Status = ParametrizationStatus.Create;
				} else {
                    object.Status = ParametrizationStatus.Update;
				}
                object.Status = ParametrizationStatus.Update;
                $('#listViewInsuredObject').UifListView('editItem', objectIndex, object);
			}
            InsuredObject.Clear();
        }
    }

    SaveInsurencesObjects() {
        var itemModified = [];
        var dataTable = $("#listViewInsuredObject").UifListView('getData');
        $.each(dataTable, function (index, value) {
            if (value.Status == 0 || value.Status == undefined) {
                value.Status = ParametrizationStatus.Original
            }
            itemModified.push(value);
        });

        if (itemModified.length > 0) {
            request('Parametrization/InsuredObject/SaveInsurencesObjects', JSON.stringify({ insurencesObjects: itemModified }), 'POST', Resources.Language.SaveInsuredObject, InsuredObject.ResultSave);
        }
    }

    static ResultSave(data) {
        request('Parametrization/InsuredObject/GetInsuredObjectByDescription', null, 'POST', Resources.Language.ErrorExistInsuredObject, InsuredObject.LoadListViewInsuredObject)
        if (data.message != null && data.message!= "") {
        
            $.UifNotify('show', { 'type': 'info', 'message': data.message, 'autoclose': true });
        }
    }

	static DeleteItemInsurenceObject(deferred, result) {
		deferred.resolve();
        if (result.Id !== 0 && result.Id !== undefined) //Se elimina unicamente si existe en DB
		{
			result.Status = ParametrizationStatus.Delete;
			result.allowEdit = false;
			result.allowDelete = false; 
			$("#listViewInsuredObject").UifListView("addItem", result);
        }
        InsuredObject.Clear();       
    }
   
    SearchInsuredObject() {
        var inputObject = $('#inputInsuredObject').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            request('Parametrization/InsuredObject/GetInsuredObjectByDescription', JSON.stringify({ description: inputObject }), 'POST', Resources.Language.ErrorExistInsuredObject, InsuredObject.ShowSearchAdv)
        } 
        $('#inputInsuredObject').val("");
        $("#lvSearchAdvInsuredObject").UifListView("clear");
    }

    static ShowSearchAdv(data) {
        if (data.length != 0) {          
            if (data.length === 1) {
                var list = $("#listViewInsuredObject").UifListView('getData');
                $.each(list, function (key, value) {
                    if (value.Id == data[0].Id) {
                        index = key;
                    }
                });
                var object =
                    {
                        Id: data[0].Id,
                        Description: data[0].Description,
                        SmallDescription: data[0].SmallDescription,
                        IsDeraclarative: data[0].IsDeraclarative,
                        DeclarativeDescription: data[0].DeclarativeDescription
                    };
                InsuredObject.ShowData(null, object, index);
            } else {
                $.each(data, function (key, value) {
                    var object =
                        {
                            Id: this.Id,
                            Description: this.Description,
                            SmallDescription: this.SmallDescription,
                            IsDeraclarative: this.IsDeraclarative,
                            DeclarativeDescription: this.DeclarativeDescription
                        };
                     $("#lvSearchAdvInsuredObject").UifListView("addItem", object);
                });
               InsuredObjectAdvancedSearch.SearchAdvInsuredObject();
            }
        }
    }

    static DownloadFile(data) {
        DownloadFile(data);
    }
    sendExcelInsuredObject() {
        request('Parametrization/InsuredObject/GenerateFileToExport', null, 'POST', Resources.Language.ErrorExistInsuredObject, InsuredObject.DownloadFile)

    }
    SelectSearch() {
        var list = $("#listViewInsuredObject").UifListView('getData')
        var index = $(this).children()[0].innerHTML;
        var object = [];
        $.each(list, function (key, value) {
            if (value.Id == index) {
                object.push(value);
            }
        });
        InsuredObject.ShowData(null, object[0], index);
        $('#inputInsuredObject').val("");
        $('#modalDefaultSearch').UifModal("hide");
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    static LoadListViewInsuredObject(data) {
        $("#listViewInsuredObject").UifListView({ displayTemplate: "#InsuredObjectTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: InsuredObject.DeleteItemInsurenceObject, height: 300 });
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
            $("#listViewInsuredObject").UifListView("addItem", object);
        })
    }

    static GetForm() {
        var data = {
        };
        $("#formInsuredObject").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Description = $("#inputLongDescription").val();
        data.SmallDescription = $("#inputShortDescription").val();
        data.IsDeraclarative = $("#chkIsDeclarative").is(":checked");
        data.Id = $("#hiddenId").val()
        return data;
    }

    static ShowData(event, result, index) {
        InsuredObject.Clear();
        if (result.Id != undefined) {
            objectIndex = index;
            $("#Id").val(result.Id);
            $("#inputLongDescription").val(result.Description);
            $("#inputShortDescription").val(result.SmallDescription);
            $("#hiddenId").val(result.Id);
            if (result.IsDeraclarative) {
                $('#chkIsDeclarative').prop("checked", true);
            }
            formInsuredObject.bandUpdate = true;
        }
    }

    static SetStatus(object) {
		if (object.Id != "" && object.Id !="0" ) {
            object.Status = ParametrizationStatus.Update;
        }
        else {
            object.Status = ParametrizationStatus.Create;
        }
    }

    static SetListToSend() {
        var objectsData = $('#listViewInsuredObject').UifListView('getData');
        $.each(objectsData, function (key, value) {
            if (value.Status === ParametrizationStatus.Create) {
                glbObjectsCreate.push(value);
            }
            else if (value.Status == ParametrizationStatus.Update) {
                glbObjectsUpdate.push(value);
            }
        })
    }
    /**
       * @summary 
       * Limpiar formulario
    */
    static Clear() {
        $("#hiddenId").val("");
        $("#Id").val(0);
        $("#chkIsDeclarative").prop("checked", "");
        $("#inputLongDescription").val("");
        $("#inputLongDescription").focus();
        $("#inputShortDescription").val("");
        objectIndex = null;
        ClearValidation('#formInsuredObject');
    }

    static RefreshList() {
        glbObjectsCreate = [];
        glbObjectsUpdate = [];
        glbObjectsDelete = [];
    }
}