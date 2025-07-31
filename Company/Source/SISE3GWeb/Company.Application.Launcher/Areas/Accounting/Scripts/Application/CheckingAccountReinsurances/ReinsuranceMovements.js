var tempImputationId = 0;
var _endorsementId = 0;
var oReinsuranceCheckingAccountModel = null;
var oReinsuranceCheckingAccountItemModel = null;

setTimeout(function () {
    tempImputationId = $("#ViewBagTempImputationId").val();
}, 1000);


function RowReinsurerModel() {
    this.BranchId;
    this.BranchName;
    this.SalePointId;
    this.SalePointName;
    this.CompanyId;
    this.CompanyName;
    this.LineBusinessId;
    this.LineBusinessName;
    this.SubLineBusinessId;
    this.SubLineBusinessName;
    this.Description;
    this.BrokerId;
    this.BrokerDocumentNumber;
    this.BrokerName;
    this.ReinsurerId;
    this.ReinsurerDocumentNumber;
    this.ReinsurerName;
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
    this.ContractTypeId;
    this.ContractTypeName;
    this.ContractId;
    this.ContractNumber;
    this.StretchId;
    this.StretchName;
    this.ReinsurerCheckingAccountItemId;
    this.AgentTypeCode;
    this.AgentAgencyCode;
    this.BillNumber;
    this.Region;
    this.Excercise;
    this.YearMonthApplies;
    this.ApplicationYear;
    this.ApplicationMonth;
    this.FacultativeId;
    this.SlipNumber;
    this.PolicyBranch;
    this.PolicyPrefix;
    this.PolicyEndorsement;
    this.ReinsurerPolicyId;
    this.ReinsurerEndorsementId;
    this.TempReinsuranceParentId;
    this.Status;
}

var oReinsuranceCheckingAccountUpdateModel = {
    ImputationId: 0,
    ReinsuranceCheckingAccountTransactionItems: []
};

var oReinsuranceCheckingAccountItemUpdateModel = {
    ReinsuranceCheckingAccountItemId: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    LineBusinessId: 0,
    SubLineBusinessId: 0,
    AgentId: 0,
    ReinsuranceCompanyId: 0,
    IsFacultative: 0,
    SlipNumber: null,
    ContractTypeId: 0,
    ContractNumber: null,
    Section: null,
    Region: null,
    CheckingAccountConceptId: 0,
    AccountingNature: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    Period: 0,
    PolicyId: 0,
    EndorsementId: 0,
    ApplicationYear: 0,
    ApplicationMonth: 0,
    ReinsuranceCheckingAccountTransactionChild: []
};

var oReinsuranceCheckingAccountItemChildUpdateModel = {
    ReinsuranceCheckingAccountItemId: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    LineBusinessId: 0,
    SubLineBusinessId: 0,
    AgentId: 0,
    ReinsuranceCompanyId: 0,
    IsFacultative: 0,
    SlipNumber: null,
    ContractTypeId: 0,
    ContractNumber: null,
    Section: null,
    Region: null,
    CheckingAccountConceptId: 0,
    AccountingNature: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0,
    ReinsuranceCheckingAccountId: 0,
    Period: 0,
    PolicyId: 0,
    EndorsementId: 0,
    ApplicationYear: 0,
    ApplicationMonth: 0
};

var reinsurerId = 0;
var reinsurerCompanyId = 0;
var contractId = 0;
var brokerId = 0;
var brokerCompanyId = 0;
var editReinsurance = -1;
var reinsuranceLocalAmount = 0;


var reinsuranceDocumentNumber = $('#ReinsurerDocumentNumber').val();
var reinsuranceName = $('#ReinsurerName').val();
var brokerDocumentNumber = $('#BrokerDocumentNumber').val();
var brokerName = $('#BrokerName').val();

