var glbScoreTypeDoc = {};
var inputSearch = "";
var modalListType;
var ScoreTypeDocIndex = null;
var glbScoreTypeDocAdded = [];
var glbScoreTypeDocModified = [];
var glbScoreTypeDocDelete = [];
var dropDownSearchAdvScoreTypeDoc = null;


$(() => {
    new ParametrizationScoreTypeDoc();
});


class ScoreTypeDocs {

    /**
    *@summary Obtiene la lista de los tipos documento datcrédito
    *@returns {Array} Lista de los tipos documento datcrédito
    */
    static GetScoreTypeDocs() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/GetScoreTypeDocs',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los tipos documento datcrédito
    *@returns {Array} Lista de los tipos documento datcrédito con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
    static SaveScoreTypeDocs(glbScoreTypeDocAdded, glbScoreTypeDocModified, glbScoreTypeDocDelete) {
        return $.ajax({
            type: 'POST',
            url: 'SaveScoreTypeDocs',
            data: JSON.stringify({ listAdded: glbScoreTypeDocAdded, listModified: glbScoreTypeDocModified, listDeleted: glbScoreTypeDocDelete }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para los tipos documento datcrédito
    */
    static GenerateScoreTypeDocFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateScoreTypeDocFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class ScoreTypeDocControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get ScoreTypeDocTemplate() { return "#ScoreTypeDocTemplate"; }
    static get listScoreTypeDoc() { return $("#listScoreTypeDoc"); }
    static get btnNewScoreTypeDoc() { return $("#btnNewScoreTypeDoc"); }
    static get btnExit() { return $("#btnExit"); }
    static get btnScoreTypeDocAccept() { return $("#btnScoreTypeDocAccept"); }
    static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
    static get inputDescription() { return $("#inputDescription"); }
    static get inputIdScore3g() { return $("#inputIdScore3g"); }
    static get inputSmallDescription() { return $("#inputSmallDescription"); }
    static get inputSearchScoreTypeDoc() { return $("#inputSearchScoreTypeDoc"); }
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formScoreTypeDoc() { return $("#formScoreTypeDoc"); }
    static get btnSaveScoreTypeDoc() { return $("#btnSaveScoreTypeDoc"); }
    static get sendExcelScoreTypeDoc() { return $("#btnExport"); }
}

class ParametrizationScoreTypeDoc extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        ScoreTypeDocControls.listScoreTypeDoc.UifListView({
            displayTemplate: "#ScoreTypeDocTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemScoreTypeDoc,
            customAdd: true,
            customEdit: true,
            height: 300
        });

        /**
        * @summary Inicializa la lista de los tipos documento datcrédito
        * @returns {Array} Lista de los tipos documento datcrédito
        */
        DocumentTypeCodeRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#selectDocumentTypeCode").UifSelect({ sourceData: data.result });
			}

			ScoreTypeDocs.GetScoreTypeDocs().done(function (data) {
				if (data.success) {
					$.each(data.result, function (key, value) {
						value.Status = "Origin";
						value.StatusType = 1;
					});
					glbScoreTypeDoc = data.result;
					ParametrizationScoreTypeDoc.LoadScoreTypeDocs();
				}
				else {
					$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
				}
			});
        });

