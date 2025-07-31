    /*-----------------------------------------------------------------------------------------------------------------------------------------*/
    //DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
    /*-----------------------------------------------------------------------------------------------------------------------------------------*/
var timePreliquidationSearch = window.setInterval(validateCompanyPreliquidationSearch, 700);
var beneficiaryId = 0;
var searchByDocumentNumber = null;
var userId = 0;
var accoutingCompanyId = -1;
var salesPointId = -1;
var payerTypeId = -1;
var rowSelected = null;
var tempImputationId = 0;
var preliquidationId = 0;
var selectedAutocomplete;

    var oPreLiquidation = {
        Id: 0,  
        BranchId: 0,
        CompanyId: 0,
        Description: "",
        ImputationId: 0,
        IsTemporal: 0,
        IndividualId: 0,
        PersonTypeId: 0,
        RegisterDate: null,
        SalePointId: 0,
        StatusId: 0,
        BranchDescription: "",
        CompanyDescription: "",
        PayerDocumentNumber: "",
        PayerName: "",
        TempImputationId: 0,
        TotalAmount:0
    };

    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    //ACCIONES / EVENTOS
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    if ($("#ViewBagBranchDisable").val() == "1") {
        setTimeout(function () {
            $("#Branch").attr("disabled", true);
        }, 300);
    }
    else {
        $("#Branch").attr("disabled", false);
    }

    setTimeout(function () {
        GetSalePointsPreliquidationSearch();
    }, 2000);

    $("#editPreliquidation").hide();
    $("#cancelPreliquidation").hide();
    $("#printPreliquidation").hide();
    $("#applyPreliquidation").hide();

    //dropdown de sucursal
    $("#Branch").on('itemSelected', function (event, selectedItem) {
        // Se obtiene los puntos de venta
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#SalePoint").UifSelect({ source: controller });
        }
    });

    //Valida que no ingresen una fecha invalida.
    $("#StartDate").blur(function () {

        if ($("#StartDate").val() != '') {

            if (IsDate($("#StartDate").val()) == true) {

                if (CompareDates($("#StartDate").val(), getCurrentDate()) == 2) {
                    $("#StartDate").val(getCurrentDate);
                }
            } else {

                $("#alert").UifAlert('show', Resources.InvalidDates, "danger");

                $("#StartDate").val("");
            }
        }
    });

    $("#EndDate").blur(function () {
        if ($("#EndDate").val() != '') {
            if (IsDate($("#EndDate").val()) == true) {
                if (CompareDates($("#EndDate").val(), getCurrentDate()) == 2) {
                    $("#EndDate").val(getCurrentDate);
                }
            } else {
                $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
                $("#EndDate").val("");
            }
        }
    });

    //Controla que la fecha final sea mayor a la inicial
    $('#StartDate').on('datepicker.change', function (event, date) {
        if ($("#EndDate").val() != "") {
            if (compare_dates($('#StartDate').val(), $("#EndDate").val())) {

                $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");

                $("#StartDate").val('');
            } else {
                $("#StartDate").val($('#StartDate').val());
                $("#alert").UifAlert('hide');
            }
        }
    });

$('#EndDate').on('datepicker.change', function (event, date) {
        if ($("#StartDate").val() != "") {
            if (compare_dates($("#StartDate").val(), $('#EndDate').val())) {

                $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");

                $("#EndDate").val('');
            } else {
                $("#EndDate").val($('#EndDate').val());
                $("#alert").UifAlert('hide');
            }
        }
});

    //habilito variable para los autocompletes de búsqueda
$("#DocumentNumber").on('focusin', function () {
    searchByDocumentNumber = true;
    if ($("#DocumentNumber").val() != "")
        $("#DocumentNumber").val($("#DocumentNumber").val());
});


$("#Name").on('blur', function (event, selectedItem) {
    setTimeout(function () {
        if (beneficiaryId > 0) {
            searchByDocumentNumber = false;
            if (selectedAutocomplete != null) {
                $("#Name").val(selectedAutocomplete.Name);
            }
        }
    }, 50);
});

$("#DocumentNumber").on('blur', function (event, selectedItem) {
    setTimeout(function () {
        if (beneficiaryId > 0) {
            searchByDocumentNumber = true;
            if (selectedAutocomplete != null) {
                $("#DocumentNumber").val(selectedAutocomplete.DocumentNumber);
            }
        }
    }, 50);

});

   //autocomplete por número de documento
