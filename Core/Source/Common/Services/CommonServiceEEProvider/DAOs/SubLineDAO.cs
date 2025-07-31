using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class SubLineDAO
    {
        /// <summary>
        /// Obtener lista de subramos tecnicos
        /// </summary>
        /// <param name="lineBusinessId">Id ramo tecnico</param>
        /// <returns></returns>
        public List<COMMML.SubLineBusiness> GetSubLinesBusinessByLineBusinessId(int lineBusinessId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.SubLineBusiness.Properties.LineBusinessCode, typeof(COMMEN.SubLineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.SubLineBusiness), filter.GetPredicate()));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetSubLinesBusinessByLineBusinessId");
            return ModelAssembler.CreateSubLinesBusiness(businessCollection);
        }

        public int GetSubLineBusinessIdByLineBusinessId(int lineBusinessId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int consecutive = 1;
            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Max);
            function.AddParameter(new Column(COMMEN.SubLineBusiness.Properties.SubLineBusinessCode, typeof(COMMEN.SubLineBusiness).Name));
            select.AddSelectValue(new SelectValue(function, "SubLineBusinessCode"));

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.SubLineBusiness.Properties.LineBusinessCode, typeof(COMMEN.SubLineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            select.Where = filter.GetPredicate();
            select.Table = new ClassNameTable(typeof(COMMEN.SubLineBusiness));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        consecutive = Convert.ToInt32(reader[0].ToString()) + 1;
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetSubLinesBusinessByLineBusinessId");

            return consecutive;
        }
        public ParametrizationResponse<COMMML.SubLineBusiness> CreateSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessAdd, List<COMMML.SubLineBusiness> subLineBusinessEdit, List<COMMML.SubLineBusiness> subLineBusinessDelete)
        {
            ParametrizationResponse<COMMML.SubLineBusiness> returnSubLineBusiness = new ParametrizationResponse<COMMML.SubLineBusiness>();
            #region Agregar SubRamo Técnico
            if (subLineBusinessAdd != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (var item in subLineBusinessAdd)
                        {
                            PrimaryKey key = COMMEN.SubLineBusiness.CreatePrimaryKey(item.Id, item.LineBusinessId);
                            COMMEN.SubLineBusiness subLineBusinessEntity = new COMMEN.SubLineBusiness(item.Id, item.LineBusinessId);
                            subLineBusinessEntity = (COMMEN.SubLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (item.Description != null && item.Description != "")
                            {
                                subLineBusinessEntity = EntityAssembler.CreateSubLineBusiness(item);
                                subLineBusinessEntity.SubLineBusinessCode = GetSubLineBusinessIdByLineBusinessId(subLineBusinessEntity.LineBusinessCode);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(subLineBusinessEntity);
                            }
                        }
                        transaction.Complete();
                        returnSubLineBusiness.TotalAdded = subLineBusinessAdd.Count;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        returnSubLineBusiness.ErrorAdded = "ErrorSaveSubBranchAdded";
                    }
                }
            }
            #endregion

            #region Editar SubRamo Técnico
            if (subLineBusinessEdit != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (var item in subLineBusinessEdit)
                        {
                            PrimaryKey key = COMMEN.SubLineBusiness.CreatePrimaryKey(item.Id, item.LineBusinessId);
                            COMMEN.SubLineBusiness subLineBusinessEntitye = new COMMEN.SubLineBusiness(item.Id, item.LineBusinessId);
                            subLineBusinessEntitye = (COMMEN.SubLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (subLineBusinessEntitye.SmallDescription != item.SmallDescription || subLineBusinessEntitye.Description != item.Description || subLineBusinessEntitye.LineBusinessCode != item.LineBusinessId)
                            {
                                subLineBusinessEntitye.Description = item.Description;
                                subLineBusinessEntitye.SmallDescription = item.SmallDescription;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(subLineBusinessEntitye);
                            }
                        }
                        transaction.Complete();
                        returnSubLineBusiness.TotalModify = subLineBusinessEdit.Count;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        returnSubLineBusiness.ErrorModify = "ErrorSaveSubBranchModify";
                    }
                }
            }
            #endregion

            #region Eliminar SubRamo Técnico
            if (subLineBusinessDelete != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (var item in subLineBusinessDelete)
                        {
                            PrimaryKey key = COMMEN.SubLineBusiness.CreatePrimaryKey(item.Id, item.LineBusinessId);
                            COMMEN.SubLineBusiness subLineBusinessEntity = new COMMEN.SubLineBusiness(item.Id, item.LineBusinessId);
                            subLineBusinessEntity = (COMMEN.SubLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(subLineBusinessEntity);
                        }
                        transaction.Complete();
                        returnSubLineBusiness.TotalDeleted = subLineBusinessDelete.Count;
                    }
                    catch (RelatedObjectException ex)
                    {
                        transaction.Dispose();
                        returnSubLineBusiness.ErrorDeleted = "ErrorSaveSubBranchRelated";
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Dispose();
                        returnSubLineBusiness.ErrorDeleted = "ErrorSaveSubBranchDeleted";
                    }
                }
            }
            #endregion
            returnSubLineBusiness.ReturnedList = GetSubLineBusinessByLineBusinessId();
            return returnSubLineBusiness;
        }

        public List<COMMML.SubLineBusiness> CreateSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessAdd)
        {
            
            #region Agregar SubRamo Técnico
            if (subLineBusinessAdd != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (var item in subLineBusinessAdd)
                        {
                            PrimaryKey key = COMMEN.SubLineBusiness.CreatePrimaryKey(item.Id, item.LineBusinessId);
                            COMMEN.SubLineBusiness subLineBusinessEntity = new COMMEN.SubLineBusiness(item.Id, item.LineBusinessId);
                            subLineBusinessEntity = (COMMEN.SubLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (item.Description != null && item.Description != "")
                            {
                                subLineBusinessEntity = EntityAssembler.CreateSubLineBusiness(item);
                                subLineBusinessEntity.SubLineBusinessCode = GetSubLineBusinessIdByLineBusinessId(subLineBusinessEntity.LineBusinessCode);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(subLineBusinessEntity);
                            }
                        }
                        transaction.Complete();
                       
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw (ex);
                    }
                }
            }
            #endregion


            return GetSubLineBusinessByLineBusinessId();
        }

        public List<COMMML.SubLineBusiness> UpdateSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessEdit)
        {
           
            #region Editar SubRamo Técnico
            if (subLineBusinessEdit != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (var item in subLineBusinessEdit)
                        {
                            PrimaryKey key = COMMEN.SubLineBusiness.CreatePrimaryKey(item.Id, item.LineBusinessId);
                            COMMEN.SubLineBusiness subLineBusinessEntitye = new COMMEN.SubLineBusiness(item.Id, item.LineBusinessId);
                            subLineBusinessEntitye = (COMMEN.SubLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (subLineBusinessEntitye.SmallDescription != item.SmallDescription || subLineBusinessEntitye.Description != item.Description || subLineBusinessEntitye.LineBusinessCode != item.LineBusinessId)
                            {
                                subLineBusinessEntitye.Description = item.Description;
                                subLineBusinessEntitye.SmallDescription = item.SmallDescription;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(subLineBusinessEntitye);
                            }
                        }
                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw (ex);
                    }
                }
            }
            #endregion
            return GetSubLineBusinessByLineBusinessId();
        }

        public List<COMMML.SubLineBusiness> DeleteSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessDelete)
        {

            #region Eliminar SubRamo Técnico
            if (subLineBusinessDelete != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (var item in subLineBusinessDelete)
                        {
                            PrimaryKey key = COMMEN.SubLineBusiness.CreatePrimaryKey(item.Id, item.LineBusinessId);
                            COMMEN.SubLineBusiness subLineBusinessEntity = new COMMEN.SubLineBusiness(item.Id, item.LineBusinessId);
                            subLineBusinessEntity = (COMMEN.SubLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(subLineBusinessEntity);
                        }
                        transaction.Complete();
                    }
                    catch (RelatedObjectException ex)
                    {
                        transaction.Dispose();
                        throw (ex);
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Dispose();
                        throw (ex);
                    }
                }
            }
            #endregion
            return GetSubLineBusinessByLineBusinessId();
            
        }

        public List<COMMML.SubLineBusiness> GetSubLineBusinessByLineBusinessId()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SubLineBusinessView view = new SubLineBusinessView();
            ViewBuilder builder = new ViewBuilder("SubLineBusinessView");
            List<COMMML.SubLineBusiness> sublineBusinessSets = new List<COMMML.SubLineBusiness>();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.SubLineBusiness.Count > 0)
            {
                List<COMMML.SubLineBusiness> subLineBusiness = ModelAssembler.CreateSubLinesBusiness(view.SubLineBusiness);
                List<COMMEN.LineBusiness> entityLineBusiness = view.LineBusiness.Cast<COMMEN.LineBusiness>().ToList();

                foreach (COMMML.SubLineBusiness SubLineBusiness in subLineBusiness)
                {
                    SubLineBusiness.LineBusinessDescription = entityLineBusiness.First(X => X.LineBusinessCode == SubLineBusiness.LineBusinessId).Description;
                    SubLineBusiness.LineBusinessId = entityLineBusiness.First(X => X.LineBusinessCode == SubLineBusiness.LineBusinessId).LineBusinessCode;
                    sublineBusinessSets.Add(SubLineBusiness);
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetSubLineBusinessByLineBusinessId");
            return sublineBusinessSets.OrderBy(x => x.Description).ToList();
        }


    }
}
