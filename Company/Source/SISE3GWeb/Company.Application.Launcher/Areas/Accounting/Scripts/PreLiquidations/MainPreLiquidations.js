/*------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*														DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS														*/
/*------------------------------------------------------------------------------------------------------------------------------------------------------*/
var timePreliquidation = window.setInterval(validateCompanyPreliquidation, 1000);
var timerLoadPreliquidation = 0;
var IsNew = true;
var documentNumberText;
var nameText;
var tempPreLiquidationId = 0;
var amount = 0;
var tempImputationId = 0;
var individualIdPreLiquidation = 0;
var typeImputationId = 3;
var applyCollecId = $("#ViewBagApplyCollecId").val();
var applyAccountingDate = $("#ViewBagDateAccounting").val();
//flag para saber si es búsqueda de recibos.
var IsMainSearchBills = 0;
//flag para saber si es búsqueda de preliquidaciones.
var IsMainSearchPreliquidations = 1;
//flag para saber si es ingreso de caja.

var IsCheckPremiumPay = 0;
//flag para saber si se realiza pago saldo de primas.

var IsBilling = 0;
var accountingDate = "";
var AccountingCompanyDefault = "";
var BranchUserDefault = "";
var SalePointBranchUserDefault = "";

var oPreLiquidation = {
    Id: 0,
    BranchId: 0,
    CompanyId: 0,
    Description: "",
    ImputationId: 0,
    IsTemporal: 0,
    IndividualId: 0,
    PersonTypeId: 0,
    RegisterDate: null,
    SalePointId: 0,
    StatusId: 1,
    BranchDescription: "",
    CompanyDescription: "",
    PayerDocumentNumber: "",
    PayerName: "",
    TempImputationId: 0
};

var oMovementSumary = {
    MovementSumary: []
};

var oMovementSumaryDetails = {
    Id: 0,
    Description: "",
    Debit: 0,
    Credit: 0
};


setTimeout(function () {
    accountingDate = $("#ViewBagDateAccounting").val();

    if (accountingDate != undefined) {
        $("#GenerationDate").val(accountingDate.split(" ")[0]);
    }
    AccountingCompanyDefault = $("#ViewBagAccountingCompanyDefault").val();
    BranchUserDefault = $("#ViewBagBranchUserDefault").val();
    SalePointBranchUserDefault = $("#ViewBagSalePointBranchUserDefault").val();

    //control para que no duplique la carga en asiento diario
    if ($("#ViewBagImputationType").val() == typeImputationId) {
        GetDebitsAndCreditsMovementTypesPreLiquidation($("#ViewBagTempImputationId").val(), 0);
    }
}, 1000);


if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchGenerationDrop").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchGenerationDrop").removeAttr("disabled");
}


setTimeout(function () {

    if ($("#ViewBagIsEdition").val() == "1") {

        lockScreen();
        timerLoadPreliquidation = window.setInterval(loadAllFieldsPreliquidation, 2500);
    }
    else {
        loadPreliquidation(true);
        setTimeout(function () {
            setSalesPointPreLiquidation();
        }, 700);
    }
}, 700);

$(window).load(function () {

    cleanAutocompletesPreLiquidation('SearchSuppliers');
    cleanAutocompletesPreLiquidation('SearchInsured');
    cleanAutocompletesPreLiquidation('SearchCoinsurance');
    cleanAutocompletesPreLiquidation('SearchPerson');
    cleanAutocompletesPreLiquidation('SearchAgent');
    cleanAutocompletesPreLiquidation('SearchEmployee');
    cleanAutocompletesPreLiquidation('SearchReinsurer');

    //Cargando los eventos
    loadAutocompleteEventPreLiquidation('SearchSuppliers');
    loadAutocompleteEventPreLiquidation('SearchInsured');
    loadAutocompleteEventPreLiquidation('SearchCoinsurance');
    loadAutocompleteEventPreLiquidation('SearchPerson');
    loadAutocompleteEventPreLiquidation('SearchAgent');
    loadAutocompleteEventPreLiquidation('SearchEmployee');
    loadAutocompleteEventPreLiquidation('SearchReinsurer');

    setTimeout(function () {
        if ($("#ViewBagPointSalesPreliquidation").val() == 'false') {
            $("#SalesPointDrop").attr("disabled", "disabled");
        }
    }, 2500);
});


//Verifica si están llenados todos los campos
function loadAllFieldsPreliquidation() {

    loadPreliquidation(false);
    setTimeout(function () {

        if (($("#BranchGenerationDrop").val() != "" && $("#BranchGenerationDrop").val() != null) &&
            ($("#CompanyGenerationDrop").val() != "" && $("#CompanyGenerationDrop").val() != null) &&
            ($("#GenerationDate").val() != "" && $("#GenerationDate").val() != null) &&
            ($("#PayerTypeDrop").val() != "" && $("#PayerTypeDrop").val() != null) &&
            ($("#BeneficiaryDocumentNumber").val() != "" && $("#BeneficiaryDocumentNumber").val() != null) &&
            ($("#SalesPointDrop").val() != "" && $("#SalesPointDrop").val() != null) &&
            ($("#BeneficiaryName").val() != "" && $("#BeneficiaryName").val() != null) //&&
            ) {
                clearInterval(timerLoadPreliquidation);
                unlockScreen();
                SetTotalApplicationPreLiquidation();
        }
    }, 1000);
}



/*----------------------------------------------------------------------------------------------------------------------*/
/*														A C C I O N E S 						                        */
/*----------------------------------------------------------------------------------------------------------------------*/


