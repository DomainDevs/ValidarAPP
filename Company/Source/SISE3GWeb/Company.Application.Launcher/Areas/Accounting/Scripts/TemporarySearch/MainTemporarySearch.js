//declaración de variables


var branchId = -1;
var applicationTypeId = 0;
var startDate = "";
var endDate = "";
var userId = -1;
var temporaryNumber = -1;
var accountingDate = "";
var tempSourceCode = -1;
var tempBranchName = "";
var tempImputationId = 0;
var amount = 0;
var registerDate = "";

var tempJournalEntryPromise;

var oItemsToDeleteGrid = {
    temporals: []
};

var oTemporal = {
    tempImputationId: 0,
    imputationTypeId: 0,
    sourceId: 0
};



$(() => {
    new MainTemporarySearch();
});

class MainTemporarySearch extends Uif2.Page {

    getInitialState() {
        $("#DeleteTemporaryButton").hide();

        // Controla que la fecha inicial sea menor o igual a la final
        $('#StartDateTempSearch').on('datepicker.change', function (event, date) {

            if ($("#EndDateTempSearch").val() != "") {

                if (compare_dates($('#StartDateTempSearch').val(), $("#EndDateTempSearch").val())) {

                    $("#alertTempSearch").UifAlert('show', Resources.ValidateDateTo, "warning");

                    $("#StartDateTempSearch").val('');
                } else {
                    $("#StartDateTempSearch").val($('#StartDateTempSearch').val());
                    $("#alertTempSearch").UifAlert('hide');
                }
            }
        });

        // Controla que la fecha final sea mayor o igual a la inicial
        $('#EndDateTempSearch').on('datepicker.change', function (event, date) {

            if ($("#StartDateTempSearch").val() != "") {
                if (compare_dates($("#StartDateTempSearch").val(), $('#EndDateTempSearch').val())) {

                    $("#alertTempSearch").UifAlert('show', Resources.ValidateDateFrom, "warning");

                    $("#EndDateTempSearch").val('');
                } else {
                    $("#EndDateTempSearch").val($('#EndDateTempSearch').val());
                    $("#alertTempSearch").UifAlert('hide');
                }
            }
        });


        setTimeout(function () {
            setParametersFromTempPreliquidation($("#ViewBagTempPreliquidationBranchId").val(),
                $("#ViewBagTempImputationId").val(),
                $("#ViewBagAplicationPreLiquidation").val());
        }, 700);

    }

    bindEvents() {
        $('#SearchTemporalsTable').on('rowSelected', this.temporalsTableRowSelected);
        $('#SearchTemporalsTable').on('selectAll', this.temporalsTableSelectAll);
        $('#SearchTemporalsTable').on('desSelectAll', this.temporalsTableDesSelectAll);
        $("#TemporaryUser").on('itemSelected', this.temporaryUserItemSelected);
        $("#StartDateTempSearch").on('blur', this.startDateTempSearchBlur);
        $("#EndDateTempSearch").on('blur', this.endDateTempSearchBlur);
        $("#DeleteTemporaryButton").on('click', this.deleteTemporary);
        $('#SearchTemporalsTable').on('rowEdit', this.showApplicationType);
        $("#SearchTemporaryButton").on('click', this.searchTemporaryRecords);
        $("#CleanTemporaryButton").on('click', this.setDataFieldsEmpty);
        $("#TemporaryUser").on('blur', this.temporaryUserAutocompleteControl);
    }

    temporalsTableRowSelected(event, data, position) {
        $("#DeleteTemporaryButton").show();
        $("#alertTempSearch").UifAlert('hide');
    }

    temporalsTableSelectAll(event, data, position) {
        var rowTemporals = $("#SearchTemporalsTable").UifDataTable("getSelected");
        if (rowTemporals != null) {
            $("#DeleteTemporaryButton").show();
            $("#alertTempSearch").UifAlert('hide');
        }
    }

    temporalsTableDesSelectAll(event, data, position) {
        $("#DeleteTemporaryButton").hide();
        $("#alertTempSearch").UifAlert('hide');
    }

    temporaryUserItemSelected(event, selectedItem) {
        userId = selectedItem.id;
        if (userId < 0 || userId == "") {
            $("#TemporaryUser").val("");
            $("#TemporaryUser").trigger('blur');
        }
    }

