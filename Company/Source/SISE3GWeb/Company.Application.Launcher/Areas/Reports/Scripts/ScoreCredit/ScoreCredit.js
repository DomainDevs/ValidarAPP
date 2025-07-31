var DataExport;

//$(document).ready(function () {
//    getInitialState();
//});

class ScoreCreditQueries {
    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Reports/ScoreCredit/GetDocumentType",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHistoryScoreCredit(documenttype, documentNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Reports/ScoreCredit/GetHistoryScoreCredit",
            data: JSON.stringify({
                documentType: documenttype,
                documentNum: documentNumber
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static loadPrefix(prefixes) {
        $("#listViewScoreCreditValid").UifListView({ source: null, displayTemplate: "#BusinessTemplate", edit: false, delete: false, customEdit: false, height: 300 });
        //prefixes.forEach(item => {
        //item.Status = ParametrizationStatus.Create;
        prefixes.dateRequest = FormatDate(prefixes.dateRequest)
        $("#listViewScoreCreditValid").UifListView("addItem", prefixes);
        //});
    }
}

class ReportScoreCredit extends Uif2.Page {
    getInitialState() {

        $("#listViewScoreCreditValid").UifListView({
            displayTemplate: "#BusinessTemplate",
            edit: false,
            delete: false,
            customEdit: false,
            height: 300
        });

        ScoreCreditQueries.GetDocumentType("3").done(function (data) {
            if (data.success) {
                $("#ddlDocumentType").UifSelect({ sourceData: data.result });
            }
        });
    }

    bindEvents() {
        $('#inputProcess').on('buttonClick', this.searchProc);
        $('#btnExport').on('click', this.fnExcelReport);
        $('.OnlyNumber').ValidatorKey(ValidatorType.Number);
    }

    searchProc(event, selectedItem) {
        if (!selectedItem || !selectedItem.trim() || selectedItem.length < 3) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInputSearchCoverage, 'autoclose': true })
            return;
        }
        if ($("#ddlDocumentType").UifSelect("getSelected") == "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageDocumentType, 'autoclose': true })
            return;
        }

        if ($("#listViewScoreCreditValid").UifListView('getData').length > 0) {
            $("#listViewScoreCreditValid").UifListView("clear");
        }
        if ($("#tableScoreCredit").UifDataTable('getData').length > 0) {
            $("#tableScoreCredit").UifDataTable("clear");
        }

        inputSearch = selectedItem;
        ScoreCreditQueries.GetHistoryScoreCredit($("#ddlDocumentType").UifSelect("getSelected"), inputSearch).done(function (data) {
            if (data.data.ErrorTypeService == 0) {
                DataExport = data.data.scoreCredits;
                $('#tableScoreCredit').UifDataTable({
                    sourceData: data.data.scoreCredits,
                    order: false
                });
                ScoreCreditQueries.loadPrefix(data.data.scoreCreditValid);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.data.ErrorDescription[0], 'autoclose': true })
            }
        });
    }

    fnExcelReport() {
        var tab_text = "<table border='1px'>";
        var tab_body = "";
        var tab_header = "";
        var textRange; var j = 0;
        var tab = document.getElementById('tableScoreCredit'); // id of table

        tab_header = "<tr>" +
            "<th>Tipo de Documento</th>" +
            "<th>Número Documento</th>" +
            "<th>Puntaje(Score)</th>" +
            "<th>A1</th>" +
            "<th>A2</th>" +
            "<th>A3</th>" +
            "<th>A4</th>" +
            "<th>A5</th>" +
            "<th>A6</th>" +
            "<th>A7</th>" +
            "<th>A8</th>" +
            "<th>A9</th>" +
            "<th>A10</th>" +
            "<th>A11</th>" +
            "<th>A12</th>" +
            "<th>A13</th>" +
            "<th>A14</th>" +
            "<th>A15</th>" +
            "<th>A16</th>" +
            "<th>A17</th>" +
            "<th>A18</th>" +
            "<th>A19</th>" +
            "<th>A20</th>" +
            "<th>A21</th>" +
            "<th>A22</th>" +
            "<th>A23</th>" +
            "<th>A24</th>" +
            "<th>A25</th>" +
            "<th>Código Respuesta</th>" +
            "<th>Respuesta</th>" +
            "<th>Fecha Consulta</th>" +
            "<th>Es valor por defecto ?</th>" +
            "<th>Nombre de Usuario</th>" +
            "</tr>";
        
        $.each(DataExport, function (i, item) {
            tab_body = tab_body + "<tr>" +
                "<td>" + item.idCardTypeCd + "</td>" +
                "<td>" + item.idCardNo + "</td>" +
                "<td>" + item.a1 + "</td>" +
                "<td>" + item.a2 + "</td>" +
                "<td>" + item.a3 + "</td>" +
                "<td>" + item.a4 + "</td>" +
                "<td>" + item.a5 + "</td>" +
                "<td>" + item.a6 + "</td>" +
                "<td>" + item.a7 + "</td>" +
                "<td>" + item.a8 + "</td>" +
                "<td>" + item.a9 + "</td>" +
                "<td>" + item.a10 + "</td>" +
                "<td>" + item.a11 + "</td>" +
                "<td>" + item.a12 + "</td>" +
                "<td>" + item.a13 + "</td>" +
                "<td>" + item.a14 + "</td>" +
                "<td>" + item.a15 + "</td>" +
                "<td>" + item.a16 + "</td>" +
                "<td>" + item.a17 + "</td>" +
                "<td>" + item.a18 + "</td>" +
                "<td>" + item.a19 + "</td>" +
                "<td>" + item.a20 + "</td>" +
                "<td>" + item.a21 + "</td>" +
                "<td>" + item.a22 + "</td>" +
                "<td>" + item.a23 + "</td>" +
                "<td>" + item.a24 + "</td>" +
                "<td>" + item.a25 + "</td>" +
                "<td>" + item.responseCode + "</td>" +
                "<td>" + item.response + "</td>" +
                "<td>" + item.dateRequest + "</td>" +
                "<td>" + item.isDefaultValue + "</td>" +
                "<td>" + item.userName + "</td>" +
                "</tr>"
        });

        tab_text = tab_text + tab_header + tab_body + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
        tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
        {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
        }
        else                 //other browser not tested on IE 11
            var sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text), );

        return (sa);
    }
}