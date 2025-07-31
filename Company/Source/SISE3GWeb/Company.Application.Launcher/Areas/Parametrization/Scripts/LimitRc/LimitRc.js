var glbLimitsRc = {};
var glbLimitsRcDelete = [];
var LimitsRc = {};
var LimitRcIndex = null;

class LimitRcQueries {

    static GetListLimitRc() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LimitRc/GetLimitsRc',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetLimitRcByDescription(LimitRcSearch) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LimitRc/GetLimitRcByDescription',
            data: JSON.stringify({ LimitRcSearch: LimitRcSearch }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    //Exportar excel
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LimitRc/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetValidationButtonUser() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LimitRc/ValidateButton',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class LimitRc extends Uif2.Page {
    
    getInitialState() {
        $("#listViewLimitRc").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: this.DeleteItemLimitRc, displayTemplate: "#LimitRcTemplate", selectionType: 'single', height: 310 });
        LimitRc.GetValidationButtonUser();
        $('#inputLimit1').ValidatorKey(ValidatorType.Onlynumbers, 3, 0);
        $('#inputLimit2').ValidatorKey(ValidatorType.Onlynumbers, 3, 0);
        $('#inputLimitUnique').ValidatorKey(ValidatorType.Onlynumbers, 3, 0);        
        LimitRcQueries.GetListLimitRc().done(function (data) {
            if (data.success) {
                LimitRc.loadLimitRc(data.result);
                glbLimitsRc = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        
    }

    bindEvents() {
        $("#inputLimit1").focusin(this.NotFormatMoneyIn);
        $("#inputLimit1").focusout(this.FormatMoneyOut);
        $("#inputLimit2").focusin(this.NotFormatMoneyIn);
        $("#inputLimit2").focusout(this.FormatMoneyOut);
        $("#inputLimitUnique").focusin(this.NotFormatMoneyIn);
        $("#inputLimitUnique").focusout(this.FormatMoneyOut);
        $('#btnSave').on('click', LimitRc.save);
        $('#btnExport').on('click', LimitRc.sendExcelLimitRc);
        $('#btnExit').click(this.exit);
        $('#listViewLimitRc').on('rowEdit', LimitRc.showData);
        //$('#listViewLimitRc').on('rowDelete', this.DeleteItemLimitRc);
        $('#btnAcceptLimitRc').on('click', LimitRc.AddItem);
        $('#btnNewLimitRc').on('click', LimitRc.clearPanel);
        $('#inputLimit1').focusout(LimitRc.Validation);
        $('#inputLimit2').focusout(LimitRc.Validation);
        $("#inputLimitUnique").focusout(LimitRc.ValidationLimitUnique)
        $('#inputLimitRcSearch').on('buttonClick', LimitRc.SearchById);
        $("#inputLimit1").on('focus', function () { $(this).select(); });
        $("#inputLimit2").on('focus', function () { $(this).select(); });
        $("#inputLimitUnique").on('focus', function () { $(this).select(); });
        
    }
    static GetValidationButtonUser() {
        
        LimitRcQueries.GetValidationButtonUser().done(function (data) {
            if (data.result) {                
                $('#btnAcceptLimitRc').hide();
                $('#btnSave').hide();
                $('#btnExport').hide();
                $('#delete-button').hide();
                $('#btnNewLimitRc').hide();
                $("#listViewLimitRc").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: false, deleteCallback: this.DeleteItemLimitRc, displayTemplate: "#LimitRcTemplate", selectionType: 'single', height: 310 });
            }
        });
    }
    NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    FormatMoneyOut() {
        $(this).val(FormatMoney($(this).val()));
    }

