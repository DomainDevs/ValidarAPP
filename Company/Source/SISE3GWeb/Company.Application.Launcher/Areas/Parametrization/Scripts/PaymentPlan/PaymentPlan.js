var glbPaymentPlanEdit = null;
var glbListPaymentPlanDelete = [];
var glbBandDistribution = true;
var gblComponentType = [];
var quotasList = [];
var quotasListGet = [];
var listQuotas = [];
$.ajaxSetup({ async: true });

class PaymentPlanParametrization extends Uif2.Page {
    getInitialState() {
        $("#FirstPayQuantity").val(0);
        $("#LastPayQuantity").val(0);
        $("#listPaymentPlan").UifListView({
            displayTemplate: "#templatePaymentPlan",
            source: null,
            selectionType: 'single',
            height: 350
        });
        request('Parametrization/PaymentPlan/GetParametrizationPaymentPlans', null, 'GET', AppResources.ErrorSearchPaymentPlan, PaymentPlanParametrization.getPaymentPlans);
        request('Parametrization/PaymentPlan/GetQuotaTypes', null, 'GET', AppResources.ErrorGetQuotaTypes, PaymentPlanParametrization.getQuotaTypes);
        request('Parametrization/PaymentPlan/GetPaymentCalculationTypes', null, 'GET', AppResources.ErrorSearchPaymentPlan, PaymentPlanParametrization.getPaymentCalculationTypes);
        request('Parametrization/PaymentPlan/GetTypeComponent', null, 'GET', AppResources.ErrorSearchPaymentPlan, PaymentPlanParametrization.getComponentType);
        $("#FirstPayQuantity").UifMask({
            pattern: '000'
        });
        $("#Quantity").UifMask({
            pattern: '000'
        });
        $("#LastPayQuantity").UifMask({
            pattern: '000'
        });
        $("#GapQuantity").UifMask({
            pattern: '00'
        });
    }

