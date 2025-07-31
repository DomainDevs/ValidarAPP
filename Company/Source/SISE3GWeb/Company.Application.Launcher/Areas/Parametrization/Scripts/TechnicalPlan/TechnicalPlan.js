$.ajaxSetup({ async: true });
var StatusEnum = {
	Original: 1,
	Create: 2,
	Update: 3,
	Delete: 4,
	Error: 5
};

var glbTechnicalPlan =
{
	Id: 0,
	Description: "",
	ShortDescription: "",
	CurrentFrom: "",
	CurrentTo: "",
	RiskTypeId: 0,
	RiskTypeSmallDescription: "",
	Status: StatusEnum.Create,
	Coverages: []
}

class TechnicalPlan extends Uif2.Page {

	getInitialState() {
		request('Parametrization/TechnicalPlan/GetRiskTypes', null, 'GET', AppResources.ErrorSearchRiskType, this.loadCoveredRiskType);
		$('#BtnDeleteTechnicalPlan').prop('disabled', 'disabled');		
	}
	bindEvents() {
		$("#TechnicalPlanDescription").TextTransform(ValidatorType.UpperCase);
		$("#TechnicalPlanShortDescription").TextTransform(ValidatorType.UpperCase);
		$("#BtnExitTechnicalPlan").click(this.redirectIndex)
		$("#BtnDeleteTechnicalPlan").click(this.deleteItem)
		$("#BtnNewTechnicalPlan").click(TechnicalPlan.NewItem)
		$("#BtnExportTechnicalPlan").click(this.exportData)
		$("#BtnSaveTechnicalPlan").click(this.saveItem)
		$("#BtnCoverages").click(this.showModalCoverages);		
	}

	/***************************************
				Cargar Combos 
	***************************************/
	loadCoveredRiskType(data) {
		$("#TechnicalPlanCoveredRiskType").UifSelect({ sourceData: data, id: 'Id', name: 'ShortDescription' });
	}
	/**************************************/
	

	/***************************************
				Funciones 
	***************************************/
	static SetDataControls(data)
	{
		TechnicalPlan.ClearForm();		
		glbTechnicalPlan.Id = data.Id;
		glbTechnicalPlan.Description = data.Description;
		glbTechnicalPlan.ShortDescription = data.SmallDescription;
		glbTechnicalPlan.CurrentFrom = data.CurrentFrom;
		glbTechnicalPlan.CurrentTo = data.CurrentTo;
		glbTechnicalPlan.RiskTypeId = data.CoveredRiskType.Id;
		glbTechnicalPlan.RiskTypeSmallDescription = data.CoveredRiskType.SmallDescription;
		glbTechnicalPlan.Status = StatusEnum.Original;
        TechnicalPlan.GetCoverageData(glbTechnicalPlan.Id);
        $('#BtnDeleteTechnicalPlan').prop('disabled', '');
        $('#TechnicalPlanCoveredRiskType').prop('disabled', 'disabled');
	}

	static GetCoverageData(id)
	{
		request('Parametrization/TechnicalPlan/GetCoverageByTechnicalPlanId', { "technicalPlanId": id }, 'GET', AppResources.ErrorSearchRiskType, TechnicalPlan.LoadCoverageData);
	}

	static LoadCoverageData(data)
	{
		glbTechnicalPlan.Coverages = [];		
		data.forEach(function (item)
		{
			let coverageItem = {
				InsuredObjectId: item.InsuredObject.Id,
				InsuredObjectDescription: item.InsuredObject.Description,
				CoverageId: item.Coverage.Id,
				CoverageDescription: item.Coverage.Description,
				PrincipalCoverageId: item.PrincipalCoverage === null ? null : item.PrincipalCoverage.Id,
				CoveragePercentage: item.CoveragePercentage,
				Status: item.StatusTypeService,
				AllyCoverages: []
			}
			if (item.AlliedCoverages.length > 0) {
				item.AlliedCoverages.forEach(function (ally) {
					let allyCoverageItem = {
						Id: ally.Id,
						Description: ally.Description,
						CoveragePercentage: ally.AlliedCoveragePercentage,
						Status: ally.StatusTypeService
					}					
					coverageItem.AllyCoverages.push(allyCoverageItem);
				});
			}
			glbTechnicalPlan.Coverages.push(coverageItem);			
		});
		TechnicalPlan.AssingValues();
	}

	static AssingValues() {
		$("#TechnicalPlanId").val(glbTechnicalPlan.Id);
		$("#TechnicalPlanStatus").val(glbTechnicalPlan.Status);
		$("#TechnicalPlanCoveredRiskType").UifSelect('setSelected', glbTechnicalPlan.RiskTypeId);
		$("#TechnicalPlanDescription").val(glbTechnicalPlan.Description);
		$("#TechnicalPlanShortDescription").val(glbTechnicalPlan.ShortDescription);
		let cntCoverage = glbTechnicalPlan.Coverages.length;		
		if (glbTechnicalPlan.Coverages.length === 0) {
			$('#SelectedCoverages').text("");
		}
		else {
			$('#SelectedCoverages').text("(" + cntCoverage + ")");
		}
	}

