using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AuditServices.EEProvider.Helpers
{
    public class CloneHelper
    {
        public static T CloneObject<T>(T obj)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("El tipo de dato debe ser serializable.", "fuente");
            }
            if (Object.ReferenceEquals(obj, null))
            {
                return default(T);
            }
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        public static T CloneField<T>(T obj)
        {
            if (obj.GetType().IsValueType)
            {
                return obj;
            }
            else
            {
                return (T)CloneReferenceType(obj);
            }
        }

        static object CloneReferenceType(object referenceTypeObject)
        {
            var type = referenceTypeObject.GetType();
            var clone = FormatterServices.GetUninitializedObject(type);

            while (type != null)
            {
                Parallel.ForEach(type.GetRuntimeFields().Where
                    (
                    x => !x.IsStatic &&
                        x.DeclaringType == type
                    ).ToList(),
                    fieldInfo =>
                    {
                        fieldInfo.SetValue(clone, CloneObject(fieldInfo.GetValue(referenceTypeObject)));
                    });
                //foreach (var fieldInfo in type
                //    .GetRuntimeFields()
                //    .Where(
                //        x => !x.IsStatic &&
                //        x.DeclaringType == type
                //    )
                //)
                //{
                //    fieldInfo.SetValue(clone, CloneObject(fieldInfo.GetValue(referenceTypeObject)));
                //}

                type = type.BaseType;
            }

            return clone;
        }
    }
}
