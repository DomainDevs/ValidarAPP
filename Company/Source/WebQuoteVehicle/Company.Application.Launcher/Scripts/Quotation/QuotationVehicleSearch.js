//Codigo de la pagina QuotationSearch/Index.cshtml
var searchingCustomer = false;

$(document).ready(function () {
    $.uif2.helpers.setGlobalTitle($('#hiddenTitle').val());
    $('#inputDocument').ValidatorKey(ValidatorType.Number, 1, 1);
    $('#inputTradeName').ValidatorKey(ValidatorType.lettersandnumbersSpecial, 1, 1);
    $('#inputNames').ValidatorKey(ValidatorType.Letter, 1, 1);
    $('#inputSurname').ValidatorKey(ValidatorType.Letter, 1, 1);
    $('#inputSecondSurname').ValidatorKey(ValidatorType.Letter, 1, 1);
    $.fn.UifDataTable.exporttoexcel = true;

    $('input[type=text]').keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });
    $("#inputNames").attr("disabled", "disabled");
    $("#inputSurname").attr("disabled", "disabled");
    $("#inputSecondSurname").attr("disabled", "disabled");
    $('.panelPerson').show();
    $('.panelCompany').hide();
    $('#panelGetQuotation').hide();
    $('#panelSummary').hide();
    GetDocumentTypesByIndividualType(1, 0);
    $('#inputIssueDate').text(GetCurrentFromDate());

    if ($('#radioIndividualTypePerson').prop('checked')) {
        $('#inputVerifyDigit').hide();
    } else {
        $('#inputVerifyDigit').show();
    }
    $('.panel-actions button').css({ 'display': 'none' })

});

//Eventos

$('#btnExport').on('click', function (e) {
    e.preventDefault()
    xportToExcel();
});

$('#radioIndividualTypePerson').change(function () {
    if ($('#radioIndividualTypePerson').prop('checked')) {
        $('.panelPerson').show();
        $('.panelCompany').hide();
        ClearCustomerSearch();
        GetDocumentTypesByIndividualType(1, 0);
        $('#inputVerifyDigit').val('');
        $('#inputVerifyDigit').hide();
    }
});

$('#radioIndividualTypeCompany').change(function () {
    if ($('#radioIndividualTypeCompany').prop('checked')) {
        $('.panelPerson').hide();
        $('.panelCompany').show();
        ClearCustomerSearch();
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
    }
});

$('#btnSearchCustomer').click(function () {
    $("#formQuotationSearch").validate();
    if ($("#formQuotationSearch").valid()) {
        SearchCustomer();
    }
});

$('#btnNewQuotationSearch').on('click', function () {
    ClearQuotationSearch();
});

//Metodos

function ClearQuotationSearch() {
    $('#formQuotationSearch').formReset();
    $('#radioIndividualTypePerson').prop('checked', true)
    $('.panelPerson').show();
    $('.panelCompany').hide();
    ClearCustomerSearch();
    GetDocumentTypesByIndividualType(1, 0);
}

function GetQuotationByQuotationId(quotationId) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Quotation/GetQuotationByQuotationId',
        data: JSON.stringify({ quotationId: quotationId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
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
    var vehicle = quotation;
    $('#inputDocument').val(vehicle.Risk.MainInsured.IdentificationDocument.Number);
    $('#inputVerifyDigit').val(CalculateDigitVerify($('#inputDocument').val()));
    LoadCustomer(vehicle.Risk.MainInsured);
    $('#selectProduct').UifSelect('setSelected', vehicle.Risk.Policy.Product.Id);
    LoadPremium(quotation);
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
                    SearchQuotation(data.result);
                }
                else {
                    ClearCustomerSearch();
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

function SearchQuotation(dataquo) {
    if ($('#selectDocumentType').val() > 0 && $('#inputDocument').val() > 0) {
        searchingCustomer = true;
        $.ajax({
            type: 'POST',
            url: rootPath + 'Quotation/GetCompanyQuotationVehicleSearch',
            data: JSON.stringify({ tempId: dataquo.Id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            LoadSearchQuotation(data,dataquo);
            searchingCustomer = false;
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Error al consultar cliente', 'autoclose': true });
            searchingCustomer = false;
        });
    }
}


function LoadSearchQuotation(dataQuotation, dataquo) {
    $.each(dataQuotation.data, function (index, value) {
        this.DateQuotation = FormatDate(this.DateQuotation);
        this.DocumentNumber = dataquo.IdentificationDocument.Number;
        this.Name = dataquo.Name;
        this.Surname = dataquo.Surname;
        this.SecondSurname = dataquo.SecondSurname;
    });
    if (dataQuotation.data != null) {
        $("#tblSearchQuotation").UifDataTable({ sourceData: dataQuotation.data })
    } else {
        var name = (dataquo.TradeName != null) ? dataquo.TradeName : dataquo.Name + " " + dataquo.Surname;
        $.UifNotify('show', { 'type': 'danger', 'message': 'El usuario ' + name + ' no ha realizado cotizaciones previamente', 'autoclose': true });
    }
    
    $('.panel-actions button').css({ 'display': 'none' })
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

function ClearCustomerSearch() {
    $('#hiddenCustomerId').val('');
    $('#hiddenCustomerType').val('');
    $('#inputNames').val('');
    $('#inputSurname').val('');
    $('#inputSecondSurname').val('');
    $("#tblSearchQuotation").UifDataTable('clear')
}

function xportToExcel() {
    var rowCount = $("#tblSearchQuotation").UifDataTable('getData');
    if (rowCount.length > 0) {
        $('.panel-actions button').click();
    } else {
        $.UifNotify('show', { 'type': 'info', 'message': "Debe indicar un numero de documento   ", 'autoclose': true });
    }

}