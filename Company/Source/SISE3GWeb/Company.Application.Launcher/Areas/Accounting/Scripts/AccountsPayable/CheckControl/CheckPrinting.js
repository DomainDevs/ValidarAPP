/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var oListPrintCheckModelPrint = {
    PrintCheckModels: []
};

var oPrintCheckModelPrint = {
    AddressCompany: null,
    Amount: 0,
    BeneficiaryName: null,
    CheckNumber: 0,
    CompanyName: null,
    EstimatedPaymentDate: null,
    NumberPaymentOrder: 0,
    BankName: "",
    AccountCurrentNumber: null,
    CurrencyName: null,
    CheckPaymentOrderCode: 0
};


/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisablePrinting").val() == "1") {
    setTimeout(function () {
        $("#BranchCheckPrinting").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#BranchCheckPrinting").removeAttr("disabled");
}

setTimeout(function () {
    GetBanksPrint();
}, 2000);

//Carga reporte
function loadReportPrint() {

    lockScreen();
    setTimeout(function () {        
        $.ajax({
        type: "POST",
        url: ACC_ROOT + "CheckControl/LoadReportPrintCheck",
        data: JSON.stringify({ "printCheckModel": setDataPrintAssigmentCheck() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
            success: function (data) {
                unlockScreen();
            printCheck();
            $("#alertCheckPrinting").UifAlert('show', Resources.WarningPrintCheckSucces, "success");
            setDataEmptyPrintAssigment();
            setCleanFieldsPrint();
        }
        });

    }, 500);
}

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirmPrint() {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.MainPathCheckPrinting }, function (result) {
        if (result) {
            setCleanFieldsPrint();
        }
    });
};

//////////////////////////////////////////////
// Visualiza el reporte de cheques en pantalla //
//////////////////////////////////////////////
function ShowPrintCheckReport() {
    var controller = ACC_ROOT  + "CheckControl/ShowPrintCheckReport";
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

///////////////////////////////////////////////////
//  Limpia Objeto   oListPrintCheckModelPrint        //
//////////////////////////////////////////////////
function setDataEmptyPrintAssigment() {
    oListPrintCheckModelPrint = {
        PrintCheckModels: []
    };
}


//FUNCION QUE NOS PERMITE IMPRIMIR EL CHEQUE EN EL CLIENTE.
function printCheck() {

    var printCommand = '<object id="PrintCommandObject" name="printObject" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>';
    var scriptChrome = "<script language='javascript1.2' > function printChrome(){;var p=window.document.getElementById(\"framePDF\") ;";
    scriptChrome += "  p.contentWindow.print();} <\/script>";
    var scriptIe = "<script language='javascript1.2' > function printIe(){;var p=window.document.getElementById(\"framePDF\") ;";
    scriptIe += " p.setActive(); 	p.print(); } <\/script>";

    var printIe = '<link rel=\"alternate\" media=\"print\"  type="application/pdf" href=\''+ACC_ROOT +'Content/file.pdf\" \/>';
    var pageStyleIe = "<style type=\"text\/css\"   media=\"print\">\n<!-- #mainTab{display:none;} \n #mainToolbar{display:none;}    \n ";
    pageStyleIe += "  #menubilling{display:none;} \n #printReady{display:block;} \n  --><\/style>\n \n";

    var pageStyle = "<style type=\"text\/css\"><!--@@page {  margin: 0cm }--><\/style>\n";
    var style = "<style type=\"text\/css\"> #framePDF {border-width: 0px; position:absolute;top:0px;left:0px;  overflow:hidden; width:100%;height:100%;z-index: 50}<\/style>";


    var html = '<!DOCTYPE html>\n<html>\n<head>\n';
    var headTags = document.getElementsByTagName("head");
    if (headTags.length > 0) html += headTags[0].innerHTML;

    html += '\n ' + style + '\n ' + pageStyle + '\n ' + scriptChrome + '\n ';

    html += '\n</head>\n<';

    if (navigator.userAgent.indexOf('Chrome') != -1) {
        html += 'body onLoad="javascript: window.setTimeout(function(){printChrome()} , 3000);">\n';
    }
    if (navigator.userAgent.indexOf('Firefox') != -1) {
        html += 'body onLoad="javascript: window.setTimeout(function(){window.print()} , 3000);">\n';
    }
    if (navigator.appName == "Microsoft Internet Explorer") {
        html += 'body>\n';
    }

    html += '<iframe src=' + ACC_ROOT + 'CheckControl/ShowPrintCheckReport name = "framePDF"  id="framePDF"  scrolling="auto" frameborder="no" width="100%" height="100%" style="margin:0 0px 0 0;"></iframe>';

    html += ' \n  </body>\n</html>';
    var printWin = document.getElementById("printFrame").contentWindow;
    printWin.document.write(html);
    printWin.document.close();


    //   Object para imprimir en Iexplorer
    if (navigator.appName == "Microsoft Internet Explorer") {
        var printCommandObject = null;
        printCommandObject = '<object data='+ ACC_ROOT + 'Content/file.pdf name = "framePDF"  id="framePDF" type="application/pdf"  width="0" height="0" ></object>';
        document.body.insertAdjacentHTML('beforeEnd', printCommandObject);

        var p = window.document.getElementById("framePDF");
        p.focus();
        window.setTimeout(function () { p.print(); }, 3000);
    }

}


///////////////////////////////////////////////////
//  Arma Objeto   oListPrintCheckModels         //
//////////////////////////////////////////////////
function setDataPrintAssigmentCheck() {
    var rowid = $("#CheckPrinting").UifDataTable("getSelected");

    if (rowid != null) {
        var bankName = $("#BankNameCheckPrinting option:selected").html();
        var accountCurrentNumber = $("#AccountNumberCheckPrinting option:selected").html();
        var currencyName = $("#CurrencyCheckPrinting").val();
        for (var i in rowid) {
            oPrintCheckModelPrint = {
                AddressCompany: null,
                Amount: 0,
                BeneficiaryName: null,
                CheckNumber: 0,
                CompanyName: null,
                EstimatedPaymentDate: null,
                NumberPaymentOrder: 0,
                BankName: null,
                AccountCurrentNumber: null,
                CurrencyName: null,
                CheckPaymentOrderCode: 0
            };

            oPrintCheckModelPrint.AddressCompany = rowid[i].AddressCompany;
            oPrintCheckModelPrint.Amount = parseFloat(ClearFormatCurrency(rowid[i].Amount.replace("", ",")));
            oPrintCheckModelPrint.CheckNumber = rowid[i].CheckNumber;
            oPrintCheckModelPrint.BeneficiaryName = rowid[i].BeneficiaryName;
            oPrintCheckModelPrint.CompanyName = rowid[i].CompanyName;
            oPrintCheckModelPrint.EstimatedPaymentDate = rowid[i].EstimatedPaymentDate;
            oPrintCheckModelPrint.NumberPaymentOrder = rowid[i].NumberPaymentOrder;
            oPrintCheckModelPrint.CheckPaymentOrderCode = rowid[i].CheckPaymentOrderCode;
            oPrintCheckModelPrint.BankName = bankName;
            oPrintCheckModelPrint.AccountCurrentNumber = accountCurrentNumber;
            oPrintCheckModelPrint.CurrencyName = currencyName;


            oListPrintCheckModelPrint.PrintCheckModels.push(oPrintCheckModelPrint);
        }
    }

    return oListPrintCheckModelPrint;
}


//REFRESCAR EL GRID
function refreshGridPrint() {
    $("#CheckPrinting").jqGrid('setGridParam', { postData: { "paymentSourceId": $("#OriginPaymentCheckPrinting").val(), "estimatedPaymentDate": $("#DatePaymentCheckPrinting").val(), "numberCheck": $("#CheckNumberCheckPrinting").val(), "accountBankId": $("#AccountNumberCheckPrinting").val() } });
    $("#CheckPrinting").trigger("reloadGrid");
};



///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function setCleanFieldsPrint() {
    $("#OriginPaymentCheckPrinting").val('');
    $("#BranchCheckPrinting").val('');
    $("#BankNameCheckPrinting").val('');
    $("#AccountNumberCheckPrinting").val('');
    $("#DatePaymentCheckPrinting").val('');
    $("#CheckNumberCheckPrinting").val('');
    $("#CurrencyCheckPrinting").val('');
    var numberAssociated = 0;
    var checkBookControlId = 0;
    var currencyId = -1;
    var individualId = -1;
    var countSelect = 0;

    $("#CheckPrinting").dataTable().fnClearTable();
}




/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE ACCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////////
//  Botón  Buscar                     //
/////////////////////////////////////////
$("#SearchCheckPrinting").click(function () {
    $("#alertCheckPrinting").UifAlert('hide');
    
    $("#CheckPrintingForm").validate();
    if ($("#CheckPrintingForm").valid()) {
        
        if (($("#BankNameCheckPrinting").val() != "" && $("#BankNameCheckPrinting").val() != null) &&
            ($("#AccountNumberCheckPrinting").val() != "" && $("#AccountNumberCheckPrinting").val() != null))
        {
            $("#CheckPrinting").UifDataTable();
            var controller = ACC_ROOT + "CheckControl/GetPrintCheck?paymentSourceId=" + $('#OriginPaymentCheckPrinting').val() +
                             "&estimatedPaymentDate=" + $('#DatePaymentCheckPrinting').val() + "&numberCheck=" + $('#CheckNumberCheckPrinting').val() +
                             "&accountBankId=" + $('#AccountNumberCheckPrinting').val();
            $("#CheckPrinting").UifDataTable({ source: controller });
        }
        else
        {
            $("#alertCheckPrinting").UifAlert('show', Resources.SelectBranchBankAssociate, "warning");
        }


    }

});

/////////////////////////////////////////
//  Botón  Aceptar                     //
/////////////////////////////////////////
$("#AcceptCheckPrinting").click(function () {
    var rowid = $("#CheckPrinting").UifDataTable("getSelected");
    if (rowid != null) {
        loadReportPrint();
    } else {
        $("#alertCheckPrinting").UifAlert('show', Resources.WarningSelectedPaymentOrder, "warning");
    }
});


/////////////////////////////////////////
//  Botón  Cancelar                    //
/////////////////////////////////////////
$("#CancelCheckPrinting").click(function () {
    showConfirmPrint();
});


//Valida que no ingresen una fecha invalida.
$("#DatePaymentCheckPrinting").blur(function () {
    $("#alertCheckPrinting").UifAlert('hide');
    if ($("#DatePaymentCheckPrinting").val() != '') {
        if (IsDate($("#DatePaymentCheckPrinting").val()) == true) {
            if (CompareDates($("#DatePaymentCheckPrinting").val(), getCurrentDate()) == 2) {
                $("#DatePaymentCheckPrinting").val(getCurrentDate);
            }
        }
        else {
            $("#alertCheckPrinting").UifAlert('show', Resources.InvalidDates, "warning");
            setTimeout(function () {
                $("#alertCheckPrinting").UifAlert('hide');
            }, 3000);
            $("#DatePaymentCheckPrinting").val("");
        }
    }

});

///////////////////////////////////////////////////////
///  Sucursal                                      ///
///////////////////////////////////////////////////////
$('#BranchCheckPrinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckPrinting").UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + selectedItem.Id;
        $("#BankNameCheckPrinting").UifSelect({ source: controller });
    }
    CleanCheckPrintingFunction();
});


