var dataInsuredGuarantee = {};

class Binnacle extends Uif2.Page {

    getInitialState() {

    }

    bindEvents() {

        $("#btnBinnacle").click(function () {
            dataInsuredGuarantee = Guarantee.GetinsuredGuarantee();
            if (dataInsuredGuarantee.Id != null) {
                Binnacle.LoadPartialBinnacle();
                Guarantee.HidePanelsGuarantee();
                $("#buttonsGuarantee").hide();
                $('#Observation').val("");
                Guarantee.ShowPanelsGuarantee(MenuType.BINNACLE);

            } else {
                $.UifDialog('alert', { 'message': "Validar guardado de contragarantia." });
            }
        });

        //Seccion Eventos
        $("#btnCancelBinnacle").click(function () {
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        });

        $("#btnNewBinnacle").click(function () {

            $("#AddBinnacle").validate();
            var isValid = $("#AddBinnacle").valid();
            var text = "";

            if (isValid) {
                var observation = $('#Observation').val();
                var num = $('#Observation').val() == '' ? 0 : 1;

                var InsuredGuaranteeLogData = {
                    IndividualId: dataInsuredGuarantee.IndividualId,
                    GuaranteeId: dataInsuredGuarantee.Id,
                    GuaranteeStatusCode: $("#selectStatusGuarantee").UifSelect("getSelected"),
                    LogDate: DateNowPerson,
                    Description: observation
                };

                GuaranteeRequest.CreateInsuredGuaranteeLog(InsuredGuaranteeLogData).done(function (data) {
                    if (data.success) {
                        var num = 0;
                        var list = $("#listBinnacle").UifListView("getData");
                        list.forEach(function (element) {
                            num += 1;
                        });
                        text = "(" + num + ")";
                        $('#selectedBinnacle').html(text);
                    } else {
                        $.UifDialog('alert', { 'message': AppResources.ErrorSavingTheLog });
                    }
                });
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);

            }
        });
    }

    static LoadPartialBinnacle() {
        if (dataInsuredGuarantee.Id != null) {
            GuaranteeRequest.GetInsuredGuaranteeLog(dataInsuredGuarantee.IndividualId, dataInsuredGuarantee.Id).done(function (data) {
                if (data.success) {

                    var num = 0;
                    var text = "";
                    var list = $("#listBinnacle").UifListView("getData");
                    list.forEach(function (element) {
                        num += 1;
                    });
                    text = "(" + num + ")";
                    $('#selectedBinnacle').html(text);

                    $("#listBinnacle").UifListView(
                        {
                            sourceData: data.result,
                            displayTemplate: "#display-binnacle",
                            height: 200
                        });
                }
                else {
                    $.UifDialog('alert', { 'message': AppResources.ErrorGetLog });
                }
            });

            $('#Binnacle-NumberDocument').val(numberDoc);
            $('#inputTypeGuarantee').val(dataInsuredGuarantee.Id);
            $('#inputBinacleSecureName').val($("#inputContractorName").text());
        }
    }
}

