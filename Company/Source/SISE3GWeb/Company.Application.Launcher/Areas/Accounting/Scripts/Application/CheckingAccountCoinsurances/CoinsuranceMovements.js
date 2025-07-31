var tempImputationId = 0;

setTimeout(function () {
    tempImputationId = $("#ViewBagTempImputationId").val();
}, 1000);


function RowCoinsurerModel() {
    this.BranchId;
    this.BranchName;
    this.SalePointId;
    this.SalePointName;
    this.CompanyId;
    this.CompanyName;
    this.CoinsuranceTypeId;
    this.CoinsuranceTypeName;
    this.Description;
    this.CoinsurerId;
    this.CoinsurerDocumentNumber;
    this.CoinsurerName;
    this.ConceptId;
    this.ConceptName;
    this.NatureId;
    this.NatureName;
    this.AccountingNature;
    this.CurrencyId;
    this.Currency;
    this.ExchangeRate;
    this.Amount;
    this.LocalAmount;
    this.CoinsurerCheckingAccountItemId;
    this.AgentTypeCode;
    this.AgentAgencyCode;
    this.BillNumber;
    this.TempCoinsuranceParentId;
    this.Status;
}

var oCoinsuranceCheckingAccountUpdateModel = {
    ImputationId: 0,
    CoinsuranceCheckingAccountTransactionItems: []
};

var oCoinsuranceCheckingAccountItemUpdateModel = {
    CoinsuranceCheckingAccountItemId: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    CoinsuranceCompanyId: 0,
    CoinsuranceType: 0,
    CoinsuredCompanyId: 0,
    CheckingAccountConceptId: 0,
    AccountingNatureId: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    AccountingDate: null,
    CoinsuranceCheckingAccountId: 0,
    CoinsuranceCheckingAccountTransactionChild: []
};

var oCoinsuranceCheckingAccountItemChildUpdateModel = {
    CoinsuranceCheckingAccountItemId: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    CoinsuranceCompanyId: 0,
    CoinsuranceType: 0,
    CoinsuredCompanyId: 0,
    CheckingAccountConceptId: 0,
    AccountingNatureId: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    AccountingDate: null,
    CoinsuranceCheckingAccountId: 0
};
var oCoinsuranceCheckingAccountItemModel = {
    CoinsuranceCheckingAccountItemId: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    CoinsuranceCompanyId: 0,
    CoinsuranceType: 0,
    CoinsuredCompanyId: 0,
    CheckingAccountConceptId: 0,
    AccountingNatureId: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    AccountingDate: null,
    CoinsuranceCheckingAccountId: 0,
    CoinsuranceCheckingAccountTransactionChild: []
};
var oCoinsuranceCheckingAccountModel = {
    ImputationId: 0,
    CoinsuranceCheckingAccountTransactionItems: []
};
var coinsurerId = 0;
var coinsurerCompanyId = 0;
var editCoinsurance = -1;
var coinsuranceLocalAmount = 0;

var selectedCoinsurace;
var idControlCoinsurace = "";

var coinsuranceDocumentNumber = $('#CoinsuranceDocumentNumber').val();
var coinsuranceName = $('#CoinsuranceName').val();

