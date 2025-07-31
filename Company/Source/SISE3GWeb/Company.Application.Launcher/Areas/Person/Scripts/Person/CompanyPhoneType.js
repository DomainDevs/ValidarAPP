var glbCompanyPhoneType = {};
var inputSearch = "";
var modalListType;
var CompanyPhoneTypeIndex = null;
var glbCompanyPhoneTypeAdded = [];
var glbCompanyPhoneTypeModified = [];
var glbCompanyPhoneTypeDelete = [];
var dropDownSearchAdvCompanyPhoneType = null;


$(() => {
    new ParametrizationCompanyPhoneType();
});


class CompanyPhoneTypes {

    /**
    *@summary Obtiene la lista de Tipos de teléfono
    *@returns {Array} Lista de tipos de teléfono
    */
    static GetCompanyPhoneTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/GetCompanyPhoneTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Tipos de teléfono
    *@returns {Array} Lista de tipos de teléfono con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveCompanyPhoneTypes(glbCompanyPhoneTypeAdded, glbCompanyPhoneTypeModified, glbCompanyPhoneTypeDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveCompanyPhoneTypes',
            data: JSON.stringify({ listAdded: glbCompanyPhoneTypeAdded, listModified: glbCompanyPhoneTypeModified, listDeleted: glbCompanyPhoneTypeDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Tipos de teléfono
    */
    static GenerateCompanyPhoneTypeFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateCompanyPhoneTypeFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class CompanyPhoneTypeControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get CompanyPhoneTypeTemplate() { return "#CompanyPhoneTypeTemplate"; }
    static get listCompanyPhoneType() { return $("#listCompanyPhoneType"); }
    static get btnNewCompanyPhoneType() { return $("#btnNewCompanyPhoneType"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnCompanyPhoneTypeAccept() { return $("#btnCompanyPhoneTypeAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputDescription() { return $("#inputDescription"); }
    static get inputSmallDescription() { return $("#inputSmallDescription"); }
    static get inputIsCellphone() { return $("#inputIsCellphone"); }
    static get inputRegExpression() { return $("#inputRegExpression"); }
    static get inputErrorMessage() { return $("#inputErrorMessage"); }
    static get inputSearchCompanyPhoneType() { return $("#inputSearchCompanyPhoneType"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formCompanyPhoneType() { return $("#formCompanyPhoneType"); }
    static get btnSaveCompanyPhoneType() { return $("#btnSaveCompanyPhoneType"); }
    static get sendExcelCompanyPhoneType() { return $("#btnExport"); }
}

class ParametrizationCompanyPhoneType extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        CompanyPhoneTypeControls.listCompanyPhoneType.UifListView({
            displayTemplate: "#CompanyPhoneTypeTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemCompanyPhoneType,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Tipos de teléfono
        * @returns {Array} Lista de tipos de teléfono
        */
        CompanyPhoneTypes.GetCompanyPhoneTypes().done(function (data) {
            if (data.success) {
                glbCompanyPhoneType = data.result;
                ParametrizationCompanyPhoneType.LoadCompanyPhoneTypes();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });



    }


    //---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        CompanyPhoneTypeControls.inputSearchCompanyPhoneType.on('itemSelected', this.showCompanyPhoneTypeSerch);
        CompanyPhoneTypeControls.inputSearchCompanyPhoneType.on("buttonClick", this.showCompanyPhoneTypeSerch);
        CompanyPhoneTypeControls.btnCompanyPhoneTypeAccept.on("click", this.AddItemCompanyPhoneType);
        CompanyPhoneTypeControls.listCompanyPhoneType.on('rowEdit', ParametrizationCompanyPhoneType.ShowData);
        CompanyPhoneTypeControls.listCompanyPhoneType.on('rowDelete', this.DeleteItemCompanyPhoneType);
        CompanyPhoneTypeControls.btnSaveCompanyPhoneType.on("click", this.SaveCompanyPhoneType);
        CompanyPhoneTypeControls.btnNewCompanyPhoneType.on("click", ParametrizationCompanyPhoneType.CleanForm);
        CompanyPhoneTypeControls.btnExit.on("click", this.Exit);
        CompanyPhoneTypeControls.sendExcelCompanyPhoneType.on("click", this.sendExcelCompanyPhoneType);
    }



    static ShowSearchAdv(data) {

        $("#lvSearchAdvCompanyPhoneType").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvCompanyPhoneType").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvCompanyPhoneType.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvCompanyPhoneType.hide();
    }





    /**
    * @summary Carga la lista de Tipos de teléfono
    */
    static LoadCompanyPhoneTypes() {
        CompanyPhoneTypeControls.listCompanyPhoneType.UifListView("clear");
        $.each(glbCompanyPhoneType, function (key, value) {
            if (this.IsCellphone == true) {
                var CompanyPhoneTypes = {
                    PhoneTypeCode: this.PhoneTypeCode,
                    Description: this.Description,
                    SmallDescription: this.SmallDescription,
                    IsCellphone: this.IsCellphone,
                    LabelIsCellphone: "Celular",
                    RegExpression: this.RegExpression,
                    ErrorMessage: this.ErrorMessage,
                    allowDelete: this.AllowDelete,
                    StatusType: 1
                }
            }
            else {
                var CompanyPhoneTypes = {
                    PhoneTypeCode: this.PhoneTypeCode,
                    Description: this.Description,
                    SmallDescription: this.SmallDescription,
                    IsCellphone: this.IsCellphone,
                    RegExpression: this.RegExpression,
                    ErrorMessage: this.ErrorMessage,
                    allowDelete: this.AllowDelete,
                    StatusType: 1
                }
            }
            CompanyPhoneTypeControls.listCompanyPhoneType.UifListView("addItem", CompanyPhoneTypes);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelCompanyPhoneType() {
        CompanyPhoneTypes.GenerateCompanyPhoneTypeFileToExport().done(function (data) {
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
    //----------------------------------------------------------//

    /**
    * @summary Limpia las listas para el CRUD de Tipos de teléfono
    */
    static RefreshList() {
        glbCompanyPhoneTypeAdded = [];
        glbCompanyPhoneTypeModified = [];
        glbCompanyPhoneTypeDelete = [];
    }



    //---------------------BUSCAR TIPO DE TELEFONO-------------------//

    /**
    *@summary Inicia la busqueda de Tipos de teléfono
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showCompanyPhoneTypeSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationCompanyPhoneType.SearchCompanyPhoneType(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de Tipos de teléfono
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchCompanyPhoneType(description) {
        var find = false;
        var data = [];
        var search = glbCompanyPhoneType;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': AppResourcesPerson.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            $.each(search, function (key, value) {
                if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                    includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ||
                    (value.SmallDescription.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                ) {
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });


            if (find == false) {
                $.UifNotify('show',
                    {
                        'type': 'danger', 'message': AppResourcesPerson.CompanyPhoneTypeNotFound, 'autoclose': true
                    })
            } else {
                ParametrizationCompanyPhoneType.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra los tipos de teléfono Encontrados
    *@returns {object} *Obtiene el tipo de teléfono encontrada y la muestra 
    *@returns {Array} *Obtiene la lista de tipos de teléfono que coiniciden con la busqueda
    */
    static ShowData(event, result, index) {
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ParametrizationCompanyPhoneType.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            CompanyPhoneTypeIndex = index;
            CompanyPhoneTypeControls.inputDescription.val(resultUnique.Description);
            CompanyPhoneTypeControls.inputSmallDescription.val(resultUnique.SmallDescription);
            CompanyPhoneTypeControls.inputSearchCompanyPhoneType.val(resultUnique.Description);
            CompanyPhoneTypeControls.inputRegExpression.val(resultUnique.RegExpression);
            CompanyPhoneTypeControls.inputErrorMessage.val(resultUnique.ErrorMessage);
            CompanyPhoneTypeControls.inputId.val(resultUnique.PhoneTypeCode);
            if (resultUnique.IsCellphone == true) {
                CompanyPhoneTypeControls.inputIsCellphone.prop('checked', true);
            }
            else {
                CompanyPhoneTypeControls.inputIsCellphone.prop('checked', false);
            }

        }
    }

    /**
    *@summary Muestra los tipos de teléfono Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de tipos de teléfono encontrados
    */
    static ShowDefaultResult(dataTable) {
        CompanyPhoneTypeControls.tableResults.UifDataTable('clear');
        CompanyPhoneTypeControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR TIPO DE TELEFONO-------------------//

    /**
    *@summary Obtiene el tipo de teléfono seleccionado
    *@param {id} * ID del tipo de teléfono
    */
    static GetCompanyPhoneType(id) {
        var find = false;
        var data = [];
        var search = glbCompanyPhoneType;
        $.each(search, function (key, value) {
            if (value.PhoneTypeCode == id) {
                CompanyPhoneTypeIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationCompanyPhoneType.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': AppResourcesPerson.ProtectionNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un tipo de teléfono al listview y al array de tipos de teléfono por agregar 
    */
    AddItemCompanyPhoneType() {
        CompanyPhoneTypeControls.formCompanyPhoneType.validate();
        if (CompanyPhoneTypeControls.formCompanyPhoneType.valid()) {
            if ($(CompanyPhoneTypeControls.inputIsCellphone).is(":checked")) {
                var companyPhoneType = {
                    PhoneTypeCode: CompanyPhoneTypeControls.inputId.val(),
                    Description: CompanyPhoneTypeControls.inputDescription.val(),
                    SmallDescription: CompanyPhoneTypeControls.inputSmallDescription.val(),
                    IsCellphone: true,
                    RegExpression: CompanyPhoneTypeControls.inputRegExpression.val(),
                    ErrorMessage: CompanyPhoneTypeControls.inputErrorMessage.val(),
                    LabelIsCellphone: "Celular"
                }
            }
            else {
                var companyPhoneType = {
                    PhoneTypeCode: CompanyPhoneTypeControls.inputId.val(),
                    Description: CompanyPhoneTypeControls.inputDescription.val(),
                    SmallDescription: CompanyPhoneTypeControls.inputSmallDescription.val(),
                    IsCellphone: false,
                    RegExpression: CompanyPhoneTypeControls.inputRegExpression.val(),
                    ErrorMessage: CompanyPhoneTypeControls.inputErrorMessage.val()
                }
            }

            if (CompanyPhoneTypeIndex == null) {
                var list = CompanyPhoneTypeControls.listCompanyPhoneType.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.Description.toLowerCase().sistranReplaceAccentMark()
                        == companyPhoneType.Description.toLowerCase().sistranReplaceAccentMark();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': AppResourcesPerson.ErrorExistCompanyPhoneTypeName, 'autoclose': true
                    });
                }
                else {
                    companyPhoneType.Status = 'Added';
                    companyPhoneType.StatusTypeService = ParametrizationStatus.Create;
                    CompanyPhoneTypeControls.listCompanyPhoneType.UifListView("addItem", companyPhoneType);
                }
            }
            else {
                if (companyPhoneType.PhoneTypeCode != 0) {
                    companyPhoneType.StatusTypeService = ParametrizationStatus.Update;
                    companyPhoneType.Status = 'Modified';
                } else {
                    companyPhoneType.StatusTypeService = ParametrizationStatus.Create;
                    companyPhoneType.Status = 'Added';
                }

                CompanyPhoneTypeControls.listCompanyPhoneType.UifListView('editItem', CompanyPhoneTypeIndex, companyPhoneType);
            }
            ParametrizationCompanyPhoneType.CleanForm();
        }
    }

    /**
    *@summary Elimina un tipo de teléfono del listview y lo agrega al array de tipos de teléfono por eliminiar 
    */
	DeleteItemCompanyPhoneType(event, data) {
		event.resolve();
		if (data.PhoneTypeCode != 0) {
			glbCompanyPhoneTypeDelete.push(data);
			data.StatusTypeService = ParametrizationStatus.Delete;
			data.allowEdit = false;
			data.allowDelete = false;
			CompanyPhoneTypeControls.listCompanyPhoneType.UifListView("addItem", data);
        }
        ParametrizationCompanyPhoneType.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para los Tipos de teléfono con los arrays cargados 
    */
    SaveCompanyPhoneType() {
        var companyPhoneTypes = CompanyPhoneTypeControls.listCompanyPhoneType.UifListView('getData');
        $.each(companyPhoneTypes, function (key, value) {
            if (this.Status == "Added") {
                glbCompanyPhoneTypeAdded.push(this);
            }
            else if (this.Status == "Modified") {
                glbCompanyPhoneTypeModified.push(this);
            }
        });
        if (glbCompanyPhoneTypeAdded.length > 0 || glbCompanyPhoneTypeModified.length > 0 || glbCompanyPhoneTypeDelete.length > 0) {
            CompanyPhoneTypes.SaveCompanyPhoneTypes(glbCompanyPhoneTypeAdded, glbCompanyPhoneTypeModified, glbCompanyPhoneTypeDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationCompanyPhoneType.RefreshList();
                        glbCompanyPhoneType = data.result.data;
                        ParametrizationCompanyPhoneType.LoadCompanyPhoneTypes();
                        $.UifNotify('show', {
                            'type': 'info', 'message': data.result.message, 'autoclose': true
                        });
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': data.result, 'autoclose': true
                        });
                    }
                })
                .fail(function () {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorSaveCompanyPhoneTypesDeleted, 'autoclose': true
                    })

                });
        }

    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        CompanyPhoneTypeIndex = null;
        CompanyPhoneTypeControls.inputDescription.val(null);
        CompanyPhoneTypeControls.inputDescription.focus();
        CompanyPhoneTypeControls.inputSmallDescription.val(null);
        CompanyPhoneTypeControls.inputRegExpression.val(null);
        CompanyPhoneTypeControls.inputErrorMessage.val(null);
        CompanyPhoneTypeControls.inputSearchCompanyPhoneType.val(null);
        CompanyPhoneTypeControls.inputId.val(null);
        CompanyPhoneTypeControls.inputIsCellphone.prop('checked', false);
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

