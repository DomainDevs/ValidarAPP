class Search extends Uif2.Page {

    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#inputPolicyNumber').ValidatorKey(ValidatorType.Number, 0, 0);
        $('#labelEmission').hide();
        $("#rowConsultationRisk").hide();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputTemporal").ValidatorKey(ValidatorType.Number, 2, 0);
        $("#btnEndorsementList").hide();
        if (glbPolicy == null) {
            glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, Endorsement: {}, Change: false, BoolParameter: false };
        }

        if (GetQueryParameter("BranchId") !== undefined && GetQueryParameter("PrefixId") !== undefined && GetQueryParameter("PolicyNumber") !== undefined && GetQueryParameter("EndorsementId") !== undefined && GetQueryParameter("ProductIsCollective") !== undefined) {

            glbPolicy.Endorsement = { Id: GetQueryParameter("EndorsementId") };
            glbPolicy.productIsCollective = GetQueryParameter("ProductIsCollective");
            glbPolicy.Branch = { Id: GetQueryParameter("BranchId") };
            glbPolicy.DocumentNumber = GetQueryParameter("PolicyNumber");
            glbPolicy.Prefix = { Id: GetQueryParameter("PrefixId") };
            $("#inputPolicyNumber").val(GetQueryParameter("PolicyNumber"));
        }

        Search.HideItemsEndorsement();
        if (prefixEndoEnabledList.length > 0) {
            $.each(prefixEndoEnabledList, function (key, value) {
                Search.ShowItemsEndorsement(this.EndorsementCode);
            });
        }
        else if ($("#selectPrefix").UifSelect("getSelected") != null && $("#selectPrefix").UifSelect("getSelected") != "") {
            Search.GetPrefixEndoEnabled($("#selectPrefix").UifSelect("getSelected"));
        }

        Search.InitialLoad().done(function (daResult) {
            Search.HidePanelsSearch();
            Search.HideTitlePanelSearch();
            Search.HideSearchPlate();
            GetDateIssue();
            //Search.checkhierarchy();

            if (glbPolicy.DocumentNumber != null) {
                Search.LoadPolicy();
            }
        });
        Search.UniquePolicy();
    }

    bindEvents() {

        $('#selectBranch').on('itemSelected', function (event, selectedItem) {
            Search.HideSearchPlate();
            if (selectedItem.Id > 0) {
                Search.ClearGlobalPolicy();
            }
        });
        $("#btnExit").on("click", function () {
            window.location = rootPath + "Home/Index";
        });
        $('#selectPrefix').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                Search.ClearGlobalPolicy();
            }
            Search.GetPrefix(selectedItem);
        });
        $('#inputPolicyNumber').focusout(function () {
            $('#inputPolicyNumber').val($('#inputPolicyNumber').val().trim());
        });


        $('#inputPolicyNumber').on('buttonClick', function () {
            glbPolicy.Message = "";
            if ($('#inputPolicyNumber').val().trim().length > 0 /*&& $("#selectPrefix").UifSelect("getSelected") != ""*/) {
                if (glbPolicy.BoolParameter) {
                    if ($('#selectPrefix').UifSelect("getSelected") == "" && $('#selectBranch').UifSelect("getSelected") != "") {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.PrefixRequired, 'autoclose': true });
                    }
                    else if ($('#selectPrefix').UifSelect("getSelected") != "" && $('#selectBranch').UifSelect("getSelected") == "") {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchRequired, 'autoclose': true });
                    }
                    else {
                        Search.searchPolicyNumber();
                    }
                } else if ($('#selectPrefix').UifSelect("getSelected") == "" || $('#selectBranch').UifSelect("getSelected") == "" || $('#inputPolicyNumber').val().trim().length == 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchPrefixAndDocumentNumRequired, 'autoclose': true });
                } else {
                    Search.searchPolicyNumber();
                }

            }
        });
        $("#btnCancelSearch").on('click', function () {
            Search.ClearDataSearch();
            Search.SummaryClear();
            Search.ClearSummary();
        });

        $("#btnExtensionPolicy").on('click', function () {
            Search.EndorsementExtension();
        });

        $("#btnRiskModification").on('click', function () {
            Search.GetRiskEndorsement();
        });
        $("#btnRenewalPolicy").on('click', function () {
            Search.EndorsementRenewal();
        });
        $("#btnChangeTermPolicy").on('click', function () {
            Search.RedirectController(EndorsementType.ChangeTerm, EndorsementMovements.ChangeTerm, EndorsementController.ChangeTerm, true);
        });
        $("#btnChangeAgent").on('click', function () {
            Search.RedirectController(EndorsementType.ChangeAgent, EndorsementMovements.ChangeAgent, EndorsementController.ChangeAgent, true);
        });
        $("#btnChangePolicyHolder").on('click', function () {
            Search.RedirectController(EndorsementType.PolicyHonderChange, EndorsementMovements.ChangePolicyHolder, EndorsementController.ChangePolicyHolder, true);
        });
        $("#btnChangeConsolidation").on('click', function () {
            Search.RedirectController(EndorsementType.ChangeConsolidation, EndorsementMovements.ChangeConsolidation, EndorsementController.ChangeConsolidation, true);
        });
        $("#btnChangeCoinsurance").on('click', function () {
            if (glbPolicy.BusinessType != 1) {
                Search.RedirectController(EndorsementType.ChangeCoinsurance, EndorsementMovements.ChangeCoinsurance, EndorsementController.ChangeCoinsurance, true);
            } else {
                $.UifDialog('alert', { 'message': Resources.Language.LabelPolicyNotExistCoinsurance }, null);
            }
        });
        $("#btnCancelPolicy").on('click', function () {
            Search.CancellationEndorsement();
        });
        $("#btnAdjustment").on('click', function () {
            Search.EndorsementAdjust();
        });
        $("#btnCreditNote").on('click', function () {
            Search.EndorsementCreditNote();
        });
        $("#btnDeclaration").on('click', function () {
            Search.EndorsemenDeclaration();
        });
        $("#btnReversionEndorsement").on('click', function () {
            Search.EndorsementReversion();
        });
        $("#btnInclusionExlusionRisk").on('click', function () {
            if (glbPolicy.IsMassive != null && glbPolicy.IsMassive && glbPolicy.isCollective) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCollective, 'autoclose': true });
                return;
            }
            if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                Search.RedirectCollectiveEndorsement('Collective/EndorsementModification');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ButtonNotApplyIndividualPolicies, 'autoclose': true });
            }
        });
        $("#btnRiskBilling").on('click', function () {
            Search.RedirectController(EndorsementMovements.Billing, EndorsementController.Billing, true);
        });
        $("#btnText").on('click', function () {
            $('#inputTextPrecataloged').val('');
            if (glbPolicy.Endorsement.PolicyId != 0) {
                LoadPartialText();
            }
        });
        $('#btnSendExcel').on('click', function () {
            Search.SendToExcel();
        });
        $("#btnPrintPolicy").on('click', function () {
            if (glbPolicy.Endorsement.PolicyId > 0) {
                Search.RedirectPrinting(EndorsementController.Printer);
            }
        });
        $("#btnTextChange").on('click', function () {
            Search.ChangeText();
        });
        $("#btnClauseChange").on('click', function () {
            Search.ChangeClause();
        });
        $('#btnDeleteTemporal').on('click', function () {
            if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
                Search.ValidateTemporal($('#inputPolicyNumber').val(), $("#selectBranch").UifSelect("getSelected"), $("#selectPrefix").UifSelect("getSelected")).then(function (dataTemporal) {
                    if (dataTemporal.Id == -1) {
                        $("#panelButtonsRight").hide();
                    }
                    else {
                        SearchRequest.ValidateDeleteTemporal(glbPolicy.Endorsement.Id).done(function (data) {
                            if (data.success) {
                                Search.DeleteTemporal();
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                });

            }
        });
        $("#inputDescriptionRisk").on("buttonClick", function () {
            Search.LoadEndorsementCollective();
        });
        $("#btnIdenticalCondition").on('click', function () {
            lockScreen();
            glbPolicy.IsUnderIdenticalConditions = true;
            Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, 'Renewal', 'Renewal', false).then(function (dataTemporal) {
                if (dataTemporal) {
                    var renewalModel = Search.CreatePolicyBase(EndorsementType.Renewal);
                    //Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, 'Renewal', 'Renewal', true);
                    Search.CreateTemporal(renewalModel);
                }
            });

        });
        $("#btnOtherCondition").on('click', function () {
            lockScreen();
            Search.RenewalotherCondition();
        });
        $('#inputTemporal').on('buttonClick', this.SearchTemporal);
        $("#btnEndorsementList").on("click", this.ShowViewListEndorsement);
        $("#btnListEndorsementClose").on("click", this.HidenViewListEndorsement);
    }

    SearchTemporal() {
        Search.GetTemporalById($('#inputTemporal').val().trim());
    }
    static UniquePolicy() {
        SearchRequest.GetUniquePolicyId().done(function (data) {
            if (data.success) {
                glbPolicy.BoolParameter = data.result.BoolParameter
            }
        });
    }

    ShowViewListEndorsement() {
        if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            Search.GetListEndorsement(glbPolicy.Branch.Id, glbPolicy.Prefix.Id, glbPolicy.DocumentNumber, true);
        }

    }

    HidenViewListEndorsement() {
        $("#modalListEndorsement").UifModal('hide');
    }
    static GetPrefix(selectedItem) {
        Search.HideSearchPlate();
        Search.HideItemsEndorsement();
        if (selectedItem.Id > 0) {
            Search.ClearGlobalPolicy();
            Search.HideItemsEndorsementComun();
            Search.GetPrefixEndoEnabled(selectedItem.Id);
        }
    }

    static GetRiskEndorsement() {
        Search.RedirectController(EndorsementType.Modification, EndorsementMovements.Modification, EndorsementController.Modification, false);
    }


    static CancellationEndorsement() {
        Search.RedirectController(EndorsementType.Cancellation, EndorsementMovements.Cancellation, EndorsementController.Cancellation, false);
    }
    static ChangeText() {
        Search.RedirectController(EndorsementType.Modification, EndorsementMovements.EndorsementChangeText, EndorsementController.Modification, true);
    }

    static ChangeClause() {
        Search.RedirectController(EndorsementType.Modification, EndorsementMovements.EndorsementModificationClause, EndorsementController.Modification, true);
    }
    static ChangeTerm() {
        Search.RedirectController(EndorsementType.Renewal, EndorsementMovements.ChangeTerm, EndorsementController.ChangeTerm, true);
    }
    static EndorsementExtension() {
        if (Search.ExtensionOtherPrefix()) {
            Search.RedirectController(EndorsementType.EffectiveExtension, EndorsementMovements.Extension, EndorsementController.Extension, true);
        }
    }
    static EndorsementRenewal() {
        if (Search.ValidateBase(true)) {
            if (glbPolicy.Id > 0) {
                var objectModel = Search.CreatePolicyBase(EndorsementType.Renewal);
                SearchRequest.ValidateEndorsement(objectModel).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            glbPolicy.HasEvent = data.result.HasEvent;
                            glbPolicy.IssueDate = FormatFullDate(data.result.IssueDate);
                            glbPolicy.Message = data.result.Message;
                            if (data.result.Message != "" && !glbPolicy.HasEvent) {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result.Message, 'autoclose': true });
                            }
                            else {
                                if (!Search.RenewalOtherPrefix()) {
                                    if (!glbPolicy.IsUnderIdenticalConditions) {
                                        router.run("prtTemporal");
                                    }
                                    else {
                                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, 'Renewal', 'Renewal', true);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                if (!Search.RenewalOtherPrefix()) {
                    Search.ShowPanelsSearch(MenuType.Renewal);
                }
            }
        }
    }
    static RenewalotherCondition() {
        glbPolicy.IsUnderIdenticalConditions = false;
        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, 'Renewal', 'Renewal', false).then(function (dataTemporal) {
            if (dataTemporal) {
                var renewalModel = Search.CreatePolicyBase(EndorsementType.Renewal);
                renewalModel.CurrentFrom = FormatDate(glbPolicy.Endorsement.CurrentTo);
                renewalModel.CurrentTo = FormatDate(AddToDate(glbPolicy.Endorsement.CurrentTo, 0, 0, 1));
                renewalModel.ModifyFrom = FormatDate(glbPolicy.Endorsement.CurrentTo);
                renewalModel.ModifyTo = FormatDate(AddToDate(glbPolicy.Endorsement.CurrentTo, 0, 0, 1));
                renewalModel.IssueDate = FormatDate(AddToDate(glbPolicy.IssueDate, 0, 0, 1));
                Search.CreateTemporal(renewalModel);
            }
        });
    }

    static EndorsementReversion() {
        if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCollective, 'autoclose': true });
                return;
            }
            else {
                if (glbPolicy.Endorsement.Number < 1) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.DisabledEmisionReversion, 'autoclose': true });
                }
                else {
                    var resultEndorsement = Search.ValidateEndorsementBase(EndorsementType.LastEndorsementCancellation);
                    $.when(resultEndorsement).done(function (resultData) {
                        if (resultData)
                            Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Reversion, EndorsementController.Reversion, true);
                    });

                }
            }
        }
    }
    static EndorsementAdjust() {
        if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            if (glbPolicy.Id) {
                var resultEndorsement = Search.ValidateEndorsementBase(EndorsementType.Adjustment);
                $.when(resultEndorsement).done(function (resultData) {
                    if (resultData)
                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Adjustment, EndorsementController.Adjustment, true);
                });
            }
            else {
                var subcovered = Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Adjustment, EndorsementController.Adjustment, false);
                $.when(subcovered).done(function (resul) {
                    switch (glbPolicy.Product.CoveredRisk.SubCoveredRiskType) {
                        case SubCoveredRiskType.Transport:
                            if (glbPolicy.PolicyType.IsFloating) {
                                if (glbPolicy.Endorsement.PolicyId > 0) {
                                    SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                                        if (data.success) {
                                            if (data.result.CanMakeEndorsement && data.result.AllowEndorsement == EndorsementType.Adjustment) {
                                                Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Adjustment, EndorsementController.Adjustment, true);
                                            }
                                            else if (data.result.AllowEndorsement == 0) {
                                                $.UifDialog('alert', { 'message': data.result.Message }, null);
                                            } else {
                                                $.UifDialog('alert', { 'message': Resources.Language.UnavaibleAdjustmentEndorsement }, null);
                                            }
                                        }
                                        else {
                                            $.UifDialog('alert', { 'message': data.result.Message }, null);
                                        }
                                    });
                                }
                            }
                            else {
                                $.UifDialog('alert', { 'message': Resources.Language.EndorsmentNotApply }, null);
                            }
                            break;
                        case SubCoveredRiskType.Property:
                            switch (glbPolicy.Prefix.Id) {
                                case PrefixType.INCENDIO:
                                case PrefixType.SUSTRACCION:
                                case PrefixType.ROTURA_DE_MAQUINARIA:
                                case PrefixType.CORRIENTE_DEBIL:
                                    if (glbPolicy.Endorsement.PolicyId > 0) {
                                        SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                                            if (data.success) {
                                                if (data.result.CanMakeEndorsement && data.result.AllowEndorsement == EndorsementType.Adjustment) {
                                                    SearchRequest.ValidateDeclarativeInsuredObjects(glbPolicy.Endorsement.PolicyId).done(function (result) {
                                                        if (result.success) {
                                                            if (result.result) {
                                                                Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Adjustment, EndorsementController.Adjustment, true);
                                                            } else {
                                                                Search.GenerateErrorMessages(glbPolicy.Prefix.Id, glbPolicy.Prefix.Description);
                                                            }
                                                        } else {
                                                            Search.GenerateErrorMessages(glbPolicy.Prefix.Id, glbPolicy.Prefix.Description);
                                                        }
                                                    })
                                                } else {
                                                    $.UifDialog('alert', { 'message': data.result.Message }, null);
                                                }
                                            }
                                            else {
                                                $.UifDialog('alert', { 'message': Resources.Language.ErrorValidatingAdjustmentEndorsement }, null);
                                            }
                                        });
                                    } else {
                                        $.UifDialog('alert', { 'message': Resources.Language.ErrorNoPolicySelected }, null);
                                    }
                                    break;
                            }
                            break;
                    }
                });
            }
        }
    }
    static EndorsemenDeclaration() {
        if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            var subcovered = Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Declaration, EndorsementController.Declaration, false);
            $.when(subcovered).done(function (resul) {
                switch (glbPolicy.Product.CoveredRisk.SubCoveredRiskType) {
                    case SubCoveredRiskType.Transport:
                        if (glbPolicy.PolicyType.IsFloating) {
                            if (glbPolicy.Endorsement.PolicyId > 0) {
                                SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                                    if (data.success) {
                                        if (data.result.CanMakeEndorsement && data.result.AllowEndorsement == EndorsementType.Declaration) {
                                            Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Declaration, EndorsementController.Declaration, true);
                                        } else if (data.result.AllowEndorsement == 0) {
                                            $.UifDialog('alert', { 'message': data.result.Message }, null);
                                        }
                                        else {
                                            $.UifDialog('alert', { 'message': Resources.Language.UnavaibleDeclarationEndorsement }, null);
                                        }
                                    }
                                    else {
                                        $.UifDialog('alert', { 'message': Resources.Language.ErrorValidatingDeclarationEndorsement }, null);
                                    }
                                });
                            }
                        } else {
                            $.UifDialog('alert', { 'message': Resources.Language.EndorsmentNotApply }, null);
                        }
                        break;
                    case SubCoveredRiskType.Property:
                        switch (glbPolicy.Prefix.Id) {
                            case PrefixType.INCENDIO:
                            case PrefixType.SUSTRACCION:
                            case PrefixType.ROTURA_DE_MAQUINARIA:
                            case PrefixType.CORRIENTE_DEBIL:
                                if (glbPolicy.Endorsement.PolicyId > 0) {
                                    SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                                        if (data.success) {
                                            if (data.result.CanMakeEndorsement && data.result.AllowEndorsement == EndorsementType.Declaration) {
                                                SearchRequest.ValidateDeclarativeInsuredObjects(glbPolicy.Endorsement.PolicyId).done(function (result) {
                                                    if (result.success) {
                                                        if (result.result) {
                                                            Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Declaration, EndorsementController.Declaration, true);
                                                        } else {
                                                            Search.GenerateErrorMessages(glbPolicy.Prefix.Id, glbPolicy.Prefix.Description);
                                                        }
                                                    } else {
                                                        Search.GenerateErrorMessages(glbPolicy.Prefix.Id, glbPolicy.Prefix.Description);
                                                    }
                                                })
                                            } else {
                                                $.UifDialog('alert', { 'message': data.result.Message }, null);
                                            }
                                        }
                                        else {
                                            $.UifDialog('alert', { 'message': Resources.Language.ErrorValidatingDeclarationEndorsement }, null);
                                        }
                                    });
                                }
                                break;
                        }
                        break;
                }
            });
        }
    }
    static EndorsementCreditNote() {
        if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            if (glbPolicy.Id) {
                var resultEndorsement = Search.ValidateEndorsementBase(EndorsementType.Adjustment);
                $.when(resultEndorsement).done(function (resultData) {
                    if (resultData)
                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.CreditNote, EndorsementController.CreditNote, true);
                });
            }
            else {
                if (!Isnull(glbPolicy.PolicyType)) {
                    if (glbPolicy.Endorsement.PolicyId > 0) {
                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.CreditNote, EndorsementController.CreditNote, true);
                    }
                } else {
                    $.UifDialog('alert', { 'message': Resources.Language.EndorsmentNotApply }, null);
                }
            }
        }
    }
    static LoadEndorsementCollective() {
        if ($('#inputDescriptionRisk').val().trim().length > 0) {
            $("#resultConsultationRisk").show();
            SearchRequest.GetRiskByPolicyIdByRiskDescription(glbPolicy.Endorsement.PolicyId, glbPolicy.Endorsement.Id, $('#inputDescriptionRisk').val().trim()).done(function (data) {
                if (data.success) {
                    $("#resultConsultationRisk").css({ border: "1px solid #e0e2e3", "margin-left": "-4px", "margin-right": "-4px" });
                    $("#resultConsultationRisk").html(
                        "<strong>  " + data.result.LicensePlate + "-"
                        + (data.result.Make.Description ? " Marca: " + data.result.Make.Description : "")
                        + (data.result.Model.Description ? " Modelo: " + data.result.Model.Description : "")
                        + " Año: " + data.result.Year + "</strong><br/>" +
                        (data.result.Price ? " Valor: " + FormatMoney(data.result.Price) : "$0") +
                        (data.result.PriceAccesories ? " Accesorios: " + FormatMoney(data.result.PriceAccesories) : "") +
                        " <br/> " + (data.result.Version.Description ? " Tipo: " + data.result.Version.Description : "")
                    );
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {

                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryPlate, 'autoclose': true });
            });
        }
        else {
            $("#resultConsultationRisk").text("");
            $("#resultConsultationRisk").hide();
        }
    }
    static GetTemporalById(id) {
        var temporalInfo;
        if (id > 0) {
            SearchRequest.GetTemporalByIdTemporalType(id, TemporalType.Endorsement, 0).done(function (data) {
                if (data.success) {
                    temporalInfo = data.result;
                    $('#selectBranch').UifSelect('setSelected', temporalInfo.Branch.Id);
                    $('#selectPrefix').UifSelect('setSelected', temporalInfo.Prefix.Id);
                    $('#inputPolicyNumber').val(temporalInfo.DocumentNumber);
                    Search.searchPolicyNumber();
                    Search.GetPrefixEndoEnabled($("#selectPrefix").UifSelect("getSelected"));
                    $('#itemRenewalPolicy').addClass("selected");
                }
                else {
                    $('#itemRenewalPolicy').removeClass("selected");
                    Search.ClearSummaryEndorsement();
                    Search.ClearDataSearch();
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorTempNoExist, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $('#itemRenewalPolicy').removeClass("selected");
                Search.ClearSummaryEndorsement();
                Search.ClearDataSearch();
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static EvaluateInsuredObjects(insuredObjects) {
        if (insuredObjects.length > 0) {
            return true;
        } else {
            return false;
        }
    }

    static GenerateErrorMessages(prefixId, description) {
        var message = "";
        switch (prefixId) {
            case 5:
                $.UifDialog('alert', { 'message': Resources.Language.ErrorValidatingAdjustmentEndorsement }, null);
                break;
            case 3:
            case PrefixType.SUSTRACCION:
            case PrefixType.ROTURA_DE_MAQUINARIA:
            case PrefixType.CORRIENTE_DEBIL:
                message = Resources.Language.UnavaibleDeclarativeInsuredObjects + description;
                $.UifDialog('alert', { 'message': message }, null);
                break;

            default:
        }
    }

    static InitialLoad() {
        var dfLoad = $.Deferred();
        $.when(SearchRequest.GetBranches(), SearchRequest.GetPrefixes()).done(function (branch, prefix) {
            if (branch != null && prefix != null) {
                if (branch[0].success && branch[0].result != null) {
                    $('#selectBranch').UifSelect({ sourceData: branch[0].result });
                }
                else {
                    dfdLoad.reject(false);
                }
                if (prefix[0].success && prefix[0].result != null) {
                    $('#selectPrefix').UifSelect({ sourceData: prefix[0].result });

                }
                else {
                    dfdLoad.reject(false);
                }
                dfLoad.resolve(true);
            }
            else {
                dfLoad.reject(false);
            }
        });
        return dfLoad.promise();
    }

    static async ValidateExchangeRate(currency) {
        if (currency != 0) {

            let issueDate = await $.ajax({
                type: "POST",
                url: rootPath + "Underwriting/Underwriting/GetModuleDateIssue",
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });

            let data = await UnderwritingRequest.GetExchangeRateByCurrencyId(currency);
            if (data.success) {
                if (data.result != null) {
                    if (issueDate.success) {
                        issueDate = issueDate.result;
                    }
                    if (FormatDate(data.result.RateDate) != FormatDate(issueDate)) {

                        $.UifNotify('show', {
                            'type': 'danger', 'message': Resources.Language.updateexchangerate, 'autoclose': true
                        });

                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            return true;
        }

        return true;
    }

    static searchPolicyNumber() {
        Search.ClearSummary();
        Search.SummaryClear();
        Search.HideSearchPlate();
        $('#formSearch').validate();
        if ($('#formSearch').valid()) {
            //  if (dataTemporal.Id != -1) {
            var branch = $("#selectBranch").UifSelect("getSelected");
            var prefix = $("#selectPrefix").UifSelect("getSelected");

            if (branch == "" && prefix == "") {
                branch = 0
                prefix = 0
            }
            SearchRequest.GetCompanyEndorsementsByFilterPolicy(branch, prefix, $('#inputPolicyNumber').val(), true).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        SearchRequest.GetCurrentPolicyByEndorsementId(data.result.Id, true).done(function (data) {
                            if (data.success) {
                                if (data.result != null) {

                                    Search.ValidateTemporal(data.result.DocumentNumber, data.result.Branch.Id, data.result.Prefix.Id).then(function (dataTemporal) {
                                        Search.ValidateExchangeRate(data.result.ExchangeRate.Currency.Id).then(isActualCurrency => {
                                            data.result.Id = dataTemporal.Id;
                                            data.result.EndorsementTemporal = dataTemporal.EndorsementType;
                                            data.result.TemporalType = TemporalType.Endorsement;
                                            Search.LoadSummaryEndorsement(data.result);
                                            Search.GetListEndorsement(glbPolicy.Branch.Id, glbPolicy.Prefix.Id, glbPolicy.DocumentNumber, false);
                                            if (glbPolicy.Change)
                                                Search.searchPolicyNumber();
                                            else {
                                                Search.LoadSummaryBase();
                                                if (dataTemporal.Id == -1) {
                                                    $("#panelButtonsRight").hide();
                                                }
                                                else
                                                    $("#panelButtonsRight").show();
                                            }

                                            if (!isActualCurrency) {
                                                $("#panelButtonsRight").hide();
                                            }
                                        });
                                    });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                }
                            }
                        });
                        $("#btnEndorsementList").show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Datos Faltantes en el Formulario", 'autoclose': true });
        }
    }

    static GetEndorsementsByPrefixIdBranchIdPolicyNumber(selectedId) {
        $('#formSearch').validate();
        if ($('#formSearch').valid()) {
            var isCurrent = true;
            SearchRequest.GetCompanyEndorsementsByFilterPolicy($("#selectBranch").UifSelect("getSelected"), $("#selectPrefix").UifSelect("getSelected"), $('#inputPolicyNumber').val(), isCurrent).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        Search.GetCurrentStatusPolicyByEndorsementIdIsCurrent(data.result.selectedId, isCurrent);
                        $("#btnEndorsementList").show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Datos faltantes en el formulario", 'autoclose': true });
            $('#selectEndorsement').UifSelect();
        }

    }

    static GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent) {
        SearchRequest.GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (isCurrent) {
                        Search.LoadSummaryEndorsement(data.result);
                    }
                    else {
                        Search.LoadSummaryEndorsement(data.result);
                        Search.ShowSearchPlate();
                        if ($("#selectPrefix").UifSelect("getSelected") != "") {
                            Search.GetPrefixEndoEnabled($("#selectPrefix").UifSelect("getSelected"));

                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
        });
    }

    static GetAllEndorsement(endorsementId, isCurrent) {
        var temporalObjListEndorsement = null;
        var dfd = $.Deferred();
        SearchRequest.GetEndorsementPolicyInformation(endorsementId, false).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    dfd.resolve(data.result);
                }
                else {
                    dfd.reject();
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotFoundEndorsementHistoric, 'autoclose': true });
                }
            }
            else {
                dfd.reject();
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryEndorsermentHistoric, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfd.reject();

            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryEndorsermentHistoric, 'autoclose': true });
        });

        return dfd.promise();
    }

    static showButtons(endorsementType, isCollective) {
        $('#itemInclusionExlusionRisk').hide();
        $("#btnSendExcel").hide();
        $("#panelButtonsRight").hide();
        $("#panelButtonsConsultation").hide();

        if (endorsementType == 1) {
            $("#panelButtonsConsultation").show();
            $("#btnAdvSearch").hide();

            $('#btnCancelSearch').hide();
            $("#titlePrincipal").text(Resources.Language.QueryPolicy);
            if (isCollective) {
                $("#btnSendExcel").show();
            }
        }
        else {
            $("#panelButtonsRight").show();

            $('#btnCancelSearch').show();
            $("#titlePrincipal").text(Resources.Language.Endorsement);
        }
    }

    static LoadSummaryEndorsement(policyData) {
        glbPolicy.IsUnderIdenticalConditions = false;
        glbPolicy = policyData;
        glbPolicy.EndorsementOriginId = glbPolicy.Endorsement.Id;
        glbPolicy.Change = false;
        productIsCollective = policyData.PolicyOrigin == PolicyOrigin.Collective ? true : false;
        Search.showButtons(AccessEndorsementType.isWrite, productIsCollective);
        $('#labelEmission').show();
        $('#labelIssueDate').text(' ' + FormatFullDate(policyData.IssueDate));
        $("#labelTextTermOf").html(Resources.Language.LabelTermOfPolicy);
        $('#labelCurrentFromTo').text(FormatDate(policyData.CurrentFrom) + ' - ' + FormatDate(policyData.Endorsement.CurrentTo));
        $('#labelInsured').text(policyData.Holder.Name + " - " + policyData.Holder.IdentificationDocument.Number);
        $('#labelAgent').text(policyData.Agencies[0].Agent.FullName);
        $('#labelBusinessType').text(policyData.BusinessTypeDescription);
        $('#labelCurrency').text(policyData.ExchangeRate.Currency.Description);
        $('#labelLastEndorsement').text(policyData.Endorsement.Number + ' - ' + policyData.Endorsement.EndorsementTypeDescription);
        $('#labelCurrentStatus').text(policyData.Endorsement.Description);
        $("#selectBranch").UifSelect("setSelected", policyData.Branch.Id);
        $("#selectPrefix").UifSelect("setSelected", policyData.Prefix.Id);
        policyData.Summary.AmountInsured = FormatMoney(policyData.Summary.AmountInsured);
        policyData.Summary.Premium = FormatMoney(policyData.Summary.Premium);
        policyData.Summary.Expenses = FormatMoney(policyData.Summary.Expenses);
        policyData.Summary.Taxes = FormatMoney(policyData.Summary.Taxes);
        policyData.Summary.FullPremium = FormatMoney(policyData.Summary.FullPremium);
        glbPolicyEndorsement = glbPolicy;
        Search.GetPrefixEndoEnabled(policyData.Prefix.Id);
        Search.SummaryClear();
        Search.SummaryFill(policyData.Summary);
        Search.LoadTitle();
        //if (glbPolicy.Prefix.Id = PrefixCollective.Surety) {
        //    $('#itemExtensionPolicy').show();
        //    $('#itemChangeAgent').show();
        //}
    }

    static LoadTitle() {
        if (glbPolicy.Endorsement.PolicyId != 0) {
            titlePrincipal = $("#selectBranch").UifSelect("getSelectedText");
            titlePrincipal = '¡' + titlePrincipal + '!, ¡' + $("#selectPrefix").UifSelect("getSelectedText");
            titlePrincipal = titlePrincipal + ', ' + $('#inputPolicyNumber').val() + '! ';
            titlePrincipal = titlePrincipal + '  ¡' + Resources.Language.Emission + ':! ' + FormatFullDate(glbPolicyEndorsement.IssueDate);
            titlePrincipal = titlePrincipal + '  ¡' + Resources.Language.From + ':! ' + FormatDate(glbPolicyEndorsement.CurrentFrom);
            titlePrincipal = titlePrincipal + '  ¡' + Resources.Language.LabelToPrinter + ':! ' + FormatDate(glbPolicy.Endorsement.CurrentTo);
            titlePrincipal = titlePrincipal + '  ¡' + Resources.Language.LabelDays + ':! ' + CalculateDays(FormatDate(glbPolicyEndorsement.CurrentFrom), FormatDate(glbPolicy.Endorsement.CurrentTo));
            titlePrincipal = titlePrincipal + '  ¡' + Resources.Language.ExchangeRate + ':! ' + glbPolicy.ExchangeRate.SellAmount;
            var titlePage = titlePrincipal;
            titlePage = titlePage.replace(/¡/g, '<strong>');
            titlePage = titlePage.replace(/!/g, '</strong>');
            glbPolicyEndorsement.Title = titlePage;
        }
    }

    static htmlTitle(tittle) {
        var titlePage = tittle.replace(/¡/g, '<strong>');
        titlePage = titlePage.replace(/!/g, '</strong>');
        return titlePage;
    }

    static ClearSummaryEndorsement() {
        $('#labelLastEndorsement').text('');
        $('#labelIssueDate').text('');
        $('#labelCurrentFromTo').text('');
        $('#labelInsured').text('');
        $('#labelAgent').text('');
        $('#labelBusinessType').text('');
        $('#labelCurrentStatus').text('');
        $('#labelCurrency').text('');
        Search.SummaryClear();
        glbPolicy = { Id: 0 };
    }

    static GetEndorsementController(prefixId, productId, endorsementType, controller, isRedirect) {
        var dfLoad = $.Deferred();
        SearchRequest.GetEndorsementController(prefixId, productId, endorsementType).done(function (data) {
            if (data.success) {
                if (glbPolicy.Product.CoveredRisk == null) {
                    glbPolicy.Product.CoveredRisk = {};
                }
                if (data.result != null) {
                    glbPolicy.EndorsementType = data.result;
                    switch (data.result) {
                        case SubCoveredRiskType.Vehicle:

                            glbPolicy.EndorsementController = "Vehicle" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Vehicle;
                            break;
                        case SubCoveredRiskType.ThirdPartyLiability:
                            glbPolicy.EndorsementController = "ThirdPartyLiability" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.ThirdPartyLiability;
                            break;
                        case SubCoveredRiskType.Property:
                            glbPolicy.EndorsementController = "Property" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Property;
                            break;
                        case SubCoveredRiskType.Liability:
                            glbPolicy.EndorsementController = "Liability" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Liability;
                            break;
                        case SubCoveredRiskType.Surety:
                            glbPolicy.EndorsementController = "Surety" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Surety;
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            glbPolicy.EndorsementController = "JudicialSurety" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.JudicialSurety;
                            break;
                        case SubCoveredRiskType.Transport:
                            glbPolicy.EndorsementController = "Transport" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Transport;
                            break;
                        case SubCoveredRiskType.Lease:
                            glbPolicy.EndorsementController = "Surety" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Lease;
                            break;
                        case SubCoveredRiskType.Aircraft:
                            glbPolicy.EndorsementController = "Aircraft" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Aircraft;
                            break;

                        case SubCoveredRiskType.Marine:
                            glbPolicy.EndorsementController = "Marine" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.Marine;
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            glbPolicy.EndorsementController = "JudicialSurety" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.JudicialSurety;
                            break;
                        case SubCoveredRiskType.FidelityNewVersion:
                            glbPolicy.EndorsementController = "Fidelity" + controller;
                            glbPolicy.Product.CoveredRisk.SubCoveredRiskType = SubCoveredRiskType.FidelityNewVersion;
                            break;

                    }
                    dfLoad.resolve(true);
                    if (isRedirect) {
                        if (glbPolicy.Product.CoveredRisk.SubCoveredRiskType == SubCoveredRiskType.Surety && (glbPolicy.Endorsement.EndorsementType != EndorsementType.Cancellation && glbPolicy.EndorsementType != EndorsementType.LastEndorsementCancellation)) {

                            var band = Search.checkCoveragePostContractual();
                            $.when(band).done(function (result) {
                                if (result || (endorsementType == 'Reversion'))
                                    Search.LoadSiteEndorsement(endorsementType);
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MensajeCoveragePost, 'autoclose': true });
                                }
                            });
                        }
                        else
                            Search.LoadSiteEndorsement(endorsementType);
                    }

                }
            }
            else {
                dfLoad.resolve(false);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            dfLoad.resolve(false);
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryEndorserDriver, 'autoclose': true });
        });
        return dfLoad.promise();
    }

    static GetTemporalPolicyByPolicyId(policyId, endorsementType) {
        var temporalPolicy = null;
        SearchRequest.GetTemporalPolicyByPolicyId(policyId, endorsementType, endorsementController).done(function (data) {
            temporalPolicy = data.result;
            if (!data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });

        return temporalPolicy;
    }

    static RedirectCollectiveEndorsement(controller) {
        var policyModel = {
            PolicyNumber: $('#inputPolicyNumber').val(),
            PolicyId: glbPolicy.Endorsement.PolicyId,
            EndorsementId: glbPolicy.Endorsement.Id,
            BranchId: $("#selectBranch").UifSelect("getSelected"),
            PrefixId: $("#selectPrefix").UifSelect("getSelected"),
            Prefix: $("#selectPrefix").UifSelect("getSelectedText"),
            Branch: $("#selectBranch").UifSelect("getSelectedText"),
            Product: glbPolicy.Product.Description,
            EndorsementType: glbPolicy.Endorsement.EndorsementType,
            IssueDate: FormatFullDate(glbPolicy.IssueDate),
            CurrentFrom: FormatDate(glbPolicy.CurrentFrom),
            CurrentTo: FormatDate(glbPolicy.Endorsement.CurrentTo),
            RiskCount: glbPolicy.Summary.RiskCount,
            AmountInsured: NotFormatMoney(glbPolicy.Summary.AmountInsured),
            Premium: NotFormatMoney(glbPolicy.Summary.Premium),
            Expenses: NotFormatMoney(glbPolicy.Summary.Expenses),
            Taxes: NotFormatMoney(glbPolicy.Summary.Taxes),
            FullPremium: NotFormatMoney(glbPolicy.Summary.FullPremium),
            Title: titlePrincipal,
            LoadTypeId: Policy.SubEndorsementType,
            EndorsementController: endorsementController
        };

        $.redirect(rootPath + 'Collective/' + controller, glbPolicyModel);
    }

    static RedirectPrinting(controller) {
        var printerModelsView = {
            BranchId: $("#selectBranch").UifSelect("getSelected"),
            PrefixId: $("#selectPrefix").UifSelect("getSelected"),
            PolicyNumber: $('#inputPolicyNumber').val(),
            EndorsementId: glbPolicy.Endorsement.Id
        };

        $.redirect(rootPath + 'Printing/' + controller, printerModelsView);
    }

    static HidePanelsSearch() {
        $("#modalAgentsSearch").UifModal('hide');
        $("#modalPaymentPlanSearch").UifModal('hide');
        $("#modalTextsSearch").UifModal('hide');
        $("#modalClausesSearch").UifModal('hide');
        $("#modalOperatingQuota").UifModal('hide');
        $("#modalOptionRenewal").UifModal('hide');
    }

    static ShowPanelsSearch(Menu) {
        switch (Menu) {
            case MenuType.Search:
                break;
            case MenuType.Texts:
                $("#modalTextsSearch").UifModal('showLocal', Resources.Language.LabelTexts);
                break;
            case MenuType.Clauses:
                $("#modalClausesSearch").UifModal('showLocal', Resources.Language.LabelClauses);
                break;
            case MenuType.PaymentPlan:
                $("#modalPaymentPlanSearch").UifModal('showLocal', Resources.Language.LabelQuotas);
                break;
            case MenuType.Agents:
                $("#modalAgentsSearch").UifModal('showLocal', Resources.Language.DetailCommissions);
                break;
            case MenuType.Renewal:
                $("#modalOptionRenewal").UifModal('showLocal', Resources.Language.RenewalPolicy);
                break;
            default:
                break;
        }
    }

    static TitlePanelSearch() {
        if (glbPolicy.Endorsement.Text == null) {
            $("#selectedText").text(Resources.Language.LabelWithoutData);
        }
        else if (glbPolicy.Endorsement.Text.TextBody != null && glbPolicy.Endorsement.Text.Observations != null) {
            $("#selectedText").text(Resources.Language.LabelWithTextAndObservations);
        }
        else if (glbPolicy.Endorsement.Text.TextBody != null) {
            $("#selectedText").text(Resources.Language.LabelWithTextWithoutObservations);
        }
        else if (glbPolicy.Endorsement.Text.Observations != null) {
            $("#selectedText").text(Resources.Language.LabelWithoutTextWithObservations);
        }

        if (glbPolicy.Clauses != undefined) {
            $("#selectedClauses").text(glbPolicy.Clauses.length);
        }
        else {
            $("#selectedClauses").text(Resources.Language.LabelWithoutData);
        }

        if (glbPolicy.Agencies != null) {
            $.each(glbPolicy.Agencies, function (key, value) {
                if (this.IsPrincipal == true || glbPolicy.Agencies.length == 1) {
                    if (this.Participation == 0) {
                        this.Participation = 100;
                    }
                    $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" + glbPolicy.Agencies.length + ") ");
                }
            });
        }

        $("#selectPaymentPlan option:eq(0)").text(" ");
        $("#selectedPaymentPlan").text($("#selectPaymentPlan option:selected").text());
    }

    static HideTitlePanelSearch() {
        $("#selectedText").text("");
        $("#selectedClauses").text("");
        $("#selectedAgents").text("");
        $("#selectedPaymentPlan").text("");
    }

    static ShowSearchPlate() {
        if ($("#selectPrefix").UifSelect("getSelected") == PrefixCollective.Vehicle && glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
            $("#btnConsultationRisk").show();
            $("#resultConsultationRisk").show();
        }
    }

    static HideSearchPlate() {
        $("#resultConsultationRisk").hide();
        $("#inputDescriptionRisk").val("");
        $("#resultConsultationRisk").text("");
        $("#btnConsultationRisk").hide();
        $("#resultConsultationRisk").css("border", "none");
    }

    static SendToExcel() {
        var policyModel = {
            Branch: { Id: $("#selectBranch").UifSelect("getSelected") },
            Prefix: { Id: $("#selectPrefix").UifSelect("getSelected") },
            DocumentNumber: $('#inputPolicyNumber').val()
        };
        SearchRequest.SendToExcel(policyModel, $('#inputDescriptionRisk').val()).done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
        });
    }

    static RedirectCollective(data) {
        var collectiveModel = {
            CreateRiskFromPolicy: true,
            IdLoad: data.Id,
            IdBranch: data.Branch.Id,
            IdPrefixCommercial: data.Prefix.Id,
            PolicyId: data.Endorsement.PolicyId,
            EndorsementId: data.Endorsement.Id,
            TempId: data.Id
        };

        $.redirect(rootPath + 'Collective/Collective/Collective', collectiveModel);
    }

    static SummaryFill(summarySearch) {
        $("#SearchRiskCount").text(summarySearch.RiskCount);
        $("#SearchAmountInsured").text(summarySearch.AmountInsured);
        $("#SearchPremium").text(summarySearch.Premium);
        $("#SearchExpenses").text(summarySearch.Expenses);
        $("#SearchTaxes").text(summarySearch.Taxes);
        $("#SearchFullPremium").text(summarySearch.FullPremium);
    }

    static SummaryClear() {
        $("#SearchRiskCount").text('');
        $("#SearchAmountInsured").text('');
        $("#SearchPremium").text('');
        $("#SearchExpenses").text('');
        $("#SearchTaxes").text('');
        $("#SearchFullPremium").text('');
    }

    static ClearDataSearch() {
        glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, Endorsement: {}, Change: false };
        $('#selectBranch').UifSelect("setSelected", null);
        $('#selectPrefix').UifSelect("setSelected", null);
        $('#inputPolicyNumber').val("");
        $("#btnEndorsementList").hide();
    }

    static CreateTemporal(renewalModel) {
        SearchRequest.CreateTemporal(renewalModel).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.Endorsement.IsUnderIdenticalConditions) {
                        glbPolicy.Id = data.result.Id;
                        glbPolicy.TemporalType = TemporalType.Endorsement;
                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, 'Renewal', 'Renewal', true);
                    } else {
                        glbPolicy.Id = data.result.Id;
                        glbPolicy.TemporalType = TemporalType.Endorsement;
                        Search.LoadRiskView();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static LoadRiskView() {
        if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCollective, 'autoclose': true });
            return;
        } else {
            router.run("prtTemporal");
        }

    }

    static GetPrefixEndoEnabled(prefixId) {
        SearchRequest.GetPrefixEndoEnabled(prefixId).done(function (data) {
            if (data.success) {
                prefixEndoEnabledList = data.result;
                $.each(data.result, function (key, value) {
                    Search.ShowItemsEndorsement(this.EndorsementCode);
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryInformationBranch, 'autoclose': true });
        });
    }

    static HideItemsEndorsement() {
        $('#itemReversionEndorsement').hide();
        $('#itemCancelPolicy').hide();
        $('#itemExtensionPolicy').hide();
        $('#itemRiskModification').hide();
        $('#itemRenewalPolicy').hide();
        $('#itemChangeTerm').hide();
        $('#itemCreditNote').hide();
        $('#itemDeclaration').hide();
        $('#itemAdjustment').hide();
        $('#itemPrintPolicy').hide();
        $('#itemChangePolicyHolder').hide();
        $('#itemChangeConsolidation').hide();
    }

    static ShowItemsEndorsement(endorsementType) {
        switch (endorsementType) {
            case EndorsementType.Modification:
                $('#itemRiskModification').show();
                break;
            case EndorsementType.Cancellation:
                $('#itemCancelPolicy').show();
                break;
            case EndorsementType.EffectiveExtension:
                $('#itemExtensionPolicy').show();
                break;
            case EndorsementType.Renewal:
                $('#itemRenewalPolicy').show();
                break;
            case EndorsementType.LastEndorsementCancellation:
                $('#itemReversionEndorsement').show();
                break;
            case EndorsementType.ChangeTerm:
                $('#itemChangeTerm').show();
                break;
            case EndorsementType.ChangeAgent:
                $('#itemChangeAgent').show();
                break;
            case EndorsementType.Declaration:
                $('#itemDeclaration').show();
                break;
            case EndorsementType.Adjustment:
                $('#itemAdjustment').show();
                break;
            case EndorsementType.CreditNote:
                $('#itemCreditNote').show();
                break;
            case EndorsementType.PolicyHonderChange:
                $('#itemChangePolicyHolder').show();
                break;
            case EndorsementType.ChangeConsolidation:
                $('#itemChangeConsolidation').show();
                break;
            default:
                break;
        }
    }

    static ClearSummary() {
        $('#labelIssueDate').text('');
        $('#labelCurrentFromTo').text('');
        $('#labelInsured').text('');
        $('#labelAgent').text('');
        $('#labelBusinessType').text('');
        $('#labelCurrency').text('');
        $('#labelLastEndorsement').text('');
        $('#labelCurrentStatus').text('');
        glbPolicy.Message = '';
    }

    static DeleteTemporal() {
        if (glbPolicy.Endorsement.PolicyId > 0) {
            SearchRequest.GetTemporalEndorsementByPolicyId(glbPolicy.Endorsement.PolicyId).done(data => {
                if (data.success) {
                    RequestSummaryAuthorization.GetPoliciesTemporal(data.result.TemporalId).done(data1 => {
                        if (data1.success) {
                            if (data1.result != null) {
                                $.UifDialog('confirm', { 'message': data1.result.Message }, (responsePolicies) => {
                                    if (responsePolicies) {
                                        RequestSummaryAuthorization.DeleteNotificationByTemporalId(data.result.TemporalId, FunctionType.Individual).done((data3) => {
                                            if (data3.success) {
                                                SearchRequest.DeleteTemporal($('#inputPolicyNumber').val(), $("#selectPrefix").UifSelect("getSelected"), $("#selectBranch").UifSelect("getSelected"))
                                                    .done(data4 => {
                                                        if (data4.success) {
                                                            glbPolicy.Id = 0;
                                                            $.UifNotify('show', { 'type': 'info', 'message': data4.result, 'autoclose': true });
                                                        }
                                                        else {
                                                            $.UifNotify('show', { 'type': 'danger', 'message': data4.result, 'autoclose': true });
                                                        }
                                                    }).fail(function (jqXHR, textStatus, errorThrown) {
                                                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDeleteTemporal, 'autoclose': true });
                                                    });
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': data3.result, 'autoclose': true });
                                            }
                                        }).fail(function (jqXHR, textStatus, errorThrown) {
                                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                                        });
                                    }
                                });
                            } else {
                                $.UifDialog('confirm', { 'message': Resources.Language.RemoveTemporalQuestion + Resources.Language.WishContinue }, (response) => {
                                    if (response) {
                                        SearchRequest.DeleteTemporal($('#inputPolicyNumber').val(), $("#selectPrefix").UifSelect("getSelected"), $("#selectBranch").UifSelect("getSelected"))
                                            .done(data4 => {
                                                if (data4.success) {
                                                    glbPolicy.Id = 0;
                                                    $.UifNotify('show', { 'type': 'info', 'message': data4.result, 'autoclose': true });
                                                }
                                                else {
                                                    $.UifNotify('show', { 'type': 'danger', 'message': data4.result, 'autoclose': true });
                                                }
                                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDeleteTemporal, 'autoclose': true });
                                            });
                                    }
                                });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data1.result.Message, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                    });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDeleteTemporal, 'autoclose': true });
                });
        }
    }

    static HideItemsEndorsementComun() {
        $('#itemCreditNote').hide();
        $('#itemDeclaration').hide();
        $('#itemAdjustment').hide();
        $('#itemChangeAgent').hide();
        $('#itemPrintPolicy').hide();
    }

    //static checkhierarchy() {
    //    SearchRequest.ValidationAccessAndHierarchy().done(function (data) {
    //        if (data.success) {
    //        }
    //        else {
    //            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
    //        }
    //    }).fail(function (jqXHR, textStatus, errorThrown) {
    //        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
    //    });
    //}

    static checkCoveragePostContractual() {
        var dfLoad = $.Deferred();

        SearchRequest.ValidateCoveragePostContractual(glbPolicy.Endorsement.PolicyId).done(function (data) {
            if (data.success) {
                if ((data.result != null && data.result.length > 0)) {

                    dfLoad.resolve(false);
                }
                dfLoad.resolve(true);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPostContract, 'autoclose': true });
            dfLoad.resolve(false);
        });

        return dfLoad.promise();

    }

    static GetListEndorsement(branchId, prefixId, policyNumber, table) {
        SearchRequest.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(branchId, prefixId, policyNumber).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    if (table) {
                        data.result.forEach(function (item) {
                            item.TotalPremium = FormatMoney(item.TotalPremium);
                            item.AssuredSum = FormatMoney(item.AssuredSum);
                            item.EmissionDate = FormatFullDate(item.EmissionDate);
                            item.CurrentFrom = FormatDate(item.CurrentFrom);
                            item.CurrentTo = FormatDate(item.CurrentTo);
                        });
                        $("#tableListEndorsement").UifDataTable({ sourceData: data.result });
                        $("#modalListEndorsement").UifModal('showLocal', Resources.Language.EndorsementsList);
                    }
                    else {
                        Object.defineProperty(glbPolicy, 'EndorsementTexts', {
                            value: [],
                            writable: true,
                            enumerable: true,
                            configurable: true
                        });
                        data.result.forEach(function (item) {
                            glbPolicy.EndorsementTexts.push({
                                Text: item.ConditionText != null && item.ConditionText != "" ? item.ConditionText : Resources.Language.NoText, Observations: item.Annotations != null && item.Annotations != "" ? item.Annotations : Resources.Language.NoObservations,
                                Date: item.EmissionDate, Number: item.EndorsementNumber,
                                Type: item.DescriptionEndorsementType,
                                ModificationTypeId: item.ModificationTypeId
                            });
                        });
                        glbEndorsementTexts = glbPolicy.EndorsementTexts;
                    }
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryEndorsermentHistoric, 'autoclose': true });
        });
    }

    static LoadSiteEndorsement(endorsement) {
        var uriSPA = "prt" + endorsement;
        if (endorsement == EndorsementMovements.Search) {
            router.run(uriSPA);
        }

        else if (glbPolicy != null && endorsement != "") {
            router.run(uriSPA);
        }
        else {
            window.location = rootPath + "Home/Index";
        }
    }

    static GetControllerRisk() {
        if (glbPolicy != null) {
            if (glbPolicy.Product == undefined)
                return;
            switch (glbPolicy.Product.CoveredRisk.SubCoveredRiskType) {
                case SubCoveredRiskType.Vehicle:
                    return "RiskVehicle";
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    return "RiskThirdPartyLiability";
                    break;
                case SubCoveredRiskType.Property:
                    return "RiskProperty";
                    break;
                case SubCoveredRiskType.Liability:
                    return "RiskLiability";
                    break;
                case SubCoveredRiskType.Surety:
                case SubCoveredRiskType.Lease:
                    return "RiskSurety";
                    break;
                case SubCoveredRiskType.JudicialSurety:
                    return "RiskJudicialSurety";
                    break;
                case SubCoveredRiskType.Transport:
                    return "RiskTransport";
                    break;
            }
        }
        else {
            window.location = rootPath + "Home/Index";
        }
    }

    static LoadPolicy() {
        $('#inputPolicyNumber').val(glbPolicy.DocumentNumber);
        var dataSource = [{ Id: glbPolicy.Endorsement.Id, Description: Resources.Language.ActualState }];
        $("#selectBranch").UifSelect("setSelected", glbPolicy.Branch.Id);
        $("#selectPrefix").UifSelect("setSelected", glbPolicy.Prefix.Id);
        Search.HideItemsEndorsementComun();
        Search.GetPrefixEndoEnabled($("#selectPrefix").UifSelect("getSelected"));
        Search.searchPolicyNumber();



    }

    static LoadSummaryBase() {
        $('#labelEmission').show();
        $('#labelIssueDate').text(' ' + FormatFullDate(glbPolicy.IssueDate));
        if (glbPolicy.Endorsement != null && glbPolicy.Endorsement.Id > 0) {
            glbPolicy.EndorsementOriginId = glbPolicy.Endorsement.Id;
            $("#labelTextTermOf").html(`${Resources.Language.LabelTermOfPolicy}:`);
        }
        $('#labelCurrentFromTo').text(FormatDate(glbPolicyEndorsement.CurrentFrom) + ' - ' + FormatDate(glbPolicyEndorsement.Endorsement.CurrentTo));
        $('#labelInsured').text(glbPolicyEndorsement.Holder.Name + ' - ' + glbPolicyEndorsement.Holder.IdentificationDocument.Number);
        $('#labelAgent').text(glbPolicyEndorsement.Agencies[0].Agent.FullName);
        $('#labelBusinessType').text(glbPolicyEndorsement.BusinessTypeDescription);
        $('#labelCurrency').text(glbPolicyEndorsement.ExchangeRate.Currency.Description);
        $('#labelLastEndorsement').text(glbPolicyEndorsement.Endorsement.Number + ' - ' + glbPolicyEndorsement.Endorsement.EndorsementTypeDescription);
        $('#labelCurrentStatus').text(glbPolicyEndorsement.Endorsement.Description);
        //todo rivisar
        glbPolicyEndorsement.Summary.AmountInsured = FormatMoney(glbPolicyEndorsement.Summary.AmountInsured);
        glbPolicyEndorsement.Summary.Premium = FormatMoney(glbPolicyEndorsement.Summary.Premium);
        glbPolicyEndorsement.Summary.Expenses = FormatMoney(glbPolicyEndorsement.Summary.Expenses);
        glbPolicyEndorsement.Summary.Taxes = FormatMoney(glbPolicyEndorsement.Summary.Taxes);
        glbPolicyEndorsement.Summary.FullPremium = FormatMoney(glbPolicyEndorsement.Summary.FullPremium);
        Search.SummaryClear();
        Search.SummaryFill(glbPolicyEndorsement.Summary);
        Search.LoadTitle();
    }


    static ValidateBase(validateCollective) {
        var isValid = true;
        if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective && validateCollective) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCollective, 'autoclose': true });
            isValid = false;
            return isValid;
        }
        else if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Cancellation) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EndorsementCancelation, 'autoclose': true });
            isValid = false;
            return isValid;
        }
        else {
            if (glbPolicy == null || glbPolicy.Endorsement == null || glbPolicy.Endorsement.PolicyId < 0) {
                isValid = false;
            }
        }
        return isValid;
    }
    static ExtensionOtherPrefix() {
        var otherPrefix = false;
        if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            var subcovered = Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Declaration, EndorsementController.Declaration, false);
            $.when(subcovered).done(function (resul) {
                switch (glbPolicy.Product.CoveredRisk.SubCoveredRiskType) {
                    case SubCoveredRiskType.Transport:
                        otherPrefix = true;
                        SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                            if (data.success) {
                                if (!data.result.CanMakeEndorsement) {
                                    Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Extension, EndorsementController.Extension, true);
                                }
                                else {
                                    $.UifDialog('alert', { 'message': data.result.Message }, null);
                                }
                            }
                            else {
                                $.UifDialog('alert', { 'message': Resources.Language.ErrorValidatingextensionEndorsement }, null);
                            }
                        });

                        break;
                    case SubCoveredRiskType.Property:
                        switch (glbPolicy.Prefix.Id) {
                            case PrefixType.INCENDIO:
                            case PrefixType.SUSTRACCION:
                            case PrefixType.ROTURA_DE_MAQUINARIA:
                            case PrefixType.CORRIENTE_DEBIL:
                                otherPrefix = true;
                                SearchRequest.ValidateDeclarativeInsuredObjects(glbPolicy.Endorsement.PolicyId).done(function (result) {
                                    if (result.success) {
                                        if (result.result) {
                                            SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                                                if (data.success) {
                                                    if (!data.result.CanMakeEndorsement) {
                                                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Extension, EndorsementController.Extension, true);
                                                    }
                                                    else {
                                                        $.UifDialog('alert', { 'message': data.result.Message }, null);
                                                    }
                                                } else {
                                                    $.UifDialog('alert', { 'message': Resources.Language.ErrorValidatingextensionEndorsement }, null);
                                                }
                                            });
                                        } else {
                                            Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Extension, EndorsementController.Extension, true);
                                        }
                                    } else {
                                        Search.GenerateErrorMessages(policy.Prefix.Id, policy.Prefix.Description);
                                    }
                                });

                                break;
                        }
                        break;
                    default:
                        otherPrefix = false;
                        Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, EndorsementMovements.Extension, EndorsementController.Extension, true);
                        break;
                }
            });
        }
        else {
            $.uifdialog('alert', { 'message': resources.language.ErrorNoPolicySelected }, null);
        }
        return otherPrefix;
    }

    static CreatePolicyBase(endorsementType) {
        var PolicyBaseViewModel = {
            Id: glbPolicy.Id,
            PolicyId: glbPolicy.Endorsement.PolicyId,
            EndorsementId: glbPolicy.Endorsement.Id,
            EndorsementTypeOriginal: glbPolicy.Endorsement.EndorsementType,
            EndorsementType: endorsementType,
            EndorsementTemporal: glbPolicy.EndorsementTemporal,
            IsUnderIdenticalConditions: glbPolicy.IsUnderIdenticalConditions
        };
        return PolicyBaseViewModel;
    }

    static ValidateTemporal(policyNumber, branchId, prefixId) {
        var dfLoad = $.Deferred();
        if (branchId == "" && prefixId == "") {
            branchId = 0
            prefixId = 0
        }
        SearchRequest.GetValidateOriginPolicy(policyNumber, prefixId, branchId).done(function (data) {
            glbPolicy.Id = 0;
            if (data.success) {
                if (data.result != null) {
                    dfLoad.resolve(data.result);
                }
                dfLoad.resolve({ Id: 0 });
            }
            else {
                glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, Endorsement: { EndorsementType: EndorsementType.Modification }, Change: false };
                dfLoad.resolve({ Id: -1 });
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ValidateOriginPolicy, 'autoclose': true });
            }
        });
        return dfLoad.promise();
    }

    static RenewalOtherPrefix() {
        var otherPrefix = true;
        switch (glbPolicy.Prefix.Id) {
            case PrefixType.TRANSPORTE:
                if (glbPolicy.PolicyType.IsFloating) {
                    if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Cancellation) {
                        SearchRequest.makeEndorsement(glbPolicy.Endorsement.PolicyId).done(function (data) {
                            if (data.success) {
                                if (!data.result.CanMakeEndorsement && data.result.AllowEndorsement == 0) {
                                    if (glbPolicy.Id > 0) {
                                        if (!glbPolicy.IsUnderIdenticalConditions) {
                                            router.run("prtTemporal");
                                        }
                                        else {
                                            Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, 'Renewal', 'Renewal', true);
                                        }
                                    }
                                    else {
                                        Search.ShowPanelsSearch(MenuType.Renewal);
                                    }
                                } else {
                                    $.UifDialog('alert', { 'message': data.result.Message }, null);
                                }
                            }
                        });
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoCanRenewCancellation, 'autoclose': true });
                    }

                }
                break;
            default:
                otherPrefix = false;
        }
    }
    static RedirectController(endorsementType, routerEndorsement, controller, validatCollective) {
        if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective && validatCollective) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCollective, 'autoclose': true });
            return;
        }
        else if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Cancellation) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EndorsementCancelation, 'autoclose': true });
            return;
        }
        else if (glbPolicy != null && glbPolicy.Endorsement != null && glbPolicy.Endorsement.PolicyId > 0) {
            if (glbPolicy.Id > 0) {
                var objectModel = Search.CreatePolicyBase(endorsementType);
                if (objectModel.EndorsementTemporal == 23)
                    objectModel.EndorsementTemporal = EndorsementType.Cancellation;
                if (objectModel.EndorsementType == EndorsementType.Modification)
                    objectModel.ModificationEndorsementType = routerEndorsement;
                SearchRequest.ValidateEndorsement(objectModel).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            glbPolicy.HasEvent = data.result.HasEvent;
                            glbPolicy.IssueDate = FormatFullDate(data.result.IssueDate);
                            glbPolicy.Message = data.result.Message;
                            if (data.result.Message != "" && !glbPolicy.HasEvent) {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result.Message, 'autoclose': true });
                            }
                            else {
                                Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, routerEndorsement, controller, true);
                            }
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                Search.GetEndorsementController($("#selectPrefix").UifSelect("getSelected"), glbPolicy.Product.Id, routerEndorsement, controller, true);
            }
        }
    }
    static SetNewEndorsement(dataResult, endorsementOriginId) {
        if (dataResult != null) {
            glbPolicy.Endorsement.Id = parseFloat(dataResult.split(':')[3]);
            glbPolicy.Change = true;
            glbPolicy.Endorsement.Number = parseFloat(dataResult.split(':')[2]);
            glbPolicy.Id = 0;
            glbPolicy.EndorsementOriginId = endorsementOriginId;
        }
    }
    static DeleteEndorsementControl() {
        if (glbPolicy.Endorsement != null && glbPolicy.EndorsementOriginId != null) {
            SearchRequest.DeleteEndorsementControl(glbPolicy.EndorsementOriginId);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        }

    }
    static ClearGlobalPolicy() {
        glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, BoolParameter: glbPolicy.BoolParameter, Endorsement: { EndorsementType: EndorsementType.Modification }, Change: false };
    }
    static ValidateEndorsementBase(endorsementType) {
        var dfd = $.Deferred();
        if (glbPolicy.Id > 0) {
            var objectModel = Search.CreatePolicyBase(endorsementType);
            SearchRequest.ValidateEndorsement(objectModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        glbPolicy.HasEvent = data.result.HasEvent;
                        glbPolicy.IssueDate = FormatFullDate(data.result.IssueDate);
                        glbPolicy.Message = data.result.Message;
                        if (data.result.Message != "" && !glbPolicy.HasEvent) {
                            dfd.reject(false);
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.Message, 'autoclose': true });
                        }
                        else {
                            dfd.resolve(true);
                        }
                    }
                    else {
                        dfd.reject(false);
                    }
                }
                else {
                    dfd.reject(false);
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            dfd.resolve(true);
        }
        return dfd.promise();
    }

}