$(document).ready(function () {
    /*---------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                        ACCIONES / EVENTOS                                                       */
    /*---------------------------------------------------------------------------------------------------------------------------------*/


    $("#modalCoinsurance").find("#coinsurancesListView").UifListView({
        autoHeight: true,
        source: ACC_ROOT + "CheckingAccountCoinsurances/GetTempCoinsuranceCheckingAccountItemByTempImputationId?tempImputationId=" + $("#ViewBagTempImputationId").val(),
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#coinsurance-display-template",
        deleteCallback: deleteCoinsuranceCallback
    });


    ///////////////////////////////////////////////////////
    /// Combo sucursal - coaseguradora                  ///
    ///////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#selectCoinsuranceBranch").on('itemSelected', function (event, selectedItem) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        // Se obtiene los puntos de venta
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#selectCoinsuranceSalePoint").UifSelect({
                source: controller
            });

            controller = ACC_ROOT + "Common/GetAccountingAccountConceptByBranchId?branchId=" +
                selectedItem.Id + "&sourceId=2";
            $("#selectCoinsuranceAccountingAccountConcept").UifSelect({
                source: controller
            });
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo concepto cuenta contable - coaseguradora  ///
    ///////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#selectCoinsuranceAccountingAccountConcept").on('itemSelected', function (event, selectedItem) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        if (selectedItem.Id > 0) {
            $.ajax({
                async: false,
                type: "GET",
                url: ACC_ROOT + "Common/GetAdditionalInformationConcept",
                data: {
                    "branchId": $("#selectCoinsuranceBranch").val(),
                    "conceptId": selectedItem.Id,
                    "sourceId": 2
                },
                success: function (data) {
                    if (data[0].ConceptId > 0) {
                        agentEnabled = data[0].AgentEnabled;
                        currencyId = data[0].CurrencyId;
                        itemsEnabled = data[0].ItemsEnabled;
                        paymentConceptId = data[0].PaymentConceptId;
                        percentageDifference = data[0].PercentageDiference;
                    } else {
                        agentEnabled = -1;
                        currencyId = -1;
                        itemsEnabled = -1;
                        paymentConceptId = -1;
                        percentageDifference = 0;
                    }
                }
            });
        }
    });


    ///////////////////////////////////////////////////////
    /// Combo moneda - coaseguradora                    ///
    ///////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#selectCoinsuranceCurrency").on('itemSelected', function (event, selectedItem) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        SetCurrency("selectCoinsuranceCurrency", "CoinsuranceExchangeRate");
        if ($("#CoinsuranceAmount").val() != "") {
            SetCoinsuranceLocalAmount();
        }
    });

    ///////////////////////////////////////////////////////
    /// Importe - coaseguradora                         ///
    ///////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#CoinsuranceAmount").blur(function () {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        if ($("#CoinsuranceAmount").val() != "") {
            var coinsuranceAmount = $("#CoinsuranceAmount").val();
            $("#CoinsuranceAmount").val("$ " + NumberFormatSearch(coinsuranceAmount, "2", ".", ","));
            SetCoinsuranceLocalAmount();
        }
    });

    ///////////////////////////////////////////////
    /// Autocomplete documento - coaseguradora  ///
    ///////////////////////////////////////////////
    $("#modalCoinsurance").find('#CoinsuranceDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        coinsurerId = selectedItem.Id;
        setValuesCoinsurance(selectedItem);
        selectedCoinsurace = selectedItem;
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#CoinsuranceDocumentNumber").on('blur', function (event) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        if (selectedCoinsurace != undefined) {
            if (selectedCoinsurace.TributaryIdCardNo != $('#CoinsuranceDocumentNumber').val()) {
                if ((idControlCoinsurace == "K") && ($('#CoinsuranceDocumentNumber').val() == "")) {
                    clearCoinsurace();
                    idControlCoinsurace = "";
                    selectedCoinsurace = undefined;
                    return;
                }
            }
            var selectedItem = selectedCoinsurace;
            setValuesCoinsurance(selectedItem);
        }
        else {
            clearCoinsurace();
        }

    });


    ////////////////////////////////////////////////////////
    // Control techa de borrado //
    ////////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#CoinsuranceDocumentNumber").on('keyup', function (event) {
        var tecla = (document.all) ? event.keyCode : event.which;
        if ((tecla == 8) || (tecla == 46)) {
            idControlCoinsurace = "K";
        }

    });

    ////////////////////////////////////////////////
    /// Autocomplete nombre - coaseguradora      ///
    ////////////////////////////////////////////////
    $("#modalCoinsurance").find('#CoinsuranceName').on('itemSelected', function (event, selectedItem) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        coinsurerId = selectedItem.Id;
        setValuesCoinsurance(selectedItem);
        selectedCoinsurace = selectedItem;
    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#CoinsuranceName").on('blur', function (event) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        if (selectedCoinsurace != undefined) {
            if (selectedCoinsurace.Description != $('#CoinsuranceName').val()) {
                if ((idControlCoinsurace == "K") && ($('#CoinsuranceName').val() == "")) {
                    clearCoinsurace();
                    idControlCoinsurace = "";
                    selectedCoinsurace = undefined;
                    return;
                }
            }
            var selectedItem = selectedCoinsurace;
            setValuesCoinsurance(selectedItem);
        }
        else {
            clearCoinsurace();
        }

    });

    ////////////////////////////////////////////////////////
    // Control techa de borrado //
    ////////////////////////////////////////////////////////
    $("#modalCoinsurance").find("#CoinsuranceName").on('keyup', function (event) {
        tecla = (document.all) ? event.keyCode : event.which;
        if ((tecla == 8) || (tecla == 46)) {
            idControlCoinsurace = "K";
        }

    });


    ///////////////////////////////////////////
    /// ListView - coaseguradora            ///
    ///////////////////////////////////////////
    $("#modalCoinsurance").find('#coinsurancesListView').on('rowEdit', function (event, data, index) {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        $("#CoinsuranceAmount").val(data.Amount);
        $("#CoinsuranceExchangeRate").val(data.ExchangeRate);
        $("#CoinsuranceLocalAmount").val(data.LocalAmount);
        $("#inputCheckIssuingBank").val(data.IssuingBankName);
        $("#CheckAccountNumber").val(data.IssuingBankAccountNumber);
        $("#HiddenCheckAccountNumber").val(data.IssuingBankAccountNumber);
        $("#CheckDocumentNumber").val(data.DocumentNumber);
        $("#HiddenCheckDocumentNumber").val(data.DocumentNumber);
        $("#CheckHolderName").val(data.IssuerName);
        $("#CheckDate").val(data.Date);
        coinsurerId = data.CoinsurerId;
        $("#HiddenIssuingBankId").val(data.CoinsurerId);

        $('#selectCoinsuranceType').val(data.CoinsuranceTypeId);
        $('#CoinsuranceMovementDescription').val(data.Description);
        $('#CoinsuranceDocumentNumber').val(data.CoinsurerDocumentNumber);
        $('#CoinsuranceName').val(data.CoinsurerName);
        $("#selectCoinsuranceSalePoint").UifSelect("setSelected", data.SalePointId);
        $('#selectCoinsuranceAccountingAccountConcept').UifSelect("setSelected", data.ConceptId);
        $('#selectCoinsuranceNature').UifSelect("setSelected", data.NatureId);
        $('#selectCoinsuranceCurrency').UifSelect("setSelected", data.CurrencyId);
        $('#selectCoinsuranceBranch').UifSelect("setSelected", data.BranchId);
        $('#selectCoinsuranceCompany').UifSelect("setSelected", data.CompanyId);
        $("#MovementBillNumber").val(data.BillNumber);

        editCoinsurance = index;
    });


    //////////////////////////////////////////////////////////////
    /// Botón agregar movimiento cta. cte. coaseguradora modal ///
    //////////////////////////////////////////////////////////////
    $("#modalCoinsurance").find('#CoinsuranceAdd').click(function () {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        var existMovement = 0;

        $("#addCoinsuranceForm").validate();

        if ($("#addCoinsuranceForm").valid()) {

            if (ValidateCoinsuranceAddForm() == true) {
                if (ValidateDuplicateCoinsurance() == 0) {
                    // Se valida que no se ingrese el mismo registro
                    var keyValid = $("#selectCoinsuranceBranch").val() + $("#selectCoinsuranceSalePoint").val() +
                        $("#selectCoinsuranceCompany").val() + coinsurerId + $("#CoinsuranceMovementDescription").val() +
                        $("#selectCoinsuranceAccountingAccountConcept").val() +
                        $("#selectCoinsuranceNature").val() + $("#selectCoinsuranceCurrency").val() + $("#CoinsuranceAmount").val();

                    var movements = $('#coinsurancesListView').UifListView("getData");

                    if (movements != null) {
                        for (var j = 0; j < movements.length; j++) {
                            var movementIndex = movements[j].BranchId + movements[j].SalePointId + movements[j].CompanyId +
                                movements[j].CoinsurerId + movements[j].Description + movements[j].ConceptId + movements[j].NatureId +
                                movements[j].CurrencyId + movements[j].Amount;
                            if (movementIndex == keyValid) {
                                existMovement = 1;
                                break;
                            }
                        }
                    }
                    if (existMovement == 0) {
                        var rowModel = new RowCoinsurerModel();

                        rowModel.BranchId = $('#selectCoinsuranceBranch').val();
                        rowModel.BranchName = $('#selectCoinsuranceBranch option:selected').text();
                        rowModel.SalePointId = $('#selectCoinsuranceSalePoint').val();
                        rowModel.SalePointName = $('#selectCoinsuranceSalePoint option:selected').text();
                        rowModel.CompanyId = $('#selectCoinsuranceCompany').val();
                        rowModel.CompanyName = $('#selectCoinsuranceCompany option:selected').text();
                        rowModel.CoinsuranceTypeId = $('#selectCoinsuranceType').val();
                        rowModel.CoinsuranceTypeName = $('#selectCoinsuranceType option:selected').text();
                        rowModel.Description = $('#CoinsuranceMovementDescription').val();
                        rowModel.CoinsurerId = coinsurerId;
                        rowModel.CoinsurerDocumentNumber = $('#CoinsuranceDocumentNumber').val();
                        rowModel.CoinsurerName = $('#CoinsuranceName').val();
                        rowModel.ConceptId = $('#selectCoinsuranceAccountingAccountConcept').val();
                        rowModel.ConceptName = $('#selectCoinsuranceAccountingAccountConcept option:selected').text();
                        rowModel.NatureId = $('#selectCoinsuranceNature').val();
                        rowModel.NatureName = $('#selectCoinsuranceNature option:selected').text();
                        rowModel.AccountingNature = $('#selectCoinsuranceNature').val();
                        rowModel.CurrencyId = $('#selectCoinsuranceCurrency').val();
                        rowModel.Currency = $('#selectCoinsuranceCurrency option:selected').text();
                        rowModel.ExchangeRate = $("#CoinsuranceExchangeRate").val();
                        rowModel.Amount = $('#CoinsuranceAmount').val();
                        rowModel.LocalAmount = coinsuranceLocalAmount;
                        rowModel.CoinsurerCheckingAccountItemId = 0;
                        rowModel.AgentTypeCode = agentTypeId;
                        rowModel.AgentAgencyCode = agentAgencyId;
                        rowModel.BillNumber = $("#MovementBillNumber").val();
                        rowModel.Status = "1";

                        $('#TotalCoinsuranceMovement').text(rowModel.Amount);

                        if (editCoinsurance == -1) {
                            $('#coinsurancesListView').UifListView("addItem", rowModel);
                        } else {
                            $('#coinsurancesListView').UifListView("editItem", editCoinsurance, rowModel);
                            editCoinsurance = -1;
                        }



                        $("#addCoinsuranceForm").formReset();
                        SetCoinsuranceTotalMovement();
                        SetCoinsuranceAccountingCompany();
                    } else {
                        $("#modalAgent").find("#alertForm").UifAlert('show', Resources.ValidateDuplicatePaymentMethods, "warning");
                    }
                } else {
                    var message = Resources.AgentsMessageValidateDuplicate + $("#MovementMessageDuplicate").val();
                    $("#modalCoinsurance").find("#alertForm").UifAlert('show', message, "warning");
                }
            }

            setTimeout(function () {
                SetCoinsuranceTotalMovement();
                SetCoinsuranceAccountingCompany();
            }, 1000);
        }
    });

    ////////////////////////////////////////////////////
    /// Botón Cerrar cta. cte. coaseguradora modal /////
    ////////////////////////////////////////////////////
    $('#modalCoinsurance').on('closed.modal', function () {

        if ($("#ViewBagImputationType").val() == 1) {
            GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount);
        }
        if ($("#ViewBagImputationType").val() == 2) {
            GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount);
        }
        if ($("#ViewBagImputationType").val() == 3) {
            GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount);
        }
        if ($("#ViewBagImputationType").val() == 4) {
            GetDebitsAndCreditsMovementTypes(tempImputationId, amount);
        }


        CheckLoadedMovements();
        loadedMovementsPromise.then(function (isLoaded) {
            if (isLoaded) {
                clearTimeout(time);
                if ($("#ViewBagImputationType").val() == 1) {
                    SetTotalApplicationReceipt();
                    SetTotalControlReceipt();
                }
                if ($("#ViewBagImputationType").val() == 2) {
                    SetTotalApplicationJournal();
                    SetTotalControlJournal();
                }
                if ($("#ViewBagImputationType").val() == 3) {
                    SetTotalApplicationPreLiquidation();
                    SetTotalControlPreLiquidation();
                }
                if ($("#ViewBagImputationType").val() == 4) {
                    SetTotalApplication();
                    SetTotalControl();
                }
            }
        });
    })
    /////////////////////////////////////////////////////
    /// Botón aceptar movimientos coaseguradora modal ///
    /////////////////////////////////////////////////////
    $("#modalCoinsurance").find('#CoinsuranceAcceptMovement').click(function () {
        $("#modalCoinsurance").find("#alertForm").UifAlert('hide');
        var movements = $("#coinsurancesListView").UifListView("getData");
        if (movements.length > 0) {
            movements.forEach(function (item, index) {
                if (ValidateAcceptButtonDuplicateCoinsurance(item) == 0) {
                    creditAmount = 0;
                    debitAmount = 0;
                    if (movements != null) {
                        for (var j = 0; j < movements.length; j++) {
                            if (movements[j].DebitCreditName == "Crédito") {
                                creditAmount = creditAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
                            } else {
                                debitAmount = debitAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
                            }
                        }
                    }

                    SetUpdateDataCoinsuranceCheckingAccount();

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "CheckingAccountCoinsurances/SaveTempCoinsuranceCheckingAccountRequest",
                        data: {
                            "coinsuranceCheckingAccount": SetDataCoinsuranceCheckingAccount(),
                            "status": 1
                        },
                        success: function () {
                            RefreshCoinsuranceMovements();
                        }
                    });

                    /*if ($("#ReceiptAmount").val() == "") {
                        $("#ReceiptAmount").val("0");
                    }

                    amount = $("#ReceiptAmount").val();
                    if (isNaN(amount)) {
                        amount = 0;
                    } else {
                        amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val()));
                    }*/

                    $('#modalCoinsurance').UifModal('hide');

                    if ($("#ViewBagImputationType").val() == 1) {
                        GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount);
                    }
                    if ($("#ViewBagImputationType").val() == 2) {
                        GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount);
                    }
                    if ($("#ViewBagImputationType").val() == 3) {
                        GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount);
                    }
                    if ($("#ViewBagImputationType").val() == 4) {
                        GetDebitsAndCreditsMovementTypes(tempImputationId, amount);
                    }


                    CheckLoadedMovements();
                    loadedMovementsPromise.then(function (isLoaded) {
                        if (isLoaded) {
                            clearTimeout(time);
                            if ($("#ViewBagImputationType").val() == 1) {
                                SetTotalApplicationReceipt();
                                SetTotalControlReceipt();
                            }
                            if ($("#ViewBagImputationType").val() == 2) {
                                SetTotalApplicationJournal();
                                SetTotalControlJournal();
                            }
                            if ($("#ViewBagImputationType").val() == 3) {
                                SetTotalApplicationPreLiquidation();
                                SetTotalControlPreLiquidation();
                            }
                            if ($("#ViewBagImputationType").val() == 4) {
                                SetTotalApplication();
                                SetTotalControl();
                            }
                        }
                    });
                } else {
                    var message = Resources.AgentsMessageValidateDuplicate + $("#MovementMessageDuplicate").val();
                    $("#modalCoinsurance").find("#alertForm").UifAlert('show', message, "warning");
                }
            });
        } else {
            $('#modalCoinsurance').UifModal('hide');

        }
    });
}); //fin ready


