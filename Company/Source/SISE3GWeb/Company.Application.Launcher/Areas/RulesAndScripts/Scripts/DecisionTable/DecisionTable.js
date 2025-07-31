var isEvent = false;
var glbDecisionTable = null;
var glbConcepts = null;


class RequestDecisionTable {

    /**
     * @summary 
     *  Obtiene el listado de tablas de decision por el filtro indicado
     * @param {int} idPackage
     * d del paquete
     * @param {list<int>} levels 
     * lista de los niveles
     * @param {bool} isPolicie 
     * si es politica
     * @param {string} filter 
     * filtro like de la descripcion
     */
    static GetDecisionTableByFilter(idPackage, levels, isPolicie, filter, tableId, dateModification, isPublished) {
        return $.ajax({
            type: "POST",
            data: {
                "idPackage": idPackage,
                "levels": levels,
                "isPolicie": isPolicie,
                "filter": filter,
                "tableId": tableId,
                "dateModification": dateModification,
                "isPublished": isPublished
            },
            url: rootPath + "RulesAndScripts/DecisionTable/GetDecisionTableByFilter"
        });
    }

    /** <summary>
    * elimina la tabla de decision 
    *@param {int} ruleBaseId
    * id de la DT
    **/
    static DeleteTableDecision(ruleBaseId) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId
            },
            url: rootPath + "RulesAndScripts/DecisionTable/DeleteTableDecision"
        });
    }

    /**
     * @summary 
    * obtiene la cabecera de la tabla de decision 
    * @param {int}ruleBaseId
     * id de la DT
    */
    static GetTableDecisionHead(ruleBaseId) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId
            },
            url: rootPath + "RulesAndScripts/DecisionTable/GetTableDecisionHead"
        });
    }

    static CreateTableDecisionHead(ruleBase, conceptCondition, conceptAction) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBase": ruleBase,
                "conceptCondition": conceptCondition,
                "conceptAction": conceptAction
            },
            url: rootPath + "RulesAndScripts/DecisionTable/CreateTableDecisionHead"
        });
    }

    static UpdateTableDecisionHead(ruleBase, conceptCondition, conceptAction) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBase": ruleBase,
                "conceptCondition": conceptCondition,
                "conceptAction": conceptAction
            },
            url: rootPath + "RulesAndScripts/DecisionTable/UpdateTableDecisionHead"
        });
    }

    /**
    * @summary 
   * obtiene la Data de la tabla de decision 
   * @param {int}ruleBaseId
    * id de la DT
   */
    static GetTableDecisionBody(ruleBaseId) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId
            },
            url: rootPath + "RulesAndScripts/DecisionTable/GetTableDecisionBody"
        });
    }

    /**
     * @summary 
     * Realiza el guardado de la DT
     * Reglas para eliminar
     */
    static SaveDecisionTable(ruleBaseId, lastRule) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId,
                "lastRule": JSON.stringify([lastRule])
            },
            url: rootPath + "RulesAndScripts/DecisionTable/SaveDecisionTable"
        });
    }

    /**
     * @summary 
     * valida y publica una TD
     * @param {int} ruleBaseId 
     * id dT a publicar
     */
    static PublishDecisionTable(ruleBaseId, isEvent) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId,
                "isEvent": isEvent
            },
            url: rootPath + "RulesAndScripts/DecisionTable/PublishDecisionTable",
            async: true
        });
    }

    static PreviewXlsFile(pathXml, pathXls, save, isEvent) {
        return $.ajax({
            type: "POST",
            data: {
                "pathXml": pathXml,
                "pathXls": pathXls,
                "save": save,
                "isEvent": isEvent
            },
            url: rootPath + "RulesAndScripts/DecisionTable/PreviewXlsFile",
            async: true
        });
    }

    /**
     * @summary 
     * Exporta archivo excel
     */
    static GenerateFileToExportDecisionTable(ruleBaseId) {
        return $.ajax({
            type: 'POST',
            data: {
                "ruleBaseId": ruleBaseId
            },
            url: rootPath + 'RulesAndScripts/DecisionTable/GenerateFileToExportDecisionTable'
        });
    }

    static GenerateFileToExportDecisionTables() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'RulesAndScripts/DecisionTable/GenerateFileToExportDecisionTables'
        });
    }

    static DecisionTableItem(ruleBaseId, rule, status) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId,
                "rule": JSON.stringify([rule]),
                "status": status
            },
            url: rootPath + 'RulesAndScripts/DecisionTable/DecisionTableItem'
        });
    }

    static GetRuleByItem(ruleBaseId, ruleId) {
        return $.ajax({
            type: "POST",
            data: {
                "ruleBaseId": ruleBaseId,
                "ruleId": ruleId
            },
            url: rootPath + 'RulesAndScripts/DecisionTable/GetRuleByItem'
        });
    }

    /**
     * @summary 
     * Exporta archivo excel
     */
    static GenerateFileToExportDecisionTableLocal(urlFile) {
        return $.ajax({
            type: 'POST',
            data: {
                "urlFile": urlFile
            },
            url: rootPath + 'RulesAndScripts/DecisionTable/descarga'
        });
    }
}

