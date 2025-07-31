using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.CommonParamService.EEProvider.Assemblers;
using Sistran.Company.Application.CommonParamService.EEProvider.Entities.Views;
using Sistran.Company.Application.CommonParamService.EEProvider.Resources;
using Sistran.Company.Application.ModelServices.Models;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonModelsCore = Sistran.Core.Application.CommonService.Models;
using CoreModelsCommom = Sistran.Core.Application.CommonService.Models;
using ParamEntitiesCore = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Company.Application.CommonParamService.EEProvider.DAOs
{
    public class CompanyPrefixDAO
    {
        /// <summary>
        /// Elimina la informacion adicional del ramo
        /// </summary>
        /// <param name="prefixCode">Codigo de ramo</param>
        public void DeleteAditionalInformationPrefix(int prefixCode)
        {
            PrimaryKey keyCptPrefix = Prefix.CreatePrimaryKey(prefixCode);
            PrimaryKey keyCptPrefixScore = CptPrefixScore.CreatePrimaryKey(prefixCode);

            Prefix cptPrefix = (Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCptPrefix);
            //CptPrefixScore cptPrefixScore = (CptPrefixScore)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCptPrefixScore);

            if (cptPrefix != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(cptPrefix);
            }

            //if (cptPrefixScore != null)
            //{
            //    DataFacadeManager.Instance.GetDataFacade().DeleteObject(cptPrefixScore);
            //}
        }

        /// <summary>
        /// Actualiza o crea la informacion adicional del ramo
        /// </summary>
        /// <param name="prefix">Modelo de ramo</param>
        public void SaveAditionalInformationPrefix(CompanyPrefix prefix)
        {
            if (prefix.AditionalInformation != null)
            {
                PrimaryKey keyCptPrefix = CptPrefix.CreatePrimaryKey(prefix.Id);
                CptPrefix cptPrefix = (CptPrefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCptPrefix);

                if (cptPrefix == null)
                {
                    EntityAssembler.CreateCptPrefix(prefix, ref cptPrefix);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(cptPrefix);
                }
                else
                {
                    EntityAssembler.CreateCptPrefix(prefix, ref cptPrefix);
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(cptPrefix);
                }

                PrimaryKey keyCptPrefixScore = CptPrefixScore.CreatePrimaryKey(prefix.Id);
                CptPrefixScore cptPrefixScore = (CptPrefixScore)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCptPrefixScore);

                if (cptPrefixScore == null)
                {
                    EntityAssembler.CreateCptPrefixScore(prefix, ref cptPrefixScore);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(cptPrefixScore);
                }
                else
                {
                    EntityAssembler.CreateCptPrefixScore(prefix, ref cptPrefixScore);
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(cptPrefixScore);
                }
            }
        }

        /// <summary>
        /// Crea, Actualiza y Elimina los ramos de acuerdo a su estado
        /// </summary>
        /// <param name="prefix">Ramo tecnico</param>
        /// <returns>Listado de mensajes de resultado de operacion</returns>
        public List<string> SavePrefix(List<CompanyPrefix> prefix)
        {
            List<string> result = new List<string>();
            int newRegister = 0;
            int deleteRegister = 0;

            foreach (CompanyPrefix item in prefix)
            {
                PrimaryKey key = Prefix.CreatePrimaryKey(item.Id);
                Prefix prefixEntity = new Prefix(item.Id);

                prefixEntity = (Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (prefixEntity != null)
                {
                    if (item.Status == ((int)StatusTypeService.Delete).ToString())
                    {
                        using (Transaction transaction = new Transaction())
                        {
                            try
                            {
                                this.DeleteLineBusinessByPrefixIdLineId(item.Id, 0);
                                this.DeleteAditionalInformationPrefix(item.Id);
                                //DataFacadeManager.Instance.GetDataFacade().DeleteObject(prefixEntity);
                                deleteRegister += 1;
                                transaction.Complete();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.CompareTo("ERROR_DELETING_OBJECT")==0)
                                    result.Add("No se puede eliminar" + " " + item.Description + "(" + item.Id + ") " + "Asociado");
                                else
                                result.Add(Errors.ErrorDeleteItem + " " + item.Description + "(" + item.Id + ") " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        using (Transaction transaction = new Transaction())
                        {
                            try
                            {
                                prefixEntity.Description = item.Description;
                                prefixEntity.TinyDescription = item.TinyDescription;
                                prefixEntity.SmallDescription = item.SmallDescription;
                                prefixEntity.PrefixTypeCode = item.PrefixType.Id;
                                prefixEntity.HasDetailCommiss = item.HasDetailCommiss;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(prefixEntity);
                                this.SaveLineBusinessPrefix(item);
                                this.SaveAditionalInformationPrefix(item);
                                newRegister += 1;
                                transaction.Complete();
                            }
                            catch (Exception ex)
                            {
                                result.Add(Errors.ErrorUpdateItem + " " + item.Description + "(" + item.Id + ") " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    if (item.Status != StatusTypeService.Delete.ToString())
                    {
                        using (Transaction transaction = new Transaction())
                        {
                            try
                            {
                                prefixEntity = EntityAssembler.CreatePrefix(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(prefixEntity);
                                item.Id = prefixEntity.PrefixCode;
                                this.SaveLineBusinessPrefix(item);
                                //this.SaveAditionalInformationPrefix(item);
                                newRegister += 1;
                                transaction.Complete();
                            }
                            catch (Exception ex)
                            {
                                result.Add(Errors.ErrorInsertItem + " " + item.Description + "(" + item.Id + ") " + ex.Message);
                            }
                        }
                    }
                }
            }

            if (newRegister > 0)
            {
                result.Add("Se actualizaron " + newRegister + " registros");
            }

            if (deleteRegister > 0)
            {
                result.Add("Se eliminaron " + deleteRegister + " registros");
            }

            return result;
        }

        private void SaveLineBusinessPrefix(CompanyPrefix prefix)
        {
            foreach (CoreModelsCommom.LineBusiness item in prefix.LineBusiness)
            {
                PrimaryKey keyLnBsPrefix = PrefixLineBusiness.CreatePrimaryKey(prefix.Id, item.Id);
                PrefixLineBusiness lnBsPrefix = (PrefixLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyLnBsPrefix);
                item.TyniDescription = "";
                item.ShortDescription = "";
                if (lnBsPrefix != null && item.Status == ((int)StatusTypeService.Delete).ToString())
                {
                    this.DeleteLineBusinessByPrefixIdLineId(prefix.Id, item.Id);
                }
                if (lnBsPrefix == null)
                {
                    lnBsPrefix = new PrefixLineBusiness(prefix.Id, item.Id);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(lnBsPrefix);
                }
            }
        }

        /// <summary>
        /// borra las lineas de negocio asociadas al prefix
        /// </summary>
        /// <param name="prefixId">prefix Id</param>
        /// <param name="lineId">line Id</param>
        public void DeleteLineBusinessByPrefixIdLineId(int prefixId, int lineId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PrefixLineBusiness.Properties.PrefixCode, typeof(PrefixLineBusiness).Name);
            filter.Equal();
            filter.Constant(prefixId);

            if (lineId != 0)
            {
                filter.And();
                filter.Property(PrefixLineBusiness.Properties.LineBusinessCode, typeof(PrefixLineBusiness).Name);
                filter.Equal();
                filter.Constant(lineId);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PrefixLineBusiness), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                List<PrefixLineBusiness> entities = businessCollection.Cast<PrefixLineBusiness>().ToList();
                foreach (PrefixLineBusiness item in entities)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }
            }
        }
        
        /// <summary>
        /// Obtenr Modelo Ramos Comerciales
        /// </summary>
        /// <returns>Lista de Ramos tecnicos</returns>
        public List<CompanyPrefix> GetAllPrefix()
        {
            ViewBuilder viewPrefix = new ViewBuilder("CompanyPrefixParametrizationView");
            CompanyPrefixParametrizationView prefixParametrizationView = new CompanyPrefixParametrizationView();
            DataFacadeManager.Instance.GetDataFacade().FillView(viewPrefix, prefixParametrizationView);

            List<ParamEntitiesCore.PrefixType> listPrefixType = DataFacadeManager.Instance.GetDataFacade().List(typeof(ParamEntitiesCore.PrefixType), null).Cast<ParamEntitiesCore.PrefixType>().ToList();
            CompanyLineBusinessPrefixView view = new CompanyLineBusinessPrefixView();
            ViewBuilder builder = new ViewBuilder("CompanyLineBusinessPrefixView");
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<CommonModelsCore.LineBusiness> listLineBusiness = ModelAssembler.CreateLinesBusiness(view.LineBusiness);
            if (prefixParametrizationView.Prefix != null && prefixParametrizationView.Prefix.Count > 0)
            {
                List<CompanyPrefix> listPrefix = new List<CompanyPrefix>();
                foreach (Prefix entityPrefix in prefixParametrizationView.Prefix)
                {
                    CompanyPrefix prefix = ModelAssembler.CreatePrefix(entityPrefix);
                    prefix.PrefixType = new CommonModelsCore.PrefixType();
                    prefix.PrefixType.Id = listPrefixType.First(m => m.PrefixTypeCode == prefix.PrefixTypeCode).PrefixTypeCode;
                    prefix.PrefixType.Description = listPrefixType.First(m => m.PrefixTypeCode == prefix.PrefixTypeCode).Description;
                    prefix.LineBusiness = new List<CommonModelsCore.LineBusiness>();
                    var prefixLineBusiness = view.PrefixLineBusiness.Cast<PrefixLineBusiness>().Where(y => y.PrefixCode == prefix.Id).Select(x => x.LineBusinessCode).ToList();
                    prefix.LineBusiness = listLineBusiness.Where(x => prefixLineBusiness.Contains(x.Id)).ToList();                    
                    listPrefix.Add(prefix);
                }

                return listPrefix;
            }
            else
            {
                return null;
            }
        }
    }
}
