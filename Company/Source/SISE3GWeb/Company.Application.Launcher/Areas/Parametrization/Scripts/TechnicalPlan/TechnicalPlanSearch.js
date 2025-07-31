$.ajaxSetup({ async: true });
var StatusEnum = {
	Original: 1,
	Create: 2,
	Update: 3,
	Delete: 4,
	Error: 5
};
var dropDownSearch;
var technicalPlan = {};
var paramData = {};

class TechnicalPlanSearch extends Uif2.Page {

	getInitialState() {
			dropDownSearch = uif2.dropDown({
			source: rootPath + 'Parametrization/TechnicalPlan/TechnicalPlanAdvSearch',
			element: '#BtnSearchAdvTechnicalPlan',
			align: 'right',
			width: 550,
			height: 551,
			container: "#main",
			loadedCallback: TechnicalPlanSearch.ComponentLoadedCallback
		});		
		$("#SearchTechnicalPlan").TextTransform(ValidatorType.UpperCase);
		$("#SearchShortDescription").TextTransform(ValidatorType.UpperCase);
	}
	bindEvents() {		
		$('#SearchTechnicalPlan').on('buttonClick', TechnicalPlanSearch.searchTechnicalPlan);				
	}

	static itemSelected() {
		var Item = $("#LvSearchAdvTechnicalPlan").UifListView("getSelected");
		if (Item != "") {
			dropDownSearch.hide();
			paramData = {
				Id: Item[0].TechnicalPlanId,
				Description: Item[0].TechnicalPlanDescription,
				SmallDescription: Item[0].TechnicalPlanShortDescription,
				CurrentFrom: Item[0].TechnicalPlanCurrentFrom,
				CurrentTo: Item[0].TechnicalPlanCurrentTo,
				CoveredRiskType: { Id: Item[0].CoveredRiskTypeId, SmallDescription: Item[0].CoveredRiskTypeDescription }				
			}			
			TechnicalPlan.SetDataControls(paramData);
		} else {
			$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorListElement, 'autoclose': true });

		}		
	}

	static searchTechnicalPlan() {
		var inputObject = $('#SearchTechnicalPlan').val();
		if (inputObject.length < 3) {
			$.UifNotify('show', { type: 'warning', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
		}
		else {
			request('Parametrization/TechnicalPlan/AdvancedSearchTechnicalPlan', { "description": inputObject, "coveredRiskType": -1 }, 'GET', AppResources.ErrorExistInsuredObject, TechnicalPlanSearch.loadResultSimpleSearch)
		}
		$('#SearchTechnicalPlan').val("");
	}
	static searchAdvancedTechnicalPlan() {
		$("#FormSearchTechnicalPlan").validate();
		if ($("#FormSearchTechnicalPlan").valid()) {
			$("#LvSearchAdvTechnicalPlan").UifListView("refresh");
			var tPlanAdvancedSearch = {
				description: $('#SearchShortDescription').val(),
				coveredRiskType: parseInt($("#SearchCoveredRiskType").UifSelect('getSelected'))
			};
			if (isNaN(tPlanAdvancedSearch.coveredRiskType)) { tPlanAdvancedSearch.coveredRiskType = 0 };
			request('Parametrization/TechnicalPlan/AdvancedSearchTechnicalPlan', { "description": tPlanAdvancedSearch.description, "coveredRiskType": tPlanAdvancedSearch.coveredRiskType }, 'GET', AppResources.ErrorExistInsuredObject, TechnicalPlanSearch.loadResultAdvancedSearch)			
		}
	}

	static loadResultSimpleSearch(data) {
		TechnicalPlanSearch.loadResultData(data, true);
	}

	static loadResultAdvancedSearch(data) {
		TechnicalPlanSearch.loadResultData(data, false);
	}

	static loadResultData(data, simple) {
		if (data.length == 0) {
			$.UifNotify('show', { type: 'warning', message: AppResources.TechnicalPlanNotFound, autoclose: false }); //Pendiente Mensaje
		}
		else if (data.length == 1 && simple)
		{
			TechnicalPlan.SetDataControls(data[0]);			
		}
		else
		{
			TechnicalPlanSearch.ClearAdvanced();
			data.forEach(function (item) {
				technicalPlan =
					{
						TechnicalPlanId: item.Id,
						TechnicalPlanDescription: item.Description,
						TechnicalPlanShortDescription: item.SmallDescription,
						TechnicalPlanCurrentFrom: item.CurrentFrom,
						TechnicalPlanCurrentTo: item.CurrentTo,
						CoveredRiskTypeId: item.CoveredRiskType.Id,
						CoveredRiskTypeDescription: item.CoveredRiskType.SmallDescription
					}
				$("#LvSearchAdvTechnicalPlan").UifListView("addItem", technicalPlan);
			});
			if (simple)
			{
				$('#BtnAdvancedSearchTechnicalPlan').prop('disabled', 'disabled');
				$('#SearchShortDescription').prop('disabled', 'disabled');
				$("#SearchCoveredRiskType").UifSelect("disabled", true);		
			}
			dropDownSearch.show();			
		}
	}
	
	static ClearAdvanced() {
		$('#FormSearchTechnicalPlan')[0].reset()
		$("#LvSearchAdvTechnicalPlan").UifListView("refresh");
		$("#SearchShortDescription").val('');
		$('#SearchShortDescription').prop('disabled', '');
		$("#SearchCoveredRiskType").UifSelect("setSelected", "");
		$("#SearchCoveredRiskType").UifSelect("disabled", false);		
		$('#BtnAdvancedSearchTechnicalPlan').prop('disabled', '');
	}

	static ComponentLoadedCallback() {
		$("#SearchTechnicalPlan").TextTransform(ValidatorType.UpperCase);
		$("#SearchShortDescription").TextTransform(ValidatorType.UpperCase);

		request('Parametrization/TechnicalPlan/GetRiskTypes', null, 'GET', AppResources.ErrorSearchRiskType, TechnicalPlanSearch.loadCoveredRiskType);
		$("#BtnSearchAdvTechnicalPlan").on("click", function () {
			TechnicalPlanSearch.ClearAdvanced();
			dropDownSearch.show();
		});

		$("#BtnOkTechnicalPlanSearchAdv").on("click", function () {
				TechnicalPlanSearch.itemSelected();
		});

		$("#BtnCancelTechnicalPlanSearchAdv").on('click', function () {
			TechnicalPlanSearch.ClearAdvanced();
			dropDownSearch.hide();
		})

		$("#BtnAdvancedSearchTechnicalPlan").on('click', function () {
			TechnicalPlanSearch.searchAdvancedTechnicalPlan();
		})		
		$("#LvSearchAdvTechnicalPlan").UifListView({
			displayTemplate: "#ListTechnicalPlanTemplate",
			selectionType: 'single',
			source: null,
			edit: false,
			delete: false,
			customEdit: false,
			customDelete: false,
			height: 300
		});
	}
	static loadCoveredRiskType(data) {
		$("#SearchCoveredRiskType").UifSelect({ sourceData: data, id: 'Id', name: 'ShortDescription' });		
	}	
}