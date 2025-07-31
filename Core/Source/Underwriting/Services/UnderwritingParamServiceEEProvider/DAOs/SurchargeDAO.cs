// -----------------------------------------------------------------------
// <copyright file="SurchargeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommonService.Models;
    using EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Framework.DAF;
    using Framework.DAF.Engine;
    using Framework.Queries;
    using Models;
    using UnderwritingParamServices.EEProvider.Assemblers;
    using UnderwritingServices.Models;
    using Utilities.DataFacade;
    using Entities.Views;
    using ENUMUN = UnderwritingServices.Enums;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using ParamEntities = Sistran.Core.Application.Parameters.Entities;
    using QuotationEntities = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using EnumsUnCo = Sistran.Core.Application.UnderwritingServices.Enums;


    /// <summary>
    /// DAO para recargos
    /// </summary>
    public class SurchargeDAO
    {
        /// <summary>
        /// Lista los componente y recargos
        /// </summary>
        /// <returns> retorna el modelo de componentes y recargos </returns>
        public UTMO.Result<List<ParamSurcharge>, UTMO.ErrorModel> GetSurcharge()
        {
            try
            {
                List<ParamSurcharge> surchangeComponent = new List<ParamSurcharge>();

                var viewBuilder = new ViewBuilder("SurchargeView");

                SurchargeView surchargeView = new SurchargeView();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, surchargeView);

                if (surchargeView.Surcharge.Count > 0)
                {
                    foreach (QuotationEntities.SurchargeComponent componentQuotation in surchargeView.Surcharge)
                    {
                        QuotationEntities.SurchargeComponent surchargeComponent = surchargeView.Surcharge.Cast<QuotationEntities.SurchargeComponent>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                        QuotationEntities.Component component = surchargeView.Component.Cast<QuotationEntities.Component>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                        ParamEntities.RateType rateType = surchargeView.RateType.Cast<ParamEntities.RateType>().First(x => x.RateTypeCode == componentQuotation.RateTypeCode);
                        surchangeComponent.Add(ModelAssembler.CreateSurcharge(component, surchargeComponent, rateType));
                    }
                }

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(QuotationEntities.Component.Properties.ComponentTypeCode);
                filter.Equal();
                filter.Constant(EnumsUnCo.ComponentType.Surcharges);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QuotationEntities.Component), filter.GetPredicate());

                foreach (QuotationEntities.Component componentSurchange in businessCollection)
                {
                    var query = surchangeComponent.Where(x => x.Id.Equals(componentSurchange.ComponentCode)).FirstOrDefault();
                    if (query == null)
                    {
                    surchangeComponent.Add(ModelAssembler.CreateComponentSurcharge(componentSurchange));
                    }
                }              
                return new UTMO.ResultValue<List<ParamSurcharge>, UTMO.ErrorModel>(surchangeComponent);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingSurchargeErrorBD);
                return new UTMO.ResultError<List<ParamSurcharge>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Lista los recargos
        /// </summary>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public List<Component> GetSurcharges()
        {
            var businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QuotationEntities.Component)));
            return businessCollection.Select(x => ModelAssembler.CreateComponent((QuotationEntities.Component)x)).OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// Obtiene el Id del recargo
        /// </summary>
        /// <returns> retorna id </returns>
        public int GetSurchargeId()
        {
            return this.GetSurcharges().Max(p => p.Id) + 1;
        }

        /// <summary>
        /// Genera archivo excel de recargos
        /// </summary>
        /// <param name="surcharge">Listado de recargos</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToSurcharge(List<ParamSurcharge> surcharge, string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationSurcharge
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamSurcharge item in surcharge)
                    {
                        for (int i = 0; i < file.Templates[0].Rows.Count; i++)
                        {
                            var fields = file.Templates[0].Rows[i].Fields.Select(x => new Field
                            {
                                ColumnSpan = x.ColumnSpan,
                                Description = x.Description,
                                FieldType = x.FieldType,
                                Id = x.Id,
                                IsEnabled = x.IsEnabled,
                                IsMandatory = x.IsMandatory,
                                Order = x.Order,
                                RowPosition = x.RowPosition,
                                SmallDescription = x.SmallDescription
                            }).ToList();

                            if (fields.Count > 1)
                            {
                                fields[0].Value = item.Id.ToString();
                                fields[1].Value = item.Description;
                                fields[2].Value = item.TinyDescription;
                                switch (item.Type)
                                {
                                    case ModelServices.Enums.RateType.Percentage:
                                        fields[3].Value = Resources.Errors.Percentage;
                                        break;
                                    case ModelServices.Enums.RateType.Permilage:
                                        fields[3].Value = Resources.Errors.Permilage;
                                        break;
                                    case ModelServices.Enums.RateType.FixedValue:
                                        fields[3].Value = Resources.Errors.FixedValue;
                                        break;
                                    default:
                                        break;
                                }

                                fields[4].Value = item.Rate.ToString();
                            }

                            rows.Add(new Row
                            {
                                Fields = fields
                            });
                        }
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
            }
        }

        /// <summary>
        /// Elimina el componente
        /// </summary> 
        /// <param name="surcharge"> Modelo de recargos </param>       
        /// <returns> retorna si se elimino </returns>
        public UTMO.Result<ParamSurcharge, UTMO.ErrorModel> DeleteSurchargeComponet(ParamSurcharge surcharge)
        {
            try
            {
                PrimaryKey key = QuotationEntities.SurchargeComponent.CreatePrimaryKey(surcharge.Id);
                var componentEntity = (QuotationEntities.SurchargeComponent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (componentEntity != null)
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(QuotationEntities.SurchargeComponent.Properties.ComponentCode);
                    filter.Equal();
                    filter.Constant(componentEntity.ComponentCode);

                    BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QuotationEntities.SurchargeComponent), filter.GetPredicate());

                    foreach (var itemDelete in businessCollection)
                    {
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemDelete);
                    }
                }
                return new UTMO.ResultValue<ParamSurcharge, UTMO.ErrorModel>(surcharge);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingSurchargeErrorBD);
                return new UTMO.ResultError<ParamSurcharge, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Elimina el recargo del componente
        /// </summary> 
        /// <param name="surcharge"> Modelo de recargos </param>       
        /// <returns> retorna si se elimino </returns>
        public UTMO.Result<ParamSurcharge, UTMO.ErrorModel> DeleteComponet(ParamSurcharge surcharge)
        {
            try
            {
                PrimaryKey key = QuotationEntities.Component.CreatePrimaryKey(surcharge.Id);
                var componentEntity = (QuotationEntities.Component)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QuotationEntities.Component.Properties.ComponentCode);
                filter.Equal();
                filter.Constant(componentEntity.ComponentCode);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QuotationEntities.Component), filter.GetPredicate());

                foreach (var itemDelete in businessCollection)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemDelete);
                }

                return new UTMO.ResultValue<ParamSurcharge, UTMO.ErrorModel>(surcharge);
            }

            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();

                listErrors.Add($"({surcharge.Description}) {Resources.Errors.ItemInUse}");
                return new UTMO.ResultError<ParamSurcharge, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea componente de recargos
        /// </summary>
        /// <param name="item" > Modelo de recargos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public UTMO.Result<ParamSurcharge, UTMO.ErrorModel> CreateComponentSurcharge(ParamSurcharge item)
        {
            try
            {
                item.Id = this.GetSurchargeId();
                QuotationEntities.Component component = this.CreateCompenent(item);
                QuotationEntities.SurchargeComponent surchargeComponent = this.CreateSurchargeComponent(item);
                PrimaryKey key = ParamEntities.RateType.CreatePrimaryKey((int)item.Type);
                ParamEntities.RateType rateType = (ParamEntities.RateType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                ParamSurcharge surchargeParam = ModelAssembler.CreateSurcharge(component, surchargeComponent, rateType);
                return new UTMO.ResultValue<ParamSurcharge, UTMO.ErrorModel>(surchargeParam);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingSurchargeErrorBD);
                return new UTMO.ResultError<ParamSurcharge, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea recargos
        /// </summary>
        /// <param name="item" > Modelo de recargos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public QuotationEntities.SurchargeComponent CreateSurchargeComponent(ParamSurcharge item)
        {
            QuotationEntities.SurchargeComponent surchargeComponent = EntityAssembler.CreateSurchargeComponent(item);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(surchargeComponent);

            return surchargeComponent;
        }

        /// <summary>
        /// Crea recargos
        /// </summary>       
        /// <param name="item" > Modelo de recargos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public QuotationEntities.Component CreateCompenent(ParamSurcharge item)
        {
            QuotationEntities.Component componentEntity = EntityAssembler.CreateSurcharge(item, ENUMUN.ComponentType.Surcharges, ENUMUN.ComponentClassType.Surcharges);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(componentEntity);

            return componentEntity;
        }

        /// <summary>
        /// Acceso a DB para actualización recargos
        /// </summary>
        /// <param name="surchargeParam">recargos MOD-B</param>
        /// <returns>Modelo Result con resultado de operacion de actualizacion</returns>
        public UTMO.Result<ParamSurcharge, UTMO.ErrorModel> UpdateSurchargeComponent(ParamSurcharge surchargeParam)
        {
            try
            {
                QuotationEntities.Component component = this.UpdateComponent(surchargeParam);
                QuotationEntities.SurchargeComponent surchargeComponent = this.UpdateSurcharge(surchargeParam);
                PrimaryKey key = ParamEntities.RateType.CreatePrimaryKey((int)surchargeParam.Type);
                ParamEntities.RateType rateType = (ParamEntities.RateType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                ParamSurcharge surcharge = ModelAssembler.CreateSurcharge(component, surchargeComponent, rateType);
                return new UTMO.ResultValue<ParamSurcharge, UTMO.ErrorModel>(surchargeParam);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingSurchargeErrorBD);
                return new UTMO.ResultError<ParamSurcharge, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Acceso a DB para actualización recargos
        /// </summary>
        /// <param name="surchargeParam">recargos MOD-B</param>
        /// <returns>Modelo Result con resultado de operacion de actualizacion</returns>
        public QuotationEntities.SurchargeComponent UpdateSurcharge(ParamSurcharge surchargeParam)
        {
            PrimaryKey key = QuotationEntities.SurchargeComponent.CreatePrimaryKey(surchargeParam.Id);
            QuotationEntities.SurchargeComponent surchargeEntity = (QuotationEntities.SurchargeComponent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (surchargeEntity != null)
            {
                surchargeEntity.RateTypeCode = (int)surchargeParam.Type;
                surchargeEntity.Rate = surchargeParam.Rate;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(surchargeEntity);
            }
            else
            {
                surchargeEntity = CreateSurchargeComponent(surchargeParam);
            }
            return surchargeEntity;
        }

        /// <summary>
        /// Elimina el componente
        /// </summary>
        /// <returns> retorna si se elimino el descuento </returns>        
        private bool DeleteComponet()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QuotationEntities.Component.Properties.ComponentTypeCode);
                filter.Equal();
                filter.Constant(3);
                filter.And();
                filter.Property(QuotationEntities.Component.Properties.ComponentClassCode);
                filter.Equal();
                filter.Constant(5);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QuotationEntities.Component), filter.GetPredicate());

                foreach (var itemDelete in businessCollection)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemDelete);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Acceso a DB para actualización componente
        /// </summary>
        /// <param name="surchargeParam">recargos MOD-B</param>
        /// <returns>Modelo Result con resultado de operacion de actualizacion</returns>
        private QuotationEntities.Component UpdateComponent(ParamSurcharge surchargeParam)
        {
            PrimaryKey key = QuotationEntities.Component.CreatePrimaryKey(surchargeParam.Id);
            QuotationEntities.Component componentEntity = (QuotationEntities.Component)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            componentEntity.SmallDescription = surchargeParam.Description;
            componentEntity.TinyDescription = surchargeParam.TinyDescription;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(componentEntity);

            return componentEntity;
        }
    }
}