var glbAlliancePrintFormat = {};
var inputSearch = "";
var modalListType;
var alliancePrintFormatIndex = null;
var glbAlliancePrintFormatAdded = [];
var glbAlliancePrintFormatModified = [];
var glbAlliancePrintFormatDelete = [];
var dropDownSearchAdvAlliancePrintFormat = null;
var glbPrefixList = [];
var glbEndorsementTypeList = [];

$(() => {
    new ParametrizationAlliancePrintFormat();
});


class AlliancePrintFormats {

    /**
    *@summary Obtiene la lista de los formatos de impresión de aliados.
    *@returns {Array} Lista de los formatos de impresión de aliados.
    */
    static GetAlliancePrintFormats() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AlliancePrintFormat/GetAlliancePrintFormats',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los formatos de impresión de aliados.
    *@returns {Array} Lista de los formatos de impresión de aliados con los cambios.
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveAlliancePrintFormats(glbAlliancePrintFormatAdded, glbAlliancePrintFormatModified, glbAlliancePrintFormatDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveAlliancePrintFormats',
            data: JSON.stringify({ listAdded: glbAlliancePrintFormatAdded, listModified: glbAlliancePrintFormatModified, listDeleted: glbAlliancePrintFormatDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los formatos de impresión de aliados.
    */
    static GenerateFileToExportAlliancePrintFormats() {
        return $.ajax({
            type: 'POST',
			url: 'GenerateFileToExportAlliancePrintFormats',
			data: JSON.stringify({ listEndorsementType: glbEndorsementTypeList, listPrefix: glbPrefixList }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class PrefixsRequest {
	static GetPrefixs() {
		return $.ajax({
			type: "POST",
			url: rootPath + "Parametrization/AlliancePrintFormat/GetPrefixs",			
			dataType: "json",
			contentType: "application/json; charset=utf-8"
		});
	}
}

class EndorsementTypeRequest {
	static GetEndorsementTypes() {
		return $.ajax({
			type: "POST",
			url: rootPath + "Parametrization/AlliancePrintFormat/GetEndorsementTypes",
			dataType: "json",
			contentType: "application/json; charset=utf-8"
		});
	}
}


class AlliancePrintFormatControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get AlliancePrintFormatTemplate() { return "#AlliancePrintFormatTemplate"; }
    static get listAlliancePrintFormat() { return $("#listAlliancePrintFormat"); }
    static get btnNewAlliancePrintFormat() { return $("#btnNewAlliancePrintFormat"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnAlliancePrintFormatAccept() { return $("#btnAlliancePrintFormatAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputPrefixCd() { return $("#inputPrefixCd"); }
    static get inputEndoTypeCd() { return $("#inputEndoTypeCd"); }
    static get inputFormat() { return $("#inputFormat"); }
    static get inputSearchAlliancePrintFormat() { return $("#inputSearchAlliancePrintFormat"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formAlliancePrintFormat() { return $("#formAlliancePrintFormat"); }
    static get btnSaveAlliancePrintFormat() { return $("#btnSaveAlliancePrintFormat"); }
    static get sendExcelAlliancePrintFormat() { return $("#btnExport"); }    
    static get inputIdAlliancePrintFormat() { return $("#inputIdAlliancePrintFormat"); }
	static get chkIsEnable() { return $("#chkIsEnable"); }
	static get selectPrefixs() { return $("#selectPrefixs"); }
	static get selectEndorsementTypes() { return $("#selectEndorsementTypes"); }
}

class ParametrizationAlliancePrintFormat extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);        
        AlliancePrintFormatControls.listAlliancePrintFormat.UifListView({
            displayTemplate: "#AlliancePrintFormatTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemAlliancePrintFormat,
            customAdd: true,
            customEdit: true,
            height: 300
        });

		PrefixsRequest.GetPrefixs().done(function (data) {
			if (data.success) {
				glbPrefixList = data.result;
				$("#selectPrefixs").UifSelect({ sourceData: data.result });
			}
		});

		EndorsementTypeRequest.GetEndorsementTypes().done(function (data) {
			if (data.success) {
				glbEndorsementTypeList = data.result;
				$("#selectEndorsementTypes").UifSelect({ sourceData: data.result });
			}
		});

        /**
        * @summary Inicializa la lista de los formatos de impresión de aliados.
        * @returns {Array} Lista de los formatos de impresión de aliados.
        */
        AlliancePrintFormats.GetAlliancePrintFormats().done(function (data) {
            if (data.success) {
                glbAlliancePrintFormat = data.result;
                ParametrizationAlliancePrintFormat.LoadAlliancePrintFormats();
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
        AlliancePrintFormatControls.inputSearchAlliancePrintFormat.on('itemSelected', this.showAlliancePrintFormatSerch);
        AlliancePrintFormatControls.inputSearchAlliancePrintFormat.on("buttonClick", this.showAlliancePrintFormatSerch);
        AlliancePrintFormatControls.btnAlliancePrintFormatAccept.on("click", this.AddItemAlliancePrintFormat);
        AlliancePrintFormatControls.listAlliancePrintFormat.on('rowEdit', ParametrizationAlliancePrintFormat.ShowData);
        AlliancePrintFormatControls.listAlliancePrintFormat.on('rowDelete', this.DeleteItemAlliancePrintFormat);
        AlliancePrintFormatControls.btnSaveAlliancePrintFormat.on("click", this.SaveAlliancePrintFormat);
        AlliancePrintFormatControls.btnNewAlliancePrintFormat.on("click", ParametrizationAlliancePrintFormat.CleanForm);
        AlliancePrintFormatControls.btnExit.on("click", this.Exit);
        AlliancePrintFormatControls.sendExcelAlliancePrintFormat.on("click", this.sendExcelAlliancePrintFormat);        
       
    }

   
    
    static ShowSearchAdv(data) {        
        $("#lvSearchAdvAlliancePrintFormat").UifListView("clear");        
        if (data) {
			data.forEach(item => {
				item.PrefixDescription = $("#selectPrefixs option[value='" + item.PrefixCd + "']").text();
				item.EndorsementDescription = $("#selectEndorsementTypes option[value='" + item.EndoTypeCd + "']").text();
				//PrefixDescription: 
				//EndorsementDescription: 
                $("#lvSearchAdvAlliancePrintFormat").UifListView("addItem", item);                
            });
        }
        dropDownSearchAdvAlliancePrintFormat.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvAlliancePrintFormat.hide();
    }





    /**
    * @summary Carga la lista de los formatos de impresión de aliados.
    */
    static LoadAlliancePrintFormats() {
        AlliancePrintFormatControls.listAlliancePrintFormat.UifListView("clear");
        $.each(glbAlliancePrintFormat, function (key, value) {
            var lAlliancePrintFormats = {
                Id: this.Id,
                PrefixCd: this.PrefixCd,
                EndoTypeCd: this.EndoTypeCd,
                Format: this.Format,
				Enable: this.Enable,
				PrefixDescription: $("#selectPrefixs option[value='" + this.PrefixCd + "']").text(),
				EndorsementDescription: $("#selectEndorsementTypes option[value='" + this.EndoTypeCd + "']").text()
            }
            AlliancePrintFormatControls.listAlliancePrintFormat.UifListView("addItem", lAlliancePrintFormats);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelAlliancePrintFormat() {
        AlliancePrintFormats.GenerateFileToExportAlliancePrintFormats().done(function (data) {
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
    * @summary Limpia las listas para el CRUD de los formatos de impresión de aliados.
    */
    static RefreshList() {
        glbAlliancePrintFormatAdded = [];
        glbAlliancePrintFormatModified = [];
        glbAlliancePrintFormatDelete = [];
    }



    //---------------------BUSCAR Perfiles de Asegurado-------------------//

    /**
    *@summary Inicia la busqueda de los formatos de impresión de aliados.
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showAlliancePrintFormatSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationAlliancePrintFormat.SearchAlliancePrintFormat(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de los formatos de impresión de aliados.
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchAlliancePrintFormat(description) {
        var find = false;
        var data = [];
        var search = glbAlliancePrintFormat;
        if (description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            $.each(search, function (key, value) {
                if ((value.Format.toLowerCase().sistranReplaceAccentMark().
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
                            'type': 'danger', 'message': Resources.Language.AlliancePrintFormatNotFound, 'autoclose': true
                        })
            } else {
                ParametrizationAlliancePrintFormat.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra los formatos de impresión de aliados Encontrados.
    *@returns {object} *Obtiene los formatos de impresión de aliados y la muestra.
    *@returns {Array} *Obtiene los formatos de impresión de aliados que coiniciden con la busqueda.
    */
    static ShowData(event, result, index) {
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {            
            ParametrizationAlliancePrintFormat.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            alliancePrintFormatIndex = index;
			AlliancePrintFormatControls.inputPrefixCd.val(resultUnique.PrefixCd);			
			AlliancePrintFormatControls.selectPrefixs.UifSelect('setSelected', resultUnique.PrefixCd);
			AlliancePrintFormatControls.selectEndorsementTypes.UifSelect('setSelected', resultUnique.EndoTypeCd);
			AlliancePrintFormatControls.selectPrefixs.attr("disabled", "true");
			AlliancePrintFormatControls.selectEndorsementTypes.attr("disabled", "true");
            AlliancePrintFormatControls.inputEndoTypeCd.val(resultUnique.EndoTypeCd);
            AlliancePrintFormatControls.inputFormat.val(resultUnique.Format);
            AlliancePrintFormatControls.inputSearchAlliancePrintFormat.val(resultUnique.Format);
            AlliancePrintFormatControls.inputIdAlliancePrintFormat.val(resultUnique.Id);
            AlliancePrintFormatControls.inputId.val(resultUnique.Id);
            if (resultUnique.Enable == true)
            {
                AlliancePrintFormatControls.chkIsEnable.prop('checked', true);
            }
            else {
                AlliancePrintFormatControls.chkIsEnable.prop('checked', false);
            }
        }
    }

    /**
    *@summary Muestra los formatos de impresión de aliados Encontrados.
    *@param {dataTable} * Captura el control donde se mostraran los formatos de impresión de aliados encontrados.
    */
    static ShowDefaultResult(dataTable) {
        AlliancePrintFormatControls.tableResults.UifDataTable('clear');
        AlliancePrintFormatControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR los formatos de impresión de aliados.-------------------//

    /**
    *@summary Obtiene el formato de impresión de aliados seleccionado.
    *@param {id} * ID del formato de impresión de aliado.
    */
    static GetAlliancePrintFormat(id) {
        var find = false;
        var data = [];
        var search = glbAlliancePrintFormat;
        $.each(search, function (key, value) {
            if (value.Id == id) {
                alliancePrintFormatIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationAlliancePrintFormat.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.AlliancePrintFormatNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un formato de impresión de aliados al listview y al array de los formatos de impresión de aliados por agregar.
    */
    AddItemAlliancePrintFormat() {
        AlliancePrintFormatControls.formAlliancePrintFormat.validate();
        var valEnable = false;
        if ($(AlliancePrintFormatControls.chkIsEnable).is(":checked"))
        {
            valEnable = true;
        }
        if (AlliancePrintFormatControls.formAlliancePrintFormat.valid()) {
            var lAlliancePrintFormat = {
                Id: AlliancePrintFormatControls.inputId.val(),                
				PrefixCd: AlliancePrintFormatControls.selectPrefixs.UifSelect("getSelected"),
				EndoTypeCd: AlliancePrintFormatControls.selectEndorsementTypes.UifSelect("getSelected"),
                Format: AlliancePrintFormatControls.inputFormat.val(),
				Enable: valEnable,
				PrefixDescription: $("#selectPrefixs option:selected").html(),
				EndorsementDescription: $("#selectEndorsementTypes option:selected").html()
			}
			var valSelectsRequiredOk = true;
			if ($("#selectPrefixs option:selected").index() == 0)
			{
				$.UifNotify('show', {
					'type': 'danger', 'message': Resources.Language.RequiredPrefixName, 'autoclose': true
				});
				valSelectsRequiredOk = false;
			}
			if ($("#selectEndorsementTypes option:selected").index() == 0)
			{
				$.UifNotify('show', {
					'type': 'danger', 'message': Resources.Language.RequiredEndoType, 'autoclose': true
				});
				valSelectsRequiredOk = false;
			}
			if (valSelectsRequiredOk)
			{
				if (alliancePrintFormatIndex == null) {
					var list = AlliancePrintFormatControls.listAlliancePrintFormat.UifListView('getData');
					var ifExist = list.filter(function (item) {
						return item.Format.toLowerCase()
							== lAlliancePrintFormat.Format.toLowerCase();
					});
					if (ifExist.length > 0) {
						$.UifNotify('show', {
							'type': 'danger', 'message': Resources.Language.ErrorExistAlliancePrintFormatName, 'autoclose': true
						});
					}
					else {
                        lAlliancePrintFormat.Status = ParametrizationStatus.Create;
						AlliancePrintFormatControls.listAlliancePrintFormat.UifListView("addItem", lAlliancePrintFormat);
					}
				}
				else {
					if (lAlliancePrintFormat.Id != 0) {
                        lAlliancePrintFormat.Status = ParametrizationStatus.Update;
					} else {
                        lAlliancePrintFormat.Status = ParametrizationStatus.Create;
					}

					AlliancePrintFormatControls.listAlliancePrintFormat.UifListView('editItem', alliancePrintFormatIndex, lAlliancePrintFormat);
				}
				ParametrizationAlliancePrintFormat.CleanForm();
			}
        }
    }

    /**
    *@summary Elimina un formato de impresión de aliados del listview y lo agrega al array de los formatos de impresión de aliados por eliminiar.
    */
    DeleteItemAlliancePrintFormat(event, data) {
        event.resolve();
        if (data.Id != 0) {
            data.Status = ParametrizationStatus.Delete;
            data.allowEdit = false;
            data.allowDelete = false;
            $("#listAlliancePrintFormat").UifListView("addItem", data);
        }
        
        ParametrizationAlliancePrintFormat.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para los formatos de impresión de aliados con los arrays cargados.
    */
    SaveAlliancePrintFormat() {
        var lAlliancePrintFormats = AlliancePrintFormatControls.listAlliancePrintFormat.UifListView('getData');
        $.each(lAlliancePrintFormats, function (key, value) {
            if (this.Status == ParametrizationStatus.Create) {
                glbAlliancePrintFormatAdded.push(this);
            }
            else if (this.Status == ParametrizationStatus.Update) {
                glbAlliancePrintFormatModified.push(this);
            }
            else if (this.Status == ParametrizationStatus.Delete) {
                glbAlliancePrintFormatDelete.push(this);
            }
        });
        if (glbAlliancePrintFormatAdded.length > 0 || glbAlliancePrintFormatModified.length > 0 || glbAlliancePrintFormatDelete.length > 0) {
            AlliancePrintFormats.SaveAlliancePrintFormats(glbAlliancePrintFormatAdded, glbAlliancePrintFormatModified, glbAlliancePrintFormatDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationAlliancePrintFormat.RefreshList();
                        glbAlliancePrintFormat = data.result.data;
                        ParametrizationAlliancePrintFormat.LoadAlliancePrintFormats();
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
                        'type': 'danger', 'message': Resources.Language.ErrorSaveAlliancePrintFormat, 'autoclose': true
                    })

                });
        }

    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        alliancePrintFormatIndex = null;
        AlliancePrintFormatControls.inputPrefixCd.val(null);
        AlliancePrintFormatControls.inputEndoTypeCd.val(null);
        AlliancePrintFormatControls.inputFormat.val(null);
        AlliancePrintFormatControls.chkIsEnable.prop('checked', false);
        AlliancePrintFormatControls.inputSearchAlliancePrintFormat.val(null);
        AlliancePrintFormatControls.inputIdAlliancePrintFormat.val(0);
		AlliancePrintFormatControls.inputId.val(null);
		AlliancePrintFormatControls.selectPrefixs.UifSelect('setSelected', null);
		AlliancePrintFormatControls.selectEndorsementTypes.UifSelect('setSelected', null);
		AlliancePrintFormatControls.selectPrefixs.prop("disabled", false);
		AlliancePrintFormatControls.selectEndorsementTypes.prop("disabled", false);
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}