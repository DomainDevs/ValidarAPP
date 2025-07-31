
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//                                                                      DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var timerLoadJournalEntry = 0;
var amount = 0;
var creditAmount = 0;
var debitAmount = 0;
var tempImputationId = 0;
var isMulticompany;
var salePointBranchUserDefault;
var userId;
var userNick;
var journalPayerIndividualId;
var journalEntryCode;
var accountingCompanyDefault;
var branchUserDefault = $("#ViewBagBranchUserDefault").val();
var statusApplied = $("#ViewBagStatusApplied").val();
var applyCollecId = $("#ViewBagApplyCollecId").val();
var applyAccountingDate = $("#ViewBagDateAccounting").val();
var timeJournal = window.setInterval(validateFieldsForDefault, 1000);
var personTypeTrigger = true;

var oItemsToDeleteGrid = {
    temporals: []
};
var oTemporal = {
    tempImputationId: 0,
    imputationTypeId: 0,
    sourceId: 0
};


/* Pagos siniestros y varios */
var oClaimsPaymentRequestItem = {
    TempClaimPaymentCode: 0,
    TempImputationCode: 0,
    PaymentRequestCode: 0,
    ClaimCode: 0,
    BeneficiaryId: 0,
    CurrencyCode: 0,
    IncomeAmount: 0,
    ExchangeRate: 0,
    Amount: 0,
    RegistrationDate: null,
    EstimationDate: null,
    BussinessType: 0,
    RequestType: 0,
    PaymentNum: 0,
    PaymentExpirationDate: null,
    PaymentRequestNumber: 0
};

var oPaymentRequestItem = {
    TempPaymentCode: 0,
    TempImputationCode: 0,
    PaymentRequestCode: 0,
    BeneficiaryId: 0,
    CurrencyCode: 0,
    IncomeAmount: 0,
    ExchangeRate: 0,
    Amount: 0,
    RegistrationDate: null,
    EstimationDate: null,
    BussinessType: 0,
    PaymentNumber: 0,
    PaymentExpirationDate: null,
    PaymentRequestNumber: 0
};

var oJournalEntry = {
    JournalEntryItemId: 0,
    AccountingDate: null,
    BranchId: 0,
    CompanyId: 0,
    Comments: null,
    Description: null,
    IndividualId: 0,
    PersonTypeId: 0,
    SalePointId: 0,
    StatusId: 0
};

$("#listViewAplicationReceipt").UifListView();

JournalEntryZero();

//cargo los datos para edición
if ($("#ViewBagIsEdit").val() == 'True') {
    
    lockScreen();
    timerLoadJournalEntry = window.setInterval(loadAllFieldsJournal, 1500);
}


//Verifica si están llenados todos los campos
function loadAllFieldsJournal() {
    
     loadJournalEntry();
    
            if (
                ($("#listViewAplicationReceipt").UifListView("getData").length > 0) &&
                (($("#AccountingDateEntry").val() != "" && $("#AccountingDateEntry").val() != null) &&
                ($("#BranchJournalEntry").val() != "" && $("#BranchJournalEntry").val() != null) &&                
                ($("#CompanyJournalEntry").val() != "" && $("#CompanyJournalEntry").val() != null) &&                
                ($("#DescriptionJournal").val() != "" && $("#DescriptionJournal").val() != null) &&
                ($("#SalesPointJournal").val() != null) &&
                ($("#PayerTypeJournal").val() != null )) 
                
            ) {
                    unlockScreen();            
                    setTimeout(function () {
                         clearInterval(timerLoadJournalEntry);
                    }, 2000);                    
            }            
}


//Se ocultan campos de busqueda de autocompletes
$(window).load(function () {

    cleanAutocompletesJournal('SearchSuppliers');
    cleanAutocompletesJournal('SearchInsured');
    cleanAutocompletesJournal('SearchCoinsurance');
    cleanAutocompletesJournal('SearchPerson');
    cleanAutocompletesJournal('SearchAgent');
    cleanAutocompletesJournal('SearchEmployee');
    cleanAutocompletesJournal('SearchReinsurer');

    //Cargando los eventos
    loadAutocompleteEventJournal('SearchSuppliers');
    loadAutocompleteEventJournal('SearchInsured');
    loadAutocompleteEventJournal('SearchCoinsurance');
    loadAutocompleteEventJournal('SearchPerson');
    loadAutocompleteEventJournal('SearchAgent');
    loadAutocompleteEventJournal('SearchEmployee');
    loadAutocompleteEventJournal('SearchReinsurer');
});

setTimeout(function () {
    SetTotalApplicationJournal();
    SetTotalControlJournal();
}, 3000);


