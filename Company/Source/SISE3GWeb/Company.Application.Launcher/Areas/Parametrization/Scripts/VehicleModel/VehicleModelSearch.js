var dropDownSearchAdvVehicleModel = null;
$.ajaxSetup({ async: true });
class VehiculeModelParametrization extends Uif2.Page {
    getInitialState() {
    }
    bindEvents() {
        $('#btnShowAdvancedVehicleModel').click(VehiculeModelParametrization.ShowAdvancedSearch)
        dropDownSearchAdvVehicleModel = uif2.dropDown({
            source: rootPath + 'Parametrization/VehicleModel/VehicleModelSearch',
            element: '#btnShowAdvancedVehicleModel',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: VehiculeModelParametrization.LoadbindignsSearchAdvanzed
        });
    }
    static ShowAdvancedSearch() {
        dropDownSearchAdvVehicleModel.show();
        VehiculeModelParametrization.CleanAdvancedSearch();
    }
    static LoadResultsListView(result) {
        console.log(result);
        $("#lvSearchAdvVehicleModel").UifListView("refresh");
        $.each(result, function (index, val) {
            $("#lvSearchAdvVehicleModel").UifListView("addItem", val);
        });
    }
    static successSearch(data) {
        VehiculeModelParametrization.LoadResultsListView(data);
    }
    static SearchAdvancedSearch() {
        var form = $('#frmAdvanzedSearch').serializeObject();
        if (form.DescriptionModelSearch === "" && form.MakeSearchId == "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelSearchAdvancedCriteria });
        } else {
            var validationSearch = true;
            if (form.MakeSearchId == "") {
                if (form.DescriptionModelSearch.length > 1) { validationSearch = true; } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelMinimumTwoCharacters });
                }
            }
            if (validationSearch) {
                form.MakeId_Id = form.MakeSearchId;
                form.DescriptionModel = form.DescriptionModelSearch;
                request('Parametrization/VehicleModel/ModelVehicleSearchAdv', JSON.stringify({ vehicleModelViewModel: form }), 'POST', AppResources.ErrorSaveVehicleVersion, VehiculeModelParametrization.successSearch);
            }
        }
    }
    static LoadbindignsSearchAdvanzed() {
        $('#btnSearchAdvVh').on('click', VehiculeModelParametrization.SearchAdvancedSearch);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#btnOkSearchAdvModel').on('click', VehiculeModelParametrization.AcceptSearchAdvanzed);
        $("#lvSearchAdvVehicleModel").UifListView({ source: null, displayTemplate: "#advancedSearchTemplate", selectionType: 'single', height: 300 });
        $('#btnCancelSearchAdvModel').click(VehiculeModelParametrization.CancelSearchAdvanzed);
    }
    static AcceptSearchAdvanzed() {
        var Selected = $("#lvSearchAdvVehicleModel").UifListView("getSelected");
        if (Selected.length > 0) {
            dropDownSearchAdvVehicleModel.hide();
            ModelParametrization.setDataForm(Selected[0]);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
        }
    }
    static CleanAdvancedSearch() {
        $('#MakeSearchId').UifSelect('setSelected', null);
        $('#DescriptionModelSearch').val('');
        $("#lvSearchAdvVehicleModel").UifListView({ source: null, displayTemplate: "#advancedSearchTemplate", selectionType: 'single', height: 300 });
    }
    static successSearch(data) {
        VehiculeModelParametrization.LoadResultsListView(data);
    }
    static CancelSearchAdvanzed() {
        VehiculeModelParametrization.CleanAdvancedSearch();
        dropDownSearchAdvVehicleModel.hide();
    }
}

