$.ajaxSetup({ async: true });
var principalCoverages = [];
var coverage = {};

var StatusEnum = {
	Original: 1,
	Create: 2,
	Update: 3,
	Delete: 4,
	Error: 5
};

var coverageIndex = null;

var CoverageModel = {
	InsuredObjectId: 0,
	InsuredObjectDescription: "",
	CoverageId: 0,
	CoverageDescription: "",
	PrincipalCoverageId: 0,
	CoveragePercentage: 0.00,
	Status: StatusEnum.Create,
	AllyCoverages: []
}

var CoverageModelEmpty = {
	InsuredObjectId: 0,
	InsuredObjectDescription: "",
	CoverageId: 0,
	CoverageDescription: "",
	PrincipalCoverageId: 0,
	CoveragePercentage: 0.00,
	Status: StatusEnum.Create,
	AllyCoverages: []
}

class TechnicalPlanCoverages extends Uif2.Page
{
	getInitialState()
	{
		$('#BtnModalAlliedCoverages').prop('disabled', 'disabled');
		$("#CoverageId").UifSelect("disabled", true);
		$("#CoveragePrincipalCoverageId").UifSelect("disabled", true);
		$("#CoveragePercentage").OnlyDecimals(2);
		$('#CoveragePercentage').prop('disabled', 'disabled');
		$("#ProcessStatus").val(StatusEnum.Create);		
		$("#ListAssignCoverages").UifListView({
			displayTemplate: "#ListAssignCoverageTemplate",
			selectionType: 'single',
			source: null,
			edit: true,
			delete: true,
			customEdit: true,
			deleteCallback: TechnicalPlanCoverages.deleteCallbackList,
			height: 180
		});
		
	}
	bindEvents() {
		$("#CoverageInsuredObject").on('itemSelected', this.changeInsuredObject);
		$("#CoverageId").on('itemSelected', this.changeCoverage);
		$("#CoveragePrincipalCoverageId").on('itemSelected', this.changeCoverageId);
		$("#BtnModalAlliedCoverages").click(this.showModalAlliedCoverages);
		$('#ListAssignCoverages').on('rowEdit', this.editItemCoverage);
		//$('#ListAssignCoverages').on('rowDelete', TechnicalPlanCoverages.deleteItemCoverage);
		$("#BtnNewCoverage").click(TechnicalPlanCoverages.NewCoverage);
		$("#BtnAddCoverage").click(this.addCoverage);	
		$("#BtnModalCoverageSave").click(TechnicalPlanCoverages.CoverageSave);	
	}

	/***************************************
				Cargar Combos 
	***************************************/
	static LoadInsuredObjects(data) {
		$("#CoverageInsuredObject").UifSelect({ sourceData: data, id: 'Id', name: 'Description', filter:true, native: false });
	}
	/**************************************/


	/***************************************
				Funciones 
	***************************************/
	static ClearForm() {
		$("#CoverageInsuredObject").UifSelect("setSelected", "");
		$("#CoverageId").UifSelect("setSelected", "");
		$("#CoverageId").UifSelect("disabled", true);
		$("#PrincipalCoverageLabel").removeClass("field-required");
		$("#CoveragePercentageLabel").removeClass("field-required");
		$("#CoveragePrincipalCoverageId").UifSelect("setSelected", "");
		$("#CoveragePrincipalCoverageId").UifSelect("disabled", true);
		$("#CoveragePercentage").val("0");
		$('#CoveragePercentage').prop('disabled', 'disabled');
		$("#CoverageIndex").val("");
		$("#ProcessStatus").val(StatusEnum.Create);
		TechnicalPlanCoverages.ClearTechnicalPlanCoverageModel();
	}

