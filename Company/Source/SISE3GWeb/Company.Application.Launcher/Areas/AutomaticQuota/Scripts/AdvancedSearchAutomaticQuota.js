class AdvancedSearchAutomaticQuota extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: true });
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'AutomaticQuota/AutomaticQuota/AdvancedSearchAutomaticQuota',
            element: '#btnShowAdvancedAq',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.AdvancedSearchEventsAQ
        });
       
    }

    AdvancedSearchEventsAQ() {
        $("#btnCancelSearchAutomatic").on("click", AdvancedSearchAutomaticQuota.CancelSearchAutomatic);

        AdvancedSearchAutomaticQuota.GetPersonType();
    }

    //Seccion Eventos
    bindEvents() {

    }

    static CancelSearchAutomatic() {
        dropDownSearch.hide();
    }
    

    static GetPersonType() {
        $("#selectTypePersonAq").UifSelect('disabled', false);
        AutomaticQuotaRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $("#selectTypePersonAq").UifSelect({ sourceData: data.result });                             
            }
        });
    }
}