class ScriptRequestT {
    static GetAllScriptByLevelId() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetAllScriptByLevelId',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}