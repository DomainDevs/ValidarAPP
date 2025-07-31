/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
var timeCreditNote = window.setInterval(validateCompanyCreditNote, 800);
var beneficiaryIndividualId = -1;

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

/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

//Fecha contable
$("#AccountingDate").val($("#ViewBagDateAccounting").val());


//Se ocultan campos de busqueda de autocompletes
$(window).load(function () {
    cleanAutocompletesCreditNote('SearchSuppliers');
    cleanAutocompletesCreditNote('SearchInsured');
    cleanAutocompletesCreditNote('SearchCoinsurance');
    cleanAutocompletesCreditNote('SearchPerson');
    cleanAutocompletesCreditNote('SearchAgent');
    cleanAutocompletesCreditNote('SearchEmployee');
    cleanAutocompletesCreditNote('SearchReinsurer');

    //Cargando los eventos
    loadAutocompleteEventCreditNote('SearchSuppliers');
    loadAutocompleteEventCreditNote('SearchInsured');
    loadAutocompleteEventCreditNote('SearchCoinsurance');
    loadAutocompleteEventCreditNote('SearchPerson');
    loadAutocompleteEventCreditNote('SearchAgent');
    loadAutocompleteEventCreditNote('SearchEmployee');
    loadAutocompleteEventCreditNote('SearchReinsurer');

    setTimeout(function () {
        setBranchJournalEntry();
    }, 2500);
});


$("#_cancelCreditNotes").click(function () {
    cleanFieldsCreditNote();
});


$("#_saveCreditNotes").click(function () {

    $("#frmCreditNotes").validate();

    if ($("#frmCreditNotes").valid()) {
        $.ajax({
            url: ACC_ROOT + "JournalEntry/ValidateCreditNoteGeneration",
            data: JSON.stringify({
                "policyDocumentNumber": $("#PolicyNumber").val(), "branchId": $("#BranchJournal").val(),
                "prefixId": $("#PrefixJournal").val()
            }),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data) {
                    $.ajax({
                        url: ACC_ROOT + "JournalEntry/GenerateCreditNoteRequest",
                        data: JSON.stringify({
                            "journalEntryModel": setDataJournalEntry(),
                            "policyDocumentNumber": $("#PolicyNumber").val(),
                            "prefixId": $("#PrefixJournal").val()
                        }),
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.success == false) {
                                $("#alert").UifAlert('show', data.result, "danger");

                            } else {
                                if (data) {
                                    showReportCreditNote($("#PolicyNumber").val(), $("#BranchJournal").val(),
                                                          $("#PrefixJournal").val(), beneficiaryIndividualId);
                                    setDataJournalEntryEmpty();
                                    cleanFieldsCreditNote();
                                    $("#alert").UifAlert('show', Resources.MsgSuccessProcess, "success");
                                }
                                else {
                                    $("#alert").UifAlert('show', Resources.ThereIsNoConditionsForProcess, "warning");
                                }
                            }
                        }
                    });
                } else {
                    $("#alert").UifAlert('show', Resources.PolicyNotExists, "warning");
                }
            }
        });

    }
    else {
        $("#textMessage").text(Resources.RequiredFieldsMissing);

    }
});