    startDateTempSearchBlur() {
        if ($("#StartDateTempSearch").val() != '') {
            if (IsDate($("#StartDateTempSearch").val()) == true) {
                if ($("#EndDateTempSearch").val() != '') {
                    if (compare_dates($('#StartDateTempSearch').val(), $("#EndDateTempSearch").val())) {
                        $("#alertTempSearch").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#StartDateTempSearch").val('');
                    }
                }
            } else {
                $("#alertTempSearch").UifAlert('show', Resources.InvalidDates, "danger");
                $("#StartDateTempSearch").val("");
            }
        }
    }

    endDateTempSearchBlur() {
        if ($("#EndDateTempSearch").val() != '') {
            if (IsDate($("#EndDateTempSearch").val()) == true) {
                if (compare_dates($("#StartDateTempSearch").val(), $('#EndDateTempSearch').val())) {
                    $("#alertTempSearch").UifAlert('show', Resources.ValidateDateFrom, "warning");
                    $("#EndDateTempSearch").val('');
                }
            } else {
                $("#alertTempSearch").UifAlert('show', Resources.InvalidDates, "danger");
                $("#EndDateTempSearch").val("");
            }
        }
    }

    deleteTemporary() {
        var rowid = $("#SearchTemporalsTable").UifDataTable("getSelected");

        if (rowid != null) {
            ConfirmDialogDelete(Resources.DeletedMultipleRecords, rowid);
        } else {
            $("#alertTempSearch").UifAlert('show', Resources.SelectOneItem, "warning");
            return;
        }
    }

    searchTemporaryRecords() {
        $("#alertTempSearch").UifAlert('hide');
        $("#DeleteTemporaryButton").hide();
        if ($("#TemporarySearchForm").valid()) {

            branchId = $("#TemporaryBranchDrop").val();
            applicationTypeId = $("#ApplicationTypeDrop").val();
            startDate = $("#StartDateTempSearch").val();
            endDate = $("#EndDateTempSearch").val();
            temporaryNumber = $("#TemporaryNumber").val();

            if (temporaryNumber == "") {
                temporaryNumber = -1;
            }
            if (userId == -1 || $("#TemporaryUser").val() == "") {
                userId = "";
            }

            $("#SearchTemporalsTable").dataTable().fnClearTable();

            var control = ACC_ROOT + "TemporarySearch/SearchTemporaryItems?branchId=" + branchId +
                "&imputationId=" + temporaryNumber + "&userId=" + userId + "&imputationTypeId=" + applicationTypeId +
                "&startDate=" + startDate + "&endDate=" + endDate;

            $("#SearchTemporalsTable").UifDataTable({ source: control });
        }
    }