/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#selectCoinsuranceBranch").attr("disabled", "disabled");
    }, 300);
} else {
    $("#selectCoinsuranceBranch").removeAttr("disabled");
}
function LoadCoinsuresMovementsBranchs(branchUserDefault) {
    CoinsuranceMovementsRequest.GetBranchs().done(function (data) {
        if (branchUserDefault == null || branchUserDefault == 0) {
            $("#selectCoinsuranceBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectCoinsuranceBranch").UifSelect({ sourceData: data.data, selectedId: branchUserDefault });
        }
    });
}

function LoadCoinsuresMovementsSalesPointByBranchId(branchUserDefault, salePointBranchUserDefault) {
    CoinsuranceMovementsRequest.GetSalesPointByBranchId(branchUserDefault).done(function (data) {
        if (salePointBranchUserDefault == null || salePointBranchUserDefault == 0) {
            $("#selectCoinsuranceSalePoint").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectCoinsuranceSalePoint").UifSelect({ sourceData: data.data, selectedId: salePointBranchUserDefault });
        }
    });
}
function LoadCoinsuresMovementsAccountingAccountConceptByBranchId(branchUserDefault) {
    CoinsuranceMovementsRequest.GetAccountingAccountConceptByBranchId(branchUserDefault).done(function (data) {
        $("#selectCoinsuranceAccountingAccountConcept").UifSelect({ sourceData: data.data });
    });
}

