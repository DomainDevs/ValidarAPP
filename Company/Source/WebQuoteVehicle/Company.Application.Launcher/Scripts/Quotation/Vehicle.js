//Codigo de la pagina Quotation/Index.cshtml
var fasecoldaCode = '';
var searchingCustomer = false;
var glbQuotation = null;
var gblRiskNum = 0;
var glbPath = null;
var glbFileName = null;
var glbFilePath = null;
$(document).ready(function () {
    $.uif2.helpers.setGlobalTitle($('#hiddenTitle').val());
    $('#inputQuotationId').ValidatorKey(ValidatorType.Number, 1, 1);
    $("#inputPlate").ValidatorKey(7, 0, 1);
    $('#inputFasecoldaCode').ValidatorKey(ValidatorType.Number, 1, 1);
    $('#inputInsuredAmount').OnlyDecimals(2);
    $('#inputAmountAccesories').OnlyDecimals(2);
    $('#inputDocument').ValidatorKey(ValidatorType.Number, 1, 1);
    $('#inputTradeName').ValidatorKey(ValidatorType.lettersandnumbersSpecial, 1, 1);
    $('#inputNames').ValidatorKey(ValidatorType.Letter, 1, 1);
    $('#inputSurname').ValidatorKey(ValidatorType.Letter, 1, 1);
    $('#inputSecondSurname').ValidatorKey(ValidatorType.Letter, 1, 1);
    $('#inputBirthDate').ValidatorKey(ValidatorType.Dates, 1, 1);
    $('#inputPersonAddress').ValidatorKey(ValidatorType.Addresses, 1, 1);
    $('#inputPersonPhone').ValidatorKey(ValidatorType.Number, 1, 1);
    $('#inputEmail').ValidatorKey(ValidatorType.Emails, 1, 1);

    $('input[type=text]').keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('.panelPerson').show();
    $('.panelCompany').hide();
    $('#panelGetQuotation').hide();
    $('#panelSummary').hide();
    GetDocumentTypesByIndividualType(1, 0);
    $("#listCoverages").UifListView({ source: null, add: false, edit: false, delete: false, displayTemplate: "#coverageTemplate", title: "Coberturas", height: 590 });
    $('#inputIssueDate').text(GetCurrentFromDate());

    //if (!ValidateIntermediates()) {
    //    $(":input").attr("disabled", "disabled");
    //}
    //else {
    //    $(':input').removeAttr('disabled');
    //}

    if ($('#radioIndividualTypePerson').prop('checked')) {
        $('#inputVerifyDigit').hide();
    } else {
        $('#inputVerifyDigit').show();
    }
});

//Eventos
$('#inputQuotationId').on('buttonClick', function () {
    if ($('#inputQuotationId').val().trim().length > 0) {
        GetQuotationByQuotationId($('#inputQuotationId').val().trim());
    }
});

$('#btnSearchVehicle').on('click', function () {
    if ($("#inputFasecoldaCode").val() == "") {
        $("#ReqFasecoldaCode").show();
        return;
    }
    else {
        $("#ReqFasecoldaCode").hide();
    }
    if ($('#inputPlate').val().trim().length > 0 || ($('#inputFasecoldaCode').val().trim().length > 0 && $('#inputFasecoldaCode').val().trim() != fasecoldaCode)) {
            GetVehicleByPlateFasecoldaCode($('#inputPlate').val().trim(), $('#inputFasecoldaCode').val().trim());
    }
    
});

$('#selectMake').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        GetModelsByMakeId(selectedItem.Id, 0);
    }
    else {
        $('#selectModel').UifSelect();
        $('#selectVersion').UifSelect();
        $('#selectType').UifSelect();
        $('#selectYear').UifSelect();
        $('#inputInsuredAmount').val(0);
        $('#inputFasecoldaCode').val('');
    }
});

$('#selectModel').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        GetVersionsByMakeIdModelId($('#selectMake').UifSelect('getSelected'), selectedItem.Id, 0);
    }
    else {
        $('#selectVersion').UifSelect();
        $('#selectType').UifSelect();
        $('#selectYear').UifSelect();
        $('#inputInsuredAmount').val(0);
        $('#inputFasecoldaCode').val('');
    }
});

