//Seccion Funciones
var objOther =
    {
        bindEvents: function () {
        },
        showOthers: function (data) {
            window.TypeGuarantee = GuaranteeType.Others;
            $("#Others-Observations").val(data.InsuredGuarantee.DescriptionOthers);
        },

        clearOthers: function () {
            $("#Others-Observations").val("");
        },

        bindEventsOthers: function () {
            window.TypeGuarantee = GuaranteeType.Others;
        },
        SaveOthers: function () {

            $("#Guarantee").validate();
            $("#AddOthers").validate();

            var guaranteeValid = $("#Guarantee").valid();
            var promissoryNoteValid = $("#AddOthers").valid();

            if (guaranteeValid && promissoryNoteValid) {
                objOther.pushOthers(guaranteeId);
                return true;
            }
            return false;
        },

        pushOthers: function (id) {

            var result = Guarantee.existGuarantee(id);
            var dataResult;

            if (result >= 0) {
                guarantee[result].InsuredGuarantee.DescriptionOthers = $("#Others-Observations").val();
                guarantee[result].InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;
                guarantee[result].InsuredGuarantee.Apostille = false;
                guarantee[result].Apostille = false;
                guarantee[result].InsuredGuarantee.LastChangeDate = FormatDate(new Date());

                dataResult = guarantee[result];
            }
            else {
                guaranteeTmp.InsuredGuarantee.Apostille = false;
                guaranteeTmp.Code = GuaranteeType.Others;
                guaranteeTmp.Description = "OTROS";
                guaranteeTmp.Apostille = false;

                var guaranteeType = {};
                guaranteeType.Code = GuaranteeType.Others;
                guaranteeType.Description = "OTROS";

                guaranteeTmp.GuaranteeType = guaranteeType;

                var Branch = {};
                Branch.Id = $("#selectBranchGuarantee").val();

                var GuaranteeStatus = {};
                GuaranteeStatus.Id = $("#selectStatusGuarantee").val();
                GuaranteeStatus.Description = $("#selectStatusGuarantee option[value='" + GuaranteeStatus.Id + "']").text();

                guaranteeTmp.InsuredGuarantee.Code = guaranteeCode;
                guaranteeTmp.InsuredGuarantee.IndividualId = individualId;
                guaranteeTmp.InsuredGuarantee.Branch = Branch;
                guaranteeTmp.InsuredGuarantee.DescriptionOthers = $("#Others-Observations").val();
                guaranteeTmp.InsuredGuarantee.IsCloseInd = $('#IsClosed').prop("checked") ? 1 : 0;
                guaranteeTmp.InsuredGuarantee.LastChangeDate = FormatDate(new Date());
                guaranteeTmp.InsuredGuarantee.Status = GuaranteeStatus
                dataResult = guaranteeTmp;
            }
            Guarantee.saveGuarantee(dataResult, GuaranteeType.Others);
        }
    }
