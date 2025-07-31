var dropDownSearchAdvDt;
var index;
var level = [];
//$.ajaxSetup({ async: true });
class AdvancedSearchDt extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: false });
        dropDownSearchAdvDt = uif2.dropDown({
            source: rootPath + "RulesAndScripts/DecisionTable/AdvancedSearchDt",
            element: "#btnSearchAdvDecisionTable",
            align: "right",
            width: 600,
            height: 551,
            container: "#main",
            loadedCallback: AdvancedSearchDt.componentLoadedCallback
        });
    }

	bindEvents() {
		AdvancedSearchDt.componentLoadedCallback();
	}

	static componentLoadedCallback() {
		$("#inputDateModification").UifDatepicker();
		$("#selectLevel").UifSelect();
		$("#selectPublished").UifSelect();
		$("#lvSearchScript").UifListView({ displayTemplate: "#template-DecisionTablelist", selectionType: "single", height: 200 });
		$("#lvSearchDesicionTable").UifListView({ displayTemplate: "#template-SearchDecisionTable", selectionType: "single", height: 180 });
		request("RulesAndScripts/RuleSet/GetPackages", {}, "POST", Resources.Language.ErrorSearchCountry, AdvancedSearchDt.SetPackages);
		$("#selectPackage").on("itemSelected", AdvancedSearchDt.Level);
		$("#btnsearchDtAdv").on("click", AdvancedSearchDt.SearchDecisionTable);
		$("#btnOkSearchDt").on("click", AdvancedSearchDt.OkButton);
		$("#btnCancelSearchDt").on("click", AdvancedSearchDt.CancelButton);
	}

	static SetPackages(data) {
		let comboConfig = { sourceData: data };
		$("#selectPackage").UifSelect(comboConfig);
	}

	static GetPublished(data) {
		let comboConfig = { sourceData: data };
		$("#selectPublished").UifSelect(comboConfig);
	}

	static OkButton() {
		var object = $("#lvSearchDesicionTable").UifListView("getSelected");
		if (object) {
			DecisionTable.SetListDecisionTable(object);
		}
		AdvancedSearchDt.ClearSearch();
		AdvancedSearchDt.CancelSearchAdv();
	}

	static CancelButton() {
		AdvancedSearchDt.ClearSearch();
		AdvancedSearchDt.CancelSearchAdv();
		$("#lsvDecisionTable").UifListView("setSelected", 0, true);
	}

	static ClearSearch() {
		$("#selectLevel").UifSelect('setSelected', null);
		$("#selectPackage").UifSelect('setSelected', null);
		$("#selectPublished").UifSelect('setSelected', null);
		$("#lvSearchDesicionTable").UifListView("clear");
		$("#inputTableDescription").val("");
		$("#inputTableId").val("");
		$("#inputDateModification").UifDatepicker('clear');
	}

	static Level(event, IdModule) {
		if (IdModule.Id > 0) {
			request("RulesAndScripts/RuleSet/GetLevelsByIdPackage", JSON.stringify({ idPackage: IdModule.Id }), "POST", Resources.Language.ErrorSearchCountry, AdvancedSearchDt.LevelByPackege);
			request('RulesAndScripts/RuleSet/GetPublished', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchDt.GetPublished);
		}
		else {
			$("#selectLevel").UifSelect({ sourceData: null });
		}
	}

	static LevelByPackege(data) {
		let comboConfig = { sourceData: data };
		$("#selectLevel").UifSelect(comboConfig);
	}

	static DecisionTable(data) {
		$("#lvSearchDesicionTable").UifListView("clear");
		data.forEach((item) => {
			$("#lvSearchDesicionTable").UifListView("addItem", item);
		});
		if (data.length == 1) {
			$("#lvSearchDesicionTable").UifListView("setSelected", 0, true);
		}
	}

	static SearchDecisionTable() {
		level = [];
		var validationResult = false;
		if ($("#inputTableId").val() != "" || $("#inputDateModification").val() != "") {
			validationResult = true;
		}
		if (validationResult && $("#selectLevel").val() != null || $("#selectPackage").val() != "" || $("#inputTableDescription").val() != "") {
			if ($("#selectLevel").val() == null || $("#selectPackage").val() == "" || $("#inputTableDescription").val() == "") {
				$.UifNotify("show", { 'type': "danger", 'message': Resources.Language.DecisionTableErrorFilter, 'autoclose': true });
			} else {
				if ($("#selectLevel").val() == "" && $("#selectPublished").val() == "") {
					$.UifNotify("show", { 'type': "danger", 'message': Resources.Language.SelectTwoCriteria, 'autoclose': true });
				} else {
					validationResult = true;
				}
			}
		}
		if (validationResult) {
			if ($("#selectLevel").val() != null) {
				level.push($("#selectLevel").UifSelect("getSelected"));
			}
			let packageId = $("#selectPackage").UifSelect("getSelected");
			let published = $("#selectPublished").UifSelect("getSelected");
			let description = $("#inputTableDescription").val();
			let tableId = $("#inputTableId").val() == "" ? "0" : $("#inputTableId").val();
			let dateModification = $("#inputDateModification").val() != "" ? $("#inputDateModification").val() : null;
			if (packageId) {
				packageId = null;
			}
			switch (published) {
				case "2":
					published = true;
					break;
				case "3":
					published = false;
					break;
				default:
					published = null;
					break;
			}
			RequestDecisionTable.GetDecisionTableByFilter(packageId, level, false, description, tableId, dateModification, published).done((data) => {
				if (data.success) {
					var result = data.result;
					AdvancedSearchDt.DecisionTable(result);
				} else {
					$.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
				}
			});

		}
	}

	static SearchAdvBranch() {
		dropDownSearchAdvDt.show();
	}

	static CancelSearchAdv() {
		dropDownSearchAdvDt.hide();
		AdvancedSearchDt.ClearSearch();
	}
}