	static ClearTechnicalPlanModel() {
		//-------------------------------------------
		glbTechnicalPlan.Id = 0;
		glbTechnicalPlan.Description = "";
		glbTechnicalPlan.ShortDescription = "";
		glbTechnicalPlan.RiskTypeId = 0;
		glbTechnicalPlan.RiskTypeSmallDescription = "";
		glbTechnicalPlan.Status = StatusEnum.Create;
		glbTechnicalPlan.Coverages = [];
		//-------------------------------------------
	}
	
    static ClearForm() {
        $('#TechnicalPlanCoveredRiskType').prop('disabled', '');
		$("#TechnicalPlanCoveredRiskType").UifSelect('setSelected', "");
        $("#TechnicalPlanDescription").val("");
        $("#TechnicalPlanDescription").focus();
		$("#TechnicalPlanShortDescription").val("");
		$("#TechnicalPlanId").val("");
		$("#TechnicalPlanStatus").val(StatusEnum.Create);
		$('#SelectedCoverages').text("");
        $('#BtnDeleteTechnicalPlan').prop('disabled', 'disabled');	
       
		TechnicalPlan.ClearTechnicalPlanModel();
	}

	static getAccion() {
		return $("#TechnicalPlanStatus").val();
	}

	redirectIndex() {
		window.location = rootPath + "Home/Index";
	}

	deleteItem() {
		
		$.UifDialog('confirm', { 'message': AppResources.TechnicalPlanDeleteQuestion  }, function (result) {
			if (result)
			{
				if (glbTechnicalPlan.Status === StatusEnum.Create) {
					TechnicalPlan.NewItem;
				}
				else {
					glbTechnicalPlan.Status = StatusEnum.Delete;
                    request('Parametrization/TechnicalPlan/ExecuteOperations', JSON.stringify({ technicalPlan: glbTechnicalPlan }), 'POST', AppResources.ErrorSearchRiskType, TechnicalPlan.ClearForm);
				}
				$.UifNotify('show', { type: 'info', message: AppResources.TechnicalPlanDeleteMessage, autoclose: true });
				TechnicalPlan.ClearForm();
			}			

		});
	}

	static NewItem()
	{
		
		$('#FormTechnicalPlan')[0].reset()
		
		TechnicalPlan.ClearForm();
	}

	exportData()
	{
		request('Parametrization/TechnicalPlan/GenerateFileToExport', null, 'GET', AppResources.ErrorGeneratingExcelFile, TechnicalPlan.generateFileToExport);		
	}

	static generateFileToExport(data) {
		DownloadFile(data);
	}

	saveItem() {
		$("#FormTechnicalPlan").validate()
		if ($("#FormTechnicalPlan").valid())
		{
			if (glbTechnicalPlan.Status === StatusEnum.Original) {
				if (glbTechnicalPlan.Description !== $("#TechnicalPlanDescription").val() ||
					glbTechnicalPlan.ShortDescription !== $("#TechnicalPlanShortDescription").val() ||
					glbTechnicalPlan.RiskTypeId !== $("#TechnicalPlanCoveredRiskType").UifSelect('getSelected')) {
					glbTechnicalPlan.Status = StatusEnum.Update;
				}
			}
			TechnicalPlan.GetDataForm();
			request('Parametrization/TechnicalPlan/ExecuteOperations', JSON.stringify({ technicalPlan: glbTechnicalPlan }), 'POST', AppResources.ErrorSearchRiskType, TechnicalPlan.LoadSaveItemResult);
		}
	}

	static LoadSaveItemResult(data)
	{
		
		TechnicalPlan.NewItem;
		$.UifNotify('show', { 'type': 'info', 'message': AppResources.TechnicalPlanListMessageSave, 'autoclose': true });
		TechnicalPlan.ClearForm();
		
	}

	showModalCoverages() {
		$("#FormTechnicalPlan").validate()
		if ($("#FormTechnicalPlan").valid())
		{
			TechnicalPlan.GetDataForm();
			TechnicalPlanCoverages.ClearForm();
			TechnicalPlanCoverages.LoadCoveragesFromData();			
			$("#ModalCoverages").UifModal('showLocal', Resources.Language.LabelCoverages);
		} 
	}

	static GetDataForm() {
		glbTechnicalPlan.Id = $("#TechnicalPlanId").val();
		glbTechnicalPlan.Description = $("#TechnicalPlanDescription").val();
		glbTechnicalPlan.ShortDescription = $("#TechnicalPlanShortDescription").val();
		glbTechnicalPlan.RiskTypeId = $("#TechnicalPlanCoveredRiskType").UifSelect('getSelected');
		glbTechnicalPlan.RiskTypeSmallDescription = $("#TechnicalPlanCoveredRiskType").UifSelect('getSelectedText');
	}
	/**************************************/
}