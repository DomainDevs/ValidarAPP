//Sistran FWK
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Sistran.Co.Application.Data;
using System.Data;
using System;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    /// <summary>
    /// 
    /// </summary>
    internal class AssociationLineDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAssociationLine
        /// </summary>
        /// <param name="lineAssociation"></param>
        /// <returns>LineAssociation</returns>
        public LineAssociation SaveAssociationLine(LineAssociation lineAssociation)
        {
            // Convertir de model a entity
            REINSEN.LineAssociation entityAssociationLine = EntityAssembler.CreateAssociationLine(lineAssociation);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityAssociationLine);
            return ModelAssembler.CreateAssociationLine(entityAssociationLine);
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAssociationLine
        /// </summary>
        /// <param name="associationLine"></param>
        public void UpdateAssociationLine(LineAssociation associationLine)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LineAssociation.CreatePrimaryKey(associationLine.LineAssociationId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.LineAssociation entityAssociationLine = (REINSEN.LineAssociation)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityAssociationLine.LineCode = associationLine.Line.LineId;
            entityAssociationLine.DateFrom = associationLine.DateFrom;
            entityAssociationLine.DateTo = associationLine.DateTo;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityAssociationLine);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAssociationLine
        /// </summary>
        /// <param name="associationLineId"></param>
        public void DeleteAssociationLine(int associationLineId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LineAssociation.CreatePrimaryKey(associationLineId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.LineAssociation entityAssociationLine = (REINSEN.LineAssociation)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityAssociationLine);

        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetAssociationLineById
        /// </summary>
        /// <param name="associationLineId"></param>
        /// <returns>LineAssociation</returns>
        public LineAssociation GetAssociationLineById(int associationLineId)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LineAssociation.CreatePrimaryKey(associationLineId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.LineAssociation entityAssociationLine = (REINSEN.LineAssociation)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            // Retornar el model
            return ModelAssembler.CreateAssociationLine(entityAssociationLine);
        }

        public List<LineAssociation> ValidateDuplicateLineAssociation(LineAssociation lineAssociation)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.LineAssociation.Properties.LineAssociationTypeCode, typeof(REINSEN.LineAssociation).Name, lineAssociation.AssociationType.LineAssociationTypeId).And();
            filter.PropertyEquals(REINSEN.LineAssociation.Properties.LineCode, typeof(REINSEN.LineAssociation).Name, lineAssociation.Line.LineId).And();
            filter.PropertyEquals(REINSEN.LineAssociation.Properties.DateFrom, typeof(REINSEN.LineAssociation).Name, lineAssociation.DateFrom).And();
            filter.PropertyEquals(REINSEN.LineAssociation.Properties.DateTo, typeof(REINSEN.LineAssociation).Name, lineAssociation.DateTo);
            
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.LineAssociation), filter.GetPredicate());
           
            return ModelAssembler.CreateAssociationLines(businessObjects);
        }

        /// <summary>
        /// GetAssociationLine
        /// Obtiene los tipos de asociaciones dados el tipo, la línea y rango de fechas
        /// </summary>
        /// <param name="year"></param>
        /// <param name="associationTypeId"></param>
        /// <param name="associationLineId"></param>
        /// <returns>List<AssociationLine/></returns>
        public List<AssociationLine> GetAssociationLine(int year, int associationTypeId, int associationLineId)
        {
            NameValue[] parameters = new NameValue[3];

            parameters[0] = new NameValue("IN_YEAR_DATE_FROM", year);
            parameters[1] = new NameValue("IN_ASSOCIATION_TYPE_ID", associationTypeId);
            parameters[2] = new NameValue("IN_ASSOCIATION_LINE_ID", associationLineId);

            DataTable associationLines;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                associationLines = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_PRM_ASSOCIATION_LINE", parameters);
            }
            List<AssociationLine> associationLineDTOs = new List<AssociationLine>();
            if (associationLines != null && associationLines.Rows.Count > 0)
            {

                foreach (DataRow associationLine in associationLines.Rows)
                {
                    if (associationLineId == 0)
                    {
                        associationLineDTOs.Add(new AssociationLine()
                        {
                            LineDescription = associationLine[0] == DBNull.Value ? "" : associationLine[0].ToString(),
                            LineId = associationLine[1] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[1]),
                            AssociationLineId = associationLine[2] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[2]),
                            DateFrom = associationLine[3] == DBNull.Value ? "" : associationLine[3].ToString(),
                            DateTo = associationLine[4] == DBNull.Value ? "" : associationLine[4].ToString(),
                            AssociationTypeId = associationLine[5] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[5]),
                        });
                    }
                    else
                    {
                        associationLineDTOs.Add(new AssociationLine()
                        {
                            LineDescription = associationLine[0] == DBNull.Value ? "" : associationLine[0].ToString(),
                            LineId = associationLine[1] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[1]),
                            AssociationLineId = associationLine[2] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[2]),
                            DateFrom = associationLine[3] == DBNull.Value ? "" : associationLine[3].ToString(),
                            DateTo = associationLine[4] == DBNull.Value ? "" : associationLine[4].ToString(),
                            AssociationTypeId = associationLine[5] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[5]),
                            AssociationColumnId = associationLine[6] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[6]),
                            Order = associationLine[7] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[7]),
                            ValueFrom = associationLine[8] == DBNull.Value ? 0 : Convert.ToInt32(associationLine[8]),
                            SubLineBusinessDescriptionFrom = associationLine[9] == DBNull.Value ? "" : associationLine[9].ToString(),
                            LineBusinessDescriptionFrom = associationLine[10].ToString()
                        });
                    }
                }
            }
            return associationLineDTOs;
        }
        #endregion Get
    }
}