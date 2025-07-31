namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider
{
    public static class Initializator
    {
        static Initializator()
        {

        }

        public static void LoadCache()
        {
            CacheBusinessServiceProvider cacheBusinessServiceProvider = new CacheBusinessServiceProvider();
            cacheBusinessServiceProvider.LoadCache(null);
        }

        public static void LoadEnumParameterValuesFromDB()
        {

            CacheBusinessServiceProvider cacheBusinessServiceProvider = new CacheBusinessServiceProvider();
            cacheBusinessServiceProvider.LoadEnumParameterValuesFromDB();
        }
    }
}