class DecisionTable extends Uif2.Page {

    constructor(e) {
        super(e);

        glbConcepts = null;

        isEvent = e;

        if (isEvent === "true") {
            RequestPackage.GetPackagePolicies().done((data) => {
                if (data.success) {
                    $("#ddlPackage").UifSelect({
                        sourceData: data.result,
                        selectedId: 10
                    });
                    //$("#ddlPackageAdv").UifSelect({
                    //    sourceData: data.result
                    //});
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            RequestPackage.GetPackages().done((data) => {
                if (data.success) {
                    $("#ddlPackage").UifSelect({
                        sourceData: data.result,
                        selectedId: 1
                    });
                    //$("#ddlPackageAdv").UifSelect({
                    //    sourceData: data.result
                    //});
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    getInitialState() {
        new AdvancedSearchDt();
        $("#lsvDecisionTable").UifListView({
            sourceData: glbDecisionTable,
            displayTemplate: "#template-DecisionTable",
            selectionType: "single",
            title: Resources.Language.DecisionTables,
            deleteCallback: this.DeleteDecisionTable
        });
        $("#lsvDecisionTable").UifListView("setSelected", 0, true);
    }

    bindEvents() {
        $("#btnExit").on("click", this.Exit);
        $("#ddlPackage").on("itemSelected", this.ChangePackage);
        $("#SearchDecisionTable").on("search", this.SearchDecisionTable);
        $("#btnModifyConcepts").on("click", this.ModifyConcepts);
        $("#btnModifyData").on("click", this.ModifyData);
        $("#LoadFromFile").on("click", this.LoadFromFile);
        $("#btnExportDecisionTable").on("click", this.ExportDecisionTable);
        $("#btnExportDecisionTables").on("click", this.ExportDecisionTables);
        $("#btnNewTable").on("click", this.NewTable);
        $("#btnSearchAdvDecisionTable").on("click", this.Search);
        $("#lsvDecisionTable").on("itemSelected", this.selectedDecisionTable);
    }

    Search() {
        AdvancedSearchDt.SearchAdvBranch();
    }
    LoadFromFile() {
        router.run("#prtDecisionTableFile");
    }

    ModifyConcepts() {
        let selected = DecisionTable.GetSelectedDecisionTable();
        if (selected !== false) {
            glbDecisionTable = selected;
            router.run("#prtDecisionTableHeader");
        }
    }

    ModifyData() {
        let selected = DecisionTable.GetSelectedDecisionTable();
        if (selected !== false) {
            glbDecisionTable = selected;
            router.run("#prtDecisionTableData");
        }
    }

    NewTable() {
        glbDecisionTable = null;
        router.run("#prtDecisionTableHeader");
    }

    /**
     * @summary 
     * Elimana una DT
     * @param {} item 
     * Dt a eliminar
     */
    DeleteDecisionTable(deferred, item) {
        if (item.RuleBaseId === 0) {
            deferred.resolve();
        } else {
            RequestDecisionTable.DeleteTableDecision(item.RuleBaseId).done((data) => {
                if (data.success) {
                    deferred.resolve();
                    $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    /**
     * @summary 
     * realiza la busqueda simple de DT
     * @param  {string} value 
     * Valor a consultar
     */
    SearchDecisionTable(event, value) {
        $("#SearchDecisionTable").val("");
        $("#lvSearchDesicionTable").UifListView("clear");
		let packageId = $("#ddlPackage").UifSelect("getSelected");
		var filterTableDecision = "";
		var tableDecisionId = 0;
		if (isNaN(value)) {
			filterTableDecision = value;
		}
		else {
			if (value != "") {
				tableDecisionId = value;
			}
		}
        if (packageId) {
			RequestDecisionTable.GetDecisionTableByFilter(packageId, [], isEvent, filterTableDecision, tableDecisionId, "").done((data) => {
                if (data.success) {
                    if (data.result.length == 0) {
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true });
                    }
                    else if (data.result.length == 1) {
                        $("#lsvDecisionTable").UifListView("clear");
                        $("#lsvDecisionTable").UifListView("addItem", data.result[0]);
                        $("#lsvDecisionTable").UifListView("setSelected", 0, true);
                    } else {
                        DecisionTable.SetListDecisionTable(data.result);
                        AdvancedSearchDt.SearchAdvBranch();
                    }

                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $("#lsvDecisionTable").UifListView("clear");
        }
    }

    selectedDecisionTable(e, item) {
        $("#lsvDecisionTable").UifListView("setSelected", 0, true);
    }

    /**
     * @summary 
     * Evento al cambiar el paquete de la busqueda sencilla
     * @param  item 
     * Paquete selecionado
     */
    ChangePackage(e, item) {
        if (item.Id) {
            //RequestDecisionTable.GetDecisionTableByFilter(item.Id, [], isEvent, "").done((data) => {
            //    if (data.success) {
            //        DecisionTable.SetListDecisionTable(data.result);
            //    } else {
            //        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            //    }
            //});
        } else {
            $("#lsvDecisionTable").UifListView("clear");
        }
    }

    /**
     * @summary 
     * sale a la pantalla inicial
     */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    /**
     * @summary 
     * Exportar archivo excel
     */
    ExportDecisionTable() {
        let selected = DecisionTable.GetSelectedDecisionTable();
        if (selected !== false) {
            RequestDecisionTable.GenerateFileToExportDecisionTable(selected.RuleBaseId).done(function (data) {
                if (data.success) {
                    try { 

                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                 
                    } catch(ex){
                       $.UifNotify('show', { 'type': 'info', 'message': 'Error al descargar Tabla', 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            
        }
    }
    /**
     * @summary 
     * Exportar archivo excel
     */
    ExportDecisionTables() {
        RequestDecisionTable.GenerateFileToExportDecisionTables().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    /**
     * @summary 
     * Setea la lista de tablas de decision 
     * @param {RuleBase} data 
     * Lista de las tablas de decision
     */
    static SetListDecisionTable(data) {
        data.forEach((item) => {
            $("#lvSearchDesicionTable").UifListView("addItem", item);
            if ($("#lvSearchDesicionTable").UifListView("getSelected") != '') {
                $("#lsvDecisionTable").UifListView("clear");
                $("#lsvDecisionTable").UifListView("addItem", item);
                $("#lsvDecisionTable").UifListView("setSelected", 0, true);
            }
        });
    }

    static GetSelectedDecisionTable() {
        let selected = $("#lsvDecisionTable").UifListView("getSelected");

        if (selected.length === 0) {
            $.UifNotify("show", { 'type': "warning", 'message': "Seleccione un item", 'autoclose': true });
            return false;
        } else {
            return selected[0];
        }
    }

    
}