function LoadCoinsuresMovementsCoinsuranceTypes() {
    CoinsuranceMovementsRequest.GetCoinsuranceTypes().done(function (data) {
        $("#selectCoinsuranceType").UifSelect({ sourceData: data.data });
    });
}
function LoadCoinsuresMovementsNatures() {
    CoinsuranceMovementsRequest.GetNatures().done(function (data) {
        $("#selectCoinsuranceNature").UifSelect({ sourceData: data.data });
    });
}
function LoadCoinsuresMovementsCurrencies() {
    CoinsuranceMovementsRequest.GetCurrencies().done(function (data) {
        $("#selectCoinsuranceCurrency").UifSelect({ sourceData: data.data });
    });
}
function LoadCoinsuresMovementsAccountingCompanies(idCompany) {
    CoinsuranceMovementsRequest.GetAccountingCompanies().done(function (data) {
        if (isMulticompany == 0)
            $("#selectCoinsuranceCompany").UifSelect({ sourceData: data.data, selectedId: idCompany, enable: false });
        else
            $("#selectCoinsuranceCompany").UifSelect({ sourceData: data.data });

    });
}

function setValuesCoinsurance(selectedItem) {
    if (selectedItem.Id > -1) {
        $('#CoinsuranceDocumentNumber').val(selectedItem.TributaryIdCardNo);
        $('#CoinsuranceName').val(selectedItem.Description);
        coinsurerCompanyId = selectedItem.Id;
        coinsuranceDocumentNumber = selectedItem.TributaryIdCardNo;
        coinsuranceName = selectedItem.Description;
        idControlCoinsurace = "S";
    } else {
        clearCoinsurace();
        idControlCoinsurace = "";
    }
}

