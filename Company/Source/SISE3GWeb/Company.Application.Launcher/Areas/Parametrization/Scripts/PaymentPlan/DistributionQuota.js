$.ajaxSetup({ async: true });

class DistributionQuotaParametrization extends Uif2.Page {
    getInitialState() {
    }
    bindEvents() {
        $("#btnDistributionQuotaAccept").click(this.AddDistributionQuota);
    }
    AddDistributionQuota() {
        if (DistributionQuotaParametrization.validateRequiredGapQuantity() && DistributionQuotaParametrization.calculateSumDistribution()) {
            glbBandDistribution = true;
            DistributionQuotaParametrization.getformQuotas();
            $("#modalDistributionQuota").UifModal("hide");
        }
    }
    static validateRequiredGapQuantity() {
        var band = true;
        var quotasObject = $("#formDistributionQuota").serializeObject();
        var sumaImpuestos = 0;

        let componetsTmp = [];
        if (quotasList.length > 0) {
            for (var i = 0; i < quotasList.length; i++) {
                quotasList[i].GapQuantity = quotasObject.GapQuantity[i];
            }
            quotasList.forEach((itemQuota) => {
                itemQuota.QuotaComponentTypeServiceModel.forEach((itemOptions) => {
                    let c = componetsTmp.filter(x => { return x.valComponentType == itemOptions.Id });
                    if (c.length == 0) {
                        componetsTmp.push({ valComponentType: itemOptions.Id, valPercentage: parseInt(itemOptions.Value) })
                    }
                    else {
                        c[0].valPercentage += parseInt(itemOptions.Value);
                    }
                });
            });
        }

        let prima = componetsTmp.filter(item => item.valComponentType === "1" && item.valPercentage === 100);
        let gastos = componetsTmp.filter(item => item.valComponentType === "2" && item.valPercentage === 100);
        let surcharges = componetsTmp.filter(item => item.valComponentType === "3" && item.valPercentage === 100);
        let impuestos = componetsTmp.filter(item => item.valComponentType === "4" && item.valPercentage === 100);
        let IVA = componetsTmp.filter(item => item.valComponentType === "5" && item.valPercentage === 100);

        if (impuestos.length > 0) {
            $.UifNotify('show', { 'type': 'info', 'message': "Valor componentes de tipo Impuestos no corresponde al 100%", 'autoclose': true });
            band = false;
        }
        if (gastos.length > 0) {
            $.UifNotify('show', { 'type': 'info', 'message': "Valor componentes de tipo Gastos no corresponde al 100%", 'autoclose': true });
            band = false;
        }

        if (IVA.length > 0) {
            $.UifNotify('show', { 'type': 'info', 'message': "Valor componentes de tipo IVA no corresponde al 100%", 'autoclose': true });
            band = false;
        }

        if (prima.length > 0) {
            $.UifNotify('show', { 'type': 'info', 'message': "Valor componentes de tipo PRIMA no corresponde al 100%", 'autoclose': true });
            band = false;
        }

        if (surcharges.length > 0) {
            $.UifNotify('show', { 'type': 'info', 'message': "Valor componentes de tipo Surcharges no corresponde al 100%", 'autoclose': true });
            band = false;
        }


        if ((Array.isArray(quotasObject.GapQuantity))
            && (quotasObject.GapQuantity.length < (parseInt($("#Quantity").val()) - 1) //Se resta 1 debido a q' el primer "Tiempo entre cuotas" no se muestra en la distribucción
                || quotasObject.GapQuantity.filter(function (item) { return item === "" }).length > 0)) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.TimeRequiredAllField, 'autoclose': true });
            band = false;
        }

        //$.each(values, function (index, value) {
        //    $('#' + this.ControlName).val(this.ControlValue)
        //});




        //if (Array.isArray(quotasObject.Percentage)
        //    && (quotasObject.Percentage.length < parseInt($("#Quantity").val())
        //        || quotasObject.Percentage.filter(function (item) { return item === "" }).length > 0)) {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.DistributionRequiredAllField, 'autoclose': true });
        //    band = false;
        //}
        //if ((Array.isArray(quotasObject.GapQuantity))
        //    && (quotasObject.GapQuantity.length < (parseInt($("#Quantity").val()) - 1) //Se resta 1 debido a q' el primer "Tiempo entre cuotas" no se muestra en la distribucción
        //        || quotasObject.GapQuantity.filter(function (item) { return item === "" }).length > 0)) {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.TimeRequiredAllField, 'autoclose': true });
        //    band = false;
        //}
        return band;
    }
    static calculateSumDistribution() {
        var band = false;
        var sum = "0";
        if (parseInt($("#Quantity").val()) === 1 && parseInt($("#formDistributionQuota").serializeObject().Percentage) === 100) {
            band = true;
        }
        else {
            $($("#formDistributionQuota").serializeObject().Percentage).each(function (index, item) {
                sum += "+" + item;
            })
            if (eval("+" + "(" + sum + ")" + ".toFixed(12)") === 100) {
                band = true;
            }
        }
        if (band === false) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPercentageTotal, 'autoclose': true });
        }
        return band;
    }
    static getformQuotas() {
        var quotas = [];
        var contQuotas = parseInt($("#Quantity").val());
        var formQuotas = $("#formDistributionQuota").serializeObject();
        ///formQuotas.GapQuantity.splice(0, 0, "0");
        if (contQuotas === 1) {
            quotas[0] = { GapQuantity: parseFloat(formQuotas.GapQuantity), Percentage: parseFloat(formQuotas.Percentage) };
        }
        else {
            for (var i = 0; i < contQuotas; i++) {
                quotas[i] = { GapQuantity: parseFloat(formQuotas.GapQuantity[i]), Percentage: parseFloat(formQuotas.Percentage[i]) };
            }
        }
        listQuotas = formQuotas.GapQuantity;
        $("#Quotas").data('arrayQuotas', quotas)
    }

}