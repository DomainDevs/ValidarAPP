using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Helper
{
    public class ValidateData
    {
        /// <summary>
        /// Validates the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Campo nulo:</exception>
        public static T ValidateNull<T>(string Name, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Campo nulo:", Name);
            }
            else
            {
                return value;
            }
        }
      
    }
}
