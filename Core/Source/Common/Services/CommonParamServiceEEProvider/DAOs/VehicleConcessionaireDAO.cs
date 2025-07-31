using System.Collections.Generic;
using Sistran.Core.Application.CommonParamService.Assemblers;
using Sistran.Core.Application.CommonParamService.Models;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.CommonParamService.DAOs
{
    public class VehicleConcessionaireDAO
    {
        internal List<ParamVehicleConcessionaire> GetVehicleConcessionaires()
        {
            return ModelAssembler.CreateVehicleConcessionaires(DataFacadeManager.GetObjects(typeof(Common.Entities.CiaVehicleConcessionaire)));
        }
    }
}
