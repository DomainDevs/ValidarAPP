// Instanciar variables de entorno
var glbExpenseIndex = null;
var glbExpenseStatus = null;
var glbExpenseCreate = [];
var glbExpenseAdded = [];
var glbExpenseModified = [];
var glbExpenseDeleted = [];
var glbRules = [];
var idRow = null, idDBRow = null;
var dropDownSearchAdvExpense = null;

var StatusEnum = {
    Original: 1,
    Create: 2,
    Update: 3,
    Delete: 4,
    Error: 5
};


// Clase procesos Asyncronos
class asynchronousProcess {
   
    // Obtener las reglas de Negocio
    static GetRules() {       
        return $.ajax({
            type: "GET",
            url: rootPath + "Parametrization/Expense/GetRules",
            dataType: "json",
            contentTYpe: "application/json; charset=utf-8"
        });
    }

    // Obtener por Ajax GetDataSectionExpenses y llenar el ListView
    static GetDataSectionExpenses() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Parametrization/Expense/GetDataSectionExpenses",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetValidationButtonUser() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Expense/ValidateButton',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class Expense extends Uif2.Page {
    // Eventos Iniciales
    getInitialState() {
        // Definir el ListView 
        $("#listviewSectionExpenses").UifListView({
            displayTemplate: "#SectionExpensesTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.deleteExpense,
            customAdd: true,
            customEdit: true,
            height: 300
        });
        $("#groupRateTypes").hide();
        $("#groupRuleTypes").hide();
        Expense.GetValidationButtonUser();
        // Convertir a Mayusculas el texto de los input
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);

                

