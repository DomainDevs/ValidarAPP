
/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var tempImputationId;
var branchDisable;
var branchId = 0;
var paymentMethod = 0;
var oBrokerCheckingAccountModel = null;
var oBrokerCheckingAccountItemModel = null;

setTimeout(function () {
    tempImputationId = $("#ViewBagTempImputationId").val();
    branchDisable = $("#ViewBagBranchDisable").val();
}, 2000);


function RowAgentModel() {
    this.BranchId;
    this.BranchName;
    this.SalePointId;
    this.SalePointName;
    this.CompanyId;
    this.CompanyName;
    this.Description;
    this.AgentId;
    this.AgentDocumentNumber;
    this.AgentName;
    this.ConceptId;
    this.ConceptName;
    this.NatureId;
    this.NatureName;
    this.AccountingNature;
    this.CurrencyId;
    this.CurrencyName;
    this.ExchangeRate;
    this.Amount;
    this.LocalAmount;
    this.BrokerCheckingAccountItemId;
    this.AgentTypeCode;
    this.AgentAgencyCode;
    this.BillNumber;
    this.TempBrokerParentId;
    this.Items;
    this.Status;
}

var oBrokerCheckingAccountUpdateModel = {
    ImputationId: 0,
    BrokersCheckingAccountTransactionItems: []
};

var oBrokerCheckingAccountItemUpdateModel = {
    BrokerCheckingAccountItemId: 0,
    AgentTypeId: 0,
    AgentId: 0,
    AgentAgencyId: 0,
    AccountingNature: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    AccountingDate: null,
    CheckingAccountConceptId: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    PolicyId: 0,
    PrefixId: 0,
    InsuredId: 0,
    CommissionPercentage: 0,
    CommissionType: 0,
    CommissionAmount: 0,
    DiscountedCommission: 0,
    CommissionBalance: 0,
    BrokersCheckingAccountTransactionChild: []
};

var oBrokerCheckingAccountItemChildUpdateModel = {
    BrokerCheckingAccountItemId: 0,
    AgentTypeId: 0,
    AgentId: 0,
    AgentAgencyId: 0,
    AccountingNature: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    AccountingDate: null,
    CheckingAccountConceptId: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    BrokerCheckingAccountId: 0,
    PolicyId: 0,
    PrefixId: 0,
    InsuredId: 0,
    CommissionPercentage: 0,
    CommissionType: 0,
    CommissionAmount: 0,
    DiscountedCommission: 0,
    CommissionBalance: 0
};

var agentId = 0;
var agentEnabled = -1;
var itemsEnabled = 0;
var currencyId = -1;
var paymentConceptId = -1;
var percentageDifference = 0;
var agentType = 0;
var agentTypeId = 0;
var agentAgencyId = 0;
var editAgent = -1;

var agentDocumentNumber = $('#AgentDocumentNumber').val();
var agentName = $('#AgentName').val();