$("#DocumentNumber").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.IndividualId > 0) {
        $("#DocumentNumber").val(selectedItem.DocumentNumber);
        $("#Name").val(selectedItem.Name);
        beneficiaryId = selectedItem.IndividualId;
        searchByDocumentNumber = true;
        selectedAutocomplete = selectedItem;
    } else {
        $("#DocumentNumber").val("");
        $("#Name").val("");
        beneficiaryId = 0;
        searchByDocumentNumber = null;
        selectedAutocomplete = null;
    }
});

    //autocomplete por nombre
$("#Name").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.IndividualId > 0) {
        $("#DocumentNumber").val(selectedItem.DocumentNumber);
        $("#Name").val(selectedItem.Name);
        beneficiaryId = selectedItem.IndividualId;
        searchByDocumentNumber = false;
        selectedAutocomplete = selectedItem;
    } else {
        $("#DocumentNumber").val("");
        $("#Name").val("");
        beneficiaryId = 0;
        searchByDocumentNumber = null;
        selectedAutocomplete = null;
    }
});


    //autocomplete de usuario
    $("#User").on('itemSelected', function (event, selectedItem) {
        if (selectedItem.id > 0) {
            $("#User").val(selectedItem.nick);
            userId = selectedItem.id;
        } else {
            $("#User").val("");
            userId = 0;
        }
    });

    //botón buscar
    $("#search").click(function () {

        //Oculta botones y grilla
        $('#PreliquidationsSearchTable').dataTable().fnClearTable();
        $("#editPreliquidation").hide();
        $("#cancelPreliquidation").hide();
        $("#printPreliquidation").hide();
        $("#applyPreliquidation").hide();
        
        $("#PreliquidationsSearchForm").validate();
        if ($("#PreliquidationsSearchForm").valid()) {
            //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

            var startDate = $("#StartDate").val();
            var endDate = $("#EndDate").val();

            if ($("#Company").val() != "")
                accoutingCompanyId = $("#Company").val();
            if ($("#SalePoint").val() != "" && $("#SalePoint").val() != null)
                salesPointId = $("#SalePoint").val();
            else {
                salesPointId = -1;
            }
            if ($("#PayerType").val() != "")
                payerTypeId = $("#PayerType").val();

            //valida usuario
            if ($("#User").val() == "") {
                userId = 0;
            }

            var controller = ACC_ROOT + "PreLiquidations/GetPreliquidations?branchId=" + $("#Branch").val() +
                "&accountingCompanyId=" + accoutingCompanyId + "&salesPointId=" + salesPointId +
                "&startDate=" + startDate + "&endDate=" + endDate +
                "&preliquidationId=" + $("#PreliquidationNumber").val() + "&personTypeId=" + payerTypeId +
                "&beneficiaryIndividualId=" + beneficiaryId + "&userId=" + userId;
            $("#PreliquidationsSearchTable").UifDataTable({ source: controller });

            rowSelected = null;
        }
    });

    //botón limpiar
    $("#clear").click(function () {
        setDataFieldsEmptyPreLiquidationSearch();
    });

    //Evento de selección en tabla de movimientos
    $('#PreliquidationsSearchTable').on('rowSelected', function (event, data) {
        rowSelected = data;

        if (data.Status == 1) {
            $("#editPreliquidation").show();
            $("#cancelPreliquidation").show();
            $("#printPreliquidation").show();

           //Solo se permite aplicar cuando el total esté en valor negativo
            if (ClearFormatCurrency(data.TotalAmount) >= 0) {
                $("#applyPreliquidation").hide();
            } else {
                $("#applyPreliquidation").show();
            }
        }
        else if (data.Status == 2) {
            $("#editPreliquidation").hide();
            $("#cancelPreliquidation").hide();
            $("#printPreliquidation").hide();
            $("#applyPreliquidation").hide();
        }
        else if (data.Status == 3) {
            $("#editPreliquidation").hide();
            $("#cancelPreliquidation").hide();
            $("#printPreliquidation").show();
            $("#applyPreliquidation").hide();
        }
    });

    //botón de anular
    $("#cancelPreliquidation").click(function () {

        if (rowSelected != null) {

            var tempImputationId = rowSelected.TempImputationId;
            var preliquidationId = rowSelected.PreliquidationId;

            showConfirmPreLiquidationCancel(preliquidationId, tempImputationId);

        } else {
            $("#alert").UifAlert('show', Resources.SelectOneItem, "warning");
        }
    });

    function showConfirmPreLiquidationCancel(paramPreliquidationId, paramTempImputationId) {
        $.UifDialog('confirm', {
            'message': Resources.PreLiquidationSearchCancelation, 'title': Resources.PreliquidationsSearch
        }, function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "PreLiquidations/CancelPreliquidation",
                    data: { "preliquidationId": paramPreliquidationId, "tempImputationId": paramTempImputationId },
                    success: function (data) {

                        if (data) {
                            //$("#alert").UifAlert('show', Resources.SuccessfulCancellation, "success");
                            $("#editPreliquidation").hide();
                            $("#cancelPreliquidation").hide();
                            $("#printPreliquidation").hide();
                            $("#applyPreliquidation").hide();
                            $("#search").trigger('click');
                            tempImputationId = 0;
                            preliquidationId = 0;
                        }
                    }
                });

            }
        });
    };




    //botón imprimir
    $("#printPreliquidation").click(function () {
        if (rowSelected != null) {
            tempImputationId = rowSelected.TempImputationId;
            preliquidationId = rowSelected.PreliquidationId;
            showConfirmPreLiquidationSearch();
        } else {
            $("#alert").UifAlert('show', Resources.SelectOneItem, "warning");
        }
    });

    //click en botón editar (isPreliquidation = 1 para relacionar que la edicion es por Preliquidacion y no permitir la edicion de la cabezera)
    $("#editPreliquidation").click(function () {
        if (rowSelected != null) {

            lockScreen();
            setTimeout(function () {    
              location.href = $("#ViewBagLoadPreliquidationsLink").val()+"?preliquidationId=" + rowSelected.PreliquidationId +
                
                "&branchId=" + rowSelected.BranchId + "&salePointId=" + rowSelected.SalesPointId +
                "&companyId=" + rowSelected.AccountingCompanyId + "&generationDate=" + rowSelected.RegisterDate +
                "&personTypeId=" + rowSelected.PersonTypeId + "&documentNumber=" + rowSelected.BeneficiaryDocumentNumber +
                "&name=" + rowSelected.BeneficiaryName +
                "&beneficiaryId=" + rowSelected.BeneficiaryIndividualId +
                "&tempImputationId=" + rowSelected.TempImputationId +
                "&description=" + rowSelected.Description+
                "&isPreliquidation=1";

            }, 300);
        } else {
            $("#alert").UifAlert('show', Resources.SelectOneItem, "warning");
        }
    });

    $("#applyPreliquidation").click(function () {
        if (rowSelected != null) {
            var totalAmount = ClearFormatCurrency(rowSelected.TotalAmount);

            lockScreen();
            setTimeout(function () {                   

                location.href = $("#ViewBagLoadBillingLink").val()+ "?preliquidationId=" + rowSelected.PreliquidationId +

                    "&branchId=" + rowSelected.BranchId + "&branchDescription=" + rowSelected.BranchDescription +
                    "&documentNumber=" + rowSelected.BeneficiaryDocumentNumber +
                    "&name=" + rowSelected.BeneficiaryName +
                    "&tempImputationId=" + rowSelected.TempImputationId +
                    "&description=" + rowSelected.Description +
                    "&totalAmount=" + totalAmount +
                    "&individualId=" + rowSelected.BeneficiaryIndividualId +
                    "&preliquidationBranch=" + rowSelected.BranchId +
                    "&personTypeId=" + rowSelected.PersonTypeId;
            }, 300);

        } else {
            $("#alert").UifAlert('show', Resources.SelectOneItem, "warning");
        }                       
    });

    /*---------------------------------------------------------------------------------------------------------------------------------------------------*/
    //DEFINICION DE FUNCIONES
    /*---------------------------------------------------------------------------------------------------------------------------------------------------*/
