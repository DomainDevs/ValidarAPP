/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

//var tempImputationId = 0;

//setTimeout(function () {
//    tempImputationId = $("#ViewBagTempImputationId").val();
//}, 1000);


//function RowInsuredLoanModel() {
//    this.LoanNumber;
//    this.IndividualId;
//    this.InsuredDocumentNumber;
//    this.InsuredName;
//    this.NatureId;
//    this.NatureName;
//    this.AccountingNature;
//    this.Description;
//    this.CurrencyId;
//    this.CurrencyName;
//    this.ExchangeRate;
//    this.Amount;
//    this.LocalAmount;
//    this.InsuredLoanItemId;
//    this.CurrentInterest;
//    this.PreviousInterest;
//    this.BillNumber;
//    this.Status;
//}

//var oInsuredLoanUpdateModel = {
//    ImputationId: 0,
//    InsuredLoansTransactionItems: []
//};

//var oInsuredLoanItemUpdateModel = {
//    InsuredLoanItemId: 0,
//    Description: null,
//    ImputationId: 0,
//    LoanNumber: 0,
//    IndividualId: 0,
//    InsuredDocumentNumber: null,
//    InsuredName: null,
//    AccountingNature: 0,
//    CurrencyId: 0,
//    ExchangeRate: 0,
//    IncomeAmount: 0,
//    Amount: 0,
//    Capital: 0,
//    CurrentInterest: 0,
//    PreviousInterest: 0,
//    InsuredLoansTransactionChild: []
//};

//var oInsuredLoanItemChildUpdateModel = {
//    InsuredLoanItemId: 0,
//    Description: null,
//    ImputationId: 0,
//    LoanNumber: 0,
//    IndividualId: 0,
//    InsuredDocumentNumber: null,
//    InsuredName: null,
//    AccountingNature: 0,
//    CurrencyId: 0,
//    ExchangeRate: 0,
//    IncomeAmount: 0,
//    Amount: 0,
//    Capital: 0,
//    CurrentInterest: 0,
//    PreviousInterest: 0
//};

//var individualId = 0;
//var currencyId = -1;
//var percentageDifference = 0;
//var insuredDocumentNumber = "";
//var insuredName = "";
//var editInsuredLoan = -1;

//var insuredLoanNumber = $('#LoanNumber').val();


///*---------------------------------------------------------------------------------------------------------------------------------------------------*/
///*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
///*---------------------------------------------------------------------------------------------------------------------------------------------------*/

////////////////////////////////////////////////////////////////////////////
///// Setear el modelo de préstamos asegurado con los valores ingresados ///
////////////////////////////////////////////////////////////////////////////
//function SetDataInsuredLoans() {
//    SetDataInsuredLoansEmpty();

//    oInsuredLoanModel.ImputationId = tempImputationId;

//    var movements = $("#loansListView").UifListView("getData");

//    if (movements != null) {
//        for (var j = 0; j < movements.length; j++) {

//            if (movements[j].Status == 1) {
//                oInsuredLoanItemModel = {
//                    InsuredLoanItemId: 0,
//                    Description: null,
//                    //ImputationId: 0,
//                    LoanNumber: 0,
//                    IndividualId: 0,
//                    InsuredDocumentNumber: null,
//                    InsuredName: null,
//                    AccountingNature: 0,
//                    CurrencyId: 0,
//                    ExchangeRate: 0,
//                    IncomeAmount: 0,
//                    Amount: 0,
//                    Capital: 0,
//                    CurrentInterest: 0,
//                    PreviousInterest: 0,
//                    InsuredLoansTransactionChild: []
//                };