$(document).ready(function () {

    /*---------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                        ACCIONES / EVENTOS                                                       */
    /*---------------------------------------------------------------------------------------------------------------------------------*/



    ////////////////////////////////////////
    // Autocomplete nombre - agente       //
    ////////////////////////////////////////
    $("#modalAgent").find('#AgentName').on('itemSelected', function (event, selectedItem) {
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        agentId = selectedItem.IndividualId;
        if (agentId > 0) {
            $('#AgentDocumentNumber').val(selectedItem.DocumentNumber);
            $('#AgentName').val(selectedItem.Name);
            agentType = selectedItem.AgentType;
            agentTypeId = selectedItem.AgentTypeId;
            agentAgencyId = selectedItem.AgentAgencyId;
            branchId = selectedItem.BranchId;
            agentDocumentNumber = selectedItem.DocumentNumber;
            agentName = selectedItem.Name;
        } else {
            $('#AgentDocumentNumber').val("");
            $('#AgentName').val("");
            agentType = 0;
            agentTypeId = 0;
            agentAgencyId = 0;
            branchId = -1;
        }
    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalAgent").find("#AgentName").on('blur', function (event) {
        setTimeout(function () {
            $('#AgentName').val(agentName);
        }, 50);
    });

    $("#modalAgent").find("#agentsListView").UifListView(
        {
            autoHeight: true,
            //source: ACC_ROOT + "CheckingAccountBrokers/GetTempBrokerCheckingAccountItemByTempImputationId?tempImputationId=" + tempImputationId,
            source: ACC_ROOT + "CheckingAccountBrokers/GetTempBrokerCheckingAccountItemByTempImputationId?tempImputationId=" + $("#ViewBagTempImputationId").val(),
            customDelete: false,
            customAdd: false,
            customEdit: true,
            add: false,
            //Inicio SMT-1994
            edit: true,
            //Fin SMT-1994
            delete: true,
            displayTemplate: "#agent-display-template",
            deleteCallback: deleteAgentCallback
        });

    ///////////////////////////////////////////////////////
    /// Combo sucursal - agente                         ///
    ///////////////////////////////////////////////////////
    $("#modalAgent").find("#selectAgentBranch").on('itemSelected', function (event, selectedItem) {
        // debugger
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        // Se obtiene los puntos de venta
        if (selectedItem.Id > 0) {
            LoadAgentSalesPointByBranchId(selectedItem.Id)
            LoadAgentAccountingAccountConceptByBranchId(selectedItem.Id);
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo concepto cuenta contable - agente         ///
    ///////////////////////////////////////////////////////
    $("#modalAgent").find("#selectAgentAccountingAccountConcept").on('itemSelected', function (event, selectedItem) {
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        if (selectedItem.Id > 0) {

            lockScreen();
            setTimeout(function () {
                $.ajax({
                    async: false,
                    type: "GET",
                    url: ACC_ROOT + "Common/GetAdditionalInformationConcept",
                    data: {
                        "branchId": $("#selectAgentBranch").val(),
                        "conceptId": selectedItem.Id, "sourceId": 1
                    },
                    success: function (data) {

                        unlockScreen();

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
            }, 500);
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo moneda - agente                           ///
    ///////////////////////////////////////////////////////
    $("#modalAgent").find("#selectAgentCurrency").on('itemSelected', function (event, selectedItem) {
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        SetCurrency("selectAgentCurrency", "AgentExchangeRate");
        if ($("#AgentAmount").val() != "") {
            SetAgentLocalAmount();
        }
    });

    ///////////////////////////////////////////////////////
    /// Importe - agente                                ///
    ///////////////////////////////////////////////////////
    $("#modalAgent").find("#AgentAmount").blur(function () {
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        if ($("#AgentAmount").val() != "") {
            var agentAmount = $("#AgentAmount").val();
            $("#AgentAmount").val("$ " + NumberFormatSearch(agentAmount, "2", ".", ","));
            SetAgentLocalAmount();
        }
    });

    ////////////////////////////////////////
    // Autocomplete documento - agente    //
    ////////////////////////////////////////
    $("#modalAgent").find('#AgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        agentId = selectedItem.IndividualId;
        if (agentId > 0) {
            $('#AgentDocumentNumber').val(selectedItem.DocumentNumber);
            $('#AgentName').val(selectedItem.Name);
            agentType = selectedItem.AgentType;
            agentTypeId = selectedItem.AgentTypeId;
            agentAgencyId = selectedItem.AgentAgencyId;
            branchId = selectedItem.BranchId;
            agentDocumentNumber = selectedItem.DocumentNumber;
            agentName = selectedItem.Name;
            $("#AgentDocumentNumber");
        } else {
            $('#AgentDocumentNumber').val("");
            $('#AgentName').val("");
            agentType = 0;
            agentTypeId = 0;
            agentAgencyId = 0;
            branchId = -1;
        }
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalAgent").find("#AgentDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#AgentDocumentNumber').val(agentDocumentNumber);
        }, 50);
    });

    /////////////////////////////////////
    // ListView - agente               //
    /////////////////////////////////////
    $("#modalAgent").find('#agentsListView').on('rowEdit', function (event, data, index) {
        $("#selectAgentCurrency").val(data.CurrencyId);
        $("#AgentAmount").val(data.Amount);
        $("#AgentExchangeRate").val(data.ExchangeRate);
        $("#AgentLocalAmount").val(data.LocalAmount);
        $("#inputCheckIssuingBank").val(data.IssuingBankName);
        $("#CheckAccountNumber").val(data.IssuingBankAccountNumber);
        $("#HiddenCheckAccountNumber").val(data.IssuingBankAccountNumber);
        $("#CheckDocumentNumber").val(data.DocumentNumber);
        $("#HiddenCheckDocumentNumber").val(data.DocumentNumber);
        $("#CheckHolderName").val(data.IssuerName);
        $("#CheckDate").val(data.Date);
        agentId = data.IssuingBankId;
        $("#HiddenIssuingBankId").val(data.IssuingBankId);
        //Inicio SMT-1994
        $('#AgentMovementDescription').val(data.Description);
        $('#AgentDocumentNumber').val(data.AgentDocumentNumber);
        $('#AgentName').val(data.AgentName);
        $('#selectAgentAccountingAccountConcept').UifSelect("setSelected", data.ConceptId);
        $('#selectAgentNature').UifSelect("setSelected", data.NatureId);
        $('#selectAgentCurrency').UifSelect("setSelected", data.CurrencyId);
        $('#selectAgentSalePoint').UifSelect("setSelected", data.SalePointId);
        $('#selectAgentBranch').UifSelect("setSelected", data.BranchId);
        //Fin SMT-1994

        editAgent = index;
    });

    $("#modalAgent").find('#agentsListView').on('rowDelete', function (event, data) {
        SetAgentTotalMovement();
    });

    //////////////////////////
    // Botón agregar modal ///
    //////////////////////////
    $("#modalAgent").find('#AgentAdd').click(function () {
        paymentMethod = "C";
        var existMovement = 0;
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        //$("#addAgentForm").validate();

        if ($("#addAgentForm").valid()) {
            if (ValidateAgentAddForm() == true) {
                if (ValidateDuplicateBroker() == 0) {
                    // Se valida que no se ingrese el mismo registro
                    var keyValid = $("#selectAgentBranch").val() + $("#selectAgentSalePoint").val() +
                        $("#selectAgentCompany").val() + agentId +
                        $("#selectAgentAccountingAccountConcept").val() +
                        $("#selectAgentNature").val() + $("#selectAgentCurrency").val();

                    var movements = $('#agentsListView').UifListView("getData");

                    if (movements != null) {
                        for (var j = 0; j < movements.length; j++) {
                            var movementIndex = movements[j].BranchId + movements[j].SalePointId + movements[j].CompanyId +
                                movements[j].AgentId + movements[j].ConceptId + movements[j].NatureId +
                                movements[j].CurrencyId;
                            if (movementIndex == keyValid) {
                                existMovement = 1;
                                break;
                            }
                        }
                    }

                    if (editAgent > -1) {
                        if (($("#selectAgentBranch").val() == $("#HiddenBranchId").val()) &&
                            ($("#selectAgentSalePoint").val() == $("#HiddenSalePointId").val()) &&
                            (agentId == $("#HiddenAgentId").val())) {
                            existMovement = 0;
                        }
                    }

                    if (existMovement == 0) {
                        var rowModel = new RowAgentModel();
                        rowModel.BranchId = $('#selectAgentBranch').val();
                        rowModel.BranchName = $('#selectAgentBranch option:selected').text();
                        rowModel.SalePointId = $('#selectAgentSalePoint').val();
                        rowModel.SalePointName = $('#selectAgentSalePoint option:selected').text();
                        rowModel.CompanyId = $('#selectAgentCompany').val();
                        rowModel.CompanyName = $('#selectAgentCompany option:selected').text();
                        rowModel.Description = $('#AgentMovementDescription').val();
                        rowModel.AgentId = agentId;
                        rowModel.AgentDocumentNumber = $('#AgentDocumentNumber').val();
                        rowModel.AgentName = $('#AgentName').val();
                        rowModel.ConceptId = $('#selectAgentAccountingAccountConcept').val();
                        rowModel.ConceptName = $('#selectAgentAccountingAccountConcept option:selected').text();
                        rowModel.NatureId = $('#selectAgentNature').val();
                        rowModel.NatureName = $('#selectAgentNature option:selected').text();
                        rowModel.AccountingNature = $('#selectAgentNature').val();
                        rowModel.CurrencyId = $('#selectAgentCurrency').val();
                        rowModel.Currency = $('#selectAgentCurrency option:selected').text();
                        rowModel.ExchangeRate = $("#AgentExchangeRate").val();
                        rowModel.Amount = $('#AgentAmount').val();
                        rowModel.LocalAmount = $("#AgentLocalAmount").val();
                        rowModel.BrokerCheckingAccountItemId = 0;
                        rowModel.AgentTypeCode = agentTypeId;
                        rowModel.AgentAgencyCode = agentAgencyId;
                        rowModel.BillNumber = $("#MovementBillNumber").val();
                        rowModel.Status = "1";

                        $('#TotalAgentMovementCredit').text(rowModel.Amount);

                        if (editAgent == -1) {
                            $('#agentsListView').UifListView("addItem", rowModel);
                        } else {
                            $('#agentsListView').UifListView("editItem", editAgent, rowModel);
                            editAgent = -1;
                        }



                        agentDocumentNumber = "";
                        agentName = "";
                        $("#addAgentForm").formReset();
                        SetAgentTotalMovement();
                        SetAgentAccountingCompany();
                    } else {
                        $("#modalAgent").find("#alertForm").UifAlert('show', Resources.ValidateDuplicatePaymentMethods, "warning");
                    }
                } else {
                    var message = Resources.AgentsMessageValidateDuplicate + " " + $("#MovementMessageDuplicate").val();
                    $("#modalAgent").find("#alertForm").UifAlert('show', message, "warning");
                }
            }

            setTimeout(function () {
                SetAgentTotalMovement();
                SetAgentAccountingCompany();
            }, 2000);

        }
    });

    $('#modalAgent').on('closed.modal', function () {
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


    /////////////////////////////////////////////
    // Botón aceptar movimientos agente modal ///
    /////////////////////////////////////////////
    $("#modalAgent").find('#AgentAcceptMovement').click(function () {
        $("#modalAgent").find("#alertForm").UifAlert('hide');
        var movements = $("#agentsListView").UifListView("getData");
        if (movements.length > 0) {
            movements.forEach(function (item, index) {
                if (ValidateAceptButtonDuplicateAgent(item) == 0) {
                    creditAmount = 0;
                    debitAmount = 0;
                    if (movements != null) {
                        for (var j = 0; j < movements.length; j++) {
                            if (movements[j].NatureName == "Crédito") {
                                creditAmount = parseFloat(creditAmount) + MoneyFormattoNumber(movements[j].Amount);
                            } else {
                                debitAmount = parseFloat(debitAmount) + MoneyFormattoNumber(movements[j].Amount);
                            }
                        }
                    }


                    SetUpdateDataBrokersCheckingAccount();
                    if ($("#ReceiptAmount").val() == "") {
                        $("#ReceiptAmount").val("0");
                    }
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "CheckingAccountBrokers/SaveTempBrokersCheckingAccountRequest",
                        data: { "brokerCheckingAccount": SetDataBrokersCheckingAccount(), "status": 1 }
                    }).done(function () {
                        RefreshAgentMovements();
                    });


                    //amount = $("#ReceiptAmount").val();
                    //if (isNaN(amount)) {
                    //    amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val()));
                    //}
                    //else {
                    //    amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val()));
                    //}
                    //$("#agentsListView").UifListView("clear");
                    $('#modalAgent').UifModal('hide');

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
        }
        else {
            $('#modalAgent').UifModal('hide');
           
        }
        
        
    });
});//fin $(document).ready

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

if (branchDisable == "1") {
    setTimeout(function () {
        $("#selectAgentBranch").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#selectAgentBranch").removeAttr("disabled");
}

/*-----------------------------------------------------------------------------------------------------*/
/*-------------------------------------Modal Agentes --------------------------------------------------*/
/*-----------------------------------------------------------------------------------------------------*/
function LoadAgentSalesPointByBranchId(branchUserDefault, salePointBranchUserDefault) {
    AgentMovementsRequest.GetSalesPointByBranchId(branchUserDefault).done(function (data) {
        if (salePointBranchUserDefault == null || salePointBranchUserDefault == 0) {
            $("#selectAgentSalePoint").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectAgentSalePoint").UifSelect({ sourceData: data.data, selectedId: salePointBranchUserDefault });
        }
    });
}

function LoadAgentAccountingAccountConceptByBranchId(branchUserDefault) {
    AgentMovementsRequest.GetAccountingAccountConceptByBranchId(branchUserDefault).done(function (data) {
        $("#selectAgentAccountingAccountConcept").UifSelect({ sourceData: data.data });
    });
}

function LoadAgentNatures() {
    AgentMovementsRequest.GetNatures().done(function (data) {
        $("#selectAgentNature").UifSelect({ sourceData: data.data });
    });
}
function LoadAgentCurrencies() {
    AgentMovementsRequest.GetCurrencies().done(function (data) {
        $("#selectAgentCurrency").UifSelect({ sourceData: data.data });
    });
}
function LoadAgentBranchs(branchUserDefault) {
    AgentMovementsRequest.GetBranchs().done(function (data) {
        if (branchUserDefault == null || branchUserDefault == 0) {
            $("#selectAgentBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectAgentBranch").UifSelect({ sourceData: data.data, selectedId: branchUserDefault });
        }
    });
}
function LoadAgentCompanies() {
    AgentMovementsRequest.GetAccoutingCompanies().done(function (data) {
        $("#AccountingCompany").UifSelect({ sourceData: data.data });
    });
}


///////////////////////////////////////////////////////////////////////
/// Setear el modelo de cta.cte. agentes con los valores ingresados ///
///////////////////////////////////////////////////////////////////////
function SetDataBrokersCheckingAccount() {
    SetDataBrokersCheckingAccountEmpty();

    oBrokerCheckingAccountModel.ImputationId = tempImputationId;

    var movements = $("#agentsListView").UifListView("getData");

    if (movements != null) {
        for (var j = 0; j < movements.length; j++) {

            if (movements[j].Status == 1) {
                oBrokerCheckingAccountItemModel = {
                    BrokerCheckingAccountItemId: 0,
                    AgentTypeId: 0,
                    AgentId: 0,
                    AgentAgencyId: 0,
                    AccountingNature: 0,
                    BranchId: 0,
                    SalePointId: 0,
                    AccountingCompanyId: 0,
                    AccountingDate: null,
                    CheckingAccountConceptId: 0,
                    CurrencyCode: 0,
                    ExchangeRate: 0,
                    IncomeAmount: 0,
                    Amount: 0,
                    UserId: 0,
                    Description: null,
                    BillId: 0,
                    PolicyId: 0,
                    PrefixId: 0,
                    InsuredId: 0,
                    CommissionPercentage: 0,
                    CommissionType: 0,
                    CommissionAmount: 0,
                    DiscountedCommission: 0,
                    CommissionBalance: 0,
                    BrokersCheckingAccountTransactionChild: []
                };

                oBrokerCheckingAccountItemModel.BrokerCheckingAccountItemId = movements[j].BrokerCheckingAccountItemId;
                oBrokerCheckingAccountItemModel.AgentTypeId = movements[j].AgentTypeCode;
                oBrokerCheckingAccountItemModel.AgentId = movements[j].AgentId;
                oBrokerCheckingAccountItemModel.AgentAgencyId = movements[j].AgentAgencyCode;
                oBrokerCheckingAccountItemModel.AccountingNature = movements[j].AccountingNature;
                oBrokerCheckingAccountItemModel.BranchId = movements[j].BranchId;
                oBrokerCheckingAccountItemModel.SalePointId = movements[j].SalePointId;
                oBrokerCheckingAccountItemModel.AccountingCompanyId = movements[j].CompanyId;
                oBrokerCheckingAccountItemModel.AccountingDate = getCurrentDate();
                oBrokerCheckingAccountItemModel.CheckingAccountConceptId = movements[j].ConceptId;
                oBrokerCheckingAccountItemModel.CurrencyCode = movements[j].CurrencyId;
                oBrokerCheckingAccountItemModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].ExchangeRate).replace(",", "."));
                oBrokerCheckingAccountItemModel.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].LocalAmount).replace(",", "."));
                oBrokerCheckingAccountItemModel.Amount = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].Amount).replace(",", "."));
                oBrokerCheckingAccountItemModel.UserId = userId;
                oBrokerCheckingAccountItemModel.Description = movements[j].Description;
                oBrokerCheckingAccountItemModel.BillId = movements[j].BillNumber;
                oBrokerCheckingAccountItemModel.PolicyId = 0;
                oBrokerCheckingAccountItemModel.PrefixId = 0;
                oBrokerCheckingAccountItemModel.InsuredId = 0;
                oBrokerCheckingAccountItemModel.CommissionPercentage = 0;
                oBrokerCheckingAccountItemModel.CommissionType = 0;
                oBrokerCheckingAccountItemModel.CommissionAmount = 0;
                oBrokerCheckingAccountItemModel.DiscountedCommission = 0;
                oBrokerCheckingAccountItemModel.CommissionBalance = 0;

                // Items a aplicar
                var agentItemsToApply = $("#agentItemsListView").UifListView("getData");

                if (agentItemsToApply != null) {
                    for (var k = 0; k < agentItemsToApply.length; k++) {

                        oBrokerCheckingAccountItemChildModel = {
                            BrokerCheckingAccountItemId: 0,
                            AgentTypeId: 0,
                            AgentId: 0,
                            AgentAgencyId: 0,
                            AccountingNature: 0,
                            BranchId: 0,
                            SalePointId: 0,
                            AccountingCompanyId: 0,
                            AccountingDate: null,
                            CheckingAccountConceptId: 0,
                            CurrencyCode: 0,
                            ExchangeRate: 0,
                            IncomeAmount: 0,
                            Amount: 0,
                            UserId: 0,
                            Description: null,
                            BillId: 0,
                            BrokerCheckingAccountId: 0,
                            PrefixId: 0,
                            InsuredId: 0,
                            CommissionPercentage: 0,
                            CommissionType: 0,
                            CommissionAmount: 0,
                            DiscountedCommission: 0,
                            CommissionBalance: 0
                        };

                        oBrokerCheckingAccountItemChildModel.BrokerCheckingAccountItemId = movements[j].BrokerCheckingAccountItemId;
                        oBrokerCheckingAccountItemChildModel.AgentTypeId = movements[j].AgentTypeCode;
                        oBrokerCheckingAccountItemChildModel.AgentId = movements[j].AgentCode;
                        oBrokerCheckingAccountItemChildModel.AgentAgencyId = movements[j].AgentAgencyCode;
                        oBrokerCheckingAccountItemChildModel.AccountingNature = movements[j].AccountingNature;
                        oBrokerCheckingAccountItemChildModel.BranchId = agentItemsToApply[k].BranchCode;
                        oBrokerCheckingAccountItemChildModel.SalePointId = agentItemsToApply[k].SalePointCode;
                        oBrokerCheckingAccountItemChildModel.AccountingCompanyId = movements[j].CompanyCode;
                        oBrokerCheckingAccountItemChildModel.AccountingDate = getCurrentDate();
                        oBrokerCheckingAccountItemChildModel.CheckingAccountConceptId = movements[j].CheckingAccountConceptCode;
                        oBrokerCheckingAccountItemChildModel.CurrencyCode = agentItemsToApply[k].CurrencyCode;
                        oBrokerCheckingAccountItemChildModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].CurrencyChange).replace(",", "."));
                        oBrokerCheckingAccountItemChildModel.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency(agentItemsToApply[k].IncomeAmount).replace(",", "."));
                        oBrokerCheckingAccountItemChildModel.Amount = ReplaceDecimalPoint(ClearFormatCurrency(agentItemsToApply[k].IncomeAmount).replace(",", "."));
                        oBrokerCheckingAccountItemChildModel.UserId = userId;
                        oBrokerCheckingAccountItemChildModel.Description = movements[j].Description;
                        oBrokerCheckingAccountItemChildModel.BillId = movements[j].BillNumber;
                        oBrokerCheckingAccountItemChildModel.BrokerCheckingAccountId = agentItemsToApply[k].BrokerCheckingAccountItemId;
                        oBrokerCheckingAccountItemChildModel.PolicyId = agentItemsToApply[k].PolicyId;
                        oBrokerCheckingAccountItemChildModel.PrefixId = agentItemsToApply[k].PrefixCode;
                        oBrokerCheckingAccountItemChildModel.InsuredId = agentItemsToApply[k].InsuredId;
                        oBrokerCheckingAccountItemChildModel.CommissionPercentage = ReplaceDecimalPoint((ClearFormatCurrency(agentItemsToApply[k].CommissionPercentage).replace(",", ".")));
                        oBrokerCheckingAccountItemChildModel.CommissionType = agentItemsToApply[k].CommissionTypeCode;
                        oBrokerCheckingAccountItemChildModel.CommissionAmount = ReplaceDecimalPoint((ClearFormatCurrency(agentItemsToApply[k].CommissionAmount).replace(",", ".")));
                        oBrokerCheckingAccountItemChildModel.DiscountedCommission = ReplaceDecimalPoint(ClearFormatCurrency(agentItemsToApply[k].CommissionDiscounted).replace(",", "."));
                        oBrokerCheckingAccountItemChildModel.CommissionBalance = ReplaceDecimalPoint(ClearFormatCurrency(agentItemsToApply[k].CommissionBalance).replace(",", "."));

                        oBrokerCheckingAccountItemModel.BrokersCheckingAccountTransactionChild.push(oBrokerCheckingAccountItemChildModel);
                    }
                }

                oBrokerCheckingAccountModel.BrokersCheckingAccountTransactionItems.push(oBrokerCheckingAccountItemModel);
            }
        }
    }

    return oBrokerCheckingAccountModel;
}

