var time;
var operationId = 0;
var branchId = 0;
var prefixId = 0;
var individualId = 0;
var policyNumber = "";
var operationType = "";
var timerItemsProcess = 0;
var timerCreditNotes = 0;

var oCreditNoteItems = {
    CreditNoteItem: []
};

var oCreditNoteItem = {
    ProcessId: 0,
    ImputationId: 0,
    CreditNoteItemId: 0,
    BranchId: 0,
    BranchName: "",
    PrefixId: 0,
    PrefixName: "",
    PolicyId: 0,
    PolicyNumber: "",
    EndorsementNumber: 0,
    InsuredId: 0,
    InsuredDocumentNumber: "",
    InsuredName: "",
    InsuredPersonTypeId: 0,
    AgentId: 0,
    AgentDocumentNumber: "",
    AgentName: "",
    AgentPersonTypeId: 0,
    PayerId: 0,
    PayerDocumentNumber: "",
    PayerName: "",
    PayerPersonTypeId: 0,
    CurrencyId: 0,
    CurrencyName: "",
    IncomeAmount: 0,
    Exchange: 0,
    Amount: 0,
    ImputationReceiptNumber: 0
};

$("#UserNameAutoCreditNote").attr("disabled", "disabled");
$("#ProcessDate").attr("disabled", "disabled");
$("#TotalDebits").attr("disabled", "disabled");
$("#TotalCredits").attr("disabled", "disabled");

$("#Apply").hide();
$("#Low").hide();
$("#Print").hide();
$("#Delete").hide();

setTimeout(function () {
    
    refreshGridAutoCreditNote();
   
}, 500);

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#selectBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#selectBranch").removeAttr("disabled");
}


///////////////////////////////////////////////
/// Autocomplete número documento asegurado ///
///////////////////////////////////////////////
$('#InsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $('#InsuredDocumentNumber').val(selectedItem.DocumentNumber);
        $('#InsuredName').val(selectedItem.Name);
    }
    else {
        $('#InsuredDocumentNumber').val("");
        $('#InsuredName').val("");
    }
});

////////////////////////////////////////////////
/// Autocomplete nombre asegurado            ///
////////////////////////////////////////////////
$('#InsuredName').on('itemSelected', function (event, selectedItem) {
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $('#InsuredDocumentNumber').val(selectedItem.DocumentNumber);
        $('#InsuredName').val(selectedItem.Name);
    }
    else {
        $('#InsuredDocumentNumber').val("");
        $('#InsuredName').val("");
    }
});


////////////////////////////////////
/// Botón Cancelar               ///
////////////////////////////////////
$('#CleanSearch').click(function () {
    $('#selectOperation').val("");
    $('#selectBranch').val("");
    $("#selectPrefix").val("");
    $("#PolicyNumber").val("");
    $("#InsuredDocumentNumber").val("");
    $("#InsuredName").val("");
    $("#existing").val("");
});

///////////////////////////////////////////////////////////////////
/// Botón generación proceso búsqueda cruce de notas de crédito ///
///////////////////////////////////////////////////////////////////
$('#Generate').click(function () {
    $("#formCreditNotes").validate();

    if ($("#formCreditNotes").valid()) {
        if ($("#PolicyNumber").val() != "") {
            if (($("#selectBranch").val() == "") || ($("#selectPrefix").val() == "")) {
                $("#alert").UifAlert('show', Resources.SelectBranchPrefix, "danger");
                return;
            }
        }

        operationType = "G";
        $('#modalSave').UifModal('showLocal', Resources.GeneratingCreditNotes);
        return true;
    }
});

   

/////////////////////////////////////////////////////////////
/// Botón aplicación cruce automático de notas de crédito ///
/////////////////////////////////////////////////////////////
$('#Apply').click(function () {
    $("#Low").hide();
    var rows = $("#tableCreditNotes").UifDataTable("getData");
    if (rows != null) {
        operationType = "A";
        $("#saveMessageTitle").text(Resources.ApplyCreditNotes);
        $("#saveMessageModal").text(Resources.ApplyCreditNotesConfirmation);
        $('#modalSave').UifModal('showLocal', Resources.ApplyCreditNotes);
        return true;
    }
});