$('#selectVersion').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        GetYearsByMakeIdModelIdVersionId($('#selectMake').UifSelect('getSelected'), $('#selectModel').UifSelect('getSelected'), selectedItem.Id, 0);
        GetVehicleByMakeIdModelIdVersionId($('#selectMake').UifSelect('getSelected'), $('#selectModel').UifSelect('getSelected'), selectedItem.Id);
    }
    else {
        $('#selectType').UifSelect();
        $('#selectYear').UifSelect();
        $('#inputInsuredAmount').val(0);
        $('#inputFasecoldaCode').val('');
    }
});

$('#selectYear').on('itemSelected', function (event, selectedItem) {
    ClearSummary();
    if (selectedItem.Id > 0) {
        GetPriceByMakeIdModelIdVersionId($('#selectMake').UifSelect('getSelected'), $('#selectModel').UifSelect('getSelected'), $('#selectVersion').UifSelect('getSelected'), selectedItem.Id);
    }
    else {
        $('#inputInsuredAmount').val(0);
    }
});

$('#inputInsuredAmount').focusin(function () {
    var value = NotFormatMoney($.trim($(this).val()));
    value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
});

$('#inputInsuredAmount').focusout(function () {
    var value = NotFormatMoney($.trim($(this).val()));
    value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
});

$('#inputInsuredAmount').keypress(function (e) {
    var chartlong = $('#inputInsuredAmount').val()
    if (chartlong.length >= 16) {
        e.preventDefault();
    }
});

$('#inputAmountAccesories').focusin(function () {
    ClearSummary();
    $(this).val(NotFormatMoney($(this).val()));
});

$('#inputAmountAccesories').focusout(function () {
    if ($.trim($(this).val()) != '') {
        $(this).val(FormatMoney($(this).val()));
    }
});

$('#radioIndividualTypePerson').change(function () {
    if ($('#radioIndividualTypePerson').prop('checked')) {
        $('.panelPerson').show();
        $('.panelCompany').hide();
        ClearCustomer();
        GetDocumentTypesByIndividualType(1, 0);
        $('#inputVerifyDigit').val('');
        $('#inputVerifyDigit').hide();
    }
});

$('#radioIndividualTypeCompany').change(function () {
    if ($('#radioIndividualTypeCompany').prop('checked')) {
        $('.panelPerson').hide();
        $('.panelCompany').show();
        ClearCustomer();
        GetDocumentTypesByIndividualType(2, 0);
        $('#inputDocument').focusout();
        $('#inputVerifyDigit').show();
    }
});

$('#inputDocument').focusout(function () {
    if ($.trim($(this).val()) != '') {
        if ($('#radioIndividualTypeCompany').prop('checked')) {
            $('#inputVerifyDigit').val(CalculateDigitVerify($(this).val()));
        }
        SearchCustomer();
    }
});

$('#btnSearchCustomer').click(function () {
    SearchCustomer();
});

$('#selectProduct').on('itemSelected', function (event, selectedItem) {
    ClearSummary();
    if (selectedItem.Id > 0) {
        GetGroupCoveragesByProductId(selectedItem.Id, 0);
    }
    else {
        $('#selectGroupCoverage').UifSelect();
        $('#selectLimitRC').UifSelect();
    }
});

$('#selectGroupCoverage').on('itemSelected', function (event, selectedItem) {
    ClearSummary();
    if (selectedItem.Id > 0) {
        GetCoveragesByProductIdGroupCoverageId($('#selectProduct').UifSelect('getSelected'), selectedItem.Id);
    }
    else {
        $("#listCoverages").UifListView("refresh");
    }
});

$('#btnNewQuotation').on('click', function () {
    ClearQuotation();
});

