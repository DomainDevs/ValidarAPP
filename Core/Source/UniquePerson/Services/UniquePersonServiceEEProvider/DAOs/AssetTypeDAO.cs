using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.EEProvider.DAOs
{
    public class AssetTypeDAO
    {
        /// <summary>
        /// Consulta listados de tipo de bien
        /// </summary>
        /// <returns> Listado de tipos de bien </returns>
        public List<AssetType> GetAssetType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.AssetType)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetAssetType");
            return ModelAssembler.CreateAssetTypes(businessCollection);
        }
    }
}
