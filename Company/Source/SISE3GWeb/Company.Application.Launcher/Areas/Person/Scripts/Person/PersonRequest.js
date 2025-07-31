
class BranchRequest {
    static GetBranchs() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetBranchs",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
}

class LineBusiness {
    static GetLinesBusiness() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetLinesBusiness",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class PartnerRequest {
    static GetAplicationPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationPartnerByDocumentIdDocumentTypeIndividualId",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ documentId: documentId, documentType: documentType, IndividualId: IndividualId })
        });
    }
    static GetAplicationPartnerByIndividualId(PartnerNew, opcion) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationPartnerByIndividualId",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ IndividualId: PartnerNew.IndividualId }),
        }).done(function (data) {
            $.UifProgress('close');
            Partner.LoadPartners(data.result);
        }).fail(function (data) {
            $.UifProgress('close');
        });
    }

    static CreateAplicationPartner(partnerDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/CreateAplicationPartner",
            data: JSON.stringify({
                partnerDTO: partnerDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class CountryRequest {
    static GetCountries() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetCountries",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountriesByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCountriesByDescription",
            data: JSON.stringify({ Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountriesById(id_country) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCountryByCountryId",
            data: JSON.stringify({ country_Id: id_country }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class StateRequest {
    static GetStatesByCountryIdByDescription(countryId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetStatesByCountryIdByDescription",
            data: JSON.stringify({ CountryId: countryId, Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetStatesByCountryId",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCitiesByCountryIdStateId",
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class CityRequest {
    static GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetCitiesByCountryIdByStateIdByDescription',
            data: JSON.stringify({ CountryId: countryId, StateId: stateId, Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class DaneCodeRequest {
    static GetDaneCodeByCountryIdByStateIdByCityId(countryId, stateId, cityId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetDaneCodeByCountryIdByStateIdByCityId",
            data: JSON.stringify({
                countryId: countryId,
                stateId: stateId,
                cityId: cityId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCountryAndStateAndCityByDaneCode",
            data: JSON.stringify({ CountryId: countryId, DaneCode: daneCode }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class GenderRequest {
    static GetGenderTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetGenderTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class MaritalStatusRequest {
    static GetMaritalStatus() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetMaritalStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class PhoneRequest {
    static GetPhoneTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPhoneTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class EmailRequest {
    static GetEmailTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetEmailTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class ExonerationRequest {
    static GetExonerationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetExonerationTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class AddressRequest {
    static GetAddressesTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAddressesTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAddressesTypesByIsEmail(IsEmail) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAddressesTypesByIsEmail",
            data: JSON.stringify({
                IsEmail: IsEmail
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class AssociationTypeRequest {
    static GetAssociationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAssociationTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class CompanyTypeRequest {
    static GetCompanyTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCompanyTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}


class EconomicActivityRequest {
    static GetEconomicActivitiesByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetEconomicActivitiesByDescription",
            data: JSON.stringify({
                description: description
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class AgentRequest {
    static GetAgentDeclinedTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgentDeclinedTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgents(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgents",
            data: JSON.stringify({
                query: query
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgenciesByAgentIdDescription(agentId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgenciesByAgentIdDescription",
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgenciesByAgentId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgenciesByAgentId",
            data: JSON.stringify({
                agentId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgent(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetAgentByIndividualId',
            data: JSON.stringify({ IndividualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgentPrefix(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetAgentPrefixByIndividualId',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "IndividualId": individualId })
        });
    }

    static CreateAgent(agent) {
        return $.ajax({
            type: "POST",
            dataType: "json",
            url: rootPath + 'Person/Person/CreateAgent',
            data: JSON.stringify({ agentDTOs: agent }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgentTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgentTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGroupAgent() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetGroupAgent',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetSalesChannel() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSalesChannel",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetEmployeePersonByFullName(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetEmployeePersonByFullName",
            data: JSON.stringify({
                description: name

            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgenAgencytByAgentCodeAndType(agentCode, agentType) {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + "Person/Person/GetAgenAgencytByAgentCodeAndType",
            data: JSON.stringify({ agentCode: agentCode, agentType: agentType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class CoInsurerRequest {
    static GetCoInsurer(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCompanyCoInsuredIndivualID",
            data: JSON.stringify({
                individualId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateCoInsurer(coInsurerTmp) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/CreateCoInsurer",
            data: JSON.stringify({
                companyCoInsuredDTO: coInsurerTmp
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateCompanyCoInsured(coInsurerTmp) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/UpdateCompanyCoInsured",
            data: JSON.stringify({
                companyCoInsuredDTO: coInsurerTmp
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class ReInsurerRequest {
    static GetAplicationReInsurerByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationReInsurerByIndividualId",
            data: JSON.stringify({
                IndividualId: IndividualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateAplicationReInsurer(reInsurerDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/CreateAplicationReInsurer",
            data: JSON.stringify({
                ReInsurerDTO: reInsurerDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class CurrencyRequest {
    static GetCurrencies() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCurrencies",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class InsuredRequest {
    static GetInsuredGuaranteeByIndividualId(individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Guarantee/GetInsuredGuaranteeByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetInsuredByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetInsuredByIndividualId",
            data: JSON.stringify({
                "individualId": individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static CreateInsuredConsortium(insured) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateInsuredConsortium',
            data: JSON.stringify({ "insured": insured }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredAgent(modelInsuredAgent) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateInsuredAgent',
            data: JSON.stringify({ "insuredAgent": modelInsuredAgent }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsured(insured) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateInsured',
            data: JSON.stringify({ "insured": insured }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredsByDescription(description, insuredSearchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetInsuredsByDescription',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredDeclinedTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetInsuredDeclinedTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsProfiles() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetInsuredProfile",
            dataType: "json"
        });
    }

    static GetInsMain(dataName) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetInsuredsByName",
            data: { name: dataName },
            dataType: "json"
        });
    }

    static GetInsSegment() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetInsuredSegment",
            dataType: "json"
        });
    }

    static GetInsBranchs() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetBranchs",
            dataType: "json"
        });
    }
}

class DocumentTypeRequest {
    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetDocumentType",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
}
class EducativeLevelRequest {
    static GetEducativeLevels() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetEducativeLevels",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class HouseTypeRequest {
    static GetHouseTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetHouseTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class SocialLayerRequest {
    static GetSocialLayers() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSocialLayers",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class OccupationRequest {
    static GetOccupations() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetOccupations",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class SpecialtyRequest {
    static GetSpecialties() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSpecialties",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSpecialtiesTable() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSpecialtiesTable",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class IncomeLevelRequest {
    static GetIncomeLevels() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetIncomeLevels",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class PaymentMethodRequest {
    static GetPaymentMethods() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPaymentMethods",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPaymentTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentConcept() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPaymentConcept",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualpaymentMethodByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + "Person/Person/GetIndividualpaymentMethodByIndividualId?individualId=" + individualId,
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateIndividualpayment(individualPayment, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/createIndividualPayment",
            data: JSON.stringify({ "individualPaymentMethods": individualPayment, "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}

class BankRequest {
    static GetBanks() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetBanks",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBankBranches(bankId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetBankBranches?bankId=" + bankId,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}
class OperatingQuotaRequest {
    static GetOperatingQuota(individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/GetOperatingQuotaByIndividualId?individualId=' + individualId,
            data: JSON.stringify({ individualId: individualId }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static DeleteOperatingQuota(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/DeleteOperatingQuota',
            data: JSON.stringify({ operatingQuotaDTO: data }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static SaveOperatingQuotaByEvent(operatingQuotaDTOs, OperatingQuotaEventDTOs) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/SaveOperatingQuotaByEvent',
            data: JSON.stringify({ operatingQuotaDTOs: operatingQuotaDTOs, OperatingQuotaEventDTOs: OperatingQuotaEventDTOs }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }
}

class EconomicActivityTaxRequest{
    static GetEconomicActivitiesTax() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Person/Person/GetEconomicActivitiesTax",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class TaxRequest {
    static SaveTax(listIndividualTaxExeption) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateIndividualTax',
            data: JSON.stringify({ "listIndividualTaxExeption": listIndividualTaxExeption }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static getTaxAttributeTypeByTaxId(taxId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/getTaxAttributeTypeByTaxId',
            data: JSON.stringify({ taxId: taxId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static getTaxRateByTaxIdByAttributes(taxId, taxConditionId, taxCategoryId, countryCode, stateCode, cityCode, economicActivityCode, prefixId, coverageId, technicalBranchId) {

        var data = JSON.stringify({taxId: taxId, taxConditionId: taxConditionId, taxCategoryId: taxCategoryId,
            countryCode: countryCode, stateCode: stateCode, cityCode: cityCode,
            economicActivityCode: economicActivityCode, prefixId: prefixId, coverageId: coverageId,
            technicalBranchId: technicalBranchId
        });
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/getTaxRateByTaxIdByAttributes',
            data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static getTaxRateById(taxRateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/getTaxRateById',
            data: JSON.stringify({ taxRateId: taxRateId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    //static UpdateTax(listIndividualTaxExeption, individualId) {
    //    return $.ajax({
    //        type: "POST",
    //        url: rootPath + 'Person/Person/UpdateIndividualTaxExeption',
    //        data: JSON.stringify({ listIndividualTaxExeption: listIndividualTaxExeption }),
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8"
    //    });
    //}

    static DeleteTax(IndividualTaxExeptionDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/DeleteIndividualTaxExeption',
            data: JSON.stringify({ IndividualTaxExeptionDTO: IndividualTaxExeptionDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualTaxExeptionByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetIndividualTaxExeptionByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }
        )
    }

    static GetTax() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetTax",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompanyIndividualRoleByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetCompanyIndividualRoleByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetStatesByStateTaxId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetStatesByCountryId",
            dataType: "json",
            data: JSON.stringify({ countryId: countryId }),
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class PrefixRequest {
    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class ProviderRequest {
    //wmw usado
    static CreateSupplier(objBasicInformationProvider) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateSupplier',
            data: JSON.stringify({ provider: objBasicInformationProvider }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSupplierTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSupplierTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetSupplierDeclinedType() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSupplierDeclinedType",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetSupplierByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSupplierByIndividualId",
            data: JSON.stringify({ IndividualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            $.UifProgress('close');
            if (data.success === true) {
                Provider.GetProvider(data.result);
            } else {

                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (data) {
            $.UifProgress('close');
        });
    }
    static ValidateSupplierByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSupplierByIndividualId",
            data: JSON.stringify({ IndividualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        })
    }

    static GetSupplierProfiles(suppilierTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSupplierProfiles",
            data: JSON.stringify({ suppilierTypeId: suppilierTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetGroupSupplier() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetGroupSupplier",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    
    static GetAccountingConcepts() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAccountingConcepts",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetSupplierAccountingConceptsBySupplierId(SupplierId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSupplierAccountingConceptsBySupplierId",
            data: JSON.stringify({ SupplierId: SupplierId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    //wmw fin usado

    static GetOriginTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetOriginTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}



class PersonRequest {

    static ValidateListRiskPerson(documentNumber, fullName) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Person/Person/ValidateListRiskPerson",
            data: { "documentNumber": documentNumber, "fullName": fullName},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEnumsRoles() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Person/Person/GetEnumsRoles",
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrentDatetime() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/DateNow',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetParameters() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetParametersRouter",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadInitialData(isEmail) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/LoadInitialData",
            data: JSON.stringify({
                isEmail: isEmail
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: false
        });
    }

    static LoadInitialLegalData(typeDocument) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/LoadInitialLegalData",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetPersonTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetPersonTypes",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: false
        });
    }

    static GetPersonJobByIndividualId(individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetPersonJobByIndividualId",
            data: JSON.stringify({
                individualId: individualId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetPersonByDocumentNumberByNameByFirstLastName(searchType, documentNumber) {

        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationPersonByDocument",
            data: JSON.stringify({ searchType: searchType, documentNumber: documentNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPerson2gByDocumentNumber(documentNumber, company) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPerson2gByDocumentNumber",
            data: JSON.stringify({ documentNumber: documentNumber, company: company }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
       
    static GetPerson2gByPersonId(personId, company) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPerson2gByPersonId",
            data: JSON.stringify({ personId: personId, company: company }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompany2gByPersonId(personId, company) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetCompany2gByPersonId",
            data: JSON.stringify({ personId: personId, company: company }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetAplicationCompanyByDocument(searchType, documentNumber, name) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationCompanyByDocument",
            data: JSON.stringify({
                documentNumber: documentNumber, name: name, searchType: searchType
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProspectByDocumentNum(documentNumber, searchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetProspectByDocumentNum",
            data: JSON.stringify({
                documentNum: documentNumber,
                searchType: searchType
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompanyByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationCompanyById",
            data: JSON.stringify({
                Id: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonByIndividualId(individualId) {

        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAplicationPersonById",
            data: JSON.stringify({
                individualId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultValues() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetDefaultValues",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProspectByDocumenNumberOrDescription(prospectId, insuredSearchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetProspectByDocumenNumberOrDescription",
            data: JSON.stringify({ description: prospectId, insuredSearchType: insuredSearchType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreatePerson(personData) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/CreateAplicationPerson",
            data: JSON.stringify({ personDTO: personData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrentDate() {
        return $.ajax({
            url: rootPath + 'Person/Person/GetDate',
            type: 'POST',
            contentType: 'application/json; charset=utf-8;',
            dataType: 'json'
        });
    }

    static SavePersonJob(LaboralPersonalInformation) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/UpdatePersonJob",
            data: { "PersonInformationAndLaborDTO": LaboralPersonalInformation }
        });
    }

    static GetPersonTypesInformationPersonal() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPersonTypesInformationPersonal",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAplicationPersonAdv(personDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetAplicationPersonAdv',
            data: JSON.stringify({ personDTO: personDTO }),

            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAplicationCompanyAdv(companyData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetAplicationCompanyAdv',
            data: JSON.stringify({ companyDTO: companyData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAplicationProspectLegalAdv(companyData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetAplicationProspectLegalAdv',
            data: JSON.stringify({ companyDTO: companyData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAplicationProspectnNaturalAdv(companyData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetAplicationProspectnNaturalAdv',
            data: JSON.stringify({ companyDTO: companyData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateCompany(companyData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateCompany',
            data: JSON.stringify({ companyDTO: companyData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateProspect(prospectData, type) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateProspect',
            data: JSON.stringify({ "prospectModel": prospectData, "type": type }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUserAssignedConsortiumByparameterFutureSocietyByuserId() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetUserAssignedConsortiumByparameterFutureSocietyByuserId',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProspectByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetProspectByIndividualId",
            data: JSON.stringify({
                individualId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonInterestGroups(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPersonInterestGroups",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTypeRolByIndividual(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetTypeRolByIndividual",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonByDocumentNumberSurnameMotherLastName(personData) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetPersonByDocumentNumberSurnameMotherLastName",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetNotificationByIndividualId(individualId) {

        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetIndividualNotification",
            data: JSON.stringify({
                individualId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DisablePolicies(authorizationRequests) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/Authorize/DisablePolicies',
            data: JSON.stringify({ authorizationRequests: authorizationRequests }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonByDocumentByDocumentType(document, documentType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetPersonByDocumentByDocumentType',
            data: JSON.stringify({ document: document, documentType : documentType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompanyByDocumentByDocumentType(document, documentType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetCompanyByDocumentByDocumentType',
            data: JSON.stringify({ document: document, documentType: documentType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByLinesBusinessId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetCoveragesByLinesBusinessId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParameterEmployee() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetParameterEmployee',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}


class Prospect {

    static CreateProspectPersonNatural(prospectData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateProspectPersonNatural',
            data: JSON.stringify({ "prospectModel": prospectData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateProspectPersonLegal(prospectData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateProspectPersonLegal',
            data: JSON.stringify({ "prospectLegal": prospectData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class ComissionAgencyRequest {
    static GetAgencybyIndividualId(IndividualId) {
        return $.ajax({

            url: rootPath + "Person/Person/GetAgencybyIndividualId",
            dataType: "json",
            data: { individualId: IndividualId },
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPrefixesByAgentId(IndividualId) {
        return $.ajax({
            url: rootPath + "Person/Person/GetPrefixesByAgentId",
            dataType: "json",
            data: { individualId: IndividualId },
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLineBusinessByPrefixId(prefixId) {
        return $.ajax({
            url: rootPath + "Person/Person/GetLineBusinessByPrefixId",
            dataType: "json",
            data: { individualId: IndividualId },
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAgentCommissionByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgentCommissionByIndividualId",
            data: JSON.stringify({
                agentId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgentCommissionByAgentId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgentCommissionByAgentId",
            data: JSON.stringify({
                agentId: individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteAgentCommission(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/DeleteAgentCommission',
            data: JSON.stringify({ commissionAgent: data }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateAplicationCompany(companyData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/UpdateAplicationCompany',
            data: JSON.stringify({ companyDTO: companyData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class DocumentTypeRange {
    static GetIndividualTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "DocumentTypeRange/GetIndividualTypes",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });

    }

    static GetListIndividualTypes() {
        $('#selectPersonTypeSearch').on('itemSelected', function (event, selectedItem) {
            $('#tblTypePerson').UifDataTable('clear');
            $.ajax({
                async: false,
                type: "POST",
                url: rootPath + "DocumentTypeRange/GetDocumentTypeRangeById",
                data: JSON.stringify({ "IndividualTypeId": $('#selectPersonTypeSearch').val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done(function (data) {
                if (data.success) {
                    $('#tblTypePerson').UifDataTable({ sourceData: data.result, order: false });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.ajax({
                    type: "POST",
                    url: rootPath + 'DocumentTypeRange/GetDocumentTypeRange',
                    data: JSON.stringify({}),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        $('#tblTypePerson').UifDataTable('clear');
                        if (data.result.length > 0) {
                            $('#tblTypePerson').UifDataTable({ sourceData: data.result });
                        }
                    }
                });
            });
        });
    }

}


class LegalRepresentativeRequest {

    static GetLegalRepresentByIndividualId(individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetLegalRepresentByIndividualId",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateLegalRepresent(legalRepresentative, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateLegalRepresent',
            data: JSON.stringify({ "legalRepresentative": legalRepresentative, "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}


class ConsortiumRequest {

    static CreateConsortiumEvent(consortiumEventDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateConsortiumEvent',
            data: JSON.stringify({ "consortiumEventDTO": consortiumEventDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static AssigendIndividualToConsotiumEvent(consortiumEventDTOs) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/AssigendIndividualToConsotiumEvent',
            data: JSON.stringify({ "consortiumEventDTOs": consortiumEventDTOs }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetConsortiumEventByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetConsortiumEventByIndividualId',
            data: JSON.stringify({ "IndividualId": IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DelteConsortiumEvent(consortiumEventDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/DelteConsortiumEvent',
            data: JSON.stringify({ "consortiumEventDTO": consortiumEventDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateConsortium(Consortium, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateConsortium',
            data: JSON.stringify({ "consorciateds": Consortium, "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetConsortiumInsuredCodeAndIndividualID(InsureCode, IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetConsortiumInsuredCodeAndIndividualID',
            data: JSON.stringify({ "InsureCode": InsureCode, "IndividualId": IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetConsortiumInsuredCode(InsureCode) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetConsortiumInsuredCode',
            data: JSON.stringify({ "InsureCode": InsureCode }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateConsortium(Consorciated) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/UpdateConsortium',
            data: JSON.stringify({ "consorciated": Consorciated }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteConsortium(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/DeleteConsortium',
            data: JSON.stringify({ "InsuredIdDto": data }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class LabourPersonRequest {
    static GetLabourPersonByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetLabourPersonByIndividuald",
            data: JSON.stringify({ IndividualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        }).done(function (data) {
            $.UifProgress('close');
            StaffLabour.GetLabourPerson(data.result)
        }).fail(function (data) {
            $.UifProgress('close');
        });
    }

    static SaveLabour(LaboralPersonalInformation) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/SaveLabourPerson',
            data: JSON.stringify({ labourperson: LaboralPersonalInformation }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class ThirdRequest {
    static GetThirdDeclinedType() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetThirdDeclinedType",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    //Crear tercero
    static CreateThird(thirdParty) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateThird',
            data: JSON.stringify({ thirdParty: thirdParty }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetThirdByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetThirdByIndividualId",
            data: JSON.stringify({ IndividualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"            
        }).done(function (data) {
            $.UifProgress('close');
            if (data.success === true) {
                Third.GetThird(data.result);
            } else {

                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (data) {
            $.UifProgress('close');
        });
    }

    static ValidateThirdByIndividualId(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetThirdByIndividualId",
            data: JSON.stringify({ IndividualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"            
        });
    }

}

class BusinessNameRequest {
    static SaveBusinessName(listCompanyName) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateBusinessName',
            data: JSON.stringify({ "listCompanyName": listCompanyName }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCompanyBusinessByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetCompanyBusinnesByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class BasicInfoRequest {

    static GetPersonOrCompanyByDescription(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetPersonOrCompanyByDescription',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHolders(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdatePersonBasicInfo(person) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/UpdateApplicationPersonBasicInfo',
            data: JSON.stringify({ person: person }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateCompanyBasicInfo(company) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/UpdateApplicationCompanyBasicData',
            data: JSON.stringify({ company: company }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
