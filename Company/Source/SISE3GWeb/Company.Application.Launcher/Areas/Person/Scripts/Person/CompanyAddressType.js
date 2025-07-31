var glbCompanyAddressType = {};
var inputSearch = "";
var modalListType;
var CompanyAddressTypeIndex = null;
var glbCompanyAddressTypeAdded = [];
var glbCompanyAddressTypeModified = [];
var glbCompanyAddressTypeDelete = [];
var dropDownSearchAdvCompanyAddressType = null;


$(() => {
    new ParametrizationCompanyAddressType();
});


class CompanyAddressTypes {

    /**
    *@summary Obtiene la lista de tipos de dirección
    *@returns {Array} Lista de tipos de dirección
    */
    static GetCompanyAddressTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/GetCompanyAddressTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Tipos de dirección
    *@returns {Array} Lista de tipos de dirección con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveCompanyAddressTypes(glbCompanyAddressTypeAdded, glbCompanyAddressTypeModified, glbCompanyAddressTypeDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveCompanyAddressTypes',
            data: JSON.stringify({ listAdded: glbCompanyAddressTypeAdded, listModified: glbCompanyAddressTypeModified, listDeleted: glbCompanyAddressTypeDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Tipos de dirección
    */
    static GenerateCompanyAddressTypeFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateCompanyAddressTypeFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class CompanyAddressTypeControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get CompanyAddressTypeTemplate() { return "#CompanyAddressTypeTemplate"; }
    static get listCompanyAddressType() { return $("#listCompanyAddressType"); }
    static get btnNewCompanyAddressType() { return $("#btnNewCompanyAddressType"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnCompanyAddressTypeAccept() { return $("#btnCompanyAddressTypeAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputSmallDescription() { return $("#inputSmallDescription"); }
    static get inputTinyDescription() { return $("#inputTinyDescription"); }
    static get inputIsElectronicMail() { return $("#inputIsElectronicMail"); }
    static get inputSearchCompanyAddressType() { return $("#inputSearchCompanyAddressType"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formCompanyAddressType() { return $("#formCompanyAddressType"); }
    static get btnSaveCompanyAddressType() { return $("#btnSaveCompanyAddressType"); }
    static get sendExcelCompanyAddressType() { return $("#btnExport"); }
}

class ParametrizationCompanyAddressType extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        CompanyAddressTypeControls.listCompanyAddressType.UifListView({
            displayTemplate: "#CompanyAddressTypeTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemCompanyAddressType,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Tipos de dirección
        * @returns {Array} Lista de tipos de dirección
        */
        CompanyAddressTypes.GetCompanyAddressTypes().done(function (data) {
            if (data.success) {
                glbCompanyAddressType = data.result;
                ParametrizationCompanyAddressType.LoadCompanyAddressTypes();
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
        CompanyAddressTypeControls.inputSearchCompanyAddressType.on('itemSelected', this.showCompanyAddressTypeSerch);
        CompanyAddressTypeControls.inputSearchCompanyAddressType.on("buttonClick", this.showCompanyAddressTypeSerch);
        CompanyAddressTypeControls.btnCompanyAddressTypeAccept.on("click", this.AddItemCompanyAddressType);
        CompanyAddressTypeControls.listCompanyAddressType.on('rowEdit', ParametrizationCompanyAddressType.ShowData);
        CompanyAddressTypeControls.listCompanyAddressType.on('rowDelete', this.DeleteItemCompanyAddressType);
        CompanyAddressTypeControls.btnSaveCompanyAddressType.on("click", this.SaveCompanyAddressType);
        CompanyAddressTypeControls.btnNewCompanyAddressType.on("click", ParametrizationCompanyAddressType.CleanForm);
        CompanyAddressTypeControls.btnExit.on("click", this.Exit);
        CompanyAddressTypeControls.sendExcelCompanyAddressType.on("click", this.sendExcelCompanyAddressType);
    }



    static ShowSearchAdv(data) {

        $("#lvSearchAdvCompanyAddressType").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvCompanyAddressType").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvCompanyAddressType.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvCompanyAddressType.hide();
    }





    /**
    * @summary Carga la lista de Tipos de dirección
    */
    static LoadCompanyAddressTypes() {
        CompanyAddressTypeControls.listCompanyAddressType.UifListView("clear");
        $.each(glbCompanyAddressType, function (key, value) {
            if (this.IsElectronicMail == true) {
                var CompanyAddressTypes = {
                    AddressTypeCode: this.AddressTypeCode,
                    SmallDescription: this.SmallDescription,
                    TinyDescription: this.TinyDescription,
                    IsElectronicMail: this.IsElectronicMail,
                    LabelIsMail: "Correo Electrónico",
                    allowDelete: this.AllowDelete,
                    StatusType: 1
                }
            }
            else {
                var CompanyAddressTypes = {
                    AddressTypeCode: this.AddressTypeCode,
                    SmallDescription: this.SmallDescription,
                    TinyDescription: this.TinyDescription,
                    IsElectronicMail: this.IsElectronicMail,
                    LabelIsMail: "",
                    allowDelete: this.AllowDelete,
                    StatusType: 1
                }
            }
            CompanyAddressTypeControls.listCompanyAddressType.UifListView("addItem", CompanyAddressTypes);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelCompanyAddressType() {
        CompanyAddressTypes.GenerateCompanyAddressTypeFileToExport().done(function (data) {
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
    * @summary Limpia las listas para el CRUD de Tipos de dirección
    */
    static RefreshList() {
        glbCompanyAddressTypeAdded = [];
        glbCompanyAddressTypeModified = [];
        glbCompanyAddressTypeDelete = [];
    }



    //---------------------BUSCAR TIPO DE DIRECCION-------------------//

    /**
    *@summary Inicia la busqueda de Tipos de dirección
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showCompanyAddressTypeSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationCompanyAddressType.SearchCompanyAddressType(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de Tipos de dirección
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchCompanyAddressType(description) {
        var find = false;
        var data = [];
        var search = glbCompanyAddressType;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': AppResourcesPerson.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            $.each(search, function (key, value) {
                if ((value.SmallDescription.toLowerCase().sistranReplaceAccentMark().
                    includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ||
                    (value.TinyDescription.toLowerCase().sistranReplaceAccentMark().
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
                        'type': 'danger', 'message': AppResourcesPerson.CompanyAddressTypeNotFound, 'autoclose': true
                    })
            } else {
                ParametrizationCompanyAddressType.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra las tipos de dirección Encontrados
    *@returns {object} *Obtiene el tipo de dirección encontrado y lo muestra 
    *@returns {Array} *Obtiene la lista de tipos de dirección que coiniciden con la busqueda
    */
    static ShowData(event, result, index) {
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ParametrizationCompanyAddressType.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            CompanyAddressTypeIndex = index;
            CompanyAddressTypeControls.inputSmallDescription.val(resultUnique.SmallDescription);
            CompanyAddressTypeControls.inputTinyDescription.val(resultUnique.TinyDescription);
            CompanyAddressTypeControls.inputSearchCompanyAddressType.val(resultUnique.SmallDescription);
            CompanyAddressTypeControls.inputId.val(resultUnique.AddressTypeCode);
            if (resultUnique.IsElectronicMail == true) {
                CompanyAddressTypeControls.inputIsElectronicMail.prop('checked', true);
            }
            else {
                CompanyAddressTypeControls.inputIsElectronicMail.prop('checked', false);
            }

        }
    }

    /**
    *@summary Muestra los tipos de dirección Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de tipos de dirección encontrados
    */
    static ShowDefaultResult(dataTable) {
        CompanyAddressTypeControls.tableResults.UifDataTable('clear');
        CompanyAddressTypeControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR TIPO DE DIRECCION-------------------//

    /**
    *@summary Obtiene el tipo de dirección seleccionado
    *@param {id} * ID del tipo de dirección
    */
    static GetCompanyAddressType(id) {
        var find = false;
        var data = [];
        var search = glbCompanyAddressType;
        $.each(search, function (key, value) {
            if (value.AddressTypeCode == id) {
                CompanyAddressTypeIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationCompanyAddressType.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': AppResourcesPerson.ProtectionNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un tipo de dirección al listview y al array de tipos de dirección por agregar 
    */
    AddItemCompanyAddressType() {
        CompanyAddressTypeControls.formCompanyAddressType.validate();
        if (CompanyAddressTypeControls.formCompanyAddressType.valid()) {
            if ($(CompanyAddressTypeControls.inputIsElectronicMail).is(":checked")) {
                var companyAddressType = {
                    AddressTypeCode: CompanyAddressTypeControls.inputId.val(),
                    SmallDescription: CompanyAddressTypeControls.inputSmallDescription.val(),
                    TinyDescription: CompanyAddressTypeControls.inputTinyDescription.val(),
                    IsElectronicMail: true,
                    LabelIsMail: "Correo Electrónico",
                    StatusType: 1
                }
            }
            else {
                var companyAddressType = {
                    AddressTypeCode: CompanyAddressTypeControls.inputId.val(),
                    SmallDescription: CompanyAddressTypeControls.inputSmallDescription.val(),
                    TinyDescription: CompanyAddressTypeControls.inputTinyDescription.val(),
                    IsElectronicMail: false,
                    StatusType: 1
                }
            }

            if (CompanyAddressTypeIndex == null) {
                var list = CompanyAddressTypeControls.listCompanyAddressType.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.SmallDescription.toLowerCase().sistranReplaceAccentMark()
                        == companyAddressType.SmallDescription.toLowerCase().sistranReplaceAccentMark();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': AppResourcesPerson.ErrorExistCompanyAddressTypeName, 'autoclose': true
                    });
                }
				else {
					companyAddressType.StatusType = 2;
                    companyAddressType.Status = 'Added';
                    CompanyAddressTypeControls.listCompanyAddressType.UifListView("addItem", companyAddressType);
                }
            }
            else {
				if (companyAddressType.AddressTypeCode != 0) {
					companyAddressType.StatusType = 3;
                    companyAddressType.Status = 'Modified';
				} else {
					companyAddressType.StatusType = 2;
                    companyAddressType.Status = 'Added';
                }
                CompanyAddressTypeControls.listCompanyAddressType.UifListView('editItem', CompanyAddressTypeIndex, companyAddressType);
			}
			listViewColors("listCompanyAddressType");
            ParametrizationCompanyAddressType.CleanForm();
        }
    }

    /**
    *@summary Elimina un tipo de dirección del listview y lo agrega al array de tipos de dirección por eliminiar 
    */
	DeleteItemCompanyAddressType(event, data) {
		event.resolve();
		if (data.AddressTypeCode != 0) {
			glbCompanyAddressTypeDelete.push(data);
			data.StatusTypeService = ParametrizationStatus.Delete;
			data.allowEdit = false;
			data.allowDelete = false;
			CompanyAddressTypeControls.listCompanyAddressType.UifListView("addItem", data);
        }
        ParametrizationCompanyAddressType.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para los Tipos de dirección con los arrays cargados 
    */
    SaveCompanyAddressType() {
        var companyAddressTypes = CompanyAddressTypeControls.listCompanyAddressType.UifListView('getData');
        $.each(companyAddressTypes, function (key, value) {
            if (this.Status == "Added") {
                glbCompanyAddressTypeAdded.push(this);
            }
            else if (this.Status == "Modified") {
                glbCompanyAddressTypeModified.push(this);
            }
        });
        if (glbCompanyAddressTypeAdded.length > 0 || glbCompanyAddressTypeModified.length > 0 || glbCompanyAddressTypeDelete.length > 0) {
            CompanyAddressTypes.SaveCompanyAddressTypes(glbCompanyAddressTypeAdded, glbCompanyAddressTypeModified, glbCompanyAddressTypeDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationCompanyAddressType.RefreshList();
                        glbCompanyAddressType = data.result.data;
                        ParametrizationCompanyAddressType.LoadCompanyAddressTypes();
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
                        'type': 'danger', 'message': AppResourcesPerson.ErrorSaveCompanyAddressTypesDeleted, 'autoclose': true
                    })

                });
        }

    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        CompanyAddressTypeIndex = null;
        CompanyAddressTypeControls.inputSmallDescription.val(null);
        CompanyAddressTypeControls.inputSmallDescription.focus();
        CompanyAddressTypeControls.inputTinyDescription.val(null);
        CompanyAddressTypeControls.inputSearchCompanyAddressType.val(null);
        CompanyAddressTypeControls.inputId.val(null);
        CompanyAddressTypeControls.inputIsElectronicMail.prop('checked', false);
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

