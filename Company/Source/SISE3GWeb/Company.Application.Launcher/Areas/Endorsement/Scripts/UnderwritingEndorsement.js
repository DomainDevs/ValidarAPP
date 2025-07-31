//Codigo de la pagina Temporal.cshtml
class UnderwritingEndorsement extends Uif2.Page {
    getInitialState() {
        
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            TemporalRequest.GetTemporalByIdTemporalType(glbPolicy.Id, glbPolicy.TemporalType, 0).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        Underwriting.UpdateGlbPolicy(data.result);
                        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                            if (!UnderwritingDecimal) {
                                Underwriting.LoadDecimal();     
                            }
                            Underwriting.LoadSiteRisk();
                        }
                    }
                }
            });
        }
    }
    bindEvents() {
    }
}