//                oInsuredLoanItemModel.InsuredLoanItemId = movements[j].InsuredLoanItemId;
//                oInsuredLoanItemModel.Description = movements[j].Description;
//                oInsuredLoanItemModel.LoanNumber = movements[j].LoanNumber;
//                oInsuredLoanItemModel.IndividualId = movements[j].IndividualId;
//                oInsuredLoanItemModel.InsuredDocumentNumber = movements[j].InsuredDocumentNumber;
//                oInsuredLoanItemModel.InsuredName = movements[j].InsuredName;
//                oInsuredLoanItemModel.AccountingNature = movements[j].AccountingNature;
//                oInsuredLoanItemModel.CurrencyId = movements[j].CurrencyId;
//                oInsuredLoanItemModel.ExchangeRate = ClearFormatCurrency(movements[j].ExchangeRate).replace(",", ".");
//                oInsuredLoanItemModel.IncomeAmount = ClearFormatCurrency(movements[j].LocalAmount).replace(",", ".");
//                oInsuredLoanItemModel.Amount = ClearFormatCurrency(movements[j].Amount).replace(",", ".");
//                oInsuredLoanItemModel.Capital = ClearFormatCurrency(movements[j].Amount).replace(",", ".");
//                oInsuredLoanItemModel.CurrentInterest = ClearFormatCurrency(movements[j].CurrentInterest).replace(",", ".");
//                oInsuredLoanItemModel.PreviousInterest = ClearFormatCurrency(movements[j].PreviousInterest).replace(",", ".");

//                // Items a aplicar
//                var loanItemsToApply = $("#loanItemsListView").UifListView("getData");

//                if (loanItemsToApply != null) {
//                    for (var k = 0; k < loanItemsToApply.length; k++) {

//                        oInsuredLoanItemChildModel = {
//                            InsuredLoanItemId: 0,
//                            Description: null,
//                            //ImputationId: 0,
//                            LoanNumber: 0,
//                            IndividualId: 0,
//                            InsuredDocumentNumber: null,
//                            InsuredName: null,
//                            AccountingNature: 0,
//                            CurrencyId: 0,
//                            ExchangeRate: 0,
//                            IncomeAmount: 0,
//                            Amount: 0,
//                            Capital: 0,
//                            CurrentInterest: 0,
//                            PreviousInterest: 0
//                        };

//                        oInsuredLoanItemChildModel.InsuredLoanItemId = loanItemsToApply[j].InsuredLoanItemId;
//                        oInsuredLoanItemChildModel.Description = loanItemsToApply[j].Description;
//                        oInsuredLoanItemChildModel.LoanNumber = loanItemsToApply[j].LoanNumber;
//                        oInsuredLoanItemChildModel.IndividualId = loanItemsToApply[j].IndividualId;
//                        oInsuredLoanItemChildModel.InsuredDocumentNumber = loanItemsToApply[j].InsuredDocumentNumber;
//                        oInsuredLoanItemChildModel.InsuredName = loanItemsToApply[k].InsuredName;
//                        oInsuredLoanItemChildModel.AccountingNature = loanItemsToApply[k].AccountingNature;
//                        oInsuredLoanItemChildModel.CurrencyId = loanItemsToApply[j].CurrencyId;
//                        oInsuredLoanItemChildModel.ExchangeRate = ClearFormatCurrency(loanItemsToApply[j].ExchangeRate).replace(",", ".");
//                        oInsuredLoanItemChildModel.IncomeAmount = ClearFormatCurrency(loanItemsToApply[k].LocalAmount).replace(",", ".");
//                        oInsuredLoanItemChildModel.Amount = ClearFormatCurrency(loanItemsToApply[k].Amount).replace(",", ".");
//                        oInsuredLoanItemChildModel.Capital = ClearFormatCurrency(loanItemsToApply[k].Amount).replace(",", ".");
//                        oInsuredLoanItemChildModel.CurrentInterest = ClearFormatCurrency(loanItemsToApply[j].CurrentInterest).replace(",", ".");
//                        oInsuredLoanItemChildModel.PreviousInterest = ClearFormatCurrency(loanItemsToApply[k].PreviousInterest).replace(",", ".");

//                        oInsuredLoanItemModel.InsuredLoasTransactionChild.push(oInsuredLoanItemChildModel);
//                    }
//                }

//                oInsuredLoanModel.InsuredLoansTransactionItems.push(oInsuredLoanItemModel);
//            }
//        }
//    }

//    return oInsuredLoanModel;
//}

//////////////////////////////////////////////////////////////////////
///// Setea el modelo de préstamos asegurado con valores iniciales ///
//////////////////////////////////////////////////////////////////////
//function SetDataInsuredLoansEmpty() {
//    oInsuredLoanModel = {
//        ImputationId: 0,
//        InsuredLoansTransactionItems: []
//    };