		$("#inputId").ValidatorKey(ValidatorType.Number, 2, 0);

    }


    //---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        ScoreTypeDocControls.inputSearchScoreTypeDoc.on('itemSelected', this.showScoreTypeDocSerch);
        ScoreTypeDocControls.inputSearchScoreTypeDoc.on("buttonClick", this.showScoreTypeDocSerch);
        ScoreTypeDocControls.btnScoreTypeDocAccept.on("click", this.AddItemScoreTypeDoc);
        ScoreTypeDocControls.listScoreTypeDoc.on('rowEdit', ParametrizationScoreTypeDoc.ShowData);
        ScoreTypeDocControls.listScoreTypeDoc.on('rowDelete', this.DeleteItemScoreTypeDoc);
        ScoreTypeDocControls.btnSaveScoreTypeDoc.on("click", this.SaveScoreTypeDoc);
        ScoreTypeDocControls.btnNewScoreTypeDoc.on("click", ParametrizationScoreTypeDoc.CleanForm);
        ScoreTypeDocControls.btnExit.on("click", this.Exit);
        ScoreTypeDocControls.sendExcelScoreTypeDoc.on("click", this.sendExcelScoreTypeDoc);
    }



    static ShowSearchAdv(data) {
        $("#lvSearchAdvScoreTypeDoc").UifListView("clear");
        if (data) {
			data.forEach(item => {
				if (item.IdCardTypeCode == "0") {
					var ScoreTypeDocs = {
						IdCardTypeScore: item.IdCardTypeScore,
						Description: item.Description,
						SmallDescription: item.SmallDescription,
						IdCardTypeCode: item.IdCardTypeCode,
						IdScore3g: item.IdScore3g,
						NoneSISEDescription: "Ninguno"
					}
				}
				else {
					var siseDescription = $("#selectDocumentTypeCode option[value='" + item.IdCardTypeCode + "']").text() + " (" + item.IdCardTypeCode + ")";
					var ScoreTypeDocs = {
						IdCardTypeScore: item.IdCardTypeScore,
						Description: item.Description,
						SmallDescription: item.SmallDescription,
						IdCardTypeCode: item.IdCardTypeCode,
						IdScore3g: item.IdScore3g,
						SISEDescription: siseDescription
					}
				}
				$("#lvSearchAdvScoreTypeDoc").UifListView("addItem", ScoreTypeDocs);
            });
        }
        dropDownSearchAdvScoreTypeDoc.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvScoreTypeDoc.hide();
    }





    /**
    * @summary Carga la lista de los tipos documento datcrédito
    */
	static LoadScoreTypeDocs() {
        ScoreTypeDocControls.listScoreTypeDoc.UifListView("clear");
        $.each(glbScoreTypeDoc, function (key, value) {
            if (this.IdCardTypeCode=="0") {
                var ScoreTypeDocs = {
                    IdCardTypeScore: this.IdCardTypeScore,
                    Description: this.Description,
                    SmallDescription: this.SmallDescription,
                    IdCardTypeCode: this.IdCardTypeCode,
                    IdScore3g: this.IdScore3g,
                    NoneSISEDescription: "Ninguno"
                }
            }
			else {
				var siseDescription = $("#selectDocumentTypeCode option[value='" + this.IdCardTypeCode + "']").text() + " (" + this.IdCardTypeCode + ")";
                var ScoreTypeDocs = {
                    IdCardTypeScore: this.IdCardTypeScore,
                    Description: this.Description,
                    SmallDescription: this.SmallDescription,
                    IdCardTypeCode: this.IdCardTypeCode,
                    IdScore3g: this.IdScore3g,
					SISEDescription: siseDescription
                }
            }
            ScoreTypeDocControls.listScoreTypeDoc.UifListView("addItem", ScoreTypeDocs);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelScoreTypeDoc() {
        ScoreTypeDocs.GenerateScoreTypeDocFileToExport().done(function (data) {
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
    * @summary Limpia las listas para el CRUD de los tipos documento datcrédito
    */
    static RefreshList() {
        glbScoreTypeDocAdded = [];
        glbScoreTypeDocModified = [];
        glbScoreTypeDocDelete = [];
    }



    //---------------------BUSCAR SUCURSAL-------------------//

    /**
    *@summary Inicia la busqueda de los tipos documento datcrédito
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
    showScoreTypeDocSerch(event, selectedItem) {
        inputSearch = selectedItem;
        ParametrizationScoreTypeDoc.SearchScoreTypeDoc(inputSearch);
    }

    /**
     *@summary Realiza la busqueda de los tipos documento datcrédito
     *@param {description} *Captura el texto escrito por el usuario
     */
    static SearchScoreTypeDoc(description) {
        var find = false;
        var data = [];
        var search = glbScoreTypeDoc;
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
                        'type': 'danger', 'message': AppResourcesPerson.ScoreTypeDocNotFound, 'autoclose': true
                    })
            } else {
                ParametrizationScoreTypeDoc.ShowData(null, data, data.key);
            }
        }

    }

    /**
    *@summary Muestra los tipos documento datcrédito Encontrados
    *@returns {object} *Obtiene el tipo documento datcrédito encontrado y la muestra
    *@returns {Array} *Obtiene la lista de los tipos documento datcrédito que coiniciden con la busqueda
    */
    static ShowData(event, result, index) {
        ParametrizationScoreTypeDoc.CleanForm();
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            resultUnique = result[0];
        }
        else if (result.length > 1) {
            ParametrizationScoreTypeDoc.ShowSearchAdv(result);
        }
        else if (result.length == undefined) {
            resultUnique = result;
        }
        if (resultUnique != null) {
            ScoreTypeDocIndex = index;
            ScoreTypeDocControls.inputDescription.val(resultUnique.Description);
            ScoreTypeDocControls.inputSmallDescription.val(resultUnique.SmallDescription);
            ScoreTypeDocControls.inputSearchScoreTypeDoc.val(resultUnique.Description);
            ScoreTypeDocControls.inputId.val(resultUnique.IdCardTypeScore);
            ScoreTypeDocControls.inputIdScore3g.val(resultUnique.IdScore3g);
            $("#selectDocumentTypeCode").UifSelect("setSelected", resultUnique.IdCardTypeCode);
        }
    }

    /**
    *@summary Muestra los tipos documento datcrédito Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de los tipos documento datcrédito encontrados
    */
    static ShowDefaultResult(dataTable) {
        ScoreTypeDocControls.tableResults.UifDataTable('clear');
        ScoreTypeDocControls.tableResults.UifDataTable('addRow', dataTable);
    }

    //----------------------------------------------------------//


    //---------------------SELECCIONAR los tipos documento datcrédito-------------------//

    /**
    *@summary Obtiene el tipo documento datcrédito seleccionado
    *@param {id} * ID del tipo documento datcrédito
    */
    static GetScoreTypeDoc(id) {
        var find = false;
        var data = [];
        var search = glbScoreTypeDoc;
        $.each(search, function (key, value) {
            if (value.IdCardTypeScore == id) {
                ScoreTypeDocIndex = key;
                value.key = key;
                data.push(value);
                find = true;
                return false;
            }
        });
        ParametrizationScoreTypeDoc.ShowData(null, data, data.key);
        if (find == false) {
            $.UifNotify('show', {
                'type': 'danger', 'message': AppResourcesPerson.ProtectionNotFound, 'autoclose': true
            })
        }
    }

    //----------------------------------------------------------//


    //---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega un tipo documento datcrédito al listview y al array de los tipos documento datcrédito por agregar
    */
    AddItemScoreTypeDoc() {
        ScoreTypeDocControls.formScoreTypeDoc.validate();
        if (ScoreTypeDocControls.formScoreTypeDoc.valid()) {
            if ($('#selectDocumentTypeCode').UifSelect("getSelected") != null && $('#selectDocumentTypeCode').UifSelect("getSelected")!=0 ) {

                var scoreTypeDoc = {
                    IdCardTypeScore: ScoreTypeDocControls.inputId.val(),
                    Description: ScoreTypeDocControls.inputDescription.val(),
                    SmallDescription: ScoreTypeDocControls.inputSmallDescription.val(),
                    IdCardTypeCode: $('#selectDocumentTypeCode').UifSelect("getSelected"),
                    IdScore3g: ScoreTypeDocControls.inputIdScore3g.val(),
                    SISEDescription: $("#selectDocumentTypeCode option:selected").html()
                }
            }
            else {
                var scoreTypeDoc = {
                    IdCardTypeScore: ScoreTypeDocControls.inputId.val(),
                    Description: ScoreTypeDocControls.inputDescription.val(),
                    SmallDescription: ScoreTypeDocControls.inputSmallDescription.val(),
                    IdCardTypeCode: $('#selectDocumentTypeCode').UifSelect("getSelected"),
                    IdScore3g: ScoreTypeDocControls.inputIdScore3g.val(),
                    NoneSISEDescription: "Ninguno"
                }
            }
            if (ScoreTypeDocIndex == null) {
                var list = ScoreTypeDocControls.listScoreTypeDoc.UifListView('getData');
                var ifExist = list.filter(function (item) {
                    return item.IdCardTypeScore == scoreTypeDoc.IdCardTypeScore;
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': AppResourcesPerson.ErrorExistScoreTypeDocCode, 'autoclose': true
                    });
                }
				else {
					var ifExist = list.filter(function (item) {
						return item.Description.toLowerCase().sistranReplaceAccentMark()
							== scoreTypeDoc.Description.toLowerCase().sistranReplaceAccentMark();
					});
					if (ifExist.length > 0) {
                        $.UifNotify('show', {
                            'type': 'danger', 'message': AppResourcesPerson.ErrorExistScoreTypeDocName, 'autoclose': true
						});
					}
					else {
						scoreTypeDoc.Status = 'Added';
						scoreTypeDoc.StatusType = 2;
						ScoreTypeDocControls.listScoreTypeDoc.UifListView("addItem", scoreTypeDoc);
					}
                }
            }
            else {
				if (scoreTypeDoc.Status != 'Added') {
					scoreTypeDoc.Status = 'Modified';
					scoreTypeDoc.StatusType = 3;
                } 

                else {
                    scoreTypeDoc.Status = 'Added';
                    scoreTypeDoc.StatusType = 2;
                }

                ScoreTypeDocControls.listScoreTypeDoc.UifListView('editItem', ScoreTypeDocIndex, scoreTypeDoc);
			}
			listViewColors('listScoreTypeDoc');
            ParametrizationScoreTypeDoc.CleanForm();
        }
    }

    /**
    *@summary Elimina un tipo documento datcrédito del listview y lo agrega al array de los tipos documento datcrédito por eliminiar
    */
    DeleteItemScoreTypeDoc(event, data) {
        event.resolve();
        if (data.Id != 0 && data.Status != "Added") {
            data.Status = "Delete"
            data.StatusType = 4;
            data.allowEdit = false;
            data.allowDelete = false;
            $("#listScoreTypeDoc").UifListView("addItem", data);
            //glbScoreTypeDocDelete.push(data);
        }
        
        ParametrizationScoreTypeDoc.CleanForm();
    }

    /**
    *@summary Inicia los procesos del CRUD para los tipos documento datcrédito con los arrays cargados
    */
    SaveScoreTypeDoc() {
        var scoreTypeDocs = ScoreTypeDocControls.listScoreTypeDoc.UifListView('getData');
        $.each(scoreTypeDocs, function (key, value) {
            if (this.Status == "Added") {
                glbScoreTypeDocAdded.push(this);
            }
            else if (this.Status == "Modified") {
                glbScoreTypeDocModified.push(this);
            }
            else if (this.Status == "Delete") {
                glbScoreTypeDocDelete.push(this);
            }
        });
        if (glbScoreTypeDocAdded.length > 0 || glbScoreTypeDocModified.length > 0 || glbScoreTypeDocDelete.length > 0) {
            ScoreTypeDocs.SaveScoreTypeDocs(glbScoreTypeDocAdded, glbScoreTypeDocModified, glbScoreTypeDocDelete)
                .done(function (data) {
                    if (data.success) {
                        ParametrizationScoreTypeDoc.RefreshList();
                        glbScoreTypeDoc = data.result.data;
                        ParametrizationScoreTypeDoc.LoadScoreTypeDocs();
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
                        'type': 'danger', 'message': AppResourcesPerson.ErrorSaveScoreTypeDocs, 'autoclose': true
                    })

                });
        }

    }

    //----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
    static CleanForm() {
        ScoreTypeDocIndex = null;
        ScoreTypeDocControls.inputDescription.val(null);
        ScoreTypeDocControls.inputSmallDescription.val(null);
        ScoreTypeDocControls.inputSearchScoreTypeDoc.val(null);
        ScoreTypeDocControls.inputId.val(null);
        ScoreTypeDocControls.inputId.focus();
        $("#selectDocumentTypeCode").UifSelect("setSelected", null);
        ScoreTypeDocControls.inputIdScore3g.val(null);
        ClearValidation("#formScoreTypeDoc");
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

class DocumentTypeCodeRequest {
    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetDocumentType",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

