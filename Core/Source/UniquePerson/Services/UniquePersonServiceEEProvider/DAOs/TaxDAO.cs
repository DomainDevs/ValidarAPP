using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using MUp = Sistran.Core.Application.UniquePersonService.Models;
using entitiesTax = Sistran.Core.Application.Tax.Entities;
using modelTax = Sistran.Core.Application.TaxServices.Models;
using System.Linq;
using System;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.UniquePersonService.DAOs
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
            UniquePerson.Entities.IndividualTax individualTaxEntity = EntityAssembler.CreateIndividualTax(individualTax);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(individualTaxEntity);
            return ModelAssembler.CreateIndividualTax(individualTaxEntity);
        }

        public MUp.IndividualTaxExeption CreateIndividualExemptionTax(MUp.IndividualTaxExeption individualTaxExemption)
        {

            entitiesTax.IndividualTaxExemption entityIndividualTaxExemption = EntityAssembler.CreateIndividualTaxExemption(individualTaxExemption);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityIndividualTaxExemption);
            return ModelAssembler.CreateIndividualTaxExemption(entityIndividualTaxExemption);
        }

        /// <summary>
        /// Elimina el impuesto de la persona
        /// </summary>
        /// <param name="indivualTax"></param>
        /// <returns></returns>
        public void DeleteIndividualTax(MUp.IndividualTax indivualTax)
        {
            PrimaryKey primaryKey = UniquePerson.Entities.IndividualTax.CreatePrimaryKey(indivualTax.Id);
            UniquePerson.Entities.IndividualTax individualTax = (UniquePerson.Entities.IndividualTax)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualTax != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(individualTax);
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
            PrimaryKey primaryKey = UniquePerson.Entities.IndividualTax.CreatePrimaryKey(individualTax.Id);
            UniquePerson.Entities.IndividualTax individualTaxEntities = (UniquePerson.Entities.IndividualTax)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualTaxEntities != null)
            {
                individualTaxEntities.TaxCode = individualTax.Tax.Id;
                individualTaxEntities.TaxConditionCode = individualTax.TaxCondition.Id;
                individualTaxEntities.IndividualId = individualTax.IndividualId;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(individualTaxEntities);
            }
            return ModelAssembler.CreateIndividualTax(individualTaxEntities);
        }


        public MUp.IndividualTaxExeption UpdateIndividualTaxExemption(MUp.IndividualTaxExeption individualTaxExeption)
        {
            PrimaryKey primaryKey = Tax.Entities.IndividualTaxExemption.CreatePrimaryKey(individualTaxExeption.IndividualTaxExemptionId);
            Tax.Entities.IndividualTaxExemption individualTaxExemptionEntities = (Tax.Entities.IndividualTaxExemption)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualTaxExemptionEntities != null)
            {
                individualTaxExemptionEntities.TaxCode = individualTaxExeption.Tax.Id;
                individualTaxExemptionEntities.StateCode = individualTaxExeption.StateCode.Id;
                individualTaxExemptionEntities.TaxCategoryCode = individualTaxExeption.TaxCategory.Id;
                individualTaxExemptionEntities.ResolutionNumber = individualTaxExeption.ResolutionNumber;
                individualTaxExemptionEntities.ExemptionPercentage = individualTaxExeption.ExtentPercentage;
                individualTaxExemptionEntities.BulletinDate = individualTaxExeption.OfficialBulletinDate;
                individualTaxExemptionEntities.CurrentFrom = individualTaxExeption.Datefrom;
                individualTaxExemptionEntities.CurrentTo = individualTaxExeption.DateUntil;
                individualTaxExemptionEntities.HasFullRetention = individualTaxExeption.TotalRetention;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(individualTaxExemptionEntities);
            }
            return ModelAssembler.CreateIndividualTaxExemption(individualTaxExemptionEntities);
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
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateIndivualTaxs");
            return GetIndivualTaxsByIndividualId(individualTax[0].IndividualId);
        }
    

        public List<MUp.IndividualTaxExeption> CreateIndivualExemptionTaxs(List<MUp.IndividualTaxExeption> individualTaxExeptions)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var item in individualTaxExeptions)
            {
                if (item.IndividualTaxExemptionId <= 0)
                {
                    CreateIndividualExemptionTax(item);
                }
                else if (item.Tax.Id == -1)
                {
                    DeleteIndividualTaxExemption(item);
                }
                else
                {
                    UpdateIndividualTaxExemption(item);
                }

            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateIndivualTaxs");
            //return GetIndivualTaxsByExemptionId(individualTaxExeptions[0].IndividualId);
            return individualTaxExeptions;
        }



        /// <summary>
        /// Get Impuestos por individualId de persona
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<MUp.IndividualTax> GetIndivualTaxsByIndividualId(int individualId)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<MUp.IndividualTax> individualTax = new List<MUp.IndividualTax>();
            PersonTaxIndividualTaxView view = new PersonTaxIndividualTaxView();
            ViewBuilder builder = new ViewBuilder("PersonTaxIndividualTaxView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.IndividualTax.Properties.IndividualId, typeof(UniquePerson.Entities.IndividualTax).Name);
            filter.Equal();
            filter.Constant(individualId);
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            individualTax = ModelAssembler.CreateIndividualTaxs(view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetIndivualTaxsByIndividualId");
            return individualTax;

        }

        public List<MUp.IndividualTaxExeption> GetIndivualTaxsByExemptionId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<MUp.IndividualTaxExeption> individualTaxException = new List<MUp.IndividualTaxExeption>();
            List<MUp.IndividualTax> individualTax = new List<MUp.IndividualTax>();
            List<modelTax.Tax> tax = new List<modelTax.Tax>();
            List<modelTax.TaxCondition> taxCondition = new List<modelTax.TaxCondition>();
            TaxIndividualTaxExemptionView view = new TaxIndividualTaxExemptionView();
            ViewBuilder builder = new ViewBuilder("TaxIndividualTaxExemptionView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.IndividualTax.Properties.IndividualId, typeof(UniquePerson.Entities.IndividualTax).Name);
            filter.Equal();
            filter.Constant(individualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            individualTaxException = ModelAssembler.CreateIndividualTaxExemptions(view);
            individualTax = ModelAssembler.CreateIndividualTaxs(view.IndividualTax);
            tax = ModelAssembler.CreateTaxs(view.Tax);
            taxCondition = ModelAssembler.CreateConditionTaxs(view.TaxCondition);

            if (individualTaxException == null || individualTaxException.Count == 0)
            {
                foreach (var item in individualTax)
                {
                    MUp.IndividualTaxExeption individualTaxAdd = new MUp.IndividualTaxExeption();
                    individualTaxAdd.Tax = new modelTax.Tax();
                    individualTaxAdd.Tax.Id = item.Tax.Id;
                    individualTaxAdd.TaxCondition = new modelTax.TaxCondition();
                    individualTaxAdd.TaxCondition.Id = item.TaxCondition.Id;
                    foreach (var itemTax in tax)
                    {
                        individualTaxAdd.Tax.Description = itemTax.Description;
                    }
                 
                    foreach (var itemTaxCondition in taxCondition)
                    {
                        individualTaxAdd.TaxCondition.Description = itemTaxCondition.Description;
                       
                    } 
                  
                    
                    individualTaxException.Add(individualTaxAdd);
                }
                //individualTaxException = ModelAssembler.CreateIndividualTaxExemptions(view);
            }
            


            stopWatch.Stop();
            //Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetIndivualTaxsByIndividualId");
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
                    item.Tax.Id = modIndividualTax.Tax.Id;

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