$('#btnQuote').on('click', function () {
    $.UifDialog('confirm', { 'message': "¿Desea realizar cotización para los datos ingresados?" }, function (result) {
        if (result) {
            Quote();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    });
});

$('#btnDownloadPDF').on('click', function () {
    GeneratePDF(glbQuotation);
    DownloadPDF(glbPath, true, glbFileName);
});

$('#btnSendToEmail').on('click', function () {
    var path = GeneratePDF(glbQuotation);
    SendToEmail(glbFilePath);
});

$("#inputFasecoldaCode").focusin(FasecoldaCodeFocusIn);

$("#inputPlate").focusout(ValidationPlate);

//Metodos
function GetQuotationByQuotationId(quotationId) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/GetQuotationByQuotationId',
        data: JSON.stringify({ quotationId: quotationId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        ClearQuotation();
        if (data.success) {
            LoadQuotation(data.result);
        }
        else {

            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        ClearVehicle();
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al consultar cotización', 'autoclose': true });
    });
}

function LoadQuotation(quotation) {
    //var vehicle = quotation.CompanyVehicles[0];
    glbQuotation = quotation;
    var vehicle = quotation;
    $('#inputPlate').val(vehicle.LicensePlate);
    $('#inputFasecoldaCode').val(vehicle.Fasecolda.Description);
    $('#selectMake').UifSelect('setSelected', vehicle.Make.Id);
    GetModelsByMakeId(vehicle.Make.Id, vehicle.Model.Id);
    GetVersionsByMakeIdModelId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
    GetTypesByTypeId(vehicle.Version.Type.Id, vehicle.Version.Type.Id);
    GetYearsByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id, vehicle.Year);
    $('#selectUse').UifSelect('setSelected', vehicle.Use.Id);
    $('#inputInsuredAmount').val(FormatMoney(vehicle.Price));
    $('#inputAmountAccesories').val(FormatMoney(vehicle.PriceAccesories));
    $('#selectRatingZone').UifSelect('setSelected', vehicle.Risk.RatingZone.Id);
    $('#radioReplacementVehicle').attr('checked', vehicle.ReplacementVehicle);
    GetDocumentTypesByIndividualType(vehicle.Risk.MainInsured.IndividualType, vehicle.Risk.MainInsured.IdentificationDocument.DocumentType.Id);
    $('#inputDocument').val(vehicle.Risk.MainInsured.IdentificationDocument.Number);
    $('#inputVerifyDigit').val(CalculateDigitVerify($('#inputDocument').val()));
    LoadCustomer(vehicle.Risk.MainInsured);
    $('#selectProduct').UifSelect('setSelected', vehicle.Risk.Policy.Product.Id);
    GetGroupCoveragesByProductId(vehicle.Risk.Policy.Product.Id, vehicle.Risk.GroupCoverage.Id, vehicle.Risk.LimitRc.Id);
    if (vehicle.Risk.DynamicProperties.length > 0) {
        if (vehicle.Risk.DynamicProperties[0].Value > 0) {
            $('#selectDiscount').UifSelect('setSelected', vehicle.Risk.DynamicProperties[0].Value);
        }
    }
    LoadPremium(quotation);
}

function GetVehicleByPlateFasecoldaCode(plate, fasecoldaCode) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/GetVehicleByPlateFasecoldaCode',
        data: JSON.stringify({ plate: plate, fasecoldaCode: fasecoldaCode }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            LoadVehicle(data.result);
        }
        else {
            ClearVehicle();
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        ClearVehicle();
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al consultar vehículo', 'autoclose': true });
    });
}

function LoadVehicle(vehicle) {
    $('#inputFasecoldaCode').val(vehicle.Fasecolda.Description);
    $('#selectMake').UifSelect('setSelected', vehicle.Make.Id);
    GetModelsByMakeId(vehicle.Make.Id, vehicle.Model.Id);
    GetVersionsByMakeIdModelId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
    GetTypesByTypeId(vehicle.Version.Type.Id, vehicle.Version.Type.Id);
    GetYearsByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id, vehicle.Year);
    if (vehicle.Use != null) {
        $('#selectUse').UifSelect('setSelected', vehicle.Use.Id);
    }
    $('#inputInsuredAmount').val(FormatMoney(vehicle.Price));
}

function ClearVehicle() {
    $('#selectMake').UifSelect('setSelected', null);
    $('#selectModel').UifSelect();
    $('#selectVersion').UifSelect();
    $('#selectType').UifSelect();
    $('#selectYear').UifSelect();
    $('#inputInsuredAmount').val(0);


}

