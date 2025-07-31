$.ajaxSetup({ async: false });
var PayMentPlan = null;
var PayMentPlanAssing = null;
var PayMentFinancialPlan = null;
$(document).ready(function () {
    EventsPaymentPlan();
});
function EventsPaymentPlan() {
    //Plan de Pagos
    $("#btnPaymentPlan").on("click", function () {
        //paymentPlan_btnPaymentPlan();
        if ($("#TableCurrency").UifDataTable("getSelected").length < 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectCurrency, 'autoclose': true });
            return false;
        }
        lockScreen();
        setTimeout(function () {
            LoadPaymenPlan();
            $("#modalPaymentPlan").UifModal('showLocal', Resources.Language.LabelPaymentPlans);
        }, 200);
    });
    //Asignar Todos
    $("#btnModalPaymentPlanAssignAll").on("click", function () {
        CopyPaymentPlan($("#listviewPaymentPlan").UifListView('getData'))
    });
    //Asignar Uno
    $("#btnModalPaymentPlanAssign").on("click", function () {
        if ($("#listviewPaymentPlan").UifListView('getData').length > 0) {
            $("#btnModalPaymentPlanAssign").prop("disabled", true);
            try {
                CopyPaymentPlanSelected($("#listviewPaymentPlan").UifListView('getSelected'))
            } catch (e) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMovingPaymentPlan, 'autoclose': true });
            } finally {
                $("#btnModalPaymentPlanAssign").prop("disabled", false);
            }
        }
    });
    //Desasignar Todos
    $("#btnModalPaymentPlanDeallocateAll").on("click", function () {
        deallocatePaymentPlanAll($("#listviewPaymentPlanAssing").UifListView('getData'))
    });
    //Desasignar Uno
    $("#btnModalPaymentPlanDeallocate").on("click", function () {
        if ($("#listviewPaymentPlanAssing").UifListView('getData').length > 0) {
            deallocatePaymentPlanSelect($("#listviewPaymentPlanAssing").UifListView('getSelected'))
        }
    });
    //grabar plan de pago
    $("#btnModalPaymentPlanSave").on("click", function () {
        if (SavePaymentPlan()) {
            HidePanelsProduct(MenuProductType.PaymentPlan);
            Product.pendingChanges = true;
        }
    });
    //Cerrar plan de pago
    $("#btnModalPaymentPlanClose").on("click", function () {
        PayMentPlan = null;
        PayMentPlanAssing = null;
        HidePanelsProduct(MenuProductType.PaymentPlan);
    });
    $("#btnModalPaymentPlanDefault").on("click", function () {
        AssignPaymentPlanDefault();
    });
}
//asigna el plan de pago prederteminado
function AssignPaymentPlanDefault() {
    var planSelected = $("#listviewPaymentPlanAssing").UifListView('getSelected');
    if (planSelected.length == 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectPaymentPlan, 'autoclose': true });
    }
    else if (planSelected.length > 1) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectMaxiumPaymentPlan, 'autoclose': true });
    }
    else {
        planSelected = planSelected[0];
        var planData = $("#listviewPaymentPlanAssing").UifListView('getData');
        $.each(planData, function (key, value) {
            if (value.Id == planSelected.Id) {
                planSelected.IsDefaultDescription = "Predeterminado"
                planSelected.IsDefault = true;
                if (planSelected.StatusTypeService != 2) {
                    planSelected.StatusTypeService = 3;
                }
                $('#listviewPaymentPlanAssing').UifListView("editItem", key, planSelected);
            } else {
                if (value.Currency.Id == planSelected.Currency.Id) {
                    delete value.IsDefaultDescription;
                    value.IsDefault = false;
                    if (value.StatusTypeService != 2) {
                        value.StatusTypeService = 3;
                    }
                    
                    $('#listviewPaymentPlanAssing').UifListView("editItem", key, value);
                }
            }
        });
    }

    $("#listviewPaymentPlanAssing .item").removeClass("selected");
}