    showApplicationType(event, data, position) {
                
        $("#DeleteTemporaryButton").hide();
        $("#alertTempSearch").UifAlert('hide');
        var salePointId;
        var companyId;
        var generationDate;
        var personTypeId;
        var documentNumber;
        var name;
        var beneficiaryId;
        var description;
        var comments;
        var tempImputationTypeId = 0;
        var imputationTypeCode;

        if (data != null) {

            temporaryNumber = data.TemporaryNumber;
            tempImputationId = data.TemporaryNumber;
            tempImputationTypeId = parseInt(data.ImputationTypeCode);
            tempSourceCode = data.SourceCode;
            tempBranchName = data.BranchName;
            branchId = data.BranchCode;
            imputationTypeCode = tempImputationTypeId;
            registerDate = data.RegisterDate;

            switch (tempImputationTypeId) {

                case parseInt($("#ViewBagAplicationBill").val()):
                    getReceiptApplicationInformation(tempSourceCode).then(function (data) {

                        lockScreen();
                        setTimeout(function () {  

                            location.href = $("#ViewBagLoadApplicationReceiptLink").val() + "?receiptNumber=" +
                                tempSourceCode +
                                "&depositer=" + data[0].PayerDocumentNumber + " - " + data[0].Payer + 
                                "&amount=" + data[0].PaymentsTotal +
                                "&localAmount=" + data[0].PaymentsTotal +
                                "&branch=" + tempBranchName +
                                "&incomeConcept=" + data[0].CollectConceptDescription +
                                "&postedValue=" + data[0].PostdatedValue +
                                "&description=" + data[0].CollectDescription +
                                "&accountingDate=" + registerDate +
                                "&tempImputationId=" + tempImputationId +
                                "&comments=" + data[0].Comments +
                                "&branchId=" + branchId +
                                "&technicalTransaction=0" +
                                    "&applyCollecId=" + tempSourceCode;
                        }, 500);
                    });
                    break;

                case parseInt($("#ViewBagAplicationJournalEntry").val()):
                    getTempJournalEntryByTempId(tempSourceCode);
                    tempJournalEntryPromise.then(function (tempJournalEntrData) {

                    lockScreen();
                        setTimeout(function () {  

                            salePointId = tempJournalEntrData.SalesPointId;
                            companyId = tempJournalEntrData.CompanyId;
                            generationDate = tempJournalEntrData.AccountingDate;
                            personTypeId = tempJournalEntrData.PersonTypeId;
                            documentNumber = tempJournalEntrData.DocumentNumber;
                            name = tempJournalEntrData.PayerName;
                            beneficiaryId = tempJournalEntrData.IndividualId;
                            description = tempJournalEntrData.Description;
                            comments = tempJournalEntrData.Comments;

                            location.href = $("#ViewBagLoadJournalEntryLink").val() + "?generationDate=" +
                                generationDate + "&branchId=" + branchId + "&salePointId=" + salePointId +
                                "&companyId=" + companyId + "&personTypeId=" + personTypeId +
                                "&documentNumber=" + documentNumber + "&name=" + name +
                                "&beneficiaryId=" + beneficiaryId + "&tempImputationId=" + tempImputationId +
                                "&description=" + description + "&comments=" + comments + "&journalEntryId=" + tempSourceCode;                       
                        }, 300);
                     });

                    break;

                case parseInt($("#ViewBagAplicationPreLiquidation").val()):

                    getTempPreLiquidation(tempSourceCode).then(function (data) {
                        salePointId = data.SalesPointId;
                        companyId = data.AccountingCompanyId;
                        generationDate = data.RegisterDate;
                        personTypeId = data.PersonTypeId;
                        documentNumber = data.BeneficiaryDocumentNumber;
                        name = data.BeneficiaryName;
                        beneficiaryId = data.BeneficiaryIndividualId;
                        description = data.Description;

                        location.href = $("#ViewBagLoadPreliquidationsLink").val() + "?preliquidationId=" +
                            tempSourceCode + "&branchId=" + branchId + "&salePointId=" + salePointId +
                            "&companyId=" + companyId + "&generationDate=" + generationDate +
                            "&personTypeId=" + personTypeId + "&documentNumber=" + documentNumber +
                            "&name=" + name +
                            "&beneficiaryId=" + beneficiaryId +
                            "&tempImputationId=" + tempImputationId +
                            "&description=" + description +
                            "&isPreliquidation=2";
                    });

                    break;
                //ORDEN DE PAGO
                case parseInt($("#ViewBagAplicationPaymentOrder").val()):


                    lockScreen();
                    setTimeout(function () {  
                        
                    location.href = $("#ViewBagLoadPaymentOrdersApplicationLink").val() + "?paymentOrderId=" + tempImputationId +
                        "&amount=" + 0 + "&paymentOrderNumber=" + tempSourceCode + "&branchId=" + branchId + "&tempSearchId=" + temporaryNumber;
                    }, 1500);
                    break;
            }
        }
    }

    setDataFieldsEmpty() {
        $("#SearchTemporalsTable").dataTable().fnClearTable();
        $("#TemporaryBranchDrop").val("");
        $("#ApplicationTypeDrop").val("");
        $("#StartDateTempSearch").val("");
        $("#EndDateTempSearch").val("");
        $("#TemporaryNumber").val("");
        $("#TemporaryUser").val("");
        userId = -1;
        $("#DeleteTemporaryButton").hide();
        $("#alertTempSearch").UifAlert('hide');
    }

