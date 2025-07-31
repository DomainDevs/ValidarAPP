using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Sistran.Core.Application.Utilities.DataFacade
{
    public static class DataFacadeManager
    {
        private static Context context = null;
        private static IDataFacadeManager instance = AppConfigDataFacadeManagerFactory.SingletonInstance;

        public static IDataFacadeManager Instance
        {
            get
            {
                if (Context.Current == null)
                {
                    context = new Context();
                }
                if (BusinessContext.Current != null && String.IsNullOrEmpty(Context.Current?["UserId"]?.ToString()))
                {
                    Context.Current["UserId"] = BusinessContext.Current?.UserId;
                    Context.Current["IpAddress"] = BusinessContext.Current?.IPAddress;
                    Context.Current["AccountName"] = BusinessContext.Current?.AccountName;
                }

                return instance;
            }
        }

        public static void Dispose()
        {
            if (Context.Current != null)
            {
                Context.Current.Dispose();
                context = null;
            }
        }

        public static List<int> GetPackageProcesses(int processesCount, string tagConfig)
        {
            List<int> packageProcesses = new List<int>();
            int packageThreads = Convert.ToInt32(ConfigurationManager.AppSettings[tagConfig]);

            int packages = processesCount / packageThreads;

            for (int i = 0; i < packages; i++)
            {
                packageProcesses.Add(packageThreads);
            }

            int lastPackage = processesCount % packageThreads;

            if (lastPackage > 0)
            {
                packageProcesses.Add(lastPackage);
            }

            return packageProcesses;
        }


        public static BusinessObject Insert(BusinessObject businessObject)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(businessObject);
            return businessObject;
        }

        public static bool Update(BusinessObject businessObject)
        {
            BusinessObject oldBusinessObject = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(businessObject.PrimaryKey);

            if (oldBusinessObject != null)
            {
                foreach (DictionaryEntry property in businessObject.GetProperties())
                {
                    if (!oldBusinessObject.PrimaryKey.GetKeys().Contains(property.Key.ToString()))
                    {
                        oldBusinessObject.SetProperty(property.Key.ToString(), property.Value);
                    }
                }

                oldBusinessObject.ExtendedProperties = businessObject.ExtendedProperties;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(oldBusinessObject);
                return true;
            }
            else
            {
                throw new ValidationException("ENTITY_NOT_FOUND");
            }
        }

        public static bool Delete(PrimaryKey primaryKey)
        {
            BusinessObject businessObject = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (businessObject != null)
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(businessObject);
            return true;
        }

        public static BusinessObject GetObject(PrimaryKey primaryKey)
        {
            return DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
        }

        public static BusinessCollection GetObjects(System.Type type)
        {
            return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(type));
        }

        public static BusinessCollection GetObjects(System.Type type, Predicate predicate)
        {
            return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(type, predicate));
        }

        public static BusinessCollection GetObjects(System.Type type, Predicate predicate, string[] sort)
        {
            return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(type, predicate, sort));
        }

        public static BusinessCollection GetObjects(System.Type type, Predicate predicate, string[] sort, int page, int pageSize, bool getTotalCount)
        {
            return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(type, predicate, sort, page, pageSize, getTotalCount));
        }
    }
}