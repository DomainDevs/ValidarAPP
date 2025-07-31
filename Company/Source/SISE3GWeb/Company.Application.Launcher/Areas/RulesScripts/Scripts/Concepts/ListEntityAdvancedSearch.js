var dropDownSearchAdvList;
class ConceptListEntityAdvanced extends Uif2.Page {
    getInitialState() {
        this.setDropDow();        
    }
    bindEvents() {
        
    }
    setDropDow() {
        dropDownSearchAdvList = uif2.dropDown({
            source: rootPath + "RulesScripts/Concepts/ListEntityAdvancedSearch",
            element: "#btnSearchAdvRange",
            align: "right",
            width: 550,
            height: 551,
            loadedCallback: ConceptListEntityAdvanced.componentLoadedCallback
        });
    }
    static setListEntityAdv(data) {
        $("#lvListEntityAdv").UifListView({
            sourceData: data,
            displayTemplate: "#advancedSearchTemplate",
            selectionType: "single",
            source: null,
            height: 470
        });
    }
    static componentLoadedCallback() {
        ConceptListEntityAdvanced.setListEntityAdv(null);
        $("#lvListEntityAdv").on("itemSelected", ConceptListEntityAdvanced.itemSelectedAdvListEntity);
        
    }
    static showdropDownSearchAdvList() {
        $("#lvListEntityAdv").UifListView("clear");
        dropDownSearchAdvList.show();
    }
    static itemSelectedAdvListEntity(event, item) {
        const find = function (element, index, array) {
            element.Index = index;
            return parseInt(element.ListEntityCode) === parseInt(item.ListEntityCode);
        }
        var itemListEntity = $("#lsvListEntity").UifListView("find", find);
        ConceptListEntity.editListEntity(null, itemListEntity, itemListEntity.Index);
        dropDownSearchAdvList.hide();
    }
    static searchListEntity(event, value) {
        if (value != null) {
            $("#inputSearchListEntity").val("");
            var listEntityValue = $("#lsvListEntity").UifListView("getData");

            listEntityValue = listEntityValue.filter(item => { return parseInt(item.ListEntityCode)>=0 && item.Description.toLowerCase().sistranReplaceAccentMark().includes(value.toLowerCase().sistranReplaceAccentMark()) });
            if (listEntityValue.length > 0)
            {
                ConceptListEntityAdvanced.setListEntityAdv(listEntityValue);
                dropDownSearchAdvList.show();
            }
            else
            {
                $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrMsgListEntityNotFound, 'autoclose': true });
            }
        }
    }    
}