//BOTON ACEPTAR
$("#PreLiquidationAccept").click(function () {
    
    $("#alertPreliquidation").UifAlert('hide');
    if ($("#PreLiquidationForm").valid() && individualIdPreLiquidation > 0) {

        documentNumberText = $("#ViewBagDocumentNumber").val();
        nameText = $("#ViewBagName").val();

        if (parseFloat(ClearFormatCurrency($("#TotalControl").val().replace("", ","))) >= 0) {

            if ($("#ViewBagIsEdition").val() == "1") {
                
                //GRABA REAL EDITADO
                if ($("#ViewBagIsPreliquidation").val() == "1") {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "PreLiquidations/UpdatePreLiquidation",
                        data: { "preLiquidationModel": setDataPreLiquidation(parseInt($("#ViewBagPreliquidationId").val())) },
                        success: function (data) {
            
                            var msg = Resources.PreLiquidationUpdate + " " + data.Id ;
                            confirmDialogReturn(msg, Resources.Preliquidation, $("#ViewBagPreliquidationId").val(), $("#BranchGenerationDrop").val());
                            cleanFieldsPreLiquidation();
                        }
                    });
                }
                else if ($("#ViewBagIsPreliquidation").val() == "2")
                {
                    //GRABA TEMPORAL EDITADO REGRESA A LA BÚSQUEDA DE TEMPORALES
                    confirmDialogAccept($("#BranchGenerationDrop").val(), tempImputationId);
                }
            } else {
             
                //GRABA TEMPORAL NUEVO
                confirmDialogAccept($("#BranchGenerationDrop").val(), tempImputationId);
            }

        } else {
            //GRABA REAL NUEVO
            convertTempPreLiquidationToPreLiquidation(tempPreLiquidationId, tempImputationId, Resources.ImputationType, true);

        }
    } else {
        $("#alertPreliquidation").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
    }
});


function confirmDialogReturn (messageIn, title, preliquidationId, branchId ) {
    $.UifDialog('confirm', { 'message': messageIn, 'title': title },
    function (result) {
        if (result) {
            setTimeout(function () {
                if ($("#ViewBagIsPreliquidation").val() == "1") {
                       location.href = $("#ViewBagLoadPreliquidationSearchLink").val()+ "?branchId=" + branchId + "&preliquidationId=" + preliquidationId;
                }
            }, 1000);
        }  
    });
};


//BOTÓN CANCELAR
$("#PreLiquidationCancel").click(function () {
    confirmDialogCancel(Resources.PreLiquidationCancelation, Resources.MenuPreLiquidation, "");
});


$('#BranchGenerationDrop').on('itemSelected', function (event, selectedItem) {

    $("#alertPreliquidation").UifAlert('hide');
    if ($("#ViewBagPointSalesPreliquidation").val() == 'true') {
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#SalesPointDrop").UifSelect({ source: controller });
        }
    }
});

// Autocomplete nombre
$("#PayerTypeDrop").on('itemSelected', function (event, selectedItem) {

    cleanAutocompletesPreLiquidation('SearchSuppliers');
    cleanAutocompletesPreLiquidation('SearchInsured');
    cleanAutocompletesPreLiquidation('SearchCoinsurance');
    cleanAutocompletesPreLiquidation('SearchPerson');
    cleanAutocompletesPreLiquidation('SearchAgent');
    cleanAutocompletesPreLiquidation('SearchEmployee');
    cleanAutocompletesPreLiquidation('SearchReinsurer');

    if (selectedItem != null) {
        $("#BeneficiaryDocumentNumberData").hide();
        $("#BeneficiaryNameData").hide();
        // 1 Proveedor  
        if (selectedItem.Id == $("#ViewBagSupplierCode").val()) {

            $("#SearchSuppliersByDocumentNumber").parent().parent().show();
            $("#SearchSuppliersByName").parent().parent().show();

            // 2 Asegurado 
        } else if (selectedItem.Id == $("#ViewBagInsuredCode").val()) {

            $("#SearchInsuredByDocumentNumber").parent().parent().show();
            $("#SearchInsuredByName").parent().parent().show();

            // 7 Coaseguradora  
        } else if (selectedItem.Id == $("#ViewBagCoinsurerCode").val()) {

            $("#SearchCoinsuranceByDocumentNumber").parent().parent().show();
            $("#SearchCoinsuranceByName").parent().parent().show();

            //8 Tercero 
        } else if (selectedItem.Id == $("#ViewBagThirdPartyCode").val()) {

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();

            // 10 Productor  
        } else if (selectedItem.Id == $("#ViewBagProducerCode").val()) {

            $("#SearchAgentByDocumentNumber").parent().parent().show();
            $("#SearchAgentByName").parent().parent().show();

            // 11 Empleado  
        } else if (selectedItem.Id == $("#ViewBagEmployeeCode").val()) {

            $("#SearchEmployeeByDocumentNumber").parent().parent().show();
            $("#SearchEmployeeByName").parent().parent().show();

            //13 Reaseguradora  
        } else if (selectedItem.Id == $("#ViewBagReinsurerCode").val()) {
            
                $("#SearchReinsurerByDocumentNumber").prop('disabled', true);
                $("#SearchReinsurerByName").prop('disabled', true);
            
            // 14 Asesor comercial 
        } else if (selectedItem.Id == $("#ViewBagTradeConsultant").val()) {

            //$("#SearchPersonByDocumentNumber").parent().parent().show();
            //$("#SearchPersonByName").parent().parent().show();
            $("#SearchReinsurerByDocumentNumber").prop('disabled', true);
            $("#SearchReinsurerByName").prop('disabled', true);

        } else if (selectedItem.Id == $("#ViewBagAgentCode").val()) { //15 Agente 

            $("#SearchAgentByDocumentNumber").parent().parent().show();
            $("#SearchAgentByName").parent().parent().show();

            //15 Agente 
        } else if (selectedItem.Id == '') {

            cleanAutocompletesPreLiquidation('SearchSuppliers');
            cleanAutocompletesPreLiquidation('SearchInsured');
            cleanAutocompletesPreLiquidation('SearchCoinsurance');
            cleanAutocompletesPreLiquidation('SearchPerson');
            cleanAutocompletesPreLiquidation('SearchAgent');
            cleanAutocompletesPreLiquidation('SearchEmployee');
            cleanAutocompletesPreLiquidation('SearchReinsurer');

        } else {

                $("#SearchReinsurerByDocumentNumber").prop('disabled', true);
                $("#SearchReinsurerByName").prop('disabled', true);
        }

    } else {
        $("#BeneficiaryDocumentNumberData").show();
        $("#BeneficiaryNameData").show();
        cleanAutocompletesPreLiquidation('SearchSuppliers');
        cleanAutocompletesPreLiquidation('SearchInsured');
        cleanAutocompletesPreLiquidation('SearchCoinsurance');
        cleanAutocompletesPreLiquidation('SearchPerson');
        cleanAutocompletesPreLiquidation('SearchAgent');
        cleanAutocompletesPreLiquidation('SearchEmployee');
        cleanAutocompletesPreLiquidation('SearchReinsurer');
    }
});