$(document).ready(function () {
    /*---------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                        ACCIONES / EVENTOS                                                       */
    /*---------------------------------------------------------------------------------------------------------------------------------*/

    $("#modalReinsurance").find("#reinsurancesListView").UifListView({
        autoHeight: true,
        source: ACC_ROOT + "CheckingAccountReinsurances/GetTempReinsuranceCheckingAccountItemByTempImputationId?tempImputationId=" + $("#ViewBagTempImputationId").val(),
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#reinsurance-display-template",
        deleteCallback: deleteReinsuranceCallback
    });


    ///////////////////////////////////////////////////////
    /// Combo sucursal - reaseguradora                  ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#selectReinsuranceBranch").on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#selectReinsuranceSalePoint").UifSelect({
                source: controller
            });

            controller = ACC_ROOT + "Common/GetAccountingAccountConceptByBranchId?branchId=" +
                selectedItem.Id + "&sourceId=3";
            $("#selectReinsuranceAccountingAccountConcept").UifSelect({
                source: controller
            });
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo concepto cuenta contable - reaseguradora  ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#selectReinsuranceAccountingAccountConcept").on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        if (selectedItem.Id > 0) {
            $.ajax({
                async: false,
                type: "GET",
                url: ACC_ROOT + "Common/GetAdditionalInformationConcept",
                data: {
                    "branchId": $("#selectReinsuranceBranch").val(),
                    "conceptId": selectedItem.Id,
                    "sourceId": 3
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
    /// Combo ramo técnico - reaseguradora              ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#selectReinsurancePrefix").on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        if (selectedItem.Id > 0) {
            LoadTechnicalSubPrefixesByPrefixId(selectedItem.Id)
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo tipo contrato - reaseguradora             ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#selectReinsuranceContractType").on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        $('#ReinsurerContract').UifAutoComplete('clean');
        $("#selectReinsuranceStretch").UifSelect();;
        if (selectedItem.Id > 0) {
            $('#ReinsurerContract').removeAttr('disabled');
            $("#ReinsurerContract").UifAutoComplete(
                {
                    source: ACC_ROOT + "Common/GetContractNumberByNumber",
                    displayRecords: 5,
                    queryParameter: "contractTypeId=" + selectedItem.Id + "&query"
                });

            if (selectedItem.Id == 7) {
                $("#OptionalOne").show();
                $("#OptionalThree").show();
            } else {
                $("#OptionalOne").hide();
                $("#OptionalThree").hide();
            }
        }
        else {
            $("#ReinsurerContract").attr("disabled", "disabled");
            $("#selectReinsuranceStretch").attr("disabled", "disabled");


        }
    });

    ///////////////////////////////////////////////////////
    /// Combo moneda - reaseguradora                    ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#selectReinsuranceCurrency").on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        SetCurrency("selectReinsuranceCurrency", "ReinsuranceExchangeRate");
        if ($("#ReinsuranceAmount").val() != "") {
            SetReinsuranceLocalAmount();
        }
    });

    ///////////////////////////////////////////////////////
    /// Importe - reaseguradora                         ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#ReinsuranceAmount").blur(function () {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        if ($("#ReinsuranceAmount").val() != "") {
            var reinsuranceAmount = $("#ReinsuranceAmount").val();
            $("#ReinsuranceAmount").val("$ " + NumberFormatSearch(reinsuranceAmount, "2", ".", ","));
            SetReinsuranceLocalAmount();
        }
    });

    ///////////////////////////////////////////////
    /// Autocomplete documento - reaseguradora  ///
    ///////////////////////////////////////////////
    $("#modalReinsurance").find('#ReinsurerDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        reinsurerId = selectedItem.Id;
        if (reinsurerId > 0) {
            $('#ReinsurerDocumentNumber').val(selectedItem.ReinsurerDocumentNumber);
            $('#ReinsurerName').val(selectedItem.ReinsurerName);
            reinsurerCompanyId = selectedItem.ReinsurerIndividualId;
            reinsuranceDocumentNumber = selectedItem.ReinsurerDocumentNumber;
            reinsuranceName = selectedItem.ReinsurerName;
        } else {
            $('#ReinsurerDocumentNumber').val("");
            $('#ReinsurerName').val("");
        }
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalReinsurance").find("#ReinsurerDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#ReinsurerDocumentNumber').val(reinsuranceDocumentNumber);
        }, 50);
    });

    ////////////////////////////////////////////////
    /// Autocomplete nombre - reaseguradora      ///
    ////////////////////////////////////////////////
    $("#modalReinsurance").find('#ReinsurerName').on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        reinsurerId = selectedItem.Id;
        if (reinsurerId > 0) {
            $('#ReinsurerDocumentNumber').val(selectedItem.ReinsurerDocumentNumber);
            $('#ReinsurerName').val(selectedItem.ReinsurerName);
            reinsurerCompanyId = selectedItem.ReinsurerIndividualId;
            reinsuranceDocumentNumber = selectedItem.ReinsurerDocumentNumber;
            reinsuranceName = selectedItem.ReinsurerName;
        } else {
            $('#ReinsurerDocumentNumber').val("");
            $('#ReinsurerName').val("");
            reinsurerCompanyId = -1;
        }
    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalReinsurance").find("#ReinsurerName").on('blur', function (event) {
        setTimeout(function () {
            $('#ReinsurerName').val(reinsuranceName);
        }, 50);
    });

    ///////////////////////////////////////////////
    /// Autocomplete contrato - reaseguradora   ///
    ///////////////////////////////////////////////
    $("#modalReinsurance").find('#ReinsurerContract').on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        contractId = selectedItem.Id;
        if (contractId > 0) {
            $('#ReinsurerContract').val(selectedItem.Name);
            $('#ReinsurerContract').removeAttr('disabled');
            LoadContractStretchs(contractId);
            //var controller = ACC_ROOT + "Common/GetContractStretchs?contractId=" + contractId;
            //$("#selectReinsuranceStretch").UifSelect({
            //    source: controller
            //});
        }
        else {
            $("#selectReinsuranceStretch").UifSelect();
            $("#selectReinsuranceStretch").attr("disabled", "disabled");
            $('#ReinsurerContract').val("");
        }
    });

    ///////////////////////////////////////////////
    /// Autocomplete documento - broker         ///
    ///////////////////////////////////////////////
    $("#modalReinsurance").find('#BrokerDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        brokerId = selectedItem.Id;
        if (brokerId > 0) {
            $('#BrokerDocumentNumber').val(selectedItem.BrokerDocumentNumber);
            $('#BrokerName').val(selectedItem.BrokerName);
            brokerCompanyId = selectedItem.BrokerIndividualId;
            brokerDocumentNumber = selectedItem.BrokerDocumentNumber;
            brokerName = selectedItem.BrokerName;
        } else {
            $('#BrokerDocumentNumber').val("");
            $('#BrokerName').val("");
            brokerCompanyId = -1;
        }
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalReinsurance").find("#BrokerDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#BrokerDocumentNumber').val(brokerDocumentNumber);
        }, 50);
    });

    ////////////////////////////////////////////////
    /// Autocomplete nombre - broker              ///
    ////////////////////////////////////////////////
    $("#modalReinsurance").find('#BrokerName').on('itemSelected', function (event, selectedItem) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        brokerId = selectedItem.Id;
        if (brokerId > 0) {
            $('#BrokerDocumentNumber').val(selectedItem.BrokerDocumentNumber);
            $('#BrokerName').val(selectedItem.BrokerName);
            brokerCompanyId = selectedItem.BrokerIndividualId;
            brokerDocumentNumber = selectedItem.BrokerDocumentNumber;
            brokerName = selectedItem.BrokerName;
        } else {
            $('#BrokerDocumentNumber').val("");
            $('#BrokerName').val("");
            brokerCompanyId = -1;
        }
    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalReinsurance").find("#BrokerName").on('blur', function (event) {
        setTimeout(function () {
            $('#BrokerName').val(brokerName);
        }, 50);
    });

    ///////////////////////////////////////////
    /// ListView - reaseguradora            ///
    ///////////////////////////////////////////
    $("#modalReinsurance").find('#reinsurancesListView').on('rowEdit', function (event, data, index) {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
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
        brokerId = data.IssuingBankId;
        reinsurerId = data.IssuingBankId;
        $("#HiddenIssuingBankId").val(data.IssuingBankId);

        $("#selectReinsuranceBranch").UifSelect("setSelected", data.BranchId);
        $("#selectReinsuranceSalePoint").UifSelect("setSelected", data.SalePointId);
        $("#selectReinsurancePrefix").UifSelect("setSelected", data.LineBusinessId);//------------
        $("#selectReinsuranceSubPrefix").UifSelect("setSelected", data.SubLineBusinessId);//--------
        $("#ReinsurerDocumentNumber").val(data.ReinsurerDocumentNumber);
        $("#ReinsurerName").val(data.ReinsurerName);
        $("#selectReinsuranceContractType").val(data.ContractTypeId);//tipo de contrato
        $("#ReinsurerContract").val(data.ContractId);//contrato
        $("#selectReinsuranceStretch").UifSelect("setSelected", data.StretchId);//tramo
        $("#BrokerDocumentNumber").val(data.BrokerDocumentNumber);//document number broker
        $("#BrokerName").val(data.BrokerName);
        $("#ReinsuranceMovementDescription").val(data.Description);//descripcion del movimiento
        $("#selectReinsuranceAccountingAccountConcept").UifSelect("setSelected", data.ConceptId);//codigo concepto
        $("#selectReinsuranceNature").UifSelect("setSelected", data.NatureId);//naturaeleza
        $("#selectReinsuranceCurrency").UifSelect("setSelected", data.CurrencyId);//moneda
        $("#ReinsuranceExchangeRate").val(data.ExchangeRate);//importe cambio
        $("#ReinsuranceAmount").val(data.Amount);//importe
        $("#ReinsuranceApplicationYear").val(data.ApplicationYear);//año aplicacion
        $("#selectReinsuranceApplicationMonth").UifSelect("setSelected", data.ApplicationMonth);//mes


        editReinsurance = index;
    });


    //////////////////////////////////////////////////////////////
    /// Botón agregar movimiento cta. cte. reaseguradora modal ///
    //////////////////////////////////////////////////////////////
    $("#modalReinsurance").find('#ReinsuranceAdd').click(function () {
        var existMovement = 0;
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');

        $("#addReinsuranceForm").validate();

        if ($("#addReinsuranceForm").valid()) {
            if ($("#selectReinsuranceContractType").val() == 7) {
                if (ValidateReinsurerPolicyAndEndorsement() == true) {
                    return;
                }
            }

            if (ValidateReinsuranceAddForm() == true) {
                if (ValidateDuplicateReinsurance() == 0) {

                    var rowModel = new RowReinsurerModel();

                    rowModel.BranchId = $('#selectReinsuranceBranch').val();
                    rowModel.BranchName = $('#selectReinsuranceBranch option:selected').text();
                    rowModel.SalePointId = $('#selectReinsuranceSalePoint').val();
                    rowModel.SalePointName = $('#selectReinsuranceSalePoint option:selected').text();
                    rowModel.CompanyId = $('#selectReinsuranceCompany').val();
                    rowModel.CompanyName = $('#selectReinsuranceCompany option:selected').text();
                    rowModel.LineBusinessId = $('#selectReinsurancePrefix').val();
                    rowModel.LineBusinessName = $('#selectReinsurancePrefix option:selected').text();
                    rowModel.SubLineBusinessId = $('#selectReinsuranceSubPrefix').val();
                    rowModel.SubLineBusinessName = $('#selectReinsuranceSubPrefix option:selected').text();
                    rowModel.Description = $('#ReinsuranceMovementDescription').val();
                    rowModel.BrokerId = brokerId;
                    rowModel.BrokerDocumentNumber = $('#BrokerDocumentNumber').val();
                    rowModel.BrokerName = $('#BrokerName').val();;
                    rowModel.ReinsurerId = reinsurerId;
                    rowModel.ReinsurerDocumentNumber = $('#ReinsurerDocumentNumber').val();
                    rowModel.ReinsurerName = $('#ReinsurerName').val();
                    rowModel.ConceptId = $('#selectReinsuranceAccountingAccountConcept').val();
                    rowModel.ConceptName = $('#selectReinsuranceAccountingAccountConcept option:selected').text();
                    rowModel.NatureId = $('#selectReinsuranceNature').val();
                    rowModel.NatureName = $('#selectReinsuranceNature option:selected').text();
                    rowModel.AccountingNature = $('#selectReinsuranceNature').val();
                    rowModel.CurrencyId = $('#selectReinsuranceCurrency').val();
                    rowModel.Currency = $('#selectReinsuranceCurrency option:selected').text();
                    rowModel.ExchangeRate = $("#ReinsuranceExchangeRate").val();
                    rowModel.Amount = $('#ReinsuranceAmount').val();
                    rowModel.LocalAmount = reinsuranceLocalAmount;
                    rowModel.FacultativeId = $("#selectReinsuranceContractType").val();
                    rowModel.ContractTypeId = $("#selectReinsuranceContractType").val();
                    rowModel.ContractTypeName = $('#selectReinsuranceContractType option:selected').text();
                    rowModel.ContractId = contractId;
                    rowModel.ContractNumber = contractId;
                    rowModel.StretchId = $("#selectReinsuranceStretch").val();
                    rowModel.StretchName = $("#selectReinsuranceStretch option:selected").text();
                    rowModel.ReinsurerCheckingAccountItemId = 0;
                    rowModel.AgentTypeCode = agentTypeId;
                    rowModel.AgentAgencyCode = agentAgencyId;
                    rowModel.BillNumber = $("#MovementBillNumber").val();
                    rowModel.Region = "";
                    rowModel.Exercise = $("#ReinsuranceApplicationYear").val();
                    rowModel.ApplicationYear = $("#ReinsuranceApplicationYear").val();
                    rowModel.ApplicationMonth = $("#selectReinsuranceApplicationMonth").val();
                    rowModel.YearMonthApplies = $("#ReinsuranceApplicationYear").val() + "-" + $("#selectReinsuranceApplicationMonth").val();

                    if ($("#selectReinsuranceContractType").val() == 7) {
                        rowModel.SlipNumber = $("#selectReinsuranceSlip option:selected").text();
                        rowModel.PolicyBranch = $("#selectReinsurancePolicyBranch option:selected").text();
                        var policyBranch = $("#selectReinsurancePolicyBranch option:selected").text();
                        rowModel.PolicyPrefix = $("#selectReinsurancePolicyPrefix option:selected").text();
                        var policyPrefix = $("#selectReinsurancePolicyPrefix option:selected").text();
                        rowModel.PolicyEndorsement = policyBranch.substring(0, 4) + "-" + policyPrefix.substring(0, 4) + "-" +
                            $("#ReinsurancePolicy").val() + "-" +
                            $("#ReinsuranceEndorsement").val();
                        rowModel.ReinsurerPolicyId = $("#ReinsurancePolicy").val();
                        rowModel.ReinsurerEndorsementId = _endorsementId;
                    } else {
                        rowModel.SlipNumber = "";
                        rowModel.PolicyBranch = "";
                        rowModel.PolicyPrefix = "";
                        rowModel.PolicyEndorsement = "";
                        rowModel.ReinsurerPolicyId = "";
                        rowModel.ReinsurerEndorsementId = "";
                    }
                    rowModel.Status = "1";

                    $('#TotalReinsuranceMovement').text(rowModel.Amount);

                    if (editReinsurance == -1) {
                        $('#reinsurancesListView').UifListView("addItem", rowModel);
                    } else {
                        $('#reinsurancesListView').UifListView("editItem", editReinsurance, rowModel);
                        editReinsurance = -1;
                    }


                    $("#selectReinsuranceSlip").UifSelect();
                    $("#addReinsuranceForm").formReset();
                    SetReinsuranceTotalMovement();
                    SetReinsuranceAccountingCompany();
                } else {
                    var message = Resources.AgentsMessageValidateDuplicate + " " + $("#MovementMessageDuplicate").val();
                    $("#modalReinsurance").find("#alertForm").UifAlert('show', message, "warning");
                }
            }

            setTimeout(function () {
                SetReinsuranceTotalMovement();
                SetReinsuranceAccountingCompany();
            }, 1000);
        }
    });


    /////////////////////////////////////////////////////
    /// Botón aceptar movimientos reaseguradora modal ///
    /////////////////////////////////////////////////////
    $("#modalReinsurance").find('#ReinsuranceAcceptMovement').click(function () {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        var movements = $("#reinsurancesListView").UifListView("getData");

        if (movements.length > 0) {
            movements.forEach(function (item, index) {
                if (ValidateAcceptButtonDuplicateReinsurance(item) == 0) {
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

                    SetUpdateDataReinsuranceCheckingAccount();

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "CheckingAccountReinsurances/SaveTempReinsuranceCheckingAccountRequest",
                        data: {
                            "reinsuranceCheckingAccount": SetDataReinsuranceCheckingAccount(),
                            "status": 1
                        },
                        success: function () {
                            RefreshReinsuranceMovements();
                        }
                    });

                    if ($("#ReceiptAmount").val() == "") {
                        $("#ReceiptAmount").val("0");
                    }

                    amount = $("#ReceiptAmount").val();
                    if (isNaN(amount)) {
                        amount = 0;
                    } else {
                        amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val()));
                    }


                    $('#modalReinsurance').UifModal('hide');

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
                    var message = Resources.AgentsMessageValidateDuplicate + " " + $("#MovementMessageDuplicate").val();
                    $("#modalReinsurance").find("#alertForm").UifAlert('show', message, "warning");
                }
            });
        } else {
            $('#modalReinsurance').UifModal('hide');

        }

    });

    /////////////////////////////////////////////////////
    /// Botón Cerrar movimientos reaseguradora modal ///
    /////////////////////////////////////////////////////
    $('#modalReinsurance').on('closed.modal', function () {
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
    ///////////////////////////////////////////////////////
    /// Año aplicación - reaseguradora                  ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find("#ReinsuranceApplicationYear").blur(function () {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');

        if ($("#ReinsuranceApplicationYear").val() != "") {
            var currentDate = getDate();
            currentDate = currentDate.split("/");
            var currentYear = currentDate[2];

            var minYearValue = currentYear - 5;
            var rangeYearMessage = "[" + minYearValue + "-" + currentYear + "]";
            if (parseInt($("#ReinsuranceApplicationYear").val()) < minYearValue || parseInt($("#ReinsuranceApplicationYear").val()) > currentYear) {
                $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.ValidateAplicationYear + ' ' + rangeYearMessage, "warning");
                $("#ReinsuranceApplicationYear").val("");
                $("#ReinsuranceApplicationYear").focus();
            }
        }
    });

    ///////////////////////////////////////////////////////
    /// Número póliza - reaseguradora                   ///
    ///////////////////////////////////////////////////////
    $("#ReinsurancePolicy").blur(function () {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        if (ValidateReinsurerPolicyAndEndorsement() == false) {
            var controller = ACC_ROOT + "Common/GetSlipNumbers?endorsementId=" + _endorsementId;
            $("#selectReinsuranceSlip").UifSelect({
                source: controller
            });
        }
        else {
            $("#selectReinsuranceSlip").UifSelect();
        }
    });

    ///////////////////////////////////////////////////////
    /// Número endoso - reaseguradora                   ///
    ///////////////////////////////////////////////////////
    $("#modalReinsurance").find('#ReinsuranceEndorsement').blur(function () {
        $("#modalReinsurance").find("#alertForm").UifAlert('hide');
        if (ValidateReinsurerPolicyAndEndorsement() == false) {
            var controller = ACC_ROOT + "Common/GetSlipNumbers?endorsementId=" + _endorsementId;
            $("#selectReinsuranceSlip").UifSelect({
                source: controller
            });
        }
        else {
            $("#selectReinsuranceSlip").UifSelect();
        }

    });



});

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                     DEFINICION DE FUNCIONES                                                     */
/*---------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#selectReinsuranceBranch").attr("disabled", "disabled");
    }, 300);
} else {
    $("#selectReinsuranceBranch").removeAttr("disabled");
}

function LoadReinsuraceMovementsBranchs(branchUserDefault) {
    ReinsuraceMovementsRequest.GetBranchs().done(function (data) {
        if (branchUserDefault == null || branchUserDefault == 0) {
            $("#selectReinsuranceBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectReinsuranceBranch").UifSelect({ sourceData: data.data, selectedId: branchUserDefault });
        }
    });
}
function LoadReinsuraceMovementsSalesPointByBranchId(branchUserDefault, salePointBranchUserDefault) {
    ReinsuraceMovementsRequest.GetSalesPointByBranchId(branchUserDefault).done(function (data) {
        if (salePointBranchUserDefault == null || salePointBranchUserDefault == 0) {
            $("#selectReinsuranceSalePoint").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectReinsuranceSalePoint").UifSelect({ sourceData: data.data, selectedId: salePointBranchUserDefault });
        }
    });
}

function LoadAgentReinsuraceMovementsAccountConceptByBranchId(branchUserDefault) {
    ReinsuraceMovementsRequest.GetAccountingAccountConceptByBranchId(branchUserDefault).done(function (data) {
        $("#selectReinsuranceAccountingAccountConcept").UifSelect({ sourceData: data.data });
    });
}
function LoadTechnicalPrefixes() {
    ReinsuraceMovementsRequest.GetTechnicalPrefixes().done(function (data) {
        $("#selectReinsurancePrefix").UifSelect({ sourceData: data.data });
    });
}
function LoadTechnicalSubPrefixesByPrefixId(idPrefix) {
    ReinsuraceMovementsRequest.GetTechnicalSubPrefixesByPrefixId(idPrefix).done(function (data) {
        $("#selectReinsuranceSubPrefix").UifSelect({ sourceData: data.data });
    });
}
function LoadContractTypeEnabled() {
    ReinsuraceMovementsRequest.GetContractTypeEnabled().done(function (data) {
        $("#selectReinsuranceContractType").UifSelect({ sourceData: data.data });
    });
}
function LoadContractStretchs() {
    ReinsuraceMovementsRequest.GetContractStretchs(contractId).done(function (data) {
        $("#selectReinsuranceStretch").UifSelect({ sourceData: data.data });
    });
}
function LoadReinsuraceMovementsNature() {
    ReinsuraceMovementsRequest.GetNatures().done(function (data) {
        $("#selectReinsuranceNature").UifSelect({ sourceData: data.data });
    });
}
function LoadReinsuraceMovementsCurrencies() {
    ReinsuraceMovementsRequest.GetCurrencies().done(function (data) {
        $("#selectReinsuranceCurrency").UifSelect({ sourceData: data.data });
    });
}
function LoadReinsuraceMovementsYearMonths() {
    ReinsuraceMovementsRequest.GetYearMonths().done(function (data) {
        $("#selectReinsuranceApplicationMonth").UifSelect({ sourceData: data.data });
    });
}
function LoadReinsuraceMovementsCompanies(idCompany) {
    ReinsuraceMovementsRequest.GetAccountingCompanies().done(function (data) {
        if (isMulticompany == 0) {
            $("#selectReinsuranceCompany").UifSelect({ sourceData: data.data, selectedId: idCompany, enable: false });
        }
        else {
            $("#selectReinsuranceCompany").UifSelect({ sourceData: data.data });
        }

    });
}
///////////////////////////////////////////////////////
// Setea el valor del importe local en reaseguradora //
///////////////////////////////////////////////////////
function SetReinsuranceLocalAmount() {
    var reinsuranceAmount = $("#ReinsuranceAmount").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    var exchangeRate = $("#ReinsuranceExchangeRate").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    reinsuranceLocalAmount = reinsuranceAmount * exchangeRate;
    $("#ReinsuranceLocalAmount").val("$ " + NumberFormatSearch(reinsuranceLocalAmount, "2", ".", ","));
}

//////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios     //
//////////////////////////////////////////////////
function ValidateReinsuranceAddForm() {
    if ($('#selectReinsuranceBranch').val() == "") {
        $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
        return false;
    }
    if ($('#ReinsuranceAmount').val() == "") {
        $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
        return false;
    }
    if ($('#ReinsurerDocumentNumber').val() == $('#BrokerDocumentNumber').val()) {
        $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.ValidateReinsurerEqualBroker, "warning");
        return false;
    }
    if ($("#selectReinsuranceContractType").val() == 7) {
        if ($("#selectReinsuranceSlip").val() == "") {
            $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.SlipNumberRequired, "warning");
            return false;
        }
        if ($("#selectReinsurancePolicyBranch").val() == "") {
            $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.PolicyBranchRequired, "warning");
            return false;
        }
        if ($("#selectReinsurancePolicyPrefix").val() == "") {
            $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.PolicyPrefixRequired, "warning");
            return false;
        }
        if ($("#ReinsurancePolicyNumber").val() == "") {
            $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.PolicyRequired, "warning");
            return false;
        }
        if ($("#ReinsuranceEndorsementNumber").val() == "") {
            $("#modalReinsurance").find("#alertForm").UifAlert('show', Resources.EndorsementRequired, "warning");
            return false;
        }
    }

    return true;
}

//////////////////////////////////////////////////////////////
// Setear el total de la listview movimientos reaseguradora //
//////////////////////////////////////////////////////////////
function SetReinsuranceTotalMovement() {
    var totalMovement = 0;
    var totalDebitMovement = 0;

    var reinsurances = $("#reinsurancesListView").UifListView("getData");

    if (reinsurances != null) {

        for (var j = 0; j < reinsurances.length; j++) {
            if (reinsurances[j].AccountingNature == 1) {
                var reinsuranceCredit = String(reinsurances[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                totalMovement += parseFloat(reinsuranceCredit);
            } else {
                var reinsuranceDebit = String(reinsurances[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                totalDebitMovement += parseFloat(reinsuranceDebit);
            }
        }
    } else {
        $("#TotalReinsuranceMovement").text("");
        $("#TotalDebitReinsuranceMovement").text("");
    }
    $("#TotalReinsuranceMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
    $("#TotalDebitReinsuranceMovement").text("$ " + NumberFormatSearch(totalDebitMovement, "2", ".", ","));
}

//////////////////////////////////////////////////////////
/// Validación de duplicados movimientos reaseguradora ///
//////////////////////////////////////////////////////////
function ValidateDuplicateReinsurance() {
    var duplicate = -1;
    var checkingAccountConceptId = 0;
    var prefixId = 0;
    var subprefixId = 0;

    if (editReinsurance > -1) {
        $("#MovementMessageDuplicate").val("");
        duplicate = 0;
    }

    if (!($("#selectReinsuranceAccountingAccountConcept").val() == null))
        checkingAccountConceptId = $("#selectReinsuranceAccountingAccountConcept").val();

    if (!($("#selectReinsurancePrefix").val() == null))
        prefixId = $("#selectReinsurancePrefix").val();

    if (!($("#selectReinsuranceSubPrefix").val() == null))
        subprefixId = $("#selectReinsuranceSubPrefix").val();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckingAccountReinsurances/ValidateDuplicateReinsuranceCheckingAccount",
        data: {
            "branchId": $("#selectReinsuranceBranch").val(),
            "salePointId": $("#selectReinsuranceSalePoint").val(),
            "companyId": $("#selectReinsuranceCompany").val(),
            "reinsuranceId": reinsurerId,
            "accountingNatureId": $("#selectReinsuranceNature").val(),
            "checkingAccountConceptId": checkingAccountConceptId, //$("#selectReinsuranceAccountingAccountConcept").val(),
            "currencyId": $("#selectReinsuranceCurrency").val(),
            "agentId": brokerId,
            "prefixId": prefixId, //$("#selectReinsurancePrefix").val(),
            "subprefixId": subprefixId, //$("#selectReinsuranceSubPrefix").val(),
            "contractTypeId": $("#selectReinsuranceContractType").val(),
            "contractNumber": contractId,
            "sectionId": $("#selectReinsuranceStretch").val(),
            "applicationYear": $("#ReinsuranceApplicationYear").val(),
            "applicationMonth": $("#selectReinsuranceApplicationMonth").val()
        },
        success: function (data) {
            if (data[0].source == -1) {
                duplicate = 0;
                $("#MovementMessageDuplicate").val("");
            } else if (data[0].source >= 0) {
                duplicate = 1;
                if (data[0].source == 0) {
                    $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + ":" + data[0].imputationId);
                } else if (data[0].source == 1) {
                    $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + " " + data[0].imputationId);
                }
            }
        }
    });

    return duplicate;
}

///////////////////////////////////////////////////////////////////////
/// Validación de duplicados movimientos reaseguradora boton aceptar///
///////////////////////////////////////////////////////////////////////
function ValidateAcceptButtonDuplicateReinsurance(reasegData) {
    var duplicate = -1;
    var checkingAccountConceptId = 0;
    var prefixId = 0;
    var subprefixId = 0;

    /*    if (!($("#selectReinsuranceAccountingAccountConcept").val() == null))
            checkingAccountConceptId = $("#selectReinsuranceAccountingAccountConcept").val();
    
        if (!($("#selectReinsurancePrefix").val() == null))
            prefixId = $("#selectReinsurancePrefix").val();
    
        if (!($("#selectReinsuranceSubPrefix").val() == null))
            subprefixId = $("#selectReinsuranceSubPrefix").val();*/

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckingAccountReinsurances/ValidateDuplicateReinsuranceCheckingAccount",
        data: {
            "branchId": reasegData.BranchId,
            "salePointId": reasegData.SalePointId,
            "companyId": reasegData.CompanyId,
            "reinsuranceId": reasegData.ReinsurerId,
            "accountingNatureId": reasegData.NatureId,
            "checkingAccountConceptId": reasegData.ConceptId, //$("#selectReinsuranceAccountingAccountConcept").val(),
            "currencyId": reasegData.CurrencyId,
            "agentId": reasegData.BrokerId,
            "prefixId": reasegData.LineBusinessId, //$("#selectReinsurancePrefix").val(),
            "subprefixId": reasegData.SubLineBusinessId, //$("#selectReinsuranceSubPrefix").val(),
            "contractTypeId": reasegData.ContractTypeId,
            "contractNumber": reasegData.ContractId,
            "sectionId": reasegData.StretchId,
            "applicationYear": reasegData.ApplicationYear,
            "applicationMonth": reasegData.ApplicationMonth
        },
        success: function (data) {
            if (data[0].source == -1) {
                duplicate = 0;
                $("#MovementMessageDuplicate").val("");
            } else if (data[0].source >= 0) {
                duplicate = 1;
                if (data[0].source == 0) {
                    $("#MovementMessageDuplicate").val(Resources.AgentMessageValidateDuplicateTemporal + ":" + data[0].imputationId);
                } else if (data[0].source == 1) {
                    $("#MovementMessageDuplicate").val(Resources.AgentsMessageValidateDuplicateReal + " " + data[0].imputationId);
                }
            }
        }
    });

    return duplicate;
}
////////////////////////////////////////////////////////////////////////////
/// Setea los valores de los items a aplicar de cta. cte. reaseguradoras ///
////////////////////////////////////////////////////////////////////////////
function SetUpdateDataReinsuranceCheckingAccount() {
    oReinsuranceCheckingAccountUpdateModel = {
        ImputationId: 0,
        ReinsuranceCheckingAccountTransactionItems: []
    };

    oReinsuranceCheckingAccountItemUpdateModel = {
        ReinsuranceCheckingAccountItemId: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        LineBusinessId: 0,
        SubLineBusinessId: 0,
        AgentId: 0,
        ReinsuranceCompanyId: 0,
        IsFacultative: 0,
        SlipNumber: null,
        ContractTypeId: 0,
        ContractNumber: null,
        Section: null,
        Region: null,
        CheckingAccountConceptId: 0,
        AccountingNature: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        Period: 0,
        PolicyId: 0,
        EndorsementId: 0,
        ApplicationYear: 0,
        ApplicationMonth: 0
    };

    // Items a aplicar
    var reinsuranceItemsToApply = $("#reinsuranceItemsListView").UifListView("getData");

    if (reinsuranceItemsToApply != null) {
        for (var k = 0; k < reinsuranceItemsToApply.length; k++) {
            if (reinsuranceItemsToApply[k].Status == 1) {
                oReinsuranceCheckingAccountUpdateModel.ImputationId = tempImputationId;
                oReinsuranceCheckingAccountItemUpdateModel = {
                    ReinsuranceCheckingAccountItemId: 0,
                    BranchId: 0,
                    SalePointId: 0,
                    AccountingCompanyId: 0,
                    LineBusinessId: 0,
                    SubLineBusinessId: 0,
                    AgentId: 0,
                    ReinsuranceCompanyId: 0,
                    IsFacultative: 0,
                    SlipNumber: null,
                    ContractTypeId: 0,
                    ContractNumber: null,
                    Section: null,
                    Region: null,
                    CheckingAccountConceptId: 0,
                    AccountingNature: 0,
                    CurrencyCode: 0,
                    ExchangeRate: 0,
                    IncomeAmount: 0,
                    Amount: 0,
                    UserId: 0,
                    Description: null,
                    BillId: 0,
                    Period: 0,
                    PolicyId: 0,
                    EndorsementId: 0,
                    ApplicationYear: 0,
                    ApplicationMonth: 0
                };

                oReinsuranceCheckingAccountItemUpdateModel.ReinsuranceCheckingAccountItemId = reinsuranceItemsToApply[k].ReinsuranceCheckingAccountItemId;
                oReinsuranceCheckingAccountItemUpdateModel.BranchId = reinsuranceItemsToApply[k].BranchCode;
                oReinsuranceCheckingAccountItemUpdateModel.SalePointId = reinsuranceItemsToApply[k].PosCode;
                oReinsuranceCheckingAccountItemUpdateModel.AccountingCompanyId = reinsuranceItemsToApply[k].CompanyCode;
                oReinsuranceCheckingAccountItemUpdateModel.LineBusinessId = reinsuranceItemsToApply[k].LineBusinessCode;
                oReinsuranceCheckingAccountItemUpdateModel.SubLineBusinessId = reinsuranceItemsToApply[k].SubLineBusinessCode;
                oReinsuranceCheckingAccountItemUpdateModel.AgentId = reinsuranceItemsToApply[k].AgentCode;
                oReinsuranceCheckingAccountItemUpdateModel.ReinsuranceCompanyId = reinsuranceItemsToApply[k].ReinsuranceCompanyCode;
                oReinsuranceCheckingAccountItemUpdateModel.IsFacultative = reinsuranceItemsToApply[k].FacultativeCode;
                oReinsuranceCheckingAccountItemUpdateModel.SlipNumber = reinsuranceItemsToApply[k].SlipNumber;
                oReinsuranceCheckingAccountItemUpdateModel.ContractTypeId = reinsuranceItemsToApply[k].ContractTypeCode;
                oReinsuranceCheckingAccountItemUpdateModel.ContractNumber = reinsuranceItemsToApply[k].Contract;
                oReinsuranceCheckingAccountItemUpdateModel.Section = reinsuranceItemsToApply[k].Stretch;
                oReinsuranceCheckingAccountItemUpdateModel.Region = reinsuranceItemsToApply[k].Region;
                oReinsuranceCheckingAccountItemUpdateModel.CheckingAccountConceptId = reinsuranceItemsToApply[k].CheckingAccountConceptCode;
                oReinsuranceCheckingAccountItemUpdateModel.AccountingNature = reinsuranceItemsToApply[k].AccountingNature;
                oReinsuranceCheckingAccountItemUpdateModel.CurrencyCode = reinsuranceItemsToApply[k].CurrencyCode;
                oReinsuranceCheckingAccountItemUpdateModel.ExchangeRate = ClearFormatCurrency(reinsuranceItemsToApply[k].CurrencyChange).replace(",", ".");
                oReinsuranceCheckingAccountItemUpdateModel.IncomeAmount = ClearFormatCurrency(reinsuranceItemsToApply[k].IncomeAmount).replace(",", ".");
                oReinsuranceCheckingAccountItemUpdateModel.Amount = ClearFormatCurrency(reinsuranceItemsToApply[k].Amount).replace(",", ".");
                oReinsuranceCheckingAccountItemUpdateModel.UserId = userId;
                oReinsuranceCheckingAccountItemUpdateModel.Description = reinsuranceItemsToApply[k].Description;
                oReinsuranceCheckingAccountItemUpdateModel.BillId = reinsuranceItemsToApply[k].BillNumber;
                oReinsuranceCheckingAccountItemUpdateModel.Period = reinsuranceItemsToApply[k].Excercise;
                oReinsuranceCheckingAccountItemUpdateModel.PolicyId = reinsuranceItemsToApply[k].ReinsurerPolicyId;
                oReinsuranceCheckingAccountItemUpdateModel.EndorsementId = reinsuranceItemsToApply[k].ReinsurerEndorsementId;
                oReinsuranceCheckingAccountItemUpdateModel.ApplicationYear = reinsuranceItemsToApply[k].ApplicationYear;
                oReinsuranceCheckingAccountItemUpdateModel.ApplicationMonth = reinsuranceItemsToApply[k].ApplicationMonth;

                oReinsuranceCheckingAccountUpdateModel.ReinsuranceCheckingAccountTransactionItems.push(oReinsuranceCheckingAccountItemUpdateModel);
            }
        }
    }

    return oReinsuranceCheckingAccountUpdateModel;
}


