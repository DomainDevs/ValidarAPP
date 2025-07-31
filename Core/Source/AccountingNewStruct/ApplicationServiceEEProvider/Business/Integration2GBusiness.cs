using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Integration2G;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class Integration2GBusiness
    {
        public Models.Integration2G.CollectApplicationControl Save(Models.Integration2G.CollectApplicationControl integrationModel)
        {
            CollectApplicationControlDAO collectApplicationControlDAO = new CollectApplicationControlDAO();
            return integrationModel;
            return collectApplicationControlDAO.Insert(integrationModel);
        }
    }
}
