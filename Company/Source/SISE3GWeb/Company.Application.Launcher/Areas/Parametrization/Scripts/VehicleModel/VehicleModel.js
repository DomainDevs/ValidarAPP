var glbVehicleModelDelete = [];

$.ajaxSetup({ async: true });

class ModelParametrization extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }
    bindEvents() {

        $('#btnSaveModel').click(this.SaveModel);
        $('#btnNewVehicleModel').click(ModelParametrization.cleanForm);
        $('#inputVehicleModel').on("buttonClick", ModelParametrization.SearchVehicleModel);
        $('#btnDeleteVehicleModel').click(ModelParametrization.DeleteVehicleModel)
        $('#VehicleVersionAcceptDelete').click(ModelParametrization.ConfirmDeleteVersion);
        $('#btnExport').click(this.exportExcel);
        $('#btnExit').click(ModelParametrization.redirectIndex)

    }
    SaveModel(e) {
        $("#formVehicleModel").validate();
        if ($("#formVehicleModel").valid()) {
            e.stopPropagation()
            var form = $("#formVehicleModel").serializeObject();
            if (form.Id == "") {
                form.StatusTypeService = ParametrizationStatus.Create;
            }
            else {
                form.StatusTypeService = ParametrizationStatus.Update;
            }
            request('Parametrization/VehicleModel/SaveVehicleModel', JSON.stringify({ listvehicleModelViewModel: form }), 'POST', AppResources.ErrorSavePaymentMethod,
                ModelParametrization.successSave);
        }
    }
    static cleanForm() {

		$('#DescriptionModel').val('');
		$('#inputVehicleModel').val('');		
        $('#SmallDescriptionModel').val('');
        $('#MakeId_Id').UifSelect("setSelected", null);
        $('#SmallDescriptionModel').attr('disabled', false)
        $('#MakeId_Id').attr('disabled', false)
        $('#DescriptionModel').attr('disabled', false)
        $('#Id').val('');        
        ClearValidation("#formVehicleModel");

    }
    static successSave(data) {

        ModelParametrization.cleanForm();
        $.UifNotify('show', {
			'type': 'info',
            'message': data + '',
            'autoclose': true
        });
    }

    static setDataForm(data) {
        ClearValidation("#formVehicleModel");
        $('#DescriptionModel').val(data.DescriptionModel);
        $('#SmallDescriptionModel').val(data.SmallDescriptionModel);
        $('#MakeId_Id').UifSelect("setSelected", data.MakeId_Id); 
        $('#Id').val(data.Id);

        $('#SmallDescriptionModel').attr('disabled', true)
        $('#MakeId_Id').attr('disabled', true)
    }

    static DeleteVehicleModel() {

        var form = $("#formVehicleModel").serializeObject();
        if (form.Id != "") {
            $.UifDialog('confirm', { 'message': Resources.Language.ConfirmDelete }, function (result) {
                if (result) {
                    ModelParametrization.ConfirmDeleteVersion();
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.IsNoDataToEliminate });
        }
    }
    static ConfirmDeleteVersion() {
        var form = $("#formVehicleModel").serializeObject();
        form.StatusTypeService = ParametrizationStatus.Delete;

        if (form.Id != "") {

			request('Parametrization/VehicleModel/DeleteVehicleModel', JSON.stringify({ vehicleModelView: form }), 'POST', AppResources.ErrorDeleteVehicleModel, ModelParametrization.successDelete);
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
				'message': AppResources.Error,
				'autoclose': true
            });
        }
        else {
            $.UifNotify('show', {
                'type': 'success',
				'message': AppResources.ModelEliminated,
				'autoclose': true
            });
            ModelParametrization.cleanForm();
        }


    }
    static SearchVehicleModel() {
        var search = $('#inputVehicleModel').val()
        if (search.length < 2) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelMinimumThreeCharacters });
        }
        else {
			request('Parametrization/VehicleModel/SearchVehiculeModelDescription/' + search, JSON.stringify({ Description: search }), 'POST', AppResources.ErrorSearchVehicleVersion, ModelParametrization.SearchVehiculeModelDescriptionSuccess);
			
        }
    }
    static SearchVehiculeModelDescriptionSuccess(data) {

        console.log(data);
        if (data != null && data != AppResources.Error) {
            if (data.length > 1) {
                VehiculeModelParametrization.ShowAdvancedSearch();
				VehiculeModelParametrization.LoadResultsListView(data);
				ModelParametrization.cleanForm();
            }
			else {
				ModelParametrization.cleanForm();
				ModelParametrization.setDataForm(data[0])
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
        }
    }
    exportExcel() {
        request('Parametrization/VehicleModel/GenerateFileToExport', null, 'GET', AppResources.ErrorGeneratingExcelFile, ModelParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }
    static redirectIndex() {
        window.location = rootPath + "Home/Index";
    }
}