function GetModelsByMakeId(makeId, selectedId) {
    var controller = rootPath + 'Quotation/GetModelsByMakeId?makeId=' + makeId;

    if (selectedId == 0) {
        $('#selectModel').UifSelect({ source: controller });
    }
    else {
        $('#selectModel').UifSelect({ source: controller, selectedId: selectedId });
    }
}

function GetVersionsByMakeIdModelId(makeId, modelId, selectedId) {
    var controller = rootPath + 'Quotation/GetVersionsByMakeIdModelId?makeId=' + makeId + '&modelId=' + modelId;

    if (selectedId == 0) {
        $('#selectVersion').UifSelect({ source: controller });
    }
    else {
        $('#selectVersion').UifSelect({ source: controller, selectedId: selectedId });
    }
}

function GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId, selectedId) {
    var controller = rootPath + 'Quotation/GetYearsByMakeIdModelIdVersionId?makeId=' + makeId + '&modelId=' + modelId + '&versionId=' + versionId;

    if (selectedId == 0) {
        $('#selectYear').UifSelect({ source: controller });
    }
    else {
        $('#selectYear').UifSelect({ source: controller, selectedId: selectedId });
    }
}

function GetVehicleByMakeIdModelIdVersionId(makeId, modelId, versionId) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/GetVehicleByMakeIdModelIdVersionId',
        data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            $('#inputFasecoldaCode').val(data.result.Fasecolda.Description);
            GetTypesByTypeId(data.result.Version.Type.Id, data.result.Version.Type.Id);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al consultar código fasecolda', 'autoclose': true });
    });
}

function GetTypesByTypeId(typeId, selectedId) {
    var controller = rootPath + 'Quotation/GetTypesByTypeId?typeId=' + typeId;

    if (selectedId == 0) {
        $('#selectType').UifSelect({ source: controller });
    }
    else {
        $('#selectType').UifSelect({ source: controller, selectedId: selectedId });
    }
}

function GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/GetPriceByMakeIdModelIdVersionId',
        data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId, year: year }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            $('#inputInsuredAmount').val(FormatMoney(data.result));
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al consultar suma asegurada', 'autoclose': true });
    });
}

function GetDocumentTypesByIndividualType(individualType, selectedId) {
    var controller = rootPath + 'Quotation/GetDocumentTypesByIndividualType?individualType=' + individualType;

    if (selectedId == 0) {
        $('#selectDocumentType').UifSelect({ source: controller });
    }
    else {
        $('#selectDocumentType').UifSelect({ source: controller, selectedId: selectedId });
    }
}

function CalculateDigitVerify(documentNumber) {
    var vpri, x, y, z, i, nit1, dv1;
    nit1 = documentNumber;
    if (!isNaN(nit1)) {
        vpri = [];
        x = 0; y = 0; z = nit1.length;
        vpri[1] = 3;
        vpri[2] = 7;
        vpri[3] = 13;
        vpri[4] = 17;
        vpri[5] = 19;
        vpri[6] = 23;
        vpri[7] = 29;
        vpri[8] = 37;
        vpri[9] = 41;
        vpri[10] = 43;
        vpri[11] = 47;
        vpri[12] = 53;
        vpri[13] = 59;
        vpri[14] = 67;
        vpri[15] = 71;
        for (i = 0; i < z; i++) {
            y = (nit1.substr(i, 1));
            x += (y * vpri[z - i]);
        }

        y = x % 11;

        if (y > 1) {
            dv1 = 11 - y;
        } else {
            dv1 = y;
        }
        return dv1;
    }
}