///////////////////////////////////////////////////////////////////
/// Realiza la consulta de movimientos cta. cte. reaseguradoras ///
///////////////////////////////////////////////////////////////////
function RefreshReinsuranceMovements() {
    $("#reinsurancesListView").UifListView({
        autoHeight: true,
        source: ACC_ROOT + "CheckingAccountReinsurances/GetTempReinsuranceCheckingAccountItemByTempImputationId?tempImputationId=" + tempImputationId,
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: true,
        delete: true,
        displayTemplate: "#reinsurance-display-template",
        deleteCallback: deleteReinsuranceCallback
    });

    // setTimeout(function () {
    SetReinsuranceTotalMovement();
    //}, 1000);
}

/////////////////////////////////////////////////////////////////
/// Setea la compañía y sucursal por default - reaseguradoras ///
/////////////////////////////////////////////////////////////////
function SetReinsuranceAccountingCompany() {
    //    debugger
    if (isMulticompany == 0) {
        $("#selectReinsuranceCompany").val(accountingCompanyDefault);
        $("#selectReinsuranceCompany").attr("disabled", "disabled");
    } else {
        $("#selectReinsuranceCompany").removeAttr("disabled");
    }
    if (branchUserDefault > 0) {
        $("#selectReinsuranceBranch").val(branchUserDefault);

        if (branchUserDefault) {
            LoadReinsuraceMovementsSalesPointByBranchId(branchUserDefault, null)//jira SMT-2000 quitar puntos de venta por defecto.
            LoadAgentReinsuraceMovementsAccountConceptByBranchId(branchUserDefault);
        }
    } else {
        $("#selectReinsuranceBranch").val("");
    }
}

