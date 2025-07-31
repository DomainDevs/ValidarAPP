// namespace global
var languagei18n = languagei18n || {};
languagei18n = {
    ln: 'es',
    include: function (filePath) {
        var scriptJs = document.createElement('script');
        scriptJs.setAttribute('type', 'text/javascript');
        scriptJs.setAttribute('src', filePath);
        document.getElementsByTagName('head')[0].appendChild(scriptJs);
    },
    includelanguage: function (filePath) {
        var sc = document.getElementsByTagName("script");
        for (var x in sc)
            if (sc[x].src != null && sc[x].src.indexOf(file_path) != -1)
                return;
        include(filePath);
    },
}
$(document).ready(function () {
    languagei18n.ln = x = navigator.browserLanguage || window.navigator.language || window.lang;
    if (languagei18n.ln.indexOf("es") > -1) {
        //var url = "https://code.jquery.com/color/jquery.color.js";
        //$.getScript(url, function () { }).fail;
        var file = {};
        file = "Areas/Person/Scripts/Person/Resources.Language.";
        if (languagei18n.ln.length >= 2) {
            files.push(paths[1] + pkg + '-' + lang.substring(0, 2) + '.js');
        }
        file = rootPath + file + languagei18n.ln + ".js";
        includelanguage(+ "Resources." + "Person." + ".js");
    };
});
