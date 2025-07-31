$(() => {
    new AccountBlockadeSpreading();
});

var accountingAccountSourceId = 0;
var accountingAccountDestinationId = 0;
var baseNumber = "";

class AccountBlockadeSpreading extends Uif2.Page {
    getInitialState() {

    }
    bindEvents() {
        $("#SourceAccount").on("itemSelected", this.SourceAccountAutocomplete)
        $("#DestinationAccount").on("itemSelected", this.DestinationAccountAutocomplete)
        $("#Spread").on("click", this.ConfirmSpreading)
        $("#Cancel").on("click", AccountBlockadeSpreading.ClearAll)
        $("#SpreadConfirmationModal").find("#spreadConfirmationAccept").on("click", this.ExecuteSpreading)
        $("#DestinationAccount").on("blur", AccountBlockadeSpreading.Checkwildcard)
    }
    SourceAccountAutocomplete(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.AccountingAccountId > 0) {
                accountingAccountSourceId = selectedItem.AccountingAccountId;
            }
        }
    }
    DestinationAccountAutocomplete(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.AccountingAccountId > 0) {
                accountingAccountDestinationId = selectedItem.AccountingAccountId;
            }
        }
    }
    ConfirmSpreading() {
        $("#blockadeSpreadingForm").validate();
        if ($("#blockadeSpreadingForm").valid()) {
            if ($("#DestinationAccount").val() != "" && $("#DestinationAccount").val() != null) {
                var result = false;

                result = AccountBlockadeSpreading.Checkwildcard();
                if (result) {
                    if ($("#DestinationAccount").val().includes("%")) {

                        accountingAccountDestinationId = 0;
                        baseNumber = $("#DestinationAccount").val();
                        ValidateAccountsToBeModified(baseNumber).then(function (spreadingData) {
                            if (spreadingData == 0) {
                                //no existen datos                                
                                $("#blockadeSpreadingAlert").UifAlert('show', Resources.NoRecordsToContinue, "warning");
                            }
                            if (spreadingData > 100) {
                                //no se puede procesar más de 100 registros                                
                                $("#blockadeSpreadingAlert").UifAlert('show', Resources.CantProcessMoreThanHundred, "warning");
                            }
                            if (spreadingData > 0 && spreadingData < 100) {
                                //se ejecuta la propagación.
                                $("#SpreadConfirmationModal").UifModal('showLocal', Resources.Warning);
                            }
                        });
                    } else {
                        if (accountingAccountDestinationId != 0) {
                            if (!$("#DestinationAccount").val().includes("-")) {
                                $("#blockadeSpreadingAlert").UifAlert('show', Resources.DestinationAccountNotEntered, "warning");
                            } else {
                                // alert("se ejecuta - única cuenta");
                                $("#SpreadConfirmationModal").UifModal('showLocal', Resources.Warning);
                            }

                        } else {
                            if ($("#DestinationAccount").val().includes("-")) {
                                $("#blockadeSpreadingAlert").UifAlert('show', Resources.DestinationAccountNotEntered, "warning");
                            } 
                        }
                    }
                }

            }

        }
    }
    static Checkwildcard() {
        var result = false;
        var checkValue = $("#DestinationAccount").val();
        var count = 0;

        count = checkValue.split('%').length - 1;
        if (count > 1) {
            $("#blockadeSpreadingAlert").UifAlert('show', Resources.OneWildcardValidation, "warning");
            result = false;
        } else {
            result = true;
        }
        if (result) {
            var part = checkValue.split("%");

            if (part[0] == "") {
                $("#blockadeSpreadingAlert").UifAlert('show', Resources.WildcardBeginingValidation, "warning");
                result = false;
            } else {
                result = true;
            }
        }
        if (result) {
            var part = checkValue.split("%");

            if (part.length > 1 && part[1] != "") {
                $("#blockadeSpreadingAlert").UifAlert('show', Resources.WildcardIntermediateValidation, "warning");
                result = false;
            } else {
                result = true;
            }

        }
        if(result){
            $("#blockadeSpreadingAlert").UifAlert('hide');
        }

        return result;
    }
    ExecuteSpreading(){
        $("#SpreadConfirmationModal").UifModal('hide');

        SpreadAccountBlockade(accountingAccountSourceId, accountingAccountDestinationId, baseNumber).then(function(spreadingData){
            if (spreadingData){
                $("#blockadeSpreadingAlert").UifAlert('show', Resources.SaveEnded, "success");
                AccountBlockadeSpreading.ClearFields();
            }else{
                $("#blockadeSpreadingAlert").UifAlert('show', Resources.ErrorInProcess, "danger");
                AccountBlockadeSpreading.ClearFields();
            }
        });
    }
    static ClearAll(){
        $("#blockadeSpreadingAlert").UifAlert('hide');
        accountingAccountSourceId = 0;
        accountingAccountDestinationId = 0;
        baseNumber = "";
        $("#SourceAccount").val("");
        $("#DestinationAccount").val("");
    }
    static ClearFields(){        
        accountingAccountSourceId = 0;
        accountingAccountDestinationId = 0;
        baseNumber = "";
        $("#SourceAccount").val("");
        $("#DestinationAccount").val("");
    }    
}

function ValidateAccountsToBeModified(baseNumber) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountingAccount/ValidateAccountsToBeModified",
            data: { "baseNumber": baseNumber },
            success: function (spreadingData) {
                resolve(spreadingData);
            }
        });
    });
}

function SpreadAccountBlockade(accountSourceId, accountDestinationId, baseNumber){
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountingAccount/SpreadAccountBlockade",
            data: { "accountingAccountSourceId": accountSourceId, "accountingAccountDestinationId": accountDestinationId, "baseNumber": baseNumber },
            success: function (spreadData) {
                resolve(spreadData);
            }
        });
    });
}