// S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R
// Botón Aceptar
$("#SaveSearchPoliciesButton").click(function () {
    
    if ($("#ViewBagImputationType").val() == "2") {
        var amount = 0;
        if ($("#ViewBagAmount").val() != "") {
            amount = parseFloat(ClearFormatCurrency($("#ViewBagAmount").val().replace("", ",")));
        }

        //GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount);

        SetDataPremiumReceivableEmpty();
        ClearFields();
        HideFields();

        CheckLoadedMovements();
        loadedMovementsPromise.then(function(isLoaded){
            if(isLoaded){
                clearTimeout(time);
                SetTotalApplicationJournal();
                $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitJournal(), TotalCreditJournal(), 3))));
            }
        });

        /* setTimeout(function () {
            SetTotalApplicationJournal();
            $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitJournal(), TotalCreditJournal(), 3))));
        }, 2000); */

        $('#ModalPremiums').UifModal('hide');
    }
});


// Botón cerrar modal
$("#CloseModalButton").click(function () {

    var amount = 0;
    if ($("#ViewBagAmount").val() != "") {
        amount = parseFloat(ClearFormatCurrency($("#ViewBagAmount").val().replace("", ",")));
    }

    GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount);

    SetDataPremiumReceivableEmpty();
    ClearFields();
    HideFields();

    setTimeout(function () {
        SetTotalApplicationJournal();
        $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitJournal(),TotalCreditJournal(), 3))));
    }, 2000);

    $('#ModalPremiums').UifModal('hide');
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                                    ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/