/////////////////////////////////////
// Autocomplete nombre             //
/////////////////////////////////////
$("#BeneficiaryTypeSelect").on('itemSelected', function (event, selectedItem) {

    var enableAjax = 0;

    cleanAutocompletesCreditNote('SearchSuppliers');
    cleanAutocompletesCreditNote('SearchInsured');
    cleanAutocompletesCreditNote('SearchCoinsurance');
    cleanAutocompletesCreditNote('SearchPerson');
    cleanAutocompletesCreditNote('SearchAgent');
    cleanAutocompletesCreditNote('SearchEmployee');
    cleanAutocompletesCreditNote('SearchReinsurer');


    if (selectedItem != null) {
        $("#BeneficiaryDocumentNumber").hide();
        $("#BeneficiaryName").hide();

        if (selectedItem.Id == $("#ViewBagSupplierCode").val()) { // 1 Proveedor  

            $('#SearchSuppliersByDocumentNumber').parent().parent().show();
            $("#SearchSuppliersByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagInsuredCode").val()) { // 2 Asegurado  

            $("#SearchInsuredByDocumentNumber").parent().parent().show();
            enableAjax = 1;

            $("#SearchInsuredByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagCoinsurerCode").val()) { // 7 Coaseguradora  

            $("#SearchCoinsuranceByDocumentNumber").parent().parent().show();
            $("#SearchCoinsuranceByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagThirdPartyCode").val()) { //8 Tercero   

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagProducerCode").val()) { // 10 Productor  

            $("#SearchAgentByDocumentNumber").parent().parent().show();
            $("#SearchAgentByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagEmployeeCode").val()) { // 11 Empleado  

            $("#SearchEmployeeByDocumentNumber").parent().parent().show();
            enableAjax = 1;

            $("#SearchEmployeeByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagReinsurerCode").val()) { //13 Reaseguradora  

            $("#SearchReinsurerByDocumentNumber").parent().parent().show();
            $("#SearchReinsurerByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagTradeConsultant").val()) { // 14 Asesor comercial   

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagAgentCode").val()) { //15 Agente  

            $("#SearchAgentByDocumentNumber").parent().parent().show();
            $("#SearchAgentByName").parent().parent().show();

        } else if (selectedItem.Id == '') { //15 Agente 

            cleanAutocompletesCreditNote('SearchSuppliers');
            cleanAutocompletesCreditNote('SearchInsured');
            cleanAutocompletesCreditNote('SearchCoinsurance');
            cleanAutocompletesCreditNote('SearchPerson');
            cleanAutocompletesCreditNote('SearchAgent');
            cleanAutocompletesCreditNote('SearchEmployee');
            cleanAutocompletesCreditNote('SearchReinsurer');

        } else {

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();
        }

        if (enableAjax > 0) {
        }

    } else {

        cleanAutocompletesCreditNote('SearchSuppliers');
        cleanAutocompletesCreditNote('SearchInsured');
        cleanAutocompletesCreditNote('SearchCoinsurance');
        cleanAutocompletesCreditNote('SearchPerson');
        cleanAutocompletesCreditNote('SearchAgent');
        cleanAutocompletesCreditNote('SearchEmployee');
        cleanAutocompletesCreditNote('SearchReinsurer');
    }
});

//AUTOCOMPLETE ------------------------------------------------------------------------------

$("#BranchJournal").on('itemSelected', function (event, selectedItem) {
    if ($("#BranchJournal").val()) {
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#SalePointJournal").UifSelect({ source: controller });
        }
    }
});


/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                    FUNCIONES 
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/


/////////////////////////////////////////////////////////////////////////////////////////////////
// loadAutocompleteEventCreditNote  - carga los autocompletes de nombre y número de documento  //
/////////////////////////////////////////////////////////////////////////////////////////////////
function loadAutocompleteEventCreditNote(identifier) {

    var selectedDeptor;
    $('#' + identifier + 'ByDocumentNumber').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesCreditNote(identifier, selectedItem);
        selectedDeptor = selectedItem;
    });

    $('#' + identifier + 'ByName').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesCreditNote(identifier, selectedItem);
        selectedDeptor = selectedItem;
    });

    $('#' + identifier + 'ByDocumentNumber').on('keyup', function (event, selectedItem) {
        if (selectedItem != undefined) {
            $("#alert").UifAlert('hide');
            selectedItem = selectedDeptor;
            fillAutocompletesCreditNote(identifier, selectedItem);
        }
    });

    $('#' + identifier + 'ByDocumentNumber').on('blur', function (event, selectedItem) {
        if (selectedDeptor != undefined) {
            selectedItem = selectedDeptor;
            $("#alert").UifAlert('hide');
            fillAutocompletesCreditNote(identifier, selectedItem);
        } else {
            $('#' + identifier + 'ByDocumentNumber').val("");
            $('#' + identifier + 'ByName').val("");
        
        }
    });

    //Eventos keyup y blur para evitar borrado al dar tab en nombre de Abonador
    $('#' + identifier + 'ByName').on('keyup', function (event, selectedItem) {
        
        if (selectedItem != undefined) {
            selectedItem = selectedDeptor;
            $("#alert").UifAlert('hide');

            if (selectedItem.Id > -1) {
                fillAutocompletesCreditNote(identifier, selectedItem);
            }            
        }
    });

    $('#' + identifier + 'ByName').on('blur', function (event, selectedItem) {
        if (selectedDeptor != undefined) {
            selectedItem = selectedDeptor;
            $("#alert").UifAlert('hide');
            fillAutocompletesCreditNote(identifier, selectedItem);
        }

    });
}

