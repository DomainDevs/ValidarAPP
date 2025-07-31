var massiveLoads = {};
var massive = {};
var toDate = "";
var fromDate = "";

class MassiveProcessRequest {
    static GenerateMassiveProcessReport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/MassiveProcess/GenerateMassiveProcessReport',
            data: JSON.stringify({ rangeFrom: fromDate, rangeTo: toDate, massiveLoad: massive }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class MassiveProcess extends Uif2.Page {

    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {

        MassiveProcessSearch.SearchAdvancedMassiveProcess().done(function (data) {
            $('#TableLoadProcess').UifDataTable('clear');
            if (data.success) {
                if (data.result.length > 0) {
                    MassiveProcess.LoadTable(data.result);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    //---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        $("#btnShowAdvanced").on("click", this.ShowAdvanced);
        $("#btnShowSearch").on("click", this.ShowSearch);
        $("#btnExport").on("click", this.Report);
        $("#btnExit").on("click", this.Exit);
    }

    //-------------------------------------------------------//

    /**
 * Funcion para abrir la vista de Busquedas Avanzadas
 */
    ShowAdvanced() {
        $("#listViewSearchAdvancedMassiveProcess").UifListView(
            {
                displayTemplate: "#advancedSearchMassiveProcessTemplate",
                selectionType: 'single',
                source: null,
                height: 220
            });
        dropDownSearchMassiveProcess.show();
    }

    /**
     * Funcion para buscar los procesos masivos
     */
    ShowSearch() {
        MassiveProcessSearch.SearchAdvancedMassiveProcess($("#inputFrom").val(), null, null).done(function (data) {
            if (data.success) {
                fromDate = $("#inputFrom").val();
                var massive = {};
                var toDate = "";

                $('#TableLoadProcess').UifDataTable('clear');
                if (data.result.length > 0) {
                    MassiveProcess.LoadTable(data.result);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    Report() {
        MassiveProcessRequest.GenerateMassiveProcessReport(fromDate, toDate, massive).done(function (data) {
            if (data.success) {
                DownloadFile(data.result, true, (url) => url.match(/([^\\]+.csv)/)[0]);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * @summary Funcion para cargar los procesos masivos en la tabla 
     * @param {result} Resultado de la busqueda de procesos masivos
     */
    static LoadTable(result) {
        massiveLoads = {}
        massiveLoads = result;

        $.each(result, function (key, value) {
            var massive = {
                ProcessType: value.LoadType.ProcessTypeDescription,
                LoadNumber: value.Id,
                LoadDescription: value.Description,
                RisksNumber: value.TotalRows,
                StartDateLoadProcess: null,
                EndDateLoadProcess: null,
                //StartDateExternalProcess: null,
                //EndDateExternalProcess: null,
                StartDateTariffProcess: null,
                EndDateTariffProcess: null,
                StartDateIssueProcess: null,
                EndDateIssueProcess: null,
                AccountName: value.User.AccountName
            };

            $.each(value.Logs, function (key, value) {

                if (value.Description == "VALIDACIÓN ARCHIVO") {
                    massive.StartDateLoadProcess = MassiveProcess.FormatDateTime(value.Time);
                }
                if (value.Description == "VALIDADO") {
                    massive.EndDateLoadProcess = MassiveProcess.FormatDateTime(value.Time);
                }
                //if (value.Description == "CONSULTANDO") {
                //    massive.StartDateExternalProcess = MassiveProcess.FormatDateTime(value.Time);
                //}
                //if (value.Description == "CONSULTADO") {
                //    massive.EndDateExternalProcess = MassiveProcess.FormatDateTime(value.Time);
                //}
                if (value.Description == "TARIFANDO") {
                    massive.StartDateTariffProcess = MassiveProcess.FormatDateTime(value.Time);
                }
                if (value.Description == "TARIFADO") {
                    massive.EndDateTariffProcess = MassiveProcess.FormatDateTime(value.Time);
                }
                if (value.Description == "EMITIENDO") {
                    massive.StartDateIssueProcess = MassiveProcess.FormatDateTime(value.Time);
                }
                if (value.Description == "EMITIDO") {
                    massive.EndDateIssueProcess = MassiveProcess.FormatDateTime(value.Time);
                }

            });
           
            $('#TableLoadProcess').UifDataTable('addRow', massive);
        });
    }

    static FormatDateTime(date) {
        if (date != null) {
            var datetime = FormatDate(date);
            if (datetime != null) {
                var dateString = date.substr(6);
                var currentTime = new Date(parseInt(dateString));
                var hour = currentTime.getHours();
                var minute = currentTime.getMinutes();
                var second = currentTime.getSeconds();
                var hourFormat = hour < 10 ? '0' + hour : '' + hour;
                var minuteFormat = minute < 10 ? '0' + minute : '' + minute;
                var secondFormat = second < 10 ? '0' + second : '' + second;
                var datetime = datetime + " " + hourFormat + ":" + minuteFormat + ":" + secondFormat;
            }
        }
        return datetime;
    }

    /**
    *@summary Redirecciona al index
    */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