/////////////////////////////////////////////////////////////////
/// Setea el modelo de cta.cte. agentes con valores iniciales ///
/////////////////////////////////////////////////////////////////
function SetDataBrokersCheckingAccountEmpty() {
    oBrokerCheckingAccountModel = {
        ImputationId: 0,
        BrokersCheckingAccountTransactionItems: []
    };

    oBrokerCheckingAccountItemModel = {
        BrokerCheckingAccountItemId: 0,
        AgentTypeId: 0,
        AgentId: 0,
        AgentAgencyId: 0,
        AccountingNature: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        AccountingDate: null,
        CheckingAccountConceptId: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        BrokersCheckingAccountTransactionChild: []
    };
}

/////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de items cta.cte. agentes con los valores ingresados ///
/////////////////////////////////////////////////////////////////////////////
function SetDataBrokersCheckingAccountItem(brokerTempCheckingAccountCode) {
    SetDataBrokersCheckingAccountEmpty();

    oBrokerCheckingAccountModel.ImputationId = tempImputationId;

    oBrokerCheckingAccountItemModel = {
        BrokerCheckingAccountItemId: 0,
        AgentTypeId: 0,
        AgentId: 0,
        AgentAgencyId: 0,
        AccountingNature: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        AccountingDate: null,
        CheckingAccountConceptId: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        PolicyId: 0,
        PrefixId: 0,
        InsuredId: 0,
        CommissionPercentage: 0,
        CommissionType: 0,
        CommissionAmount: 0,
        DiscountedCommission: 0,
        CommissionBalance: 0,
        BrokersCheckingAccountTransactionChild: []
    };

    var movementItemsToApply = $("#agentItemsListView").UifListView("getData");
    if (movementItemsToApply != null) {
        for (var j = 0; j < movementItemsToApply.length; j++) {

            oBrokerCheckingAccountItemModel.BrokerCheckingAccountItemId = brokerTempCheckingAccountCode;

            oBrokerCheckingAccountItemChildModel = {
                BrokerCheckingAccountItemId: 0,
                AgentTypeId: 0,
                AgentId: 0,
                AgentAgencyId: 0,
                AccountingNature: 0,
                BranchId: 0,
                SalePointId: 0,
                AccountingCompanyId: 0,
                AccountingDate: null,
                CheckingAccountConceptId: 0,
                CurrencyCode: 0,
                ExchangeRate: 0,
                IncomeAmount: 0,
                Amount: 0,
                UserId: 0,
                Description: null,
                BillId: 0,
                BrokerCheckingAccountId: 0,
                PrefixId: 0,
                InsuredId: 0,
                CommissionPercentage: 0,
                CommissionType: 0,
                CommissionAmount: 0,
                DiscountedCommission: 0,
                CommissionBalance: 0
            };

            oBrokerCheckingAccountItemChildModel.BrokerCheckingAccountItemId = movementItemsToApply[j].BrokerCheckingAccountItemId;
            oBrokerCheckingAccountItemChildModel.BrokerCheckingAccountId = movementItemsToApply[j].BrokerCheckingAccountItemId;
            oBrokerCheckingAccountItemModel.BrokersCheckingAccountTransactionChild.push(oBrokerCheckingAccountItemChildModel);
        }
    }
    oBrokerCheckingAccountModel.BrokersCheckingAccountTransactionItems.push(oBrokerCheckingAccountItemModel);

    return oBrokerCheckingAccountModel;
}

