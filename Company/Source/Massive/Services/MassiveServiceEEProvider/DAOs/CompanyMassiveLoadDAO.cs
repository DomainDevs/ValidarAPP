using Sistran.Co.Application.Data;
using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using Sistran.Company.Application.MassiveServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.EEProvider.Entities.Views;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MSVEN = Sistran.Core.Application.Massive.Entities;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class CompanyMassiveLoadDAO
    {
        /// <summary>
        /// Obtener Cargues Por Descripción, Id del cargue
        /// </summary>
        /// <param name="description">Descripción</param>
        /// <returns>Cargues</returns>
        public List<MassiveLoad> GetMassiveLoadsByDescription(string description)
        {
            List<MassiveLoad> massiveLoads = new List<MassiveLoad>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int32 massiveLoadId = 0;
            Int32.TryParse(description, out massiveLoadId);

            if (massiveLoadId > 0)
            {
                filter.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name).Equal().Constant(massiveLoadId);
            }
            else
            {
                filter.Property(MSVEN.MassiveLoad.Properties.Description, typeof(MSVEN.MassiveLoad).Name).Like().Constant("%" + description + "%");
            }

            MassiveLoadView massiveLoadView = new MassiveLoadView();
            ViewBuilder builder = new ViewBuilder("MassiveLoadView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, massiveLoadView);

            if (massiveLoadView.MassiveLoads.Count > 0)
            {
                massiveLoads = ModelAssembler.CreateMassiveLoads(massiveLoadView.MassiveLoads);

                if (massiveLoads.Count == 1)
                {
                    massiveLoads[0].LoadType.ProcessType = (MassiveProcessType)massiveLoadView.LoadTypes.Cast<MSVEN.LoadType>().First().ProcessTypeCode;

                    NameValue[] parameters = new NameValue[1];
                    parameters[0] = new NameValue("MASSIVE_LOAD_ID", massiveLoadId);

                    DataTable resultTable;

                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        resultTable = dynamicDataAccess.ExecuteSPDataTable("MSV.MASSIVE_LOAD_SUMMARY_COMPANY", parameters);
                    }
                    var statusLoadId = 0;
                    if (resultTable != null && resultTable.Rows.Count > 0)
                    {
                        statusLoadId = (int)resultTable.Rows[0][5];
                        //return resultTable;
                        for (int i = 1; i < resultTable.Rows.Count; i++)
                        {
                            if (i >= 1)
                            {
                                massiveLoads.Add(new MassiveLoad());
                            }

                            massiveLoads[i].TotalRows = (int)resultTable.Rows[i][0];
                            massiveLoads[i].Pendings = (int)resultTable.Rows[i][2];
                            massiveLoads[i].WithEvents = (int)resultTable.Rows[i][3];
                            massiveLoads[i].WithErrors = (int)resultTable.Rows[i][4];
                            switch ((int)resultTable.Rows[i][5])
                            {
                                case (int)MassiveLoadStatus.Validated:
                                    massiveLoads[i].Processeds = (int)resultTable.Rows[i][1];
                                    massiveLoads[i].Status = MassiveLoadStatus.Validated;
                                    break;
                                case (int)MassiveLoadStatus.Queried:
                                    massiveLoads[i].Processeds = (int)resultTable.Rows[i][1];
                                    massiveLoads[i].Status = MassiveLoadStatus.Queried;
                                    break;
                                case (int)MassiveLoadStatus.Tariffed:
                                    massiveLoads[i].Tariffed = (int)resultTable.Rows[i][1];
                                    massiveLoads[i].Status = MassiveLoadStatus.Tariffed;
                                    break;
                                case (int)MassiveLoadStatus.Issued:
                                    massiveLoads[i].Issued = (int)resultTable.Rows[i][1];
                                    massiveLoads[i].Status = MassiveLoadStatus.Issued;
                                    break;
                            }

                        }

                        return massiveLoads;
                    }
                }
            }

            return null;

            //                                                                                                                                                                                                                                                                               return resultTable;
        }


        public bool UpdateMassiveLoadStatusIfComplete(int massiveLoadId, bool changeStatus)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@MASSIVE_LOAD_ID", massiveLoadId);
            parameters[1] = new NameValue("@CHANGE_STATUS", changeStatus);

            DataTable dataTable;
            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                dataTable = dataAccess.ExecuteSPDataTable("MSV.UPDATE_MASSIVE_LOAD_STATUS_WHEN_COMPLETE", parameters);
            }
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int result;
                if (int.TryParse(dataTable.Rows[0][0].ToString(), out result))
                {
                    return Convert.ToBoolean(result);
                }
            }
            return false;
        }

        public int GetpendingOperationIdByMassiveLoadIdRowId(int massiveLoadId, int rowId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@MASSIVE_LOAD_ID", massiveLoadId);
            parameters[1] = new NameValue("@ROW_ID", rowId);

            DataTable result;
            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                result = dataAccess.ExecuteSPDataTable("MSV.GET_OPERATION_ID", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                Int32 operationId;

                if (int.TryParse(result.Rows[0][0].ToString(), out operationId))
                {
                    return operationId;
                }
                else
                {
                    throw new Exception(result.ToString());
                }
            }
            return 0;
        }

        public List<CompanyClause> GetClauses(Template templateClauses, EmissionLevel emissionLevel)
        {
            return GetClausesByCoverageId(templateClauses, emissionLevel, null);
        }

        public List<CompanyClause> GetClausesByCoverageId(Template templateClauses, EmissionLevel emissionLevel, int? coverageId)
        {
            List<CompanyClause> clauses = new List<CompanyClause>();
            if (templateClauses != null)
            {

                List<Clause> clausesCoverages = new List<Clause>();
                if (coverageId != null)
                {
                    clausesCoverages = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Coverage, coverageId.Value);
                }

                if (templateClauses != null)
                {
                    List<ConditionLevel> conditionLevels = DelegateService.underwritingService.GetConditionLevels();

                    foreach (Row row in templateClauses.Rows)
                    {
                        int levelCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.LevelCode));
                        int clauseCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.ClauseCode));

                        if (levelCode == 0)
                        {
                            row.HasError = true;
                            row.ErrorDescription += Errors.ClauseErrorLevel + "|";
                        }

                        if (clauseCode == 0)
                        {
                            row.HasError = true;
                            row.ErrorDescription += Errors.ClauseError + "|";
                        }

                        ConditionLevel conditionLevel = conditionLevels.FirstOrDefault(c => c.Id == levelCode);

                        if (conditionLevel.EmissionLevel == emissionLevel)
                        {
                            Clause clause;
                            if (emissionLevel == EmissionLevel.Coverage)
                            {
                                clause = clausesCoverages.Find(p => p.Id == clauseCode);
                            }
                            else
                            {
                                clause = DelegateService.underwritingService.GetClauseByClauseId(clauseCode);
                                if (clause == null)
                                    throw new ValidationException(string.Format(Errors.ErrorClauseNotExists, clauseCode));
                            }
                            if (clause != null)
                            {
                                clause.ConditionLevel = new ConditionLevel()
                                {
                                    EmissionLevel = emissionLevel
                                };

                                //Pendiente actualizar assembly en underwriting que devuelva CompanyClauses
                                CompanyClause cClause = new CompanyClause()
                                {
                                    ConditionLevel = clause.ConditionLevel,
                                    ExtendedProperties = clause.ExtendedProperties,
                                    Id = clause.Id,
                                    IsMandatory = clause.IsMandatory,
                                    Name = clause.Name,
                                    Text = clause.Text,
                                    Title = clause.Title
                                };
                                if (!clauses.Any(x => x.Id == cClause.Id))
                                {
                                    clauses.Add(cClause);
                                }

                            }

                        }
                    }
                }
            }
            return clauses;
        }

        public bool GetMassiveLoadErrorStatus(int massiveLoadId)
        {
            DataTable dataTable;
            int result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@MASSIVE_LOAD_ID", massiveLoadId);

            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                dataTable = dataAccess.ExecuteSPDataTable("MSV.GET_MASSIVE_LOAD_ERROR_STATUS", parameters);
            }

            if (int.TryParse(dataTable.Rows[0][0].ToString(), out result))
            {
                return Convert.ToBoolean(result);
            }
            return false;
        }
    }
}
