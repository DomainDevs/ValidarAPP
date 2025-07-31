class SubLineBusinessRequest {
    static GetLineBusiness() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/SubLineBusiness/GetsLinesBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetSubLineBusinessByNameAnTitle(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/SubLineBusiness/GetListSubLineBusinessByName",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchAdvancedSubLineBusiness(SubLinesBusiness) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/SubLineBusiness/GetSubLineBusinessAdvancedSearch',
            data: JSON.stringify({ subLineBusinessView: SubLinesBusiness }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}