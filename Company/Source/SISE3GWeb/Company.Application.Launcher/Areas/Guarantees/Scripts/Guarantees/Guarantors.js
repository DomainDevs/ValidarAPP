//Variables globales
var guarantor = "";
var guarantorIndex = null;
var GuarantorsTmp = [];
var guarantorTmp = {};
var guarantorCardNro = "";

var objGuarantors =
{
    bindEvents: function () {
        $("#Guarantors-Phone").ValidatorKey(ValidatorType.Number, 2, 0);

        $("#btnGuarantors").on("click", function () {
            $("#Guarantee").validate();
            $("#AddPromissoryNote").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddPromissoryNote").valid();

            if (guaranteeValid && promissoryNoteValid) {
                objGuarantors.LoadPartialGuarantors();
                Guarantee.LoadListGuarantors(guaranteeId);
                GuarantorsTmp.push($("#tableGuarantors").UifListView("getData"));
                $("#IndividualName-Guarantors").text($("#inputContractorName").text());
                $("#NumberDocument-Guarantors").text(numberDoc);
            }
        });

        $("#btnAcceptGuarantor").click(function () {

            $("#formGuarantor").validate();
            var guarantorValidate = $("#formGuarantor").valid();

            if ($('#naturalPerson').is(':checked') && $("#Guarantor-Name").val().length > 30) {
                guarantorValidate = false;
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NamePersonNatural, 'autoclose': true })
            }

            if (guarantorValidate) {
                objGuarantors.pushGuarantor(guaranteeId, guarantor);
            }
            objGuarantors.clearGuarantor();
        });

        $("#btnRecordGuarantors").click(function () {
            objGuarantors.setSignatoriesNumber(guaranteeId);
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        });
        $('#tableGuarantors').on('rowEdit', function (event, data, index) {
            guarantorCardNro = data.CardNro;
            guarantor = data.GuarantorId;
            guarantorIndex = index;
            objGuarantors.showGuarantor(data);
        });

        $('#btnNewGuarantor').on('click', function () {
            objGuarantors.clearGuarantor();
        });

        $("#btnCancelGuarantors").click(function () {
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        });
        $('#naturalPerson').click(function () {
            if ($('#naturalPerson').is(':checked')) {
                $("#Guarantor-Name").attr('maxlength', 30);
            }
        });
        $('#legalPerson').click(function () {
            if ($('#legalPerson').is(':checked')) {
                $("#Guarantor-Name").attr('maxlength', 120);
            }
        });
        if ($('#naturalPerson').is(':checked')) {
            $("#Guarantor-Name").attr('maxlength', 30);
        }
        if ($('#legalPerson').is(':checked')) {
            $("#Guarantor-Name").attr('maxlength', 120);
        }
        $("#PromissoryNote-ExpirationDate").on('datepicker.change', this.ChangeTo);
    },

    setSignatoriesNumber: function (guaranteeId) {
        var signatoriesNumber = 0;
        var result = Guarantee.existGuarantee(guaranteeId);
        var list = $("#tableGuarantors").UifListView("getData");

        if (result >= 0) {
            guarantee[result].InsuredGuarantee.Guarantors = list;
        }
        else if (guaranteeId == 0) {
            guaranteeTmp.InsuredGuarantee.Guarantors = list;
        }
        /////////////////////////////////////////////


        signatoriesNumber = list.length;

        $("#PromissoryNote-SignatoriesNumber").val(signatoriesNumber);
    },
    ChangeTo(event, date) {
        if (CompareDates($("#PromissoryNote-ConstitutionDate").val(), $("#PromissoryNote-ExpirationDate").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoCanDateExpGreaterDateConst, 'autoclose': true });
            $("#PromissoryNote-ExpirationDate").UifDatepicker('setValue', null);
        }
    },

    LoadPartialGuarantors: function () {
        Guarantee.ShowPanelsGuarantee(MenuType.GUARANTORS);
    },

    deleteGuarantor: function (deferred, data) {
        deferred.resolve();
    },

    clearGuarantor: function () {
        guarantor = "";
        $("#Guarantor-Name").val("");
        $("#Guarantors-DocumentNumber").val("");
        $("#Guarantors-Address").val("");
        $("#Guarantor-City").val("");
        $("#Guarantors-Phone").val("");
        guarantorIndex = null;
    },

    showGuarantor: function (data) {

        var name = "";
        if (data.Name != " ") {
            name = data.Name;
            $("#naturalPerson").prop("checked", true);
            $("#Guarantor-Name").attr('maxlength', 30);
        }
        else {
            name = data.TradeName;
            $("#legalPerson").prop("checked", true);
            $("#Guarantor-Name").attr('maxlength', 120);
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
    },

    existGuarantor: function (indexguarantee, idGuarantor) {
        var result = -1;

        var list = $("#tableGuarantors").UifListView("getData");

        if (list != null) {
            for (var i = 0; i < list.length; i++) {
                if (list[i] != null && list[i].GuarantorId == idGuarantor) {
                    result = i;
                    break;
                }
            }
        }

        /*if (guarantee[indexguarantee].InsuredGuarantee.Guarantors != null) {
            for (var i = 0; i < guarantee[indexguarantee].InsuredGuarantee.Guarantors.length; i++) {
    
                if (list[i] != null && guarantee[indexguarantee].InsuredGuarantee.Guarantors[i].GuarantorId == idGuarantor) {
    
                    result = i;
                    break;
                }
            }
        }*/
        return result;
    },

    existGuarantorNumberId: function (indexguarantee, numberGuarantor) {
        var result = -1;

        var list = $("#tableGuarantors").UifListView("getData");

        for (var i = 0; i < list.length; i++) {
            if (list[i].CardNro != null && list[i].CardNro == numberGuarantor) {
                result = i;
                break;
            }
        }
        return result;
    },

    existGuarantorTmpNumberId: function (numberGuarantor) {
        var result = -1;

        if (GuarantorsTmp != null) {

            for (var i = 0; i < GuarantorsTmp.length; i++) {

                if (GuarantorsTmp[i].CardNro != null && GuarantorsTmp[i].CardNro == numberGuarantor) {
                    result = i;
                    break;
                }
            }
        }
        return result;
    },

    pushGuarantor: function (idGuarantee, idGuarantor) {
        var result = Guarantee.existGuarantee(idGuarantee);

        if (result >= 0) {

            var result2 = objGuarantors.existGuarantor(result, idGuarantor);
            var numberGuarantor = objGuarantors.existGuarantorNumberId(result, $("#Guarantors-DocumentNumber").val())

            if (result2 >= 0) {

                if (numberGuarantor >= 0 && $("#Guarantors-DocumentNumber").val() != guarantorCardNro && guarantorIndex == null) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.DocumentNumberAlreadyRegistered, 'autoclose': true });
                }
                else {

                    var radioValue = $("input[name='personType']:checked").val();

                    if (radioValue == "naturalPerson") {
                        guarantee[result].InsuredGuarantee.Guarantors[result2].Name = $("#Guarantor-Name").val();
                        guarantee[result].InsuredGuarantee.Guarantors[result2].CardNro = $("#Guarantors-DocumentNumber").val();
                        guarantee[result].InsuredGuarantee.Guarantors[result2].TradeName = " ";
                        guarantee[result].InsuredGuarantee.Guarantors[result2].TributaryIdNo = " ";
                    }
                    else {
                        guarantee[result].InsuredGuarantee.Guarantors[result2].TradeName = $("#Guarantor-Name").val();
                        guarantee[result].InsuredGuarantee.Guarantors[result2].TributaryIdNo = $("#Guarantors-DocumentNumber").val();
                        guarantee[result].InsuredGuarantee.Guarantors[result2].Name = " ";
                        guarantee[result].InsuredGuarantee.Guarantors[result2].CardNro = " ";
                    }

                    guarantee[result].InsuredGuarantee.Guarantors[result2].Adrress = $("#Guarantors-Address").val();
                    guarantee[result].InsuredGuarantee.Guarantors[result2].PhoneNumber = $("#Guarantors-Phone").val();
                    guarantee[result].InsuredGuarantee.Guarantors[result2].CityText = $("#Guarantor-City").val();
                    $("#tableGuarantors").UifListView("editItem", guarantorIndex, guarantee[result].InsuredGuarantee.Guarantors[result2]);
                }
            }
            else {
                if (numberGuarantor >= 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.DocumentNumberAlreadyRegistered, 'autoclose': true });
                }
                else {
                    var guarantor = {};

                    var radioValue = $("input[name='personType']:checked").val();

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
                    guarantor.IndividualId = $('#ContractorId').val();
                    guarantor.GuaranteeId = idGuarantee;

                    if (guarantee[result].InsuredGuarantee.Guarantors == null) {
                        guarantee[result].InsuredGuarantee.Guarantors = [];
                    }

                    guarantee[result].InsuredGuarantee.Guarantors.push(guarantor);
                    $("#tableGuarantors").UifListView("addItem", guarantor);
                }
            }
        }

        else {

            var result = objGuarantors.existGuarantorTmpNumberId($("#Guarantors-DocumentNumber").val());

            if (result >= 0 && guarantorIndex == null) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.DocumentNumberAlreadyRegistered, 'autoclose': true });
            }

            else {

                var radioValue = $("input[name='personType']:checked").val();

                guarantorTmp = {}
                if (radioValue == "naturalPerson") {
                    guarantorTmp.Name = $("#Guarantor-Name").val();
                    guarantorTmp.CardNro = $("#Guarantors-DocumentNumber").val();
                    guarantorTmp.TradeName = " ";
                    guarantorTmp.TributaryIdNo = " ";
                }
                else {
                    guarantorTmp.TradeName = $("#Guarantor-Name").val();
                    guarantorTmp.TributaryIdNo = $("#Guarantors-DocumentNumber").val();
                    guarantorTmp.Name = " ";
                    guarantorTmp.CardNro = " ";
                }

                guarantorTmp.Adrress = $("#Guarantors-Address").val();
                guarantorTmp.PhoneNumber = $("#Guarantors-Phone").val();
                guarantorTmp.CityText = $("#Guarantor-City").val();
                guarantorTmp.IndividualId = $('#ContractorId').val();
                guarantorTmp.GuaranteeId = idGuarantee;

                GuarantorsTmp.push(guarantorTmp);
                if (guarantorIndex == null) {
                    $("#tableGuarantors").UifListView("addItem", guarantorTmp);
                }
                else {
                    $("#tableGuarantors").UifListView("editItem", guarantorIndex, guarantorTmp);
                }
                guaranteeTmp.InsuredGuarantee.Guarantors = GuarantorsTmp;
            }
        }
    }
}
