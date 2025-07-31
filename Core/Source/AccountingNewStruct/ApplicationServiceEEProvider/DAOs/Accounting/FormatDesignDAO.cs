//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Debit = Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class FormatDesignDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveFormatDesign
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatDesignId"></param>
        /// <param name="formatTypeId"></param>
        /// <param name="formatUsingTypeId"></param>
        /// <returns>Format</returns>
        public Debit.Format SaveFormatDesign(Debit.Format format, int formatDesignId, int formatTypeId, int formatUsingTypeId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.FormatDesign formatDesign = EntityAssembler.CreateFormatDesign(format, formatDesignId, formatTypeId, formatUsingTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(formatDesign);

                // Return del model
                return ModelAssembler.CreateFormatDesign(formatDesign, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateFormatDesign
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatId"></param>
        /// <param name="formatTypeId"></param>
        /// <param name="formatUsingTypeId"></param>
        /// <returns>Format</returns>
        public Debit.Format UpdateFormatDesign(Debit.Format format, int formatId, int formatTypeId, int formatUsingTypeId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.FormatDesign.CreatePrimaryKey(formatId, format.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.FormatDesign actionFormatDesign = (ACCOUNTINGEN.FormatDesign)
                     (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionFormatDesign.FormatTypeCode = formatTypeId;
                actionFormatDesign.UseFileCode = formatUsingTypeId;
                actionFormatDesign.Separator = format.Separator;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionFormatDesign);

                // Return del model
                return ModelAssembler.CreateFormatDesign(actionFormatDesign, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteFormatDesign
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatId"></param>
        /// <returns>Format</returns>
        public Debit.Format DeleteFormatDesign(Debit.Format format, int formatId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.FormatDesign.CreatePrimaryKey(formatId, format.Id);

                ACCOUNTINGEN.FormatDesign actionFormatDesign = (ACCOUNTINGEN.FormatDesign)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(actionFormatDesign);

                // Return del model
                return ModelAssembler.CreateFormatDesign(actionFormatDesign, format);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
