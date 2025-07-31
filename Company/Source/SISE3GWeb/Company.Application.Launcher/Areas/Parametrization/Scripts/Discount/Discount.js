$.ajaxSetup({ async: true });
var Discount = {};
var glbDiscount = {};
var discountIndex = null;
var status = 0;
var formDiscount = {};

class DiscountParametrization extends Uif2.Page {
    getInitialState() {
        $("#milaje").hide(); 
        $("#importe").hide();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listViewSearchAdvanced").UifListView({ displayTemplate: "#DiscountTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        request('Parametrization/Discount/GetRateTypes', null, 'GET', Resources.Language.ErrorSearch, DiscountParametrization.GetRateTypes);
        request('Parametrization/Discount/GetDiscount', null, 'GET', AppResources.ErrorConsultingDiscount, DiscountParametrization.GetDiscount);
       
    }
    /////////Eventos////////////////////////////////////
    bindEvents() {
        $("#btnShowAdvanced").on("click", this.ShowAdvanced);
        $('#btnExit').click(this.Exit);
        $('#btnExport').click(this.SendExcelDiscount);
        $('#btnNewDiscount').on('click', this.NewLoad);
        $('#btnSaveDiscount').on('click', this.SaveDiscount);
        $('#btnDiscountAccept').on('click', this.AddItemDiscount);       
        $('#listViewSearchAdvanced').on('rowEdit', DiscountParametrization.ShowData);       
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
    SendExcelDiscount() {
        request('Parametrization/Discount/GenerateFileToExport', null, 'GET', AppResources.ErrorConsultingDiscount, DiscountParametrization.GenerateFileToExport);
    }
    static GetRateTypes(data) {
        let comboConfig = { sourceData: data };
        $("#selectType").UifSelect(comboConfig);
    }

    ///////////Selecciona un item del modal de búsqueda/////////////
    static GetDiscount(data) {
        $("#listViewSearchAdvanced").UifListView({ source: null, displayTemplate: "#DiscountTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: DiscountParametrization.DeleteItemDiscount, height: 300 });
        if (data != AppResources.ErrorGetDiscount) {
            data.forEach(item => {
                item.Status = 'Original';
                if (item.Type == DiscountType.Percentage) {
                    item.RateDescription = DiscountDescription.Percentage
                }
                if (item.Type == DiscountType.Permilage) {
                    item.RateDescription = DiscountDescription.Permilage
                }
                if (item.Type == DiscountType.FixedValue) {
                    item.RateDescription = DiscountDescription.FixedValue
                }
                $("#listViewSearchAdvanced").UifListView("addItem", item);
            });
        }
    }

    
    ///////////Agrega un item a la lista /////////////
    AddItemDiscount() {

        $("#formDiscount").validate();
        if ($("#formDiscount").valid()) {
            var discountNew = {};
            if ($("#Id").val() == "") {
                discountNew.Id = 0;
            }
            else {
                discountNew.Id = parseInt($("#Id").val());
            }
            discountNew.Description = $("#inputDescription").val();
            discountNew.Type = $("#selectType").UifSelect("getSelected");
            if (discountNew.Type == DiscountType.Percentage) {
                discountNew.Rate = parseFloat($("#inputRate").val());
                discountNew.RateDescription = DiscountDescription.Percentage
            }
            if (discountNew.Type == DiscountType.Permilage) {
                discountNew.Rate = parseFloat($("#inputRateM").val());
                discountNew.RateDescription = DiscountDescription.Permilage
            }
            if (discountNew.Type == DiscountType.FixedValue) {
                discountNew.Rate = parseFloat($("#inputRateI").val());
                discountNew.RateDescription = DiscountDescription.FixedValue
            }
            discountNew.TinyDescription = $("#inputDescriptionShort").val();
            if (formDiscount.bandUpdate) {
                discountNew.StatusTypeService = ParametrizationStatus.Update;
            } else {
                discountNew.StatusTypeService = ParametrizationStatus.Create;
            }

            if (discountIndex == null) {

                var ifExist = $("#listViewSearchAdvanced").UifListView('getData').filter(function (item) {

                    return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistDiscount, 'autoclose': true });
                }
                else {
                    $("#listViewSearchAdvanced").UifListView("addItem", discountNew);
                }
            }
            else {
                var ifExist = $("#listViewSearchAdvanced").UifListView('getData').filter(function (item) {
                    return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase() && item.Id != $("#Id").val()
                                        
                });

                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistDiscount, 'autoclose': true });
                } else {
                    $('#listViewSearchAdvanced').UifListView('editItem', discountIndex, discountNew);
                }
            }
            DiscountParametrization.Clear();
            formDiscount.bandUpdate = false;
        };
      
    }
    ///////////Edita la Búsqueda /////////////
    static ShowData(event, result, index) {
        DiscountParametrization.Clear();
        var resultUnique = null;
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }       

        discountIndex = index;
        $("#Id").val(result.Id)
        $("#inputDescription").val(result.Description);
        $("#selectType").UifSelect("setSelected", result.Type);
        if (result.Type == DiscountType.Percentage) {
            $("#porcentaje").show();
            $("#milaje").hide();
            $("#importe").hide();
            $("#inputRate").val(result.Rate);
        }
        if (result.Type == DiscountType.Permilage) {
            $("#porcentaje").hide();
            $("#milaje").show();
            $("#importe").hide();
            $("#inputRateM").val(result.Rate);
        } if (result.Type == DiscountType.FixedValue) {
            $("#porcentaje").hide();
            $("#milaje").hide();
            $("#importe").show();
            $("#inputRateI").val(result.Rate);
        }
        $("#inputDescriptionShort").val(result.TinyDescription);
        if (result.Id > 0) {
            formDiscount.bandUpdate = true;
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
        ClearValidation("#formDiscount");
        discountIndex = null;
    }
    ///////////Limpia los campos/////////////
    NewLoad() {
        DiscountParametrization.Clear();
    }

    ///////////Renderiza al index principal/////////////////
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    ///////////Carga la lista /////////////
    static loadDiscount() {
        $("#listViewSearchAdvanced").UifListView({ source: null, displayTemplate: "#DiscountTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $.each(glbDiscount, function (key, value) {
            Discount =
                {
                    Description: this.Description,
                    Id: this.Id,
                    Rate: this.Rate,

                }
            $("#listViewSearchAdvanced").UifListView("addItem", Discount);
        })
    }

    /////Elimina un item en la lista //////
    static DeleteItemDiscount(deferred, result) {
		deferred.resolve();
		if (result.Id !== 0 && result.Id !== undefined) //Se elimina unicamente si existe en DB
		{
			result.StatusTypeService = ParametrizationStatus.Delete;
			result.allowEdit = false;
			result.allowDelete = false;
			$("#listViewSearchAdvanced").UifListView("addItem", result);
        }
        DiscountParametrization.Clear();
    }
    /////////Actualiza la lista //////////
    static LoadDiscounts() {
        request('Parametrization/Discount/GetDiscount', null, 'POST', AppResources.ErrorConsultingDiscount, DiscountParametrization.getDiscount);
    }

    ///////Almacena descuentos en base de datos////////
    SaveDiscount() {

        var itemModified = [];
        var dataTable = $("#listViewSearchAdvanced").UifListView('getData');
        $.each(dataTable, function (index, value) {
            if (value.StatusTypeService == 0) {
                value.StatusTypeService = ParametrizationStatus.Original
            }
            itemModified.push(value);
        });

        if (itemModified.length > 0) {
            request('Parametrization/Discount/SaveDiscounts', JSON.stringify({ discount: itemModified }), 'POST', Resources.Language.SaveDiscount, DiscountParametrization.ResultSave);
        }
    }

    static ResultSave(data) {
        request('Parametrization/Discount/GetDiscount', null, 'GET', AppResources.ErrorConsultingDiscount, DiscountParametrization.GetDiscount);

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