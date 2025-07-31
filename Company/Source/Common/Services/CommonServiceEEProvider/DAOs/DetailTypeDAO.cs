using Sistran.Company.Application.CommonService.Assemblers;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUOENT = Sistran.Core.Application.Quotation.Entities;


namespace Sistran.Company.Application.CommonServices.EEProvider.DAOs
{
    public class DetailTypeDAO
    {
        private IDataFacade _dataFacade;
        public DetailTypeDAO(IDataFacade dataFacade)
        {
            _dataFacade = dataFacade;
        }

        /// <summary>
        /// Obtener lista de tipos de detalle
        /// </summary>
        /// <returns></returns>
        public List<DetailType> GetDetailTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection DetailTypeList = new BusinessCollection(_dataFacade.SelectObjects(typeof(QUOENT.DetailType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.Providers.DetailTypes");
            return ModelAssembler.CreateDetailTypes(DetailTypeList);
        }
    }
}
