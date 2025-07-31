using System.Collections.Generic;
using System.Data;

namespace Domain.DataAccess
{
    public interface IDataAccess
    {
        DataSet consultarSP(string storeProcedure, IDictionary<string, object> param);

        DataSet consultarSP(string storeProcedure, int id);
    }
}