///////////////////////////////////////////////////
//  Añade registros en listView                  //
///////////////////////////////////////////////////
var saveAgentCallback = function (deferred, data) {
    $("#agentsListView").UifListView("addItem", data);
    deferred.resolve();
};

///////////////////////////////////////////////////
//  Edita un registro en listView                //
//////////////////////////////////////////////////
var editAgentCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

///////////////////////////////////////////////////
//  Elimina un registro en listView              //
///////////////////////////////////////////////////
var deleteAgentCallback = function (deferred, data) {
    deferred.resolve();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckingAccountBrokers/DeleteTempBrokersCheckingAccountItem",
        data: { "tempImputationCode": tempImputationId, "tempBrokersCheckingAccountCode": data.BrokerCheckingAccountItemId },
        success: function () {
            setTimeout(function () {
                //RefreshAgentMovements();
                SetAgentTotalMovement();
            }, 2000);        
        }
    });

    

};


//////////////////////////////////////////////////
// Setea el valor del importe local en agente   //
//////////////////////////////////////////////////
function SetAgentLocalAmount() {
    var agentAmount = $("#AgentAmount").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    var exchangeRate = $("#AgentExchangeRate").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    var agentLocalAmount = agentAmount * exchangeRate;
    $("#AgentLocalAmount").val("$ " + NumberFormatSearch(agentLocalAmount, "2", ".", ","));
}

