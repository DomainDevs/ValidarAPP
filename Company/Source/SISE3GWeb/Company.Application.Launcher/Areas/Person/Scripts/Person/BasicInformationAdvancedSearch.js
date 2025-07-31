var dropDownSearchAdv = null;
var typesDocumentPerson = null;
var typesDocumentCompany = null;
$.ajaxSetup({ async: true });
class PersonBasicInformationAdvancedSearch extends Uif2.Page {
    getInitialState() {
        BasicInformationRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                typesDocumentPerson = data.result;
            }
        });
        BasicInformationRequest.GetDocumentType("2").done(function (data) {
            if (data.success) {
                typesDocumentCompany = data.result;
            }
        });
    }
    bindEvents() {
        $('#ShowSearchAdvBasicInformation').click(PersonBasicInformationAdvancedSearch.ShowAdvancedSearch);

        dropDownSearchAdv = uif2.dropDown({
            source: rootPath + 'Person/Person/BasicInformationAdvancedSearch',
            element: '#ShowSearchAdvBasicInformation',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: PersonBasicInformationAdvancedSearch.AdvancedSearchEvents
        });
    }
    static LoadListSearch(data) {
        if (data != null) {
            $("#lvSearchAdvBasicInformation").UifListView("clear");
            $.each(data, function (index, item) {
                item.Role = PersonBasicInformationAdvancedSearch.GetRolesIndividual(item);
                item.NameTypeDocument = PersonBasicInformationAdvancedSearch.GetNameTypeDocument(item);
                $("#lvSearchAdvBasicInformation").UifListView("addItem", item);
            });
        } else {
            $("#lvSearchAdvBasicInformation").UifListView({ source: null, displayTemplate: "#AdvSearchBasicInfoTemplate", selectionType: 'single', height: 140 });
        }

    }
    static GetNameTypeDocument(individual) {
        if (TypePersonSearch == TypePerson.PersonNatural) {
            if (typesDocumentPerson != null) {
                return typesDocumentPerson.filter(x => x.Id == individual.DocumentType)[0].SmallDescription;
            }
        } else if (TypePersonSearch == TypePerson.PersonLegal) {
            if (typesDocumentCompany != null) {
                return typesDocumentCompany.filter(x => x.Id == individual.DocumentType)[0].SmallDescription;
            }
        }
        return '';
    }
    static GetRolesIndividual(individual) {
        var roles = '';
        if (individual.Beneficiary) {
            roles += AppResourcesPerson.LabelBeneficiary;
        }
        if (individual.Policy) {
            roles += (roles.length > 0 ? '/' : '') + AppResourcesPerson.LabelHolder;
        }
        if (individual.Insured) {
            roles += (roles.length > 0 ? '/' : '') + AppResourcesPerson.LabelInsured;
        }
        return roles;
    }
    static InitialAdvancedSearch() {
        PersonBasicInformationAdvancedSearch.LoadListSearch(null);
    }
    static ShowAdvancedSearch() {
        dropDownSearchAdv.show();
        PersonBasicInformationAdvancedSearch.ClearAdvancedSearch();
        PersonBasicInformationAdvancedSearch.LoadListSearch(null);
    }
    static CloseAdvancedSearch() {
        dropDownSearchAdv.hide();
    }
    static SelecteAdvancedSearchd() {
        var selectedtype = TypePersonSearch;
        var select = $("#lvSearchAdvBasicInformation").UifListView("getSelected");
        if (select != "" && select.length > 0) {
            if (TypePersonSearch == TypePerson.PersonNatural) {
                if (PersonBasicInformation.ValidateIndividualPolicy(select[0])) {
                    PersonBasicInformation.ClearPersonBasic();
                    $("#BasicInformationPerson").show();
                    $("#BasicInformationCompany").hide();
                    PersonBasicInformation.LoadPersonBasic(select[0]);
                    PersonBasicInformationAdvancedSearch.CloseAdvancedSearch();
                } else {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.ErrorPersonPoliceUpdate
                    })
                }
            } else if (TypePersonSearch == TypePerson.PersonLegal) {
                if (PersonBasicInformation.ValidateIndividualPolicy(select[0])) {
                    PersonBasicInformation.ClearCompanyBasic();
                    $("#BasicInformationPerson").hide();
                    $("#BasicInformationCompany").show();
                    PersonBasicInformation.LoadCompanyBasic(select[0]);
                    PersonBasicInformationAdvancedSearch.CloseAdvancedSearch();
                } else {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.ErrorCompanyPoliceUpdate
                    })
                }
            }
            PersonBasicInformation.LoadPersonType();
            $('#inputDocument').val('');
        } else {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResourcesPerson.ErrorSelectedElementList
            })
        }
    }
    static AdvancedSearchEvents() {
        PersonBasicInformationAdvancedSearch.LoadPersonType(null);
        PersonBasicInformationAdvancedSearch.InitialAdvancedSearch();
        PersonBasicInformation.LoadPersonDocumentTypeSearch();
        $('#CancelSearchAdvBasicInformation').click(PersonBasicInformationAdvancedSearch.CloseAdvancedSearch);
        $('#SelectedSearchAdvBasicInformation').click(PersonBasicInformationAdvancedSearch.SelecteAdvancedSearchd);
        $('#SearchAdvancedBasicInformation').click(PersonBasicInformationAdvancedSearch.SearchAdvancedBasicInformation);
        $('#PersonTypeSearch').on('itemSelected', PersonBasicInformationAdvancedSearch.ChangeTypePerson);
        $("#InformationPersonAdvSearch").show();
        $("#InformationCompanyAdvSearch").hide();
    }
    static SearchAdvancedBasicInformation() {
        $("#formAdvSearchBasicInformation").validate();
        if ($("#formAdvSearchBasicInformation").valid()) {
            var formAdvSearchBasicInformation = $("#formAdvSearchBasicInformation").serializeObject();
            var validations = true;
            if (formAdvSearchBasicInformation.FirstNameSearch != "") {
                if (formAdvSearchBasicInformation.FirstNameSearch.length < 3) {
                    validations = false;
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.ErrorSurnameMiniumCharacters
                    })
                }
            }
            if (formAdvSearchBasicInformation.LastNameSearch != "") {
                if (formAdvSearchBasicInformation.LastNameSearch.length < 3) {
                    validations = false;
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.ErrorMotherNameMiniumCharacters
                    })
                }
            }
            if (formAdvSearchBasicInformation.NameSearch != "") {
                if (formAdvSearchBasicInformation.NameSearch.length < 3) {
                    validations = false;
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.ErrorNameMiniumCharacters
                    })
                }
            }
            if (formAdvSearchBasicInformation.TradeName != "") {
                if (formAdvSearchBasicInformation.TradeName.length < 3) {
                    validations = false;
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.ErrorTradeNameMiniumCharacters
                    })
                }
            }
            if (validations) {
                TypePersonSearch = $("#PersonTypeSearch").UifSelect("getSelected");
                if ($("#PersonTypeSearch").UifSelect("getSelected") == TypePerson.PersonNatural) {
                    PersonBasicInformationAdvancedSearch.GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByDocumentNumber(
                        formAdvSearchBasicInformation.PersonCodeSearch,
                        formAdvSearchBasicInformation.FirstNameSearch,
                        formAdvSearchBasicInformation.LastNameSearch,
                        formAdvSearchBasicInformation.NameSearch,
                        formAdvSearchBasicInformation.DocumentNumberSearch,
                        formAdvSearchBasicInformation.DocumentTypeSearch
                    );
                } else if ($("#PersonTypeSearch").UifSelect("getSelected") == TypePerson.PersonLegal) {
                    PersonBasicInformationAdvancedSearch.GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(
                        formAdvSearchBasicInformation.PersonCodeSearch,
                        formAdvSearchBasicInformation.TradeName,
                        formAdvSearchBasicInformation.DocumentNumberSearch,
                        formAdvSearchBasicInformation.DocumentTypeSearch
                    );;
                }

            }
        }
    }
    static ChangeTypePerson() {
        var PersonTypeSearch = $("#PersonTypeSearch").UifSelect("getSelected");
        PersonBasicInformationAdvancedSearch.LoadListSearch(null);
        if (PersonTypeSearch == TypePerson.PersonNatural) {
            PersonBasicInformation.LoadPersonDocumentTypeSearch();
            $("#InformationPersonAdvSearch").show();
            $("#InformationCompanyAdvSearch").hide();
        }
        else if (PersonTypeSearch == TypePerson.PersonLegal) {
            PersonBasicInformation.LoadCompanyDocumentTypeSearch();
            $("#InformationCompanyAdvSearch").show();
            $("#InformationPersonAdvSearch").hide();
        }
        PersonBasicInformationAdvancedSearch.ClearAdvancedSearch();
        $("#PersonTypeSearch").UifSelect("setSelected", PersonTypeSearch);
    }
    static ClearAdvancedSearch() {
        $('#DocumentNumberSearch').val('');
        $('#PersonCodeSearch').val('');
        $('#FirstNameSearch').val('');
        $('#LastNameSearch').val('');
        $('#NameSearch').val('');
        $('#TradeName').val('');
        $("#PersonTypeSearch").UifSelect("setSelected", null);
        $("#DocumentTypeSearch").UifSelect("setSelected", null);
        ClearValidation('#formAdvSearchBasicInformation');
    }
    static LoadPersonType(item) {
        BasicInformationRequest.GetPersonTypes().done(function (data) {
            $("#PersonTypeSearch").UifSelect({ sourceData: data.data });
            if (item != null) {
                $("#PersonTypeSearch").UifSelect("setSelected", item);
            }
        });
    }

    static GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByDocumentNumber(codePerson, firstName, lastName, name, documentNumber, typeDocument) {
        BasicInformationRequest.GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(codePerson, firstName, lastName, name, documentNumber, typeDocument).done(function (data) {
            if (data.success) {
                PersonBasicInformationAdvancedSearch.LoadListSearch(data.result.BasicPersonServiceModel);
            } else {
                PersonBasicInformationAdvancedSearch.LoadListSearch(null);
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result
                })
                PersonBasicInformation.ClearPersonBasic();
            }

        });
    }
    static GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(codeCompany, tradeName, documentNumber, typeDocument) {
        BasicInformationRequest.GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(codeCompany, tradeName, documentNumber, typeDocument).done(function (data) {
            if (data.success) {
                PersonBasicInformationAdvancedSearch.LoadListSearch(data.result.BasicCompanyServiceModel);
            } else {
                PersonBasicInformationAdvancedSearch.LoadListSearch(null);
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result
                })
                PersonBasicInformation.ClearCompanyBasic();
            }
        });
    }
}