/// Botón para edición en el listview
$("#listViewAplicationReceipt").on('rowEdit', function (event, data, index) {

    $("#JournalyFormulario").validate();
    if ($("#JournalyFormulario").valid() && journalPayerIndividualId > 0) {

        SaveJournalTemporal();
        $("#alertJournaly").hide();

        // Primas por cobrar
        //if (data.Id == 1) {

        //    refreshApplyView();
        //    ClearSearchFields();
        //    LoadSearchByForPolicies();
        //    LoadBranch();
        //    LoadPrefix();
        //    var msjTitle = Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + tempImputationId;
        //    $("#ModalPremiumsTitleDialog").text(msjTitle);
        //    $('#ModalPremiums').UifModal('showLocal', msjTitle);
        //}

        // Primas en depósito
        if (data.Id == 2) {
            $('#modalPremiumDeposit').UifModal('showLocal', Resources.AgentMovementTitle);
            
        }
        // Comisiones descontadas
        if (data.Id == 3) {
            $('#modalDiscountedCommission').UifModal('showLocal', Resources.AgentMovementTitle);
        }
        // Cuenta corriente agentes
        if (data.Id == 4) {
            editAgent = -1;
            $("#addAgentForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
            LoadAgentBranchs();
            LoadAgentNatures();
            LoadAgentCurrencies();
            LoadAgentCompanies();
            $('#modalAgent').UifModal('showLocal', Resources.AgentMovementTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            SetAgentFieldEmpty();
            SetAgentTotalMovement();
            SetAgentAccountingCompany();
        }
        // Cuenta corriente coaseguros
        if (data.Id == 5) {
            editCoinsurance = -1;
            $("#addCoinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
            LoadCoinsuresMovementsBranchs();
            LoadCoinsuresMovementsCoinsuranceTypes();
            LoadCoinsuresMovementsNatures();
            LoadCoinsuresMovementsCurrencies();
            LoadCoinsuresMovementsAccountingCompanies(oJournalEntry.CompanyId);
            $('#modalCoinsurance').UifModal('showLocal', Resources.DialogCoinsuranceMovementsTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            SetCoinsuranceFieldEmpty();
            SetCoinsuranceTotalMovement();
            SetCoinsuranceAccountingCompany();
        }
        // Cuenta corriente reaseguros
        if (data.Id == 6) {
            editReinsurance = -1;
            $("#addReinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
            LoadReinsuraceMovementsBranchs();
            LoadTechnicalPrefixes();
            LoadContractTypeEnabled();
            LoadReinsuraceMovementsNature();
            LoadReinsuraceMovementsCurrencies();
            LoadReinsuraceMovementsYearMonths();
            LoadReinsuraceMovementsCompanies(oJournalEntry.CompanyId);
            $('#modalReinsurance').UifModal('showLocal', Resources.DialogReinsuranceMovementsTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            $("#modalReinsurance").find("#OptionalOne").hide();
            $("#modalReinsurance").find("#OptionalTwo").hide();
            $("#modalReinsurance").find("#OptionalThree").hide();
            SetReinsuranceFieldEmpty();
            SetReinsuranceTotalMovement();
            SetReinsuranceAccountingCompany();
        }
        //// Contabilidad
        //if (data.Id == 7) {
        //    $("#addAccountingMovementForm").formReset();
        //    LoadAccountingBranchs();
        //    LoadAcountingNatures();
        //    LoadAccountingCurrencies();
        //    LoadAcountingCompanies(accountingCompanyDefault);
        //    AccountingMovementsReload();
        //    LoadAccountingMovementCompany();
        //    LoadthirdAccountingUsed(); // DANC 2018-06-13
        //    SetAccountingTotalMovement();
        //    GetAccountingSalePoints();
        //    $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
        //}
        // Solicitud pagos varios
        if (data.Id == 8) {
            $("#modalVarious").find("#TitleAgentNumber").hide();
            $("#modalVarious").find("#TitleEmployeeNumber").hide();
            $("#modalVarious").find("#TitleSupplierNumber").hide();
            $("#modalVarious").find("#TitleAgentName").hide();
            $("#modalVarious").find("#TitleEmployeeName").hide();
            $("#modalVarious").find("#TitleSupplierName").hide();

            $('#modalVarious').UifModal('showLocal', Resources.VariousPaymentRequest + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            $("#modalVarious").find("#requestVariousListView").UifListView();
            LoadPersonTypes();
            LoadPaymentVariousBranchs(branchUserDefault);
            LoadPaymentVariousCompanies(oJournalEntry.CompanyId)
            SetRequestVariousFieldEmpty();
            RefreshRequestVariousMovements();
            SetVariousTotalMovement();
            SetVariousAccountingCompany();
            GetVariousRequestSalePoints();
        }
        // Solicitud pagos siniestros
        if (data.Id == 9) {
            $("#modalClaims").find("#TitleAgentNumber").hide();
            $("#modalClaims").find("#TitleInsuredNumber").hide();
            $("#modalClaims").find("#TitleSupplierNumber").hide();
            $("#modalClaims").find("#TitleAgentName").hide();
            $("#modalClaims").find("#TitleInsuredName").hide();
            $("#modalClaims").find("#TitleSupplierName").hide();

            $('#modalClaims').UifModal('showLocal', Resources.ClaimsPaymentRequestLabel + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            $("#modalClaims").find("#requestClaimsListView").UifListView();
            LoadPaymentClaimsMovementsPersonTypes();
            LoadPaymentClaimsMovementsBranchs(branchUserDefault)
            LoadPaymentClaimsMovementsPrefixes();
            LoadPaymentClaimsMovementsRequestTypes();
            LoadPaymentClaimsMovementsCompanies(oJournalEntry.CompanyId);
            SetRequestClaimsFieldEmpty();
            RefreshRequestClaimsMovements();
            SetClaimsTotalMovement();
            SetClaimsAccountingCompany();
            GetClaimsRequestSalePoints();
        }
        // Préstamos asegurados
        if (data.Id == 10) {
            editInsuredLoan = -1;
            $("#addCoinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
            $('#modalInsuredLoan').UifModal('showLocal', Resources.DialogInsuredLoanMovementsTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            SetInsuredLoanFieldEmpty();
            SetInsuredLoanTotalMovement();
        }
    } else {
        $("#alertJournaly").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
    }

});


/*
/// Botón aceptar aplicación de asiento diario
$("#JournalAccept").click(function () {
    
    $("#alertJournaly").UifAlert('hide');
    $("#JournalyFormulario").validate();
    if ($("#JournalyFormulario").valid()) {
        if (TotalDebitJournal() != 0 || TotalCreditJournal() != 0) {
            if (SetMainTotalControl(0, TotalDebitJournal(), TotalCreditJournal(), 3) != 0) {
                //DIALOGO DE COMFIRMACION PARA GRABAR IMPUTATION EN TEMPORALES, ACTUALIZA ESTADO DE JOURNAL ENTRY
                SaveJournalTemporalConfirmation(Resources.SaveTemporalConfirmationMessage);
                $("#modalAgent").find("#agentsListView").UifListView("clear");
            }
            else {
                //Actualiza fecha y estado aplicado - JOURNAL ENTRY

                lockScreen();
                setTimeout(function () {
                    $.ajax({
                    type: "POST",
                    async: false,
                    url: ACC_ROOT + "JournalEntry/UpdateTempJournalEntry",
                    data: JSON.stringify({ "journalEntryModel": SetDataJournalEntry(statusApplied) }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",

                    success: function (data)
                    {
                        if (data.success == false) {
                            
                            $("#alertJournaly").UifAlert('show', data.result, "danger");
                                    
                        } else {
                            
                        //GRABA IMPUTATION A REALES, ELIMINA DE TEMPORALES, ACTUALIZA ESTADO DE JOURNAL ENTRY
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "JournalEntry/SaveRealJournalEntryApplication",
                            data: {
                                "tempJournalEntryId": journalEntryCode, "tempImputationId": tempImputationId,
                                "imputationTypeId": $("#ViewBagImputationType").val(), "statusId": 3
                            },
                            success: function (dataEntry) {

                                unlockScreen();
                                //EXCEPCION ROLLBACK
                                    if (dataEntry.success == false) {
                                        
                                        $("#alertJournaly").UifAlert('show', dataEntry.result, "danger");

                                } else {
                                        if (dataEntry.IsEnabledGeneralLedger == false) {
                                        $("#accountingLabelDiv").hide();
                                        $("#accountingMessageDiv").hide();
                                    } else {
                                        $("#accountingLabelDiv").show();
                                        $("#accountingMessageDiv").show();
                                    }
                                        $("#journalEntryId").val(dataEntry.JournalEntryId + " - " + Resources.TransactionNumber + " " + dataEntry.TechnicalTransaction);
                                    $("#accountingMessage").val(dataEntry.Message);
                                    $('#modalSuccess').UifModal('showLocal', Resources.JournalEntrySaveSuccess);

                                }
                                SetDataJournalEntryEmpty();
                                tempImputationId = 0;
                                SaveTempJournalEntryZero();
                                setDataFieldsEmptyJournalEntry();
                            }
                        });
                    }
                    }
                });
                }, 500);
                $("#modalAgent").find("#agentsListView").UifListView("clear");
           }
        }
        else {
            $("#alertJournaly").UifAlert('show', Resources.MovementsTypeValidation, "warning");
        }
    }
});*/

/// Botón Cancelar
$("#JournalCancel").click(function (e) { showConfirmTempCancelJournal(); $("#modalAgent").find("#agentsListView").UifListView("clear"); });


$('#PayerTypeJournal').on('itemSelected', function (event, selectedItem) {
    
    cleanAutocompletesJournal('SearchSuppliers');    
    cleanAutocompletesJournal('SearchInsured');
    cleanAutocompletesJournal('SearchCoinsurance');
    cleanAutocompletesJournal('SearchPerson');
    cleanAutocompletesJournal('SearchAgent');
    cleanAutocompletesJournal('SearchEmployee');
    cleanAutocompletesJournal('SearchReinsurer');

    setTimeout(function () {
        
        if (selectedItem.Id != "") {

            $("#BeneficiaryDocumentNumber").hide();
            $("#BeneficiaryName").hide();            
                              
            if (selectedItem.Id == $("#ViewBagSupplierCode").val()) {
                
                $('#SearchSuppliersByDocumentNumber').parent().parent().show();
                $("#SearchSuppliersByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagInsuredCode").val()) {              
                $("#SearchInsuredByDocumentNumber").parent().parent().show();
                $("#SearchInsuredByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagCoinsurerCode").val()) {

                $("#SearchCoinsuranceByDocumentNumber").parent().parent().show();
                $("#SearchCoinsuranceByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagThirdPartyCode").val()) {

                $("#SearchPersonByDocumentNumber").parent().parent().show();
                $("#SearchPersonByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagProducerCode").val()) {

                $("#SearchAgentByDocumentNumber").parent().parent().show();
                $("#SearchAgentByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagEmployeeCode").val()) {
               
                $("#SearchEmployeeByDocumentNumber").parent().parent().show();
                $("#SearchEmployeeByName").parent().parent().show();                
            
            } else if (selectedItem.Id == $("#ViewBagReinsurerCode").val()) {

                $("#SearchReinsurerByDocumentNumber").parent().parent().show();
                $("#SearchReinsurerByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagTradeConsultant").val()) {

                $("#SearchPersonByDocumentNumber").parent().parent().show();
                $("#SearchPersonByName").parent().parent().show();
            
            } else if (selectedItem.Id == $("#ViewBagAgentCode").val()) {

                $("#SearchAgentByDocumentNumber").parent().parent().show();
                $("#SearchAgentByName").parent().parent().show();

            } else if (selectedItem.Id == '') {

                cleanAutocompletesJournal('SearchSuppliers');
                cleanAutocompletesJournal('SearchInsured');
                cleanAutocompletesJournal('SearchCoinsurance');
                cleanAutocompletesJournal('SearchPerson');
                cleanAutocompletesJournal('SearchAgent');
                cleanAutocompletesJournal('SearchEmployee');
                cleanAutocompletesJournal('SearchReinsurer');

            } else {

                $("#SearchPersonByDocumentNumber").parent().parent().show();
                $("#SearchPersonByName").parent().parent().show();
            }

        } else {
            $("#BeneficiaryDocumentNumber").show();
            $("#BeneficiaryName").show();            
            
            cleanAutocompletesJournal('SearchSuppliers');
            cleanAutocompletesJournal('SearchInsured');
            cleanAutocompletesJournal('SearchCoinsurance');
            cleanAutocompletesJournal('SearchPerson');
            cleanAutocompletesJournal('SearchAgent');
            cleanAutocompletesJournal('SearchEmployee');
            cleanAutocompletesJournal('SearchReinsurer');
        }

    }, 300);
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                            DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

//cargo los datos para edición
function loadJournalEntry() {
    
            $("#AccountingDateEntry").val($("#ViewBagGenerationDateEntry").val());
            
            setTimeout(function () {
                               
                $("#CompanyJournalEntry").val($("#ViewBagCompanyId").val());
                
                if ($("#ViewBagParameterMulticompany").val() == 0) {
                    $("#CompanyJournalEntry").attr("disabled", true);
                }
                else {
                    $("#CompanyJournalEntry").attr("disabled", false);
                }
            }, 1000);

            setTimeout(function () {

                if ($("#ViewBagPersonTypeId").val() == "0") {
                    $("#PayerTypeJournal").val("");
                } else {
                    $("#PayerTypeJournal").val($("#ViewBagPersonTypeId").val());
                }        

                if ($("#PayerTypeJournal").val() != "" && personTypeTrigger) {                    
                    $("#PayerTypeJournal").trigger('change');
                    personTypeTrigger = false
                }
            }, 100);

            $("#DescriptionJournal").val($("#ViewBagDescription").val());
            $("#ObservationsJournal").val($("#ViewBagComments").val());

            setTimeout(function () {

                journalPayerIndividualId = $("#ViewBagIndividualId").val();

                $("#SearchSuppliersByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
                $("#SearchInsuredByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
                $("#SearchCoinsuranceByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
                $("#SearchPersonByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
                $("#SearchAgentByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
                $("#SearchEmployeeByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
                $("#SearchReinsurerByDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());

                $("#SearchSuppliersByName").UifAutoComplete('setValue', $("#ViewBagName").val());
                $("#SearchInsuredByName").UifAutoComplete('setValue', $("#ViewBagName").val());
                $("#SearchCoinsuranceByName").UifAutoComplete('setValue', $("#ViewBagName").val());
                $("#SearchPersonByName").UifAutoComplete('setValue', $("#ViewBagName").val());
                $("#SearchAgentByName").UifAutoComplete('setValue', $("#ViewBagName").val());
                $("#SearchEmployeeByName").UifAutoComplete('setValue', $("#ViewBagName").val());
                $("#SearchReinsurerByName").UifAutoComplete('setValue', $("#ViewBagName").val());
            }, 1000);

            SetTotalApplicationJournal();
            $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitJournal(), TotalCreditJournal(), 3))));

}

//Se debe ejecutar solo cuando es un Asiento nuevo
function validateFieldsForDefault() {
    
    if ($("#ViewBagIsEdit").val() == 'False') {

        if ($("#CompanyJournalEntry").val() != "" && $("#CompanyJournalEntry").val() != null) {

            if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {
                
                $("#CompanyJournalEntry").attr("disabled", true);
            }
            else {
                $("#CompanyJournalEntry").attr("disabled", false);
            }
            clearInterval(timeJournal);
        }
    } else {
        clearInterval(timeJournal);
    }
}


//Actualiza el temporal de Asiento Diario para garantizar un temporal válido
function SaveJournalTemporal() {
    var obj = SetDataJournalEntry(2);
    $.ajax({
        type: "POST",
        async: false,
        url: ACC_ROOT + "JournalEntry/UpdateTempJournalEntry",
        data: JSON.stringify({ "journalEntryModel": obj }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (dataJournal) {

            if (dataJournal > 0) {
                //ACTUALIZA ESTADO, FECHA Y USUARIO DE ASIENTO DIARIO - PARCIALMENTE APLICADO
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                    data: {
                        "imputationId": tempImputationId,
                        "imputationTypeId": Resources.ImputationTypeJournalEntry,
                        "sourceCode": parseInt(journalEntryCode),
                        "comments": $("#ObservationsJournal").val(),
                        "statusId": 2
                    },
                    success: function () {

                    }
                });
            }
        }
    });
}

// Autocomplete
function cleanAutocompletesJournal(identifier) {

    $('#' + identifier + 'ByDocumentNumber').val("");
    $('#' + identifier + 'ByName').val("");
    
    journalPayerIndividualId = 0;

    $('#' + identifier + 'ByDocumentNumber').parent().parent().hide();
    $('#' + identifier + 'ByName').parent().parent().hide(); 
}

function loadAutocompleteEventJournal(identifier) {
    $('#' + identifier + 'ByDocumentNumber').on('itemSelected', function (event, selectedItem) {
        fillAutocompletes(identifier, selectedItem);
    });

    $('#' + identifier + 'ByName').on('itemSelected', function (event, selectedItem) {
        fillAutocompletes(identifier, selectedItem);
    });
}

function fillAutocompletes(identifier, selectedItem) {

    $('#' + identifier + 'ByDocumentNumber').val(selectedItem.DocumentNumber);
    $('#' + identifier + 'ByName').val(selectedItem.Name);

    if (selectedItem.Id != undefined) {
        journalPayerIndividualId = selectedItem.Id;
    } else if (selectedItem.AgentId != undefined) {
        journalPayerIndividualId = selectedItem.AgentId;
    } else {
        journalPayerIndividualId = selectedItem.CoinsuranceIndividualId;
    }   
}


/// Se refresca el listview resumen de movimientos
function GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount) {

    $("#listViewAplicationReceipt").UifListView({
        autoHeight: true, theme: 'dark',
        source: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes?tempImputationId=" + tempImputationId + "&amount=" + amount,
        customDelete: true,
        customEdit: true,
        edit: true, delete: false,
        displayTemplate: "#display-aplication-receipt-template"
    });
}

/// Obtiene el total de débitos del resumen de movimientos
function TotalDebitJournal() {
    var total = 0;

    var debits = $("#listViewAplicationReceipt").UifListView("getData");

    if (debits != null) {

        for (var j = 0; j < debits.length; j++) {
            var debitAmount = String(debits[j].Debits).replace("$", "").replace(/,/g, "").replace(" ", "");
            total += parseFloat(debitAmount);
        }
    }

    return total;
}

/// Obtiene el total de créditos del resumen de movimientos
function TotalCreditJournal() {
    var total = 0;

    var credits = $("#listViewAplicationReceipt").UifListView("getData");

    if (credits != null) {

        for (var j = 0; j < credits.length; j++) {
            var creditAmount = String(credits[j].Credits).replace("$", "").replace(/,/g, "").replace(" ", "");
            total += parseFloat(creditAmount);
        }
    }

    return total;
}

/// Setea el total de la aplicación = total recibo - para controlar
function SetTotalControlJournal() {
    
    var total = 0;
    total = parseFloat(TotalDebitJournal()) - parseFloat(TotalCreditJournal());
    $("#TotalControl").val(total);

    var receiptAmount = $("#TotalControl").val();
    $("#TotalControl").val("$ " + NumberFormatSearch(receiptAmount, "2", ".", ","));
}

/// Setea el temporal de imputación a null
function CleanGlobalVariablesJournal() {
    tempImputationId = null;
    $("#ViewBagTempImputationId").val(0);
}


// Setear el total de la listview resumen de movimientos
function SetTotalApplicationJournal() {
    var totalCreditMovement = 0;
    var totalDebitMovement = 0;

    var summaries = $("#listViewAplicationReceipt").UifListView("getData");

    if (summaries != null) {

        for (var j = 0; j < summaries.length; j++) {
            var debitAmount = String(summaries[j].Debits).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalDebitMovement += parseFloat(debitAmount);

            var creditAmount = String(summaries[j].Credits).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalCreditMovement += parseFloat(creditAmount);
        }
    }
    else {
        $("#TotalDebit").text("");
        $("#TotalCredit").text("");
    }
    $("#TotalDebit").text("$ " + NumberFormatSearch(totalDebitMovement, "2", ".", ","));
    $("#TotalCredit").text("$ " + NumberFormatSearch(totalCreditMovement, "2", ".", ","));
}

// Setear la tasa de cambio de la moneda
function SetCurrency(source, destination) {
    
    var selectCurrency = $("#" + source).val();
    var textExchangeRate = $("#" + destination);

    if (selectCurrency >= 0) {
        var response = GetCurrencyRateBilling($("#ViewBagDateAccounting").val(), selectCurrency);
        textExchangeRate.val("$ " + NumberFormatSearch(response[0], "6", ".", ","));      

        if (response[1] == false) {
            $("#alert").UifAlert('show', Resources.ExchageRateNotUpToDate, "warning");
        }
    }
    else {
        textExchangeRate.val("");
    }
}

// Obtiene la tasa de cambio de la moneda
function GetCurrencyRateBilling(accountingDate, currencyId) {
    var alert = true;
    var rate = 0;
    var response = new Array();

       $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetCurrencyExchangeRate",
        data: {"rateDate": accountingDate,"currencyId": currencyId },
           success: function (data) {
               unlockScreen();
            if (data == 1 || data == 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
                    data: {
                        "rateDate": accountingDate,
                        "currencyId": currencyId
                    },
                    success: function (dataRate) {
                        if (dataRate == 0 || dataRate == 1) {
                            rate = 1;
                            exchangeRate = rate;
                            alert = true;
                        } else {
                            rate = dataRate;
                            exchangeRate = rate;
                            alert = false;
                        }
                    }
                });
            }
            else {
                rate = data;
                alert = true;
            }
        }
    });

    response[0] = rate;
    response[1] = alert;
    exchangeRate = rate;

    return response;
}


function SetDataJournalEntry(journalStatusId) {
    oJournalEntry.JournalEntryItemId = journalEntryCode;
    oJournalEntry.AccountingDate = $("#AccountingDateEntry").val();
    oJournalEntry.BranchId = $("#BranchJournalEntry").val();
    oJournalEntry.Comments = $("#ObservationsJournal").val();
    oJournalEntry.CompanyId = $("#CompanyJournalEntry").val();
    oJournalEntry.Description = $("#DescriptionJournal").val();
    oJournalEntry.IndividualId = journalPayerIndividualId;
    oJournalEntry.PersonTypeId = $("#PayerTypeJournal").val();
    oJournalEntry.SalePointId = $("#SalesPointJournal").val();
    oJournalEntry.StatusId = journalStatusId;

    return oJournalEntry;
}

function SaveJournalTemporalConfirmation(message) {
    var obj = SetDataJournalEntry(2);
    $.UifDialog('confirm', { 'message': message + '?', 'title': Resources.JournalEntry }, function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                async: false,
                url: ACC_ROOT + "JournalEntry/UpdateTempJournalEntry",
                data: JSON.stringify({ "journalEntryModel": obj }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (dataJournal) {

                    if (dataJournal.success == false) {

                        $("#alertJournaly").UifAlert('show', dataJournal.result, "danger");

                    } else {
                        
                    if (dataJournal > 0) {
                        //ACTUALIZA ESTADO, FECHA Y USUARIO DE ASIENTO DIARIO - PARCIALMENTE APLICADO
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                            data: {
                                "imputationId": tempImputationId,
                                "imputationTypeId": Resources.ImputationTypeJournalEntry,
                                "sourceCode": parseInt(journalEntryCode),
                                "comments": $("#ObservationsJournal").val(),
                                "statusId": 2
                            },
                            success: function () {
                                $("#alertJournaly").UifAlert('show', Resources.TemporalSuccessfullySaved + " " + tempImputationId, "success");

                                SetDataJournalEntryEmpty();
                                CleanGlobalVariablesJournal();
                                SaveTempJournalEntryZero();
                                setDataFieldsEmptyJournalEntry();

                            }
                        });
                    }
                }
                }
            });
        }
    });
}

function JournalEntryZero() {
    if ($("#ViewBagDataId").val() == 0) {
        journalEntryCode = $("#ViewBagJournalId").val();

        //if ($("#ViewBagIsEdit").val() == 'False') {
           setDataFieldsEmptyJournalEntry();
        //}

    }
}

function SaveTempJournalEntryZero() {

    lockScreen();
    setTimeout(function () {
        //GRABA A TEMPORALES
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "JournalEntry/SaveTempJournalEntry",
            data: { "journalEntryId": 0 },
            success: function (dataJournal) {
                unlockScreen();
                //CONSULTAR SI EXISTE TEMPORAL
                if (dataJournal != null) {
                    journalEntryCode = dataJournal.Id;
                    $.ajax({
                        async: false,
                        type: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
                        data: { "imputationTypeId": Resources.ImputationTypeJournalEntry, "sourceCode": dataJournal.Id },
                        success: function (data) {
                        
                            if (data.Id == 0) {
                                $("#_JournalEntryCode").val(dataJournal.Id);

                                //GRABA A TEMPORALES
                                $.ajax({
                                    async: false,
                                    type: "POST",
                                    url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                                    data: { "imputationTypeId": Resources.ImputationTypeJournalEntry, "sourceCode": dataJournal.Id },
                                    success: function (dataImputation) {

                                        tempImputationId = dataImputation.Id;
                                        var msj = Resources.ApplicationJournalTitle + " " + tempImputationId;

                                        $("#globalTitle").text(msj);
                                    }
                                });
                            } else {
                                //YA EXITE EN TEMPORALES
                                tempImputationId = data.Id;
                            }
                        }
                    });
                }//fin dataJournal
            }
        });
    }, 500);

}

function setDataFieldsEmptyJournalEntry() {
    
    $("#SalesPointJournal").val("");
    if (isMulticompany == 1) {
        $("#CompanyJournalEntry").val("");
    }
   

    $("NameJournal").val("");
    $("#DescriptionJournal").val('');
    $("#ObservationsJournal").val('');
    journalPayerIndividualId = '';
    $("#CompanyJournalEntry").val($("#ViewBagAccountingCompanyDefault").val());
    amount = 0;

    $("#PayerTypeJournal").trigger('change');

    $("#PayerTypeJournal").val("");
    $("#TotalControl").val('');

    //carga los datos de las imputaciones ya sean para edición o cuando es nuevo
    setTimeout(function () {
        GetDebitsAndCreditsMovementTypesJournal($("#ViewBagTempImputationId").val(), amount);
    }, 500);

    SetTotalApplicationJournal();

}

function SetDataJournalEntryEmpty() {
    oJournalEntry = {
        JournalEntryItemId: 0,
        AccountingDate: null,
        BranchId: 0,
        CompanyId: 0,
        Comments: null,
        Description: null,
        IndividualId: 0,
        PersonTypeId: 0,
        SalePointId: 0,
        StatusId: 0
    };
}

function showConfirmTempCancelJournal() {

    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.JournalEntry },

    function (result) {
        if (result) {
            if (TotalDebitJournal() != 0 || TotalCreditJournal() != 0 || tempImputationId != 0) {

                oItemsToDeleteGrid.temporals = [];

                oTemporal = {
                    tempImputationId: 0,
                    imputationTypeId: 0,
                    sourceId: 0
                };

                oTemporal.tempImputationId = tempImputationId;
                oTemporal.imputationTypeId = parseInt($("#ViewBagImputationType").val());
                oTemporal.sourceId = parseInt(journalEntryCode);
                oItemsToDeleteGrid.temporals.push(oTemporal);

                lockScreen();
                setTimeout(function () {   

                    //Cuando el usuario desea cancelar un temporal el sistema lo elimina y también a sus dependientes
                     $.ajax({
                        type: 'POST',
                        url: ACC_ROOT + "TemporarySearch/DeleteTemporaryApplication",
                        data: JSON.stringify({ "temporals": oItemsToDeleteGrid }),
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {   

                            unlockScreen();
                            if (data) {
                                GetDebitsAndCreditsMovementTypesJournal(0, 0);
                                var msj = Resources.ApplicationJournalTitle + " " + tempImputationId;

                                $("#journalEntryId").val(msj);
                                $("#accountingMessage").val(Resources.ImputationMovementCancel);

                                setTimeout(function () {
                                    $("#LabelJournalEntry").hide();
                                    $("#LabelAccountingMessage").hide();
                                }, 1000);                            
                                $('#modalSuccess').UifModal('showLocal', Resources.Cancel + " " +Resources.JournalEntry);                            
                            }
                          }
                        });
                }, 500);
            }
            //Cuando el TotalControl es 0 solo blanquea los campos
            setDataFieldsEmptyJournalEntry();
        }
    });
}

