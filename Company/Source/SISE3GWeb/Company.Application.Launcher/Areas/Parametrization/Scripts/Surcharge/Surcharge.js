$.ajaxSetup({ async: true });
var Surcharge = {};
var glbSurcharge = {};
var inputSearch = "";
var surchargeIndex = null;
var status = 0;
var formSurcharge = {};

class SurchargeParametrization extends Uif2.Page {
    getInitialState() {

        $("#milaje").hide();
        $("#importe").hide();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listViewSearchAdvanced").UifListView({ displayTemplate: "#SurchargeTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        request('Parametrization/Surcharge/GetRateTypes', null, 'GET', Resources.Language.ErrorSearch, SurchargeParametrization.GetRateTypes);
        request('Parametrization/Surcharge/GetSurcharge', null, 'GET', AppResources.ErrorConsultingSurcharge, SurchargeParametrization.GetSurcharge);

    }
    /////////Eventos////////////////////////////////////
    bindEvents() {
        $("#btnShowAdvanced").on("click", this.ShowAdvanced);
        $('#btnExit').click(this.Exit);
        $('#btnExport').click(this.SendExcelSurcharge);
        $('#btnNewSurcharge').on('click', this.NewLoad);
        $('#btnSaveSurcharge').on('click', this.SaveSurcharge);
        $('#btnSurchargeAccept').on('click', this.AddItemSurcharge);        
        $('#listViewSearchAdvanced').on('rowEdit', SurchargeParametrization.ShowData);      
        $("#selectType").on('itemSelected', (event, result) => { this.SelectType(event, result) });
    }
    SelectType(event, result) {
        $("#inputRate").val('');
        $("#inputRateM").val('');
        $("#inputRateI").val('');

        if (result.Id == 1) {
            $("#porcentaje").show();
            $("#milaje").hide();
            $("#importe").hide();

        }
        if (result.Id == 2) {
            $("#porcentaje").hide();
            $("#milaje").show();
            $("#importe").hide();
        }
        if (result.Id == 3) {
            $("#porcentaje").hide();
            $("#milaje").hide();
            $("#importe").show();
        }
    }   

    static GenerateFileToExport(data) {
        DownloadFile(data);
    }
    ///////////Creacion del Archivo Excel/////////////
    SendExcelSurcharge() {
        request('Parametrization/Surcharge/GenerateFileToExport', null, 'GET', AppResources.ErrorConsultingSurcharge, SurchargeParametrization.GenerateFileToExport);
    }
    static GetRateTypes(data) {
        let comboConfig = { sourceData: data };
        $("#selectType").UifSelect(comboConfig);
    }

