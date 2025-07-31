var glbInsuredSegment = {};
var inputSearch = "";
var modalListType;
var insuredSegmentIndex = null;
var glbInsuredSegmentAdded = [];
var glbInsuredSegmentModified = [];
var glbInsuredSegmentDelete = [];
var dropDownSearchAdvInsuredSegment = null;

$(() => {
    new ParametrizationInsuredSegment();
});


class InsuredSegments {

    /**
    *@summary Obtiene la lista de Perfiles de asegurado
    *@returns {Array} Lista de sucursales
    */
    static GetInsuredSegments() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/InsuredSegment/GetInsuredSegments',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Perfiles de Asegurado
    *@returns {Array} Lista de Perfiles de Asegurado con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveInsuredSegments(glbInsuredSegmentAdded, glbInsuredSegmentModified, glbInsuredSegmentDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveInsuredSegments',
            data: JSON.stringify({ listAdded: glbInsuredSegmentAdded, listModified: glbInsuredSegmentModified, listDeleted: glbInsuredSegmentDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para las Sucursales
    */
    static GenerateFileToExportInsuredSegments() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExportInsuredSegments',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class InsuredSegmentControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get InsuredSegmentTemplate() { return "#InsuredSegmentTemplate"; }
    static get listInsuredSegment() { return $("#listInsuredSegment"); }
    static get btnNewInsuredSegment() { return $("#btnNewInsuredSegment"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnInsuredSegmentAccept() { return $("#btnInsuredSegmentAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputLongDescription() { return $("#inputLongDescription"); }
    static get inputShortDescription() { return $("#inputShortDescription"); }
    static get inputSearchInsuredSegment() { return $("#inputSearchInsuredSegment"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formInsuredSegment() { return $("#formInsuredSegment"); }
    static get btnSaveInsuredSegment() { return $("#btnSaveInsuredSegment"); }
    static get sendExcelInsuredSegment() { return $("#btnExport"); }
    static get inputIdInsuredSegment() { return $("#inputIdInsuredSegment"); }
}

class ParametrizationInsuredSegment extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputIdInsuredSegment").attr("disabled", "true");
        InsuredSegmentControls.listInsuredSegment.UifListView({
            displayTemplate: "#InsuredSegmentTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemInsuredSegment,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Perfiles de Asegurado
        * @returns {Array} Lista de de Perfiles de Asegurado
        */
        InsuredSegments.GetInsuredSegments().done(function (data) {
            if (data.success) {
                glbInsuredSegment = data.result;
                ParametrizationInsuredSegment.LoadInsuredSegments();
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
        InsuredSegmentControls.inputSearchInsuredSegment.on('itemSelected', this.showInsuredSegmentSerch);
        InsuredSegmentControls.inputSearchInsuredSegment.on("buttonClick", this.showInsuredSegmentSerch);
        InsuredSegmentControls.btnInsuredSegmentAccept.on("click", this.AddItemInsuredSegment);
        InsuredSegmentControls.listInsuredSegment.on('rowEdit', ParametrizationInsuredSegment.ShowData);
        InsuredSegmentControls.listInsuredSegment.on('rowDelete', this.DeleteItemInsuredSegment);
        InsuredSegmentControls.btnSaveInsuredSegment.on("click", this.SaveInsuredSegment);
        InsuredSegmentControls.btnNewInsuredSegment.on("click", ParametrizationInsuredSegment.CleanForm);
        InsuredSegmentControls.btnExit.on("click", this.Exit);
        InsuredSegmentControls.sendExcelInsuredSegment.on("click", this.sendExcelInsuredSegment);        
       
    }

   
    
    static ShowSearchAdv(data) {

        $("#lvSearchAdvInsuredSegment").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvInsuredSegment").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvInsuredSegment.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvInsuredSegment.hide();
    }





    /**
    * @summary Carga la lista de Perfiles de Asegurado
    */
    static LoadInsuredSegments() {
        InsuredSegmentControls.listInsuredSegment.UifListView("clear");
        $.each(glbInsuredSegment, function (key, value) {
            var lInsuredSegments = {
                Id: this.Id,
                LongDescription: this.Description,
				ShortDescription: this.SmallDescription,
				StatusType: 1
            }
            InsuredSegmentControls.listInsuredSegment.UifListView("addItem", lInsuredSegments);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelInsuredSegment() {
        InsuredSegments.GenerateFileToExportInsuredSegments().done(function (data) {
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
    * @summary Limpia las listas para el CRUD de Perfiles de Asegurado
    */
    static RefreshList() {
        glbInsuredSegmentAdded = [];
        glbInsuredSegmentModified = [];
        glbInsuredSegmentDelete = [];
    }



    //---------------------BUSCAR Perfiles de Asegurado-------------------//

    /**
    *@summary Inicia la busqueda de Sucursales
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showInsuredSegmentSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationInsuredSegment.SearchInsuredSegment(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de Sucursales
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchInsuredSegment(description) {
        var find = false;
        var data = [];
        var search = glbInsuredSegment;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            $.each(search, function (key, value) {
                if ((value.LongDescription.toLowerCase().sistranReplaceAccentMark().
                    includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ||
                    (value.ShortDescription.toLowerCase().sistranReplaceAccentMark().
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
                            'type': 'danger', 'message': Resources.Language.InsuredSegmentNotFound, 'autoclose': true
                        })
            } else {
                ParametrizationInsuredSegment.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra los Perfiles de Asegurado Encontrados
    *@returns {object} *Obtiene el Perfiles de Asegurado y la muestra
    *@returns {Array} *Obtiene la lista de Perfiles de Asegurado que coiniciden con la busqueda
    */
    static ShowData(event, result, index) {
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ParametrizationInsuredSegment.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            insuredSegmentIndex = index;
            InsuredSegmentControls.inputLongDescription.val(resultUnique.LongDescription);
            InsuredSegmentControls.inputShortDescription.val(resultUnique.ShortDescription);
            InsuredSegmentControls.inputSearchInsuredSegment.val(resultUnique.LongDescription);
            InsuredSegmentControls.inputIdInsuredSegment.val(resultUnique.Id);
            InsuredSegmentControls.inputId.val(resultUnique.Id);
        }
    }

    /**
    *@summary Muestra los Perfiles de Asegurado Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de Perfiles de Asegurado encontrados
    */
    static ShowDefaultResult(dataTable) {
        InsuredSegmentControls.tableResults.UifDataTable('clear');
        InsuredSegmentControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR Perfil de Asegurado-------------------//

    /**
    *@summary Obtiene el Perfil de Asegurado seleccionado
    *@param {id} * ID del Perfile de Asegurado
    */
    static GetInsuredSegment(id) {
        var find = false;
        var data = [];
        var search = glbInsuredSegment;
        $.each(search, function (key, value) {
            if (value.Id == id) {
                insuredSegmentIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationInsuredSegment.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.InsuredSegmentNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un Perfil de Asegurado al listview y al array de Perfiles de Asegurado por agregar 
    */
    AddItemInsuredSegment() {
        InsuredSegmentControls.formInsuredSegment.validate();
        if (InsuredSegmentControls.formInsuredSegment.valid()) {
            var lInsuredSegment = {
                Id: InsuredSegmentControls.inputId.val(),
                LongDescription: InsuredSegmentControls.inputLongDescription.val(),
                ShortDescription: InsuredSegmentControls.inputShortDescription.val(),
            }
            if (insuredSegmentIndex == null) {
                var list = InsuredSegmentControls.listInsuredSegment.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.LongDescription.toLowerCase().sistranReplaceAccentMark()
                        == lInsuredSegment.LongDescription.toLowerCase().sistranReplaceAccentMark();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorExistInsuredSegmentName, 'autoclose': true
                    });
                }
                else {
					lInsuredSegment.Status = 'Added';
                    lInsuredSegment.StatusTypeService = ParametrizationStatus.Create;
                    InsuredSegmentControls.listInsuredSegment.UifListView("addItem", lInsuredSegment);
                }
            }
            else {
				if (lInsuredSegment.Id != 0) {
                    lInsuredSegment.StatusTypeService = ParametrizationStatus.Update;
				} else {
                    lInsuredSegment.StatusTypeService = ParametrizationStatus.Create;
                }				
                InsuredSegmentControls.listInsuredSegment.UifListView('editItem', insuredSegmentIndex, lInsuredSegment);
			}
            ParametrizationInsuredSegment.CleanForm();
        }
    }

    /**
    *@summary Elimina un Perfil de Asegurado del listview y lo agrega al array de Perfiles de Asegurado por eliminiar
    */
	DeleteItemInsuredSegment(event, data) {
		event.resolve();
        if (data.Id != 0) {
			glbInsuredSegmentDelete.push(data);
			data.StatusTypeService = ParametrizationStatus.Delete;
			data.allowEdit = false;
			data.allowDelete = false;
			InsuredSegmentControls.listInsuredSegment.UifListView("addItem", data);
        }
        ParametrizationInsuredSegment.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para los Perfiles de Asegurados con los arrays cargados 
    */
    SaveInsuredSegment() {
        var lInsuredSegments = InsuredSegmentControls.listInsuredSegment.UifListView('getData');
        $.each(lInsuredSegments, function (key, value) {
            if (this.StatusTypeService == ParametrizationStatus.Create) {
                glbInsuredSegmentAdded.push(this);
            }
            else if (this.StatusTypeService == ParametrizationStatus.Update) {
                glbInsuredSegmentModified.push(this);
            }
        });
        if (glbInsuredSegmentAdded.length > 0 || glbInsuredSegmentModified.length > 0 || glbInsuredSegmentDelete.length > 0) {
            InsuredSegments.SaveInsuredSegments(glbInsuredSegmentAdded, glbInsuredSegmentModified, glbInsuredSegmentDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationInsuredSegment.RefreshList();
                        glbInsuredSegment = data.result.data;
                        ParametrizationInsuredSegment.LoadInsuredSegments();
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
                        'type': 'danger', 'message': Resources.Language.ErrorSaveInsuredSegment, 'autoclose': true
                    })

                });
        }

    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        insuredSegmentIndex = null;
        InsuredSegmentControls.inputLongDescription.val(null);
        InsuredSegmentControls.inputLongDescription.focus();
        InsuredSegmentControls.inputShortDescription.val(null);
        InsuredSegmentControls.inputSearchInsuredSegment.val(null);
        InsuredSegmentControls.inputIdInsuredSegment.val(0);
        InsuredSegmentControls.inputId.val(null);
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