	static ClearTechnicalPlanCoverageModel() {
		CoverageModel.InsuredObjectId = 0;
		CoverageModel.InsuredObjectDescription = "";
		CoverageModel.CoverageId = 0;
		CoverageModel.CoverageDescription = "";
		CoverageModel.PrincipalCoverageId = 0;
		CoverageModel.CoveragePercentage = 0.00;
		CoverageModel.Status = StatusEnum.Create;
		CoverageModel.AllyCoverages = [];
	}

	static LoadInsuredObjectByCoveredRiskType(coveredRiskTypeId) {
		request('Parametrization/TechnicalPlan/GetInsuredObjects', { "coveredRiskType": coveredRiskTypeId }, 'GET', AppResources.ErrorSearchRiskType, TechnicalPlanCoverages.LoadInsuredObjects)
	}

	static LoadCoveragesFromData() {
		$("#ListAssignCoverages").UifListView("refresh");

		if (glbTechnicalPlan.Coverages.length > 0) {
			glbTechnicalPlan.Coverages.forEach(function (item)
			{
				if (item.Status != StatusEnum.Delete) {
					$("#ListAssignCoverages").UifListView("addItem", item);
				}
			});
		}
		TechnicalPlanCoverages.LoadInsuredObjectByCoveredRiskType(glbTechnicalPlan.RiskTypeId);
	}

	static LoadComboCoverageByInsuredObject(insuredObjectId) {
		if (insuredObjectId > 0) {
			request('Parametrization/TechnicalPlan/GetCoverageByInsuredObjects', { "insuredObjectId": insuredObjectId }, 'GET', AppResources.ErrorSearchRiskType, TechnicalPlanCoverages.LoadComboPrincipalCoverageByCoverages)
		}
	}

	static LoadComboPrincipalCoverageByCoverages(data)
	{
		principalCoverages = [];
		data.forEach(function (item) {
			if (item.IsPrimary) {
				principalCoverages.push(item);
			}
		});
        $("#CoverageId").UifSelect("disabled", false);
        $("#CoverageId").UifSelect({ sourceData: data, id: 'Id', name: 'Description', filter: true, native: false });
        $("#CoveragePrincipalCoverageId").UifSelect({ sourceData: principalCoverages, id: 'Id', name: 'Description', filter: true, native: false });
		$("#CoveragePrincipalCoverageId").UifSelect("disabled", true);
		
		if ($("#ProcessStatus").val() != StatusEnum.Create) {
			$("#CoverageInsuredObject").UifSelect("setSelected", CoverageModel.InsuredObjectId);
			$("#CoverageId").UifSelect("setSelected", CoverageModel.CoverageId);
			let disabledAllyCoverages = (CoverageModel.AllyCoverages.length == 0);

			if (isNaN(CoverageModel.PrincipalCoverageId) || (!isNaN(CoverageModel.PrincipalCoverageId) && CoverageModel.PrincipalCoverageId <= 0)) {
				$("#CoveragePrincipalCoverageId").UifSelect("setSelected", "");
				$("#CoveragePercentage").val(0);
				TechnicalPlanCoverages.EnabledControlSecundary(true, disabledAllyCoverages);
			}
			else {
				$("#CoveragePrincipalCoverageId").UifSelect("setSelected", CoverageModel.PrincipalCoverageId);
				$("#CoveragePercentage").val(CoverageModel.CoveragePercentage);
				TechnicalPlanCoverages.EnabledControlSecundary(false, disabledAllyCoverages);
			}

			if (CoverageModel.Status == StatusEnum.Create && CoverageModel.CoverageId > 0) {
				request('Parametrization/TechnicalPlan/GetAlliedCoverages', { "coverageId": CoverageModel.CoverageId }, 'GET', AppResources.ErrorSearchRiskType, TechnicalPlanCoverages.GetAllyCoverages)
			}
		}	
		$("#CoverageId").UifSelect("disabled", false);		
	}

