$.ajaxSetup({ async: true });
var StatusEnum = {
	Original: 1,
	Create: 2,
	Update: 3,
	Delete: 4,
	Error: 5
};

class TechnicalPlanAllyCoverages extends Uif2.Page {

	getInitialState() {
		$("#EditAllyPercentageCoverage").OnlyDecimals(2);		
		$('#TableAllyCoverages').on('rowSelected', function (event, data) {
			TechnicalPlanParametrization.TableBind(data);
		});
		$("#TableAllyCoverages").on("rowEdit", this.EditItemRow);				

		$("#EditAction").UifInline("hide");
		$('#EditAction').on('Next', function () {
			$('#TableAllyCoverages').UifDataTable("next");
		});

		$('#EditAction').on('Previous', function () {
			$('#TableAllyCoverages').UifDataTable("previous");
		});
		$("#EditAction").on("Save", this.SaveItemRow);		
	}
	bindEvents() {		
		$("#BtnModalAllyCoverageSave").click(TechnicalPlanAllyCoverages.AllyCoverageSave);
	}

	EditItemRow(event, data, position) {
		$('#AllyIndex').val(position);
		TechnicalPlanAllyCoverages.TableBind(data);
		$('#EditAction').UifInline('show');
	}

	SaveItemRow()
	{
		let index = $('#AllyIndex').val();
		let tableItem = {
			coverageId: $('#Id').val(),
			coveragePercentage: $('#EditAllyCoveragePercentage').val(),
			coverageDescription: $('#EditAllyCoverage').val(),
			coverageStatus: StatusEnum.Update
		};
		$("#TableAllyCoverages").UifDataTable('editRow', tableItem, index);
		$("#EditAction").UifInline("hide");
	}

	static TableBind(item) {
		$("#EditForm").find("#Id").val(item.coverageId);
		$("#EditForm").find("#EditAllyCoverage").val(item.coverageDescription);
		$("#EditForm").find("#EditAllyCoveragePercentage").val(item.coveragePercentage);
		$("#EditForm").find("#Status").val(item.coverageStatus);
	}

	static LoadAllyCoverages() {
		$("#TableAllyCoverages").UifDataTable("clear");
		CoverageModel.AllyCoverages.forEach(function (item) {
			let tableItem = {
				coverageId: item.Id,
				coveragePercentage: item.CoveragePercentage,
				coverageDescription: item.Description,
				coverageStatus: CoverageModel.Status
			};
			$("#TableAllyCoverages").UifDataTable("addRow", tableItem);
		});	
	}

	static AllyCoverageCancel() {

	}

	static AllyCoverageSave() {
		var source = $("#TableAllyCoverages").UifDataTable('getData');
		if (source != null && source != undefined)
		{
			source.forEach(function (item)
			{
				let coverage = glbTechnicalPlan.Coverages.filter((coverages) => coverages.AllyCoverages.some((allys) => allys.Id == item.coverageId));				
				var indexCoverage = -1;

				if (coverage.length > 0) {
					indexCoverage = glbTechnicalPlan.Coverages.indexOf(coverage[0]);		
				}

				if (indexCoverage != -1) {
					var allyCoverage = glbTechnicalPlan.Coverages[indexCoverage].AllyCoverages.filter(x => x.Id == item.coverageId);
					var indexAllyCoverage = -1;
					if (allyCoverage.length > 0) {
						indexAllyCoverage = glbTechnicalPlan.Coverages[indexCoverage].AllyCoverages.indexOf(allyCoverage[0]);	
					}

					if (indexAllyCoverage != -1) {
						if (item.coverageStatus == StatusEnum.Update) {
							glbTechnicalPlan.Coverages[indexCoverage].AllyCoverages[indexAllyCoverage].CoveragePercentage = item.coveragePercentage;
							if (glbTechnicalPlan.Coverages[indexCoverage].AllyCoverages[indexAllyCoverage].Status == StatusEnum.Original) {
								glbTechnicalPlan.Coverages[indexCoverage].AllyCoverages[indexAllyCoverage].Status = StatusEnum.Update;
							}
						}
					}
					else {
						let newAllyCoverage =
							{
								Id: item.coverageId,
								CoveragePercentage: item.coveragePercentage,
								Description: item.coverageDescription,
								Status: StatusEnum.Create,
							}
						glbTechnicalPlan.Coverages[indexCoverage].AllyCoverages.push(newAllyCoverage);
					}
				}

				var allyItem = CoverageModel.AllyCoverages.filter(x => x.Id == item.coverageId);
				var index = -1;
				if (allyItem.length > 0) {
					var index = CoverageModel.AllyCoverages.indexOf(allyItem[0]);					
				}
				if (index != -1) {
					if (item.coverageStatus == StatusEnum.Update) {
						CoverageModel.AllyCoverages[index].CoveragePercentage = item.coveragePercentage;
						if (CoverageModel.AllyCoverages[index].Status == StatusEnum.Original) {
							CoverageModel.AllyCoverages[index].Status = StatusEnum.Update;
						}
					}
				}				
			});
			$("#ModalAllyCoverages").UifModal('hide');
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.TechnicalPlanListMessageSave, 'autoclose': true });
		}
	}
}