var userId = 0;
var insuredId = 0;
var holderId = 0;
var paymentRequestIndividualId = 0;
var chargeRequestIndividualId = 0;
var rowSelected = false;
var individualId = 0;
var riskVehicleId = null;
var riskLocationId = null;
var suretyIndividualId = null;
var noticeToClaim = null;
var IndividualId = 0;
var modelSearchNotice =
{
    claimNoticeId: null,
    coveredRiskTypeId: null,
    prefixId: null
};
var modelSearchCriteria =
{
    claimId: null,
    claimNumber: null,
    policyDocumentNumber: null,
    prefixId: null,
    paymentRequestId: null,
    paymentRequestNumber: null,
    number: null,
    branchId: null,
    endorsementId: null,
    paymentRequestTypeId: null,
    chargeRequestId: null
};
var modelSearchNewClaim =
{
    policyNumber: null,
    branchId: null,
    prefixId: null,
    dateFrom: null
};
var modelSearchNewNotice =
{
    riskId: null,
    endorsementId: null,
    prefixId: null
};
var modelSearchNewClaimByNotice =
{
    claimBranch: null
};

class ClaimSearch extends Uif2.Page {
    getInitialState() {
        ClaimSearch.GetVehicleMakes();
        ClaimSearch.GetCountries();
        ClaimSearch.GetPaymentSource();        
        if (GetQueryParameter("SearchType") !== undefined) {
            switch (parseInt(GetQueryParameter("SearchType"))) {
                case 11:
                    if (GetQueryParameter("BranchId") !== undefined && GetQueryParameter("PrefixId") !== undefined && GetQueryParameter("ClaimNumber") !== undefined && GetQueryParameter("PolicyNumber") !== undefined) {
                        ClaimSearch.LoadSearchType(function (searchTypes) {
                            $("#selectSearchType").UifSelect({ sourceData: searchTypes });
                            $("#selectSearchType").UifSelect('setSelected', 2);
                            $("#selectSearchType").trigger('change');

                            ClaimSearch.GetBranches(function (branches) {
                                $("#selectBranchClaim").UifSelect({ sourceData: branches });
                                $("#selectBranchClaim").UifSelect('setSelected', GetQueryParameter("BranchId"));
                                ClaimSearch.GetPrefixes(function (prefixes) {
                                    $("#selectPrefixClaim").UifSelect({ sourceData: prefixes });
                                    $("#selectPrefixClaim").UifSelect('setSelected', GetQueryParameter("PrefixId"));

                                    $("#inputDocumentNumber").val(GetQueryParameter("PolicyNumber"));
                                    $("#inputClaimSearchClaimNumber").val(GetQueryParameter("ClaimNumber"));
                                    ClaimSearch.Search();
                                });
                            });
                        });
                    }
                    break;
                case 14:
                    if (GetQueryParameter("BranchId") !== undefined && GetQueryParameter("Number") !== undefined && GetQueryParameter("ClaimNumber") !== undefined) {
                        ClaimSearch.LoadSearchType(function (searchTypes) {
                            $("#selectSearchType").UifSelect({ sourceData: searchTypes });
                            $("#selectSearchType").UifSelect('setSelected', 4);
                            $("#selectSearchType").trigger('change');

                            ClaimSearch.GetBranchesByUserId(function (branches) {
                                $("#selectBranchPayment").UifSelect({ sourceData: branches });
                                $("#selectBranchPayment").UifSelect('setSelected', GetQueryParameter("BranchId"));

                                $("#inputPaymentRequestNumber").val(GetQueryParameter("Number"));

                                $("#inputClaimNumber").val(GetQueryParameter("ClaimNumber"));

                                ClaimSearch.Search();
                            });
                        });
                    }
                    break;
                case 17:
                    if (GetQueryParameter("PrefixId") !== undefined && GetQueryParameter("NoticeNumber") !== undefined) {
                        ClaimSearch.LoadSearchType(function (searchTypes) {
                            $("#selectSearchType").UifSelect({ sourceData: searchTypes });
                            $("#selectSearchType").UifSelect('setSelected', 1);
                            $("#selectSearchType").trigger('change');

                            ClaimSearch.GetPrefixes(function (prefixes) {
                                $("#selectPrefixNotice").UifSelect({ sourceData: prefixes });
                                $("#selectPrefixNotice").UifSelect('setSelected', GetQueryParameter("PrefixId"));

                                $("#inputClaimNoticeNumber").val(GetQueryParameter("NoticeNumber"));
                                ClaimSearch.Search();
                            });

                            ClaimSearch.GetBranchesByUserId(function (branches) {
                                $('#selectClaimBranchSearch').UifSelect({ sourceData: branches });
                            });
                        });
                    }
                    break;
                case 23:
                    if (GetQueryParameter("BranchId") !== undefined && GetQueryParameter("Number") !== undefined && GetQueryParameter("PaymentSourceId") !== undefined) {
                        ClaimSearch.LoadSearchType(function (searchTypes) {
                            $("#selectSearchType").UifSelect({ sourceData: searchTypes });
                            $("#selectSearchType").UifSelect('setSelected', 5);
                            $("#selectSearchType").trigger('change');                            

                            ClaimSearch.GetBranchesByUserId(function (branches) {
                                $("#selectBranchCharge").UifSelect({ sourceData: branches });
                                $("#selectBranchCharge").UifSelect('setSelected', GetQueryParameter("BranchId"));

                                $("#inputChargeRequestNumber").val(GetQueryParameter("Number"));

                                ClaimSearch.GetChargeSource(function (paymentSources) {
                                    $('#selectChargeSource').UifSelect({ sourceData: paymentSources });                                    
                                    $("#selectChargeSource").UifSelect('setSelected', GetQueryParameter("PaymentSourceId"));

                                    ClaimSearch.Search();
                                });
                                
                            });
                            
                        });
                    }
                    break;
            }
        }
        else {
            ClaimSearch.LoadSearchType();
            ClaimSearch.GetBranches();
            ClaimSearch.GetPrefixes();
            ClaimSearch.GetBranchesByUserId();
            ClaimSearch.GetChargeSource();
        }

    }

