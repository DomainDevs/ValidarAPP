var objBindPolicy =
    {
        bindEvents: function () {
            $("#btnBindPolicy").click(function () {
                objBindPolicy.LoadPartialBindPolicy();
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
        },
        LoadPartialBindPolicy: function (){
            Guarantee.HidePanelsGuarantee();
            $("#buttonsGuarantee").hide();
            Guarantee.ShowPanelsGuarantee(MenuType.BINDPOLICY);
            //$("#listBindPolicyHistory").UifListView({ source: rootPath + 'Person/Guarantee/GetInsuredPolicyGuaranteeByIndividualId?individualId=' + individualId + '&guaranteeId=' + guaranteeId, displayTemplate: "#display-bindPolicyHistory" });
            $("#listBindPolicyHistory").UifListView({ source: rootPath + 'Guarantees/Guarantees/GetInsuredPolicyGuaranteeByIndividualId?individualId=' + individualId + '&guaranteeId=' + guaranteeId, displayTemplate: "#display-bindPolicyHistory" });
        }
    }

