$.ajaxSetup({ async: true });
var surcharge = {};
var surchargeType = null;//por ahora
var surchargeIndex = null;
var surchagesTotal = 0;
var comboList = [];

class UnderwritingSurcharge extends Uif2.Page {
    getInitialState() {
        surchagesTotal = 0;
    
        $("#listSurcharges").UifListView({
            displayTemplate: "#SurchargesTemplate",
            source: null,
            delete: true,
            customAdd: true,
            customEdit: true,
            height: 150,
            deleteCallback: this.DeleteItemSurcharge
        });
    }

    //Seccion Eventos
    bindEvents() {
        $("#btnSurcharges").on("click", UnderwritingSurcharge.SurchargeLoad);
        $("#btnExitSurcharges").on("click", UnderwritingSurcharge.SurchargeClose);
        $("#btnNewSurcharge").on("click", UnderwritingSurcharge.Clear);
        $("#btnAcceptSurcharge").click(UnderwritingSurcharge.AcceptSurcharge);
        $("#listSurcharges").on('rowDelete', UnderwritingSurcharge.DeleteItemSurcharge);
        $("#selectDesSurcharge").on('itemSelected', (event, result) => { this.SelectItemSurcharge(event, result) });
        $("#btnSaveSurcharges").on('click', UnderwritingSurcharge.SaveSurcharges)

        //Solo números con dos decimales
        $("#inputRate").on("keypress keyup blur", function (event) {
            //{0,2} => solo 3 a izquierda/derecha de "."  
            this.value = this.value.match(/\d{0,3}(\.\d{0,2})?/)[0];
            this.value(this.value.replace(/[^0-9\.]/g, ''));
        });
    }
        
