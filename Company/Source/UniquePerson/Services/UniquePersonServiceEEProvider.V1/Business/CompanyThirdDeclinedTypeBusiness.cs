using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyThirdDeclinedTypeBusiness
    {

        //public List<CompanyThirdDeclinedType> GetCompanySupplierDeclinedType()
        //{
        //    try
        //    {
        //        var imapper = ModelAssembler.CreateMapperSupplier();
        //        var result = coreProvider.GetSupplierDeclinedTypes();
        //        return imapper.Map<List<SupplierDeclinedType>, List<CompanySupplierDeclinedType>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }

        //}

        /// <summary>
        /// Obtiene los motivos de baja
        /// </summary>
        /// <returns></returns>
        public List<CompanyThirdDeclinedType> GetAllThirdDeclinedTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(DeclinedType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAllOthersDeclinedTypes");
            return ModelAssembler.GetAllThirdDeclinedTypes(businessCollection);
        }
    }
}
