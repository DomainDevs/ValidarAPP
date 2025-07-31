//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Debit = Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class FormatDesignDetailDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveFormatDesignDetail
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Debit.Format SaveFormatDesignDetail(Debit.Format format)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.FormatDesignDetail formatDesignDetail = EntityAssembler.CreateFormatDesignDetail(format);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(formatDesignDetail);

                // Return del model
                return ModelAssembler.CreateFormatDesignDetail(formatDesignDetail, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateFormatDesignDetail
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public Debit.Format UpdateFormatDesignDetail(Debit.Format format)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.FormatDesignDetail.CreatePrimaryKey(format.Id, Convert.ToInt32(format.Description), format.Fields[0].Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.FormatDesignDetail actionFormatDesignDetail = (ACCOUNTINGEN.FormatDesignDetail)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionFormatDesignDetail.DescriptionColumn = format.Fields[0].Description;
                actionFormatDesignDetail.StartNumber = format.Fields[0].Start;
                actionFormatDesignDetail.NumberLength = format.Fields[0].Length;
                actionFormatDesignDetail.Value = format.Fields[0].Value ?? " ";
                actionFormatDesignDetail.Field = format.Fields[0].ExternalDescription ?? " ";
                actionFormatDesignDetail.Format = format.Fields[0].Mask;
                actionFormatDesignDetail.Filler = format.Fields[0].Filled ?? " ";
                actionFormatDesignDetail.Alignment = format.Fields[0].Align ?? " ";
                
                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionFormatDesignDetail);

                // Return del model
                return ModelAssembler.CreateFormatDesignDetail(actionFormatDesignDetail, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePositionOtherFilesformatDesignDetail
        /// </summary>
        /// <param name="format"></param>
        /// <param name="numberColumn"></param>
        /// <param name="fieldLength"></param>
        public void UpdatePositionOtherFilesformatDesignDetail(Debit.Format format, int numberColumn, int fieldLength)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.FormatDesignDetail.CreatePrimaryKey(format.Id, Convert.ToInt32(format.Description), numberColumn);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.FormatDesignDetail actionFormatDesignDetail = (ACCOUNTINGEN.FormatDesignDetail)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionFormatDesignDetail.StartNumber = actionFormatDesignDetail.StartNumber + fieldLength;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionFormatDesignDetail);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormatDesignDetail
        /// </summary>
        /// <param name="format"></param>
        public void DeleteFormatDesignDetail(Debit.Format format)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.FormatDesignDetail.CreatePrimaryKey(format.Id, Convert.ToInt32(format.Description), format.Fields[0].Id);

                ACCOUNTINGEN.FormatDesignDetail actionFormatDesignDetail = (ACCOUNTINGEN.FormatDesignDetail)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(actionFormatDesignDetail);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
