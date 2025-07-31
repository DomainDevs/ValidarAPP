$.ajaxSetup({ async: true });
var dropDownAdv;
class PaymentPlanParametrizationAdv extends Uif2.Page {
    getInitialState() {
        dropDownAdv = uif2.dropDown({
            source: rootPath + 'Parametrization/PaymentPlan/AdvancedSearch',
            element: '#inputPaymenPlanSearch',
            align: 'right',
            width: 600,
            height: 400,
            container: "#main",
            loadedCallback: PaymentPlanParametrizationAdv.componentLoadedCallback
        });
    }
    bindEvents() {        
    }
    static componentLoadedCallback() {
        PaymentPlanParametrizationAdv.setListViewAdv(null);
        $("#btnCancelAdv").on("click", PaymentPlanParametrizationAdv.hideDropDownAdv);
        $("#btnAcceptdAdv").on("click", PaymentPlanParametrizationAdv.loadPaymentPlan);
    }
       
    static hideDropDownAdv() {
        dropDownAdv.hide();
        PaymentPlanParametrizationAdv.clearAdvanced();
    }

    static clearAdvanced() {
        $("#listViewAdv").UifListView({ sourceData: null });
        $("#inputPaymenPlanSearch").val("");
    }

    static searchPaymentPlan(event, value)
    {
        if (value.length < 3)
        {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.MustEnterThreeCharactersSearch, 'autoclose': true });
        }
        else
        {
            request('Parametrization/PaymentPlan/GetPaymentPlansByDescription', JSON.stringify({ description: value}), 'POST', AppResources.ErrorSearchPaymentPlan, PaymentPlanParametrizationAdv.setPaymentPlans)
        }
    }

    static setPaymentPlans(data) {
        if (data.length > 0)
        {
            dropDownAdv.show();
            PaymentPlanParametrizationAdv.setListViewAdv(data);
        }
        else
        {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.PaymentPlanNotExist, 'autoclose': true });
            PaymentPlanParametrizationAdv.clearAdvanced();
        }
    }

    static setListViewAdv(dataWithoutNegative)
    {
        if (dataWithoutNegative != null && dataWithoutNegative != undefined && dataWithoutNegative != "undefined") {
            dataWithoutNegative = dataWithoutNegative.filter(function (item) { return item.Id > 0 });
        }
        $("#listViewAdv").UifListView({
            displayTemplate: "#templatePaymentPlanAdv",
            selectionType: 'single',
            sourceData: dataWithoutNegative,
            height: 300
        });
    }

    static loadPaymentPlan() {
        var paymentPlanSelected = $("#listViewAdv").UifListView("getSelected");
        if (paymentPlanSelected.length > 0)
        {
            const findIndexPaymentPlan = function (element, index, array) {
                return element.Id === paymentPlanSelected[0].Id;
            }
            var index = $("#listPaymentPlan").UifListView("findIndex", findIndexPaymentPlan);
            PaymentPlanParametrization.editPaymentPlan(null, paymentPlanSelected[0], index);
            PaymentPlanParametrizationAdv.hideDropDownAdv();
        }
        else
        {
            $.UifNotify("show", { 'type': "info", 'message': "Debe seleccionar un item", 'autoclose': true });
        }
    }
}
    