	static EnabledControlSecundary(state, stateButton) {
		$("#CoveragePrincipalCoverageId").UifSelect("disabled", state);
		$('#CoveragePercentage').prop('disabled', state == true ? 'disabled' : "");
		$("#BtnModalAlliedCoverages").prop('disabled', stateButton == true ? 'disabled' : "");
	}

	changeInsuredObject() {
		var item = parseInt($("#CoverageInsuredObject").UifSelect('getSelected'));
		if (isNaN(item)) {
			item = 0;
		}
		TechnicalPlanCoverages.LoadComboCoverageByInsuredObject(item);
	}

	changeCoverage() {
		var item = $("#CoverageId").UifSelect('getSelectedSource');
		if ((item != null) && (item != undefined)) {
			$("#CoveragePrincipalCoverageId").UifSelect("setSelected", "");
			$('#CoveragePercentage').val("0");
			if (item.IsPrimary) {
				$("#CoveragePrincipalCoverageId").UifSelect("disabled", true);
				$('#CoveragePercentage').prop('disabled', 'disabled');
				$("#PrincipalCoverageLabel").removeClass("field-required");
				$("#CoveragePercentageLabel").removeClass("field-required");
			}
			else {
				$("#CoveragePrincipalCoverageId").UifSelect("disabled", false);
				$('#CoveragePercentage').prop('disabled', '');
				$("#PrincipalCoverageLabel").addClass("field-required");
				$("#CoveragePercentageLabel").addClass("field-required");
			}
			request('Parametrization/TechnicalPlan/GetAlliedCoverages', { "coverageId": item.Id }, 'GET', AppResources.ErrorSearchRiskType, TechnicalPlanCoverages.GetAllyCoverages)
		}		
	}
	
	showModalAlliedCoverages() {
		$("#FormCoverage").validate();
		if ($("#FormCoverage").valid()) {
			TechnicalPlanAllyCoverages.LoadAllyCoverages();
			$("#ModalAllyCoverages").UifModal('showLocal', Resources.Language.LabelTechnicalPlanAlliedCoverages);
		}
	}
	static NewCoverage() {
		$('#FormCoverage')[0].reset()		
		TechnicalPlanCoverages.ClearForm();
	}

    addCoverage() {
			$("#FormCoverage").validate();
		if ($("#FormCoverage").valid())
		{
			var existsCoverage;
			var existsPrincipalCoverage;
			var canAdd = true;
			var message = "";

			CoverageModel.InsuredObjectId = $("#CoverageInsuredObject").UifSelect("getSelected");
			CoverageModel.InsuredObjectDescription = $("#CoverageInsuredObject").UifSelect("getSelectedText");
			CoverageModel.CoverageId = $("#CoverageId").UifSelect("getSelected");
			CoverageModel.CoverageDescription = $("#CoverageId").UifSelect("getSelectedText");
			CoverageModel.PrincipalCoverageId = $("#CoveragePrincipalCoverageId").UifSelect("getSelected");
			CoverageModel.CoveragePercentage = $("#CoveragePercentage").val();	
			
			var source = $("#ListAssignCoverages").UifListView('getData');
			existsCoverage = false;
			if (source != null && source != undefined) {
				source.forEach(function (item)
				{
					if (item.InsuredObjectId == CoverageModel.InsuredObjectId && item.CoverageId == CoverageModel.CoverageId) {
						existsCoverage = true;
					}					
				});
			}

			canAdd = false;
			message = "";
			if (existsCoverage) {
				switch (CoverageModel.Status) {
					case StatusEnum.Create:
						message = AppResources.ErrorExistsCoverage;
						break;
					case StatusEnum.Original:
						CoverageModel.Status == StatusEnum.Update;
						canAdd = true;
						break;
					case StatusEnum.Update:
						canAdd = true;
						break;
				}				
			}
			else {
				existsPrincipalCoverage = true;
				if (CoverageModel.PrincipalCoverageId > 0) {
					if (source != null && source != undefined) {
						source.forEach(function (item) {
							if (item.InsuredObjectId == CoverageModel.InsuredObjectId && item.CoverageId == CoverageModel.PrincipalCoverageId) {
								existsPrincipalCoverage = true;
							}
						});
					}
					if (existsPrincipalCoverage == true) {
						canAdd = true;
					}
					else {
						message = AppResources.ErrorPrincipalCoverageNotFound;
					}
				}
				else {
					canAdd = true;
				}				
			}
			
			if (canAdd == true)
			{
				var index = parseInt($("#CoverageIndex").val());
				if (isNaN(index)) {
					index = null;
				}				
				let coverage = JSON.parse(JSON.stringify(CoverageModel));					
				if (index == null || index == undefined)
				{
					coverage.Status = StatusEnum.Create;
					$("#ListAssignCoverages").UifListView("addItem", coverage);
				}
				else
				{
					coverage.Status = StatusEnum.Update;
					$('#ListAssignCoverages').UifListView('editItem', index, coverage);
				}
				TechnicalPlanCoverages.NewCoverage();
			}
			else {
				$.UifNotify('show', { 'type': 'warning', 'message': message , 'autoclose': true });
			}
		}
	}