/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
//Muestra un diálogo de Si o No
function confirmDialogCancel (messageIn, title, nameDialog) {
    $.UifDialog('confirm', { 'message': messageIn, 'title': title },
    function (result) {
        if (result) {

            setDataPreLiquidationEmpty();
            cleanFieldsPreLiquidation();
        }
    });
};


//Convierte preliquidacion temporal a real y llama al reporte
function convertTempPreLiquidationToPreLiquidation(tempPreLiquidationId, tempImputationId,
                                                   imputationTypeId, isNew) {

    //Viene de la busqueda de reales ViewBag.IsPreliquidation == "1"
    if ($("#ViewBagIsPreliquidation").val() == "1") {

        lockScreen();
        setTimeout(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PreLiquidations/UpdatePreLiquidation",
                data: { "preLiquidationModel": setDataPreLiquidation(tempPreLiquidationId) },
                success: function (data) {
                    unlockScreen();
                    var msg = Resources.PreLiquidationSave + " " + data.Id + " " + Resources.PrintReportQuestion;
                    loadPreLiquidationReport(data.Id, isNew, msg );
                    cleanFieldsPreLiquidation();
                }
            });
        }, 500);
    }
        //Viene de la busqueda de temporales ViewBag.IsPreliquidation" == "2"
    else {
        ConvertTempPreLiquidationToPreLiquidation();
    }

}



function ConvertTempPreLiquidationToPreLiquidation() {

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "PreLiquidations/UpdateTempPreLiquidation",
        data: { "preLiquidationModel": setDataPreLiquidation(tempPreLiquidationId) },
        success: function (dataTemp) {
            if (dataTemp == -1) {
                $("#alertPreliquidation").UifAlert('show', Resources.ErrorTransaction, 'danger');
            }

            //Convierte la PreLiquidacion temporal
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PreLiquidations/ConvertTempPreLiquidationToPreLiquidation",
                data: JSON.stringify({
                    "tempPreLiquidationId": tempPreLiquidationId,
                    "tempImputationId": tempImputationId,
                    "imputationTypeId": $("#ViewBagImputationType").val()
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //EXCEPCION ROLLBACK
                    if (data.success == false) {
                        $("#alertPreliquidation").UifAlert('show', data.result, "danger");

                    } else {
                        var msg = Resources.PreLiquidationSave + " " + data.Id + " " + Resources.PrintReportQuestion;
                        loadPreLiquidationReport(data.Id, IsNew, msg);
                        //cleanFieldsPreLiquidation();
                    }
                    cleanFieldsPreLiquidation();
                }
            });
        }
    });
}


