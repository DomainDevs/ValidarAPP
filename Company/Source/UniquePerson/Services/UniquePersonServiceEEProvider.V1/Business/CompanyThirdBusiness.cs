using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyThirdBusiness
    {

        /// <summary>
        /// Crear nuevo tercero
        /// </summary>
        /// <param name="third">modelo third</param>
        /// <returns></returns>
        public CompanyThird CreateThird(CompanyThird third)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CompanyThird thirdPerson = CreateThirdInformationBasic(third);


            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateThird");
            return thirdPerson;
        }

        /// <summary>
        /// Crear información basica tercero
        /// </summary>
        public CompanyThird CreateThirdInformationBasic(CompanyThird third)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ThirdParty thirdEntity = EntityAssembler.CreateThird(third);
            DataFacadeManager.Insert(thirdEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplierInformationBasic");
            return ModelAssembler.CreateThird(thirdEntity);
        }

        /// <summary>
        /// Actualizar tercero
        /// </summary>        
        public CompanyThird UpdateThird(CompanyThird third)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UpdateThirdBasic(third);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateProvider");
            return third;
        }

        /// <summary>
        /// actualizar tercero
        /// </summary>
        /// <param name="supplier">modelo supplier</param>
        /// <returns></returns>
        public void UpdateThirdBasic(CompanyThird third)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = ThirdParty.CreatePrimaryKey(third.Id);
            ThirdParty ThirdEntity = (ThirdParty)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (ThirdEntity != null)
            {
         
                ThirdEntity.DeclinedDate = third.DeclinedDate;


                ThirdEntity.IndividualId = third.IndividualId;
                ThirdEntity.ModificationDate = third.ModificationDate;
                ThirdEntity.Annotation = third.Annotation;
                ThirdEntity.DeclinedTypeCode = third.DeclinedTypeId;
             
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateSupplierInformationBasic");
            DataFacadeManager.Update(ThirdEntity);
        }


        /// <summary>
        /// Consultar tercero por individualID
        /// </summary>
        public CompanyThird GetThirdByIndividualId(int individualId)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                CompanyThird supplier = new CompanyThird();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ThirdParty.Properties.IndividualId, typeof(ThirdParty).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(ThirdParty), filter.GetPredicate()));
                }

                supplier = ModelAssembler.CreateThirsParty(businessCollection).FirstOrDefault();

                //if (supplier != null)
                //{

                //}

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierByIndividualId");
                return supplier;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
            
        }
    }
}