	editItemCoverage(event, data, index)
	{
		TechnicalPlanCoverages.ClearForm();
		if (data != undefined) {
			CoverageModel = JSON.parse(JSON.stringify(data));		
			$("#CoverageIndex").val(index);
			$("#ProcessStatus").val(StatusEnum.Update);
			TechnicalPlanCoverages.LoadComboCoverageByInsuredObject(data.InsuredObjectId);			
		}
	}
	static deleteItemCoverage(event, data)
	{
		var canDelete;
		if (data != undefined)
		{
			var coverageItem = TechnicalPlanCoverages.GetCoverageByIdFromList(data.CoverageId);
			var coverageIndex = -1;
			if (coverageItem.length > 0) {
				var coverageIndex = glbTechnicalPlan.Coverages.indexOf(coverageItem[0]);
			}
			
			if (coverageIndex != -1)
			{
				var childCoverages = TechnicalPlanCoverages.GetChildCoveragesByPrincipalCoverageIdFromList(data.CoverageId);
				if (childCoverages == null) {
					canDelete = true;
				}
				else {
					canDelete = (childCoverages.length == 0);
				}
			}

			if (canDelete) {
				switch (coverageItem[0].Status) {
					case StatusEnum.Create:
						glbTechnicalPlan.Coverages.pop(glbTechnicalPlan.Coverages[coverageIndex]);
						break;
					case StatusEnum.Original:
					case StatusEnum.Update:
						glbTechnicalPlan.Coverages[coverageIndex].Status = StatusEnum.Delete;
						break;
				}

               // $("#ListAssignCoverages").UifListView({ displayTemplate: "#ListAssignCoverageTemplate", selectionType: 'single', source: null, edit: true, delete: true, customEdit: true, deleteCallback: TechnicalPlanCoverages.deleteCallbackList , height: 180 });
				//if (glbTechnicalPlan.Coverages.length > 0) {
				//	glbTechnicalPlan.Coverages.forEach(function (item) {
    //                    if (item.Status != StatusEnum.Delete) {
    //                        $("#ListAssignCoverages").UifListView("addItem", item);
    //                    } 

				//	});
				//}
			}
			else {
                $.UifNotify('show', { 'type': 'warning', 'message': AppResources.ErrorDependenceWithAnother, 'autoclose': true });
               
			}				
		}
	}

	static GetCoverageByIdFromList(coverageId) {
		if (coverageId != null && coverageId != undefined) {
			var source = $("#ListAssignCoverages").UifListView('getData');
			if (source != null && source != undefined) {
				return source.filter(x => x.CoverageId == coverageId);
			}
		}	
		return null;
	}

