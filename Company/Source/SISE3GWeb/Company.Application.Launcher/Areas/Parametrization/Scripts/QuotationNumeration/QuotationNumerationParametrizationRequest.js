class QuotationNumerationParametrizationRequest {
    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/QuotationNumeration/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
    static GetBranchs() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/QuotationNumeration/GetBranchs',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
	static GetQuotationNumerationByBranchIdPrefixId(branchId, prefixId) {
        return $.ajax({
            type: 'POST',
			url: rootPath + 'Parametrization/QuotationNumeration/GetQuotationNumerationByBranchIdPrefixId',
			data: JSON.stringify({ branchId: branchId, prefixId: prefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
	}

	//Guardar
	static Save(quotationNumerationViewModel) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/QuotationNumeration/Save',
			data: JSON.stringify({ quotationNumerationViewModel: quotationNumerationViewModel }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}

	//Eliminar
	static Delete(quotationNumerationViewModel) {
		return $.ajax({
			type: 'POST',
			url: rootPath + 'Parametrization/QuotationNumeration/Delete',
			data: JSON.stringify({ quotationNumerationViewModel: quotationNumerationViewModel }),
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});
	}
    
	static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
			url: rootPath + 'Parametrization/QuotationNumeration/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}