$(document).ajaxSend(function (event, xhr, settings) {

        if (searchByDocumentNumber != null) {
            settings.url = settings.url + "&param=" + $("#PayerType").val() + "/" + searchByDocumentNumber;
        }
});


    function setDataFieldsEmptyPreLiquidationSearch() {
        $("#Branch").val("");
        $("#SalePoint").val("");
        $("#Company").val("");
        $("#StartDate").val("");
        $("#EndDate").val("");
        $("#PreliquidationNumber").val("");
        $("#PayerType").val("");
        $("#DocumentNumber").val("");
        $("#Name").val("");
        $("#User").val("");

        beneficiaryId = 0;
        searchByDocumentNumber = null;
        userId = 0;
        accoutingCompanyId = -1;
        salesPointId = -1;
        payerTypeId = -1;
        rowSelected = null;

        $('#PreliquidationsSearchTable').dataTable().fnClearTable();

        $("#editPreliquidation").hide();
        $("#cancelPreliquidation").hide();
        $("#printPreliquidation").hide();
    }

    function showConfirmPreLiquidationSearch() {
        $.UifDialog('confirm', { 'message': Resources.PreliquidationRePrint + ' ' + preliquidationId + '?',
                                 'title': Resources.PreliquidationsSearch }, function (result) {
            if (result) {
                rePrintPreliquidationReport();
                $("#editPreliquidation").hide();
                $("#cancelPreliquidation").hide();
                $("#printPreliquidation").hide();
                $("#applyPreliquidation").hide();
            }
        });
    };

    function rePrintPreliquidationReport() {
        $.ajax({
            type: "POST",
            async: false,
            url: ACC_ROOT + "Report/LoadRePrintPreLiquidationsReport",
            data: { "preLiquidationModel": setDataPrintReportPreLiquidations(preliquidationId, tempImputationId),
                    "tempImputationId": tempImputationId },
            success: function (data) {

                if (parseInt(data) > 0) {
                    showRePrintPreliquidationReport();
                    $("#search").trigger("click");
                    tempImputationId = 0;
                    preliquidationId = 0;
                }
            }
        });
    }

    function setDataPrintReportPreLiquidations(preliquidationId, tempImputationId) {
        if (rowSelected != null) {
            oPreLiquidation.Id = preliquidationId;
            oPreLiquidation.BranchId = rowSelected.BranchId;
            oPreLiquidation.CompanyId = rowSelected.AccountingCompanyId;
            oPreLiquidation.Description = rowSelected.PersonTypeDescription;
            oPreLiquidation.IndividualId = rowSelected.BeneficiaryIndividualId;
            oPreLiquidation.PersonTypeId = rowSelected.PersonTypeId;
            oPreLiquidation.SalePointId = rowSelected.SalesPointId;
            oPreLiquidation.RegisterDate = rowSelected.RegisterDate;
            oPreLiquidation.BranchDescription = rowSelected.BranchDescription;
            oPreLiquidation.CompanyDescription = rowSelected.AccountingCompanyDescription;
            oPreLiquidation.PayerDocumentNumber = rowSelected.BeneficiaryDocumentNumber;
            oPreLiquidation.PayerName = rowSelected.BeneficiaryName;
            oPreLiquidation.TempImputationId = tempImputationId;
        }

        return oPreLiquidation;

    }

    function showRePrintPreliquidationReport() {
        window.open(ACC_ROOT + "Report/ShowPreLiquidationReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }

    function GetSalePointsPreliquidationSearch() {
        if ($('#Branch').val() > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + $('#Branch').val();
            $("#SalePoint").UifSelect({ source: controller });

            //Setea el punto de venta de default
            setTimeout(function () {
                $("#SalePoint").val($("#ViewBagSalePointBranchUserDefault").val());
            }, 500);
        }
    };

    //Habilita / deshabilita combo Company segùn corresponda
    function validateCompanyPreliquidationSearch() {

     if ($("#Company").val() != "" && $("#Company").val() != null) {

                if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

                    $("#Company").attr("disabled", true);

                    if (parseInt($("#ViewBagBranchIdFromPreliquidation").val()) > 0 ) { //viene de Preliquidación

                        setParametersFromPreliquidation();
                    }

                }
                else {
                    $("#Company").attr("disabled", false);
                }
                clearInterval(timePreliquidationSearch);
            } else {
            $("#Company").val($("#ViewBagAccountingCompanyDefault").val());
            }
    }


function setParametersFromPreliquidation() {

    $("#Branch").val($("#ViewBagBranchIdFromPreliquidation").val());
    $("#PreliquidationNumber").val($("#ViewBagPreliquidationIdFromPreliquidation").val());

    setTimeout(function () {
        $("#search").trigger('click');
    }, 300);

}