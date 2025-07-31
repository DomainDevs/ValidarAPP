//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.CommonService.Models;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System.Collections.Generic;
using System.Data;
using System;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using Sistran.Core.Framework.Views;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
   
    internal class CumulusTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;

        #endregion

        /// <summary>
        /// Establece el cúmulo por endorsement id
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        public void SetCumulusByEndorsement(int endorsementId)
        {
            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("PROCESS_ID", 0);
            parameters[1] = new NameValue("ENDORSEMENT_ID", endorsementId);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC2_2_CUMULUS", parameters);
            }
        }

        /// <summary>
        /// GetCumulusTypes
        /// </summary>
        /// <returns>List<CumulusType></returns>
        public List<CumulusType> GetCumulusTypes()
        {
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.CumulusType)));

            // Return como Lista
            return ModelAssembler.CreateCumulusTypes(businessCollection);
        }

        /// <summary>
        /// GetLineCumulusType
        /// </summary>
        /// <returns>List<LineCumulusType/></returns>
        public List<LineCumulusType> GetLineCumulusType()
        {
            UIView lineCumulusTypes = _dataFacadeManager.GetDataFacade().GetView("GetLineCumulusType",
                null, null, 0, 50, null, false, out RowsGrid);

            List<LineCumulusType> lineCumulusTypeDTOs = new List<LineCumulusType>();

            foreach (DataRow dataRow in lineCumulusTypes.Rows)
            {
                lineCumulusTypeDTOs.Add(new LineCumulusType
                {
                    LineId = dataRow["LineId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["LineId"]),
                    Description = dataRow["Description"] == DBNull.Value ? "" : Convert.ToString(dataRow["Description"]),
                    CumulusTypeId = dataRow["CumulusTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CumulusTypeId"]),
                    CumulusTypeDescription = dataRow["CumulusTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CumulusTypeDescription"])
                });
            }

            return lineCumulusTypeDTOs;
        }

        ///// <summary>
        ///// GetParametrizationLineCumulusKey
        ///// Devuelve el objeto Policy con los Riesgos/Coberturas actualizada la Linea y llave de Cùmulo
        ///// </summary>
        ///// <param name = "policyReins" ></ param >
        ///// < returns > Policy </ returns >
        //public TempCommonServices.Models.Policy GetParametrizationLineCumulusKey(TempCommonServices.Models.Policy policyReins)
        //{
        //    int service = 1;//Parámetro necesario para el SP, significa que devuelve un select 

        //    NameValue[] parameters = new NameValue[7];

        //    parameters[0] = new NameValue("@ENDORSEMENT_ID", policyReins.Endorsement.Id);
        //    parameters[1] = new NameValue("@DATE_FROM", "");//cambio a SyBase
        //    parameters[2] = new NameValue("@DATE_TO", "");//cambio a SyBase
        //    parameters[3] = new NameValue("@PROCESS_TYPE", (int)ProcessTypes.Manual);
        //    parameters[4] = new NameValue("@USER_ID", policyReins.UserId);
        //    parameters[5] = new NameValue("@PREFIX_XML", "");//cambio a SyBase                
        //    parameters[6] = new NameValue("@SERVICE", service.ToString());//cambio a SyBase
        //    DataTable lineCumulusKey;
        //    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
        //    {
        //        lineCumulusKey = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC1_WORKTABLES_LOAD", parameters);
        //    }

        //    int riskIndex = 0;
        //    int spIndex = 0;

        //    //Ordena los riesgos
        //    List<TempCommonServices.Models.Risk> riskOrder = policyReins.Endorsement.Risks.OrderBy(x => x.Id).ToList();
        //    policyReins.Endorsement.Risks = new List<TempCommonServices.Models.Risk>();

        //    foreach (TempCommonServices.Models.Risk riskItem in riskOrder)
        //    {
        //        policyReins.Endorsement.Risks.Add(riskItem);
        //    }

        //    foreach (TempCommonServices.Models.Risk risk in policyReins.Endorsement.Risks)
        //    {
        //        int coverageIndex = 0;

        //        //Ordena las coberturas
        //        List<TempCommonServices.Models.Coverage> coveragesOrder = risk.Coverages.OrderBy(x => x.Id).ToList();
        //        policyReins.Endorsement.Risks[riskIndex].Coverages = new List<TempCommonServices.Models.Coverage>();

        //        foreach (TempCommonServices.Models.Coverage coverageItem in coveragesOrder)
        //        {
        //            policyReins.Endorsement.Risks[riskIndex].Coverages.Add(coverageItem);
        //        }

        //        foreach (TempCommonServices.Models.Coverage coverage in policyReins.Endorsement.Risks[riskIndex].Coverages)
        //        {
        //            DataRowCollection anArray = lineCumulusKey.Rows;
        //            DataRow item = anArray[spIndex];

        //            policyReins.Endorsement.Risks[riskIndex].Coverages[coverageIndex].LineId = item[3] == DBNull.Value ? 0 : Convert.ToInt32(item[3]); //LineId
        //            policyReins.Endorsement.Risks[riskIndex].Coverages[coverageIndex].CumulusKey = item[4] == DBNull.Value ? "" : item[4].ToString(); //CumulusKey
        //            policyReins.Endorsement.Risks[riskIndex].Coverages[coverageIndex].ErrorId = item[5] == DBNull.Value ? 0 : Convert.ToInt32(item[5]); //Error a nivel de Cobertura

        //            spIndex++;
        //            coverageIndex++;
        //        }

        //        riskIndex++;
        //    }

        //    return policyReins;
        //}

        /// <summary>
        /// GetCumulusByPayment
        /// </summary>
        /// <param name="requestPaymentId"></param>
        /// <param name="typeProcess"></param>
        /// <returns></returns>
        public List<Cumulus> GetCumulusByPayment(int requestPaymentId, int typeProcess)
        {
            List<Cumulus> listCumulus = new List<Cumulus>();
            Cumulus cumulus = new Cumulus();
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("ID", requestPaymentId);
            parameters[1] = new NameValue("CLAIM", typeProcess);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.CLM_PYM_CUMULUS", parameters);
            }

            foreach (DataRow item in result.Rows)
            {
                cumulus.Id = Convert.ToInt32(item[0]);
                cumulus.MovementId = Convert.ToInt32(item[1]);
                cumulus.IssueLayers = new List<IssueLayer>();

                IssueLayer issueLayer = new IssueLayer();
                issueLayer.Id = Convert.ToInt32(item[7]);

                issueLayer.IssueLayerLines = new List<IssueLayerLine>();
                IssueLayerLine issueLayerLine = new IssueLayerLine();

                issueLayerLine.Line = new Line
                {
                    LineId = Convert.ToInt32(item[8])
                };

                issueLayerLine.IssueAllocations = new List<IssueAllocation>();
                IssueAllocation issueAllocations = new IssueAllocation();

                issueAllocations.Id = Convert.ToInt32(item[2]);
                issueAllocations.Facultative = Convert.ToBoolean(item[3]);
                issueAllocations.Currency = new Currency();
                issueAllocations.Currency.Id = Convert.ToInt32(item[5]);
                issueAllocations.Amount = new Amount();
                issueAllocations.Amount.Value = Convert.ToDecimal(item[6]);

                issueAllocations.ContractCompany = new Contract();
                issueAllocations.ContractCompany.ContractLevels = new List<Level>();
                Level level = new Level();
                level.ContractLevelId = Convert.ToInt32(item[4]);

                issueAllocations.ContractCompany.ContractLevels.Add(level);
                issueLayerLine.IssueAllocations.Add(issueAllocations);
                issueLayer.IssueLayerLines.Add(issueLayerLine);
                cumulus.IssueLayers.Add(issueLayer);

                listCumulus.Add(cumulus);
            }
            return listCumulus;
        }

    }
}