//////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios     //
//////////////////////////////////////////////////
function ValidateAgentAddForm() {
    if ($('#selectAgentCurrency').val() == "") {
        $("#modalAgent").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
        return false;
    }
    if ($('#AgentAmount').val() == "") {
        $("#modalAgent").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
        return false;
    }
    if ($("#selectAgentAccountingAccountConcept").val() == null) {
        $("#modalAgent").find("#alertForm").UifAlert('show', Resources.CurrentAccountConceptCodeRequired, "warning");
        return false;
    }
    return true;
}

///////////////////////////////////////////////////////
// Setear el total de la listview movimientos agente //
///////////////////////////////////////////////////////
function SetAgentTotalMovement() {
    var totalMovementCredit = 0;
    var totalMovementDebit = 0;

    var agents = $("#agentsListView").UifListView("getData");

    if (agents != null) {

        for (var j = 0; j < agents.length; j++) {
            var agentAmount = String(agents[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
            if (agents[j].NatureId == 1)
                totalMovementCredit += parseFloat(agentAmount);
            else
                totalMovementDebit += parseFloat(agentAmount);
        }
    }
    else {
        $("#TotalAgentMovementCredit").text("");
        $("#TotalAgentMovementDebit").text("");
    }
    $("#TotalAgentMovementCredit").text("$" + NumberFormatSearch(totalMovementCredit, "2", ".", ","));
    $("#TotalAgentMovementDebit").text("$" + NumberFormatSearch(totalMovementDebit, "2", ".", ","));
}

///////////////////////////////////////////////////
/// Validación de duplicados movimientos agente ///
///////////////////////////////////////////////////
function ValidateDuplicateBroker() {
    var duplicate = -1;

    if (editAgent > -1) {
        if (($("#selectAgentBranch").val() == $("#HiddenBranchId").val()) &&
            ($("#selectAgentSalePoint").val() == $("#HiddenSalePointId").val()) &&
            (agentId == $("#HiddenAgentId").val())) {
            duplicate = -1;
        }
        else {
            $("#MovementMessageDuplicate").val("");
            duplicate = 0;
        }
    }

    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "CheckingAccountBrokers/ValidateDuplicateBrokerCheckingAccount",
        data: {
            "branchId": $("#selectAgentBranch").val(),
            "salePointId": $("#selectAgentSalePoint").val(),
            "companyId": $("#selectAgentCompany").val(),
            "agentId": agentId,
            "accountingNatureId": $("#selectAgentNature").val(),
            "checkingAccountConceptId": $("#selectAgentAccountingAccountConcept").val(),
            "currencyId": $("#selectAgentCurrency").val()
        },
        success: function (data) {
            //console.log(data);
            if (data[0].source == -1) {
                duplicate = 0;
                $("#MovementMessageDuplicate").val("");
            }
            else if (data[0].source >= 0) {
                duplicate = 1;
                if (data[0].source == 0) {

                    if (data[0].isReal == 1) {

                        if (data[0].type == 1) {
                            $("#MovementMessageDuplicate").val(Resources.CashReceipt + ":" + data[0].imputationId);
                        }
                        else if (data[0].type == 2) {
                            $("#MovementMessageDuplicate").val(Resources.JournalEntry + ":" + data[0].imputationId);
                        }
                        else if (data[0].type == 3) {
                            $("#MovementMessageDuplicate").val(Resources.Preliquidation + ":" + data[0].imputationId);
                        }
                        else if (data[0].type == 4) {
                            $("#MovementMessageDuplicate").val(Resources.PaymentOrder + ":" + data[0].imputationId);
                        }
                    }
                    else {
                        $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + " " + data[0].imputationId);
                    }
                }
                else if (data[0].source == 1) {
                    $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + " " + data[0].imputationId);
                }
            }
        }
    });

    return duplicate;
}

