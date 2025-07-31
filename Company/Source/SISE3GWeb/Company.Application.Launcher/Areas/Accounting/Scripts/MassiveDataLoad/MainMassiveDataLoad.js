/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//                                          DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var idBillControl = 0;
var isOpen = true;
var time;


/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                          ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    
//LLAMADA AL DIALOGO APERTURA DE CAJA Y CIERRE DE CAJA


$(document).ready(function () {

    if ($("#MassiveBranch").val() != "") {

        $.ajax({
            url: ACC_ROOT + "Billing/AllowOpenBill",
            data: { "branchId": $("#MassiveBranch").val(), "accountingDate": $("#ViewBagDateAccounting").val() },
            success: function (userata) {

                if (userata[0].resp == true) {
                    OpenBillingDialogMassiveData();
                } else {
                    isOpen = true;
                }
            }
        });
    }
});
          
$("#MassiveBranch").on('itemSelected', function (event, selectedItem) {
    
    if ($("#MassiveBranch").val() != "") {
                $.ajax({
                    url: ACC_ROOT + "Billing/AllowOpenBill",
                    data: { "branchId": $("#MassiveBranch").val(), "accountingDate": $("#ViewBagDateAccounting").val() },
                    success: function (userata) {

                    if (userata[0].resp == true) {
                        OpenBillingDialogMassiveData();
                    } else {
                        isOpen = true;
                    }
                }
            });
}});  
    
//botón del diàlogo de cierre de caja            
$("#AcceptClosureBilling").click(function () {
    
    $("#modalBillingClosure").hide();
    location.href = $("#ViewBagBillingClosureIdLink").val() + "?billControlId=" + idBillControl + "&branchId=" + $("#MassiveBranch").val();
});
       
//////////////////////////////////////////
// Despliega la ventana para abrir caja //
//////////////////////////////////////////
function OpenBillingDialogMassiveData() {
    $('#modalOpeningBilling').find("#AccountingDate").val($("#ViewBagAccountingDate").val());
    $('#modalOpeningBilling').find("#UserId").val(GetUserNick());
    $('#modalOpeningBilling').UifModal('showLocal', Resources.OpeningBilling);
}
       
function GetUserNick() {
    var userNick;
    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "Common/GetUserNick",
        success: function (data) {  
            userNick = data;
        }
    });
          
    return userNick;
}       
        

//////////////////////////////////////
// Botón Aceptar - Apertura Caja    //
//////////////////////////////////////
$("#modalOpeningBilling").find('#AcceptOpening').click(function () {
    
    $("#modalOpeningBilling").modal('hide');

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/SaveBillControl",
        data: {
            "branchId": $("#MassiveBranch").val(),
            "accountingDate": $("#modalOpeningBilling").find("#AccountingDate").val()
        },
        success: function (data) {
            //billControlId = data.Id;
            idBillControl = data.result[0].Id;
            isOpen = true;
            $("#modalOpeningBilling").modal('hide');
        }
    });   
});
            
            
///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirmMassiveData() {
    $.UifDialog('confirm', { 'message': Resources.ConfirmMessageProcess, 'title': Resources.MassiveDataLoad },
       function (result) {
        if (result) {                      
             
            if (isOpen == true) {
       
                time = window.setInterval(refreshGridMassiveData, 10000);
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "MassiveDataLoad/SaveMassiveBillRequest",
                    data: { "branchId": $("#MassiveBranch").val(),"billControlId":idBillControl },
                    success: function () {
                        refreshGridMassiveData();                     

                        $("#MassiveBranch").val("");
                        $('#_fileLocation').val("");
                        $("#_infoFile").text("");                  
                    }
                });
            } else {
                              
                $("#alertDataLoad").UifAlert('show', Resources.DialogMassiveProcess, "warning");
            }                                    
        }
    });
};

    
/*-----------------------------------------------------------------------------------------------------------*/
    
$("#_fileLocation").val("");

setTimeout(function () {
    refreshGridMassiveData();
}, 500);

$("#LoadCancel").click(function () {
    $("#MassiveBranch").val("");
    $('#_fileLocation').val("");
    $("#_infoFile").text("");
    $('#uploadDataLoadFile').val("");
    $('#uploadDataLoadFile').closest("form").trigger('reset');
});

$("#LoadProcess").click(function () {
    if ($("#_fileLocation").val() == "") {            
        $("#alertDataLoad").UifAlert('show', Resources.SelectFile, "warning");
    }
    else {
        if ($("#MassiveBranch").val() == "") {
                
            $("#alertDataLoad").UifAlert('show', Resources.SelectBranch, "warning");
  
        }
        else {
            var params = {};
            if (isOpen == true) {
        
                showConfirmMassiveData();
            } 
            else {
                    
                $("#alertDataLoad").UifAlert('show', Resources.DialogMassiveProcess, "warning");         
            }
        }
    }
});  
  

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES GLOBALES                                                                       */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
function ValidateFileMassiveDataLoad() {
    var validFilesTypes = ["csv", "xml", "xls", "xlsx", "txt"];
    var file = document.getElementById("uploadDataLoadFile");
    var path = file.value;
    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
    var isValidFile = false;
    for (var i = 0; i < validFilesTypes.length; i++) {
        if (ext == validFilesTypes[i]) {
            isValidFile = true;
            break;
        }
    }
    if (!isValidFile) {
        $('#_fileLocation').val('');                        
        $("#alertDataLoad").UifAlert('show', Resources.IncorrectFileType, "warning");

    }
    return isValidFile;
}