//    oInsuredLoanItemModel = {
//        InsuredLoanItemId: 0,
//        Description: null,
//        ImputationId: 0,
//        LoanNumber: 0,
//        IndividualId: 0,
//        InsuredDocumentNumber: null,
//        InsuredName: null,
//        AccountingNature: 0,
//        CurrencyId: 0,
//        ExchangeRate: 0,
//        IncomeAmount: 0,
//        Amount: 0,
//        Capital: 0,
//        CurrentInterest: 0,
//        PreviousInterest: 0,
//        InsuredLoansTransactionChild: []
//    };
//}

//////////////////////////////////////////////////////////////////////////////////
///// Setear el modelo de items préstamos asegurado con los valores ingresados ///
//////////////////////////////////////////////////////////////////////////////////
//function SetDataInsuredLoansItem(tempInsuredLoanCode) {
//    SetDataInsuredLoansEmpty();

//    oInsuredLoanModel.ImputationId = tempImputationId;

//    oInsuredLoanItemModel = {
//        InsuredLoanItemId: 0,
//        Description: null,
//        ImputationId: 0,
//        LoanNumber: 0,
//        IndividualId: 0,
//        InsuredDocumentNumber: null,
//        InsuredName: null,
//        AccountingNature: 0,
//        CurrencyId: 0,
//        ExchangeRate: 0,
//        IncomeAmount: 0,
//        Amount: 0,
//        Capital: 0,
//        CurrentInterest: 0,
//        PreviousInterest: 0,
//        InsuredLoansTransactionChild: []
//    };

//    var movementItemsToApply = $("#loanItemsListView").UifListView("getData");
//    if (movementItemsToApply != null) {
//        for (var j = 0; j < movementItemsToApply.length; j++) {

//            oInsuredLoanItemModel.InsuredLoanItemId = tempInsuredLoanCode;

//            oInsuredLoanItemChildModel = {
//                InsuredLoanItemId: 0,
//                InsuredLoanId: 0,
//                Description: null,
//                ImputationId: 0,
//                LoanNumber: 0,
//                IndividualId: 0,
//                InsuredDocumentNumber: null,
//                InsuredName: null,
//                AccountingNature: 0,
//                CurrencyId: 0,
//                ExchangeRate: 0,
//                IncomeAmount: 0,
//                Amount: 0,
//                Capital: 0,
//                CurrentInterest: 0,
//                PreviousInterest: 0
//            };

//            oInsuredLoanItemChildModel.InsuredLoanItemId = movementItemsToApply[j].InsuredLoanItemId;
//            oInsuredLoanItemChildModel.InsuredLoanId = movementItemsToApply[j].InsuredLoanId;
//            oInsuredLoanItemModel.InsuredLoansTransactionChild.push(oInsuredLoanItemChildModel);
//        }
//    }
//    oInsuredLoanModel.InsuredLoansTransactionItems.push(oInsuredLoanItemModel);

//    return oInsuredLoanModel;
//}

/////////////////////////////////////////////////////
////  Añade registros en listView                  //
/////////////////////////////////////////////////////
//var saveInsuredLoanCallback = function (deferred, data) {
//    $("#loansListView").UifListView("addItem", data);
//    deferred.resolve();
//};

/////////////////////////////////////////////////////
////  Edita un registro en listView                //
////////////////////////////////////////////////////
//var editInsuredLoanCallback = function (deferred, data) {
//    data.NombreEmpresa = "Sistran";
//    deferred.resolve(data);
//};

/////////////////////////////////////////////////////
////  Elimina un registro en listView                //
////////////////////////////////////////////////////
//var deleteInsuredLoanCallback = function (deferred, data) {
//    deferred.resolve();

//    $.ajax({
//        async: false,
//        type: "POST",
//        url: ACC_ROOT + "InsuredLoans/DeleteTempInsuredLoansItem",
//        data: { "tempImputationId": tempImputationId, "tempInsuredLoanId": data.InsuredLoanItemId },
//        success: function () {
//            RefreshInsuredLoanMovements();
//        }
//    });

//    setTimeout(function () {
//        SetInsuredLoanTotalMovement();
//    }, 2000);
//};


