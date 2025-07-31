var updateUser = null;
var updateCompany = null;
var parameters = null;
var nitAssociationType = null;

class EditBasicInfo extends Uif2.Page {
    getInitialState() {
        PersonRequest.LoadInitialData(false).done(function (data) {
            if (data.success) {
                $("#selectGender").UifSelect({ sourceData: data.result.GenderTypes });
                $("#selectCivilStatus").UifSelect({ sourceData: data.result.MaritalStatus });
                $("#selectGender").prop("disabled", true);
                $("#selectCivilStatus").prop("disabled", true);
            }
        });

        DocumentTypeRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#selectIdTypePerson").UifSelect({ sourceData: data.result });
                $("#selectIdTypePerson").prop("disabled", true);
            }
        });
        PersonRequest.LoadInitialLegalData(2).done(function (data) {
            if (data.success) {
                $("#selectIdTypeCompany").UifSelect({ sourceData: data.result.DocumentTypes });
                $("#selectAsociationType").UifSelect({ sourceData: data.result.AssociationTypes });
                $("#selectCompanyType").UifSelect({ sourceData: data.result.CompanyTypes });
                $("#selectIdTypeCompany").prop("disabled", true);
                $("#selectAsociationType").prop("disabled", true);
                $("#selectCompanyType").prop("disabled", true);
            }
        });

        if (parameters == null) {
            ParameterRequest.GetParameters().done(function (data) {
                if (data.success) {
                    parameters = data.result;
                }
            });
        }
        $("#inputIdNumberDigit").prop('disabled', true);
    }

    //Seccion Eventos
    bindEvents() {
        $('#inputIdNumber').on("search", this.SearchPerson);
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividualResults);
        $('#btnUpdatePerson').on('click', this.UpdatePerson);
        $('#btnCancelView').on('click', this.RedirectIndex);
        $("#inputIdNumberCompany").focusout(this.SearchCompanyDocument);
    }

    SearchPerson() {
        if ($("#inputIdNumber").val() == "" || $("#inputIdNumber").val() == null) {
            $.UifNotify('show', {
                'type': 'info', 'message': 'ERROR'
            })
        } else {
            var dataList = [];
            BasicInfoRequest.GetPersonOrCompanyByDescription($("#inputIdNumber").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual, 1).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0 && individualSearchType == 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                    }
                    else if (data.result.length == 1) {
                        EditBasicInfo.GetPersonByIndividualId(data.result[0].IndividualId, data.result[0].IndividualType);
                    }
                    else if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].InsuredId,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].IndividualType,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description,
                                DocumentTypeCode: data.result[i].IdentificationDocument.DocumentType.Id
                            });
                        }

                        EditBasicInfo.ShowIndividualResults(dataList);
                        $('#modalIndividualSearch').UifModal('showLocal', AppResourcesPerson.SelectPerson);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.NotFoundPerson, 'autoclose': true });
                    EditBasicInfo.clearFields();
                }
            });
        }
    }

    SelectIndividualResults() {
        var individualId = $(this).children()[0].innerHTML;
        var individualType = parseInt($(this).children()[5].innerHTML);
        EditBasicInfo.GetPersonByIndividualId(individualId, individualType);
    }

    SearchCompanyDocument() {
        if ($.trim($("#inputIdNumberCompany").val()) != "") {
            PersonRequest.GetAplicationCompanyByDocument(TypePerson.PersonLegal, $("#inputIdNumberCompany").val()).done(function (data) {
                if (data.success) {
                    if (data.result.length >= 1) {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                        $("#inputIdNumberDigit").val("0");
                        $("#inputIdNumberCompany").val("");
                    }
                }
                $("#inputIdNumberDigit").val(Shared.CalculateDigitVerify($('#inputIdNumberCompany').val()))
            });
        }
    }

    UpdatePerson() {
        if (updateUser != null) {
            updateUser.Names = $('#inputName').val();
            updateUser.Surname = $('#inputLastName1').val();
            updateUser.SecondSurname = $('#inputLastName2').val();
            BasicInfoRequest.UpdatePersonBasicInfo(updateUser).done(function (data) {
                if (data.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies,
                                data.result.OperationId,
                                FunctionType.PersonBasicInfo);
                        }
                    } else {
                        $("#inputIdNumber").val(data.result.SurName.trim() + " " + data.result.SecondSurName.trim() + " " + data.result.Name.trim());
                        $.UifDialog('alert', { 'message': AppResourcesPerson.LabelPersonCode + data.result.IndividualId + " " + AppResourcesPerson.LabelPersonUpdate });
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            updateCompany.BusinessName = $("#inputSocialReason").val();
            updateCompany.Document = $("#inputIdNumberCompany").val();
            updateCompany.NitAssociationType = $("#inputIdNumberCompany").val() == nitAssociationType ? nitAssociationType : "0";
            updateCompany.VerifyDigit = $("#inputIdNumberDigit").val();
            BasicInfoRequest.UpdateCompanyBasicInfo(updateCompany).done(function (data) {
                if (data.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies,
                                data.result.OperationId,
                                FunctionType.PersonBasicInfo);
                        }
                    } else {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.LabelPersonCode + data.result.IndividualId + " " + AppResourcesPerson.LabelPersonUpdate });
                        EditBasicInfo.GetPersonByIndividualId(data.result.IndividualId, data.result.IndividualType);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }

    }

    RedirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    static ShowIndividualResults(dataList) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataList);
        unlockScreen();
    }

    static GetPersonByIndividualId(individualId, individualType) {
        if (individualType === 1) {

            PersonRequest.GetPersonByIndividualId(individualId).done(function (resp) {
                if (resp.success) {
                    if (resp.result.AuthorizationRequests !== undefined) {
                        $.UifNotify('show', { 'type': 'info', 'message': 'La persona tiene politicas pendientes por autorizar', 'autoclose': true });
                    } else {
                        updateCompany = null;
                        EditBasicInfo.LoadPerson(resp.result);
                        updateUser = resp.result;
                    }

                }
            });

        }
        else if (individualType === 2) {
            PersonRequest.GetCompanyByIndividualId(individualId).done(function (resp) {
                if (resp.success) {
                    if (resp.result.AuthorizationRequests !== undefined) {
                        $.UifNotify('show', { 'type': 'info', 'message': 'La persona tiene politicas pendientes por autorizar', 'autoclose': true });
                    } else {
                        updateUser = null;
                        EditBasicInfo.LoadPerson(resp.result);
                        updateCompany = resp.result;
                    }
                } else {
                    showErrorToast(resp.result);
                }

            });
        }

        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    static LoadPerson(person) {
        nitAssociationType = null;
        if (person.BusinessName == undefined) {
            $("#companySection").hide();
            $("#inputLastName1").val(person.Surname);
            $("#inputLastName2").val(person.SecondSurname);
            $("#inputName").val(person.Names);
            if (person.Gender == 'M') { $('#selectGender').UifSelect("setSelected", GenderType.Male); }
            else if (person.Gender == 'F') { $('#selectGender').UifSelect("setSelected", GenderType.Female); }
            $("#inputBirthDate").val(FormatDate(person.BirthDate, 1));
            $("#selectCivilStatus").val(person.MaritalStatusId);
            $("#selectIdTypePerson").val(person.DocumentTypeId);
            $("#inputIdNumberPerson").val(person.Document);
            $("#inputNationality").val();
            $("#inputEconomicActivity").val(person.EconomicActivityDescription);
            $("#inputIdNumber").val(person.Surname.trim() + " " + person.SecondSurname + " " + person.Names.trim());
            LabourPersonRequest.GetLabourPersonByIndividualId(person.Id).done(function (resp) {
                if (resp.success && resp.result.BirthCountryId > 0) {
                    CountryRequest.GetCountriesById(resp.result.BirthCountryId).done(function (data) {
                        if (data.success) {
                            $("#inputNationality").val(data.result.Description);
                        } else {
                            $("#inputNationality").val();
                        }

                    });
                } else {
                    $("#inputNationality").val('NO REGISTRADO');
                }
            });

            $("#personSection").show();
        } else {
            $("#personSection").hide();
            $("#inputSocialReason").val(person.BusinessName);
            $("#selectCompanyType").val(person.CompanyTypeId);
            $("#selectAsociationType").val(person.AssociationTypeId);
            $("#inputEconomicActivityCompany").val(person.EconomicActivityDescription);
            $("#selectIdTypeCompany").val(person.DocumentTypeId);
            if (person.NitAssociationType > 0 && person.AssociationTypeId != "1" || person.VerifyDigit == 0 && person.AssociationTypeId != "1" ) {
                nitAssociationType = person.NitAssociationType;
                $("#inputIdNumberCompany").prop('disabled', false);
            } else {
                $("#inputIdNumberCompany").prop('disabled', true);
            }
            $("#inputIdNumberCompany").val(person.Document);
            $("#inputIdNumberDigit").val(Shared.CalculateDigitVerify($('#inputIdNumberCompany').val()));
            $("#companySection").show();
            $("#inputIdNumber").val(person.BusinessName);
            
            if (person.CountryOriginId > 0) {
                CountryRequest.GetCountriesById(person.CountryOriginId).done(function (resp) {
                    if (resp.success) {
                        $("#inputOriginCountry").val(resp.result.Description);
                    } else {
                        $("#inputOriginCountry").val();
                    }

                });
            } else {
                $("#inputOriginCountry").val('NO REGISTRADO');
            }
        }
    }

    static clearFields() {
        $("#inputLastName1").val('');
        $("#inputLastName2").val('');
        $("#inputName").val('');
        $('#selectGender').UifSelect("setSelected", null);
        $("#inputBirthDate").val('');
        $("#selectCivilStatus").val('');
        $("#selectIdTypePerson").val('');
        $("#inputIdNumberPerson").val('');
        $("#inputNationality").val('');
        $("#inputEconomicActivity").val('');
        $("#inputIdNumber").val('');
        $("#inputNationality").val('');
        $("#inputSocialReason").val('');
        $("#selectCompanyType").val('');
        $("#selectAsociationType").val('');
        $("#inputEconomicActivityCompany").val('');
        $("#selectIdTypeCompany").val('');
        $("#inputIdNumberCompany").val('');
        $("#inputIdNumberDigit").val('');
        $("#inputIdNumber").val('');
        $("#inputOriginCountry").val('');
    }
}
