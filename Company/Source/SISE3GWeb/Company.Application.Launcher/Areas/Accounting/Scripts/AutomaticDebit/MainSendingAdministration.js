//variables
var sendNetId = 0;
var idsPrefix = [];
var sendAdministrationSendDate = "";
var statusCodeProcess = 0;
var sendAdministrationSendDateTime = "";
var sendAdministrationBillId = -1;
$('#datepicker').on("datepicker.change", function (event, date) {
    console.log(date);
});

$(() => {
    new MainSendingAdministration();
});

class MainSendingAdministration extends Uif2.Page {

    getInitialState() {
        $("#SendingAdministrationLoadingDiv").hide();
        $("#btnApply").attr("disabled", "disabled");
        $("#btnReverseCollections").hide();
    }

    bindEvents() {
        $('#sendingSelectNet').on('itemSelected', this.sendingSelectNet);
        $("#sendingSendDateTime").on("datepicker.change", this.lotNumber);
        $('#sendingLotNumber').on('itemSelected', this.sendingLotNumberSelect);
        $('#btnNotification').on('click', this.preNotify);
        $('#btnGenerateTXT').on('click', this.generateText);
        $('#btnSendingExcel').on('click', this.generateExcel);
        $('#tableLogProcess').on('rowSelected', this.downloadFile);
        $("#btnRejectionManual").on('click', this.manualRejection);
        $('#tblReverse').on('rowSelected', this.reverseSelection);
        $("#btnReverseCollections").on('click', this.reverseCollections);
        $('#btnApplyRejection').on('click', this.applyRejection);
        $("#btnRejectCoupons").on('click', this.rejectCoupon);
        $("#btnReverse").on('click', this.showReverseModal);
        $('#tblReverse').on('rowSelected', this.reverseRecord);
        $('#btnSendingCancel').on('click', this.sendingAdministrationClearFields);
        $('#btnApply').on('click', this.applyCollections);
    }

