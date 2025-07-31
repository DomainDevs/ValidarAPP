using Sistran.Core.Application.Tax.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
//using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MUp = Sistran.Core.Application.UniquePersonService.V1.Models;
using modelsTax = Sistran.Core.Application.TaxServices.Models;
using TAXEN = Sistran.Core.Application.Tax.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class TaxBusiness
    {
        /// <summary>
        /// crear nuevo impuesto
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public List<Models.IndividualTax> GetIndivualTaxsByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Transaction transaction = new Transaction())
            {
                List<MUp.IndividualTaxExeption> individualTaxExceptions = new List<MUp.IndividualTaxExeption>();
                List<MUp.IndividualTax> individualTax = new List<MUp.IndividualTax>();
                List<Models.IndividualTax> individualTaxResult = new List<Models.IndividualTax>();
                List<MUp.Tax> tax = new List<MUp.Tax>();
                List<modelsTax.TaxRate> taxRates = new List<modelsTax.TaxRate>();
                List<MUp.TaxCondition> taxCondition = new List<MUp.TaxCondition>();
                List<MUp.Role> roles = new List<MUp.Role>();
                TaxIndividualTaxExemptionViewV1 view = new TaxIndividualTaxExemptionViewV1();
                ViewBuilder builder = new ViewBuilder("TaxIndividualTaxExemptionViewV1");
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                try
                {

                    filter.Property(TAXEN.IndividualTax.Properties.IndividualId, typeof(TAXEN.IndividualTax).Name);
                    filter.Equal();
                    filter.Constant(individualId);
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    individualTax = ModelAssembler.CreateIndividualTaxs(view.IndividualTax);
                    tax = ModelAssembler.CreateTaxs(view.Tax);
                    taxRates = ModelAssembler.createTaxRates(view.TaxRate, view.TaxConditions);
                    roles = ModelAssembler.CreateRoles(view.Role);
                    individualTax = ModelAssembler.CreateIndividualTaxTaxRates(taxRates, individualTax);
                    individualTax = ModelAssembler.CreateIndividualTaxRoles(roles, individualTax);

                    individualTaxExceptions = ModelAssembler.CreateIndividualTaxExeptions(view.IndividualTaxExemption, view.State, view.TaxCategory);

                    // taxCondition = ModelAssembler.CreateConditionTaxs(view.TaxCondition);


                    if (individualTax != null && individualTax.Count != 0)
                    {
                        foreach (var item in individualTax)
                        {
                            MUp.IndividualTax individualTaxAdd = new MUp.IndividualTax();

                            individualTaxAdd = new MUp.IndividualTax();
                            individualTaxAdd.Id = item.Id;

                            individualTaxAdd.TaxRate = new modelsTax.TaxRate();
                            individualTaxAdd.TaxRate.Id = item.TaxRate.Id;
                            individualTaxAdd.TaxRate.Tax = new modelsTax.Tax();
                            individualTaxAdd.TaxRate.Tax.Id = item.TaxRate.Tax.Id;
                            individualTaxAdd.TaxRate.Tax.Description = tax.Where(m => m.Id == item.TaxRate.Tax.Id).First().Description;
                            individualTaxAdd.Role = roles.Where(x => x.Id == item.Role.Id).FirstOrDefault();
                            individualTaxAdd.IndividualTaxExeption = new MUp.IndividualTaxExeption();
                            individualTaxAdd.IndividualId = individualId;
                            individualTaxAdd.TaxRate.TaxCondition = new modelsTax.TaxCondition();
                            individualTaxAdd.TaxRate.TaxCondition.Id = item.TaxRate.TaxCondition.Id;
                            individualTaxAdd.TaxRate.TaxCondition.Description = item.TaxRate.TaxCondition.Description;
                            individualTaxAdd.IndividualTaxExeption = individualTaxExceptions.FirstOrDefault(m => m.TaxCode == item.TaxRate.Tax.Id);
        
                            if (individualTaxAdd.IndividualTaxExeption == null && (view.TaxCategory != null && view.TaxCategory.Count > 0))
                            {
                                individualTaxAdd.IndividualTaxExeption = new MUp.IndividualTaxExeption();
                                individualTaxAdd.IndividualTaxExeption.TaxCategory = new MUp.TaxCategory();
                                individualTaxAdd.IndividualTaxExeption.TaxCategory.Id = view.TaxCategory.Cast<TAXEN.TaxCategory>().
                                    FirstOrDefault(x => x.TaxCode == item.TaxRate.Tax.Id && x.TaxCategoryCode == item.TaxRate.TaxCategory.Id).TaxCategoryCode;
                                individualTaxAdd.IndividualTaxExeption.TaxCategory.Description = view.TaxCategory.Cast<TAXEN.TaxCategory>().
                                    FirstOrDefault(x => x.TaxCode == item.TaxRate.Tax.Id && x.TaxCategoryCode == item.TaxRate.TaxCategory.Id).Description;
                            }
                            individualTaxResult.Add(individualTaxAdd);

                        }
                    }
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetIndivualTaxsByExemptionId");
                    return individualTaxResult;
                }
                catch (DuplicatedObjectException)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw;
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw new BusinessException("Error in CreatePerson", ex);
                }

            }

        }



        public MUp.IndividualTax CreateIndividualTax(MUp.IndividualTax individualTax)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var taxBusinessEntity = EntityAssembler.CreateIndividualTax(individualTax);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(taxBusinessEntity);
            //taxBusinessEntity = (TAXEN.IndividualTax)DataFacadeManager.Insert(taxBusinessEntity);
            MUp.IndividualTax individualTaxModel = ModelAssembler.CreateIndividualTax(taxBusinessEntity);
            individualTaxModel = GetIndivualTaxsByIndividualId(individualTaxModel.IndividualId).Where(x => x.Id == individualTaxModel.Id).FirstOrDefault();

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateIndividualTax");
            return individualTaxModel;

        }

        public MUp.IndividualTaxExeption CreateIndividualTaxExeption(MUp.IndividualTaxExeption individualTaxExemption)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var taxBusinessEntity = EntityAssembler.CreateIndividualTaxExemption(individualTaxExemption);
            taxBusinessEntity = (IndividualTaxExemption)DataFacadeManager.Insert(taxBusinessEntity);
            MUp.IndividualTaxExeption individualTaxModel = ModelAssembler.CreateIndividualTaxExeption(taxBusinessEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateIndividualTax");
            return individualTaxModel;

        }

        public MUp.IndividualTax UpdateIndividualTax(MUp.IndividualTax individualTax)
        {

            PrimaryKey primaryKey = UniquePersonV1.Entities.IndividualTax.CreatePrimaryKey(individualTax.Id);
            TAXEN.IndividualTax individualTaxEntities = (TAXEN.IndividualTax)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualTaxEntities != null)
            {
                individualTaxEntities.IndividualId = individualTax.Id;
                individualTaxEntities.TaxRateCode = individualTax.TaxRate.Id;
                individualTaxEntities.IndividualTaxId = individualTax.IndividualId;


                DataFacadeManager.Instance.GetDataFacade().UpdateObject(individualTaxEntities);
            }
            return ModelAssembler.CreateIndividualTax(individualTaxEntities);
        }

        public MUp.IndividualTaxExeption UpdateIndividualTaxExeption(MUp.IndividualTaxExeption individualTaxExeption)
        {

            PrimaryKey primaryKey = Tax.Entities.IndividualTaxExemption.CreatePrimaryKey(individualTaxExeption.IndividualTaxExemptionId);
            Tax.Entities.IndividualTaxExemption individualTaxExemptionEntities = (Tax.Entities.IndividualTaxExemption)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualTaxExemptionEntities != null)
            {
                DateTime? Datefrom = individualTaxExeption.Datefrom;
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

        public void DeleteIndividualTaxExeption(MUp.IndividualTaxExeption individualTaxExeption, int indivudalId)
        {
            PrimaryKey primaryKey = Tax.Entities.IndividualTaxExemption.CreatePrimaryKey(individualTaxExeption.IndividualTaxExemptionId);
            Tax.Entities.IndividualTaxExemption individualExemptionTax = (Tax.Entities.IndividualTaxExemption)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (individualExemptionTax != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(individualExemptionTax);
            }
        }
    }
}
