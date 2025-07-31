//Codigo de la pagina Temporal.cshtml
class Endorsement extends Uif2.Page {
    getInitialState() {
        glbPolicy = { Id: 0, TemporalType: TemporalType.Endorsement, Endorsement: { EndorsementType: EndorsementType.Modification } };
        var temporalId = Underwriting.getQueryVariable("temporalId");        
        TemporalRequest.GetTemporalByIdTemporalType(temporalId, glbPolicy.TemporalType, 0).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    Underwriting.UpdateGlbPolicy(data.result);
                    if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                        Underwriting.LoadSiteRisk();
                    }
                }
            }
        });
    }
    bindEvents() {
    }
    static getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) {
                return pair[1];
            }
        }
    }
}