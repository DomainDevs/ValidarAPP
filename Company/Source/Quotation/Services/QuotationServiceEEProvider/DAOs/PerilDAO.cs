using Sistran.Company.Application.QuotationServices.EEProvider.Assemblers;
using Sistran.Company.Application.QuotationServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonEntities = Sistran.Core.Application.Common.Entities;
using QuotationEntities = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Company.Application.QuotationServices.EEProvider.DAOs
{
    public class PerilDAO
    {
        private IDataFacade _dataFacade;
        public PerilDAO(IDataFacade dataFacade)
        {
            _dataFacade = dataFacade;
        }
        /// <summary>
        /// Obtiene la lista de Amparos
        /// </summary>
        /// <returns></returns>
        public List<Peril> GetPerils()
        {
            var businessCollection = new BusinessCollection(_dataFacade.SelectObjects(typeof(QuotationEntities.Peril)));
            return businessCollection.Select(x => ModelAssembler.CreatePeril((QuotationEntities.Peril)x)).OrderBy(p => p.Description).ToList();
        }

        /// <summary>
        /// Obtiene el Id del Peril
        /// </summary>
        /// <returns></returns>
        public int GetIdPeril()
        {
            return GetPerils().Max(p => p.Id) + 1;
        }
        /// <summary>
        /// Crea la lista de amparos
        /// </summary>
        /// <param name="perils"></param>
        /// <returns></returns>
        public ParametrizationResponse<Peril> SavePerils(List<Peril> perilsAdded, List<Peril> perilsModify, List<Peril> perilsDeleted)
        {

            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<Peril> returnPerils = new ParametrizationResponse<Peril>();
            stopWatch.Start();
            using (Context.Current)
            {
                #region Borrar Amparos
                if (perilsDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in perilsDeleted)
                            {
                                PrimaryKey key = QuotationEntities.Peril.CreatePrimaryKey(item.Id);
                                QuotationEntities.Peril perilEntity = new QuotationEntities.Peril(item.Id);
                                perilEntity = (QuotationEntities.Peril)_dataFacade.GetObjectByPrimaryKey(key);
                                _dataFacade.DeleteObject(perilEntity);
                            }
                            transaction.Complete();
                            returnPerils.TotalDeleted = perilsDeleted.Count;
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnPerils.ErrorDeleted = "ErrorSavePerilsRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnPerils.ErrorDeleted = "ErrorSavePerilsDeleted";
                        }
                    }
                }
                #endregion

                #region Modificar Amparos
                if (perilsModify != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in perilsModify)
                            {
                                PrimaryKey key = QuotationEntities.Peril.CreatePrimaryKey(item.Id);
                                QuotationEntities.Peril perilEntity = new QuotationEntities.Peril(item.Id);
                                perilEntity = (QuotationEntities.Peril)_dataFacade.GetObjectByPrimaryKey(key);
                                perilEntity.Description = item.Description;
                                perilEntity.SmallDescription = item.SmallDescription;
                                _dataFacade.UpdateObject(perilEntity);
                            }
                            transaction.Complete();
                            returnPerils.TotalModify = perilsModify.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnPerils.ErrorModify = "ErrorSavePerilsModify";
                        }
                    }
                }
                #endregion

                #region Agregar Amparos
                if (perilsAdded != null)
                {

                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in perilsAdded)
                            {
                                PrimaryKey key = QuotationEntities.Peril.CreatePrimaryKey(item.Id);
                                QuotationEntities.Peril perilEntity = new QuotationEntities.Peril(item.Id);
                                perilEntity = (QuotationEntities.Peril)_dataFacade.GetObjectByPrimaryKey(key);
                                if (GetPerilByDescription(item.Description).Count == 0)
                                {
                                    item.Id = GetIdPeril();
                                    perilEntity = Assemblers.EntityAssembler.CreatePeril(item);
                                    _dataFacade.InsertObject(perilEntity);
                                }
                            }
                            transaction.Complete();
                            returnPerils.TotalAdded = perilsAdded.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnPerils.ErrorAdded = "ErrorSavePerilsAdded";

                        }
                    }
                }
                #endregion

                returnPerils.ReturnedList = GetPerils();
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.QuotationServices.EEProvider.DAOs.CreatePerils");
            return returnPerils;
        }

        /// <summary>
        /// Obtiene los amparos por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Peril> GetPerilByDescription(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(description))
            {
                filter.Property(QuotationEntities.Peril.Properties.Description);
                filter.Equal();
                filter.Constant(description);
            }
            BusinessCollection businessCollection = new BusinessCollection(_dataFacade.SelectObjects(typeof(QuotationEntities.Peril), filter.GetPredicate()));
            return businessCollection.Select(x => ModelAssembler.CreatePeril((QuotationEntities.Peril)x)).OrderBy(p => p.Description).ToList();
        }

        public List<Peril> GetPerilsByLineBusinessId(int lineBusinessId)
        {
            var viewBuilder = new ViewBuilder("CompanyPerilLineBusinessView");
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CommonEntities.LineBusiness.Properties.LineBusinessCode, typeof(CommonEntities.LineBusiness).Name, lineBusinessId);
            viewBuilder.Filter = filter.GetPredicate();
            var perilLineBusinessView = new CompanyPerilLineBusinessView();
            _dataFacade.FillView(viewBuilder, perilLineBusinessView);
            return perilLineBusinessView.Perils.Select(x => ModelAssembler.CreatePeril((QuotationEntities.Peril)x)).ToList();
        }

        public PerilLineBusiness GetPerilsByLineBusinessAssigned(int lineBusinessId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PerilLineBusiness retorno = new PerilLineBusiness();
            retorno.IdLineBusiness = lineBusinessId;
            retorno.PerilAssign = GetPerilsByLineBusinessId(lineBusinessId);
            var totalPerils = GetPerils();
            retorno.PerilNotAssign = new List<Peril>();
            foreach (var item in totalPerils)
            {
                if (retorno.PerilAssign.Find(p => p.Id == item.Id) == null)
                {
                    retorno.PerilNotAssign.Add(item);
                }
            }
            retorno.PerilAssign = retorno.PerilAssign.OrderBy(p => p.Description).ToList();
            retorno.PerilNotAssign = retorno.PerilNotAssign.OrderBy(p => p.Description).ToList();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.QuotationServices.EEProvider.DAOs.GetPerilsByLineBusinessAssigned");
            return retorno;
        }

    }
}