    ///////////Selecciona un item del modal de búsqueda/////////////
    static GetSurcharge(data) {

        $("#listViewSearchAdvanced").UifListView({ source: null, displayTemplate: "#SurchargeTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: SurchargeParametrization.DeleteItemSurcharge, height: 300 });
        if (data != AppResources.ErrorGetSurcharge) {
            data.forEach(item => {               
                if (item.Type == SurchargeType.Percentage) {
                    item.RateDescription = SurchargeDescription.Percentage
                }
                if (item.Type == SurchargeType.Permilage) {
                    item.RateDescription = SurchargeDescription.Permilage
                }
                if (item.Type == SurchargeType.FixedValue) {
                    item.RateDescription = SurchargeDescription.FixedValue
                }
                $("#listViewSearchAdvanced").UifListView("addItem", item);
            });
        }
    }

    
    ///////////Agrega un item a la lista /////////////
    AddItemSurcharge() {
        var formPaymentPlan = $("#formSurcharge").serializeObject();
        $("#formSurcharge").validate();
        if ($("#formSurcharge").valid()) {
            var surchargeNew = {};
            if ($("#Id").val() == "") {
                surchargeNew.Id = 0;
            }
            else {
                surchargeNew.Id = parseInt($("#Id").val());
            }
            surchargeNew.Description = $("#inputDescription").val();
            surchargeNew.Type = $("#selectType").UifSelect("getSelected");
            if (surchargeNew.Type == SurchargeType.Percentage) {
                surchargeNew.Rate = parseFloat($("#inputRate").val());
                surchargeNew.RateDescription = SurchargeDescription.Percentage
            }
            if (surchargeNew.Type == SurchargeType.Permilage) {
                surchargeNew.Rate = parseFloat($("#inputRateM").val());
                surchargeNew.RateDescription = SurchargeDescription.Permilage
            }
            if (surchargeNew.Type == SurchargeType.FixedValue) {
                surchargeNew.Rate = parseFloat($("#inputRateI").val());
                surchargeNew.RateDescription = SurchargeDescription.FixedValue
            }
            surchargeNew.TinyDescription = $("#inputDescriptionShort").val();
            if (formSurcharge.bandUpdate) {
                surchargeNew.StatusTypeService = ParametrizationStatus.Update;
            } else {
                surchargeNew.StatusTypeService = ParametrizationStatus.Create;
            }

            if (surchargeIndex == null) {

                var ifExist = $("#listViewSearchAdvanced").UifListView('getData').filter(function (item) {

                    return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistSurcharge, 'autoclose': true });
                }
                else {
                    $("#listViewSearchAdvanced").UifListView("addItem", surchargeNew);
                }
            }
            else {

                var ifExist = $("#listViewSearchAdvanced").UifListView('getData').filter(function (item) {
                    return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase() && item.Id != $("#Id").val()
                       
                });

               
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistSurcharge, 'autoclose': true });
                } else {
                    $('#listViewSearchAdvanced').UifListView('editItem', surchargeIndex, surchargeNew);
                }
               
            }
            SurchargeParametrization.Clear();
            formSurcharge.bandUpdate = false;
        };
       
    }
    ///////////Editar /////////////
    static ShowData(event, result, index) {
        SurchargeParametrization.Clear();
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }       

        surchargeIndex = index;
        $("#Id").val(result.Id);
        $("#inputDescription").val(result.Description);
        $("#selectType").UifSelect("setSelected", result.Type);
        if (result.Type == SurchargeType.Percentage) {
            $("#porcentaje").show();
            $("#milaje").hide();
            $("#importe").hide();
            $("#inputRate").val(result.Rate);
        }
        if (result.Type == SurchargeType.Permilage) {
            $("#porcentaje").hide();
            $("#milaje").show();
            $("#importe").hide();
            $("#inputRateM").val(result.Rate);
        } if (result.Type == SurchargeType.FixedValue) {
            $("#porcentaje").hide();
            $("#milaje").hide();
            $("#importe").show();
            $("#inputRateI").val(result.Rate);
        }
        $("#inputDescriptionShort").val(result.TinyDescription);
        if (result.Id > 0) {
            formSurcharge.bandUpdate = true;
        }

    }
    ///////////Limpia los campos/////////////
    static Clear() {
        $("#inputDescription").val('');
        $("#inputDescription").focus();
        $("#inputDescriptionShort").val('');
        $("#selectType").UifSelect("setSelected", null);
        $("#selectType").UifSelect("disabled", false);
        $("#inputRate").val('');
        $("#inputRateM").val('');
        $("#inputRateI").val('');
        ClearValidation("#formSurcharge");
        surchargeIndex = null;
    }
    ///////////Limpia los campos/////////////
    NewLoad() {
        SurchargeParametrization.Clear();
    }
    ///////////Renderiza al index principal/////////////////
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    ///////////Carga la lista /////////////
    static loadSurcharge() {
        $("#listViewSearchAdvanced").UifListView({ source: null, displayTemplate: "#SurchargeTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(glbSurcharge, function (key, value) {
            Surcharge =
                {
                    Description: this.Description,
                    Id: this.Id,
                    Rate: this.Rate,

                }
            $("#listViewSearchAdvanced").UifListView("addItem", Surcharge);
        })
    }

    /////Elimina un item en la lista //////
	static DeleteItemSurcharge(deferred, result) {
		deferred.resolve();
        if (result.Id !== "" && result.Id !== 0) //Se elimina unicamente si existe en DB
		{
			result.StatusTypeService = ParametrizationStatus.Delete;
			result.allowEdit = false;
			result.allowDelete = false;
			$("#listViewSearchAdvanced").UifListView("addItem", result);
        }
        SurchargeParametrization.Clear();
    }

    /////////Actualiza la lista //////////
    static LoadSurcharges() {
        request('Parametrization/Surcharge/GetSurcharge', null, 'POST', AppResources.ErrorConsultingSurcharge, SurchargeParametrization.GetSurcharge);
    }

    ///////Almacena descuentos en base de datos////////
    SaveSurcharge() {

        var itemModified = [];
        var dataTable = $("#listViewSearchAdvanced").UifListView('getData');
        $.each(dataTable, function (index, value) {
            if (value.StatusTypeService == 0) {
                value.StatusTypeService = ParametrizationStatus.Original
            }
            itemModified.push(value);
        });

        if (itemModified.length > 0) {
            request('Parametrization/Surcharge/SaveSurcharges', JSON.stringify({ surcharge: itemModified }), 'POST', Resources.Language.SaveSurcharge, SurchargeParametrization.ResultSave);
        }
    }

    static ResultSave(data) {
        request('Parametrization/Surcharge/GetSurcharge', null, 'GET', AppResources.ErrorConsultingSurcharge, SurchargeParametrization.GetSurcharge);

        if (data.Message === null) {
            data.Message = 0;
        }


        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
            AppResources.Aggregates + ':' + data.TotalAdded + '<br> ' +
            AppResources.Updated + ':' + data.TotalModified + '<br> ' +
            AppResources.Removed + ':' + data.TotalDeleted + '<br> ' +
            AppResources.Errors + ':' + data.Message,
            'autoclose': true
        });
    }
}