function clearCoinsurace() {
    $('#CoinsuranceDocumentNumber').val("");
    $('#CoinsuranceName').val("");
    coinsurerCompanyId = -1;
}


//////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de cta.cte. coaseguradoras con los valores ingresados ///
//////////////////////////////////////////////////////////////////////////////
function SetDataCoinsuranceCheckingAccount() {
    SetDataCoinsuranceCheckingAccountEmpty();
    oCoinsuranceCheckingAccountModel.ImputationId = tempImputationId;

    var movements = $("#coinsurancesListView").UifListView("getData");
    if (movements != null) {
        for (var j = 0; j < movements.length; j++) {
            if (movements[j].Status == 1) {
                oCoinsuranceCheckingAccountItemModel = {
                    CoinsuranceCheckingAccountItemId: 0,
                    BranchId: 0,
                    SalePointId: 0,
                    AccountingCompanyId: 0,
                    CoinsuranceCompanyId: 0,
                    CoinsuranceType: 0,
                    CoinsuredCompanyId: 0,
                    CheckingAccountConceptId: 0,
                    AccountingNatureId: 0,
                    CurrencyCode: 0,
                    ExchangeRate: 0,
                    IncomeAmount: 0,
                    Amount: 0,
                    UserId: 0,
                    Description: null,
                    BillId: 0,
                    AccountingDate: null,
                    CoinsuranceCheckingAccountId: 0,
                    CoinsuranceCheckingAccountTransactionChild: []
                };
                oCoinsuranceCheckingAccountItemModel.BranchId = movements[j].BranchId;
                oCoinsuranceCheckingAccountItemModel.SalePointId = movements[j].SalePointId;
                oCoinsuranceCheckingAccountItemModel.AccountingCompanyId = movements[j].CompanyId;
                oCoinsuranceCheckingAccountItemModel.CoinsuredCompanyId = movements[j].CoinsurerId;
                oCoinsuranceCheckingAccountItemModel.CoinsuranceType = movements[j].CoinsuranceTypeId;
                oCoinsuranceCheckingAccountItemModel.CheckingAccountConceptId = movements[j].ConceptId;
                oCoinsuranceCheckingAccountItemModel.AccountingNatureId = movements[j].AccountingNature;
                oCoinsuranceCheckingAccountItemModel.CurrencyCode = movements[j].CurrencyId;
                oCoinsuranceCheckingAccountItemModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].ExchangeRate).replace(",", "."));
                oCoinsuranceCheckingAccountItemModel.IncomeAmount = ReplaceDecimalPoint(String(movements[j].LocalAmount).replace(",", "."));
                oCoinsuranceCheckingAccountItemModel.Amount = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].Amount).replace(",", "."));
                oCoinsuranceCheckingAccountItemModel.UserId = userId;
                oCoinsuranceCheckingAccountItemModel.Description = movements[j].Description;
                var coinsuranceItemsToApply = $("#coinsuranceItemsListView").UifListView("getData");

                if (coinsuranceItemsToApply != null) {
                    for (var k = 0; k < coinsuranceItemsToApply.length; k++) {
                        oCoinsuranceCheckingAccountItemChildModel = {
                            CoinsuranceCheckingAccountItemId: 0,
                            BranchId: 0,
                            SalePointId: 0,
                            AccountingCompanyId: 0,
                            CoinsuranceCompanyId: 0,
                            CoinsuranceType: 0,
                            CoinsuredCompanyId: 0,
                            CheckingAccountConceptId: 0,
                            AccountingNatureId: 0,
                            CurrencyCode: 0,
                            ExchangeRate: 0,
                            IncomeAmount: 0,
                            Amount: 0,
                            UserId: 0,
                            Description: null,
                            BillId: 0,
                            AccountingDate: null,
                            CoinsuranceCheckingAccountId: 0,
                        };
                        oCoinsuranceCheckingAccountItemChildModel.BranchId = movements[j].BranchId;
                        oCoinsuranceCheckingAccountItemChildModel.SalePointId = movements[j].SalePointId;
                        oCoinsuranceCheckingAccountItemChildModel.AccountingCompanyId = movements[j].CompanyId;
                        oCoinsuranceCheckingAccountItemChildModel.CoinsuredCompanyId = movements[j].CoinsurerId;
                        oCoinsuranceCheckingAccountItemChildModel.CoinsuranceType = movements[j].CoinsuranceTypeId;
                        oCoinsuranceCheckingAccountItemChildModel.CheckingAccountConceptId = movements[j].ConceptId;
                        oCoinsuranceCheckingAccountItemChildModel.AccountingNatureId = movements[j].AccountingNature;
                        oCoinsuranceCheckingAccountItemChildModel.CurrencyCode = movements[j].CurrencyId;
                        oCoinsuranceCheckingAccountItemChildModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].ExchangeRate).replace(",", "."));
                        oCoinsuranceCheckingAccountItemChildModel.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].LocalAmount).replace(",", "."));
                        oCoinsuranceCheckingAccountItemChildModel.Amount = ReplaceDecimalPoint(ClearFormatCurrency(coinsuranceItemsToApply[k].Amount).replace(",", "."));
                        oCoinsuranceCheckingAccountItemChildModel.UserId = userId;
                        oCoinsuranceCheckingAccountItemChildModel.Description = movements[j].Description;
                        oCoinsuranceCheckingAccountItemChildModel.CoinsuranceCheckingAccountId = coinsuranceItemsToApply[k].CoinsurerCheckingAccountItemId;

                        oCoinsuranceCheckingAccountItemModel.CoinsuranceCheckingAccountTransactionChild.push(oCoinsuranceCheckingAccountItemChildModel);
                    }
                }
                oCoinsuranceCheckingAccountModel.CoinsuranceCheckingAccountTransactionItems.push(oCoinsuranceCheckingAccountItemModel);
            }
        }
    }

    return oCoinsuranceCheckingAccountModel;
}

