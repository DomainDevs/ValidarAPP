class UnderwritingTaxes extends Uif2.Page {
    getInitialState() {

        $("#listTaxes").UifListView({
            displayTemplate: "#TaxesTemplate",
            source: null,
            delete: true,
            customAdd: true,
            customEdit: true,
            height: 350,
            deleteCallback: this.DeleteItemTaxe
        });
    }

    //Seccion Eventos
    bindEvents() {
        $("#btnOpenTaxes").on("click", UnderwritingTaxes.TaxeLoad);
        $("#btnExitTaxes").on("click", UnderwritingTaxes.TaxeClose);
    }

    static TaxeLoad() {
        if (glbPolicy.Id == 0) {
            if ($("#formUnderwriting").valid()) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
            }
        }
        else {
            //UnderwritingTaxes.Clear;
            Underwriting.ShowPanelsIssuance(MenuType.Taxes);
            //Obtener recargos a
            //request('Underwriting/Underwriting/GetSurcharges', null, 'GET', AppResources.ErrorConsultingSurcharge, UnderwritingSurcharge.GetComboSurcharges);
            //request('Parametrization/Surcharge/GetListQuoSurcharges', null, 'GET', AppResources.ErrorConsultingSurcharge, UnderwritingSurcharge.GetQuoSurcharges);       
        }
    }

    static TaxeClose() {
        $('#modalTaxes').UifModal("hide");   
    }
}