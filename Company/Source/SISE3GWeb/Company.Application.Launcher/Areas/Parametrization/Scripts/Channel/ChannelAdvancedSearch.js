var dropDownSearchAdvChannel = null;
$(() => {
    new ChannelAdvancedSearch();
});
class ChannelAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvChannel = uif2.dropDown({
            source: rootPath + 'Parametrization/Channel/ChannelAdvancedSearch',
            element: '#btnSearchAdvChannel',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }   
    componentLoadedCallback() {
        $("#lvSearchAdvChannel").UifListView({
            displayTemplate: "#ChannelTemplate",
            selectionType: "single",
            height: 450
        });

        $("#btnSearchAdvChannel").on("click", ChannelAdvancedSearch.SearchAdvChannel);
        $("#btnCancelSearchAdv").on("click", ChannelAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", ChannelAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvChannel() {
        dropDownSearchAdvChannel.show();
    }

    static CancelSearchAdv() {
        ParametrizationChannel.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvChannel").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationChannel.ShowData(null, data, data.key);
        }
        ParametrizationChannel.HideSearchAdv();
    }
}