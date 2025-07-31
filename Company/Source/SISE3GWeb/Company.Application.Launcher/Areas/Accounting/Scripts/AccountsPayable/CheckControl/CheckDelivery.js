$(document).ready(function () {


    /*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
    /*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
    /*----------------------------------------------------------------------------------------------------------------------------------------------------------*/


    var Alert = 0;
    var Numerrors = 0;
    var currencyId = -1;
    var numberAssociated = 0;
    var checkBookControlId = 0;
    var individualId = -1;
    var resultGrid = -1;
    var countSelect = 0;
    var viewDateReception = false;
    var viewDevolution = false;

    var oListPrintCheckModelDelivery = {
        PrintCheckModels: []
    };

    var oPrintCheckModel = {
        AddressCompany: null,
        Amount: 0,
        BeneficiaryName: null,
        CheckNumber: 0,
        CompanyName: null,
        EstimatedPaymentDate: null,
        NumberPaymentOrder:0,
        BankName:null,
        AccountCurrentNumber:null,
        CurrencyName:null,
        CheckPaymentOrderCode:0,
        PersonTypeId:0,
        CourierName: null,
        RefoundDate:null,
        DeliveryTypeId: 0
    };


    /*---------------------------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
    /*---------------------------------------------------------------------------------------------------------------------------------------------------*/

    if ($("#ViewBagBranchDisableCheckDeliver").val() == "1") {
        setTimeout(function () {
            $("#BranchCheckDelivery").attr("disabled", "disabled");
        }, 300);
    }
    else {
        $("#BranchCheckDelivery").removeAttr("disabled");
    }

    setTimeout(function () {
        GetBanksDeliver();
    }, 2000);

    ///////////////////////////////////////////////////
    //  Limpia Objeto   oListPrintCheckModelDelivery //
    //////////////////////////////////////////////////
    function setDataEmptyDeliveryCheck()
    {
        oListPrintCheckModelDelivery = {
            PrintCheckModels: []
        };
    }


    //////////////////////////////////////////////////
    //  Carga datos en Grid                         //
    //////////////////////////////////////////////////
    function loadGridDeliver() {
        var amount = ClearFormatCurrency($("#amountPaymentOrderCheckDelivery").val());
        var amountFormat = amount.toString().replace(",", ".").replace("$", "");
        $("#DeliveryChecksCheckDelivery").UifDataTable();
        var controller = ACC_ROOT + "CheckControl/GetDeliveryCheck?accountBankId=" + $("#AccountNumberCheckDelivery").val()
        +"&deliveryTypeId="+$("#DeliveryTypeCheckDelivery").val()
        +"&paymentOrderNumber="+ $("#numberPaymentOrderCheckDelivery").val()
        +"&checkNumber="+$("#checkNumberCheckDelivery").val()
        +"&amount="+amountFormat
        +"&payTo="+$("#checkPayableToCheckDelivery").val();

        $("#DeliveryChecksCheckDelivery").UifDataTable({ source: controller });
    }


    ///////////////////////////////////////////////////
    //  Limpia campos del formulario                //
    //////////////////////////////////////////////////
    function cleanFieldsDeliver() {
        $("#BranchCheckDelivery").val("");
        $("#NameBankCheckDelivery").val("");
        $("#AccountNumberCheckDelivery").val("");
        $("#PayerTypeCheckDelivery").val("");
        if($("#ViewBagDeliveryType").val() != 0)
        {
            $("#DeliveryTypeCheckDelivery").val("");
        }
        $("#numberPaymentOrderCheckDelivery").val("");
        $("#checkNumberCheckDelivery").val("");
        $("#amountPaymentOrderCheckDelivery").val("");
        $("#checkPayableToCheckDelivery").val("");
        $("#personReceivingNameCheckDelivery").val("");
        numberAssociated = 0;
        checkBookControlId = 0;
        currencyId = -1;
        individualId = -1;
        countSelect = 0;
        viewDateReception = true;
        viewDevolution = true;

        $("#DeliveryChecksCheckDelivery").dataTable().fnClearTable();
        }


    /////////////////////////////////////////////////////////
    //  Carga datos en  Objetos                           //
    ////////////////////////////////////////////////////////
    function saveDataDelivery() {


        lockScreen();
        setTimeout(function () {

            $.ajax({
            type: "POST",
            url: ACC_ROOT + "CheckControl/UpdateCheckPaymentOrder",
            data: JSON.stringify({ "listPrintCheckModel": setDataDeliveryCheck() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
                success: function (data) {

                unlockScreen();
                setCleanFieldsDeliver();
                //EXCEPCION ROLLBACK
                if (data.success == false) {
                    $("#alertCheckDelivery").UifAlert('show', data.result, "danger");

                } else {

                    $("#alertCheckDelivery").UifAlert('show', Resources.SuccesSave, "success");
                }

                setDataEmptyDeliveryCheck();
            }
            });
        }, 500);
    }


    //////////////////////////////////////////////////
    //  Arma Objeto   oListPrintCheckModelDelivery  //
    //////////////////////////////////////////////////
    function setDataDeliveryCheck() {

        var ids = $("#DeliveryChecksCheckDelivery").UifDataTable("getSelected");
        var row = $("#DeliveryChecksCheckDelivery").UifDataTable('getData');

        if (ids.length > 0 )
        {
            if (ids != null) {

                for (var i in ids) {
                    oPrintCheckModel = {
                        AddressCompany: null,
                        Amount: 0,
                        BeneficiaryName: null,
                        CheckNumber: 0,
                        CompanyName: null,
                        EstimatedPaymentDate: null,
                        NumberPaymentOrder:0,
                        BankName:null,
                        AccountCurrentNumber:null,
                        CurrencyName:null,
                        CheckPaymentOrderCode:0,
                        PersonTypeId:0,
                        CourierName: null,
                        RefoundDate:null,
                        DeliveryTypeId: 0

                    };

                    oPrintCheckModel.AddressCompany = ids[i].AddressCompany;
                    oPrintCheckModel.Amount = ids[i].Amount;
                    oPrintCheckModel.CheckNumber = ids[i].CheckNumber;
                    oPrintCheckModel.BeneficiaryName = ids[i].BeneficiaryName;
                    oPrintCheckModel.CompanyName = ids[i].CompanyName;
                    oPrintCheckModel.EstimatedPaymentDate = ids[i].EstimatedPaymentDate;
                    oPrintCheckModel.NumberPaymentOrder = ids[i].NumberPaymentOrder;
                    oPrintCheckModel.BankName = $("#NameBankCheckDelivery option:selected").text();
                    oPrintCheckModel.AccountCurrentNumber = $("#AccountNumberCheckDelivery option:selected").text();
                    oPrintCheckModel.CheckPaymentOrderCode = ids[i].CheckPaymentOrderCode;
                    oPrintCheckModel.DeliveryTypeId = $("#DeliveryTypeCheckDelivery").val();
                    oPrintCheckModel.PersonTypeId = $("#PayerTypeCheckDelivery").val();          //tipo
                    oPrintCheckModel.CourierName = $("#personReceivingNameCheckDelivery").val(); //nombre
                    oPrintCheckModel.RefoundDate = $("#dateDeliveryCheckDelivery").val();
                    oPrintCheckModel.PaymentOrderCode = ids[i].NumberPaymentOrder;

                    oListPrintCheckModelDelivery.PrintCheckModels.push(oPrintCheckModel);
                }
            }

        }
        return oListPrintCheckModelDelivery;
    }


    ///////////////////////////////////////////////////
    //  Limpia campos y variables                   //
    //////////////////////////////////////////////////
    function setCleanFieldsDeliver() {
        $("#BranchCheckDelivery").val("");
        $("#NameBankCheckDelivery").val("");
        $("#AccountNumberCheckDelivery").val("");
        if($("#ViewBagDeliveryType").val() != 0)
        {
            $("#DeliveryTypeCheckDelivery").val("");
        }
        $("#PayerTypeCheckDelivery").val("");
        $("#numberPaymentOrder").val("");
        $("#checkNumber").val("");
        $("#amountPaymentOrder").val("");
        $("#checkPayableTo").val("");
        $("#personReceivingNameCheckDelivery").val("");
        numberAssociated = 0;
        checkBookControlId = 0;
        currencyId = -1;
        individualId = -1;
        countSelect = 0;
        viewDateReception = true;
        viewDevolution = true;
        $("#DeliveryChecksCheckDelivery").dataTable().fnClearTable();
        $("#alertCheckDelivery").UifAlert('hide');
    }


    ///////////////////////////////////////////////////
    //  Muestra mensaje de confirmación              //
    //////////////////////////////////////////////////
    function showConfirmDeliver() {
        $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': 'Entrega de Cheques' }, function (result) {
            if (result) {
                setCleanFieldsDeliver();
            }
        });
    };


    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    //                                                                  ACCIONES / EVENTOS
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/

    /////////////////////////////////////////
    //  Dropdown Sucursal
    /////////////////////////////////////////
    $('#BranchCheckDelivery').on('itemSelected', function (event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchCheckDelivery').val();
            $("#NameBankCheckDelivery").UifSelect({ source: controller });
            $("#AccountNumberCheckDelivery").UifSelect();
            $("#NameBankCheckDelivery").removeAttr('disabled');
        }
    });

    /////////////////////////////////////////
    //  Dropdown Banco                     //
    /////////////////////////////////////////
    $("#NameBankCheckDelivery").on('itemSelected', function (event, selectedItem) {
        if ($("#NameBankCheckDelivery").val() != "" && $("#NameBankCheckDelivery").val() != null) {
            var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + $("#NameBankCheckDelivery").val()
            + "&branchId=" + $("#BranchCheckDelivery").val();
            $("#AccountNumberCheckDelivery").UifSelect({ source: controller });
            $("#AccountNumberCheckDelivery").removeAttr('disabled');
        }
    });

    /////////////////////////////////////////
    //  Formato como Moneda
    /////////////////////////////////////////
    $("#amountPaymentOrderCheckDelivery").blur(function () {
        var amountPaymentOrder = $("#amountPaymentOrderCheckDelivery").val();
        amountPaymentOrder = amountPaymentOrder.replace("$", "");
        $("#amountPaymentOrderCheckDelivery").val(FormatCurrency(FormatDecimal( amountPaymentOrder)));
    });


    if ($("#ViewBagDeliveryType").val() == 0) {
        viewDateReception = false;
        viewDevolution = true;
        $("#DeliveryChecksCheckDelivery").dataTable().fnClearTable();
        $("#enabledDeliveryType").show();
        $("#enabledDeliveryName").show();
        setTimeout(function () {
            $("#DeliveryTypeCheckDelivery").attr('disabled','disabled');
            $("#DeliveryTypeCheckDelivery").val(2);
        }, 2000)
    }

    //Carga fecha del día
    $("#dateDeliveryCheckDelivery").val($("#ViewBagDateNow").val());


    /////////////////////////////////////////
    //  Botón  Buscar                      //
    /////////////////////////////////////////
    $("#SearchCheckDelivery").click(function () {

        if (!$("#CheckDelivery").valid()) {
            return false;
        }
        else{
            Numerrors = 0;
            $("#validate_FormDeliveryAsigmentCheckDelivery").click();
            if ((Numerrors == 0) || (Numerrors == undefined)) {
                Alert = 0;
            } else {
                Alert = 1;
                Numerrors = 0;
            }

            if (Alert == 0) {
                loadGridDeliver();

            } else {
                $("#alertCheckDelivery").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
            }
        }
    });



    /////////////////////////////////////////
    //  Dropdown DeliveryType
    /////////////////////////////////////////
    $("#DeliveryTypeCheckDelivery").on('itemSelected', function (event, selectedItem) {
        
        if ($("#DeliveryTypeCheckDelivery").val() == 1 || $("#DeliveryTypeCheckDelivery").val() == "") {
            viewDateReception = true;
            viewDevolution = true;
            $("#DeliveryChecksCheckDelivery").dataTable().fnClearTable();
        
            $("#payerTypeCheckDeliveryLabel").hide();
            $("#personReceivingNameCheckDeliveryLabel").hide();
            $("#PayerTypeCheckDelivery").hide(); //PayerTypeCheckDelivery
            $("#personReceivingNameCheckDelivery").hide();  //personReceivingNameCheckDelivery
        }
        if ($("#DeliveryTypeCheckDelivery").val() == 2) {
            viewDateReception = false;
            viewDevolution = true;
            $("#DeliveryChecksCheckDelivery").dataTable().fnClearTable();

            $("#payerTypeCheckDeliveryLabel").show();
            $("#personReceivingNameCheckDeliveryLabel").show();
            $("#PayerTypeCheckDelivery").show();
            $("#personReceivingNameCheckDelivery").show();
        }
        if ($("#DeliveryTypeCheckDelivery").val() == 3) {
            viewDateReception = true;
            viewDevolution = false;
            $("#DeliveryChecksCheckDelivery").dataTable().fnClearTable();
            $("#payerTypeCheckDeliveryLabel").hide();
            $("#personReceivingNameCheckDeliveryLabel").hide();
            $("#PayerTypeCheckDelivery").hide();
            $("#personReceivingNameCheckDelivery").hide();
        }

    });



    /////////////////////////////////////////
    //  Botón  Limpiar                     //
    /////////////////////////////////////////
    $("#CleanCheckDelivery").click(function () {
        cleanFieldsDeliver();
    });

    /////////////////////////////////////////
    //  Botón  Aceptar                     //
    /////////////////////////////////////////
    $("#AcceptCheckDelivery").click(function () {

        if ($("#formCheckDelivery").valid()) {
            var ids = $("#DeliveryChecksCheckDelivery").UifDataTable("getSelected");

            if(ids != null )
            {
                if (ids.length > 0) {
                    for (var i in ids) {
                        if ($("#DeliveryTypeCheckDelivery").val() == 2) {
                            if ($("#PayerTypeCheckDelivery").val() != "" && $("#personReceivingNameCheckDelivery").val() != "") {
                                saveDataDelivery();
                            } else {
                                $("#alertCheckDelivery").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
                            }
                        }
                        else {
                            saveDataDelivery();
                        }
                    }
                }
                else {
                    $("#alertCheckDelivery").UifAlert('show', Resources.InsertedCheckPrint, "warning");
                }
            }
            else{
                $("#alertCheckDelivery").UifAlert('show', Resources.WarningSelectedPaymentOrder, "warning");
            }
        }
    });

    /////////////////////////////////////////
    //  Botón  Cancelar                    //
    /////////////////////////////////////////
    $("#CancelCheckDelivery").click(function () {
        showConfirmDeliver();
    });

    $('#DeliveryChecksCheckDelivery').on('rowSelected', function (event, data, position) {
        //$("#alertCheckDelivery").UifAlert('show', 'selellele', "warning");
    });



    function GetBanksDeliver() {
        if ($('#BranchCheckDelivery').val() > 0) {
            var controller = ACC_ROOT+ "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchCheckDelivery').val();
            $("#NameBankCheckDelivery").UifSelect({ source: controller });

        }
    };

});
   