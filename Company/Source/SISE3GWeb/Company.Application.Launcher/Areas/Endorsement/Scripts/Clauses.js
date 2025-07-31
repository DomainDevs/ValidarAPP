//Codigo de la pagina Clauses.cshtml
var globalClausesLoaded = [];
var glbPaymentPlan;
class Clauses extends Uif2.Page {
    getInitialState() {
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            Clauses.GetEndorsement();
            Clauses.LoadData();
            Clauses.CalculateDays();
            Clauses.Coinsurance();
            Clauses.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
            $("#inputFilingNumber").OnlyDecimals(0);
            $('#textClausesRecord').prop('disabled', true);
            Clauses.LoadRecordObservation();
            GetDateIssue();
            this.ValidatePolicies();
            Clauses.GetClausesByLevelsConditionLevelId(glbPolicy.Prefix.Id);
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
        }
    }

    bindEvents() {
        $("#inputFrom").UifDatepicker('setMaxDate', $("#inputTo").val());
        $("#inputFrom").UifDatepicker('setMinDate', $("#inputFrom").val());
        $("#inputFrom").on("datepicker.change", function (event, date) {
            Clauses.CalculateDays();
        });
        $("#selectEndorsement").UifSelect("disabled", true);
        $('#tableClauses tbody').on('click', 'tr', function (e) {
            e.preventDefault();
            var title = $(this).children()[0].innerHTML;
            if (!$(this).hasClass('row-selected')) {
                $.each($('#tableClauses').UifDataTable('getData'), function (key, value) {
                    if (title == this.Name) {
                        $('#textClausesDetail').val(this.Text);
                    }
                });
            } else {
                $('#textClausesDetail').val('');
            }
        });

        $("#btnCancel").on('click', function () {
            Clauses.RedirectSearchController();
        });

        $("#btnSave").on('click', function () {
            if (glbPaymentPlan == EnumRateTypePrev.PreviCredit30 || glbPaymentPlan == EnumRateTypePrev.PreviCredit10 || glbPaymentPlan == EnumRateTypePrev.PreviCredit20) {
                $.UifDialog('confirm',
                    { 'message': Resources.Language.NotificationPaymentPlan },
                    function (result) {
                        if (result) {
                            Clauses.ValidateCreateClausesCoInsuredAccepted();
                        }
                    });
            } else {
                Clauses.ValidateCreateClausesCoInsuredAccepted();
            }
            
        });
        $("#btnModalClose").on("click", function () {
            Clauses.RedirectSearchController();
        });
    }
    static Coinsurance() {
        switch (glbPolicy.BusinessType) {
            case BusinessType.Accepted:
                $("#divCoInsuranceAccepted").show();
                break;
            case BusinessType.Assigned:
                $("#divCoInsuranceAccepted").hide();
                break;
            default:
                $("#divCoInsuranceAccepted").hide();
                break;
        }
    }
    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    let id = glbPolicy.Id;
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Clauses.RedirectSearchController();
                }
            });
        }
    }

    static GetEndorsement() {
        var controller = "";
        var selectedOptions = 0;
        ClausesEndorsementRequest.GetEndorsementsByPrefixIdBranchIdPolicyNumber(glbPolicy.Branch.Id, glbPolicy.Prefix.Id, glbPolicy.DocumentNumber, true).done(function (data) {
            if (data.success) {
                controller = data.result.endorsements;
                selectedOptions = data.result.selectedId;
                if (selectedOptions == 0) {
                    $('#selectEndorsement').UifSelect({ sourceData: controller });

                } else {
                    $('#selectEndorsement').UifSelect({ sourceData: controller, selectedId: selectedOptions });
                    Clauses.GetPaymentPlanId(controller[0].PolicyId, controller[0].Id);
                }
            }
        });
    }

    static GetPaymentPlanId(temporalId, endorsementId) {
        ClausesEndorsementRequest.GetPaymentPlanScheduleByPolicyEndorsementId(temporalId, endorsementId).done(function (data) {
            if (data.success) {
                glbPaymentPlan = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }


    static CalculateDays() {
        var aFecha1 = $("#inputFrom").val().toString().split('/');
        var aFecha2 = $("#inputTo").val().toString().split('/');
        var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(dias)) {
            $("#inputDays").val(0);
        }
        else {
            $("#inputDays").val(dias);
        }
    }

    static GetClausesByLevelsConditionLevelId(conditionLevelId) {
        ClausesEndorsementRequest.GetClausesByLevelsConditionLevelId(Levels.General, conditionLevelId).done(function (data) {
            if (data.success) {
                var ClausesIsMandatory =
                {
                    label: 'Id',
                    values: []
                };
                $('#tableClauses').UifDataTable('clear');
                if (data.result.length > 0) {
                    $('#tableClauses').UifDataTable('addRow', data.result);
                    $.each(data.result, function (id, item) {
                        if (item.IsMandatory == true) {
                            ClausesIsMandatory.values.push(item.Id);
                        }
                    });
                    $("#tableClauses").UifDataTable('setSelect', ClausesIsMandatory)
                }
            }
            else {
                $('#tableClauses').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchClauses, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchClauses, 'autoclose': true })
        });
    } 

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static ValidateCreateClausesCoInsuredAccepted() {
        if (glbPolicy.BusinessType == BusinessType.Accepted) {
            if ($("#inputBusinessTypeDescription").val() != 0) {
                Clauses.CreateClauses();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ValidateCoInsuranceAccepted, 'autoclose': true });
            }
        }
        else {
            Clauses.CreateClauses();
        }
    }
    static CreateClauses() {
        if ($("#formClauses").valid()) {
            var modificationModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formClauses);
            modificationModel.Clauses = $('#tableClauses').UifDataTable('getSelected');
            var countMandatory = 0;
            if (modificationModel.Clauses != null)
                modificationModel.Clauses.forEach((x) => { if (x.IsMandatory) countMandatory++ });
            if (countMandatory >= globalClausesLoaded.length) {
                var companyModification = {
                    Text: $('#inputText').val(),
                    Observations: $('#inputObservations').val(),
                    RegistrationNumber: $("#inputTicketNumber").val(),
                    RegistrationDate: $("#inputTicketDate").val()
                };

                lockScreen();
                ClausesEndorsementRequest.CreateClauses(modificationModel, companyModification).done(function (data) {
                    if (data.success) {
                        if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            glbPolicy.Id = data.result.Id ? data.result.Id : data.result.TemporalId;
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, glbPolicy.Id, FunctionType.Individual);
                        } else {
                            var message = "";
                            glbPolicy.EndorsementOriginId = glbPolicy.Endorsement.Id;
                            glbPolicy.Change = true;

                            if (data.result.EndorsementId === undefined) {
                                glbPolicy.Endorsement.Id = data.result.EndorsementId;
                            } else { glbPolicy.Endorsement.Id = data.result.EndorsementId; }
                            if (data.result.EndorsementNumber === undefined) {
                                glbPolicy.Endorsement.Number = data.result.Endorsement.Number;
                                message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result.Endorsement.Number;
                            } else {
                                glbPolicy.Endorsement.Number = data.result.EndorsementNumber;
                                message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result.EndorsementNumber;
                            }
                            glbPolicy.Id = 0;
                            
                            $.UifDialog('confirm', { 'message': message + '\n' + Resources.Language.MessagePrint }, function (result) {
                                if (result) {
                                    var endorsementNumber = data.result.EndorsementNumber;
                                    var endorsementId = 0;
                                    var policyId = 0;
                                    if (data.result.Endorsement != undefined) {
                                        endorsementId = data.result.Endorsement.Id;
                                        policyId = data.result.Endorsement.PolicyId;
                                    }
                                    else {
                                        endorsementId = data.result.EndorsementId;
                                        policyId = data.result.PolicyId;
                                    }
                                    PrinterRequest.PrintReportEndorsement(glbPolicy.Prefix.Id,
                                        policyId, endorsementId).done(function (data) {
                                            if (data.success) {
                                                if (data.result.Url != undefined) {
                                                    DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.pdf)/)[0]);
                                                }
                                                else {
                                                    $.UifNotify('show', { 'type': 'info', 'message': 'Se genero el proceso de impresion numero ' + data.result + ' favor consultarlo en pantalla de impresion', 'autoclose': false });
                                                }
                                            }
                                            else {
                                                if (data.result == Resources.Language.EndorsmentNotReinsured) {
                                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data.result, 'autoclose': true });
                                                }
                                                else {
                                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                                }
                                            }
                                        }).fail(function (data) {
                                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                        });
                                }
                                Clauses.RedirectSearchController();
                            });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                    unlockScreen();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveClauses, 'autoclose': true });
                    unlockScreen();
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': 'No ha seleccionado todas las clausulas obligatorias', 'autoclose': true });
            }
        }
    }

    static LoadCurrentSummaryEndorsement(policyData) {
        if (policyData != null && policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);
        }
    }
    static LoadData() {
        $('#inputFrom').val(FormatDate(glbPolicy.CurrentFrom));
        $('#inputTo').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
    }

    static LoadRecordObservation() {
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0) {
            var recordText = "";
            glbPolicy.EndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#textClausesRecord').val(recordText);
        }
    }
}