//////////////////////////////////////////////////////////////
//// Setea el valor del importe local en préstamo asegurado //
//////////////////////////////////////////////////////////////
//function SetInsuredLoanLocalAmount() {
//    var loanAmount = $("#LoanAmount").val().replace("$", "").replace(/,/g, "").replace(" ", "");
//    var exchangeRate = $("#LoanExchangeRate").val().replace("$", "").replace(/,/g, "").replace(" ", "");
//    var loanLocalAmount = loanAmount * exchangeRate;
//    $("#LoanLocalAmount").val("$ " + NumberFormatSearch(loanLocalAmount, "2", ".", ","));
//}

////////////////////////////////////////////////////
//// Valida el ingreso de campos obligatorios     //
////////////////////////////////////////////////////
//function ValidateInsuredLoanAddForm() {
//    if ($('#LoanCurrency').val() == "") {
//        $("#modalInsuredLoan").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
//        return false;
//    }
//    if ($('#LoanAmount').val() == "") {
//        $("#modalInsuredLoan").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
//        return false;
//    }
//    return true;
//}

//////////////////////////////////////////////////////////////////////
//// Setear el total de la listview movimientos préstamos asegurado //
//////////////////////////////////////////////////////////////////////
//function SetInsuredLoanTotalMovement() {
//    var totalMovementCredit = 0;
//    var totalMovementDebit = 0;

//    var loans = $("#loansListView").UifListView("getData");

//    if (loans != null) {

//        for (var j = 0; j < loans.length; j++) {
//            var loanAmount = String(loans[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
//            if (loans[j].NatureId == 1)
//                totalMovementCredit += parseFloat(loanAmount);
//            else
//                totalMovementDebit += parseFloat(loanAmount);
//        }
//    }
//    else {
//        $("#TotalLoanMovementCredit").text("");
//        $("#TotalLoanMovementDebit").text("");
//    }
//    $("#TotalLoanMovementCredit").text("$" + NumberFormatSearch(totalMovementCredit, "2", ".", ","));
//    $("#TotalLoanMovementDebit").text("$" + NumberFormatSearch(totalMovementDebit, "2", ".", ","));
//}

//////////////////////////////////////////////////////////////////
///// Validación de duplicados movimientos préstamos asegurado ///
//////////////////////////////////////////////////////////////////
//function ValidateDuplicateInsuredLoan() {
//    var duplicate = -1;

//    $.ajax({
//        async: false,
//        type: "POST",
//        url: ACC_ROOT + "InsuredLoans/ValidateDuplicateInsuredLoan",
//        data: {
//            "loanNumber": $("#LoanNumber").val(),
//            "individualId": individualId,
//            "accountingNatureId": $("#LoanNature").val(),
//            "currencyId": $("#LoanCurrency").val()
//        },
//        success: function (data) {

//            if (data[0].source == -1) {
//                duplicate = 0;
//                $("#MovementMessageDuplicate").val("");
//            }
//            else if (data[0].source >= 0) {
//                duplicate = 1;
//                if (data[0].source == 0) {

//                    if (data[0].isReal == 1) {

//                        if (data[0].type == 1) {
//                            $("#MovementMessageDuplicate").val(Resources.CashReceipt + ": " + data[0].imputationId);
//                        }
//                        else if (data[0].type == 2) {
//                            $("#MovementMessageDuplicate").val(Resources.JournalEntry + ": " + data[0].imputationId);
//                        }
//                        else if (data[0].type == 3) {
//                            $("#MovementMessageDuplicate").val(Resources.Preliquidation + ": " + data[0].imputationId);
//                        }
//                        else if (data[0].type == 4) {
//                            $("#MovementMessageDuplicate").val(Resources.PaymentOrder + ": " + data[0].imputationId);
//                        }
//                    }
//                    else {
//                        $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + ": " + data[0].imputationId);
//                    }
//                }
//                else if (data[0].source == 1) {
//                    $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + ": " + data[0].imputationId);
//                }
//            }
//        }
//    });

//    return duplicate;
//}

/////////////////////////////////////////////////////////////////////////
///// Setea los valores de los items a aplicar de préstamos asegurado ///
/////////////////////////////////////////////////////////////////////////
//function SetUpdateDataInsuredLoans() {
//    oInsuredLoanUpdateModel = {
//        ImputationId: 0,
//        InsuredLoansTransactionItems: []
//    };