///////////////////////////////////////////////////
/// Validación de duplicados movimientos agente ///
///////////////////////////////////////////////////
function ValidateAceptButtonDuplicateAgent(agentData) {
    var duplicate = -1;

    if (editAgent > -1) {
        if (($("#selectAgentBranch").val() == $("#HiddenBranchId").val()) &&
            ($("#selectAgentSalePoint").val() == $("#HiddenSalePointId").val()) &&
            (agentId == $("#HiddenAgentId").val())) {
            duplicate = -1;
        }
        else {
            $("#MovementMessageDuplicate").val("");
            duplicate = 0;
        }
    }
    
        
            $.ajax({
                async: false,
                type: "GET",
                url: ACC_ROOT + "CheckingAccountBrokers/ValidateDuplicateBrokerCheckingAccount",
                data: {
                    "branchId": agentData.BranchId,
                    "salePointId": agentData.SalePointId,
                    "companyId": agentData.CompanyId,
                    "agentId": agentData.AgentId,
                    "accountingNatureId": agentData.NatureId,
                    "checkingAccountConceptId": agentData.ConceptId,
                    "currencyId": agentData.CurrencyId
                },
                success: function (data) {
                    //console.log(data);
                    if (data[0].source == -1) {
                        duplicate = 0;
                        $("#MovementMessageDuplicate").val("");
                    }
                    else if (data[0].source >= 0) {
                        duplicate = 1;
                        if (data[0].source == 0) {

                            if (data[0].isReal == 1) {

                                if (data[0].type == 1) {
                                    $("#MovementMessageDuplicate").val(Resources.CashReceipt + ":" + data[0].imputationId);
                                }
                                else if (data[0].type == 2) {
                                    $("#MovementMessageDuplicate").val(Resources.JournalEntry + ":" + data[0].imputationId);
                                }
                                else if (data[0].type == 3) {
                                    $("#MovementMessageDuplicate").val(Resources.Preliquidation + ":" + data[0].imputationId);
                                }
                                else if (data[0].type == 4) {
                                    $("#MovementMessageDuplicate").val(Resources.PaymentOrder + ":" + data[0].imputationId);
                                }
                            }
                            else {
                                $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + " " + data[0].imputationId);
                            }
                        }
                        else if (data[0].source == 1) {
                            $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + " " + data[0].imputationId);
                        }
                    }
                }
            });
        
    
    return duplicate;
}

