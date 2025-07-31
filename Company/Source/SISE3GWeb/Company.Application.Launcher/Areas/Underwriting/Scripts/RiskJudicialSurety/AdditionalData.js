class ObjectAdditionalDataRiskJudicialSurety extends Uif2.Page {
    getInitialState() {
        DocumentTypeRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#selectDescriptionId").UifSelect({ sourceData: data.result });
            }
        });
        $("#NumberId").OnlyDecimals(UnderwritingDecimal);
    }
    bindEvents() {
        $("#inputInsured").on("buttonClick", ObjectAdditionalDataRiskJudicialSurety.SearchInsured);
        $("#btnAdditionalData").click(ObjectAdditionalDataRiskJudicialSurety.LoadAdditionalData);
        $("#btnAdditionalDataSave").click(ObjectAdditionalDataRiskJudicialSurety.SaveAdditionalData);
    }

    static GetAdditionalData() {
        var additionalData = $("#formAdditionalData").serializeObject();
        additionalData.RiskId = parseInt(glbRisk.Id);
        additionalData.DocumentType = parseInt($("#selectDescriptionId").UifSelect("getSelected"));
        additionalData.NumberId = parseInt($('#NumberId').val());
        additionalData.AttorneyId = glbRisk.IdentificationIdAgent;
        return additionalData;
    }
    static SaveAdditionalData() {

        $("#formAdditionalData").validate();

        if ($("#formAdditionalData").valid()) {

            var additionalData = ObjectAdditionalDataRiskJudicialSurety.GetAdditionalData();

            RiskJudicialSuretyAdditionalDataRequest.SaveAdditionalData(additionalData).done(function (data) {
                if (data.success) {
                    RiskJudicialSurety.UpdateGlbRisk(data.result);
                    RiskJudicialSurety.ShowPanelsRisk(MenuType.Risk);
                    RiskJudicialSurety.LoadSubTitles(4);
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
    static HideAdditionalData() {
        if (RiskJudicialCustodyModel.CardProfessionalNumber != null) {
            $('#selectedAdditionalData').val('1');
        }
        $('#modalAdditionalData').UifModal("hide");
    }
    static LoadAdditionalData() {

        $("#formJudicialSurety").validate();
        if (glbRisk.Id == 0) {

            glbRisk.Class.SaveRisk(MenuType.AdditionalData, 0);

        }
        if (glbRisk.Class == undefined) {
            window[glbRisk.Object].ShowPanelsRisk(MenuType.AdditionalData);
            ObjectAdditionalDataRiskJudicialSurety.AdditionalDataLoad();
        }
        else {
            glbRisk.Class.ShowPanelsRisk(MenuType.AdditionalData);
            ObjectAdditionalDataRiskJudicialSurety.AdditionalDataLoad();
        }
    }
    static AdditionalDataLoad() {
        if (glbRisk != null) {           
          
            if (glbRisk.Attorney != null) {   
                $("#AttorneyName").val(glbRisk.Attorney.Name);
                $("#selectDescriptionId").UifSelect("setSelected", glbRisk.Attorney.IdentificationDocument.DocumentType.Id);
                $("#NumberId").val(glbRisk.Attorney.IdentificationDocument.Number);
                $("#CardProfessional").val(glbRisk.Attorney.IdProfessionalCard);
                $("#inputInsured").val(glbRisk.Attorney.InsuredToPrint);
            }
             
        }
    }
    static SearchInsured() {
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        if ($.trim($("#inputInsured").val()).length > 0) {
            RiskJudicialSurety.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val(), InsuredSearchType.DocumentNumber, null)
        }
    }
}