// Obtener los metodos de pago
function GetPaymentMethodSelect() {
    var controller = rootPath + "Product/Product/GetPaymentMethodSelect";
    $("#selectModalPaymentMethods").UifSelect({ source: controller });
}
//Obtener planes de pago
var deletePaymentPlanCallback = function (deferred, data) {
    deferred.resolve();
};
function LoadPaymenPlan() {
    PayMentFinancialPlan = null;
    var currencies = [];
    $.each($("#TableCurrency").UifDataTable("getSelected"), function (key, value) {
        currencies.push(value.Id)
    });

    if (PayMentFinancialPlan == null) {
        var controller = null;

        PaymentPlanRequestT.LoadPaymenPlan(currencies).done(function (data) {
            controller = data;
            unlockScreen();
        });

        //var controller = rootPath + 'Product/Product/GetPaymentSchudeleByCurrencies?currencies=' + currencies;
        $("#listviewPaymentPlan").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: controller, displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
        $("#listviewPaymentPlanAssing").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
        PayMentPlan = $("#listviewPaymentPlan").UifListView('getData');
        PayMentFinancialPlan = PayMentPlan;
    }
    else {
        unlockScreen();
    }
    LoadPaymentFinancialPlan();
}
function LoadPaymentFinancialPlan() {
    var PaymentPlanTemp = [];
    if (Product.FinancialPlan != null && Product.FinancialPlan.length > 0) {
        $.each(PayMentFinancialPlan, function (keyPayMent, PayMent) {
            var exist = false;
            $.each(Product.FinancialPlan, function (keyPayMentAssign, PayMentAssign) {
                if (PayMentAssign.IsDefault) {
                    Product.FinancialPlan[keyPayMentAssign].IsDefaultDescription = "Predeterminado";
                }
                if (PayMentAssign.Id == PayMent.Id) {
                    exist = true;
                    return;
                }
            });
            if (!exist) {
                PaymentPlanTemp.push(PayMentFinancialPlan[keyPayMent]);
            }
        });
        $.each(Product.FinancialPlan, function (keyPayMentAssign, PayMentAssign) {
            if (PayMentAssign.StatusTypeService === 4) {
                PaymentPlanTemp.push(PayMentAssign);
            }
        });
        PayMentPlan = PaymentPlanTemp;
        var financialPlans = Product.FinancialPlan.filter(function (item) {
            return item.StatusTypeService !== 4;
        });
        if (financialPlans != null && financialPlans.length > 0) {
            PayMentPlanAssing = jQuery.extend(true, [], financialPlans);
        }
        $("#listviewPaymentPlan").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: PaymentPlanTemp, displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
        $("#listviewPaymentPlanAssing").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: PayMentPlanAssing, displayTemplate: "#tmpForAssign", selectionType: 'multiple', height: 310 });

    }
}

function CopyPaymentPlan(data) {
    if ($("#listviewPaymentPlan").UifListView('getData').length > 0) {

        $.each(data, function (key, value) {
            value.IsSelected = true;
            if (value.StatusTypeService === 1) {
                value.StatusTypeService = 2;
            }
            if (value.StatusTypeService === 4) {
                value.StatusTypeService = 3;
            }
        });
        data = $("#listviewPaymentPlanAssing").UifListView('getData').concat(data);
        $("#listviewPaymentPlanAssing").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: data, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForAssign", selectionType: 'multiple', height: 310 });
        $("#listviewPaymentPlan").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
        PayMentPlan = [];
        PayMentPlanAssing = data;
    }
}
function CopyPaymentPlanSelected(data) {
    var PaymentNoAsign = $("#listviewPaymentPlan").UifListView('getData');
    var PaymentAsign = $("#listviewPaymentPlanAssing").UifListView('getData');
    $.each(data, function (index, data) {
        data.IsSelected = true;
        if (data.StatusTypeService === 1) {
            data.StatusTypeService = 2;
        }
        if (data.StatusTypeService === 4) {
            data.StatusTypeService = 3;
        }
        var findPayment = function (element, index, array) {
            return element.Id === data.Id
        }
        var index = $("#listviewPaymentPlan").UifListView("findIndex", findPayment);
        $("#listviewPaymentPlan").UifListView("deleteItem", index);
    });
    PaymentAsign = PaymentAsign.concat(data);
    $("#listviewPaymentPlanAssing").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: PaymentAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForAssign", selectionType: 'multiple', height: 310 });
}
(function ($) {

    $.fn.filterByData = function (prop, val) {
        var $self = this;
        if (typeof val === 'undefined') {
            return $self.filter(
                function () { return typeof $(this).data(prop) !== 'undefined'; }
            );
        }
        return $self.filter(
            function () { return $(this).data(prop) == val; }
        );
    };

})(window.jQuery);
function deallocatePaymentPlanAll(data) {
    $.each(data, function (key, value) {
        delete data[key].IsDefaultDescription;
        data[key].IsDefault = false;
        value.IsSelected = false;
        if (value.StatusTypeService === 1 || value.StatusTypeService === 3) {
            value.StatusTypeService = 4;
        }
        if (value.StatusTypeService === 2) {
            value.StatusTypeService = 1;
        }

    });

    if ($("#listviewPaymentPlanAssing").UifListView('getData').length > 0) {
        data = $("#listviewPaymentPlan").UifListView('getData').concat(data);
        $("#listviewPaymentPlan").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: data, displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
        $("#listviewPaymentPlanAssing").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], displayTemplate: "#tmpForAssign", selectionType: 'multiple', height: 310 });
        PayMentPlan = data;
        PayMentPlanAssing = [];
    }
}
function deallocatePaymentPlanSelect(data) {
    var PaymentNoAsign = $("#listviewPaymentPlan").UifListView('getData');
    var PaymentAsign = $("#listviewPaymentPlanAssing").UifListView('getData');
    $.each(data, function (index, data) {
        delete data.IsDefaultDescription;
        data.IsDefault = false;
        data.IsSelected = false;
        if (data.StatusTypeService === 1 || data.StatusTypeService === 3) {
            data.StatusTypeService = 4;
        }
        if (data.StatusTypeService === 2) {
            data.StatusTypeService = 1;
        }
        var findPayment = function (element, index, array) {
            return element.Id === data.Id
        }
        var index = $("#listviewPaymentPlanAssing").UifListView("findIndex", findPayment);
        $("#listviewPaymentPlanAssing").UifListView("deleteItem", index);
    });
    PaymentNoAsign = PaymentNoAsign.concat(data);
    $("#listviewPaymentPlan").UifListView({ filterColumns: ['Currency.Description', 'PaymentSchedule.Description', 'PaymentMethod.Description'], sourceData: PaymentNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForAssign", selectionType: 'multiple', height: 310 });
}

function SavePaymentPlan() {
    var currency = $("#TableCurrency").UifDataTable("getSelected");
    var planData = $("#listviewPaymentPlanAssing").UifListView('getData');
    var allCurrencies = [];

    $.each(planData, function (key, value) {
        if (value.IsDefault) {
            $.each(currency, function (keyCurrency, currencyValue) {
                if (currencyValue.Id == value.Currency.Id) {
                    allCurrencies.push(currencyValue);
                }
            });
        }
    });

    if (validateAllCurrenciesHasPlanPayment($("#listviewPaymentPlanAssing").UifListView('getData'))) {
        Product.FinancialPlan = [];
        if ($("#listviewPaymentPlanAssing").UifListView('getData') != null && $("#listviewPaymentPlanAssing").UifListView('getData').length > 0) {
            Product.FinancialPlan = $("#listviewPaymentPlanAssing").UifListView('getData').slice();
        }
        var deletedFinancialPlan = $("#listviewPaymentPlan").UifListView('getData');
        if (deletedFinancialPlan != null && deletedFinancialPlan.length > 0) {
            $.each(deletedFinancialPlan, function (key, Value) {
                if (Value.StatusTypeService === 4) {
                    Product.FinancialPlan.push(Value);
                }
            });
        }
        return true;
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCurrencyTypePaymentPlan, 'autoclose': true });
        return false;
    }
}

