using System;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.Queries;
using System.Data;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class ContractLineDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;

        #endregion

        #region Save

        /// <summary>
        /// SaveContractLine
        /// </summary>
        /// <param name="contractLine"></param>
        public void SaveContractLine(Line contractLine)
        {
            // Convertir de model a entity
            REINSURANCEEN.ContractLine entityContractLine = EntityAssembler.CreateContractLine(contractLine);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityContractLine);
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateContractLine
        /// </summary>
        /// <param name="line"></param>
        public void UpdateContractLine(Line line)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.ContractLine.CreatePrimaryKey(line.ContractLines[0].ContractLineId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.ContractLine entityContractLine = (REINSURANCEEN.ContractLine)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityContractLine.ContractId = line.ContractLines[0].Contract.ContractId;
            entityContractLine.LineId = line.LineId;
            entityContractLine.Priority = line.ContractLines[0].Priority;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityContractLine);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteContractLine
        /// </summary>
        /// <param name="contractLineId"></param>
        public void DeleteContractLine(int contractLineId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.ContractLine.CreatePrimaryKey(contractLineId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.ContractLine entityContractLine = (REINSURANCEEN.ContractLine)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityContractLine);
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetContractLineById
        /// </summary>
        /// <param name="contractLineId"></param>
        /// <returns>ContractLine</returns>
        public ContractLine GetContractLineById(int contractLineId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.ContractLine.CreatePrimaryKey(contractLineId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.ContractLine entityContractLine = (REINSURANCEEN.ContractLine)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Retornar el model
            return ModelAssembler.CreateContractLine(entityContractLine);
        }

        /// <summary>
        /// GetContractLines
        /// </summary>
        /// <returns>List<ContractLine></returns>
        public List<ContractLine> GetContractLines()
        {
            // Asignar BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                (typeof(REINSURANCEEN.ContractLine)));

            // Retornar el model como Lista
            return ModelAssembler.CreateContractLines(businessCollection);
        }

        /// <summary>
        /// GetContractLineByLineId
        /// Obtiene la relación entre ContractLine, Contract y Line dado el lineId
        /// </summary>
        /// <param name = "lineId" ></ param >
        /// < returns > Line </ returns >
        public Line GetContractLineByLineId(int lineId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.ContractLine.Properties.LineId, lineId);

            UIView lines = _dataFacadeManager.GetDataFacade().GetView("GetContractLine", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            Line line = new Line();

            foreach (DataRow dataRow in lines)
            {
                line.LineId = dataRow["LineId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["LineId"]);
                line.Description = dataRow["LineDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["LineDescription"]);
            }

            List<ContractLine> contractLines = new List<ContractLine>();
            for (int i = 0; i < lines.Count; i++)
            {
                ContractLine contractLine = new ContractLine();
                contractLine.ContractLineId = Convert.ToInt32(lines.Rows[i]["ContractLineId"]);
                contractLine.Priority = Convert.ToInt32(lines.Rows[i]["Priority"]);
                contractLine.Contract = new Contract();
                contractLine.Contract.ContractId = Convert.ToInt32(lines.Rows[i]["ContractId"]);
                contractLine.Contract.Description = lines.Rows[i]["ContractDescription"].ToString();
                contractLines.Add(contractLine);
            }

            line.ContractLines = new List<ContractLine>();
            line.ContractLines = contractLines;

            return line;
        }

        /// <summary>
        /// LineIsUsed
        /// </summary>
        /// <returns>List<ContractLine></returns>
        public bool LineIsUsed(int lineId)
        {
            //Crear criterio de busqueda
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.IssueLayerLine.Properties.LineId, lineId);
            // Obtener businessCollection
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.IssueLayerLine), criteriaBuilder.GetPredicate()));
            // Retornar true/false
            return businessCollection.Count > 0;
        }

        #endregion Get
    }
}
