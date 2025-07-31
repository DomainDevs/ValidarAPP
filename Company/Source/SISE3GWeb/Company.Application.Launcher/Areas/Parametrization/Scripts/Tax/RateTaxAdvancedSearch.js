class RateTaxAdvancedSearch extends Uif2.Page {
    getInitialState() {
        
    }
    bindEvents() {
        $('#SearchAdvancedRateTax').click(RateTaxAdvancedSearch.AdvancedSearch);
        $('#btnCancelSearchAdvVehicle').click(RateTaxAdvancedSearch.CloseAdvancedSearch);
    }

    static AdvancedSearch() {
        $("#formAdvSearchRateTax").validate();
        if ($("#formAdvSearchRateTax").valid()) {

        }
    }

    static CloseAdvancedSearch() {
        $('.uif-dropdown').hide();
    }
}