    bindEvents() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#FirstPayQuantity").OnlyDecimals(2);
        $("#Quantity").OnlyDecimals(2);
        $("#GapQuantity").OnlyDecimals(2);
        $("#FirstPayQuantity").OnlyDecimals(2);
        $("#LastPayQuantity").OnlyDecimals(2);
        $('#listPaymentPlan').on('rowEdit', PaymentPlanParametrization.editPaymentPlan);
        $("#btnNewPaymentPlan").click(PaymentPlanParametrization.clearFormPaymentPlan);
        $("#btnShowQuotas").click(this.showQuotas);
        $("#btnCalculateDistribution").click(PaymentPlanParametrization.distributionPaymentPlan);
        $("#btnAddPaymentPlan").click(this.addPaymentPlan);
        $("#btnCreatePaymentPlan").click(PaymentPlanParametrization.createPaymentPlan);
        $("#QuotaType").on('itemSelected', this.selectQuotaType);
        $("#Quantity").focusout(PaymentPlanParametrization.focusoutQuantity);
        $("#GapUnit").on('itemSelected', PaymentPlanParametrization.selectGapUnit);
        $("#btnExport").click(this.exportExcel);
        $("#btnExit").click(this.redirectIndex);
        $("#inputPaymenPlanSearch").on("search", PaymentPlanParametrizationAdv.searchPaymentPlan);
    }
    static getPaymentPlans(data) {
        var dataWithoutNegative = data.filter(function (item) { return item.Id > 0 });
        $("#listPaymentPlan").UifListView({
            displayTemplate: "#templatePaymentPlan",
            sourceData: dataWithoutNegative,
            selectionType: 'single',
            height: 350,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: PaymentPlanParametrization.deleteCallbackListPaymentPlan,
        });
    }
    static editPaymentPlan(event, result, index) {
        $("#Quotas").data('arrayQuotas', null);
        PaymentPlanParametrization.clearFormPaymentPlan();
        glbPaymentPlanEdit = result;
        glbPaymentPlanEdit.Index = index;
        $("#Id").val(result.Id);
        $("#Description").val(result.Description);
        $("#SmallDescription").val(result.SmallDescription);
        $("#FirstPayQuantity").val(result.FirstPayQuantity);
        $("#Quantity").val(result.Quantity);
        $("#Quantity").prop('disabled', true);
        $("#LastPayQuantity").val(result.LastPayQuantity);
        $("#GapUnit").prop('disabled', true);
        PaymentPlanParametrization.selectGapUnit(null, { Id: result.GapUnit });
        $("#GapQuantity").val(result.GapQuantity)
        if (!result.GapUnit > 0) {
            result.GapUnit = PaymentCalculationType.Day;
        }
        $("#GapUnit").UifSelect("setSelected", result.GapUnit);

        if (result.IsGreaterDate) {
            $("#QuotaType").UifSelect("setSelected", QuotaType.GreaterDate)
        }
        else if (result.IsIssueDate) {
            $("#QuotaType").UifSelect("setSelected", QuotaType.IssueDate)
        }
        else {
            $("#QuotaType").UifSelect("setSelected", QuotaType.CurrentDate)
        }
        $("#Quotas").data('arrayQuotas', result.QuotasServiceModel);
        quotasList = result.QuotasServiceModel;
        PaymentPlanParametrization.buildHmtlDistribution();
    }
    addPaymentPlan() {
        if ($("#formPaymentPlan").valid() && PaymentPlanParametrization.validateDistribution()) {
            var formPaymentPlan = $("#formPaymentPlan").serializeObject();

            if ($('#Financing').is(':checked')) {
                formPaymentPlan.Financing = true;
            }
            var paymenPlanTmp = PaymentPlanParametrization.CreatePaymentPlanModel(formPaymentPlan);

            //formPaymentPlan.QuotasServiceModel = quotasList;
            if (parseInt(formPaymentPlan.Id) > 0) {
                PaymentPlanParametrization.UpdatePaymentPlanParametrization(formPaymentPlan);
            }
            else {
                PaymentPlanParametrization.InsertPaymentPlanParametrization(formPaymentPlan);
            }
            PaymentPlanParametrization.clearFormPaymentPlan();
        }
    }

    static CreatePaymentPlanModel(formPaymentPlan) {
        if (quotasList.length > 0) {
            var arrayOptions = [];
            quotasList.forEach((itemQuota) => {
                let d = arrayOptions.filter(x => { return x.Number == itemQuota.Number });
                if (d.length == 0) {
                    var QuotaComponentTypeServiceModel = [];
                    itemQuota.QuotaComponentTypeServiceModel.forEach((itemOptions) => {

                        let c = QuotaComponentTypeServiceModel.filter(x => { return x.valComponentType == itemOptions.Id });
                        if (c.length == 0) {
                            QuotaComponentTypeServiceModel.push({ Id: itemOptions.Id, Value: parseInt(itemOptions.Value) })

                        }
                    });
                    if ((quotasList[(itemQuota.Number) - 1].GapQuantity == undefined)) {
                        arrayOptions.push({ Number: itemQuota.Number, GapQuantity: listQuotas[(itemQuota.Number) - 1].GapQuantity ? 0 : listQuotas[(itemQuota.Number) - 1], Percentage: itemQuota.QuotaComponentTypeServiceModel[0].Value, QuotaComponentTypeServiceModel })
                    } else {
                        arrayOptions.push({ Number: itemQuota.Number, GapQuantity: quotasList[(itemQuota.Number) - 1].GapQuantity, Percentage: itemQuota.QuotaComponentTypeServiceModel[0].Value, QuotaComponentTypeServiceModel })
                    }
                    
                }
            });
        }
        formPaymentPlan.QuotasServiceModel = arrayOptions;

        return formPaymentPlan;
    }


    static validateDistribution() {
        if (!glbBandDistribution) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.DistributionQuotaRequired, 'autoclose': true });
        }
        return glbBandDistribution;
    }

    static clearFormPaymentPlan() {
        $("#fieldsDistributionQuota").html("");
        glbBandDistribution = true;
        glbPaymentPlanEdit = null;
        $("#Id").val("");
        $("#Description").val("");
        $("#Description").focus();
        $("#SmallDescription").val("");
        $("#FirstPayQuantity").val(0);
        $("#LastPayQuantity").val(0);
        $("#Quantity").val("");
        $("#GapQuantity").val("");
        $("#Quantity").prop('disabled', false);
        $("#QuotaType").UifSelect("setSelected", null);
        $("#GapUnit").prop('disabled', false);
        $("#GapUnit").UifSelect("setSelected", null);
        $("#Quotas").data('arrayQuotas', null);
        $("#hideGapQuantity").show();
        ClearValidation("#formPaymentPlan");
    }
    static UpdatePaymentPlanParametrization(formPaymentPlan) {
        formPaymentPlan.StatusTypeService = ParametrizationStatus.Update;
        $("#listPaymentPlan").UifListView("editItem", glbPaymentPlanEdit.Index, formPaymentPlan);
    }
    static InsertPaymentPlanParametrization(formPaymentPlan) {
        formPaymentPlan.StatusTypeService = ParametrizationStatus.Create;
        if (glbPaymentPlanEdit !== null) {
            $("#listPaymentPlan").UifListView("editItem", glbPaymentPlanEdit.Index, formPaymentPlan);
        }
        else {
            $("#listPaymentPlan").UifListView("addItem", formPaymentPlan);
        }
    }
    static deleteCallbackListPaymentPlan(deferred, result) {
        deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined) //Se elimina unicamente si existe en DB
        {
            result.StatusTypeService = ParametrizationStatus.Delete;
            //glbListPaymentPlanDelete.push(result);
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listPaymentPlan").UifListView("addItem", result);
        }

        PaymentPlanParametrization.clearFormPaymentPlan();
    }
    static getQuotaTypes(data) {
        $("#QuotaType").UifSelect({ sourceData: data, id: 'Value', name: 'Text' });
    }
    static getPaymentCalculationTypes(data) {
        $("#GapUnit").UifSelect({ sourceData: data, id: 'Value', name: 'Text' });
    }

    static getComponentType(data) {
        gblComponentType = data;
        $("#ComponentType").UifSelect({ sourceData: data, id: 'ComponentTypeId', name: 'SmallDescription' });
    }

    static createPaymentPlan() {
        var paymentPlans = $("#listPaymentPlan").UifListView('getData');
        $.each(glbListPaymentPlanDelete, function (index, item) {
            paymentPlans.push(item);
        })
        var paymentPlansFilter = paymentPlans.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        if (paymentPlansFilter.length > 0) {
            request('Parametrization/PaymentPlan/CreateParametrizationPaymentPlan', JSON.stringify({ parametrizationPaymentPlanVM: paymentPlansFilter }), 'POST', AppResources.ErrorSavePaymentPlan, PaymentPlanParametrization.confirmCreateParametrizationPaymentPlan);
        }
    }
    static confirmCreateParametrizationPaymentPlan(data) {
        glbListPaymentPlanDelete = [];
        PaymentPlanParametrization.clearFormPaymentPlan();
        request('Parametrization/PaymentPlan/GetParametrizationPaymentPlans', null, 'GET', AppResources.ErrorSearchPaymentPlan, PaymentPlanParametrization.getPaymentPlans)
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
    selectQuotaType(event, selectedItem) {
        $("#IsGreaterDate").val(false);
        $("#IsIssueDate").val(false);
        switch (parseInt(selectedItem.Id)) {
            case QuotaType.GreaterDate:
                $("#IsGreaterDate").val(true);
                break;
            case QuotaType.IssueDate:
                $("#IsIssueDate").val(true);
                break;
            default:
                break;
        }
    }

    static selectGapUnit(event, selectedItem) {
        if (parseInt(selectedItem.Id) === PaymentCalculationType.Day && !($("#Id").val() > 0)) {
            $("#Quotas").data('arrayQuotas', null);
            glbBandDistribution = false;
        }
        else {
            glbBandDistribution = true;
        }
        PaymentPlanParametrization.focusoutQuantity();
    }

    static getQuotasDistribution() {
        var quotas = $("#Quotas").data('arrayQuotas');
        var numberQuotas = parseInt($("#Quantity").val());
        if (((quotas == null || quotas === undefined)) || (quotas.length === 0 || quotas.length !== numberQuotas)) {
            quotas = [];
            for (var i = 0; i < numberQuotas; i++) {
                var percentageAutomatic = 100 / numberQuotas;
                quotas.push({ Percentage: undefined, GapQuantity: undefined });
            }
            quotas = PaymentPlanParametrization.validateSumAutomatic(quotas);
            $("#Quotas").data('arrayQuotas', quotas)
        }
        listQuotas = $("#Quotas").data('arrayQuotas');
        return $("#Quotas").data('arrayQuotas');
    }

    static validateSumAutomatic(quotas) {
        var sumatory = 0;
        for (var i = 0; i < quotas.length; i++) {
            sumatory = sumatory + quotas[i].Percentage;
        }
        var diference100 = 100 - sumatory;
        quotas[quotas.length - 1].Percentage = quotas[quotas.length - 1].Percentage + diference100;
        return quotas;
    }

    static focusoutQuantity() {
        var valueQuantity = $("#Quantity").val();
        if (valueQuantity > 0) {
            if (PaymentPlanParametrization.validateRangeQuantity(valueQuantity)) {
                PaymentPlanParametrization.buildHmtlDistribution();
            }
        }
    }
    static validateRangeQuantity(value) {
        var bandRangeQuantity = true;
        var message;
        switch (parseInt($("#GapUnit").val())) {
            case PaymentCalculationType.Day:
                if (value < 1 || value > 365) {
                    bandRangeQuantity = false;
                    message = AppResources.RangeAllowBetween + " 1 y 365";
                }
                break;
            case PaymentCalculationType.Fortnight:
                if (value < 1 || value > 60) {
                    bandRangeQuantity = false;
                    message = AppResources.RangeAllowBetween + " 1 y 60";
                }
                break;
            case PaymentCalculationType.Month:
                if (value < 1 || value > 180) {
                    bandRangeQuantity = false;
                    message = AppResources.RangeAllowBetween + " 1 y 180";
                }
                break;
            default:
                bandRangeQuantity = true;
                break;
        }
        if (!bandRangeQuantity) {
            $("#Quantity").val("");
            $.UifNotify('show', { 'type': 'info', 'message': message, 'autoclose': true });
        }

        return bandRangeQuantity;
    }
    static buildHmtlDistribution() {
        var quotas = PaymentPlanParametrization.getQuotasDistribution();
        $("#fieldsDistributionQuota").html("");
        var htmlRowDistribution = "";

        $("#ComponentType").UifSelect({ sourceData: gblComponentType, id: 'ComponentTypeId', name: 'SmallDescription' });
        for (var i = 1; i <= quotas.length; i++) {
            if (quotas[i - 1].GapQuantity === undefined) {;
                ///Para el caso de Unidad de tiempo Dia, se debe mostrar en limpio los campos de tiempo entre cuotas
                if (parseInt($("#GapUnit").val()) === PaymentCalculationType.Fortnight || parseInt($("#GapUnit").val()) === PaymentCalculationType.Month) {
                    quotas[i - 1].GapQuantity = 1;
                } 
                
            }
            htmlRowDistribution = htmlRowDistribution +
                '<div class="row">' +
                '<div class="uif-col-4">' +
                '<label class="field-required">' + AppResources.Quota + ' </label><span style="text-align: center;padding: 25px;">' + i + '</span>' +
                '</div > ' +

                '<div class="uif-col-4" >' +
                '<label class="field-required">' + 'Tipo Componente' + '</label> <select class="ComponentType" id="ComponentType' + i + '" data-quota="' + i + '" name="ComponentType" data-filter="true" data-native="false" ></select>' +
                '</div >' +

                '<div class="uif-col-4" >' +
                '<label class="field-required">' + AppResources.Distribution + '</label> <input class="onlyNumber PercentageValue" type="text" data-quota="' + i + '" name="Percentage" id="IdPercentage' + i + '" value="" />' +
                '</div >' +

                '<div class="uif-col-3"> <label class="field-required labelQuantityQuota">' + AppResources.TimeBetweenQuota + '</label><input class="GapQuantityQuota" type="text" name="GapQuantity" value="' + quotas[i - 1].GapQuantity + '"/>' +
                '</div >' +
                '</div >';

            //'<div class="uif-col-2" >' +
            //'<label class="field-required">' + AppResources.Component + '</label> <select class="uif-select" id="ComponentType" name="GapUnit" data-val-required="true" data-val="true" data - filter="true" data - native="false" ></select ></div >' +
            //'</div >';


        }

        $("#fieldsDistributionQuota").append(htmlRowDistribution);
        $(".ComponentType").UifSelect({ sourceData: gblComponentType, id: 'ComponentTypeId', name: 'SmallDescription' });
        $(".ComponentType").on('change', function () {

            var Id = parseInt($(this).val());
            var Number = $(this).data('quota');
            var itemQuota = quotasList.filter(item => item.Number === Number);
            if (itemQuota.length > 0) {
                var itemOptions = itemQuota[0].QuotaComponentTypeServiceModel.filter(item => item.Id === Id);
                if (itemOptions.length > 0)
                    $("#IdPercentage" + Number).val(itemOptions[0].Value);
                else
                    $("#IdPercentage" + Number).val('');
            }
            else
                $("#IdPercentage" + Number).val('');



        });
        $(".PercentageValue").on('blur', function () {
            var Value = $(this).val();
            var Number = $(this).data('quota');
            var Id = parseInt($("#ComponentType" + Number).val());
            if (Id) {
                var object = { Number, QuotaComponentTypeServiceModel: [] };

                var itemQuota = quotasList.filter(item => item.Number === Number);
                if (itemQuota.length > 0) {
                    var itemOptions = itemQuota[0].QuotaComponentTypeServiceModel.filter(item => item.Id === Id);
                    if (itemOptions.length > 0) {
                        itemOptions[0].Value = Value;
                    }
                    else {
                        itemQuota[0].QuotaComponentTypeServiceModel.push({ Value, Id })
                    }
                }
                else {
                    object.QuotaComponentTypeServiceModel.push({ Value, Id });
                    quotasList.push(object);
                }
            }

        });
        $(".onlyNumber").OnlyDecimalsWithDot(2);
        $(".GapQuantityQuota").OnlyDecimalsWithDot(2);
        $(".GapQuantityQuota").UifMask({ //Solo se permiten dos digitos
            pattern: '00'
        });
        //$(".GapQuantityQuota").focusout(function () {
        //    if ($(this).val() > 99) {
        //        $.UifNotify('show', { 'type': 'info', 'message': AppResources.OnlyTwoDigitsQuota, 'autoclose': true });
        //        $(this).val(0);
        //    }
        //    if (parseInt($(this).val()) >= parseInt($(this).closest('div.row').next().find('input.GapQuantityQuota').val())) {
        //        $.UifNotify('show', { 'type': 'info', 'message': AppResources.TimeQuotaNotEqualFollowing, 'autoclose': true });
        //        $(this).val("");
        //    }
        //    if (parseInt($(this).val()) <= parseInt($(this).closest('div.row').prev().find('input.GapQuantityQuota').val())) {
        //        $.UifNotify('show', { 'type': 'info', 'message': AppResources.TimeQuotaNotEqualPrevious, 'autoclose': true });
        //        $(this).val("");
        //    }
        //});
        if (parseInt($("#Id").val()) > 0)//Si es edicion de DB no se puede modificar 
        {
            $(".GapQuantityQuota").prop("disabled", true);
        }
        $(".GapQuantityQuota:first").val(0);
        $(".labelQuantityQuota:first").css("display", "none");
        $(".GapQuantityQuota:first").css("display", "none");
    }
    showQuotas() {
        if ($("#formPaymentPlan").valid()) {
            PaymentPlanParametrization.showModalQuotas();
        }
    }
    static showModalQuotas() {
        PaymentPlanParametrization.buildHmtlDistribution();
        $("#modalDistributionQuota").UifModal("showLocal", AppResources.QuotaDistribution)
    }

    exportExcel() {
        request('Parametrization/PaymentPlan/GenerateFileToExport', null, 'GET', AppResources.ErrorGeneratingExcelFile, PaymentPlanParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }


}