$.ajaxSetup({ async: false });
var listScriptsGeneral = [];
var listScriptRisk = [];
var listScriptCoverage = [];
var CurrentIdScripts = 0;
$(document).ready(function () {
    EventScripts();
});

function EventScripts() {
    //Accion al seleccionar el tipo de riesgo (Script) 
    $("#selectModalRiskTypeScript").on("itemSelected", function (event, selectedItem) {
        if (ProductRules.ProductCoveredRisks != null && CurrentIdScripts != 0 && $("#inputModalRiskInitialScriptPackage").val()=="") {
            $.each(ProductRules.ProductCoveredRisks, function (key, item) {
                if (item.Id == CurrentIdScripts) {
                    if (item.ScriptId != null) {
                        item.ScriptId = null;
                    }
                    return;
                }
            });
        }
		if (selectedItem.Id > 0) {
			if (listScriptRisk.length>0) {
				$('#inputModalRiskInitialScriptPackage').UifInputSearch('disabled', false);
			}
            $("#selectModalRiskInitialScriptPackage").prop("disabled", false);
            SetProductCoveredRisksScripts();
            DisableSelectScriptsCoverage();

        }
        else {
            CurrentIdScripts = 0;
			$('#inputModalRiskInitialScriptPackage').val('');
			$('#inputModalRiskInitialScriptPackage').UifInputSearch('disabled', true);
            $("#tableNoneNew").UifDataTable("clear");
        }

    });

    //Accion validar y deshabilitar acciones coberturas (guiones)
	$('#tableNoneNew').on('click', 'tr', function () {
		setTimeout(DisableSelectScriptsCoverage(), 300);
	});

    //Accion al seleccionar todas las coberturas (guiones)
    $('#tableNoneNew').on('selectAll', function (event) {
        DisableSelectScriptsCoverage();
	});

    //Accion al seleccionar un item de coberturas (guiones)
    $('#tableNoneNew').on('rowSelected', function (event, data) {
		DisableSelectScriptsCoverage();
		if (listScriptCoverage.length > 0) {
			var result = [];
			$.each(listScriptCoverage, function (key, value) {
				if (value.ScriptId == data.InitialBusinessScriptsId) {
					result.push(value);
				}
			});
			if (result.length>0) {
				$("#inputModalCoverageInitialScriptPackage").data("Object", result[0]);
				$("#inputModalCoverageInitialScriptPackage").val(result[0].Description);
			}
		}
	});

    //Accion del boton asignar guiones
    $('#btnModalAssignScript').on('click', function () {
        AssignScriptsCoverages();
    });
}

//Llena la tabla de coberturas (Guiones)
function LoadTableCoveragesScripts() {
    var listCoverages = [];
    $("#tableNoneNew").UifDataTable('clear');

    if (ProductRules.ProductCoveredRisks != null) {
        $.each(ProductRules.ProductCoveredRisks, function (keyCoveredRisks, CoveredRisks) {
            if (CoveredRisks.GroupCoverages != null) {
                $.each(CoveredRisks.GroupCoverages, function (keyGroupCoverages, GroupCoverage) {
                    if (GroupCoverage.RiskTypeId == $("#selectModalRiskTypeScript").val() && GroupCoverage.StatusTypeService !== 4) {
                        if (GroupCoverage.InsuredObjects != null) {
                            $.each(GroupCoverage.InsuredObjects, function (keyInsuredObjects, InsuredObject) {
                                if (InsuredObject.StatusTypeService !== 4) {
                                    if (InsuredObject.Coverages != null) {
                                        $.each(InsuredObject.Coverages, function (keyCoverages, Coverage) {
                                            var ScriptSetName = "";
                                            if (Coverage.Coverage.ScriptId != null) {
                                                var filter = [];
                                                $.each(listScriptCoverage, function (key, value) {
                                                    if (value.ScriptId == Coverage.Coverage.ScriptId) {
                                                        filter.push(value);
                                                    }
                                                });
                                                if (filter.length > 0) {
                                                    ScriptSetName = filter[0].Description;
                                                }
                                            }
                                            listCoverages.push({
                                                GroupCoverageId: GroupCoverage.Id,
                                                CoverageId: Coverage.Coverage.Id,
                                                GroupCoverage: GroupCoverage.Description,
                                                LineBusiness: Coverage.Coverage.LineBusinessDescription == null ? '' : Coverage.Coverage.LineBusinessDescription,
                                                SubLineBusiness: Coverage.Coverage.SubLineBusinessDescription == null ? '' : Coverage.Coverage.SubLineBusinessDescription,
                                                Amparo: Coverage.Coverage.Description,
                                                ObjetoSeguro: InsuredObject.Description,
                                                InitialBusinessScriptsId: Coverage.Coverage.ScriptId == null ? '' : Coverage.Coverage.ScriptId,
                                                InitialBusinessScripts: ScriptSetName
                                            });
                                        });
                                    }
                                }
                            })
                        }
                    }
                });
            }
        });
    }

    $("#tableNoneNew").UifDataTable({ sourceData: listCoverages });
}

