using Newtonsoft.Json;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Dao;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Bussiness
{
    public class VehicleFasecoldaBussiness
    {
        public void CreateFasecoldaValue(string businessCollection)
        {
            CompanyFasecoldaPrice fasecoldaPrice = JsonConvert.DeserializeObject<CompanyFasecoldaPrice>(businessCollection);
            FaseColdaLoadDAO faseColdaLoadDAO = new FaseColdaLoadDAO();
            faseColdaLoadDAO.CreateFasecoldaPrice(fasecoldaPrice);
        }

        public void CreateFasecoldaCode(string businessCollection)
        {
            CompanyFasecoldaCode fasecoldaCode = JsonConvert.DeserializeObject<CompanyFasecoldaCode>(businessCollection);
            FaseColdaLoadDAO faseColdaLoadDAO = new FaseColdaLoadDAO();
            faseColdaLoadDAO.CreateFasecoldaCode(fasecoldaCode);
        }
    }
}
