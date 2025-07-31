
    /*-----------------------------------------------------------------------------------------------------------------------------------------*/
    //DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
    /*-----------------------------------------------------------------------------------------------------------------------------------------*/
    var issuingBankId = -1;

    var bankId = -1;
    var checkNumber = -1;

    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    //ACCIONES / EVENTOS
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/


    if ($("#ViewBagBranchDisable").val() == "1") {
        setTimeout(function () {
            $("#branch").attr("disabled", "disabled");
        }, 300);
    }
    else {
        $("#branch").removeAttr("disabled");
    }

    /// Fecha Proceso Desde
    $('#dateFrom').on("datepicker.change", function (event, date) {

        $("#alert").UifAlert('hide');
        if (IsDate($('#dateFrom').val())) {
            if ($("#dateTo").val() != "")
            {
                if (CompareDates($('#dateFrom').val(), $("#dateTo").val()) == true) {
                    $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
                    $("#dateFrom").val('');
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "warning");
        }

        setTimeout(function () {
            $("#alert").UifAlert('hide');
        }, 3000);
    });

    $("#dateFrom").blur(function () {
        $("#alert").UifAlert('hide');

        if ($("#dateFrom").val() != '') {

            if (IsDate($("#dateFrom").val()) == true) {
                if ($("#dateTo").val() != '') {
                    if (CompareDates($("#dateFrom").val(), $("#dateTo").val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#dateFrom").val("");
                        return true;
                    }
                }
            } else {
                $("#alert").UifAlert('show', Resources.InvalidDates, "warning");
                $("#dateFrom").val("");
            }
        }
    });

    /// Fecha Proceso Hasta
    $('#dateTo').on("datepicker.change", function (event, date) {
        $("#alert").UifAlert('hide');
        if (IsDate($('#dateTo').val())) {
            if ($('#dateFrom').val() != "") {
                if (CompareDates($('#dateFrom').val(), $("#dateTo").val()) == true) {
                    $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
                    $("#dateTo").val("");
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "warning");
        }

        setTimeout(function () {
            $("#alert").UifAlert('hide');
        }, 3000);
    });

    $("#dateTo").blur(function () {
        $("#alert").UifAlert('hide');

        if ($("#dateTo").val() != '') {

            if (IsDate($("#dateTo").val()) == true) {
                if ($("#dateFrom").val() != '') {
                    if (CompareDates($("#dateFrom").val(), $("#dateTo").val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
                        $("#dateTo").val("");
                        return true;
                    }
                }
            } else {
                $("#alert").UifAlert('show', Resources.InvalidDates, "warning");
                $("#dateTo").val("");
            }
        }
    });

    /// Reporte de Cheques 
    $("#Report").click(function () {

        $("#MaincheckPending").validate();

        if ($("#MaincheckPending").valid()) {
            var branchId = ($("#branch").val() == "") ? -1 : $("#branch").val();
            if (branchId == null || branchId == undefined) {
                    branchId = -1;
                }
                ShowCheckPendingDepositReport(issuingBankId, $("#dateFrom").val(), $("#dateTo").val(), branchId);
        }
    });

    /// Tabla procesos pendientes de carga respuesta banco
    $('#tableProcess').on('rowSelected', function (event, data, position) {
        if (data.RecordsFailed > 0) {
            var url = ACC_ROOT;
            url = url + "AutomaticDebit/ExportToExcelErrorLoadBankResponse?bankNetworkId=" + data.BankNetworkId
                      + "&lotNumber=" + data.LotNumber;
            var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
            setTimeout(function () {
                newPage.open('', '_self', '');
            }, 100);
        }
    });

    // Autocomplete banco emisor - cheque
    $('#Bank').on('itemSelected', function(event, selectedItem) {
        issuingBankId = selectedItem.id;
    });

    // Botón búsqueda
    $("#SearchPendingDeposit").click(function () {
        $("#alert").UifAlert('hide');
        $("#MaincheckPending").validate();

        if ($("#MaincheckPending").valid() && validateRangeDate() ) {

            var branchId = ($("#branch").val() == "") ? -1 : $("#branch").val();
            if (branchId == null) {
                branchId = -1;
            }

            var controller = ACC_ROOT + "Transaction/GetChecksDepositingPending?issuingBankCode=" + issuingBankId + "&startDate=" +
            $("#dateFrom").val() + "&endDate=" + $("#dateTo").val() + "&branchCode=" + branchId;

            $("#CheckPendingDepositTbl").UifDataTable({ source: controller });
        }
        else
        {
            $("#Clean").trigger( "click" );
        }
    });



    $("#Clean").click(function () {
        $("#dateFrom").val('');
        $("#dateTo").val('');
        $("#Bank").val('');
        $("#branch").val('');
        issuingBankId = -1;

        $("#CheckPendingDepositTbl").dataTable().fnClearTable();
    });

    /*---------------------------------------------------------------------------------------------------------------------------------------------------*/
    //DEFINICION DE FUNCIONES
    /*---------------------------------------------------------------------------------------------------------------------------------------------------*/

    // Visualiza el reporte de caja en pantalla
    function ShowCheckPendingDepositReport(bankId, startDate, endDate, branchId) {
        var controller = ACC_ROOT + "Report/ShowChecksDepositingPendingReport?issuingBankCode=" + bankId +
                                    "&startDate=" + startDate + "&endDate=" + endDate + "&branchCode=" + branchId;
        window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }


    function setDataFieldsEmpty() {
 
        clearCheckSearch();
        clearCheckInformation();
        clearDepositInformation();
        clearRejectedInformation();
        clearRegularizedInformation();
        clearLegalInformation();
        IssuingBankId = -1;
      //$("#tblSearchCheck").jqGrid("clearGridData", true);
        $("#tblSearchCheck").dataTable().fnClearTable();
        $("#btnReport").hide();
    }

    function validateDateFrom() {
        if ($('#dateFrom').val() == "") return true;
        if (IsDate($('#dateFrom').val())) {
            if ($("#dateTo").val() != "")
            {
                if (CompareDates($('#dateFrom').val(), $("#dateTo").val()) == true) {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateFrom, "warning");
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessFrom, "warning");
            return false;
        }
    }


    function validateDateTo() {
        if ($('#dateTo').val() == "" && $('#dateFrom').val() == "") {
            return true;
        }
        else {
            if (IsDate($('#dateTo').val())) {
                if (CompareDates($('#dateFrom').val(), $("#dateTo").val()) == true) {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateTo, "warning");
                    return true;
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "warning");
                return false;
            }
        }
    }


    function validateRangeDate() {

        if (IsDate($('#dateTo').val()) && IsDate($('#dateFrom').val())) {
            if ($('#dateFrom').val() == $("#dateTo").val()) {
                return true;
            }

            if (CompareDates($('#dateFrom').val(), $("#dateTo").val()) == true) {
                $("#alert").UifAlert('show', Resources.MessageValidateProcessDateTo, "warning");
                return false;
            }
        }
        return true;
    }

