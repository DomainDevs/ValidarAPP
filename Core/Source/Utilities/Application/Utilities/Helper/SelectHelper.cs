using System;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Core.Application.Utilities.Helper
{
    public static class SelectHelper
    {
        /// <summary>
        /// Selects the specified projection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="projection">The projection.</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectReader<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}