    static loadLimitRc(LimitRc) {       
        if (Array.isArray(LimitRc)) {
            if (LimitRc != undefined) {
                LimitRc.forEach(item => {
                    item.StatusTypeService = ParametrizationStatus.Original;
                    item.Limit1 = FormatMoney(item.Limit1);
                    item.Limit2 = FormatMoney(item.Limit2);
                    item.Limit3 = FormatMoney(item.Limit3);
                    if (item.LimitUnique === 0) {
                        item.ValidationLimit = true;
                        item.ValidationUnique = false;
                    }
                    else if (item.LimitUnique !== 0) {
                        item.ValidationLimit = false;
                        item.ValidationUnique = true;
                        item.LimitUnique = FormatMoney(item.LimitUnique);
                    };
                    $("#listViewLimitRc").UifListView("addItem", item);
                });
            }
        }

        //$("#listViewLimitRc").UifListView({ sourceData: LimitRc, add: false, edit: true, customEdit: true, delete: true, deleteCallback: this.DeleteItemLimitRc, displayTemplate: "#LimitRcTemplate", selectionType: 'single', height: 310 });
        
    }

    static clearPanel() {
        LimitRcIndex = null;
        $("#inputLimitCode").val(0);
        $("#inputLimit1").val(0);
        $("#inputLimit1").focus();
        $("#inputLimit2").val(0);
        $("#inputLimit3").val(0);
        $("#inputLimitUnique").val(0);
        $("#inputDescription").val('');
        $('#inputLimitRcSearch').val('');
        $('#inputLimit1').attr("disabled", false);
        $('#inputLimit2').attr("disabled", false);
        $('#inputLimitUnique').attr("disabled", false);
    }

    static showData(event, result, index) {
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        else if (result.length > 1) {
            LimitRc.ShowSearchAdv(result);
        }
        if (result.LimitRcCd != undefined) {
            LimitRcIndex = index;
            $("#inputLimitCode").val(result.LimitRcCd);
            $("#inputLimit1").val(result.Limit1);
            $("#inputLimit2").val(result.Limit2);
            $("#inputLimit3").val(result.Limit3);
            $("#inputLimitUnique").val(result.LimitUnique);
            $("#inputDescription").val(result.Description);
            if (parseInt(result.LimitUnique) === 0) {
                $('#inputLimit1').attr("disabled", false);
                $('#inputLimit2').attr("disabled", false);
                $('#inputLimitUnique').attr("disabled", true);
            }
            else {
                $('#inputLimit1').attr("disabled", true);
                $('#inputLimit2').attr("disabled", true);
                $('#inputLimitUnique').attr("disabled", false);
            }
            $("#inputLimitRcSearch").val('');
        }
    }