///////////////////////////////////////////////////////
///  Bancos                                        ///
///////////////////////////////////////////////////////
$('#BankNameCheckPrinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckPrinting").UifAlert('hide');
    //if (selectedItem.Id > 0) {
    if ($('#BankNameCheckPrinting').val() != "" && $('#BankNameCheckPrinting').val() != null) {
        var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + selectedItem.Id + "&branchId=" + $('#BranchCheckPrinting').val();
        $("#AccountNumberCheckPrinting").UifSelect({ source: controller });
    } 
});


///////////////////////////////////////////////////////
///  Cuentas bancarias                              ///
///////////////////////////////////////////////////////
$('#AccountNumberCheckPrinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckPrinting").UifAlert('hide');

    if ($("#AccountNumberCheckPrinting").val() != "" && $("#AccountNumberCheckPrinting").val()!= null) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "CheckControl/GetAccountBankByAccountBankId",            
            data: { "accountBankId": $("#AccountNumberCheckPrinting").val() },
            success: function (data) {
                if (data.length > 0) {
                    $("#CurrencyCheckPrinting").val(data[0].CurrencyName);
                    var currencyId = data[0].CurrencyId;
                } else {
                    $("#CurrencyCheckPrinting").val("");
                    var currencyId = -1;
                }
            }
        });
    }
 
});