//    oInsuredLoanItemUpdateModel = {
//        InsuredLoanItemId: 0,
//        Description: null,
//        ImputationId: 0,
//        LoanNumber: 0,
//        IndividualId: 0,
//        InsuredDocumentNumber: null,
//        InsuredName: null,
//        AccountingNature: 0,
//        CurrencyId: 0,
//        ExchangeRate: 0,
//        IncomeAmount: 0,
//        Amount: 0,
//        Capital: 0,
//        CurrentInterest: 0,
//        PreviousInterest: 0,
//    };

//    var itemsToApply = $("#loansListView").UifListView("getData");

//    if (itemsToApply != null) {

//        for (var j = 0; j < itemsToApply.length; j++) {
//            if (itemsToApply[j].Status == 0) {
//                oInsuredLoanUpdateModel.ImputationId = tempImputationId;
//                oInsuredLoanItemUpdateModel = {
//                    InsuredLoanItemId: 0,
//                    Description: null,
//                    ImputationId: 0,
//                    LoanNumber: 0,
//                    IndividualId: 0,
//                    InsuredDocumentNumber: null,
//                    InsuredName: null,
//                    AccountingNature: 0,
//                    CurrencyId: 0,
//                    ExchangeRate: 0,
//                    IncomeAmount: 0,
//                    Amount: 0,
//                    Capital: 0,
//                    CurrentInterest: 0,
//                    PreviousInterest: 0,
//                };

//                oInsuredLoanItemUpdateModel.InsuredLoanItemId = itemsToApply[j].InsuredLoanItemId;
//                oInsuredLoanItemUpdateModel.Description = itemsToApply[j].Description;
//                oInsuredLoanItemUpdateModel.ImputationId = itemsToApply[j].ImputationId;
//                oInsuredLoanItemUpdateModel.LoanNumber = itemsToApply[j].LoanNumber;
//                oInsuredLoanItemUpdateModel.IndividualId = itemsToApply[j].IndividualId;
//                oInsuredLoanItemUpdateModel.InsuredDocumentNumber = itemsToApply[j].InsuredDocumentNumber;
//                oInsuredLoanItemUpdateModel.InsuredName = itemsToApply[j].InsuredName;
//                oInsuredLoanItemUpdateModel.AccountingNature = itemsToApply[j].AccountingNature
//                oInsuredLoanItemUpdateModel.CurrencyId = itemsToApply[j].CurrencyId;
//                oInsuredLoanItemUpdateModel.ExchangeRate = ClearFormatCurrency(String(itemsToApply[j].EchangeRate)).replace(",", ".");
//                oInsuredLoanItemUpdateModel.IncomeAmount = ClearFormatCurrency(String(itemsToApply[j].LocalAmount)).replace(",", ".");
//                oInsuredLoanItemUpdateModel.Amount = ClearFormatCurrency(String(itemsToApply[j].Amount)).replace(",", ".");
//                oInsuredLoanItemUpdateModel.Capital = itemsToApply[j].Capital;
//                oInsuredLoanItemUpdateModel.CurrentInterest = itemsToApply[j].CurrentInterest;

//                oInsuredLoanUpdateModel.InsuredLoansTransactionItems.push(oInsuredLoanItemUpdateModel);
//            }
//        }
//    }

//    return oInsuredLoanUpdateModel;
//}

////////////////////////////////////////////////////////////////
///// Realiza la consulta de movimientos préstamos asegurado ///
////////////////////////////////////////////////////////////////
//function RefreshInsuredLoanMovements() {
//    $("#loansListView").UifListView(
//    {
//        autoHeight: true,
//        source: ACC_ROOT + "InsuredLoans/GetTempInsuredLoansItemByTempImputationId?tempImputationId=" + tempImputationId,
//        customDelete: false,
//        customAdd: false,
//        customEdit: true,
//        add: false,
//        edit: false,
//        delete: true,
//        displayTemplate: "#loan-display-template",
//        deleteCallback: deleteInsuredLoanCallback
//    });
//    SetInsuredLoanTotalMovement();
//}

////////////////////////////////////////////////////////////////////
///// Setea los campos a valores iniciales - préstamos asegurado ///
////////////////////////////////////////////////////////////////////
//function SetInsuredLoanFieldEmpty() {
//    $("#LoanNumber").val("");
//    $("#InsuredDocumentNumber").val("");
//    $("#InsuredName").val("");
//    $("#LoanNature").val("");
//    $("#LoanCurrency").val("");
//    $("#LoanExchangeRate").val("");
//    $("#LoanAmount").val("");
//    $("#LoanLocalAmount").val("");
//    $("#LoanCurrentInterest").val("");
//    $("#LoanPreviousInterest").val("");
//}

