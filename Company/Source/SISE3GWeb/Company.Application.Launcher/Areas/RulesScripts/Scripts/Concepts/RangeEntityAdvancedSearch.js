var dropDownSearchAdvRange;
class ConceptRangeEntityAdvanced extends Uif2.Page {
    getInitialState() {
        this.setDropDow();
    }
    bindEvents() {

    }
    setDropDow() {
        dropDownSearchAdvRange = uif2.dropDown({
            source: rootPath + 'RulesScripts/Concepts/RangeEntityAdvancedSearch',
            element: '#btnSearchAdvRange',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: ConceptRangeEntityAdvanced.componentLoadedCallback
        });
    }
    static setRangeEntityAdv(data) {
        $("#lvRangeEntityAdv").UifListView({
            sourceData: data,
            displayTemplate: "#advancedSearchTemplate",
            selectionType: 'single',
            source: null,
            height: 470
        });
    }
    static componentLoadedCallback() {
        ConceptRangeEntityAdvanced.setRangeEntityAdv(null);
        $("#lvRangeEntityAdv").on("itemSelected", ConceptRangeEntityAdvanced.itemSelectedAdvRangeEntity);

    }
    static showdropDownSearchAdvRange() {
        $("#lvRangeEntityAdv").UifListView("clear");
        dropDownSearchAdvRange.show();
    }
    static itemSelectedAdvRangeEntity(event, item) {
        const find = function (element, index, array) {
            element.Index = index;
            return parseInt(element.RangeEntityCode) === parseInt(item.RangeEntityCode)
        }
        var itemRangeEntity = $("#lsvRangeEntity").UifListView("find", find);
        console.log(itemRangeEntity);
        ConceptRangeEntity.editRangeEntity(null, itemRangeEntity, itemRangeEntity.Index);
        dropDownSearchAdvRange.hide();
    }
    static searchRangeEntity(event, value) {
        if (value != null) {
            $("#inputSearchRangeEntity").val("");
            var rangeEntityValue = $("#lsvRangeEntity").UifListView("getData");

            rangeEntityValue = rangeEntityValue.filter(item => { return parseInt(item.RangeEntityCode) >= 0 && item.Description.toLowerCase().sistranReplaceAccentMark().includes(value.toLowerCase().sistranReplaceAccentMark()) });
            if (rangeEntityValue.length > 0) {
                ConceptRangeEntityAdvanced.setRangeEntityAdv(rangeEntityValue);
                dropDownSearchAdvRange.show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrMsgRangeEntityNotFound, 'autoclose': true });
            }
        }
    }
}