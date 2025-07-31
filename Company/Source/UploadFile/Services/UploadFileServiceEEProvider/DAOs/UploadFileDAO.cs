using Sistran.Co.Application.Data;
using Sistran.Company.Application.UploadFileServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using Et = Sistran.Company.Application.UploadFileServices.EEProvider.Entities;

namespace Sistran.Company.Services.CollectiveUnderwritingServices.EEProvider.Providers
{
    public class UploadFileDAO
    {
        /// <summary>
        /// valida el cargue masivo.
        /// </summary>
        /// <param name="massiveId">Identificador de cargue masivo</param>
        /// <param name="fielSetId">Identificador tipo de cargue masivo</param>
        public void ValidateMassiveLoadByMassiveId(int massiveId, int fielSetId)
        {
            NameValue[] spParams = new NameValue[1];
            spParams[0] = new NameValue("MASSIVEID", massiveId);
            DynamicDataAccess pdb = new DynamicDataAccess();
            pdb.ExecuteSPNonQuery("QUO.VALIDATE_MASSIVE_LOAD", spParams);
        }

        /// <summary>
        /// Actualiza el proceso asincrono.
        /// </summary>
        /// <param name="processId">Identificador de proceso</param>
        /// <param name="errorMessage">Mensaje de error</param>
        public void UpdateAsynchronousProcessByProcessId(int processId, string errorMessage)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("MASSIVE_LOAD_ID", processId);
            parameters[1] = new NameValue("ERROR_MESS", errorMessage);
            DynamicDataAccess pdb = new DynamicDataAccess();
            pdb.ExecuteSPNonQuery("COMM.UPDATE_ASSINCRONOUS_MAS", parameters);
        }

        /// <summary>
        /// Inserta el cargue masivo
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="massiveLoadId">Identificador de cargue masivo</param>
        /// <param name="fieldSet">fieldSet.</param>
        /// <param name="values">Valores</param>
        /// <param name="tempId">Identificador temporario</param>
        public void InsertMassiveLoad(int userId, int massiveLoadId, int fieldSet, string values, int tempId)
        {
            try
            {
                NameValue[] parameters = new NameValue[6];
                parameters[0] = new NameValue("USER_ID", userId);
                parameters[1] = new NameValue("MASSIVE_LOAD_ID", massiveLoadId);
                parameters[2] = new NameValue("FIELD_SET", fieldSet);
                parameters[3] = new NameValue("VALUES", values);
                parameters[4] = new NameValue("MASSIVE_LOAD_TYPE_CD", 1);
                parameters[5] = new NameValue("TEMP_ID", tempId);
                DynamicDataAccess pdb = new DynamicDataAccess();
                pdb.ExecuteSPNonQuery("QUO.EXEC_MASSIVE_LOAD", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Encuentra el cargue masivo
        /// </summary>
        /// <param name="massiveId">Identificador de cargue masivo</param>
        /// <returns></returns>
        public int FindMassiveLoad(int massiveId)
        {
            int intRespuesta = 0;
            NameValue[] spParams = new NameValue[1];
            spParams[0] = new NameValue("IDCARGUE", massiveId);
            DynamicDataAccess pdb = new DynamicDataAccess();
            intRespuesta = (int)pdb.ExecuteScalar("select  count(MASSIVE_LOAD_ID) from QUO.MASSIVE_LOAD where MASSIVE_LOAD_ID=@IDCARGUE", spParams);

            return intRespuesta;
        }

        /// <summary>
        ///  Obtiene una lista con el nombre de las columnas del archivo de excel.
        /// </summary>
        /// <param name="fieldSet">Codigo del FieldSetId.</param>
        /// <returns>Listado de Nombres</returns>
        public List<MassiveLoadFields> GetMassiveLoadFieldsNameByFieldSet(int fieldSet)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(Et.MassiveLoadFields.Properties.MassiveFieldName, "MLF"), "MassiveFieldName"));

            Join join = new Join(new ClassNameTable(typeof(Et.MassiveLoadFieldsMap), "MLM"), new ClassNameTable(typeof(Et.MassiveLoadFields), "MLF"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder())
                            .Property(Et.MassiveLoadFields.Properties.MassiveFieldId, "MLF")
                            .Equal()
                            .Property(Et.MassiveLoadFieldsMap.Properties.MassiveFieldId, "MLM")
                            .GetPredicate();
            select.Table = join;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Et.MassiveLoadFieldsMap.Properties.FieldSetId, "MLM");
            filter.Equal();
            filter.Constant(fieldSet);
            select.Where = filter.GetPredicate();

            select.AddSortValue(new SortValue(new Column(Et.MassiveLoadFieldsMap.Properties.FieldOrder), SortOrderType.Ascending));

            List<MassiveLoadFields> massiveLoadFields = new List<MassiveLoadFields>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                massiveLoadFields = (List<MassiveLoadFields>)CollectionHelper.ConvertDAFTo<MassiveLoadFields>(reader);

            }

            return massiveLoadFields;

        }
    }
}
