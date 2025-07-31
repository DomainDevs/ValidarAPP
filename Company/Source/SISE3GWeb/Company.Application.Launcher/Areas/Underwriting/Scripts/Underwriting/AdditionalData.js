//Codigo de la pagina AdditionalData.cshtml
class UnderwritingAdditionalData extends Uif2.Page {
    getInitialState() {
        UnderwritingAdditionalData.LoadAdditionalData();
    }

    bindEvents() {
        $("#btnAdditionalData").on('click', this.AdditionalData);
        $("#btnAdditionalDataSave").on("click", UnderwritingAdditionalData.SaveAdditionalData);
    }

    AdditionalData() {
        if (glbPolicy.Id == 0) {            
            if ($("#formUnderwriting").valid()) {
                //Underwriting.SaveTemporalPartial(MenuType.CoInsurance);      
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
            }
        }
        else {
            UnderwritingAdditionalData.LoadPartialAdditionalData();
        }
    }

    static LoadAdditionalData() {
        if (glbPolicy.CalculateMinPremium == true) {
            $("#chkCalculateMinimumPremium").prop('checked', true);
        }
        else {
            $("#chkCalculateMinimumPremium").prop('checked', false);
        }
        Underwriting.LoadSubTitles(8);
    }

    static LoadPartialAdditionalData() {

        $("#formUnderwriting").validate();

        if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
            Underwriting.ShowPanelsIssuance(MenuType.AdditionalData)


        }
    }

    static SaveAdditionalData() {

        $("#formAdditionalData").validate();

        if ($("#formAdditionalData").valid()) {
            var additionalDataModel = $("#formAdditionalData").serializeObject();
            additionalDataModel.CalculateMinimumPremium = $("#chkCalculateMinimumPremium").prop('checked');
            AdditionalDataRequest.SaveAdditionalDAta(glbPolicy.Id, additionalDataModel).done(function (data) {
                if (data.success) {
                    Underwriting.HidePanelsIssuance(MenuType.AdditionalData);
                    glbPolicy.CalculateMinPremium = data.result;
                    UnderwritingAdditionalData.LoadAdditionalData();
                    if (glbPolicy.TemporalType == TemporalType.Quotation) {
                        Underwriting.SaveTemporal(false);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTexts, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTexts, 'autoclose': true });
            });
        }
    }

    static GetCalculateMinPremiumByProductId(productId) {
        AdditionalDataRequest.GetCalculateMinPremiumByProductId(productId).done(function (data) {
            if (data.success) {
                glbPolicy.CalculateMinPremium = data.result;
                UnderwritingAdditionalData.LoadAdditionalData();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTexts, 'autoclose': true });
        });

    }
}