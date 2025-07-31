class SharedEndorsementBase {

    static SetPolicyBase(form) {
        if (!Isnull(glbPolicy) && !Isnull(form)) {

            var policyBase = $(form).serializeObject();
            policyBase.IssueDate = FormatFullDate($("#inputIssueDate").text());
            policyBase.BranchId = glbPolicy.Branch.Id;
            policyBase.PrefixId = glbPolicy.Prefix.Id;
            policyBase.PolicyId = glbPolicy.Endorsement.PolicyId;
            policyBase.PolicyNumber = glbPolicy.DocumentNumber;
            policyBase.EndorsementId = glbPolicy.Endorsement.Id;
            policyBase.TemporalId = glbPolicy.Id;
            policyBase.Title = glbPolicy.Title;
            policyBase.EndorsementController = glbPolicy.EndorsementController;
            policyBase.EndorsementFrom = FormatDate(glbPolicy.CurrentFrom);
            policyBase.EndorsementTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
            policyBase.ProductIsCollective = glbPolicy.Product.IsCollective;
            policyBase.Message = '';
            policyBase.HasEvent = glbPolicy.HasEvent;
            return policyBase;
        }
    }
}