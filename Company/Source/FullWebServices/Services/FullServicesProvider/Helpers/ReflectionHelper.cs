using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using System.Data;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;


namespace Sistran.Co.Previsora.Application.FullServicesProvider.Helpers
{
    public static class ReflectionHelper
    {

        public static void Update<T>(this T entity, string field, object value)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(field);
            if (propertyInfo != null)
                propertyInfo.SetValue(entity, value, null);
        }

        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propInfo = null;
            do
            {
                propInfo = type.GetProperty(propertyName,
                       BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (propInfo == null && type != null);
            return propInfo;
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type objType = obj.GetType();
            PropertyInfo propInfo = GetPropertyInfo(objType, propertyName);
            if (propInfo == null)
                throw new ArgumentOutOfRangeException("propertyName",
                  string.Format("Couldn't find property {0} in type {1}", propertyName, objType.FullName));
            return propInfo.GetValue(obj, null);
        }

        public static void SetPropertyValue(this object obj, string propertyName, object val)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type objType = obj.GetType();
            PropertyInfo propInfo = GetPropertyInfo(objType, propertyName);
            if (propInfo == null)
                throw new ArgumentOutOfRangeException("propertyName",
                  string.Format("Couldn't find property {0} in type {1}", propertyName, objType.FullName));
            propInfo.SetValue(obj, val, null);
        }

        private static DataSet TableEsquem;

        public static void EvalsDto(DtoMaster dtoMaster, int id_rol, DataSet TempTableEsquem)
        {
            TableEsquem = TempTableEsquem;
            Type type = typeof(DtoMaster);
            string DtoValidate = string.Empty;
            bool IsValidate = false;
            switch (id_rol)
            {
                case 1: //SI EL ROL ES ASEGURADO
                    DtoValidate = "DtoInsured";
                    break;
                case 2: //SI EL ROL ES ABOGADO
                    DtoValidate = "DtoLawyer";
                    break;
                case 3: //SI EL ROL ES BENEFICIARIO
                    DtoValidate = "DtoBeneficiary";
                    break;
                case 4: //SI EL ROL ES CESIONARIO
                    DtoValidate = "DtoAssigneed";
                    break;
                case 5: //SI EL ROL ES DIRECTOR NACIONAL
                    DtoValidate = "DtoPrincipalNational";
                    break;
                case 6: //SI EL ROL ES DIRECTOR COMERCIAL
                    DtoValidate = "DtoPrincipalComertial";
                    break;
                case 7: //SI EL ROL ES ASISTENTE TECNICO
                    DtoValidate = "DtoTechnicalAssistant";
                    break;
                case 8: //SI EL ROL ES EMPLEADO
                    DtoValidate = "DtoEmployee";
                    break;
                case 9: //SI EL ROL ES INTERMEDIARIO/AGENTE  
                    DtoValidate = "DtoAgent";
                    break;
                case 10: //SI EL ROL ES PROVEEDOR
                    DtoValidate = "DtoProvider";
                    break;
                case 11: //SI EL ROL ES TERCERO
                    DtoValidate = "DtoThird";
                    break;
                case 12: //SI EL ROL ES USUARIO
                    DtoValidate = "DtoUser";
                    break;
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                string FieldName = propertyInfo.Name;
                switch (FieldName)
                {
                    case "DtoInsured": //SI EL ROL ES ASEGURADO
                        Entitiesproperties(dtoMaster.DtoInsured);
                        IsValidate = true;
                        break;
                    case "DtoLawyer": //SI EL ROL ES ABOGADO
                        Entitiesproperties(dtoMaster.DtoLawyer);
                        IsValidate = true;
                        break;
                    case "DtoBeneficiary": //SI EL ROL ES BENEFICIARIO
                        Entitiesproperties(dtoMaster.DtoBeneficiary);
                        IsValidate = true;
                        break;
                    case "DtoAssigneed": //SI EL ROL ES CESIONARIO
                        Entitiesproperties(dtoMaster.DtoAssigneed);
                        IsValidate = true;
                        break;
                    case "DtoPrincipalNational": //SI EL ROL ES DIRECTOR NACIONAL
                        //Entitiesproperties(dtoMaster.DtoPrincipalNational);
                        //IsValidate = true;
                        break;
                    case "DtoPrincipalComertial": //SI EL ROL ES DIRECTOR COMERCIAL
                        //Entitiesproperties(dtoMaster.DtoPrincipalComertial);                        
                        //IsValidate = true;
                        break;
                    case "DtoTechnicalAssistant": //SI EL ROL ES ASISTENTE TECNICO
                        //Entitiesproperties(dtoMaster.DtoTechnicalAssistant);
                        //IsValidate = true;
                        break;
                    case "DtoEmployee": //SI EL ROL ES EMPLEADO
                        Entitiesproperties(dtoMaster.DtoEmployee);
                        IsValidate = true;
                        break;
                    case "DtoAgent": //SI EL ROL ES INTERMEDIARIO/AGENTE                          
                        Entitiesproperties(dtoMaster.DtoAgent);
                        IsValidate = true;
                        break;
                    case "DtoProvider": //SI EL ROL ES PROVEEDOR
                        Entitiesproperties(dtoMaster.DtoProvider);
                        IsValidate = true;
                        break;
                    case "DtoThird": //SI EL ROL ES TERCERO
                        Entitiesproperties(dtoMaster.DtoThird);
                        IsValidate = true;
                        break;
                    case "DtoUser": //SI EL ROL ES USUARIO
                        Entitiesproperties(dtoMaster.DtoUser);
                        IsValidate = true;
                        break;
                }

                if (IsValidate) break;
            }
        }

