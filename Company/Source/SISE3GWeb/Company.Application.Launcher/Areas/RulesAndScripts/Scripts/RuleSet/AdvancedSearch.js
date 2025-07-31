var dropDownSearchAdv = null;
class AdvancedSearchRules extends Uif2.Page {
    getInitialState() {
        dropDownSearchAdv = uif2.dropDown({
            source: rootPath + "RulesAndScripts/RuleSet/AdvancedSearch",
            element: "#btnSearchAdvRules",
            align: "right",
            width: 550,
            height: 551,
            container: "#main"
            //,loadedCallback: AdvancedSearchRules.componentLoadedCallback
        });
        request('RulesAndScripts/RuleSet/GetPackages', null, 'POST', AppResources.ErrorGetRules, AdvancedSearchRules.setPackages);
    }
    bindEvents() {
        AdvancedSearchRules.componentLoadedCallback();
    }    
    static componentLoadedCallback() {        
        $("#DateCreation").UifDatepicker();
        $("#DateModification").UifDatepicker();
        $("#btnAdvancedSearch").on("click", AdvancedSearchRules.searchAdv);
        $("#TypePackageId").on("itemSelected", AdvancedSearchRules.changePackageAdv);
        $("#btnCancelAdv").on("click", AdvancedSearchRules.hideDropDownAdv);
        $("#btnAcceptdAdv").on("click", AdvancedSearchRules.loadRule);
        AdvancedSearchRules.listViewAdv(null);
        $("#IdAdv").ValidatorKey(ValidatorType.Number, 1, 0);
        ClearValidation("#formAdvRules");
    }
    static listViewAdv(data) {
        if (data!=null && data.length===0)
        {
            data = null;
        }
        if (data)
        {
			data = $.each(data, function (index, value) {
				value.CurrentFrom = FormatDate(value.CurrentFrom);
				value.CurrentTo = FormatDate(value.CurrentTo);
			});
        }
        $("#listViewAdv").UifListView({
            displayTemplate: "#templateAdvRules",
            selectionType: 'single',
            sourceData: data,
            height: 180
        });
        if (data!=null && data.length === 1)
        {
            $("#listViewAdv").UifListView("setSelected", 0, true);
        }

    }
    static showDropDow() {
        dropDownSearchAdv.show();
    }
    static hideDropDownAdv() {
        dropDownSearchAdv.hide();
        AdvancedSearchRules.clearAdvanced();
    }
    static clearAdvanced() {
        $("#IdAdv").val("");
        $("#DateCreation").UifDatepicker("clear");
        $("#DateModification").UifDatepicker("clear");
        $("#TypePackageId").UifSelect("setSelected",null);
        $("#LevelIdAdv").UifSelect();
        $("#RuleDescription").val("");
        AdvancedSearchRules.listViewAdv(null);
        ClearValidation("#formAdvRules");
    }
    static setPackages(data) {
        $("#TypePackageId").UifSelect({ sourceData: data });
    }
    static changePackageAdv(event, item) {
        AdvancedSearchRules.setLevelsByIdPackage(null);
        if (item.Id) {
            request('RulesAndScripts/RuleSet/GetLevelsByIdPackage', JSON.stringify({ idPackage: item.Id }), 'POST', AppResources.ErrorGetLevels, AdvancedSearchRules.setLevelsByIdPackage);
        }
    }
    static setLevelsByIdPackage(data) {
        $("#LevelIdAdv").UifSelect({sourceData: data});
    }

    static searchAdv() {
        //ClearValidation("#formAdvRules");
        var form = $("#formAdvRules").serializeObject();
        
        //if (form.IdAdv > 0 || form.DateCreation != "" || form.DateModification != "")
        //{
        //    request('RulesAndScripts/RuleSet/GetRulesByRuleSet', JSON.stringify({ ruleAdv: form }), 'POST', AppResources.ErrorGetRules, AdvancedSearchRules.setRulesByFilter);
        //}
        if (form.LevelIdAdv == "" || form.TypePackageId == "" || form.RuleDescription == "") {
            $("#formAdvRules").valid()
        }
        else {
            request('RulesAndScripts/RuleSet/GetRulesByRuleSet', JSON.stringify({ ruleAdv: form }), 'POST', AppResources.ErrorGetRules, AdvancedSearchRules.setRulesByFilter);
        }
        
    }

    static setRulesByFilter(data) {
        AdvancedSearchRules.listViewAdv(data);
    }

    static loadRule() {
        var listRules = $("#listViewAdv").UifListView("getData");
        var itemSelected= $("#listViewAdv").UifListView("getSelected");        
        if (itemSelected.length>0)
        {
            RulesSet.SetListRulesSet(itemSelected);
            AdvancedSearchRules.hideDropDownAdv();
            $("#ddlPackage").UifSelect("setSelected", itemSelected[0].Package.PackageId);//Se setea el combo de modulo de la pantalla principal 
        }
        else
        {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.SelectItemList, 'autoclose': true });
        }
    }
}