/////////////////////////////////////////////////////////////////////////////////////
// cleanAutocompletesCreditNote - limpia los autocompletes de nombre y número de documento   //
////////////////////////////////////////////////////////////////////////////////////
function cleanAutocompletesCreditNote(identifier) {

    $('#' + identifier + 'ByDocumentNumber').val("");
    $('#' + identifier + 'ByName').val("");
    beneficiaryIndividualId = 0;
    $("#BeneficiaryDocumentNumber").val("");
    $("#BeneficiaryName").val("");
    $("#PaymentOrderPayableTo").val("");
    beneficiaryIndividualId = 0;
    $("#btnPaymentOrderBankAccount").hide();

    $('#' + identifier + 'ByDocumentNumber').parent().parent().hide();
    $('#' + identifier + 'ByName').parent().parent().hide();
    $("#BeneficiaryDocumentNumber").show();
    $("#BeneficiaryName").show();
}

function fillAutocompletesCreditNote(identifier, selectedItem) {

  

        //setTimeout(function () {
            $("#alert").UifAlert('hide');
            $('#' + identifier + 'ByDocumentNumber').val(selectedItem.DocumentNumber);
            $('#' + identifier + 'ByName').val(selectedItem.Name);
            if (selectedItem.Id != undefined) {
                beneficiaryIndividualId = selectedItem.Id;
            } else if (selectedItem.AgentId != undefined) {
                beneficiaryIndividualId = selectedItem.AgentId;
            } else {
                beneficiaryIndividualId = selectedItem.CoinsuranceIndividualId;
            }

        
        //}, 2500);

    if (selectedItem.Id > -1) {
        $("#BeneficiaryDocumentNumber").val(selectedItem.DocumentNumber);
        $("#BeneficiaryName").val(selectedItem.Name);
        $("#PaymentOrderPayableTo").val(selectedItem.Name);
    } else {
        $('#' + identifier + 'ByDocumentNumber').val("");
        $('#' + identifier + 'ByName').val("");

    } 
}


function setBranchJournalEntry() {

    if ($("#ViewBagBranchUserDefault").val() > 0) {
        $("#BranchJournal").val($("#ViewBagBranchUserDefault").val());
        if ($("#BranchJournal").val()) {

            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + $("#ViewBagBranchUserDefault").val();
            $("#SalePointJournal").UifSelect({ source: controller });

            //Setea el punto de venta de default
            setTimeout(function () {
                $("#SalePointJournal").val($("#ViewBagSalePointBranchUserDefault").val());
            }, 500);
        }
    }
    else {
        $("#BranchJournal").val("");
    }
}



function cleanFieldsCreditNote() {
    $("#BranchJournal").val($("#ViewBagBranchUserDefault").val());
    $("#SalePointJournal").val($("#ViewBagSalePointBranchUserDefault").val());
    $("#CompanyJournal").val($("#ViewBagAccountingCompanyDefault").val());
    $("#BeneficiaryTypeSelect").val("");
    $("#BeneficiaryDocumentNumber").val("");
    $("#BeneficiaryName").val("");
    $("#_DescriptionJournal").val("");
    $("#_ObservationsJournal").val("");
    $("#PrefixJournal").val(-1);
    $("#PolicyNumber").val("");
    beneficiaryIndividualId = -1;

    $("#BeneficiaryTypeSelect").trigger('itemSelected');
}


function setDataJournalEntry() {

    oJournalEntry.JournalEntryItemId = 0; //autonumérico
    oJournalEntry.AccountingDate = $("#AccountingDate").val();
    oJournalEntry.BranchId = $("#BranchJournal").val();
    oJournalEntry.CompanyId = $("#CompanyJournal").val();
    oJournalEntry.Comments = $("#_ObservationsJournal").val();
    oJournalEntry.Description = $("#_DescriptionJournal").val();
    oJournalEntry.IndividualId = beneficiaryIndividualId;
    oJournalEntry.PersonTypeId = $("#BeneficiaryTypeSelect").val();
    oJournalEntry.SalePointId = $("#SalePointJournal").val();
    oJournalEntry.StatusId = 3; //el JournalEntry pasa a reales en el servicio.

    return oJournalEntry;
}

function setDataJournalEntryEmpty() {
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


//funcion que muestra reporte en pantalla
function showReportCreditNote(policyDocumentNumber, branchId, prefixId, payerId) {
    window.open(ACC_ROOT + "Report/ShowCreditNoteReport?policyDocumentNumber="
                + policyDocumentNumber + "&branchId=" + branchId + "&prefixId=" + prefixId + "&payerId=" + payerId,
                'mywindow', 'fullscreen=yes, scrollbars=auto');
}


//Valida y setea combo de Compañía
function validateCompanyCreditNote() {

    if ($("#CompanyJournal").val() != "" && $("#CompanyJournal").val() != null) {

        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#CompanyJournal").attr("disabled", true);
        }
        else {
            $("#CompanyJournal").attr("disabled", false);
        }
        clearInterval(timeCreditNote);
    }
}