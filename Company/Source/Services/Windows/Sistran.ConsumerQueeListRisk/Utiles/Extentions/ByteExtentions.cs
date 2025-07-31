using System;

namespace Utiles.Extentions
{
    public static class ByteExtentions
    {
        /// <summary>
        /// Function to get object from byte array
        /// </summary>
        /// <param name="byteArray">byte array to get object</param>
        /// <returns>object</returns>
        public static T ByteArrayToObject<T>(this byte[] byteArray)
        {
            try
            {
                // convert byte array to memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(byteArray);

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // set memory stream position to starting point
                _MemoryStream.Position = 0;

                // Deserializes a stream into an object graph and return as a object.
                return (T)_BinaryFormatter.Deserialize(_MemoryStream);
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return default(T);
        }
    }
}