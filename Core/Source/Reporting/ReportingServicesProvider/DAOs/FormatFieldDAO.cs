// Sistran Core
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.ReportingServices.Provider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.ReportingServices.Provider.DAOs
{
    public class FormatFieldDAO
    {
        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del DataFacadeManager
        /// </summary>
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        private readonly FormatDetailDAO _formatDetailDAO = new FormatDetailDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region FormatField

        /// <summary>
        /// SaveFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        public FormatField SaveFormatField(FormatField formatField)
        {
            try
            {
                Entities.FormatTypeField formatTypeFieldEntity = EntityAssembler.CreateFormatTypeField(formatField);
                _dataFacadeManager.GetDataFacade().InsertObject(formatTypeFieldEntity);

                formatField.Id = formatTypeFieldEntity.FormatTypeFieldCode;
                return formatField;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        public FormatField UpdateFormatField(FormatField formatField)
        {
            try
            {
                PrimaryKey primaryKey = Entities.FormatTypeField.CreatePrimaryKey(formatField.Id);

                Entities.FormatTypeField formatTypeFieldEntity = (Entities.FormatTypeField)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                formatTypeFieldEntity.Description = formatField.Description;
                formatTypeFieldEntity.Order = formatField.Order;
                formatTypeFieldEntity.Start = formatField.Start;
                formatTypeFieldEntity.Length = formatField.Length;
                formatTypeFieldEntity.Value = formatField.Value;
                formatTypeFieldEntity.Filled = formatField.Filled;
                formatTypeFieldEntity.Align = formatField.Align;
                formatTypeFieldEntity.ColumnNumber = formatField.ColumnNumber;
                formatTypeFieldEntity.RowNumber = formatField.RowNumber;
                formatTypeFieldEntity.Field = formatField.Field;
                formatTypeFieldEntity.FieldFormat = formatField.Mask;
                formatTypeFieldEntity.IsRequired = formatField.IsRequired;

                _dataFacadeManager.GetDataFacade().UpdateObject(formatTypeFieldEntity);

                return formatField;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>FormatField</returns>
        public FormatField DeleteFormatField(FormatField formatField)
        {
            try
            {
                PrimaryKey primaryKey = Entities.FormatTypeField.CreatePrimaryKey(formatField.Id);
                Entities.FormatTypeField formatTypeFieldEntity = (Entities.FormatTypeField)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (formatTypeFieldEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(formatTypeFieldEntity);
                }
                return formatField;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormatFieldsByFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail DeleteFormatFieldsByFormatDetail(FormatDetail formatDetail)
        {
            FormatDetail formatDetails;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(Entities.FormatTypeField.Properties.FormatTypeCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(formatDetail.Id);

                BusinessCollection collections = new BusinessCollection(
                        _dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.FormatTypeField),
                        criteriaBuilder.GetPredicate()));

                foreach (Entities.FormatTypeField FormatTypeField in collections.OfType<Entities.FormatTypeField>())
                {
                    FormatField formatField = new FormatField();
                    formatField.Id = FormatTypeField.FormatTypeFieldCode;
                    DeleteFormatField(formatField);
                }

                formatDetails = _formatDetailDAO.DeleteFormatDetail(formatDetail);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return formatDetails;
        }

        /// <summary>
        /// GetFormatFieldsByFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns> List<FormatField></returns>
        public List<FormatField> GetFormatFieldsByFormatDetail(FormatDetail formatDetail)
        {
            List<FormatField> formatFields = new List<FormatField>();
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(Entities.FormatTypeField.Properties.FormatTypeCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(formatDetail.Format.Id);

                BusinessCollection collections = new BusinessCollection(
                        _dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.FormatTypeField),
                        criteriaBuilder.GetPredicate()));

                foreach (Entities.FormatTypeField formatTypeFieldEntity in collections.OfType<Entities.FormatTypeField>())
                {
                    formatFields.Add(new FormatField
                    {
                        Id = formatTypeFieldEntity.FormatTypeFieldCode,
                        Order = Convert.ToInt32(formatTypeFieldEntity.Order),
                        Description = formatTypeFieldEntity.Description,
                        Start = Convert.ToInt32(formatTypeFieldEntity.Start),
                        Length = Convert.ToInt32(formatTypeFieldEntity.Length),
                        Value = formatTypeFieldEntity.Value,
                        ColumnNumber = Convert.ToInt32(formatTypeFieldEntity.ColumnNumber),
                        RowNumber = Convert.ToInt32(formatTypeFieldEntity.RowNumber),
                        Filled = formatTypeFieldEntity.Filled,
                        Align = formatTypeFieldEntity.Align,
                        Field = formatTypeFieldEntity.Field,
                        Mask = formatTypeFieldEntity.FieldFormat,
                        IsRequired = Convert.ToBoolean(formatTypeFieldEntity.IsRequired)
                    });
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return formatFields;
        }

        #endregion

        #endregion Public Methods

    }
}
