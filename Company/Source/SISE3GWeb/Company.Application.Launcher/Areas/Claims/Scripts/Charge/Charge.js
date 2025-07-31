var currentEditConceptIndex = 0;
var currentPaymentConceptId = 0;
var rowsConcept = [];
var currentRecoveryOrSalvage = {};
var currentPositionRecoveryOrSalvage = 0;
var payerId = 0;
var IndividualDocumentTypeId = 0;
var chargeRequestsModel = [];
var estimationsTypesId = [];

class Charge extends Uif2.Page {

    getInitialState() {
        Charge.GetMethods();
        $('#inputRegistrationDate').val(GetCurrentFromDate());

        $('#divHolder').hide();
        $('#divInsured').hide();
        $('#divSupplier').hide();
        $("#divThirdparty").hide();

        $("#divRecoveries").hide();
        $("#divSalvages").hide();
        $("#divRecoveriesMirror").hide();
        $("#divSalvagesMirror").hide();
    }

    /** EVENTOS**/
    bindEvents() {
        /* Item Selected */
        $('#selectPaymentSource').on('itemSelected', function () {
            Charge.GetPaymentMovementTypesByPaymentSourceId();
            Charge.UpdateClaimsTable();
            $('#tblClaimsRecoverySalvage').UifDataTable('clear');
        });
        $('#selectCollectTo').on('itemSelected', Charge.ShowDocumentNumberAndFullName);
        $('#selectMovementType').on('itemSelected', Charge.GetPaymentConceptsByBranchIdMovementTypeId);
        $('#selectRecoveryBranch').on('itemSelected', Charge.GetPaymentConceptsByBranchIdMovementTypeId);
        $("#inputDocumentNumberInsured").on('itemSelected', Charge.SetInsuredInformation);
        $("#inputFullNameInsured").on('itemSelected', Charge.SetInsuredInformation);
        $("#inputDocumentNumberSupplier").on('itemSelected', Charge.SetSupplierInformation);
        $("#inputFullNameSupplier").on('itemSelected', Charge.SetSupplierInformation);
        $("#InputDocumentNumberThirdParty").on("keydown", Charge.ClearParticipationInformation);
        $("#InputFullNameThirdParty").on("keydown", Charge.ClearParticipationInformation);
        $("#InputDocumentNumberThirdParty").on('itemSelected', Charge.SetParticipationInformation);
        $("#InputFullNameThirdParty").on('itemSelected', Charge.SetParticipationInformation);
        /* Clicks */
        $('#btnClearChargeRequest').on('click', Charge.ClearCharge);
        $('#btnSearch').on('click', Charge.GetRecoveriesAndSalvageByClaim);
        $('#btnSaveCharge').on('click', Charge.SaveCharge);
        $('#btnPaymentConcept').on('click', Charge.SaveConcepts);

        /* Row Selected */
        $('#tblClaimsRecoverySalvage').on('rowSelected', Charge.FillGridSalvageOrRecovery);
        $('#tblSalvages').on('rowSelected', Charge.OpenBillingInformation);
        $('#tblRecoveries').on('rowSelected', Charge.OpenBillingInformation);
        $('#tblPaymentConcept').on('rowSelected', Charge.CreateConcepts);
        $('#tblPaymentConcept').on('rowDeselected', Charge.DeselectConcept);

        /* Only Numbers */
        $("#inputConceptValue").OnlyDecimals(2);
        $('#inputDocumentNumberInsured').on('keypress', Charge.OnlyNumbers);
        $('#inputDocumentNumberSupplier').on('keypress', Charge.OnlyNumbers);
        $('#inputClaimNum').ValidatorKey(ValidatorType.Number, 2, 1);
        $("#inputConceptValue").ValidatorKey(ValidatorType.Decimal, 2, 1);
        $("#InputDocumentNumberThirdParty").on('keypress', Charge.OnlyNumbers);
        $("#inputPolicyDocumentNumber").ValidatorKey(ValidatorType.Number, 2, 1);

        /* Row Edit */
        $('#tblSingleBillingInformation').on('rowEdit', Charge.EditConcept);

        /* Row Delete */
        $('#tblSingleBillingInformation').on('rowDelete', Charge.DeleteConcept);

        /* Focus */
        $("#inputConceptValue").focusin(Charge.NotFormatMoneyIn);
        $("#inputConceptValue").focusout(Charge.FormatMoneyOut);

        /* Save */
        $('#editAction').on('Save', Charge.AddConcepts);

        // Ventana Modal
        $('#modalSingleBillingInformation').on('closed.modal', function () {
            Charge.CloseConcepts();
        });

    }

    /** METODOS**/
    static GetMethods() {
        if (modelSearchCriteria.chargeRequestId == null) {
            Charge.GetPaymentSourcesByChargeRequest();
            Charge.GetBranches();
            Charge.GetMovementDate();
            Charge.GetPersonTypes();
            Charge.GetPrefixes();            
        }
        Charge.DoConsultChargeRequest();
    }