    temporaryUserAutocompleteControl() {
        if (userId < 0 || userId == "") {
            $('#TemporaryUser').val("");
        } else {
            ValidateUserNick(userId, $('#TemporaryUser').val()).then(function (validateData) {
                if (!validateData) {
                    $('#TemporaryUser').val("");
                    userId = -1;
                }
            });
        }
    }
}

function ConfirmDialogDelete(message, arrayTemporaryNumber) {
    $.UifDialog('confirm', { 'message': message, 'title': Resources.Delete }, function (result) {
        if (result) {
            lockScreen();

            DeleteTemporaryApplication(arrayTemporaryNumber).then(function (data) {
                                
                branchId = $("#TemporaryBranchDrop").val();
                applicationTypeId = $("#ApplicationTypeDrop").val();
                startDate = $("#StartDateTempSearch").val();
                endDate = $("#EndDateTempSearch").val();
                temporaryNumber = -1;

                var control = ACC_ROOT + "TemporarySearch/SearchTemporaryItems?branchId=" + branchId +
                    "&imputationId=" + temporaryNumber + "&userId=" + userId + "&imputationTypeId=" + applicationTypeId +
                    "&startDate=" + startDate + "&endDate=" + endDate;

                $("#SearchTemporalsTable").UifDataTable({ source: control });

                $("#DeleteTemporaryButton").hide();

                if (data.success == false) {

                    $("#alertTempSearch").UifAlert('show', data.result, 'danger');
                } else {

                    $("#alertTempSearch").UifAlert('show', Resources.DeleteSuccessfully, 'success');
                }               
                unlockScreen();
            });
        }
    });
}


function DeleteTemporaryApplication(temporary) {
    return $.ajax({
        type: 'POST',
        url: ACC_ROOT + "TemporarySearch/DeleteTemporaryApplication",
        data: JSON.stringify({ "temporals": SetDataItemToDelete(temporary) }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
}

function getReceiptApplicationInformation(tempSourceId) {
    return $.ajax({
        type: 'POST',
        url: ACC_ROOT + "ReceiptApplication/GetReceiptApplicationInformationByBillId",
        data: JSON.stringify({ "billId": tempSourceId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
}

function getTempJournalEntryByTempId(tempSourceId) {
    return tempJournalEntryPromise = new Promise(function (resolve, reject) {

      lockScreen();
      setTimeout(function () {        
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "JournalEntry/GetTempJournalEntryByTempId",
            data: { "tempJournalEntryId": tempSourceId }
        }).done(function (tempJournalEntrData) {
            unlockScreen();
            resolve(tempJournalEntrData);
         });
       }, 500);
    });
}


function getTempPreLiquidation(tempSourceId) {

    lockScreen();    
    return $.ajax({
        type: 'POST',
        url: ACC_ROOT + "PreLiquidations/GetTempPreLiquidationByTempPreliquidationId",
        data: JSON.stringify({ "tempPreliquidationId": tempSourceId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function () {
        unlockScreen();
    });
}

function setParametersFromTempPreliquidation(branchId, number, type) {

    if (branchId != "" && number != "" && type != "") {

        $("#TemporaryBranchDrop").val(branchId);
        $("#TemporaryNumber").val(number);
        $("#ApplicationTypeDrop").val(type);

        setTimeout(function () {
            $("#SearchTemporaryButton").trigger('click');
        }, 300);
    }
}

function ValidateUserNick(userId, userNick) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BillSearch/ValidateUserNick",
            data: JSON.stringify({
                "userId": userId,
                "userNick": userNick
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (validateData) {
                resolve(validateData);
            }
        });
    });
}

function SetDataItemToDelete(arrayTemporaryNumber) {

    oItemsToDeleteGrid.temporals = [];

    for (var i in arrayTemporaryNumber) {
        oTemporal = {
            tempImputationId: 0,
            imputationTypeId: 0,
            sourceId: 0
        };

        oTemporal.tempImputationId = arrayTemporaryNumber[i].TemporaryNumber;
        oTemporal.imputationTypeId = arrayTemporaryNumber[i].ImputationTypeCode;
        oTemporal.sourceId = arrayTemporaryNumber[i].SourceCode;

        oItemsToDeleteGrid.temporals.push(oTemporal);
    }

    return oItemsToDeleteGrid;
}