function setDataFieldsMovementEmpty() {
    $("#selectAgentBranch").val("");
    $("#selectAgentSalePoint").val("");
    $("#selectAgentCompany").val("");
    $("#selectAgentNature").val("");
    $("#AgentAmount").val("");
    $("#AgentDocumentNumber").val("");
    $("#AgentName").val("");
    $("#AgentMovementDescription").val("");
    $("#selectAgentAccountingAccountConcept").val("");
    $("#selectAgentCurrency").val("");
    $("#AgentExchangeRate").val("");
}

$(document).ready(function () {

    if ($("#ViewBagImputationType").val() == Resources.ImputationTypeJournalEntry) {
        tempImputationId = $("#ViewBagTempImputationId").val();
        isMulticompany = parseInt($("#ViewBagParameterMulticompany").val());
        accountingCompanyDefault = $("#ViewBagAccountingCompanyDefault").val();
        salePointBranchUserDefault = $("#ViewBagSalePointBranchUserDefault").val();
        userId = $("#ViewBagUserId").val();
        userNick = $("#ViewBagUserNick").val();
    }

    /// Botón aceptar modal aplicación de recibo
    $("#modalSuccess").find("#AcceptSuccess").click(function () {
        $('#modalSuccess').UifModal('hide');

        if ($("#ViewBagIsEdit").val() == 'True') {
            location.href = $("#ViewBagMainTemporarySearchLink").val();
        } else {
            location.href = $("#ViewBagMainJournalEntryLink").val();
        }
    });
});

