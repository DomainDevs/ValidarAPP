var positionTaxEdit = null;
class AditionalDataPn extends Uif2.Page {
    getInitialState() {
    }

    bindEvents() {
        $("#btnAdditionalData").click(this.btnAdditionalData);
        $('#modalAdditionalData').on('opened.modal', this.modalTaxOpen);
    }

    btnAdditionalData() {
        if (individualId != Person.New) {
            Persons.ShowPanelsPerson(RolType.AdditionalData);
        }
        else {
            var valid = "";
            if (searchType == TypePerson.PersonNatural) {
                if (PersonNatural.ValidatePerson()) {
                    if (Persons.validateSarlaftPerson()) {
                        PersonNatural.RecordPerson(false);
                        Persons.ShowPanelsPerson(RolType.AdditionalData);
                    }
                }

            }
            else if (searchType == TypePerson.PersonLegal) {
                valid = PersonLegal.ValidateCompany();
                if (valid == "") {
                    PersonLegal.RecordCompany(false);
                    Persons.ShowPanelsPerson(RolType.AdditionalData);
                }
                else {
                    $.UifNotify('show',
                        {
                            'type': 'info',
                            'message': AppResourcesPerson.LabelInformative + ":<br>" + valid
                        });
                }
            }
            return false;
        }
    }
        modalTaxOpen() {
            positionTaxEdit = null;
        }

}