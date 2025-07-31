using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    public class InsuredObjectBusiness
    {
        UnderwritingServiceEEProviderCore underwritingServiceEEProviderCore = new UnderwritingServiceEEProviderCore();
        
        public List<CompanyInsuredObject> GetCompanyInsuredObjects()
        {
            List<InsuredObject> insuredObjects = underwritingServiceEEProviderCore.GetInsuredObjects();
            return ModelAssembler.CreateCompanyInsuredObjects(insuredObjects);
        }

        public string GenerateFileToCompanyInsuredObject(List<CompanyInsuredObject> companyInsuredObjects, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.ParametrizationInsuredObject
            };

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();
                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    List<Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = companyInsuredObject.Id.ToString();
                    fields[1].Value = companyInsuredObject.Description;
                    fields[2].Value = companyInsuredObject.SmallDescription;

                    if (companyInsuredObject.IsDeclarative)
                    {
                        fields[3].Value = "Si";
                    }
                    else
                    {
                        fields[3].Value = "No";
                    }

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            List<InsuredObject> insuredObjects = underwritingServiceEEProviderCore.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
            return ModelAssembler.CreateCompanyInsuredObjects(insuredObjects);
        }
        public List<CompanyInsuredObject> GetCompanyInsuredObjectByPrefixIdList(int prefixId)
        {
            List<InsuredObject> insuredObjects = underwritingServiceEEProviderCore.GetInsuredObjectByPrefixIdList(prefixId);
            return ModelAssembler.CreateCompanyInsuredObjects(insuredObjects);
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByRiskId(int riskId)
        {
            List<InsuredObject> insuredObjects = underwritingServiceEEProviderCore.GetInsuredObjectsByRiskId(riskId);
            return ModelAssembler.CreateCompanyInsuredObjects(insuredObjects);
        }

        public CompanyInsuredObject GetCompanyInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            InsuredObject insuredObject = underwritingServiceEEProviderCore.GetInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId);
            return ModelAssembler.CreateCompanyInsuredObject(insuredObject);
        }
    }
}