    sendingSelectNet(event, selectedItem) {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        $("#LotStatus").val("");
        $('#tableSummary').dataTable().fnClearTable();
        $('#tableLogProcess').dataTable().fnClearTable();

        if (selectedItem.Id != "") {
            //time = window.setInterval(refreshGrid, 8000);
            sendingAdministrationLoadPendingProcessGrid();

            $("#sendingSendDateTime").removeAttr("disabled");
            $("#btnGenerateTXT").removeAttr("disabled");

            var controller = ACC_ROOT + "Common/GetLotNumbersByNetId?netId=" + selectedItem.Id + "&sendDate=" + $("#sendingSendDateTime").val();
            $("#sendingLotNumber").UifSelect({ source: controller });
            $("#btnApply").attr("disabled", "disabled");
            sendNetId = $('#sendingSelectNet').val();

            /*---------------------se valida si requiere notificacion-----------------------------*/

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "AutomaticDebit/GetRequiresNotification",
                data: JSON.stringify({ "bankNetworkId": selectedItem.Id, "sendDate": $("#sendingSendDateTime").val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data != null) {
                        if (data == "0") {
                            $("#btnNotification").attr("disabled", "disabled");
                        }
                        else {
                            $("#btnNotification").attr("disabled", false);
                        }
                    }
                }
            });
        }
        else {
            $("#sendingLotNumber").UifSelect();
        }
    }

    lotNumber(event, date) {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        if (IsDate($('#sendingSendDateTime').val())) {
            var controller = ACC_ROOT + "Common/GetLotNumbersByNetId?netId=" + $('#sendingSelectNet').val() + "&sendDate=" + $("#sendingSendDateTime").val();
            $("#sendingLotNumber").UifSelect({ source: controller });
            $("#btnApply").attr("disabled", "disabled");
        }
    }

    sendDateTimeChange() {
        if (IsDate($('#sendingSendDateTime').val())) {
            var controller = ACC_ROOT + "Common/GetLotNumbersByNetId?netId=" + $('#sendingSelectNet').val() + "&sendDate=" + $("#sendingSendDateTime").val();
            $("#sendingLotNumber").UifSelect({ source: controller });
            $("#btnApply").attr("disabled", "disabled");
            SendingAdministrationSetFieldsEmpty();
        }
    }

    sendingLotNumberSelect(event, selectedItem) {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');
        // Combo número de lote
        // Estados de cupones
        // 2. PENDIENTES DE ENVIO AL BANCO
        // 3. ARCHIVO ENVIADO AL BANCO
        // 5. ARCHIVO CARGADO AL BANCO
        // 6. ARCHIVO FINALIZADO
        if (selectedItem.Id != undefined) {
            if (selectedItem.Id != "") {
                if (selectedItem.Id == 2) {
                    $("#btnApply").attr("disabled", "disabled");
                    $("#btnRejectionManual").attr("disabled", false);
                    //$("#btnNotification").attr("disabled", false);
                    $("#btnGenerateTXT").attr("disabled", false);
                    $("#btnReverseCollections").hide();
                }

                if (selectedItem.Id == 3) {
                    $("#btnNotification").attr("disabled", "disabled");
                    $("#btnGenerateTXT").attr("disabled", "disabled");
                    $("#btnApply").attr("disabled", "disabled");
                    $("#btnReverseCollections").hide();
                }

                if (selectedItem.Id == 5) {
                    $("#btnGenerateTXT").attr("disabled", "disabled");
                    $("#btnApply").attr("disabled", false);
                    $("#btnRejectionManual").attr("disabled", "disabled");
                    $("#btnReverseCollections").hide();
                }

                if (selectedItem.Id == 6) {
                    $("#btnGenerateTXT").attr("disabled", "disabled");
                    $("#btnApply").attr("disabled", "disabled");
                    $("#btnSendingExcel").attr("disabled", "disabled");
                    $("#btnRejectionManual").attr("disabled", "disabled");
                    $("#btnNotification").attr("disabled", "disabled");
                    $("#btnReverseCollections").show();
                }

                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "AutomaticDebit/GetLotStatus",
                    data: JSON.stringify({ "statusCode": selectedItem.Id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data != null) {
                            $("#LotStatus").val(data.toString());
                        }
                    }
                });

                var lotNumberLength = 0;
                var hourMinute = "";
                lotNumberLength = $("#sendingLotNumber option:selected").html().length - 4;
                hourMinute = $("#sendingLotNumber option:selected").html().substr(lotNumberLength, 4);
                sendAdministrationSendDateTime = $('#sendingSendDateTime').val().substr(0, 10);

                sendAdministrationSendDateTime = sendAdministrationSendDateTime + " " + hourMinute.substr(0, 2) + ":" + hourMinute.substr(2, 4);

                var controller = ACC_ROOT + "AutomaticDebit/GetSummaryByNet?netId=" + $('#sendingSelectNet').val() + "&sendDate=" +
                                     sendAdministrationSendDateTime + "&lotNumber=" + $("#sendingLotNumber option:selected").html();
                $("#tableSummary").UifDataTable({ source: controller });
            }
            else {
                $("#LotStatus").val("");
                $('#tableSummary').dataTable().fnClearTable();
            }
        }
    }

    preNotify() {

        $("#formSending").validate();
        if ($("#formSending").valid()) {
            if (sendAdministrationSendDateTime != "") {

                $("#btnNotification").attr("disabled", "disabled");
                $("#alertBankStatement").UifAlert('hide');
                $("#alert").UifAlert('hide');

                statusCodeProcess = 0;
                var dataLogProcess = $("#tableLogProcess").UifDataTable("getData");
                if (dataLogProcess.length > 0) {
                    for (var i in dataLogProcess) {
                        if (dataLogProcess[i].LotNumber == $("#sendingLotNumber option:selected").html()) {
                            statusCodeProcess = 1;
                            $("#alert").UifAlert('show', Resources.LotNumberProcess, "warning");
                            $("#btnNotification").attr("disabled", false);
                        }
                    }
                }

                if (statusCodeProcess == 0) {
                    sendNetId = $('#sendingSelectNet').val();
                    sendAdministrationSendDate = sendAdministrationSendDateTime;
                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "AutomaticDebit/GeneratePreNotification",
                        data: JSON.stringify({ "bankNetworkId": sendNetId, "sendDate": sendAdministrationSendDate, "processTypeId": 2 }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data != null) {
                                if (data == "false") {
                                    $("#alert").UifAlert('show', Resources.WrongLoadBankResponseMistakes, "danger");
                                }
                                else if (data == "true") {
                                    $("#sendingLotNumber").UifSelect();

                                    setTimeout(function () {
                                        var controller = ACC_ROOT + "Common/GetLotNumbersByNetId?netId=" + sendNetId + "&sendDate=" + $("#sendingSendDateTime").val();
                                        $("#sendingLotNumber").UifSelect({ source: controller });
                                        $("#LotStatus").val("");
                                        sendingAdministrationLoadPendingProcessGrid();
                                    }, 3000);
                                    $("#alert").UifAlert('show', Resources.PreNoticeGenerated, "success");
                                }
                                else {
                                    $("#alert").UifAlert('show', data, "danger");
                                }
                            }
                            else {
                                sendingAdministrationLoadPendingProcessGrid();
                            }
                        }
                    });
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectLotNumber, "warning");
            }
        }
        else {
            $("#btnNotification").attr("disabled", false);
        }
    }

    generateText() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        $("#formSending").validate();

        if ($("#formSending").valid()) {
            statusCodeProcess = 0;
            if ($('#sendingLotNumber').val() != "" && $('#sendingLotNumber').val() != null) {
                var dataLogProcess = $("#tableLogProcess").UifDataTable("getData");
                if (dataLogProcess.length > 0) {
                    for (var i in dataLogProcess) {
                        if (dataLogProcess[i].LotNumber == $("#sendingLotNumber option:selected").html()) {
                            statusCodeProcess = 1;
                            $("#alert").UifAlert('show', Resources.LotNumberProcess, "danger");
                        }
                    }
                }

                if (statusCodeProcess == 0) {
                    $('#SendingAdministrationLoadingDiv').show();
                    sendingAdministrationLoadPendingProcessGrid();

                    sendNetId = $('#sendingSelectNet').val();
                    sendAdministrationSendDate = sendAdministrationSendDateTime;

                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "AutomaticDebit/GeneratePreNotification",
                        data: JSON.stringify({ "bankNetworkId": sendNetId, "sendDate": sendAdministrationSendDate, "processTypeId": 3 }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data != null) {
                                if (data == "false") {
                                    $("#alert").UifAlert('show', Resources.WrongLoadBankResponseMistakes, "danger");
                                    setTimeout(function () {
                                        $("#alert").UifAlert('hide');
                                    }, 3000);
                                    sendingAdministrationLoadPendingProcessGrid() 
                                }
                            }
                            else {
                                sendingAdministrationLoadPendingProcessGrid();
                            }
                        }
                    });
                    $('#SendingAdministrationLoadingDiv').hide();
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectLotNumber, "danger");
            }
        }
    }

    generateExcel() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');
        $("#formSending").validate();

        if ($("#formSending").valid()) {
            if ($('#sendingLotNumber').val() != "" && $('#sendingLotNumber').val() != null) {
                if (sendAdministrationSendDateTime != "") {
                    $('#SendingAdministrationLoadingDiv').show();
                    sendNetId = $('#sendingSelectNet').val();
                    sendAdministrationSendDate = sendAdministrationSendDateTime;
                    ExportPreNotificationExcel(sendNetId, sendAdministrationSendDate);
                    $('#SendingAdministrationLoadingDiv').hide();
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectLotNumber, "danger");
            }
        }
    }

    downloadFile(event, data, position) {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        if (data != null) {
            var fileNane = "";
            if (data.statusCode == 1) {
                $('#SendingAdministrationLoadingDiv').show();
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "AutomaticDebit/ExportFileNotification",
                    data: JSON.stringify({ "bankNetworkId": data.BankNetworkId, "lotNumber": data.LotNumber, "processTypeId": data.processTypeId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data != "") {
                            fileNane = data.toString();
                            SetDownloadTxt(data);
                        }
                        $('#SendingAdministrationLoadingDiv').hide();
                    }
                });
            }
            else {
                $("#alert").UifAlert('show', Resources.PendingProcesses, "danger");
            }
        }
    }

    manualRejection() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        var rowIds = $("#tableSummary").UifDataTable("getSelected");

        if ($("#sendingLotNumber option:selected").val() == '') {
            $("#alert").UifAlert('show', 'Seleccione un Envio', "warning");
            return;
        }


        if (rowIds != null) {

            var lotNumberLength = 0;
            var hourMinute = "";
            lotNumberLength = $("#sendingLotNumber option:selected").html().length - 4;
            hourMinute = $("#sendingLotNumber option:selected").html().substr(lotNumberLength, 4);
            sendAdministrationSendDateTime = $('#sendingSendDateTime').val().substr(0, 10);
            sendAdministrationSendDateTime = sendAdministrationSendDateTime + " " + hourMinute.substr(0, 2) + ":" + hourMinute.substr(2, 4);
            var controller = ACC_ROOT + "AutomaticDebit/GetDetailByNet?netId=" + $('#sendingSelectNet').val() + "&sendDate=" +
                                 sendAdministrationSendDateTime + "&lotNumber=" + $("#sendingLotNumber option:selected").text();
            $("#tblRejection").UifDataTable();
            $("#tblRejection").UifDataTable({ source: controller });

            var rejectioncontroller = ACC_ROOT + "AutomaticDebit/GetRejectionReason?networkId=" + $('#sendingSelectNet').val();
            $("#rejectionStatusBank").UifSelect({ source: rejectioncontroller });

            var rowData = $("#tblRejection").UifDataTable('getData')[0];
            //var codConducto = rowData.PaymentMethodCd;
            var codConducto = "";
            var paymentMethodDescription = "";
            $.ajax({
                url: ACC_ROOT + "Common/GetPaymentMethodByCode",
                dataType: "json",
                data: { 'paymentMethodCd': codConducto }
            }).done(function (data) {
                paymentMethodDescription = data;
            });

            //Armamos el titulo del envio
            var taxConditionTitle = '<strong>' + Resources.Net + ': </strong>' + $("#sendingSelectNet option:selected").text() + '<strong> Conducto: </strong>' + paymentMethodDescription + '<br/><strong>' + Resources.LotNumber + ':</strong>' + $("#sendingLotNumber option:selected").text() + '<strong>' + Resources.LotStatus + ': </strong>' + $("#LotStatus").val();
            $('#ModalTaxCondition').find('.ptitle').html(taxConditionTitle);
            $('#RejetionManual').UifModal('showLocal', taxConditionTitle);

            //Ancho de Check 10px
            $("#RejetionManual .modal-lg table .sorting_1 div").css('min-width', '10px');
            $("#RejetionManual .modal-lg table .sorting_1 div").css('width', '10px');
        }
        else {
            $("#alert").UifAlert('show', 'Seleccione un item de la tabla', "warning");
        }
    }

    reverseSelection(event, data, position) {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        $("#alertModalReverse").UifAlert('hide');
        if (parseInt(data.ReceiptNumber) == 0) {
            $("#alertModalReverse").UifAlert('show', 'El Numero de Recibo debe ser mayor a cero', "warning");
            event.preventDefault();
            var lengthPage = $("#tblReverse > #tblReverse_length select").val();
            position = position % 10;
            $($("#tblReverse > tbody > tr")[position]).find('button').trigger('click');
        }
    }

    reverseCollections() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        var rowIds = $("#tableSummary").UifDataTable("getSelected");
        if ($("#sendingLotNumber option:selected").val() == '') {
            $("#alert").UifAlert('show', Resources.SelectOneItem, "warning");
            return;
        }
        if (rowIds != null) {
            var lotNumberLength = 0, hourMinute = "";

            lotNumberLength = $("#sendingLotNumber option:selected").html().length - 4;
            hourMinute = $("#sendingLotNumber option:selected").html().substr(lotNumberLength, 4);
            sendAdministrationSendDateTime = $('#sendingSendDateTime').val().substr(0, 10);
            sendAdministrationSendDateTime = sendAdministrationSendDateTime + " " + hourMinute.substr(0, 2) + ":" + hourMinute.substr(2, 4);

            var controller = ACC_ROOT + "Accounting/AutomaticDebit/GetDetailByNet?netId=" + $('#sendingSelectNet').val() + "&sendDate=" +
                                 sendAdministrationSendDateTime + "&lotNumber=" + $("#sendingLotNumber option:selected").text();
            console.log(controller);
            $("#tblReverse").UifDataTable({ source: controller });

            var codConducto = "";
            var paymentMethodDescription = "";

            //Armamos el titulo del envio
            var title = '<strong>' + Resources.Net + ': </strong>' + $("#sendingSelectNet option:selected").text() + '<br/><strong>' + Resources.LotNumber + ':</strong>' + $("#sendingLotNumber option:selected").text() + '<strong>' + Resources.LotStatus + ': </strong>' + $("#LotStatus").val();
            $('#RevertModal').UifModal('showLocal', title);

            //Ancho de Check 10px
            $("#RevertModal .modal-lg table .sorting_1 div").css('min-width', '10px');
            $("#RevertModal .modal-lg table .sorting_1 div").css('width', '10px');
        }
        else {
            $("#alert").UifAlert('show', Resources.SelectRecordSummaryTable, "warning");
        }
    }

    applyRejection() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        $("#alertModal").UifAlert('hide');

        if (($('#rejectionStatusBank').val() == '') && ($('#rejectionStatusInternal').val() == '')) {
            $("#alertModal").UifAlert('show', Resources.SelectOneItem, "warning");
            return;
        }

        var rowIdSelected = $("#tblRejection").UifDataTable("getSelected");
        var rowIdAll = $("#tblRejection").UifDataTable("getData");

        if (rowIdSelected == null) {
            $("#alertModal").UifAlert('show', Resources.SelectOneItem, "warning");
            return;
        }

        for (var i in rowIdAll) {

            if (ValidateSelectedRow(rowIdAll[i])) {

                if ($("#rejectionStatusBank option:selected").val() != "") {
                    rowIdAll[i].RejectionReasonBank = $("#rejectionStatusBank option:selected").text();
                    rowIdAll[i].RejectionBankId = $("#rejectionStatusBank option:selected").val();
                }
                $('#tblRejection').UifDataTable('editRow', rowIdAll[i], i);
            }
        }
        $($("#tblRejection > thead > tr")[0]).find('button').trigger('click');
        $($("#tblRejection > thead > tr")[0]).find('button').trigger('click');

        //Seteamos nuevamente a selecciones los combos
        $('#rejectionStatusBank').val("");
    }

    rejectCoupon() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        var rowids = $("#tblRejection").UifDataTable("getData");
        if (rowids == null) {
            $("#alertModal").UifAlert('show', Resources.SelectOneItem, "danger");
            return;
        }

        var listCoupon = [];
        if (rowids != null) {
            for (var i in rowids) {
                if (parseInt(rowids[i].RejectionBankId) != 0 || (parseInt(rowids[i].RejectionInternalId) != 0)) {
                    var oCoupon = {
                        CouponsNumber: 0,
                        CouponSecuence: 0,
                        RejectionBankId: 0,
                        ShippingId: 0
                    };
                    oCoupon.CouponsNumber = parseInt(rowids[i].CouponsNumber);
                    oCoupon.CouponSecuence = parseInt(rowids[i].CouponSecuence);
                    oCoupon.ShippingId = Number($("#sendingLotNumber option:selected").text());
                    oCoupon.RejectionBankId = rowids[i].RejectionBankId;
                    listCoupon.push(oCoupon);
                }
            }
        }
        if (listCoupon.length == 0) {
            $("#alertModal").UifAlert('show', 'Aplique un motivo de Rechazo', "danger");
            return;
        }

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "/AutomaticDebit/SaveCouponRejectionManual",
            data: JSON.stringify({ "rejectionCoupons": listCoupon }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data != null) {
                    $("#alertBankStatement").UifAlert('show', Resources.SuccesSave, "success");
                    var controller = ACC_ROOT + "AutomaticDebit/GetSummaryByNet?netId=" + $('#sendingSelectNet').val() + "&sendDate=" +
                        sendAdministrationSendDateTime + "&lotNumber=" + $("#sendingLotNumber option:selected").html();
                    $("#tableSummary").UifDataTable({ source: controller });
                    $('#RejetionManual').UifModal('hide');
                }
            }
        });
    }

    showReverseModal() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        var rowids = $("#tblReverse").UifDataTable("getSelected");
        if (rowids == null) {
            $("#alertModal").UifAlert('show', Resources.SelectOneItem, "danger");
            return;
        } else {
            $("#alertModalReverse").UifAlert('hide');
            validateRevert(sendAdministrationBillId, parseInt($("#imputationTypeId").val()));
            validateRevertPromise.then(function (revertData) {

                if (!(revertData)) {
                    validateDeposited();
                    validateDepositedPromise.then(function (depositedData) {
                        if (!depositedData) { //pago en boleta de depósito
                            validatePayment(sendAdministrationBillId, parseInt($("#imputationTypeId").val()));

                            validatePaymentPromise.then(function (paymentData) {
                                if (paymentData)
                                    reverseApplication();
                                else
                                    $("#alertModalReverse").UifAlert('show', Resources.PaymentRevertValidation, "warning");
                            });
                        }
                        else
                            $("#alertModalReverse").UifAlert('show', Resources.BillValuesAlreadyDeposited, "warning");
                    });
                }
                else {
                    $("#alertModalReverse").UifAlert('show', Resources.ReceiptIsAlreadyReversed, "warning");
                }
            });
        }
    }

    reverseRecord(event, data, position) {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        sendAdministrationBillId = data.ReceiptNumber;
    }

    sendingAdministrationClearFields() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        $("#sendingSelectNet").UifSelect();
        $("#sendingSendDateTime").val($("#ViewBagCurrentDate").val());
        $("#sendingLotNumber").UifSelect();
        $("#LotStatus").val("");
        $('#tableSummary').dataTable().fnClearTable();
        $('#tableLogProcess').dataTable().fnClearTable();
        $("#btnReverseCollections").hide();
    }

    applyCollections() {
        $("#alertBankStatement").UifAlert('hide');
        $("#alert").UifAlert('hide');

        $("#formSending").validate();

        if ($("#formSending").valid()) {
            if ($('#sendingLotNumber').val() == 5) {
                $('#SendingAdministrationLoadingDiv').show();
                sendNetId = $('#sendingSelectNet').val();
                sendAdministrationSendDate = sendAdministrationSendDateTime;

                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "AutomaticDebit/ApplyCollections",
                    data: JSON.stringify({ "bankNetworkId": sendNetId, "sendDateTime": sendAdministrationSendDate }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data != null) {
                            if (data.substr(0, 2) == "-1") {
                                $("#alert").UifAlert('show', data, "success");
                            }
                            else {
                                $("#alert").UifAlert('show', Resources.MessageCoupon, "success");
                            }
                        }
                    }
                });
                $('#SendingAdministrationLoadingDiv').hide();
            }
            else {
                $("#alert").UifAlert('show', Resources.UploadFileApplyBank, "danger"); //UploadFileApplyBank
            }
            setTimeout(function () {
                $("#alert").UifAlert('hide');
            }, 3000);
        }
    }
}