//////////////////////////////////////////////////////////////////////
///// Llena los combos con valores iniciales - préstamos asegurado ///
//////////////////////////////////////////////////////////////////////
//function FillLoanDropDown() {
//    var controller = ACC_ROOT + "Common/GetNatures";
//    $("#LoanNature").UifSelect({ source: controller });

//    controller = ACC_ROOT + "Common/GetCurrencies";
//    $("#LoanCurrency").UifSelect({ source: controller });
//}

///*---------------------------------------------------------------------------------------------------------------------------------*/
///*                                                        ACCIONES / EVENTOS                                                       */
///*---------------------------------------------------------------------------------------------------------------------------------*/

//$("#modalInsuredLoan").find('#InsuredDocumentNumber').attr("disabled", "disabled");
//$("#modalInsuredLoan").find('#InsuredName').attr("disabled", "disabled");

//setTimeout(function () {
//    $("#loansListView").UifListView(
//    {
//        autoHeight: true,
//        //source: ACC_ROOT + "InsuredLoans/GetTempInsuredLoansItemByTempImputationId?tempImputationId=" + tempImputationId,
//        source: ACC_ROOT + "InsuredLoans/GetTempInsuredLoansItemByTempImputationId?tempImputationId=" + $("#ViewBagTempImputationId").val(),
//        customDelete: false,
//        customAdd: false,
//        customEdit: true,
//        add: false,
//        edit: false,
//        delete: true,
//        displayTemplate: "#loan-display-template",
//        deleteCallback: deleteInsuredLoanCallback
//    });
//}, 500);

/////////////////////////////////////////////////////////
///// Combo moneda - préstamos asegurado              ///
/////////////////////////////////////////////////////////
//$("#LoanCurrency").on('itemSelected', function (event, selectedItem) {
//    $("#modalInsuredLoan").find("#alertForm").hide();
//    SetCurrency("LoanCurrency", "LoanExchangeRate");
//    if ($("#LoanAmount").val() != "") {
//        SetInsuredLoanLocalAmount();
//    }
//});

/////////////////////////////////////////////////////////
///// Importe - préstamos asegurado                   ///
/////////////////////////////////////////////////////////
//$("#LoanAmount").blur(function () {
//    $("#modalInsuredLoan").find("#alertForm").hide();
//    if ($("#LoanAmount").val() != "") {
//        var loanAmount = $("#LoanAmount").val();
//        $("#LoanAmount").val("$ " + NumberFormatSearch(loanAmount, "2", ".", ","));
//        SetInsuredLoanLocalAmount();
//    }
//});

//////////////////////////////////////////////////////////
//// Autocomplete número préstamo - préstamos asegurado //
//////////////////////////////////////////////////////////
//$('#LoanNumber').on('itemSelected', function (event, selectedItem) {
//    $("#modalInsuredLoan").find("#alertForm").hide();
//    individualId = selectedItem.IndividualId;
//    if (individualId > 0) {
//        $("#modalInsuredLoan").find('#InsuredDocumentNumber').val(selectedItem.DocumentNumber);
//        $("#modalInsuredLoan").find('#InsuredName').val(selectedItem.Name);
//        insuredDocumentNumber = selectedItem.DocumentNumber;
//        insuredName = selectedItem.Name;
//        insuredLoanNumber = selectedItem.LoanNumber;
//    }
//    else {
//        $("#modalInsuredLoan").find('#InsuredDocumentNumber').val("");
//        $("#modalInsuredLoan").find('#InsuredName').val("");
//        insuredDocumentNumber = "";
//        insuredName = "";
//        insuredLoanNumber = "";
//    }
//});

//////////////////////////////////////////////////////////////////////////
//// Control de borrado de autocomplete en campo de número de préstamo  //
//////////////////////////////////////////////////////////////////////////
//$("#LoanNumber").on('blur', function (event) {
//    setTimeout(function () {
//        $('#LoanNumber').val(insuredLoanNumber);
//    }, 50);
//});