////////////////////////////////////////////////////////
/// Selección de un registro de la tabla de procesos ///
////////////////////////////////////////////////////////
$('#tablePendingProcesses').on('rowSelected', function (event, data, position) {
    $("#alert").UifAlert('hide');
    $("#Apply").hide();
    $("#Print").hide();
    $("#Delete").hide();
  
    var finalized = data.Progress.split("/");
    if ((finalized[1].trim() == Resources.Finalized ) && (data.IsActive == "Actived")) {
            $("#Low").show();
        }
        else {
            $("#Low").hide();
        }
  
});

///////////////////////////////////////////////////////////////////
/// Búsqueda de cruce de notas de crédito por número de proceso ///
///////////////////////////////////////////////////////////////////
$('#ProcessCreditNotes').on("search", function (event, value) {
    GetProcessDetails(value);
    
});


/////////////////////////////////////////////////////////////////////////////
/// Búsqueda de cruce de notas de crédito aplicadas por número de proceso ///
/////////////////////////////////////////////////////////////////////////////
$('#AppliedProcessCreditNotes').on("search", function (event, value) {
    GetAppliedProcessDetails(value);
});



////////////////////////////////////////
/// Reporte Pendientes de Aplicación ///
////////////////////////////////////////
$("#Print").click(function () {

    $("#formCreditNotes").validate();

    if ($("#formCreditNotes").valid()) {

        if ($("#ProcessCreditNotes").val() != "") {

            lockScreen();
            setTimeout(function () {
                ShowCreditNotesByProcessNumber($("#ProcessCreditNotes").val());
            }, 300);            
        }
    }
});
        
   

        
////////////////////////////////////////////////////////////////////////////////////////////
///// Controla que solo se pueda eliminar registros pertenecientes a una misma poliza(s) ///
////////////////////////////////////////////////////////////////////////////////////////////
$('body').delegate('#tableCreditNotes tbody tr', "click", function (event, data, position) {
    
    var branch = $(this).closest("tr").children("td")[0];
    branch = $(branch).text();
    var prefix = $(this).closest("tr").children("td")[1];
    prefix = $(prefix).text();
    var policy = $(this).closest("tr").children("td")[2];
    policy = $(policy).text();
            
    var rows = $("#tableCreditNotes").UifDataTable("getData");
            
    //contar cuantas polizas de una misma póliza hay
    var countTotPol = 0;
    for (var k in rows) {
        if (branch == rows[k].Branch && prefix == rows[k].Prefix && policy == rows[k].Policy) {
            countTotPol++;
        }
    }
            
    var countSelPol = 0;      
    var rowsSelected = $("#tableCreditNotes").UifDataTable("getSelected");
    for (var x in rowsSelected) {
        if (branch == rowsSelected[x].Branch && prefix == rowsSelected[x].Prefix && policy == rowsSelected[x].Policy) {
            countSelPol++;
        }
    }

    if ((countTotPol > countSelPol) && (!countSelPol == 0)) {

        $("#alertProcess").UifAlert('show', Resources.SelectAllPolicy, "warning");
        $("#Delete").hide();
            
    }  else {
        $("#Delete").show();
        $("#alertProcess").UifAlert('hide');
    }
});
        

/////////////////////////////////////////////////
/// Clic en los tabs para ocultar los botones ///
/////////////////////////////////////////////////
$('#tabMainCreditNotesAutomatic').on('change', function (event, newly, old) {
    $("#Apply").hide();
    $("#Low").hide();
    $("#Print").hide();
    $("#Delete").hide();

    if (newly.hash == "#tabProcess" || old == "#tabProcess") {
        var pendingProcess = $("#tablePendingProcesses").UifDataTable("getSelected");
        if (pendingProcess != null) {
            if ((pendingProcess[0].Progress == Resources.Finalized) && (pendingProcess[0].IsActive == "Actived")) {
                $("#Low").show();
            }
            else {
                $("#Low").hide();
            }
        }
    }
    else if (newly.hash == "#tabCreditNotes" || old == "#tabCreditNotes") {

        var ids = $("#tableCreditNotes").UifDataTable("getData");

        if (ids.length > 0) {
            $("#Apply").show();
            $("#Print").show();
            $("#Delete").show();
        }
    }
});