function SearchCustomer() {
    if (searchingCustomer === false) {
        if ($('#selectDocumentType').val() > 0 && $('#inputDocument').val() > 0) {
            searchingCustomer = true;
            $.ajax({
                type: 'POST',
                url: rootPath + 'Quotation/GetProspectByIndividualTypeDocumentTypeIdDocument',
                data: JSON.stringify({ individualType: $('input[name=radioIndividualType]:checked').val(), documentTypeId: $('#selectDocumentType').val(), document: $('#inputDocument').val() }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done(function (data) {
                if (data.success) {
                    LoadCustomer(data.result);
                }
                else {
                    ClearCustomer();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                searchingCustomer = false;
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': 'Error al consultar cliente', 'autoclose': true });
                searchingCustomer = false;
            });
        }
    }
}

function LoadCustomer(customer) {
    $('#hiddenCustomerId').val(customer.Id);
    $('#hiddenCustomerType').val(customer.CustomerType);
    if (customer.CompanyName != null && customer.CompanyName.Email != null) {
        $('#inputEmail').val(customer.CompanyName.Email.Description);
    }

    if (customer.IndividualType == 1) {
        $('.panelPerson').show();
        $('.panelCompany').hide();
        $('#radioIndividualTypePerson').prop('checked', true);
        $('#inputNames').val(customer.Name);
        $('#inputSurname').val(customer.Surname);
        $('#inputSecondSurname').val(customer.SecondSurname);
        $('#inputBirthDate').UifDatepicker('setValue', FormatDate(customer.BirthDate));

        if (customer.CompanyName != null && customer.CompanyName.Address != null) {
            $('#inputPersonAddress').val(customer.CompanyName.Address.Description);
        }
        if (customer.CompanyName != null && customer.CompanyName.Phone != null) {
            $('#inputPersonPhone').val(customer.CompanyName.Phone.Description);
        }

        if (customer.Gender == 'M') {
            $('#radioGenderMale').attr('checked', true);
        }
        else {
            $('#radioGenderFemale').attr('checked', true);
        }
    }
    else if (customer.IndividualType == 2) {
        $('.panelPerson').hide();
        $('.panelCompany').show();
        $('#radioIndividualTypeCompany').attr('checked', true);
        $('#inputTradeName').val(customer.TradeName);
        $('#inputCompanyAddress').val(customer.CompanyName.Address.Description);
        if (customer.CompanyName.Phone != null) {
            $('#inputCompanyPhone').val(customer.CompanyName.Phone.Description);
        }
    }
}

function ClearCustomer() {
    $('#hiddenCustomerId').val('');
    $('#hiddenCustomerType').val('');
    $('#inputNames').val('');
    $('#inputSurname').val('');
    $('#inputSecondSurname').val('');
    $('#inputBirthDate').val('');
    $('#inputPersonAddress').val('');
    $('#inputPersonPhone').val('');
    $('#inputTradeName').val('');
    $('#inputCompanyAddress').val('');
    $('#inputCompanyPhone').val('');
    $('#inputEmail').val('');
}

function GetGroupCoveragesByProductId(productId, selectedId, idLimitRc) {
    var controller = rootPath + 'Quotation/GetGroupCoveragesByProductId?productId=' + productId;
    var controllerRC = rootPath + 'Quotation/GetLimitRcByFilter?productId=' + productId;
    if (selectedId == 0 || idLimitRc == 0) {
        $('#selectGroupCoverage').UifSelect({ source: controller });
        $('#selectLimitRC').UifSelect({ source: controllerRC });
    }
    else {
        $('#selectGroupCoverage').UifSelect({ source: controller, selectedId: selectedId });
        $('#selectLimitRC').UifSelect({ source: controllerRC, selectedId: idLimitRc });
    }
}

function GetCoveragesByProductIdGroupCoverageId(productId, groupCoverageId) {
    var controller = rootPath + "Quotation/GetCoveragesByProductIdGroupCoverageId?productId=" + productId + "&groupCoverageId=" + groupCoverageId;
    $("#listCoverages").UifListView({ source: controller, add: false, edit: false, delete: false, displayTemplate: "#coverageTemplate", title: "Cotización", height: 590 });
}

function ClearQuotation() {
    $('#formQuotation').formReset();
    $('#hiddenId').val(0);
    $('#hiddenTemporalId').val('');
    $('#inputQuotationId').val('');
    $('#hiddenQuotationVersion').val('');
    $('#selectModel').UifSelect();
    $('#selectVersion').UifSelect();
    $('#selectType').UifSelect();
    $('#selectYear').UifSelect();
    $('#selectGroupCoverage').UifSelect();
    $('#radioIndividualTypePerson').prop('checked', true);
    $('.panelPerson').show();
    $('.panelCompany').hide();
    $("#listCoverages").UifListView({ source: null, add: false, edit: false, delete: false, displayTemplate: "#coverageTemplate", title: "Coberturas", height: 590 });
    ClearCustomer();
    GetDocumentTypesByIndividualType(1, 0);
    ClearSummary();
    //gblRiskNum = 0;
}

function Quote() {
    $("#formQuotation").validate();

    if ($("#formQuotation").valid()) {
        var quotationModel = $("#formQuotation").serializeObject();
        quotationModel.Id = $('#hiddenId').val();
        quotationModel.TemporalId = $('#hiddenTemporalId').val();
        quotationModel.QuotationId = $('#inputQuotationId').val();
        quotationModel.QuotationVersion = $('#hiddenQuotationVersion').val();
        quotationModel.MakeDescription = $('#selectMake').UifSelect('getSelectedText');
        quotationModel.ModelDescription = $('#selectModel').UifSelect('getSelectedText');
        quotationModel.VersionDescription = $('#selectVersion').UifSelect('getSelectedText');
        quotationModel.InsuredAmount = NotFormatMoney(quotationModel.InsuredAmount);
        quotationModel.AmountAccesories = NotFormatMoney(quotationModel.AmountAccesories);
        quotationModel.Gender = quotationModel.radioGender;
        quotationModel.IndividualType = quotationModel.radioIndividualType;
        quotationModel.CustomerId = $('#hiddenCustomerId').val();
        quotationModel.ReplacementVehicle = $("#radioReplacementVehicle").is(':checked');
        quotationModel.RiskNum = gblRiskNum;
        if (quotationModel.ReplacementVehicle == undefined) {

            quotationModel.ReplacementVehicle = 0;
        }

        if (quotationModel.IndividualType == "1") {
            quotationModel.Address = $("#inputPersonAddress").val();
            quotationModel.Phone = $("#inputPersonPhone").val();
        }
        else if (quotationModel.IndividualType == "2") {
            quotationModel.Address = $("#inputCompanyAddress").val();
            quotationModel.Phone = $("#inputCompanyPhone").val();
        }

        $.ajax({
            type: 'POST',
            url: rootPath + 'Quotation/Quote',
            data: JSON.stringify({ quotationModel: quotationModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                LoadPremium(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Error al cotizar', 'autoclose': true });
        });
    }
}

function LoadPremium(quotation) {
    var vehicle = quotation;
    gblRiskNum = quotation.Risk.Number;
    $('#panelGetQuotation').show();
    $('#panelSummary').show();
    $('#hiddenId').val(quotation.Id);
    $('#hiddenTemporalId').val(quotation.Risk.Policy.Endorsement.TemporalId);
    $('#inputQuotationId').val(quotation.Risk.Policy.Endorsement.QuotationId);
    $('#hiddenQuotationVersion').val(quotation.Risk.Policy.Endorsement.QuotationVersion);
    $('#hiddenCustomerId').val(quotation.Risk.Policy.Holder.IndividualId);
    $('#hiddenCustomerType').val(quotation.Risk.Policy.Holder.CustomerType);
    $('#labelPremium').text(FormatMoney(quotation.Risk.Policy.Summary.Premium));
    $('#labelTax').text(FormatMoney(quotation.Risk.Policy.Summary.Taxes));
    $('#labelExpenses').text(FormatMoney(quotation.Risk.Policy.Summary.Expenses));
    $('#labelTotal').text(FormatMoney(quotation.Risk.Policy.Summary.FullPremium));

    $("#listCoverages").UifListView({ add: false, edit: false, delete: false, displayTemplate: "#coverageTemplate", title: "Cotización", height: 590 });

    $.each(vehicle.Risk.Coverages, function (index, val) {
        vehicle.Risk.Coverages[index].PremiumAmount = FormatMoney(vehicle.Risk.Coverages[index].PremiumAmount);
        vehicle.Risk.Coverages[index].CurrentFrom = FormatDate(vehicle.Risk.Coverages[index].CurrentFrom);
        vehicle.Risk.Coverages[index].CurrentTo = FormatDate(vehicle.Risk.Coverages[index].CurrentTo);
        if (vehicle.Risk.Coverages[index].Deductible != null) {
            vehicle.Risk.Coverages[index].DeductibleDescription = vehicle.Risk.Coverages[index].Deductible.Description
        }
        $("#listCoverages").UifListView("addItem", vehicle.Risk.Coverages[index]);
    });
    glbQuotation = quotation;
}

function ClearSummary() {
    $('#panelGetQuotation').hide();
    $('#panelSummary').hide();

    $('#labelPremium').text(0);
    $('#labelTax').text(0);
    $('#labelExpenses').text(0);
    $('#labelTotal').text(0);

    if ($('#selectProduct').val() > 0 && $('#selectGroupCoverage').val() > 0) {
        GetCoveragesByProductIdGroupCoverageId($('#selectProduct').val(), $('#selectGroupCoverage').val());
    }
    else {
        $("#listCoverages").UifListView("refresh");
    }
}

function SendToEmail(path) {
    if ($('#inputEmail').val() != '') {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Quotation/SendToEmail',
            data: JSON.stringify({ email: $('#inputEmail').val(), filePath: path }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success && data.result) {
                $.UifNotify('show', { 'type': 'info', 'message': "Cotización enviada", 'autoclose': true });
                UpdatePrintedDate();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': "Error al enviar cotización", 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': "Error al generar archivo", 'autoclose': true });
        });
    }
    else {
        $.UifNotify('show', { 'type': 'danger', 'message': "Falta email destino", 'autoclose': true });
        $('#inputEmail').css({
            'backgroundColor': '#f77259'
        });
    }
}

function GeneratePDF(quotation) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/GenerateReportQuotation',
        data: JSON.stringify({
            quotationId: quotation.Risk.Policy.Endorsement.QuotationId,
            temporaryId: quotation.Risk.Policy.Endorsement.TemporalId,
            prefixId: quotation.Risk.Policy.Prefix.Id,
            versionId: quotation.Risk.Policy.Endorsement.QuotationVersion
        }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        glbPath = data.result.Url;
        glbFileName = data.result.Filename;
        glbFilePath = data.result.FilePathResult;
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': 'Error al consultar PDF', 'autoclose': true });
    });
}

function DownloadPDF(urlFile, forceDownload, getFilename) {
    var link = document.createElement("a");
    document.body.appendChild(link);
    link.href = urlFile;
    if (forceDownload) {
        link.download = getFilename;
        link.target = "_blank";
    }
    if (urlFile != undefined && getFilename != undefined) {
        link.click();
    } else {
        $.UifNotify('show', { 'type': 'danger', 'message': 'Error al generar PDF', 'autoclose': true });
    }

}

function ValidateIntermediates() {
    var validated = true;
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/ValidateIntermediates',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (!data.success) {
            validated = false;
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        ClearVehicle();
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al consultar cotización', 'autoclose': true });
    });
    return validated;
}

function UpdatePrintedDate() {
    debugger;
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/UpdateCompanyPrintedDateByTempId',
        data: JSON.stringify({ "tempId": glbQuotation.Risk.Policy.Endorsement.TemporalId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (response) {
        console.log(response);
    });
}

 function FasecoldaCodeFocusIn() {
     $("#ReqFasecoldaCode").hide();
}

 function ValidationPlate() {
    var regex = /(^[a-zA-Z]{3}[0-9]{3}$)/;
    var regex2 = /(^[a-zA-Z]{3}[0-9]{2}[a-zA-Z]{1}$)/
    var regex3 = /(^[rRsS]{1}[0-9]{5}$)/
    var regex4 = /(^[a-zA-z]{2}[0-9]{4}$)/
    if (!regex.test($("#inputPlate").val()) && !regex2.test($("#inputPlate").val()) && !regex3.test($("#inputPlate").val()) && !regex4.test($("#inputPlate").val())) {
        $.UifNotify('show', { 'type': 'danger', 'message': "El formato de placa no es valido", 'autoclose': true });
    } else {
        return true;
    }
}



