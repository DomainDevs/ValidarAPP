using System;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
//Sistran Core
using REINSMO = Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.ReinsuranceServices.Assemblers;
using System.Linq;
using System.Collections.Generic;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class AssociationColumnValueDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAssociationColumnValue
        /// </summary>
        /// <param name="associationColumnId"></param>
        /// <param name="associationLineId"></param>
        /// <param name="value"></param>
        /// <returns>int</returns>
        public int SaveAssociationColumnValue(int associationLineId, int associationColumnId, int value)
        {
            // Convertir de model a entity
            REINSURANCEEN.AssociationColumnValue entityAssociationColumnValue = EntityAssembler.CreateAssociationColumnValue(associationLineId, associationColumnId, value);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityAssociationColumnValue);
            return entityAssociationColumnValue.AssociationColumnValueId;
        }

        #endregion Save

        #region Update

        

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAssociationColumnValue
        /// </summary>
        /// <param name="associationColumnValueId"></param>
        public void DeleteAssociationColumnValue(int associationColumnValueId)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.AssociationColumnValue.CreatePrimaryKey(associationColumnValueId);

            //realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.AssociationColumnValue entityAssociationColumnValue = (REINSURANCEEN.AssociationColumnValue)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityAssociationColumnValue);

        }

        /// <summary>
        /// DeleteAssociationColumnValueByAssociationLineId
        /// </summary>
        /// <param name="associationLineId"></param>
        public void DeleteAssociationColumnValueByAssociationLineId(int associationLineId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumnValue.Properties.LineAssociationCode, associationLineId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                     typeof(REINSURANCEEN.AssociationColumnValue), criteriaBuilder.GetPredicate()));

            foreach (BusinessObject businessObject in businessCollection)
            {
                REINSURANCEEN.AssociationColumnValue entityAssociationColumnValue = (REINSURANCEEN.AssociationColumnValue)businessObject;
                DeleteAssociationColumnValue(entityAssociationColumnValue.AssociationColumnValueId);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetCountAssociationColumn
        /// </summary>
        /// <param name="associationTypeId"></param>
        /// <returns>int</returns>
        public int GetCountAssociationColumn(int associationTypeId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumn.Properties.AssociationTypeId, associationTypeId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                     typeof(REINSURANCEEN.AssociationColumn), criteriaBuilder.GetPredicate()));

            return businessCollection.Count();
        }

        /// <summary>
        /// GetAssociationColumnByAssociationTypeId
        /// </summary>
        /// <param name="associationTypeId"></param>
        /// <returns>List<LineAssociationType/></returns>
        public List<LineAssociationType> GetAssociationColumnByAssociationTypeId(int associationTypeId)
        {
            switch (associationTypeId)
            {
                case 1:
                    {
                        // Retorna todas los registros de COMM.LINE_BUSINESS
                        return GetLineOfBusinesses();
                    }

                case 2:
                    {
                        List<LineAssociationType> lineAssociationTypes = new List<LineAssociationType>();
                        LineAssociationType entityLineAssociation = new LineAssociationType();

                        for (int i = 0; i < GetCountAssociationColumn(associationTypeId); i++)
                        {
                            lineAssociationTypes.Add(entityLineAssociation);
                        }

                        // Retorna el número de registros de REINS.ASSOCIATION_COLUMN dado ASSOCIATION_TYPE_ID
                        return lineAssociationTypes;
                    }
            }

            return new List<LineAssociationType>();
        }

        /// <summary>
        /// GetLineOfBusinesses
        /// </summary>
        /// <returns>List<LineAssociationType/></returns>
        public List<LineAssociationType> GetLineOfBusinesses()
        {
            return new List<LineAssociationType>();
        }

        /// <summary>
        /// GetAssociationColumnId
        /// </summary>
        /// <param name="associationTypeId"></param>
        /// <param name="tableName"></param>
        /// <param name="columnIdName"></param>
        /// <returns>int</returns>
        public int GetAssociationColumnId(int associationTypeId, string tableName, string columnIdName)
        {
            int associationColumnId = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumn.Properties.AssociationTypeId, associationTypeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumn.Properties.TableName, tableName);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumn.Properties.ColumnIdName, columnIdName);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                         typeof(REINSURANCEEN.AssociationColumn), criteriaBuilder.GetPredicate()));

            foreach (REINSURANCEEN.AssociationColumn associationColumn in businessCollection.OfType<REINSURANCEEN.AssociationColumn>())
            {
                associationColumnId = associationColumn.AssociationColumnId;
            }

            return associationColumnId;
        }

        /// <summary>
        /// GetAssociationColumnValueId
        /// </summary>
        /// <param name="lineAssociationId"></param>
        /// <param name="associationColumnId"></param>
        /// <returns>int</returns>
        public int GetAssociationColumnValueId(int lineAssociationId, int associationColumnId)
        {
            int associationColumnValueId = 0;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumnValue.Properties.LineAssociationCode, lineAssociationId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.AssociationColumnValue.Properties.AssociationColumnCode, associationColumnId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                         typeof(REINSURANCEEN.AssociationColumnValue), criteriaBuilder.GetPredicate()));

            foreach (REINSURANCEEN.AssociationColumnValue associationColumnValue in businessCollection.OfType<REINSURANCEEN.AssociationColumnValue>())
            {
                associationColumnValueId = associationColumnValue.AssociationColumnValueId;
            }
            return associationColumnValueId;
        }
        #endregion

    }
}