////////////////////////////////////////////////////////////////////
/// Valida la existencia de póliza y endoso para sucursal y ramo ///
////////////////////////////////////////////////////////////////////
function ValidateReinsurerPolicyAndEndorsement() {
    var validate = false;

    $("#modalReinsurance").find("#alertForm").UifAlert('hide');

    if ($('#ReinsuranceEndorsement').val() != "" && $('#ReinsurancePolicy').val() != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "CheckingAccountReinsurances/GetEndorsementIdByPolicyEndorsement",
            data: {
                "policyNumber": $('#ReinsurancePolicy').val(),
                "endorsementNumber": $("#ReinsuranceEndorsement").val(),
                "branchId": $("#selectReinsurancePolicyBranch").val(),
                "prefixId": $("#selectReinsurancePolicyPrefix").val()
            },
            success: function (data) {
                if (data == 0) {
                    var reinsurerMessage = Resources.ValidateEndorsementZero + $('#ReinsurancePolicy').val();
                    reinsurerMessage = reinsurerMessage + " " + Resources.ValidateEndorsementOne + " " + $("#selectReinsurancePolicyBranch option:selected").text();
                    reinsurerMessage = reinsurerMessage + " " + Resources.ValidateEndorsementTwo + " " + $("#selectReinsurancePolicyPrefix option:selected").text();
                    _endorsementId = 0;
                    $("#modalReinsurance").find("#alertForm").UifAlert('show', reinsurerMessage, "warning");
                    validate = true;
                } else {
                    _endorsementId = data;
                    validate = false;
                }
            }
        });
    }
    return validate;
}

