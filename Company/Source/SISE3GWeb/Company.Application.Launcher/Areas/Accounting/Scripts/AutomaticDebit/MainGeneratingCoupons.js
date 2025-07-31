var time;
var time2;
var processType = 0;

$(() => {
    new MainGeneratingCoupons();
});

class MainGeneratingCoupons extends Uif2.Page
{
    

    getInitialState()
    {
        $('#processMessage').hide();
    }

    bindEvents()
    {
        $("#selectBranch").on("binded", this.BindedBranchDefault);
        $('#btnGenerateCoupon').on('click', this.generateCoupon);
        $('#btnGenerateModal').on('click', this.generateModal);
        $('#selectNet').on('itemSelected', this.selectNetItem);
        $('#btnCancel').on('click', this.clearAll);
        $("#SelectLotStatus").on('itemSelected', this.selectLotStatusItem);
    }

    /**
        * Setea la sucursal por default una vez que esta cargado.
        *
        */
    BindedBranchDefault() {
        if ($("#ViewBagBranchDisable").val() == "1") {
            $("#selectBranch").attr("disabled", "disabled");
        }
        else {
            $("#selectBranch").removeAttr("disabled");
        }
    }

    generateCoupon()
    {
        $("#formCoupons").validate();

        if ($("#formCoupons").valid()) {
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
    }

    generateModal() 
    {
        $('#modalSave').modal('hide');
        $("#btnGenerateCoupon").attr("disabled", "disabled");
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        var branchCode = $('#selectBranch').val() == "" ? 0 : $('#selectBranch').val();
        var policyNumber = $('#PolicyNumber').val() == "" ? 0 : $('#PolicyNumber').val();
        var prefixCode = $('#selectPrefix').val() == "" ? 0 : $('#selectPrefix').val();
        var retryCount = $('#RetryCount').val() == "" ? -999 : $('#RetryCount').val();
        
        if ($('#selectNet').val() != "") {
            if ($('#SendDateTime').val() != "") {
                $('#processMessage').show();
                var netId = $('#selectNet').val() == "" ? -999 : $('#selectNet').val();
                var sendDateTime = $('#SendDateTime').val();
                var couponsGenerated = generateCoupons(netId, branchCode, prefixCode, policyNumber, sendDateTime, retryCount);

                couponsGenerated.then(function (data) {
                    if (data[0].LotNumber > 0) {

                        $("#SendDateTime").val(GetCurrentDateTime());
                        var setTimePromise = setDateTime(netId, sendDateTime);
                        setTimePromise.then(function (data) {
                            $("#LotNumber").val(data);
                            generatingCouponRefreshGrid();
                            processType = 0;
                            time = window.setInterval(IntervalProcessCupon, 8000);
                            time2 = time;
                            //generatingCouponRefreshGrid();
                            $("#alert").UifAlert('show', Resources.MessageProcessGenerateCoupons, "success");
                        });
                    }
                    else {
                        $("#alert").UifAlert('show', data[0].MessageError, "danger");
                    }
                    $('#processMessage').hide();
                    $("#btnGenerateCoupon").removeAttr("disabled");

                    setTimeout(function () {
                        $("#alert").UifAlert('hide');
                    }, 5000);
                });
            }
            else {
                $("#alert").UifAlert('show', Resources.EntrySendDateTime, "danger");
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.SelectNet, "danger");
        }

        setTimeout(function () {
            $("#alert").UifAlert('hide');
        }, 3000);
    }

    selectNetItem(event, selectedItem) 
    {
        if (selectedItem.Id > 0) {
            $("#SelectLotStatus").val("");
            $("#tableProcess").UifDataTable('clear');
            $("#SendDateTime").val(GetCurrentDateTime());
            var sendDateTime = $('#SendDateTime').val();
            
            var setTimePromise = setDateTime(selectedItem.Id, sendDateTime);
            setTimePromise.then(function (data) {
                $("#LotNumber").val(data);
            });

            IntervalProcessCupon();
            processType = 3;
            time = window.setInterval(IntervalProcessCupon, 8000);
            time2 = time;
            //IntervalProcessCupon();
        }
        else {
            $("#LotNumber").val("");
        }
    }

    selectLotStatusItem(event, selectedItem)
    {
        $("#formCoupons").validate();
        if ($("#formCoupons").valid()) {
            refreshGridLotByStatus(parseInt($("#selectNet").val()), 1, selectedItem.Id);
            return true;
        } else {
            $("#SelectLotStatus").val("");
            return false;
        }
    }

    clearAll() 
    {
        $('#selectNet').val("");
        $('#selectBranch').val("");
        $("#selectPrefix").val("");
        $("#PolicyNumber").val("");
        $("#SendDateTime").val("");
        $("#LotNumber").val("");
        $("#RetryCount").val("");
        $("#SelectLotStatus").val("");
    }
}


function generateCoupons(netId, branchId, prefixId, policyNumber, sendDateTime, retryCount) {
    return $.ajax({
        type: 'POST',
        url: ACC_ROOT + "AutomaticDebit/GenerateCoupons",
        data: JSON.stringify({ "netId": netId, "branchCode": branchId, "prefixCode": prefixId, "policyNumber": policyNumber, "sendDateTime": sendDateTime, "retryCount": retryCount }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
}

function setDateTime(netId, sendDateTime) {
    return $.ajax({
        type: 'POST',
        url: ACC_ROOT + "Common/GenerateLotNumber",
        data: JSON.stringify({ "netId": netId, "sendDateTime": sendDateTime }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
}

function GetCurrentDateTime() {
    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();
    var hour = currentDate.getHours();
    var minute = currentDate.getMinutes();
    var second = currentDate.getSeconds();

    if (month < 10) {
        month = '0' + month;
    }

    if (day < 10) {
        day = '0' + day;
    }

    if (hour < 10) {
        hour = '0' + hour;
    }

    if (minute < 10) {
        minute = '0' + minute;
    }

    return (day + "/" + month + "/" + year + " " + hour + ":" + minute);
}

function IntervalProcessCupon() {
    var process = $('#tableProcess').UifDataTable('getData');

    // Se valida que no se esten ejecutando procesos
    if (process.length == 0) {
        generatingCouponRefreshGrid();
    }
    else {

        if (validateProcessGenerationCoupon()) {
            clearInterval(time2);
        }
        else {
            time2 = time;
        }
        generatingCouponRefreshGrid();
    }

    /*
    if (processType >= 5)
    {
        if (validateProcessGenerationCoupon()) {
            clearInterval(time2);
        }
        else {
            time2 = time;
            generatingCouponRefreshGrid();
        }
    }
    else
    {
        generatingCouponRefreshGrid();
        processType = processType + 1;
    }
    */
}

function generatingCouponRefreshGrid() {
    var controller = ACC_ROOT + "Common/GetDebitPendingProcess?bankNetworkId=" + parseInt($("#selectNet").val()) + "&processTypeId=1";
    $("#tableProcess").UifDataTable({ source: controller });
}

function validateProcessGenerationCoupon() {
    var returns = true;
    var process = $('#tableProcess').UifDataTable('getData');
    
    for (var i in process) {

        var progresNum = process[i].Progress.split(" ");
        var end = -1;
        if (progresNum[0] != "100.00") {
            returns = false;
            break;
        }
    }
    return returns;
}

function refreshGridLotByStatus(netId, processTypeId, statusId) {
    var controller = ACC_ROOT + "AutomaticDebit/GetLotByStatus?bankNetworkId=" + netId + "&processTypeId=" + processTypeId + "&statusId=" + statusId;
    $("#tableProcess").UifDataTable({ source: controller });
}