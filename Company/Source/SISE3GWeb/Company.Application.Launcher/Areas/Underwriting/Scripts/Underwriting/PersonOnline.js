class UnderwritingPersonOnline {
    static RedirectToPersonsByIndividualIdIndividualTypeCustomerType(individualId, individualType, customerType, individualSearchType) {
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

    static RedirectToPersonsByDescription(description) {
        $('#modalPersonOnline').UifModal('hide');

        var isNumeric = new RegExp("^[0-9]*$").test(description);
        if (!isNumeric) {
            glbPersonOnline.Name = description;
            glbPersonOnline.DocumentNumber = "";
        }
        else {
            glbPersonOnline.Name = "";
            glbPersonOnline.DocumentNumber = description;

            if (description.length > 14) {
                let msg = AppResources.ErrorLength.replace(/[{0}]{3}/i, AppResources.holderIdentificationDocument).replace(/[{2}]{3}/i, "1").replace(/[{1}]{3}/i, "14");

                $.UifNotify("show", { 'type': "danger", 'message': msg, 'autoclose': true });
                return;
            }
        }

        switch ($('input:radio[name=searchType]:checked').val()) {
            case "naturalPerson":
                glbPersonOnline.RolType = 1;
                router.run("prtPersonNatural");
                break;
            case "legalPerson":
                glbPersonOnline.RolType = 2;
                router.run("prtPersonLegal");
                break;
            case "naturalprospectus":
                glbPersonOnline.RolType = 3;
                router.run("prtProspectusNatural");
                break;
            case "legalprospectus":
                glbPersonOnline.RolType = 4;
                router.run("prtProspectusLegal");
                break;
        }
    }

    static ShowOnlinePerson() {
        $.UifDialog('confirm', { 'message': AppResources.MessagePersonCreate }, function (result) {
            if (result) {
                if (glbPolicy != null && glbPolicy.TemporalType == TemporalType.Quotation) {
                    $('#divlegalprospectus').show();
                    $('#divnaturalprospectus').show();
                }
                $('#modalPersonOnline').UifModal('showLocal', AppResources.LabelOnlinePerson);
            }
        });
    }
}