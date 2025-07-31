
/**
 * Variables Locales y Globales
 */
var moduleIndex = null;
var inputSearch = "";
var module = {};
var glbRiskTypeByLineBusiness = {};
var datalinebusiness = [];
var RiskTypeByLineBusiness = {};
var glbLinesBusinessCoveredRiskType = {};
var search;
var idCurrentLineBusiness = 0;
var listAsigned = {};
var listAsignedPeril = {};
var loadRisktypes = [];
var update = false;


/**
 * Clase Principal de ramo tecnico
 */

class LineBusinessParametrization extends Uif2.Page {

    /**
     * Función que se ejecuta al instanciar la clase
     */
	getInitialState() {
		new AdvancedSearchLineBusiness();
		new RiskTypeLineBusiness();
		new LineBusinessInsuranceObjectsParametrization();
		new LineBusinessClausesParametrization();
		new LineBusinessProtectionsParametrization();
		LineBusinessCoveredRiskTypeRequest.GetRiskType().done(function (data) {
			if (data.success) {
				loadRisktypes = data.result;

				$('#selectRiskTypeLineBusinessMain').UifSelect({ sourceData: data.result });
				$('#selectRiskTypeCovered').UifSelect({ sourceData: loadRisktypes });
				$('#selectRiskTypeTechnical').UifSelect({ sourceData: loadRisktypes });
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});

		$("#inputLineBusinessCode").ValidatorKey(ValidatorType.Number, 2, 0);

		$("input[type=text]").TextTransform(ValidatorType.UpperCase);
		//LineBussiness.GetLinesBusiness().done(function (data) {
		//    if (data.success) {
		//        datalinebusiness = data.result;
		//    }
		//    else {
		//        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
		//    }
		//});
	}

	//EVENTOS CONTROLES
    /**
     * Eventos de los controles de la clase
     */
	bindEvents() {
		$('#btnRiskTypeLineBusiness').on('click', this.OpenRyskType);
		$('#btnProtections').on('click', this.Protection);
		$("#inputLineBusinessSearch").on("buttonClick", LineBusinessParametrization.GetLinesBusinessById);
		$('#btnSearchAdv').click(this.SearchLineBusiness);
		$('#btnNewLineBusiness').click(this.SaveAll);
		$("#btnSave").click(this.SaveRiskTypeByLineBusiness);
		$("#btnShowAdvanced").on("click", this.ShowAdvanced);
		$("#btnExit").click(this.Exit);
		$("#btnNew").click(LineBusinessParametrization.ClearControls);
		$("#btnDelete").click(this.deleteLineBusiness);
		$("#btnExport").click(this.sendExcelLineBusiness);
		$('#selectRiskTypeLineBusinessMain').on("itemSelected", this.ChangeSelectedRyskType);
		$("#inputLineBusinessCode").focusout(this.GetLineBusinessByDescriptionById);
		$("#inputDescriptionLong").focusout(this.GetLineBusinessByDescriptionById);
	}



	//METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES

    /**
     * Funcion para abrir la vista de tipos de riesgo para asociar al ramo tecnico
     */
	OpenRyskType() {
		$('#ModalRiskTypeLineBusiness').UifModal('showLocal', Resources.Language.MessageRiskType);
		LineBusinessParametrization.loadLinesBusiness();
	}

	ChangeSelectedRyskType() {
		if ($("#selectRiskTypeLineBusinessMain").UifSelect("getSelected") > 0) {
			glbRiskTypeByLineBusiness = [];
			glbRiskTypeByLineBusiness.push($("#selectRiskTypeLineBusinessMain").UifSelect("getSelected"));
		}
	}

    /**
     * Funcion para visualizar la vista de Amparos
     */
	Protection() {
		if (update == true) {
			$.each(listAsignedPeril, function (key, value) {
				var protectionObjectAsg =
					{
						DescriptionLong: this.DescriptionLong,
						Id: this.Id,
					};
				var findObject = function (element, index, array) {
					return element.Id === protectionObjectAsg.Id
				}
				var index = $("#listviewProtectionLineBusiness").UifListView("findIndex", findObject);
				if (index > -1) {
					$("#listviewProtectionLineBusiness").UifListView("deleteItem", index);
					$("#listviewProtectionLineBusinessAssing").UifListView("addItem", protectionObjectAsg);
				}
			});
			$('#ModalProtection').UifModal('showLocal', Resources.Language.LabelProtection);
		}
	}

	deleteLineBusiness() {
		var id = $("#inputLineBusinessCode").val();
		if (id == "") {
			$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectLineBusinessDelete, 'autoclose': true });
		} else {
			var associated = 0;
			
			if ($("#selectedCoInsurance").text() == "" && $("#selectedProtections").text() == "" && $("#selectedClauses").text() == "") {
				associated = 1;
			}
			
			if (associated > 0) {
				$.UifDialog('confirm', { 'message': Resources.Language.ConfirmDelete }, function (result) {
					if (result) {
						LineBusinessRequest.DeleteLineBusiness(id).done(function (data) {
							if (data.success) {
								LineBusinessParametrization.ClearControls();
								$.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
							} else {
								$.UifNotify('show', { 'type': 'danger', 'message': data.result.message, 'autoclose': true });
							}
						});
					}
				});
			} else {
				$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.LineBusinessAssociatedNoDelete, 'autoclose': true });
			}
		}
	}

    /**
     * Funcion para descargar excel
     */
	sendExcelLineBusiness() {
		LineBusinessRequest.GenerateFileToExport().done(function (data) {
			if (data.success) {
				DownloadFile(data.result);
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});
	}

    /**
     * Obtiene y guarda los tipos de riesgo asociados al ramo tecnico
     */
	SaveRiskTypeByLineBusiness() {
		glbRiskTypeByLineBusiness = []
		$.each($("#listViewRiskTypeLineBusiness").UifListView("getData"), function (key, value) {
			glbRiskTypeByLineBusiness.push(value.Id);
		});


		$("#ModalRiskTypeLineBusiness").UifModal('hide');
		if (glbRiskTypeByLineBusiness.length == 1) {
			$('#selectRiskTypeLineBusinessMain').attr("disabled", false);
			$("#selectRiskTypeLineBusinessMain").UifSelect("setSelected", glbRiskTypeByLineBusiness);
		}
		else if (glbRiskTypeByLineBusiness.length > 1) {
			$('#selectRiskTypeLineBusinessMain').attr("disabled", true);
			$("#selectRiskTypeLineBusinessMain").UifSelect("setSelected", null);
		}
	}



    /**
     * Funcion que envia todos los datos asociados al ramo tecnico a la BD
     */
	SaveAll() {
		$("#formLineBusiness").validate();
		if ($("#formLineBusiness").valid()) {
			let linebusinessData = $("#formLineBusiness").serializeObject();
			linebusinessData.Update = update;


			linebusinessData.CoveredRiskTypes = glbRiskTypeByLineBusiness;
			
			LineBusinessRequest.SaveLineBusiness(linebusinessData).done(function (data) {

				if (data.success) {
                    update = true;
                    $("#inputDescriptionLong").attr("tag", $("#inputDescriptionLong").val());
                    $("#inputLineBusinessCode").attr("tag", $("#inputLineBusinessCode").val());
					$("#inputLineBusinessCode").attr("disabled", true);
					$.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
				}
				else {
					$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
				}
			});
		}
	}




	//METODOS PARA CARGAR INFORMACION EN CAMPOS
    /**
     * Funcion para obtener los datos ingresados en los controles
     */
	static getLineModel() {

	}


    /**
     * Funcion para validar y visualizar los datos encontrados en la busqueda
     */
	static showData(result) {
		if (result.length === 0) {
			LineBusinessParametrization.ClearControls();
			$.UifNotify('show',
				{
					'type': 'danger', 'message': Resources.Language.TechicalBranchNotExist, 'autoclose': true
				});
		}
		else if (result.length == 1) {
			result = result[0];
			idCurrentLineBusiness = result.Id;
            $("#inputDescriptionLong").val(result.Description);
            $("#inputDescriptionLong").attr("tag", result.Description);
            $("#inputLineBusinessCode").val(result.Id);
            $("#inputLineBusinessCode").attr("tag", result.Id);
			$('#inputLineBusinessCode').attr("disabled", true);
			$("#inputDescriptionShort").val(result.SmallDescription);
			$("#inputAbbreviation").val(result.TinyDescription);
			$('#btnRiskTypeLineBusiness').attr("disabled", false);
				glbRiskTypeByLineBusiness = result.CoveredRiskTypes;
				$("#selectRiskTypeLineBusinessMain").UifSelect("setSelected", result.CoveredRiskTypes[0]);
			if (result.CoveredRiskTypes.length > 1)
			{
				$('#selectRiskTypeLineBusinessMain').attr("disabled", true);
			} else if (result.CoveredRiskTypes.length == 1) {
				$('#selectRiskTypeLineBusinessMain').attr("disabled", false);
			}
			InsObjAssign = result.InsuredObjects;
			$.each(result.Clauses, function (postion, item) {
				this.Description = this.Name;
			})
			clausesAssign = result.Clauses;
			ProtectionAssign = result.Perils;
			LineBusinessParametrization.countModal();
			update = true;

		}
		else if (result.length > 1) {
			AdvancedSearchLineBusiness.ShowSearchAdv(result);
		}
	}
	static ShowDefaultResults(dataTable) {
		$('#tableResults').UifDataTable('clear');
		$('#tableResults').UifDataTable('addRow', dataTable);
	}


	//-----CARGAR LISTA DE TIPOS DE RIESGO ASOCIADOS AL RAMO TECNICO
	static loadLinesBusiness() {

		var LinesBusinessCoveredRiskType = {};
		if (glbRiskTypeByLineBusiness.length > 0) {
			$("#listViewRiskTypeLineBusiness").UifListView("refresh");
			$.each(glbRiskTypeByLineBusiness, function (key, value) {
				//LineBusinessParametrization.loadRisktype();
				$.each(loadRisktypes, function (key, valueRisk) {
					if (value == valueRisk.Id) {
						LinesBusinessCoveredRiskType =
							{
								IdRiskType: valueRisk.Description,
								Id: valueRisk.Id
							}
						$("#listViewRiskTypeLineBusiness").UifListView("addItem", LinesBusinessCoveredRiskType);
					}
				})
			})

		}
	}

    /**
     * Funcion para mostrar en la vista los resultados encontrados
     * @param {any} dataTable datos encontrados
     */
	static ShowDefaultResults(dataTable) {
		$('#tableResults').UifDataTable('clear');
		$('#tableResults').UifDataTable('addRow', dataTable);
	}



    /**
     * Funcion para abrir la vista de Busquedas Avanzadas
     */
	ShowAdvanced() {
		$('#selectRiskTypeCovered').UifSelect({
			sourceData: loadRisktypes
		});
		$("#listviewSearchPerson").UifListView(
			{
				displayTemplate: "#searchNaturalTemplate",
				selectionType: 'single',
				source: null,
				height: 180
			});
		dropDownSearch.show();
	}

    GetLineBusinessByDescriptionById(item) {
        var inputDescriptionLong = $("#inputDescriptionLong").val();

        if ($("#inputDescriptionLong").attr("tag") == $("#inputDescriptionLong").val()) {
            inputDescriptionLong = "";
        }

		var inputLineBusinessCode = $("#inputLineBusinessCode").val();

        if ((inputLineBusinessCode == "") || ($("#inputLineBusinessCode").attr("tag") == $("#inputLineBusinessCode").val())) {
			inputLineBusinessCode = 0;
        }

        if (!(inputLineBusinessCode == 0 && inputDescriptionLong == "")) {
            LineBusinessRequest.GetLineBusinessByDescriptionById($("#inputDescriptionLong").val(), inputLineBusinessCode).done(function (data) {
                if (!data.success) {
                    $("#" + item.currentTarget.id).val('');
                    $.UifNotify('show', {
                        'type': 'info', 'message': data.result, 'autoclose': true
                    });
                }
            });
        }
	}

	static GetLinesBusinessById(event, selectedItem, id) {
		if ((!selectedItem || !selectedItem.trim() || selectedItem.length < 3) && id == undefined) {
			$.UifNotify('show', {
				'type': 'danger', 'message': Resources.Language.ErrorInputSearchCoverage, 'autoclose': true
			})
			return;
		}
		if (id == undefined) {
			id = 0;
		}
		LineBusinessRequest.GetLineBusinessByDescriptionId(selectedItem, id).done(function (data) {
			if (data.success) {
				LineBusinessParametrization.showData(data.result);
			}
			else {
				$.UifNotify('show', {
					'type': 'info', 'message': data.result, 'autoclose': true
				});
			}
		});

	}

	//METODOS ADICIONALES 
    /**
     * Funcion para limpiar los controles
     */
	static ClearControls() {
        glbRiskTypeByLineBusiness = [];
        $("#inputDescriptionLong").attr("tag", "");
        $("#inputLineBusinessCode").attr("tag", "");
		$("#inputLineBusinessCode").val('');
        $('#inputLineBusinessCode').attr("disabled", false);
        $('#inputLineBusinessCode').focus();
		$("#inputDescriptionLong").val('');
		$("#inputDescriptionShort").val('');
		$("#inputAbbreviation").val('');
		$("#selectRiskTypeLineBusinessMain").UifSelect("setSelected", null);
		$('#selectRiskTypeLineBusinessMain').attr("disabled", false);
		$("#inputLineBusinessSearch").val('');
		$("#listViewRiskTypeLineBusiness").UifListView('clear');
		LineBusinessInsuranceObjectsParametrization.ClearInsuranceObjectsParametrization();
		LineBusinessProtectionsParametrization.ClearProtectionsParametrization();
		LineBusinessClausesParametrization.ClearClausesParametrization();
		$("#selectedProtections").text('');
		$("#selectedCoInsurance").text('');
		$("#selectedClauses").text('');
		ClearValidation('#formLineBusiness');
		update = false;
	}

	newClear() {
		LineBusinessParametrization.ClearControls();
	}
	//------------ACCIONES PRINCIPALES Y COMUNES
	Exit() {
		window.location = rootPath + "Home/Index";
	}
	static countModal() {
		$("#selectedCoInsurance").text("");
		$("#selectedProtections").text("");
		$("#selectedClauses").text("");
		if (InsObjAssign.length > 0) {
			$("#selectedCoInsurance").text(InsObjAssign.length);
		}
		if (ProtectionAssign.length > 0) {
			$("#selectedProtections").text(ProtectionAssign.length);
		}
		if (clausesAssign.length > 0) {
			$("#selectedClauses").text(clausesAssign.length);
		}
	}
}