    static GetPaymentSourcesByChargeRequest(callback) {
        ChargeRequest.GetPaymentSourcesByChargeRequest().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                $('#selectPaymentSource').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetPaymentSourcesByChargeRequest, 'autoclose': true });
            }
        });
    }

    static GetPaymentMovementTypesByPaymentSourceId(callback) {
        var sourceId = $("#selectPaymentSource").UifSelect("getSelected");
        if (sourceId) {
            ChargeRequest.GetPaymentMovementTypesByPaymentSourceId(sourceId).done(function (data) {
                if (data.success) {
                    if (callback)
                        return callback(data.result);

                    $('#selectMovementType').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetPaymentMovementTypesByPaymentSourceId, 'autoclose': true });
                }
            });
        }
        else {
            $('#selectMovementType').UifSelect("disabled", true);
        }
    }

    static GetBranches(callback) {
        ChargeRequest.GetBranches().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                $('#selectRecoveryBranch').UifSelect({ sourceData: data.result });
                $('#selectBranch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.GetBranches, 'autoclose': true });
            }
        });
    }

    static GetMovementDate() {
        ChargeRequest.GetModuleDateByModuleTypeMovementDate().done(function (data) {
            if (data.success) {
                $('#inputAccountingDate').val(FormatDate(data.result));
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGetMovementDate, 'autoclose': true });
            }
        });
    }

    static GetPersonTypes(callback) {
        ChargeRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                $('#selectCollectTo').UifSelect({ sourceData: data.result });
            }
        });
    }

    static GetPrefixes(callback) {
        ChargeRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                $('#selectPrefixesOfClaims').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetPrefixes, 'autoclose': true });
            }
        });
    }

    /* Muestra los campos DocumentNumber y FullName según la selección actual (Proveedor o Asegurado) */
    static ShowDocumentNumberAndFullName() {
        switch ($("#selectCollectTo").UifSelect('getSelectedText')) {
            case "PROVEEDOR": //Proveedor
                $('#divInsured').hide();
                $('#divThirdparty').hide();
                $('#divHolder').hide();

                $('#divSupplier').show();
                $('#inputDocumentNumberSupplier').UifAutoComplete('clean');
                $('#inputFullNameSupplier').UifAutoComplete('clean');
                break;
            case "ASEGURADO": //Asegurado
                $('#divSupplier').hide();
                $('#divThirdparty').hide();
                $('#divHolder').hide();

                $('#divInsured').show();
                $('#inputDocumentNumberInsured').UifAutoComplete('clean');
                $('#inputFullNameInsured').UifAutoComplete('clean');
                break;
            case "TOMADOR": //Tomador
                $('#divSupplier').hide();
                $('#divThirdparty').hide();
                $('#divInsured').hide();

                $('#divHolder').show();
                $('#inputDocumentNumberHolder').UifAutoComplete('clean');
                $('#inputFullNameHolder').UifAutoComplete('clean');

                Charge.ValidateSalvagesOrRecoveries();
                break;
            case 'TERCERO': //tercero
                $('#divSupplier').hide();
                $('#divInsured').hide();
                $('#divHolder').hide();

                $('#divThirdparty').show();
                $('#InputDocumentNumberThirdParty').UifAutoComplete('clean');
                $('#InputFullNameThirdParty').UifAutoComplete('clean');
                break;
            default:
                $('#divInsured').hide();
                $('#divSupplier').hide();
                $('#divThirdparty').hide();
                $('#divHolder').hide();
        }

        payerId = 0;
    }

    static ValidateSalvagesOrRecoveries() {
        var salvagesOrRecoveries = $("#tblClaimsRecoverySalvage").UifDataTable('getData');
        if (salvagesOrRecoveries.length > 0) {
            Charge.SetHolderAutomatically(salvagesOrRecoveries[0].PolicyHolderId);
        }
    }

    static SetHolderAutomatically(individualId) {
        ChargeRequest.GetHolderByIndividualId(individualId).done(function (response) {
            if (response.success) {
                IndividualDocumentTypeId = response.result[0].DocumentTypeId;
                $("#inputDocumentNumberHolder").UifAutoComplete("setValue", response.result[0].DocumentNumber);
                $("#inputFullNameHolder").UifAutoComplete("setValue", response.result[0].FullName);
                $("#inputDocumentNumberHolder").UifAutoComplete('disabled', true);
                $("#inputFullNameHolder").UifAutoComplete('disabled', true);
                payerId = response.result[0].IndividualId;
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetHoldersByName, 'autoclose': true });
            }
        });
    }

    /* Limpia los campos del formulario Charge */
    static ClearCharge() {

        $('#selectPaymentSource').UifSelect('setSelected', null);
        $('#selectMovementType').UifSelect('setSelected', null);
        $('#selectMovementType').UifSelect("disabled", true);
        $('#selectRecoveryBranch').UifSelect('setSelected', null);
        $('#selectCollectTo').UifSelect('setSelected', null);
        $('#inputDocumentNumberInsured').val("");
        $('#inputFullNameInsured').val("");
        $('#inputDocumentNumberSupplier').val("");
        $('#inputFullNameSupplier').val("");
        Charge.ShowDocumentNumberAndFullName();
        $('#selectBranch').UifSelect('setSelected', null);
        $('#selectPrefixesOfClaims').UifSelect('setSelected', null);
        $('#inputClaimNum').val("");
        $('#InputDocumentNumberThirdParty').val("");
        $('#InputFullNameThirdParty').val("");
        $("#inputPolicyDocumentNumber").val("");

        Charge.UpdateClaimsTable();
        Charge.CloseConcepts();

        chargeRequestsModel = [];
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if ($(this).val() != '')
            $(this).val(FormatMoney($(this).val().includes(',') ? $(this).val().replace(',', '.') : $(this).val()));
    }

    static ClearConceptsTables() {
        $("").UifDataTable("unselect");
        $("").UifDataTable("clear");
    }

    /* Establece la visualización de las tablas de Reclamos o Salvamentos según el origen de pago seleccionado */
    static UpdateClaimsTable() {
        var paymentSourceId = $("#selectPaymentSource").UifSelect('getSelected');
        $("#divSalvages").hide();
        $("#divRecoveries").hide();
        $("#tblRecoveries").UifDataTable('clear');
        $("#tblSalvages").UifDataTable('clear');
        $("#tblClaimsRecoverySalvage").UifDataTable('clear');

        switch (parseInt(paymentSourceId)) {
            case 2: //Salvamento
                $("#RecoveryOrSalvage").text(Resources.Language.Salvage);
                $("#divSalvages").show();
                $("#divRecoveries").hide();
                break;
            case 3: //Recobro
                $("#RecoveryOrSalvage").text(Resources.Language.Recovery);
                $("#divSalvages").hide();
                $("#divRecoveries").show();
                break;
            default:
                $("#RecoveryOrSalvage").text(Resources.Language.Recovery);
                $("#divSalvages").hide();
                $("#divRecoveries").hide();
                break;
        }
    }

    /* Carga la información del asegurado después de seleccionar una opción del autocomplete */
    static SetInsuredInformation(event, insured) {
        $('#inputDocumentNumberInsured').UifAutoComplete('setValue', insured.DocumentNumber);
        $('#inputFullNameInsured').UifAutoComplete('setValue', insured.FullName);
        IndividualDocumentTypeId = insured.DocumentTypeId;
        payerId = insured.IndividualId;
    }

    /* Carga la información del proveedor después de seleccionar una opción del autocomplete */
    static SetSupplierInformation(event, supplier) {
        $('#inputDocumentNumberSupplier').UifAutoComplete('setValue', supplier.DocumentNumber);
        $('#inputFullNameSupplier').UifAutoComplete('setValue', supplier.Name);
        IndividualDocumentTypeId = supplier.DocumentTypeId;
        payerId = supplier.IndividualId;
    }

    static SetParticipationInformation(event, participant) {
        $('#InputDocumentNumberThirdParty').UifAutoComplete('setValue', participant.DocumentNumber);
        $('#InputFullNameThirdParty').UifAutoComplete('setValue', participant.Name);
        IndividualDocumentTypeId = participant.DocumentTypeId;
        payerId = participant.IndividualId;
    }

    static ClearParticipationInformation() {
        if ($('#InputDocumentNumberThirdParty').val() != "") {
            payerId = 0;
        }
    }

    /** Obtiene los recobros y salvamentos */
    static GetRecoveriesAndSalvageByClaim(callback) {
        $("#tblSalvages").UifDataTable('clear');
        $("#tblRecoveries").UifDataTable('clear');

        var paymentSourceId = $("#selectPaymentSource").UifSelect('getSelected');
        var policyDocumentNumber = $("#inputPolicyDocumentNumber").val();
        var claimNumber = $("#inputClaimNum").val();
        var collectTo = $("#selectCollectTo").UifSelect('getSelectedText');      
        var estimationIsValid = false;         
        chargeRequestsModel = [];
        $('#tblClaimsRecoverySalvage').UifDataTable('clear');

        switch (parseInt(paymentSourceId)) {
            case 2:
                ChargeRequest.GetSalvagesByClaim($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixesOfClaims").UifSelect("getSelected"), policyDocumentNumber, $("#inputClaimNum").val()).done(function (data) {
                    if (data.success) {
                        if (data.result.length > 0) {
                            if (typeof callback === "function")
                                return callback(data.result);

                            $("#selectBranch").UifSelect('setSelected', data.result[0].BranchId);
                            $("#selectPrefixesOfClaims").UifSelect('setSelected', data.result[0].PrefixId);

                            ClaimsPaymentRequest.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(data.result[0].PrefixId, data.result[0].BranchId, policyDocumentNumber, claimNumber).done(function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        $.each(estimationsTypesId, function (index, value) {
                                            if (response.result.some(x => x.EstimationTypeId == value)) {
                                                estimationIsValid = true;
                                                return false;
                                            }
                                        });

                                        if (estimationIsValid) {
                                            $('#tblClaimsRecoverySalvage').UifDataTable({ sourceData: data.result, selectionType: 'multiple' });
                                            if (collectTo == 'TOMADOR') {
                                                Charge.SetHolderAutomatically(data.result[0].PolicyHolderId);
                                            }
                                        }
                                        else {
                                            $.UifNotify('show', { 'type': 'danger', 'message': String.format(Resources.Language.CannotPayChargeWithoutAReserve, $("#selectMovementType").UifSelect("getSelectedText")), 'autoclose': true });                                            
                                        }

                                    }
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ClaimFindDontExist, 'autoclose': true });
                                }
                            });                

                        }
                        else {
                            $('#tblClaimsRecoverySalvage').UifDataTable('clear');
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ClaimDoNotHaveSalvages, 'autoclose': true });
                        }
                    } else {
                        $('#tblClaimsRecoverySalvage').UifDataTable('clear');
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
                break;
            case 3:
                ChargeRequest.GetRecoveriesByClaim($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixesOfClaims").UifSelect("getSelected"), policyDocumentNumber, $("#inputClaimNum").val()).done(function (data) {
                    if (data.success) {
                        if (data.result.length > 0) {
                            if (typeof callback === "function")
                                return callback(data.result);

                            $("#selectBranch").UifSelect('setSelected', data.result[0].BranchId);
                            $("#selectPrefixesOfClaims").UifSelect('setSelected', data.result[0].PrefixId);

                            ClaimsPaymentRequest.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(data.result[0].PrefixId, data.result[0].BranchId, policyDocumentNumber, claimNumber).done(function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        $.each(estimationsTypesId, function (index, value) {
                                            if (response.result.some(x => x.EstimationTypeId == value)) {
                                                estimationIsValid = true;
                                                return false;
                                            }
                                        });

                                        if (estimationIsValid) {
                                            $('#tblClaimsRecoverySalvage').UifDataTable({ sourceData: data.result });
                                            if (collectTo == 'TOMADOR') {
                                                Charge.SetHolderAutomatically(data.result[0].PolicyHolderId);
                                            }
                                        }
                                        else {
                                            $.UifNotify('show', { 'type': 'danger', 'message': String.format(Resources.Language.CannotPayChargeWithoutAReserve, $("#selectMovementType").UifSelect("getSelectedText")), 'autoclose': true });
                                        }

                                    }
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ClaimFindDontExist, 'autoclose': true });
                                }
                            });

                        }
                        else {
                            $('#tblClaimsRecoverySalvage').UifDataTable('clear');
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ClaimDoNotHaveRecoveries, 'autoclose': true });
                        }
                    } else {
                        $('#tblClaimsRecoverySalvage').UifDataTable('clear');
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });

                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetRecoveriesAndSalvageByClaim, 'autoclose': true });
                break;
        }
    }

    /* Guarda el Charge */
    static SaveCharge() {
        $("#formCharge").validate();
        if ($("#formCharge").valid()) {
            if (Charge.ValidateChargeRequests(chargeRequestsModel)) {
                if (payerId != 0) {
                    ChargeRequest.SaveChargeRequest(chargeRequestsModel).done(function (data) {
                        if (data.success) {

                            var successMessage = "";
                            var countChargeCreated = 0;
                            var countChargePolicies = 0;

                            $.each(data.result, function (index, value) {
                                
                                LaunchPolicies.ValidateInfringementPolicies(this.AuthorizationPolicies, true);
                                var countAutorizationPolicies = this.AuthorizationPolicies.filter(x => x.Type == TypeAuthorizationPolicies.Authorization || x.Type == TypeAuthorizationPolicies.Restrictive).length;

                                if (countAutorizationPolicies > 0) {
                                    LaunchPolicies.RenderViewAuthorizationPolicies(this.AuthorizationPolicies, this.TemporalId, FunctionType.ChargeRequest);
                                    countChargePolicies++;
                                }
                                else {
                                    if (countChargeCreated == 0) {
                                        successMessage += String.format("[{0}]", this.Number);
                                    }
                                    else {
                                        successMessage += String.format(", [{0}]", this.Number);
                                    }
                                    
                                    countChargeCreated++;
                                }
                            });

                            if (countChargeCreated > 0) {

                                successMessage = String.format(Resources.Language.MessageRecoveryRequest, successMessage);

                                $.UifDialog('alert', {
                                    message: successMessage
                                }, function (result) {
                                    if (countChargePolicies == 0) {
                                        Charge.ClearCharge();
                                    }
                                });
                            }
                            
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectedCompanyPersonBasic, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSaveChargeVwAll, 'autoclose': true });
            }
        }
    }

    /* LLena el grid de salvamentos o recobros según el origen de pago actual */
    static FillGridSalvageOrRecovery(event, data, position) {
        $("#formCharge").validate();
        if ($("#formCharge").valid()) {
            switch (parseInt($("#selectPaymentSource").UifSelect('getSelected'))) {
                case 2:
                    //Si es un salvamento
                    Charge.FillGridSalvageBySalvageId(data.Id);
                    break;
                case 3:
                    //Si es un recobro
                    Charge.FillGridRecoveryByRecoveryId(data.Id);
                    break;
            }
        }
        else {
            $("#tblClaimsRecoverySalvage").UifDataTable('unselect');
            ScrollTop();
        }
    }

    /* Llena la tabla de pagos de salvamentos a partir del salvamento seleccionado */
    static FillGridSalvageBySalvageId(salvageId) {
        //Si no se ha agregado aún el salvamento lo agrega a la tabla
        if (Charge.ValidateAddSalvageBySalvageId(salvageId)) {
            ChargeRequest.GetSalvageBySalvageId(salvageId).done(function (data) {
                if (data.success) {
                    $.each([data.result], function (index, value) {
                        this.CreationDate = FormatDate(this.CreationDate);
                        this.EndDate = FormatDate(this.EndDate);
                        this.AssignmentDate = FormatDate(this.AssignmentDate);
                        this.RemainingAmount = (this.EstimatedSale - this.RecoveryAmount);
                    });

                    $("#tblSalvages").UifDataTable('addRow', data.result);
                    Charge.CreateChargeRequestModel();
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }

            });
        }
        else {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.SalvageAlreadyAdded, 'autoclose': true });
        }
    }

    /* Valida que no se haya agregado anteriormente el salvamento seleccionado */
    static ValidateAddSalvageBySalvageId(salvageId) {
        var data = $("#tblSalvages").UifDataTable('getData');
        return !data.some(x => x.Id == salvageId);
    }

    /* Llena la tabla de pagos de recobros a partir del recobro seleccionado */
    static FillGridRecoveryByRecoveryId(recoveryId) {
        //Si no se ha agregado aún el salvamento lo agrega a la tabla
        if (Charge.ValidateAddRecoveryByRecoveryId(recoveryId)) {
            ChargeRequest.GetRecoveryByRecoveryId(recoveryId).done(function (data) {
                if (data.success) {
                    $.each([data.result], function (index, value) {
                        this.CreatedDate = FormatDate(this.CreatedDate);
                        this.TotalAmount += this.TotalAmount * (this.TaxPercentage / 100);
                        this.RemainingAmount = (this.TotalAmount - this.RecoveryAmount);
                    });

                    $("#tblRecoveries").UifDataTable('addRow', data.result);
                    Charge.CreateChargeRequestModel();
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.RecoveryAlreadyAdded, 'autoclose': true });
        }
    }

    /* Valida que no se haya agregado anteriormente el recobro seleccionado */
    static ValidateAddRecoveryByRecoveryId(recoveryId) {
        var data = $("#tblRecoveries").UifDataTable('getData');
        return !data.some(x => x.Id == recoveryId);
    }

    /* Permite el ingreso de carateres únicamente numéricos */
    static OnlyNumbers(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            }
        }
    }

    /* Muestra la información de facturación con los conceptos de pago en una modal */
    static OpenBillingInformation(event, data, position) {
        var index = 0;
        switch (parseInt($("#selectPaymentSource").UifSelect('getSelected'))) {
            case 2:
                //Si es un salvamento
                currentRecoveryOrSalvage.Id = data.Id;
                currentRecoveryOrSalvage.Description = data.Description;
                currentRecoveryOrSalvage.EstimatedSale = data.EstimatedSale;
                currentRecoveryOrSalvage.CreationDate = data.CreationDate;
                currentRecoveryOrSalvage.Location = data.Location;
                currentRecoveryOrSalvage.TotalAmount = data.TotalAmount;
                currentRecoveryOrSalvage.UnitsQuantity = data.UnitsQuantity;
                currentRecoveryOrSalvage.ClaimId = data.ClaimId;
                currentRecoveryOrSalvage.SubClaimId = data.SubClaimId;
                currentRecoveryOrSalvage.EstimationTypeId = data.EstimationTypeId;
                currentRecoveryOrSalvage.ClaimNumber = data.ClaimNumber;
                currentRecoveryOrSalvage.EstimationValue = data.EstimatedSale;
                currentRecoveryOrSalvage.BranchCode = data.BranchId;
                currentRecoveryOrSalvage.PaymentValue = data.PaymentValue;
                currentRecoveryOrSalvage.TotalAmount = data.EstimatedSale;
                currentRecoveryOrSalvage.RemainingAmount = data.RemainingAmount;

                index = chargeRequestsModel.findIndex(x => x.SalvageId == data.Id);

                currentPositionRecoveryOrSalvage = position;
                $('#modalSingleBillingInformation').UifModal('showLocal', Resources.Language.Salvage);
                break;
            case 3:
                //Si es un recobro
                currentRecoveryOrSalvage.Id = data.Id;
                currentRecoveryOrSalvage.Description = data.Description;
                currentRecoveryOrSalvage.CreationDate = data.CreatedDate;
                currentRecoveryOrSalvage.TotalAmount = data.TotalAmount;

                currentRecoveryOrSalvage.ClaimId = data.ClaimId;
                currentRecoveryOrSalvage.SubClaimId = data.SubClaimId;
                currentRecoveryOrSalvage.EstimationTypeId = data.EstimationTypeId;
                currentRecoveryOrSalvage.ClaimNumber = data.ClaimNumber;
                currentRecoveryOrSalvage.EstimationValue = data.TotalAmount;
                currentRecoveryOrSalvage.BranchCode = data.BranchId;
                currentRecoveryOrSalvage.PaymentValue = data.PaymentValue;
                currentRecoveryOrSalvage.RemainingAmount = data.RemainingAmount;

                index = chargeRequestsModel.findIndex(x => x.RecoveryId == data.Id);

                currentPositionRecoveryOrSalvage = position;
                $('#modalSingleBillingInformation').UifModal('showLocal', Resources.Language.Recoveries);
                break;
        }

        if (modelSearchCriteria.chargeRequestId == null) {
            rowsConcept = chargeRequestsModel[index].Voucher.Concepts;

            if (rowsConcept.length > 0) {
                var value =
                {
                    label: 'Id',
                    values: rowsConcept.map(x => x.PaymentConceptId)
                }

                $("#tblPaymentConcept").UifDataTable("setSelect", value);
            }           

            $("#tblSingleBillingInformation").UifDataTable({ sourceData: rowsConcept, edit: true, delete: true, selectionType: 'none' }); 
        }       
    }

    static GetPaymentConceptsByBranchIdMovementTypeId(callback) {
        $("#tblPaymentConcept").UifDataTable('clear');

        if ($('#selectRecoveryBranch').UifSelect("getSelected") && $('#selectMovementType').UifSelect("getSelected")) {
            ChargeRequest.GetPaymentConceptsByBranchIdMovementTypeId(parseInt($('#selectRecoveryBranch').UifSelect('getSelected')), parseInt($('#selectMovementType').UifSelect('getSelected'))).done(function (data) {
                if (typeof callback === "function")
                    return callback(data.result);

                $("#tblPaymentConcept").UifDataTable({ sourceData: data.result, selectionType: 'multiple' });
                $("#tblPaymentConcept").find('.selectall-button').hide();
            });
        }

        ChargeRequest.GetEstimationsTypesIdByMovementTypeId($("#selectMovementType").UifSelect("getSelected")).done(function (response) {
            if (response.success) {
                if (response.result.length > 0) {
                    estimationsTypesId = response.result;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

        Charge.UpdateClaimsTable();
        $('#tblClaimsRecoverySalvage').UifDataTable('clear');
    }


    /* Crea el concepto */
    static CreateConcepts(event, data, position) {
        var rowConcept = {};
        rowConcept.PaymentConceptId = data.Id;
        rowConcept.PaymentConcept = data.Description;
        rowConcept.Value = 0;
        rowConcept.TaxValue = 0;

        //Agrega un nuevo concepto de pago
        Charge.ValidateToAddPaymentConcept(rowConcept);
    }

    /**
     * Valida la deselección de un concepto
     * @param {any} event
     * @param {any} data
     */
    static DeselectConcept(event, data) {
        if ($("#tblSingleBillingInformation").UifDataTable('getData').some(x => x.PaymentConceptId == data.Id)) {
            var value = {
                label: 'Id',
                values: [data.Id]
            };
            $("#tblPaymentConcept").UifDataTable('setSelect', value);
        }
    }

    /* Control para que no se agregue un concepto duplicado, al Grid de Conceptos de Pago */
    static ValidateToAddPaymentConcept(rowConcept) {
        var data = $("#tblSingleBillingInformation").UifDataTable('getData');

        if (data.length == 1) {

            var value = {
                label: 'Id',
                values: [rowConcept.PaymentConceptId]
            };

            $("#tblPaymentConcept").UifDataTable('setUnselect', value);
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotIncludeMoreThanOneConcepts, 'autoclose': true });
        }
        else {
            if (data.some(x => x.PaymentConceptId == rowConcept.PaymentConceptId)) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.PaymentConceptAlreadySet, 'autoclose': true })
            }
            else {
                $('#tblSingleBillingInformation').UifDataTable('addRow', rowConcept);
                rowsConcept.push(rowConcept);
            }
        }
    }

    static ValidateChargeRequests(chargeRequests) {
        return chargeRequests.length > 0 && chargeRequests.every(x => x.Voucher.Concepts != undefined && x.Voucher.Concepts.length > 0 && x.Voucher.Concepts != null);
    }

    /* Edita un concepto */
    static EditConcept(event, data, position) {
        currentEditConceptIndex = position;
        currentPaymentConceptId = data.PaymentConceptId;
        $('#editAction').UifInline('show');
        Charge.bindConcept(data);
    }

    /* Carga los datos del concepto en edición */
    static bindConcept(vm) {
        $("#editForm").find("#inputPaymentConceptId").val(vm.PaymentConceptId);
        $("#editForm").find("#inputPaymentConcept").val(vm.PaymentConcept);
        $("#editForm").find("#inputConceptValue").val(FormatMoney(vm.Value));
        $("#editForm").find("#inputTaxValue").val(FormatMoney(vm.TaxValue));
    }

    /* Guarda el concepto */
    static AddConcepts() {
        var index = rowsConcept.findIndex(x => x.PaymentConceptId === currentPaymentConceptId);

        rowsConcept[index].PaymentConceptId = $("#editForm").find("#inputPaymentConceptId").val();
        rowsConcept[index].PaymentConcept = $("#editForm").find("#inputPaymentConcept").val();
        rowsConcept[index].Value = parseFloat(NotFormatMoney($("#editForm").find("#inputConceptValue").val()).replace(',', '.'));

        var rowModel = {};
        rowModel.PaymentConceptId = rowsConcept[index].PaymentConceptId;
        rowModel.PaymentConcept = rowsConcept[index].PaymentConcept;
        rowModel.Value = rowsConcept[index].Value;
        rowModel.TaxValue = 0;

        $('#tblSingleBillingInformation').UifDataTable('editRow', rowModel, currentEditConceptIndex);
        $("#editForm").formReset();
        $('#editAction').UifInline('hide');
    }

    /* Limpia los conceptos */
    static CloseConcepts() {
        $('#modalSingleBillingInformationDialog').UifModal('hide');
        $("#tblSalvages").UifDataTable('unselect');
        $("#tblRecoveries").UifDataTable('unselect');

        if (modelSearchCriteria.chargeRequestId == null) {
            if (rowsConcept.length > 0) {
                rowsConcept = [];
                $('#tblSingleBillingInformation').UifDataTable('clear');
                $("#tblBillingInformation").UifDataTable('unselect');
                $("#tblPaymentConcept").UifDataTable('unselect');
            }
            else {
                $("#tblClaimsRecoverySalvage").UifDataTable('unselect');
            }
        }
    }

    /* Calcula el concepto total y actualiza el registro en la tabla */
    static SaveConcepts() {
        if (rowsConcept.length > 0) {
            var totalConcept = 0;

            //Sumatoria
            for (var i = 0; i < rowsConcept.length; i++) {
                totalConcept += parseFloat(rowsConcept[i].Value);
            }

            if (totalConcept > currentRecoveryOrSalvage.RemainingAmount) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.WarningSaveConcepts, 'autoclose': true });
            }
            else {
                switch (parseInt($("#selectPaymentSource").UifSelect('getSelected'))) {
                    case 2:
                        //Si es un salvamento
                        var selected = $("#tblSalvages").UifDataTable('getSelected')[0];
                        selected.TotalConcept = totalConcept;
                        var index = chargeRequestsModel.findIndex(x => x.SalvageId == selected.Id);
                        chargeRequestsModel[index].Voucher.Concepts = rowsConcept;
                        $("#tblSalvages").UifDataTable('unselect');
                        $('#tblSalvages').UifDataTable('editRow', selected, currentPositionRecoveryOrSalvage);
                        $('#modalSingleBillingInformation').UifModal('hide');

                        break;
                    case 3:
                        //Si es un recobro
                        var selected = $("#tblRecoveries").UifDataTable('getSelected')[0];
                        selected.TotalConcept = totalConcept;
                        var index = chargeRequestsModel.findIndex(x => x.RecoveryId == selected.Id);
                        chargeRequestsModel[index].Voucher.Concepts = rowsConcept;
                        $("#tblRecoveries").UifDataTable('unselect');
                        $('#tblRecoveries').UifDataTable('editRow', selected, currentPositionRecoveryOrSalvage);
                        $('#modalSingleBillingInformation').UifModal('hide');
                        break;
                }

                Charge.CloseConcepts();
            }
        }
        else {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.ErrorSaveConcepts, 'autoclose': true })
        }
    }

    /* Elimina un concepto de pago */
    static DeleteConcept(event, data, position) {
        var index = rowsConcept.findIndex(x => x.PaymentConceptId === data.PaymentConceptId);
        rowsConcept.splice(index, 1);

        var value = {
            label: 'Id',
            values: [data.PaymentConceptId]
        };

        $("#tblPaymentConcept").UifDataTable('setUnselect', value);

        $('#tblSingleBillingInformation').UifDataTable('deleteRow', position);
    }

    /* Quita la selección de los registros */
    static ClearItemSelectRecoveyOrSalvage() {
        switch (parseInt($("#selectPaymentSource").UifSelect('getSelected'))) {
            case 2:
                //Si es un salvamento
                $("#tblSalvages").UifDataTable('unselect');
                break;
            case 3:
                //Si es un recobro
                $("#tblRecoveries").UifDataTable('unselect');
                break;
        }
    }

    /* Crea el modelo de la solicitud de cobro */
    static CreateChargeRequestModel() {
        switch (parseInt($("#selectPaymentSource").UifSelect('getSelected'))) {
            case 2:
                //Si es un salvamento
                var salvagesToCharge = $("#tblSalvages").UifDataTable('getData');
                $.each(salvagesToCharge, function (index, value) {
                    if (!chargeRequestsModel.some(x => x.SalvageId == this.Id)) {
                        var chargeRequest = {};
                        chargeRequest.Id = 0;
                        chargeRequest.Number = 0;
                        chargeRequest.PolicyDocumentNumber = $("#inputPolicyDocumentNumber").val();
                        chargeRequest.RegistrationDate = $("#inputRegistrationDate").val();
                        chargeRequest.EstimatedDate = $("#inputRegistrationDate").val();
                        chargeRequest.AccountingDate = $("#inputAccountingDate").val();
                        chargeRequest.PrefixId = $("#selectPrefixesOfClaims").UifSelect("getSelected");
                        chargeRequest.MovementTypeId = $("#selectMovementType").UifSelect("getSelected");
                        chargeRequest.IsPrinted = false;
                        chargeRequest.IndividualId = payerId;
                        chargeRequest.PersonTypeId = $("#selectCollectTo").UifSelect("getSelected");
                        chargeRequest.BranchId = $("#selectRecoveryBranch").UifSelect("getSelected");
                        chargeRequest.PaymentRequestTypeId = 2;
                        chargeRequest.PaymentSourceId = $("#selectPaymentSource").UifSelect("getSelected");
                        chargeRequest.ClaimId = this.ClaimId;
                        chargeRequest.SubClaim = this.SubClaimId;
                        chargeRequest.ClaimBranchId = this.BranchId;
                        chargeRequest.ClaimNumber = this.ClaimNumber;
                        chargeRequest.SalvageId = this.Id;
                        chargeRequest.Voucher = Charge.CreateVoucer();
                        chargeRequest.BeneficiaryDocumentType = IndividualDocumentTypeId;

                        switch ($("#selectCollectTo").UifSelect("getSelectedText")) {
                            case 'PROVEEDOR':
                                chargeRequest.BeneficiaryDocumentNumber = $("#inputDocumentNumberSupplier").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#inputFullNameSupplier").UifAutoComplete('getValue');
                                break;
                            case 'ASEGURADO':
                                chargeRequest.BeneficiaryDocumentNumber = $("#inputDocumentNumberInsured").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#inputFullNameInsured").UifAutoComplete('getValue');
                                break;
                            case 'TERCERO':
                                chargeRequest.BeneficiaryDocumentNumber = $("#InputDocumentNumberThirdParty").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#InputFullNameThirdParty").UifAutoComplete('getValue');
                                break;
                            case 'TOMADOR':
                                chargeRequest.BeneficiaryDocumentNumber = $("#inputDocumentNumberHolder").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#inputFullNameHolder").UifAutoComplete('getValue');
                                break;
                        }

                        chargeRequestsModel.push(chargeRequest);
                    }
                });
                break;
            case 3:
                //Si es un recobro
                var recoveriesToCharge = $("#tblRecoveries").UifDataTable('getData');

                $.each(recoveriesToCharge, function (index, value) {
                    if (!chargeRequestsModel.some(x => x.RecoveryId == this.Id)) {
                        var chargeRequest = {};
                        chargeRequest.Id = 0;
                        chargeRequest.Number = 0;
                        chargeRequest.PolicyDocumentNumber = $("#inputPolicyDocumentNumber").val();
                        chargeRequest.RegistrationDate = $("#inputRegistrationDate").val();
                        chargeRequest.EstimatedDate = $("#inputRegistrationDate").val();
                        chargeRequest.AccountingDate = $("#inputAccountingDate").val();
                        chargeRequest.PrefixId = $("#selectPrefixesOfClaims").UifSelect("getSelected");
                        chargeRequest.MovementTypeId = $("#selectMovementType").UifSelect("getSelected");
                        chargeRequest.IsPrinted = false;
                        chargeRequest.IndividualId = payerId;
                        chargeRequest.PersonTypeId = $("#selectCollectTo").UifSelect("getSelected");
                        chargeRequest.BranchId = $("#selectRecoveryBranch").UifSelect("getSelected");
                        chargeRequest.PaymentRequestTypeId = 2;
                        chargeRequest.PaymentSourceId = $("#selectPaymentSource").UifSelect("getSelected");
                        chargeRequest.ClaimId = this.ClaimId;
                        chargeRequest.SubClaim = this.SubClaimId;
                        chargeRequest.ClaimBranchId = this.BranchCode;
                        chargeRequest.ClaimNumber = this.ClaimNumber;
                        chargeRequest.RecoveryId = this.Id;
                        chargeRequest.Voucher = Charge.CreateVoucer();
                        chargeRequest.Voucher.ClaimNumber = this.ClaimNumber;
                        chargeRequest.BeneficiaryDocumentType = IndividualDocumentTypeId;

                        switch ($("#selectCollectTo").UifSelect("getSelectedText")) {
                            case 'PROVEEDOR':
                                chargeRequest.BeneficiaryDocumentNumber = $("#inputDocumentNumberSupplier").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#inputFullNameSupplier").UifAutoComplete('getValue');
                                break;
                            case 'ASEGURADO':
                                chargeRequest.BeneficiaryDocumentNumber = $("#inputDocumentNumberInsured").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#inputFullNameInsured").UifAutoComplete('getValue');
                                break;
                            case 'TERCERO':
                                chargeRequest.BeneficiaryDocumentNumber = $("#InputDocumentNumberThirdParty").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#InputFullNameThirdParty").UifAutoComplete('getValue');
                                break;
                            case 'TOMADOR':
                                chargeRequest.BeneficiaryDocumentNumber = $("#inputDocumentNumberHolder").UifAutoComplete('getValue');
                                chargeRequest.BeneficiaryFullName = $("#inputFullNameHolder").UifAutoComplete('getValue');
                                break;
                        }

                        chargeRequestsModel.push(chargeRequest);
                    }
                });
                break;
        }
    }

    static CreateVoucer() {
        var voucher = {};
        voucher.Id = 0;
        voucher.VoucherTypeId = 1;
        voucher.CurrencyId = 0;
        voucher.Number = 0;
        voucher.Date = new Date();
        voucher.ExchangeRate = 1;
        voucher.EstimationTypeId = parseInt($("#selectPaymentSource").UifSelect('getSelected')) == 2 ? 2 : 7;
        voucher.Concepts = [];
        return voucher;
    }

    static DoConsultChargeRequest() {

        if (modelSearchCriteria.chargeRequestId == null)
            return;

        ChargeRequest.GetChargeRequestByChargeRequestId(modelSearchCriteria.chargeRequestId).done(function (response) {
            if (response.success) {
                rowsConcept = response.result.Voucher.Concepts;
                $("#tblSingleBillingInformation").UifDataTable({ sourceData: rowsConcept, add: false, delete: false, edit: false, selectionType: 'none' });

                Charge.GetPaymentSourcesByChargeRequest(function (paymentSources) {
                    $('#selectPaymentSource').UifSelect({ sourceData: paymentSources, enable: false });
                    $("#selectPaymentSource").UifSelect("setSelected", response.result.PaymentSourceId);
                    Charge.UpdateClaimsTable();

                    Charge.GetBranches(function (branches) {
                        $('#selectRecoveryBranch').UifSelect({ sourceData: branches, enable: false });
                        $('#selectBranch').UifSelect({ sourceData: branches, enable: false });

                        $("#selectRecoveryBranch").UifSelect("setSelected", response.result.BranchId);
                        $("#selectBranch").UifSelect("setSelected", response.result.ClaimBranchId);

                        Charge.GetPrefixes(function (prefixes) {
                            $('#selectPrefixesOfClaims').UifSelect({ sourceData: prefixes, enable: false });
                            $("#selectPrefixesOfClaims").UifSelect("setSelected", response.result.PrefixId);

                            $("#inputPolicyDocumentNumber").prop('disabled', true);
                            $("#inputPolicyDocumentNumber").val(response.result.PolicyDocumentNumber);

                            $("#inputClaimNum").prop('disabled', true);
                            $("#inputClaimNum").val(response.result.ClaimNumber);

                            Charge.GetRecoveriesAndSalvageByClaim(function (recoveryOrSalvage) {
                                switch (response.result.PaymentSourceId) {
                                    case 2:
                                        $('#tblClaimsRecoverySalvage').UifDataTable({ sourceData: recoveryOrSalvage.filter(x => x.Id == response.result.SalvageId), selectionType: 'none' });
                                        Charge.FillGridSalvageOrRecovery(null, { Id: response.result.SalvageId }, null);
                                        break;
                                    case 3:
                                        $('#tblClaimsRecoverySalvage').UifDataTable({ sourceData: recoveryOrSalvage.filter(x => x.Id == response.result.RecoveryId), selectionType: 'none' });
                                        Charge.FillGridSalvageOrRecovery(null, { Id: response.result.RecoveryId }, null);
                                        break;
                                }
                            });
                        });
                        Charge.GetPaymentMovementTypesByPaymentSourceId(function (movementTypes) {
                            $('#selectMovementType').UifSelect({ sourceData: movementTypes, enable: false });
                            $("#selectMovementType").UifSelect("setSelected", response.result.MovementTypeId);

                            Charge.GetPaymentConceptsByBranchIdMovementTypeId(function (paymentConcepts) {
                                $("#tblPaymentConcept").UifDataTable({ sourceData: paymentConcepts.filter(x => { return rowsConcept.some(y => y.PaymentConceptId == x.Id) }), selectionType: 'none' });
                            });
                        });
                    });
                });

                $("#inputRegistrationDate").val(FormatDate(response.result.RegistrationDate));

                Charge.GetPersonTypes(function (personTypes) {
                    $('#selectCollectTo').UifSelect({ sourceData: personTypes, enable: false });
                    $("#selectCollectTo").UifSelect("setSelected", response.result.PersonTypeId);
                    $("#selectCollectTo").trigger('change');

                    IndividualDocumentTypeId = response.result.BeneficiaryDocumentType;

                    switch ($('#selectCollectTo').UifSelect('getSelectedText')) {
                        case "PROVEEDOR":
                            $("#inputDocumentNumberSupplier").val(response.result.BeneficiaryDocumentNumber);
                            $("#inputDocumentNumberSupplier").prop("disabled", true);
                            $("#inputFullNameSupplier").val(response.result.BeneficiaryFullName);
                            $("#inputFullNameSupplier").prop("disabled", true);
                            break;
                        case "ASEGURADO":
                            $("#inputDocumentNumberInsured").val(response.result.BeneficiaryDocumentNumber);
                            $("#inputDocumentNumberInsured").prop("disabled", true);
                            $("#inputFullNameInsured").val(response.result.BeneficiaryFullName);
                            $("#inputFullNameInsured").prop("disabled", true);
                            break;
                        case 'TERCERO':
                            $("#InputDocumentNumberThirdParty").val(response.result.BeneficiaryDocumentNumber);
                            $("#InputDocumentNumberThirdParty").prop("disabled", true);
                            $("#InputFullNameThirdParty").val(response.result.BeneficiaryFullName);
                            $("#InputFullNameThirdParty").prop("disabled", true);
                            break;
                        case 'TOMADOR':
                            $("#inputDocumentNumberHolder").UifAutoComplete('setValue', response.result.BeneficiaryDocumentNumber);
                            $("#inputDocumentNumberHolder").UifAutoComplete('disabled', true);
                            $("#inputFullNameHolder").UifAutoComplete('setValue', response.result.BeneficiaryFullName);
                            $("#inputFullNameHolder").UifAutoComplete('disabled', true);
                            break;
                        default:
                            $('#divSupplier').hide();
                            $('#divInsured').hide();
                            $('#divThirdparty').hide();
                            $('#divHolder').hide();
                            break;
                    }
                });

                $("#btnClearChargeRequest").hide();
                $("#btnSaveCharge").hide();
                $("#btnSearch").hide();
                $("#btnPaymentConcept").hide();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }

        });
    }

}