//////////////////////////////////////////////////////////////////////
/// Botón dar de baja proceso cruce automático de notas de crédito ///
//////////////////////////////////////////////////////////////////////
$('#Low').click(function () {

    var process = $("#tablePendingProcesses").UifDataTable("getSelected");
    if (process != null) {
        operationType = "L";
        $("#saveMessageTitle").text(Resources.Low);
        $("#saveMessageModal").text(Resources.LowProcessConfirmation);
        $('#modalSave').UifModal('showLocal', Resources.Low);
        return true;
    }
});


/////////////////////////////////////////////////////
/// Botón eliminar temporales de notas de crédito ///
/////////////////////////////////////////////////////
$('#Delete').click(function () {
    var rows = $("#tableCreditNotes").UifDataTable("getSelected");
    if (rows != null) {
        $('#modalDeleteCredit').appendTo("body").modal('show');
        return true;
    }
    else {
        $("#alertProcess").UifAlert('show', Resources.SelectRecords, "danger");
    }
});


/////////////////////////////////////////
//  Dropdown Sucursal
/////////////////////////////////////////
$('#SelectStatusProcess').on('itemSelected', function (event, selectedItem) {
   refreshGridAutoCreditNote();
});



/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/



//////////////////////////////////////////////////////
/// Refresca la grilla de proceso notas de crédito ///
//////////////////////////////////////////////////////
function refreshGridAutoCreditNote() {

    operationId = ($('#selectOperation').val() != "") ? $('#selectOperation').val() : -1;
    branchId = ($('#selectBranch').val() != "") ? $('#selectBranch').val() : -999;
    prefixId = ($('#selectPrefix').val() != "") ? $('#selectPrefix').val() : -999;
    policyNumber = ($('#PolicyNumber').val() != "") ? $('#PolicyNumber').val() : "0";
    
    AutomaticCreditNotesRequest.GetNotesCredit($("#SelectStatusProcess").val()).done(function (response) {
        $("#tablePendingProcesses").UifDataTable({ sourceData: response.aaData });
    });

}

  /**
        * Valida si el proceso ha finalizado.
        *
        */
function validateCreditNoteProcess() {
    var process = $('#tablePendingProcesses').UifDataTable('getData');
    var count = 0;

    for (var i in process) {
        if (process[i].ProgressStatus == Resources.InProcess) {
            return false;
        }
        else {
            count++;
        }
    }
    return ((count > 0) && (process.length > 0))
}


///////////////////////////////////////////////////////////////
/// Visualiza el reporte de Pendiente de Bancos en pantalla ///
///////////////////////////////////////////////////////////////
function ShowCreditNotesByProcessNumber(processCreditNotes) {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "AutomaticCreditNotes/GetCreditNotesByProcessNumberReport",
        data: { "processCreditNotes": processCreditNotes },
        success: function () {
            showReportAutoCreditNote();
            unlockScreen();
        }
    });
}

