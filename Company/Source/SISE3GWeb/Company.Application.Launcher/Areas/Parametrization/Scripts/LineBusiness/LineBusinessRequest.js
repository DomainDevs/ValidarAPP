class LineBusinessRequest {
    
    static GetLineBusinessById(id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/GetLinesBusinessById',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
	
	static GetLineBusinessByDescriptionById(description, id) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/LineBusiness/GetLineBusinessByDescriptionById',
			data: JSON.stringify({ description: description, id: id }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}

    static GetLineBusinessByDescriptionId(description, id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/GetLinesBusinessByDescription',
            data: JSON.stringify({ description: description, id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetPerilByLineBusinessId(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/LineBusiness/GetPerilByLineBusinessId',
            data: JSON.stringify({ idLineBusiness: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetObjectByLineBusinessId(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/LineBusiness/GetObjectByLineBusinessId',
            data: JSON.stringify({ idLineBusiness: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
    static SaveLineBusiness(linebusiness) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/SaveLineBusiness',
            data: JSON.stringify({ linebusinesModel: linebusiness }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static DeleteLineBusiness(id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/DeleteLineBusiness',
            data: JSON.stringify({ idLineBusiness: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
	
	static SaveInsuredObject(idLineBusiness, insuranceObjects) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/LineBusiness/SaveInsuredObject',
			data: JSON.stringify({ idLineBusiness: idLineBusiness, insuranceObjects: insuranceObjects }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}

	static SavePeril(idLineBusiness, perils) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/LineBusiness/SavePeril',
			data: JSON.stringify({ idLineBusiness: idLineBusiness, perils: perils }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}

	static SaveClause(idLineBusiness, clauses) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/LineBusiness/SaveClause',
			data: JSON.stringify({ idLineBusiness: idLineBusiness, clauses: clauses }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
		});
	}
}

class LineBusinessCoveredRiskTypeRequest {
    static GetRiskType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/GetRiskTypeLineBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}


class LineBusinessProtectionsRequest {
    static GetPerils() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/GetPerils',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}


class LineBusinessInsuranceObjectsRequest {
    static GetInsuranceObjects() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/LineBusiness/GetInsuranceObjects',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}
class LineBusinessClausesRequest {
    static GetClauses() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Parametrization/LineBusiness/GetClauses',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class LineBusinessSearchAdvancedRequest {
    static SearchAdvancedLineBusiness(description, CoveredRiskType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/LineBusiness/GetLineBusinessAdvancedSearch',
            data: JSON.stringify({ description: description, CoveredRiskType: CoveredRiskType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}