//Carga el reporte y lo mantiene en una sesion en el controlador
function loadPreLiquidationReport(preliquidationId, isNew, msg) {

    $.ajax({
        type: "POST",
        async: false,
        url: ACC_ROOT + "Report/LoadPreLiquidationReport",
        data: JSON.stringify({
            "preLiquidationModel": setDataPreLiquidation(preliquidationId),
            "movementSumaryModel": loadMovementSumary()
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (parseInt(data) > 0) {
                confirmDialogPrintReport(msg, Resources.MenuPreLiquidation, isNew, preliquidationId, $("#BranchGenerationDrop").val());

            }
        }
    });
}


function confirmDialogPrintReport (messageIn, title, isNew, preliquidationId, branchId) {
    $.UifDialog('confirm', { 'message': messageIn, 'title': title },
    function (result) {
        if (result) {
            showReportPreliquidation();
            setDataPreLiquidationEmpty();
            cleanFieldsPreLiquidation();
            loadPreliquidation(isNew);

            setTimeout(function () {

                if ($("#ViewBagIsPreliquidation").val() == "1") {
                    location.href = $("#ViewBagLoadPreliquidationSearchLink").val() + "?branchId=" + branchId + "&preliquidationId=" + preliquidationId;
                }

            }, 1000);
        }
    });
};


function showReportPreliquidation() {
    window.open(ACC_ROOT + "Report/ShowPreLiquidationReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}


function loadMovementSumary() {

    var ids = $("#listViewAplicationReceipt").UifListView("getData");

    if (ids.length > 0) {
        for (var i in ids) {

            var rowid = ids[i];

            oMovementSumaryDetails = {
                Id: 0,
                Description: "",
                Debit: 0,
                Credit: 0
            };

            oMovementSumaryDetails.Id = rowid.Id;
            oMovementSumaryDetails.Description = rowid.MovementType;
            var debits = rowid.Debits;
            var credits = rowid.Credits
            oMovementSumaryDetails.Debit = parseFloat(debits.toString().trim().replace('$', '').replace(new RegExp(',', 'g'), ''));
            oMovementSumaryDetails.Credit = parseFloat(credits.toString().trim().replace('$', '').replace(new RegExp(',', 'g'), ''));
            oMovementSumary.MovementSumary.push(oMovementSumaryDetails);

        }
    }

    return oMovementSumary;
}


function confirmDialogAccept (branchId, tempImputationId) {
    $.UifDialog('confirm', {
        'message': Resources.MsgPreLiquidationConfirm, 'title': Resources.MenuPreLiquidation
    },
        function (result) {
            if (result) {

                updateTempPreLiquidation(tempPreLiquidationId, tempImputationId);

                setTimeout(function () {
                    setDataPreLiquidationEmpty();
                    cleanFieldsPreLiquidation();
                    loadPreliquidation(true);
                    validateCompanyPreliquidation();
                }, 2000);
                
                //regresa a la búsqueda de temporales
                if ($("#ViewBagIsPreliquidation").val() == "2") {
                    setTimeout(function () {
                        
                        location.href = $("#ViewBagLoadTempPreliquidationSearchLink").val() + "?branchId=" + branchId + "&tempImputationId=" + tempImputationId;
                        
                    }, 2000);
                 }
            }
        });
};

function cleanFieldsPreLiquidation() {

    $("#BranchGenerationDrop").val($("#ViewBagBranchUserDefault").val());
    $("#SalesPointDrop").val("");
    $("#PayerTypeDrop").val("");
    $("#BeneficiaryDocumentNumber").val("");
    $("#BeneficiaryName").val("");
    $("#Description").val("");
    $("#TotalControl").val("");

    cleanAutocompletesPreLiquidation('SearchSuppliers');
    cleanAutocompletesPreLiquidation('SearchInsured');
    cleanAutocompletesPreLiquidation('SearchCoinsurance');
    cleanAutocompletesPreLiquidation('SearchPerson');
    cleanAutocompletesPreLiquidation('SearchAgent');
    cleanAutocompletesPreLiquidation('SearchEmployee');
    cleanAutocompletesPreLiquidation('SearchReinsurer');


    tempPreLiquidationId = 0;
    individualIdPreLiquidation = 0;

    emptyMovementSumary();
    
    GetDebitsAndCreditsMovementTypesPreLiquidation(0, 0);
    SetTotalApplicationPreLiquidation();

    $("#BranchGenerationDrop").removeAttr("disabled");
    $("#SalesPointDrop").removeAttr("disabled");
    $("#PayerTypeDrop").removeAttr("disabled");
    $("#Description").removeAttr("disabled");
    $("#Description").removeAttr("disabled");
    
    $("#PayerTypeDrop").trigger('change');
}

function emptyMovementSumary() {
    oMovementSumary = {
        MovementSumary: []
    };

    oMovementSumaryDetails = {
        Id: 0,
        Description: "",
        Debit: 0,
        Credit: 0
    };

    return oMovementSumary;
}


function updateTempPreLiquidation(tempPreLiquidationId, tempImputationId) {

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "PreLiquidations/UpdateTempPreLiquidation",
        data: JSON.stringify({ "preLiquidationModel": setDataPreLiquidation(tempPreLiquidationId) }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function () {
            cleanFieldsPreLiquidation();
            setTimeout(function () {
                var msj = Resources.TempPreLiquidationSave + " " + tempImputationId;
                $("#alertPreliquidation").UifAlert("show", msj, "success");
            }, 500);

        }
    });

    tempPreLiquidationId = 0;
}


function setDataPreLiquidation(id) {

    oPreLiquidation.Id = id;
    oPreLiquidation.BranchId = $("#BranchGenerationDrop").val();
    oPreLiquidation.CompanyId = $("#CompanyGenerationDrop").val();
    oPreLiquidation.Description = $("#Description").val();
    oPreLiquidation.IndividualId = individualIdPreLiquidation;
    oPreLiquidation.PersonTypeId = $("#PayerTypeDrop").val();
    oPreLiquidation.SalePointId = $("#SalesPointDrop").val();
    oPreLiquidation.RegisterDate = $("#GenerationDate").val();
    oPreLiquidation.BranchDescription = $("#BranchGenerationDrop option:selected").text();
    oPreLiquidation.CompanyDescription = $("#CompanyGenerationDrop option:selected").text();
    oPreLiquidation.PayerDocumentNumber = documentNumberText;
    oPreLiquidation.PayerName = nameText;
    oPreLiquidation.TempImputationId = tempImputationId;
    oPreLiquidation.StatusId = 1;


    //la consulta viene de la pantalla: Busqueda de Preliquidacion, se debe editar solo el campo descripcion
    if ($("#ViewBagIsPreliquidation").val() == "1") {
        oPreLiquidation.IsTemporal = true;
    }
    else {
        //la consulta viene de la panatalla de Temporales(preliquidacion) se puden actualizar todos los campos
        oPreLiquidation.IsTemporal = false;
    }


    return oPreLiquidation;

}

function setDataPreLiquidationEmpty() {

    oPreLiquidation = {
        Id: 0,
        BranchId: 0,
        CompanyId: 0,
        Description: "",
        ImputationId: 0,
        IsTemporal: 0,
        IndividualId: 0,
        PersonTypeId: 0,
        RegisterDate: null,
        SalePointId: 0,
        StatusId: 0,

        BranchDescription: "",
        CompanyDescription: "",
        PayerDocumentNumber: "",
        PayerName: "",
        TempImputationId: 0
    };

    return oPreLiquidation;
}


// Autocomplete
function loadAutocompleteEventPreLiquidation(identifier) {
    $('#' + identifier + 'ByDocumentNumber').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesPreLiquidation(identifier, selectedItem);
    });


    $('#' + identifier + 'ByName').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesPreLiquidation(identifier, selectedItem);
    });

}


