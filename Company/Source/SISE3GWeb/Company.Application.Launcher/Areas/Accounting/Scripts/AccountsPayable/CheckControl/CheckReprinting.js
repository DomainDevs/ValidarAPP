/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var oListPrintCheckModel = {
    PrintCheckModels: []
};

var oPrintCheckModel = {
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

if ($("#ViewBagBranchDisableReprinting").val() == "1") {
    setTimeout(function () {
        $("#BranchCheckReprinting").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchCheckReprinting").removeAttr("disabled");
}

setTimeout(function () {
    GetBanksReprint();
}, 2000);

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
///////////////////////////////////////////////////
function showConfirmReprint() {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.MainPathCheckReprinting }, function (result) {
        if (result) {
            setCleanFieldsReprint();
        }

    });
};


//Carga reporte
function loadReportReprint() {
    
    lockScreen();
    setTimeout(function () {     

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "CheckControl/LoadReportPrintCheck",
        data: JSON.stringify({ "printCheckModel": setDataReprintAssigmentCheck() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function () {
            unlockScreen();
            rePrintCheck();
            $("#alertCheckReprinting").UifAlert('show', Resources.WarningPrintCheckSucces, "success");
            setDataEmptyPrintAssigmentRePrint();
            setCleanFieldsReprint();
        }
        });
    }, 500);
}


///////////////////////////////////////////////////
//  Limpia Objeto   oListPrintCheckModel        //
//////////////////////////////////////////////////
function setDataEmptyPrintAssigmentRePrint() {
    oListPrintCheckModel = {
        PrintCheckModels: []
    };
}


///////////////////////////////////////////////////
//  Arma Objeto   oListPrintCheckModels          //
///////////////////////////////////////////////////
function setDataReprintAssigmentCheck() {
    var rowid = $("#CheckReprinting").UifDataTable("getSelected");

    if (rowid != null) {
        var bankName = $("#BankNameCheckReprinting option:selected").html();
        var accountCurrentNumber = $("#AccountNumberCheckReprinting option:selected").html();
        var currencyName = $("#CurrencyCheckReprinting").val();
        for (var i in rowid) {
            oPrintCheckModel = {
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

            oPrintCheckModel.AddressCompany = rowid[i].AddressCompany;
            oPrintCheckModel.Amount = parseFloat(ClearFormatCurrency(rowid[i].Amount.replace("", ",")));
            oPrintCheckModel.CheckNumber = rowid[i].CheckNumber;
            oPrintCheckModel.BeneficiaryName = rowid[i].BeneficiaryName;
            oPrintCheckModel.CompanyName = rowid[i].CompanyName;
            oPrintCheckModel.EstimatedPaymentDate = rowid[i].EstimatedPaymentDate;
            oPrintCheckModel.NumberPaymentOrder = rowid[i].NumberPaymentOrder;
            oPrintCheckModel.CheckPaymentOrderCode = rowid[i].CheckPaymentOrderCode;
            oPrintCheckModel.BankName = bankName;
            oPrintCheckModel.AccountCurrentNumber = accountCurrentNumber;
            oPrintCheckModel.CurrencyName = currencyName;


            oListPrintCheckModel.PrintCheckModels.push(oPrintCheckModel);
        }
    }

    return oListPrintCheckModel;
}

//REFRESCAR EL GRID
function refreshGridReprint() {
    $("#CheckReprinting").jqGrid('setGridParam', { postData: { "paymentSourceId": $("#OriginPaymentCheckReprinting").val(), "estimatedPaymentDate": $("#DatePaymentCheckReprinting").val(), "numberCheck": $("#CheckNumberCheckReprinting").val(), "accountBankId": $("#AccountNumberCheckReprinting").val() } });
    $("#CheckReprinting").trigger("reloadGrid");
};


///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function setCleanFieldsReprint() {
    $("#OriginPaymentCheckReprinting").val('');
    $("#BranchCheckReprinting").val('');
    $("#BankNameCheckReprinting").val('');
    $("#AccountNumberCheckReprinting").val('');
    $("#CheckNumberFromCheckReprinting").val('');
    $("#CheckNumberToCheckReprinting").val('');
    $("#CurrencyCheckReprinting").val('');
    /*numberAssociated = 0;
    checkBookControlId = 0;
    currencyId = -1;
    individualId = -1;
    countSelect = 0;*/

    $("#CheckReprinting").dataTable().fnClearTable();
}

/*
    * Permite imprimir el cheque en el cliente
    */
function rePrintCheck() {

    var printCommand = '<object id="PrintCommandObject" name="printObject" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>';
    var scriptChrome = "<script language='javascript1.2' > function printChrome(){;var p=window.document.getElementById(\"framePDF\") ;";
    scriptChrome += "  p.contentWindow.print();} <\/script>";
    var scriptIe = "<script language='javascript1.2' > function printIe(){;var p=window.document.getElementById(\"framePDF\") ;";
    scriptIe += " p.setActive(); 	p.print(); } <\/script>";

    var printIe = '<link rel=\"alternate\" media=\"print\"  type="application/pdf" href=\'' + ACC_ROOT + 'Content/file.pdf\" \/>';
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
        printCommandObject = '<object data=' + ACC_ROOT + 'Content/file.pdf name = "framePDF"  id="framePDF" type="application/pdf"  width="0" height="0" ></object>';
        document.body.insertAdjacentHTML('beforeEnd', printCommandObject);

        var p = window.document.getElementById("framePDF");
        p.focus();
        window.setTimeout(function () { p.print(); }, 3000);
    }

}

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE ACCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/


