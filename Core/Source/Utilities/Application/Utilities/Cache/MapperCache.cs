using AutoMapper;
using System;
using System.Collections.Concurrent;

namespace Sistran.Core.Application.Utilities.Cache
{
    public class MapperCache
    {
        private static ConcurrentDictionary<string, object> mapperCache = new ConcurrentDictionary<string, object>();

        public static IMapper GetMapper<T,U>(Action<IMapperConfigurationExpression> configure)
        {
            IMapper mapper;
            var key = string.Format("{0}_{1}", typeof(T).FullName, typeof(U).FullName);
            if (!MapperCache.mapperCache.ContainsKey(key))
            {
                var config = new MapperConfiguration(configure);
                mapper = config.CreateMapper();
                MapperCache.mapperCache.TryAdd(key, mapper);
            }
            else
            {
                object mapperOut;
                MapperCache.mapperCache.TryGetValue(key, out mapperOut);
                mapper = (IMapper)mapperOut;
            }
            return mapper;
        }
    }
}