function fillAutocompletesPreLiquidation(identifier, selectedItem) {

    $('#' + identifier + 'ByDocumentNumber').val(selectedItem.DocumentNumber);
    $('#' + identifier + 'ByName').val(selectedItem.Name);

    documentNumberText = selectedItem.DocumentNumber;
    nameText = selectedItem.Name;

    if (selectedItem.Id != undefined) {
        individualIdPreLiquidation = selectedItem.Id;
    } else if (selectedItem.AgentId != undefined) {
        individualIdPreLiquidation = selectedItem.AgentId;
    } else {
        individualIdPreLiquidation = selectedItem.CoinsuranceIndividualId;
    }
    
}

function updateInput(element) {
    setTimeout(function () {
            if (element == "SearchInsuredByName") {
                var nameInsured = document.getElementById("SearchInsuredByName").value;
                if (nameInsured == "" || nameInsured != nameText) {
            $('#SearchInsuredByName').val(nameText);

            }
        } else if (element == "SearchInsuredByDocumentNumber") {
                var numberDocInsured = document.getElementById("SearchInsuredByDocumentNumber").value;
                if (numberDocInsured == "" || numberDocInsured != documentNumberText) {
                $('#SearchInsuredByDocumentNumber').val(documentNumberText);

            }
        }
    }, 10);
}

function setSalesPointPreLiquidation() {
    
   if (BranchUserDefault > 0) {
            $("#BranchGenerationDrop").val(BranchUserDefault);

            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + BranchUserDefault;
            $("#SalesPointDrop").UifSelect({ source: controller });

            //Setea el punto de venta de default
            setTimeout(function () {
                $("#SalesPointDrop").val($("#ViewBagSalePointBranchUserDefault").val());
            }, 500);

        } else {
            $("#BranchGenerationDrop").val("");
  }
}