function validateAllCurrenciesHasPlanPayment(data) {
    var currency = Product.Currencies.filter(function (item) {
        return item.StatusTypeService !== 4;
    });
    var planData = data;
    var allCurrencies = [];

    if (planData != null) {
        $.each(planData, function (key, value) {
            if (value.IsDefault) {
                $.each(currency, function (keyCurrency, currencyValue) {
                    if (currencyValue.Id == value.Currency.Id) {
                        allCurrencies.push(currencyValue);
                    }
                });
            }
        });


        if (currency.length == allCurrencies.length) {
            return true;
        }
        else {
            return false;
        }
    } else {
        return false;
    }
}

function ajaxGetFinancialPlanByProductId(productId) {
    return PaymentPlanRequestT.ajaxGetFinancialPlanByProductId(productId);
}

//function  paymentPlan_btnPaymentPlan() {
//    if (Product.Id > 0 && Product.FinancialPlan.length == 0) {
//        var getFinancialPlanByProductId = ajaxGetFinancialPlanByProductId(Product.Id);
//        getFinancialPlanByProductId.done(function (data) {
//            if (ObjectProduct.successAjax(data)) {
//                if (data.result != null) {
//                    Product.FinancialPlans = data.result;
//                    Product.ProductOld.FinancialPlans = data.result;
//                    $.each(data.result, function (key, item) {
//                        if (item.PaymentSchedule != null) {
//                            item.PaymentSchedule.DisabledDate = FormatDate(item.PaymentSchedule.DisabledDate);
//                            Product.ProductOld.FinancialPlans[key].PaymentSchedule.DisabledDate = FormatDate(item.PaymentSchedule.DisabledDate);
//                        }
//                    });
//                    if (Product.ProductOld.FinancialPlans != null) {
//                        $.each(Product.ProductOld.FinancialPlans, function (key, item) {
//                            if (item.PaymentSchedule != null) {
//                                item.PaymentSchedule.DisabledDate = FormatDate(item.PaymentSchedule.DisabledDate);
//                                Product.ProductOld.FinancialPlans[key].PaymentSchedule.DisabledDate = FormatDate(item.PaymentSchedule.DisabledDate);
//                            }
//                        });
//                    }
//                }
//            }            
//        }).fail(function (jqXHR, textStatus, errorThrown) {
//            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryFinancialPlanByProductId, 'autoclose': true })
//        });
//    }
//}