    static SurchargeLoad() {    
        if (glbPolicy.Id == 0) {
            if ($("#formUnderwriting").valid()) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
            }          
        }
        else {
            UnderwritingSurcharge.Clear;
            Underwriting.ShowPanelsIssuance(MenuType.Surcharges);
            //Obtener recargos a
            request('Underwriting/Underwriting/GetSurcharges', null, 'GET', AppResources.ErrorConsultingSurcharge, UnderwritingSurcharge.GetComboSurcharges);
            UnderwritingSurcharge.GetSurchargesQuotation();       
        }
    }

    //Llena la lista de recargos que pertenezcan al temporal
    static GetSurchargesQuotation() {
        SurchargesRequest.GetSurchargesQuotation(glbPolicy.Id).done(function (data) {
            $("#listSurcharges").UifListView({ source: null, displayTemplate: "#SurchargesTemplate", delete: true, customAdd: true, deleteCallback: UnderwritingSurcharge.DeleteItemSurcharge, height: 150 });
            if (data != AppResources.ErrorGetSurcharge) {
                data.result.forEach(item => {
                    switch (item.Type) {
                        case SurchargeType.Percentage:
                            item.RateDescription = SurchargeDescription.Percentage;
                            break;
                        case SurchargeType.Permilage:
                            item.RateDescription = SurchargeDescription.Permilage;
                            break;
                        case SurchargeType.FixedValue:
                            item.RateDescription = SurchargeDescription.FixedValue;
                            break;
                    }
                    //solo 2 decimales pero seguir usando string del modelo
                    item.Rate = String(parseFloat(item.Rate).toFixed(2));
                    $("#listSurcharges").UifListView("addItem", item);
                });              
            }
            UnderwritingSurcharge.CalculateTotalSurcharge();
        });    
    }

    //LLena el dropdownlist con todos los recargos registrados
    static GetComboSurcharges(data) {
        var comboSurcharges = { sourceData: data };
        $("#selectDesSurcharge").UifSelect(comboSurcharges);

        //Almacenamos para usar global
        comboList = data;
    }

    //Envia a guardar en la BD los items de la lista
    static SaveSurcharges() {
        SurchargesRequest.SaveSurcharges(glbPolicy.Id, $("#listSurcharges").UifListView('getData')).done(function (data) {
            if (data.success) {
                Underwriting.HidePanelsIssuance(MenuType.Surcharges);
                glbPolicy.SummaryComponent.Surcharge = data.result.Surcharge;
                Underwriting.LoadSubTitles(5);
                if (glbPolicy.TemporalType == TemporalType.Quotation) {
                    Underwriting.SaveTemporal(false);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveSurcharges, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveSurcharges, 'autoclose': true });
        });
    }


    //Al seleccionar un item del dropdownlist
    SelectItemSurcharge(event, result) {
        comboList.forEach(item => {
            if (item.Id === parseInt(result.Id)) {
                //capturar Id surcharge
                surchargeIndex = item.Id;

                switch (item.Type) {
                    case SurchargeType.Percentage:
                        $("#inputRateType").val("" + SurchargeDescription.Percentage);
                        break;
                    case SurchargeType.Permilage:
                        $("#inputRateType").val("" + SurchargeDescription.Permilage);
                        break;
                    case SurchargeType.FixedValue:
                        $("#inputRateType").val("" + SurchargeDescription.FixedValue);
                        break;
                }
                //Desbloquear tasa
                $("#inputRate").prop('disabled', false);
                $("#inputRate").val("" + parseFloat(item.Rate).toFixed(2));
            }
        });    
    }

    static Clear() {
        $("#selectPrefixSurcharge").val(null);
        $("#inputRateType").val(null);
        $("#inputRate").val(null);
        $("#selectDesSurcharge").UifSelect("setSelected", null);
        $("#inputRate").prop('disabled', true);
    }

    //verifica antes si se puede guardar el item seleccionado del dropdownlist al uiflistview
    static AcceptSurcharge() {
        if ($("#formSurcharge").valid()) {
            if (surchargeIndex != null) {
                var lista = $("#listSurcharges").UifListView('getData');
                var ifExist = lista.filter(function (item) {
                    return item.Id == surchargeIndex;
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistSurcharge, 'autoclose': true });
                }
                else {
                    UnderwritingSurcharge.AddSurchargeList();
                }
            }
            UnderwritingSurcharge.Clear();
        }          
    }

    //Agrega un item seleccionado del dropdownlist en el uiflistview
    static AddSurchargeList() {
        //agregamos surcharge del combo al uiflistview
        var surchargeSearch = comboList.filter(function (item) {
            return item.Id == surchargeIndex;
        });
        var surchargeAdd = surchargeSearch[0];
        switch (surchargeAdd.Type) {
            case SurchargeType.Percentage:
                surchargeAdd.RateDescription = SurchargeDescription.Percentage;
                break;
            case SurchargeType.Permilage:
                surchargeAdd.RateDescription = SurchargeDescription.Permilage;
                break;
            case SurchargeType.FixedValue:
                surchargeAdd.RateDescription = SurchargeDescription.FixedValue;
                break;
        }
        surchargeAdd.Rate = ($("#inputRate").val() == '') ? String(0) : parseFloat($("#inputRate").val()).toFixed(2);
        surchargeAdd.StatusTypeService = ParametrizationStatus.Create;
        $("#listSurcharges").UifListView("addItem", surchargeAdd);

        UnderwritingSurcharge.CalculateTotalSurcharge();
    }

    static CalculateTotalSurcharge() {
        surchagesTotal = 0;
        var surchargeList = $("#listSurcharges").UifListView('getData');
        $.each(surchargeList, function (index, value) {
            surchagesTotal += parseFloat(value.Rate);
        });
        $("#surchagesTotal").text(surchagesTotal.toFixed(2));//Solo mostrar dos decimales
    }

    static DeleteItemSurcharge(deferred, result) {
        deferred.resolve();
        if (result.Id !== "" && result.Id !== 0) //Se elimina unicamente si existe en DB
        {
            result.StatusTypeService = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listSurcharges").UifListView("addItem", result);
        }
        UnderwritingSurcharge.Clear();
    }

    static SurchargeClose() {
        UnderwritingSurcharge.Clear();
        $('#modalSurcharges').UifModal("hide");     
    }
}