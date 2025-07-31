var glbListBranch = {};
var glbListPrefix = {};

class QuotationNumerationParametrization extends Uif2.Page {
	getInitialState() {
		new QuotationNumerationParametrizationRequest();
		$("input[type=text]").TextTransform(ValidatorType.UpperCase);


		QuotationNumerationParametrizationRequest.GetBranchs().done(function (data) {
			if (data.success) {
				QuotationNumerationParametrization.getBranchs(data.result);
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});

		QuotationNumerationParametrizationRequest.GetPrefixes().done(function (data) {
			if (data.success) {
				QuotationNumerationParametrization.getPrefixs(data.result);
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});
	}

	bindEvents() {
		$("#btnExit").on("click", this.Exit);
		$("#btnNewQuotationNumeration").on("click", QuotationNumerationParametrization.CleanForm);
		$("#btnDelete").on("click", this.DeleteQuotationNumeration);
		$("#btnExport").on("click", this.sendExcelQuotationNumeration);
		$("#btnSaveQuotationNumeration").on("click", this.SaveQuotationNumeration);
		$("#selectBranch").on("itemSelected", this.GetQuotationNumerationByBranchIdPrefixId);
		$("#selectPrefix").on("itemSelected", this.GetQuotationNumerationByBranchIdPrefixId);
	}

	GetQuotationNumerationByBranchIdPrefixId() {
		if ($("#selectBranch").UifSelect("getSelected") != "" && $("#selectPrefix").UifSelect("getSelected") != "") {
			var branchId = $("#selectBranch").UifSelect("getSelected")
			var prefixId = $("#selectPrefix").UifSelect("getSelected")
			QuotationNumerationParametrizationRequest.GetQuotationNumerationByBranchIdPrefixId(branchId, prefixId).done(function (data) {
				if (data.success) {
					if (data.result != null) {
						$("#inputLastQuotation").val(data.result.LastQuotation);
						$("#inputLastQuotation").attr("disabled", "disabled");
						$.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NumerationExsists, 'autoclose': true });
						$("#btnSaveQuotationNumeration").attr("disabled", "disabled");
					}
					else {
						$("#inputLastQuotation").val("");
						$("#inputLastQuotation").attr("disabled", false);
						$("#btnSaveQuotationNumeration").attr("disabled", false);
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

	sendExcelQuotationNumeration() {
		QuotationNumerationParametrizationRequest.GenerateFileToExport().done(function (data) {
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

	SaveQuotationNumeration() {
		$('#formQuotationNumeration').validate();

		if ($('#formQuotationNumeration').valid()) {
			var QuotationNumerations = $('#formQuotationNumeration').serializeObject();
			if (QuotationNumerations.LastQuotation == 0) {
				$.UifNotify('show', {
					'type': 'danger', 'message': Resources.Language.FileStringCero, 'autoclose': true
				})
			}
			else {
				QuotationNumerationParametrizationRequest.Save(QuotationNumerations)
					.done(function (data) {
						if (data.success) {
							$.UifNotify('show', {
								'type': 'info', 'message': data.result.message, 'autoclose': true
							});
							QuotationNumerationParametrization.CleanForm();
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

	DeleteQuotationNumeration() {
		$('#formQuotationNumeration').validate();

		if ($('#formQuotationNumeration').valid()) {
			var branchId = $("#selectBranch").UifSelect("getSelected")
			var prefixId = $("#selectPrefix").UifSelect("getSelected")
			QuotationNumerationParametrizationRequest.GetQuotationNumerationByBranchIdPrefixId(branchId, prefixId).done(function (data) {
				if (data.result) {
					$.UifDialog('confirm', { 'message': Resources.Language.ConfirmDelete }, function (result) {
						if (result) {
							var QuotationNumerations = $('#formQuotationNumeration').serializeObject();
							QuotationNumerationParametrizationRequest.Delete(QuotationNumerations)
								.done(function (data) {
									if (data.success) {
										$.UifNotify('show', {
											'type': 'info', 'message': data.result.message, 'autoclose': true
										});
										QuotationNumerationParametrization.CleanForm();
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
		$("#inputLastQuotation").val(null);
		$("#selectBranch").val(null);
		$("#selectPrefix").val(null);
		$("#inputLastQuotation").attr("disabled", false);
		$("#btnSaveQuotationNumeration").attr("disabled", false);
	}
	/**
   *@summary Redirecciona al index
   */
	Exit() {
		window.location = rootPath + "Home/Index";
	}
}