//Oculta alert al perder/ganar foco
$('#OriginPaymentCheckPrinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckPrinting").UifAlert('hide');

});



///////////////////////////////////////////////////
//  Botón limpiar - Limpia campos del formulario //
//////////////////////////////////////////////////
$("#CleanCheckPrinting").click(function () {
    CleanCheckPrintingFunction();
    $("#BranchCheckPrinting").val('');
});


function CleanCheckPrintingFunction() {
    $("#CheckPrinting").dataTable().fnClearTable();
    $("#BankNameCheckPrinting").val('');
    $("#AccountNumberCheckPrinting").val('');
    $("#OriginPaymentCheckPrinting").val('');
    $("#DatePaymentCheckPrinting").val('');
    $("#CheckNumberCheckPrinting").val('');
    $("#CurrencyCheckPrinting").val('');
    $("#alertCheckPrinting").UifAlert('hide');
    $("#BankNameCheckPrinting").UifSelect("disabled", true);
    $("#AccountNumberCheckPrinting").UifSelect("disabled", true);
}
function GetBanksPrint() {
    if ($('#BranchCheckPrinting').val() > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchCheckPrinting').val();
        $("#BankNameCheckPrinting").UifSelect({ source: controller });
    }
};

var switchSelect = true;

$("#CheckPrinting").on('rowDelete', function (event, data, position) {        
    selectItemCheck(data.CheckNumber, false);
    switchSelect = false;
    $('#CheckPrinting').UifDataTable('deleteRow', position);
});

//$('body').delegate('#CheckPrinting tbody tr', "click", function (event, data, position) {   
//    var checkNumber = $(this).closest("tr").children("td")[0];
//    checkNumber = $(checkNumber).text();
//    if (switchSelect) {
//        selectItemCheck(checkNumber, true);
//    }
//    switchSelect = true;
//});

$("#CheckPrinting").on('rowSelected', function (event, data, index) {
    if (switchSelect) {
        selectItemCheck(data.CheckNumber, true);
    }
    switchSelect = true;
});


$("#CheckPrinting").on('rowDeselected', function (event, data, index) {
    if (switchSelect) {
        selectItemCheck(data.CheckNumber, false);
    }
    switchSelect = true;
});


///////////////////////////////////////////////////
//  Chequea / Deschequea op de la grilla según   //
//  el No. de Cheque                             //
///////////////////////////////////////////////////
function selectItemCheck(checkNumber, select) {
    
    var rows = $("#CheckPrinting").UifDataTable("getData");  
    var value = { label: "CheckNumber", values: [checkNumber] }
    if (select) {
        $("#CheckPrinting").UifDataTable('setSelect', value);
    }
    else {
        $("#CheckPrinting").UifDataTable('setUnselect', value);
    }
}