class GuaranteeStatusRouteRequest extends Uif2.Page {
    static GetGuaranteeStatus() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/GuaranteeStatusRoute/GetGuaranteeStatus',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveGuaranteeStatusRoute(allGuaranteeEstatusAssign, guaranteeStatusId) {

        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/GuaranteeStatusRoute/CreateGuaranteeStatusRoutes',
            data: JSON.stringify({ allGuaranteeEstatusAssign: allGuaranteeEstatusAssign  , guaranteeStatusId: guaranteeStatusId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetListGuaranteeStatus(guaranteeStatusId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/GuaranteeStatusRoute/GetUnassignedGuaranteeStatusByGuaranteeStatusId',
            data: JSON.stringify({ guaranteeStatusId: guaranteeStatusId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGuaranteeStatusRoutesByGuaranteeStatusId(guaranteeStatusId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/GuaranteeStatusRoute/GetGuaranteeStatusRoutesByGuaranteeStatusId',
            data: JSON.stringify({ guaranteeStatusId: guaranteeStatusId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    bindEvents() {
     
    }
     
}