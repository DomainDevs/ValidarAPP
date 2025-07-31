var guaranteeTmp = {};

class Other extends Uif2.Page {
    getInitialState() { }

    bindEvents() { }

    static showOthers(data) {
        guaranteeTmp = data;
        window.TypeGuarantee = GuaranteeType.Others;
        $("#Others-Observations").val(data.DescriptionOthers);
    }

    static clearOthers() {
        $("#Others-Observations").val("");
    }

    static bindEventsOthers() {
        window.TypeGuarantee = GuaranteeType.Others;
    }

    static SaveOthers() {
        $("#Guarantee").validate();
        $("#AddOthers").validate();

        var guaranteeValid = $("#Guarantee").valid();
        var promissoryNoteValid = $("#AddOthers").valid();

        if (guaranteeValid && promissoryNoteValid) {
            Other.pushOthers(guaranteeId);
        }
    }

    static pushOthers(id) {
        var ID = 0;
        var dateRegistration;
        if (guaranteeTmp != null) {
            ID = guaranteeTmp.Id;
            dateRegistration = guaranteeTmp.RegistrationDate;
        }
        var othersData = {
            Branch: { Id: $("#selectBranchGuarantee").UifSelect("getSelected"), Description: $("#selectBranchGuarantee").UifSelect("getSelectedText") },
            ClosedInd: $('#IsClosed').is(':checked'),
            RegistrationDate: dateRegistration,
            DescriptionOthers: $("#Others-Observations").val(),
            Guarantee: {
                Id: $("#selectGuaranteeList").UifSelect("getSelected"),
                Description: $("#selectGuaranteeList").UifSelect("getSelectedText"),
                HasApostille: false,
                HasPromissoryNote: false,
                GuaranteeType: { Id: GuaranteeType.Others, Description: null }
            },
            Id: ID,
            IndividualId: individualId,
            LastChangeDate: DateNowPerson,
            Status: { Id: $("#selectStatusGuarantee").UifSelect("getSelected"), Description: $("#selectStatusGuarantee").UifSelect("getSelectedText") }
        };

        GuaranteeRequest.CreateInsuredGuaranteeOthers(othersData).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $.UifDialog('alert', { 'message': AppResources.SuccessfullyGuaranteesSaved });
                    Guarantee.ClearFormByGuaranteeType(GuaranteeType.Others);
                    Guarantee.LoadGuarantees();
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorSavingGuarantee });
            }
        });

    }
}