function uploadAjaxMassiveDataLoad() {
    var inputFileImage = document.getElementById("uploadDataLoadFile");
    var file = inputFileImage.files[0];
    var data = new FormData();

    data.append('uploadedFile', file);

    var url = "ReadFileInMemory";
    $("#_fileLocation").val("");
    $("#_infoFile").text("");

    $.ajax({
        url: url,
        type: 'POST',
        contentType: false,
        data: data,
        processData: false,
        cache: false,
        success: function (data) {

            if (data == "FormatException") {
                    
                $("#alertDataLoad").UifAlert('show', Resources.WrongFormatBlankRows, "warning");
                               
            }
            else if (data == "OverflowException") {
                    
                $("#alertDataLoad").UifAlert('show', Resources.WrongFormatBlankColumns, "warning");
                    
            }
            else if (data == "IndexOutOfRangeException") {
                    
                $("#alertDataLoad").UifAlert('show', Resources.FileDoesNotHaveColumnsNeeded, "warning");
                                        
            }
            else if (data == "InvalidCastException") {
                    
                var msj = Resources.EmptyFile + " / " + Resources.FileDoesNotHaveColumnsNeeded;
                $("#alertDataLoad").UifAlert('show',msj , "warning");

            }
            else if (data == "NegativeId") {
                    
                $("#alertDataLoad").UifAlert('show', Resources.ValuesNoNegative, "warning");
            }
            else if (data == "NullReferenceException") {

                $("#alertDataLoad").UifAlert('show', Resources.FileDoesNotHaveColumnsNeeded, "warning");
                                   
            }
            else if (data == "XmlException") {
                    
                $("#alertDataLoad").UifAlert('show', Resources.ErrorInXMLTags, "warning");
                                 
            }
            else if (data == "Exception") {
                    
                $("#alertDataLoad").UifAlert('show', Resources.WrongFormatBlankRows, "warning");
                                  
            }
            else if (data == "CommaValue") {
                $("#alertDataLoad").UifAlert('show', Resources.MassiveDataLoadWrongFormatFieldAmount, "warning");

            }
            else {
                var dataSplit = data.split(";");
                if (dataSplit[1] == 0) {
                    $("#alertDataLoad").UifAlert('show', Resources.WrongFormatBlankRows, "warning");
                                      
                }
                else {

                    $("#_fileLocation").val(dataSplit[0]);

                    var lang;
                    if (typeof navigator.userLanguage != "undefined") {
                        lang = navigator.userLanguage.toUpperCase();
                    } else if (typeof navigator.language != "undefined") {
                        lang = navigator.language.toUpperCase();
                    }

                    var loc = window.location;
                    var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);

                    $("#_TotalRecords").val(dataSplit[1]);
                    //$("#_TotalRecords").currency({ region: 'USD', thousands: ',', decimal: '.', decimals: 2 });
                    $("#_TotalRecords").val($("#_TotalRecords").val().replace("$", ""));

                    $("#_TotalValueRecords").val(dataSplit[2]);
                    //$("#_TotalValueRecords").currency({ region: 'USD', thousands: ',', decimal: '.', decimals: 2 });

                    var val = 0;        
                        
                    $("#_infoFile").text(Resources.MassiveDataLoadResultMessageOne + " " + $("#_TotalRecords").val() + " "+ Resources.MassiveDataLoadResultMessageTwo + " : "+ FormatMoney($("#_TotalValueRecords").val()));
                               
                }
            }
        }
    });       
}

function ClearGridData() {
    $('#_fileLocation').val("");        
    $("#StatisticalLoadFileTable").dataTable().fnClearTable();        
}

function refreshGridMassiveData() {
  
    var control = ACC_ROOT + "MassiveDataLoad/GetMassiveProcess";
    $("#StatisticalLoadFileTable").UifDataTable({ source: control });

    setTimeout(function () {
        refreshOff();
    }, 1000);
        
};
            

function refreshOff() {                    
  
    var control = 0;      
    var ids = $("#StatisticalLoadFileTable").UifDataTable("getData");
    var totRecords = ids.length;
    for (idx in ids)
    {   
        if (ids[idx].PorcentageAdvance == 100) {
            control+=1 ;
        }           
    }
    if (totRecords == control) {

        window.clearInterval(time);
    }  
}
            