    static ShowSearchAdv(data) {
        if (data) {
            $("#listviewSearchLimitRc").UifListView(
                {
                    displayTemplate: "#LimitRcTemplate",
                    selectionType: 'single',
                    source: null,
                    height: 400
                });
            data.forEach(item => {
                $("#listviewSearchLimitRc").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvLimitRc.show();
    }

    static AddItem() {
        $("#formLimitRc").validate();
        if ($("#formLimitRc").valid()) {

            if ($("#inputLimitUnique").val()!=0) {
                $("#inputLimit1").val(0);
                $("#inputLimit2").val(0);
                $("#inputLimit3").val(0);
            }
            
            var inputLimit1 = parseInt(NotFormatMoney($("#inputLimit1").val()))
            var inputLimit2 = parseInt(NotFormatMoney($("#inputLimit2").val()))
            var inputLimitUnique = parseInt(NotFormatMoney($("#inputLimitUnique").val()))
            if ((inputLimit1 > 0 && inputLimit2 > 0) || (inputLimitUnique > 0)) {
                if (inputLimit1 <= inputLimit2) {
                    var LimitRcNew = {};
                    if ($("#inputLimitCode").val() == "") {
                        LimitRcNew.LimitRcCd = 0;
                    }
                    else {
                        LimitRcNew.LimitRcCd = parseInt($("#inputLimitCode").val());
                    }

                    LimitRcNew.Limit1 = $("#inputLimit1").val();
                    LimitRcNew.Limit2 = $("#inputLimit2").val();
                    LimitRcNew.Limit3 = $("#inputLimit3").val();
                    LimitRcNew.LimitUnique = $("#inputLimitUnique").val();
                    LimitRcNew.Description = $("#inputDescription").val();

                    if (parseInt(LimitRcNew.LimitUnique) === 0) {
                        LimitRcNew.ValidationLimit = true;
                        LimitRcNew.ValidationUnique = false;
                    }
                    else if (parseInt(LimitRcNew.LimitUnique) !== 0) {
                        LimitRcNew.ValidationLimit = false;
                        LimitRcNew.ValidationUnique = true;
                    };

                    //Si los datos a guardar venían de la busqueda avanzada, se busca el index de la lista principal
                    if (LimitRcIndex == null && LimitRcNew.LimitRcCd != 0) {
                        const findLimitRc = function (element, index, array) {
                            return element.LimitRcCd === LimitRcNew.LimitRcCd
                        }
                        LimitRcIndex = $("#listViewLimitRc").UifListView("findIndex", findLimitRc);
                    }
                    if (LimitRcIndex == null) {
                        LimitRcNew.StatusTypeService = ParametrizationStatus.Create;
                        var ifExist = $("#listViewLimitRc").UifListView('getData').filter(function (item) {
                            return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase();
                        });
                        if (ifExist.length > 0) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistLimitRc, 'autoclose': true });
                        }
                        else {
                            $("#listViewLimitRc").UifListView("addItem", LimitRcNew);
                        }
                    }
                    else {
                        LimitRcNew.StatusTypeService = ParametrizationStatus.Update;
                        var ifExist = $("#listViewLimitRc").UifListView('getData').filter(function (item) {
                            return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase();
                        });
                        if (ifExist.length > 0) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistLimitRc, 'autoclose': true });
                        }
                        $('#listViewLimitRc').UifListView('editItem', LimitRcIndex, LimitRcNew);
                    }
                    LimitRc.clearPanel();
                } else {
                    $.UifNotify('show',
                        {
                            'type': 'danger', 'message': Resources.Language.ErrorLimitRcGreaterOrEqual, 'autoclose': true
                        });
                }
            } else {
                $.UifNotify('show',
                    {
                        'type': 'danger', 'message': Resources.Language.ErrorLimitRcHigher, 'autoclose': true
                    });
            }
        };
    }

    static SearchById() {
        var description = $('#inputLimitRcSearch').val();
        var find = false;
        var data = [];
        var search = glbLimitsRc;

        $.each(search, function (key, value) {
            if ((value.LimitRcCd.toString().includes($('#inputLimitRcSearch').val()))) {
                value.key = key;
                data.push(value);
                find = true;
            }
        });
        if (find == false) {
            $.UifNotify('show',
                {
                    'type': 'danger', 'message': Resources.Language.LimitRcNotFound, 'autoclose': true
                });
        } else {
            LimitRc.showData(null, data, data.key);
        }
    }

    static save() {
        var itemModified = [];
        var dataTable = $("#listViewLimitRc").UifListView('getData');
        $.each(dataTable, function (index, value) {
            if (value.StatusTypeService != undefined && value.StatusTypeService != ParametrizationStatus.Original) {
                value.Limit1 = NotFormatMoney(value.Limit1);
                value.Limit2 = NotFormatMoney(value.Limit2);
                value.Limit3 = NotFormatMoney(value.Limit3);
                value.LimitUnique = NotFormatMoney(value.LimitUnique);
                itemModified.push(value);
            }
        });

        if (itemModified.length > 0) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/LimitRc/ExecuteOperations',
                data: JSON.stringify({ limitRcViewModel: itemModified }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
                    LimitRc.clearPanel();
                    if ($("#listViewLimitRc").UifListView('getData').length > 0) {
                        $("#listViewLimitRc").UifListView("clear");
                    }
                    glbLimitsRcDelete = [];
                    LimitRc.loadLimitRc(data.result.data);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveLimitRc, 'autoclose': true })
            });
        }
        LimitsRc = null;
    }

    static Validation() {
        if ($("#inputLimit1").val() == "")
            $("#inputLimit1").val(0);
        if ($("#inputLimit2").val() == "")
            $("#inputLimit2").val(0);

        var inputLimit1 = parseInt(NotFormatMoney($("#inputLimit1").val()));
        var inputLimit2 = parseInt(NotFormatMoney($("#inputLimit2").val()));
        $("#inputLimit3").val(FormatMoney(inputLimit1 + inputLimit2));
        $("#inputDescription").val($("#inputLimit1").val() + "/" + $("#inputLimit2").val() + "/" + $("#inputLimit3").val());
        //$('#inputLimitUnique').attr("disabled", true);
        $('#inputLimitUnique').val(0);
    }

    static ValidationLimitUnique() {
        if ($("#inputLimitUnique").val() == "")
            $("#inputLimitUnique").val(0), $("#inputDescription").val("");

        $("#inputDescription").val($("#inputLimitUnique").val());
        //$('#inputLimit1').attr("disabled", true);
        //$('#inputLimit2').attr("disabled", true);
        $('#inputLimit1').val(0);
        $('#inputLimit2').val(0);
        $("#inputLimit3").val(0);
    }

    static showDataAdv(LimitRc) {
        var find = false;
        var data = [];
        $.each(glbLimitsRc, function (key, value) {
            if (
                (value.LimitRcCd == LimitRc.LimitRcCd)

            ) {
                LimitRcIndex = key;
                data.push(value);
                find = true;
            }
        });
        if (find == false) {
            $.UifNotify('show',
                {
                    'type': 'danger', 'message': Resources.Language.LimitRcNotFound, 'autoclose': true
                })
        }
        $("#inputLimitCode").val(LimitRc.LimitRcCd);
        $("#inputLimit1").val(LimitRc.Limit1);
        $("#inputLimit2").val(LimitRc.Limit2);
        $("#inputLimit3").val(LimitRc.Limit3);
        $("#inputLimitUnique").val(LimitRc.LimitUnique);
        $("#inputDescription").val(LimitRc.Description);
        if (LimitRc.LimitUnique === 0) {
            $('#inputLimit1').attr("disabled", false);
            $('#inputLimit2').attr("disabled", false);
            $('#inputLimitUnique').attr("disabled", true);
        }
        else {
            $('#inputLimit1').attr("disabled", true);
            $('#inputLimit2').attr("disabled", true);
            $('#inputLimitUnique').attr("disabled", false);
        }
        $("#inputLimitRcSearch").val('');
    }

    static sendExcelLimitRc() {
        LimitRcQueries.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    DeleteItemLimitRc(deferred, data) {
        deferred.resolve();
        var LtsLimitRc = $("#listViewLimitRc").UifListView('getData');
        $.each(LtsLimitRc, function (index, value) {
            if (this.LimitRcCd == data.LimitRcCd) {
                if (value.StatusTypeService != ParametrizationStatus.Create) {
                    value.StatusTypeService = ParametrizationStatus.Delete;
                    if (glbLimitsRcDelete.length > 0) {
                        $.each(glbLimitsRcDelete, function (index, item) {

                            if (item.LimitRcCd != value.LimitRcCd) {
                                glbLimitsRcDelete.push(value);
                                $("#listViewLimitRc").UifListView("addItem", this);
                            }
                        });
                    }
                    else {
                        glbLimitsRcDelete.push(value);
                        value.allowEdit = false;
                        value.allowDelete = false;
                        $("#listViewLimitRc").UifListView("addItem", this);
                    }
                }
                else {
                    value.StatusTypeService = ParametrizationStatus.Original;
                    //value.allowEdit = false;
                    //value.allowDelete = false;
                    //$("#listViewLimitRc").UifListView("addItem", this);
                }
            }
        });
        //LimitRc.clearPanel();
        
    }

    exit() {
        window.location = rootPath + "Home/Index";
    }
}