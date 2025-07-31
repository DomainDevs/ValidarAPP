var glbLegalRepresentativeSing = {};
var inputSearch = "";
var modalListType;
var LegalRepresentativeSingIndex = null;
var glbLegalRepresentativeSingAdded = [];
var glbLegalRepresentativeSingModified = [];
var dropDownSearchAdvLegalRepresentativeSing = null;

$(() => {
	new ParametrizationLegalRepresentativeSing();
});

class LegalRepresentativeSing {
    /**
    *@summary Obtiene la lista de firma de representante legal
    *@returns {Array} Lista de canales
    */
	static GetAllLegalRepresentativeSing() {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Printing/LegalRepresentativeSing/GetAllLegalRepresentativeSing',
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}

    /**
    *@summary Realiza los procesos del CRUD para las firma de representante legal
    *@returns {Array} Lista de canales con los cambios
    *@returns {string} Mensaje con los procesos realizados
    */
	static SaveLegalRepresentativeSing(glbLegalRepresentativeSingAdded, glbLegalRepresentativeSingModified) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Printing/LegalRepresentativeSing/SaveLegalRepresentativeSing',
			data: JSON.stringify({ listAdded: glbLegalRepresentativeSingAdded, listModified: glbLegalRepresentativeSingModified }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}

    /**
    *@summary Realiza los procesos del CRUD para las firma de representante legal
    */
	static GenerateFileToExport() {
		return $.ajax({
			type: 'POST',
			url: 'GenerateFileToExport',
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}

	static GetBranchTypes(selectedId) {
		$.ajax({
			type: "POST",
			url: rootPath + "Printing/LegalRepresentativeSing/GetBranchTypes",
			dataType: "json",
			contentType: "application/json; charset=utf-8"
		}).done(function (data) {
			if (data.success) {
				if (selectedId == undefined) {
					$("#selectBranchTypeCode").UifSelect({ sourceData: data.result });
				}
				else {
					$("#selectBranchTypeCode").UifSelect({ sourceData: data.result, selectedId: selectedId });
				}
			}
		});
	}

	static GetCompanyTypes(selectedId) {
		$.ajax({
			type: "POST",
			url: rootPath + "Printing/LegalRepresentativeSing/GetCompanyTypes",
			dataType: "json",
			contentType: "application/json; charset=utf-8"
		}).done(function (data) {
			if (data.success) {
				if (selectedId == undefined) {
					$("#selectCiaCode").UifSelect({ sourceData: data.result });
				}
				else {
					$("#selectCiaCode").UifSelect({ sourceData: data.result, selectedId: selectedId });
				}
			}
		});
	}
}

class LegalRepresentativeSingControls {
	static get SelectorTypeText() { return $("input[type=text]"); }
	static get LegalRepresentativeSingTemplate() { return "#LegalRepresentativeSingTemplate"; }
	static get listLegalRepresentativeSing() { return $("#listLegalRepresentativeSing"); }
	static get btnNewLegalRepresentativeSing() { return $("#btnNewLegalRepresentativeSing"); }
	static get btnExit() { return $("#btnExit"); }
	static get btnLegalRepresentativeSingAccept() { return $("#btnLegalRepresentativeSingAccept"); }
	static get modalDefaultSearch() { return $("#modalDefaultSearch"); }
	static get selectCiaCode() { return $("#selectCiaCode"); }
	static get selectBranchTypeCode() { return $("#selectBranchTypeCode"); }
	static get selectCiaDescription() { return $("#selectCiaCode option:selected"); }
	static get selectBranchTypeDescription() { return $("#selectBranchTypeCode option:selected"); }
	static get inputLegalRepresentative() { return $("#inputLegalRepresentative"); }
	static get inputCurrentFrom() { return $("#inputCurrentFrom"); }
	static get inputPathSignatureImg() { return $("#inputPathSignatureImg"); }
	static get imgSignatureImg() { return $("#imgSignatureImg"); }
	static get inputSearchLegalRepresentativeSing() { return $("#inputSearchLegalRepresentativeSing"); }
	static get tableResults() { return $("#tableResults"); }
	static get formLegalRepresentativeSing() { return $("#formLegalRepresentativeSing"); }
	static get btnSaveLegalRepresentativeSing() { return $("#btnSaveLegalRepresentativeSing"); }
	static get sendExcelLegalRepresentativeSing() { return $("#btnExport"); }
}

class ParametrizationLegalRepresentativeSing extends Uif2.Page {
	//---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
	getInitialState() {
		$("input[type=text]").TextTransform(ValidatorType.UpperCase);
		LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView({
			displayTemplate: "#LegalRepresentativeSingTemplate",
			edit: true,
			customAdd: true,
			customEdit: true,
			height: 300
		});

        /**
        * @summary Inicializa la lista de firma de representante legal
        * @returns {Array} Lista de canales
        */
        LegalRepresentativeSing.GetAllLegalRepresentativeSing().done(function (data) {
			if (data.success) {
				//alert(JSON.stringify(data.result))
				glbLegalRepresentativeSing = data.result;
				ParametrizationLegalRepresentativeSing.LoadLegalRepresentativeSing();
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});

		LegalRepresentativeSing.GetBranchTypes();

		LegalRepresentativeSing.GetCompanyTypes();
	}

	//---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
	bindEvents() {
		LegalRepresentativeSingControls.inputSearchLegalRepresentativeSing.on('itemSelected', this.showLegalRepresentativeSingSerch);
		LegalRepresentativeSingControls.inputSearchLegalRepresentativeSing.on("buttonClick", this.showLegalRepresentativeSingSerch);
		LegalRepresentativeSingControls.btnLegalRepresentativeSingAccept.on("click", this.AddItemLegalRepresentativeSing);
		LegalRepresentativeSingControls.listLegalRepresentativeSing.on('rowEdit', ParametrizationLegalRepresentativeSing.ShowData);
		LegalRepresentativeSingControls.btnSaveLegalRepresentativeSing.on("click", this.SaveLegalRepresentativeSing);
		LegalRepresentativeSingControls.btnNewLegalRepresentativeSing.on("click", ParametrizationLegalRepresentativeSing.CleanForm);
		LegalRepresentativeSingControls.btnExit.on("click", this.Exit);
		LegalRepresentativeSingControls.sendExcelLegalRepresentativeSing.on("click", this.sendExcelLegalRepresentativeSing);

		function readURL(input) {
			if (input.files && input.files[0]) {
				var archivo = $("#imgInp").val();
				var extension = archivo.substring(archivo.lastIndexOf("."));
				var extenciones = $("#imgInp").attr("accept").split(",");
				var bandera = 0;

				$.each(extenciones, function (key, value) {
					if (value == extension) {
						bandera = 1;
					}
				});

				if (bandera != 0) {
					var reader = new FileReader();
					var signature_width = $("#Signature_width").val();
					var signature_heigth = $("#Signature_height").val();
					var signature_size = $("#Signature_size").val();

					if (input.files[0].size <= signature_size) {
						reader.onload = function (e) {
							var img = new Image;
							img.src = reader.result;
							img.onload = function () {
								if (img.width == signature_width && img.height == signature_heigth) {
									LegalRepresentativeSingControls.imgSignatureImg.attr('src', e.target.result);
								} else {
									$("#imgInp").val(null);
									$($("#imgInp").siblings("div").children("input")[0]).val("");
									LegalRepresentativeSingControls.imgSignatureImg.removeAttr('src');
									$.UifNotify('show', {
										'type': 'danger', 'message': $("#ImageWidthAndHeight").val(), 'autoclose': false
									});
								}
							};
						}
					} else {
						$("#imgInp").val(null);
						$($("#imgInp").siblings("div").children("input")[0]).val("");
						LegalRepresentativeSingControls.imgSignatureImg.removeAttr('src');
						$.UifNotify('show', {
							'type': 'danger', 'message': $("#ImageSize").val(), 'autoclose': false
						});
						return false;
					}
					reader.readAsDataURL(input.files[0]);
				} else {
					$($("#imgInp").siblings("div").children("input")[0]).val("");
					LegalRepresentativeSingControls.imgSignatureImg.removeAttr('src');
					$.UifNotify('show', {
						'type': 'danger', 'message': $("#ImageFormt").val(), 'autoclose': false
					});
				}
			}
		}

		$("#imgInp").on("change", function () {
			readURL(this);
		});
	}
	
	static ShowSearchAdv(data) {
		$("#lvSearchAdvLegalRepresentativeSing").UifListView("clear");
		if (data) {
			data.forEach(item => {
				$("#lvSearchAdvLegalRepresentativeSing").UifListView("addItem", item);
			});
		}
		dropDownSearchAdvLegalRepresentativeSing.show();
	}

	static HideSearchAdv() {
		dropDownSearchAdvLegalRepresentativeSing.hide();
	}

    /**
    * @summary Carga la lista de firma de representante legal
    */
	static LoadLegalRepresentativeSing() {
		LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView("clear");
		$.each(glbLegalRepresentativeSing, function (key, value) {
			var legalRepresentativesSing = {
				CiaCode: this.CiaCode,//this.Id,
				BranchTypeCode: this.BranchTypeCode,
				CiaDescription: this.CiaDescription,
				BranchTypeDescription: this.BranchTypeDescription,
				CurrentFrom: FormatDate(this.CurrentFrom),
				LegalRepresentative: this.LegalRepresentative,
				PathSignatureImg: this.PathSignatureImg,
				SignatureImg: this.SignatureImg,
				UserId: this.UserId
			}
			LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView("addItem", legalRepresentativesSing);
		});
	}

    /**
    * @summary Descarga archivo excel
    */
	sendExcelLegalRepresentativeSing() {
		LegalRepresentativeSing.GenerateFileToExport().done(function (data) {
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
    * @summary Limpia las listas para el CRUD de firma de representante legal
    */
	static RefreshList() {
		glbLegalRepresentativeSingAdded = [];
		glbLegalRepresentativeSingModified = [];
	}

	//---------------------BUSCAR SUCURSAL-------------------//

    /**
    *@summary Inicia la busqueda de firma de representante legal
    *@param {event} *Captura el evento del control
    *@param {selectedItem} *Captura el texto escrito por el usuario
    */
	showLegalRepresentativeSingSerch(event, selectedItem) {
		inputSearch = selectedItem;
		ParametrizationLegalRepresentativeSing.SearchLegalRepresentativeSing(inputSearch);
	}

    /**
     *@summary Realiza la busqueda de firma de representante legal
     *@param {description} *Captura el texto escrito por el usuario
     */
	static SearchLegalRepresentativeSing(description) {
		var find = false;
		var data = [];
		var search = glbLegalRepresentativeSing;
		if (description.length < 3) {
			$.UifNotify('show', {
				'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
			})
		} else {
			$.each(search, function (key, value) {
				if ((value.LegalRepresentative.toLowerCase().sistranReplaceAccentMark().
					includes(description.toLowerCase().sistranReplaceAccentMark()))) {
					value.key = key;
					data.push(value);
					find = true;
				}
				value.CurrentFrom = FormatDate(value.CurrentFrom);
			});

			if (find == false) {
				$.UifNotify('show',
					{
						'type': 'danger', 'message': Resources.Language.LegalRepresentativeSingNotFound, 'autoclose': true
					})
			} else {
				ParametrizationLegalRepresentativeSing.ShowData(null, data, data.key);
			}
		}
	}

    /**
    *@summary Muestra las canales Encontradas
    *@returns {object} *Obtiene la canal encontrada y la muestra 
    *@returns {Array} *Obtiene la lista de canales que coiniciden con la busqueda
    */
	static ShowData(event, result, index) {
		var resultUnique = null;
		if (result.length == 1) {
			index = result[0].key;
			resultUnique = result[0];
		}
		else if (result.length > 1) {
			ParametrizationLegalRepresentativeSing.ShowSearchAdv(result);
		}
		else if (result.length == undefined) {
			resultUnique = result;
		}
		if (resultUnique != null) {
			LegalRepresentativeSingIndex = index;
            		LegalRepresentativeSingControls.selectCiaCode.UifSelect("setSelected", resultUnique.CiaCode);
            		LegalRepresentativeSingControls.selectBranchTypeCode.UifSelect("setSelected", resultUnique.BranchTypeCode);
			LegalRepresentativeSingControls.inputLegalRepresentative.val(resultUnique.LegalRepresentative);
			LegalRepresentativeSingControls.inputCurrentFrom.UifDatepicker('setValue', FormatDate(resultUnique.CurrentFrom));
			LegalRepresentativeSingControls.inputPathSignatureImg.val(resultUnique.PathSignatureImg);
            		LegalRepresentativeSingControls.selectCiaCode.UifSelect("disabled", true);
            		LegalRepresentativeSingControls.selectBranchTypeCode.UifSelect("disabled", true);
			LegalRepresentativeSingControls.inputCurrentFrom.prop("disabled", true);
			LegalRepresentativeSingControls.imgSignatureImg.attr('src', 'data:image/jpeg;base64,' + resultUnique.SignatureImg);
		}
	}

    /**
    *@summary Muestra las canales Encontradas
    *@param {dataTable} * Captura el control donde se mostrara la lista de canales encontradas
    */
	static ShowDefaultResult(dataTable) {
		LegalRepresentativeSingControls.tableResults.UifDataTable('clear');
		LegalRepresentativeSingControls.tableResults.UifDataTable('addRow', dataTable);
	}

	//----------------------------------------------------------//


	//---------------------PROCESOS BASICOS-------------------//

    /**
    *@summary Agrega una canal al listview y al array de canales por agregar 
    */
	AddItemLegalRepresentativeSing() {
		LegalRepresentativeSingControls.formLegalRepresentativeSing.validate();
		if (LegalRepresentativeSingControls.formLegalRepresentativeSing.valid()) {
			var src = LegalRepresentativeSingControls.imgSignatureImg.attr("src");
			if (src != undefined) {
				var imgSignatureImg = src.split(",");

				var nameImg = $("#imgInp").val();
				var pathSignatureImg = nameImg.split("\\");

				var legalRepresentativeSing = {
			                CiaCode: LegalRepresentativeSingControls.selectCiaCode.UifSelect("getSelected"),
			                BranchTypeCode: LegalRepresentativeSingControls.selectBranchTypeCode.UifSelect("getSelected"),
					CiaDescription: LegalRepresentativeSingControls.selectCiaDescription.text(),
					BranchTypeDescription: LegalRepresentativeSingControls.selectBranchTypeDescription.text(),
					LegalRepresentative: LegalRepresentativeSingControls.inputLegalRepresentative.val(),
					CurrentFrom: LegalRepresentativeSingControls.inputCurrentFrom.val(),
					PathSignatureImg: pathSignatureImg[pathSignatureImg.length - 1],
					SignatureImg: imgSignatureImg[1]
				}
				if (LegalRepresentativeSingIndex == null) {
					var list = LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView('getData');
					var ifExist = list.filter(function (item) {
						return item.CiaCode == legalRepresentativeSing.CiaCode &&
							item.BranchTypeCode == legalRepresentativeSing.BranchTypeCode &&
							item.CurrentFrom.toLowerCase().sistranReplaceAccentMark()
							== legalRepresentativeSing.CurrentFrom.toLowerCase().sistranReplaceAccentMark();
					});
					if (ifExist.length > 0) {
						$.UifNotify('show', {
							'type': 'danger', 'message': Resources.Language.ErrorExistLegalRepresentativeSingName, 'autoclose': true
						});
						return;
					}
					else {
                        legalRepresentativeSing.Status = ParametrizationStatus.Create;
						LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView("addItem", legalRepresentativeSing);
					}
				}
				else {
					if (legalRepresentativeSing.Id != "") {
                        legalRepresentativeSing.Status = ParametrizationStatus.Update;
					} else {
                        legalRepresentativeSing.Status = ParametrizationStatus.Create;
					}

					LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView('editItem', LegalRepresentativeSingIndex, legalRepresentativeSing);
				}
				ParametrizationLegalRepresentativeSing.CleanForm();
			} else {
				$.UifNotify('show', {
					'type': 'danger', 'message': Resources.Language.SelectImage, 'autoclose': true
				})
			}
		}
	}

    /**
    *@summary Inicia los procesos del CRUD para las firma de representante legal con los arrays cargados 
    */
	SaveLegalRepresentativeSing() {
		var legalRepresentativesSingModel = LegalRepresentativeSingControls.listLegalRepresentativeSing.UifListView('getData');
		$.each(legalRepresentativesSingModel, function (key, value) {
            if (this.Status == ParametrizationStatus.Create) {
				glbLegalRepresentativeSingAdded.push(this);
			}
            else if (this.Status == ParametrizationStatus.Update) {
				glbLegalRepresentativeSingModified.push(this);
			}
		});

		if (glbLegalRepresentativeSingAdded.length > 0 || glbLegalRepresentativeSingModified.length > 0) {
			LegalRepresentativeSing.SaveLegalRepresentativeSing(glbLegalRepresentativeSingAdded, glbLegalRepresentativeSingModified)
				.done(function (data) {
					if (data.success) {
						ParametrizationLegalRepresentativeSing.RefreshList();
						glbLegalRepresentativeSing = data.result.data;
						ParametrizationLegalRepresentativeSing.LoadLegalRepresentativeSing();
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
						'type': 'danger', 'message': Resources.Language.ErrorSaveLegalRepresentativeSing, 'autoclose': true
					})
				});
		}
	}

	//----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
	static CleanForm() {
		LegalRepresentativeSingIndex = null;
		LegalRepresentativeSingControls.inputLegalRepresentative.val(null);
		LegalRepresentativeSingControls.inputPathSignatureImg.val(null);
		LegalRepresentativeSingControls.inputSearchLegalRepresentativeSing.val(null);
		LegalRepresentativeSingControls.inputCurrentFrom.UifDatepicker('clear');
	        LegalRepresentativeSingControls.imgSignatureImg.removeAttr("src");
        	LegalRepresentativeSingControls.selectCiaCode.UifSelect("disabled", false);
	        LegalRepresentativeSingControls.selectBranchTypeCode.UifSelect("disabled", false);
        	LegalRepresentativeSingControls.selectCiaCode.UifSelect("setSelected", null);
	        LegalRepresentativeSingControls.selectBranchTypeCode.UifSelect("setSelected", null);
		LegalRepresentativeSingControls.inputCurrentFrom.prop("disabled", false);
		$($("#imgInp").siblings("div").children("input")[0]).val("");
		$("#imgInp").val("")
	}

    /**
   *@summary Redirecciona al index
   */
	Exit() {
		window.location = rootPath + "Home/Index";
	}
}

