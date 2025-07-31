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
    public class FormatDAO
    {
        #region Instance Variables

        /// <summary>
        /// Declaración del contexto y del DataFacadeManager
        /// </summary>
        readonly internal static  IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance Variables

        #region Public Methods

        #region Format

        /// <summary>
        /// SaveFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format SaveFormat(Format format)
        {
            try
            {
                Entities.Format formatEntity = EntityAssembler.CreateFormat(format);
                _dataFacadeManager.GetDataFacade().InsertObject(formatEntity);

                format.Id = formatEntity.FormatCode;
                return format;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format UpdateFormat(Format format)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.Format.CreatePrimaryKey(format.Id);

                Entities.Format formatEntity = (Entities.Format)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                formatEntity.FileType = Convert.ToInt32(format.FileType);
                formatEntity.Description = format.Description;
                formatEntity.DateFrom = format.DateFrom;
                formatEntity.DateTo = format.DateTo;

                _dataFacadeManager.GetDataFacade().UpdateObject(formatEntity);

                return format;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format DeleteFormat(Format format)
        {
            try
            {
                PrimaryKey primaryKey = Entities.Format.CreatePrimaryKey(format.Id);
                Entities.Format formatEntity = (Entities.Format)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (formatEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(formatEntity);
                }
                return format;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Format GetFormat(Format format)
        {
            try
            {
                ObjectCriteriaBuilder formatCriteriaFilter = new ObjectCriteriaBuilder();
                formatCriteriaFilter.PropertyEquals(Entities.Format.Properties.FormatCode, format.Id);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.Format),
                    formatCriteriaFilter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return ModelAssembler.CreateFormat(businessCollection);
                }
                else
                {
                    return new Format();
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetFormatsByFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>List<Format/></returns>
        public List<Format> GetFormatsByFormatModule(FormatModule formatModule)
        {
            List<Format> formats = new List<Format>();
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(Entities.Format.Properties.FormatModuleCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(formatModule.Id);

                BusinessCollection collections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(Entities.Format), criteriaBuilder.GetPredicate()));

                foreach (Entities.Format formatEntity in collections.OfType<Entities.Format>())
                {
                    formats.Add(new Format
                    {
                        Id = formatEntity.FormatCode,
                        Description = formatEntity.Description,
                        FileType = (FileTypes)formatEntity.FileType,
                        DateFrom = Convert.ToDateTime(formatEntity.DateFrom),
                        DateTo = Convert.ToDateTime(formatEntity.DateTo)
                    });
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return formats;
        }

        #endregion

        #endregion Public Methods

    }
}
