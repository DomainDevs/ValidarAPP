//Clase Detalle
var dropDownDetail;
class ClauseDetail extends Uif2.Page {

    getInitialState() {
        dropDownDetail = uif2.dropDown({
            source: rootPath + 'Parametrization/Clauses/DetailClause',
            element: '#btnDetail',
            align: 'right',
            width: 300,
            height: 300,
            container: '#main',
            loadedCallback: ClauseDetail.componentLoadedCallback()

        });    
    }

    bindEvents() {
        $("#btnDetail").on('click', this.ShowAdvanced);
        
    }

    ShowAdvanced() {
        ClauseDetail.componentLoadedCallback()
        dropDownDetail.show();        
    }

    static componentLoadedCallback() {
        if ($("#inputPrecatalogedText").data("Object") != undefined) {
            $('textarea#textArea').val($("#inputPrecatalogedText").data("Object").TextBody);
        }
            
    }

}