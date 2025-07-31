using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.UnderwritingUtilities
{
    public class GetDatatables
    {
        public CommonDataTables GetcommonDataTables(CompanyRisk risk)
        {
            CommonDataTables datatables = new CommonDataTables();

            datatables.dtTempRisk = ModelAssembler.GetDataTableTempRISK(risk);
            datatables.dtCOTempRisk = ModelAssembler.GetDataTableCOTempRisk(risk);
            datatables.dtBeneficary = ModelAssembler.GetDataTableRiskBeneficiary(risk);
            datatables.dtRiskPayer = ModelAssembler.GetDataTableRiskPayer(risk);
            datatables.dtClause = ModelAssembler.GetDataTableRiskClause(risk);
            datatables.dtRiskClause = ModelAssembler.GetDataTableRiskClause(risk);
            datatables.dtRiskCoverage = ModelAssembler.GetDataTableRiskCoverage(risk);
            datatables.dtDeduct = ModelAssembler.GetDataTableDeduct(risk);
            datatables.dtCoverClause = ModelAssembler.GetDataTableCoverClause(risk);
            datatables.dtDynamic = ModelAssembler.GetDataTableDynamic(risk);
            datatables.dtDynamicCoverage = ModelAssembler.GetDataTableDynamicCoverage(risk);

            return datatables;

        }

        public CommonDataTables GetcommonDataTablesMa(CompanyRisk risk)
        {
            CommonDataTables datatables = new CommonDataTables();

            datatables.dtTempRisk = ModelAssembler.GetDataTableTempRISK(risk);
            datatables.dtCOTempRisk = ModelAssembler.GetDataTableCOTempRisk(risk);
            datatables.dtBeneficary = ModelAssembler.GetDataTableRiskBeneficiary(risk);
            datatables.dtRiskPayer = ModelAssembler.GetDataTableRiskPayer(risk);
            datatables.dtClause = ModelAssembler.GetDataTableRiskClause(risk);
            datatables.dtCoverClause = ModelAssembler.GetDataTableRiskCoverage(risk);
            datatables.dtDeduct = ModelAssembler.GetDataTableDeduct(risk);
            datatables.dtRiskClause = ModelAssembler.GetDataTableCoverClause(risk);
            datatables.dtDynamic = ModelAssembler.GetDataTableDynamic(risk);
            datatables.dtDynamicCoverage = ModelAssembler.GetDataTableDynamicCoverage(risk);

            return datatables;

        }

    }
}
