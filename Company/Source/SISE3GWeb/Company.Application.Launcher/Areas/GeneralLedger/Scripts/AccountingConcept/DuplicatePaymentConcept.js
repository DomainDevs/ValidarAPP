$(() => {
    new DuplicatePaymentConcept();
});

var accountingConceptId = 0;

class DuplicatePaymentConcept extends Uif2.Page {

    getInitialState() {

    }
    bindEvents() {
        $("#PaymentConcept").on("itemSelected", this.AccountingConceptAutocomplete)
        $("#SourceBranch").on("itemSelected", this.LoadRemainingBranches)
        $("#Duplicate").on("click", this.ConfirmDuplicate)
        $("#Cancel").on("click", DuplicatePaymentConcept.ClearAll)
        $("#DuplicateConfirmationModal").find("#duplicateConfirmationAccept").on("click", this.ExecuteDuplication)
    }
    AccountingConceptAutocomplete(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {
                accountingConceptId = selectedItem.Id;
                var controller = GL_ROOT + "AccountingConcept/GetSourceBranches?accountingConceptId=" + accountingConceptId;
                $("#SourceBranch").UifSelect({ source: controller });

                if ($("#SourceBranch").val() == null)
                    $("#SourceBranch").trigger('change')

            } else {
            }
        } else {

        }
    }
    LoadRemainingBranches(event, selectedItem) {
        if (selectedItem != null) {

            if (selectedItem.Id > 0) {
                var controller = GL_ROOT + "AccountingConcept/GetDestinationBranches?accountingConceptId=" + accountingConceptId;
                $("#DestinationBranch").UifSelect({ source: controller });
            } else {
                $("#DestinationBranch").UifSelect({ source: null });
            }
        } else {
            $("#DestinationBranch").UifSelect({ source: null });
        }
    }
    ConfirmDuplicate() {
        if ($("#PaymentConcept").val() != ""){
            if ($("#SourceBranch").val() != null && $("#SourceBranch").val() != ""){
                if($("#SourceBranch").val() != null && $("#DestinationBranch").val() != ""){
                    $("#DuplicateConfirmationModal").UifModal('showLocal', Resources.Warning);
                    $("#duplicatePaymentConceptAlert").UifAlert('hide');
                }else{
                    $("#duplicatePaymentConceptAlert").UifAlert('show', Resources.EnterField + " " + "'" + Resources.ToBranch + "'", "danger")
                }                
            }else{
                $("#duplicatePaymentConceptAlert").UifAlert('show', Resources.EnterField + " " + "'" + Resources.FromBranch + "'", "danger")
            }
        }else{
            $("#duplicatePaymentConceptAlert").UifAlert('show', Resources.EnterField + " " + "'" + Resources.ConceptToBeDuplicated + "'", "danger")
        }        
    }
    static ClearFields(){        
        accountingConceptId = 0;
        $("#PaymentConcept").val("");
        $("#SourceBranch").UifSelect({ source: null });
        $("#DestinationBranch").UifSelect({ source: null });
    }
    static ClearAll(){
        $("#duplicatePaymentConceptAlert").UifAlert('hide');
        accountingConceptId = 0;
        $("#PaymentConcept").val("");
        $("#SourceBranch").UifSelect({ source: null });
        $("#DestinationBranch").UifSelect({ source: null });
    }
    ExecuteDuplication(){
        $("#DuplicateConfirmationModal").UifModal('hide');
        var sourceBranch = $("#SourceBranch").val();
        var destinationBranch = $("#DestinationBranch").val();

        SaveDuplication(accountingConceptId, sourceBranch, destinationBranch).then(function(duplicationData){
            if (duplicationData == 1){
                $("#duplicatePaymentConceptAlert").UifAlert('show', Resources.SaveEnded, "success");
                DuplicatePaymentConcept.ClearFields();
            }
            if (duplicationData == 0){
                $("#duplicatePaymentConceptAlert").UifAlert('show', Resources.ErrorInProcess, "danger");
            }
            if (duplicationData == 2){
                $("#duplicatePaymentConceptAlert").UifAlert('show', Resources.PaymentConceptBeenCopied, "info");
            }
        });
    }
}

function SaveDuplication(paymentConceptId, sourceBranch, destinationBranch){
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountingConcept/SavePaymentConceptDuplication",
            data: { "paymentConceptId": paymentConceptId, "sourceBranch": sourceBranch, "destinationBranch": destinationBranch },
            success: function (duplicationData) {
                resolve(duplicationData);
            }
        });
    });
}