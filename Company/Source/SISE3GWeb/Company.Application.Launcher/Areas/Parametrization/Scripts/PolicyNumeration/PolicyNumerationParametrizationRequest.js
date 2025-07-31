class PolicyNumerationParametrizationRequest {
    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/PolicyNumeration/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
    static GetBranchs() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/PolicyNumeration/GetBranchs',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
	static GetPolicyNumerationByBranchIdPrefixId(branchId, prefixId) {
        return $.ajax({
            type: 'POST',
			url: rootPath + 'Parametrization/PolicyNumeration/GetPolicyNumerationByBranchIdPrefixId',
			data: JSON.stringify({ branchId: branchId, prefixId: prefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
	}

	//Guardar
	static Save(PolicyNumerationViewModel) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/PolicyNumeration/Save',
			data: JSON.stringify({ PolicyNumerationViewModel: PolicyNumerationViewModel }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}

	//Eliminar
	static Delete(PolicyNumerationViewModel) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/PolicyNumeration/Delete',
			data: JSON.stringify({ PolicyNumerationViewModel: PolicyNumerationViewModel }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}
    
	static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
			url: rootPath + 'Parametrization/PolicyNumeration/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}