$.ajaxSetup({ async: false });
var ObjectAdvanceSearchRules =
{
    bindEvents: function () {

        $("#SearchRule").on("search", function (event, value) {
            var IsEvent = getQueryVariable("IsEvent") ? true : false;
            if (value.length == 0) {
                $.ajax({
                    type: "POST",
                    url: rootPath + "RulesScripts/RuleSet/GetAllRuleSets",
                    data: {
                        "IsEvent": IsEvent
                    },
                }).done(function (data) {
                    ControlRuleSet.fillRulePack(data);
                });
            }
            else if (value.length < 3) {
                $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
            } else {
                $.ajax({
                    type: "POST",
                    url: rootPath + "RulesScripts/RuleSet/GetRuleByName",
                    data: {
                        "RuleName": value,
                        "IsEvent": IsEvent
                    },
                }).done(function (data) {
                    $("#SearchRule").val("");
                    if (data.length == 0) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.NoItemsFound, autoclose: false });
                    }
                    else if (data.length == 1) {
                        ControlRuleSet.fillRulePack(data);
                    } else {
                        ObjectAdvanceSearchRules.dropDownRules.show();
                        $("#RuleSetListAdvSearch").UifListView({
                            sourceData: data,
                            displayTemplate: "#template-RuleSetListAdvSearch",
                            selectionType: 'single',
                            height: 260
                        });
                    }
                });
            }
        }),

        $("#RuleSetListAdvSearch").UifListView({
            source: null,
            displayTemplate: "#template-RuleSetListAdvSearch",
            selectionType: 'single',
            height: 260
        });

        $("#selectPackage").UifSelect({
            source: rootPath + "RulesScripts/RuleSet/GetPackages"
        });

        $("#btnCloseSearchAdv").on("click", function () {
            ObjectAdvanceSearchRules.dropDownRules.hide();
            ObjectAdvanceSearchRules.clearAdvSearchRules();
        })

        $("#selectPackage").on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                var controller = rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + selectedItem.Id;
                $("#selectLevel").UifSelect({ source: controller });
            }
        });

        $("#selectLevel").on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                var IsEvent = getQueryVariable("IsEvent") ? true : false;
                $("#RuleSetListAdvSearch").UifListView({
                    source: rootPath + "RulesScripts/RuleSet/GetRuleSetDTOsByLevelId?levelId=" + selectedItem.Id + "&IsEvent=" + IsEvent,
                    displayTemplate: "#template-RuleSetListAdvSearch",
                    selectionType: 'single',
                    height: 207
                });

                ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar()

            }
        });

        $("#selectPrefix").on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                var controller = rootPath + "RulesScripts/RuleSet/GetProducts?prefixCode=" + selectedItem.Id;
                $("#selectProduct").UifSelect({ source: controller });
            }
        });

        $("#btnLoadRuleSet").on("click", function () {
            ObjectAdvanceSearchRules.getRuleSelected();
            ObjectAdvanceSearchRules.clearAdvSearchRules();
            ObjectAdvanceSearchRules.dropDownRules.hide();
        })
    },

    clearAdvSearchRules: function () {
        $("#panelFilterRuleSets").formReset();
        $("#RuleSetListAdvSearch").UifListView({
            source: null,
            displayTemplate: "#template-RuleSetListAdvSearch",
            selectionType: 'single',
            height: 260
        });
    },
    getRuleSelected: function () {
        var dataRuleSelected = $("#RuleSetListAdvSearch").UifListView("getSelected");
        if (dataRuleSelected.length > 0) {
            ControlRuleSet.fillRulePack(dataRuleSelected);
        }
    },
    dropDownRules: uif2.dropDown({
        source: rootPath + 'RulesScripts/RuleSet/AdvancedSearch',
        element: '#btnSearchAdvRules',
        align: 'right',
        width: 600,
        height: 500
    })
}

ObjectAdvanceSearchRules.bindEvents();