///////////////////////////////////////////////////////
///  Sucursal                                      ///
///////////////////////////////////////////////////////
$('#BranchCheckReprinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckReprinting").UifAlert('hide');
    
    if ($('#BranchCheckReprinting').val() != "" && $('#BranchCheckReprinting').val() != null ) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + selectedItem.Id;
        $("#BankNameCheckReprinting").UifSelect({ source: controller });
    }

});

///////////////////////////////////////////////////////
///  Bancos                                        ///
///////////////////////////////////////////////////////
$('#BankNameCheckReprinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckReprinting").UifAlert('hide');
    if ($('#BankNameCheckReprinting').val() != "" && $('#BankNameCheckReprinting').val() != null) {
        var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + selectedItem.Id + "&branchId=" + $('#BranchCheckReprinting').val();
        $("#AccountNumberCheckReprinting").UifSelect({ source: controller });
    }
    CleanCheckReprintingFunction();
});


///////////////////////////////////////////////////////
///  Cuentas bancarias                              ///
///////////////////////////////////////////////////////
$('#AccountNumberCheckReprinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckReprinting").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "CheckControl/GetAccountBankByAccountBankId",
        data: { "accountBankId": ($("#AccountNumberCheckReprinting").val() == "") ? -1 : $("#AccountNumberCheckReprinting").val() },
        success: function (data) {
            if (data.length > 0) {
                $("#CurrencyCheckReprinting").val(data[0].CurrencyName);
                //currencyId = data[0].CurrencyId;
            } else {
                $("#CurrencyCheckReprinting").val("");
                //currencyId = -1;
            }
        }
    });

    refreshRePrintChecks();
});


//Oculta alert al perder/ganar foco
$('#OriginPaymentCheckReprinting').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckReprinting").UifAlert('hide');

});


//Oculta alert al perder/ganar foco
$('#CheckNumberFromCheckReprinting').blur(function () {
    $("#alertCheckReprinting").UifAlert('hide');
});


/////////////////////////////////////////
//  Botón  Buscar                     //
/////////////////////////////////////////
$("#SearchCheckReprinting").click(function () {

    refreshRePrintChecks();
});


function refreshRePrintChecks() {
    $("#alertCheckReprinting").UifAlert('hide');
    $("#CheckReprintingForm").validate();
    if ($("#CheckReprintingForm").valid()) {
        if (($("#BankNameCheckReprinting").val() != "" && $("#BankNameCheckReprinting").val() != null) && (
            $("#AccountNumberCheckReprinting").val() != "" && $("#AccountNumberCheckReprinting").val() != null))  {
            $("#CheckReprinting").UifDataTable();
            var controller = ACC_ROOT + "CheckControl/GetReprintCheck?paymentSourceId=" + $('#OriginPaymentCheckReprinting').val() +
                             "&accountBankId=" + $('#AccountNumberCheckReprinting').val() + "&checkFrom=" + $('#CheckNumberFromCheckReprinting').val() +
                "&checkTo=" + $('#CheckNumberToCheckReprinting').val();

            $("#CheckReprinting").UifDataTable({ source: controller });
        }
        else {
            $("#alertCheckReprinting").UifAlert('show', Resources.SelectBranchBankAssociate, "warning");
        }
    }
}



/////////////////////////////////////////
//  Botón  Aceptar                     //
/////////////////////////////////////////
$("#AcceptCheckReprinting").click(function () {
    var rowid = $("#CheckReprinting").UifDataTable("getSelected");
    if (rowid != null) {
        loadReportReprint();
    }
    else {
        $("#alertCheckReprinting").UifAlert('show', Resources.WarningSelectedPaymentOrder, "warning");
    }
});

/////////////////////////////////////////
//  Botón  Cancelar                    //
/////////////////////////////////////////
$("#CancelCheckReprinting").click(function () {
    showConfirmReprint();
});



///////////////////////////////////////////////////
//  Botón limpiar - Limpia campos del formulario //
//////////////////////////////////////////////////
$("#CleanCheckReprinting").click(function () {
    CleanCheckReprintingFunction();
    $("#BranchCheckReprinting").val('');
});


function CleanCheckReprintingFunction() {
    $("#CheckReprinting").dataTable().fnClearTable();
    $("#AccountNumberCheckReprinting").val('');
    $("#OriginPaymentCheckReprinting").val('');
    $("#CheckNumberFromCheckReprinting").val('');
    $("#CheckNumberToCheckReprinting").val('');
    $("#CurrencyCheckReprinting").val('');
    $("#alertCheckReprinting").UifAlert('hide');
}


function GetBanksReprint() {
    if ($('#BranchCheckReprinting').val() > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchCheckReprinting').val();
        $("#BankNameCheckReprinting").UifSelect({ source: controller });
    }
};



var switchSelect = true;

$("#CheckReprinting").on('rowDelete', function (event, data) {        
    selectItemCheck(data.CheckNumber, false);
    switchSelect = false;
});

$('body').delegate('#CheckReprinting tbody tr', "click", function (event, data, position) {   
    var checkNumber = $(this).closest("tr").children("td")[0];
    checkNumber = $(checkNumber).text();
    if (switchSelect) {
        selectItemCheck(checkNumber, true);
    }
    switchSelect = true;
});


///////////////////////////////////////////////////
//  Chequea / Deschequea op de la grilla según   //
//  el No. de Cheque                             //
///////////////////////////////////////////////////
function selectItemCheck(checkNumber, select) {
    
    var rows = $("#CheckReprinting").UifDataTable("getData");  

    for (var j in rows) {
        var item = rows[j];

        if (item.CheckNumber == checkNumber) {
            var value = { label: "CheckNumber", values: [checkNumber] }

            if (select) {
                $("#CheckReprinting").UifDataTable('setSelect', value);
            }
            else {
                $("#CheckReprinting").UifDataTable('setUnselect', value);
            }

        }
    }
}