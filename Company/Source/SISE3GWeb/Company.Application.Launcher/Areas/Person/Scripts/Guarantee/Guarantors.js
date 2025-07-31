//Variables globales
var guarantorId = 0;
var guarantorIndex = null;
var ListGuarantor = [];

class Guarantors extends Uif2.Page {

    getInitialState() {
        //$('#Guarantors-DocumentNumber').ValidatorKey(ValidatorType.Number);
        $('#Guarantors-Phone').ValidatorKey(ValidatorType.Number);
    }

    bindEvents() {
        $("#btnAcceptGuarantor").click(function () {

            $("#formGuarantor").validate();
            var guarantorValidate = $("#formGuarantor").valid();

            if (guarantorValidate) {
                Guarantors.pushGuarantor();
            }
            Guarantors.clearGuarantor();
        });

        $("#btnRecordGuarantors").click(function () {
            Guarantors.CreateGuartor();
            Guarantors.setSignatoriesNumber();
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);

        });

        $('#tableGuarantors').on('rowEdit', function (event, data, index) {
            if (data.ParametrizationStatus == 1) {
                data.ParametrizationStatus = 3;
            }
            guarantorId = data.GuarantorId;
            guarantorIndex = index;
            Guarantors.showGuarantor(data);
        });

        $('#btnNewGuarantor').on('click', function () {
            Guarantors.clearGuarantor();
        });

        $("#btnCancelGuarantors").click(function () {
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        });
    }

    static loadTableGuarantors() {
        var data = Guarantee.GetinsuredGuarantee();
        GuaranteeRequest.GetGuarantorByIndividualIdById(data.IndividualId, data.Id).done(function (data) {
            if (data.success) {
                $("#tableGuarantors").UifListView(
                    {
                        sourceData: data.result,
                        customAdd: true,
                        customEdit: true,
                        delete: true,
                        deleteCallback: Guarantors.deleteGuarantor,
                        edit: true, displayTemplate: "#templateGuarantors",
                        addTemplate: '#add-template',
                        height: 300
                    });
            } else {

            }
        });
    }

    static setSignatoriesNumber() {
        var signatoriesNumber = 0;

        var list = $("#tableGuarantors").UifListView("getData");
        signatoriesNumber = list.length;

        $("#PromissoryNote-SignatoriesNumber").val(signatoriesNumber);

    }

    static LoadPartialGuarantors() {
        Guarantee.ShowPanelsGuarantee(MenuType.GUARANTORS);
    }

    static deleteGuarantor(deferred, data) {
        data.ParametrizationStatus = 4;
        ListGuarantor.push(data);
        deferred.resolve();
    }

    static clearGuarantor() {
        guarantorId = 0;
        $("#Guarantor-Name").val("");
        $("#Guarantors-DocumentNumber").val("");
        $("#Guarantors-Address").val("");
        $("#Guarantor-City").val("");
        $("#Guarantors-Phone").val("");
        guarantorIndex = null;
    }

    static showGuarantor(data) {

        var name = "";
        if (data.Name != " ") {
            name = data.Name;
            $("#naturalPerson").prop("checked", true)
        }
        else {
            name = data.TradeName;
            $("#legalPerson").prop("checked", true)
        }

        var number = "";
        if (data.CardNro != " ") {
            number = data.CardNro;
        }
        else {
            number = data.TributaryIdNo;
        }

        $("#Guarantors-Address").val(data.Adrress);
        $("#Guarantors-DocumentNumber").val(number);
        $("#Guarantor-City").val(data.CityText);
        $("#Guarantor-Name").val(name);
        $("#Guarantors-Phone").val(data.PhoneNumber);
    }

    static existGuarantorNumberId(indexguarantee, numberGuarantor) {
        var result = -1;

        var list = $("#tableGuarantors").UifListView("getData");

        for (var i = 0; i < list.length; i++) {
            if (list[i].CardNro != null && list[i].CardNro == numberGuarantor) {
                if (i != indexguarantee) {
                    result = i;
                    break;
                }
            }
        }
        return result;
    }

    static pushGuarantor() {

        var numberGuarantor = Guarantors.existGuarantorNumberId(guarantorIndex, $("#Guarantors-DocumentNumber").val())
        var guarantee = Guarantee.GetinsuredGuarantee();
        if (numberGuarantor >= 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.DocumentNumberAlreadyRegistered, 'autoclose': true });
        } else {
            var guarantor = {};
            guarantor.GuarantorId = guarantorId;

            var radioValue = $("input[name='personType']:checked").val();
            guarantor.GuaranteeId = guarantee.Id;
            guarantor.IndividualId = guarantee.IndividualId;

            if (radioValue == "naturalPerson") {
                guarantor.Name = $("#Guarantor-Name").val();
                guarantor.CardNro = $("#Guarantors-DocumentNumber").val();
                guarantor.TradeName = " ";
                guarantor.TributaryIdNo = " ";

            }
            else {
                guarantor.TradeName = $("#Guarantor-Name").val();
                guarantor.TributaryIdNo = $("#Guarantors-DocumentNumber").val();
                guarantor.Name = " ";
                guarantor.CardNro = " ";
            }

            guarantor.Adrress = $("#Guarantors-Address").val();
            guarantor.PhoneNumber = $("#Guarantors-Phone").val();
            guarantor.CityText = $("#Guarantor-City").val();

            if (guarantorIndex == null) {
                guarantor.ParametrizationStatus = 2;
                $("#tableGuarantors").UifListView("addItem", guarantor);
            } else {
                if (guarantor.GuarantorId > 0) {
                    guarantor.ParametrizationStatus = 3;
                } else {
                    guarantor.ParametrizationStatus = 2;
                }
                $("#tableGuarantors").UifListView("editItem", guarantorIndex, guarantor);
            }

        }


    }

    static CreateGuartor() {
        var list = $("#tableGuarantors").UifListView("getData");

        ListGuarantor.forEach(function (element) {
            list.push(element);
        });

        GuaranteeRequest.CreateGuarantor(list).done(function (data) {
            if (!data.success) {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            } else {
                ListGuarantor = [];
            }
        });

    }

}
