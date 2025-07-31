class RequestRulesSetCache {
    static GetPublishedVersions() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSetCache/GetPublishedVersions"
        });
    }
    static GetNodeVersions() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSetCache/GetNodeVersions"
        });
    }
    static RecordRuleNode(ruleSet) {
        return $.ajax({
            type: "POST",
            data: { "ruleSet": JSON.stringify(ruleSet) },
            url: rootPath + "RulesAndScripts/RuleSetCache/RecordRuleNode"
        });
    }
}

class RulesSetCache extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: false });
		$("#tableJsonVersion").UifDataTable({ sourceData: [], widthColumns: [{ width: '10px', column: 0 }, { width: '200px', column: 1 }, , { width: '20px', column: 2 }] })
		$("#tableJsonVersionNode").UifDataTable({ sourceData: [], widthColumns: [{ width: '10px', column: 0 }, { width: '20px', column: 1 }, { width: '200px', column: 2 }, { width: '20px', column: 3 }, { width: '20px', column: 4 }] })
        RulesSetCache.GetNodeVersions();
        RulesSetCache.GetPublishedVersions();
    }

    bindEvents() {
        $('#btnExit').click(RulesSetCache.Exit);
        $('#btnLoadRecordRuleNode').click(RulesSetCache.LoadTables);
        $("#btnRecordRuleNode").click(RulesSetCache.RecordRuleNode);
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static GetPublishedVersions() {
        RequestRulesSetCache.GetPublishedVersions().done(function (data) {
            if (data.success) {
                var PublisheVersions = data.result;
                PublisheVersions.forEach(function (element) {
                    var date = new Date(parseInt(element.VersionDatetime.substr(6)));
                    var formatted = date.getFullYear() + "-" +
                        ("0" + (date.getMonth() + 1)).slice(-2) + "-" +
                        ("0" + date.getDate()).slice(-2) + " " + date.getHours() + ":" +
                        date.getMinutes(); 
                    element.VersionDatetime = formatted;
                });
                $("#tableJsonVersion").UifDataTable({ sourceData: PublisheVersions });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetNodeVersions() {
        RequestRulesSetCache.GetNodeVersions().done(function (data) {
            if (data.success) {
                var NodeVersions = data.result;
                NodeVersions.forEach(function (element) {
					element.CreationDate = RulesSetCache.FormatToDateTime(element.CreationDate.substr(6));
					if (element.FinishDate != null && element.FinishDate != "") {
						element.FinishDate = RulesSetCache.FormatToDateTime(element.FinishDate.substr(6));
					}
                });
                $("#tableJsonVersionNode").UifDataTable({ sourceData: NodeVersions });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadTables() {
        RulesSetCache.GetPublishedVersions();
        RulesSetCache.GetNodeVersions();
    }
    static RecordRuleNode() {
        $.UifDialog('confirm', { 'message': AppResources.SurepublishRuleSetLoad, 'title': AppResources.Confirmation }, function (result) {
            if (result === true) {
                RequestRulesSetCache.RecordRuleNode().done((data) => {
                    if (data.success) {
                        $.UifNotify("show", { 'type': "info", 'message': data.result, 'autoclose': false });
                    }
                    else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
            }
        });
	}

	static FormatToDateTime(datetime) {
		var date = new Date(parseInt(datetime));
		var formatted = date.getFullYear() + "-" +
			("0" + (date.getMonth() + 1)).slice(-2) + "-" +
			("0" + date.getDate()).slice(-2) + " " + date.getHours() + ":" +
			date.getMinutes();
		return formatted;
	}

}