//Recupera la preliquidación desde la búsqueda o setea para una nueva
function loadPreliquidation(isNew) {

    if (isNew) { //NUEVA

        setTimeout(function () {
    
            //SOLO DEBE EJECUTARSE CUANDO SE LEVANTE PANTALLA DE PRELIQUIDACIÒN
            if ($("#ViewBagImputationType").val() == typeImputationId) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "PreLiquidations/SaveTempPreLiquidation",
                    success: function (data) {

                        tempPreLiquidationId = data.Id;

                        //GRABA IMPUTACION EN TEMPORALES
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                            data: { "imputationTypeId": $("#ViewBagImputationType").val(), "sourceCode": data.Id },
                            success: function (dataImputation) {
                                tempImputationId = dataImputation.Id;

                                if (tempImputationId > 0) {

                                    var msj = Resources.PreLiquilidationTemporal + " " + tempImputationId;

                                    $("#globalTitle").text(msj);
                                }
                            }
                        });
                    }
                });
           }
        }, 1500);

    } else {//RECUPERADA
        
        tempImputationId = $("#ViewBagTempImputationId").val();
        tempPreLiquidationId = $("#ViewBagPreliquidationId").val();

        if ($("#ViewBagIsPreliquidation").val() == "2") { //VIENE DE TEMPORALES
            $("#globalTitle").html(Resources.PreLiquilidationTemporal + " " + tempImputationId);
        } else if ($("#ViewBagIsPreliquidation").val() == "1") {// VIENE DE REALES
            $("#globalTitle").html(Resources.Preliquidation + ": " + tempPreLiquidationId);
        }

       setTimeout(function () {
            $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitPreLiquidation(), TotalCreditPreLiquidation(), 3))));
        }, 2000);

       // setTimeout(function () {

            $("#BranchGenerationDrop").val($("#ViewBagBranchId").val());
            $("#BranchGenerationDrop").trigger('change');

            setTimeout(function () {

                $("#SalesPointDrop").val($("#ViewBagSalePointId").val());
                $("#SalesPointDrop").trigger('change');

                if ($("#ViewBagIsPreliquidation").val() == "2") {//VIENE DE TEMPORALES

                    $("#SalesPointDrop").attr("disabled", false);
                    if ($("#SalesPointDrop").val() == "" || $("#SalesPointDrop").val() == null) {
                        $("#SalesPointDrop").attr("disabled", true);
                    }
                }
                if ($("#ViewBagIsPreliquidation").val() == "1") {// VIENE DE REALES
                    $("#SalesPointDrop").attr("disabled", true);
                }
            }, 800);
            
            $("#CompanyGenerationDrop").val($("#ViewBagCompanyId").val());

            if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

                $("#CompanyGenerationDrop").attr("disabled", true);
            }
            else {
                $("#CompanyGenerationDrop").attr("disabled", false);
            }
            
            $("#CompanyGenerationDrop").attr('disabled', true);

            $("#GenerationDate").val($("#ViewBagGenerationDate").val());
            $("#PayerTypeDrop").val($("#ViewBagPersonTypeId").val());
            $("#PayerTypeDrop").trigger('change');

            $("#BeneficiaryDocumentNumber").val($("#ViewBagDocumentNumber").val());
            //$("#BeneficiaryDocumentNumber").prop('disabled', true);
            $("#BeneficiaryName").val($("#ViewBagName").val());
            //$("#BeneficiaryName").prop('disabled', true);
            $("#BeneficiaryDocumentNumberData").show();
            $("#BeneficiaryNameData").show();
            $("#Description").val($("#ViewBagDescription").val());

            if ($("#ViewBagIsPreliquidation").val() == "1") {//VIENE DE REALES
                $("#BranchGenerationDrop").prop('disabled', true);
                $("#CompanyGenerationDrop").prop('disabled', true);
                $("#PayerTypeDrop").prop('disabled', true);
                $("#BeneficiaryDocumentNumber").prop('disabled', true);
                $("#BeneficiaryName").prop('disabled', true);
            }
            if ($("#ViewBagIsPreliquidation").val() == "2") {//VIENE DE TEMPORALES
                $("#BranchGenerationDrop").removeAttr("disabled");
                $("#PayerTypeDrop").removeAttr("disabled");
                //$("#BeneficiaryDocumentNumber").attr('disabled', false);
                //$("#BeneficiaryName").attr('disabled', false);
            }

            $("#SearchSuppliersByDocumentNumber").parent().parent().hide();
            $("#SearchInsuredByDocumentNumber").parent().parent().hide();
            $("#SearchCoinsuranceByDocumentNumber").parent().parent().hide();
            $("#SearchPersonByDocumentNumber").parent().parent().hide();
            $("#SearchAgentByDocumentNumber").parent().parent().hide();
            $("#SearchEmployeeByDocumentNumber").parent().parent().hide();
            $("#SearchReinsurerByDocumentNumber").parent().parent().hide();

            $("#SearchSuppliersByName").parent().parent().hide();
            $("#SearchInsuredByName").parent().parent().hide();
            $("#SearchCoinsuranceByName").parent().parent().hide();
            $("#SearchPersonByName").parent().parent().hide();
            $("#SearchAgentByName").parent().parent().hide();
            $("#SearchEmployeeByName").parent().parent().hide();
            $("#SearchReinsurerByName").parent().parent().hide();

            individualIdPreLiquidation = $("#ViewBagBeneficiaryId").val();
            SetTotalApplicationPreLiquidation();
            SetTotalControlPreLiquidation();

        //}, 5000);
    }
}


function cleanAutocompletesPreLiquidation(identifier) {

    $('#' + identifier + 'ByDocumentNumber').val("");
    $('#' + identifier + 'ByName').val("");

    individualIdPreLiquidation = 0;

    $('#' + identifier + 'ByDocumentNumber').parent().parent().hide();
    $('#' + identifier + 'ByName').parent().parent().hide();
}


//****************************************************************************************************************
//****************************************************************************************************************
// INICIO // S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R
//****************************************************************************************************************
//****************************************************************************************************************

// Botón Aceptar
$("#SaveSearchPoliciesButton").click(function () {
    if ($("#ViewBagImputationType").val() == "3") {
        var amount = 0;
        if ($("#ViewBagAmount").val() != "") {
            amount = parseFloat(ClearFormatCurrency($("#ViewBagAmount").val().replace("", ",")));
        }
        $('#ModalPremiums').UifModal('hide');
        GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount);

        SetDataPremiumReceivableEmpty(); //--> DialogSearchPolicies.js
        ClearFields();//--> Accounting.js
        HideFields(); //--> DialogSearchPolicies.js

        CheckLoadedMovements();
        loadedMovementsPromise.then(function(isLoaded){
            if(isLoaded){
                clearTimeout(time);
                SetTotalApplicationPreLiquidation();
                $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitPreLiquidation(), TotalCreditPreLiquidation(), 3))));
            }
        });

        
    }
});


// Botón Cerrar
$("#CloseModalButton").click(function () {

    var amount = 0;
    if ($("#ViewBagAmount").val() != "") {
        amount = parseFloat(ClearFormatCurrency($("#ViewBagAmount").val().replace("", ",")));
    }

    GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount);

    SetDataPremiumReceivableEmpty();
    ClearFields();
    HideFields();

    setTimeout(function () {
        SetTotalApplicationPreLiquidation();
        $("#TotalControl").val(FormatCurrency(FormatDecimal(SetMainTotalControl(0, TotalDebitPreLiquidation(),
                                                            TotalCreditPreLiquidation(), 3))));
    }, 2000);

    $('#ModalPremiums').UifModal('hide');
});

//*********************************************************************************************************************
// FIN // S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R

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


var amount = 0;
var creditAmount = 0;
var debitAmount = 0;

