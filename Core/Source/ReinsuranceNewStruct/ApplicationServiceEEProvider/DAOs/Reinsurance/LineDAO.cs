using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Co.Application.Data;
using System.Data;
using System;
using Sistran.Core.Framework.Queries;
using System.Linq;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    /// <summary>
    /// </summary>
    internal class LineDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;

        #endregion

        #region Save

        /// <summary>
        /// SaveLine
        /// </summary>
        /// <param name="line"></param>
        public void SaveLine(Line line)
        {
            // Convertir de model a entity
            REINSEN.Line entityLine = EntityAssembler.CreateLine(line);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityLine);
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateLine
        /// </summary>
        /// <param name="line"></param>
        public void UpdateLine(Line line)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.Line.CreatePrimaryKey(line.LineId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.Line entityLine = (REINSEN.Line)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityLine.CumulusTypeId = line.CumulusType.CumulusTypeId;
            entityLine.Description = line.Description;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityLine);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteLine
        /// </summary>
        /// <param name="lineId"></param>
        public void DeleteLine(int lineId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.Line.CreatePrimaryKey(lineId);

            //realizar las operaciones con los entities utilizando DAF
            REINSEN.Line entityLine = (REINSEN.Line)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityLine);
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetLineByLineId
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>Line</returns>
        public Line GetLineByLineId(int lineId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.Line.CreatePrimaryKey(lineId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.Line entityLine = (REINSEN.Line)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Return del model
            return ModelAssembler.CreateLine(entityLine);
        }

        /// <summary>
        /// GetLines
        /// </summary>
        /// <returns>List<Line></returns>
        public List<Line> GetLines()
        {
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection
                (_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.Line)));

            // Return como Lista
            return ModelAssembler.CreateLines(businessCollection);
        }


        /// <summary>
        /// GetTempLineCumulus
        /// Método que obtiene datos de la vista REINS.GET_ISS_TMP_LINE_CUMULUS
        /// </summary>
        /// <param name="tempIssueLayerId"></param>
        /// <returns>ReinsuranceLayerIssuance</returns>
        public ReinsuranceLayerIssuance GetTempLineCumulus(int tempIssueLayerId)
        {
            ReinsuranceLayerIssuance reinusranceLayerIssuance = new ReinsuranceLayerIssuance();
            List<ReinsuranceLine> reinsuranceLines;
            List<ReinsuranceLine> reinsuranceLinesResult = new List<ReinsuranceLine>();
            ReinsuranceLine reinsuranceLineResult = new ReinsuranceLine();
            List<LineCumulusKey> linesCumulusKey = new List<LineCumulusKey>();
            List<ReinsuranceCumulusRiskCoverage> reinsuranceCumulusRiskCoveragesResult = new List<ReinsuranceCumulusRiskCoverage>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(REINSEN.GetIssTmpLineCumulus.Properties.TempReinsLayerIssuanceId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(tempIssueLayerId);

            UIView lineCumulus = _dataFacadeManager.GetDataFacade().GetView("GetIssTmpLineCumulus", criteriaBuilder.GetPredicate(),
                                                                  null, 0, 50, null, true, out RowsGrid);
            int endorsementId = 0;
            reinsuranceLines = new List<ReinsuranceLine>();

            foreach (DataRow reinsuranceLineEntity in lineCumulus.Rows)
            {
                endorsementId = Convert.ToInt32(reinsuranceLineEntity["EndorsementCode"]);

                Line line = new Line();
                line.LineId = Convert.ToInt32(reinsuranceLineEntity["LineId"]);
                line.Description = reinsuranceLineEntity["LineDescription"].ToString();
                line.CumulusType = new CumulusType();
                line.CumulusType.Description = reinsuranceLineEntity["CumulusDescription"].ToString();

                CommonService.Models.Amount retainedSum = new CommonService.Models.Amount();
                retainedSum.Value = Convert.ToDecimal(reinsuranceLineEntity["RetainedSum"]);

                CommonService.Models.Amount givenSum = new CommonService.Models.Amount();
                givenSum.Value = Convert.ToDecimal(reinsuranceLineEntity["GivenSum"]);

                ReinsuranceAllocation reinsuranceAllocation = new ReinsuranceAllocation();
                reinsuranceAllocation.Premium = new CommonService.Models.Amount();
                reinsuranceAllocation.Premium.Value = givenSum.Value;
                reinsuranceAllocation.Amount = new CommonService.Models.Amount(); 
                reinsuranceAllocation.Amount.Value = retainedSum.Value;

                ReinsuranceLine reinsuranceLine = new ReinsuranceLine();

                reinsuranceLine.ReinsuranceLineId = Convert.ToInt32(reinsuranceLineEntity["TempReinsuranceLineId"]);
                reinsuranceLine.CumulusKey = reinsuranceLineEntity["CumulusKey"].ToString();
                reinsuranceLine.Line = new Line();
                reinsuranceLine.Line = line;

                reinsuranceLine.ReinsuranceAllocations = new List<ReinsuranceAllocation>();
                reinsuranceLine.ReinsuranceAllocations.Add(reinsuranceAllocation);

                reinsuranceLines.Add(reinsuranceLine);

            }


            //****************************************************************************************
            //CARGA LINEAS RISK COVERAGE LineCumulusKey (Línea,Claveúmulo,Riesgo,Cobertura) 

            linesCumulusKey = GetCumulusKeyRiskCoverageByEndorsement(endorsementId);

            foreach (ReinsuranceLine itemLine in reinsuranceLines)
            {
                string keyMasterLevelUp = itemLine.Line.LineId.ToString() + itemLine.CumulusKey.ToString(); //la línea + clave cúmulo

                foreach (LineCumulusKey lineCumulusKey in linesCumulusKey)
                {
                    string keyMasterLevelDown = lineCumulusKey.Line.LineId.ToString() + lineCumulusKey.CumulusKey;

                    if (keyMasterLevelUp == keyMasterLevelDown)
                    {
                        foreach (LineCumulusKeyRiskCoverage lineCumulusKeyRiskCoverage in lineCumulusKey.LineCumulusKeyRiskCoverages)
                        {
                            if (lineCumulusKeyRiskCoverage.Id == tempIssueLayerId)
                            {
                                ReinsuranceCumulusRiskCoverage reinsuranceCumulusRiskCoverage = new ReinsuranceCumulusRiskCoverage();

                                reinsuranceCumulusRiskCoverage.RiskNumber = lineCumulusKeyRiskCoverage.RiskNumber;
                                reinsuranceCumulusRiskCoverage.CoverageNumber = lineCumulusKeyRiskCoverage.CoverageNumber;
                                reinsuranceCumulusRiskCoverage.ReinsuranceCumulusRiskCoverageId = lineCumulusKeyRiskCoverage.Id;

                                reinsuranceCumulusRiskCoveragesResult.Add(reinsuranceCumulusRiskCoverage);
                            }
                        }
                    }
                }

                reinsuranceLineResult = new ReinsuranceLine();

                reinsuranceLineResult = itemLine;
                reinsuranceLineResult.ReinsuranceCumulusRiskCoverages = new List<ReinsuranceCumulusRiskCoverage>();
                reinsuranceLineResult.ReinsuranceCumulusRiskCoverages = reinsuranceCumulusRiskCoveragesResult;
                reinsuranceCumulusRiskCoveragesResult = new List<ReinsuranceCumulusRiskCoverage>();
            }

            reinusranceLayerIssuance.Lines = reinsuranceLines;

            return reinusranceLayerIssuance;
        }

        /// <summary>
        /// GetCumulusKeyRiskCoverageByEndorsement
        /// Obtener la línea, la clave de cúmulo y el valor
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<LineCumulusKey></returns>
        public List<LineCumulusKey> GetCumulusKeyRiskCoverageByEndorsement(int endorsementId)
        {
            List<LineCumulusKey> lineCumulusKeys = new List<LineCumulusKey>();

            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("PROCESS_ID", 0);
            parameters[1] = new NameValue("ENDORSEMENT_ID", endorsementId);
            parameters[2] = new NameValue("LINE_CHANGE", 0);
            parameters[3] = new NameValue("HEADER", 1);

            DataTable resultHeader;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                resultHeader = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC2_1_LINE_ASSIGN", parameters);
            }
            
            foreach (DataRow row in resultHeader.Rows)
            {
                LineCumulusKey lineCumulusKey = new LineCumulusKey();
                lineCumulusKey.Line = new Line();
                lineCumulusKey.Line.LineId = Convert.ToInt32(row[0]);
                lineCumulusKey.CumulusKey = row[1].ToString();
                lineCumulusKey.Amount = new CommonService.Models.Amount();
                lineCumulusKey.Amount.Value = row[2] == DBNull.Value ? 0 : Convert.ToDecimal(row[2]);
                lineCumulusKey.Premium = new CommonService.Models.Amount();
                lineCumulusKey.Premium.Value = row[3] == DBNull.Value ? 0 : Convert.ToDecimal(row[3]);
                lineCumulusKeys.Add(lineCumulusKey);
            }

            parameters = new NameValue[4];

            parameters[0] = new NameValue("PROCESS_ID", 0);
            parameters[1] = new NameValue("ENDORSEMENT_ID", endorsementId);
            parameters[2] = new NameValue("LINE_CHANGE", 0);
            parameters[3] = new NameValue("HEADER", 0); //Obtiene solo la cabecera o todo


            DataTable resultAll;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                resultAll = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC2_1_LINE_ASSIGN", parameters);
            }
            
            foreach (LineCumulusKey lineCumulusKey in lineCumulusKeys)
            {
                string keyMaster = lineCumulusKey.Line.LineId.ToString() + lineCumulusKey.CumulusKey;
                lineCumulusKey.LineCumulusKeyRiskCoverages = new List<LineCumulusKeyRiskCoverage>();
                foreach (DataRow row in resultAll.Rows)
                {
                    string keyMasterDetail = row[0].ToString() + row[1].ToString();
                    if (keyMaster == keyMasterDetail)
                    {
                        LineCumulusKeyRiskCoverage lineCumulusKeyRiskCoverage = new LineCumulusKeyRiskCoverage();
                        lineCumulusKeyRiskCoverage.RiskNumber = Convert.ToInt32(row[2]);
                        lineCumulusKeyRiskCoverage.CoverageNumber = Convert.ToInt32(row[3]);
                        lineCumulusKeyRiskCoverage.Id = Convert.ToInt32(row[4]);
                        lineCumulusKey.LineCumulusKeyRiskCoverages.Add(lineCumulusKeyRiskCoverage);
                    }
                }
            }
            return lineCumulusKeys;
        }


        /// <summary>
        /// GetReinsuranceLines
        /// Método que obtiene líneas disponibles para llenar el combo
        /// y actualizar la línea de reaseguros
        /// </summary>
        /// <returns>List<Line/></returns>
        public List<Line> GetReinsuranceLines()
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            List<Line> lines = new List<Line>();

            criteriaBuilder.Property(REINSEN.GetLines.Properties.LineId);
            criteriaBuilder.Greater();
            criteriaBuilder.Constant(0);

            BusinessCollection reinsuranceLines = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(REINSEN.GetLines), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.GetLines reinsuranceLineEntity in reinsuranceLines.OfType<REINSEN.GetLines>())
            {
                CumulusType cumulusType = new CumulusType();

                cumulusType.Description = reinsuranceLineEntity.CumulusTypeDescription;

                lines.Add(new Line
                {
                    LineId = Convert.ToInt32(reinsuranceLineEntity.LineId),
                    Description = reinsuranceLineEntity.LineDescription,
                    CumulusType = cumulusType
                });
            }

            return lines;
        }
        #endregion Get
    }
}