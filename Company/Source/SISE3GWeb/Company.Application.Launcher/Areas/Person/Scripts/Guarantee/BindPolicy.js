class BindPolicy extends Uif2.Page {

    getInitialState() {

    }

    bindEvents() {
        $("#btnBindPolicy").click(function () {
            BindPolicy.LoadPartialBindPolicy();
        });
        $("#btnCancelBindPolicy").click(function () {
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        });

        $("#btnSearchPolicy").click(function () {
            var searchModel = $('#BindPolicy').serializeObject();
            var controller = rootPath + 'Endorsement/Search/GetEndorsementsByPrefixIdBranchIdPolicyNumber?branchId=' + searchModel.Branch_Cd + '&prefixId=' + searchModel.Prefix_Cd + '&policyNumber=' + searchModel.PolicyNumber + '&current=' + true;
            $('#selectEndorsement').UifSelect({ source: controller });
        });
    }

    static LoadPartialBindPolicy() {
        Guarantee.HidePanelsGuarantee();
        $("#buttonsGuarantee").hide();
        Guarantee.ShowPanelsGuarantee(MenuType.BINDPOLICY);
        $("#listBindPolicyHistory").UifListView({ source: rootPath + 'Person/Guarantee/GetInsuredPolicyGuaranteeByIndividualId?individualId=' + individualId + '&guaranteeId=' + guaranteeId, displayTemplate: "#display-bindPolicyHistory" });
    }

}

