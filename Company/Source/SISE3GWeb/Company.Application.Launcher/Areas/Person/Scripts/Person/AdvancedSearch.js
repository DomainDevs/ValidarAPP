var dropDownSearch;
var TypePersonSearch;

class AdvancedSearch extends Uif2.Page {
    
    getInitialState() {
        $.ajaxSetup({ async: true });
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'Person/Person/AdvancedSearch',
            element: '#btnSearchAdvPerson',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.AdvancedSearchEventsPerson
        });
        TypePersonSearch = TypePerson.PersonNatural;
    }



    AdvancedSearchEventsPerson() {
        $("#btnCancelSearchPerson").on("click", AdvancedSearch.CancelSearchPerson);
        $("#selectTypePerson").on("itemSelected", AdvancedSearch.TypePersonSelected);
        $("#btnsearchPersonAdv").click(AdvancedSearch.SearchPersonAdv);
        $("#btnsearchCompanyAdv").click(AdvancedSearch.SearchCompanyAdv);
        $("#btnLoadPerson").click(AdvancedSearch.LoadPerson);
        
        
        AdvancedSearch.LoadSelectTypePerson();
    }

    //Seccion Eventos
    bindEvents() {

    }

    static CancelSearchPerson() {
        dropDownSearch.hide();
    }

    static SearchAdvPerson(searchType) {
        AdvancedSearch.clearFieldAdv();
        $("#listviewSearchPerson").UifListView(
            {
                displayTemplate: "#searchNaturalTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
        dropDownSearch.show();
        $("#panelSearchPerson").show();
        $("#panelSearchCompany").hide();
        $("#selectTypePerson").UifSelect();
        $("#selectCompanyTypePnSearch").UifSelect();
        $("#selectDocumentTypePnSearch").UifSelect();

        PersonRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $("#selectTypePerson").UifSelect({ sourceData: data.result, selectedId: searchType });//TypePerson.PersonNatural });                             
            }
        });
    }

    static TypePersonSelected() {
        $("#listviewSearchPerson").UifListView( "clear");
        AdvancedSearch.clearFieldAdv();
        if ($("#selectTypePerson").UifSelect("getSelected") == TypePerson.PersonNatural || $("#selectTypePerson").UifSelect("getSelected") == TypePerson.ProspectNatural) {
            $("#panelSearchPerson").show();
            $("#panelSearchCompany").hide();
        }
        else if ($("#selectTypePerson").UifSelect("getSelected") == TypePerson.PersonLegal || $("#selectTypePerson").UifSelect("getSelected") == TypePerson.ProspectLegal) {
            $("#panelSearchPerson").hide();
            $("#panelSearchCompany").show();
        }
        else {
            $("#panelSearchPerson").hide();
            $("#panelSearchCompany").hide();
        }

        switch (parseInt( $("#selectTypePerson").UifSelect("getSelected"))) {
            case (TypePerson.PersonNatural):
                TypePersonSearch = TypePerson.PersonNatural;
                break;
            case (TypePerson.PersonLegal):
                TypePersonSearch = TypePerson.PersonLegal;
                break;
            case (TypePerson.ProspectNatural):
                TypePersonSearch = TypePerson.ProspectNatural;
                break;
            case (TypePerson.ProspectLegal):
                TypePersonSearch = TypePerson.ProspectLegal;
                break;
        };
        Persons.setTypePersonDefault(TypePersonSearch);
    }

    static SearchPersonAdv() {

        if ($.trim($("#inputSurnameAdv").val()) == "" &&
            $.trim($("#inputSecondsurnameAdv").val()) == "" &&
            $.trim($("#inputNamesAdv").val()) == "" &&
            $.trim($("#inputDocumentPersonAdv").val()) == "" &&
            $.trim($("#inputCodePersonAdv").val()) == "") {
            $.UifDialog('alert', { 'message': AppResourcesPerson.EnterSearchCriteria });
            return false;
        }
        if ($.trim($("#inputDocumentPersonAdv").val()) != "" && $("#selectDocumentTypePnSearch").UifSelect("getSelected") == "") {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorDocumentDocumentType });
            return false;
        }
        $("#listviewSearchPerson").UifListView("clear");
        //AdvancedSearch.GetPersonsByDocumentNumberNameSearchTypeAdv();
        
         AdvancedSearch.CreateModel();
      //  AdvancedSearch.GetAplicationPersonAdv(personData);


    }

    static CreateModel()
    {
        var personData = null;
        switch (TypePersonSearch) {
            case (TypePerson.PersonNatural):
            case (TypePerson.PersonLegal):
                personData= AdvancedSearch.CreatePersonModel();
                AdvancedSearch.GetAplicationPersonAdv(personData);
                break;
            
            case (TypePerson.ProspectNatural):
                personData = AdvancedSearch.CreateProspectNaturalModel();
                AdvancedSearch.GetAplicationProspectnNaturalAdv(personData);
                break;
            case (TypePerson.ProspectLegal):
                personData = AdvancedSearch.CreateProspectLegalModel();
                AdvancedSearch.GetAplicationCompanyAdv(personData);
                break;

            default:
                personData = AdvancedSearch.CreatePersonModel();
                AdvancedSearch.GetAplicationPersonAdv(personData);
                break;
        };
        
    }


    static SearchCompanyAdv() {
        if ($("#inputTradeNameAdv").val() == '' || $("#inputDocumentCompanyAdv").val() == "") {
            if ($("#inputTradeNameAdv").val().length < 3 && $("#inputDocumentCompanyAdv").val() == "" && $("#inputCodePersonCompanyAdv").val() == "") {
                $.UifDialog('alert', { 'message': AppResourcesPerson.Minimo3CharactersSearch });
            }
            if ($.trim($("#inputDocumentCompanyAdv").val()) != "" && $("#selectCompanyTypePnSearch").UifSelect("getSelected") == "") {
                $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorDocumentDocumentType });
            }
            else {
                
                //AdvancedSearch.GetCompaniesByDocumentNumberByNameBySearchTypeAdv();       
                if (TypePersonSearch == TypePerson.ProspectLegal) {
                    var companyData = AdvancedSearch.CreateProspectLegalModel();
                    AdvancedSearch.GetAplicationCompanyAdv(companyData);
                }
                else {
                    var companyData = AdvancedSearch.CreateCompanyModel();
                    AdvancedSearch.GetAplicationCompanyAdv(companyData);
                }
            }
        }
    }

    static LoadPerson() {

        var typePersonAdv = parseInt($("#selectTypePerson").UifSelect('getSelected'));
        var SelectedPersonAdv = $("#listviewSearchPerson").UifListView("getSelected");
        //dropDownSearch.hide();
        if (SelectedPersonAdv.length > 0) {
            switch (typePersonAdv) {
                case 1:
                    
                case 2:
                    glbPersonIndividualId = SelectedPersonAdv[0].Id;
                    break;
                case 3:
                    if (SelectedPersonAdv[0].ProspectCode == undefined) {
                        glbPersonIndividualId = SelectedPersonAdv[0].IdCardNo;
                    }
                    else {
                        glbPersonIndividualId = SelectedPersonAdv[0].Document;
                    }
                    break;
                case 4:
                    if (SelectedPersonAdv[0].Id == undefined) {
                        glbPersonIndividualId = SelectedPersonAdv[0].IdentificationDocument.Number;
                    }
                    else
                    {
                        glbPersonIndividualId = SelectedPersonAdv[0].Id;
                    }
                    
                    break;
                default:
                    break;
            }


            //if (searchType != typePersonAdv) 
             //   Persons.setTypePersonDefault(typePersonAdv);
            
            //else {
                switch (typePersonAdv) {
                    case 1:
                        Persons.GetPersonByIndividualId(glbPersonIndividualId)
                    case 2:
                        Persons.GetCompanyByIndividualId(SelectedPersonAdv[0].IndividualId );
                        break;
                    case 3:
                        Persons.GetProspectByDocumentNumSearchType(glbPersonIndividualId, typePersonAdv);
                        break;
                    case 4:
                        Persons.GetProspectByDocumentNumSearchType(glbPersonIndividualId, typePersonAdv);
                        break;
                    default:
                        break;
                }
            //}
        }
        dropDownSearch.hide();
    }

    static LoadSelectTypePerson() {
        PersonRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $("#selectTypePerson").UifSelect({ sourceData: data.result, selectedId: TypePerson.PersonNatural });
            }
        });
    }

    //Seccion Funciones
    static GetAplicationPersonAdv(PersonData) {
        PersonRequest.GetAplicationPersonAdv(PersonData).done(function (data) {
            if (data.success) {
                if (data.result != null  && data.result.length > 0) {
                    AdvancedSearch.LoadPersonaAdv(data.result, "#searchNaturalTemplate");
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
                    AdvancedSearch.clearFieldAdv();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });

            }
            
        });
    }

    static LoadPersonaAdv(persons) {
        $.each(persons, function (index, item) {
            if (item.ProspectCode != null || item.ProspectCode != "") //Solo aplica para prospectos
            {                
                item.ProspectCode = item.PersonCode;
                //$("#listviewSearchPerson").UifListView("addItem", item);
            }
            $("#listviewSearchPerson").UifListView("addItem", item);
        });
    }

    static clearFieldAdv() {
        $("#inputSurnameAdv").val("");
        $("#inputCodePersonAdv").val("");
        $("#inputSecondsurnameAdv").val("");
        $("#inputNamesAdv").val("");
        $("#inputDocumentPersonAdv").val("");
        $("#inputTradeNameAdv").val("");
        $("#inputDocumentCompanyAdv").val("");
        $("#inputDocumentCompanyAdv").val("");
        $("#inputCodePersonAdv").val("");
        $("#inputCodePersonCompanyAdv").val("");

    }

    static GetAplicationProspectnNaturalAdv(companyData) {
        PersonRequest.GetAplicationProspectnNaturalAdv(companyData).done(function (data) {
            if (data.success) {
                if (data.result.length > 0 && data.result[0].ProspectCode != null && data.result[0].ProspectCode >0) {
                    var dta = AdvancedSearch.ConvertCompanyDtoToModel(data);
                    //AdvancedSearch.LoadPersonaAdv(data.result, "#searchLegalTemplate");
                    $("#listviewSearchPerson").UifListView({
                        displayTemplate: "#searchNaturalTemplate",
                        selectionType: 'single',
                        source: null,
                        height: 180
                    });
                    AdvancedSearch.LoadPersonaAdv(dta);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
                    AdvancedSearch.clearFieldAdv();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });

            }
        });
    }


    static GetAplicationCompanyAdv(companyData) {
        if (TypePersonSearch == TypePerson.ProspectLegal) {
            PersonRequest.GetAplicationProspectLegalAdv(companyData).done(function (data) {
                if (data.success) {
                    if (data.result != null && data.result.length > 0 && data.result[0].ProspectCode != null && data.result[0].ProspectCode > 0) {
                        var dta = ProspectusLegal.ConvertCompanyDtoToModel(data);
                        //AdvancedSearch.LoadPersonaAdv(data.result, "#searchLegalTemplate");
                        $("#listviewSearchPerson").UifListView({
                            displayTemplate: "#searchLegalTemplate",
                            selectionType: 'single',
                            source: null,
                            height: 180
                        });
                        AdvancedSearch.LoadPersonaAdv(dta);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
                        AdvancedSearch.clearFieldAdv();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
                    
                }
            });
        }
        else
        PersonRequest.GetAplicationCompanyAdv(companyData).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    var dta = PersonLegal.ConvertCompanyDtoToModel(data);
                    //AdvancedSearch.LoadPersonaAdv(data.result, "#searchLegalTemplate");
                    $("#listviewSearchPerson").UifListView({
                        displayTemplate: "#searchLegalTemplate",
                        selectionType: 'single',
                        source: null,
                        height: 180
                    });
                    AdvancedSearch.LoadPersonaAdv(dta);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
                    AdvancedSearch.clearFieldAdv();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel });
                
            }
        });
    }

    static CreateCompanyModel() {
        var companyData = {
            BusinessName: $('#inputTradeNameAdv').val(),
            DocumentTypeId: $('#selectCompanyTypePnSearch').UifSelect("getSelected"),
            Document: $('#inputDocumentCompanyAdv').val(),
            Id: $('#inputCodePersonCompanyAdv').val()
        };
        return companyData;
    }

    static CreateProspectLegalModel() {
        var companyData = {
            TradeName: $('#inputTradeNameAdv').val(),
            TributaryIdTypeCode: $('#selectCompanyTypePnSearch').UifSelect("getSelected"),
            TributaryIdNumber: $('#inputDocumentCompanyAdv').val(),
            ProspectCode: $('#inputCodePersonCompanyAdv').val()
            
        };
        return companyData;
    }

    static CreateProspectNaturalModel() {
        var companyData = {
            Name: $('#inputNamesAdv').val(),
            IdCardTypeCode: $('#selectDocumentTypePnSearch').UifSelect("getSelected"),
            IdCardNo: $('#inputDocumentPersonAdv').val(),
            ProspectCode: $('#inputCodePersonAdv').val()

        };
        return companyData;
    }

    static CreatePersonModel() {
        var personData = {
            typePersonAdv: parseInt($("#selectTypePerson").UifSelect('getSelected')),
            Surname: $("#inputSurnameAdv").val(),
            SecondSurname: $("#inputSecondsurnameAdv").val(),
            Names: $("#inputNamesAdv").val(),
            Document: $("#inputDocumentPersonAdv").val(),
            inputCodePersonAdv: $("#inputCodePersonAdv").val(),
            Id: $("#inputCodePersonAdv").val(),
            DocumentTypeId: $("#selectDocumentTypePnSearch").val()
        };
        return personData;
    }

    static ConvertCompanyDtoToModel(company) {
        var rstl = [];
        var resultData = {};
        if (company.result.length > 0) {
            $.each(company.result, function (index, item) {

                resultData.Names = item.Name + " " + item.SurName;
                resultData.IdCardNo = item.Card.Description;
                resultData.Document = item.ProspectCode;
                resultData.DocumentTypeId = item.Card.Id;
                //resultData.CountryOrigin.Id = company.result[index].Country;
                //resultData.CountryOrigin.Description = company.result[index].Addresses[0].CountryDescription;


                //resultData.Addresses = company.result[index].Address;
                //resultData.Phones = company.result[index].PhoneNumber;
                //resultData.Emails = company.result[index].EmailAddres;

                rstl.push(resultData);
            });
        }
        return rstl;
    }

}
