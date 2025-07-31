$(() => {
	new ParametrizationVehicleBody();
	new ParametrizationVehicleUse();
});

var glbVehicleBody = [];
var glbVehicleBodyDelete = [];
var vehicleBody = {};
var vehicleBodyIndex = null;
var inputSearch = "";
var dropDownSearchAdvVehicleBody = null;

class VehicleBody {
	static GetVehicleBodies() {
		return $.ajax({
			type: 'GET',
			url: rootPath + 'Parametrization/VehicleBody/GetVehicleBodies',
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}

	static Save(vehicleBodies) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/VehicleBody/Save',
			data: JSON.stringify({ vehicleBodiesView: vehicleBodies }),
			dataType: "json",
			contentType: "application/json; charset=utf-8"
		});
	}

	static GenerateFileVehicleBodyToExport() {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/VehicleBody/GenerateFileVehicleBodyToExport',
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}

	static GenerateFileVehicleUseToExport(vehicleBodyView) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/VehicleBody/GenerateFileVehicleUseToExport',
			data: JSON.stringify({ vehicleBody: vehicleBodyView }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}
}


class ParametrizationVehicleBody extends Uif2.Page {
	getInitialState() {
		$("input[type=text]").TextTransform(ValidatorType.UpperCase);
		VehicleBody.GetVehicleBodies().done(function (data) {
			if (data.success) {
				ParametrizationVehicleBody.LoadListViewVehicleBodies(data);
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});

		$("#listVehicleBody").UifListView({
			displayTemplate: "#VehicleBodyTemplate",
			edit: true,
			delete: true,
			customAdd: true,
			customEdit: true,
			height: 300,
			deleteCallback: this.DeleteItemVehicleBody
		});

		dropDownSearchAdvVehicleBody = uif2.dropDown({
			source: rootPath + 'Parametrization/VehicleBody/AdvancedSearch',
			element: '#btnSearchAdvVehicleBody',
			align: 'right',
			width: 550,
			height: 551,
			loadedCallback: function () { }
		});

		$("#listViewSearchAdv").UifListView({
			displayTemplate: '#VehicleBodyTemplateAdv',
			selectionType: "single",
			height: 450
		});
	}

	bindEvents() {
		$("#btnAsociateBody").click(this.BtnAsociateBody);
		$("#btnNewVehicleBody").click(ParametrizationVehicleBody.Clear);
		$("#btnAcceptVehicleUse").click(this.BtnAcceptVehicleUse);
		$("#btnAcceptVehicleBody").click(ParametrizationVehicleBody.BtnAcceptVehicleBody);
		$('#inputSearchVehicleBody').on('buttonClick', this.SearchVehicleBodies);
		$('#inputSearchVehicleBody').on('itemSelected', this.SearchVehicleBodies);
		$("#listVehicleBody").on('rowEdit', ParametrizationVehicleBody.ShowData);
		$("#listVehicleBody").on('rowDelete', this.DeleteItemVehicleBody);
		$("#btnSearchAdvVehicleBody").on("click", this.SearchAdvVehicleBody);
		$("#btnSaveVehicleBody").on("click", this.SaveVehicleBodies);
		$("#btnExport").on("click", this.sendExcelVehicleBody);
		$("#btnExportVehicleUse").on("click", this.sendExcelVehicleUse);
		$("#btnExit").on("click", this.redirectIndex);

		glbVehicleBodyDelete = [];
	}

	sendExcelVehicleBody() {
		VehicleBody.GenerateFileVehicleBodyToExport().done(function (data) {
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

	sendExcelVehicleUse() {
		ParametrizationVehicleBody.ConstructVehicleBody();
        VehicleBody.GenerateFileVehicleUseToExport(vehicleBody).done(function (data) {
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

	static ConstructMessageArray(title, array) {
		var arr = [];
		if (array == null) {
			return null;
		}
		if (array.length == 0) {
			return null;
		}
		var messageResult = title + "<br />";
		messageResult += array.join("<br />");
		return messageResult;
	}

	SaveVehicleBodies() {
		
		var vehicleBodies = $("#listVehicleBody").UifListView('getData');
		glbVehicleBodyDelete.forEach(function (item) {
			vehicleBodies.push(item);
		});
		VehicleBody.Save(vehicleBodies)
			.done(function (data) {
				if (data.success) {
					ParametrizationVehicleBody.Clear();
					glbVehicleBodyDelete = [];
					var messageResult = "";

					if (data.result.messageCreated != null) {
						messageResult += data.result.messageCreated + "<br />";
					}

					if (data.result.messageUpdated != null) {
						messageResult += data.result.messageUpdated + "<br />";
					}

					if (data.result.messageDeleted != null) {
						messageResult += data.result.messageDeleted + "<br />";
					}

					if (messageResult !== "") {
						$.UifNotify('show', { 'type': 'info', 'message': messageResult, 'autoclose': true });
					}

					var messageErrorCreated = ParametrizationVehicleBody.ConstructMessageArray(Resources.Language.ErrorCreated, data.result.errorCreated);
					var messageErrorUpdated = ParametrizationVehicleBody.ConstructMessageArray(Resources.Language.ErrorUpdated, data.result.errorUpdated);
					var messageErrorDeleted = ParametrizationVehicleBody.ConstructMessageArray(Resources.Language.ErrorDeleted, data.result.errorDeleted);
					if (messageErrorCreated != null) {
						$.UifNotify('show', { 'type': 'danger', 'message': messageErrorCreated, 'autoclose': true });
					}
					if (messageErrorUpdated != null) {
						$.UifNotify('show', { 'type': 'danger', 'message': messageErrorUpdated, 'autoclose': true });
					}
					if (messageErrorDeleted != null) {
						$.UifNotify('show', { 'type': 'danger', 'message': messageErrorDeleted, 'autoclose': true });
					}

					VehicleBody.GetVehicleBodies().done(function (data) {
						if (data.success) {
							ParametrizationVehicleBody.LoadListViewVehicleBodies(data);
						}
						else {
							$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
						}
					});
				}
				else {
					$.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
				}
			})
			.fail(function (jqXHR, textStatus, errorThrown) {
				$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveVehicleBodies, 'autoclose': true })
			});
	}

	SearchAdvVehicleBody() {
		ParametrizationVehicleBody.ShowSearchAdv();
	}

	BtnAsociateBody() {
		ParametrizationVehicleUse.OpenVehicleUse($("#inputShortDescription").val());
	}

	SearchVehicleBodies(event, selectedItem) {
		inputSearch = selectedItem;
		ParametrizationVehicleBody.Clear();

		ParametrizationVehicleBody.Get(0, inputSearch);
	}

	static OkSearchVehicleBodyAdv() {
		//$("#listViewSearchAdv").UifListView({ displayTemplate: '#VehicleBodyTemplateAdv', selectionType: "single", height: 400 });
		let data = $("#listViewSearchAdv").UifListView("getSelected");
		if (data.length == 1) {
			ParametrizationVehicleBody.ShowData(null, data, data.key);
		}
		ParametrizationVehicleBody.HideSearchAdv();
	}

	static CancelSearchAdv() {
		ParametrizationVehicleBody.HideSearchAdv();
	}

	DeleteItemVehicleBody(event, data, index) {
		event.resolve();
		var vehicleBodyList = $("#listVehicleBody").UifListView('getData');
		$.each(vehicleBodyList, function (index, value) {
			if (value.BodyCode !== "" && value.BodyCode !== undefined) {
				if (this.BodyCode == data.BodyCode && this.ShortDescription == data.ShortDescription) {
					value.State = ParametrizationStatus.Delete;
					value.allowEdit = false;
					value.allowDelete = false;
					$("#listVehicleBody").UifListView('addItem', this);
				}
			}
		});
		ParametrizationVehicleBody.Clear();
	}

	BtnAcceptVehicleUse() {
		vehicleBody.VehicleUses = ParametrizationVehicleUse.SaveUses();
	}

	static ConstructVehicleBody() {
		vehicleBody.ShortDescription = $("#inputShortDescription").val();
		//vehicleBody.ShortDescriptino = $("#inputShortDescriptino").val();
		vehicleBody.BodyCode = $("#inputCode").val();
		vehicleBody.IsTruck = $('#checkIsTruck').is(':checked');
		vehicleBody.IsActive = $('#checkIsActive').is(':checked');
		if (vehicleBody.BodyCode > 0) {
			vehicleBody.State = 3;
		}
		else {
			vehicleBody.State = 2;
		}
	}

	static BtnAcceptVehicleBody() {
		$("#formVehicleBody").validate();
		if ($("#formVehicleBody").valid()) {
			ParametrizationVehicleBody.ConstructVehicleBody();
	
			var mensage = Resources.Language.Assignmenterrorofuse;
			var uses = [];

			uses = $("#tableVehicleUse").UifDataTable("getSelected");
			if (uses == null) {
				$.UifNotify('show', {
					'type': 'danger',
					'message': mensage,
					'autoclose': false
				});
			} else {
			if (vehicleBodyIndex == null) {
				var lista = $("#listVehicleBody").UifListView('getData');
				var ifExist = lista.filter(function (item) {
					return item.ShortDescription.toUpperCase() == vehicleBody.ShortDescription.toUpperCase();
				});
				if (ifExist.length > 0) {
					$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExitsVehicleBodyName, 'autoclose': true });
				}
				else {
					vehicleBody.State = ParametrizationStatus.Create;
					$("#listVehicleBody").UifListView("addItem", vehicleBody);
				}
			}
			else {
				vehicleBody.State = ParametrizationStatus.Update;
				$("#listVehicleBody").UifListView('editItem', vehicleBodyIndex, vehicleBody);
			}
			ParametrizationVehicleBody.Clear();
		}
		}
		}

	static Clear() {
		$("#inputCode").val(null);
        $("#inputShortDescription").val(null);
        $("#inputShortDescription").focus();
		//$("#inputShortDescriptino").val(null);
		$('#checkIsTruck').attr('checked', false);
		$('#checkIsActive').attr('checked', false);

		ParametrizationVehicleUse.ClearUses();
		vehicleBody = {};
		vehicleBodyIndex = null;
		//glbVehicleBodyDelete = [];
	}

	static Get(id, inputSearch) {
		var find = false;
		var data = [];
		var search = $("#listVehicleBody").UifListView('getData');
		if (id == 0) {
			$.each(search, function (key, value) {
				if (value.ShortDescription.toLowerCase().sistranReplaceAccentMark().includes(inputSearch.toLowerCase().sistranReplaceAccentMark())) {
					value.key = key;
					data.push(value);
					find = true;
				}
			});
		}
		else {
			$.each(search, function (key, value) {
				if (value.BodyCode == id) {
					vehicleBodyIndex = key;
					value.key = key;
					data.push(value);
					find = true;
				}
			});
		}
		if (find === false) {
			$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.VehicleBodyNotFound, 'autoclose': true });
		}
		else {
			if (data.length === 1) {
				ParametrizationVehicleBody.ShowData(null, data, data.key);
			}
			else {
				ParametrizationVehicleBody.ShowSearchAdv(data);
			}
		}
	}

	static LoadListViewVehicleBodies(data) {
		if (data.success) {
			// Limpiar ListView
			$("#listVehicleBody").UifListView("clear");

			$.each(data.result, function (key, value) {
				var tmpVehicleBody = {
					BodyCode: this.BodyCode,
					ShortDescription: this.ShortDescription,
					IsTruck: this.IsTruck,
					IsActive: this.IsActive,
					VehicleUses: this.VehicleUses,
					State: ParametrizationStatus.Original
				};
				$("#listVehicleBody").UifListView("addItem", tmpVehicleBody);
			});
		}
	}

	static ShowData(event, result, index) {
		ParametrizationVehicleBody.Clear();
		if (result.length == 1) {
			index = result[0].key;
			result = result[0];
		}
		if (result.BodyCode != undefined) {
			vehicleBodyIndex = index;
			$("#inputCode").val(result.BodyCode);
			$("#inputShortDescription").val(result.ShortDescription);
			//$("#inputShortDescriptino").val(result.ShortDescriptino);

			$("#checkIsTruck").prop("checked", result.IsTruck);
			$("#checkIsActive").prop("checked", result.IsActive);
			vehicleBody.VehicleUses = result.VehicleUses;
			ParametrizationVehicleUse.SelectUses(result.VehicleUses);
		}
	}

	static ShowSearchAdv(data) {
		$("#listViewSearchAdv").UifListView({ displayTemplate: '#VehicleBodyTemplateAdv', selectionType: "single", height: 450 });
		$("#listViewSearchAdv").UifListView("clear");
		if (data) {
			data.forEach(item => {
				$("#listViewSearchAdv").UifListView("addItem", item);
			});
		}
		dropDownSearchAdvVehicleBody.show();
		$("#btnOkSearchVehicleBodyAdv").off("click");
		$("#btnCancelVehicleBodyAdv").off("click");
		$("#btnOkSearchVehicleBodyAdv").on("click", this.OkSearchVehicleBodyAdv);
		$("#btnCancelVehicleBodyAdv").on("click", this.CancelSearchAdv)
	}

	static HideSearchAdv() {
		dropDownSearchAdvVehicleBody.hide();
	}
	 redirectIndex() {
		window.location = rootPath + "Home/Index";
	}
}
