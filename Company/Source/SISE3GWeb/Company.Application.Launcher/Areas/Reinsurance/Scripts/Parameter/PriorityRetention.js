var glbPriorityRetention = {};
var PriorityRetentionIndex = null;
var priorityRetentionId = null;
var glbPriorityRetentionDelete = [];
var glbPrefixes = {};

class PriorityRetention extends Uif2.Page {
    getInitialState() {
        $("#listPriorityRetention").UifListView({
            displayTemplate: "#PriorityRetentionTemplate",
            edit: true,
            delete: false,
            deleteCallback: PriorityRetention.DeleteItemPriorityRetention,
            customAdd: true,
            customEdit: true,
            height: 300
        });
        PriorityRetention.getPrefixes();
        $("#btnPriorityRetentionAccept").on("click", PriorityRetention.addItemPriorityRetention);
        $("#ValidityFrom").UifDatepicker('setValue', new Date());
        $("#ValidityTo").UifDatepicker('setValue', PriorityRetention.dateAddYear($("#ValidityFrom").val(), 1));
        PriorityRetention.GetPriorityRetentions();

        $.validator.addMethod("greaterThanDateTo",
            function (value, element, params) {
                if (!((IsDate(value)) && (IsDate($(params).val())))) {
                    return false;
                }
                return CompareDates($(params).val(), value) == 0 ? true : false;

            });

        $.validator.addMethod("lessThanDateTo",
            function (value, element, params) {
                if (!((IsDate(value)) && (IsDate($(params).val())))) {
                    return false;
                }
                return CompareDates($(params).val(), value) == 1 ? true : false;

            });
    }

    bindEvents() {
        $("#PriorityRetentionAmount").focusout(PriorityRetention.FormatMoneyOut);
        $("#ValidityFrom").focusout(PriorityRetention.setDateTo);
        $("#listPriorityRetention").on("rowEdit", PriorityRetention.ShowData);
        $("#btnNewPriorityRetention").on("click", PriorityRetention.clearPriorityRetentionData);
        $("#btnSavePriorityRetention").on("click", PriorityRetention.SavePriorityRetentions);
        $("#btnExit").on("click", PriorityRetention.Exit);
        $("#PrefixId").on("change", PriorityRetention.clearForm);
    }

    static GetPriorityRetentions() {
        PriorityRetentionRequest.GetPriorityRetentions().done(function (data) {
            if (data.success) {
                glbPriorityRetention = data.result;
                PriorityRetention.LoadPriorityRetentions();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SavePriorityRetentions() {
        var glbPriorityRetentionAdded = [];
        var glbPriorityRetentionModified = [];
        var listPriorityRetention = $("#listPriorityRetention").UifListView('getData');
        glbPriorityRetentionAdded = listPriorityRetention.filter(function (item) {
            return item.StatusTypeService == ParametrizationStatus.Create;
        });
        glbPriorityRetentionModified = listPriorityRetention.filter(function (item) {
            return item.StatusTypeService == ParametrizationStatus.Update;
        });

        if (glbPriorityRetentionModified.length > 0 || glbPriorityRetentionAdded.length > 0 || glbPriorityRetentionDelete.length > 0) {
            PriorityRetentionRequest.SavePriorityRetentions(glbPriorityRetentionAdded, glbPriorityRetentionModified, glbPriorityRetentionDelete).done(function (response) {
                if (response.success) {
                    glbPriorityRetentionDelete = [];
                    $.UifNotify('show', { 'type': 'success', 'message': response.result, 'autoclose': true });
                    PriorityRetention.GetPriorityRetentions();
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }

            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Global.ThereIsNotChangePending, 'autoclose': true });
        }


    }

    static DeleteItemPriorityRetention(event, result, index) {
        if (result.Id > 0) {
            glbPriorityRetentionDelete.push(result);
        }
        event.resolve();
    }

    //Añade N años a la fecha enviada
    static dateAddYear(paramDate, addYear) {
        var resultDate = "";
        if (paramDate != "") {
            var dateExercise = paramDate.split("/");
            var nextYear = parseInt(dateExercise[2]) + addYear;
            resultDate = dateExercise[0] + "/" + dateExercise[1] + "/" + nextYear;
        }
        return resultDate;
    }

    static addItemPriorityRetention() {
        $("#formPriorityRetention").validate();
        if ($("#formPriorityRetention").valid()) {

            var listPriorityRetention = $("#listPriorityRetention").UifListView('getData').filter(function (item) {
                return item.Id != $("#PriorityRetentionId").val();
            });

            var existItem = listPriorityRetention.filter(function (item) {
                return (((item.Prefix.Id == $("#PrefixId").val()) &&
                    (item.ValidityFrom == $("#ValidityFrom").val()) &&
                    (item.ValidityTo == $("#ValidityTo").val())) ||
                    ((item.Prefix.Id == $("#PrefixId").val()) &&
                        (PriorityRetention.PeriodExist(item.ValidityTo, $("#ValidityFrom").val()))));
            });

            if (existItem.length > 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Global.PeriodPriorityRetentionExist, 'autoclose': true });
                PriorityRetention.clearPriorityRetentionData();
            }
            else {
                var lPriorityRetention = {};
                lPriorityRetention.Prefix = new Object();
                lPriorityRetention.Id = $("#PriorityRetentionId").val();
                lPriorityRetention.Prefix.Id = $("#PrefixId").val();
                lPriorityRetention.Prefix.Description = $('select[name="PrefixId"] option:selected')[0].outerText;
                lPriorityRetention.PriorityRetentionAmount = RemoveFormatMoney($("#PriorityRetentionAmount").val());
                lPriorityRetention.PriorityRetentionAmountFormat = $("#PriorityRetentionAmount").val();
                lPriorityRetention.ValidityFrom = $("#ValidityFrom").val();
                lPriorityRetention.ValidityTo = $("#ValidityTo").val();
                lPriorityRetention.Enabled = $("#Enabled").prop("checked");
                if (lPriorityRetention.Id > 0) {
                    PriorityRetentionRequest.CanPriorityRetentionUpdated(lPriorityRetention.Id).done(function (data) {
                        if (data.success) {
                            if (!data.result) {
                                lPriorityRetention.StatusTypeService = ParametrizationStatus.Update;
                                $("#listPriorityRetention").UifListView("editItem", PriorityRetentionIndex, lPriorityRetention);
                                PriorityRetention.clearPriorityRetentionData();
                            } else {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Global.YouCanNotChangePriorityRetention, 'autoclose': true });
                                PriorityRetention.clearPriorityRetentionData();
                            }
                        }
                    });
                }
                else {
                    lPriorityRetention.StatusTypeService = ParametrizationStatus.Create;
                    if (PriorityRetentionIndex == null) {
                        $("#listPriorityRetention").UifListView("addItem", lPriorityRetention);
                        PriorityRetention.clearPriorityRetentionData();
                    }
                    else {
                        $("#listPriorityRetention").UifListView("editItem", PriorityRetentionIndex, lPriorityRetention);
                        PriorityRetention.clearPriorityRetentionData();
                    }
                }
            }
        }
    }

