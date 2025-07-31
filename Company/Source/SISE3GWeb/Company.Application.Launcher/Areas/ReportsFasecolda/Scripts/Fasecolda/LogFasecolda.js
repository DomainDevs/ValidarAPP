class LogFasecolda extends Uif2.Page {

    getInitialState() {
        $("#dateStart").UifDatepicker('setValue', new Date());
        $("#dateEnd").UifDatepicker('setValue', new Date());
    }
    bindEvents() {
        $('#btnExitLogFasecolda').on('click', this.exit);
        $('#btnSearchLogFasecolda').on('click', LogFasecolda.SearchLogFasecolda);
        $('#dateStart').on("datepicker.change", function (event, date) {
            LogFasecolda.DateValidator(date, "dateStart")
        });
        $('#dateEnd').on("datepicker.change", function (event, date) {
            LogFasecolda.DateValidator(date, "dateEnd")
        });

    }
    exit() {
        window.location = rootPath + "Home/Index";
    }

    static SearchLogFasecolda() {
        var dateStart = $("#dateStart").UifDatepicker('getValue');
        var dateEnd = $("#dateEnd").UifDatepicker('getValue');
        if (!(dateStart <= dateEnd)){
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.EventDates })
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': "Ajax" + AppResources.Query })

        }
    }
    static DateValidator(Date, input) {
        $("#TableLogFasecolda").UifDataTable({ sourceData: [] })
        var dateStart = $("#dateStart").UifDatepicker('getValue');
        var dateEnd = $("#dateEnd").UifDatepicker('getValue');
        if (!(dateStart <= dateEnd) || Date == undefined) {
            if (input == "dateStart") {
                $("#" + input + "").UifDatepicker('setValue', dateEnd);
            } else {
                $("#" + input + "").UifDatepicker('setValue', dateStart);
            }
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.EventDates })
        }
    }
}