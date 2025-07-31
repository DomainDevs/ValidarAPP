var printIndexSelected = null;
$.ajaxSetup({ async: true });
class PrintCoveragesParametrization extends Uif2.Page {
	bindEvents() {
		$("#listViewPrintsCoverage").UifListView({
			displayTemplate: "#tmpForPrints",
			edit: true,
			delete: true,
			deleteCallback: (deferred) => {
				deferred.resolve();
			},
			customEdit: true,
			height: 310
		});
		$("#btnPrints").click(this.btnPrints);
		$("#btnSavePrint").click(this.SavePrintCoverage);
		$("#btnNewPrint").click(this.NewPrint);
		$("#btnModalPrintsSave").click(this.SaveModalPrints);
		$("#btnModalPrintsClose").click(this.CloseModalPrints);
		$('#listViewPrintsCoverage').on('rowEdit', PrintCoveragesParametrization.showEditPrint);
	}
	btnPrints() {
		if ($("#formCoverage").valid()) {
			var namecoverag = $("#inputCoverageName").val();
			if (namecoverag != '') {
				$("#modalPrints").UifModal('showLocal', AppResources.TitleModalImpressionsCoverage + ' ' + namecoverag);
			} else {
				$("#modalPrints").UifModal('showLocal', AppResources.TitleModalImpressionsCoverage);
			}
		}
	}
	static LoadPrintsByCoverageId(data) {
		if (data != null) {
			if (data.length > 0) {
				$("#selectPrints").text("(" + data.length + ")");
				$("#listViewPrintsCoverage").UifListView({
					sourceData: data,
					displayTemplate: "#tmpForPrints",
					edit: true,
					delete: true,
					deleteCallback: (deferred) => {
						deferred.resolve();
					},
					customEdit: true,
					height: 310
				});
			}
		}
	}
	SaveModalPrints() {
		PrintCoveragesParametrization.ClearFormNewPrint();
		$('#modalPrints').UifModal('hide');
		PrintCoveragesParametrization.LoadListPrints();
	}
	CloseModalPrints() {
		PrintCoveragesParametrization.ClearFormNewPrint();
		$('#modalPrints').UifModal('hide');
		PrintCoveragesParametrization.LoadListPrints();
	}
	NewPrint() {
		PrintCoveragesParametrization.ClearFormNewPrint();
	}
	SavePrintCoverage() {
		if ($("#formCoCoverage").valid()) {
			var allPrints = $("#listViewPrintsCoverage").UifListView('getData');
			var newCoverage = {};
			var coveragePrint = $("#formCoCoverage").serializeObject();
			newCoverage.Id = coveragePrint.IdPrint;
			if (coveragePrint.IsAccMinPremiumPrint === "on") {
				newCoverage.IsAccMinPremium = true;
			} else {
				newCoverage.IsAccMinPremium = false;
			}
			if (coveragePrint.IsAssistancePrint === "on") {
				newCoverage.IsAssistance = true;
			} else {
				newCoverage.IsAssistance = false;
			}
			if (coveragePrint.IsImpressionPrint === "on") {
				newCoverage.IsImpression = true;
			} else {
				newCoverage.IsImpression = false;
			}
			newCoverage.Description = coveragePrint.ImpressionNamePrint;
			newCoverage.ImpressionValue = coveragePrint.ImpressionValuePrint;
			if (coveragePrint.IdPrint == "") {
				if (allPrints.filter(x => x.Description.toLowerCase() == newCoverage.Description.toLowerCase()).length == 0) {
					if (printIndexSelected == null) {
						newCoverage.Status = ParametrizationStatus.Create;
						$("#listViewPrintsCoverage").UifListView("addItem", newCoverage);
					} else {
						newCoverage.Status = ParametrizationStatus.Create;
						$('#listViewPrintsCoverage').UifListView("editItem", printIndexSelected, newCoverage);
					}
					PrintCoveragesParametrization.LoadListPrints();
					PrintCoveragesParametrization.ClearFormNewPrint();
				} else {
					$.UifNotify("show", { 'type': "info", 'message': AppResources.ValidationRepeatedImpressionsCoverage, 'autoclose': true });
				}
			} else {
				if (allPrints.filter(x => x.Description.toLowerCase() == newCoverage.Description.toLowerCase() && x.Id != newCoverage.Id).length == 0) {
					newCoverage.Status = ParametrizationStatus.Update;
					$.each($("#listViewPrintsCoverage").UifListView('getData'), function (key, value) {
						if (value.Id == newCoverage.Id) {
							$('#listViewPrintsCoverage').UifListView("editItem", key, newCoverage);
						}
					});
					PrintCoveragesParametrization.LoadListPrints();
					PrintCoveragesParametrization.ClearFormNewPrint();
				} else {
					$.UifNotify("show", { 'type': "info", 'message': AppResources.ValidationRepeatedImpressionsCoverage, 'autoclose': true });
				}
			}
		}
	}
	static ClearFormNewPrint() {
		printIndexSelected = null;
		$("#IsImpressionPrint").prop("checked", false);
		//$("#IsAccMinPremiumPrint").prop("checked", false);
		//$("#IsAssistancePrint").prop("checked", false);
		$("#ImpressionNamePrint").val("");
		$("#ImpressionValuePrint").val("");
		$("#IdPrint").val("");
		ClearValidation("#formCoCoverage");
	}
	static ClearListPrints() {
		$('#listViewPrintsCoverage').UifListView("clear");
	}
	static LoadListPrints() {
		var allPrints = $("#listViewPrintsCoverage").UifListView('getData');
		if (allPrints != null && allPrints != undefined) {
			if (allPrints.length > 0) {
				$("#selectPrints").text("(" + allPrints.length + ")");
			} else {
				$("#selectPrints").text("");
			}
		} else {
			$("#selectPrints").text("");
		}

	}
	static showEditPrint(event, result, index) {
		PrintCoveragesParametrization.ClearFormNewPrint();
		printIndexSelected = index;
		$("#IdPrint").val(result.Id);
		$("#IsImpressionPrint").prop("checked", result.IsImpression);
		//$("#IsAccMinPremiumPrint").prop("checked", result.IsAccMinPremium);
		//$("#IsAssistancePrint").prop("checked", result.IsAssistance);
		$("#ImpressionNamePrint").val(result.Description);
		$("#ImpressionValuePrint").val(result.ImpressionValue);
	}
}