////////////////////////////////////////////////////////////////////////
/// Setea el modelo de cta.cte. coaseguradoras con valores iniciales ///
////////////////////////////////////////////////////////////////////////
function SetDataCoinsuranceCheckingAccountEmpty() {
    oCoinsuranceCheckingAccountModel = {
        ImputationId: 0,
        CoinsuranceCheckingAccountTransactionItems: []
    };

    oCoinsuranceCheckingAccountItemModel = {
        CoinsuranceCheckingAccountItemId: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        CoinsuranceCompanyId: 0,
        CoinsuranceType: 0,
        CoinsuredCompanyId: 0,
        CheckingAccountConceptId: 0,
        AccountingNatureId: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        AccountingDate: null,
        CoinsuranceCheckingAccountId: 0,
        CoinsuranceCheckingAccountTransactionChild: []
    };
}

////////////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de items cta.cte. coaseguradoras con los valores ingresados ///
////////////////////////////////////////////////////////////////////////////////////
function SetDataCoinsuranceCheckingAccountItem(coinsuranceTempCheckingAccountCode) {
    SetDataCoinsuranceCheckingAccountEmpty();
    oCoinsuranceCheckingAccountModel.ImputationId = tempImputationId;

    var coinsuranceItemsToApply = $("#coinsuranceItemsListView").UifListView("getData");

    oCoinsuranceCheckingAccountItemModel = {
        CoinsuranceCheckingAccountItemId: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        CoinsuranceCompanyId: 0,
        CoinsuranceType: 0,
        CoinsuredCompanyId: 0,
        CheckingAccountConceptId: 0,
        AccountingNatureId: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        AccountingDate: null,
        CoinsuranceCheckingAccountId: 0,
        CoinsuranceCheckingAccountTransactionChild: []
    };

    if (coinsuranceItemsToApply != null) {
        for (var k = 0; k < coinsuranceItemsToApply.length; k++) {
            oCoinsuranceCheckingAccountItemModel.CoinsuranceCheckingAccountItemId = coinsuranceTempCheckingAccountCode;

            oCoinsuranceCheckingAccountItemChildModel = {
                CoinsuranceCheckingAccountItemId: 0,
                BranchId: 0,
                SalePointId: 0,
                AccountingCompanyId: 0,
                CoinsuranceCompanyId: 0,
                CoinsuranceType: 0,
                CoinsuredCompanyId: 0,
                CheckingAccountConceptId: 0,
                AccountingNatureId: 0,
                CurrencyCode: 0,
                ExchangeRate: 0,
                IncomeAmount: 0,
                Amount: 0,
                UserId: 0,
                Description: null,
                BillId: 0,
                AccountingDate: null,
                CoinsuranceCheckingAccountId: 0
            };

            oCoinsuranceCheckingAccountItemChildModel.CoinsuranceCheckingAccountItemId = coinsuranceItemsToApply[k].CoinsuranceCheckingAccountItemId;
            oCoinsuranceCheckingAccountItemChildModel.CoinsuranceCheckingAccountId = coinsuranceItemsToApply[k].CoinsuranceCheckingAccountItemId;
            oCoinsuranceCheckingAccountItemModel.CoinsuranceCheckingAccountTransactionChild.push(oCoinsuranceCheckingAccountItemChildModel);
        }
    }

    oCoinsuranceCheckingAccountModel.CoinsuranceCheckingAccountTransactionItems.push(oCoinsuranceCheckingAccountItemModel);

    return oCoinsuranceCheckingAccountModel;
}

var saveCoinsuranceCallback = function (deferred, data) {
    $("#coinsurancesListView").UifListView("addItem", data);
    deferred.resolve();
};

var editCoinsuranceCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteCoinsuranceCallback = function (deferred, data) {
    deferred.resolve();
    editCoinsurance = -1;


    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckingAccountCoinsurances/DeleteTempCoinsuranceCheckingAccountItem",
        data: {
            "tempImputationCode": tempImputationId,
            "tempCoinsuranceCheckingAccountCode": data.CoinsurerCheckingAccountItemId
        },
        success: function () {
            setTimeout(function () {
                //RefreshCoinsuranceMovements();
                SetCoinsuranceTotalMovement();
            }, 1000);
        }
    });

    //SetCoinsuranceTotalMovement();

};

///////////////////////////////////////////////////////
// Setea el valor del importe local en coaseguradora //
///////////////////////////////////////////////////////
function SetCoinsuranceLocalAmount() {
    var coinsuranceAmount = $("#CoinsuranceAmount").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    var exchangeRate = $("#CoinsuranceExchangeRate").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    coinsuranceLocalAmount = coinsuranceAmount * exchangeRate;
    $("#CoinsuranceLocalAmount").val("$ " + NumberFormatSearch(coinsuranceLocalAmount, "2", ".", ","));
}

//////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios     //
//////////////////////////////////////////////////
function ValidateCoinsuranceAddForm() {
    if ($('#selectCoinsuranceBranch').val() == "") {
        $("#modalCoinsurance").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
        return false;
    }
    if ($('#CoinsuranceAmount').val() == "") {
        $("#modalCoinsurance").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
        return false;
    }
    if ($('#CoinsuranceDocumentNumber').val() == "") {
        $("#modalCoinsurance").find("#alertForm").UifAlert('show', Resources.ValidateReinsurerEqualBroker, "warning");
        return false;
    }

    return true;
}