/////////////////////////////////////////////////////////////
/// Setea los campos a valores iniciales - reaseguradoras ///
/////////////////////////////////////////////////////////////
function SetReinsuranceFieldEmpty() {
    $("#selectReinsuranceBranch").val("");
    $("#selectReinsuranceSalePoint").val("");
    $("#selectReinsuranceCompany").val("");
    $("#selectReinsurancePrefix").val("");
    $("#selectReinsuranceSubPrefix").val("");
    $("#ReinsurerDocumentNumber").val("");
    $("#ReinsurerName").val("");
    $("#selectReinsuranceContractType").val("");
    $("#ReinsurerContract").val("");
    $("#selectReinsuranceStretch").val("");
    $("#BrokerDocumentNumber").val("");
    $("#BrokerName").val("");
    $("#ReinsuranceMovementDescription").val("");
    $("#selectReinsuranceAccountingAccountConcept").val("");
    $("#selectReinsuranceNature").val("");
    $("#selectReinsuranceCurrency").val("");
    $("#ReinsuranceExchangeRate").val("");
    $("#ReinsuranceAmount").val("");
    $("#ReinsuranceLocalAmount").val("");
    $("#ReinsuranceApplicationYear").val("");
    $("#selectReinsuranceApplicationMonth").val("");
    $("#selectReinsuranceSlip").val("");
    $("#selectReinsurancePolicyBranch").val("");
    $("#selectReinsurancePolicyPrefix").val("");
    $("#ReinsurancePolicy").val("");
    $("#ReinsuranceEndorsement").val("");
    reinsuranceLocalAmount = 0;
}

