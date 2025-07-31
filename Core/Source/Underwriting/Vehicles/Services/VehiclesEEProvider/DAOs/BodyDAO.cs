using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Vehicles.EEProvider.Assemblers;
using System.Diagnostics;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.Vehicles.EEProvider.DAOs
{
    /// <summary>
    /// Carroceria
    /// </summary>
    public class BodyDAO
    {
        /// <summary>
        /// Obtener lista de colores
        /// </summary>
        /// <returns></returns>
        public List<Models.Body> GetBodies()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleBody)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetBodies");
            return ModelAssembler.CreateBodies(businessCollection);
        }

        /// <summary>
        /// Obtener lista de carrocerias por tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeId"></param>
        /// <returns></returns>
        public List<Models.Body> GetBodiesByVehicleTypeId(int vehicleTypeId)
        {
            List<Models.Body> bodies = new List<Models.Body>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleTypeBody.Properties.VehicleTypeCode, typeof(VehicleTypeBody).Name);
            filter.Equal();
            filter.Constant(vehicleTypeId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleTypeBody), filter.GetPredicate()));

            List<Models.Body> listBodies = GetBodies();

            foreach (VehicleTypeBody item in businessCollection)
            {
                Models.Body body = new Models.Body()
                {
                    Id = item.VehicleBodyCode,
                    Description = listBodies.First(x => x.Id == item.VehicleBodyCode).Description
                };
                bodies.Add(body);
            }


            return bodies;
        }
    }
}
