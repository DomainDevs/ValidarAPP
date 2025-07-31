var glbListBranch = {};
var glbListPrefix = {};
var formPolicyNumeration = {};
var formUpdate;

class PolicyNumerationParametrization extends Uif2.Page {
	getInitialState() {
		new PolicyNumerationParametrizationRequest();
		$("input[type=text]").TextTransform(ValidatorType.UpperCase);
		$('#inputDueDateFrom').val(GetCurrentFromDate());
        $('#inputDueDateFrom').attr('disabled', 'disabled');
		PolicyNumerationParametrizationRequest.GetBranchs().done(function (data) {
			if (data.success) {
				PolicyNumerationParametrization.getBranchs(data.result);
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});

		PolicyNumerationParametrizationRequest.GetPrefixes().done(function (data) {
			if (data.success) {
				PolicyNumerationParametrization.getPrefixs(data.result);
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});
	}

	bindEvents() {
	    $("#btnExit").on("click", this.Exit);	    
		$("#btnNewPolicyNumeration").on("click", PolicyNumerationParametrization.CleanForm);
		$("#btnDelete").on("click", this.DeletePolicyNumeration);
		$("#btnExport").on("click", this.sendExcelPolicyNumeration);
		$("#btnSavePolicyNumeration").on("click", this.SavePolicyNumeration);
		$("#selectBranch").on("itemSelected", this.GetPolicyNumerationByBranchIdPrefixId);
		$("#selectPrefix").on("itemSelected", this.GetPolicyNumerationByBranchIdPrefixId);
	}

	GetPolicyNumerationByBranchIdPrefixId() {
		if ($("#selectBranch").UifSelect("getSelected") != "" && $("#selectPrefix").UifSelect("getSelected") != "") {
			var branchId = $("#selectBranch").UifSelect("getSelected")
			var prefixId = $("#selectPrefix").UifSelect("getSelected")
			PolicyNumerationParametrizationRequest.GetPolicyNumerationByBranchIdPrefixId(branchId, prefixId).done(function (data) {
				if (data.success) {
				    if (data.result != null) {
						$("#inputLastPolicy").val(data.result.LastPolicy);
						$("#inputLastPolicy").attr("disabled", "disabled");
						$('#inputDueDateFrom').val(data.result.DueDateTo);
						$.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NumerationExsists, 'autoclose': true });
						$("#btnSavePolicyNumeration").attr("disabled", "disabled");
				    }
					else {
						$("#inputLastPolicy").val("");
						$("#inputLastPolicy").attr("disabled", false);
						$('#inputDueDateFrom').val(GetCurrentFromDate());
						$("#btnSavePolicyNumeration").attr("disabled", false);
					}
				}
				else {
					$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
				}
			});
		}
	}

	static getBranchs(data) {
		glbListBranch = data;
		$('#selectBranch').UifSelect({ sourceData: data });
		$('#selectSearchSucursal').UifSelect({ sourceData: data });
	}

	static getPrefixs(data) {
		glbListPrefix = data;
		$('#selectPrefix').UifSelect({ sourceData: data });
	}

	sendExcelPolicyNumeration() {
	    PolicyNumerationParametrization.CleanForm();
		PolicyNumerationParametrizationRequest.GenerateFileToExport().done(function (data) {
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

    /**
    *@summary Elimina una sucursal del listview y lo agrega al array de sucursales por eliminiar 
    */
	DeletePolicyNumeration() {

	}

    SavePolicyNumeration() {
		$('#formPolicyNumeration').validate();

        if ($('#formPolicyNumeration').valid()) {
			var PolicyNumerations = $('#formPolicyNumeration').serializeObject();
			if (PolicyNumerations.LastPolicy == 0) {
				$.UifNotify('show', {
					'type': 'danger', 'message': Resources.Language.FileStringCero, 'autoclose': true
				})
			}
			else {
				if (formPolicyNumeration.Status == ParametrizationStatus.Update) {
					PolicyNumerations.StatusTypeService = ParametrizationStatus.Update;
				} else {
					PolicyNumerations.StatusTypeService = ParametrizationStatus.Create;
				}
				PolicyNumerationParametrizationRequest.Save(PolicyNumerations)
					.done(function (data) {
						if (data.success) {
							$.UifNotify('show', {
								'type': 'info', 'message': data.result.message, 'autoclose': true
							});
							PolicyNumerationParametrization.CleanForm();
						}
						else {
							$.UifNotify('show', {
								'type': 'info', 'message': data.result, 'autoclose': true
							});
						}
					})
					.fail(function () {
						$.UifNotify('show', {
							'type': 'danger', 'message': Resources.Language.ErrorSavingNumeration, 'autoclose': true
						})

					});
			}
		}

	}

	DeletePolicyNumeration() {
		$('#formPolicyNumeration').validate();

		if ($('#formPolicyNumeration').valid()) {
			var branchId = $("#selectBranch").UifSelect("getSelected")
			var prefixId = $("#selectPrefix").UifSelect("getSelected")
			PolicyNumerationParametrizationRequest.GetPolicyNumerationByBranchIdPrefixId(branchId, prefixId).done(function (data) {
				if (data.result) {
					$.UifDialog('confirm', { 'message': Resources.Language.ConfirmDelete }, function (result) {
						if (result) {
							var PolicyNumerations = $('#formPolicyNumeration').serializeObject();
							PolicyNumerationParametrizationRequest.Delete(PolicyNumerations)
								.done(function (data) {
									if (data.success) {
										$.UifNotify('show', {
											'type': 'info', 'message': data.result.message, 'autoclose': true
										});
										PolicyNumerationParametrization.CleanForm();
									}
									else {
										$.UifNotify('show', {
											'type': 'info', 'message': data.result, 'autoclose': true
										});
									}
								})
								.fail(function () {
									$.UifNotify('show', {
										'type': 'danger', 'message': Resources.Language.ErrorDeletingNumeration, 'autoclose': true
									})

								});
						}
					});
				}
				else {
					$.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDeletingNumeration, 'autoclose': true });
				}
			});
		}

	}

	//----------------------------------------------------------//

    /**
    *@summary Limpia los controles de la vista
    */
	static CleanForm() {
		$("#inputLastPolicy").val(null);
		$("#selectBranch").val(null);
		$("#selectPrefix").val(null);
		$("#inputLastPolicy").attr("disabled", false);
		$("#btnSavePolicyNumeration").attr("disabled", false);
		ClearValidation('#formPolicyNumeration');
	}
	/**
   *@summary Redirecciona al index
   */
	Exit() {
		window.location = rootPath + "Home/Index";
	}
}