//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using model = Sistran.Core.Application.AutomaticDebitServices.Models;
using formats = Sistran.Core.Application.ReportingServices.Models.Formats;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class AutomaticDebitFormatDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns></returns>
        public AutomaticDebitFormat SaveAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            try
            {
                // Convertir de model a entity
                Entities.BankNetworkFormat bankNetworkEntity = EntityAssembler.CreateBankNetworkFormat(automaticDebitFormat);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankNetworkEntity);

                // Return del model
                return ModelAssembler.CreateBankNetworkFormat(bankNetworkEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateBankNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns></returns>
        public AutomaticDebitFormat UpdateAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankNetworkFormat.CreatePrimaryKey(automaticDebitFormat.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.BankNetworkFormat bankNetworkFormatEntity = (Entities.BankNetworkFormat)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankNetworkFormatEntity.FormatCode = automaticDebitFormat.Format.Id;
                bankNetworkFormatEntity.FormatUsingType = Convert.ToInt32(automaticDebitFormat.FormatUsingType);

                // Realiza las operaciones con las entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankNetworkFormatEntity);

                // Return del model
                return ModelAssembler.CreateBankNetworkFormat(bankNetworkFormatEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAutomaticDebitFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns></returns>
        public bool DeleteAutomaticDebitFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankNetworkFormat.CreatePrimaryKey(automaticDebitFormat.Id);

                // Realiza las operaciones con las entities utilizando DAF
                Entities.BankNetworkFormat bankNetworkFormatEntity = (Entities.BankNetworkFormat)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(bankNetworkFormatEntity);

                return true;
            }
            catch (Exception exception)
            {
                if (exception.Message == "RELATED_OBJECT")
                {
                    return false;
                }
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetFormatsbyBankNetworkId
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns></returns>
        public List<AutomaticDebitFormat> GetFormatsbyBankNetworkId(int bankNetworkId)
        {
            int rows;
            List<AutomaticDebitFormat> automaticDebitFormats = new List<AutomaticDebitFormat>();

            try
            {
                if (bankNetworkId > 0)
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                    criteriaBuilder.PropertyEquals(Entities.BankNetworkFormat.Properties.BankNetworkCode, bankNetworkId);

                    UIView formats = _dataFacadeManager.GetDataFacade().GetView("BankNetworkFormatView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out rows);

                    if (formats.Count > 0)
                    {
                        foreach (DataRow dataRow in formats)
                        {
                            AutomaticDebitFormat automaticDebitFormat = new AutomaticDebitFormat();
                            string fileType = "";
                            FileTypes fileTypes;
                            FormatUsingTypes formatUsingTypes;

                            if (Convert.ToInt32(dataRow["FileType"]) == 1)
                            {
                                fileType = "Texto";
                                fileTypes = FileTypes.Text;
                            }
                            else if (Convert.ToInt32(dataRow["FileType"]) == 2)
                            {
                                fileType = "Excel";
                                fileTypes = FileTypes.Excel;
                            }
                            else
                            {
                                fileType = "ExcelTemplate";
                                fileTypes = FileTypes.ExcelTemplate;
                            }

                            if (Convert.ToInt32(dataRow["FormatUsingType"]) == 1)
                            {
                                formatUsingTypes = FormatUsingTypes.Sending;
                            }
                            else if (Convert.ToInt32(dataRow["FormatUsingType"]) == 2)
                            {
                                formatUsingTypes = FormatUsingTypes.Reception;
                            }
                            else if (Convert.ToInt32(dataRow["FormatUsingType"]) == 3)
                            {
                                formatUsingTypes = FormatUsingTypes.SendingNotification;
                            }
                            else
                            {
                                formatUsingTypes = FormatUsingTypes.ReceptionNotification;
                            }

                            automaticDebitFormat.BankNetwork = new BankNetwork()
                            {
                                Description = dataRow["BankNetworkDescription"].ToString() + " - " + dataRow["BankNetworkDescription"].ToString(),
                                Id = ReferenceEquals(dataRow["BankNetworkCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["BankNetworkCode"])
                            };
                            automaticDebitFormat.Format = new Format()
                            {
                                Description = dataRow["FormatDescription"].ToString() + " - " + fileType,
                                FileType = fileTypes,
                                Id = ReferenceEquals(dataRow["FormatCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["FormatCode"])
                            };
                            automaticDebitFormat.Id = Convert.ToInt32(dataRow["BankNetworkFormatId"]);
                            automaticDebitFormat.FormatUsingType = formatUsingTypes;

                            automaticDebitFormats.Add(automaticDebitFormat);
                        }
                    }
                }
                else if (bankNetworkId == 0)
                {
                    ObjectCriteriaBuilder filterFormatsbyBankNetwork = new ObjectCriteriaBuilder();
                    filterFormatsbyBankNetwork.Property(Entities.FormatDataDebit.Properties.FormatDataDebitId);
                    filterFormatsbyBankNetwork.GreaterEqual();
                    filterFormatsbyBankNetwork.Constant(0);

                    BusinessCollection formatsbyBankNetworkCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.FormatDataDebit), filterFormatsbyBankNetwork.GetPredicate()));

                    foreach (Entities.FormatDataDebit formatDataDebit in formatsbyBankNetworkCollection.OfType<Entities.FormatDataDebit>())
                    {
                        AutomaticDebitFormat newFormat = new AutomaticDebitFormat()
                        {
                            BankNetwork = new BankNetwork() { Description = formatDataDebit.Format ?? "" },
                            Format = new Format()
                            {
                                Description = formatDataDebit.Description ?? "",
                                Id = formatDataDebit.FormatDataDebitId,
                                FileType = FileTypes.Text,
                            },
                            FormatUsingType = FormatUsingTypes.Sending
                        };

                        automaticDebitFormats.Add(newFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return automaticDebitFormats;
        }

        #endregion

        #endregion
    }
}
