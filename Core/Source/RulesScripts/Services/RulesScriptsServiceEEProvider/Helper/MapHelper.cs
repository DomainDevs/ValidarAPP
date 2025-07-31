using System;
using AutoMapper;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Helpers
{
    public class MapHelper
    {
        //public static void Map(object source, object destination)
        //{
        //    Mapper.CreateMap(source.GetType(), destination.GetType());
        //    Mapper.Map(source, destination);
        //}

        public static object ChangeType(object value, Type conversion)
        {
            if (conversion == null)
            {
                return null;
            }
            else
            {
                var t = conversion;

                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                    {
                        return null;
                    }
                    t = Nullable.GetUnderlyingType(t);
                }
                return Convert.ChangeType(value, t);
            }
        }
    }
}