/////////////////////////////////////////////////////////
///// Interés año actual - préstamos asegurado        ///
/////////////////////////////////////////////////////////
//$("#LoanCurrentInterest").blur(function () {
//    $("#modalInsuredLoan").find("#alertForm").hide();
//    if ($("#LoanCurrentInterest").val() != "") {
//        var currentAmount = $("#LoanCurrentInterest").val();
//        $("#LoanCurrentInterest").val("$ " + NumberFormatSearch(currentAmount, "2", ".", ","));
//        //SetInsuredLoanLocalAmount();
//    }
//});

/////////////////////////////////////////////////////////
///// Interés año anterior - préstamos asegurado      ///
/////////////////////////////////////////////////////////
//$("#LoanPreviousInterest").blur(function () {
//    $("#modalInsuredLoan").find("#alertForm").hide();
//    if ($("#LoanPreviousInterest").val() != "") {
//        var previousAmount = $("#LoanPreviousInterest").val();
//        $("#LoanPreviousInterest").val("$ " + NumberFormatSearch(previousAmount, "2", ".", ","));
//        //SetInsuredLoanLocalAmount();
//    }
//});

//////////////////////////////////////////////
//// Edición ListView - préstamos asegurado //
//////////////////////////////////////////////
//$('#loansListView').on('rowEdit', function (event, data, index) {
//    $("#LoanCurrency").val(data.CurrencyId);
//    $("#LoanAmount").val(data.Amount);
//    $("#LoanExchangeRate").val(data.ExchangeRate);
//    $("#LoanLocalAmount").val(data.LocalAmount);
//    $("#LoanCurrentInterest").val(data.LoanCurrentInterest);
//    $("#LoanPreviousInterest").val(data.LoanPreviousInterest);
//    $("#InsuredDocumentNumber").val(data.InsuredDocumentNumber);
//    $("#InsuredName").val(data.InsuredName);
//    $("#LoanNature").val(data.NatureId);
//    individualId = data.IndividualId;

//    editInsuredLoan = index;
//});

//$('#loansListView').on('rowDelete', function (event, data) {
//    //alert("cata");
//    SetInsuredLoanTotalMovement();
//});

////////////////////////////
//// Botón agregar modal ///
////////////////////////////
//$('#InsuredLoanAdd').click(function () {
//    paymentMethod = "C";
//    var existMovement = 0;
//    $("#modalInsuredLoan").find("#alertForm").hide();

//    $("#addInsuredLoanForm").validate();

//    if ($("#addInsuredLoanForm").valid()) {
//        if (ValidateInsuredLoanAddForm() == true) {
//            if (ValidateDuplicateInsuredLoan() == 0) {
//                // Se valida que no se ingrese el mismo registro
//                var keyValid = $("#LoanNumber").val() + $("#modalInsuredLoan").find("#InsuredDocumentNumber").val() +
//                               $("#modalInsuredLoan").find("#InsuredName").val() + individualId +
//                               $("#LoanNature").val() + $("#LoanCurrency").val();

//                var movements = $('#loansListView').UifListView("getData");

//                if (movements != null) {
//                    for (var j = 0; j < movements.length; j++) {
//                        var movementIndex = movements[j].LoanNumber + movements[j].InsuredDocumentNumber + movements[j].InsuredName +
//                                            movements[j].IndividualId + movements[j].NatureId + movements[j].CurrencyId;
//                        if (movementIndex == keyValid) {
//                            existMovement = 1;
//                            break;
//                        }
//                    }
//                }

//                if (editInsuredLoan > -1) {
//                    if (($("#LoanNumber").val() == $("#HiddenLoanNumber").val()) &&
//                        ($("#LoanCurrency").val() == $("#HiddenCurrencyId").val()) &&
//                        (individualId == $("#HiddenIndividualId").val())) {
//                        existMovement = 0;
//                    }
//                }

