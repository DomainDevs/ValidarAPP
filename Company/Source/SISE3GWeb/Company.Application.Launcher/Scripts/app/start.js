var uif2 = new Uif2.App();
//mientras se define el manejo del scroll se coloco aqui
function ScrollTop() {
    $('.main-container').animate({ scrollTop: 0 }, "slow");
}
var GetQueryParameter = function (parameterName) {
    var queryParameters = window.location.search.substring(1);
    var parameters = queryParameters.split("&");
    for (var i = 0; i < parameters.length; i++) {
        var parameter = parameters[i].split("=");
        if (parameter[0] == parameterName) {
            return parameter[1];
        }
    }
}
$.validator.setDefaults({
    ignore: ':hidden, [readonly=readonly]'
});