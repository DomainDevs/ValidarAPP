using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers;
using Sistran.Company.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.DAOs
{
    public class CoVehicleTypeDAO
    {
        public bool CreateCoVehicleType(int vehicleTypeID)
        {
            try
            {
                CoVehicleType coVehicleType = new CoVehicleType(vehicleTypeID)
                {
                    TransportMinVehicleTypeCode = vehicleTypeID,
                    ApprovedVehicleTypeCode = Convert.ToString(vehicleTypeID),
                };

                DataFacadeManager.Instance.GetDataFacade().InsertObject(coVehicleType);

                return true;
            }
            catch (Exception exc)
            {
                throw new BusinessException("Error en CoVehicleTypeInsert", exc);
            }
        }

        public bool DeleteCoVehicleType(int vehicleTypeID)
        {
            try
            {
                var filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CoVehicleType.Properties.VehicleTypeCode, vehicleTypeID);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(CoVehicleType), filter.GetPredicate());
                return true;
            }
            catch (Exception exc)
            {
                throw new BusinessException("Error en CoVehicleTypeDelete", exc);
            }
        }
    }
}