	static GetChildCoveragesByPrincipalCoverageIdFromList(coverageId) {
		if (coverageId != null && coverageId != undefined) {
			var source = $("#ListAssignCoverages").UifListView('getData');
			if (source != null && source != undefined) {
				return source.filter(x => x.PrincipalCoverageId == coverageId);
			}
		}
		return null;
	}

	static GetCoverages(data) {
		principalCoverages = [];
		data.forEach(function (item) {
			if (item.IsPrimary) {
				principalCoverages.push(item);
			}
		});
		$("#CoverageId").UifSelect("disabled", false);
        $("#CoverageId").UifSelect({ sourceData: data, id: 'Id', name: 'Description', filter: true, native: false });
        $("#CoveragePrincipalCoverageId").UifSelect({ sourceData: principalCoverages, id: 'Id', name: 'Description', filter: true, native: false });
		$("#CoveragePrincipalCoverageId").UifSelect("disabled", true);
	}

	static GetAllyCoverages(data) {
		CoverageModel.AllyCoverages = [];
		data.forEach(function (item) {
			if (CoverageModel.Status == StatusEnum.Create) {
				item.Status = StatusEnum.Create;
			}
			CoverageModel.AllyCoverages.push(item);
		});

		if (CoverageModel.Status == StatusEnum.Create) {
			if (CoverageModel.AllyCoverages.length == 0) {
				$('#BtnModalAlliedCoverages').prop('disabled', 'disabled');
			}
			else {
				$('#BtnModalAlliedCoverages').prop('disabled', '');
			}
		}
	}

	static CoverageSave()
    {
		var source = $("#ListAssignCoverages").UifListView('getData');
		if (source != null && source != undefined)
		{
			source.forEach(function (item) {
				var coverageItem = glbTechnicalPlan.Coverages.filter(x => x.CoverageId == item.CoverageId);
				var index = -1;
				if (coverageItem.length > 0) {
					var index = glbTechnicalPlan.Coverages.indexOf(coverageItem[0]);
				}
				if (index != -1) {
					if (item.Status == StatusEnum.Update ) {
						glbTechnicalPlan.Coverages[index].InsuredObjectId = item.InsuredObjectId;
						glbTechnicalPlan.Coverages[index].InsuredObjectDescription = item.InsuredObjectDescription;
						glbTechnicalPlan.Coverages[index].CoveragePercentage = item.CoveragePercentage;
						glbTechnicalPlan.Coverages[index].PrincipalCoverageId = item.PrincipalCoverageId;
						if (glbTechnicalPlan.Coverages[index].Status == StatusEnum.Original) {
							glbTechnicalPlan.Coverages[index].Status = StatusEnum.Update;
						}
					}
				}
				else {
					let newCoverage =
						{
							InsuredObjectId: item.InsuredObjectId,
							InsuredObjectDescription: item.InsuredObjectDescription,
							CoverageId: item.CoverageId,
							CoverageDescription: item.CoverageDescription,
							PrincipalCoverageId: item.PrincipalCoverageId,
							CoveragePercentage: item.CoveragePercentage,
							Status: StatusEnum.Create,
							AllyCoverages: CoverageModel.AllyCoverages
						}
					glbTechnicalPlan.Coverages.push(newCoverage);
				}
			});
			$("#ModalCoverages").UifModal('hide');
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoveragesStoredCorrectly, 'autoclose': true });
		}
	}
	static deleteCallbackList(deferred, result) {
        deferred.resolve();
        TechnicalPlanCoverages.deleteItemCoverage(null, result);
        if (result.Status == ParametrizationStatus.Delete) {
            result.allowEdit = false;
            result.allowDelete = false;
            $("#ListAssignCoverages").UifListView("addItem", result);
        } else {
            if (result.Status == ParametrizationStatus.Original) {
                $("#ListAssignCoverages").UifListView("addItem", result);
            }
        }
       
		//deferred.resolve();
		
	}
	/**************************************/
}