function sendingAdministrationLoadPendingProcessGrid() {
    var networkId = 0;

    if ($("#sendingSelectNet").val() == "") {
        networkId = 0;
    }
    else {
        networkId = $("#sendingSelectNet").val();
    }
    var controller = ACC_ROOT + "Common/GetDebitPendingProcess?bankNetworkId=" + networkId + "&processTypeId=0";
    $("#tableLogProcess").UifDataTable({ source: controller });
}

function ExportPreNotificationExcel(netId, sendAdministrationSendDate) {

    idsPrefix = [];
    var prefixes = $("#tableSummary").UifDataTable("getSelected");
    if (prefixes != null) {
        if (prefixes.length != 0) {
            for (var l = 0; l < prefixes.length; l++) {
                idsPrefix.push(prefixes[l].Prefix);
            }
        }

        var url = ACC_ROOT + "AutomaticDebit/GeneratePreNotificationExcel?bankNetworkId=" + sendNetId + "&sendDate=" + sendAdministrationSendDate
            + "&lotNumber=" + $('#sendingLotNumber option:selected').html() + "&prefixes=" + idsPrefix;
        var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
        setTimeout(function () {
            newPage.open('', '_self', '');
        }, 100);
    }
    else {
        $("#alertBankStatement").UifAlert('show', Resources.SelectRecordSummaryTable, "warning");
    }
}