/////////////////////////////////////////////////////////////////////
/// Setea los valores de los itema a aplicar de cta. cte. agentes ///
/////////////////////////////////////////////////////////////////////
function SetUpdateDataBrokersCheckingAccount() {
    oBrokerCheckingAccountUpdateModel = {
        ImputationId: 0,
        BrokersCheckingAccountTransactionItems: []
    };

    oBrokerCheckingAccountItemUpdateModel = {
        BrokerCheckingAccountItemId: 0,
        AgentTypeId: 0,
        AgentId: 0,
        AgentAgencyId: 0,
        AccountingNature: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        AccountingDate: null,
        CheckingAccountConceptId: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0
    };

    var itemsToApply = $("#agentsListView").UifListView("getData");

    if (itemsToApply != null) {

        for (var j = 0; j < itemsToApply.length; j++) {
            if (itemsToApply[j].Status == 0) {
                oBrokerCheckingAccountUpdateModel.ImputationId = tempImputationId;
                oBrokerCheckingAccountItemUpdateModel = {
                    BrokerCheckingAccountItemId: 0,
                    AgentTypeId: 0,
                    AgentId: 0,
                    AgentAgencyId: 0,
                    AccountingNature: 0,
                    BranchId: 0,
                    SalePointId: 0,
                    AccountingCompanyId: 0,
                    AccountingDate: null,
                    CheckingAccountConceptId: 0,
                    CurrencyCode: 0,
                    ExchangeRate: 0,
                    IncomeAmount: 0,
                    Amount: 0,
                    UserId: 0,
                    Description: null,
                    BillId: 0
                };
            }
            oBrokerCheckingAccountItemUpdateModel.BrokerCheckingAccountItemId = itemsToApply[j].BrokerCheckingAccountItemId;
            oBrokerCheckingAccountItemUpdateModel.AgentTypeId = itemsToApply[j].AgentTypeCode;
            oBrokerCheckingAccountItemUpdateModel.AgentId = itemsToApply[j].AgentCode;
            oBrokerCheckingAccountItemUpdateModel.AgentAgencyId = itemsToApply[j].AgentAgencyCode;
            oBrokerCheckingAccountItemUpdateModel.AccountingNature = itemsToApply[j].AccountingNature;
            oBrokerCheckingAccountItemUpdateModel.BranchId = itemsToApply[j].BranchId;
            oBrokerCheckingAccountItemUpdateModel.SalePointId = itemsToApply[j].SalePointId;
            oBrokerCheckingAccountItemUpdateModel.AccountingCompanyId = itemsToApply[j].CompanyId
            oBrokerCheckingAccountItemUpdateModel.AccountingDate = getCurrentDate();
            oBrokerCheckingAccountItemUpdateModel.CheckingAccountConceptId = itemsToApply[j].ConceptId;
            oBrokerCheckingAccountItemUpdateModel.CurrencyCode = itemsToApply[j].CurrencyId;
            oBrokerCheckingAccountItemUpdateModel.ExchangeRate = ClearFormatCurrency(String(itemsToApply[j].EchangeRate)).replace(",", ".");
            oBrokerCheckingAccountItemUpdateModel.IncomeAmount = ClearFormatCurrency(String(itemsToApply[j].LocalAmount)).replace(",", ".");
            oBrokerCheckingAccountItemUpdateModel.Amount = ClearFormatCurrency(String(itemsToApply[j].Amount)).replace(",", ".");
            oBrokerCheckingAccountItemUpdateModel.UserId = userId;
            oBrokerCheckingAccountItemUpdateModel.Description = itemsToApply[j].Description;
            oBrokerCheckingAccountItemUpdateModel.BillId = itemsToApply[j].BillNumber;

            oBrokerCheckingAccountUpdateModel.BrokersCheckingAccountTransactionItems.push(oBrokerCheckingAccountItemUpdateModel);

        }
    }

    return oBrokerCheckingAccountUpdateModel;
}

