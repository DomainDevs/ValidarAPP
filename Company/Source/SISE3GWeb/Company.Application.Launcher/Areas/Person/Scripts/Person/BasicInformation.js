
$.ajaxSetup({ async: true });
var IndividualId = null;
var TypePersonSelected = null;
var TypePersonSearch = null;
class PersonBasicInformation extends Uif2.Page {
    bindEvents() {
        $('#SaveBasicInformation').click(PersonBasicInformation.SaveBasicInformation);
        $('#CloseBasicInformation').click(PersonBasicInformation.redirectIndex);
        $('#inputDocument').on('buttonClick', PersonBasicInformation.SearchDocument);
        $('#selectSearchPersonType').on('itemSelected', PersonBasicInformation.ChangeTypePerson);
    }
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        PersonBasicInformation.LoadPersonType();
        PersonBasicInformation.ChangeStatusControlsPerson(true);
        PersonBasicInformation.ChangeStatusControlsCompany(true);
        $("#BasicInformationPerson").show();
        $("#BasicInformationCompany").hide();
    }
    static ChangeTypePerson() {
        PersonBasicInformation.ClearPersonBasic();
        PersonBasicInformation.ChangeStatusControlsPerson(true);
        PersonBasicInformation.ChangeStatusControlsCompany(true);
        if ($("#selectSearchPersonType").UifSelect("getSelected") == TypePerson.PersonNatural) {
            $("#BasicInformationPerson").show();
            $("#BasicInformationCompany").hide();
        }
        else if ($("#selectSearchPersonType").UifSelect("getSelected") == TypePerson.PersonLegal) {
            $("#BasicInformationCompany").show();
            $("#BasicInformationPerson").hide();
        }
    }
    static SearchDocument() {
        if ($("#selectSearchPersonType").UifSelect("getSelected") == "" || $("#selectSearchPersonType").UifSelect("getSelected") == null) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResourcesPerson.SelectTypePerson
            })
        }
        else {

            if ($('#inputDocument').val().trim() != "") {
                if ($('#inputDocument').val().trim().length >= 3) {
                    PersonBasicInformation.ChangeStatusControlsPerson(true);
                    PersonBasicInformation.ChangeStatusControlsCompany(true);
                    PersonBasicInformation.ClearPersonBasic();
                    PersonBasicInformation.ClearCompanyBasic();
                    searchType = parseInt($("#selectSearchPersonType").UifSelect('getSelected'), 10);
                    if (searchType == TypePerson.PersonNatural) {
                        TypePersonSearch = TypePerson.PersonNatural;
                        PersonBasicInformation.GetPersonBasicByDocumentNumber($('#inputDocument').val().trim());
                    }
                    if (searchType == TypePerson.PersonLegal) {
                        TypePersonSearch = TypePerson.PersonLegal;
                        PersonBasicInformation.GetCompanyBasicByDocumentNumber($('#inputDocument').val().trim());
                    }
                } else {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.Minimo3CharactersSearch
                    })
                }
            } else {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResourcesPerson.ErrorRequiredDocument
                })
            }
        }
    }
    static LoadControlsPerson() {
        PersonBasicInformation.LoadDocumentType();
        PersonBasicInformation.LoadGender();
        PersonBasicInformation.LoadMaritalStatus();
    }
    static ChangeStatusControlsPerson(status) {
        if (status) {
            $('#DocumentType').attr("disabled", status);
            $('#PersonCode').attr("disabled", status);
            $('#Gender').attr("disabled", status);
            $('#MaritalStatus').attr("disabled", status);
            $('#Birthdate').attr("disabled", status);
            $('#Age').attr("disabled", status);
            $('#BirthPlace').attr("disabled", status);
            $('#LastUpdate').attr("disabled", status);
            $('#UpdateBy').attr("disabled", status);
        }
        $('#DocumentNumber').attr("disabled", status);
        $('#FirstName').attr("disabled", status);
        $('#LastName').attr("disabled", status);
        $('#Name').attr("disabled", status);
    }
    static ChangeStatusControlsCompany(status) {
        if (status) {
            $('#DocumentTypeCompany').attr("disabled", status);
            $('#CompanyDigit').attr("disabled", status);
            $('#CompanyCode').attr("disabled", status);
            $('#TypePartnership').attr("disabled", status);
            $('#CompanyTypePartnership').attr("disabled", status);
            $('#Country').attr("disabled", status);
            $('#LastUpdateCompany').attr("disabled", status);
            $('#UpdateByCompany').attr("disabled", status);
        }
        $('#DocumentNumberCompany').attr("disabled", status);
        $('#TradeName').attr("disabled", status);
    }
    static SaveBasicInformation() {
        if (IndividualId != null) {
            if (TypePersonSelected == TypePerson.PersonNatural) {
                $("#formPersonInfoBasic").validate();
                if ($("#formPersonInfoBasic").valid()) {
                    var formPersonInfoBasic = $("#formPersonInfoBasic").serializeObject();
                    formPersonInfoBasic.IndividualId = IndividualId;
                    BasicInformationRequest.SaveBasicPerson(formPersonInfoBasic).done(function (data) {
                        if (data.success) {
                            PersonBasicInformation.ClearPersonBasic();
                            $('#inputDocument').val('');
                            PersonBasicInformation.LoadPersonType();
                            $.UifNotify('show', {
                                'type': 'info', 'message': AppResourcesPerson.SuccessUpdatePersonBasic
                            })
                        } else {
                            $.UifNotify('show', {
                                'type': 'info', 'message': data.result
                            })
                        }
                    });
                }
            } else {
                $("#formCompanyInfoBasic").validate();
                if ($("#formCompanyInfoBasic").valid()) {
                    var formCompanyInfoBasic = $("#formCompanyInfoBasic").serializeObject();
                    formCompanyInfoBasic.IndividualId = IndividualId;
                    formCompanyInfoBasic.DocumentType = formCompanyInfoBasic.DocumentTypeCompany;
                    formCompanyInfoBasic.DocumentNumber = formCompanyInfoBasic.DocumentNumberCompany;
                    BasicInformationRequest.SaveBasicCompany(formCompanyInfoBasic).done(function (data) {
                        if (data.success) {
                            PersonBasicInformation.ClearCompanyBasic();
                            $('#inputDocument').val('');
                            PersonBasicInformation.LoadPersonType();
                            $.UifNotify('show', {
                                'type': 'info', 'message': AppResourcesPerson.SuccessUpdateCompanyBasic
                            })
                        } else {
                            $.UifNotify('show', {
                                'type': 'info', 'message': data.result
                            })
                        }
                    });
                }
            }
        } else {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResourcesPerson.ErrorSelectedCompanyPersonBasic
            })
        }
    }
    static GetPersonBasicByDocumentNumber(documentNumber) {
        BasicInformationRequest.GetPersonBasicByDocumentNumber(documentNumber).done(function (data) {
            if (data.success) {
                if (data.result.BasicPersonServiceModel.length == 1) {
                    if (PersonBasicInformation.ValidateIndividualPolicy(data.result.BasicPersonServiceModel[0])) {
                        PersonBasicInformation.LoadPersonBasic(data.result.BasicPersonServiceModel[0]);
                    } else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': AppResourcesPerson.ErrorPersonPoliceUpdate
                        })
                    }
                } else {
                    PersonBasicInformation.LoadPersonsBasic(data.result.BasicPersonServiceModel.slice(0, 50));
                }
            } else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result
                })
                PersonBasicInformation.ClearPersonBasic();
            }

        });
    }
    static GetCompanyBasicByDocumentNumber(documentNumber) {
        BasicInformationRequest.GetCompanyBasicByDocumentNumber(documentNumber).done(function (data) {
            if (data.success) {
                if (data.result.BasicCompanyServiceModel.length == 1) {
                    if (PersonBasicInformation.ValidateIndividualPolicy(data.result.BasicCompanyServiceModel[0])) {
                        PersonBasicInformation.LoadCompanyBasic(data.result.BasicCompanyServiceModel[0]);
                    } else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': AppResourcesPerson.ErrorCompanyPoliceUpdate
                        })
                    }
                } else {
                    PersonBasicInformation.LoadPersonsBasic(data.result.BasicCompanyServiceModel.slice(0, 50));
                }
            } else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result
                })
                PersonBasicInformation.ClearPersonBasic();
            }
        });
    }

    static ValidateIndividualPolicy(individual) {
        if (individual.Beneficiary || individual.Insured || individual.Policy) {
            return false;
        } else {
            return true;
        }
    }
    static ClearPersonBasic() {
        $('#DocumentNumber').val('');
        $('#PersonCode').val('');
        $('#FirstName').val('');
        $('#LastName').val('');
        $('#Name').val('');
        $('#Birthdate').val('');
        $('#Age').val('');
        $('#BirthPlace').val('');
        $('#UpdateBy').val('');
        $("#DocumentType").UifSelect("setSelected", null);
        $("#Gender").UifSelect("setSelected", null);
        $("#MaritalStatus").UifSelect("setSelected", null);
        ClearValidation('#formPersonInfoBasic');
        IndividualId = null;
        TypePersonSelected = null;
    }
    static ClearCompanyBasic() {
        $('#DocumentNumberCompany').val('');
        $('#CompanyDigit').val('');
        $('#CompanyCode').val('');
        $('#TradeName').val('');
        $('#LastUpdateCompany').val('');
        $('#UpdateByCompany').val('');
        $("#DocumentTypeCompany").UifSelect("setSelected", null);
        $("#TypePartnership").UifSelect("setSelected", null);
        $("#CompanyTypePartnership").UifSelect("setSelected", null);
        $("#Country").UifSelect("setSelected", null);
        ClearValidation('#formCompanyInfoBasic');
        IndividualId = null;
        TypePersonSelected = null;
    }
    static LoadPersonsBasic(persons) {
        PersonBasicInformationAdvancedSearch.ShowAdvancedSearch();
        PersonBasicInformationAdvancedSearch.LoadListSearch(persons);
    }
    static LoadCompanysBasic(persons) {
        PersonBasicInformationAdvancedSearch.ShowAdvancedSearch();
        PersonBasicInformationAdvancedSearch.LoadListSearch(persons);
    }
    static LoadPersonBasic(person) {
        IndividualId = person.IndividualId;
        TypePersonSelected = TypePerson.PersonNatural;
        PersonBasicInformation.ChangeStatusControlsPerson(false);
        PersonBasicInformation.LoadPersonDocumentType(person.DocumentType);
        PersonBasicInformation.LoadMaritalStatus(person.MaritalStatus);
        PersonBasicInformation.LoadGender(person.Gender);
        $('#DocumentNumber').val(person.DocumentNumber);
        $('#PersonCode').val(person.PersonCode);
        $('#FirstName').val(person.FirstName);
        $('#LastName').val(person.LastName);
        $('#Name').val(person.Name);
        $('#Birthdate').val(FormatDate(person.Birthdate, 1));
        var date = new Date();
        var BirthDate = PersonBasicInformation.toDate(FormatDate(person.Birthdate, 1));
        $('#Age').val(date.getFullYear() - BirthDate.getFullYear());
        $('#BirthPlace').val(person.BirthPlace);
        $('#LastUpdate').val(FormatDate(person.LastUpdate, 1));
        $('#UpdateBy').val(person.UpdateBy);
    }
    static toDate(dateStr) {
        var parts = dateStr.split("/")
        return new Date(parts[2], parts[1] - 1, parts[0])
    }
    static LoadCompanyBasic(company) {
        IndividualId = company.IndividualId;
        TypePersonSelected = TypePerson.PersonLegal;
        PersonBasicInformation.ChangeStatusControlsCompany(false);
        PersonBasicInformation.LoadCompanyDocumentType(company.DocumentType);
        PersonBasicInformation.LoadTypePartnership(company.TypePartnership);
        PersonBasicInformation.LoadCompanyTypePartnership(company.CompanyTypePartnership);
        PersonBasicInformation.LoadCountry(company.Country);
        $('#CompanyDigit').val(Shared.CalculateDigitVerify(company.DocumentNumber));
        $('#DocumentNumberCompany').val(company.DocumentNumber);
        $('#CompanyCode').val(company.CompanyCode);
        $('#TradeName').val(company.TradeName);
        $('#LastUpdateCompany').val(FormatDate(company.LastUpdate, 1));
        $('#UpdateByCompany').val(company.UpdateBy);

    }
    static LoadMaritalStatus(selected) {
        BasicInformationRequest.GetMaritalStatus().done(function (data) {
            if (data.success) {
                $("#MaritalStatus").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#MaritalStatus").UifSelect("setSelected", selected);
                }
                $('#MaritalStatus').attr("disabled", true);
            }
        });
    }
    static LoadGender(selected) {
        BasicInformationRequest.GetGenderTypes().done(function (data) {
            if (data.success) {
                $("#Gender").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    if (prospect.Gender == 'M') {
                        $("#Gender").UifSelect("setSelected", GenderType.Male);
                    } else {
                        $("#Gender").UifSelect("setSelected", GenderType.Female);
                    }
                }
                $('#Gender').attr("disabled", true);
            }
        });
    }
    static LoadPersonDocumentTypeSearch(selected) {
        BasicInformationRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#DocumentTypeSearch").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#DocumentTypeSearch").UifSelect("setSelected", selected);
                }
            }
        });
    }
    static LoadPersonDocumentType(selected) {
        BasicInformationRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#DocumentType").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#DocumentType").UifSelect("setSelected", selected);
                }
                $('#DocumentType').attr("disabled", true);
            }
        });
    }
    static LoadCompanyDocumentTypeSearch(selected) {
        BasicInformationRequest.GetDocumentType("2").done(function (data) {
            if (data.success) {
                $("#DocumentTypeSearch").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#DocumentTypeSearch").UifSelect("setSelected", selected);
                }
            }
        });
    }
    static LoadCompanyDocumentType(selected) {
        BasicInformationRequest.GetDocumentType("2").done(function (data) {
            if (data.success) {
                $("#DocumentTypeCompany").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#DocumentTypeCompany").UifSelect("setSelected", selected);
                }
                $('#DocumentTypeCompany').attr("disabled", true);
            }
        });
    }

    static LoadTypePartnership(selected) {
        BasicInformationRequest.GetAssociationTypes().done(function (data) {
            if (data.success) {
                $("#TypePartnership").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#TypePartnership").UifSelect("setSelected", selected);
                }
                $('#TypePartnership').attr("disabled", true);
            }
        });
    }
    static LoadCompanyTypePartnership(selected) {
        BasicInformationRequest.GetCompanyTypes().done(function (data) {
            if (data.success) {
                $("#CompanyTypePartnership").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#CompanyTypePartnership").UifSelect("setSelected", selected);
                }
                $('#CompanyTypePartnership').attr("disabled", true);
            }
        });
    }
    static LoadCountry(selected) {
        BasicInformationRequest.GetCountries().done(function (data) {
            if (data.success) {
                $("#Country").UifSelect({ sourceData: data.result });
                if (selected != null) {
                    $("#Country").UifSelect("setSelected", selected);
                }
                $('#Country').attr("disabled", true);
            }
        });
    }

    static LoadPersonType() {
        BasicInformationRequest.GetPersonTypes().done(function (data) {
            $("#selectSearchPersonType").UifSelect({ sourceData: data.data });
        });
    }
    static redirectIndex() {
        window.location = rootPath + "Home/Index";
    }
}
class BasicInformationRequest {
    static SaveBasicPerson(personData) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/SaveBasicPerson",
            data: JSON.stringify({ "personViewModel": personData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveBasicCompany(companyData) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/SaveBasicCompany",
            data: JSON.stringify({ "companyViewModel": companyData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPersonTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetIndividualTypes",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }
    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetDocumentType",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGenderTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetGenderTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetMaritalStatus() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetMaritalStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(codePerson, firstName, lastName, name, documentNumber, typeDocument) {

        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber",
            data: {
                codePerson: codePerson, firstName: firstName, lastName: lastName, name: name, documentNumber: documentNumber, typeDocument: typeDocument
            },
            dataType: "json"
        });
    }

    static GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(codeCompany, tradeName, documentNumber, typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber",
            data: {
                codeCompany: codeCompany, tradeName: tradeName, documentNumber: documentNumber, typeDocument: typeDocument
            },
            dataType: "json"
        });
    }


    static GetPersonBasicByDocumentNumber(documentNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPersonBasicByDocumentNumber",
            data: {
                documentNumber: documentNumber
            },
            dataType: "json"
        });
    }

    static GetCompanyBasicByDocumentNumber(documentNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCompanyBasicByDocumentNumber",
            data: {
                documentNumber: documentNumber
            },
            dataType: "json"
        });
    }
    static GetCountries() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetCountries",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCompanyTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCompanyTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAssociationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAssociationTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}