    bindEvents() {

        $('#ddlSearchPersonType').on('itemSelected', ClaimSearch.SelectPersonType);
        $('#ddlClaimSearchPersonType').on('itemSelected', ClaimSearch.SelectClaimPersonType);

        $('#ddlNoticeSearchPersonType').on('itemSelected', ClaimSearch.SelectNoticePersonType);        

        $("#inputAddress").TextTransform(ValidatorType.UpperCase);
        $('#selectSearchType').on('itemSelected', ClaimSearch.ViewSearchType);

        $('#radioClaim').on('click', ClaimSearch.ViewClaimNumber);
        $('#radioTemporary').on('click', ClaimSearch.ViewTemporary);

        $('#radioPaymentRequest').on('click', ClaimSearch.ViewPaymentRequest);
        $('#radioTemporaryPayment').on('click', ClaimSearch.ViewTemporaryPayment);

        $('#btnClean').on('click', ClaimSearch.CleanSearch);
        $('#btnSearch').on('click', ClaimSearch.Search);

        $('#inputUserClaim').on('itemSelected', ClaimSearch.LoadSearchUser);
        $('#inputUserNotice').on('itemSelected', ClaimSearch.LoadSearchUser);

        $('#tblPolicies').on('rowSelected', ClaimSearch.LoadButtons);
        $('#tblPayment').on('rowSelected', ClaimSearch.LoadButtons);
        $('#tblNotices').on('rowSelected', ClaimSearch.LoadButtons);
        $('#tblClaims').on('rowSelected', ClaimSearch.LoadButtons);
        $('#tblCharge').on('rowSelected', ClaimSearch.LoadButtons);

        $('#selectPrefixNotice').on('itemSelected', ClaimSearch.ViewPrefixOptions);
        $('#selectPrefixPolicy').on('itemSelected', ClaimSearch.ViewPrefixOptions);
        $('#selectPrefixClaim').on('itemSelected', ClaimSearch.GetSearchPersonType);
        $('#selectPrefixNotice').on('itemSelected', ClaimSearch.GetSearchPersonTypeNotice);

        $('#selectVehicleMake').on('itemSelected', ClaimSearch.LoadVehicleMake);
        $('#selectVehicleModel').on('itemSelected', ClaimSearch.LoadVehicleModel);
        $('#selectVehicleVersion').on('itemSelected', ClaimSearch.LoadVehicleVersion);

        $('#selectCountryLocation').on('itemSelected', ClaimSearch.SelectCountry);
        $('#selectStateLocation').on('itemSelected', ClaimSearch.SelectState);
        $('#inputBeneficiaryDocumentNum').on('itemSelected', ClaimSearch.LoadBeneficiaryName);
        $('#inputBeneficiaryDocumentNum').on('keydown', function () {
            if (paymentRequestIndividualId != 0) {
                ClaimSearchRequest.GetPaymentBeneficiaryByBeneficiaryId(paymentRequestIndividualId).done(function (response) {
                    if (response.success) {
                        if ($('#inputBeneficiaryDocumentNum').UifAutoComplete('getValue') != response.result.DocumentNumber) {
                            paymentRequestIndividualId = 0;
                            $("#inputBeneficiaryName").val("");
                        }
                    }
                });
            }
        });

        $('#inputBeneficiaryChargeDocumentNum').on('itemSelected', ClaimSearch.LoadBeneficiaryChargeName);
        $('#inputBeneficiaryChargeDocumentNum').on('keydown', function () {
            if (chargeRequestIndividualId != 0) {
                ClaimSearchRequest.GetPaymentBeneficiaryByBeneficiaryId(paymentRequestIndividualId).done(function (response) {
                    if (response.success) {
                        if ($("#inputBeneficiaryChargeDocumentNum").UifAutoComplete('getValue') != response.result.DocumentNumber) {
                            chargeRequestIndividualId = 0;
                            $("#inputBeneficiaryChargeName").val("");
                        }
                    }
                });
            }
        });

        $('#inputAddress').on('buttonClick', ClaimSearch.GetRiskLocationByAddress);
        $("#inputPlate").on('buttonClick', ClaimSearch.GetRisksByPlate);
        $('#inputSuretyName').on('buttonClick', ClaimSearch.GetRiskSuretyByInsuredId);
        $("#tblVehicles").on('rowSelected', ClaimSearch.SetVehicleInformation);
        $("#tblLocation").on('rowSelected', ClaimSearch.SetLocationInformation);
        $("#tblSurety").on('rowSelected', ClaimSearch.LoadSuretyName);

        //AVISO BOTONES
        $('#btnConvertToClaim').on('click', ClaimSearch.ToConvertNoticeToClaim);
        $('#btnClaimNoticeYes').on('click', ClaimSearch.ConvertNoticeToClaim);
        $('#btnEditNotice').on('click', ClaimSearch.ToEditNotice);
        $('#btnScheduleNotice').on('click', ClaimSearch.ScheduleNotice);
        $('#btnObjected').on('click', ClaimSearch.NoticeObjection);
        $('#btnObjectedYes').on('click', ClaimSearch.UpdateNoticeObjection);

        //DENUNCIA BOTONES
        $('#btnEditClaim').on('click', ClaimSearch.ToEditClaim);
        $('#btnPay').on('click', ClaimSearch.ToPayClaim);
        $('#btnSalvage').on('click', ClaimSearch.ToSalvageClaim);
        $('#btnRecovery').on('click', ClaimSearch.ToRecoveryClaim);
        $('#btnSetClaimReserve').on('click', ClaimSearch.ToSetClaimReserve);

        //POLIZA BOTONES
        $('#btnNewNotice').on('click', ClaimSearch.ToCreateNotice);
        $('#btnNewClaim').on('click', ClaimSearch.ToCreateClaim);
        $("#inputHolderPolicy").on("itemSelected", ClaimSearch.SetHolderInformation);        
        $("#inputHolderPolicy").on("keyup", ClaimSearch.ClearPersonInformation);


        //SOLICITUD DE PAGOS BOTONES
        $('#btnEditPay').on('click', ClaimSearch.ToConsultPaymentRequest);
        $('#btnCancelPay').on('click', ClaimSearch.ToCancelPaymentRequest);
        $("#btnPrint").on('click', ClaimSearch.CreatePrintPaymentRequest)

        //SOLICITUD DE COBRO BOTONES
        $("#btnConsulCharge").on('click', ClaimSearch.ToConsultChargeRequest);
        $("#btnCancelCharge").on('click', ClaimSearch.ToCancelChargeRequest);
        
        //VALIDATIONS
        $('#dest').on("blur", ClaimSearch.ValidateEmail);
        $('#inputClaimNoticeNumber').ValidatorKey(ValidatorType.Number, 2, 1);
        $('#inputClaimSearchTemporary').ValidatorKey(ValidatorType.Number, 1, 1);
        $("#inputDocumentNumber").ValidatorKey(ValidatorType.Number, 2, 1);
        $("#inputDocumentNumberNotice").ValidatorKey(ValidatorType.Number, 2, 1);
        $("#inputPolicy").ValidatorKey(ValidatorType.Number,2,1);
        $('#inputPaymentRequestNumber').ValidatorKey(ValidatorType.Number, 2, 1);
        $('#inputChargeRequestNumber').ValidatorKey(ValidatorType.Number, 2, 1);
        $('#inputClaimNumber').ValidatorKey(ValidatorType.Number, 2, 1);

        $('#inputPaymentTemporaryNumber').ValidatorKey(ValidatorType.Number, 1, 1);        
        $('#inputDateNoticeFromNotice').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateNoticeToNotice').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateOcurrenceFromNotice').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateOcurrenceToNotice').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateOcurrenceFromClaim').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateOcurrenceToClaim').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateNoticeFromClaim').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateNoticeToClaim').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateFrom').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDateTo').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputPaymentRequestEstimatedDate').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);

        //AGENDAR
        $('#btnSchedule').on('click', ClaimSearch.SaveSchedule);
        $('#btnCancelSchedule').on('click', ClaimSearch.ClearSchedule);

        $('#insuredSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.FullName);
            }
        });
        $("#insuredSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#participantSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
            }
        });
        $("#participantSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#holderSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                holderId = selectedItem.IndividualId;
            }
        });
        $("#holderSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#suretySearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.FullName);
            }
        });
        $("#suretySearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#insuredClaimSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.FullName);
            }
        });
        $("#insuredClaimSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#insuredNoticeSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.FullName);
            }
        });
        $("#insuredNoticeSearchName").on("keyup", ClaimSearch.ClearPersonInformation);
               
        $('#participantClaimSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.Fullname);
            }
        });
        $("#participantClaimSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#participantNoticeSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.Id;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.Fullname);
            }
        });
        $("#participantNoticeSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#holderClaimSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                holderId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.FullName);
            }
        });
        $("#holderClaimSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#holderNoticeSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                holderId = selectedItem.IndividualId;
                sessionStorage.setItem('TypePersonSelectedName', selectedItem.FullName);
            }
        });
        $("#holderNoticeSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#suretyClaimSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
            }
        });
        $("#suretyClaimSearchName").on("keyup", ClaimSearch.ClearPersonInformation);

        $('#suretyNoticeSearchName').on('itemSelected', function (event, selectedItem) {
            if (selectedItem != null) {
                IndividualId = selectedItem.IndividualId;
            }
        });
        $("#suretyNoticeSearchName").on("keyup", ClaimSearch.ClearPersonInformation);
    }

    static SelectNoticePersonType(event, selectedItem) {
        switch (parseInt(selectedItem.Id)) {
            case ClaimPersonType.Insured:
                $('#divNoticeHolderSearchName').hide();
                $('#divNoticeParticipantSearchName').hide();
                $('#divNoticeSuretySearchName').hide();
                $('#divNoticeInsured').show();
                $('#insuredNoticeSearchName').attr('disabled', false);
                $('#insuredNoticeSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.Participant:
                $('#divNoticeHolderSearchName').hide();
                $('#divNoticeParticipantSearchName').show();
                $('#divNoticeInsured').hide();
                $('#divNoticeSuretySearchName').hide();
                $('#participantNoticeSearchName').attr('disabled', false);
                $('#participantNoticeSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.Holder:
                $('#divNoticeHolderSearchName').show();
                $('#divNoticeParticipantSearchName').hide();
                $('#divNoticeInsured').hide();
                $('#divNoticeSuretySearchName').hide();
                $('#holderNoticeSearchName').attr('disabled', false);
                $('#holderNoticeSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.SuretyPerson:
                $('#divNoticeSuretySearchName').show();
                $('#divNoticeParticipantSearchName').hide();
                $('#divNoticeHolderSearchName').hide();
                $('#divNoticeInsured').hide();
                $('#suretyNoticeSearchName').attr('disabled', false);
                $('#suretyNoticeSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;

            default:
                $('#insuredNoticeSearchName').attr('disabled', true);
                $('#participantNoticeSearchName').attr('disabled', true);
                $('#holderNoticeSearchName').attr('disabled', true);
                $('#suretyNoticeSearchName').attr('disabled', true);
                $("#insuredNoticeSearchName").val('');
                $("#participantNoticeSearchName").val('');
                $("#holderNoticeSearchName").val('');
                $("#suretyNoticeSearchName").val('');
                IndividualId = 0;
                holderId = 0;
                break;
        }
    }


    static SelectClaimPersonType(event, selectedItem) {
        switch (parseInt(selectedItem.Id)) {
            case ClaimPersonType.Insured:
                $('#divClaimHolderSearchName').hide();
                $('#divClaimParticipantSearchName').hide();
                $('#divClaimSuretySearchName').hide();
                $('#divClaimInsured').show();
                $('#insuredClaimSearchName').attr('disabled', false);
                $('#insuredClaimSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.Participant:
                $('#divClaimHolderSearchName').hide();
                $('#divClaimParticipantSearchName').show();
                $('#divClaimInsured').hide();
                $('#divClaimSuretySearchName').hide();
                $('#participantClaimSearchName').attr('disabled', false);
                $('#participantClaimSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.Holder:
                $('#divClaimHolderSearchName').show();
                $('#divClaimParticipantSearchName').hide();
                $('#divClaimInsured').hide();
                $('#divClaimSuretySearchName').hide();
                $('#holderClaimSearchName').attr('disabled', false);
                $('#holderClaimSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.SuretyPerson:
                $('#divClaimSuretySearchName').show();
                $('#divClaimParticipantSearchName').hide();
                $('#divClaimHolderSearchName').hide();
                $('#divClaimInsured').hide();
                $('#suretyClaimSearchName').attr('disabled', false);
                $('#suretyClaimSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;

            default:
                $('#insuredClaimSearchName').attr('disabled', true);
                $('#participantClaimSearchName').attr('disabled', true);
                $('#holderClaimSearchName').attr('disabled', true);
                $('#suretyClaimSearchName').attr('disabled', true);
                $("#insuredClaimSearchName").val('');
                $("#participantClaimSearchName").val('');
                $("#holderClaimSearchName").val('');
                $("#suretyClaimSearchName").val('');
                IndividualId = 0;
                holderId = 0;
                break;
        }
    }

    static SelectPersonType(event, selectedItem) {

        switch (parseInt(selectedItem.Id)) {
            case ClaimPersonType.Insured:
                $('#divHolderSearchName').hide();
                $('#divParticipantSearchName').hide();
                $('#divSuretySearchName').hide();
                $('#divInsuredFullName').show();
                $('#insuredSearchName').attr('disabled', false);
                $('#insuredSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.Participant:
                $('#divHolderSearchName').hide();
                $('#divParticipantSearchName').show();
                $('#divInsuredFullName').hide();
                $('#divSuretySearchName').hide();
                $('#participantSearchName').attr('disabled', false);
                $('#participantSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.Holder:
                $('#divHolderSearchName').show();
                $('#divParticipantSearchName').hide();
                $('#divInsuredFullName').hide();
                $('#divSuretySearchName').hide();
                $('#holderSearchName').attr('disabled', false);
                $('#holderSearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
            case ClaimPersonType.SuretyPerson:
                $('#divSuretySearchName').show();
                $('#divParticipantSearchName').hide();
                $('#divHolderSearchName').hide();
                $('#divInsuredFullName').hide();
                $('#suretySearchName').attr('disabled', false);
                $('#suretySearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;

            default:
                $('#insuredSearchName').attr('disabled', true);
                $('#participantSearchName').attr('disabled', true);
                $('#holderSearchName').attr('disabled', true);
                $('#suretySearchName').attr('disabled', true);
                $('#participantSearchName').val('');
                $('#insuredSearchName').val('');
                $('#holderSearchName').val('');
                $('#suretySearchName').val('');
                IndividualId = 0;
                holderId = 0;
                break;
        }
    }

    static LoadSearchType(callback) {
        ClaimSearchRequest.GetSearchTypes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectSearchType').UifSelect({ sourceData: response.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        $("#inputClaimSearchTemporary").attr('disabled', true);
        $("#inputBeneficiaryName").attr('disabled', true);
        $("#inputPaymentTemporaryNumber").attr('disabled', true);
        $("#inputBeneficiaryChargeName").attr('disabled', true);

        $('#noticeSearchPartial').hide();
        $('#claimSearchPartial').hide();
        $('#policySearchPartial').hide();
        $('#paymentSearchPartial').hide();
        $('#chargeSearchPartial').hide();
        $("#buttonsRescue").hide();
        $("#toolbar").hide();
        $("#searchClaimTable").hide();
        $("#searchNoticeTable").hide();
        $("#searchPoliciesTable").hide();
        $("#searchPaymentTable").hide();
        $("#searchChargeTable").hide();
        $("#policyVehicleSearchPartial").hide();
        $("#policySuretySearchPartial").hide();
        $("#policyLocationSearchPartial").hide();
    }

    static GetPrefixes(callback) {
        ClaimSearchRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectPrefixNotice').UifSelect({ sourceData: response.result });
                $('#selectPrefixClaim').UifSelect({ sourceData: response.result });
                $('#selectPrefixPolicy').UifSelect({ sourceData: response.result });
                ClaimSearch.HideDivsPersonSearch();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetBranches(callback) {
        ClaimSearchRequest.GetBranches().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectBranchClaim').UifSelect({ sourceData: response.result });
                $('#selectBranchNotice').UifSelect({ sourceData: response.result });
                $('#selectBranchPolicy').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetBranchesByUserId(callback) {
        ClaimSearchRequest.GetBranchesByUserId().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectBranchPayment').UifSelect({ sourceData: response.result });
                $('#selectClaimBranchSearch').UifSelect({ sourceData: response.result });
                $('#selectBranchCharge').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleMakes() {
        ClaimSearchRequest.GetVehicleMakes().done(function (response) {
            if (response.success) {
                $('#selectVehicleMake').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleModelsByMakeId(vehicleMakeId) {
        ClaimSearchRequest.GetVehicleModelsByMakeId(vehicleMakeId).done(function (response) {
            if (response.success) {
                $('#selectVehicleModel').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleVersionsByMakeIdModelId(vehicleMakeId, vehicleModelId) {
        ClaimSearchRequest.GetVehicleVersionsByMakeIdModelId(vehicleMakeId, vehicleModelId).done(function (response) {
            if (response.success) {
                $('#selectVehicleVersion').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId) {
        ClaimSearchRequest.GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId).done(function (response) {
            if (response.success) {
                $('#selectVehicleVersionYear').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetCountries() {
        ClaimSearchRequest.GetCountries().done(function (response) {
            if (response.success) {
                $('#selectCountryLocation').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetStatesByCountryId(CountryId) {
        ClaimSearchRequest.GetStatesByCountryId(CountryId).done(function (response) {
            if (response.success) {
                $('#selectStateLocation').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        ClaimSearchRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
            if (response.success) {
                $('#selectCityLocation').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetPaymentSource() {
        ClaimSearchRequest.GetPaymentSource().done(function (response) {
            if (response.success) {
                $('#selectPaymentSource').UifSelect({ sourceData: response.result, enable: false });
                $("#selectPaymentSource").UifSelect('setSelected', 1); //informacion por defecto para ese modulo
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetChargeSource(callback) {
        ChargeRequest.GetPaymentSourcesByChargeRequest().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectChargeSource').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetPaymentSourcesByChargeRequest, 'autoclose': true });
            }
        });
    }

    static SetHolderInformation(event, holder) {
        holderId = holder.IndividualId;
        sessionStorage.setItem('HolderSelectedName', holder.FullName);
    }

    static SearchNotice() {
        $("#buttonsRescue").hide();
        $("#tblNotices").UifDataTable('clear');
        if ($("#inputDocumentNumberNotice").val() != "" || $("#inputClaimNoticeNumber").val() != "" || ($("#inputDateNoticeFromNotice").val() != "" && $("#inputDateNoticeToNotice").val() != "")) {
            var prefixId = 0;
            var ClaimNoticeType = 0;
            var validateNotice = true;

            ClaimSearchRequest.GetClaimPrefixCoveredRiskTypeByPrefixCode(prefixId).done(function (response) {
                if (response.success) {
                    ClaimNoticeType = response.result;

                    if ((($("#inputDateNoticeFromNotice").val() != "") && ($("#inputDateNoticeToNotice").val() == "")) ||
                        (($("#inputDateNoticeFromNotice").val() == "") && ($("#inputDateNoticeToNotice").val() != ""))) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfNoticeDates, 'autoclose': true });
                    }
                    else if ((($("#inputDateOcurrenceFromNotice").val() != "") && ($("#inputDateOcurrenceToNotice").val() == "")) ||
                        (($("#inputDateOcurrenceFromNotice").val() == "") && ($("#inputDateOcurrenceToNotice").val() != ""))) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfOcurrenceDates, 'autoclose': true });
                    }
                    else if ($("#inputDocumentNumberNotice").val() != "" || $("#inputClaimNoticeNumber").val() != "" || ($("#inputDateNoticeFromNotice").val() != "" && $("#inputDateNoticeToNotice").val() != "")) {//$("#inputClaimNoticeNumber").val() != "" || $("#selectPrefixNotice").val() != "") {
                                                
                        if ($("#inputDateNoticeFromNotice").val() != "" && $("#inputDateNoticeToNotice").val() != "") {
                            if (CompareClaimDates($("#inputDateNoticeFromNotice").val(), $("#inputDateNoticeToNotice").val())) {
                                var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateNoticeFromNotice").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateNoticeToNotice").val()) + '"';
                                $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
                                validateNotice = false;
                            }
                        }

                        if ($("#inputDateOcurrenceFromNotice").val() != "" && $("#inputDateOcurrenceToNotice").val() != "") {
                            if (CompareClaimDates($("#inputDateOcurrenceFromNotice").val(), $("#inputDateOcurrenceToNotice").val())) {
                                var msgValidDateOcurrence = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateOcurrenceFromNotice").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateOcurrenceToNotice").val()) + '"';
                                $.UifNotify('show', { 'type': 'danger', 'message': msgValidDateOcurrence, 'autoclose': true });
                                validateNotice = false;
                            }
                        }

                        if (validateNotice) {
                            ClaimSearch.SetParametersNotice(ClaimNoticeType);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ChooseSearchParameters + ' (' + Resources.Language.NoticeNumber + ')' + ' ó (' + Resources.Language.Prefix + ')', 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ObligatoryNoticeNumberNoticeDates, 'autoclose': true });
        }
    }

    static SearchClaim() {
        $("#buttonsRescue").hide();

        $("#tblClaims").UifDataTable('clear');
        if (($("#inputDocumentNumber").val() != "" || $("#inputClaimSearchClaimNumber").val()) != "" || ($("#inputDateOcurrenceFromClaim").val() != "" && $("#inputDateOcurrenceToClaim").val() != "")) {
            if ($("#inputDateOcurrenceFromClaim").val() == "" && $("#inputDateOcurrenceToClaim").val() == "" &&
                $("#inputDateNoticeFromClaim").val() == "" && $("#inputDateNoticeToClaim").val() == "" &&
                $("#selectBranchClaim").val() == "" && $("#selectPrefixClaim").val() == "" &&
                $("#inputUserClaim").UifAutoComplete('getValue') == "" && $("#inputClaimSearchClaimNumber").val() == "" && $("#inputDocumentNumber").val() == ""
            ) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeSearchParameters, 'autoclose': true });
            }
            else {
                //Valida si esta chequeado denuncia
                if ((($("#inputDateOcurrenceFromClaim").val() != "") && ($("#inputDateOcurrenceToClaim").val() == "")) ||
                    (($("#inputDateOcurrenceFromClaim").val() == "") && ($("#inputDateOcurrenceToClaim").val() != ""))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfOcurrenceDates, 'autoclose': true });
                }
                else if ((($("#inputDateNoticeFromClaim").val() != "") && ($("#inputDateNoticeToClaim").val() == "")) ||
                    (($("#inputDateNoticeFromClaim").val() == "") && ($("#inputDateNoticeToClaim").val() != ""))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfNoticeDates, 'autoclose': true });
                }
                else {
                    var validateDate = true;

                    //Valida que la Fecha de Ocurrencia desde no sea mayor que Fecha de Ocurrencia Hasta
                    if (($("#inputDateOcurrenceFromClaim").val() != "") && ($("#inputDateOcurrenceToClaim").val() != "")) {
                        if (CompareClaimDates($("#inputDateOcurrenceFromClaim").val(), $("#inputDateOcurrenceToClaim").val())) {
                            var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateOcurrenceFromClaim").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateOcurrenceToClaim").val()) + '"';
                            $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
                            validateDate = false;
                        }
                    }

                    //Valida que la Fecha de Aviso Desde no sea mayor que Fecha de Aviso Hasta
                    if (($("#inputDateNoticeFromClaim").val() != "") && ($("#inputDateNoticeToClaim").val() != "")) {
                        if (CompareClaimDates($("#inputDateNoticeFromClaim").val(), $("#inputDateNoticeToClaim").val())) {
                            var msgValidNoticeDate = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateNoticeFromClaim").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateNoticeToClaim").val()) + '"';
                            $.UifNotify('show', { 'type': 'danger', 'message': msgValidNoticeDate, 'autoclose': true });
                            validateDate = false;
                        }
                    }

                    if (validateDate) {
                        ClaimSearch.SetParametersClaim();
                    }
                }
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ObligatoryNoticeNumberOcurrenceDates, 'autoclose': true });
        }
    }

    static SearchPolicy() {
        $("#buttonsRescue").hide();
        var validateDate = true;
        if ($("#inputPolicy").val() != "" || ($("#inputDateFrom").val() != "" || $("#inputDateTo").val() != "")) {

            if ($("#inputDateFrom").val() != "" || $("#inputDateTo").val() != "" || $("#selectBranchPolicy").val() != "" ||
                $("#selectPrefixPolicy").val() != "" || $("#inputPolicy").val() != "" || $("#inputHolderPolicy").UifAutoComplete('getValue') != "") {
                $("#tblPolicies").UifDataTable('clear');

                if ((($("#inputDateFrom").val() != "") && ($("#inputDateTo").val() == "")) ||
                    (($("#inputDateFrom").val() == "") && ($("#inputDateTo").val() != ""))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeARangeOfDates, 'autoclose': true });
                }
                else {
                    if (($("#inputDateFrom").val() != "") && ($("#inputDateTo").val() != "")) {
                        if (CompareClaimDates($("#inputDateFrom").val(), $("#inputDateTo").val())) {
                            var msgValidDate = Resources.Language.TheDate + ' "' + Resources.Language.Since + ': ' + ($("#inputDateFrom").val() + '" ' + Resources.Language.GreaterThanDate + ' "' + Resources.Language.Untill + ': ' + $("#inputDateTo").val()) + '"';
                            $.UifNotify('show', { 'type': 'danger', 'message': msgValidDate, 'autoclose': true });
                            validateDate = false;
                        }
                    }

                    if (validateDate) {
                        var prefixId = 0;
                        var ClaimNoticeType = 0;

                        if ($("#selectPrefixPolicy").val() != "") {
                            prefixId = $("#selectPrefixPolicy").UifSelect("getSelected");
                        }

                        ClaimSearchRequest.GetClaimPrefixCoveredRiskTypeByPrefixCode(prefixId).done(function (response) {
                            if (response.success) {
                                ClaimSearch.SetParametersPolicy(ClaimNoticeType);
                            }
                        });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TypeSearchParameters, 'autoclose': true });
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ObligatoryNoticeNumberPolicyDates, 'autoclose': true });
        }
    }

    static SearchPaymentRequest() {
        $("#buttonsRescue").hide();
        $("#searchPaymentTable").hide();
        $("#tblPayment").UifDataTable('clear');
        if ($("#selectBranchPayment").val() != "" || $("#inputPaymentRequestNumber").val() != "") {
            ClaimSearch.SetParametersPayment();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ObligatoryNoticeNumberPaymentDates, 'autoclose': true });
        }
    }

    static SearchChargeRequest() {
        $("#buttonsRescue").hide();
        $("#searchPaymentTable").hide();
        $("#tblCharge").UifDataTable('clear');
        if ($("#selectBranchCharge").val() != "" || $("#inputChargeRequestNumber").val() != "") {
            ClaimSearch.SetParametersCharge();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ObligatoryNoticeNumberChargeBranch, 'autoclose': true });
        }
    }

    static SetParametersNotice(ClaimNoticeType) {
        var searchNoticeModel =
        {
            ClaimNoticeType: ClaimNoticeType,
            NoticeNumber: null,
            DateNoticeFrom: null,
            DateNoticeTo: null,
            DateOcurrenceFrom: null,
            DateOcurrenceTo: null,
            UserId: null,
            PrefixId: null,
            /* Vehicle */
            LicensePlate: null,
            VehicleMakeId: null,
            VehicleModelId: null,
            VehicleVersionId: null,
            VehicleYear: null,
            /* Location */
            Address: null,
            CountryId: null,
            StateId: null,
            CityId: null,
            /* Surety */
            SuretyIndividualId: null,
            CourtNumber: null,
            BidNumber: null,
            BranchId: null,
            DocumentNumber: null,
            IndividualId: null,
            HolderId: null            
        };

        //IndividualId Persona
        if (IndividualId > 0) {
            searchNoticeModel.IndividualId = IndividualId;
        }

        if (holderId > 0) {
            searchNoticeModel.HolderId = holderId;
        }

        if ($("#inputClaimNoticeNumber").val() != "") {
            searchNoticeModel.NoticeNumber = $("#inputClaimNoticeNumber").val();
        }

        if ($("#inputDateNoticeFromNotice").val() != "") {
            searchNoticeModel.DateNoticeFrom = $("#inputDateNoticeFromNotice").val();
        }

        if ($("#inputDateNoticeToNotice").val() != "") {
            searchNoticeModel.DateNoticeTo = $("#inputDateNoticeToNotice").val();
        }

        if ($("#inputDateOcurrenceFromNotice").val() != "") {
            searchNoticeModel.DateOcurrenceFrom = $("#inputDateOcurrenceFromNotice").val();
        }

        if ($("#inputDateOcurrenceToNotice").val() != "") {
            searchNoticeModel.DateOcurrenceTo = $("#inputDateOcurrenceToNotice").val();
        }

        if ($("#inputUserNotice").val() != "") {
            searchNoticeModel.UserId = userId;
        }

        if ($("#selectPrefixNotice").UifSelect('getSelected') != "") {
            searchNoticeModel.PrefixId = $("#selectPrefixNotice").val();
            switch (parseInt(searchNoticeModel.PrefixId)) {
                case 2: case 29: case 30: case 31: case 32: case 100:
                    if (suretyIndividualId != null) {
                        searchNoticeModel.RiskId = suretyIndividualId;
                    }
                    break;
                case 3: case 4: case 13: case 15: case 21: case 81: case 82: case 83: case 84: case 85: case 86: case 87: case 88: case 89: case 90: case 91: case 92: case 93: case 94: case 95: case 98:
                    if (riskLocationId != null) {
                        searchNoticeModel.RiskId = riskLocationId;
                    }
                    break;
                case 7:
                    if (riskVehicleId != null) {
                        searchNoticeModel.RiskId = riskVehicleId;
                    }
                    break;
            }
        }

        if ($("#selectVehicleMake").val() != "") {
            searchNoticeModel.VehicleMakeId = $("#selectVehicleMake").val();
        }

        if ($("#selectVehicleModel").val() != "") {
            searchNoticeModel.VehicleModelId = $("#selectVehicleModel").val();
        }

        if ($("#selectVehicleVersion").val() != "") {
            searchNoticeModel.VehicleVersionId = $("#selectVehicleVersion").val();
        }

        if ($("#selectVehicleVersionYear").val() != "") {
            searchNoticeModel.VehicleYear = $("#selectVehicleVersionYear").val();
        }

        if ($("#inputPlate").val() != "") {
            searchNoticeModel.LicensePlate = $("#inputPlate").val();
        }

        if ($("#inputAddress").val() != "") {
            searchNoticeModel.Address = $("#inputAddress").val();
        }

        if ($("#selectCountryLocation").val() != "") {
            searchNoticeModel.CountryId = $("#selectCountryLocation").val();
        }

        if ($("#selectStateLocation").val() != "") {
            searchNoticeModel.StateId = $("#selectStateLocation").val();
        }

        if ($("#selectCityLocation").val() != "") {
            searchNoticeModel.CityId = $("#selectCityLocation").val();
        }

        if ($("#inputSuretyName").val() != "") {
            searchNoticeModel.SuretyIndividualId = suretyIndividualId;
        }

        if ($("#inputBidNumber").val() != "") {
            searchNoticeModel.BidNumber = $("#inputBidNumber").val();
        }

        if ($("#inputCourtNumber").val() != "") {
            searchNoticeModel.CourtNumber = $("#inputCourtNumber").val();
        }

        //Sucursal
        if ($("#selectBranchNotice").val() != "" || $("#selectBranchNotice").val() != 0) {
            searchNoticeModel.BranchId = $("#selectBranchNotice").val();
        }

        //Numero de Poliza
        if ($("#inputDocumentNumberNotice").val() != "") {
            searchNoticeModel.DocumentNumber = $("#inputDocumentNumberNotice").val();
        }

        ClaimSearch.LoadGridNotice(searchNoticeModel);
    }

    static LoadGridNotice(searchModel) {
        lockScreen();
        $("#searchNoticeTable").hide();

        ClaimSearchRequest.SearchClaimNotice(searchModel).done(function (response) {
            if (response.success) {
                $.each(response.result, function (index, value) {
                    this.NoticeDate = FormatDate(this.NoticeDate);
                    this.OcurrenceDate = FormatDate(this.OcurrenceDate);
                    this.ContactInformation = this.ContactName + " - " + this.PhoneNumber;
                    if (this.Email) {
                        this.ContactInformation += " - " + this.Email;
                    }
                });

                $("#tblNotices").UifDataTable({ sourceData: response.result });
                $("#searchNoticeTable").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }            
        }).always(function () {
            unlockScreen();
        });
    }

    static SetParametersClaim() {
        var searchClaimModel = {
            BranchId: null,
            ClaimDateFrom: null,
            ClaimDateTo: null,
            ClaimNumber: null,
            NoticeDateFrom: null,
            NoticeDateTo: null,
            PrefixId: null,
            TemporaryNumber: null,
            UserId: null,
            IndividualId: null,
            HolderId: null,
            DocumentNumber: null
        };

        //IndividualId Persona
        if (IndividualId > 0) {
            searchClaimModel.IndividualId = IndividualId;
        }

        if (holderId > 0) {
            searchClaimModel.HolderId = holderId;
        }

        //Fechas de Ocurrencia
        if ($("#inputDateOcurrenceFromClaim").val() != "") {
            searchClaimModel.ClaimDateFrom = $("#inputDateOcurrenceFromClaim").val();
        }
        if ($("#inputDateOcurrenceToClaim").val() != "") {
            searchClaimModel.ClaimDateTo = $("#inputDateOcurrenceToClaim").val();
        }

        //Fechas de Aviso 
        if ($("#inputDateNoticeFromClaim").val() != "") {
            searchClaimModel.NoticeDateFrom = $("#inputDateNoticeFromClaim").val();
        }
        if ($("#inputDateNoticeToClaim").val() != "") {
            searchClaimModel.NoticeDateTo = $("#inputDateNoticeToClaim").val();
        }

        //Denuncia
        if ($("#inputClaimSearchClaimNumber").val() != "") {
            searchClaimModel.ClaimNumber = $("#inputClaimSearchClaimNumber").val();
        }

        //Temporario
        if ($("#inputClaimSearchTemporary").val() != "") {
            searchClaimModel.TemporaryNumber = $("#inputClaimSearchTemporary").val();
        }

        //Sucursal
        if ($("#selectBranchClaim").val() != "" || $("#selectBranchClaim").val() != 0) {
            searchClaimModel.BranchId = $("#selectBranchClaim").val();
        }  

        //Ramo
        if ($("#selectPrefixClaim").val() != "" || $("#selectPrefixClaim").val() != 0) {
            searchClaimModel.PrefixId = $("#selectPrefixClaim").val();
        }

        if ($("#inputDocumentNumber").val() != "") {
            searchClaimModel.DocumentNumber = $("#inputDocumentNumber").val();
        }

        //Usuario
        if ($("#inputUserClaim").val() != "" && $("#inputUserClaim").val() != null) {
            searchClaimModel.UserId = userId;
        }

        $("#tblClaim").UifDataTable('clear');
        ClaimSearch.LoadGridClaim(searchClaimModel);
    }

    static LoadGridClaim(searchClaimModel) {
        lockScreen();
        $("#searchClaimTable").hide();

        ClaimSearchRequest.GetSearchClaims(searchClaimModel).done(function (response) {
            if (response.success) {

                $.each(response.result, function (index, value) {
                    this.NoticeDate = FormatDate(this.NoticeDate);
                    this.OccurrenceDate = FormatDate(this.OccurrenceDate);
                });
                $("#tblClaims").UifDataTable({ sourceData: response.result });
                $("#searchClaimTable").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }            
        }).always(function () {
            unlockScreen();
        });
    }

    static SetParametersPolicy(ClaimNoticeType) {
        var searchModelPolicy =
        {
            ClaimNoticeType: ClaimNoticeType,
            CurrentFrom: null,
            CurrentTo: null,
            BranchId: null,
            PrefixId: null,
            DocumentNumber: null,
            InsuredId: null,
            HolderId: null,
            RiskId: null,
            RiskDescription: null,
            /* Vehicle */
            Plate: null,
            VehicleMakeCode: null,
            VehicleModelCode: null,
            VehicleVersionCode: null,
            VehicleVersionYearCode: null,
            /* Location */
            Address: null,
            CountryCode: null,
            StateCode: null,
            CityCode: null,
            /* Surety */
            SuretyIndividualId: null,
            BidNumber: null,
            CourtNumber: null,
            IndividualId: null,
            PersonTypeId: null
        };

        if ($("#inputDateFrom").val() != "") {
            searchModelPolicy.CurrentFrom = $("#inputDateFrom").val();
        }

        if ($("#inputDateTo").val() != "") {
            searchModelPolicy.CurrentTo = $("#inputDateTo").val();
        }

        if ($("#selectBranchPolicy").UifSelect('getSelected') != "") {
            searchModelPolicy.BranchId = $("#selectBranchPolicy").UifSelect('getSelected');
        }

        if ($("#selectPrefixPolicy").UifSelect('getSelected') != "") {
            searchModelPolicy.PrefixId = $("#selectPrefixPolicy").UifSelect('getSelected');
            switch (parseInt(searchModelPolicy.PrefixId)) {
                case 2: case 29: case 30: case 31: case 32: case 100:
                    if (suretyIndividualId != null) {
                        searchModelPolicy.RiskId = suretyIndividualId;
                    }
                    break;
                case 3: case 4: case 13: case 15: case 21: case 81: case 82: case 83: case 84: case 85: case 86: case 87: case 88: case 89: case 90: case 91: case 92: case 93: case 94: case 95: case 98:
                    if (riskLocationId != null) {
                        searchModelPolicy.RiskId = riskLocationId;
                    }
                    break;
                case 7:
                    if (riskVehicleId != null) {
                        searchModelPolicy.RiskId = riskVehicleId;
                    }
                    break;
            }
        }

        if ($('#ddlSearchPersonType').UifSelect('getSelected') != "") {
            switch ($('#ddlSearchPersonType').UifSelect('getSelectedText')) {
                case 'ASEGURADO':
                    searchModelPolicy.IndividualId = IndividualId;
                    break;
                case 'AFIANZADO':
                    searchModelPolicy.IndividualId = IndividualId;
                    break;
                default:
            }

            searchModelPolicy.PersonTypeId = parseInt($('#ddlSearchPersonType').UifSelect('getSelected'));
        }

        if ($("#inputPolicy").val() != "") {
            searchModelPolicy.DocumentNumber = $("#inputPolicy").val();
        }

        if ($("#inputHolderPolicy").val() != "") {
            searchModelPolicy.HolderId = holderId;
        }

        if ($("#inputPlate").val() != "") {
            searchModelPolicy.Plate = $("#inputPlate").val();
        }

        ClaimSearch.LoadGridPolicy(searchModelPolicy);
    }

    static LoadGridPolicy(searchModelPolicy) {
        lockScreen();
        $("#searchPoliciesTable").hide();

        ClaimSearchRequest.SearchPolicy(searchModelPolicy).done(function (response) {
            if (response.success) {

                $.each(response.result, function (index, value) {
                    this.CurrentFrom = FormatDate(this.CurrentFrom);
                    this.CurrentTo = FormatDate(this.CurrentTo);
                });

                $("#tblPolicies").UifDataTable({ sourceData: response.result });

                $("#searchPoliciesTable").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }            
        }).always(function () {
            unlockScreen();
        });
    }

    static ClearPersonInformation() {
        
        switch (parseInt($("#selectSearchType").UifSelect("getSelected"))) {

            case 2:

                switch ($('#ddlClaimSearchPersonType').UifSelect('getSelectedText')) {

                    case 'ASEGURADO':

                        if (IndividualId != 0) {                            
                            if ($('#insuredClaimSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                IndividualId = 0;
                            }                                                                                                                                   
                        }
                        break;

                    case 'TERCERO':

                        if (IndividualId != 0) {
                            if ($('#participantClaimSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                IndividualId = 0;
                            }
                        }
                        break;

                    case 'TOMADOR':

                        if (holderId != 0) {                            
                            if ($('#holderClaimSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                holderId = 0;
                            }
                        }                     
                        break;
                }

                switch ($('#ddlNoticeSearchPersonType').UifSelect('getSelectedText')) {

                    case 'ASEGURADO':

                        if (IndividualId != 0) {
                            if ($('#insuredNoticeSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                IndividualId = 0;
                            }
                        }
                        break;

                    case 'TERCERO':

                        if (IndividualId != 0) {
                            if ($('#participantNoticeSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                IndividualId = 0;
                            }
                        }
                        break;

                    case 'TOMADOR':

                        if (holderId != 0) {
                            if ($('#holderNoticeSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                holderId = 0;
                            }
                        }
                        break;
                }


                break;                

            case 3:
                
                if (holderId != 0) {                    
                    if ($('#inputHolderPolicy').UifAutoComplete('getValue') != sessionStorage.getItem('HolderSelectedName')) {
                        holderId = 0;
                    }
                }

                switch ($('#ddlSearchPersonType').UifSelect('getSelectedText')) {
                    case 'ASEGURADO':

                        if (IndividualId != 0) {
                            if ($('#insuredSearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                IndividualId = 0;
                            }
                        }
                        break;

                    case 'AFIANZADO':

                        if (IndividualId != 0) {
                            if ($('#suretySearchName').UifAutoComplete('getValue') != sessionStorage.getItem('TypePersonSelectedName')) {
                                IndividualId = 0;
                            }
                        }
                        break;
                }

                break;
        }
        
    }           

    static SetParametersPayment() {
        var paymentRequest = {
            PaymentSourceId: null,
            BranchId: null,
            Number: null,
            ClaimNumber: null,
            EstimatedDate: null,
            IndividualId: null
        };

        if ($("#selectPaymentSource").UifSelect('getSelected') != "") {
            paymentRequest.PaymentSourceId = $("#selectPaymentSource").UifSelect('getSelected');
        }

        if ($("#selectBranchPayment").UifSelect('getSelected') != "") {
            paymentRequest.BranchId = $("#selectBranchPayment").UifSelect('getSelected');
        }

        if ($("#inputPaymentRequestNumber").val() != "") {
            paymentRequest.Number = $("#inputPaymentRequestNumber").val();
        }

        if ($("#inputClaimNumber").val() != "") {
            paymentRequest.ClaimNumber = $("#inputClaimNumber").val();
        }

        if ($("#inputPaymentRequestEstimatedDate").val() != "") {
            paymentRequest.EstimatedDate = $("#inputPaymentRequestEstimatedDate").val();
        }

        if ($("#inputBeneficiaryDocumentNum").val() != "") {
            paymentRequest.IndividualId = paymentRequestIndividualId;
        }

        ClaimSearch.LoadGridPayment(paymentRequest);
    }

    static LoadGridPayment(paymentRequest) {
        lockScreen();
        $("#searchPaymentTable").hide();

        ClaimSearchRequest.SearchPaymentRequests(paymentRequest).done(function (response) {
            if (response.success) {

                response.result[0].PayedAmount = response.result[0].TotalAmount - response.result[0].TotalTax;

                $("#tblPayment").UifDataTable({ sourceData: response.result });

                $("#searchPaymentTable").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }            
        }).always(function () {
            unlockScreen();
        });
    }


    static SetParametersCharge() {
        var chargeRequest = {
            PaymentSourceId: null,
            BranchId: null,
            Number: null,            
            IndividualId: null
        };

        if ($("#selectChargeSource").UifSelect('getSelected') != "") {
            chargeRequest.PaymentSourceId = $("#selectChargeSource").UifSelect('getSelected');
        }

        if ($("#selectBranchCharge").UifSelect('getSelected') != "") {
            chargeRequest.BranchId = $("#selectBranchCharge").UifSelect('getSelected');
        }

        if ($("#inputChargeRequestNumber").val() != "") {
            chargeRequest.Number = $("#inputChargeRequestNumber").val();
        }

        if ($("#inputBeneficiaryChargeDocumentNum").val() != "") {
            chargeRequest.IndividualId = chargeRequestIndividualId;
        }

        ClaimSearch.LoadGridCharge(chargeRequest);

    }


    static LoadGridCharge(chargeRequest) {
        lockScreen();
        $("#searchChargeTable").hide();

        ClaimSearchRequest.SearchChargeRequests(chargeRequest).done(function (response) {
            if (response.success) {

                $("#tblCharge").UifDataTable({ sourceData: response.result });

                $("#searchChargeTable").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });
    }

    static SearchOption(prefixId) {
        if (prefixId == null || prefixId == "") {
            $("#policyVehicleSearchPartial").hide();
            $("#policySuretySearchPartial").hide();
            $("#policyLocationSearchPartial").hide();
            $("#searchClaimTable").hide();
            $("#searchNoticeTable").hide();
            $("#searchPoliciesTable").hide();
            $("#searchPaymentTable").hide();
            $('#insuredSearchName').attr('disabled', true);
            $('#ddlSearchPersonType').UifSelect();
            $("#insuredSearchName").val("");
            $("#participantSearchName").val("");
            $("#holderSearchName").val("");
            $("#suretySearchName").val("");
            $("#divPersonSearch").hide();
            return;
        }
        else {
            var prefixId = 0;
            ClaimSearchRequest.GetClaimPrefixCoveredRiskTypeByPrefixCode(prefixId).done(function (data) {
                if (data.success) {
                    switch (parseInt(data.result)) {
                        case parseInt(Resources.Language.CoveredRiskTypeVehicle):
                            $("#policyVehicleSearchPartial").show();
                            $("#policySuretySearchPartial").hide();
                            $("#policyLocationSearchPartial").hide();
                            break;
                        case parseInt(Resources.Language.CoveredRiskTypeProperty):
                            $("#policyVehicleSearchPartial").hide();
                            $("#policySuretySearchPartial").hide();
                            $("#policyLocationSearchPartial").show();
                            break;
                        case parseInt(Resources.Language.CoveredRiskTypeSurety):
                            $("#policyVehicleSearchPartial").hide();
                            $("#policySuretySearchPartial").show();
                            $("#policyLocationSearchPartial").hide();
                            break;

                        default:
                            $("#policyVehicleSearchPartial").hide();
                            $("#policySuretySearchPartial").hide();
                            $("#policyLocationSearchPartial").hide();
                            $("#searchClaimTable").hide();
                            $("#searchNoticeTable").hide();
                            $("#searchPoliciesTable").hide();
                            $("#searchPaymentTable").hide();
                    }
                }
            });
        }
    }

    static CleanSearch() {
        rowSelected = false;
        ClaimSearch.HideDivsPersonSearch();
        //AVISO
        $("#inputClaimNoticeNumber").val("");
        $("#inputDateNoticeFromNotice").val("");
        $("#inputDateNoticeToNotice").val("");
        $("#inputDateOcurrenceFromNotice").val("");
        $("#inputDateOcurrenceToNotice").val("");
        $("#inputUserNotice").val("");
        $("#selectBranchNotice").UifSelect('setSelected', null);
        $("#selectPrefixNotice").UifSelect('setSelected', null);
        $("#policyVehicleSearchPartial").hide();
        $("#policyLocationSearchPartial").hide();
        $("#policySuretySearchPartial").hide();

        //DENUNCIA
        $("#inputDateNoticeFromClaim").val("");
        $("#inputDateNoticeToClaim").val("");
        $("#inputDateOcurrenceFromClaim").val("");
        $("#inputDateOcurrenceToClaim").val("");
        $("#inputClaimSearchClaimNumber").val("");
        $("#inputClaimSearchTemporary").val("");
        $("#selectBranchClaim").UifSelect('setSelected', null);
        $("#selectPrefixClaim").UifSelect('setSelected', null);
        $("#inputUserClaim").val("");
        $("#inputDocumentNumber").val("");

        //POLIZA
        $("#inputDateFrom").val("");
        $("#inputDateTo").val("");
        $("#selectBranchPolicy").UifSelect('setSelected', null);
        $("#selectPrefixPolicy").UifSelect('setSelected', null);
        $("#inputPolicy").val("");
        $("#inputHolderPolicy").UifAutoComplete('clean');

        //SOLICITUD DE PAGOS
        $("#selectPaymentSource").UifSelect('setSelected', null);
        $("#selectBranchPayment").UifSelect('setSelected', null);
        $("#inputPaymentRequestNumber").val("");
        $("#inputPaymentTemporaryNumber").val("");
        $("#inputClaimNumber").val("");
        $("#inputPaymentRequestEstimatedDate").val("");
        $("#inputBeneficiaryDocumentNum").val("");
        paymentRequestIndividualId = 0;
        $("#inputBeneficiaryName").val("");
        $("#selectPaymentSource").UifSelect('setSelected', 1);

        //SOLICITUD DE COBRO
        $("#selectChargeSource").UifSelect('setSelected', null);
        $("#selectBranchCharge").UifSelect('setSelected', null);
        $("#inputChargeRequestNumber").val("");
        $("#inputBeneficiaryDocumentNum").UifAutoComplete('clean');
        $("#inputBeneficiaryName").val("");

        //GRID DE BUSQUEDA
        $("#searchClaimTable").hide();
        $("#searchNoticeTable").hide();
        $("#searchPoliciesTable").hide();
        $("#searchPaymentTable").hide();
        $("#searchChargeTable").hide();

        //Búsqueda Específica
        $("#selectVehicleModel").attr('disabled', 'disabled');
        $("#selectVehicleVersion").attr('disabled', 'disabled');
        $("#selectVehicleVersionYear").attr('disabled', 'disabled');
        $("#selectStateLocation").attr('disabled', 'disabled');
        $("#selectCityLocation").attr('disabled', 'disabled');
        $('#selectVehicleModel').UifSelect('setSelected', null);
        $('#selectVehicleVersion').UifSelect('setSelected', null);
        $('#selectVehicleVersionYear').UifSelect('setSelected', null);
        $('#selectStateLocation').UifSelect('setSelected', null);
        $('#selectCityLocation').UifSelect('setSelected', null);

        $("#inputPlate").val("");
        $("#selectVehicleMake").UifSelect('setSelected', null);
        $("#selectCountryLocation").UifSelect('setSelected', null);
        $("#inputSuretyName").val("");
        $("#inputCourtNumber").val("");
        $("#inputBidNumber").val("");
        $("#inputAddress").val("");

        $("#insuredSearchName").val("");
        $("#participantSearchName").val("");
        $("#holderSearchName").val("");
        $("#suretySearchName").val("");

        //BOTONES
        $("#btnEditClaim").hide();
        $("#btnPay").hide();
        $("#btnSalvage").hide();
        $("#btnRecovery").hide();
        $("#btnConvertToClaim").hide();
        $("#btnEditNotice").hide();
        $("#btnScheduleNotice").hide();
        $("#btnObjected").hide();
        $("#btnNewNotice").hide();
        $("#btnNewClaim").hide();
        $("#btnEditPay").hide();
        $("#btnCancelPay").hide();
        $("btnSchedule").hide();
        $("#btnPrint").hide();
        $("#btnSetClaimReserve").hide();

        $("#btnConsulCharge").hide();
        $("#btnCancelCharge").hide();

        $('#insuredSearchName').attr('disabled', true);
        $('#participantSearchName').attr('disabled', true);
        $('#holderSearchName').attr('disabled', true);
        $('#suretySearchName').attr('disabled', true);
        $('#participantSearchName').val('');
        $('#insuredSearchName').val('');
        $('#holderSearchName').val('');
        $('#suretySearchName').val('');
        $('#insuredClaimSearchName').attr('disabled', true);
        $('#insuredNoticeSearchName').attr('disabled', true);
        $('#participantClaimSearchName').attr('disabled', true);
        $('#participantNoticeSearchName').attr('disabled', true);
        $('#holderClaimSearchName').attr('disabled', true);
        $('#holderNoticeSearchName').attr('disabled', true);
        $('#suretyClaimSearchName').attr('disabled', true);
        $('#suretyNoticeSearchName').attr('disabled', true);
        $("#insuredClaimSearchName").val('');
        $("#insuredNoticeSearchName").val('');
        $("#participantClaimSearchName").val('');
        $("#participantNoticeSearchName").val('');
        $("#holderClaimSearchName").val('');
        $("#holderNoticeSearchName").val('');
        $("#suretyClaimSearchName").val('');
        $("#suretyNoticeSearchName").val('');
        $('#ddlSearchPersonType').UifSelect();
        $('#ddlClaimSearchPersonType').UifSelect();
        $('#ddlNoticeSearchPersonType').UifSelect();
        IndividualId = 0;
        holderId = 0;
    }

    static CleanPrefix() {
        suretyIndividualId = null;
        riskVehicleId = null;
        riskLocationId = null;
        holderId = 0;
        rowSelected = false;

        $("#inputPlate").val("");
        $("#selectVehicleMake").val("");
        $('#selectVehicleModel').UifSelect('setSelected', null);
        $("#selectVehicleModel").attr('disabled', 'disabled');
        $('#selectVehicleVersion').UifSelect('setSelected', null);
        $("#selectVehicleVersion").attr('disabled', 'disabled');
        $('#selectVehicleVersionYear').UifSelect('setSelected', null);
        $("#selectVehicleVersionYear").attr('disabled', 'disabled');

        $("#inputSuretyName").val("");
        $("#inputCourtNumber").val("");
        $("#inputBidNumber").val("");
        $("#inputAddress").val("");
        $("#selectCountryLocation").UifSelect('setSelected', null);
        $("#selectStateLocation").UifSelect('setSelected', null);
        $("#selectStateLocation").attr('disabled', 'disabled');
        $("#selectCityLocation").UifSelect('setSelected', null);
        $("#selectCityLocation").attr('disabled', 'disabled');
    }

    static ViewSearchType(event, selectedItem) {
        ClaimSearch.CleanSearch();
        ClaimSearch.ClearInputClaimPerson();
        ClaimSearch.HideDivsPersonSearch();
        $("#toolbar").show();
        switch (parseInt(selectedItem.Id)) {
            case 1:
                $('#noticeSearchPartial').show();
                $('#claimSearchPartial').hide();
                $('#policySearchPartial').hide();
                $('#paymentSearchPartial').hide();
                $('#chargeSearchPartial').hide();
                $('#insuredNoticeSearchName').attr('disabled', true);
                break;
            case 2:
                $('#noticeSearchPartial').hide();
                $('#claimSearchPartial').show();
                $('#policySearchPartial').hide();
                $('#paymentSearchPartial').hide();
                $('#chargeSearchPartial').hide();
                $('#insuredClaimSearchName').attr('disabled', true);
                break;
            case 3:
                $('#noticeSearchPartial').hide();
                $('#claimSearchPartial').hide();
                $('#policySearchPartial').show();
                $('#paymentSearchPartial').hide();
                $('#chargeSearchPartial').hide();
                $('#insuredSearchName').attr('disabled', true);
                break;
            case 4:
                $('#noticeSearchPartial').hide();
                $('#claimSearchPartial').hide();
                $('#policySearchPartial').hide();
                $('#paymentSearchPartial').show();
                $('#chargeSearchPartial').hide();
                break;

            case 5:
                $('#noticeSearchPartial').hide();
                $('#claimSearchPartial').hide();
                $('#policySearchPartial').hide();
                $('#paymentSearchPartial').hide();
                $('#chargeSearchPartial').show();
                break;

            default:
                $('#noticeSearchPartial').hide();
                $('#claimSearchPartial').hide();
                $('#policySearchPartial').hide();
                $('#paymentSearchPartial').hide();
                $("#toolbar").hide();
                break;
        }
    }

    static ViewClaimNumber() {
        $("#radioTemporary").removeAttr('checked');
        $("#inputClaimSearchTemporary").attr('disabled', true);
        $("#inputClaimSearchTemporary").val('');
        $("#inputClaimSearchClaimNumber").attr('disabled', false);
        $("#inputClaimSearchClaimNumber").val('');
        $("#inputClaimSearchClaimNumber").focus();
        $("#radioClaim").prop("checked", "checked");
    }

    static ViewTemporary() {
        $("#radioClaim").removeAttr('checked');
        $("#inputClaimSearchClaimNumber").attr('disabled', true);
        $("#inputClaimSearchClaimNumber").val('');
        $("#inputClaimSearchTemporary").attr('disabled', false);
        $("#inputClaimSearchTemporary").val('');
        $("#inputClaimSearchTemporary").focus();
        $("#radioTemporary").prop("checked", "checked");
    }

    static ViewPaymentRequest() {
        $("#radioTemporaryPayment").removeAttr('checked');
        $("#inputPaymentTemporaryNumber").attr('disabled', true);
        $("#inputPaymentTemporaryNumber").val('');
        $("#inputPaymentRequestNumber").attr('disabled', false);
        $("#inputPaymentRequestNumber").val('');
        $("#inputPaymentRequestNumber").focus();
        $("#radioPaymentRequest").prop("checked", "checked");
    }

    static ViewTemporaryPayment() {
        $("#radioPaymentRequest").removeAttr('checked');
        $("#inputPaymentRequestNumber").attr('disabled', true);
        $("#inputPaymentRequestNumber").val('');
        $("#inputPaymentTemporaryNumber").attr('disabled', false);
        $("#inputPaymentTemporaryNumber").val('');
        $("#inputPaymentTemporaryNumber").focus();
        $("#radioTemporaryPayment").prop("checked", "checked");
    }

    static Search() {
        var searchType = $("#selectSearchType").UifSelect('getSelected');

        switch (parseInt(searchType)) {
            case 1:
                ClaimSearch.SearchNotice();
                break;
            case 2:
                ClaimSearch.SearchClaim();
                break;
            case 3:
                ClaimSearch.SearchPolicy();
                break;
            case 4:
                ClaimSearch.SearchPaymentRequest();
                break;
            case 5:
                ClaimSearch.SearchChargeRequest();
                break;
        }
    }

    static LoadSearchUser(event, user) {
        userId = user.Id;
    }

    static LoadSuretyName(event, surety) {
        $("#modalSureties").UifModal('hide');
        suretyIndividualId = surety.IndividualId;
        $("#inputSuretyName").val(surety.IdentificationDocument + "(" + surety.Bonded + ")");
        $("#inputBidNumber").val(surety.BidNumber);
        $("#inputCourtNumber").val(surety.CourtNum);
    }

    static LoadButtons(event, selectedItem) {
        rowSelected = selectedItem;

        $("#buttonsRescue").show();
        var searchType = $("#selectSearchType").UifSelect('getSelected');
        switch (parseInt(searchType)) {
            case 1:
                $("#btnConvertToClaim").show();
                $("#btnEditNotice").show();
                $("#btnScheduleNotice").show();
                $("#btnObjected").show();
                $("btnSchedule").show();
                $("#btnEditClaim").hide();
                $("#btnPay").hide();
                $("#btnSalvage").hide();
                $("#btnRecovery").hide();
                $("#btnNewNotice").hide();
                $("#btnNewClaim").hide();
                $("#btnEditPay").hide();
                $("#btnCancelPay").hide();
                $("#btnPrint").hide();
                $("#btnSetClaimReserve").hide();
                $("#btnConsulCharge").hide();
                $("#btnCancelCharge").hide();
                break;
            case 2:
                $("#btnEditClaim").show();
                $("#btnSetClaimReserve").show();
                $("#btnPay").show();
                $("#btnSalvage").show();
                $("#btnRecovery").show();

                $("#btnConvertToClaim").hide();
                $("#btnEditNotice").hide();
                $("#btnScheduleNotice").hide();
                $("btnSchedule").hide();
                $("#btnObjected").hide();
                $("#btnNewNotice").hide();
                $("#btnNewClaim").hide();
                $("#btnEditPay").hide();
                $("#btnCancelPay").hide();
                $("#btnPrint").hide();
                $("#btnConsulCharge").hide();
                $("#btnCancelCharge").hide();
                break;
            case 3:
                $("#btnNewNotice").show();
                $("#btnNewClaim").show();

                $("#btnEditClaim").hide();
                $("#btnPay").hide();
                $("#btnSalvage").hide();
                $("#btnRecovery").hide();
                $("#btnConvertToClaim").hide();
                $("#btnEditNotice").hide();
                $("#btnScheduleNotice").hide();
                $("btnSchedule").hide();
                $("#btnObjected").hide();
                $("#btnEditPay").hide();
                $("#btnCancelPay").hide();
                $("#btnPrint").hide();
                $("#btnSetClaimReserve").hide();
                $("#btnConsulCharge").hide();
                $("#btnCancelCharge").hide();
                break;
            case 4:
                $("#btnEditPay").show();
                $("#btnCancelPay").show();
                $("#btnPrint").show();

                $("#btnEditClaim").hide();
                $("#btnPay").hide();
                $("#btnSalvage").hide();
                $("#btnRecovery").hide();
                $("#btnConvertToClaim").hide();
                $("#btnEditNotice").hide();
                $("#btnScheduleNotice").hide();
                $("btnSchedule").hide();
                $("#btnObjected").hide();
                $("#btnNewNotice").hide();
                $("#btnNewClaim").hide();
                $("#btnSetClaimReserve").hide();
                $("#btnConsulCharge").hide();
                $("#btnCancelCharge").hide();
                break;
            case 5:
                $("#btnConsulCharge").show();
                $("#btnCancelCharge").show();

                $("#btnConvertToClaim").hide();
                $("#btnEditNotice").hide();
                $("#btnScheduleNotice").hide();
                $("#btnObjected").hide();
                $("btnSchedule").hide();
                $("#btnEditClaim").hide();
                $("#btnPay").hide();
                $("#btnSalvage").hide();
                $("#btnRecovery").hide();
                $("#btnNewNotice").hide();
                $("#btnNewClaim").hide();
                $("#btnEditPay").hide();
                $("#btnCancelPay").hide();
                $("#btnPrint").hide();
                $("#btnSetClaimReserve").hide();
                break;
            default:
                
                $("#btnEditClaim").hide();
                $("#btnPay").hide();
                $("#btnSalvage").hide();
                $("#btnRecovery").hide();
                $("#btnConvertToClaim").hide();
                $("#btnEditNotice").hide();
                $("#btnScheduleNotice").hide();
                $("btnSchedule").hide();
                $("#btnObjected").hide();
                $("#btnNewNotice").hide();
                $("#btnNewClaim").hide();
                $("#btnEditPay").hide();
                $("#btnCancelPay").hide();
                $("#btnPrint").hide();
                $("#btnSetClaimReserve").hide();
                $("#btnConsulCharge").hide();
                $("#btnCancelCharge").hide();
                break;
        }
    }

    static ViewPrefixOptions(event, prefix) {
        $("#policyVehicleSearchPartial").hide();
        $("#policyLocationSearchPartial").hide();
        $("#policySuretySearchPartial").hide();
        $("#searchClaimTable").hide();
        $("#searchNoticeTable").hide();
        $("#searchPoliciesTable").hide();
        $("#searchPaymentTable").hide();
        $('#insuredSearchName').attr('disabled', true);
        $('#participantSearchName').attr('disabled', true);
        $('#holderSearchName').attr('disabled', true);
        $('#suretySearchName').attr('disabled', true);
        $('#participantSearchName').val('');
        $('#insuredSearchName').val('');
        $('#holderSearchName').val('');
        $('#suretySearchName').val('');
        IndividualId = 0;
        holderId = 0;
        ClaimSearch.CleanPrefix();
        ClaimSearch.SearchOption(prefix.Id);
        ClaimSearch.SearchPersonType(prefix.Id);
    }

    static GetSearchPersonTypeNotice(event, prefix) {
        ClaimSearch.ClearInputClaimPerson();

        if (prefix.Id == "") {
            ClaimSearch.HideDivsPersonSearch();
        } else {
            var searchType = $("#selectSearchType").UifSelect('getSelected');
            ClaimsPaymentRequest.GetSearchPersonType(prefix.Id, searchType).done(function (data) {
                if (data.success) {
                    $("#divNoticePersonSearch").show();
                    $('#ddlNoticeSearchPersonType').UifSelect({ sourceData: data.result });
                } else {
                    ClaimSearch.HideDivsPersonSearch();
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
                }
            });
        }
    }

    static GetSearchPersonType(event, prefix) {
        ClaimSearch.ClearInputClaimPerson();

        if (prefix.Id == "") {
            ClaimSearch.HideDivsPersonSearch();
        } else {
            var searchType = $("#selectSearchType").UifSelect('getSelected');
            ClaimsPaymentRequest.GetSearchPersonType(prefix.Id, searchType).done(function (data) {
                if (data.success) {
                    $("#divClaimPersonSearch").show();
                    $('#ddlClaimSearchPersonType').UifSelect({ sourceData: data.result });
                } else {
                    ClaimSearch.HideDivsPersonSearch();
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
                }
            });
        }
    }
    
    static ClearInputClaimPerson() {
        //Póliza
        $("#insuredSearchName").UifAutoComplete('clean');
        $("#participantSearchName").UifAutoComplete('clean');
        $("#holderSearchName").UifAutoComplete('clean');
        $("#suretySearchName").UifAutoComplete('clean');

        //Denuncia
        $("#insuredClaimSearchName").UifAutoComplete('clean');
        $("#participantClaimSearchName").UifAutoComplete('clean');
        $("#holderClaimSearchName").UifAutoComplete('clean');
        $("#suretyClaimSearchName").UifAutoComplete('clean');

        //Avisos
        $("#insuredNoticeSearchName").UifAutoComplete('clean');
        $("#participantNoticeSearchName").UifAutoComplete('clean');
        $("#holderNoticeSearchName").UifAutoComplete('clean');
        $("#suretyNoticeSearchName").UifAutoComplete('clean');

        IndividualId = 0;
        holderId = 0;
    }

    static HideDivsPersonSearch() {
        ClaimSearch.ClearInputClaimPerson();
        $("#divClaimPersonSearch").hide();
        $("#divNoticePersonSearch").hide();
        $("#divPersonSearch").hide();
    }

    static SearchPersonType(prefixId) {
        if (prefixId == "") {
            ClaimSearch.HideDivsPersonSearch();
            return;
        }           

        var searchType = $("#selectSearchType").UifSelect('getSelected');
        ClaimsPaymentRequest.GetSearchPersonType(prefixId, searchType).done(function (data) {
            if (data.success) {
                $("#divPersonSearch").show();
                $('#ddlSearchPersonType').UifSelect({ sourceData: data.result });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': /*Resources.Language.DontExistBonded*/"TODO: CREAR RECURSO", 'autoclose': true });
                ClaimSearch.ClearInputClaimPerson();
                ClaimSearch.HideDivsPersonSearch();
            }
        });
    }

    static LoadVehicleMake(event, selectedItem) {
        $('#selectVehicleVersion').UifSelect('setSelected', null);
        $('#selectVehicleVersionYear').UifSelect('setSelected', null);

        if (selectedItem.Id != "") {
            ClaimSearch.GetVehicleModelsByVehicleMakeId(selectedItem.Id)
        }
        else {
            $('#selectVehicleModel').UifSelect();
        }
    }

    static LoadVehicleModel(event, selectedItem) {
        var vehicleMakeId = $('#selectVehicleMake').UifSelect('getSelected');
        $('#selectVehicleVersionYear').UifSelect('setSelected', null);

        if (vehicleMakeId != "" && selectedItem.Id != "") {
            ClaimSearch.GetVehicleVersionsByMakeIdModelId(vehicleMakeId, selectedItem.Id)
        }
        else {
            $('#selectVehicleVersion').UifSelect('setSelected', null);
        }
    }

    static LoadVehicleVersion(event, selectedItem) {
        var vehicleMakeId = $('#selectVehicleMake').UifSelect('getSelected');
        var vehicleModelId = $('#selectVehicleModel').UifSelect('getSelected');

        if (vehicleMakeId != "" && vehicleModelId != "" && selectedItem.Id != "") {
            ClaimSearch.GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, selectedItem.Id)
        }
        else {
            $('#selectVehicleVersionYear').UifSelect('setSelected', null);
        }
    }

    static GetVehicleModelsByVehicleMakeId(vehicleMakeId) {
        ClaimSearchRequest.GetVehicleModelsByMakeId(vehicleMakeId).done(function (response) {
            if (response.success) {
                $("#selectVehicleModel").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleVersionsByMakeIdModelId(vehicleMakeId, vehicleModelId) {
        ClaimSearchRequest.GetVehicleVersionsByMakeIdModelId(vehicleMakeId, vehicleModelId).done(function (response) {
            if (response.success) {
                $("#selectVehicleVersion").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId) {
        ClaimSearchRequest.GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId).done(function (response) {
            if (response.success) {
                $("#selectVehicleVersionYear").UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetVehicleVersionYearsByMakeIdModelIdVersionId, 'autoclose': true });
            }
        });
    }

    static GetRiskSuretyByInsuredId() {
        var query = $("#inputSuretyName").val();

        ClaimSearchRequest.GetRiskSuretyByInsuredId(query).done(function (response) {
            if (response.length > 0) {
                $.each(response, function (index, value) {
                    this.BondedFullName = this.IdentificationDocument + "(" + this.Bonded + ")";
                });

                $('#modalSureties').UifModal('showLocal', Resources.Language.Bonded);
                $('#tblSurety').UifDataTable({ sourceData: response });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
            }
        });
    }

    static GetRiskLocationByAddress() {
        var query = $("#inputAddress").val();

        ClaimSearchRequest.GetRiskLocationByAddress(query).done(function (response) {
            if (response.length > 0) {
                $('#modalLocations').UifModal('showLocal', Resources.Language.Property);
                $('#tblLocation').UifDataTable({ sourceData: response });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistsProperties, 'autoclose': true });
            }
        });
    }

    static GetRisksByPlate() {
        var query = $("#inputPlate").val();

        ClaimSearchRequest.GetRisksByPlate(query).done(function (response) {
            if (response.length > 0) {
                $('#modalVehicles').UifModal('showLocal', Resources.Language.Vehicles);
                $("#tblVehicles").UifDataTable({ sourceData: response });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistVehicles, 'autoclose': true });
            }
        });
    }

    static SelectCountry(event, selectedItem) {
        ClaimSearch.GetStatesByCountryId(selectedItem.Id);
    }

    static SelectState(event, selectedItem) {
        var countryId = $('#selectCountryLocation').UifSelect('getSelected');
        var stateId = selectedItem.Id;
        ClaimSearch.GetCitiesByCountryIdStateId(countryId, stateId);
    }

    static SetLocationInformation(event, riskLocation) {
        $("#modalLocations").UifModal('hide');
        riskLocationId = riskLocation.RiskId;
        var countryId = riskLocation.CountryId;
        var stateId = riskLocation.StateId;
        var cityId = riskLocation.CityId;
        $("#selectCountryLocation").UifSelect('setSelected', null);
        $("#selectStateLocation").UifSelect('setSelected', null);
        $("#selectCityLocation").UifSelect('setSelected', null);
        $("#selectCountryLocation").UifSelect("setSelected", countryId);
        ClaimSearchRequest.GetStatesByCountryId(countryId).done(function (response) {
            if (response.success) {
                $("#selectStateLocation").UifSelect({ sourceData: response.result });
                $("#selectStateLocation").UifSelect("setSelected", stateId);
                ClaimSearchRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
                    if (response.success) {
                        $("#selectCityLocation").UifSelect({ sourceData: response.result });
                        $("#selectCityLocation").UifSelect("setSelected", cityId);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotFindCitiesByState, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotFindStatesByCountry, 'autoclose': true });
            }
        });
    }

    static SetVehicleInformation(event, riskVehicle) {
        $("#modalVehicles").UifModal('hide');
        $("#inputPlate").val(riskVehicle.Plate);
        riskVehicleId = riskVehicle.RiskId;
        var vehicleMake = riskVehicle.MakeId;
        var vehicleModel = riskVehicle.ModelId;
        var vehicleVersion = riskVehicle.VersionId;
        var vehicleYear = riskVehicle.Year;
        $("#selectVehicleMake").UifSelect('setSelected', null);
        $("#selectVehicleModel").UifSelect('setSelected', null);
        $("#selectVehicleVersion").UifSelect('setSelected', null);
        $("#selectVehicleVersionYear").UifSelect('setSelected', null);
        $("#selectVehicleMake").UifSelect("setSelected", vehicleMake);
        ClaimSearchRequest.GetVehicleModelsByMakeId(vehicleMake).done(function (response) {
            if (response.success) {
                $("#selectVehicleModel").UifSelect({ sourceData: response.result });
                $("#selectVehicleModel").UifSelect("setSelected", vehicleModel);
                ClaimSearchRequest.GetVehicleVersionsByMakeIdModelId(vehicleMake, vehicleModel).done(function (response) {
                    if (response.success) {
                        $("#selectVehicleVersion").UifSelect({ sourceData: response.result });
                        $("#selectVehicleVersion").UifSelect("setSelected", vehicleVersion);
                        ClaimSearchRequest.GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMake, vehicleModel, vehicleVersion).done(function (response) {
                            if (response.success) {
                                $("#selectVehicleVersionYear").UifSelect({ sourceData: response.result });
                                $("#selectVehicleVersionYear").UifSelect("setSelected", vehicleYear);
                            }
                            else {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotFindYearsByVersion, 'autoclose': true });
                            }
                        });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotFindVersionByModel, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotFindModelsBtMake, 'autoclose': true });
            }
        });
    }

    static LoadBeneficiaryName(event, paymentBeneficiary) {
        $('#inputBeneficiaryName').val(paymentBeneficiary.FullName);
        paymentRequestIndividualId = paymentBeneficiary.IndividualId;
    }

    static LoadBeneficiaryChargeName(event, paymentBeneficiary) {
        $("#inputBeneficiaryChargeName").val(paymentBeneficiary.FullName);
        chargeRequestIndividualId = paymentBeneficiary.IndividualId;
    }

    /* Notice Option Buttons */

    static ToConvertNoticeToClaim() {
        if (rowSelected.ObjectedDescription != null && rowSelected.ObjectedDescription != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotDenounceObjectedNotice, 'autoclose': true });
        }
        else if (rowSelected) {
            if (rowSelected.RiskId == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotConvertUnsafeNoticeToClaim, 'autoclose': true });
                return;
            }

            $('#modalClaimBranch').UifModal('showLocal', Resources.Language.DeductibleNotice);
            noticeToClaim = rowSelected;
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectNoticeClaim, 'autoclose': true });
        }
    }

    static ToEditNotice() {
        if (rowSelected) {
            
            notice = rowSelected;
            
            modelSearchNotice.claimNoticeId = rowSelected.Id;
            modelSearchNotice.coveredRiskTypeId = rowSelected.CoveredRiskTypeId;
            modelSearchNotice.prefixId = rowSelected.PrefixId;
            switch (parseInt(rowSelected.CoveredRiskTypeId)) {
                case parseInt(Resources.Language.CoveredRiskTypeVehicle):
                    router.run("prtNoticeVehicle");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeProperty):
                    router.run("prtNoticeLocation");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeSurety):
                    if (rowSelected.CountryId == 0) {
                        router.run("prtNoticeSurety");
                    }
                    else {
                        router.run("prtNoticeFidelity");
                    }
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeTransport):
                    router.run("prtNoticeTransport");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeAirCraft):
                    router.run("prtNoticeAirCraft");
                    break;
            }

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectNoticeClaim, 'autoclose': true });
        }
    }

    static ScheduleNotice() {        
        var notice = rowSelected;

        $("#subject").val("Aviso de Siniestros Nro. " + notice.Number);
        var contactInformation = {
            name: notice.ContactInformation.split(' - ')[0],
            phone: notice.ContactInformation.split(' - ')[1],
            mail: notice.ContactInformation.split(' - ')[2] !== undefined ? notice.ContactInformation.split(' - ')[2] : ''
        };

        $('#msj').text(Resources.Language.Policy + ': ' + notice.DocumentNumber + '\n' + Resources.Language.FullName + ': ' + contactInformation.name + '\n' + Resources.Language.Phone + ': ' + contactInformation.phone + '\n' + Resources.Language.Mail + ': ' + contactInformation.mail);

        
        $('#modalSchedule').UifModal('showLocal', Resources.Language.AppointmentDate);
    }

    static NoticeObjection() {
        if (rowSelected.ObjectedDescription != "" && rowSelected.ObjectedDescription != null) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ObjectedNotice, 'autoclose': true });
            return;
        }

        if (rowSelected) {
            $('#modalObjected').UifModal('showLocal', Resources.Language.ToObjectClaimNotice);
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectNoticeClaim, 'autoclose': true });
        }
    }

    /* Notice Option Buttons */

    /* Claim Option Buttons */

    static ToEditClaim() {
        if (rowSelected) {
            var prefixId = rowSelected.PrefixId;

            modelSearchCriteria.claimId = rowSelected.ClaimId;
            modelSearchCriteria.claimNumber = rowSelected.Number;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;
            modelSearchCriteria.endorsementId = rowSelected.EndorsementId;

            switch (rowSelected.PrefixId) {
                case 1:
                    glClaimOption = 6;
                    router.run("prtClaimFidelity");
                    break;
                case 2: case 29: case 30: case 31: case 32: case 100:
                    glClaimOption = 3;
                    router.run("prtClaimSurety");
                    break;
                case 3: case 4: case 13: case 15: case 21: case 81: case 82: case 83: case 84: case 85: case 86: case 87: case 88: case 89: case 90: case 91: case 92: case 93: case 94: case 95: case 98:
                    glClaimOption = 2;
                    router.run("prtClaimLocation");
                    break;
                case 5:
                    glClaimOption = 4;
                    router.run("prtClaimTransport");
                    break;
                case 7:
                    glClaimOption = 1;
                    router.run("prtClaimVehicle");
                    break;
                case 10:
                    glClaimOption = 5;
                    router.run("prtClaimAirCraft");
                    break;
            }

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static ToPayClaim() {
        if (rowSelected) {
            modelSearchCriteria.claimNumber = rowSelected.Number;
            modelSearchCriteria.policyDocumentNumber = rowSelected.PolicyDocumentNumber;
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;

            router.run("prtPaymentRequest");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static ToSetClaimReserve() {
        if (rowSelected) {
            modelSearchCriteria.claimNumber = rowSelected.Number;
            modelSearchCriteria.policyDocumentNumber = rowSelected.PolicyDocumentNumber;
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;

            router.run("prtSetClaimReseve");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static ToSalvageClaim() {
        if (rowSelected) {
            $("#buttonsRescue").hide();
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;
            modelSearchCriteria.claimNumber = rowSelected.Number;
            modelSearchCriteria.policyDocumentNumber = rowSelected.PolicyDocumentNumber;

            router.run("prtSalvage");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static ToRecoveryClaim() {
        if (rowSelected) {
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;
            modelSearchCriteria.claimNumber = rowSelected.Number;
            modelSearchCriteria.policyDocumentNumber = rowSelected.PolicyDocumentNumber;

            router.run("prtRecovery");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    /* Claim Option Buttons */

    static ToCreateNotice() {
        if (rowSelected) {
            var prefixId = rowSelected.PrefixId;
            if (rowSelected.RiskId == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RiskDontParameterized, 'autoclose': true });
                return;
            }
            modelSearchNewNotice.riskId = rowSelected.RiskId;
            modelSearchNewNotice.endorsementId = rowSelected.EndorsementId;
            modelSearchNewNotice.prefixId = rowSelected.PrefixId;
            switch (prefixId) {
                case 1:
                    router.run("prtNoticeFidelity");
                    break;
                case 2: case 29: case 30: case 31: case 32: case 100:
                    router.run("prtNoticeSurety");
                    break;
                case 3: case 4: case 13: case 15: case 21: case 81: case 82: case 83: case 84: case 85: case 86: case 87: case 88: case 89: case 90: case 91: case 92: case 93: case 94: case 95: case 98:
                    router.run("prtNoticeLocation");
                    break;
                case 5:
                    router.run("prtNoticeTransport");
                    break;
                case 7:
                    router.run("prtNoticeVehicle");
                    break;
                case 10:
                    router.run("prtNoticeAirCraft");
                    break;
            }

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static ToCreateClaim() {
        if (rowSelected) {
            var prefixId = rowSelected.PrefixId;
            if (rowSelected.RiskId == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RiskDontParameterized, 'autoclose': true });
                return;
            }
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;
            modelSearchCriteria.number = rowSelected.DocumentNumber;

            switch (rowSelected.PrefixId) {
                case 1:
                    router.run("prtClaimFidelity");
                    break;
                case 2: case 29: case 30: case 31: case 32: case 100:
                    router.run("prtClaimSurety");
                    break;
                case 3: case 4: case 13: case 15: case 21: case 81: case 82: case 83: case 84: case 85: case 86: case 87: case 88: case 89: case 90: case 91: case 92: case 93: case 94: case 95: case 98:
                    router.run("prtClaimLocation");
                    break;
                case 5:
                    router.run("prtClaimTransport");
                    break;
                case 7:
                    router.run("prtClaimVehicle");
                    break;
                case 6: case 10:
                    router.run("prtClaimAirCraft");
                    break;
            }

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    /* Claim Option Buttons */

    /* PaymentRequest Option Buttons */
    static ToConsultPaymentRequest() {
        if (rowSelected) {
            modelSearchCriteria.paymentRequestId = rowSelected.Id;

            router.run("prtPaymentRequest");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static ToCancelPaymentRequest() {
        if (rowSelected) {
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;
            modelSearchCriteria.paymentRequestNumber = rowSelected.Number;

            router.run("prtRequestCancellation");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    static CreatePrintPaymentRequest() {
        if (rowSelected) {
            var controller = rootPath + "Claims/PaymentRequest/GetReportPaymentRequestByPaymentRequestId?paymentRequestId=" + rowSelected.Id;
            window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }
    /* PaymentRequest Option Buttons */


    /* ChargeRequest Option Buttons */

    static ToConsultChargeRequest() {

        if (rowSelected) {

            modelSearchCriteria.chargeRequestId = rowSelected.Id;

            router.run("prtCharge");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UiNotify('show', { 'type' : 'danger', 'message' : Resources.Language.SelectRegister, 'autoclose' : true });            
        }
    }

    static ToCancelChargeRequest() {
        if (rowSelected) {
            modelSearchCriteria.branchId = rowSelected.BranchId;
            modelSearchCriteria.prefixId = rowSelected.PrefixId;
            modelSearchCriteria.paymentRequestNumber = rowSelected.Number;

            router.run("prtRequestCancellation");

            ClaimSearch.CleanSearch();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectRegister, 'autoclose': true });
        }
    }

    /* ChargeRequest Option Buttons */

    static ConvertNoticeToClaim() {
        $("#claimBranchForm").validate();

        if ($("#claimBranchForm").valid()) {
            $("#modalClaimBranch").UifModal('hide');
            modelSearchNewClaimByNotice.claimBranch = $("#selectClaimBranchSearch").UifSelect('getSelected');

            switch (noticeToClaim.CoveredRiskTypeId) {
                case parseInt(Resources.Language.CoveredRiskTypeVehicle):
                    glClaimOption = 1;
                    router.run("prtClaimVehicle");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeProperty):
                    glClaimOption = 2;
                    router.run("prtClaimLocation");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeSurety):
                    if (noticeToClaim.CountryId == 0) {
                        glClaimOption = 3;
                        router.run("prtClaimSurety");
                    } else {
                        glClaimOption = 6;
                        router.run("prtClaimFidelity");
                    }
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeTransport):
                    glClaimOption = 4;
                    router.run("prtClaimTransport");
                    break;
                case parseInt(Resources.Language.CoveredRiskTypeAirCraft):
                    glClaimOption = 5;
                    router.run("prtClaimAirCraft");
                    break;
                default:
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CoveredRiskTypeNotParametrized, 'autoclose': true });
                    break;
            }

            ClaimSearch.CleanSearch();
        }
    }

    static UpdateNoticeObjection() {
        var notice = rowSelected;
        notice.ObjectedDescription = $("#ClaimNoticeObject_Description").val();
        if (notice.ObjectedDescription != "") {
            notice.NoticeStateId = 2;
            ClaimSearchRequest.UpdateObjectedClaimNotice(notice).done(function (response) {
                if (response.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.ObjectedNoticeSuccessfully, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }

                $("#modalObjected").UifModal('hide');
            });
        }

    }

    static SaveSchedule() {
        //SendEmail
        var subject = $("#subject").val();
        var message = $("#msj").val();
        var mailDestination = $("#dest").val();
        var startEventDate = $("#dateTimeIni").val();
        var finishEventDate = $("#dateTimeFin").val();        
        var noticeNumber = rowSelected.Number;

        if (!CompareClaimDates($("#dateTimeIni").val(), $("#dateTimeFin").val())) {
            ClaimSearchRequest.SendEmailToAgendNotice(subject, message, mailDestination).done(function (response) {
                if (response.success) {
                    $.UifNotify('show', { 'type': 'success', 'message': response.result, 'autoclose': true });

                    //Agend
                    ClaimSearchRequest.ScheduleNotice(subject, message, startEventDate, finishEventDate).done(function (response) {
                        var blob = new Blob([response], { type: "application/ics" });
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        var fileName = noticeNumber + ".ics";
                        link.download = fileName;
                        link.click();
                        link.remove();
                    });

                    $("#modalSchedule").UifModal('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });            
        }
        else {
            $("#dateTimeIni").val("");
            $("#dateTimeFin").val("");
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.TheFinalDateLessthanInitialDate, 'autoclose': true });
        }
    }

    static ClearSchedule() {
        rowSelected = false;
        $("#dest").val("");
        $("#dateTimeIni").val("");
        $("#dateTimeFin").val("");
        $("#subject").val("");
    }

    /////////////////////////////////////////////////
    // Validar correo
    static ValidateEmail() {
        var mail = $("#dest").val();
        if (!ClaimSearch.isValidEmailAddress(mail)) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.BadMailAdderess, 'autoclose': true });
            //$("#dest").val("");
        }
    }

    static isValidEmailAddress(emailAddress) {
        if (emailAddress != "") {
            var pattern = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return pattern.test(emailAddress);
        } else {
            return true;
        }
    }
}