function SetDownloadTxt(fileName) {
    var result = ACC_ROOT + "AutomaticDebit/Download?fileName=" + fileName;
    var newPage = window.open(result, '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
}

function validateDeposited() {

    return validateDepositedPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BillSearch/ValidateDeposited",
            data: { "billId": sendAdministrationBillId }
        }).done(function (depositedData) {
            resolve(depositedData);
        });
    });
}

function validatePayment(sendAdministrationBillId, imputationTypeId) {

    return validatePaymentPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BillSearch/ValidatePaymentRevertion",
            data: { "sourceId": sendAdministrationBillId, "imputationTypeId": imputationTypeId }
        }).done(function (paymentData) {
            resolve(paymentData);
        });
    });
}

function validateRevert(sendAdministrationBillId, imputationTypeId) {

    return validateRevertPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BillSearch/GetImputationBySourceCodeImputationTypeId",
            data: { "sourceId": sendAdministrationBillId, "imputationTypeId": imputationTypeId }
        }).done(function (revertData) {
            resolve(revertData);
        });
    });
}

function reverseApplication() {

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/ReverseImputationRequest",
        data: { "sourceId": sendAdministrationBillId, "imputationTypeId": $("#imputationTypeId").val(), "accountingDate": "01/01/1900" }, // La fecha se envia hasta definir la reversión en administración de Envío
        success: function (data) {
            $("#PremiumReceivableListSelectionDialog").modal('hide');
            if (data.success) {
                var description = "";

                if (data.result.length > 0) {
                    for (var i = 0; i < data.result.length; i++) {
                        description = description + data.result[i] + ". ";
                    }
                }

                $("#alertModalReverse").UifAlert('show', Resources.ApplicationReverseSuccessLabel + '. ' + Resources.AccountingTransactionNumberGenerated + description, "success");

                if ($("#ReceiptStatus").val() != "") {
                    receiptStatus = $("#ReceiptStatus").val();
                } else {
                    receiptStatus = -1;
                }

                // refresca la grilla
                var control = '@Url.Content("~/")' + "BillSearch/SearchBills?branchId=" + branchId + "&billingConceptId=" +
                                billingConceptId + "&startDate=" + $("#StartDateBillSearch").val() + "&endDate=" + $("#EndDateBillSearch").val() +
                                "&userId=" + userId + "&billId=" + sendAdministrationBillId + "&receiptStatus=" + receiptStatus +
                                "&imputationTypeId=" + $("#imputationTypeId").val();
                $("#SearchBillsTable").UifDataTable({ source: control });
            } else {
                $("#alertBillSearch").UifAlert('show', Resources.ApplicationReverseFailureLabel, "danger");
                $("#CancelBill").hide();
            }
        }
    });
}

function ValidateSelectedRow(rowId) {

    var rowIdSelected = $("#tblRejection").UifDataTable("getSelected");
    var result = false;
    if (rowIdSelected != null) {
        for (var i in rowIdSelected) {

            if (rowId.CouponsNumber == rowIdSelected[i].CouponsNumber && rowId.CouponSecuence == rowIdSelected[i].CouponSecuence) {
                result = true;
                break;
            }
        }
    }
    return result;
}

function SendingAdministrationSetFieldsEmpty() {
    $("#LotStatus").val("");
    $('#tableSummary').dataTable().fnClearTable();
}