///////////////////////////////////////////////////////////////
/// Setea los ítems seleccionados para eliminar             ///
///////////////////////////////////////////////////////////////
function SetDeleteCreditNotes() {

    oCreditNoteItems.CreditNoteItem = [];

    var rows = $("#tableCreditNotes").UifDataTable("getSelected");

    if (rows != null) {
        for (var j = 0; j < rows.length; j++) {
            oCreditNoteItem = {
                ProcessId: 0,
                ImputationId: 0,
                CreditNoteItemId: 0,
                BranchId: 0,
                BranchName: "",
                PrefixId: 0,
                PrefixName: "",
                PolicyId: 0,
                PolicyNumber: "",
                EndorsementNumber: 0,
                InsuredId: 0,
                InsuredDocumentNumber: "",
                InsuredName: "",
                InsuredPersonTypeId: 0,
                AgentId: 0,
                AgentDocumentNumber: "",
                AgentName: "",
                AgentPersonTypeId: 0,
                PayerId: 0,
                PayerDocumentNumber: "",
                PayerName: "",
                PayerPersonTypeId: 0,
                CurrencyId: 0,
                CurrencyName: "",
                IncomeAmount: 0,
                Exchange: 0,
                Amount: 0,
                ImputationReceiptNumber: 0
            };

            oCreditNoteItem.ProcessId = $("#ProcessCreditNotes").val();
            oCreditNoteItem.ImputationId = rows[j].ImputationId;
            oCreditNoteItem.CreditNoteItemId = rows[j].Id;
            oCreditNoteItem.BranchId = rows[j].BranchId;
            oCreditNoteItem.BranchName = rows[j].Branch;
            oCreditNoteItem.PrefixId = rows[j].PrefixId;
            oCreditNoteItem.PrefixName = rows[j].Prefix;
            oCreditNoteItem.PolicyId = rows[j].PolicyId;
            oCreditNoteItem.PolicyNumber = rows[j].Policy;
            oCreditNoteItem.EndorsementNumber = rows[j].Endorsement;
            oCreditNoteItem.InsuredId = 0;
            oCreditNoteItem.InsuredDocumentNumber = rows[j].Insured;
            oCreditNoteItem.InsuredName = "";
            oCreditNoteItem.InsuredPersonTypeId = rows[j].InsuredPersonType;
            oCreditNoteItem.AgentId = 0;
            oCreditNoteItem.AgentDocumentNumber = rows[j].PrincipalAgent;
            oCreditNoteItem.AgentName = "";
            oCreditNoteItem.AgentPersonTypeId = rows[j].AgentPersonType;
            oCreditNoteItem.PayerId = 0;
            oCreditNoteItem.PayerDocumentNumber = rows[j].Payer;
            oCreditNoteItem.PayerName = "";
            oCreditNoteItem.PayerPersonTypeId = rows[j].PayerPersonType;
            oCreditNoteItem.CurrencyId = rows[j].CurrencyId;
            oCreditNoteItem.CurrencyName = rows[j].Currency;
            oCreditNoteItem.IncomeAmount = rows[j].Amount;
            oCreditNoteItem.Exchange = rows[j].ExchangeRate;
            oCreditNoteItem.Amount = rows[j].LocalAmount;
            oCreditNoteItem.ImputationReceiptNumber = rows[j].ApplicationReceiptNumber;

            oCreditNoteItems.CreditNoteItem.push(oCreditNoteItem);
        }
    }

    return oCreditNoteItems;
}

