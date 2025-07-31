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
    public class FormatDetailDAO
    {
        #region Instance Variables

        /// <summary>
        /// Declaración del contexto y del DataFacadeManager
        /// </summary>
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance Variables

        #region Public Methods

        #region FormatDetail

        /// <summary>
        /// SaveFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail SaveFormatDetail(FormatDetail formatDetail)
        {
            try
            {
                Entities.FormatType formatTypeEntity = EntityAssembler.CreateFormatType(formatDetail);

                _dataFacadeManager.GetDataFacade().InsertObject(formatTypeEntity);

                formatDetail.Id = formatTypeEntity.FormatTypeCode;
                return formatDetail;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail UpdateFormatDetail(FormatDetail formatDetail)
        {
            try
            {
                //Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.FormatType.CreatePrimaryKey(formatDetail.Id);

                Entities.FormatType formatTypeEntity = (Entities.FormatType)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                formatTypeEntity.FormatAreaCode = Convert.ToInt32(formatDetail.FormatType);
                formatTypeEntity.Separator = formatDetail.Separator;

                _dataFacadeManager.GetDataFacade().UpdateObject(formatTypeEntity);

                return formatDetail;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>FormatDetail</returns>
        public FormatDetail DeleteFormatDetail(FormatDetail formatDetail)
        {
            try
            {
                PrimaryKey primaryKey = Entities.FormatType.CreatePrimaryKey(formatDetail.Id);
                Entities.FormatType formatEntity = (Entities.FormatType)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (formatEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(formatEntity);
                }
                return formatDetail;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetFormatDetailsByFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns> List<FormatDetail/></returns>
        public List<FormatDetail> GetFormatDetailsByFormat(Format format)
        {
            List<FormatDetail> formatDetails = new List<FormatDetail>();
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(Entities.FormatType.Properties.FormatCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(format.Id);

                BusinessCollection collections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(Entities.FormatType), criteriaBuilder.GetPredicate()));

                foreach (Entities.FormatType formatTypeEntity in collections.OfType<Entities.FormatType>())
                {
                    List<FormatField> formatFields = new List<FormatField>();
                    // Consulta los detalles de cada sección del formato(cabecera, detalle, sumario)
                    ObjectCriteriaBuilder formatTypeFieldCriteriaBuilder = new ObjectCriteriaBuilder();
                    formatTypeFieldCriteriaBuilder.PropertyEquals(Entities.FormatTypeField.Properties.FormatTypeCode, formatTypeEntity.FormatTypeCode);

                    BusinessCollection collectionsFormatTypeField = new BusinessCollection(
                            _dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.FormatTypeField),
                            formatTypeFieldCriteriaBuilder.GetPredicate()));
                    if (collectionsFormatTypeField.Count > 0)
                    {
                        foreach (Entities.FormatTypeField formatTypeField in collectionsFormatTypeField.OfType<Entities.FormatTypeField>())
                        {
                            formatFields.Add(new FormatField
                            {
                                Id = formatTypeField.FormatTypeFieldCode,
                                Order = Convert.ToInt32(formatTypeField.Order),
                                Description = formatTypeField.Description,
                                Start = Convert.ToInt32(formatTypeField.Start),
                                Length = Convert.ToInt32(formatTypeField.Length),
                                Value = formatTypeField.Value,
                                Filled = formatTypeField.Filled ?? "",
                                Align = formatTypeField.Align ?? "",
                                ColumnNumber = Convert.ToInt32(formatTypeField.ColumnNumber),
                                RowNumber = Convert.ToInt32(formatTypeField.RowNumber),
                                IsRequired = Convert.ToBoolean(formatTypeField.IsRequired),
                                Field = formatTypeField.Field ?? "",
                                Mask = formatTypeField.FieldFormat
                            });
                        }
                    }

                    formatDetails.Add(new FormatDetail
                    {
                        Id = formatTypeEntity.FormatTypeCode,
                        FormatType = (FormatTypes)formatTypeEntity.FormatAreaCode,
                        Separator = formatTypeEntity.Separator,
                        Fields = formatFields
                    });

                    formatDetails = formatDetails.Select(g => g).OrderBy(g => g.FormatType).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return formatDetails;
        }

        #endregion

        #endregion Public Methods

    }
}
