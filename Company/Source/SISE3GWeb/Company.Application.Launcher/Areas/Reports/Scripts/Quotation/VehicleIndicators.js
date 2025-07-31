var EXAMPLE = [{ "Id": 1, "Empresa": "Remplazar con Ajax1" }, { "Id": 2, "Empresa": "Remplazar con Ajax2" }, { "Id": 3, "Empresa": "Remplazar con Ajax3" }, { "Id": 4, "Empresa": "Remplazar con Ajax4" }];

class VehicleIndicators extends Uif2.Page {

    getInitialState() {
        $("#dateStart").UifDatepicker('setValue', new Date());
        $("#dateEnd").UifDatepicker('setValue', new Date());
    }
    bindEvents() {
        //Porterior Remplar con ajax
        $("#searchAgent").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });
        $("#SelectProduct").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });
        $("#SelectMake").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });
        $("#SelectTypeVehicle").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });
        $("#SelectEffectiveSale").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });
        VehicleIndicators.GetValueSelectOffice();
        //Fin Porterior Remplar con ajax



        $("#SelectModel").UifSelect("disabled", true);
        $("#SelectBodyType").UifSelect("disabled", true);
        $("#SelectTypeService").UifSelect("disabled", true);
        $("#SelectSalesPoint").UifSelect("disabled", true);
        $('#btnExitQuotationAuto').on('click', this.exit);
        $('#btnSearchQuotationAuto').on('click', VehicleIndicators.SearchVehicleIndicators);
        $('#btnNewQuotationAuto').on('click', VehicleIndicators.clearFormSearch);
        $('#searchAgent').on('search', (event, value) => { this.searchAgent(value); });
        $('#dateStart').on('datepicker.change', (event, date) => { VehicleIndicators.DateValidator(date, "dateStart"); });
        $('#dateEnd').on('datepicker.change', (event, date) => { VehicleIndicators.DateValidator(date, "dateEnd"); });
        $('#SelectOffice').on('itemSelected', (event, selectedItems) => { VehicleIndicators.LoadSelectSalesPoint(event, selectedItems); });
        $('#SelectMake').on('itemSelected', (event, selectedItem) => { VehicleIndicators.LoadSelectModel(event, selectedItem); });
        $('#SelectTypeVehicle').on('itemSelected', (event, selectedItem) => { VehicleIndicators.LoadSelectBodyType(event, selectedItem); });
        $('#SelectBodyType').on('itemSelected', (event, selectedItem) => { VehicleIndicators.LoadSelectTypeService(event, selectedItem); });


    }
    exit() {
        window.location = rootPath + "Home/Index";
    }
    searchAgent(value) {
        if (!(value.length < 3)) {
            $.UifNotify('show', { 'type': 'info', 'message': "AjaxSearch" })
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.Agent + ": " + AppResources.Minimo3CharactersSearch })
        }
    }
    static clearFormSearch() {
        VehicleIndicators.GetValueSelectOffice();
        $('#searchAgent').val('');
        VehicleIndicators.DisabledSelectSalesPoint();
        $("#SelectProduct").UifSelect("setSelected", null);
        $("#SelectMake").UifSelect("setSelected", null);
        VehicleIndicators.DisabledSelectModel();
        $("#SelectTypeVehicle").UifSelect("setSelected", null);
        VehicleIndicators.DisabledBodytype();
        VehicleIndicators.DisabledTypeService();
        $("#dateStart").UifDatepicker('setValue', new Date());
        $("#dateEnd").UifDatepicker('setValue', new Date());
        $('#Quotation').val('');
        $("#SelectEffectiveSale").UifSelect("setSelected", null);
    }
    static GetValueSelectOffice() {
        $("#SelectOffice").UifMultiSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa" });//Porterior Remplar con ajax
    }


    static DisabledSelectSalesPoint() {
        $("#SelectSalesPoint").UifSelect("disabled", true);
        $("#SelectSalesPoint").UifSelect("setSelected", null);
    }
    static GetValueSelesPoint(SelectedOffices) {
        $("#SelectSalesPoint").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });//Porterior Remplar con ajax
    }

    static DisabledSelectModel() {
        $("#SelectModel").UifSelect("disabled", true);
        $("#SelectModel").UifSelect("setSelected", null);
    }
    static GetValueSelectModel(SelectedMake) {
        $("#SelectModel").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });//Porterior Remplar con ajax
    }

    static DisabledBodytype() {
        $("#SelectBodyType").UifSelect("disabled", true);
        $("#SelectBodyType").UifSelect("setSelected", null);
    }
    static GetValueSelectBodyType(SelectedTypeVehicle) {
        $("#SelectBodyType").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });//Porterior Remplar con ajax
    }

    static DisabledTypeService() {
        $("#SelectTypeService").UifSelect("disabled", true);
        $("#SelectTypeService").UifSelect("setSelected", null);
    }
    static GetValueSelectTypeService(SelectedTypeVehicle) {
        $("#SelectTypeService").UifSelect({ sourceData: EXAMPLE, id: "Id", name: "Empresa", native: false, filter: true });//Porterior Remplar con ajax
    }

    static SearchVehicleIndicators() {

        if (!($("#dateStart").UifDatepicker('getValue') <= $("#dateEnd").UifDatepicker('getValue'))) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.EventDates })
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': "Ajax" + AppResources.Query })
        }
    }
    static DateValidator(Date, input) {
        $("#TableVehicleIndicators").UifDataTable({ sourceData: [] })
        var dateStart = $("#dateStart").UifDatepicker('getValue');
        var dateEnd = $("#dateEnd").UifDatepicker('getValue');
        if (!(dateStart <= dateEnd) || Date == undefined) {
            if (input == "dateStart") {
                $("#" + input + "").UifDatepicker('setValue', dateEnd);
            } else {
                $("#" + input + "").UifDatepicker('setValue', dateStart);
            }
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.EventDates })
        }
    }
    static LoadSelectSalesPoint(event, selectedItems) //Busca items y habilita el select de Puntos de venta
    {
        if ($('#SelectOffice').UifMultiSelect('getSelected') == null) {
            VehicleIndicators.DisabledSelectSalesPoint();
        }
        else {
            VehicleIndicators.GetValueSelesPoint(JSON.stringify($('#SelectOffice').UifMultiSelect('getSelected')));

        }
    }
    static LoadSelectModel(event, selectedItems) //Busca items y habilita el Modelo
    {
        if ($('#SelectMake').UifMultiSelect('getSelected')[0] == "") {
            VehicleIndicators.DisabledSelectModel();
        }
        else {
            VehicleIndicators.GetValueSelectModel(JSON.stringify($('#SelectMake').UifMultiSelect('getSelected')));


        }
    }
    static LoadSelectBodyType(event, selectedItems) //Busca items y habilita el select de Tipo de Carroceria
    {
        if ($('#SelectTypeVehicle').UifMultiSelect('getSelected')[0] == "") {
            VehicleIndicators.DisabledBodytype();
            VehicleIndicators.DisabledTypeService();
        }
        else {
            VehicleIndicators.GetValueSelectBodyType(JSON.stringify($('#SelectTypeVehicle').UifMultiSelect('getSelected')));
        }
    }
    static LoadSelectTypeService(event, selectedItems) //Busca items y habilita el select de Tipo de servicio
    {
        if ($('#SelectBodyType').UifMultiSelect('getSelected')[0] == "") {
            VehicleIndicators.DisabledTypeService();
        }
        else {
            VehicleIndicators.GetValueSelectTypeService(JSON.stringify($('#SelectBodyType').UifMultiSelect('getSelected')));
        }
    }

}