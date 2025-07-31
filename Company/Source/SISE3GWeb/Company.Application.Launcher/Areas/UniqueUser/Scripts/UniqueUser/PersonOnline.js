
class PersonOnline extends Uif2.Page {
    getInitialState() {
     }
     bindEvents() {
     }
    RedirectToPersonsByIndividualIdIndividualTypeCustomerType(individualId, individualType, customerType, individualSearchType) {
        glbPersonOnline.CustomerType = customerType;
        glbPersonOnline.IndividualId = individualId;
        glbPersonOnline.IndividualSearchType = individualSearchType;
        glbPersonOnline.RolType = individualType;
        if (individualType == TypePerson.PersonNatural) {
            router.run("prtPersonNatural");
        }
        else {
            router.run("prtPersonLegal");
        }
    }

     RedirectToPersonsByDescription (description) {
        $('#modalPersonOnline').UifModal('hide');

        var documentNumber = parseInt(description, 10);
        if (isNaN(documentNumber)) {
            glbPersonOnline.Name = description;
            glbPersonOnline.DocumentNumber = "";
        }
        else {
            glbPersonOnline.Name = "";
            glbPersonOnline.DocumentNumber = description;
        }

        switch ($('input:radio[name=searchType]:checked').val()) {
            case "naturalPerson":
                glbPersonOnline.RolType = 1;
                router.run("prtPersonNatural")
                break;
        }
    }


     ShowOnlinePerson () {
        $.UifDialog('confirm', { 'message': Resources.Language.MessagePersonCreate }, function (result) {
            if (result) {               
                $('#modalPersonOnline').UifModal('showLocal', Resources.Language.LabelOnlinePerson);
            }
        });
    }

}