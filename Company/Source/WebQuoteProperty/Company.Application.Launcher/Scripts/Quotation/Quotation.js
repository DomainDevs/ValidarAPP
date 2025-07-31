var countryDefaultId = 1;
var coverages = null;

$(() => {
    new QuotationView();
});

class Quotation {
    static GetCurrentDate() {
        return $.ajax({
            type: 'POST',
            url: 'GetCurrentDate',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetCountries() {
        return $.ajax({
            type: 'POST',
            url: 'GetCountries',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: 'POST',
            url: 'GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: 'POST',
            url: 'GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetDocumentTypesByIndividualType(individualType) {
        return $.ajax({
            type: 'POST',
            url: 'GetDocumentTypesByIndividualType',
            data: JSON.stringify({ individualType: individualType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetProducts() {
        return $.ajax({
            type: 'POST',
            url: 'GetProducts',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetProspectByIndividualTypeDocumentTypeIdDocumentNumber(individualType, documentTypeId, documentNumber) {
        return $.ajax({
            type: 'POST',
            url: 'GetProspectByIndividualTypeDocumentTypeIdDocumentNumber',
            data: JSON.stringify({ individualType: individualType, documentTypeId: documentTypeId, documentNumber: documentNumber }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCoverageGroupsByProductId(productId) {
        return $.ajax({
            type: 'POST',
            url: 'GetCoverageGroupsByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetInsuredObjectsByProductIdCoverageGroupId(productId, coverageGroupId) {
        return $.ajax({
            type: 'POST',
            url: 'GetInsuredObjectsByProductIdCoverageGroupId',
            data: JSON.stringify({ productId: productId, coverageGroupId: coverageGroupId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SetInsuredValue(productId, coverageGroupId, insuredObjectId, insuredValue, insuredObjects) {
        return $.ajax({
            type: 'POST',
            url: 'SetInsuredValue',
            data: JSON.stringify({ productId: productId, coverageGroupId: coverageGroupId, insuredObjectId: insuredObjectId, insuredValue: insuredValue, insuredObjects: insuredObjects }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static Quotate(quotationViewModel, companyInsuredObjects) {
        return $.ajax({
            type: 'POST',
            url: 'Quotate',
            data: JSON.stringify({ quotationViewModel: quotationViewModel, companyInsuredObjects: companyInsuredObjects }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetNormalizedAddress(address) {
        return $.ajax({
            type: 'POST',
            url: 'GetNormalizedAddress',
            data: JSON.stringify({ address: address }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateReport(temporalId) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateReport',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetQuotationByQuotationId(quotationId) {
        return $.ajax({
            type: 'POST',
            url: 'GetQuotationByQuotationId',
            data: JSON.stringify({ quotationId: quotationId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetQuotationByTemporalId(temporalId) {
        return $.ajax({
            type: 'POST',
            url: 'GetQuotationByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SendToEmail(temporalId) {
        return $.ajax({
            type: 'POST',
            url: 'SendToEmail',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class QuotationView extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputConstructionYear').ValidatorKey(ValidatorType.Number, 0, 0);
        $('#inputDocumentNumber').ValidatorKey(ValidatorType.Number, 0, 0);
        $('#inputNames').ValidatorKey(ValidatorType.Letter, 0, 0);
        $('#inputSurname').ValidatorKey(ValidatorType.Letter, 0, 0);
        $('#inputSecondSurname').ValidatorKey(ValidatorType.Letter, 0, 0);
        $('#inputTradeName').ValidatorKey(ValidatorType.Letter, 0, 0);
        $('#inputCompanyPhone').ValidatorKey(ValidatorType.Number, 0, 0);
        $('#inputPersonPhone').ValidatorKey(ValidatorType.Number, 0, 0);
        $('#inputInsuredValue').OnlyDecimals(2);
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $('#listInsuredObjects').UifListView({ add: false, edit: false, delete: true, customDelete: true, displayTemplate: "#templateInsuredObject", height: 500 });

        Quotation.GetCurrentDate().done(function (data) {
            if (data.success) {
                $('#inputIssueDate').text(FormatDate(data.result));
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        Quotation.GetCountries().done(function (data) {
            if (data.success) {
                $('#selectCountry').UifSelect({ sourceData: data.result, selectedId: countryDefaultId });
                $('#selectCountry').prop('disabled', 'disabled');

                Quotation.GetStatesByCountryId($('#selectCountry').UifSelect('getSelected')).done(function (data) {
                    if (data.success) {
                        $('#selectState').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        this.ShowPanelCustomer();

        Quotation.GetProducts().done(function (data) {
            if (data.success) {
                $('#selectProduct').UifSelect({ sourceData: data.result });             
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {
        $('#selectState').on('itemSelected', this.SearchCity);
        $('#radioIndividualTypePerson').change(this.ShowPanelCustomer);
        $('#radioIndividualTypeCompany').change(this.ShowPanelCustomer);
        $('#inputDocumentNumber').on('buttonClick', this.SearchCustomer);
        $('#selectProduct').on('itemSelected', this.SearchCoverageGroup);
        $('#selectCoverageGroup').on('itemSelected', this.SearchInsuredObject);
        $('#inputFullAddress').focusout(this.NormalizeAddress);
        $('#selectInsuredObject').on('itemSelected', this.GetInsuredValue);
        $('#inputInsuredValue').focusin(this.NotFormatInsuredValue);
        $('#inputInsuredValue').focusout(QuotationView.SetInsuredValue);
        $('#inputQuotationId').on('buttonClick', QuotationView.SearchQuotation);
       $('#inputQuotationId').on('keydown', this.SearchQuotationEnter);
        $('#btnQuote').click(this.Quote);
        $('#btnDownload').click(this.DownloadFile);
        $('#btnSendToEmail').click(this.SendEmail);
        $('#btnNewQuotation').click(this.ClearForm);
        $('#tableResults tbody').on('click', 'tr', this.SearchQuotationVersion);
        $('#listInsuredObjects').on('rowDelete', function (event, data) {
            $('#inputInsuredValue').val(0);
            QuotationView.SetInsuredValue();
        });
    }

    SearchCity() {
        Quotation.GetCitiesByCountryIdStateId($('#selectCountry').UifSelect('getSelected'), $('#selectState').UifSelect('getSelected')).done(function (data) {
            if (data.success) {
                $('#selectCity').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    ShowPanelCustomer() {
        QuotationView.ClearPanelCustomer();

        Quotation.GetDocumentTypesByIndividualType($('input[name=radioIndividualType]:checked').val()).done(function (data) {
            if (data.success) {
                $('#selectDocumentType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        if ($('#radioIndividualTypePerson').prop('checked')) {
            $('.panelPerson').show();
            $('.panelCompany').hide();
            $('#inputVerifyDigit').hide();
        }
        else if ($('#radioIndividualTypeCompany').prop('checked')) {
            $('.panelPerson').hide();
            $('.panelCompany').show();
            $('#inputVerifyDigit').show();
        }
    }

    static ClearPanelCustomer() {
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

    SearchCustomer() {
        if ($('#selectDocumentType').val() > 0 && $('#inputDocumentNumber').val() > 0) {
            Quotation.GetProspectByIndividualTypeDocumentTypeIdDocumentNumber($('input[name=radioIndividualType]:checked').val(), $('#selectDocumentType').UifSelect('getSelected'), $('#inputDocumentNumber').val()).done(function (data) {
                if (data.success) {
                    QuotationView.LoadCustomer(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static LoadCustomer(customer) {
        $('#hiddenCustomerId').val(customer.Id);
        $('#hiddenCustomerTypeId').val(customer.CustomerType);

        Quotation.GetDocumentTypesByIndividualType(customer.IndividualType).done(function (data) {
            if (data.success) {
                $('#selectDocumentType').UifSelect({ sourceData: data.result, selectedId: customer.IdentificationDocument.DocumentType.Id });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $('#inputDocumentNumber').val(customer.IdentificationDocument.Number);

        if (customer.IndividualType == 1) {
            $('.panelPerson').show();
            $('.panelCompany').hide();
            $('#radioIndividualTypePerson').prop('checked', true);
            $('#inputNames').val(customer.Name);
            $('#inputSurname').val(customer.Surname);
            $('#inputSecondSurname').val(customer.SecondSurname);
            $('#inputBirthDate').UifDatepicker('setValue', FormatDate(customer.BirthDate));

            if (customer.CompanyName.Address != null) {
                $('#inputPersonAddress').val(customer.CompanyName.Address.Description);
            }

            if (customer.CompanyName.Phone != null) {
                $('#inputPersonPhone').val(customer.CompanyName.Phone.Description);
            }

            if (customer.CompanyName.Email != null) {
                $('#inputPersonEmail').val(customer.CompanyName.Email.Description);
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
            $('#inputVerifyDigit').val(QuotationView.CalculateDigitVerify($('#inputDocumentNumber').val()));

            if (customer.CompanyName.Address != null) {
                $('#inputCompanyAddress').val(customer.CompanyName.Address.Description);
            }

            if (customer.CompanyName.Phone != null) {
                $('#inputCompanyPhone').val(customer.CompanyName.Phone.Description);
            }

            if (customer.CompanyName.Email != null) {
                $('#inputCompanyEmail').val(customer.CompanyName.Email.Description);
            }
        }
    }
    
    static CalculateDigitVerify(documentNumber) {
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

    SearchCoverageGroup() {
        Quotation.GetCoverageGroupsByProductId($('#selectProduct').UifSelect('getSelected')).done(function (data) {
            if (data.success) {
                $('#selectCoverageGroup').UifSelect({ sourceData: data.result });
                $('#selectInsuredObject').UifSelect();
                $('#inputInsuredValue').val('');
                $('#listInsuredObjects').UifListView('refresh');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    SearchInsuredObject() {
        Quotation.GetInsuredObjectsByProductIdCoverageGroupId($('#selectProduct').UifSelect('getSelected'), $('#selectCoverageGroup').UifSelect('getSelected')).done(function (data) {
            if (data.success) {
                $('#selectInsuredObject').UifSelect({ sourceData: data.result });
                $('#inputInsuredValue').val('');
                $('#listInsuredObjects').UifListView('refresh');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    NormalizeAddress() {
        if ($('#inputFullAddress').val().trim().length > 0) {
            Quotation.GetNormalizedAddress($('#inputFullAddress').val()).done(function (data) {
                if (data.success) {
                    $('#inputFullAddress').val(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    GetInsuredValue() {
        var insuredObjectId = parseInt($('#selectInsuredObject').UifSelect('getSelected'));
        var insuredObjects = $('#listInsuredObjects').UifListView('getData');
        var insuredValue = '';

        $.each(insuredObjects, function (index, item) {
            if (item.Id === insuredObjectId) {
                insuredValue = item.Amount;
            }
        });

        $('#inputInsuredValue').val(insuredValue);
    }

    NotFormatInsuredValue() {
        $('#inputInsuredValue').val(NotFormatMoney($('#inputInsuredValue').val()));
    }

    static SetInsuredValue() {
        if ($('#inputInsuredValue').val().length > 0) {
            var insuredObjects = $('#listInsuredObjects').UifListView('getData')
            $.each(insuredObjects, function (index, item) {
                item.Amount = NotFormatMoney(item.Amount);
                item.Premium = NotFormatMoney(item.Premium);
            });

            Quotation.SetInsuredValue($('#selectProduct').UifSelect('getSelected'), $('#selectCoverageGroup').UifSelect('getSelected'), $('#selectInsuredObject').UifSelect('getSelected'), $('#inputInsuredValue').val(), insuredObjects).done(function (data) {
                if (data.success) {
                    $('#inputInsuredValue').val(FormatMoney($('#inputInsuredValue').val()));
                    $('#listInsuredObjects').UifListView('refresh');

                    $.each(data.result, function (index, item) {
                        item.Amount = FormatMoney(item.Amount);
                        item.Premium = FormatMoney(item.Premium);
                        $('#listInsuredObjects').UifListView('addItem', item);
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    Quote() {
        $('#formQuotation').validate();
        if ($('#formQuotation').valid()) {
            if (CompareDates($('#inputBirthDate').val(), $('#inputIssueDate').text()) != 0) {
                $.UifNotify('show', { 'type': 'info', 'message': 'La fecha no debe ser mayor a al actual' , 'autoclose': true });
            }
            else {
                if (CompareYear($('#inputConstructionYear').val(), $('#inputIssueDate').text()) != 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': 'La fecha no debe ser mayor a al actual', 'autoclose': true });
                }
                else {
                    var quotationData = $('#formQuotation').serializeObject();
                    quotationData.QuotationId = $('#inputQuotationId').val();
                    quotationData.IndividualTypeId = $('input[name=radioIndividualType]:checked').val();
                    quotationData.Gender = $('input[name=radioGender]:checked').val();

                    var insuredObjects = $('#listInsuredObjects').UifListView('getData');
                    $.each(insuredObjects, function (index, item) {
                        item.Amount = NotFormatMoney(item.Amount);
                        item.Premium = NotFormatMoney(item.Premium);
                    });

                    Quotation.Quotate(quotationData, insuredObjects).done(function (data) {
                        if (data.success) {
                            QuotationView.LoadPremium(data.result);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }               
            }
           
        }
    }

    static LoadPremium(property) {
        $('#hiddenTemporalId').val(property.Policy.Id);
        $('#inputQuotationId').val(property.Policy.Endorsement.QuotationId);
        $('#hiddenQuotationVersion').val(property.Policy.Endorsement.QuotationVersion);
        $('#labelPremium').text(FormatMoney(property.Policy.Summary.Premium));
        $('#labelTax').text(FormatMoney(property.Policy.Summary.Taxes));
        $('#labelExpenses').text(FormatMoney(property.Policy.Summary.Expenses));
        $('#labelTotal').text(FormatMoney(property.Policy.Summary.FullPremium));

        $('#listInsuredObjects').UifListView('refresh');
        var insuredObjectIds = [];

        $.each(property.Coverages, function (index, item) {
            if (insuredObjectIds.indexOf(item.InsuredObject.Id) == -1) {
                item.InsuredObject.Amount = FormatMoney(item.InsuredObject.Amount);
                item.InsuredObject.Premium = FormatMoney(item.InsuredObject.Premium);
                $('#listInsuredObjects').UifListView('addItem', item.InsuredObject);

                insuredObjectIds.push(item.InsuredObject.Id);
            }
        });

        coverages = property.Coverages;
    }
    
    static ShowCoverages(insuredObjectId) {
        if (coverages != null) {
            var coverageDescriptions = [];

            $.each(coverages, function (index, item) {
                if (item.InsuredObject.Id == insuredObjectId) {
                    var deductibleDescription = '';

                    if (item.Deductible != null) {
                        deductibleDescription = item.Deductible.Description;
                    }

                    coverageDescriptions.push({
                        Coverage: item.Description,
                        Deductible: deductibleDescription
                    });
                }
            });

            if (coverageDescriptions.length > 0) {
                $('#tableCoverages').UifDataTable('clear');
                $('#tableCoverages').UifDataTable('addRow', coverageDescriptions);

                $('#modalCoverages').UifModal('showLocal', 'Resumen Coberturas' + " - " + $('#selectProduct').UifSelect('getSelectedText'));
            }
        }
    }

    SearchQuotationEnter() {
        if (event.which == 13) {
            QuotationView.SearchQuotation();
        }
    }
    
    static SearchQuotation() {
        if ($('#inputQuotationId').val().trim().length > 0) {
            Quotation.GetQuotationByQuotationId($('#inputQuotationId').val(), 0).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {                       
                        Quotation.GetQuotationByTemporalId(data.result[0].Id).done(function (data) {
                            if (data.success) {
                                QuotationView.LoadQuotation(data.result);
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });

                    }
                    else {
                        var versions = [];

                        for (var i = 0; i < data.result.length; i++) {
                            versions.push({
                                Temporal: data.result[i].Id,
                                Version: data.result[i].Endorsement.QuotationVersion,
                                Branch: data.result[i].Branch.Description
                            });
                        }

                        $('#tableResults').UifDataTable('clear');
                        $('#tableResults').UifDataTable('addRow', versions);
                        
                        $('#modalVersions').UifModal('showLocal', 'Seleccione una Versión');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchQuotationVersion() {
        Quotation.GetQuotationByTemporalId($(this).children()[0].innerHTML).done(function (data) {
            if (data.success) {
                QuotationView.LoadQuotation(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $('#modalVersions').UifModal('hide');
    }

    static LoadQuotation(property) {
        $('#inputFullAddress').val(property.FullAddress);
        $('#inputConstructionYear').val(property.ConstructionYear);
        $('#selectState').UifSelect('setSelected', property.City.State.Id);

        Quotation.GetCitiesByCountryIdStateId($('#selectCountry').UifSelect('getSelected'), $('#selectState').UifSelect('getSelected')).done(function (data) {
            if (data.success) {
                $('#selectCity').UifSelect({ sourceData: data.result, selectedId: property.City.Id });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $('#selectProduct').UifSelect('setSelected', property.Policy.Product.Id);

        Quotation.GetCoverageGroupsByProductId($('#selectProduct').UifSelect('getSelected')).done(function (data) {
            if (data.success) {
                $('#selectCoverageGroup').UifSelect({ sourceData: data.result, selectedId: property.GroupCoverage.Id });

                Quotation.GetInsuredObjectsByProductIdCoverageGroupId($('#selectProduct').UifSelect('getSelected'), $('#selectCoverageGroup').UifSelect('getSelected')).done(function (data) {
                    if (data.success) {
                        $('#selectInsuredObject').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        QuotationView.LoadCustomer(property.MainInsured);
        $('#hiddenCustomerId').val(property.MainInsured.IndividualId);
        $('#inputTradeName').val(property.MainInsured.Name);
        QuotationView.LoadPremium(property);
    }

    DownloadFile() {
        if ($('#hiddenTemporalId').val() > 0) {
            Quotation.GenerateReport($('#hiddenTemporalId').val()).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SendEmail() {
        if ($('#hiddenTemporalId').val() > 0) {
            Quotation.SendToEmail($('#hiddenTemporalId').val()).done(function (data) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            });
        }
    }

    ClearForm() {
        $('#formQuotation').formReset();
        $('#inputQuotationId').val('');
        $('#hiddenTemporalId').val('');
        $('#hiddenQuotationVersion').val('');
        $('#hiddenCustomerId').val('');
        $('#hiddenCustomerTypeId').val('');
        $('#selectCity').UifSelect();
        $('#selectCoverageGroup').UifSelect();
        $('#selectInsuredObject').UifSelect();
        $('#listInsuredObjects').UifListView('refresh');
        QuotationView.ClearPremium();
    }

    static ClearPremium() {
        $('#labelPremium').text(0);
        $('#labelTax').text(0);
        $('#labelExpenses').text(0);
        $('#labelTotal').text(0);
    }
}
