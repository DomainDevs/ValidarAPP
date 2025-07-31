var dropDownSearchAdvVehicleVersion = null;
$.ajaxSetup({ async: true });
class VehiculeVersionSearchParametrization extends Uif2.Page {
	getInitialState() {
		
	}
	bindEvents() {
		$('#btnAdvSearch').click(VehiculeVersionSearchParametrization.ShowAdvancedSearch)
		dropDownSearchAdvVehicleVersion = uif2.dropDown({
			source: rootPath + 'Parametrization/VehicleVersion/VehicleVersionSearch',
			element: '#btnAdvSearch',
			align: 'right',
			width: 550,
			height: 551,
			loadedCallback: VehiculeVersionSearchParametrization.LoadbindignsSearchAdvanzed
		});
	}
	static LoadbindignsSearchAdvanzed() {
		$('#btnSearchAdvanzed').click(VehiculeVersionSearchParametrization.SearchAdvancedSearch)
		$('#VehicleMakeServiceQueryModelSearch').on("itemSelected", VehiculeVersionSearchParametrization.GetModelByMakeSearch);
		$('#btnCancelSearchAdvVehicle').click(VehiculeVersionSearchParametrization.CancelSearchAdvanzed)
		$('#btnOkSearchAdvVehicle').click(VehiculeVersionSearchParametrization.AcceptSearchAdvanzed)
		$("input[type=text]").TextTransform(ValidatorType.UpperCase);
		$("#lvSearchAdvVehicleVersion").UifListView({ source: null, displayTemplate: "#advancedSearchTemplate", selectionType: 'single', height: 180 });
	}
	static GetModelByMakeSearch(event, selectedItem) {
		if (selectedItem.Id) {
			VehiculeVersionParametrization.GetModelByMake(selectedItem.Id).done(function (resultModel) {
				if (resultModel.success) {
					$("#VehicleModelServiceQueryModelSearch").UifSelect({ sourceData: resultModel.result});
				}
			});
		}
		else {
			$("#VehicleModelServiceQueryModelSearch").UifSelect();
		}
	}
	static CancelSearchAdvanzed() {
		dropDownSearchAdvVehicleVersion.hide();
	}
	static AcceptSearchAdvanzed() {
		
		var Selected = $("#lvSearchAdvVehicleVersion").UifListView("getSelected");
		if (Selected.length > 0) {
			dropDownSearchAdvVehicleVersion.hide();
            VehiculeVersionParametrization.setDataForm(Selected[0]);
            $('#inputVehicleVersion').val(null);
		}
		else {
			$.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
		}
	}
	static SearchAdvancedSearch() {
		var form = $('#frmAdvanzedSearch').serializeObject();
		if (form.VehicleMakeServiceQueryModel != "" && form.VehicleModelServiceQueryModel!="") {
			request('Parametrization/VehicleVersion/VehicleVersionSearchAdvanzed', JSON.stringify({ VehicleVersionViewModel: form }), 'POST', AppResources.ErrorSaveVehicleVersion, VehiculeVersionSearchParametrization.successSearch);
		}
		else {
			$.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorFilterNotSelectedVehicleVersion });
		}
	}
	static successSearch(data) {
		VehiculeVersionSearchParametrization.LoadResultsListView(data);
	}
	static LoadResultsListView(result) {
		$("#lvSearchAdvVehicleVersion").UifListView("refresh");
		$.each(result, function (index, val) {
			$("#lvSearchAdvVehicleVersion").UifListView("addItem", val);
		});
	}
	static ShowAdvancedSearch() {
		dropDownSearchAdvVehicleVersion.show();
		VehiculeVersionSearchParametrization.CleanAdvancedSearch();
	}
	static CleanAdvancedSearch() {
		$('#VehicleMakeServiceQueryModelSearch').UifSelect('setSelected', null);
		$('#VehicleModelServiceQueryModelSearch').UifSelect('setSelected', null);
		$('#DescriptionSearch').val('');
		$("#lvSearchAdvVehicleVersion").UifListView({ source: null, displayTemplate: "#advancedSearchTemplate", selectionType: 'single', height: 180 });
	}
}