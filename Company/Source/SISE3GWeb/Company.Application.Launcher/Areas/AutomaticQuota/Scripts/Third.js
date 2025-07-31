class Third extends Uif2.Page {
    getInitialState() {
        Third.LoadThird();
    }

    bindEvents() {

    }

    static LoadThird() {
        if ($("#formThird").valid()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
        } else {
            AutomaticQuota.ShowPanelsAutomaticQuota(MenuType.AdditionalData)
            // Third.LoadPartialThird();
        }
    }

    //static LoadPartialThird() {

    //    $("#formUnderwriting").validate();
    //    if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
    //        Underwriting.ShowPanelsIssuance(MenuType.AdditionalData)
    //    }
    //}
}
