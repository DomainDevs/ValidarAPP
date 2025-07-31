var glbInsuredProfile = {};
var inputSearch = "";
var modalListType;
var insuredProfileIndex = null;
var glbInsuredProfileAdded = [];
var glbInsuredProfileModified = [];
var glbInsuredProfileDelete = [];
var dropDownSearchAdvInsuredProfile = null;

$(() => {
    new ParametrizationInsuredProfile();
});


class InsuredProfiles {

    /**
    *@summary Obtiene la lista de Sucursales
    *@returns {Array} Lista de sucursales
    */
    static GetInsuredProfiles() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/InsuredProfile/GetInsuredProfiles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los Perfiles de Asegurado
    *@returns {Array} Lista de Perfiles de Asegurado con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveInsuredProfiles(glbInsuredProfileAdded, glbInsuredProfileModified, glbInsuredProfileDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveInsuredProfiles',
            data: JSON.stringify({ listAdded: glbInsuredProfileAdded, listModified: glbInsuredProfileModified, listDeleted: glbInsuredProfileDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para las Sucursales
    */
    static GenerateFileToExportInsuredProfiles() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExportInsuredProfiles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class InsuredProfileControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get InsuredProfileTemplate() { return "#InsuredProfileTemplate"; }
    static get listInsuredProfile() { return $("#listInsuredProfile"); }
    static get btnNewInsuredProfile() { return $("#btnNewInsuredProfile"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnInsuredProfileAccept() { return $("#btnInsuredProfileAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputLongDescription() { return $("#inputLongDescription"); }
    static get inputShortDescription() { return $("#inputShortDescription"); }
    static get inputSearchInsuredProfile() { return $("#inputSearchInsuredProfile"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formInsuredProfile() { return $("#formInsuredProfile"); }
    static get btnSaveInsuredProfile() { return $("#btnSaveInsuredProfile"); }
    static get sendExcelInsuredProfile() { return $("#btnExport"); }    
    static get inputIdInsuredProfile() { return $("#inputIdInsuredProfile"); }
}

class ParametrizationInsuredProfile extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputIdInsuredProfile").attr("disabled", "true");
        InsuredProfileControls.listInsuredProfile.UifListView({
            displayTemplate: "#InsuredProfileTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemInsuredProfile,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Perfiles de Asegurado
        * @returns {Array} Lista de de Perfiles de Asegurado
        */
        InsuredProfiles.GetInsuredProfiles().done(function (data) {
            if (data.success) {
                glbInsuredProfile = data.result;
                ParametrizationInsuredProfile.LoadInsuredProfiles();
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
        InsuredProfileControls.inputSearchInsuredProfile.on('itemSelected', this.showInsuredProfileSerch);
        InsuredProfileControls.inputSearchInsuredProfile.on("buttonClick", this.showInsuredProfileSerch);
        InsuredProfileControls.btnInsuredProfileAccept.on("click", this.AddItemInsuredProfile);
        InsuredProfileControls.listInsuredProfile.on('rowEdit', ParametrizationInsuredProfile.ShowData);
        InsuredProfileControls.listInsuredProfile.on('rowDelete', this.DeleteItemInsuredProfile);
        InsuredProfileControls.btnSaveInsuredProfile.on("click", this.SaveInsuredProfile);
        InsuredProfileControls.btnNewInsuredProfile.on("click", ParametrizationInsuredProfile.CleanForm);
        InsuredProfileControls.btnExit.on("click", this.Exit);
        InsuredProfileControls.sendExcelInsuredProfile.on("click", this.sendExcelInsuredProfile);        
       
    }

   
    
    static ShowSearchAdv(data) {

        $("#lvSearchAdvInsuredProfile").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvInsuredProfile").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvInsuredProfile.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvInsuredProfile.hide();
    }





    /**
    * @summary Carga la lista de Perfiles de Asegurado
    */
    static LoadInsuredProfiles() {
        InsuredProfileControls.listInsuredProfile.UifListView("clear");
        $.each(glbInsuredProfile, function (key, value) {
            var lInsuredProfiles = {
                Id: this.Id,
                Description: this.Description,
                SmallDescription: this.SmallDescription,
                StatusType: 1
            }
            InsuredProfileControls.listInsuredProfile.UifListView("addItem", lInsuredProfiles);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelInsuredProfile() {
        InsuredProfiles.GenerateFileToExportInsuredProfiles().done(function (data) {
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
        glbInsuredProfileAdded = [];
        glbInsuredProfileModified = [];
        glbInsuredProfileDelete = [];
    }



    //---------------------BUSCAR Perfiles de Asegurado-------------------//

    /**
    *@summary Inicia la busqueda de Sucursales
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showInsuredProfileSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationInsuredProfile.SearchInsuredProfile(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de Sucursales
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchInsuredProfile(description) {
        var find = false;
        var data = [];
        var search = glbInsuredProfile;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
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
                            'type': 'danger', 'message': Resources.Language.InsuredProfileNotFound, 'autoclose': true
                        })
            } else {
                ParametrizationInsuredProfile.ShowData(null, data, data.key);
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
            ParametrizationInsuredProfile.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            insuredProfileIndex = index;
            InsuredProfileControls.inputLongDescription.val(resultUnique.Description);
            InsuredProfileControls.inputShortDescription.val(resultUnique.SmallDescription);
            InsuredProfileControls.inputSearchInsuredProfile.val(resultUnique.Description);
            InsuredProfileControls.inputIdInsuredProfile.val(resultUnique.Id);
            InsuredProfileControls.inputId.val(resultUnique.Id);
        }
    }

    /**
    *@summary Muestra los Perfiles de Asegurado Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de Perfiles de Asegurado encontrados
    */
    static ShowDefaultResult(dataTable) {
        InsuredProfileControls.tableResults.UifDataTable('clear');
        InsuredProfileControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR Perfil de Asegurado-------------------//

    /**
    *@summary Obtiene el Perfil de Asegurado seleccionado
    *@param {id} * ID del Perfile de Asegurado
    */
    static GetInsuredProfile(id) {
        var find = false;
        var data = [];
        var search = glbInsuredProfile;
        $.each(search, function (key, value) {
            if (value.Id == id) {
                insuredProfileIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationInsuredProfile.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.InsuredProfileNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un Perfil de Asegurado al listview y al array de Perfiles de Asegurado por agregar 
    */
    AddItemInsuredProfile() {
        InsuredProfileControls.formInsuredProfile.validate();
        if (InsuredProfileControls.formInsuredProfile.valid()) {
            var lInsuredProfile = {
                Id: InsuredProfileControls.inputId.val(),
                Description: InsuredProfileControls.inputLongDescription.val(),
                SmallDescription: InsuredProfileControls.inputShortDescription.val(),
            }
            if (insuredProfileIndex == null) {
                var list = InsuredProfileControls.listInsuredProfile.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.Description.toLowerCase().sistranReplaceAccentMark()
                        == lInsuredProfile.Description.toLowerCase().sistranReplaceAccentMark();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.ErrorExistInsuredProfileName, 'autoclose': true
                    });
                }
                else {
                    lInsuredProfile.Status = 'Added';
                    lInsuredProfile.StatusTypeService = ParametrizationStatus.Create;
                    InsuredProfileControls.listInsuredProfile.UifListView("addItem", lInsuredProfile);
                }
            }
            else {
                if (lInsuredProfile.Id != 0) {
                    lInsuredProfile.StatusTypeService = ParametrizationStatus.Update;
                } else {
                    lInsuredProfile.StatusTypeService = ParametrizationStatus.Create;
                }
                InsuredProfileControls.listInsuredProfile.UifListView('editItem', insuredProfileIndex, lInsuredProfile);
            }
            ParametrizationInsuredProfile.CleanForm();
        }
    }

    /**
    *@summary Elimina un Perfil de Asegurado del listview y lo agrega al array de Perfiles de Asegurado por eliminiar
    */
	DeleteItemInsuredProfile(event, data) {
		event.resolve();
        if (data.Id != 0) {
			glbInsuredProfileDelete.push(data);
			data.StatusTypeService = ParametrizationStatus.Delete;
			data.allowEdit = false;
			data.allowDelete = false;
			InsuredProfileControls.listInsuredProfile.UifListView("addItem", data);
        }
        ParametrizationInsuredProfile.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para los Perfiles de Asegurados con los arrays cargados 
    */
    SaveInsuredProfile() {
        var lInsuredProfiles = InsuredProfileControls.listInsuredProfile.UifListView('getData');
        $.each(lInsuredProfiles, function (key, value) {
            if (this.StatusTypeService == ParametrizationStatus.Create) {
                glbInsuredProfileAdded.push(this);
            }
            else if (this.StatusTypeService == ParametrizationStatus.Update) {
                glbInsuredProfileModified.push(this);
            }
        });
        if (glbInsuredProfileAdded.length > 0 || glbInsuredProfileModified.length > 0 || glbInsuredProfileDelete.length > 0) {
            InsuredProfiles.SaveInsuredProfiles(glbInsuredProfileAdded, glbInsuredProfileModified, glbInsuredProfileDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationInsuredProfile.RefreshList();
                        glbInsuredProfile = data.result.data;
                        ParametrizationInsuredProfile.LoadInsuredProfiles();
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
                        'type': 'danger', 'message': Resources.Language.ErrorSaveInsuredProfile, 'autoclose': true
                    })

                });
        }

    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        insuredProfileIndex = null;
        InsuredProfileControls.inputLongDescription.val(null);
        InsuredProfileControls.inputLongDescription.focus();
        InsuredProfileControls.inputShortDescription.val(null);
        InsuredProfileControls.inputSearchInsuredProfile.val(null);
        InsuredProfileControls.inputIdInsuredProfile.val(0);
        InsuredProfileControls.inputId.val(null);
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

