using Sistran.Company.Application.UploadFileServices.Models;
using Sistran.Core.Application.UploadFileService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Et = Sistran.Company.Application.UploadFileServices.EEProvider.Entities;

namespace Sistran.Company.Services.UploadFileServices.EEProvider.DAOs
{
    public class MassiveDAO
    {

        /// <summary>
        /// Guardar proceso asincrono.
        /// </summary>
        /// <param name="asynchronousProcess">Proceso Asincrono</param>
        /// <returns></returns>
        public int SaveAsynchronousProcess(int userId, int idMassive)
        {
            try
            {
                AsynchronousProcess asynchronousProcess = new AsynchronousProcess();
                asynchronousProcess.UserId = userId;
                asynchronousProcess.Description = "Cargue masivo: " + idMassive;
                asynchronousProcess.Status = Convert.ToBoolean(1);
                asynchronousProcess.BeginDate = DateTime.Today;
                asynchronousProcess.EndDate = DateTime.Today;


              
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }


        public void SaveMassiveCollectiveRelation(int tempId, int idMassive, int processId, int userId, int statusId)
        {
            Et.MassiveCollectiveRelation massiveCollectiveRelation = new Et.MassiveCollectiveRelation(tempId, idMassive, processId);
            massiveCollectiveRelation.IsEvent = false;
            massiveCollectiveRelation.StateId = statusId;
            massiveCollectiveRelation.UserId = userId;

            DataFacadeManager.Instance.GetDataFacade().InsertObject(massiveCollectiveRelation);

        }


        /// <summary>
        /// Daniel Romero  - dromero - 08/04/2015
        /// Obtiene un objeto del tipo Models.MassiveLoadFieldSet filtrado por el ID
        /// </summary>
        /// <param name="fieldSetId">El id de la tabla.</param>
        /// <returns>Lista de objetos del tipo Models.MassiveLoadFieldSet</returns>
        public MassiveLoadFieldSet GetMassiveLoadFieldSetByFieldSetIdPrefixCd(int fieldSetId, int prefixCd)
        {
            //Filtro
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Et.MassiveLoadFieldSet.Properties.IsCollective);
            filter.Equal();
            filter.Constant(true);

            if (fieldSetId != 0)
            {
                filter.And();
                filter.Property(Et.MassiveLoadFieldSet.Properties.FieldSetId);
                filter.Equal();
                filter.Constant(fieldSetId);
            }


            if (prefixCd != 0)
            {
                filter.And();
                filter.Property(Et.MassiveLoadFieldSet.Properties.PrefixCode);
                filter.Equal();
                filter.Constant(prefixCd);
            }
            //Obtenemos el BusinessCollection de Entities.MassiveLoadFieldSet
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Et.MassiveLoadFieldSet), filter.GetPredicate()));

            //retornar como objeto
            return ModelAssembler.MassiveLoadFieldSetById(businessCollection);
        }


        /// <summary>
        /// Obtiene una lista de objetos del tipo Models.MassiveLoadFields
        /// </summary>
        /// <returns>Lista de objetos del tipo Models.MassiveLoadFields</returns>
        public List<MassiveLoadFields> GetMassiveLoadFields()
        {
            //Obtenemos el BusinessCollection de Entities.MassiveLoadFields
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Et.MassiveLoadFields)));

            //retornar como Lista
            return ModelAssembler.MassiveLoadFields(businessCollection);
        }


        /// <summary>
        /// Gina Gomez - 22/06/2015
        ///  Obtiene una lista con el nombre de las columnas del archivo de excel.
        /// </summary>
        /// <param name="fieldSetId">Codigo del FieldSetId.</param>
        /// <returns>Listado de Nombres</returns>
        public List<MassiveLoadFields> GetMassiveLoadFieldsByFieldSetId(int fieldSetId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(Et.MassiveLoadFields.Properties.MassiveFieldName, "MLF"), "MassiveFieldName"));
            select.AddSelectValue(new SelectValue(new Column(Et.MassiveLoadFields.Properties.MassiveFieldDescription, "MLF"), "MassiveFieldDescription"));
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
            filter.Constant(fieldSetId);
            select.Where = filter.GetPredicate();

            select.AddSortValue(new SortValue(new Column(Et.MassiveLoadFieldsMap.Properties.FieldOrder), SortOrderType.Ascending));

            List<MassiveLoadFields> massiveLoadFields = new List<MassiveLoadFields>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                massiveLoadFields = (List<MassiveLoadFields>)CollectionHelper.ConvertDAFTo<MassiveLoadFields>(reader);

            }

            return massiveLoadFields;
        }

        /// <summary>
        /// Gets the risk exclude count by massive identifier.
        /// </summary>
        /// <param name="massiveLoadId">The massive load identifier.</param>
        /// <returns></returns>
        public int GetRiskExcludeCountByMassiveId(int massiveLoadId, string licensePlate, int tempId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Et.RiskExclude.Properties.TempId, typeof(Et.RiskExclude).Name);
            filter.Equal();
            filter.Constant(tempId);

            if (massiveLoadId != 0)
            {
                filter.And();
                filter.Property(Et.RiskExclude.Properties.MassiveId, typeof(Et.RiskExclude).Name);
                filter.Distinct();
                filter.Constant(massiveLoadId);
            }

            if (licensePlate != "")
            {
                filter.And();
                filter.Property(Et.RiskExclude.Properties.LicensePlate, typeof(Et.RiskExclude).Name);
                filter.Equal();
                filter.Constant(licensePlate);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Et.RiskExclude), filter.GetPredicate()));

            return businessCollection.Count();
        }

    }
}
