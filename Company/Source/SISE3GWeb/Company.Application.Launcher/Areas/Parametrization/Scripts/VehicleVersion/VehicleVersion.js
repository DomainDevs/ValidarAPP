
$.ajaxSetup({ async: true });
class VehiculeVersionParametrization extends Uif2.Page {
	getInitialState() {
		$("input[type=text]").TextTransform(ValidatorType.UpperCase);
		$("#EngineQuantity").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(4)
		});
		$("#HorsePower").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(3)
		});
		$("#TonsQuantity").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(3)
		});
		$("#Weight").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(5)
		});
		$("#PassengerQuantity").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(5)
		});
		$("#MaxSpeedQuantity").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(3)
		});
		$("#DoorQuantity").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(3)
		});
		
		$("#Price").UifMask({
			pattern: VehiculeVersionParametrization.MaxLengthInput(18)
		});
  
	}
	bindEvents() {
		$('#inputVehicleVersion').on("buttonClick", VehiculeVersionParametrization.SearchVehicleVersion);
		$('#VehicleMakeServiceQueryModel').on("itemSelected", VehiculeVersionParametrization.GetModel);
        $('#SaveVehicleVersion').click(VehiculeVersionParametrization.ConfirmSaveModel)
		$('#AddNewVehicleVersion').click(VehiculeVersionParametrization.cleanForm)
		$('#ExitVehicleVersion').click(VehiculeVersionParametrization.redirectIndex)
		$('#DeleteVehicleVersion').click(VehiculeVersionParametrization.DeleteVehicleVersion)
		$('#ExcelVehicleVersion').click(VehiculeVersionParametrization.ExportFile)
        $('#VehicleVersionAcceptDelete').click(VehiculeVersionParametrization.ConfirmDeleteVersion);
        $("#Price").focusin(this.NotFormatMoneyIn);
        $("#Price").focusout(this.FormatMoneyOut);
    }

    FormatMoneyOut() {
        //parseInt(NotFormatMoney($("#inputLimit1").val()
        $(this).val(parseInt(NotFormatMoney($(this).val())));
        var price = $(this).val(FormatMoney($(this).val()));
        $("#Price").val(price.val());
    }

    NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }


    static SearchVehicleVersion() {       
		var search = $('#inputVehicleVersion').val()
		if (search.length < 3) {
			$.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelMinimumThreeCharacters });
		}
        else {
            $("#Description").prop("disabled", false);
            $("#VehicleMakeServiceQueryModel").prop("disabled", false);
            $("#VehicleModelServiceQueryModel").prop("disabled", false);
			request('Parametrization/VehicleVersion/SearchVehiculeVersionDescription', JSON.stringify({ Description: search }), 'POST', AppResources.ErrorSearchVehicleVersion, VehiculeVersionParametrization.SearchVehiculeVersionDescriptionSuccess);
		}		
	}
	static MaxLengthInput(max)
	{
		var maxLength = '';
		for (var i = 0; i < max; i++) {
			maxLength = maxLength + '0';
		}
		return maxLength;
	}
	static DeleteVehicleVersion() {
		var formVehicleVersion = $("#formVehicleVersion").serializeObject();
		if (formVehicleVersion.Id != "") {
            $.UifDialog('confirm', { 'message': Resources.Language.MessageConfirmDelete }, function (result) {
                if (result) {
                    ///<summary>
                    ///Se adiciono una línea (formVehicleVersion.Price = formVehicleVersion.Price.replace(/[.]+/g, "");) 
                    ///para dar formato al campo precio al momento de eliminar, ya que de no hacer esto 
                    ///el valor era null y el proceso de eliminación no se podía realizar. 
                    ///</summary>
                    ///<author>Diego Leon</author>
                    ///<date>11/07/2018</date>
                    ///<purpose>REQ_#079</purpose>
                    ///<returns></returns>
                    formVehicleVersion.Price = formVehicleVersion.Price.replace(/[.]+/g, "");
					request('Parametrization/VehicleVersion/DeleteVehicleVersion', JSON.stringify({ VehicleVersionViewModel: formVehicleVersion }), 'POST', AppResources.ErrorDeleteVehicleVersion, VehiculeVersionParametrization.successDelete);
					$("#VehicleVersionConfirmationDeleteModal").UifModal("hide");
				}
			});
			
		}
		else {
			$.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteNoSelected });
		}
	}
	static ExportFile() {
		//var MakeID = $('#VehicleMakeServiceQueryModel').val();
		//var ModelID = $('#VehicleModelServiceQueryModel').val();
		//if (MakeID == "" || ModelID== ""  )
		//{
		//	$.UifNotify('show', { 'type': 'info', 'message': AppResources.ParameterRequiredExportVersion });
		//	return;
		//}
		request('Parametrization/VehicleVersion/ExportFileVehicleVersion', JSON.stringify({ }), 'POST', AppResources.ErrorExportVehicleVersion, VehiculeVersionParametrization.ExportFileSuccess);
	}
	static ExportFileSuccess(data) {
		DownloadFile(data);
	}

	static ConfirmDeleteVersion() {
		var formVehicleVersion = $("#formVehicleVersion").serializeObject();
		if (formVehicleVersion.Id != "") {
		    formVehicleVersion.Price = formVehicleVersion.Price.replace(/[.]+/g, "");
			request('Parametrization/VehicleVersion/DeleteVehicleVersion', JSON.stringify({ VehicleVersionViewModel: formVehicleVersion }), 'POST', AppResources.ErrorDeleteVehicleVersion, VehiculeVersionParametrization.successDelete);
			$("#VehicleVersionConfirmationDeleteModal").UifModal("hide");
		}
		else {
			$.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteNoSelected });
		}
	}
	static successDelete(data) {
		if (data.type == ParametrizationStatus.Error) {
			$.UifNotify('show', {
				'type': 'danger',
				'message': data.message + '',
				'autoclose': false
			});
		}
		else {
			$.UifNotify('show', {
				'type': 'info',
				'message': data.message + '',
				'autoclose': false
			});
			VehiculeVersionParametrization.cleanForm();
		}
		
		
	}

    static SearchVehiculeVersionDescriptionSuccess(data) {
        ClearValidation("#formVehicleVersion");
		if (data != null) {
            if (data.length == 0) {
                $('#inputVehicleVersion').val(null);
				$.UifNotify('show', {
					'type': 'info',
					'message': AppResources.MessageNotFoundVersion + ''					
				});
			}
			if (data.length == 1) {
                VehiculeVersionParametrization.setDataForm(data[0])
                $('#inputVehicleVersion').val(null);
			}
			else if(data.length > 1){
				
				VehiculeVersionSearchParametrization.LoadResultsListView(data);
                dropDownSearchAdvVehicleVersion.show()
                
			}
		}
	}
    static GetModel(event, selectedItem) {
        $("#formVehicleVersion").valid();
		if (selectedItem.Id) {
			VehiculeVersionParametrization.GetModelByMake(selectedItem.Id).done(function (data) {
				if (data.success) {
					$("#VehicleModelServiceQueryModel").UifSelect({ sourceData: data.result });
				}
			});
		}
		else {
			$("#VehicleModelServiceQueryModel").UifSelect();
		}
	}
	static GetModelByMake(Id) {
		return $.ajax({
			url: rootPath + 'VehicleVersion/GetModelByMake',
			async: false,
			data:{ MakeID: Id},
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		}); 
	}

    static ConfirmSaveModel() {
        $("#formVehicleVersion").validate();
        if ($("#formVehicleVersion").valid()) {
            var formVehicleVersion = $("#formVehicleVersion").serializeObject();
            if (parseInt(formVehicleVersion.Id) > 0) {
                formVehicleVersion.StatusTypeService = ParametrizationStatus.Update;
            }
            else {
                formVehicleVersion.StatusTypeService = ParametrizationStatus.Create;
            }
            formVehicleVersion.IsImported = $('#IsImported').is(':checked')
            formVehicleVersion.LastModel = $('#LastModel').is(':checked')
            formVehicleVersion.Price = formVehicleVersion.Price.replace(/[.]+/g, "");
            
            request('Parametrization/VehicleVersion/SaveVehicleVersion', JSON.stringify({ VehicleVersionViewModel: formVehicleVersion }), 'POST', AppResources.ErrorSaveVehicleVersion, VehiculeVersionParametrization.successSave);
            $("#VehicleVersionConfirmationModal").UifModal("hide");
        }
		
	}
	static successSave(data) {
		VehiculeVersionParametrization.cleanForm();
		$.UifNotify('show', {
			'type': 'info',
			'message': data + ''
			
		});
	}
    static setDataForm(data) {
        VehiculeVersionParametrization.cleanForm();
		$('#Id').val(data.Id);
        $('#Description').val(data.Description);
        $("#Description").prop("disabled", true);
        $('#VehicleMakeServiceQueryModel').UifSelect("setSelected", data.VehicleMakeServiceQueryModel);
        //$("#VehicleMakeServiceQueryModel").prop("disabled", true);
        $("#VehicleMakeServiceQueryModel").UifSelect("disabled", true)
		VehiculeVersionParametrization.GetModelByMake(data.VehicleMakeServiceQueryModel).done(function (resultModel) {
			if (resultModel.success) {
                $("#VehicleModelServiceQueryModel").UifSelect({ sourceData: resultModel.result, selectedId: data.VehicleModelServiceQueryModel });
                //$("#VehicleModelServiceQueryModel").prop("disabled", true);
                $("#VehicleModelServiceQueryModel").UifSelect("disabled", true)
			}
		});
		$('#EngineQuantity').val(data.EngineQuantity);
		$('#HorsePower').val(data.HorsePower);
		$('#Weight').val(data.Weight);
		$('#TonsQuantity').val(data.TonsQuantity);
		$('#PassengerQuantity').val(data.PassengerQuantity);
		$('#VehicleFuelServiceQueryModel').UifSelect("setSelected", data.VehicleFuelServiceQueryModel);
		$('#VehicleBodyServiceQueryModel').UifSelect("setSelected", data.VehicleBodyServiceQueryModel);
		$('#Currency').UifSelect("setSelected", data.Currency);
		$('#VehicleTypeServiceQueryModel').UifSelect("setSelected", data.VehicleTypeServiceQueryModel);
		$('#VehicleTransmissionTypeServiceQueryModel').UifSelect("setSelected", data.VehicleTransmissionTypeServiceQueryModel);
		$('#MaxSpeedQuantity').val(data.MaxSpeedQuantity);
		$('#DoorQuantity').val(data.DoorQuantity);
        $('#Price').val(FormatMoney(data.Price)); 
		if (data.IsImported === true) {
			$('#IsImported').prop('checked', true);
		}
		else {
			$('#IsImported').prop('checked', false);
		}
		if (data.LastModel === true) {
			$('#LastModel').prop('checked', true);
		}
		else {
			$('#LastModel').prop('checked', false);
		}
	}
	static cleanForm() {
		$('#LastModel').prop('checked', false);
		$('#IsImported').prop('checked', false);
		$('#Id').val('');
		$('#Description').val('');
        $("#VehicleMakeServiceQueryModel").UifSelect("disabled", false)
        $("#VehicleModelServiceQueryModel").UifSelect("disabled", false)
        $('#VehicleModelServiceQueryModel').UifSelect("setSelected", null);
        $('#VehicleMakeServiceQueryModel').UifSelect("setSelected", null);
        $("#Description").prop("disabled", false);
        //$("#VehicleMakeServiceQueryModel").prop("disabled", false);
        //$("#VehicleModelServiceQueryModel").prop("disabled", false);
        
		$('#EngineQuantity').val('');
		$('#HorsePower').val('');
		$('#Weight').val('');
		$('#TonsQuantity').val('');
		$('#PassengerQuantity').val('');
		$('#VehicleFuelServiceQueryModel').UifSelect("setSelected", null);
		$('#VehicleBodyServiceQueryModel').UifSelect("setSelected", null);
		$('#Currency').UifSelect("setSelected", null);
		$('#VehicleTypeServiceQueryModel').UifSelect("setSelected", null);
		$('#VehicleTransmissionTypeServiceQueryModel').UifSelect("setSelected", null);
		$('#MaxSpeedQuantity').val('');
		$('#DoorQuantity').val('');
		$('#Price').val('');
		$('#IsImported').val('');
        $('#LastModel').val('');
        $('#inputVehicleVersion').val(null);
        
		ClearValidation("#formVehicleVersion");

	}
	static redirectIndex() {
		window.location = rootPath + "Home/Index";
	}

}