//                if (existMovement == 0) {
//                    var rowModel = new RowInsuredLoanModel();
//                    rowModel.LoanNumber = $('#LoanNumber').val();
//                    rowModel.IndividualId = individualId;
//                    rowModel.InsuredDocumentNumber = $("#modalInsuredLoan").find('#InsuredDocumentNumber').val();
//                    rowModel.InsuredName = $("#modalInsuredLoan").find('#InsuredName').val();
//                    rowModel.NatureId = $('#LoanNature').val();
//                    rowModel.NatureName = $('#LoanNature option:selected').text();
//                    rowModel.AccountingNature = $('#LoanNature').val();
//                    rowModel.Description = "";
//                    rowModel.CurrencyId = $('#LoanCurrency').val();
//                    rowModel.CurrencyName = $('#LoanCurrency option:selected').text();
//                    rowModel.ExchangeRate = $("#LoanExchangeRate").val();
//                    rowModel.Amount = $('#LoanAmount').val();
//                    rowModel.LocalAmount = $("#LoanLocalAmount").val();
//                    rowModel.InsuredLoanItemId = 0;
//                    rowModel.CurrentInterest = $("#LoanCurrentInterest").val();
//                    rowModel.PreviousInterest = $("#LoanPreviousInterest").val();
//                    rowModel.BillNumber = $("#MovementBillNumber").val();
//                    rowModel.Status = "1";

//                    $('#TotalLoanMovementCredit').text(rowModel.Amount);

//                    if (editInsuredLoan == -1) {
//                        $('#loansListView').UifListView("addItem", rowModel);
//                    } else {
//                        $('#loansListView').UifListView("editItem", editInsuredLoan, rowModel);
//                        editInsuredLoan = -1;
//                    }

//                    $.ajax({
//                        async: false,
//                        type: "POST",
//                        url: ACC_ROOT + "InsuredLoans/SaveTempInsuredLoansRequest",
//                        data: { "insuredLoanModel": SetDataInsuredLoans() },
//                        success: function () {
//                            RefreshInsuredLoanMovements();
//                        }
//                    });

//                    $("#addInsuredLoanForm").formReset();
//                    SetInsuredLoanTotalMovement();
//                }
//                else {
//                    $("#modalInsuredLoan").find("#alertForm").UifAlert('show', Resources.ValidateDuplicatePaymentMethods, "warning");
//                }
//            }
//            else {
//                var message = Resources.AgentsMessageValidateDuplicate + ": " + $("#MovementMessageDuplicate").val();
//                $("#modalInsuredLoan").find("#alertForm").UifAlert('show', message, "warning");
//            }
//        }

//        setTimeout(function () {
//            SetInsuredLoanTotalMovement();
//        }, 1000);
//    }
//});


//////////////////////////////////////////////////////
//// Botón aceptar movimientos préstamos asegurado ///
//////////////////////////////////////////////////////
//$('#InsuredLoanAcceptMovement').click(function () {
//    $("#modalInsuredLoan").find("#alertForm").hide();
//    var movements = $("#loansListView").UifListView("getData");

//    if (movements != null) {
//        for (var j = 0; j < movements.length; j++) {
//            if (movements[j].DebitCreditName == "Crédito") {
//                creditAmount = creditAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
//            } else {
//                debitAmount = debitAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
//            }
//        }
//    }

//    SetUpdateDataInsuredLoans();
//    if ($("#ReceiptAmount").val() == "") {
//        $("#ReceiptAmount").val("0");
//    }

//    amount = $("#ReceiptAmount").val();
//    if (isNaN(amount)) {
//        amount = 0;
//    }
//    else {
//        amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val()));
//    }

//    $('#modalInsuredLoan').UifModal('hide');

//    if ($("#ViewBagImputationType").val() == 1) {
//        GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount);
//    }
//    if ($("#ViewBagImputationType").val() == 2) {
//        GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount);
//    }
//    if ($("#ViewBagImputationType").val() == 3) {
//        GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount);
//    }
//    if ($("#ViewBagImputationType").val() == 4) {
//        GetDebitsAndCreditsMovementTypes(tempImputationId, amount);
//    }

//    setTimeout(function () {
//        if ($("#ViewBagImputationType").val() == 1) {
//            SetTotalApplicationReceipt();
//            SetTotalControlReceipt();
//        }
//        if ($("#ViewBagImputationType").val() == 2) {
//            SetTotalApplicationJournal();
//            SetTotalControlJournal();
//        }
//        if ($("#ViewBagImputationType").val() == 3) {
//            SetTotalApplicationPreLiquidation();
//            SetTotalControlPreLiquidation();
//        }
//        if ($("#ViewBagImputationType").val() == 4) {
//            SetTotalApplication();
//            SetTotalControl();
//        }
//    }, 3000);
//});
