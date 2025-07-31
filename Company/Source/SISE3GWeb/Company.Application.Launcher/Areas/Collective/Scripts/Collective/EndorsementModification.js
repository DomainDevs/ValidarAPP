class Type {
    static SaveTemporal(temporal) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Collective/Collective/SetTemporal',
            data: JSON.stringify({ temporalModel: temporal }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetLoadTypes() {
        return $.ajax({
            type: 'POST',
            url: 'GetLoadTypes',
            data: JSON.stringify({ endorsementType: MassiveProcessType.Emission }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}
$(() => {
    new EndoresemenModificationCollective();
});
class EndoresemenModificationCollective extends Uif2.Page {
    getInitialState() {
        $("textarea").TextTransform(ValidatorType.UpperCase);
        this.LoadSummaryEndorsement();
    }
    bindEvents() {
        $("#buttonsModification").show();
        $('#btnCreate').on('click', this.CreateTemporal);
        $("#btnExit").on('click', this.RedirectSearchController);
    }
    CreateTemporal() {
        if (EndorsementCollectiveModel.PolicyNumber > 0 && $("#selectTypeLoad").UifSelect("getSelected") != "") {
            var temporal = {};
            temporal.DocumentNumber = EndorsementCollectiveModel.PolicyNumber;
            temporal.Branch = { Id: EndorsementCollectiveModel.BranchId };
            temporal.Prefix = { Id: EndorsementCollectiveModel.Prefix };
            temporal.Text = { Observations: $("#inputText").val() };
            temporal.Endorsement = { Id: EndorsementCollectiveModel.EndorsementId };
            temporal.Id = EndorsementCollectiveModel.PolicyId;
            temporal.SubEndorsementType = $("#selectTypeLoad").UifSelect("getSelected");
            Type.SaveTemporal(temporal).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.Id != 0) {
                        $("#inputText").val("");
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelTemporyIssuance + ': ' + data.result.Id });
                        EndorsementCollectiveModel.TempId = data.result.Id;
                        $.redirect(rootPath + 'Collective/Collective/Collective', EndorsementCollectiveModel);
                    }
                    else {
                        $("#inputText").val("");
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.Message });
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorCreateTemporary, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Requerido:' + Resources.Language.lblDocumentNumber + '\n' + Resources.Language.ProcessTypeObligatory, 'autoclose': true });
        }
    }
    RedirectSearchController() {
        var searchModel = {
            BranchId: EndorsementCollectiveModel.BranchId,
            PrefixId: EndorsementCollectiveModel.Prefix,
            PolicyNumber: EndorsementCollectiveModel.PolicyNumber,
            EndorsementId: EndorsementCollectiveModel.EndorsementId
        };
        $.redirect(rootPath + 'Endorsement/Search/Search?type=2', searchModel);
    }
    LoadSummaryEndorsement() {
        $('#labelRisk').text(EndorsementCollectiveModel.RiskCount);
        $('#labelSum').text(FormatMoney(EndorsementCollectiveModel.AmountInsured));
        $('#labelPremium').text(FormatMoney(EndorsementCollectiveModel.Premium));
        $('#labelExpenses').text(FormatMoney(EndorsementCollectiveModel.Expenses));
        $('#labelTaxes').text(FormatMoney(EndorsementCollectiveModel.Taxes));
        $('#labelTotalPremium').text(FormatMoney(EndorsementCollectiveModel.FullPremium));

        Type.GetLoadTypes().done(function (data) {
            if (data.success) {
                var loadTypes = [];
                $.each(data.result, function (index, value) {
                    if (this.Id != SubMassiveProcessType.CollectiveEmission) {
                        loadTypes.push({
                            Id: this.Id,
                            Description: this.Description
                        });
                    }
                });
                //if (EndorsementCollectiveModel.LoadTypeId == 0) {
                //    EndorsementCollectiveModel.LoadTypeId = loadTypes[0].Id;
                //}
                $("#selectTypeLoad").UifSelect({ sourceData: loadTypes });
                if (EndorsementCollectiveModel.LoadTypeId != null && EndorsementCollectiveModel.LoadTypeId != 0) {
                    $("#selectTypeLoad").UifSelect({ sourceData: data.result, selectedId: EndorsementCollectiveModel.LoadTypeId });
                    $("#selectTypeLoad").prop('disabled', 'disabled');
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        this.LoadTitle();
        $("#formEndorsementModification").validate({
            ignore: ".ignore"
        });
        $("#formEndorsementModification").valid();
    }
    LoadTitle(data) {
        var titlePrincipal = Resources.Language.TemporaryCreationModification;
        $.uif2.helpers.setGlobalTitle(titlePrincipal);
    }
}