////////////////////////////////////////////////////////////////
/// Setear el total de la listview movimientos coaseguradora ///
////////////////////////////////////////////////////////////////
function SetCoinsuranceTotalMovement() {
    setTimeout(function () {
        var totalMovement = 0;
        var totalDebitMovement = 0;

        var coinsurances = $("#modalCoinsurance").find("#coinsurancesListView").UifListView("getData");

        if (coinsurances != null) {

            for (var j = 0; j < coinsurances.length; j++) {
                if (coinsurances[j].NatureId == 1) {
                    var coinsuranceCredit = String(coinsurances[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                    totalMovement += parseFloat(coinsuranceCredit);
                } else {
                    var coinsuranceDebit = String(coinsurances[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                    totalDebitMovement += parseFloat(coinsuranceDebit);
                }
            }
        } else {
            $("#modalCoinsurance").find("#TotalCoinsuranceMovement").text("");
            $("#modalCoinsurance").find("#TotalDebitCoinsuranceMovement").text("");
        }
        $("#modalCoinsurance").find("#TotalCoinsuranceMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
        $("#modalCoinsurance").find("#TotalDebitCoinsuranceMovement").text("$ " + NumberFormatSearch(totalDebitMovement, "2", ".", ","));
    }, 100);
}

//////////////////////////////////////////////////////////
/// Validación de duplicados movimientos coaseguradora ///
//////////////////////////////////////////////////////////
function ValidateDuplicateCoinsurance() {
    var duplicate = -1;
    var checkingAccountConceptId = 0;

    if (!($("#selectCoinsuranceAccountingAccountConcept").val() == null))
        checkingAccountConceptId = $("#selectCoinsuranceAccountingAccountConcept").val();
    if (editCoinsurance > -1) {
        duplicate = 0;
        $("#MovementMessageDuplicate").val("");
    } else {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "CheckingAccountCoinsurances/ValidateDuplicateCoinsuranceCheckingAccount",

            data: {
                "branchId": $("#selectCoinsuranceBranch").val(),
                "salePointId": $("#selectCoinsuranceSalePoint").val(),
                "companyId": $("#selectCoinsuranceCompany").val(),
                "coinsuranceId": coinsurerId,
                "accountingNatureId": $("#selectCoinsuranceNature").val(),
                "checkingAccountConceptId": checkingAccountConceptId, //$("#selectCoinsuranceAccountingAccountConcept").val(),
                "currencyId": $("#selectCoinsuranceCurrency").val(),
                "coinsuranceTypeId": $("#selectCoinsuranceType").val()
            },
            success: function (data) {
                if (data[0].source == -1) {
                    duplicate = 0;
                    $("#MovementMessageDuplicate").val("");
                }
                else if (data[0].source >= 0) {
                    duplicate = 1;
                    if (data[0].source == 0) {
                        $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + ":" + data[0].imputationId);
                    }
                    else if (data[0].source == 1) {
                        $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + data[0].imputationId);
                    }
                }
            }
        });
    }
    return duplicate;
}

////////////////////////////////////////////////////////////////////////
/// Validación boton acpetar de duplicados movimientos coaseguradora ///
////////////////////////////////////////////////////////////////////////
function ValidateAcceptButtonDuplicateCoinsurance(coasegData) {
    var duplicate = -1;
    var checkingAccountConceptId = 0;

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckingAccountCoinsurances/ValidateDuplicateCoinsuranceCheckingAccount",

        data: {
            "branchId": coasegData.BranchId,
            "salePointId": coasegData.SalePointId,
            "companyId": coasegData.CompanyId,
            "coinsuranceId": coasegData.CoinsurerId,
            "accountingNatureId": coasegData.NatureId,
            "checkingAccountConceptId": coasegData.ConceptId,
            "currencyId": coasegData.CurrencyId,
            "coinsuranceTypeId": coasegData.CoinsuranceTypeId
        },
        success: function (data) {
            if (data[0].source == -1) {
                duplicate = 0;
                $("#MovementMessageDuplicate").val("");
            }
            else if (data[0].source >= 0) {
                duplicate = 1;
                if (data[0].source == 0) {
                    $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + ":" + data[0].imputationId);
                }
                else if (data[0].source == 1) {
                    $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + data[0].imputationId);
                }
            }
        }
    });





    return duplicate;
}


///////////////////////////////////////////////////////////////////
/// Realiza la consulta de movimientos cta. cte. coaseguradoras ///
///////////////////////////////////////////////////////////////////
function RefreshCoinsuranceMovements() {
    $("#coinsurancesListView").UifListView({
        autoHeight: true,
        source: ACC_ROOT + "CheckingAccountCoinsurances/GetTempCoinsuranceCheckingAccountItemByTempImputationId?tempImputationId=" + tempImputationId,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#coinsurance-display-template",
        deleteCallback: deleteCoinsuranceCallback
    });

    //setTimeout(function () {
    SetCoinsuranceTotalMovement();
    //}, 1000);
}

/////////////////////////////////////////////////////////////////
/// Setea la compañía y sucursal por default - coaseguradoras ///
/////////////////////////////////////////////////////////////////
function SetCoinsuranceAccountingCompany() {
    //debugger
    if (isMulticompany == 0) {
        $("#selectCoinsuranceCompany").val(accountingCompanyDefault);
        $("#selectCoinsuranceCompany").attr("disabled", "disabled");
    } else {
        $("#selectCoinsuranceCompany").removeAttr("disabled");
    }
    if (branchUserDefault > 0) {
        $("#selectCoinsuranceBranch").val(branchUserDefault);

        if (branchUserDefault) {
            LoadCoinsuresMovementsSalesPointByBranchId(branchUserDefault, null)//jira SMT-2000 quitar puntos de venta por defecto.
            LoadCoinsuresMovementsAccountingAccountConceptByBranchId(branchUserDefault);
        }
    } else {
        $("#selectCoinsuranceBranch").val("");
    }
}

