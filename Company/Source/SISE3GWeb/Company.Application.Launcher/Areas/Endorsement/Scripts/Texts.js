var glbPaymentPlan;
class Texts extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $("#inputFilingNumber").OnlyDecimals(0);
            $("#inputCurrentFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $("#inputCurrentTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $('#inputTextRecord').prop('disabled', true);
            $("#selectEndorsement").UifSelect();
            Texts.GetEndorsement();
            Texts.Coinsurance();
            GetDateIssue();
            Texts.LoadData();
            Texts.CalculateDays();
            Texts.LoadRecordObservation();
            this.ValidatePolicies();
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);           
        }
    }

    bindEvents() {
        $('#inputCurrentFrom').UifDatepicker();
        $('#inputCurrentTo').UifDatepicker();
        $('#inputFilingDate').UifDatepicker();
        $("#inputCurrentFrom").on("datepicker.change", function (event, date) {
            $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val()));
        });
        $("#inputTextPrecataloged").on('buttonClick', function () {
            if ($.trim($("#inputTextPrecataloged").val()) != "") {
                Texts.GetTextsByNameLevelIdConditionLevelId($("#inputTextPrecataloged").val(), glbPolicy.Prefix.Id);
            }
        });
        $("#btnCancel").on('click', function () {
            Texts.RedirectSearchController();
        });
        $("#btnSave").on("click", function () {
            if (glbPaymentPlan == EnumRateTypePrev.PreviCredit30 || glbPaymentPlan == EnumRateTypePrev.PreviCredit10 || glbPaymentPlan == EnumRateTypePrev.PreviCredit20) {
                $.UifDialog('confirm', 
                    { 'message': Resources.Language.NotificationPaymentPlan },
                    function (result) {
                        if (result) {
                            Texts.ValidateCreateTexCoinsurance();
                        }
                    });
            } else {
                Texts.ValidateCreateTexCoinsurance();
            }
            
        });
        $('#tableTextResults tbody').on('click', 'tr', function (e) {
            e.preventDefault();
            $("#inputTextPrecataloged").val($(this).children()[1].innerHTML);
            $("#inputTextEdit").val($(this).children()[2].innerHTML);
        });
        $("#btnTextCancel").on("click", function () {
            $('#modalTextSearch').UifModal("hide");
        });
        $("#btnTextSelect").on("click", function () {
            if ($("#inputText").val().trim().length == 0) {
                $("#inputText").val($("#inputTextEdit").val());
            }
            else {
                $("#inputText").val($("#inputText").val() + ' \n ' + $("#inputTextEdit").val());
            }
            $('#modalTextSearch').UifModal("hide");
        });
        $("#btnModalClose").on("click", function () {
            Texts.RedirectSearchController();
        });
        $("#inputCurrentFrom").UifDatepicker('setMaxDate', $("#inputCurrentTo").val());
        $("#inputCurrentFrom").UifDatepicker('setMinDate', $("#inputCurrentFrom").val());
    }

    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(glbPolicy.Id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Texts.RedirectSearchController();
                }
            });
        }
    }

    static GetTemporalById(temporalId, endorsementId) {


        TextsRequest.GetPaymentPlanScheduleByPolicyEndorsementId(temporalId, endorsementId).done(function (data) {
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

    static LoadTitle() {
        var titlePage = $('#hiddenTitle').val();
        titlePage = titlePage.replace(/¡/g, '<strong>');
        titlePage = titlePage.replace(/!/g, '</strong>');
        $("#globalTitle").html(titlePage);
    }
    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static GetTextsByNameLevelIdConditionLevelId(name, conditionLevelId) {
        TextsRequest.GetTextsByNameLevelIdConditionLevelId(name, Levels.General, conditionLevelId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    Texts.ShowTextList(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchTexts, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorLoadingTexts, 'autoclose': true });
            }
        });
    }

    static ValidateCreateTexCoinsurance() {
        if (glbPolicy.BusinessType == BusinessType.Accepted) {
            if ($("#inputBusinessTypeDescription").val() != 0) {
                Texts.CreateTexts();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ValidateCoInsuranceAccepted, 'autoclose': true });
            }
        }
        else {
            Texts.CreateTexts();
        }
    }

    static CreateTexts() {
        $("#formTexts").validate();
        if ($("#formTexts").valid()) {
            var modificationModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formTexts);
            $('#btnSave').prop('disabled', true);
            var companyModification = {
                Text: $('#inputText').val(),
                Observations: $('#inputTextObservations').val(),
                RegistrationNumber: $("#inputFilingNumber").val(),
                RegistrationDate: $("#inputFilingDate").val()
            };
            lockScreen();
            TextsRequest.CreateTexts(glbPolicy.EndorsementController, modificationModel, companyModification).done(function (data) {
                if (data.success) {
                    $('#btnSave').prop('disabled', false);
                    if (data.result != null) {
                        if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            glbPolicy.Id = data.result.Id ? data.result.Id : data.result.TemporalId;
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, glbPolicy.Id, FunctionType.Individual);
                        } else {
                            var message = "";
                            glbPolicy.EndorsementOriginId = glbPolicy.Endorsement.Id;
                            glbPolicy.Change = true;
                            if (data.result.EndorsementId === undefined) {
                                glbPolicy.Endorsement.Id = data.result.Endorsement.Id;
                            } else { glbPolicy.Endorsement.Id = data.result.EndorsementId;}                          
                            if (data.result.EndorsementNumber === undefined) {
                                glbPolicy.Endorsement.Number = data.result.Endorsement.Number;
                                message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result.Endorsement.Number;
                            } else {
                                glbPolicy.Endorsement.Number = data.result.EndorsementNumber;
                                message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result.EndorsementNumber;
                            }
                            glbPolicy.Id = 0;
                            $.UifDialog('confirm', { 'message': message + '\n ' + Resources.Language.MessagePrint }, function (result) {
                                if (result) {
                                    var endorsementNumber = data.result.EndorsementNumber;
                                    PrinterRequest.PrintReportFromOutside(glbPolicy.Branch.Id,
                                        glbPolicy.Prefix.Id,
                                        glbPolicy.DocumentNumber, data.result.EndorsementId).done(function (data) {
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
                                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                        });
                                }
                                Texts.RedirectSearchController();
                            });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }
                else {
                    $('#btnSave').prop('disabled', false);
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveText, 'autoclose': true });
                }
                unlockScreen();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $('#btnSave').prop('disabled', false);
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveText, 'autoclose': true });
                unlockScreen();
            });
        }
    }
    static ShowTextList(dataTable) {
        $('#tableTextResults').UifDataTable('clear');
        $('#tableTextResults').UifDataTable('addRow', dataTable);
        $('#modalTextSearch').UifModal('showLocal', Resources.Language.LabelSelectText);
        $('#inputTextEdit').val('');
    }
    static CalculateDays() {
        var aFecha1 = $("#inputCurrentFrom").val().toString().split('/');
        var aFecha2 = $("#inputCurrentTo").val().toString().split('/');
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
    static GetEndorsement() {

        var dataSource = "";
        var selectedOptions = 0;
        TextsRequest.GetEndorsementsByPrefixIdBranchIdPolicyNumber(glbPolicy.Branch.Id, glbPolicy.Prefix.Id, glbPolicy.DocumentNumber, true).done(function (data) {
            if (data.success) {
                dataSource = data.result.endorsements;
                selectedOptions = data.result.selectedId;
                if (selectedOptions == 0) {
                    $('#selectEndorsement').UifSelect({ sourceData: dataSource });
                } else {
                    $('#selectEndorsement').UifSelect({ sourceData: dataSource, selectedId: selectedOptions });
                    Texts.GetTemporalById(dataSource[0].PolicyId, dataSource[0].Id);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryEndorsement, 'autoclose': true });
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
    static LoadData() {
        $('#inputCurrentFrom').val(FormatDate(glbPolicy.CurrentFrom));
        $('#inputCurrentTo').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
        $('#modalTextSearch').UifModal("hide");
    }  

    static LoadRecordObservation() {
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0) {
            var recordText = "";
            glbPolicy.EndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#inputTextRecord').val(recordText);
        }
    }
}