////////////////////////////////////////////////////////////
/// Realiza la consulta de movimientos cta. cte. agentes ///
////////////////////////////////////////////////////////////
function RefreshAgentMovements() {
    $("#agentsListView").UifListView(
        {
            autoHeight: true,
            source: ACC_ROOT + "CheckingAccountBrokers/GetTempBrokerCheckingAccountItemByTempImputationId?tempImputationId=" + tempImputationId,
            customDelete: false,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: true,
            displayTemplate: "#agent-display-template",
            deleteCallback: deleteAgentCallback
        });
    SetAgentTotalMovement();
}

/////////////////////////////////////////////////////////////////
/// Setea la compañía y sucursal por default - agentes        ///
/////////////////////////////////////////////////////////////////
function SetAgentAccountingCompany() {
    //setTimeout(function () {
    if (isMulticompany == 0) {
        $("#selectAgentCompany").val(accountingCompanyDefault);
        $("#selectAgentCompany").attr("disabled", true);
    }
    else {
        $("#selectAgentCompany").attr("disabled", false);
    }
    if (branchUserDefault > 0) {
        $("#selectAgentBranch").val(branchUserDefault);

        if (branchUserDefault) {
            LoadAgentSalesPointByBranchId(branchUserDefault,null)//jira SMT-2000 quitar puntos de venta por defecto.
            LoadAgentAccountingAccountConceptByBranchId(branchUserDefault);
        }
    }
    else {
        $("#selectAgentBranch").val("");
    }

    //}, 1000);
}

//////////////////////////////////////////////////////
/// Setea los campos a valores iniciales - agentes ///
//////////////////////////////////////////////////////
function SetAgentFieldEmpty() {
    agentDocumentNumber = "";
    agentName = "";
    $("#selectAgentBranch").val("");
    $("#selectAgentSalePoint").val("");
    $("#selectAgentCompany").val("");
    $("#AgentMovementDescription").val("");
    $("#AgentDocumentNumber").UifAutoComplete('clean');
    $("#AgentName").UifAutoComplete('clean');
    $("#selectAgentAccountingAccountConcept").val("");
    $("#selectAgentNature").val("");
    $("#selectAgentCurrency").val("");
    $("#AgentExchangeRate").val("");
    $("#AgentAmount").val("");
    $("#AgentLocalAmount").val("");
    ClearValidation("#addAgentForm");
}


function MoneyFormattoNumber(text) {    
    return  parseFloat(text.toString().trim().replace('$ ', '').replace(new RegExp(',', 'g'), ''));
}