////////////////////////////////////////////////////////////////////////////
/// Setea los valores de los items a aplicar de cta. cte. reaseguradoras ///
////////////////////////////////////////////////////////////////////////////
function SetUpdateDataCoinsuranceCheckingAccount() {
    oCoinsuranceCheckingAccountUpdateModel = {
        ImputationId: 0,
        CoinsuranceCheckingAccountTransactionItems: []
    };

    oCoinsuranceCheckingAccountItemUpdateModel = {
        CoinsuranceCheckingAccountItemId: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        CoinsuranceCompanyId: 0,
        CoinsuranceType: 0,
        CoinsuredCompanyId: 0,
        CheckingAccountConceptId: 0,
        AccountingNatureId: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        AccountingDate: null,
        CoinsuranceCheckingAccountId: 0
    };

    // Items a aplicar
    var coinsuranceItemsToApply = $("#coinsuranceItemsListView").UifListView("getData");

    if (coinsuranceItemsToApply != null) {
        for (var k = 0; k < coinsuranceItemsToApply.length; k++) {
            if (coinsuranceItemsToApply[k].Status == 1) {
                oCoinsuranceCheckingAccountUpdateModel.ImputationId = tempImputationId;
                oCoinsuranceCheckingAccountItemUpdateModel = {
                    CoinsuranceCheckingAccountItemId: 0,
                    BranchId: 0,
                    SalePointId: 0,
                    AccountingCompanyId: 0,
                    CoinsuranceCompanyId: 0,
                    CoinsuranceType: 0,
                    CoinsuredCompanyId: 0,
                    CheckingAccountConceptId: 0,
                    AccountingNatureId: 0,
                    CurrencyCode: 0,
                    ExchangeRate: 0,
                    IncomeAmount: 0,
                    Amount: 0,
                    UserId: 0,
                    Description: null,
                    BillId: 0,
                    AccountingDate: null,
                    CoinsuranceCheckingAccountId: 0
                };

                oCoinsuranceCheckingAccountItemUpdateModel.CoinsuranceCheckingAccountItemId = coinsuranceItemsToApply[k].CoinsuranceCheckingAccountItemId;

                oCoinsuranceCheckingAccountItemUpdateModel.BranchId = coinsuranceItemsToApply[k].BranchCode;
                oCoinsuranceCheckingAccountItemUpdateModel.SalePointId = coinsuranceItemsToApply[k].PosCode;
                oCoinsuranceCheckingAccountItemUpdateModel.AccountingCompanyId = coinsuranceItemsToApply[k].CompanyCode;
                oCoinsuranceCheckingAccountItemUpdateModel.CoinsuredCompanyId = coinsuranceItemsToApply[k].CoinsuranceCompanyCode;
                oCoinsuranceCheckingAccountItemUpdateModel.CoinsuranceType = coinsuranceItemsToApply[k].CoinsuranceType;
                oCoinsuranceCheckingAccountItemUpdateModel.CheckingAccountConceptId = coinsuranceItemsToApply[k].CheckingAccountConceptCode;
                oCoinsuranceCheckingAccountItemUpdateModel.AccountingNatureId = coinsuranceItemsToApply[k].AccountingNature;
                oCoinsuranceCheckingAccountItemUpdateModel.CurrencyCode = coinsuranceItemsToApply[k].CurrencyCode;
                oCoinsuranceCheckingAccountItemUpdateModel.ExchangeRate = ClearFormatCurrency(coinsuranceItemsToApply[k].CurrencyChange).replace(",", ".");
                oCoinsuranceCheckingAccountItemUpdateModel.IncomeAmount = ClearFormatCurrency(coinsuranceItemsToApply[k].Amount).replace(",", ".");
                oCoinsuranceCheckingAccountItemUpdateModel.Amount = ClearFormatCurrency(coinsuranceItemsToApply[k].Amount).replace(",", ".");
                oCoinsuranceCheckingAccountItemUpdateModel.UserId = userId;
                oCoinsuranceCheckingAccountItemUpdateModel.Description = coinsuranceItemsToApply[k].Description;

                oCoinsuranceCheckingAccountUpdateModel.CoinsuranceCheckingAccountTransactionItems.push(oCoinsuranceCheckingAccountItemUpdateModel);
            }
        }
    }

    return oCoinsuranceCheckingAccountUpdateModel;
}

/////////////////////////////////////////////////////////////
/// Setea los campos a valores iniciales - coaseguradoras ///
/////////////////////////////////////////////////////////////
function SetCoinsuranceFieldEmpty() {
    var coinsurerCompanyId = "";
    var coinsuranceDocumentNumber = "";
    var coinsuranceName = "";
    $("#selectCoinsuranceBranch").val("");
    $("#selectCoinsuranceSalePoint").val("");
    $("#selectCoinsuranceCompany").val("");
    $("#selectCoinsuranceType").val("");
    $("#CoinsuranceMovementDescription").val("");
    $("#CoinsuranceDocumentNumber").UifAutoComplete('clean');
    $("#CoinsuranceName").UifAutoComplete('clean');
    $("#selectCoinsuranceAccountingAccountConcept").val("");
    $("#selectCoinsuranceNature").val("");
    $("#selectCoinsuranceCurrency").val("");
    $("#CoinsuranceExchangeRate").val("");
    $("#CoinsuranceAmount").val("");
    $("#CoinsuranceLocalAmount").val("");
    coinsuranceLocalAmount = 0;
    ClearValidation("#addCoinsuranceForm");
}
