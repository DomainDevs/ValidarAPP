class RiskTPLAdditionalData extends Uif2.Page {
    getInitialState() {
        $("#Tons").ValidatorKey(1, 0, 0);
        $("#TrailerQuantity").ValidatorKey(1, 0, 0);
    }

    bindEvents() {
    
        $("#btnAdditionalData").on('click', this.AdditionalDataLoad);
        $("#btnAdditionalDataClose").on("click", this.AdditionalDataClose);
        $("#btnAdditionalDataSave").on("click", this.AdditionalDataSave);
        $("#btnAdditionalData").on('click', this.AdditionalData);
    }

    AdditionalDataLoad() {
        RiskTPLAdditionalData.LoadAdditionalData();
    }

    AdditionalDataClose() {
        individualSearchType = 1;
        RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Risk);
    }

    AdditionalDataSave() {
        individualSearchType = 1;
        RiskTPLAdditionalData.SaveAdditionalData();
    }
    
    static LoadAdditionalData() {
        $("#formRiskTPL").validate();
        if (glbRisk.Id == 0) {
            glbRisk.redirectController = MenuType.AdditionalData;
            RiskThirdPartyLiability.SaveRisk(MenuType.AdditionalData, 0);
            RiskThirdPartyLiability.HidePanelsRisk();
            RiskThirdPartyLiability.ShowPanelsRisk(MenuType.AdditionalData);
            RiskTPLAdditionalData.LoadAdditionalDataRisk();
        }

        if (glbRisk.Id > 0) {
            RiskThirdPartyLiability.HidePanelsRisk();
            RiskThirdPartyLiability.ShowPanelsRisk(MenuType.AdditionalData);
            RiskTPLAdditionalData.LoadAdditionalDataRisk();
        }
    }

    static LoadAdditionalDataRisk() {
            if (glbRisk.Tons != null) {
                $("#Tons").val(glbRisk.Tons);
            }
            if (glbRisk.TrailerQuantity != null) {
                $("#TrailerQuantity").val(glbRisk.TrailerQuantity);
            }
            if (glbRisk.PhoneNumber != null) {
                $("#PhoneNumber").val(glbRisk.PhoneNumber);
            }   
    }

    static SaveAdditionalData() {
        $("#formAdditionalData").validate();
        if ($("#formAdditionalData").valid()) {
            var additionalData = $("#formAdditionalData").serializeObject();
            glbRisk.Tons = additionalData.Tons;
            glbRisk.TrailerQuantity = additionalData.TrailerQuantity;
            glbRisk.PhoneNumber = additionalData.PhoneNumber;
            RiskTPLAdditionalDataRequest.SaveAdditionalData(glbRisk.Id, additionalData).done(function (data) {
                if (data.success) {
                    RiskThirdPartyLiability.UpdateGlbRisk(data.result);
                    RiskThirdPartyLiability.ShowPanelsRisk(MenuType.Risk);
                    RiskThirdPartyLiability.LoadSubTitles(4);
                    $("#modalAdditionalData").UifModal('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveAdditionalData, 'autoclose': true });

            });
        }
    }    


 
 
}