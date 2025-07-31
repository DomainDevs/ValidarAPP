//using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using COMMEN = Sistran.Company.Application.Common.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Assembler
{
    public static class ModelAssembler
    {

        public static List<IdentityCardTypes> CreateIdentityCardTypes(BusinessCollection businessObjects)
        {
            List<IdentityCardTypes> cardTypes = new List<IdentityCardTypes>();
            foreach (IdentityCardType item in businessObjects)
            {
                cardTypes.Add(new IdentityCardTypes
                {
                    Id = item.IdCardTypeCode,
                    Description = item.Description
                });

            }
            return cardTypes;

        }
        public static List<RiskListModel> CreateRiskList(BusinessCollection businessObjects)
        {
            List<RiskListModel> riskLists = new List<RiskListModel>();
            foreach (RiskList item in businessObjects)
            {
                riskLists.Add(new RiskListModel
                {
                    Id = item.RiskListCode,
                    Description = item.Description,
                    RiskListType = item.RiskListType
                });

            }
            return riskLists;
        }

        public static CompanyListRiskLoad CreateProcessListRisk(COMMEN.CiaAsynchronousProcessListRiskMassiveLoad listRiskMassiveLoad)
        {
            return new CompanyListRiskLoad
            {
                Id = listRiskMassiveLoad.Id,
                ProcessId = listRiskMassiveLoad.ProcessId,
                TotalRows = listRiskMassiveLoad.TotalRows,
                File = new File
                {
                    Name = listRiskMassiveLoad.FileName
                },
                ListRiskId = listRiskMassiveLoad.ListCodeId,
                RiskListType = listRiskMassiveLoad.RiskListTypeCd
            };
        }

        public static List<CompanyListRiskRow> CreateMassiveListRiskRow(BusinessCollection businessCollection)
        {
            List<CompanyListRiskRow> listRiskRows = new List<CompanyListRiskRow>();

            foreach (COMMEN.CiaAsynchronousProcessListriskRow entityCollectiveEmissionRow in businessCollection)
            {
                listRiskRows.Add(CreateListRiskRow(entityCollectiveEmissionRow));
            }

            return listRiskRows;
        }
        public static CompanyListRiskRow CreateListRiskRow(COMMEN.CiaAsynchronousProcessListriskRow ciaAsynchronousProcessListrisk)
        {
            return new CompanyListRiskRow()
            {
                Id = ciaAsynchronousProcessListrisk.Id,
                ProcessMassiveLoadId = ciaAsynchronousProcessListrisk.Id,
                ProcessId = ciaAsynchronousProcessListrisk.ProcessId,
                RowNumber = ciaAsynchronousProcessListrisk.RowNumber,
                HasError = ciaAsynchronousProcessListrisk.HasError.Value,
                Error_Description = ciaAsynchronousProcessListrisk.ErrorDescription,
                SerializedRow = ciaAsynchronousProcessListrisk.SerializedRow,
                IdCardNo = ciaAsynchronousProcessListrisk.IdCardNo
            };
        }

    }
}
