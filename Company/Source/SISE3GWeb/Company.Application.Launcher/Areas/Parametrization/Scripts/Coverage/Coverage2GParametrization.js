
class Coverage2G {
	static GetCoverages2G() {
		return $.ajax({
			type: 'GET',
			url: rootPath + 'Parametrization/Coverage/GetCoverages2G',
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}

	static GetInsuredObjectVehicle() {
		return $.ajax({
			type: 'GET',
			url: rootPath + 'Parametrization/Coverage/GetInsuredObjectVehicle',
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}
}

var glbCoverages2G = [];
class Coverage2GParametrization extends Uif2.Page {
	getInitialState() {

		Coverage2G.GetInsuredObjectVehicle().done(function (data) {
			if (data.success) {
				$("#selectInsuranceObject2G").UifSelect({ sourceData: data.result });
				$("#selectInsuranceObject2G").UifSelect("setSelected", data.result[0].Id);
				$("#selectInsuranceObject2G").UifSelect("disabled", true); //X funcionalidad de framework

			}
			else {
				$.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
			}
		});

		//Coverage2G.GetCoverages2G().done(function (data) {
		//	if (data.success) {
		//		glbCoverages2G = data.result;
		//		$("#selectCoCoverage2G").UifSelect({ sourceData: glbCoverages2G });
		//		$("#selectCoCoverage2G").UifSelect("setSelected", null);

		//	}
		//	else {
		//		$.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
		//	}

		//});

	}

	static LoadSubTitles() {
        if ($("#Coverage2GId").val() != "" && $("#Coverage2GId").val() != undefined) {
            if (glbCoverages2G.length > 0) {
                var coverage2GDesc = glbCoverages2G.filter(x => x.CoverageId2G == $("#Coverage2GId").val()).shift().Description;
                $("#selectCoverage2G").text(coverage2GDesc);
            }
		} else {
			$("#selectCoverage2G").text('');
		}
	}

	bindEvents() {
		$("#btnCoverage2G").click(this.OpenModal);
		$("#btnModalCoverage2GSave").click(this.SaveCoverage2G);
	}


	SaveCoverage2G() {
		if ($("#formCoverage2G").valid()) {
			var selectedCverage2G = $("#selectCoCoverage2G").UifSelect("getSelectedSource");
			if (selectedCverage2G == undefined) {
				$("#Coverage2GId").val(null);
				$("#InsuredObject2GId").val(null);
				$("#LineBusiness2GId").val(null);
				$("#SubLineBusiness2GId").val(null);

			}
			else {
				$("#Coverage2GId").val(parseInt(selectedCverage2G.CoverageId2G));
				$("#InsuredObject2GId").val(parseInt(selectedCverage2G.InsuredObject2G));
				$("#LineBusiness2GId").val(parseInt(selectedCverage2G.LineBusiness2G));
				$("#SubLineBusiness2GId").val(parseInt(selectedCverage2G.SubLineBusiness2G));
			}
			Coverage2GParametrization.LoadSubTitles();
			$('#modelCoverage2G').UifModal('hide');
		}
	}

	OpenModal() {
		if ($("#formCoverage").valid()) {
			var formSerialize = $("#formCoverage").serializeArray();
			var coverageDescription = formSerialize.find(function (value) {
				return value.name == "Description";
			});
			if (coverageDescription != undefined) {
				var coverage2GId = formSerialize.find(function (value) {
					return value.name == "Coverage2GId";
				});
				var setCoverage2G = false;
				if (coverage2GId != undefined) {
					if (coverage2GId.value != null && coverage2GId.value != undefined && coverage2GId.value != "" && coverage2GId.value != "NaN") {
						setCoverage2G = true;
					}
				}
				if (setCoverage2G) {
					$("#selectCoCoverage2G").UifSelect("setSelected", coverage2GId.value);

				}
				else {
					$("#selectCoCoverage2G").UifSelect("setSelected", null);
				}
				$("#modelCoverage2G").UifModal('showLocal', 'Homologación 2G: ' + coverageDescription.value);
			}

		}
	}
}
