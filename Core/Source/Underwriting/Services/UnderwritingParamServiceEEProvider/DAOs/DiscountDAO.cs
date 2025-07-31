// -----------------------------------------------------------------------
// <copyright file="DiscountDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using EEProvider.Assemblers;
    using Entities.Views;
    using Framework.DAF;
    using Framework.DAF.Engine;
    using Framework.Queries;
    using Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnderwritingParamServices.EEProvider.Assemblers;
    using UnderwritingServices.Models;
    using Utilities.DataFacade;
    using ENUMUN = UnderwritingServices.Enums;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using ParamEntities = Sistran.Core.Application.Parameters.Entities;
    using QuotationEntities = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// DAO para descuentos
    /// </summary>
    public class DiscountDAO
    {
        /// <summary>
        /// Lista los componente
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public UTMO.Result<List<ParamDiscount>, UTMO.ErrorModel> GetDiscount()
        {
            try
            {
                List<ParamDiscount> surchangeComponent = new List<ParamDiscount>();

                var viewBuilder = new ViewBuilder("DiscountView");

                DiscountView discountView = new DiscountView();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, discountView);
                foreach (QuotationEntities.DiscountComponent componentQuotation in discountView.Discount)
                {
                    QuotationEntities.DiscountComponent discountComponent = discountView.Discount.Cast<QuotationEntities.DiscountComponent>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    QuotationEntities.Component component = discountView.Component.Cast<QuotationEntities.Component>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    ParamEntities.RateType rateType = discountView.RateType.Cast<ParamEntities.RateType>().First(x => x.RateTypeCode == componentQuotation.RateTypeCode);
                    surchangeComponent.Add(ModelAssembler.CreateDiscount(component, discountComponent, rateType));
                }

                return new UTMO.ResultValue<List<ParamDiscount>, UTMO.ErrorModel>(surchangeComponent);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<List<ParamDiscount>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }           
        }

        /// <summary>
        /// Crea componente de descuentos
        /// </summary>
        /// <param name="item"> Modelo de descuentos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public UTMO.Result<ParamDiscount, UTMO.ErrorModel> CreateComponentDiscount(ParamDiscount item)
        {
            try
            {
                item.Id = this.GetDicount();
                QuotationEntities.Component component = this.CreateCompenent(item);
                QuotationEntities.DiscountComponent discountComponent = this.CreateDiscountComponent(item);
                PrimaryKey key = ParamEntities.RateType.CreatePrimaryKey((int)item.Type);
                ParamEntities.RateType rateType = (ParamEntities.RateType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                ParamDiscount discountParam = ModelAssembler.CreateDiscount(component, discountComponent, rateType);
                return new UTMO.ResultValue<ParamDiscount, UTMO.ErrorModel>(discountParam);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<ParamDiscount, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }        

        /// <summary>
        /// Crea descuentos
        /// </summary>       
        /// <param name="item" > Modelo de descuentos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public QuotationEntities.Component CreateCompenent(ParamDiscount item)
        {
            QuotationEntities.Component componentEntity = EntityAssembler.CreateComponent(item, ENUMUN.ComponentType.Discounts, ENUMUN.ComponentClassType.Discounts);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(componentEntity);

            return componentEntity;
        }

        /// <summary>
        /// Acceso a DB para actualización descuentos
        /// </summary>
        /// <param name="discountParam">descuentos MOD-B</param>
        /// <returns>Modelo Result con resultado de operacion de actualizacion</returns>
        public UTMO.Result<ParamDiscount, UTMO.ErrorModel> UpdateDiscountComponent(ParamDiscount discountParam)
        {
            try
            {
                QuotationEntities.Component component = this.UpdateComonent(discountParam);
                QuotationEntities.DiscountComponent discountComponent = this.UdateDiscount(discountParam);
                PrimaryKey key = ParamEntities.RateType.CreatePrimaryKey((int)discountParam.Type);
                ParamEntities.RateType rateType = (ParamEntities.RateType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                ParamDiscount discount = ModelAssembler.CreateDiscount(component, discountComponent, rateType);
                return new UTMO.ResultValue<ParamDiscount, UTMO.ErrorModel>(discountParam);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<ParamDiscount, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Elimina el componente
        /// </summary> 
        /// <param name="discount"> Modelo de descuentos </param>       
        /// <returns> retorna si se elimino </returns>
        public UTMO.Result<ParamDiscount, UTMO.ErrorModel> DeleteDiscountComponet(ParamDiscount discount)
        {
            try
            {
                PrimaryKey key = QuotationEntities.DiscountComponent.CreatePrimaryKey(discount.Id);
                var componentEntity = (QuotationEntities.DiscountComponent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QuotationEntities.DiscountComponent.Properties.ComponentCode);
                filter.Equal();
                filter.Constant(componentEntity.ComponentCode);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QuotationEntities.DiscountComponent), filter.GetPredicate());

                foreach (var itemDelete in businessCollection)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemDelete);
                }

                return new UTMO.ResultValue<ParamDiscount, UTMO.ErrorModel>(discount);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<ParamDiscount, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Lista los descuentos
        /// </summary>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public List<Component> GetDicounts()
        {
            var businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QuotationEntities.Component)));
            return businessCollection.Select(x => ModelAssembler.CreateComponent((QuotationEntities.Component)x)).OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// Obtiene el Id del Descuento
        /// </summary>
        /// <returns> retorna id </returns>
        public int GetDicount()
        {
            return this.GetDicounts().Max(p => p.Id) + 1;
        }

        /// <summary>
        /// Elimina el recargo del componente
        /// </summary> 
        /// <param name="discount"> Modelo de descuentos </param>       
        /// <returns> retorna si se elimino </returns>
        public UTMO.Result<ParamDiscount, UTMO.ErrorModel> DeleteComponet(ParamDiscount discount)
        {
            try
            {
                PrimaryKey key = QuotationEntities.Component.CreatePrimaryKey(discount.Id);
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

                return new UTMO.ResultValue<ParamDiscount, UTMO.ErrorModel>(discount);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add($"({discount.Description}) {Resources.Errors.ItemInUse}");
                //listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<ParamDiscount, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Genera archivo excel de coverturas
        /// </summary>
        /// <param name="discount">Listado de descuentos</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToDiscount(List<ParamDiscount> discount, string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationDiscount
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamDiscount item in discount)
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
                                fields[0].Value = item.Rate.ToString();
                                switch (item.Type)
                                {
                                    case ModelServices.Enums.RateType.Percentage:
                                        fields[1].Value = Resources.Errors.Percentage;
                                        break;
                                    case ModelServices.Enums.RateType.Permilage:
                                        fields[1].Value = Resources.Errors.Permilage;
                                        break;
                                    case ModelServices.Enums.RateType.FixedValue:
                                        fields[1].Value = Resources.Errors.FixedValue;
                                        break;
                                    default:
                                        break;
                                }    
                                                           
                                fields[2].Value = item.Description;
                                fields[3].Value = item.TinyDescription;
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
        /// Elimina el descuento componente
        /// </summary> 
        /// <param name="discountId"> Modelo de descuentos </param>       
        /// <returns> retorna si se elimino </returns>
        private bool DeleteDiscountComponet(PrimaryKey discountId)
        {
            try
            {
                var componentEntity = (Sistran.Core.Application.Quotation.Entities.Component)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(discountId);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QuotationEntities.DiscountComponent.Properties.ComponentCode);
                filter.Equal();
                filter.Constant(componentEntity.ComponentCode);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QuotationEntities.DiscountComponent), filter.GetPredicate());

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
        /// Actualiza componente de descuentos
        /// </summary>
        /// <param name="discountParam"> Modelo de descuentos </param>
        /// <returns> retorna enitad de componente </returns>
        private QuotationEntities.Component UpdateComonent(ParamDiscount discountParam)
        {
            PrimaryKey key = QuotationEntities.Component.CreatePrimaryKey(discountParam.Id);
            QuotationEntities.Component componentEntity = (QuotationEntities.Component)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            componentEntity.SmallDescription = discountParam.Description;
            componentEntity.TinyDescription = discountParam.TinyDescription;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(componentEntity);

            return componentEntity;
        }

        /// <summary>
        /// Crea descuentos
        /// </summary>
        /// <param name="item" > Modelo de descuentos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        private QuotationEntities.DiscountComponent CreateDiscountComponent(ParamDiscount item)
        {
            QuotationEntities.DiscountComponent discountComponent = EntityAssembler.CreateDiscountComponent(item);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(discountComponent);

            return discountComponent;
        }

        /// <summary>
        /// Crea descuentos
        /// </summary>
        /// <param name="discountParam"> Modelo de descuentos</param>
        /// <returns> retorna entidad de descuentos </returns>
        private QuotationEntities.DiscountComponent UdateDiscount(ParamDiscount discountParam)
        {
            PrimaryKey key = QuotationEntities.DiscountComponent.CreatePrimaryKey(discountParam.Id);
            QuotationEntities.DiscountComponent discountEntity = (QuotationEntities.DiscountComponent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            discountEntity.RateTypeCode = (int)discountParam.Type;
            discountEntity.Rate = discountParam.Rate;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(discountEntity);

            return discountEntity;
        }
    }
}