    static LoadPriorityRetentions() {
        $("#listPriorityRetention").UifListView("clear");
        $.each(glbPriorityRetention, function (key, value) {
            var lPriorityRetention = {};
            lPriorityRetention.Prefix = new Object();
            lPriorityRetention.Id = this.Id;
            lPriorityRetention.Prefix.Id = this.Prefix.Id;
            if (glbPrefixes.length > 0) {
                lPriorityRetention.Prefix.Description = glbPrefixes.filter(function (item) { return item.Id == lPriorityRetention.Prefix.Id; })[0].Description;
            }
            lPriorityRetention.Enabled = this.Enabled;
            lPriorityRetention.PriorityRetentionAmount = this.PriorityRetentionAmount;
            lPriorityRetention.PriorityRetentionAmountFormat = FormatMoney(this.PriorityRetentionAmount);
            lPriorityRetention.ValidityFrom = FormatDate(this.ValidityFrom);
            lPriorityRetention.ValidityTo = FormatDate(this.ValidityTo);
            $("#listPriorityRetention").UifListView("addItem", lPriorityRetention);
        });
    }

    static PeriodExist(validityTo, validityFrom) {
        var periodExist = false;
        var validityTo = FormatDate(validityTo).split("/");
        var validityFrom = FormatDate(validityFrom).split("/");
        var dateTo = new Date(validityTo[2], validityTo[1] - 1, validityTo[0]);
        var dateFrom = new Date(validityFrom[2], validityFrom[1] - 1, validityFrom[0]);
        if (dateFrom < dateTo) {
            periodExist = true;;
        }
        return periodExist;
    }

    static ShowData(event, result, index) {
        PriorityRetentionIndex = index;
        $("#PrefixId").UifSelect("setSelected", result.Prefix.Id);
        $("#PriorityRetentionAmount").val(FormatMoney(RemoveFormatMoney(result.PriorityRetentionAmount)));
        $("#ValidityFrom").val(result.ValidityFrom);
        $("#ValidityTo").val(result.ValidityTo);
        $("#PriorityRetentionId").val(result.Id);
        $("#Enabled").prop("checked", result.Enabled);
    }

    static clearPriorityRetentionData() {
        $("#PrefixId").UifSelect("setSelected", null);
        $("#PriorityRetentionId").val("0");
        PriorityRetentionIndex = null;
        PriorityRetention.clearForm();
    }

    static clearForm() {
        $("#PriorityRetentionAmount").val("");
        $("#ValidityFrom").UifDatepicker('setValue', new Date());
        $("#ValidityTo").UifDatepicker('setValue', PriorityRetention.dateAddYear($("#ValidityFrom").val(), 1));
        $("#PriorityRetentionAmount").val("");
        $("#Enabled").prop("checked", false);
    }

    static getPrefixes() {
        PriorityRetentionRequest.getPrefixes().done(function (response) {
            $('#PrefixId').UifSelect({ sourceData: response.data });
            glbPrefixes = response.data;
        });
    }

    static FormatMoneyOut() {
        $(this).val(FormatMoney(RemoveFormatMoney($(this).val())));
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }
}

$(document).ready(function () {
    new PriorityRetention();
    new PriorityRetentionRequest();


});