var accountingCompanyDefault = $("#ViewBagAccountingCompanyDefault").val();
var branchUserDefault = $("#ViewBagBranchUserDefault").val();
var isMulticompany = $("#ViewBagParameterMulticompany").val();
var salePointBranchUserDefault = $("#ViewBagSalePointBranchUserDefault").val();
var userId = $("#ViewBagUserId").val();
var userNick = $("#ViewBagUserNick").val();

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES GLOBALES                                                                       */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

// Se refresca el listview resumen de movimientos
function GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount) {
    $("#listViewAplicationReceipt").UifListView({
        autoHeight: true,
        theme: 'dark',
        source: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes?tempImputationId=" + tempImputationId + "&amount=" + amount,
        customDelete: true,
        customEdit: true,
        edit: true,
        delete: false,
        displayTemplate: "#display-aplication-receipt-template"
    });
}

/// Obtiene el total de débitos del resumen de movimientos
function TotalDebitPreLiquidation() {
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
function TotalCreditPreLiquidation() {
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
function SetTotalControlPreLiquidation() {
    $("#ReceiptAmount").val("0");

    if ($("#ReceiptAmount").val() != undefined) {
        $("#TotalControl").val(SetMainTotalControl(parseFloat(ClearFormatCurrency($("#ReceiptAmount").val().replace("", ","))),
                                           TotalDebitPreLiquidation(), TotalCreditPreLiquidation(), 3));
    }

    var receiptAmount = $("#TotalControl").val();
    if (receiptAmount == "NaN") {
        receiptAmount = 0;
    }

    $("#TotalControl").val("$ " + NumberFormatSearch(receiptAmount, "2", ".", ","));
}

/// Setea el temporal de imputación a null
function CleanGlobalVariablesPreLiquidation() {
    tempImputationId = null;
}


/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                                    ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/


setTimeout(function () {
    SetTotalApplicationPreLiquidation();
    SetTotalControlPreLiquidation();
}, 2000);


/// Botón para edición en el listview
$("#listViewAplicationReceipt").on('rowEdit', function (event, data, index) {
    
    if ($("#PreLiquidationForm").valid() && individualIdPreLiquidation > 0) {

      $("#alertPreliquidation").UifAlert('hide');

        //ACTUALIZA EL TEMPORAL DE PRELIQUIDACIÓN
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PreLiquidations/UpdateTempPreLiquidation",
            data: JSON.stringify({ "preLiquidationModel": setDataPreLiquidation(tempPreLiquidationId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {}
        });
        
        // Primas por cobrar
        if (data.Id == 1) {
            refreshApplyView();
            ClearSearchFields();
            $('#ModalPremiums').UifModal('showLocal', Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            LoadSearchByForPolicies();
            LoadBranch();
            LoadPrefix();
        }
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
            LoadCoinsuresMovementsAccountingCompanies(oPreLiquidation.CompanyId);
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
            LoadReinsuraceMovementsCompanies(oPreLiquidation.CompanyId);
            $('#modalReinsurance').UifModal('showLocal', Resources.DialogReinsuranceMovementsTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
            $("#modalReinsurance").find("#OptionalOne").hide();
            $("#modalReinsurance").find("#OptionalTwo").hide();
            $("#modalReinsurance").find("#OptionalThree").hide();
            SetReinsuranceFieldEmpty();
            SetReinsuranceTotalMovement();
            SetReinsuranceAccountingCompany();
            
        }
        // Contabilidad
        if (data.Id == 7) {
            $("#addAccountingMovementForm").formReset(); //reseteo el formulario
            LoadAccountingBranchs();
            LoadAcountingNatures();
            LoadAccountingCurrencies();
            LoadAcountingCompanies(accountingCompanyDefault);
            AccountingMovementsReload();
            LoadAccountingMovementCompany();
            LoadthirdAccountingUsed(); // DANC 2018-06-13
            SetAccountingTotalMovement();
            GetAccountingSalePoints();
           
            $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
        }
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
            LoadPaymentVariousBranchs();
            LoadPaymentVariousCompanies(oPreLiquidation.CompanyId)
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
            LoadPaymentClaimsMovementsBranchs()
            LoadPaymentClaimsMovementsPrefixes();
            LoadPaymentClaimsMovementsRequestTypes();
            LoadPaymentClaimsMovementsCompanies(oPreLiquidation.CompanyId);
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
            $('#modalInsuredLoan').UifModal('showLocal', Resources.DialogInsuredLoanMovementsTitle + " " +
                                                         Resources.DialogTitleTemporary + " " + tempImputationId);
            SetInsuredLoanFieldEmpty();     //esta función está en InsuredLoans
            SetInsuredLoanTotalMovement();  //esta función está en InsuredLoans
        }
     } else {
        $("#alertPreliquidation").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
    }
});


function SavePreliquidationTemporal() {
    $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PreLiquidations/SaveTempPreLiquidation",
            success: function (data) {

                tempPreLiquidationId = data.Id;

                //GRABA IMPUTACION EN TEMPORALES
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                    data: { "imputationTypeId": $("#ViewBagImputationType").val(), "sourceCode": data.Id },
                    success: function (dataImputation) {
                        tempImputationId = dataImputation.Id;

                        if (tempImputationId > 0) {

                            var msj = Resources.PreLiquilidationTemporal + " " + tempImputationId;

                            $("#globalTitle").text(msj);
                        }
                    }
                });
            }
        });
}








$("#ApplyCancel").click(function () {
    CloseDialog(Resources.CancelApplicationMessage);
});