//Llenado combos
//Carga los combos de los guiones   
var GetAllScriptByLevelId = function () {

    ScriptRequestT.GetAllScriptByLevelId().done(function (data) {
		if (data.success) {
			$.each(data.result, function (index, value) {
				if (value.LevelId == Level.General) {
					listScriptsGeneral.push(this);
				} else if (value.LevelId == Level.Risk) {
					listScriptRisk.push(this);
				} else if (value.LevelId == Level.Coverage) {
					listScriptCoverage.push(this);
				}
			});
			if (listScriptsGeneral.length > 0) {
				$('#inputModalGeneralInitialScriptPackage').UifInputSearch('disabled', false);
			}
			else {
				$('#inputModalGeneralInitialScriptPackage').UifInputSearch('disabled', true);
			}
			if (listScriptRisk.length > 0) {
				$('#inputModalRiskInitialScriptPackage').UifInputSearch('disabled', false);
			}
			else {
				$('#inputModalRiskInitialScriptPackage').UifInputSearch('disabled', true);
			}
			if (listScriptCoverage.length > 0) {
				$('#inputModalCoverageInitialScriptPackage').UifInputSearch('disabled', false);
			}
			else {
				$('#inputModalCoverageInitialScriptPackage').UifInputSearch('disabled', true);
			}
			if (ProductRules.ScriptId != null) {
				var filter = [];
				$.each(listScriptsGeneral, function (key, value) {
					if (value.ScriptId == ProductRules.ScriptId) {
						filter.push(value);
					}
				});
				if (filter.length > 0) {
					$("#inputModalGeneralInitialScriptPackage").data("Object", filter[0]);
					$("#inputModalGeneralInitialScriptPackage").val(filter[0].Description);
				}
			}
			$("#selectModalRiskInitialScriptPackage").attr("disabled", "disabled");
			$("#selectModalCoverageInitialScriptPackage").attr("disabled", "disabled");
			$("#btnModalAssignScript").attr("disabled", "disabled");
		}
	});
}

//Asigna las guiones a las coberturas
function AssignScriptsCoverages() {
		var dataSelected = $('#tableNoneNew').UifDataTable('getSelected');

		if (dataSelected != null) {
			if (ProductRules.ProductCoveredRisks != null) {
				$.each(ProductRules.ProductCoveredRisks, function (keyCoveredRisk, CoveredRisk) {
					if (CoveredRisk.Id == $("#selectModalRiskTypeScript").val()) {
						if (CoveredRisk.GroupCoverages != null) {
							$.each(CoveredRisk.GroupCoverages, function (keyGroupCoverages, GroupCoverage) {
                                if (GroupCoverage.RiskTypeId == $("#selectModalRiskTypeScript").val() && GroupCoverage.StatusTypeService !== 4) {

                                    if (GroupCoverage.InsuredObjects != null) {
                                        $.each(GroupCoverage.InsuredObjects, function (keyInsuredObjects, InsuredObject) {
                                            if (InsuredObject.Coverages != null && InsuredObject.StatusTypeService !== 4) {
                                                $.each(InsuredObject.Coverages, function (keyCoverages, Coverage) {
                                                    $.each(dataSelected, function (keyCoverageSelected, CoverageSelected) {
                                                        if (CoverageSelected.GroupCoverageId == GroupCoverage.Id) {
                                                            if (CoverageSelected.CoverageId == Coverage.Coverage.Id) {
                                                                var ScriptSetId = null;
                                                                if ($("#inputModalCoverageInitialScriptPackage").val() != '' && $("#inputModalCoverageInitialScriptPackage").val() == $("#inputModalCoverageInitialScriptPackage").data("Object").Description) {
                                                                    ScriptSetId = $("#inputModalCoverageInitialScriptPackage").data("Object").ScriptId;
                                                                }
                                                                ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.ScriptId = ScriptSetId;
                                                                if (ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.StatusTypeService === 1) {
                                                                    ProductRules.ProductCoveredRisks[keyCoveredRisk].GroupCoverages[keyGroupCoverages].InsuredObjects[keyInsuredObjects].Coverages[keyCoverages].Coverage.StatusTypeService = 3;
                                                                }   
                                                            }
                                                        }
                                                    });
                                                })
                                            }
                                        });
                                    }

								}
							});
						}
					}
				});
				LoadTableCoveragesScripts();
			}
		}
	
}

//Desabilita los combos de guiones de las coberturas
function DisableSelectScriptsCoverage() {
    var data = $('#tableNoneNew').UifDataTable('getSelected');
	if (data != null && listScriptCoverage.length>0) {
		$('#inputModalCoverageInitialScriptPackage').UifInputSearch('disabled', false);
        $("#btnModalAssignScript").removeAttr("disabled");
	} else {
		$('#inputModalCoverageInitialScriptPackage').UifInputSearch('disabled', true);
		$('#inputModalCoverageInitialScriptPackage').val('');
        $("#btnModalAssignScript").attr("disabled", "disabled");
    }
}