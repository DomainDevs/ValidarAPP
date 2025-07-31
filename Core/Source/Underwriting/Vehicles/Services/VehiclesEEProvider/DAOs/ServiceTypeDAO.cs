using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Vehicles.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Sistran.Core.Application.Vehicles.EEProvider.DAOs
{
    /// <summary>
    /// Tipo de servicio
    /// </summary>
    public class ServiceTypeDAO
    {
        public static ServiceType GetServiceTypeById(int serviceTypeCode)
        {
            PrimaryKey key = ServiceType.CreatePrimaryKey(serviceTypeCode);
            return (ServiceType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        public static List<Models.ServiceType> GetServiceTypesByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(ServiceType.Properties.ServiceTypeCode, "ServiceType")));
            select.AddSelectValue(new SelectValue(new Column(ServiceType.Properties.Description, "ServiceType")));
            select.AddSelectValue(new SelectValue(new Column(ServiceType.Properties.SmallDescription, "ServiceType")));
            select.AddSelectValue(new SelectValue(new Column(ServiceType.Properties.Enabled, "ServiceType")));

            Join join = new Join(new ClassNameTable(typeof(ServiceType), "ServiceType"), new ClassNameTable(typeof(ServiceTypeProduct), "ServiceTypeProduct"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder())
                            .Property(ServiceType.Properties.ServiceTypeCode, "ServiceType")
                            .Equal()
                            .Property(ServiceTypeProduct.Properties.ServiceTypeCode, "ServiceTypeProduct")
                            .GetPredicate();
            select.Table = join;

            select.Where = (new ObjectCriteriaBuilder())
                            .Property(ServiceTypeProduct.Properties.ProductId, "ServiceTypeProduct")
                            .Equal()
                            .Constant(productId)
                            .GetPredicate();
            List<Models.ServiceType> serticeTipes = new List<Models.ServiceType>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.ServiceType sertiveType;
                while (reader.Read())
                {
                    sertiveType = new Models.ServiceType
                    {
                        Id = (int)reader[ServiceType.Properties.ServiceTypeCode],
                        Description = reader[ServiceType.Properties.Description].ToString(),
                        SmallDescription = reader[ServiceType.Properties.SmallDescription].ToString(),
                        Enabled = (bool)reader[ServiceType.Properties.Enabled]
                    };
                    serticeTipes.Add(sertiveType);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetServiceTypesByProductId");
            return serticeTipes;
        }

        /// <summary>
        /// Obtener Tipos de servicios
        /// </summary>
        /// <returns></returns>
        public List<Models.ServiceType> GetServiceTypes()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ServiceType)));
            return ModelAssembler.CreateServiceTypes(businessCollection);
        }
    }
}
