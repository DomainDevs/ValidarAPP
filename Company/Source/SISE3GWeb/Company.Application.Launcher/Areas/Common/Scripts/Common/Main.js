class MainRequest {
    static GetParameters() {
        return $.ajax({
            type: 'POST',
            url: "GetParameters",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
        });
    };

    static ajax(url, data, type) {
        return $.ajax({
            type: type,
            url: rootPath + url,
            data: data,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    };   

}