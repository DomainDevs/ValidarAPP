using Sistran.Company.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ModelServices.Models.Reports
{
    public class VehiclesParametersServiceModel : ErrorServiceModel
    {
        public List<VehicleParameterServiceModel> vehicleParameter { get; set; }

    }
}