//////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de cta.cte. reaseguradoras con los valores ingresados ///
//////////////////////////////////////////////////////////////////////////////
function SetDataReinsuranceCheckingAccount() {
    SetDataReinsuranceCheckingAccountEmpty();

    oReinsuranceCheckingAccountModel.ImputationId = tempImputationId;

    var movements = $("#reinsurancesListView").UifListView("getData");
    if (movements != null) {
        for (var j = 0; j < movements.length; j++) {

            if (movements[j].Status == 1) {
                oReinsuranceCheckingAccountItemModel = {
                    ReinsuranceCheckingAccountItemId: 0,
                    BranchId: 0,
                    SalePointId: 0,
                    AccountingCompanyId: 0,
                    LineBusinessId: 0,
                    SubLineBusinessId: 0,
                    AgentId: 0,
                    ReinsuranceCompanyId: 0,
                    IsFacultative: 0,
                    SlipNumber: null,
                    ContractTypeId: 0,
                    ContractNumber: null,
                    Section: null,
                    Region: null,
                    CheckingAccountConceptId: 0,
                    AccountingNature: 0,
                    CurrencyCode: 0,
                    ExchangeRate: 0,
                    IncomeAmount: 0,
                    Amount: 0,
                    UserId: 0,
                    Description: null,
                    BillId: 0,
                    Period: 0,
                    PolicyId: 0,
                    EndorsementId: 0,
                    ApplicationYear: 0,
                    ApplicationMonth: 0,
                    ReinsuranceCheckingAccountTransactionChild: []
                };

                oReinsuranceCheckingAccountItemModel.ReinsuranceCheckingAccountItemId = movements[j].ReinsurerCheckingAccountItemId;
                oReinsuranceCheckingAccountItemModel.BranchId = movements[j].BranchId;
                oReinsuranceCheckingAccountItemModel.SalePointId = movements[j].SalePointId;
                oReinsuranceCheckingAccountItemModel.AccountingCompanyId = movements[j].CompanyId;
                oReinsuranceCheckingAccountItemModel.LineBusinessId = movements[j].LineBusinessId;
                oReinsuranceCheckingAccountItemModel.SubLineBusinessId = movements[j].SubLineBusinessId;
                oReinsuranceCheckingAccountItemModel.AgentId = movements[j].BrokerId;
                oReinsuranceCheckingAccountItemModel.ReinsuranceCompanyId = movements[j].ReinsurerId;
                oReinsuranceCheckingAccountItemModel.IsFacultative = movements[j].FacultativeId;
                oReinsuranceCheckingAccountItemModel.SlipNumber = movements[j].SlipNumber;
                oReinsuranceCheckingAccountItemModel.ContractTypeId = movements[j].ContractTypeId;
                oReinsuranceCheckingAccountItemModel.ContractNumber = movements[j].ContractNumber;
                oReinsuranceCheckingAccountItemModel.Section = movements[j].StretchId;
                oReinsuranceCheckingAccountItemModel.Region = movements[j].Region;
                oReinsuranceCheckingAccountItemModel.CheckingAccountConceptId = movements[j].ConceptId;
                oReinsuranceCheckingAccountItemModel.AccountingNature = movements[j].AccountingNature;
                oReinsuranceCheckingAccountItemModel.CurrencyCode = movements[j].CurrencyId;
                oReinsuranceCheckingAccountItemModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].ExchangeRate).replace(",", "."));
                oReinsuranceCheckingAccountItemModel.IncomeAmount = ReplaceDecimalPoint(String(movements[j].LocalAmount).replace(",", "."));
                oReinsuranceCheckingAccountItemModel.Amount = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].Amount).replace(",", "."));
                oReinsuranceCheckingAccountItemModel.UserId = userId;
                oReinsuranceCheckingAccountItemModel.Description = movements[j].Description;
                oReinsuranceCheckingAccountItemModel.BillId = movements[j].BillNumber;
                oReinsuranceCheckingAccountItemModel.Period = movements[j].Excercise;
                oReinsuranceCheckingAccountItemModel.PolicyId = movements[j].ReinsurerPolicyId;
                oReinsuranceCheckingAccountItemModel.EndorsementId = movements[j].ReinsurerEndorsementId;
                oReinsuranceCheckingAccountItemModel.ApplicationYear = movements[j].ApplicationYear;
                oReinsuranceCheckingAccountItemModel.ApplicationMonth = movements[j].ApplicationMonth;

                var reinsuranceItemsToApply = $("#reinsuranceItemsListView").UifListView("getData");

                if (reinsuranceItemsToApply != null) {
                    for (var k = 0; k < reinsuranceItemsToApply.length; k++) {

                        oReinsuranceCheckingAccountItemChildModel = {
                            ReinsuranceCheckingAccountItemId: 0,
                            BranchId: 0,
                            SalePointId: 0,
                            AccountingCompanyId: 0,
                            LineBusinessId: 0,
                            SubLineBusinessId: 0,
                            AgentId: 0,
                            ReinsuranceCompanyId: 0,
                            IsFacultative: 0,
                            SlipNumber: null,
                            ContractTypeId: 0,
                            ContractNumber: null,
                            Section: null,
                            Region: null,
                            CheckingAccountConceptId: 0,
                            AccountingNature: 0,
                            CurrencyCode: 0,
                            ExchangeRate: 0,
                            IncomeAmount: 0,
                            Amount: 0,
                            UserId: 0,
                            Description: null,
                            BillId: 0,
                            ReinsuranceCheckingAccountId: 0,
                            Period: 0,
                            PolicyId: 0,
                            EndorsementId: 0,
                            ApplicationYear: 0,
                            ApplicationMonth: 0
                        };

                        oReinsuranceCheckingAccountItemChildModel.ReinsuranceCheckingAccountItemId = movements[j].ReinsuranceCheckingAccountItemId;
                        oReinsuranceCheckingAccountItemChildModel.BranchId = reinsuranceItemsToApply[k].BranchCode;
                        oReinsuranceCheckingAccountItemChildModel.SalePointId = reinsuranceItemsToApply[k].LineBusinessCode;
                        oReinsuranceCheckingAccountItemChildModel.AccountingCompanyId = movements[j].CompanyCode;
                        oReinsuranceCheckingAccountItemChildModel.LineBusinessId = movements[j].LineBusinessCode;
                        oReinsuranceCheckingAccountItemChildModel.SubLineBusinessId = movements[j].SubLineBusinessCode;
                        oReinsuranceCheckingAccountItemChildModel.AgentId = movements[j].AgentCode;
                        oReinsuranceCheckingAccountItemChildModel.ReinsuranceCompanyId = movements[j].ReinsuranceCompanyCode;
                        oReinsuranceCheckingAccountItemChildModel.IsFacultative = movements[j].FacultativeCode;
                        oReinsuranceCheckingAccountItemChildModel.SlipNumber = movements[j].SlipNumber;
                        oReinsuranceCheckingAccountItemChildModel.ContractTypeId = movements[j].ContractTypeCode;
                        oReinsuranceCheckingAccountItemChildModel.ContractNumber = movements[j].Contract;
                        oReinsuranceCheckingAccountItemChildModel.Section = movements[j].Stretch;
                        oReinsuranceCheckingAccountItemChildModel.Region = movements[j].Region;
                        oReinsuranceCheckingAccountItemChildModel.CheckingAccountConceptId = movements[j].CheckingAccountConceptCode;
                        oReinsuranceCheckingAccountItemChildModel.AccountingNature = movements[j].AccountingNature;
                        oReinsuranceCheckingAccountItemChildModel.CurrencyCode = reinsuranceItemToApplyRow.CurrencyCode;
                        oReinsuranceCheckingAccountItemChildModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(movements[j].CurrencyChange).replace(",", "."));
                        oReinsuranceCheckingAccountItemChildModel.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency(reinsuranceItemToApply[k].IncomeAmount).replace(",", "."));
                        oReinsuranceCheckingAccountItemChildModel.Amount = ReplaceDecimalPoint(ClearFormatCurrency(reinsuranceItemToApply[k].IncomeAmount).replace(",", "."));
                        oReinsuranceCheckingAccountItemChildModel.UserId = userId;
                        oReinsuranceCheckingAccountItemChildModel.Description = movements[j].Description;
                        oReinsuranceCheckingAccountItemChildModel.BillId = movements[j].BillNumber;
                        oReinsuranceCheckingAccountItemChildModel.ReinsuranceCheckingAccountId = reinsuranceItemsToApply[k].ReinsuranceCheckingAccountItemId;
                        oReinsuranceCheckingAccountItemChildModel.Period = movements[j].Excercise;
                        oReinsuranceCheckingAccountItemChildModel.PolicyId = movements[j].ReinsurerPolicyId;
                        oReinsuranceCheckingAccountItemChildModel.EndorsementId = movements[j].ReinsurerEndorsementId;
                        oReinsuranceCheckingAccountItemChildModel.ApplicationYear = movements[j].ApplicationYear;
                        oReinsuranceCheckingAccountItemChildModel.ApplicationMonth = movements[j].ApplicationMonth;

                        oReinsuranceCheckingAccountItemModel.ReinsuranceCheckingAccountTransactionChild.push(oReinsuranceCheckingAccountItemChildModel);
                    }
                }
                oReinsuranceCheckingAccountModel.ReinsuranceCheckingAccountTransactionItems.push(oReinsuranceCheckingAccountItemModel);
            }
        }
    }

    return oReinsuranceCheckingAccountModel;
}

