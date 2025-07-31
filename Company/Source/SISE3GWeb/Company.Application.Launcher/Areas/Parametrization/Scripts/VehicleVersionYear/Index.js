class VehicleVersionYearParametrization extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });
        request('Parametrization/VehicleVersionYear/GetMakes', null, 'GET', AppResources.ErrorGetMakesCONNEX, VehicleVersionYearParametrization.setMakes);
        request('Parametrization/VehicleVersionYear/GetCurrencies', null, 'GET', AppResources.ErrorGetCurrencyCONNEX, VehicleVersionYearParametrization.setCurrencies);
        $("#Year").ValidatorKey(ValidatorType.Number, 0, 0);
        $("#Price").OnlyDecimals(2);        
        $("#Status").val(ParametrizationStatus.Create);
    }
    bindEvents() {
        $("#btnExit").on("click", this.exit);
        $("#MakeId").on("itemSelected", VehicleVersionYearParametrization.changeMake);
        $("#ModelId").on("itemSelected", VehicleVersionYearParametrization.changeModel);
        $("#VersionId").on("itemSelected", VehicleVersionYearParametrization.changeVersion);
        $("#CurrencyId").on("itemSelected", VehicleVersionYearParametrization.changeCurrency);
        $("#Year").on("focusout", VehicleVersionYearParametrization.changeYear)
        $("#btnClear").on("click", VehicleVersionYearParametrization.clear);
        $("#btnSave").on("click", VehicleVersionYearParametrization.save);
        $("#btnDelete").on("click", VehicleVersionYearParametrization.delete);
        $("#btnShowSearchAdv").on("click", VehicleVersionYearParametrizationAdv.showDropDown);
        $("#btnExport").click(this.exportExcel);
    }
    exit() {
        window.location = rootPath + "Home/Index";
    }   

    static changeMake(event, selectedItem) {
        $("#formVehicleVersionYear").valid();
        VehicleVersionYearParametrization.setVersions({ items: null }); //Siempre debe limipar la version, sin importar cual marca se selecciono
        VehicleVersionYearParametrization.setModels({ items: null }); 
        if (selectedItem.Id > 0) {
            request('Parametrization/VehicleVersionYear/GetModelsByMakeId', JSON.stringify({ makeId: selectedItem.Id }), 'POST', AppResources.ErrorGetModelsCONNEX, VehicleVersionYearParametrization.setModels)
        }
        else
        {
            VehicleVersionYearParametrization.setModels({ items: null });
        }
    }

    static changeCurrency(event, selectedItem) {
        $("#formVehicleVersionYear").valid();
        
    }

    static changeModel(event, selectedItem) {
        $("#formVehicleVersionYear").valid();
        var selectedItemMake = $("#MakeId").UifSelect("getSelected");
        VehicleVersionYearParametrization.setVersions({ items: null }); //Siempre debe limipar la version, sin importar cual marca se selecciono
        if (selectedItem.Id > 0 && selectedItemMake>0) {
            request('Parametrization/VehicleVersionYear/GetVersionsByMakeIdModelId', JSON.stringify({ makeId: selectedItemMake, modelId: selectedItem.Id }), 'POST', AppResources.ErrorGetVersionsCONNEX, VehicleVersionYearParametrization.setVersions)
        }
        else
        {
            VehicleVersionYearParametrization.setVersions({ items: null });
        }
    }

    static changeVersion(event, selectedItem) {
        $("#formVehicleVersionYear").valid();
        VehicleVersionYearParametrization.getVehicleExistent();
    }

    static changeYear() {
        VehicleVersionYearParametrization.getVehicleExistent();
    }

    static getVehicleExistent() {
        var form= $("#formVehicleVersionYear").serializeObject();
        if (parseInt(form.MakeId) > 0 && parseInt(form.ModelId) && parseInt(form.VersionId) && parseInt(form.Year) > 0)
        {
            request('Parametrization/VehicleVersionYear/GetVehicleVersionYearServiceModel', JSON.stringify({ vehicle: form }), 'POST', AppResources.ErrorGetVehicleRelationCONNEX, VehicleVersionYearParametrization.setVehicleWhithoutSelect);
        }

    }

    static formatMoneyPrice() {
        var price = $("#Price").val();
        $("#Price").val(FormatMoney(price));
    }

    static formatOriginalPrice() {
        var price = $("#Price").val();
        price = price.replace(".","");
        $("#Price").val(price);
    }

    static setCurrencies(data) {
        $("#CurrencyId").UifSelect({ sourceData: data});
    }

    static setMakes(data) {
        $("#MakeId").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
        if (parseInt(data.selectedId) >= 0)
        {
            $("#MakeId").UifSelect('disabled', true); 
        }
    }

    static setModels(data) {
        $("#ModelId").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
        if (parseInt(data.selectedId) >= 0) {
            $("#ModelId").UifSelect('disabled', true);
        }
    }

    static setVersions(data) {
        $("#VersionId").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
        if (parseInt(data.selectedId) >= 0) {
            $("#VersionId").UifSelect('disabled', true);
        }
    }

    static setVehicleSelect(data) {
        $("#MakeId").UifSelect("setSelected", data[0].VehicleMakeServiceQueryModel.Id);
        request('Parametrization/VehicleVersionYear/GetModelsByMakeId', JSON.stringify({ makeId: data[0].VehicleMakeServiceQueryModel.Id, selectedId: data[0].VehicleModelServiceQueryModel.Id }), 'POST', AppResources.ErrorGetModelsCONNEX, VehicleVersionYearParametrization.setModels);
        request('Parametrization/VehicleVersionYear/GetVersionsByMakeIdModelId', JSON.stringify({ makeId: data[0].VehicleMakeServiceQueryModel.Id, modelId: data[0].VehicleModelServiceQueryModel.Id, selectedId: data[0].VehicleVersionServiceQueryModel.Id }), 'POST', AppResources.ErrorGetVersionsCONNEX, VehicleVersionYearParametrization.setVersions);
        $("#Year").val(data[0].Year);        
    }

    static setVehicleWhithoutSelect(data) {
        if (data.length === 1)
        {   
            $("#Status").val(ParametrizationStatus.Update);
            $("#CurrencyId").UifSelect("setSelected", data[0].CurrencyServiceQueryModel.Id);
            $("#MakeId").UifSelect('disabled', true);
            $("#ModelId").UifSelect("disabled", true);
            $("#VersionId").UifSelect("disabled", true);            
            $("#Year").prop("disabled", true);
            $("#Price").val(String(data[0].Price).replace('.', ','));
            VehicleVersionYearParametrization.formatMoneyPrice();
        }
    }

    static clear() {       
        $("#Status").val(ParametrizationStatus.Create);
        $("#MakeId").UifSelect('disabled', false);
        $("#ModelId").UifSelect("disabled", false);
        $("#VersionId").UifSelect("disabled", false);
        $("#CurrencyId").UifSelect("disabled", false);
        $("#Year").prop("disabled", false);
        $("#MakeId").UifSelect("setSelected", null);
        $("#CurrencyId").UifSelect("setSelected", null);
        $("#Year").val("");
        $("#Price").val("");        
        VehicleVersionYearParametrization.setModels({ items: null });
        VehicleVersionYearParametrization.setVersions({ items: null });
        ClearValidation("#formVehicleVersionYear");
    }

    static save() {
        VehicleVersionYearParametrization.formatOriginalPrice();
        if ($("#formVehicleVersionYear").valid())
        {
            request('Parametrization/VehicleVersionYear/Save', JSON.stringify({ vehicleVM: $("#formVehicleVersionYear").serializeObject() }), 'POST', AppResources.ErrorSaveVehicleCONNEX, VehicleVersionYearParametrization.setResultSave);            
        }
    }

    static delete() {
		if (parseInt($("#Status").val()) === ParametrizationStatus.Update) {
            $.UifDialog('confirm', { 'message': AppResources.SureWantDeleteRecord }, function (result) {
				if (result) {
					$("#Status").val(ParametrizationStatus.Delete);
					request('Parametrization/VehicleVersionYear/Save', JSON.stringify({ vehicleVM: $("#formVehicleVersionYear").serializeObject() }), 'POST', AppResources.ErrorDeleteVehicleCONNEX, VehicleVersionYearParametrization.setResultSave);
				}
			});
		}
		else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorNotSelectedRegisterDelete, 'autoclose': true });
		}
    }

    static setResultSave(data) {        
        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        VehicleVersionYearParametrization.clear();
    }

    exportExcel() {
        var form = $("#formVehicleVersionYear").serializeObject();
        if (parseInt(form.MakeId) >= 0 && parseInt(form.ModelId) >= 0 && parseInt(form.VersionId))
        {
            request('Parametrization/VehicleVersionYear/GenerateFileToExport', JSON.stringify({ makeId: form.MakeId, modelId: form.ModelId, versionId: form.VersionId }), 'POST', AppResources.ErrorGeneratingExcelFile, VehicleVersionYearParametrization.generateFileToExport);
        }
        else
        {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ParameterRequiredExportVehicle, 'autoclose': true });
        }
    }

    static generateFileToExport(urlFile) {
        DownloadFile(urlFile);        
    }
}