$.ajaxSetup({ async: false });
var currentEditIndex = 0;
var currencyRow = [];
var DECIMAL_QTY = 0;
$(document).ready(function () {
    EventsCurrency();
});
function EventsCurrency() {
 
    GetParameterDecimalQty();
    $("#DecimalValue").attr("maxlength", 1);
    $("#DecimalValue").on('input', function () {
        if (this.value >= 0 && this.value <= DECIMAL_QTY)
            this.value = this.value;
        else {
            this.value = 0;
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ControlDecimalQty + DECIMAL_QTY+"]" })
        }
    });

    //Guardar Moneda
    $("#btnModalCurrencySave").on("click", function () {
        SaveProductCurrency();
        HidePanelsProduct(MenuProductType.Currency);
        Product.pendingChanges = true;
    });
    $("#btnModalCurrencyClose").on("click", function () {
        HidePanelsProduct(MenuProductType.Currency);
    });
    $("#TableCurrency").on("rowEdit", function (event, data, position) {

        currencyRow = data;
        currentEditIndex = position;

        $("#editForm").find("#DecimalValue").val(data.DecimalQuantity);
        $("#editAction").UifInline("show");
    });
    //ELEMENTOS DEL FORMULARIO
    $('#editAction').on('Save', function () {

        var rowModel = new RowModelCurrency();

        rowModel.Id = currencyRow.Id;
        rowModel.Description = currencyRow.Description;
        rowModel.DecimalQuantity = parseInt($("#DecimalValue").val());
        $('#TableCurrency').UifDataTable('editRow', rowModel, currentEditIndex);
        SetCurrency(currencyRow.Id);
        $("#editForm").formReset();
        $('#editAction').UifInline('hide');
    });


    $('#editAction').on('Next', function () {
        $('#TableCurrency').UifDataTable("next");
    });

    $('#editAction').on('Previous', function () {
        $('#TableCurrency').UifDataTable("previous");
    });
}

function RowModelCurrency() {
    this.Id;
    this.Description;
    this.DecimalQuantity;
}

function SaveProductCurrency() {
    var currencies = $("#TableCurrency").UifDataTable("getSelected");
    if (Product.Currencies !== null) {
        $.each(Product.Currencies, function (key, value) {
            var ifExist = [];
            if (currencies !== null) {
                ifExist = currencies.filter(function (item) {
                    return item.Id === value.Id;
                });
            }
            if (ifExist.length === 0) {
                if (value.StatusTypeService === 1 || value.StatusTypeService === 4) {
                    value.StatusTypeService = 4;
                }
                else {
                    Product.Currencies.splice(key, 1);
                }
            }
            else {
                if (value.StatusTypeService === 4) {
                    value.StatusTypeService = 1;
                }
                value.DecimalQuantity = ifExist[0].DecimalQuantity;
                value.StatusTypeService = 3;
            }
        });
        if (currencies !== null) {
            $.each(currencies, function (key, value) {
                var ifExist = Product.Currencies.filter(function (item) {
                    return item.Id === value.Id;
                });
                if (ifExist.length === 0) {
                    Product.Currencies.push({
                        Id: value.Id,
                        Description: null,
                        DecimalQuantity: value.DecimalQuantity,
                        StatusTypeService: 2
                    })
                }
            });
        }
    }
    else {
        $.each(currencies, function (key, value) {
            Product.Currencies.push({
                Id: value.Id,
                Description: null,
                DecimalQuantity: value.DecimalQuantity,
                StatusTypeService: 2
            });

        });
    }
    var idCurrencies = [];
    $.each($("#TableCurrency").UifDataTable("getSelected"), function (key, value) {
        //Product.Currencies.push({ Id: value.Id })
        idCurrencies.push(value.Id);
    });
    if (Product.FinancialPlan != null) {
        $.each(Product.FinancialPlan, function (keyPlan, Plan) {
            if (idCurrencies.indexOf(Product.FinancialPlan[keyPlan].Currency.Id) === -1) {
                if (Plan.StatusTypeService === 2) {
                    Product.FinancialPlan.splice(keyPlan, 1);
                }
                else {
                    Plan.IsDefault = false;
                    Plan.IsSelected = false;
                    Plan.StatusTypeService = 4;
                }
            }
        });
    }

    var currenciesEnable = Product.Currencies.filter(function (item) {
        return item.StatusTypeService !== 4;
    });
    $('#selectCurrency').UifSelect("setSelected", currenciesEnable[0].Id);
}

function GetParameterDecimalQty() {
    

    ProductRequestT.GetParameterDecimalQty().done(function (data) {
        if (data.success) {
            DECIMAL_QTY = data.result;
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        DECIMAL_QTY =0
    });
 
}