        // Carga de datos para selectRateTypes
        Expense.GetRateTypes().done(function (data) {
            if (data.success) {                
                $("#selectRateType").UifSelect({ sourceData: data.result.RateTypeServicesModel })
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        // Carga de datos para listviewSectionExpenses
        asynchronousProcess.GetDataSectionExpenses().done(function (data) {
            Expense.loadExpenses(data);
        });
      

        // Carga de datos para selectExecutionTypes
        Expense.GetExecutionTypes().done(function (data) {
            if (data.success) {
                $("#selectExecutionType").UifSelect({ sourceData: data.result })
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        dropDownSearchAdvExpense = uif2.dropDown({
            source: rootPath + 'Parametrization/Expense/ExpenseSearch',
            element: '#inputSearchIssuanceExpenses',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: Expense.LoadEventsAdvanzedSearch
        })
    }

    // Eventos en tiempo de ejecucion
    bindEvents() {
        
        // Mostrar u Ocultar groupRateTypes al cambio del selectExecutionType
        $("#selectExecutionType").on("itemSelected", Expense.showRate);

        // Evento de Guardar cambios contra DB
        $("#btnSaveExpense").on("click", Expense.SaveExpense);

        // Evento de exportar Expense
        $("#btnExport").on("click", this.sendExcelExpense);

        // Evento de Editar un registro
        $("#listviewSectionExpenses").on("rowEdit", Expense.editExpense);

        // Evento de borrar un registro
        $("#listviewSectionExpenses").on("rowDelete", this.deleteExpense);

        // Agregar gastos 
        $("#btnExpenseAccept").on("click", Expense.Addexpense);

        // Resetar formulario
        $("#btnNewExpense").on("click", Expense.newExpense);

        // Controlar envio de formulario
        $("#formExpense").on("submit", function () {
            return false;
        });

        // Cargar datos de la tabla de Gastos al formulario
        $("#tableResultsSearchs tbody").on("click", "tr", this.SelectSearch);

        // Cargar datos de la tabla de Reglas de negocio al formulario
        $("#tableResultsSearchsRules tbody").on("click", "tr", this.SelectRulesSearch);

        // Buscar Gasto entre el ListView
        $("#inputSearchIssuanceExpenses").on("buttonClick", this.searchExpense);

        // Buscar Regla de Negocio
        $("#inputRuleSet").on("buttonClick", this.searchRuleSet);

    }

    static GetValidationButtonUser() {
        
        asynchronousProcess.GetValidationButtonUser().done(function (data) {
            if (data.result) {
                // Definir el ListView 
                $("#listviewSectionExpenses").UifListView({displayTemplate: "#SectionExpensesTemplate",edit: true,
                    delete: false,deleteCallback: this.deleteExpense,customAdd: true,customEdit: true,height: 300});
                $('#btnExpenseAccept').hide();
                $('#btnNewExpense').hide();
                $('#btnSaveExpense').hide();
                $('#btnExport').hide();
                $('#btnNewLimitRc').hide();                
            }
        });
    }

    sendExcelExpense() {
        Expense.GenerateFileExpenseToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }

    static GenerateFileExpenseToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Expense/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    

    static LoadEventsAdvanzedSearch() {
        $("#lvSearchAdvExpense").UifListView({
            displayTemplate: "#SectionExpensesTemplate",
            selectionType: "single",
            height: 300
        });
        $("#btnCancelSearchAdv").on("click", Expense.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", Expense.OkSearchAdv);
    }
    // Obtener por Ajax ExecutionTypes
    static GetExecutionTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Parametrization/Expense/GetExecutionTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    // Obtener por Ajax RateTypes
    static GetRateTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Parametrization/Expense/GetRateTypes",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    // Cargar datos al ListView
    static loadExpenses(data) {
        if (data.success) {

            // Limpiar ListView
            $("#listviewSectionExpenses").UifListView("clear");

            // Llenar objeto con los datos obtenidos
            $.each(data.result, function (key, value) {
                if (this.IsMandatory != false) {
                    this.MandatoryValue = Resources.Language.LabelMandatory;
                } else {
                    this.MandatoryValue = "";
                }

                if (this.IsInitially != false) {
                    this.InitiallyValue = Resources.Language.LabelInitialInclude;
                } else {
                    this.InitiallyValue = "";
                }
                if (this.RuleSetServiceQueryModel.Id != null) {
                    this.Execution = Resources.Language.LabelBusinessRule + ": " + this.RuleSetServiceQueryModel.description;
                } else {
                    this.Execution = Resources.Language.LabelRate + ": " + this.Rate + "% (" + this.RateTypeServiceQueryModel.description + ")";

                }
                // Agregar objeto al ListView
                $("#listviewSectionExpenses").UifListView("addItem", this);
            });
        }
    }
     // Funcion para guardar todos los cambios
   
    static SaveExpense() {
        var listExpense = $("#listviewSectionExpenses").UifListView('getData');
        var listExpense = listExpense.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        if (listExpense.length > 0) {
            request('Parametrization/Expense/SaveExpense', JSON.stringify({ lstExpense: listExpense }), 'POST', AppResources.ErrorSavePaymentMethod, Expense.confirmCreateExpense);

        }
        else {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ExpenseNotDataOperation, 'autoclose': true
            })
        }
        asynchronousProcess.GetDataSectionExpenses().done(function (data) {
            Expense.loadExpenses(data);
        });
        
    } 
    
    static confirmCreateExpense(data) {
        Expense.newExpense();
        request('Parametrization/Expense/GetDataSectionExpenses', null, 'GET', AppResources.ErrorSearchPaymentMethod, asynchronousProcess.GetDataSectionExpenses)

        if (data.Message === null) {
            data.Message = 0;
        }
        asynchronousProcess.GetDataSectionExpenses().done(function (data) {
            Expense.loadExpenses(data);
        });
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
            AppResources.Aggregates + ':' + data.TotalAdded + '<br> ' +
            AppResources.Updated + ':' + data.TotalModified + '<br> ' +
            AppResources.Removed + ':' + data.TotalDeleted + '<br> ' +
            AppResources.Errors + ':' + data.Message,
            'autoclose': true
        });
    }

    deleteExpense(event, result) {
        // Valida que el registro sea de DB
        if (result != null && result.Id != null) {

            // Agregar al Array de Registros a Eliminar
            result.SmallDescription = result.SmallDescription;
            result.Description = result.SmallDescription;
			result.StatusTypeService = ParametrizationStatus.Delete;
			result.Abbreviation = result.TinyDescripcion;
			result.allowEdit = false;
			result.allowDelete = false;
			$("#listviewSectionExpenses").UifListView("addItem", result);
        }

        // Continuar con la accion del event
        event.resolve();
    }

    deleteExpense1(event, result) {
        // Valida que el registro sea de DB
        if (result != null && result.Id != null) {

            // Agregar al Array de Registros a Eliminar
            glbExpenseDeleted.push(result)
            glbExpenseStatus = StatusEnum.Delete;
        }

        // Continuar con la accion del event
        event.resolve();
    }

    // Editar regitros de las lista
    static editExpense(event, result, index) {
        if (result != null) {
            $("#inputDescription").val(result.SmallDescription);
            $("#inputAbbreviation").val(result.TinyDescripcion);
            $("#checkMandatory").prop("checked", result.IsMandatory);
            $("#checkInitiallyIncluded").prop("checked", result.IsInitially);
            $("#checkMandatory").prop("checked", result.Mandatory);
            $("#checkInitiallyIncluded").prop("checked", result.InitiallyIncluded);
            glbExpenseIndex = index;
            glbExpenseStatus = StatusEnum.Update;
            idRow = index;
            idDBRow = result.Id;
            if (result.RuleSetServiceQueryModel.Id == null) {
                $("#selectExecutionType").val("1");
                $("#selectRateType").attr("data-rule-id", result.RateTypeServiceQueryModel.Id);
                $("#selectRateType").attr("data-rule-name", result.RateTypeServiceQueryModel.description);
                $("#selectRateType").val(result.RateTypeServiceQueryModel.Id);               
                $("#inputRate").val(result.Rate);
            } else {
                $("#inputRate").val("");
                $("#selectRateType").UifSelect('setSelected', null);
                $("#selectExecutionType").val("2");
                $("#inputRuleSet").attr("data-rule-id", result.RuleSetServiceQueryModel.Id);
                $("#inputRuleSet").attr("data-rule-name", result.RuleSetServiceQueryModel.description);
                $("#inputRuleSet").val(result.RuleSetServiceQueryModel.description);
            }
            $("#selectExecutionType").change();
        }
    }
    //Agregar un nuevo registro a lista V2

    static Addexpense() {  
        $("#formExpense").validate();
        if ($("#formExpense").valid()) {
            var group = Expense.GetForm();
            if (glbExpenseIndex == null) {
                var lista = $("#listviewSectionExpenses").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.SmallDescription.toUpperCase() == group.SmallDescription.toUpperCase();
                });
                if (ifExist.length > 0 && glbExpenseStatus != StatusEnum.Create) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExistExpense, 'autoclose': true });
                }
                else {
                    Expense.SetStatus(group, StatusEnum.Create);
                    $("#listviewSectionExpenses").UifListView("addItem", group);
                }
            }
            else {
                if (glbExpenseIndex != undefined && glbExpenseStatus != undefined) {
                    Expense.SetStatus(group, glbExpenseStatus);
                } else {
                    Expense.SetStatus(group, StatusEnum.Update);
                }
                $('#listviewSectionExpenses').UifListView('editItem', glbExpenseIndex, group);
            }
            
            
        }
        Expense.newExpense();
    }
    // Crear el objeto a enviar o la data
    static GetForm() {
        var data = {
        };
        $("#formExpense").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Id = idDBRow;
        data.InfringementCode = $("#inputInfringementCode").val();
        data.SmallDescription = $("#inputDescription").val();
        data.TinyDescripcion = $("#inputAbbreviation").val();
        data.IsMandatory = $("#checkMandatory").prop("checked");
        data.Mandatory = $("#checkMandatory").prop("checked");
        data.IsInitially = $("#checkInitiallyIncluded").prop("checked");
        data.InitiallyIncluded = $("#checkInitiallyIncluded").prop("checked");
        data.ComponentClass = 3;
        data.ComponentType = 2;
        if (data.IsMandatory == true) {
            data.MandatoryValue = Resources.Language.LabelMandatory;
        } else {
            data.MandatoryValue = "";
        }
        if (data.InitiallyIncluded == true) {
            data.InitiallyValue = Resources.Language.LabelInitialInclude;
        } else {
            data.InitiallyValue = "";
        }
        if ($("#selectExecutionType").val() == 1) {
            data.RateTypeName = $("#selectRateType option:selected").text().toUpperCase();
            data.RateTypeServiceQueryModel = $("#selectRateType").val();
            data.Rate = $("#inputRate").val();
            data.RuleSet = null;
            data.Execution = Resources.Language.LabelRate + ": " + data.Rate + "% (" + data.RateTypeName + ")";

        } else {
            
            data.RateTypeServiceQueryModel = 1;
            data.RuleSet = $("#inputRuleSet").attr("data-rule-id");
            data.RuleSetName = $("#inputRuleSet").attr("data-rule-name");
            data.Execution = Resources.Language.LabelBusinessRule + ": " + data.RuleSetName;
            data.RateType = 1;
        }
        return data;
    }   

    // Agrega el estado de registro a enviar ExecuteOperation
    static SetStatus(object, status) {
        object.StatusTypeService = status;
    }
    // Agregar un nuevo registro a la lista
    addExpense() {

        //Valida el formulario
        if ($("#formExpense").valid()) {
            
            // Asignar valores al objeto
            var objectExpense = {
                SmallDescription: $("#inputDescription").val(),
                TinyDescripcion: $("#inputAbbreviation").val(),
                IsMandatory: $("#checkMandatory").prop("checked"),
                IsInitially: $("#checkInitiallyIncluded").prop("checked"),
                id: idDBRow,
                ComponentClass: 3,
                ComponentType: 2
            };
            if (objectExpense.IsMandatory == true) {
                objectExpense.MandatoryValue = Resources.Language.LabelMandatory;
            } else {
                objectExpense.MandatoryValue = "";
            }
            if (objectExpense.InitiallyIncluded == true) {
                objectExpense.InitiallyValue = Resources.Language.LabelInitialInclude;
            } else {
                objectExpense.InitiallyValue = "";
            }
            if ($("#selectExecutionType").val() == 1) {
                objectExpense.RateTypeName = $("#selectRateType option:selected").text().toUpperCase();                
                objectExpense.RateTypeServiceQueryModel = $("#selectRateType").val();
                objectExpense.Rate = $("#inputRate").val();
                objectExpense.RuleSet = null;
                objectExpense.Execution = Resources.Language.LabelRate + ": " + objectExpense.Rate + "% (" + objectExpense.RateTypeServiceQueryModel + ")";
                
            } else {
                objectExpense.RateTypeServiceQueryModel = 1;
                objectExpense.RuleSet = $("#inputRuleSet").attr("data-rule-id");
                objectExpense.RuleSetName = $("#inputRuleSet").attr("data-rule-name");
                objectExpense.Execution = Resources.Language.LabelBusinessRule + ": " + objectExpense.RuleSetName;
            }
            
            // Obtener los registro de la ListView
            var ListExpenses = $("#listviewSectionExpenses").UifListView('getData');

            // Variables de Comprobacion
            var stateRow = null;
            var indexRow = null;

            // Iteración de los registros de la ListView
            $.each(ListExpenses, function (i, val) {
                if (idRow == i) {
                    indexRow = i;
                    if(idDBRow != null){
                        stateRow = "Modified";
                    }
                }
            });
            if (stateRow == null) {
                stateRow = "Added";
            }

            idRow = null;
            idDBRow = null;

            objectExpense.Status = stateRow;

            // Ejecucion de la accion contra la ListView
            switch (stateRow) {
                case "Added":
                    if (indexRow == null) {
                        $("#listviewSectionExpenses").UifListView("addItem", objectExpense);
                    } else {
                        $("#listviewSectionExpenses").UifListView("editItem", indexRow, objectExpense);
                    }
                    break;
                case "Modified":
                    $("#listviewSectionExpenses").UifListView("editItem", indexRow, objectExpense);
                    break;
            }

            Expense.newExpense();
        }
        
    }

    // Limpiar interfaz para nuevo Gasto
    static newExpense() {
        $("#formExpense")[0].reset();
        $("#selectExecutionType").change();
        $("#inputDescription").focus();
        idRow = null; 
        ClearValidation("#formExpense"); 
        glbExpenseIndex = null;
    }

    // Funcion de ocultar groupRateTypes y groupRuleTypes
    static showRate() {
        switch ($("#selectExecutionType").val()) {            
            case "1":                
                $("#groupRuleTypes").hide();
                $("#groupRateTypes").show();
                break;
            case "2":
                $("#groupRateTypes").hide();
                $("#groupRuleTypes").show();
                break;
            default:
                $("#groupRuleTypes").hide();
                $("#groupRateTypes").hide();
                break;
        }
    }

    // ejecuta la funcion para buscar por el valor ingresado
    searchExpense(event) {
        Expense.searchExpenses($("#inputSearchIssuanceExpenses").val());
    }

    // Busca las coincidencias del parametro con las reglas de negocio
    searchRuleSet(event, selectedItem) {
        asynchronousProcess.GetRules().done(function (data) {
            if (data.success) {
                console.log(data.result);
                glbRules = data.result;
                var find = false, data = [];
                $.each(glbRules.RuleSetServiceQueryModel, function (i, value) {
                    if (
                        (value.description.toLowerCase().sistranReplaceAccentMark().includes(selectedItem.toLowerCase().sistranReplaceAccentMark()))
                        ||
                        (value.Id == selectedItem)
                    ) {
                        value.key = i;
                        data.push(value);
                        find = true;
                    }
                });
                Expense.showRulesData(glbRules.RuleSetServiceQueryModel, glbRules.RuleSetServiceQueryModel.length);
                if (find == false) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.RuleSetNotFound, 'autoclose': true
                    });
                }  
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': "Error: No se pueden cargar las reglas de negocio", 'autoclose': true });
            }
        });

        
        
    }

    // Busca por el valor ingresado
    static searchExpenses(description) {
        var find = false, data = [];
        var search = $("#listviewSectionExpenses").UifListView('getData');
        if (description.length < 3) {
            asynchronousProcess.GetDataSectionExpenses().done(function (data) {
                Expense.loadExpenses(data);
            });
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true
            });        

        } else {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/Expense/GetExpenseByDescription',
                data: JSON.stringify({ description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var groups = data.result;
                    if (data.result.length > 1) {
                        Expense.ShowSearchAdv(groups);
                    }
                    else {
                        $.each(groups, function (key, value) {
                            var lista = $("#listviewSectionExpenses").UifListView('getData')
                            var index = lista.findIndex(function (item) {
                                return item.Id == value.Id;
                            });
                            Expense.ShowData( data, index);
                        });
                        
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorDescription, 'autoclose': true });
                }
            });
        }
        $('#inputSearchIssuanceExpenses').val('');
    }

    // Carga en listView 
    static ShowSearchAdv(data) {
      
        $("#listviewSectionExpenses").UifListView("clear");
        if (data) {
            $.each(data, function (key, value) {
                if (this.IsMandatory != false) {
                    this.MandatoryValue = Resources.Language.LabelMandatory;
                } else {
                    this.MandatoryValue = "";
                }
                if (this.IsInitially != false) {
                    this.InitiallyValue = Resources.Language.LabelInitialInclude;
                } else {
                    this.InitiallyValue = "";
                }
                if (this.RuleSet != null) {
                    this.Execution = Resources.Language.LabelBusinessRule + ": " + this.RuleSetName;
                } else {
                    this.Execution = Resources.Language.LabelRate + ": " + this.Rate + " % (" + this.RateTypeServiceQueryModel + ")";
                }

                // Agregar objeto al ListView
                $("#listviewSectionExpenses").UifListView("addItem", this);
            });
        }
       
    }
    // Asigna los datos de la Regla de negocio al formulario
    static showRulesData(result, index) {
        var resultUnique = null;
        if(result.length == 1){
            index = result[0].Id;
            resultUnique = result[0];
        }else if(result.length > 1){
            $("#tableResultsSearchsRules").UifDataTable('clear');
            $("#tableResultsSearchsRules").UifDataTable('addRow', result);
            $("#modalDefaultSearchRules").UifModal('showLocal', Resources.Language.IssuanceExpenses);
        }
        if(result.length == undefined){
            resultUnique == result;
        }
        if(resultUnique != null){
            $("#inputRuleSet").val(resultUnique.description + " (" + resultUnique.Id + ")");
            $("#inputRuleSet").attr("data-rule-id", resultUnique.Id);
            $("#inputRuleSet").attr("data-rule-name", resultUnique.description);
        }
    }

    // carga la data si la busqueda da solo un registro o muestra el modal si hay varios
    static ShowData(result, index) {
        var resultUnique = null;
        if (result.result.length == 1) {
            resultUnique = result.result[0];
        } 
        if (resultUnique != null) {
            idRow = index;
            $("#inputDescription").val(resultUnique.SmallDescription);
            $("#inputAbbreviation").val(resultUnique.TinyDescripcion);
            $("#checkMandatory").prop("checked", resultUnique.IsMandatory);
            $("#checkInitiallyIncluded").prop("checked", resultUnique.IsInitially);     
            glbExpenseIndex = index;
            idRow = index;
            idDBRow = result.id;
            if (result.RuleSet == null) {
                $("#selectExecutionType").val("1");
                $("#selectRateType").UifSelect('setSelected', resultUnique.RateTypeServiceQueryModel);

                $("#inputRate").val(resultUnique.Rate);
            } else {
                $("#inputRate").val("");
                $("#selectRateType").UifSelect('setSelected', null);
                $("#selectExecutionType").val("2");
                $("#inputRuleSet").attr("data-rule-id", resultUnique.RuleSet);
                $("#inputRuleSet").attr("data-rule-name", resultUnique.RuleSetName);
                $("#inputRuleSet").val(resultUnique.RuleSetName + " ("+resultUnique.RuleSet+")");
            }
            $("#selectExecutionType").change();
        }
    }

    // Obtiene la Desripcion del registro seleccionado
    SelectSearch() {
        Expense.getExpenseById($(this).children()[0].innerHTML, "");
        $('#modalDefaultSearch').UifModal("hide");
    };

    SelectRulesSearch() {
        $('#modalDefaultSearchRules').UifModal("hide");
        var data = Expense.getRulesById($(this).children()[0].innerHTML, "");
        var id = $(this).children()[0].innerHTML; 
        $("#inputRuleSet").attr("data-rule-id", id);
        $("#inputRuleSet").attr("data-rule-name", data.description);
        $("#inputRuleSet").val(data.description);
        

    }

    // Busca entre los datos registros de la tabla de Reglas de Negocio
    static getRulesById(id) {
        var find = false, data = [];
        $.each(glbRules.RuleSetServiceQueryModel, function (key, value) {
            if(value.Id == id){
                data.id = value.id;
                data.description = value.description;
            }
        });
        return data;
    }

    // Buscar los registros por la descripción y las carga a la interfaz
    static getExpenseById(Description) {
        var find = false, data = [], search = $("#listviewSectionExpenses").UifListView('getData');

        $.each(search, function (key, value) {
            if (value.Description == Description) {
                idRow = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        Expense.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ExenseNotFound, 'autoclose': true })
        }
    }
}