////////////////////////////////////////////////////////////////////////
/// Setea el modelo de cta.cte. reaseguradoras con valores iniciales ///
////////////////////////////////////////////////////////////////////////
function SetDataReinsuranceCheckingAccountEmpty() {
    oReinsuranceCheckingAccountModel = {
        ImputationId: 0,
        ReinsuranceCheckingAccountTransactionItems: []
    };

    oReinsuranceCheckingAccountItemModel = {
        ReinsuranceCheckingAccountItemId: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        LineBusinessId: 0,
        SubLineBusinessId: 0,
        AgentId: 0,
        ReinsuranceCompanyId: 0,
        IsFacultative: 0,
        SlipNumber: null,
        ContractTypeId: 0,
        ContractNumber: null,
        Section: null,
        Region: null,
        CheckingAccountConceptId: 0,
        AccountingNature: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        Period: 0,
        PolicyId: 0,
        EndorsementId: 0,
        ApplicationYear: 0,
        ApplicationMonth: 0,
        ReinsuranceCheckingAccountTransactionChild: []
    };
}

////////////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de items cta.cte. reaseguradoras con los valores ingresados ///
////////////////////////////////////////////////////////////////////////////////////
function SetDataReinsuranceCheckingAccountItem(reinsuranceTempCheckingAccountCode) {
    SetDataReinsuranceCheckingAccountEmpty();

    oReinsuranceCheckingAccountModel.ImputationId = tempImputationId;

    var reinsuranceItemsToApply = $("#reinsuranceItemsListView").UifListView("getData");

    oReinsuranceCheckingAccountItemModel = {
        ReinsuranceCheckingAccountItemId: 0,
        BranchId: 0,
        SalePointId: 0,
        AccountingCompanyId: 0,
        LineBusinessId: 0,
        SubLineBusinessId: 0,
        AgentId: 0,
        ReinsuranceCompanyId: 0,
        IsFacultative: 0,
        SlipNumber: null,
        ContractTypeId: 0,
        ContractNumber: null,
        Section: null,
        Region: null,
        CheckingAccountConceptId: 0,
        AccountingNature: 0,
        CurrencyCode: 0,
        ExchangeRate: 0,
        IncomeAmount: 0,
        Amount: 0,
        UserId: 0,
        Description: null,
        BillId: 0,
        Period: 0,
        PolicyId: 0,
        EndorsementId: 0,
        ApplicationYear: 0,
        ApplicationMonth: 0,
        ReinsuranceCheckingAccountTransactionChild: []
    };

    if (reinsuranceItemsToApply != null) {
        for (var j = 0; j < reinsuranceItemsToApply.length; j++) {
            oReinsuranceCheckingAccountItemModel.ReinsuranceCheckingAccountItemId = reinsuranceTempCheckingAccountCode;

            oReinsuranceCheckingAccountItemChildModel = {
                ReinsuranceCheckingAccountItemId: 0,
                BranchId: 0,
                SalePointId: 0,
                AccountingCompanyId: 0,
                LineBusinessId: 0,
                SubLineBusinessId: 0,
                AgentId: 0,
                ReinsuranceCompanyId: 0,
                IsFacultative: 0,
                SlipNumber: null,
                ContractTypeId: 0,
                ContractNumber: null,
                Section: null,
                Region: null,
                CheckingAccountConceptId: 0,
                AccountingNature: 0,
                CurrencyCode: 0,
                ExchangeRate: 0,
                IncomeAmount: 0,
                Amount: 0,
                UserId: 0,
                Description: null,
                BillId: 0,
                ReinsuranceCheckingAccountId: 0,
                Period: 0,
                PolicyId: 0,
                EndorsementId: 0,
                ApplicationYear: 0,
                ApplicationMonth: 0
            };

            oReinsuranceCheckingAccountItemChildModel.ReinsuranceCheckingAccountItemId = reinsuranceItemsToApply[j].ReinsuranceCheckingAccountItemId;
            oReinsuranceCheckingAccountItemChildModel.ReinsuranceCheckingAccountId = reinsuranceItemsToApply[j].ReinsuranceCheckingAccountItemId;
            oReinsuranceCheckingAccountItemModel.ReinsuranceCheckingAccountTransactionChild.push(oReinsuranceCheckingAccountItemChildModel);
        }
    }

    oReinsuranceCheckingAccountModel.ReinsuranceCheckingAccountTransactionItems.push(oReinsuranceCheckingAccountItemModel);

    return oReinsuranceCheckingAccountModel;
}

var saveReinsuranceCallback = function (deferred, data) {
    $("#reinsurancesListView").UifListView("addItem", data);
    deferred.resolve();
};

var editReinsuranceCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteReinsuranceCallback = function (deferred, data) {
    deferred.resolve();

    setTimeout(function () {

        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "CheckingAccountReinsurances/DeleteTempReinsuranceCheckingAccountItem",
            data: {
                "tempImputationCode": tempImputationId,
                "tempReinsuranceCheckingAccountCode": data.ReinsurerCheckingAccountItemId
            },
            success: function () {
                RefreshReinsuranceMovements();
                SetReinsuranceTotalMovement();
            }
        });

        SetReinsuranceTotalMovement();
    }, 1000);
};

///////////////////////////////////////////////////
//  Obtiene fecha del servidor                   //
//////////////////////////////////////////////////
function getDate() {
    var systemDate;
    $.ajax({
        type: "GET",
        async: false,
        url: ACC_ROOT + "Common/GetDate",
        success: function (data) {
            systemDate = data;
        }
    });

    return systemDate;
} 
