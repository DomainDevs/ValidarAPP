var guaranteePrefixs = []

class PrefixAssocieted extends Uif2.Page {

    getInitialState() { }

    bindEvents() {
        $("#btnPrefixAssociated").click(function () {
            dataInsuredGuarantee = Guarantee.GetinsuredGuarantee();
            if (dataInsuredGuarantee.Id != null) {
                PrefixAssocieted.LoadPartialPrefixAssociated();
                Guarantee.HidePanelsGuarantee();
                $("#buttonsGuarantee").hide();
                Guarantee.ShowPanelsGuarantee(MenuType.PREFIXASSOCIATED);
            } else {
                $.UifDialog('alert', { 'message': "Validar guardado de contragarantia." });
            }
        });
        $("#btnCancelPrefixAssociated").click(function () {
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);

        });
        $("#btnNewBranchAssociated").click(this.SaveBranch);

    }

    SaveBranch() {
        var data = Guarantee.GetinsuredGuarantee();

        if (data.Id != null) {
            var selectedPrefix = $("#tablePrefixAssociated").UifDataTable('getSelected');

            var Prefixs = []
            var prefixsQnt = 0

            $.each(selectedPrefix, function () {
                if (guaranteePrefixs.findIndex(x => x.PrefixCode === this.Id) < 0) {
                    Prefixs.push({
                        IndividualId: data.IndividualId,
                        GuaranteeId: data.Id,
                        PrefixCode: this.Id,
                        Parameter: 2 //Se crea el objeto

                    });
                    prefixsQnt++
                } else {
                    Prefixs.push({
                        IndividualId: data.IndividualId,
                        GuaranteeId: data.Id,
                        PrefixCode: this.Id,
                        Parameter: 3 //Se actualiza el objeto
                    });
                    prefixsQnt++
                }
            });
            $.each(guaranteePrefixs, function () {
                if (selectedPrefix.findIndex(x => x.Id === this.PrefixCode) < 0) {
                    Prefixs.push({
                        IndividualId: data.IndividualId,
                        GuaranteeId: data.Id,
                        PrefixCode: this.PrefixCode,
                        Parameter: 4 //Se elimina el objeto
                    });
                }
            })
            GuaranteeRequest.CreateInsuredGuaranteePrefix(Prefixs).done(function (data) {
                if (data.success) {
                    $('#selectedPrefixAssociated').html("(" + prefixsQnt + ")");

                    Guarantee.HidePanelsGuarantee();
                    Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);

                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveGuarantees, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveGuarantees, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectGuarantee, 'autoclose': true });
        };
    }

    static LoadPartialPrefixAssociated() {
        var guarantee = Guarantee.GetinsuredGuarantee();
        if (guarantee.Id != null) {
            $('#inputTypeGuaranteePrefixAssociated').val(guarantee.Guarantee.Description + "  " + guarantee.Id);
            GuaranteeRequest.GetPrefixAssociated($('#hardRiskType_Code').val()).done(function (data) {
                if (data.success) {
                    $("#tablePrefixAssociated").UifDataTable({ sourceData: data.result });
                    PrefixAssocieted.LoadPrefix(individualId, guarantee.Id);
                }
            });
        }
    }
    static LoadPrefix(individualId, guaranteeId) {
        GuaranteeRequest.GetAplicationInsuredGuaranteePrefixByindividualIdByguaranteeId(individualId, guaranteeId).done(function (data) {
            if (data.success) {
                var object = [];
                guaranteePrefixs = data.result;
                $.each(data.result, function () {
                    object.push(this.PrefixCode);
                });
                var items = {
                    label: 'Id',
                    values: object
                }
                $('#selectedPrefixAssociated').html("(" + object.length + ")");
                $("#tablePrefixAssociated").UifDataTable('setSelect', items)
            }

        });
    }

}