////////////////////////////////////////////////////
/// Permite visualizar el reporte en formato pdf ///
////////////////////////////////////////////////////
function showReportAutoCreditNote() {
    window.open(ACC_ROOT + "AutomaticCreditNotes/ShowAutomaticCreditNotes", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

////////////////////////////////////////////////////////////////////////////
/// Busca el detalle del proceso de cruce automático de notas de crédito ///
///////////////////////////////////////////////////////////////////////////
function GetProcessDetails(processNumber) {

    $("#alertProcess").UifAlert('hide');
    $("#Apply").hide();
    $("#Low").hide();
    $("#Print").hide();
    $("#Delete").hide();

    $("#UserNameAutoCreditNote").val("");
    $("#ProcessDate").val("");
    $("#TotalDebits").val("");
    $("#TotalCredits").val("");
    $("#tableCreditNotes").dataTable().fnClearTable();

    lockScreen();  

    setTimeout(function () {
     
         $.ajax({
            type: "GET",
            url: ACC_ROOT + "AutomaticCreditNotes/GetHeaderCreditNotesByProcessNumber",
            data: { "processNumber": processNumber },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                unlockScreen();    

                if (data.success == false) {
                    $("#alertProcess").UifAlert('show', data.result, "danger");
                } else {

                    if (data.length > 0) {

                        if (data[0].UserName != "-1") {
                            $("#UserNameAutoCreditNote").val(data[0].UserName);
                            $("#ProcessDate").val(data[0].ProcessDate);
                            $("#TotalDebits").val("$ " + NumberFormat(data[0].TotalCredits, "2", ".", ","));
                            $("#TotalCredits").val("$ " + NumberFormat(data[0].TotalDebits, "2", ".", ","));

                            lockScreen();

                            var controller = ACC_ROOT + "AutomaticCreditNotes/GetCreditNotesByProcessNumber?processNumber=" + processNumber;
                            $("#tableCreditNotes").UifDataTable({ source: controller });

                            timerCreditNotes = window.setInterval(validateQuery, 2000);

                            $("#Apply").show();
                            $("#Print").show();
                            $("#Delete").show();

                        }
                        else {
                            $("#alertProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "warning");

                        }
                    } else {
                        $("#alertProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "warning");
                    }
                }
            }
        });

    }, 500);
}


function validateQuery() {
    
    if ($("#tableCreditNotes").UifDataTable("getData").length > 0 || $("#tableCreditNotes td")[0].innerHTML == "Ningún dato disponible en esta tabla") {
        clearInterval(timerCreditNotes);
        unlockScreen();
    }    
}

////////////////////////////////////////////
/// Busca las notas de crédito aplicadas  DANC///
////////////////////////////////////////////
function GetAppliedProcessDetails(processNumber) {
    $("#alertAppliedProcess").UifAlert('hide');
    $("#ApplyAutomaticCreditNote").hide();
    $("#LowAutomaticCreditNote").hide();
    $("#PrintAutomaticCreditNote").hide();
    $("#DeleteAutomaticCreditNote").hide();
    $("#PrintApplyAutomaticCreditNote").hide();

    $("#AppliedUserNameAutoCreditNote").val("");
    $("#AppliedProcessDate").val("");
    $("#AppliedTotalDebits").val("");
    $("#AppliedTotalCredits").val("");
    $("#tableAppliedCreditNotes").dataTable().fnClearTable();

    $.ajax({
        type: "GET",
        url: ACC_ROOT + "AutomaticCreditNotes/GetHeaderAppliedCreditNotesByProcessNumber",
        data: { "processNumber": processNumber },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

             if(data.success == false)
                {
                    $("#alertAppliedProcess").UifAlert('show', data.result, "danger");
                }
            else if (data != false) {

                    $("#AppliedUserNameAutoCreditNote").val(data[0].UserName);
                    $("#AppliedProcessDate").val(data[0].ProcessDate);
                    $("#AppliedTotalDebits").val("$ " + NumberFormat(data[0].TotalCredits, "2", ".", ","));
                    $("#AppliedTotalCredits").val("$ " + NumberFormat(data[0].TotalDebits, "2", ".", ","));

                    var controller = ACC_ROOT + "AutomaticCreditNotes/GetAppliedCreditNotesByProcessNumber?processNumber=" + processNumber;
                    $("#tableAppliedCreditNotes").UifDataTable({ source: controller });
                    $("#PrintApplyAutomaticCreditNote").show();
            }       
         
        }
    });
}




///////////////////////////////////////////////////////////////
/// Setea los ítems seleccionados para aplicar              ///
///////////////////////////////////////////////////////////////
function SetDataCreditNotes() {
    
    var rows = $("#tableCreditNotes").UifDataTable("getData");

        if (rows != null) {
            for (var j = 0; j < 1 /*rows.length*/; j++) {
                oCreditNoteItem = {
                    ProcessId: 0,
                    ImputationId: 0,
                    CreditNoteItemId: 0,
                    BranchId: 0,
                    BranchName: "",
                    PrefixId: 0,
                    PrefixName: "",
                    PolicyId: 0,
                    PolicyNumber: "",
                    EndorsementNumber: 0,
                    InsuredId: 0,
                    InsuredDocumentNumber: "",
                    InsuredName: "",
                    InsuredPersonTypeId: 0,
                    AgentId: 0,
                    AgentDocumentNumber: "",
                    AgentName: "",
                    AgentPersonTypeId: 0,
                    PayerId: 0,
                    PayerDocumentNumber: "",
                    PayerName: "",
                    PayerPersonTypeId: 0,
                    CurrencyId: 0,
                    CurrencyName: "",
                    IncomeAmount: 0,
                    Exchange: 0,
                    Amount: 0,
                    ImputationReceiptNumber: 0
                };

                oCreditNoteItem.ProcessId = $("#ProcessCreditNotes").val();
                oCreditNoteItem.ImputationId = rows[j].ImputationId;
                oCreditNoteItem.CreditNoteItemId = rows[j].Id;
                oCreditNoteItem.BranchId = rows[j].BranchId;
                oCreditNoteItem.BranchName = rows[j].Branch;
                oCreditNoteItem.PrefixId = rows[j].PrefixId;
                oCreditNoteItem.PrefixName = rows[j].Prefix;
                oCreditNoteItem.PolicyId = rows[j].PolicyId;
                oCreditNoteItem.PolicyNumber = rows[j].Policy;
                oCreditNoteItem.EndorsementNumber = rows[j].Endorsement;
                oCreditNoteItem.InsuredId = 0;
                oCreditNoteItem.InsuredDocumentNumber = rows[j].Insured;
                oCreditNoteItem.InsuredName = "";
                oCreditNoteItem.InsuredPersonTypeId = rows[j].InsuredPersonType;
                oCreditNoteItem.AgentId = 0;
                oCreditNoteItem.AgentDocumentNumber = rows[j].PrincipalAgent;
                oCreditNoteItem.AgentName = "";
                oCreditNoteItem.AgentPersonTypeId = rows[j].AgentPersonType;
                oCreditNoteItem.PayerId = 0;
                oCreditNoteItem.PayerDocumentNumber = rows[j].Payer;
                oCreditNoteItem.PayerName = "";
                oCreditNoteItem.PayerPersonTypeId = rows[j].PayerPersonType;
                oCreditNoteItem.CurrencyId = rows[j].CurrencyId;
                oCreditNoteItem.CurrencyName = rows[j].Currency;
                oCreditNoteItem.IncomeAmount = rows[j].Amount;
                oCreditNoteItem.Exchange = rows[j].ExchangeRate;
                oCreditNoteItem.Amount = rows[j].LocalAmount;
                oCreditNoteItem.ImputationReceiptNumber = rows[j].ApplicationReceiptNumber;

                oCreditNoteItems.CreditNoteItem.push(oCreditNoteItem);
            }
    }

    return oCreditNoteItems;
}

//////////////////////////////////////////////
/// Se limpia los campos luego de aplicar  ///
//////////////////////////////////////////////
function SetEmptyApplication()
{
    $("#ProcessCreditNotes").val("");
    $("#UserNameAutoCreditNote").val("");
    $("#ProcessDate").val("");
    $("#TotalDebits").val("");
    $("#TotalCredits").val("");
}


//BOTONES DE MODALES
$(document).ready(function () {

    $("#GenerateModal").on('click', function () {
        
        $('#modalSave').UifModal('hide');

        // Generación
        if (operationType == "G") {
            operationId = ($('#selectOperation').val() != "") ? $('#selectOperation').val() : -1;
            branchId = ($('#selectBranch').val() != "") ? $('#selectBranch').val() : -999;
            prefixId = ($('#selectPrefix').val() != "") ? $('#selectPrefix').val() : -999;
            policyNumber = ($('#PolicyNumber').val() != "") ? $('#PolicyNumber').val() : "0";

            if ($('#selectOperation').val() != "") {
                $('div#container').show();

                $("#Generate").attr("disabled", "disabled");

                lockScreen();

                setTimeout(function () {
                 
                    $.ajax({
                        url: ACC_ROOT + "AutomaticCreditNotes/GenerateAutomaticCreditNotes",
                        data: {
                            "operationTypeId": operationId, "branchId": branchId, "prefixId": prefixId,
                            "individualId": individualId, "policyNumber": policyNumber,
                            "currencyId": $('#SelectCreditNoteCurrency').val()
                        },
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            unlockScreen();
                            if (data[0].ProcessNumber > 0) {
                                $("#alert").UifAlert('show', Resources.MessageProcessAutomaticCreditNotes, "success");
                                refreshGridAutoCreditNote();
                            }
                            else {
                                $("#alert").UifAlert('show', data[0].MessageError, "danger");
                            }
                            $('div#container').hide();
                            $("#Generate").removeAttr("disabled");

                            setTimeout(function () {
                                $("#alert").UifAlert('hide');
                            }, 5000);
                        }
                    });
                }, 500);
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectOperationType, "danger");
            }
        }

        // Baja
        if (operationType == "L") {
            var process = $("#tablePendingProcesses").UifDataTable("getSelected");
            if (process != null) {
                $("#Low").attr("disabled", "disabled");

                lockScreen();
                setTimeout(function () {                         
                    $.ajax({
                    url: ACC_ROOT + "AutomaticCreditNotes/LowAutomaticCreditNotes",
                    data: { "processId": process[0].Id },
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        unlockScreen();
                        if (data == true) {

                            $("#alert").UifAlert('show', Resources.MessageLowProcessAutomaticCreditNotes, "success");
                            refreshGridAutoCreditNote();

                        }else if (data.success == false) {

                            $("#alert").UifAlert('show', data.result, "danger");
                        } 
                        
                        $('div#container').hide();
                        $("#Low").removeAttr("disabled");

                        setTimeout(function () {
                            $("#alert").UifAlert('hide');
                        }, 5000);

                        $("#Low").hide();
                    }
                  });

                }, 500);
            }
        }

        // Aplicar
        if (operationType == "A") {

            $("#Apply").attr("disabled", "disabled");

            lockScreen();
            
            setTimeout(function () {            
            
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "AutomaticCreditNotes/ApplyAutomaticCreditNotes",
                        data: {
                           "itemsToAppliedModel": SetDataCreditNotes(),
                            "totalDebits":  ClearFormatCurrency($("#TotalDebits").val().replace("", ",")).trim(), 
                            "totalCredits": ClearFormatCurrency($("#TotalCredits").val().replace("", ",")).trim() 
                        },
                        success: function (data) {

                            unlockScreen();

                            if (data.success == false)
                           {
                                 $("#alertProcess").UifAlert('show', data.result, "danger");
                           }
                           else if (data[0].JournalEntryId > 0) {


                                if (data[0].ResultGeneralLedger == Resources.AccountingIntegrationUnbalanceEntry || data[0].ResultGeneralLedger == Resources.EntryRecordingError) {

                                    $("#alertProcess").UifAlert('show', Resources.ApplySuccessfully + ", " + Resources.TransactionNumber + ': ' + data[0].JournalEntryId 
                                        + ", " + data[0].ResultGeneralLedger,  "danger");
                                } else {


                                    //Refresca grilla de cabecera del proceso
                                    refreshGridAutoCreditNote();
                                    
                                    $("#tableCreditNotes").UifDataTable('clear');

                                    $("#alertProcess").UifAlert('show', Resources.ApplySuccessfully + ", " + Resources.TransactionNumber + ': ' + data[0].JournalEntryId 
                                        + ", " + data[0].ResultGeneralLedger, "success");
                                }

                                SetEmptyApplication();
                            }                      

                            $('div#container').hide();
                            $("#Apply").removeAttr("disabled");                          
                        }
                });

            }, 400);            
        }
    });


    $("#DeleteModalCreditNotes").on('click', function () {
        $('#modalDeleteCredit').modal('hide');

        $("#Delete").attr("disabled", "disabled");

        lockScreen();

        setTimeout(function () {    

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "AutomaticCreditNotes/DeleteAutomaticCreditNotes",
                data: { "itemsToAppliedModel": SetDeleteCreditNotes() },
                success: function (data) {
                                        
                    $.unblockUI();

                    if (data[0].ProcessNumber == 0) {                    

                        GetProcessDetails($("#ProcessCreditNotes").val());
                        $("#alertProcess").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    }
                    else {
                        $("#alertProcess").UifAlert('show', data[0].MessageError, "danger");
                    }
                    $('div#container').hide();
                    $("#Delete").removeAttr("disabled");

                    setTimeout(function () {
                        $("#alertProcess").UifAlert('hide');
                    }, 5000);
                }
            });

        }, 1500);
    });
});