        private static void Entitiesproperties<T>(T Entity)
        {
            string FieldName = "";
            foreach (var propertyInfo in Entity.GetType().GetProperties())
            {
                FieldName = propertyInfo.Name;
                if (FieldName == "Mpersona" || FieldName == "dtoDataInsured" || FieldName == "List_Logbook")
                {
                    if (FieldName.Contains("list"))
                    {
                        IList EntSub = propertyInfo.GetValue(Entity, null) as IList;
                        foreach (object obj in EntSub)
                            ForeachEntity(obj);

                    }
                    else
                    {
                        object EntSub = propertyInfo.GetValue(Entity, null);
                        ForeachEntity(EntSub);
                    }

                }
            }
        }

        private static void ForeachEntity(object Entity)
        {
            object FieldValue;
            string FieldName;
            //Table TableEsquem.
            foreach (var pop in Entity.GetType().GetProperties())
            {
                //aqui se valida 
                FieldName = pop.Name;
                FieldValue = Entity.GetPropertyValue(pop.Name);
                //linq Dataset por el fullName Entity
                //1 validacion validar que exista el campo en tabla
                //2 validar el tipo del campo
                //3 validar el valor del campo
                //4 validar llaver foraneas

                if (FieldValue != null)
                {
                    if (!FieldValue.GetType().ToString().Contains("System"))
                    {
                        object EntSub = pop.GetValue(Entity, null);
                        ForeachEntity(EntSub);
                    }
                }
            }
        }

        public static void Update<T>(this IEnumerable<T> enumerable, Action<T> callback)
        {
            if (enumerable != null)
            {
                IterateHelper(enumerable, (x, i) => callback(x));
            }
        }

        public static void Update<T>(this IEnumerable<T> enumerable, Action<T, int> callback)
        {
            if (enumerable != null)
            {
                IterateHelper(enumerable, callback);
            }
        }

        private static void IterateHelper<T>(this IEnumerable<T> enumerable, Action<T, int> callback)
        {
            int count = 0;
            foreach (var cur in enumerable)
            {
                callback(cur, count);
                count++;
            }
        }

    }
}