function CloseDialog (message) {
    $.UifDialog('confirm', { 'message': message, 'title': Resources.Yes },
        function (result) {
            if (result) {

                if (IsMainSearchBills == 1) {
                    if (TotalDebitPreLiquidation() != 0 || TotalCreditPreLiquidation() != 0) {

                        //ACTUALIZA ESTADO DE BILL
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "ReceiptApplication/UpdateBillStatus",
                            data: {
                                "imputationId": tempImputationId, "billCode": parseInt($("#ReceiptNumber").val()),
                                "comments": $("#_Observations").val(), "statusId": 2
                            },
                            success: function () {
                                CleanGlobalVariablesPreLiquidation();
                            }
                        });
                    }

                    CleanGlobalVariablesPreLiquidation();
                }

            }
        });
};


/// Botón aceptar aplicación de recibo
$("#ApplyAccept").click(function () {

    if (TotalDebitPreLiquidation() != 0 || TotalCreditPreLiquidation() != 0) {
        if (parseFloat(ClearFormatCurrency($("#TotalControl").val().replace("", ","))) != 0) {
            //DIALOGO DE COMFIRMACION PARA GRABAR IMPUTATION EN TEMPORALES, ACTUALIZA ESTADO DE BILL
            SaveTemporalConfirmation();
        } else {

            lockScreen();
            setTimeout(function () {     
            // GRABA IMPUTATION A REALES, ELIMINA DE TEMPORALES, ACTUALIZA ESTADO DE BILL
                $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "ReceiptApplication/SaveReceiptApplication",
                data: {
                    "sourceCode": $("#ReceiptNumber").val(), "tempImputationId": tempImputationId,
                    "imputationTypeId": Resources.ImputationTypePreLiquidation, "comments": $("#_Observations").val(),
                    "statusId": 3
                },
                success: function (data) {
                    unlockScreen();
                    $("#modalSuccess").find("#receiptApplicationMessage").text(Resources.SaveRealSuccessMessage + " :" + $("#ReceiptNumber").val());
                    if (data.IsEnabledGeneralLedger == false) {
                        $("#modalSuccess").find("#accountingApplicationLabelDiv").hide();
                        $("#modalSuccess").find("#accountingApplicationMessageDiv").hide();
                    } else {
                        $("#modalSuccess").find("#accountingApplicationLabelDiv").show();
                        $("#modalSuccess").find("#accountingApplicationMessageDiv").show();
                    }
                    $("#modalSuccess").find("#receiptApplicationAccountingIntegrationMessage").text(data.Message);
                    $('#modalSuccess').UifModal('showLocal', Resources.ReceiptsApplication);
                  }
                });
            }, 1000);   
        }
    } else {
        $("#alertPreliquidation").UifAlert('show', Resources.MovementsTypeValidation, "warning");
    }
});


function SaveTemporalConfirmation () {
    $.UifDialog('confirm', { 'message': Resources.SaveTemporalConfirmationMessage, 'title': Resources.ReceiptsApplication },
      function (result) {
          if (result) {
              // ACTUALIZA FECHA Y USUARIO DE TEMP-IMPUTATION. ACTUALIZA ESTADO, FECHA Y USUARIO DE RECIBO - PARCIALMENTE APLICADO
              $.ajax({
                  async: false,
                  type: "POST",
                  url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                  data: {
                      "imputationId": tempImputationId, "imputationTypeId": Resources.ImputationTypePreLiquidation,
                      "sourceCode": parseInt($("#ReceiptNumber").val()), "comments": $("#_Observations").val(), "statusId": 2
                  },
                  success: function () {
                      $("#alertPreliquidation").UifAlert('show', Resources.TemporalSuccessfullySaved + " " + tempImputationId, "success");
                  }
              });
          }
      });
};

/// Botón aceptar modal aplicación de recibo
$("#modalSuccess").find("#AcceptSuccess").click(function () {

    $('#modalSuccess').UifModal('hide');
    location.href = $("#ViewBagMainBillSearchLink").val();
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                            DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

// Setear el total de la listview resumen de movimientos
function SetTotalApplicationPreLiquidation() {
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
    } else {
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
        var response;
        response = GetCurrencyRateBilling($("#ViewBagDateAccounting").val(), selectCurrency);
        textExchangeRate.val("$ " + NumberFormatSearch(response[0], "6", ".", ","));

        if (response[1] == false) {
            $("#alertPreliquidation").UifAlert('show', Resources.ExchageRateNotUpToDate, "warning");
        }
    } else {
        textExchangeRate.val("");
    }
}

// Obtiene la tasa de cambio de la moneda
function GetCurrencyRateBilling(accountingDate, currencyId) {
    var alert = true;
    var rate;
    var response = new Array();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetCurrencyExchangeRate",
        data: {
            "rateDate": accountingDate,
            "currencyId": currencyId
        },
        success: function (data) {
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
            } else {
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


//Habilita / deshabilita combo Company segùn corresponda
function validateCompanyPreliquidation() {

    if (IsNew) {

        if ($("#CompanyGenerationDrop").val() != "" && $("#CompanyGenerationDrop").val() != null) {

            if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {
                
                $("#CompanyGenerationDrop").attr("disabled", true);
            }
            else {
                $("#CompanyGenerationDrop").attr("disabled", false);
            }
            clearInterval(timePreliquidation);
        } else {
            $("#CompanyGenerationDrop").val($("#ViewBagAccountingCompanyDefault").val());
        }
    } else {
        clearInterval(timePreliquidation);
    }
}


