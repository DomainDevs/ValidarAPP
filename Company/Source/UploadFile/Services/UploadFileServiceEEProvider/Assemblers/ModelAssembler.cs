using Sistran.Company.Application.UploadFileServices.Models;
using System.Collections.Generic;
using Et = Sistran.Company.Application.UploadFileServices.EEProvider.Entities;

namespace Sistran.Core.Application.UploadFileService.Assemblers
{
    public class ModelAssembler
    {


        #region AsynchronousProcess

        #endregion
        #region AsynchronousProcess

        /// <summary>
        /// De acuerdo al BusinessCollection obtiene una lista de objetos del tipo Models.MassiveLoadFieldSetRelation
        /// </summary>
        /// <param name="businessCollection">La colección que contiene la lista de objetos.</param>
        /// <returns>Retorna un objeto del tipo Models.MassiveLoadFieldSetRelation</returns>
        public static AsynchronousProcess AsynchronousProcessById(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            AsynchronousProcess asynchronousProcess = new AsynchronousProcess();

            return asynchronousProcess;
        }
        public static List<AsynchronousProcess> CreateAsynchronousProcessLst(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<AsynchronousProcess> asynchronousProcessLst = new List<AsynchronousProcess>();

            return asynchronousProcessLst;
        }
        #endregion


        #region MassiveLoadFields

        /// <summary>
        ///  Convierte un objeto de tipo Entidad.MassiveLoadFields a Models.MassiveLoadFields
        /// </summary>
        /// <param name="massiveLoadFields">Objeto del tipo Entities.MassiveLoadFields</param>
        /// <returns>Retorna un objeto del tipo Models.MassiveLoadFields</returns>        
        public static MassiveLoadFields CreateMassiveLoadFields(Et.MassiveLoadFields massiveLoadFields)
        {
            return new MassiveLoadFields()
            {
                MassiveFieldId = massiveLoadFields.MassiveFieldId,
                MassiveFieldName = massiveLoadFields.MassiveFieldName,
                MassiveFieldDescription = massiveLoadFields.MassiveFieldDescription,
                FieldLong = massiveLoadFields.FieldLong
            };
        }

        /// <summary>
        /// De acuerdo al BusinessCollection obtiene una lista de objetos del tipo Models.MassiveLoadFields
        /// </summary>
        /// <param name="businessCollection">La colección que contiene la lista de objetos.</param>
        /// <returns>Retorna una lista de objetos del tipo Models.MassiveLoadFields</returns>
        /// <autor>Héctor Daniel Romero - dromero - Fecha: 30/03/2015</autor>
        public static List<MassiveLoadFields> MassiveLoadFields(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<MassiveLoadFields> ListMassiveLoadFields = new List<MassiveLoadFields>();

            foreach (Et.MassiveLoadFields FieldsEntity in businessCollection)
            {
                ListMassiveLoadFields.Add(ModelAssembler.CreateMassiveLoadFields(FieldsEntity));
            }

            return ListMassiveLoadFields;
        }

        #endregion

        #region MassiveLoadFieldSet

        /// <summary>
        /// Convierte un objeto de tipo Entidad.MassiveLoadFieldSet a Models.MassiveLoadFieldSet
        /// </summary>
        /// <param name="massiveLoadFieldSet">Objeto del tipo Entities.MassiveLoadFieldSet</param>
        /// <returns>Retorna un objeto del tipo Models.MassiveLoadFieldSet</returns>
        public static MassiveLoadFieldSet CreateMassiveLoadFieldSet(Et.MassiveLoadFieldSet massiveLoadFieldSet)
        {
            return new MassiveLoadFieldSet()
            {
                FieldSetId = massiveLoadFieldSet.FieldSetId,
                Description = massiveLoadFieldSet.Description,
                BeginColumn = massiveLoadFieldSet.BeginColumn,
                EndColumn = massiveLoadFieldSet.EndColumn,
                CountColumn = massiveLoadFieldSet.CountColumn,
                IsEnabled = massiveLoadFieldSet.IsEnabled,
                PrefixCode = massiveLoadFieldSet.PrefixCode,
                IsRequest = massiveLoadFieldSet.IsRequest,
                IsCollective = massiveLoadFieldSet.IsCollective,
                IsExclude = massiveLoadFieldSet.IsExclude
            };
        }

        /// <summary>
        /// De acuerdo al BusinessCollection obtiene una lista de objetos del tipo Models.MassiveLoadFieldSet
        /// </summary>
        /// <param name="businessCollection">La colección que contiene la lista de objetos.</param>
        /// <returns>Retorna una lista de objetos del tipo Models.MassiveLoadFieldSet</returns>
        public static List<MassiveLoadFieldSet> MassiveLoadFieldSets(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<MassiveLoadFieldSet> ListMassiveLoadFieldSet = new List<MassiveLoadFieldSet>();

            foreach (Et.MassiveLoadFieldSet FieldsEntity in businessCollection)
            {
                ListMassiveLoadFieldSet.Add(ModelAssembler.CreateMassiveLoadFieldSet(FieldsEntity));
            }

            return ListMassiveLoadFieldSet;
        }

        /// <summary>
        /// De acuerdo al BusinessCollection obtiene un objeto del tipo Models.MassiveLoadFieldSet
        /// </summary>
        /// <param name="businessCollection">La colección que contiene la lista de objetos (1 solo objeto).</param>
        /// <returns>Retorna un objeto del tipo Models.MassiveLoadFieldSet</returns>
        public static MassiveLoadFieldSet MassiveLoadFieldSetById(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            MassiveLoadFieldSet ListMassiveLoadFieldSet = new MassiveLoadFieldSet();

            foreach (Et.MassiveLoadFieldSet FieldsEntity in businessCollection)
            {
                ListMassiveLoadFieldSet = ModelAssembler.CreateMassiveLoadFieldSet(FieldsEntity);
            }

            return ListMassiveLoadFieldSet;
        }

        #endregion
    }
}
