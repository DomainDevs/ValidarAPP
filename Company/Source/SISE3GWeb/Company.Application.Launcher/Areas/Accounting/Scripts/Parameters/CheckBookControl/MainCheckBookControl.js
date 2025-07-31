var accountBank;
var checkFrom;
var checkTo;

var idChek;
var checkFromControl;
var accountBankId;
var lastCheck;
var validation = false;
//const mainCheckBookControlRequest = new MainCheckBookControlRequest();

var modelUpdate = {
    Id: 0,
    CheckFrom: 0,
    CheckTo: 0,
    LastCheck: 0,
    StatusId: 0,
    DisabledDate: "",
    BankId: 0,
    BranchId: 0,
    AccountBankId: 0
};


var modelMainCheck = {
    Id: 0,
    CheckFrom: 0,
    CheckTo: 0,
    LastCheck: 0,
    StatusId: 0,
    DisabledDate: "",
    BankId: 0,
    BranchId: 0,
    AccountBankId: 0
};


if ($("#ViewBagBranchDisableMainCheckBook").val() == "1") {
    setTimeout(function () {
        $("#BranchSelect").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchSelect").removeAttr("disabled");
}

setTimeout(function () {
    GetBanksCheckBook();
}, 2000);

$(document).ready(function () {
    $('#cancelSave').click(function () {
        $('#addItem').hide();
        $('#addItem form').formReset();
    })
    $('#checkBookListView').on('rowAdd', function () {
        debugger
        $('#addItem').show();
        var listado = $('#checkBookListView').UifListView('getData');
        var lastItem = listado[listado.length - 1];
        if (listado.length > 0) {
            var items = listado.filter(function (elem) { return elem.LastCheck > 0 })
            if (items.length > 0) {
                $('#IsAutomaticId').UifSelect('disabled', true)
                $('#IsAutomaticId').val(items[0].IsAutomaticId);
            } else if (lastItem.StatusId == 2) {

                $('#IsAutomaticId').UifSelect('disabled', false)
                $('#IsAutomaticId').val('');

            } else {

                $("#alertCheckBook").UifAlert('show', Resources.LastCheckWarning, "danger");
                $('#addItem').hide();
                $('#addItem form').formReset();
            }
        } else {
            $('#IsAutomaticId').UifSelect('disabled', false)
            $('#IsAutomaticId').val('');
        }
        
    });
    $('#addItem').click(function () {
        if ($('#addItem form').valid()) {
            var deferred = jQuery.Deferred();
            var promise = deferred.promise();
            var data = $('#addItem form').serializeObject();
            saveCallbackCheck(deferred, data)
            promise.done(function (data) {
                if (data !== undefined) {
                    $('#checkBookListView').UifListView('addItem', data);
                }
                $('#addItem').hide();
                $('#addItem form').formReset();
            });
        }
    })
    $('[data-toggle="tooltip"]').tooltip();
    
    var saveCallbackCheck = function (deferred, data) {        
        $("#alertCheckBook").UifAlert('hide');

        MainCheckBookControlRequest.GetCheckBookControls($('#AccountBankSelect').val()).done(function (dataBank) {
            for (var i = 0; i < dataBank.length; i++) {
                if (data.CheckFrom >= dataBank[i].CheckFrom && data.CheckFrom <= dataBank[i].CheckTo || data.CheckTo >= dataBank[i].CheckFrom && data.CheckTo <= dataBank[i].CheckTo) {
                    //$("#alertCheckBook").UifAlert('show', Resources.RangeCheck, "warning");
                    validation = true;
                }
            }
        });

        if (!validation) {
            if (parseInt(data.CheckFrom, 10) > 0) {

                if (parseInt(data.CheckTo, 10) > 0) {

                    if (parseInt(data.CheckFrom, 10) < parseInt(data.CheckTo, 10)) {

                        $.ajax({
                            type: "POST",
                            url: ACC_ROOT + "Parameters/SaveCheckBookControl",
                            data: JSON.stringify({ "model": data, "accountBankId": $('#AccountBankSelect').val() }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8"
                        }).done(function (data) {
                            //CD INICIO
                            if (data.Id == 0) {
                                $("#alertCheckBook").UifAlert('show', Resources.StatusDuplicated, "warning");
                            }
                            deferred.resolve(data);
                            //CD FIN

                            if (data.Id == -1) {

                                modelUpdate.CheckFrom = data.CheckFrom;
                                modelUpdate.CheckTo = data.CheckTo;
                                modelUpdate.Id = 1;
                                modelUpdate.BankId = $('#AccountBankSelect').val();
                                modelUpdate.DisabledDate = getDateCheckBook();
                                modelUpdate.AccountBankId = data.AccountBank.Id;
                                accountBank = data.AccountBank.Id;
                                checkFrom = data.CheckFrom;
                                checkTo = data.CheckTo;

                                $('#MessageHeader').text(Resources.RangeCheckBookControl + ' ' + checkFrom + ' ' + Resources.ToCheckBookControl + ' ' + checkTo + ' ' + Resources.CheckbookActive);
                                $('#MessageBody').text(Resources.DeactivateAccount);
                                $('#modalCheckBookControl').UifModal('showLocal', '');

                            } else {
                                if (data.Id == -2) {
                                    $("#alertCheckBook").UifAlert('show', Resources.AnotherCheckbook);
                                } else {
                                    if (data.Id == -3) {
                                        $("#alertCheckBook").UifAlert('show', Resources.RangeCheck);
                                    } else {
                                        if (data.Id == -4) {
                                            $("#alertCheckBook").UifAlert('show', Resources.SaveSuccessfully, "success");
                                            deferred.resolve(data);
                                        } else {
                                            if (data.Id == -5) {
                                                $("#alertCheckBook").UifAlert('show', Resources.CheckbookInactive, "success");
                                            } else {
                                                if (data.Id == -6) {
                                                    $("#alertCheckBook").UifAlert('show', Resources.CheckbookActive);
                                                } else {
                                                    if (data.Id == -7) {
                                                        $("#alertCheckBook").UifAlert('show', Resources.RangeCheck);
                                                    } else {
                                                        if (data.Id == -8) {
                                                            $("#alertCheckBook").UifAlert('show', Resources.EditSuccessfully, "success");
                                                            $('#modalCheckBookControl').UifModal('showLocal', '');

                                                            deferred.resolve(data);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (data != null) {
                                refreshCheckBookListView();
                            }
                        });

                    } else {
                        $("#alertCheckBook").UifAlert('show', Resources.ValidateCheckbookRange, "warning");
                    }
                } else {
                    $("#alertCheckBook").UifAlert('show', Resources.ValidateCheckbookEndNumber, "warning");
                }
            } else {
                $("#alertCheckBook").UifAlert('show', Resources.ValidateCheckbookStartNumber, "warning");
            }
        }
        else {
            $("#alertCheckBook").UifAlert('show', Resources.RangeCheck, "warning");
        }

            
        
        
        hideAlertCheckBook();        
    };
    

$('#BranchSelect').on('itemSelected', function (event, selectedItem) {
    $("#AccountBankSelect").UifSelect();
    $("#alertCheckBook").UifAlert('hide');
    if ($('#BranchSelect').val() > 0) {

        var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelect').val();
        $("#BankSelectCheckBook").UifSelect({ source: controller });

        setTimeout(function () {
            refreshCheckBookListView();    
        }, 1000);        
    } else {
        $("#AccountBankSelect").UifSelect();
        $("#BankSelectCheckBook").UifSelect();

        $("#checkBookListView").find('.cancel-button').click(); 
        $("#checkBookListView").UifListView(
        {
            source: null
        });
        $('.add-button').hide();        
    }
});

$('#BankSelectCheckBook').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckBook").UifAlert('hide');
    if ($('#BankSelectCheckBook').val() !="") {

        var controller = ACC_ROOT + "Parameters/GetAccountByBranchIdBankId?branchId="
                    + $('#BranchSelect').val() + '&bankId=' + $('#BankSelectCheckBook').val();

        $("#AccountBankSelect").UifSelect({ source: controller });

    } else {
        $("#AccountBankSelect").UifSelect();
        $("#checkBookListView").find('.cancel-button').click();
        $("#checkBookListView").UifListView(
        {
            source: null
        });
        $('.add-button').hide();        
    }

    refreshCheckBookListView();
});

 $('#AccountBankSelect').on('itemSelected', function (event, selectedItem) {
     validation = false;
    $("#alertCheckBook").UifAlert('hide');
    $("#checkBookListView").find('.cancel-button').click();
    
    if ($('#AccountBankSelect').val() > 0) {
        
        refreshCheckBookListView();
    } else {
        $("#checkBookListView").find('.cancel-button').click();
        $("#checkBookListView").UifListView(
        {
            source: null
        });
        $('.add-button').hide();        
    }
});


$("#checkBookListView").on("rowEdit", function (event, data, position) {
    $("#alertCheckBook").UifAlert('hide');

    idChek = data.Id;
    checkFromControl = data.CheckFrom;
    accountBankId = data.AccountBankId;
    lastCheck = data.LastCheck;

    bindgCheckBook(data.StatusId);
    $('#modalCheckBook').UifModal('showLocal', Resources.EditRecord);
});


$("#CheckBookRefresh").click(function () {
    $("#alertCheckBook").UifAlert('hide'); 
    $("#checkBookListView").UifListView("refresh");
});

$("#rptCheck").click(function () {

    $("#rptCheck").attr("disabled", false);
    $("#alertCheckBook").UifAlert('hide');
    var branchId = $("#BranchSelect").val();
    var bankId = $("#BankSelectCheckBook").val();

    window.open(ACC_ROOT + "Parameters/ShowCheckBookControlReport?branchId="
        + branchId + '&bankId=' + bankId, 'mywindow', 'fullscreen=yes, scrollbars=auto');

});

////Modal CheckBookControl
$("#modalCheckBook").find('#status').on('itemSelected', function (event, selectedItem) {
    $("#modalCheckBook").find("#alertModel").UifAlert('hide');
    if ($("#modalCheckBook").find('#status').val() == 1 || $("#modalCheckBook").find('#status').val() == null || $("#modalCheckBook").find('#status').val() == "") {
        $("#modalCheckBook").find("#DisabledDate").attr("disabled", "disabled");
        $("#modalCheckBook").find("#DisabledDate").val("");
    }
    else {
        $("#modalCheckBook").find("#DisabledDate").attr("disabled", false);
    }
});

$("#modalCheckBook").find("#saveButton").click(function () {

    $("#modalCheckBook").find("#editCheckBook").validate();

    if ($("#modalCheckBook").find("#editCheckBook").valid()) {

        $("#modalCheckBook").find("#alertModel").UifAlert('hide');
        modelMainCheck.Id = idChek;
        modelMainCheck.CheckFrom = checkFromControl;
        modelMainCheck.StatusId = parseInt($("#modalCheckBook").find('#status').val());
        modelMainCheck.DisabledDate = $("#modalCheckBook").find("#DisabledDate").val();
        modelMainCheck.AccountBankId = accountBankId;
        modelMainCheck.LastCheck = lastCheck;
        var listado = $('#checkBookListView').UifListView('getData');
        var lastItem = listado.filter(function (elem) { return elem.StatusId == 1 });

        if ($("#modalCheckBook").find('#status').val() == "") {
            $("#modalCheckBook").find("#alertModel").UifAlert('show', Resources.WarningSelectState, "danger");
        }
        else if (($("#modalCheckBook").find('#status').val() == 0 || $("#modalCheckBook").find('#status').val() == 2) && $("#modalCheckBook").find("#DisabledDate").val() == "") {
            $("#modalCheckBook").find("#alertModel").UifAlert('show', Resources.DateRequired, "danger");
        }
        else if (lastItem.length > 0) {
            if (lastItem[0].StatusId == modelMainCheck.StatusId) {
                $("#modalCheckBook").find("#alertModel").UifAlert('show', Resources.StatusDuplicated, "danger");
            } else {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "Parameters/UpdateCheckBookControl",
                    data: JSON.stringify({ "model": modelMainCheck }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $('#modalCheckBook').UifModal('hide');
                        $("#checkBookListView").UifListView('refresh');
                        $("#alertCheckBook").UifAlert('show', Resources.EditSuccessfully, "success");
                        refreshCheckBookListView();
                    }
                });
            }                           
        }
        else {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "Parameters/UpdateCheckBookControl",
                data: JSON.stringify({ "model": modelMainCheck }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#modalCheckBook').UifModal('hide');
                    $("#checkBookListView").UifListView('refresh');
                    $("#alertCheckBook").UifAlert('show', Resources.EditSuccessfully, "success");
                    refreshCheckBookListView();
                }
            });
        }
    }
});
/////fin CheckBookControl

function hideAlertCheckBook() {
    setTimeout(function () { $("#alertCheckBook").UifAlert('hide'); }, 5000);    
}
    function refreshCheckBookListView() { 
        
        if ($('#AccountBankSelect').val() != "" && $('#AccountBankSelect').val() != null) {
        MainCheckBookControlRequest.GetCheckBookControls($('#AccountBankSelect').val()).done(function (data) {
        
            if (data) {
                $("#checkBookListView").UifListView(
                    {
                        sourceData: data,
                        customDelete: false,
                        customAdd: true,
                        customEdit: true,
                        add: true,
                        edit: true,
                        delete: false,
                        displayTemplate: "#listTemplate",
                        addTemplate: '#add-templateMainCheck',
                        addCallback: saveCallbackCheck
                    });

            }
        });
        
        $("#rptCheck").attr("disabled", false);
    } else {
        $("#checkBookListView").UifListView({ source: null });
        $('.add-button').hide();
        $("#rptCheck").attr("disabled", true);
    }
}
});


$("#DeleteModal").on('click', function () {

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/UpdateCheckBookControl",
        data: JSON.stringify({ "model": modelUpdate, "accountBankId": $('#AccountBankSelect').val() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        //$.each(var datos =data)
        //{ if (datos[0].checktype = 1 && datos[0].state != "anulad")
    
        //else if (datos[0].checktype = 0 && datos[0].state != "anulad")
                
        //else ( datos[0].state == "habilit " && chequeUtilizado  = chequehasta)
        //}
        if (data.Id == -1) {

            modelUpdate.CheckFrom = data.CheckFrom;
            modelUpdate.CheckTo = data.CheckTo;
            modelUpdate.Id = 1;
            modelUpdate.BankId = $('#AccountBankSelect').val();
            modelUpdate.DisabledDate = getDateCheckBook();

            accountBank = data.AccountBank.Id;
            checkFrom = data.CheckFrom;
            checkTo = data.CheckTo;

            $('#MessageHeader').text(Resources.RangeCheckBookControl + ' ' + checkFrom + ' ' + Resources.ToCheckBookControl + ' ' + checkTo + ' ' + Resources.CheckbookActive);
            $('#MessageBody').text(Resources.DeactivateAccount);
            $('#modalCheckBookControl').UifModal('showLocal', '');            
        }
        else {
            if (data.Id == -2) {
                $("#alertCheckBook").UifAlert('show', Resources.AnotherCheckbook);
            }
            else {
                if (data.Id == -3) {
                    $("#alertCheckBook").UifAlert('show', Resources.RangeCheck);
                }
                else {
                    if (data.Id == -4) {
                        $("#alertCheckBook").UifAlert('show', Resources.SaveSuccessfully, "success");
                    }
                    else {
                        if (data.Id == -5) {
                            $("#alertCheckBook").UifAlert('show', Resources.CheckbookInactive, "success");
                        }
                        else {
                            if (data.Id == -6) {
                                $("#alertCheckBook").UifAlert('show', Resources.CheckbookActive);
                            }
                            else {
                                if (data.Id == -7) {
                                    $("#alertCheckBook").UifAlert('show', Resources.RangeCheck);
                                }
                                else {
                                    if (data.Id == -8) {
                                        $("#alertCheckBook").UifAlert('show', Resources.EditSuccessfully, "success");
                                        $('#modalCheckBookControl').UifModal('showLocal', '');                                        
                                        deferred.resolve();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    });
});


///////////////////////////////////////////////////
//  Obtiene fecha del servidor                   //
//////////////////////////////////////////////////
function getDateCheckBook() {
    var systemDate;

    $.ajax({
        type: "GET",
        async: false,
        url: ACC_ROOT + "Common/GetDate",
        success: function (data) {
            systemDate = data;
        }
    });

    return systemDate;
}

function GetBanksCheckBook() {
    if ($('#BranchSelectCheck').val() > 0) {
        var controller = ACC_ROOT + " Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelect').val();
        $("#BranchSelectCheck").UifSelect({ source: controller });
    }
};

function bindgCheckBook(statusCode) {
    if (statusCode > 0) {
        var controller = ACC_ROOT + "Parameters/GetStatus";
        $("#modalCheckBook").find('#status').UifSelect({ source: controller, selectedId: statusCode });
    }
}