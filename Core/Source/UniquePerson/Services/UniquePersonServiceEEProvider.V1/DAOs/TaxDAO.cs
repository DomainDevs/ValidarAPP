using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Business;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using MUp = Sistran.Core.Application.UniquePersonService.V1.Models;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using System.Linq;
using System;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class TaxDAO
    {
        /// <summary>
        /// Crea el impuesto de la persona
        /// </summary>
        /// <param name="indivualTax"></param>
        /// <returns></returns>
        public MUp.IndividualTax CreateIndividualTax(MUp.IndividualTax individualTax)
        {
            TAXEN.IndividualTax individualTaxEntity = EntityAssembler.CreateIndividualTax(individualTax);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(individualTaxEntity);
            return ModelAssembler.CreateIndividualTax(individualTaxEntity);
        }

        public MUp.IndividualTaxExeption CreateIndividualExemptionTax(MUp.IndividualTaxExeption individualTaxExemption)
        {

            TAXEN.IndividualTaxExemption entityIndividualTaxExemption = EntityAssembler.CreateIndividualTaxExemption(individualTaxExemption);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityIndividualTaxExemption);
            return ModelAssembler.CreateIndividualTaxExeption(entityIndividualTaxExemption);
        }

        /// <summary>
        /// Elimina el impuesto de la persona
        /// </summary>
        /// <param name="indivualTax"></param>
        /// <returns></returns>
        public void DeleteIndividualTax(MUp.IndividualTax indivualTax)
        {
            TAXEN.IndividualTax entityIndividualTax = new TAXEN.IndividualTax();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.IndividualTax.Properties.IndividualTaxId, typeof(TAXEN.IndividualTax).Name);
            filter.Equal();
            filter.Constant(indivualTax.Id);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.IndividualTax), filter.GetPredicate()));
            if (businessCollection.Count> 0)
            {
                entityIndividualTax = (TAXEN.IndividualTax)businessCollection.FirstOrDefault();
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityIndividualTax);
            }
        }

        public void DeleteIndividualTaxExemption(MUp.IndividualTaxExeption individualTaxExeption)
        {
            PrimaryKey primaryKey = Tax.Entities.IndividualTaxExemption.CreatePrimaryKey(individualTaxExeption.IndividualTaxExemptionId);
            Tax.Entities.IndividualTaxExemption individualExemptionTax = (Tax.Entities.IndividualTaxExemption)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            
            if (individualExemptionTax != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(individualExemptionTax);
            }
        }

        /// <summary>
        /// Actualizar el impuesto de la persona
        /// </summary>
        /// <param name="indivualTax"></param>
        /// <returns></returns>
        public MUp.IndividualTax UpdateIndividualTax(MUp.IndividualTax individualTax)
        {
            TAXEN.IndividualTax entityIndividualTax = new TAXEN.IndividualTax();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TAXEN.IndividualTax.Properties.IndividualTaxId, typeof(TAXEN.IndividualTax).Name);
            filter.Equal();
            filter.Constant(individualTax.Id);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TAXEN.IndividualTax), filter.GetPredicate()));
           
            if (businessCollection.Count > 0)
            {
                entityIndividualTax = (TAXEN.IndividualTax)businessCollection.FirstOrDefault();
                entityIndividualTax.TaxRateCode = individualTax.TaxRate.Id;
                entityIndividualTax.RoleCode = individualTax.Role.Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityIndividualTax);
            }
            individualTax = ModelAssembler.CreateIndividualTax(entityIndividualTax);
            return new TaxBusiness().GetIndivualTaxsByIndividualId(individualTax.IndividualId).Where(x=>x.Id == individualTax.Id).FirstOrDefault();
        }


        public MUp.IndividualTaxExeption UpdateIndividualTaxExemption(MUp.IndividualTaxExeption individualTaxExeption)
        {
            PrimaryKey primaryKey = Tax.Entities.IndividualTaxExemption.CreatePrimaryKey(individualTaxExeption.IndividualTaxExemptionId);
            Tax.Entities.IndividualTaxExemption individualTaxExemptionEntities = (Tax.Entities.IndividualTaxExemption)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualTaxExemptionEntities != null)
            {
                DateTime? Datefrom = individualTaxExeption.Datefrom;
                individualTaxExemptionEntities.CountryCode = individualTaxExeption.CountryCode;
                individualTaxExemptionEntities.TaxCode = individualTaxExeption.TaxCode;
                individualTaxExemptionEntities.StateCode = individualTaxExeption.StateCode.Id;
                individualTaxExemptionEntities.TaxCategoryCode = individualTaxExeption.TaxCategory.Id;
                individualTaxExemptionEntities.ResolutionNumber = individualTaxExeption.ResolutionNumber;
                individualTaxExemptionEntities.ExemptionPercentage = individualTaxExeption.ExtentPercentage;
                individualTaxExemptionEntities.BulletinDate = individualTaxExeption.OfficialBulletinDate;
                individualTaxExemptionEntities.CurrentFrom = Datefrom == DateTime.MinValue ? null : Datefrom;
                individualTaxExemptionEntities.CurrentTo = individualTaxExeption.DateUntil == DateTime.MinValue ? null : individualTaxExeption.DateUntil;
                individualTaxExemptionEntities.HasFullRetention = individualTaxExeption.TotalRetention;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(individualTaxExemptionEntities);
            }
            return ModelAssembler.CreateIndividualTaxExeption(individualTaxExemptionEntities);
        }
        /// <summary>
        /// Crea los impuestos de la persona
        /// </summary>
        /// <param name="indivualTax"></param>
        /// <returns></returns>
        public List<MUp.IndividualTax> CreateIndivualTaxs(List<MUp.IndividualTax> individualTax)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var item in individualTax)
            {
                if (item.Id <= 0)
                {
                    CreateIndividualTax(item);
                }
                else if (item.Tax.Id == -1)
                {
                    DeleteIndividualTax(item);
                }
                else
                {
                    UpdateIndividualTax(item);
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateIndivualTaxs");
            return individualTax;
        }
    

        public List<MUp.IndividualTaxExeption> CreateIndivualExemptionTaxs(List<MUp.IndividualTaxExeption> individualTaxExeptions)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //foreach (var item in individualTaxExeptions)
            //{
            //    if (item.IndividualTaxExemptionId <= 0)
            //    {
            //        CreateIndividualExemptionTax(item);
            //    }
            //    else if (item.Tax.Id == -1)
            //    {
            //        DeleteIndividualTaxExemption(item);
            //    }
            //    else
            //    {
            //        UpdateIndividualTaxExemption(item);
            //    }

            //}
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateIndivualTaxs");
            //return GetIndivualTaxsByExemptionId(individualTaxExeptions[0].IndividualId);
            return individualTaxExeptions;
        }



        ///// <summary>
        ///// Get Impuestos por individualId de persona
        ///// </summary>
        ///// <param name="individualId"></param>
        ///// <returns></returns>
        //public List<MUp.IndividualTax> GetIndivualTaxsByIndividualId(int individualId)
        //{

        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    List<MUp.IndividualTax> individualTax = new List<MUp.IndividualTax>();
        //    TaxIndividualTaxView view = new TaxIndividualTaxView();
        //    ViewBuilder builder = new ViewBuilder("TaxIndividualTaxView");
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(UniquePersonV1.Entities.IndividualTax.Properties.IndividualId, typeof(UniquePersonV1.Entities.IndividualTax).Name);
        //    filter.Equal();
        //    filter.Constant(individualId);
        //    builder.Filter = filter.GetPredicate();
        //    using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //    {
        //        daf.FillView(builder, view);
        //    }
        //    individualTax = ModelAssembler.CreateIndividualTaxs(view);

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetIndivualTaxsByIndividualId");
        //    return individualTax;

        //}

        public List<MUp.IndividualTaxExeption> GetIndivualTaxsByExemptionId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<MUp.IndividualTaxExeption> individualTaxException = new List<MUp.IndividualTaxExeption>();
            List<MUp.IndividualTax> individualTax = new List<MUp.IndividualTax>();
            List<Models.Tax> tax = new List<Models.Tax>();
            List<Models.TaxCondition> taxCondition = new List<Models.TaxCondition>();
            TaxIndividualTaxExemptionViewV1 view = new TaxIndividualTaxExemptionViewV1();
            ViewBuilder builder = new ViewBuilder("TaxIndividualTaxExemptionViewV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.IndividualTax.Properties.IndividualId, typeof(UniquePersonV1.Entities.IndividualTax).Name);
            filter.Equal();
            filter.Constant(individualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            //individualTaxException = ModelAssembler.CreateIndividualTaxView(view);
            individualTax = ModelAssembler.CreateIndividualTaxs(view.IndividualTax);
            tax = ModelAssembler.CreateTaxs(view.Tax);
           // taxCondition = ModelAssembler.CreateConditionTaxs(view.TaxCondition);

            if (individualTaxException == null || individualTaxException.Count == 0)
            {
                foreach (var item in individualTax)
                {
                    MUp.IndividualTaxExeption individualTaxAdd = new MUp.IndividualTaxExeption();
                    //individualTaxAdd.Tax = new Models.Tax();
                    //individualTaxAdd.Tax.Id = item.Tax.Id;
                    //individualTaxAdd.TaxCondition = new Models.TaxCondition();
                    //individualTaxAdd.TaxCondition.Id = item.TaxCondition.Id;
                    //foreach (var itemTax in tax)
                    //{
                    //    individualTaxAdd.Tax.Description = itemTax.Description;
                    //}
                 
                    //foreach (var itemTaxCondition in taxCondition)
                    //{
                    //    individualTaxAdd.TaxCondition.Description = itemTaxCondition.Description;
                       
                    //} 
                  
                    
                    individualTaxException.Add(individualTaxAdd);
                }
                //individualTaxException = ModelAssembler.CreateIndividualTaxExemptions(view);
            }
            


            stopWatch.Stop();
            //Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetIndivualTaxsByIndividualId");
            return individualTaxException;

        }
        public List<MUp.IndividualTaxExeption> CreateIndividualTaxExemption(List<MUp.IndividualTaxExeption> individualTaxExemption)
        {
            try
            {
                foreach (var item in individualTaxExemption)
                {
                    MUp.IndividualTax modIndividualTax = ModelAssembler.MappIndividualTaxFromIndividualTaxExeption(item);
                    List<MUp.IndividualTax> individualTaxList = new List<MUp.IndividualTax>();
                    individualTaxList.Add(modIndividualTax);
                    individualTaxList = CreateIndivualTaxs(individualTaxList);
                    //item.Tax.Id = modIndividualTax.Tax.Id;

                    MUp.IndividualTaxExeption modIndividualTaxExemption = ModelAssembler.MappIndividualTaxExeptionFromIndividualTaxExeption(item);
                    List<MUp.IndividualTaxExeption> individualTaxExeptionsList = new List<MUp.IndividualTaxExeption>();
                    individualTaxExeptionsList.Add(modIndividualTaxExemption);
                    individualTaxExeptionsList = CreateIndivualExemptionTaxs(individualTaxExeptionsList);
                    //CreateIndivualExemptionTaxs(individualTaxExeptionsList).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            return individualTaxExemption;
        }
    }
}
